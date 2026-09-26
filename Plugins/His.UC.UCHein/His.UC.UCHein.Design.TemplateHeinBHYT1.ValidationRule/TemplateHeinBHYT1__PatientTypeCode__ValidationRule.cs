using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__PatientTypeCode__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal GridLookUpEdit cboPatientCode;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (cboPatientCode == null)
				{
					return result;
				}
				if (cboPatientCode.Enabled && cboPatientCode.EditValue == null)
				{
					return result;
				}
				if (cboPatientCode.EditValue.ToString() == "" || string.IsNullOrEmpty(cboPatientCode.Text))
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
