using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Inventec.Common.SignFile.XmlProcess.Common;
using Inventec.Common.SignFile.XmlProcess.Common.Exceptions;
using Inventec.Common.SignFile.XmlProcess.Utils;
using Inventec.Common.SignFile.XmlProcess.Utils.Cryptography;
using Inventec.Common.SignFile.XmlProcess.Utils.Xml;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Operations;

namespace Inventec.Common.SignFile.XmlProcess.Xades.Operations
{
	internal class XadesVerifyOperation
	{
		public static void Verify(VerificationParameters parameters)
		{
			VerificationResults verificationResults = XmlDsigVerifyOperation.VerifyAndGetResults(parameters);
			VerifySigningCertificate(parameters, verificationResults.SigningCertificate);
		}

		private static void VerifySigningCertificate(VerificationParameters parameters, X509Certificate2 signingCertificate)
		{
			byte[] rawData = signingCertificate.RawData;
			byte[] bytesSHA = CryptoHelper.GetBytesSHA1(rawData);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(parameters.InputPath);
			List<XmlElement> list = XmlHelper.FindNodesIn(xmlDocument.DocumentElement, "Signature/Object/QualifyingProperties/SignedProperties/SignedSignatureProperties/SigningCertificate/Cert/CertDigest");
			List<XmlElement> list2 = XmlHelper.FindNodesIn(list[0], "DigestValue");
			byte[] array = Convert.FromBase64String(list2[0].InnerText);
			if (!ArrayHelper.ArraysAreEqual(array, bytesSHA))
			{
				throw new InvalidSignedDocumentException("SigningCertificate cannot be verified");
			}
		}

		public static VerificationResults VerifyAndGetResults(VerificationParameters parameters)
		{
			VerificationResults verificationResults = XmlDsigVerifyOperation.VerifyAndGetResults(parameters);
			VerifySigningCertificate(parameters, verificationResults.SigningCertificate);
			return verificationResults;
		}
	}
}
