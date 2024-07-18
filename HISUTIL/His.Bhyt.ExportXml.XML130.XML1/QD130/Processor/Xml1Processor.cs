using His.Bhyt.ExportXml.XML130.XML1.ADO;
using His.Bhyt.ExportXml.XML130.XML1.Base;
using His.Bhyt.ExportXml.XML130.XML1.QD130.ADO;
using His.Bhyt.ExportXml.XML130.XML1.QD130.XML;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML1.Processor
{
    class Xml1Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXml1Data(InputADO data)
        {
            ResultADO result = null;
            try
            {
                V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter = null;
                if (data.PatientTypeAlter != null && data.PatientTypeAlter.Count > 0)
                {
                    PatientTypeAlter = data.PatientTypeAlter.OrderByDescending(o => o.ID).ToList()[0];
                }
                HIS_DHST dhst = null;
                if (data.Dhst != null && data.Dhst.Count > 0)
                {
                    dhst = data.Dhst.Where(o => o.WEIGHT != null && o.WEIGHT > 0).Count() > 0 ? data.Dhst.Where(o => o.WEIGHT != null && o.WEIGHT > 0).OrderByDescending(o => o.EXECUTE_TIME).ToList()[0] : null;
                }
                XML1Data xml1 = new XML1Data();
                xml1.MA_LK = data.vTreatment.TREATMENT_CODE ?? "";
                xml1.STT = 1;
                xml1.MA_BN = data.vTreatment.TDL_PATIENT_CODE ?? "";
                xml1.HO_TEN = this.ConvertStringToXmlDocument(data.vTreatment.TDL_PATIENT_NAME);
                xml1.SO_CCCD = !string.IsNullOrEmpty(data.vTreatment.TDL_PATIENT_CCCD_NUMBER) ? data.vTreatment.TDL_PATIENT_CCCD_NUMBER : "";
                xml1.NGAY_SINH = data.vTreatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == 1 ? data.vTreatment.TDL_PATIENT_DOB.ToString().Substring(0, 4) + "00000000" : data.vTreatment.TDL_PATIENT_DOB.ToString().Substring(0, 12);
                xml1.GIOI_TINH = data.vTreatment.TDL_PATIENT_GENDER_ID == 1 ? "2" : data.vTreatment.TDL_PATIENT_GENDER_ID == 2 ? "1" : "3";
                xml1.NHOM_MAU = data.vTreatment.TDL_PATIENT_BLOOD_ABO_CODE ?? "";
                if (string.IsNullOrWhiteSpace(xml1.MA_QUOCTICH)||xml1.MA_QUOCTICH.Length < 3)
                {
                    xml1.MA_QUOCTICH = "000";
                }
                xml1.MA_DANTOC = data.vTreatment.ETHNIC_CODE ?? "";
                xml1.MA_NGHE_NGHIEP = data.vTreatment.CAREER_CODE ?? "";
                if (string.IsNullOrWhiteSpace(xml1.MA_NGHE_NGHIEP) || xml1.MA_NGHE_NGHIEP.Length < 5)
                {
                    xml1.MA_NGHE_NGHIEP = "00000";
                }
                xml1.DIA_CHI = this.ConvertStringToXmlDocument(data.vTreatment.TDL_PATIENT_ADDRESS ?? "");

                //cap nhat thx
                if (data.vTreatment != null && His.Bhyt.ExportXml.XML130.XML1.ADO.THX_ADO.Get_THX().FirstOrDefault(o => o.MA_PHUONG_XA == (data.vTreatment.TDL_PATIENT_COMMUNE_CODE ?? "")
                    && o.MA_QUAN_HUYEN == (data.vTreatment.TDL_PATIENT_DISTRICT_CODE ?? "")
                    && o.MA_THANH_PHO == (data.vTreatment.TDL_PATIENT_PROVINCE_CODE ?? "")) == null)
                {
                    UpdateTHX(data.vTreatment, His.Bhyt.ExportXml.XML130.XML1.ADO.THX_ADO.Get_THX(), data.vTreatment.TDL_PATIENT_ADDRESS);
                    if (string.IsNullOrWhiteSpace(data.vTreatment.TDL_PATIENT_COMMUNE_CODE))
                    {
                        UpdateTHX(data.vTreatment, His.Bhyt.ExportXml.XML130.XML1.ADO.THX_ADO.Get_THX(), PatientTypeAlter.ADDRESS);
                        xml1.DIA_CHI = this.ConvertStringToXmlDocument(PatientTypeAlter.ADDRESS ?? "");
                    }
                }
                xml1.MATINH_CU_TRU = data.vTreatment.TDL_PATIENT_PROVINCE_CODE ?? "";
                xml1.MAHUYEN_CU_TRU = data.vTreatment.TDL_PATIENT_DISTRICT_CODE ?? "";
                xml1.MAXA_CU_TRU = data.vTreatment.TDL_PATIENT_COMMUNE_CODE ?? "";
                xml1.DIEN_THOAI = data.vTreatment.TDL_PATIENT_MOBILE ?? "";
                xml1.MA_CSKCB = data.vTreatment.HEIN_MEDI_ORG_CODE ?? "";
                string MaDTKCB = "";
                if (PatientTypeAlter != null)
                {
                    xml1.MA_THE_BHYT = PatientTypeAlter.HEIN_CARD_NUMBER ?? "";
                    xml1.MA_DKBD = PatientTypeAlter.HEIN_MEDI_ORG_CODE ?? "";
                    xml1.GT_THE_TU = PatientTypeAlter.HEIN_CARD_FROM_TIME != null ? PatientTypeAlter.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8) : "";
                    xml1.GT_THE_DEN = PatientTypeAlter.HEIN_CARD_TO_TIME != null ? PatientTypeAlter.HEIN_CARD_TO_TIME.ToString().Substring(0, 8) : "";
                    xml1.NGAY_MIEN_CCT = PatientTypeAlter.FREE_CO_PAID_TIME != null ? PatientTypeAlter.FREE_CO_PAID_TIME.ToString().Substring(0, 8) : "";
                    xml1.MA_KHUVUC = PatientTypeAlter.LIVE_AREA_CODE ?? "";
                    xml1.NAM_NAM_LIEN_TUC = PatientTypeAlter.JOIN_5_YEAR_TIME != null ? PatientTypeAlter.JOIN_5_YEAR_TIME.ToString().Substring(0, 8) : "";
                    if (!string.IsNullOrEmpty(xml1.MA_THE_BHYT))
                    {
                        string branchHeinMediOrgCode = (data.vTreatment.HEIN_MEDI_ORG_CODE ?? "").Trim();
                        string ProvinceCode = !string.IsNullOrEmpty(PatientTypeAlter.HEIN_MEDI_ORG_CODE) ? PatientTypeAlter.HEIN_MEDI_ORG_CODE.Substring(0, 2) : "";
                        if (data.ListHeinMediOrg != null && data.ListHeinMediOrg.Count > 0)
                        {
                            GlobalConfigStore.HisHeinMediOrg = data.ListHeinMediOrg;
                        }
                        string levelCodeHeinMediOrg = !string.IsNullOrEmpty(PatientTypeAlter.HEIN_MEDI_ORG_CODE) ? GlobalConfigStore.HisHeinMediOrg.FirstOrDefault(o => o.MEDI_ORG_CODE == PatientTypeAlter.HEIN_MEDI_ORG_CODE) != null ? GlobalConfigStore.HisHeinMediOrg.FirstOrDefault(o => o.MEDI_ORG_CODE == PatientTypeAlter.HEIN_MEDI_ORG_CODE).LEVEL_CODE : "" : "";
                        string branchProvinceCode = branchHeinMediOrgCode.Length > 2 ? branchHeinMediOrgCode.Substring(0, 2) : null;
                        if (PatientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE && PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        {
                            MaDTKCB = "2";
                        }
                        else if (PatientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                        {
                            if (PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.PRESENT)
                            {
                                MaDTKCB = "1.3";
                            }
                            else if (PatientTypeAlter.HAS_ABSENT_LETTER == 1 || PatientTypeAlter.HAS_WORKING_LETTER == 1)
                            {
                                MaDTKCB = "1.4";
                            }
                            else if (PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.APPOINTMENT)
                            {
                                MaDTKCB = "1.5";
                            }
                            else if ((!String.IsNullOrEmpty(data.vTreatment.ICD_CODE) && data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(data.vTreatment.ICD_CODE)) != null && data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(data.vTreatment.ICD_CODE)).IS_LATENT_TUBERCULOSIS == 1) || (!String.IsNullOrEmpty(data.vTreatment.ICD_SUB_CODE) && data.TotalIcdData.FirstOrDefault(o => (";" + data.vTreatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")) != null && data.TotalIcdData.FirstOrDefault(o => (";" + data.vTreatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")).IS_LATENT_TUBERCULOSIS == 1) || data.vTreatment.IS_TUBERCULOSIS == 1)
                            {
                                MaDTKCB = "1.8";
                            }
                            else if (data.vTreatment.IS_HIV == 1)
                            {
                                MaDTKCB = "1.9";
                            }
                            else if ((!String.IsNullOrEmpty(data.vTreatment.ICD_CODE) && data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(data.vTreatment.ICD_CODE)) != null && data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(data.vTreatment.ICD_CODE)).IS_COVID == 1) || (!String.IsNullOrEmpty(data.vTreatment.ICD_SUB_CODE) && data.TotalIcdData.FirstOrDefault(o => (";" + data.vTreatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")) != null && data.TotalIcdData.FirstOrDefault(o => (";" + data.vTreatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")).IS_COVID == 1))
                            {
                                MaDTKCB = "1.10";
                            }
                            else if (PatientTypeAlter.HEIN_MEDI_ORG_CODE == branchHeinMediOrgCode
                            || ValidAccept(PatientTypeAlter.HEIN_MEDI_ORG_CODE, data.vTreatment.ACCEPT_HEIN_MEDI_ORG_CODE)
                            || ValidAccept(PatientTypeAlter.HEIN_MEDI_ORG_CODE, data.vTreatment.SYS_MEDI_ORG_CODE))
                            {
                                MaDTKCB = "1.1";
                            }
                            else if (PatientTypeAlter.HAS_BIRTH_CERTIFICATE == MOS.LibraryHein.Bhyt.HeinHasBirthCertificate.HeinHasBirthCertificateCode.TRUE || xml1.MA_THE_BHYT.StartsWith("TE") || ((MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT == data.vTreatment.HEIN_LEVEL_CODE || MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE == data.vTreatment.HEIN_LEVEL_CODE) && ProvinceCode == branchProvinceCode && (levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT || levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE)))
                            {
                                MaDTKCB = "1.2";
                            }
                            else if (PatientTypeAlter.HEIN_CARD_NUMBER.StartsWith("HN") || PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K1 || PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K2 || PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K3)
                            {
                                MaDTKCB = "3.6";
                            }
                            
                        }
                        else if (PatientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                        {
                            if (data.vTreatment.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL && data.vTreatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                MaDTKCB = "3.1";
                            }
                            else if (data.vTreatment.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE && data.vTreatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                MaDTKCB = "3.2";
                            }
                            else if (data.vTreatment.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT && (ProvinceCode != branchProvinceCode || (levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL || levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE)))
                            {
                                MaDTKCB = "3.3";
                            }
                            else if (data.vTreatment.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL && data.vTreatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                MaDTKCB = "3.4";
                            }
                            else if (data.vTreatment.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE && data.vTreatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                MaDTKCB = "3.5";
                            }
                            else if (PatientTypeAlter.HEIN_CARD_NUMBER.StartsWith("HN") || PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K1 || PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K2 || PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K3)
                            {
                                MaDTKCB = "3.6";
                            }
                            else
                            {
                                MaDTKCB = "3.7";
                            }
                        }
                        else if (data.vTreatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__NHANTHUOC)
                        {
                            MaDTKCB = "7.1";
                        }
                    }
                    else
                    {
                        MaDTKCB = "9";
                    }
                }
                else
                {
                    xml1.MA_THE_BHYT = "";
                    xml1.MA_DKBD = "";
                    xml1.GT_THE_TU = "";
                    xml1.GT_THE_DEN = "";
                    xml1.NGAY_MIEN_CCT = "";
                    xml1.MA_KHUVUC = "";
                    xml1.NAM_NAM_LIEN_TUC = "";
                }
                xml1.MA_KHOA = data.vTreatment.END_ROOM_BHYT_CODE ?? data.vTreatment.EXIT_BHYT_CODE ?? data.vTreatment.END_DEPARTMENT_BHYT_CODE ?? "";
                xml1.MA_DOITUONG_KCB = MaDTKCB;
                var sss = data.vSereServ.FirstOrDefault(o=>!string.IsNullOrWhiteSpace(o.HOSPITALIZATION_REASON))??new V_HIS_SERE_SERV_2();
                xml1.LY_DO_VV = this.ConvertStringToXmlDocument(data.vTreatment.HOSPITALIZATION_REASON ?? sss.HOSPITALIZATION_REASON?? "");
                xml1.LY_DO_VNT = this.ConvertStringToXmlDocument(data.vTreatment.HOSPITALIZE_REASON_NAME ?? "");
                xml1.MA_LY_DO_VNT = data.vTreatment.HOSPITALIZE_REASON_CODE ?? "";
                xml1.CHAN_DOAN_VAO = this.ConvertStringToXmlDocument(data.vTreatment.PROVISIONAL_DIAGNOSIS ?? data.vTreatment.ICD_NAME ?? "");
                xml1.CHAN_DOAN_RV = this.ConvertStringToXmlDocument(data.vTreatment.ICD_NAME + ";" + data.vTreatment.ICD_TEXT);
                xml1.MA_BENH_CHINH = data.vTreatment.ICD_CODE ?? "";
                xml1.MA_BENH_KT = "";
                if (!string.IsNullOrEmpty(data.vTreatment.ICD_SUB_CODE))
                {
                    List<string> icdSubCode = new List<string>();
                    var ArrIcdSubCode = data.vTreatment.ICD_SUB_CODE.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ArrIcdSubCode != null && ArrIcdSubCode.Length > 12)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            icdSubCode.Add(ArrIcdSubCode[i]);
                        }
                    }
                    else if (ArrIcdSubCode != null)
                    {
                        icdSubCode = ArrIcdSubCode.ToList();
                    }
                    if (icdSubCode != null && icdSubCode.Count > 0)
                        xml1.MA_BENH_KT = string.Join(";", icdSubCode);
                }
                xml1.MA_BENH_YHCT = "";
                List<string> lstTotalIcdYhct = new List<string>();
                if (!string.IsNullOrEmpty(data.vTreatment.TRADITIONAL_ICD_CODE))
                {
                    lstTotalIcdYhct.AddRange(data.vTreatment.TRADITIONAL_ICD_CODE.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                }
                if (!string.IsNullOrEmpty(data.vTreatment.TRADITIONAL_ICD_SUB_CODE))
                {
                    lstTotalIcdYhct.AddRange(data.vTreatment.TRADITIONAL_ICD_SUB_CODE.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                }
                if (lstTotalIcdYhct != null && lstTotalIcdYhct.Count > 0)
                {
                    lstTotalIcdYhct = lstTotalIcdYhct.Where(o => !o.Equals(data.vTreatment.ICD_CODE)).ToList().Distinct().ToList();
                    if (lstTotalIcdYhct != null && lstTotalIcdYhct.Count > 0)
                        xml1.MA_BENH_YHCT = string.Join(";", lstTotalIcdYhct);
                }
                xml1.MA_PTTT_QT = "";
                if (data.vSereServPTTT != null && data.vSereServPTTT.Count > 0)
                {
                    List<string> lstTotalIcdPttt = new List<string>();
                    foreach (var item in data.vSereServPTTT)
                    {
                        if (!string.IsNullOrEmpty(item.ICD_CM_CODE))
                        {
                            lstTotalIcdPttt.AddRange(item.ICD_CM_CODE.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                        }
                        if (!string.IsNullOrEmpty(item.ICD_CM_SUB_CODE))
                        {
                            lstTotalIcdPttt.AddRange(item.ICD_CM_SUB_CODE.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                        }
                    }
                    lstTotalIcdPttt = lstTotalIcdPttt.Distinct().ToList();
                    if (lstTotalIcdPttt != null && lstTotalIcdPttt.Count > 0)
                    {
                        xml1.MA_PTTT_QT = string.Join(";", lstTotalIcdPttt);
                    }
                }
                xml1.MA_NOI_DI = data.vTreatment.TRANSFER_IN_MEDI_ORG_CODE ?? "";
                xml1.MA_NOI_DEN = data.vTreatment.MEDI_ORG_CODE ?? "";
                xml1.MA_TAI_NAN = data.vTreatment.ACCIDENT_HURT_TYPE_BHYT_CODE ?? "0";
                xml1.NGAY_VAO = data.vTreatment.IN_TIME.ToString().Substring(0, 12);
                xml1.NGAY_VAO_NOI_TRU = data.vTreatment.CLINICAL_IN_TIME != null ? data.vTreatment.CLINICAL_IN_TIME.ToString().Substring(0, 12) : "";
                xml1.NGAY_RA = data.vTreatment.OUT_TIME != null ? data.vTreatment.OUT_TIME.ToString().Substring(0, 12) : "";
                xml1.GIAY_CHUYEN_TUYEN = data.vTreatment.TRANSFER_IN_CODE ?? "";
                string ppDieuTri = data.vTreatment.TREATMENT_METHOD ?? "";
                if (!string.IsNullOrEmpty(ppDieuTri) && Encoding.UTF8.GetByteCount(ppDieuTri) > 2000)
                {
                    ppDieuTri = SubStringWithSeparate(ppDieuTri, 2000);
                }
                xml1.PP_DIEU_TRI = ppDieuTri;
                xml1.KET_QUA_DTRI = !string.IsNullOrEmpty(data.vTreatment.TREATMENT_RESULT_CODE) ? data.vTreatment.TREATMENT_RESULT_CODE.Reverse().ToList()[0].ToString() : "";
                xml1.MA_LOAI_RV = "1";
                switch (data.vTreatment.TREATMENT_END_TYPE_ID)
                {
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON:
                        xml1.MA_LOAI_RV = "3";
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN:
                        xml1.MA_LOAI_RV = "4";
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN:
                        switch (data.vTreatment.TRAN_PATI_REASON_ID)
                        {
                            case 1:
                                xml1.MA_LOAI_RV = "2";
                                break;
                            case 2:
                                xml1.MA_LOAI_RV = "5";
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                xml1.GHI_CHU = this.ConvertStringToXmlDocument(data.vTreatment.ADVISE ?? "");
                xml1.NGAY_TTOAN = data.vTreatment.FEE_LOCK_TIME != null ? data.vTreatment.FEE_LOCK_TIME.ToString().Substring(0, 12) : "";
                xml1.T_THUOC = Math.Round(data.tienThuoc, 2).ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_VTYT = Math.Round(data.tienVTYT, 2).ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_TONGCHI_BV = data.tongchiBV.ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_TONGCHI_BH = data.tongchiBH.ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_BNTT = data.tongBNTT.ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_BHTT = data.tongBHTT.ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_BNCCT = data.tongBNCCT.ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_NGUONKHAC = data.tongNguonKhac.ToString("G27", CultureInfo.InvariantCulture);

                if (xml1.MA_THE_BHYT.StartsWith("CA") || xml1.MA_THE_BHYT.StartsWith("QN") || xml1.MA_THE_BHYT.StartsWith("CY"))
                {
                    xml1.T_BHTT_GDV = "0";
                }
                else
                {
                    xml1.T_BHTT_GDV = data.tongBHTTGDV.ToString("G27", CultureInfo.InvariantCulture);
                }
                
                xml1.NAM_QT = data.vTreatment.HEIN_LOCK_TIME != null ? data.vTreatment.HEIN_LOCK_TIME.ToString().Substring(0, 4) : (data.vTreatment.OUT_TIME != null ? data.vTreatment.OUT_TIME.ToString().Substring(0, 4) : "");
                xml1.THANG_QT = data.vTreatment.HEIN_LOCK_TIME != null ? data.vTreatment.HEIN_LOCK_TIME.ToString().Substring(4, 2) : (data.vTreatment.OUT_TIME != null ? data.vTreatment.OUT_TIME.ToString().Substring(4, 2) : "");
                string MaLoaiKCB = "10";
                switch (data.vTreatment.TDL_TREATMENT_TYPE_ID)
                {
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM:
                        MaLoaiKCB = "01";
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU:
                        if (data.vTreatment.OUT_TIME.HasValue && data.vTreatment.CLINICAL_IN_TIME.HasValue && (data.vTreatment.OUT_TIME.Value - data.vTreatment.CLINICAL_IN_TIME.Value) > 0 && Inventec.Common.DateTime.Calculation.DifferenceTime(data.vTreatment.CLINICAL_IN_TIME.Value, data.vTreatment.OUT_TIME.Value, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR) < 4)
                            MaLoaiKCB = "09";
                        else
                            MaLoaiKCB = "03";
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY:
                        MaLoaiKCB = "04";
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__TYTXA:
                        MaLoaiKCB = "06";
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__NHANTHUOC:
                        MaLoaiKCB = "07";
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU:
                        if (data.vTreatment.IS_CHRONIC != 1)
                            MaLoaiKCB = "02";
                        else if (data.vSereServ != null && data.vSereServ.Count > 0)
                        {
                            if (data.vSereServ.Exists(o => o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH && o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT && o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT && o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK))
                            {
                                MaLoaiKCB = "08";
                            }
                            else
                            {
                                MaLoaiKCB = "05";
                            }
                        }
                        break;
                    default:
                        break;
                }
                string cannang = "";
                if (dhst != null && dhst.WEIGHT.HasValue)
                {
                    cannang = Math.Round(dhst.WEIGHT.Value, 2).ToString("G27", CultureInfo.InvariantCulture);
                }
                xml1.CAN_NANG = cannang;
                if (data.vBaby != null && data.vBaby.Count > 0)
                {
                    List<string> lstWe = new List<string>();
                    foreach (var item in data.vBaby)
                    {
                        if (item.WEIGHT.HasValue)
                            lstWe.Add(Math.Round(item.WEIGHT.Value, 2).ToString("G27", CultureInfo.InvariantCulture));
                    }
                    xml1.CAN_NANG_CON = string.Join(";", lstWe);
                }
                else
                    xml1.CAN_NANG_CON = "";
                xml1.MA_LOAI_KCB = MaLoaiKCB ?? "";

                if (xml1.MA_LOAI_KCB == "01" || xml1.MA_LOAI_KCB == "07" || xml1.MA_LOAI_KCB == "09")
                {
                    xml1.SO_NGAY_DTRI = 0;
                }
                if (xml1.MA_LOAI_KCB == "02" || xml1.MA_LOAI_KCB == "03" || xml1.MA_LOAI_KCB == "04" || xml1.MA_LOAI_KCB == "06")
                {
                    long dayOfTreatment = 0;
                    if (data.vTreatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || data.vTreatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || data.vTreatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY || data.vTreatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__TYTXA)
                    {
                        var outTime = Int64.Parse(!string.IsNullOrEmpty(xml1.NGAY_RA) ? xml1.NGAY_RA.Substring(0, 8) + "000000" : "00000000000000");
                        var inTime = Int64.Parse(xml1.NGAY_VAO.Substring(0, 8) + "000000");
                        DateTime otD = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(outTime) ?? DateTime.Now;
                        DateTime itD = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(inTime) ?? DateTime.Now;
                        var timeSpan = otD - itD;
                        dayOfTreatment = (int)timeSpan.TotalDays + 1;
                    }
                    xml1.SO_NGAY_DTRI = dayOfTreatment;
                }
                if (xml1.MA_LOAI_KCB == "05")
                {
                    long dayOfTreatment = 0;
                    if (data.vSereServ != null && data.vSereServ.Count > 0)
                    {
                         var ssFinishs = data.vSereServ.Where(a => a.FINISH_TIME.HasValue).ToList();
                         if (ssFinishs != null && ssFinishs.Count() > 0)
                         {
                             List<long> soNgaySuDungThuoc = new List<long>();
                             foreach (var ss in ssFinishs)
                             {
                                 if (ss.USE_TIME_TO.HasValue)
                                 {
                                     DateTime UseTimeToD = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ss.USE_TIME_TO.Value) ?? DateTime.Now;
                                     DateTime InstructionTimeD = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ss.INTRUCTION_TIME) ?? DateTime.Now;
                                     
                                     var timeSpan = UseTimeToD - InstructionTimeD;
                                     long day = (int)timeSpan.TotalDays + 1;
                                     soNgaySuDungThuoc.Add(day);
                                 }
                             }
                             if (soNgaySuDungThuoc != null && soNgaySuDungThuoc.Count > 0)
                             {
                                 dayOfTreatment = soNgaySuDungThuoc.Max();
                             }
                         }
                    }
                    xml1.SO_NGAY_DTRI = dayOfTreatment;
                }
                if (xml1.MA_LOAI_KCB == "08")
                {
                    if (data.vSereServPTTT != null && data.vSereServPTTT.Count > 0)
                    {
                        var ssPTTT = data.vSereServPTTT.Where(o => o.TDL_TREATMENT_ID == data.vTreatment.ID).ToList();
                        if (ssPTTT != null && ssPTTT.Count > 0)
                        {
                            long dayOfTreatment = 0;
                            if (data.vSereServ != null && data.vSereServ.Count > 0)
                            {
                                var ssFinishs = data.vSereServ.Where(a => a.FINISH_TIME.HasValue).OrderByDescending(o => o.INTRUCTION_TIME).ToList();
                                if (ssFinishs != null && ssFinishs.Count() > 0)
                                {
                                    long? lastFinishTime = ssFinishs.Select(s => s.FINISH_TIME).FirstOrDefault();
                                    var inTime = Int64.Parse(xml1.NGAY_VAO.Substring(0, 8) + "000000");
                                    DateTime InstructionTimeD = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(lastFinishTime.Value) ?? DateTime.Now;
                                    DateTime itD = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(inTime) ?? DateTime.Now;
                                    var timeSpan = InstructionTimeD - itD;
                                    dayOfTreatment = (int)timeSpan.TotalDays + 1;
                                }
                            }

                            xml1.SO_NGAY_DTRI = dayOfTreatment;
                        }
                        else
                        {
                            xml1.SO_NGAY_DTRI = 1;
                        }
                    }
                    else
                    {
                        xml1.SO_NGAY_DTRI = 1;
                    }
                }
                xml1.NGAY_TAI_KHAM = data.vTreatment.APPOINTMENT_TIME != null ? data.vTreatment.APPOINTMENT_TIME.ToString().Substring(0, 8) : "";
                xml1.MA_HSBA = data.vTreatment.TREATMENT_CODE ?? "";
                xml1.MA_TTDV = data.vTreatment.REPRESENTATIVE_HEIN_CODE ?? "";
                xml1.DU_PHONG = "";
                result = new ResultADO(true, "", new object[] { xml1 });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new ResultADO(false, ex.Message, null);
            }
            return result;
        }
        private string SubStringWithSeparate(string multiCharString, decimal limit)
        {
            string result = "";
            try
            {
                Encoding utf8 = Encoding.UTF8;
                int leng = utf8.GetByteCount(multiCharString);
                if (leng > limit)
                {
                    int index = multiCharString.LastIndexOf(";");
                    while (utf8.GetByteCount(multiCharString) > limit)
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
        //., Thị trấn Anh Sơn, Huyện Anh Sơn, Nghệ An
        private void UpdateTHX(V_HIS_TREATMENT_12 treatment, List<THX_ADO> listAdoThx, string DiaChiThe)
        {
            try
            {
                string maTinh = "";
                string maHuyen = "";
                string maXa = "";
                List<string> array = DiaChiThe.ToString().Split(',').ToList();
                foreach (THX_ADO drHC in listAdoThx)
                {
                    foreach (string t in array)
                    {
                        if (drHC.TEN_THANH_PHO.ToUpper().Trim().Contains(t.ToUpper().Trim()))
                        {
                            maTinh = drHC.MA_THANH_PHO.ToString();
                            foreach (string h in array.Where(o => o != t).ToList())
                            {

                                if (drHC.TEN_QUAN_HUYEN.ToUpper().Trim().Contains(h.ToString().ToUpper().Trim()))
                                {
                                    maHuyen = drHC.MA_QUAN_HUYEN.ToString();
                                    foreach (string x in array.Where(o => o != t && o != h).ToList())
                                    {
                                        if (Inventec.Common.String.Convert.UnSignVNese2(drHC.TEN_PHUONG_XA.ToUpper().Trim()).Contains(Inventec.Common.String.Convert.UnSignVNese2(x.ToString().ToUpper().Trim())))
                                        {
                                            maXa = drHC.MA_PHUONG_XA.ToString();
                                            goto exit;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                exit:;
                treatment.TDL_PATIENT_PROVINCE_CODE = maTinh;
                treatment.TDL_PATIENT_DISTRICT_CODE = maHuyen;
                treatment.TDL_PATIENT_COMMUNE_CODE = maXa;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        
        private bool ValidAccept(string mediOrgCode, string AcceptCode)
        {
            bool valid = false;
            try
            {
                if (String.IsNullOrWhiteSpace(mediOrgCode) || String.IsNullOrWhiteSpace(AcceptCode))
                    return false;
                string[] listCode = AcceptCode.Split(',', ';');
                foreach (var code in listCode)
                {
                    if (code != null && code.Trim() == mediOrgCode.Trim())
                        return true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return valid;
        }
    }
}
