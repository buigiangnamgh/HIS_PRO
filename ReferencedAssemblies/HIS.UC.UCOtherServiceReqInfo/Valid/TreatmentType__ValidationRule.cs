using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class TreatmentType__ValidationRule : ValidationRule
	{
		internal LookUpEdit cboTreatmentType;

		public override bool Validate(Control control, object value)
		{
			bool flag = true;
			try
			{
				flag = flag && cboTreatmentType != null;
				if (flag && (cboTreatmentType.EditValue == null || (long)cboTreatmentType.EditValue == 0))
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
