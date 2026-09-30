using System;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.ServiceReqList
{
	internal class ValidateMaxLength : ValidationRule
	{
		internal MemoEdit memoEdit;

		internal int? maxLength;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (memoEdit == null)
				{
					return result;
				}
				if (string.IsNullOrEmpty(memoEdit.Text))
				{
					base.ErrorText = "Trường dữ liệu bắt buộc";
					return result;
				}
				if (!string.IsNullOrEmpty(memoEdit.Text) && Encoding.UTF8.GetByteCount(memoEdit.Text) > maxLength)
				{
					base.ErrorText = "Trường dữ liệu vượt quá ký tự cho phép";
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
