using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary
{
    internal class VerifyPdfFileHandle
    {
        internal VerifyPdfFileHandle() { }

        // Methods
        internal List<VerifierADO> verify(PdfReader reader)
        {
            List<VerifierADO> list = new List<VerifierADO>();
            AcroFields acroFields = reader.AcroFields;
            foreach (string str in acroFields.GetSignatureNames())
            {
                iTextSharp.text.pdf.security.PdfPKCS7 fpkcs = acroFields.VerifySignature(str);
                DateTime signDate = fpkcs.SignDate;
                Org.BouncyCastle.X509.X509Certificate[] signCertificateChain = fpkcs.SignCertificateChain;
                if ((signCertificateChain == null) || (signCertificateChain.Length == 0))
                {
                    return null;
                }
                Org.BouncyCastle.X509.X509Certificate cert = signCertificateChain[0];
                System.Security.Cryptography.X509Certificates.X509Certificate2 certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2();
                certificate2.Import(cert.GetEncoded());
                string nameInfo = certificate2.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.DnsName, false);
                string location = fpkcs.Location;
                VerifierADO item = new VerifierADO(cert, certificate2, nameInfo, signDate, !fpkcs.Verify(), location)
                {
                    Comment = fpkcs.Reason,
                    Location = fpkcs.Location,
                    SignerSerialNumber = cert.SerialNumber.ToString(0x10),
                    SignerDN = cert.SubjectDN.ToString(),
                    IsserDN = cert.IssuerDN.ToString(),
                    NotAfter = cert.NotAfter,
                    NotBefore = cert.NotBefore,
                    KeyLength = certificate2.PublicKey.Key.KeySize
                };
                item.SubjectDN = new SubjectDNADO(item.SignerDN);
                if (certificate2.Verify())
                {
                    item.Valid = true;
                }
                else
                {
                    item.Valid = false;
                    //X509Chain chain = new X509Chain
                    //{
                    //    ChainPolicy = { RevocationMode = X509RevocationMode.Online }
                    //};
                    //chain.Build(certificate2);
                    //if (chain.ChainStatus.Length != 0)
                    //{
                    //    item.InvalidReasonList = new List<InvalidReasonADO>();
                    //    int index = 0;
                    //    InvalidReasonADO reason = new InvalidReasonADO
                    //    {
                    //        Status = chain.ChainStatus[index].Status.ToString(),
                    //        ReasonEnLang = chain.ChainStatus[index].StatusInformation
                    //    };
                    //    System.Security.Cryptography.X509Certificates.X509ChainStatusFlags revoked = X509ChainStatusFlags.Revoked;
                    //    if (revoked.Equals(chain.ChainStatus[index].Status))
                    //    {
                    //        reason.ReasonVnLang = "Chứng thư số bị thu hồi hay tạm ngưng";
                    //    }
                    //    else
                    //    {
                    //        revoked = X509ChainStatusFlags.NotTimeValid;
                    //        if (revoked.Equals(chain.ChainStatus[index].Status))
                    //        {
                    //            reason.ReasonVnLang = "Chứng thư số c\x00f3 thời hạn kh\x00f4ng hợp lệ hay hết hạn";
                    //        }
                    //        else
                    //        {
                    //            revoked = X509ChainStatusFlags.NotTimeNested;
                    //            if (revoked.Equals(chain.ChainStatus[index].Status))
                    //            {
                    //                reason.ReasonVnLang = "Kh\x00f4ng được chấp nhận, cờ n\x00e0y kh\x00f4ng c\x00f3 hiệu lực.";
                    //            }
                    //            else
                    //            {
                    //                revoked = X509ChainStatusFlags.NotSignatureValid;
                    //                if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                {
                    //                    reason.ReasonVnLang = "Chữ k\x00fd tr\x00ean Chứng thư số kh\x00f4ng hợp lệ";
                    //                }
                    //                else
                    //                {
                    //                    revoked = X509ChainStatusFlags.NoIssuanceChainPolicy;
                    //                    if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                    {
                    //                        reason.ReasonVnLang = "Key usage kh\x00f4ng hợp lệ.";
                    //                    }
                    //                    else
                    //                    {
                    //                        revoked = X509ChainStatusFlags.UntrustedRoot;
                    //                        if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                        {
                    //                            reason.ReasonVnLang = "Chứng thư số CA kh\x00f4ng hợp lệ";
                    //                        }
                    //                        else
                    //                        {
                    //                            revoked = X509ChainStatusFlags.RevocationStatusUnknown;
                    //                            if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                            {
                    //                                reason.ReasonVnLang = "Trạng th\x00e1i của Chứng thứ số kh\x00f4ng x\x00e1c định";
                    //                            }
                    //                            else
                    //                            {
                    //                                revoked = X509ChainStatusFlags.Cyclic;
                    //                                if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                {
                    //                                    reason.ReasonVnLang = "Chuỗi chứng thư số kh\x00f4ng hợp lệ";
                    //                                }
                    //                                else
                    //                                {
                    //                                    revoked = X509ChainStatusFlags.InvalidExtension;
                    //                                    if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                    {
                    //                                        reason.ReasonVnLang = "Chuỗi chứng thư số kh\x00f4ng hợp lệ do một phần mở rộng kh\x00f4ng hợp lệ.";
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        revoked = X509ChainStatusFlags.InvalidPolicyConstraints;
                    //                                        if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                        {
                    //                                            reason.ReasonVnLang = "Chuỗi chứng thư số kh\x00f4ng hợp lệ do c\x00e1c r\x00e0ng buộc ch\x00ednh s\x00e1ch kh\x00f4ng hợp lệ.";
                    //                                        }
                    //                                        else
                    //                                        {
                    //                                            revoked = X509ChainStatusFlags.InvalidBasicConstraints;
                    //                                            if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                            {
                    //                                                reason.ReasonVnLang = "Chuỗi chứng thư số kh\x00f4ng hợp lệ do c\x00e1c r\x00e0ng buộc cơ bản kh\x00f4ng hợp lệ.";
                    //                                            }
                    //                                            else
                    //                                            {
                    //                                                revoked = X509ChainStatusFlags.InvalidNameConstraints;
                    //                                                if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                {
                    //                                                    reason.ReasonVnLang = "Chuỗi chứng thư số kh\x00f4ng hợp lệ do c\x00e1c r\x00e0ng buộc t\x00ean kh\x00f4ng hợp lệ.";
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    revoked = X509ChainStatusFlags.HasNotSupportedNameConstraint;
                    //                                                    if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                    {
                    //                                                        reason.ReasonVnLang = "Chứng thư số kh\x00f4ng c\x00f3 t\x00ean được hỗ trợ cố định hoặc c\x00f3 một hằng t\x00ean kh\x00f4ng được hỗ trợ.";
                    //                                                    }
                    //                                                    else
                    //                                                    {
                    //                                                        revoked = X509ChainStatusFlags.HasNotDefinedNameConstraint;
                    //                                                        if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                        {
                    //                                                            reason.ReasonVnLang = "Chứng thư số c\x00f3 một hằng t\x00ean kh\x00f4ng x\x00e1c định.";
                    //                                                        }
                    //                                                        else
                    //                                                        {
                    //                                                            revoked = X509ChainStatusFlags.HasNotPermittedNameConstraint;
                    //                                                            if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                            {
                    //                                                                reason.ReasonVnLang = "Chứng thư số c\x00f3 một giới hạn t\x00ean kh\x00f4ng được chấp nhận.";
                    //                                                            }
                    //                                                            else
                    //                                                            {
                    //                                                                revoked = X509ChainStatusFlags.HasExcludedNameConstraint;
                    //                                                                if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                                {
                    //                                                                    reason.ReasonVnLang = "Chuỗi chứng thư số kh\x00f4ng hợp lệ v\x00ec một chứng chỉ đ\x00e3 loại trừ một hạn chế t\x00ean.";
                    //                                                                }
                    //                                                                else
                    //                                                                {
                    //                                                                    revoked = X509ChainStatusFlags.PartialChain;
                    //                                                                    if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                                    {
                    //                                                                        reason.ReasonVnLang = "Chuỗi chứng thư số kh\x00f4ng thể được x\x00e2y dựng với Chứng thư số ROOT.";
                    //                                                                    }
                    //                                                                    else
                    //                                                                    {
                    //                                                                        revoked = X509ChainStatusFlags.CtlNotTimeValid;
                    //                                                                        if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                                        {
                    //                                                                            reason.ReasonVnLang = "Danh s\x00e1ch Chứng thư số được tin tưởng (The Certificate Trust List - CTL) kh\x00f4ng hợp lệ v\x00ec gi\x00e1 trị thời gian kh\x00f4ng hợp lệ.";
                    //                                                                        }
                    //                                                                        else
                    //                                                                        {
                    //                                                                            revoked = X509ChainStatusFlags.CtlNotSignatureValid;
                    //                                                                            if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                                            {
                    //                                                                                reason.ReasonVnLang = "Danh s\x00e1ch Chứng thư số được tin tưởng (The Certificate Trust List - CTL) c\x00f3 chứa một chữ k\x00fd kh\x00f4ng hợp lệ.";
                    //                                                                            }
                    //                                                                            else
                    //                                                                            {
                    //                                                                                revoked = X509ChainStatusFlags.CtlNotValidForUsage;
                    //                                                                                if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                                                {
                    //                                                                                    reason.ReasonVnLang = "Danh s\x00e1ch Chứng thư số được tin tưởng (The Certificate Trust List - CTL) kh\x00f4ng hợp lệ cho việc sử dụng n\x00e0y.";
                    //                                                                                }
                    //                                                                                else
                    //                                                                                {
                    //                                                                                    revoked = X509ChainStatusFlags.OfflineRevocation;
                    //                                                                                    if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                                                    {
                    //                                                                                        reason.ReasonVnLang = "Danh s\x00e1ch thu hồi chứng chỉ trực tuyến (CRL) hiện đang ngoại tuyến (offline).";
                    //                                                                                    }
                    //                                                                                    else
                    //                                                                                    {
                    //                                                                                        revoked = X509ChainStatusFlags.NoIssuanceChainPolicy;
                    //                                                                                        if (revoked.Equals(chain.ChainStatus[index].Status))
                    //                                                                                        {
                    //                                                                                            reason.ReasonVnLang = "Kh\x00f4ng c\x00f3 phần mở rộng ch\x00ednh s\x00e1ch chứng thư số (certificate policy extension) trong chứng thư số.";
                    //                                                                                        }
                    //                                                                                    }
                    //                                                                                }
                    //                                                                            }
                    //                                                                        }
                    //                                                                    }
                    //                                                                }
                    //                                                            }
                    //                                                        }
                    //                                                    }
                    //                                                }
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    item.InvalidReasonList.Add(reason);
                    //}
                }
                list.Add(item);
            }
            return list;
        }

        internal List<VerifierADO> verify(string fileName)
        {
            PdfReader reader = new PdfReader(fileName);
            return this.verify(reader);
        }

        internal List<VerifierADO> verify(Stream stream)
        {
            PdfReader reader = new PdfReader(stream);
            return this.verify(reader);
        }
    }
}
