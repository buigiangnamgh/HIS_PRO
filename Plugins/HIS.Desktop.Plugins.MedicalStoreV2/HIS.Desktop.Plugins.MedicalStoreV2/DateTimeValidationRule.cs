using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using HIS.Desktop.LibraryMessage;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.MedicalStoreV2
{
	internal class DateTimeValidationRule : ValidationRule
	{
		internal DateEdit dtTime;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (dtTime == null)
				{
					return result;
				}
				if (dtTime.EditValue == null)
				{
					base.ErrorText = MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
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
