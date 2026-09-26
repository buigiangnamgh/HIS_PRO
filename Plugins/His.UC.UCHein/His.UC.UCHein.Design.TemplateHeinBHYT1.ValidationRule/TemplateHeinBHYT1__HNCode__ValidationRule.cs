using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;
using Inventec.Common.String;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__HNCode__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal TextEdit txtHNCode;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtHNCode == null)
				{
					return result;
				}
				string text = txtHNCode.Text.Trim();
				if (!txtHNCode.Enabled || (txtHNCode.Enabled && string.IsNullOrEmpty(text)))
				{
					return true;
				}
				if (CheckString.IsOverMaxLengthUTF8(text, 20))
				{
					base.ErrorText = "Nhập quá kí tự cho phép (20)";
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
