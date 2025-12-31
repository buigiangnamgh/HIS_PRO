using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Utils.Xml;

namespace Inventec.Common.SignFile.XmlProcess.Common
{
	public class ExtendedSignedXml : SignedXml
	{
		public const string XmlDsigSignatureProperties = "http://www.w3.org/2000/09/xmldsig#SignatureProperties";

		private readonly List<DataObject> _dataObjects = new List<DataObject>();

		public const string XmlDSigTimestampNamespace = "http://xadesnet.codeplex.com/#timestamp";

		public XmlElement PropertiesNode { get; set; }

		public ExtendedSignedXml(XmlDocument document)
			: base(document)
		{
		}

		public override XmlElement GetIdElement(XmlDocument doc, string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return null;
			}
			XmlElement idElement = base.GetIdElement(doc, id);
			if (idElement != null)
			{
				return idElement;
			}
			if (_dataObjects.Count == 0)
			{
				return null;
			}
			foreach (DataObject dataObject in _dataObjects)
			{
				XmlElement xmlElement = XmlHelper.FindNodeWithAttributeValueIn(dataObject.Data, "Id", id);
				if (xmlElement != null)
				{
					return xmlElement;
				}
			}
			return null;
		}

		public new void AddObject(DataObject dataObject)
		{
			base.AddObject(dataObject);
			_dataObjects.Add(dataObject);
		}
	}
}
