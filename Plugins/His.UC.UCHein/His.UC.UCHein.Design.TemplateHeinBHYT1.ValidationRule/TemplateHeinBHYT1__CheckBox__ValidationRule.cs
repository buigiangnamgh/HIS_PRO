using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__CheckBox__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal ButtonEdit txt;

		internal CheckEdit chk;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				string value2 = txt.Text.Trim();
				if (chk.Checked && string.IsNullOrEmpty(value2))
				{
					base.ErrorText = "Trường dữ liệu bắt buộc";
					return false;
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
