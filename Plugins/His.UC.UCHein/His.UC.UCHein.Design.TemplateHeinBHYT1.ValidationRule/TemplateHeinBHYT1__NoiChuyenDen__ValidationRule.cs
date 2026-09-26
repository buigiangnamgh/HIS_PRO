using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__NoiChuyenDen__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal GridLookUpEdit cboNoiChuyenDen;

		internal TextEdit txtMaNoiChuyenDen;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtMaNoiChuyenDen == null || cboNoiChuyenDen == null)
				{
					return result;
				}
				if (txtMaNoiChuyenDen.Enabled && cboNoiChuyenDen.Enabled && (string.IsNullOrEmpty(txtMaNoiChuyenDen.Text) || cboNoiChuyenDen.EditValue == null))
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
