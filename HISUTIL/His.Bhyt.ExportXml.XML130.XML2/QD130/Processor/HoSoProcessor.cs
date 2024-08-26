using His.Bhyt.ExportXml.XML130.XML2.Base;
using His.Bhyt.ExportXml.XML130.XML2.QD130.ADO;
using His.Bhyt.ExportXml.XML130.XML2.QD130.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML2.QD130.Processor
{
    class HoSoProcessor : XmlProcessorBase
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

        public XML2Data GetXml2Data()
        {
            XML2Data rs = null;
            try
            {
                if (string.IsNullOrEmpty(XmlString))
                    return rs;
                rs = Deserialize<XML2Data>(RemoveByteOrderMark(XmlString));
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

        private void ConvertValue(ref XML2Data rs)
        {
            try
            {
                if (rs == null)
                    return;

                foreach (var item in rs.DSACH_CHI_TIET_THUOC.CHI_TIET_THUOC)
                {
                    item.DON_GIA = !String.IsNullOrEmpty(item.DON_GIA) ? FormatString(item.DON_GIA) : "";
                    item.THANH_TIEN_BV = !String.IsNullOrEmpty(item.THANH_TIEN_BV) ? FormatString(item.THANH_TIEN_BV) : "";
                    item.THANH_TIEN_BH = !String.IsNullOrEmpty(item.THANH_TIEN_BH) ? FormatString(item.THANH_TIEN_BH) : "";
                    item.T_NGUONKHAC_NSNN = !String.IsNullOrEmpty(item.T_NGUONKHAC_NSNN) ? FormatString(item.T_NGUONKHAC_NSNN) : "0";
                    item.T_NGUONKHAC_VTNN = !String.IsNullOrEmpty(item.T_NGUONKHAC_VTNN) ? FormatString(item.T_NGUONKHAC_VTNN) : "0";
                    item.T_NGUONKHAC_VTTN = !String.IsNullOrEmpty(item.T_NGUONKHAC_VTTN) ? FormatString(item.T_NGUONKHAC_VTTN) : "0";
                    item.T_NGUONKHAC_CL = !String.IsNullOrEmpty(item.T_NGUONKHAC_CL) ? FormatString(item.T_NGUONKHAC_CL) : "";
                    item.T_NGUONKHAC = !String.IsNullOrEmpty(item.T_NGUONKHAC) ? FormatString(item.T_NGUONKHAC) : "";
                    item.T_BHTT = !String.IsNullOrEmpty(item.T_BHTT) ? FormatString(item.T_BHTT) : "";
                    item.T_BNCCT = !String.IsNullOrEmpty(item.T_BNCCT) ? FormatString(item.T_BNCCT) : "";
                    item.T_BNTT = !String.IsNullOrEmpty(item.T_BNTT) ? FormatString(item.T_BNTT) : "";
                    item.NGAY_YL = !string.IsNullOrEmpty(item.NGAY_YL) ? TimeNumberToTimeString(item.NGAY_YL) : "";
                    item.NGAY_TH_YL = !string.IsNullOrEmpty(item.NGAY_TH_YL) ? TimeNumberToTimeString(item.NGAY_TH_YL) : "";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public XML2Data GetXml2Ado()
        {
            XML2Data rs = null;
            try
            {
                var file = this.CreateNoiDungFile();
                if (file == null || !file.Success || file.Data == null)
                    return rs;
                return file.Data[0] as XML2Data;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = null;
            }
            return rs;
        }

        public string GetXml2String()
        {
            string rs = null;
            try
            {
                var file = this.CreateNoiDungFile();
                if (file == null || !file.Success || file.Data == null)
                    return rs;
                XML2Data ChiTieuChiTietThuoc = file.Data[0] as XML2Data;
                if (ChiTieuChiTietThuoc.DSACH_CHI_TIET_THUOC != null && ChiTieuChiTietThuoc.DSACH_CHI_TIET_THUOC.CHI_TIET_THUOC !=null && ChiTieuChiTietThuoc.DSACH_CHI_TIET_THUOC.CHI_TIET_THUOC.Count > 0)
                {
                    rs = this.CreatedXmlString(ChiTieuChiTietThuoc);
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
                List<XML2DetailData> listDetailXml2 = new List<XML2DetailData>();

                //xu ly xml 2
                Xml2Processor xml2Processor = new Xml2Processor();
                var xml2Result = xml2Processor.GenerateXml2ADO(this.Data);
                if (xml2Result == null || !xml2Result.Success || xml2Result.Data == null)
                    return xml2Result;
                var listXmlGiamDinhYKhoaAdo = xml2Result.Data[0] as List<Xml2ADO>;

                xml2Processor.MapADOToXml(listXmlGiamDinhYKhoaAdo, ref listDetailXml2);

                if (listDetailXml2 == null)
                    return rs = new ResultADO(false, "Khong Map duoc tu ADO sang XMLDetail", null);

                //Create danh sach GiamDinhYKhoa XML2
                XML2Data dsChiTieuChiTietThuoc = new XML2Data();
                dsChiTieuChiTietThuoc.DSACH_CHI_TIET_THUOC = new DsChiTietThuoc();
                dsChiTieuChiTietThuoc.DSACH_CHI_TIET_THUOC.CHI_TIET_THUOC = listDetailXml2;

                rs = new ResultADO();
                rs.Success = true;
                rs.Data = new object[] { dsChiTieuChiTietThuoc };
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return rs;
        }
    }
}
