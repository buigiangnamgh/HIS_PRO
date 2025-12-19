using Inventec.Common.Logging;
using Inventec.Common.SignFile.XmlProcess.XmlDsig;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec;
using iTextSharp.text.pdf.security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace Inventec.Common.SignFile
{
    public class SignPdfFile
    {
        // Fields
        private X509Certificate[] chain;
        private string fieldName;
        private byte[] hash;
        internal static string HASH_ALGORITHM_SHA_1 = "SHA1";
        internal static string HASH_ALGORITHM_SHA_256 = "SHA256";
        internal static string HASH_ALGORITHM_SHA_512 = "SHA512";
        private DateTime signDate;
        private string tmpFile;
        DisplayConfig displayConfigParam;

        #region Public Method

        public bool SignPDF(X509Certificate2 cert, Stream inStream, string outFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
        {
            string src = "";
            bool success = false;
            SharedUtils.SaveNewFileFromReader(inStream, ref src);
            try
            {
                if (File.Exists(src))
                {
                    success = SignPDF(cert, src, outFile, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, "", isSignElectronic);
                }

                DeleteFileTemp(src);
                DisposeFileStream(inStream);
            }
            catch (Exception exception)
            {
                success = false;
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }

            return success;
        }

        public bool SignPDF(X509Certificate2 cert, Stream inStream, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
        {
            string src = "";
            bool success = false;
            SharedUtils.SaveNewFileFromReader(inStream, ref src);
            try
            {
                if (File.Exists(src))
                {
                    success = SignPDF(cert, src, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, isSignElectronic);
                }

                DeleteFileTemp(src);
                DisposeFileStream(inStream);
            }
            catch (Exception exception)
            {
                success = false;
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }

            return success;
        }

        public bool SignPDF(X509Certificate2 cert, Stream inStream, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer, bool isSignElectronic = false)
        {
            string src = "";
            bool success = false;
            SharedUtils.SaveNewFileFromReader(inStream, ref src);
            try
            {
                if (File.Exists(src))
                {
                    success = SignPDF(cert, src, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, hasHashPkcsServer, isSignElectronic);
                }

                DeleteFileTemp(src);
                DisposeFileStream(inStream);
            }
            catch (Exception exception)
            {
                success = false;
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }

            return success;
        }

        public bool SignPDF(X509Certificate2 cert, byte[] bInFile, string outFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
        {
            bool success = false;
            string src = "";
            SharedUtils.SaveNewFileFromReader(bInFile, ref src);
            try
            {
                if (File.Exists(src))
                {
                    success = SignPDF(cert, src, outFile, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, "", isSignElectronic);
                }
            }
            catch (Exception exception)
            {
                success = false;
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }

            DeleteFileTemp(src);
            return success;
        }

        public bool SignPDF(X509Certificate2 cert, byte[] bInFile, byte[] bOutFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
        {
            bool success = false;
            string src = "";
            string outFile = "";
            SharedUtils.SaveNewFileFromReader(bInFile, ref src);
            try
            {
                if (File.Exists(src))
                {
                    success = SignPDF(cert, src, outFile, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, "", isSignElectronic);
                    if (success && File.Exists(outFile))
                    {
                        bOutFile = File.ReadAllBytes(outFile);
                    }
                }
            }
            catch (Exception exception)
            {
                success = false;
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }

            DeleteFileTemp(src);
            return success;
        }

        public bool SignPDF(X509Certificate2 cert, string inFile, string outFile, string reason, string location, TimestampConfig timestampConfig, ref string errMessage, bool isSignElectronic = false)
        {
            try
            {
                return SignPDF(cert, inFile, outFile, reason, location, timestampConfig, null, null, ref errMessage, "", isSignElectronic);
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
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
                else
                {
                    return SignPDFDigital(cert, inFile, outFile, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, pinCode);
                }
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
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
                else
                {
                    return SignPDFDigital(cert, inFile, outFile, reason, location, useTimestamp, displayConfig, ref errMessage);
                }
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return false;
        }

        public bool SignPDF(X509Certificate2 cert, string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool isSignElectronic = false)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignPDF____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSignElectronic), isSignElectronic)
                    + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inFile), inFile));
                if (isSignElectronic)
                {
                    return SignPDFElectronic(inFile, outStream, reason, location, displayConfig, ref errMessage);
                }
                else
                {
                    return SignPDFDigital(cert, inFile, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage);
                }
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return false;
        }

        public bool SignPDF(X509Certificate2 cert, string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer, bool isSignElectronic = false)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignPDF____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSignElectronic), isSignElectronic)
                    + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inFile), inFile));
                if (isSignElectronic)
                {
                    return SignPDFElectronic(inFile, outStream, reason, location, displayConfig, ref errMessage);
                }
                else
                {
                    return SignPDFDigital(cert, inFile, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, hasHashPkcsServer);
                }
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return false;
        }

        public bool SignXml(X509Certificate2 cert, byte[] bInFile, ref byte[] bOutFile, XmlConfig xmlConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, string pinCode)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignXml. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => xmlConfig), xmlConfig));

                string src = "";
                SharedUtils.SaveNewFileFromReader(bInFile, ref src, ".xml");

                // Create a new XML document.
                if (!String.IsNullOrEmpty(src))
                {
                    var howToSign =
             XmlDsigHelper.Sign(src).Using(cert).UsingFormat(XmlDsigSignatureFormat.Enveloped)
                 .IncludingCertificateInSignature().IncludeTimestamp(false)
                 .NodeToSign("");

                    xmlConfig.SigningTime = (xmlConfig.SigningTime == DateTime.MinValue ? DateTime.Now : xmlConfig.SigningTime);
                    string strDate = string.Format("{0:dd/MM/yyyy HH:mm:ss}", signDate);
                    howToSign.WithProperty(String.IsNullOrEmpty(xmlConfig.Reason) ? "SigningTime" : "SigningTimeReason", String.IsNullOrEmpty(xmlConfig.Reason) ? strDate : String.Format("{0} - {1}", strDate, xmlConfig.Reason), "http://xades.codeplex.com/#properties");

                    string outputPath = SharedUtils.GenerateTempFile();
                    string dataServerHsmResponse = "";

                    if (dlgGetHSMServerResponseData != null)
                    {
                        string s = Convert.ToBase64String(SharedUtils.FileToByte(howToSign.getParameters().InputPath));
                        try
                        {
                            dataServerHsmResponse = dlgGetHSMServerResponseData(s, ref errMessage);
                        }
                        catch { }
                    }

                    howToSign.SignToFile(outputPath, pinCode, dataServerHsmResponse);
                    var VerificationResults = XmlDsigHelper.Verify(outputPath).PerformAndGetResults();

                    if (!String.IsNullOrEmpty(outputPath) && VerificationResults != null && VerificationResults.SigningCertificate != null)
                    {
                        success = true;
                        bOutFile = File.ReadAllBytes(outputPath);
                        DeleteFileTemp(outputPath);
                    }
                }

                DeleteFileTemp(src);
                Inventec.Common.Logging.LogSystem.Debug("SignXml. 2");
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        public bool SignXml(X509Certificate2 cert, byte[] bInFile, string outFile, XmlConfig xmlConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, string pinCode)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignXml. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => xmlConfig), xmlConfig));

                string src = "";
                SharedUtils.SaveNewFileFromReader(bInFile, ref src, ".xml");

                // Create a new XML document.
                if (!String.IsNullOrEmpty(src))
                {
                    var howToSign =
             XmlDsigHelper.Sign(src).Using(cert).UsingFormat(XmlDsigSignatureFormat.Enveloped)
                 .IncludingCertificateInSignature().IncludeTimestamp(false)
                 .NodeToSign("");

                    xmlConfig.SigningTime = (xmlConfig.SigningTime == DateTime.MinValue ? DateTime.Now : xmlConfig.SigningTime);
                    string strDate = string.Format("{0:dd/MM/yyyy HH:mm:ss}", signDate);
                    howToSign.WithProperty(String.IsNullOrEmpty(xmlConfig.Reason) ? "SigningTime" : "SigningTimeReason", String.IsNullOrEmpty(xmlConfig.Reason) ? strDate : String.Format("{0} - {1}", strDate, xmlConfig.Reason), "http://xades.codeplex.com/#properties");

                    string outputPath = SharedUtils.GenerateTempFile(".xml");
                    string dataServerHsmResponse = "";

                    if (dlgGetHSMServerResponseData != null)
                    {
                        string s = Convert.ToBase64String(SharedUtils.FileToByte(howToSign.getParameters().InputPath));
                        try
                        {
                            dataServerHsmResponse = dlgGetHSMServerResponseData(s, ref errMessage);
                        }
                        catch { }
                    }

                    howToSign.SignToFile(outFile, pinCode, dataServerHsmResponse);
                    var VerificationResults = XmlDsigHelper.Verify(outFile).PerformAndGetResults();

                    if (!String.IsNullOrEmpty(outFile) && VerificationResults != null && VerificationResults.SigningCertificate != null)
                    {
                        success = true;
                    }
                    else
                    {
                        outFile = "";
                    }
                    Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outFile), outFile));
                }

                Inventec.Common.Logging.LogSystem.Debug("SignXml. 2");
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        public bool VerifySignedXml(string xmlSignedFile, ref string signedInfoData, ref string errMessage)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("VerifySignedXml. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => xmlSignedFile), xmlSignedFile));

                // Create a new XML document.
                if (!String.IsNullOrEmpty(xmlSignedFile))
                {
                    var verificationResults = XmlDsigHelper.Verify(xmlSignedFile).PerformAndGetResults();
                    if (verificationResults != null && verificationResults.SigningCertificate != null && verificationResults.OriginalDocument != null)
                    {
                        success = true;
                        X509Certificate[] certChain = CertUtil.GetX509CertChain(verificationResults.SigningCertificate);
                        signedInfoData = String.Format("{0} - {1}", SharedUtils.GetCN(certChain[0]), verificationResults.Timestamp);
                    }
                }

                Inventec.Common.Logging.LogSystem.Debug("VerifySignedXml. 2");
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        public bool SignJson(X509Certificate2 cert, string jsonFile, Stream outStream, string reason, string location, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, string pinCode)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignJson. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => jsonFile), jsonFile));
                this.displayConfigParam = displayConfig;
                // Create a new XML document.
                if (!String.IsNullOrEmpty(jsonFile))
                {
                    string jsonData = File.Exists(jsonFile) ? File.ReadAllText(jsonFile) : "";
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var now = DateTime.UtcNow;
                    string checkSumFile = "";

                    string jwtData = "";
                    if (jsonData.Contains("<SignatureCertificate>") && jsonData.Contains("</SignatureCertificate>"))
                    {
                        var arrDataSigned = jsonData.Split(new string[] { "<SignatureCertificate>", "</SignatureCertificate>" }, StringSplitOptions.None);
                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => arrDataSigned), arrDataSigned));
                        if (arrDataSigned != null && arrDataSigned.Length > 1)
                        {
                            jwtData = arrDataSigned[1].Replace("</SignatureCertificate>", "");
                        }
                    }

                    var principal = String.IsNullOrEmpty(jwtData) ? null : GetPrincipal(jwtData, Inventec.Common.SignFile.SharedUtils.CertManager.Certificate);

                    string dataInfo = "";
                    if (principal != null && principal.Identity != null)
                    {
                        var claim__Name = principal.FindFirst(ClaimTypes.Name);
                        if (claim__Name != null && !String.IsNullOrEmpty(claim__Name.Value))
                        {
                            var name = (claim__Name.Value);
                            dataInfo += name + "\r\n";
                        }
                        var claim__NameIdentifier = principal.FindFirst(ClaimTypes.NameIdentifier);
                        if (claim__NameIdentifier != null && !String.IsNullOrEmpty(claim__NameIdentifier.Value))
                        {
                            try
                            {
                                checkSumFile = claim__NameIdentifier.Value;
                            }
                            catch (Exception exxx)
                            {
                                Inventec.Common.Logging.LogSystem.Warn(exxx);
                            }
                        }
                    }
                    if (String.IsNullOrEmpty(checkSumFile))
                    {
                        checkSumFile = SharedUtils.GetFileContentHash(jsonData);
                    }

                    Inventec.Common.Logging.LogSystem.Info("cert is " + (cert != null ? "not null" : "null"));

                    displayConfig.SignDate = DateTime.Now;
                    DateTime signDate = displayConfig.SignDate;
                    string strDate = string.Format(displayConfig.DateFormatstring, signDate);
                    if ("".Equals(displayConfig.Contact) && cert != null)
                    {
                        X509Certificate[] certChain = CertUtil.GetX509CertChain(cert);
                        displayConfig.Contact = SharedUtils.GetCN(certChain[0]);
                    }
                    displayConfig.Reason = reason;
                    dataInfo += SignPdfAsynchronous.GetDisplayText(displayConfig, strDate).Replace("\r\n", " - ");

                    //if (cert != null)
                    //{
                    //    X509Certificate[] certChain = CertUtil.GetX509CertChain(cert);
                    //    dataInfo += String.Format("{0} đã ký - {1}", SharedUtils.GetCN(certChain[0]), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    //}
                    //else
                    //{
                    //    dataInfo += String.Format("{0} đã ký - {1}", displayConfig.Contact, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    //}
                    //if (!String.IsNullOrWhiteSpace(reason))
                    //{
                    //    dataInfo += String.Format("{0} ", reason);
                    //}

                    string extSig = "";
                    var arrHash = this.EncodeData(SharedUtils.FileToByte(jsonFile), SignPdfAsynchronous.HASH_ALG); // this.GetHashTypeRectangleText(inFile, certChain, reason, location, displayConfig.IsDisplaySignNote);
                    if (dlgGetHSMServerResponseData != null)
                    {
                        string s = ByteArrayToHexString(arrHash);
                        try
                        {
                            extSig = dlgGetHSMServerResponseData(s, ref errMessage);
                        }
                        catch { }
                        if (String.IsNullOrEmpty(extSig))
                        {
                            extSig = Convert.ToBase64String(arrHash);
                        }
                    }
                    else if (cert != null && cert.PrivateKey != null)
                    {
                        RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
                        Inventec.Common.Logging.LogSystem.Debug((!String.IsNullOrEmpty(pinCode)) ? "pinCode has set" : "pinCode not set");
                        if (!String.IsNullOrEmpty(pinCode))
                        {
                            Inventec.Common.Logging.LogSystem.Debug("pinCode has set");

                            try
                            {
                                string preSerialNumber = KeyStore.GetValue(KeyStore.SERIALNUMBER_KEY);
                                if (!String.IsNullOrEmpty(preSerialNumber) && preSerialNumber != cert.SerialNumber)
                                {
                                    KeyStore.SetValue(KeyStore.CHANGE_USB_KEY, "1");
                                    pinCode = "";
                                    Inventec.Common.Logging.LogSystem.Info("check preSerialNumber => 1");
                                }
                                else
                                {
                                    KeyStore.SetValue(KeyStore.CHANGE_USB_KEY, "0");
                                    Inventec.Common.Logging.LogSystem.Info("check preSerialNumber => 2");
                                }
                                Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => preSerialNumber), preSerialNumber) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => cert.SerialNumber), cert.SerialNumber));
                                CspParameters cspp = new CspParameters();
                                cspp.KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName;
                                cspp.ProviderName = rsa.CspKeyContainerInfo.ProviderName;
                                cspp.ProviderType = rsa.CspKeyContainerInfo.ProviderType;
                                cspp.KeyPassword = SharedUtils.GetSecurePin(pinCode);

                                Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("cspp", cspp));

                                //the pin code will be cached for next access to the smart card
                                RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider(cspp);

                                KeyStore.SetValue(KeyStore.SERIALNUMBER_KEY, cert.SerialNumber);
                            }
                            catch (Exception ex)
                            {
                                Inventec.Common.Logging.LogSystem.Warn("PIN code Cached for next access to the smart card fail: " + ex.Message);
                            }
                        }

                        string s = Convert.ToBase64String(arrHash);
                        new SHA1Managed();
                        Inventec.Common.Logging.LogSystem.Debug("SignJson.1.1");
                        extSig = Convert.ToBase64String((rsa).SignHash(Convert.FromBase64String(s), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));
                        Inventec.Common.Logging.LogSystem.Debug("SignJson.1.2");
                    }


                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, checkSumFile),
                            new Claim(ClaimTypes.Name, dataInfo),
                            new Claim(ClaimTypes.UserData, extSig),
                        }),

                        Expires = now.AddYears(10),
                        SigningCredentials = new SigningCredentials(GetRsaKey(Inventec.Common.SignFile.SharedUtils.CertManager.Certificate), SecurityAlgorithms.RsaSha256Signature)
                    };

                    SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    string outData = tokenHandler.WriteToken(securityToken);

                    if (!String.IsNullOrEmpty(outData))
                    {
                        success = true;
                        var writer = new StreamWriter(outStream);
                        string strSignResultWriter = "";
                        if (jsonData.Contains("<Signature>") && jsonData.Contains("</Signature>"))
                        {
                            string dataRaw = jsonData.Substring(0, jsonData.IndexOf("<Signature>"));
                            strSignResultWriter = dataRaw;
                        }
                        else
                        {
                            strSignResultWriter = jsonData;
                        }
                        strSignResultWriter += String.Format("<Signature><SignatureCertificate>{0}</SignatureCertificate><SignatureDisplayInfo>{1}</SignatureDisplayInfo></Signature>", outData, dataInfo);
                        writer.Write(strSignResultWriter);
                        writer.Flush();
                        outStream.Position = 0;
                    }
                }

                Inventec.Common.Logging.LogSystem.Debug("SignJson. 2");
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        public bool VerifySignedJson(X509Certificate2 cert, string jsonSignedData, ref string signedInfoData, ref string errMessage)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignJson. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => jsonSignedData), jsonSignedData));

                if (!String.IsNullOrEmpty(jsonSignedData))
                {
                    //string jsonSigned = File.ReadAllText(jsonSignedFile);
                    var principal = GetPrincipal(jsonSignedData, cert);
                    if (principal == null)
                    {
                        errMessage = "Verify thất bại";
                    }
                    else
                    {
                        errMessage = "";
                        errMessage = "";
                        var claim__Name = principal.FindFirst(ClaimTypes.Name);
                        if (claim__Name != null && !String.IsNullOrEmpty(claim__Name.Value))
                        {
                            var name = (claim__Name.Value);
                            signedInfoData += name;
                        }
                    }
                }

                Inventec.Common.Logging.LogSystem.Debug("SignJson. 2");
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        internal bool SignJson2(X509Certificate2 cert, string jsonData, Stream outStream, string reason, string location, DisplayConfig displayConfig, ref string errMessage)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignJson. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig));
                this.displayConfigParam = displayConfig;
                // Create a new XML document.
                if (!String.IsNullOrEmpty(jsonData))
                {
                    RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey;
                    // Hash the data
                    SHA1Managed sha1 = new SHA1Managed();
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    byte[] data = encoding.GetBytes(jsonData);
                    byte[] hash = sha1.ComputeHash(data);

                    // Sign the hash
                    var signHashed = csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
                    if (signHashed != null && signHashed.Length > 0)
                    {
                        outStream = new MemoryStream(signHashed);
                        outStream.Position = 0;
                    }
                }

                Inventec.Common.Logging.LogSystem.Debug("SignJson. 2");
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        internal bool VerifySignedJson2(X509Certificate2 cert, string jsonSignedFile, ref string errMessage)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignJson. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => jsonSignedFile), jsonSignedFile));

                if (!String.IsNullOrEmpty(jsonSignedFile))
                {
                    string jsonSigned = File.ReadAllText(jsonSignedFile);

                    //string jsonData = File.ReadAllText("576e990d20191Signed.json");
                    //byte[] signature = File.ReadAllBytes("576e990d20192Signed.json");
                    //// Verify signature. Testcert.cer corresponds to "cn=my cert subject"
                    //if (VerifyJsonNew(jsonData, signature))
                    //{
                    //    Console.WriteLine("Signature verified");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("ERROR: Signature not valid!");
                    //}


                    var principal = GetPrincipal(jsonSigned, cert);
                    if (principal == null)
                    {
                        errMessage = "Verify thất bại";
                    }
                    else
                    {
                        errMessage = "OK";
                    }
                }

                Inventec.Common.Logging.LogSystem.Debug("SignJson. 2");
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        internal bool SignPDFElectronic(string inFile, string outFile, string reason, string location, DisplayConfig displayConfig, ref string errMessage)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignPDFElectronic. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig));
                displayConfig.Reason = reason;
                this.displayConfigParam = displayConfig;


                PdfReader readerWorking = new PdfReader(inFile);
                int pageCount = readerWorking.NumberOfPages;

                if (!String.IsNullOrEmpty(displayConfig.Contact))
                {
                    displayConfig.Contact = displayConfig.Contact.ToUpper();
                }

                Image instance = null;
                if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT || displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
                {
                    if (!String.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
                    {
                        instance = Image.GetInstance(displayConfig.PathImage);
                    }
                    else if (displayConfig.BImage != null)
                    {
                        instance = Image.GetInstance(displayConfig.BImage);
                    }
                }

                float coorXRectangle = displayConfig.CoorXRectangle - (displayConfig.WidthRectangle / 2);
                float coorYRectangle = displayConfig.CoorYRectangle;// -(displayConfig.HeightRectangle / 2);
                float widthRectangle = displayConfig.WidthRectangle;
                float heightRectangle = displayConfig.HeightRectangle;
                string strDate = string.Format(displayConfig.DateFormatstring, (displayConfig.SignDate == DateTime.MinValue ? DateTime.Now : displayConfig.SignDate));

                SignPdfAsynchronous.ProcessFontSizeFit(displayConfig);

                using (FileStream fs_ = File.Open(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (PdfStamper stam = new PdfStamper(readerWorking, fs_))
                    {
                        for (int i = 1; i <= pageCount; i++)
                        {
                            if (i == displayConfig.NumberPageSign)
                            {
                                iTextSharp.text.Rectangle rec = readerWorking.GetPageSize(i);
                                PdfContentByte cb = stam.GetOverContent(i);
                                Inventec.Common.Logging.LogSystem.Debug("SignPDFElectronic____PageNUm=" + displayConfig.NumberPageSign
                                    + "__WidthRectangle=" + displayConfig.WidthRectangle
                                    + "__HeightRectangle=" + displayConfig.HeightRectangle
                                    + "__CoorXRectangle=" + displayConfig.CoorXRectangle
                                    + "__CoorYRectangle=" + displayConfig.CoorYRectangle
                                    );

                                success = true;

                                if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP && instance != null)
                                {
                                    PdfPCell imageCell = new PdfPCell();
                                    if (instance != null)
                                    {
                                        instance.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                                        float plusH = SignPdfAsynchronous.ProcessHeightPlus(100, displayConfig);
                                        instance.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, instance, displayConfig.SignaltureImageWidth, 100, plusH);

                                        imageCell.AddElement(instance);
                                        imageCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        imageCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                        imageCell.Border = Rectangle.NO_BORDER;
                                        imageCell.MinimumHeight = heightRectangle;
                                    }

                                    PdfPTable newtable = new PdfPTable(1);
                                    newtable.TotalWidth = widthRectangle;
                                    newtable.LockedWidth = true;
                                    newtable.AddCell(imageCell);
                                    newtable.WriteSelectedRows(0, -1, coorXRectangle, coorYRectangle + (newtable.TotalHeight / 2), cb);

                                    //cb.EndLayer();
                                }
                                else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
                                {
                                    float width = widthRectangle;
                                    string displayText = SignPdfAsynchronous.GetDisplayText(displayConfig, strDate);

                                    float widthImagePercent = 0;

                                    PdfPTable newtable = null;
                                    if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100)
                                    {
                                        newtable = new PdfPTable(1);
                                        widthImagePercent = 100;
                                    }
                                    else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75)
                                    {
                                        newtable = new PdfPTable(new float[] { 25, 75 });
                                        widthImagePercent = 25;
                                    }
                                    else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70)
                                    {
                                        newtable = new PdfPTable(new float[] { 30, 70 });
                                        widthImagePercent = 30;
                                    }
                                    else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60)
                                    {
                                        newtable = new PdfPTable(new float[] { 40, 60 });
                                        widthImagePercent = 40;
                                    }
                                    else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50)
                                    {
                                        newtable = new PdfPTable(new float[] { 50, 50 });
                                        widthImagePercent = 50;
                                    }
                                    else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x60x40)
                                    {
                                        newtable = new PdfPTable(new float[] { 60, 40 });
                                        widthImagePercent = 40;
                                    }
                                    else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x70x30)
                                    {
                                        newtable = new PdfPTable(new float[] { 70, 30 });
                                        widthImagePercent = 30;
                                    }
                                    else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x75x25)
                                    {
                                        newtable = new PdfPTable(new float[] { 75, 25 });
                                        widthImagePercent = 25;
                                    }
                                    float plusH = SignPdfAsynchronous.ProcessHeightPlus(widthImagePercent, displayConfig);

                                    PdfPCell imageCell = new PdfPCell();
                                    if (instance != null)
                                    {
                                        instance.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                                        instance.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, instance, displayConfig.SignaltureImageWidth, widthImagePercent, plusH);

                                        imageCell.AddElement(instance);
                                        imageCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        imageCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                        imageCell.Border = Rectangle.NO_BORDER;
                                    }

                                    PdfPCell textCell = SignPdfFile.GetTextCell(displayText, displayConfig);

                                    newtable.TotalWidth = width;
                                    newtable.LockedWidth = true;

                                    if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100
                                        || displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75
                                        || displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70
                                        || displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60
                                        || displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50
                                        )
                                    {
                                        newtable.AddCell(imageCell);
                                        newtable.AddCell(textCell);
                                    }
                                    else
                                    {
                                        newtable.AddCell(textCell);
                                        newtable.AddCell(imageCell);
                                    }


                                    PdfPTable newtableParent = new PdfPTable(1);
                                    PdfPCell imageCellParent = new PdfPCell();

                                    imageCellParent.AddElement(newtable);
                                    imageCellParent.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    imageCellParent.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                    imageCellParent.Border = Rectangle.NO_BORDER;
                                    imageCellParent.MinimumHeight = heightRectangle;

                                    newtableParent.TotalWidth = width;
                                    newtableParent.LockedWidth = true;
                                    newtableParent.AddCell(imageCellParent);

                                    newtableParent.WriteSelectedRows(0, -1, coorXRectangle, coorYRectangle + (newtableParent.TotalHeight / 2), cb);
                                }
                                else if (displayConfig.TypeDisplay == Constans.DISPLAY_RECTANGLE_TEXT)
                                {
                                    float width = widthRectangle;

                                    string displayText = SignPdfAsynchronous.GetDisplayText(displayConfig, strDate);
                                    PdfPCell textCell = SignPdfFile.GetTextCell(displayText, displayConfig);
                                    textCell.MinimumHeight = heightRectangle;

                                    PdfPTable newtable = new PdfPTable(1);
                                    newtable.TotalWidth = width;
                                    newtable.LockedWidth = true;
                                    newtable.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                                    newtable.AddCell(textCell);
                                    newtable.CompleteRow();
                                    newtable.WriteSelectedRows(0, -1, coorXRectangle, coorYRectangle + (newtable.TotalHeight / 2), cb);
                                }
                            }
                        }
                    }
                }

                readerWorking.Close();
                Inventec.Common.Logging.LogSystem.Debug("SignPDFElectronic. 2");
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        internal bool SignPDFElectronic(string inFile, Stream outStream, string reason, string location, DisplayConfig displayConfig, ref string errMessage)
        {
            bool success = false;
            try
            {
                string outFile = SharedUtils.GenerateTempFile();
                success = SignPDFElectronic(inFile, outFile, reason, location, displayConfig, ref errMessage);
                if (success && File.Exists(outFile))
                {
                    var bFile = SharedUtils.FileToByte(outFile);
                    MemoryStream ms = new MemoryStream(bFile);
                    ms.CopyTo(outStream);
                    outStream.Position = 0;
                }
                Inventec.Common.Logging.LogSystem.Debug("outStream!=null" + (outStream != null) + ",outStream.length = " + (outStream != null ? outStream.Length : 0) + ",outStream.Position = " + (outStream != null ? outStream.Position : 0));
                DeleteFileTemp(outFile);
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        internal bool SignPDFDigital(X509Certificate2 cert, string inFile, string outFile, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, string pinCode)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignPDFDigital. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => reason), reason));
                this.displayConfigParam = displayConfig;
                CertUtil.GetX509Cert(cert);
                X509Certificate[] certChain = CertUtil.GetX509CertChain(cert);
                string extSig = "";
                var arrHash = this.GetHashTypeRectangleText(inFile, certChain, reason, location, displayConfig.IsDisplaySignNote);

                if (dlgGetHSMServerResponseData != null)
                {
                    string s = ByteArrayToHexString(arrHash);
                    extSig = dlgGetHSMServerResponseData(s, ref errMessage);
                }
                else
                {
                    RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
                    Inventec.Common.Logging.LogSystem.Debug((!String.IsNullOrEmpty(pinCode)) ? "pinCode has set" : "pinCode not set");
                    if (!String.IsNullOrEmpty(pinCode))
                    {
                        Inventec.Common.Logging.LogSystem.Debug("pinCode has set");

                        try
                        {
                            string preSerialNumber = KeyStore.GetValue(KeyStore.SERIALNUMBER_KEY);
                            if (!String.IsNullOrEmpty(preSerialNumber) && preSerialNumber != cert.SerialNumber)
                            {
                                KeyStore.SetValue(KeyStore.CHANGE_USB_KEY, "1");
                                pinCode = "";
                                Inventec.Common.Logging.LogSystem.Info("check preSerialNumber => 1");
                            }
                            else
                            {
                                KeyStore.SetValue(KeyStore.CHANGE_USB_KEY, "0");
                                Inventec.Common.Logging.LogSystem.Info("check preSerialNumber => 2");
                            }
                            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => preSerialNumber), preSerialNumber) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => cert.SerialNumber), cert.SerialNumber));
                            CspParameters cspp = new CspParameters();
                            cspp.KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName;
                            cspp.ProviderName = rsa.CspKeyContainerInfo.ProviderName;
                            cspp.ProviderType = rsa.CspKeyContainerInfo.ProviderType;
                            cspp.KeyPassword = SharedUtils.GetSecurePin(pinCode);

                            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("cspp", cspp));

                            //the pin code will be cached for next access to the smart card
                            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider(cspp);

                            KeyStore.SetValue(KeyStore.SERIALNUMBER_KEY, cert.SerialNumber);
                        }
                        catch (Exception ex)
                        {
                            Inventec.Common.Logging.LogSystem.Warn("PIN code Cached for next access to the smart card fail: " + ex.Message);
                        }
                    }

                    string s = Convert.ToBase64String(arrHash);
                    new SHA1Managed();
                    Inventec.Common.Logging.LogSystem.Debug("SignPDFDigital.1.1");
                    extSig = Convert.ToBase64String((rsa).SignHash(Convert.FromBase64String(s), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));
                    Inventec.Common.Logging.LogSystem.Debug("SignPDFDigital.1.2");
                }
                if (!String.IsNullOrEmpty(extSig))
                {
                    Inventec.Common.Logging.LogSystem.Debug("SignPDFDigital.1.3");
                    this.InsertSignature(extSig, outFile, timestampConfig);
                    Inventec.Common.Logging.LogSystem.Debug("SignPDFDigital.1.4");
                    return true;
                }
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
                System.Windows.Forms.MessageBox.Show("Chứng thư không hợp lệ, vui lòng chọn lại");
            }
            return false;
        }

        internal bool SignPDFDigital(X509Certificate2 cert, string inFile, string outFile, string reason, string location, bool useTimestamp, DisplayConfig displayConfig, ref string errMessage)
        {
            try
            {
                CertUtil.GetX509Cert(cert);
                X509Certificate[] certChain = CertUtil.GetX509CertChain(cert);
                this.displayConfigParam = displayConfig;
                string extSig = "";
                var arrHash = this.GetHashTypeRectangleText(inFile, certChain, reason, location, displayConfig.IsDisplaySignNote);

                string s = Convert.ToBase64String(arrHash);
                new SHA1Managed();
                extSig = Convert.ToBase64String(((RSACryptoServiceProvider)cert.PrivateKey).SignHash(Convert.FromBase64String(s), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));

                if (!String.IsNullOrEmpty(extSig))
                {
                    TimestampConfig timestampConfig = new TimestampConfig();
                    timestampConfig.UseTimestamp = true;
                    timestampConfig.TsaUrl = "http://ca.gov.vn/tsa";

                    this.InsertSignature(extSig, outFile, timestampConfig);
                    return true;
                }
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return false;
        }

        internal bool SignPDFDigital(X509Certificate2 cert, string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage)
        {
            try
            {
                return SignPDFDigital(cert, inFile, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, false);
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return false;
        }

        internal bool SignPDFDigital(X509Certificate2 cert, string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer)
        {
            bool success = false;
            try
            {
                CertUtil.GetX509Cert(cert);
                X509Certificate[] certChain = CertUtil.GetX509CertChain(cert);

                this.displayConfigParam = displayConfig;
                if (hasHashPkcsServer)
                {
                    success = EmptySignatureHashPkcsServer(inFile, outStream, SharedUtils.getSignName(), reason, location, certChain[0], dlgGetHSMServerResponseData, ref errMessage);
                }
                else
                {
                    string extSig = "";
                    var arrHash = this.GetHashTypeRectangleText(inFile, certChain, reason, location, displayConfig.IsDisplaySignNote);
                    if (dlgGetHSMServerResponseData != null)
                    {
                        string hexHash = ByteArrayToHexString(arrHash);
                        extSig = dlgGetHSMServerResponseData(hexHash, ref errMessage);
                        if (String.IsNullOrEmpty(extSig) || !String.IsNullOrEmpty(errMessage))
                        {
                            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("errMessage", errMessage) + "____extSig!=null=" + (extSig != null));
                        }
                    }
                    else
                    {
                        string base64Hash = Convert.ToBase64String(arrHash);
                        new SHA1Managed();
                        extSig = Convert.ToBase64String(((RSACryptoServiceProvider)cert.PrivateKey).SignHash(Convert.FromBase64String(base64Hash), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));
                    }
                    if (!String.IsNullOrEmpty(extSig))
                    {
                        this.InsertSignature(extSig, outStream, timestampConfig);
                        success = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            return success;
        }

        public bool SignPDFAllPage(X509Certificate2 cert, Stream inStream, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage)
        {
            string src = "";
            bool success = false;
            SharedUtils.SaveNewFileFromReader(inStream, ref src);
            try
            {
                if (File.Exists(src))
                {
                    success = SignPDFDigitalAllPage(cert, ref src, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, false);
                }

                DeleteFileTemp(src);
                DisposeFileStream(inStream);
            }
            catch (Exception exception)
            {
                success = false;
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }

            return success;
        }

        public bool SignPDFAllPage(X509Certificate2 cert, Stream inStream, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer, bool isAutoChangeSignLocation = false)
        {
            string src = "";
            bool success = false;
            SharedUtils.SaveNewFileFromReader(inStream, ref src);
            try
            {
                if (File.Exists(src))
                {
                    success = SignPDFDigitalAllPage(cert, ref src, outStream, reason, location, timestampConfig, displayConfig, dlgGetHSMServerResponseData, ref errMessage, hasHashPkcsServer, isAutoChangeSignLocation);
                }

                DeleteFileTemp(src);
                DisposeFileStream(inStream);
            }
            catch (Exception exception)
            {
                success = false;
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }

            return success;
        }

        internal bool SignPDFDigitalAllPage(X509Certificate2 cert, ref string inFile, Stream outStream, string reason, string location, TimestampConfig timestampConfig, DisplayConfig displayConfig, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage, bool hasHashPkcsServer = false, bool isAutoChangeSignLocation = false)
        {
            PdfReader reader = null;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("SignPDFDigitalAllPage. 1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig));
                //ký tất cả các trang
                //lấy tổng số trang ở file gốc
                reader = new PdfReader(inFile);
                int numberOfPages = reader.NumberOfPages;

                X509Certificate[] certChain = CertUtil.GetX509CertChain(cert);

                for (int i = 1; i <= numberOfPages; i++)
                {
                    this.fieldName = "";
                    this.signDate = DateTime.Now;
                    this.tmpFile = "";
                    this.hash = null;
                    this.chain = null;

                    //Nếu vị trí ký nằm ngoài khổ giấy thì xử lý xác định lại vị trí ký
                    float coorX = displayConfig.CoorXRectangle;
                    float coorY = displayConfig.CoorYRectangle;
                    if (isAutoChangeSignLocation)
                    {
                        Rectangle currentPageRectangle = reader.GetPageSizeWithRotation(i);
                        coorX = (displayConfig.CoorXRectangle / PageSize.A4.Width) * currentPageRectangle.Width;
                        coorY = (displayConfig.CoorYRectangle / PageSize.A4.Height) * currentPageRectangle.Height;
                        if (coorX <= (displayConfig.WidthRectangle / 2))
                            coorX = (displayConfig.WidthRectangle / 2);
                        else if (coorX >= currentPageRectangle.Width - displayConfig.WidthRectangle / 2)
                            coorX = (currentPageRectangle.Width - displayConfig.WidthRectangle / 2);
                        if (coorY <= displayConfig.HeightRectangle / 2)
                            coorY = (displayConfig.HeightRectangle / 2);
                        else if (coorY >= currentPageRectangle.Height - displayConfig.HeightRectangle / 2)
                            coorY = (currentPageRectangle.Height - displayConfig.HeightRectangle / 2);
                    }

                    using (MemoryStream signStream = new MemoryStream())
                    {
                        if (hasHashPkcsServer)
                        {
                            bool success = EmptySignatureHashPkcsServer(inFile, signStream, SharedUtils.getSignName(), reason, location, certChain[0], dlgGetHSMServerResponseData, ref errMessage);
                        }

                        else
                        {
                            DisplayConfig displayConfigParam = DisplayConfig.generateDisplayConfigRectangleText(
                                            displayConfig.TypeDisplay,
                                            displayConfig.SizeFont,
                                            displayConfig.TextPosition,
                                            displayConfig.PathImage,
                                            displayConfig.BImage,
                                            i,
                                            coorX,
                                            coorY,
                                            (displayConfig.WidthRectangle > 0 ? displayConfig.WidthRectangle : 320f),
                                            (displayConfig.HeightRectangle > 0 ? displayConfig.HeightRectangle : 140f),
                                            null,
                                            (!String.IsNullOrEmpty(displayConfig.FormatRectangleText) ? displayConfig.FormatRectangleText : Constans.SIGN_TEXT_FORMAT_3_1),
                                            SharedUtils.GetCN(certChain[0]),
                                            (!String.IsNullOrEmpty(displayConfig.Reason) ? displayConfig.Reason : reason),
                                            (!String.IsNullOrEmpty(displayConfig.Location) ? displayConfig.Location : location),
                                            Constans.DATE_FORMAT_1);

                            displayConfigParam.IsDisplaySignature = displayConfig.IsDisplaySignature;
                            displayConfigParam.SignaltureImageWidth = displayConfig.SignaltureImageWidth;
                            displayConfigParam.TotalPageSign = numberOfPages;
                            displayConfigParam.TextFormat = displayConfig.TextFormat;

                            string extSig = "";
                            var arrHash = this.CreateHash(inFile, certChain, displayConfigParam);
                            if (dlgGetHSMServerResponseData != null)
                            {
                                string hexHash = (hasHashPkcsServer ? ByteArrayToHexString(this.hash) : ByteArrayToHexString(arrHash));
                                extSig = dlgGetHSMServerResponseData(hexHash, ref errMessage);
                                if (String.IsNullOrEmpty(extSig) || !String.IsNullOrEmpty(errMessage))
                                {
                                    Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("errMessage", errMessage) + "____extSig!=null=" + (extSig != null));
                                }
                            }
                            else
                            {
                                string base64Hash = Convert.ToBase64String(arrHash);
                                new SHA1Managed();
                                extSig = Convert.ToBase64String(((RSACryptoServiceProvider)cert.PrivateKey).SignHash(Convert.FromBase64String(base64Hash), CryptoConfig.MapNameToOID(SignPdfAsynchronous.HASH_ALG)));
                            }

                            if (String.IsNullOrEmpty(extSig) || !this.InsertSignature(extSig, signStream, timestampConfig))
                            {
                                return false;
                            }
                        }

                        //xóa file mỗi lần ký
                        DeleteFileTemp(inFile);
                        signStream.Position = 0;

                        //mỗi lần ký sẽ lưu lại file ký
                        if (i == numberOfPages)
                        {
                            signStream.CopyTo(outStream);
                        }
                        else
                        {
                            inFile = SharedUtils.GenerateTempFile();
                            using (Stream saveFile = File.Open(inFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                signStream.CopyTo(saveFile);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return false;
        }

        internal static PdfPCell GetTextCell(string displayText, DisplayConfig displayConfig)
        {
            int alignment = Element.ALIGN_CENTER;
            if (displayConfig.TextFormat != null)
            {
                switch (displayConfig.TextFormat.Alignment)
                {
                    case ALIGNMENT_OPTION.Left:
                        alignment = Element.ALIGN_LEFT;
                        break;
                    case ALIGNMENT_OPTION.Center:
                        alignment = Element.ALIGN_CENTER;
                        break;
                    case ALIGNMENT_OPTION.Right:
                        alignment = Element.ALIGN_RIGHT;
                        break;
                    case ALIGNMENT_OPTION.Justify:
                        alignment = Element.ALIGN_JUSTIFIED;
                        break;
                    default:
                        alignment = Element.ALIGN_CENTER;
                        break;
                }
            }
            Font fontText = GetFontByConfig(displayConfig);
            Paragraph elementTextCell = new Paragraph(displayText, fontText)
            {
                Alignment = alignment
            };
            PdfPCell textCell = new PdfPCell(elementTextCell);
            textCell.HorizontalAlignment = alignment;
            textCell.VerticalAlignment = Element.ALIGN_TOP;
            textCell.Border = Rectangle.NO_BORDER;
            return textCell;
        }

        internal static Font GetFontByConfig(DisplayConfig displayConfig)
        {
            int style = 0;
            string fontName = null;
            if (displayConfig.TextFormat != null)
            {
                fontName = (displayConfig.TextFormat.FontName ?? "").ToLower();
                if (displayConfig.TextFormat.IsBold) style += Font.BOLD;
                if (displayConfig.TextFormat.IsItalic) style += Font.ITALIC;
                if (displayConfig.TextFormat.IsUnderlined) style += Font.UNDERLINE;
            }
            BaseFont baseFont = null;
            if (!String.IsNullOrWhiteSpace(fontName))
            {
                try
                {
                    string fontName_TFF = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), String.Format("{0}.ttf", fontName));
                    baseFont = BaseFont.CreateFont(fontName_TFF, BaseFont.IDENTITY_H, true) ?? BaseFont.CreateFont(displayConfig.FontPath, BaseFont.IDENTITY_H, true);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Warn(ex);
                    baseFont = BaseFont.CreateFont(displayConfig.FontPath, BaseFont.IDENTITY_H, true);
                }
            }
            else
            {
                baseFont = BaseFont.CreateFont(displayConfig.FontPath, BaseFont.IDENTITY_H, true);
            }
            Font fontText = new Font(baseFont, (float)displayConfig.SizeFont, style);
            return fontText;
        }

        #endregion

        #region Private method
        private System.Security.Claims.ClaimsPrincipal GetPrincipal(string jsonDataSigned, X509Certificate2 cert)
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

        private RsaSecurityKey GetRsaKey(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
        {
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificate.PrivateKey;
            return new RsaSecurityKey(rsa);
        }

        private void SignXmlProcess(XmlDocument xmlDoc, X509Certificate2 cert)
        {

            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
            if (cert == null)
                throw new ArgumentException("cert");

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(xmlDoc);

            // Add the key to the SignedXml document.
            signedXml.SigningKey = cert.PrivateKey;// signedXml.SigningKey = manager.Certificate.PrivateKey;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data(cert);
            keyInfo.AddClause(keyInfoData);
            signedXml.KeyInfo = keyInfo;

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the XML document.
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
        }

        private void AddPageEmpty(string src, string dest, DisplayConfig config)
        {
            PdfReader reader = null;
            FileStream os = null;
            try
            {
                Paragraph paragraph;
                reader = new PdfReader(src);
                os = new FileStream(dest, FileMode.Append);
                PdfStamper stamper = new PdfStamper(reader, os);
                for (int i = 0; i < config.NumberPageSign; i++)
                {
                    stamper.InsertPage(i, config.PageSize);
                }
                PdfContentByte underContent = stamper.GetUnderContent(1);
                float width = config.PageSize.Width;
                float marginRight = config.MarginRight;
                float lly = (config.PageSize.Height - config.MarginTop) - config.HeightTitle;
                float heightTitle = config.HeightTitle;
                float num5 = width - (config.MarginRight * 2f);
                BaseFont bf = BaseFont.CreateFont(config.FontPath, "Identity-H", true);
                if (config.IsDisplayTitlePageSign)
                {
                    ColumnText text1 = new ColumnText(underContent);
                    text1.SetSimpleColumn(marginRight, lly + config.HeightTitle, marginRight + num5, (lly + config.HeightTitle) + config.HeightRowTitlePageSign);
                    paragraph = new Paragraph(config.TitlePageSign, new Font(bf, (float)config.FontSizeTitlePageSign, 1))
                    {
                        Alignment = 1
                    };
                    text1.AddElement(paragraph);
                    text1.Go();
                }
                PdfPTable element = new PdfPTable(config.WidthsPercen.Length);
                element.SetWidths(config.WidthsPercen);
                element.WidthPercentage = 100f;
                string[] titles = config.Titles;
                for (int j = 0; j < titles.Length; j++)
                {
                    paragraph = new Paragraph(titles[j], new Font(bf, (float)config.SizeFont, 1))
                    {
                        Alignment = 1
                    };
                    PdfPCell cell = new PdfPCell();
                    cell.AddElement(paragraph);
                    cell.FixedHeight = config.HeightTitle;
                    cell.BackgroundColor = config.BackgroundColorTitle;
                    element.AddCell(cell);
                }
                ColumnText text2 = new ColumnText(underContent);
                text2.SetSimpleColumn(marginRight, lly, marginRight + num5, lly + config.HeightTitle);
                text2.AddElement(element);
                text2.Go();
                stamper.Close();
                reader.Close();
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
            finally
            {
                if (os != null)
                {
                    try
                    {
                        os.Close();
                    }
                    catch (IOException exception2)
                    {
                        Inventec.Common.Logging.LogSystem.Warn(exception2);
                    }
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private byte[] CreateHash(string filePath, X509Certificate[] chain, DisplayConfig displayConfig)
        {
            try
            {
                string tempFile = SharedUtils.GenerateTempFile();
                this.fieldName = SharedUtils.getSignName();
                DateTime signDate = displayConfig.SignDate;
                this.signDate = displayConfig.SignDate;
                List<byte[]> list = new SignPdfAsynchronous().CreateHash(filePath, tempFile, this.fieldName, chain, displayConfig);
                if (list == null)
                {
                    return null;
                }
                this.tmpFile = tempFile;
                this.hash = list[1];
                this.chain = chain;
                return this.EncodeData(list[0], SignPdfAsynchronous.HASH_ALG);
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
                return null;
            }
        }

        private bool EmptySignatureHashPkcsServer(string inFile, Stream outStream, string fieldName, string reason, string location, X509Certificate cert, GetHSMServerResponseData dlgGetHSMServerResponseData, ref string errMessage)
        {
            FileStream os = null;
            bool success = false;
            string outFile = "";
            PdfReader reader = null;
            try
            {
                DisplayConfig displayConfig;
                if (this.displayConfigParam != null)
                {
                    displayConfig = DisplayConfig.generateDisplayConfigRectangleText(
                       this.displayConfigParam.TypeDisplay,
                       this.displayConfigParam.SizeFont,
                       this.displayConfigParam.TextPosition,
                       this.displayConfigParam.PathImage,
                       this.displayConfigParam.BImage,
                       this.displayConfigParam.NumberPageSign,
                       this.displayConfigParam.CoorXRectangle,
                       this.displayConfigParam.CoorYRectangle,
                       (this.displayConfigParam.WidthRectangle > 0 ? this.displayConfigParam.WidthRectangle : 320f),
                       (this.displayConfigParam.HeightRectangle > 0 ? this.displayConfigParam.HeightRectangle : 140f),
                       null,
                       (!String.IsNullOrEmpty(this.displayConfigParam.FormatRectangleText) ? this.displayConfigParam.FormatRectangleText : Constans.SIGN_TEXT_FORMAT_3_1),
                       SharedUtils.GetCN(cert),
                       (!String.IsNullOrEmpty(this.displayConfigParam.Reason) ? this.displayConfigParam.Reason : reason),
                       (!String.IsNullOrEmpty(this.displayConfigParam.Location) ? this.displayConfigParam.Location : location),
                       Constans.DATE_FORMAT_1);

                    displayConfig.IsDisplaySignature = this.displayConfigParam.IsDisplaySignature;
                    displayConfig.SignaltureImageWidth = this.displayConfigParam.SignaltureImageWidth;
                    displayConfig.IsDisplaySignNote = (this.displayConfigParam.IsDisplaySignNote.HasValue ? this.displayConfigParam.IsDisplaySignNote : (bool?)false);
                    displayConfig.TextFormat = this.displayConfigParam.TextFormat;
                }
                else
                {
                    displayConfig = DisplayConfig.generateDisplayConfigRectangleText(1, 10f, 10f, 320f, 140f, null, Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(cert), reason, location, Constans.DATE_FORMAT_1);
                }

                outFile = SharedUtils.GenerateTempFile();
                reader = new PdfReader(inFile);
                int numberOfPages = reader.NumberOfPages;
                int numberPageSign = displayConfig.NumberPageSign;
                if ((numberPageSign < 1) || (numberPageSign > numberOfPages))
                {
                    numberPageSign = 1;
                }
                os = new FileStream(outFile, FileMode.Create);
                bool IsRebuilt = reader.IsRebuilt();
                PdfSignatureAppearance signatureAppearance = null;
                if (IsRebuilt)
                {
                    reader.Catalog.Remove(PdfName.PERMS);//TODO
                    reader.RemoveUsageRights();//TODO

                    signatureAppearance = PdfStamper.CreateSignature(reader, os, '\0', null).SignatureAppearance;
                }
                else
                    signatureAppearance = PdfStamper.CreateSignature(reader, os, '\0', null, true).SignatureAppearance;

                displayConfig.SignDate = DateTime.Now;
                DateTime signDate = displayConfig.SignDate;
                if ("".Equals(displayConfig.Contact))
                {
                    displayConfig.Contact = SharedUtils.GetCN(cert);
                }
                signatureAppearance.Contact = displayConfig.Contact;
                signatureAppearance.SignDate = signDate;
                signatureAppearance.Reason = displayConfig.Reason;
                signatureAppearance.Location = displayConfig.Location;
                string strDate = string.Format(displayConfig.DateFormatstring, signDate);
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig));
                if (displayConfig.IsDisplaySignature)
                {
                    float coorXRectangle = displayConfig.CoorXRectangle - (displayConfig.WidthRectangle / 2);
                    float coorYRectangle = displayConfig.CoorYRectangle - (displayConfig.HeightRectangle / 2);
                    float widthRectangle = displayConfig.WidthRectangle;
                    float heightRectangle = displayConfig.HeightRectangle;

                    SignPdfAsynchronous.ProcessFontSizeFit(displayConfig);

                    float wPlus = 0, hPlus = 0;
                    Image instance = null;
                    if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
                    {
                        if (!String.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
                        {
                            instance = Image.GetInstance(displayConfig.PathImage);
                        }
                        else if (displayConfig.BImage != null)
                        {
                            instance = Image.GetInstance(displayConfig.BImage);
                        }
                    }
                    else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
                    {
                        if (!String.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
                        {
                            instance = Image.GetInstance(displayConfig.PathImage);
                        }
                        else if (displayConfig.BImage != null)
                        {
                            instance = Image.GetInstance(displayConfig.BImage);
                        }
                    }

                    if (displayConfig.SignType == Constans.SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD)
                    {
                        Rectangle pageRect = new Rectangle(coorXRectangle, coorYRectangle, coorXRectangle + widthRectangle + wPlus, coorYRectangle + heightRectangle + hPlus);
                        signatureAppearance.SetVisibleSignature(pageRect, numberPageSign, fieldName);
                    }
                    else
                    {
                        signatureAppearance.SetVisibleSignature(fieldName);
                    }

                    if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
                    {
                        float width = widthRectangle;

                        PdfTemplate layer = signatureAppearance.GetLayer(2);
                        float left = layer.BoundingBox.Left;
                        float bottom = layer.BoundingBox.Bottom;
                        float urx = layer.BoundingBox.Width;
                        float ury = layer.BoundingBox.Height;
                        ColumnText column = new ColumnText(layer);
                        PdfPCell imageCell = new PdfPCell();
                        if (instance != null)
                        {
                            instance.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                            float plusH = SignPdfAsynchronous.ProcessHeightPlus(100, displayConfig);
                            instance.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, instance, displayConfig.SignaltureImageWidth, 100, plusH);

                            imageCell.AddElement(instance);
                            imageCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            imageCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                            imageCell.Border = Rectangle.NO_BORDER;
                            imageCell.MinimumHeight = heightRectangle;
                        }

                        PdfPTable newtable = new PdfPTable(1);
                        newtable.TotalWidth = width;
                        newtable.LockedWidth = true;

                        newtable.AddCell(imageCell);

                        column.AddElement(newtable);
                        column.SetSimpleColumn(left, bottom, urx, ury);
                        column.Alignment = Element.ALIGN_CENTER;
                        column.Go();
                    }
                    else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
                    {
                        string displayText = SignPdfAsynchronous.GetDisplayText(displayConfig, strDate);

                        float widthImagePercent = 0;
                        PdfPTable newtable = null;
                        if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100)
                        {
                            newtable = new PdfPTable(1);
                            widthImagePercent = 100;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75)
                        {
                            newtable = new PdfPTable(new float[] { 25, 75 });
                            widthImagePercent = 25;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70)
                        {
                            newtable = new PdfPTable(new float[] { 30, 70 });
                            widthImagePercent = 30;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60)
                        {
                            newtable = new PdfPTable(new float[] { 40, 60 });
                            widthImagePercent = 40;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50)
                        {
                            newtable = new PdfPTable(new float[] { 50, 50 });
                            widthImagePercent = 50;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x60x40)
                        {
                            newtable = new PdfPTable(new float[] { 60, 40 });
                            widthImagePercent = 40;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x70x30)
                        {
                            newtable = new PdfPTable(new float[] { 70, 30 });
                            widthImagePercent = 30;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x75x25)
                        {
                            newtable = new PdfPTable(new float[] { 75, 25 });
                            widthImagePercent = 25;
                        }

                        PdfTemplate layer = signatureAppearance.GetLayer(2);
                        float left = layer.BoundingBox.Left;
                        float bottom = layer.BoundingBox.Bottom;
                        float urx = layer.BoundingBox.Width;
                        float ury = layer.BoundingBox.Height;
                        ColumnText column = new ColumnText(layer);
                        PdfPCell imageCell = new PdfPCell();
                        if (instance != null)
                        {
                            instance.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                            float plusH = SignPdfAsynchronous.ProcessHeightPlus(widthImagePercent, displayConfig);
                            instance.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, instance, displayConfig.SignaltureImageWidth, widthImagePercent, plusH);

                            imageCell.AddElement(instance);
                            imageCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            imageCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                            imageCell.Border = Rectangle.NO_BORDER;

                            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("instance.WidthPercentage", instance.WidthPercentage)
                                 + Inventec.Common.Logging.LogUtil.TraceData("instance.Width", instance.Width)
                                 + Inventec.Common.Logging.LogUtil.TraceData("displayConfig.SignaltureImageWidth", displayConfig.SignaltureImageWidth));

                        }

                        PdfPCell textCell = SignPdfFile.GetTextCell(displayText, displayConfig);

                        newtable.TotalWidth = widthRectangle;
                        newtable.LockedWidth = true;

                        if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100
                            || displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75
                            || displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70
                            || displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60
                            || displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50
                            )
                        {
                            newtable.AddCell(imageCell);
                            newtable.AddCell(textCell);
                        }
                        else
                        {
                            newtable.AddCell(textCell);
                            newtable.AddCell(imageCell);
                        }

                        PdfPTable newtableParent = new PdfPTable(1);
                        PdfPCell imageCellParent = new PdfPCell();

                        imageCellParent.AddElement(newtable);
                        imageCellParent.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        imageCellParent.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        imageCellParent.Border = Rectangle.NO_BORDER;
                        imageCellParent.MinimumHeight = heightRectangle;

                        newtableParent.TotalWidth = widthRectangle;
                        newtableParent.LockedWidth = true;
                        newtableParent.AddCell(imageCellParent);

                        column.AddElement(newtableParent);
                        column.SetSimpleColumn(left, bottom, urx, ury);
                        column.Alignment = Element.ALIGN_CENTER;
                        column.Go();
                    }
                    else if (displayConfig.TypeDisplay == Constans.DISPLAY_RECTANGLE_TEXT)
                    {
                        PdfTemplate layer = signatureAppearance.GetLayer(2);
                        float left = layer.BoundingBox.Left;
                        float bottom = layer.BoundingBox.Bottom;
                        float urx = layer.BoundingBox.Width;
                        float ury = layer.BoundingBox.Height;
                        ColumnText text = new ColumnText(layer);
                        text.SetSimpleColumn(left, bottom, urx, ury);
                        text.Alignment = Element.ALIGN_MIDDLE;
                        string displayText = SignPdfAsynchronous.GetDisplayText(displayConfig, strDate);
                        PdfPCell textCell = SignPdfFile.GetTextCell(displayText, displayConfig);
                        textCell.MinimumHeight = heightRectangle;

                        PdfPTable newtable = new PdfPTable(1);
                        newtable.TotalWidth = widthRectangle;
                        newtable.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                        newtable.LockedWidth = true;


                        newtable.AddCell(textCell);
                        newtable.CompleteRow();

                        text.AddElement(newtable);
                        text.Go();
                    }

                }
                else if (displayConfig.SignType == Constans.SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD)
                {
                    signatureAppearance.SetVisibleSignature(new Rectangle(0f, 0f, 0f, 0f), 1, fieldName);
                }
                else
                {
                    signatureAppearance.SetVisibleSignature(fieldName);
                }

                signatureAppearance.Certificate = cert;
                signatureAppearance.CryptoDictionary = (PdfDictionary)new PdfSignature((PdfName)PdfName.ADOBE_PPKLITE, (PdfName)PdfName.ADBE_PKCS7_DETACHED)
                {
                    Reason = signatureAppearance.Reason,
                    Location = signatureAppearance.Location,
                    Contact = signatureAppearance.Contact,
                    Date = new PdfDate(signatureAppearance.SignDate)
                };

                Dictionary<PdfName, int> dictionary = new Dictionary<PdfName, int>();
                dictionary.Add((PdfName)PdfName.CONTENTS, 16386);
                signatureAppearance.PreClose(dictionary);
                var digest = DigestAlgorithms.Digest(signatureAppearance.GetRangeStream(), SignPdfAsynchronous.HASH_ALG);

                if (dlgGetHSMServerResponseData != null && digest != null)
                {
                    string hexHash = ByteArrayToHexString(digest);
                    string extSig = dlgGetHSMServerResponseData(hexHash, ref errMessage);
                    var extSignature = Convert.FromBase64String(extSig);

                    byte[] numArray = new byte[8192];
                    extSignature.CopyTo((Array)numArray, 0);
                    PdfDictionary pdfDictionary = new PdfDictionary();
                    pdfDictionary.Put((PdfName)PdfName.CONTENTS, (PdfObject)new PdfString(numArray).SetHexWriting(true));
                    signatureAppearance.Close(pdfDictionary);

                    MemoryStream tmpStream = new MemoryStream(SharedUtils.FileToByte(outFile));
                    if (tmpStream != null)
                    {
                        tmpStream.Position = 0;
                        tmpStream.CopyTo(outStream);
                    }
                    tmpStream.Close();
                    tmpStream.Dispose();

                }
                success = true;
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Error(exception);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (os != null)
                {
                    try
                    {
                        os.Close();
                        os.Dispose();
                    }
                    catch (IOException exception2)
                    {
                        LogSystem.Warn("Error emptySignature: " + exception2.Message);
                    }
                }
                try
                {
                    if (!String.IsNullOrEmpty(outFile) && File.Exists(outFile))
                    {
                        File.Delete(outFile);
                    }
                }
                catch (IOException exception2)
                {
                    LogSystem.Warn("Error emptySignature: " + exception2.Message);
                }
            }

            return success;
        }

        private string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private string CreateHashExistedSignatureField(string filePath, X509Certificate[] chain, DisplayConfig displayConfig, string fieldName)
        {
            try
            {
                string tempFile = SharedUtils.GenerateTempFile();
                this.fieldName = fieldName;
                DateTime signDate = displayConfig.SignDate;
                this.signDate = displayConfig.SignDate;
                List<byte[]> list = new SignPdfAsynchronous().CreateHash(filePath, tempFile, fieldName, chain, displayConfig);
                if (list == null)
                {
                    return null;
                }
                this.tmpFile = tempFile;
                this.hash = list[1];
                this.chain = chain;
                return Convert.ToBase64String(this.EncodeData(list[0], SignPdfAsynchronous.HASH_ALG));
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
                return null;
            }
        }

        private byte[] EncodeData(byte[] orginalData, string algorithm)
        {
            byte[] buffer = null;
            if (HASH_ALGORITHM_SHA_1.Equals(algorithm))
            {
                return new SHA1Managed().ComputeHash(orginalData);
            }
            else if (HASH_ALGORITHM_SHA_256.Equals(algorithm))
            {
                buffer = new SHA256Managed().ComputeHash(orginalData);
            }
            else if (HASH_ALGORITHM_SHA_512.Equals(algorithm))
            {
                buffer = new SHA512Managed().ComputeHash(orginalData);
            }
            return buffer;
        }

        private byte[] GetHashTypeRectangleText(string src, X509Certificate[] certChain, string reason, string location)
        {
            DisplayConfig displayConfig;
            if (this.displayConfigParam != null)
            {
                displayConfig = DisplayConfig.generateDisplayConfigRectangleText(
                    this.displayConfigParam.TypeDisplay,
                    this.displayConfigParam.SizeFont,
                    this.displayConfigParam.TextPosition,
                    this.displayConfigParam.PathImage,
                    this.displayConfigParam.BImage,
                    this.displayConfigParam.NumberPageSign,
                    this.displayConfigParam.CoorXRectangle,
                    this.displayConfigParam.CoorYRectangle,
                    (this.displayConfigParam.WidthRectangle > 0 ? this.displayConfigParam.WidthRectangle : 320f),
                    (this.displayConfigParam.HeightRectangle > 0 ? this.displayConfigParam.HeightRectangle : 140f),
                    null,
                    (!String.IsNullOrEmpty(this.displayConfigParam.FormatRectangleText) ? this.displayConfigParam.FormatRectangleText : Constans.SIGN_TEXT_FORMAT_3_1),
                    SharedUtils.GetCN(certChain[0]),
                    (!String.IsNullOrEmpty(this.displayConfigParam.Reason) ? this.displayConfigParam.Reason : reason),
                    (!String.IsNullOrEmpty(this.displayConfigParam.Location) ? this.displayConfigParam.Location : location),
                    Constans.DATE_FORMAT_1);

                displayConfig.IsDisplaySignature = this.displayConfigParam.IsDisplaySignature;
                displayConfig.SignaltureImageWidth = this.displayConfigParam.SignaltureImageWidth;
                displayConfig.TextFormat = this.displayConfigParam.TextFormat;
            }
            else
            {
                displayConfig = DisplayConfig.generateDisplayConfigRectangleText(1, 10f, 10f, 320f, 140f, null, Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(certChain[0]), reason, location, Constans.DATE_FORMAT_1);
            }
            return this.CreateHash(src, certChain, displayConfig);
        }

        private byte[] GetHashTypeRectangleText(string src, X509Certificate[] certChain, string reason, string location, bool? isDisplaySignNote)
        {
            DisplayConfig displayConfig;
            if (this.displayConfigParam != null)
            {
                displayConfig = DisplayConfig.generateDisplayConfigRectangleText(
                    this.displayConfigParam.TypeDisplay,
                    this.displayConfigParam.SizeFont,
                    this.displayConfigParam.TextPosition,
                    this.displayConfigParam.PathImage,
                    this.displayConfigParam.BImage,
                    this.displayConfigParam.NumberPageSign,
                    this.displayConfigParam.CoorXRectangle,
                    this.displayConfigParam.CoorYRectangle,
                    (this.displayConfigParam.WidthRectangle > 0 ? this.displayConfigParam.WidthRectangle : 320f),
                    (this.displayConfigParam.HeightRectangle > 0 ? this.displayConfigParam.HeightRectangle : 140f),
                    null,
                    (!String.IsNullOrEmpty(this.displayConfigParam.FormatRectangleText) ? this.displayConfigParam.FormatRectangleText : Constans.SIGN_TEXT_FORMAT_3_1),
                    SharedUtils.GetCN(certChain[0]),
                    (!String.IsNullOrEmpty(this.displayConfigParam.Reason) ? this.displayConfigParam.Reason : reason),
                    (!String.IsNullOrEmpty(this.displayConfigParam.Location) ? this.displayConfigParam.Location : location),
                    Constans.DATE_FORMAT_1);

                displayConfig.IsDisplaySignature = this.displayConfigParam.IsDisplaySignature;
                displayConfig.SignaltureImageWidth = this.displayConfigParam.SignaltureImageWidth;
                displayConfig.IsDisplaySignNote = (this.displayConfigParam.IsDisplaySignNote.HasValue ? this.displayConfigParam.IsDisplaySignNote : (bool?)false);
                displayConfig.TextFormat = this.displayConfigParam.TextFormat;
            }
            else
            {
                displayConfig = DisplayConfig.generateDisplayConfigRectangleText(1, 10f, 10f, 320f, 140f, null, Constans.SIGN_TEXT_FORMAT_3_1, SharedUtils.GetCN(certChain[0]), reason, location, Constans.DATE_FORMAT_1);
            }
            return this.CreateHash(src, certChain, displayConfig);
        }

        private bool InsertSignature(string extSig, string outFile, TimestampConfig timestampConfig)
        {
            try
            {
                SignPdfAsynchronous asynchronous = new SignPdfAsynchronous();
                if (File.Exists(this.tmpFile))
                {
                    if (asynchronous.InsertSignature(this.tmpFile, outFile, this.fieldName, this.hash, Convert.FromBase64String(extSig), this.chain, this.signDate, timestampConfig))
                    {

                    }
                    try
                    {
                        File.Delete(this.tmpFile);
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
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
                return false;
            }
        }

        private bool InsertSignature(string extSig, Stream outStream, TimestampConfig timestampConfig)
        {
            try
            {
                SignPdfAsynchronous asynchronous = new SignPdfAsynchronous();
                if (File.Exists(this.tmpFile))
                {
                    if (asynchronous.InsertSignature(this.tmpFile, outStream, this.fieldName, this.hash, Convert.FromBase64String(extSig), this.chain, this.signDate, timestampConfig))
                    {

                    }
                    try
                    {
                        File.Delete(this.tmpFile);
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
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
                return false;
            }
        }

        private void DeleteFileTemp(string fileDeletePath)
        {
            try
            {
                //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => fileDeletePath), fileDeletePath));
                if (File.Exists(fileDeletePath)) File.Delete(fileDeletePath);
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
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
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Warn(exception);
            }
        }
        #endregion
    }
}
