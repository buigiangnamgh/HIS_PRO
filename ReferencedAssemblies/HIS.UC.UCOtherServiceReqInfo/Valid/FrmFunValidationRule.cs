using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class FrmFunValidationRule : ValidationRule
	{
		internal GridLookUpEdit cboCCT;

		internal UCOtherServiceReqInfo frm;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (cboCCT != null && cboCCT.EditValue != null)
				{
					HIS_TREATMENT val = ((frm != null) ? frm._HisTreatment : null);
					if (val == null)
					{
						return result;
					}
					if (string.IsNullOrEmpty(val.FUND_NUMBER))
					{
						return result;
					}
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
