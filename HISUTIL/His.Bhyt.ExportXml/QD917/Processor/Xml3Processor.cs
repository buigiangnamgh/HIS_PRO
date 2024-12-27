using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.QD917.ADO;
using His.Bhyt.ExportXml.QD917.XML.XML3;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.QD917.Processor
{
    class Xml3Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXml2ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                List<Xml3ADO> listXml3Ado = new List<Xml3ADO>();
                var listHeinServiceType = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC
                };

                var listHeinServiceTypeMaterial = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                };

                var listServeservs = data.ListSereServ.Where(s => listHeinServiceType.Contains(s.TDL_HEIN_SERVICE_TYPE_ID.Value) || listHeinServiceTypeMaterial.Contains(s.TDL_HEIN_SERVICE_TYPE_ID.Value)).ToList();//lấy các dịch vụ không phải là thuốc, vật tư và có tỷ lệ chi trả tiền, các dịch vụ được hưởng BHYT
                foreach (var hisSereServ in listServeservs)
                {
                    string maVatTu = "";
                    string maNhom = "";
                    string maDichVu = "";

                    var xml3 = new Xml3ADO();
                    if (listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                    {
                        xml3.IsMaterial = true;
                        maVatTu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        maNhom = hisSereServ.TDL_HST_BHYT_CODE ?? "";
                        if (hisSereServ.PARENT_ID.HasValue)
                        {
                            var parent = data.ListSereServ.FirstOrDefault(o => o.ID == hisSereServ.PARENT_ID.Value);
                            if (parent != null && parent.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                            {
                                maDichVu = parent.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                            }
                        }
                    }
                    else
                    {
                        maDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        maNhom = hisSereServ.TDL_HST_BHYT_CODE ?? "";
                    }


                    xml3.MaLienKet = data.HeinApproval.HEIN_APPROVAL_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                    xml3.Stt = 1;
                    xml3.MaDichVu = maDichVu;
                    xml3.MaVatTu = maVatTu;
                    xml3.MaNhom = maNhom;
                    xml3.TenDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                    xml3.DonViTinh = hisSereServ.SERVICE_UNIT_NAME ?? "";
                    xml3.SoLuong = Math.Round(hisSereServ.AMOUNT, 2);

                    if (listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                    {
                        decimal donGia = hisSereServ.HEIN_LIMIT_PRICE.HasValue ? hisSereServ.ORIGINAL_PRICE : hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO);
                        xml3.DonGia = Math.Round(donGia, 2, MidpointRounding.AwayFromZero);

                        decimal tyle = hisSereServ.HEIN_LIMIT_PRICE.HasValue ? (hisSereServ.HEIN_LIMIT_PRICE.Value / donGia) * 100 : (hisSereServ.PRICE / donGia) * 100;
                        xml3.TyLeTT = Math.Round(tyle, 0);
                    }
                    else
                    {
                        xml3.DonGia = Math.Round((hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO)), 2, MidpointRounding.AwayFromZero);
                        decimal tyle = 0;
                        if (hisSereServ.ORIGINAL_PRICE > 0)
                        {
                            tyle = hisSereServ.HEIN_LIMIT_PRICE.HasValue ? (hisSereServ.HEIN_LIMIT_PRICE.Value / (hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO))) * 100 : (hisSereServ.PRICE / hisSereServ.ORIGINAL_PRICE) * 100;
                        }
                        xml3.TyLeTT = Math.Round(tyle, 0);
                    }

                    xml3.ThanhTien = Math.Round(xml3.SoLuong * xml3.DonGia * (xml3.TyLeTT / 100), 2, MidpointRounding.AwayFromZero);

                    decimal thanhTien = (hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0) + (hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                    decimal donGias = thanhTien / xml3.SoLuong / (xml3.TyLeTT / 100);
                    xml3.DonGia = Math.Round(donGias, 2, MidpointRounding.AwayFromZero);
                    xml3.ThanhTien = Math.Round(xml3.SoLuong * xml3.DonGia * (xml3.TyLeTT / 100), 2, MidpointRounding.AwayFromZero);

                    xml3.MaKhoa = hisSereServ.REQUEST_BHYT_CODE ?? "";
                    if (GlobalConfigStore.ListEmployees != null && GlobalConfigStore.ListEmployees.Count > 0)
                    {
                        if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            var dataEmployee = GlobalConfigStore.ListEmployees.FirstOrDefault(p => p.LOGINNAME == hisSereServ.EXECUTE_LOGINNAME);
                            if (dataEmployee != null)
                            {
                                xml3.MaBacSi = dataEmployee.DIPLOMA ?? "";
                            }
                            else
                            {
                                xml3.MaBacSi = "";
                            }
                        }
                        else
                        {
                            var dataEmployee = GlobalConfigStore.ListEmployees.FirstOrDefault(p => p.LOGINNAME == hisSereServ.REQUEST_LOGINNAME);
                            if (dataEmployee != null)
                            {
                                xml3.MaBacSi = dataEmployee.DIPLOMA ?? "";
                            }
                            else
                            {
                                xml3.MaBacSi = "";
                            }
                        }
                    }
                    else
                    {
                        xml3.MaBacSi = "";//TO DO - chưa có nghiệp vụ quản lý nhân viện bệnh viện
                    }
                    xml3.MaBenh = hisSereServ.ICD_CODE ?? ""; ;
                    xml3.NgayYLenh = hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);
                    xml3.NgayKetQua = hisSereServ.FINISH_TIME.HasValue ? hisSereServ.FINISH_TIME.ToString().Substring(0, 12) : "";
                    xml3.MaPTTT = 1;//TO 
                    listXml3Ado.Add(xml3);
                }
                rs = new ResultADO(true, "", new object[] { listXml3Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        internal void MapADOToXml(List<Xml3ADO> listAdo, ref List<XML3DetailData> datas)
        {
            try
            {
                if (datas == null)
                    datas = new List<XML3DetailData>();
                if (listAdo != null || listAdo.Count > 0)
                {
                    foreach (var ado in listAdo)
                    {
                        XML3DetailData detail = new XML3DetailData();
                        detail.DON_GIA = ado.DonGia.ToString("G27", CultureInfo.InvariantCulture);
                        detail.DON_VI_TINH = ado.DonViTinh;
                        detail.MA_BAC_SI = ado.MaBacSi;
                        detail.MA_BENH = ado.MaBenh;
                        detail.MA_DICH_VU = ado.MaDichVu;
                        detail.MA_KHOA = ado.MaKhoa;
                        detail.MA_LK = ado.MaLienKet;
                        detail.MA_NHOM = ado.MaNhom;
                        detail.MA_PTTT = ado.MaPTTT;
                        detail.MA_VAT_TU = ado.MaVatTu;
                        detail.NGAY_YL = ado.NgayYLenh;
                        detail.NGAY_KQ = ado.NgayKetQua;
                        detail.SO_LUONG = ado.SoLuong.ToString("G27", CultureInfo.InvariantCulture);
                        detail.STT = ado.Stt;
                        detail.TEN_DICH_VU = this.ConvertStringToXmlDocument(ado.TenDichVu);
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
