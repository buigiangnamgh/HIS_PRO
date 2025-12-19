using Inventec.Common.SignFile.XmlProcess.Common;
using Inventec.Common.SignFile.XmlProcess.Xades.Operations;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;

namespace Inventec.Common.SignFile.XmlProcess.Xades.Dsl
{
    public class XadesVerifyDsl
    {
        private readonly VerificationParameters _parameters = new VerificationParameters();

        public XadesVerifyDsl SignaturePath(string signaturePath)
        {
            _parameters.InputPath = signaturePath;
            return this;
        }

        public XadesVerifyDsl AlsoVerifyCertificate()
        {
            _parameters.VerifyCertificate = true;
            return this;
        }
        public XadesVerifyDsl DoNotVerifyCertificate()
        {
            _parameters.VerifyCertificate = false;
            return this;
        }

        public void Perform()
        {
            XadesVerifyOperation.Verify(_parameters);
        }

        public VerificationResults PerformAndGetResults()
        {
            return XadesVerifyOperation.VerifyAndGetResults(_parameters);
        }
    }
}