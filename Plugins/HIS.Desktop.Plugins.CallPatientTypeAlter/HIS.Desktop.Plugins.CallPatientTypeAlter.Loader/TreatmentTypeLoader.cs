using System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter.Loader
{
	public class TreatmentTypeLoader
	{
		public static void LoadDataToComboTreatmentType(LookUpEdit cboTreatmentType, object data)
		{
			try
			{
				cboTreatmentType.Properties.DataSource = data;
				cboTreatmentType.Properties.DisplayMember = "TREATMENT_TYPE_NAME";
				cboTreatmentType.Properties.ValueMember = "ID";
				cboTreatmentType.Properties.ForceInitialize();
				cboTreatmentType.Properties.Columns.Clear();
				cboTreatmentType.Properties.Columns.Add(new LookUpColumnInfo("TREATMENT_TYPE_CODE", "", 100));
				cboTreatmentType.Properties.Columns.Add(new LookUpColumnInfo("TREATMENT_TYPE_NAME", "", 200));
				cboTreatmentType.Properties.ShowHeader = false;
				cboTreatmentType.Properties.ImmediatePopup = true;
				cboTreatmentType.Properties.DropDownRows = 10;
				cboTreatmentType.Properties.PopupWidth = 300;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static void LoadDataToComboTreatmentType(GridLookUpEdit cboTreatmentType, object data)
		{
			try
			{
				cboTreatmentType.Properties.DataSource = data;
				cboTreatmentType.Properties.DisplayMember = "TREATMENT_TYPE_NAME";
				cboTreatmentType.Properties.ValueMember = "ID";
				cboTreatmentType.Properties.TextEditStyle = TextEditStyles.Standard;
				cboTreatmentType.Properties.PopupFilterMode = PopupFilterMode.Contains;
				cboTreatmentType.Properties.ImmediatePopup = true;
				cboTreatmentType.ForceInitialize();
				cboTreatmentType.Properties.View.Columns.Clear();
				GridColumn gridColumn = cboTreatmentType.Properties.View.Columns.AddField("TREATMENT_TYPE_CODE");
				gridColumn.Caption = "Mã";
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 50;
				GridColumn gridColumn2 = cboTreatmentType.Properties.View.Columns.AddField("TREATMENT_TYPE_NAME");
				gridColumn2.Caption = "Tên";
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 2;
				gridColumn2.Width = 100;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
