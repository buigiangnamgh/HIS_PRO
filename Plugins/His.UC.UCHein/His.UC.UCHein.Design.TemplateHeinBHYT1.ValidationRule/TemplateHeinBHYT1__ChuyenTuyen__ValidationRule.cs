using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__ChuyenTuyen__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal CheckEdit chkMediRecordRouteTransfer;

		internal CheckEdit chkMediRecordNoRouteTransfer;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (chkMediRecordRouteTransfer == null || chkMediRecordNoRouteTransfer == null)
				{
					return result;
				}
				if (!chkMediRecordRouteTransfer.Checked && !chkMediRecordNoRouteTransfer.Checked)
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
