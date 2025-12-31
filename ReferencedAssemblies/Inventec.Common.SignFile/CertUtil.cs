using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Inventec.Common.Logging;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace Inventec.Common.SignFile
{
	public static class CertUtil
	{
		public static IEnumerable<T> GetAll<T>(Func<X509Certificate2, T> selector, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false)
		{
			if (selector == null)
			{
				throw new ArgumentNullException("selector");
			}
			X509Store x509Store = null;
			try
			{
				x509Store = new X509Store(storeName, storeLocation);
				x509Store.Open(OpenFlags.ReadOnly);
				return (from X509Certificate2 cert in x509Store.Certificates
					where requirePrivateKey ? cert.HasPrivateKey : (!validOnly || (validOnly && cert.Verify()))
					select cert).Select(selector).ToArray();
			}
			finally
			{
				if (x509Store != null)
				{
					x509Store.Close();
				}
			}
		}

		public static X509Certificate2 GetBySerial(string serial, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false)
		{
			if (string.IsNullOrEmpty(serial))
			{
				throw new ArgumentNullException("serial");
			}
			X509Store x509Store = null;
			try
			{
				x509Store = new X509Store(storeName, storeLocation);
				x509Store.Open(OpenFlags.ReadOnly);
				return (from X509Certificate2 c in x509Store.Certificates.Find(X509FindType.FindBySerialNumber, serial, validOnly)
					where !requirePrivateKey || c.HasPrivateKey
					select c).FirstOrDefault();
			}
			finally
			{
				if (x509Store != null)
				{
					x509Store.Close();
				}
			}
		}

		public static X509Certificate2 GetFromFile(string fileName, string password)
		{
			return new X509Certificate2(fileName, password);
		}

		public static bool CheckHasCertificate(StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false)
		{
			X509Store x509Store = null;
			try
			{
				x509Store = new X509Store(storeName, storeLocation);
				x509Store.Open(OpenFlags.ReadOnly);
				X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates;
				if (requirePrivateKey)
				{
					X509Certificate2[] certificates = (from X509Certificate2 cert in x509Certificate2Collection
						where cert.HasPrivateKey && (!validOnly || (validOnly && cert.Verify()))
						select cert).ToArray();
					x509Certificate2Collection = new X509Certificate2Collection(certificates);
				}
				int count = x509Certificate2Collection.Count;
				if (count > 0)
				{
					return true;
				}
			}
			finally
			{
				if (x509Store != null)
				{
					x509Store.Close();
				}
			}
			return false;
		}

		public static X509Certificate2 GetByDialog(StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false, string issuerName = "", string subjectName = "")
		{
			return GetByDialog(null, storeName, storeLocation, validOnly, requirePrivateKey, issuerName, subjectName);
		}

		public static X509Certificate2 GetByDialog(IntPtr? hwndParent, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false, string issuerName = "", string subjectName = "")
		{
			X509Store x509Store = null;
			try
			{
				x509Store = new X509Store(storeName, storeLocation);
				x509Store.Open(OpenFlags.ReadOnly);
				X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates;
				if (requirePrivateKey)
				{
					X509Certificate2[] certificates = (from X509Certificate2 cert in x509Certificate2Collection
						where cert.HasPrivateKey && (!validOnly || (validOnly && cert.Verify()))
						select cert).ToArray();
					x509Certificate2Collection = new X509Certificate2Collection(certificates);
					if (!string.IsNullOrEmpty(subjectName))
					{
						x509Certificate2Collection = x509Certificate2Collection.Find(X509FindType.FindBySubjectName, subjectName, validOnly);
					}
					if (!string.IsNullOrEmpty(issuerName))
					{
						x509Certificate2Collection = x509Certificate2Collection.Find(X509FindType.FindByIssuerName, issuerName, validOnly);
					}
				}
				X509Certificate2Collection x509Certificate2Collection2 = null;
				x509Certificate2Collection2 = ((!hwndParent.HasValue) ? X509Certificate2UI.SelectFromCollection(x509Certificate2Collection, "Certificates", "Select one to sign", X509SelectionFlag.SingleSelection) : X509Certificate2UI.SelectFromCollection(x509Certificate2Collection, "Certificates", "Select one to sign", X509SelectionFlag.SingleSelection, hwndParent.Value));
				return (x509Certificate2Collection2.Count == 0) ? null : x509Certificate2Collection2.Cast<X509Certificate2>().FirstOrDefault();
			}
			finally
			{
				if (x509Store != null)
				{
					x509Store.Close();
				}
			}
		}

        public static Org.BouncyCastle.X509.X509Certificate GetX509Cert(System.Security.Cryptography.X509Certificates.X509Certificate cert)
		{
			try
			{
                return Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(cert);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error get Chain cert: " + ex.Message);
				return null;
			}
		}

        public static Org.BouncyCastle.X509.X509Certificate GetX509Cert(string CertStr)
		{
			try
			{
				byte[] bytes = SharedUtils.GetBytes(CertStr);
                System.Security.Cryptography.X509Certificates.X509Certificate x509Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate();
				x509Certificate.Import(bytes);
				return DotNetUtilities.FromX509Certificate(x509Certificate);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error get Chain cert: " + ex.Message);
				return null;
			}
		}

        public static Org.BouncyCastle.X509.X509Certificate[] GetX509CertChain(X509Certificate2 cert)
		{
            List<System.Security.Cryptography.X509Certificates.X509Certificate> list = new List<System.Security.Cryptography.X509Certificates.X509Certificate>();
			X509Chain x509Chain = new X509Chain
			{
				ChainPolicy = 
				{
					RevocationMode = X509RevocationMode.Online
				}
			};
			x509Chain.Build(cert);
			X509ChainElementEnumerator enumerator = x509Chain.ChainElements.GetEnumerator();
			while (enumerator.MoveNext())
			{
				X509ChainElement current = enumerator.Current;
				list.Add(current.Certificate);
			}
			if (list != null && list.Count != 0)
			{
				return GetX509CertChain(list.ToArray());
			}
			return null;
		}

        public static Org.BouncyCastle.X509.X509Certificate[] GetX509CertChain(System.Security.Cryptography.X509Certificates.X509Certificate[] certChain)
		{
			if (certChain == null || certChain.Length == 0)
			{
				return null;
			}
            Org.BouncyCastle.X509.X509Certificate[] array = (Org.BouncyCastle.X509.X509Certificate[])(object)new Org.BouncyCastle.X509.X509Certificate[certChain.Length];
			try
			{
				for (int i = 0; i < certChain.Length; i++)
				{
					array[i] = GetX509Cert(certChain[i]);
				}
				return array;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error get Chain cert: " + ex.Message);
				return null;
			}
		}

		public static BigInteger HexadecimalStringToBigInt(string input)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Expected O, but got Unknown
			return new BigInteger(input, 16);
		}

		public static string BigIntToHexadecimalString(BigInteger input)
		{
			return input.ToString(16);
		}

		public static string NormalizeSerialString(string input)
		{
			return (input != null) ? input.Replace(" ", "").ToUpperInvariant() : string.Empty;
		}

		public static void AddCertificate(X509Certificate2 cert, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser)
		{
			X509Store x509Store = null;
			try
			{
				x509Store = new X509Store(storeName, storeLocation);
				x509Store.Open(OpenFlags.ReadWrite);
				x509Store.Add(cert);
			}
			finally
			{
				if (x509Store != null)
				{
					x509Store.Close();
				}
			}
		}

		public static void RemoveCertificate(string serial, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser)
		{
			if (string.IsNullOrEmpty(serial))
			{
				throw new ArgumentNullException("serial");
			}
			X509Store x509Store = null;
			try
			{
				x509Store = new X509Store(storeName, storeLocation);
				x509Store.Open(OpenFlags.ReadWrite);
				X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySerialNumber, serial, false);
				if (x509Certificate2Collection.Count != 0)
				{
					x509Store.RemoveRange(x509Certificate2Collection);
				}
			}
			finally
			{
				if (x509Store != null)
				{
					x509Store.Close();
				}
			}
		}

		internal static void InstallPINStore(RSACryptoServiceProvider rsa, string pinCode)
		{
			CspParameters cspParameters = new CspParameters();
			cspParameters.KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName;
			cspParameters.ProviderName = rsa.CspKeyContainerInfo.ProviderName;
			cspParameters.ProviderType = rsa.CspKeyContainerInfo.ProviderType;
			cspParameters.KeyPassword = SharedUtils.GetSecurePin(pinCode);
			cspParameters.Flags = CspProviderFlags.NoPrompt;
			LogSystem.Info(LogUtil.TraceData("cspp", (object)cspParameters));
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
		}
	}
}
