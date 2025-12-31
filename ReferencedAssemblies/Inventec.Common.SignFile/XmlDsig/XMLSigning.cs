using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig
{
	public class XMLSigning
	{
		public static string createHash(string xmlRaw, string SignedTagId, string SigningTagId, string namespacePrefix, X509Certificate2 certificate2, out string xmlWithHashInfo, string path, bool isSignTagName)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = false;
			xmlDocument.LoadXml(xmlRaw);
			byte[] array = DsigSignatureHelper.HashForRemote(xmlDocument, SignedTagId, SigningTagId, namespacePrefix, certificate2, path, isSignTagName);
			xmlWithHashInfo = xmlDocument.OuterXml;
			AlgorithmIdentifier val = new AlgorithmIdentifier(CryptoConfig.MapNameToOID("SHA1"));
			return Convert.ToBase64String(((Asn1Encodable)new DigestInfo(val, array)).GetEncoded());
		}

		public static string wrapSignature(string xml, string signatureB64, string tagId)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml(xml);
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Signature");
			foreach (XmlNode item in elementsByTagName)
			{
				if (item.Attributes["Id"] == null || !item.Attributes["Id"].Value.ToString().Equals(tagId))
				{
					continue;
				}
				XmlNodeList childNodes = item.ChildNodes;
				foreach (XmlNode item2 in childNodes)
				{
					if (item2.Name.Equals("SignatureValue"))
					{
						item2.InnerText = signatureB64;
					}
				}
			}
			return xmlDocument.OuterXml;
		}

		public static bool isError(string xml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml(xml);
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Signature");
			foreach (XmlNode item in elementsByTagName)
			{
				XmlAttribute xmlAttribute = item.Attributes["Id"];
				XmlNodeList childNodes = item.ChildNodes;
				foreach (XmlNode item2 in childNodes)
				{
					string name = item2.Name;
					if (name.Equals("SignatureValue"))
					{
						string value = item2.LastChild.Value;
						int result;
						if (int.TryParse(value, out result))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public static byte[] Hash(XmlDocument xmlDoc)
		{
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			XmlNodeList elementsByTagName = xmlDoc.GetElementsByTagName("SignedInfo");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(elementsByTagName[0].OuterXml);
			XmlDsigC14NTransform xmlDsigC14NTransform = new XmlDsigC14NTransform();
			xmlDsigC14NTransform.LoadInput(xmlDocument);
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
			byte[] array2 = new SHA1Managed().ComputeHash(array);
			AlgorithmIdentifier val = new AlgorithmIdentifier(CryptoConfig.MapNameToOID("SHA1"));
			string s = Convert.ToBase64String(((Asn1Encodable)new DigestInfo(val, array2)).GetEncoded());
			return Convert.FromBase64String(s);
		}
	}
}
