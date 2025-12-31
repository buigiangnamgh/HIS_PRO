using System.Security.Cryptography.Xml;
using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Common.Exceptions;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig.Operations.Signers
{
	internal class XmlDsigDetachedSignOperation : XmlDsigSignOperation
	{
		protected override void CreateAndAddReferenceTo(SignedXml signedXml, XmlDocument document, string inputPath, string xpathToNodeToSign)
		{
			if (signedXml == null)
			{
				throw new InvalidParameterException("Signed Xml cannot be null");
			}
			if (inputPath == null)
			{
				throw new InvalidParameterException("Input path cannot be null");
			}
			Reference reference = new Reference
			{
				Uri = "file://" + inputPath.Replace("\\", "/")
			};
			signedXml.AddReference(reference);
		}
	}
}
