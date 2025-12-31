using System.Security.Cryptography.X509Certificates;
using Inventec.Common.SignFile.XmlProcess.Utils.Certificates.Exceptions;

namespace Inventec.Common.SignFile.XmlProcess.Utils.Certificates
{
	public abstract class CertificatesHelper
	{
		public static X509Certificate2Collection GetCertificatesFrom(CertificateStore certificateStoreType)
		{
			if (CertificateStore.My.Equals(certificateStoreType))
			{
				X509Store x509Store = new X509Store(StoreName.My);
				x509Store.Open(OpenFlags.ReadOnly);
				return x509Store.Certificates;
			}
			throw new CertificateStoreAccessDeniedException(string.Format("Cannot access the certificate store {0}", certificateStoreType));
		}
	}
}
