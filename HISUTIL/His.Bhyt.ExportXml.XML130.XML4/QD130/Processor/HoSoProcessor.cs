using His.Bhyt.ExportXml.XML130.XML4.Base;
using His.Bhyt.ExportXml.XML130.XML4.QD130.ADO;
using His.Bhyt.ExportXml.XML130.XML4.QD130.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
//using static System.Net.Mime.MediaTypeNames;

namespace His.Bhyt.ExportXml.XML130.XML4.QD130.Processor
{
    internal class HoSoProcessor : XmlProcessorBase
    {
        InputADO Data { get; set; }
        string XmlString { get; set; }
        internal HoSoProcessor(InputADO data)
        {
            this.Data = data;
        }
        internal HoSoProcessor(string XmlString)
        {
            this.XmlString = XmlString;
        }
        public List<XML4DetailData> GetXml4DetailData()
        {
            List<XML4DetailData> rs = null;
            try
            {
                if (string.IsNullOrEmpty(XmlString))
                    return rs;
                var Data = Deserialize<XML4Data>(RemoveByteOrderMark(XmlString));
                if (Data != null && Data.DSACH_CHI_TIET_CLS != null)
                {
                    rs = Data.DSACH_CHI_TIET_CLS.CHI_TIET_CLS;
                    ConvertValue(ref rs);
                }
            }
            catch (Exception ex)
            {
                rs = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return rs;
        }
        public string RemoveByteOrderMark(string XML)
        {
            string byteOrderMark = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (XML.StartsWith(byteOrderMark))
            {
                XML = XML.Remove(0, byteOrderMark.Length);
            }
            return XML;
        }

        private void ConvertValue(ref List<XML4DetailData> rs)
        {
            try
            {
                if (rs == null || rs.Count == 0)
                    return;
                rs.ForEach(o =>
                {
                    o.NGAY_KQ = !string.IsNullOrEmpty(o.NGAY_KQ) ? Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Int64.Parse(o.NGAY_KQ + "00")) : "";
                });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public XML4Data GetXml4Ado()
        {
            XML4Data rs = null;
            try
            {
                var file = this.CreateNoiDungFile();
                if (file == null || !file.Success || file.Data == null)
                    return rs;
                return file.Data[0] as XML4Data;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = null;
            }
            return rs;
        }

        public string GetXml4String()
        {
            string rs = null;
            try
            {
                var file = this.CreateNoiDungFile();
                if (file == null || !file.Success || file.Data == null)
                    return rs;
                XML4Data dsChiTietCLS = file.Data[0] as XML4Data;
                if (dsChiTietCLS.DSACH_CHI_TIET_CLS != null && dsChiTietCLS.DSACH_CHI_TIET_CLS.CHI_TIET_CLS != null && dsChiTietCLS.DSACH_CHI_TIET_CLS.CHI_TIET_CLS.Count > 0)
                {
                    rs = this.CreatedXmlString(dsChiTietCLS);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = null;
            }
            return rs;
        }

        private ResultADO CreateNoiDungFile()
        {
            ResultADO rs = null;
            try
            {
                List<XML4DetailData> listDetailXml4 = new List<XML4DetailData>();
                //xu ly xml 4
                Xml4Processor xml4Processor = new Xml4Processor();
                var xml4Result = xml4Processor.GenerateXml4ADO(this.Data);
                if (xml4Result == null || !xml4Result.Success || xml4Result.Data == null)
                    return xml4Result;
                var listXmlClsAdo = xml4Result.Data[0] as List<Xml4ADO>;
                xml4Processor.MapADOToXml(listXmlClsAdo, ref listDetailXml4);
                if (listDetailXml4 == null)
                    return rs = new ResultADO(false, "Khong Map duoc tu ADO sang XMLDetail", null);

                //Create danh sach ChiTietCLS XML4
                XML4Data dsChiTietCLS = new XML4Data();
                dsChiTietCLS.DSACH_CHI_TIET_CLS = new DsChiTietDichVuCLS();
                dsChiTietCLS.DSACH_CHI_TIET_CLS.CHI_TIET_CLS = listDetailXml4;


                rs = new ResultADO();
                rs.Success = true;
                rs.Data = new object[] { dsChiTietCLS };
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return rs;
        }
    }
}
