using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using Inventec.Common.SignFile.XmlProcess.Schemas;

namespace Inventec.Common.SignFile.XmlProcess.Utils.Xml
{
	internal class XmlHelper
	{
		public static string XmlSchemaUrl = "http://www.w3.org/2001/XMLSchema.dtd";

		public static XmlElement CreateNodeIn(XmlDocument document, string nodeName, string nameSpace, XmlElement rootNode)
		{
			XmlElement xmlElement = document.CreateElement(nodeName, nameSpace);
			rootNode.AppendChild(xmlElement);
			return xmlElement;
		}

		public static XmlElement CreateNodeWithTextIn(XmlDocument document, string nodeName, string text, string nameSpace, XmlElement rootNode)
		{
			XmlElement xmlElement = CreateNodeIn(document, nodeName, nameSpace, rootNode);
			xmlElement.InnerText = text;
			return xmlElement;
		}

		public static string DateTimeToCanonicalRepresentation(DateTime ahora)
		{
			return ahora.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
		}

		public static string NowInCanonicalRepresentation()
		{
			DateTime now = DateTime.Now;
			return DateTimeToCanonicalRepresentation(now);
		}

		public static XmlElement FindNodeWithAttributeValueIn(XmlNodeList nodeList, string attributeName, string value)
		{
			if (nodeList.Count == 0)
			{
				return null;
			}
			foreach (XmlNode node in nodeList)
			{
				XmlElement xmlElement = FindNodeWithAttributeValueIn(node, attributeName, value);
				if (xmlElement != null)
				{
					return xmlElement;
				}
			}
			return null;
		}

		private static XmlElement FindNodeWithAttributeValueIn(XmlNode node, string attributeName, string value)
		{
			string attributeValueInNodeOrNull = GetAttributeValueInNodeOrNull(node, attributeName);
			if (attributeValueInNodeOrNull != null && attributeValueInNodeOrNull.Equals(value))
			{
				return (XmlElement)node;
			}
			return FindNodeWithAttributeValueIn(node.ChildNodes, attributeName, value);
		}

		private static string GetAttributeValueInNodeOrNull(XmlNode node, string attributeName)
		{
			XmlAttributeCollection attributes = node.Attributes;
			if (attributes != null)
			{
				XmlAttribute xmlAttribute = attributes[attributeName];
				if (xmlAttribute != null)
				{
					return xmlAttribute.Value;
				}
			}
			return null;
		}

		public static void ValidateFromSchemas(string inputPath, params string[] schemas)
		{
			XmlTextReader reader = new XmlTextReader(inputPath);
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
			{
				ProhibitDtd = false,
				ValidationType = ValidationType.Schema
			};
			xmlReaderSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
			foreach (string text in schemas)
			{
				xmlReaderSettings.Schemas.Add(text, SignatureSchemas.Get(text));
			}
			using (XmlReader xmlReader = XmlReader.Create(reader, xmlReaderSettings))
			{
				while (xmlReader.Read())
				{
				}
			}
		}

		public static XmlElement DescendantWith(XmlElement rootNode, Predicate<XmlElement> conditionToComply)
		{
			foreach (object childNode in rootNode.ChildNodes)
			{
				if (childNode is XmlElement)
				{
					XmlElement xmlElement = (XmlElement)childNode;
					if (conditionToComply(xmlElement))
					{
						return xmlElement;
					}
				}
			}
			return null;
		}

		public static List<XmlElement> DescendantsWith(XmlElement rootNode, Predicate<XmlElement> conditionToComply)
		{
			List<XmlElement> list = new List<XmlElement>();
			foreach (object childNode in rootNode.ChildNodes)
			{
				if (childNode is XmlElement)
				{
					XmlElement xmlElement = (XmlElement)childNode;
					if (conditionToComply(xmlElement))
					{
						list.Add(xmlElement);
					}
				}
			}
			return list;
		}

		public static List<XmlElement> FindNodesIn(XmlElement rootNode, string path)
		{
			List<XmlElement> list = new List<XmlElement>();
			if (string.IsNullOrEmpty(path))
			{
				return list;
			}
			if (!path.Contains("/"))
			{
				return DescendantsWith(rootNode, (XmlElement n) => path.Equals(n.LocalName));
			}
			string token = path.Split('/')[0];
			List<XmlElement> list2 = DescendantsWith(rootNode, (XmlElement n) => token.Equals(n.LocalName));
			if (list2.Count == 0)
			{
				return list;
			}
			foreach (XmlElement item in list2)
			{
				list.AddRange(FindNodesIn(item, path.Substring(token.Length + 1)));
			}
			return list;
		}

		public static XmlDocument ReadXmlFromUri(string uri)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlTextReader reader = new XmlTextReader(uri);
			xmlDocument.Load(reader);
			return xmlDocument;
		}

		public static string AttributeOf(XmlElement node, string attributeName)
		{
			XmlAttribute xmlAttribute = node.Attributes[attributeName];
			return (xmlAttribute == null) ? null : xmlAttribute.Value;
		}
	}
}
