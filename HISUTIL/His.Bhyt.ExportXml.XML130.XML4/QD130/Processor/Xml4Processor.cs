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
            ResultADO result = null;
            try
            {
                List<Xml4ADO> list = new List<Xml4ADO>();
                int num = 1;
                if (data.vSereServ == null || data.vSereServ.Count == 0)
                {
                    return result;
                }
                data.vSereServ = data.vSereServ.Where((V_HIS_SERE_SERV_2 o) => new List<long> { 1L, 8L, 18L }.Exists((long p) => p == o.TDL_HEIN_SERVICE_TYPE_ID)).ToList();
                if (data.vSereServ == null || data.vSereServ.Count == 0)
                {
                    return result;
                }
                if (data.vSereServTein == null)
                {
                    data.vSereServTein = new List<V_HIS_SERE_SERV_TEIN>();
                }
                data.vSereServTein = data.vSereServTein.OrderBy((V_HIS_SERE_SERV_TEIN t) => t.ID).ToList();
                foreach (V_HIS_SERE_SERV_TEIN ssTein in data.vSereServTein)
                {
                    V_HIS_SERE_SERV_2 v_HIS_SERE_SERV_ = data.vSereServ.FirstOrDefault((V_HIS_SERE_SERV_2 o) => o.ID == ssTein.SERE_SERV_ID);
                    if (v_HIS_SERE_SERV_ == null)
                    {
                        continue;
                    }
                    Xml4ADO xml4ADO = new Xml4ADO();
                    xml4ADO.MaLienKet = data.vTreatment.TREATMENT_CODE?? "";
                    xml4ADO.Stt = num;
                    xml4ADO.MaDichVu = v_HIS_SERE_SERV_.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                    xml4ADO.MaChiSo = ssTein.BHYT_CODE ?? "";
                    xml4ADO.TenChiSo = ssTein.BHYT_NAME ?? "";
                    xml4ADO.GiaTri = ssTein.VALUE ?? "";
                    xml4ADO.DonViDo = ssTein.TEST_INDEX_UNIT_NAME ?? "";
                    xml4ADO.MoTa = SubMaxLength(ssTein.RESULT_DESCRIPTION ?? "");
                    xml4ADO.KetLuan = SubMaxLength(v_HIS_SERE_SERV_.CONCLUDE ?? "");
                    xml4ADO.NgayKetQua = (v_HIS_SERE_SERV_.FINISH_TIME.HasValue ? v_HIS_SERE_SERV_.FINISH_TIME.ToString().Substring(0, 12) : (v_HIS_SERE_SERV_.START_TIME.HasValue ? v_HIS_SERE_SERV_.START_TIME.ToString().Substring(0, 12) : v_HIS_SERE_SERV_.INTRUCTION_TIME.ToString().Substring(0, 12)));
                    xml4ADO.MaBacSiDocKetQua = "";
                    if (data.Employee != null && data.Employee.Count > 0 && (!string.IsNullOrEmpty(v_HIS_SERE_SERV_.SUBCLINICAL_RESULT_LOGINNAME) || !string.IsNullOrEmpty(v_HIS_SERE_SERV_.EXECUTE_LOGINNAME)))
                    {
                        List<string> list2 = (v_HIS_SERE_SERV_.SUBCLINICAL_RESULT_LOGINNAME ?? v_HIS_SERE_SERV_.EXECUTE_LOGINNAME).Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (string item3 in list2)
                        {
                            HIS_EMPLOYEE hIS_EMPLOYEE = data.Employee.FirstOrDefault((HIS_EMPLOYEE o) => o.LOGINNAME == item3);
                            if (hIS_EMPLOYEE != null && !string.IsNullOrEmpty(hIS_EMPLOYEE.DIPLOMA))
                            {
                                xml4ADO.MaBacSiDocKetQua = hIS_EMPLOYEE.DIPLOMA;
                                break;
                            }
                        }
                    }
                    xml4ADO.DuPhong = "";
                    list.Add(xml4ADO);
                    num++;
                }
                List<long> listHeinServiceTypeCLS = new List<long> { 1L, 8L, 18L };
                List<long> sereServHasTein = new List<long>();
                sereServHasTein = data.vSereServTein.Select((V_HIS_SERE_SERV_TEIN s) => s.SERE_SERV_ID).Distinct().ToList();
                if (data.vSereServSuin == null)
                {
                    data.vSereServSuin = new List<V_HIS_SERE_SERV_SUIN>();
                }
                List<V_HIS_SERE_SERV_2> list3 = (from o in data.vSereServ
                                                 where listHeinServiceTypeCLS.Contains(o.TDL_HEIN_SERVICE_TYPE_ID.Value) && !sereServHasTein.Contains(o.ID) && !data.vSereServSuin.Exists((V_HIS_SERE_SERV_SUIN p) => p.SERE_SERV_ID == o.ID)
                                                 select o into t
                                                 orderby t.INTRUCTION_TIME
                                                 select t).ToList();
                if (list3 == null)
                {
                    list3 = new List<V_HIS_SERE_SERV_2>();
                }
                foreach (V_HIS_SERE_SERV_2 hisSereServ in list3)
                {
                    Xml4ADO xml4ADO2 = new Xml4ADO();
                    xml4ADO2.MaLienKet = data.vTreatment.TREATMENT_CODE ?? "";
                    xml4ADO2.Stt = num;
                    xml4ADO2.MaDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                    V_HIS_SERVICE v_HIS_SERVICE = null;
                    if (data.ListServices != null && data.ListServices.Count > 0)
                    {
                        v_HIS_SERVICE = data.ListServices.FirstOrDefault((V_HIS_SERVICE o) => o.ID == hisSereServ.SERVICE_ID);
                    }
                    if (v_HIS_SERVICE != null)
                    {
                        xml4ADO2.MaChiSo = v_HIS_SERVICE.SUIM_INDEX_CODE ?? "";
                        xml4ADO2.TenChiSo = v_HIS_SERVICE.SUIM_INDEX_NAME ?? "";
                    }
                    else
                    {
                        xml4ADO2.MaChiSo = "";
                        xml4ADO2.TenChiSo = "";
                    }
                    xml4ADO2.GiaTri = "";
                    xml4ADO2.DonViDo = "";
                    xml4ADO2.MoTa = SubMaxLength(hisSereServ.DESCRIPTION ?? "");
                    xml4ADO2.KetLuan = SubMaxLength(hisSereServ.CONCLUDE ?? "");
                    xml4ADO2.NgayKetQua = (hisSereServ.END_TIME.HasValue ? hisSereServ.END_TIME.ToString().Substring(0, 12) : (hisSereServ.START_TIME.HasValue ? hisSereServ.START_TIME.ToString().Substring(0, 12) : hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12)));
                    xml4ADO2.MaBacSiDocKetQua = "";
                    if (data.Employee != null && data.Employee.Count > 0 && (!string.IsNullOrEmpty(hisSereServ.SUBCLINICAL_RESULT_LOGINNAME) || !string.IsNullOrEmpty(hisSereServ.EXECUTE_LOGINNAME)))
                    {
                        List<string> list4 = (hisSereServ.EXECUTE_LOGINNAME ?? hisSereServ.SUBCLINICAL_RESULT_LOGINNAME).Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (string item2 in list4)
                        {
                            HIS_EMPLOYEE hIS_EMPLOYEE2 = data.Employee.FirstOrDefault((HIS_EMPLOYEE o) => o.LOGINNAME == item2);
                            if (hIS_EMPLOYEE2 != null && !string.IsNullOrEmpty(hIS_EMPLOYEE2.DIPLOMA))
                            {
                                xml4ADO2.MaBacSiDocKetQua = hIS_EMPLOYEE2.DIPLOMA;
                                break;
                            }
                        }
                    }
                    xml4ADO2.DuPhong = "";
                    list.Add(xml4ADO2);
                    num++;
                }
                if (data.vSereServSuin.Count > 0)
                {
                    foreach (V_HIS_SERE_SERV_2 item in data.vSereServ)
                    {
                        IEnumerable<V_HIS_SERE_SERV_SUIN> enumerable = data.vSereServSuin.Where((V_HIS_SERE_SERV_SUIN o) => o.SERE_SERV_ID == item.ID);
                        foreach (V_HIS_SERE_SERV_SUIN item4 in enumerable)
                        {
                            if (item4 == null)
                            {
                                continue;
                            }
                            Xml4ADO xml4ADO3 = new Xml4ADO();
                            xml4ADO3.MaLienKet = data.vTreatment.TREATMENT_CODE ?? "";
                            xml4ADO3.Stt = num;
                            xml4ADO3.MaDichVu = item.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                            xml4ADO3.MaChiSo = item4.SUIM_INDEX_CODE ?? "";
                            xml4ADO3.TenChiSo = item4.SUIM_INDEX_NAME ?? "";
                            if (string.IsNullOrEmpty(xml4ADO3.MaChiSo) || string.IsNullOrEmpty(xml4ADO3.TenChiSo))
                            {
                                V_HIS_SERVICE v_HIS_SERVICE2 = null;
                                if (data.ListServices != null && data.ListServices.Count > 0)
                                {
                                    v_HIS_SERVICE2 = data.ListServices.FirstOrDefault((V_HIS_SERVICE o) => o.ID == item.SERVICE_ID);
                                }
                                if (v_HIS_SERVICE2 != null)
                                {
                                    xml4ADO3.MaChiSo = v_HIS_SERVICE2.SUIM_INDEX_CODE ?? "";
                                    xml4ADO3.TenChiSo = v_HIS_SERVICE2.SUIM_INDEX_NAME ?? "";
                                }
                                else
                                {
                                    xml4ADO3.MaChiSo = "";
                                    xml4ADO3.TenChiSo = "";
                                }
                            }
                            xml4ADO3.GiaTri = item4.VALUE ?? "";
                            xml4ADO3.DonViDo = item4.SUIM_INDEX_UNIT_NAME ?? "";
                            xml4ADO3.MoTa = SubMaxLength(item4.DESCRIPTION ?? "");
                            xml4ADO3.KetLuan = SubMaxLength(item.CONCLUDE ?? "");
                            xml4ADO3.NgayKetQua = (item.FINISH_TIME.HasValue ? item.FINISH_TIME.ToString().Substring(0, 12) : (item.START_TIME.HasValue ? item.START_TIME.ToString().Substring(0, 12) : item.INTRUCTION_TIME.ToString().Substring(0, 12)));
                            xml4ADO3.MaBacSiDocKetQua = "";
                            if (data.Employee != null && data.Employee.Count > 0 && (!string.IsNullOrEmpty(item.SUBCLINICAL_RESULT_LOGINNAME) || !string.IsNullOrEmpty(item.EXECUTE_LOGINNAME))) 
                            {
                                List<string> list5 = (item.EXECUTE_LOGINNAME ?? item.SUBCLINICAL_RESULT_LOGINNAME).Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                foreach (string itemL in list5)
                                {
                                    HIS_EMPLOYEE hIS_EMPLOYEE3 = data.Employee.FirstOrDefault((HIS_EMPLOYEE o) => o.LOGINNAME == itemL);
                                    if (hIS_EMPLOYEE3 != null && !string.IsNullOrEmpty(hIS_EMPLOYEE3.DIPLOMA))
                                    {
                                        xml4ADO3.MaBacSiDocKetQua = hIS_EMPLOYEE3.DIPLOMA;
                                        break;
                                    }
                                }
                            }
                            xml4ADO3.DuPhong = "";
                            num++;
                            list.Add(xml4ADO3);
                        }
                    }
                }
                return new ResultADO(true, "", new object[1] { list });
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new ResultADO(false, ex.Message, null);
            }
        }

        internal void MapADOToXml(List<Xml4ADO> listAdo, ref List<XML4DetailData> datas)
        {
            try
            {
                if (datas == null)
                {
                    datas = new List<XML4DetailData>();
                }
                if (listAdo == null && listAdo.Count <= 0)
                {
                    return;
                }
                foreach (Xml4ADO item in listAdo)
                {
                    XML4DetailData xML4DetailData = new XML4DetailData();
                    xML4DetailData.MA_LK = item.MaLienKet;
                    xML4DetailData.STT = item.Stt;
                    xML4DetailData.MA_DICH_VU = item.MaDichVu;
                    xML4DetailData.MA_CHI_SO = item.MaChiSo;
                    xML4DetailData.TEN_CHI_SO = ConvertStringToXmlDocument(item.TenChiSo);
                    xML4DetailData.GIA_TRI = ConvertStringToXmlDocument(item.GiaTri);
                    xML4DetailData.DON_VI_DO = ConvertStringToXmlDocument(item.DonViDo);
                    xML4DetailData.MO_TA = ConvertStringToXmlDocument(item.MoTa);
                    xML4DetailData.KET_LUAN = ConvertStringToXmlDocument(item.KetLuan);
                    xML4DetailData.NGAY_KQ = item.NgayKetQua;
                    xML4DetailData.MA_BS_DOC_KQ = item.MaBacSiDocKetQua;
                    xML4DetailData.DU_PHONG = item.DuPhong;
                    datas.Add(xML4DetailData);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
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
