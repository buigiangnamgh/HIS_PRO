using System.Xml;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Dsl;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig
{
	public abstract class XmlDsigHelper
	{
		public static SignDSL Sign(string inputPath)
		{
			SignDSL signDSL = new SignDSL();
			VerificationResults verificationResults = Verify(inputPath).PerformAndGetResults();
			if (verificationResults != null && verificationResults.SigningCertificate != null && verificationResults.OriginalDocument != null)
			{
				verificationResults.OriginalDocument.Save(inputPath);
				signDSL.InputPath(inputPath);
			}
			else
			{
				signDSL.InputPath(inputPath);
			}
			return signDSL;
		}

		public static SignDSL Sign(XmlDocument xmlDocument)
		{
			SignDSL signDSL = new SignDSL();
			signDSL.InputXml(xmlDocument);
			return signDSL;
		}

		public static VerificationDsl Verify(string signaturePath)
		{
			VerificationDsl verificationDsl = new VerificationDsl();
			verificationDsl.SignaturePath(signaturePath);
			return verificationDsl;
		}

		public static BatchSignDSL BatchSign(params string[] inputPaths)
		{
			BatchSignDSL batchSignDSL = new BatchSignDSL();
			batchSignDSL.InputPaths(inputPaths);
			return batchSignDSL;
		}
	}
}
