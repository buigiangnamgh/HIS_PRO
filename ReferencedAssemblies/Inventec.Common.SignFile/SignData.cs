using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Inventec.Common.Logging;

namespace Inventec.Common.SignFile
{
	public class SignData
	{
		public static bool SignXml130(XmlDocument xmlDoc, X509Certificate2 cer)
		{
			bool flag = false;
			try
			{
				if (xmlDoc == null)
				{
					throw new ArgumentException(null, "xmlDoc");
				}
				if (cer == null)
				{
					throw new ArgumentException(null, "cer");
				}
				XmlElement xmlElement = xmlDoc.GetElementsByTagName("CHUKYDONVI")[0] as XmlElement;
				if (xmlElement == null)
				{
					throw new Exception("Không tìm thấy phần tử 'CHUKYDONVI' trong tài liệu XML.");
				}
				SignedXml signedXml = new SignedXml(xmlDoc);
				RSA signingKey = (RSA)cer.PrivateKey;
				signedXml.SigningKey = signingKey;
				signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
				signedXml.Signature.Id = Guid.NewGuid().ToString();
				KeyInfo keyInfo = new KeyInfo();
				KeyInfoX509Data keyInfoX509Data = new KeyInfoX509Data(cer);
				keyInfoX509Data.AddSubjectName(cer.Subject);
				keyInfo.AddClause(keyInfoX509Data);
				signedXml.KeyInfo = keyInfo;
				Reference reference = new Reference();
				reference.Uri = "";
				reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
				XmlDsigEnvelopedSignatureTransform transform = new XmlDsigEnvelopedSignatureTransform();
				reference.AddTransform(transform);
				signedXml.AddReference(reference);
				signedXml.ComputeSignature();
				XmlElement xml = signedXml.GetXml();
				xmlElement.AppendChild(xmlDoc.ImportNode(xml, true));
				flag = true;
			}
			catch (Exception ex)
			{
				flag = false;
				LogSystem.Error(ex);
			}
			return flag;
		}

		public static bool SignXml130(XmlDocument xmlDoc, X509Certificate2 cer, string dataSign)
		{
			bool flag = false;
			try
			{
				if (xmlDoc == null)
				{
					throw new ArgumentException(null, "xmlDoc");
				}
				if (cer == null)
				{
					throw new ArgumentException(null, "cer");
				}
				XmlElement xmlElement = xmlDoc.GetElementsByTagName(dataSign)[0] as XmlElement;
				if (!string.IsNullOrEmpty(dataSign) && xmlElement == null)
				{
					throw new Exception(string.Format("Không tìm thấy phần tử {0} trong tài liệu XML.", dataSign));
				}
				SignedXml signedXml = new SignedXml(xmlDoc);
				RSA signingKey = (RSA)cer.PrivateKey;
				signedXml.SigningKey = signingKey;
				signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
				signedXml.Signature.Id = Guid.NewGuid().ToString();
				KeyInfo keyInfo = new KeyInfo();
				KeyInfoX509Data keyInfoX509Data = new KeyInfoX509Data(cer);
				keyInfoX509Data.AddSubjectName(cer.Subject);
				keyInfo.AddClause(keyInfoX509Data);
				signedXml.KeyInfo = keyInfo;
				Reference reference = new Reference();
				reference.Uri = "";
				reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
				XmlDsigEnvelopedSignatureTransform transform = new XmlDsigEnvelopedSignatureTransform();
				reference.AddTransform(transform);
				signedXml.AddReference(reference);
				signedXml.ComputeSignature();
				XmlElement xml = signedXml.GetXml();
				xmlElement.AppendChild(xmlDoc.ImportNode(xml, true));
				flag = true;
			}
			catch (Exception ex)
			{
				flag = false;
				LogSystem.Error(ex);
			}
			return flag;
		}

		public static bool SignHashXml130(XmlDocument xmlDoc, X509Certificate2 cer, string storeDataSign, Func<string, string> dlgSignHash)
		{
			bool flag = false;
			try
			{
				if (xmlDoc == null)
				{
					throw new ArgumentException(null, "xmlDoc");
				}
				if (cer == null)
				{
					throw new ArgumentException(null, "cer");
				}
				if (dlgSignHash == null)
				{
					throw new ArgumentException(null, "dlgSignHash");
				}
				XmlElement xmlElement = xmlDoc.GetElementsByTagName(storeDataSign)[0] as XmlElement;
				if (!string.IsNullOrEmpty(storeDataSign) && xmlElement == null)
				{
					throw new Exception(string.Format("Không tìm thấy phần tử {0} trong tài liệu XML.", storeDataSign));
				}
				string diget = ComputeDigest(xmlDoc);
				XmlElement xmlElement2 = CreateSignedInfo(diget);
				string arg = ComputeDigest(xmlElement2.OwnerDocument);
				string signedData = dlgSignHash(arg);
				XmlElement node = AddSignedToXML(diget, signedData, cer);
				xmlElement.AppendChild(xmlDoc.ImportNode(node, true));
				flag = true;
			}
			catch (Exception ex)
			{
				flag = false;
				LogSystem.Error(ex);
			}
			return flag;
		}

		private static string ComputeDigest(XmlDocument xmlDocument)
		{
			string result = "";
			XmlDsigC14NTransform xmlDsigC14NTransform = new XmlDsigC14NTransform();
			xmlDsigC14NTransform.LoadInput(xmlDocument);
			using (Stream inputStream = (Stream)xmlDsigC14NTransform.GetOutput(typeof(Stream)))
			{
				byte[] inArray = new SHA256Managed().ComputeHash(inputStream);
				result = Convert.ToBase64String(inArray);
			}
			return result;
		}

		private static XmlElement CreateSignedInfo(string diget)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<SignedInfo xmlns=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
			stringBuilder.Append("<CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" />");
			stringBuilder.Append("<SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\" />");
			stringBuilder.Append("<Reference URI=\"\">");
			stringBuilder.Append("<Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" /></Transforms>");
			stringBuilder.Append("<DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\" />");
			stringBuilder.AppendFormat("<DigestValue>{0}</DigestValue>", diget);
			stringBuilder.Append("</Reference>");
			stringBuilder.Append("</SignedInfo>");
			xmlDocument.LoadXml(stringBuilder.ToString());
			return xmlDocument.DocumentElement;
		}

		private static XmlElement AddSignedToXML(string diget, string signedData, X509Certificate2 certificate)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<Signature Id=\"{0}\" xmlns=\"http://www.w3.org/2000/09/xmldsig#\">", Guid.NewGuid().ToString());
			stringBuilder.Append("<SignedInfo>");
			stringBuilder.Append("<CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" />");
			stringBuilder.Append("<SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\" />");
			stringBuilder.Append("<Reference URI=\"\">");
			stringBuilder.Append("<Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" /></Transforms>");
			stringBuilder.Append("<DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\" />");
			stringBuilder.AppendFormat("<DigestValue>{0}</DigestValue>", diget);
			stringBuilder.Append("</Reference>");
			stringBuilder.Append("</SignedInfo>");
			stringBuilder.AppendFormat("<SignatureValue>{0}</SignatureValue>", signedData);
			if (certificate != null)
			{
				KeyInfo keyInfo = new KeyInfo();
				KeyInfoX509Data keyInfoX509Data = new KeyInfoX509Data(certificate);
				keyInfoX509Data.AddSubjectName(certificate.Subject);
				keyInfo.AddClause(keyInfoX509Data);
				stringBuilder.Append("<KeyInfo>");
				stringBuilder.Append(keyInfo.GetXml().InnerXml);
				stringBuilder.Append("</KeyInfo>");
			}
			stringBuilder.Append("</Signature>");
			xmlDocument.LoadXml(stringBuilder.ToString());
			return xmlDocument.DocumentElement;
		}
	}
}
