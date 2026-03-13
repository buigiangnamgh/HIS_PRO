using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class Combo___ValidationRule : ValidationRule
	{
		internal GridLookUpEdit cbo;

		internal TextEdit txt;

		public override bool Validate(Control control, object value)
		{
			bool result = true;
			try
			{
				if (txt == null || cbo == null)
				{
					result = false;
				}
				if (cbo.EditValue == null || (long)cbo.EditValue == 0L || string.IsNullOrEmpty(txt.Text))
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}
	}
}
