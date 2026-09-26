using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using Inventec.Common.Logging;
using MOS.LibraryHein.Bhyt.HeinLiveArea;

namespace His.UC.UCHein.ControlProcess
{
	public class LiveAreaProcess
	{
		public static void LoadDataToComboNoiSong(GridLookUpEdit cboNoiSong, List<HeinLiveAreaData> data)
		{
			try
			{
				cboNoiSong.Properties.DataSource = data;
				cboNoiSong.Properties.DisplayMember = "HeinLiveName";
				cboNoiSong.Properties.ValueMember = "HeinLiveCode";
				cboNoiSong.Properties.TextEditStyle = TextEditStyles.Standard;
				cboNoiSong.Properties.PopupFilterMode = PopupFilterMode.Contains;
				cboNoiSong.Properties.ImmediatePopup = true;
				cboNoiSong.ForceInitialize();
				cboNoiSong.Properties.View.Columns.Clear();
				GridColumn gridColumn = cboNoiSong.Properties.View.Columns.AddField("HeinLiveCode");
				gridColumn.Caption = "Mã";
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 70;
				GridColumn gridColumn2 = cboNoiSong.Properties.View.Columns.AddField("HeinLiveName");
				gridColumn2.Caption = "Tên";
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 2;
				gridColumn2.Width = 300;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
