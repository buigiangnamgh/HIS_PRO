using System;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;
using Inventec.Desktop.Common.Controls.ValidationRule;
using Inventec.Desktop.Common.LibraryMessage;

namespace HIS.Desktop.Plugins.DebateDiagnostic
{
	internal class CustomControlValidationRule : ValidationRule
	{
		internal object editor;

		internal IsValidControl validEditorReference;

		internal IsValidControl isValidControl;

		internal bool isUseOnlyCustomValidControl = false;

		internal int? maxLength;

		internal string messageError;

		internal bool IsRequired { get; set; }

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (editor == null)
				{
					return result;
				}
				if (!isUseOnlyCustomValidControl && editor is TextEdit)
				{
					if (IsRequired && ((validEditorReference == null && string.IsNullOrEmpty(((TextEdit)editor).Text)) || (validEditorReference != null && !validEditorReference.Invoke())))
					{
						base.ErrorText = ((!string.IsNullOrEmpty(messageError)) ? messageError : Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage((Inventec.Desktop.Common.LibraryMessage.Message.Enum)64));
						return result;
					}
					if (!string.IsNullOrEmpty(((TextEdit)editor).Text.Trim()) && Encoding.UTF8.GetByteCount(((TextEdit)editor).Text.Trim()) > maxLength)
					{
						int? num = maxLength;
						base.ErrorText = "Nhập quá kí tự cho phép [" + num + "]";
						return result;
					}
				}
				if (isValidControl != null && !isValidControl.Invoke())
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
