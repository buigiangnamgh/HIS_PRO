using His.Bhyt.ExportXml.XML130.XML4.Base;
using His.Bhyt.ExportXml.XML130.XML4.QD130.ADO;
using His.Bhyt.ExportXml.XML130.XML4.QD130.XML;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML4.QD130.Processor
{
    class Xml4Processor : XmlProcessorBase
    {
        internal ResultADO GenerateXml4ADO(InputADO data)
        {
            ResultADO rs = null;
            try
            {
                List<Xml4ADO> listXml4Ado = new List<Xml4ADO>();
                int count = 1;
                if (data.vSereServ == null || data.vSereServ.Count == 0)
                    return rs;
                data.vSereServ = data.vSereServ.Where(o => new List<long> {  IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,
                     IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,
                     IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN }.Exists(p => p == o.TDL_HEIN_SERVICE_TYPE_ID)).ToList();
                if (data.vSereServ == null || data.vSereServ.Count == 0)
                    return rs;
                if (data.vSereServTein == null) data.vSereServTein = new List<V_HIS_SERE_SERV_TEIN>();

                data.vSereServTein = data.vSereServTein.OrderBy(t => t.ID).ToList();
                foreach (var ssTein in data.vSereServTein)
                {
                    V_HIS_SERE_SERV_2 ss = data.vSereServ.FirstOrDefault(o => o.ID == ssTein.SERE_SERV_ID);
                    if (ss == null)
                    {
                        continue;
                    }
                    Xml4ADO xml4 = new Xml4ADO();
                    xml4.MaLienKet = data.vTreatment.TREATMENT_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                    xml4.Stt = count;
                    xml4.MaDichVu = ss.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                    xml4.MaChiSo = ssTein.BHYT_CODE ?? "";
                    xml4.TenChiSo = ssTein.BHYT_NAME ?? "";
                    xml4.GiaTri = ssTein.VALUE ?? "";
                    xml4.DonViDo = ssTein.TEST_INDEX_UNIT_NAME ?? "";
                    xml4.MoTa = this.SubMaxLength(ssTein.RESULT_DESCRIPTION ?? "");
                    xml4.KetLuan = this.SubMaxLength(ss.CONCLUDE ?? "");
                    xml4.NgayKetQua = ss.FINISH_TIME.HasValue ? ss.FINISH_TIME.ToString().Substring(0, 12) : "";
                    xml4.MaBacSiDocKetQua = ss.EXECUTE_LOGINNAME ?? "";
                    if (data.Employee != null && data.Employee.Count > 0)
                    {
                        var loginName = data.Employee.FirstOrDefault(o => o.LOGINNAME.Equals(ss.SUBCLINICAL_RESULT_LOGINNAME ?? ss.EXECUTE_LOGINNAME));
                        if (loginName != null)
                            xml4.MaBacSiDocKetQua = loginName.SOCIAL_INSURANCE_NUMBER ?? "";
                        else
                            xml4.MaBacSiDocKetQua = ss.EXECUTE_LOGINNAME ?? "";
                    }
                    xml4.DuPhong = "";
                    listXml4Ado.Add(xml4);
                    count++;
                }

                List<long> listHeinServiceTypeCLS = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN
                    };

                List<long> sereServHasTein = new List<long>();
                sereServHasTein = data.vSereServTein.Select(s => s.SERE_SERV_ID).Distinct().ToList();

                if (data.vSereServSuin == null)
                    data.vSereServSuin = new List<V_HIS_SERE_SERV_SUIN>();


                //lấy các dịch vụ là CDHA, TDCN không có chỉ số
                var hisSereServs = data.vSereServ.Where(o => listHeinServiceTypeCLS.Contains(o.TDL_HEIN_SERVICE_TYPE_ID.Value) && !sereServHasTein.Contains(o.ID) && !data.vSereServSuin.Exists(p => p.SERE_SERV_ID == o.ID)).OrderBy(t => t.INTRUCTION_TIME).ToList();
                if (hisSereServs == null) hisSereServs = new List<V_HIS_SERE_SERV_2>();
                foreach (var hisSereServ in hisSereServs)
                {
                    Xml4ADO xml4 = new Xml4ADO();
                    xml4.MaLienKet = data.vTreatment.TREATMENT_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                    xml4.Stt = count;
                    xml4.MaDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                    V_HIS_SERVICE service = null;
                    if (data.ListServices != null && data.ListServices.Count > 0)
                    {
                        service = data.ListServices.FirstOrDefault(o => o.ID == hisSereServ.SERVICE_ID);
                    }
                    if (service != null)
                    {
                        xml4.MaChiSo = service.SERVICE_CODE ?? "";
                        xml4.TenChiSo = service.HEIN_SERVICE_BHYT_NAME ?? "";
                    }
                    else
                    {
                        xml4.MaChiSo = "";
                        xml4.TenChiSo = "";
                    }
                    xml4.GiaTri = "";
                    xml4.DonViDo = "";
                    xml4.MoTa = this.SubMaxLength(hisSereServ.DESCRIPTION ?? "");
                    xml4.KetLuan = this.SubMaxLength(hisSereServ.CONCLUDE ?? "");
                    xml4.NgayKetQua = hisSereServ.END_TIME.HasValue ? hisSereServ.END_TIME.ToString().Substring(0, 12) : "";
                    xml4.MaBacSiDocKetQua = hisSereServ.EXECUTE_LOGINNAME ?? "";
                    if (data.Employee != null && data.Employee.Count > 0)
                    {
                        var loginName = data.Employee.FirstOrDefault(o => o.LOGINNAME.Equals(hisSereServ.SUBCLINICAL_RESULT_LOGINNAME ?? hisSereServ.EXECUTE_LOGINNAME));
                        if (loginName != null)
                            xml4.MaBacSiDocKetQua = loginName.SOCIAL_INSURANCE_NUMBER ?? "";
                        else
                            xml4.MaBacSiDocKetQua = hisSereServ.EXECUTE_LOGINNAME ?? "";
                    }
                    xml4.DuPhong = "";
                    listXml4Ado.Add(xml4);
                    count++;
                }

                //Có chỉ số
                if (data.vSereServSuin.Count > 0)
                {
                    foreach (var item in data.vSereServ)
                    {
                        var ssSuinList = data.vSereServSuin.Where(o => o.SERE_SERV_ID == item.ID);
                        foreach (var ssSuin in ssSuinList)
                        {
                            if (ssSuin != null)
                            {
                                Xml4ADO xml4 = new Xml4ADO();
                                xml4.MaLienKet = data.vTreatment.TREATMENT_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                                xml4.Stt = count;
                                xml4.MaDichVu = item.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                                xml4.MaChiSo = ssSuin.SUIM_INDEX_CODE ?? "";
                                xml4.TenChiSo = ssSuin.SUIM_INDEX_NAME ?? "";
                                if (string.IsNullOrEmpty(xml4.MaChiSo) || string.IsNullOrEmpty(xml4.TenChiSo))
                                {
                                    V_HIS_SERVICE service = null;
                                    if (data.ListServices != null && data.ListServices.Count > 0)
                                    {
                                        service = data.ListServices.FirstOrDefault(o => o.ID == item.SERVICE_ID);
                                    }
                                    if (service != null)
                                    {
                                        xml4.MaChiSo = service.SUIM_INDEX_CODE ?? "";
                                        xml4.TenChiSo = service.SUIM_INDEX_NAME ?? "";
                                    }
                                    else
                                    {
                                        xml4.MaChiSo = "";
                                        xml4.TenChiSo = "";
                                    }
                                }
                                xml4.GiaTri = ssSuin.VALUE ?? "";
                                xml4.DonViDo = ssSuin.SUIM_INDEX_UNIT_NAME ?? "";
                                xml4.MoTa = this.SubMaxLength(ssSuin.DESCRIPTION ?? "");
                                xml4.KetLuan = this.SubMaxLength(item.CONCLUDE ?? "");
                                xml4.NgayKetQua = item.FINISH_TIME.HasValue ? item.FINISH_TIME.ToString().Substring(0, 12) : "";
                                xml4.MaBacSiDocKetQua = item.EXECUTE_LOGINNAME ?? "";
                                if (data.Employee != null && data.Employee.Count > 0)
                                {
                                    var loginName = data.Employee.FirstOrDefault(o => o.LOGINNAME.Equals(item.SUBCLINICAL_RESULT_LOGINNAME ?? item.EXECUTE_LOGINNAME));
                                    if (loginName != null)
                                        xml4.MaBacSiDocKetQua = loginName.SOCIAL_INSURANCE_NUMBER ?? "";
                                    else
                                        xml4.MaBacSiDocKetQua = item.EXECUTE_LOGINNAME ?? "";
                                }
                                xml4.DuPhong = "";
                                count++;
                                listXml4Ado.Add(xml4);
                            }
                        }
                    }
                }
                rs = new ResultADO(true, "", new object[] { listXml4Ado });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        internal void MapADOToXml(List<Xml4ADO> listAdo, ref List<XML4DetailData> datas)
        {
            try
            {
                if (datas == null)
                    datas = new List<XML4DetailData>();
                if (listAdo != null || listAdo.Count > 0)
                {
                    foreach (var ado in listAdo)
                    {
                        XML4DetailData detail = new XML4DetailData();
                        detail.MA_LK = ado.MaLienKet;
                        detail.STT = ado.Stt;
                        detail.MA_DICH_VU = ado.MaDichVu;
                        detail.MA_CHI_SO = ado.MaChiSo;
                        detail.TEN_CHI_SO = this.ConvertStringToXmlDocument(ado.TenChiSo);
                        detail.GIA_TRI = this.ConvertStringToXmlDocument(ado.GiaTri);
                        detail.DON_VI_DO = this.ConvertStringToXmlDocument(ado.DonViDo);
                        detail.MO_TA = this.ConvertStringToXmlDocument(ado.MoTa);
                        detail.KET_LUAN = this.ConvertStringToXmlDocument(ado.KetLuan);
                        detail.NGAY_KQ = ado.NgayKetQua;
                        detail.MA_BS_DOC_KQ = ado.MaBacSiDocKetQua;
                        detail.DU_PHONG = ado.DuPhong;
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
