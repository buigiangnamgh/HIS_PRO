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
                string NgayYLenhOption = "";
                string MaBacSiHeinServiceType = "";
                string NgayKetQuaOption = "";
                string MaBacSiOption = "";
                if (data.ConfigData != null && data.ConfigData.Count > 0)
                {
                    Config_PatientTypeCodeBHYTOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.PatientTypeCodeBHYTCFG);
                    NgayYLenhOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.NgayYlenhOption);
                    MaBacSiHeinServiceType = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.MA_BAC_SI_HEIN_SERVICE_TYPE);
                    MaBacSiOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.MaBacSiOption);
                    NgayKetQuaOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.XML__4210__XML3__NGAY_KQ_OPTION);
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
                //lấy các dịch vụ không phải là thuốc, máu và có tỷ lệ chi trả tiền, các dịch vụ được hưởng BHYT
                if (data.ListSereServ == null) return result;
                var listServeservs = data.ListSereServ.Where(s => s != null && (listHeinServiceType.Contains(s.TDL_HEIN_SERVICE_TYPE_ID.Value) || listHeinServiceTypeMaterial.Contains(s.TDL_HEIN_SERVICE_TYPE_ID.Value))).OrderBy(b => b.INTRUCTION_TIME).ToList();
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
                    decimal? tNguonKhacCl = null;
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
                    if (NgayKetQuaOption == "1" && hisSereServ.FINISH_TIME.HasValue)
                    {
                        ngayKQ = hisSereServ.FINISH_TIME < hisSereServ.INTRUCTION_TIME ? hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12) : hisSereServ.FINISH_TIME.ToString().Substring(0, 12);
                    }
                    else if (NgayKetQuaOption == "2")
                    {
                        ngayKQ = hisSereServ.FINISH_TIME.HasValue ? hisSereServ.FINISH_TIME.ToString().Substring(0, 12) : "";
                    }
                    if (listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                    {
                        xml3.IsMaterial = true;
                        maVatTu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        tenVatTu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                        tTranTT = hisSereServ.HEIN_LIMIT_PRICE;
                        maHieuSp = hisSereServ.MODEL_CODE ?? "";
                        if (!String.IsNullOrEmpty(hisSereServ.MATERIAL_SERIAL_NUMBER))
                            taiSuDung = "1";
                        if (string.IsNullOrWhiteSpace(hisSereServ.MATERIAL_BID_GROUP_CODE) || !hisSereServ.MATERIAL_BID_GROUP_CODE.StartsWith("N"))
                        {
                            hisSereServ.MATERIAL_BID_GROUP_CODE = "N0";
                        }
                        if (hisSereServ.MATERIAL_INFORMATION_BID == 2)
                        {
                            ttThau = string.Format("{0};{1};{2};{3}", hisSereServ.MATERIAL_BID_EXTRA_CODE, hisSereServ.MATERIAL_BID_PACKAGE_CODE, hisSereServ.MATERIAL_BID_GROUP_CODE, hisSereServ.MATERIAL_BID_YEAR);
                        }
                        else if (hisSereServ.MATERIAL_INFORMATION_BID == 3)
                        {
                            ttThau = string.Format("{0};{1}", hisSereServ.MATERIAL_BID_EXTRA_CODE, hisSereServ.MATERIAL_BID_YEAR);
                        }
                        else if (hisSereServ.MATERIAL_INFORMATION_BID == 4)
                        {
                            ttThau = string.Format("{0};{1};{2}", hisSereServ.MATERIAL_BID_EXTRA_CODE, hisSereServ.MATERIAL_BID_PACKAGE_CODE, hisSereServ.MATERIAL_BID_YEAR);
                        }
                        else
                        {
                            ttThau = string.Format("{0};{1};{2};{3}", hisSereServ.MATERIAL_BID_EXTRA_CODE, hisSereServ.MATERIAL_BID_PACKAGE_CODE, hisSereServ.MATERIAL_BID_GROUP_CODE, hisSereServ.MATERIAL_BID_YEAR);
                        }

                        maKhoa = hisSereServ.REQUEST_BHYT_CODE ?? "";
                        tenDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";

                        if (hisSereServ.PARENT_ID.HasValue)
                        {
                            var parent = listHighTech.FirstOrDefault(o => o.ID == hisSereServ.PARENT_ID.Value);
                            if (parent != null)
                            {
                                maKhoa = parent.REQUEST_BHYT_CODE ?? "";
                                maDichVu = parent.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                                tenDichVu = parent.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
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
                            }
                        }
                    }
                    // vật tư có ngày kết quả < ngày y lệnh thì để trống ngày kết quả
                    if (listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value) && hisSereServ.FINISH_TIME > 0 && hisSereServ.FINISH_TIME < hisSereServ.INTRUCTION_TIME)
                    {
                        ngayKQ = "";
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

                    //decimal? donGiaBV_TamTinh = null;
                    //if (hisSereServ.PRIMARY_PATIENT_TYPE_ID != null)
                    //    donGiaBV_TamTinh = hisSereServ.LIMIT_PRICE;
                    //else
                    //    donGiaBV_TamTinh = hisSereServ.PRIMARY_PRICE;
                    //if (donGiaBV_TamTinh > 0)
                    //    donGiaBV = Math.Round(donGiaBV_TamTinh.Value * (1 + hisSereServ.VAT_RATIO), 3, MidpointRounding.AwayFromZero);
                    //else
                    donGiaBV = Math.Round(hisSereServ.VIR_PRICE ?? 0, 3, MidpointRounding.AwayFromZero);
                    tyLeThanhToanDV = 100;

                    thanhTienBV = Math.Round(soLuong * donGiaBV * (tyLeThanhToanDV / 100), 2, MidpointRounding.AwayFromZero);

                    mucHuong = hisSereServ.HEIN_RATIO.HasValue ? (int)(hisSereServ.HEIN_RATIO.Value * 100) : 0;

                    tNguonKhac = Math.Round(soLuong * (hisSereServ.OTHER_SOURCE_PRICE ?? 0), 2, MidpointRounding.AwayFromZero);
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
                    donGiaBH = Math.Round(hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO), 3, MidpointRounding.AwayFromZero);

                    tBhtt = Math.Round((hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0), 2, MidpointRounding.AwayFromZero);
                    tBncct = Math.Round((hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0), 2, MidpointRounding.AwayFromZero);
                    tBntt = Math.Round((hisSereServ.VIR_TOTAL_PATIENT_PRICE ?? 0) - (hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0) - tNguonKhac, 2, MidpointRounding.AwayFromZero);
                    thanhTienBH = Math.Round((hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0) + (hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0), 2, MidpointRounding.AwayFromZero);

                    decimal tyle = 0;
                    if (hisSereServ.ORIGINAL_PRICE > 0)
                    {
                        tyle = ((hisSereServ.VIR_TOTAL_HEIN_PRICE ?? 0) + (hisSereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)) / (hisSereServ.AMOUNT * (hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO))) * 100;
                    }
                   
                    if (patientType.PATIENT_TYPE_CODE == Config_PatientTypeCodeBHYTOption)
                    {
                        tyLeThanhToanBH = Math.Round(tyle, 0);
                    }
                    else
                    {
                        tyLeThanhToanBH = 0;
                    }
                    if (tBntt < 0)
                    {
                        tBncct += tBntt;
                        tBntt = 0;
                    }
                    if (tBhtt > 0)
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
                    else if (tBhtt == 0)
                    {
                        phamVi = "2";
                    }

                    if (String.IsNullOrEmpty(MaBacSiHeinServiceType) || (!String.IsNullOrEmpty(MaBacSiHeinServiceType) && data.Services != null && data.Services.Count > 0 && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.HasValue && data.Services.FirstOrDefault(o => o.ID == hisSereServ.SERVICE_ID) != null && data.Services.FirstOrDefault(o => o.ID == hisSereServ.SERVICE_ID).HEIN_SERVICE_TYPE_CODE != MaBacSiHeinServiceType))
                    {
                        List<String> lstMaBacSi = ProcessorGetMaBacSi(hisSereServ, MaBacSiOption, data.EkipUsers, listHeinServiceTypeMaterial, data.Employees);

                        if (MaBacSiOption == "4")
                            maBacSi = string.Join(";", lstMaBacSi.ToList());
                        else
                            maBacSi = string.Join(";", lstMaBacSi.Distinct().ToList());
                    }
                    List<string> lstNguoiThucHien = new List<string>();
                    if (!String.IsNullOrEmpty(hisSereServ.SAMPLER_LOGINNAME))
                        lstNguoiThucHien.Add(GetMaBacSi(hisSereServ.SAMPLER_LOGINNAME, data.Employees));

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
                            }
                        }
                        lstNguoiThucHien.AddRange(lstTmp.Distinct().ToList());
                    }
                    if (!String.IsNullOrEmpty(hisSereServ.EXECUTE_LOGINNAME))
                        lstNguoiThucHien.Add(GetMaBacSi(hisSereServ.EXECUTE_LOGINNAME, data.Employees));

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
                                    lstNguoiThucHien.Add(cchn);
                            }
                        }
                    }
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
                        lstMaBenh.Add(data.Treatment.ICD_CODE);
                    }
                    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_SUB_CODE))
                    {
                        var benh = data.Treatment.ICD_SUB_CODE.Trim(';').Split(';').ToList();
                        lstMaBenh.AddRange(benh);
                    }
                    maBenh = string.Join(";", lstMaBenh.Where(o => !String.IsNullOrWhiteSpace(o)).Distinct());
                   if (Encoding.UTF8.GetByteCount(maBenh) > 100)
                    {
                        maBenh = SubStringWithSeparate(maBenh);
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
                    if (!String.IsNullOrWhiteSpace(data.Treatment.TRADITIONAL_ICD_CODE))
                    {
                        lstMaBenhYHCT.Add(data.Treatment.TRADITIONAL_ICD_CODE);
                    }
                    if (!String.IsNullOrWhiteSpace(data.Treatment.TRADITIONAL_ICD_SUB_CODE))
                    {
                        var benh = data.Treatment.TRADITIONAL_ICD_SUB_CODE.Trim(';').Split(';').ToList();
                        lstMaBenhYHCT.AddRange(benh);
                    }
                    maBenhYHCT = string.Join(";", lstMaBenhYHCT.Where(o => !String.IsNullOrWhiteSpace(o)).Distinct());

                    ngayYL = hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);
                    ngayTHYL = hisSereServ.START_TIME.HasValue && hisSereServ.FINISH_TIME.HasValue ? hisSereServ.START_TIME.ToString().Substring(0, 12) : "";
                    if (NgayYLenhOption == "3")
                    {
                        ngayYL = ngayTHYL;
                    }
                    else if (hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && NgayYLenhOption == "2")
                    {
                        ngayYL = ngayTHYL;
                    }
                    else if (hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT ||
                       hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                    {
                        ngayYL = hisSereServ.BEGIN_TIME.HasValue ? hisSereServ.BEGIN_TIME.ToString().Substring(0, 12) : ngayYL;
                        ngayKQ = hisSereServ.END_TIME.HasValue ? hisSereServ.END_TIME.ToString().Substring(0, 12) : ngayKQ;
                    }

                    //ngayKQ = hisSereServ.END_TIME.HasValue ? hisSereServ.END_TIME.ToString().Substring(0, 12) : hisSereServ.FINISH_TIME.HasValue ? hisSereServ.FINISH_TIME.ToString().Substring(0, 12) : hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);
                    //if (Convert.ToInt64(ngayKQ) < Convert.ToInt64(ngayTHYL))
                    //    ngayKQ = ngayTHYL;

                    if ((hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT))
                    {
                        ppVoCam = sereServPttt != null && !String.IsNullOrEmpty(sereServPttt.EMME_HEIN_CODE) ? sereServPttt.EMME_HEIN_CODE : (sereServPttt != null && !String.IsNullOrEmpty(sereServPttt.EMME_SECOND_HEIN_CODE) ? sereServPttt.EMME_SECOND_HEIN_CODE : "4");
                    }
                    string serialNum = "";
                    if (hisSereServTein != null && hisSereServTein.Count > 0)
                    {
                        var ssTeinHasMachines = hisSereServTein.Where(o => o.MACHINE_ID != null && !String.IsNullOrEmpty(o.SERIAL_NUMBER)).ToList();
                        if (ssTeinHasMachines != null && ssTeinHasMachines.Count > 0)
                        {
                            serialNum = String.Join(";", ssTeinHasMachines.Select(o => o.SERIAL_NUMBER).Distinct());
                            var ssTein = ssTeinHasMachines.First();
                            maMay = String.Format("{0}.{1}.{2}.{3}", ssTein.MACHINE_GROUP_CODE, ssTein.SOURCE_CODE, data.Treatment.HEIN_MEDI_ORG_CODE, serialNum);
                        }

                    }
                    else if (String.IsNullOrEmpty(maMay) && hisSereServ.MACHINE_ID != null && !String.IsNullOrEmpty(hisSereServ.SERIAL_NUMBER))
                    {
                        maMay = String.Format("{0}.{1}.{2}.{3}", hisSereServ.MACHINE_GROUP_CODE, hisSereServ.SOURCE_CODE, data.Treatment.HEIN_MEDI_ORG_CODE, hisSereServ.SERIAL_NUMBER);
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
                    Inventec.Common.Logging.LogSystem.Info("Xml3Processor Ma benh " + maBenh);
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

        private string SubStringWithSeparate(string multiCharString)
        {
            string result = "";
            try
            {
                Encoding utf8 = Encoding.UTF8;
                int leng = utf8.GetByteCount(multiCharString);
                if (leng > 100)
                {
                    int index = multiCharString.LastIndexOf(";");
                    while (utf8.GetByteCount(multiCharString) > 100)
                    {
                        index = multiCharString.LastIndexOf(";");
                        multiCharString = multiCharString.Substring(0, index);
                        result = multiCharString;
                    }
                }
                else
                {
                    result = multiCharString;
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<string> ProcessorGetMaBacSi(V_HIS_SERE_SERV_2 hisSereServ, string MaBacSiOption, List<HIS_EKIP_USER> ekipUsers, List<long> listHeinServiceTypeMaterial, List<HIS_EMPLOYEE> employees)
        {
            List<string> lstMaBacSi = new List<string>();
            try
            {
                string executeName = null;
                string reqName = null;
                string samplerLoginName = null;
                List<string> resultName = new List<string>();
                if (MaBacSiOption == "4")
                {
                    if (hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT)
                    {
                        string cchn = GetMaBacSi(hisSereServ.REQUEST_LOGINNAME, employees);
                        if (!String.IsNullOrWhiteSpace(cchn))
                        {
                            lstMaBacSi.Add(cchn);
                        }
                    }
                    else
                    {
                        string cchnA = GetMaBacSi(hisSereServ.REQUEST_LOGINNAME, employees);
                        if (!String.IsNullOrWhiteSpace(cchnA))
                        {
                            lstMaBacSi.Add(cchnA);
                        }
                        if (hisSereServ.EKIP_ID.HasValue && ekipUsers != null && ekipUsers.Count > 0)
                        {
                            //có kíp add theo kíp
                            var dataEkip = ekipUsers.Where(o => o.EKIP_ID == hisSereServ.EKIP_ID.Value).ToList();
                            if (dataEkip != null && dataEkip.Count > 0)
                            {
                                foreach (var item in dataEkip)
                                {
                                    string cchn = GetMaBacSi(item.LOGINNAME, employees);
                                    if (!String.IsNullOrWhiteSpace(cchn))
                                    {
                                        lstMaBacSi.Add(cchn);
                                    }
                                }
                            }
                        }
                        else if (!String.IsNullOrWhiteSpace(hisSereServ.SUBCLINICAL_RESULT_LOGINNAME))
                        {
                            string[] loginname = hisSereServ.SUBCLINICAL_RESULT_LOGINNAME.Split(';');
                            var lstTmp = new List<string>();
                            foreach (var item in loginname)
                            {
                                string cchn = GetMaBacSi(item, employees);
                                if (!String.IsNullOrWhiteSpace(cchn))
                                {
                                    lstTmp.Add(cchn);
                                }
                            }
                            lstMaBacSi.AddRange(lstTmp.Distinct().ToList());
                        }
                        else
                        {
                            string cchnB = GetMaBacSi(hisSereServ.EXECUTE_LOGINNAME, employees);
                            if (!String.IsNullOrWhiteSpace(cchnB))
                            {
                                lstMaBacSi.Add(cchnB);
                            }
                        }
                    }
                }
                else
                {
                    executeName = GetMaBacSi(hisSereServ.EXECUTE_LOGINNAME, employees);
                    reqName = GetMaBacSi(hisSereServ.REQUEST_LOGINNAME, employees);
                    samplerLoginName = GetMaBacSi(hisSereServ.SAMPLER_LOGINNAME, employees);  //Người lấy mẫu
                    resultName = new List<string>();
                    if (MaBacSiOption == "5"
                        && hisSereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN
                        && !String.IsNullOrWhiteSpace(hisSereServ.RECEIVE_SAMPLE_LOGINNAME))
                    {
                        samplerLoginName = GetMaBacSi(hisSereServ.RECEIVE_SAMPLE_LOGINNAME, employees);
                    }
                    else
                        if (!String.IsNullOrWhiteSpace(hisSereServ.SUBCLINICAL_RESULT_LOGINNAME))
                        {
                            string[] loginname = hisSereServ.SUBCLINICAL_RESULT_LOGINNAME.Split(';');
                            foreach (var item in loginname)
                            {
                                string cchn = GetMaBacSi(item, employees);
                                if (!String.IsNullOrWhiteSpace(cchn))
                                {
                                    resultName.Add(cchn);
                                }
                            }
                        }

                    if (!String.IsNullOrWhiteSpace(executeName) || resultName.Count > 0)
                    {
                        if (resultName.Count > 0)
                        {
                            lstMaBacSi.AddRange(resultName);
                        }
                        else
                        {
                            lstMaBacSi.Add(executeName);
                        }
                    }
                    else if (!String.IsNullOrWhiteSpace(reqName))
                    {
                        lstMaBacSi.Add(reqName);
                    }

                    if (hisSereServ.EKIP_ID.HasValue && ekipUsers != null && ekipUsers.Count > 0)
                    {
                        //có kíp add theo kíp
                        var dataEkip = ekipUsers.Where(o => o.EKIP_ID == hisSereServ.EKIP_ID.Value).Distinct().ToList();
                        if (dataEkip != null && dataEkip.Count > 0)
                        {
                            lstMaBacSi = new List<string>();
                            //thêm nã bác sĩ yêu cầu.
                            if (!String.IsNullOrWhiteSpace(reqName))
                            {
                                lstMaBacSi.Add(reqName);
                            }

                            foreach (var item in dataEkip)
                            {
                                string cchn = GetMaBacSi(item.LOGINNAME, employees);
                                if (!String.IsNullOrWhiteSpace(cchn))
                                {
                                    lstMaBacSi.Add(cchn);
                                }
                            }
                        }
                    }
                    else if (listHeinServiceTypeMaterial.Contains(hisSereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
                    {
                        lstMaBacSi = new List<string>();
                        if (!String.IsNullOrWhiteSpace(reqName))
                        {
                            lstMaBacSi.Add(reqName);
                        }
                        else if (!String.IsNullOrWhiteSpace(executeName))
                        {
                            lstMaBacSi.Add(executeName);
                        }
                    }
                    else if (MaBacSiOption == "2")
                    {
                        //key giá trị 2 add cả 2
                        lstMaBacSi = new List<string>();
                    }
                    else if ((MaBacSiOption == "3" || MaBacSiOption == "5") &&
                        (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH
                        && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN
                        && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L
                        && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT
                        && hisSereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT))
                    {
                        // key giá trị 3 thì ngoài thuốc, vật tư, giường. tất cả sẽ add 2 cái nếu có
                        lstMaBacSi = new List<string>();
                    }
                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH && hisSereServ.TDL_IS_MAIN_EXAM != 1 && (MaBacSiOption == "1" || MaBacSiOption == "3" || MaBacSiOption == "5"))
                    {
                        //ko phải khám chính thì add cả 2 nếu có cấu hình
                        lstMaBacSi = new List<string>();
                    }
                }
                if (lstMaBacSi.Count == 0)
                {
                    if (!String.IsNullOrWhiteSpace(reqName))
                    {
                        lstMaBacSi.Add(reqName);
                    }
                    if (!String.IsNullOrWhiteSpace(samplerLoginName) && (MaBacSiOption == "1" || MaBacSiOption == "2" || MaBacSiOption == "3" || MaBacSiOption == "5"))
                    {
                        lstMaBacSi.Add(samplerLoginName);    //Người lấy mẫu
                    }

                    if (!String.IsNullOrWhiteSpace(executeName) || resultName.Count > 0)
                    {
                        if (resultName.Count > 0)
                        {
                            lstMaBacSi.AddRange(resultName);
                        }
                        else
                        {
                            lstMaBacSi.Add(executeName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return lstMaBacSi;
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
