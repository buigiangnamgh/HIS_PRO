using System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using Inventec.Common.Logging;

namespace His.UC.UCHein.ControlProcess
{
	public class MediOrgProcess
	{
		public static void LoadDataToComboNoiDKKCBBD(GridLookUpEdit cboMediOrg, object data)
		{
			try
			{
				cboMediOrg.Properties.DataSource = data;
				cboMediOrg.Properties.DisplayMember = "MEDI_ORG_NAME";
				cboMediOrg.Properties.ValueMember = "MEDI_ORG_CODE";
				cboMediOrg.Properties.TextEditStyle = TextEditStyles.Standard;
				cboMediOrg.Properties.PopupFilterMode = PopupFilterMode.Contains;
				cboMediOrg.Properties.ImmediatePopup = true;
				cboMediOrg.ForceInitialize();
				cboMediOrg.Properties.View.Columns.Clear();
				GridColumn gridColumn = cboMediOrg.Properties.View.Columns.AddField("MEDI_ORG_CODE");
				gridColumn.Caption = "Mã";
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 70;
				GridColumn gridColumn2 = cboMediOrg.Properties.View.Columns.AddField("MEDI_ORG_NAME");
				gridColumn2.Caption = "Tên";
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 2;
				gridColumn2.Width = 300;
				GridColumn gridColumn3 = cboMediOrg.Properties.View.Columns.AddField("MEDI_ORG_NAME_UNSIGNED");
				gridColumn3.Caption = "Tên";
				gridColumn3.Visible = false;
				gridColumn3.VisibleIndex = -1;
				gridColumn3.Width = 5;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
