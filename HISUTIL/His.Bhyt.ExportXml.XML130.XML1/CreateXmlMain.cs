using His.Bhyt.ExportXml.XML130.XML1.Base;
using His.Bhyt.ExportXml.XML130.XML1.QD130.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML1
{
    public class CreateXmlMain
    {
        private InputADO inputADO { get; set; }
        QD130.Processor.HoSoProcessor _HoSoProcessor130;
        private string MessageError { get; set; }
        private string CodeError;
        public CreateXmlMain() { }
        public CreateXmlMain(InputADO inputADO)
        {
            try
            {
                this.inputADO = inputADO;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        public XML1Data RunXml1Data(string xmlString)
        {
            XML1Data result = null;
            try
            {
                _HoSoProcessor130 = new QD130.Processor.HoSoProcessor(xmlString);
                result = _HoSoProcessor130.GetXml1Data();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        public string RunXml1String()
        {
            string result = null;
            try
            {
                _HoSoProcessor130 = new QD130.Processor.HoSoProcessor(this.inputADO);
                result = _HoSoProcessor130.GetXml1String();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }          
            return result;
        }

        public XML1Data RunXml1Ado()
        {
            XML1Data result = null;
            try
            {
                _HoSoProcessor130 = new QD130.Processor.HoSoProcessor(this.inputADO);
                result = _HoSoProcessor130.GetXml1Ado();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
