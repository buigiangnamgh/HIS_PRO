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
    class XmlCt06Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXmlCt06ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                V_HIS_TREATMENT_10 Treatment = data.Treatment2076;
                XmlCt06ADO xmlCt06Ado = null;
                if (Treatment.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_DUONG_THAI
                    && (!String.IsNullOrWhiteSpace(Treatment.SICK_HEIN_CARD_NUMBER) || !String.IsNullOrWhiteSpace(Treatment.TDL_HEIN_CARD_NUMBER) || !String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER)))
                {
                    xmlCt06Ado = new XmlCt06ADO();


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

                    xmlCt06Ado.ChanDoan = this.SubString(tenBenh, 1500) ?? "";
                    xmlCt06Ado.HoTen = Treatment.TDL_PATIENT_NAME ?? "";

                    HIS_EMPLOYEE employee = null;
                    if (!String.IsNullOrWhiteSpace(Treatment.SICK_LOGINNAME))
                    {
                        employee = data.Employees != null ? data.Employees.FirstOrDefault(o => o.LOGINNAME == Treatment.SICK_LOGINNAME) : null;
                    }

                    if (employee != null)
                    {
                        xmlCt06Ado.MaBacSi = employee.DIPLOMA ?? "";
                    }
                    else
                    {
                        xmlCt06Ado.MaBacSi = "";
                    }
                    if (!String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER))
                    {
                        xmlCt06Ado.MaBhxh = Treatment.TDL_SOCIAL_INSURANCE_NUMBER;
                        xmlCt06Ado.MaThe = Treatment.SICK_HEIN_CARD_NUMBER ?? (Treatment.TDL_HEIN_CARD_NUMBER ?? "");
                    }
                    else if (!String.IsNullOrWhiteSpace(Treatment.SICK_HEIN_CARD_NUMBER))
                    {
                        xmlCt06Ado.MaBhxh = Treatment.SICK_HEIN_CARD_NUMBER.Substring(5, 10);
                        xmlCt06Ado.MaThe = Treatment.SICK_HEIN_CARD_NUMBER;
                    }
                    else
                    {
                        xmlCt06Ado.MaBhxh = Treatment.TDL_HEIN_CARD_NUMBER.Substring(5, 10);
                        xmlCt06Ado.MaThe = Treatment.TDL_HEIN_CARD_NUMBER;
                    }
                    xmlCt06Ado.MaChungTu = "";
                    xmlCt06Ado.NgayChungTu = Treatment.OUT_TIME.Value.ToString().Substring(0, 8);
                    if (Treatment.SICK_LEAVE_TO.HasValue)
                    {
                        xmlCt06Ado.NgayRa = Treatment.SICK_LEAVE_TO.Value.ToString().Substring(0, 8);
                    }
                    else
                    {
                        xmlCt06Ado.NgayRa = "";
                    }
                    if (Treatment.SICK_LEAVE_FROM.HasValue)
                    {
                        xmlCt06Ado.NgayVao = Treatment.SICK_LEAVE_FROM.Value.ToString().Substring(0, 8);
                    }
                    else
                    {
                        xmlCt06Ado.NgayVao = "";
                    }

                    if (Treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == (short)1)
                    {
                        xmlCt06Ado.NgaySinh = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                    }
                    else
                    {
                        xmlCt06Ado.NgaySinh = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 8);
                    }
                    xmlCt06Ado.NguoiDaiDien = Treatment.TDL_PATIENT_RELATIVE_NAME ?? "";
                    xmlCt06Ado.SoKhamChuaBenh = Treatment.TREATMENT_CODE;
                    xmlCt06Ado.SoSeri = "";
                    xmlCt06Ado.TenBacSi = Treatment.SICK_USERNAME ?? "";
                    xmlCt06Ado.TenDonVi = Treatment.TDL_PATIENT_WORK_PLACE ?? (Treatment.TDL_PATIENT_WORK_PLACE_NAME ?? "");
                }
                rs = new ResultADO(true, "", new object[] { xmlCt06Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        internal void MapADOToXml(XmlCt06ADO ado, ref string content)
        {
            try
            {
                if (ado != null)
                {
                    content = contentXml;

                    content = content.Replace("%HEADER%", this.header_xml);
                    content = content.Replace("%CHANDOAN%", ado.ChanDoan);
                    content = content.Replace("%HOTEN%", ado.HoTen);
                    content = content.Replace("%MABHXH%", ado.MaBhxh);
                    content = content.Replace("%MABACSI%", ado.MaBacSi);
                    content = content.Replace("%MACHUNGTU%", ado.MaChungTu);
                    content = content.Replace("%MATHE%", ado.MaThe);
                    content = content.Replace("%NGAYCHUNGTU%", ado.NgayChungTu);
                    content = content.Replace("%NGAYRA%", ado.NgayRa);
                    content = content.Replace("%NGAYSINH%", ado.NgaySinh);
                    content = content.Replace("%NGAYVAO%", ado.NgayVao);
                    content = content.Replace("%NGUOIDAIDIEN%", ado.NguoiDaiDien);
                    content = content.Replace("%SOKCB%", ado.SoKhamChuaBenh);
                    content = content.Replace("%SOSERI%", ado.SoSeri);
                    content = content.Replace("%TENBACSI%", ado.TenBacSi);
                    content = content.Replace("%TENDONVI%", ado.TenDonVi);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                content = null;
            }
        }

        private string contentXml = @"<CT06 %HEADER% >
<SO_SERI>%SOSERI%</SO_SERI>
<MA_CT>%MACHUNGTU%</MA_CT>
<SO_KCB>%SOKCB%</SO_KCB>
<MA_BHXH>%MABHXH%</MA_BHXH>
<MA_THE>%MATHE%</MA_THE>
<HO_TEN><![CDATA[%HOTEN%]]></HO_TEN>
<NGAY_SINH>%NGAYSINH%</NGAY_SINH>
<TEN_DVI><![CDATA[%TENDONVI%]]></TEN_DVI>
<CHAN_DOAN><![CDATA[%CHANDOAN%]]></CHAN_DOAN>
<NGAY_VAO>%NGAYVAO%</NGAY_VAO>
<NGAY_RA>%NGAYRA%</NGAY_RA>
<NGUOI_DAI_DIEN><![CDATA[%NGUOIDAIDIEN%]]></NGUOI_DAI_DIEN>
<TEN_BS><![CDATA[%TENBACSI%]]></TEN_BS>
<MA_BS>%MABACSI%</MA_BS>
<NGAY_CT>%NGAYCHUNGTU%</NGAY_CT>
</CT06>";
    }
}
