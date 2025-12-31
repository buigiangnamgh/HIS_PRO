using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Inventec.Common.Logging;
using Inventec.Common.SignFile.XmlProcess.Common;
using Inventec.Common.SignFile.XmlProcess.Common.Exceptions;
using Inventec.Common.SignFile.XmlProcess.Utils.Xml;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Helpers;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig.Operations
{
	internal abstract class XmlDsigVerifyOperation
	{
		internal static bool Verify(VerificationParameters parameters)
		{
			VerifyAndGetResults(parameters);
			return true;
		}

		internal static VerificationResults VerifyAndGetResults(VerificationParameters parameters)
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument
				{
					PreserveWhitespace = true
				};
				xmlDocument.LoadXml(File.ReadAllText(parameters.InputPath));
				string outerXml = xmlDocument.OuterXml;
				return PerformValidationFromXml(outerXml, parameters);
			}
			catch (Exception)
			{
			}
			return null;
		}

		private static VerificationResults PerformValidationFromXml(string xml, VerificationParameters validationParameters)
		{
			XmlDocument xmlDocument = new XmlDocument
			{
				PreserveWhitespace = false
			};
			xmlDocument.LoadXml(xml);
			ExtendedSignedXml extendedSignedXml = new ExtendedSignedXml(xmlDocument);
			if (xmlDocument.DocumentElement == null)
			{
				throw new InvalidDocumentException("Document has no root element");
			}
			XmlElement signatureNode = XmlDsigNodesHelper.GetSignatureNode(xmlDocument);
			extendedSignedXml.LoadXml(signatureNode);
			X509Certificate2 verificationCertificate = GetVerificationCertificate(extendedSignedXml, validationParameters);
			if (verificationCertificate == null)
			{
				throw new Exception("Signer public key could not be found");
			}
			if (!extendedSignedXml.CheckSignature(verificationCertificate, !validationParameters.VerifyCertificate))
			{
				LogSystem.Warn("Gọi hàm SignedXml.CheckSignature: Signature is invalid. Nguyên nhân do node cha có chứa Attribute đặc biệt");
			}
			return new VerificationResults
			{
				Timestamp = GetTimeStampFromSignature(xmlDocument),
				OriginalDocument = GetDocumentFromSignature(xmlDocument),
				SigningCertificate = GetCertificateFromSignature(xmlDocument)
			};
		}

		private static X509Certificate2 GetVerificationCertificate(SignedXml signedXml, VerificationParameters verificationParameters)
		{
			X509Certificate2 x509Certificate = verificationParameters.VerificationCertificate;
			if (x509Certificate == null && signedXml.KeyInfo != null)
			{
				IEnumerator enumerator = signedXml.KeyInfo.GetEnumerator();
				while (enumerator.MoveNext())
				{
					try
					{
						KeyInfoX509Data keyInfoX509Data = (KeyInfoX509Data)enumerator.Current;
						if (keyInfoX509Data != null)
						{
							if (keyInfoX509Data.Certificates.Count > 0)
							{
								x509Certificate = (X509Certificate2)keyInfoX509Data.Certificates[0];
							}
							if (x509Certificate != null)
							{
								break;
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return x509Certificate;
		}

		private static X509Certificate2 GetCertificateFromSignature(XmlDocument signedXml)
		{
			XmlElement xmlElement = signedXml.DocumentElement;
			if (!XmlDsigNodesHelper.IsSignatureNode(xmlElement))
			{
				xmlElement = XmlHelper.DescendantWith(xmlElement, XmlDsigNodesHelper.IsSignatureNode);
			}
			XmlElement rootNode = XmlHelper.DescendantWith(xmlElement, XmlDsigNodesHelper.IsKeyInfo);
			XmlElement rootNode2 = XmlHelper.DescendantWith(rootNode, XmlDsigNodesHelper.IsX509Data);
			XmlElement xmlElement2 = XmlHelper.DescendantWith(rootNode2, XmlDsigNodesHelper.IsX509Certificate);
			return new X509Certificate2(Convert.FromBase64String(xmlElement2.InnerText));
		}

		private static XmlDocument GetDocumentFromSignature(XmlDocument signedXml)
		{
			XmlElement documentElement = signedXml.DocumentElement;
			if (XmlDsigNodesHelper.IsSignatureNode(documentElement))
			{
				return GetDocumentFromDetachedOrEnvelopingSignature(signedXml);
			}
			return GetDocumentFromEnvelopedSignature(signedXml);
		}

		private static XmlDocument GetDocumentFromDetachedOrEnvelopingSignature(XmlDocument signedXml)
		{
			XmlElement rootNode = XmlHelper.DescendantWith(signedXml.DocumentElement, XmlDsigNodesHelper.IsSignedInfoNode);
			XmlElement xmlElement = XmlHelper.DescendantWith(rootNode, XmlDsigNodesHelper.IsReferenceToOriginalContent);
			if (xmlElement == null)
			{
				throw new Exception("Reference to Original Content not found");
			}
			string uri = XmlHelper.AttributeOf(xmlElement, "URI");
			if (uri.StartsWith("#"))
			{
				uri = uri.Substring(1);
			}
			if (XmlDsigNodesHelper.ReferenceIsDetached(xmlElement))
			{
				return XmlHelper.ReadXmlFromUri(uri);
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement2 = XmlHelper.DescendantWith(signedXml.DocumentElement, (XmlElement n) => XmlDsigNodesHelper.ObjectNodeWithIdAsUri(n, uri));
			xmlDocument.LoadXml(xmlElement2.InnerXml);
			return xmlDocument;
		}

		private static XmlDocument GetDocumentFromEnvelopedSignature(XmlDocument signedXml)
		{
			if (signedXml.DocumentElement == null)
			{
				throw new InvalidParameterException("Root node cannot be found in signed XML");
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(signedXml.OuterXml);
			XmlElement documentElement = xmlDocument.DocumentElement;
			if (documentElement == null)
			{
				throw new InvalidParameterException("Root node cannot be found in signed XML");
			}
			XmlElement oldChild = XmlHelper.DescendantWith(documentElement, XmlDsigNodesHelper.IsSignatureNode);
			documentElement.RemoveChild(oldChild);
			return xmlDocument;
		}

		private static string GetTimeStampFromSignature(XmlDocument document)
		{
			XmlElement documentElement = document.DocumentElement;
			XmlElement xmlElement = (XmlDsigNodesHelper.IsSignatureNode(documentElement) ? documentElement : XmlHelper.DescendantWith(documentElement, XmlDsigNodesHelper.IsSignatureNode));
			if (xmlElement == null)
			{
				throw new Exception("Signature node not found");
			}
			List<XmlElement> list = XmlHelper.FindNodesIn(xmlElement, "Object/SignatureProperties/SignatureProperty/SigningTime");
			return (list.Count == 0) ? "" : list[0].InnerText;
		}
	}
}
