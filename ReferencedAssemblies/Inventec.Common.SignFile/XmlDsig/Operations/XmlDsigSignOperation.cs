using System;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Inventec.Common.Logging;
using Inventec.Common.SignFile.XmlProcess.Common;
using Inventec.Common.SignFile.XmlProcess.Common.Exceptions;
using Inventec.Common.SignFile.XmlProcess.Utils.Xml;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Operations.Signers;

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
			default:
				throw new Exception("There isn't a '" + parameters.SignatureFormat.ToString() + "' signer implemented");
			}
		}

		internal void Sign(XmlDsigSignParameters signParameters)
		{
			if (signParameters.OutputPath == null)
			{
				throw new InvalidParameterException("Path of signed file cannot be null");
			}
			Sign(signParameters, null);
		}

		internal void Sign(XmlDsigSignParameters signParameters, Action<ExtendedSignedXml> signedXmlPostProcessing)
		{
			XmlDocument xml = SignAndGetXml(signParameters, signedXmlPostProcessing);
			SaveSignatureToFile(xml, signParameters);
		}

		internal XmlDocument SignAndGetXml(XmlDsigSignParameters signParameters)
		{
			return SignAndGetXml(signParameters, null);
		}

		internal XmlDocument SignAndGetXml(XmlDsigSignParameters signParameters, Action<ExtendedSignedXml> signedXmlPostProcessing)
		{
			LogSystem.Info("SignAndGetXml.1");
			ValidateParameters(signParameters);
			XmlDocument xmlDocument = signParameters.InputXml;
			if (xmlDocument == null)
			{
				xmlDocument = new XmlDocument();
				xmlDocument.Load(signParameters.InputPath);
				signParameters.InputXml = xmlDocument;
			}
			LogSystem.Info("SignAndGetXml.2");
			XmlDocument xmlDocument2 = null;
			if (signParameters.DlgGetHSMServerResponseData != null)
			{
				LogSystem.Info("SignAndGetXml.3");
				string dataServerHsmResponse = "";
				string errMessage = "";
				File.ReadAllBytes(signParameters.InputPath);
				string hashData = Convert.ToBase64String(File.ReadAllBytes(signParameters.InputPath));
				dataServerHsmResponse = signParameters.DlgGetHSMServerResponseData(hashData, ref errMessage);
				SharedUtils.ByteToFile(Convert.FromBase64String(dataServerHsmResponse), signParameters.OutputPath);
				LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => dataServerHsmResponse)), (object)dataServerHsmResponse));
				xmlDocument2 = new XmlDocument();
				xmlDocument2.Load(signParameters.OutputPath);
				LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => signParameters.OutputPath)), (object)signParameters.OutputPath));
				if (!string.IsNullOrEmpty(signParameters.XPathNodeToSign))
				{
					XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument2.NameTable);
					xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
					XmlElement xmlElement = xmlDocument2.SelectSingleNode("//ds:Signature", xmlNamespaceManager) as XmlElement;
					if (xmlElement == null)
					{
						LogSystem.Info("Không tìm thấy node Signature trong xmlDocument");
					}
					XmlElement xmlElement2 = signParameters.InputXml.GetElementsByTagName(signParameters.XPathNodeToSign)[0] as XmlElement;
					if (xmlElement2 == null)
					{
						LogSystem.Info("Không tìm thấy node CHUKYDONVI trong InputXml");
					}
					XmlElement newChild = signParameters.InputXml.ImportNode(xmlElement, true) as XmlElement;
					xmlElement2.AppendChild(newChild);
					signParameters.InputXml.Save(signParameters.OutputPath);
				}
				xmlDocument2 = new XmlDocument();
				xmlDocument2.Load(signParameters.OutputPath);
				signParameters.InputXml = xmlDocument2;
				LogSystem.Info(LogUtil.TraceData("xmlDocument.InnerXml", (object)xmlDocument2.InnerXml));
				LogSystem.Info("SignAndGetXml.4");
			}
			else
			{
				LogSystem.Info("SignAndGetXml.5");
				ExtendedSignedXml signature = GetSignature(xmlDocument, signParameters, signedXmlPostProcessing);
				xmlDocument2 = BuildFinalSignedXmlDocument(xmlDocument, signature.GetXml());
				LogSystem.Info("SignAndGetXml.6");
			}
			return xmlDocument2;
		}

		protected void SaveSignatureToFile(XmlDocument xml, XmlDsigSignParameters signParameters)
		{
			xml.Save(signParameters.OutputPath);
		}

		private static void ValidateParameters(XmlDsigSignParameters signParameters)
		{
			if (signParameters == null)
			{
				throw new InvalidParameterException("Parameters to sign cannot be null");
			}
			if (signParameters.SignatureCertificate == null)
			{
				throw new InvalidParameterException("Signer Certificate cannot be null");
			}
			if (signParameters.InputPath == null)
			{
				throw new InvalidParameterException("Document to sign cannot be null");
			}
		}

		private ExtendedSignedXml GetSignature(XmlDocument inputXml, XmlDsigSignParameters signParameters, Action<ExtendedSignedXml> signedXmlPostProcessing)
		{
			if (inputXml.DocumentElement == null)
			{
				throw new InvalidDocumentException("Document to sign has no root element");
			}
			X509Certificate2 signatureCertificate = signParameters.SignatureCertificate;
			string inputPath = signParameters.InputPath;
			ExtendedSignedXml extendedSignedXml = new ExtendedSignedXml(inputXml);
			extendedSignedXml.Signature.Id = "signature";
			CreateAndAddReferenceTo(extendedSignedXml, inputXml, inputPath, signParameters.XPathNodeToSign);
			CreateTimestampNodeIfNeeded(extendedSignedXml, signParameters);
			CreateNodesForProperties(extendedSignedXml, signParameters);
			IncludeSignatureCertificateIfNeeded(extendedSignedXml, signatureCertificate, signParameters);
			AddCanonicalizationMethodTo(extendedSignedXml);
			if (signedXmlPostProcessing != null)
			{
				signedXmlPostProcessing(extendedSignedXml);
			}
			extendedSignedXml.ComputeSignature();
			return extendedSignedXml;
		}

		private static void CreateNodesForProperties(ExtendedSignedXml signedXml, XmlDsigSignParameters signParameters)
		{
			if (signParameters.Properties != null && signParameters.Properties.Count > 0)
			{
				foreach (XmlPropertyDescriptor property in signParameters.Properties)
				{
					AddPropertyFromNameAndValue(property.Name, property.Value, property.NameSpace, signedXml, signParameters);
				}
			}
			if (signParameters.PropertyBuilders == null || signParameters.PropertyBuilders.Count <= 0)
			{
				return;
			}
			foreach (Converter<XmlDocument, XmlElement> propertyBuilder in signParameters.PropertyBuilders)
			{
				AddProperty(signParameters.InputXml, signedXml, propertyBuilder(signParameters.InputXml));
			}
		}

		private static void CreateTimestampNodeIfNeeded(ExtendedSignedXml signedXml, XmlDsigSignParameters signParameters)
		{
			if (signParameters.IncludeTimestamp)
			{
				string propertyValue = XmlHelper.NowInCanonicalRepresentation();
				AddPropertyFromNameAndValue("Timestamp", propertyValue, "http://xadesnet.codeplex.com/#timestamp", signedXml, signParameters);
			}
		}

		private static void AddPropertyFromNameAndValue(string propertyName, string propertyValue, string propertyNameSpace, ExtendedSignedXml signedXml, XmlDsigSignParameters signParameters)
		{
			XmlDocument inputXml = signParameters.InputXml;
			if (inputXml == null)
			{
				throw new InvalidParameterException("Document cannot be null");
			}
			XmlElement xmlElement = (string.IsNullOrEmpty(propertyNameSpace) ? inputXml.CreateElement(propertyName) : inputXml.CreateElement(propertyName, propertyNameSpace));
			xmlElement.InnerText = propertyValue;
			AddProperty(inputXml, signedXml, xmlElement);
		}

		private static void AddProperty(XmlDocument document, ExtendedSignedXml signedXml, XmlElement propertyNode)
		{
			if (signedXml.PropertiesNode == null)
			{
				signedXml.PropertiesNode = CreatePropertiesNode(document, signedXml);
			}
			XmlElement xmlElement = document.CreateElement("SignatureProperty", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement.SetAttribute("Target", "#" + signedXml.Signature.Id);
			xmlElement.AppendChild(propertyNode);
			signedXml.PropertiesNode.AppendChild(xmlElement);
		}

		private static XmlElement CreatePropertiesNode(XmlDocument document, ExtendedSignedXml signedXml)
		{
			DataObject dataObject = new DataObject();
			XmlElement xmlElement = document.CreateElement("SignatureProperties", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement.SetAttribute("Id", "signatureProperties");
			dataObject.Data = xmlElement.SelectNodes(".");
			signedXml.AddObject(dataObject);
			Reference reference = new Reference
			{
				Uri = "#signatureProperties",
				Type = "http://www.w3.org/2000/09/xmldsig#SignatureProperties"
			};
			signedXml.AddReference(reference);
			return xmlElement;
		}

		protected virtual void AddCanonicalizationMethodTo(SignedXml signedXml)
		{
		}

		protected void IncludeSignatureCertificateIfNeeded(SignedXml signedXml, X509Certificate2 certificate, XmlDsigSignParameters signParameters)
		{
			LogSystem.Info("IncludeSignatureCertificateIfNeeded.1");
			KeyInfo keyInfo = new KeyInfo();
			if (!signParameters.IncludeCertificateInSignature)
			{
				LogSystem.Info("IncludeCertificateInSignature not has set");
				return;
			}
			LogSystem.Info("IncludeSignatureCertificateIfNeeded.2");
			if (certificate != null && certificate.PrivateKey != null)
			{
				LogSystem.Info("IncludeSignatureCertificateIfNeeded.3");
				signedXml.SigningKey = certificate.PrivateKey;
				RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)certificate.PrivateKey;
				RSACryptoServiceProvider key = (RSACryptoServiceProvider)certificate.PublicKey.Key;
				RSAKeyValue clause = new RSAKeyValue(key);
				keyInfo.AddClause(clause);
				LogSystem.Info("IncludeSignatureCertificateIfNeeded.6");
			}
			keyInfo.AddClause(new KeyInfoX509Data(certificate));
			signedXml.KeyInfo = keyInfo;
			LogSystem.Info("IncludeSignatureCertificateIfNeeded.7");
		}

		protected virtual XmlDocument BuildFinalSignedXmlDocument(XmlDocument inputXml, XmlElement signatureXml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(signatureXml.OuterXml);
			return xmlDocument;
		}

		protected virtual XmlDocument BuildFinalSignedXmlDocument(XmlDocument inputXml, string signatureXml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(signatureXml);
			return xmlDocument;
		}

		protected virtual XmlDocument BuildFinalSignedXmlElement(XmlDocument inputXml, string signatureXml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(signatureXml);
			return xmlDocument;
		}

		protected abstract void CreateAndAddReferenceTo(SignedXml signedXml, XmlDocument document, string inputPath, string xpathToNodeToSign);
	}
}
