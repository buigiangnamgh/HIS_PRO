using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps000387.PDO;
using MPS.ProcessorBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Processor.Mps000387
{
    public class Mps000387Processor : AbstractProcessor
    {
        Mps000387PDO rdo;
        List<ADOs> PathologicalHistorys = new List<ADOs>();
        List<ADOs> HospitalizationStates = new List<ADOs>();
        List<ADOs> TreatmentTrackings = new List<ADOs>();
        List<ADOs> InternalMedicineStates = new List<ADOs>();
        List<ADOs> Prognosiss = new List<ADOs>();
        List<ADOs> SubclinicalProcessess = new List<ADOs>();
        public Mps000387Processor(CommonParam param, PrintData printData)
            : base(param, printData)
        {
            rdo = (Mps000387PDO)rdoBase;
        }

        public override bool ProcessData()
        {
            bool result = false;
            try
            {
                Inventec.Common.FlexCellExport.ProcessSingleTag singleTag = new Inventec.Common.FlexCellExport.ProcessSingleTag();
                //Inventec.Common.FlexCellExport.ProcessBarCodeTag barCodeTag = new Inventec.Common.FlexCellExport.ProcessBarCodeTag();
                Inventec.Common.FlexCellExport.ProcessObjectTag objectTag = new Inventec.Common.FlexCellExport.ProcessObjectTag();

                store.ReadTemplate(System.IO.Path.GetFullPath(fileName));
                CreateListKey();
                SetSingleKey();
                singleTag.ProcessData(store, singleValueDictionary);
                objectTag.AddObjectData(store, "Participants", rdo.LstHisDebateUser);
                objectTag.AddObjectData(store, "ParticipantsEkip", rdo.LstHisDebateEkipUser);
                objectTag.AddObjectData(store, "PathologicalHistorys", PathologicalHistorys);
                objectTag.AddObjectData(store, "HospitalizationStates", HospitalizationStates);
                objectTag.AddObjectData(store, "TreatmentTrackings", TreatmentTrackings);
                objectTag.AddObjectData(store, "InternalMedicineStates", InternalMedicineStates);
                objectTag.AddObjectData(store, "Prognosiss", Prognosiss);
                objectTag.AddObjectData(store, "SubclinicalProcessess", SubclinicalProcessess);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

            return result;
        }
        private void CreateListKey()
        {
            try
            {
                if (rdo.CurrentHisDebate != null)
                {
                    if (!String.IsNullOrEmpty(rdo.CurrentHisDebate.PATHOLOGICAL_HISTORY))
                    {
                        rdo.CurrentHisDebate.PATHOLOGICAL_HISTORY.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(o => PathologicalHistorys.Add(new ADOs(o.Trim())));
                    }
                    if (!String.IsNullOrEmpty(rdo.CurrentHisDebate.HOSPITALIZATION_STATE))
                    {
                        rdo.CurrentHisDebate.HOSPITALIZATION_STATE.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(o => HospitalizationStates.Add(new ADOs(o.Trim())));
                    }
                    if (!String.IsNullOrEmpty(rdo.CurrentHisDebate.TREATMENT_TRACKING))
                    {
                        rdo.CurrentHisDebate.TREATMENT_TRACKING.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(o => TreatmentTrackings.Add(new ADOs(o.Trim())));
                    }
                    if (!String.IsNullOrEmpty(rdo.CurrentHisDebate.INTERNAL_MEDICINE_STATE))
                    {
                        rdo.CurrentHisDebate.INTERNAL_MEDICINE_STATE.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(o => InternalMedicineStates.Add(new ADOs(o.Trim())));
                    }
                    if (!String.IsNullOrEmpty(rdo.CurrentHisDebate.PROGNOSIS))
                    {
                        rdo.CurrentHisDebate.PROGNOSIS.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(o => Prognosiss.Add(new ADOs(o.Trim()))); ;
                    }
                    if (!String.IsNullOrEmpty(rdo.CurrentHisDebate.SUBCLINICAL_PROCESSES))
                    {
                        rdo.CurrentHisDebate.SUBCLINICAL_PROCESSES.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(o => SubclinicalProcessess.Add(new ADOs(o.Trim())));
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetSereServ()
        {
            try
            {
                MOS.Filter.HisSereServView11Filter filter = new MOS.Filter.HisSereServView11Filter();
                filter.TREATMENT_ID = rdo.CurrentHisDebate.TREATMENT_ID;
                 CommonParam param = new CommonParam();
               List< V_HIS_SERE_SERV_11> sereServs = new Inventec.Common.Adapter.BackendAdapter(param).Get<List<V_HIS_SERE_SERV_11>>("api/HisSereServ/GetView11", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, param);
                if (sereServs!=null && sereServs.Count>0)
                {
                    foreach (var item in sereServs)
                    {
                        if (item.TDL_SERVICE_NAME.Trim().ToLower() == "tổng phân tích tế bào máu ngoại vi (bằng máy đếm laser)")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.TONG_PT_TE_BAO_MAU_NGOAI_VI, "X"));
                        }
                        else if(item.TDL_SERVICE_NAME.Trim().ToLower() == "tổng phân tích nước tiểu (bằng máy tự động)")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.TONG_PT_NUOC_TIEU, "X"));
                        }
                        else if (item.TDL_SERVICE_NAME.Trim().ToLower() == "định lượng creatinin (máu)")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.DINH_LUONG_CREATININ, "X"));
                        }
                        else if (item.TDL_SERVICE_NAME.Trim().ToLower() == "điện tim thường")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.DIEN_TIM_THUONG, "X"));
                        }
                        else if (item.TDL_SERVICE_NAME.Trim().ToLower() == "định lượng fibrinogen (tên khác: định lượng yếu tố I), phương pháp clauss- phương pháp trực tiếp, bằng máy tự động")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.DINH_LUONG_FIBRINOGEN, "X"));
                        }
                        else if (item.TDL_SERVICE_NAME.Trim().ToLower() == "định lượng urê máu [máu]")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.DINH_LUONG_URE_MAU, "X"));
                        }
                        else if (item.TDL_SERVICE_NAME.Trim().ToLower() == "định nhóm máu hệ abo, rh(d) (kỹ thuật scangel/gelcard trên máy tự động)")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.DINH_NHOM_MAU_HE_ABO, "X"));
                        }
                        else if (item.TDL_SERVICE_NAME.Trim().ToLower() == "đo hoạt độ alt (gpt) [máu]")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.DO_HOAT_DO_ALT, "X"));
                        }
                        else if (item.TDL_SERVICE_NAME.Trim().ToLower() == "đo hoạt độ ast (got) [máu]")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.DO_HOAT_DO_AST, "X"));
                        }
                        else if (item.TDL_SERVICE_NAME.Trim().ToLower() == "hbsag test nhanh")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.HBSAG_TEST_NHANH, "X"));
                        }
                        else if (item.TDL_SERVICE_NAME.Trim().ToLower() == "hiv ab test nhanh")
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.HIV_AB_TEST_NHANH, "X"));
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void SetSingleKey()
        {
            try
            {
                if (rdo.CurrentHisDebate != null)
                {
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.DEBATE_TIME_STR, Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.CurrentHisDebate.DEBATE_TIME ?? 0)));
                    AddObjectKeyIntoListkey<V_HIS_DEBATE>(rdo.CurrentHisDebate, false);
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.SERVICE_CODE, rdo.CurrentHisDebate.SERVICE_CODE));
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.SERVICE_NAME, rdo.CurrentHisDebate.SERVICE_NAME));
                }

                if (rdo.Treatment != null)
                {
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.IN_CODE, rdo.Treatment.IN_CODE));
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.TREATMENT_CODE, rdo.Treatment.TREATMENT_CODE));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.AGE, CalculateFullAge(rdo.Treatment.TDL_PATIENT_DOB))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.DOB_STR, Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.Treatment.TDL_PATIENT_DOB))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.D_O_B, rdo.Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.GENDER_MALE, rdo.Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE ? "X" : "")));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.GENDER_FEMALE, rdo.Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE ? "X" : "")));
                    AddObjectKeyIntoListkey<V_HIS_TREATMENT>(rdo.Treatment, false);
                }

                if (rdo.PatyAlterBhyt != null && !string.IsNullOrEmpty(rdo.PatyAlterBhyt.HEIN_CARD_NUMBER))
                {
                    AddObjectKeyIntoListkey<V_HIS_PATIENT_TYPE_ALTER>(rdo.PatyAlterBhyt, false);
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.IS_HEIN, "X")));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.IS_VIENPHI, "")));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.HEIN_CARD_NUMBER_1, rdo.PatyAlterBhyt.HEIN_CARD_NUMBER.Substring(0, 2))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.HEIN_CARD_NUMBER_2, rdo.PatyAlterBhyt.HEIN_CARD_NUMBER.Substring(2, 1))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.HEIN_CARD_NUMBER_3, rdo.PatyAlterBhyt.HEIN_CARD_NUMBER.Substring(3, 2))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.HEIN_CARD_NUMBER_4, rdo.PatyAlterBhyt.HEIN_CARD_NUMBER.Substring(5, 2))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.HEIN_CARD_NUMBER_5, rdo.PatyAlterBhyt.HEIN_CARD_NUMBER.Substring(7, 3))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.HEIN_CARD_NUMBER_6, rdo.PatyAlterBhyt.HEIN_CARD_NUMBER.Substring(10, 5))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.STR_HEIN_CARD_FROM_TIME, Inventec.Common.DateTime.Convert.TimeNumberToDateString((rdo.PatyAlterBhyt.HEIN_CARD_FROM_TIME ?? 0)))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.STR_HEIN_CARD_TO_TIME, Inventec.Common.DateTime.Convert.TimeNumberToDateString((rdo.PatyAlterBhyt.HEIN_CARD_TO_TIME ?? 0)))));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.HEIN_MEDI_ORG_CODE, rdo.PatyAlterBhyt.HEIN_MEDI_ORG_CODE)));
                    SetSingleKey((new KeyValue(Mps000387ExtendSingleKey.HEIN_MEDI_ORG_NAME, rdo.PatyAlterBhyt.HEIN_MEDI_ORG_NAME)));
                }

                if (rdo.DepartmentTran != null)
                {
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.OPEN_TIME_SEPARATE_STR, Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.DepartmentTran.DEPARTMENT_IN_TIME ?? 0)));
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.CLOSE_TIME_SEPARATE_STR, Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.DepartmentTran.DEPARTMENT_IN_TIME ?? 0)));
                }

                if (rdo.SingleKey != null)
                {
                    AddObjectKeyIntoListkey<MPS.Processor.Mps000387.PDO.Mps000387PDO.Mps000387SingleKey>(rdo.SingleKey, true);
                }

                if (this.rdo.LstHisDebateUser != null)
                {
                    //Tìm chủ tọa và thư ký
                    foreach (var item_User in this.rdo.LstHisDebateUser)
                    {
                        if (item_User.IS_PRESIDENT == 1)
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.USERNAME_PRESIDENT, item_User.USERNAME));
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.PRESIDENT_DESCRIPTION, item_User.DESCRIPTION));
                        }

                        if (item_User.IS_SECRETARY == 1)
                        {
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.USERNAME_SECRETARY, item_User.USERNAME));
                            SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.SECRETARY_DESCRIPTION, item_User.DESCRIPTION));
                        }
                    }

                    rdo.LstHisDebateUser = rdo.LstHisDebateUser.Where(o => o.IS_SECRETARY != 1 && o.IS_PRESIDENT != 1).ToList();
                }

                MOS.Filter.HisServiceReqFilter filter = new MOS.Filter.HisServiceReqFilter();
                filter.TREATMENT_ID = rdo.CurrentHisDebate.TREATMENT_ID;
                filter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                CommonParam param = new CommonParam();
                HIS_SERVICE_REQ serviceReq = new Inventec.Common.Adapter.BackendAdapter(param).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, param).ToList().FirstOrDefault();
                if (serviceReq != null)
                {
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.PATHOLOGICAL_HISTORY_2, serviceReq.PATHOLOGICAL_HISTORY));
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.PATHOLOGICAL_HISTORY_FAMILY, serviceReq.PATHOLOGICAL_HISTORY_FAMILY));
                    SetSingleKey(new KeyValue(Mps000387ExtendSingleKey.PATHOLOGICAL_PROCESS, serviceReq.PATHOLOGICAL_PROCESS));
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("rdo._ServiceReq IS null");
                }

                GetSereServ();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string CalculateFullAge(long ageNumber)
        {
            string tuoi;
            string cboAge;
            try
            {
                DateTime dtNgSinh = Inventec.Common.TypeConvert.Parse.ToDateTime(Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ageNumber));
                TimeSpan diff = DateTime.Now - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    tuoi = "";
                    cboAge = "Tuổi";
                    return "";
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;
                int thang = newDate.Month - 1;
                int ngay = newDate.Day - 1;
                int gio = newDate.Hour;
                int phut = newDate.Minute;
                int giay = newDate.Second;

                if (nam > 0)
                {
                    tuoi = nam.ToString();
                    cboAge = "Tuổi";
                }
                else
                {
                    if (thang > 0)
                    {
                        tuoi = thang.ToString();
                        cboAge = "Tháng";
                    }
                    else
                    {
                        if (ngay > 0)
                        {
                            tuoi = ngay.ToString();
                            cboAge = "ngày";
                        }
                        else
                        {
                            tuoi = "";
                            cboAge = "Giờ";
                        }
                    }
                }
                return tuoi + " " + cboAge;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return "";
            }
        }
    }
}
