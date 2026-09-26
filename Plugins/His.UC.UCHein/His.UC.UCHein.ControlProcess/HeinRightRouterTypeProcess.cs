using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Inventec.Common.Logging;
using MOS.LibraryHein.Bhyt.HeinRightRouteType;

namespace His.UC.UCHein.ControlProcess
{
	public class HeinRightRouterTypeProcess
	{
		public static void FillDataToComboHeinRightRouterType(LookUpEdit cboHeinRightRouterType, List<HeinRightRouteTypeData> data)
		{
			try
			{
				cboHeinRightRouterType.Properties.DataSource = data;
				cboHeinRightRouterType.Properties.DisplayMember = "HeinRightRouteTypeName";
				cboHeinRightRouterType.Properties.ValueMember = "HeinRightRouteTypeCode";
				cboHeinRightRouterType.Properties.ForceInitialize();
				cboHeinRightRouterType.Properties.Columns.Clear();
				cboHeinRightRouterType.Properties.Columns.Add(new LookUpColumnInfo("HeinRightRouteTypeCode", "", 100));
				cboHeinRightRouterType.Properties.Columns.Add(new LookUpColumnInfo("HeinRightRouteTypeName", "", 200));
				cboHeinRightRouterType.Properties.ShowHeader = false;
				cboHeinRightRouterType.Properties.ImmediatePopup = true;
				cboHeinRightRouterType.Properties.DropDownRows = 20;
				cboHeinRightRouterType.Properties.PopupWidth = 300;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
