using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.QD917.ADO;
using His.Bhyt.ExportXml.QD917.XML.XML2;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.QD917.Processor
{
    class Xml2Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXml2ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                List<Xml2ADO> ListXml2Ado = new List<Xml2ADO>();
                var listHeinServiceType = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU
                };
                var hisSereServs = data.ListSereServ.Where(o => listHeinServiceType.Contains(o.TDL_HEIN_SERVICE_TYPE_ID.Value)).ToList();//lấy các dịch vụ là thuốc, vật tư và không phải hao phí
                foreach (var hisSereServ in hisSereServs)
                {
                    string maThuoc = "";
                    string maNhomThuoc = "";
                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                    {
                        maThuoc = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        maNhomThuoc = hisSereServ.TDL_HST_BHYT_CODE ?? "";
                    }
                    else
                    {
                        maThuoc = hisSereServ.ACTIVE_INGR_BHYT_CODE ?? "";
                        maNhomThuoc = hisSereServ.TDL_HST_BHYT_CODE ?? "";
                    }

                    var xml2 = new Xml2ADO();
                    xml2.MaLienKet = data.HeinApproval.HEIN_APPROVAL_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                    xml2.Stt = 1;
                    xml2.MaThuoc = maThuoc;
                    xml2.MaNhom = maNhomThuoc;
                    xml2.TenThuoc = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                    xml2.DonViTinh = hisSereServ.SERVICE_UNIT_NAME ?? "";
                    xml2.HamLuong = hisSereServ.CONCENTRA ?? "";
                    xml2.DuongDung = hisSereServ.MEDICINE_USE_FORM_CODE ?? "";
                    xml2.LieuDung = hisSereServ.TUTORIAL ?? "";
                    xml2.SoDangKy = hisSereServ.MEDICINE_REGISTER_NUMBER ?? "";
                    xml2.SoLuong = Math.Round(hisSereServ.AMOUNT, 2);
                    //if (hisSereServ.HEIN_LIMIT_PRICE.HasValue)
                    //{
                    //    xml2.DonGia = Math.Round(hisSereServ.ORIGINAL_PRICE, 2);
                    //}
                    //else
                    //{
                    xml2.DonGia = Math.Round((hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO)), 2);
                    //}
                    xml2.TyLeTT = Math.Round(hisSereServ.ORIGINAL_PRICE > 0 ? (hisSereServ.HEIN_LIMIT_PRICE.HasValue ? (hisSereServ.HEIN_LIMIT_PRICE.Value / (hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO))) * 100 : (hisSereServ.PRICE / hisSereServ.ORIGINAL_PRICE) * 100) : 0, 0);
                    xml2.ThanhTien = Math.Round(xml2.SoLuong * xml2.DonGia * (xml2.TyLeTT / 100), 2, MidpointRounding.AwayFromZero);

                    decimal thanhTien = (hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0) + (hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                    decimal donGias = thanhTien / xml2.SoLuong / (xml2.TyLeTT / 100);
                    xml2.DonGia = Math.Round(donGias, 2, MidpointRounding.AwayFromZero);
                    xml2.ThanhTien = Math.Round(xml2.SoLuong * xml2.DonGia * (xml2.TyLeTT / 100), 2, MidpointRounding.AwayFromZero);

                    xml2.MaKhoa = hisSereServ.REQUEST_BHYT_CODE ?? "";
                    if (GlobalConfigStore.ListEmployees != null && GlobalConfigStore.ListEmployees.Count > 0)
                    {
                        var dataEmployee = GlobalConfigStore.ListEmployees.FirstOrDefault(p => p.LOGINNAME == hisSereServ.REQUEST_LOGINNAME);
                        if (dataEmployee != null)
                        {
                            xml2.MaBacSi = dataEmployee.DIPLOMA ?? "";
                        }
                        else
                        {
                            xml2.MaBacSi = "";
                        }
                    }
                    else
                    {
                        xml2.MaBacSi = "";//TO DO - phầm mềm chưa quản lý
                    }
                    xml2.MaBenh = hisSereServ.ICD_CODE ?? "";
                    xml2.NgayYLenh = hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);
                    xml2.MaPTTT = 1;//TO DO - phần mềm chưa có nghiệp vụ
                    ListXml2Ado.Add(xml2);
                }
                rs = new ResultADO(true, "", new object[] { ListXml2Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        internal void MapADOToXml(List<Xml2ADO> listAdo, ref List<XML2DetailData> datas)
        {
            try
            {
                if (datas == null)
                    datas = new List<XML2DetailData>();
                if (listAdo != null && listAdo.Count >= 0)
                {
                    foreach (var ado in listAdo)
                    {
                        XML2DetailData detail = new XML2DetailData();
                        detail.DON_GIA = ado.DonGia.ToString("G27", CultureInfo.InvariantCulture);
                        detail.DON_VI_TINH = ado.DonViTinh;
                        detail.DUONG_DUNG = ado.DuongDung;
                        detail.HAM_LUONG = this.ConvertStringToXmlDocument(ado.HamLuong);
                        detail.LIEU_DUNG = this.ConvertStringToXmlDocument(ado.LieuDung);
                        detail.MA_BAC_SI = ado.MaBacSi;
                        detail.MA_BENH = ado.MaBenh;
                        detail.MA_KHOA = ado.MaKhoa;
                        detail.MA_LK = ado.MaLienKet;
                        detail.MA_NHOM = ado.MaNhom;
                        detail.MA_PTTT = ado.MaPTTT;
                        detail.MA_THUOC = ado.MaThuoc;
                        detail.NGAY_YL = ado.NgayYLenh;
                        detail.SO_DANG_KY = ado.SoDangKy;
                        detail.SO_LUONG = ado.SoLuong.ToString("G27", CultureInfo.InvariantCulture);
                        detail.STT = ado.Stt;
                        detail.TEN_THUOC = this.ConvertStringToXmlDocument(ado.TenThuoc);
                        detail.THANH_TIEN = ado.ThanhTien.ToString("G27", CultureInfo.InvariantCulture);
                        detail.TYLE_TT = ado.TyLeTT.ToString("G27", CultureInfo.InvariantCulture);
                        datas.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                datas = null;
            }
        }
    }
}
