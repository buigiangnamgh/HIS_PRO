using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	internal class PatientTypeValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal TextEdit txtMaDoiTuong;

		internal LookUpEdit cboDoiTuong;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtMaDoiTuong == null || cboDoiTuong == null)
				{
					return result;
				}
				if (string.IsNullOrEmpty(txtMaDoiTuong.Text) || cboDoiTuong.EditValue == null)
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
