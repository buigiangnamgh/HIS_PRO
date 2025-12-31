using System;
using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Common.Exceptions;
using Inventec.Common.SignFile.XmlProcess.Utils.Xml;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig.Helpers
{
	internal class XmlDsigNodesHelper
	{
		private XmlDsigNodesHelper()
		{
		}

		internal static bool ObjectNodeWithIdAsUri(XmlElement n, string uri)
		{
			return "Object".Equals(n.LocalName) && "http://www.w3.org/2000/09/xmldsig#".Equals(n.NamespaceURI) && uri.Equals(XmlHelper.AttributeOf(n, "Id"));
		}

		internal static bool IsSignedInfoNode(XmlElement node)
		{
			if (node == null)
			{
				return false;
			}
			return "SignedInfo".Equals(node.LocalName) && "http://www.w3.org/2000/09/xmldsig#".Equals(node.NamespaceURI);
		}

		internal static bool IsX509Certificate(XmlElement node)
		{
			return "X509Certificate".Equals(node.LocalName) && "http://www.w3.org/2000/09/xmldsig#".Equals(node.NamespaceURI);
		}

		internal static bool IsX509Data(XmlElement node)
		{
			return "X509Data".Equals(node.LocalName) && "http://www.w3.org/2000/09/xmldsig#".Equals(node.NamespaceURI);
		}

		internal static bool IsKeyInfo(XmlElement node)
		{
			return "KeyInfo".Equals(node.LocalName) && "http://www.w3.org/2000/09/xmldsig#".Equals(node.NamespaceURI);
		}

		internal static bool IsSignatureNode(XmlElement node)
		{
			if (node == null)
			{
				throw new Exception("Node cannot be null");
			}
			return "Signature".Equals(node.LocalName) && "http://www.w3.org/2000/09/xmldsig#".Equals(node.NamespaceURI);
		}

		internal static bool IsReferenceToOriginalContent(XmlElement node)
		{
			if (node == null)
			{
				return false;
			}
			bool flag = "Reference".Equals(node.LocalName);
			bool flag2 = "http://www.w3.org/2000/09/xmldsig#".Equals(node.NamespaceURI);
			XmlAttribute xmlAttribute = node.Attributes["Type"];
			bool flag3 = true;
			if (xmlAttribute != null)
			{
				flag3 = "http://www.w3.org/2000/09/xmldsig#SignatureProperties".Equals(xmlAttribute.Value);
			}
			return flag && flag2 && flag3;
		}

		internal static bool ReferenceIsDetached(XmlElement referenceToContentsNode)
		{
			return !referenceToContentsNode.Attributes["URI"].Value.StartsWith("#");
		}

		internal static XmlElement GetSignatureNode(XmlDocument document)
		{
			if (document == null)
			{
				throw new InvalidParameterException("Xml document cannot be null");
			}
			if (document.DocumentElement == null)
			{
				throw new InvalidParameterException("Xml document must have root element");
			}
			if (document.DocumentElement.LocalName.Equals("Signature"))
			{
				return document.DocumentElement;
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(document.NameTable);
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			XmlElement xmlElement = document.DocumentElement.SelectSingleNode("ds:Signature", xmlNamespaceManager) as XmlElement;
			if (xmlElement == null)
			{
				throw new InvalidSignedDocumentException("'Signature' node not found in enveloped signature");
			}
			return xmlElement;
		}
	}
}
