using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig.Common
{
	public class VerificationResults
	{
		public string Timestamp { get; set; }

		public XmlDocument OriginalDocument { get; set; }

		public X509Certificate2 SigningCertificate { get; set; }
	}
}
