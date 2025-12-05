using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using HIS.Desktop.LibraryMessage;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.DebateDiagnostic
{
	internal class TimeValidationRule : ValidationRule
	{
		internal DateEdit DateEdit1;

		internal DateEdit DateEdit2;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (DateEdit1 == null)
				{
					return result;
				}
				if (DateEdit2 == null)
				{
					return result;
				}
				if (DateEdit2.EditValue != null && Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateEdit2.DateTime) < Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateEdit1.DateTime))
				{
					base.ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage((Inventec.Desktop.Common.LibraryMessage.Message.Enum)33);
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
