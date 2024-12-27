using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.CV2076.ADO;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.CV2076.Processor
{
    class XmlCt07Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXmlCt07ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                V_HIS_TREATMENT_10 Treatment = data.Treatment2076;
                XmlCt07ADO xmlCt07Ado = null;
                if (Treatment.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_OM
                    && (!String.IsNullOrWhiteSpace(Treatment.SICK_HEIN_CARD_NUMBER) || !String.IsNullOrWhiteSpace(Treatment.TDL_HEIN_CARD_NUMBER) || !String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER)))
                {
                    xmlCt07Ado = new XmlCt07ADO();

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

                    if (!String.IsNullOrWhiteSpace(Treatment.TREATMENT_METHOD))
                    {
                        tenBenh = String.Format("{0} _ {1}", tenBenh, Treatment.TREATMENT_METHOD);
                    }
                    xmlCt07Ado.ChanDoanDieuTri = this.SubString(tenBenh, 1500);
                    xmlCt07Ado.HoTen = Treatment.TDL_PATIENT_NAME ?? "";
                    if (Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        xmlCt07Ado.GioiTinh = "2";
                    }
                    else if (Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        xmlCt07Ado.GioiTinh = "1";
                    }
                    else
                    {
                        xmlCt07Ado.GioiTinh = "3";
                    }

                    DateTime dtDob = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(Treatment.TDL_PATIENT_DOB).Value;
                    if (dtDob != DateTime.MinValue && MOS.LibraryHein.Bhyt.BhytPatientTypeData.IsChild(dtDob))
                    {
                        if ((Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "cha"
                            || (Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "bố"
                            || (Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "bo")
                        {
                            xmlCt07Ado.HoTenCha = Treatment.TDL_PATIENT_RELATIVE_NAME;
                            xmlCt07Ado.HoTenMe = "";
                        }
                        else if ((Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "mẹ"
                            || (Treatment.TDL_PATIENT_RELATIVE_TYPE ?? "").ToLower() == "me")
                        {
                            xmlCt07Ado.HoTenMe = Treatment.TDL_PATIENT_RELATIVE_NAME;
                            xmlCt07Ado.HoTenCha = "";
                        }
                        else
                        {
                            xmlCt07Ado.HoTenCha = Treatment.TDL_PATIENT_RELATIVE_NAME ?? "";
                            xmlCt07Ado.HoTenMe = "";
                        }
                    }

                    HIS_EMPLOYEE employee = null;
                    if (!String.IsNullOrWhiteSpace(Treatment.SICK_LOGINNAME))
                    {
                        employee = data.Employees != null ? data.Employees.FirstOrDefault(o => o.LOGINNAME == Treatment.SICK_LOGINNAME) : null;
                    }

                    if (employee != null)
                    {
                        xmlCt07Ado.MaChungChiHanhNghe = employee.DIPLOMA ?? "";
                    }
                    else
                    {
                        xmlCt07Ado.MaChungChiHanhNghe = "";
                    }
                    if (!String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER))
                    {
                        xmlCt07Ado.MaBhxh = Treatment.TDL_SOCIAL_INSURANCE_NUMBER;
                        xmlCt07Ado.MaThe = Treatment.SICK_HEIN_CARD_NUMBER ?? (Treatment.TDL_HEIN_CARD_NUMBER ?? "");
                    }
                    else if (!String.IsNullOrWhiteSpace(Treatment.SICK_HEIN_CARD_NUMBER))
                    {
                        xmlCt07Ado.MaBhxh = Treatment.SICK_HEIN_CARD_NUMBER.Substring(5, 10);
                        xmlCt07Ado.MaThe = Treatment.SICK_HEIN_CARD_NUMBER;
                    }
                    else
                    {
                        xmlCt07Ado.MaBhxh = Treatment.TDL_HEIN_CARD_NUMBER.Substring(5, 10);
                        xmlCt07Ado.MaThe = Treatment.TDL_HEIN_CARD_NUMBER;
                    }
                    xmlCt07Ado.MaChungTu = "";
                    xmlCt07Ado.NgayChungTu = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.Ct07OptionNgayChungTu) == "1" ? Treatment.IN_TIME.ToString().Substring(0, 8) : Treatment.OUT_TIME.Value.ToString().Substring(0, 8);
                    if (Treatment.SICK_LEAVE_TO.HasValue)
                    {
                        xmlCt07Ado.DenNgay = Treatment.SICK_LEAVE_TO.Value.ToString().Substring(0, 8);
                    }
                    else
                    {
                        xmlCt07Ado.DenNgay = "";
                    }
                    if (Treatment.SICK_LEAVE_FROM.HasValue)
                    {
                        xmlCt07Ado.TuNgay = Treatment.SICK_LEAVE_FROM.Value.ToString().Substring(0, 8);
                    }
                    else
                    {
                        xmlCt07Ado.TuNgay = "";
                    }

                    if (Treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == (short)1)
                    {
                        xmlCt07Ado.NgaySinh = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                    }
                    else
                    {
                        xmlCt07Ado.NgaySinh = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 8);
                    }
                    xmlCt07Ado.MauSo = "CT07";
                    xmlCt07Ado.SoKhamChuaBenh = Treatment.TREATMENT_CODE;
                    xmlCt07Ado.SoSeri = "";
                    xmlCt07Ado.TenNguoiHanhNghe = Treatment.SICK_USERNAME ?? "";
                    xmlCt07Ado.DonVi = Treatment.TDL_PATIENT_WORK_PLACE ?? (Treatment.TDL_PATIENT_WORK_PLACE_NAME ?? "");
                    xmlCt07Ado.ThuTruongDonVi = data.Branch.REPRESENTATIVE ?? "";
                    xmlCt07Ado.TreEmKhongThe = "0";
                }
                rs = new ResultADO(true, "", new object[] { xmlCt07Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        internal void MapADOToXml(XmlCt07ADO ado, ref string content)
        {
            try
            {
                if (ado != null)
                {
                    content = contentXml;

                    content = content.Replace("%HEADER%", this.header_xml);
                    content = content.Replace("%CHANDOANDIEUTRI%", ado.ChanDoanDieuTri);
                    content = content.Replace("%DENNGAY%", ado.DenNgay);
                    content = content.Replace("%DONVI%", ado.DonVi);
                    content = content.Replace("%GIOITINH%", ado.GioiTinh);
                    content = content.Replace("%HOTEN%", ado.HoTen);
                    content = content.Replace("%HOTENCHA%", ado.HoTenCha);
                    content = content.Replace("%HOTENME%", ado.HoTenMe);
                    content = content.Replace("%MABHXH%", ado.MaBhxh);
                    content = content.Replace("%MACCHN%", ado.MaChungChiHanhNghe);
                    content = content.Replace("%MACHUNGTU%", ado.MaChungTu);
                    content = content.Replace("%MATHE%", ado.MaThe);
                    content = content.Replace("%MAUSO%", ado.MauSo);
                    content = content.Replace("%NGAYCHUNGTU%", ado.NgayChungTu);
                    content = content.Replace("%NGAYSINH%", ado.NgaySinh);
                    content = content.Replace("%SOKCB%", ado.SoKhamChuaBenh);
                    content = content.Replace("%SOSERI%", ado.SoSeri);
                    content = content.Replace("%TREEM%", ado.TreEmKhongThe);
                    content = content.Replace("%TENNGUOIHANHNGHE%", ado.TenNguoiHanhNghe);
                    content = content.Replace("%THUTRUONGDONVI%", ado.ThuTruongDonVi);
                    content = content.Replace("%TUNGAY%", ado.TuNgay);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                content = null;
            }
        }

        private string contentXml = @"<CT07 %HEADER% >
<MA_CT>%MACHUNGTU%</MA_CT>
<SO_SERI>%SOSERI%</SO_SERI>
<SO_KCB>%SOKCB%</SO_KCB>
<MA_BHXH>%MABHXH%</MA_BHXH>
<MA_THE>%MATHE%</MA_THE>
<HO_TEN><![CDATA[%HOTEN%]]></HO_TEN>
<NGAY_SINH>%NGAYSINH%</NGAY_SINH>
<GIOI_TINH>%GIOITINH%</GIOI_TINH>
<DON_VI><![CDATA[%DONVI%]]></DON_VI>
<CHANDOAN_DIEUTRI><![CDATA[%CHANDOANDIEUTRI%]]></CHANDOAN_DIEUTRI>
<TU_NGAY>%TUNGAY%</TU_NGAY>
<DEN_NGAY>%DENNGAY%</DEN_NGAY>
<HO_TEN_CHA><![CDATA[%HOTENCHA%]]></HO_TEN_CHA>
<HO_TEN_ME><![CDATA[%HOTENME%]]></HO_TEN_ME>
<THU_TRUONG_DV><![CDATA[%THUTRUONGDONVI%]]></THU_TRUONG_DV>
<MA_CCHN>%MACCHN%</MA_CCHN>
<TEN_NGUOI_HANH_NGHE><![CDATA[%TENNGUOIHANHNGHE%]]></TEN_NGUOI_HANH_NGHE>
<NGAY_CHUNG_TU>%NGAYCHUNGTU%</NGAY_CHUNG_TU>
<TEKT>%TREEM%</TEKT>
<MAU_SO>%MAUSO%</MAU_SO>
</CT07>";
    }
}
