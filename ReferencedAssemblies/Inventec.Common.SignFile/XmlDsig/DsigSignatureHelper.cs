using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig
{
	public class DsigSignatureHelper
	{
		public enum DsigSignatureMode
		{
			Client,
			Server
		}

		public static XmlNode tempSignature(byte[] base64Digest, byte[] base64SignatureValue, X509Certificate2 CustomerCert, string SignedTagId, string SigningTagId, bool useNamespacePrefix, bool isTagName)
		{
			if (string.IsNullOrEmpty(SigningTagId))
			{
				SigningTagId = "serSig";
			}
			if (string.IsNullOrEmpty(SignedTagId))
			{
				throw new Exception("Signed Tag Id is required");
			}
			string value = SigningTagId;
			string value2 = "#" + SignedTagId;
			string text = ((base64SignatureValue == null) ? "" : Encoding.UTF8.GetString(base64SignatureValue));
			XmlDocument xmlDocument = new XmlDocument();
			XmlNode xmlNode = xmlDocument.CreateElement("Signature");
			XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("Id");
			xmlAttribute.Value = value;
			XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute("xmlns");
			xmlAttribute2.Value = "http://www.w3.org/2000/09/xmldsig#";
			xmlNode.Attributes.Append(xmlAttribute);
			xmlNode.Attributes.Append(xmlAttribute2);
			XmlNode xmlNode2 = xmlNode.AppendChild(xmlDocument.CreateElement("SignedInfo"));
			XmlNode xmlNode3 = xmlNode2.AppendChild(xmlDocument.CreateElement("CanonicalizationMethod"));
			XmlAttribute xmlAttribute3 = xmlDocument.CreateAttribute("Algorithm");
			if (useNamespacePrefix)
			{
				xmlAttribute3.Value = "http://www.w3.org/2001/10/xml-exc-c14n#";
			}
			else
			{
				xmlAttribute3.Value = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
			}
			xmlNode3.Attributes.Append(xmlAttribute3);
			XmlNode xmlNode4 = xmlNode2.AppendChild(xmlDocument.CreateElement("SignatureMethod"));
			XmlAttribute xmlAttribute4 = xmlDocument.CreateAttribute("Algorithm");
			xmlAttribute4.Value = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
			xmlNode4.Attributes.Append(xmlAttribute4);
			XmlNode xmlNode5 = xmlNode2.AppendChild(xmlDocument.CreateElement("Reference"));
			XmlAttribute xmlAttribute5 = xmlDocument.CreateAttribute("URI");
			if (isTagName)
			{
				xmlAttribute5.Value = "";
			}
			else
			{
				xmlAttribute5.Value = value2;
			}
			xmlNode5.Attributes.Append(xmlAttribute5);
			XmlNode xmlNode6 = xmlNode5.AppendChild(xmlDocument.CreateElement("Transforms"));
			XmlNode xmlNode7 = xmlNode6.AppendChild(xmlDocument.CreateElement("Transform"));
			XmlAttribute xmlAttribute6 = xmlDocument.CreateAttribute("Algorithm");
			xmlAttribute6.Value = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";
			xmlNode7.Attributes.Append(xmlAttribute6);
			if (useNamespacePrefix)
			{
				XmlNode xmlNode8 = xmlNode6.AppendChild(xmlDocument.CreateElement("Transform"));
				XmlAttribute xmlAttribute7 = xmlDocument.CreateAttribute("Algorithm");
				xmlAttribute7.Value = "http://www.w3.org/2001/10/xml-exc-c14n#";
				xmlNode8.Attributes.Append(xmlAttribute7);
			}
			XmlNode xmlNode9 = xmlNode5.AppendChild(xmlDocument.CreateElement("DigestMethod"));
			XmlAttribute xmlAttribute8 = xmlDocument.CreateAttribute("Algorithm");
			xmlAttribute8.Value = "http://www.w3.org/2000/09/xmldsig#sha1";
			xmlNode9.Attributes.Append(xmlAttribute8);
			xmlNode5.AppendChild(xmlDocument.CreateElement("DigestValue")).InnerText = Convert.ToBase64String(base64Digest);
			xmlNode.AppendChild(xmlDocument.CreateElement("SignatureValue")).InnerText = ((base64SignatureValue == null) ? "" : text);
			xmlNode.AppendChild(xmlDocument.CreateElement("KeyInfo")).AppendChild(xmlDocument.CreateElement("X509Data")).AppendChild(xmlDocument.CreateElement("X509Certificate"))
				.InnerText = ((CustomerCert == null) ? "" : Convert.ToBase64String(CustomerCert.RawData));
			return xmlDocument.AppendChild(xmlNode);
		}

		public static byte[] HashForRemote(XmlDocument xd, string SignedTagId, string SigningTagId, string namespacePrefix, X509Certificate2 CustomerCert, string path, bool isSignTagName)
		{
			try
			{
				XmlNode xmlNode = null;
				xmlNode = ((!isSignTagName) ? tempSignature(getDigestForRemoteByID(xd, SignedTagId, namespacePrefix), null, CustomerCert, SignedTagId, SigningTagId, !string.IsNullOrEmpty(namespacePrefix), false) : tempSignature(getDigestForRemoteByTagName(xd, SignedTagId, namespacePrefix), null, CustomerCert, SignedTagId, SigningTagId, !string.IsNullOrEmpty(namespacePrefix), true));
				return PerformHash(xd, xmlNode, path);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private static byte[] PerformHash(XmlDocument xd, XmlNode signature, string path)
		{
			XmlNode newChild = xd.ImportNode(signature, true);
			if (!string.IsNullOrEmpty(path))
			{
				try
				{
					XmlNode xmlNode = xd.SelectSingleNode(path);
					xmlNode.AppendChild(newChild);
				}
				catch (Exception)
				{
					throw new Exception("Đường dẫn đặt thẻ ký không hợp lệ");
				}
			}
			else
			{
				xd.DocumentElement.AppendChild(newChild);
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(signature.OuterXml);
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("SignedInfo");
			XmlDocument xmlDocument2 = new XmlDocument();
			xmlDocument2.LoadXml(elementsByTagName[0].OuterXml);
			XmlDsigC14NTransform xmlDsigC14NTransform = new XmlDsigC14NTransform();
			xmlDsigC14NTransform.LoadInput(xmlDocument2);
			Stream stream = (Stream)xmlDsigC14NTransform.GetOutput(typeof(Stream));
			stream.Position = 0L;
			byte[] array = new byte[16384];
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int count;
				while ((count = stream.Read(array, 0, array.Length)) > 0)
				{
					memoryStream.Write(array, 0, count);
				}
				array = memoryStream.ToArray();
			}
			return new SHA1Managed().ComputeHash(array);
		}

		private static byte[] PerformGetDigest(XmlNode xn, string namespacePrefix)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xn.OuterXml);
			Transform transform = ((!string.IsNullOrEmpty(namespacePrefix)) ? ((Transform)new XmlDsigExcC14NTransform(false, namespacePrefix)) : ((Transform)new XmlDsigC14NTransform()));
			transform.LoadInput(xmlDocument);
			byte[] array = new SHA1CryptoServiceProvider().ComputeHash((Stream)transform.GetOutput(typeof(Stream)));
			Convert.ToBase64String(array);
			return array;
		}

		private static byte[] PerformGetDigest(string xml)
		{
			XmlDsigC14NTransform xmlDsigC14NTransform = new XmlDsigC14NTransform();
			xmlDsigC14NTransform.LoadInput(xml);
			byte[] array = new SHA1CryptoServiceProvider().ComputeHash((Stream)xmlDsigC14NTransform.GetOutput(typeof(Stream)));
			Convert.ToBase64String(array);
			return array;
		}

		public static XmlDocument AddCustomerData(byte[] invdata)
		{
			string xml = Encoding.UTF8.GetString(invdata);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml(xml);
			XmlElement node = (XmlElement)xmlDocument.GetElementsByTagName("Signature")[0];
			XmlElement newChild = (XmlElement)xmlDocument.GetElementsByTagName("Content")[0];
			XmlElement node2 = (XmlElement)xmlDocument.GetElementsByTagName("qrCodeData")[0];
			xmlDocument.DocumentElement.RemoveAll();
			XmlNode xmlNode = xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("ClientData"));
			XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("Id");
			xmlAttribute.Value = "ClientSigningData";
			xmlNode.Attributes.Append(xmlAttribute);
			xmlDocument.DocumentElement.AppendChild(xmlNode);
			xmlDocument.GetElementsByTagName("ClientData")[0].AppendChild(newChild);
			xmlDocument.GetElementsByTagName("ClientData")[0].AppendChild(xmlDocument.CreateElement("Date")).InnerText = DateTime.Now.ToString("dd/MM/yyyy");
			XmlNode newChild2 = xmlDocument.ImportNode(node2, true);
			xmlDocument.DocumentElement.AppendChild(newChild2);
			XmlNode newChild3 = xmlDocument.ImportNode(node, true);
			xmlDocument.DocumentElement.AppendChild(newChild3);
			return xmlDocument;
		}

		public static byte[] getDigest(XmlDocument xmldoc, string namespacePrefix)
		{
			return PerformGetDigest(xmldoc.GetElementsByTagName("ClientData")[0], namespacePrefix);
		}

		public static byte[] getDigestForRemoteByTagName(XmlDocument xmldoc, string signingElementId, string namespacePrefix)
		{
			XmlNode xmlNode = xmldoc.SelectSingleNode(signingElementId);
			if (xmlNode == null)
			{
				throw new Exception("Signing tag name did not found!");
			}
			return PerformGetDigest(xmlNode, namespacePrefix);
		}

		public static byte[] getDigestForRemoteByID(XmlDocument xmldoc, string signingElementId, string namespacePrefix)
		{
			XmlNode xmlNode = xmldoc.SelectSingleNode("//*[@Id|@id='" + signingElementId + "']");
			if (xmlNode == null && xmldoc.FirstChild.Attributes != null)
			{
				xmlNode = xmldoc.FirstChild;
				XmlAttribute xmlAttribute = xmldoc.CreateAttribute("Id");
				xmlAttribute.Value = (string.IsNullOrEmpty(signingElementId) ? generateUUID() : signingElementId);
				xmlNode.Attributes.Append(xmlAttribute);
			}
			return PerformGetDigest(xmlNode, namespacePrefix);
		}

		public static XmlNode GetNodeWithCustomNamespace(XmlDocument xmlDoc, string namespaceName, string signingElementId)
		{
			string text = string.Empty;
			XmlNamespaceManager xmlNamespaceManager = null;
			if (xmlDoc.FirstChild.Attributes != null)
			{
				XmlAttribute xmlAttribute = xmlDoc.FirstChild.Attributes["xmlns"];
				if (xmlAttribute != null)
				{
					xmlNamespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
					xmlNamespaceManager.AddNamespace(namespaceName, xmlAttribute.Value);
					text = namespaceName + ":";
				}
				else
				{
					xmlNamespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
					xmlNamespaceManager.AddNamespace(namespaceName, "http://laphoadon.gdt.gov.vn/2014/09/invoicexml/v1");
					text = namespaceName + ":";
				}
			}
			XmlNode result = xmlDoc.SelectSingleNode("//*[@id='" + signingElementId + "']", xmlNamespaceManager);
			XmlNode xmlNode = xmlDoc.SelectSingleNode(text + "Test1", xmlNamespaceManager);
			return result;
		}

		public static XmlDocument AddCustomerDataForRemote(byte[] invdata, DateTime? signDate = null)
		{
			string xml = Encoding.UTF8.GetString(invdata);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml(xml);
			string innerText = ((signDate.HasValue && signDate.HasValue && signDate != DateTime.MinValue) ? signDate.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy"));
			xmlDocument.GetElementsByTagName("Content")[0].AppendChild(xmlDocument.CreateElement("SigningTime")).InnerText = innerText;
			return xmlDocument;
		}

		public static string generateUUID()
		{
			return Guid.NewGuid().ToString();
		}
	}
}
