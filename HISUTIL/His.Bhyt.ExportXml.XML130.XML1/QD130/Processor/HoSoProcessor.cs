using His.Bhyt.ExportXml.XML130.XML1.Base;
using His.Bhyt.ExportXml.XML130.XML1.Processor;
using His.Bhyt.ExportXml.XML130.XML1.QD130.ADO;
using His.Bhyt.ExportXml.XML130.XML1.QD130.XML;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace His.Bhyt.ExportXml.XML130.XML1.QD130.Processor
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
        public XML1Data GetXml1Data()
        {
            XML1Data rs = null;
            try
            {
                if (string.IsNullOrEmpty(XmlString))
                    return rs;
                rs = Deserialize<XML1Data>(RemoveByteOrderMark(XmlString));
                ConvertValue(ref rs);
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
        private void ConvertValue(ref XML1Data rs)
        {
            try
            {
                if (rs == null)
                    return;
                rs.NGAY_SINH = rs.NGAY_SINH.Contains("00000000") ? rs.NGAY_SINH.Substring(0, 4) : Inventec.Common.DateTime.Convert.TimeNumberToDateString(rs.NGAY_SINH);

                List<string> lstGTTheTu = new List<string>();
                List<string> lstGTTheDen = new List<string>();

                lstGTTheTu.AddRange(rs.GT_THE_TU.Split(';'));
                var lstGTTheTuConvert = lstGTTheTu.Select(item => Inventec.Common.DateTime.Convert.TimeNumberToDateString(item).ToString()).ToList();
                rs.GT_THE_TU = string.Join(";", lstGTTheTuConvert);

                lstGTTheDen.AddRange(rs.GT_THE_DEN.Split(';'));
                var lstGTTheDenConvert = lstGTTheDen.Select(item => Inventec.Common.DateTime.Convert.TimeNumberToDateString(item).ToString()).ToList();
                rs.GT_THE_DEN = string.Join(";", lstGTTheDenConvert);

                rs.NGAY_MIEN_CCT = !string.IsNullOrEmpty(rs.NGAY_MIEN_CCT) ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(rs.NGAY_MIEN_CCT) : "";
                rs.NAM_NAM_LIEN_TUC = !string.IsNullOrEmpty(rs.NAM_NAM_LIEN_TUC) ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(rs.NAM_NAM_LIEN_TUC) : "";
                rs.NGAY_VAO = !string.IsNullOrEmpty(rs.NGAY_VAO) ? Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Int64.Parse(rs.NGAY_VAO + "00")) : "";
                rs.NGAY_VAO_NOI_TRU = !string.IsNullOrEmpty(rs.NGAY_VAO_NOI_TRU) ? Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Int64.Parse(rs.NGAY_VAO_NOI_TRU + "00")) : "";
                rs.NGAY_RA = !string.IsNullOrEmpty(rs.NGAY_RA) ? Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Int64.Parse(rs.NGAY_RA + "00")) : "";
                rs.NGAY_TTOAN = !string.IsNullOrEmpty(rs.NGAY_TTOAN) ? Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Int64.Parse(rs.NGAY_TTOAN + "00")) : "";
                rs.NGAY_TAI_KHAM = !string.IsNullOrEmpty(rs.NGAY_TAI_KHAM) ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(rs.NGAY_TAI_KHAM) : "";
                //rs.T_THUOC = !string.IsNullOrEmpty(rs.T_THUOC) ? Inventec.Common.Number.Convert.NumberToString(ConvertDecimal(rs.T_THUOC), 0) : "";
                //rs.T_VTYT = !string.IsNullOrEmpty(rs.T_VTYT) ? Inventec.Common.Number.Convert.NumberToString(ConvertDecimal(rs.T_VTYT), 0) : "";
                //rs.T_TONGCHI_BV = !string.IsNullOrEmpty(rs.T_TONGCHI_BV) ? Inventec.Common.Number.Convert.NumberToString(ConvertDecimal(rs.T_TONGCHI_BV), 0) : "";
                //rs.T_TONGCHI_BH = !string.IsNullOrEmpty(rs.T_TONGCHI_BH) ? Inventec.Common.Number.Convert.NumberToString(ConvertDecimal(rs.T_TONGCHI_BH), 0) : "";
                //rs.T_BNTT = !string.IsNullOrEmpty(rs.T_BNTT) ? Inventec.Common.Number.Convert.NumberToString(ConvertDecimal(rs.T_BNTT), 0) : "";
                //rs.T_BHTT = !string.IsNullOrEmpty(rs.T_BHTT) ? Inventec.Common.Number.Convert.NumberToString(ConvertDecimal(rs.T_BHTT), 0) : "";
                //rs.T_BNCCT = !string.IsNullOrEmpty(rs.T_BNCCT) ? Inventec.Common.Number.Convert.NumberToString(ConvertDecimal(rs.T_BNCCT), 0) : "";
                //rs.T_NGUONKHAC = !string.IsNullOrEmpty(rs.T_NGUONKHAC) ? Inventec.Common.Number.Convert.NumberToString(ConvertDecimal(rs.T_NGUONKHAC), 0) : "";
                //rs.T_BHTT_GDV = !string.IsNullOrEmpty(rs.T_BHTT_GDV) ? Inventec.Common.Number.Convert.NumberToString(ConvertDecimal(rs.T_BHTT_GDV), 0) : "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private decimal ConvertDecimal(string txt)
        {
            decimal rs = 0;
            try
            {
                CultureInfo culture = new CultureInfo("en-US");
                if (txt.Contains(","))
                    culture = new CultureInfo("fr-FR");
                rs = Convert.ToDecimal(txt, culture);
            }
            catch (Exception ex)
            {
                rs = 0;
                Inventec.Common.Logging.LogSystem.Error(txt + "________" + ex);
            }
            return rs;
        }

        public XML1Data GetXml1Ado()
        {
            XML1Data rs = null;
            try
            {
                var file = this.CreateNoiDungFile();
                if (file == null || !file.Success || file.Data == null)
                    return rs;
                return file.Data[0] as XML1Data;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = null;
            }
            return rs;
        }
        public string GetXml1String()
        {
            string rs = null;
            try
            {
                var file = this.CreateNoiDungFile();
                if (file == null || !file.Success || file.Data == null)
                    return rs;
                XML1Data dsGiamDinhYKhoa = file.Data[0] as XML1Data;
                if (dsGiamDinhYKhoa != null)
                {
                    rs = this.CreatedXmlString(dsGiamDinhYKhoa);
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
                object Xml1Data = null;
                //Xu ly Xml 1
                Xml1Processor xml1Procssor = new Xml1Processor();
                var xml1Result = xml1Procssor.GenerateXml1Data(this.Data);
                if (xml1Result == null || !xml1Result.Success || xml1Result.Data == null || xml1Result.Data.Length == 0)
                    return xml1Result;
                Xml1Data = xml1Result.Data[0];

                if (Xml1Data == null)
                    return rs = new ResultADO(false, "Khong Map duoc tu ADO sang XMLDetail", null);

                rs = new ResultADO();
                rs.Success = true;
                rs.Data = new object[] { Xml1Data};
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return rs;
        }
    }
}
