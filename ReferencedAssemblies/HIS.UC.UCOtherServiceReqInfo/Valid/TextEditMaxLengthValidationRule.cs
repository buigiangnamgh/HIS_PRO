using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using HIS.Desktop.LibraryMessage;
using Inventec.Common.Logging;
using Inventec.Common.String;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class TextEditMaxLengthValidationRule : ValidationRule
	{
		internal TextEdit txtEdit;

		internal int maxlength;

		internal bool isVali;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (isVali && (txtEdit == null || string.IsNullOrEmpty(txtEdit.Text)))
				{
					base.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
					return result;
				}
				if (txtEdit != null && !string.IsNullOrEmpty(txtEdit.Text) && CheckString.IsOverMaxLengthUTF8(txtEdit.Text, maxlength))
				{
					base.ErrorText = "Trường dữ liệu vượt quá maxlength( " + maxlength + " kí tự)";
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
