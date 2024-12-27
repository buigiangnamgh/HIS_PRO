using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.CheckIn.XML;
using His.Bhyt.ExportXml;
using Inventec.Common.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.CheckIn.Processor
{
    class CheckInProcessor : XmlProcessorBase
    {
        private Dictionary<string, LoginResultLDO> dicLoginResultByUserName = new Dictionary<string, LoginResultLDO>();
        private LoginResultLDO plv = null;
        InputADO Data { get; set; }
        string XmlString { get; set; }
        internal CheckInProcessor(InputADO data)
        {
            this.Data = data;
        }
        internal CheckInProcessor(string XmlString)
        {
            this.XmlString = XmlString;
        }
        internal string ProcessorPath()
        {
            string result = "";
            try
            {
                ResultADO checkIn = this.CreateCheckIn();
                if (checkIn == null || !checkIn.Success || checkIn.Data == null || checkIn.Data.Length == 0)
                    return "";
                var fileName = ProcessFileName();
                var path = string.Format("{0}/{1}.xml", GlobalConfigStore.PathSaveXml, fileName);
                var rs = this.CreatedXmlFileEncoding(checkIn.Data[0] as DataCheckIn, false, true, true, path);
                if (rs == null || !rs.Success)
                    return "";
                result = path;
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal MemoryStream ProcessorPlus(ref string fileName)
        {
            MemoryStream result = null;
            try
            {
                ResultADO checkIn = this.CreateCheckIn();
                if (checkIn == null || !checkIn.Success || checkIn.Data == null || checkIn.Data.Length == 0)
                    return null;
                fileName = ProcessFileName();
                var path = string.Format("{0}/{1}", GlobalConfigStore.PathSaveXml, fileName);
                result = this.CreatedXmlFileEncodingPlus(checkIn.Data[0] as DataCheckIn, false, true, true, path);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        internal MemoryStream ProcessorQd130CheckIn(ref string fileName)
        {
            MemoryStream result = null;
            try
            {
                ResultADO checkIn = this.CreateQd130CheckIn();
                if (checkIn == null || !checkIn.Success || checkIn.Data == null || checkIn.Data.Length == 0)
                    return null;
                fileName = ProcessFileName();
                var path = string.Format("{0}/{1}", GlobalConfigStore.PathSaveXml, fileName);
                result = this.CreatedXmlFileEncodingPlus(checkIn.Data[0] as DataQd130CheckIn, false, true, true, path);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        private string ProcessFileName()
        {
            string result = "";
            try
            {
                List<string> data = new List<string>();
                data.Add(this.Data.Treatment.IN_TIME.ToString().Substring(0, 12));
                data.Add(this.Data.Treatment.TDL_HEIN_CARD_NUMBER ?? this.Data.Treatment.TREATMENT_CODE);
                data.Add("CheckIn.xml");
                result = string.Join("_", data);
            }
            catch (Exception ex)
            {
                result = DateTime.Now.ToString("yyyyMMddHHmm") + "__CheckIn";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        private ResultADO CreateCheckIn()
        {
            ResultADO rs = null;
            try
            {
                ResultADO header = this.CreateThongHeader();
                if (header == null || !header.Success || header.Data == null || header.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Thong Tin header That Bai", null);
                ResultADO body = this.CreateThongBody();
                if (body == null || !body.Success || body.Data == null || body.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Thong Tin body That Bai", null);
                DataCheckIn checkIn = new DataCheckIn();
                checkIn.HEADER = header.Data[0] as Header;
                checkIn.BODY = body.Data[0] as Body;
                checkIn.SECURITY = new Security { SIGNATURE = "" };
                rs = new ResultADO(true, "", new object[] { checkIn });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }
        private ResultADO CreateQd130CheckIn()
        {
            ResultADO rs = null;
            try
            {
                ResultADO body = this.CreateThongQd130Body();
                if (body == null || !body.Success || body.Data == null || body.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Thong Tin body That Bai", null);
                DataQd130CheckIn checkIn = new DataQd130CheckIn();
                checkIn.DSACH_TRANG_THAI_KCB = body.Data[0] as Qd130CheckInRootList;
                checkIn.CHUKYDONVI = "";
                rs = new ResultADO(true, "", new object[] { checkIn });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }
        private ResultADO CreateThongQd130Body()
        {
            ResultADO rs = null;
            try
            {
                var checkIn = this.CreateQd130CheckInData();
                if (checkIn == null || !checkIn.Success || checkIn.Data == null || checkIn.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Danh Sach Ho So That Bai", null);
                Qd130CheckInRootList  body = checkIn.Data[0] as Qd130CheckInRootList;
                rs = new ResultADO(true, "", new object[] { body });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateQd130CheckInData()
        {
            ResultADO rs = null;
            try
            {
                Qd130CheckInRootList qd130CheckInRootList = new Qd130CheckInRootList();
                List<Qd130CheckIn> lst = new List<Qd130CheckIn>();
                Qd130CheckIn checkIn = new Qd130CheckIn();
                checkIn.MA_LK = this.Data.Treatment.TREATMENT_CODE ?? "";
                checkIn.STT = 1;
                checkIn.MA_BN = this.Data.Treatment.TDL_PATIENT_CODE ?? "";
                checkIn.HO_TEN = this.ConvertStringToXmlDocument(this.Data.Treatment.TDL_PATIENT_NAME ?? "");
                checkIn.SO_CCCD = !string.IsNullOrEmpty(this.Data.Treatment.TDL_PATIENT_CCCD_NUMBER) ? this.Data.Treatment.TDL_PATIENT_CCCD_NUMBER : "";
                checkIn.NGAY_SINH = this.Data.Treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == 1 ? this.Data.Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4) + "00000000" : this.Data.Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 12);
                checkIn.GIOI_TINH = Data.Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE ? 2 : 1;
                string MaDTKCB = "";
                if (Data.PatientTypeAlter != null)
                {
                    checkIn.MA_THE_BHYT = this.Data.PatientTypeAlter.HEIN_CARD_NUMBER ?? "";
                    checkIn.MA_DKBD = this.Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE ?? "";
                    checkIn.GT_THE_TU = this.Data.PatientTypeAlter.HEIN_CARD_FROM_TIME.HasValue ? this.Data.PatientTypeAlter.HEIN_CARD_FROM_TIME.Value.ToString().Substring(0, 8) : "";
                    checkIn.GT_THE_DEN = this.Data.PatientTypeAlter.HEIN_CARD_TO_TIME.HasValue ? this.Data.PatientTypeAlter.HEIN_CARD_TO_TIME.Value.ToString().Substring(0, 8) : "";
                    if (!string.IsNullOrEmpty(checkIn.MA_THE_BHYT))
                    {
                        string branchHeinMediOrgCode = (Data.Branch.HEIN_MEDI_ORG_CODE ?? "").Trim();
                        string ProvinceCode = !string.IsNullOrEmpty(Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE) ? Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE.Substring(0, 2) : "";
                        if (Data.ListHeinMediOrg != null && Data.ListHeinMediOrg.Count > 0)
                        {
                            GlobalConfigStore.HisHeinMediOrg = Data.ListHeinMediOrg;
                        }
                        string levelCodeHeinMediOrg = !string.IsNullOrEmpty(Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE) ? GlobalConfigStore.HisHeinMediOrg.FirstOrDefault(o => o.MEDI_ORG_CODE == Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE).LEVEL_CODE : "";
                        string branchProvinceCode = branchHeinMediOrgCode.Length > 2 ? branchHeinMediOrgCode.Substring(0, 2) : null;
                        if (Data.PatientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE && Data.PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        {
                            MaDTKCB = "2";
                        }
                        else if (Data.PatientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                        {
                            if (Data.PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.PRESENT)
                            {
                                MaDTKCB = "1.3";
                            }
                            else if (Data.PatientTypeAlter.HAS_ABSENT_LETTER == 1 || Data.PatientTypeAlter.HAS_WORKING_LETTER == 1)
                            {
                                MaDTKCB = "1.4";
                            }
                            else if (Data.PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.APPOINTMENT)
                            {
                                MaDTKCB = "1.5";
                            }
                            else if ((!String.IsNullOrEmpty(Data.Treatment.ICD_CODE) && Data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(Data.Treatment.ICD_CODE)) != null && Data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(Data.Treatment.ICD_CODE)).IS_LATENT_TUBERCULOSIS == 1) || (!String.IsNullOrEmpty(Data.Treatment.ICD_SUB_CODE) && Data.TotalIcdData.FirstOrDefault(o => (";" + Data.Treatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")) != null && Data.TotalIcdData.FirstOrDefault(o => (";" + Data.Treatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")).IS_LATENT_TUBERCULOSIS == 1) || Data.Treatment.IS_TUBERCULOSIS == 1)
                            {
                                MaDTKCB = "1.8";
                            }
                            else if (Data.Treatment.IS_HIV == 1)
                            {
                                MaDTKCB = "1.9";
                            }
                            else if ((!String.IsNullOrEmpty(Data.Treatment.ICD_CODE) && Data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(Data.Treatment.ICD_CODE)) != null && Data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(Data.Treatment.ICD_CODE)).IS_COVID == 1) || (!String.IsNullOrEmpty(Data.Treatment.ICD_SUB_CODE) && Data.TotalIcdData.FirstOrDefault(o => (";" + Data.Treatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")) != null && Data.TotalIcdData.FirstOrDefault(o => (";" + Data.Treatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")).IS_COVID == 1))
                            {
                                MaDTKCB = "1.10";
                            }
                            else if (Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE == branchHeinMediOrgCode
                            || ValidAccept(Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE, Data.Branch.ACCEPT_HEIN_MEDI_ORG_CODE)
                            || ValidAccept(Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE, Data.Branch.SYS_MEDI_ORG_CODE))
                            {
                                MaDTKCB = "1.1";
                            }
                            else if (Data.PatientTypeAlter.HAS_BIRTH_CERTIFICATE == MOS.LibraryHein.Bhyt.HeinHasBirthCertificate.HeinHasBirthCertificateCode.TRUE || ((MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT == Data.Branch.HEIN_LEVEL_CODE || MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE == Data.Branch.HEIN_LEVEL_CODE) && ProvinceCode == branchProvinceCode && (levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT || levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE)))
                            {
                                MaDTKCB = "1.2";
                            }
                        }
                        else if (Data.PatientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                        {
                            if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL && this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                MaDTKCB = "3.1";
                            }
                            else if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE && this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                MaDTKCB = "3.2";
                            }
                            else if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT && (ProvinceCode != branchProvinceCode || (levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL || levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE)))
                            {
                                MaDTKCB = "3.3";
                            }
                            else if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL && this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                MaDTKCB = "3.4";
                            }
                            else if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE && this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                MaDTKCB = "3.5";
                            }
                            else if (this.Data.PatientTypeAlter.HEIN_CARD_NUMBER.StartsWith("HN") || this.Data.PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K1 || this.Data.PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K2 || this.Data.PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K3)
                            {
                                MaDTKCB = "3.6";
                            }
                            else
                            {
                                MaDTKCB = "3.7";
                            }
                        }
                        else if (this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__NHANTHUOC)
                        {
                            MaDTKCB = "7.1";
                        }
                    }
                    else
                    {
                        MaDTKCB = "9";
                    }
                }
                checkIn.MA_DOITUONG_KCB = MaDTKCB;
                checkIn.NGAY_VAO = this.Data.Treatment.IN_TIME.ToString().Substring(0, 12);
                checkIn.NGAY_VAO_NOI_TRU = this.Data.Treatment.CLINICAL_IN_TIME.HasValue ? this.Data.Treatment.CLINICAL_IN_TIME.ToString().Substring(0, 12) : "";
                checkIn.LY_DO_VNT = this.ConvertStringToXmlDocument(this.Data.Treatment.HOSPITALIZE_REASON_NAME ?? "");
                checkIn.MA_LY_DO_VNT = this.Data.Treatment.HOSPITALIZE_REASON_CODE ?? "";
                checkIn.DU_PHONG = "";
                string MaLoaiKCB = "10";
                switch (this.Data.Treatment.TDL_TREATMENT_TYPE_ID)
                {
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM:
                        MaLoaiKCB = "01";
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU:
                        if (Data.Treatment.OUT_TIME.HasValue && Data.Treatment.CLINICAL_IN_TIME.HasValue && (Data.Treatment.OUT_TIME.Value - Data.Treatment.CLINICAL_IN_TIME.Value) > 0 && Inventec.Common.DateTime.Calculation.DifferenceTime(Data.Treatment.CLINICAL_IN_TIME.Value, Data.Treatment.OUT_TIME.Value, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR) < 4)
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
                        if (this.Data.Treatment.IS_CHRONIC != 1)
                            MaLoaiKCB = "02";
                        else if (this.Data.ListSereServ != null && this.Data.ListSereServ.Count > 0)
                        {
                            if (this.Data.ListSereServ.Exists(o => o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH && o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT && o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT && o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK))
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
                checkIn.MA_LOAI_KCB = MaLoaiKCB;
                checkIn.MA_CSKCB = "";
                if (Data.Branch != null)
                {
                    checkIn.MA_CSKCB = Data.Branch.HEIN_MEDI_ORG_CODE;
                }
                else
                {
                    checkIn.MA_CSKCB = GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE;
                }
                checkIn.MA_DICH_VU = "";
                checkIn.TEN_DICH_VU = this.ConvertStringToXmlDocument("");
                checkIn.NGAY_YL = "";
                if (this.Data.ListSereServ != null && this.Data.ListSereServ.Count > 0)
                {
                    var SereServ = this.Data.ListSereServ.Where(o => o.IS_NO_EXECUTE != 1 && o.IS_DELETE != 1).ToList();
                    if (SereServ != null && SereServ.Count > 0)
                    {
                        SereServ = SereServ.OrderBy(o => o.SERVICE_REQ_ID).ToList();
                        checkIn.MA_DICH_VU = SereServ[0].TDL_HEIN_SERVICE_BHYT_CODE;
                        checkIn.TEN_DICH_VU = this.ConvertStringToXmlDocument(SereServ[0].TDL_HEIN_SERVICE_BHYT_NAME ?? "");
                        checkIn.NGAY_YL = SereServ[0].TDL_INTRUCTION_TIME.ToString().Substring(0, 12);

                        var SereServMedi = SereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).ToList();
                        if (SereServMedi != null && SereServMedi.Count > 0)
                        {
                            var sMediMate = SereServMedi.OrderBy(o=>o.INTRUCTION_TIME).ThenBy(o=>o.SERVICE_REQ_ID).ToList()[0];
                            if (sMediMate.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM)
                            {
                                checkIn.MA_THUOC = this.ConvertStringToXmlDocument(sMediMate.TDL_ACTIVE_INGR_BHYT_CODE ?? "");
                                checkIn.TEN_THUOC = this.ConvertStringToXmlDocument(sMediMate.TDL_HEIN_SERVICE_BHYT_NAME ?? "");
                            }
                            else
                            {
                                checkIn.MA_THUOC = this.ConvertStringToXmlDocument("00.0000");
                                checkIn.TEN_THUOC = this.ConvertStringToXmlDocument(sMediMate.TDL_SERVICE_NAME ?? "");
                            }
                        }
                        else
                        {
                            checkIn.MA_THUOC = this.ConvertStringToXmlDocument("");
                            checkIn.TEN_THUOC = this.ConvertStringToXmlDocument("");
                        }

                        var SereServMate = SereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                        if (SereServMate != null && SereServMate.Count > 0)
                        {
                            var sMediMate = SereServMate.OrderBy(o => o.INTRUCTION_TIME).ThenBy(o => o.SERVICE_REQ_ID).ToList()[0];
                            if (sMediMate.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM)
                            {
                                checkIn.MA_VAT_TU = this.ConvertStringToXmlDocument(sMediMate.TDL_HEIN_SERVICE_BHYT_CODE ?? "");
                                checkIn.TEN_VAT_TU = this.ConvertStringToXmlDocument(sMediMate.TDL_HEIN_SERVICE_BHYT_NAME ?? "");
                            }
                            else
                            {
                                checkIn.MA_VAT_TU = this.ConvertStringToXmlDocument(sMediMate.TDL_SERVICE_CODE ?? "");
                                checkIn.TEN_VAT_TU = this.ConvertStringToXmlDocument(sMediMate.TDL_SERVICE_NAME ?? "");
                            }
                        }
                        else
                        {
                            checkIn.MA_VAT_TU = this.ConvertStringToXmlDocument("");
                            checkIn.TEN_VAT_TU = this.ConvertStringToXmlDocument("");
                        }
                    }
                }
                lst.Add(checkIn);
                qd130CheckInRootList.TRANG_THAI_KCB = lst;
                rs = new ResultADO(true, "", new object[] { qd130CheckInRootList });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateThongBody()
        {
            ResultADO rs = null;
            try
            {
                var checkIn = this.CreateCheckInData();
                if (checkIn == null || !checkIn.Success || checkIn.Data == null || checkIn.Data.Length == 0)
                    return rs = new ResultADO(false, "Tao Danh Sach Ho So That Bai", null);
                Body body = new Body();
                body.CHECKIN = checkIn.Data[0] as XML.CheckIn;
                rs = new ResultADO(true, "", new object[] { body });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        private ResultADO CreateCheckInData()
        {
            ResultADO rs = null;
            try
            {
                XML.CheckIn checkIn = new XML.CheckIn();
                checkIn.MA_LK = this.Data.Treatment.TREATMENT_CODE ?? "";
                checkIn.STT = 1;
                checkIn.MA_BN = this.Data.Treatment.TDL_PATIENT_CODE ?? "";
                checkIn.HO_TEN = this.ConvertStringToXmlDocument(this.Data.Treatment.TDL_PATIENT_NAME ?? "");
                checkIn.SO_CCCD = !string.IsNullOrEmpty(this.Data.Treatment.TDL_PATIENT_CCCD_NUMBER) ? this.Data.Treatment.TDL_PATIENT_CCCD_NUMBER : "";
                checkIn.NGAY_SINH = this.Data.Treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == 1 ? this.Data.Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4) + "00000000" : this.Data.Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 12);
                checkIn.GIOI_TINH = Data.Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE ? 2 : 1;
                string MaDTKCB = "";
                if (Data.PatientTypeAlter != null)
                {
                    checkIn.MA_THE_BHYT = this.Data.PatientTypeAlter.HEIN_CARD_NUMBER ?? "";
                    checkIn.MA_DKBD = this.Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE ?? "";
                    checkIn.GT_THE_TU = this.Data.PatientTypeAlter.HEIN_CARD_FROM_TIME.HasValue ? this.Data.PatientTypeAlter.HEIN_CARD_FROM_TIME.Value.ToString().Substring(0, 8) : "";
                    checkIn.GT_THE_DEN = this.Data.PatientTypeAlter.HEIN_CARD_TO_TIME.HasValue ? this.Data.PatientTypeAlter.HEIN_CARD_TO_TIME.Value.ToString().Substring(0, 8) : "";
                    if (!string.IsNullOrEmpty(checkIn.MA_THE_BHYT))
                    {
                        string branchHeinMediOrgCode = (Data.Branch.HEIN_MEDI_ORG_CODE ?? "").Trim();
                        string ProvinceCode = !string.IsNullOrEmpty(Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE) ? Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE.Substring(0, 2) : "";
                        if (Data.ListHeinMediOrg != null && Data.ListHeinMediOrg.Count > 0)
                        {
                            GlobalConfigStore.HisHeinMediOrg = Data.ListHeinMediOrg;
                        }
                        string levelCodeHeinMediOrg = !string.IsNullOrEmpty(Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE) ? GlobalConfigStore.HisHeinMediOrg.FirstOrDefault(o => o.MEDI_ORG_CODE == Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE).LEVEL_CODE : "";
                        string branchProvinceCode = branchHeinMediOrgCode.Length > 2 ? branchHeinMediOrgCode.Substring(0, 2) : null;
                        if (Data.PatientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE && Data.PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        {
                            MaDTKCB = "2";
                        }
                        else if (Data.PatientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                        {
                            if (Data.PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.PRESENT)
                            {
                                MaDTKCB = "1.3";
                            }
                            else if (Data.PatientTypeAlter.HAS_ABSENT_LETTER == 1 || Data.PatientTypeAlter.HAS_WORKING_LETTER == 1)
                            {
                                MaDTKCB = "1.4";
                            }
                            else if (Data.PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.APPOINTMENT)
                            {
                                MaDTKCB = "1.5";
                            }
                            else if ((!String.IsNullOrEmpty(Data.Treatment.ICD_CODE) && Data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(Data.Treatment.ICD_CODE)) != null && Data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(Data.Treatment.ICD_CODE)).IS_LATENT_TUBERCULOSIS == 1) || (!String.IsNullOrEmpty(Data.Treatment.ICD_SUB_CODE) && Data.TotalIcdData.FirstOrDefault(o => (";" + Data.Treatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")) != null && Data.TotalIcdData.FirstOrDefault(o => (";" + Data.Treatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")).IS_LATENT_TUBERCULOSIS == 1) || Data.Treatment.IS_TUBERCULOSIS == 1)
                            {
                                MaDTKCB = "1.8";
                            }
                            else if (Data.Treatment.IS_HIV == 1)
                            {
                                MaDTKCB = "1.9";
                            }
                            else if ((!String.IsNullOrEmpty(Data.Treatment.ICD_CODE) && Data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(Data.Treatment.ICD_CODE)) != null && Data.TotalIcdData.FirstOrDefault(o => o.ICD_CODE.Equals(Data.Treatment.ICD_CODE)).IS_COVID == 1) || (!String.IsNullOrEmpty(Data.Treatment.ICD_SUB_CODE) && Data.TotalIcdData.FirstOrDefault(o => (";" + Data.Treatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")) != null && Data.TotalIcdData.FirstOrDefault(o => (";" + Data.Treatment.ICD_SUB_CODE + ";").Contains(";" + o.ICD_CODE + ";")).IS_COVID == 1))
                            {
                                MaDTKCB = "1.10";
                            }
                            else if (Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE == branchHeinMediOrgCode
                            || ValidAccept(Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE, Data.Branch.ACCEPT_HEIN_MEDI_ORG_CODE)
                            || ValidAccept(Data.PatientTypeAlter.HEIN_MEDI_ORG_CODE, Data.Branch.SYS_MEDI_ORG_CODE))
                            {
                                MaDTKCB = "1.1";
                            }
                            else if (Data.PatientTypeAlter.HAS_BIRTH_CERTIFICATE == MOS.LibraryHein.Bhyt.HeinHasBirthCertificate.HeinHasBirthCertificateCode.TRUE || ((MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT == Data.Branch.HEIN_LEVEL_CODE || MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE == Data.Branch.HEIN_LEVEL_CODE) && ProvinceCode == branchProvinceCode && (levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT || levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE)))
                            {
                                MaDTKCB = "1.2";
                            }
                        }
                        else if (Data.PatientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                        {
                            if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL && this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                MaDTKCB = "3.1";
                            }
                            else if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE && this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                MaDTKCB = "3.2";
                            }
                            else if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT && (ProvinceCode != branchProvinceCode || (levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL || levelCodeHeinMediOrg == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE)))
                            {
                                MaDTKCB = "3.3";
                            }
                            else if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL && this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                MaDTKCB = "3.4";
                            }
                            else if (Data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE && this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                MaDTKCB = "3.5";
                            }
                            else if (this.Data.PatientTypeAlter.HEIN_CARD_NUMBER.StartsWith("HN") || this.Data.PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K1 || this.Data.PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K2 || this.Data.PatientTypeAlter.LIVE_AREA_CODE == MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K3)
                            {
                                MaDTKCB = "3.6";
                            }
                            else
                            {
                                MaDTKCB = "3.7";
                            }
                        }
                        else if (this.Data.Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__NHANTHUOC)
                        {
                            MaDTKCB = "7.1";
                        }
                    }
                    else
                    {
                        MaDTKCB = "9";
                    }
                }
                checkIn.MA_DOITUONG_KCB = MaDTKCB;
                checkIn.NGAY_VAO = this.Data.Treatment.IN_TIME.ToString().Substring(0, 12);
                checkIn.NGAY_VAO_NOI_TRU = this.Data.Treatment.CLINICAL_IN_TIME.HasValue ? this.Data.Treatment.CLINICAL_IN_TIME.ToString().Substring(0, 12) : "";
                checkIn.LY_DO_VNT = this.ConvertStringToXmlDocument(this.Data.Treatment.HOSPITALIZE_REASON_NAME ?? "");
                checkIn.MA_LY_DO_VNT = this.Data.Treatment.HOSPITALIZE_REASON_CODE ?? "";
                checkIn.DU_PHONG = "";
                string MaLoaiKCB = "10";
                switch (this.Data.Treatment.TDL_TREATMENT_TYPE_ID)
                {
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM:
                        MaLoaiKCB = "01";
                        break;
                    case IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU:
                        if (Data.Treatment.OUT_TIME.HasValue && Data.Treatment.CLINICAL_IN_TIME.HasValue && (Data.Treatment.OUT_TIME.Value - Data.Treatment.CLINICAL_IN_TIME.Value) > 0 && Inventec.Common.DateTime.Calculation.DifferenceTime(Data.Treatment.CLINICAL_IN_TIME.Value, Data.Treatment.OUT_TIME.Value, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR) < 4)
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
                        if (this.Data.Treatment.IS_CHRONIC != 1)
                            MaLoaiKCB = "02";
                        else if (this.Data.ListSereServ != null && this.Data.ListSereServ.Count > 0)
                        {
                            if (this.Data.ListSereServ.Exists(o => o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH && o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT && o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT && o.TDL_SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK))
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
                checkIn.MA_LOAI_KCB = MaLoaiKCB;

                if (Data.Branch != null)
                {
                    checkIn.MA_CSKCB = Data.Branch.HEIN_MEDI_ORG_CODE;
                }
                else
                {
                    checkIn.MA_CSKCB = GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE;
                }
                if (this.Data.ListSereServ != null && this.Data.ListSereServ.Count > 0)
                {
                    var SereServ = this.Data.ListSereServ.Where(o => o.IS_NO_EXECUTE != 1 && o.IS_DELETE != 1).ToList();
                    if (SereServ != null && SereServ.Count > 0)
                    {
                        SereServ = SereServ.OrderBy(o => o.SERVICE_REQ_ID).ToList();
                        checkIn.MA_DICH_VU = SereServ[0].TDL_HEIN_SERVICE_BHYT_CODE;
                        checkIn.TEN_DICH_VU = this.ConvertStringToXmlDocument(SereServ[0].TDL_HEIN_SERVICE_BHYT_NAME ?? "");
                        checkIn.NGAY_YL = SereServ[0].TDL_INTRUCTION_TIME.ToString().Substring(0, 12);
                    }
                }

                rs = new ResultADO(true, "", new object[] { checkIn });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
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
        private ResultADO CreateThongHeader()
        {
            ResultADO rs = null;
            try
            {
                Header header = new Header();
                header.MESSAGE_VERSION = "1.0";
                header.TRANSACTION_TYPE = "M0001";
                header.TRANSACTION_NAME = "Web Service";
                header.TRANSACTION_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                header.TRANSACTION_ID = "";
                header.REQUEST_ID = "";
                header.ACTION_TYPE = 0;
                if (Data.Branch != null)
                {
                    header.SENDER_CODE = Data.Branch.HEIN_MEDI_ORG_CODE;
                    header.SENDER_NAME = Data.Branch.BRANCH_NAME;
                }
                else
                {
                    header.SENDER_CODE = GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE;
                    header.SENDER_NAME = GlobalConfigStore.Branch.BRANCH_NAME;
                }
                rs = new ResultADO(true, "", new object[] { header });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        internal async Task<SyncResultADO> SendXmlCheckIn(InputADO data)
        {
            SyncResultADO sResult = new SyncResultADO();
            try
            {
                if (data == null || data.serverInfo == null || string.IsNullOrEmpty(data.serverInfo.Username) || string.IsNullOrEmpty(data.serverInfo.Password) || string.IsNullOrEmpty(data.serverInfo.Address))
                {
                    sResult.ErrorCode = ServerInfo.CodeLoiCauHinhHeThong;
                    sResult.Message = ServerInfo.Text01;
                    return sResult;
                }
                if (dicLoginResultByUserName.ContainsKey(data.serverInfo.Username))
                {
                    plv = dicLoginResultByUserName[data.serverInfo.Username];
                }
                else
                    plv = null;
                var expires_in = plv != null ? Convert.ToDateTime(plv.APIKey.expires_in) : DateTime.MinValue;
                if (plv == null || DateTime.Now >= expires_in)
                    sResult = await RegisToken(data.serverInfo.Username, data.serverInfo.Password, data.serverInfo.Address);
                if (!dicLoginResultByUserName.ContainsKey(data.serverInfo.Username))
                {
                    dicLoginResultByUserName.Add(data.serverInfo.Username, plv);
                }
                else
                    dicLoginResultByUserName[data.serverInfo.Username] = plv;
                if ((plv == null || (plv != null && plv.maKetQua == "401")) && sResult != null && !sResult.Success)
                    return sResult;
                if (string.IsNullOrEmpty(data.checkInData))
                {
                    sResult.ErrorCode = ServerInfo.CodeLoiTaoDuLieuXML;
                    sResult.Message = ServerInfo.Text04;
                    return sResult;
                }
                using (var client = new HttpClient())
                {
                    string requestUri = data.serverInfo.checkinApi ?? "api/qd130/checkInKcbQd4750";
                    string fullrequestUri = requestUri;
                    int index = data.serverInfo.Address.IndexOf('/', data.serverInfo.Address.IndexOf("//") + 2);
                    if (index > 0)
                    {
                        string extension = data.serverInfo.Address.Substring(index);
                        if (!requestUri.Contains(extension))
                        {
                            fullrequestUri = extension + requestUri;
                        }
                    }
                    var values = new Dictionary<string, string>
                    {
                           { "accessToken", plv.APIKey.access_token },
                           { "tokenId", plv.APIKey.id_token },
                           { "passwordHash", ConvertStringToMD5(data.serverInfo.Password) }
                        };
                    client.BaseAddress = new Uri(data.serverInfo.Address);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    foreach (var kvp in values)
                    {
                        client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                    }
                    var resp = client.PostAsync(fullrequestUri,
                        new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new CheckInInputADO()
                        {
                            username = data.serverInfo.Username,
                            loaiHoSo = "0",
                            maTinh = data.Branch.HEIN_PROVINCE_CODE,
                            maCSKCB = data.Branch.HEIN_MEDI_ORG_CODE,
                            fileHSBase64 = data.checkInData,
                            chukydonvi = ""
                        })
                        , Encoding.UTF8, "application/json")).Result;

                    if (resp == null || !resp.IsSuccessStatusCode)
                    {
                        int statusCode = resp.StatusCode.GetHashCode();
                        sResult.ErrorCode = ServerInfo.CodeLoiGoiApiGuiHoSo;
                        sResult.Message = ServerInfo.Text06;
                        Inventec.Common.Logging.LogSystem.Error(string.Format("Lỗi khi gọi API: {0}. StatusCode: {1}", data.serverInfo.Address, statusCode));
                        return sResult;
                    }
                    string responseData = resp.Content.ReadAsStringAsync().Result;
                    CheckInResultADO result = JsonConvert.DeserializeObject<CheckInResultADO>(responseData);
                    Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => responseData), responseData));
                    if (result == null)
                    {
                        sResult.ErrorCode = ServerInfo.CodeKhongXacDinh;
                        sResult.Message = string.Format("Lỗi khi gọi API. Response {0}:", responseData);
                        return sResult;
                    }
                    else
                    {
                        if (result.maKetQua != "200") {
                            sResult.ErrorCode = ServerInfo.CodeLoiDoHeThongBHYTTraVe;
                            sResult.Message = ServerInfo.Text05;
                        }
                        else  if (!string.IsNullOrEmpty(result.thongDiep) && result.thongDiep != "Tiếp nhận thành công")
                        {
                            sResult.ErrorCode = ServerInfo.CodeLoiGoiApiGuiHoSo;
                            sResult.Message = string.Format("{0}: {1}", ServerInfo.Text06, result.thongDiep);
                        }
                        else
                        {
                            sResult.CheckCode = result.maGiaoDich;
                            sResult.Success = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return sResult;
        }
        private static string ConvertStringToMD5(string password)
        {
            string s_PasswordMD5 = string.Empty;
            try
            {
                byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
                byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
                s_PasswordMD5 = BitConverter.ToString(hash).Replace("-", string.Empty);
            }
            catch (Exception)
            {
                LogSystem.Error("Lỗi khi convert chuỗi sang dạng mã hóa md5.");
            }
            return s_PasswordMD5;
        }

        private async Task<SyncResultADO> RegisToken(string loginname, string password, string addressBase)
        {
            SyncResultADO sResult = new SyncResultADO();
            try
            {
                try
                {
                    Inventec.Common.Logging.LogSystem.Debug("RegisToken.1");
                    ServicePointManager
                 .ServerCertificateValidationCallback +=
                 (sender, cert, chain, sslPolicyErrors) => true;
                    System.Net.ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls12 |
            SecurityProtocolType.Tls11 |
            SecurityProtocolType.Tls;
                    Inventec.Common.Logging.LogSystem.Debug("RegisToken.2");
                }
                catch (Exception exx)
                {
                    LogSystem.Warn("ServicePointManager.ServerCertificateValidationCallback error:", exx);
                }

                using (var client = new HttpClient())
                {
                    Inventec.Common.Logging.LogSystem.Error("Bat dau HttpClient ");
                    client.BaseAddress = new Uri(addressBase);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var values = new Dictionary<string, string>
                        {
                           { "username", loginname },
                           { "password", ConvertStringToMD5(password) }
                        };
                    Inventec.Common.Logging.LogSystem.Debug(ConvertStringToMD5(password));
                    var content = new FormUrlEncodedContent(values);

                    Inventec.Common.Logging.LogSystem.Error("Bat dau PostAsync HttpResponseMessage ");
                    HttpResponseMessage response = await client.PostAsync("api/token/take", content);

                    Inventec.Common.Logging.LogSystem.Error("Ket thuc PostAsync HttpResponseMessage");
                    if (response.IsSuccessStatusCode)
                    {
                        plv = response.Content.ReadAsAsync<LoginResultLDO>().Result;
                    }
                    else
                    {
                        sResult.ErrorCode = ServerInfo.CodeTaiKhoanKhongHopLe;
                        sResult.Message = ServerInfo.Text03;
                        LogSystem.Error("Dang nhap That Bai >>>> :" + content);
                    }
                }
            }
            catch (Exception ex)
            {
                sResult.ErrorCode = ServerInfo.CodeLoiKetNoiHeThongBHYT;
                sResult.Message = ServerInfo.Text02;
                LogSystem.Error("Dang nhap That Bai >>>> :" + ex);
            }
            if (plv != null && plv.maKetQua == "401")
            {
                sResult.ErrorCode = ServerInfo.CodeTaiKhoanKhongHopLe;
                sResult.Message = ServerInfo.Text03;
                LogSystem.Warn("Dang ky token that bai");
            }
            return sResult;
        }

        internal DataCheckIn GetXmlCheckInData()
        {
            DataCheckIn result = null;
            try
            {
                if (string.IsNullOrEmpty(XmlString))
                    return result;
                result = Deserialize<DataCheckIn>(XmlString);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        internal DataQd130CheckIn GetXmlQd130CheckInData()
        {
            DataQd130CheckIn result = null;
            try
            {
                if (string.IsNullOrEmpty(XmlString))
                    return result;
                result = Deserialize<DataQd130CheckIn>(XmlString);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
