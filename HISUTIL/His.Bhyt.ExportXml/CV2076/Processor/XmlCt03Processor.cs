using ACS.EFMODEL.DataModels;
using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.CV2076.ADO;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.CV2076.Processor
{
    class XmlCt03Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXmlCt03ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                V_HIS_TREATMENT_10 Treatment = data.Treatment2076;
                XmlCt03ADO xmlCt03Ado = null;
                if (Treatment.TREATMENT_END_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON
                    && (!String.IsNullOrWhiteSpace(Treatment.TDL_HEIN_CARD_NUMBER) || !String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER))
                    && (Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU
                    || Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU
                    || Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY))
                {
                    xmlCt03Ado = new XmlCt03ADO();
                    string tenBenh = "";
                    if (!String.IsNullOrWhiteSpace(Treatment.ICD_NAME))
                    {
                        tenBenh = String.Format("{0}({1})", Treatment.ICD_NAME, Treatment.ICD_CODE);
                    }

                    if (!String.IsNullOrWhiteSpace(Treatment.ICD_TEXT))
                    {
                        if (!String.IsNullOrWhiteSpace(Treatment.ICD_SUB_CODE))
                        {
                            if (tenBenh == "")
                                tenBenh += String.Format("{0}({1})", Treatment.ICD_TEXT.Trim(';'), Treatment.ICD_SUB_CODE.Trim(';'));
                            else
                                tenBenh += String.Format(";{0}({1})", Treatment.ICD_TEXT.Trim(';'), Treatment.ICD_SUB_CODE.Trim(';'));
                        }
                        else
                        {
                            if (tenBenh == "")
                                tenBenh += Treatment.ICD_TEXT.Trim(';');
                            else
                                tenBenh += Treatment.ICD_TEXT.Trim(';');
                        }
                    }

                    xmlCt03Ado.ChanDoan = this.SubString(tenBenh, 1500) ?? "";
                    xmlCt03Ado.DiaChi = this.SubString(Treatment.TDL_PATIENT_ADDRESS, 500) ?? "";
                    xmlCt03Ado.DinhChiThaiNgen = "";
                    if (!String.IsNullOrWhiteSpace(Treatment.ADVISE))
                    {

                        xmlCt03Ado.GhiChu = Treatment.ADVISE ?? "";
                    }
                    if (Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        xmlCt03Ado.GioiTinh = "2";
                    }
                    else if (Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        xmlCt03Ado.GioiTinh = "1";
                    }
                    else
                    {
                        xmlCt03Ado.GioiTinh = "3";
                    }
                    xmlCt03Ado.HoTen = Treatment.TDL_PATIENT_NAME ?? "";

                    if (!String.IsNullOrWhiteSpace(Treatment.MOTHER_NAME) || !String.IsNullOrWhiteSpace(Treatment.FATHER_NAME))
                    {
                        xmlCt03Ado.HoTenCha = Treatment.FATHER_NAME ?? "";
                        xmlCt03Ado.HoTenMe = Treatment.MOTHER_NAME ?? "";
                    }
                    else if (!String.IsNullOrWhiteSpace(Treatment.TDL_PATIENT_RELATIVE_NAME))
                    {
                        if ((Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "cha"
                            || (Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "bố"
                            || (Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "bo")
                        {
                            xmlCt03Ado.HoTenCha = Treatment.TDL_PATIENT_RELATIVE_NAME;
                            xmlCt03Ado.HoTenMe = "";
                        }
                        else if ((Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "mẹ"
                            || (Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "me")
                        {
                            xmlCt03Ado.HoTenMe = Treatment.TDL_PATIENT_RELATIVE_NAME;
                            xmlCt03Ado.HoTenCha = "";
                        }
                        else
                        {
                            xmlCt03Ado.HoTenCha = Treatment.TDL_PATIENT_RELATIVE_NAME ?? "";
                            xmlCt03Ado.HoTenMe = "";
                        }
                    }
                    else
                    {
                        xmlCt03Ado.HoTenCha = "";
                        xmlCt03Ado.HoTenMe = "";
                    }
                    if (!String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER))
                    {
                        xmlCt03Ado.MaBhxh = Treatment.TDL_SOCIAL_INSURANCE_NUMBER;
                    }
                    else
                    {
                        xmlCt03Ado.MaBhxh = Treatment.TDL_HEIN_CARD_NUMBER.Substring(5, 10);
                    }
                    xmlCt03Ado.MaThe = Treatment.TDL_HEIN_CARD_NUMBER;

                    HIS_EMPLOYEE employee = null;
                    ACS_USER user = null;
                    if (!String.IsNullOrWhiteSpace(Treatment.END_HEAD_LOGINNAME))
                    {
                        employee = data.Employees != null ? data.Employees.FirstOrDefault(o => o.LOGINNAME == Treatment.END_HEAD_LOGINNAME) : null;
                        user = data.AcsUsers != null ? data.AcsUsers.FirstOrDefault(o => o.LOGINNAME == Treatment.END_HEAD_LOGINNAME) : null;
                    }


                    if (employee != null)
                    {
                        xmlCt03Ado.MaCchnTruongKhoa = employee.DIPLOMA ?? "";

                    }
                    else
                    {
                        xmlCt03Ado.MaCchnTruongKhoa = "";
                    }
                    if (user != null)
                    {
                        xmlCt03Ado.TenTruongKhoa = user.USERNAME ?? "";

                    }
                    else
                    {
                        xmlCt03Ado.TenTruongKhoa = "";
                    }
                    if (!String.IsNullOrWhiteSpace(Treatment.ETHNIC_CODE))
                    {
                        xmlCt03Ado.MaDanToc = Convert.ToInt32(Treatment.ETHNIC_CODE).ToString("G27", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        xmlCt03Ado.MaDanToc = "";
                    }
                    xmlCt03Ado.MaKhoa = Treatment.END_DEPARTMENT_BHYT_CODE ?? "";
                    xmlCt03Ado.MaYte = "";
                    xmlCt03Ado.NgayChungTu = Treatment.OUT_TIME.Value.ToString().Substring(0, 8);
                    xmlCt03Ado.NgayRa = Treatment.OUT_TIME.Value.ToString().Substring(0, 12);
                    xmlCt03Ado.NgayVao = Treatment.IN_TIME.ToString().Substring(0, 12);
                    if (Treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == (short)1)
                    {
                        xmlCt03Ado.NgaySinh = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                    }
                    else
                    {
                        xmlCt03Ado.NgaySinh = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 8);
                    }

                    xmlCt03Ado.NgheNghiep = Treatment.TDL_PATIENT_CAREER_NAME ?? "";
                    if (Treatment.OUTPATIENT_DATE_FROM.HasValue)
                    {
                        xmlCt03Ado.NgoaiTruTuNgay = Treatment.OUTPATIENT_DATE_FROM.ToString().Substring(0, 8);
                    }

                    if (Treatment.OUTPATIENT_DATE_TO.HasValue)
                    {
                        xmlCt03Ado.NgoaiTruDenNgay = Treatment.OUTPATIENT_DATE_TO.ToString().Substring(0, 8);
                    }

                    xmlCt03Ado.PhuongPhapDieuTri = this.SubString(Treatment.TREATMENT_METHOD, 1500) ?? "";
                    xmlCt03Ado.SoLuuTru = "";
                    xmlCt03Ado.ThuTruocDonVi = data.Branch.REPRESENTATIVE ?? "";
                    xmlCt03Ado.TreEmKhongThe = "";
                    xmlCt03Ado.TuoiThai = "";
                }

                rs = new ResultADO(true, "", new object[] { xmlCt03Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        internal void MapADOToXml(XmlCt03ADO ado, ref string content)
        {
            try
            {
                if (ado != null)
                {
                    content = contentXml;

                    content = content.Replace("%HEADER%", this.header_xml);
                    content = content.Replace("%CHANDOAN%", ado.ChanDoan);
                    content = content.Replace("%DIACHI%", ado.DiaChi);
                    content = content.Replace("%DINHCHITHAINGHEN%", ado.DinhChiThaiNgen);
                    content = content.Replace("%GHICHU%", ado.GhiChu);
                    content = content.Replace("%GIOITINH%", ado.GioiTinh);
                    content = content.Replace("%HOTEN%", ado.HoTen);
                    content = content.Replace("%HOTENCHA%", ado.HoTenCha);
                    content = content.Replace("%HOTENME%", ado.HoTenMe);
                    content = content.Replace("%MABHXH%", ado.MaBhxh);
                    content = content.Replace("%MACCHN%", ado.MaCchnTruongKhoa);
                    content = content.Replace("%MADANTOC%", ado.MaDanToc);
                    content = content.Replace("%MAKHOA%", ado.MaKhoa);
                    content = content.Replace("%MATHE%", ado.MaThe);
                    content = content.Replace("%MAYTE%", ado.MaYte);
                    content = content.Replace("%NGAYCHUNGTU%", ado.NgayChungTu);
                    content = content.Replace("%NGAYRA%", ado.NgayRa);
                    content = content.Replace("%NGAYSINH%", ado.NgaySinh);
                    content = content.Replace("%NGAYVAO%", ado.NgayVao);
                    content = content.Replace("%NGHENGHIEP%", ado.NgheNghiep);
                    content = content.Replace("%NGOAITRUTUNGAY%", ado.NgoaiTruTuNgay);
                    content = content.Replace("%NGOAITRUDENNGAY%", ado.NgoaiTruDenNgay);
                    content = content.Replace("%PPDIEUTRI%", ado.PhuongPhapDieuTri);
                    content = content.Replace("%SOLUUTRU%", ado.SoLuuTru);
                    content = content.Replace("%TREEM%", ado.TreEmKhongThe);
                    content = content.Replace("%TENTRUONGKHOA%", ado.TenTruongKhoa);
                    content = content.Replace("%THUTRUONGDONVI%", ado.ThuTruocDonVi);
                    content = content.Replace("%TUOITHAI%", ado.TuoiThai); ;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                content = null;
            }
        }

        private string contentXml = @"<CT03 %HEADER% >
<SO_LUU_TRU>%SOLUUTRU%</SO_LUU_TRU>
<MA_YTE>%MAYTE%</MA_YTE>
<MA_KHOA>%MAKHOA%</MA_KHOA>
<MA_BHXH>%MABHXH%</MA_BHXH>
<MA_THE>%MATHE%</MA_THE>
<HO_TEN><![CDATA[%HOTEN%]]></HO_TEN>
<NGAY_SINH>%NGAYSINH%</NGAY_SINH>
<GIOI_TINH>%GIOITINH%</GIOI_TINH>
<MA_DANTOC>%MADANTOC%</MA_DANTOC>
<NGHE_NGHIEP><![CDATA[%NGHENGHIEP%]]></NGHE_NGHIEP>
<DIA_CHI><![CDATA[%DIACHI%]]></DIA_CHI>
<NGAY_VAO>%NGAYVAO%</NGAY_VAO>
<NGAY_RA>%NGAYRA%</NGAY_RA>
<DINH_CHI_THAI_NGHEN>%DINHCHITHAINGHEN%</DINH_CHI_THAI_NGHEN>
<TUOI_THAI>%TUOITHAI%</TUOI_THAI>
<CHAN_DOAN><![CDATA[%CHANDOAN%]]></CHAN_DOAN>
<PP_DIEUTRI><![CDATA[%PPDIEUTRI%]]></PP_DIEUTRI>
<GHI_CHU><![CDATA[%GHICHU%]]></GHI_CHU>
<THU_TRUONG_DVI><![CDATA[%THUTRUONGDONVI%]]></THU_TRUONG_DVI>
<MA_CCHN_TRUONGKHOA>%MACCHN%</MA_CCHN_TRUONGKHOA>
<TEN_TRUONGKHOA><![CDATA[%TENTRUONGKHOA%]]></TEN_TRUONGKHOA>
<NGAY_CHUNG_TU>%NGAYCHUNGTU%</NGAY_CHUNG_TU>
<TEKT>%TREEM%</TEKT>
<HO_TEN_CHA><![CDATA[%HOTENCHA%]]></HO_TEN_CHA>
<HO_TEN_ME><![CDATA[%HOTENME%]]></HO_TEN_ME>
<NGOAITRU_TUNGAY>%NGOAITRUTUNGAY%</NGOAITRU_TUNGAY>
<NGOAITRU_DENNGAY>%NGOAITRUDENNGAY%</NGOAITRU_DENNGAY>
</CT03>";
    }
}
