using His.Bhyt.ExportXml.XML130.XML4.Base;
using His.Bhyt.ExportXml.XML130.XML4.QD130.ADO;
using His.Bhyt.ExportXml.XML130.XML4.QD130.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML4
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

        public List<XML4DetailData> RunXml4DetailData(string xmlString)
        {
            List<XML4DetailData> result = null;
            try
            {
                _HoSoProcessor130 = new QD130.Processor.HoSoProcessor(xmlString);
                result = _HoSoProcessor130.GetXml4DetailData();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }      

        public string RunXml4String()
        {
            string result = null;
            try
            {
                _HoSoProcessor130 = new QD130.Processor.HoSoProcessor(this.inputADO);
                result = _HoSoProcessor130.GetXml4String();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public XML4Data RunXml4Ado()
        {
            XML4Data result = null;
            try
            {
                _HoSoProcessor130 = new QD130.Processor.HoSoProcessor(this.inputADO);
                result = _HoSoProcessor130.GetXml4Ado();
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
