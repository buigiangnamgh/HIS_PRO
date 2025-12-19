using iTextSharp.text.pdf;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Inventec.Common.SignFile
{
    /// <summary>
    /// Certificate utilities
    /// </summary>
    public static class CertUtil
    {
        #region Retrieval
        /// <summary>
        /// Gets the certificates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLocation">The store location.</param>
        /// <param name="validOnly">if set to <c>true</c> [valid only].</param>
        /// <param name="requirePrivateKey">if set to <c>true</c> [require private key].</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>(Func<X509Certificate2, T> selector, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            X509Store store = null;
            try
            {
                store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly);
                return store.Certificates
                            .Cast<X509Certificate2>()
                            .Where(cert => requirePrivateKey ? cert.HasPrivateKey : true
                                && (!validOnly || (validOnly && cert.Verify())))
                            .Select(selector)
                            .ToArray();
            }
            finally
            {
                if (store != null)
                {
                    store.Close();
                }
            }
        }

        /// <summary>
        /// Gets a certificate with the specified serial number.
        /// </summary>
        /// <param name="serial">The serial.</param>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLocation">The store location.</param>
        /// <param name="validOnly">if set to <c>true</c> [valid only].</param>
        /// <param name="requirePrivateKey">if set to <c>true</c> [require private key].</param>
        /// <returns></returns>
        public static X509Certificate2 GetBySerial(string serial, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false)
        {
            if (String.IsNullOrEmpty(serial))
            {
                throw new ArgumentNullException("serial");
            }
            X509Store store = null;
            try
            {
                store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly);
                return store.Certificates
                            .Find(X509FindType.FindBySerialNumber, serial, validOnly)
                            .Cast<X509Certificate2>()
                            .Where(c => requirePrivateKey ? c.HasPrivateKey : true)
                            .FirstOrDefault();
            }
            finally
            {
                if (store != null)
                {
                    store.Close();
                }
            }
        }

        /// <summary>
        /// Gets the certificate from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static X509Certificate2 GetFromFile(string fileName, string password)
        {
            return new X509Certificate2(fileName, password);
        }

        /// <summary>
        /// Check has certificate?
        /// </summary>
        /// <returns></returns>
        public static bool CheckHasCertificate(StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false)
        {
            X509Store store = null;
            try
            {
                store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly);
                var selections = store.Certificates;
                if (requirePrivateKey)
                {
                    var filtered = selections
                                    .Cast<X509Certificate2>()
                                    .Where(cert => cert.HasPrivateKey
                                        && (!validOnly || (validOnly && cert.Verify())))
                                    .ToArray();
                    selections = new X509Certificate2Collection(filtered);
                }

                int countCertificate = selections.Count;
                if (countCertificate > 0)
                {
                    return true;
                }
            }
            finally
            {
                if (store != null)
                {
                    store.Close();
                }
            }
            return false;
        }

        /// <summary>
        /// Show a dialog and have the user select a certificate to sign.
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2 GetByDialog(StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false, string issuerName = "", string subjectName = "")
        {
            return GetByDialog(null, storeName, storeLocation, validOnly, requirePrivateKey, issuerName, subjectName);
        }
        public static X509Certificate2 GetByDialog(IntPtr? hwndParent, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool validOnly = false, bool requirePrivateKey = false, string issuerName = "", string subjectName = "")
        {
            X509Store store = null;
            X509Certificate2 result;
            try
            {
                store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly);
                var selections = store.Certificates;
                if (requirePrivateKey)
                {
                    var filtered = selections
                                    .Cast<X509Certificate2>()
                                    .Where(cert => cert.HasPrivateKey
                                        && (!validOnly || (validOnly && cert.Verify()))
                                        )
                                    .ToArray();

                    selections = new X509Certificate2Collection(filtered);
                    if (!String.IsNullOrEmpty(subjectName))
                        selections = selections.Find(X509FindType.FindBySubjectName, subjectName, validOnly);
                    if (!String.IsNullOrEmpty(issuerName))
                        selections = selections.Find(X509FindType.FindByIssuerName, issuerName, validOnly);
                }
                X509Certificate2Collection selection = null;
                if (hwndParent.HasValue)
                {
                    selection = X509Certificate2UI.SelectFromCollection(selections, "Certificates", "Select one to sign", X509SelectionFlag.SingleSelection, hwndParent.Value);
                }
                else
                {
                    selection = X509Certificate2UI.SelectFromCollection(selections, "Certificates", "Select one to sign", X509SelectionFlag.SingleSelection);
                }
                result = selection.Count == 0
                             ? null
                             : selection
                                    .Cast<X509Certificate2>()
                                    .FirstOrDefault();
            }
            finally
            {
                if (store != null)
                {
                    store.Close();
                }
            }
            return result;
        }

        public static Org.BouncyCastle.X509.X509Certificate GetX509Cert(System.Security.Cryptography.X509Certificates.X509Certificate cert)
        {
            try
            {
                return Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(cert);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error get Chain cert: " + exception.Message);
                return null;
            }
        }

        public static Org.BouncyCastle.X509.X509Certificate GetX509Cert(string CertStr)
        {
            try
            {
                byte[] bytes = SharedUtils.GetBytes(CertStr);
                System.Security.Cryptography.X509Certificates.X509Certificate certificate1 = new System.Security.Cryptography.X509Certificates.X509Certificate();
                certificate1.Import(bytes);
                return Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(certificate1);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error get Chain cert: " + exception.Message);
                return null;
            }
        }

        public static Org.BouncyCastle.X509.X509Certificate[] GetX509CertChain(X509Certificate2 cert)
        {
            List<System.Security.Cryptography.X509Certificates.X509Certificate> list = new List<System.Security.Cryptography.X509Certificates.X509Certificate>();
            X509Chain chain1 = new X509Chain
            {
                ChainPolicy = { RevocationMode = X509RevocationMode.Online }
            };
            chain1.Build(cert);
            X509ChainElementEnumerator enumerator = chain1.ChainElements.GetEnumerator();
            while (enumerator.MoveNext())
            {
                X509ChainElement current = enumerator.Current;
                list.Add(current.Certificate);
            }
            if ((list != null) && (list.Count != 0))
            {
                return GetX509CertChain(list.ToArray());
            }
            return null;
        }

        public static Org.BouncyCastle.X509.X509Certificate[] GetX509CertChain(System.Security.Cryptography.X509Certificates.X509Certificate[] certChain)
        {
            if ((certChain == null) || (certChain.Length == 0))
            {
                return null;
            }
            Org.BouncyCastle.X509.X509Certificate[] certificateArray = new Org.BouncyCastle.X509.X509Certificate[certChain.Length];
            try
            {
                for (int i = 0; i < certChain.Length; i++)
                {
                    certificateArray[i] = GetX509Cert(certChain[i]);
                }
                return certificateArray;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error get Chain cert: " + exception.Message);
                return null;
            }
        }
        #endregion

        #region Misc
        /// <summary>
        /// Convert from hexadecimal string to big integer.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static BigInteger HexadecimalStringToBigInt(string input)
        {
            //return BigInteger.Parse(NormalizeSerialString(input), System.Globalization.NumberStyles.HexNumber);
            return new BigInteger(input, 16);
        }

        /// <summary>
        /// Convert from big integer to hexadecimal string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string BigIntToHexadecimalString(BigInteger input)
        {
            //return input.ToString("x");
            return input.ToString(16);
        }

        /// <summary>
        /// Normalizes the serial string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string NormalizeSerialString(string input)
        {
            return input != null ? input.Replace(" ", "").ToUpperInvariant() : String.Empty;
        }
#pragma warning disable 1591
        public static void AddCertificate(X509Certificate2 cert, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser)
        {
            X509Store store = null;
            try
            {
                store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);
            }
            finally
            {
                if (store != null)
                {
                    store.Close();
                }
            }
        }
#pragma warning disable 1591
        public static void RemoveCertificate(string serial, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser)
        {
            if (String.IsNullOrEmpty(serial))
            {
                throw new ArgumentNullException("serial");
            }
            X509Store store = null;
            try
            {
                store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadWrite);
                var existings = store.Certificates.Find(X509FindType.FindBySerialNumber, serial, false);
                if (existings.Count == 0)
                {
                    return;
                }
                store.RemoveRange(existings);
            }
            finally
            {
                if (store != null)
                {
                    store.Close();
                }
            }
        }


        #endregion

        internal static void InstallPINStore(RSACryptoServiceProvider rsa, string pinCode)
        {
            CspParameters cspp = new CspParameters();
            cspp.KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName;
            cspp.ProviderName = rsa.CspKeyContainerInfo.ProviderName;
            cspp.ProviderType = rsa.CspKeyContainerInfo.ProviderType;
            cspp.KeyPassword = SharedUtils.GetSecurePin(pinCode);
            cspp.Flags = CspProviderFlags.NoPrompt;

            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("cspp", cspp));

            //the pin code will be cached for next access to the smart card
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider(cspp);
        }
    }
}