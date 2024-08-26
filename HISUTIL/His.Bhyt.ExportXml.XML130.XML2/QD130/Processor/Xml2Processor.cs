using His.Bhyt.ExportXml.XML130.XML2.Base;
using His.Bhyt.ExportXml.XML130.XML2.QD130.ADO;
using His.Bhyt.ExportXml.XML130.XML2.QD130.XML;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML2.QD130.Processor
{
    class Xml2Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXml2ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                string Config_PatientTypeCodeBHYTOption = "";
                string Config_TutorialFormat = "";
                if (data.HisConfig != null && data.HisConfig.Count > 0)
                {
                    Config_PatientTypeCodeBHYTOption = HisConfigKey.GetConfigData(data.HisConfig, HisConfigKey.PatientTypeCodeBHYTCFG);
                    Config_TutorialFormat = HisConfigKey.GetConfigData(data.HisConfig, HisConfigKey.TutorialFormatCFG);
                }

                List<Xml2ADO> listXml2Ado = new List<Xml2ADO>();
                if (data.vSereServ2 == null || data.vSereServ2.Count == 0 || data.vTreatment12 == null)
                {
                    return new ResultADO(true, "", new object[] { listXml2Ado });
                }

                var HeinServiceType = new List<long?>
        {
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU,
            IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM
        };
                var SereServs = data.vSereServ2.Where(o => HeinServiceType.Contains(o.TDL_HEIN_SERVICE_TYPE_ID)).OrderBy(o => o.INTRUCTION_TIME).ToList();
                if (SereServs == null || SereServs.Count == 0)
                    return new ResultADO(true, "", new object[] { listXml2Ado });

                int stt = 1;
                foreach (var SereServ in SereServs)
                {
                    var patientType = data.HisPatientTypes != null ? data.HisPatientTypes.Where(o => o.ID == SereServ.PATIENT_TYPE_ID).FirstOrDefault() : null;

                    Xml2ADO xml2 = new Xml2ADO();
                    xml2.MA_LK = data.vTreatment12.TREATMENT_CODE ?? "";
                    xml2.STT = stt.ToString("G27", CultureInfo.InvariantCulture);
                    if (SereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU ||
                        SereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM ||
                        !SereServ.MEDICINE_ID.HasValue)
                    {
                        xml2.MA_THUOC = SereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                    }
                    else
                    {
                        xml2.MA_THUOC = SereServ.ACTIVE_INGR_BHYT_CODE ?? "";
                    }

                    if (String.IsNullOrWhiteSpace(xml2.MA_THUOC))
                    {
                        xml2.MA_THUOC = SereServ.TDL_SERVICE_CODE ?? "";
                    }

                    List<string> ppCheBien = new List<string>();
                    if (!string.IsNullOrEmpty(SereServ.PREPROCESSING_CODE))
                        ppCheBien.Add(SereServ.PREPROCESSING_CODE);

                    if (!string.IsNullOrEmpty(SereServ.PROCESSING_CODE))
                        ppCheBien.Add(SereServ.PROCESSING_CODE);

                    if (ppCheBien.Count > 0)
                    {
                        var CheBien = String.Join(";", ppCheBien);
                        if (CheBien.EndsWith(";"))
                        {
                            CheBien = CheBien.Substring(0, CheBien.Length - 1);
                        }
                        xml2.MA_PP_CHEBIEN = CheBien ?? "";
                    }
                    else
                        xml2.MA_PP_CHEBIEN = "";


                    xml2.MA_CSKCB_THUOC = "";
                    xml2.MA_NHOM = SereServ.HST_BHYT_CODE ?? "";
                    xml2.TEN_THUOC = SereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                    xml2.DON_VI_TINH = SereServ.SERVICE_UNIT_NAME ?? "";
                    xml2.HAM_LUONG = SereServ.CONCENTRA ?? "";
                    xml2.DUONG_DUNG = SereServ.MEDICINE_USE_FORM_CODE ?? "";
                    xml2.DANG_BAO_CHE = SereServ.DOSAGE_FORM ?? "";
                    xml2.LIEU_DUNG = SereServ.TUTORIAL ?? "";
                    if (!String.IsNullOrWhiteSpace(xml2.LIEU_DUNG))
                    {
                        if (SereServ.MEDICINE_LINE_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__CP_YHCT && SereServ.MEDICINE_LINE_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__VT_YHCT)
                        {
                            xml2.LIEU_DUNG = ProcessDataTutorial(SereServ.TUTORIAL, Config_TutorialFormat);
                            System.DateTime? dateBefore = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTimeUTC(SereServ.TDL_INTRUCTION_DATE);
                            System.DateTime? dateAfter = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTimeUTC(SereServ.USE_TIME_TO ?? 0);
                            if (dateBefore != null && dateAfter != null)
                            {
                                int SoNgay = (int)((TimeSpan)(dateAfter.Value.Date - dateBefore.Value.Date)).TotalDays + 1;
                                decimal TongSoThuoc = SereServ.AMOUNT / SoNgay;
                                xml2.LIEU_DUNG += " * " + SoNgay + " ngày [" + TongSoThuoc + " " + SereServ.SERVICE_UNIT_NAME + "/ngày]";
                            }
                        }
                        else
                        {
                            xml2.LIEU_DUNG = SereServ.TUTORIAL ?? "";
                        }
                    }
                    else
                    {
                        xml2.LIEU_DUNG = "";
                    }
                    xml2.CACH_DUNG = (SereServ.ADVISE ?? "");
                    if (xml2.CACH_DUNG.Length > 1024)
                        xml2.CACH_DUNG = xml2.CACH_DUNG.Substring(0, 1024);
                    while (Encoding.UTF8.GetByteCount(xml2.CACH_DUNG) > 1023)
                    {
                        xml2.CACH_DUNG = xml2.CACH_DUNG.Substring(0, xml2.CACH_DUNG.Length-2);
                    }
                    xml2.SO_DANG_KY = SereServ.MEDICINE_REGISTER_NUMBER ?? "";

                    List<string> ttThau = new List<string>();
                    if (!string.IsNullOrEmpty(SereServ.MEDICINE_BID_EXTRA_CODE)) ttThau.Add(SereServ.MEDICINE_BID_EXTRA_CODE);

                    if (!string.IsNullOrEmpty(SereServ.MEDICINE_BID_PACKAGE_CODE)) ttThau.Add(SereServ.MEDICINE_BID_PACKAGE_CODE);

                    if (!string.IsNullOrEmpty(SereServ.MEDICINE_BID_GROUP_CODE)) ttThau.Add(SereServ.MEDICINE_BID_GROUP_CODE);

                    if (!string.IsNullOrEmpty(SereServ.MEDICINE_BID_YEAR)) ttThau.Add(SereServ.MEDICINE_BID_YEAR);

                    if (ttThau.Count > 0)
                    {
                        var thau = String.Join(";", ttThau);
                        if (thau.EndsWith(";"))
                        {
                            thau = thau.Substring(0, thau.Length - 1);
                        }
                        xml2.TT_THAU = thau ?? "";
                    }
                    else
                        xml2.TT_THAU = "";

                    decimal TyleTTBH = 0;
                    if (patientType.PATIENT_TYPE_CODE == Config_PatientTypeCodeBHYTOption)
                    {
                        if (SereServ.ORIGINAL_PRICE > 0)
                        {
                            if (SereServ.HEIN_LIMIT_PRICE != null)
                            {
                                TyleTTBH = Math.Round((SereServ.HEIN_LIMIT_PRICE.Value / (SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO))) * 100, 0);
                            }
                            else
                            {
                                TyleTTBH = Math.Round((SereServ.PRICE / SereServ.ORIGINAL_PRICE) * 100, 0);
                            }
                        }
                    }

                    decimal SoLuong = Math.Round(SereServ.AMOUNT, 3, MidpointRounding.AwayFromZero);
                    decimal DonGia = Math.Round(SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO), 3, MidpointRounding.AwayFromZero);

                    //quy doi don vi tinh
                    if (SereServ.CONVERT_RATIO != null && SereServ.USE_ORIGINAL_UNIT_FOR_PRES != 1)
                    {
                        xml2.DON_VI_TINH = SereServ.CONVERT_UNIT_NAME ?? "";
                        SoLuong = SereServ.AMOUNT * SereServ.CONVERT_RATIO.Value;
                        DonGia = (SereServ.ORIGINAL_PRICE * (1 + SereServ.VAT_RATIO)) / SereServ.CONVERT_RATIO.Value;
                    }

                    decimal tBHTT = 0, TienBH = 0, TienBV = 0, NguonKhac = 0;

                    if (SoLuong != 0 && DonGia != 0)
                    {
                        TienBV = Math.Round(SoLuong * DonGia, 2, MidpointRounding.AwayFromZero);

                        if (TyleTTBH != 0)
                        {
                            TienBH = Math.Round(SoLuong * DonGia * (TyleTTBH / 100), 2, MidpointRounding.AwayFromZero);
                        }
                    }
                    if (TienBH > 0 && SereServ.HEIN_RATIO != null)
                    {
                        tBHTT = Math.Round(TienBH * (SereServ.HEIN_RATIO ?? 0), 2, MidpointRounding.AwayFromZero);
                    }

                    if (SereServ.OTHER_SOURCE_PRICE != null)
                    {
                        NguonKhac = Math.Round((SereServ.OTHER_SOURCE_PRICE ?? 0) * SereServ.AMOUNT, 2, MidpointRounding.AwayFromZero);
                    }

                    if (tBHTT > 0)
                    {
                    //    if (!String.IsNullOrEmpty(SereServ.HEIN_CARD_NUMBER) &&
                    //(SereServ.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CA") ||
                    //SereServ.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CY") ||
                    //SereServ.HEIN_CARD_NUMBER.Substring(0, 2).Equals("QN")))
                    //    {
                    //        xml2.PHAM_VI = "3";
                    //    }
                    //    else
                         xml2.PHAM_VI = "1";
                    }
                    else if (tBHTT == 0)
                    {
                        xml2.PHAM_VI = "2";
                    }
                    else
                        xml2.PHAM_VI = "";

                    xml2.TYLE_TT_BH = TyleTTBH.ToString("G27", CultureInfo.InvariantCulture);
                    xml2.SO_LUONG = SoLuong.ToString("G27", CultureInfo.InvariantCulture);
                    xml2.DON_GIA = DonGia.ToString("G27", CultureInfo.InvariantCulture);

                    xml2.THANH_TIEN_BV = TienBV.ToString("G27", CultureInfo.InvariantCulture);
                    xml2.THANH_TIEN_BH = TienBH.ToString("G27", CultureInfo.InvariantCulture);
                    if (SereServ.HEIN_PAY_SOURCE_TYPE_ID == 1)
                    {
                        xml2.T_NGUONKHAC_NSNN = NguonKhac.ToString("G27", CultureInfo.InvariantCulture);
                    }
                    else
                        xml2.T_NGUONKHAC_NSNN = "0";

                    if (SereServ.HEIN_PAY_SOURCE_TYPE_ID == 2)
                    {
                        xml2.T_NGUONKHAC_VTNN = NguonKhac.ToString("G27", CultureInfo.InvariantCulture);
                    }
                    else
                        xml2.T_NGUONKHAC_VTNN = "0";

                    if (SereServ.HEIN_PAY_SOURCE_TYPE_ID == 3)
                    {
                        xml2.T_NGUONKHAC_VTTN = NguonKhac.ToString("G27", CultureInfo.InvariantCulture);
                    }
                    else
                        xml2.T_NGUONKHAC_VTTN = "0";

                    if (SereServ.HEIN_PAY_SOURCE_TYPE_ID != 1 || SereServ.HEIN_PAY_SOURCE_TYPE_ID != 2 || SereServ.HEIN_PAY_SOURCE_TYPE_ID != 3)
                    {
                        xml2.T_NGUONKHAC_CL = NguonKhac.ToString("G27", CultureInfo.InvariantCulture);
                    }
                    else
                        xml2.T_NGUONKHAC_CL = "";

                    xml2.T_NGUONKHAC = NguonKhac.ToString("G27", CultureInfo.InvariantCulture);
                    xml2.MUC_HUONG = SereServ.HEIN_RATIO != null ? (SereServ.HEIN_RATIO.Value * 100).ToString("G27", CultureInfo.InvariantCulture) : "0";
                    xml2.T_BHTT = tBHTT.ToString("G27", CultureInfo.InvariantCulture);
                    var tbntt = TienBV - TienBH - NguonKhac;
                    if (tbntt < 0)
                    {
                        xml2.T_BNCCT = (TienBH - tBHTT + tbntt).ToString("G27", CultureInfo.InvariantCulture);
                        xml2.T_BNTT = (0).ToString("G27", CultureInfo.InvariantCulture);

                    }
                    else
                    {
                        xml2.T_BNCCT = (TienBH - tBHTT).ToString("G27", CultureInfo.InvariantCulture);
                        xml2.T_BNTT = (tbntt).ToString("G27", CultureInfo.InvariantCulture);

                    }
                    
                    xml2.MA_KHOA = SereServ.REQUEST_BHYT_CODE ?? "";
                    if (data.HisEmployee != null && data.HisEmployee.Count > 0)
                    {
                        var Employee = data.HisEmployee.FirstOrDefault(o => o.LOGINNAME == SereServ.REQUEST_LOGINNAME);
                        xml2.MA_BAC_SI = Employee != null ? Employee.DIPLOMA ?? "" : "";
                    }
                    else
                        xml2.MA_BAC_SI = "";

                    xml2.MA_DICH_VU = SereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";

                    if (SereServ.PARENT_ID != null && SereServ.MEDICINE_IS_ANAESTHESIA == 1 && data.vSereServ2 != null && data.vSereServ2.Count > 0)
                    {
                        var ssParent = data.vSereServ2.FirstOrDefault(o => o.ID == SereServ.PARENT_ID);
                        if (ssParent != null)
                        {
                            var ssPttt = data.vHisSereServPttt.FirstOrDefault(o => o.SERE_SERV_ID == ssParent.ID);
                            if (ssPttt != null && ssPttt.IS_ANAESTHESIA == 1)
                            {
                                if (!xml2.MA_DICH_VU.EndsWith("_GT"))
                                {
                                    xml2.MA_DICH_VU += "_GT";
                                }
                            }
                        }
                    }

                    xml2.NGAY_YL = SereServ.INTRUCTION_TIME.ToString().Substring(0, 12);
                    xml2.NGAY_TH_YL = SereServ.START_TIME != null && SereServ.START_TIME > 0 ? SereServ.START_TIME.ToString().Substring(0, 12) : xml2.NGAY_YL;
                    xml2.MA_PTTT = "1";

                    if (tBHTT > 0)
                    {
                        xml2.NGUON_CTRA = "1";
                    }
                    else if (SereServ.HEIN_PAY_SOURCE_TYPE_ID == 2 || SereServ.HEIN_PAY_SOURCE_TYPE_ID == 3)
                    {
                        xml2.NGUON_CTRA = "2";
                    }
                    else if (SereServ.HEIN_PAY_SOURCE_TYPE_ID == 1)
                    {
                        xml2.NGUON_CTRA = "3";
                    }
                    else
                    {
                        xml2.NGUON_CTRA = "4";
                    }

                    xml2.VET_THUONG_TP = "";
                    xml2.DU_PHONG = "";

                    listXml2Ado.Add(xml2);
                    stt++;
                }
                rs = new ResultADO(true, "", new object[] { listXml2Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }
        /// <summary>
        /// "1 viên/lần * 2 lần/ngày (Ngày uống 3 viên chia 2 lần, sáng 01 viên, chiều 02 viên)" lên xml là "1 viên/lần * 2 lần/ngày"
        /// "(Ngày uống 3 viên chia 2 lần, sáng 01 viên, chiều 02 viên)" lên xml là "Ngày uống 3 viên chia 2 lần, sáng 01 viên, chiều 02 viên"
        /// </summary>
        /// <param name="tutorial"></param>
        /// <param name="configValue"></param>
        /// <returns></returns>
        private string ProcessDataTutorial(string tutorial, string configValue)
        {
            string result = tutorial;
            try
            {
                if (configValue == "3")
                {
                    List<string> DataInParentheses = new List<string>();

                    while (result.Contains("(") && result.Contains(")"))
                    {
                        List<int> indexStart = new List<int>();
                        List<int> indexEnd = new List<int>();

                        for (int i = 0; i < result.Length; i++)
                        {
                            if (result[i] == '(')
                            {
                                indexStart.Add(i);
                            }
                            else if (result[i] == ')')
                            {
                                indexEnd.Add(i);
                            }
                        }

                        if (indexStart.Count > 0 && indexEnd.Count > 0)
                        {
                            for (int i = indexStart.Count - 1; i >= 0; i--)
                            {
                                if (indexEnd.Count == 0)
                                {
                                    break;
                                }

                                int end = indexEnd.FirstOrDefault(o => o - indexStart[i] > 0);
                                if (end > 0)
                                {
                                    DataInParentheses.Add(result.Substring(indexStart[i], end - indexStart[i] + 1));
                                    indexEnd.Remove(end);
                                }
                            }
                        }

                        foreach (var item in DataInParentheses)
                        {
                            result = result.Replace(item, "");
                        }
                    }

                    if (String.IsNullOrWhiteSpace(result) && DataInParentheses.Count > 0)
                    {
                        result = DataInParentheses.OrderByDescending(o => o.Length).FirstOrDefault();
                        result = result.Substring(1, result.Length - 2);
                    }
                }
            }
            catch (Exception ex)
            {
                result = tutorial;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        internal void MapADOToXml(List<Xml2ADO> listAdo, ref List<XML2DetailData> datas)
        {
            try
            {
                if (datas == null)
                    datas = new List<XML2DetailData>();
                if (listAdo != null || listAdo.Count > 0)
                {
                    foreach (var ado in listAdo)
                    {
                        XML2DetailData detail = new XML2DetailData();
                        detail.MA_LK = ado.MA_LK;
                        detail.STT = ado.STT;
                        detail.MA_THUOC = ado.MA_THUOC;
                        detail.MA_PP_CHEBIEN = ado.MA_PP_CHEBIEN;
                        detail.MA_CSKCB_THUOC = ado.MA_CSKCB_THUOC;
                        detail.MA_NHOM = ado.MA_NHOM;
                        detail.TEN_THUOC = this.ConvertStringToXmlDocument(ado.TEN_THUOC);
                        detail.DON_VI_TINH = ado.DON_VI_TINH;
                        detail.HAM_LUONG = this.ConvertStringToXmlDocument(ado.HAM_LUONG);
                        detail.DUONG_DUNG = this.ConvertStringToXmlDocument(ado.DUONG_DUNG);
                        detail.DANG_BAO_CHE = ado.DANG_BAO_CHE;
                        detail.LIEU_DUNG = this.ConvertStringToXmlDocument(ado.LIEU_DUNG);
                        detail.CACH_DUNG = this.ConvertStringToXmlDocument(ado.CACH_DUNG);
                        detail.SO_DANG_KY = ado.SO_DANG_KY;
                        detail.TT_THAU = ado.TT_THAU;
                        detail.PHAM_VI = ado.PHAM_VI;
                        detail.TYLE_TT_BH = ado.TYLE_TT_BH;
                        detail.SO_LUONG = ado.SO_LUONG;
                        detail.DON_GIA = ado.DON_GIA;
                        detail.THANH_TIEN_BV = ado.THANH_TIEN_BV;
                        detail.THANH_TIEN_BH = ado.THANH_TIEN_BH;
                        detail.T_NGUONKHAC_NSNN = ado.T_NGUONKHAC_NSNN;
                        detail.T_NGUONKHAC_VTNN = ado.T_NGUONKHAC_VTNN;
                        detail.T_NGUONKHAC_VTTN = ado.T_NGUONKHAC_VTTN;
                        detail.T_NGUONKHAC_CL = ado.T_NGUONKHAC_CL;
                        detail.T_NGUONKHAC = ado.T_NGUONKHAC;
                        detail.MUC_HUONG = ado.MUC_HUONG;
                        detail.T_BHTT = ado.T_BHTT;
                        detail.T_BNCCT = ado.T_BNCCT;
                        detail.T_BNTT = ado.T_BNTT;
                        detail.MA_KHOA = ado.MA_KHOA;
                        detail.MA_BAC_SI = ado.MA_BAC_SI;
                        detail.MA_DICH_VU = ado.MA_DICH_VU;
                        detail.NGAY_YL = ado.NGAY_YL;
                        detail.NGAY_TH_YL = ado.NGAY_TH_YL;
                        detail.MA_PTTT = ado.MA_PTTT;
                        detail.NGUON_CTRA = ado.NGUON_CTRA;
                        detail.VET_THUONG_TP = ado.VET_THUONG_TP;
                        detail.DU_PHONG = ado.DU_PHONG;
                        datas.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string SubMaxLength(string input)
        {
            string result = input;
            if (!String.IsNullOrEmpty(input) && input.Length > GlobalConfigStore.MAX_LENGTH)
            {
                result = input.Substring(0, GlobalConfigStore.MAX_LENGTH);
            }
            return result;
        }
    }
}
