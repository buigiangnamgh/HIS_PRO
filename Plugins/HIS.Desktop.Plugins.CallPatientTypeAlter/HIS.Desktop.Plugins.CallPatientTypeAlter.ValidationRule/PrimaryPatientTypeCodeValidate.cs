using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter.ValidationRule
{
	internal class PrimaryPatientTypeCodeValidate : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal TextEdit txtPrimaryPatientTypeCode;

		internal GridLookUpEdit cboPrimaryPatientTypeCode;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtPrimaryPatientTypeCode == null || txtPrimaryPatientTypeCode == null)
				{
					return result;
				}
				if (string.IsNullOrEmpty(txtPrimaryPatientTypeCode.Text) || txtPrimaryPatientTypeCode.EditValue == null)
				{
					return result;
				}
				result = true;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}
	}
}
