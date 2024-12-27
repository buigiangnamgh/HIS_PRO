using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.QD4210.ADO;
using His.Bhyt.ExportXml.QD4210.XML.XML1;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace His.Bhyt.ExportXml.QD4210.Processor
{
    class Xml1Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXml1Data(InputADO data, List<Xml2ADO> listXmlThuocAdo, List<Xml3ADO> listXmlDvktVt)
        {
            ResultADO result = null;
            try
            {
                string tenBenhOption = "";
                string IsTreatmentDayCount6556 = "";
                string transferOption = "";
                string addressOptionCFG = "";
                string ngayVaoNoiTruOptionCFG = "";
                string vneIdOption = "";

                if (data.ConfigData != null && data.ConfigData.Count > 0)
                {
                    tenBenhOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.TEN_BENH_OPTION);
                    IsTreatmentDayCount6556 = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.IS_TREATMENT_DAY_COUNT_6556);
                    transferOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.TransferOptionCFG);
                    addressOptionCFG = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.AddressOptionCFG);
                    ngayVaoNoiTruOptionCFG = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.XML__4210__NGAY_VAO_NOI_TRU_OPTION);
                    vneIdOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.XML__4210__VNEID_OPTION);
                }
                else
                {
                    tenBenhOption = data.TenBenhOption;
                    IsTreatmentDayCount6556 = data.IsTreatmentDayCount6556;
                }


                XML1Data xml1 = new XML1Data();

                if (data.HeinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                {
                    string province = !String.IsNullOrWhiteSpace(data.HeinApproval.HEIN_MEDI_ORG_CODE) ? data.HeinApproval.HEIN_MEDI_ORG_CODE.Substring(0, 2) : "";
                    var mediOrg = GlobalConfigStore.HisHeinMediOrg.FirstOrDefault(o => o.MEDI_ORG_CODE == data.HeinApproval.HEIN_MEDI_ORG_CODE);

                    if (data.HeinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        xml1.MA_LYDO_VVIEN = 2;
                    else if (data.HeinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.OVER)
                        xml1.MA_LYDO_VVIEN = 4;
                    else if (data.HeinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.PRESENT)
                        xml1.MA_LYDO_VVIEN = 1;
                    else if (!String.IsNullOrWhiteSpace(data.HeinApproval.HEIN_MEDI_ORG_CODE) &&
                        (data.HeinApproval.HEIN_MEDI_ORG_CODE == data.Branch.HEIN_MEDI_ORG_CODE
                        || (!String.IsNullOrWhiteSpace(data.Branch.ACCEPT_HEIN_MEDI_ORG_CODE) && data.Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(data.HeinApproval.HEIN_MEDI_ORG_CODE))
                        ))
                        xml1.MA_LYDO_VVIEN = 1;
                    else if (data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT || data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE
                        )
                    {
                        xml1.MA_LYDO_VVIEN = 3;
                        if (province == data.Branch.HEIN_PROVINCE_CODE && mediOrg != null && (mediOrg.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT || mediOrg.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE))
                        {
                            xml1.MA_LYDO_VVIEN = 4;
                        }
                    }
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

                if (IsTreatmentDayCount6556 == "1")
                {
                    xml1.SO_NGAY_DTRI = (HIS.Common.Treatment.Calculation.DayOfTreatment6556(data.Treatment.IN_TIME, data.Treatment.CLINICAL_IN_TIME, data.Treatment.OUT_TIME, data.Treatment.TDL_TREATMENT_TYPE_ID ?? 0) ?? 0).ToString();
                }
                else
                {
                    if (data.Treatment.TREATMENT_DAY_COUNT.HasValue)
                    {
                        xml1.SO_NGAY_DTRI = Convert.ToInt64(data.Treatment.TREATMENT_DAY_COUNT.Value).ToString();
                    }
                    else
                    {
                        if (data.HeinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT ||
                            data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)//them so ngay dt voi bn ngoai tru
                        {
                            if (data.Treatment.CLINICAL_IN_TIME.HasValue && data.Treatment.OUT_TIME.HasValue)
                            {
                                xml1.SO_NGAY_DTRI = HIS.Common.Treatment.Calculation.DayOfTreatment(data.Treatment.CLINICAL_IN_TIME,
                                    data.Treatment.OUT_TIME, data.Treatment.TREATMENT_END_TYPE_ID,
                                    data.Treatment.TREATMENT_RESULT_ID, HIS.Common.Treatment.PatientTypeEnum.TYPE.BHYT) + "";
                            }
                            else
                            {
                                xml1.SO_NGAY_DTRI = "0";
                            }
                        }
                        else
                        {
                            xml1.SO_NGAY_DTRI = "0";
                        }
                    }
                }

                if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.Khoi;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.Do;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.KhongThayDoi;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.NangHon;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET||data.Treatment.TREATMENT_RESULT_ID == 8)
                    xml1.KET_QUA_DTRI = TreatmentResultBhytCFG.TuVong;

                if (data.Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    xml1.TINH_TRANG_RV = TreatmentEndTypeBhytCFG.ChuyenVien;
                else if (data.Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                    xml1.TINH_TRANG_RV = TreatmentEndTypeBhytCFG.TronVien;
                else if (data.Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN)
                    xml1.TINH_TRANG_RV = TreatmentEndTypeBhytCFG.XinRaVien;
                else if (data.Treatment.TREATMENT_END_TYPE_ID.HasValue)
                    xml1.TINH_TRANG_RV = TreatmentEndTypeBhytCFG.RaVien;

                decimal tongChi = 0;
                decimal tongBNTT = 0;
                decimal tongBNCCT = 0;
                decimal tongBHTT = 0;
                decimal tongNguonKhac = 0;
                decimal tongNgoaiDs = 0;
                decimal tienThuoc = 0;
                if (listXmlThuocAdo != null && listXmlThuocAdo.Count > 0)
                {
                    foreach (var xml2 in listXmlThuocAdo)
                    {
                        tongChi += Math.Round(xml2.ThanhTien, 2);
                        tienThuoc += Math.Round(xml2.ThanhTien, 2);
                        tongBNTT += Math.Round(xml2.TongBNTT, 2);
                        tongBNCCT += Math.Round(xml2.TongBNCCT, 2);
                        tongBHTT += Math.Round(xml2.TongBHTT, 2);
                        tongNguonKhac += Math.Round(xml2.TongNguonKhac, 2);
                        tongNgoaiDs += Math.Round(xml2.TongNgoaiDS, 2);
                    }
                }

                decimal tientVTYT = 0;
                if (listXmlDvktVt != null && listXmlDvktVt.Count > 0)
                {
                    foreach (var xml3 in listXmlDvktVt)
                    {
                        tongChi += Math.Round(xml3.ThanhTien, 2);
                        if (xml3.IsMaterial)
                            tientVTYT += Math.Round(xml3.ThanhTien, 2);
                        tongBNTT += Math.Round(xml3.TongBNTT, 2);
                        tongBNCCT += Math.Round(xml3.TongBNCCT, 2);
                        tongBHTT += Math.Round(xml3.TongBHTT, 2);
                        tongNguonKhac += Math.Round(xml3.TongNguonKhac, 2);
                        tongNgoaiDs += Math.Round(xml3.TongNgoaiDS, 2);
                    }
                }

                xml1.T_THUOC = Math.Round(tienThuoc, 2).ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_VTYT = Math.Round(tientVTYT, 2).ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_TONGCHI = Math.Round(tongChi, 2).ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_BNTT = tongBNTT.ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_BHTT = tongBHTT.ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_BNCCT = tongBNCCT.ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_NGUONKHAC = tongNguonKhac.ToString("G27", CultureInfo.InvariantCulture);
                xml1.T_NGOAIDS = tongNgoaiDs.ToString("G27", CultureInfo.InvariantCulture);

                if (data.Treatment.FEE_LOCK_TIME.HasValue)
                {
                    xml1.NGAY_TTOAN = data.Treatment.FEE_LOCK_TIME.Value.ToString().Substring(0, 12);
                }
                else
                {
                    xml1.NGAY_TTOAN = "";
                }

                if (data.HeinApproval.EXECUTE_TIME.HasValue)
                {
                    xml1.NAM_QT = Convert.ToInt32(data.HeinApproval.EXECUTE_TIME.Value.ToString().Substring(0, 4));
                    xml1.THANG_QT = Convert.ToInt32(data.HeinApproval.EXECUTE_TIME.Value.ToString().Substring(4, 2));
                }
                else
                {
                    xml1.NAM_QT = DateTime.Now.Year;
                    xml1.THANG_QT = DateTime.Now.Month;
                }
                if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.Kham;
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.NgoaiTru;
                else if ((ngayVaoNoiTruOptionCFG == "1" || ngayVaoNoiTruOptionCFG == "2")
                    && data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU
                    && data.Treatment.OUT_TIME.HasValue && data.Treatment.CLINICAL_IN_TIME.HasValue
                    && Inventec.Common.DateTime.Calculation.DifferenceTime(data.Treatment.CLINICAL_IN_TIME.Value, data.Treatment.OUT_TIME.Value, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.HOUR) < 4)
                {
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.NoiTru_Tu4GioTroXuong;
                    if (ngayVaoNoiTruOptionCFG == "1")
                        xml1.NGAY_VAO_NOI_TRU = data.Treatment.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                }
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.NoiTru;
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__NHANTHUOC)
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.NhanThuoc;
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__TYTXA)
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.Tyt;

                var listIcdCode = data.ListSereServ.Where(s => s.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || s.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT).Select(s => s.ICD_CODE).Distinct().ToList();
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
                var tinhTuoi = Inventec.Common.DateTime.Calculation.DifferenceDate(data.Treatment.TDL_PATIENT_DOB, data.Treatment.IN_TIME);
                if (tinhTuoi <= 365 && data.Dhst != null)
                {
                    if (data.Dhst.WEIGHT.HasValue)
                    {
                        cannang = Math.Round(data.Dhst.WEIGHT.Value, 2).ToString("G27", CultureInfo.InvariantCulture);
                    }
                }

                xml1.CAN_NANG = cannang;
                xml1.MA_LK = data.Treatment.TREATMENT_CODE ?? "";
                xml1.STT = 1;
                xml1.MA_BN = data.Treatment.TDL_PATIENT_CODE ?? "";
                xml1.HO_TEN = this.ConvertStringToXmlDocument(data.Treatment.TDL_PATIENT_NAME.ToLower());
                //if (data.HeinApproval.HAS_NOT_DAY_DOB == IMSys.DbConfig.HIS_RS.HIS_PATIENT.HAS_NOT_DAY_DOB__TRUE)
                //{
                //    xml1.NGAY_SINH = data.HeinApproval.DOB.ToString().Substring(0, 4);
                //}
                //else
                //{
                //xml1.NGAY_SINH = data.Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 8);
                //}
                if (data.Treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == 1)
                {
                    xml1.NGAY_SINH = data.Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4) + "0101";
                }
                else
                {
                    xml1.NGAY_SINH = data.Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 8);
                }
                xml1.GIOI_TINH = data.Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE ? 2 : 1;

                V_HIS_SERE_SERV_2 ssMainExam = data.ListSereServ != null && data.ListSereServ.Count > 0 ? (data.ListSereServ.FirstOrDefault(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && o.TDL_IS_MAIN_EXAM == 1) ?? data.ListSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).OrderBy(o=>o.TDL_INTRUCTION_TIME).FirstOrDefault()) : null;

                string diachi = "";
                if (addressOptionCFG == "1" && !String.IsNullOrWhiteSpace(data.Treatment.TDL_PATIENT_ADDRESS))
                {
                    diachi = data.Treatment.TDL_PATIENT_ADDRESS ?? "";
                }
                else
                {
                    diachi = data.HeinApproval.ADDRESS ?? "";
                }
                if (vneIdOption != "1")
                {
                    xml1.DIA_CHI = this.ConvertStringToXmlDocument(SubString(diachi, 1024));
                }
                else
                {
                    contentDiaChi = contentDiaChi.Replace("%DIACHI%", SubString(diachi, 1024));
                    contentDiaChi = contentDiaChi.Replace("%NHOMMAUABO%", SubString(data.Treatment.TDL_PATIENT_BLOOD_ABO_CODE ?? "", 255));
                    //contentDiaChi = contentDiaChi.Replace("%NHOMMAUABO%", "");
                    if (data.Dhst != null && data.Dhst.HEIGHT != null)
                        contentDiaChi = contentDiaChi.Replace("%CHIEUCAO%", ((int)data.Dhst.HEIGHT).ToString());
                    else
                        contentDiaChi = contentDiaChi.Replace("%CHIEUCAO%", "");
                    contentDiaChi = contentDiaChi.Replace("%LYDODENKHAM%", SubString(data.Treatment.HOSPITALIZATION_REASON ?? "", 2000));
                    contentDiaChi = contentDiaChi.Replace("%HUONGDIEUTRI%", SubString(data.Treatment.TREATMENT_DIRECTION ?? data.Treatment.ADVISE ?? "", 2000));
                    contentDiaChi = contentDiaChi.Replace("%TTPPDT%", SubString(ssMainExam != null ? (ssMainExam.TREATMENT_INSTRUCTION ?? "") : "", 2000));
                    contentDiaChi = contentDiaChi.Replace("%GHICHU%", SubString(data.HeinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM ? (ssMainExam != null ? (ssMainExam.NOTE ?? "") : "") : data.Treatment.ADVISE ?? "", 2000));
                    //contentDiaChi = contentDiaChi.Replace("%GHICHU%", SubString(data.Treatment.APPROVE_FINISH_NOTE ?? "", 2000));
                    xml1.DIA_CHI = this.ConvertStringToXmlDocument(EncodeBase64(Encoding.UTF8, contentDiaChi));
                }
                xml1.MA_THE = data.HeinApproval.HEIN_CARD_NUMBER ?? "";
                xml1.MA_DKBD = data.HeinApproval.HEIN_MEDI_ORG_CODE ?? "";
                xml1.GT_THE_TU = data.HeinApproval.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8);
                xml1.GT_THE_DEN = data.HeinApproval.HEIN_CARD_TO_TIME.ToString().Substring(0, 8);
                if (data.HeinApprovals != null && data.HeinApprovals.Count > 1)
                {
                    //sap xep the theo thu tu han den cua the de tranh viec day nhieu lan bi tao ra nhieu dong tren cong bhyt.
                    List<V_HIS_HEIN_APPROVAL> heinApprovals = new List<V_HIS_HEIN_APPROVAL>();
                    heinApprovals = data.HeinApprovals.OrderBy(d => d.HEIN_CARD_TO_TIME).ToList();
                    //the dau tien lay theo the luu trong treatment                    

                    List<Base.BhytCardADO> listCard = new List<BhytCardADO>();
                    foreach (var item in heinApprovals)
                    {
                        Base.BhytCardADO ado = new BhytCardADO();
                        ado.HeinCard = item.HEIN_CARD_NUMBER;
                        ado.MediOrg = item.HEIN_MEDI_ORG_CODE;
                        ado.TimeFrom = item.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8);
                        ado.TimeTo = item.HEIN_CARD_TO_TIME.ToString().Substring(0, 8);
                        listCard.Add(ado);
                    }

                    listCard = listCard.Distinct().ToList();

                    var listHeinCard = listCard.Select(s => s.HeinCard).ToList();
                    var listMediOrg = listCard.Select(s => s.MediOrg).ToList();
                    var listTimeFrom = listCard.Select(s => s.TimeFrom).ToList();
                    var listTimeTo = listCard.Select(s => s.TimeTo).ToList();

                    if (listHeinCard != null && listHeinCard.Count > 0)
                    {
                        xml1.MA_THE = String.Join(";", listHeinCard);
                    }

                    if (listMediOrg != null && listMediOrg.Count > 0)
                    {
                        xml1.MA_DKBD = String.Join(";", listMediOrg);
                    }

                    if (listTimeFrom != null && listTimeFrom.Count > 0)
                    {
                        xml1.GT_THE_TU = String.Join(";", listTimeFrom);
                    }

                    if (listTimeTo != null && listTimeTo.Count > 0)
                    {
                        xml1.GT_THE_DEN = String.Join(";", listTimeTo);
                    }
                }

                xml1.MIEN_CUNG_CT = "";
                if (data.HeinApproval.FREE_CO_PAID_TIME.HasValue)
                {
                    xml1.MIEN_CUNG_CT = data.HeinApproval.FREE_CO_PAID_TIME.Value.ToString().Substring(0, 8);
                }

                string tenBenh = data.Treatment.ICD_NAME ?? "";
                if (tenBenhOption == "2")
                {
                    List<string> lstTenBenh = new List<string>();
                    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_NAME))
                    {
                        lstTenBenh.Add(data.Treatment.ICD_NAME);
                    }

                    if (!string.IsNullOrEmpty(data.Treatment.ICD_TEXT))
                    {
                        var icdname = data.Treatment.ICD_TEXT.Split(';');
                        icdname = icdname.Where(o => !String.IsNullOrWhiteSpace(o)).ToArray();
                        if (icdname != null && icdname.Count() > 0)
                        {
                            lstTenBenh.AddRange(icdname);
                        }
                    }

                    var reqIcd = data.ListSereServ.GroupBy(g => g.SERVICE_REQ_ID ?? 0).ToList();
                    foreach (var item in reqIcd)
                    {
                        if (!String.IsNullOrWhiteSpace(item.First().ICD_NAME))
                        {
                            lstTenBenh.Add(item.First().ICD_NAME);
                        }

                        if (!String.IsNullOrWhiteSpace(item.First().ICD_TEXT))
                        {
                            var icdname = item.First().ICD_TEXT.Split(';');
                            icdname = icdname.Where(o => !String.IsNullOrWhiteSpace(o)).ToArray();
                            if (icdname != null && icdname.Count() > 0)
                            {
                                lstTenBenh.AddRange(icdname);
                            }
                        }
                    }

                    if (lstTenBenh != null && lstTenBenh.Count > 0)
                    {
                        tenBenh = string.Join(";", lstTenBenh.Distinct().ToList());
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(data.Treatment.ICD_TEXT))
                    {
                        if (tenBenh == "")
                            tenBenh += data.Treatment.ICD_TEXT.Trim(';');
                        else
                            tenBenh += ";" + data.Treatment.ICD_TEXT.Trim(';');
                    }
                }

                xml1.TEN_BENH = this.ConvertStringToXmlDocument(tenBenh);
                xml1.MA_BENH = data.Treatment.ICD_CODE ?? "";
                string mabenhKhac = "";
                if (!String.IsNullOrEmpty(data.Treatment.ICD_SUB_CODE))
                {
                    mabenhKhac = data.Treatment.ICD_SUB_CODE.Trim(';');
                }

                xml1.MA_BENHKHAC = mabenhKhac;
                xml1.MA_NOI_CHUYEN = data.Treatment.TRANSFER_IN_MEDI_ORG_CODE ?? "";
                //if (data.HeinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT && data.Treatment.CLINICAL_IN_TIME.HasValue)
                //{
                //    xml1.NGAY_VAO = data.Treatment.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                //}
                //else
                //{
                xml1.NGAY_VAO = data.Treatment.IN_TIME.ToString().Substring(0, 12);
                //}

                if (data.Treatment.OUT_TIME.HasValue)
                {
                    xml1.NGAY_RA = data.Treatment.OUT_TIME.Value.ToString().Substring(0, 12);
                }
                else
                {
                    xml1.NGAY_RA = "";
                }

                if (!string.IsNullOrWhiteSpace(data.Treatment.END_ROOM_BHYT_CODE))
                {
                    xml1.MA_KHOA = data.Treatment.END_ROOM_BHYT_CODE;
                }
                else if (!string.IsNullOrWhiteSpace(data.Treatment.EXIT_BHYT_CODE))
                {
                    xml1.MA_KHOA = data.Treatment.EXIT_BHYT_CODE;
                }
                else
                {
                    xml1.MA_KHOA = data.Treatment.END_BHYT_CODE ?? "";
                }

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

                //icd lao tiềm ẩn
                if (Base.Checker.TreatmentChecker.IsLatentTuberculosis(data.TotalIcdData, data.Treatment.ICD_CODE)
                    && !String.IsNullOrWhiteSpace(data.Treatment.TUBERCULOSIS_ISSUED_ORG_NAME)
                    && data.Treatment.TUBERCULOSIS_ISSUED_DATE > 0)
                {
                    xml1.MA_LYDO_VVIEN = 7;
                    xml1.MA_LOAI_KCB = TreatmentTypeBhytCFG.NhanThocLao;

                    string icdName = string.Format("Cấp thuốc theo Giấy xác nhận điều trị nội trú của cơ sở khám bệnh, chữa bệnh {0} {1}", data.Treatment.TUBERCULOSIS_ISSUED_ORG_NAME, Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(data.Treatment.TUBERCULOSIS_ISSUED_DATE ?? 0));
                    xml1.TEN_BENH = this.ConvertStringToXmlDocument(icdName);
                }

                if (transferOption == "1")
                {
                    //XML1DataPlus xml1Plus = new XML1DataPlus();
                    AutoMapper.Mapper.CreateMap<XML1Data, XML1DataPlus>();
                    XML1DataPlus xml1Plus = AutoMapper.Mapper.Map<XML1DataPlus>(xml1);
                    //Inventec.Common.Mapper.DataObjectMapper.Map<XML1DataPlus>(xml1Plus, xml1);

                    xml1Plus.MA_NOI_DEN = data.Treatment.MEDI_ORG_CODE ?? "";
                    xml1Plus.MA_NOI_DI = data.Treatment.TRANSFER_IN_MEDI_ORG_CODE ?? "";

                    result = new ResultADO(true, "", new object[] { xml1Plus });
                }
                else
                {
                    result = new ResultADO(true, "", new object[] { xml1 });
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new ResultADO(false, ex.Message, null);
            }
            return result;
        }

        private string contentDiaChi = @"<TT_BOSUNG_4210>
  <DIA_CHI><![CDATA[%DIACHI%]]></DIA_CHI>
  <NHOM_MAU_ABO><![CDATA[%NHOMMAUABO%]]></NHOM_MAU_ABO>
  <CHIEU_CAO>%CHIEUCAO%</CHIEU_CAO>
  <LY_DO_DEN_KHAM><![CDATA[%LYDODENKHAM%]]></LY_DO_DEN_KHAM>
  <HUONG_DIEU_TRI><![CDATA[%HUONGDIEUTRI%]]></HUONG_DIEU_TRI>
  <TT_PP_DT><![CDATA[%TTPPDT%]]></TT_PP_DT>
  <GHI_CHU><![CDATA[%GHICHU%]]></GHI_CHU>
</TT_BOSUNG_4210>";
    }
}
