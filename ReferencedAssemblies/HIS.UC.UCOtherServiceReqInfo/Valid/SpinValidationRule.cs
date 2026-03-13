using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class SpinValidationRule : ValidationRule
	{
		internal SpinEdit spinEdit;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (spinEdit == null)
				{
					return result;
				}
				if (string.IsNullOrEmpty(spinEdit.Text))
				{
					return result;
				}
				if (spinEdit.Value < 0m)
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
