using Inventec.Common.SignFile.XmlProcess.XmlDsig.Dsl;
using iTextSharp.text.pdf.codec;
using System.Xml;

namespace Inventec.Common.SignFile.XmlProcess.XmlDsig
{
    public abstract class XmlDsigHelper
    {
        public static SignDSL Sign(string inputPath)
        {
            var signDsl = new SignDSL();
            var VerificationResults = XmlDsigHelper.Verify(inputPath).PerformAndGetResults();
            if (VerificationResults != null && VerificationResults.SigningCertificate != null && VerificationResults.OriginalDocument != null)
            {
                VerificationResults.OriginalDocument.Save(inputPath);
                signDsl.InputPath(inputPath);
            }
            else
            {
                signDsl.InputPath(inputPath);
            }          
        
            return signDsl;
        }
        public static SignDSL Sign(XmlDocument xmlDocument)
        {
            var signDsl = new SignDSL();
            signDsl.InputXml(xmlDocument);
            return signDsl;
        }

        public static VerificationDsl Verify(string signaturePath)
        {
            var validationDsl = new VerificationDsl();
            validationDsl.SignaturePath(signaturePath);
            return validationDsl;
        }

        public static BatchSignDSL BatchSign(params string[] inputPaths)
        {
            var batchSignDsl = new BatchSignDSL();
            batchSignDsl.InputPaths(inputPaths);
            return batchSignDsl;
        }
    }
}