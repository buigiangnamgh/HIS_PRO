using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Inventec.Common.Logging;
using Inventec.Common.SignFile.XmlProcess.XmlDsig;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Dsl;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.X509;

namespace Inventec.Common.SignFile
{
	public class SignPdfFile
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass18_0
		{
			public bool isSignElectronic;

			public string inFile;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass19_0
		{
			public bool isSignElectronic;

			public string inFile;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass23_0
		{
			public DisplayConfig displayConfig;

			public string jsonFile;

			public X509Certificate2 cert;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass25_1
		{
			public string jwtData;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass30_0
		{
			public DisplayConfig displayConfig;

			public string reason;

			public X509Certificate2 cert;
		}

        private Org.BouncyCastle.X509.X509Certificate[] chain;

		private string fieldName;

		private byte[] hash;

		internal static string HASH_ALGORITHM_SHA_1 = "SHA1";

		internal static string HASH_ALGORITHM_SHA_256 = "SHA256";

		internal static string HASH_ALGORITHM_SHA_512 = "SHA512";

		private DateTime signDate;

		private string tmpFile;

		private DisplayConfig displayConfigParam;

		public bool SignPDF(X509Certificate2 cert, Stream inStream, string outFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
		{
			string outFilePath = "";
			bool result = false;
			SharedUtils.SaveNewFileFromReader(inStream, ref outFilePath);
			try
			{
				if (File.Exists(outFilePath))
				{
					result = SignPDF(cert, outFilePath, outFile, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, "", isSignElectronic);
				}
				DeleteFileTemp(outFilePath);
				DisposeFileStream(inStream);
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Warn(ex);
			}
			return result;
		}

		public bool SignPDF(X509Certificate2 cert, Stream inStream, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
		{
			string outFilePath = "";
			bool result = false;
			SharedUtils.SaveNewFileFromReader(inStream, ref outFilePath);
			try
			{
				if (File.Exists(outFilePath))
				{
					result = SignPDF(cert, outFilePath, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, isSignElectronic);
				}
				DeleteFileTemp(outFilePath);
				DisposeFileStream(inStream);
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Warn(ex);
			}
			return result;
		}

		public bool SignPDF(X509Certificate2 cert, Stream inStream, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer, bool isSignElectronic = false)
		{
			string outFilePath = "";
			bool result = false;
			SharedUtils.SaveNewFileFromReader(inStream, ref outFilePath);
			try
			{
				if (File.Exists(outFilePath))
				{
					result = SignPDF(cert, outFilePath, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, hasHashPkcsServer, isSignElectronic);
				}
				DeleteFileTemp(outFilePath);
				DisposeFileStream(inStream);
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Warn(ex);
			}
			return result;
		}

		public bool SignPDF(X509Certificate2 cert, byte[] bInFile, string outFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
		{
			bool result = false;
			string outFilePath = "";
			SharedUtils.SaveNewFileFromReader(bInFile, ref outFilePath);
			try
			{
				if (File.Exists(outFilePath))
				{
					result = SignPDF(cert, outFilePath, outFile, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, "", isSignElectronic);
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Warn(ex);
			}
			DeleteFileTemp(outFilePath);
			return result;
		}

		public bool SignPDF(X509Certificate2 cert, byte[] bInFile, byte[] bOutFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
		{
			bool flag = false;
			string outFilePath = "";
			string text = "";
			SharedUtils.SaveNewFileFromReader(bInFile, ref outFilePath);
			try
			{
				if (File.Exists(outFilePath))
				{
					flag = SignPDF(cert, outFilePath, text, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, "", isSignElectronic);
					if (flag && File.Exists(text))
					{
						bOutFile = File.ReadAllBytes(text);
					}
				}
			}
			catch (Exception ex)
			{
				flag = false;
				LogSystem.Warn(ex);
			}
			DeleteFileTemp(outFilePath);
			return flag;
		}

		public bool SignPDF(X509Certificate2 cert, string inFile, string outFile, string reason, string location, TimestampConfig timestampConfig, ref string errMessage, bool isSignElectronic = false)
		{
			try
			{
				return SignPDF(cert, inFile, outFile, reason, location, timestampConfig, null, null, ref errMessage, "", isSignElectronic);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return false;
		}

		public bool SignPDF(X509Certificate2 cert, string inFile, string outFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
		{
			return SignPDF(cert, inFile, outFile, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, "", isSignElectronic);
		}

		public bool SignPDF(X509Certificate2 cert, string inFile, string outFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, string pinCode, bool isSignElectronic = false)
		{
			try
			{
				if (isSignElectronic)
				{
					return SignPDFElectronic(inFile, outFile, reason, location, displayConfig, ref errMessage);
				}
				return SignPDFDigital(cert, inFile, outFile, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, pinCode);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return false;
		}

		public bool SignPDF(X509Certificate2 cert, string inFile, string outFile, string reason, string location, bool useTimestamp, DisplayConfig displayConfig, ref string errMessage, bool isSignElectronic = false)
		{
			try
			{
				if (isSignElectronic)
				{
					return SignPDFElectronic(inFile, outFile, reason, location, displayConfig, ref errMessage);
				}
				return SignPDFDigital(cert, inFile, outFile, reason, location, useTimestamp, displayConfig, ref errMessage);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return false;
		}

		public bool SignPDF(X509Certificate2 cert, string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
		{
			_003C_003Ec__DisplayClass18_0 CS_0024_003C_003E8__locals9 = new _003C_003Ec__DisplayClass18_0();
			CS_0024_003C_003E8__locals9.isSignElectronic = isSignElectronic;
			CS_0024_003C_003E8__locals9.inFile = inFile;
			try
			{
                //LogSystem.Debug("SignPDF____" + LogUtil.TraceData(LogUtil.GetMemberName<bool>((Expression<Func<bool>>)(() => CS_0024_003C_003E8__locals9.isSignElectronic)), (object)CS_0024_003C_003E8__locals9.isSignElectronic) + LogUtil.TraceData(LogUtil.GetMemberName<string>(Expression.Lambda<Func<string>>(Expression.Field(Expression.Constant(CS_0024_003C_003E8__locals9, typeof(_003C_003Ec__DisplayClass18_0)), FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[0])), (object)CS_0024_003C_003E8__locals9.inFile));
				if (CS_0024_003C_003E8__locals9.isSignElectronic)
				{
					return SignPDFElectronic(CS_0024_003C_003E8__locals9.inFile, outStream, reason, location, displayConfig, ref errMessage);
				}
				return SignPDFDigital(cert, CS_0024_003C_003E8__locals9.inFile, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return false;
		}

		public bool SignPDF(X509Certificate2 cert, string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer, bool isSignElectronic = false)
		{
			_003C_003Ec__DisplayClass19_0 CS_0024_003C_003E8__locals9 = new _003C_003Ec__DisplayClass19_0();
			CS_0024_003C_003E8__locals9.isSignElectronic = isSignElectronic;
			CS_0024_003C_003E8__locals9.inFile = inFile;
			try
			{
                //LogSystem.Debug("SignPDF____" + LogUtil.TraceData(LogUtil.GetMemberName<bool>((Expression<Func<bool>>)(() => CS_0024_003C_003E8__locals9.isSignElectronic)), (object)CS_0024_003C_003E8__locals9.isSignElectronic) + LogUtil.TraceData(LogUtil.GetMemberName<string>(Expression.Lambda<Func<string>>(Expression.Field(Expression.Constant(CS_0024_003C_003E8__locals9, typeof(_003C_003Ec__DisplayClass19_0)), FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[0])), (object)CS_0024_003C_003E8__locals9.inFile));
				if (CS_0024_003C_003E8__locals9.isSignElectronic)
				{
					return SignPDFElectronic(CS_0024_003C_003E8__locals9.inFile, outStream, reason, location, displayConfig, ref errMessage);
				}
				return SignPDFDigital(cert, CS_0024_003C_003E8__locals9.inFile, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, hasHashPkcsServer);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return false;
		}

		public bool SignXml(X509Certificate2 cert, byte[] bInFile, ref byte[] bOutFile, XmlConfig xmlConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, string pinCode)
		{
			bool result = false;
			string text = SharedUtils.GenerateTempFile(".xml");
			try
			{
				if (SignXml(cert, bInFile, text, xmlConfig, dlgGetHSMServerResponseData, ref errMessage, pinCode))
				{
					result = true;
					bOutFile = File.ReadAllBytes(text);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			finally
			{
				DeleteFileTemp(text);
			}
			return result;
		}

		public bool SignXml(X509Certificate2 cert, byte[] bInFile, string outFile, XmlConfig xmlConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, string pinCode)
		{
			bool success = false;
			try
			{
				LogSystem.Debug("SignXml. 1____" + LogUtil.TraceData(LogUtil.GetMemberName<XmlConfig>((Expression<Func<XmlConfig>>)(() => xmlConfig)), (object)xmlConfig));
				string outFilePath = "";
				SharedUtils.SaveNewFileFromReader(bInFile, ref outFilePath, ".xml");
				if (!string.IsNullOrEmpty(outFilePath))
				{
					SignDSL signDSL = XmlDsigHelper.Sign(outFilePath).Using(cert).UsingFormat(XmlDsigSignatureFormat.Enveloped)
						.IncludingCertificateInSignature()
						.IncludingHSMServer(dlgGetHSMServerResponseData)
						.IncludeTimestamp(xmlConfig.IncludeTimestamp)
						.NodeToSign(xmlConfig.NodeToSign);
					xmlConfig.SigningTime = ((xmlConfig.SigningTime == DateTime.MinValue) ? DateTime.Now : xmlConfig.SigningTime);
					string text = string.Format("{0:dd/MM/yyyy HH:mm:ss}", (signDate == DateTime.MinValue) ? DateTime.Now : signDate);
					signDSL.WithProperty(string.IsNullOrEmpty(xmlConfig.Reason) ? "SigningTime" : "SigningTimeReason", string.IsNullOrEmpty(xmlConfig.Reason) ? text : string.Format("{0} - {1}", text, xmlConfig.Reason), "http://xades.codeplex.com/#properties");
					signDSL.SignToFile(outFile, pinCode);
					VerificationResults VerificationResults = XmlDsigHelper.Verify(outFile).PerformAndGetResults();
					LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => outFile)), (object)outFile) + LogUtil.TraceData(LogUtil.GetMemberName<VerificationResults>((Expression<Func<VerificationResults>>)(() => VerificationResults)), (object)VerificationResults));
					if (!string.IsNullOrEmpty(outFile) && File.Exists(outFile))
					{
						success = true;
					}
					else
					{
						outFile = "";
					}
					LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<bool>((Expression<Func<bool>>)(() => success)), (object)success) + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => outFile)), (object)outFile));
				}
				LogSystem.Debug("SignXml. 2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return success;
		}

		public bool VerifySignedXml(string xmlSignedFile, ref string signedInfoData, ref string errMessage)
		{
			bool result = false;
			try
			{
				LogSystem.Debug("VerifySignedXml. 1____" + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => xmlSignedFile)), (object)xmlSignedFile));
				if (!string.IsNullOrEmpty(xmlSignedFile))
				{
					VerificationResults verificationResults = XmlDsigHelper.Verify(xmlSignedFile).PerformAndGetResults();
					if (verificationResults != null && verificationResults.SigningCertificate != null && verificationResults.OriginalDocument != null)
					{
						result = true;
                        Org.BouncyCastle.X509.X509Certificate[] x509CertChain = CertUtil.GetX509CertChain(verificationResults.SigningCertificate);
						signedInfoData = string.Format("{0} - {1}", SharedUtils.GetCN(x509CertChain[0]), verificationResults.Timestamp);
					}
				}
				LogSystem.Debug("VerifySignedXml. 2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		public bool SignJson(X509Certificate2 cert, string jsonFile, Stream outStream, string reason, string location, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, string pinCode)
		{
			//IL_0726: Unknown result type (might be due to invalid IL or missing references)
			//IL_072d: Expected O, but got Unknown
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			//IL_079f: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			_003C_003Ec__DisplayClass23_0 CS_0024_003C_003E8__locals29 = new _003C_003Ec__DisplayClass23_0();
			CS_0024_003C_003E8__locals29.displayConfig = displayConfig;
			CS_0024_003C_003E8__locals29.jsonFile = jsonFile;
			CS_0024_003C_003E8__locals29.cert = cert;
			bool result = false;
			try
			{
                Inventec.Common.Logging.LogSystem.Debug("SignJson. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => jsonFile), jsonFile));
				displayConfigParam = CS_0024_003C_003E8__locals29.displayConfig;
				if (!string.IsNullOrEmpty(CS_0024_003C_003E8__locals29.jsonFile))
				{
					string text = (File.Exists(CS_0024_003C_003E8__locals29.jsonFile) ? File.ReadAllText(CS_0024_003C_003E8__locals29.jsonFile) : "");
					string text2 = "";
					JwtSecurityTokenHandler val = new JwtSecurityTokenHandler();
					DateTime utcNow = DateTime.UtcNow;
					string text3 = "";
					string text4 = "";
					if (text.Contains("<SignatureCertificate>") && text.Contains("</SignatureCertificate>"))
					{
						string[] arrDataSigned = text.Split(new string[2] { "<SignatureCertificate>", "</SignatureCertificate>" }, StringSplitOptions.None);
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<string[]>((Expression<Func<string[]>>)(() => arrDataSigned)), (object)arrDataSigned));
						if (arrDataSigned != null && arrDataSigned.Length > 1)
						{
							text4 = arrDataSigned[1].Replace("</SignatureCertificate>", "");
						}
						string[] array = text.Split(new string[2] { "<Signature>", "</Signature>" }, StringSplitOptions.None);
						if (array != null && array.Length != 0)
						{
							text2 = array[0].Replace("</Signature>", "");
						}
					}
					ClaimsPrincipal claimsPrincipal = (string.IsNullOrEmpty(text4) ? null : GetPrincipal(text4, SharedUtils.CertManager.Certificate));
					string text5 = "";
					if (claimsPrincipal != null && claimsPrincipal.Identity != null)
					{
						Claim claim = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
						if (claim != null && !string.IsNullOrEmpty(claim.Value))
						{
							string value = claim.Value;
							text5 = text5 + value + "\r\n";
						}
						Claim claim2 = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
						if (claim2 != null && !string.IsNullOrEmpty(claim2.Value))
						{
							try
							{
								text3 = claim2.Value;
							}
							catch (Exception ex)
							{
								LogSystem.Warn(ex);
							}
						}
						if (!string.IsNullOrEmpty(text3) && !string.IsNullOrEmpty(text2) && text3 != SharedUtils.GetFileContentHash(text2))
						{
							LogSystem.Debug("checkSumSignedFile=" + text3 + ",checkSumUnSignFile=" + SharedUtils.GetFileContentHash(text2) + ",jsonDataRaw=" + SharedUtils.GetFileContentHash(text2));
							errMessage = "Nội dung dữ liệu đã ký không thể sửa đổi";
							return false;
						}
					}
					if (string.IsNullOrEmpty(text3))
					{
						text3 = SharedUtils.GetFileContentHash(text);
					}
					LogSystem.Info("cert is " + ((CS_0024_003C_003E8__locals29.cert != null) ? "not null" : "null"));
					CS_0024_003C_003E8__locals29.displayConfig.SignDate = DateTime.Now;
					DateTime dateTime = CS_0024_003C_003E8__locals29.displayConfig.SignDate;
					string strDate = string.Format(CS_0024_003C_003E8__locals29.displayConfig.DateFormatstring, dateTime);
					if ("".Equals(CS_0024_003C_003E8__locals29.displayConfig.Contact) && CS_0024_003C_003E8__locals29.cert != null)
					{
                        Org.BouncyCastle.X509.X509Certificate[] x509CertChain = CertUtil.GetX509CertChain(CS_0024_003C_003E8__locals29.cert);
						CS_0024_003C_003E8__locals29.displayConfig.Contact = SharedUtils.GetCN(x509CertChain[0]);
					}
					CS_0024_003C_003E8__locals29.displayConfig.Reason = reason;
					text5 += SignPdfAsynchronous.GetDisplayText(CS_0024_003C_003E8__locals29.displayConfig, strDate).Replace("\r\n", " - ");
					string value2 = "";
					byte[] array2 = EncodeData(SharedUtils.FileToByte(CS_0024_003C_003E8__locals29.jsonFile), SignPdfAsynchronous.HASH_ALG);
					if (dlgGetHSMServerResponseData != null)
					{
						string hashData = ByteArrayToHexString(array2);
						try
						{
							value2 = dlgGetHSMServerResponseData(hashData, ref errMessage);
						}
						catch
						{
						}
					}
					else if (CS_0024_003C_003E8__locals29.cert != null && CS_0024_003C_003E8__locals29.cert.PrivateKey != null)
					{
						RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)CS_0024_003C_003E8__locals29.cert.PrivateKey;
						LogSystem.Debug((!string.IsNullOrEmpty(pinCode)) ? "pinCode has set" : "pinCode not set");
						if (!string.IsNullOrEmpty(pinCode))
						{
							LogSystem.Debug("pinCode has set");
							try
							{
								string preSerialNumber = KeyStore.GetValue("SERIALNUMBER");
								if (!string.IsNullOrEmpty(preSerialNumber) && preSerialNumber != CS_0024_003C_003E8__locals29.cert.SerialNumber)
								{
									KeyStore.SetValue("CHANGE_USB", "1");
									pinCode = "";
									LogSystem.Info("check preSerialNumber => 1");
								}
								else
								{
									KeyStore.SetValue("CHANGE_USB", "0");
									LogSystem.Info("check preSerialNumber => 2");
								}
								LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => preSerialNumber)), (object)preSerialNumber) + "____" + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => CS_0024_003C_003E8__locals29.cert.SerialNumber)), (object)CS_0024_003C_003E8__locals29.cert.SerialNumber));
								CspParameters cspParameters = new CspParameters();
								cspParameters.KeyContainerName = rSACryptoServiceProvider.CspKeyContainerInfo.KeyContainerName;
								cspParameters.ProviderName = rSACryptoServiceProvider.CspKeyContainerInfo.ProviderName;
								cspParameters.ProviderType = rSACryptoServiceProvider.CspKeyContainerInfo.ProviderType;
								cspParameters.KeyPassword = SharedUtils.GetSecurePin(pinCode);
								LogSystem.Info(LogUtil.TraceData("cspp", (object)cspParameters));
								RSACryptoServiceProvider rSACryptoServiceProvider2 = new RSACryptoServiceProvider(cspParameters);
								KeyStore.SetValue("SERIALNUMBER", CS_0024_003C_003E8__locals29.cert.SerialNumber);
							}
							catch (Exception ex2)
							{
								LogSystem.Warn("PIN code Cached for next access to the smart card fail: " + ex2.Message);
							}
						}
						string s = Convert.ToBase64String(array2);
						new SHA1Managed();
						LogSystem.Debug("SignJson.1.1");
						value2 = Convert.ToBase64String(rSACryptoServiceProvider.SignHash(Convert.FromBase64String(s), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));
						LogSystem.Debug("SignJson.1.2");
					}
					SecurityTokenDescriptor val2 = new SecurityTokenDescriptor();
					val2.Subject = new ClaimsIdentity(new Claim[3]
					{
						new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", text3),
						new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", text5),
						new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata", value2)
					});
					val2.Expires = utcNow.AddYears(10);
					val2.SigningCredentials = new SigningCredentials((SecurityKey)(object)GetRsaKey(SharedUtils.CertManager.Certificate), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");
					SecurityTokenDescriptor val3 = val2;
					SecurityToken val4 = ((SecurityTokenHandler)val).CreateToken(val3);
					string text6 = ((SecurityTokenHandler)val).WriteToken(val4);
					if (!string.IsNullOrEmpty(text6))
					{
						result = true;
						StreamWriter streamWriter = new StreamWriter(outStream);
						string text7 = "";
						if (text.Contains("<Signature>") && text.Contains("</Signature>"))
						{
							string text8 = text.Substring(0, text.IndexOf("<Signature>"));
							text7 = text8;
						}
						else
						{
							text7 = text;
						}
						string text9 = ConfigurationManager.AppSettings["Common.SignFile.SecretKeyEncrypt"] ?? "";
						string text10 = ((!string.IsNullOrEmpty(text9)) ? string.Format("<SecretKey>{0}</SecretKey>", text9) : "");
						text7 += string.Format("<Signature><SignatureCertificate>{0}</SignatureCertificate><SignatureDisplayInfo>{1}</SignatureDisplayInfo><Checksum>{2}</Checksum>{3}</Signature>", text6, text5, text3, text10);
						streamWriter.Write(text7);
						streamWriter.Flush();
						outStream.Position = 0L;
					}
				}
				LogSystem.Debug("SignJson. 2");
			}
			catch (Exception ex3)
			{
				LogSystem.Warn(ex3);
			}
			return result;
		}

		public bool VerifySignedJson(X509Certificate2 cert, string jsonSignedData, ref string signedInfoData, ref string errMessage)
		{
			bool result = false;
			try
			{
				LogSystem.Debug("SignJson. 1____" + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => jsonSignedData)), (object)jsonSignedData));
				if (!string.IsNullOrEmpty(jsonSignedData))
				{
					ClaimsPrincipal principal = GetPrincipal(jsonSignedData, cert);
					if (principal == null)
					{
						errMessage = "Verify thất bại";
					}
					else
					{
						errMessage = "";
						errMessage = "";
						Claim claim = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
						if (claim != null && !string.IsNullOrEmpty(claim.Value))
						{
							string value = claim.Value;
							signedInfoData += value;
						}
					}
				}
				LogSystem.Debug("SignJson. 2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		public bool VerifySignedJson(string jsonSignedData, GetHSMServerResponseData dlgGetHSMServerVerifyData, ref string signedInfoData, ref string errMessage)
		{
			bool result = false;
			try
			{
				LogSystem.Debug("SignJson. 1____" + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => jsonSignedData)), (object)jsonSignedData));
				if (!string.IsNullOrEmpty(jsonSignedData))
				{
					_003C_003Ec__DisplayClass25_1 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass25_1();
					CS_0024_003C_003E8__locals8.jwtData = "";
					if (jsonSignedData.Contains("<SignatureCertificate>") && jsonSignedData.Contains("</SignatureCertificate>"))
					{
						string[] arrDataSigned = jsonSignedData.Split(new string[2] { "<SignatureCertificate>", "</SignatureCertificate>" }, StringSplitOptions.None);
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<string[]>((Expression<Func<string[]>>)(() => arrDataSigned)), (object)arrDataSigned));
						if (arrDataSigned != null && arrDataSigned.Length > 1)
						{
							CS_0024_003C_003E8__locals8.jwtData = arrDataSigned[1].Replace("</SignatureCertificate>", "");
						}
					}
					ClaimsPrincipal claimsPrincipal = ((!string.IsNullOrEmpty(CS_0024_003C_003E8__locals8.jwtData)) ? GetPrincipal(CS_0024_003C_003E8__locals8.jwtData, SharedUtils.CertManager.Certificate) : null);
					if (claimsPrincipal == null)
					{
						errMessage = "Verify thất bại";
                        //LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => CS_0024_003C_003E8__locals8.jwtData)), (object)CS_0024_003C_003E8__locals8.jwtData) + "____" + LogUtil.TraceData(LogUtil.GetMemberName<string>(Expression.Lambda<Func<string>>(Expression.Field(Expression.Constant(CS_0024_003C_003E8__locals8, typeof(_003C_003Ec__DisplayClass25_1)), FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[0])), (object)CS_0024_003C_003E8__locals8.jwtData));
					}
					else
					{
						if (dlgGetHSMServerVerifyData != null)
						{
						}
						errMessage = "";
						errMessage = "";
						Claim claim = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
						if (claim != null && !string.IsNullOrEmpty(claim.Value))
						{
							string value = claim.Value;
							signedInfoData += value;
							result = true;
						}
					}
				}
				LogSystem.Debug("SignJson. 2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		internal bool SignJson2(X509Certificate2 cert, string jsonData, Stream outStream, string reason, string location, DisplayConfig displayConfig, ref string errMessage)
		{
			bool result = false;
			try
			{
				LogSystem.Debug("SignJson. 1____" + LogUtil.TraceData(LogUtil.GetMemberName<DisplayConfig>((Expression<Func<DisplayConfig>>)(() => displayConfig)), (object)displayConfig));
				displayConfigParam = displayConfig;
				if (!string.IsNullOrEmpty(jsonData))
				{
					RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)cert.PrivateKey;
					SHA1Managed sHA1Managed = new SHA1Managed();
					UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
					byte[] bytes = unicodeEncoding.GetBytes(jsonData);
					byte[] rgbHash = sHA1Managed.ComputeHash(bytes);
					byte[] array = rSACryptoServiceProvider.SignHash(rgbHash, CryptoConfig.MapNameToOID("SHA1"));
					if (array != null && array.Length != 0)
					{
						outStream = new MemoryStream(array);
						outStream.Position = 0L;
					}
				}
				LogSystem.Debug("SignJson. 2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		internal bool VerifySignedJson2(X509Certificate2 cert, string jsonSignedFile, ref string errMessage)
		{
			bool result = false;
			try
			{
				LogSystem.Debug("SignJson. 1____" + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => jsonSignedFile)), (object)jsonSignedFile));
				if (!string.IsNullOrEmpty(jsonSignedFile))
				{
					string jsonDataSigned = File.ReadAllText(jsonSignedFile);
					ClaimsPrincipal principal = GetPrincipal(jsonDataSigned, cert);
					if (principal == null)
					{
						errMessage = "Verify thất bại";
					}
					else
					{
						errMessage = "OK";
					}
				}
				LogSystem.Debug("SignJson. 2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		internal bool SignPDFElectronic(string inFile, string outFile, string reason, string location, DisplayConfig displayConfig, ref string errMessage)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Expected O, but got Unknown
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Expected O, but got Unknown
			//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c6: Expected O, but got Unknown
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Expected O, but got Unknown
			//IL_0463: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Expected O, but got Unknown
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fc: Expected O, but got Unknown
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Expected O, but got Unknown
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Expected O, but got Unknown
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Expected O, but got Unknown
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Expected O, but got Unknown
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a2: Expected O, but got Unknown
			//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ff: Expected O, but got Unknown
			//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0706: Expected O, but got Unknown
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05de: Expected O, but got Unknown
			bool result = false;
			try
			{
				LogSystem.Debug("SignPDFElectronic. 1____" + LogUtil.TraceData(LogUtil.GetMemberName<DisplayConfig>((Expression<Func<DisplayConfig>>)(() => displayConfig)), (object)displayConfig));
				displayConfig.Reason = reason;
				displayConfigParam = displayConfig;
				PdfReader val = new PdfReader(inFile);
				int numberOfPages = val.NumberOfPages;
				if (!string.IsNullOrEmpty(displayConfig.Contact))
				{
					displayConfig.Contact = displayConfig.Contact.ToUpper();
				}
				Image val2 = null;
				if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT || displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
				{
					if (!string.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
					{
						val2 = Image.GetInstance(displayConfig.PathImage);
					}
					else if (displayConfig.BImage != null)
					{
						val2 = Image.GetInstance(displayConfig.BImage);
					}
				}
				float num = displayConfig.CoorXRectangle - displayConfig.WidthRectangle / 2f;
				float coorYRectangle = displayConfig.CoorYRectangle;
				float widthRectangle = displayConfig.WidthRectangle;
				float heightRectangle = displayConfig.HeightRectangle;
				string strDate = string.Format(displayConfig.DateFormatstring, (displayConfig.SignDate == DateTime.MinValue) ? DateTime.Now : displayConfig.SignDate);
				SignPdfAsynchronous.ProcessFontSizeFit(displayConfig);
				using (FileStream fileStream = File.Open(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
				{
					PdfStamper val3 = new PdfStamper(val, (Stream)fileStream);
					try
					{
						for (int num2 = 1; num2 <= numberOfPages; num2++)
						{
							if (num2 != displayConfig.NumberPageSign)
							{
								continue;
							}
							Rectangle pageSize = val.GetPageSize(num2);
							PdfContentByte overContent = val3.GetOverContent(num2);
							LogSystem.Debug("SignPDFElectronic____PageNUm=" + displayConfig.NumberPageSign + "__WidthRectangle=" + displayConfig.WidthRectangle + "__HeightRectangle=" + displayConfig.HeightRectangle + "__CoorXRectangle=" + displayConfig.CoorXRectangle + "__CoorYRectangle=" + displayConfig.CoorYRectangle);
							result = true;
							if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP && val2 != null)
							{
								PdfPCell val4 = new PdfPCell();
								if (val2 != null)
								{
									val2.Alignment = 1;
									float plusH = SignPdfAsynchronous.ProcessHeightPlus(100f, displayConfig);
									val2.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, val2, displayConfig.SignaltureImageWidth, 100f, plusH);
									val4.AddElement((IElement)(object)val2);
									val4.HorizontalAlignment = 1;
									val4.VerticalAlignment = 5;
									((Rectangle)val4).Border = 0;
									val4.MinimumHeight = heightRectangle;
								}
								PdfPTable val5 = new PdfPTable(1);
								val5.TotalWidth = widthRectangle;
								val5.LockedWidth = true;
								val5.AddCell(val4);
								val5.WriteSelectedRows(0, -1, num, coorYRectangle + val5.TotalHeight / 2f, overContent);
							}
							else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
							{
								float totalWidth = widthRectangle;
								string displayText = SignPdfAsynchronous.GetDisplayText(displayConfig, strDate);
								float widthImagePercent = 0f;
								PdfPTable val6 = null;
								if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100)
								{
									val6 = new PdfPTable(1);
									widthImagePercent = 100f;
								}
								else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75)
								{
									val6 = new PdfPTable(new float[2] { 25f, 75f });
									widthImagePercent = 25f;
								}
								else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70)
								{
									val6 = new PdfPTable(new float[2] { 30f, 70f });
									widthImagePercent = 30f;
								}
								else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60)
								{
									val6 = new PdfPTable(new float[2] { 40f, 60f });
									widthImagePercent = 40f;
								}
								else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50)
								{
									val6 = new PdfPTable(new float[2] { 50f, 50f });
									widthImagePercent = 50f;
								}
								else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x60x40)
								{
									val6 = new PdfPTable(new float[2] { 60f, 40f });
									widthImagePercent = 40f;
								}
								else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x70x30)
								{
									val6 = new PdfPTable(new float[2] { 70f, 30f });
									widthImagePercent = 30f;
								}
								else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x75x25)
								{
									val6 = new PdfPTable(new float[2] { 75f, 25f });
									widthImagePercent = 25f;
								}
								float plusH2 = SignPdfAsynchronous.ProcessHeightPlus(widthImagePercent, displayConfig);
								PdfPCell val7 = new PdfPCell();
								if (val2 != null)
								{
									val2.Alignment = 1;
									val2.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, val2, displayConfig.SignaltureImageWidth, widthImagePercent, plusH2);
									val7.AddElement((IElement)(object)val2);
									val7.HorizontalAlignment = 1;
									val7.VerticalAlignment = 5;
									((Rectangle)val7).Border = 0;
								}
								PdfPCell textCell = GetTextCell(displayText, displayConfig);
								val6.TotalWidth = totalWidth;
								val6.LockedWidth = true;
								if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100 || displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75 || displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70 || displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60 || displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50)
								{
									val6.AddCell(val7);
									val6.AddCell(textCell);
								}
								else
								{
									val6.AddCell(textCell);
									val6.AddCell(val7);
								}
								PdfPTable val8 = new PdfPTable(1);
								PdfPCell val9 = new PdfPCell();
								val9.AddElement((IElement)(object)val6);
								val9.HorizontalAlignment = 1;
								val9.VerticalAlignment = 5;
								((Rectangle)val9).Border = 0;
								val9.MinimumHeight = heightRectangle;
								val8.TotalWidth = totalWidth;
								val8.LockedWidth = true;
								val8.AddCell(val9);
								val8.WriteSelectedRows(0, -1, num, coorYRectangle + val8.TotalHeight / 2f, overContent);
							}
							else if (displayConfig.TypeDisplay == Constans.DISPLAY_RECTANGLE_TEXT)
							{
								float totalWidth2 = widthRectangle;
								string displayText2 = SignPdfAsynchronous.GetDisplayText(displayConfig, strDate);
								PdfPCell textCell2 = GetTextCell(displayText2, displayConfig);
								textCell2.MinimumHeight = heightRectangle;
								PdfPTable val10 = new PdfPTable(1);
								val10.TotalWidth = totalWidth2;
								val10.LockedWidth = true;
								val10.HorizontalAlignment = 1;
								val10.AddCell(textCell2);
								val10.CompleteRow();
								val10.WriteSelectedRows(0, -1, num, coorYRectangle + val10.TotalHeight / 2f, overContent);
							}
						}
					}
					finally
					{
						if (val3 != null)
						{
							((IDisposable)val3).Dispose();
						}
					}
				}
				val.Close();
				LogSystem.Debug("SignPDFElectronic. 2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		internal bool SignPDFElectronic(string inFile, Stream outStream, string reason, string location, DisplayConfig displayConfig, ref string errMessage)
		{
			bool flag = false;
			try
			{
				string text = SharedUtils.GenerateTempFile();
				flag = SignPDFElectronic(inFile, text, reason, location, displayConfig, ref errMessage);
				if (flag && File.Exists(text))
				{
					byte[] buffer = SharedUtils.FileToByte(text);
					MemoryStream memoryStream = new MemoryStream(buffer);
					memoryStream.CopyTo(outStream);
					outStream.Position = 0L;
				}
				LogSystem.Debug("outStream!=null" + (outStream != null) + ",outStream.length = " + ((outStream != null) ? outStream.Length : 0) + ",outStream.Position = " + ((outStream != null) ? outStream.Position : 0));
				DeleteFileTemp(text);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return flag;
		}

		internal bool SignPDFDigital(X509Certificate2 cert, string inFile, string outFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, string pinCode)
		{
			_003C_003Ec__DisplayClass30_0 CS_0024_003C_003E8__locals17 = new _003C_003Ec__DisplayClass30_0();
			CS_0024_003C_003E8__locals17.displayConfig = displayConfig;
			CS_0024_003C_003E8__locals17.reason = reason;
			CS_0024_003C_003E8__locals17.cert = cert;
			try
			{
                Inventec.Common.Logging.LogSystem.Debug("SignPDFDigital. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => reason), reason));
				displayConfigParam = CS_0024_003C_003E8__locals17.displayConfig;
				CertUtil.GetX509Cert(CS_0024_003C_003E8__locals17.cert);
                Org.BouncyCastle.X509.X509Certificate[] x509CertChain = CertUtil.GetX509CertChain(CS_0024_003C_003E8__locals17.cert);
				string text = "";
				byte[] hashTypeRectangleText = GetHashTypeRectangleText(inFile, x509CertChain, CS_0024_003C_003E8__locals17.reason, location, CS_0024_003C_003E8__locals17.displayConfig.IsDisplaySignNote);
				if (dlgGetHSMServerResponseData != null)
				{
					string text2 = ByteArrayToHexString(hashTypeRectangleText);
					string text3 = Convert.ToBase64String(hashTypeRectangleText);
					LogSystem.Debug("SignPDFDigital.1.0____" + LogUtil.TraceData("ByteArrayToHexString.s", (object)text2));
					LogSystem.Debug("SignPDFDigital.1.0____" + LogUtil.TraceData("base64S", (object)text3));
					text = dlgGetHSMServerResponseData(text2, ref errMessage);
					LogSystem.Debug("SignPDFDigital.1.0____" + LogUtil.TraceData("extSig", (object)text));
				}
				else
				{
					RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)CS_0024_003C_003E8__locals17.cert.PrivateKey;
					LogSystem.Debug((!string.IsNullOrEmpty(pinCode)) ? "pinCode has set" : "pinCode not set");
					if (!string.IsNullOrEmpty(pinCode))
					{
						LogSystem.Debug("pinCode has set");
						try
						{
							string preSerialNumber = KeyStore.GetValue("SERIALNUMBER");
							if (!string.IsNullOrEmpty(preSerialNumber) && preSerialNumber != CS_0024_003C_003E8__locals17.cert.SerialNumber)
							{
								KeyStore.SetValue("CHANGE_USB", "1");
								pinCode = "";
								LogSystem.Info("check preSerialNumber => 1");
							}
							else
							{
								KeyStore.SetValue("CHANGE_USB", "0");
								LogSystem.Info("check preSerialNumber => 2");
							}
							LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => preSerialNumber)), (object)preSerialNumber) + "____" + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => CS_0024_003C_003E8__locals17.cert.SerialNumber)), (object)CS_0024_003C_003E8__locals17.cert.SerialNumber));
							CspParameters cspParameters = new CspParameters();
							cspParameters.KeyContainerName = rSACryptoServiceProvider.CspKeyContainerInfo.KeyContainerName;
							cspParameters.ProviderName = rSACryptoServiceProvider.CspKeyContainerInfo.ProviderName;
							cspParameters.ProviderType = rSACryptoServiceProvider.CspKeyContainerInfo.ProviderType;
							cspParameters.KeyPassword = SharedUtils.GetSecurePin(pinCode);
							LogSystem.Info(LogUtil.TraceData("cspp", (object)cspParameters));
							RSACryptoServiceProvider rSACryptoServiceProvider2 = new RSACryptoServiceProvider(cspParameters);
							KeyStore.SetValue("SERIALNUMBER", CS_0024_003C_003E8__locals17.cert.SerialNumber);
						}
						catch (Exception ex)
						{
							LogSystem.Warn("PIN code Cached for next access to the smart card fail: " + ex.Message);
						}
					}
					string s = Convert.ToBase64String(hashTypeRectangleText);
					new SHA1Managed();
					LogSystem.Debug("SignPDFDigital.1.1");
					text = Convert.ToBase64String(rSACryptoServiceProvider.SignHash(Convert.FromBase64String(s), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));
					LogSystem.Debug("SignPDFDigital.1.2");
				}
				if (!string.IsNullOrEmpty(text))
				{
					LogSystem.Debug("SignPDFDigital.1.3");
					InsertSignature(text, outFile, timestampConfig);
					LogSystem.Debug("SignPDFDigital.1.4");
					return true;
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
				MessageBox.Show("Chứng thư không hợp lệ, vui lòng chọn lại");
			}
			return false;
		}

		internal bool SignPDFDigital(X509Certificate2 cert, string inFile, string outFile, string reason, string location, bool useTimestamp, DisplayConfig displayConfig, ref string errMessage)
		{
			try
			{
				CertUtil.GetX509Cert(cert);
                Org.BouncyCastle.X509.X509Certificate[] x509CertChain = CertUtil.GetX509CertChain(cert);
				displayConfigParam = displayConfig;
				string text = "";
				byte[] hashTypeRectangleText = GetHashTypeRectangleText(inFile, x509CertChain, reason, location, displayConfig.IsDisplaySignNote);
				string s = Convert.ToBase64String(hashTypeRectangleText);
				new SHA1Managed();
				text = Convert.ToBase64String(((RSACryptoServiceProvider)cert.PrivateKey).SignHash(Convert.FromBase64String(s), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));
				if (!string.IsNullOrEmpty(text))
				{
					TimestampConfig timestampConfig = new TimestampConfig();
					timestampConfig.UseTimestamp = true;
					timestampConfig.TsaUrl = "http://ca.gov.vn/tsa";
					InsertSignature(text, outFile, timestampConfig);
					return true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return false;
		}

		internal bool SignPDFDigital(X509Certificate2 cert, string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage)
		{
			try
			{
				return SignPDFDigital(cert, inFile, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, false);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return false;
		}

		internal bool SignPDFDigital(X509Certificate2 cert, string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer)
		{
			bool result = false;
			try
			{
				CertUtil.GetX509Cert(cert);
                Org.BouncyCastle.X509.X509Certificate[] x509CertChain = CertUtil.GetX509CertChain(cert);
				displayConfigParam = displayConfig;
				if (hasHashPkcsServer)
				{
					result = EmptySignatureHashPkcsServer(inFile, outStream, SharedUtils.getSignName(), reason, location, x509CertChain[0], dlgGetHSMServerResponseData, ref errMessage);
				}
				else
				{
					string text = "";
					byte[] hashTypeRectangleText = GetHashTypeRectangleText(inFile, x509CertChain, reason, location, displayConfig.IsDisplaySignNote);
					if (dlgGetHSMServerResponseData != null)
					{
						string text2 = ByteArrayToHexString(hashTypeRectangleText);
						text = dlgGetHSMServerResponseData(text2, ref errMessage);
						string text3 = Convert.ToBase64String(hashTypeRectangleText);
						LogSystem.Debug("SignPDFDigital.1.0____" + LogUtil.TraceData("hexHash", (object)text2));
						LogSystem.Debug("SignPDFDigital.1.0____" + LogUtil.TraceData("base64S", (object)text3));
						LogSystem.Debug("SignPDFDigital.1.0____" + LogUtil.TraceData("extSig", (object)text));
						if (string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(errMessage))
						{
							LogSystem.Debug(LogUtil.TraceData("errMessage", (object)errMessage) + "____extSig!=null=" + (text != null));
						}
					}
					else
					{
						string s = Convert.ToBase64String(hashTypeRectangleText);
						new SHA1Managed();
						text = Convert.ToBase64String(((RSACryptoServiceProvider)cert.PrivateKey).SignHash(Convert.FromBase64String(s), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));
					}
					if (!string.IsNullOrEmpty(text))
					{
						InsertSignature(text, outStream, timestampConfig);
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		public string GetHashSignalture(X509Certificate2 cert, Stream inStream, Stream outStream, string reason, string location, DisplayConfig displayConfig, ref string errMessage, ref byte[] hashRaw, ref string fieldSignName)
		{
			string result = string.Empty;
			try
			{
				string outFilePath = "";
				SharedUtils.SaveNewFileFromReader(inStream, ref outFilePath);
				CertUtil.GetX509Cert(cert);
                Org.BouncyCastle.X509.X509Certificate[] x509CertChain = CertUtil.GetX509CertChain(cert);
				displayConfigParam = displayConfig;
				byte[] hashTypeRectangleText = GetHashTypeRectangleText(outFilePath, x509CertChain, reason, location, displayConfig.IsDisplaySignNote);
				result = ((hashTypeRectangleText != null && hashTypeRectangleText.Length != 0) ? Convert.ToBase64String(hashTypeRectangleText) : string.Empty);
				if (!string.IsNullOrEmpty(tmpFile) && File.Exists(tmpFile))
				{
					using (FileStream fileStream = new FileStream(tmpFile, FileMode.Open, FileAccess.Read))
					{
						fileStream.CopyTo(outStream);
						outStream.Position = 0L;
					}
				}
				hashRaw = hash;
				fieldSignName = fieldName;
				DeleteFileTemp(outFilePath);
				DeleteFileTemp(tmpFile);
			}
			catch (Exception ex)
			{
				errMessage = "Lỗi khởi tạo chuỗi hash phục vụ tạo chữ ký: " + ex.Message;
				LogSystem.Warn(ex);
			}
			return result;
		}

		public bool InsertSignatureIntegrate(X509Certificate2 cert, Stream inStream, Stream outStream, string signature, string fieldSignName, DisplayConfig displayConfig, TimestampConfig timestampConfig, byte[] hashRaw, ref string errMessage)
		{
			bool result = false;
			try
			{
				string outFilePath = "";
				SharedUtils.SaveNewFileFromReader(inStream, ref outFilePath);
				if (string.IsNullOrEmpty(outFilePath))
				{
					errMessage = "File đầu vào không hợp lệ.";
					return false;
				}
				if (string.IsNullOrEmpty(signature))
				{
					errMessage = "Chữ ký đầu vào không hợp lệ.";
					return false;
				}
				if (string.IsNullOrEmpty(fieldSignName) || hashRaw == null)
				{
					errMessage = "Dữ liệu đầu vào không hợp lệ.";
					return false;
				}
				tmpFile = outFilePath;
				fieldName = fieldSignName;
				DateTime dateTime = displayConfig.SignDate;
				signDate = displayConfig.SignDate;
				chain = CertUtil.GetX509CertChain(cert);
				hash = hashRaw;
				displayConfigParam = displayConfig;
				result = InsertSignature(signature, outStream, timestampConfig);
				try
				{
					File.Delete(outFilePath);
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
			}
			catch (Exception ex2)
			{
				errMessage = "Có lỗi trong quá trình chèn chữ ký vào file pdf.";
				LogSystem.Warn(ex2);
			}
			return result;
		}

		public bool SignPDFAllPage(X509Certificate2 cert, Stream inStream, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage)
		{
			string outFilePath = "";
			bool result = false;
			SharedUtils.SaveNewFileFromReader(inStream, ref outFilePath);
			try
			{
				if (File.Exists(outFilePath))
				{
					result = SignPDFDigitalAllPage(cert, ref outFilePath, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage);
				}
				DeleteFileTemp(outFilePath);
				DisposeFileStream(inStream);
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Warn(ex);
			}
			return result;
		}

		public bool SignPDFAllPage(X509Certificate2 cert, Stream inStream, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer, bool isAutoChangeSignLocation = false)
		{
			string outFilePath = "";
			bool result = false;
			SharedUtils.SaveNewFileFromReader(inStream, ref outFilePath);
			try
			{
				if (File.Exists(outFilePath))
				{
					result = SignPDFDigitalAllPage(cert, ref outFilePath, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, hasHashPkcsServer, isAutoChangeSignLocation);
				}
				DeleteFileTemp(outFilePath);
				DisposeFileStream(inStream);
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Warn(ex);
			}
			return result;
		}

		internal bool SignPDFDigitalAllPage(X509Certificate2 cert, ref string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer = false, bool isAutoChangeSignLocation = false)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			PdfReader val = null;
			try
			{
				LogSystem.Debug("SignPDFDigitalAllPage. 1____" + LogUtil.TraceData(LogUtil.GetMemberName<DisplayConfig>((Expression<Func<DisplayConfig>>)(() => displayConfig)), (object)displayConfig));
				val = new PdfReader(inFile);
				int numberOfPages = val.NumberOfPages;
                Org.BouncyCastle.X509.X509Certificate[] x509CertChain = CertUtil.GetX509CertChain(cert);
				for (int num = 1; num <= numberOfPages; num++)
				{
					fieldName = "";
					signDate = DateTime.Now;
					tmpFile = "";
					hash = null;
					chain = null;
					float num2 = displayConfig.CoorXRectangle;
					float num3 = displayConfig.CoorYRectangle;
					if (isAutoChangeSignLocation)
					{
						Rectangle pageSizeWithRotation = val.GetPageSizeWithRotation(num);
						num2 = displayConfig.CoorXRectangle / PageSize.A4.Width * pageSizeWithRotation.Width;
						num3 = displayConfig.CoorYRectangle / PageSize.A4.Height * pageSizeWithRotation.Height;
						if (num2 <= displayConfig.WidthRectangle / 2f)
						{
							num2 = displayConfig.WidthRectangle / 2f;
						}
						else if (num2 >= pageSizeWithRotation.Width - displayConfig.WidthRectangle / 2f)
						{
							num2 = pageSizeWithRotation.Width - displayConfig.WidthRectangle / 2f;
						}
						if (num3 <= displayConfig.HeightRectangle / 2f)
						{
							num3 = displayConfig.HeightRectangle / 2f;
						}
						else if (num3 >= pageSizeWithRotation.Height - displayConfig.HeightRectangle / 2f)
						{
							num3 = pageSizeWithRotation.Height - displayConfig.HeightRectangle / 2f;
						}
					}
					using (MemoryStream memoryStream = new MemoryStream())
					{
						if (hasHashPkcsServer)
						{
							bool flag = EmptySignatureHashPkcsServer(inFile, memoryStream, SharedUtils.getSignName(), reason, location, x509CertChain[0], dlgGetHSMServerResponseData, ref errMessage);
						}
						else
						{
							DisplayConfig displayConfig2 = DisplayConfig.generateDisplayConfigRectangleText(displayConfig.TypeDisplay, displayConfig.SizeFont, displayConfig.TextPosition, displayConfig.PathImage, displayConfig.BImage, num, num2, num3, (displayConfig.WidthRectangle > 0f) ? displayConfig.WidthRectangle : 320f, (displayConfig.HeightRectangle > 0f) ? displayConfig.HeightRectangle : 140f, null, (!string.IsNullOrEmpty(displayConfig.FormatRectangleText)) ? displayConfig.FormatRectangleText : Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(x509CertChain[0]), (!string.IsNullOrEmpty(displayConfig.Reason)) ? displayConfig.Reason : reason, (!string.IsNullOrEmpty(displayConfig.Location)) ? displayConfig.Location : location, "{0:dd/MM/yyyy HH:mm:ss}");
							displayConfig2.IsDisplaySignature = displayConfig.IsDisplaySignature;
							displayConfig2.SignaltureImageWidth = displayConfig.SignaltureImageWidth;
							displayConfig2.TotalPageSign = numberOfPages;
							displayConfig2.TextFormat = displayConfig.TextFormat;
							string text = "";
							byte[] array = CreateHash(inFile, x509CertChain, displayConfig2);
							if (dlgGetHSMServerResponseData != null)
							{
								string hashData = (hasHashPkcsServer ? ByteArrayToHexString(hash) : ByteArrayToHexString(array));
								text = dlgGetHSMServerResponseData(hashData, ref errMessage);
								if (string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(errMessage))
								{
									LogSystem.Debug(LogUtil.TraceData("errMessage", (object)errMessage) + "____extSig!=null=" + (text != null));
								}
							}
							else
							{
								string s = Convert.ToBase64String(array);
								new SHA1Managed();
								text = Convert.ToBase64String(((RSACryptoServiceProvider)cert.PrivateKey).SignHash(Convert.FromBase64String(s), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));
							}
							if (string.IsNullOrEmpty(text) || !InsertSignature(text, memoryStream, timestampConfig))
							{
								return false;
							}
						}
						DeleteFileTemp(inFile);
						memoryStream.Position = 0L;
						if (num == numberOfPages)
						{
							memoryStream.CopyTo(outStream);
							continue;
						}
						inFile = SharedUtils.GenerateTempFile();
						using (Stream destination = File.Open(inFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
						{
							memoryStream.CopyTo(destination);
						}
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			finally
			{
				if (val != null)
				{
					val.Close();
					val.Dispose();
				}
			}
			return false;
		}

		internal static PdfPCell GetTextCell(string displayText, DisplayConfig displayConfig)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			int num = 1;
			if (displayConfig.TextFormat != null)
			{
				switch (displayConfig.TextFormat.Alignment)
				{
				case ALIGNMENT_OPTION.Left:
					num = 0;
					break;
				case ALIGNMENT_OPTION.Center:
					num = 1;
					break;
				case ALIGNMENT_OPTION.Right:
					num = 2;
					break;
				case ALIGNMENT_OPTION.Justify:
					num = 3;
					break;
				default:
					num = 1;
					break;
				}
			}
			Font fontByConfig = GetFontByConfig(displayConfig);
			Paragraph val = new Paragraph(displayText, fontByConfig)
			{
				Alignment = num
			};
			PdfPCell val2 = new PdfPCell((Phrase)(object)val);
			val2.HorizontalAlignment = num;
			val2.VerticalAlignment = 4;
			((Rectangle)val2).Border = 0;
			return val2;
		}

		internal static Font GetFontByConfig(DisplayConfig displayConfig)
		{
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Expected O, but got Unknown
			int num = 0;
			string text = null;
			if (displayConfig.TextFormat != null)
			{
				text = (displayConfig.TextFormat.FontName ?? "").ToLower();
				if (displayConfig.TextFormat.IsBold)
				{
					num++;
				}
				if (displayConfig.TextFormat.IsItalic)
				{
					num += 2;
				}
				if (displayConfig.TextFormat.IsUnderlined)
				{
					num += 4;
				}
			}
			BaseFont val = null;
			if (!string.IsNullOrWhiteSpace(text))
			{
				try
				{
					string text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), string.Format("{0}.ttf", text));
					val = BaseFont.CreateFont(text2, "Identity-H", true) ?? BaseFont.CreateFont(displayConfig.FontPath, "Identity-H", true);
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
					val = BaseFont.CreateFont(displayConfig.FontPath, "Identity-H", true);
				}
			}
			else
			{
				val = BaseFont.CreateFont(displayConfig.FontPath, "Identity-H", true);
			}
			return new Font(val, (float)displayConfig.SizeFont, num);
		}

		private ClaimsPrincipal GetPrincipal(string jsonDataSigned, X509Certificate2 cert)
		{
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(jsonDataSigned) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = GetRsaKey(cert)
                };

                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(jsonDataSigned, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return null;
		}

		private RsaSecurityKey GetRsaKey(X509Certificate2 certificate)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			RSACryptoServiceProvider rSACryptoServiceProvider = (RSACryptoServiceProvider)certificate.PrivateKey;
			return new RsaSecurityKey((RSA)rSACryptoServiceProvider);
		}

		private void SignXmlProcess(XmlDocument xmlDoc, X509Certificate2 cert)
		{
			if (xmlDoc == null)
			{
				throw new ArgumentException("xmlDoc");
			}
			if (cert == null)
			{
				throw new ArgumentException("cert");
			}
			SignedXml signedXml = new SignedXml(xmlDoc);
			signedXml.SigningKey = cert.PrivateKey;
			Reference reference = new Reference();
			reference.Uri = "";
			XmlDsigEnvelopedSignatureTransform transform = new XmlDsigEnvelopedSignatureTransform();
			reference.AddTransform(transform);
			KeyInfo keyInfo = new KeyInfo();
			KeyInfoX509Data clause = new KeyInfoX509Data(cert);
			keyInfo.AddClause(clause);
			signedXml.KeyInfo = keyInfo;
			signedXml.AddReference(reference);
			signedXml.ComputeSignature();
			XmlElement xml = signedXml.GetXml();
			xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xml, true));
		}

		private void AddPageEmpty(string src, string dest, DisplayConfig config)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Expected O, but got Unknown
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Expected O, but got Unknown
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Expected O, but got Unknown
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Expected O, but got Unknown
			PdfReader val = null;
			FileStream fileStream = null;
			try
			{
				val = new PdfReader(src);
				fileStream = new FileStream(dest, FileMode.Append);
				PdfStamper val2 = new PdfStamper(val, (Stream)fileStream);
				for (int i = 0; i < config.NumberPageSign; i++)
				{
					val2.InsertPage(i, config.PageSize);
				}
				PdfContentByte underContent = val2.GetUnderContent(1);
				float width = config.PageSize.Width;
				float marginRight = config.MarginRight;
				float num = config.PageSize.Height - config.MarginTop - config.HeightTitle;
				float heightTitle = config.HeightTitle;
				float num2 = width - config.MarginRight * 2f;
				BaseFont val3 = BaseFont.CreateFont(config.FontPath, "Identity-H", true);
				if (config.IsDisplayTitlePageSign)
				{
					ColumnText val4 = new ColumnText(underContent);
					val4.SetSimpleColumn(marginRight, num + config.HeightTitle, marginRight + num2, num + config.HeightTitle + config.HeightRowTitlePageSign);
					Paragraph val5 = new Paragraph(config.TitlePageSign, new Font(val3, (float)config.FontSizeTitlePageSign, 1))
					{
						Alignment = 1
					};
					val4.AddElement((IElement)(object)val5);
					val4.Go();
				}
				PdfPTable val6 = new PdfPTable(config.WidthsPercen.Length);
				val6.SetWidths(config.WidthsPercen);
				val6.WidthPercentage = 100f;
				string[] titles = config.Titles;
				for (int j = 0; j < titles.Length; j++)
				{
					Paragraph val5 = new Paragraph(titles[j], new Font(val3, (float)config.SizeFont, 1))
					{
						Alignment = 1
					};
					PdfPCell val7 = new PdfPCell();
					val7.AddElement((IElement)(object)val5);
					val7.FixedHeight = config.HeightTitle;
					((Rectangle)val7).BackgroundColor = config.BackgroundColorTitle;
					val6.AddCell(val7);
				}
				ColumnText val8 = new ColumnText(underContent);
				val8.SetSimpleColumn(marginRight, num, marginRight + num2, num + config.HeightTitle);
				val8.AddElement((IElement)(object)val6);
				val8.Go();
				val2.Close();
				val.Close();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			finally
			{
				if (fileStream != null)
				{
					try
					{
						fileStream.Close();
					}
					catch (IOException ex2)
					{
						LogSystem.Warn((Exception)ex2);
					}
				}
				if (val != null)
				{
					val.Close();
				}
			}
		}

        private byte[] CreateHash(string filePath, Org.BouncyCastle.X509.X509Certificate[] chain, DisplayConfig displayConfig)
		{
			try
			{
				string tempFile = SharedUtils.GenerateTempFile();
				fieldName = SharedUtils.getSignName();
				DateTime dateTime = displayConfig.SignDate;
				signDate = displayConfig.SignDate;
				List<byte[]> list = new SignPdfAsynchronous().CreateHash(filePath, tempFile, fieldName, chain, displayConfig);
				if (list == null)
				{
					return null;
				}
				tmpFile = tempFile;
				hash = list[1];
				this.chain = chain;
				return EncodeData(list[0], (!string.IsNullOrEmpty(displayConfig.HashAlgorithm)) ? displayConfig.HashAlgorithm : SignPdfAsynchronous.HASH_ALG);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return null;
			}
		}

        private bool EmptySignatureHashPkcsServer(string inFile, Stream outStream, string fieldName, string reason, string location, Org.BouncyCastle.X509.X509Certificate cert, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage)
		{
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Expected O, but got Unknown
			//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c58: Expected O, but got Unknown
			//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c82: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbe: Expected O, but got Unknown
			//IL_0cc4: Expected O, but got Unknown
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Expected O, but got Unknown
			//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d4f: Expected O, but got Unknown
			//IL_0d58: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Expected O, but got Unknown
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f5: Expected O, but got Unknown
			//IL_0674: Unknown result type (might be due to invalid IL or missing references)
			//IL_067b: Expected O, but got Unknown
			//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b8d: Expected O, but got Unknown
			//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd8: Expected O, but got Unknown
			//IL_0717: Unknown result type (might be due to invalid IL or missing references)
			//IL_071e: Expected O, but got Unknown
			//IL_0757: Unknown result type (might be due to invalid IL or missing references)
			//IL_075e: Expected O, but got Unknown
			//IL_0924: Unknown result type (might be due to invalid IL or missing references)
			//IL_092b: Expected O, but got Unknown
			//IL_092b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0932: Expected O, but got Unknown
			//IL_0797: Unknown result type (might be due to invalid IL or missing references)
			//IL_079e: Expected O, but got Unknown
			//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07de: Expected O, but got Unknown
			//IL_0817: Unknown result type (might be due to invalid IL or missing references)
			//IL_081e: Expected O, but got Unknown
			//IL_0857: Unknown result type (might be due to invalid IL or missing references)
			//IL_085e: Expected O, but got Unknown
			//IL_0894: Unknown result type (might be due to invalid IL or missing references)
			//IL_089b: Expected O, but got Unknown
			//IL_0a9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa1: Expected O, but got Unknown
			//IL_0aa1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa8: Expected O, but got Unknown
			//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d8: Expected O, but got Unknown
			FileStream fileStream = null;
			bool result = false;
			string text = "";
			PdfReader val = null;
			try
			{
				DisplayConfig displayConfig;
				if (displayConfigParam != null)
				{
					displayConfig = DisplayConfig.generateDisplayConfigRectangleText(displayConfigParam.TypeDisplay, displayConfigParam.SizeFont, displayConfigParam.TextPosition, displayConfigParam.PathImage, displayConfigParam.BImage, displayConfigParam.NumberPageSign, displayConfigParam.CoorXRectangle, displayConfigParam.CoorYRectangle, (displayConfigParam.WidthRectangle > 0f) ? displayConfigParam.WidthRectangle : 320f, (displayConfigParam.HeightRectangle > 0f) ? displayConfigParam.HeightRectangle : 140f, null, (!string.IsNullOrEmpty(displayConfigParam.FormatRectangleText)) ? displayConfigParam.FormatRectangleText : Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(cert), (!string.IsNullOrEmpty(displayConfigParam.Reason)) ? displayConfigParam.Reason : reason, (!string.IsNullOrEmpty(displayConfigParam.Location)) ? displayConfigParam.Location : location, "{0:dd/MM/yyyy HH:mm:ss}");
					displayConfig.IsDisplaySignature = displayConfigParam.IsDisplaySignature;
					displayConfig.SignaltureImageWidth = displayConfigParam.SignaltureImageWidth;
					displayConfig.IsDisplaySignNote = (displayConfigParam.IsDisplaySignNote.HasValue ? displayConfigParam.IsDisplaySignNote : new bool?(false));
					displayConfig.TextFormat = displayConfigParam.TextFormat;
				}
				else
				{
					displayConfig = DisplayConfig.generateDisplayConfigRectangleText(1, 10f, 10f, 320f, 140f, null, Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(cert), reason, location, "{0:dd/MM/yyyy HH:mm:ss}");
				}
				text = SharedUtils.GenerateTempFile();
				val = new PdfReader(inFile);
				int numberOfPages = val.NumberOfPages;
				int num = displayConfig.NumberPageSign;
				if (num < 1 || num > numberOfPages)
				{
					num = 1;
				}
				fileStream = new FileStream(text, FileMode.Create);
				bool flag = val.IsRebuilt();
				PdfSignatureAppearance val2 = null;
				if (flag)
				{
					val.Catalog.Remove(PdfName.PERMS);
					val.RemoveUsageRights();
					val2 = PdfStamper.CreateSignature(val, (Stream)fileStream, '\0', (string)null).SignatureAppearance;
				}
				else
				{
					val2 = PdfStamper.CreateSignature(val, (Stream)fileStream, '\0', (string)null, true).SignatureAppearance;
				}
				displayConfig.SignDate = DateTime.Now;
				DateTime dateTime = displayConfig.SignDate;
				if ("".Equals(displayConfig.Contact))
				{
					displayConfig.Contact = SharedUtils.GetCN(cert);
				}
				val2.Contact = displayConfig.Contact;
				val2.SignDate = dateTime;
				val2.Reason = displayConfig.Reason;
				val2.Location = displayConfig.Location;
				string strDate = string.Format(displayConfig.DateFormatstring, dateTime);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<DisplayConfig>((Expression<Func<DisplayConfig>>)(() => displayConfig)), (object)displayConfig));
				if (displayConfig.IsDisplaySignature)
				{
					float num2 = displayConfig.CoorXRectangle - displayConfig.WidthRectangle / 2f;
					float num3 = displayConfig.CoorYRectangle - displayConfig.HeightRectangle / 2f;
					float widthRectangle = displayConfig.WidthRectangle;
					float heightRectangle = displayConfig.HeightRectangle;
					SignPdfAsynchronous.ProcessFontSizeFit(displayConfig);
					float num4 = 0f;
					float num5 = 0f;
					Image val3 = null;
					if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
					{
						if (!string.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
						{
							val3 = Image.GetInstance(displayConfig.PathImage);
						}
						else if (displayConfig.BImage != null)
						{
							val3 = Image.GetInstance(displayConfig.BImage);
						}
					}
					else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
					{
						if (!string.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
						{
							val3 = Image.GetInstance(displayConfig.PathImage);
						}
						else if (displayConfig.BImage != null)
						{
							val3 = Image.GetInstance(displayConfig.BImage);
						}
					}
					if (displayConfig.SignType == Constans.SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD)
					{
						Rectangle val4 = new Rectangle(num2, num3, num2 + widthRectangle + num4, num3 + heightRectangle + num5);
						val2.SetVisibleSignature(val4, num, fieldName);
					}
					else
					{
						val2.SetVisibleSignature(fieldName);
					}
					if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
					{
						float totalWidth = widthRectangle;
						PdfTemplate layer = val2.GetLayer(2);
						float left = layer.BoundingBox.Left;
						float bottom = layer.BoundingBox.Bottom;
						float width = layer.BoundingBox.Width;
						float height = layer.BoundingBox.Height;
						ColumnText val5 = new ColumnText((PdfContentByte)(object)layer);
						PdfPCell val6 = new PdfPCell();
						if (val3 != null)
						{
							val3.Alignment = 1;
							float plusH = SignPdfAsynchronous.ProcessHeightPlus(100f, displayConfig);
							val3.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, val3, displayConfig.SignaltureImageWidth, 100f, plusH);
							val6.AddElement((IElement)(object)val3);
							val6.HorizontalAlignment = 1;
							val6.VerticalAlignment = 5;
							((Rectangle)val6).Border = 0;
							val6.MinimumHeight = heightRectangle;
						}
						PdfPTable val7 = new PdfPTable(1);
						val7.TotalWidth = totalWidth;
						val7.LockedWidth = true;
						val7.AddCell(val6);
						val5.AddElement((IElement)(object)val7);
						val5.SetSimpleColumn(left, bottom, width, height);
						val5.Alignment = 1;
						val5.Go();
					}
					else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
					{
						string displayText = SignPdfAsynchronous.GetDisplayText(displayConfig, strDate);
						float widthImagePercent = 0f;
						PdfPTable val8 = null;
						if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100)
						{
							val8 = new PdfPTable(1);
							widthImagePercent = 100f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75)
						{
							val8 = new PdfPTable(new float[2] { 25f, 75f });
							widthImagePercent = 25f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70)
						{
							val8 = new PdfPTable(new float[2] { 30f, 70f });
							widthImagePercent = 30f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60)
						{
							val8 = new PdfPTable(new float[2] { 40f, 60f });
							widthImagePercent = 40f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50)
						{
							val8 = new PdfPTable(new float[2] { 50f, 50f });
							widthImagePercent = 50f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x60x40)
						{
							val8 = new PdfPTable(new float[2] { 60f, 40f });
							widthImagePercent = 40f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x70x30)
						{
							val8 = new PdfPTable(new float[2] { 70f, 30f });
							widthImagePercent = 30f;
						}
						else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x75x25)
						{
							val8 = new PdfPTable(new float[2] { 75f, 25f });
							widthImagePercent = 25f;
						}
						PdfTemplate layer2 = val2.GetLayer(2);
						float left2 = layer2.BoundingBox.Left;
						float bottom2 = layer2.BoundingBox.Bottom;
						float width2 = layer2.BoundingBox.Width;
						float height2 = layer2.BoundingBox.Height;
						ColumnText val9 = new ColumnText((PdfContentByte)(object)layer2);
						PdfPCell val10 = new PdfPCell();
						if (val3 != null)
						{
							val3.Alignment = 1;
							float plusH2 = SignPdfAsynchronous.ProcessHeightPlus(widthImagePercent, displayConfig);
							val3.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, val3, displayConfig.SignaltureImageWidth, widthImagePercent, plusH2);
							val10.AddElement((IElement)(object)val3);
							val10.HorizontalAlignment = 1;
							val10.VerticalAlignment = 5;
							((Rectangle)val10).Border = 0;
							LogSystem.Info(LogUtil.TraceData("instance.WidthPercentage", (object)val3.WidthPercentage) + LogUtil.TraceData("instance.Width", (object)((Rectangle)val3).Width) + LogUtil.TraceData("displayConfig.SignaltureImageWidth", (object)displayConfig.SignaltureImageWidth));
						}
						PdfPCell textCell = GetTextCell(displayText, displayConfig);
						val8.TotalWidth = widthRectangle;
						val8.LockedWidth = true;
						if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100 || displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75 || displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70 || displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60 || displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50)
						{
							val8.AddCell(val10);
							val8.AddCell(textCell);
						}
						else
						{
							val8.AddCell(textCell);
							val8.AddCell(val10);
						}
						PdfPTable val11 = new PdfPTable(1);
						PdfPCell val12 = new PdfPCell();
						val12.AddElement((IElement)(object)val8);
						val12.HorizontalAlignment = 1;
						val12.VerticalAlignment = 5;
						((Rectangle)val12).Border = 0;
						val12.MinimumHeight = heightRectangle;
						val11.TotalWidth = widthRectangle;
						val11.LockedWidth = true;
						val11.AddCell(val12);
						val9.AddElement((IElement)(object)val11);
						val9.SetSimpleColumn(left2, bottom2, width2, height2);
						val9.Alignment = 1;
						val9.Go();
					}
					else if (displayConfig.TypeDisplay == Constans.DISPLAY_RECTANGLE_TEXT)
					{
						PdfTemplate layer3 = val2.GetLayer(2);
						float left3 = layer3.BoundingBox.Left;
						float bottom3 = layer3.BoundingBox.Bottom;
						float width3 = layer3.BoundingBox.Width;
						float height3 = layer3.BoundingBox.Height;
						ColumnText val13 = new ColumnText((PdfContentByte)(object)layer3);
						val13.SetSimpleColumn(left3, bottom3, width3, height3);
						val13.Alignment = 5;
						string displayText2 = SignPdfAsynchronous.GetDisplayText(displayConfig, strDate);
						PdfPCell textCell2 = GetTextCell(displayText2, displayConfig);
						textCell2.MinimumHeight = heightRectangle;
						PdfPTable val14 = new PdfPTable(1);
						val14.TotalWidth = widthRectangle;
						val14.HorizontalAlignment = 1;
						val14.LockedWidth = true;
						val14.AddCell(textCell2);
						val14.CompleteRow();
						val13.AddElement((IElement)(object)val14);
						val13.Go();
					}
				}
				else if (displayConfig.SignType == Constans.SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD)
				{
					val2.SetVisibleSignature(new Rectangle(0f, 0f, 0f, 0f), 1, fieldName);
				}
				else
				{
					val2.SetVisibleSignature(fieldName);
				}
				val2.Certificate = cert;
				val2.CryptoDictionary = (PdfDictionary)new PdfSignature(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED)
				{
					Reason = val2.Reason,
					Location = val2.Location,
					Contact = val2.Contact,
					Date = new PdfDate(val2.SignDate)
				};
				Dictionary<PdfName, int> dictionary = new Dictionary<PdfName, int>();
				dictionary.Add(PdfName.CONTENTS, 16386);
				val2.PreClose(dictionary);
				byte[] array = DigestAlgorithms.Digest(val2.GetRangeStream(), SignPdfAsynchronous.HASH_ALG);
				if (dlgGetHSMServerResponseData != null && array != null)
				{
					string hashData = ByteArrayToHexString(array);
					string s = dlgGetHSMServerResponseData(hashData, ref errMessage);
					byte[] array2 = Convert.FromBase64String(s);
					byte[] array3 = new byte[8192];
					array2.CopyTo(array3, 0);
					PdfDictionary val15 = new PdfDictionary();
					val15.Put(PdfName.CONTENTS, (PdfObject)(object)new PdfString(array3).SetHexWriting(true));
					val2.Close(val15);
					MemoryStream memoryStream = new MemoryStream(SharedUtils.FileToByte(text));
					if (memoryStream != null)
					{
						memoryStream.Position = 0L;
						memoryStream.CopyTo(outStream);
					}
					memoryStream.Close();
					memoryStream.Dispose();
				}
				result = true;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			finally
			{
				if (val != null)
				{
					val.Close();
				}
				if (fileStream != null)
				{
					try
					{
						fileStream.Close();
						fileStream.Dispose();
					}
					catch (IOException ex2)
					{
						LogSystem.Warn("Error emptySignature: " + ex2.Message);
					}
				}
				try
				{
					if (!string.IsNullOrEmpty(text) && File.Exists(text))
					{
						File.Delete(text);
					}
				}
				catch (IOException ex3)
				{
					LogSystem.Warn("Error emptySignature: " + ex3.Message);
				}
			}
			return result;
		}

		private string ByteArrayToHexString(byte[] ba)
		{
			StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}

        private string CreateHashExistedSignatureField(string filePath, Org.BouncyCastle.X509.X509Certificate[] chain, DisplayConfig displayConfig, string fieldName)
		{
			try
			{
				string tempFile = SharedUtils.GenerateTempFile();
				this.fieldName = fieldName;
				DateTime dateTime = displayConfig.SignDate;
				signDate = displayConfig.SignDate;
				List<byte[]> list = new SignPdfAsynchronous().CreateHash(filePath, tempFile, fieldName, chain, displayConfig);
				if (list == null)
				{
					return null;
				}
				tmpFile = tempFile;
				hash = list[1];
				this.chain = chain;
				return Convert.ToBase64String(EncodeData(list[0], SignPdfAsynchronous.HASH_ALG));
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return null;
			}
		}

		private byte[] EncodeData(byte[] orginalData, string algorithm)
		{
			byte[] result = null;
			if (HASH_ALGORITHM_SHA_1.Equals(algorithm))
			{
				return new SHA1Managed().ComputeHash(orginalData);
			}
			if (HASH_ALGORITHM_SHA_256.Equals(algorithm))
			{
				result = new SHA256Managed().ComputeHash(orginalData);
			}
			else if (HASH_ALGORITHM_SHA_512.Equals(algorithm))
			{
				result = new SHA512Managed().ComputeHash(orginalData);
			}
			return result;
		}

        private byte[] GetHashTypeRectangleText(string src, Org.BouncyCastle.X509.X509Certificate[] certChain, string reason, string location)
		{
			DisplayConfig displayConfig;
			if (displayConfigParam != null)
			{
				displayConfig = DisplayConfig.generateDisplayConfigRectangleText(displayConfigParam.TypeDisplay, displayConfigParam.SizeFont, displayConfigParam.TextPosition, displayConfigParam.PathImage, displayConfigParam.BImage, displayConfigParam.NumberPageSign, displayConfigParam.CoorXRectangle, displayConfigParam.CoorYRectangle, (displayConfigParam.WidthRectangle > 0f) ? displayConfigParam.WidthRectangle : 320f, (displayConfigParam.HeightRectangle > 0f) ? displayConfigParam.HeightRectangle : 140f, null, (!string.IsNullOrEmpty(displayConfigParam.FormatRectangleText)) ? displayConfigParam.FormatRectangleText : Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(certChain[0]), (!string.IsNullOrEmpty(displayConfigParam.Reason)) ? displayConfigParam.Reason : reason, (!string.IsNullOrEmpty(displayConfigParam.Location)) ? displayConfigParam.Location : location, "{0:dd/MM/yyyy HH:mm:ss}");
				displayConfig.IsDisplaySignature = displayConfigParam.IsDisplaySignature;
				displayConfig.SignaltureImageWidth = displayConfigParam.SignaltureImageWidth;
				displayConfig.TextFormat = displayConfigParam.TextFormat;
				DateTime signDate2 = displayConfigParam.SignDate;
				if (displayConfigParam.SignDate != DateTime.MinValue)
				{
					displayConfig.SignDate = displayConfigParam.SignDate;
				}
				else
				{
					displayConfig.SignDate = DateTime.Now;
				}
				displayConfig.HashAlgorithm = displayConfigParam.HashAlgorithm;
			}
			else
			{
				displayConfig = DisplayConfig.generateDisplayConfigRectangleText(1, 10f, 10f, 320f, 140f, null, Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(certChain[0]), reason, location, "{0:dd/MM/yyyy HH:mm:ss}");
			}
			return CreateHash(src, certChain, displayConfig);
		}

        private byte[] GetHashTypeRectangleText(string src, Org.BouncyCastle.X509.X509Certificate[] certChain, string reason, string location, bool? isDisplaySignNote)
		{
			DisplayConfig displayConfig;
			if (displayConfigParam != null)
			{
				displayConfig = DisplayConfig.generateDisplayConfigRectangleText(displayConfigParam.TypeDisplay, displayConfigParam.SizeFont, displayConfigParam.TextPosition, displayConfigParam.PathImage, displayConfigParam.BImage, displayConfigParam.NumberPageSign, displayConfigParam.CoorXRectangle, displayConfigParam.CoorYRectangle, (displayConfigParam.WidthRectangle > 0f) ? displayConfigParam.WidthRectangle : 320f, (displayConfigParam.HeightRectangle > 0f) ? displayConfigParam.HeightRectangle : 140f, null, (!string.IsNullOrEmpty(displayConfigParam.FormatRectangleText)) ? displayConfigParam.FormatRectangleText : Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(certChain[0]), (!string.IsNullOrEmpty(displayConfigParam.Reason)) ? displayConfigParam.Reason : reason, (!string.IsNullOrEmpty(displayConfigParam.Location)) ? displayConfigParam.Location : location, "{0:dd/MM/yyyy HH:mm:ss}");
				displayConfig.IsDisplaySignature = displayConfigParam.IsDisplaySignature;
				displayConfig.SignaltureImageWidth = displayConfigParam.SignaltureImageWidth;
				displayConfig.IsDisplaySignNote = (displayConfigParam.IsDisplaySignNote.HasValue ? displayConfigParam.IsDisplaySignNote : new bool?(false));
				displayConfig.TextFormat = displayConfigParam.TextFormat;
				DateTime signDate2 = displayConfigParam.SignDate;
				if (displayConfigParam.SignDate != DateTime.MinValue)
				{
					displayConfig.SignDate = displayConfigParam.SignDate;
				}
				else
				{
					displayConfig.SignDate = DateTime.Now;
				}
				displayConfig.HashAlgorithm = displayConfigParam.HashAlgorithm;
			}
			else if (isDisplaySignNote.HasValue && isDisplaySignNote.Value)
			{
				displayConfig = DisplayConfig.generateDisplayConfigRectangleText(1, 10f, 10f, 320f, 140f, null, Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(certChain[0]), reason, location, "{0:dd/MM/yyyy HH:mm:ss}");
				displayConfig.IsDisplaySignNote = isDisplaySignNote;
			}
			else
			{
				displayConfig = DisplayConfig.generateDisplayConfigRectangleText(1, 10f, 10f, 320f, 140f, null, Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(certChain[0]), reason, location, "{0:dd/MM/yyyy HH:mm:ss}");
			}
			return CreateHash(src, certChain, displayConfig);
		}

		private bool InsertSignature(string extSig, string outFile, TimestampConfig timestampConfig)
		{
			try
			{
				SignPdfAsynchronous signPdfAsynchronous = new SignPdfAsynchronous();
				if (File.Exists(tmpFile))
				{
					if (signPdfAsynchronous.InsertSignature(tmpFile, outFile, fieldName, hash, Convert.FromBase64String(extSig), chain, signDate, displayConfigParam, timestampConfig))
					{
					}
					try
					{
						File.Delete(tmpFile);
					}
					catch (Exception)
					{
						Console.WriteLine("Error: delete file Temp is fail ");
					}
					return false;
				}
				Console.WriteLine("Error: file Temp is not exist ");
				return false;
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
				return false;
			}
		}

		private bool InsertSignature(string extSig, Stream outStream, TimestampConfig timestampConfig)
		{
			try
			{
				SignPdfAsynchronous signPdfAsynchronous = new SignPdfAsynchronous();
				if (File.Exists(tmpFile))
				{
					if (signPdfAsynchronous.InsertSignature(tmpFile, outStream, fieldName, hash, Convert.FromBase64String(extSig), chain, signDate, displayConfigParam, timestampConfig))
					{
					}
					try
					{
						File.Delete(tmpFile);
					}
					catch (Exception)
					{
						Console.WriteLine("Error: delete file Temp is fail ");
					}
					return true;
				}
				Console.WriteLine("Error: file Temp is not exist ");
				return false;
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
				return false;
			}
		}

		private void DeleteFileTemp(string fileDeletePath)
		{
			try
			{
				if (File.Exists(fileDeletePath))
				{
					File.Delete(fileDeletePath);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void DisposeFileStream(Stream fileStream)
		{
			try
			{
				if (fileStream != null)
				{
					fileStream.Flush();
					fileStream.Close();
					fileStream.Dispose();
					fileStream = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
