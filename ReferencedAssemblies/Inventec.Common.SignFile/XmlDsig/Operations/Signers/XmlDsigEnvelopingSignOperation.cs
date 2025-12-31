using System.Security.Cryptography.Xml;
using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Common.Exceptions;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig.Operations.Signers
{
	internal class XmlDsigEnvelopingSignOperation : XmlDsigSignOperation
	{
		protected override void CreateAndAddReferenceTo(SignedXml signedXml, XmlDocument document, string inputPath, string xpathToNodeToSign)
		{
			if (signedXml == null)
			{
				throw new InvalidParameterException("Signed Xml cannot be null");
			}
			if (document == null)
			{
				throw new InvalidParameterException("Xml document cannot be null");
			}
			if (document.DocumentElement == null)
			{
				throw new InvalidParameterException("Xml document must have root element");
			}
			string text = "";
			Reference reference = new Reference("#documentdata" + text);
			reference.AddTransform(new XmlDsigExcC14NTransform());
			signedXml.AddReference(reference);
			DataObject dataObject = new DataObject("documentdata" + text, "", "", document.DocumentElement);
			signedXml.AddObject(dataObject);
		}

		protected override void AddCanonicalizationMethodTo(SignedXml signedXml)
		{
			signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
		}
	}
}
