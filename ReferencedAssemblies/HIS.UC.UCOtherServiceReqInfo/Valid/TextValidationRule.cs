using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class TextValidationRule : ValidationRule
	{
		internal TextEdit txtText;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtText == null)
				{
					return result;
				}
				if (string.IsNullOrEmpty(txtText.Text.Trim()))
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
