using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	internal class TreatmentTypeValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal LookUpEdit cboTreatmentType;

		internal TextEdit txtTreatmentTypeCode;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtTreatmentTypeCode == null || cboTreatmentType == null)
				{
					return result;
				}
				if (string.IsNullOrEmpty(txtTreatmentTypeCode.Text) || cboTreatmentType.EditValue == null)
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
