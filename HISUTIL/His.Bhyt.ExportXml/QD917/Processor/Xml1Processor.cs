using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.QD917.ADO;
using His.Bhyt.ExportXml.QD917.XML.HoSo;
using His.Bhyt.ExportXml.QD917.XML.XML1;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.LibraryHein.Bhyt;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.QD917.Processor
{
    class Xml1Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXml1Data(InputADO data, List<Xml2ADO> listXmlThuocAdo, List<Xml3ADO> listXmlDvktVt)
        {
            ResultADO result = null;
            try
            {
                XML1Data xml1 = new XML1Data();

                if (data.HeinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                {
                    if (data.HeinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        xml1.MA_LYDO_VVIEN = 2;
                    else
                        xml1.MA_LYDO_VVIEN = 1;

                }
                else if (data.HeinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                    xml1.MA_LYDO_VVIEN = 3;

                if (data.Treatment.ACCIDENT_HURT_TYPE_ID.HasValue && !String.IsNullOrEmpty(data.Treatment.ACCIDENT_HURT_TYPE_BHYT_CODE))
                {
                    xml1.MA_TAI_NAN = data.Treatment.ACCIDENT_HURT_TYPE_BHYT_CODE;
                }
                else
                {
                    xml1.MA_TAI_NAN = "0";
                }

                if (data.HeinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                {
                    if (data.Treatment.CLINICAL_IN_TIME.HasValue && data.Treatment.OUT_TIME.HasValue)
                    {
                        xml1.SO_NGAY_DTRI = HIS.Treatment.DateTime.Calculation.DayOfTreatment(data.Treatment.CLINICAL_IN_TIME.Value, data.Treatment.OUT_TIME.Value) + "";
                    }
                    else
                    {
                        xml1.SO_NGAY_DTRI = "0";
                    }
                }
                else
                {
                    xml1.SO_NGAY_DTRI = "1";
                }

                if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.Khoi;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.Do;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.KhongThayDoi;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.NangHon;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.TuVong;

                if (data.Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    xml1.TINH_TRANG_RV = TreatmentEndTypeBhytCFG.ChuyenVien;
                else if (data.Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                    xml1.TINH_TRANG_RV = TreatmentEndTypeBhytCFG.TronVien;
                else if (data.Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN)
                    xml1.TINH_TRANG_RV = TreatmentEndTypeBhytCFG.XinRaVien;
                else
                    xml1.TINH_TRANG_RV = TreatmentEndTypeBhytCFG.RaVien;

                decimal tongchi = 0;
                decimal tientThuoc = 0;
                if (listXmlThuocAdo != null && listXmlThuocAdo.Count > 0)
                {
                    foreach (var xml2 in listXmlThuocAdo)
                    {
                        tientThuoc += Math.Round(xml2.ThanhTien, 2, MidpointRounding.AwayFromZero);
                        tongchi += xml2.ThanhTien;
                    }
                }
                xml1.T_THUOC = tientThuoc.ToString("G27", CultureInfo.InvariantCulture);

                decimal tientVTYT = 0;
                if (listXmlDvktVt != null && listXmlDvktVt.Count > 0)
                {
                    foreach (var xml3 in listXmlDvktVt)
                    {
                        tongchi += xml3.ThanhTien;
                        if (xml3.IsMaterial)
                            tientVTYT += Math.Round(xml3.ThanhTien, 2, MidpointRounding.AwayFromZero);
                    }
                }

                xml1.T_VTYT = Math.Round(tientVTYT, 0).ToString("G27", CultureInfo.InvariantCulture);
                //decimal tongchi = data.ListSereServ.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                //xml1.T_TONGCHI = Math.Round(tongchi, 0).ToString("G27", CultureInfo.InvariantCulture);

                decimal bntra = Math.Round(data.ListSereServ.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0), 2, MidpointRounding.AwayFromZero);

                xml1.T_BNTT = bntra.ToString("G27", CultureInfo.InvariantCulture);

                //var bhytTra = Math.Round(data.ListSereServ.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0), 0, MidpointRounding.AwayFromZero);
                //decimal tongchi = data.ListSereServ.Sum(s => ((s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0) + (s.VIR_TOTAL_HEIN_PRICE ?? 0)));
                LogSystem.Info("TongChi: " + tongchi);
                tongchi = Math.Round(tongchi, 2, MidpointRounding.AwayFromZero);

                xml1.T_TONGCHI = tongchi.ToString("G27", CultureInfo.InvariantCulture);

                xml1.MUC_HUONG = this.GetDefaultHeinRatio(data.HeinApproval, tongchi);

                xml1.T_BHTT = (tongchi - bntra).ToString("G27", CultureInfo.InvariantCulture);

                if (CheckBhytNsd(GlobalConfigStore.ListIcdCode_Nds, GlobalConfigStore.ListIcdCode_Nds_Te,
                    data.Treatment, data.HeinApproval))
                {
                    xml1.T_NGOAIDS = xml1.T_BHTT;
                }
                else
                {
                    xml1.T_NGOAIDS = "0";
                }

                xml1.T_NGUONKHAC = "0";

                xml1.NGAY_TTOAN = data.HeinApproval.EXECUTE_TIME.Value.ToString().Substring(0, 12);
                xml1.NAM_QT = Convert.ToInt32(data.HeinApproval.EXECUTE_TIME.Value.ToString().Substring(0, 4));
                xml1.THANG_QT = Convert.ToInt32(data.HeinApproval.EXECUTE_TIME.Value.ToString().Substring(4, 2));

                if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.Kham;
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.NgoaiTru;
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.NoiTru;


                var listIcdCode = data.ListSereServ.Where(s => s.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT).Select(s => s.ICD_CODE).Distinct().ToList();
                if (listIcdCode != null && listIcdCode.Count > 0)
                {
                    xml1.MA_PTTT_QT = string.Join(";", listIcdCode);
                }
                else
                {
                    xml1.MA_PTTT_QT = "";
                }
                //lấy cân nặng với trường hợp là trẻ em dưới 1 tuổi
                string cannang = "";
                var tinhTuoi = Inventec.Common.DateTime.Calculation.DifferenceDate(data.HeinApproval.TDL_PATIENT_DOB, data.Treatment.IN_TIME);
                if (tinhTuoi <= 365 && data.Dhst != null)
                {
                    if (data.Dhst.WEIGHT.HasValue)
                    {
                        cannang = Math.Round(data.Dhst.WEIGHT.Value, 2).ToString("G27", CultureInfo.InvariantCulture);
                    }
                }
                xml1.CAN_NANG = cannang;

                xml1.MA_LK = data.HeinApproval.HEIN_APPROVAL_CODE ?? "";
                xml1.STT = 1;
                xml1.MA_BN = data.HeinApproval.TDL_PATIENT_CODE ?? "";
                xml1.HO_TEN = this.ConvertStringToXmlDocument(data.HeinApproval.TDL_PATIENT_NAME.ToLower());
                if (data.HeinApproval.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    xml1.NGAY_SINH = data.HeinApproval.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                }
                else
                {
                    xml1.NGAY_SINH = data.HeinApproval.TDL_PATIENT_DOB.ToString().Substring(0, 8);
                }
                xml1.GIOI_TINH = data.HeinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE ? 2 : 1;
                xml1.DIA_CHI = this.ConvertStringToXmlDocument(data.HeinApproval.ADDRESS ?? "");
                xml1.MA_THE = data.HeinApproval.HEIN_CARD_NUMBER ?? "";
                xml1.MA_DKBD = data.HeinApproval.HEIN_MEDI_ORG_CODE ?? "";
                xml1.GT_THE_TU = data.HeinApproval.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8);
                xml1.GT_THE_DEN = data.HeinApproval.HEIN_CARD_TO_TIME.ToString().Substring(0, 8);
                xml1.TEN_BENH = this.ConvertStringToXmlDocument(data.Treatment.ICD_NAME ?? "");
                xml1.MA_BENH = data.Treatment.ICD_CODE ?? "";
                xml1.MA_BENHKHAC = string.IsNullOrWhiteSpace(data.Treatment.ICD_SUB_CODE) ? "" : data.Treatment.ICD_SUB_CODE.Trim(';');
                xml1.MA_NOI_CHUYEN = data.Treatment.TRANSFER_IN_MEDI_ORG_CODE ?? "";
                if (data.HeinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT && data.Treatment.CLINICAL_IN_TIME.HasValue)
                {
                    xml1.NGAY_VAO = data.Treatment.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                }
                else
                {
                    xml1.NGAY_VAO = data.Treatment.IN_TIME.ToString().Substring(0, 12);
                }

                if (data.Treatment.OUT_TIME.HasValue)
                {
                    xml1.NGAY_RA = data.Treatment.OUT_TIME.Value.ToString().Substring(0, 12);
                }
                else
                {
                    xml1.NGAY_RA = "";
                }
                xml1.MA_KHOA = data.Treatment.EXIT_BHYT_CODE ?? data.Treatment.END_BHYT_CODE ?? "";
                string maKcbdb = "";
                if (data.Branch != null && !String.IsNullOrEmpty(data.Branch.HEIN_MEDI_ORG_CODE))
                {
                    maKcbdb = data.Branch.HEIN_MEDI_ORG_CODE;
                }
                else if (GlobalConfigStore.Branch != null && !String.IsNullOrEmpty(GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE))
                {
                    maKcbdb = GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE;
                }
                xml1.MA_CSKCB = maKcbdb;
                xml1.MA_KHUVUC = data.HeinApproval.LIVE_AREA_CODE ?? "";
                result = new ResultADO(true, "", new object[] { xml1 });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new ResultADO(false, ex.Message, null);
            }
            return result;
        }

        private int GetDefaultHeinRatioForView(string heinCardNumber, string treatmentTypeCode, string levelCode, string rightRouteCode, decimal virTotalPrice)
        {
            decimal result = 0;
            try
            {
                result = ((new MOS.LibraryHein.Bhyt.BhytHeinProcessor().GetDefaultHeinRatio(treatmentTypeCode, heinCardNumber, levelCode, rightRouteCode, virTotalPrice) ?? 0) * 100);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return (int)result;
        }

        private int GetDefaultHeinRatio(V_HIS_HEIN_APPROVAL approval, decimal virTotalPrice)
        {
            decimal result = 0;
            try
            {
                BhytPatientTypeData data = new BhytPatientTypeData();
                data.HEIN_CARD_FROM_TIME = approval.HEIN_CARD_FROM_TIME;
                data.HEIN_CARD_NUMBER = approval.HEIN_CARD_NUMBER;
                data.HEIN_CARD_TO_TIME = approval.HEIN_CARD_TO_TIME;
                data.HEIN_MEDI_ORG_CODE = approval.HEIN_MEDI_ORG_CODE;
                data.HEIN_MEDI_ORG_NAME = approval.HEIN_MEDI_ORG_NAME;
                data.JOIN_5_YEAR = approval.JOIN_5_YEAR;
                data.LEVEL_CODE = approval.LEVEL_CODE;
                data.LIVE_AREA_CODE = approval.LIVE_AREA_CODE;
                data.PAID_6_MONTH = approval.PAID_6_MONTH;
                data.RIGHT_ROUTE_CODE = approval.RIGHT_ROUTE_CODE;
                data.RIGHT_ROUTE_TYPE_CODE = approval.RIGHT_ROUTE_TYPE_CODE;
                result = ((new MOS.LibraryHein.Bhyt.BhytHeinProcessor().GetDefaultHeinRatio(approval.HEIN_TREATMENT_TYPE_CODE, approval.HEIN_CARD_NUMBER, data, virTotalPrice) ?? 0) * 100);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return (int)result;
        }

        private bool CheckBhytNsd(List<string> listIcdCode, List<string> listIcdCodeTe, V_HIS_TREATMENT_3 hisTreatment, V_HIS_HEIN_APPROVAL hisHeinApprovalBhyt)
        {
            var result = false;
            try
            {
                if ((listIcdCode == null || listIcdCode.Count == 0) && (listIcdCodeTe == null || listIcdCodeTe.Count == 0))
                {
                    return result;
                }
                if (listIcdCode.Contains(hisTreatment.ICD_CODE))
                    result = true;
                else if (!string.IsNullOrEmpty(hisTreatment.ICD_CODE))
                    if (hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && listIcdCodeTe.Contains(hisTreatment.ICD_CODE.Substring(0, 3)))
                        result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
