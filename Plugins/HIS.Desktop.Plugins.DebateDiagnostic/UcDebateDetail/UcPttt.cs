using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACS.EFMODEL.DataModels;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTab;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.ModuleExt;
using HIS.Desktop.Plugins.DebateDiagnostic.ADO;
using HIS.Desktop.Plugins.DebateDiagnostic.Base;
using HIS.Desktop.Plugins.DebateDiagnostic.EkipTemp;
using HIS.Desktop.Utilities.Extensions;
using Inventec.Common.Adapter;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.Controls.ValidationRule;
using Inventec.Desktop.Common.LibraryMessage;
using Inventec.Desktop.Common.Message;
using Inventec.UC.Login.Base;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace HIS.Desktop.Plugins.DebateDiagnostic.UcDebateDetail
{
	public class UcPttt : UserControl
	{
		private int positionHandleControl = -1;

		private long treatmentId;

		private long _roomId;

		private long _roomTypeId;

		public List<ACS_USER> UserList;

		public List<V_HIS_EMPLOYEE> EmployeeList;

		public List<HIS_DEPARTMENT> DepartmentList;

		public List<HIS_EXECUTE_ROLE> ExecuteRoleList;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private SimpleButton BtnChooseCls;

		private EmptySpaceItem emptySpaceItem1;

		private LayoutControlItem layoutControlItem1;

		private MemoEdit TxtKqCls;

		private LayoutControlItem LciKqCls;

		private GridLookUpEdit CboEkipTemp;

		private GridView gridLookUpEdit1View;

		private LayoutControlItem LciEkipTemp;

		private SimpleButton BtnSaveEkipTemp;

		private LayoutControlItem layoutControlItem2;

		private GridControl grdControlInformationSurg;

		private GridView grdViewInformationSurg;

		private GridColumn gridColumn1;

		private RepositoryItemCustomGridLookUpEdit cbo_UseName;

		private CustomGridView repositoryItemCustomGridLookUpEditNew1View;

		private GridColumn gridColumn2;

		private RepositoryItemLookUpEdit cboPosition;

		private GridColumn gridColumn3;

		private RepositoryItemCustomGridLookUpEdit GridLookUpEdit_Department;

		private CustomGridView customGridView1;

		private GridColumn gridColumn4;

		private RepositoryItemButtonEdit btnAdd;

		private GridColumn gridColumn5;

		private RepositoryItemTextEdit txtLogin;

		private RepositoryItemButtonEdit btnDelete;

		private RepositoryItemGridLookUpEdit repositoryItemGridLookUpUsername;

		private GridView gridView2;

		private RepositoryItemSearchLookUpEdit repositoryItemSearchLookUpEdit1;

		private GridView gridView3;

		private LayoutControlItem lciInformationSurg;

		private DateEdit DtSurgeryTime;

		private GridLookUpEdit CboEmotionlessMethod;

		private GridView gridLookUpEdit2View;

		private TextEdit TxtEmotionlessMethod;

		private GridLookUpEdit CboMethod;

		private GridView gridView4;

		private TextEdit TxtMethodCode;

		private LayoutControlItem LciMethodCode;

		private LayoutControlItem LciEmotionlessMethod;

		private LayoutControlItem layoutControlItem6;

		private LayoutControlItem LciSurgeryTime;

		private DXValidationProvider dxValidationProvider1;

		private PanelControl panelControl1;

		private LayoutControlItem layoutControlItem3;

		private TextEdit txtPtttMethod;

		private CheckEdit checkEditMethod;

		private LayoutControlItem layoutControlItem4;

		private GroupBox groupBox2;

		private GroupBox groupBox1;

		private LayoutControlItem layoutControlItem8;

		private LayoutControlItem layoutControlItem9;

		private LayoutControl layoutControl3;

		private LayoutControlGroup layoutControlGroup3;

		private LayoutControl layoutControl2;

		private LayoutControlGroup layoutControlGroup2;

		private XtraTabControl xtraTabControl1;

		private XtraTabPage xtraTabPage1;

		private XtraTabPage xtraTabPage2;

		private LayoutControlItem layoutControlItem10;

		private XtraTabControl xtraTabControl2;

		private XtraTabPage xtraTabPage3;

		private XtraTabPage xtraTabPage4;

		private LayoutControlItem layoutControlItem11;

		private XtraTabPage xtraTabPage5;

		private XtraTabPage xtraTabPage6;

		private XtraTabPage xtraTabPage7;

		private XtraTabPage xtraTabPage8;

		private XtraTabPage xtraTabPage9;

		private MemoEdit TxtPrognosis;

		private MemoEdit txtTreatmentMethod;

		private MemoEdit memCONCLUSION;

		private MemoEdit memDiscussion;

		private MemoEdit memCareMethod;

		private XtraTabPage xtraTabPage10;

		private MemoEdit TxtTreatmentTracking;

		private MemoEdit memBeforeDiagnostic;

		private MemoEdit txtInternalMedicineState;

		private MemoEdit memHosState;

		private MemoEdit memPathHis;

		private List<HIS_EKIP_TEMP> ekipTemps { get; set; }

		private List<HIS_EXECUTE_ROLE_USER> executeRoleUsers { get; set; }

		internal List<AcsUserADO> AcsUserADOList { get; set; }

		private List<HIS_PTTT_METHOD> ListPtttMethod { get; set; }

		private List<HIS_EMOTIONLESS_METHOD> ListEmotionMethod { get; set; }

		public UcPttt(long treatmentId, long roomId, long roomTypeId, List<ACS_USER> UserList, List<V_HIS_EMPLOYEE> EmployeeList, List<HIS_DEPARTMENT> DepartmentList, List<HIS_EXECUTE_ROLE> ExecuteRoleList)
		{
			InitializeComponent();
			this.treatmentId = treatmentId;
			_roomId = roomId;
			_roomTypeId = roomTypeId;
			this.UserList = UserList;
			this.EmployeeList = EmployeeList;
			this.DepartmentList = DepartmentList;
			this.ExecuteRoleList = ExecuteRoleList;
		}

		private void UcPttt_Load(object sender, EventArgs e)
		{
			try
			{
				WaitingManager.Show();
				ComboEkipTemp(CboEkipTemp);
				ComboExecuteRole();
				ComboAcsUser();
				LoadDataToComboDepartment();
				ComboEmotionlessMothod();
				ComboPPMethod();
				FillDataToInformationSurg();
				ValidationControl();
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex);
			}
		}

		private void BtnSaveEkipTemp_Click(object sender, EventArgs e)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			try
			{
                List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO> list = grdControlInformationSurg.DataSource as List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO>;
                if ((list.Where(delegate(HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO o)
				{
					int result;
					if (!string.IsNullOrEmpty(((V_HIS_EKIP_USER)o).LOGINNAME) && ((V_HIS_EKIP_USER)o).EXECUTE_ROLE_ID > 0)
					{
						long eXECUTE_ROLE_ID = ((V_HIS_EKIP_USER)o).EXECUTE_ROLE_ID;
						result = 0;
					}
					else
					{
						result = 1;
					}
					return (byte)result != 0;
				}).FirstOrDefault() != null) ? true : false)
				{
					MessageBox.Show("Không có thông tin kip thực hiện");
					return;
				}
				frmEkipTemp frmEkipTemp = new frmEkipTemp(list, new DelegateRefreshData(RefeshDataEkipTemp));
				frmEkipTemp.ShowDialog();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void RefeshDataEkipTemp()
		{
			try
			{
				ComboEkipTemp(CboEkipTemp);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private async Task ComboEkipTemp(GridLookUpEdit cbo)
		{
			try
			{
				Action myaction = delegate
				{
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Expected O, but got Unknown
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0023: Expected O, but got Unknown
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					string logginName = ClientTokenManagerStore.ClientTokenManager.GetLoginName();
					CommonParam val = new CommonParam();
					HisEkipTempFilter val2 = new HisEkipTempFilter();
					ekipTemps = ((AdapterBase)new BackendAdapter(val)).Get<List<HIS_EKIP_TEMP>>("/api/HisEkipTemp/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, val);
					if (ekipTemps != null && ekipTemps.Count > 0)
					{
						ekipTemps = (from o in ekipTemps
							where o.IS_PUBLIC == 1 || o.CREATOR == logginName
							orderby o.CREATE_TIME descending
							select o).ToList();
					}
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				ControlEditorADO controlEditorADO = new ControlEditorADO("EKIP_TEMP_NAME", "ID", new List<ColumnInfo>
				{
					new ColumnInfo("EKIP_TEMP_NAME", "", 250, 1)
				}, false, 250);
				ControlEditorLoader.Load((object)cbo, (object)ekipTemps, controlEditorADO);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private async Task ComboEmotionlessMothod()
		{
			try
			{
				Action myaction = delegate
				{
					ListEmotionMethod = BackendDataWorker.Get<HIS_EMOTIONLESS_METHOD>();
					ListEmotionMethod = ((ListEmotionMethod != null) ? ListEmotionMethod.Where((HIS_EMOTIONLESS_METHOD p) => p.IS_ACTIVE == 1 && (p.IS_FIRST == 1 || (p.IS_FIRST != 1 && p.IS_SECOND != 1))).ToList() : null);
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				ControlEditorADO controlEditorADO = new ControlEditorADO("EMOTIONLESS_METHOD_NAME", "ID", new List<ColumnInfo>
				{
					new ColumnInfo("EMOTIONLESS_METHOD_CODE", "", 150, 1),
					new ColumnInfo("EMOTIONLESS_METHOD_NAME", "", 250, 2)
				}, false, 250);
				ControlEditorLoader.Load((object)CboEmotionlessMethod, (object)ListEmotionMethod, controlEditorADO);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private async Task ComboPPMethod()
		{
			try
			{
				Action myaction = delegate
				{
					ListPtttMethod = (from p in BackendDataWorker.Get<HIS_PTTT_METHOD>()
						where p.IS_ACTIVE == 1
						select p).ToList();
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				ControlEditorADO controlEditorADO = new ControlEditorADO("PTTT_METHOD_NAME", "ID", new List<ColumnInfo>
				{
					new ColumnInfo("PTTT_METHOD_CODE", "", 150, 1),
					new ColumnInfo("PTTT_METHOD_NAME", "", 250, 2)
				}, false, 250);
				ControlEditorLoader.Load((object)CboMethod, (object)ListPtttMethod, controlEditorADO);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private async Task ComboAcsUser()
		{
			try
			{
				Action myaction = delegate
				{
					AcsUserADOList = ProcessAcsUser();
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				ControlEditorADO controlEditorADO = new ControlEditorADO("USERNAME", "LOGINNAME", new List<ColumnInfo>
				{
					new ColumnInfo("LOGINNAME", "", 150, 1),
					new ColumnInfo("USERNAME", "", 250, 2),
					new ColumnInfo("DEPARTMENT_NAME", "", 250, 3)
				}, false, 750);
				ControlEditorLoader.Load((object)cbo_UseName, (object)AcsUserADOList, controlEditorADO);
				repositoryItemSearchLookUpEdit1.DataSource = AcsUserADOList;
				repositoryItemSearchLookUpEdit1.DisplayMember = "USERNAME";
				repositoryItemSearchLookUpEdit1.ValueMember = "LOGINNAME";
				repositoryItemSearchLookUpEdit1.TextEditStyle = TextEditStyles.Standard;
				repositoryItemSearchLookUpEdit1.PopupFilterMode = PopupFilterMode.Contains;
				repositoryItemSearchLookUpEdit1.ImmediatePopup = true;
				repositoryItemSearchLookUpEdit1.View.Columns.Clear();
				GridColumn aColumnCode = repositoryItemSearchLookUpEdit1.View.Columns.AddField("LOGINNAME");
				aColumnCode.Caption = "Mã";
				aColumnCode.Visible = true;
				aColumnCode.VisibleIndex = 1;
				aColumnCode.Width = 100;
				GridColumn aColumnName = repositoryItemSearchLookUpEdit1.View.Columns.AddField("USERNAME");
				aColumnName.Caption = "Tên";
				aColumnName.Visible = true;
				aColumnName.VisibleIndex = 2;
				aColumnName.Width = 200;
				GridColumn aColumnDepartment = repositoryItemSearchLookUpEdit1.View.Columns.AddField("DEPARTMENT_NAME");
				aColumnDepartment.Caption = "Khoa";
				aColumnDepartment.Visible = true;
				aColumnDepartment.VisibleIndex = 3;
				aColumnDepartment.Width = 200;
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private void ComboExecuteRole()
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			try
			{
				List<HIS_EXECUTE_ROLE> list = ExecuteRoleList;
				if (list != null && list.Count > 0)
				{
					list = list.Where((HIS_EXECUTE_ROLE p) => p.IS_ACTIVE == 1).ToList();
				}
				List<ColumnInfo> list2 = new List<ColumnInfo>();
				list2.Add(new ColumnInfo("EXECUTE_ROLE_CODE", "", 150, 1));
				list2.Add(new ColumnInfo("EXECUTE_ROLE_NAME", "", 250, 2));
				ControlEditorADO val = new ControlEditorADO("EXECUTE_ROLE_NAME", "ID", list2, false, 250);
				ControlEditorLoader.Load((object)cboPosition, (object)list, val);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private async Task LoadDataToComboDepartment()
		{
			try
			{
				List<HIS_DEPARTMENT> departmentClinic = DepartmentList.Where((HIS_DEPARTMENT o) => o.IS_CLINICAL == 1 && o.IS_ACTIVE == 1).ToList();
				ControlEditorADO controlEditorADO = new ControlEditorADO("DEPARTMENT_NAME", "ID", new List<ColumnInfo>
				{
					new ColumnInfo("DEPARTMENT_CODE", "", 150, 1),
					new ColumnInfo("DEPARTMENT_NAME", "", 250, 2)
				}, false, 400);
				ControlEditorLoader.Load((object)GridLookUpEdit_Department, (object)departmentClinic, controlEditorADO);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private List<AcsUserADO> ProcessAcsUser()
		{
			List<AcsUserADO> list = null;
			try
			{
				List<ACS_USER> source = UserList.Where((ACS_USER o) => o.IS_ACTIVE == 1).ToList();
				List<V_HIS_EMPLOYEE> list2 = EmployeeList.Where((V_HIS_EMPLOYEE o) => o.IS_ACTIVE == 1).ToList();
				List<HIS_DEPARTMENT> source2 = DepartmentList.Where((HIS_DEPARTMENT o) => o.IS_ACTIVE == 1).ToList();
				list = new List<AcsUserADO>();
				foreach (V_HIS_EMPLOYEE item in list2)
				{
					AcsUserADO acsUserADO = new AcsUserADO();
					((ACS_USER)acsUserADO).ID = item.ID;
					((ACS_USER)acsUserADO).LOGINNAME = item.LOGINNAME;
					HIS_DEPARTMENT val = source2.FirstOrDefault((HIS_DEPARTMENT o) => o.ID == item.DEPARTMENT_ID);
					if (val != null)
					{
						acsUserADO.DEPARTMENT_NAME = val.DEPARTMENT_NAME;
					}
					ACS_USER val2 = source.FirstOrDefault((ACS_USER o) => o.LOGINNAME == item.LOGINNAME);
					if (val2 != null)
					{
						((ACS_USER)acsUserADO).USERNAME = val2.USERNAME;
						((ACS_USER)acsUserADO).MOBILE = val2.MOBILE;
						((ACS_USER)acsUserADO).PASSWORD = val2.PASSWORD;
					}
					list.Add(acsUserADO);
				}
				list = list.OrderBy((AcsUserADO o) => ((ACS_USER)o).USERNAME).ToList();
			}
			catch (Exception ex)
			{
				list = null;
				LogSystem.Warn(ex);
			}
			return list;
		}

		private void ValidationControl()
		{
			try
			{
				ValidationControlMaxLength(TxtKqCls, 4000, false);
				ValidationControlMaxLength(txtInternalMedicineState, 4000, false);
				ValidationControlMaxLength(TxtTreatmentTracking, 4000, false);
				ValidationControlMaxLength(TxtPrognosis, 4000, false);
				ValidationControlMaxLength(memCONCLUSION, 4000, false);
				ValidationControlMaxLength(memPathHis, 4000, false);
				ValidationControlMaxLength(memHosState, 2000, false);
				ValidationControlMaxLength(memBeforeDiagnostic, 2000, false);
				ValidationControlMaxLength(memDiscussion, 2000, false);
				ValidationControlMaxLength(memCareMethod, 2000, false);
				ValidationControlMaxLength(txtTreatmentMethod, 2000, false);
				ValidationControlMaxLength(memCONCLUSION, 2000, false);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidationControlMaxLength(BaseEdit control, int? maxLength, bool IsRequired)
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
				dxValidationProvider1.SetValidationRule(control, (ValidationRuleBase)(object)val);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void TxtMethodCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					string searchCode = (sender as TextEdit).Text.ToUpper();
					LoadMethod(searchCode, false);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadMethod(string searchCode, bool isExpand)
		{
			try
			{
				if (string.IsNullOrEmpty(searchCode))
				{
					CboMethod.Focus();
					CboMethod.ShowPopup();
					return;
				}
				List<HIS_PTTT_METHOD> list = ListPtttMethod.Where((HIS_PTTT_METHOD o) => o.PTTT_METHOD_CODE.Contains(searchCode)).ToList();
				if (list != null)
				{
					if (list.Count == 1)
					{
						CboMethod.EditValue = list[0].ID;
						checkEditMethod.Focus();
						checkEditMethod.SelectAll();
						return;
					}
					HIS_PTTT_METHOD val = list.FirstOrDefault((HIS_PTTT_METHOD m) => m.PTTT_METHOD_CODE == searchCode);
					if (val != null)
					{
						CboMethod.EditValue = val.ID;
						checkEditMethod.Focus();
						checkEditMethod.SelectAll();
					}
					else
					{
						CboMethod.EditValue = null;
						CboMethod.Focus();
						CboMethod.ShowPopup();
					}
				}
				else
				{
					CboMethod.EditValue = null;
					CboMethod.Focus();
					CboMethod.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CboMethod_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					CboMethod.EditValue = null;
					TxtMethodCode.Text = "";
					txtPtttMethod.Text = "";
					TxtMethodCode.Focus();
					TxtMethodCode.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CboMethod_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if ((e.CloseMode == PopupCloseMode.Normal || e.CloseMode == PopupCloseMode.Immediate) && CboMethod.EditValue != null)
				{
					HIS_PTTT_METHOD val = ListPtttMethod.FirstOrDefault((HIS_PTTT_METHOD o) => o.ID == Parse.ToInt64(CboMethod.EditValue.ToString()));
					if (val != null)
					{
						TxtMethodCode.Text = val.PTTT_METHOD_CODE;
						txtPtttMethod.Text = val.PTTT_METHOD_NAME;
						checkEditMethod.Focus();
						checkEditMethod.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CboMethod_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				CboMethod.ClosePopup();
				if (CboMethod.EditValue != null)
				{
					HIS_PTTT_METHOD val = ListPtttMethod.FirstOrDefault((HIS_PTTT_METHOD o) => o.ID == Parse.ToInt64((CboMethod.EditValue ?? ((object)0)).ToString()));
					if (val != null)
					{
						TxtMethodCode.Text = val.PTTT_METHOD_CODE;
						txtPtttMethod.Text = val.PTTT_METHOD_NAME;
						checkEditMethod.Focus();
						checkEditMethod.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void TxtEmotionlessMethod_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					string searchCode = (sender as TextEdit).Text.ToUpper();
					LoadEmotionlessMethod(searchCode, false);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadEmotionlessMethod(string searchCode, bool isExpand)
		{
			try
			{
				if (string.IsNullOrEmpty(searchCode))
				{
					CboEmotionlessMethod.Focus();
					CboEmotionlessMethod.ShowPopup();
					return;
				}
				List<HIS_EMOTIONLESS_METHOD> list = ListEmotionMethod.Where((HIS_EMOTIONLESS_METHOD o) => o.EMOTIONLESS_METHOD_CODE.ToUpper().Contains(searchCode)).ToList();
				if (list != null)
				{
					if (list.Count == 1)
					{
						CboEmotionlessMethod.EditValue = list[0].ID;
						DtSurgeryTime.Focus();
						DtSurgeryTime.SelectAll();
						return;
					}
					HIS_EMOTIONLESS_METHOD val = list.FirstOrDefault((HIS_EMOTIONLESS_METHOD m) => m.EMOTIONLESS_METHOD_CODE.ToUpper() == searchCode);
					if (val != null)
					{
						CboEmotionlessMethod.EditValue = val.ID;
						DtSurgeryTime.Focus();
						DtSurgeryTime.SelectAll();
					}
					else
					{
						CboEmotionlessMethod.EditValue = null;
						CboEmotionlessMethod.Focus();
						CboEmotionlessMethod.ShowPopup();
					}
				}
				else
				{
					CboEmotionlessMethod.EditValue = null;
					CboEmotionlessMethod.Focus();
					CboEmotionlessMethod.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CboEmotionlessMethod_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					CboEmotionlessMethod.EditValue = null;
					TxtEmotionlessMethod.Text = "";
					TxtEmotionlessMethod.Focus();
					TxtEmotionlessMethod.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CboEmotionlessMethod_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal && CboEmotionlessMethod.EditValue != null)
				{
					HIS_EMOTIONLESS_METHOD val = ListEmotionMethod.FirstOrDefault((HIS_EMOTIONLESS_METHOD o) => o.ID == Parse.ToInt64(CboEmotionlessMethod.EditValue.ToString()));
					if (val != null)
					{
						TxtEmotionlessMethod.Text = val.EMOTIONLESS_METHOD_CODE;
						DtSurgeryTime.Focus();
						DtSurgeryTime.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CboEmotionlessMethod_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					if (CboEmotionlessMethod.EditValue != null)
					{
						HIS_EMOTIONLESS_METHOD val = ListEmotionMethod.FirstOrDefault((HIS_EMOTIONLESS_METHOD o) => o.ID == Parse.ToInt64(CboEmotionlessMethod.EditValue.ToString()));
						if (val != null)
						{
							TxtEmotionlessMethod.Text = val.EMOTIONLESS_METHOD_CODE;
							DtSurgeryTime.Focus();
							DtSurgeryTime.SelectAll();
						}
					}
				}
				else
				{
					CboEmotionlessMethod.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void DtSurgeryTime_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (DtSurgeryTime.EditValue != null)
				{
					DateTime dateTime = DtSurgeryTime.DateTime;
					DtSurgeryTime.DateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
					TxtPrognosis.Focus();
					TxtPrognosis.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void DtSurgeryTime_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					TxtPrognosis.Focus();
					TxtPrognosis.SelectAll();
				}
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

		private void BtnChooseCls_Click(object sender, EventArgs e)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			try
			{
				List<object> list = new List<object>();
				list.Add(treatmentId);
				list.Add((object)new DelegateSelectData(SelectDataResult));
				list.Add(true);
				PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.ContentSubclinical", _roomId, _roomTypeId, list);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SelectDataResult(object data)
		{
			try
			{
				LogSystem.Debug(LogUtil.TraceData("SelectDataResult: ", data));
				TxtKqCls.Text = "";
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
				TxtKqCls.Text = string.Join("\r\n", list);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private string GetSpaceByName(string p)
		{
			string text = " ";
			try
			{
				if (!string.IsNullOrWhiteSpace(p))
				{
					for (int i = 0; i < p.Length; i++)
					{
						text += " ";
					}
				}
			}
			catch (Exception ex)
			{
				text = " ";
				LogSystem.Error(ex);
			}
			return text;
		}

		private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			try
			{
                HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO hisEkipUserADO = (HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO)grdViewInformationSurg.GetFocusedRow();
				if (e.Column.FieldName == "LOGINNAME")
				{
					grdControlInformationSurg.RefreshDataSource();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridView1_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			try
			{
				if (e.Column.FieldName == "BtnDelete")
				{
					int num = System.Convert.ToInt32(e.RowHandle);
					switch (Parse.ToInt32((grdViewInformationSurg.GetRowCellValue(e.RowHandle, "Action") ?? "").ToString()))
					{
					case 1:
						e.RepositoryItem = btnAdd;
						break;
					case 2:
						e.RepositoryItem = btnDelete;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void grdViewInformationSurg_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (!e.IsGetData)
				{
					return;
				}
				GridView gridView = sender as GridView;
				if (!(e.Column.FieldName == "USERNAME"))
				{
					return;
				}
				try
				{
					string status = (gridView.GetRowCellValue(e.ListSourceRowIndex, "LOGINNAME") ?? "").ToString();
					ACS_USER val = BackendDataWorker.Get<ACS_USER>().SingleOrDefault((ACS_USER o) => o.LOGINNAME == status);
					e.Value = val.USERNAME;
				}
				catch (Exception ex)
				{
					LogSystem.Warn("Loi hien thi gia tri cot USERNAME", ex);
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}

		private void grdViewInformationSurg_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
		{
			try
			{
				if (e.FocusedColumn.FieldName == "LOGINNAME")
				{
					grdViewInformationSurg.ShowEditor();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void grdViewInformationSurg_ShowingEditor(object sender, CancelEventArgs e)
		{
			try
			{
				GridView gridView = sender as GridView;
				HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO data = gridView.GetFocusedRow() as HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO;
				if (!(gridView.FocusedColumn.FieldName == "LOGINNAME") || !(gridView.ActiveEditor is CustomGridLookUpEdit))
				{
					return;
				}
				BaseEdit activeEditor = gridView.ActiveEditor;
				CustomGridLookUpEdit val = (CustomGridLookUpEdit)(object)((activeEditor is CustomGridLookUpEdit) ? activeEditor : null);
				List<string> loginNames = new List<string>();
				if (data != null && ((V_HIS_EKIP_USER)data).EXECUTE_ROLE_ID > 0)
				{
					if (((V_HIS_EKIP_USER)data).LOGINNAME != null)
					{
						((BaseEdit)(object)val).EditValue = ((V_HIS_EKIP_USER)data).LOGINNAME;
					}
					List<HIS_EXECUTE_ROLE_USER> list = ((executeRoleUsers != null) ? executeRoleUsers.Where((HIS_EXECUTE_ROLE_USER o) => o.EXECUTE_ROLE_ID == ((V_HIS_EKIP_USER)data).EXECUTE_ROLE_ID).ToList() : null);
					if (list != null && list.Count > 0)
					{
						loginNames = list.Select((HIS_EXECUTE_ROLE_USER o) => o.LOGINNAME).Distinct().ToList();
					}
				}
				ComboAcsUser(val, loginNames);
				grdViewInformationSurg.RefreshData();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ComboAcsUser(CustomGridLookUpEdit cbo, List<string> loginNames)
		{
			try
			{
				List<AcsUserADO> list = new List<AcsUserADO>();
				list = ((loginNames == null || loginNames.Count <= 0) ? AcsUserADOList : AcsUserADOList.Where((AcsUserADO o) => loginNames.Contains(((ACS_USER)o).LOGINNAME)).ToList());
				((RepositoryItemLookUpEditBase)(object)cbo.Properties).DataSource = list;
				((RepositoryItemLookUpEditBase)(object)cbo.Properties).DisplayMember = "USERNAME";
				((RepositoryItemLookUpEditBase)(object)cbo.Properties).ValueMember = "LOGINNAME";
				((RepositoryItemButtonEdit)(object)cbo.Properties).TextEditStyle = TextEditStyles.Standard;
				((RepositoryItemGridLookUpEditBase)(object)cbo.Properties).PopupFilterMode = PopupFilterMode.Contains;
				((RepositoryItemPopupBaseAutoSearchEdit)(object)cbo.Properties).ImmediatePopup = true;
				((GridLookUpEditBase)(object)cbo).ForceInitialize();
				((RepositoryItemGridLookUpEditBase)(object)cbo.Properties).View.Columns.Clear();
				GridColumn gridColumn = ((RepositoryItemGridLookUpEditBase)(object)cbo.Properties).View.Columns.AddField("LOGINNAME");
				gridColumn.Caption = "Tài khoản";
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 100;
				GridColumn gridColumn2 = ((RepositoryItemGridLookUpEditBase)(object)cbo.Properties).View.Columns.AddField("USERNAME");
				gridColumn2.Caption = "Họ tên";
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 2;
				gridColumn2.Width = 200;
				GridColumn gridColumn3 = ((RepositoryItemGridLookUpEditBase)(object)cbo.Properties).View.Columns.AddField("DEPARTMENT_NAME");
				gridColumn3.Caption = "Tên khoa";
				gridColumn3.Visible = true;
				gridColumn3.VisibleIndex = 3;
				gridColumn3.Width = 200;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnAdd_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO> list = new List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO>();
				List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO> list2 = grdControlInformationSurg.DataSource as List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO>;
				if (list2 == null || list2.Count < 1)
				{
					HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO hisEkipUserADO = new HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO();
					hisEkipUserADO.Action = 2;
					list.Add(hisEkipUserADO);
					grdControlInformationSurg.DataSource = null;
					grdControlInformationSurg.DataSource = list;
				}
				else
				{
					HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO hisEkipUserADO2 = new HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO();
					hisEkipUserADO2.Action = 2;
					list2.Add(hisEkipUserADO2);
					grdControlInformationSurg.DataSource = null;
					grdControlInformationSurg.DataSource = list2;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			try
			{
				CommonParam val = new CommonParam();
				List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO> list = grdControlInformationSurg.DataSource as List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO>;
				HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO hisEkipUserADO = (HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO)grdViewInformationSurg.GetFocusedRow();
				if (hisEkipUserADO != null && list.Count > 0)
				{
					list.Remove(hisEkipUserADO);
					grdControlInformationSurg.DataSource = null;
					grdControlInformationSurg.DataSource = list;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cbo_UseName_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				grdViewInformationSurg.FocusedRowHandle = -2147483647;
				grdViewInformationSurg.FocusedColumn = grdViewInformationSurg.VisibleColumns[2];
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void WarningValidMessage()
		{
			try
			{
				IList<Control> invalidControls = dxValidationProvider1.GetInvalidControls();
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
						if (item.Name == txtInternalMedicineState.Name)
						{
							list.Add(xtraTabPage5.Text);
						}
						else if (item.Name == TxtTreatmentTracking.Name)
						{
							list.Add(xtraTabPage10.Text);
						}
						else if (item.Name == TxtPrognosis.Name)
						{
							list.Add(xtraTabPage2.Text);
						}
						else if (item.Name == memCONCLUSION.Name)
						{
							list.Add(xtraTabPage9.Text);
						}
						else if (item.Name == memPathHis.Name)
						{
							list.Add(xtraTabPage3.Text);
						}
						else if (item.Name == memHosState.Name)
						{
							list.Add(xtraTabPage4.Text);
						}
						else if (item.Name == memBeforeDiagnostic.Name)
						{
							list.Add(xtraTabPage6.Text);
						}
						else if (item.Name == memDiscussion.Name)
						{
							list.Add(xtraTabPage1.Text);
						}
						else if (item.Name == memCareMethod.Name)
						{
							list.Add(xtraTabPage8.Text);
						}
						else if (item.Name == txtTreatmentMethod.Name)
						{
							list.Add(xtraTabPage7.Text);
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

		internal bool ValidControl()
		{
			bool flag = false;
			try
			{
				positionHandleControl = -1;
				flag = !dxValidationProvider1.Validate();
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
				TxtEmotionlessMethod.ReadOnly = true;
				txtInternalMedicineState.ReadOnly = true;
				TxtKqCls.ReadOnly = true;
				TxtTreatmentTracking.ReadOnly = true;
				TxtMethodCode.ReadOnly = true;
				TxtPrognosis.ReadOnly = true;
				txtTreatmentMethod.ReadOnly = true;
				memCONCLUSION.ReadOnly = true;
				CboEmotionlessMethod.ReadOnly = true;
				CboEkipTemp.ReadOnly = true;
				CboMethod.ReadOnly = true;
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
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Expected O, but got Unknown
			try
			{
				if (saveData == null)
				{
					saveData = new HIS_DEBATE();
				}
				saveData.SUBCLINICAL_PROCESSES = TxtKqCls.Text;
				saveData.INTERNAL_MEDICINE_STATE = txtInternalMedicineState.Text;
				saveData.TREATMENT_TRACKING = TxtTreatmentTracking.Text;
				saveData.TREATMENT_METHOD = txtTreatmentMethod.Text;
				saveData.PTTT_METHOD_ID = null;
				if (CboMethod.EditValue != null)
				{
					saveData.PTTT_METHOD_ID = long.Parse(CboMethod.EditValue.ToString());
				}
				if (checkEditMethod.Checked)
				{
					saveData.PTTT_METHOD_NAME = txtPtttMethod.Text;
				}
				else
				{
					saveData.PTTT_METHOD_NAME = CboMethod.Text;
				}
				saveData.EMOTIONLESS_METHOD_ID = null;
				if (CboEmotionlessMethod.EditValue != null)
				{
					saveData.EMOTIONLESS_METHOD_ID = long.Parse(CboEmotionlessMethod.EditValue.ToString());
				}
				saveData.SURGERY_TIME = null;
				if (DtSurgeryTime.EditValue != null && DtSurgeryTime.DateTime != DateTime.MinValue)
				{
					saveData.SURGERY_TIME = Parse.ToInt64(System.Convert.ToDateTime((DtSurgeryTime.EditValue ?? "").ToString()).ToString("yyyyMMddHHmm") + "00");
				}
				saveData.PROGNOSIS = TxtPrognosis.Text;
				if (!string.IsNullOrEmpty(memCONCLUSION.Text))
				{
					saveData.CONCLUSION = memCONCLUSION.Text;
				}
				List<HIS_DEBATE_EKIP_USER> list = new List<HIS_DEBATE_EKIP_USER>();
				List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO> list2 = grdControlInformationSurg.DataSource as List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO>;
				foreach (HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO item_DebateUser in list2)
				{
					HIS_DEBATE_EKIP_USER val = new HIS_DEBATE_EKIP_USER();
					DataObjectMapper.Map<HIS_DEBATE_EKIP_USER>((object)val, (object)item_DebateUser);
					ACS_USER val2 = GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((V_HIS_EKIP_USER)item_DebateUser).LOGINNAME);
					if (val2 != null)
					{
						val.LOGINNAME = val2.LOGINNAME;
						if (!string.IsNullOrEmpty(val2.USERNAME))
						{
							val.USERNAME = val2.USERNAME;
						}
						list.Add(val);
					}
				}
				if (list.Count > 0)
				{
					saveData.HIS_DEBATE_EKIP_USER = list;
				}
				else
				{
					saveData.HIS_DEBATE_EKIP_USER = null;
				}
				saveData.PATHOLOGICAL_HISTORY = memPathHis.Text.Trim();
				saveData.HOSPITALIZATION_STATE = memHosState.Text.Trim();
				saveData.BEFORE_DIAGNOSTIC = memBeforeDiagnostic.Text.Trim();
				saveData.DISCUSSION = memDiscussion.Text.Trim();
				saveData.CARE_METHOD = memCareMethod.Text.Trim();
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

		private void LoadDataDebateDiagnostic(HIS_SERVICE_REQ hisDebate)
		{
			try
			{
				memPathHis.Text = hisDebate.PATHOLOGICAL_HISTORY;
				string text = hisDebate.FULL_EXAM + "\r\n" + hisDebate.PART_EXAM + "\r\n" + hisDebate.SUBCLINICAL;
				memHosState.Text = text.Trim();
				TxtTreatmentTracking.Text = hisDebate.PATHOLOGICAL_PROCESS;
				txtTreatmentMethod.Text = hisDebate.TREATMENT_INSTRUCTION;
				memCONCLUSION.Text = hisDebate.NEXT_TREATMENT_INSTRUCTION;
				if (!string.IsNullOrEmpty(hisDebate.ICD_CODE))
				{
					HIS_ICD val = GlobalStore.HisIcds.Where((HIS_ICD o) => o.ICD_CODE == hisDebate.ICD_CODE).FirstOrDefault();
					memBeforeDiagnostic.Text = val.ICD_NAME;
				}
				else
				{
					memBeforeDiagnostic.Text = "";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDataDebateDiagnostic(HIS_DEBATE hisDebate)
		{
			try
			{
				if (hisDebate == null)
				{
					return;
				}
				memPathHis.Text = hisDebate.PATHOLOGICAL_HISTORY;
				memHosState.Text = hisDebate.HOSPITALIZATION_STATE;
				memBeforeDiagnostic.Text = hisDebate.BEFORE_DIAGNOSTIC;
				memDiscussion.Text = hisDebate.DISCUSSION;
				memCareMethod.Text = hisDebate.CARE_METHOD;
				TxtKqCls.Text = hisDebate.SUBCLINICAL_PROCESSES;
				txtInternalMedicineState.Text = hisDebate.INTERNAL_MEDICINE_STATE;
				TxtTreatmentTracking.Text = hisDebate.TREATMENT_TRACKING;
				TxtPrognosis.Text = hisDebate.PROGNOSIS;
				txtTreatmentMethod.Text = hisDebate.TREATMENT_METHOD;
				if (!string.IsNullOrEmpty(hisDebate.CONCLUSION))
				{
					memCONCLUSION.Text = hisDebate.CONCLUSION;
				}
				memCONCLUSION.Text = hisDebate.CONCLUSION;
				DtSurgeryTime.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(hisDebate.SURGERY_TIME.GetValueOrDefault());
				CboMethod.EditValue = hisDebate.PTTT_METHOD_ID;
				HIS_PTTT_METHOD val = ListPtttMethod.FirstOrDefault((HIS_PTTT_METHOD o) => o.ID == hisDebate.PTTT_METHOD_ID);
				if (val != null)
				{
					TxtMethodCode.Text = val.PTTT_METHOD_CODE;
					if ((hisDebate.PTTT_METHOD_NAME ?? " ").ToLower() != (val.PTTT_METHOD_CODE ?? " ").ToLower())
					{
						checkEditMethod.Checked = true;
						txtPtttMethod.Text = hisDebate.PTTT_METHOD_NAME;
					}
					else
					{
						checkEditMethod.Checked = false;
					}
				}
				CboEmotionlessMethod.EditValue = hisDebate.EMOTIONLESS_METHOD_ID;
				HIS_EMOTIONLESS_METHOD val2 = ListEmotionMethod.FirstOrDefault((HIS_EMOTIONLESS_METHOD o) => o.ID == hisDebate.EMOTIONLESS_METHOD_ID);
				if (val2 != null)
				{
					TxtEmotionlessMethod.Text = val2.EMOTIONLESS_METHOD_CODE;
				}
				LoadGridEkipUserFromDebate(hisDebate.ID);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadGridEkipUserFromDebate(long debateId)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				CommonParam val = new CommonParam();
				HisDebateEkipUserFilter val2 = new HisDebateEkipUserFilter();
				val2.DEBATE_ID = debateId;
				List<HIS_DEBATE_EKIP_USER> list = ((AdapterBase)new BackendAdapter(val)).Get<List<HIS_DEBATE_EKIP_USER>>("api/HisDebateEkipUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, val);
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO> list2 = new List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO>();
				foreach (HIS_DEBATE_EKIP_USER ekipTempUser in list)
				{
					HIS_EXECUTE_ROLE val3 = BackendDataWorker.Get<HIS_EXECUTE_ROLE>().FirstOrDefault((HIS_EXECUTE_ROLE p) => p.ID == ekipTempUser.EXECUTE_ROLE_ID && p.IS_ACTIVE == 1);
					if (val3 != null && val3.ID != 0)
					{
						HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO hisEkipUserADO = new HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO();
						((V_HIS_EKIP_USER)hisEkipUserADO).EXECUTE_ROLE_ID = ekipTempUser.EXECUTE_ROLE_ID;
						((V_HIS_EKIP_USER)hisEkipUserADO).LOGINNAME = ekipTempUser.LOGINNAME;
						((V_HIS_EKIP_USER)hisEkipUserADO).USERNAME = ekipTempUser.USERNAME;
						((V_HIS_EKIP_USER)hisEkipUserADO).DEPARTMENT_ID = ekipTempUser.DEPARTMENT_ID;
						if (list2.Count == 0)
						{
							hisEkipUserADO.Action = 1;
						}
						else
						{
							hisEkipUserADO.Action = 2;
						}
						list2.Add(hisEkipUserADO);
					}
				}
				grdControlInformationSurg.DataSource = list2;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadGridEkipUserFromTemp(long ekipTempId)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				CommonParam val = new CommonParam();
				HisEkipTempUserFilter val2 = new HisEkipTempUserFilter();
				val2.EKIP_TEMP_ID = ekipTempId;
				List<HIS_EKIP_TEMP_USER> list = ((AdapterBase)new BackendAdapter(val)).Get<List<HIS_EKIP_TEMP_USER>>("api/HisEkipTempUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, val);
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO> list2 = new List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO>();
				foreach (HIS_EKIP_TEMP_USER ekipTempUser in list)
				{
					HIS_EXECUTE_ROLE val3 = BackendDataWorker.Get<HIS_EXECUTE_ROLE>().FirstOrDefault((HIS_EXECUTE_ROLE p) => p.ID == ekipTempUser.EXECUTE_ROLE_ID && p.IS_ACTIVE == 1);
					if (val3 != null && val3.ID != 0)
					{
						HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO hisEkipUserADO = new HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO();
						((V_HIS_EKIP_USER)hisEkipUserADO).EXECUTE_ROLE_ID = ekipTempUser.EXECUTE_ROLE_ID;
						((V_HIS_EKIP_USER)hisEkipUserADO).LOGINNAME = ekipTempUser.LOGINNAME;
						((V_HIS_EKIP_USER)hisEkipUserADO).USERNAME = ekipTempUser.USERNAME;
						if (list2.Count == 0)
						{
							hisEkipUserADO.Action = 1;
						}
						else
						{
							hisEkipUserADO.Action = 2;
						}
						list2.Add(hisEkipUserADO);
					}
				}
				grdControlInformationSurg.DataSource = list2;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal async Task FillDataToInformationSurg()
		{
			try
			{
				List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO> ekipUserAdoTemps = new List<HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO>();
				Action myaction = delegate
				{
					string text = HisConfigs.Get<string>("HIS.DESKTOP.PLUGINS.SURGSERVICEREQEXECUTE.EXECUTE_ROLE_DEFAULT");
					List<HIS_EXECUTE_ROLE> executeRoleList = ExecuteRoleList;
					if (!string.IsNullOrEmpty(text))
					{
						string[] collection = text.Split(',');
						List<string> list = new List<string>(collection);
						foreach (string item in list)
						{
							HIS_EXECUTE_ROLE executeRoleCheck = executeRoleList.FirstOrDefault((HIS_EXECUTE_ROLE o) => o.EXECUTE_ROLE_CODE == item);
							if (executeRoleCheck != null)
							{
								HIS_EXECUTE_ROLE val = BackendDataWorker.Get<HIS_EXECUTE_ROLE>().FirstOrDefault((HIS_EXECUTE_ROLE p) => p.ID == executeRoleCheck.ID && p.IS_ACTIVE == 1);
								if (val != null && val.ID != 0)
								{
									HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO hisEkipUserADO = new HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO();
									((V_HIS_EKIP_USER)hisEkipUserADO).EXECUTE_ROLE_ID = executeRoleCheck.ID;
									if (ekipUserAdoTemps.Count == 0)
									{
										hisEkipUserADO.Action = 1;
									}
									else
									{
										hisEkipUserADO.Action = 2;
									}
									ekipUserAdoTemps.Add(hisEkipUserADO);
								}
							}
						}
					}
					else
					{
						HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO item2 = new HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO
						{
							Action = 1
						};
						ekipUserAdoTemps.Add(item2);
					}
					if (ekipUserAdoTemps == null || ekipUserAdoTemps.Count == 0)
					{
						HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO item3 = new HIS.Desktop.Plugins.DebateDiagnostic.ADO.HisEkipUserADO
						{
							Action = 1
						};
						ekipUserAdoTemps.Add(item3);
					}
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				if (grdControlInformationSurg.DataSource == null)
				{
					grdControlInformationSurg.DataSource = ekipUserAdoTemps;
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private void CboEkipTemp_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal && CboEkipTemp.EditValue != null)
				{
					HIS_EKIP_TEMP val = ekipTemps.FirstOrDefault((HIS_EKIP_TEMP o) => o.ID == Parse.ToInt64((CboEkipTemp.EditValue ?? ((object)0)).ToString()));
					if (val != null)
					{
						LoadGridEkipUserFromTemp(val.ID);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CboEkipTemp_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				try
				{
					if (e.Button.Kind == ButtonPredefines.Delete)
					{
						CboEkipTemp.EditValue = null;
					}
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}

		private void CboMethod_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(CboMethod.Text))
				{
					CboMethod.EditValue = null;
					txtPtttMethod.Text = "";
					checkEditMethod.Checked = false;
				}
				else
				{
					txtPtttMethod.Text = CboMethod.Text;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void checkEditMethod_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (checkEditMethod.Checked)
				{
					TxtMethodCode.Enabled = false;
					CboMethod.Visible = false;
					txtPtttMethod.Visible = true;
					txtPtttMethod.Text = CboMethod.Text;
					txtPtttMethod.Focus();
					txtPtttMethod.SelectAll();
				}
				else if (!checkEditMethod.Checked)
				{
					TxtMethodCode.Enabled = true;
					txtPtttMethod.Visible = false;
					CboMethod.Visible = true;
					txtPtttMethod.Text = CboMethod.Text;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void checkEditMethod_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					TxtEmotionlessMethod.Focus();
					TxtEmotionlessMethod.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtPtttMethod_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Tab)
				{
					checkEditMethod.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void TxtPrognosis_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtTreatmentMethod.Focus();
					txtTreatmentMethod.SelectAll();
				}
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
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Expected O, but got Unknown
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Expected O, but got Unknown
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Expected O, but got Unknown
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Expected O, but got Unknown
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
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.layoutControl3 = new DevExpress.XtraLayout.LayoutControl();
			this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
			this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
			this.memDiscussion = new DevExpress.XtraEditors.MemoEdit();
			this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
			this.TxtPrognosis = new DevExpress.XtraEditors.MemoEdit();
			this.xtraTabPage7 = new DevExpress.XtraTab.XtraTabPage();
			this.txtTreatmentMethod = new DevExpress.XtraEditors.MemoEdit();
			this.xtraTabPage8 = new DevExpress.XtraTab.XtraTabPage();
			this.memCareMethod = new DevExpress.XtraEditors.MemoEdit();
			this.xtraTabPage9 = new DevExpress.XtraTab.XtraTabPage();
			this.memCONCLUSION = new DevExpress.XtraEditors.MemoEdit();
			this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
			this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
			this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
			this.memPathHis = new DevExpress.XtraEditors.MemoEdit();
			this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
			this.memHosState = new DevExpress.XtraEditors.MemoEdit();
			this.xtraTabPage5 = new DevExpress.XtraTab.XtraTabPage();
			this.txtInternalMedicineState = new DevExpress.XtraEditors.MemoEdit();
			this.xtraTabPage6 = new DevExpress.XtraTab.XtraTabPage();
			this.memBeforeDiagnostic = new DevExpress.XtraEditors.MemoEdit();
			this.xtraTabPage10 = new DevExpress.XtraTab.XtraTabPage();
			this.TxtTreatmentTracking = new DevExpress.XtraEditors.MemoEdit();
			this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
			this.checkEditMethod = new DevExpress.XtraEditors.CheckEdit();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.CboMethod = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.txtPtttMethod = new DevExpress.XtraEditors.TextEdit();
			this.DtSurgeryTime = new DevExpress.XtraEditors.DateEdit();
			this.CboEmotionlessMethod = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.TxtEmotionlessMethod = new DevExpress.XtraEditors.TextEdit();
			this.TxtMethodCode = new DevExpress.XtraEditors.TextEdit();
			this.BtnSaveEkipTemp = new DevExpress.XtraEditors.SimpleButton();
			this.CboEkipTemp = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.TxtKqCls = new DevExpress.XtraEditors.MemoEdit();
			this.BtnChooseCls = new DevExpress.XtraEditors.SimpleButton();
			this.grdControlInformationSurg = new DevExpress.XtraGrid.GridControl();
			this.grdViewInformationSurg = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.cbo_UseName = new RepositoryItemCustomGridLookUpEdit();
			this.repositoryItemCustomGridLookUpEditNew1View = new CustomGridView();
			this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.cboPosition = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
			this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.GridLookUpEdit_Department = new RepositoryItemCustomGridLookUpEdit();
			this.customGridView1 = new CustomGridView();
			this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.btnAdd = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.txtLogin = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.btnDelete = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.repositoryItemGridLookUpUsername = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
			this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.repositoryItemSearchLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
			this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.LciKqCls = new DevExpress.XtraLayout.LayoutControlItem();
			this.LciMethodCode = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.LciSurgeryTime = new DevExpress.XtraLayout.LayoutControlItem();
			this.LciEkipTemp = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciInformationSurg = new DevExpress.XtraLayout.LayoutControlItem();
			this.LciEmotionlessMethod = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
			this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.layoutControl3).BeginInit();
			this.layoutControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
			this.xtraTabControl1.SuspendLayout();
			this.xtraTabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.memDiscussion.Properties).BeginInit();
			this.xtraTabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.TxtPrognosis.Properties).BeginInit();
			this.xtraTabPage7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtTreatmentMethod.Properties).BeginInit();
			this.xtraTabPage8.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.memCareMethod.Properties).BeginInit();
			this.xtraTabPage9.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.memCONCLUSION.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.layoutControl2).BeginInit();
			this.layoutControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
			this.xtraTabControl2.SuspendLayout();
			this.xtraTabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.memPathHis.Properties).BeginInit();
			this.xtraTabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.memHosState.Properties).BeginInit();
			this.xtraTabPage5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtInternalMedicineState.Properties).BeginInit();
			this.xtraTabPage6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.memBeforeDiagnostic.Properties).BeginInit();
			this.xtraTabPage10.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.TxtTreatmentTracking.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.checkEditMethod.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.CboMethod.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtPtttMethod.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.DtSurgeryTime.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.DtSurgeryTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.CboEmotionlessMethod.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit2View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.TxtEmotionlessMethod.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.TxtMethodCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.CboEkipTemp.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.TxtKqCls.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.grdControlInformationSurg).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.grdViewInformationSurg).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cbo_UseName).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemCustomGridLookUpEditNew1View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboPosition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.GridLookUpEdit_Department).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.customGridView1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.btnAdd).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtLogin).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.btnDelete).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpUsername).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemSearchLookUpEdit1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.LciKqCls).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.LciMethodCode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.LciSurgeryTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.LciEkipTemp).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciInformationSurg).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.LciEmotionlessMethod).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.groupBox2);
			this.layoutControl1.Controls.Add(this.groupBox1);
			this.layoutControl1.Controls.Add(this.checkEditMethod);
			this.layoutControl1.Controls.Add(this.panelControl1);
			this.layoutControl1.Controls.Add(this.DtSurgeryTime);
			this.layoutControl1.Controls.Add(this.CboEmotionlessMethod);
			this.layoutControl1.Controls.Add(this.TxtEmotionlessMethod);
			this.layoutControl1.Controls.Add(this.TxtMethodCode);
			this.layoutControl1.Controls.Add(this.BtnSaveEkipTemp);
			this.layoutControl1.Controls.Add(this.CboEkipTemp);
			this.layoutControl1.Controls.Add(this.TxtKqCls);
			this.layoutControl1.Controls.Add(this.BtnChooseCls);
			this.layoutControl1.Controls.Add(this.grdControlInformationSurg);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(807, 500);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.groupBox2.Controls.Add(this.layoutControl3);
			this.groupBox2.Location = new System.Drawing.Point(2, 349);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(803, 149);
			this.groupBox2.TabIndex = 43;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Nội dung kết luận";
			this.layoutControl3.Controls.Add(this.xtraTabControl1);
			this.layoutControl3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl3.Location = new System.Drawing.Point(3, 16);
			this.layoutControl3.Name = "layoutControl3";
			this.layoutControl3.Root = this.layoutControlGroup3;
			this.layoutControl3.Size = new System.Drawing.Size(797, 130);
			this.layoutControl3.TabIndex = 0;
			this.layoutControl3.Text = "layoutControl3";
			this.xtraTabControl1.Location = new System.Drawing.Point(2, 2);
			this.xtraTabControl1.Name = "xtraTabControl1";
			this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
			this.xtraTabControl1.Size = new System.Drawing.Size(793, 126);
			this.xtraTabControl1.TabIndex = 4;
			this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[5] { this.xtraTabPage1, this.xtraTabPage2, this.xtraTabPage7, this.xtraTabPage8, this.xtraTabPage9 });
			this.xtraTabPage1.Controls.Add(this.memDiscussion);
			this.xtraTabPage1.Name = "xtraTabPage1";
			this.xtraTabPage1.Size = new System.Drawing.Size(787, 98);
			this.xtraTabPage1.Text = "Ý kiến thảo luận";
			this.memDiscussion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memDiscussion.Location = new System.Drawing.Point(0, 0);
			this.memDiscussion.Name = "memDiscussion";
			this.memDiscussion.Size = new System.Drawing.Size(787, 98);
			this.memDiscussion.TabIndex = 0;
			this.xtraTabPage2.Controls.Add(this.TxtPrognosis);
			this.xtraTabPage2.Name = "xtraTabPage2";
			this.xtraTabPage2.Size = new System.Drawing.Size(787, 98);
			this.xtraTabPage2.Text = "Dự trù tiên lượng";
			this.TxtPrognosis.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TxtPrognosis.Location = new System.Drawing.Point(0, 0);
			this.TxtPrognosis.Name = "TxtPrognosis";
			this.TxtPrognosis.Size = new System.Drawing.Size(787, 98);
			this.TxtPrognosis.TabIndex = 0;
			this.xtraTabPage7.Controls.Add(this.txtTreatmentMethod);
			this.xtraTabPage7.Name = "xtraTabPage7";
			this.xtraTabPage7.Size = new System.Drawing.Size(787, 98);
			this.xtraTabPage7.Text = "Hướng điều trị tiếp";
			this.txtTreatmentMethod.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTreatmentMethod.Location = new System.Drawing.Point(0, 0);
			this.txtTreatmentMethod.Name = "txtTreatmentMethod";
			this.txtTreatmentMethod.Size = new System.Drawing.Size(787, 98);
			this.txtTreatmentMethod.TabIndex = 0;
			this.xtraTabPage8.Controls.Add(this.memCareMethod);
			this.xtraTabPage8.Name = "xtraTabPage8";
			this.xtraTabPage8.Size = new System.Drawing.Size(787, 98);
			this.xtraTabPage8.Text = "Chăm sóc";
			this.memCareMethod.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memCareMethod.Location = new System.Drawing.Point(0, 0);
			this.memCareMethod.Name = "memCareMethod";
			this.memCareMethod.Size = new System.Drawing.Size(787, 98);
			this.memCareMethod.TabIndex = 0;
			this.xtraTabPage9.Controls.Add(this.memCONCLUSION);
			this.xtraTabPage9.Name = "xtraTabPage9";
			this.xtraTabPage9.Size = new System.Drawing.Size(787, 98);
			this.xtraTabPage9.Text = "Kết luận";
			this.memCONCLUSION.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memCONCLUSION.Location = new System.Drawing.Point(0, 0);
			this.memCONCLUSION.Name = "memCONCLUSION";
			this.memCONCLUSION.Size = new System.Drawing.Size(787, 98);
			this.memCONCLUSION.TabIndex = 0;
			this.layoutControlGroup3.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.False;
			this.layoutControlGroup3.GroupBordersVisible = false;
			this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[1] { this.layoutControlItem10 });
			this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup3.Name = "layoutControlGroup3";
			this.layoutControlGroup3.Size = new System.Drawing.Size(797, 130);
			this.layoutControlGroup3.TextVisible = false;
			this.layoutControlItem10.Control = this.xtraTabControl1;
			this.layoutControlItem10.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem10.Name = "layoutControlItem10";
			this.layoutControlItem10.Size = new System.Drawing.Size(797, 130);
			this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem10.TextVisible = false;
			this.groupBox1.Controls.Add(this.layoutControl2);
			this.groupBox1.Location = new System.Drawing.Point(2, 189);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(803, 156);
			this.groupBox1.TabIndex = 42;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Diễn biến bệnh";
			this.layoutControl2.Controls.Add(this.xtraTabControl2);
			this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl2.Location = new System.Drawing.Point(3, 16);
			this.layoutControl2.Name = "layoutControl2";
			this.layoutControl2.Root = this.layoutControlGroup2;
			this.layoutControl2.Size = new System.Drawing.Size(797, 137);
			this.layoutControl2.TabIndex = 0;
			this.layoutControl2.Text = "layoutControl2";
			this.xtraTabControl2.Location = new System.Drawing.Point(2, 2);
			this.xtraTabControl2.Name = "xtraTabControl2";
			this.xtraTabControl2.SelectedTabPage = this.xtraTabPage3;
			this.xtraTabControl2.Size = new System.Drawing.Size(793, 133);
			this.xtraTabControl2.TabIndex = 4;
			this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[5] { this.xtraTabPage3, this.xtraTabPage4, this.xtraTabPage5, this.xtraTabPage6, this.xtraTabPage10 });
			this.xtraTabPage3.Controls.Add(this.memPathHis);
			this.xtraTabPage3.Name = "xtraTabPage3";
			this.xtraTabPage3.Size = new System.Drawing.Size(787, 105);
			this.xtraTabPage3.Text = "Tóm tắt tiền sử bệnh";
			this.memPathHis.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memPathHis.Location = new System.Drawing.Point(0, 0);
			this.memPathHis.Name = "memPathHis";
			this.memPathHis.Size = new System.Drawing.Size(787, 105);
			this.memPathHis.TabIndex = 0;
			this.xtraTabPage4.Controls.Add(this.memHosState);
			this.xtraTabPage4.Name = "xtraTabPage4";
			this.xtraTabPage4.Size = new System.Drawing.Size(787, 105);
			this.xtraTabPage4.Text = "Tình trạng vào viện";
			this.memHosState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memHosState.Location = new System.Drawing.Point(0, 0);
			this.memHosState.Name = "memHosState";
			this.memHosState.Size = new System.Drawing.Size(787, 105);
			this.memHosState.TabIndex = 0;
			this.xtraTabPage5.Controls.Add(this.txtInternalMedicineState);
			this.xtraTabPage5.Name = "xtraTabPage5";
			this.xtraTabPage5.Size = new System.Drawing.Size(787, 105);
			this.xtraTabPage5.Text = "Tình trạng bệnh nội khoa";
			this.txtInternalMedicineState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtInternalMedicineState.Location = new System.Drawing.Point(0, 0);
			this.txtInternalMedicineState.Name = "txtInternalMedicineState";
			this.txtInternalMedicineState.Size = new System.Drawing.Size(787, 105);
			this.txtInternalMedicineState.TabIndex = 0;
			this.xtraTabPage6.Controls.Add(this.memBeforeDiagnostic);
			this.xtraTabPage6.Name = "xtraTabPage6";
			this.xtraTabPage6.Size = new System.Drawing.Size(787, 105);
			this.xtraTabPage6.Text = "Chẩn đoán (Tuyến dưới, KKC, điều trị)";
			this.memBeforeDiagnostic.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memBeforeDiagnostic.Location = new System.Drawing.Point(0, 0);
			this.memBeforeDiagnostic.Name = "memBeforeDiagnostic";
			this.memBeforeDiagnostic.Size = new System.Drawing.Size(787, 105);
			this.memBeforeDiagnostic.TabIndex = 0;
			this.xtraTabPage10.Controls.Add(this.TxtTreatmentTracking);
			this.xtraTabPage10.Name = "xtraTabPage10";
			this.xtraTabPage10.Size = new System.Drawing.Size(787, 105);
			this.xtraTabPage10.Text = "Tóm tắt diễn biến bệnh, quá trình điều trị, chăm sóc";
			this.TxtTreatmentTracking.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TxtTreatmentTracking.Location = new System.Drawing.Point(0, 0);
			this.TxtTreatmentTracking.Name = "TxtTreatmentTracking";
			this.TxtTreatmentTracking.Size = new System.Drawing.Size(787, 105);
			this.TxtTreatmentTracking.TabIndex = 0;
			this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.False;
			this.layoutControlGroup2.GroupBordersVisible = false;
			this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[1] { this.layoutControlItem11 });
			this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup2.Name = "layoutControlGroup2";
			this.layoutControlGroup2.Size = new System.Drawing.Size(797, 137);
			this.layoutControlGroup2.TextVisible = false;
			this.layoutControlItem11.Control = this.xtraTabControl2;
			this.layoutControlItem11.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem11.Name = "layoutControlItem11";
			this.layoutControlItem11.Size = new System.Drawing.Size(797, 137);
			this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem11.TextVisible = false;
			this.checkEditMethod.Location = new System.Drawing.Point(364, 165);
			this.checkEditMethod.Name = "checkEditMethod";
			this.checkEditMethod.Properties.Caption = "Sửa";
			this.checkEditMethod.Size = new System.Drawing.Size(46, 19);
			this.checkEditMethod.StyleController = this.layoutControl1;
			this.checkEditMethod.TabIndex = 39;
			this.checkEditMethod.CheckedChanged += new System.EventHandler(checkEditMethod_CheckedChanged);
			this.checkEditMethod.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(checkEditMethod_PreviewKeyDown);
			this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.panelControl1.Controls.Add(this.CboMethod);
			this.panelControl1.Controls.Add(this.txtPtttMethod);
			this.panelControl1.Location = new System.Drawing.Point(190, 165);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(170, 20);
			this.panelControl1.TabIndex = 38;
			this.CboMethod.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CboMethod.Location = new System.Drawing.Point(0, 0);
			this.CboMethod.Name = "CboMethod";
			this.CboMethod.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.CboMethod.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
			});
			this.CboMethod.Properties.NullText = "";
			this.CboMethod.Properties.View = this.gridView4;
			this.CboMethod.Size = new System.Drawing.Size(170, 20);
			this.CboMethod.TabIndex = 33;
			this.CboMethod.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(CboMethod_Closed);
			this.CboMethod.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(CboMethod_ButtonClick);
			this.CboMethod.TextChanged += new System.EventHandler(CboMethod_TextChanged);
			this.CboMethod.KeyUp += new System.Windows.Forms.KeyEventHandler(CboMethod_KeyUp);
			this.gridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView4.Name = "gridView4";
			this.gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView4.OptionsView.ShowGroupPanel = false;
			this.txtPtttMethod.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPtttMethod.Location = new System.Drawing.Point(0, 0);
			this.txtPtttMethod.Name = "txtPtttMethod";
			this.txtPtttMethod.Size = new System.Drawing.Size(170, 20);
			this.txtPtttMethod.TabIndex = 0;
			this.txtPtttMethod.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtPtttMethod_PreviewKeyDown);
			this.DtSurgeryTime.EditValue = null;
			this.DtSurgeryTime.Location = new System.Drawing.Point(140, 2);
			this.DtSurgeryTime.Name = "DtSurgeryTime";
			this.DtSurgeryTime.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.DtSurgeryTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.DtSurgeryTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.DtSurgeryTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
			this.DtSurgeryTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.DtSurgeryTime.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm";
			this.DtSurgeryTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.DtSurgeryTime.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm";
			this.DtSurgeryTime.Size = new System.Drawing.Size(109, 20);
			this.DtSurgeryTime.StyleController = this.layoutControl1;
			this.DtSurgeryTime.TabIndex = 36;
			this.DtSurgeryTime.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(DtSurgeryTime_Closed);
			this.DtSurgeryTime.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(DtSurgeryTime_PreviewKeyDown);
			this.CboEmotionlessMethod.Location = new System.Drawing.Point(568, 165);
			this.CboEmotionlessMethod.Name = "CboEmotionlessMethod";
			this.CboEmotionlessMethod.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.CboEmotionlessMethod.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
			});
			this.CboEmotionlessMethod.Properties.NullText = "";
			this.CboEmotionlessMethod.Properties.View = this.gridLookUpEdit2View;
			this.CboEmotionlessMethod.Size = new System.Drawing.Size(237, 20);
			this.CboEmotionlessMethod.StyleController = this.layoutControl1;
			this.CboEmotionlessMethod.TabIndex = 35;
			this.CboEmotionlessMethod.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(CboEmotionlessMethod_Closed);
			this.CboEmotionlessMethod.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(CboEmotionlessMethod_ButtonClick);
			this.CboEmotionlessMethod.KeyUp += new System.Windows.Forms.KeyEventHandler(CboEmotionlessMethod_KeyUp);
			this.gridLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit2View.Name = "gridLookUpEdit2View";
			this.gridLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit2View.OptionsView.ShowGroupPanel = false;
			this.TxtEmotionlessMethod.Location = new System.Drawing.Point(509, 165);
			this.TxtEmotionlessMethod.Name = "TxtEmotionlessMethod";
			this.TxtEmotionlessMethod.Size = new System.Drawing.Size(59, 20);
			this.TxtEmotionlessMethod.StyleController = this.layoutControl1;
			this.TxtEmotionlessMethod.TabIndex = 34;
			this.TxtEmotionlessMethod.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(TxtEmotionlessMethod_PreviewKeyDown);
			this.TxtMethodCode.Location = new System.Drawing.Point(140, 165);
			this.TxtMethodCode.Name = "TxtMethodCode";
			this.TxtMethodCode.Size = new System.Drawing.Size(50, 20);
			this.TxtMethodCode.StyleController = this.layoutControl1;
			this.TxtMethodCode.TabIndex = 32;
			this.TxtMethodCode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(TxtMethodCode_PreviewKeyDown);
			this.BtnSaveEkipTemp.Location = new System.Drawing.Point(705, 2);
			this.BtnSaveEkipTemp.Name = "BtnSaveEkipTemp";
			this.BtnSaveEkipTemp.Size = new System.Drawing.Size(100, 22);
			this.BtnSaveEkipTemp.StyleController = this.layoutControl1;
			this.BtnSaveEkipTemp.TabIndex = 9;
			this.BtnSaveEkipTemp.Text = "Lưu kíp mẫu";
			this.BtnSaveEkipTemp.Click += new System.EventHandler(BtnSaveEkipTemp_Click);
			this.CboEkipTemp.Location = new System.Drawing.Point(509, 2);
			this.CboEkipTemp.Name = "CboEkipTemp";
			this.CboEkipTemp.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.CboEkipTemp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
			});
			this.CboEkipTemp.Properties.NullText = "";
			this.CboEkipTemp.Properties.View = this.gridLookUpEdit1View;
			this.CboEkipTemp.Size = new System.Drawing.Size(192, 20);
			this.CboEkipTemp.StyleController = this.layoutControl1;
			this.CboEkipTemp.TabIndex = 8;
			this.CboEkipTemp.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(CboEkipTemp_Closed);
			this.CboEkipTemp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(CboEkipTemp_ButtonClick);
			this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
			this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
			this.TxtKqCls.Location = new System.Drawing.Point(140, 28);
			this.TxtKqCls.Name = "TxtKqCls";
			this.TxtKqCls.Properties.NullValuePrompt = "Click \"Chọn kết quả\" để chọn dịch vụ";
			this.TxtKqCls.Properties.NullValuePromptShowForEmptyValue = true;
			this.TxtKqCls.Properties.ShowNullValuePromptWhenFocused = true;
			this.TxtKqCls.Size = new System.Drawing.Size(270, 133);
			this.TxtKqCls.StyleController = this.layoutControl1;
			this.TxtKqCls.TabIndex = 5;
			this.BtnChooseCls.Location = new System.Drawing.Point(326, 2);
			this.BtnChooseCls.Name = "BtnChooseCls";
			this.BtnChooseCls.Size = new System.Drawing.Size(84, 22);
			this.BtnChooseCls.StyleController = this.layoutControl1;
			this.BtnChooseCls.TabIndex = 4;
			this.BtnChooseCls.Text = "Chọn kết quả";
			this.BtnChooseCls.Click += new System.EventHandler(BtnChooseCls_Click);
			this.grdControlInformationSurg.Location = new System.Drawing.Point(509, 28);
			this.grdControlInformationSurg.MainView = this.grdViewInformationSurg;
			this.grdControlInformationSurg.Name = "grdControlInformationSurg";
			this.grdControlInformationSurg.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[8]
			{
				this.btnAdd,
				(DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.cbo_UseName,
				this.txtLogin,
				this.cboPosition,
				this.btnDelete,
				this.repositoryItemGridLookUpUsername,
				this.repositoryItemSearchLookUpEdit1,
				(DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.GridLookUpEdit_Department
			});
			this.grdControlInformationSurg.Size = new System.Drawing.Size(296, 133);
			this.grdControlInformationSurg.TabIndex = 31;
			this.grdControlInformationSurg.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.grdViewInformationSurg });
			this.grdViewInformationSurg.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[5] { this.gridColumn1, this.gridColumn2, this.gridColumn3, this.gridColumn4, this.gridColumn5 });
			this.grdViewInformationSurg.GridControl = this.grdControlInformationSurg;
			this.grdViewInformationSurg.Name = "grdViewInformationSurg";
			this.grdViewInformationSurg.OptionsView.ShowGroupPanel = false;
			this.grdViewInformationSurg.OptionsView.ShowIndicator = false;
			this.grdViewInformationSurg.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(gridView1_CustomRowCellEdit);
			this.grdViewInformationSurg.ShowingEditor += new System.ComponentModel.CancelEventHandler(grdViewInformationSurg_ShowingEditor);
			this.grdViewInformationSurg.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(grdViewInformationSurg_FocusedColumnChanged);
			this.grdViewInformationSurg.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gridView1_CellValueChanged);
			this.grdViewInformationSurg.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(grdViewInformationSurg_CustomUnboundColumnData);
			this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn1.Caption = "Họ và tên";
			this.gridColumn1.ColumnEdit = (DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.cbo_UseName;
			this.gridColumn1.FieldName = "LOGINNAME";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn1.Visible = true;
			this.gridColumn1.VisibleIndex = 1;
			this.gridColumn1.Width = 319;
			((DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit)(object)this.cbo_UseName).AutoComplete = false;
			((DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.cbo_UseName).AutoHeight = false;
			((DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit)(object)this.cbo_UseName).Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance, appearanceHovered, appearancePressed, appearanceDisabled, "", null, null, true)
			});
			((DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.cbo_UseName).Name = "cbo_UseName";
			((DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.cbo_UseName).NullText = "";
			((DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit)(object)this.cbo_UseName).TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			((DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEditBase)(object)this.cbo_UseName).View = (DevExpress.XtraGrid.Views.Grid.GridView)(object)this.repositoryItemCustomGridLookUpEditNew1View;
			((DevExpress.XtraEditors.Repository.RepositoryItemPopupBase)(object)this.cbo_UseName).Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cbo_UseName_Closed);
			((DevExpress.XtraGrid.Views.Grid.GridView)(object)this.repositoryItemCustomGridLookUpEditNew1View).FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			((DevExpress.XtraGrid.Views.Base.BaseView)(object)this.repositoryItemCustomGridLookUpEditNew1View).Name = "repositoryItemCustomGridLookUpEditNew1View";
			((DevExpress.XtraGrid.Views.Grid.GridView)(object)this.repositoryItemCustomGridLookUpEditNew1View).OptionsSelection.EnableAppearanceFocusedCell = false;
			((DevExpress.XtraGrid.Views.Grid.GridView)(object)this.repositoryItemCustomGridLookUpEditNew1View).OptionsView.ShowGroupPanel = false;
			this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColumn2.Caption = "Vai trò";
			this.gridColumn2.ColumnEdit = this.cboPosition;
			this.gridColumn2.FieldName = "EXECUTE_ROLE_ID";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 0;
			this.gridColumn2.Width = 215;
			this.cboPosition.AutoHeight = false;
			this.cboPosition.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance2, appearanceHovered2, appearancePressed2, appearanceDisabled2, "", null, null, true)
			});
			this.cboPosition.Name = "cboPosition";
			this.cboPosition.NullText = "";
			this.cboPosition.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.gridColumn3.Caption = "Khoa";
			this.gridColumn3.ColumnEdit = (DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.GridLookUpEdit_Department;
			this.gridColumn3.FieldName = "DEPARTMENT_ID";
			this.gridColumn3.Name = "gridColumn3";
			this.gridColumn3.Visible = true;
			this.gridColumn3.VisibleIndex = 2;
			this.gridColumn3.Width = 194;
			((DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit)(object)this.GridLookUpEdit_Department).AutoComplete = false;
			((DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.GridLookUpEdit_Department).AutoHeight = false;
			((DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit)(object)this.GridLookUpEdit_Department).Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			((DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.GridLookUpEdit_Department).Name = "GridLookUpEdit_Department";
			((DevExpress.XtraEditors.Repository.RepositoryItem)(object)this.GridLookUpEdit_Department).NullText = "";
			((DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit)(object)this.GridLookUpEdit_Department).TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			((DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEditBase)(object)this.GridLookUpEdit_Department).View = (DevExpress.XtraGrid.Views.Grid.GridView)(object)this.customGridView1;
			((DevExpress.XtraGrid.Views.Grid.GridView)(object)this.customGridView1).FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			((DevExpress.XtraGrid.Views.Base.BaseView)(object)this.customGridView1).Name = "customGridView1";
			((DevExpress.XtraGrid.Views.Grid.GridView)(object)this.customGridView1).OptionsSelection.EnableAppearanceFocusedCell = false;
			((DevExpress.XtraGrid.Views.Grid.GridView)(object)this.customGridView1).OptionsView.ShowGroupPanel = false;
			this.gridColumn4.Caption = "Thêm";
			this.gridColumn4.ColumnEdit = this.btnAdd;
			this.gridColumn4.FieldName = "BtnDelete";
			this.gridColumn4.Name = "gridColumn4";
			this.gridColumn4.OptionsColumn.ShowCaption = false;
			this.gridColumn4.Visible = true;
			this.gridColumn4.VisibleIndex = 3;
			this.gridColumn4.Width = 51;
			this.btnAdd.AutoHeight = false;
			this.btnAdd.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
			});
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
			this.btnAdd.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(btnAdd_ButtonClick);
			this.gridColumn5.Caption = "gridColumn1";
			this.gridColumn5.FieldName = "LOGINNAME";
			this.gridColumn5.Name = "gridColumn5";
			this.txtLogin.AutoHeight = false;
			this.txtLogin.Name = "txtLogin";
			this.btnDelete.AutoHeight = false;
			this.btnDelete.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Minus)
			});
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
			this.btnDelete.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(btnDelete_ButtonClick);
			this.repositoryItemGridLookUpUsername.AutoHeight = false;
			this.repositoryItemGridLookUpUsername.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance3, appearanceHovered3, appearancePressed3, appearanceDisabled3, "", null, null, true)
			});
			this.repositoryItemGridLookUpUsername.Name = "repositoryItemGridLookUpUsername";
			this.repositoryItemGridLookUpUsername.NullText = "";
			this.repositoryItemGridLookUpUsername.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.repositoryItemGridLookUpUsername.View = this.gridView2;
			this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView2.Name = "gridView2";
			this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView2.OptionsView.ShowGroupPanel = false;
			this.repositoryItemSearchLookUpEdit1.AutoHeight = false;
			this.repositoryItemSearchLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance4, appearanceHovered4, appearancePressed4, appearanceDisabled4, "", null, null, true)
			});
			this.repositoryItemSearchLookUpEdit1.HideSelection = false;
			this.repositoryItemSearchLookUpEdit1.Name = "repositoryItemSearchLookUpEdit1";
			this.repositoryItemSearchLookUpEdit1.NullText = "";
			this.repositoryItemSearchLookUpEdit1.ShowClearButton = false;
			this.repositoryItemSearchLookUpEdit1.View = this.gridView3;
			this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView3.Name = "gridView3";
			this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView3.OptionsView.ShowGroupPanel = false;
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[14]
			{
				this.emptySpaceItem1, this.layoutControlItem1, this.LciKqCls, this.LciMethodCode, this.layoutControlItem3, this.LciSurgeryTime, this.LciEkipTemp, this.layoutControlItem2, this.lciInformationSurg, this.LciEmotionlessMethod,
				this.layoutControlItem6, this.layoutControlItem4, this.layoutControlItem8, this.layoutControlItem9
			});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(807, 500);
			this.layoutControlGroup1.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(251, 0);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(73, 26);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.Control = this.BtnChooseCls;
			this.layoutControlItem1.Location = new System.Drawing.Point(324, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(88, 26);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			this.LciKqCls.AppearanceItemCaption.Options.UseTextOptions = true;
			this.LciKqCls.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.LciKqCls.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
			this.LciKqCls.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.LciKqCls.Control = this.TxtKqCls;
			this.LciKqCls.Location = new System.Drawing.Point(0, 26);
			this.LciKqCls.Name = "LciKqCls";
			this.LciKqCls.OptionsToolTip.ToolTip = "Diễn biến kết quả cận lâm sàng";
			this.LciKqCls.Size = new System.Drawing.Size(412, 137);
			this.LciKqCls.Text = "Kết quả CLS:";
			this.LciKqCls.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.LciKqCls.TextSize = new System.Drawing.Size(133, 20);
			this.LciKqCls.TextToControlDistance = 5;
			this.LciMethodCode.AppearanceItemCaption.Options.UseTextOptions = true;
			this.LciMethodCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.LciMethodCode.Control = this.TxtMethodCode;
			this.LciMethodCode.Location = new System.Drawing.Point(0, 163);
			this.LciMethodCode.Name = "LciMethodCode";
			this.LciMethodCode.OptionsToolTip.ToolTip = "Phương pháp phẫu thuật";
			this.LciMethodCode.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.LciMethodCode.Size = new System.Drawing.Size(190, 24);
			this.LciMethodCode.Text = "PP phẫu thuật:";
			this.LciMethodCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.LciMethodCode.TextSize = new System.Drawing.Size(133, 20);
			this.LciMethodCode.TextToControlDistance = 5;
			this.layoutControlItem3.Control = this.panelControl1;
			this.layoutControlItem3.Location = new System.Drawing.Point(190, 163);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.layoutControlItem3.Size = new System.Drawing.Size(172, 24);
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextVisible = false;
			this.LciSurgeryTime.AppearanceItemCaption.Options.UseTextOptions = true;
			this.LciSurgeryTime.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.LciSurgeryTime.Control = this.DtSurgeryTime;
			this.LciSurgeryTime.Location = new System.Drawing.Point(0, 0);
			this.LciSurgeryTime.Name = "LciSurgeryTime";
			this.LciSurgeryTime.Size = new System.Drawing.Size(251, 26);
			this.LciSurgeryTime.Text = "Ngày phẫu thuật:";
			this.LciSurgeryTime.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.LciSurgeryTime.TextSize = new System.Drawing.Size(133, 20);
			this.LciSurgeryTime.TextToControlDistance = 5;
			this.LciEkipTemp.AppearanceItemCaption.Options.UseTextOptions = true;
			this.LciEkipTemp.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.LciEkipTemp.Control = this.CboEkipTemp;
			this.LciEkipTemp.Location = new System.Drawing.Point(412, 0);
			this.LciEkipTemp.Name = "LciEkipTemp";
			this.LciEkipTemp.Size = new System.Drawing.Size(291, 26);
			this.LciEkipTemp.Text = "Kíp mẫu:";
			this.LciEkipTemp.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.LciEkipTemp.TextSize = new System.Drawing.Size(90, 20);
			this.LciEkipTemp.TextToControlDistance = 5;
			this.layoutControlItem2.Control = this.BtnSaveEkipTemp;
			this.layoutControlItem2.Location = new System.Drawing.Point(703, 0);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(104, 26);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.lciInformationSurg.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciInformationSurg.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciInformationSurg.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
			this.lciInformationSurg.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.lciInformationSurg.Control = this.grdControlInformationSurg;
			this.lciInformationSurg.CustomizationFormText = "Kíp thực hiện:";
			this.lciInformationSurg.Location = new System.Drawing.Point(412, 26);
			this.lciInformationSurg.Name = "lciInformationSurg";
			this.lciInformationSurg.Size = new System.Drawing.Size(395, 137);
			this.lciInformationSurg.Text = "Chỉ định kíp mổ:";
			this.lciInformationSurg.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciInformationSurg.TextSize = new System.Drawing.Size(90, 20);
			this.lciInformationSurg.TextToControlDistance = 5;
			this.LciEmotionlessMethod.AppearanceItemCaption.Options.UseTextOptions = true;
			this.LciEmotionlessMethod.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.LciEmotionlessMethod.Control = this.TxtEmotionlessMethod;
			this.LciEmotionlessMethod.Location = new System.Drawing.Point(412, 163);
			this.LciEmotionlessMethod.Name = "LciEmotionlessMethod";
			this.LciEmotionlessMethod.OptionsToolTip.ToolTip = "Phương pháp vô cảm";
			this.LciEmotionlessMethod.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.LciEmotionlessMethod.Size = new System.Drawing.Size(156, 24);
			this.LciEmotionlessMethod.Text = "PP vô cảm:";
			this.LciEmotionlessMethod.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.LciEmotionlessMethod.TextSize = new System.Drawing.Size(90, 20);
			this.LciEmotionlessMethod.TextToControlDistance = 5;
			this.layoutControlItem6.Control = this.CboEmotionlessMethod;
			this.layoutControlItem6.Location = new System.Drawing.Point(568, 163);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.layoutControlItem6.Size = new System.Drawing.Size(239, 24);
			this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem6.TextVisible = false;
			this.layoutControlItem4.Control = this.checkEditMethod;
			this.layoutControlItem4.Location = new System.Drawing.Point(362, 163);
			this.layoutControlItem4.MaxSize = new System.Drawing.Size(50, 24);
			this.layoutControlItem4.MinSize = new System.Drawing.Size(50, 24);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(50, 24);
			this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem4.TextVisible = false;
			this.layoutControlItem8.Control = this.groupBox1;
			this.layoutControlItem8.Location = new System.Drawing.Point(0, 187);
			this.layoutControlItem8.Name = "layoutControlItem8";
			this.layoutControlItem8.Size = new System.Drawing.Size(807, 160);
			this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem8.TextVisible = false;
			this.layoutControlItem9.Control = this.groupBox2;
			this.layoutControlItem9.Location = new System.Drawing.Point(0, 347);
			this.layoutControlItem9.Name = "layoutControlItem9";
			this.layoutControlItem9.Size = new System.Drawing.Size(807, 153);
			this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem9.TextVisible = false;
			this.dxValidationProvider1.ValidationFailed += new DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventHandler(dxValidationProvider1_ValidationFailed);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.layoutControl1);
			base.Name = "UcPttt";
			base.Size = new System.Drawing.Size(807, 500);
			base.Load += new System.EventHandler(UcPttt_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.layoutControl3).EndInit();
			this.layoutControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
			this.xtraTabControl1.ResumeLayout(false);
			this.xtraTabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.memDiscussion.Properties).EndInit();
			this.xtraTabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.TxtPrognosis.Properties).EndInit();
			this.xtraTabPage7.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.txtTreatmentMethod.Properties).EndInit();
			this.xtraTabPage8.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.memCareMethod.Properties).EndInit();
			this.xtraTabPage9.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.memCONCLUSION.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).EndInit();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.layoutControl2).EndInit();
			this.layoutControl2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
			this.xtraTabControl2.ResumeLayout(false);
			this.xtraTabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.memPathHis.Properties).EndInit();
			this.xtraTabPage4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.memHosState.Properties).EndInit();
			this.xtraTabPage5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.txtInternalMedicineState.Properties).EndInit();
			this.xtraTabPage6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.memBeforeDiagnostic.Properties).EndInit();
			this.xtraTabPage10.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.TxtTreatmentTracking.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).EndInit();
			((System.ComponentModel.ISupportInitialize)this.checkEditMethod.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
			this.panelControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.CboMethod.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtPtttMethod.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.DtSurgeryTime.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.DtSurgeryTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.CboEmotionlessMethod.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit2View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.TxtEmotionlessMethod.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.TxtMethodCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.CboEkipTemp.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.TxtKqCls.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.grdControlInformationSurg).EndInit();
			((System.ComponentModel.ISupportInitialize)this.grdViewInformationSurg).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cbo_UseName).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemCustomGridLookUpEditNew1View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboPosition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.GridLookUpEdit_Department).EndInit();
			((System.ComponentModel.ISupportInitialize)this.customGridView1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.btnAdd).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtLogin).EndInit();
			((System.ComponentModel.ISupportInitialize)this.btnDelete).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpUsername).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemSearchLookUpEdit1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.LciKqCls).EndInit();
			((System.ComponentModel.ISupportInitialize)this.LciMethodCode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.LciSurgeryTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.LciEkipTemp).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciInformationSurg).EndInit();
			((System.ComponentModel.ISupportInitialize)this.LciEmotionlessMethod).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
