using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.Library.CacheClient;
using HIS.Desktop.LocalStorage.ConfigSystem;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.Plugins.Library.EmrGenerate;
using HIS.Desktop.Plugins.ServiceReqList.ADO;
using HIS.Desktop.Plugins.ServiceReqList.Resources;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Common.RichEditor;
using Inventec.Common.RichEditor.Base;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.String;
using Inventec.Core;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Common.Modules;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;
using MPS;
using MPS.Processor.Mps000433.PDO;
using MPS.ProcessorBase;
using MPS.ProcessorBase.Core;

namespace HIS.Desktop.Plugins.ServiceReqList.Reason
{
	public class frmReason : FormBase
	{
		private ServiceReqADO serviceReqAdo;

		private ListMedicineADO sereServAdo;

		private string moduleLink = "HIS.Desktop.Plugins.ServiceReqList";

		private ControlStateWorker controlStateWorker;

		private List<ControlStateRDO> currentControlStateRDO;

		private Module module;

		private bool isSereServ;

		private bool isInit;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private MemoEdit txtReason;

		private LayoutControlItem layoutControlItem1;

		private SimpleButton btnPrint;

		private SimpleButton btnSave;

		private CheckEdit chkReasonClose;

		private CheckEdit chkResonPrint;

		private LayoutControlItem layoutControlItem2;

		private LayoutControlItem layoutControlItem3;

		private LayoutControlItem layoutControlItem4;

		private LayoutControlItem layoutControlItem5;

		private EmptySpaceItem emptySpaceItem1;

		public frmReason(Module module)
			: base(module)
		{
			InitializeComponent();
		}

		public frmReason(Module module, ServiceReqADO _serviceReq)
			: this(module)
		{
			this.module = module;
			serviceReqAdo = _serviceReq;
			InitializeComponent();
		}

		public frmReason(Module module, ServiceReqADO _serviceReq, ListMedicineADO _sereServ)
			: this(module)
		{
			this.module = module;
			serviceReqAdo = _serviceReq;
			sereServAdo = _sereServ;
			InitializeComponent();
		}

		private void frmReason_Load(object sender, EventArgs e)
		{
			try
			{
				SetCaptionByLanguageKey();
				SetDefaultValue();
				InitControlState();
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
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmReason.layoutControl1.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				btnPrint.Text = Inventec.Common.Resource.Get.Value("frmReason.btnPrint.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				btnSave.Text = Inventec.Common.Resource.Get.Value("frmReason.btnSave.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				chkReasonClose.Properties.Caption = Inventec.Common.Resource.Get.Value("frmReason.chkReasonClose.Properties.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				chkResonPrint.Properties.Caption = Inventec.Common.Resource.Get.Value("frmReason.chkResonPrint.Properties.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("frmReason.layoutControlItem1.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				Text = Inventec.Common.Resource.Get.Value("frmReason.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetDefaultValue()
		{
			try
			{
				txtReason.Text = "BS YC hoàn dịch vụ";
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
				isInit = true;
				controlStateWorker = new ControlStateWorker();
				currentControlStateRDO = controlStateWorker.GetData(moduleLink);
				if (currentControlStateRDO != null && currentControlStateRDO.Count > 0)
				{
					foreach (ControlStateRDO item in currentControlStateRDO)
					{
						if (item.KEY == chkResonPrint.Name)
						{
							chkResonPrint.Checked = item.VALUE == "1";
						}
						if (item.KEY == chkReasonClose.Name)
						{
							chkReasonClose.Checked = item.VALUE == "1";
						}
					}
				}
				isInit = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkResonPrint_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (isInit)
				{
					return;
				}
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkResonPrint.Name && o.MODULE_LINK == moduleLink).FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (chkResonPrint.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = chkResonPrint.Name;
					csAddOrUpdate.VALUE = (chkResonPrint.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = moduleLink;
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(csAddOrUpdate);
				}
				controlStateWorker.SetData(currentControlStateRDO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkReasonClose_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (isInit)
				{
					return;
				}
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkReasonClose.Name && o.MODULE_LINK == moduleLink).FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (chkReasonClose.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = chkReasonClose.Name;
					csAddOrUpdate.VALUE = (chkReasonClose.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = moduleLink;
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(csAddOrUpdate);
				}
				controlStateWorker.SetData(currentControlStateRDO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (CheckString.IsOverMaxLengthUTF8(txtReason.Text, 500))
				{
					XtraMessageBox.Show("Vui lòng nhập lý do nhỏ hơn 500 ký tự.");
					return;
				}
				bool value = false;
				CommonParam commonParam = new CommonParam();
				if (sereServAdo != null)
				{
					HisSereServAcceptNoExecuteSDO hisSereServAcceptNoExecuteSDO = new HisSereServAcceptNoExecuteSDO();
					hisSereServAcceptNoExecuteSDO.SereServId = sereServAdo.ID;
					hisSereServAcceptNoExecuteSDO.WorkingRoomId = module.RoomId;
					hisSereServAcceptNoExecuteSDO.NoExecuteReason = txtReason.Text;
					HIS_SERVICE_REQ hIS_SERVICE_REQ = new BackendAdapter(commonParam).Post<HIS_SERVICE_REQ>("api/HisSereServ/AcceptNoExecute", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServAcceptNoExecuteSDO, commonParam);
					if (hIS_SERVICE_REQ != null)
					{
						value = true;
						if (chkResonPrint.Checked)
						{
							ReasonPrint();
						}
						if (chkReasonClose.Checked)
						{
							Close();
						}
					}
				}
				else
				{
					HisServiceReqAcceptNoExecuteSDO hisServiceReqAcceptNoExecuteSDO = new HisServiceReqAcceptNoExecuteSDO();
					hisServiceReqAcceptNoExecuteSDO.ServiceReqId = serviceReqAdo.ID;
					hisServiceReqAcceptNoExecuteSDO.WorkingRoomId = module.RoomId;
					hisServiceReqAcceptNoExecuteSDO.NoExecuteReason = txtReason.Text;
					HIS_SERVICE_REQ hIS_SERVICE_REQ = new BackendAdapter(commonParam).Post<HIS_SERVICE_REQ>("api/HisSereServ/AcceptNoExecuteByServiceReq", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqAcceptNoExecuteSDO, commonParam);
					if (hIS_SERVICE_REQ != null)
					{
						value = true;
						if (chkResonPrint.Checked)
						{
							ReasonPrint();
						}
						if (chkReasonClose.Checked)
						{
							Close();
						}
					}
				}
				MessageManager.Show(this, commonParam, value);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ReasonPrint()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			try
			{
				RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), PrintStoreLocation.PrintTemplatePath);
				val.RunPrintTemplate("Mps000433", new DelegateRunPrinter(DelegateRunPrinter));
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
				if (printTypeCode != null && printTypeCode == "Mps000433")
				{
					InGiayDeNghiDoiTraDichVu(printTypeCode, fileName, ref result);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private void InGiayDeNghiDoiTraDichVu(string printTypeCode, string fileName, ref bool result)
		{
			try
			{
				if (serviceReqAdo.ID <= 0)
				{
					return;
				}
				V_HIS_SERVICE_REQ serviceReqForPrint = GetServiceReqForPrint(serviceReqAdo.ID);
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
				InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode(serviceReqAdo.TDL_TREATMENT_CODE, printTypeCode, (module != null) ? module.RoomId : 0);
				if (GlobalVariables.CheDoInChoCacChucNangTrongPhanMem == 2)
				{
					result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName)
					{
						EmrInputADO = emrInputADO
					});
				}
				else
				{
					result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.ShowDialog, printerName)
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

		private void LoadCurrentPatientTypeAlter(long treatmentId, ref V_HIS_PATIENT_TYPE_ALTER hisPatientTypeAlter)
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

		private void btnPrint_Click(object sender, EventArgs e)
		{
			try
			{
				ReasonPrint();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public override void ProcessDisposeModuleDataAfterClose()
		{
			try
			{
				isSereServ = false;
				module = null;
				currentControlStateRDO = null;
				controlStateWorker = null;
				moduleLink = null;
				sereServAdo = null;
				serviceReqAdo = null;
				btnPrint.Click -= new EventHandler(btnPrint_Click);
				btnSave.Click -= new EventHandler(btnSave_Click);
				base.FormClosed -= new FormClosedEventHandler(frmReason_FormClosed);
				base.Load -= new EventHandler(frmReason_Load);
				emptySpaceItem1 = null;
				layoutControlItem5 = null;
				layoutControlItem4 = null;
				layoutControlItem3 = null;
				layoutControlItem2 = null;
				chkResonPrint = null;
				chkReasonClose = null;
				btnSave = null;
				btnPrint = null;
				layoutControlItem1 = null;
				txtReason = null;
				layoutControlGroup1 = null;
				layoutControl1 = null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void frmReason_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				ControlStateRDO controlStateRDO = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkReasonClose.Name && o.MODULE_LINK == moduleLink).FirstOrDefault() : null);
				if (controlStateRDO != null)
				{
					controlStateRDO.VALUE = (chkReasonClose.Checked ? "1" : "");
				}
				else
				{
					controlStateRDO = new ControlStateRDO();
					controlStateRDO.KEY = chkReasonClose.Name;
					controlStateRDO.VALUE = (chkReasonClose.Checked ? "1" : "");
					controlStateRDO.MODULE_LINK = moduleLink;
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(controlStateRDO);
				}
				controlStateWorker.SetData(currentControlStateRDO);
				ControlStateRDO controlStateRDO2 = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkResonPrint.Name && o.MODULE_LINK == moduleLink).FirstOrDefault() : null);
				if (controlStateRDO2 != null)
				{
					controlStateRDO2.VALUE = (chkResonPrint.Checked ? "1" : "");
				}
				else
				{
					controlStateRDO2 = new ControlStateRDO();
					controlStateRDO2.KEY = chkResonPrint.Name;
					controlStateRDO2.VALUE = (chkResonPrint.Checked ? "1" : "");
					controlStateRDO2.MODULE_LINK = moduleLink;
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(controlStateRDO2);
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
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
			this.btnSave = new DevExpress.XtraEditors.SimpleButton();
			this.chkReasonClose = new DevExpress.XtraEditors.CheckEdit();
			this.chkResonPrint = new DevExpress.XtraEditors.CheckEdit();
			this.txtReason = new DevExpress.XtraEditors.MemoEdit();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.chkReasonClose.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkResonPrint.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtReason.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.btnPrint);
			this.layoutControl1.Controls.Add(this.btnSave);
			this.layoutControl1.Controls.Add(this.chkReasonClose);
			this.layoutControl1.Controls.Add(this.chkResonPrint);
			this.layoutControl1.Controls.Add(this.txtReason);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(449, 91);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.btnPrint.Location = new System.Drawing.Point(371, 67);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(76, 22);
			this.btnPrint.StyleController = this.layoutControl1;
			this.btnPrint.TabIndex = 8;
			this.btnPrint.Text = "In";
			this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
			this.btnSave.Location = new System.Drawing.Point(291, 67);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(76, 22);
			this.btnSave.StyleController = this.layoutControl1;
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Lưu";
			this.btnSave.Click += new System.EventHandler(btnSave_Click);
			this.chkReasonClose.Location = new System.Drawing.Point(239, 67);
			this.chkReasonClose.Name = "chkReasonClose";
			this.chkReasonClose.Properties.Caption = "Đóng";
			this.chkReasonClose.Size = new System.Drawing.Size(48, 19);
			this.chkReasonClose.StyleController = this.layoutControl1;
			this.chkReasonClose.TabIndex = 6;
			this.chkReasonClose.CheckedChanged += new System.EventHandler(chkReasonClose_CheckedChanged);
			this.chkResonPrint.Location = new System.Drawing.Point(203, 67);
			this.chkResonPrint.Name = "chkResonPrint";
			this.chkResonPrint.Properties.Caption = "In";
			this.chkResonPrint.Size = new System.Drawing.Size(32, 19);
			this.chkResonPrint.StyleController = this.layoutControl1;
			this.chkResonPrint.TabIndex = 5;
			this.chkResonPrint.CheckedChanged += new System.EventHandler(chkResonPrint_CheckedChanged);
			this.txtReason.Location = new System.Drawing.Point(35, 2);
			this.txtReason.Name = "txtReason";
			this.txtReason.Size = new System.Drawing.Size(412, 61);
			this.txtReason.StyleController = this.layoutControl1;
			this.txtReason.TabIndex = 4;
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.False;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[6] { this.layoutControlItem1, this.layoutControlItem2, this.layoutControlItem3, this.layoutControlItem4, this.layoutControlItem5, this.emptySpaceItem1 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Size = new System.Drawing.Size(449, 91);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.Control = this.txtReason;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(449, 65);
			this.layoutControlItem1.Text = "Lý do:";
			this.layoutControlItem1.TextSize = new System.Drawing.Size(30, 13);
			this.layoutControlItem2.Control = this.chkResonPrint;
			this.layoutControlItem2.Location = new System.Drawing.Point(201, 65);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(36, 26);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.layoutControlItem3.Control = this.chkReasonClose;
			this.layoutControlItem3.Location = new System.Drawing.Point(237, 65);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(52, 26);
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextVisible = false;
			this.layoutControlItem4.Control = this.btnSave;
			this.layoutControlItem4.Location = new System.Drawing.Point(289, 65);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(80, 26);
			this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem4.TextVisible = false;
			this.layoutControlItem5.Control = this.btnPrint;
			this.layoutControlItem5.Location = new System.Drawing.Point(369, 65);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Size = new System.Drawing.Size(80, 26);
			this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem5.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 65);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(201, 26);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(449, 91);
			base.Controls.Add(this.layoutControl1);
			base.Name = "frmReason";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Lý do không thực hiện";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmReason_FormClosed);
			base.Load += new System.EventHandler(frmReason_Load);
			base.Controls.SetChildIndex(this.layoutControl1, 0);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.chkReasonClose.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkResonPrint.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtReason.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
