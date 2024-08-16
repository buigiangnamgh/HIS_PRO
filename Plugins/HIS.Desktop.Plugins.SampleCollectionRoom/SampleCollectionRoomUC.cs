using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LIS.EFMODEL.DataModels;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
//using HIS.Desktop.LocalStorage.SdaConfigKey.Config;
using DevExpress.XtraGrid.Views.Grid;
using Inventec.Desktop.Common.Message;
using Inventec.Core;
using Inventec.Common.Adapter;
using HIS.Desktop.Controls.Session;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Inventec.Common.Logging;
using HIS.Desktop.LocalStorage.BackendData;
using MOS.EFMODEL.DataModels;
using HIS.Desktop.LocalStorage.ConfigApplication;
using MOS.Filter;
using Inventec.Common.Controls.EditorLoader;
using System.Resources;
using Inventec.Desktop.Common.LanguageManager;
using LIS.Filter;
using HIS.Desktop.Plugins.SampleCollectionRoom.ADO;
using LIS.SDO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Plugins.SampleCollectionRoom.Config;
using DevExpress.XtraBars;
using HIS.Desktop.Common;
using System.Globalization;
using HIS.Desktop.LocalStorage.LocalData;
using AutoMapper;
using HIS.Desktop.Utility;
using Bartender.PrintClient;
using MOS.SDO;
using HIS.Desktop.Utilities.Extensions;
using HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL;
using HIS.Desktop.LocalStorage.ConfigSystem;
using HIS.UC.TreeSereServ7V2;

namespace HIS.Desktop.Plugins.SampleCollectionRoom
{
    public partial class SampleCollectionRoomUC : HIS.Desktop.Utility.UserControlBase
    {
        #region Declare
        HIS_SAMPLE_ROOM SampleRoom = null;
        Inventec.Desktop.Common.Modules.Module currentModule;
        bool statecheckColumn = false;
        internal Inventec.Core.ApiResultObject<List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>> apiResult;
        internal TreatmentSampleListViewADO rowSample { get; set; }
        int lastRowHandle = -1;
        GridColumn lastColumn = null;
        ToolTipControlInfo lastInfo = null;
        List<TreatmentSampleListViewADO> lstAll { get; set; }
        List<SampleDeskCounterADO> lstAllCounter { get; set; }
        int rowCount = 0;
        int dataTotal = 0;
        int startPage = 0;
        int limit = 0;

        List<HIS_TREATMENT_TYPE> listTreatmentType;
        List<HIS_TREATMENT_TYPE> _DienDieuTriSelecteds;
        CPA.WCFClient.CallPatientClient.CallPatientClientManager clienttManager = null;
        BarManager baManager = null;
        PopupMenuProcessor popupMenuProcessor = null;
        Inventec.Common.WebApiClient.ApiConsumer mosUserConsummer;
        public static List<HIS.Desktop.Library.CacheClient.ControlStateRDO> currentControlStateRDO;
        public static HIS.Desktop.Library.CacheClient.ControlStateWorker controlStateWorker;
        bool isNotLoadWhileChangeControlStateInFirst;
        List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK> listUpdateSampleDesk;
        int numPageSize;
        //List<L_HIS_SERVICE_REQ> ServiceReqCurrentTreatment { get; set; }
        List<LocalStorage.BackendData.V2.EFMODEL.L_HIS_SERVICE_REQ_101> hisServiceReq101List;
        V_HIS_TREATMENT_SAMPLE_DESK treatmentSampleDeskPrint;
        #endregion

        #region Contructor

        public SampleCollectionRoomUC()
        {
            InitializeComponent();
            lblDaThanhToan.Text = "";
            HisConfigCFG.LoadConfig();
            GetConsumer();
        }

        public SampleCollectionRoomUC(Inventec.Desktop.Common.Modules.Module currentModule)
            : base(currentModule)
        {
            InitializeComponent();
            try
            {
                lblDaThanhToan.Text = "";
                this.currentModule = currentModule;
                HisConfigCFG.LoadConfig();
                GetConsumer();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        private void UC_SampleCollectionRoomUC_Load(object sender, EventArgs e)
        {
            try
            {
                if (SampleRoom == null)
                {
                    SampleRoom = BackendDataWorker.Get<HIS_SAMPLE_ROOM>().FirstOrDefault(o => o.ROOM_ID == currentModule.RoomId);
                }
                InitControlState();
                LoadDefaultData();
                InitTreatmentArea();
                InitCheck(cboTreatmentArea, SelectionGrid__TreatmentArea);
                InitCombo(cboTreatmentArea, listTreatmentType, "TREATMENT_TYPE_NAME", "ID");
                this.gridControlTreatmentSampleDesk.ToolTipController = this.toolTipControllerGrid;
                this.gridControlSampleDeskCounter.ToolTipController = this.toolTipController1;
                FillDataToGridControl();

                txtGateNumber.Text = this.SampleRoom.SAMPLE_ROOM_CODE;
                setSizeInformationPatient();
                SetCheckAllColumn(this.statecheckColumn);
                LoadDefaultScreenSaver();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void GetConsumer()
        {
            mosUserConsummer = new Inventec.Common.WebApiClient.ApiConsumer(HisConfigCFG.MOS_USER_URI, GlobalVariables.APPLICATION_CODE);
            mosUserConsummer.SetTokenCode(HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer.GetTokenCode());
        }

        private void InitTreatmentArea()
        {
            try
            {
                this.listTreatmentType = BackendDataWorker.Get<HIS_TREATMENT_TYPE>();
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();
                ControlEditorADO controlEditorADO = new ControlEditorADO("TREATMENT_TYPE_NAME", "ID", columnInfos, false, 200);
                ControlEditorLoader.Load(cboTreatmentArea, this.listTreatmentType, controlEditorADO);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void InitCombo(GridLookUpEdit cbo, object data, string DisplayValue, string ValueMember)
        {
            try
            {
                cbo.Properties.DataSource = data;
                cbo.Properties.DisplayMember = DisplayValue;
                cbo.Properties.ValueMember = ValueMember;

                DevExpress.XtraGrid.Columns.GridColumn col2 = cbo.Properties.View.Columns.AddField(DisplayValue);
                col2.VisibleIndex = 1;
                col2.Width = 200;
                col2.Caption = "Tất cả";

                cbo.Properties.PopupFormWidth = 200;
                cbo.Properties.View.OptionsView.ShowColumnHeaders = true;
                cbo.Properties.View.OptionsSelection.MultiSelect = true;

                GridCheckMarksSelection gridCheckMark = cbo.Properties.Tag as GridCheckMarksSelection;
                if (gridCheckMark != null)
                {
                    gridCheckMark.ClearSelection(cbo.Properties.View);
                    gridCheckMark.SelectAll(this.listTreatmentType);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void SelectionGrid__TreatmentArea(object sender, EventArgs e)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                _DienDieuTriSelecteds = new List<HIS_TREATMENT_TYPE>();
                foreach (HIS_TREATMENT_TYPE rv in (sender as GridCheckMarksSelection).Selection)
                {
                    if (rv != null)
                    {
                        _DienDieuTriSelecteds.Add(rv);
                        if (rv != null)
                        {
                            if (sb.ToString().Length > 0) { sb.Append(", "); }
                            sb.Append(rv.TREATMENT_TYPE_NAME);
                        }
                    }
                }


                //GridCheckMarksSelection gridCheckMark = sender as GridCheckMarksSelection;
                //if (gridCheckMark != null)
                //{
                //    List<HIS_TREATMENT_TYPE> erSelectedNews = new List<HIS_TREATMENT_TYPE>();
                //    foreach (HIS_TREATMENT_TYPE er in (sender as GridCheckMarksSelection).Selection)
                //    {
                //        if (er != null)
                //        {
                //            if (sb.ToString().Length > 0) { sb.Append(", "); }
                //            sb.Append(er.TREATMENT_TYPE_NAME);
                //            erSelectedNews.Add(er);
                //        }

                //    }
                //}
                this.cboTreatmentArea.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void InitCheck(GridLookUpEdit cbo, GridCheckMarksSelection.SelectionChangedEventHandler eventSelect)
        {
            try
            {
                GridCheckMarksSelection gridCheck = new GridCheckMarksSelection(cbo.Properties);
                gridCheck.SelectionChanged += new GridCheckMarksSelection.SelectionChangedEventHandler(eventSelect);
                cbo.Properties.Tag = gridCheck;
                cbo.Properties.View.OptionsSelection.MultiSelect = true;
                GridCheckMarksSelection gridCheckMark = cbo.Properties.Tag as GridCheckMarksSelection;
                if (gridCheckMark != null)
                {
                    gridCheckMark.ClearSelection(cbo.Properties.View);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        #endregion

        #region Gridview Sample

        private void toolTipControllerGrid_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            try
            {
                if (e.Info == null && e.SelectedControl == gridControlTreatmentSampleDesk)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = gridControlTreatmentSampleDesk.FocusedView as DevExpress.XtraGrid.Views.Grid.GridView;
                    GridHitInfo info = view.CalcHitInfo(e.ControlMousePosition);
                    if (info.InRowCell)
                    {
                        if (lastRowHandle != info.RowHandle || lastColumn != info.Column)
                        {
                            lastColumn = info.Column;
                            lastRowHandle = info.RowHandle;
                            string text = "";
                            if (info.Column.FieldName == "STATUS")
                            {
                                //text = (view.GetRowCellValue(lastRowHandle, "SAMPLE_STT_NAME") ?? "").ToString();
                            }
                            lastInfo = new ToolTipControlInfo(new DevExpress.XtraGrid.GridToolTipInfo(view, new DevExpress.XtraGrid.Views.Base.CellToolTipInfo(info.RowHandle, info.Column, "Text")), text);
                        }
                        e.Info = lastInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridViewSample_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                WaitingManager.Show();
                rowSample = null;
                rowSample = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                LoadInformationPatient();
                LoadServiceReq(rowSample);
                //LoadDataToPanelRight(this.hisServiceReq101List);

                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void InitControlState()
        {
            try
            {
                isNotLoadWhileChangeControlStateInFirst = true;
                SampleCollectionRoomUC.controlStateWorker = new HIS.Desktop.Library.CacheClient.ControlStateWorker();
                SampleCollectionRoomUC.currentControlStateRDO = SampleCollectionRoomUC.controlStateWorker.GetData(ControlStateConstant.MODULE_LINK);
                if (SampleCollectionRoomUC.currentControlStateRDO != null && SampleCollectionRoomUC.currentControlStateRDO.Count > 0)
                {
                    foreach (var item in SampleCollectionRoomUC.currentControlStateRDO)
                    {
                        if (item.KEY == ControlStateConstant.CHECK_PRINT_NOW)
                        {
                            chkPrintNow.Checked = item.VALUE == "1";
                        }
                        if (item.KEY == ControlStateConstant.CHECK_DA_PHAN_O)
                        {
                            chkDaPhanO.Checked = item.VALUE == "1";
                        }
                        if (item.KEY == ControlStateConstant.CHECK_CHUA_PHAN_O)
                        {
                            chkChuaPhanO.Checked = item.VALUE == "1";
                        }
                        if (item.KEY == ControlStateConstant.CHECK_CO_BHYT)
                        {
                            chkBaoHiemTinhTien.Checked = item.VALUE == "1";
                        }
                        if (item.KEY == ControlStateConstant.CHECK_GOI_SAU_KHI_QUET)
                        {
                            chkGoiSauKhiQuet.Checked = item.VALUE == "1";
                        }
                        if (item.KEY == chkScreenSaver.Name)
                        {
                            chkScreenSaver.Checked = item.VALUE == "1";
                        }
                    }
                }

                isNotLoadWhileChangeControlStateInFirst = false;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        /// <summary>
        /// mở màn hình mặc định
        /// </summary>
        private void LoadDefaultScreenSaver()
        {
            try
            {
                if (chkScreenSaver.Checked)
                {
                    List<object> _listObj = new List<object>();
                    WaitingManager.Hide();
                    var SCREEN_SAVER = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault(o => o.ID == this.SampleRoom.ROOM_ID);
                    if (SCREEN_SAVER != null)
                    {
                        if (!string.IsNullOrEmpty(SCREEN_SAVER.SCREEN_SAVER_MODULE_LINK))
                        {
                            HIS.Desktop.ModuleExt.PluginInstanceBehavior.ShowModule(SCREEN_SAVER.SCREEN_SAVER_MODULE_LINK, this.SampleRoom.ROOM_ID, SCREEN_SAVER.ROOM_TYPE_ID, _listObj);
                        }
                        else
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show(string.Format("Không có màn hình chờ mặc định", string.Join(",", SCREEN_SAVER.ROOM_NAME)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

        }

        private void LoadServiceReq(TreatmentSampleListViewADO treatmentSample)
        {
            try
            {
                List<ServiceADO> ServiceAll = new List<ServiceADO>();

                CommonParam param = new CommonParam();
                HIS.Desktop.LocalStorage.BackendData.V2.Filter.HisServiceReqLView101Filter filter = new LocalStorage.BackendData.V2.Filter.HisServiceReqLView101Filter();
                filter.ASSIGN_TURN_CODE__EXACT = treatmentSample.TDL_ASSIGN_TURN_CODE;
                filter.TREATMENT_ID = treatmentSample.TREATMENT_ID;
                this.hisServiceReq101List = new BackendAdapter(param).Get<List<LocalStorage.BackendData.V2.EFMODEL.L_HIS_SERVICE_REQ_101>>("api/HisServiceReq/GetLView101", mosUserConsummer, filter, param);

                var parentListTemp = this.hisServiceReq101List.Where(o => o.PARENT_SERVICE_ID.HasValue && o.PARENT_SERVICE_ID.Value > 0).ToList();
                var groupParent = parentListTemp.GroupBy(o => o.PARENT_SERVICE_ID).ToList();

                var maxId = this.hisServiceReq101List.Max(o => o.ID);
                foreach (var item in groupParent)
                {
                    ServiceADO s1 = new ServiceADO();
                    s1.IdService = item.First().PARENT_SERVICE_ID ?? 0;
                    s1.ServiceCode = item.First().PARENT_SERVICE_CODE;
                    s1.ServiceName = item.First().PARENT_SERVICE_NAME;
                    s1.ParentServiceId = null;
                    ServiceAll.Add(s1);
                }
                foreach (var item in this.hisServiceReq101List)
                {
                    maxId++;
                    ServiceADO s1 = new ServiceADO();
                    s1.IdService = maxId;
                    s1.ServiceCode = item.TDL_SERVICE_CODE;
                    s1.ServiceName = item.TDL_SERVICE_NAME;
                    s1.ParentServiceId = item.PARENT_SERVICE_ID;
                    ServiceAll.Add(s1);
                }

                treeListService.DataSource = ServiceAll;
                treeListService.ParentFieldName = "ParentServiceId";
                treeListService.KeyFieldName = "IdService";
                treeListService.ExpandAll();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void setSizeInformationPatient()
        {
            try
            {
                string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
                string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();
                if (Int64.Parse(screenHeight) >= 768)
                {
                    layoutControlItem5.Size = new System.Drawing.Size(812, 240);
                }
                if (Int64.Parse(screenHeight) >= 900)
                {
                    layoutControlItem5.Size = new System.Drawing.Size(812, 190);
                }
                if (Int64.Parse(screenHeight) >= 1000)
                {
                    layoutControlItem5.Size = new System.Drawing.Size(812, 160);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void LoadInformationPatient()
        {
            try
            {
                rowSample = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                CommonParam param = new CommonParam();
                lblPatientCode.Text = rowSample.TDL_PATIENT_CODE;
                lblPatientName.Text = rowSample.TDL_PATIENT_NAME;

                lblDOB.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rowSample.TDL_PATIENT_DOB.ToString());

                lblTreatmentType.Text = rowSample.PATIENT_TYPE_NAME;
                if (rowSample.IsBhytOrPaid)
                {
                    lblDaThanhToan.Text = "Đã đóng tiền";
                }
                else
                {
                    lblDaThanhToan.Text = "";
                }
                HisTreatmentViewFilter Filter = new HisTreatmentViewFilter();
                Filter.ID = rowSample.TREATMENT_ID;
                var treatment = new BackendAdapter(param).Get<List<V_HIS_TREATMENT>>("api/HisTreatment/GetView", this.mosUserConsummer, Filter, param);
                if (treatment != null && treatment.Count > 0)
                {
                    lblIcdName.Text = treatment[0].ICD_CODE + " - " + treatment[0].ICD_NAME;
                    lblIcdText.Text = treatment[0].ICD_SUB_CODE + " - " + treatment[0].ICD_TEXT;
                    lblGender.Text = treatment[0].TDL_PATIENT_GENDER_NAME;
                    lblAdress.Text = treatment[0].TDL_PATIENT_ADDRESS;
                    lblHeinCardNumber.Text = treatment[0].TDL_HEIN_CARD_NUMBER;
                    lblHeinMediOrgCode.Text = treatment[0].TDL_HEIN_MEDI_ORG_CODE;
                    lblHanTu.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment[0].TDL_HEIN_CARD_FROM_TIME ?? 0) + " - " + Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment[0].TDL_HEIN_CARD_TO_TIME ?? 0);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridViewSample_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                GridView View = sender as GridView;
                if (e.RowHandle >= 0)
                {
                    //var sampleStt = Inventec.Common.TypeConvert.Parse.ToInt64((gridViewTreatmentSampleDesk.GetRowCellValue(e.RowHandle, "SAMPLE_STT_ID")).ToString());
                    var data = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetRow(e.RowHandle);
                    if (data == null) return;

                    if (e.Column.FieldName == "CALL_PATIENT")
                    {
                        if (data.IS_CALLING)
                            e.RepositoryItem = ButtonEdit_CallPatientDisable;
                        else
                            e.RepositoryItem = ButtonEdit_CallPatientEnable;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridViewSample_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    TreatmentSampleListViewADO data = (TreatmentSampleListViewADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (data != null)
                    {
                        if (e.Column.FieldName == "STT")
                        {
                            e.Value = e.ListSourceRowIndex + 1 + startPage;
                        }
                        else if (e.Column.FieldName == "STATUS")
                        {
                        }
                        else if (e.Column.FieldName == "DOB_DISPLAY")
                        {
                            e.Value = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.TDL_PATIENT_DOB);
                        }
                        else if (e.Column.FieldName == "CREATE_TIME_STR")
                        {
                            e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(data.CREATE_TIME ?? 0);
                        }
                        else if (e.Column.FieldName == "MODIFY_TIME_STR")
                        {
                            e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(data.MODIFY_TIME ?? 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        #endregion

        #region Method

        internal void FillDataToGridControl()
        {
            txtFindTreamentCode.Focus();
            txtFindTreamentCode.SelectAll();

            if (ucPaging1.pagingGrid != null)
            {
                numPageSize = ucPaging1.pagingGrid.PageSize;
            }
            else
            {
                numPageSize = (int)ConfigApplications.NumPageSize;
            }

            FillDataToGridSample(new CommonParam(0, (int)ConfigApplications.NumPageSize));

            CommonParam param = new CommonParam();
            param.Limit = rowCount;
            param.Count = dataTotal;
            ucPaging1.Init(FillDataToGridSample, param, numPageSize, this.gridControlTreatmentSampleDesk);
            LoadDataToGridSampleDeskCounter();

        }

        internal void FillDataToGridSample(object param)
        {
            try
            {
                WaitingManager.Show();
                startPage = ((CommonParam)param).Start ?? 0;
                limit = ((CommonParam)param).Limit ?? 0;
                CommonParam paramCommon = new CommonParam(startPage, limit);

                gridControlTreatmentSampleDesk.DataSource = null;


                HIS.Desktop.LocalStorage.BackendData.V2.Filter.HisTreatmentSampleDeskViewFilter lisSampleFilter = new HIS.Desktop.LocalStorage.BackendData.V2.Filter.HisTreatmentSampleDeskViewFilter();
                if (chkChuaPhanO.Checked && chkDaPhanO.Checked)
                {
                    lisSampleFilter.HASNT_SAMPLE_DESK = null;
                }
                else if (chkDaPhanO.Checked)
                {
                    lisSampleFilter.HASNT_SAMPLE_DESK = false;
                }
                else if (chkChuaPhanO.Checked)
                {
                    lisSampleFilter.HASNT_SAMPLE_DESK = true;
                }

                lisSampleFilter.SAMPLE_ROOM_ID = SampleRoom.ID;
                if (!String.IsNullOrWhiteSpace(txtFindTreamentCode.Text))
                {
                    string code = txtFindTreamentCode.Text.Trim();
                    if (code.Length < 12)
                    {
                        code = string.Format("{0:000000000000}", Convert.ToInt64(code));
                        txtFindTreamentCode.Text = code;
                    }
                    lisSampleFilter.TREATMENT_CODE__EXACT = code;
                }
                else if (!String.IsNullOrWhiteSpace(txtFindPatientCode.Text))
                {
                    string code = txtFindPatientCode.Text.Trim();
                    if (code.Length < 10)
                    {
                        code = string.Format("{0:0000000000}", Convert.ToInt64(code));
                        txtFindPatientCode.Text = code;
                    }
                    lisSampleFilter.PATIENT_CODE__EXACT = code;
                }
                else
                {
                    if (!String.IsNullOrEmpty(txtSearchKey.Text))
                        lisSampleFilter.KEY_WORD = txtSearchKey.Text.Trim();
                    if (dtCreatefrom != null && dtCreatefrom.DateTime != DateTime.MinValue)
                        lisSampleFilter.CREATE_TIME_FROM = Inventec.Common.TypeConvert.Parse.ToInt64(Convert.ToDateTime(dtCreatefrom.EditValue).ToString("yyyyMMdd") + "000000");
                    if (dtCreateTo != null && dtCreateTo.DateTime != DateTime.MinValue)
                        lisSampleFilter.CREATE_TIME_TO = Inventec.Common.TypeConvert.Parse.ToInt64(Convert.ToDateTime(dtCreateTo.EditValue).ToString("yyyyMMdd") + "235959");
                    if (chkBaoHiemTinhTien.Checked)
                    {
                        lisSampleFilter.IS_BHYT_OR_PAID = true;
                    }
                    else
                    {
                        lisSampleFilter.IS_BHYT_OR_PAID = null;
                    }
                }

                lisSampleFilter.ORDER_FIELD = "CREATE_TIME";
                lisSampleFilter.ORDER_DIRECTION = "ASC";
                lisSampleFilter.ORDER_FIELD1 = "IS_PRIORITY";
                lisSampleFilter.ORDER_DIRECTION1 = "ASC";

                List<long> vs = new List<long>();
                if (cboTreatmentArea.EditValue != null)
                {
                    if (this._DienDieuTriSelecteds != null && this._DienDieuTriSelecteds.Count > 0)
                    {
                        lisSampleFilter.TREATMENT_TYPE_IDs = this._DienDieuTriSelecteds.Select(o => o.ID).ToList();
                    }

                    if (cboTreatmentArea.EditValue is List<long>)
                    {
                        lisSampleFilter.TREATMENT_TYPE_IDs = (List<long>)cboTreatmentArea.EditValue;
                    }
                }

                apiResult = new ApiResultObject<List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>();

                apiResult = new BackendAdapter(paramCommon).GetRO<List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>>("api/HisTreatmentSampleDesk/GetView", mosUserConsummer, lisSampleFilter, paramCommon);

                if (apiResult != null)
                {
                    WaitingManager.Hide();
                    var data = (List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>)apiResult.Data;
                    if (data != null)
                    {
                        List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK> distinctData = new List<V_HIS_TREATMENT_SAMPLE_DESK>();
                        distinctData = data.GroupBy(p => new { p.TREATMENT_ID, p.TDL_ASSIGN_TURN_CODE })
                            .Select(g => g.First())
                            .ToList();
                        lstAll = new List<TreatmentSampleListViewADO>();
                        foreach (var item in distinctData)
                        {
                            lstAll.Add(new TreatmentSampleListViewADO(item));
                        }
                        gridControlTreatmentSampleDesk.DataSource = lstAll;
                        rowCount = (distinctData == null ? 0 : distinctData.Count);
                        dataTotal = (apiResult.Param == null ? 0 : apiResult.Param.Count ?? 0);
                        if (distinctData.Count == 1)
                        {
                            gridViewTreatmentSampleDesk.FocusedRowHandle = 0;
                        }
                    }
                    #region Process has exception
                    SessionManager.ProcessTokenLost((CommonParam)param);
                    #endregion
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                WaitingManager.Hide();
            }
        }

        private void SetCheckAllColumn(bool state)
        {
            try
            {
                this.grcChecked.ImageAlignment = StringAlignment.Center;
                this.grcChecked.Image = (state ? this.imageCollectionV2.Images[1] : this.imageCollectionV2.Images[0]);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }


        void LoadDefaultData()
        {
            try
            {
                txtSearchKey.Focus();
                txtSearchKey.SelectAll();
                dtCreatefrom.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(Inventec.Common.DateTime.Get.StartDay() ?? 0);
                dtCreateTo.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(Inventec.Common.DateTime.Get.EndDay() ?? 0);
                gridControlTreatmentSampleDesk.DataSource = null;
                gridControlSampleDeskCounter.DataSource = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private bool checkDigit(string s)
        {
            bool result = false;
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (char.IsDigit(s[i]) == true) result = true;
                    else result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return result;
            }
        }

        #region UpdateStt
        private void UpdateStt(long sampleSTT)
        {
            try
            {
                WaitingManager.Show();
                bool result = false;
                CommonParam param = new CommonParam();
                LIS.SDO.UpdateSampleSttSDO updateStt = new LIS.SDO.UpdateSampleSttSDO();
                var row = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                if (row != null)
                {
                    updateStt.Id = row.ID;
                    updateStt.SampleSttId = sampleSTT;

                    var curentSTT = new BackendAdapter(param).Post<LIS_SAMPLE>("/api/LisSample/UpdateStt", ApiConsumer.ApiConsumers.LisConsumer, updateStt, param);
                    if (curentSTT != null)
                    {
                        result = true;
                        FillDataToGridControl();
                        WaitingManager.Hide();
                    }
                }
                WaitingManager.Hide();
                #region Show message
                MessageManager.Show(this.ParentForm, param, result);
                #endregion

                #region Process has exception
                SessionManager.ProcessTokenLost(param);
                #endregion
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        #endregion

        #endregion

        #region Event Button Sample

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    gridControlSampleDeskCounter.BeginUpdate();
                    gridControlSampleDeskCounter.DataSource = null;
                    gridControlSampleDeskCounter.EndUpdate();
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
                FillDataToGridControl();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }


        private void DuyetE_Click(object sender, EventArgs e)
        {
            try
            {

                WaitingManager.Show();
                bool result = false;
                CommonParam param = new CommonParam();
                var row = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                if (row != null && (row.SAMPLE_DESK_ID == IMSys.DbConfig.LIS_RS.LIS_SAMPLE_STT.ID__CHUA_LM
                    || row.SAMPLE_DESK_ID == IMSys.DbConfig.LIS_RS.LIS_SAMPLE_STT.ID__TU_CHOI))
                {

                    {
                        WaitingManager.Show();
                        LisSampleSampleSDO sdo = new LisSampleSampleSDO();
                        sdo.SampleId = row.ID;
                        sdo.RequestRoomCode = SampleRoom.SAMPLE_ROOM_NAME;
                        var curentSTT = new BackendAdapter(param).Post<LIS_SAMPLE>("api/LisSample/Sample", ApiConsumer.ApiConsumers.LisConsumer, sdo, param);
                        if (curentSTT != null)
                        {
                            rowSample = row;
                            //rowSample.SAMPLE_STT_ID = curentSTT.SAMPLE_STT_ID;
                            //rowSample.SAMPLE_TYPE_ID = curentSTT.SAMPLE_TYPE_ID;
                            //rowSample.SAMPLE_TIME = curentSTT.SAMPLE_TIME;
                            //rowSample.SAMPLE_LOGINNAME = curentSTT.SAMPLE_LOGINNAME;
                            //rowSample.SAMPLE_USERNAME = curentSTT.SAMPLE_USERNAME;
                            //rowSample.SAMPLE_ORDER = curentSTT.SAMPLE_ORDER;

                            FillDataToGridControl();
                            result = true;
                            gridViewTreatmentSampleDesk.RefreshData();
                        }
                        WaitingManager.Hide();
                        #region Show message
                        MessageManager.Show(this.ParentForm, param, result);
                        #endregion

                        #region Process has exception
                        SessionManager.ProcessTokenLost(param);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                WaitingManager.Hide();
            }
        }
        private void HuyDuyetE_Click(object sender, EventArgs e)
        {
            try
            {
                var row = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                if (row != null)
                {
                    //frmCancelReason frm = new frmCancelReason(this.currentModuleBase, row, SampleRoom.ROOM_CODE, FillDataToGridControl);
                    //frm.ShowDialog();
                    //gridViewTreatmentSampleDesk.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void txtSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    FillDataToGridControl();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        #endregion

        #region Print Barcode

        internal enum PrintType
        {
            IN_BARCODE,
            IN_PHIEU_HEN
        }



        #endregion

        #region Print Result

        internal enum PrintTypeKXN
        {
            IN_KET_QUA_XET_NGHIEM,
        }


        private void CallModuleShowPrintLog(string printTypeCode, string uniqueCode)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(printTypeCode) && !String.IsNullOrWhiteSpace(uniqueCode))
                {
                    //goi modul
                    HIS.Desktop.ADO.PrintLogADO ado = new HIS.Desktop.ADO.PrintLogADO(printTypeCode, uniqueCode);

                    List<object> listArgs = new List<object>();
                    listArgs.Add(ado);

                    HIS.Desktop.ModuleExt.PluginInstanceBehavior.ShowModule("Inventec.Desktop.Plugins.PrintLog", this.currentModule.RoomId, this.currentModule.RoomTypeId, listArgs);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        #endregion

        private void SetCaptionByLanguageKey()
        {
            try
            {
                if (this.currentModule != null && !String.IsNullOrEmpty(this.currentModule.text))
                {
                    this.Text = this.currentModule.text;
                }
                ////Khoi tao doi tuong resource
                Resources.ResourceLanguageManager.LanguageResource = new ResourceManager("HIS.Desktop.Plugins.SampleCollectionRoom.Resources.Lang", typeof(HIS.Desktop.Plugins.SampleCollectionRoom.SampleCollectionRoomUC).Assembly);

                ////Gan gia tri cho cac control editor co Text/Caption/ToolTip/NullText/NullValuePrompt/FindNullPrompt
                this.layoutControl1.Text = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.layoutControl1.Text", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                //this.btnResetNumOrder.Text = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.btnResetNumOrder.Text", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.layoutControl2.Text = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.layoutControl2.Text", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                //this.btnSave.Text = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.btnSave.Text", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                //this.btnPrint.Text = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.btnPrint.Text", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.grdCollSTT.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.grdCollSTT.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.grdColIndexCode.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.grdColIndexCode.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.grdColIndexName.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.grdColIndexName.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.btnSearch.Text = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.btnSearch.Text", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.txtSearchKey.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.txtSearchKey.Properties.NullValuePrompt", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn1.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn1.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn2.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn2.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn3.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn3.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn5.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn5.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn6.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn6.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn7.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn7.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn15.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn15.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn10.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn10.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn17.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn17.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.gridColumn11.Caption = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.gridColumn11.Caption", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.layoutControlItem11.Text = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.layoutControlItem11.Text", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.layoutControlItem12.Text = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.layoutControlItem12.Text", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
                this.layoutControlItem34.Text = Inventec.Common.Resource.Get.Value("UC_ConnectionTest.layoutControlItem34.Text", Resources.ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridViewTestResult_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {

        }

        private void LoadDataToGridSampleDeskCounter()
        {
            try
            {
                this.lstAllCounter = new List<SampleDeskCounterADO>();
                CommonParam param = new CommonParam();
                List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.L_HIS_SAMPLE_DESK_COUNTER> sampleDeskCounter = new List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.L_HIS_SAMPLE_DESK_COUNTER>();
                HIS.Desktop.LocalStorage.BackendData.V2.Filter.HisSampleDeskLViewCounterFilter filter = new HIS.Desktop.LocalStorage.BackendData.V2.Filter.HisSampleDeskLViewCounterFilter();
                filter.SAMPLE_ROOM_ID = this.SampleRoom.ID;
                filter.ORDER_FIELD = "NUM_ORDER";
                filter.ORDER_DIRECTION = "ASC";
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                sampleDeskCounter = new BackendAdapter(param).Get<List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.L_HIS_SAMPLE_DESK_COUNTER>>("api/HisSampleDesk/GetLViewCounter", this.mosUserConsummer, filter, param);
                if (sampleDeskCounter != null && sampleDeskCounter.Count > 0)
                {
                    foreach (var item in sampleDeskCounter)
                    {
                        SampleDeskCounterADO ado = new SampleDeskCounterADO(item);
                        this.lstAllCounter.Add(ado);
                    }
                }
                gridControlSampleDeskCounter.BeginUpdate();
                gridControlSampleDeskCounter.DataSource = this.lstAllCounter;
                gridControlSampleDeskCounter.EndUpdate();
                gridViewSampleDeskCounter.FocusedRowHandle = 0;
                gridViewSampleDeskCounter.SetRowCellValue(gridViewSampleDeskCounter.FocusedRowHandle, "IsChecked", true);
                //gridViewSampleDeskCounter.FocusedColumn = gridViewSampleDeskCounter.VisibleColumns[3];
                gridViewSampleDeskCounter.ShowEditor();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void gridViewTestResult_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                //long is_parent = Inventec.Common.TypeConvert.Parse.ToInt64(gridViewSampleDeskCounter.GetRowCellValue(e.RowHandle, "IS_PARENT").ToString());
                //long has_one_child = Inventec.Common.TypeConvert.Parse.ToInt64(gridViewSampleDeskCounter.GetRowCellValue(e.RowHandle, "HAS_ONE_CHILD").ToString());
                //if (is_parent == 1 || has_one_child == 1)
                //{
                //    e.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void gridViewTestResult_RowStyle(object sender, RowStyleEventArgs e)
        {

        }

        private void repositoryItemTextValue_Enable_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void gridViewTestResult_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                //if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                //{
                //    SampleLisResultADO data = (SampleLisResultADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                //    if (data != null && data.IS_PARENT != 1)
                //    {
                //        if (e.Column.FieldName == "IMAGE")
                //        {
                //            long statusId = data.SAMPLE_SERVICE_STT_ID.Value;

                //            if (statusId == IMSys.DbConfig.LIS_RS.LIS_SAMPLE_SERVICE_STT.ID__CHUA_CO_KQ)
                //            {
                //                e.Value = imageList1.Images[0];
                //            }
                //            else if (statusId == IMSys.DbConfig.LIS_RS.LIS_SAMPLE_SERVICE_STT.ID__DA_CO_KQ)
                //            {

                //                e.Value = imageList1.Images[1];
                //            }
                //            else if (statusId == IMSys.DbConfig.LIS_RS.LIS_SAMPLE_SERVICE_STT.ID__DA_TRA_KQ)
                //            {
                //                e.Value = imageList1.Images[2];
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void toolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            try
            {
                if (e.Info == null && e.SelectedControl == gridControlSampleDeskCounter)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = gridControlSampleDeskCounter.FocusedView as DevExpress.XtraGrid.Views.Grid.GridView;
                    GridHitInfo info = view.CalcHitInfo(e.ControlMousePosition);
                    if (info.InRowCell)
                    {
                        if (lastRowHandle != info.RowHandle || lastColumn != info.Column)
                        {
                            lastColumn = info.Column;
                            lastRowHandle = info.RowHandle;
                            string text = "";
                            if (info.Column.FieldName == "IMAGE")
                            {
                                //var data = ((SampleLisResultADO)view.GetRow(lastRowHandle));
                                //text = data.SAMPLE_SERVICE_STT_NAME;
                            }
                            lastInfo = new ToolTipControlInfo(new DevExpress.XtraGrid.GridToolTipInfo(view, new DevExpress.XtraGrid.Views.Base.CellToolTipInfo(info.RowHandle, info.Column, "Text")), text);
                        }
                        e.Info = lastInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridViewTestResult_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                //if (e.Column.FieldName != "VALUE_RANGE")
                //{
                //    return;
                //}
                //var row = (SampleLisResultADO)gridViewSampleDeskCounter.GetFocusedRow();
                //if (row != null && row.LIS_RESULT_ID > 0 && !string.IsNullOrEmpty(row.VALUE_RANGE))
                //{
                //    row.Item_Edit_Value = 1;
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void gridViewTestResult_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            try
            {
                //if (e.RowHandle >= 0 && e.Column.FieldName == "DX$CheckboxSelectorColumn")
                //{
                //    var data = (SampleLisResultADO)gridViewSampleDeskCounter.GetRow(e.RowHandle);
                //    if (data != null && data.IS_PARENT != 1)
                //    {
                //        e.Handled = true;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void gridViewTestResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        public void SEARCH()
        {
            try
            {
                btnFind_Click(null, null);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void FocusF1()
        {
            try
            {
                txtFindPatientCode.Focus();
                txtFindPatientCode.SelectAll();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void FocusF2()
        {
            try
            {
                txtFindTreamentCode.Focus();
                txtFindTreamentCode.SelectAll();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void FocusF3()
        {
            try
            {
                txtSearchKey.Focus();
                txtSearchKey.SelectAll();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void ShotcurtReCall()
        {
            try
            {
                btnRecallPatient_Click(null, null);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void ShotcurtCall()
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("F6 call patient");
                btnCallPatient_Click(null, null);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void PrintBarcode()
        {
            try
            {
                MessageBox.Show("In barcode");
                btnPrintBarcode_Click(null, null);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void gridViewTestResult_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                {
                    GridView view = sender as GridView;
                    GridHitInfo hi = view.CalcHitInfo(e.Location);
                    if (hi.Column.FieldName == "IsChecked")
                    {
                        if (hi.InColumnPanel)
                        {
                            statecheckColumn = !statecheckColumn;
                            this.SetCheckAllColumn(statecheckColumn);
                            this.GridCheckChange(statecheckColumn);
                        }
                        else if (hi.InRowCell)
                        {
                            if (lstAll.Where(o => o.IsChecked).Count() == lstAll.Count)
                            {
                                statecheckColumn = true;
                                this.SetCheckAllColumn(statecheckColumn);
                            }
                            else
                            {
                                statecheckColumn = false;
                                this.SetCheckAllColumn(statecheckColumn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void txtSearchKey_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearchKey.Focus();
                txtSearchKey.SelectAll();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void gridViewTestResult_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                //DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                //SampleLisResultADO data = view.GetFocusedRow() as SampleLisResultADO;
                //if (view.FocusedColumn.FieldName == "MACHINE_ID" && view.ActiveEditor is GridLookUpEdit && data.IS_PARENT == 1)
                //{
                //    GridLookUpEdit editor = view.ActiveEditor as GridLookUpEdit;
                //    //editor.EditValue = data != null ? data.MACHINE_ID : 0;
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void btnCallPatient_Click(object sender, EventArgs e)
        {
            try
            {
                btnCallPatient.Focus();
                if (gridControlTreatmentSampleDesk.DataSource != null && gridViewTreatmentSampleDesk.RowCount > 0)
                {
                    List<TreatmentSampleListViewADO> dataSource = (List<TreatmentSampleListViewADO>)(gridControlTreatmentSampleDesk.DataSource);
                    List<TreatmentSampleListViewADO> selectData = dataSource.Where(o => o.IsChecked).ToList();
                    if (selectData != null && selectData.Count > 0)
                    {
                        UpdateDicCallPatient(selectData, dataSource);
                        LoadCallPatientByThread(selectData);
                    }
                    else
                        MessageManager.Show("Chọn bệnh nhân");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void btnRecallPatient_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnRecallPatient.Enabled)
                    return;
                CreateThreadReCallPatient();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LayMauNhanh()
        {
            try
            {
                //frmGetSampleFaster frmGetSampleFaster = new frmGetSampleFaster(this.currentModule, this.SampleRoom, this.delegateSelectData);
                //frmGetSampleFaster.ShowDialog();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void delegateSelectData(object result)
        {
            try
            {
                if (result != null)
                {
                    this.FillDataToGridControl();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void btnCallPatientFaster_Click(object sender, EventArgs e)
        {
        }

        private void btnGetSampleFaster_Click(object sender, EventArgs e)
        {
            try
            {
                LayMauNhanh();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void gridViewSample_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "BARCODE")
                {
                    var focus = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        private void gridViewSample_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                GridHitInfo hi = e.HitInfo;
                if (hi.InRowCell)
                {
                    var row = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                    if (this.baManager == null)
                    {
                        this.baManager = new BarManager();
                        this.baManager.Form = this;
                    }
                    if (row != null)
                    {
                        if (row.IsChecked)
                        {
                            this.popupMenuProcessor = new PopupMenuProcessor(this.baManager, Sample_MouseRightClick);
                            this.popupMenuProcessor.InitMenuChecked();
                        }
                        else
                        {
                            this.popupMenuProcessor = new PopupMenuProcessor(this.baManager, Sample_MouseRightClick);
                            this.popupMenuProcessor.InitMenu();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void Sample_MouseRightClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var row = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                if ((e.Item is BarButtonItem) && row != null)
                {
                    var type = (PopupMenuProcessor.ItemType)e.Item.Tag;
                    switch (type)
                    {
                        case PopupMenuProcessor.ItemType.LichSuXetNghiem:
                            this.LichSuXetNghiemCuaBenhNha(row);
                            break;
                        case PopupMenuProcessor.ItemType.LayMau:
                            var ListRow = lstAll.Where(o => o.IsChecked).ToList();
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        private void LichSuXetNghiemCuaBenhNha(TreatmentSampleListViewADO data)
        {
            try
            {
                if (data != null)
                {
                    Inventec.Desktop.Common.Modules.Module moduleData = GlobalVariables.currentModuleRaws.Where(o => o.ModuleLink == "LIS.Desktop.Plugins.TestHistory").FirstOrDefault();
                    if (moduleData == null) throw new NullReferenceException("Not found module by ModuleLink = 'LIS.Desktop.Plugins.TestHistory'");
                    if (!moduleData.IsPlugin || moduleData.ExtensionInfo == null) throw new NullReferenceException("Module 'LIS.Desktop.Plugins.TestHistory' is not plugins");
                    List<object> listArgs = new List<object>();
                    //listArgs.Add(data.PATIENT_CODE);
                    listArgs.Add(PluginInstance.GetModuleWithWorkingRoom(moduleData, this.currentModule.RoomId, this.currentModule.RoomTypeId));
                    var extenceInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(moduleData, this.currentModule.RoomId, this.currentModule.RoomTypeId), listArgs);
                    if (extenceInstance == null) throw new ArgumentNullException("moduleData is null");
                    ((Form)extenceInstance).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void txtFindServiceReqCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnFind_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!btnPrintBarcode.Enabled) return;
                //this.onClickBtnPrintBarCode();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        private void txtFindPatientCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnFind_Click(null, null);
                    if (chkGoiSauKhiQuet.Checked)
                    {
                        LoadInformationPatient();
                        CallFocusPatient();
                    }
                    if (String.IsNullOrWhiteSpace(txtFindPatientCode.Text))
                    {
                        txtFindTreamentCode.Focus();
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void txtFindTreamentCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnFind_Click(null, null);
                    if (chkGoiSauKhiQuet.Checked)
                    {
                        LoadInformationPatient();
                        CallFocusPatient();
                    }
                    txtFindTreamentCode.Focus();
                    txtFindTreamentCode.SelectAll();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void repositoryItemBtnUpdateBarcodeTime_Enable_Click(object sender, EventArgs e)
        {
            try
            {
                var data = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                if (data != null)
                {
                    WaitingManager.Show();
                    CommonParam param = new CommonParam();
                    bool success = false;
                    var rs = new Inventec.Common.Adapter.BackendAdapter(param).Post<LIS_SAMPLE>("api/LisSample/UpdateBarcodeTime", ApiConsumers.LisConsumer, data.ID, param);
                    if (rs != null)
                    {
                        success = true;
                    }
                    WaitingManager.Hide();
                    if (success)
                    {
                        MessageManager.Show(this.ParentForm, param, success);
                    }
                    else
                    {
                        MessageManager.Show(param, success);
                    }

                    SessionManager.ProcessTokenLost(param);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void gridViewSample_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                {
                    GridView view = sender as GridView;
                    GridHitInfo hi = view.CalcHitInfo(e.Location);
                    if (hi.Column.FieldName == "IsChecked")
                    {
                        if (hi.InColumnPanel)
                        {
                            statecheckColumn = !statecheckColumn;
                            this.SetCheckAllColumn(statecheckColumn);
                            this.GridCheckChange(statecheckColumn);
                        }
                        else if (hi.InRowCell)
                        {
                            if (lstAll.Where(o => o.IsChecked).Count() == lstAll.Count)
                            {
                                statecheckColumn = true;
                                this.SetCheckAllColumn(statecheckColumn);
                            }
                            else
                            {
                                statecheckColumn = false;
                                this.SetCheckAllColumn(statecheckColumn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        private void GridCheckChange(bool checkedAll)
        {
            try
            {
                foreach (var item in this.lstAll)
                {
                    item.IsChecked = checkedAll;
                }
                this.gridViewTreatmentSampleDesk.BeginUpdate();
                this.gridViewTreatmentSampleDesk.GridControl.DataSource = this.lstAll;
                this.gridViewTreatmentSampleDesk.EndUpdate();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboTreatmentArea_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            try
            {
                e.DisplayText = "";
                string dienDieuTri = "";
                if (_DienDieuTriSelecteds != null && _DienDieuTriSelecteds.Count > 0)
                {
                    foreach (var item in _DienDieuTriSelecteds)
                    {
                        dienDieuTri += item.TREATMENT_TYPE_NAME + ", ";
                    }

                    dienDieuTri = dienDieuTri.TrimEnd(',', ' ');

                    dienDieuTri = dienDieuTri.TrimStart(',', ' ');
                }

                e.DisplayText = dienDieuTri;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void updateOLayMau()
        {
            try
            {
                Inventec.Common.WebApiClient.ApiConsumer mosUserConsummer = new Inventec.Common.WebApiClient.ApiConsumer(HisConfigCFG.MOS_USER_URI, GlobalVariables.APPLICATION_CODE);
                mosUserConsummer.SetTokenCode(HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer.GetTokenCode());
                gridViewSampleDeskCounter.UpdateCurrentRow();
                gridViewTreatmentSampleDesk.UpdateCurrentRow();
                TreatmentSampleListViewADO treatmentSampleDesk = (TreatmentSampleListViewADO)(gridViewTreatmentSampleDesk.GetFocusedRow());
                CommonParam param = new CommonParam();
                bool success = false;
                List<SampleDeskCounterADO> DeskCounters = (List<SampleDeskCounterADO>)gridControlSampleDeskCounter.DataSource;
                if (treatmentSampleDesk != null && DeskCounters != null && DeskCounters.Count > 0)
                {
                    var focus = DeskCounters.FirstOrDefault(o => o.IsChecked == true);
                    if (focus != null)
                    {
                        listUpdateSampleDesk = new List<HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK>();
                        treatmentSampleDesk.SAMPLE_DESK_ID = focus.ID;
                        treatmentSampleDesk.SAMPLE_DESK_NAME = focus.SAMPLE_DESK_NAME;
                        treatmentSampleDesk.NUM_ORDER = Convert.ToInt64(focus.CURRENT_NUM ?? 0) + 1;
                        listUpdateSampleDesk.Add(treatmentSampleDesk);

                        var rs = HIS.Desktop.LocalStorage.BackendData.V2.CallPatient.CallPtDataWorker.UpdateSampleDesk(listUpdateSampleDesk, currentModule.RoomId, mosUserConsummer, param);

                        if (rs)
                        {
                            this.treatmentSampleDeskPrint = treatmentSampleDesk;
                            success = true;
                            gridViewTreatmentSampleDesk.BeginDataUpdate();
                            gridViewTreatmentSampleDesk.EndDataUpdate();
                            gridViewSampleDeskCounter.BeginDataUpdate();
                            gridViewSampleDeskCounter.EndDataUpdate();
                            FillDataToGridControl();
                            txtFindTreamentCode.Focus();
                            txtFindTreamentCode.SelectAll();
                            LoadInformationPatient();
                        }
                        else
                        {
                            this.treatmentSampleDeskPrint = null;
                        }
                    }
                }
                MessageManager.Show(this.ParentForm, param, success);
                SessionManager.ProcessTokenLost(param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            updateOLayMau();
        }
        public void ShortcutSave()
        {
            btnSave_Click(null, null);
        }

        public void ShotcurtPrint()
        {
            btnPrint_Click(null, null);
        }

        private void repositoryItemChkChon_Click(object sender, EventArgs e)
        {
            try
            {
                var row = (SampleDeskCounterADO)gridViewSampleDeskCounter.GetFocusedRow();
                if (row.IsChecked == true)
                {
                    row.IsChecked = false;
                    return;
                }
                foreach (var item in lstAllCounter)
                {
                    item.IsChecked = false;
                    if (item.ID == row.ID)
                    {
                        item.IsChecked = true;
                    }
                }
                gridControlSampleDeskCounter.DataSource = lstAllCounter;
                gridControlSampleDeskCounter.RefreshDataSource();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private bool deletePrintTemplate(string printTypeCode, string fileName)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(printTypeCode) && !String.IsNullOrEmpty(fileName))
                {
                    switch (printTypeCode)
                    {

                        case "Mps100494":
                            Mps100494(printTypeCode, fileName);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void Mps100494(string printTypeCode, string fileName)
        {
            bool result = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("Mps100494 treatmentSampleDeskPrint " + treatmentSampleDeskPrint.TREATMENT_CODE);
                MPS.Processor.Mps100494.PDO.Mps100494PDO rdo = new MPS.Processor.Mps100494.PDO.Mps100494PDO(treatmentSampleDeskPrint);
                if (chkPrintNow.Checked)
                {
                    result = MPS.MpsPrinter.Run(new MPS.ProcessorBase.Core.PrintData(printTypeCode, fileName, rdo, MPS.ProcessorBase.PrintConfig.PreviewType.ShowDialog, ""));
                }
                else
                {
                    result = MPS.MpsPrinter.Run(new MPS.ProcessorBase.Core.PrintData(printTypeCode, fileName, rdo, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, ""));
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void printMps000494()
        {
            Inventec.Common.RichEditor.RichEditorStore store = new Inventec.Common.RichEditor.RichEditorStore(ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
            store.RunPrintTemplate("Mps100494", deletePrintTemplate);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                treatmentSampleDeskPrint = (HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.V_HIS_TREATMENT_SAMPLE_DESK)gridViewTreatmentSampleDesk.GetFocusedRow();
                printMps000494();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void chkPrintNow_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isNotLoadWhileChangeControlStateInFirst)
                {
                    return;
                }

                WaitingManager.Show();
                HIS.Desktop.Library.CacheClient.ControlStateRDO csAddOrUpdate = (SampleCollectionRoomUC.currentControlStateRDO != null && SampleCollectionRoomUC.currentControlStateRDO.Count > 0) ? SampleCollectionRoomUC.currentControlStateRDO.Where(o => o.KEY == ControlStateConstant.CHECK_PRINT_NOW && o.MODULE_LINK == ControlStateConstant.MODULE_LINK).FirstOrDefault() : null;
                //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
                if (csAddOrUpdate != null)
                {
                    csAddOrUpdate.VALUE = (chkPrintNow.Checked ? "1" : "");
                }
                else
                {
                    csAddOrUpdate = new HIS.Desktop.Library.CacheClient.ControlStateRDO();
                    csAddOrUpdate.KEY = ControlStateConstant.CHECK_PRINT_NOW;
                    csAddOrUpdate.VALUE = (chkPrintNow.Checked ? "1" : "");
                    csAddOrUpdate.MODULE_LINK = ControlStateConstant.MODULE_LINK;
                    if (SampleCollectionRoomUC.currentControlStateRDO == null)
                        SampleCollectionRoomUC.currentControlStateRDO = new List<HIS.Desktop.Library.CacheClient.ControlStateRDO>();
                    SampleCollectionRoomUC.currentControlStateRDO.Add(csAddOrUpdate);
                }
                SampleCollectionRoomUC.controlStateWorker.SetData(SampleCollectionRoomUC.currentControlStateRDO);
                WaitingManager.Hide();
                //if (this._RefreshCheckPrint != null)
                //{
                //    this._RefreshCheckPrint();
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void chkDaPhanO_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isNotLoadWhileChangeControlStateInFirst)
                {
                    return;
                }

                WaitingManager.Show();
                HIS.Desktop.Library.CacheClient.ControlStateRDO csAddOrUpdate = (SampleCollectionRoomUC.currentControlStateRDO != null && SampleCollectionRoomUC.currentControlStateRDO.Count > 0) ? SampleCollectionRoomUC.currentControlStateRDO.Where(o => o.KEY == ControlStateConstant.CHECK_DA_PHAN_O && o.MODULE_LINK == ControlStateConstant.MODULE_LINK).FirstOrDefault() : null;
                //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
                if (csAddOrUpdate != null)
                {
                    csAddOrUpdate.VALUE = (chkDaPhanO.Checked ? "1" : "");
                }
                else
                {
                    csAddOrUpdate = new HIS.Desktop.Library.CacheClient.ControlStateRDO();
                    csAddOrUpdate.KEY = ControlStateConstant.CHECK_DA_PHAN_O;
                    csAddOrUpdate.VALUE = (chkDaPhanO.Checked ? "1" : "");
                    csAddOrUpdate.MODULE_LINK = ControlStateConstant.MODULE_LINK;
                    if (SampleCollectionRoomUC.currentControlStateRDO == null)
                        SampleCollectionRoomUC.currentControlStateRDO = new List<HIS.Desktop.Library.CacheClient.ControlStateRDO>();
                    SampleCollectionRoomUC.currentControlStateRDO.Add(csAddOrUpdate);
                }
                SampleCollectionRoomUC.controlStateWorker.SetData(SampleCollectionRoomUC.currentControlStateRDO);
                WaitingManager.Hide();
                //if (this._RefreshCheckPrint != null)
                //{
                //    this._RefreshCheckPrint();
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void chkChuaPhanO_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isNotLoadWhileChangeControlStateInFirst)
                {
                    return;
                }

                WaitingManager.Show();
                HIS.Desktop.Library.CacheClient.ControlStateRDO csAddOrUpdate = (SampleCollectionRoomUC.currentControlStateRDO != null && SampleCollectionRoomUC.currentControlStateRDO.Count > 0) ? SampleCollectionRoomUC.currentControlStateRDO.Where(o => o.KEY == ControlStateConstant.CHECK_CHUA_PHAN_O && o.MODULE_LINK == ControlStateConstant.MODULE_LINK).FirstOrDefault() : null;
                //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
                if (csAddOrUpdate != null)
                {
                    csAddOrUpdate.VALUE = (chkChuaPhanO.Checked ? "1" : "");
                }
                else
                {
                    csAddOrUpdate = new HIS.Desktop.Library.CacheClient.ControlStateRDO();
                    csAddOrUpdate.KEY = ControlStateConstant.CHECK_CHUA_PHAN_O;
                    csAddOrUpdate.VALUE = (chkChuaPhanO.Checked ? "1" : "");
                    csAddOrUpdate.MODULE_LINK = ControlStateConstant.MODULE_LINK;
                    if (SampleCollectionRoomUC.currentControlStateRDO == null)
                        SampleCollectionRoomUC.currentControlStateRDO = new List<HIS.Desktop.Library.CacheClient.ControlStateRDO>();
                    SampleCollectionRoomUC.currentControlStateRDO.Add(csAddOrUpdate);
                }
                SampleCollectionRoomUC.controlStateWorker.SetData(SampleCollectionRoomUC.currentControlStateRDO);
                WaitingManager.Hide();
                //if (this._RefreshCheckPrint != null)
                //{
                //    this._RefreshCheckPrint();
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void chkBaoHiemTinhTien_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isNotLoadWhileChangeControlStateInFirst)
                {
                    return;
                }

                WaitingManager.Show();
                HIS.Desktop.Library.CacheClient.ControlStateRDO csAddOrUpdate = (SampleCollectionRoomUC.currentControlStateRDO != null && SampleCollectionRoomUC.currentControlStateRDO.Count > 0) ? SampleCollectionRoomUC.currentControlStateRDO.Where(o => o.KEY == ControlStateConstant.CHECK_CO_BHYT && o.MODULE_LINK == ControlStateConstant.MODULE_LINK).FirstOrDefault() : null;
                //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
                if (csAddOrUpdate != null)
                {
                    csAddOrUpdate.VALUE = (chkBaoHiemTinhTien.Checked ? "1" : "");
                }
                else
                {
                    csAddOrUpdate = new HIS.Desktop.Library.CacheClient.ControlStateRDO();
                    csAddOrUpdate.KEY = ControlStateConstant.CHECK_CO_BHYT;
                    csAddOrUpdate.VALUE = (chkBaoHiemTinhTien.Checked ? "1" : "");
                    csAddOrUpdate.MODULE_LINK = ControlStateConstant.MODULE_LINK;
                    if (SampleCollectionRoomUC.currentControlStateRDO == null)
                        SampleCollectionRoomUC.currentControlStateRDO = new List<HIS.Desktop.Library.CacheClient.ControlStateRDO>();
                    SampleCollectionRoomUC.currentControlStateRDO.Add(csAddOrUpdate);
                }
                SampleCollectionRoomUC.controlStateWorker.SetData(SampleCollectionRoomUC.currentControlStateRDO);
                WaitingManager.Hide();
                //if (this._RefreshCheckPrint != null)
                //{
                //    this._RefreshCheckPrint();
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void ShortCutSaveAndPrint()
        {
            btnSaveAndPrint_Click(null, null);
        }

        private void btnSaveAndPrint_Click(object sender, EventArgs e)
        {
            updateOLayMau();
            printMps000494();
        }

        private void btnRecall_Click(object sender, EventArgs e)
        {
            try
            {
                var currentHisServiceReq = (TreatmentSampleListViewADO)gridViewTreatmentSampleDesk.GetFocusedRow();
                LoadCallPatientByThread(new List<TreatmentSampleListViewADO> { currentHisServiceReq });
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void ShortCutRecall()
        {
            btnRecall_Click(null, null);
        }

        private void treeListService_NodeCellStyle(object sender, DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs e)
        {
            try
            {
                if (e.Node.HasChildren)
                {
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void chkGoiSauKhiQuet_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isNotLoadWhileChangeControlStateInFirst)
                {
                    return;
                }

                WaitingManager.Show();
                HIS.Desktop.Library.CacheClient.ControlStateRDO csAddOrUpdate = (SampleCollectionRoomUC.currentControlStateRDO != null && SampleCollectionRoomUC.currentControlStateRDO.Count > 0) ? SampleCollectionRoomUC.currentControlStateRDO.Where(o => o.KEY == ControlStateConstant.CHECK_GOI_SAU_KHI_QUET && o.MODULE_LINK == ControlStateConstant.MODULE_LINK).FirstOrDefault() : null;
                //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
                if (csAddOrUpdate != null)
                {
                    csAddOrUpdate.VALUE = (chkGoiSauKhiQuet.Checked ? "1" : "");
                }
                else
                {
                    csAddOrUpdate = new HIS.Desktop.Library.CacheClient.ControlStateRDO();
                    csAddOrUpdate.KEY = ControlStateConstant.CHECK_GOI_SAU_KHI_QUET;
                    csAddOrUpdate.VALUE = (chkGoiSauKhiQuet.Checked ? "1" : "");
                    csAddOrUpdate.MODULE_LINK = ControlStateConstant.MODULE_LINK;
                    if (SampleCollectionRoomUC.currentControlStateRDO == null)
                        SampleCollectionRoomUC.currentControlStateRDO = new List<HIS.Desktop.Library.CacheClient.ControlStateRDO>();
                    SampleCollectionRoomUC.currentControlStateRDO.Add(csAddOrUpdate);
                }
                SampleCollectionRoomUC.controlStateWorker.SetData(SampleCollectionRoomUC.currentControlStateRDO);
                WaitingManager.Hide();
                //if (this._RefreshCheckPrint != null)
                //{
                //    this._RefreshCheckPrint();
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void chkScreenSaver_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isNotLoadWhileChangeControlStateInFirst)
                {
                    return;
                }
                WaitingManager.Show();
                HIS.Desktop.Library.CacheClient.ControlStateRDO csAddOrUpdate = (SampleCollectionRoomUC.currentControlStateRDO != null && SampleCollectionRoomUC.currentControlStateRDO.Count > 0) ? SampleCollectionRoomUC.currentControlStateRDO.Where(o => o.KEY == chkScreenSaver.Name && o.MODULE_LINK == ControlStateConstant.MODULE_LINK).FirstOrDefault() : null;
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
                if (csAddOrUpdate != null)
                {
                    csAddOrUpdate.VALUE = (chkScreenSaver.Checked ? "1" : "");
                }
                else
                {
                    csAddOrUpdate = new HIS.Desktop.Library.CacheClient.ControlStateRDO();
                    csAddOrUpdate.KEY = chkScreenSaver.Name;
                    csAddOrUpdate.VALUE = (chkScreenSaver.Checked ? "1" : "");
                    csAddOrUpdate.MODULE_LINK = ControlStateConstant.MODULE_LINK;
                    if (SampleCollectionRoomUC.currentControlStateRDO == null)
                        SampleCollectionRoomUC.currentControlStateRDO = new List<HIS.Desktop.Library.CacheClient.ControlStateRDO>();
                    SampleCollectionRoomUC.currentControlStateRDO.Add(csAddOrUpdate);
                }
                SampleCollectionRoomUC.controlStateWorker.SetData(SampleCollectionRoomUC.currentControlStateRDO);
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
