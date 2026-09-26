using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__RightRouteType__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal TextEdit txtHeinRightRouteCode;

		internal LookUpEdit cboHeinRightRoute;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtHeinRightRouteCode == null || cboHeinRightRoute == null)
				{
					return result;
				}
				if (txtHeinRightRouteCode.Enabled && cboHeinRightRoute.Enabled && (string.IsNullOrEmpty(txtHeinRightRouteCode.Text) || cboHeinRightRoute.EditValue == null))
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
