using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.ControlProcess
{
	public class HeinCardProcess
	{
		public static void LoadDataToCombo(List<HIS_PATIENT_TYPE_ALTER> patientTypeAlters, LookUpEdit cboSoThe)
		{
			try
			{
				cboSoThe.Properties.BeginUpdate();
				cboSoThe.Properties.DataSource = patientTypeAlters;
				cboSoThe.Properties.DisplayMember = "RENDERER_HEIN_CARD_NUMBER";
				cboSoThe.Properties.ValueMember = "ID";
				cboSoThe.Properties.ForceInitialize();
				cboSoThe.Properties.Columns.Clear();
				cboSoThe.Properties.Columns.Add(new LookUpColumnInfo("RENDERER_HEIN_CARD_NUMBER", "", 150));
				cboSoThe.Properties.Columns.Add(new LookUpColumnInfo("RENDERER_FROM_DATE_TODATE", "", 150));
				cboSoThe.Properties.Columns.Add(new LookUpColumnInfo("HEIN_MEDI_ORG_NAME", "", 150));
				cboSoThe.Properties.ShowHeader = false;
				cboSoThe.Properties.ImmediatePopup = true;
				cboSoThe.Properties.DropDownRows = 5;
				cboSoThe.Properties.PopupWidth = 500;
				cboSoThe.Properties.EndUpdate();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static string ProcessHeinCardNumber(string HEIN_CARD_NUMBER)
		{
			string result = null;
			try
			{
				if (HEIN_CARD_NUMBER != null)
				{
					string text = "";
					string text2 = "";
					string text3 = "";
					string text4 = "";
					string text5 = "";
					string text6 = "";
					try
					{
						text = HEIN_CARD_NUMBER.Substring(0, 2);
						text2 = HEIN_CARD_NUMBER.Substring(2, 1);
						text3 = HEIN_CARD_NUMBER.Substring(3, 2);
						text4 = HEIN_CARD_NUMBER.Substring(5, 2);
						text5 = HEIN_CARD_NUMBER.Substring(7, 3);
						text6 = HEIN_CARD_NUMBER.Substring(10);
					}
					catch (Exception ex)
					{
						LogSystem.Warn("Gan chuoi RENDERER_HEIN_CARD_NUMBER the BHYT loi", ex);
					}
					result = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", text, text2, text3, text4, text5, text6);
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
			return result;
		}
	}
}
