using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.LocalStorage.BackendData.V2.CallPatient;
using HIS.Desktop.Plugins.CallPatientSample.Config;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Core;
using LIS.EFMODEL.DataModels;
using LIS.Filter;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.Desktop.Plugins.CallPatientSample
{
    public partial class frmWaitingScreenSample22 : FormBase
    {
        internal HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK lisSample;
        const int STEP_NUMBER_ROW_GRID_SCROLL = 5;
        internal V_HIS_SAMPLE_ROOM room;
        private int scrll { get; set; }
        string organizationName = "";
        List<int> newStatusForceColorCodes = new List<int>();
        List<int> gridpatientBodyForceColorCodes;
        int rowCount = 0;
        bool isSetNum = false;
        bool? chkIsNotInDebt;

        private Inventec.Common.WebApiClient.ApiConsumer mosUserConsummer;

        public frmWaitingScreenSample22(Inventec.Desktop.Common.Modules.Module module, HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK sample, V_HIS_SAMPLE_ROOM r, bool? _chkIsNotInDebt)
            : base(module)
        {
            InitializeComponent();
            lblCallPatient.Text = "";
            this.lisSample = sample;
            this.room = r;
            this.chkIsNotInDebt = _chkIsNotInDebt;
        }

        private void frmWaitingScreen_QY_Load(object sender, EventArgs e)
        {
            try
            {
                HisConfigCFG.LoadConfig();
                SetDataToRoom(this.room);
                GetCallTime();
                SetDataToGridControlWaitingCLSs();
                StartAllTimer();
                SetFromConfigToControl();
                var emp = BackendDataWorker.Get<HIS_EMPLOYEE>().FirstOrDefault(o => o.LOGINNAME == Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetLoginName());
                lblDoctorName.Text = string.Format("{0} {1}", emp != null ? (emp.TITLE != null ? emp.TITLE + ": " : "") : "", Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetUserName().ToUpper());
                rowCount = gridViewWaiting.RowCount - 1;
                SetFormFrontOfAll();
                SetIcon();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void SetFormFrontOfAll()
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                this.BringToFront();
                this.TopMost = true;
                this.Focus();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void StartAllTimer()
        {
            try
            {

                if (WaitingScreenCFG.TIMER_FOR_AUTO_LOAD_WAITING_SCREENS > 0)
                    timerSetDataToGridControl.Interval = WaitingScreenCFG.TIMER_FOR_AUTO_LOAD_WAITING_SCREENS * 1000;
                timerSetDataToGridControl.Enabled = true;
                timerSetDataToGridControl.Start();
                timerCalling.Enabled = true;
                timerCalling.Start();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void SetDataToRoom(V_HIS_SAMPLE_ROOM room)
        {
            try
            {
                if (room != null)
                {
                    lblRoomName.Text = (room.SAMPLE_ROOM_NAME + " (" + room.DEPARTMENT_NAME + ")").ToUpper();
                }
                else
                {
                    lblRoomName.Text = "";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void SetIcon()
        {
            try
            {
                this.Icon = Icon.ExtractAssociatedIcon(System.IO.Path.Combine(ApplicationStoreLocation.ApplicationDirectory, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetFromConfigToControl()
        {
            try
            {
                organizationName = WaitingScreenCFG.ORGANIZATION_NAME;
                // mau phong xu ly
                List<int> roomNameColorCodes = WaitingScreenCFG.ROOM_NAME_FORCE_COLOR_CODES;
                Inventec.Common.Logging.LogSystem.Debug("roomNameColorCodes:" + string.Join(",", roomNameColorCodes));
                if (roomNameColorCodes != null && roomNameColorCodes.Count == 3)
                {
                    lblRoomName.Appearance.ForeColor = System.Drawing.Color.FromArgb(roomNameColorCodes[0], roomNameColorCodes[1], roomNameColorCodes[2]);
                    lblMoiNguoiBenh.Appearance.ForeColor = System.Drawing.Color.FromArgb(roomNameColorCodes[0], roomNameColorCodes[1], roomNameColorCodes[2]);
                    labelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(roomNameColorCodes[0], roomNameColorCodes[1], roomNameColorCodes[2]);
                }
                // co chu phong xu ly
                int roomNameSizeCodes = WaitingScreenCFG.ROOM_NAME_SIZE_CODES;
                if (roomNameSizeCodes != null && roomNameSizeCodes > 0)
                {
                    this.lblRoomName.Appearance.Font = new System.Drawing.Font("Arial", roomNameSizeCodes, System.Drawing.FontStyle.Bold);
                }

                // màu tên bác sĩ
                List<int> userNameColorCodes = WaitingScreenCFG.USER_NAME_FORCE_COLOR_CODES;
                Inventec.Common.Logging.LogSystem.Debug("userNameColorCodes:" + string.Join(",", userNameColorCodes));
                if (userNameColorCodes != null && userNameColorCodes.Count == 3)
                {
                    lblDoctorName.Appearance.ForeColor = System.Drawing.Color.FromArgb(userNameColorCodes[0], userNameColorCodes[1], userNameColorCodes[2]);
                }

                // co chu ten bac si
                int userSizeCodes = WaitingScreenCFG.USER_NAME_SIZE_CODES;
                if (userSizeCodes != null && userSizeCodes > 0)
                {
                    this.lblDoctorName.Appearance.Font = new System.Drawing.Font("Arial", userSizeCodes, System.Drawing.FontStyle.Bold);
                }

                //mau background
                List<int> parentBackColorCodes = WaitingScreenCFG.PARENT_BACK_COLOR_CODES;
                Inventec.Common.Logging.LogSystem.Debug("parentBackColorCodes:" + string.Join(",", parentBackColorCodes));
                if (parentBackColorCodes != null && parentBackColorCodes.Count == 3)
                {
                    layoutControlGroup1.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    layoutControlGroup3.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    layoutControlGroup5.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    Root.AppearanceGroup.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                    lblMoiNguoiBenh.BackColor = System.Drawing.Color.FromArgb(parentBackColorCodes[0], parentBackColorCodes[1], parentBackColorCodes[2]);
                }

                //màu chữ tên tổ chức
                //List<int> organizationColorCodes = WaitingScreenCFG.ORGANIZATION_FORCE_COLOR_CODES;
                //if (organizationColorCodes != null && organizationColorCodes.Count == 3)
                //{
                //    lblSrollText.ForeColor = System.Drawing.Color.FromArgb(organizationColorCodes[0], organizationColorCodes[1], organizationColorCodes[2]);
                //}
                //gridControlWaitngCls
                //màu nền grid patients
                List<int> gridpatientBackColorCodes = WaitingScreenCFG.GRID_PATIENTS_BACK_COLOR_CODES;//gridControlWaitingCls
                Inventec.Common.Logging.LogSystem.Debug("gridpatientBackColorCodes:" + string.Join(",", gridpatientBackColorCodes));
                if (gridpatientBackColorCodes != null && gridpatientBackColorCodes.Count == 3)
                {
                    this.gridViewWaiting.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(gridpatientBackColorCodes[0], gridpatientBackColorCodes[1], gridpatientBackColorCodes[2]);
                    this.gridViewWaiting.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(gridpatientBackColorCodes[0], gridpatientBackColorCodes[1], gridpatientBackColorCodes[2]);
                    this.gridViewWaiting.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(gridpatientBackColorCodes[0], gridpatientBackColorCodes[1], gridpatientBackColorCodes[2]);
                }


                //màu nền của header danh sách bệnh nhân
                List<int> gridpatientHeaderBackColorCodes = WaitingScreenCFG.GRID_PATIENTS_HEADER_BACK_COLOR_CODES;
                Inventec.Common.Logging.LogSystem.Debug("gridpatientHeaderBackColorCodes:" + string.Join(",", gridpatientHeaderBackColorCodes));
                if (gridpatientHeaderBackColorCodes != null && gridpatientHeaderBackColorCodes.Count == 3)
                {
                    this.gridViewWaiting.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    this.gridViewWaiting.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    this.gridViewWaiting.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                    this.gridViewWaiting.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.FromArgb(gridpatientHeaderBackColorCodes[0], gridpatientHeaderBackColorCodes[1], gridpatientHeaderBackColorCodes[2]);
                }

                //màu chữ của header danh sách bệnh nhân
                List<int> gridpatientHeaderForceColorCodes = WaitingScreenCFG.GRID_PATIENTS_HEADER_FORCE_COLOR_CODES;
                Inventec.Common.Logging.LogSystem.Debug("gridpatientHeaderForceColorCodes:" + string.Join(",", gridpatientHeaderForceColorCodes));
                if (gridpatientHeaderForceColorCodes != null && gridpatientHeaderForceColorCodes.Count == 3)
                {
                    gridColumnAge.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnFirstName.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnInstructionTime.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnLastName.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnServiceReqStt.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnServiceReqType.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnSTT.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                    gridColumnAddress.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(gridpatientHeaderForceColorCodes[0], gridpatientHeaderForceColorCodes[1], gridpatientHeaderForceColorCodes[2]);
                }

                //màu chữ của body danh sách bệnh nhân
                gridpatientBodyForceColorCodes = WaitingScreenCFG.GRID_PATIENTS_BODY_FORCE_COLOR_CODES;
                Inventec.Common.Logging.LogSystem.Debug("gridpatientBodyForceColorCodes:" + string.Join(",", gridpatientBodyForceColorCodes));
                if (gridpatientBodyForceColorCodes != null && gridpatientBodyForceColorCodes.Count == 3)
                {
                    gridColumnAge.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnFirstName.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnInstructionTime.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb
(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnLastName.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnServiceReqStt.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb
(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnServiceReqType.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnSTT.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                    gridColumnAddress.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(gridpatientBodyForceColorCodes[0], gridpatientBodyForceColorCodes[1], gridpatientBodyForceColorCodes[2]);
                }

                //cỡ chữ của body danh sách bệnh nhân
                int patientBodySizeCodes = WaitingScreenCFG.PATIENT_BODY_SIZE_CODES;

                if (patientBodySizeCodes != null && patientBodySizeCodes > 0)
                {
                    gridColumnAge.AppearanceCell.Font = new System.Drawing.Font("Arial", patientBodySizeCodes, System.Drawing.FontStyle.Bold);
                    gridColumnFirstName.AppearanceCell.Font = new System.Drawing.Font("Arial", patientBodySizeCodes, System.Drawing.FontStyle.Bold);
                    gridColumnInstructionTime.AppearanceCell.Font = new System.Drawing.Font("Arial", patientBodySizeCodes, System.Drawing.FontStyle.Bold);
                    gridColumnLastName.AppearanceCell.Font = new System.Drawing.Font("Arial", patientBodySizeCodes, System.Drawing.FontStyle.Bold);
                    gridColumnServiceReqStt.AppearanceCell.Font = new System.Drawing.Font("Arial", patientBodySizeCodes, System.Drawing.FontStyle.Bold);
                    gridColumnServiceReqType.AppearanceCell.Font = new System.Drawing.Font("Arial", patientBodySizeCodes, System.Drawing.FontStyle.Bold);
                    gridColumnSTT.AppearanceCell.Font = new System.Drawing.Font("Arial", patientBodySizeCodes, System.Drawing.FontStyle.Bold);
                    gridColumnAddress.AppearanceCell.Font = new System.Drawing.Font("Arial", patientBodySizeCodes, System.Drawing.FontStyle.Bold);
                }

                //màu chữ của trạng thái yêu cầu là mới
                newStatusForceColorCodes = WaitingScreenCFG.NEW_STATUS_REQUEST_FORCE_COLOR_CODES;

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void gridViewInDesk_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK data = (HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (data != null)
                    {
                        DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                        if (e.Column.FieldName == "AGE_DISPLAY")
                        {
                            e.Value = GetYearOld(data.TDL_PATIENT_DOB);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridViewWaitingCls_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK data = (HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (data != null)
                    {
                        DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                        if (e.Column.FieldName == "AGE_DISPLAY")
                        {
                            e.Value = GetYearOld(data.TDL_PATIENT_DOB);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private string GetYearOld(long dob)
        {
            string yearDob = "";
            try
            {
                if (dob > 0)
                {
                    yearDob = dob.ToString().Substring(0, 4);
                }
            }
            catch (Exception ex)
            {
                yearDob = "";
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return yearDob;
        }

        private void gridViewWaitingCls_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    bool IsCalling = Inventec.Common.TypeConvert.Parse.ToBoolean((View.GetRowCellValue(e.RowHandle, "IS_CALLING") ?? "").ToString());
                    if (IsCalling)
                    {
                        e.Appearance.Font = new System.Drawing.Font("Arial", 29, FontStyle.Bold);
                        e.HighPriority = true;
                        e.Appearance.BackColor = Color.Blue;
                        e.Appearance.ForeColor = Color.Yellow;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void SetDataToGridControlWaitingCLSs()
        {
            try
            {
                // danh sách chờ kết quả cận lâm sàng

                if (CallPtDataWorker.DicCallPatient != null && CallPtDataWorker.DicCallPatient.Count > 0 && CallPtDataWorker.DicCallPatient[room.ROOM_ID] != null && CallPtDataWorker.DicCallPatient[room.ROOM_ID].Count > 0)
                {
                    int countPatient = 0;
                    try { countPatient = HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<int>(AppConfigKeys.CONFIG_KEY__SO_BENH_NHAN_TREN_DANH_SACH_CHO_KHAM_VA_CLS); }
                    catch (Exception) { };
                    if (countPatient == 0)
                        countPatient = 10;
                    var lisCallNew = CallPtDataWorker.DicCallPatient[room.ROOM_ID].Where(o => o.CALL_TIME > 0).OrderByDescending(p => p.CALL_TIME).GroupBy(g => g.ID).Select(q => q.First()).Take(countPatient).ToList();
                    var listClearCall = new List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>();
                    foreach (var item in lisCallNew)
                    {
                        if (item.IS_CALLING)
                        {
                            listClearCall.Add(new HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK() { ID = item.ID, CALL_TIME = item.CALL_TIME, TDL_PATIENT_NAME = item.TDL_PATIENT_NAME, TDL_PATIENT_DOB = item.TDL_PATIENT_DOB, TDL_PATIENT_ADDRESS = item.TDL_PATIENT_ADDRESS });
                        }
                        else
                        {
                            listClearCall.Add(item);
                        }
                    }
                    var now = DateTime.Now.Second;
                    var s = (int)((now - now % (timerSetDataToGridControl.Interval / 1000)) / (timerSetDataToGridControl.Interval / 1000)) % 2;
                    if (s == 1)
                    {
                        gridControlWaiting.Invoke(new MethodInvoker(delegate
                        {
                            gridControlWaiting.BeginUpdate();
                            gridControlWaiting.DataSource = listClearCall;
                            gridControlWaiting.EndUpdate();
                        }));
                    }
                    else
                    {
                        gridControlWaiting.Invoke(new MethodInvoker(delegate
                        {
                            gridControlWaiting.BeginUpdate();
                            gridControlWaiting.DataSource = lisCallNew;
                            gridControlWaiting.EndUpdate();
                        }));
                    }
                    Inventec.Common.Logging.LogSystem.Info("Du lieu DicCallPatient:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CallPtDataWorker.DicCallPatient[room.ROOM_ID].Take(countPatient).ToList()), CallPtDataWorker.DicCallPatient[room.ROOM_ID].Take(countPatient).ToList()));
                }
                else
                {
                    gridControlWaiting.Invoke(new MethodInvoker(delegate
                    {
                        gridControlWaiting.BeginUpdate();
                        gridControlWaiting.DataSource = null;
                        gridControlWaiting.EndUpdate();
                    }));
                }
                if (CallPtDataWorker.DicDeskPatient != null && CallPtDataWorker.DicDeskPatient.Count > 0 && CallPtDataWorker.DicDeskPatient[room.ROOM_ID] != null && CallPtDataWorker.DicDeskPatient[room.ROOM_ID].Count > 0)
                {
                    //gridControlInDesk.Invoke(new MethodInvoker(delegate
                    //{
                    //    gridControlInDesk.BeginUpdate();
                    //    gridControlInDesk.DataSource = CallPtDataWorker.DicDeskPatient[room.ROOM_ID];
                    //    gridControlInDesk.EndUpdate();
                    //}));
                }
                else
                {
                    //gridControlInDesk.Invoke(new MethodInvoker(delegate
                    //{
                    //    gridControlInDesk.BeginUpdate();
                    //    gridControlInDesk.DataSource = null;
                    //    gridControlInDesk.EndUpdate();
                    //}));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetCallTime()
        {
            try
            {
                CommonParam param = new CommonParam();
                HIS.Desktop.LocalStorage.BackendData.V2.Filter.HisTreatmentSampleDeskViewFilter filter = new HIS.Desktop.LocalStorage.BackendData.V2.Filter.HisTreatmentSampleDeskViewFilter();
                filter.SAMPLE_ROOM_ID = room.ID;
                if (DateTime.Now.Hour >= 12)
                {
                    filter.CALL_TIME_FROM = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Today) + 120000;
                    filter.CALL_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Today) + 235959;
                }
                else
                {
                    filter.CALL_TIME_FROM = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Today);
                    filter.CALL_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Today) + 120000;
                }
                filter.ORDER_FIELD = "CALL_TIME";
                filter.ORDER_DIRECTION = "DESC";
                filter.HASNT_SAMPLE_DESK = true;
                int countPatient = 0;
                try { countPatient = HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<int>(AppConfigKeys.CONFIG_KEY__SO_BENH_NHAN_TREN_DANH_SACH_CHO_KHAM_VA_CLS); }
                catch (Exception) { };
                if (countPatient == 0)
                    countPatient = 10;
                param.Limit = countPatient;
                param.Start = 0;
                mosUserConsummer = new Inventec.Common.WebApiClient.ApiConsumer(HisConfigCFG.MOS_USER_URI, GlobalVariables.APPLICATION_CODE);
                mosUserConsummer.SetTokenCode(HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer.GetTokenCode());

                LogSystem.Debug(HisConfigCFG.MOS_USER_URI);
                var result = new BackendAdapter(param).Get<List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>("api/HisTreatmentSampleDesk/GetView", mosUserConsummer, filter, param);
                //Inventec.Common.Logging.LogSystem.Debug("Data Update." + result.Count);
                if (result != null && result.Count > 0)
                {
                    CallPtDataWorker.DicCallPatient[room.ROOM_ID] = result;

                }
                else
                {
                    CallPtDataWorker.DicCallPatient[room.ROOM_ID] = new List<LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>();
                }
                filter.HASNT_SAMPLE_DESK = false;
                result = new BackendAdapter(param).Get<List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>("api/HisTreatmentSampleDesk/GetView", mosUserConsummer, filter, param);
                //Inventec.Common.Logging.LogSystem.Debug("Data Update." + result.Count);
                if (result != null && result.Count > 0)
                {
                    CallPtDataWorker.DicDeskPatient[room.ROOM_ID] = result;

                }
                else
                {
                    CallPtDataWorker.DicDeskPatient[room.ROOM_ID] = new List<LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>();
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        // gan du lieu vao gridcontrol
        private void timerSetDataToGridControl_Tick(object sender, EventArgs e)
        {
            try
            {
                SetDataToGridControlCLS();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        void SetDataToGridControlCLS()
        {
            try
            {
                Task ts = Task.Factory.StartNew(executeThreadSetDataToGridControl);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        void CallingPatient()
        {
            try
            {
                Task ts = Task.Factory.StartNew(excuteThreadCalling);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        void executeThreadSetDataToGridControl()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { StartTheadSetDataToGridControl(); }));
                }
                else
                {
                    StartTheadSetDataToGridControl();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        void excuteThreadCalling()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { StartThreadCalling(); }));
                }
                else
                {
                    StartThreadCalling();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        void StartTheadSetDataToGridControl()
        {
            SetDataToGridControlWaitingCLSs();
        }

        void StartThreadCalling()
        {
            SetDataLabelPatient();
        }
        void SetDataLabelPatient()
        {
            try
            {
                var CallNew = CallPtDataWorker.DicCallPatient[room.ROOM_ID].Where(o => o.CALL_TIME > 0).OrderByDescending(p => p.CALL_TIME).GroupBy(g => g.ID).Select(q => q.First()).FirstOrDefault();
                if (CallNew != null && CallNew.IS_CALLING)
                {
                    lblCallPatient.Text = CallNew.TDL_PATIENT_NAME.ToUpper() + " " + CallNew.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                }
                else
                {
                    lblCallPatient.Text = "";
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public void CallNumOrder(int min, int max)
        {
            try
            {
                isSetNum = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void frmWaitingScreenSample22_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                timerSetDataToGridControl.Enabled = false;

                timerSetDataToGridControl.Stop();

                timerSetDataToGridControl.Dispose();

                timerCalling.Enabled = false;
                timerCalling.Stop();
                timerCalling.Dispose();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void timerCalling_Tick(object sender, EventArgs e)
        {
            CallingPatient();
        }
    }
}
