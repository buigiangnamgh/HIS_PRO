using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;
using Inventec.Common.TypeConvert;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class PriorityType__ValidationRule : ValidationRule
	{
		internal LookUpEdit cboPriorityType;

		internal bool hasDataAutoCheckPriority;

		public override bool Validate(Control control, object value)
		{
			bool flag = true;
			try
			{
				flag = flag && cboPriorityType != null;
				if (flag && hasDataAutoCheckPriority && (cboPriorityType.EditValue == null || Parse.ToInt64((cboPriorityType.EditValue ?? "").ToString()) == 0))
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
