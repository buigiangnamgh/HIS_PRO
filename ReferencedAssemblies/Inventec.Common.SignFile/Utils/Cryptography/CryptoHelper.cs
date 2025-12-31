using System;
using System.Security.Cryptography;
using System.Text;

namespace Inventec.Common.SignFile.XmlProcess.Utils.Cryptography
{
	public class CryptoHelper
	{
		private CryptoHelper()
		{
		}

		public static string GetBase64SHA1(string inputString)
		{
			byte[] bytes = Encoding.Default.GetBytes(inputString.ToCharArray());
			return GetBase64SHA1(bytes);
		}

		public static string GetBase64SHA1(byte[] inputBytes)
		{
			byte[] bytesSHA = GetBytesSHA1(inputBytes);
			return Convert.ToBase64String(bytesSHA);
		}

		public static byte[] GetBytesSHA1(byte[] inputBytes)
		{
			SHA1 sHA = new SHA1CryptoServiceProvider();
			return sHA.ComputeHash(inputBytes);
		}

		public static byte[] GetBytesSHA1(string inputString)
		{
			byte[] bytes = Encoding.Default.GetBytes(inputString.ToCharArray());
			return GetBytesSHA1(bytes);
		}
	}
}
