using System.Security.Cryptography.Xml;
using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Common;
using Inventec.Common.SignFile.XmlProcess.Utils.Cryptography;
using Inventec.Common.SignFile.XmlProcess.Utils.Xml;
using Inventec.Common.SignFile.XmlProcess.Xades.Common;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Operations;

namespace Inventec.Common.SignFile.XmlProcess.Xades.Operations
{
	public abstract class XadesSignOperation
	{
		public const string XadesNamespaceUrl = "http://uri.etsi.org/01903/v1.3.2#";

		public static void SignToFile(XadesSignParameters parameters)
		{
			XmlDsigSignParameters xmlDSigParameters = CreateXmlDSigParametersFrom(parameters);
			XmlDsigSignOperation.From(xmlDSigParameters).Sign(xmlDSigParameters, delegate(ExtendedSignedXml signedXml)
			{
				AddXAdESNodes(signedXml, xmlDSigParameters);
			});
		}

		public static XmlDocument SignAndGetXml(XadesSignParameters parameters)
		{
			XmlDsigSignParameters xmlDSigParameters = CreateXmlDSigParametersFrom(parameters);
			return XmlDsigSignOperation.From(xmlDSigParameters).SignAndGetXml(xmlDSigParameters, delegate(ExtendedSignedXml signedXml)
			{
				AddXAdESNodes(signedXml, xmlDSigParameters);
			});
		}

		private static void AddXAdESNodes(ExtendedSignedXml signedXml, XmlDsigSignParameters parameters)
		{
			XmlDocument inputXml = parameters.InputXml;
			XmlElement qualifyingPropertiesNode = AddQualifyingPropertiesNode(signedXml, inputXml);
			XmlElement xmlElement = AddSignedPropertiesNode(inputXml, qualifyingPropertiesNode);
			CreateReferenceToSignedProperties(signedXml, xmlElement);
			XmlElement xmlElement2 = AddSignedSignaturePropertiesNode(inputXml, xmlElement);
			AddSigningTimeNode(inputXml, xmlElement2);
			AddSigningCertificate(inputXml, xmlElement2, parameters);
			XmlElement unsignedPropertiesNode = AddUnsignedPropertiesNode(inputXml, qualifyingPropertiesNode);
			AddUnsignedSignaturePropertiesNode(inputXml, unsignedPropertiesNode);
		}

		private static void CreateReferenceToSignedProperties(ExtendedSignedXml signedXml, XmlElement signedPropertiesNode)
		{
			Reference reference = new Reference("#" + signedPropertiesNode.GetAttribute("Id"))
			{
				Type = "http://www.w3.org/2000/09/xmldsig#SignatureProperties"
			};
			signedXml.AddReference(reference);
		}

		private static XmlElement AddQualifyingPropertiesNode(ExtendedSignedXml signedXml, XmlDocument document)
		{
			DataObject dataObject = new DataObject();
			XmlElement xmlElement = document.CreateElement("QualifyingProperties", "http://uri.etsi.org/01903/v1.3.2#");
			xmlElement.SetAttribute("Target", signedXml.Signature.Id);
			dataObject.Data = xmlElement.SelectNodes(".");
			signedXml.AddObject(dataObject);
			return xmlElement;
		}

		private static XmlElement AddSignedPropertiesNode(XmlDocument document, XmlElement qualifyingPropertiesNode)
		{
			XmlElement xmlElement = XmlHelper.CreateNodeIn(document, "SignedProperties", "http://uri.etsi.org/01903/v1.3.2#", qualifyingPropertiesNode);
			xmlElement.SetAttribute("Id", "xadesSignedProperties");
			return xmlElement;
		}

		private static XmlElement AddSignedSignaturePropertiesNode(XmlDocument document, XmlElement propertiesNode)
		{
			return XmlHelper.CreateNodeIn(document, "SignedSignatureProperties", "http://uri.etsi.org/01903/v1.3.2#", propertiesNode);
		}

		private static void AddSigningTimeNode(XmlDocument document, XmlElement signedSignaturePropertiesNode)
		{
			XmlHelper.CreateNodeWithTextIn(document, "SigningTime", XmlHelper.NowInCanonicalRepresentation(), "http://uri.etsi.org/01903/v1.3.2#", signedSignaturePropertiesNode);
		}

		private static void AddSigningCertificate(XmlDocument document, XmlElement signedSignatureProperties, XmlDsigSignParameters parameters)
		{
			XmlElement rootNode = XmlHelper.CreateNodeIn(document, "SigningCertificate", "http://uri.etsi.org/01903/v1.3.2#", signedSignatureProperties);
			XmlElement certNode = XmlHelper.CreateNodeIn(document, "Cert", "http://uri.etsi.org/01903/v1.3.2#", rootNode);
			AddCertDigestNode(document, certNode, parameters);
			AddIssuerSerialNode(document, certNode, parameters);
		}

		private static void AddCertDigestNode(XmlDocument document, XmlElement certNode, XmlDsigSignParameters parameters)
		{
			XmlElement rootNode = XmlHelper.CreateNodeIn(document, "CertDigest", "http://uri.etsi.org/01903/v1.3.2#", certNode);
			XmlHelper.CreateNodeWithTextIn(document, "DigestMethod", "http://www.w3.org/2000/09/xmldsig#rsa-sha1", "http://www.w3.org/2000/09/xmldsig#", rootNode);
			byte[] rawData = parameters.SignatureCertificate.RawData;
			string base64SHA = CryptoHelper.GetBase64SHA1(rawData);
			XmlHelper.CreateNodeWithTextIn(document, "DigestValue", base64SHA, "http://www.w3.org/2000/09/xmldsig#", rootNode);
		}

		private static void AddIssuerSerialNode(XmlDocument document, XmlElement certNode, XmlDsigSignParameters parameters)
		{
			XmlElement rootNode = XmlHelper.CreateNodeIn(document, "IssuerSerial", "http://uri.etsi.org/01903/v1.3.2#", certNode);
			XmlHelper.CreateNodeWithTextIn(document, "X509IssuerName", parameters.SignatureCertificate.Issuer, "http://www.w3.org/2000/09/xmldsig#", rootNode);
			XmlHelper.CreateNodeWithTextIn(document, "X509SerialNumber", parameters.SignatureCertificate.SerialNumber, "http://www.w3.org/2000/09/xmldsig#", rootNode);
		}

		private static XmlElement AddUnsignedPropertiesNode(XmlDocument document, XmlElement qualifyingPropertiesNode)
		{
			return XmlHelper.CreateNodeIn(document, "UnsignedProperties", "http://uri.etsi.org/01903/v1.3.2#", qualifyingPropertiesNode);
		}

		private static XmlElement AddUnsignedSignaturePropertiesNode(XmlDocument document, XmlElement unsignedPropertiesNode)
		{
			return XmlHelper.CreateNodeIn(document, "UnsignedSignatureProperties", "http://uri.etsi.org/01903/v1.3.2#", unsignedPropertiesNode);
		}

		private static XmlDsigSignParameters CreateXmlDSigParametersFrom(XadesSignParameters xadesSignParameters)
		{
			return new XmlDsigSignParameters
			{
				IncludeCertificateInSignature = xadesSignParameters.IncludeCertificateInSignature,
				IncludeTimestamp = false,
				InputPath = xadesSignParameters.InputPath,
				InputXml = xadesSignParameters.InputXml,
				OutputPath = xadesSignParameters.OutputPath,
				Properties = xadesSignParameters.Properties,
				PropertyBuilders = xadesSignParameters.PropertyBuilders,
				SignatureCertificate = xadesSignParameters.SignatureCertificate,
				SignatureFormat = XmlDsigSignatureFormat.Enveloped
			};
		}
	}
}
