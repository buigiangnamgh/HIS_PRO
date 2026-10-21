using System;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.MedicalStoreV2.Validtion
{
	public class ValidateMaxLength : ValidationRule
	{
		internal TextEdit textEdit;

		internal int? maxLength;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (textEdit == null)
				{
					return result;
				}
				if (string.IsNullOrEmpty(textEdit.Text))
				{
					base.ErrorText = "Trường dữ liệu bắt buộc nhập";
					base.ErrorType = ErrorType.Warning;
					return result;
				}
				if (!string.IsNullOrEmpty(textEdit.Text) && Encoding.UTF8.GetByteCount(textEdit.Text) > maxLength)
				{
					int? num = maxLength;
					base.ErrorText = "Vượt quá độ dài cho phép (" + num + ")";
					base.ErrorType = ErrorType.Warning;
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
