using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.MedicalStoreV2
{
	internal class ValidateThoiGianHenTra : ValidationRule
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
				if (dtTime.EditValue != null && dtTime.DateTime < DateTime.Now)
				{
					base.ErrorText = "Thời gian hẹn trả không được nhỏ hơn thời gian hiện tại";
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
