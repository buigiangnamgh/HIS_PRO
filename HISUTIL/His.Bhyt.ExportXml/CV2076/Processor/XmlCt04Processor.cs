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
    class XmlCt04Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXmlCt04ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                V_HIS_TREATMENT_10 Treatment = data.Treatment2076;
                XmlCt04ADO xmlCt04Ado = null;
                if ((Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU
                    || Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU
                    || Treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                    && (!String.IsNullOrWhiteSpace(Treatment.TDL_HEIN_CARD_NUMBER) || !String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER)))
                {
                    xmlCt04Ado = new XmlCt04ADO();

                    string tenBenh = "";
                    if (!String.IsNullOrWhiteSpace(Treatment.ICD_NAME))
                    {
                        tenBenh = String.Format("{0}({1})", Treatment.ICD_NAME, Treatment.ICD_CODE);
                    }

                    if (!string.IsNullOrEmpty(Treatment.ICD_TEXT))
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

                    xmlCt04Ado.ChanDoanRa = this.SubString(tenBenh, 1500) ?? "";
                    xmlCt04Ado.ChanDoanVao = this.SubString((Treatment.IN_ICD_NAME ?? Treatment.ICD_NAME), 1500) ?? "";
                    xmlCt04Ado.DiaChi = this.SubString(Treatment.TDL_PATIENT_ADDRESS, 500) ?? "";
                    xmlCt04Ado.GhiChu = "";
                    if (Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        xmlCt04Ado.GioiTinh = "2";
                    }
                    else if (Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        xmlCt04Ado.GioiTinh = "1";
                    }
                    else
                    {
                        xmlCt04Ado.GioiTinh = "3";
                    }
                    xmlCt04Ado.HoTen = Treatment.TDL_PATIENT_NAME ?? "";
                    if (!String.IsNullOrWhiteSpace(Treatment.TDL_PATIENT_RELATIVE_NAME))
                    {
                        if ((Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "cha"
                            || (Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "bố"
                            || (Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "bo")
                        {
                            xmlCt04Ado.HoTenCha = Treatment.TDL_PATIENT_RELATIVE_NAME;
                            xmlCt04Ado.HoTenMe = "";
                            xmlCt04Ado.NguoiGiamHo = "";
                        }
                        else if ((Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "mẹ"
                            || (Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "me")
                        {
                            xmlCt04Ado.HoTenMe = Treatment.TDL_PATIENT_RELATIVE_NAME;
                            xmlCt04Ado.HoTenCha = "";
                            xmlCt04Ado.NguoiGiamHo = "";
                        }
                        else
                        {
                            xmlCt04Ado.NguoiGiamHo = Treatment.TDL_PATIENT_RELATIVE_NAME ?? "";
                            xmlCt04Ado.HoTenCha = "";
                            xmlCt04Ado.HoTenMe = "";
                        }
                    }
                    else
                    {
                        xmlCt04Ado.HoTenCha = "";
                        xmlCt04Ado.HoTenMe = "";
                        xmlCt04Ado.NguoiGiamHo = "";
                    }

                    if (!String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER))
                    {
                        xmlCt04Ado.MaBhxh = Treatment.TDL_SOCIAL_INSURANCE_NUMBER;
                        xmlCt04Ado.MaThe = Treatment.TDL_HEIN_CARD_NUMBER ?? "";
                    }
                    else
                    {
                        xmlCt04Ado.MaBhxh = Treatment.TDL_HEIN_CARD_NUMBER.Substring(5, 10);
                        xmlCt04Ado.MaThe = Treatment.TDL_HEIN_CARD_NUMBER;
                    }
                    xmlCt04Ado.MaChungTu = "";

                    HIS_EMPLOYEE employee = null;
                    if (!String.IsNullOrWhiteSpace(Treatment.END_HEAD_LOGINNAME))
                    {
                        employee = data.Employees != null ? data.Employees.FirstOrDefault(o => o.LOGINNAME == Treatment.END_HEAD_LOGINNAME) : null;
                    }

                    if (!String.IsNullOrWhiteSpace(Treatment.ETHNIC_CODE))
                    {
                        xmlCt04Ado.MaDanToc = Convert.ToInt32(Treatment.ETHNIC_CODE).ToString("G27", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        xmlCt04Ado.MaDanToc = "";
                    }

                    xmlCt04Ado.NgayChetCon = "";
                    xmlCt04Ado.NgayChungTu = Treatment.OUT_TIME.Value.ToString().Substring(0, 8);
                    xmlCt04Ado.NgayRa = Treatment.OUT_TIME.Value.ToString().Substring(0, 12);
                    xmlCt04Ado.NgayVao = Treatment.IN_TIME.ToString().Substring(0, 12);
                    if (Treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == (short)1)
                    {
                        xmlCt04Ado.NgaySinh = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                    }
                    else
                    {
                        xmlCt04Ado.NgaySinh = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 8);
                    }
                    xmlCt04Ado.NgaySinhCon = "";

                    xmlCt04Ado.NgheNghiep = Treatment.TDL_PATIENT_CAREER_NAME ?? "";
                    xmlCt04Ado.NguoiDaiDien = "";
                    xmlCt04Ado.PhuongPhapDieuTri = this.SubString(Treatment.TREATMENT_METHOD, 1500) ?? "";
                    xmlCt04Ado.QuaTrinhBenhLy = this.SubString(Treatment.CLINICAL_NOTE, 1500) ?? "";
                    xmlCt04Ado.SoConChet = "";
                    xmlCt04Ado.SoSeri = "";
                    xmlCt04Ado.TenDonVi = Treatment.TDL_PATIENT_WORK_PLACE_NAME ?? (Treatment.TDL_PATIENT_WORK_PLACE ?? "");
                    xmlCt04Ado.TreEmKhongThe = "";
                    if (Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    {
                        xmlCt04Ado.TinhTrangRaVien = "2";
                    }
                    else if (Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                    {
                        xmlCt04Ado.TinhTrangRaVien = "3";
                    }
                    else if (Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN)
                    {
                        xmlCt04Ado.TinhTrangRaVien = "4";
                    }
                    else
                    {
                        xmlCt04Ado.TinhTrangRaVien = "1";
                    }
                    xmlCt04Ado.TomTatKetQua = this.SubString(Treatment.SUBCLINICAL_RESULT, 1500) ?? "";
                }
                rs = new ResultADO(true, "", new object[] { xmlCt04Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        internal void MapADOToXml(XmlCt04ADO ado, ref string content)
        {
            try
            {
                if (ado != null)
                {
                    content = contentXml;

                    content = content.Replace("%HEADER%", this.header_xml);
                    content = content.Replace("%CHANDOANRA%", ado.ChanDoanRa);
                    content = content.Replace("%CHANDOANVAO%", ado.ChanDoanVao);
                    content = content.Replace("%DIACHI%", ado.DiaChi);
                    content = content.Replace("%GHICHU%", ado.GhiChu);
                    content = content.Replace("%GIOITINH%", ado.GioiTinh);
                    content = content.Replace("%HOTEN%", ado.HoTen);
                    content = content.Replace("%HOTENCHA%", ado.HoTenCha);
                    content = content.Replace("%HOTENME%", ado.HoTenMe);
                    content = content.Replace("%MABHXH%", ado.MaBhxh);
                    content = content.Replace("%MACHUNGTU%", ado.MaChungTu);
                    content = content.Replace("%MADANTOC%", ado.MaDanToc);
                    content = content.Replace("%MATHE%", ado.MaThe);
                    content = content.Replace("%NGAYCHETCON%", ado.NgayChetCon);
                    content = content.Replace("%NGAYCHUNGTU%", ado.NgayChungTu);
                    content = content.Replace("%NGAYRA%", ado.NgayRa);
                    content = content.Replace("%NGAYSINH%", ado.NgaySinh);
                    content = content.Replace("%NGAYVAO%", ado.NgayVao);
                    content = content.Replace("%NGAYSINHCON%", ado.NgaySinhCon);
                    content = content.Replace("%NGHENGHIEP%", ado.NgheNghiep);
                    content = content.Replace("%NGUOIDAIDIEN%", ado.NguoiDaiDien);
                    content = content.Replace("%NGUOIGIAMHO%", ado.NguoiGiamHo);
                    content = content.Replace("%PPDIEUTRI%", ado.PhuongPhapDieuTri);
                    content = content.Replace("%QUATRINHBENHLY%", ado.QuaTrinhBenhLy);
                    content = content.Replace("%SOCONCHET%", ado.SoConChet);
                    content = content.Replace("%SOSERI%", ado.SoSeri);
                    content = content.Replace("%TREEM%", ado.TreEmKhongThe);
                    content = content.Replace("%TENDONVI%", ado.TenDonVi);
                    content = content.Replace("%TOMTATKETQUA%", ado.TomTatKetQua);
                    content = content.Replace("%TINHTRANGRAVIEN%", ado.TinhTrangRaVien);

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                content = null;
            }
        }

        private string contentXml = @"<CT04 %HEADER% >
<MA_CT>%MACHUNGTU%</MA_CT>
<SO_SERI>%SOSERI%</SO_SERI>
<MA_BHXH>%MABHXH%</MA_BHXH>
<MA_THE>%MATHE%</MA_THE>
<HO_TEN><![CDATA[%HOTEN%]]></HO_TEN>
<NGAY_SINH>%NGAYSINH%</NGAY_SINH>
<GIOI_TINH>%GIOITINH%</GIOI_TINH>
<MA_DANTOC>%MADANTOC%</MA_DANTOC>
<DIA_CHI><![CDATA[%DIACHI%]]></DIA_CHI>
<NGHE_NGHIEP><![CDATA[%NGHENGHIEP%]]></NGHE_NGHIEP>
<HO_TEN_CHA><![CDATA[%HOTENCHA%]]></HO_TEN_CHA>
<HO_TEN_ME><![CDATA[%HOTENME%]]></HO_TEN_ME>
<NGUOI_GIAM_HO><![CDATA[%NGUOIGIAMHO%]]></NGUOI_GIAM_HO>
<TEN_DONVI><![CDATA[%TENDONVI%]]></TEN_DONVI>
<NGAY_VAO>%NGAYVAO%</NGAY_VAO>
<NGAY_RA>%NGAYRA%</NGAY_RA>
<CHAN_DOAN_VAO><![CDATA[%CHANDOANVAO%]]></CHAN_DOAN_VAO>
<CHAN_DOAN_RA><![CDATA[%CHANDOANRA%]]></CHAN_DOAN_RA>
<QT_BENHLY><![CDATA[%QUATRINHBENHLY%]]></QT_BENHLY>
<TOMTAT_KQ><![CDATA[%TOMTATKETQUA%]]></TOMTAT_KQ>
<PP_DIEUTRI><![CDATA[%PPDIEUTRI%]]></PP_DIEUTRI>
<NGAY_SINHCON>%NGAYSINHCON%</NGAY_SINHCON>
<NGAY_CHETCON>%NGAYCHETCON%</NGAY_CHETCON>
<SO_CONCHET>%SOCONCHET%</SO_CONCHET>
<TT_RAVIEN>%TINHTRANGRAVIEN%</TT_RAVIEN>
<GHI_CHU><![CDATA[%GHICHU%]]></GHI_CHU>
<NGUOI_DAI_DIEN><![CDATA[%NGUOIDAIDIEN%]]></NGUOI_DAI_DIEN>
<NGAY_CT>%NGAYCHUNGTU%</NGAY_CT>
<TEKT>%TREEM%</TEKT>
</CT04>";
    }
}
