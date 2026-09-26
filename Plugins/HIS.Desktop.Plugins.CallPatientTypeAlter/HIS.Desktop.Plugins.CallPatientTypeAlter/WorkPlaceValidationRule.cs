using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	internal class WorkPlaceValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal GridLookUpEdit cbo;

		internal TextEdit txt;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txt == null || cbo == null)
				{
					return result;
				}
				if (string.IsNullOrEmpty(txt.Text) && cbo.EditValue == null)
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
