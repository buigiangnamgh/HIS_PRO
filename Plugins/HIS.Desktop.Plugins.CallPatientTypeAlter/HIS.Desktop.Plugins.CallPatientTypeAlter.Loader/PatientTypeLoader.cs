using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter.Loader
{
	public class PatientTypeLoader
	{
		public static void LoadDataToCombo(LookUpEdit cboPatientType, List<HIS_PATIENT_TYPE> listData)
		{
			try
			{
				cboPatientType.Properties.BeginUpdate();
				cboPatientType.Properties.DataSource = listData;
				cboPatientType.Properties.DisplayMember = "PATIENT_TYPE_NAME";
				cboPatientType.Properties.ValueMember = "ID";
				cboPatientType.Properties.ForceInitialize();
				cboPatientType.Properties.Columns.Clear();
				cboPatientType.Properties.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_CODE", "", 100));
				cboPatientType.Properties.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_NAME", "", 200));
				cboPatientType.Properties.ShowHeader = false;
				cboPatientType.Properties.ImmediatePopup = true;
				cboPatientType.Properties.DropDownRows = 10;
				cboPatientType.Properties.PopupWidth = 300;
				cboPatientType.Properties.EndUpdate();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static void LoadDataToCombo(GridLookUpEdit cboPatientType, List<HIS_PATIENT_TYPE> data)
		{
			try
			{
				cboPatientType.Properties.DataSource = data;
				cboPatientType.Properties.DisplayMember = "PATIENT_TYPE_NAME";
				cboPatientType.Properties.ValueMember = "ID";
				cboPatientType.Properties.TextEditStyle = TextEditStyles.Standard;
				cboPatientType.Properties.PopupFilterMode = PopupFilterMode.Contains;
				cboPatientType.Properties.ImmediatePopup = true;
				cboPatientType.ForceInitialize();
				cboPatientType.Properties.View.Columns.Clear();
				GridColumn gridColumn = cboPatientType.Properties.View.Columns.AddField("PATIENT_TYPE_CODE");
				gridColumn.Caption = "Mã";
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 50;
				GridColumn gridColumn2 = cboPatientType.Properties.View.Columns.AddField("PATIENT_TYPE_NAME");
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

		public static void LoadDataToCombo(RepositoryItemLookUpEdit cboPatientType, List<HIS_PATIENT_TYPE> listData)
		{
			try
			{
				cboPatientType.Properties.BeginUpdate();
				cboPatientType.Properties.DataSource = listData;
				cboPatientType.Properties.DisplayMember = "PATIENT_TYPE_NAME";
				cboPatientType.Properties.ValueMember = "ID";
				cboPatientType.Properties.ForceInitialize();
				cboPatientType.Properties.Columns.Clear();
				cboPatientType.Properties.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_CODE", "", 100));
				cboPatientType.Properties.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_NAME", "", 200));
				cboPatientType.Properties.ShowHeader = false;
				cboPatientType.Properties.ImmediatePopup = true;
				cboPatientType.Properties.DropDownRows = 10;
				cboPatientType.Properties.PopupWidth = 300;
				cboPatientType.Properties.EndUpdate();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static void LoadDataToComboForNameValue(LookUpEdit cboPatientType, List<HIS_PATIENT_TYPE> listData)
		{
			try
			{
				cboPatientType.Properties.BeginUpdate();
				cboPatientType.Properties.DataSource = listData;
				cboPatientType.Properties.DisplayMember = "PATIENT_TYPE_NAME";
				cboPatientType.Properties.ValueMember = "PATIENT_TYPE_NAME";
				cboPatientType.Properties.ForceInitialize();
				cboPatientType.Properties.Columns.Clear();
				cboPatientType.Properties.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_CODE", "", 100));
				cboPatientType.Properties.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_NAME", "", 200));
				cboPatientType.Properties.ShowHeader = false;
				cboPatientType.Properties.ImmediatePopup = true;
				cboPatientType.Properties.DropDownRows = 10;
				cboPatientType.Properties.PopupWidth = 300;
				cboPatientType.Properties.EndUpdate();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static void LoadDataToComboForNameValue(RepositoryItemLookUpEdit cboPatientType, List<HIS_PATIENT_TYPE> listData)
		{
			try
			{
				cboPatientType.Properties.BeginUpdate();
				cboPatientType.Properties.DataSource = listData;
				cboPatientType.Properties.DisplayMember = "PATIENT_TYPE_NAME";
				cboPatientType.Properties.ValueMember = "PATIENT_TYPE_NAME";
				cboPatientType.Properties.ForceInitialize();
				cboPatientType.Properties.Columns.Clear();
				cboPatientType.Properties.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_CODE", "", 100));
				cboPatientType.Properties.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_NAME", "", 200));
				cboPatientType.Properties.ShowHeader = false;
				cboPatientType.Properties.ImmediatePopup = true;
				cboPatientType.Properties.DropDownRows = 10;
				cboPatientType.Properties.PopupWidth = 300;
				cboPatientType.Properties.EndUpdate();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal static void LoadDataToPatientTypeRepositoryItemCombo(RepositoryItemLookUpEdit repositoryItemcboPatientType, object data)
		{
			try
			{
				repositoryItemcboPatientType.DataSource = data;
				repositoryItemcboPatientType.DisplayMember = "PATIENT_TYPE_NAME";
				repositoryItemcboPatientType.ValueMember = "PATIENT_TYPE_CODE";
				repositoryItemcboPatientType.ForceInitialize();
				repositoryItemcboPatientType.Columns.Clear();
				repositoryItemcboPatientType.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_CODE", "", 100));
				repositoryItemcboPatientType.Columns.Add(new LookUpColumnInfo("PATIENT_TYPE_NAME", "", 200));
				repositoryItemcboPatientType.ShowHeader = false;
				repositoryItemcboPatientType.ImmediatePopup = true;
				repositoryItemcboPatientType.DropDownRows = 10;
				repositoryItemcboPatientType.PopupWidth = 300;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
