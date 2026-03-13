using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class Priority__ValidationRule : ValidationRule
	{
		internal CheckEdit chkPriority;

		internal bool hasDataAutoCheckPriority;

		public override bool Validate(Control control, object value)
		{
			bool flag = true;
			try
			{
				flag = flag && chkPriority != null;
				if (flag && hasDataAutoCheckPriority && !chkPriority.Checked)
				{
					flag = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return flag;
		}
	}
}
