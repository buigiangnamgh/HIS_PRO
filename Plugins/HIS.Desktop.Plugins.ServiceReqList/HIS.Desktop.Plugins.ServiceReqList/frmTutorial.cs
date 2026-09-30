using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.Library.CacheClient;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.Plugins.ServiceReqList.ADO;
using HIS.Desktop.Plugins.ServiceReqList.Resources;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Message;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace HIS.Desktop.Plugins.ServiceReqList
{
	public class frmTutorial : FormBase
	{
		private const string moduleLink = "HIS.Desktop.Plugins.ServiceReqList";

		private int positionHandleControl = -1;

		private ListMedicineADO listMedicineADO;

		private DelegateRefreshData delegateRefresh;

		private bool isNotLoadWhileChangeControlStateInFirst;

		private ControlStateWorker controlStateWorker;

		private List<ControlStateRDO> currentControlStateRDO;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private SimpleButton btnSave;

		private MemoEdit txtHuongDanSuDung;

		private LayoutControlGroup layoutControlGroup1;

		private LayoutControlItem layoutControlItem1;

		private LayoutControlItem layoutControlItem2;

		private EmptySpaceItem emptySpaceItem1;

		private BarManager barManager1;

		private Bar bar1;

		private BarButtonItem barButtonItem1;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private DXValidationProvider dxValidationProvider1;

		private SpinEdit spinSpeed;

		private LayoutControlItem layoutControlItem3;

		private SpinEdit spinDayNumber;

		private LayoutControlItem layoutControlItem4;

		private EmptySpaceItem emptySpaceItem2;

		private SpinEdit spinCountUsedBefore;

		private LayoutControlItem lciSpinCountUsedBefore;

		public frmTutorial(DelegateRefreshData delegateRefresh, ListMedicineADO listMedicineADO)
			: base(null)
		{
			InitializeComponent();
			try
			{
				SetCaptionByLanguageKey();
				this.listMedicineADO = listMedicineADO;
				this.delegateRefresh = delegateRefresh;
				string filePath = Path.Combine(ApplicationStoreLocation.ApplicationStartupPath, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]);
				base.Icon = Icon.ExtractAssociatedIcon(filePath);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetCaptionByLanguageKey()
		{
			try
			{
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmTutorial.layoutControl1.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				bar1.Text = Inventec.Common.Resource.Get.Value("frmTutorial.bar1.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				barButtonItem1.Caption = Inventec.Common.Resource.Get.Value("frmTutorial.barButtonItem1.Caption", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				btnSave.Text = Inventec.Common.Resource.Get.Value("frmTutorial.btnSave.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("frmTutorial.layoutControlItem1.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				layoutControlItem3.Text = Inventec.Common.Resource.Get.Value("frmTutorial.layoutControlItem3.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				layoutControlItem4.Text = Inventec.Common.Resource.Get.Value("frmTutorial.layoutControlItem4.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				lciSpinCountUsedBefore.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("frmTutorial.lciSpinCountUsedBefore.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				lciSpinCountUsedBefore.Text = Inventec.Common.Resource.Get.Value("frmTutorial.lciSpinCountUsedBefore.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
				Text = Inventec.Common.Resource.Get.Value("frmTutorial.Text", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void frmTutorial_Load(object sender, EventArgs e)
		{
			try
			{
				txtHuongDanSuDung.Text = listMedicineADO.HuongDanSuDung;
				spinSpeed.EditValue = listMedicineADO.TocDoTruyen;
				CommonParam commonParam = new CommonParam();
				HisExpMestMedicineFilter hisExpMestMedicineFilter = new HisExpMestMedicineFilter();
				hisExpMestMedicineFilter.ID = listMedicineADO.ExpMestMedicineId;
				List<HIS_EXP_MEST_MEDICINE> list = new BackendAdapter(commonParam).Get<List<HIS_EXP_MEST_MEDICINE>>("api/HisExpMestMedicine/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisExpMestMedicineFilter, commonParam);
				if (list != null)
				{
					spinCountUsedBefore.Value = Parse.ToDecimal(list.FirstOrDefault().PREVIOUS_USING_COUNT.ToString());
				}
				if (listMedicineADO.xpMestMedicine != null)
				{
					long iD = listMedicineADO.xpMestMedicine.ID;
				}
				if (listMedicineADO.USE_TIME_TO.HasValue)
				{
					DateTime? dateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(listMedicineADO.USE_TIME_TO.Value);
					LogSystem.Debug("useTimeTo" + dateTime);
					DateTime? dateTime2 = null;
					dateTime2 = ((!listMedicineADO.USE_TIME.HasValue) ? Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(listMedicineADO.TDL_INTRUCTION_TIME) : Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(listMedicineADO.USE_TIME.Value));
					LogSystem.Debug("intructionTime" + dateTime2);
					if (dateTime.Value.CompareTo(dateTime2) == 0)
					{
						spinDayNumber.EditValue = 1;
					}
					else
					{
						TimeSpan? timeSpan = dateTime - dateTime2;
						if (timeSpan.HasValue)
						{
							spinDayNumber.EditValue = timeSpan.Value.Days + 1;
						}
					}
				}
				else if (listMedicineADO.serviceReqMety.USE_TIME_TO.HasValue)
				{
					DateTime? dateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(listMedicineADO.serviceReqMety.USE_TIME_TO.Value);
					LogSystem.Debug("useTimeTo" + dateTime);
					DateTime? dateTime2 = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(listMedicineADO.serviceReqMety.CREATE_TIME.Value);
					LogSystem.Debug("intructionTime" + dateTime2);
					if (dateTime.Value.CompareTo(dateTime2) == 0)
					{
						spinDayNumber.EditValue = 1;
					}
					else
					{
						TimeSpan? timeSpan = dateTime - dateTime2;
						if (timeSpan.HasValue)
						{
							spinDayNumber.EditValue = timeSpan.Value.Days + 1;
						}
					}
				}
				ValidationMaxlength(txtHuongDanSuDung, 1000);
				ValidationBiggerThan0(spinDayNumber);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
		{
			btnSave_Click(null, null);
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				positionHandleControl = -1;
				bool value = false;
				if (!dxValidationProvider1.Validate())
				{
					return;
				}
				CommonParam commonParam = new CommonParam();
				if (listMedicineADO.serviceReqMety == null)
				{
					HIS_EXP_MEST_MEDICINE hIS_EXP_MEST_MEDICINE = new HIS_EXP_MEST_MEDICINE();
					hIS_EXP_MEST_MEDICINE.ID = listMedicineADO.ExpMestMedicineId;
					hIS_EXP_MEST_MEDICINE.TUTORIAL = txtHuongDanSuDung.Text.Trim();
					hIS_EXP_MEST_MEDICINE.SPEED = (decimal?)spinSpeed.EditValue;
					DateTime? dateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime((listMedicineADO.USE_TIME > 0) ? listMedicineADO.USE_TIME.Value : listMedicineADO.TDL_INTRUCTION_TIME);
					long num = 0L;
					if (spinDayNumber.EditValue != null && Parse.ToInt64(spinDayNumber.EditValue.ToString()) > 0)
					{
						num = Parse.ToInt64(spinDayNumber.EditValue.ToString()) - 1;
					}
					if (spinCountUsedBefore.EditValue != null && Parse.ToInt64(spinCountUsedBefore.EditValue.ToString()) > 0)
					{
						hIS_EXP_MEST_MEDICINE.PREVIOUS_USING_COUNT = Parse.ToInt64(spinCountUsedBefore.Value.ToString());
					}
					DateTime value2 = dateTime.Value.AddDays(num);
					hIS_EXP_MEST_MEDICINE.USE_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(value2);
					WaitingManager.Show();
					HIS_EXP_MEST_MEDICINE hIS_EXP_MEST_MEDICINE2 = new BackendAdapter(commonParam).Post<HIS_EXP_MEST_MEDICINE>("api/HisExpMestMedicine/UpdateCommonInfo", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hIS_EXP_MEST_MEDICINE, commonParam);
					if (hIS_EXP_MEST_MEDICINE2 != null)
					{
						value = true;
						if (delegateRefresh != null)
						{
							delegateRefresh();
						}
						Close();
					}
					WaitingManager.Hide();
				}
				else
				{
					listMedicineADO.serviceReqMety.TUTORIAL = txtHuongDanSuDung.Text.Trim();
					listMedicineADO.serviceReqMety.SPEED = (decimal?)spinSpeed.EditValue;
					DateTime? dateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(listMedicineADO.serviceReqMety.CREATE_TIME.Value);
					long num = 0L;
					if (spinDayNumber.EditValue != null && Parse.ToInt64(spinDayNumber.EditValue.ToString()) > 0)
					{
						num = Parse.ToInt64(spinDayNumber.EditValue.ToString()) - 1;
					}
					DateTime value2 = dateTime.Value.AddDays(num);
					listMedicineADO.serviceReqMety.USE_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(value2);
					WaitingManager.Show();
					HIS_SERVICE_REQ_METY hIS_SERVICE_REQ_METY = new BackendAdapter(commonParam).Post<HIS_SERVICE_REQ_METY>("api/HisServiceReqMety/UpdateCommonInfo", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, listMedicineADO.serviceReqMety, commonParam);
					if (hIS_SERVICE_REQ_METY != null)
					{
						value = true;
						if (delegateRefresh != null)
						{
							delegateRefresh();
						}
						Close();
					}
					WaitingManager.Hide();
				}
				MessageManager.Show(this, commonParam, value);
				SessionManager.ProcessTokenLost(commonParam);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dxValidationProvider1_ValidationFailed(object sender, ValidationFailedEventArgs e)
		{
			try
			{
				try
				{
					BaseEdit baseEdit = e.InvalidControl as BaseEdit;
					if (baseEdit == null)
					{
						return;
					}
					BaseEditViewInfo baseEditViewInfo = baseEdit.GetViewInfo() as BaseEditViewInfo;
					if (baseEditViewInfo == null)
					{
						return;
					}
					if (positionHandleControl == -1)
					{
						positionHandleControl = baseEdit.TabIndex;
						if (baseEdit.Visible)
						{
							baseEdit.SelectAll();
							baseEdit.Focus();
						}
					}
					if (positionHandleControl > baseEdit.TabIndex)
					{
						positionHandleControl = baseEdit.TabIndex;
						if (baseEdit.Visible)
						{
							baseEdit.SelectAll();
							baseEdit.Focus();
						}
					}
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidationMaxlength(MemoEdit control, int maxLength)
		{
			try
			{
				ValidateMaxLength validateMaxLength = new ValidateMaxLength();
				validateMaxLength.maxLength = maxLength;
				validateMaxLength.memoEdit = control;
				validateMaxLength.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(control, validateMaxLength);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidationBiggerThan0(SpinEdit control)
		{
			try
			{
				ValidateBiggerThan0 validateBiggerThan = new ValidateBiggerThan0();
				validateBiggerThan.spinEdit = control;
				validateBiggerThan.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(control, validateBiggerThan);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
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
					if (item.KEY == spinDayNumber.Name)
					{
						spinDayNumber.EditValue = item.VALUE == "1";
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void spinDayNumber_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtHuongDanSuDung.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void spinSpeed_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					btnSave.Focus();
				}
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
				currentControlStateRDO = null;
				controlStateWorker = null;
				isNotLoadWhileChangeControlStateInFirst = false;
				delegateRefresh = null;
				listMedicineADO = null;
				positionHandleControl = 0;
				barButtonItem1.ItemClick -= new ItemClickEventHandler(barButtonItem1_ItemClick);
				spinDayNumber.PreviewKeyDown -= new PreviewKeyDownEventHandler(spinDayNumber_PreviewKeyDown);
				spinSpeed.PreviewKeyDown -= new PreviewKeyDownEventHandler(spinSpeed_PreviewKeyDown);
				btnSave.Click -= new EventHandler(btnSave_Click);
				dxValidationProvider1.ValidationFailed -= new ValidationFailedEventHandler(dxValidationProvider1_ValidationFailed);
				base.Load -= new EventHandler(frmTutorial_Load);
				lciSpinCountUsedBefore = null;
				spinCountUsedBefore = null;
				emptySpaceItem2 = null;
				layoutControlItem4 = null;
				spinDayNumber = null;
				layoutControlItem3 = null;
				spinSpeed = null;
				dxValidationProvider1 = null;
				barDockControlRight = null;
				barDockControlLeft = null;
				barDockControlBottom = null;
				barDockControlTop = null;
				barButtonItem1 = null;
				bar1 = null;
				barManager1 = null;
				emptySpaceItem1 = null;
				layoutControlItem2 = null;
				layoutControlItem1 = null;
				layoutControlGroup1 = null;
				txtHuongDanSuDung = null;
				btnSave = null;
				layoutControl1 = null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
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
			this.spinCountUsedBefore = new DevExpress.XtraEditors.SpinEdit();
			this.barManager1 = new DevExpress.XtraBars.BarManager();
			this.bar1 = new DevExpress.XtraBars.Bar();
			this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.spinDayNumber = new DevExpress.XtraEditors.SpinEdit();
			this.spinSpeed = new DevExpress.XtraEditors.SpinEdit();
			this.btnSave = new DevExpress.XtraEditors.SimpleButton();
			this.txtHuongDanSuDung = new DevExpress.XtraEditors.MemoEdit();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.lciSpinCountUsedBefore = new DevExpress.XtraLayout.LayoutControlItem();
			this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.spinCountUsedBefore.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.spinDayNumber.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.spinSpeed.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtHuongDanSuDung.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciSpinCountUsedBefore).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.spinCountUsedBefore);
			this.layoutControl1.Controls.Add(this.spinDayNumber);
			this.layoutControl1.Controls.Add(this.spinSpeed);
			this.layoutControl1.Controls.Add(this.btnSave);
			this.layoutControl1.Controls.Add(this.txtHuongDanSuDung);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 29);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(723, 169);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			DevExpress.XtraEditors.SpinEdit spinEdit = this.spinCountUsedBefore;
			int[] bits = new int[4];
			spinEdit.EditValue = new decimal(bits);
			this.spinCountUsedBefore.Location = new System.Drawing.Point(417, 2);
			this.spinCountUsedBefore.MenuManager = this.barManager1;
			this.spinCountUsedBefore.Name = "spinCountUsedBefore";
			this.spinCountUsedBefore.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.spinCountUsedBefore.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.spinCountUsedBefore.Properties.IsFloatValue = false;
			this.spinCountUsedBefore.Properties.Mask.EditMask = "N00";
			this.spinCountUsedBefore.Size = new System.Drawing.Size(88, 20);
			this.spinCountUsedBefore.StyleController = this.layoutControl1;
			this.spinCountUsedBefore.TabIndex = 8;
			this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[1] { this.bar1 });
			this.barManager1.DockControls.Add(this.barDockControlTop);
			this.barManager1.DockControls.Add(this.barDockControlBottom);
			this.barManager1.DockControls.Add(this.barDockControlLeft);
			this.barManager1.DockControls.Add(this.barDockControlRight);
			this.barManager1.Form = this;
			this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[1] { this.barButtonItem1 });
			this.barManager1.MaxItemId = 1;
			this.bar1.BarName = "Tools";
			this.bar1.DockCol = 0;
			this.bar1.DockRow = 0;
			this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[1]
			{
				new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)
			});
			this.bar1.Text = "Tools";
			this.bar1.Visible = false;
			this.barButtonItem1.Caption = "Lưu";
			this.barButtonItem1.Id = 0;
			this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control);
			this.barButtonItem1.Name = "barButtonItem1";
			this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem1_ItemClick);
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(723, 29);
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 198);
			this.barDockControlBottom.Size = new System.Drawing.Size(723, 0);
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 169);
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(723, 29);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 169);
			this.spinDayNumber.EditValue = new decimal(new int[4] { 1, 0, 0, 0 });
			this.spinDayNumber.Location = new System.Drawing.Point(97, 2);
			this.spinDayNumber.MenuManager = this.barManager1;
			this.spinDayNumber.Name = "spinDayNumber";
			this.spinDayNumber.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.spinDayNumber.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.spinDayNumber.Properties.MaxValue = new decimal(new int[4] { 1000, 0, 0, 0 });
			this.spinDayNumber.Size = new System.Drawing.Size(61, 20);
			this.spinDayNumber.StyleController = this.layoutControl1;
			this.spinDayNumber.TabIndex = 7;
			this.spinDayNumber.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(spinDayNumber_PreviewKeyDown);
			DevExpress.XtraEditors.SpinEdit spinEdit2 = this.spinSpeed;
			bits = new int[4];
			spinEdit2.EditValue = new decimal(bits);
			this.spinSpeed.Location = new System.Drawing.Point(97, 121);
			this.spinSpeed.MenuManager = this.barManager1;
			this.spinSpeed.Name = "spinSpeed";
			this.spinSpeed.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.spinSpeed.Properties.MaxValue = new decimal(new int[4] { -1981284353, -1966660860, 0, 131072 });
			this.spinSpeed.Size = new System.Drawing.Size(624, 20);
			this.spinSpeed.StyleController = this.layoutControl1;
			this.spinSpeed.TabIndex = 6;
			this.spinSpeed.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(spinSpeed_PreviewKeyDown);
			this.btnSave.Location = new System.Drawing.Point(607, 145);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(114, 22);
			this.btnSave.StyleController = this.layoutControl1;
			this.btnSave.TabIndex = 5;
			this.btnSave.Text = "Lưu (Ctrl S)";
			this.btnSave.Click += new System.EventHandler(btnSave_Click);
			this.txtHuongDanSuDung.Location = new System.Drawing.Point(97, 26);
			this.txtHuongDanSuDung.Name = "txtHuongDanSuDung";
			this.txtHuongDanSuDung.Size = new System.Drawing.Size(624, 91);
			this.txtHuongDanSuDung.StyleController = this.layoutControl1;
			this.txtHuongDanSuDung.TabIndex = 4;
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[7] { this.layoutControlItem1, this.layoutControlItem2, this.emptySpaceItem1, this.layoutControlItem3, this.layoutControlItem4, this.emptySpaceItem2, this.lciSpinCountUsedBefore });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(723, 169);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.layoutControlItem1.Control = this.txtHuongDanSuDung;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(723, 95);
			this.layoutControlItem1.Text = "Hướng dẫn sử dụng:";
			this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem1.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem1.TextToControlDistance = 5;
			this.layoutControlItem2.Control = this.btnSave;
			this.layoutControlItem2.Location = new System.Drawing.Point(605, 143);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(118, 26);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 143);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(605, 26);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem3.Control = this.spinSpeed;
			this.layoutControlItem3.Location = new System.Drawing.Point(0, 119);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(723, 24);
			this.layoutControlItem3.Text = "Tốc độ truyền:";
			this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem3.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem3.TextToControlDistance = 5;
			this.layoutControlItem4.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem4.Control = this.spinDayNumber;
			this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(160, 24);
			this.layoutControlItem4.Text = "Số ngày:";
			this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem4.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem4.TextToControlDistance = 5;
			this.emptySpaceItem2.AllowHotTrack = false;
			this.emptySpaceItem2.Location = new System.Drawing.Point(507, 0);
			this.emptySpaceItem2.Name = "emptySpaceItem2";
			this.emptySpaceItem2.Size = new System.Drawing.Size(216, 24);
			this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
			this.lciSpinCountUsedBefore.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciSpinCountUsedBefore.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciSpinCountUsedBefore.Control = this.spinCountUsedBefore;
			this.lciSpinCountUsedBefore.Location = new System.Drawing.Point(160, 0);
			this.lciSpinCountUsedBefore.Name = "lciSpinCountUsedBefore";
			this.lciSpinCountUsedBefore.OptionsToolTip.ToolTip = "Số ngày sử dụng thuốc trước đó";
			this.lciSpinCountUsedBefore.Size = new System.Drawing.Size(347, 24);
			this.lciSpinCountUsedBefore.Text = "Số lần sử dụng trước đó:";
			this.lciSpinCountUsedBefore.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciSpinCountUsedBefore.TextSize = new System.Drawing.Size(250, 20);
			this.lciSpinCountUsedBefore.TextToControlDistance = 5;
			this.dxValidationProvider1.ValidationFailed += new DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventHandler(dxValidationProvider1_ValidationFailed);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(723, 198);
			base.Controls.Add(this.layoutControl1);
			base.Controls.Add(this.barDockControlLeft);
			base.Controls.Add(this.barDockControlRight);
			base.Controls.Add(this.barDockControlBottom);
			base.Controls.Add(this.barDockControlTop);
			base.Name = "frmTutorial";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sửa thông tin chung";
			base.Load += new System.EventHandler(frmTutorial_Load);
			base.Controls.SetChildIndex(this.barDockControlTop, 0);
			base.Controls.SetChildIndex(this.barDockControlBottom, 0);
			base.Controls.SetChildIndex(this.barDockControlRight, 0);
			base.Controls.SetChildIndex(this.barDockControlLeft, 0);
			base.Controls.SetChildIndex(this.layoutControl1, 0);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.spinCountUsedBefore.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.spinDayNumber.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.spinSpeed.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtHuongDanSuDung.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciSpinCountUsedBefore).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
