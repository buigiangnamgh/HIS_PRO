using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__Icd__ValidationRule_Is_MediOrg : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal TextEdit txtIcdName;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtIcdName == null)
				{
					return result;
				}
				if (txtIcdName.Enabled && string.IsNullOrEmpty(txtIcdName.Text))
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
