using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class TimeValidationRule : ValidationRule
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
				if (string.IsNullOrEmpty(dtTime.Text))
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
