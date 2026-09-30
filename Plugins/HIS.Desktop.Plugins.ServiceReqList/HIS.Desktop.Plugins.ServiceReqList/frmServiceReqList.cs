using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using Bartender.PrintBloodServiceReq;
using Bartender.PrintClient;
using Bartender.PrintGpblServiceReq;
using Bartender.PrintTestServiceReq;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using EMR.EFMODEL.DataModels;
using EMR.Filter;
using EMR.SDO;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.IsAdmin;
using HIS.Desktop.Library.CacheClient;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.ConfigSystem;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.ModuleExt;
using HIS.Desktop.Plugins.Library.AlertHospitalFeeNotBHYT;
using HIS.Desktop.Plugins.Library.EmrGenerate;
using HIS.Desktop.Plugins.Library.PrintPrescription;
using HIS.Desktop.Plugins.Library.PrintServiceReq;
using HIS.Desktop.Plugins.Library.PrintServiceReqTreatment;
using HIS.Desktop.Plugins.ServiceReqList.ADO;
using HIS.Desktop.Plugins.ServiceReqList.Base;
using HIS.Desktop.Plugins.ServiceReqList.Reason;
using HIS.Desktop.Plugins.ServiceReqList.Resources;
using HIS.Desktop.Print;
using HIS.Desktop.Utilities;
using HIS.Desktop.Utilities.Extensions;
using HIS.Desktop.Utility;
using His.EventLog;
using Inventec.Common.Adapter;
using Inventec.Common.BarcodeLib;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.DateTime;
using Inventec.Common.DocumentViewer.Template;
using Inventec.Common.FlexCelPrint;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.Resource;
using Inventec.Common.RichEditor;
using Inventec.Common.RichEditor.Base;
using Inventec.Common.SignLibrary;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.String;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Common.Modules;
using Inventec.Fss.Client;
using Inventec.UC.EventLogControl.Data;
using Inventec.UC.Login.Base;
using Inventec.UC.Paging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein.Bhyt;
using MOS.SDO;
using MPS;
using MPS.ADO;
using MPS.Processor.Mps000010.PDO;
using MPS.Processor.Mps000014.PDO;
using MPS.Processor.Mps000033.PDO;
using MPS.Processor.Mps000035.PDO;
using MPS.Processor.Mps000063.PDO;
using MPS.Processor.Mps000097.PDO;
using MPS.Processor.Mps000108.PDO;
using MPS.Processor.Mps000178.PDO;
using MPS.Processor.Mps000204.PDO;
using MPS.Processor.Mps000275.PDO;
using MPS.Processor.Mps000420.PDO;
using MPS.Processor.Mps000433.PDO;
using MPS.Processor.Mps000494.PDO;
using MPS.Processor.Mps190001.PDO;
using MPS.ProcessorBase;
using MPS.ProcessorBase.Core;
using SAR.EFMODEL.DataModels;
using SAR.Filter;
using ThermalPrinter.PrintTestServiceReq;

namespace HIS.Desktop.Plugins.ServiceReqList
{
    public class frmServiceReqList : FormBase
    {
        private const string moduleLink = "HIS.Desktop.Plugins.ServiceReqList";

        private const string MPS000167 = "Mps000167";

        private const string MPS000033 = "Mps000033";

        private const string MPS000035 = "Mps000035";

        private const string MPS000037 = "Mps000037";

        private const string MPS000097 = "Mps000097";

        private const string MPS000063 = "Mps000063";

        private const string MPS000234 = "Mps000234";

        private const string MPS000204 = "Mps000204";

        private const string MPS000433 = "Mps000433";

        private const string Mps190001 = "Mps190001";

        private const string PRINT_TYPE_CODE__PHIEU_THU_KIEM_YC_KHAM__MPS000420 = "Mps000420";

        private const int Thuoc = 1;

        private const int VatTu = 2;

        private const int ThuocNgoaiKho = 3;

        private const int VatTuNgoaiKho = 4;

        private const int TuTuc = 5;

        private string loginName = null;

        private int rowCount = 0;

        private int dataTotal = 0;

        private int start = 0;

        private int lastRowHandle = -1;

        private ToolTipControlInfo lastInfo = null;

        private GridColumn lastColumn = null;

        private V_HIS_ROOM currentRoom;

        private List<HIS_SERVICE_REQ_TYPE> serviceReqTypeSelecteds;

        private List<HIS_SERVICE_REQ_STT> serviceReqSttSelecteds;

        private HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO currentServiceReqPrint;

        private HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO currentServiceReq;

        private V_HIS_SERVICE_REQ serviceReqPrintRaw;

        private V_HIS_PATIENT currentPatient = null;

        private HIS_TREATMENT treatment = null;

        private HIS_EXP_MEST prescriptionPrint;

        private HIS_EXP_MEST currentPrescription;

        private Inventec.Desktop.Common.Modules.Module currentModule;

        private WorkPlaceSDO currentWorkPlace;

        private PrintPopupMenuProcessor PrintPopupMenuProcessor;

        private List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> listServiceReq;

        private List<ListMedicineADO> _listMedicine;

        private bool isCheckAll = true;

        private List<HIS_RATION_TIME> lsRationTime;

        private HIS_SERE_SERV sereServPrint;

        private ListMedicineADO rightClickData;

        private string treatmentCode = "";

        private bool isNotLoadWhileChangeControlStateInFirst;

        private ControlStateWorker controlStateWorker;

        private List<ControlStateRDO> currentControlStateRDO;

        private HisTreatmentWithPatientTypeInfoSDO TreatmentWithPatientTypeAlter;

        private Dictionary<string, object> dicParam;

        private Dictionary<string, System.Drawing.Image> dicImage;

        internal List<SereServNumOder> _SereServNumOders;

        private List<V_HIS_SERE_SERV_TEIN> lstSereServTein;

        private HIS_SERE_SERV_EXT sereServExtPrint = null;

        internal V_HIS_SERE_SERV_4 sereServ;

        private List<string> keyPrint = new List<string> { "<#CONCLUDE_PRINT;>", "<#NOTE_PRINT;>", "<#DESCRIPTION_PRINT;>", "<#CURRENT_USERNAME_PRINT;>" };

        private List<long> ConfigIds = new List<long>();

        private List<ConfigADO> lstConfig;

        private IContainer components = null;

        private LayoutControl layoutControl1;

        private LayoutControlGroup layoutControlGroup1;

        private SimpleButton btnFind;

        private TextEdit txtKeyword;

        private GridControl gridControlServiceReq;

        private GridView gridViewServiceReq;

        private LayoutControlItem layoutControlItem1;

        private UcPaging ucPaging1;

        private LayoutControlItem lciServiceReqPaging;

        private GridColumn gridColumn_Transaction_Stt;

        private GridColumn gridColumn_ServiceReq_Edit;

        private GridColumn gridColumn_Transaction_TransactionCode;

        private GridColumn gridColumn_Transaction_Amount;

        private GridColumn gridColumn_Transaction_PayFormName;

        private GridColumn gridColumn_Request_Username;

        private GridColumn gridColumn_Transaction_CashierRoomName;

        private GridColumn gridColumn_Transaction_TreatmentCode;

        private GridColumn gridColumn_Transaction_VirPatientName;

        private GridColumn gridColumn_Transaction_Dob;

        private GridColumn gridColumn_Transaction_GenderName;

        private GridColumn gridColumn_Execute_Username;

        private GridColumn gridColumn_Transaction_CreateTime;

        private GridColumn gridColumn_Transaction_Creator;

        private GridColumn gridColumn_Transaction_ModifyTime;

        private GridColumn gridColumn_Transaction_Modifier;

        private RepositoryItemButtonEdit repositoryItemBtnDeleteServiceReq;

        private RepositoryItemButtonEdit repositoryItemBtnDeleteServiceReqDisable;

        private BarManager barManager1;

        private Bar bar1;

        private BarButtonItem bbtnRCFind;

        private BarDockControl barDockControlTop;

        private BarDockControl barDockControlBottom;

        private BarDockControl barDockControlLeft;

        private BarDockControl barDockControlRight;

        private BarButtonItem bbtnRCRefresh;

        private GridColumn gridColumn_ServiceReq_Delete;

        private RepositoryItemButtonEdit repositoryItemBtnEditServiceReq;

        private RepositoryItemButtonEdit repositoryItemBtnPrintServiceReq;

        private RepositoryItemButtonEdit repositoryItemBtnEditServiceReqDisable;

        private RepositoryItemButtonEdit repositoryItemBtnPrintServiceReqDisable;

        private LayoutControlItem lciKeyword;

        private LayoutControlItem layoutControlItem3;

        private TextEdit txtServiceReqCode;

        private LayoutControlItem lciServiceReqCode;

        private GridControl grdSereServServiceReq;

        private GridView grdViewSereServServiceReq;

        private GridColumn gridColSerSevSTT;

        private GridColumn gridColSerSevView;

        private RepositoryItemButtonEdit repositoryItemButtonView;

        private GridColumn gridColSerSevPrint;

        private RepositoryItemButtonEdit repositoryItemButtonPrint;

        private GridColumn gcolServiceTypeName;

        private GridColumn gridColumn1;

        private GridColumn gridColSerSevCode;

        private GridColumn gridColSerSevName;

        private GridColumn gridColSerSevUnitName;

        private GridColumn gridColSerSevAmount;

        private GridColumn gridColSerSevTypeName;

        private GridColumn gridColOtherPrintForm;

        private RepositoryItemButtonEdit repositoryItemButtonEdit3;

        private LayoutControlItem layoutControlItem5;

        internal GridLookUpEdit cboServiceReqType;

        private GridView gridView1;

        internal GridLookUpEdit cboServiceReqStt;

        private GridView gridLookUpEdit1View;

        private LayoutControlItem layoutControlItem7;

        private LayoutControlItem layoutControlItem8;

        private GridColumn gridColumn_ServiceReq_Print;

        private GridColumn gridColumn_ServiceReq_Stt;

        private RepositoryItemPictureEdit repositoryItempicServiceReqStatus;

        private ImageList imageListPriority;

        private ImageList imageListIcon;

        private ToolTipController tooltipServiceRequest;

        private RepositoryItemTextEdit repositoryItemReadOnly;

        private DateEdit dtIntructionTimeTo;

        private DateEdit dtIntructionTimeFrom;

        private LayoutControlItem lciIntructionTimeFrom;

        private LayoutControlItem lciIntructionTimeTo;

        private LookUpEdit cboFilter;

        private LayoutControlItem layoutControlItem4;

        private LayoutControl layoutControl2;

        private LayoutControlGroup Root;

        private LabelControl lblPatientName;

        private LabelControl lblTreatmentCode;

        private LayoutControlItem lciPatientName;

        private LayoutControlItem lciTreatmentCode;

        private LabelControl lblGender;

        private LayoutControlItem lciGender;

        private SimpleButton btnAggrExpMest;

        private LabelControl lblExpMestRoom;

        private LabelControl lblAggrExpMestCode;

        private LabelControl lblExpMestCode;

        private LayoutControlItem lciExpMestCode;

        private LayoutControlItem lciAggrExpMestCode;

        private LayoutControlItem lciExpMestRoom;

        private LayoutControlItem lciBtnAggrExpMest;

        private GroupControl groupControlInfo;

        private LayoutControlItem layoutControlItem6;

        private SimpleButton btnMobaCreate;

        private LayoutControlItem lciBtnMobaCreate;

        private LabelControl lblExpMestStt;

        private LayoutControlItem lciExpMestStt;

        private GridColumn gridColumn_BieuMauKhac;

        private RepositoryItemButtonEdit repositoryItemBtnBieuMauKhac;

        private GridColumn GridColumnInReqExeute;

        private GridColumn gridColumn_ServiceReq_EditIntructionTime;

        private RepositoryItemButtonEdit repositoryItemButtonEditIntructionTime;

        private TextEdit txtTreatmentCode;

        private LayoutControlItem layoutControlItem2;

        private LabelControl lblReqDepartment;

        private LayoutControlItem lciReqDepartment;

        private SimpleButton btnPrintTotal;

        private LayoutControlItem layoutControlItem9;

        private RepositoryItemTextEdit repositoryItemTextEditDisable;

        private GridColumn gridColumn_ServiceReq_Choose;

        private RepositoryItemCheckEdit repositoryItemCheckEditChoose;

        private ImageList imageListCheck;

        private GridColumn gridColumn2;

        private RepositoryItemButtonEdit Btn_EvenLog;

        private GridColumn gridColumn3;

        private RepositoryItemCheckEdit repositoryItemCheckEditMainExam;

        private SimpleButton btnPrintMedicine;

        private LayoutControlItem layoutControlItem10;

        private ImageCollection imageCollection2;

        private GridColumn gridColumn4;

        private LabelControl lblSoThang;

        private LayoutControlItem layoutControlItem11;

        private EmptySpaceItem emptySpaceItem2;

        private EmptySpaceItem emptySpaceItem1;

        private LabelControl lblRationTime;

        private LayoutControlItem lciRationTime;

        private DropDownButton btnDropDownPrint;

        private LayoutControlItem layoutControlItem13;

        private GridColumn gridColumn5;

        private RepositoryItemButtonEdit repositoryItemButton__BieuMauKhac;

        private GridColumn Gc_HisSendOldSystem;

        private GridColumn gridColSerSevConvertRatio;

        private GridColumn gridColSerSevConvertName;

        private GridColumn gridColSerSevConvertAmount;

        private GridColumn gridColumn6;

        private LabelControl lblSoTheTM;

        private LabelControl lblSoTT;

        private LayoutControlItem lciNumOrder;

        private LayoutControlItem lciSoTheTM;

        private SimpleButton btnPrintTemBarcode;

        private LayoutControlItem layoutControlItem12;

        private BarButtonItem barButtonPrintTemBarcode;

        private GridColumn grdColRationTime;

        private GridColumn gridColumn_Transaction_PatientCode;

        private TextEdit txtPatientCode;

        private LayoutControlItem layoutControlItem14;

        private CheckEdit chkPK;

        private GridLookUpEdit cboExecuteRoom;

        private GridView gridView2;

        private LayoutControlItem layoutControlItem15;

        private LayoutControlItem layoutControlItem17;

        private CheckEdit chkReqSended;

        private CheckEdit chkIsHomePres;

        private CheckEdit chkIsKidney;

        private LabelControl lbDOB;

        private LabelControl lbExcuteDepartment;

        private LayoutControlItem lciExcuteDepartment;

        private LayoutControlItem layoutControlItem18;

        private LayoutControlItem lciIsKidney;

        private LayoutControlItem lciIsHomePres;

        private LayoutControlItem lciReqSended;

        private LabelControl lblAssignTurnCode;

        private LabelControl lblBarcode;

        private LayoutControlItem lciBarcode;

        private LayoutControlItem lciAssignTurnCode;

        private GridColumn gridColumn7;

        private RepositoryItemButtonEdit repositoryItemButtonIsAcceptNoExecute;

        private GridColumn gridColumn_AllowNotExecute;

        private RepositoryItemButtonEdit repositoryItemButtonEditAllowNotExecute_Enable;

        private RepositoryItemButtonEdit repositoryItemButtonEditAllowNotExecute_Disable;

        private TextEdit txtStoreCode;

        private LayoutControlItem layoutControlItem16;

        private GridColumn gridColumn8;

        private RepositoryItemButtonEdit repositoryItemButtonEditDeleteEna;

        private RepositoryItemButtonEdit repositoryItemButtonEditDeleteDis;

        private EmptySpaceItem emptySpaceItem3;

        private LabelControl lblRationSumCode;

        private LayoutControlItem lciRationSumCode;

        private GridColumn gridColSerSevPatientTypeName;

        private ToolTipController toolTipController1;

        private RepositoryItemTextEdit repositoryItemTextEdit;

        private GridColumn gridColSerSevPresAmount;

        private SimpleButton btnConfig;

        private LayoutControlItem s;

        private PopupControlContainer popupControlContainer1;

        private LayoutControl layoutControl3;

        private LayoutControlGroup layoutControlGroup2;

        private GridControl gridConfig;

        private GridView gvConfig;

        private LayoutControlItem layoutControlItem19;

        private GridColumn gridColumn9;

        private GridColumn gridColumn10;

        private RepositoryItemCheckEdit repCheckConfig;

        private GridColumn gridColumn11;

        private RepositoryItemButtonEdit repositoryItemButtonEditServiceConfirmEna;

        private RepositoryItemButtonEdit repositoryItemButtonEditServiceConfirmDis;

        private GridColumn gridColumn_IsConfirmNoExcute;

        private RepositoryItemButtonEdit repositoryItemButtonEditIsConfirm_Ena;

        private RepositoryItemButtonEdit repositoryItemButtonEditIsConfirm_Dis;

        private LabelControl lblReceiveSampleName;

        private LabelControl lblSamplerName;

        private LayoutControlItem lciSamplerName;

        private LayoutControlItem lciReceiveSampleName;

        private LabelControl lblTestSampleTypeName;

        private LayoutControlItem lciTestSampleTypeName;

        private GridColumn gridColumn_Pttt_Group_Name;

        private Dictionary<string, object> dicParamPlus = new Dictionary<string, object>();

        private Dictionary<string, Inventec.Common.BarcodeLib.Barcode> dicImageBarcodePlus = new Dictionary<string, Inventec.Common.BarcodeLib.Barcode>();

        private Dictionary<string, System.Drawing.Image> dicImagePlus = new Dictionary<string, System.Drawing.Image>();

        private CommonParam param = new CommonParam();

        internal long printChangeServiceId = 0L;

        private int SetDefaultDepositPrice = Parse.ToInt32(HisConfigs.Get<string>("HIS_RS.HIS_DEPOSIT.DEFAULT_PRICE_FOR_BHYT_OUT_PATIENT"));

        private long patientTypeId_Bhyt = Parse.ToInt32(HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT"));

        private List<ListMedicineADO> lstSereServSelected = new List<ListMedicineADO>();

        public override void ProcessDisposeModuleDataAfterClose()
        {
            try
            {
                loginName = null;
                lastInfo = null;
                lastColumn = null;
                currentRoom = null;
                serviceReqTypeSelecteds = null;
                serviceReqSttSelecteds = null;
                currentServiceReqPrint = null;
                currentServiceReq = null;
                serviceReqPrintRaw = null;
                currentPatient = null;
                treatment = null;
                prescriptionPrint = null;
                currentPrescription = null;
                currentModule = null;
                currentWorkPlace = null;
                PrintPopupMenuProcessor = null;
                listServiceReq = null;
                _listMedicine = null;
                lsRationTime = null;
                sereServPrint = null;
                rightClickData = null;
                treatmentCode = null;
                controlStateWorker = null;
                currentControlStateRDO = null;
                TreatmentWithPatientTypeAlter = null;
                dicParam = null;
                dicImage = null;
                _SereServNumOders = null;
                lstSereServTein = null;
                sereServExtPrint = null;
                sereServ = null;
                keyPrint = null;
                ConfigIds = null;
                lstConfig = null;
                popupControlContainer1.CloseUp -= new EventHandler(popupControlContainer1_CloseUp);
                repCheckConfig.CheckedChanged -= new EventHandler(repCheckConfig_CheckedChanged);
                bbtnRCFind.ItemClick -= new ItemClickEventHandler(bbtnRCFind_ItemClick);
                barButtonPrintTemBarcode.ItemClick -= new ItemClickEventHandler(barButtonPrintTemBarcode_ItemClick);
                btnConfig.Click -= new EventHandler(btnConfig_Click);
                txtStoreCode.KeyDown -= new KeyEventHandler(txtStoreCode_KeyDown);
                chkPK.CheckedChanged -= new EventHandler(chkPK_CheckedChanged);
                cboExecuteRoom.Closed -= new ClosedEventHandler(cboExecuteRoom_Closed);
                cboExecuteRoom.ButtonClick -= new ButtonPressedEventHandler(cboExecuteRoom_ButtonClick);
                txtPatientCode.KeyDown -= new KeyEventHandler(txtPatientCode_KeyDown);
                btnPrintTemBarcode.Click -= new EventHandler(btnPrintTemBarcode_Click);
                btnDropDownPrint.Click -= new EventHandler(btnDropDownPrint_Click);
                btnPrintMedicine.Click -= new EventHandler(btnPrintMedicine_Click);
                btnPrintTotal.Click -= new EventHandler(btnPrintTotal_Click);
                txtTreatmentCode.KeyDown -= new KeyEventHandler(txtTreatmentCode_KeyDown);
                btnMobaCreate.Click -= new EventHandler(btnMobaCreate_Click);
                btnAggrExpMest.Click -= new EventHandler(btnAggrExpMest_Click);
                cboServiceReqType.Closed -= new ClosedEventHandler(cboServiceReqType_Closed);
                cboServiceReqType.CustomDisplayText -= new CustomDisplayTextEventHandler(cboServiceReqType_CustomDisplayText);
                gridView1.KeyUp -= new KeyEventHandler(gridView1_KeyUp);
                cboServiceReqStt.Closed -= new ClosedEventHandler(cboServiceReqStt_Closed);
                cboServiceReqStt.CustomDisplayText -= new CustomDisplayTextEventHandler(cboServiceReqStt_CustomDisplayText);
                gridLookUpEdit1View.KeyUp -= new KeyEventHandler(gridLookUpEdit1View_KeyUp);
                grdSereServServiceReq.DataSourceChanged -= new EventHandler(grdSereServServiceReq_DataSourceChanged);
                grdViewSereServServiceReq.CustomDrawGroupRow -= new RowObjectCustomDrawEventHandler(grdViewSereServServiceReq_CustomDrawGroupRow);
                grdViewSereServServiceReq.RowCellStyle -= new RowCellStyleEventHandler(grdViewSereServServiceReq_RowCellStyle);
                grdViewSereServServiceReq.CustomRowCellEdit -= new CustomRowCellEditEventHandler(grdViewSereServServiceReq_CustomRowCellEdit);
                grdViewSereServServiceReq.PopupMenuShowing -= new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(grdViewSereServServiceReq_PopupMenuShowing);
                grdViewSereServServiceReq.CustomUnboundColumnData -= new CustomColumnDataEventHandler(grdViewSereServServiceReq_CustomUnboundColumnData);
                repositoryItemButtonView.ButtonClick -= new ButtonPressedEventHandler(repositoryItemButtonView_ButtonClick);
                repositoryItemButtonPrint.ButtonClick -= new ButtonPressedEventHandler(repositoryItemButtonPrint_ButtonClick);
                repositoryItemButtonEditDeleteEna.ButtonClick -= new ButtonPressedEventHandler(repositoryItemButtonEditDeleteEna_ButtonClick);
                txtServiceReqCode.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtServiceReqCode_PreviewKeyDown);
                txtKeyword.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtKeyword_PreviewKeyDown);
                btnFind.Click -= new EventHandler(btnFind_Click);
                gridControlServiceReq.Click -= new EventHandler(gridControlServiceReq_Click);
                gridViewServiceReq.RowCellStyle -= new RowCellStyleEventHandler(gridViewServiceReq_RowCellStyle);
                gridViewServiceReq.CustomRowCellEdit -= new CustomRowCellEditEventHandler(gridViewServiceReq_CustomRowCellEdit);
                gridViewServiceReq.PopupMenuShowing -= new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewServiceReq_PopupMenuShowing);
                gridViewServiceReq.CustomUnboundColumnData -= new CustomColumnDataEventHandler(gridViewServiceReq_CustomUnboundColumnData);
                gridViewServiceReq.MouseDown -= new MouseEventHandler(gridViewServiceReq_MouseDown);
                repositoryItemCheckEditChoose.CheckedChanged -= new EventHandler(repositoryItemCheckEditChoose_CheckedChanged);
                repositoryItemButtonEditAllowNotExecute_Enable.ButtonClick -= new ButtonPressedEventHandler(repositoryItemButtonEditAllowNotExecute_Enable_ButtonClick);
                repositoryItemButtonEditIntructionTime.ButtonClick -= new ButtonPressedEventHandler(repositoryItemButtonEditIntructionTime_ButtonClick);
                repositoryItemButton__BieuMauKhac.ButtonClick -= new ButtonPressedEventHandler(repositoryItemButton__BieuMauKhac_ButtonClick);
                Btn_EvenLog.ButtonClick -= new ButtonPressedEventHandler(Btn_EvenLog_ButtonClick);
                repositoryItemTextEdit.Click -= new EventHandler(repositoryItemTextEdit_Click);
                repositoryItemBtnDeleteServiceReq.ButtonClick -= new ButtonPressedEventHandler(repositoryItemBtnServiceReqDelete_ButtonClick);
                repositoryItemBtnEditServiceReq.ButtonClick -= new ButtonPressedEventHandler(repositoryItemBtnServiceReqEdit_ButtonClick);
                repositoryItemBtnPrintServiceReq.ButtonClick -= new ButtonPressedEventHandler(repositoryItemBtnServiceReqPrint_ButtonClick);
                repositoryItemBtnBieuMauKhac.ButtonClick -= new ButtonPressedEventHandler(repositoryItemBtnBieuMauKhac_ButtonClick);
                tooltipServiceRequest.GetActiveObjectInfo -= new ToolTipControllerGetActiveObjectInfoEventHandler(tooltipServiceRequest_GetActiveObjectInfo);
                base.Load -= new EventHandler(frmServiceReqList_Load);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private bool CheckListServiceReqV2(List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> listServiceReq, CommonParam param)
        {
            bool result = false;
            try
            {
                param.Messages = new List<string>();
                Dictionary<long, List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>> dictionary = new Dictionary<long, List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>>();
                foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in listServiceReq)
                {
                    if (!dictionary.ContainsKey(item.TREATMENT_ID))
                    {
                        dictionary[item.TREATMENT_ID] = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                    }
                    dictionary[item.TREATMENT_ID].Add(item);
                }
                if (dictionary.Count > 1)
                {
                    param.Messages.Add(ResourceMessage.DichVuKhongCungHoSoDieuTri);
                }
                if (param.Messages.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = true;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void ExecuteBefPrint(HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO _ServiceReq)
        {
            try
            {
                currentServiceReqPrint = new HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO();
                if (_ServiceReq == null)
                {
                    return;
                }
                currentServiceReqPrint = _ServiceReq;
                serviceReqPrintRaw = GetServiceReqForPrint(_ServiceReq.ID);
                WaitingManager.Hide();
                if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 6 || currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 15 || currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 14)
                {
                    prescriptionPrint = null;
                    HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
                    hisExpMestFilter.SERVICE_REQ_ID = currentServiceReqPrint.ID;
                    List<HIS_EXP_MEST> list = new BackendAdapter(new CommonParam()).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, new Action(SessionManager.ActionLostToken), null);
                    if (list != null && list.Count > 0)
                    {
                        prescriptionPrint = list.FirstOrDefault();
                        if (prescriptionPrint.EXP_MEST_TYPE_ID != 12)
                        {
                            InDonThuocVatTu();
                        }
                    }
                    else
                    {
                        InDonThuocVatTu();
                    }
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 16)
                {
                    PrintBlood();
                }
                else
                {
                    ProcessingPrintV2();
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessingPrintV2()
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_001c: Expected O, but got Unknown
            try
            {
                RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 1)
                {
                    InPhieuYeuCauDichVu("Mps000001");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 9)
                {
                    InPhieuYeuCauDichVu("Mps000030");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 2)
                {
                    InPhieuYeuCauDichVu("Mps000026");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 8)
                {
                    InPhieuYeuCauDichVu("Mps000029");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 5)
                {
                    InPhieuYeuCauDichVu("Mps000038");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 4)
                {
                    InPhieuYeuCauDichVu("Mps000031");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 10)
                {
                    InPhieuYeuCauDichVu("Mps000036");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 3)
                {
                    InPhieuYeuCauDichVu("Mps000028");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 12)
                {
                    InPhieuYeuCauDichVu("Mps000053");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 11)
                {
                    InPhieuYeuCauDichVu("Mps000040");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 7)
                {
                    InPhieuYeuCauDichVu("Mps000042");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 13)
                {
                    InPhieuYeuCauDichVu("Mps000167");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        public frmServiceReqList(Inventec.Desktop.Common.Modules.Module module)
            : base(module)
        {
            InitializeComponent();
            try
            {
                SetIcon();
                ResourceLangManager.InitResourceLanguageManager();
                loginName = ClientTokenManagerStore.ClientTokenManager.GetLoginName();
                gridControlServiceReq.ToolTipController = tooltipServiceRequest;
                ResourceLanguageManager.LanguagefrmServiceReqList = new ResourceManager("HIS.Desktop.Plugins.ServiceReqList.Resources.Lang", typeof(frmServiceReqList).Assembly);
                currentModule = module;
                Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public frmServiceReqList(Inventec.Desktop.Common.Modules.Module module, HIS_TREATMENT data)
            : this(module)
        {
            try
            {
                treatment = data;
                currentModule = module;
                Text = module.text;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public frmServiceReqList(Inventec.Desktop.Common.Modules.Module module, V_HIS_PATIENT data)
            : this(module)
        {
            try
            {
                currentPatient = data;
                currentModule = module;
                Text = module.text;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetIcon()
        {
            try
            {
                base.Icon = Icon.ExtractAssociatedIcon(Path.Combine(ApplicationStoreLocation.ApplicationDirectory, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void frmServiceReqList_Load(object sender, EventArgs e)
        {
            try
            {
                WaitingManager.Show();
                isNotLoadWhileChangeControlStateInFirst = true;
                SetCaptionByLanguageKey();
                HisConfigCFG.LoadConfig();
                currentRoom = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == currentModule.RoomId);
                if (treatment != null)
                {
                    treatment = LoadDataToCurrentTreatmentData(treatment.ID);
                }
                LoadDataCboFilterType();
                SetPrintTypeToMps();
                LoadComboExcuteRoom();
                InitControlState();
                SetDefaultValueControl();
                FillDataToGrid();
                GeneratePopupMenu();
                InitListConfig();
                Gc_HisSendOldSystem.Visible = HisConfigCFG.IsOldSystemIntegration;
                LoadDataRationTime();
                isNotLoadWhileChangeControlStateInFirst = false;
                VisibleColumnPresAmount();
                WaitingManager.Hide();
                LogSystem.Info("end load");
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void VisibleColumnPresAmount()
        {
            try
            {
                if (!HisConfigCFG.IsShowPresAmount)
                {
                    gridColSerSevPresAmount.VisibleIndex = -1;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void InitControlState()
        {
            try
            {
                controlStateWorker = new ControlStateWorker();
                currentControlStateRDO = controlStateWorker.GetData("HIS.Desktop.Plugins.ServiceReqList");
                if (currentControlStateRDO == null || currentControlStateRDO.Count <= 0)
                {
                    return;
                }
                foreach (ControlStateRDO item in currentControlStateRDO)
                {
                    if (item.KEY == chkPK.Name)
                    {
                        chkPK.Checked = item.VALUE == "1";
                    }
                    else
                    {
                        if (!(item.KEY == gridConfig.Name) || string.IsNullOrEmpty(item.VALUE))
                        {
                            continue;
                        }
                        List<string> list = item.VALUE.Split(';').ToList();
                        foreach (string item2 in list)
                        {
                            ConfigIds.Add(long.Parse(item2));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void LoadComboExcuteRoom()
        {
            try
            {
                CommonParam commonParam = new CommonParam();
                HisExecuteRoomFilter hisExecuteRoomFilter = new HisExecuteRoomFilter();
                hisExecuteRoomFilter.IS_ACTIVE = 1;
                List<HIS_EXECUTE_ROOM> dataSource = new BackendAdapter(commonParam).Get<List<HIS_EXECUTE_ROOM>>("api/HisExecuteRoom/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExecuteRoomFilter, null).ToList();
                List<ColumnInfo> list = new List<ColumnInfo>();
                list.Add(new ColumnInfo("EXECUTE_ROOM_CODE", "", 50, 1));
                list.Add(new ColumnInfo("EXECUTE_ROOM_NAME", "", 200, 2));
                ControlEditorADO controlEditorADO = new ControlEditorADO("EXECUTE_ROOM_NAME", "ROOM_ID", list, false, 250);
                ControlEditorLoader.Load(cboExecuteRoom, dataSource, controlEditorADO);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadComboExcuteRoom(bool check)
        {
            try
            {
                if (check)
                {
                    CommonParam commonParam = new CommonParam();
                    HisExecuteRoomFilter hisExecuteRoomFilter = new HisExecuteRoomFilter();
                    hisExecuteRoomFilter.IS_ACTIVE = 1;
                    hisExecuteRoomFilter.IS_EXAM = check;
                    List<HIS_EXECUTE_ROOM> dataSource = new BackendAdapter(commonParam).Get<List<HIS_EXECUTE_ROOM>>("api/HisExecuteRoom/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExecuteRoomFilter, null).ToList();
                    List<ColumnInfo> list = new List<ColumnInfo>();
                    list.Add(new ColumnInfo("EXECUTE_ROOM_CODE", "", 50, 1));
                    list.Add(new ColumnInfo("EXECUTE_ROOM_NAME", "", 200, 2));
                    ControlEditorADO controlEditorADO = new ControlEditorADO("EXECUTE_ROOM_NAME", "ROOM_ID", list, false, 250);
                    ControlEditorLoader.Load(cboExecuteRoom, dataSource, controlEditorADO);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadDataRationTime()
        {
            try
            {
                CommonParam commonParam = new CommonParam();
                HisRationTimeFilter filter = new HisRationTimeFilter();
                lsRationTime = new BackendAdapter(commonParam).Get<List<HIS_RATION_TIME>>("api/HisRationTime/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, commonParam);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetCaptionByLanguageKey()
        {
            try
            {
                layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.layoutControl1.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                cboServiceReqType.Properties.NullText = Inventec.Common.Resource.Get.Value("frmServiceReqList.cboServiceReqType.Properties.NullText", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                cboServiceReqStt.Properties.NullText = Inventec.Common.Resource.Get.Value("frmServiceReqList.cboServiceReqStt.Properties.NullText", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevSTT.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevSTT.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevView.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevView.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevPrint.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevPrint.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gcolServiceTypeName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gcolServiceTypeName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn1.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn1.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevAmount.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevAmount.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevUnitName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevUnitName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevTypeName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevTypeName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColOtherPrintForm.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColOtherPrintForm.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                txtServiceReqCode.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("frmServiceReqList.txtServiceReqCode.Properties.NullValuePrompt", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                txtKeyword.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("frmServiceReqList.txtKeyword.Properties.NullValuePrompt", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                btnFind.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.btnFind.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_Stt.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_Stt.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_ServiceReq_Edit.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_ServiceReq_Edit.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_ServiceReq_Delete.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_ServiceReq_Delete.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_ServiceReq_Print.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_ServiceReq_Print.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_ServiceReq_Stt.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_ServiceReq_Stt.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                repositoryItempicServiceReqStatus.NullText = Inventec.Common.Resource.Get.Value("frmServiceReqList.repositoryItempicServiceReqStatus.NullText", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_TransactionCode.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_TransactionCode.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_Amount.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_Amount.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Request_Username.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_Cashier.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_CashierRoomName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_CashierRoomName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_PayFormName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_PayFormName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_Dob.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_Dob.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_TreatmentCode.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_TreatmentCode.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_VirPatientName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_VirPatientName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_GenderName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_GenderName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Execute_Username.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Execute_Username.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_CreateTime.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_CreateTime.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_Creator.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_Creator.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_ModifyTime.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_ModifyTime.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_Modifier.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_Modifier.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                grdColRationTime.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.grdColRationTime.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciServiceReqCode.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciServiceReqCode.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                layoutControlItem8.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.layoutControlItem8.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciKeyword.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciKeyword.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                layoutControlItem7.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.layoutControlItem7.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                bar1.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.bar1.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                bbtnRCFind.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.bbtnRCFind.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                bbtnRCRefresh.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.bbtnRCRefresh.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciIntructionTimeFrom.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciIntructionTimeFrom.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciIntructionTimeTo.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciIntructionTimeTo.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciAggrExpMestCode.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciAggrExpMestCode.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                btnAggrExpMest.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciBtnAggrExpMest.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciExpMestCode.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciExpMestCode.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciExpMestRoom.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciExpMestRoom.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciGender.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciGender.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciPatientName.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciPatientName.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciTreatmentCode.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciTreatmentCode.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                btnMobaCreate.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.btnMobaCreate.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                groupControlInfo.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.groupControlInfo.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciExpMestStt.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciExpMestStt.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciExcuteDepartment.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciExcuteDepartment.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciNumOrder.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciNumOrder.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciSoTheTM.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciSoTheTM.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                txtTreatmentCode.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("frmServiceReqList.txtTreatmentCode.Properties.NullValuePrompt", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                repositoryItemBtnBieuMauKhac.Buttons[0].ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.repositoryItemBtnBieuMauKhac.Buttons[0].ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                repositoryItemBtnDeleteServiceReq.Buttons[0].ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.repositoryItemBtnDeleteServiceReq.Buttons[0].ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                repositoryItemBtnEditServiceReq.Buttons[0].ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.repositoryItemBtnEditServiceReq.Buttons[0].ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                repositoryItemBtnPrintServiceReq.Buttons[0].ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.repositoryItemBtnPrintServiceReq.Buttons[0].ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                repositoryItemButtonEditIntructionTime.Buttons[0].ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.repositoryItemButtonEditIntructionTime.Buttons[0].ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                repositoryItemButtonPrint.Buttons[0].ToolTip = repositoryItemBtnPrintServiceReq.Buttons[0].ToolTip;
                repositoryItemButtonView.Buttons[0].ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.repositoryItemButtonView.Buttons[0].ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciReqDepartment.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciReqDepartment.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciRationTime.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciRationTime.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                layoutControlItem15.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.layoutControlItem15.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciReqSended.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciReqSended.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciReqSended.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciReqSended.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciExpMestRoom.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciExpMestRoom.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciIsKidney.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciIsKidney.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciIsHomePres.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciIsHomePres.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciRationSumCode.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciRationSumCode.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciRationSumCode.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciRationSumCode.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                layoutControlItem11.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.layoutControlItem11.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                btnDropDownPrint.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.btnDropDownPrint.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                btnPrintMedicine.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.btnPrintMedicine.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                btnPrintTotal.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.btnPrintTotal.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                btnPrintTemBarcode.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.btnPrintTemBarcode.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn3.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn3.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn3.ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn3.ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Transaction_PatientCode.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Transaction_PatientCode.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                txtPatientCode.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("frmServiceReqList.txtPatientCode.Properties.NullValuePrompt", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                txtStoreCode.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("frmServiceReqList.txtStoreCode.Properties.NullValuePrompt", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciAssignTurnCode.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciAssignTurnCode.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciAssignTurnCode.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciAssignTurnCode.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevCode.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevCode.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevPatientTypeName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevPatientTypeName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevPatientTypeName.ToolTip = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevPatientTypeName.ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevConvertRatio.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevConvertRatio.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevConvertAmount.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevConvertAmount.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColSerSevConvertName.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColSerSevConvertName.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn4.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn4.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn6.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn6.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                Gc_HisSendOldSystem.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.Gc_HisSendOldSystem.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                layoutControlItem18.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.layoutControlItem18.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                lciTestSampleTypeName.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciTestSampleTypeName.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                gridColumn_Pttt_Group_Name.Caption = Inventec.Common.Resource.Get.Value("frmServiceReqList.gridColumn_Pttt_Group_Name.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadDataCboFilterType()
        {
            try
            {
                List<FilterTypeADO> list = new List<FilterTypeADO>();
                list.Add(new FilterTypeADO(0L, new HIS.Desktop.Plugins.ServiceReqList.Base.GlobalStore().TOI_TAO));
                list.Add(new FilterTypeADO(1L, new HIS.Desktop.Plugins.ServiceReqList.Base.GlobalStore().PHONG_CHI_DINH));
                list.Add(new FilterTypeADO(2L, new HIS.Desktop.Plugins.ServiceReqList.Base.GlobalStore().KHOA_CHI_DINH));
                list.Add(new FilterTypeADO(3L, new HIS.Desktop.Plugins.ServiceReqList.Base.GlobalStore().KHOA_THUC_HIEN));
                list.Add(new FilterTypeADO(4L, new HIS.Desktop.Plugins.ServiceReqList.Base.GlobalStore().TAT_CA));
                cboFilter.Properties.DataSource = list;
                cboFilter.Properties.DisplayMember = "FilterTypeName";
                cboFilter.Properties.ValueMember = "ID";
                cboFilter.Properties.ForceInitialize();
                cboFilter.Properties.Columns.Clear();
                cboFilter.Properties.Columns.Add(new LookUpColumnInfo("FilterTypeName", "", 200));
                cboFilter.Properties.ShowHeader = false;
                cboFilter.Properties.ImmediatePopup = true;
                cboFilter.Properties.DropDownRows = 5;
                cboFilter.Properties.PopupWidth = 220;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetDefaultValueControl()
        {
            try
            {
                InitServiceReqTypeCheck();
                InitServiceReqSttCheck();
                InitComboServiceReqType();
                InitComboServiceReqStt();
                cboFilter.EditValue = 0L;
                txtKeyword.Text = "";
                dtIntructionTimeFrom.DateTime = DateTime.Now;
                dtIntructionTimeTo.DateTime = DateTime.Now;
                string inputValue = HisConfigs.Get<string>("HIS.Desktop.Plugins.ServiceReqList.Filter_Type_For_Treatment_Patient");
                long num = Parse.ToInt64(inputValue);
                if (treatment != null)
                {
                    txtTreatmentCode.Text = treatment.TREATMENT_CODE;
                    cboFilter.EditValue = ((num > 0 && num <= 5) ? (num - 1) : 2);
                    dtIntructionTimeFrom.EditValue = null;
                    dtIntructionTimeTo.EditValue = null;
                }
                if (currentPatient != null)
                {
                    cboFilter.EditValue = ((num > 0 && num <= 5) ? (num - 1) : 2);
                    dtIntructionTimeFrom.EditValue = null;
                }
                currentWorkPlace = WorkPlace.WorkPlaceSDO.FirstOrDefault((WorkPlaceSDO o) => o.RoomId == currentModule.RoomId && o.RoomTypeId == currentModule.RoomTypeId);
                SetVisibleControl(false);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private HIS_TREATMENT LoadDataToCurrentTreatmentData(long treatmentId)
        {
            HIS_TREATMENT result = null;
            try
            {
                CommonParam commonParam = new CommonParam();
                HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
                hisTreatmentFilter.ID = treatmentId;
                List<HIS_TREATMENT> list = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, new Action(SessionManager.ActionLostToken), commonParam);
                if (list != null && list.Count > 0)
                {
                    result = list[0];
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return result;
        }

        private void InitServiceReqTypeCheck()
        {
            try
            {
                GridCheckMarksSelection gridCheckMarksSelection = new GridCheckMarksSelection(cboServiceReqType.Properties);
                gridCheckMarksSelection.SelectionChanged += new GridCheckMarksSelection.SelectionChangedEventHandler(SelectionGrid__ServiceReqType);
                cboServiceReqType.Properties.Tag = gridCheckMarksSelection;
                cboServiceReqType.Properties.View.OptionsSelection.MultiSelect = true;
                GridCheckMarksSelection gridCheckMarksSelection2 = cboServiceReqType.Properties.Tag as GridCheckMarksSelection;
                if (gridCheckMarksSelection2 != null)
                {
                    gridCheckMarksSelection2.ClearSelection(cboServiceReqType.Properties.View);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void SelectionGrid__ServiceReqType(object sender, EventArgs e)
        {
            try
            {
                serviceReqTypeSelecteds = new List<HIS_SERVICE_REQ_TYPE>();
                foreach (HIS_SERVICE_REQ_TYPE item in (sender as GridCheckMarksSelection).Selection)
                {
                    if (item != null)
                    {
                        serviceReqTypeSelecteds.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void InitServiceReqSttCheck()
        {
            try
            {
                GridCheckMarksSelection gridCheckMarksSelection = new GridCheckMarksSelection(cboServiceReqStt.Properties);
                gridCheckMarksSelection.SelectionChanged += new GridCheckMarksSelection.SelectionChangedEventHandler(SelectionGrid__ServiceReqStt);
                cboServiceReqStt.Properties.Tag = gridCheckMarksSelection;
                cboServiceReqStt.Properties.View.OptionsSelection.MultiSelect = true;
                GridCheckMarksSelection gridCheckMarksSelection2 = cboServiceReqStt.Properties.Tag as GridCheckMarksSelection;
                if (gridCheckMarksSelection2 != null)
                {
                    gridCheckMarksSelection2.ClearSelection(cboServiceReqStt.Properties.View);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void SelectionGrid__ServiceReqStt(object sender, EventArgs e)
        {
            try
            {
                serviceReqSttSelecteds = new List<HIS_SERVICE_REQ_STT>();
                foreach (HIS_SERVICE_REQ_STT item in (sender as GridCheckMarksSelection).Selection)
                {
                    if (item != null)
                    {
                        serviceReqSttSelecteds.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void SetPrintTypeToMps()
        {
            try
            {
                if (MPS.PrintConfig.PrintTypes == null || MPS.PrintConfig.PrintTypes.Count == 0)
                {
                    MPS.PrintConfig.PrintTypes = BackendDataWorker.Get<SAR_PRINT_TYPE>();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void FillDataToGrid()
        {
            try
            {
                int num = (int)((ucPaging1.pagingGrid != null) ? ucPaging1.pagingGrid.PageSize : ConfigApplications.NumPageSize);
                FillDataToGridTransaction(new CommonParam(0, num));
                CommonParam commonParam = new CommonParam();
                commonParam.Limit = rowCount;
                commonParam.Count = dataTotal;
                ucPaging1.Init(new LoadDataDelegate(FillDataToGridTransaction), commonParam, num, gridControlServiceReq);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void FillDataToGridTransaction(object param)
        {
            try
            {
                LogSystem.Debug("FillDataToGridTransaction. 1");
                WaitingManager.Show();
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                gridControlServiceReq.DataSource = null;
                start = ((CommonParam)param).Start ?? 0;
                int value = ((CommonParam)param).Limit ?? 0;
                CommonParam commonParam = new CommonParam(start, value);
                HisServiceReqFilter filter = new HisServiceReqFilter();
                SetFilter(ref filter);
                LogSystem.Debug("FillDataToGridTransaction. 2");
                ApiResultObject<List<HIS_SERVICE_REQ>> rO = new BackendAdapter(commonParam).GetRO<List<HIS_SERVICE_REQ>>(HisConfigCFG.IsUseGetDynamic ? "api/HisServiceReq/GetDynamic" : "api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, new Action(SessionManager.ActionLostToken), commonParam);
                LogSystem.Debug("FillDataToGridTransaction. 3");
                if (rO != null)
                {
                    rowCount = ((rO.Data != null) ? rO.Data.Count : 0);
                    dataTotal = ((rO.Param != null) ? (rO.Param.Count ?? 0) : 0);
                    if (rO.Data != null && rO.Data.Count > 0)
                    {
                        foreach (HIS_SERVICE_REQ datum in rO.Data)
                        {
                            HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item = new HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO(datum);
                            list.Add(item);
                        }
                    }
                    else
                    {
                        list = null;
                    }
                    gridColumn_ServiceReq_Choose.Image = imageListCheck.Images[4];
                }
                gridControlServiceReq.BeginUpdate();
                gridControlServiceReq.DataSource = list;
                gridControlServiceReq.EndUpdate();
                grdSereServServiceReq.BeginUpdate();
                grdSereServServiceReq.DataSource = null;
                grdSereServServiceReq.EndUpdate();
                WaitingManager.Hide();
                LogSystem.Debug("FillDataToGridTransaction. 4");
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void SetFilter(ref HisServiceReqFilter filter)
        {
            try
            {
                bool flag = false;
                bool flag2 = false;
                if (filter == null)
                {
                    filter = new HisServiceReqFilter();
                }
                filter.ORDER_FIELD = "INTRUCTION_TIME";
                filter.ORDER_DIRECTION = "DESC";
                filter.ORDER_FIELD1 = "SERVICE_REQ_CODE";
                filter.ORDER_DIRECTION1 = "DESC";
                if (HisConfigCFG.IsUseGetDynamic)
                {
                    filter.ColumnParams = new List<string>
					{
						"BARCODE", "SESSION_CODE", "CALL_COUNT", "CALL_SAMPLE_ORDER", "CREATE_TIME", "CREATOR", "DESCRIPTION", "DHST_ID", "REMEDY_COUNT", "USE_TIME",
						"USE_TIME_TO", "TRACKING_ID", "KIDNEY_TIMES", "EXE_SERVICE_MODULE_ID", "EXECUTE_DEPARTMENT_ID", "EXECUTE_GROUP_ID", "EXECUTE_LOGINNAME", "EXECUTE_USERNAME", "EXECUTE_ROOM_ID", "EXP_MEST_TEMPLATE_ID",
						"FINISH_TIME", "ADVISE", "JSON_PRINT_ID", "ECG_AFTER", "ECG_BEFORE", "ICD_CAUSE_NAME", "ICD_CAUSE_CODE", "ICD_CODE", "ICD_NAME", "ICD_SUB_CODE",
						"ICD_TEXT", "ID", "INTRUCTION_DATE", "INTRUCTION_TIME", "IS_ACTIVE", "IS_EMERGENCY", "IS_EXECUTE_KIDNEY_PRES", "IS_HOME_PRES", "IS_KIDNEY", "IS_NO_EXECUTE",
						"IS_NOT_REQUIRE_FEE", "IS_WAIT_CHILD", "MACHINE_ID", "MODIFIER", "MODIFY_TIME", "NUM_ORDER", "PRIORITY", "REQUEST_DEPARTMENT_ID", "REQUEST_LOGINNAME", "REQUEST_USERNAME",
						"REQUEST_ROOM_ID", "SERVICE_REQ_CODE", "SERVICE_REQ_STT_ID", "SERVICE_REQ_TYPE_ID", "START_TIME", "IS_INTEGRATE_HIS_SENT", "TREATMENT_ID", "TREATMENT_TYPE_ID", "TDL_TREATMENT_CODE", "TDL_PATIENT_NAME",
						"SERVICE_REQ_TYPE_NAME", "EXECUTE_ROOM_NAME", "REQUEST_ROOM_NAME", "IS_MAIN_EXAM", "TDL_PATIENT_GENDER_ID", "TDL_PATIENT_DOB", "TDL_PATIENT_ID", "TDL_PATIENT_GENDER_NAME", "ATTACHMENT_FILE_URL", "LIS_STT_ID",
						"IS_SENT_EXT", "PRESCRIPTION_TYPE_ID", "TDL_SERVICE_TYPE_ID", "IS_ANTIBIOTIC_RESISTANCE", "RATION_TIME_ID"
					};
                    filter.ColumnParams = filter.ColumnParams.Distinct().ToList();
                }
                if (!string.IsNullOrEmpty(txtStoreCode.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(txtServiceReqCode.Text.Trim()) && !string.IsNullOrEmpty(txtTreatmentCode.Text.Trim()))
                    {
                        flag2 = true;
                        string text = txtServiceReqCode.Text.Trim();
                        string text2 = txtTreatmentCode.Text.Trim();
                        if (text.Length < 12 && checkDigit(text))
                        {
                            text = string.Format("{0:000000000000}", System.Convert.ToInt64(text));
                            txtServiceReqCode.Text = text;
                        }
                        filter.SERVICE_REQ_CODE__EXACT = text;
                        if (text2.Length < 12 && checkDigit(text2))
                        {
                            text2 = string.Format("{0:000000000000}", System.Convert.ToInt64(text2));
                            txtTreatmentCode.Text = text2;
                        }
                        filter.TREATMENT_CODE__EXACT = text2;
                    }
                    else
                    {
                        flag = true;
                        CommonParam commonParam = new CommonParam();
                        HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
                        hisTreatmentFilter.STORE_CODE__EXACT = txtStoreCode.Text.Trim();
                        List<HIS_TREATMENT> list = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, commonParam);
                        List<long> list2 = new List<long>();
                        foreach (HIS_TREATMENT item in list)
                        {
                            list2.Add(item.ID);
                        }
                        filter.TREATMENT_IDs = list2;
                    }
                }
                if (!string.IsNullOrEmpty(txtServiceReqCode.Text.Trim()))
                {
                    string text = txtServiceReqCode.Text.Trim();
                    if (text.Length < 12 && checkDigit(text))
                    {
                        text = string.Format("{0:000000000000}", System.Convert.ToInt64(text));
                        txtServiceReqCode.Text = text;
                    }
                    filter.SERVICE_REQ_CODE__EXACT = text;
                }
                else
                {
                    flag = true;
                    if (!string.IsNullOrEmpty(txtTreatmentCode.Text.Trim()))
                    {
                        string text2 = txtTreatmentCode.Text.Trim();
                        if (text2.Length < 12 && checkDigit(text2))
                        {
                            text2 = string.Format("{0:000000000000}", System.Convert.ToInt64(text2));
                            txtTreatmentCode.Text = text2;
                        }
                        filter.TREATMENT_CODE__EXACT = text2;
                    }
                    if (!string.IsNullOrEmpty(txtPatientCode.Text))
                    {
                        string text3 = txtPatientCode.Text.Trim();
                        if (text3.Length < 10)
                        {
                            text3 = string.Format("{0:0000000000}", System.Convert.ToInt64(text3));
                            txtPatientCode.Text = text3;
                        }
                        filter.TDL_PATIENT_CODE__EXACT = text3;
                    }
                }
                if (!flag)
                {
                    return;
                }
                filter.KEY_WORD = txtKeyword.Text.Trim();
                if (serviceReqTypeSelecteds != null && serviceReqTypeSelecteds.Count > 0)
                {
                    filter.SERVICE_REQ_TYPE_IDs = serviceReqTypeSelecteds.Select((HIS_SERVICE_REQ_TYPE o) => o.ID).ToList();
                }
                if (serviceReqSttSelecteds != null && serviceReqSttSelecteds.Count > 0)
                {
                    filter.SERVICE_REQ_STT_IDs = serviceReqSttSelecteds.Select((HIS_SERVICE_REQ_STT o) => o.ID).ToList();
                }
                switch (System.Convert.ToInt32(cboFilter.EditValue))
                {
                    case 0:
                        filter.CREATOR = ClientTokenManagerStore.ClientTokenManager.GetLoginName();
                        break;
                    case 1:
                        if (currentModule != null && currentModule.RoomId > 0)
                        {
                            filter.REQUEST_ROOM_ID = currentModule.RoomId;
                        }
                        break;
                    case 2:
                        filter.REQUEST_DEPARTMENT_ID = currentWorkPlace.DepartmentId;
                        break;
                    case 3:
                        filter.EXECUTE_DEPARTMENT_ID = currentWorkPlace.DepartmentId;
                        break;
                }
                if (currentPatient != null && currentPatient.ID > 0)
                {
                    filter.TDL_PATIENT_ID = currentPatient.ID;
                }
                if (dtIntructionTimeFrom.EditValue != null && dtIntructionTimeFrom.DateTime != DateTime.MinValue)
                {
                    filter.INTRUCTION_DATE_FROM = Parse.ToInt64(System.Convert.ToDateTime(dtIntructionTimeFrom.EditValue).ToString("yyyyMMdd") + "000000");
                }
                if (dtIntructionTimeTo.EditValue != null && dtIntructionTimeTo.DateTime != DateTime.MinValue)
                {
                    filter.INTRUCTION_DATE_TO = Parse.ToInt64(System.Convert.ToDateTime(dtIntructionTimeTo.EditValue).ToString("yyyyMMdd") + "235959");
                }
                if (cboExecuteRoom.EditValue != null && cboExecuteRoom.EditValue.ToString() != "")
                {
                    filter.EXECUTE_ROOM_ID = (long)cboExecuteRoom.EditValue;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private bool checkDigit(string s)
        {
            bool result = true;
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (!char.IsDigit(s[i]))
                    {
                        return false;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return false;
            }
        }

        private void InitComboServiceReqStt()
        {
            try
            {
                List<HIS_SERVICE_REQ_STT> list = BackendDataWorker.Get<HIS_SERVICE_REQ_STT>();
                if (list != null)
                {
                    cboServiceReqStt.Properties.DataSource = list;
                    cboServiceReqStt.Properties.DisplayMember = "SERVICE_REQ_STT_NAME";
                    cboServiceReqStt.Properties.ValueMember = "ID";
                    GridColumn gridColumn = cboServiceReqStt.Properties.View.Columns.AddField("SERVICE_REQ_STT_NAME");
                    gridColumn.VisibleIndex = 1;
                    gridColumn.Width = 200;
                    gridColumn.Caption = "";
                    cboServiceReqStt.Properties.PopupFormWidth = 200;
                    cboServiceReqStt.Properties.View.OptionsView.ShowColumnHeaders = false;
                    cboServiceReqStt.Properties.View.OptionsSelection.MultiSelect = true;
                    GridCheckMarksSelection gridCheckMarksSelection = cboServiceReqStt.Properties.Tag as GridCheckMarksSelection;
                    if (gridCheckMarksSelection != null)
                    {
                        gridCheckMarksSelection.ClearSelection(cboServiceReqStt.Properties.View);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void InitComboServiceReqType()
        {
            try
            {
                List<HIS_SERVICE_REQ_TYPE> list = BackendDataWorker.Get<HIS_SERVICE_REQ_TYPE>();
                if (list != null)
                {
                    cboServiceReqType.Properties.DataSource = list;
                    cboServiceReqType.Properties.DisplayMember = "SERVICE_REQ_TYPE_NAME";
                    cboServiceReqType.Properties.ValueMember = "ID";
                    GridColumn gridColumn = cboServiceReqType.Properties.View.Columns.AddField("SERVICE_REQ_TYPE_NAME");
                    gridColumn.VisibleIndex = 1;
                    gridColumn.Width = 200;
                    gridColumn.Caption = "";
                    cboServiceReqType.Properties.PopupFormWidth = 200;
                    cboServiceReqType.Properties.View.OptionsView.ShowColumnHeaders = false;
                    cboServiceReqType.Properties.View.OptionsSelection.MultiSelect = true;
                    GridCheckMarksSelection gridCheckMarksSelection = cboServiceReqType.Properties.Tag as GridCheckMarksSelection;
                    if (gridCheckMarksSelection != null)
                    {
                        gridCheckMarksSelection.ClearSelection(cboServiceReqType.Properties.View);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private async Task FillDataGridDetail(HIS_EXP_MEST dataExpMest, HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceClick)
        {
            List<ListMedicineADO> listMedicine = new List<ListMedicineADO>();
            try
            {
                CommonParam paramCommon = new CommonParam();
                if (serviceClick.SERVICE_REQ_TYPE_ID == 16)
                {
                    if (dataExpMest.ID > 0)
                    {
                        HisExpMestBltyReqFilter bltyFilter = new HisExpMestBltyReqFilter
                        {
                            EXP_MEST_ID = dataExpMest.ID
                        };
                        List<HIS_EXP_MEST_BLTY_REQ> bloods = await new BackendAdapter(paramCommon).GetAsync<List<HIS_EXP_MEST_BLTY_REQ>>("api/HisExpMestBltyReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, bltyFilter, new Action(SessionManager.ActionLostToken), paramCommon);
                        if (bloods != null && bloods.Count > 0)
                        {
                            List<IGrouping<long, HIS_EXP_MEST_BLTY_REQ>> list = (from o in bloods
                                                                                 group o by o.BLOOD_TYPE_ID).ToList();
                            foreach (IGrouping<long, HIS_EXP_MEST_BLTY_REQ> expMestBltyGroup in list)
                            {
                                ListMedicineADO listMedicineADO = new ListMedicineADO();
                                listMedicineADO.NUM_ORDER = expMestBltyGroup.First().NUM_ORDER ?? 999999;
                                listMedicineADO.AMOUNT = expMestBltyGroup.Sum((HIS_EXP_MEST_BLTY_REQ o) => o.AMOUNT);
                                listMedicineADO.CREATE_TIME = expMestBltyGroup.First().CREATE_TIME;
                                listMedicineADO.type = 2;
                                V_HIS_BLOOD_TYPE v_HIS_BLOOD_TYPE = BackendDataWorker.Get<V_HIS_BLOOD_TYPE>().FirstOrDefault((V_HIS_BLOOD_TYPE o) => o.ID == expMestBltyGroup.First().BLOOD_TYPE_ID);
                                if (v_HIS_BLOOD_TYPE != null)
                                {
                                    listMedicineADO.TDL_SERVICE_NAME = v_HIS_BLOOD_TYPE.BLOOD_TYPE_NAME;
                                    listMedicineADO.TDL_SERVICE_CODE = v_HIS_BLOOD_TYPE.BLOOD_TYPE_CODE;
                                    listMedicineADO.SERVICE_UNIT_NAME = v_HIS_BLOOD_TYPE.SERVICE_UNIT_NAME;
                                }
                                listMedicine.Add(listMedicineADO);
                            }
                        }
                    }
                }
                else if (serviceClick.SERVICE_REQ_TYPE_ID == 6 || serviceClick.SERVICE_REQ_TYPE_ID == 15 || serviceClick.SERVICE_REQ_TYPE_ID == 14)
                {
                    if (dataExpMest.ID > 0)
                    {
                        paramCommon = new CommonParam();
                        HisExpMestMedicineFilter mediFilter = new HisExpMestMedicineFilter
                        {
                            EXP_MEST_ID = dataExpMest.ID
                        };
                        List<HIS_EXP_MEST_MEDICINE> medicines = await new BackendAdapter(paramCommon).GetAsync<List<HIS_EXP_MEST_MEDICINE>>("api/HisExpMestMedicine/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, mediFilter, new Action(SessionManager.ActionLostToken), paramCommon);
                        if (medicines != null && medicines.Count > 0)
                        {
                            var list2 = (from o in medicines
                                         group o by new { o.TDL_MEDICINE_TYPE_ID, o.TUTORIAL }).ToList();
                            foreach (var expMestMetyGroup in list2)
                            {
                                ListMedicineADO listMedicineADO2 = new ListMedicineADO();
                                listMedicineADO2.NUM_ORDER = expMestMetyGroup.First().NUM_ORDER ?? 999999;
                                listMedicineADO2.AMOUNT = expMestMetyGroup.Sum((HIS_EXP_MEST_MEDICINE o) => o.AMOUNT);
                                listMedicineADO2.CREATE_TIME = expMestMetyGroup.First().CREATE_TIME;
                                listMedicineADO2.kind = 0;
                                listMedicineADO2.type = 1;
                                listMedicineADO2.HuongDanSuDung = expMestMetyGroup.First().TUTORIAL;
                                listMedicineADO2.TocDoTruyen = expMestMetyGroup.First().SPEED;
                                listMedicineADO2.ExpMestMedicineId = expMestMetyGroup.First().ID;
                                listMedicineADO2.TDL_INTRUCTION_TIME = dataExpMest.TDL_INTRUCTION_TIME.Value;
                                listMedicineADO2.USE_TIME_TO = expMestMetyGroup.First().USE_TIME_TO;
                                listMedicineADO2.PRES_AMOUNT = expMestMetyGroup.Sum((HIS_EXP_MEST_MEDICINE o) => o.PRES_AMOUNT ?? o.AMOUNT);
                                listMedicineADO2.USE_TIME = serviceClick.USE_TIME;
                                V_HIS_MEDICINE_TYPE v_HIS_MEDICINE_TYPE = BackendDataWorker.Get<V_HIS_MEDICINE_TYPE>().FirstOrDefault((V_HIS_MEDICINE_TYPE o) => o.ID == expMestMetyGroup.First().TDL_MEDICINE_TYPE_ID);
                                if (v_HIS_MEDICINE_TYPE != null)
                                {
                                    listMedicineADO2.TDL_SERVICE_NAME = v_HIS_MEDICINE_TYPE.MEDICINE_TYPE_NAME;
                                    listMedicineADO2.TDL_SERVICE_CODE = v_HIS_MEDICINE_TYPE.MEDICINE_TYPE_CODE;
                                    listMedicineADO2.SERVICE_UNIT_NAME = v_HIS_MEDICINE_TYPE.SERVICE_UNIT_NAME;
                                    listMedicineADO2.CONVERT_RATIO = v_HIS_MEDICINE_TYPE.CONVERT_RATIO;
                                    listMedicineADO2.CONVERT_UNIT_NAME = v_HIS_MEDICINE_TYPE.CONVERT_UNIT_NAME;
                                    if (v_HIS_MEDICINE_TYPE.CONVERT_RATIO.HasValue)
                                    {
                                        listMedicineADO2.CONVERT_AMOUNT = listMedicineADO2.AMOUNT * v_HIS_MEDICINE_TYPE.CONVERT_RATIO.Value;
                                    }
                                }
                                if (expMestMetyGroup.First().PATIENT_TYPE_ID.HasValue)
                                {
                                    HIS_PATIENT_TYPE hIS_PATIENT_TYPE = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == expMestMetyGroup.First().PATIENT_TYPE_ID);
                                    listMedicineADO2.PATIENT_TYPE_ID = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.ID : 0);
                                    listMedicineADO2.PATIENT_TYPE_NAME = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.PATIENT_TYPE_NAME : null);
                                    listMedicineADO2.IS_RATION = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.IS_RATION : ((short?)null));
                                }
                                listMedicine.Add(listMedicineADO2);
                            }
                        }
                        paramCommon = new CommonParam();
                        HisExpMestMaterialFilter mateFilter = new HisExpMestMaterialFilter
                        {
                            EXP_MEST_ID = dataExpMest.ID
                        };
                        List<HIS_EXP_MEST_MATERIAL> materials = await new BackendAdapter(paramCommon).GetAsync<List<HIS_EXP_MEST_MATERIAL>>("api/HisExpMestMaterial/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, mateFilter, new Action(SessionManager.ActionLostToken), paramCommon);
                        if (materials != null && materials.Count > 0)
                        {
                            var list3 = (from o in materials
                                         group o by new { o.TDL_MATERIAL_TYPE_ID, o.TUTORIAL }).ToList();
                            foreach (var expMestMatyGroup in list3)
                            {
                                ListMedicineADO listMedicineADO3 = new ListMedicineADO();
                                listMedicineADO3.NUM_ORDER = expMestMatyGroup.First().NUM_ORDER ?? 999999;
                                listMedicineADO3.AMOUNT = expMestMatyGroup.Sum((HIS_EXP_MEST_MATERIAL o) => o.AMOUNT);
                                listMedicineADO3.CREATE_TIME = expMestMatyGroup.First().CREATE_TIME;
                                listMedicineADO3.kind = 0;
                                listMedicineADO3.type = 0;
                                listMedicineADO3.HuongDanSuDung = expMestMatyGroup.First().TUTORIAL;
                                listMedicineADO3.PRES_AMOUNT = expMestMatyGroup.Sum((HIS_EXP_MEST_MATERIAL o) => o.PRES_AMOUNT ?? o.AMOUNT);
                                listMedicineADO3.USE_TIME = serviceClick.USE_TIME;
                                V_HIS_MATERIAL_TYPE v_HIS_MATERIAL_TYPE = BackendDataWorker.Get<V_HIS_MATERIAL_TYPE>().FirstOrDefault((V_HIS_MATERIAL_TYPE o) => o.ID == expMestMatyGroup.First().TDL_MATERIAL_TYPE_ID);
                                if (v_HIS_MATERIAL_TYPE != null)
                                {
                                    listMedicineADO3.TDL_SERVICE_CODE = v_HIS_MATERIAL_TYPE.MATERIAL_TYPE_CODE;
                                    listMedicineADO3.TDL_SERVICE_NAME = v_HIS_MATERIAL_TYPE.MATERIAL_TYPE_NAME;
                                    listMedicineADO3.SERVICE_UNIT_NAME = v_HIS_MATERIAL_TYPE.SERVICE_UNIT_NAME;
                                    listMedicineADO3.CONVERT_RATIO = v_HIS_MATERIAL_TYPE.CONVERT_RATIO;
                                    listMedicineADO3.CONVERT_UNIT_NAME = v_HIS_MATERIAL_TYPE.CONVERT_UNIT_NAME;
                                    if (v_HIS_MATERIAL_TYPE.CONVERT_RATIO.HasValue)
                                    {
                                        listMedicineADO3.CONVERT_AMOUNT = listMedicineADO3.AMOUNT * v_HIS_MATERIAL_TYPE.CONVERT_RATIO.Value;
                                    }
                                }
                                if (expMestMatyGroup.First().PATIENT_TYPE_ID.HasValue)
                                {
                                    HIS_PATIENT_TYPE hIS_PATIENT_TYPE = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == expMestMatyGroup.First().PATIENT_TYPE_ID);
                                    listMedicineADO3.PATIENT_TYPE_ID = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.ID : 0);
                                    listMedicineADO3.PATIENT_TYPE_NAME = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.PATIENT_TYPE_NAME : null);
                                    listMedicineADO3.IS_RATION = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.IS_RATION : ((short?)null));
                                }
                                listMedicine.Add(listMedicineADO3);
                            }
                        }
                    }
                    paramCommon = new CommonParam();
                    HisServiceReqMetyFilter metyFilter = new HisServiceReqMetyFilter
                    {
                        SERVICE_REQ_ID = dataExpMest.SERVICE_REQ_ID
                    };
                    List<HIS_SERVICE_REQ_METY> metys = await new BackendAdapter(paramCommon).GetAsync<List<HIS_SERVICE_REQ_METY>>("api/HisServiceReqMety/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, metyFilter, new Action(SessionManager.ActionLostToken), paramCommon);
                    if (metys != null && metys.Count > 0)
                    {
                        if (!metys.Exists((HIS_SERVICE_REQ_METY p) => !p.MEDICINE_TYPE_ID.HasValue))
                        {
                            var list4 = (from o in metys
                                         group o by new { o.MEDICINE_TYPE_ID, o.TUTORIAL }).ToList();
                            foreach (var expMestMetyGroup2 in list4)
                            {
                                ListMedicineADO listMedicineADO2 = new ListMedicineADO();
                                listMedicineADO2.TDL_SERVICE_NAME = expMestMetyGroup2.First().MEDICINE_TYPE_NAME;
                                listMedicineADO2.NUM_ORDER = expMestMetyGroup2.First().NUM_ORDER ?? 999999;
                                listMedicineADO2.AMOUNT = expMestMetyGroup2.Sum((HIS_SERVICE_REQ_METY o) => o.AMOUNT);
                                listMedicineADO2.CREATE_TIME = expMestMetyGroup2.First().CREATE_TIME;
                                listMedicineADO2.HuongDanSuDung = expMestMetyGroup2.First().TUTORIAL;
                                listMedicineADO2.TocDoTruyen = expMestMetyGroup2.First().SPEED;
                                listMedicineADO2.subPress = expMestMetyGroup2.First().IS_SUB_PRES;
                                listMedicineADO2.kind = 1;
                                listMedicineADO2.type = 1;
                                listMedicineADO2.serviceReqMety = expMestMetyGroup2.First();
                                listMedicineADO2.USE_TIME = serviceClick.USE_TIME;
                                V_HIS_MEDICINE_TYPE v_HIS_MEDICINE_TYPE = BackendDataWorker.Get<V_HIS_MEDICINE_TYPE>().FirstOrDefault((V_HIS_MEDICINE_TYPE o) => o.ID == expMestMetyGroup2.First().MEDICINE_TYPE_ID);
                                if (v_HIS_MEDICINE_TYPE != null)
                                {
                                    listMedicineADO2.isStartMark = v_HIS_MEDICINE_TYPE.IS_STAR_MARK;
                                    listMedicineADO2.TDL_SERVICE_CODE = v_HIS_MEDICINE_TYPE.MEDICINE_TYPE_CODE;
                                    listMedicineADO2.SERVICE_UNIT_NAME = v_HIS_MEDICINE_TYPE.SERVICE_UNIT_NAME;
                                    listMedicineADO2.CONVERT_RATIO = v_HIS_MEDICINE_TYPE.CONVERT_RATIO;
                                    listMedicineADO2.CONVERT_UNIT_NAME = v_HIS_MEDICINE_TYPE.CONVERT_UNIT_NAME;
                                    if (v_HIS_MEDICINE_TYPE.CONVERT_RATIO.HasValue)
                                    {
                                        listMedicineADO2.CONVERT_AMOUNT = listMedicineADO2.AMOUNT * v_HIS_MEDICINE_TYPE.CONVERT_RATIO.Value;
                                    }
                                }
                                listMedicine.Add(listMedicineADO2);
                            }
                        }
                        else
                        {
                            List<HIS_SERVICE_REQ_METY> list5 = metys.Where((HIS_SERVICE_REQ_METY o) => o.MEDICINE_TYPE_ID.HasValue).ToList();
                            if (list5 != null && list5.Count > 0)
                            {
                                var list4 = (from o in list5
                                             group o by new { o.MEDICINE_TYPE_ID, o.TUTORIAL }).ToList();
                                foreach (var expMestMetyGroup3 in list4)
                                {
                                    ListMedicineADO listMedicineADO2 = new ListMedicineADO();
                                    listMedicineADO2.TDL_SERVICE_NAME = expMestMetyGroup3.First().MEDICINE_TYPE_NAME;
                                    listMedicineADO2.NUM_ORDER = expMestMetyGroup3.First().NUM_ORDER ?? 999999;
                                    listMedicineADO2.AMOUNT = expMestMetyGroup3.Sum((HIS_SERVICE_REQ_METY o) => o.AMOUNT);
                                    listMedicineADO2.CREATE_TIME = expMestMetyGroup3.First().CREATE_TIME;
                                    listMedicineADO2.HuongDanSuDung = expMestMetyGroup3.First().TUTORIAL;
                                    listMedicineADO2.TocDoTruyen = expMestMetyGroup3.First().SPEED;
                                    listMedicineADO2.subPress = expMestMetyGroup3.First().IS_SUB_PRES;
                                    listMedicineADO2.kind = 1;
                                    listMedicineADO2.type = 1;
                                    listMedicineADO2.serviceReqMety = expMestMetyGroup3.First();
                                    listMedicineADO2.USE_TIME = serviceClick.USE_TIME;
                                    V_HIS_MEDICINE_TYPE v_HIS_MEDICINE_TYPE = BackendDataWorker.Get<V_HIS_MEDICINE_TYPE>().FirstOrDefault((V_HIS_MEDICINE_TYPE o) => o.ID == expMestMetyGroup3.First().MEDICINE_TYPE_ID);
                                    if (v_HIS_MEDICINE_TYPE != null)
                                    {
                                        listMedicineADO2.isStartMark = v_HIS_MEDICINE_TYPE.IS_STAR_MARK;
                                        listMedicineADO2.TDL_SERVICE_CODE = v_HIS_MEDICINE_TYPE.MEDICINE_TYPE_CODE;
                                        listMedicineADO2.SERVICE_UNIT_NAME = v_HIS_MEDICINE_TYPE.SERVICE_UNIT_NAME;
                                        listMedicineADO2.CONVERT_RATIO = v_HIS_MEDICINE_TYPE.CONVERT_RATIO;
                                        listMedicineADO2.CONVERT_UNIT_NAME = v_HIS_MEDICINE_TYPE.CONVERT_UNIT_NAME;
                                        if (v_HIS_MEDICINE_TYPE.CONVERT_RATIO.HasValue)
                                        {
                                            listMedicineADO2.CONVERT_AMOUNT = listMedicineADO2.AMOUNT * v_HIS_MEDICINE_TYPE.CONVERT_RATIO.Value;
                                        }
                                    }
                                    listMedicine.Add(listMedicineADO2);
                                }
                            }
                            List<HIS_SERVICE_REQ_METY> list6 = metys.Where((HIS_SERVICE_REQ_METY o) => !o.MEDICINE_TYPE_ID.HasValue).ToList();
                            if (list6 != null && list6.Count > 0)
                            {
                                var list7 = (from o in list6
                                             group o by new { o.MEDICINE_TYPE_NAME, o.MEDICINE_USE_FORM_ID, o.TUTORIAL }).ToList();
                                foreach (var item3 in list7)
                                {
                                    ListMedicineADO listMedicineADO2 = new ListMedicineADO();
                                    listMedicineADO2.TDL_SERVICE_NAME = item3.First().MEDICINE_TYPE_NAME;
                                    listMedicineADO2.NUM_ORDER = item3.First().NUM_ORDER ?? 999999;
                                    listMedicineADO2.AMOUNT = item3.Sum((HIS_SERVICE_REQ_METY o) => o.AMOUNT);
                                    listMedicineADO2.CREATE_TIME = item3.First().CREATE_TIME;
                                    listMedicineADO2.HuongDanSuDung = item3.First().TUTORIAL;
                                    listMedicineADO2.TocDoTruyen = item3.First().SPEED;
                                    listMedicineADO2.subPress = item3.First().IS_SUB_PRES;
                                    listMedicineADO2.type = 1;
                                    listMedicineADO2.kind = 1;
                                    listMedicineADO2.serviceReqMety = item3.First();
                                    listMedicineADO2.USE_TIME = serviceClick.USE_TIME;
                                    listMedicineADO2.SERVICE_UNIT_NAME = item3.First().UNIT_NAME;
                                    listMedicine.Add(listMedicineADO2);
                                }
                            }
                        }
                    }
                    paramCommon = new CommonParam();
                    HisServiceReqMatyFilter matyFilter = new HisServiceReqMatyFilter
                    {
                        SERVICE_REQ_ID = dataExpMest.SERVICE_REQ_ID
                    };
                    List<HIS_SERVICE_REQ_MATY> matys = await new BackendAdapter(paramCommon).GetAsync<List<HIS_SERVICE_REQ_MATY>>("api/HisServiceReqMaty/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, matyFilter, new Action(SessionManager.ActionLostToken), paramCommon);
                    if (matys != null && matys.Count > 0)
                    {
                        if (!matys.Exists((HIS_SERVICE_REQ_MATY p) => !p.MATERIAL_TYPE_ID.HasValue))
                        {
                            var list8 = (from o in matys
                                         group o by new { o.MATERIAL_TYPE_ID, o.TUTORIAL }).ToList();
                            foreach (var expMestMatyGroup2 in list8)
                            {
                                ListMedicineADO listMedicineADO3 = new ListMedicineADO();
                                listMedicineADO3.NUM_ORDER = expMestMatyGroup2.First().NUM_ORDER ?? 999999;
                                listMedicineADO3.AMOUNT = expMestMatyGroup2.Sum((HIS_SERVICE_REQ_MATY o) => o.AMOUNT);
                                listMedicineADO3.CREATE_TIME = expMestMatyGroup2.First().CREATE_TIME;
                                listMedicineADO3.subPress = expMestMatyGroup2.First().IS_SUB_PRES;
                                listMedicineADO3.kind = 1;
                                listMedicineADO3.type = 0;
                                listMedicineADO3.HuongDanSuDung = expMestMatyGroup2.First().TUTORIAL;
                                listMedicineADO3.USE_TIME = serviceClick.USE_TIME;
                                V_HIS_MATERIAL_TYPE v_HIS_MATERIAL_TYPE = BackendDataWorker.Get<V_HIS_MATERIAL_TYPE>().FirstOrDefault((V_HIS_MATERIAL_TYPE o) => o.ID == expMestMatyGroup2.First().MATERIAL_TYPE_ID);
                                if (v_HIS_MATERIAL_TYPE != null)
                                {
                                    listMedicineADO3.TDL_SERVICE_CODE = v_HIS_MATERIAL_TYPE.MATERIAL_TYPE_CODE;
                                    listMedicineADO3.TDL_SERVICE_NAME = v_HIS_MATERIAL_TYPE.MATERIAL_TYPE_NAME;
                                    listMedicineADO3.SERVICE_UNIT_NAME = v_HIS_MATERIAL_TYPE.SERVICE_UNIT_NAME;
                                    listMedicineADO3.CONVERT_RATIO = v_HIS_MATERIAL_TYPE.CONVERT_RATIO;
                                    listMedicineADO3.CONVERT_UNIT_NAME = v_HIS_MATERIAL_TYPE.CONVERT_UNIT_NAME;
                                    if (v_HIS_MATERIAL_TYPE.CONVERT_RATIO.HasValue)
                                    {
                                        listMedicineADO3.CONVERT_AMOUNT = listMedicineADO3.AMOUNT * v_HIS_MATERIAL_TYPE.CONVERT_RATIO.Value;
                                    }
                                }
                                listMedicine.Add(listMedicineADO3);
                            }
                        }
                        else
                        {
                            List<HIS_SERVICE_REQ_MATY> list9 = matys.Where((HIS_SERVICE_REQ_MATY o) => o.MATERIAL_TYPE_ID.HasValue).ToList();
                            if (list9 != null && list9.Count > 0)
                            {
                                var list8 = (from o in list9
                                             group o by new { o.MATERIAL_TYPE_ID, o.TUTORIAL }).ToList();
                                foreach (var expMestMatyGroup3 in list8)
                                {
                                    ListMedicineADO listMedicineADO3 = new ListMedicineADO();
                                    listMedicineADO3.NUM_ORDER = expMestMatyGroup3.First().NUM_ORDER ?? 999999;
                                    listMedicineADO3.AMOUNT = expMestMatyGroup3.Sum((HIS_SERVICE_REQ_MATY o) => o.AMOUNT);
                                    listMedicineADO3.CREATE_TIME = expMestMatyGroup3.First().CREATE_TIME;
                                    listMedicineADO3.subPress = expMestMatyGroup3.First().IS_SUB_PRES;
                                    listMedicineADO3.kind = 1;
                                    listMedicineADO3.type = 0;
                                    listMedicineADO3.HuongDanSuDung = expMestMatyGroup3.First().TUTORIAL;
                                    listMedicineADO3.USE_TIME = serviceClick.USE_TIME;
                                    V_HIS_MATERIAL_TYPE v_HIS_MATERIAL_TYPE = BackendDataWorker.Get<V_HIS_MATERIAL_TYPE>().FirstOrDefault((V_HIS_MATERIAL_TYPE o) => o.ID == expMestMatyGroup3.First().MATERIAL_TYPE_ID);
                                    if (v_HIS_MATERIAL_TYPE != null)
                                    {
                                        listMedicineADO3.TDL_SERVICE_CODE = v_HIS_MATERIAL_TYPE.MATERIAL_TYPE_CODE;
                                        listMedicineADO3.TDL_SERVICE_NAME = v_HIS_MATERIAL_TYPE.MATERIAL_TYPE_NAME;
                                        listMedicineADO3.SERVICE_UNIT_NAME = v_HIS_MATERIAL_TYPE.SERVICE_UNIT_NAME;
                                        listMedicineADO3.CONVERT_RATIO = v_HIS_MATERIAL_TYPE.CONVERT_RATIO;
                                        listMedicineADO3.CONVERT_UNIT_NAME = v_HIS_MATERIAL_TYPE.CONVERT_UNIT_NAME;
                                        if (v_HIS_MATERIAL_TYPE.CONVERT_RATIO.HasValue)
                                        {
                                            listMedicineADO3.CONVERT_AMOUNT = listMedicineADO3.AMOUNT * v_HIS_MATERIAL_TYPE.CONVERT_RATIO.Value;
                                        }
                                    }
                                    listMedicine.Add(listMedicineADO3);
                                }
                            }
                            List<HIS_SERVICE_REQ_MATY> list10 = matys.Where((HIS_SERVICE_REQ_MATY o) => !o.MATERIAL_TYPE_ID.HasValue).ToList();
                            if (list10 != null && list10.Count > 0)
                            {
                                var list8 = (from o in list10
                                             group o by new { o.MATERIAL_TYPE_ID, o.TUTORIAL }).ToList();
                                foreach (var item4 in list8)
                                {
                                    ListMedicineADO listMedicineADO3 = new ListMedicineADO();
                                    listMedicineADO3.NUM_ORDER = item4.First().NUM_ORDER ?? 999999;
                                    listMedicineADO3.AMOUNT = item4.Sum((HIS_SERVICE_REQ_MATY o) => o.AMOUNT);
                                    listMedicineADO3.CREATE_TIME = item4.First().CREATE_TIME;
                                    listMedicineADO3.subPress = item4.First().IS_SUB_PRES;
                                    listMedicineADO3.kind = 1;
                                    listMedicineADO3.type = 0;
                                    listMedicineADO3.SERVICE_UNIT_NAME = item4.First().UNIT_NAME;
                                    listMedicineADO3.HuongDanSuDung = item4.First().TUTORIAL;
                                    listMedicineADO3.USE_TIME = serviceClick.USE_TIME;
                                    listMedicine.Add(listMedicineADO3);
                                }
                            }
                        }
                    }
                }
                else if (serviceClick.SERVICE_REQ_TYPE_ID == 17)
                {
                    HisSereServRationFilter rationFilter = new HisSereServRationFilter
                    {
                        SERVICE_REQ_ID = serviceClick.ID,
                        ORDER_DIRECTION = "DESC",
                        ORDER_FIELD = "ID"
                    };
                    paramCommon = new CommonParam();
                    List<V_HIS_SERE_SERV_RATION> listRation = await new BackendAdapter(paramCommon).GetAsync<List<V_HIS_SERE_SERV_RATION>>("api/HisSereServRation/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, rationFilter, new Action(SessionManager.ActionLostToken), paramCommon);
                    if (listRation != null && listRation.Count > 0)
                    {
                        foreach (V_HIS_SERE_SERV_RATION item in listRation)
                        {
                            ListMedicineADO listMedicineADO4 = new ListMedicineADO();
                            listMedicineADO4.IS_RATION = item.IS_RATION;
                            listMedicineADO4.AMOUNT = item.AMOUNT;
                            listMedicineADO4.DISCOUNT = item.DISCOUNT;
                            listMedicineADO4.HuongDanSuDung = item.INSTRUCTION_NOTE;
                            listMedicineADO4.PATIENT_TYPE_ID = item.PATIENT_TYPE_ID;
                            listMedicineADO4.PATIENT_TYPE_NAME = item.PATIENT_TYPE_NAME;
                            listMedicineADO4.PRICE = item.PRICE;
                            listMedicineADO4.DISCOUNT = item.DISCOUNT;
                            listMedicineADO4.VAT_RATIO = item.VAT_RATIO ?? 0m;
                            listMedicineADO4.SERVICE_ID = item.SERVICE_ID;
                            V_HIS_SERVICE v_HIS_SERVICE = BackendDataWorker.Get<V_HIS_SERVICE>().FirstOrDefault((V_HIS_SERVICE o) => o.ID == item.SERVICE_ID);
                            if (v_HIS_SERVICE != null)
                            {
                                listMedicineADO4.TDL_SERVICE_CODE = v_HIS_SERVICE.SERVICE_CODE;
                                listMedicineADO4.TDL_SERVICE_NAME = v_HIS_SERVICE.SERVICE_NAME;
                                listMedicineADO4.SERVICE_UNIT_NAME = v_HIS_SERVICE.SERVICE_UNIT_NAME;
                            }
                            listMedicine.Add(listMedicineADO4);
                        }
                    }
                }
                if (listMedicine != null && listMedicine.Count > 0)
                {
                    listMedicine = listMedicine.OrderBy((ListMedicineADO o) => o.NUM_ORDER).ToList();
                }
                else
                {
                    HisSereServFilter filter = new HisSereServFilter
                    {
                        SERVICE_REQ_ID = serviceClick.ID,
                        ORDER_DIRECTION = "DESC",
                        ORDER_FIELD = "ID"
                    };
                    paramCommon = new CommonParam();
                    listMedicine = await new BackendAdapter(paramCommon).GetAsync<List<ListMedicineADO>>("api/HisSereServ/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, new Action(SessionManager.ActionLostToken), paramCommon);
                    if (listMedicine != null)
                    {
                        foreach (ListMedicineADO item2 in listMedicine)
                        {
                            List<HIS_SERVICE_UNIT> source = BackendDataWorker.Get<HIS_SERVICE_UNIT>();
                            Func<HIS_SERVICE_UNIT, bool> predicate = (HIS_SERVICE_UNIT o) => o.ID == item2.TDL_SERVICE_UNIT_ID;
                            HIS_SERVICE_UNIT unit = source.FirstOrDefault(predicate);
                            if (unit != null)
                            {
                                item2.SERVICE_UNIT_NAME = unit.SERVICE_UNIT_NAME;
                                item2.CONVERT_RATIO = unit.CONVERT_RATIO;
                                if (unit.CONVERT_ID.HasValue && unit.CONVERT_RATIO.HasValue)
                                {
                                    HIS_SERVICE_UNIT hIS_SERVICE_UNIT = BackendDataWorker.Get<HIS_SERVICE_UNIT>().FirstOrDefault((HIS_SERVICE_UNIT o) => o.ID == unit.CONVERT_ID);
                                    if (hIS_SERVICE_UNIT != null)
                                    {
                                        item2.CONVERT_UNIT_NAME = hIS_SERVICE_UNIT.SERVICE_UNIT_NAME;
                                        item2.CONVERT_AMOUNT = item2.AMOUNT * unit.CONVERT_RATIO.Value;
                                    }
                                }
                            }
                            HIS_PATIENT_TYPE hIS_PATIENT_TYPE = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == item2.PATIENT_TYPE_ID);
                            item2.PATIENT_TYPE_ID = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.ID : 0);
                            item2.PATIENT_TYPE_NAME = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.PATIENT_TYPE_NAME : null);
                            item2.IS_RATION = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.IS_RATION : ((short?)null));
                            item2.USE_TIME = serviceClick.USE_TIME;
                            V_HIS_SERVICE v_HIS_SERVICE = BackendDataWorker.Get<V_HIS_SERVICE>().FirstOrDefault((V_HIS_SERVICE o) => o.ID == item2.SERVICE_ID);
                            if (v_HIS_SERVICE != null)
                            {
                                item2.PTTT_GROUP_NAME = v_HIS_SERVICE.PTTT_GROUP_NAME;
                            }
                        }
                    }
                }
                _listMedicine = new List<ListMedicineADO>();
                _listMedicine.AddRange(listMedicine);
                grdViewSereServServiceReq.BeginUpdate();
                grdViewSereServServiceReq.GridControl.DataSource = listMedicine;
                grdViewSereServServiceReq.EndUpdate();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ControlServiceReqClick(HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data)
        {
            try
            {
                if (data == null)
                {
                    return;
                }
                if (data == null || data.ID <= 0)
                {
                    return;
                }
                WaitingManager.Show();
                CommonParam commonParam = new CommonParam();
                HIS_EXP_MEST hIS_EXP_MEST = null;
                if (data.SERVICE_REQ_TYPE_ID == 6 || data.SERVICE_REQ_TYPE_ID == 16 || data.SERVICE_REQ_TYPE_ID == 15 || data.SERVICE_REQ_TYPE_ID == 14)
                {
                    currentPrescription = null;
                    HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
                    hisExpMestFilter.SERVICE_REQ_ID = data.ID;
                    List<HIS_EXP_MEST> list = new BackendAdapter(new CommonParam()).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, new Action(SessionManager.ActionLostToken), null);
                    if (list != null && list.Count > 0)
                    {
                        currentPrescription = list.FirstOrDefault();
                        hIS_EXP_MEST = list.FirstOrDefault();
                    }
                    else
                    {
                        hIS_EXP_MEST = new HIS_EXP_MEST();
                        hIS_EXP_MEST.SERVICE_REQ_ID = data.ID;
                    }
                }
                FillDataGridDetail(hIS_EXP_MEST, data);
                FillDataToControl(hIS_EXP_MEST, data);
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private async Task GetSereServ(HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceClick)
        {
            try
            {
                if (serviceClick == null || serviceClick.ID <= 0)
                {
                    return;
                }
                new CommonParam();
                HIS_EXP_MEST expMest = null;
                if (serviceClick.SERVICE_REQ_TYPE_ID == 6 || serviceClick.SERVICE_REQ_TYPE_ID == 16 || serviceClick.SERVICE_REQ_TYPE_ID == 15 || serviceClick.SERVICE_REQ_TYPE_ID == 14)
                {
                    currentPrescription = null;
                    HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
                    hisExpMestFilter.SERVICE_REQ_ID = serviceClick.ID;
                    List<HIS_EXP_MEST> list = new BackendAdapter(new CommonParam()).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, new Action(SessionManager.ActionLostToken), null);
                    if (list != null && list.Count > 0)
                    {
                        currentPrescription = list.FirstOrDefault();
                        expMest = list.FirstOrDefault();
                    }
                    else
                    {
                        expMest = new HIS_EXP_MEST
                        {
                            SERVICE_REQ_ID = serviceClick.ID
                        };
                    }
                }
                await FillDataGridDetail(expMest, serviceClick);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridControlServiceReq_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    LogSystem.Debug(LogUtil.TraceData("serviceClick___", serviceReqADO));
                    if (serviceReqADO != null && serviceReqADO.ID != 0)
                    {
                        currentServiceReq = serviceReqADO;
                        ControlServiceReqClick(currentServiceReq);
                    }
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void gridViewServiceReq_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                if (!e.IsGetData || e.Column.UnboundType == UnboundColumnType.Bound)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                if (data == null)
                {
                    return;
                }
                if (e.Column.FieldName == "STT")
                {
                    try
                    {
                        e.Value = e.ListSourceRowIndex + 1 + (((ucPaging1.pagingGrid != null) ? ucPaging1.pagingGrid.CurrentPage : 0) - 1) * ((ucPaging1.pagingGrid != null) ? ucPaging1.pagingGrid.PageSize : 0);
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        return;
                    }
                }
                if (e.Column.FieldName == "IMG")
                {
                    try
                    {
                        long sERVICE_REQ_STT_ID = data.SERVICE_REQ_STT_ID;
                        if (sERVICE_REQ_STT_ID == 1 && data.SERVICE_REQ_TYPE_ID == 2 && ((HIS_SERVICE_REQ)data).SAMPLE_TIME.HasValue)
                        {
                            e.Value = imageListIcon.Images[6];
                        }
                        else if (sERVICE_REQ_STT_ID == 2 && data.SERVICE_REQ_TYPE_ID == 2 && ((HIS_SERVICE_REQ)data).RECEIVE_SAMPLE_TIME.HasValue)
                        {
                            e.Value = imageListIcon.Images[2];
                        }
                        else
                        {
                            switch (sERVICE_REQ_STT_ID)
                            {
                                case 1L:
                                    e.Value = imageListIcon.Images[0];
                                    break;
                                case 2L:
                                    e.Value = imageListIcon.Images[1];
                                    break;
                                case 3L:
                                    e.Value = imageListIcon.Images[3];
                                    break;
                                default:
                                    e.Value = imageListIcon.Images[0];
                                    break;
                            }
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Warn("Loi set gia tri cho cot icon trang thai yeu cau dich vu IMG", ex);
                        return;
                    }
                }
                if (e.Column.FieldName == "PRIORITY_DISPLAY")
                {
                    long num = data.PRIORITY ?? 0;
                    if (num == 1)
                    {
                        e.Value = imageListPriority.Images[0];
                    }
                    return;
                }
                if (e.Column.FieldName == "REQUEST_USERNAME_DISPLAY")
                {
                    try
                    {
                        e.Value = data.REQUEST_LOGINNAME + (string.IsNullOrEmpty(data.REQUEST_USERNAME) ? "" : (" - " + data.REQUEST_USERNAME));
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        return;
                    }
                }
                if (e.Column.FieldName == "EXECUTE_USERNAME_DISPLAY")
                {
                    try
                    {
                        e.Value = data.EXECUTE_LOGINNAME + (string.IsNullOrEmpty(data.EXECUTE_USERNAME) ? "" : (" - " + data.EXECUTE_USERNAME));
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        return;
                    }
                }
                if (e.Column.FieldName == "INTRUCTION_TIME_STR")
                {
                    try
                    {
                        e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(data.INTRUCTION_TIME);
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        return;
                    }
                }
                if (e.Column.FieldName == "CREATE_TIME_STR")
                {
                    try
                    {
                        e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(data.CREATE_TIME ?? 0);
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        return;
                    }
                }
                if (e.Column.FieldName == "MODIFY_TIME_STR")
                {
                    try
                    {
                        e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(data.MODIFY_TIME ?? 0);
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        return;
                    }
                }
                if (e.Column.FieldName == "YEAR")
                {
                    try
                    {
                        e.Value = data.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        return;
                    }
                }
                if (e.Column.FieldName == "MAIN_EXAM")
                {
                    try
                    {
                        if (data.SERVICE_REQ_TYPE_ID == 1)
                        {
                            e.Value = ((data.IS_MAIN_EXAM == 1) ? "Khám chính" : "");
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        return;
                    }
                }
                if (e.Column.FieldName == "IS_INTEGRATE_HIS_SENT_STR")
                {
                    try
                    {
                        e.Value = ((data.IS_INTEGRATE_HIS_SENT == 1) ? "Đã gửi" : "Chưa gửi");
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Error(ex);
                        return;
                    }
                }
                if (!(e.Column.FieldName == "RATION_TIME_STR"))
                {
                    return;
                }
                try
                {
                    HIS_RATION_TIME hIS_RATION_TIME = lsRationTime.FirstOrDefault((HIS_RATION_TIME o) => o.ID == (data.RATION_TIME_ID ?? 0));
                    if (hIS_RATION_TIME != null)
                    {
                        e.Value = hIS_RATION_TIME.RATION_TIME_NAME;
                    }
                }
                catch (Exception ex)
                {
                    LogSystem.Error(ex);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridViewServiceReq_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetRow(e.RowHandle);
                V_HIS_ROOM v_HIS_ROOM = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == currentModule.RoomId);
                string text = (gridViewServiceReq.GetRowCellValue(e.RowHandle, "CREATOR") ?? "").ToString().Trim();
                string text2 = (gridViewServiceReq.GetRowCellValue(e.RowHandle, "REQUEST_LOGINNAME") ?? "").ToString().Trim();
                long num = Parse.ToInt64((gridViewServiceReq.GetRowCellValue(e.RowHandle, "SERVICE_REQ_STT_ID") ?? "").ToString().Trim());
                long num2 = Parse.ToInt64((gridViewServiceReq.GetRowCellValue(e.RowHandle, "SERVICE_REQ_TYPE_ID") ?? "").ToString().Trim());
                short num3 = Parse.ToInt16((gridViewServiceReq.GetRowCellValue(e.RowHandle, "IS_NO_EXECUTE") ?? "").ToString());
                short num4 = Parse.ToInt16((gridViewServiceReq.GetRowCellValue(e.RowHandle, "IS_CONFIRM_NO_EXCUTE") ?? "").ToString());
                string value = (gridViewServiceReq.GetRowCellValue(e.RowHandle, "JSON_PRINT_ID") ?? "").ToString().Trim();
                long num5 = Parse.ToInt64((gridViewServiceReq.GetRowCellValue(e.RowHandle, "REQUEST_DEPARTMENT_ID") ?? "").ToString().Trim());
                long num6 = Parse.ToInt64((gridViewServiceReq.GetRowCellValue(e.RowHandle, "REQUEST_ROOM_ID") ?? "").ToString().Trim());
                long num7 = Parse.ToInt64((gridViewServiceReq.GetRowCellValue(e.RowHandle, "EXECUTE_ROOM_ID") ?? "").ToString().Trim());
                if (e.Column.FieldName == "ServiceReqDelete")
                {
                    bool flag = loginName == text || CheckLoginAdmin.IsAdmin(loginName) || loginName == text2;
                    bool flag2 = currentRoom != null && (currentRoom.ID == num7 || currentRoom.ID == num6);
                    if (num == 1 && (flag || (num2 == 1 && num5 == currentRoom.DEPARTMENT_ID && flag2)))
                    {
                        e.RepositoryItem = repositoryItemBtnDeleteServiceReq;
                    }
                    else
                    {
                        e.RepositoryItem = repositoryItemBtnDeleteServiceReqDisable;
                    }
                    if (((HIS_SERVICE_REQ)serviceReqADO).CARER_CARD_BORROW_ID.HasValue)
                    {
                        e.RepositoryItem = repositoryItemBtnDeleteServiceReqDisable;
                    }
                }
                else if (e.Column.FieldName == "BieuMauKhac")
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        e.RepositoryItem = repositoryItemBtnBieuMauKhac;
                    }
                    else
                    {
                        e.RepositoryItem = repositoryItemReadOnly;
                    }
                }
                else if (e.Column.FieldName == "ServiceReqEdit")
                {
                    if (text == loginName || text2 == loginName || CheckLoginAdmin.IsAdmin(loginName))
                    {
                        if (num3 != 1 && (num == 1 || HisConfigs.Get<string>("MOS.HIS_SERVICE_REQ.ALLOW_MODIFYING_OF_STARTED") == "1" || (HisConfigs.Get<string>("MOS.HIS_SERVICE_REQ.ALLOW_MODIFYING_OF_STARTED") == "2" && num2 == 1)))
                        {
                            e.RepositoryItem = repositoryItemBtnEditServiceReq;
                        }
                    }
                    else
                    {
                        e.RepositoryItem = repositoryItemReadOnly;
                    }
                    if (((HIS_SERVICE_REQ)serviceReqADO).CARER_CARD_BORROW_ID.HasValue)
                    {
                        e.RepositoryItem = repositoryItemBtnEditServiceReqDisable;
                    }
                }
                else if (e.Column.FieldName == "AllowNotExecute")
                {
                    if (((HIS_SERVICE_REQ)serviceReqADO).IS_ACCEPTING_NO_EXECUTE == 1)
                    {
                        e.RepositoryItem = repositoryItemButtonEditAllowNotExecute_Enable;
                    }
                    else
                    {
                        e.RepositoryItem = repositoryItemButtonEditAllowNotExecute_Disable;
                    }
                }
                else if (e.Column.FieldName == "ServiceReqPrint")
                {
                    if (((HIS_SERVICE_REQ)serviceReqADO).CARER_CARD_BORROW_ID.HasValue)
                    {
                        e.RepositoryItem = repositoryItemBtnPrintServiceReqDisable;
                    }
                    else
                    {
                        e.RepositoryItem = repositoryItemBtnPrintServiceReq;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridViewServiceReq_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetRow(e.RowHandle);
                if (serviceReqADO != null)
                {
                    if (serviceReqADO.IS_NO_EXECUTE == 1)
                    {
                        e.Appearance.Font = new System.Drawing.Font(e.Appearance.Font, FontStyle.Strikeout);
                    }
                    if (((HIS_SERVICE_REQ)serviceReqADO).IS_TEMPORARY_PRES == 1)
                    {
                        e.Appearance.ForeColor = Color.Orange;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridViewServiceReq_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = (List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>)gridControlServiceReq.DataSource;
                if (list.Count <= 0)
                {
                    return;
                }
                listServiceReq = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in list)
                {
                    if (item.isCheck)
                    {
                        listServiceReq.Add(item);
                    }
                }
                if (listServiceReq != null && listServiceReq.Count > 0)
                {
                    PopupMenuProcessorCheck popupMenuProcessorCheck = new PopupMenuProcessorCheck(barManager1, new MouseRightClick(RightMenuCheck_Click), listServiceReq, loginName, currentRoom, listServiceReq);
                    popupMenuProcessorCheck.InitMenu();
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO != null)
                {
                    serviceReqADO.DeleteCheck = CheckLoginAdmin.IsAdmin(loginName) || (currentRoom != null && serviceReqADO.REQUEST_DEPARTMENT_ID == currentRoom.DEPARTMENT_ID && serviceReqADO.SERVICE_REQ_TYPE_ID == 1);
                    serviceReqADO.AddInforPTTT = serviceReqADO.SERVICE_REQ_STT_ID != 3 && currentRoom != null && serviceReqADO.EXECUTE_DEPARTMENT_ID == currentRoom.DEPARTMENT_ID && (serviceReqADO.SERVICE_REQ_TYPE_ID == 10 || serviceReqADO.SERVICE_REQ_TYPE_ID == 4);
                    PrintPopupMenuProcessor = new PrintPopupMenuProcessor(new PrintMedicine_Click(RightMenu_Click), barManager1, serviceReqADO, loginName, currentRoom);
                    PrintPopupMenuProcessor.currentDepartmentId = ((currentRoom != null) ? currentRoom.DEPARTMENT_ID : 0);
                    PrintPopupMenuProcessor.RightMenu();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void RightMenuCheck_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e.Item is BarButtonItem)
                {
                    switch ((PopupMenuProcessorCheck.ItemType)e.Item.Tag)
                    {
                        case PopupMenuProcessorCheck.ItemType.In:
                            ProcessDataCheckedToPrint();
                            break;
                        case PopupMenuProcessorCheck.ItemType.Xoa:
                            ProcessDataCheckedToDelete();
                            break;
                        case PopupMenuProcessorCheck.ItemType.ChuyenPhong:
                            ProcessDataCheckedToChangeRoom();
                            break;
                        case PopupMenuProcessorCheck.ItemType.InKemKetQua:
                            ProcessDataInKemKetQua();
                            break;
                        case PopupMenuProcessorCheck.ItemType.KetQuaHeThongBenhAnhDienTu:
                            PrintKetQuaHeThongBenhAnhDienTu();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessDataInKemKetQua()
        {
            //IL_0240: Unknown result type (might be due to invalid IL or missing references)
            //IL_0247: Expected O, but got Unknown
            //IL_0255: Unknown result type (might be due to invalid IL or missing references)
            //IL_025f: Expected O, but got Unknown
            try
            {
                try
                {
                    if (listServiceReq == null || listServiceReq.Count <= 0)
                    {
                        return;
                    }
                    foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in listServiceReq)
                    {
                        if (item == null || item.IS_NO_EXECUTE == 1)
                        {
                            continue;
                        }
                        currentServiceReqPrint = new HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO();
                        ExecuteBefPrint(item);
                        if (item.SERVICE_REQ_STT_ID != 3)
                        {
                            continue;
                        }
                        currentServiceReqPrint = item;
                        serviceReqPrintRaw = GetServiceReqForPrint(item.ID);
                        WaitingManager.Hide();
                        if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 3 || currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 13 || currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 8 || currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 9 || currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 5)
                        {
                            HisSereServExtFilter hisSereServExtFilter = new HisSereServExtFilter();
                            hisSereServExtFilter.TDL_SERVICE_REQ_ID = currentServiceReqPrint.ID;
                            List<HIS_SERE_SERV_EXT> list = new BackendAdapter(new CommonParam()).Get<List<HIS_SERE_SERV_EXT>>("api/HisSereServExt/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServExtFilter, new Action(SessionManager.ActionLostToken), null);
                            if (list != null && list.Count > 0)
                            {
                                sereServExtPrint = list.FirstOrDefault();
                                SAR_PRINT listPrintByDescriptionPrint = GetListPrintByDescriptionPrint(sereServExtPrint);
                                if (listPrintByDescriptionPrint != null && listPrintByDescriptionPrint.ID > 0)
                                {
                                    LoadTreatmentWithPaty();
                                    ProcessDicParamForPrint();
                                    if (HisConfigs.Get<string>("HIS.Desktop.Plugins.ServiceExecute.PrintOption") == "1")
                                    {
                                        PrintOption1(false, TextLibHelper.BytesToStringConverted(listPrintByDescriptionPrint.CONTENT));
                                    }
                                    else
                                    {
                                        PrintOption2(false, TextLibHelper.BytesToStringConverted(listPrintByDescriptionPrint.CONTENT));
                                    }
                                }
                            }
                        }
                        else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 2)
                        {
                            RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                            val.RunPrintTemplate("Mps000014", new DelegateRunPrinter(InKetQuaXetNghiem));
                        }
                        WaitingManager.Hide();
                    }
                }
                catch (Exception ex)
                {
                    LogSystem.Error(ex);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadTreatmentWithPaty()
        {
            try
            {
                LogSystem.Info("1. Begin LoadTreatmentWithPaty");
                CommonParam commonParam = new CommonParam();
                HisTreatmentWithPatientTypeInfoFilter hisTreatmentWithPatientTypeInfoFilter = new HisTreatmentWithPatientTypeInfoFilter();
                hisTreatmentWithPatientTypeInfoFilter.TREATMENT_ID = currentServiceReqPrint.TREATMENT_ID;
                hisTreatmentWithPatientTypeInfoFilter.INTRUCTION_TIME = currentServiceReqPrint.INTRUCTION_TIME;
                List<HisTreatmentWithPatientTypeInfoSDO> list = new BackendAdapter(commonParam).Get<List<HisTreatmentWithPatientTypeInfoSDO>>("api/HisTreatment/GetTreatmentWithPatientTypeInfoSdo", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentWithPatientTypeInfoFilter, new Action(SessionManager.ActionLostToken), commonParam);
                if (list != null && list.Count > 0)
                {
                    TreatmentWithPatientTypeAlter = list.FirstOrDefault();
                }
                LogSystem.Info("1. End LoadTreatmentWithPaty");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal static string GetCurrentTimeSeparateBeginTime(DateTime now)
        {
            string result = "";
            try
            {
                if (now != DateTime.MinValue)
                {
                    string text = string.Format("{0:00}", now.Month);
                    string text2 = string.Format("{0:00}", now.Day);
                    string text3 = string.Format("{0:00}", now.Hour);
                    string text4 = string.Format("{0:00}", now.Hour);
                    string text5 = string.Format("{0:00}", now.Minute);
                    string text6 = "ngày";
                    string text7 = "tháng";
                    string text8 = "năm";
                    result = string.Format("{0}:{1} " + text6 + " {2} " + text7 + " {3} " + text8 + " {4}", text4, text5, now.Day, now.Month, now.Year);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        internal static string GetCurrentTimeSeparateBeginTime(long time)
        {
            string result = "";
            try
            {
                if (time > 0)
                {
                    string text = time.ToString();
                    string text2 = string.Format("{0:00}", text.Substring(0, 4));
                    string text3 = string.Format("{0:00}", text.Substring(4, 2));
                    string text4 = string.Format("{0:00}", text.Substring(6, 2));
                    string text5 = string.Format("{0:00}", text.Substring(8, 2));
                    string text6 = string.Format("{0:00}", text.Substring(10, 2));
                    result = string.Format("{0} giờ {1} phút ngày {2} tháng {3} năm {4}", text5, text6, text4, text3, text2);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string CalculatorAge(long ageYearNumber, bool isHl7)
        {
            string result = "";
            try
            {
                string text = "Tuổi";
                string text2 = "Tháng tuổi";
                string text3 = "Ngày tuổi";
                string text4 = "Giờ tuổi";
                if (isHl7)
                {
                    text = "T";
                    text2 = "TH";
                    text3 = "NT";
                    text4 = "GT";
                }
                if (ageYearNumber > 0)
                {
                    DateTime value = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageYearNumber).Value;
                    if (value == DateTime.MinValue)
                    {
                        throw new ArgumentNullException("dtNgSinh");
                    }
                    TimeSpan timeSpan = DateTime.Now - value;
                    TimeSpan timeSpan2 = DateTime.Now.Date - value.Date;
                    double totalHours = timeSpan.TotalHours;
                    if (totalHours < 24.0)
                    {
                        result = (int)totalHours + " " + text4;
                    }
                    else
                    {
                        long ticks = timeSpan.Ticks;
                        DateTime dateTime = new DateTime(ticks);
                        if ((dateTime.Year - 1) * 12 + dateTime.Month - 1 == 0)
                        {
                            result = (int)timeSpan2.TotalDays + " " + text3;
                        }
                        else
                        {
                            long ticks2 = timeSpan2.Ticks;
                            DateTime dateTime2 = new DateTime(ticks2);
                            int num = (dateTime2.Year - 1) * 12 + dateTime2.Month - 1;
                            if (num == 0)
                            {
                                result = (int)timeSpan2.TotalDays + " " + text3;
                            }
                            else if (num < 72)
                            {
                                result = num + " " + text2;
                            }
                            else
                            {
                                int num2 = DateTime.Now.Year - value.Year;
                                result = num2 + " " + text;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private void AddKeyIntoDictionaryPrint<T>(T data, Dictionary<string, object> dicParamPlus, bool autoOveride)
        {
            try
            {
                if (data == null)
                {
                    return;
                }
                PropertyInfo[] properties = typeof(T).GetProperties();
                if (properties == null || properties.Length <= 0)
                {
                    return;
                }
                PropertyInfo[] array = properties;
                foreach (PropertyInfo pi in array)
                {
                    if (!pi.GetGetMethod().IsVirtual)
                    {
                        if (string.IsNullOrEmpty(dicParamPlus.SingleOrDefault((KeyValuePair<string, object> o) => o.Key == pi.Name).Key))
                        {
                            dicParamPlus.Add(pi.Name, pi.GetValue(data));
                        }
                        else if (autoOveride)
                        {
                            dicParamPlus[pi.Name] = pi.GetValue(data);
                        }
                        else if (dicParamPlus[pi.Name] == null)
                        {
                            dicParamPlus[pi.Name] = pi.GetValue(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void ProcessDicParamForPrint()
        {
            try
            {
                ProcessDicParam();
                V_HIS_SERVICE service = BackendDataWorker.Get<V_HIS_SERVICE>().FirstOrDefault((V_HIS_SERVICE o) => o.ID == sereServ.SERVICE_ID);
                if (service.PARENT_ID.HasValue)
                {
                    V_HIS_SERVICE v_HIS_SERVICE = BackendDataWorker.Get<V_HIS_SERVICE>().FirstOrDefault((V_HIS_SERVICE o) => o.ID == service.PARENT_ID);
                    if (v_HIS_SERVICE != null)
                    {
                        dicParam.Add("SERVICE_CODE_PARENT", v_HIS_SERVICE.SERVICE_CODE);
                        dicParam.Add("SERVICE_NAME_PARENT", v_HIS_SERVICE.SERVICE_NAME);
                        dicParam.Add("HEIN_SERVICE_BHYT_CODE_PARENT", v_HIS_SERVICE.HEIN_SERVICE_BHYT_CODE);
                        dicParam.Add("HEIN_SERVICE_BHYT_NAME_PARENT", v_HIS_SERVICE.HEIN_SERVICE_BHYT_NAME);
                    }
                }
                dicParam["IS_COPY"] = "BẢN SAO";
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessDicParam()
        {
            try
            {
                dicParam = new Dictionary<string, object>();
                dicImage = new Dictionary<string, System.Drawing.Image>();
                SetCommonKey.SetCommonSingleKey(dicParam);
                if (currentServiceReqPrint != null)
                {
                    dicParam.Add("INTRUCTION_TIME_FULL_STR", GetCurrentTimeSeparateBeginTime(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(currentServiceReqPrint.INTRUCTION_TIME) ?? DateTime.Now));
                    dicParam.Add("INTRUCTION_DATE_FULL_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(currentServiceReqPrint.INTRUCTION_TIME));
                    dicParam.Add("INTRUCTION_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(currentServiceReqPrint.INTRUCTION_TIME));
                    dicParam.Add("START_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(currentServiceReqPrint.START_TIME ?? 0));
                    dicParam.Add("START_TIME_FULL_STR", GetCurrentTimeSeparateBeginTime(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(currentServiceReqPrint.START_TIME ?? 0) ?? DateTime.Now));
                    dicParam.Add("ICD_MAIN_TEXT", currentServiceReqPrint.ICD_NAME);
                    dicParam.Add("NATIONAL_NAME", currentServiceReqPrint.TDL_PATIENT_NATIONAL_NAME);
                    dicParam.Add("WORK_PLACE", currentServiceReqPrint.TDL_PATIENT_WORK_PLACE_NAME);
                    dicParam.Add("ADDRESS", currentServiceReqPrint.TDL_PATIENT_ADDRESS);
                    dicParam.Add("CAREER_NAME", currentServiceReqPrint.TDL_PATIENT_CAREER_NAME);
                    dicParam.Add("PATIENT_CODE", currentServiceReqPrint.TDL_PATIENT_CODE);
                    dicParam.Add("DISTRICT_CODE", currentServiceReqPrint.TDL_PATIENT_DISTRICT_CODE);
                    dicParam.Add("GENDER_NAME", currentServiceReqPrint.TDL_PATIENT_GENDER_NAME);
                    dicParam.Add("MILITARY_RANK_NAME", currentServiceReqPrint.TDL_PATIENT_MILITARY_RANK_NAME);
                    dicParam.Add("VIR_ADDRESS", currentServiceReqPrint.TDL_PATIENT_ADDRESS);
                    dicParam.Add("AGE", CalculatorAge(currentServiceReqPrint.TDL_PATIENT_DOB, false));
                    dicParam.Add("STR_YEAR", currentServiceReqPrint.TDL_PATIENT_DOB.ToString().Substring(0, 4));
                    dicParam.Add("VIR_PATIENT_NAME", currentServiceReqPrint.TDL_PATIENT_NAME);
                    V_HIS_ROOM v_HIS_ROOM = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == currentServiceReqPrint.EXECUTE_ROOM_ID);
                    if (v_HIS_ROOM != null)
                    {
                        dicParam.Add("EXECUTE_DEPARTMENT_CODE", v_HIS_ROOM.DEPARTMENT_CODE);
                        dicParam.Add("EXECUTE_DEPARTMENT_NAME", v_HIS_ROOM.DEPARTMENT_NAME);
                        dicParam.Add("EXECUTE_ROOM_CODE", v_HIS_ROOM.ROOM_CODE);
                        dicParam.Add("EXECUTE_ROOM_NAME", v_HIS_ROOM.ROOM_NAME);
                    }
                    V_HIS_ROOM v_HIS_ROOM2 = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == currentServiceReqPrint.REQUEST_ROOM_ID);
                    if (v_HIS_ROOM2 != null)
                    {
                        dicParam.Add("REQUEST_DEPARTMENT_CODE", v_HIS_ROOM2.DEPARTMENT_CODE);
                        dicParam.Add("REQUEST_DEPARTMENT_NAME", v_HIS_ROOM2.DEPARTMENT_NAME);
                        dicParam.Add("REQUEST_ROOM_CODE", v_HIS_ROOM2.ROOM_CODE);
                        dicParam.Add("REQUEST_ROOM_NAME", v_HIS_ROOM2.ROOM_NAME);
                    }
                }
                if (TreatmentWithPatientTypeAlter != null)
                {
                    if (!string.IsNullOrEmpty(TreatmentWithPatientTypeAlter.HEIN_CARD_NUMBER))
                    {
                        dicParam.Add("HEIN_CARD_NUMBER_SEPARATE", HeinCardHelper.SetHeinCardNumberDisplayByNumber(TreatmentWithPatientTypeAlter.HEIN_CARD_NUMBER));
                        dicParam.Add("STR_HEIN_CARD_FROM_TIME", Inventec.Common.DateTime.Convert.TimeNumberToDateString(TreatmentWithPatientTypeAlter.HEIN_CARD_FROM_TIME));
                        dicParam.Add("STR_HEIN_CARD_TO_TIME", Inventec.Common.DateTime.Convert.TimeNumberToDateString(TreatmentWithPatientTypeAlter.HEIN_CARD_TO_TIME));
                        dicParam.Add("HEIN_CARD_ADDRESS", TreatmentWithPatientTypeAlter.HEIN_CARD_ADDRESS);
                    }
                    else
                    {
                        dicParam.Add("HEIN_CARD_NUMBER_SEPARATE", "");
                        dicParam.Add("STR_HEIN_CARD_FROM_TIME", "");
                        dicParam.Add("STR_HEIN_CARD_TO_TIME", "");
                        dicParam.Add("HEIN_CARD_ADDRESS", "");
                    }
                    HIS_PATIENT_TYPE hIS_PATIENT_TYPE = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.PATIENT_TYPE_CODE == TreatmentWithPatientTypeAlter.PATIENT_TYPE_CODE);
                    if (hIS_PATIENT_TYPE != null)
                    {
                        dicParam.Add("PATIENT_TYPE_NAME", hIS_PATIENT_TYPE.PATIENT_TYPE_NAME);
                    }
                    else
                    {
                        dicParam.Add("PATIENT_TYPE_NAME", "");
                    }
                    HIS_TREATMENT_TYPE hIS_TREATMENT_TYPE = BackendDataWorker.Get<HIS_TREATMENT_TYPE>().FirstOrDefault((HIS_TREATMENT_TYPE o) => o.TREATMENT_TYPE_CODE == TreatmentWithPatientTypeAlter.TREATMENT_TYPE_CODE);
                    if (hIS_TREATMENT_TYPE != null)
                    {
                        dicParam.Add("TREATMENT_TYPE_NAME", hIS_TREATMENT_TYPE.TREATMENT_TYPE_NAME);
                    }
                    else
                    {
                        dicParam.Add("TREATMENT_TYPE_NAME", "");
                    }
                    dicParam.Add("TREATMENT_ICD_CODE", TreatmentWithPatientTypeAlter.ICD_CODE);
                    dicParam.Add("TREATMENT_ICD_NAME", TreatmentWithPatientTypeAlter.ICD_NAME);
                    dicParam.Add("TREATMENT_ICD_SUB_CODE", TreatmentWithPatientTypeAlter.ICD_SUB_CODE);
                    dicParam.Add("TREATMENT_ICD_TEXT", TreatmentWithPatientTypeAlter.ICD_TEXT);
                    AddKeyIntoDictionaryPrint(TreatmentWithPatientTypeAlter, dicParam, false);
                    int num = Calculation.Age(TreatmentWithPatientTypeAlter.TDL_PATIENT_DOB, TreatmentWithPatientTypeAlter.IN_TIME);
                    dicParam.Add("AGE_NUM", num);
                }
                else
                {
                    dicParam.Add("HEIN_CARD_NUMBER_SEPARATE", "");
                    dicParam.Add("STR_HEIN_CARD_FROM_TIME", "");
                    dicParam.Add("STR_HEIN_CARD_TO_TIME", "");
                    dicParam.Add("HEIN_CARD_ADDRESS", "");
                    HisTreatmentWithPatientTypeInfoSDO data = new HisTreatmentWithPatientTypeInfoSDO();
                    AddKeyIntoDictionaryPrint(data, dicParam, false);
                }
                CommonParam commonParam = new CommonParam();
                HisSereServView4Filter hisSereServView4Filter = new HisSereServView4Filter();
                hisSereServView4Filter.ID = sereServExtPrint.SERE_SERV_ID;
                List<V_HIS_SERE_SERV_4> list = new BackendAdapter(commonParam).Get<List<V_HIS_SERE_SERV_4>>("api/HisSereServ/GetView4", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServView4Filter, commonParam);
                if (list != null && list.Count > 0)
                {
                    sereServ = list[0];
                }
                HIS_SERE_SERV hIS_SERE_SERV = new HIS_SERE_SERV();
                DataObjectMapper.Map<HIS_SERE_SERV>(hIS_SERE_SERV, sereServ);
                AddKeyIntoDictionaryPrint((HIS_SERVICE_REQ)currentServiceReq, dicParam, true);
                AddKeyIntoDictionaryPrint(hIS_SERE_SERV, dicParam, false);
                AddKeyIntoDictionaryPrint(sereServExtPrint, dicParam, true);
                if (sereServExtPrint != null)
                {
                    if (!dicParam.ContainsKey("END_TIME_FULL_STR"))
                    {
                        dicParam.Add("END_TIME_FULL_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServExtPrint.END_TIME ?? 0));
                    }
                    else
                    {
                        dicParam["END_TIME_FULL_STR"] = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServExtPrint.END_TIME ?? 0);
                    }
                    if (!dicParam.ContainsKey("BEGIN_TIME_FULL_STR"))
                    {
                        dicParam.Add("BEGIN_TIME_FULL_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServExtPrint.BEGIN_TIME ?? 0));
                    }
                    else
                    {
                        dicParam["BEGIN_TIME_FULL_STR"] = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServExtPrint.BEGIN_TIME ?? 0);
                    }
                    if (sereServExtPrint.MACHINE_ID.HasValue)
                    {
                        HIS_MACHINE hIS_MACHINE = BackendDataWorker.Get<HIS_MACHINE>().FirstOrDefault((HIS_MACHINE o) => o.ID == sereServExtPrint.MACHINE_ID.Value);
                        if (hIS_MACHINE != null)
                        {
                            dicParam["MACHINE_NAME"] = hIS_MACHINE.MACHINE_NAME;
                        }
                    }
                    if (sereServExtPrint.END_TIME.HasValue)
                    {
                        dicParam["EXECUTE_DATE_FULL_STR"] = Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(sereServExtPrint.END_TIME.Value);
                        dicParam["EXECUTE_TIME_FULL_STR"] = GetCurrentTimeSeparateBeginTime(sereServExtPrint.END_TIME.Value);
                    }
                    else if (sereServExtPrint.MODIFY_TIME.HasValue)
                    {
                        dicParam["EXECUTE_DATE_FULL_STR"] = Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(sereServExtPrint.MODIFY_TIME.Value);
                        dicParam["EXECUTE_TIME_FULL_STR"] = GetCurrentTimeSeparateBeginTime(sereServExtPrint.MODIFY_TIME.Value);
                    }
                    else
                    {
                        dicParam["EXECUTE_DATE_FULL_STR"] = "";
                        dicParam["EXECUTE_TIME_FULL_STR"] = "";
                    }
                }
                else
                {
                    dicParam["EXECUTE_DATE_FULL_STR"] = "";
                    dicParam["EXECUTE_TIME_FULL_STR"] = "";
                    dicParam["MACHINE_NAME"] = "";
                }
                dicParam.Add("USER_NAME", ClientTokenManagerStore.ClientTokenManager.GetUserName());
                foreach (string item in keyPrint)
                {
                    dicParam.Remove(item);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintOption1(bool printNow, string content)
        {
            //IL_0063: Unknown result type (might be due to invalid IL or missing references)
            //IL_0069: Expected O, but got Unknown
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(content))
                {
                    dictionary["DESCRIPTION_WORD"] = content;
                    SAR_PRINT_TYPE sAR_PRINT_TYPE = BackendDataWorker.Get<SAR_PRINT_TYPE>().FirstOrDefault((SAR_PRINT_TYPE o) => o.PRINT_TYPE_CODE == "Mps000354");
                    RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), PrintStoreLocation.PrintTemplatePath);
                    InputADO inputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((TreatmentWithPatientTypeAlter != null) ? TreatmentWithPatientTypeAlter.TREATMENT_CODE : "", "Mps000354", (currentModule != null) ? currentModule.RoomId : 0);
                    val.RunPrintTemplate(sAR_PRINT_TYPE.PRINT_TYPE_CODE, sAR_PRINT_TYPE.FILE_PATTERN, sereServ.TDL_SERVICE_NAME, (DelegateUpdateTableReference)null, (DelegateGetListFromPrintReference)null, dicParam, dicImage, inputADO, dictionary, printNow);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintOption2(bool printNow, string content)
        {
            try
            {
                if (!string.IsNullOrEmpty(content))
                {
                    RichEditControl richEditControl = ProcessDocumentBeforePrint(content);
                    if (richEditControl == null)
                    {
                        LogSystem.Error("printDocument is null");
                    }
                    else if (printNow)
                    {
                        richEditControl.Print();
                    }
                    else if (ConfigApplications.CheDoInChoCacChucNangTrongPhanMem == 2)
                    {
                        richEditControl.Print();
                    }
                    else
                    {
                        richEditControl.ShowPrintPreview();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private RichEditControl ProcessDocumentBeforePrint(string document)
        {
            RichEditControl richEditControl = null;
            try
            {
                if (document != null)
                {
                    richEditControl = new RichEditControl();
                    HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
                    hisServiceReqFilter.ID = sereServ.SERVICE_REQ_ID;
                    List<HIS_SERVICE_REQ> list = new BackendAdapter(new CommonParam()).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqFilter, new Action(SessionManager.ActionLostToken), null);
                    long? num = null;
                    if (list != null && list.Count > 0)
                    {
                        num = list.FirstOrDefault().FINISH_TIME;
                    }
                    richEditControl.RtfText = document;
                    if (string.IsNullOrEmpty(richEditControl.Text))
                    {
                        return null;
                    }
                    string text = HisConfigs.Get<string>("HIS.Desktop.Plugins.ServiceExecute.ThoiGianKetThuc");
                    string text2 = HisConfigs.Get<string>("HIS.Desktop.Plugins.ServiceExecute.HideTimePrint");
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        foreach (DevExpress.XtraRichEdit.API.Native.Section section in richEditControl.Document.Sections)
                        {
                            if (text2 != "1")
                            {
                                section.Margins.HeaderOffset = 50f;
                                section.Margins.FooterOffset = 50f;
                                SubDocument subDocument = section.BeginUpdateHeader(HeaderFooterType.Odd);
                                subDocument.Delete(subDocument.Range);
                                subDocument.InsertText(subDocument.CreatePosition(0), string.Format(Inventec.Common.Resource.Get.Value("NgayIn", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                                subDocument.Fields.Update();
                                section.EndUpdateHeader(subDocument);
                            }
                            string text3 = "";
                            if (num.HasValue)
                            {
                                text3 = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(num.Value);
                            }
                            DocumentRange[] array = richEditControl.Document.FindAll(text, SearchOptions.CaseSensitive);
                            if (array != null && array.Length > 0)
                            {
                                for (int i = 0; i < array.Length; i++)
                                {
                                    richEditControl.Document.Replace(array[i], text3);
                                }
                            }
                        }
                    }
                    if (sereServExtPrint != null)
                    {
                        foreach (string item in keyPrint)
                        {
                            DocumentRange[] array = richEditControl.Document.FindAll(item, SearchOptions.CaseSensitive);
                            DocumentRange[] array2 = array;
                            foreach (DocumentRange range in array2)
                            {
                                CharacterProperties characterProperties = richEditControl.Document.BeginUpdateCharacters(range);
                                characterProperties.ForeColor = Color.Black;
                                richEditControl.Document.EndUpdateCharacters(characterProperties);
                            }
                        }
                        richEditControl.Document.ReplaceAll("<#CONCLUDE_PRINT;>", sereServExtPrint.CONCLUDE, SearchOptions.CaseSensitive);
                        richEditControl.Document.ReplaceAll("<#NOTE_PRINT;>", sereServExtPrint.NOTE, SearchOptions.CaseSensitive);
                        richEditControl.Document.ReplaceAll("<#DESCRIPTION_PRINT;>", sereServExtPrint.DESCRIPTION, SearchOptions.CaseSensitive);
                        richEditControl.Document.ReplaceAll("<#CURRENT_USERNAME_PRINT;>", list.FirstOrDefault().EXECUTE_USERNAME, SearchOptions.CaseSensitive);
                        foreach (KeyValuePair<string, object> item2 in dicParam)
                        {
                            if (item2.Value != null && CheckType(item2.Value))
                            {
                                string current2 = string.Format("<#{0}_PRINT;>", item2.Key);
                                DocumentRange[] array = richEditControl.Document.FindAll(current2, SearchOptions.CaseSensitive);
                                DocumentRange[] array2 = array;
                                foreach (DocumentRange range in array2)
                                {
                                    CharacterProperties characterProperties = richEditControl.Document.BeginUpdateCharacters(range);
                                    characterProperties.ForeColor = Color.Black;
                                    richEditControl.Document.EndUpdateCharacters(characterProperties);
                                }
                                richEditControl.Document.ReplaceAll(current2, item2.Value.ToString(), SearchOptions.CaseSensitive);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                richEditControl = null;
                LogSystem.Error(ex);
            }
            return richEditControl;
        }

        private bool CheckType(object value)
        {
            bool flag = false;
            try
            {
                flag = value.GetType() == typeof(long) || value.GetType() == typeof(int) || value.GetType() == typeof(string) || value.GetType() == typeof(short) || value.GetType() == typeof(decimal) || value.GetType() == typeof(double) || value.GetType() == typeof(float);
            }
            catch (Exception ex)
            {
                flag = false;
                LogSystem.Error(ex);
            }
            return flag;
        }

        private SAR_PRINT GetListPrintByDescriptionPrint(HIS_SERE_SERV_EXT sereServExt)
        {
            SAR_PRINT result = null;
            try
            {
                List<long> listPrintIdBySereServ = GetListPrintIdBySereServ(sereServExt);
                if (listPrintIdBySereServ != null && listPrintIdBySereServ.Count > 0)
                {
                    CommonParam commonParam = new CommonParam();
                    SarPrintFilter sarPrintFilter = new SarPrintFilter();
                    sarPrintFilter.IS_ACTIVE = 1;
                    sarPrintFilter.IDs = listPrintIdBySereServ;
                    List<SAR_PRINT> list = new BackendAdapter(commonParam).Get<List<SAR_PRINT>>("api/SarPrint/Get", HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, sarPrintFilter, commonParam);
                    if (list != null && list.Count > 0)
                    {
                        result = list.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private List<long> GetListPrintIdBySereServ(HIS_SERE_SERV_EXT item)
        {
            List<long> list = new List<long>();
            try
            {
                if (!string.IsNullOrEmpty(item.DESCRIPTION_SAR_PRINT_ID))
                {
                    string[] array = item.DESCRIPTION_SAR_PRINT_ID.Split(',', ';');
                    if (array != null && array.Length > 0)
                    {
                        string[] array2 = array;
                        foreach (string inputValue in array2)
                        {
                            long num = Parse.ToInt64(inputValue);
                            if (num > 0)
                            {
                                list.Add(num);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return list;
        }

        private bool InKetQuaXetNghiem(string printTypeCode, string fileName)
        {
            bool result = false;
            try
            {
                WaitingManager.Show();
                CommonParam commonParam = new CommonParam();
                long tREATMENT_ID = currentServiceReqPrint.TREATMENT_ID;
                HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
                hisServiceReqFilter.ID = currentServiceReqPrint.ID;
                V_HIS_SERVICE_REQ item = new BackendAdapter(commonParam).Get<List<V_HIS_SERVICE_REQ>>("api/HisServiceReq/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqFilter, commonParam).FirstOrDefault();
                HIS_TREATMENT hIS_TREATMENT = new HIS_TREATMENT();
                HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
                hisTreatmentFilter.ID = tREATMENT_ID;
                hIS_TREATMENT = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, commonParam).FirstOrDefault();
                V_HIS_PATIENT_TYPE_ALTER v_HIS_PATIENT_TYPE_ALTER = new V_HIS_PATIENT_TYPE_ALTER();
                HisPatientTypeAlterViewFilter hisPatientTypeAlterViewFilter = new HisPatientTypeAlterViewFilter();
                hisPatientTypeAlterViewFilter.TREATMENT_ID = tREATMENT_ID;
                hisPatientTypeAlterViewFilter.ORDER_FIELD = "LOG_TIME";
                hisPatientTypeAlterViewFilter.ORDER_DIRECTION = "DESC";
                v_HIS_PATIENT_TYPE_ALTER = new BackendAdapter(commonParam).Get<List<V_HIS_PATIENT_TYPE_ALTER>>("/api/HisPatientTypeAlter/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientTypeAlterViewFilter, commonParam).FirstOrDefault();
                decimal ratio_text = 0m;
                HIS_BRANCH hIS_BRANCH = BackendDataWorker.Get<HIS_BRANCH>().FirstOrDefault((HIS_BRANCH o) => o.ID == WorkPlace.GetBranchId());
                string levelCode = ((hIS_BRANCH != null) ? hIS_BRANCH.HEIN_LEVEL_CODE : null);
                if (v_HIS_PATIENT_TYPE_ALTER != null)
                {
                    ratio_text = GetDefaultHeinRatioForView(v_HIS_PATIENT_TYPE_ALTER.HEIN_CARD_NUMBER, v_HIS_PATIENT_TYPE_ALTER.HEIN_TREATMENT_TYPE_CODE, levelCode, v_HIS_PATIENT_TYPE_ALTER.RIGHT_ROUTE_CODE);
                }
                List<object> list = new List<object>();
                list.Add(v_HIS_PATIENT_TYPE_ALTER);
                list.Add(item);
                list.Add(hIS_TREATMENT);
                HisSereServViewFilter hisSereServViewFilter = new HisSereServViewFilter();
                hisSereServViewFilter.SERVICE_REQ_ID = currentServiceReqPrint.ID;
                hisSereServViewFilter.SERVICE_TYPE_ID = 2L;
                List<HIS_SERE_SERV> list2 = new BackendAdapter(commonParam).Get<List<HIS_SERE_SERV>>("/api/HisSereServ/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServViewFilter, commonParam);
                _SereServNumOders = new List<SereServNumOder>();
                if (list2 != null && list2.Count > 0)
                {
                    foreach (HIS_SERE_SERV item4 in list2)
                    {
                        SereServNumOder item2 = new SereServNumOder(item4, BackendDataWorker.Get<V_HIS_SERVICE>());
                        _SereServNumOders.Add(item2);
                    }
                    _SereServNumOders = (from p in _SereServNumOders
                                         orderby p.SERVICE_NUM_ODER descending, p.TDL_SERVICE_NAME
                                         select p).ToList();
                    HisSereServTeinViewFilter hisSereServTeinViewFilter = new HisSereServTeinViewFilter();
                    hisSereServTeinViewFilter.SERE_SERV_IDs = list2.Select((HIS_SERE_SERV o) => o.ID).ToList();
                    hisSereServTeinViewFilter.IS_ACTIVE = 1;
                    lstSereServTein = new BackendAdapter(commonParam).Get<List<V_HIS_SERE_SERV_TEIN>>("api/HisSereServTein/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServTeinViewFilter, commonParam);
                }
                List<V_HIS_SERE_SERV_TEIN> list3 = Mapper.Map<List<V_HIS_SERE_SERV_TEIN>>(lstSereServTein);
                List<SereServNumOder> list4 = new List<SereServNumOder>();
                List<SereServNumOder> list5 = new List<SereServNumOder>();
                Dictionary<long, List<SereServNumOder>> dictionary = new Dictionary<long, List<SereServNumOder>>();
                foreach (SereServNumOder sereServNumOder in _SereServNumOders)
                {
                    if (!sereServNumOder.ServiceParentId.HasValue)
                    {
                        if (!dictionary.ContainsKey(0L))
                        {
                            dictionary[0L] = new List<SereServNumOder>();
                        }
                        dictionary[0L].Add(sereServNumOder);
                    }
                    else
                    {
                        list4.Add(sereServNumOder);
                    }
                }
                foreach (SereServNumOder item3 in list4)
                {
                    List<V_HIS_SERVICE> source = BackendDataWorker.Get<V_HIS_SERVICE>();
                    Func<V_HIS_SERVICE, bool> predicate = (V_HIS_SERVICE o) => o.ID == item3.ServiceParentId;
                    V_HIS_SERVICE v_HIS_SERVICE = source.FirstOrDefault(predicate);
                    if (!v_HIS_SERVICE.PARENT_ID.HasValue)
                    {
                        if (!dictionary.ContainsKey(v_HIS_SERVICE.ID))
                        {
                            dictionary[v_HIS_SERVICE.ID] = new List<SereServNumOder>();
                        }
                        dictionary[v_HIS_SERVICE.ID].Add(item3);
                    }
                    else
                    {
                        list5.Add(item3);
                    }
                }
                foreach (SereServNumOder item5 in list5)
                {
                    if (!dictionary.ContainsKey(item5.GrandParentID.Value))
                    {
                        dictionary[item5.GrandParentID.Value] = new List<SereServNumOder>();
                    }
                    dictionary[item5.GrandParentID.Value].Add(item5);
                }
                foreach (long key in dictionary.Keys)
                {
                    Mps000014PDO data = new Mps000014PDO(list.ToArray(), dictionary[key], list3, ratio_text, BackendDataWorker.Get<V_HIS_TEST_INDEX_RANGE>(), hIS_TREATMENT.TDL_PATIENT_GENDER_ID, BackendDataWorker.Get<V_HIS_SERVICE>());
                    WaitingManager.Hide();
                    PrintData printData = null;
                    printData = ((GlobalVariables.CheDoInChoCacChucNangTrongPhanMem != 2) ? new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.Show, "") : new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, ""));
                    InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((hIS_TREATMENT != null) ? hIS_TREATMENT.TREATMENT_CODE : "", printTypeCode, (currentModule != null) ? currentModule.RoomId : 0);
                    LogSystem.Info(hIS_TREATMENT.TREATMENT_CODE);
                    printData.EmrInputADO = emrInputADO;
                    result = MpsPrinter.Run(printData);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
            return result;
        }

        public decimal GetDefaultHeinRatioForView(string heinCardNumber, string treatmentTypeCode, string levelCode, string rightRouteCode)
        {
            decimal result = 0m;
            try
            {
                result = new BhytHeinProcessor().GetDefaultHeinRatio(treatmentTypeCode, heinCardNumber, levelCode, rightRouteCode) ?? 0m;
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return result;
        }

        private void ProcessDataCheckedToChangeRoom()
        {
            try
            {
                if (listServiceReq == null || listServiceReq.Count <= 0)
                {
                    return;
                }
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = listServiceReq.Where((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.SERVICE_REQ_STT_ID == 3).ToList();
                if (list != null && list.Count > 0)
                {
                    string text = "";
                    List<string> list2 = new List<string>();
                    foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in list)
                    {
                        string sERVICE_REQ_CODE = item.SERVICE_REQ_CODE;
                        list2.Add(sERVICE_REQ_CODE);
                    }
                    text = string.Join(",", list2);
                    if (XtraMessageBox.Show("Chỉ định " + text + " đã kết thúc, không cho phép chuyển phòng", ResourceMessage.ThongBao, MessageBoxButtons.OK) != DialogResult.OK)
                    {
                    }
                }
                else if (listServiceReq.Count == 1)
                {
                    Btn_ChangeRoom_ButtonClick();
                }
                else
                {
                    frmChangeRoom frmChangeRoom2 = new frmChangeRoom(currentModule, listServiceReq);
                    frmChangeRoom2.ShowDialog();
                    btnFind_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessDataCheckedToDelete()
        {
            try
            {
                if (listServiceReq == null || listServiceReq.Count <= 0 || MessageBox.Show(ResourceMessage.HeThongTBCuaSoThongBaoBanCoMuonHuyDuLieuKhong, ResourceMessage.ThongBao, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
                bool flag = false;
                foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in listServiceReq)
                {
                    if (CheckParentBeforeDelete(item.ID))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && XtraMessageBox.Show("Đã có y lệnh đính kèm (CLS). Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                CommonParam commonParam = new CommonParam();
                bool flag2 = true;
                WaitingManager.Show();
                foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item2 in listServiceReq)
                {
                    HisServiceReqSDO hisServiceReqSDO = new HisServiceReqSDO();
                    hisServiceReqSDO.Id = item2.ID;
                    hisServiceReqSDO.RequestRoomId = currentModule.RoomId;
                    flag2 = flag2 && new BackendAdapter(commonParam).Post<bool>("api/HisServiceReq/Delete", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqSDO, new Action(SessionManager.ActionLostToken), commonParam);
                }
                FillDataToGrid();
                WaitingManager.Hide();
                MessageManager.Show(this, commonParam, flag2);
                SessionManager.ProcessTokenLost(commonParam);
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void ProcessDataCheckedToPrint()
        {
            try
            {
                if (listServiceReq == null || listServiceReq.Count <= 0)
                {
                    return;
                }
                foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in listServiceReq)
                {
                    ExecuteBefPrint(item);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridViewServiceReq_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridView gridView = sender as GridView;
                GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.Location);
                if (gridHitInfo.HitTest == GridHitTest.Column && gridHitInfo.Column.FieldName == "isCheck")
                {
                    List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = gridControlServiceReq.DataSource as List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>;
                    if (list != null && list.Count > 0)
                    {
                        gridViewServiceReq.BeginUpdate();
                        if (isCheckAll)
                        {
                            isCheckAll = false;
                            foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in list)
                            {
                                item.isCheck = true;
                            }
                            gridColumn_ServiceReq_Choose.Image = imageListCheck.Images[3];
                        }
                        else
                        {
                            isCheckAll = true;
                            foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item2 in list)
                            {
                                item2.isCheck = false;
                            }
                            gridColumn_ServiceReq_Choose.Image = imageListCheck.Images[4];
                        }
                        gridViewServiceReq.EndUpdate();
                    }
                }
                if (gridHitInfo.InRowCell)
                {
                    if (gridHitInfo.Column.FieldName == "SERVICE_REQ_CODE" || gridHitInfo.Column.FieldName == "TDL_TREATMENT_CODE" || gridHitInfo.Column.FieldName == "TDL_PATIENT_CODE" || gridHitInfo.Column.FieldName == "TDL_PATIENT_NAME")
                    {
                        gridViewServiceReq.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDownFocused;
                    }
                    else
                    {
                        gridViewServiceReq.OptionsBehavior.EditorShowMode = EditorShowMode.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void grdViewSereServServiceReq_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                if (!e.IsGetData || e.Column.UnboundType == UnboundColumnType.Bound)
                {
                    return;
                }
                ListMedicineADO listMedicineADO = (ListMedicineADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                if (listMedicineADO == null)
                {
                    return;
                }
                if (e.Column.FieldName == "STT")
                {
                    e.Value = e.ListSourceRowIndex + 1;
                }
                if (e.Column.FieldName == "IS_ACCEPTING_NO_EXECUTE_STR")
                {
                    if (((HIS_SERE_SERV)listMedicineADO).IS_ACCEPTING_NO_EXECUTE == 1)
                    {
                        e.Value = "true";
                    }
                }
                else if (e.Column.FieldName == "PRES_AMOUNT_DISPLAY")
                {
                    e.Value = listMedicineADO.PRES_AMOUNT.ToString();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void grdViewSereServServiceReq_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            try
            {
                rightClickData = null;
                GridHitInfo hitInfo = e.HitInfo;
                if (hitInfo.InRowCell)
                {
                    int visibleRowHandle = grdViewSereServServiceReq.GetVisibleRowHandle(hitInfo.RowHandle);
                    ListMedicineADO listMedicineADO = (ListMedicineADO)grdViewSereServServiceReq.GetRow(visibleRowHandle);
                    if (listMedicineADO != null)
                    {
                        rightClickData = listMedicineADO;
                        BarManager barManager = new BarManager();
                        barManager.Form = this;
                        PopupMenuProcessorMedicine popupMenuProcessorMedicine = new PopupMenuProcessorMedicine(listMedicineADO, barManager, new MouseRightClick(MouseRight_Click), currentWorkPlace);
                        popupMenuProcessorMedicine.InitMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void grdViewSereServServiceReq_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                GridView gridView = sender as GridView;
                if (e.RowHandle < 0)
                {
                    return;
                }
                int num = Parse.ToInt32((gridView.GetRowCellValue(e.RowHandle, "subPress") ?? "0").ToString());
                int num2 = Parse.ToInt32((gridView.GetRowCellValue(e.RowHandle, "kind") ?? "").ToString());
                if (num == 1 && num2 == 1)
                {
                    e.Appearance.FontStyleDelta = FontStyle.Italic;
                    string value = (gridView.GetRowCellValue(e.RowHandle, "isStartMark") ?? "").ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        e.Appearance.FontStyleDelta = FontStyle.Bold | FontStyle.Italic;
                    }
                }
                else if (num2 == 1)
                {
                    e.Appearance.ForeColor = Color.Green;
                }
                short num3 = Parse.ToInt16((gridView.GetRowCellValue(e.RowHandle, "IS_NO_EXECUTE") ?? "").ToString());
                if (num3 == 1)
                {
                    e.Appearance.Font = new System.Drawing.Font(e.Appearance.Font, FontStyle.Strikeout);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void grdViewSereServServiceReq_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {
            try
            {
                GridGroupRowInfo gridGroupRowInfo = e.Info as GridGroupRowInfo;
                string groupText = System.Convert.ToString(grdViewSereServServiceReq.GetGroupRowValue(e.RowHandle, GridColumnInReqExeute));
                gridGroupRowInfo.GroupText = groupText;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void grdViewSereServServiceReq_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                ListMedicineADO data = (ListMedicineADO)grdViewSereServServiceReq.GetRow(e.RowHandle);
                HIS_EXECUTE_ROOM hIS_EXECUTE_ROOM = BackendDataWorker.Get<HIS_EXECUTE_ROOM>().FirstOrDefault((HIS_EXECUTE_ROOM o) => o.ROOM_ID == data.TDL_EXECUTE_ROOM_ID);
                string text = (grdViewSereServServiceReq.GetRowCellValue(e.RowHandle, "CREATOR") ?? "").ToString().Trim();
                long num = long.Parse((grdViewSereServServiceReq.GetRowCellValue(e.RowHandle, "TDL_SERVICE_TYPE_ID") ?? "").ToString());
                if (e.Column.FieldName == "btnView_Tab")
                {
                    if (!serviceReqADO.EXE_SERVICE_MODULE_ID.HasValue || (serviceReqADO.EXE_SERVICE_MODULE_ID != 1 && serviceReqADO.EXE_SERVICE_MODULE_ID != 4 && serviceReqADO.EXE_SERVICE_MODULE_ID != 7 && serviceReqADO.EXE_SERVICE_MODULE_ID != 3 && serviceReqADO.EXE_SERVICE_MODULE_ID != 5 && serviceReqADO.EXE_SERVICE_MODULE_ID != 2) || (HisConfigCFG.ShowResultWhenReqComplete == "1" && serviceReqADO.SERVICE_REQ_STT_ID != 3) || (HisConfigCFG.ShowResultWhenReqComplete == "2" && serviceReqADO.SERVICE_REQ_TYPE_ID == 2 && serviceReqADO.SERVICE_REQ_STT_ID != 3))
                    {
                        e.RepositoryItem = repositoryItemTextEditDisable;
                    }
                    else
                    {
                        e.RepositoryItem = repositoryItemButtonView;
                    }
                }
                else if (e.Column.FieldName == "IS_ACCEPTING_NO_EXECUTE_STR" && e.CellValue == "true")
                {
                    e.RepositoryItem = repositoryItemButtonIsAcceptNoExecute;
                }
                if (e.Column.FieldName == "btnPrint_Tab")
                {
                    if (num == 11 || num == 4 || num == 13)
                    {
                        e.RepositoryItem = repositoryItemButtonPrint;
                    }
                    else
                    {
                        e.RepositoryItem = repositoryItemTextEditDisable;
                    }
                }
                else if (e.Column.FieldName == "SereSerDeleteDQ")
                {
                    if ((text == loginName || CheckLoginAdmin.IsAdmin(loginName)) && data.TDL_SERVICE_TYPE_ID == 1 && currentServiceReq.SERVICE_REQ_STT_ID != 3 && hIS_EXECUTE_ROOM.ALLOW_NOT_CHOOSE_SERVICE == 1)
                    {
                        e.RepositoryItem = repositoryItemButtonEditDeleteEna;
                    }
                    else
                    {
                        e.RepositoryItem = repositoryItemButtonEditDeleteDis;
                    }
                }
                if (e.Column.FieldName == "IS_CONFIRM_NO_EXCUTE")
                {
                    if (((HIS_SERE_SERV)data).IS_CONFIRM_NO_EXCUTE == 1)
                    {
                        e.RepositoryItem = repositoryItemButtonEditServiceConfirmEna;
                    }
                    else
                    {
                        e.RepositoryItem = repositoryItemButtonEditServiceConfirmDis;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void cboServiceReqStt_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                GridCheckMarksSelection gridCheckMarksSelection = ((sender is GridLookUpEdit) ? ((sender as GridLookUpEdit).Properties.Tag as GridCheckMarksSelection) : ((sender as RepositoryItemGridLookUpEdit).Tag as GridCheckMarksSelection));
                if (gridCheckMarksSelection == null)
                {
                    return;
                }
                foreach (HIS_SERVICE_REQ_STT item in gridCheckMarksSelection.Selection)
                {
                    if (stringBuilder.ToString().Length > 0)
                    {
                        stringBuilder.Append(", ");
                    }
                    stringBuilder.Append(item.SERVICE_REQ_STT_NAME.ToString());
                }
                e.DisplayText = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void cboServiceReqStt_Closed(object sender, ClosedEventArgs e)
        {
            try
            {
                if (e.CloseMode == PopupCloseMode.Normal || e.CloseMode == PopupCloseMode.Immediate)
                {
                    cboServiceReqType.Focus();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void cboServiceReqType_Closed(object sender, ClosedEventArgs e)
        {
            try
            {
                if (e.CloseMode == PopupCloseMode.Normal || e.CloseMode == PopupCloseMode.Immediate)
                {
                    btnFind.Focus();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridLookUpEdit1View_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.F)
                {
                    FillDataToGrid();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.F)
                {
                    FillDataToGrid();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void cboServiceReqType_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                GridCheckMarksSelection gridCheckMarksSelection = ((sender is GridLookUpEdit) ? ((sender as GridLookUpEdit).Properties.Tag as GridCheckMarksSelection) : ((sender as RepositoryItemGridLookUpEdit).Tag as GridCheckMarksSelection));
                if (gridCheckMarksSelection == null)
                {
                    return;
                }
                foreach (HIS_SERVICE_REQ_TYPE item in gridCheckMarksSelection.Selection)
                {
                    if (stringBuilder.ToString().Length > 0)
                    {
                        stringBuilder.Append(", ");
                    }
                    stringBuilder.Append(item.SERVICE_REQ_TYPE_NAME.ToString());
                }
                e.DisplayText = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void repositoryItemCheckEditChoose_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO == null)
                {
                    return;
                }
                serviceReqADO.isCheck = !serviceReqADO.isCheck;
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = gridControlServiceReq.DataSource as List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>;
                bool flag = false;
                bool flag2 = false;
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list2 = list.Where((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.isCheck).ToList();
                if (list2 != null && list2.Count > 0)
                {
                    if (list2.Count == list.Count)
                    {
                        flag2 = true;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                if (flag2)
                {
                    isCheckAll = false;
                    gridColumn_ServiceReq_Choose.Image = imageListCheck.Images[3];
                }
                else if (flag)
                {
                    gridColumn_ServiceReq_Choose.Image = imageListCheck.Images[5];
                }
                else
                {
                    isCheckAll = true;
                    gridColumn_ServiceReq_Choose.Image = imageListCheck.Images[4];
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void txtServiceReqCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(txtServiceReqCode.Text))
                {
                    FillDataToGrid();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void txtTreatmentCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(txtTreatmentCode.Text))
                {
                    FillDataToGrid();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void txtKeyword_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(txtKeyword.Text))
                {
                    FillDataToGrid();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private async void repositoryItemBtnServiceReqDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                bool IsBreak = false;
                List<string> lstServiceName = new List<string>();
                if (!CheckLoginAdmin.IsAdmin(loginName))
                {
                    await GetSereServ(data);
                    List<ListMedicineADO> sereServ = grdSereServServiceReq.DataSource as List<ListMedicineADO>;
                    if (sereServ != null && sereServ.Count > 0)
                    {
                        foreach (ListMedicineADO item in sereServ)
                        {
                            List<HIS_PATIENT_TYPE> source = BackendDataWorker.Get<HIS_PATIENT_TYPE>();
                            Func<HIS_PATIENT_TYPE, bool> predicate = (HIS_PATIENT_TYPE o) => o.ID == item.PATIENT_TYPE_ID;
                            HIS_PATIENT_TYPE hIS_PATIENT_TYPE = source.FirstOrDefault(predicate);
                            if (hIS_PATIENT_TYPE != null && hIS_PATIENT_TYPE.IS_NOT_EDIT_ASSIGN_SERVICE == 1)
                            {
                                lstServiceName.Add(item.TDL_SERVICE_NAME);
                                IsBreak = true;
                            }
                        }
                    }
                }
                if (IsBreak)
                {
                    XtraMessageBox.Show(string.Format("Dịch vụ {0} có đối tượng thanh toán được tích \"Không cho phép sửa xóa dịch vụ đã chỉ định\", vui lòng liên hệ với quản trị hệ thống", string.Join(", ", lstServiceName)), "Thông báo");
                }
                else
                {
                    if (gridViewServiceReq.FocusedRowHandle < 0 || IsBreak)
                    {
                        return;
                    }
                    CommonParam commonParam = new CommonParam();
                    CommonParam commonParam2 = new CommonParam();
                    if (data == null)
                    {
                        LogSystem.Info("Data thuc hien huy yeu cau dich vu null: " + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data));
                    }
                    else
                    {
                        if (data == null)
                        {
                            return;
                        }
                        EmrDocumentFilter emrDocumentFilter = new EmrDocumentFilter();
                        LogSystem.Debug("TDL_TREATMENT_CODE_______________________________________" + data.TDL_TREATMENT_CODE);
                        emrDocumentFilter.TREATMENT_CODE__EXACT = data.TDL_TREATMENT_CODE;
                        emrDocumentFilter.DOCUMENT_TYPE_ID = 21L;
                        List<EMR_DOCUMENT> list = new BackendAdapter(commonParam).Get<List<EMR_DOCUMENT>>("api/EmrDocument/Get", HIS.Desktop.ApiConsumer.ApiConsumers.EmrConsumer, emrDocumentFilter, commonParam);
                        if (list != null && list.Count() > 0)
                        {
                            list = list.Where((EMR_DOCUMENT o) => o.IS_DELETE != 1).ToList();
                            string value = "SERVICE_REQ_CODE:" + data.SERVICE_REQ_CODE;
                            List<EMR_DOCUMENT> list2 = new List<EMR_DOCUMENT>();
                            foreach (EMR_DOCUMENT item2 in list)
                            {
                                if (item2.HIS_CODE != null && item2.HIS_CODE.Contains(value))
                                {
                                    list2.Add(item2);
                                }
                            }
                            if (list2.Count() > 0 && list2 != null)
                            {
                                if (MessageBox.Show("Y lệnh này đã tồn tại văn bản ký, bạn có muốn hủy dữ liệu?", ResourceMessage.ThongBao, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                                {
                                    return;
                                }
                                WaitingManager.Show();
                                HisServiceReqSDO hisServiceReqSDO = new HisServiceReqSDO();
                                hisServiceReqSDO.Id = data.ID;
                                hisServiceReqSDO.RequestRoomId = currentModule.RoomId;
                                bool flag = new BackendAdapter(commonParam2).Post<bool>("api/HisServiceReq/Delete", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqSDO, new Action(SessionManager.ActionLostToken), commonParam2);
                                WaitingManager.Hide();
                                if (flag)
                                {
                                    bool flag2 = false;
                                    foreach (EMR_DOCUMENT item3 in list2)
                                    {
                                        flag2 = new BackendAdapter(commonParam).Post<bool>("api/EmrDocument/Delete", HIS.Desktop.ApiConsumer.ApiConsumers.EmrConsumer, item3.ID, commonParam);
                                        if (flag2)
                                        {
                                            FillDataToGrid();
                                        }
                                    }
                                    MessageManager.Show(this, commonParam, flag2);
                                }
                                if (!flag)
                                {
                                    MessageManager.Show(this, commonParam2, flag);
                                }
                                SessionManager.ProcessTokenLost(commonParam2);
                            }
                            else if (MessageBox.Show(ResourceMessage.HeThongTBCuaSoThongBaoBanCoMuonHuyDuLieuKhong, ResourceMessage.ThongBao, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes && (!CheckParentBeforeDelete(data.ID) || XtraMessageBox.Show("Đã có y lệnh đính kèm (CLS). Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) != DialogResult.No))
                            {
                                WaitingManager.Show();
                                HisServiceReqSDO hisServiceReqSDO = new HisServiceReqSDO();
                                hisServiceReqSDO.Id = data.ID;
                                hisServiceReqSDO.RequestRoomId = currentModule.RoomId;
                                bool flag = new BackendAdapter(commonParam2).Post<bool>("api/HisServiceReq/Delete", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqSDO, new Action(SessionManager.ActionLostToken), commonParam2);
                                WaitingManager.Hide();
                                if (flag)
                                {
                                    FillDataToGrid();
                                }
                                MessageManager.Show(this, commonParam2, flag);
                                SessionManager.ProcessTokenLost(commonParam2);
                            }
                        }
                        else if (MessageBox.Show(ResourceMessage.HeThongTBCuaSoThongBaoBanCoMuonHuyDuLieuKhong, ResourceMessage.ThongBao, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes && (!CheckParentBeforeDelete(data.ID) || XtraMessageBox.Show("Đã có y lệnh đính kèm (CLS). Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) != DialogResult.No))
                        {
                            WaitingManager.Show();
                            HisServiceReqSDO hisServiceReqSDO = new HisServiceReqSDO();
                            hisServiceReqSDO.Id = data.ID;
                            hisServiceReqSDO.RequestRoomId = currentModule.RoomId;
                            bool flag = new BackendAdapter(commonParam2).Post<bool>("api/HisServiceReq/Delete", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqSDO, new Action(SessionManager.ActionLostToken), commonParam2);
                            WaitingManager.Hide();
                            if (flag)
                            {
                                FillDataToGrid();
                            }
                            MessageManager.Show(this, commonParam2, flag);
                            SessionManager.ProcessTokenLost(commonParam2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                WaitingManager.Hide();
            }
        }

        private bool CheckParentBeforeDelete(long _serviceReqId)
        {
            bool result = false;
            try
            {
                CommonParam commonParam = new CommonParam();
                HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
                hisServiceReqFilter.PARENT_ID = _serviceReqId;
                List<HIS_SERVICE_REQ> list = new BackendAdapter(commonParam).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqFilter, commonParam);
                if (list != null && list.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                return false;
            }
            return result;
        }

        private async void repositoryItemBtnServiceReqEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                bool IsBreak = false;
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                List<string> lstServiceName = new List<string>();
                if (!CheckLoginAdmin.IsAdmin(loginName))
                {
                    await GetSereServ(data);
                    List<ListMedicineADO> sereServ = grdSereServServiceReq.DataSource as List<ListMedicineADO>;
                    if (sereServ != null && sereServ.Count > 0)
                    {
                        foreach (ListMedicineADO item in sereServ)
                        {
                            List<HIS_PATIENT_TYPE> source = BackendDataWorker.Get<HIS_PATIENT_TYPE>();
                            Func<HIS_PATIENT_TYPE, bool> predicate = (HIS_PATIENT_TYPE o) => o.ID == item.PATIENT_TYPE_ID;
                            HIS_PATIENT_TYPE hIS_PATIENT_TYPE = source.FirstOrDefault(predicate);
                            if (hIS_PATIENT_TYPE != null && hIS_PATIENT_TYPE.IS_NOT_EDIT_ASSIGN_SERVICE == 1)
                            {
                                lstServiceName.Add(item.TDL_SERVICE_NAME);
                                IsBreak = true;
                            }
                        }
                    }
                }
                if (IsBreak)
                {
                    XtraMessageBox.Show(string.Format("Dịch vụ {0} có đối tượng thanh toán được tích \"Không cho phép sửa xóa dịch vụ đã chỉ định\", vui lòng liên hệ với quản trị hệ thống", string.Join(", ", lstServiceName)), "Thông báo");
                }
                else
                {
                    if (data == null || IsBreak)
                    {
                        return;
                    }
                    if (HisConfigCFG.AutoDeleteEmrDocumentWhenEditReq == "1")
                    {
                        EmrDocumentViewFilter emrDocumentViewFilter = new EmrDocumentViewFilter();
                        emrDocumentViewFilter.TREATMENT_CODE__EXACT = data.TDL_TREATMENT_CODE;
                        emrDocumentViewFilter.IS_DELETE = false;
                        List<V_EMR_DOCUMENT> list = new BackendAdapter(new CommonParam()).Get<List<V_EMR_DOCUMENT>>("api/EmrDocument/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.EmrConsumer, emrDocumentViewFilter, null);
                        if (list != null && list.Count() > 0)
                        {
                            string checkServiceReqCode = "SERVICE_REQ_CODE:" + data.SERVICE_REQ_CODE;
                            IEnumerable<V_EMR_DOCUMENT> enumerable = list.Where((V_EMR_DOCUMENT o) => o.DOCUMENT_TYPE_ID != 22 && !string.IsNullOrEmpty(o.HIS_CODE) && o.HIS_CODE.Contains(checkServiceReqCode));
                            if (enumerable != null && enumerable.Count() > 0)
                            {
                                if (XtraMessageBox.Show("Y lệnh đã tồn tại văn bản ký, tiếp tục sẽ tự động Xóa văn bản ký hiện tại. Bạn có muốn tiếp tục?", ResourceMessage.ThongBao, MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                WaitingManager.Show();
                                foreach (V_EMR_DOCUMENT item4 in enumerable)
                                {
                                    new BackendAdapter(new CommonParam()).Post<bool>("api/EmrDocument/Delete", HIS.Desktop.ApiConsumer.ApiConsumers.EmrConsumer, item4.ID, null);
                                }
                                WaitingManager.Hide();
                            }
                        }
                    }
                    CommonParam commonParam = new CommonParam();
                    new HIS_TREATMENT();
                    HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
                    hisTreatmentFilter.ID = data.TREATMENT_ID;
                    List<HIS_TREATMENT> list2 = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, new Action(SessionManager.ActionLostToken), commonParam);
                    if (list2 != null && list2.Count == 1)
                    {
                        HIS_TREATMENT hIS_TREATMENT = list2.FirstOrDefault();
                        if (hIS_TREATMENT.IS_PAUSE == 1 || hIS_TREATMENT.IS_ACTIVE != 1)
                        {
                            LogSystem.Debug(ResourceMessage.HoSoDieuTriDangTamKhoa);
                            MessageBox.Show(ResourceMessage.HoSoDieuTriDangTamKhoa);
                            return;
                        }
                        serviceReqPrintRaw = GetServiceReqForPrint(data.ID);
                        if (data.SERVICE_REQ_TYPE_ID == 1)
                        {
                            WaitingManager.Show();
                            List<object> list3 = new List<object>();
                            list3.Add(serviceReqPrintRaw.ID);
                            List<object> data2 = list3;
                            CallModule("HIS.Desktop.Plugins.UpdateExamServiceReq", data2);
                            WaitingManager.Hide();
                        }
                        else if (data.SERVICE_REQ_TYPE_ID == 6 || data.SERVICE_REQ_TYPE_ID == 14 || data.SERVICE_REQ_TYPE_ID == 15)
                        {
                            WaitingManager.Show();
                            HIS_SERVICE_REQ hIS_SERVICE_REQ = new HIS_SERVICE_REQ();
                            DataObjectMapper.Map<HIS_SERVICE_REQ>(hIS_SERVICE_REQ, serviceReqPrintRaw);
                            HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
                            hisExpMestFilter.SERVICE_REQ_ID = serviceReqPrintRaw.ID;
                            List<HIS_EXP_MEST> list4 = new BackendAdapter(new CommonParam()).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, new Action(SessionManager.ActionLostToken), null);
                            AssignPrescriptionEditADO assignPrescriptionEditADO;
                            if (list4 != null && list4.Count == 1)
                            {
                                HIS_EXP_MEST hIS_EXP_MEST = list4.FirstOrDefault();
                                if (hIS_EXP_MEST.IS_NOT_TAKEN.HasValue && hIS_EXP_MEST.IS_NOT_TAKEN.Value == 1)
                                {
                                    WaitingManager.Hide();
                                    MessageBox.Show(ResourceMessage.DonKhongLayKhongChoPhepSua);
                                    return;
                                }
                                assignPrescriptionEditADO = new AssignPrescriptionEditADO(hIS_SERVICE_REQ, hIS_EXP_MEST, new AssignPrescriptionEditADO.DelegateRefeshData(FillDataApterSave));
                            }
                            else
                            {
                                assignPrescriptionEditADO = new AssignPrescriptionEditADO(hIS_SERVICE_REQ, null, new AssignPrescriptionEditADO.DelegateRefeshData(FillDataApterSave));
                            }
                            if (data.IS_EXECUTE_KIDNEY_PRES == 1)
                            {
                                AssignPrescriptionKidneyADO assignPrescriptionKidneyADO = new AssignPrescriptionKidneyADO();
                                assignPrescriptionKidneyADO.AssignPrescriptionEditADO = assignPrescriptionEditADO;
                                List<object> list5 = new List<object>();
                                list5.Add(assignPrescriptionKidneyADO);
                                List<object> data2 = list5;
                                CallModule("HIS.Desktop.Plugins.AssignPrescriptionKidney", data2);
                            }
                            else
                            {
                                AssignPrescriptionADO assignPrescriptionADO = new AssignPrescriptionADO(data.TREATMENT_ID, 0L, hIS_SERVICE_REQ.ID);
                                assignPrescriptionADO.GenderName = data.TDL_PATIENT_GENDER_NAME;
                                assignPrescriptionADO.PatientDob = data.TDL_PATIENT_DOB;
                                assignPrescriptionADO.PatientName = data.TDL_PATIENT_NAME;
                                assignPrescriptionADO.AssignPrescriptionEditADO = assignPrescriptionEditADO;
                                List<object> list6 = new List<object>();
                                list6.Add(assignPrescriptionADO);
                                List<object> data2 = list6;
                                if (data.PRESCRIPTION_TYPE_ID == 1)
                                {
                                    if (data.PARENT_ID.HasValue)
                                    {
                                        HisSereServViewFilter hisSereServViewFilter = new HisSereServViewFilter();
                                        hisSereServViewFilter.SERVICE_REQ_ID = data.PARENT_ID;
                                        List<V_HIS_SERE_SERV> list7 = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERE_SERV>>("api/HisSereServ/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServViewFilter, new Action(SessionManager.ActionLostToken), null);
                                        if (list7 != null && list7.Count > 0 && list7.FirstOrDefault().TDL_SERVICE_TYPE_ID != 1 && list7.FirstOrDefault().TDL_SERVICE_TYPE_ID != 8)
                                        {
                                            assignPrescriptionADO.SereServ = list7.FirstOrDefault();
                                        }
                                    }
                                    CallModule("HIS.Desktop.Plugins.AssignPrescriptionPK", data2);
                                }
                                else if (data.PRESCRIPTION_TYPE_ID == 2)
                                {
                                    CallModule("HIS.Desktop.Plugins.AssignPrescriptionYHCT", data2);
                                }
                                else if (data.PRESCRIPTION_TYPE_ID == 3)
                                {
                                    CallModule("HIS.Desktop.Plugins.AssignPrescriptionCLS", data2);
                                }
                            }
                            WaitingManager.Hide();
                        }
                        else if (data.SERVICE_REQ_TYPE_ID == 16)
                        {
                            HIS_SERVICE_REQ hIS_SERVICE_REQ = new HIS_SERVICE_REQ();
                            DataObjectMapper.Map<HIS_SERVICE_REQ>(hIS_SERVICE_REQ, serviceReqPrintRaw);
                            AssignBloodADO assignBloodADO = new AssignBloodADO(data.TREATMENT_ID, 0L, 0L);
                            assignBloodADO.PatientDob = data.TDL_PATIENT_DOB;
                            assignBloodADO.DgProcessDataResult = new AssignBloodADO.DelegateProcessDataResult(FillDataApterSave);
                            assignBloodADO.PatientName = data.TDL_PATIENT_NAME;
                            assignBloodADO.GenderName = data.TDL_PATIENT_GENDER_NAME;
                            List<object> list8 = new List<object>();
                            list8.Add(assignBloodADO);
                            list8.Add(hIS_SERVICE_REQ);
                            List<object> data2 = list8;
                            CallModule("HIS.Desktop.Plugins.HisAssignBlood", data2);
                        }
                        else if (data.SERVICE_REQ_TYPE_ID == 17)
                        {
                            AssignServiceEditADO item2 = new AssignServiceEditADO(data.ID, data.INTRUCTION_TIME, new RefeshReference(RefreshClick));
                            List<object> list9 = new List<object>();
                            list9.Add(item2);
                            list9.Add(currentModuleBase);
                            List<object> data2 = list9;
                            CallModule("HIS.Desktop.Plugins.AssignNutritionEdit", data2);
                        }
                        else if (data.SERVICE_REQ_TYPE_ID == 2 && data.SERVICE_REQ_STT_ID == 2 && ((HIS_SERVICE_REQ)data).SAMPLE_TIME.HasValue)
                        {
                            XtraMessageBox.Show("Y lệnh đã thực hiện lấy mẫu. Bạn phải thực hiện bỏ tích lấy mẫu.", ResourceMessage.ThongBao, MessageBoxButtons.OK);
                        }
                        else
                        {
                            AssignServiceEditADO item3 = new AssignServiceEditADO(data.ID, data.INTRUCTION_TIME, new RefeshReference(RefreshClick));
                            List<object> list10 = new List<object>();
                            list10.Add(item3);
                            List<object> data2 = list10;
                            CallModule("HIS.Desktop.Plugins.AssignServiceEdit", data2);
                        }
                    }
                    else
                    {
                        LogSystem.Debug(ResourceMessage.KhongTimThayHoSoDieuTri);
                        MessageBox.Show(ResourceMessage.KhongTimThayHoSoDieuTri);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void repositoryItemBtnServiceReqPrint_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                WaitingManager.Hide();
                currentServiceReqPrint = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetRow(gridViewServiceReq.FocusedRowHandle);
                if (currentServiceReqPrint == null)
                {
                    return;
                }
                serviceReqPrintRaw = GetServiceReqForPrint(currentServiceReqPrint.ID);
                WaitingManager.Hide();
                if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 6 || currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 15 || currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 14)
                {
                    prescriptionPrint = null;
                    HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
                    hisExpMestFilter.SERVICE_REQ_ID = currentServiceReqPrint.ID;
                    List<HIS_EXP_MEST> list = new BackendAdapter(new CommonParam()).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, new Action(SessionManager.ActionLostToken), null);
                    if (list != null && list.Count > 0)
                    {
                        prescriptionPrint = list.FirstOrDefault();
                        if (prescriptionPrint.EXP_MEST_TYPE_ID != 12)
                        {
                            PrintPopupMenuProcessor = new PrintPopupMenuProcessor(new PrintMedicine_Click(PrintMedicine_Click), barManager1, loginName);
                            PrintPopupMenuProcessor.InitMenu();
                        }
                    }
                    else
                    {
                        PrintPopupMenuProcessor = new PrintPopupMenuProcessor(new PrintMedicine_Click(PrintMedicine_Click), barManager1, loginName);
                        PrintPopupMenuProcessor.InitMenu();
                    }
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 16)
                {
                    PrintBlood();
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 1)
                {
                    PrintPopupMenuProcessor = new PrintPopupMenuProcessor(new PrintMedicine_Click(PrintExam_Click), barManager1, loginName);
                    PrintPopupMenuProcessor.InitMenuKham(currentServiceReqPrint);
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 2)
                {
                    PrintPopupMenuProcessor = new PrintPopupMenuProcessor(new PrintMedicine_Click(PrintTest_Click), barManager1, loginName);
                    PrintPopupMenuProcessor.InitMenuXetNghiem();
                }
                else
                {
                    ProcessingPrint();
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void repositoryItemButtonView_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                ListMedicineADO sereServRow = (ListMedicineADO)grdViewSereServServiceReq.GetFocusedRow();
                LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => sereServRow), sereServRow));
                if (serviceReqADO == null || sereServRow == null)
                {
                    return;
                }
                List<object> list = new List<object>();
                if (serviceReqADO.EXE_SERVICE_MODULE_ID == 1)
                {
                    list.Add(sereServRow.ID);
                    CallModule("HIS.Desktop.Plugins.ExamServiceReqResult", list);
                    return;
                }
                if (serviceReqADO.EXE_SERVICE_MODULE_ID == 4)
                {
                    if (((HIS_SERVICE_REQ)serviceReqADO).IS_ANTIBIOTIC_RESISTANCE == 1)
                    {
                        Mapper.CreateMap<ListMedicineADO, HIS_SERE_SERV>();
                        HIS_SERE_SERV item = Mapper.Map<ListMedicineADO, HIS_SERE_SERV>(sereServRow);
                        list.Add(item);
                        CallModule("HIS.Desktop.Plugins.SereServTeinBacterium", list);
                    }
                    else
                    {
                        Mapper.CreateMap<ListMedicineADO, HIS_SERE_SERV>();
                        HIS_SERE_SERV item = Mapper.Map<ListMedicineADO, HIS_SERE_SERV>(sereServRow);
                        list.Add(item);
                        CallModule("HIS.Desktop.Plugins.SereServTein", list);
                    }
                    return;
                }
                if (serviceReqADO.EXE_SERVICE_MODULE_ID != 7 && serviceReqADO.EXE_SERVICE_MODULE_ID != 3 && serviceReqADO.EXE_SERVICE_MODULE_ID != 5)
                {
                    long? eXE_SERVICE_MODULE_ID = serviceReqADO.EXE_SERVICE_MODULE_ID;
                    if (eXE_SERVICE_MODULE_ID != 2 || !eXE_SERVICE_MODULE_ID.HasValue || sereServRow.IS_SENT_EXT != 1)
                    {
                        return;
                    }
                }
                list.Add(sereServRow.ID);
                CallModule("HIS.Desktop.Plugins.ServiceReqResultView", list);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void repositoryItemBtnBieuMauKhac_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                currentServiceReqPrint = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (currentServiceReqPrint != null)
                {
                    WaitingManager.Show();
                    if (currentServiceReqPrint != null && currentServiceReqPrint.JSON_PRINT_ID != null)
                    {
                        SarPrintADO sarPrintADO = new SarPrintADO();
                        sarPrintADO.JSON_PRINT_ID = currentServiceReqPrint.JSON_PRINT_ID;
                        sarPrintADO.JsonPrintResult = new DelegateSelectData(JsonPrintResult);
                        List<object> list = new List<object>();
                        list.Add(sarPrintADO);
                        List<object> data = list;
                        CallModule("SAR.Desktop.Plugins.SarPrintList", data);
                    }
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void FillDataApterSave(object prescription)
        {
            try
            {
                if (prescription != null)
                {
                    btnFind_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private V_HIS_SERVICE_REQ GetServiceReqForPrint(long serviceReqId)
        {
            V_HIS_SERVICE_REQ result = new V_HIS_SERVICE_REQ();
            try
            {
                if (serviceReqId > 0)
                {
                    HisServiceReqViewFilter hisServiceReqViewFilter = new HisServiceReqViewFilter();
                    hisServiceReqViewFilter.ID = serviceReqId;
                    hisServiceReqViewFilter.IS_ACTIVE = 1;
                    List<V_HIS_SERVICE_REQ> list = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERVICE_REQ>>("api/HisServiceReq/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqViewFilter, new Action(SessionManager.ActionLostToken), null);
                    if (list != null && list.Count > 0)
                    {
                        result = list.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new V_HIS_SERVICE_REQ();
            }
            return result;
        }

        private void JsonPrintResult(object data)
        {
            try
            {
                if (data != null)
                {
                    List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = (List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>)gridControlServiceReq.DataSource;
                    list[list.IndexOf(currentServiceReqPrint)].JSON_PRINT_ID = data.ToString();
                    gridControlServiceReq.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void tooltipServiceRequest_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            try
            {
                if (e.Info != null || e.SelectedControl != gridControlServiceReq)
                {
                    return;
                }
                GridView gridView = gridControlServiceReq.FocusedView as GridView;
                GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.ControlMousePosition);
                if (!gridHitInfo.InRowCell)
                {
                    return;
                }
                if (lastRowHandle != gridHitInfo.RowHandle || lastColumn != gridHitInfo.Column)
                {
                    lastColumn = gridHitInfo.Column;
                    lastRowHandle = gridHitInfo.RowHandle;
                    string text = "";
                    if (gridHitInfo.Column.FieldName == "IMG")
                    {
                        long num = (long)gridView.GetRowCellValue(lastRowHandle, "SERVICE_REQ_STT_ID");
                        long num2 = (long)gridView.GetRowCellValue(lastRowHandle, "SERVICE_REQ_TYPE_ID");
                        text = ((num == 1 && num2 == 2 && gridView.GetRowCellValue(lastRowHandle, "SAMPLE_TIME") != null) ? "Đã lấy mẫu" : ((num != 2 || num2 != 2 || gridView.GetRowCellValue(lastRowHandle, "RECEIVE_SAMPLE_TIME") == null) ? (gridView.GetRowCellValue(lastRowHandle, "SERVICE_REQ_STT_NAME") ?? "").ToString() : "Đã nhận mẫu"));
                    }
                    else if (gridHitInfo.Column.FieldName == "PRIORITY_DISPLAY")
                    {
                        string text2 = Inventec.Common.Resource.Get.Value("His.Desktop.Plugins.ServiceReqlist.PriorityName", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture());
                        text = text2.ToString();
                    }
                    else if (gridHitInfo.Column.FieldName == "ServiceReqEdit")
                    {
                        text = ((gridView.GetRowCellValue(lastRowHandle, "CARER_CARD_BORROW_ID") == null) ? "Sửa" : "Chỉ định dịch vụ mượn thẻ chỉ cho phép sửa/xóa thông qua chức năng \"Quản lý mượn thẻ\"");
                    }
                    else if (gridHitInfo.Column.FieldName == "ServiceReqDelete")
                    {
                        text = ((gridView.GetRowCellValue(lastRowHandle, "CARER_CARD_BORROW_ID") == null) ? "Xóa" : "Chỉ định dịch vụ mượn thẻ chỉ cho phép sửa/xóa thông qua chức năng \"Quản lý mượn thẻ\"");
                    }
                    else if (gridHitInfo.Column.FieldName == "ServiceReqPrint")
                    {
                        text = ((gridView.GetRowCellValue(lastRowHandle, "CARER_CARD_BORROW_ID") == null) ? "In" : "Chỉ định dịch vụ mượn thẻ chỉ cho phép sửa/xóa thông qua chức năng \"Quản lý mượn thẻ\"");
                    }
                    else if (gridView.GetRowCellValue(lastRowHandle, "IS_TEMPORARY_PRES") != null && (gridView.GetRowCellValue(lastRowHandle, "IS_TEMPORARY_PRES") ?? "").ToString() == "1")
                    {
                        text = "Là đơn tạm";
                    }
                    lastInfo = new ToolTipControlInfo(new GridToolTipInfo(gridView, new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "Text")), text);
                }
                e.Info = lastInfo;
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtServiceReqCode.Text) && string.IsNullOrWhiteSpace(txtTreatmentCode.Text))
                {
                    if (dtIntructionTimeFrom.EditValue != null)
                    {
                        if (dtIntructionTimeTo.EditValue != null)
                        {
                            if ((dtIntructionTimeTo.DateTime.Date - dtIntructionTimeFrom.DateTime.Date).TotalDays > 90.0)
                            {
                                XtraMessageBox.Show("Khoảng thời gian tìm kiếm quá dài, vui lòng chọn tối đa 3 tháng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            WaitingManager.Show();
                            FillDataToGrid();
                            WaitingManager.Hide();
                            return;
                        }
                        dtIntructionTimeTo.EditValue = DateTime.Now;
                        if ((dtIntructionTimeTo.DateTime.Date - dtIntructionTimeFrom.DateTime.Date).TotalDays > 90.0)
                        {
                            XtraMessageBox.Show("Khoảng thời gian tìm kiếm quá dài, vui lòng chọn tối đa 3 tháng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        WaitingManager.Show();
                        FillDataToGrid();
                        WaitingManager.Hide();
                    }
                    else
                    {
                        XtraMessageBox.Show("Thời gian từ không được bỏ trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    WaitingManager.Show();
                    FillDataToGrid();
                    WaitingManager.Hide();
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void bbtnRCFind_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                btnFind_Click(null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void FillDataToControl(HIS_EXP_MEST data, HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReq2)
        {
            try
            {
                if (serviceReq2 != null)
                {
                    lblPatientName.Text = serviceReq2.TDL_PATIENT_NAME;
                    lblTreatmentCode.Text = serviceReq2.TDL_TREATMENT_CODE;
                    lblGender.Text = serviceReq2.TDL_PATIENT_GENDER_NAME;
                    if (serviceReq2.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == 1)
                    {
                        lbDOB.Text = serviceReq2.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                    }
                    else
                    {
                        lbDOB.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serviceReq2.TDL_PATIENT_DOB);
                    }
                    lblSoTT.Text = (serviceReq2.NUM_ORDER.HasValue ? serviceReq2.NUM_ORDER.ToString() : "");
                    HIS_CARD hisCard = GetHisCard(serviceReq2.TDL_PATIENT_ID);
                    lblSoTheTM.Text = ((hisCard != null) ? hisCard.CARD_CODE : "");
                    if (serviceReq2.SERVICE_REQ_STT_ID == 1)
                    {
                        lblExpMestStt.Text = "Chưa xử lý";
                    }
                    else if (serviceReq2.SERVICE_REQ_STT_ID == 2)
                    {
                        lblExpMestStt.Text = "Đang xử lý";
                    }
                    else if (serviceReq2.SERVICE_REQ_STT_ID == 3)
                    {
                        lblExpMestStt.Text = "Hoàn thành";
                    }
                    HIS_DEPARTMENT hIS_DEPARTMENT = BackendDataWorker.Get<HIS_DEPARTMENT>().FirstOrDefault((HIS_DEPARTMENT o) => o.ID == serviceReq2.REQUEST_DEPARTMENT_ID);
                    if (hIS_DEPARTMENT != null)
                    {
                        lblReqDepartment.Text = hIS_DEPARTMENT.DEPARTMENT_NAME;
                    }
                    HIS_DEPARTMENT hIS_DEPARTMENT2 = BackendDataWorker.Get<HIS_DEPARTMENT>().FirstOrDefault((HIS_DEPARTMENT o) => o.ID == serviceReq2.EXECUTE_DEPARTMENT_ID);
                    if (hIS_DEPARTMENT2 != null)
                    {
                        lbExcuteDepartment.Text = hIS_DEPARTMENT2.DEPARTMENT_NAME;
                    }
                    if (data != null)
                    {
                        SetVisibleControl(true);
                        lciAggrExpMestCode.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciAggrExpMestCode.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                        lciExpMestCode.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciExpMestCode.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                        lciExpMestRoom.Text = Inventec.Common.Resource.Get.Value("frmServiceReqList.lciExpMestRoom.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
                    }
                    else
                    {
                        SetVisibleControl(false);
                    }
                    if (serviceReq2.SERVICE_REQ_TYPE_ID == 2 || serviceReq2.SERVICE_REQ_TYPE_ID == 13 || serviceReq2.SERVICE_REQ_TYPE_ID == 3 || serviceReq2.SERVICE_REQ_TYPE_ID == 5 || serviceReq2.SERVICE_REQ_TYPE_ID == 8 || serviceReq2.SERVICE_REQ_TYPE_ID == 9)
                    {
                        SetVisibleControlRegion2(true);
                        lblBarcode.Text = serviceReq2.BARCODE;
                        chkReqSended.Checked = serviceReq2.IS_SENT_EXT == 1;
                        lblAssignTurnCode.Text = ((HIS_SERVICE_REQ)serviceReq2).ASSIGN_TURN_CODE;
                    }
                    else
                    {
                        SetVisibleControlRegion2(false);
                    }
                    if (serviceReq2.SERVICE_REQ_TYPE_ID == 15 || serviceReq2.SERVICE_REQ_TYPE_ID == 6 || serviceReq2.SERVICE_REQ_TYPE_ID == 14 || serviceReq2.SERVICE_REQ_TYPE_ID == 16)
                    {
                        SetVisibleControlRegion3(true);
                        chkIsKidney.ReadOnly = true;
                        chkIsHomePres.ReadOnly = true;
                        HIS_EXP_MEST aData = data;
                        lblAggrExpMestCode.Text = aData.TDL_AGGR_EXP_MEST_CODE;
                        lblExpMestCode.Text = aData.EXP_MEST_CODE;
                        HIS_EXP_MEST_STT hIS_EXP_MEST_STT = BackendDataWorker.Get<HIS_EXP_MEST_STT>().FirstOrDefault((HIS_EXP_MEST_STT o) => o.ID == aData.EXP_MEST_STT_ID);
                        V_HIS_MEDI_STOCK v_HIS_MEDI_STOCK = BackendDataWorker.Get<V_HIS_MEDI_STOCK>().FirstOrDefault((V_HIS_MEDI_STOCK o) => o.ID == aData.MEDI_STOCK_ID);
                        lblExpMestRoom.Text = ((v_HIS_MEDI_STOCK != null) ? v_HIS_MEDI_STOCK.MEDI_STOCK_NAME : "");
                        chkIsKidney.Checked = serviceReq2.IS_KIDNEY == 1;
                        chkIsHomePres.Checked = serviceReq2.IS_HOME_PRES == 1;
                        lblSoThang.Text = (serviceReq2.REMEDY_COUNT.HasValue ? serviceReq2.REMEDY_COUNT.ToString() : "");
                        if (((HIS_SERVICE_REQ)serviceReq2).IS_TEMPORARY_PRES == 1)
                        {
                            btnMobaCreate.Enabled = false;
                        }
                        else if (currentWorkPlace == null || currentWorkPlace.DepartmentId != hIS_DEPARTMENT.ID)
                        {
                            btnMobaCreate.Enabled = false;
                        }
                        else if (aData.EXP_MEST_STT_ID != 5)
                        {
                            btnMobaCreate.Enabled = false;
                        }
                        else if (aData.EXP_MEST_TYPE_ID == 1)
                        {
                            btnMobaCreate.Enabled = false;
                        }
                        else if (v_HIS_MEDI_STOCK != null && v_HIS_MEDI_STOCK.DEPARTMENT_ID == currentWorkPlace.DepartmentId)
                        {
                            btnMobaCreate.Enabled = true;
                        }
                        else
                        {
                            btnMobaCreate.Enabled = true;
                        }
                    }
                    else
                    {
                        SetVisibleControlRegion3(false);
                    }
                    if (serviceReq2.SERVICE_REQ_TYPE_ID == 17)
                    {
                        lciRationTime.Visibility = LayoutVisibility.Always;
                        HIS_RATION_TIME hIS_RATION_TIME = lsRationTime.FirstOrDefault((HIS_RATION_TIME o) => o.ID == (serviceReq2.RATION_TIME_ID ?? 0));
                        if (hIS_RATION_TIME != null)
                        {
                            lblRationTime.Text = hIS_RATION_TIME.RATION_TIME_NAME;
                        }
                        lciRationSumCode.Visibility = LayoutVisibility.Always;
                        HIS_RATION_SUM rationSumByID = GetRationSumByID(serviceReq2.RATION_SUM_ID);
                        lblRationSumCode.Text = ((rationSumByID != null) ? rationSumByID.RATION_SUM_CODE : "");
                    }
                    else
                    {
                        lciRationTime.Visibility = LayoutVisibility.Never;
                        lciRationSumCode.Visibility = LayoutVisibility.Never;
                    }
                    if (serviceReq2.SERVICE_REQ_TYPE_ID == 2)
                    {
                        lciSamplerName.Visibility = LayoutVisibility.Always;
                        lciReceiveSampleName.Visibility = LayoutVisibility.Always;
                        lblSamplerName.Text = ((HIS_SERVICE_REQ)serviceReq2).SAMPLER_LOGINNAME + " - " + ((HIS_SERVICE_REQ)serviceReq2).SAMPLER_USERNAME;
                        lblReceiveSampleName.Text = ((HIS_SERVICE_REQ)serviceReq2).RECEIVE_SAMPLE_LOGINNAME + " - " + ((HIS_SERVICE_REQ)serviceReq2).RECEIVE_SAMPLE_USERNAME;
                        lciTestSampleTypeName.Visibility = LayoutVisibility.Always;
                        HIS_TEST_SAMPLE_TYPE val = BackendDataWorker.Get<HIS_TEST_SAMPLE_TYPE>().FirstOrDefault((HIS_TEST_SAMPLE_TYPE o) => o.ID == ((HIS_SERVICE_REQ)serviceReq2).TEST_SAMPLE_TYPE_ID);
                        lblTestSampleTypeName.Text = ((val != null) ? val.TEST_SAMPLE_TYPE_NAME : "");
                    }
                    else
                    {
                        lciSamplerName.Visibility = LayoutVisibility.Never;
                        lciReceiveSampleName.Visibility = LayoutVisibility.Never;
                        lciTestSampleTypeName.Visibility = LayoutVisibility.Never;
                    }
                }
                else
                {
                    lblPatientName.Text = "";
                    lblRationTime.Text = "";
                    lblTreatmentCode.Text = "";
                    lblGender.Text = "";
                    lblReqDepartment.Text = "";
                    lblSoTT.Text = "";
                    lblSoTheTM.Text = "";
                    lblBarcode.Text = "";
                    lblSamplerName.Text = "";
                    lblReceiveSampleName.Text = "";
                    lblAssignTurnCode.Text = "";
                    lblTestSampleTypeName.Text = "";
                    SetVisibleControl(false);
                    SetVisibleControlRegion2(false);
                    SetVisibleControlRegion3(false);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private HIS_RATION_SUM GetRationSumByID(long? ID)
        {
            HIS_RATION_SUM result = null;
            try
            {
                if (!ID.HasValue || ID < 0)
                {
                    return null;
                }
                HisRationSumFilter hisRationSumFilter = new HisRationSumFilter();
                hisRationSumFilter.ID = ID;
                List<HIS_RATION_SUM> list = new BackendAdapter(new CommonParam()).Get<List<HIS_RATION_SUM>>("api/HisRationSum/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisRationSumFilter, new CommonParam());
                if (list != null)
                {
                    result = list.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
                LogSystem.Error(ex);
            }
            return result;
        }

        private HIS_CARD GetHisCard(long patientId)
        {
            try
            {
                HisCardFilter hisCardFilter = new HisCardFilter();
                hisCardFilter.PATIENT_ID = patientId;
                hisCardFilter.ORDER_DIRECTION = "DESC";
                hisCardFilter.ORDER_FIELD = "MODIFY_TIME";
                List<HIS_CARD> list = new BackendAdapter(new CommonParam()).Get<List<HIS_CARD>>("api/HisCard/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisCardFilter, new Action(SessionManager.ActionLostToken), null);
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return null;
        }

        private void SetVisibleControl(bool p)
        {
            try
            {
                LayoutVisibility visibility = ((!p) ? LayoutVisibility.Never : LayoutVisibility.Always);
                lciBtnAggrExpMest.Visibility = visibility;
                emptySpaceItem2.Visibility = visibility;
                lciBtnMobaCreate.Visibility = visibility;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetVisibleControlRegion2(bool p)
        {
            try
            {
                LayoutVisibility visibility = ((!p) ? LayoutVisibility.Never : LayoutVisibility.Always);
                lciReqSended.Visibility = visibility;
                chkReqSended.ReadOnly = true;
                lciBarcode.Visibility = visibility;
                lciAssignTurnCode.Visibility = visibility;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SetVisibleControlRegion3(bool p)
        {
            try
            {
                LayoutVisibility visibility = ((!p) ? LayoutVisibility.Never : LayoutVisibility.Always);
                lciExpMestRoom.Visibility = visibility;
                lciExpMestCode.Visibility = visibility;
                layoutControlItem11.Visibility = visibility;
                lciIsKidney.Visibility = visibility;
                lciAggrExpMestCode.Visibility = visibility;
                lciIsHomePres.Visibility = visibility;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void btnAggrExpMest_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentServiceReq != null && currentPrescription != null)
                {
                    MobaImpMestListADO item = new MobaImpMestListADO(2L, currentPrescription.EXP_MEST_CODE, currentServiceReq.TDL_TREATMENT_CODE);
                    List<object> list = new List<object>();
                    list.Add(item);
                    List<object> data = list;
                    CallModule("HIS.Desktop.Plugins.MobaImpMestList", data);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void btnMobaCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnMobaCreate.Enabled && currentServiceReq != null && currentPrescription != null && (currentServiceReq.SERVICE_REQ_TYPE_ID == 6 || currentServiceReq.SERVICE_REQ_TYPE_ID == 15 || currentServiceReq.SERVICE_REQ_TYPE_ID == 14 || currentServiceReq.SERVICE_REQ_TYPE_ID == 16) && currentPrescription.EXP_MEST_STT_ID == 5)
                {
                    List<object> list = new List<object>();
                    list.Add(currentPrescription.ID);
                    List<object> data = list;
                    List<object> list2 = new List<object>();
                    list2.Add(currentServiceReq.ID);
                    List<object> list3 = list2;
                    if (currentPrescription.EXP_MEST_TYPE_ID == 9)
                    {
                        CallModule("HIS.Desktop.Plugins.MobaPrescriptionCreate", data);
                    }
                    else if (currentPrescription.EXP_MEST_TYPE_ID == 11)
                    {
                        CallModule("HIS.Desktop.Plugins.MobaCabinetCreate", data);
                    }
                    else if (currentPrescription.EXP_MEST_TYPE_ID == 12)
                    {
                        CallModule("HIS.Desktop.Plugins.MobaBloodCreate", data);
                    }
                    else
                    {
                        MessageManager.Show(ResourceMessage.TaiKhoanKhongCoQuyenThucHienChucNang);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void btnPrintTotal_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridControlServiceReq.DataSource == null)
                {
                    return;
                }
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = (List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>)gridControlServiceReq.DataSource;
                if (list.Count > 0)
                {
                    listServiceReq = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                    foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in list)
                    {
                        if (item.isCheck)
                        {
                            listServiceReq.Add(item);
                        }
                    }
                    if (listServiceReq != null && listServiceReq.Count > 0)
                    {
                        PrintMps190001();
                    }
                }
                else
                {
                    XtraMessageBox.Show(ResourceMessage.BanChuaChonDichVu);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintMps190001()
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_001c: Expected O, but got Unknown
            //IL_0029: Unknown result type (might be due to invalid IL or missing references)
            //IL_0033: Expected O, but got Unknown
            try
            {
                RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                val.RunPrintTemplate("Mps190001", new DelegateRunPrinter(DelegateRunPrinter));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private bool CheckListServiceReq(List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> listServiceReq, CommonParam param)
        {
            bool result = false;
            try
            {
                if (listServiceReq != null && listServiceReq.Count > 0)
                {
                    param.Messages = new List<string>();
                    Dictionary<long, List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>> dictionary = new Dictionary<long, List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>>();
                    string text = "";
                    foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in listServiceReq)
                    {
                        if (item.SERVICE_REQ_TYPE_ID == 6 || item.SERVICE_REQ_TYPE_ID == 16 || item.SERVICE_REQ_TYPE_ID == 15 || item.SERVICE_REQ_TYPE_ID == 14)
                        {
                            text += string.Format("{0}, ", item.SERVICE_REQ_CODE);
                        }
                        if (!dictionary.ContainsKey(item.TREATMENT_ID))
                        {
                            dictionary[item.TREATMENT_ID] = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                        }
                        dictionary[item.TREATMENT_ID].Add(item);
                    }
                    string text2 = HisConfigs.Get<string>("HIS.Desktop.Plugins.ServiceReqList.MpsTotalToBordereau");
                    if (!string.IsNullOrEmpty(text) && !(text2 == "1"))
                    {
                        param.Messages.Add(string.Format(ResourceMessage.DichVuLaThuoc, text));
                    }
                    else if (dictionary.Count > 1)
                    {
                        param.Messages.Add(ResourceMessage.DichVuKhongCungHoSoDieuTri);
                    }
                    if (param.Messages.Count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = true;
                LogSystem.Error(ex);
            }
            return result;
        }

        private bool CheckListServiceReqMedicine(List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> listServiceReq, CommonParam param)
        {
            bool result = false;
            try
            {
                if (listServiceReq != null && listServiceReq.Count > 0)
                {
                    param.Messages = new List<string>();
                    Dictionary<long, List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>> dictionary = new Dictionary<long, List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>>();
                    string text = "";
                    foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in listServiceReq)
                    {
                        if (item.SERVICE_REQ_TYPE_ID != 6)
                        {
                            text += string.Format("{0}, ", item.SERVICE_REQ_CODE);
                        }
                        if (!dictionary.ContainsKey(item.TREATMENT_ID))
                        {
                            dictionary[item.TREATMENT_ID] = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                        }
                        dictionary[item.TREATMENT_ID].Add(item);
                    }
                    if (!string.IsNullOrEmpty(text))
                    {
                        param.Messages.Add(string.Format("Các yêu cầu sau không là đơn phòng khám : {0}", text));
                    }
                    else if (dictionary.Count > 1)
                    {
                        param.Messages.Add(ResourceMessage.DichVuKhongCungHoSoDieuTri);
                    }
                    if (param.Messages.Count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = true;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void repositoryItemButtonEditIntructionTime_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                WaitingManager.Show();
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    if (serviceReqADO != null)
                    {
                        List<object> list = new List<object>();
                        list.Add(serviceReqADO.ID);
                        list.Add(new RefeshReference(RefreshClick));
                        CallModule("HIS.Desktop.Plugins.ServiceReqUpdateInstruction", list);
                    }
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void repositoryItemButtonPrint_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS_SERE_SERV hIS_SERE_SERV = (HIS_SERE_SERV)grdViewSereServServiceReq.GetFocusedRow();
                if (hIS_SERE_SERV == null)
                {
                    return;
                }
                sereServPrint = hIS_SERE_SERV;
                treatmentCode = ((!string.IsNullOrEmpty(sereServPrint.TDL_TREATMENT_CODE)) ? sereServPrint.TDL_TREATMENT_CODE : "");
                if (hIS_SERE_SERV.TDL_SERVICE_TYPE_ID == 11 || hIS_SERE_SERV.TDL_SERVICE_TYPE_ID == 4)
                {
                    V_HIS_SERE_SERV_1 v_HIS_SERE_SERV_ = new V_HIS_SERE_SERV_1();
                    HisSereServView1Filter hisSereServView1Filter = new HisSereServView1Filter();
                    hisSereServView1Filter.ID = sereServPrint.ID;
                    List<V_HIS_SERE_SERV_1> list = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERE_SERV_1>>("api/HisSereServ/GetView1", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServView1Filter, null);
                    if (list != null && list.Count > 0)
                    {
                        v_HIS_SERE_SERV_ = list.FirstOrDefault();
                    }
                    if (v_HIS_SERE_SERV_ != null && v_HIS_SERE_SERV_.ID > 0)
                    {
                        PrintPopupMenuProcessor = new PrintPopupMenuProcessor(new PrintMedicine_Click(PrintPttt_Click), barManager1, v_HIS_SERE_SERV_.SERVICE_REQ_STT_ID, hIS_SERE_SERV.TDL_SERVICE_TYPE_ID, loginName);
                        PrintPopupMenuProcessor.InitMenuPttt();
                    }
                    else
                    {
                        PrintPopupMenuProcessor = new PrintPopupMenuProcessor(new PrintMedicine_Click(PrintPttt_Click), barManager1, hIS_SERE_SERV.TDL_SERVICE_TYPE_ID, loginName);
                        PrintPopupMenuProcessor.InitMenuPttt();
                    }
                }
                else if (hIS_SERE_SERV.TDL_SERVICE_TYPE_ID == 13)
                {
                    ProcessPrintResult();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void CallModule(string moduleLink, List<object> data)
        {
            try
            {
                CallModule callModule = new CallModule(moduleLink, currentModule.RoomId, currentModule.RoomTypeId, data);
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        public void RefreshClick()
        {
            try
            {
                WaitingManager.Show();
                FillDataToGrid();
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void Btn_EvenLog_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO != null)
                {
                    WaitingManager.Show();
                    List<object> list = new List<object>();
                    DataInit dataInit = new DataInit(ConfigApplications.NumPageSize, "", "", "", "", "", serviceReqADO.SERVICE_REQ_CODE);
                    KeyCodeADO keyCodeADO = new KeyCodeADO();
                    keyCodeADO.serviceRequestCode = serviceReqADO.SERVICE_REQ_CODE;
                    list.Add(keyCodeADO);
                    list.Add(2L);
                    Inventec.Desktop.Common.Modules.Module moduleWithWorkingRoom = PluginInstance.GetModuleWithWorkingRoom(currentModule, currentModule.RoomId, currentModule.RoomTypeId);
                    object pluginInstance = PluginInstance.GetPluginInstance(moduleWithWorkingRoom, list);
                    if (pluginInstance == null)
                    {
                        throw new ArgumentNullException("Khoi tao moduleData that bai. extenceInstance = null");
                    }
                    WaitingManager.Hide();
                    PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.EventLog", moduleWithWorkingRoom.RoomId, moduleWithWorkingRoom.RoomTypeId, list);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void RightMenu_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!(e.Item is BarButtonItem))
                {
                    return;
                }
                BarButtonItem barButtonItem = sender as BarButtonItem;
                PrintPopupMenuProcessor.ModuleType moduleType = (PrintPopupMenuProcessor.ModuleType)e.Item.Tag;
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                switch (moduleType)
                {
                    case PrintPopupMenuProcessor.ModuleType.Edit:
                        repositoryItemBtnServiceReqEdit_ButtonClick(null, null);
                        break;
                    case PrintPopupMenuProcessor.ModuleType.Delete:
                        repositoryItemBtnServiceReqDelete_ButtonClick(null, null);
                        break;
                    case PrintPopupMenuProcessor.ModuleType.Print:
                        repositoryItemBtnServiceReqPrint_ButtonClick(null, null);
                        break;
                    case PrintPopupMenuProcessor.ModuleType.EditIntruction:
                        repositoryItemButtonEditIntructionTime_ButtonClick(null, null);
                        break;
                    case PrintPopupMenuProcessor.ModuleType.EvenLog:
                        Btn_EvenLog_ButtonClick(null, null);
                        break;
                    case PrintPopupMenuProcessor.ModuleType.BieuMauKhac:
                        repositoryItemBtnBieuMauKhac_ButtonClick(null, null);
                        break;
                    case PrintPopupMenuProcessor.ModuleType.ExamMain:
                        Btn_ExamMain_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.GuiLaiXN:
                        if (serviceReqADO.SERVICE_REQ_TYPE_ID == 2)
                        {
                            GuiLaiXNSangLIS(null, null);
                        }
                        else
                        {
                            SendResultToPacs(null, null);
                        }
                        break;
                    case PrintPopupMenuProcessor.ModuleType.BieuMauKhacV2:
                        Btn_BieuMauKhacV2_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.SendOldSystemIntegration:
                        Btn_SendOldSystemIntegration_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.OpenAttachFile:
                        Btn_OpenAttachFile_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.EnterInforBeforeSurgery:
                        Btn_EnterInforBeforeSurgery_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.Execute:
                        Btn_SeqUpdateNoExecute_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.NoExecute:
                        Btn_SeqUpdateNoExecute_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.SampleInfo:
                        Btn_LayMauBenhPham_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.SampleType:
                        UpdateSampleType();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.ChangeRoom:
                        Btn_ChangeRoom_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.DrugInterventionInfo:
                        Btn_DrugInterventionInfo_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.AllowNotExecute:
                        Btn_AllowNotExecute_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.DisposeAllowNotExecute:
                        Btn_DisposeAllowNotExecute_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.DanhSachVanBanDaKy:
                        ShowEmrDocumentList();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.KetQuaHeThongBenhAnhDienTu:
                        PrintKetQuaHeThongBenhAnhDienTu();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.GiayDeNghiDoiTraDichVu:
                        GiayDeNghiDoiTraDichVu();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.TaoPhieuYeuCauSuDungKhangSinh:
                        TaoPhieuYeuCauSuDungKhangSinh();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.HuyLayMau:
                        Btn_HuyLayMau_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.ChuyenThanhDonTam:
                        Btn_ChuyenDonTam_ButtonClick();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.InVatTuTSD:
                        Btn_InVatTuTSD();
                        break;
                    case PrintPopupMenuProcessor.ModuleType.TheBenhNhan:
                    case PrintPopupMenuProcessor.ModuleType.PhieuThuKiemPhieuYcKham:
                    case PrintPopupMenuProcessor.ModuleType.HenKhamLai:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void Btn_InVatTuTSD()
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_001c: Expected O, but got Unknown
            //IL_0029: Unknown result type (might be due to invalid IL or missing references)
            //IL_0033: Expected O, but got Unknown
            try
            {
                RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), PrintStoreLocation.ROOT_PATH);
                val.RunPrintTemplate("Mps000494", new DelegateRunPrinter(DelegateRunPrinter));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void RunPrint(string printTypeCode, string fileName, object data, DelegateEventLog EventLogPrint, bool result, long? roomId)
        {
            try
            {
                string printerName = "";
                if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
                {
                    printerName = GlobalVariables.dicPrinter[printTypeCode];
                }
                if (ConfigApplications.CheDoInChoCacChucNangTrongPhanMem == 2)
                {
                    result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName, EventLogPrint));
                    return;
                }
                InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode(((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow()).TDL_TREATMENT_CODE, printTypeCode, roomId);
                PrintData printData = new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.ShowDialog, printerName, EventLogPrint);
                printData.EmrInputADO = emrInputADO;
                result = MpsPrinter.Run(printData);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void Btn_ChuyenDonTam_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                bool flag = false;
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (data == null || data.ID <= 0)
                {
                    return;
                }
                List<V_EMR_DOCUMENT> list = null;
                if (data.TRACKING_ID.HasValue)
                {
                    EmrDocumentViewFilter emrDocumentViewFilter = new EmrDocumentViewFilter();
                    emrDocumentViewFilter.TREATMENT_CODE__EXACT = data.TDL_TREATMENT_CODE;
                    List<V_EMR_DOCUMENT> list2 = new BackendAdapter(new CommonParam()).Get<List<V_EMR_DOCUMENT>>("api/EmrDocument/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.EmrConsumer, emrDocumentViewFilter, null);
                    if (list2 != null && list2.Count() > 0)
                    {
                        list = list2.Where((V_EMR_DOCUMENT o) => !string.IsNullOrEmpty(o.HIS_CODE) && o.HIS_CODE.Equals(string.Format("Mps000062 TREATMENT_CODE:{0} HIS_TRACKING:{1}", data.TDL_TREATMENT_CODE, data.TRACKING_ID))).ToList();
                        if (list != null && list.Count() > 0)
                        {
                            if (XtraMessageBox.Show(string.Format("Y lệnh {0} đã được gắn tờ điều trị và tờ điều trị đã được ký. Bạn có muốn bỏ gắn tờ điều trị và hủy văn bản ký của tờ điều trị không?", data.SERVICE_REQ_CODE), ResourceMessage.ThongBao, MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }
                            flag = true;
                        }
                        else if (XtraMessageBox.Show(string.Format("Y lệnh {0} đã được gắn tờ điều trị. Bạn có muốn bỏ gắn tờ điều trị không?", data.SERVICE_REQ_CODE), ResourceMessage.ThongBao, MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else if (XtraMessageBox.Show(string.Format("Y lệnh {0} đã được gắn tờ điều trị. Bạn có muốn bỏ gắn tờ điều trị không?", data.SERVICE_REQ_CODE), ResourceMessage.ThongBao, MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                bool value = false;
                WaitingManager.Show();
                CommonParam commonParam = new CommonParam();
                HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
                hisServiceReqFilter.ID = data.ID;
                List<HIS_SERVICE_REQ> list3 = new BackendAdapter(new CommonParam()).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqFilter, null);
                HIS_SERVICE_REQ hIS_SERVICE_REQ = ((list3 != null) ? list3.FirstOrDefault() : null);
                if (hIS_SERVICE_REQ != null)
                {
                    HIS_SERVICE_REQ hIS_SERVICE_REQ2 = new BackendAdapter(commonParam).Post<HIS_SERVICE_REQ>("api/HisServiceReq/UpdateToTemporaryPres", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hIS_SERVICE_REQ, commonParam);
                    if (hIS_SERVICE_REQ2 != null)
                    {
                        value = true;
                        if (flag)
                        {
                            bool flag2 = new BackendAdapter(commonParam).Post<bool>("api/EmrDocument/DeleteList", HIS.Desktop.ApiConsumer.ApiConsumers.EmrConsumer, list.Select((V_EMR_DOCUMENT o) => o.ID).ToList(), commonParam);
                        }
                        FillDataToGrid();
                    }
                }
                WaitingManager.Hide();
                MessageManager.Show(this, commonParam, value);
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Warn(ex);
            }
        }

        private void Btn_HuyLayMau_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO != null && serviceReqADO.ID > 0)
                {
                    bool value = false;
                    WaitingManager.Show();
                    CommonParam commonParam = new CommonParam();
                    ServiceReqSampleInfoSDO serviceReqSampleInfoSDO = new ServiceReqSampleInfoSDO();
                    serviceReqSampleInfoSDO.ServiceReqId = serviceReqADO.ID;
                    serviceReqSampleInfoSDO.ReqRoomId = currentModule.RoomId;
                    serviceReqSampleInfoSDO.IsCancel = true;
                    HIS_SERVICE_REQ hIS_SERVICE_REQ = new BackendAdapter(commonParam).Post<HIS_SERVICE_REQ>("api/HisServiceReq/UpdateSampleInfo", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, serviceReqSampleInfoSDO, commonParam);
                    if (hIS_SERVICE_REQ != null)
                    {
                        value = true;
                        FillDataToGrid();
                    }
                    WaitingManager.Hide();
                    MessageManager.Show(this, commonParam, value);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Warn(ex);
            }
        }

        private void TaoPhieuYeuCauSuDungKhangSinh()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
                hisExpMestFilter.SERVICE_REQ_ID = serviceReqADO.ID;
                List<HIS_EXP_MEST> list = new BackendAdapter(new CommonParam()).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, new Action(SessionManager.ActionLostToken), null);
                if (list == null || list.Count() <= 0)
                {
                    return;
                }
                HIS_EXP_MEST hIS_EXP_MEST = list.FirstOrDefault();
                List<object> list4;
                AntibioticRequestADO ado;
                HisExpMestMedicineFilter hisExpMestMedicineFilter;
                List<V_HIS_EXP_MEST_MEDICINE> list2;
                HisMedicineTypeAcinViewFilter hisMedicineTypeAcinViewFilter;
                List<V_HIS_MEDICINE_TYPE_ACIN> list3;
                if (!hIS_EXP_MEST.ANTIBIOTIC_REQUEST_ID.HasValue)
                {
                    hisExpMestMedicineFilter = new HisExpMestMedicineFilter();
                    hisExpMestMedicineFilter.TDL_SERVICE_REQ_ID = serviceReqADO.ID;
                    list2 = new BackendAdapter(new CommonParam()).Get<List<V_HIS_EXP_MEST_MEDICINE>>("api/HisExpMestMedicine/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestMedicineFilter, null);
                    if (list2 == null || list2.Count() <= 0)
                    {
                        return;
                    }
                    hisMedicineTypeAcinViewFilter = new HisMedicineTypeAcinViewFilter();
                    hisMedicineTypeAcinViewFilter.MEDICINE_TYPE_IDs = list2.Select((V_HIS_EXP_MEST_MEDICINE o) => o.TDL_MEDICINE_TYPE_ID.Value).ToList();
                    list3 = new BackendAdapter(new CommonParam()).Get<List<V_HIS_MEDICINE_TYPE_ACIN>>("api/HisMedicineTypeAcin/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisMedicineTypeAcinViewFilter, null);
                    if (list3 == null && list3.Count() == 0)
                    {
                        XtraMessageBox.Show("Đơn không chứa hoạt chất nào cần tạo phiếu yêu cầu sử dụng", ResourceMessage.ThongBao, MessageBoxButtons.OK);
                        return;
                    }
                    list3 = list3.Where((V_HIS_MEDICINE_TYPE_ACIN o) => o.IS_APPROVAL_REQUIRED == 1).ToList();
                    list4 = new List<object>();
                    ado = new AntibioticRequestADO();
                    ado.ExpMestId = hIS_EXP_MEST.ID;
                    ado.InstructionDate = serviceReqADO.INTRUCTION_DATE;
                    ProcessDataToSend(serviceReqADO, list3, list2, ref ado);
                    list4.Add(ado);
                    PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.AntibioticRequest", currentModule.RoomId, currentModule.RoomTypeId, list4);
                    return;
                }
                HisAntibioticRequestFilter hisAntibioticRequestFilter = new HisAntibioticRequestFilter();
                hisAntibioticRequestFilter.ID = hIS_EXP_MEST.ANTIBIOTIC_REQUEST_ID;
                V_HIS_ANTIBIOTIC_REQUEST antibioticRequest = new BackendAdapter(new CommonParam()).Get<List<V_HIS_ANTIBIOTIC_REQUEST>>("api/HisAntibioticRequest/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisAntibioticRequestFilter, null).FirstOrDefault();
                list4 = new List<object>();
                ado = new AntibioticRequestADO();
                ado.AntibioticRequest = antibioticRequest;
                ado.ExpMestId = hIS_EXP_MEST.ID;
                ado.InstructionDate = serviceReqADO.INTRUCTION_DATE;
                hisExpMestMedicineFilter = new HisExpMestMedicineFilter();
                hisExpMestMedicineFilter.TDL_SERVICE_REQ_ID = serviceReqADO.ID;
                list2 = new BackendAdapter(new CommonParam()).Get<List<V_HIS_EXP_MEST_MEDICINE>>("api/HisExpMestMedicine/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestMedicineFilter, null);
                list3 = new List<V_HIS_MEDICINE_TYPE_ACIN>();
                bool flag = false;
                hisMedicineTypeAcinViewFilter = new HisMedicineTypeAcinViewFilter();
                hisMedicineTypeAcinViewFilter.MEDICINE_TYPE_IDs = list2.Select((V_HIS_EXP_MEST_MEDICINE o) => o.TDL_MEDICINE_TYPE_ID.Value).ToList();
                list3 = new BackendAdapter(new CommonParam()).Get<List<V_HIS_MEDICINE_TYPE_ACIN>>("api/HisMedicineTypeAcin/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisMedicineTypeAcinViewFilter, null);
                list3 = list3.Where((V_HIS_MEDICINE_TYPE_ACIN o) => o.IS_APPROVAL_REQUIRED == 1).ToList();
                ProcessDataToSend(serviceReqADO, list3, list2, ref ado);
                list4.Add(ado);
                PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.AntibioticRequest", currentModule.RoomId, currentModule.RoomTypeId, list4);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void ProcessDataToSend(HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data, List<V_HIS_MEDICINE_TYPE_ACIN> lstMedicineTypeAcin, List<V_HIS_EXP_MEST_MEDICINE> lstExpMestMedicine, ref AntibioticRequestADO ado)
        {
            //IL_015e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0164: Expected O, but got Unknown
            try
            {
                ado.PatientCode = data.TDL_PATIENT_CODE;
                ado.PatientName = data.TDL_PATIENT_NAME;
                ado.Dob = data.TDL_PATIENT_DOB;
                ado.IsHasNotDayDob = data.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == 1;
                ado.GenderName = data.TDL_PATIENT_GENDER_NAME;
                ado.IcdSubCode = data.ICD_SUB_CODE;
                ado.IcdText = data.ICD_TEXT;
                HisDhstFilter hisDhstFilter = new HisDhstFilter();
                hisDhstFilter.TREATMENT_ID = data.TREATMENT_ID;
                List<HIS_DHST> list = new BackendAdapter(new CommonParam()).Get<List<HIS_DHST>>("api/HisDhst/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisDhstFilter, null);
                if (list != null && list.Count() > 0)
                {
                    HIS_DHST hIS_DHST = list.OrderByDescending((HIS_DHST o) => o.EXECUTE_TIME ?? 0).FirstOrDefault();
                    ado.Temperature = hIS_DHST.TEMPERATURE;
                    ado.Weight = hIS_DHST.WEIGHT;
                    ado.Height = hIS_DHST.HEIGHT;
                }
                ado.NewRegimen = new List<HIS_ANTIBIOTIC_NEW_REG>();
                foreach (V_HIS_MEDICINE_TYPE_ACIN item in lstMedicineTypeAcin)
                {
                    HIS_ANTIBIOTIC_NEW_REG val = new HIS_ANTIBIOTIC_NEW_REG();
                    Func<V_HIS_EXP_MEST_MEDICINE, bool> predicate = (V_HIS_EXP_MEST_MEDICINE o) => o.TDL_MEDICINE_TYPE_ID == item.MEDICINE_TYPE_ID;
                    V_HIS_EXP_MEST_MEDICINE v_HIS_EXP_MEST_MEDICINE = lstExpMestMedicine.Where(predicate).FirstOrDefault();
                    val.DOSAGE = v_HIS_EXP_MEST_MEDICINE.TUTORIAL;
                    val.USE_FORM = v_HIS_EXP_MEST_MEDICINE.MEDICINE_USE_FORM_NAME;
                    val.USE_DAY = v_HIS_EXP_MEST_MEDICINE.USE_TIME_TO - data.INTRUCTION_TIME;
                    val.ACTIVE_INGREDIENT_ID = item.ACTIVE_INGREDIENT_ID;
                    val.CONCENTRA = v_HIS_EXP_MEST_MEDICINE.CONCENTRA;
                    ado.NewRegimen.Add(val);
                }
                ado.processType = AntibioticRequestADO.ProcessType.Request;
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void GiayDeNghiDoiTraDichVu()
        {
            //IL_0046: Unknown result type (might be due to invalid IL or missing references)
            //IL_004c: Expected O, but got Unknown
            //IL_0059: Unknown result type (might be due to invalid IL or missing references)
            //IL_0063: Expected O, but got Unknown
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    printChangeServiceId = serviceReqADO.ID;
                    RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), PrintStoreLocation.PrintTemplatePath);
                    val.RunPrintTemplate("Mps000433", new DelegateRunPrinter(DelegateRunPrinter));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void ShowEmrDocumentList()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO == null || serviceReqADO.ID <= 0)
                {
                    return;
                }
                WaitingManager.Show();
                Inventec.Desktop.Common.Modules.Module module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == "EMR.Desktop.Plugins.SignedDocument").FirstOrDefault();
                if (module == null)
                {
                    throw new NullReferenceException("Not found module by ModuleLink = 'EMR.Desktop.Plugins.SignedDocument'");
                }
                if (module.IsPlugin && module.ExtensionInfo != null)
                {
                    List<object> list = new List<object>();
                    EmrDocumentInfoADO emrDocumentInfoADO = new EmrDocumentInfoADO();
                    emrDocumentInfoADO.TreatmentCode = serviceReqADO.TDL_TREATMENT_CODE;
                    emrDocumentInfoADO.HisCode = "SERVICE_REQ_CODE:" + serviceReqADO.SERVICE_REQ_CODE;
                    list.Add(emrDocumentInfoADO);
                    list.Add(new RefeshReference(FillDataToGrid));
                    object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), list);
                    if (pluginInstance == null)
                    {
                        throw new ArgumentNullException("extenceInstance is null");
                    }
                    WaitingManager.Hide();
                    ((Form)pluginInstance).ShowDialog();
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void UpdateSampleType()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    if (serviceReqADO != null && serviceReqADO.ID > 0)
                    {
                        HIS_SERVICE_REQ hIS_SERVICE_REQ = new HIS_SERVICE_REQ();
                        DataObjectMapper.Map<HIS_SERVICE_REQ>(hIS_SERVICE_REQ, serviceReqADO);
                        List<object> list = new List<object>();
                        list.Add(hIS_SERVICE_REQ);
                        list.Add(new RefeshReference(RefreshClick));
                        CallModule("HIS.Desktop.Plugins.ServiceReqUpdateSampleType", list);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void Btn_LayMauBenhPham_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO == null || serviceReqADO.ID <= 0)
                {
                    return;
                }
                Inventec.Desktop.Common.Modules.Module module;
                if (HisConfigCFG.IsUseInventecLis)
                {
                    if (serviceReqADO.LIS_STT_ID.HasValue || serviceReqADO.IS_SENT_EXT.HasValue)
                    {
                        module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == "LIS.Desktop.Plugins.SampleInfo").FirstOrDefault();
                        if (module == null)
                        {
                            LogSystem.Error("khong tim thay moduleLink = LIS.Desktop.Plugins.SampleInfo");
                        }
                        if (module.IsPlugin && module.ExtensionInfo != null)
                        {
                            List<object> list = new List<object>();
                            list.Add(serviceReqADO.SERVICE_REQ_CODE);
                            list.Add(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId));
                            object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), list);
                            if (pluginInstance == null)
                            {
                                throw new ArgumentNullException("moduleData is null");
                            }
                            ((Form)pluginInstance).ShowDialog();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Bệnh nhân đang nợ viện phí cần thực hiện thanh toán hoặc tạm ứng dịch vụ mới được lấy mẫu bệnh phẩm");
                    }
                    return;
                }
                module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == "HIS.Desktop.Plugins.ServiceReqSampleInfo").FirstOrDefault();
                if (module == null)
                {
                    LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.ServiceReqSampleInfo");
                }
                if (module.IsPlugin && module.ExtensionInfo != null)
                {
                    List<object> list = new List<object>();
                    list.Add(serviceReqADO.ID);
                    list.Add(new RefeshReference(FillDataToGrid));
                    list.Add(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId));
                    object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), list);
                    if (pluginInstance == null)
                    {
                        throw new ArgumentNullException("moduleData is null");
                    }
                    ((Form)pluginInstance).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void Btn_DrugInterventionInfo_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO == null || serviceReqADO.ID <= 0)
                {
                    return;
                }
                Inventec.Desktop.Common.Modules.Module module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == "HIS.Desktop.Plugins.DrugInterventionInfo").FirstOrDefault();
                if (module == null)
                {
                    LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.DrugInterventionInfo");
                }
                if (module.IsPlugin && module.ExtensionInfo != null)
                {
                    List<object> list = new List<object>();
                    list.Add(serviceReqADO.ID);
                    list.Add(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId));
                    object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), list);
                    if (pluginInstance == null)
                    {
                        throw new ArgumentNullException("moduleData is null");
                    }
                    ((Form)pluginInstance).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void Btn_ChangeRoom_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                Mapper.CreateMap<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO, L_HIS_SERVICE_REQ>();
                L_HIS_SERVICE_REQ val = Mapper.Map<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO, L_HIS_SERVICE_REQ>(serviceReqADO);
                if (serviceReqADO == null || serviceReqADO.ID <= 0)
                {
                    return;
                }
                if (val.SERVICE_REQ_STT_ID == 3)
                {
                    if (XtraMessageBox.Show("Chỉ định " + val.SERVICE_REQ_CODE + " đã kết thúc, không cho phép chuyển phòng", ResourceMessage.ThongBao, MessageBoxButtons.OK) != DialogResult.OK)
                    {
                    }
                    return;
                }
                Inventec.Desktop.Common.Modules.Module module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == "HIS.Desktop.Plugins.ChangeExamRoomProcess").FirstOrDefault();
                if (module == null)
                {
                    LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.ChangeExamRoomProcess");
                }
                if (module.IsPlugin && module.ExtensionInfo != null)
                {
                    List<object> list = new List<object>();
                    list.Add(val);
                    list.Add(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId));
                    object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), list);
                    if (pluginInstance == null)
                    {
                        throw new ArgumentNullException("moduleData is null");
                    }
                    ((Form)pluginInstance).ShowDialog();
                    btnFind_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void Btn_AllowNotExecute_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReq = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    frmReason frmReason2 = new frmReason(currentModule, serviceReq);
                    frmReason2.ShowDialog();
                    btnFind_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void Btn_DisposeAllowNotExecute_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO != null && serviceReqADO.ID > 0)
                {
                    bool value = false;
                    CommonParam commonParam = new CommonParam();
                    HisServiceReqAcceptNoExecuteSDO hisServiceReqAcceptNoExecuteSDO = new HisServiceReqAcceptNoExecuteSDO();
                    hisServiceReqAcceptNoExecuteSDO.ServiceReqId = serviceReqADO.ID;
                    hisServiceReqAcceptNoExecuteSDO.WorkingRoomId = currentModule.RoomId;
                    HIS_SERVICE_REQ hIS_SERVICE_REQ = new BackendAdapter(commonParam).Post<HIS_SERVICE_REQ>("api/HisSereServ/UnacceptNoExecuteByServiceReq", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqAcceptNoExecuteSDO, commonParam);
                    if (hIS_SERVICE_REQ != null)
                    {
                        value = true;
                    }
                    MessageManager.Show(this, commonParam, value);
                    btnFind_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void Btn_SeqUpdateNoExecute_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                WaitingManager.Show();
                CommonParam commonParam = new CommonParam();
                bool value = false;
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO != null && serviceReqADO.ID > 0)
                {
                    HisSereServNoExecuteSDO hisSereServNoExecuteSDO = new HisSereServNoExecuteSDO();
                    hisSereServNoExecuteSDO.IsNoExecute = ((serviceReqADO.IS_NO_EXECUTE != 1) ? true : false);
                    hisSereServNoExecuteSDO.RequestRoomId = currentModule.RoomId;
                    hisSereServNoExecuteSDO.TreatmentId = serviceReqADO.TREATMENT_ID;
                    hisSereServNoExecuteSDO.ServiceReqIds = new List<long>();
                    hisSereServNoExecuteSDO.ServiceReqIds.Add(serviceReqADO.ID);
                    List<HIS_SERE_SERV> list = new BackendAdapter(commonParam).Post<List<HIS_SERE_SERV>>("api/HisSereServ/UpdateNoExecute", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServNoExecuteSDO, commonParam);
                    if (list != null && list.Count > 0)
                    {
                        value = true;
                        btnFind_Click(null, null);
                    }
                }
                WaitingManager.Hide();
                MessageManager.Show(base.ParentForm, commonParam, value);
                SessionManager.ProcessTokenLost(commonParam);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Btn_EnterInforBeforeSurgery_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    if (serviceReqADO != null)
                    {
                        HIS_SERVICE_REQ hIS_SERVICE_REQ = new HIS_SERVICE_REQ();
                        hIS_SERVICE_REQ = serviceReqADO;
                        List<object> list = new List<object>();
                        list.Add(hIS_SERVICE_REQ);
                        list.Add(new RefeshReference(RefreshClick));
                        CallModule("HIS.Desktop.Plugins.EnterInforBeforeSurgery", list);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Btn_SendOldSystemIntegration_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    if (serviceReqADO != null)
                    {
                        CommonParam commonParam = new CommonParam();
                        bool flag = false;
                        flag = new BackendAdapter(commonParam).Post<bool>("api/HisServiceReq/SendToOldSystem", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, serviceReqADO.ID, new Action(SessionManager.ActionLostToken), commonParam);
                        MessageManager.Show(this, commonParam, flag);
                        SessionManager.ProcessTokenLost(commonParam);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void Btn_OpenAttachFile_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    if (serviceReqADO != null)
                    {
                        AttachFileADO attachFileADO = new AttachFileADO();
                        attachFileADO.AttachFileUrl = serviceReqADO.ATTACHMENT_FILE_URL;
                        attachFileADO.FileType = "Pdf";
                        AttachFileADO item = attachFileADO;
                        List<object> list = new List<object>();
                        list.Add(item);
                        List<object> data = list;
                        CallModule("HIS.Desktop.Plugins.AttachFileViewer", data);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void Btn_BieuMauKhacV2_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    if (serviceReqADO != null)
                    {
                        List<object> list = new List<object>();
                        list.Add(serviceReqADO.ID);
                        List<object> data = list;
                        CallModule("HIS.Desktop.Plugins.OtherFormAssService", data);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void SendResultToPacs(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                CommonParam commonParam = new CommonParam();
                bool flag = false;
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (data == null)
                {
                    LogSystem.Info("Data thuc hien gui lai yeu cau sang PACS: " + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data));
                    return;
                }
                WaitingManager.Show();
                flag = new BackendAdapter(commonParam).Post<bool>("api/HisServiceReq/RequestOrder", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, data.ID, new Action(SessionManager.ActionLostToken), commonParam);
                WaitingManager.Hide();
                if (flag)
                {
                    FillDataToGrid();
                }
                MessageManager.Show(this, commonParam, flag);
                SessionManager.ProcessTokenLost(commonParam);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                WaitingManager.Hide();
            }
        }

        private void GuiLaiXNSangLIS(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                CommonParam commonParam = new CommonParam();
                bool flag = false;
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (data == null)
                {
                    LogSystem.Info("Data thuc hien gui lai yeu cau XN sang LIS: " + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data));
                    return;
                }
                WaitingManager.Show();
                flag = new BackendAdapter(commonParam).Post<bool>("api/HisTestServiceReq/RequestOrder", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, data.ID, new Action(SessionManager.ActionLostToken), commonParam);
                WaitingManager.Hide();
                if (flag)
                {
                    FillDataToGrid();
                }
                MessageManager.Show(this, commonParam, flag);
                SessionManager.ProcessTokenLost(commonParam);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                WaitingManager.Hide();
            }
        }

        private void btnPrintMedicine_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (gridControlServiceReq.DataSource == null)
                    {
                        return;
                    }
                    List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = (List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>)gridControlServiceReq.DataSource;
                    if (list.Count > 0)
                    {
                        listServiceReq = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                        foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in list)
                        {
                            if (item.isCheck)
                            {
                                listServiceReq.Add(item);
                            }
                        }
                        if (listServiceReq != null && listServiceReq.Count > 0)
                        {
                            CommonParam commonParam = new CommonParam();
                            if (CheckListServiceReqMedicine(listServiceReq, commonParam))
                            {
                                MessageManager.Show(this, commonParam, false);
                            }
                            else
                            {
                                InDonThuocTongHop("Mps000234");
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(ResourceMessage.BanChuaChonDichVu);
                    }
                }
                catch (Exception ex)
                {
                    LogSystem.Error(ex);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void Btn_ExamMain_ButtonClick()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                WaitingManager.Show();
                CommonParam commonParam = new CommonParam();
                bool value = false;
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO != null && serviceReqADO.ID > 0)
                {
                    HIS_SERVICE_REQ hIS_SERVICE_REQ = new BackendAdapter(commonParam).Post<HIS_SERVICE_REQ>("api/HisServiceReq/ChangeMain", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, serviceReqADO.ID, commonParam);
                    if (hIS_SERVICE_REQ != null && hIS_SERVICE_REQ.ID > 0)
                    {
                        value = true;
                        btnFind_Click(null, null);
                    }
                }
                WaitingManager.Hide();
                MessageManager.Show(base.ParentForm, commonParam, value);
                SessionManager.ProcessTokenLost(commonParam);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void MouseRight_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e.Item is BarButtonItem && rightClickData != null)
                {
                    BarButtonItem barButtonItem = sender as BarButtonItem;
                    switch ((PopupMenuProcessorMedicine.ItemType)e.Item.Tag)
                    {
                        case PopupMenuProcessorMedicine.ItemType.SuaHuongDanSuDung:
                            SuaHuongDanSuDung(rightClickData);
                            LogSystem.Warn("log 1");
                            break;
                        case PopupMenuProcessorMedicine.ItemType.AssignPres:
                            AssignPreClick();
                            break;
                        case PopupMenuProcessorMedicine.ItemType.AssignPresCabinet:
                            AssignPreCabinetClick();
                            break;
                        case PopupMenuProcessorMedicine.ItemType.PrintServiceReq:
                            PrintServiceReqBySelectedService();
                            break;
                        case PopupMenuProcessorMedicine.ItemType.AssignInKip:
                            AssignInKip();
                            break;
                        case PopupMenuProcessorMedicine.ItemType.AssignOutKip:
                            AssignOutKip();
                            break;
                        case PopupMenuProcessorMedicine.ItemType.AcceptNoExecute:
                            AcceptNoExecute();
                            break;
                        case PopupMenuProcessorMedicine.ItemType.UnacceptNoExecute:
                            UnacceptNoExecute(rightClickData);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void UnacceptNoExecute(ListMedicineADO data)
        {
            try
            {
                bool value = false;
                CommonParam commonParam = new CommonParam();
                HisSereServAcceptNoExecuteSDO hisSereServAcceptNoExecuteSDO = new HisSereServAcceptNoExecuteSDO();
                hisSereServAcceptNoExecuteSDO.SereServId = data.ID;
                hisSereServAcceptNoExecuteSDO.WorkingRoomId = currentModule.RoomId;
                HIS_SERE_SERV result = new BackendAdapter(commonParam).Post<HIS_SERE_SERV>("api/HisSereServ/UnacceptNoExecute", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServAcceptNoExecuteSDO, commonParam);
                if (result != null && result.IS_ACCEPTING_NO_EXECUTE != 1)
                {
                    value = true;
                    foreach (ListMedicineADO item in _listMedicine)
                    {
                        ListMedicineADO listMedicineADO = _listMedicine.FirstOrDefault((ListMedicineADO o) => o.ID == result.ID);
                        if (listMedicineADO != null)
                        {
                            ((HIS_SERE_SERV)_listMedicine.FirstOrDefault((ListMedicineADO o) => o.ID == result.ID)).IS_ACCEPTING_NO_EXECUTE = result.IS_ACCEPTING_NO_EXECUTE;
                        }
                    }
                    grdViewSereServServiceReq.BeginUpdate();
                    grdViewSereServServiceReq.GridControl.DataSource = _listMedicine;
                    grdViewSereServServiceReq.EndUpdate();
                }
                MessageManager.Show(this, commonParam, value);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void AcceptNoExecute()
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReq = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    ListMedicineADO listMedicineADO = (ListMedicineADO)grdViewSereServServiceReq.GetFocusedRow();
                    if (listMedicineADO != null)
                    {
                        frmReason frmReason2 = new frmReason(currentModule, serviceReq, listMedicineADO);
                        frmReason2.ShowDialog();
                    }
                    else
                    {
                        frmReason frmReason2 = new frmReason(currentModule, serviceReq);
                        frmReason2.ShowDialog();
                    }
                    btnFind_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void RefreshHuongDanSuDung()
        {
            try
            {
                ControlServiceReqClick(currentServiceReq);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void AssignPreClick()
        {
            try
            {
                bool flag = currentServiceReq.IS_EXECUTE_KIDNEY_PRES == 1;
                string moduleLinkAssignPres = (flag ? "HIS.Desktop.Plugins.AssignPrescriptionKidney" : "HIS.Desktop.Plugins.AssignPrescriptionPK");
                Inventec.Desktop.Common.Modules.Module module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == moduleLinkAssignPres).FirstOrDefault();
                if (module == null)
                {
                    LogSystem.Error("khong tim thay moduleLink = " + moduleLinkAssignPres);
                }
                if (!module.IsPlugin || module.ExtensionInfo == null)
                {
                    return;
                }
                List<object> list = new List<object>();
                Inventec.Desktop.Common.Modules.Module module2 = new Inventec.Desktop.Common.Modules.Module();
                long intructionTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now) ?? 0;
                Mapper.CreateMap<ListMedicineADO, V_HIS_SERE_SERV>();
                V_HIS_SERE_SERV v_HIS_SERE_SERV = Mapper.Map<ListMedicineADO, V_HIS_SERE_SERV>(rightClickData);
                if (flag)
                {
                    AssignPrescriptionKidneyADO assignPrescriptionKidneyADO = new AssignPrescriptionKidneyADO();
                    assignPrescriptionKidneyADO.AssignPrescriptionEditADO = new AssignPrescriptionEditADO(currentServiceReq, null);
                    HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
                    hisExpMestFilter.SERVICE_REQ_ID = currentServiceReq.ID;
                    List<HIS_EXP_MEST> list2 = new BackendAdapter(new CommonParam()).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, new Action(SessionManager.ActionLostToken), null);
                    if (list2 != null && list2.Count > 0)
                    {
                        assignPrescriptionKidneyADO.AssignPrescriptionEditADO.ExpMest = list2.First();
                    }
                    CommonParam commonParam = new CommonParam();
                    HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
                    hisTreatmentFilter.ID = currentServiceReq.TREATMENT_ID;
                    List<HIS_TREATMENT> list3 = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, commonParam);
                    if (list3 != null && list3.Count > 0)
                    {
                        if (list3.FirstOrDefault().TDL_TREATMENT_TYPE_ID == 3)
                        {
                            assignPrescriptionKidneyADO.IsAutoCheckExpend = true;
                        }
                        if (list3.FirstOrDefault().TDL_TREATMENT_TYPE_ID == 2)
                        {
                            assignPrescriptionKidneyADO.IsAutoCheckExpend = true;
                        }
                    }
                    list.Add(assignPrescriptionKidneyADO);
                }
                else
                {
                    AssignPrescriptionADO assignPrescriptionADO = new AssignPrescriptionADO(currentServiceReq.TREATMENT_ID, intructionTime, currentServiceReq.ID, v_HIS_SERE_SERV, null);
                    assignPrescriptionADO.GenderName = currentServiceReq.TDL_PATIENT_GENDER_NAME;
                    assignPrescriptionADO.PatientName = currentServiceReq.TDL_PATIENT_NAME;
                    assignPrescriptionADO.PatientDob = currentServiceReq.TDL_PATIENT_DOB;
                    CommonParam commonParam = new CommonParam();
                    HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
                    hisTreatmentFilter.ID = currentServiceReq.TREATMENT_ID;
                    List<HIS_TREATMENT> list3 = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, commonParam);
                    if (list3 != null && list3.Count > 0)
                    {
                        if (list3.FirstOrDefault().TDL_TREATMENT_TYPE_ID == 3)
                        {
                            assignPrescriptionADO.IsExecutePTTT = true;
                            assignPrescriptionADO.IsAutoCheckExpend = true;
                        }
                        if (list3.FirstOrDefault().TDL_TREATMENT_TYPE_ID == 2)
                        {
                            assignPrescriptionADO.IsExecutePTTT = true;
                            assignPrescriptionADO.IsAutoCheckExpend = true;
                        }
                    }
                    list.Add(assignPrescriptionADO);
                }
                object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), list);
                if (pluginInstance == null)
                {
                    throw new ArgumentNullException("moduleData is null");
                }
                ((Form)pluginInstance).ShowDialog();
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void AssignPreCabinetClick()
        {
            try
            {
                Inventec.Desktop.Common.Modules.Module module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == "HIS.Desktop.Plugins.AssignPrescriptionPK").FirstOrDefault();
                if (module == null)
                {
                    LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.AssignPrescriptionPK");
                }
                if (module.IsPlugin && module.ExtensionInfo != null)
                {
                    List<object> list = new List<object>();
                    Inventec.Desktop.Common.Modules.Module module2 = new Inventec.Desktop.Common.Modules.Module();
                    long intructionTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now) ?? 0;
                    Mapper.CreateMap<ListMedicineADO, V_HIS_SERE_SERV>();
                    V_HIS_SERE_SERV v_HIS_SERE_SERV = Mapper.Map<ListMedicineADO, V_HIS_SERE_SERV>(rightClickData);
                    AssignPrescriptionADO assignPrescriptionADO = new AssignPrescriptionADO(currentServiceReq.TREATMENT_ID, intructionTime, currentServiceReq.ID, v_HIS_SERE_SERV, null);
                    assignPrescriptionADO.GenderName = currentServiceReq.TDL_PATIENT_GENDER_NAME;
                    assignPrescriptionADO.PatientName = currentServiceReq.TDL_PATIENT_NAME;
                    assignPrescriptionADO.PatientDob = currentServiceReq.TDL_PATIENT_DOB;
                    assignPrescriptionADO.IsCabinet = true;
                    assignPrescriptionADO.IsAutoCheckExpend = true;
                    list.Add(assignPrescriptionADO);
                    object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), list);
                    if (pluginInstance == null)
                    {
                        throw new ArgumentNullException("moduleData is null");
                    }
                    ((Form)pluginInstance).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void SuaHuongDanSuDung(ListMedicineADO data)
        {
            try
            {
                frmTutorial frmTutorial2 = new frmTutorial(new DelegateRefreshData(RefreshHuongDanSuDung), data);
                frmTutorial2.ShowDialog();
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void AssignOutKip()
        {
            try
            {
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                ListMedicineADO source = (ListMedicineADO)grdViewSereServServiceReq.GetFocusedRow();
                AlertHospitalFeeNotBHYTManager alertHospitalFeeNotBHYTManager = new AlertHospitalFeeNotBHYTManager();
                if (!alertHospitalFeeNotBHYTManager.Run(serviceReqADO.TREATMENT_ID, ((HIS_SERVICE_REQ)serviceReqADO).TDL_PATIENT_TYPE_ID ?? 0, currentModule.RoomId))
                {
                    return;
                }
                Inventec.Desktop.Common.Modules.Module module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == "HIS.Desktop.Plugins.AssignService").FirstOrDefault();
                if (module == null)
                {
                    LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.AssignService");
                }
                if (module.IsPlugin && module.ExtensionInfo != null)
                {
                    List<object> list = new List<object>();
                    Inventec.Desktop.Common.Modules.Module module2 = new Inventec.Desktop.Common.Modules.Module();
                    long intructionTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now) ?? 0;
                    Mapper.CreateMap<ListMedicineADO, V_HIS_SERE_SERV>();
                    V_HIS_SERE_SERV v_HIS_SERE_SERV = Mapper.Map<ListMedicineADO, V_HIS_SERE_SERV>(source);
                    AssignServiceADO assignServiceADO = new AssignServiceADO(serviceReqADO.TREATMENT_ID, intructionTime, serviceReqADO.ID, v_HIS_SERE_SERV, new AssignServiceADO.DelegateProcessDataResult(LoadSereServOutKipResult), false);
                    assignServiceADO.GenderName = serviceReqADO.TDL_PATIENT_GENDER_NAME;
                    assignServiceADO.PatientName = serviceReqADO.TDL_PATIENT_NAME;
                    assignServiceADO.PatientDob = serviceReqADO.TDL_PATIENT_DOB;
                    assignServiceADO.IsNotUseBhyt = ((HIS_SERVICE_REQ)serviceReqADO).IS_NOT_USE_BHYT == 1;
                    list.Add(assignServiceADO);
                    object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), list);
                    if (pluginInstance == null)
                    {
                        throw new ArgumentNullException("moduleData is null");
                    }
                    ((Form)pluginInstance).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private List<HisEkipUserADO> GetEkipUserADO()
        {
            List<HisEkipUserADO> list = new List<HisEkipUserADO>();
            try
            {
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                ListMedicineADO listMedicineADO = (ListMedicineADO)grdViewSereServServiceReq.GetFocusedRow();
                if (listMedicineADO != null)
                {
                    CommonParam commonParam = new CommonParam();
                    if (listMedicineADO.EKIP_ID.HasValue)
                    {
                        HisEkipUserViewFilter hisEkipUserViewFilter = new HisEkipUserViewFilter();
                        hisEkipUserViewFilter.EKIP_ID = listMedicineADO.EKIP_ID;
                        List<V_HIS_EKIP_USER> list2 = new BackendAdapter(commonParam).Get<List<V_HIS_EKIP_USER>>("api/HisEkipUser/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisEkipUserViewFilter, commonParam);
                        if (list2.Count > 0)
                        {
                            foreach (V_HIS_EKIP_USER item in list2)
                            {
                                List<HIS_EXECUTE_ROLE> source = BackendDataWorker.Get<HIS_EXECUTE_ROLE>();
                                Func<HIS_EXECUTE_ROLE, bool> predicate = (HIS_EXECUTE_ROLE p) => p.ID == item.EXECUTE_ROLE_ID && p.IS_ACTIVE == 1;
                                HIS_EXECUTE_ROLE hIS_EXECUTE_ROLE = source.FirstOrDefault(predicate);
                                if (hIS_EXECUTE_ROLE != null && hIS_EXECUTE_ROLE.ID != 0)
                                {
                                    Mapper.CreateMap<V_HIS_EKIP_USER, HisEkipUserADO>();
                                    HisEkipUserADO item2 = Mapper.Map<V_HIS_EKIP_USER, HisEkipUserADO>(item);
                                    list.Add(item2);
                                }
                            }
                        }
                    }
                    else if (serviceReqADO.EKIP_PLAN_ID.HasValue)
                    {
                        HisEkipPlanUserViewFilter hisEkipPlanUserViewFilter = new HisEkipPlanUserViewFilter();
                        hisEkipPlanUserViewFilter.EKIP_PLAN_ID = serviceReqADO.EKIP_PLAN_ID;
                        List<V_HIS_EKIP_PLAN_USER> list3 = new BackendAdapter(commonParam).Get<List<V_HIS_EKIP_PLAN_USER>>("api/HisEkipPlanUser/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisEkipPlanUserViewFilter, commonParam);
                        if (list3.Count > 0)
                        {
                            foreach (V_HIS_EKIP_PLAN_USER item3 in list3)
                            {
                                List<HIS_EXECUTE_ROLE> source2 = BackendDataWorker.Get<HIS_EXECUTE_ROLE>();
                                Func<HIS_EXECUTE_ROLE, bool> predicate2 = (HIS_EXECUTE_ROLE p) => p.ID == item3.EXECUTE_ROLE_ID && p.IS_ACTIVE == 1;
                                HIS_EXECUTE_ROLE hIS_EXECUTE_ROLE = source2.FirstOrDefault(predicate2);
                                if (hIS_EXECUTE_ROLE != null && hIS_EXECUTE_ROLE.ID != 0)
                                {
                                    Mapper.CreateMap<V_HIS_EKIP_PLAN_USER, HisEkipUserADO>();
                                    HisEkipUserADO item2 = Mapper.Map<V_HIS_EKIP_PLAN_USER, HisEkipUserADO>(item3);
                                    list.Add(item2);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = new List<HisEkipUserADO>();
                LogSystem.Warn(ex);
            }
            return list;
        }

        private void AssignInKip()
        {
            try
            {
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                ListMedicineADO source = (ListMedicineADO)grdViewSereServServiceReq.GetFocusedRow();
                List<HisEkipUserADO> ekipUserADO = GetEkipUserADO();
                if ((ekipUserADO == null || ekipUserADO.Count <= 0) && 0 == 0)
                {
                    MessageBox.Show("Chưa có thông tin kíp thực hiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                AlertHospitalFeeNotBHYTManager alertHospitalFeeNotBHYTManager = new AlertHospitalFeeNotBHYTManager();
                if (!alertHospitalFeeNotBHYTManager.Run(serviceReqADO.TREATMENT_ID, ((HIS_SERVICE_REQ)serviceReqADO).TDL_PATIENT_TYPE_ID ?? 0, currentModule.RoomId))
                {
                    return;
                }
                Inventec.Desktop.Common.Modules.Module module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == "HIS.Desktop.Plugins.AssignService").FirstOrDefault();
                if (module == null)
                {
                    LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.AssignService");
                }
                if (module.IsPlugin && module.ExtensionInfo != null)
                {
                    List<object> list = new List<object>();
                    Inventec.Desktop.Common.Modules.Module module2 = new Inventec.Desktop.Common.Modules.Module();
                    long intructionTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now) ?? 0;
                    Mapper.CreateMap<ListMedicineADO, V_HIS_SERE_SERV>();
                    V_HIS_SERE_SERV v_HIS_SERE_SERV = Mapper.Map<ListMedicineADO, V_HIS_SERE_SERV>(source);
                    AssignServiceADO assignServiceADO = new AssignServiceADO(serviceReqADO.TREATMENT_ID, intructionTime, serviceReqADO.ID, v_HIS_SERE_SERV, new AssignServiceADO.DelegateProcessDataResult(LoadSereServInKipResult), true);
                    assignServiceADO.GenderName = serviceReqADO.TDL_PATIENT_GENDER_NAME;
                    assignServiceADO.PatientName = serviceReqADO.TDL_PATIENT_NAME;
                    assignServiceADO.PatientDob = serviceReqADO.TDL_PATIENT_DOB;
                    assignServiceADO.IsNotUseBhyt = ((HIS_SERVICE_REQ)serviceReqADO).IS_NOT_USE_BHYT == 1;
                    list.Add(assignServiceADO);
                    object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), list);
                    if (pluginInstance == null)
                    {
                        throw new ArgumentNullException("moduleData is null");
                    }
                    ((Form)pluginInstance).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void LoadSereServInKipResult(object data)
        {
        }

        private void LoadSereServOutKipResult(object data)
        {
            try
            {
                if (data == null)
                {
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void btnPrintNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridControlServiceReq.DataSource == null)
                {
                    return;
                }
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = (List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>)gridControlServiceReq.DataSource;
                if (list.Count > 0)
                {
                    listServiceReq = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                    foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in list)
                    {
                        if (item.isCheck)
                        {
                            listServiceReq.Add(item);
                        }
                    }
                    if (listServiceReq != null && listServiceReq.Count > 0)
                    {
                        CommonParam commonParam = new CommonParam();
                        if (CheckListServiceReqV2(listServiceReq, commonParam))
                        {
                            MessageManager.Show(this, commonParam, false);
                            return;
                        }
                        List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> serviceReqDv = listServiceReq.Where((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.SERVICE_REQ_TYPE_ID != 6 && o.SERVICE_REQ_TYPE_ID != 15 && o.SERVICE_REQ_TYPE_ID != 14 && o.SERVICE_REQ_TYPE_ID != 16).ToList();
                        List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list2 = listServiceReq.Where((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => !serviceReqDv.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO s) => s.ID).Contains(o.ID)).ToList();
                        if (list2 != null && list2.Count > 0)
                        {
                            foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item2 in list2)
                            {
                                ExecuteBefPrint(item2);
                            }
                        }
                        if (serviceReqDv != null && serviceReqDv.Count > 0)
                        {
                            List<V_HIS_BED_LOG> bedLogs = new List<V_HIS_BED_LOG>();
                            HisBedLogViewFilter hisBedLogViewFilter = new HisBedLogViewFilter();
                            hisBedLogViewFilter.TREATMENT_ID = serviceReqDv.First().TREATMENT_ID;
                            List<V_HIS_BED_LOG> list3 = new BackendAdapter(commonParam).Get<List<V_HIS_BED_LOG>>("api/HisBedLog/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisBedLogViewFilter, commonParam);
                            if (list3 != null)
                            {
                                bedLogs = list3;
                            }
                            HisTreatmentWithPatientTypeInfoFilter hisTreatmentWithPatientTypeInfoFilter = new HisTreatmentWithPatientTypeInfoFilter();
                            hisTreatmentWithPatientTypeInfoFilter.TREATMENT_ID = serviceReqDv.First().TREATMENT_ID;
                            hisTreatmentWithPatientTypeInfoFilter.INTRUCTION_TIME = serviceReqDv.Min((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.INTRUCTION_TIME);
                            List<HisTreatmentWithPatientTypeInfoSDO> source = new BackendAdapter(commonParam).Get<List<HisTreatmentWithPatientTypeInfoSDO>>("api/HisTreatment/GetTreatmentWithPatientTypeInfoSdo", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentWithPatientTypeInfoFilter, new Action(base.ProcessLostToken), commonParam);
                            HisServiceReqListResultSDO hisServiceReqListResultSDO = new HisServiceReqListResultSDO();
                            HisServiceReqViewFilter hisServiceReqViewFilter = new HisServiceReqViewFilter();
                            hisServiceReqViewFilter.IDs = serviceReqDv.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO s) => s.ID).ToList();
                            hisServiceReqViewFilter.IS_ACTIVE = 1;
                            hisServiceReqListResultSDO.ServiceReqs = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERVICE_REQ>>("api/HisServiceReq/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqViewFilter, new Action(SessionManager.ActionLostToken), null);
                            HisSereServViewFilter hisSereServViewFilter = new HisSereServViewFilter();
                            hisSereServViewFilter.SERVICE_REQ_IDs = serviceReqDv.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO s) => s.ID).ToList();
                            hisSereServViewFilter.IS_ACTIVE = 1;
                            hisServiceReqListResultSDO.SereServs = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERE_SERV>>("api/HisSereServ/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServViewFilter, new Action(SessionManager.ActionLostToken), null);
                            PrintServiceReqProcessor printServiceReqProcessor = new PrintServiceReqProcessor(hisServiceReqListResultSDO, source.FirstOrDefault(), bedLogs, (currentModule != null) ? currentModule.RoomId : 0);
                            printServiceReqProcessor.Print("Mps000340", false);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(ResourceMessage.BanChuaChonDichVu);
                    }
                }
                else
                {
                    XtraMessageBox.Show(ResourceMessage.BanChuaChonDichVu);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GeneratePopupMenu()
        {
            try
            {
                DXPopupMenu dXPopupMenu = new DXPopupMenu();
                dXPopupMenu.Items.Add(new DXMenuItem("Phiếu in tổng hợp", new EventHandler(btnPrintNew_Click)));
                dXPopupMenu.Items.Add(new DXMenuItem("Phiếu hướng dẫn bệnh nhân thực hiện CLS", new EventHandler(onClickPhieuHuongDan)));
                dXPopupMenu.Items.Add(new DXMenuItem("In tem dự trù máu", new EventHandler(onClickInTemDuTruMau)));
                btnDropDownPrint.DropDownControl = dXPopupMenu;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void onClickInTemDuTruMau(object sender, EventArgs e)
        {
            try
            {
                WaitingManager.Show();
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = gridControlServiceReq.DataSource as List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>;
                if (list == null || list.Count <= 0)
                {
                    XtraMessageBox.Show("Không có chỉ định nào được lựa chọn", "Thông báo", DefaultBoolean.True);
                    return;
                }
                IEnumerable<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> enumerable = list.Where((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.SERVICE_REQ_TYPE_ID == 16 && o.isCheck);
                if (enumerable != null && enumerable.Count() > 0)
                {
                    InTemBarcodeMau(enumerable.ToList());
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void InTemBarcodeMau(List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> datas)
        {
            try
            {
                string txt = "";
                if (datas == null || datas.Count <= 0)
                {
                    return;
                }
                foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data in datas)
                {
                    LogSystem.Info("BN: " + data.SERVICE_REQ_CODE);
                    GenTextMau(data, ref txt);
                }
                string text = new PrintBloodServiceReq().StartPrintBloodServiceReq(txt);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    LogSystem.Warn(text);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void GenTextMau(HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReq, ref string txt)
        {
            HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
            hisExpMestFilter.SERVICE_REQ_ID = serviceReq.ID;
            List<HIS_EXP_MEST> list = new BackendAdapter(new CommonParam()).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, new CommonParam());
            if (list != null && list.Count > 0)
            {
                txt += list.FirstOrDefault().EXP_MEST_CODE;
            }
            txt = txt + "," + serviceReq.TDL_PATIENT_NAME;
            txt = txt + "," + serviceReq.TDL_PATIENT_GENDER_NAME;
            txt = txt + "," + serviceReq.TDL_PATIENT_DOB;
            txt = txt + "," + ((serviceReq.TDL_PATIENT_DOB > 10000000000000L) ? serviceReq.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "");
            txt = txt + "," + serviceReq.REQUEST_DEPARTMENT_NAME;
            txt = txt + "," + serviceReq.REQUEST_ROOM_NAME;
            txt = txt + "," + Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Inventec.Common.DateTime.Get.Now() ?? 0);
            txt = txt + "," + serviceReq.TDL_TREATMENT_CODE;
            txt += "\n";
        }

        private void onClickPhieuHuongDan(object sender, EventArgs e)
        {
            try
            {
                if (gridControlServiceReq.DataSource == null)
                {
                    return;
                }
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = (List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>)gridControlServiceReq.DataSource;
                if (list.Count > 0)
                {
                    listServiceReq = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                    List<V_HIS_SERVICE_REQ> list2 = new List<V_HIS_SERVICE_REQ>();
                    foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in list)
                    {
                        if (item.isCheck)
                        {
                            V_HIS_SERVICE_REQ v_HIS_SERVICE_REQ = new V_HIS_SERVICE_REQ();
                            DataObjectMapper.Map<V_HIS_SERVICE_REQ>(v_HIS_SERVICE_REQ, item);
                            list2.Add(v_HIS_SERVICE_REQ);
                            listServiceReq.Add(item);
                        }
                    }
                    if (listServiceReq != null && listServiceReq.Count > 0)
                    {
                        CommonParam commonParam = new CommonParam();
                        if (CheckListServiceReqV2(listServiceReq, commonParam))
                        {
                            MessageManager.Show(this, commonParam, false);
                            return;
                        }
                        PrintServiceReqTreatmentProcessor printServiceReqTreatmentProcessor = new PrintServiceReqTreatmentProcessor(list2, (currentModule != null) ? currentModule.RoomId : 0);
                        printServiceReqTreatmentProcessor.Print("Mps000276", false);
                    }
                    else
                    {
                        XtraMessageBox.Show(ResourceMessage.BanChuaChonDichVu);
                    }
                }
                else
                {
                    XtraMessageBox.Show(ResourceMessage.BanChuaChonDichVu);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void btnDropDownPrint_Click(object sender, EventArgs e)
        {
            btnDropDownPrint.ShowDropDown();
        }

        private void repositoryItemButton__BieuMauKhac_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                Btn_BieuMauKhacV2_ButtonClick();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void btnPrintTemBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnPrintTemBarcode.Enabled)
                {
                    return;
                }
                WaitingManager.Show();
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = gridControlServiceReq.DataSource as List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>;
                if (list == null || list.Count <= 0)
                {
                    XtraMessageBox.Show("Khong co Yêu cầu nào được chọn", "Thông báo", DefaultBoolean.True);
                    return;
                }
                IEnumerable<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> enumerable = list.Where((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.SERVICE_REQ_TYPE_ID == 2 && o.isCheck);
                if (enumerable != null && enumerable.Count() > 0)
                {
                    MOS.Filter.HisServiceReqViewFilter svFilter = new HisServiceReqViewFilter();
                    svFilter.IDs = enumerable.Select(o => o.ID).Distinct().ToList();
                    var ServiceReqList = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERVICE_REQ>>("api/HisServiceReq/GetView", ApiConsumer.ApiConsumers.MosConsumer, svFilter, new CommonParam());
                    InTemBarcodeXN(ServiceReqList);
                }
                IEnumerable<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> enumerable2 = list.Where((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.SERVICE_REQ_TYPE_ID == 16 && o.isCheck);
                if (enumerable2 != null && enumerable2.Count() > 0)
                {
                    InTemBarcodeMau(enumerable2.ToList());
                }
                IEnumerable<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> enumerable3 = list.Where((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.SERVICE_REQ_TYPE_ID == 13 && o.isCheck);
                if (enumerable3 != null && enumerable3.Count() > 0)
                {
                    InTemBarcodeGpbl(enumerable3.ToList());
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void InTemBarcodeGpbl(List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> datas)
        {
            try
            {
                string txt = "";
                string text = "";
                if (datas == null || datas.Count <= 0)
                {
                    return;
                }
                HisSereServFilter hisSereServFilter = new HisSereServFilter();
                hisSereServFilter.SERVICE_REQ_IDs = datas.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.ID).ToList();
                List<HIS_SERE_SERV> source = new BackendAdapter(param).Get<List<HIS_SERE_SERV>>("api/HisSereServ/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServFilter, param);
                foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in datas)
                {
                    LogSystem.Info("BN: " + item.SERVICE_REQ_CODE);
                    Func<HIS_SERE_SERV, bool> predicate = (HIS_SERE_SERV o) => o.SERVICE_REQ_ID == item.ID;
                    List<HIS_SERE_SERV> list = source.Where(predicate).ToList();
                    if (list != null && list.Count() > 0)
                    {
                        GenText_Gpbl(item, list.FirstOrDefault(), ref txt);
                    }
                }
                text = new PrintGpblServiceReq().StartPrintGpblServiceReq(txt);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    LogSystem.Warn(text);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }


        private void InTemBarcodeXN(List<V_HIS_SERVICE_REQ> datas)
        {
            try
            {
                string txt = "";
                if (datas == null || datas.Count <= 0)
                {
                    return;
                }
                HisSereServViewFilter hisSereServFilter = new HisSereServViewFilter();
                hisSereServFilter.SERVICE_REQ_IDs = datas.Select(o => o.ID).ToList();
                List<V_HIS_SERE_SERV> source = new BackendAdapter(param).Get<List<V_HIS_SERE_SERV>>("api/HisSereServ/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServFilter, param);
                ProcessPrint.PrintBarCode(false, datas, source);
                //string text = new PrintTestServiceReq().StartPrintTestServiceReq(txt);
                //if (!string.IsNullOrWhiteSpace(text))
                //{
                //    LogSystem.Warn(text);
                //}
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void GenText_Gpbl(HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReq, HIS_SERE_SERV sereServ, ref string txt)
        {
            HIS_SERVICE service = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault((HIS_SERVICE o) => o.ID == sereServ.SERVICE_ID);
            HIS_SERVICE hIS_SERVICE = null;
            if (service != null && service.PARENT_ID.HasValue && service.PARENT_ID.Value > 0)
            {
                hIS_SERVICE = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault((HIS_SERVICE o) => o.ID == service.PARENT_ID);
            }
            txt += serviceReq.SERVICE_REQ_CODE;
            txt = txt + "," + serviceReq.TDL_PATIENT_NAME;
            txt = txt + "," + serviceReq.TDL_PATIENT_GENDER_NAME;
            txt = txt + "," + serviceReq.TDL_PATIENT_DOB;
            txt = txt + "," + ((serviceReq.TDL_PATIENT_DOB > 10000000000000L) ? serviceReq.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "");
            txt = txt + "," + serviceReq.REQUEST_DEPARTMENT_NAME;
            txt = txt + "," + serviceReq.REQUEST_USERNAME;
            txt = txt + "," + Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Inventec.Common.DateTime.Get.Now() ?? 0);
            txt = txt + "," + serviceReq.TDL_TREATMENT_CODE;
            if (((HIS_SERVICE_REQ)serviceReq).TEST_SAMPLE_TYPE_ID.HasValue && ((HIS_SERVICE_REQ)serviceReq).TEST_SAMPLE_TYPE_ID.Value > 0)
            {
                HIS_TEST_SAMPLE_TYPE val = BackendDataWorker.Get<HIS_TEST_SAMPLE_TYPE>().FirstOrDefault((HIS_TEST_SAMPLE_TYPE o) => o.ID == ((HIS_SERVICE_REQ)serviceReq).TEST_SAMPLE_TYPE_ID);
                if (val != null)
                {
                    txt = txt + "," + val.TEST_SAMPLE_TYPE_NAME.Replace(",", ";");
                }
            }
            else
            {
                txt += ",";
            }
            if (hIS_SERVICE != null)
            {
                txt = txt + "," + hIS_SERVICE.SERVICE_NAME;
            }
            else
            {
                txt += ",";
            }
            txt = txt + "," + serviceReq.EXECUTE_ROOM_CODE;
            txt += "\n";
        }

        private void GenText(HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReq, HIS_SERE_SERV sereServ, ref string txt)
        {
            HIS_SERVICE service = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault((HIS_SERVICE o) => o.ID == sereServ.SERVICE_ID);
            HIS_SERVICE hIS_SERVICE = null;
            if (service != null && service.PARENT_ID.HasValue && service.PARENT_ID.Value > 0)
            {
                hIS_SERVICE = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault((HIS_SERVICE o) => o.ID == service.PARENT_ID);
            }
            txt += serviceReq.BARCODE;
            txt = txt + "," + serviceReq.TDL_PATIENT_NAME;
            txt = txt + "," + serviceReq.TDL_PATIENT_GENDER_NAME;
            txt = txt + "," + serviceReq.TDL_PATIENT_DOB;
            txt = txt + "," + ((serviceReq.TDL_PATIENT_DOB > 10000000000000L) ? serviceReq.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "");
            txt = txt + "," + serviceReq.REQUEST_DEPARTMENT_NAME;
            txt = txt + "," + serviceReq.REQUEST_ROOM_NAME;
            txt = txt + "," + Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Inventec.Common.DateTime.Get.Now() ?? 0);
            txt = txt + "," + serviceReq.TDL_TREATMENT_CODE;
            if (((HIS_SERVICE_REQ)serviceReq).TEST_SAMPLE_TYPE_ID.HasValue && ((HIS_SERVICE_REQ)serviceReq).TEST_SAMPLE_TYPE_ID.Value > 0)
            {
                HIS_TEST_SAMPLE_TYPE val = BackendDataWorker.Get<HIS_TEST_SAMPLE_TYPE>().FirstOrDefault((HIS_TEST_SAMPLE_TYPE o) => o.ID == ((HIS_SERVICE_REQ)serviceReq).TEST_SAMPLE_TYPE_ID);
                if (val != null)
                {
                    txt = txt + "," + val.TEST_SAMPLE_TYPE_NAME.Replace(",", ";");
                }
            }
            else
            {
                txt += ",";
            }
            if (hIS_SERVICE != null)
            {
                txt = txt + "," + hIS_SERVICE.SERVICE_NAME;
            }
            else
            {
                txt += ",";
            }
            txt = txt + "," + serviceReq.EXECUTE_ROOM_CODE;
            txt += "\n";
        }

        private void PrintBarcodeByBartender(HIS_SERVICE_REQ req)
        {
            try
            {
                if (StartAppPrintBartenderProcessor.OpenAppPrintBartender())
                {
                    ClientPrintADO clientPrintADO = new ClientPrintADO();
                    clientPrintADO.DobYear = req.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                    clientPrintADO.DobAge = MPS.AgeUtil.CalculateFullAge(req.TDL_PATIENT_DOB);
                    clientPrintADO.GenderName = req.TDL_PATIENT_GENDER_NAME ?? "";
                    clientPrintADO.GenderName_Unsign = Inventec.Common.String.Convert.UnSignVNese(clientPrintADO.GenderName);
                    clientPrintADO.GenderCode = ((!req.TDL_PATIENT_GENDER_ID.HasValue) ? "" : ((req.TDL_PATIENT_GENDER_ID == 1) ? "F" : "M"));
                    clientPrintADO.PatientCode = req.TDL_PATIENT_CODE ?? "";
                    clientPrintADO.PatientName = req.TDL_PATIENT_NAME ?? "";
                    clientPrintADO.PatientName_Unsign = Inventec.Common.String.Convert.UnSignVNese(clientPrintADO.PatientName);
                    clientPrintADO.ServiceReqCode = req.SERVICE_REQ_CODE ?? "";
                    clientPrintADO.TreatmentCode = req.TDL_TREATMENT_CODE ?? "";
                    V_HIS_ROOM v_HIS_ROOM = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == req.REQUEST_ROOM_ID);
                    if (v_HIS_ROOM != null)
                    {
                        clientPrintADO.RequestRoomCode = v_HIS_ROOM.ROOM_CODE;
                        clientPrintADO.RequestRoomName = v_HIS_ROOM.ROOM_NAME ?? "";
                        clientPrintADO.RequestRoomName_Unsign = Inventec.Common.String.Convert.UnSignVNese(v_HIS_ROOM.ROOM_NAME ?? "");
                        clientPrintADO.RequestDepartmentCode = v_HIS_ROOM.DEPARTMENT_CODE ?? "";
                        clientPrintADO.RequestDepartmentName = v_HIS_ROOM.DEPARTMENT_NAME ?? "";
                        clientPrintADO.RequestDepartmentName_Unsign = Inventec.Common.String.Convert.UnSignVNese(clientPrintADO.RequestDepartmentName);
                    }
                    V_HIS_ROOM v_HIS_ROOM2 = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == req.EXECUTE_ROOM_ID);
                    if (v_HIS_ROOM2 != null)
                    {
                        clientPrintADO.ExecuteRoomCode = v_HIS_ROOM2.ROOM_CODE;
                        clientPrintADO.ExecuteRoomName = v_HIS_ROOM2.ROOM_NAME ?? "";
                        clientPrintADO.ExecuteRoomName_Unsign = Inventec.Common.String.Convert.UnSignVNese(v_HIS_ROOM2.ROOM_NAME ?? "");
                        clientPrintADO.ExecuteDepartmentCode = v_HIS_ROOM2.DEPARTMENT_CODE ?? "";
                        clientPrintADO.ExecuteDepartmentName = v_HIS_ROOM2.DEPARTMENT_NAME ?? "";
                        clientPrintADO.ExecuteDepartmentName_Unsign = Inventec.Common.String.Convert.UnSignVNese(clientPrintADO.ExecuteDepartmentName);
                    }
                    BartenderPrintClientManager bartenderPrintClientManager = new BartenderPrintClientManager();
                    if (!bartenderPrintClientManager.BartenderPrint(clientPrintADO))
                    {
                        LogSystem.Error("In barcode Bartender that bai. Check log BartenderPrint");
                    }
                }
                else
                {
                    LogSystem.Warn("Khong mo duoc APP Print Bartender");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void barButtonPrintTemBarcode_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                btnPrintTemBarcode_Click(null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void txtPatientCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(txtPatientCode.Text))
                {
                    FillDataToGrid();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void chkPK_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (isNotLoadWhileChangeControlStateInFirst)
                {
                    return;
                }
                WaitingManager.Show();
                ControlStateRDO controlStateRDO = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkPK.Name && o.MODULE_LINK == "HIS.Desktop.Plugins.ServiceReqList").FirstOrDefault() : null);
                if (controlStateRDO != null)
                {
                    controlStateRDO.VALUE = (chkPK.Checked ? "1" : "");
                }
                else
                {
                    controlStateRDO = new ControlStateRDO();
                    controlStateRDO.KEY = chkPK.Name;
                    controlStateRDO.VALUE = (chkPK.Checked ? "1" : "");
                    controlStateRDO.MODULE_LINK = "HIS.Desktop.Plugins.ServiceReqList";
                    if (currentControlStateRDO == null)
                    {
                        currentControlStateRDO = new List<ControlStateRDO>();
                    }
                    currentControlStateRDO.Add(controlStateRDO);
                }
                controlStateWorker.SetData(currentControlStateRDO);
                if (chkPK.Checked)
                {
                    LoadComboExcuteRoom(true);
                }
                else
                {
                    LoadComboExcuteRoom();
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Warn(ex);
            }
        }

        private void cboExecuteRoom_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (e.Button.Kind == ButtonPredefines.Delete)
                {
                    cboExecuteRoom.EditValue = null;
                    cboExecuteRoom.Properties.Buttons[1].Visible = false;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void cboExecuteRoom_Closed(object sender, ClosedEventArgs e)
        {
            try
            {
                if (e.CloseMode == PopupCloseMode.Normal && cboExecuteRoom.EditValue != null)
                {
                    cboExecuteRoom.Properties.Buttons[1].Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void repositoryItemButtonEditAllowNotExecute_Enable_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void txtStoreCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Return && !string.IsNullOrEmpty(txtStoreCode.Text))
                {
                    FillDataToGrid();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void repositoryItemButtonEditDeleteEna_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (grdViewSereServServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                CommonParam commonParam = new CommonParam();
                bool flag = false;
                ListMedicineADO data = (ListMedicineADO)grdViewSereServServiceReq.GetFocusedRow();
                if (data == null)
                {
                    LogSystem.Info("Data thuc hien huy yeu cau dich vu null: " + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data));
                }
                else if (!CheckLoginAdmin.IsAdmin(loginName) && BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == data.PATIENT_TYPE_ID).IS_NOT_EDIT_ASSIGN_SERVICE == 1)
                {
                    XtraMessageBox.Show(string.Format("Dịch vụ {0} có đối tượng thanh toán được tích \"Không cho phép sửa xóa dịch vụ đã chỉ định\", vui lòng liên hệ với quản trị hệ thống", data.TDL_SERVICE_NAME), "Thông báo");
                }
                else if (MessageBox.Show(string.Format("Có chắc muốn xóa dịch vụ {0} không?", data.TDL_SERVICE_NAME), ResourceMessage.ThongBao, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    WaitingManager.Show();
                    flag = new BackendAdapter(commonParam).Post<bool>("api/HisSereServ/ExamDelete", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, data.ID, commonParam);
                    if (flag)
                    {
                        ControlServiceReqClick(currentServiceReq);
                    }
                    WaitingManager.Hide();
                    MessageManager.Show(this, commonParam, flag);
                    SessionManager.ProcessTokenLost(commonParam);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void grdSereServServiceReq_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                List<ListMedicineADO> list = ((grdSereServServiceReq.DataSource != null) ? (grdSereServServiceReq.DataSource as IEnumerable<ListMedicineADO>).ToList() : null);
                if (list != null)
                {
                    if (list.All((ListMedicineADO o) => o.IS_RATION == 1))
                    {
                        gridColSerSevPatientTypeName.Caption = "Mức ăn";
                        gridColSerSevPatientTypeName.ToolTip = "";
                    }
                    else if (list.All((ListMedicineADO o) => !((int?)o.IS_RATION).HasValue))
                    {
                        gridColSerSevPatientTypeName.Caption = "ĐTTT";
                        gridColSerSevPatientTypeName.ToolTip = "Đối tượng thanh toán";
                    }
                    else
                    {
                        gridColSerSevPatientTypeName.Caption = "Mức ăn/ĐTTT";
                        gridColSerSevPatientTypeName.ToolTip = "Mức ăn/Đối tượng thanh toán";
                    }
                }
                else
                {
                    gridColSerSevPatientTypeName.Caption = "Mức ăn/ĐTTT";
                    gridColSerSevPatientTypeName.ToolTip = "Mức ăn/Đối tượng thanh toán";
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void repositoryItemTextEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle >= 0)
                {
                    HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                    if (currentServiceReq == null || (serviceReqADO != null && serviceReqADO.ID != 0 && serviceReqADO.ID != currentServiceReq.ID))
                    {
                        currentServiceReq = serviceReqADO;
                        ControlServiceReqClick(currentServiceReq);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            try
            {
                ShowPopupContainerForConfig(btnConfig);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void ShowPopupContainerForConfig(SimpleButton editor)
        {
            try
            {
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(editor.Bounds.X, editor.Bounds.Y, editor.Bounds.Width, editor.Bounds.Height);
                popupControlContainer1.ShowPopup(new Point(rectangle.X, rectangle.Bottom - 100));
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void InitListConfig()
        {
            try
            {
                List<string> list = new List<string>();
                list.Add("Không hiển thị đơn không lấy ở đơn thuốc TH");
                List<string> list2 = list;
                lstConfig = new List<ConfigADO>();
                int i;
                for (i = 0; i < list2.Count; i++)
                {
                    lstConfig.Add(new ConfigADO
                    {
                        ID = i + 1,
                        NAME = list2[i],
                        IsChecked = ConfigIds.Exists((long o) => o == i + 1)
                    });
                }
                gridConfig.DataSource = null;
                gridConfig.DataSource = lstConfig;
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void repCheckConfig_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckEdit checkEdit = sender as CheckEdit;
                foreach (ConfigADO item in lstConfig)
                {
                    if (item.ID == ((ConfigADO)gvConfig.GetFocusedRow()).ID)
                    {
                        item.IsChecked = checkEdit.Checked;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void popupControlContainer1_CloseUp(object sender, EventArgs e)
        {
            try
            {
                ControlStateRDO controlStateRDO = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == gridConfig.Name && o.MODULE_LINK == "HIS.Desktop.Plugins.ServiceReqList").FirstOrDefault() : null);
                lstConfig = gridConfig.DataSource as List<ConfigADO>;
                LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => lstConfig), lstConfig));
                IEnumerable<long> values = from o in lstConfig
                                           where o.IsChecked
                                           select o.ID;
                if (controlStateRDO != null)
                {
                    controlStateRDO.VALUE = string.Join(";", values);
                }
                else
                {
                    controlStateRDO = new ControlStateRDO();
                    controlStateRDO.KEY = gridConfig.Name;
                    controlStateRDO.VALUE = string.Join(";", values);
                    controlStateRDO.MODULE_LINK = "HIS.Desktop.Plugins.ServiceReqList";
                    if (currentControlStateRDO == null)
                    {
                        currentControlStateRDO = new List<ControlStateRDO>();
                    }
                    currentControlStateRDO.Add(controlStateRDO);
                }
                controlStateWorker.SetData(currentControlStateRDO);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DevExpress.Utils.SerializableAppearanceObject appearance = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance12 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered12 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed12 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled12 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance13 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered13 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed13 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled13 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance14 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered14 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed14 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled14 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance15 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered15 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed15 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled15 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance16 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered16 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed16 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled16 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance17 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered17 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed17 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled17 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance18 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered18 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed18 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled18 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance19 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered19 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed19 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled19 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance20 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered20 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed20 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled20 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance21 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered21 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed21 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled21 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance22 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered22 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed22 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled22 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance23 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered23 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed23 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled23 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearance24 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceHovered24 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearancePressed24 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject appearanceDisabled24 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIS.Desktop.Plugins.ServiceReqList.frmServiceReqList));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.popupControlContainer1 = new DevExpress.XtraBars.PopupControlContainer();
            this.layoutControl3 = new DevExpress.XtraLayout.LayoutControl();
            this.gridConfig = new DevExpress.XtraGrid.GridControl();
            this.gvConfig = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repCheckConfig = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.bbtnRCFind = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonPrintTemBarcode = new DevExpress.XtraBars.BarButtonItem();
            this.imageCollection2 = new DevExpress.Utils.ImageCollection();
            this.bbtnRCRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem19 = new DevExpress.XtraLayout.LayoutControlItem();
            this.btnConfig = new DevExpress.XtraEditors.SimpleButton();
            this.txtStoreCode = new DevExpress.XtraEditors.TextEdit();
            this.chkPK = new DevExpress.XtraEditors.CheckEdit();
            this.cboExecuteRoom = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtPatientCode = new DevExpress.XtraEditors.TextEdit();
            this.btnPrintTemBarcode = new DevExpress.XtraEditors.SimpleButton();
            this.btnDropDownPrint = new DevExpress.XtraEditors.DropDownButton();
            this.btnPrintMedicine = new DevExpress.XtraEditors.SimpleButton();
            this.btnPrintTotal = new DevExpress.XtraEditors.SimpleButton();
            this.txtTreatmentCode = new DevExpress.XtraEditors.TextEdit();
            this.groupControlInfo = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.lblTestSampleTypeName = new DevExpress.XtraEditors.LabelControl();
            this.lblReceiveSampleName = new DevExpress.XtraEditors.LabelControl();
            this.lblSamplerName = new DevExpress.XtraEditors.LabelControl();
            this.lblRationSumCode = new DevExpress.XtraEditors.LabelControl();
            this.lblAssignTurnCode = new DevExpress.XtraEditors.LabelControl();
            this.lblBarcode = new DevExpress.XtraEditors.LabelControl();
            this.chkReqSended = new DevExpress.XtraEditors.CheckEdit();
            this.chkIsHomePres = new DevExpress.XtraEditors.CheckEdit();
            this.chkIsKidney = new DevExpress.XtraEditors.CheckEdit();
            this.lbDOB = new DevExpress.XtraEditors.LabelControl();
            this.lbExcuteDepartment = new DevExpress.XtraEditors.LabelControl();
            this.lblSoTheTM = new DevExpress.XtraEditors.LabelControl();
            this.lblSoTT = new DevExpress.XtraEditors.LabelControl();
            this.lblSoThang = new DevExpress.XtraEditors.LabelControl();
            this.lblReqDepartment = new DevExpress.XtraEditors.LabelControl();
            this.lblExpMestStt = new DevExpress.XtraEditors.LabelControl();
            this.btnMobaCreate = new DevExpress.XtraEditors.SimpleButton();
            this.btnAggrExpMest = new DevExpress.XtraEditors.SimpleButton();
            this.lblExpMestRoom = new DevExpress.XtraEditors.LabelControl();
            this.lblAggrExpMestCode = new DevExpress.XtraEditors.LabelControl();
            this.lblExpMestCode = new DevExpress.XtraEditors.LabelControl();
            this.lblGender = new DevExpress.XtraEditors.LabelControl();
            this.lblTreatmentCode = new DevExpress.XtraEditors.LabelControl();
            this.lblPatientName = new DevExpress.XtraEditors.LabelControl();
            this.lblRationTime = new DevExpress.XtraEditors.LabelControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciPatientName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTreatmentCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciBtnAggrExpMest = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciBtnMobaCreate = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciReqDepartment = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciRationTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciGender = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciExcuteDepartment = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciExpMestStt = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem18 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciNumOrder = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciSoTheTM = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciExpMestRoom = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciExpMestCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciAggrExpMestCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciIsKidney = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciIsHomePres = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciReqSended = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciBarcode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciAssignTurnCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lciRationSumCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciReceiveSampleName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciSamplerName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTestSampleTypeName = new DevExpress.XtraLayout.LayoutControlItem();
            this.cboFilter = new DevExpress.XtraEditors.LookUpEdit();
            this.dtIntructionTimeTo = new DevExpress.XtraEditors.DateEdit();
            this.dtIntructionTimeFrom = new DevExpress.XtraEditors.DateEdit();
            this.cboServiceReqType = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cboServiceReqStt = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grdSereServServiceReq = new DevExpress.XtraGrid.GridControl();
            this.grdViewSereServServiceReq = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColSerSevSTT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevView = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevPrint = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEditServiceConfirmEna = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcolServiceTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevPatientTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevPresAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevUnitName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Pttt_Group_Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevConvertRatio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevConvertAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevConvertName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSerSevTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColOtherPrintForm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridColumnInReqExeute = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonView = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonPrint = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemTextEditDisable = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemButtonIsAcceptNoExecute = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEditDeleteEna = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEditDeleteDis = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEditServiceConfirmDis = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.txtServiceReqCode = new DevExpress.XtraEditors.TextEdit();
            this.txtKeyword = new DevExpress.XtraEditors.TextEdit();
            this.btnFind = new DevExpress.XtraEditors.SimpleButton();
            this.ucPaging1 = new Inventec.UC.Paging.UcPaging();
            this.gridControlServiceReq = new DevExpress.XtraGrid.GridControl();
            this.gridViewServiceReq = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_ServiceReq_Choose = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEditChoose = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn_Transaction_Stt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_ServiceReq_Edit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_AllowNotExecute = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEditAllowNotExecute_Enable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn_IsConfirmNoExcute = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEditIsConfirm_Ena = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn_ServiceReq_Delete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_ServiceReq_Print = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_ServiceReq_EditIntructionTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEditIntructionTime = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn_BieuMauKhac = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButton__BieuMauKhac = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn_ServiceReq_Stt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItempicServiceReqStatus = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Btn_EvenLog = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn_Transaction_TransactionCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn_Transaction_TreatmentCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_PatientCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_VirPatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_Amount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_CashierRoomName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_PayFormName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_Dob = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Request_Username = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Execute_Username = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdColRationTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_GenderName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Gc_HisSendOldSystem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_CreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_Creator = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_ModifyTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Transaction_Modifier = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemBtnDeleteServiceReq = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemBtnDeleteServiceReqDisable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemBtnEditServiceReq = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemBtnPrintServiceReq = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemBtnEditServiceReqDisable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemBtnPrintServiceReqDisable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemReadOnly = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemBtnBieuMauKhac = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemCheckEditMainExam = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemButtonEditAllowNotExecute_Disable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEditIsConfirm_Dis = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.tooltipServiceRequest = new DevExpress.Utils.ToolTipController();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciServiceReqPaging = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciIntructionTimeFrom = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciServiceReqCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciIntructionTimeTo = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciKeyword = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem17 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem16 = new DevExpress.XtraLayout.LayoutControlItem();
            this.s = new DevExpress.XtraLayout.LayoutControlItem();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController();
            this.imageListPriority = new System.Windows.Forms.ImageList();
            this.imageListIcon = new System.Windows.Forms.ImageList();
            this.imageListCheck = new System.Windows.Forms.ImageList();
            ((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.popupControlContainer1).BeginInit();
            this.popupControlContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.layoutControl3).BeginInit();
            this.layoutControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.gridConfig).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.gvConfig).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repCheckConfig).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.imageCollection2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlGroup2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem19).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.txtStoreCode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.chkPK.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.cboExecuteRoom.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.gridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.txtPatientCode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.txtTreatmentCode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.groupControlInfo).BeginInit();
            this.groupControlInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.layoutControl2).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.chkReqSended.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.chkIsHomePres.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.chkIsKidney.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciPatientName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciTreatmentCode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciBtnAggrExpMest).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciBtnMobaCreate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciReqDepartment).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciRationTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciGender).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciExcuteDepartment).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciExpMestStt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem18).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciNumOrder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciSoTheTM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciExpMestRoom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciExpMestCode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciAggrExpMestCode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciIsKidney).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciIsHomePres).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciReqSended).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciBarcode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciAssignTurnCode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.emptySpaceItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciRationSumCode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciReceiveSampleName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciSamplerName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciTestSampleTypeName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.cboFilter.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dtIntructionTimeTo.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dtIntructionTimeTo.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dtIntructionTimeFrom.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dtIntructionTimeFrom.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.cboServiceReqType.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.cboServiceReqStt.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.grdSereServServiceReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.grdViewSereServServiceReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditServiceConfirmEna).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonPrint).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEdit3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemTextEditDisable).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonIsAcceptNoExecute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditDeleteEna).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditDeleteDis).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditServiceConfirmDis).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.txtServiceReqCode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.txtKeyword.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.gridControlServiceReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.gridViewServiceReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemCheckEditChoose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditAllowNotExecute_Enable).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditIsConfirm_Ena).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditIntructionTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButton__BieuMauKhac).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItempicServiceReqStatus).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.Btn_EvenLog).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemTextEdit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnDeleteServiceReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnDeleteServiceReqDisable).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnEditServiceReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnPrintServiceReq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnEditServiceReqDisable).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnPrintServiceReqDisable).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemReadOnly).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnBieuMauKhac).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemCheckEditMainExam).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditAllowNotExecute_Disable).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditIsConfirm_Dis).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciServiceReqPaging).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem13).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciIntructionTimeFrom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciServiceReqCode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciIntructionTimeTo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem14).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lciKeyword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem15).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem17).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem16).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.s).BeginInit();
            base.SuspendLayout();
            this.layoutControl1.Controls.Add(this.popupControlContainer1);
            this.layoutControl1.Controls.Add(this.btnConfig);
            this.layoutControl1.Controls.Add(this.txtStoreCode);
            this.layoutControl1.Controls.Add(this.chkPK);
            this.layoutControl1.Controls.Add(this.cboExecuteRoom);
            this.layoutControl1.Controls.Add(this.txtPatientCode);
            this.layoutControl1.Controls.Add(this.btnPrintTemBarcode);
            this.layoutControl1.Controls.Add(this.btnDropDownPrint);
            this.layoutControl1.Controls.Add(this.btnPrintMedicine);
            this.layoutControl1.Controls.Add(this.btnPrintTotal);
            this.layoutControl1.Controls.Add(this.txtTreatmentCode);
            this.layoutControl1.Controls.Add(this.groupControlInfo);
            this.layoutControl1.Controls.Add(this.cboFilter);
            this.layoutControl1.Controls.Add(this.dtIntructionTimeTo);
            this.layoutControl1.Controls.Add(this.dtIntructionTimeFrom);
            this.layoutControl1.Controls.Add(this.cboServiceReqType);
            this.layoutControl1.Controls.Add(this.cboServiceReqStt);
            this.layoutControl1.Controls.Add(this.grdSereServServiceReq);
            this.layoutControl1.Controls.Add(this.txtServiceReqCode);
            this.layoutControl1.Controls.Add(this.txtKeyword);
            this.layoutControl1.Controls.Add(this.btnFind);
            this.layoutControl1.Controls.Add(this.ucPaging1);
            this.layoutControl1.Controls.Add(this.gridControlServiceReq);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 38);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1924, 714);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            this.popupControlContainer1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.popupControlContainer1.Controls.Add(this.layoutControl3);
            this.popupControlContainer1.Location = new System.Drawing.Point(636, 543);
            this.popupControlContainer1.Manager = this.barManager1;
            this.popupControlContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.popupControlContainer1.Name = "popupControlContainer1";
            this.popupControlContainer1.Size = new System.Drawing.Size(475, 150);
            this.popupControlContainer1.TabIndex = 33;
            this.popupControlContainer1.Visible = false;
            this.popupControlContainer1.CloseUp += new System.EventHandler(popupControlContainer1_CloseUp);
            this.layoutControl3.Controls.Add(this.gridConfig);
            this.layoutControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl3.Location = new System.Drawing.Point(0, 0);
            this.layoutControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.layoutControl3.Name = "layoutControl3";
            this.layoutControl3.Root = this.layoutControlGroup2;
            this.layoutControl3.Size = new System.Drawing.Size(475, 150);
            this.layoutControl3.TabIndex = 0;
            this.layoutControl3.Text = "layoutControl3";
            this.gridConfig.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridConfig.Location = new System.Drawing.Point(6, 6);
            this.gridConfig.MainView = this.gvConfig;
            this.gridConfig.Margin = new System.Windows.Forms.Padding(4);
            this.gridConfig.MenuManager = this.barManager1;
            this.gridConfig.Name = "gridConfig";
            this.gridConfig.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[1] { this.repCheckConfig });
            this.gridConfig.Size = new System.Drawing.Size(463, 138);
            this.gridConfig.TabIndex = 4;
            this.gridConfig.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gvConfig });
            this.gvConfig.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[2] { this.gridColumn9, this.gridColumn10 });
            this.gvConfig.GridControl = this.gridConfig;
            this.gvConfig.Name = "gvConfig";
            this.gvConfig.OptionsView.ShowColumnHeaders = false;
            this.gvConfig.OptionsView.ShowGroupPanel = false;
            this.gvConfig.OptionsView.ShowIndicator = false;
            this.gridColumn9.Caption = "gridColumn9";
            this.gridColumn9.FieldName = "NAME";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsColumn.ReadOnly = true;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 0;
            this.gridColumn9.Width = 251;
            this.gridColumn10.Caption = "gridColumn10";
            this.gridColumn10.ColumnEdit = this.repCheckConfig;
            this.gridColumn10.FieldName = "IsChecked";
            this.gridColumn10.MaxWidth = 25;
            this.gridColumn10.MinWidth = 25;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 1;
            this.gridColumn10.Width = 25;
            this.repCheckConfig.AutoHeight = false;
            this.repCheckConfig.Name = "repCheckConfig";
            this.repCheckConfig.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repCheckConfig.CheckedChanged += new System.EventHandler(repCheckConfig_CheckedChanged);
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[1] { this.bar1 });
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageCollection2;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[3] { this.bbtnRCFind, this.bbtnRCRefresh, this.barButtonPrintTemBarcode });
            this.barManager1.MaxItemId = 3;
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
			{
				new DevExpress.XtraBars.LinkPersistInfo(this.bbtnRCFind),
				new DevExpress.XtraBars.LinkPersistInfo(this.barButtonPrintTemBarcode)
			});
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
            this.bbtnRCFind.Caption = "Tìm (Ctrl F)";
            this.bbtnRCFind.Id = 0;
            this.bbtnRCFind.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F | System.Windows.Forms.Keys.Control);
            this.bbtnRCFind.Name = "bbtnRCFind";
            this.bbtnRCFind.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(bbtnRCFind_ItemClick);
            this.barButtonPrintTemBarcode.Caption = "In Tem (F7)";
            this.barButtonPrintTemBarcode.Id = 2;
            this.barButtonPrintTemBarcode.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F7);
            this.barButtonPrintTemBarcode.Name = "barButtonPrintTemBarcode";
            this.barButtonPrintTemBarcode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonPrintTemBarcode_ItemClick);
            this.imageCollection2.ImageStream = (DevExpress.Utils.ImageCollectionStreamer)resources.GetObject("imageCollection2.ImageStream");
            this.imageCollection2.Images.SetKeyName(0, "GradientBlueDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(1, "GradientGreenDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(2, "GradientLightBlueDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(3, "GradientOrangeDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(4, "GradientPurpleDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(5, "GradientRedDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(6, "SolidBlueDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(7, "SolidGreenDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(8, "SolidLightBlueDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(9, "SolidOrangeDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(10, "SolidPurpleDataBar_16x16.png");
            this.imageCollection2.Images.SetKeyName(11, "SolidRedDataBar_16x16.png");
            this.bbtnRCRefresh.Caption = "Làm lại (Ctrl R)";
            this.bbtnRCRefresh.Id = 1;
            this.bbtnRCRefresh.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.R | System.Windows.Forms.Keys.Control);
            this.bbtnRCRefresh.Name = "bbtnRCRefresh";
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1924, 38);
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 752);
            this.barDockControlBottom.Size = new System.Drawing.Size(1924, 0);
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 38);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 714);
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1924, 38);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 714);
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[1] { this.layoutControlItem19 });
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.OptionsItemText.TextToControlDistance = 4;
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup2.Size = new System.Drawing.Size(475, 150);
            this.layoutControlGroup2.TextVisible = false;
            this.layoutControlItem19.Control = this.gridConfig;
            this.layoutControlItem19.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem19.Name = "layoutControlItem19";
            this.layoutControlItem19.Size = new System.Drawing.Size(469, 144);
            this.layoutControlItem19.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem19.TextVisible = false;
            this.btnConfig.Image = (System.Drawing.Image)resources.GetObject("btnConfig.Image");
            this.btnConfig.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnConfig.Location = new System.Drawing.Point(566, 684);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(44, 27);
            this.btnConfig.StyleController = this.layoutControl1;
            this.btnConfig.TabIndex = 32;
            this.btnConfig.Click += new System.EventHandler(btnConfig_Click);
            this.txtStoreCode.Location = new System.Drawing.Point(483, 31);
            this.txtStoreCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtStoreCode.MenuManager = this.barManager1;
            this.txtStoreCode.Name = "txtStoreCode";
            this.txtStoreCode.Properties.NullValuePrompt = "Mã lưu trữ";
            this.txtStoreCode.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtStoreCode.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtStoreCode.Size = new System.Drawing.Size(174, 22);
            this.txtStoreCode.StyleController = this.layoutControl1;
            this.txtStoreCode.TabIndex = 31;
            this.txtStoreCode.KeyDown += new System.Windows.Forms.KeyEventHandler(txtStoreCode_KeyDown);
            this.chkPK.Location = new System.Drawing.Point(678, 3);
            this.chkPK.Margin = new System.Windows.Forms.Padding(4);
            this.chkPK.MenuManager = this.barManager1;
            this.chkPK.Name = "chkPK";
            this.chkPK.Properties.Caption = "PK";
            this.chkPK.Size = new System.Drawing.Size(73, 20);
            this.chkPK.StyleController = this.layoutControl1;
            this.chkPK.TabIndex = 30;
            this.chkPK.ToolTip = "Chỉ hiển thị các phòng xử lý là \"Phòng khám\"";
            this.chkPK.CheckedChanged += new System.EventHandler(chkPK_CheckedChanged);
            this.cboExecuteRoom.Location = new System.Drawing.Point(478, 3);
            this.cboExecuteRoom.Margin = new System.Windows.Forms.Padding(4);
            this.cboExecuteRoom.MenuManager = this.barManager1;
            this.cboExecuteRoom.Name = "cboExecuteRoom";
            this.cboExecuteRoom.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboExecuteRoom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
			});
            this.cboExecuteRoom.Properties.NullText = "";
            this.cboExecuteRoom.Properties.View = this.gridView2;
            this.cboExecuteRoom.Size = new System.Drawing.Size(194, 22);
            this.cboExecuteRoom.StyleController = this.layoutControl1;
            this.cboExecuteRoom.TabIndex = 28;
            this.cboExecuteRoom.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboExecuteRoom_Closed);
            this.cboExecuteRoom.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboExecuteRoom_ButtonClick);
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.txtPatientCode.EnterMoveNextControl = true;
            this.txtPatientCode.Location = new System.Drawing.Point(331, 31);
            this.txtPatientCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtPatientCode.MenuManager = this.barManager1;
            this.txtPatientCode.Name = "txtPatientCode";
            this.txtPatientCode.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtPatientCode.Properties.NullValuePrompt = "Mã bệnh nhân";
            this.txtPatientCode.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtPatientCode.Size = new System.Drawing.Size(146, 22);
            this.txtPatientCode.StyleController = this.layoutControl1;
            this.txtPatientCode.TabIndex = 27;
            this.txtPatientCode.KeyDown += new System.Windows.Forms.KeyEventHandler(txtPatientCode_KeyDown);
            this.btnPrintTemBarcode.Location = new System.Drawing.Point(1028, 684);
            this.btnPrintTemBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintTemBarcode.Name = "btnPrintTemBarcode";
            this.btnPrintTemBarcode.Size = new System.Drawing.Size(120, 27);
            this.btnPrintTemBarcode.StyleController = this.layoutControl1;
            this.btnPrintTemBarcode.TabIndex = 26;
            this.btnPrintTemBarcode.Text = "In tem (F7)";
            this.btnPrintTemBarcode.Click += new System.EventHandler(btnPrintTemBarcode_Click);
            this.btnDropDownPrint.Location = new System.Drawing.Point(616, 684);
            this.btnDropDownPrint.Margin = new System.Windows.Forms.Padding(4);
            this.btnDropDownPrint.MenuManager = this.barManager1;
            this.btnDropDownPrint.Name = "btnDropDownPrint";
            this.btnDropDownPrint.Size = new System.Drawing.Size(112, 27);
            this.btnDropDownPrint.StyleController = this.layoutControl1;
            this.btnDropDownPrint.TabIndex = 25;
            this.btnDropDownPrint.Text = "In phiếu";
            this.btnDropDownPrint.Click += new System.EventHandler(btnDropDownPrint_Click);
            this.btnPrintMedicine.Location = new System.Drawing.Point(734, 684);
            this.btnPrintMedicine.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintMedicine.Name = "btnPrintMedicine";
            this.btnPrintMedicine.Size = new System.Drawing.Size(150, 27);
            this.btnPrintMedicine.StyleController = this.layoutControl1;
            this.btnPrintMedicine.TabIndex = 23;
            this.btnPrintMedicine.Text = "In đơn thuốc TH";
            this.btnPrintMedicine.Click += new System.EventHandler(btnPrintMedicine_Click);
            this.btnPrintTotal.Location = new System.Drawing.Point(890, 684);
            this.btnPrintTotal.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintTotal.Name = "btnPrintTotal";
            this.btnPrintTotal.Size = new System.Drawing.Size(132, 27);
            this.btnPrintTotal.StyleController = this.layoutControl1;
            this.btnPrintTotal.TabIndex = 22;
            this.btnPrintTotal.Text = "In phiếu chỉ định TH";
            this.btnPrintTotal.Click += new System.EventHandler(btnPrintTotal_Click);
            this.txtTreatmentCode.EnterMoveNextControl = true;
            this.txtTreatmentCode.Location = new System.Drawing.Point(159, 31);
            this.txtTreatmentCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtTreatmentCode.MenuManager = this.barManager1;
            this.txtTreatmentCode.Name = "txtTreatmentCode";
            this.txtTreatmentCode.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTreatmentCode.Properties.NullValuePrompt = "Mã điều trị";
            this.txtTreatmentCode.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtTreatmentCode.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtTreatmentCode.Size = new System.Drawing.Size(166, 22);
            this.txtTreatmentCode.StyleController = this.layoutControl1;
            this.txtTreatmentCode.TabIndex = 21;
            this.txtTreatmentCode.KeyDown += new System.Windows.Forms.KeyEventHandler(txtTreatmentCode_KeyDown);
            this.groupControlInfo.Controls.Add(this.layoutControl2);
            this.groupControlInfo.Location = new System.Drawing.Point(1154, 3);
            this.groupControlInfo.Margin = new System.Windows.Forms.Padding(4);
            this.groupControlInfo.Name = "groupControlInfo";
            this.groupControlInfo.Size = new System.Drawing.Size(767, 319);
            this.groupControlInfo.TabIndex = 20;
            this.groupControlInfo.Text = "Thông tin chung";
            this.layoutControl2.Controls.Add(this.lblTestSampleTypeName);
            this.layoutControl2.Controls.Add(this.lblReceiveSampleName);
            this.layoutControl2.Controls.Add(this.lblSamplerName);
            this.layoutControl2.Controls.Add(this.lblRationSumCode);
            this.layoutControl2.Controls.Add(this.lblAssignTurnCode);
            this.layoutControl2.Controls.Add(this.lblBarcode);
            this.layoutControl2.Controls.Add(this.chkReqSended);
            this.layoutControl2.Controls.Add(this.chkIsHomePres);
            this.layoutControl2.Controls.Add(this.chkIsKidney);
            this.layoutControl2.Controls.Add(this.lbDOB);
            this.layoutControl2.Controls.Add(this.lbExcuteDepartment);
            this.layoutControl2.Controls.Add(this.lblSoTheTM);
            this.layoutControl2.Controls.Add(this.lblSoTT);
            this.layoutControl2.Controls.Add(this.lblSoThang);
            this.layoutControl2.Controls.Add(this.lblReqDepartment);
            this.layoutControl2.Controls.Add(this.lblExpMestStt);
            this.layoutControl2.Controls.Add(this.btnMobaCreate);
            this.layoutControl2.Controls.Add(this.btnAggrExpMest);
            this.layoutControl2.Controls.Add(this.lblExpMestRoom);
            this.layoutControl2.Controls.Add(this.lblAggrExpMestCode);
            this.layoutControl2.Controls.Add(this.lblExpMestCode);
            this.layoutControl2.Controls.Add(this.lblGender);
            this.layoutControl2.Controls.Add(this.lblTreatmentCode);
            this.layoutControl2.Controls.Add(this.lblPatientName);
            this.layoutControl2.Controls.Add(this.lblRationTime);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(2, 25);
            this.layoutControl2.Margin = new System.Windows.Forms.Padding(4);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(87, 215, 250, 350);
            this.layoutControl2.Root = this.Root;
            this.layoutControl2.Size = new System.Drawing.Size(763, 292);
            this.layoutControl2.TabIndex = 19;
            this.layoutControl2.Text = "layoutControl2";
            this.lblTestSampleTypeName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTestSampleTypeName.Location = new System.Drawing.Point(367, 107);
            this.lblTestSampleTypeName.Margin = new System.Windows.Forms.Padding(4);
            this.lblTestSampleTypeName.Name = "lblTestSampleTypeName";
            this.lblTestSampleTypeName.Size = new System.Drawing.Size(393, 20);
            this.lblTestSampleTypeName.StyleController = this.layoutControl2;
            this.lblTestSampleTypeName.TabIndex = 27;
            this.lblReceiveSampleName.Location = new System.Drawing.Point(103, 133);
            this.lblReceiveSampleName.Margin = new System.Windows.Forms.Padding(4);
            this.lblReceiveSampleName.Name = "lblReceiveSampleName";
            this.lblReceiveSampleName.Size = new System.Drawing.Size(657, 20);
            this.lblReceiveSampleName.StyleController = this.layoutControl2;
            this.lblReceiveSampleName.TabIndex = 26;
            this.lblSamplerName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblSamplerName.Location = new System.Drawing.Point(103, 107);
            this.lblSamplerName.Margin = new System.Windows.Forms.Padding(4);
            this.lblSamplerName.Name = "lblSamplerName";
            this.lblSamplerName.Size = new System.Drawing.Size(173, 20);
            this.lblSamplerName.StyleController = this.layoutControl2;
            this.lblSamplerName.TabIndex = 25;
            this.lblRationSumCode.Location = new System.Drawing.Point(367, 211);
            this.lblRationSumCode.Margin = new System.Windows.Forms.Padding(4);
            this.lblRationSumCode.Name = "lblRationSumCode";
            this.lblRationSumCode.Size = new System.Drawing.Size(170, 20);
            this.lblRationSumCode.StyleController = this.layoutControl2;
            this.lblRationSumCode.TabIndex = 24;
            this.lblAssignTurnCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblAssignTurnCode.Location = new System.Drawing.Point(618, 81);
            this.lblAssignTurnCode.Margin = new System.Windows.Forms.Padding(4);
            this.lblAssignTurnCode.Name = "lblAssignTurnCode";
            this.lblAssignTurnCode.Size = new System.Drawing.Size(142, 20);
            this.lblAssignTurnCode.StyleController = this.layoutControl2;
            this.lblAssignTurnCode.TabIndex = 23;
            this.lblBarcode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblBarcode.Location = new System.Drawing.Point(367, 81);
            this.lblBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(170, 20);
            this.lblBarcode.StyleController = this.layoutControl2;
            this.lblBarcode.TabIndex = 22;
            this.chkReqSended.Location = new System.Drawing.Point(103, 81);
            this.chkReqSended.Margin = new System.Windows.Forms.Padding(4);
            this.chkReqSended.MenuManager = this.barManager1;
            this.chkReqSended.Name = "chkReqSended";
            this.chkReqSended.Properties.Caption = "";
            this.chkReqSended.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.chkReqSended.Size = new System.Drawing.Size(173, 19);
            this.chkReqSended.StyleController = this.layoutControl2;
            this.chkReqSended.TabIndex = 21;
            this.chkIsHomePres.Location = new System.Drawing.Point(367, 185);
            this.chkIsHomePres.Margin = new System.Windows.Forms.Padding(4);
            this.chkIsHomePres.MenuManager = this.barManager1;
            this.chkIsHomePres.Name = "chkIsHomePres";
            this.chkIsHomePres.Properties.Caption = "";
            this.chkIsHomePres.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.chkIsHomePres.Size = new System.Drawing.Size(170, 19);
            this.chkIsHomePres.StyleController = this.layoutControl2;
            this.chkIsHomePres.TabIndex = 20;
            this.chkIsKidney.Location = new System.Drawing.Point(103, 185);
            this.chkIsKidney.Margin = new System.Windows.Forms.Padding(4);
            this.chkIsKidney.MenuManager = this.barManager1;
            this.chkIsKidney.Name = "chkIsKidney";
            this.chkIsKidney.Properties.Caption = "";
            this.chkIsKidney.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.chkIsKidney.Size = new System.Drawing.Size(173, 19);
            this.chkIsKidney.StyleController = this.layoutControl2;
            this.chkIsKidney.TabIndex = 19;
            this.lbDOB.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbDOB.Location = new System.Drawing.Point(103, 29);
            this.lbDOB.Margin = new System.Windows.Forms.Padding(4);
            this.lbDOB.Name = "lbDOB";
            this.lbDOB.Size = new System.Drawing.Size(173, 20);
            this.lbDOB.StyleController = this.layoutControl2;
            this.lbDOB.TabIndex = 18;
            this.lbExcuteDepartment.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbExcuteDepartment.Location = new System.Drawing.Point(367, 55);
            this.lbExcuteDepartment.Margin = new System.Windows.Forms.Padding(4);
            this.lbExcuteDepartment.Name = "lbExcuteDepartment";
            this.lbExcuteDepartment.Size = new System.Drawing.Size(170, 20);
            this.lbExcuteDepartment.StyleController = this.layoutControl2;
            this.lbExcuteDepartment.TabIndex = 17;
            this.lblSoTheTM.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblSoTheTM.Location = new System.Drawing.Point(367, 29);
            this.lblSoTheTM.Margin = new System.Windows.Forms.Padding(4);
            this.lblSoTheTM.Name = "lblSoTheTM";
            this.lblSoTheTM.Size = new System.Drawing.Size(170, 18);
            this.lblSoTheTM.StyleController = this.layoutControl2;
            this.lblSoTheTM.TabIndex = 16;
            this.lblSoTT.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblSoTT.Location = new System.Drawing.Point(618, 29);
            this.lblSoTT.Margin = new System.Windows.Forms.Padding(4);
            this.lblSoTT.Name = "lblSoTT";
            this.lblSoTT.Size = new System.Drawing.Size(142, 18);
            this.lblSoTT.StyleController = this.layoutControl2;
            this.lblSoTT.TabIndex = 15;
            this.lblSoThang.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblSoThang.Location = new System.Drawing.Point(618, 185);
            this.lblSoThang.Margin = new System.Windows.Forms.Padding(4);
            this.lblSoThang.Name = "lblSoThang";
            this.lblSoThang.Size = new System.Drawing.Size(142, 20);
            this.lblSoThang.StyleController = this.layoutControl2;
            this.lblSoThang.TabIndex = 14;
            this.lblReqDepartment.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblReqDepartment.Location = new System.Drawing.Point(103, 55);
            this.lblReqDepartment.Margin = new System.Windows.Forms.Padding(4);
            this.lblReqDepartment.Name = "lblReqDepartment";
            this.lblReqDepartment.Size = new System.Drawing.Size(173, 20);
            this.lblReqDepartment.StyleController = this.layoutControl2;
            this.lblReqDepartment.TabIndex = 13;
            this.lblExpMestStt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblExpMestStt.Location = new System.Drawing.Point(618, 55);
            this.lblExpMestStt.Margin = new System.Windows.Forms.Padding(4);
            this.lblExpMestStt.Name = "lblExpMestStt";
            this.lblExpMestStt.Size = new System.Drawing.Size(142, 20);
            this.lblExpMestStt.StyleController = this.layoutControl2;
            this.lblExpMestStt.TabIndex = 12;
            this.btnMobaCreate.Location = new System.Drawing.Point(263, 262);
            this.btnMobaCreate.Margin = new System.Windows.Forms.Padding(4);
            this.btnMobaCreate.Name = "btnMobaCreate";
            this.btnMobaCreate.Size = new System.Drawing.Size(241, 27);
            this.btnMobaCreate.StyleController = this.layoutControl2;
            this.btnMobaCreate.TabIndex = 11;
            this.btnMobaCreate.Text = "Tạo thu hồi";
            this.btnMobaCreate.Click += new System.EventHandler(btnMobaCreate_Click);
            this.btnAggrExpMest.Location = new System.Drawing.Point(510, 262);
            this.btnAggrExpMest.Margin = new System.Windows.Forms.Padding(4);
            this.btnAggrExpMest.Name = "btnAggrExpMest";
            this.btnAggrExpMest.Size = new System.Drawing.Size(250, 27);
            this.btnAggrExpMest.StyleController = this.layoutControl2;
            this.btnAggrExpMest.TabIndex = 10;
            this.btnAggrExpMest.Text = "Ds thu hồi";
            this.btnAggrExpMest.Click += new System.EventHandler(btnAggrExpMest_Click);
            this.lblExpMestRoom.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblExpMestRoom.Location = new System.Drawing.Point(103, 159);
            this.lblExpMestRoom.Margin = new System.Windows.Forms.Padding(4);
            this.lblExpMestRoom.Name = "lblExpMestRoom";
            this.lblExpMestRoom.Size = new System.Drawing.Size(173, 20);
            this.lblExpMestRoom.StyleController = this.layoutControl2;
            this.lblExpMestRoom.TabIndex = 9;
            this.lblAggrExpMestCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblAggrExpMestCode.Location = new System.Drawing.Point(624, 159);
            this.lblAggrExpMestCode.Margin = new System.Windows.Forms.Padding(4);
            this.lblAggrExpMestCode.Name = "lblAggrExpMestCode";
            this.lblAggrExpMestCode.Size = new System.Drawing.Size(136, 20);
            this.lblAggrExpMestCode.StyleController = this.layoutControl2;
            this.lblAggrExpMestCode.TabIndex = 8;
            this.lblExpMestCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblExpMestCode.Location = new System.Drawing.Point(367, 159);
            this.lblExpMestCode.Margin = new System.Windows.Forms.Padding(4);
            this.lblExpMestCode.Name = "lblExpMestCode";
            this.lblExpMestCode.Size = new System.Drawing.Size(170, 20);
            this.lblExpMestCode.StyleController = this.layoutControl2;
            this.lblExpMestCode.TabIndex = 7;
            this.lblGender.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblGender.Location = new System.Drawing.Point(618, 3);
            this.lblGender.Margin = new System.Windows.Forms.Padding(4);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(142, 20);
            this.lblGender.StyleController = this.layoutControl2;
            this.lblGender.TabIndex = 6;
            this.lblTreatmentCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTreatmentCode.Location = new System.Drawing.Point(367, 3);
            this.lblTreatmentCode.Margin = new System.Windows.Forms.Padding(4);
            this.lblTreatmentCode.Name = "lblTreatmentCode";
            this.lblTreatmentCode.Size = new System.Drawing.Size(170, 20);
            this.lblTreatmentCode.StyleController = this.layoutControl2;
            this.lblTreatmentCode.TabIndex = 5;
            this.lblPatientName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblPatientName.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblPatientName.Location = new System.Drawing.Point(103, 3);
            this.lblPatientName.Margin = new System.Windows.Forms.Padding(4);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(173, 20);
            this.lblPatientName.StyleController = this.layoutControl2;
            this.lblPatientName.TabIndex = 4;
            this.lblRationTime.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblRationTime.Location = new System.Drawing.Point(103, 211);
            this.lblRationTime.Margin = new System.Windows.Forms.Padding(4);
            this.lblRationTime.Name = "lblRationTime";
            this.lblRationTime.Size = new System.Drawing.Size(173, 20);
            this.lblRationTime.StyleController = this.layoutControl2;
            this.lblRationTime.TabIndex = 12;
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[28]
			{
				this.lciPatientName, this.lciTreatmentCode, this.lciBtnAggrExpMest, this.lciBtnMobaCreate, this.lciReqDepartment, this.emptySpaceItem2, this.emptySpaceItem1, this.lciRationTime, this.lciGender, this.lciExcuteDepartment,
				this.lciExpMestStt, this.layoutControlItem18, this.lciNumOrder, this.lciSoTheTM, this.lciExpMestRoom, this.lciExpMestCode, this.lciAggrExpMestCode, this.layoutControlItem11, this.lciIsKidney, this.lciIsHomePres,
				this.lciReqSended, this.lciBarcode, this.lciAssignTurnCode, this.emptySpaceItem3, this.lciRationSumCode, this.lciReceiveSampleName, this.lciSamplerName, this.lciTestSampleTypeName
			});
            this.Root.Location = new System.Drawing.Point(0, 0);
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(763, 292);
            this.Root.TextVisible = false;
            this.lciPatientName.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciPatientName.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciPatientName.Control = this.lblPatientName;
            this.lciPatientName.Location = new System.Drawing.Point(0, 0);
            this.lciPatientName.Name = "lciPatientName";
            this.lciPatientName.Size = new System.Drawing.Size(279, 26);
            this.lciPatientName.Text = "Họ tên:";
            this.lciPatientName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciPatientName.TextSize = new System.Drawing.Size(95, 20);
            this.lciPatientName.TextToControlDistance = 5;
            this.lciTreatmentCode.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciTreatmentCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciTreatmentCode.Control = this.lblTreatmentCode;
            this.lciTreatmentCode.Location = new System.Drawing.Point(279, 0);
            this.lciTreatmentCode.Name = "lciTreatmentCode";
            this.lciTreatmentCode.Size = new System.Drawing.Size(261, 26);
            this.lciTreatmentCode.Text = "Mã điều trị:";
            this.lciTreatmentCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciTreatmentCode.TextSize = new System.Drawing.Size(80, 20);
            this.lciTreatmentCode.TextToControlDistance = 5;
            this.lciBtnAggrExpMest.Control = this.btnAggrExpMest;
            this.lciBtnAggrExpMest.Location = new System.Drawing.Point(507, 259);
            this.lciBtnAggrExpMest.Name = "lciBtnAggrExpMest";
            this.lciBtnAggrExpMest.Size = new System.Drawing.Size(256, 33);
            this.lciBtnAggrExpMest.TextSize = new System.Drawing.Size(0, 0);
            this.lciBtnAggrExpMest.TextVisible = false;
            this.lciBtnMobaCreate.Control = this.btnMobaCreate;
            this.lciBtnMobaCreate.Location = new System.Drawing.Point(260, 259);
            this.lciBtnMobaCreate.Name = "lciBtnMobaCreate";
            this.lciBtnMobaCreate.Size = new System.Drawing.Size(247, 33);
            this.lciBtnMobaCreate.Text = "Tạo thu hồi";
            this.lciBtnMobaCreate.TextSize = new System.Drawing.Size(0, 0);
            this.lciBtnMobaCreate.TextVisible = false;
            this.lciReqDepartment.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciReqDepartment.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciReqDepartment.Control = this.lblReqDepartment;
            this.lciReqDepartment.Location = new System.Drawing.Point(0, 52);
            this.lciReqDepartment.Name = "lciReqDepartment";
            this.lciReqDepartment.OptionsToolTip.ToolTip = "Khoa yêu cầu";
            this.lciReqDepartment.Size = new System.Drawing.Size(279, 26);
            this.lciReqDepartment.Text = "Khoa y/c:";
            this.lciReqDepartment.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciReqDepartment.TextSize = new System.Drawing.Size(95, 20);
            this.lciReqDepartment.TextToControlDistance = 5;
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(260, 234);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(503, 25);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 234);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(260, 58);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            this.lciRationTime.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciRationTime.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciRationTime.Control = this.lblRationTime;
            this.lciRationTime.CustomizationFormText = "Bữa ăn:";
            this.lciRationTime.Location = new System.Drawing.Point(0, 208);
            this.lciRationTime.Name = "lciRationTime";
            this.lciRationTime.Size = new System.Drawing.Size(279, 26);
            this.lciRationTime.Text = "Bữa ăn:";
            this.lciRationTime.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciRationTime.TextSize = new System.Drawing.Size(95, 20);
            this.lciRationTime.TextToControlDistance = 5;
            this.lciGender.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciGender.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciGender.Control = this.lblGender;
            this.lciGender.Location = new System.Drawing.Point(540, 0);
            this.lciGender.Name = "lciGender";
            this.lciGender.Size = new System.Drawing.Size(223, 26);
            this.lciGender.Text = "Giới tính:";
            this.lciGender.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciGender.TextSize = new System.Drawing.Size(70, 20);
            this.lciGender.TextToControlDistance = 5;
            this.lciExcuteDepartment.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciExcuteDepartment.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciExcuteDepartment.Control = this.lbExcuteDepartment;
            this.lciExcuteDepartment.Location = new System.Drawing.Point(279, 52);
            this.lciExcuteDepartment.Name = "lciExcuteDepartment";
            this.lciExcuteDepartment.Size = new System.Drawing.Size(261, 26);
            this.lciExcuteDepartment.Text = "Khoa xử lý:";
            this.lciExcuteDepartment.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciExcuteDepartment.TextSize = new System.Drawing.Size(80, 20);
            this.lciExcuteDepartment.TextToControlDistance = 5;
            this.lciExpMestStt.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciExpMestStt.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciExpMestStt.Control = this.lblExpMestStt;
            this.lciExpMestStt.Location = new System.Drawing.Point(540, 52);
            this.lciExpMestStt.Name = "lciExpMestStt";
            this.lciExpMestStt.Size = new System.Drawing.Size(223, 26);
            this.lciExpMestStt.Text = "Trạng thái:";
            this.lciExpMestStt.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciExpMestStt.TextSize = new System.Drawing.Size(70, 20);
            this.lciExpMestStt.TextToControlDistance = 5;
            this.layoutControlItem18.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem18.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem18.Control = this.lbDOB;
            this.layoutControlItem18.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem18.Name = "layoutControlItem18";
            this.layoutControlItem18.Size = new System.Drawing.Size(279, 26);
            this.layoutControlItem18.Text = "Ngày sinh:";
            this.layoutControlItem18.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem18.TextSize = new System.Drawing.Size(95, 20);
            this.layoutControlItem18.TextToControlDistance = 5;
            this.lciNumOrder.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciNumOrder.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciNumOrder.Control = this.lblSoTT;
            this.lciNumOrder.Location = new System.Drawing.Point(540, 26);
            this.lciNumOrder.MaxSize = new System.Drawing.Size(0, 24);
            this.lciNumOrder.MinSize = new System.Drawing.Size(100, 24);
            this.lciNumOrder.Name = "lciNumOrder";
            this.lciNumOrder.Size = new System.Drawing.Size(223, 26);
            this.lciNumOrder.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciNumOrder.Text = "Số thứ tự:";
            this.lciNumOrder.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciNumOrder.TextSize = new System.Drawing.Size(70, 20);
            this.lciNumOrder.TextToControlDistance = 5;
            this.lciSoTheTM.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciSoTheTM.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciSoTheTM.Control = this.lblSoTheTM;
            this.lciSoTheTM.CustomizationFormText = "Số thẻ KCB TM:";
            this.lciSoTheTM.Location = new System.Drawing.Point(279, 26);
            this.lciSoTheTM.MaxSize = new System.Drawing.Size(0, 24);
            this.lciSoTheTM.MinSize = new System.Drawing.Size(100, 24);
            this.lciSoTheTM.Name = "lciSoTheTM";
            this.lciSoTheTM.Size = new System.Drawing.Size(261, 26);
            this.lciSoTheTM.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciSoTheTM.Text = "Thẻ KCB TM:";
            this.lciSoTheTM.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciSoTheTM.TextSize = new System.Drawing.Size(80, 20);
            this.lciSoTheTM.TextToControlDistance = 5;
            this.lciExpMestRoom.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciExpMestRoom.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciExpMestRoom.Control = this.lblExpMestRoom;
            this.lciExpMestRoom.Location = new System.Drawing.Point(0, 156);
            this.lciExpMestRoom.Name = "lciExpMestRoom";
            this.lciExpMestRoom.Size = new System.Drawing.Size(279, 26);
            this.lciExpMestRoom.Text = "Kho xuất:";
            this.lciExpMestRoom.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciExpMestRoom.TextSize = new System.Drawing.Size(95, 20);
            this.lciExpMestRoom.TextToControlDistance = 5;
            this.lciExpMestCode.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciExpMestCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciExpMestCode.Control = this.lblExpMestCode;
            this.lciExpMestCode.Location = new System.Drawing.Point(279, 156);
            this.lciExpMestCode.Name = "lciExpMestCode";
            this.lciExpMestCode.Size = new System.Drawing.Size(261, 26);
            this.lciExpMestCode.Text = "Mã xuất:";
            this.lciExpMestCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciExpMestCode.TextSize = new System.Drawing.Size(80, 20);
            this.lciExpMestCode.TextToControlDistance = 5;
            this.lciAggrExpMestCode.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciAggrExpMestCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciAggrExpMestCode.Control = this.lblAggrExpMestCode;
            this.lciAggrExpMestCode.Location = new System.Drawing.Point(540, 156);
            this.lciAggrExpMestCode.Name = "lciAggrExpMestCode";
            this.lciAggrExpMestCode.Size = new System.Drawing.Size(223, 26);
            this.lciAggrExpMestCode.Text = "Mã phiếu lĩnh:";
            this.lciAggrExpMestCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciAggrExpMestCode.TextSize = new System.Drawing.Size(76, 20);
            this.lciAggrExpMestCode.TextToControlDistance = 5;
            this.layoutControlItem11.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem11.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem11.Control = this.lblSoThang;
            this.layoutControlItem11.Location = new System.Drawing.Point(540, 182);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(223, 26);
            this.layoutControlItem11.Text = "Số thang:";
            this.layoutControlItem11.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(70, 20);
            this.layoutControlItem11.TextToControlDistance = 5;
            this.lciIsKidney.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciIsKidney.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciIsKidney.Control = this.chkIsKidney;
            this.lciIsKidney.Location = new System.Drawing.Point(0, 182);
            this.lciIsKidney.Name = "lciIsKidney";
            this.lciIsKidney.Size = new System.Drawing.Size(279, 26);
            this.lciIsKidney.Text = "Đơn chạy thận:";
            this.lciIsKidney.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciIsKidney.TextSize = new System.Drawing.Size(95, 20);
            this.lciIsKidney.TextToControlDistance = 5;
            this.lciIsHomePres.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciIsHomePres.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciIsHomePres.Control = this.chkIsHomePres;
            this.lciIsHomePres.Location = new System.Drawing.Point(279, 182);
            this.lciIsHomePres.Name = "lciIsHomePres";
            this.lciIsHomePres.Size = new System.Drawing.Size(261, 26);
            this.lciIsHomePres.Text = "Đơn mang về:";
            this.lciIsHomePres.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciIsHomePres.TextSize = new System.Drawing.Size(80, 20);
            this.lciIsHomePres.TextToControlDistance = 5;
            this.lciReqSended.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciReqSended.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciReqSended.Control = this.chkReqSended;
            this.lciReqSended.Location = new System.Drawing.Point(0, 78);
            this.lciReqSended.Name = "lciReqSended";
            this.lciReqSended.OptionsToolTip.ToolTip = "Đã gửi thông tin chỉ định sang hệ thống tích hợp (LIS, PACS, ...)";
            this.lciReqSended.Size = new System.Drawing.Size(279, 26);
            this.lciReqSended.Text = "Đã gửi yêu cầu:";
            this.lciReqSended.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciReqSended.TextSize = new System.Drawing.Size(95, 20);
            this.lciReqSended.TextToControlDistance = 5;
            this.lciBarcode.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciBarcode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciBarcode.Control = this.lblBarcode;
            this.lciBarcode.Location = new System.Drawing.Point(279, 78);
            this.lciBarcode.Name = "lciBarcode";
            this.lciBarcode.OptionsToolTip.ToolTip = "Barcode xét nghiệm";
            this.lciBarcode.Size = new System.Drawing.Size(261, 26);
            this.lciBarcode.Text = "Barcode XN:";
            this.lciBarcode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciBarcode.TextSize = new System.Drawing.Size(80, 20);
            this.lciBarcode.TextToControlDistance = 5;
            this.lciAssignTurnCode.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciAssignTurnCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciAssignTurnCode.Control = this.lblAssignTurnCode;
            this.lciAssignTurnCode.Location = new System.Drawing.Point(540, 78);
            this.lciAssignTurnCode.Name = "lciAssignTurnCode";
            this.lciAssignTurnCode.OptionsToolTip.ToolTip = "Mã lượt chỉ định";
            this.lciAssignTurnCode.Size = new System.Drawing.Size(223, 26);
            this.lciAssignTurnCode.Text = "Mã lượt CĐ:";
            this.lciAssignTurnCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciAssignTurnCode.TextSize = new System.Drawing.Size(70, 20);
            this.lciAssignTurnCode.TextToControlDistance = 5;
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(540, 208);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(223, 26);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            this.lciRationSumCode.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciRationSumCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciRationSumCode.Control = this.lblRationSumCode;
            this.lciRationSumCode.Location = new System.Drawing.Point(279, 208);
            this.lciRationSumCode.Name = "lciRationSumCode";
            this.lciRationSumCode.OptionsToolTip.ToolTip = "Mã phiếu tổng hợp";
            this.lciRationSumCode.Size = new System.Drawing.Size(261, 26);
            this.lciRationSumCode.Text = "Mã phiếu TH:";
            this.lciRationSumCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciRationSumCode.TextSize = new System.Drawing.Size(80, 20);
            this.lciRationSumCode.TextToControlDistance = 5;
            this.lciReceiveSampleName.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciReceiveSampleName.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciReceiveSampleName.Control = this.lblReceiveSampleName;
            this.lciReceiveSampleName.Location = new System.Drawing.Point(0, 130);
            this.lciReceiveSampleName.Name = "lciReceiveSampleName";
            this.lciReceiveSampleName.Size = new System.Drawing.Size(763, 26);
            this.lciReceiveSampleName.Text = "Người nhận mẫu:";
            this.lciReceiveSampleName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciReceiveSampleName.TextSize = new System.Drawing.Size(95, 20);
            this.lciReceiveSampleName.TextToControlDistance = 5;
            this.lciReceiveSampleName.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            this.lciSamplerName.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciSamplerName.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciSamplerName.Control = this.lblSamplerName;
            this.lciSamplerName.Location = new System.Drawing.Point(0, 104);
            this.lciSamplerName.Name = "lciSamplerName";
            this.lciSamplerName.Size = new System.Drawing.Size(279, 26);
            this.lciSamplerName.Text = "Người lấy mẫu:";
            this.lciSamplerName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciSamplerName.TextSize = new System.Drawing.Size(95, 20);
            this.lciSamplerName.TextToControlDistance = 5;
            this.lciSamplerName.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            this.lciTestSampleTypeName.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciTestSampleTypeName.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciTestSampleTypeName.Control = this.lblTestSampleTypeName;
            this.lciTestSampleTypeName.Location = new System.Drawing.Point(279, 104);
            this.lciTestSampleTypeName.Name = "lciTestSampleTypeName";
            this.lciTestSampleTypeName.Size = new System.Drawing.Size(484, 26);
            this.lciTestSampleTypeName.Text = "Loại mẫu:";
            this.lciTestSampleTypeName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciTestSampleTypeName.TextSize = new System.Drawing.Size(80, 20);
            this.lciTestSampleTypeName.TextToControlDistance = 5;
            this.cboFilter.EnterMoveNextControl = true;
            this.cboFilter.Location = new System.Drawing.Point(872, 31);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(4);
            this.cboFilter.MenuManager = this.barManager1;
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboFilter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
            this.cboFilter.Properties.NullText = "";
            this.cboFilter.Size = new System.Drawing.Size(146, 22);
            this.cboFilter.StyleController = this.layoutControl1;
            this.cboFilter.TabIndex = 18;
            this.dtIntructionTimeTo.EditValue = null;
            this.dtIntructionTimeTo.EnterMoveNextControl = true;
            this.dtIntructionTimeTo.Location = new System.Drawing.Point(247, 3);
            this.dtIntructionTimeTo.Margin = new System.Windows.Forms.Padding(4);
            this.dtIntructionTimeTo.MenuManager = this.barManager1;
            this.dtIntructionTimeTo.Name = "dtIntructionTimeTo";
            this.dtIntructionTimeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
            this.dtIntructionTimeTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
            this.dtIntructionTimeTo.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtIntructionTimeTo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtIntructionTimeTo.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtIntructionTimeTo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtIntructionTimeTo.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtIntructionTimeTo.Size = new System.Drawing.Size(150, 22);
            this.dtIntructionTimeTo.StyleController = this.layoutControl1;
            this.dtIntructionTimeTo.TabIndex = 17;
            this.dtIntructionTimeFrom.EditValue = null;
            this.dtIntructionTimeFrom.EnterMoveNextControl = true;
            this.dtIntructionTimeFrom.Location = new System.Drawing.Point(68, 3);
            this.dtIntructionTimeFrom.Margin = new System.Windows.Forms.Padding(4);
            this.dtIntructionTimeFrom.MenuManager = this.barManager1;
            this.dtIntructionTimeFrom.Name = "dtIntructionTimeFrom";
            this.dtIntructionTimeFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
            this.dtIntructionTimeFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
            this.dtIntructionTimeFrom.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtIntructionTimeFrom.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtIntructionTimeFrom.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtIntructionTimeFrom.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtIntructionTimeFrom.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtIntructionTimeFrom.Size = new System.Drawing.Size(173, 22);
            this.dtIntructionTimeFrom.StyleController = this.layoutControl1;
            this.dtIntructionTimeFrom.TabIndex = 16;
            this.cboServiceReqType.EnterMoveNextControl = true;
            this.cboServiceReqType.Location = new System.Drawing.Point(999, 3);
            this.cboServiceReqType.Margin = new System.Windows.Forms.Padding(4);
            this.cboServiceReqType.Name = "cboServiceReqType";
            this.cboServiceReqType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboServiceReqType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance, appearanceHovered, appearancePressed, appearanceDisabled, "", null, null, true)
			});
            this.cboServiceReqType.Properties.NullText = "";
            this.cboServiceReqType.Properties.View = this.gridView1;
            this.cboServiceReqType.Size = new System.Drawing.Size(149, 22);
            this.cboServiceReqType.StyleController = this.layoutControl1;
            this.cboServiceReqType.TabIndex = 14;
            this.cboServiceReqType.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboServiceReqType_Closed);
            this.cboServiceReqType.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(cboServiceReqType_CustomDisplayText);
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.KeyUp += new System.Windows.Forms.KeyEventHandler(gridView1_KeyUp);
            this.cboServiceReqStt.EnterMoveNextControl = true;
            this.cboServiceReqStt.Location = new System.Drawing.Point(812, 3);
            this.cboServiceReqStt.Margin = new System.Windows.Forms.Padding(4);
            this.cboServiceReqStt.Name = "cboServiceReqStt";
            this.cboServiceReqStt.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboServiceReqStt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance2, appearanceHovered2, appearancePressed2, appearanceDisabled2, "", null, null, true)
			});
            this.cboServiceReqStt.Properties.NullText = "";
            this.cboServiceReqStt.Properties.View = this.gridLookUpEdit1View;
            this.cboServiceReqStt.Size = new System.Drawing.Size(126, 22);
            this.cboServiceReqStt.StyleController = this.layoutControl1;
            this.cboServiceReqStt.TabIndex = 13;
            this.cboServiceReqStt.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboServiceReqStt_Closed);
            this.cboServiceReqStt.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(cboServiceReqStt_CustomDisplayText);
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.gridLookUpEdit1View.KeyUp += new System.Windows.Forms.KeyEventHandler(gridLookUpEdit1View_KeyUp);
            this.grdSereServServiceReq.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grdSereServServiceReq.Location = new System.Drawing.Point(1154, 328);
            this.grdSereServServiceReq.MainView = this.grdViewSereServServiceReq;
            this.grdSereServServiceReq.Margin = new System.Windows.Forms.Padding(4);
            this.grdSereServServiceReq.Name = "grdSereServServiceReq";
            this.grdSereServServiceReq.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[9] { this.repositoryItemButtonView, this.repositoryItemButtonPrint, this.repositoryItemButtonEdit3, this.repositoryItemTextEditDisable, this.repositoryItemButtonIsAcceptNoExecute, this.repositoryItemButtonEditDeleteEna, this.repositoryItemButtonEditDeleteDis, this.repositoryItemButtonEditServiceConfirmEna, this.repositoryItemButtonEditServiceConfirmDis });
            this.grdSereServServiceReq.Size = new System.Drawing.Size(767, 383);
            this.grdSereServServiceReq.TabIndex = 12;
            this.grdSereServServiceReq.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.grdViewSereServServiceReq });
            this.grdSereServServiceReq.DataSourceChanged += new System.EventHandler(grdSereServServiceReq_DataSourceChanged);
            this.grdViewSereServServiceReq.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[23]
			{
				this.gridColSerSevSTT, this.gridColSerSevView, this.gridColSerSevPrint, this.gridColumn7, this.gridColumn11, this.gridColumn8, this.gcolServiceTypeName, this.gridColumn1, this.gridColSerSevCode, this.gridColSerSevName,
				this.gridColSerSevPatientTypeName, this.gridColSerSevAmount, this.gridColSerSevPresAmount, this.gridColSerSevUnitName, this.gridColumn_Pttt_Group_Name, this.gridColSerSevConvertRatio, this.gridColSerSevConvertAmount, this.gridColSerSevConvertName, this.gridColSerSevTypeName, this.gridColOtherPrintForm,
				this.GridColumnInReqExeute, this.gridColumn4, this.gridColumn6
			});
            this.grdViewSereServServiceReq.GridControl = this.grdSereServServiceReq;
            this.grdViewSereServServiceReq.GroupCount = 1;
            this.grdViewSereServServiceReq.Name = "grdViewSereServServiceReq";
            this.grdViewSereServServiceReq.OptionsBehavior.AutoExpandAllGroups = true;
            this.grdViewSereServServiceReq.OptionsSelection.MultiSelect = true;
            this.grdViewSereServServiceReq.OptionsView.ColumnAutoWidth = false;
            this.grdViewSereServServiceReq.OptionsView.ShowGroupPanel = false;
            this.grdViewSereServServiceReq.OptionsView.ShowIndicator = false;
            this.grdViewSereServServiceReq.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[1]
			{
				new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.GridColumnInReqExeute, DevExpress.Data.ColumnSortOrder.Ascending)
			});
            this.grdViewSereServServiceReq.CustomDrawGroupRow += new DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventHandler(grdViewSereServServiceReq_CustomDrawGroupRow);
            this.grdViewSereServServiceReq.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(grdViewSereServServiceReq_RowCellStyle);
            this.grdViewSereServServiceReq.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(grdViewSereServServiceReq_CustomRowCellEdit);
            this.grdViewSereServServiceReq.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(grdViewSereServServiceReq_PopupMenuShowing);
            this.grdViewSereServServiceReq.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(grdViewSereServServiceReq_CustomUnboundColumnData);
            this.gridColSerSevSTT.Caption = "STT";
            this.gridColSerSevSTT.FieldName = "STT";
            this.gridColSerSevSTT.Name = "gridColSerSevSTT";
            this.gridColSerSevSTT.OptionsColumn.AllowEdit = false;
            this.gridColSerSevSTT.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColSerSevSTT.Visible = true;
            this.gridColSerSevSTT.VisibleIndex = 0;
            this.gridColSerSevSTT.Width = 41;
            this.gridColSerSevView.Caption = "Xem";
            this.gridColSerSevView.FieldName = "btnView_Tab";
            this.gridColSerSevView.MaxWidth = 20;
            this.gridColSerSevView.Name = "gridColSerSevView";
            this.gridColSerSevView.OptionsColumn.ShowCaption = false;
            this.gridColSerSevView.Visible = true;
            this.gridColSerSevView.VisibleIndex = 1;
            this.gridColSerSevView.Width = 20;
            this.gridColSerSevPrint.Caption = "In";
            this.gridColSerSevPrint.FieldName = "btnPrint_Tab";
            this.gridColSerSevPrint.MaxWidth = 20;
            this.gridColSerSevPrint.Name = "gridColSerSevPrint";
            this.gridColSerSevPrint.OptionsColumn.ShowCaption = false;
            this.gridColSerSevPrint.Visible = true;
            this.gridColSerSevPrint.VisibleIndex = 2;
            this.gridColSerSevPrint.Width = 20;
            this.gridColumn7.Caption = "Cho phép không thực hiện";
            this.gridColumn7.FieldName = "IS_ACCEPTING_NO_EXECUTE_STR";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.ShowCaption = false;
            this.gridColumn7.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 3;
            this.gridColumn7.Width = 20;
            this.gridColumn11.Caption = "Dịch vụ có xác nhận không thực hiện từ phòng xử lý";
            this.gridColumn11.ColumnEdit = this.repositoryItemButtonEditServiceConfirmEna;
            this.gridColumn11.FieldName = "IS_CONFIRM_NO_EXCUTE";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.OptionsColumn.ShowCaption = false;
            this.gridColumn11.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 4;
            this.gridColumn11.Width = 20;
            this.repositoryItemButtonEditServiceConfirmEna.AutoHeight = false;
            this.repositoryItemButtonEditServiceConfirmEna.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEditServiceConfirmEna.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance3, appearanceHovered3, appearancePressed3, appearanceDisabled3, "Dịch vụ có xác nhận không thực hiện từ phòng xử lý", null, null, true)
			});
            this.repositoryItemButtonEditServiceConfirmEna.Name = "repositoryItemButtonEditServiceConfirmEna";
            this.repositoryItemButtonEditServiceConfirmEna.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.gridColumn8.Caption = "gridColumn8";
            this.gridColumn8.FieldName = "SereSerDeleteDQ";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.ShowCaption = false;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 5;
            this.gridColumn8.Width = 20;
            this.gcolServiceTypeName.Caption = "Loại dịch vụ";
            this.gcolServiceTypeName.FieldName = "SERVICE_TYPE_NAME";
            this.gcolServiceTypeName.Name = "gcolServiceTypeName";
            this.gcolServiceTypeName.Width = 200;
            this.gridColumn1.Caption = "Thời gian chỉ định";
            this.gridColumn1.FieldName = "INTRUCTION_TIME_DISPLAY";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn1.Width = 52;
            this.gridColSerSevCode.Caption = "Mã";
            this.gridColSerSevCode.FieldName = "TDL_SERVICE_CODE";
            this.gridColSerSevCode.Name = "gridColSerSevCode";
            this.gridColSerSevCode.OptionsColumn.AllowEdit = false;
            this.gridColSerSevCode.Visible = true;
            this.gridColSerSevCode.VisibleIndex = 6;
            this.gridColSerSevCode.Width = 110;
            this.gridColSerSevName.Caption = "Tên";
            this.gridColSerSevName.FieldName = "TDL_SERVICE_NAME";
            this.gridColSerSevName.Name = "gridColSerSevName";
            this.gridColSerSevName.OptionsColumn.AllowEdit = false;
            this.gridColSerSevName.Visible = true;
            this.gridColSerSevName.VisibleIndex = 7;
            this.gridColSerSevName.Width = 165;
            this.gridColSerSevPatientTypeName.Caption = "Mức ăn/ĐTTT";
            this.gridColSerSevPatientTypeName.FieldName = "PATIENT_TYPE_NAME";
            this.gridColSerSevPatientTypeName.Name = "gridColSerSevPatientTypeName";
            this.gridColSerSevPatientTypeName.OptionsColumn.AllowEdit = false;
            this.gridColSerSevPatientTypeName.ToolTip = "Mức ăn/Đối tượng thanh toán";
            this.gridColSerSevPatientTypeName.Visible = true;
            this.gridColSerSevPatientTypeName.VisibleIndex = 8;
            this.gridColSerSevPatientTypeName.Width = 80;
            this.gridColSerSevAmount.Caption = "Số lượng";
            this.gridColSerSevAmount.FieldName = "AMOUNT";
            this.gridColSerSevAmount.Name = "gridColSerSevAmount";
            this.gridColSerSevAmount.OptionsColumn.AllowEdit = false;
            this.gridColSerSevAmount.Visible = true;
            this.gridColSerSevAmount.VisibleIndex = 9;
            this.gridColSerSevAmount.Width = 77;
            this.gridColSerSevPresAmount.Caption = "SL kê đơn";
            this.gridColSerSevPresAmount.FieldName = "PRES_AMOUNT_DISPLAY";
            this.gridColSerSevPresAmount.Name = "gridColSerSevPresAmount";
            this.gridColSerSevPresAmount.OptionsColumn.AllowEdit = false;
            this.gridColSerSevPresAmount.ToolTip = "Số lượng kê đơn";
            this.gridColSerSevPresAmount.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColSerSevPresAmount.Visible = true;
            this.gridColSerSevPresAmount.VisibleIndex = 10;
            this.gridColSerSevUnitName.Caption = "Đơn vị tính";
            this.gridColSerSevUnitName.FieldName = "SERVICE_UNIT_NAME";
            this.gridColSerSevUnitName.Name = "gridColSerSevUnitName";
            this.gridColSerSevUnitName.OptionsColumn.AllowEdit = false;
            this.gridColSerSevUnitName.Visible = true;
            this.gridColSerSevUnitName.VisibleIndex = 11;
            this.gridColSerSevUnitName.Width = 111;
            this.gridColumn_Pttt_Group_Name.Caption = "Loại PTTT";
            this.gridColumn_Pttt_Group_Name.FieldName = "PTTT_GROUP_NAME";
            this.gridColumn_Pttt_Group_Name.Name = "gridColumn_Pttt_Group_Name";
            this.gridColumn_Pttt_Group_Name.OptionsColumn.AllowEdit = false;
            this.gridColumn_Pttt_Group_Name.Visible = true;
            this.gridColumn_Pttt_Group_Name.VisibleIndex = 12;
            this.gridColSerSevConvertRatio.Caption = "Tỉ lệ quy đổi";
            this.gridColSerSevConvertRatio.FieldName = "CONVERT_RATIO";
            this.gridColSerSevConvertRatio.Name = "gridColSerSevConvertRatio";
            this.gridColSerSevConvertRatio.OptionsColumn.AllowEdit = false;
            this.gridColSerSevConvertRatio.Visible = true;
            this.gridColSerSevConvertRatio.VisibleIndex = 13;
            this.gridColSerSevConvertRatio.Width = 80;
            this.gridColSerSevConvertAmount.Caption = "Số lượng quy đổi";
            this.gridColSerSevConvertAmount.FieldName = "CONVERT_AMOUNT";
            this.gridColSerSevConvertAmount.Name = "gridColSerSevConvertAmount";
            this.gridColSerSevConvertAmount.OptionsColumn.AllowEdit = false;
            this.gridColSerSevConvertAmount.Visible = true;
            this.gridColSerSevConvertAmount.VisibleIndex = 14;
            this.gridColSerSevConvertAmount.Width = 90;
            this.gridColSerSevConvertName.Caption = "Đơn vị quy đổi";
            this.gridColSerSevConvertName.FieldName = "CONVERT_UNIT_NAME";
            this.gridColSerSevConvertName.Name = "gridColSerSevConvertName";
            this.gridColSerSevConvertName.OptionsColumn.AllowEdit = false;
            this.gridColSerSevConvertName.Visible = true;
            this.gridColSerSevConvertName.VisibleIndex = 15;
            this.gridColSerSevConvertName.Width = 110;
            this.gridColSerSevTypeName.Caption = "Đối tượng thanh toán";
            this.gridColSerSevTypeName.FieldName = "PATIENT_TYPE_NAME";
            this.gridColSerSevTypeName.Name = "gridColSerSevTypeName";
            this.gridColSerSevTypeName.OptionsColumn.AllowEdit = false;
            this.gridColSerSevTypeName.Width = 57;
            this.gridColOtherPrintForm.Caption = "In biểu mẫu khác";
            this.gridColOtherPrintForm.FieldName = "PRINT_OTHER_FORM";
            this.gridColOtherPrintForm.Name = "gridColOtherPrintForm";
            this.gridColOtherPrintForm.OptionsColumn.ShowCaption = false;
            this.gridColOtherPrintForm.Width = 20;
            this.GridColumnInReqExeute.Caption = "Trạng thái:";
            this.GridColumnInReqExeute.FieldName = "IN_REQ_EXECUTE";
            this.GridColumnInReqExeute.Name = "GridColumnInReqExeute";
            this.GridColumnInReqExeute.Visible = true;
            this.GridColumnInReqExeute.VisibleIndex = 6;
            this.gridColumn4.Caption = "Hướng dẫn sử dụng";
            this.gridColumn4.FieldName = "HuongDanSuDung";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 16;
            this.gridColumn4.Width = 242;
            this.gridColumn6.Caption = "Tốc độ truyền";
            this.gridColumn6.FieldName = "TocDoTruyen";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 17;
            this.gridColumn6.Width = 60;
            this.repositoryItemButtonView.AutoHeight = false;
            this.repositoryItemButtonView.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonView.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance4, appearanceHovered4, appearancePressed4, appearanceDisabled4, "Xem", null, null, true)
			});
            this.repositoryItemButtonView.Name = "repositoryItemButtonView";
            this.repositoryItemButtonView.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonView.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemButtonView_ButtonClick);
            this.repositoryItemButtonPrint.AutoHeight = false;
            this.repositoryItemButtonPrint.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonPrint.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance5, appearanceHovered5, appearancePressed5, appearanceDisabled5, "In ấn", null, null, true)
			});
            this.repositoryItemButtonPrint.Name = "repositoryItemButtonPrint";
            this.repositoryItemButtonPrint.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonPrint.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemButtonPrint_ButtonClick);
            this.repositoryItemButtonEdit3.AutoHeight = false;
            this.repositoryItemButtonEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEdit3.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance6, appearanceHovered6, appearancePressed6, appearanceDisabled6, "In biểu mẫu khác", null, null, true)
			});
            this.repositoryItemButtonEdit3.Name = "repositoryItemButtonEdit3";
            this.repositoryItemButtonEdit3.ReadOnly = true;
            this.repositoryItemButtonEdit3.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemTextEditDisable.AutoHeight = false;
            this.repositoryItemTextEditDisable.Name = "repositoryItemTextEditDisable";
            this.repositoryItemTextEditDisable.ReadOnly = true;
            this.repositoryItemButtonIsAcceptNoExecute.AutoHeight = false;
            this.repositoryItemButtonIsAcceptNoExecute.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonIsAcceptNoExecute.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance7, appearanceHovered7, appearancePressed7, appearanceDisabled7, "Cho phép không thực hiện", null, null, true)
			});
            this.repositoryItemButtonIsAcceptNoExecute.Name = "repositoryItemButtonIsAcceptNoExecute";
            this.repositoryItemButtonIsAcceptNoExecute.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEditDeleteEna.AutoHeight = false;
            this.repositoryItemButtonEditDeleteEna.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEditDeleteEna.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance8, appearanceHovered8, appearancePressed8, appearanceDisabled8, "Xóa", null, null, true)
			});
            this.repositoryItemButtonEditDeleteEna.Name = "repositoryItemButtonEditDeleteEna";
            this.repositoryItemButtonEditDeleteEna.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEditDeleteEna.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemButtonEditDeleteEna_ButtonClick);
            this.repositoryItemButtonEditDeleteDis.AutoHeight = false;
            this.repositoryItemButtonEditDeleteDis.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEditDeleteDis.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance9, appearanceHovered9, appearancePressed9, appearanceDisabled9, "", null, null, true)
			});
            this.repositoryItemButtonEditDeleteDis.Name = "repositoryItemButtonEditDeleteDis";
            this.repositoryItemButtonEditDeleteDis.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEditServiceConfirmDis.AutoHeight = false;
            this.repositoryItemButtonEditServiceConfirmDis.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEditServiceConfirmDis.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance10, appearanceHovered10, appearancePressed10, appearanceDisabled10, "", null, null, true)
			});
            this.repositoryItemButtonEditServiceConfirmDis.Name = "repositoryItemButtonEditServiceConfirmDis";
            this.repositoryItemButtonEditServiceConfirmDis.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.txtServiceReqCode.EditValue = "";
            this.txtServiceReqCode.EnterMoveNextControl = true;
            this.txtServiceReqCode.Location = new System.Drawing.Point(3, 31);
            this.txtServiceReqCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtServiceReqCode.Name = "txtServiceReqCode";
            this.txtServiceReqCode.Properties.NullValuePrompt = "Mã yêu cầu";
            this.txtServiceReqCode.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtServiceReqCode.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtServiceReqCode.Size = new System.Drawing.Size(150, 22);
            this.txtServiceReqCode.StyleController = this.layoutControl1;
            this.txtServiceReqCode.TabIndex = 10;
            this.txtServiceReqCode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtServiceReqCode_PreviewKeyDown);
            this.txtKeyword.EditValue = "";
            this.txtKeyword.EnterMoveNextControl = true;
            this.txtKeyword.Location = new System.Drawing.Point(663, 31);
            this.txtKeyword.Margin = new System.Windows.Forms.Padding(4);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Properties.NullValuePrompt = "Từ khóa tìm kiếm";
            this.txtKeyword.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtKeyword.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtKeyword.Size = new System.Drawing.Size(203, 22);
            this.txtKeyword.StyleController = this.layoutControl1;
            this.txtKeyword.TabIndex = 5;
            this.txtKeyword.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtKeyword_PreviewKeyDown);
            this.btnFind.Location = new System.Drawing.Point(1024, 31);
            this.btnFind.Margin = new System.Windows.Forms.Padding(4);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(124, 27);
            this.btnFind.StyleController = this.layoutControl1;
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "Tìm (Ctrl F)";
            this.btnFind.Click += new System.EventHandler(btnFind_Click);
            this.ucPaging1.Location = new System.Drawing.Point(3, 684);
            this.ucPaging1.Margin = new System.Windows.Forms.Padding(4);
            this.ucPaging1.Name = "ucPaging1";
            this.ucPaging1.Size = new System.Drawing.Size(557, 22);
            this.ucPaging1.TabIndex = 9;
            this.gridControlServiceReq.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControlServiceReq.Location = new System.Drawing.Point(3, 64);
            this.gridControlServiceReq.MainView = this.gridViewServiceReq;
            this.gridControlServiceReq.Margin = new System.Windows.Forms.Padding(4);
            this.gridControlServiceReq.Name = "gridControlServiceReq";
            this.gridControlServiceReq.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[19]
			{
				this.repositoryItemBtnDeleteServiceReq, this.repositoryItemBtnDeleteServiceReqDisable, this.repositoryItemBtnEditServiceReq, this.repositoryItemBtnPrintServiceReq, this.repositoryItemBtnEditServiceReqDisable, this.repositoryItemBtnPrintServiceReqDisable, this.repositoryItempicServiceReqStatus, this.repositoryItemReadOnly, this.repositoryItemBtnBieuMauKhac, this.repositoryItemButtonEditIntructionTime,
				this.repositoryItemCheckEditChoose, this.Btn_EvenLog, this.repositoryItemCheckEditMainExam, this.repositoryItemButton__BieuMauKhac, this.repositoryItemButtonEditAllowNotExecute_Enable, this.repositoryItemButtonEditAllowNotExecute_Disable, this.repositoryItemTextEdit, this.repositoryItemButtonEditIsConfirm_Ena, this.repositoryItemButtonEditIsConfirm_Dis
			});
            this.gridControlServiceReq.Size = new System.Drawing.Size(1145, 614);
            this.gridControlServiceReq.TabIndex = 4;
            this.gridControlServiceReq.ToolTipController = this.tooltipServiceRequest;
            this.gridControlServiceReq.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewServiceReq });
            this.gridControlServiceReq.Click += new System.EventHandler(gridControlServiceReq_Click);
            this.gridViewServiceReq.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[30]
			{
				this.gridColumn_ServiceReq_Choose, this.gridColumn_Transaction_Stt, this.gridColumn_ServiceReq_Edit, this.gridColumn_AllowNotExecute, this.gridColumn_IsConfirmNoExcute, this.gridColumn_ServiceReq_Delete, this.gridColumn_ServiceReq_Print, this.gridColumn_ServiceReq_EditIntructionTime, this.gridColumn_BieuMauKhac, this.gridColumn5,
				this.gridColumn_ServiceReq_Stt, this.gridColumn2, this.gridColumn_Transaction_TransactionCode, this.gridColumn_Transaction_TreatmentCode, this.gridColumn_Transaction_PatientCode, this.gridColumn_Transaction_VirPatientName, this.gridColumn_Transaction_Amount, this.gridColumn_Transaction_CashierRoomName, this.gridColumn_Transaction_PayFormName, this.gridColumn3,
				this.gridColumn_Transaction_Dob, this.gridColumn_Request_Username, this.gridColumn_Execute_Username, this.grdColRationTime, this.gridColumn_Transaction_GenderName, this.Gc_HisSendOldSystem, this.gridColumn_Transaction_CreateTime, this.gridColumn_Transaction_Creator, this.gridColumn_Transaction_ModifyTime, this.gridColumn_Transaction_Modifier
			});
            this.gridViewServiceReq.GridControl = this.gridControlServiceReq;
            this.gridViewServiceReq.Name = "gridViewServiceReq";
            this.gridViewServiceReq.OptionsView.ColumnAutoWidth = false;
            this.gridViewServiceReq.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridViewServiceReq.OptionsView.ShowGroupPanel = false;
            this.gridViewServiceReq.OptionsView.ShowIndicator = false;
            this.gridViewServiceReq.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(gridViewServiceReq_RowCellStyle);
            this.gridViewServiceReq.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(gridViewServiceReq_CustomRowCellEdit);
            this.gridViewServiceReq.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(gridViewServiceReq_PopupMenuShowing);
            this.gridViewServiceReq.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(gridViewServiceReq_CustomUnboundColumnData);
            this.gridViewServiceReq.MouseDown += new System.Windows.Forms.MouseEventHandler(gridViewServiceReq_MouseDown);
            this.gridColumn_ServiceReq_Choose.Caption = "Chọn";
            this.gridColumn_ServiceReq_Choose.ColumnEdit = this.repositoryItemCheckEditChoose;
            this.gridColumn_ServiceReq_Choose.FieldName = "isCheck";
            this.gridColumn_ServiceReq_Choose.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_ServiceReq_Choose.ImageAlignment = System.Drawing.StringAlignment.Center;
            this.gridColumn_ServiceReq_Choose.Name = "gridColumn_ServiceReq_Choose";
            this.gridColumn_ServiceReq_Choose.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Choose.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Choose.OptionsColumn.ShowCaption = false;
            this.gridColumn_ServiceReq_Choose.OptionsFilter.AllowFilter = false;
            this.gridColumn_ServiceReq_Choose.Visible = true;
            this.gridColumn_ServiceReq_Choose.VisibleIndex = 0;
            this.gridColumn_ServiceReq_Choose.Width = 25;
            this.repositoryItemCheckEditChoose.AutoHeight = false;
            this.repositoryItemCheckEditChoose.Name = "repositoryItemCheckEditChoose";
            this.repositoryItemCheckEditChoose.CheckedChanged += new System.EventHandler(repositoryItemCheckEditChoose_CheckedChanged);
            this.gridColumn_Transaction_Stt.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Transaction_Stt.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn_Transaction_Stt.Caption = "STT";
            this.gridColumn_Transaction_Stt.FieldName = "STT";
            this.gridColumn_Transaction_Stt.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_Transaction_Stt.Name = "gridColumn_Transaction_Stt";
            this.gridColumn_Transaction_Stt.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_Stt.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_Transaction_Stt.OptionsFilter.AllowFilter = false;
            this.gridColumn_Transaction_Stt.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_Transaction_Stt.Visible = true;
            this.gridColumn_Transaction_Stt.VisibleIndex = 1;
            this.gridColumn_Transaction_Stt.Width = 27;
            this.gridColumn_ServiceReq_Edit.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_ServiceReq_Edit.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_ServiceReq_Edit.Caption = "Sửa";
            this.gridColumn_ServiceReq_Edit.FieldName = "ServiceReqEdit";
            this.gridColumn_ServiceReq_Edit.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_ServiceReq_Edit.Name = "gridColumn_ServiceReq_Edit";
            this.gridColumn_ServiceReq_Edit.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Edit.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Edit.OptionsColumn.ShowCaption = false;
            this.gridColumn_ServiceReq_Edit.OptionsFilter.AllowFilter = false;
            this.gridColumn_ServiceReq_Edit.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_ServiceReq_Edit.Visible = true;
            this.gridColumn_ServiceReq_Edit.VisibleIndex = 2;
            this.gridColumn_ServiceReq_Edit.Width = 22;
            this.gridColumn_AllowNotExecute.Caption = "Không cho thực hiện";
            this.gridColumn_AllowNotExecute.ColumnEdit = this.repositoryItemButtonEditAllowNotExecute_Enable;
            this.gridColumn_AllowNotExecute.FieldName = "AllowNotExecute";
            this.gridColumn_AllowNotExecute.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_AllowNotExecute.Name = "gridColumn_AllowNotExecute";
            this.gridColumn_AllowNotExecute.OptionsColumn.ShowCaption = false;
            this.gridColumn_AllowNotExecute.Visible = true;
            this.gridColumn_AllowNotExecute.VisibleIndex = 3;
            this.gridColumn_AllowNotExecute.Width = 22;
            this.repositoryItemButtonEditAllowNotExecute_Enable.AutoHeight = false;
            this.repositoryItemButtonEditAllowNotExecute_Enable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEditAllowNotExecute_Enable.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance11, appearanceHovered11, appearancePressed11, appearanceDisabled11, "Cho phép không thực hiện", null, null, true)
			});
            this.repositoryItemButtonEditAllowNotExecute_Enable.Name = "repositoryItemButtonEditAllowNotExecute_Enable";
            this.repositoryItemButtonEditAllowNotExecute_Enable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEditAllowNotExecute_Enable.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemButtonEditAllowNotExecute_Enable_ButtonClick);
            this.gridColumn_IsConfirmNoExcute.Caption = "Dịch vụ có xác nhận không thực hiện từ phòng xử lý";
            this.gridColumn_IsConfirmNoExcute.ColumnEdit = this.repositoryItemButtonEditIsConfirm_Ena;
            this.gridColumn_IsConfirmNoExcute.FieldName = "IsConfirmNoExcute";
            this.gridColumn_IsConfirmNoExcute.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_IsConfirmNoExcute.Name = "gridColumn_IsConfirmNoExcute";
            this.gridColumn_IsConfirmNoExcute.OptionsColumn.ShowCaption = false;
            this.gridColumn_IsConfirmNoExcute.Width = 22;
            this.repositoryItemButtonEditIsConfirm_Ena.AutoHeight = false;
            this.repositoryItemButtonEditIsConfirm_Ena.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEditIsConfirm_Ena.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance12, appearanceHovered12, appearancePressed12, appearanceDisabled12, "", null, null, true)
			});
            this.repositoryItemButtonEditIsConfirm_Ena.Name = "repositoryItemButtonEditIsConfirm_Ena";
            this.repositoryItemButtonEditIsConfirm_Ena.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.gridColumn_ServiceReq_Delete.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_ServiceReq_Delete.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_ServiceReq_Delete.Caption = "Xóa";
            this.gridColumn_ServiceReq_Delete.FieldName = "ServiceReqDelete";
            this.gridColumn_ServiceReq_Delete.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_ServiceReq_Delete.Name = "gridColumn_ServiceReq_Delete";
            this.gridColumn_ServiceReq_Delete.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Delete.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Delete.OptionsColumn.ShowCaption = false;
            this.gridColumn_ServiceReq_Delete.OptionsFilter.AllowFilter = false;
            this.gridColumn_ServiceReq_Delete.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_ServiceReq_Delete.Visible = true;
            this.gridColumn_ServiceReq_Delete.VisibleIndex = 4;
            this.gridColumn_ServiceReq_Delete.Width = 22;
            this.gridColumn_ServiceReq_Print.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_ServiceReq_Print.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_ServiceReq_Print.Caption = "In";
            this.gridColumn_ServiceReq_Print.FieldName = "ServiceReqPrint";
            this.gridColumn_ServiceReq_Print.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_ServiceReq_Print.Name = "gridColumn_ServiceReq_Print";
            this.gridColumn_ServiceReq_Print.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Print.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Print.OptionsColumn.ShowCaption = false;
            this.gridColumn_ServiceReq_Print.OptionsFilter.AllowFilter = false;
            this.gridColumn_ServiceReq_Print.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_ServiceReq_Print.Visible = true;
            this.gridColumn_ServiceReq_Print.VisibleIndex = 5;
            this.gridColumn_ServiceReq_Print.Width = 22;
            this.gridColumn_ServiceReq_EditIntructionTime.Caption = "Sửa thời gian chỉ định";
            this.gridColumn_ServiceReq_EditIntructionTime.ColumnEdit = this.repositoryItemButtonEditIntructionTime;
            this.gridColumn_ServiceReq_EditIntructionTime.FieldName = "EditIntructionTime";
            this.gridColumn_ServiceReq_EditIntructionTime.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_ServiceReq_EditIntructionTime.Name = "gridColumn_ServiceReq_EditIntructionTime";
            this.gridColumn_ServiceReq_EditIntructionTime.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_EditIntructionTime.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn_ServiceReq_EditIntructionTime.OptionsColumn.ShowCaption = false;
            this.gridColumn_ServiceReq_EditIntructionTime.OptionsFilter.AllowFilter = false;
            this.gridColumn_ServiceReq_EditIntructionTime.Width = 22;
            this.repositoryItemButtonEditIntructionTime.AutoHeight = false;
            this.repositoryItemButtonEditIntructionTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEditIntructionTime.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance13, appearanceHovered13, appearancePressed13, appearanceDisabled13, "Sửa thông tin y lệnh", null, null, true)
			});
            this.repositoryItemButtonEditIntructionTime.Name = "repositoryItemButtonEditIntructionTime";
            this.repositoryItemButtonEditIntructionTime.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEditIntructionTime.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemButtonEditIntructionTime_ButtonClick);
            this.gridColumn_BieuMauKhac.Caption = "Biểu mẫu khác";
            this.gridColumn_BieuMauKhac.FieldName = "BieuMauKhac";
            this.gridColumn_BieuMauKhac.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_BieuMauKhac.Name = "gridColumn_BieuMauKhac";
            this.gridColumn_BieuMauKhac.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_BieuMauKhac.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_BieuMauKhac.OptionsColumn.ShowCaption = false;
            this.gridColumn_BieuMauKhac.OptionsFilter.AllowFilter = false;
            this.gridColumn_BieuMauKhac.Width = 22;
            this.gridColumn5.Caption = "gridColumn5";
            this.gridColumn5.ColumnEdit = this.repositoryItemButton__BieuMauKhac;
            this.gridColumn5.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.ShowCaption = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 6;
            this.gridColumn5.Width = 20;
            this.repositoryItemButton__BieuMauKhac.AutoHeight = false;
            this.repositoryItemButton__BieuMauKhac.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButton__BieuMauKhac.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance14, appearanceHovered14, appearancePressed14, appearanceDisabled14, "Biễu mẫu khác", null, null, true)
			});
            this.repositoryItemButton__BieuMauKhac.Name = "repositoryItemButton__BieuMauKhac";
            this.repositoryItemButton__BieuMauKhac.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButton__BieuMauKhac.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemButton__BieuMauKhac_ButtonClick);
            this.gridColumn_ServiceReq_Stt.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_ServiceReq_Stt.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_ServiceReq_Stt.Caption = "Trạng thái";
            this.gridColumn_ServiceReq_Stt.ColumnEdit = this.repositoryItempicServiceReqStatus;
            this.gridColumn_ServiceReq_Stt.FieldName = "IMG";
            this.gridColumn_ServiceReq_Stt.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_ServiceReq_Stt.Name = "gridColumn_ServiceReq_Stt";
            this.gridColumn_ServiceReq_Stt.OptionsColumn.AllowEdit = false;
            this.gridColumn_ServiceReq_Stt.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Stt.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ServiceReq_Stt.OptionsColumn.ShowCaption = false;
            this.gridColumn_ServiceReq_Stt.OptionsFilter.AllowFilter = false;
            this.gridColumn_ServiceReq_Stt.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_ServiceReq_Stt.Visible = true;
            this.gridColumn_ServiceReq_Stt.VisibleIndex = 7;
            this.gridColumn_ServiceReq_Stt.Width = 22;
            this.repositoryItempicServiceReqStatus.Name = "repositoryItempicServiceReqStatus";
            this.repositoryItempicServiceReqStatus.NullText = " ";
            this.gridColumn2.Caption = "Lịch sử tác động";
            this.gridColumn2.ColumnEdit = this.Btn_EvenLog;
            this.gridColumn2.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.ShowCaption = false;
            this.gridColumn2.Width = 20;
            this.Btn_EvenLog.AutoHeight = false;
            this.Btn_EvenLog.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("Btn_EvenLog.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance15, appearanceHovered15, appearancePressed15, appearanceDisabled15, "Lịch sử tác động", null, null, true)
			});
            this.Btn_EvenLog.Name = "Btn_EvenLog";
            this.Btn_EvenLog.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.Btn_EvenLog.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(Btn_EvenLog_ButtonClick);
            this.gridColumn_Transaction_TransactionCode.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Transaction_TransactionCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn_Transaction_TransactionCode.Caption = "Mã";
            this.gridColumn_Transaction_TransactionCode.ColumnEdit = this.repositoryItemTextEdit;
            this.gridColumn_Transaction_TransactionCode.FieldName = "SERVICE_REQ_CODE";
            this.gridColumn_Transaction_TransactionCode.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn_Transaction_TransactionCode.Name = "gridColumn_Transaction_TransactionCode";
            this.gridColumn_Transaction_TransactionCode.OptionsColumn.FixedWidth = true;
            this.gridColumn_Transaction_TransactionCode.Visible = true;
            this.gridColumn_Transaction_TransactionCode.VisibleIndex = 8;
            this.gridColumn_Transaction_TransactionCode.Width = 110;
            this.repositoryItemTextEdit.AutoHeight = false;
            this.repositoryItemTextEdit.Name = "repositoryItemTextEdit";
            this.repositoryItemTextEdit.ReadOnly = true;
            this.repositoryItemTextEdit.Click += new System.EventHandler(repositoryItemTextEdit_Click);
            this.gridColumn_Transaction_TreatmentCode.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Transaction_TreatmentCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn_Transaction_TreatmentCode.Caption = "Mã điều trị";
            this.gridColumn_Transaction_TreatmentCode.ColumnEdit = this.repositoryItemTextEdit;
            this.gridColumn_Transaction_TreatmentCode.FieldName = "TDL_TREATMENT_CODE";
            this.gridColumn_Transaction_TreatmentCode.Name = "gridColumn_Transaction_TreatmentCode";
            this.gridColumn_Transaction_TreatmentCode.OptionsColumn.FixedWidth = true;
            this.gridColumn_Transaction_TreatmentCode.Visible = true;
            this.gridColumn_Transaction_TreatmentCode.VisibleIndex = 9;
            this.gridColumn_Transaction_TreatmentCode.Width = 110;
            this.gridColumn_Transaction_PatientCode.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Transaction_PatientCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn_Transaction_PatientCode.Caption = "Mã bệnh nhân";
            this.gridColumn_Transaction_PatientCode.ColumnEdit = this.repositoryItemTextEdit;
            this.gridColumn_Transaction_PatientCode.FieldName = "TDL_PATIENT_CODE";
            this.gridColumn_Transaction_PatientCode.Name = "gridColumn_Transaction_PatientCode";
            this.gridColumn_Transaction_PatientCode.Visible = true;
            this.gridColumn_Transaction_PatientCode.VisibleIndex = 10;
            this.gridColumn_Transaction_PatientCode.Width = 100;
            this.gridColumn_Transaction_VirPatientName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Transaction_VirPatientName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_VirPatientName.Caption = "Bệnh nhân";
            this.gridColumn_Transaction_VirPatientName.ColumnEdit = this.repositoryItemTextEdit;
            this.gridColumn_Transaction_VirPatientName.FieldName = "TDL_PATIENT_NAME";
            this.gridColumn_Transaction_VirPatientName.Name = "gridColumn_Transaction_VirPatientName";
            this.gridColumn_Transaction_VirPatientName.Visible = true;
            this.gridColumn_Transaction_VirPatientName.VisibleIndex = 11;
            this.gridColumn_Transaction_VirPatientName.Width = 80;
            this.gridColumn_Transaction_Amount.Caption = "Loại";
            this.gridColumn_Transaction_Amount.FieldName = "SERVICE_REQ_TYPE_NAME";
            this.gridColumn_Transaction_Amount.Name = "gridColumn_Transaction_Amount";
            this.gridColumn_Transaction_Amount.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_Amount.Visible = true;
            this.gridColumn_Transaction_Amount.VisibleIndex = 12;
            this.gridColumn_Transaction_Amount.Width = 65;
            this.gridColumn_Transaction_CashierRoomName.Caption = "Phòng xử lý";
            this.gridColumn_Transaction_CashierRoomName.FieldName = "EXECUTE_ROOM_NAME";
            this.gridColumn_Transaction_CashierRoomName.Name = "gridColumn_Transaction_CashierRoomName";
            this.gridColumn_Transaction_CashierRoomName.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_CashierRoomName.Visible = true;
            this.gridColumn_Transaction_CashierRoomName.VisibleIndex = 13;
            this.gridColumn_Transaction_CashierRoomName.Width = 112;
            this.gridColumn_Transaction_PayFormName.Caption = "Phòng chỉ định";
            this.gridColumn_Transaction_PayFormName.FieldName = "REQUEST_ROOM_NAME";
            this.gridColumn_Transaction_PayFormName.Name = "gridColumn_Transaction_PayFormName";
            this.gridColumn_Transaction_PayFormName.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_PayFormName.Visible = true;
            this.gridColumn_Transaction_PayFormName.VisibleIndex = 14;
            this.gridColumn_Transaction_PayFormName.Width = 106;
            this.gridColumn3.Caption = "Loại khám";
            this.gridColumn3.FieldName = "MAIN_EXAM";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.ToolTip = "Khám chính";
            this.gridColumn3.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 15;
            this.gridColumn3.Width = 65;
            this.gridColumn_Transaction_Dob.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Transaction_Dob.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_Dob.Caption = "Thời gian y lệnh";
            this.gridColumn_Transaction_Dob.FieldName = "INTRUCTION_TIME_STR";
            this.gridColumn_Transaction_Dob.Name = "gridColumn_Transaction_Dob";
            this.gridColumn_Transaction_Dob.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_Dob.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_Transaction_Dob.Visible = true;
            this.gridColumn_Transaction_Dob.VisibleIndex = 16;
            this.gridColumn_Transaction_Dob.Width = 120;
            this.gridColumn_Request_Username.Caption = "Người yêu cầu";
            this.gridColumn_Request_Username.FieldName = "REQUEST_USERNAME_DISPLAY";
            this.gridColumn_Request_Username.Name = "gridColumn_Request_Username";
            this.gridColumn_Request_Username.OptionsColumn.AllowEdit = false;
            this.gridColumn_Request_Username.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_Request_Username.Visible = true;
            this.gridColumn_Request_Username.VisibleIndex = 17;
            this.gridColumn_Request_Username.Width = 120;
            this.gridColumn_Execute_Username.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Execute_Username.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Execute_Username.Caption = "Người thực hiện";
            this.gridColumn_Execute_Username.FieldName = "EXECUTE_USERNAME_DISPLAY";
            this.gridColumn_Execute_Username.Name = "gridColumn_Execute_Username";
            this.gridColumn_Execute_Username.OptionsColumn.AllowEdit = false;
            this.gridColumn_Execute_Username.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_Execute_Username.Visible = true;
            this.gridColumn_Execute_Username.VisibleIndex = 18;
            this.gridColumn_Execute_Username.Width = 120;
            this.grdColRationTime.Caption = "Bữa ăn";
            this.grdColRationTime.FieldName = "RATION_TIME_STR";
            this.grdColRationTime.Name = "grdColRationTime";
            this.grdColRationTime.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.grdColRationTime.Visible = true;
            this.grdColRationTime.VisibleIndex = 19;
            this.grdColRationTime.Width = 70;
            this.gridColumn_Transaction_GenderName.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Transaction_GenderName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_GenderName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Transaction_GenderName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_GenderName.Caption = "Năm sinh";
            this.gridColumn_Transaction_GenderName.FieldName = "YEAR";
            this.gridColumn_Transaction_GenderName.Name = "gridColumn_Transaction_GenderName";
            this.gridColumn_Transaction_GenderName.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_GenderName.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_Transaction_GenderName.Visible = true;
            this.gridColumn_Transaction_GenderName.VisibleIndex = 20;
            this.gridColumn_Transaction_GenderName.Width = 45;
            this.Gc_HisSendOldSystem.Caption = "Gửi HT cũ";
            this.Gc_HisSendOldSystem.FieldName = "IS_INTEGRATE_HIS_SENT_STR";
            this.Gc_HisSendOldSystem.Name = "Gc_HisSendOldSystem";
            this.Gc_HisSendOldSystem.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.Gc_HisSendOldSystem.Visible = true;
            this.Gc_HisSendOldSystem.VisibleIndex = 21;
            this.Gc_HisSendOldSystem.Width = 55;
            this.gridColumn_Transaction_CreateTime.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Transaction_CreateTime.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_CreateTime.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Transaction_CreateTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_CreateTime.Caption = "Thời gian tạo";
            this.gridColumn_Transaction_CreateTime.FieldName = "CREATE_TIME_STR";
            this.gridColumn_Transaction_CreateTime.Name = "gridColumn_Transaction_CreateTime";
            this.gridColumn_Transaction_CreateTime.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_CreateTime.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_Transaction_CreateTime.Visible = true;
            this.gridColumn_Transaction_CreateTime.VisibleIndex = 22;
            this.gridColumn_Transaction_CreateTime.Width = 120;
            this.gridColumn_Transaction_Creator.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Transaction_Creator.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_Creator.Caption = "Người tạo";
            this.gridColumn_Transaction_Creator.FieldName = "CREATOR";
            this.gridColumn_Transaction_Creator.Name = "gridColumn_Transaction_Creator";
            this.gridColumn_Transaction_Creator.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_Creator.Visible = true;
            this.gridColumn_Transaction_Creator.VisibleIndex = 23;
            this.gridColumn_Transaction_ModifyTime.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn_Transaction_ModifyTime.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_ModifyTime.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Transaction_ModifyTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_ModifyTime.Caption = "Thời gian sửa";
            this.gridColumn_Transaction_ModifyTime.FieldName = "MODIFY_TIME_STR";
            this.gridColumn_Transaction_ModifyTime.Name = "gridColumn_Transaction_ModifyTime";
            this.gridColumn_Transaction_ModifyTime.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_ModifyTime.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn_Transaction_ModifyTime.Visible = true;
            this.gridColumn_Transaction_ModifyTime.VisibleIndex = 24;
            this.gridColumn_Transaction_ModifyTime.Width = 120;
            this.gridColumn_Transaction_Modifier.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_Transaction_Modifier.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_Transaction_Modifier.Caption = "Người sửa";
            this.gridColumn_Transaction_Modifier.FieldName = "MODIFIER";
            this.gridColumn_Transaction_Modifier.Name = "gridColumn_Transaction_Modifier";
            this.gridColumn_Transaction_Modifier.OptionsColumn.AllowEdit = false;
            this.gridColumn_Transaction_Modifier.Visible = true;
            this.gridColumn_Transaction_Modifier.VisibleIndex = 25;
            this.repositoryItemBtnDeleteServiceReq.AutoHeight = false;
            this.repositoryItemBtnDeleteServiceReq.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemBtnDeleteServiceReq.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance16, appearanceHovered16, appearancePressed16, appearanceDisabled16, "Xóa", null, null, true)
			});
            this.repositoryItemBtnDeleteServiceReq.Name = "repositoryItemBtnDeleteServiceReq";
            this.repositoryItemBtnDeleteServiceReq.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemBtnDeleteServiceReq.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemBtnServiceReqDelete_ButtonClick);
            this.repositoryItemBtnDeleteServiceReqDisable.AutoHeight = false;
            this.repositoryItemBtnDeleteServiceReqDisable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemBtnDeleteServiceReqDisable.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance17, appearanceHovered17, appearancePressed17, appearanceDisabled17, "", null, null, true)
			});
            this.repositoryItemBtnDeleteServiceReqDisable.Name = "repositoryItemBtnDeleteServiceReqDisable";
            this.repositoryItemBtnDeleteServiceReqDisable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemBtnEditServiceReq.AutoHeight = false;
            this.repositoryItemBtnEditServiceReq.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemBtnEditServiceReq.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance18, appearanceHovered18, appearancePressed18, appearanceDisabled18, "Sửa", null, null, true)
			});
            this.repositoryItemBtnEditServiceReq.Name = "repositoryItemBtnEditServiceReq";
            this.repositoryItemBtnEditServiceReq.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemBtnEditServiceReq.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemBtnServiceReqEdit_ButtonClick);
            this.repositoryItemBtnPrintServiceReq.AutoHeight = false;
            this.repositoryItemBtnPrintServiceReq.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemBtnPrintServiceReq.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance19, appearanceHovered19, appearancePressed19, appearanceDisabled19, "In", null, null, true)
			});
            this.repositoryItemBtnPrintServiceReq.Name = "repositoryItemBtnPrintServiceReq";
            this.repositoryItemBtnPrintServiceReq.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemBtnPrintServiceReq.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemBtnServiceReqPrint_ButtonClick);
            this.repositoryItemBtnEditServiceReqDisable.AutoHeight = false;
            this.repositoryItemBtnEditServiceReqDisable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemBtnEditServiceReqDisable.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance20, appearanceHovered20, appearancePressed20, appearanceDisabled20, "", null, null, true)
			});
            this.repositoryItemBtnEditServiceReqDisable.Name = "repositoryItemBtnEditServiceReqDisable";
            this.repositoryItemBtnEditServiceReqDisable.ReadOnly = true;
            this.repositoryItemBtnEditServiceReqDisable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemBtnPrintServiceReqDisable.AutoHeight = false;
            this.repositoryItemBtnPrintServiceReqDisable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemBtnPrintServiceReqDisable.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance21, appearanceHovered21, appearancePressed21, appearanceDisabled21, "", null, null, true)
			});
            this.repositoryItemBtnPrintServiceReqDisable.Name = "repositoryItemBtnPrintServiceReqDisable";
            this.repositoryItemBtnPrintServiceReqDisable.ReadOnly = true;
            this.repositoryItemBtnPrintServiceReqDisable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemReadOnly.AutoHeight = false;
            this.repositoryItemReadOnly.Name = "repositoryItemReadOnly";
            this.repositoryItemReadOnly.ReadOnly = true;
            this.repositoryItemBtnBieuMauKhac.AutoHeight = false;
            this.repositoryItemBtnBieuMauKhac.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemBtnBieuMauKhac.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance22, appearanceHovered22, appearancePressed22, appearanceDisabled22, "Biểu mẫu khác", null, null, true)
			});
            this.repositoryItemBtnBieuMauKhac.Name = "repositoryItemBtnBieuMauKhac";
            this.repositoryItemBtnBieuMauKhac.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemBtnBieuMauKhac.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemBtnBieuMauKhac_ButtonClick);
            this.repositoryItemCheckEditMainExam.AutoHeight = false;
            this.repositoryItemCheckEditMainExam.Name = "repositoryItemCheckEditMainExam";
            this.repositoryItemCheckEditMainExam.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEditMainExam.ReadOnly = true;
            this.repositoryItemButtonEditAllowNotExecute_Disable.AutoHeight = false;
            this.repositoryItemButtonEditAllowNotExecute_Disable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEditAllowNotExecute_Disable.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance23, appearanceHovered23, appearancePressed23, appearanceDisabled23, "", null, null, true)
			});
            this.repositoryItemButtonEditAllowNotExecute_Disable.Name = "repositoryItemButtonEditAllowNotExecute_Disable";
            this.repositoryItemButtonEditAllowNotExecute_Disable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEditIsConfirm_Dis.AutoHeight = false;
            this.repositoryItemButtonEditIsConfirm_Dis.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, (System.Drawing.Image)resources.GetObject("repositoryItemButtonEditIsConfirm_Dis.Buttons"), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance24, appearanceHovered24, appearancePressed24, appearanceDisabled24, "", null, null, true)
			});
            this.repositoryItemButtonEditIsConfirm_Dis.Name = "repositoryItemButtonEditIsConfirm_Dis";
            this.repositoryItemButtonEditIsConfirm_Dis.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.tooltipServiceRequest.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(tooltipServiceRequest_GetActiveObjectInfo);
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[22]
			{
				this.layoutControlItem1, this.layoutControlItem5, this.lciServiceReqPaging, this.layoutControlItem6, this.layoutControlItem9, this.layoutControlItem10, this.layoutControlItem13, this.layoutControlItem12, this.lciIntructionTimeFrom, this.layoutControlItem7,
				this.layoutControlItem8, this.lciServiceReqCode, this.layoutControlItem2, this.layoutControlItem3, this.lciIntructionTimeTo, this.layoutControlItem14, this.lciKeyword, this.layoutControlItem4, this.layoutControlItem15, this.layoutControlItem17,
				this.layoutControlItem16, this.s
			});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1924, 714);
            this.layoutControlGroup1.TextVisible = false;
            this.layoutControlItem1.Control = this.gridControlServiceReq;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 61);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1151, 620);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            this.layoutControlItem5.Control = this.grdSereServServiceReq;
            this.layoutControlItem5.Location = new System.Drawing.Point(1151, 325);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(773, 389);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            this.lciServiceReqPaging.Control = this.ucPaging1;
            this.lciServiceReqPaging.Location = new System.Drawing.Point(0, 681);
            this.lciServiceReqPaging.MaxSize = new System.Drawing.Size(0, 28);
            this.lciServiceReqPaging.MinSize = new System.Drawing.Size(104, 28);
            this.lciServiceReqPaging.Name = "lciServiceReqPaging";
            this.lciServiceReqPaging.Size = new System.Drawing.Size(563, 33);
            this.lciServiceReqPaging.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciServiceReqPaging.TextSize = new System.Drawing.Size(0, 0);
            this.lciServiceReqPaging.TextVisible = false;
            this.layoutControlItem6.Control = this.groupControlInfo;
            this.layoutControlItem6.Location = new System.Drawing.Point(1151, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(773, 325);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            this.layoutControlItem9.Control = this.btnPrintTotal;
            this.layoutControlItem9.Location = new System.Drawing.Point(887, 681);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(138, 33);
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            this.layoutControlItem10.Control = this.btnPrintMedicine;
            this.layoutControlItem10.Location = new System.Drawing.Point(731, 681);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(156, 33);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            this.layoutControlItem13.Control = this.btnDropDownPrint;
            this.layoutControlItem13.Location = new System.Drawing.Point(613, 681);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(118, 33);
            this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem13.TextVisible = false;
            this.layoutControlItem12.Control = this.btnPrintTemBarcode;
            this.layoutControlItem12.Location = new System.Drawing.Point(1025, 681);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(126, 33);
            this.layoutControlItem12.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem12.TextVisible = false;
            this.lciIntructionTimeFrom.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciIntructionTimeFrom.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciIntructionTimeFrom.Control = this.dtIntructionTimeFrom;
            this.lciIntructionTimeFrom.Location = new System.Drawing.Point(0, 0);
            this.lciIntructionTimeFrom.Name = "lciIntructionTimeFrom";
            this.lciIntructionTimeFrom.OptionsToolTip.ToolTip = "Thời gian y lệnh";
            this.lciIntructionTimeFrom.Size = new System.Drawing.Size(244, 28);
            this.lciIntructionTimeFrom.Text = "TG y lệnh:";
            this.lciIntructionTimeFrom.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciIntructionTimeFrom.TextSize = new System.Drawing.Size(60, 20);
            this.lciIntructionTimeFrom.TextToControlDistance = 5;
            this.layoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem7.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem7.Control = this.cboServiceReqStt;
            this.layoutControlItem7.Location = new System.Drawing.Point(754, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(187, 28);
            this.layoutControlItem7.Text = "T/thái:";
            this.layoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(50, 20);
            this.layoutControlItem7.TextToControlDistance = 5;
            this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem8.Control = this.cboServiceReqType;
            this.layoutControlItem8.Location = new System.Drawing.Point(941, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(210, 28);
            this.layoutControlItem8.Text = "Loại:";
            this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(50, 20);
            this.layoutControlItem8.TextToControlDistance = 5;
            this.lciServiceReqCode.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciServiceReqCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciServiceReqCode.Control = this.txtServiceReqCode;
            this.lciServiceReqCode.Location = new System.Drawing.Point(0, 28);
            this.lciServiceReqCode.Name = "lciServiceReqCode";
            this.lciServiceReqCode.Size = new System.Drawing.Size(156, 33);
            this.lciServiceReqCode.Text = "Mã yêu cầu:";
            this.lciServiceReqCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciServiceReqCode.TextSize = new System.Drawing.Size(0, 0);
            this.lciServiceReqCode.TextToControlDistance = 0;
            this.lciServiceReqCode.TextVisible = false;
            this.layoutControlItem2.Control = this.txtTreatmentCode;
            this.layoutControlItem2.Location = new System.Drawing.Point(156, 28);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(172, 33);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            this.layoutControlItem3.Control = this.btnFind;
            this.layoutControlItem3.Location = new System.Drawing.Point(1021, 28);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(130, 33);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            this.lciIntructionTimeTo.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciIntructionTimeTo.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciIntructionTimeTo.Control = this.dtIntructionTimeTo;
            this.lciIntructionTimeTo.Location = new System.Drawing.Point(244, 0);
            this.lciIntructionTimeTo.Name = "lciIntructionTimeTo";
            this.lciIntructionTimeTo.Size = new System.Drawing.Size(156, 28);
            this.lciIntructionTimeTo.Text = "Đến:";
            this.lciIntructionTimeTo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciIntructionTimeTo.TextSize = new System.Drawing.Size(0, 0);
            this.lciIntructionTimeTo.TextToControlDistance = 0;
            this.lciIntructionTimeTo.TextVisible = false;
            this.layoutControlItem14.Control = this.txtPatientCode;
            this.layoutControlItem14.Location = new System.Drawing.Point(328, 28);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(152, 33);
            this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem14.TextVisible = false;
            this.lciKeyword.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciKeyword.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciKeyword.Control = this.txtKeyword;
            this.lciKeyword.Location = new System.Drawing.Point(660, 28);
            this.lciKeyword.Name = "lciKeyword";
            this.lciKeyword.Size = new System.Drawing.Size(209, 33);
            this.lciKeyword.Text = "Từ khóa tìm kiếm:";
            this.lciKeyword.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciKeyword.TextSize = new System.Drawing.Size(0, 0);
            this.lciKeyword.TextToControlDistance = 0;
            this.lciKeyword.TextVisible = false;
            this.layoutControlItem4.Control = this.cboFilter;
            this.layoutControlItem4.Location = new System.Drawing.Point(869, 28);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(152, 33);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            this.layoutControlItem15.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem15.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem15.Control = this.cboExecuteRoom;
            this.layoutControlItem15.Location = new System.Drawing.Point(400, 0);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(275, 28);
            this.layoutControlItem15.Text = "Phòng xử lý:";
            this.layoutControlItem15.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem15.TextSize = new System.Drawing.Size(70, 20);
            this.layoutControlItem15.TextToControlDistance = 5;
            this.layoutControlItem17.Control = this.chkPK;
            this.layoutControlItem17.Location = new System.Drawing.Point(675, 0);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Size = new System.Drawing.Size(79, 28);
            this.layoutControlItem17.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem17.TextVisible = false;
            this.layoutControlItem16.Control = this.txtStoreCode;
            this.layoutControlItem16.Location = new System.Drawing.Point(480, 28);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Size = new System.Drawing.Size(180, 33);
            this.layoutControlItem16.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem16.TextVisible = false;
            this.s.Control = this.btnConfig;
            this.s.Location = new System.Drawing.Point(563, 681);
            this.s.Name = "s";
            this.s.Size = new System.Drawing.Size(50, 33);
            this.s.TextSize = new System.Drawing.Size(0, 0);
            this.s.TextVisible = false;
            this.imageListPriority.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListPriority.ImageStream");
            this.imageListPriority.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListPriority.Images.SetKeyName(0, "yellow star.png");
            this.imageListPriority.Images.SetKeyName(1, "Red star.png");
            this.imageListPriority.Images.SetKeyName(2, "Ruby_star.png");
            this.imageListPriority.Images.SetKeyName(3, "star_green.png");
            this.imageListPriority.Images.SetKeyName(4, "star_blue.png");
            this.imageListPriority.Images.SetKeyName(5, "Deep_Star_full.png");
            this.imageListPriority.Images.SetKeyName(6, "star_fav_empty.png");
            this.imageListIcon.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListIcon.ImageStream");
            this.imageListIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListIcon.Images.SetKeyName(0, "circle-white.png");
            this.imageListIcon.Images.SetKeyName(1, "circle-yellow.png");
            this.imageListIcon.Images.SetKeyName(2, "circleOrgan.png");
            this.imageListIcon.Images.SetKeyName(3, "circle-red.png");
            this.imageListIcon.Images.SetKeyName(4, "circle-black.png");
            this.imageListIcon.Images.SetKeyName(5, "circle-cancel.png");
            this.imageListIcon.Images.SetKeyName(6, "GREEN.png");
            this.imageListCheck.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListCheck.ImageStream");
            this.imageListCheck.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListCheck.Images.SetKeyName(0, "check-box-empty.png");
            this.imageListCheck.Images.SetKeyName(1, "checked-box.png");
            this.imageListCheck.Images.SetKeyName(2, "selected-box.png");
            this.imageListCheck.Images.SetKeyName(3, "check.jpg");
            this.imageListCheck.Images.SetKeyName(4, "uncheck.jpg");
            this.imageListCheck.Images.SetKeyName(5, "Select.png");
            base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new System.Drawing.Size(1924, 752);
            base.Controls.Add(this.layoutControl1);
            base.Controls.Add(this.barDockControlLeft);
            base.Controls.Add(this.barDockControlRight);
            base.Controls.Add(this.barDockControlBottom);
            base.Controls.Add(this.barDockControlTop);
            base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            base.Margin = new System.Windows.Forms.Padding(4);
            base.Name = "frmServiceReqList";
            base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh sách yêu cầu dịch vụ";
            base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            base.Load += new System.EventHandler(frmServiceReqList_Load);
            base.Controls.SetChildIndex(this.barDockControlTop, 0);
            base.Controls.SetChildIndex(this.barDockControlBottom, 0);
            base.Controls.SetChildIndex(this.barDockControlRight, 0);
            base.Controls.SetChildIndex(this.barDockControlLeft, 0);
            base.Controls.SetChildIndex(this.layoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.popupControlContainer1).EndInit();
            this.popupControlContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.layoutControl3).EndInit();
            this.layoutControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.gridConfig).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.gvConfig).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repCheckConfig).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.imageCollection2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlGroup2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem19).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.txtStoreCode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.chkPK.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.cboExecuteRoom.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.gridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.txtPatientCode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.txtTreatmentCode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.groupControlInfo).EndInit();
            this.groupControlInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.layoutControl2).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.chkReqSended.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.chkIsHomePres.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.chkIsKidney.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciPatientName).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciTreatmentCode).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciBtnAggrExpMest).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciBtnMobaCreate).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciReqDepartment).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciRationTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciGender).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciExcuteDepartment).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciExpMestStt).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem18).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciNumOrder).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciSoTheTM).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciExpMestRoom).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciExpMestCode).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciAggrExpMestCode).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciIsKidney).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciIsHomePres).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciReqSended).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciBarcode).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciAssignTurnCode).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.emptySpaceItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciRationSumCode).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciReceiveSampleName).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciSamplerName).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciTestSampleTypeName).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.cboFilter.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dtIntructionTimeTo.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dtIntructionTimeTo.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dtIntructionTimeFrom.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dtIntructionTimeFrom.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.cboServiceReqType.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.cboServiceReqStt.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.grdSereServServiceReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.grdViewSereServServiceReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditServiceConfirmEna).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonView).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonPrint).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEdit3).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemTextEditDisable).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonIsAcceptNoExecute).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditDeleteEna).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditDeleteDis).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditServiceConfirmDis).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.txtServiceReqCode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.txtKeyword.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.gridControlServiceReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.gridViewServiceReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemCheckEditChoose).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditAllowNotExecute_Enable).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditIsConfirm_Ena).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditIntructionTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButton__BieuMauKhac).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItempicServiceReqStatus).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.Btn_EvenLog).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemTextEdit).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnDeleteServiceReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnDeleteServiceReqDisable).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnEditServiceReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnPrintServiceReq).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnEditServiceReqDisable).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnPrintServiceReqDisable).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemReadOnly).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemBtnBieuMauKhac).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemCheckEditMainExam).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditAllowNotExecute_Disable).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.repositoryItemButtonEditIsConfirm_Dis).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciServiceReqPaging).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem13).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem12).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciIntructionTimeFrom).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciServiceReqCode).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciIntructionTimeTo).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem14).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lciKeyword).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem15).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem17).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.layoutControlItem16).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.s).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void AddKeyIntoDictionaryPrint<T>(T data, Dictionary<string, object> dicParamPlus)
        {
            try
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                if (properties == null || properties.Length <= 0)
                {
                    return;
                }
                PropertyInfo[] array = properties;
                foreach (PropertyInfo pi in array)
                {
                    Func<KeyValuePair<string, object>, bool> predicate = (KeyValuePair<string, object> o) => o.Key == pi.Name;
                    if (string.IsNullOrEmpty(dicParamPlus.SingleOrDefault(predicate).Key))
                    {
                        dicParamPlus.Add(pi.Name, pi.GetValue(data));
                    }
                    else
                    {
                        dicParamPlus[pi.Name] = pi.GetValue(data);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void onClickInKetQuaKhacPlus(object sender, EventArgs e)
        {
            //IL_0081: Unknown result type (might be due to invalid IL or missing references)
            //IL_0087: Expected O, but got Unknown
            //IL_027b: Unknown result type (might be due to invalid IL or missing references)
            //IL_0287: Unknown result type (might be due to invalid IL or missing references)
            //IL_029d: Expected O, but got Unknown
            //IL_029d: Expected O, but got Unknown
            try
            {
                WaitingManager.Show();
                dicParamPlus = new Dictionary<string, object>();
                dicImageBarcodePlus = new Dictionary<string, Inventec.Common.BarcodeLib.Barcode>();
                dicImagePlus = new Dictionary<string, System.Drawing.Image>();
                string printTypeCode = "Mps000208";
                dicParamPlus = new Dictionary<string, object>();
                SAR_PRINT_TYPE sAR_PRINT_TYPE = BackendDataWorker.Get<SAR_PRINT_TYPE>().FirstOrDefault((SAR_PRINT_TYPE o) => o.PRINT_TYPE_CODE == printTypeCode);
                if (sAR_PRINT_TYPE != null)
                {
                    RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                    HisSereServView7Filter hisSereServView7Filter = new HisSereServView7Filter();
                    hisSereServView7Filter.SERVICE_REQ_ID = currentServiceReqPrint.ID;
                    List<V_HIS_SERE_SERV_7> list = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERE_SERV_7>>("api/HisSereServ/GetView7", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServView7Filter, new Action(SessionManager.ActionLostToken), new CommonParam());
                    string text = "";
                    foreach (V_HIS_SERE_SERV_7 item in list)
                    {
                        text = text + item.TDL_SERVICE_NAME + ".....\r\n";
                    }
                    AddKeyIntoDictionaryPrint(currentServiceReqPrint, dicParamPlus);
                    dicParamPlus.Add("SERVICE_NAME", text);
                    MPS.ADO.PatientADO patient = PrintGlobalStore.getPatient(currentServiceReqPrint.TREATMENT_ID);
                    AddKeyIntoDictionaryPrint(patient, dicParamPlus);
                    V_HIS_DEPARTMENT_TRAN departmentTran = PrintGlobalStore.getDepartmentTran(currentServiceReqPrint.TREATMENT_ID);
                    if (departmentTran != null)
                    {
                        dicParamPlus.Add("DEPARTMENT_NAME", departmentTran.DEPARTMENT_NAME);
                    }
                    else
                    {
                        dicParamPlus.Add("DEPARTMENT_NAME", WorkPlace.GetDepartmentName());
                    }
                    dicParamPlus.Add("CURRENT_DATE_SEPARATE_STR", GlobalReportQuery.GetCurrentTime());
                    dicParamPlus.Add("BED_ROOM_NAME", WorkPlace.GetRoomName());
                    dicParamPlus.Add("BED_NAME", "");
                    dicParamPlus.Add("INSTRUCTION_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(currentServiceReqPrint.INTRUCTION_TIME));
                    dicParamPlus.Add("INSTRUCTION_DATE_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(currentServiceReqPrint.INTRUCTION_TIME));
                    dicParamPlus.Add("INSTRUCTION_DATE_SEPAREATE_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(currentServiceReqPrint.INTRUCTION_TIME));
                    WaitingManager.Hide();
                    val.RunPrintTemplate(sAR_PRINT_TYPE.PRINT_TYPE_CODE, sAR_PRINT_TYPE.FILE_PATTERN, "Phiếu xét nghiệm", new DelegateUpdateTableReference(UpdateSereServJsonPrint), new DelegateGetListFromPrintReference(GetListPrintIdByServiceReq), dicParamPlus, dicImagePlus);
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private List<long> GetListPrintIdByServiceReq()
        {
            List<long> list = new List<long>();
            try
            {
                if (currentServiceReqPrint != null && !string.IsNullOrEmpty(currentServiceReqPrint.JSON_PRINT_ID))
                {
                    string[] array = currentServiceReqPrint.JSON_PRINT_ID.Split(',', ';');
                    if (array != null && array.Length > 0)
                    {
                        string[] array2 = array;
                        foreach (string inputValue in array2)
                        {
                            long num = Parse.ToInt64(inputValue);
                            if (num > 0)
                            {
                                list.Add(num);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return list;
        }

        private bool UpdateSereServJsonPrint(SAR_PRINT sarPrintCreated)
        {
            bool result = false;
            try
            {
                if (true && currentServiceReqPrint != null)
                {
                    List<FileHolder> listFileHolder = new List<FileHolder>();
                    HIS_SERVICE_REQ hisServiceReq = new HIS_SERVICE_REQ();
                    List<long> listPrintIdByServiceReq = GetListPrintIdByServiceReq();
                    ProcessServiceReqExecuteForUpdateJsonPrint(ref hisServiceReq, listPrintIdByServiceReq, sarPrintCreated);
                    SaveTestServiceReq(listFileHolder, hisServiceReq);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private void SaveTestServiceReq(List<FileHolder> listFileHolder, HIS_SERVICE_REQ hisServiceReq)
        {
            CommonParam commonParam = new CommonParam();
            bool flag = false;
            try
            {
                WaitingManager.Show();
                HIS_SERVICE_REQ hIS_SERVICE_REQ = new HIS_SERVICE_REQ();
                Mapper.CreateMap<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO, HIS_SERVICE_REQ>();
                hIS_SERVICE_REQ = Mapper.Map<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO, HIS_SERVICE_REQ>(currentServiceReqPrint);
                hisServiceReq.ID = currentServiceReq.ID;
                HIS_SERVICE_REQ hIS_SERVICE_REQ2 = new BackendAdapter(commonParam).Post<HIS_SERVICE_REQ>("/api/HisServiceReq/UpdateJsonPrintId", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReq, new Action(SessionManager.ActionLostToken), commonParam);
                if (hIS_SERVICE_REQ2 != null)
                {
                    flag = true;
                }
                WaitingManager.Hide();
                SessionManager.ProcessTokenLost(commonParam);
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Fatal(ex);
            }
        }

        private void ProcessServiceReqExecuteForUpdateJsonPrint(ref HIS_SERVICE_REQ hisServiceReq, List<long> jsonPrintId, SAR_PRINT sarPrintCreated)
        {
            try
            {
                if (currentServiceReqPrint == null)
                {
                    return;
                }
                if (jsonPrintId == null)
                {
                    jsonPrintId = new List<long>();
                }
                jsonPrintId.Add(sarPrintCreated.ID);
                string text = "";
                foreach (long item in jsonPrintId)
                {
                    text = text + item + ",";
                }
                hisServiceReq.JSON_PRINT_ID = text;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintMedicine_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e.Item is BarButtonItem)
                {
                    BarButtonItem barButtonItem = sender as BarButtonItem;
                    switch ((PrintPopupMenuProcessor.ModuleType)e.Item.Tag)
                    {
                        case PrintPopupMenuProcessor.ModuleType.donThuoc:
                            InDonThuocVatTu();
                            break;
                        case PrintPopupMenuProcessor.ModuleType.thuocTongHop:
                            InPhieuYeuCauThuocVatTuTongHop();
                            break;
                        case PrintPopupMenuProcessor.ModuleType.donThuocYHCT:
                            InPhieuYeuCauDonThuocYHocCoTruyen();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintExam_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e.Item is BarButtonItem)
                {
                    BarButtonItem barButtonItem = sender as BarButtonItem;
                    switch ((PrintPopupMenuProcessor.ModuleType)e.Item.Tag)
                    {
                        case PrintPopupMenuProcessor.ModuleType.chuyenKhoa:
                            InPhieuYeuCauDichVu("Mps000071");
                            break;
                        case PrintPopupMenuProcessor.ModuleType.PhieuThuKiemPhieuYcKham:
                            InPhieuThuKiemYcKham_Click();
                            break;
                        case PrintPopupMenuProcessor.ModuleType.kham:
                            InPhieuYeuCauDichVu("Mps000001");
                            break;
                        case PrintPopupMenuProcessor.ModuleType.TheBenhNhan:
                            InTheBenhNhan_Click();
                            break;
                        case PrintPopupMenuProcessor.ModuleType.HenKhamLai:
                            InHenKhamLai_Click();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintPttt_Click(object sender, ItemClickEventArgs e)
        {
            //IL_0049: Unknown result type (might be due to invalid IL or missing references)
            //IL_004f: Expected O, but got Unknown
            //IL_007a: Unknown result type (might be due to invalid IL or missing references)
            //IL_0084: Expected O, but got Unknown
            //IL_0094: Unknown result type (might be due to invalid IL or missing references)
            //IL_009e: Expected O, but got Unknown
            //IL_00ae: Unknown result type (might be due to invalid IL or missing references)
            //IL_00b8: Expected O, but got Unknown
            //IL_00c8: Unknown result type (might be due to invalid IL or missing references)
            //IL_00d2: Expected O, but got Unknown
            try
            {
                if (e.Item is BarButtonItem)
                {
                    BarButtonItem barButtonItem = sender as BarButtonItem;
                    PrintPopupMenuProcessor.ModuleType moduleType = (PrintPopupMenuProcessor.ModuleType)e.Item.Tag;
                    RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                    switch (moduleType)
                    {
                        case PrintPopupMenuProcessor.ModuleType.Mps000033:
                            val.RunPrintTemplate("Mps000033", new DelegateRunPrinter(DelegateRunPrinter));
                            break;
                        case PrintPopupMenuProcessor.ModuleType.Mps000035:
                            val.RunPrintTemplate("Mps000035", new DelegateRunPrinter(DelegateRunPrinter));
                            break;
                        case PrintPopupMenuProcessor.ModuleType.Mps000097:
                            val.RunPrintTemplate("Mps000097", new DelegateRunPrinter(DelegateRunPrinter));
                            break;
                        case PrintPopupMenuProcessor.ModuleType.Mps000204:
                            val.RunPrintTemplate("Mps000204", new DelegateRunPrinter(DelegateRunPrinter));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintBlood()
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_001c: Expected O, but got Unknown
            //IL_0029: Unknown result type (might be due to invalid IL or missing references)
            //IL_0033: Expected O, but got Unknown
            try
            {
                RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                val.RunPrintTemplate("Mps000108", new DelegateRunPrinter(DelegateRunPrinter));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintTest_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e.Item is BarButtonItem)
                {
                    BarButtonItem barButtonItem = sender as BarButtonItem;
                    switch ((PrintPopupMenuProcessor.ModuleType)e.Item.Tag)
                    {
                        case PrintPopupMenuProcessor.ModuleType._testPhieuYeuCau:
                            InPhieuYeuCauDichVu("Mps000026");
                            break;
                        case PrintPopupMenuProcessor.ModuleType._testDomSoi:
                            onClickInKetQuaKhacPlus(null, null);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessingPrint()
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_001c: Expected O, but got Unknown
            //IL_01d7: Unknown result type (might be due to invalid IL or missing references)
            //IL_01e1: Expected O, but got Unknown
            try
            {
                RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 9)
                {
                    InPhieuYeuCauDichVu("Mps000030");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 8)
                {
                    InPhieuYeuCauDichVu("Mps000029");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 5)
                {
                    InPhieuYeuCauDichVu("Mps000038");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 4)
                {
                    InPhieuYeuCauDichVu("Mps000031");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 10)
                {
                    InPhieuYeuCauDichVu("Mps000036");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 3)
                {
                    InPhieuYeuCauDichVu("Mps000028");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 12)
                {
                    InPhieuYeuCauDichVu("Mps000053");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 11)
                {
                    InPhieuYeuCauDichVu("Mps000040");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 7)
                {
                    InPhieuYeuCauDichVu("Mps000042");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 13)
                {
                    InPhieuYeuCauDichVu("Mps000167");
                }
                else if (currentServiceReqPrint.SERVICE_REQ_TYPE_ID == 17)
                {
                    val.RunPrintTemplate("Mps000275", new DelegateRunPrinter(DelegateRunPrinter));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void ProcessPrintResult()
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_001c: Expected O, but got Unknown
            //IL_0041: Unknown result type (might be due to invalid IL or missing references)
            //IL_004b: Expected O, but got Unknown
            try
            {
                RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                if (sereServPrint.TDL_SERVICE_TYPE_ID == 13)
                {
                    val.RunPrintTemplate("Mps000063", new DelegateRunPrinter(DelegateRunPrinter));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private bool DelegateRunPrinter(string printTypeCode, string fileName)
        {
            bool result = false;
            try
            {
                switch (printTypeCode)
                {
                    case "Mps000033":
                        LoadBieuMauPhieuThuThuatPhauThuat(printTypeCode, fileName, ref result);
                        break;
                    case "Mps000035":
                        LoadBieuMauPhieuYCInGiayCamDoan(printTypeCode, fileName, ref result);
                        break;
                    case "Mps000063":
                        InPhieuKetQuaPHCN(printTypeCode, fileName, ref result);
                        break;
                    case "Mps000097":
                        LoadBieuMauCachThucPhauThuat(printTypeCode, fileName, ref result);
                        break;
                    case "Mps000108":
                        InPhieuYeuCauChiDinhMau(printTypeCode, fileName, ref result);
                        break;
                    case "Mps000204":
                        LoadGiayChungNhanPTTT(printTypeCode, fileName, ref result);
                        break;
                    case "Mps000275":
                        InSuatAn(printTypeCode, fileName, null, ref result);
                        break;
                    case "Mps000433":
                        InGiayDeNghiDoiTraDichVu(printTypeCode, fileName, ref result);
                        break;
                    case "Mps000494":
                        PrintMps494(printTypeCode, fileName, ref result);
                        break;
                    case "Mps190001":
                        PrintMps190001(printTypeCode, fileName, ref result);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private void PrintMps494(string printTypeCode, string fileName, ref bool result)
        {
            try
            {
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (serviceReqADO == null || serviceReqADO.ID <= 0)
                {
                    return;
                }
                HisExpMestMaterialFilter hisExpMestMaterialFilter = new HisExpMestMaterialFilter();
                hisExpMestMaterialFilter.TDL_SERVICE_REQ_ID = serviceReqADO.ID;
                List<HIS_EXP_MEST_MATERIAL> list = new BackendAdapter(new CommonParam()).Get<List<HIS_EXP_MEST_MATERIAL>>("api/HisExpMestMaterial/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestMaterialFilter, null);
                if (list == null || list.Count == 0)
                {
                    XtraMessageBox.Show("Không có vật tư tái sử dụng hoặc vật tư tái sử dụng đã hết số lần tái sử dụng.");
                    return;
                }
                List<HIS_EXP_MEST_MATERIAL> list2 = list.Where((HIS_EXP_MEST_MATERIAL o) => !string.IsNullOrEmpty(o.SERIAL_NUMBER) && o.REMAIN_REUSE_COUNT.HasValue).ToList();
                if (list2 == null || list2.Count == 0)
                {
                    XtraMessageBox.Show("Không có vật tư tái sử dụng hoặc vật tư tái sử dụng đã hết số lần tái sử dụng.");
                    return;
                }
                List<HIS_MATERIAL> list3 = new List<HIS_MATERIAL>();
                List<long> list4 = list2.Select((HIS_EXP_MEST_MATERIAL p) => p.MATERIAL_ID ?? 0).ToList();
                if (list4 != null && list4.Count > 0)
                {
                    HisMaterialFilter hisMaterialFilter = new HisMaterialFilter();
                    hisMaterialFilter.IDs = list4;
                    list3 = new BackendAdapter(new CommonParam()).Get<List<HIS_MATERIAL>>("api/HisMaterial/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisMaterialFilter, null);
                }
                List<SerialADO> lstSend = new List<SerialADO>();
                foreach (HIS_EXP_MEST_MATERIAL m in list2)
                {
                    SerialADO serialADO = new SerialADO();
                    List<HIS_MATERIAL> source = list3;
                    Func<HIS_MATERIAL, bool> predicate = (HIS_MATERIAL o) => o.ID == m.MATERIAL_ID;
                    HIS_MATERIAL hIS_MATERIAL = source.FirstOrDefault(predicate);
                    if (hIS_MATERIAL != null)
                    {
                        serialADO.NEXT_REUSABLE_NUMBER = ((hIS_MATERIAL.MAX_REUSE_COUNT ?? 0) - (m.REMAIN_REUSE_COUNT ?? 0) + 2).ToString();
                        serialADO.SIZE = hIS_MATERIAL.MATERIAL_SIZE;
                        if (int.Parse(serialADO.NEXT_REUSABLE_NUMBER) <= (hIS_MATERIAL.MAX_REUSE_COUNT ?? 0))
                        {
                            serialADO.SERIAL_NUMBER = m.SERIAL_NUMBER;
                            lstSend.Add(serialADO);
                        }
                    }
                }
                LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => lstSend), lstSend));
                if (lstSend == null || lstSend.Count == 0)
                {
                    XtraMessageBox.Show("Không có vật tư tái sử dụng hoặc vật tư tái sử dụng đã hết số lần tái sử dụng.");
                    return;
                }
                Mps000494PDO data = new Mps000494PDO(lstSend);
                RunPrint(printTypeCode, fileName, data, new DelegateEventLog(EventLogPrint), result, currentModule.RoomId);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void EventLogPrint()
        {
            try
            {
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO serviceReqADO = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                string description = "In tem vật tư tái sử dụng. Mã in : Mps000494  TREATMENT_CODE: " + serviceReqADO.TDL_TREATMENT_CODE + "  Thời gian in: " + Inventec.Common.DateTime.Convert.SystemDateTimeToTimeSeparateString(DateTime.Now) + "  Người in: " + ClientTokenManagerStore.ClientTokenManager.GetLoginName();
                Logger.Log(GlobalVariables.APPLICATION_CODE, ClientTokenManagerStore.ClientTokenManager.GetLoginName(), description, ClientTokenManagerStore.ClientTokenManager.GetLoginAddress());
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private HIS_TREATMENT getTreatment(long treatmentId)
        {
            LogSystem.Debug("Begin get HIS_TREATMENT");
            CommonParam commonParam = new CommonParam();
            HIS_TREATMENT result = new HIS_TREATMENT();
            try
            {
                HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
                hisTreatmentFilter.ID = treatmentId;
                List<HIS_TREATMENT> list = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, new Action(SessionManager.ActionLostToken), commonParam);
                if (list != null && list.Count > 0)
                {
                    result = list.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            LogSystem.Debug("End get HIS_TREATMENT");
            return result;
        }

        private V_HIS_PATIENT_TYPE_ALTER getPatientTypeAlter(long treatmentId, long instructTime)
        {
            LogSystem.Debug("Begin get HispatientTypeAlter");
            CommonParam commonParam = new CommonParam();
            V_HIS_PATIENT_TYPE_ALTER v_HIS_PATIENT_TYPE_ALTER = new V_HIS_PATIENT_TYPE_ALTER();
            try
            {
                HisPatientTypeAlterViewAppliedFilter hisPatientTypeAlterViewAppliedFilter = new HisPatientTypeAlterViewAppliedFilter();
                hisPatientTypeAlterViewAppliedFilter.TreatmentId = treatmentId;
                if (instructTime > 0)
                {
                    hisPatientTypeAlterViewAppliedFilter.InstructionTime = instructTime;
                }
                else
                {
                    hisPatientTypeAlterViewAppliedFilter.InstructionTime = Inventec.Common.DateTime.Get.Now() ?? 0;
                }
                v_HIS_PATIENT_TYPE_ALTER = new BackendAdapter(commonParam).Get<V_HIS_PATIENT_TYPE_ALTER>("/api/HisPatientTypeAlter/GetApplied", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientTypeAlterViewAppliedFilter, new Action(SessionManager.ActionLostToken), commonParam);
            }
            catch (Exception ex)
            {
                v_HIS_PATIENT_TYPE_ALTER = null;
                LogSystem.Warn(ex);
            }
            LogSystem.Debug("End get HispatientTypeAlter");
            return v_HIS_PATIENT_TYPE_ALTER;
        }

        private List<V_HIS_SERE_SERV> GetSereServByServiceReqId(long serviceReqId)
        {
            LogSystem.Debug("Begin get List<V_HIS_SERE_SERV>");
            List<V_HIS_SERE_SERV> result = new List<V_HIS_SERE_SERV>();
            try
            {
                CommonParam commonParam = new CommonParam();
                HisSereServViewFilter hisSereServViewFilter = new HisSereServViewFilter();
                hisSereServViewFilter.SERVICE_REQ_ID = serviceReqId;
                List<V_HIS_SERE_SERV> list = new BackendAdapter(commonParam).Get<List<V_HIS_SERE_SERV>>("api/HisSereServ/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServViewFilter, new Action(SessionManager.ActionLostToken), commonParam);
                if (list != null && list.Count > 0)
                {
                    result = list.Where((V_HIS_SERE_SERV o) => o.IS_NO_EXECUTE != 1).ToList();
                }
            }
            catch (Exception ex)
            {
                result = new List<V_HIS_SERE_SERV>();
                LogSystem.Warn(ex);
            }
            LogSystem.Debug("End get List<V_HIS_SERE_SERV>");
            return result;
        }

        private void PrintData(string printTypeCode, string fileName, object data, ref bool result)
        {
            try
            {
                string printerName = "";
                if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
                {
                    printerName = GlobalVariables.dicPrinter[printTypeCode];
                }
                if (ConfigApplications.CheDoInChoCacChucNangTrongPhanMem == 2)
                {
                    result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName));
                    return;
                }
                InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode(treatmentCode, printTypeCode, (currentModule != null) ? currentModule.RoomId : 0);
                result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.Show, printerName)
                {
                    EmrInputADO = emrInputADO
                });
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void InPhieuYeuCauDichVu(string printTypeCode)
        {
            try
            {
                if (serviceReqPrintRaw != null)
                {
                    ThreadChiDinhDichVuADO threadChiDinhDichVuADO = new ThreadChiDinhDichVuADO(currentServiceReqPrint);
                    CreateThreadLoadDataForService(threadChiDinhDichVuADO);
                    V_HIS_PATIENT_TYPE_ALTER hisPatientTypeAlter = null;
                    LoadCurrentPatientTypeAlter(currentServiceReqPrint.TREATMENT_ID, ref hisPatientTypeAlter);
                    threadChiDinhDichVuADO.vHisPatientTypeAlter = hisPatientTypeAlter;
                    HisServiceReqListResultSDO hisServiceReqListResultSDO = new HisServiceReqListResultSDO();
                    hisServiceReqListResultSDO.SereServs = threadChiDinhDichVuADO.listVHisSereServ;
                    hisServiceReqListResultSDO.ServiceReqs = new List<V_HIS_SERVICE_REQ> { serviceReqPrintRaw };
                    hisServiceReqListResultSDO.SereServBills = threadChiDinhDichVuADO.ListSereServBill;
                    hisServiceReqListResultSDO.SereServDeposits = threadChiDinhDichVuADO.ListSereServDeposit;
                    List<V_HIS_BED_LOG> bedLogs = new List<V_HIS_BED_LOG>();
                    HisBedLogViewFilter hisBedLogViewFilter = new HisBedLogViewFilter();
                    hisBedLogViewFilter.TREATMENT_ID = serviceReqPrintRaw.TREATMENT_ID;
                    List<V_HIS_BED_LOG> list = new BackendAdapter(param).Get<List<V_HIS_BED_LOG>>("api/HisBedLog/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisBedLogViewFilter, param);
                    if (list != null)
                    {
                        bedLogs = list;
                    }
                    HisTreatmentWithPatientTypeInfoSDO hisTreatmentWithPatientTypeInfoSDO = new HisTreatmentWithPatientTypeInfoSDO();
                    DataObjectMapper.Map<HisTreatmentWithPatientTypeInfoSDO>(hisTreatmentWithPatientTypeInfoSDO, threadChiDinhDichVuADO.hisTreatment);
                    if (threadChiDinhDichVuADO.vHisPatientTypeAlter != null)
                    {
                        hisTreatmentWithPatientTypeInfoSDO.PATIENT_TYPE_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.PATIENT_TYPE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_FROM_TIME = threadChiDinhDichVuADO.vHisPatientTypeAlter.HEIN_CARD_FROM_TIME ?? 0;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_NUMBER = threadChiDinhDichVuADO.vHisPatientTypeAlter.HEIN_CARD_NUMBER;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_TO_TIME = threadChiDinhDichVuADO.vHisPatientTypeAlter.HEIN_CARD_TO_TIME ?? 0;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_MEDI_ORG_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.HEIN_MEDI_ORG_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.LEVEL_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.LEVEL_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.RIGHT_ROUTE_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.RIGHT_ROUTE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.RIGHT_ROUTE_TYPE_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.RIGHT_ROUTE_TYPE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.TREATMENT_TYPE_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.TREATMENT_TYPE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_ADDRESS = threadChiDinhDichVuADO.vHisPatientTypeAlter.ADDRESS;
                    }
                    PrintServiceReqProcessor printServiceReqProcessor = new PrintServiceReqProcessor(hisServiceReqListResultSDO, hisTreatmentWithPatientTypeInfoSDO, bedLogs, (currentModule != null) ? currentModule.RoomId : 0);
                    printServiceReqProcessor.Print(printTypeCode, false);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                WaitingManager.Hide();
            }
        }

        private void InPhieuThuKiemYcKham_Click()
        {
            //IL_0024: Unknown result type (might be due to invalid IL or missing references)
            //IL_002a: Expected O, but got Unknown
            //IL_0037: Unknown result type (might be due to invalid IL or missing references)
            //IL_0041: Expected O, but got Unknown
            try
            {
                if (serviceReqPrintRaw != null)
                {
                    RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                    val.RunPrintTemplate("Mps000420", new DelegateRunPrinter(InPhieuThuKiemYcKham));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                WaitingManager.Hide();
            }
        }

        private void InTheBenhNhan_Click()
        {
            //IL_0024: Unknown result type (might be due to invalid IL or missing references)
            //IL_002a: Expected O, but got Unknown
            //IL_0037: Unknown result type (might be due to invalid IL or missing references)
            //IL_0041: Expected O, but got Unknown
            try
            {
                if (serviceReqPrintRaw != null)
                {
                    RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                    val.RunPrintTemplate("Mps000178", new DelegateRunPrinter(InTheBenhNhan));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                WaitingManager.Hide();
            }
        }

        private void InHenKhamLai_Click()
        {
            //IL_0024: Unknown result type (might be due to invalid IL or missing references)
            //IL_002a: Expected O, but got Unknown
            //IL_0037: Unknown result type (might be due to invalid IL or missing references)
            //IL_0041: Expected O, but got Unknown
            try
            {
                if (serviceReqPrintRaw != null)
                {
                    RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                    val.RunPrintTemplate("Mps000010", new DelegateRunPrinter(InHenKhamLai));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                WaitingManager.Hide();
            }
        }

        private bool InPhieuThuKiemYcKham(string printTypeCode, string fileName)
        {
            bool result = false;
            try
            {
                if (serviceReqPrintRaw == null)
                {
                    return false;
                }
                WaitingManager.Show();
                string printerName = "";
                HisSereServViewFilter hisSereServViewFilter = new HisSereServViewFilter();
                hisSereServViewFilter.SERVICE_REQ_ID = serviceReqPrintRaw.ID;
                List<V_HIS_SERE_SERV> list = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERE_SERV>>("api/HisSereServ/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServViewFilter, null);
                if (list != null && list.Count > 0)
                {
                    HisSereServDepositFilter hisSereServDepositFilter = new HisSereServDepositFilter();
                    hisSereServDepositFilter.SERE_SERV_IDs = list.Select((V_HIS_SERE_SERV o) => o.ID).ToList();
                    List<HIS_SERE_SERV_DEPOSIT> list2 = new BackendAdapter(new CommonParam()).Get<List<HIS_SERE_SERV_DEPOSIT>>("api/HisSereServDeposit/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServDepositFilter, null);
                    if (list2 != null && list2.Count > 0)
                    {
                        HisTransactionViewFilter hisTransactionViewFilter = new HisTransactionViewFilter();
                        hisTransactionViewFilter.IDs = list2.Select((HIS_SERE_SERV_DEPOSIT o) => o.DEPOSIT_ID).ToList();
                        List<V_HIS_TRANSACTION> list3 = new BackendAdapter(new CommonParam()).Get<List<V_HIS_TRANSACTION>>("api/HisTransaction/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTransactionViewFilter, null);
                        if (list3 != null && list3.Count > 0)
                        {
                            foreach (V_HIS_TRANSACTION item in list3)
                            {
                                string tREATMENT_CODE = serviceReqPrintRaw.TREATMENT_CODE;
                                InputADO inputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode(tREATMENT_CODE, printTypeCode, (currentModule != null) ? currentModule.RoomId : 0);
                                Mps000420PDO data = new Mps000420PDO(item, list, serviceReqPrintRaw);
                                WaitingManager.Hide();
                                if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
                                {
                                    printerName = GlobalVariables.dicPrinter[printTypeCode];
                                }
                                PrintData printData = null;
                                printData = ((ConfigApplications.CheDoInChoCacChucNangTrongPhanMem != 2) ? new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.Show, printerName) : new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName));
                                printData.EmrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode(serviceReqPrintRaw.TREATMENT_CODE, printTypeCode, currentModule.RoomId);
                                result = MpsPrinter.Run(printData);
                            }
                        }
                        else
                        {
                            WaitingManager.Hide();
                            XtraMessageBox.Show("Chưa có giao dịch thanh toán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        WaitingManager.Hide();
                        XtraMessageBox.Show("Chưa có giao dịch thanh toán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool InTheBenhNhan(string printTypeCode, string fileName)
        {
            bool flag = false;
            try
            {
                if (serviceReqPrintRaw == null)
                {
                    return false;
                }
                WaitingManager.Show();
                V_HIS_TREATMENT_4 v_HIS_TREATMENT_ = null;
                V_HIS_PATIENT_TYPE_ALTER hisPatientTypeAlter = null;
                HisTreatmentView4Filter hisTreatmentView4Filter = new HisTreatmentView4Filter();
                hisTreatmentView4Filter.ID = serviceReqPrintRaw.TREATMENT_ID;
                List<V_HIS_TREATMENT_4> list = new BackendAdapter(new CommonParam()).Get<List<V_HIS_TREATMENT_4>>("api/HisTreatment/GetView4", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentView4Filter, null);
                v_HIS_TREATMENT_ = ((list != null) ? list.FirstOrDefault() : null);
                long num = Inventec.Common.DateTime.Get.Now() ?? 0;
                if (v_HIS_TREATMENT_ != null && num < v_HIS_TREATMENT_.IN_TIME)
                {
                    num = v_HIS_TREATMENT_.IN_TIME;
                }
                PrintGlobalStore.LoadCurrentPatientTypeAlter(serviceReqPrintRaw.TREATMENT_ID, num, ref hisPatientTypeAlter);
                CommonParam commonParam = new CommonParam();
                HisPatientViewFilter hisPatientViewFilter = new HisPatientViewFilter();
                hisPatientViewFilter.ID = serviceReqPrintRaw.TDL_PATIENT_ID;
                V_HIS_PATIENT v_HIS_PATIENT = new BackendAdapter(commonParam).Get<List<V_HIS_PATIENT>>("api/HisPatient/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientViewFilter, commonParam).FirstOrDefault();
                WaitingManager.Hide();
                V_HIS_DEPARTMENT_TRAN departmentTran = new V_HIS_DEPARTMENT_TRAN();
                HisDepartmentTranViewFilter hisDepartmentTranViewFilter = new HisDepartmentTranViewFilter();
                hisDepartmentTranViewFilter.TREATMENT_ID = v_HIS_TREATMENT_.ID;
                hisDepartmentTranViewFilter.ORDER_DIRECTION = "DESC";
                hisDepartmentTranViewFilter.ORDER_FIELD = "DEPARTMENT_IN_TIME";
                List<V_HIS_DEPARTMENT_TRAN> list2 = new BackendAdapter(commonParam).Get<List<V_HIS_DEPARTMENT_TRAN>>("api/HisDepartmentTran/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisDepartmentTranViewFilter, commonParam);
                if (list2 != null)
                {
                    List<V_HIS_DEPARTMENT_TRAN> list3 = (from o in list2
                                                         orderby o.DEPARTMENT_IN_TIME.HasValue descending, o.DEPARTMENT_IN_TIME descending, o.ID descending
                                                         select o).ToList();
                    departmentTran = list2.First();
                }
                Mps000178PDO data = new Mps000178PDO(v_HIS_PATIENT, hisPatientTypeAlter, v_HIS_TREATMENT_, departmentTran);
                string printerName = "";
                if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
                {
                    printerName = GlobalVariables.dicPrinter[printTypeCode];
                }
                InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((v_HIS_TREATMENT_ != null) ? v_HIS_TREATMENT_.TREATMENT_CODE : "", printTypeCode, (currentModule != null) ? currentModule.RoomId : 0);
                WaitingManager.Hide();
                if (GlobalVariables.CheDoInChoCacChucNangTrongPhanMem == 2)
                {
                    PrintData printData = new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName);
                    printData.EmrInputADO = emrInputADO;
                    flag = MpsPrinter.Run(printData);
                }
                else
                {
                    PrintData printData2 = new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.Show, printerName);
                    printData2.EmrInputADO = emrInputADO;
                    flag = MpsPrinter.Run(printData2);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
                flag = false;
            }
            return flag;
        }

        private bool InHenKhamLai(string printTypeCode, string fileName)
        {
            //IL_004a: Unknown result type (might be due to invalid IL or missing references)
            //IL_0050: Expected O, but got Unknown
            bool flag = false;
            try
            {
                if (serviceReqPrintRaw == null)
                {
                    return false;
                }
                WaitingManager.Show();
                HIS_MEDI_RECORD val = new HIS_MEDI_RECORD();
                CommonParam commonParam = new CommonParam();
                HisPatientViewFilter hisPatientViewFilter = new HisPatientViewFilter();
                hisPatientViewFilter.ID = serviceReqPrintRaw.TDL_PATIENT_ID;
                V_HIS_PATIENT data = new BackendAdapter(commonParam).Get<List<V_HIS_PATIENT>>("api/HisPatient/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientViewFilter, commonParam).FirstOrDefault();
                HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
                hisTreatmentFilter.ID = serviceReqPrintRaw.TREATMENT_ID;
                HIS_TREATMENT HisTreatment = new BackendAdapter(new CommonParam()).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, new CommonParam()).FirstOrDefault();
                HIS_SERVICE_REQ ServiceReq = new HIS_SERVICE_REQ();
                DataObjectMapper.Map<HIS_SERVICE_REQ>(ServiceReq, serviceReqPrintRaw);
                MPS.Processor.Mps000010.PDO.PatientADO patient = new MPS.Processor.Mps000010.PDO.PatientADO(data);
                Mps000010ADO mps000010ADO = new Mps000010ADO();
                if (HisTreatment.DEATH_CAUSE_ID.HasValue)
                {
                    HIS_DEATH_CAUSE hIS_DEATH_CAUSE = BackendDataWorker.Get<HIS_DEATH_CAUSE>().FirstOrDefault((HIS_DEATH_CAUSE o) => o.ID == HisTreatment.DEATH_CAUSE_ID.Value);
                    if (hIS_DEATH_CAUSE != null)
                    {
                        mps000010ADO.DEATH_CAUSE_CODE = hIS_DEATH_CAUSE.DEATH_CAUSE_CODE;
                        mps000010ADO.DEATH_CAUSE_NAME = hIS_DEATH_CAUSE.DEATH_CAUSE_NAME;
                    }
                }
                if (HisTreatment.DEATH_WITHIN_ID.HasValue)
                {
                    HIS_DEATH_WITHIN hIS_DEATH_WITHIN = BackendDataWorker.Get<HIS_DEATH_WITHIN>().FirstOrDefault((HIS_DEATH_WITHIN o) => o.ID == HisTreatment.DEATH_WITHIN_ID.Value);
                    if (hIS_DEATH_WITHIN != null)
                    {
                        mps000010ADO.DEATH_WITHIN_CODE = hIS_DEATH_WITHIN.DEATH_WITHIN_CODE;
                        mps000010ADO.DEATH_WITHIN_NAME = hIS_DEATH_WITHIN.DEATH_WITHIN_NAME;
                    }
                }
                if (HisTreatment.TRAN_PATI_FORM_ID.HasValue)
                {
                    HIS_TRAN_PATI_FORM hIS_TRAN_PATI_FORM = BackendDataWorker.Get<HIS_TRAN_PATI_FORM>().FirstOrDefault((HIS_TRAN_PATI_FORM o) => o.ID == HisTreatment.TRAN_PATI_FORM_ID.Value);
                    if (hIS_TRAN_PATI_FORM != null)
                    {
                        mps000010ADO.TRAN_PATI_FORM_CODE = hIS_TRAN_PATI_FORM.TRAN_PATI_FORM_CODE;
                        mps000010ADO.TRAN_PATI_FORM_NAME = hIS_TRAN_PATI_FORM.TRAN_PATI_FORM_NAME;
                    }
                }
                if (HisTreatment.TREATMENT_RESULT_ID.HasValue)
                {
                    HIS_TREATMENT_RESULT hIS_TREATMENT_RESULT = BackendDataWorker.Get<HIS_TREATMENT_RESULT>().FirstOrDefault((HIS_TREATMENT_RESULT o) => o.ID == HisTreatment.TREATMENT_RESULT_ID.Value);
                    if (hIS_TREATMENT_RESULT != null)
                    {
                        mps000010ADO.TREATMENT_RESULT_CODE = hIS_TREATMENT_RESULT.TREATMENT_RESULT_CODE;
                        mps000010ADO.TREATMENT_RESULT_NAME = hIS_TREATMENT_RESULT.TREATMENT_RESULT_NAME;
                    }
                }
                if (HisTreatment.TRAN_PATI_REASON_ID.HasValue)
                {
                    HIS_TRAN_PATI_REASON hIS_TRAN_PATI_REASON = BackendDataWorker.Get<HIS_TRAN_PATI_REASON>().FirstOrDefault((HIS_TRAN_PATI_REASON o) => o.ID == HisTreatment.TRAN_PATI_REASON_ID.Value);
                    if (hIS_TRAN_PATI_REASON != null)
                    {
                        mps000010ADO.TRAN_PATI_REASON_CODE = hIS_TRAN_PATI_REASON.TRAN_PATI_REASON_CODE;
                        mps000010ADO.TRAN_PATI_REASON_NAME = hIS_TRAN_PATI_REASON.TRAN_PATI_REASON_NAME;
                    }
                }
                if (HisTreatment != null && HisTreatment.MEDI_RECORD_ID.HasValue)
                {
                    HisMediRecordFilter hisMediRecordFilter = new HisMediRecordFilter();
                    hisMediRecordFilter.ID = HisTreatment.MEDI_RECORD_ID.Value;
                    List<HIS_MEDI_RECORD> list = new BackendAdapter(new CommonParam()).Get<List<HIS_MEDI_RECORD>>("api/HisMediRecord/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisMediRecordFilter, null);
                    val = ((list != null && list.Count > 0) ? list.FirstOrDefault() : null);
                }
                if (val != null)
                {
                    mps000010ADO.MEDI_RECORD_STORE_CODE = val.STORE_CODE;
                }
                if (HisTreatment.END_ROOM_ID.HasValue)
                {
                    V_HIS_ROOM v_HIS_ROOM = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == HisTreatment.END_ROOM_ID.Value);
                    if (v_HIS_ROOM != null)
                    {
                        mps000010ADO.END_DEPARTMENT_CODE = v_HIS_ROOM.DEPARTMENT_CODE;
                        mps000010ADO.END_DEPARTMENT_NAME = v_HIS_ROOM.DEPARTMENT_NAME;
                        mps000010ADO.END_ROOM_CODE = v_HIS_ROOM.ROOM_CODE;
                        mps000010ADO.END_ROOM_NAME = v_HIS_ROOM.ROOM_NAME;
                    }
                }
                if (HisTreatment.FEE_LOCK_ROOM_ID.HasValue)
                {
                    V_HIS_ROOM v_HIS_ROOM2 = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == HisTreatment.FEE_LOCK_ROOM_ID.Value);
                    if (v_HIS_ROOM2 != null)
                    {
                        mps000010ADO.FEE_LOCK_DEPARTMENT_CODE = v_HIS_ROOM2.DEPARTMENT_CODE;
                        mps000010ADO.FEE_LOCK_DEPARTMENT_NAME = v_HIS_ROOM2.DEPARTMENT_NAME;
                        mps000010ADO.FEE_LOCK_ROOM_CODE = v_HIS_ROOM2.ROOM_CODE;
                        mps000010ADO.FEE_LOCK_ROOM_NAME = v_HIS_ROOM2.ROOM_NAME;
                    }
                }
                if (HisTreatment.IN_ROOM_ID.HasValue)
                {
                    V_HIS_ROOM v_HIS_ROOM3 = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == HisTreatment.IN_ROOM_ID.Value);
                    if (v_HIS_ROOM3 != null)
                    {
                        mps000010ADO.IN_DEPARTMENT_CODE = v_HIS_ROOM3.DEPARTMENT_CODE;
                        mps000010ADO.IN_DEPARTMENT_NAME = v_HIS_ROOM3.DEPARTMENT_NAME;
                        mps000010ADO.IN_ROOM_CODE = v_HIS_ROOM3.ROOM_CODE;
                        mps000010ADO.IN_ROOM_NAME = v_HIS_ROOM3.ROOM_NAME;
                    }
                }
                if (ServiceReq.APPOINTMENT_EXAM_ROOM_ID.HasValue)
                {
                    mps000010ADO.APPOINTMENT_EXAM_ROOM_IDS = ServiceReq.APPOINTMENT_EXAM_ROOM_ID.ToString();
                    HIS_EXECUTE_ROOM hIS_EXECUTE_ROOM = BackendDataWorker.Get<HIS_EXECUTE_ROOM>().FirstOrDefault((HIS_EXECUTE_ROOM o) => o.ROOM_ID == ServiceReq.APPOINTMENT_EXAM_ROOM_ID);
                    if (hIS_EXECUTE_ROOM != null)
                    {
                        mps000010ADO.APPOINTMENT_EXAM_ROOM_NAMES = hIS_EXECUTE_ROOM.EXECUTE_ROOM_NAME;
                        mps000010ADO.APPOINTMENT_EXAM_ROOM_CODE_NAMES = hIS_EXECUTE_ROOM.EXECUTE_ROOM_CODE + " - " + hIS_EXECUTE_ROOM.EXECUTE_ROOM_NAME;
                    }
                }
                if (ServiceReq.APPOINTMENT_EXAM_SERVICE_ID.HasValue)
                {
                    HIS_SERVICE hIS_SERVICE = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault((HIS_SERVICE o) => o.ID == ServiceReq.APPOINTMENT_EXAM_SERVICE_ID);
                    if (hIS_SERVICE != null)
                    {
                        mps000010ADO.APPOINTMENT_SERVICE_CODES = hIS_SERVICE.SERVICE_CODE;
                        mps000010ADO.APPOINTMENT_SERVICE_NAMES = hIS_SERVICE.SERVICE_NAME;
                    }
                }
                HisAppointmentServViewFilter hisAppointmentServViewFilter = new HisAppointmentServViewFilter();
                hisAppointmentServViewFilter.TREATMENT_ID = serviceReqPrintRaw.TREATMENT_ID;
                List<V_HIS_APPOINTMENT_SERV> appointmentServs = new BackendAdapter(new CommonParam()).Get<List<V_HIS_APPOINTMENT_SERV>>("api/HisAppointmentServ/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisAppointmentServViewFilter, new Action(SessionManager.ActionLostToken), null);
                List<HIS_APPOINTMENT_PERIOD> listAppointmentPeriod = BackendDataWorker.Get<HIS_APPOINTMENT_PERIOD>();
                V_HIS_PATIENT_TYPE_ALTER hisPatientTypeAlter = null;
                LoadCurrentPatientTypeAlter(serviceReqPrintRaw.TREATMENT_ID, ref hisPatientTypeAlter);
                WaitingManager.Hide();
                Mps000010PDO data2 = new Mps000010PDO(patient, hisPatientTypeAlter, HisTreatment, mps000010ADO, appointmentServs, listAppointmentPeriod, ServiceReq);
                string printerName = "";
                if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
                {
                    printerName = GlobalVariables.dicPrinter[printTypeCode];
                }
                InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((HisTreatment != null) ? HisTreatment.TREATMENT_CODE : "", printTypeCode, (currentModule != null) ? currentModule.RoomId : 0);
                WaitingManager.Hide();
                if (GlobalVariables.CheDoInChoCacChucNangTrongPhanMem == 2)
                {
                    PrintData printData = new PrintData(printTypeCode, fileName, data2, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName);
                    printData.EmrInputADO = emrInputADO;
                    flag = MpsPrinter.Run(printData);
                }
                else
                {
                    PrintData printData2 = new PrintData(printTypeCode, fileName, data2, MPS.ProcessorBase.PrintConfig.PreviewType.Show, printerName);
                    printData2.EmrInputADO = emrInputADO;
                    flag = MpsPrinter.Run(printData2);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
                flag = false;
            }
            return flag;
        }

        public void LoadCurrentPatientTypeAlter(long treatmentId, ref V_HIS_PATIENT_TYPE_ALTER hisPatientTypeAlter)
        {
            try
            {
                CommonParam commonParam = new CommonParam();
                hisPatientTypeAlter = new BackendAdapter(commonParam).Get<V_HIS_PATIENT_TYPE_ALTER>("api/HisPatientTypeAlter/GetViewLastByTreatmentId", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, treatmentId, commonParam);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void PrintMps190001(string printTypeCode, string fileName, ref bool result)
        {
            try
            {
                if (listServiceReq == null || listServiceReq.Count <= 0)
                {
                    return;
                }
                Mapper.CreateMap<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO, MPS.Processor.Mps190001.PDO.ServiceReqADO>();
                List<MPS.Processor.Mps190001.PDO.ServiceReqADO> list = Mapper.Map<List<MPS.Processor.Mps190001.PDO.ServiceReqADO>>(listServiceReq);
                HisSereServViewFilter hisSereServViewFilter = new HisSereServViewFilter();
                hisSereServViewFilter.SERVICE_REQ_IDs = list.Select((MPS.Processor.Mps190001.PDO.ServiceReqADO o) => o.ID).Distinct().ToList();
                List<V_HIS_SERE_SERV> source = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERE_SERV>>("api/HisSereServ/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServViewFilter, new CommonParam());
                HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
                hisTreatmentFilter.IDs = list.Select((MPS.Processor.Mps190001.PDO.ServiceReqADO o) => o.TREATMENT_ID).Distinct().ToList();
                List<HIS_TREATMENT> source2 = new BackendAdapter(new CommonParam()).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, new CommonParam());
                foreach (MPS.Processor.Mps190001.PDO.ServiceReqADO item in list)
                {
                    Func<V_HIS_SERE_SERV, bool> predicate = (V_HIS_SERE_SERV o) => o.SERVICE_REQ_ID == item.ID;
                    List<V_HIS_SERE_SERV> list2 = (from o in source.Where(predicate)
                                                   orderby o.TDL_SERVICE_NAME
                                                   select o).ToList();
                    if (list2 != null && list2.Count() > 0)
                    {
                        list2 = list2.OrderBy((V_HIS_SERE_SERV o) => o.TDL_SERVICE_NAME).ToList();
                        item.LIST_SERVICE_NAME = string.Join("; ", list2.Select((V_HIS_SERE_SERV o) => o.TDL_SERVICE_NAME).Distinct().ToList());
                    }
                    HIS_TREATMENT hIS_TREATMENT = source2.FirstOrDefault((HIS_TREATMENT o) => o.ID == item.TREATMENT_ID);
                    if (hIS_TREATMENT != null)
                    {
                        item.IN_TIME = hIS_TREATMENT.IN_TIME;
                    }
                }
                list = list.OrderBy((MPS.Processor.Mps190001.PDO.ServiceReqADO o) => o.LIST_SERVICE_NAME).ThenBy((MPS.Processor.Mps190001.PDO.ServiceReqADO p) => p.INTRUCTION_TIME).ThenBy((MPS.Processor.Mps190001.PDO.ServiceReqADO p) => p.IN_TIME)
                    .ToList();
                Mps190001PDO data = new Mps190001PDO(list);
                RunPrint(printTypeCode, fileName, data, new DelegateEventLog(EventLogPrint), result, currentModule.RoomId);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void InPhieuYeuCauChiDinhTongHop(string printTypeCode)
        {
            try
            {
                if (listServiceReq != null && listServiceReq.Count > 0)
                {
                    ThreadChiDinhDichVuADO threadChiDinhDichVuADO = new ThreadChiDinhDichVuADO(listServiceReq.First());
                    CreateThreadLoadDataForChiDinhTongHop(threadChiDinhDichVuADO);
                    HisServiceReqViewFilter hisServiceReqViewFilter = new HisServiceReqViewFilter();
                    hisServiceReqViewFilter.IDs = listServiceReq.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.ID).ToList();
                    hisServiceReqViewFilter.IS_ACTIVE = 1;
                    List<V_HIS_SERVICE_REQ> serviceReqs = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERVICE_REQ>>("api/HisServiceReq/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqViewFilter, new Action(SessionManager.ActionLostToken), null);
                    HisServiceReqListResultSDO hisServiceReqListResultSDO = new HisServiceReqListResultSDO();
                    hisServiceReqListResultSDO.SereServs = threadChiDinhDichVuADO.listVHisSereServ;
                    hisServiceReqListResultSDO.ServiceReqs = serviceReqs;
                    List<V_HIS_BED_LOG> bedLogs = new List<V_HIS_BED_LOG>();
                    HisTreatmentWithPatientTypeInfoSDO hisTreatmentWithPatientTypeInfoSDO = new HisTreatmentWithPatientTypeInfoSDO();
                    DataObjectMapper.Map<HisTreatmentWithPatientTypeInfoSDO>(hisTreatmentWithPatientTypeInfoSDO, threadChiDinhDichVuADO.hisTreatment);
                    if (threadChiDinhDichVuADO.vHisPatientTypeAlter != null)
                    {
                        hisTreatmentWithPatientTypeInfoSDO.PATIENT_TYPE_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.PATIENT_TYPE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_FROM_TIME = threadChiDinhDichVuADO.vHisPatientTypeAlter.HEIN_CARD_FROM_TIME ?? 0;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_NUMBER = threadChiDinhDichVuADO.vHisPatientTypeAlter.HEIN_CARD_NUMBER;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_TO_TIME = threadChiDinhDichVuADO.vHisPatientTypeAlter.HEIN_CARD_TO_TIME ?? 0;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_MEDI_ORG_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.HEIN_MEDI_ORG_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.LEVEL_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.LEVEL_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.RIGHT_ROUTE_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.RIGHT_ROUTE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.RIGHT_ROUTE_TYPE_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.RIGHT_ROUTE_TYPE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.TREATMENT_TYPE_CODE = threadChiDinhDichVuADO.vHisPatientTypeAlter.TREATMENT_TYPE_CODE;
                    }
                    PrintServiceReqProcessor printServiceReqProcessor = new PrintServiceReqProcessor(hisServiceReqListResultSDO, hisTreatmentWithPatientTypeInfoSDO, bedLogs, (currentModule != null) ? currentModule.RoomId : 0);
                    printServiceReqProcessor.Print(printTypeCode, false);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void InDonThuocTongHop(string printTypeCode)
        {
            try
            {
                bool isNotShowTaken = lstConfig.Exists((ConfigADO o) => o.IsChecked && o.ID == 1);
                if (listServiceReq != null && listServiceReq.Count > 0)
                {
                    List<OutPatientPresResultSDO> list = new List<OutPatientPresResultSDO>();
                    OutPatientPresResultSDO outPatientPresResultSDO = new OutPatientPresResultSDO();
                    Mapper.CreateMap<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO, HIS_SERVICE_REQ>();
                    List<HIS_SERVICE_REQ> serviceReqs = Mapper.Map<List<HIS_SERVICE_REQ>>(listServiceReq);
                    outPatientPresResultSDO.ServiceReqs = serviceReqs;
                    CommonParam commonParam = new CommonParam();
                    HisServiceReqMetyFilter hisServiceReqMetyFilter = new HisServiceReqMetyFilter();
                    hisServiceReqMetyFilter.SERVICE_REQ_IDs = listServiceReq.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.ID).ToList();
                    List<HIS_SERVICE_REQ_METY> serviceReqMeties = new BackendAdapter(commonParam).Get<List<HIS_SERVICE_REQ_METY>>("api/HisServiceReqMety/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqMetyFilter, commonParam);
                    outPatientPresResultSDO.ServiceReqMeties = serviceReqMeties;
                    HisServiceReqMatyFilter hisServiceReqMatyFilter = new HisServiceReqMatyFilter();
                    hisServiceReqMatyFilter.SERVICE_REQ_IDs = listServiceReq.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.ID).ToList();
                    List<HIS_SERVICE_REQ_MATY> serviceReqMaties = new BackendAdapter(commonParam).Get<List<HIS_SERVICE_REQ_MATY>>("api/HisServiceReqMaty/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqMatyFilter, commonParam);
                    outPatientPresResultSDO.ServiceReqMaties = serviceReqMaties;
                    HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
                    hisExpMestFilter.SERVICE_REQ_IDs = listServiceReq.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.ID).ToList();
                    List<HIS_EXP_MEST> expMests = new BackendAdapter(commonParam).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, commonParam);
                    outPatientPresResultSDO.ExpMests = expMests;
                    HisExpMestMedicineFilter hisExpMestMedicineFilter = new HisExpMestMedicineFilter();
                    hisExpMestMedicineFilter.TDL_SERVICE_REQ_IDs = listServiceReq.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.ID).ToList();
                    List<HIS_EXP_MEST_MEDICINE> medicines = new BackendAdapter(commonParam).Get<List<HIS_EXP_MEST_MEDICINE>>("api/HisExpMestMedicine/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestMedicineFilter, commonParam);
                    outPatientPresResultSDO.Medicines = medicines;
                    HisExpMestMaterialFilter hisExpMestMaterialFilter = new HisExpMestMaterialFilter();
                    hisExpMestMaterialFilter.TDL_SERVICE_REQ_IDs = listServiceReq.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.ID).ToList();
                    List<HIS_EXP_MEST_MATERIAL> materials = new BackendAdapter(commonParam).Get<List<HIS_EXP_MEST_MATERIAL>>("api/HisExpMestMaterial/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestMaterialFilter, commonParam);
                    outPatientPresResultSDO.Materials = materials;
                    list.Add(outPatientPresResultSDO);
                    PrintPrescriptionProcessor printPrescriptionProcessor = new PrintPrescriptionProcessor(list, isNotShowTaken, currentModule);
                    printPrescriptionProcessor.Print(printTypeCode, false);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void InSuatAn(string printTypeCode, string fileName, List<ListMedicineADO> lstSereServSelected, ref bool result)
        {
            try
            {
                if (serviceReqPrintRaw == null)
                {
                    return;
                }
                WaitingManager.Show();
                InputADO inputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode(serviceReqPrintRaw.TREATMENT_CODE, printTypeCode, currentModule.RoomId);
                List<V_HIS_SERVICE_REQ> list = new List<V_HIS_SERVICE_REQ>();
                list.Add(serviceReqPrintRaw);
                HisSereServRationFilter hisSereServRationFilter = new HisSereServRationFilter();
                hisSereServRationFilter.SERVICE_REQ_ID = serviceReqPrintRaw.ID;
                hisSereServRationFilter.ORDER_DIRECTION = "DESC";
                hisSereServRationFilter.ORDER_FIELD = "ID";
                CommonParam commonParam = new CommonParam();
                List<HIS_SERE_SERV_RATION> sereServRations = new BackendAdapter(commonParam).Get<List<HIS_SERE_SERV_RATION>>("api/HisSereServRation/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServRationFilter, new Action(SessionManager.ActionLostToken), commonParam);
                HisSereServViewFilter hisSereServViewFilter = new HisSereServViewFilter();
                hisSereServViewFilter.SERVICE_REQ_ID = serviceReqPrintRaw.ID;
                hisSereServViewFilter.ORDER_DIRECTION = "DESC";
                hisSereServViewFilter.ORDER_FIELD = "ID";
                commonParam = new CommonParam();
                List<V_HIS_SERE_SERV> list2 = new BackendAdapter(commonParam).Get<List<V_HIS_SERE_SERV>>("api/HisSereServ/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServViewFilter, new Action(SessionManager.ActionLostToken), commonParam);
                if (list2 != null && list2.Count > 0 && lstSereServSelected != null && lstSereServSelected.Count > 0)
                {
                    list2 = list2.Where((V_HIS_SERE_SERV o) => lstSereServSelected.Select((ListMedicineADO s) => s.ID).Contains(o.ID)).ToList();
                }
                List<HIS_SERE_SERV_EXT> sereServExts = new List<HIS_SERE_SERV_EXT>();
                if (list2 != null && list2.Count > 0)
                {
                    HisSereServExtFilter hisSereServExtFilter = new HisSereServExtFilter();
                    hisSereServExtFilter.SERE_SERV_IDs = list2.Select((V_HIS_SERE_SERV s) => s.ID).ToList();
                    hisSereServExtFilter.ORDER_DIRECTION = "DESC";
                    hisSereServExtFilter.ORDER_FIELD = "ID";
                    commonParam = new CommonParam();
                    sereServExts = new BackendAdapter(commonParam).Get<List<HIS_SERE_SERV_EXT>>("api/HisSereServExt/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServExtFilter, new Action(SessionManager.ActionLostToken), commonParam);
                }
                Mps000275PDO data = new Mps000275PDO(list, list2, sereServRations, sereServExts, BackendDataWorker.Get<HIS_PATIENT_TYPE>());
                WaitingManager.Hide();
                PrintData(printTypeCode, fileName, data, ref result);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void InGiayDeNghiDoiTraDichVu(string printTypeCode, string fileName, ref bool result)
        {
            try
            {
                if (printChangeServiceId <= 0)
                {
                    return;
                }
                V_HIS_SERVICE_REQ serviceReqForPrint = GetServiceReqForPrint(printChangeServiceId);
                WaitingManager.Show();
                List<HIS_SERE_SERV> list = new List<HIS_SERE_SERV>();
                V_HIS_TREATMENT v_HIS_TREATMENT = new V_HIS_TREATMENT();
                HisSereServFilter hisSereServFilter = new HisSereServFilter();
                hisSereServFilter.SERVICE_REQ_ID = serviceReqForPrint.ID;
                hisSereServFilter.ORDER_DIRECTION = "DESC";
                List<HIS_SERE_SERV> list2 = new BackendAdapter(new CommonParam()).Get<List<HIS_SERE_SERV>>("api/HisSereServ/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServFilter, new Action(SessionManager.ActionLostToken), null);
                if (list2 != null && list2.Count > 0)
                {
                    list = list2.Where((HIS_SERE_SERV o) => o.IS_ACCEPTING_NO_EXECUTE == 1).ToList();
                }
                HisTreatmentViewFilter hisTreatmentViewFilter = new HisTreatmentViewFilter();
                hisTreatmentViewFilter.ID = serviceReqForPrint.TREATMENT_ID;
                v_HIS_TREATMENT = new BackendAdapter(new CommonParam()).Get<List<V_HIS_TREATMENT>>("api/HisTreatment/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentViewFilter, new Action(SessionManager.ActionLostToken), null).FirstOrDefault();
                V_HIS_PATIENT_TYPE_ALTER hisPatientTypeAlter = null;
                LoadCurrentPatientTypeAlter(serviceReqForPrint.TREATMENT_ID, ref hisPatientTypeAlter);
                Mps000433PDO data = new Mps000433PDO(serviceReqForPrint, list2, v_HIS_TREATMENT, hisPatientTypeAlter);
                WaitingManager.Hide();
                string printerName = "";
                if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
                {
                    printerName = GlobalVariables.dicPrinter[printTypeCode];
                }
                InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode(treatmentCode, printTypeCode, (currentModule != null) ? currentModule.RoomId : 0);
                if (GlobalVariables.CheDoInChoCacChucNangTrongPhanMem == 2)
                {
                    result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName)
                    {
                        EmrInputADO = emrInputADO
                    });
                }
                else
                {
                    result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.Show, printerName)
                    {
                        EmrInputADO = emrInputADO
                    });
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintServiceReqBySelectedService()
        {
            //IL_051e: Unknown result type (might be due to invalid IL or missing references)
            //IL_0525: Expected O, but got Unknown
            //IL_054d: Unknown result type (might be due to invalid IL or missing references)
            //IL_0557: Expected O, but got Unknown
            try
            {
                if (currentServiceReq == null)
                {
                    return;
                }
                lstSereServSelected = new List<ListMedicineADO>();
                serviceReqPrintRaw = GetServiceReqForPrint(currentServiceReq.ID);
                int[] selectedRows = grdViewSereServServiceReq.GetSelectedRows();
                int[] array = selectedRows;
                foreach (int rowHandle in array)
                {
                    ListMedicineADO listMedicineADO = (ListMedicineADO)grdViewSereServServiceReq.GetRow(rowHandle);
                    if (listMedicineADO != null)
                    {
                        lstSereServSelected.Add(listMedicineADO);
                    }
                }
                if (lstSereServSelected == null || lstSereServSelected.Count <= 0)
                {
                    return;
                }
                if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID != 13 && serviceReqPrintRaw.SERVICE_REQ_TYPE_ID != 17)
                {
                    ThreadChiDinhDichVuADO threadChiDinhDichVuADO = new ThreadChiDinhDichVuADO(currentServiceReq);
                    CreateThreadLoadDataForService(threadChiDinhDichVuADO);
                    V_HIS_PATIENT_TYPE_ALTER hisPatientTypeAlter = null;
                    LoadCurrentPatientTypeAlter(serviceReqPrintRaw.TREATMENT_ID, ref hisPatientTypeAlter);
                    HisServiceReqListResultSDO hisServiceReqListResultSDO = new HisServiceReqListResultSDO();
                    hisServiceReqListResultSDO.SereServs = threadChiDinhDichVuADO.listVHisSereServ.Where((V_HIS_SERE_SERV o) => lstSereServSelected.Select((ListMedicineADO s) => s.ID).Contains(o.ID)).ToList();
                    hisServiceReqListResultSDO.ServiceReqs = new List<V_HIS_SERVICE_REQ> { serviceReqPrintRaw };
                    hisServiceReqListResultSDO.SereServBills = threadChiDinhDichVuADO.ListSereServBill;
                    hisServiceReqListResultSDO.SereServDeposits = threadChiDinhDichVuADO.ListSereServDeposit;
                    List<V_HIS_BED_LOG> bedLogs = new List<V_HIS_BED_LOG>();
                    HisBedLogViewFilter hisBedLogViewFilter = new HisBedLogViewFilter();
                    hisBedLogViewFilter.TREATMENT_ID = serviceReqPrintRaw.TREATMENT_ID;
                    List<V_HIS_BED_LOG> list = new BackendAdapter(param).Get<List<V_HIS_BED_LOG>>("api/HisBedLog/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisBedLogViewFilter, param);
                    if (list != null)
                    {
                        bedLogs = list;
                    }
                    HisTreatmentWithPatientTypeInfoSDO hisTreatmentWithPatientTypeInfoSDO = new HisTreatmentWithPatientTypeInfoSDO();
                    DataObjectMapper.Map<HisTreatmentWithPatientTypeInfoSDO>(hisTreatmentWithPatientTypeInfoSDO, threadChiDinhDichVuADO.hisTreatment);
                    if (hisPatientTypeAlter != null)
                    {
                        hisTreatmentWithPatientTypeInfoSDO.PATIENT_TYPE_CODE = hisPatientTypeAlter.PATIENT_TYPE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_FROM_TIME = hisPatientTypeAlter.HEIN_CARD_FROM_TIME ?? 0;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_NUMBER = hisPatientTypeAlter.HEIN_CARD_NUMBER;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_TO_TIME = hisPatientTypeAlter.HEIN_CARD_TO_TIME ?? 0;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_MEDI_ORG_CODE = hisPatientTypeAlter.HEIN_MEDI_ORG_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.LEVEL_CODE = hisPatientTypeAlter.LEVEL_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.RIGHT_ROUTE_CODE = hisPatientTypeAlter.RIGHT_ROUTE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.RIGHT_ROUTE_TYPE_CODE = hisPatientTypeAlter.RIGHT_ROUTE_TYPE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.TREATMENT_TYPE_CODE = hisPatientTypeAlter.TREATMENT_TYPE_CODE;
                        hisTreatmentWithPatientTypeInfoSDO.HEIN_CARD_ADDRESS = hisPatientTypeAlter.ADDRESS;
                    }
                    PrintServiceReqProcessor printServiceReqProcessor = new PrintServiceReqProcessor(hisServiceReqListResultSDO, hisTreatmentWithPatientTypeInfoSDO, bedLogs, (currentModule != null) ? currentModule.RoomId : 0);
                    if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 1)
                    {
                        printServiceReqProcessor.Print("Mps000001", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 9)
                    {
                        printServiceReqProcessor.Print("Mps000030", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 2)
                    {
                        printServiceReqProcessor.Print("Mps000026", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 8)
                    {
                        printServiceReqProcessor.Print("Mps000029", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 5)
                    {
                        printServiceReqProcessor.Print("Mps000038", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 4)
                    {
                        printServiceReqProcessor.Print("Mps000031", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 10)
                    {
                        printServiceReqProcessor.Print("Mps000036", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 3)
                    {
                        printServiceReqProcessor.Print("Mps000028", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 12)
                    {
                        printServiceReqProcessor.Print("Mps000053", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 11)
                    {
                        printServiceReqProcessor.Print("Mps000040", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 7)
                    {
                        printServiceReqProcessor.Print("Mps000042", false);
                    }
                    else if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 13)
                    {
                        printServiceReqProcessor.Print("Mps000167");
                    }
                }
                else
                {
                    RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder);
                    if (serviceReqPrintRaw.SERVICE_REQ_TYPE_ID == 17)
                    {
                        val.RunPrintTemplate("Mps000275", new DelegateRunPrinter(DelegateRunPrinterWithSelectedService));
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void PrintKetQuaHeThongBenhAnhDienTu()
        {
            try
            {
                WaitingManager.Show();
                if (gridViewServiceReq.FocusedRowHandle < 0)
                {
                    return;
                }
                CommonParam commonParam = new CommonParam();
                CommonParam commonParam2 = new CommonParam();
                bool flag = false;
                HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO data = (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO)gridViewServiceReq.GetFocusedRow();
                if (data == null)
                {
                    LogSystem.Info("Data thuc hien huy yeu cau dich vu null: " + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data));
                    return;
                }
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list = new List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>();
                List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> list2 = (List<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO>)gridControlServiceReq.DataSource;
                if (list2.Count > 0)
                {
                    foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in list2)
                    {
                        if (item.isCheck)
                        {
                            list.Add(item);
                        }
                    }
                }
                IEnumerable<HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO> enumerable = list.Where((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.ID == data.ID);
                if (list.Count == 0)
                {
                    list.Add(data);
                }
                else if (enumerable == null && enumerable.Count() == 0)
                {
                    list.Add(data);
                }
                if (list == null || list.Count <= 0)
                {
                    return;
                }
                List<EMR_DOCUMENT> list3 = new List<EMR_DOCUMENT>();
                List<EMR_DOCUMENT> list4 = new List<EMR_DOCUMENT>();
                string text = Inventec.Common.SignLibrary.Utils.GenerateTempFileWithin();
                foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item2 in list)
                {
                    EmrDocumentFilter emrDocumentFilter = new EmrDocumentFilter();
                    LogSystem.Debug("TDL_TREATMENT_CODE_______________________________________" + data.TDL_TREATMENT_CODE);
                    emrDocumentFilter.TREATMENT_CODE__EXACT = item2.TDL_TREATMENT_CODE;
                    emrDocumentFilter.DOCUMENT_TYPE_ID = 22L;
                    list4 = new BackendAdapter(commonParam).Get<List<EMR_DOCUMENT>>("api/EmrDocument/Get", HIS.Desktop.ApiConsumer.ApiConsumers.EmrConsumer, emrDocumentFilter, commonParam);
                    list4 = list4.Where(delegate(EMR_DOCUMENT o)
                    {
                        short? iS_DELETE = o.IS_DELETE;
                        return iS_DELETE.GetValueOrDefault() == 0 && iS_DELETE.HasValue && o.HIS_CODE != null;
                    }).ToList();
                    if (list4 == null || list4.Count() <= 0)
                    {
                        continue;
                    }
                    string value = "SERVICE_REQ_CODE:" + item2.SERVICE_REQ_CODE;
                    foreach (EMR_DOCUMENT item3 in list4)
                    {
                        if (item3.HIS_CODE.Contains(value))
                        {
                            list3.Add(item3);
                        }
                    }
                }
                if (list3 != null && list3.Count() > 0)
                {
                    List<EmrDocumentFileSDO> emrDocumentFile = GetEmrDocumentFile(null, list3.Select((EMR_DOCUMENT o) => o.ID).ToList(), true, true, false);
                    if (emrDocumentFile == null || emrDocumentFile.Count <= 0)
                    {
                        return;
                    }
                    List<string> joinStreams = new List<string>();
                    List<MemoryStream> list5 = new List<MemoryStream>();
                    foreach (EmrDocumentFileSDO item4 in emrDocumentFile)
                    {
                        if (item4.Extension.ToLower().Equals("pdf"))
                        {
                            string text2 = Inventec.Common.SignLibrary.Utils.GenerateTempFileWithin();
                            Inventec.Common.SignLibrary.Utils.ByteToFile(Inventec.Common.SignLibrary.Utils.StreamToByte(new MemoryStream(System.Convert.FromBase64String(item4.Base64Data))), text2);
                            joinStreams.Add(text2);
                        }
                    }
                    Stream os = File.Open(text, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    PdfConcatenate pdfConcatenate = new PdfConcatenate(os);
                    List<int> list6 = new List<int>();
                    LogSystem.Debug(LogUtil.TraceData("Đây là dữ liệu joinStreams: " + LogUtil.GetMemberName(() => joinStreams), joinStreams));
                    foreach (string item5 in joinStreams)
                    {
                        PdfReader pdfReader = null;
                        pdfReader = new PdfReader(item5);
                        list6 = new List<int>();
                        for (int num = 0; num <= pdfReader.NumberOfPages; num++)
                        {
                            list6.Add(num);
                        }
                        pdfReader.SelectPages(list6);
                        pdfConcatenate.AddPages(pdfReader);
                        pdfReader.Close();
                    }
                    try
                    {
                        pdfConcatenate.Close();
                    }
                    catch
                    {
                    }
                    foreach (string item6 in joinStreams)
                    {
                        try
                        {
                            File.Delete(item6);
                        }
                        catch
                        {
                        }
                    }
                    LogSystem.Warn("output: " + text);
                    frmPdfViewer frmPdfViewer2 = new frmPdfViewer(text);
                    frmPdfViewer2.Text = "In phiếu kết quả trên hệ thống Bệnh án điện tử";
                    frmPdfViewer2.Icon = new Icon(Path.Combine(ApplicationStoreLocation.ApplicationDirectory, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]));
                    frmPdfViewer2.WindowState = FormWindowState.Normal;
                    WaitingManager.Hide();
                    frmPdfViewer2.ShowDialog();
                }
                else if (list != null && list.Count > 0)
                {
                    WaitingManager.Hide();
                    string text3 = "Không tìm thấy phiếu kết quả trên hệ thống Bệnh án điện tử ứng với y lệnh: " + string.Join(", ", list.Select((HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO o) => o.SERVICE_REQ_CODE));
                    XtraMessageBox.Show(text3, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private List<EmrDocumentFileSDO> GetEmrDocumentFile(V_EMR_DOCUMENT document, List<long> docIds, bool? IsMerge, bool? IsShowPatientSign, bool? IsShowWatermark)
        {
            CommonParam commonParam = new CommonParam();
            EmrDocumentDownloadFileSDO emrDocumentDownloadFileSDO = new EmrDocumentDownloadFileSDO();
            EmrDocumentViewFilter emrDocumentViewFilter = new EmrDocumentViewFilter();
            if (document != null)
            {
                emrDocumentViewFilter.ID = document.ID;
            }
            else
            {
                emrDocumentViewFilter.IDs = docIds;
            }
            emrDocumentDownloadFileSDO.EmrDocumentViewFilter = emrDocumentViewFilter;
            emrDocumentDownloadFileSDO.IsMerge = IsMerge;
            emrDocumentDownloadFileSDO.IsShowPatientSign = IsShowPatientSign;
            emrDocumentDownloadFileSDO.IsShowWatermark = IsShowWatermark;
            emrDocumentDownloadFileSDO.RoomCode = ((currentRoom != null) ? currentRoom.ROOM_CODE : null);
            emrDocumentDownloadFileSDO.DepartmentCode = ((currentRoom != null) ? currentRoom.DEPARTMENT_CODE : null);
            return new BackendAdapter(commonParam).Post<List<EmrDocumentFileSDO>>("api/EmrDocument/DownloadFile", HIS.Desktop.ApiConsumer.ApiConsumers.EmrConsumer, emrDocumentDownloadFileSDO, commonParam);
        }

        internal static void InsertPage1(Stream sourceStream, List<string> fileListJoin, string desFileJoined)
        {
            List<string> joinStreams = new List<string>();
            if (fileListJoin == null || fileListJoin.Count <= 0)
            {
                return;
            }
            PdfReader pdfReader = null;
            if (sourceStream != null)
            {
                pdfReader = new PdfReader(sourceStream);
            }
            int numberOfPages = pdfReader.NumberOfPages;
            iTextSharp.text.Rectangle pageSizeWithRotation = pdfReader.GetPageSizeWithRotation(pdfReader.NumberOfPages);
            iTextSharp.text.Rectangle pageSize = new iTextSharp.text.Rectangle(pageSizeWithRotation.Left, pageSizeWithRotation.Bottom, pageSizeWithRotation.Right, pageSizeWithRotation.Bottom + pageSizeWithRotation.Height, pageSizeWithRotation.Rotation);
            if (fileListJoin != null && fileListJoin.Count > 0)
            {
                fileListJoin.Remove(fileListJoin.FirstOrDefault());
            }
            foreach (string item in fileListJoin)
            {
                int num = item.LastIndexOf(".");
                string text = item.Substring((num > 0) ? (num + 1) : num);
                if (text != "pdf")
                {
                    MemoryStream file = FileDownload.GetFile(item);
                    file.Position = 0L;
                    string text2 = Inventec.Common.SignLibrary.Utils.GenerateTempFileWithin();
                    Stream os = new FileStream(text2, FileMode.Create, FileAccess.Write);
                    iTextSharp.text.Document document = new iTextSharp.text.Document(pageSize, 0f, 0f, 0f, 0f);
                    PdfWriter instance = PdfWriter.GetInstance(document, os);
                    document.Open();
                    instance.Open();
                    iTextSharp.text.Image instance2 = iTextSharp.text.Image.GetInstance(file);
                    if (instance2.Height > instance2.Width)
                    {
                        float num2 = 0f;
                        num2 = pageSizeWithRotation.Height / instance2.Height;
                        instance2.ScalePercent(num2 * 100f);
                    }
                    else
                    {
                        float num2 = 0f;
                        num2 = pageSizeWithRotation.Width / instance2.Width;
                        instance2.ScalePercent(num2 * 100f);
                    }
                    document.Add(instance2);
                    document.Close();
                    instance.Close();
                    joinStreams.Add(text2);
                }
                else
                {
                    MemoryStream file = FileDownload.GetFile(item);
                    if (file != null && file.Length > 0)
                    {
                        file.Position = 0L;
                        string text3 = Inventec.Common.SignLibrary.Utils.GenerateTempFileWithin();
                        Inventec.Common.SignLibrary.Utils.ByteToFile(Inventec.Common.SignLibrary.Utils.StreamToByte(file), text3);
                        joinStreams.Add(text3);
                    }
                    else
                    {
                        LogSystem.Error("Loi convert va luu tam file pdf tu server fss ve may tram____item=" + item);
                    }
                }
            }
            Stream os2 = File.Open(desFileJoined, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            PdfConcatenate pdfConcatenate = new PdfConcatenate(os2);
            List<int> list = new List<int>();
            for (int i = 0; i <= pdfReader.NumberOfPages; i++)
            {
                list.Add(i);
            }
            pdfReader.SelectPages(list);
            pdfConcatenate.AddPages(pdfReader);
            LogSystem.Debug(LogUtil.TraceData("Đây là dữ liệu joinStreams: " + LogUtil.GetMemberName(() => joinStreams), joinStreams));
            foreach (string item2 in joinStreams)
            {
                PdfReader pdfReader2 = null;
                pdfReader2 = new PdfReader(item2);
                list = new List<int>();
                for (int i = 0; i <= pdfReader2.NumberOfPages; i++)
                {
                    list.Add(i);
                }
                pdfReader2.SelectPages(list);
                pdfConcatenate.AddPages(pdfReader2);
                pdfReader2.Close();
            }
            try
            {
                pdfReader.Close();
            }
            catch
            {
            }
            try
            {
                if (sourceStream != null)
                {
                    sourceStream.Close();
                }
            }
            catch
            {
            }
            try
            {
                pdfConcatenate.Close();
            }
            catch
            {
            }
            foreach (string item3 in joinStreams)
            {
                try
                {
                    File.Delete(item3);
                }
                catch
                {
                }
            }
        }

        private bool DelegateRunPrinterWithSelectedService(string printTypeCode, string fileName)
        {
            bool result = false;
            try
            {
                if (printTypeCode != null && printTypeCode == "Mps000275")
                {
                    InSuatAn(printTypeCode, fileName, lstSereServSelected, ref result);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private void LoadBieuMauPhieuThuThuatPhauThuat(string printTypeCode, string fileName, ref bool result)
        {
            try
            {
                if (sereServPrint == null)
                {
                    return;
                }
                WaitingManager.Show();
                ThreadPtttADO threadPtttADO = new ThreadPtttADO(sereServPrint);
                CreateThreadLoadDataForPttt(threadPtttADO);
                MPS.Processor.Mps000033.PDO.PatientADO patient = new MPS.Processor.Mps000033.PDO.PatientADO(threadPtttADO.patient);
                HIS_SERE_SERV_EXT SereServExt = null;
                HisSereServExtFilter hisSereServExtFilter = new HisSereServExtFilter();
                hisSereServExtFilter.SERE_SERV_ID = sereServPrint.ID;
                List<HIS_SERE_SERV_EXT> list = new BackendAdapter(new CommonParam()).Get<List<HIS_SERE_SERV_EXT>>("api/HisSereServExt/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServExtFilter, new Action(SessionManager.ActionLostToken), null);
                if (list != null && list.Count > 0)
                {
                    SereServExt = list.FirstOrDefault();
                }
                V_HIS_BED_LOG bedLog = new V_HIS_BED_LOG();
                V_HIS_BED_LOG lastBedLog = new V_HIS_BED_LOG();
                if (SereServExt != null)
                {
                    HisBedLogViewFilter hisBedLogViewFilter = new HisBedLogViewFilter();
                    hisBedLogViewFilter.TREATMENT_ID = threadPtttADO.vhisTreatment.ID;
                    List<V_HIS_BED_LOG> list2 = new BackendAdapter(param).Get<List<V_HIS_BED_LOG>>("api/HisBedLog/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisBedLogViewFilter, param);
                    if ((list2 != null) & (list2.Count > 0))
                    {
                        bedLog = (from o in list2
                                  where o.START_TIME <= SereServExt.BEGIN_TIME && (!o.FINISH_TIME.HasValue || o.FINISH_TIME.Value > SereServExt.BEGIN_TIME)
                                  orderby o.START_TIME descending
                                  select o).FirstOrDefault();
                        lastBedLog = (from o in list2
                                      where o.START_TIME <= SereServExt.BEGIN_TIME
                                      orderby o.START_TIME descending
                                      select o).FirstOrDefault();
                    }
                }
                HIS_SKIN_SURGERY_DESC skinSurgeryDesc = null;
                if (threadPtttADO.sereServPttts != null && threadPtttADO.sereServPttts.SKIN_SURGERY_DESC_ID.HasValue)
                {
                    HisSkinSurgeryDescFilter hisSkinSurgeryDescFilter = new HisSkinSurgeryDescFilter();
                    hisSkinSurgeryDescFilter.ID = threadPtttADO.sereServPttts.SKIN_SURGERY_DESC_ID;
                    List<HIS_SKIN_SURGERY_DESC> list3 = new BackendAdapter(param).Get<List<HIS_SKIN_SURGERY_DESC>>("api/HisSkinSurgeryDesc/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSkinSurgeryDescFilter, param);
                    if (list3 != null && list3.Count > 0)
                    {
                        skinSurgeryDesc = list3.FirstOrDefault();
                    }
                }
                List<V_HIS_SESE_PTTT_METHOD> sesePtttMethod = null;
                if (threadPtttADO.sereServPrint != null)
                {
                    HisSesePtttMethodViewFilter hisSesePtttMethodViewFilter = new HisSesePtttMethodViewFilter();
                    hisSesePtttMethodViewFilter.TDL_SERE_SERV_ID = threadPtttADO.sereServPrint.ID;
                    sesePtttMethod = new BackendAdapter(param).Get<List<V_HIS_SESE_PTTT_METHOD>>("api/HisSesePtttMethod/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSesePtttMethodViewFilter, param);
                }
                HisExecuteRoleCFGPrint executeRoleCFG = new HisExecuteRoleCFGPrint();
                WaitingManager.Hide();
                Mps000033PDO data = new Mps000033PDO(patient, threadPtttADO.departmentTran, threadPtttADO.serviceReq, threadPtttADO.sereServ5Print, SereServExt, threadPtttADO.sereServPttts, threadPtttADO.vhisTreatment, threadPtttADO.ekipUsers, executeRoleCFG, bedLog, lastBedLog, null, skinSurgeryDesc, threadPtttADO.sereServFile, sesePtttMethod);
                PrintData(printTypeCode, fileName, data, ref result);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                WaitingManager.Hide();
            }
        }

        private void LoadBieuMauPhieuYCInGiayCamDoan(string printTypeCode, string fileName, ref bool result)
        {
            try
            {
                if (sereServPrint != null)
                {
                    WaitingManager.Show();
                    ThreadPtttADO threadPtttADO = new ThreadPtttADO(sereServPrint);
                    CreateThreadLoadDataForPttt(threadPtttADO);
                    Mps000035PDO data = new Mps000035PDO(threadPtttADO.patient, threadPtttADO.departmentTran, threadPtttADO.serviceReq, threadPtttADO.vhisTreatment);
                    WaitingManager.Hide();
                    PrintData(printTypeCode, fileName, data, ref result);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                WaitingManager.Hide();
            }
        }

        private void LoadBieuMauCachThucPhauThuat(string printTypeCode, string fileName, ref bool result)
        {
            try
            {
                if (sereServPrint != null)
                {
                    WaitingManager.Show();
                    ThreadPtttADO threadPtttADO = new ThreadPtttADO(sereServPrint);
                    CreateThreadLoadDataForPttt(threadPtttADO);
                    Mps000097PDO data = new Mps000097PDO(threadPtttADO.patient, threadPtttADO.sereServPttts, threadPtttADO.ekipUsers, threadPtttADO.vhisTreatment);
                    WaitingManager.Hide();
                    PrintData(printTypeCode, fileName, data, ref result);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void LoadGiayChungNhanPTTT(string printTypeCode, string fileName, ref bool result)
        {
            try
            {
                if (sereServPrint != null)
                {
                    WaitingManager.Show();
                    V_HIS_SERE_SERV_1 v_HIS_SERE_SERV_ = new V_HIS_SERE_SERV_1();
                    HisSereServView1Filter hisSereServView1Filter = new HisSereServView1Filter();
                    hisSereServView1Filter.TREATMENT_ID = sereServPrint.ID;
                    List<V_HIS_SERE_SERV_1> list = new BackendAdapter(param).Get<List<V_HIS_SERE_SERV_1>>("/api/HisSereServ/GetView1", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServView1Filter, param);
                    if (list != null && list.Count > 0)
                    {
                        v_HIS_SERE_SERV_ = list.FirstOrDefault();
                    }
                    V_HIS_TREATMENT currentTreatment = new V_HIS_TREATMENT();
                    HisTreatmentViewFilter hisTreatmentViewFilter = new HisTreatmentViewFilter();
                    hisTreatmentViewFilter.ID = sereServPrint.TDL_TREATMENT_ID;
                    List<V_HIS_TREATMENT> list2 = new BackendAdapter(param).Get<List<V_HIS_TREATMENT>>("api/HisTreatment/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentViewFilter, param);
                    if (list2 != null && list2.Count > 0)
                    {
                        currentTreatment = list2.FirstOrDefault();
                    }
                    HisSereServPtttViewFilter hisSereServPtttViewFilter = new HisSereServPtttViewFilter();
                    hisSereServPtttViewFilter.SERE_SERV_ID = sereServPrint.ID;
                    V_HIS_SERE_SERV_PTTT pttt = new BackendAdapter(param).Get<List<V_HIS_SERE_SERV_PTTT>>("api/HisSereServPttt/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServPtttViewFilter, param).FirstOrDefault();
                    HIS_SERE_SERV_EXT hIS_SERE_SERV_EXT = new HIS_SERE_SERV_EXT();
                    HisSereServExtFilter hisSereServExtFilter = new HisSereServExtFilter();
                    hisSereServExtFilter.SERE_SERV_ID = sereServPrint.ID;
                    List<HIS_SERE_SERV_EXT> list3 = new BackendAdapter(new CommonParam()).Get<List<HIS_SERE_SERV_EXT>>("api/HisSereServExt/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServExtFilter, null);
                    if (list3 != null && list3.Count > 0)
                    {
                        hIS_SERE_SERV_EXT = list3.FirstOrDefault();
                    }
                    List<V_HIS_EKIP_USER> listEkipUser = new List<V_HIS_EKIP_USER>();
                    if (sereServPrint.EKIP_ID.HasValue)
                    {
                        HisEkipUserViewFilter hisEkipUserViewFilter = new HisEkipUserViewFilter();
                        hisEkipUserViewFilter.EKIP_ID = sereServPrint.EKIP_ID;
                        hisEkipUserViewFilter.ORDER_FIELD = "EXECUTE_ROLE_ID";
                        hisEkipUserViewFilter.ORDER_DIRECTION = "ASC";
                        listEkipUser = new BackendAdapter(param).Get<List<V_HIS_EKIP_USER>>("api/HisEkipUser/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisEkipUserViewFilter, param);
                    }
                    HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
                    hisServiceReqFilter.ID = sereServPrint.SERVICE_REQ_ID;
                    HIS_SERVICE_REQ hIS_SERVICE_REQ = new BackendAdapter(param).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqFilter, param).FirstOrDefault();
                    List<HIS_EXECUTE_ROLE> list4 = new List<HIS_EXECUTE_ROLE>();
                    list4 = (from o in BackendDataWorker.Get<HIS_EXECUTE_ROLE>()
                             where o.IS_ACTIVE == 1
                             select o).ToList();
                    Mps000204PDO data = new Mps000204PDO(v_HIS_SERE_SERV_, currentTreatment, pttt, listEkipUser, hIS_SERVICE_REQ, list4);
                    WaitingManager.Hide();
                    PrintData(printTypeCode, fileName, data, ref result);
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Error(ex);
            }
        }

        private void InPhieuKetQuaPHCN(string printTypeCode, string fileName, ref bool result)
        {
            try
            {
                if (sereServPrint == null)
                {
                    return;
                }
                WaitingManager.Show();
                MPS.ADO.PatientADO patientById = PrintGlobalStore.GetPatientById(sereServPrint.TDL_PATIENT_ID ?? 0);
                V_HIS_ROOM v_HIS_ROOM = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == sereServPrint.TDL_EXECUTE_ROOM_ID);
                string bedRoomName = ((v_HIS_ROOM != null) ? v_HIS_ROOM.ROOM_NAME : "");
                V_HIS_DEPARTMENT_TRAN departmentTran = PrintGlobalStore.getDepartmentTran(sereServPrint.TDL_TREATMENT_ID ?? 0);
                List<V_HIS_DEPARTMENT_TRAN> list = new List<V_HIS_DEPARTMENT_TRAN>();
                list.Add(departmentTran);
                HIS_REHA_SUM hIS_REHA_SUM = new HIS_REHA_SUM();
                DataObjectMapper.Map<HIS_REHA_SUM>(hIS_REHA_SUM, currentServiceReq);
                HisSereServRehaFilter hisSereServRehaFilter = new HisSereServRehaFilter();
                hisSereServRehaFilter.SERE_SERV_ID = sereServPrint.ID;
                List<HIS_SERE_SERV_REHA> list2 = new BackendAdapter(param).Get<List<HIS_SERE_SERV_REHA>>("api/HisSereServReha/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServRehaFilter, new Action(SessionManager.ActionLostToken), param);
                HisRehaTrainViewFilter hisRehaTrainViewFilter = new HisRehaTrainViewFilter();
                hisRehaTrainViewFilter.SERE_SERV_REHA_IDs = ((list2 != null) ? list2.Select((HIS_SERE_SERV_REHA o) => o.ID).ToList() : new List<long>());
                List<V_HIS_REHA_TRAIN> list3 = new BackendAdapter(param).Get<List<V_HIS_REHA_TRAIN>>("api/HisRehaTrain/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisRehaTrainViewFilter, new Action(SessionManager.ActionLostToken), param);
                List<Mps000063PDO.ExeHisSereServRehaADO> list4 = new List<Mps000063PDO.ExeHisSereServRehaADO>();
                if (list3 != null && list3.Count > 0)
                {
                    var enumerable = list3
                                    .GroupBy(
                                        x => x.REHA_TRAIN_TYPE_ID, // Nhóm theo SERVICE_ID
                                        (key, group) => new
                                        {
                                            SERVICE_ID = key,
                                            // Nếu bạn vẫn cần giữ lại danh sách chi tiết các item trong nhóm:
                                            SereServRehas = group.Select(x => new
                                            {
                                                SereServReha = x,
                                                SERVICE_ID = x.REHA_TRAIN_TYPE_ID,
                                                SERVICE_CODE = x.REHA_TRAIN_TYPE_CODE,
                                                SERVICE_NAME = x.REHA_TRAIN_TYPE_NAME,
                                                REHA_TRAIN_UNIT_CODE = x.REHA_TRAIN_UNIT_CODE,
                                                REHA_TRAIN_UNIT_NAME = x.REHA_TRAIN_UNIT_NAME
                                            }).ToList(),

                                            // Tính tổng AMOUNT trực tiếp từ group
                                            AMOUNT_REHA_SUM = group.Sum(x => x.AMOUNT)
                                        });
                    if (enumerable != null)
                    {
                        foreach (var itemGroup in enumerable)
                        {
                            Mps000063PDO.ExeHisSereServRehaADO exeHisSereServRehaADO = new Mps000063PDO.ExeHisSereServRehaADO();
                            exeHisSereServRehaADO.AMOUNT = itemGroup.AMOUNT_REHA_SUM;

                            // Lấy ngay phần tử đầu tiên trong nhóm, không cần lọc lại theo SERVICE_ID vì cả nhóm đã chung ID rồi
                            var anon = itemGroup.SereServRehas.FirstOrDefault();
                            if (anon != null)
                            {
                                exeHisSereServRehaADO.REHA_TRAIN_TYPE_CODE = anon.SERVICE_CODE;
                                exeHisSereServRehaADO.REHA_TRAIN_TYPE_NAME = anon.SERVICE_NAME;
                                exeHisSereServRehaADO.REHA_TRAIN_UNIT_CODE = anon.REHA_TRAIN_UNIT_CODE;
                                exeHisSereServRehaADO.REHA_TRAIN_UNIT_NAME = anon.REHA_TRAIN_UNIT_NAME;
                            }

                            list4.Add(exeHisSereServRehaADO);
                        }
                    }
                }
                Mps000063PDO data = new Mps000063PDO(patientById, list, hIS_REHA_SUM, list4, bedRoomName, serviceReqPrintRaw);
                WaitingManager.Hide();
                PrintData(printTypeCode, fileName, data, ref result);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void InPhieuYeuCauChiDinhMau(string printTypeCode, string fileName, ref bool result)
        {
            try
            {
                CommonParam commonParam = new CommonParam();
                HisServiceReqViewFilter hisServiceReqViewFilter = new HisServiceReqViewFilter();
                hisServiceReqViewFilter.ID = currentServiceReqPrint.ID;
                V_HIS_SERVICE_REQ v_HIS_SERVICE_REQ = new BackendAdapter(commonParam).Get<List<V_HIS_SERVICE_REQ>>("api/HisServiceReq/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqViewFilter, new Action(SessionManager.ActionLostToken), commonParam).FirstOrDefault();
                HisExpMestFilter hisExpMestFilter = new HisExpMestFilter();
                hisExpMestFilter.SERVICE_REQ_ID = v_HIS_SERVICE_REQ.ID;
                HIS_EXP_MEST hIS_EXP_MEST = new BackendAdapter(commonParam).Get<List<HIS_EXP_MEST>>("api/HisExpMest/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestFilter, new Action(SessionManager.ActionLostToken), commonParam).FirstOrDefault();
                HisTreatmentViewFilter hisTreatmentViewFilter = new HisTreatmentViewFilter();
                hisTreatmentViewFilter.ID = v_HIS_SERVICE_REQ.TREATMENT_ID;
                V_HIS_TREATMENT v_HIS_TREATMENT = new BackendAdapter(commonParam).Get<List<V_HIS_TREATMENT>>("api/HisTreatment/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentViewFilter, new Action(SessionManager.ActionLostToken), commonParam).FirstOrDefault();
                List<V_HIS_TREATMENT_BED_ROOM> lstTreatmentBedRoom = null;
                if (v_HIS_TREATMENT != null)
                {
                    HisTreatmentBedRoomViewFilter hisTreatmentBedRoomViewFilter = new HisTreatmentBedRoomViewFilter();
                    hisTreatmentBedRoomViewFilter.TREATMENT_ID = v_HIS_TREATMENT.ID;
                    hisTreatmentBedRoomViewFilter.IS_IN_ROOM = true;
                    lstTreatmentBedRoom = new BackendAdapter(commonParam).Get<List<V_HIS_TREATMENT_BED_ROOM>>("/api/HisTreatmentBedRoom/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentBedRoomViewFilter, commonParam);
                }
                List<V_HIS_SERE_SERV_1> lstSereServ = null;
                if (v_HIS_SERVICE_REQ != null)
                {
                    HisSereServView1Filter hisSereServView1Filter = new HisSereServView1Filter();
                    hisSereServView1Filter.SERVICE_REQ_PARENT_ID = v_HIS_SERVICE_REQ.ID;
                    lstSereServ = new BackendAdapter(commonParam).Get<List<V_HIS_SERE_SERV_1>>("api/HisSereServ/GetView1", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServView1Filter, commonParam);
                }
                List<V_HIS_EXP_MEST_BLTY_REQ_1> hisExpMestBltys = null;
                List<V_HIS_EXP_MEST_BLOOD> expMestBloodList = null;
                if (hIS_EXP_MEST != null)
                {
                    HisExpMestBltyReqView1Filter hisExpMestBltyReqView1Filter = new HisExpMestBltyReqView1Filter();
                    hisExpMestBltyReqView1Filter.EXP_MEST_ID = hIS_EXP_MEST.ID;
                    hisExpMestBltys = new BackendAdapter(commonParam).Get<List<V_HIS_EXP_MEST_BLTY_REQ_1>>("api/HisExpMestBltyReq/GetView1", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestBltyReqView1Filter, commonParam);
                    HisExpMestBloodViewFilter hisExpMestBloodViewFilter = new HisExpMestBloodViewFilter();
                    hisExpMestBloodViewFilter.EXP_MEST_ID = hIS_EXP_MEST.ID;
                    expMestBloodList = new BackendAdapter(param).Get<List<V_HIS_EXP_MEST_BLOOD>>("api/HisExpMestBlood/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestBloodViewFilter, param);
                }
                string text = ((v_HIS_TREATMENT != null) ? v_HIS_TREATMENT.TREATMENT_CODE : "");
                Mps000108PDO data = new Mps000108PDO(hIS_EXP_MEST, hisExpMestBltys, v_HIS_TREATMENT, v_HIS_SERVICE_REQ, expMestBloodList, lstTreatmentBedRoom, lstSereServ);
                string printerName = "";
                if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
                {
                    printerName = GlobalVariables.dicPrinter[printTypeCode];
                }
                InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode(text, printTypeCode, (currentModule != null) ? currentModule.RoomId : 0);
                if (ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ASSIGN_PRESCRIPTION__IS_PRINT_NOW") == "1")
                {
                    result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName)
                    {
                        EmrInputADO = emrInputADO
                    });
                }
                else if (ConfigApplications.CheDoInChoCacChucNangTrongPhanMem == 2)
                {
                    result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName)
                    {
                        EmrInputADO = emrInputADO
                    });
                }
                else
                {
                    result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.Show, printerName)
                    {
                        EmrInputADO = emrInputADO
                    });
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void InPhieuYeuCauDonThuocYHocCoTruyen()
        {
            try
            {
                if (prescriptionPrint != null || currentServiceReqPrint != null)
                {
                    bool flag = false;
                    if (prescriptionPrint == null)
                    {
                        flag = true;
                        prescriptionPrint = new HIS_EXP_MEST();
                    }
                    if (currentServiceReqPrint == null)
                    {
                        flag = true;
                        currentServiceReqPrint = new HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO();
                    }
                    OutPatientPresResultSDO outPatientPresResultSDO = new OutPatientPresResultSDO();
                    outPatientPresResultSDO.ExpMests = new List<HIS_EXP_MEST> { prescriptionPrint };
                    outPatientPresResultSDO.ServiceReqs = new List<HIS_SERVICE_REQ> { currentServiceReqPrint };
                    List<OutPatientPresResultSDO> list = new List<OutPatientPresResultSDO>();
                    list.Add(outPatientPresResultSDO);
                    PrintPrescriptionProcessor printPrescriptionProcessor = new PrintPrescriptionProcessor(list, currentModule);
                    printPrescriptionProcessor.Print("Mps000050", false);
                    if (flag)
                    {
                        prescriptionPrint = null;
                        currentServiceReqPrint = null;
                    }
                }
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                LogSystem.Warn(ex);
            }
        }

        private void InDonThuocVatTu()
        {
            try
            {
                if (prescriptionPrint != null || currentServiceReqPrint != null)
                {
                    bool flag = false;
                    if (prescriptionPrint == null)
                    {
                        flag = true;
                        prescriptionPrint = new HIS_EXP_MEST();
                    }
                    if (currentServiceReqPrint == null)
                    {
                        flag = true;
                        currentServiceReqPrint = new HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO();
                    }
                    OutPatientPresResultSDO outPatientPresResultSDO = new OutPatientPresResultSDO();
                    outPatientPresResultSDO.ExpMests = new List<HIS_EXP_MEST> { prescriptionPrint };
                    outPatientPresResultSDO.ServiceReqs = new List<HIS_SERVICE_REQ> { currentServiceReqPrint };
                    List<OutPatientPresResultSDO> list = new List<OutPatientPresResultSDO>();
                    list.Add(outPatientPresResultSDO);
                    PrintPrescriptionProcessor printPrescriptionProcessor = new PrintPrescriptionProcessor(list, currentModule);
                    printPrescriptionProcessor.Print("Mps000044", false);
                    if (flag)
                    {
                        prescriptionPrint = null;
                        currentServiceReqPrint = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void InPhieuYeuCauThuocVatTuTongHop()
        {
            try
            {
                if (prescriptionPrint != null || currentServiceReqPrint != null)
                {
                    bool flag = false;
                    if (prescriptionPrint == null)
                    {
                        flag = true;
                        prescriptionPrint = new HIS_EXP_MEST();
                    }
                    if (currentServiceReqPrint == null)
                    {
                        flag = true;
                        currentServiceReqPrint = new HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO();
                    }
                    OutPatientPresResultSDO outPatientPresResultSDO = new OutPatientPresResultSDO();
                    outPatientPresResultSDO.ExpMests = new List<HIS_EXP_MEST> { prescriptionPrint };
                    outPatientPresResultSDO.ServiceReqs = new List<HIS_SERVICE_REQ> { currentServiceReqPrint };
                    List<OutPatientPresResultSDO> list = new List<OutPatientPresResultSDO>();
                    list.Add(outPatientPresResultSDO);
                    PrintPrescriptionProcessor printPrescriptionProcessor = new PrintPrescriptionProcessor(list, currentModule);
                    printPrescriptionProcessor.Print("Mps000118", false);
                    if (flag)
                    {
                        prescriptionPrint = null;
                        currentServiceReqPrint = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void CreateThreadLoadDataForService(ThreadChiDinhDichVuADO data)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(LoadDataTreatment));
            Thread thread2 = new Thread(new ParameterizedThreadStart(LoadDataSereServ));
            Thread thread3 = new Thread(new ParameterizedThreadStart(LoadDataSereServBill));
            Thread thread4 = new Thread(new ParameterizedThreadStart(LoadDataSereServDeposit));
            try
            {
                thread.Start(data);
                thread2.Start(data);
                thread3.Start(data);
                thread4.Start(data);
                thread.Join();
                thread2.Join();
                thread3.Join();
                thread4.Join();
            }
            catch (Exception ex)
            {
                thread.Abort();
                thread2.Abort();
                thread3.Abort();
                thread4.Abort();
                LogSystem.Error(ex);
            }
        }

        private void LoadDataSereServDeposit(object data)
        {
            try
            {
                LoadThreadDataSereServDeposit((ThreadChiDinhDichVuADO)data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadThreadDataSereServDeposit(ThreadChiDinhDichVuADO data)
        {
            try
            {
                if (data == null || data.vHisServiceReq2Print == null)
                {
                    return;
                }
                List<HIS_SERE_SERV_DEPOSIT> source = new List<HIS_SERE_SERV_DEPOSIT>();
                List<HIS_SESE_DEPO_REPAY> ssRepay = new List<HIS_SESE_DEPO_REPAY>();
                CommonParam commonParam = new CommonParam();
                HisSereServDepositFilter hisSereServDepositFilter = new HisSereServDepositFilter();
                hisSereServDepositFilter.TDL_TREATMENT_ID = data.vHisServiceReq2Print.TREATMENT_ID;
                hisSereServDepositFilter.IS_CANCEL = false;
                List<HIS_SERE_SERV_DEPOSIT> list = new BackendAdapter(commonParam).Get<List<HIS_SERE_SERV_DEPOSIT>>("api/HisSereServDeposit/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServDepositFilter, new Action(SessionManager.ActionLostToken), commonParam);
                if (list != null && list.Count > 0)
                {
                    source = list;
                }
                HisSeseDepoRepayFilter hisSeseDepoRepayFilter = new HisSeseDepoRepayFilter();
                hisSeseDepoRepayFilter.TDL_TREATMENT_ID = data.vHisServiceReq2Print.TREATMENT_ID;
                hisSeseDepoRepayFilter.IS_CANCEL = false;
                List<HIS_SESE_DEPO_REPAY> list2 = new BackendAdapter(commonParam).Get<List<HIS_SESE_DEPO_REPAY>>("api/HisSeseDepoRepay/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSeseDepoRepayFilter, new Action(SessionManager.ActionLostToken), commonParam);
                if (list2 != null && list2.Count > 0)
                {
                    ssRepay = list2;
                }
                data.ListSereServDeposit = source.Where((HIS_SERE_SERV_DEPOSIT o) => !ssRepay.Exists((HIS_SESE_DEPO_REPAY e) => e.SERE_SERV_DEPOSIT_ID == o.ID)).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadDataSereServBill(object data)
        {
            try
            {
                LoadThreadDataSereServBill((ThreadChiDinhDichVuADO)data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadThreadDataSereServBill(ThreadChiDinhDichVuADO data)
        {
            try
            {
                if (data != null && data.vHisServiceReq2Print != null)
                {
                    CommonParam commonParam = new CommonParam();
                    HisSereServBillFilter hisSereServBillFilter = new HisSereServBillFilter();
                    hisSereServBillFilter.TDL_TREATMENT_ID = data.vHisServiceReq2Print.TREATMENT_ID;
                    hisSereServBillFilter.IS_NOT_CANCEL = true;
                    List<HIS_SERE_SERV_BILL> list = new BackendAdapter(commonParam).Get<List<HIS_SERE_SERV_BILL>>("api/HisSereServBill/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServBillFilter, new Action(SessionManager.ActionLostToken), commonParam);
                    if (list != null && list.Count > 0)
                    {
                        data.ListSereServBill = list;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadDataTreatment(object data)
        {
            try
            {
                LoadThreadDataTreatment((ThreadChiDinhDichVuADO)data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadThreadDataTreatment(ThreadChiDinhDichVuADO data)
        {
            try
            {
                if (data != null && data.vHisServiceReq2Print != null)
                {
                    data.hisTreatment = getTreatment(data.vHisServiceReq2Print.TREATMENT_ID);
                    data.vHisPatientTypeAlter = getPatientTypeAlter(data.vHisServiceReq2Print.TREATMENT_ID, 0L);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadDataSereServ(object data)
        {
            try
            {
                LoadThreadDataSereServ((ThreadChiDinhDichVuADO)data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadThreadDataSereServ(ThreadChiDinhDichVuADO data)
        {
            try
            {
                if (data != null && data.vHisServiceReq2Print != null)
                {
                    data.listVHisSereServ = GetSereServByServiceReqId(data.vHisServiceReq2Print.ID);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void CreateThreadLoadDataForChiDinhTongHop(ThreadChiDinhDichVuADO data)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(LoadDataServiceReq));
            Thread thread2 = new Thread(new ParameterizedThreadStart(LoadListDataSereServ));
            thread.Priority = ThreadPriority.Normal;
            try
            {
                thread.Start(data);
                thread2.Start(data);
                thread.Join();
                thread2.Join();
            }
            catch (Exception ex)
            {
                thread.Abort();
                thread2.Abort();
                LogSystem.Error(ex);
            }
        }

        private void LoadDataServiceReq(object data)
        {
            try
            {
                LoadThreadDataServiceReq((ThreadChiDinhDichVuADO)data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadThreadDataServiceReq(ThreadChiDinhDichVuADO data)
        {
            try
            {
                if (data != null && data.vHisServiceReq2Print != null)
                {
                    data.hisTreatment = getTreatment(data.vHisServiceReq2Print.TREATMENT_ID);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadListDataSereServ(object data)
        {
            try
            {
                LoadThreadListDataSereServ((ThreadChiDinhDichVuADO)data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadThreadListDataSereServ(ThreadChiDinhDichVuADO data)
        {
            try
            {
                if (data == null || data.vHisServiceReq2Print == null || listServiceReq == null || listServiceReq.Count <= 0)
                {
                    return;
                }
                data.listVHisSereServ = new List<V_HIS_SERE_SERV>();
                foreach (HIS.Desktop.Plugins.ServiceReqList.ADO.ServiceReqADO item in listServiceReq)
                {
                    data.listVHisSereServ.AddRange(GetSereServByServiceReqId(item.ID));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void CreateThreadLoadDataForPttt(ThreadPtttADO data)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(LoadDataTreatmentPttt));
            Thread thread2 = new Thread(new ParameterizedThreadStart(LoadListDataSereServPttt));
            thread.Priority = ThreadPriority.Normal;
            try
            {
                thread.Start(data);
                thread2.Start(data);
                thread.Join();
                thread2.Join();
            }
            catch (Exception ex)
            {
                thread.Abort();
                thread2.Abort();
                LogSystem.Error(ex);
            }
        }

        private void LoadDataTreatmentPttt(object data)
        {
            try
            {
                LoadThreadDataTreatmentPttt((ThreadPtttADO)data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadThreadDataTreatmentPttt(ThreadPtttADO data)
        {
            try
            {
                if (data != null && data.sereServPrint != null)
                {
                    data.patient = PrintGlobalStore.GetPatientById(data.sereServPrint.TDL_PATIENT_ID ?? 0);
                    data.vhisTreatment = PrintGlobalStore.getTreatment(data.sereServPrint.TDL_TREATMENT_ID ?? 0);
                    data.departmentTran = PrintGlobalStore.getDepartmentTran(data.sereServPrint.TDL_TREATMENT_ID ?? 0);
                    HisSereServView5Filter hisSereServView5Filter = new HisSereServView5Filter();
                    hisSereServView5Filter.ID = data.sereServPrint.ID;
                    data.sereServ5Print = new BackendAdapter(new CommonParam()).Get<List<V_HIS_SERE_SERV_5>>("api/HisSereServ/GetView5", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServView5Filter, new Action(SessionManager.ActionLostToken), null).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadListDataSereServPttt(object data)
        {
            try
            {
                LoadThreadListDataSereServPttt((ThreadPtttADO)data);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadThreadListDataSereServPttt(ThreadPtttADO data)
        {
            try
            {
                if (data != null && data.sereServPrint != null)
                {
                    LogSystem.Info("Begin get serviceReq, sereServPttt, ekipUser");
                    CommonParam commonParam = new CommonParam();
                    HisServiceReqViewFilter hisServiceReqViewFilter = new HisServiceReqViewFilter();
                    hisServiceReqViewFilter.ID = data.sereServPrint.SERVICE_REQ_ID;
                    List<V_HIS_SERVICE_REQ> list = new BackendAdapter(commonParam).Get<List<V_HIS_SERVICE_REQ>>("api/HisServiceReq/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqViewFilter, new Action(SessionManager.ActionLostToken), commonParam);
                    if (list != null && list.Count == 1)
                    {
                        data.serviceReq = list.First();
                    }
                    HisSereServPtttViewFilter hisSereServPtttViewFilter = new HisSereServPtttViewFilter();
                    hisSereServPtttViewFilter.SERE_SERV_ID = data.sereServPrint.ID;
                    List<V_HIS_SERE_SERV_PTTT> list2 = new BackendAdapter(commonParam).Get<List<V_HIS_SERE_SERV_PTTT>>("api/HisSereServPttt/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServPtttViewFilter, new Action(SessionManager.ActionLostToken), commonParam);
                    if (list2 != null && list2.Count > 0)
                    {
                        data.sereServPttts = list2.FirstOrDefault();
                    }
                    if (data.sereServPrint.EKIP_ID.HasValue)
                    {
                        HisEkipUserViewFilter hisEkipUserViewFilter = new HisEkipUserViewFilter();
                        hisEkipUserViewFilter.EKIP_ID = data.sereServPrint.EKIP_ID;
                        data.ekipUsers = new BackendAdapter(commonParam).Get<List<V_HIS_EKIP_USER>>("api/HisEkipUser/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisEkipUserViewFilter, new Action(SessionManager.ActionLostToken), commonParam);
                    }
                    else
                    {
                        data.ekipUsers = new List<V_HIS_EKIP_USER>();
                    }
                    HisSereServFileFilter hisSereServFileFilter = new HisSereServFileFilter();
                    hisSereServFileFilter.SERE_SERV_ID = data.sereServPrint.ID;
                    data.sereServFile = new BackendAdapter(commonParam).Get<List<HIS_SERE_SERV_FILE>>("api/HisSereServFile/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServFileFilter, commonParam);
                    LogSystem.Info("End get serviceReq, sereServPttt, ekipUser");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
