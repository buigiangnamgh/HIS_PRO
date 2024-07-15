using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace His.Bhyt.ExportXml.XML130.XML7
{
    public class Xml7Processor
    {
        InputXml7ADO inputXmlAdo;

        public Xml7Processor(InputXml7ADO inputXmlAdo)
        {
            this.inputXmlAdo = inputXmlAdo;
        }

        public XML7Data GenerateXml7Data()
        {
            XML7Data result = null;
            try
            {
                XML7ADO ADOXml7 = GenerateXml7ADOData();
                MapADOToXml(ADOXml7, ref result);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        public XML7ADO GenerateXml7ADOData()
        {
            XML7ADO result = null;
            try
            {
                result = ProcessData(this.inputXmlAdo);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        public void MapADOToXml(XML7ADO ado, ref XML7Data data)
        {
            try
            {
                if (ado != null)
                {
                    if (data == null)
                        data = new XML7Data();
                    data.MA_LK = ado.maLienKet;
                    data.SO_LUU_TRU = ado.soLuuTru;
                    data.MA_YTE = ado.maYTe;
                    data.MA_KHOA_RV = ado.maKhoaRV;
                    data.NGAY_VAO = ado.ngayVao;
                    data.NGAY_RA = ado.ngayRa;
                    data.MA_DINH_CHI_THAI = ado.maDinhChiThai;
                    data.NGUYENNHAN_DINHCHI = ado.nguyenNhanDinhChi;
                    data.THOIGIAN_DINHCHI = ado.thoiGianDinhChi;
                    data.TUOI_THAI = ado.tuoiThai.HasValue ? ado.tuoiThai.Value.ToString("G27", CultureInfo.InvariantCulture) : "";
                    data.CHAN_DOAN_RV = ado.chuanDoanRV;
                    data.PP_DIEUTRI = ado.ppDieuTri;
                    data.GHI_CHU = ado.ghiChu;
                    data.MA_TTDV = ado.maTTDV;
                    data.MA_BS = ado.maBS;
                    data.TEN_BS = ado.tenBS;
                    data.NGAY_CT = ado.ngayCT;
                    data.MA_CHA = ado.maCha;
                    data.MA_ME = ado.maMe;
                    data.MA_THE_TAM = ado.maTheTam;
                    data.HO_TEN_CHA = ado.hoTenCha;
                    data.HO_TEN_ME = ado.hoTenMe;
                    data.SO_NGAY_NGHI = ado.soNgayNghi;
                    data.NGOAITRU_TUNGAY = ado.ngoaiTruTuNgay;
                    data.NGOAITRU_DENNGAY = ado.ngoaiTruDenNgay;
                    data.DU_PHONG = ado.duPhong;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                data = null;
            }
        }

        private XML7ADO ProcessData(InputXml7ADO data)
        {
            XML7ADO result = null;
            try
            {
                var treatment = data.Treatment;
                var listTreatmentType = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU,
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU,
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY
                };
                if (listTreatmentType.Contains(data.Treatment.TDL_TREATMENT_TYPE_ID ?? -1)
                    && treatment.TREATMENT_END_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                {
                    string maLienKet = "";
                    string soLuuTru = "";
                    string maYTe = "";
                    string maKhoaRV = "";
                    string ngayVao = "";
                    string ngayRa = "";
                    string maDinhChiThai = "";
                    string nguyenNhanDinhChi = "";
                    string thoiGianDinhChi = "";
                    long? tuoiThai = null;
                    string chanDoanRV = "";
                    string ppDieuTri = "";
                    string ghiChu = "";
                    string maTTDV = "";
                    string maBS = "";
                    string tenBS = "";
                    string ngayCT = "";
                    string maCha = "";
                    string maMe = "";
                    string maTheTam = "";
                    string hoTenCha = "";
                    string hoTenMe = "";
                    int soNgayNghi = 0;
                    string ngoaiTruTuNgay = "";
                    string ngoaiTruDenNgay = "";
                    string DuPhong = "";

                    soLuuTru = !String.IsNullOrEmpty(treatment.END_CODE) ? treatment.END_CODE : "0";
                    maLienKet = treatment.TREATMENT_CODE ?? "";
                    maYTe = treatment.TDL_PATIENT_CODE ?? "";
                    maKhoaRV = treatment.END_DEPARTMENT_BHYT_CODE ?? "";
                    ngayVao = treatment.IN_TIME.ToString().Substring(0, 12);
                    ngayRa = treatment.OUT_TIME != null ? treatment.OUT_TIME.ToString().Substring(0, 12) : "";
                    maDinhChiThai = treatment.IS_PREGNANCY_TERMINATION == 1 ? "1" : "0";
                    nguyenNhanDinhChi = treatment.PREGNANCY_TERMINATION_REASON ?? "";
                    thoiGianDinhChi = treatment.PREGNANCY_TERMINATION_TIME != null ? treatment.PREGNANCY_TERMINATION_TIME.ToString().Substring(0, 12) : "";
                    tuoiThai = treatment.GESTATIONAL_AGE;
                    chanDoanRV = treatment.ICD_NAME ?? "";
                    if (!String.IsNullOrWhiteSpace(chanDoanRV))
                    {
                        chanDoanRV += ";";
                    }
                    chanDoanRV += treatment.ICD_TEXT ?? "";

                    ppDieuTri = !string.IsNullOrWhiteSpace(treatment.TREATMENT_METHOD) ? treatment.TREATMENT_METHOD : ".";
                    if (!string.IsNullOrEmpty(ppDieuTri) && Encoding.UTF8.GetByteCount(ppDieuTri) > 1500)
                    {
                        ppDieuTri = SubStringWithSeparate(ppDieuTri, 1500);
                    }
                    ghiChu = treatment.END_TYPE_EXT_NOTE ?? "";
                    maTTDV = treatment.REPRESENTATIVE_HEIN_CODE ?? "";
                    var bacSi = GetBacSi(treatment.END_HEAD_LOGINNAME, data.Employees);
                    maBS = bacSi != null ? bacSi.SOCIAL_INSURANCE_NUMBER ?? "" : "";
                    tenBS = bacSi != null ? bacSi.TDL_USERNAME : "";
                    ngayCT = treatment.OUT_TIME != null ? treatment.OUT_TIME.ToString().Substring(0, 8) : "";
                    maCha = treatment.FATHER_SOCIAL_INSURANCE_NUMBER ?? "";
                    maMe = treatment.MOTHER_SOCIAL_INSURANCE_NUMBER ?? "";
                    if (data.PatientTypeAlter != null
                        && data.PatientTypeAlter.HAS_BIRTH_CERTIFICATE == MOS.LibraryHein.Bhyt.HeinHasBirthCertificate.HeinHasBirthCertificateCode.TRUE)
                    {
                        maTheTam = treatment.TDL_HEIN_CARD_NUMBER ?? "";
                    }
                    List<string> relativeType_Cha = new List<string>() { "cha", "bố", "bo" };
                    if (!String.IsNullOrWhiteSpace(treatment.FATHER_NAME))
                    {
                        hoTenCha = treatment.FATHER_NAME;
                    }
                    else if (relativeType_Cha.Contains((treatment.TDL_PATIENT_RELATIVE_TYPE ?? "_").ToLower()))
                    {
                        hoTenCha = treatment.TDL_PATIENT_RELATIVE_NAME ?? "";
                    }
                    List<string> relativeType_Me = new List<string>() { "mẹ", "me" };
                    if (!String.IsNullOrWhiteSpace(treatment.MOTHER_NAME))
                    {
                        hoTenMe = treatment.MOTHER_NAME;
                    }
                    else if (relativeType_Me.Contains((treatment.TDL_PATIENT_RELATIVE_TYPE ?? "_").ToLower()))
                    {
                        hoTenMe = treatment.TDL_PATIENT_RELATIVE_NAME ?? "";
                    }
                    if (treatment.OUTPATIENT_DATE_TO.HasValue && treatment.OUTPATIENT_DATE_FROM.HasValue)
                    {
                        TimeSpan outpatientDaysOff = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.OUTPATIENT_DATE_TO.Value).Value.Date - Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.OUTPATIENT_DATE_FROM.Value).Value.Date;
                        soNgayNghi = outpatientDaysOff.Days;
                    }
                    ngoaiTruTuNgay = treatment.OUTPATIENT_DATE_FROM != null ? treatment.OUTPATIENT_DATE_FROM.ToString().Substring(0, 8) : "";
                    ngoaiTruDenNgay = treatment.OUTPATIENT_DATE_TO != null ? treatment.OUTPATIENT_DATE_TO.ToString().Substring(0, 8) : "";

                    result = new XML7ADO();
                    //
                    result.maLienKet = maLienKet;
                    result.soLuuTru = soLuuTru;
                    result.maYTe = maYTe;
                    result.maKhoaRV = maKhoaRV;
                    result.ngayVao = ngayVao;
                    result.ngayRa = ngayRa;
                    result.maDinhChiThai = maDinhChiThai;
                    result.nguyenNhanDinhChi = nguyenNhanDinhChi;
                    result.thoiGianDinhChi = thoiGianDinhChi;
                    result.tuoiThai = tuoiThai;
                    result.chuanDoanRV = chanDoanRV;
                    result.ppDieuTri = ppDieuTri;
                    result.ghiChu = ghiChu;
                    result.maTTDV = maTTDV;
                    result.maBS = maBS;
                    result.tenBS = tenBS;
                    result.ngayCT = ngayCT;
                    result.maCha = maCha;
                    result.maMe = maMe;
                    result.maTheTam = maTheTam;
                    result.hoTenCha = hoTenCha;
                    result.hoTenMe = hoTenMe;
                    result.soNgayNghi = soNgayNghi;
                    result.ngoaiTruTuNgay = ngoaiTruTuNgay;
                    result.ngoaiTruDenNgay = ngoaiTruDenNgay;
                    result.duPhong = DuPhong;
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
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
                        result = dataEmployee.SOCIAL_INSURANCE_NUMBER;
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

        private HIS_EMPLOYEE GetBacSi(string loginName, List<HIS_EMPLOYEE> listEmployees)
        {
            HIS_EMPLOYEE result = null;
            try
            {
                if (String.IsNullOrEmpty(loginName))
                    return result;
                if (listEmployees != null)
                {
                    result = listEmployees.FirstOrDefault(p => p.LOGINNAME == loginName);
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("ListEmployees null");
                }
            }
            catch (Exception ex)
            {
                result = null;
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
