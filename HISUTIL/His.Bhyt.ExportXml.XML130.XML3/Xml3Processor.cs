using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace His.Bhyt.ExportXml.XML130.XML3
{
    public class Xml3Processor
    {
        InputXml3ADO inputXmlAdo;

        public Xml3Processor(InputXml3ADO inputXmlAdo)
        {
            this.inputXmlAdo = inputXmlAdo;
        }

        public XML3Data GenerateXml3Data()
        {
            XML3Data result = null;
            try
            {
                List<XML3DetailData> listDetailXml3 = new List<XML3DetailData>();
                List<XML3ADO> listADOXml3 = GenerateXml3ADODatas();
                MapADOToXml(listADOXml3, ref listDetailXml3);
                result = new XML3Data();
                result.DSACH_CHI_TIET_DVKT = new DsChiTietDVKT();
                result.DSACH_CHI_TIET_DVKT.CHI_TIET_DVKT = listDetailXml3;
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        public List<XML3ADO> GenerateXml3ADODatas()
        {
            List<XML3ADO> result = null;
            try
            {
                result = ProcessDetailsData(this.inputXmlAdo);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        public void MapADOToXml(List<XML3ADO> listAdo, ref List<XML3DetailData> datas)
        {
            try
            {
                if (datas == null)
                    datas = new List<XML3DetailData>();
                if (listAdo != null && listAdo.Count > 0)
                {
                    foreach (var ado in listAdo)
                    {
                        XML3DetailData detail = new XML3DetailData();
                        detail.DON_VI_TINH = ado.donViTinh;
                        detail.GOI_VTYT = ado.goiVTYT;
                        detail.MA_BAC_SI = ado.maBacSi;
                        detail.MA_BENH = ado.maBenh;
                        detail.MA_DICH_VU = ado.maDichVu;
                        detail.MA_GIUONG = ado.maGiuong;
                        detail.MA_KHOA = ado.maKhoa;
                        detail.MA_LK = ado.maLienKet;
                        detail.MA_NHOM = ado.maNhom;
                        detail.MA_PTTT = ado.maPTTT;
                        detail.MA_VAT_TU = ado.maVatTu;
                        detail.MUC_HUONG = ado.mucHuong;
                        detail.NGAY_YL = ado.ngayYL;
                        detail.NGAY_KQ = ado.ngayKQ;
                        detail.PHAM_VI = ado.phamVi;
                        detail.SO_LUONG = ado.soLuong.ToString("G27", CultureInfo.InvariantCulture);
                        detail.STT = ado.stt;
                        detail.T_BHTT = ado.tBhtt.ToString("G27", CultureInfo.InvariantCulture);
                        detail.T_BNCCT = ado.tBncct.ToString("G27", CultureInfo.InvariantCulture);
                        detail.T_BNTT = ado.tBntt.ToString("G27", CultureInfo.InvariantCulture);
                        detail.T_NGUONKHAC = ado.tNguonKhac.ToString("G27", CultureInfo.InvariantCulture);
                        detail.T_NGUONKHAC_NSNN = ado.tNguonKhacNsnn.HasValue ? ado.tNguonKhacNsnn.Value.ToString("G27", CultureInfo.InvariantCulture) : "0";
                        detail.T_NGUONKHAC_VTNN = ado.tNguonKhacVtnn.HasValue ? ado.tNguonKhacVtnn.Value.ToString("G27", CultureInfo.InvariantCulture) : "0";
                        detail.T_NGUONKHAC_VTTN = ado.tNguonKhacVttn.HasValue ? ado.tNguonKhacVttn.Value.ToString("G27", CultureInfo.InvariantCulture) : "0";
                        detail.T_NGUONKHAC_CL = ado.tNguonKhacCl.HasValue ? ado.tNguonKhacCl.Value.ToString("G27", CultureInfo.InvariantCulture) : "";
                        if (ado.tTranTT.HasValue)
                            detail.T_TRANTT = ado.tTranTT.Value.ToString("G27", CultureInfo.InvariantCulture);
                        else
                            detail.T_TRANTT = "";

                        detail.TEN_DICH_VU = this.ConvertStringToXmlDocument(ado.tenDichVu);
                        detail.TEN_VAT_TU = this.ConvertStringToXmlDocument(ado.tenVatTu);
                        detail.THANH_TIEN_BH = ado.thanhTienBH.ToString("G27", CultureInfo.InvariantCulture);
                        detail.THANH_TIEN_BV = ado.thanhTienBV.ToString("G27", CultureInfo.InvariantCulture);
                        detail.TT_THAU = this.ConvertStringToXmlDocument(ado.ttThau);
                        detail.TYLE_TT_BH = ado.tyLeThanhToanBH.ToString("G27", CultureInfo.InvariantCulture);
                        detail.TYLE_TT_DV = ado.tyLeThanhToanDV.ToString("G27", CultureInfo.InvariantCulture);
                        //
                        detail.MA_PTTT_QT = ado.maPtttQt;
                        detail.MA_XANG_DAU = ado.maXangDau;
                        detail.DON_GIA_BV = ado.donGiaBV.ToString("G27", CultureInfo.InvariantCulture);
                        detail.DON_GIA_BH = ado.donGiaBH.ToString("G27", CultureInfo.InvariantCulture);
                        detail.NGUOI_THUC_HIEN = ado.nguoiThucHien;
                        detail.MA_BENH_YHCT = ado.maBenhYHCT;
                        detail.NGAY_TH_YL = ado.ngayTHYL;
                        detail.VET_THUONG_TP = ado.vetThuongTP;
                        detail.PP_VO_CAM = ado.ppVoCam;
                        detail.VI_TRI_TH_DVKT = ado.viTriThDVKT;
                        detail.MA_MAY = ado.maMay;
                        detail.MA_HIEU_SP = ado.maHieuSp;
                        detail.TAI_SU_DUNG = ado.taiSuDung;
                        detail.DU_PHONG = ado.duPhong;

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

        private List<XML3ADO> ProcessDetailsData(InputXml3ADO data)
        {
            List<XML3ADO> result = null;
            try
            {
                string Config_PatientTypeCodeBHYTOption = "";

                if (data.ConfigData != null && data.ConfigData.Count > 0)
                {
                    Config_PatientTypeCodeBHYTOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.PatientTypeCodeBHYTCFG);

                }
                bool Config_BedTimeOption = false;
                string ConfigNguoiThucHienOption = "0";
                if (data.ConfigData != null && data.ConfigData.Count > 0)
                {
                    Config_BedTimeOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.BedTimeOption) == "1";
                    ConfigNguoiThucHienOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.NguoiThucHienOption);
                    if (string.IsNullOrEmpty(ConfigNguoiThucHienOption))
                    {
                        ConfigNguoiThucHienOption = "0";
                    }
                }

                result = new List<XML3ADO>();
                var listHeinServiceType = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT
                };

                var listHeinServiceTypeMaterial = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                };

                var listHeinServiceTypeMaterialGiuong = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN
                };
                //lấy các dịch vụ không phải là thuốc, máu và có tỷ lệ chi trả tiền, các dịch vụ được hưởng BHYT
                if (data.ListSereServ == null) return result;
                var listServeservs = data.ListSereServ.Where(s => s != null && (s.TDL_HEIN_SERVICE_TYPE_ID.HasValue && listHeinServiceType.Contains(s.TDL_HEIN_SERVICE_TYPE_ID.Value) || listHeinServiceTypeMaterial.Contains(s.TDL_HEIN_SERVICE_TYPE_ID.Value))).OrderBy(b => b.INTRUCTION_TIME).ToList();
                if (listServeservs == null) return result;
                Dictionary<long, List<V_HIS_SERE_SERV_2>> dicChildSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_2>>();
                foreach (var hisSereServ in listServeservs)
                {
                    if (hisSereServ.PARENT_ID.HasValue)
                    {
                        if (!dicChildSereServ.ContainsKey(hisSereServ.PARENT_ID.Value))
                            dicChildSereServ[hisSereServ.PARENT_ID.Value] = new List<V_HIS_SERE_SERV_2>();
                        dicChildSereServ[hisSereServ.PARENT_ID.Value].Add(hisSereServ);
                    }
                }
                var listHighTech = listServeservs.Where(o => dicChildSereServ.ContainsKey(o.ID)).OrderBy(b => b.INTRUCTION_TIME).ToList();
                Dictionary<long, string> dicHighTech = new Dictionary<long, string>();

                int count = 1;
                foreach (var hisSereServ in listServeservs)
                {
                    string maLienKet = "";
                    int stt = count++;
                    string maDichVu = "";
                    string maPtttQt = "";
                    string maVatTu = "";
                    string maNhom = "";
                    string goiVTYT = "";
                    string tenVatTu = "";
                    string tenDichVu = "";
                    string maXangDau = "";
                    string donViTinh = "";
                    string phamVi = "";
                    decimal soLuong = 0;
                    decimal donGiaBV = 0;
                    decimal donGiaBH = 0;
                    string ttThau = "";
                    decimal tyLeThanhToanDV = 0;
                    decimal tyLeThanhToanBH = 0;
                    decimal thanhTienBV = 0;
                    decimal thanhTienBH = 0;
                    decimal? tTranTT = null;
                    int mucHuong = 0;
                    decimal? tNguonKhacNsnn = null;
                    decimal? tNguonKhacVtnn = null;
                    decimal? tNguonKhacVttn = null;
                    decimal? tNguonKhacCl = 0;
                    decimal tNguonKhac = 0;
                    decimal tBhtt = 0;
                    decimal tBntt = 0;
                    decimal tBncct = 0;
                    string maKhoa = "";
                    string maGiuong = "";
                    string maBacSi = "";
                    string nguoiThucHien = "";
                    string maBenh = "";
                    string maBenhYHCT = "";
                    string ngayYL = "";
                    string ngayTHYL = "";
                    string ngayKQ = "";
                    int maPTTT = 1;
                    string vetThuongTP = "";
                    string ppVoCam = "";
                    string viTriThDVKT = "";
                    string maMay = "";
                    string maHieuSp = "";
                    string taiSuDung = "";
                    string duPhong = "";

                    var xml3 = new XML3ADO();
                    var patientType = data.PatientTypes != null ? data.PatientTypes.Where(o => o.ID == hisSereServ.PATIENT_TYPE_ID).FirstOrDefault() : null;

                    var hisSereServTein = data.vHisSereServTeins != null ? data.vHisSereServTeins.Where(o => o.SERE_SERV_ID == hisSereServ.ID).ToList() : null;

                    maLienKet = data.Treatment.TREATMENT_CODE ?? "";

                    maNhom = hisSereServ.HST_BHYT_CODE ?? "";
                    if (hisSereServ.HST_BHYT_CODE_IN_TIME.HasValue && data.Treatment.IN_TIME < hisSereServ.HST_BHYT_CODE_IN_TIME.Value)
                    {
                        maNhom = hisSereServ.OLD_HST_BHYT_CODE ?? (hisSereServ.HST_BHYT_CODE ?? "");
                    }
                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                    {
                        xml3.IsMaterial = true;
                        maVatTu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        tenVatTu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                        tTranTT = hisSereServ.HEIN_LIMIT_PRICE;
                        if (!String.IsNullOrEmpty(hisSereServ.MATERIAL_SERIAL_NUMBER))
                        {
                            taiSuDung = "1";
                            maHieuSp = hisSereServ.MATERIAL_SERIAL_NUMBER;
                        }
                        else
                        {
                            maHieuSp = hisSereServ.MODEL_CODE ?? "";
                        }

                        if (hisSereServ.MATERIAL_TT_THAU != null)
                        {
                            ttThau = hisSereServ.MATERIAL_TT_THAU;
                        }
                        else
                        {
                            if (hisSereServ.MATERIAL_INFORMATION_BID == 3)
                            {
                                ttThau = string.Format("{0}{1}", !string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_EXTRA_CODE) ? hisSereServ.MATERIAL_BID_EXTRA_CODE + ";" : null, !string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_YEAR) ? hisSereServ.MATERIAL_BID_YEAR : null);
                            }
                            else if (hisSereServ.MATERIAL_INFORMATION_BID == 4)
                            {
                                ttThau = string.Format("{0}{1}{2}", !string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_EXTRA_CODE) ? hisSereServ.MATERIAL_BID_EXTRA_CODE + ";" : null, !string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_PACKAGE_CODE) ? hisSereServ.MATERIAL_BID_PACKAGE_CODE + ";" : null, !string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_YEAR) ? hisSereServ.MATERIAL_BID_YEAR : null);
                            }
                            else
                            {
                                ttThau = string.Format("{0}{1}{2}{3}", !string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_EXTRA_CODE) ? hisSereServ.MATERIAL_BID_EXTRA_CODE + ";" : null, !string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_PACKAGE_CODE) ? hisSereServ.MATERIAL_BID_PACKAGE_CODE + ";" : null, !string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_GROUP_CODE) ? hisSereServ.MATERIAL_BID_GROUP_CODE + ";" : null, !string.IsNullOrEmpty(hisSereServ.MATERIAL_BID_YEAR) ? hisSereServ.MATERIAL_BID_YEAR : null);
                            }

                        }

                        maKhoa = hisSereServ.REQUEST_BHYT_CODE ?? "";
                        //tenDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";

                        if (hisSereServ.PARENT_ID.HasValue)
                        {
                            var parent = listHighTech.FirstOrDefault(o => o.ID == hisSereServ.PARENT_ID.Value);
                            if (parent != null)
                            {
                                maKhoa = parent.REQUEST_BHYT_CODE ?? "";
                                maDichVu = parent.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                                //tenDichVu = parent.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                                if (!dicHighTech.ContainsKey(hisSereServ.PARENT_ID.Value))
                                {
                                    dicHighTech[hisSereServ.PARENT_ID.Value] = "G" + (dicHighTech.Count + 1);
                                }

                                goiVTYT = dicHighTech[hisSereServ.PARENT_ID.Value];
                            }
                        }
                    }
                    else
                    {
                        maKhoa = hisSereServ.REQUEST_BHYT_CODE ?? "";
                        maDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        tenDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                        if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT ||
                            hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT ||
                            hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN ||
                            hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            maGiuong = ".";
                            if (data.BedLogs != null && data.BedLogs.Count > 0)
                            {
                                var bedlog = data.BedLogs.Where(o => o.SERVICE_REQ_ID.HasValue && o.SERVICE_REQ_ID.Value == hisSereServ.SERVICE_REQ_ID && o.BED_SERVICE_TYPE_ID == hisSereServ.SERVICE_ID && o.SHARE_COUNT == hisSereServ.SHARE_COUNT).ToList();

                                //nếu không có service_req_id thì tìm theo thời gian y lệnh
                                if (bedlog == null || bedlog.Count <= 0)
                                {
                                    bedlog = data.BedLogs.Where(o => o.ID == hisSereServ.BED_LOG_ID).ToList();
                                }

                                //nếu không có service_req_id thì tìm theo thời gian y lệnh
                                if (bedlog == null || bedlog.Count <= 0)
                                {
                                    bedlog = data.BedLogs.Where(o => o.BED_SERVICE_TYPE_ID == hisSereServ.SERVICE_ID && o.SHARE_COUNT == hisSereServ.SHARE_COUNT && o.START_TIME <= hisSereServ.TDL_INTRUCTION_TIME && (o.FINISH_TIME.HasValue && o.FINISH_TIME.Value >= hisSereServ.TDL_INTRUCTION_TIME)).ToList();
                                }

                                if (bedlog != null && bedlog.Count > 0)
                                {
                                    List<string> bedCodes = bedlog.Select(s => s.BED_CODE).Distinct().ToList();
                                    maGiuong = String.Join(";", bedCodes);
                                }
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(maDichVu) && hisSereServ.IS_NO_EXECUTE == 1)
                        maDichVu = maDichVu + "_TB";

                    var sereServPttt = data.SereServPttts != null ? data.SereServPttts.Where(o => o.SERE_SERV_ID == hisSereServ.ID).FirstOrDefault() : null;

                    if (sereServPttt != null)
                    {
                        List<string> mabenhPttt = new List<string>();
                        if (!String.IsNullOrEmpty(sereServPttt.ICD_CM_CODE))
                            mabenhPttt.Add(sereServPttt.ICD_CM_CODE);
                        if (!String.IsNullOrEmpty(sereServPttt.ICD_CM_SUB_CODE))
                            mabenhPttt.Add(sereServPttt.ICD_CM_SUB_CODE);
                        maPtttQt = string.Join(";", mabenhPttt);
                    }

                    var service = data.Services != null ? data.Services.Where(o => o.ID == hisSereServ.SERVICE_ID).FirstOrDefault() : null;
                    if (service != null)
                    {
                        maXangDau = service.PETROLEUM_CODE ?? "";
                    }
                    donViTinh = hisSereServ.SERVICE_UNIT_NAME ?? "";

                    soLuong = Math.Round(hisSereServ.AMOUNT, 3, MidpointRounding.AwayFromZero);

                    decimal? donGiaBV_TamTinh = null;
                    if (hisSereServ.PRIMARY_PATIENT_TYPE_ID != null)
                        donGiaBV_TamTinh = hisSereServ.LIMIT_PRICE;
                    else
                        donGiaBV_TamTinh = hisSereServ.PRIMARY_PRICE;
                    if (donGiaBV_TamTinh > 0)
                        donGiaBV = Math.Round(donGiaBV_TamTinh.Value * (1 + hisSereServ.VAT_RATIO), 3, MidpointRounding.AwayFromZero);
                    else
                        donGiaBV = hisSereServ.VIR_PRICE ?? 0;

                    donGiaBH = Math.Round(hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO), 3, MidpointRounding.AwayFromZero);

                    decimal tyle = 0;
                    if (hisSereServ.ORIGINAL_PRICE > 0)
                    {
                        tyle = hisSereServ.HEIN_LIMIT_PRICE.HasValue ? (hisSereServ.HEIN_LIMIT_PRICE.Value / (hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO))) * 100 : (hisSereServ.PRICE / hisSereServ.ORIGINAL_PRICE) * 100;
                    }
                    tyLeThanhToanDV = Math.Round(tyle, 0);
                    if (patientType.PATIENT_TYPE_CODE == Config_PatientTypeCodeBHYTOption)
                    {
                        if (hisSereServ.HEIN_LIMIT_RATIO.HasValue)
                            tyLeThanhToanBH = Math.Round(hisSereServ.HEIN_LIMIT_RATIO.Value, 0);
                        else
                            tyLeThanhToanBH = 100;
                    }
                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value) && tyLeThanhToanDV != 100)
                    {
                        decimal tmp = tyLeThanhToanDV;
                        tyLeThanhToanDV = tyLeThanhToanBH;
                        tyLeThanhToanBH = tmp;
                    }
                    thanhTienBV = Math.Round(soLuong * donGiaBV * (tyLeThanhToanDV / 100), 2, MidpointRounding.AwayFromZero);
                    thanhTienBH = Math.Round(soLuong * donGiaBH * (tyLeThanhToanDV / 100) * (tyLeThanhToanBH / 100), 2, MidpointRounding.AwayFromZero);

                    mucHuong = hisSereServ.HEIN_RATIO.HasValue ? (int)(hisSereServ.HEIN_RATIO.Value * 100) : 0;

                    tNguonKhac = Math.Round(Math.Round(soLuong, 3, MidpointRounding.AwayFromZero) * Math.Round((hisSereServ.OTHER_SOURCE_PRICE ?? 0), 3, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero);
                    switch (hisSereServ.HEIN_PAY_SOURCE_TYPE_ID)
                    {
                        case 1:
                            tNguonKhacNsnn = tNguonKhac;
                            break;
                        case 2:
                            tNguonKhacVtnn = tNguonKhac;
                            break;
                        case 3:
                            tNguonKhacVttn = tNguonKhac;
                            break;
                        default:
                            tNguonKhacCl = tNguonKhac;
                            break;
                    }

                    tBhtt = Math.Round(thanhTienBH * (hisSereServ.HEIN_RATIO ?? 0), 2, MidpointRounding.AwayFromZero);
                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0) && Math.Abs(tBhtt - Math.Round((hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0), 2, MidpointRounding.AwayFromZero)) > 1)
                    {
                        tBhtt = Math.Round((hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0), 2, MidpointRounding.AwayFromZero);
                        tBncct = Math.Round((hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0), 2, MidpointRounding.AwayFromZero);
                        tBntt = Math.Round(thanhTienBV - tBhtt - tBncct - tNguonKhac, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        tBhtt = Math.Round(thanhTienBH * (hisSereServ.HEIN_RATIO ?? 0), 2, MidpointRounding.AwayFromZero);
                        tBncct = Math.Round((thanhTienBH - tBhtt - tNguonKhac) < 0 ? 0 : (thanhTienBH - tBhtt - tNguonKhac), 2, MidpointRounding.AwayFromZero);
                        tBntt = Math.Round((thanhTienBV - thanhTienBH - tNguonKhac) <= 0 ? 0 : (thanhTienBV - thanhTienBH), 2, MidpointRounding.AwayFromZero);
                    }
                    if (tBntt < 0)
                        tBntt = 0;

                    if (tyLeThanhToanBH > 0)
                    {
                        //    if (!String.IsNullOrEmpty(hisSereServ.HEIN_CARD_NUMBER) &&
                        //(hisSereServ.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CA") ||
                        //hisSereServ.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CY") ||
                        //hisSereServ.HEIN_CARD_NUMBER.Substring(0, 2).Equals("QN")))
                        //    {
                        //        phamVi = "3";
                        //    }
                        //    else
                        phamVi = "1";
                    }
                    else if (tyLeThanhToanBH == 0)
                    {
                        phamVi = "2";
                    }

                    List<string> lstMaBacSi = new List<string>();
                    List<string> lstNguoiThucHien = new List<string>();
                    List<string> lstNguoiThucHien3176 = new List<string>();
                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                    {
                        if (!String.IsNullOrEmpty(hisSereServ.EXECUTE_LOGINNAME))
                            lstMaBacSi.Add(GetMaBacSi(hisSereServ.EXECUTE_LOGINNAME, data.Employees));
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(hisSereServ.REQUEST_LOGINNAME))
                            lstMaBacSi.Add(GetMaBacSi(hisSereServ.REQUEST_LOGINNAME, data.Employees));

                    }

                    string[] lstNguoiThucHiencf = ConfigNguoiThucHienOption.Split(',');
                    long configNguoiThucHien;
                    List<long> lstconfigNguoiThucHien = new List<long>();

                    if (lstNguoiThucHiencf.Length > 1)
                    {
                        foreach (string item in lstNguoiThucHiencf)
                        {
                             configNguoiThucHien = long.Parse(item);
                             lstconfigNguoiThucHien.Add(configNguoiThucHien);
                        }
                    }
                  //  long configNguoiThucHien = long.Parse(ConfigNguoiThucHienOption);
                    
                    // danh sach HIS_HEIN_SERVICE_TYPE
                    long[] lstServiceType = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };
                    bool IsNguoiThucHien = false;

                    foreach (long item in lstServiceType)
                    {
                        if (lstconfigNguoiThucHien != null && lstconfigNguoiThucHien.Count > 0)
                        {
                            foreach(long i in lstconfigNguoiThucHien)
                            {
                                if (item == i)
                                {
                                    IsNguoiThucHien = true;
                                    break;
                                }
                            }
                        }
                       
                    }
                    if (IsNguoiThucHien == true)
                    {
                        if (hisSereServ.EKIP_ID.HasValue && data.EkipUsers != null && data.EkipUsers.Count > 0)
                        {
                            //có kíp add theo kíp
                            var dataEkip = data.EkipUsers.Where(o => o.EKIP_ID == hisSereServ.EKIP_ID.Value).ToList();
                            if (dataEkip != null && dataEkip.Count > 0)
                            {
                                foreach (var item in dataEkip)
                                {
                                    string cchn = GetMaBacSi(item.LOGINNAME, data.Employees);
                                    if (!String.IsNullOrWhiteSpace(cchn))
                                    {
                                        lstNguoiThucHien.Add(cchn);
                                        lstNguoiThucHien3176.Add(cchn);
                                    }
                                    else
                                    {
                                        cchn = GetCCCD(item.LOGINNAME,data.Employees);
                                        if (!String.IsNullOrWhiteSpace(cchn))
                                            lstNguoiThucHien3176.Add(cchn);
                                    }
                                }
                            }
                        }
                    }

                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                    {
                        if ((hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT) && hisSereServ.EKIP_ID.HasValue)
                        {
                            goto Ekip;
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(hisSereServ.SAMPLER_LOGINNAME))
                            {
                                string cchn = GetMaBacSi(hisSereServ.SAMPLER_LOGINNAME, data.Employees);
                                if (!String.IsNullOrWhiteSpace(cchn))
                                {
                                    lstNguoiThucHien.Add(cchn);
                                    lstNguoiThucHien3176.Add(cchn);
                                }
                                else
                                {
                                    cchn = GetCCCD(hisSereServ.SAMPLER_LOGINNAME, data.Employees);
                                    if (!String.IsNullOrWhiteSpace(cchn))
                                        lstNguoiThucHien3176.Add(cchn);
                                }
                            }
                            if (!String.IsNullOrWhiteSpace(hisSereServ.SUBCLINICAL_RESULT_LOGINNAME))
                            {
                                string[] loginname = hisSereServ.SUBCLINICAL_RESULT_LOGINNAME.Split(';');
                                var lstTmp = new List<string>();
                                foreach (var item in loginname)
                                {
                                    string cchn = GetMaBacSi(item, data.Employees);
                                    if (!String.IsNullOrWhiteSpace(cchn))
                                    {
                                        lstTmp.Add(cchn);
                                        lstNguoiThucHien3176.Add(cchn);
                                    }
                                    else
                                    {
                                        cchn = GetCCCD(item, data.Employees);
                                        if (!String.IsNullOrWhiteSpace(cchn))
                                            lstNguoiThucHien3176.Add(cchn);
                                    }
                                }
                                lstNguoiThucHien.AddRange(lstTmp.Distinct().ToList());
                            }
                            if (!String.IsNullOrEmpty(hisSereServ.EXECUTE_LOGINNAME))
                            {
                                string cchn = GetMaBacSi(hisSereServ.EXECUTE_LOGINNAME, data.Employees);
                                if (!String.IsNullOrWhiteSpace(cchn))
                                {
                                    lstNguoiThucHien.Add(cchn);
                                    lstNguoiThucHien3176.Add(cchn);
                                }
                                else
                                {
                                    cchn = GetCCCD(hisSereServ.EXECUTE_LOGINNAME, data.Employees);
                                    if (!String.IsNullOrWhiteSpace(cchn))
                                        lstNguoiThucHien3176.Add(cchn);
                                }
                            }
                            goto Ekip;
                        }
                    Ekip:
                        if (hisSereServ.EKIP_ID.HasValue && data.EkipUsers != null && data.EkipUsers.Count > 0)
                        {
                            //có kíp add theo kíp
                            var dataEkip = data.EkipUsers.Where(o => o.EKIP_ID == hisSereServ.EKIP_ID.Value).ToList();
                            if (dataEkip != null && dataEkip.Count > 0)
                            {
                                foreach (var item in dataEkip)
                                {
                                    string cchn = GetMaBacSi(item.LOGINNAME, data.Employees);
                                    if (!String.IsNullOrWhiteSpace(cchn))
                                    {
                                        lstNguoiThucHien.Add(cchn);
                                        lstNguoiThucHien3176.Add(cchn);
                                    }
                                    else
                                    {
                                        cchn = GetCCCD(item.LOGINNAME, data.Employees);
                                        if (!String.IsNullOrWhiteSpace(cchn))
                                            lstNguoiThucHien3176.Add(cchn);
                                    }
                                }
                            }
                        }
                    }

                    if (hisSereServ != null && (lstNguoiThucHien == null || lstNguoiThucHien.Count == 0))
                    {
                        if (hisSereServ.START_TIME == null && !string.IsNullOrEmpty(hisSereServ.REQUEST_LOGINNAME))
                        {
                            string cchn = GetMaBacSi(hisSereServ.REQUEST_LOGINNAME, data.Employees);
                            if (!String.IsNullOrWhiteSpace(cchn))
                            {
                                lstNguoiThucHien.Add(cchn);
                                lstNguoiThucHien3176.Add(cchn);
                            }
                            else
                            {
                                cchn = GetCCCD(hisSereServ.REQUEST_LOGINNAME, data.Employees);
                                if (!String.IsNullOrWhiteSpace(cchn))
                                    lstNguoiThucHien3176.Add(cchn);
                            }
                        }
                    }

                    maBacSi = string.Join(";", lstMaBacSi.Where(o => !String.IsNullOrWhiteSpace(o)).Distinct());
                    nguoiThucHien = string.Join(";", lstNguoiThucHien.Where(o => !String.IsNullOrWhiteSpace(o)).Distinct());

                    List<string> lstMaBenh = new List<string>();
                    if (!String.IsNullOrWhiteSpace(hisSereServ.ICD_CODE))
                    {
                        lstMaBenh.Add(hisSereServ.ICD_CODE);
                    }
                    if (!String.IsNullOrWhiteSpace(hisSereServ.ICD_SUB_CODE))
                    {
                        var benh = hisSereServ.ICD_SUB_CODE.Trim(';').Split(';').ToList();
                        lstMaBenh.AddRange(benh);
                    }
                    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_CODE))
                    {
                        lstMaBenh = lstMaBenh.Where(o => o != data.Treatment.ICD_CODE).ToList();
                    }
                    //if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_SUB_CODE))
                    //{
                    //    var benh = data.Treatment.ICD_SUB_CODE.Trim(';').Split(';').ToList();
                    //    lstMaBenh.AddRange(benh);
                    //}
                    if (lstMaBenh == null || lstMaBenh.Count == 0)
                    {
                        if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_CODE))
                        {
                            lstMaBenh.Add(data.Treatment.ICD_CODE);
                        }
                    }
                    if (lstMaBenh != null && lstMaBenh.Count == 1)
                    {
                        maBenh = lstMaBenh.First();
                    }
                    else if (lstMaBenh != null && lstMaBenh.Count > 1)
                    {
                        maBenh = string.Join(";", lstMaBenh.Where(o => !String.IsNullOrWhiteSpace(o)).Distinct());
                    }

                    List<string> lstMaBenhYHCT = new List<string>();
                    if (!String.IsNullOrWhiteSpace(hisSereServ.TRADITIONAL_ICD_CODE))
                    {
                        lstMaBenhYHCT.Add(hisSereServ.TRADITIONAL_ICD_CODE);
                    }
                    if (!String.IsNullOrWhiteSpace(hisSereServ.TRADITIONAL_ICD_SUB_CODE))
                    {
                        var benh = hisSereServ.TRADITIONAL_ICD_SUB_CODE.Trim(';').Split(';').ToList();
                        lstMaBenhYHCT.AddRange(benh);
                    }
                    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_CODE))
                    {
                        lstMaBenhYHCT = lstMaBenhYHCT.Where(o => o != data.Treatment.ICD_CODE).ToList();
                    }
                    //if (!String.IsNullOrWhiteSpace(data.Treatment.TRADITIONAL_ICD_CODE))
                    //{
                    //    lstMaBenhYHCT.Add(data.Treatment.TRADITIONAL_ICD_CODE);
                    //}
                    //if (!String.IsNullOrWhiteSpace(data.Treatment.TRADITIONAL_ICD_SUB_CODE))
                    //{
                    //    var benh = data.Treatment.TRADITIONAL_ICD_SUB_CODE.Trim(';').Split(';').ToList();
                    //    lstMaBenhYHCT.AddRange(benh);
                    //}
                    //if (lstMaBenhYHCT == null || lstMaBenhYHCT.Count == 0)
                    //{
                    //    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_CODE))
                    //   {
                    //       lstMaBenhYHCT.Add(data.Treatment.ICD_CODE);
                    //   }
                    //}
                    if (lstMaBenhYHCT != null && lstMaBenhYHCT.Count == 1)
                    {
                        maBenhYHCT = lstMaBenhYHCT.First();
                    }
                    else if (lstMaBenhYHCT != null && lstMaBenhYHCT.Count > 1)
                    {
                        maBenhYHCT = string.Join(";", lstMaBenhYHCT.Where(o => !String.IsNullOrWhiteSpace(o)).Distinct());
                    }

                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                    {
                        if (hisSereServ.START_TIME.HasValue)
                        {
                            ngayYL = hisSereServ.START_TIME.ToString().Substring(0, 12);
                        }
                        else
                        {
                            ngayYL = hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);
                        }
                    }
                    else
                        ngayYL = hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);

                    decimal TimeExecute = hisSereServ.BEGIN_TIME ?? hisSereServ.START_TIME ?? hisSereServ.INTRUCTION_TIME;
                    ngayTHYL = TimeExecute.ToString().Substring(0, 12);

                    if (hisSereServ.END_TIME.HasValue)
                    {
                        ngayKQ = hisSereServ.END_TIME.ToString().Substring(0, 12);
                    }
                    else
                    {
                        if (hisSereServ.FINISH_TIME.HasValue)
                        {
                            ngayKQ = hisSereServ.FINISH_TIME.ToString().Substring(0, 12);
                        }
                        else
                        {
                            ngayKQ = ngayTHYL;
                        }
                    }

                    if (!Config_BedTimeOption)
                    {
                        if (listHeinServiceTypeMaterialGiuong.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
                        {
                            if (data.Treatment.OUT_TIME.HasValue)
                            {
                                ngayKQ = data.Treatment.OUT_TIME.ToString().Substring(0, 12);
                            }
                            var SereServBed = listServeservs.Where(o => listHeinServiceTypeMaterialGiuong.Contains(o.TDL_HEIN_SERVICE_TYPE_ID ?? 0) && o.ID != hisSereServ.ID).ToList();
                            if (SereServBed != null && SereServBed.Count > 0)
                            {
                                SereServBed = SereServBed.Where(o => o.TDL_INTRUCTION_TIME > hisSereServ.TDL_INTRUCTION_TIME).ToList();
                                if (SereServBed.Count > 0)
                                {
                                    var SsIntructionTime = SereServBed.OrderBy(o => o.TDL_INTRUCTION_TIME).ToList()[0];
                                    ngayKQ = SsIntructionTime.TDL_INTRUCTION_TIME.ToString().Substring(0, 12);
                                }
                            }

                            long ngayKqC = Int64.Parse(ngayKQ + "00");
                            long ngayThylC = !string.IsNullOrWhiteSpace(ngayTHYL) ? Int64.Parse(ngayTHYL + "00") : 0;
                            System.DateTime? in_t = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngayKqC);
                            System.DateTime? out_t = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngayThylC);
                            TimeSpan tdiff = in_t.Value - out_t.Value;
                            if (hisSereServ.AMOUNT >= 1)
                            {
                                if (tdiff.TotalHours > (double)(hisSereServ.AMOUNT * 24))
                                    in_t = out_t.Value.AddHours((double)hisSereServ.AMOUNT * 24);
                            }
                            else if (tdiff.TotalHours > 4)
                                in_t = out_t.Value.AddHours(4);
                            ngayKQ = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(in_t).Value.ToString().Substring(0, 12);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ngayKQ))
                            {
                                long ngayKq = Int64.Parse(ngayKQ);
                                long ngayThyl = !string.IsNullOrWhiteSpace(ngayTHYL) ? Int64.Parse(ngayTHYL) : 0;
                                //thinhdt2
                                //sua thoi gian kq 179141
                                Inventec.Common.Logging.LogSystem.Debug("ngayKq: " + ngayKq + " ngayThyl: " + ngayThyl);
                                if (ngayKq <= ngayThyl)
                                {
                                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                                    {
                                        DateTime ngayThylTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngayThyl * 100) ?? DateTime.MinValue;
                                        if (ngayThylTime != DateTime.MinValue)
                                        {
                                            DateTime ngayKQTime = ngayThylTime.AddMinutes(5);
                                            ngayKQ = ((Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(ngayKQTime) ?? 0) / 100).ToString();

                                        }
                                    }
                                    else
                                        ngayKQ = ngayTHYL;
                                    Inventec.Common.Logging.LogSystem.Debug("co ngayKq <= ngayThyl.ngayKQ = " + ngayKQ);
                                }
                            }
                        }
                    }
                    else
                    {

                        if (data.BedLogs != null && data.BedLogs.Count > 0 && data.BedLogs.Exists(o => o.ID == (hisSereServ.BED_LOG_ID ?? 0)))
                        {
                            if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT ||
                                hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT ||
                                hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN ||
                                hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L))
                            {
                                var bed = data.BedLogs != null && data.BedLogs.Count > 0 ? data.BedLogs.FirstOrDefault(o => o.ID == hisSereServ.BED_LOG_ID) : null;
                                ngayKQ = bed != null && bed.FINISH_TIME.HasValue ? bed.FINISH_TIME.ToString().Substring(0, 12) : ngayKQ;
                                if (hisSereServ.BED_LOG_ID.HasValue)
                                {
                                    var sereServs = data.ListSereServ.Where(o => hisSereServ.BED_LOG_ID.Value == o.BED_LOG_ID).ToList();
                                    if (sereServs != null && sereServs.Count > 1)
                                    {
                                        var sere = sereServs.Where(o => o.TDL_INTRUCTION_TIME > hisSereServ.TDL_INTRUCTION_TIME).ToList();
                                        var betterIntructionTime = sere != null && sere.Count > 0 ? sere.Min(o => o.TDL_INTRUCTION_TIME) : 0;

                                        if (betterIntructionTime > 0)
                                        {
                                            ngayKQ = betterIntructionTime.ToString().Substring(0, 12);
                                        }
                                    }
                                }

                                long ngayKqC = Int64.Parse(ngayKQ + "00");
                                long ngayThylC = !string.IsNullOrWhiteSpace(ngayTHYL) ? Int64.Parse(ngayTHYL + "00") : 0;
                                System.DateTime? in_t = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngayKqC);
                                System.DateTime? out_t = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngayThylC);
                                TimeSpan tdiff = in_t.Value - out_t.Value;
                                if (hisSereServ.AMOUNT >= 1)
                                {
                                    if (tdiff.TotalHours > (double)(hisSereServ.AMOUNT * 24))
                                        in_t = out_t.Value.AddHours((double)hisSereServ.AMOUNT * 24);
                                }
                                else if (tdiff.TotalHours > 4)
                                    in_t = out_t.Value.AddHours(4);
                                ngayKQ = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(in_t).Value.ToString().Substring(0, 12);
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(ngayKQ))
                            {
                                long ngayKq = Int64.Parse(ngayKQ);
                                long ngayThyl = !string.IsNullOrWhiteSpace(ngayTHYL) ? Int64.Parse(ngayTHYL) : 0;
                                //thinhdt2
                                //sua thoi gian kq 179141
                                Inventec.Common.Logging.LogSystem.Debug("ngayKq: " + ngayKq + " ngayThyl: " + ngayThyl);
                                if (ngayKq <= ngayThyl)
                                {
                                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                                    {
                                        DateTime ngayThylTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngayThyl*100) ?? DateTime.MinValue;
                                        if (ngayThylTime != DateTime.MinValue)
                                        {
                                            DateTime ngayKQTime = ngayThylTime.AddMinutes(5);
                                            ngayKQ = ((Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(ngayKQTime)??0)/100).ToString();

                                        }
                                    }
                                    else
                                        ngayKQ = ngayTHYL;
                                    Inventec.Common.Logging.LogSystem.Debug("co ngayKq <= ngayThyl.ngayKQ = " + ngayKQ);
                                }
                            }
                        }
                    }



                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT && sereServPttt != null)
                    {
                        ppVoCam = !String.IsNullOrEmpty(sereServPttt.EMME_HEIN_CODE) ? sereServPttt.EMME_HEIN_CODE : (!String.IsNullOrEmpty(sereServPttt.EMME_SECOND_HEIN_CODE) ? sereServPttt.EMME_SECOND_HEIN_CODE : "4");
                    }
                    string serialNum = "";

                    if (hisSereServTein != null && hisSereServTein.Count > 0)
                    {
                        string Config_MaMayOption = "";
                        Config_MaMayOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.MAMAYOPTION);
                        if (Config_MaMayOption == "1")
                        {

                            List<string> lstkq = new List<string>();
                            foreach (var item in hisSereServTein)
                            {
                                string kq = string.Format("{0}.{1}.{2}.{3}", item.MACHINE_GROUP_CODE, item.SOURCE_CODE, data.Treatment.HEIN_MEDI_ORG_CODE, item.SERIAL_NUMBER);
                                lstkq.Add(kq);
                            }
                            maMay = String.Join(";", lstkq.Distinct());
                        }
                        else
                        {
                            var ssTeinHasMachines = hisSereServTein.Where(o => o.MACHINE_ID != null && !String.IsNullOrEmpty(o.SERIAL_NUMBER)).ToList();
                            if (ssTeinHasMachines != null && ssTeinHasMachines.Count > 0)
                            {
                                serialNum = String.Join(";", ssTeinHasMachines.Select(o => o.SERIAL_NUMBER).Distinct());
                                var ssTein = ssTeinHasMachines.First();
                                maMay = String.Format("{0}.{1}.{2}.{3}", ssTein.MACHINE_GROUP_CODE, ssTein.SOURCE_CODE, data.Treatment.HEIN_MEDI_ORG_CODE, serialNum);
                            }
                        }
                    }
                    else if (String.IsNullOrEmpty(maMay) && hisSereServ.MACHINE_ID != null)
                    {
                        maMay = String.Format("{0}.{1}.{2}.{3}", hisSereServ.MACHINE_GROUP_CODE, hisSereServ.SOURCE_CODE, data.Treatment.HEIN_MEDI_ORG_CODE, hisSereServ.SERIAL_NUMBER);
                    }
                    if (data.IS_3176)
                    {
                        donGiaBV = Math.Round(hisSereServ.VIR_PRICE ?? 0, 3, MidpointRounding.AwayFromZero);
                        donGiaBH = Math.Round(((hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0) + (hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)) / hisSereServ.AMOUNT, 3, MidpointRounding.AwayFromZero);
                        if (!string.IsNullOrEmpty(hisSereServ.MATERIAL_SERIAL_NUMBER))
                            donGiaBV = donGiaBH;
                        thanhTienBV = Math.Round(hisSereServ.VIR_TOTAL_PRICE ?? 0, 2);
                        thanhTienBH = Math.Round((hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0) + (hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0), 2);
                        tNguonKhacNsnn = tNguonKhacVtnn = tNguonKhacVttn = null;
                        tNguonKhac = Math.Round((hisSereServ.OTHER_SOURCE_PRICE ?? 0) * (hisSereServ.AMOUNT), 2);
                        if (hisSereServ.HEIN_PAY_SOURCE_TYPE_ID == 1)
                        {
                            tNguonKhacNsnn = tNguonKhac;
                        }
                        else if (hisSereServ.HEIN_PAY_SOURCE_TYPE_ID == 2)
                        {
                            tNguonKhacVtnn = tNguonKhac;
                        }
                        else if (hisSereServ.HEIN_PAY_SOURCE_TYPE_ID == 3)
                        {
                            tNguonKhacVttn = tNguonKhac;
                        }
                        else
                        {
                            tNguonKhacCl = tNguonKhac;
                        }
                        tBhtt = Math.Round((hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0), 2);
                        tBncct = Math.Round((hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0),2);
                        var price = thanhTienBV - tBhtt - tBncct - tNguonKhac;
                        tBntt = Math.Round(price < 0 ? 0 : price, 2);
                        maMay = String.Format("{0}{1}{2}{3}", !string.IsNullOrEmpty(hisSereServ.MACHINE_GROUP_CODE) ? hisSereServ.MACHINE_GROUP_CODE + "." : "", !string.IsNullOrEmpty(hisSereServ.SOURCE_CODE) ? hisSereServ.SOURCE_CODE : "", !string.IsNullOrEmpty(hisSereServ.SOURCE_NAME) ? hisSereServ.SOURCE_NAME + "." : ".", hisSereServ.SERIAL_NUMBER);
                        if (maMay == ".")
                            maMay = "";
                        nguoiThucHien = string.Join(";", lstNguoiThucHien3176.Where(o => !string.IsNullOrEmpty(o)).ToList());
                    }
                    xml3.maLienKet = maLienKet;
                    xml3.stt = stt;
                    xml3.maDichVu = maDichVu;
                    xml3.maPtttQt = maPtttQt;
                    xml3.maVatTu = maVatTu;
                    xml3.maNhom = maNhom;
                    xml3.goiVTYT = goiVTYT;
                    xml3.tenVatTu = tenVatTu;
                    xml3.tenDichVu = tenDichVu;
                    xml3.maXangDau = maXangDau;
                    xml3.donViTinh = donViTinh;
                    xml3.phamVi = phamVi;
                    xml3.soLuong = soLuong;
                    xml3.donGiaBV = donGiaBV;
                    xml3.donGiaBH = donGiaBH;
                    xml3.ttThau = ttThau;
                    xml3.tyLeThanhToanDV = tyLeThanhToanDV;
                    xml3.tyLeThanhToanBH = tyLeThanhToanBH;
                    xml3.thanhTienBV = thanhTienBV;
                    xml3.thanhTienBH = thanhTienBH;
                    xml3.tTranTT = tTranTT;
                    xml3.mucHuong = mucHuong;
                    xml3.tNguonKhacNsnn = tNguonKhacNsnn;
                    xml3.tNguonKhacVtnn = tNguonKhacVtnn;
                    xml3.tNguonKhacVttn = tNguonKhacVttn;
                    xml3.tNguonKhacCl = tNguonKhacCl;
                    xml3.tNguonKhac = tNguonKhac;
                    xml3.tBhtt = tBhtt;
                    xml3.tBntt = tBntt;
                    xml3.tBncct = tBncct;
                    xml3.maKhoa = maKhoa;
                    xml3.maGiuong = maGiuong;
                    xml3.maBacSi = maBacSi;
                    xml3.nguoiThucHien = nguoiThucHien;
                    xml3.maBenh = maBenh;
                    xml3.maBenhYHCT = maBenhYHCT;
                    xml3.ngayYL = ngayYL;
                    xml3.ngayTHYL = ngayTHYL;
                    xml3.ngayKQ = ngayKQ;
                    xml3.maPTTT = maPTTT;
                    xml3.vetThuongTP = vetThuongTP;
                    xml3.ppVoCam = ppVoCam;
                    xml3.viTriThDVKT = viTriThDVKT;
                    xml3.maMay = maMay;
                    xml3.maHieuSp = maHieuSp;
                    xml3.taiSuDung = taiSuDung;
                    xml3.duPhong = duPhong;

                    result.Add(xml3);
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private string GetMaBacSi(string loginName, List<HIS_EMPLOYEE> listEmployees)
        {
            string result = "";
            try
            {
                if (String.IsNullOrEmpty(loginName))
                    return result;
                if (listEmployees != null)
                {
                    var dataEmployee = listEmployees.FirstOrDefault(p => p.LOGINNAME == loginName);
                    if (dataEmployee != null)
                    {
                        result = dataEmployee.DIPLOMA ?? "";
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("ListEmployees null");
                }
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        private string GetCCCD(string loginName, List<HIS_EMPLOYEE> listEmployees)
        {
            string result = "";
            try
            {
                if (String.IsNullOrEmpty(loginName))
                    return result;
                if (listEmployees != null)
                {
                    var dataEmployee = listEmployees.FirstOrDefault(p => p.LOGINNAME == loginName);
                    if (dataEmployee != null)
                    {
                        result = dataEmployee.IDENTIFICATION_NUMBER ?? "";
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("ListEmployees null");
                }
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        internal XmlCDataSection ConvertStringToXmlDocument(string data)
        {
            XmlCDataSection result;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<book genre='novel' ISBN='1-861001-57-5'>" + "<title>Pride And Prejudice</title>" + "</book>");
            result = doc.CreateCDataSection(RemoveXmlCharError(data));
            return result;
        }

        internal string RemoveXmlCharError(string data)
        {
            string result = "";
            try
            {
                StringBuilder s = new StringBuilder();
                if (!String.IsNullOrWhiteSpace(data))
                {
                    foreach (char c in data)
                    {
                        if (!System.Xml.XmlConvert.IsXmlChar(c)) continue;
                        s.Append(c);
                    }
                }

                result = s.ToString();
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
