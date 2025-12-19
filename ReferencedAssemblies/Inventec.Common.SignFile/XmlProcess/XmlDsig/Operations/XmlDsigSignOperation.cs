using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Common;
using Inventec.Common.SignFile.XmlProcess.Common.Exceptions;
using Inventec.Common.SignFile.XmlProcess.Utils.Xml;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Operations.Signers;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig.Operations
{
    internal abstract class XmlDsigSignOperation
    {
        internal const string PropertiesId = "signatureProperties";

        internal static XmlDsigSignOperation From(XmlDsigSignParameters parameters)
        {
            switch (parameters.SignatureFormat)
            {
                case XmlDsigSignatureFormat.Enveloping:
                    return new XmlDsigEnvelopingSignOperation();
                case XmlDsigSignatureFormat.Enveloped:
                    return new XmlDsigEnvelopedSignOperation();
                case XmlDsigSignatureFormat.Detached:
                    return new XmlDsigDetachedSignOperation();
            }
            throw new Exception("There isn't a '" + parameters.SignatureFormat + "' signer implemented");
        }

        internal void Sign(XmlDsigSignParameters signParameters)
        {
            if (signParameters.OutputPath == null) throw new InvalidParameterException("Path of signed file cannot be null");
            Sign(signParameters, null);
        }
        internal void Sign(XmlDsigSignParameters signParameters, Action<ExtendedSignedXml> signedXmlPostProcessing)
        {
            var signedDocument = SignAndGetXml(signParameters, signedXmlPostProcessing);
            SaveSignatureToFile(signedDocument, signParameters);
        }
        internal XmlDocument SignAndGetXml(XmlDsigSignParameters signParameters)
        {
            return SignAndGetXml(signParameters, null);
        }
        internal XmlDocument SignAndGetXml(XmlDsigSignParameters signParameters, Action<ExtendedSignedXml> signedXmlPostProcessing)
        {
            ValidateParameters(signParameters);

            var inputXml = signParameters.InputXml;
            if (inputXml == null)
            {
                inputXml = new XmlDocument();
                inputXml.Load(signParameters.InputPath);
                signParameters.InputXml = inputXml;
            }

            XmlDocument xmlDocument = null;
            if (!String.IsNullOrEmpty(signParameters.DataServerHsmResponse))
            {
                xmlDocument = BuildFinalSignedXmlDocument(inputXml, signParameters.DataServerHsmResponse);
            }
            else
            {
                var signature = GetSignature(inputXml, signParameters, signedXmlPostProcessing);
                xmlDocument = BuildFinalSignedXmlDocument(inputXml, signature.GetXml());
            }

            return xmlDocument;
        }

        protected void SaveSignatureToFile(XmlDocument xml, XmlDsigSignParameters signParameters)
        {
            xml.Save(signParameters.OutputPath);
        }

        #region Métodos de implementación de los pasos de firma

        private static void ValidateParameters(XmlDsigSignParameters signParameters)
        {
            if (signParameters == null) throw new InvalidParameterException("Parameters to sign cannot be null");
            if (signParameters.SignatureCertificate == null) throw new InvalidParameterException("Signer Certificate cannot be null");
            if (signParameters.InputPath == null) throw new InvalidParameterException("Document to sign cannot be null");
        }
        private ExtendedSignedXml GetSignature(XmlDocument inputXml, XmlDsigSignParameters signParameters, Action<ExtendedSignedXml> signedXmlPostProcessing)
        {
            if (inputXml.DocumentElement == null) throw new InvalidDocumentException("Document to sign has no root element");
            var certificate = signParameters.SignatureCertificate;
            var inputPath = signParameters.InputPath;

            var signedXml = new ExtendedSignedXml(inputXml);
            signedXml.Signature.Id = "signature";

            CreateAndAddReferenceTo(signedXml, inputXml, inputPath, signParameters.XPathNodeToSign);
            CreateTimestampNodeIfNeeded(signedXml, signParameters);
            CreateNodesForProperties(signedXml, signParameters);
            IncludeSignatureCertificateIfNeeded(signedXml, certificate, signParameters);
            AddCanonicalizationMethodTo(signedXml);
            if (signedXmlPostProcessing != null) signedXmlPostProcessing(signedXml);
            signedXml.ComputeSignature();

            return signedXml;
        }
        private static void CreateNodesForProperties(ExtendedSignedXml signedXml, XmlDsigSignParameters signParameters)
        {
            if (signParameters.Properties != null && signParameters.Properties.Count > 0)
            {
                foreach (var xmlPropertyDescriptor in signParameters.Properties)
                {
                    AddPropertyFromNameAndValue(xmlPropertyDescriptor.Name, xmlPropertyDescriptor.Value,
                                                xmlPropertyDescriptor.NameSpace,
                                                signedXml, signParameters);
                }
            }
            if (signParameters.PropertyBuilders != null && signParameters.PropertyBuilders.Count > 0)
            {
                foreach (var propertyBuilder in signParameters.PropertyBuilders)
                {
                    AddProperty(signParameters.InputXml, signedXml, propertyBuilder(signParameters.InputXml));
                }
            }
        }
        private static void CreateTimestampNodeIfNeeded(ExtendedSignedXml signedXml, XmlDsigSignParameters signParameters)
        {
            if (!signParameters.IncludeTimestamp) return;

            const string propertyName = "Timestamp";
            const string propertyNameSpace = ExtendedSignedXml.XmlDSigTimestampNamespace;
            var propertyValue = XmlHelper.NowInCanonicalRepresentation();

            AddPropertyFromNameAndValue(propertyName, propertyValue, propertyNameSpace, signedXml, signParameters);
        }
        private static void AddPropertyFromNameAndValue(string propertyName, string propertyValue, string propertyNameSpace,
            ExtendedSignedXml signedXml, XmlDsigSignParameters signParameters)
        {
            var document = signParameters.InputXml;
            if (document == null) throw new InvalidParameterException("Document cannot be null");

            var propertyNode = string.IsNullOrEmpty(propertyNameSpace) ?
                document.CreateElement(propertyName) : document.CreateElement(propertyName, propertyNameSpace);
            propertyNode.InnerText = propertyValue;
            AddProperty(document, signedXml, propertyNode);
        }
        private static void AddProperty(XmlDocument document, ExtendedSignedXml signedXml, XmlElement propertyNode)
        {
            if (signedXml.PropertiesNode == null)
            {
                signedXml.PropertiesNode = CreatePropertiesNode(document, signedXml);
            }

            var nodeSignatureProperty = document.CreateElement("SignatureProperty", SignedXml.XmlDsigNamespaceUrl);
            nodeSignatureProperty.SetAttribute("Target", "#" + signedXml.Signature.Id);
            nodeSignatureProperty.AppendChild(propertyNode);

            signedXml.PropertiesNode.AppendChild(nodeSignatureProperty);
        }
        private static XmlElement CreatePropertiesNode(XmlDocument document, ExtendedSignedXml signedXml)
        {
            var dataObject = new DataObject();
            var nodeSignatureProperties = document.CreateElement("SignatureProperties", SignedXml.XmlDsigNamespaceUrl);
            nodeSignatureProperties.SetAttribute("Id", PropertiesId);
            dataObject.Data = nodeSignatureProperties.SelectNodes(".");
            signedXml.AddObject(dataObject);

            var referenceToProperties = new Reference
            {
                Uri = "#" + PropertiesId,
                Type = ExtendedSignedXml.XmlDsigSignatureProperties
            };
            signedXml.AddReference(referenceToProperties);
            return nodeSignatureProperties;
        }

        protected virtual void AddCanonicalizationMethodTo(SignedXml signedXml)
        {
        }
        protected void IncludeSignatureCertificateIfNeeded(SignedXml signedXml, X509Certificate2 certificate, XmlDsigSignParameters signParameters)
        {
            Inventec.Common.Logging.LogSystem.Info("IncludeSignatureCertificateIfNeeded.1");
            var certificateKeyInfo = new KeyInfo();
            if (!signParameters.IncludeCertificateInSignature)
            {
                Inventec.Common.Logging.LogSystem.Info("IncludeCertificateInSignature not has set");
                return;
            }
            Inventec.Common.Logging.LogSystem.Info("IncludeSignatureCertificateIfNeeded.2");
            if (certificate != null && certificate.PrivateKey != null)
            {
                Inventec.Common.Logging.LogSystem.Info("IncludeSignatureCertificateIfNeeded.3");
                signedXml.SigningKey = certificate.PrivateKey;

                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificate.PrivateKey;
                //if (!String.IsNullOrEmpty(signParameters.PinCode))
                //{
                //    Inventec.Common.Logging.LogSystem.Info("IncludeSignatureCertificateIfNeeded.4");
                //    Inventec.Common.Logging.LogSystem.Debug("pinCode has set");

                //    try
                //    {
                //        //string preSerialNumber = KeyStore.GetValue(KeyStore.SERIALNUMBER_KEY);
                //        //if (!String.IsNullOrEmpty(preSerialNumber) && preSerialNumber != certificate.SerialNumber)
                //        //{
                //        //    KeyStore.SetValue(KeyStore.CHANGE_USB_KEY, "1");
                //        //    signParameters.PinCode = "";
                //        //    Inventec.Common.Logging.LogSystem.Info("check preSerialNumber => 1");
                //        //}
                //        //else
                //        //{
                //        //    KeyStore.SetValue(KeyStore.CHANGE_USB_KEY, "0");
                //        //    Inventec.Common.Logging.LogSystem.Info("check preSerialNumber => 2");
                //        //}
                //        //Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => preSerialNumber), preSerialNumber) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => certificate.SerialNumber), certificate.SerialNumber));
                //        CspParameters cspp = new CspParameters();
                //        cspp.KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName;
                //        cspp.ProviderName = rsa.CspKeyContainerInfo.ProviderName;
                //        cspp.ProviderType = rsa.CspKeyContainerInfo.ProviderType;
                //        cspp.KeyPassword = SharedUtils.GetSecurePin(signParameters.PinCode);

                //        Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("cspp", cspp));

                //        //the pin code will be cached for next access to the smart card
                //        RSACryptoServiceProvider rsaprovider = new RSACryptoServiceProvider(cspp);
                //        //KeyStore.SetValue(KeyStore.SERIALNUMBER_KEY, certificate.SerialNumber);
                //        RSAKeyValue rkv = new RSAKeyValue(rsaprovider);
                //        certificateKeyInfo.AddClause(rkv);
                //        Inventec.Common.Logging.LogSystem.Info("IncludeSignatureCertificateIfNeeded.5");
                //    }
                //    catch (Exception ex)
                //    {
                //        Inventec.Common.Logging.LogSystem.Warn("PIN code Cached for next access to the smart card fail: " + ex.Message);
                //    }
                //}
                //else
                //{
                    RSACryptoServiceProvider rsaprovider = (RSACryptoServiceProvider)certificate.PublicKey.Key;
                    RSAKeyValue rkv = new RSAKeyValue(rsaprovider);
                    certificateKeyInfo.AddClause(rkv);
                    Inventec.Common.Logging.LogSystem.Info("IncludeSignatureCertificateIfNeeded.6");
                //}
            }

            certificateKeyInfo.AddClause(new KeyInfoX509Data(certificate));
            signedXml.KeyInfo = certificateKeyInfo;
            Inventec.Common.Logging.LogSystem.Info("IncludeSignatureCertificateIfNeeded.7");
        }
        protected virtual XmlDocument BuildFinalSignedXmlDocument(XmlDocument inputXml, XmlElement signatureXml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(signatureXml.OuterXml);
            return xmlDocument;
        }

        protected virtual XmlDocument BuildFinalSignedXmlDocument(XmlDocument inputXml, string signatureXml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(signatureXml);
            return xmlDocument;
        }

        #endregion

        #region Métodos a sobreescribir

        protected abstract void CreateAndAddReferenceTo(SignedXml signedXml, XmlDocument document, string inputPath, string xpathToNodeToSign);

        #endregion
    }
}