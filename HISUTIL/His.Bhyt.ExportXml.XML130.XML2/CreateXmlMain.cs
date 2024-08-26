using His.Bhyt.ExportXml.XML130.XML2.Base;
using His.Bhyt.ExportXml.XML130.XML2.QD130.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML2
{
    public class CreateXmlMain
    {
        private InputADO inputADO { get; set; }
        QD130.Processor.HoSoProcessor _HoSoProcessor130;
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

        public string RunXml2String()
        {
            string result = null;
            try
            {
                _HoSoProcessor130 = new QD130.Processor.HoSoProcessor(this.inputADO);
                result = _HoSoProcessor130.GetXml2String();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public XML2Data RunXml2Ado()
        {
            XML2Data result = null;
            try
            {
                _HoSoProcessor130 = new QD130.Processor.HoSoProcessor(this.inputADO);
                result = _HoSoProcessor130.GetXml2Ado();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public XML2Data RunXml2Data(string xmlString)
        {
            XML2Data result = null;
            try
            {
                _HoSoProcessor130 = new QD130.Processor.HoSoProcessor(xmlString);
                result = _HoSoProcessor130.GetXml2Data();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
