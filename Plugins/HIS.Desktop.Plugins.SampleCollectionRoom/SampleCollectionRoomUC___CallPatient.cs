using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Inventec.Desktop.Common.Message;
using Inventec.Core;
using Inventec.Common.Logging;
using HIS.Desktop.Controls.Session;
using MOS.Filter;
using Inventec.Common.Adapter;
using HIS.Desktop.ApiConsumer;
using MOS.SDO;
using MOS.EFMODEL.DataModels;
using HIS.Desktop.LocalStorage.LocalData;
using Inventec.Common.RichEditor.Base;
using HIS.Desktop.ADO;
using System.Threading;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.HisConfig;
using LIS.EFMODEL.DataModels;
using System.Reflection;
using HIS.Desktop.Plugins.SampleCollectionRoom.ADO;
using HIS.Desktop.LocalStorage.BackendData.V2.CallPatient;

namespace HIS.Desktop.Plugins.SampleCollectionRoom
{
    public partial class SampleCollectionRoomUC : HIS.Desktop.Utility.UserControlBase
    {
        private string callPatientFormName = "";

        public void LoadCallPatientByThread(object param)
        {
            Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataToControlCallPatientThread));
            try
            {
                thread.Start(param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                thread.Abort();
            }
        }

        internal void LoadDataToControlCallPatientThread(object param)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { CallPatient(param); }));
                }
                else
                {
                    CallPatient(param);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        internal void CallPatient(object param)
        {
            try
            {
                if (param is List<TreatmentSampleListViewADO>)
                {
                    var data = param as List<TreatmentSampleListViewADO>;
                    if (data != null)
                    {
                        CallPatientByNumOder(data);
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Debug("param Type 1.1 " + param.GetType());
                    }
                    Inventec.Common.Logging.LogSystem.Debug("param Type 1 " + param.GetType());
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("param Type 2 " + param.GetType());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        internal void CallPatientByNumOder(List<TreatmentSampleListViewADO> patients)
        {
            try
            {
                if (patients != null && patients.Count > 0)
                {
                    Inventec.Speech.SpeechPlayer.TypeSpeechCFG = HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>("Inventec.Speech.TypeSpeechCFG");
                    string moiBenhNhanStr = HisConfigs.Get<string>(SdaConfigKeys.CALL_PATIENT_MOI_BENH_NHAN);
                    //string coSoSttStr = HisConfigs.Get<string>(SdaConfigKeys.CALL_PATIENT_CO_STT);
                    //string denStr = HisConfigs.Get<string>(SdaConfigKeys.CALL_PATIENT_DEN);
                    Inventec.Speech.SpeechPlayer.SpeakSingle(moiBenhNhanStr);
                    Inventec.Speech.SpeechPlayer.Speak(patients.Select(o => o.PATIENT_NAME_AND_DOB).ToArray());
                    //Inventec.Speech.SpeechPlayer.SpeakSingle(coSoSttStr);
                    //Inventec.Speech.SpeechPlayer.Speak(numOder);
                    //Inventec.Speech.SpeechPlayer.SpeakSingle(denStr);
                    //Inventec.Speech.SpeechPlayer.SpeakSingle(examRoomName);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void CreateThreadCallPatient()
        {
            Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(CallPatientNewThread));
            try
            {
                thread.Start();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                thread.Abort();
            }
        }

        private void CallPatientNewThread()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { CallPatient(); }));
                }
                else
                {
                    CallPatient();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        async void CallPatient()
        {
            try
            {
                if (!btnCallPatient.Enabled)
                    return;
                if (String.IsNullOrWhiteSpace(txtGateNumber.Text) || String.IsNullOrEmpty(txtStepNumber.Text))
                    return;
                string configKeyCallPatientCPA = ConfigApplicationWorker.Get<string>(AppConfigKey.CONFIG_KEY__DANG_KY_TIEP_DON__GOI_BENH_NHAN_BANG_PHAN_MEM_CPA);
                if (configKeyCallPatientCPA == "1")
                {
                    if (this.clienttManager == null)
                        this.clienttManager = new CPA.WCFClient.CallPatientClient.CallPatientClientManager();
                    int[] nums = await this.clienttManager.AsyncCallNumOrderPlusString(txtGateNumber.Text, int.Parse(this.txtStepNumber.Text));
                    if (nums != null && nums.Length > 0)
                    {
                        await this.CallModuleCallPatientNumOrder(nums);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void UpdateDicCallPatient(List<TreatmentSampleListViewADO> lisSample, List<TreatmentSampleListViewADO> dataSource)
        {
            try
            {
                if (lisSample != null && lisSample.Count > 0)
                {
                    dataSource.ForEach(o => o.IS_CALLING = lisSample.Exists(p => p.ID == o.ID));
                    gridControlTreatmentSampleDesk.BeginUpdate();
                    gridControlTreatmentSampleDesk.DataSource = dataSource;
                    gridControlTreatmentSampleDesk.EndUpdate();
                    List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK> listCallTime = new List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>();
                    foreach (var item in lisSample)
                    {
                        item.CALL_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now);
                        listCallTime.Add(item);
                    }

                    HIS.Desktop.LocalStorage.BackendData.V2.CallPatient.CallPtDataWorker.UpdateCallTime(listCallTime, currentModule.RoomId, mosUserConsummer);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public void CallFocusPatient()
        {
            ButtonEdit_CallPatient_ButtonClick(null, null);
        }

        private void ButtonEdit_CallPatient_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var currentHisServiceReq = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                if (currentHisServiceReq != null)
                {
                    List<TreatmentSampleListViewADO> dataSource = (List<TreatmentSampleListViewADO>)(gridControlTreatmentSampleDesk.DataSource);
                    UpdateDicCallPatient(new List<TreatmentSampleListViewADO> { currentHisServiceReq }, dataSource);
                    LoadCallPatientByThread(new List<TreatmentSampleListViewADO> { currentHisServiceReq });
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private async Task CallModuleCallPatientNumOrder(int[] num)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(callPatientFormName))
                {
                    callPatientFormName = "WAITING_PATIENT_SAMPLE_" + this.SampleRoom.SAMPLE_ROOM_NAME;
                }
                Form waitingForm = null;
                if (Application.OpenForms != null && Application.OpenForms.Count > 0)
                {
                    for (int i = 0; i < Application.OpenForms.Count; i++)
                    {
                        Form f = Application.OpenForms[i];
                        if (f.Name == callPatientFormName)
                        {
                            waitingForm = f;
                        }
                    }
                }
                if (waitingForm != null)
                {
                    MethodInfo theMethod = waitingForm.GetType().GetMethod("CallNumOrder");
                    if (theMethod != null)
                    {
                        object[] param = new object[] { num.FirstOrDefault(), num.LastOrDefault() };
                        theMethod.Invoke(waitingForm, param);
                    }
                }
                else
                {
                    LogSystem.Warn("Nguoi dung chua mo man hinh cho CALL_PATIENT_NUM_ORDER");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CreateThreadReCallPatient()
        {
            Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ReacallCallPatientNewThread));
            try
            {
                thread.Start();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                thread.Abort();
            }
        }


        private void ReacallCallPatientNewThread()
        {
            try
            {
                this.ReCallPatient();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private async void ReCallPatient()
        {
            try
            {
                if (!btnCallPatient.Enabled)
                    return;
                if (String.IsNullOrEmpty(txtStepNumber.Text) || String.IsNullOrEmpty(txtGateNumber.Text))
                    return;
                string configKeyCallPatientCPA = ConfigApplicationWorker.Get<string>(AppConfigKey.CONFIG_KEY__DANG_KY_TIEP_DON__GOI_BENH_NHAN_BANG_PHAN_MEM_CPA);
                if (configKeyCallPatientCPA == "1")
                {
                    if (this.clienttManager == null)
                        this.clienttManager = new CPA.WCFClient.CallPatientClient.CallPatientClientManager();
                    int[] nums = await this.clienttManager.AsyncRecallNumOrderPlusString(txtGateNumber.Text.Trim(), int.Parse(txtStepNumber.Text));
                    if (nums != null && nums.Length > 0)
                    {
                        await this.CallModuleCallPatientNumOrder(nums);
                    }
                }
                else
                {
                    if (this.clienttManager == null)
                        this.clienttManager = new CPA.WCFClient.CallPatientClient.CallPatientClientManager();
                    this.clienttManager.RecallNumOrder(int.Parse(txtGateNumber.Text), int.Parse(txtStepNumber.Text));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
