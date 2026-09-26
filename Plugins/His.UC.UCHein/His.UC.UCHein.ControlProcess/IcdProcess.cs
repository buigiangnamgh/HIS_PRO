using System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using Inventec.Common.Logging;

namespace His.UC.UCHein.ControlProcess
{
	public class IcdProcess
	{
		public static void LoadDataToCombo(GridLookUpEdit cboChanDoanTD, object data)
		{
			try
			{
				cboChanDoanTD.Properties.DataSource = data;
				cboChanDoanTD.Properties.DisplayMember = "ICD_NAME";
				cboChanDoanTD.Properties.ValueMember = "ID";
				cboChanDoanTD.Properties.TextEditStyle = TextEditStyles.Standard;
				cboChanDoanTD.Properties.PopupFilterMode = PopupFilterMode.Contains;
				cboChanDoanTD.Properties.ImmediatePopup = true;
				cboChanDoanTD.ForceInitialize();
				cboChanDoanTD.Properties.View.Columns.Clear();
				GridColumn gridColumn = cboChanDoanTD.Properties.View.Columns.AddField("ICD_CODE");
				gridColumn.Caption = "Mã";
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 100;
				GridColumn gridColumn2 = cboChanDoanTD.Properties.View.Columns.AddField("ICD_NAME");
				gridColumn2.Caption = "Tên";
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 2;
				gridColumn2.Width = 400;
				GridColumn gridColumn3 = cboChanDoanTD.Properties.View.Columns.AddField("ICD_NAME_UNSIGNED");
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
