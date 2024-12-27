using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.CV2076.ADO;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.CV2076.Processor
{
    class XmlCt05Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXmlCt05ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                V_HIS_TREATMENT_10 Treatment = data.Treatment2076;
                List<V_HIS_BABY> Babys = data.Babys != null ? data.Babys.Where(o => o.BIRTH_CERT_BOOK_ID.HasValue).ToList() : null;
                List<XmlCt05ADO> listXmlCt05Ado = new List<XmlCt05ADO>();
                if (
                    Babys != null
                    && Babys.Count > 0
                    && (!String.IsNullOrWhiteSpace(Treatment.TDL_HEIN_CARD_NUMBER) || !String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER)))
                {
                    foreach (V_HIS_BABY baby in Babys)
                    {
                        XmlCt05ADO xmlCt05Ado = new XmlCt05ADO();
                        if (baby.WEIGHT.HasValue)
                        {
                            xmlCt05Ado.CanNangCon = this.ToString(baby.WEIGHT.Value);
                        }
                        else
                        {
                            xmlCt05Ado.CanNangCon = "";
                        }
                        xmlCt05Ado.GhiChu = "";
                        if (baby.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            xmlCt05Ado.GioiTinhCon = "2";
                        }
                        else if (baby.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            xmlCt05Ado.GioiTinhCon = "1";
                        }
                        else
                        {
                            xmlCt05Ado.GioiTinhCon = "3";
                        }
                        xmlCt05Ado.HoTenCha = baby.FATHER_NAME ?? "";
                        xmlCt05Ado.HoTenNguoiNuoiDuong = Treatment.TDL_PATIENT_NAME ?? "";
                        if (!String.IsNullOrWhiteSpace(Treatment.TDL_SOCIAL_INSURANCE_NUMBER))
                        {
                            xmlCt05Ado.MaBhxhNguoiNuoiDuong = Treatment.TDL_SOCIAL_INSURANCE_NUMBER;
                        }
                        else
                        {
                            xmlCt05Ado.MaBhxhNguoiNuoiDuong = Treatment.TDL_HEIN_CARD_NUMBER.Substring(5, 10);
                            xmlCt05Ado.MaTheNguoiNuoiDuong = Treatment.TDL_HEIN_CARD_NUMBER;
                        }
                        xmlCt05Ado.MaChungTu = "";
                        xmlCt05Ado.NgayChungTu = baby.CREATE_TIME.HasValue ? baby.CREATE_TIME.ToString().Substring(0, 8) : "";
                        if (!String.IsNullOrWhiteSpace(Treatment.ETHNIC_CODE))
                        {
                            xmlCt05Ado.MaDanTocNguoiNuoiDuong = Convert.ToInt32(Treatment.ETHNIC_CODE).ToString("G27", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            xmlCt05Ado.MaDanTocNguoiNuoiDuong = "";
                        }

                        if (!String.IsNullOrWhiteSpace(Treatment.CMND_NUMBER))
                        {
                            xmlCt05Ado.SoCmndNguoiNuoiDuong = Treatment.CMND_NUMBER;
                            if (Treatment.CMND_DATE.HasValue)
                            {
                                xmlCt05Ado.NgayCapCmndNguoiNuoiDuong = Treatment.CMND_DATE.Value.ToString().Substring(0, 8);
                            }
                            else
                            {
                                xmlCt05Ado.NgayCapCmndNguoiNuoiDuong = "";
                            }
                            xmlCt05Ado.NoiCapCmndNguoiNuoiDuong = Treatment.CMND_PLACE ?? "";
                        }
                        else if (!String.IsNullOrWhiteSpace(Treatment.CCCD_NUMBER))
                        {
                            xmlCt05Ado.SoCmndNguoiNuoiDuong = Treatment.CCCD_NUMBER;
                            if (Treatment.CCCD_DATE.HasValue)
                            {
                                xmlCt05Ado.NgayCapCmndNguoiNuoiDuong = Treatment.CCCD_DATE.Value.ToString().Substring(0, 8);
                            }
                            else
                            {
                                xmlCt05Ado.NgayCapCmndNguoiNuoiDuong = "";
                            }
                            xmlCt05Ado.NoiCapCmndNguoiNuoiDuong = Treatment.CCCD_PLACE ?? "";
                        }
                        else
                        {
                            xmlCt05Ado.SoCmndNguoiNuoiDuong = "";
                            xmlCt05Ado.NoiCapCmndNguoiNuoiDuong = "";
                            xmlCt05Ado.NgayCapCmndNguoiNuoiDuong = "";
                        }
                        xmlCt05Ado.NgaySinhNguoiNoiDuong = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 8);

                        xmlCt05Ado.NoiDKTTNguoiNuoiDuong = this.SubString(Treatment.TDL_PATIENT_ADDRESS, 500) ?? "";
                        xmlCt05Ado.NoiSinhCon = this.SubString(data.Branch.ADDRESS, 500) ?? "";
                        xmlCt05Ado.NguoiDoDe = this.SubString(baby.MIDWIFE, 200) ?? "";
                        xmlCt05Ado.NguoiGhiPhieu = baby.ISSUER_USERNAME ?? "";
                        xmlCt05Ado.QuyenSo = this.SubString(baby.BIRTH_CERT_BOOK_NAME, 200) ?? "";
                        if (baby.WEEK_COUNT.HasValue && baby.WEEK_COUNT.Value < 32)
                        {
                            xmlCt05Ado.SinhConDuoi32Tuan = "1";
                        }
                        else
                        {
                            xmlCt05Ado.SinhConDuoi32Tuan = "0";
                        }
                        if (baby.BORN_TYPE_ID.HasValue && baby.BORN_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_BORN_TYPE.ID__MO)
                        {
                            xmlCt05Ado.SinhConPhauThuat = "1";
                        }
                        else
                        {
                            xmlCt05Ado.SinhConPhauThuat = "0";
                        }

                        if (baby.BIRTH_CERT_NUM.HasValue)
                        {
                            xmlCt05Ado.So = baby.BIRTH_CERT_NUM.Value.ToString("G27", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            xmlCt05Ado.So = "";
                        }
                        xmlCt05Ado.SoCon = Babys.Count.ToString("G27", CultureInfo.InvariantCulture);
                        xmlCt05Ado.SoSeri = "";
                        xmlCt05Ado.TenCon = baby.BABY_NAME ?? "";
                        xmlCt05Ado.ThuTruongDonVi = data.Branch.REPRESENTATIVE ?? "";
                        xmlCt05Ado.TinhTrangCon = baby.BORN_RESULT_NAME ?? "";
                        if (baby.BORN_TIME.HasValue)
                        {
                            xmlCt05Ado.NgaySinhCon = baby.BORN_TIME.Value.ToString().Substring(0, 12);
                        }
                        else
                        {
                            xmlCt05Ado.NgaySinhCon = "";
                        }

                        listXmlCt05Ado.Add(xmlCt05Ado);
                    }
                }
                rs = new ResultADO(true, "", new object[] { listXmlCt05Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        internal void MapADOToXml(List<XmlCt05ADO> listAdo, ref List<string> listContent)
        {
            try
            {
                if (listContent == null)
                    listContent = new List<string>();
                if (listAdo != null && listAdo.Count > 0)
                {
                    foreach (XmlCt05ADO ado in listAdo)
                    {
                        string context = contentXml;
                        LogSystem.Info("context: " + context);

                        context = context.Replace("%HEADER%", this.header_xml);
                        context = context.Replace("%CANNANGCON%", ado.CanNangCon);
                        context = context.Replace("%GHICHU%", ado.GhiChu);
                        context = context.Replace("%GIOITINHCON%", ado.GioiTinhCon);
                        context = context.Replace("%HOTENCHA%", ado.HoTenCha);
                        context = context.Replace("%HOTENNND%", ado.HoTenNguoiNuoiDuong);
                        context = context.Replace("%MABHXHNND%", ado.MaBhxhNguoiNuoiDuong);
                        context = context.Replace("%MACHUNGTU%", ado.MaChungTu);
                        context = context.Replace("%MADANTOCNND%", ado.MaDanTocNguoiNuoiDuong);
                        context = context.Replace("%MATHENND%", ado.MaTheNguoiNuoiDuong);
                        context = context.Replace("%NGAYCHUNGTU%", ado.NgayChungTu);
                        context = context.Replace("%NGAYSINHCON%", ado.NgaySinhCon);
                        context = context.Replace("%NGAYCAPCMNDNND%", ado.NgayCapCmndNguoiNuoiDuong);
                        context = context.Replace("%NGAYSINHNND%", ado.NgaySinhNguoiNoiDuong);
                        context = context.Replace("%NGUOIDODE%", ado.NguoiDoDe);
                        context = context.Replace("%NGUOIGHIPHIEU%", ado.NguoiGhiPhieu);
                        context = context.Replace("%NOIDKTHUONGTRUNND%", ado.NoiDKTTNguoiNuoiDuong);
                        context = context.Replace("%NOISINHCON%", ado.NoiSinhCon);
                        context = context.Replace("%NOICAPCMNDNND%", ado.NoiCapCmndNguoiNuoiDuong);
                        context = context.Replace("%QUYENSO%", ado.QuyenSo);
                        context = context.Replace("%SINHCONDUOI32TUAN%", ado.SinhConDuoi32Tuan);
                        context = context.Replace("%SINHCONPHAUTHUAT%", ado.SinhConPhauThuat);
                        context = context.Replace("%SO%", ado.So);
                        context = context.Replace("%SOCMNDNND%", ado.SoCmndNguoiNuoiDuong);
                        context = context.Replace("%SOCON%", ado.SoCon);
                        context = context.Replace("%SOSERI%", ado.SoSeri);
                        context = context.Replace("%TENCON%", ado.TenCon);
                        context = context.Replace("%THUTRUONGDONVI%", ado.ThuTruongDonVi);
                        context = context.Replace("%TINHTRANGCON%", ado.TinhTrangCon);
                        listContent.Add(context);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listContent = null;
            }
        }

        private string contentXml = @"<CT05 %HEADER% >
<MA_CT>%MACHUNGTU%</MA_CT>
<SO_SERI>%SOSERI%</SO_SERI>
<MA_BHXH_NND>%MABHXHNND%</MA_BHXH_NND>
<MA_THE_NND>%MATHENND%</MA_THE_NND>
<HOTEN_NND><![CDATA[%HOTENNND%]]></HOTEN_NND>
<NGAYSINH_NND>%NGAYSINHNND%</NGAYSINH_NND>
<MA_DANTOC_NND>%MADANTOCNND%</MA_DANTOC_NND>
<SO_CMND_NND>%SOCMNDNND%</SO_CMND_NND>
<NGAYCAP_CMND_NND>%NGAYCAPCMNDNND%</NGAYCAP_CMND_NND>
<NOICAP_CMND_NND><![CDATA[%NOICAPCMNDNND%]]></NOICAP_CMND_NND>
<NOI_DK_THUONGTRU_NND><![CDATA[%NOIDKTHUONGTRUNND%]]></NOI_DK_THUONGTRU_NND>
<HO_TEN_CHA><![CDATA[%HOTENCHA%]]></HO_TEN_CHA>
<TEN_CON><![CDATA[%TENCON%]]></TEN_CON>
<GIOI_TINH_CON>%GIOITINHCON%</GIOI_TINH_CON>
<SO_CON>%SOCON%</SO_CON>
<CAN_NANG_CON>%CANNANGCON%</CAN_NANG_CON>
<NGAY_SINH_CON>%NGAYSINHCON%</NGAY_SINH_CON>
<NOI_SINH_CON><![CDATA[%NOISINHCON%]]></NOI_SINH_CON>
<TINH_TRANG_CON><![CDATA[%TINHTRANGCON%]]></TINH_TRANG_CON>
<SINHCON_PHAUTHUAT>%SINHCONPHAUTHUAT%</SINHCON_PHAUTHUAT>
<SINHCON_DUOI32TUAN>%SINHCONDUOI32TUAN%</SINHCON_DUOI32TUAN>
<GHI_CHU><![CDATA[%GHICHU%]]></GHI_CHU>
<NGUOI_DO_DE><![CDATA[%NGUOIDODE%]]></NGUOI_DO_DE>
<NGUOI_GHI_PHIEU><![CDATA[%NGUOIGHIPHIEU%]]></NGUOI_GHI_PHIEU>
<THU_TRUONG_DVI><![CDATA[%THUTRUONGDONVI%]]></THU_TRUONG_DVI>
<NGAY_CT>%NGAYCHUNGTU%</NGAY_CT>
<SO>%SO%</SO>
<QUYEN_SO><![CDATA[%QUYENSO%]]></QUYEN_SO>
</CT05>";
    }
}
