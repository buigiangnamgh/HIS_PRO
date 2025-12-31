using System;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Inventec.Common.SignFile
{
	public static class RSAKeys
	{
		public static RSACryptoServiceProvider ImportPrivateKey(string pem)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			PemReader val = new PemReader((TextReader)new StringReader(pem));
			AsymmetricCipherKeyPair val2 = (AsymmetricCipherKeyPair)val.ReadObject();
			RSAParameters parameters = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)val2.Private);
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.ImportParameters(parameters);
			return rSACryptoServiceProvider;
		}

		public static RSACryptoServiceProvider ImportPublicKey(string pem)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			PemReader val = new PemReader((TextReader)new StringReader(pem));
			AsymmetricKeyParameter val2 = (AsymmetricKeyParameter)val.ReadObject();
			RSAParameters parameters = DotNetUtilities.ToRSAParameters((RsaKeyParameters)val2);
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.ImportParameters(parameters);
			return rSACryptoServiceProvider;
		}

		public static string ExportPrivateKey(RSACryptoServiceProvider csp, bool armor = true, bool base64Encode = true)
		{
			if (csp == null)
			{
				throw new ArgumentNullException("csp");
			}
			if (csp.PublicOnly)
			{
				throw new ArgumentException("CSP does not contain a private key", "csp");
			}
			using (StringWriter stringWriter = new StringWriter())
			{
				RSAParameters rSAParameters = csp.ExportParameters(true);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						binaryWriter.Write((byte)48);
						using (MemoryStream memoryStream2 = new MemoryStream())
						{
							using (BinaryWriter stream = new BinaryWriter(memoryStream2))
							{
								EncodeIntegerBigEndian(stream, new byte[1]);
								EncodeIntegerBigEndian(stream, rSAParameters.Modulus);
								EncodeIntegerBigEndian(stream, rSAParameters.Exponent);
								EncodeIntegerBigEndian(stream, rSAParameters.D);
								EncodeIntegerBigEndian(stream, rSAParameters.P);
								EncodeIntegerBigEndian(stream, rSAParameters.Q);
								EncodeIntegerBigEndian(stream, rSAParameters.DP);
								EncodeIntegerBigEndian(stream, rSAParameters.DQ);
								EncodeIntegerBigEndian(stream, rSAParameters.InverseQ);
								int num = (int)memoryStream2.Length;
								EncodeLength(binaryWriter, num);
								binaryWriter.Write(memoryStream2.GetBuffer(), 0, num);
							}
						}
						if (armor)
						{
							stringWriter.Write("-----BEGIN RSA PRIVATE KEY-----\n");
						}
						if (base64Encode)
						{
							char[] array = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length).ToCharArray();
							for (int i = 0; i < array.Length; i += 64)
							{
								stringWriter.Write(array, i, Math.Min(64, array.Length - i));
								stringWriter.Write("\n");
							}
						}
						else
						{
							stringWriter.Write(memoryStream.GetBuffer());
						}
						if (armor)
						{
							stringWriter.Write("-----END RSA PRIVATE KEY-----");
						}
					}
				}
				return stringWriter.ToString();
			}
		}

		public static string ExportPublicKey(RSACryptoServiceProvider csp, bool armor = true, bool base64Encode = true)
		{
			if (csp == null)
			{
				throw new ArgumentNullException("csp");
			}
			using (StringWriter stringWriter = new StringWriter())
			{
				RSAParameters rSAParameters = csp.ExportParameters(false);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						binaryWriter.Write((byte)48);
						using (MemoryStream memoryStream2 = new MemoryStream())
						{
							using (BinaryWriter binaryWriter2 = new BinaryWriter(memoryStream2))
							{
								binaryWriter2.Write((byte)48);
								EncodeLength(binaryWriter2, 13);
								binaryWriter2.Write((byte)6);
								byte[] array = new byte[9] { 42, 134, 72, 134, 247, 13, 1, 1, 1 };
								EncodeLength(binaryWriter2, array.Length);
								binaryWriter2.Write(array);
								binaryWriter2.Write((byte)5);
								EncodeLength(binaryWriter2, 0);
								binaryWriter2.Write((byte)3);
								using (MemoryStream memoryStream3 = new MemoryStream())
								{
									using (BinaryWriter binaryWriter3 = new BinaryWriter(memoryStream3))
									{
										binaryWriter3.Write((byte)0);
										binaryWriter3.Write((byte)48);
										using (MemoryStream memoryStream4 = new MemoryStream())
										{
											using (BinaryWriter stream = new BinaryWriter(memoryStream4))
											{
												EncodeIntegerBigEndian(stream, rSAParameters.Modulus);
												EncodeIntegerBigEndian(stream, rSAParameters.Exponent);
												int num = (int)memoryStream4.Length;
												EncodeLength(binaryWriter3, num);
												binaryWriter3.Write(memoryStream4.GetBuffer(), 0, num);
											}
										}
										int num2 = (int)memoryStream3.Length;
										EncodeLength(binaryWriter2, num2);
										binaryWriter2.Write(memoryStream3.GetBuffer(), 0, num2);
									}
								}
								int num3 = (int)memoryStream2.Length;
								EncodeLength(binaryWriter, num3);
								binaryWriter.Write(memoryStream2.GetBuffer(), 0, num3);
							}
						}
						if (armor)
						{
							stringWriter.Write("-----BEGIN PUBLIC KEY-----\n");
						}
						if (base64Encode)
						{
							char[] array2 = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length).ToCharArray();
							for (int i = 0; i < array2.Length; i += 64)
							{
								stringWriter.Write(array2, i, Math.Min(64, array2.Length - i));
								stringWriter.Write("\n");
							}
						}
						else
						{
							stringWriter.Write(memoryStream.GetBuffer());
						}
						if (armor)
						{
							stringWriter.Write("-----END PUBLIC KEY-----");
						}
					}
				}
				return stringWriter.ToString();
			}
		}

		private static void EncodeLength(BinaryWriter stream, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
			}
			if (length < 128)
			{
				stream.Write((byte)length);
				return;
			}
			int num = length;
			int num2 = 0;
			while (num > 0)
			{
				num >>= 8;
				num2++;
			}
			stream.Write((byte)(num2 | 0x80));
			for (int num3 = num2 - 1; num3 >= 0; num3--)
			{
				stream.Write((byte)((length >> 8 * num3) & 0xFF));
			}
		}

		private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
		{
			stream.Write((byte)2);
			int num = 0;
			for (int i = 0; i < value.Length && value[i] == 0; i++)
			{
				num++;
			}
			if (value.Length - num == 0)
			{
				EncodeLength(stream, 1);
				stream.Write((byte)0);
				return;
			}
			if (forceUnsigned && value[num] > 127)
			{
				EncodeLength(stream, value.Length - num + 1);
				stream.Write((byte)0);
			}
			else
			{
				EncodeLength(stream, value.Length - num);
			}
			for (int j = num; j < value.Length; j++)
			{
				stream.Write(value[j]);
			}
		}
	}
}
