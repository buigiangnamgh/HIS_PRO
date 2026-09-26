using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.ControlProcess
{
	public class KskContractProcess
	{
		public static void LoadDataToComboKskContract(LookUpEdit cboKskContract, List<HIS_KSK_CONTRACT> data)
		{
			try
			{
				cboKskContract.Properties.DataSource = data;
				cboKskContract.Properties.DisplayMember = "CUSTOMER_NAME";
				cboKskContract.Properties.ValueMember = "ID";
				cboKskContract.Properties.ForceInitialize();
				cboKskContract.Properties.Columns.Clear();
				cboKskContract.Properties.Columns.Add(new LookUpColumnInfo("KSK_CONTRACT_CODE", "Mã", 100));
				cboKskContract.Properties.Columns.Add(new LookUpColumnInfo("CUSTOMER_NAME", "Tên", 200));
				cboKskContract.Properties.Columns.Add(new LookUpColumnInfo("RENDERER_RATIO", "Tỉ lệ đóng chi trả", 150));
				cboKskContract.Properties.Columns.Add(new LookUpColumnInfo("RENDERER_MAX_FEE", "Trần viện phí", 100));
				cboKskContract.Properties.ShowHeader = true;
				cboKskContract.Properties.ImmediatePopup = true;
				cboKskContract.Properties.DropDownRows = 10;
				cboKskContract.Properties.PopupWidth = 550;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
