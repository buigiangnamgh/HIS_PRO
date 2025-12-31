using System.Collections.Generic;
using System.IO;
using System.Xml;
using Inventec.Common.SignFile.Properties;
using Inventec.Common.SignFile.XmlProcess.Utils.Xml;

namespace Inventec.Common.SignFile.XmlProcess.Schemas
{
	public class SignatureSchemas
	{
		private static readonly Dictionary<string, string> Schemas = new Dictionary<string, string>();

		private SignatureSchemas()
		{
		}

		public static XmlReader Get(string schemaUri)
		{
			LoadSchemasIfNeeded();
			if (Schemas.ContainsKey(schemaUri))
			{
				XmlReaderSettings settings = new XmlReaderSettings
				{
					ProhibitDtd = false
				};
				return XmlReader.Create(new StringReader(Schemas[schemaUri]), settings);
			}
			return null;
		}

		private static void LoadSchemasIfNeeded()
		{
			if (Schemas.Count == 0)
			{
				Schemas["http://www.w3.org/2000/09/xmldsig#"] = Resources.xmldsig_core_schema;
				Schemas[XmlHelper.XmlSchemaUrl] = Resources.XMLSchema;
			}
		}
	}
}
