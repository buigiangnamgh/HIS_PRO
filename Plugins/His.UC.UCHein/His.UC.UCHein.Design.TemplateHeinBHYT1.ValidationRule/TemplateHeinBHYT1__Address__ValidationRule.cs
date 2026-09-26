using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;
using Inventec.Common.String;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__Address__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal TextEdit txtAddress;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtAddress == null)
				{
					return result;
				}
				if (txtAddress.Enabled && string.IsNullOrEmpty(txtAddress.Text.Trim()))
				{
					return result;
				}
				if (txtAddress.Enabled && txtAddress != null && !string.IsNullOrEmpty(txtAddress.Text) && CheckString.IsOverMaxLengthUTF8(txtAddress.Text, 500))
				{
					base.ErrorText = "Trường dữ liệu vượt quá độ dài (" + 500 + " kí tự)";
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
