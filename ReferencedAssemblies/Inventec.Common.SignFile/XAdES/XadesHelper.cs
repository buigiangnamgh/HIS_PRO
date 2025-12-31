using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Xades.Dsl;

namespace Inventec.Common.SignFile.XmlProcess.XAdES
{
	public class XadesHelper
	{
		public static XadesSignDsl Sign(string inputPath)
		{
			XadesSignDsl xadesSignDsl = new XadesSignDsl();
			xadesSignDsl.InputPath(inputPath);
			return xadesSignDsl;
		}

		public static XadesSignDsl Sign(XmlDocument xmlDocument)
		{
			XadesSignDsl xadesSignDsl = new XadesSignDsl();
			xadesSignDsl.InputXml(xmlDocument);
			return xadesSignDsl;
		}

		public static XadesVerifyDsl Verify(string signaturePath)
		{
			XadesVerifyDsl xadesVerifyDsl = new XadesVerifyDsl();
			xadesVerifyDsl.SignaturePath(signaturePath);
			return xadesVerifyDsl;
		}
	}
}
