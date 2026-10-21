using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using HIS.Desktop.LibraryMessage;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.PharmacyCashier.Validation
{
	internal class GridLookUpAndTextValidationRule : ValidationRule
	{
		internal GridLookUpEdit gridLookUp;

		internal TextEdit textBox;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (gridLookUp == null || textBox == null)
				{
					return result;
				}
				if (gridLookUp.EditValue == null || string.IsNullOrWhiteSpace(textBox.Text))
				{
					base.ErrorText = MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
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
