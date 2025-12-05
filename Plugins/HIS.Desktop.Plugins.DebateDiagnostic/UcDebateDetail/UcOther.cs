using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTab;
using DevExpress.XtraTab.Buttons;
using DevExpress.XtraTab.ViewInfo;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.ModuleExt;
using HIS.Desktop.Plugins.DebateDiagnostic.ADO;
using HIS.Desktop.Plugins.DebateDiagnostic.Base;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.Controls.ValidationRule;
using Inventec.Desktop.Common.LibraryMessage;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.CustomControl;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace HIS.Desktop.Plugins.DebateDiagnostic.UcDebateDetail
{
	public class UcOther : UserControl
	{
		private int popupHeight = 400;

		private int positionHandleControl = -1;

		private bool IsOther;

		private long TreatmentId;

		private long RoomId;

		private long RoomTypeId;

		private HIS_SERVICE hisService;

		private List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO> currentMedicineTypeAlls;

		private List<ActiveIngredientADO> currentActiveIngredientAlls;

		internal Module modules;

		private bool isShowContainerMediMaty = false;

		private bool isShowContainerMediMatyForChoose = false;

		private bool isShow = true;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private MemoEdit txtUserManual;

		private DateEdit dtTimeUse;

		private TextEdit txtConcena;

		private MemoEdit txtRequestContent;

		private TextEdit txtUseForm;

		private TextEdit txtMedicineName;

		private LayoutControlItem LciMedicineName;

		private LayoutControlItem LciConcena;

		private LayoutControlItem LciUseForm;

		private LayoutControlItem LciTimeUse;

		private LayoutControlItem LciUserManual;

		private LayoutControlItem LciRequestContent;

		private GroupBox groupBox1;

		private LayoutControl layoutControl2;

		private LayoutControlGroup layoutControlGroup2;

		private GroupBox groupBoxTrackings;

		private LayoutControl layoutControl3;

		private MemoEdit txtTreatmentTracking;

		private MemoEdit txtBeforeDiagnostic;

		private MemoEdit txtHospitalizationState;

		private MemoEdit txtPathologicalHistory;

		private LayoutControlGroup layoutControlGroup3;

		private LayoutControlItem layoutControlItem2;

		private LayoutControlItem layoutControlItem8;

		private DXValidationProvider dxValidationProvider1;

		private LayoutControlItem lciForcboMedcineType;

		private LayoutControlItem lciForcboActiveIngredient;

		internal PopupControlContainer popupControlContainerMedicineType;

		internal CustomGridControlWithFilterMultiColumn gridControlContainerMedicineType;

		internal CustomGridViewWithFilterMultiColumn gridViewContainerMedicineType;

		private ImageCollection imageCollection1;

		private BarManager barManager1;

		private Bar bar1;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		internal PopupControlContainer popupControlContainerActiveIngredient;

		internal CustomGridControlWithFilterMultiColumn gridControlContainerActiveIngredient;

		internal CustomGridViewWithFilterMultiColumn gridViewContainerActiveIngredient;

		private ButtonEdit cboMedcineType;

		private ButtonEdit cboActiveIngredient;

		private TextEdit txtServiceName;

		private TextEdit txtServiceCode;

		private LayoutControlItem LciServiceCode;

		private XtraTabControl xtraTabControl1;

		private XtraTabPage xtraTabPage1;

		private XtraTabPage xtraTabPage2;

		private XtraTabPage xtraTabPage3;

		private XtraTabPage xtraTabPage4;

		private LayoutControlItem layoutControlItem1;

		private XtraTabControl xtraTabControl2;

		private XtraTabPage xtraTabPage5;

		private MemoEdit memoEdit1;

		private XtraTabPage xtraTabPage6;

		private XtraTabPage xtraTabPage7;

		private XtraTabPage xtraTabPage8;

		private XtraTabPage xtraTabPage9;

		private LayoutControlItem layoutControlItem3;

		private MemoEdit txtDiscussion;

		private MemoEdit txtDiagnostic;

		private MemoEdit txtTreatmentMethod;

		private MemoEdit txtCareMethod;

		private MemoEdit txtConclusion;

		private DXValidationProvider dxValidationProviderControl;

		private SimpleButton btnChonKQ;

		private MemoEdit txtKetQuaCLS;

		private LayoutControlItem lciChonKQ;

		private EmptySpaceItem emptySpaceItem1;

		private LayoutControlItem lciKetQuaCLS;

		private SimpleButton btnChonKQ2;

		private MemoEdit txtKetQuaCLS2;

		private LayoutControlItem lciKetQuaCLS2;

		private LayoutControlItem lciChonKQ2;

		private EmptySpaceItem emptySpaceItem2;

		private LayoutControlItem LciServiceName;

		public async Task InitPopupMedicineType()
		{
			try
			{
				Action myaction = delegate
				{
					List<V_HIS_MEDICINE_TYPE> list = BackendDataWorker.Get<V_HIS_MEDICINE_TYPE>();
					if (currentMedicineTypeAlls == null || currentMedicineTypeAlls.Count > 0)
					{
						currentMedicineTypeAlls = ((list != null) ? (from o in list
							where o.IS_ACTIVE == 1
                                                                     select new HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO(o)).ToList() : null);
					}
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				popupHeight = ((currentMedicineTypeAlls != null && currentMedicineTypeAlls.Count > 15) ? 400 : 200);
				((BaseView)(object)gridViewContainerMedicineType).BeginUpdate();
				((ColumnView)(object)gridViewContainerMedicineType).Columns.Clear();
				popupControlContainerMedicineType.Width = 450;
				popupControlContainerMedicineType.Height = popupHeight;
				int columnIndex = 1;
				AddFieldColumnIntoComboExt(gridViewContainerMedicineType, "IsChecked", " ", 30, columnIndex++, true, null, true);
				AddFieldColumnIntoComboExt(gridViewContainerMedicineType, "MEDICINE_TYPE_CODE", "Mã", 90, columnIndex++, true);
				AddFieldColumnIntoComboExt(gridViewContainerMedicineType, "MEDICINE_TYPE_NAME", "Tên", 270, columnIndex, true);
				((BaseView)(object)gridViewContainerMedicineType).GridControl.DataSource = currentMedicineTypeAlls;
				((BaseView)(object)gridViewContainerMedicineType).EndUpdate();
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				((BaseView)(object)gridViewContainerMedicineType).EndUpdate();
				LogSystem.Error(ex2);
			}
		}

		public async Task InitPopupActiveIngredient()
		{
			try
			{
				Action myaction = delegate
				{
					List<HIS_ACTIVE_INGREDIENT> list = BackendDataWorker.Get<HIS_ACTIVE_INGREDIENT>();
					if (currentActiveIngredientAlls == null || currentActiveIngredientAlls.Count > 0)
					{
						currentActiveIngredientAlls = ((list != null) ? (from o in list
							where o.IS_ACTIVE == 1
							select new ActiveIngredientADO(o)).ToList() : null);
					}
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				popupHeight = ((currentActiveIngredientAlls != null && currentActiveIngredientAlls.Count > 15) ? 400 : 200);
				((BaseView)(object)gridViewContainerActiveIngredient).BeginUpdate();
				((ColumnView)(object)gridViewContainerActiveIngredient).Columns.Clear();
				popupControlContainerActiveIngredient.Width = 450;
				popupControlContainerActiveIngredient.Height = popupHeight;
				int columnIndex = 1;
				AddFieldColumnIntoComboExt(gridViewContainerActiveIngredient, "IsChecked", " ", 30, columnIndex++, true, null, true);
				AddFieldColumnIntoComboExt(gridViewContainerActiveIngredient, "ACTIVE_INGREDIENT_CODE", "Mã", 90, columnIndex++, true);
				AddFieldColumnIntoComboExt(gridViewContainerActiveIngredient, "ACTIVE_INGREDIENT_NAME", "Tên", 270, columnIndex, true);
				((BaseView)(object)gridViewContainerActiveIngredient).GridControl.DataSource = currentActiveIngredientAlls;
				((BaseView)(object)gridViewContainerActiveIngredient).EndUpdate();
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				((BaseView)(object)gridViewContainerActiveIngredient).EndUpdate();
				LogSystem.Error(ex2);
			}
		}

		private void AddFieldColumnIntoComboExt(CustomGridViewWithFilterMultiColumn gridView, string FieldName, string Caption, int Width, int VisibleIndex, bool FixedWidth, UnboundColumnType? UnboundType = null, bool allowEdit = false)
		{
			GridColumn gridColumn = new GridColumn();
			gridColumn.FieldName = FieldName;
			gridColumn.Caption = Caption;
			gridColumn.Width = Width;
			gridColumn.VisibleIndex = VisibleIndex;
			gridColumn.OptionsColumn.FixedWidth = FixedWidth;
			if (UnboundType.HasValue)
			{
				gridColumn.UnboundType = UnboundType.Value;
			}
			gridColumn.OptionsColumn.AllowEdit = allowEdit;
			if (FieldName == "IsChecked")
			{
				gridColumn.ColumnEdit = GenerateRepositoryItemCheckEdit();
				gridColumn.OptionsColumn.AllowSort = DefaultBoolean.False;
				gridColumn.OptionsFilter.AllowFilter = false;
				gridColumn.OptionsFilter.AllowAutoFilter = false;
				gridColumn.OptionsColumn.AllowEdit = false;
			}
			((ColumnView)(object)gridView).Columns.Add(gridColumn);
		}

		private RepositoryItemCheckEdit GenerateRepositoryItemCheckEdit()
		{
			RepositoryItemCheckEdit repositoryItemCheckEdit = new RepositoryItemCheckEdit();
			repositoryItemCheckEdit.NullStyle = StyleIndeterminate.Unchecked;
			return repositoryItemCheckEdit;
		}

		public UcOther(long treatmentId, long roomId, long roomTypeId, bool isOther, Module modules)
		{
			InitializeComponent();
			TreatmentId = treatmentId;
			RoomId = roomId;
			RoomTypeId = roomTypeId;
			IsOther = isOther;
			this.modules = modules;
		}

		public UcOther(long treatmentId, long roomId, long roomTypeId, bool isOther, HIS_SERVICE _hisService, Module modules)
		{
			InitializeComponent();
			TreatmentId = treatmentId;
			RoomId = roomId;
			RoomTypeId = roomTypeId;
			IsOther = isOther;
			hisService = _hisService;
			this.modules = modules;
		}

		private void UcOther_Load(object sender, EventArgs e)
		{
			try
			{
				InitPopupMedicineType();
				InitPopupActiveIngredient();
				VisibilityControl();
				ValidationControl();
				UpdateCustomHeaderButtons(xtraTabControl1.SelectedTabPage);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadDataDebateDiagnostic(HIS_DEBATE hisDebate)
		{
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0586: Expected O, but got Unknown
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Expected O, but got Unknown
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				List<long> arrMetyIds = null;
				if (!string.IsNullOrEmpty(hisDebate.MEDICINE_TYPE_IDS))
				{
					string[] array = hisDebate.MEDICINE_TYPE_IDS.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
					arrMetyIds = ((array != null && array.Count() > 0) ? (from o in array
						where Parse.ToInt64(o) > 0
						select Parse.ToInt64(o)).ToList() : null);
					if (arrMetyIds != null && arrMetyIds.Count > 0)
					{
						if (currentMedicineTypeAlls == null || currentMedicineTypeAlls.Count == 0)
						{
							List<V_HIS_MEDICINE_TYPE> list = BackendDataWorker.Get<V_HIS_MEDICINE_TYPE>();
							currentMedicineTypeAlls = ((list != null) ? (from o in list
								where o.IS_ACTIVE == 1
                                                                         select new HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO(o)).ToList() : null);
						}
						isShowContainerMediMatyForChoose = true;
						if (currentMedicineTypeAlls != null && currentMedicineTypeAlls.Count > 0)
						{
                            currentMedicineTypeAlls.ForEach(delegate(HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o)
							{
								o.IsChecked = arrMetyIds.Contains(((V_HIS_MEDICINE_TYPE)o).ID);
							});
							ProcessDisplayMedicineTypeWithData();
						}
					}
				}
				if (!string.IsNullOrEmpty(hisDebate.ACTIVE_INGREDIENT_IDS))
				{
					string[] array2 = hisDebate.ACTIVE_INGREDIENT_IDS.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
					List<long> arrAcIngrIds = ((array2 != null && array2.Count() > 0) ? (from o in array2
						where Parse.ToInt64(o) > 0
						select Parse.ToInt64(o)).ToList() : null);
					if (arrAcIngrIds != null && arrAcIngrIds.Count > 0)
					{
						if (currentActiveIngredientAlls == null || currentActiveIngredientAlls.Count == 0)
						{
							List<HIS_ACTIVE_INGREDIENT> list2 = BackendDataWorker.Get<HIS_ACTIVE_INGREDIENT>();
							currentActiveIngredientAlls = ((list2 != null) ? (from o in list2
								where o.IS_ACTIVE == 1
								select new ActiveIngredientADO(o)).ToList() : null);
						}
						if (currentActiveIngredientAlls != null && currentActiveIngredientAlls.Count > 0)
						{
							currentActiveIngredientAlls.ForEach(delegate(ActiveIngredientADO o)
							{
								o.IsChecked = arrAcIngrIds.Contains(((HIS_ACTIVE_INGREDIENT)o).ID);
							});
							ProcessDisplayActiveIngredientWithData();
						}
					}
				}
				if ((arrMetyIds != null && arrMetyIds.Count == 1) || hisDebate.ID > 0)
				{
					txtMedicineName.Text = hisDebate.MEDICINE_TYPE_NAME;
					txtConcena.Text = hisDebate.MEDICINE_CONCENTRA;
					txtUserManual.Text = hisDebate.MEDICINE_TUTORIAL;
					txtUseForm.Text = hisDebate.MEDICINE_USE_FORM_NAME;
					dtTimeUse.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(hisDebate.MEDICINE_USE_TIME.GetValueOrDefault());
				}
				txtDiscussion.EditValue = hisDebate.DISCUSSION;
				txtRequestContent.Text = hisDebate.REQUEST_CONTENT;
				txtPathologicalHistory.Text = hisDebate.PATHOLOGICAL_HISTORY;
				txtHospitalizationState.Text = hisDebate.HOSPITALIZATION_STATE;
				txtBeforeDiagnostic.Text = hisDebate.BEFORE_DIAGNOSTIC;
				txtTreatmentTracking.Text = hisDebate.TREATMENT_TRACKING;
				txtDiagnostic.Text = hisDebate.DIAGNOSTIC;
				txtTreatmentMethod.Text = hisDebate.TREATMENT_METHOD;
				txtCareMethod.Text = hisDebate.CARE_METHOD;
				txtConclusion.Text = hisDebate.CONCLUSION;
				if (IsOther)
				{
					txtKetQuaCLS.Text = hisDebate.SUBCLINICAL_PROCESSES;
				}
				else
				{
					txtKetQuaCLS2.Text = hisDebate.SUBCLINICAL_PROCESSES;
				}
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<HIS_DEBATE>((Expression<Func<HIS_DEBATE>>)(() => hisDebate)), (object)hisDebate));
				if (hisDebate.SERVICE_ID.HasValue)
				{
					HisServiceFilter val = new HisServiceFilter();
					((FilterBase)val).ID = hisDebate.SERVICE_ID;
					List<HIS_SERVICE> list3 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_SERVICE>>("api/HisService/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
					if (list3 != null && list3.Count() > 0)
					{
						txtServiceCode.Text = list3.FirstOrDefault().SERVICE_CODE;
						txtServiceName.Text = list3.FirstOrDefault().SERVICE_NAME;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDataDebateDiagnostic(HIS_SERVICE_REQ hisDebate)
		{
			try
			{
				txtPathologicalHistory.Text = hisDebate.PATHOLOGICAL_HISTORY;
				string text = hisDebate.FULL_EXAM + "\r\n" + hisDebate.PART_EXAM + "\r\n" + hisDebate.SUBCLINICAL;
				txtHospitalizationState.Text = text.Trim();
				txtTreatmentTracking.Text = hisDebate.PATHOLOGICAL_PROCESS;
				txtTreatmentMethod.Text = hisDebate.TREATMENT_INSTRUCTION;
				txtConclusion.Text = hisDebate.NEXT_TREATMENT_INSTRUCTION;
				if (!string.IsNullOrEmpty(hisDebate.ICD_CODE))
				{
					HIS_ICD val = GlobalStore.HisIcds.Where((HIS_ICD o) => o.ICD_CODE == hisDebate.ICD_CODE).FirstOrDefault();
					txtBeforeDiagnostic.Text = val.ICD_NAME;
					txtDiagnostic.Text = val.ICD_NAME;
				}
				else
				{
					txtBeforeDiagnostic.Text = "";
					txtDiagnostic.Text = "";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void VisibilityControl()
		{
			try
			{
				if (IsOther)
				{
					lciForcboActiveIngredient.Visibility = LayoutVisibility.Never;
					lciForcboMedcineType.Visibility = LayoutVisibility.Never;
					LciMedicineName.Visibility = LayoutVisibility.Never;
					LciConcena.Visibility = LayoutVisibility.Never;
					LciUseForm.Visibility = LayoutVisibility.Never;
					LciTimeUse.Visibility = LayoutVisibility.Never;
					LciUserManual.Visibility = LayoutVisibility.Never;
					LciRequestContent.Visibility = LayoutVisibility.Always;
					LciServiceCode.Visibility = LayoutVisibility.Always;
					LciServiceName.Visibility = LayoutVisibility.Always;
					lciKetQuaCLS.Visibility = LayoutVisibility.Always;
					lciChonKQ.Visibility = LayoutVisibility.Always;
					emptySpaceItem1.Visibility = LayoutVisibility.Always;
					lciKetQuaCLS2.Visibility = LayoutVisibility.Never;
					lciChonKQ2.Visibility = LayoutVisibility.Never;
					emptySpaceItem2.Visibility = LayoutVisibility.Never;
				}
				else
				{
					lciForcboActiveIngredient.Visibility = LayoutVisibility.Always;
					lciForcboMedcineType.Visibility = LayoutVisibility.Always;
					LciMedicineName.Visibility = LayoutVisibility.Always;
					LciConcena.Visibility = LayoutVisibility.Always;
					LciUseForm.Visibility = LayoutVisibility.Always;
					LciTimeUse.Visibility = LayoutVisibility.Always;
					LciUserManual.Visibility = LayoutVisibility.Always;
					LciRequestContent.Visibility = LayoutVisibility.Never;
					LciServiceCode.Visibility = LayoutVisibility.Never;
					LciServiceName.Visibility = LayoutVisibility.Never;
					lciKetQuaCLS.Visibility = LayoutVisibility.Never;
					lciChonKQ.Visibility = LayoutVisibility.Never;
					emptySpaceItem1.Visibility = LayoutVisibility.Never;
					lciKetQuaCLS2.Visibility = LayoutVisibility.Always;
					lciChonKQ2.Visibility = LayoutVisibility.Always;
					emptySpaceItem2.Visibility = LayoutVisibility.Always;
				}
				if (hisService != null)
				{
					txtServiceCode.Text = hisService.SERVICE_CODE;
					txtServiceName.Text = hisService.SERVICE_NAME;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidationControl()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			try
			{
				ValidationControlMedicine(txtMedicineName, 500, !IsOther, new IsValidControl(IsValidControlMedicineNameOrActiveGredient));
				ValidationSingleControl(cboActiveIngredient, dxValidationProvider1, "", new IsValidControl(IsValidControlMedicineNameOrActiveGredient));
				ValidationControlMaxLength(txtUseForm, 100, false, dxValidationProvider1);
				ValidationControlMaxLength(txtConcena, 1000, false, dxValidationProvider1);
				ValidationControlMaxLength(txtUserManual, 2000, false, dxValidationProvider1);
				ValidationControlMaxLength(txtRequestContent, 1000, false, dxValidationProvider1);
				ValidationControlMaxLength(txtPathologicalHistory, 4000, false, dxValidationProviderControl);
				ValidationControlMaxLength(txtHospitalizationState, 2000, false, dxValidationProviderControl);
				ValidationControlMaxLength(txtBeforeDiagnostic, 2000, false, dxValidationProviderControl);
				ValidationControlMaxLength(txtTreatmentTracking, 4000, false, dxValidationProviderControl);
				ValidationControlMaxLength(txtDiscussion, 2000, false, dxValidationProviderControl);
				ValidationControlMaxLength(txtDiagnostic, 2000, false, dxValidationProviderControl);
				ValidationControlMaxLength(txtTreatmentMethod, 2000, false, dxValidationProviderControl);
				ValidationControlMaxLength(txtCareMethod, 2000, false, dxValidationProviderControl);
				ValidationControlMaxLength(txtConclusion, 2000, false, dxValidationProviderControl);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void WarningValidMessage()
		{
			try
			{
				IList<Control> invalidControls = dxValidationProviderControl.GetInvalidControls();
				List<string> list = new List<string>();
				if (invalidControls == null || invalidControls.Count <= 0)
				{
					return;
				}
				List<MemoEdit> list2 = invalidControls.OfType<MemoEdit>().ToList();
				if (list2 != null && list2.Count > 0)
				{
					foreach (MemoEdit item in list2)
					{
						if (item.Name == txtPathologicalHistory.Name)
						{
							list.Add(xtraTabPage1.Text);
						}
						else if (item.Name == txtHospitalizationState.Name)
						{
							list.Add(xtraTabPage2.Text);
						}
						else if (item.Name == txtBeforeDiagnostic.Name)
						{
							list.Add(xtraTabPage3.Text);
						}
						else if (item.Name == txtTreatmentTracking.Name)
						{
							list.Add(xtraTabPage4.Text);
						}
						else if (item.Name == txtDiscussion.Name)
						{
							list.Add(xtraTabPage5.Text);
						}
						else if (item.Name == txtDiagnostic.Name)
						{
							list.Add(xtraTabPage6.Text);
						}
						else if (item.Name == txtTreatmentMethod.Name)
						{
							list.Add(xtraTabPage7.Text);
						}
						else if (item.Name == txtCareMethod.Name)
						{
							list.Add(xtraTabPage8.Text);
						}
						else if (item.Name == txtConclusion.Name)
						{
							list.Add(xtraTabPage9.Text);
						}
					}
				}
				string text = string.Join(", ", list);
				if (!string.IsNullOrEmpty(text))
				{
					XtraMessageBox.Show(text + " vượt quá ký tự cho phép.", Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage((Inventec.Desktop.Common.LibraryMessage.Message.Enum)8), MessageBoxButtons.OK);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private bool IsValidControlMedicineNameOrActiveGredient()
		{
			bool result = true;
			try
			{
				List<ActiveIngredientADO> list = ((currentActiveIngredientAlls != null && currentActiveIngredientAlls.Count > 0) ? currentActiveIngredientAlls.Where((ActiveIngredientADO o) => o.IsChecked).ToList() : null);
				if (string.IsNullOrEmpty(txtMedicineName.Text.Trim()) && (list == null || list.Count == 0) && !IsOther)
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private void ValidationSingleControl(Control control, DXValidationProvider dxValidationProviderEditor, string messageErr, IsValidControl isValidControl)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			try
			{
				ControlEditValidationRule val = new ControlEditValidationRule();
				val.editor = control;
				if (isValidControl != null)
				{
					val.isUseOnlyCustomValidControl = true;
					val.isValidControl = isValidControl;
				}
				if (!string.IsNullOrEmpty(messageErr))
				{
					((ValidationRuleBase)(object)val).ErrorText = messageErr;
				}
				else
				{
					((ValidationRuleBase)(object)val).ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage((Inventec.Desktop.Common.LibraryMessage.Message.Enum)64);
				}
				((ValidationRuleBase)(object)val).ErrorType = ErrorType.Warning;
				dxValidationProviderEditor.SetValidationRule(control, (ValidationRuleBase)(object)val);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidationControlMedicine(BaseEdit control, int? maxLength, bool IsRequired, IsValidControl validEditorReference)
		{
			try
			{
				CustomControlValidationRule customControlValidationRule = new CustomControlValidationRule();
				customControlValidationRule.editor = control;
				customControlValidationRule.messageError = "Chọn loại là \"Hội chẩn thuốc\" bắt buộc 1 trong các trường sau phải có thông tin: Tên thuốc, Hoạt chất";
				customControlValidationRule.validEditorReference = validEditorReference;
				customControlValidationRule.maxLength = maxLength;
				customControlValidationRule.IsRequired = IsRequired;
				int? num = maxLength;
				customControlValidationRule.ErrorText = "Nhập quá kí tự cho phép [" + num + "]";
				customControlValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(control, customControlValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidationControlMaxLength(BaseEdit control, int? maxLength, bool IsRequired, DXValidationProvider dxValidationProviderEditor)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			try
			{
				ControlMaxLengthValidationRule val = new ControlMaxLengthValidationRule();
				val.editor = control;
				val.maxLength = maxLength;
				val.IsRequired = IsRequired;
				int? num = maxLength;
				((ValidationRuleBase)(object)val).ErrorText = "Nhập quá kí tự cho phép [" + num + "]";
				((ValidationRuleBase)(object)val).ErrorType = ErrorType.Warning;
				dxValidationProviderEditor.SetValidationRule(control, (ValidationRuleBase)(object)val);
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
						baseEdit.Focus();
						baseEdit.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtMedicineName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtConcena.SelectAll();
					txtConcena.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtConcena_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtUseForm.SelectAll();
					txtUseForm.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtUseForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtTimeUse.SelectAll();
					dtTimeUse.ShowPopup();
					dtTimeUse.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtTimeUse_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtUserManual.SelectAll();
					txtUserManual.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtUserManual_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtPathologicalHistory.Focus();
					txtPathologicalHistory.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtRequestContent_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtPathologicalHistory.Focus();
					txtPathologicalHistory.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal bool ValidControl()
		{
			bool flag = false;
			try
			{
				positionHandleControl = -1;
				flag = !dxValidationProvider1.Validate() || !dxValidationProviderControl.Validate();
				WarningValidMessage();
			}
			catch (Exception ex)
			{
				flag = true;
				LogSystem.Error(ex);
			}
			return flag;
		}

		internal void DisableControlItem()
		{
			try
			{
				txtPathologicalHistory.ReadOnly = true;
				txtBeforeDiagnostic.ReadOnly = true;
				txtHospitalizationState.ReadOnly = true;
				txtTreatmentTracking.ReadOnly = true;
				txtDiagnostic.ReadOnly = true;
				txtCareMethod.ReadOnly = true;
				txtTreatmentMethod.ReadOnly = true;
				txtConclusion.ReadOnly = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void GetData(ref HIS_DEBATE saveData)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			try
			{
				if (saveData == null)
				{
					saveData = new HIS_DEBATE();
				}
                List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO> list = ((currentMedicineTypeAlls != null && currentMedicineTypeAlls.Count > 0) ? currentMedicineTypeAlls.Where((HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o) => o.IsChecked).ToList() : null);
				if (list != null && list.Count > 0)
				{
                    saveData.MEDICINE_TYPE_IDS = string.Join(",", list.Select((HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o) => ((V_HIS_MEDICINE_TYPE)o).ID));
				}
				List<ActiveIngredientADO> list2 = ((currentActiveIngredientAlls != null && currentActiveIngredientAlls.Count > 0) ? currentActiveIngredientAlls.Where((ActiveIngredientADO o) => o.IsChecked).ToList() : null);
				if (list2 != null && list2.Count > 0)
				{
					saveData.ACTIVE_INGREDIENT_IDS = string.Join(",", list2.Select((ActiveIngredientADO o) => ((HIS_ACTIVE_INGREDIENT)o).ID));
				}
				saveData.MEDICINE_TYPE_NAME = txtMedicineName.Text.Trim();
				saveData.MEDICINE_CONCENTRA = txtConcena.Text.Trim();
				saveData.MEDICINE_USE_FORM_NAME = txtUseForm.Text.Trim();
				saveData.MEDICINE_TUTORIAL = txtUserManual.Text.Trim();
				saveData.DISCUSSION = txtDiscussion.Text.Trim();
				saveData.BEFORE_DIAGNOSTIC = txtBeforeDiagnostic.Text.Trim();
				saveData.CARE_METHOD = txtCareMethod.Text.Trim();
				saveData.CONCLUSION = txtConclusion.Text.Trim();
				saveData.DIAGNOSTIC = txtDiagnostic.Text.Trim();
				saveData.HOSPITALIZATION_STATE = txtHospitalizationState.Text.Trim();
				saveData.PATHOLOGICAL_HISTORY = txtPathologicalHistory.Text.Trim();
				saveData.REQUEST_CONTENT = txtRequestContent.Text.Trim();
				saveData.TREATMENT_METHOD = txtTreatmentMethod.Text.Trim();
				saveData.TREATMENT_TRACKING = txtTreatmentTracking.Text.Trim();
				if (IsOther)
				{
					saveData.SUBCLINICAL_PROCESSES = txtKetQuaCLS.Text;
				}
				else
				{
					saveData.SUBCLINICAL_PROCESSES = txtKetQuaCLS2.Text;
				}
				if (dtTimeUse.EditValue != null && dtTimeUse.DateTime != DateTime.MinValue)
				{
					saveData.MEDICINE_USE_TIME = Parse.ToInt64(Inventec.Common.TypeConvert.Parse.ToDateTime((dtTimeUse.EditValue ?? "").ToString()).ToString("yyyyMMddHHmm") + "00");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void SetData(object data)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			try
			{
				if (data != null)
				{
					if (data.GetType() == typeof(HIS_DEBATE))
					{
						LoadDataDebateDiagnostic((HIS_DEBATE)data);
					}
					else if (data.GetType() == typeof(HIS_SERVICE_REQ))
					{
						LoadDataDebateDiagnostic((HIS_SERVICE_REQ)data);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void SetContent(string data)
		{
			try
			{
				if (!string.IsNullOrEmpty(data))
				{
					txtDiscussion.Text = ((!string.IsNullOrEmpty(txtDiscussion.Text)) ? (txtDiscussion.Text + "\r\n" + data) : data);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void SetDataMedicine(HIS_DEBATE data)
		{
			try
			{
				LogSystem.Debug("SetDataMedicine.1");
				List<long> arrMetyIds = null;
				if (data.MEDICINE_TYPE_IDS != null)
				{
					string[] array = data.MEDICINE_TYPE_IDS.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
					arrMetyIds = ((array != null && array.Count() > 0) ? (from o in array
						where Parse.ToInt64(o) > 0
						select Parse.ToInt64(o)).ToList() : null);
					arrMetyIds = ((arrMetyIds != null && arrMetyIds.Count > 0) ? arrMetyIds.Distinct().ToList() : null);
					if (arrMetyIds != null && arrMetyIds.Count > 0)
					{
						isShowContainerMediMatyForChoose = true;
                        currentMedicineTypeAlls.ForEach(delegate(HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o)
						{
							o.IsChecked = arrMetyIds.Contains(((V_HIS_MEDICINE_TYPE)o).ID);
						});
						ProcessDisplayMedicineTypeWithData();
					}
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<List<long>>((Expression<Func<List<long>>>)(() => arrMetyIds)), (object)arrMetyIds));
				}
				if (data.ACTIVE_INGREDIENT_IDS != null)
				{
					string[] array2 = data.ACTIVE_INGREDIENT_IDS.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
					List<long> arrAcIngrIds = ((array2 != null && array2.Count() > 0) ? (from o in array2
						where Parse.ToInt64(o) > 0
						select Parse.ToInt64(o)).ToList() : null);
					if (arrAcIngrIds != null && arrAcIngrIds.Count > 0 && currentActiveIngredientAlls != null && currentActiveIngredientAlls.Count > 0)
					{
						currentActiveIngredientAlls.ForEach(delegate(ActiveIngredientADO o)
						{
							o.IsChecked = arrAcIngrIds.Contains(((HIS_ACTIVE_INGREDIENT)o).ID);
						});
						ProcessDisplayActiveIngredientWithData();
					}
				}
				if ((arrMetyIds != null && arrMetyIds.Count == 1) || data.ID > 0)
				{
					txtMedicineName.EditValue = data.MEDICINE_TYPE_NAME;
					txtConcena.EditValue = data.MEDICINE_CONCENTRA;
					txtUserManual.EditValue = data.MEDICINE_TUTORIAL;
					txtUseForm.EditValue = data.MEDICINE_USE_FORM_NAME;
					dtTimeUse.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(data.MEDICINE_USE_TIME.GetValueOrDefault());
				}
				LogSystem.Debug("SetDataMedicine.2");
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void MedcineTypeEditValueChanged()
		{
			try
			{
                List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO> currentMedicineTypeSelecteds = currentMedicineTypeAlls.Where((HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o) => o.IsChecked).ToList();
				if (currentMedicineTypeSelecteds != null && currentMedicineTypeSelecteds.Count > 0 && currentActiveIngredientAlls != null && currentActiveIngredientAlls.Count > 0)
				{
                    txtMedicineName.Text = string.Join(",", currentMedicineTypeSelecteds.Select((HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o) => ((V_HIS_MEDICINE_TYPE)o).MEDICINE_TYPE_NAME));
					txtConcena.EditValue = "";
					txtUserManual.EditValue = "";
					txtUseForm.EditValue = "";
					dtTimeUse.EditValue = null;
					List<V_HIS_MEDICINE_TYPE_ACIN> list = BackendDataWorker.Get<V_HIS_MEDICINE_TYPE_ACIN>();
					List<ActiveIngredientADO> list2 = null;
					List<V_HIS_MEDICINE_TYPE_ACIN> metyAcinss = ((list != null) ? (from o in list
                                                                                   where currentMedicineTypeSelecteds.Exists((HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO t) => ((V_HIS_MEDICINE_TYPE)t).ID == o.MEDICINE_TYPE_ID)
						orderby o.ACTIVE_INGREDIENT_ID descending
						select o).ToList() : null);
					if (metyAcinss != null && metyAcinss.Count > 0)
					{
						list2 = ((currentActiveIngredientAlls != null) ? currentActiveIngredientAlls.Where((ActiveIngredientADO o) => metyAcinss.Exists((V_HIS_MEDICINE_TYPE_ACIN t) => t.ACTIVE_INGREDIENT_ID == ((HIS_ACTIVE_INGREDIENT)o).ID)).ToList() : null);
					}
					currentActiveIngredientAlls.ForEach(delegate(ActiveIngredientADO o)
					{
						o.IsChecked = metyAcinss != null && metyAcinss.Count > 0 && metyAcinss.Exists((V_HIS_MEDICINE_TYPE_ACIN t) => t.ACTIVE_INGREDIENT_ID == ((HIS_ACTIVE_INGREDIENT)o).ID);
					});
					cboActiveIngredient.Properties.Buttons[1].Visible = list2 != null && list2.Count > 0;
					ProcessDisplayActiveIngredientWithData();
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<List<V_HIS_MEDICINE_TYPE_ACIN>>((Expression<Func<List<V_HIS_MEDICINE_TYPE_ACIN>>>)(() => metyAcinss)), (object)metyAcinss));
				}
				else
				{
					txtMedicineName.EditValue = "";
					txtConcena.EditValue = "";
					txtUserManual.EditValue = "";
					txtUseForm.EditValue = "";
					dtTimeUse.EditValue = null;
					cboActiveIngredient.Text = "";
					currentActiveIngredientAlls.ForEach(delegate(ActiveIngredientADO o)
					{
						o.IsChecked = false;
					});
					cboActiveIngredient.Properties.Buttons[1].Visible = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboMedcineType_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.DropDown)
				{
					LogSystem.Debug("cboMedcineType_ButtonClick.1");
					ProcessShowpopupControlContainerMedcineType();
					LogSystem.Debug("cboMedcineType_ButtonClick.2");
				}
				else if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboMedcineType.Text = null;
                    currentMedicineTypeAlls.ForEach(delegate(HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o)
					{
						o.IsChecked = false;
					});
					cboMedcineType.Properties.Buttons[1].Visible = false;
					TextEdit textEdit = txtMedicineName;
					TextEdit textEdit2 = txtConcena;
					TextEdit textEdit3 = txtUseForm;
					string text = (txtUserManual.Text = "");
					string text3 = (textEdit3.Text = text);
					string text5 = (textEdit2.Text = text3);
					textEdit.Text = text5;
					dtTimeUse.EditValue = null;
					cboActiveIngredient.Text = null;
					currentActiveIngredientAlls.ForEach(delegate(ActiveIngredientADO o)
					{
						o.IsChecked = false;
					});
					cboActiveIngredient.Properties.Buttons[1].Visible = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboActiveIngredient_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.DropDown)
				{
					LogSystem.Debug("cboActiveIngredient_ButtonClick.1");
					ProcessShowpopupControlContainerActiveIngredient();
					LogSystem.Debug("cboActiveIngredient_ButtonClick.2");
				}
				else if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboActiveIngredient.Text = null;
					currentActiveIngredientAlls.ForEach(delegate(ActiveIngredientADO o)
					{
						o.IsChecked = false;
					});
					cboActiveIngredient.Properties.Buttons[1].Visible = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboMedcineType_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					cboActiveIngredient.Focus();
					cboActiveIngredient.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboActiveIngredient_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtMedicineName.Focus();
					txtMedicineName.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboMedcineType_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Down)
				{
					LogSystem.Debug("cboMedcineType_KeyDown.1");
					ProcessShowpopupControlContainerMedcineType();
					LogSystem.Debug("cboMedcineType_KeyDown.2");
				}
				else if (e.Control && e.KeyCode == Keys.A)
				{
					cboMedcineType.SelectAll();
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboMedcineType_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(cboMedcineType.Text))
				{
					cboMedcineType.Refresh();
					LogSystem.Debug(LogUtil.TraceData("cboMedcineType.Text", (object)cboMedcineType.Text) + LogUtil.TraceData("isShowContainerMediMatyForChoose", (object)isShowContainerMediMatyForChoose) + LogUtil.TraceData("isShowContainerMediMaty", (object)isShowContainerMediMaty) + LogUtil.TraceData("isShow", (object)isShow));
					if (isShowContainerMediMatyForChoose)
					{
						((ColumnView)(object)gridViewContainerMedicineType).ActiveFilter.Clear();
					}
					else
					{
						if (!isShowContainerMediMaty)
						{
							isShowContainerMediMaty = true;
						}
						((ColumnView)(object)gridViewContainerMedicineType).ActiveFilterString = string.Format("[MEDICINE_TYPE_NAME] Like '%{0}%' OR [MEDICINE_TYPE_CODE] Like '%{0}%'  OR [MEDICINE_TYPE_NAME__UNSIGN] Like '%{0}%'", cboMedcineType.Text);
						((ColumnView)(object)gridViewContainerMedicineType).OptionsFilter.FilterEditorUseMenuForOperandsAndOperators = false;
						((ColumnView)(object)gridViewContainerMedicineType).OptionsFilter.ShowAllTableValuesInCheckedFilterPopup = false;
						((ColumnView)(object)gridViewContainerMedicineType).OptionsFilter.ShowAllTableValuesInFilterPopup = false;
						((ColumnView)(object)gridViewContainerMedicineType).FocusedRowHandle = 0;
						((GridView)(object)gridViewContainerMedicineType).OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
						((GridView)(object)gridViewContainerMedicineType).OptionsFind.HighlightFindResults = true;
						if (isShow)
						{
							ProcessShowpopupControlContainerMedcineType();
							isShow = false;
						}
						cboMedcineType.Focus();
					}
					isShowContainerMediMatyForChoose = false;
				}
				else
				{
					((ColumnView)(object)gridViewContainerMedicineType).ActiveFilter.Clear();
					if (!isShowContainerMediMaty)
					{
						popupControlContainerMedicineType.HidePopup();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboActiveIngredient_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Down)
				{
					LogSystem.Debug("cboActiveIngredient_KeyDown.1");
					ProcessShowpopupControlContainerActiveIngredient();
					LogSystem.Debug("cboActiveIngredient_KeyDown.2");
				}
				else if (e.Control && e.KeyCode == Keys.A)
				{
					cboActiveIngredient.SelectAll();
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboActiveIngredient_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(cboActiveIngredient.Text))
				{
					cboActiveIngredient.Refresh();
					LogSystem.Debug(LogUtil.TraceData("cboActiveIngredient.Text", (object)cboActiveIngredient.Text) + LogUtil.TraceData("isShowContainerMediMatyForChoose", (object)isShowContainerMediMatyForChoose) + LogUtil.TraceData("isShowContainerMediMaty", (object)isShowContainerMediMaty) + LogUtil.TraceData("isShow", (object)isShow));
					if (isShowContainerMediMatyForChoose)
					{
						((ColumnView)(object)gridViewContainerActiveIngredient).ActiveFilter.Clear();
					}
					else
					{
						if (!isShowContainerMediMaty)
						{
							isShowContainerMediMaty = true;
						}
						((ColumnView)(object)gridViewContainerActiveIngredient).ActiveFilterString = string.Format("[ACTIVE_INGREDIENT_NAME] Like '%{0}%' OR [ACTIVE_INGREDIENT_CODE] Like '%{0}%' OR [ACTIVE_INGREDIENT_NAME__UNSIGN] Like '%{0}%'", cboActiveIngredient.Text);
						((ColumnView)(object)gridViewContainerActiveIngredient).OptionsFilter.FilterEditorUseMenuForOperandsAndOperators = false;
						((ColumnView)(object)gridViewContainerActiveIngredient).OptionsFilter.ShowAllTableValuesInCheckedFilterPopup = false;
						((ColumnView)(object)gridViewContainerActiveIngredient).OptionsFilter.ShowAllTableValuesInFilterPopup = false;
						((ColumnView)(object)gridViewContainerActiveIngredient).FocusedRowHandle = 0;
						((GridView)(object)gridViewContainerActiveIngredient).OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
						((GridView)(object)gridViewContainerActiveIngredient).OptionsFind.HighlightFindResults = true;
						if (isShow)
						{
							ProcessShowpopupControlContainerActiveIngredient();
							isShow = false;
						}
						cboActiveIngredient.Focus();
					}
					isShowContainerMediMatyForChoose = false;
				}
				else
				{
					((ColumnView)(object)gridViewContainerActiveIngredient).ActiveFilter.Clear();
					if (!isShowContainerMediMaty)
					{
						popupControlContainerActiveIngredient.HidePopup();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ProcessShowpopupControlContainerActiveIngredient()
		{
			int heightPlus = 0;
			Rectangle bounds = GetClientRectangle(this, ref heightPlus);
			Rectangle bounds2 = GetAllClientRectangle(this, ref heightPlus);
			LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<Rectangle>((Expression<Func<Rectangle>>)(() => bounds)), (object)bounds) + LogUtil.TraceData(LogUtil.GetMemberName<Rectangle>((Expression<Func<Rectangle>>)(() => bounds2)), (object)bounds2) + "|bounds1.Height=" + bounds2.Height + "|popupHeight=" + popupHeight);
			bool flag = false;
			heightPlus = ((bounds2.Height <= 768) ? ((popupHeight != 400) ? ((bounds.Y >= 650) ? (-60) : ((bounds.Y > 410) ? (-60) : ((bounds.Y < 230) ? (-bounds.Y - 27) : (-78)))) : ((bounds.Y >= 650) ? (-125) : ((bounds.Y > 410) ? (-262) : ((bounds.Y < 230) ? (-bounds.Y - 227) : (-276))))) : ((popupHeight != 400) ? ((bounds.Y >= 650) ? (-122) : ((bounds.Y > 410) ? (-60) : ((bounds.Y < 230) ? (-bounds.Y - 25) : (-180)))) : ((bounds.Y >= 650) ? (-327) : ((bounds.Y > 410) ? (-260) : ((bounds.Y < 230) ? (-bounds.Y - 225) : (-608))))));
			Rectangle rectangle = new Rectangle(cboActiveIngredient.Bounds.X + 10, bounds.Y + heightPlus, bounds.Width, bounds.Height);
			LogSystem.Debug(LogUtil.TraceData("buttonBounds", (object)rectangle) + LogUtil.TraceData("heightPlus", (object)heightPlus));
			currentActiveIngredientAlls = ((currentActiveIngredientAlls != null && currentActiveIngredientAlls.Count > 0) ? (from o in currentActiveIngredientAlls
				orderby o.IsChecked descending, ((HIS_ACTIVE_INGREDIENT)o).ACTIVE_INGREDIENT_NAME
				select o).ToList() : null);
			((BaseView)(object)gridViewContainerActiveIngredient).BeginUpdate();
			((BaseView)(object)gridViewContainerActiveIngredient).GridControl.DataSource = currentActiveIngredientAlls;
			((BaseView)(object)gridViewContainerActiveIngredient).EndUpdate();
			popupControlContainerActiveIngredient.ShowPopup(new Point(rectangle.X, rectangle.Bottom));
			((BaseView)(object)gridViewContainerActiveIngredient).Focus();
			((ColumnView)(object)gridViewContainerActiveIngredient).FocusedRowHandle = 0;
		}

		private void ProcessShowpopupControlContainerMedcineType()
		{
			int heightPlus = 0;
			Rectangle bounds = GetClientRectangle(this, ref heightPlus);
			Rectangle bounds2 = GetAllClientRectangle(this, ref heightPlus);
			LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<Rectangle>((Expression<Func<Rectangle>>)(() => bounds)), (object)bounds) + LogUtil.TraceData(LogUtil.GetMemberName<Rectangle>((Expression<Func<Rectangle>>)(() => bounds2)), (object)bounds2) + "|bounds1.Height=" + bounds2.Height + "|popupHeight=" + popupHeight);
			bool flag = false;
			heightPlus = ((bounds2.Height <= 768) ? ((popupHeight != 400) ? ((bounds.Y >= 650) ? (-60) : ((bounds.Y > 410) ? (-60) : ((bounds.Y < 230) ? (-bounds.Y - 27) : (-78)))) : ((bounds.Y >= 650) ? (-125) : ((bounds.Y > 410) ? (-262) : ((bounds.Y < 230) ? (-bounds.Y - 227) : (-276))))) : ((popupHeight != 400) ? ((bounds.Y >= 650) ? (-122) : ((bounds.Y > 410) ? (-60) : ((bounds.Y < 230) ? (-bounds.Y - 25) : (-180)))) : ((bounds.Y >= 650) ? (-327) : ((bounds.Y > 410) ? (-260) : ((bounds.Y < 230) ? (-bounds.Y - 225) : (-608))))));
			Rectangle rectangle = new Rectangle(cboMedcineType.Bounds.X + 10, bounds.Y + heightPlus, bounds.Width, bounds.Height);
			LogSystem.Debug(LogUtil.TraceData("buttonBounds", (object)rectangle) + LogUtil.TraceData("heightPlus", (object)heightPlus));
			currentMedicineTypeAlls = ((currentMedicineTypeAlls != null) ? (from o in currentMedicineTypeAlls
				orderby o.IsChecked descending, ((V_HIS_MEDICINE_TYPE)o).MEDICINE_TYPE_NAME
				select o).ToList() : null);
			((BaseView)(object)gridViewContainerMedicineType).BeginUpdate();
			((BaseView)(object)gridViewContainerMedicineType).GridControl.DataSource = currentMedicineTypeAlls;
			((BaseView)(object)gridViewContainerMedicineType).EndUpdate();
			popupControlContainerMedicineType.ShowPopup(new Point(rectangle.X, rectangle.Bottom));
			((BaseView)(object)gridViewContainerMedicineType).Focus();
			((ColumnView)(object)gridViewContainerMedicineType).FocusedRowHandle = 0;
		}

		private Rectangle GetClientRectangle(Control control, ref int heightPlus)
		{
			Rectangle bounds = default(Rectangle);
			if (control != null)
			{
				bounds = control.Bounds;
				LogSystem.Debug(LogUtil.TraceData("GetClientRectangle:" + LogUtil.GetMemberName<Rectangle>((Expression<Func<Rectangle>>)(() => bounds)), (object)bounds) + "|control.name=" + control.Name + "|bounds.Y=" + bounds.Y);
				if (control.Parent != null && !(control is UserControl))
				{
					heightPlus += bounds.Y;
					return GetClientRectangle(control.Parent, ref heightPlus);
				}
			}
			return bounds;
		}

		private Rectangle GetAllClientRectangle(Control control, ref int heightPlus)
		{
			Rectangle bounds = default(Rectangle);
			if (control != null)
			{
				bounds = control.Bounds;
				LogSystem.Debug(LogUtil.TraceData("GetAllClientRectangle:" + LogUtil.GetMemberName<Rectangle>((Expression<Func<Rectangle>>)(() => bounds)), (object)bounds) + "|control.name=" + control.Name + "|bounds.Y=" + bounds.Y);
				if (control.Parent != null)
				{
					heightPlus += bounds.Y;
					return GetAllClientRectangle(control.Parent, ref heightPlus);
				}
			}
			return bounds;
		}

		private void popupControlContainerMedicineType_CloseUp(object sender, EventArgs e)
		{
			try
			{
                bool flag = currentMedicineTypeAlls.Any((HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o) => o.IsChecked);
				cboMedcineType.Properties.Buttons[1].Visible = flag;
				isShow = true;
				if (flag)
				{
					cboMedcineType.Text = string.Join(", ", from o in currentMedicineTypeAlls
						where o.IsChecked
						select ((V_HIS_MEDICINE_TYPE)o).MEDICINE_TYPE_NAME);
					cboActiveIngredient.Focus();
					cboActiveIngredient.SelectAll();
					return;
				}
				try
				{
					cboActiveIngredient.Focus();
					cboActiveIngredient.SelectAll();
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
					SendKeys.Send("{TAB}");
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		private void popupControlContainerActiveIngredient_CloseUp(object sender, EventArgs e)
		{
			try
			{
				bool flag = currentActiveIngredientAlls.Any((ActiveIngredientADO o) => o.IsChecked);
				cboActiveIngredient.Properties.Buttons[1].Visible = flag;
				isShow = true;
				if (flag)
				{
					cboActiveIngredient.Text = string.Join(", ", from o in currentActiveIngredientAlls
						where o.IsChecked
						select ((HIS_ACTIVE_INGREDIENT)o).ACTIVE_INGREDIENT_NAME);
					txtMedicineName.Focus();
					txtMedicineName.SelectAll();
					return;
				}
				try
				{
					txtMedicineName.Focus();
					txtMedicineName.SelectAll();
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
					SendKeys.Send("{TAB}");
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		private void ProcessDisplayMedicineTypeWithData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
            List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO> list = currentMedicineTypeAlls.Where((HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o) => o.IsChecked).ToList();
			if (list != null && list.Count > 0)
			{
                foreach (HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO item in list)
				{
					if (item != null)
					{
						if (stringBuilder.ToString().Length > 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(((V_HIS_MEDICINE_TYPE)item).MEDICINE_TYPE_NAME);
					}
				}
			}
			cboMedcineType.Properties.Buttons[1].Visible = list != null && list.Count > 0;
			isShowContainerMediMatyForChoose = true;
			cboMedcineType.Text = stringBuilder.ToString();
			MedcineTypeEditValueChanged();
		}

		private void ProcessDisplayActiveIngredientWithData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			List<ActiveIngredientADO> list = currentActiveIngredientAlls.Where((ActiveIngredientADO o) => o.IsChecked).ToList();
			if (list != null && list.Count > 0)
			{
				foreach (ActiveIngredientADO item in list)
				{
					if (item != null)
					{
						if (stringBuilder.ToString().Length > 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(((HIS_ACTIVE_INGREDIENT)item).ACTIVE_INGREDIENT_NAME);
					}
				}
			}
			cboActiveIngredient.Properties.Buttons[1].Visible = list != null && list.Count > 0;
			isShowContainerMediMatyForChoose = true;
			cboActiveIngredient.Text = stringBuilder.ToString();
		}

		private void gridControlContainerMedicineType_Click(object sender, EventArgs e)
		{
			try
			{
                HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO medicineTypeADO = (HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO)((ColumnView)(object)gridViewContainerMedicineType).GetFocusedRow();
				if (medicineTypeADO != null)
				{
					medicineTypeADO.IsChecked = !medicineTypeADO.IsChecked;
					((GridControl)(object)gridControlContainerMedicineType).RefreshDataSource();
					isShowContainerMediMaty = true;
					ProcessDisplayMedicineTypeWithData();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridControlContainerActiveIngredient_Click(object sender, EventArgs e)
		{
			try
			{
				ActiveIngredientADO activeIngredientADO = (ActiveIngredientADO)((ColumnView)(object)gridViewContainerActiveIngredient).GetFocusedRow();
				if (activeIngredientADO != null)
				{
					activeIngredientADO.IsChecked = !activeIngredientADO.IsChecked;
					((GridControl)(object)gridControlContainerActiveIngredient).RefreshDataSource();
					isShowContainerMediMaty = true;
					ProcessDisplayActiveIngredientWithData();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewContainerMedicineType_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				LogSystem.Debug("gridViewContainerMedicineType_KeyDown.1");
				ColumnView columnView = (ColumnView)((GridControl)(object)gridControlContainerMedicineType).FocusedView;
				if (e.KeyCode == Keys.Space)
				{
					if (((BaseView)(object)gridViewContainerMedicineType).IsEditing)
					{
						((BaseView)(object)gridViewContainerMedicineType).CloseEditor();
					}
					if (((ColumnView)(object)gridViewContainerMedicineType).FocusedRowModified)
					{
						((BaseView)(object)gridViewContainerMedicineType).UpdateCurrentRow();
					}
                    HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO rawMety = (HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO)((ColumnView)(object)gridViewContainerMedicineType).GetFocusedRow();
                    LogSystem.Debug("gridViewContainerMedicineType_KeyDown. gridViewContainerMedicineType.FocusedRowHandle=" + columnView.FocusedRowHandle + "|" + LogUtil.TraceData(LogUtil.GetMemberName<HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO>((Expression<Func<HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO>>)(() => rawMety)), (object)rawMety));
					if (rawMety != null && rawMety.IsChecked)
					{
						LogSystem.Debug("gridViewContainerMedicineType_KeyDown.1.1");
						rawMety.IsChecked = false;
						LogSystem.Debug("gridViewContainerMedicineType_KeyDown.1.2");
					}
					else if (rawMety != null)
					{
						LogSystem.Debug("gridViewContainerMedicineType_KeyDown.1.3");
						rawMety.IsChecked = true;
						LogSystem.Debug("gridViewContainerMedicineType_KeyDown.1.4");
					}
					((GridControl)(object)gridControlContainerMedicineType).RefreshDataSource();
					ProcessDisplayMedicineTypeWithData();
				}
				else if (e.KeyCode == Keys.Return)
				{
                    HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO medicineTypeADO = (HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO)((ColumnView)(object)gridViewContainerMedicineType).GetFocusedRow();
                    if (currentMedicineTypeAlls != null && !currentMedicineTypeAlls.Any((HIS.Desktop.Plugins.DebateDiagnostic.ADO.MedicineTypeADO o) => o.IsChecked) && medicineTypeADO != null)
					{
						medicineTypeADO.IsChecked = true;
						((GridControl)(object)gridControlContainerMedicineType).RefreshDataSource();
						ProcessDisplayMedicineTypeWithData();
					}
					isShowContainerMediMaty = false;
					isShowContainerMediMatyForChoose = true;
					popupControlContainerMedicineType.HidePopup();
				}
				LogSystem.Debug("gridViewContainerMedicineType_KeyDown.2");
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridViewContainerMedicineType_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					return;
				}
				GridView gridView = sender as GridView;
				GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.Location);
				if (!(gridHitInfo.Column.FieldName == "IsChecked") || !gridHitInfo.InRowCell)
				{
					return;
				}
				LogSystem.Debug("hi.InRowCell");
				if (!(gridHitInfo.Column.RealColumnEdit.GetType() == typeof(RepositoryItemCheckEdit)))
				{
					return;
				}
				gridView.FocusedRowHandle = gridHitInfo.RowHandle;
				gridView.FocusedColumn = gridHitInfo.Column;
				gridView.ShowEditor();
				CheckEdit checkEdit = gridView.ActiveEditor as CheckEdit;
				CheckEditViewInfo checkEditViewInfo = (CheckEditViewInfo)checkEdit.GetViewInfo();
				Rectangle glyphRect = checkEditViewInfo.CheckInfo.GlyphRect;
				GridViewInfo gridViewInfo = gridView.GetViewInfo() as GridViewInfo;
				Rectangle rectangle = new Rectangle(gridViewInfo.GetGridCellInfo(gridHitInfo).Bounds.X + glyphRect.X, gridViewInfo.GetGridCellInfo(gridHitInfo).Bounds.Y + glyphRect.Y, glyphRect.Width, glyphRect.Height);
				if (!rectangle.Contains(e.Location))
				{
					gridView.CloseEditor();
					if (!gridView.IsCellSelected(gridHitInfo.RowHandle, gridHitInfo.Column))
					{
						gridView.SelectCell(gridHitInfo.RowHandle, gridHitInfo.Column);
					}
					else
					{
						gridView.UnselectCell(gridHitInfo.RowHandle, gridHitInfo.Column);
					}
				}
				else
				{
					checkEdit.Checked = !checkEdit.Checked;
					gridView.CloseEditor();
				}
				((GridControl)(object)gridControlContainerMedicineType).RefreshDataSource();
				ProcessDisplayMedicineTypeWithData();
				(e as DXMouseEventArgs).Handled = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewContainerActiveIngredient_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				LogSystem.Debug("gridViewContainerActiveIngredient_KeyDown.1");
				ColumnView columnView = (ColumnView)((GridControl)(object)gridControlContainerActiveIngredient).FocusedView;
				if (e.KeyCode == Keys.Space)
				{
					if (((BaseView)(object)gridViewContainerActiveIngredient).IsEditing)
					{
						((BaseView)(object)gridViewContainerActiveIngredient).CloseEditor();
					}
					if (((ColumnView)(object)gridViewContainerActiveIngredient).FocusedRowModified)
					{
						((BaseView)(object)gridViewContainerActiveIngredient).UpdateCurrentRow();
					}
					ActiveIngredientADO rawAcgr = (ActiveIngredientADO)((ColumnView)(object)gridViewContainerActiveIngredient).GetFocusedRow();
					LogSystem.Debug("gridViewContainerActiveIngredient_KeyDown. gridViewContainerActiveIngredient.FocusedRowHandle=" + columnView.FocusedRowHandle + "|" + LogUtil.TraceData(LogUtil.GetMemberName<ActiveIngredientADO>((Expression<Func<ActiveIngredientADO>>)(() => rawAcgr)), (object)rawAcgr));
					if (rawAcgr != null && rawAcgr.IsChecked)
					{
						LogSystem.Debug("gridViewContainerActiveIngredient_KeyDown.1.1");
						rawAcgr.IsChecked = false;
						LogSystem.Debug("gridViewContainerActiveIngredient_KeyDown.1.2");
					}
					else if (rawAcgr != null)
					{
						LogSystem.Debug("gridViewContainerActiveIngredient_KeyDown.1.3");
						rawAcgr.IsChecked = true;
						LogSystem.Debug("gridViewContainerActiveIngredient_KeyDown.1.4");
					}
					((GridControl)(object)gridControlContainerActiveIngredient).RefreshDataSource();
					ProcessDisplayActiveIngredientWithData();
				}
				else if (e.KeyCode == Keys.Return)
				{
					ActiveIngredientADO activeIngredientADO = (ActiveIngredientADO)((ColumnView)(object)gridViewContainerActiveIngredient).GetFocusedRow();
					if (currentActiveIngredientAlls != null && !currentActiveIngredientAlls.Any((ActiveIngredientADO o) => o.IsChecked) && activeIngredientADO != null)
					{
						activeIngredientADO.IsChecked = true;
						((GridControl)(object)gridControlContainerActiveIngredient).RefreshDataSource();
						ProcessDisplayActiveIngredientWithData();
					}
					isShowContainerMediMaty = false;
					isShowContainerMediMatyForChoose = true;
					popupControlContainerActiveIngredient.HidePopup();
				}
				LogSystem.Debug("gridViewContainerActiveIngredient_KeyDown.2");
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridViewContainerActiveIngredient_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					return;
				}
				GridView gridView = sender as GridView;
				GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.Location);
				if (!(gridHitInfo.Column.FieldName == "IsChecked") || !gridHitInfo.InRowCell)
				{
					return;
				}
				LogSystem.Debug("hi.InRowCell");
				if (!(gridHitInfo.Column.RealColumnEdit.GetType() == typeof(RepositoryItemCheckEdit)))
				{
					return;
				}
				gridView.FocusedRowHandle = gridHitInfo.RowHandle;
				gridView.FocusedColumn = gridHitInfo.Column;
				gridView.ShowEditor();
				CheckEdit checkEdit = gridView.ActiveEditor as CheckEdit;
				CheckEditViewInfo checkEditViewInfo = (CheckEditViewInfo)checkEdit.GetViewInfo();
				Rectangle glyphRect = checkEditViewInfo.CheckInfo.GlyphRect;
				GridViewInfo gridViewInfo = gridView.GetViewInfo() as GridViewInfo;
				Rectangle rectangle = new Rectangle(gridViewInfo.GetGridCellInfo(gridHitInfo).Bounds.X + glyphRect.X, gridViewInfo.GetGridCellInfo(gridHitInfo).Bounds.Y + glyphRect.Y, glyphRect.Width, glyphRect.Height);
				if (!rectangle.Contains(e.Location))
				{
					gridView.CloseEditor();
					if (!gridView.IsCellSelected(gridHitInfo.RowHandle, gridHitInfo.Column))
					{
						gridView.SelectCell(gridHitInfo.RowHandle, gridHitInfo.Column);
					}
					else
					{
						gridView.UnselectCell(gridHitInfo.RowHandle, gridHitInfo.Column);
					}
				}
				else
				{
					checkEdit.Checked = !checkEdit.Checked;
					gridView.CloseEditor();
				}
				((GridControl)(object)gridControlContainerActiveIngredient).RefreshDataSource();
				ProcessDisplayActiveIngredientWithData();
				(e as DXMouseEventArgs).Handled = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dxValidationProviderControl_ValidationFailed(object sender, ValidationFailedEventArgs e)
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
						baseEdit.Focus();
						baseEdit.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnChonKQ_Click(object sender, EventArgs e)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			try
			{
				List<object> list = new List<object>();
				list.Add(TreatmentId);
				list.Add((object)new DelegateSelectData(SelectDataResult));
				list.Add(true);
				PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.ContentSubclinical", RoomId, RoomTypeId, list);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SelectDataResult(object data)
		{
			try
			{
				LogSystem.Debug(LogUtil.TraceData("SelectDataResult: ", data));
				txtKetQuaCLS.Text = "";
				txtKetQuaCLS2.Text = "";
				if (data == null || !(data is List<ContentSubclinicalADO>))
				{
					return;
				}
				List<ContentSubclinicalADO> source = data as List<ContentSubclinicalADO>;
				List<string> list = new List<string>();
				List<ContentSubclinicalADO> list2 = source.Where((ContentSubclinicalADO o) => o.TDL_SERVICE_TYPE_ID == 2).ToList();
				List<ContentSubclinicalADO> list3 = source.Where((ContentSubclinicalADO o) => o.TDL_SERVICE_TYPE_ID != 2).ToList();
				if (list2 != null && list2.Count > 0)
				{
					List<IGrouping<long, ContentSubclinicalADO>> list4 = (from s in list2
						orderby s.NUM_ORDER
						group s by s.SERVICE_ID).ToList();
					foreach (IGrouping<long, ContentSubclinicalADO> item in list4)
					{
						List<string> list5 = new List<string>();
						List<ContentSubclinicalADO> list6 = item.ToList();
						for (int num = 0; num < list6.Count(); num++)
						{
							if (num == 0)
							{
								if (!string.IsNullOrWhiteSpace(list6[num].TDL_SERVICE_NAME.ToLower()) && !string.IsNullOrWhiteSpace(list6[num].TEST_INDEX_NAME) && list6[num].TDL_SERVICE_NAME.ToLower() == list6[num].TEST_INDEX_NAME.ToLower())
								{
									list5.Add(string.Format("{0}: {1} {2}", list6[num].TEST_INDEX_NAME, list6[num].VALUE, list6[num].SERVICE_UNIT_NAME));
									continue;
								}
								list5.Add(string.Format("{0}: {1} {2} {3} ; ", list6[num].TDL_SERVICE_NAME, list6[num].TEST_INDEX_NAME, list6[num].VALUE, list6[num].SERVICE_UNIT_NAME));
							}
							else
							{
								list5.Add(string.Format("{0} {1} {2}", list6[num].TEST_INDEX_NAME, list6[num].VALUE, list6[num].SERVICE_UNIT_NAME));
							}
						}
						list.Add(string.Join("; ", list5));
					}
				}
				if (list3 != null && list3.Count > 0)
				{
					foreach (ContentSubclinicalADO item2 in list3)
					{
						list.Add(string.Format("{0}:{1}", item2.TDL_SERVICE_NAME, item2.VALUE));
					}
				}
				txtKetQuaCLS.Text = string.Join("\r\n", list);
				txtKetQuaCLS2.Text = string.Join("\r\n", list);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
		{
			try
			{
				UpdateCustomHeaderButtons(e.Page);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void UpdateCustomHeaderButtons(XtraTabPage page)
		{
			if (page == xtraTabPage1 || page == xtraTabPage2 || page == xtraTabPage3)
			{
				xtraTabControl1.CustomHeaderButtons[0].Enabled = false;
			}
			else if (page == xtraTabPage4)
			{
				xtraTabControl1.CustomHeaderButtons[0].Enabled = true;
			}
		}

		private void xtraTabControl1_CustomHeaderButtonClick(object sender, CustomHeaderButtonEventArgs e)
		{
			try
			{
				CustomHeaderButton button = e.Button;
				if (button.Tag != null)
				{
					string text = button.Tag.ToString();
					if (text == "Chondienbien")
					{
						frmDevelopmentCLS frmDevelopmentCLS = new frmDevelopmentCLS(TreatmentId);
						frmDevelopmentCLS.ParentUcOther = this;
						frmDevelopmentCLS.ShowDialog();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void SetTreatmentTrackingText(string text)
		{
			txtTreatmentTracking.Text = text;
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject9 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcOther));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnChonKQ2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtKetQuaCLS2 = new DevExpress.XtraEditors.MemoEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnChonKQ = new DevExpress.XtraEditors.SimpleButton();
            this.txtKetQuaCLS = new DevExpress.XtraEditors.MemoEdit();
            this.txtServiceName = new DevExpress.XtraEditors.TextEdit();
            this.txtServiceCode = new DevExpress.XtraEditors.TextEdit();
            this.popupControlContainerActiveIngredient = new DevExpress.XtraBars.PopupControlContainer();
            this.gridControlContainerActiveIngredient = new Inventec.Desktop.CustomControl.CustomGridControlWithFilterMultiColumn();
            this.gridViewContainerActiveIngredient = new Inventec.Desktop.CustomControl.CustomGridViewWithFilterMultiColumn();
            this.popupControlContainerMedicineType = new DevExpress.XtraBars.PopupControlContainer();
            this.gridControlContainerMedicineType = new Inventec.Desktop.CustomControl.CustomGridControlWithFilterMultiColumn();
            this.gridViewContainerMedicineType = new Inventec.Desktop.CustomControl.CustomGridViewWithFilterMultiColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage5 = new DevExpress.XtraTab.XtraTabPage();
            this.txtDiscussion = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage6 = new DevExpress.XtraTab.XtraTabPage();
            this.txtDiagnostic = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage7 = new DevExpress.XtraTab.XtraTabPage();
            this.txtTreatmentMethod = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage8 = new DevExpress.XtraTab.XtraTabPage();
            this.txtCareMethod = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage9 = new DevExpress.XtraTab.XtraTabPage();
            this.txtConclusion = new DevExpress.XtraEditors.MemoEdit();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.groupBoxTrackings = new System.Windows.Forms.GroupBox();
            this.layoutControl3 = new DevExpress.XtraLayout.LayoutControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.txtPathologicalHistory = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.txtHospitalizationState = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.txtBeforeDiagnostic = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.txtTreatmentTracking = new DevExpress.XtraEditors.MemoEdit();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.txtRequestContent = new DevExpress.XtraEditors.MemoEdit();
            this.txtUserManual = new DevExpress.XtraEditors.MemoEdit();
            this.dtTimeUse = new DevExpress.XtraEditors.DateEdit();
            this.txtConcena = new DevExpress.XtraEditors.TextEdit();
            this.txtUseForm = new DevExpress.XtraEditors.TextEdit();
            this.txtMedicineName = new DevExpress.XtraEditors.TextEdit();
            this.cboMedcineType = new DevExpress.XtraEditors.ButtonEdit();
            this.cboActiveIngredient = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.LciMedicineName = new DevExpress.XtraLayout.LayoutControlItem();
            this.LciConcena = new DevExpress.XtraLayout.LayoutControlItem();
            this.LciUseForm = new DevExpress.XtraLayout.LayoutControlItem();
            this.LciRequestContent = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciForcboMedcineType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciForcboActiveIngredient = new DevExpress.XtraLayout.LayoutControlItem();
            this.LciServiceCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.LciServiceName = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciChonKQ = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.LciUserManual = new DevExpress.XtraLayout.LayoutControlItem();
            this.LciTimeUse = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciKetQuaCLS = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciKetQuaCLS2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciChonKQ2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.dxValidationProviderControl = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtKetQuaCLS2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKetQuaCLS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServiceName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServiceCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainerActiveIngredient)).BeginInit();
            this.popupControlContainerActiveIngredient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlContainerActiveIngredient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewContainerActiveIngredient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainerMedicineType)).BeginInit();
            this.popupControlContainerMedicineType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlContainerMedicineType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewContainerMedicineType)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).BeginInit();
            this.xtraTabControl2.SuspendLayout();
            this.xtraTabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscussion.Properties)).BeginInit();
            this.xtraTabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiagnostic.Properties)).BeginInit();
            this.xtraTabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentMethod.Properties)).BeginInit();
            this.xtraTabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCareMethod.Properties)).BeginInit();
            this.xtraTabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtConclusion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.groupBoxTrackings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl3)).BeginInit();
            this.layoutControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPathologicalHistory.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHospitalizationState.Properties)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBeforeDiagnostic.Properties)).BeginInit();
            this.xtraTabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentTracking.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRequestContent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserManual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTimeUse.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTimeUse.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConcena.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUseForm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedicineName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMedcineType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboActiveIngredient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciMedicineName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciConcena)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciUseForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciRequestContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciForcboMedcineType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciForcboActiveIngredient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciServiceCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciServiceName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChonKQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciUserManual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciTimeUse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciKetQuaCLS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciKetQuaCLS2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChonKQ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProviderControl)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnChonKQ2);
            this.layoutControl1.Controls.Add(this.txtKetQuaCLS2);
            this.layoutControl1.Controls.Add(this.btnChonKQ);
            this.layoutControl1.Controls.Add(this.txtKetQuaCLS);
            this.layoutControl1.Controls.Add(this.txtServiceName);
            this.layoutControl1.Controls.Add(this.txtServiceCode);
            this.layoutControl1.Controls.Add(this.popupControlContainerActiveIngredient);
            this.layoutControl1.Controls.Add(this.popupControlContainerMedicineType);
            this.layoutControl1.Controls.Add(this.groupBox1);
            this.layoutControl1.Controls.Add(this.groupBoxTrackings);
            this.layoutControl1.Controls.Add(this.txtRequestContent);
            this.layoutControl1.Controls.Add(this.txtUserManual);
            this.layoutControl1.Controls.Add(this.dtTimeUse);
            this.layoutControl1.Controls.Add(this.txtConcena);
            this.layoutControl1.Controls.Add(this.txtUseForm);
            this.layoutControl1.Controls.Add(this.txtMedicineName);
            this.layoutControl1.Controls.Add(this.cboMedcineType);
            this.layoutControl1.Controls.Add(this.cboActiveIngredient);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1607, 765);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnChonKQ2
            // 
            this.btnChonKQ2.Location = new System.Drawing.Point(1463, 3);
            this.btnChonKQ2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChonKQ2.Name = "btnChonKQ2";
            this.btnChonKQ2.Size = new System.Drawing.Size(141, 27);
            this.btnChonKQ2.StyleController = this.layoutControl1;
            this.btnChonKQ2.TabIndex = 24;
            this.btnChonKQ2.Text = "Chọn kết quả";
            this.btnChonKQ2.Click += new System.EventHandler(this.btnChonKQ_Click);
            // 
            // txtKetQuaCLS2
            // 
            this.txtKetQuaCLS2.Location = new System.Drawing.Point(1169, 36);
            this.txtKetQuaCLS2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtKetQuaCLS2.MenuManager = this.barManager1;
            this.txtKetQuaCLS2.Name = "txtKetQuaCLS2";
            this.txtKetQuaCLS2.Properties.NullValuePrompt = "Click \"Chọn kết quả\" để chọn dịch vụ";
            this.txtKetQuaCLS2.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtKetQuaCLS2.Size = new System.Drawing.Size(435, 73);
            this.txtKetQuaCLS2.StyleController = this.layoutControl1;
            this.txtKetQuaCLS2.TabIndex = 23;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.MaxItemId = 0;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar1.FloatLocation = new System.Drawing.Point(854, 726);
            this.bar1.Offset = 4;
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.barDockControlTop.Size = new System.Drawing.Size(1607, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 765);
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.barDockControlBottom.Size = new System.Drawing.Size(1607, 31);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 765);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1607, 0);
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 765);
            // 
            // btnChonKQ
            // 
            this.btnChonKQ.Location = new System.Drawing.Point(1470, 115);
            this.btnChonKQ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChonKQ.Name = "btnChonKQ";
            this.btnChonKQ.Size = new System.Drawing.Size(134, 18);
            this.btnChonKQ.StyleController = this.layoutControl1;
            this.btnChonKQ.TabIndex = 22;
            this.btnChonKQ.Text = "Chọn kết quả";
            this.btnChonKQ.Click += new System.EventHandler(this.btnChonKQ_Click);
            // 
            // txtKetQuaCLS
            // 
            this.txtKetQuaCLS.Location = new System.Drawing.Point(1169, 141);
            this.txtKetQuaCLS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtKetQuaCLS.MenuManager = this.barManager1;
            this.txtKetQuaCLS.Name = "txtKetQuaCLS";
            this.txtKetQuaCLS.Properties.NullValuePrompt = "Click \"Chọn kết quả\" để chọn dịch vụ";
            this.txtKetQuaCLS.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtKetQuaCLS.Size = new System.Drawing.Size(435, 34);
            this.txtKetQuaCLS.StyleController = this.layoutControl1;
            this.txtKetQuaCLS.TabIndex = 21;
            // 
            // txtServiceName
            // 
            this.txtServiceName.Enabled = false;
            this.txtServiceName.Location = new System.Drawing.Point(563, 114);
            this.txtServiceName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtServiceName.MenuManager = this.barManager1;
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(506, 22);
            this.txtServiceName.StyleController = this.layoutControl1;
            this.txtServiceName.TabIndex = 20;
            // 
            // txtServiceCode
            // 
            this.txtServiceCode.Enabled = false;
            this.txtServiceCode.Location = new System.Drawing.Point(142, 114);
            this.txtServiceCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtServiceCode.MenuManager = this.barManager1;
            this.txtServiceCode.Name = "txtServiceCode";
            this.txtServiceCode.Size = new System.Drawing.Size(421, 22);
            this.txtServiceCode.StyleController = this.layoutControl1;
            this.txtServiceCode.TabIndex = 19;
            // 
            // popupControlContainerActiveIngredient
            // 
            this.popupControlContainerActiveIngredient.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.popupControlContainerActiveIngredient.Controls.Add(this.gridControlContainerActiveIngredient);
            this.popupControlContainerActiveIngredient.Location = new System.Drawing.Point(648, 103);
            this.popupControlContainerActiveIngredient.Manager = this.barManager1;
            this.popupControlContainerActiveIngredient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.popupControlContainerActiveIngredient.Name = "popupControlContainerActiveIngredient";
            this.popupControlContainerActiveIngredient.Size = new System.Drawing.Size(336, 31);
            this.popupControlContainerActiveIngredient.TabIndex = 18;
            this.popupControlContainerActiveIngredient.Visible = false;
            this.popupControlContainerActiveIngredient.CloseUp += new System.EventHandler(this.popupControlContainerActiveIngredient_CloseUp);
            // 
            // gridControlContainerActiveIngredient
            // 
            this.gridControlContainerActiveIngredient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlContainerActiveIngredient.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridControlContainerActiveIngredient.Location = new System.Drawing.Point(0, 0);
            this.gridControlContainerActiveIngredient.MainView = this.gridViewContainerActiveIngredient;
            this.gridControlContainerActiveIngredient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridControlContainerActiveIngredient.Name = "gridControlContainerActiveIngredient";
            this.gridControlContainerActiveIngredient.Size = new System.Drawing.Size(336, 31);
            this.gridControlContainerActiveIngredient.TabIndex = 0;
            this.gridControlContainerActiveIngredient.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewContainerActiveIngredient});
            this.gridControlContainerActiveIngredient.Click += new System.EventHandler(this.gridControlContainerActiveIngredient_Click);
            // 
            // gridViewContainerActiveIngredient
            // 
            this.gridViewContainerActiveIngredient.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridViewContainerActiveIngredient.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewContainerActiveIngredient.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.gridViewContainerActiveIngredient.GridControl = this.gridControlContainerActiveIngredient;
            this.gridViewContainerActiveIngredient.Name = "gridViewContainerActiveIngredient";
            this.gridViewContainerActiveIngredient.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewContainerActiveIngredient.OptionsView.ShowGroupPanel = false;
            this.gridViewContainerActiveIngredient.OptionsView.ShowIndicator = false;
            this.gridViewContainerActiveIngredient.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridViewContainerActiveIngredient_KeyDown);
            this.gridViewContainerActiveIngredient.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridViewContainerActiveIngredient_MouseDown);
            // 
            // popupControlContainerMedicineType
            // 
            this.popupControlContainerMedicineType.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.popupControlContainerMedicineType.Controls.Add(this.gridControlContainerMedicineType);
            this.popupControlContainerMedicineType.Location = new System.Drawing.Point(212, 106);
            this.popupControlContainerMedicineType.Manager = this.barManager1;
            this.popupControlContainerMedicineType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.popupControlContainerMedicineType.Name = "popupControlContainerMedicineType";
            this.popupControlContainerMedicineType.Size = new System.Drawing.Size(336, 31);
            this.popupControlContainerMedicineType.TabIndex = 17;
            this.popupControlContainerMedicineType.Visible = false;
            this.popupControlContainerMedicineType.CloseUp += new System.EventHandler(this.popupControlContainerMedicineType_CloseUp);
            // 
            // gridControlContainerMedicineType
            // 
            this.gridControlContainerMedicineType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlContainerMedicineType.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridControlContainerMedicineType.Location = new System.Drawing.Point(0, 0);
            this.gridControlContainerMedicineType.MainView = this.gridViewContainerMedicineType;
            this.gridControlContainerMedicineType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridControlContainerMedicineType.Name = "gridControlContainerMedicineType";
            this.gridControlContainerMedicineType.Size = new System.Drawing.Size(336, 31);
            this.gridControlContainerMedicineType.TabIndex = 0;
            this.gridControlContainerMedicineType.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewContainerMedicineType});
            this.gridControlContainerMedicineType.Click += new System.EventHandler(this.gridControlContainerMedicineType_Click);
            // 
            // gridViewContainerMedicineType
            // 
            this.gridViewContainerMedicineType.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridViewContainerMedicineType.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewContainerMedicineType.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.gridViewContainerMedicineType.GridControl = this.gridControlContainerMedicineType;
            this.gridViewContainerMedicineType.Name = "gridViewContainerMedicineType";
            this.gridViewContainerMedicineType.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewContainerMedicineType.OptionsView.ShowGroupPanel = false;
            this.gridViewContainerMedicineType.OptionsView.ShowIndicator = false;
            this.gridViewContainerMedicineType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridViewContainerMedicineType_KeyDown);
            this.gridViewContainerMedicineType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridViewContainerMedicineType_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.layoutControl2);
            this.groupBox1.Location = new System.Drawing.Point(3, 391);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1601, 371);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nội dung kết luận";
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.xtraTabControl2);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(4, 19);
            this.layoutControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup2;
            this.layoutControl2.Size = new System.Drawing.Size(1593, 348);
            this.layoutControl2.TabIndex = 0;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // xtraTabControl2
            // 
            this.xtraTabControl2.Location = new System.Drawing.Point(3, 3);
            this.xtraTabControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabControl2.Name = "xtraTabControl2";
            this.xtraTabControl2.SelectedTabPage = this.xtraTabPage5;
            this.xtraTabControl2.Size = new System.Drawing.Size(1587, 342);
            this.xtraTabControl2.TabIndex = 9;
            this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage5,
            this.xtraTabPage6,
            this.xtraTabPage7,
            this.xtraTabPage8,
            this.xtraTabPage9});
            // 
            // xtraTabPage5
            // 
            this.xtraTabPage5.Controls.Add(this.txtDiscussion);
            this.xtraTabPage5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabPage5.Name = "xtraTabPage5";
            this.xtraTabPage5.Size = new System.Drawing.Size(1580, 307);
            this.xtraTabPage5.Text = "Ý kiến thảo luận";
            // 
            // txtDiscussion
            // 
            this.txtDiscussion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiscussion.Location = new System.Drawing.Point(0, 0);
            this.txtDiscussion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDiscussion.MenuManager = this.barManager1;
            this.txtDiscussion.Name = "txtDiscussion";
            this.txtDiscussion.Size = new System.Drawing.Size(1580, 307);
            this.txtDiscussion.TabIndex = 0;
            // 
            // xtraTabPage6
            // 
            this.xtraTabPage6.Controls.Add(this.txtDiagnostic);
            this.xtraTabPage6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabPage6.Name = "xtraTabPage6";
            this.xtraTabPage6.Size = new System.Drawing.Size(1578, 300);
            this.xtraTabPage6.Text = "Chẩn đoán, nguyên nhân tiên lượng";
            // 
            // txtDiagnostic
            // 
            this.txtDiagnostic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiagnostic.Location = new System.Drawing.Point(0, 0);
            this.txtDiagnostic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDiagnostic.MenuManager = this.barManager1;
            this.txtDiagnostic.Name = "txtDiagnostic";
            this.txtDiagnostic.Size = new System.Drawing.Size(1578, 300);
            this.txtDiagnostic.TabIndex = 0;
            // 
            // xtraTabPage7
            // 
            this.xtraTabPage7.Controls.Add(this.txtTreatmentMethod);
            this.xtraTabPage7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabPage7.Name = "xtraTabPage7";
            this.xtraTabPage7.Size = new System.Drawing.Size(1580, 307);
            this.xtraTabPage7.Text = "Hướng điều trị tiếp";
            // 
            // txtTreatmentMethod
            // 
            this.txtTreatmentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTreatmentMethod.Location = new System.Drawing.Point(0, 0);
            this.txtTreatmentMethod.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTreatmentMethod.MenuManager = this.barManager1;
            this.txtTreatmentMethod.Name = "txtTreatmentMethod";
            this.txtTreatmentMethod.Size = new System.Drawing.Size(1580, 307);
            this.txtTreatmentMethod.TabIndex = 0;
            // 
            // xtraTabPage8
            // 
            this.xtraTabPage8.Controls.Add(this.txtCareMethod);
            this.xtraTabPage8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabPage8.Name = "xtraTabPage8";
            this.xtraTabPage8.Size = new System.Drawing.Size(1578, 300);
            this.xtraTabPage8.Text = "Chăm sóc";
            // 
            // txtCareMethod
            // 
            this.txtCareMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCareMethod.Location = new System.Drawing.Point(0, 0);
            this.txtCareMethod.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCareMethod.MenuManager = this.barManager1;
            this.txtCareMethod.Name = "txtCareMethod";
            this.txtCareMethod.Size = new System.Drawing.Size(1578, 300);
            this.txtCareMethod.TabIndex = 0;
            // 
            // xtraTabPage9
            // 
            this.xtraTabPage9.Controls.Add(this.txtConclusion);
            this.xtraTabPage9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabPage9.Name = "xtraTabPage9";
            this.xtraTabPage9.Size = new System.Drawing.Size(1578, 300);
            this.xtraTabPage9.Text = "Kết luận";
            // 
            // txtConclusion
            // 
            this.txtConclusion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConclusion.Location = new System.Drawing.Point(0, 0);
            this.txtConclusion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtConclusion.MenuManager = this.barManager1;
            this.txtConclusion.Name = "txtConclusion";
            this.txtConclusion.Size = new System.Drawing.Size(1578, 300);
            this.txtConclusion.TabIndex = 0;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup4";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(1593, 348);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.xtraTabControl2;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(1593, 348);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // groupBoxTrackings
            // 
            this.groupBoxTrackings.Controls.Add(this.layoutControl3);
            this.groupBoxTrackings.Location = new System.Drawing.Point(3, 181);
            this.groupBoxTrackings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxTrackings.Name = "groupBoxTrackings";
            this.groupBoxTrackings.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxTrackings.Size = new System.Drawing.Size(1601, 204);
            this.groupBoxTrackings.TabIndex = 13;
            this.groupBoxTrackings.TabStop = false;
            this.groupBoxTrackings.Text = "Diễn biến bệnh";
            // 
            // layoutControl3
            // 
            this.layoutControl3.Controls.Add(this.xtraTabControl1);
            this.layoutControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl3.Location = new System.Drawing.Point(4, 19);
            this.layoutControl3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.layoutControl3.Name = "layoutControl3";
            this.layoutControl3.Root = this.layoutControlGroup3;
            this.layoutControl3.Size = new System.Drawing.Size(1593, 181);
            this.layoutControl3.TabIndex = 0;
            this.layoutControl3.Text = "layoutControl3";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.CustomHeaderButtons.AddRange(new DevExpress.XtraTab.Buttons.CustomHeaderButton[] {
            new DevExpress.XtraTab.Buttons.CustomHeaderButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Chọn diễn biến bệnh, diễn biến CLS", -1, true, true, DevExpress.XtraEditors.ImageLocation.MiddleRight, null, serializableAppearanceObject1, "", "Chondienbien", null, true)});
            this.xtraTabControl1.Location = new System.Drawing.Point(3, 3);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1587, 175);
            this.xtraTabControl1.TabIndex = 8;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3,
            this.xtraTabPage4});
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            this.xtraTabControl1.CustomHeaderButtonClick += new DevExpress.XtraTab.ViewInfo.CustomHeaderButtonEventHandler(this.xtraTabControl1_CustomHeaderButtonClick);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtPathologicalHistory);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1580, 140);
            this.xtraTabPage1.Text = "Tóm tắt tiền sử bệnh";
            // 
            // txtPathologicalHistory
            // 
            this.txtPathologicalHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPathologicalHistory.Location = new System.Drawing.Point(0, 0);
            this.txtPathologicalHistory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPathologicalHistory.Name = "txtPathologicalHistory";
            this.txtPathologicalHistory.Size = new System.Drawing.Size(1580, 140);
            this.txtPathologicalHistory.TabIndex = 4;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.txtHospitalizationState);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1578, 108);
            this.xtraTabPage2.Text = "Tình trạng lúc vào viện";
            // 
            // txtHospitalizationState
            // 
            this.txtHospitalizationState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHospitalizationState.Location = new System.Drawing.Point(0, 0);
            this.txtHospitalizationState.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtHospitalizationState.Name = "txtHospitalizationState";
            this.txtHospitalizationState.Properties.MaxLength = 2000;
            this.txtHospitalizationState.Size = new System.Drawing.Size(1578, 108);
            this.txtHospitalizationState.TabIndex = 5;
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.txtBeforeDiagnostic);
            this.xtraTabPage3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(1578, 108);
            this.xtraTabPage3.Text = "Chẩn đoán(Tuyến dưới, KKB, điều trị)";
            // 
            // txtBeforeDiagnostic
            // 
            this.txtBeforeDiagnostic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBeforeDiagnostic.Location = new System.Drawing.Point(0, 0);
            this.txtBeforeDiagnostic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBeforeDiagnostic.Name = "txtBeforeDiagnostic";
            this.txtBeforeDiagnostic.Properties.MaxLength = 2000;
            this.txtBeforeDiagnostic.Size = new System.Drawing.Size(1578, 108);
            this.txtBeforeDiagnostic.TabIndex = 6;
            // 
            // xtraTabPage4
            // 
            this.xtraTabPage4.Controls.Add(this.txtTreatmentTracking);
            this.xtraTabPage4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Size = new System.Drawing.Size(1578, 108);
            this.xtraTabPage4.Text = "Tóm tắt diễn biến bệnh, quá trình điều trị, chăm sóc";
            // 
            // txtTreatmentTracking
            // 
            this.txtTreatmentTracking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTreatmentTracking.Location = new System.Drawing.Point(0, 0);
            this.txtTreatmentTracking.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTreatmentTracking.Name = "txtTreatmentTracking";
            this.txtTreatmentTracking.Size = new System.Drawing.Size(1578, 108);
            this.txtTreatmentTracking.TabIndex = 7;
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup3.GroupBordersVisible = false;
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup3.Size = new System.Drawing.Size(1593, 181);
            this.layoutControlGroup3.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.xtraTabControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1593, 181);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // txtRequestContent
            // 
            this.txtRequestContent.Location = new System.Drawing.Point(143, 141);
            this.txtRequestContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRequestContent.Name = "txtRequestContent";
            this.txtRequestContent.Size = new System.Drawing.Size(925, 34);
            this.txtRequestContent.StyleController = this.layoutControl1;
            this.txtRequestContent.TabIndex = 8;
            this.txtRequestContent.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtRequestContent_PreviewKeyDown);
            // 
            // txtUserManual
            // 
            this.txtUserManual.Location = new System.Drawing.Point(661, 59);
            this.txtUserManual.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUserManual.Name = "txtUserManual";
            this.txtUserManual.Size = new System.Drawing.Size(407, 50);
            this.txtUserManual.StyleController = this.layoutControl1;
            this.txtUserManual.TabIndex = 7;
            this.txtUserManual.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtUserManual_PreviewKeyDown);
            // 
            // dtTimeUse
            // 
            this.dtTimeUse.EditValue = null;
            this.dtTimeUse.Location = new System.Drawing.Point(143, 87);
            this.dtTimeUse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtTimeUse.Name = "dtTimeUse";
            this.dtTimeUse.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.dtTimeUse.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtTimeUse.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtTimeUse.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
            this.dtTimeUse.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtTimeUse.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm";
            this.dtTimeUse.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtTimeUse.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm";
            this.dtTimeUse.Size = new System.Drawing.Size(417, 22);
            this.dtTimeUse.StyleController = this.layoutControl1;
            this.dtTimeUse.TabIndex = 6;
            this.dtTimeUse.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dtTimeUse_PreviewKeyDown);
            // 
            // txtConcena
            // 
            this.txtConcena.Location = new System.Drawing.Point(661, 31);
            this.txtConcena.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtConcena.Name = "txtConcena";
            this.txtConcena.Size = new System.Drawing.Size(407, 22);
            this.txtConcena.StyleController = this.layoutControl1;
            this.txtConcena.TabIndex = 4;
            this.txtConcena.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtConcena_PreviewKeyDown);
            // 
            // txtUseForm
            // 
            this.txtUseForm.Location = new System.Drawing.Point(143, 59);
            this.txtUseForm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUseForm.Name = "txtUseForm";
            this.txtUseForm.Size = new System.Drawing.Size(417, 22);
            this.txtUseForm.StyleController = this.layoutControl1;
            this.txtUseForm.TabIndex = 5;
            this.txtUseForm.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtUseForm_PreviewKeyDown);
            // 
            // txtMedicineName
            // 
            this.txtMedicineName.Location = new System.Drawing.Point(143, 31);
            this.txtMedicineName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMedicineName.Name = "txtMedicineName";
            this.txtMedicineName.Size = new System.Drawing.Size(417, 22);
            this.txtMedicineName.StyleController = this.layoutControl1;
            this.txtMedicineName.TabIndex = 2;
            this.txtMedicineName.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtMedicineName_PreviewKeyDown);
            // 
            // cboMedcineType
            // 
            this.cboMedcineType.Location = new System.Drawing.Point(143, 3);
            this.cboMedcineType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboMedcineType.Name = "cboMedcineType";
            this.cboMedcineType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboMedcineType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, serializableAppearanceObject5, "", null, null, true)});
            this.cboMedcineType.Size = new System.Drawing.Size(417, 22);
            this.cboMedcineType.StyleController = this.layoutControl1;
            this.cboMedcineType.TabIndex = 1;
            this.cboMedcineType.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboMedcineType_ButtonClick);
            this.cboMedcineType.TextChanged += new System.EventHandler(this.cboMedcineType_TextChanged);
            this.cboMedcineType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboMedcineType_KeyDown);
            this.cboMedcineType.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cboMedcineType_PreviewKeyDown);
            // 
            // cboActiveIngredient
            // 
            this.cboActiveIngredient.Location = new System.Drawing.Point(661, 3);
            this.cboActiveIngredient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboActiveIngredient.Name = "cboActiveIngredient";
            this.cboActiveIngredient.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboActiveIngredient.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, serializableAppearanceObject9, "", null, null, true)});
            this.cboActiveIngredient.Size = new System.Drawing.Size(407, 22);
            this.cboActiveIngredient.StyleController = this.layoutControl1;
            this.cboActiveIngredient.TabIndex = 2;
            this.cboActiveIngredient.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboActiveIngredient_ButtonClick);
            this.cboActiveIngredient.TextChanged += new System.EventHandler(this.cboActiveIngredient_TextChanged);
            this.cboActiveIngredient.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboActiveIngredient_KeyDown);
            this.cboActiveIngredient.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cboActiveIngredient_PreviewKeyDown);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.LciMedicineName,
            this.LciConcena,
            this.LciUseForm,
            this.LciRequestContent,
            this.layoutControlItem2,
            this.layoutControlItem8,
            this.lciForcboMedcineType,
            this.lciForcboActiveIngredient,
            this.LciServiceCode,
            this.LciServiceName,
            this.lciChonKQ,
            this.emptySpaceItem1,
            this.LciUserManual,
            this.LciTimeUse,
            this.lciKetQuaCLS,
            this.lciKetQuaCLS2,
            this.lciChonKQ2,
            this.emptySpaceItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1607, 765);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // LciMedicineName
            // 
            this.LciMedicineName.AppearanceItemCaption.Options.UseTextOptions = true;
            this.LciMedicineName.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.LciMedicineName.Control = this.txtMedicineName;
            this.LciMedicineName.Location = new System.Drawing.Point(0, 28);
            this.LciMedicineName.Name = "LciMedicineName";
            this.LciMedicineName.Size = new System.Drawing.Size(563, 28);
            this.LciMedicineName.Text = "Tên thuốc:";
            this.LciMedicineName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.LciMedicineName.TextSize = new System.Drawing.Size(135, 20);
            this.LciMedicineName.TextToControlDistance = 5;
            // 
            // LciConcena
            // 
            this.LciConcena.AppearanceItemCaption.Options.UseTextOptions = true;
            this.LciConcena.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.LciConcena.Control = this.txtConcena;
            this.LciConcena.Location = new System.Drawing.Point(563, 28);
            this.LciConcena.Name = "LciConcena";
            this.LciConcena.Size = new System.Drawing.Size(508, 28);
            this.LciConcena.Text = "Hàm lượng:";
            this.LciConcena.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.LciConcena.TextSize = new System.Drawing.Size(90, 20);
            this.LciConcena.TextToControlDistance = 5;
            // 
            // LciUseForm
            // 
            this.LciUseForm.AppearanceItemCaption.Options.UseTextOptions = true;
            this.LciUseForm.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.LciUseForm.Control = this.txtUseForm;
            this.LciUseForm.Location = new System.Drawing.Point(0, 56);
            this.LciUseForm.Name = "LciUseForm";
            this.LciUseForm.Size = new System.Drawing.Size(563, 28);
            this.LciUseForm.Text = "Đường dùng:";
            this.LciUseForm.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.LciUseForm.TextSize = new System.Drawing.Size(135, 20);
            this.LciUseForm.TextToControlDistance = 5;
            // 
            // LciRequestContent
            // 
            this.LciRequestContent.AppearanceItemCaption.Options.UseTextOptions = true;
            this.LciRequestContent.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.LciRequestContent.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.LciRequestContent.Control = this.txtRequestContent;
            this.LciRequestContent.Location = new System.Drawing.Point(0, 138);
            this.LciRequestContent.Name = "LciRequestContent";
            this.LciRequestContent.Size = new System.Drawing.Size(1071, 40);
            this.LciRequestContent.Text = "Yêu cầu hội chẩn:";
            this.LciRequestContent.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.LciRequestContent.TextSize = new System.Drawing.Size(135, 20);
            this.LciRequestContent.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.groupBoxTrackings;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 178);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1607, 210);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.groupBox1;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 388);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(1607, 377);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // lciForcboMedcineType
            // 
            this.lciForcboMedcineType.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciForcboMedcineType.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciForcboMedcineType.Control = this.cboMedcineType;
            this.lciForcboMedcineType.Location = new System.Drawing.Point(0, 0);
            this.lciForcboMedcineType.Name = "lciForcboMedcineType";
            this.lciForcboMedcineType.Size = new System.Drawing.Size(563, 28);
            this.lciForcboMedcineType.Text = "Chọn thuốc:";
            this.lciForcboMedcineType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciForcboMedcineType.TextSize = new System.Drawing.Size(135, 20);
            this.lciForcboMedcineType.TextToControlDistance = 5;
            // 
            // lciForcboActiveIngredient
            // 
            this.lciForcboActiveIngredient.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciForcboActiveIngredient.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciForcboActiveIngredient.Control = this.cboActiveIngredient;
            this.lciForcboActiveIngredient.Location = new System.Drawing.Point(563, 0);
            this.lciForcboActiveIngredient.Name = "lciForcboActiveIngredient";
            this.lciForcboActiveIngredient.Size = new System.Drawing.Size(508, 28);
            this.lciForcboActiveIngredient.Text = "Hoạt chất:";
            this.lciForcboActiveIngredient.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciForcboActiveIngredient.TextSize = new System.Drawing.Size(90, 20);
            this.lciForcboActiveIngredient.TextToControlDistance = 5;
            // 
            // LciServiceCode
            // 
            this.LciServiceCode.AppearanceItemCaption.Options.UseTextOptions = true;
            this.LciServiceCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.LciServiceCode.Control = this.txtServiceCode;
            this.LciServiceCode.Location = new System.Drawing.Point(0, 112);
            this.LciServiceCode.Name = "LciServiceCode";
            this.LciServiceCode.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
            this.LciServiceCode.Size = new System.Drawing.Size(563, 26);
            this.LciServiceCode.Text = "Dịch vụ hội chẩn:";
            this.LciServiceCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.LciServiceCode.TextSize = new System.Drawing.Size(135, 20);
            this.LciServiceCode.TextToControlDistance = 5;
            // 
            // LciServiceName
            // 
            this.LciServiceName.Control = this.txtServiceName;
            this.LciServiceName.Location = new System.Drawing.Point(563, 112);
            this.LciServiceName.Name = "LciServiceName";
            this.LciServiceName.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
            this.LciServiceName.Size = new System.Drawing.Size(508, 26);
            this.LciServiceName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.LciServiceName.TextSize = new System.Drawing.Size(0, 0);
            this.LciServiceName.TextToControlDistance = 0;
            this.LciServiceName.TextVisible = false;
            // 
            // lciChonKQ
            // 
            this.lciChonKQ.Control = this.btnChonKQ;
            this.lciChonKQ.Location = new System.Drawing.Point(1467, 112);
            this.lciChonKQ.MaxSize = new System.Drawing.Size(0, 24);
            this.lciChonKQ.MinSize = new System.Drawing.Size(77, 24);
            this.lciChonKQ.Name = "lciChonKQ";
            this.lciChonKQ.Size = new System.Drawing.Size(140, 26);
            this.lciChonKQ.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciChonKQ.TextSize = new System.Drawing.Size(0, 0);
            this.lciChonKQ.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(1071, 112);
            this.emptySpaceItem1.MinSize = new System.Drawing.Size(104, 24);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(396, 26);
            this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // LciUserManual
            // 
            this.LciUserManual.AppearanceItemCaption.Options.UseTextOptions = true;
            this.LciUserManual.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.LciUserManual.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.LciUserManual.Control = this.txtUserManual;
            this.LciUserManual.Location = new System.Drawing.Point(563, 56);
            this.LciUserManual.Name = "LciUserManual";
            this.LciUserManual.OptionsToolTip.ToolTip = "Hướng dẫn sử dụng";
            this.LciUserManual.Size = new System.Drawing.Size(508, 56);
            this.LciUserManual.Text = "Liều dùng:";
            this.LciUserManual.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.LciUserManual.TextSize = new System.Drawing.Size(90, 20);
            this.LciUserManual.TextToControlDistance = 5;
            // 
            // LciTimeUse
            // 
            this.LciTimeUse.AppearanceItemCaption.Options.UseTextOptions = true;
            this.LciTimeUse.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.LciTimeUse.Control = this.dtTimeUse;
            this.LciTimeUse.Location = new System.Drawing.Point(0, 84);
            this.LciTimeUse.Name = "LciTimeUse";
            this.LciTimeUse.Size = new System.Drawing.Size(563, 28);
            this.LciTimeUse.Text = "Thời gian dùng:";
            this.LciTimeUse.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.LciTimeUse.TextSize = new System.Drawing.Size(135, 20);
            this.LciTimeUse.TextToControlDistance = 5;
            // 
            // lciKetQuaCLS
            // 
            this.lciKetQuaCLS.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciKetQuaCLS.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciKetQuaCLS.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lciKetQuaCLS.Control = this.txtKetQuaCLS;
            this.lciKetQuaCLS.Location = new System.Drawing.Point(1071, 138);
            this.lciKetQuaCLS.Name = "lciKetQuaCLS";
            this.lciKetQuaCLS.Size = new System.Drawing.Size(536, 40);
            this.lciKetQuaCLS.Text = "Kết quả CLS:";
            this.lciKetQuaCLS.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciKetQuaCLS.TextSize = new System.Drawing.Size(90, 20);
            this.lciKetQuaCLS.TextToControlDistance = 5;
            // 
            // lciKetQuaCLS2
            // 
            this.lciKetQuaCLS2.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciKetQuaCLS2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciKetQuaCLS2.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lciKetQuaCLS2.Control = this.txtKetQuaCLS2;
            this.lciKetQuaCLS2.Location = new System.Drawing.Point(1071, 33);
            this.lciKetQuaCLS2.Name = "lciKetQuaCLS2";
            this.lciKetQuaCLS2.Size = new System.Drawing.Size(536, 79);
            this.lciKetQuaCLS2.Text = "Kết quả CLS:";
            this.lciKetQuaCLS2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciKetQuaCLS2.TextSize = new System.Drawing.Size(90, 20);
            this.lciKetQuaCLS2.TextToControlDistance = 5;
            // 
            // lciChonKQ2
            // 
            this.lciChonKQ2.Control = this.btnChonKQ2;
            this.lciChonKQ2.Location = new System.Drawing.Point(1460, 0);
            this.lciChonKQ2.Name = "lciChonKQ2";
            this.lciChonKQ2.Size = new System.Drawing.Size(147, 33);
            this.lciChonKQ2.TextSize = new System.Drawing.Size(0, 0);
            this.lciChonKQ2.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(1071, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(389, 33);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // dxValidationProvider1
            // 
            this.dxValidationProvider1.ValidationFailed += new DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventHandler(this.dxValidationProvider1_ValidationFailed);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            // 
            // memoEdit1
            // 
            this.memoEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEdit1.Location = new System.Drawing.Point(0, 0);
            this.memoEdit1.MenuManager = this.barManager1;
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(430, 47);
            this.memoEdit1.TabIndex = 0;
            // 
            // dxValidationProviderControl
            // 
            this.dxValidationProviderControl.ValidationFailed += new DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventHandler(this.dxValidationProviderControl_ValidationFailed);
            // 
            // UcOther
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UcOther";
            this.Size = new System.Drawing.Size(1607, 796);
            this.Load += new System.EventHandler(this.UcOther_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtKetQuaCLS2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKetQuaCLS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServiceName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServiceCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainerActiveIngredient)).EndInit();
            this.popupControlContainerActiveIngredient.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlContainerActiveIngredient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewContainerActiveIngredient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainerMedicineType)).EndInit();
            this.popupControlContainerMedicineType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlContainerMedicineType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewContainerMedicineType)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).EndInit();
            this.xtraTabControl2.ResumeLayout(false);
            this.xtraTabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscussion.Properties)).EndInit();
            this.xtraTabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDiagnostic.Properties)).EndInit();
            this.xtraTabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentMethod.Properties)).EndInit();
            this.xtraTabPage8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCareMethod.Properties)).EndInit();
            this.xtraTabPage9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtConclusion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.groupBoxTrackings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl3)).EndInit();
            this.layoutControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPathologicalHistory.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtHospitalizationState.Properties)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBeforeDiagnostic.Properties)).EndInit();
            this.xtraTabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentTracking.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRequestContent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserManual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTimeUse.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTimeUse.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConcena.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUseForm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedicineName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMedcineType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboActiveIngredient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciMedicineName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciConcena)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciUseForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciRequestContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciForcboMedcineType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciForcboActiveIngredient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciServiceCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciServiceName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChonKQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciUserManual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LciTimeUse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciKetQuaCLS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciKetQuaCLS2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciChonKQ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProviderControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
