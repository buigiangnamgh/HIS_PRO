using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;
using Inventec.Common.TypeConvert;

namespace HIS.Desktop.Plugins.ServiceReqList
{
	internal class ValidateBiggerThan0 : ValidationRule
	{
		internal SpinEdit spinEdit;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (Parse.ToInt64(spinEdit.EditValue.ToString()) < 1)
				{
					base.ErrorText = "Số ngày nhập phải lớn hơn 0";
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
