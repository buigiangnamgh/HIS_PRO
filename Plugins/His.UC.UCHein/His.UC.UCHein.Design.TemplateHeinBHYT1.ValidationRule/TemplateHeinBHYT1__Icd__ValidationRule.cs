using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using His.UC.LibraryMessage;
using His.UC.UCHein.Base;
using His.UC.UCHein.Config;
using Inventec.Common.Logging;
using Inventec.Common.TypeConvert;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__Icd__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal TextEdit txtMaChanDoanTD;

		internal GridLookUpEdit cboChanDoanTD;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (txtMaChanDoanTD == null || cboChanDoanTD == null)
				{
					return result;
				}
				if (!string.IsNullOrEmpty(txtMaChanDoanTD.Text))
				{
					if (cboChanDoanTD.EditValue == null)
					{
						base.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
						return result;
					}
					HIS_ICD hIS_ICD = ((DataStore.Icds != null && DataStore.Icds.Count > 0) ? DataStore.Icds.FirstOrDefault((HIS_ICD o) => o.ICD_CODE.ToUpper() == txtMaChanDoanTD.Text.ToUpper()) : null);
					if (hIS_ICD == null)
					{
						base.ErrorText = ResourceMessage.MaBenhChinhKhongHopLe;
						return result;
					}
					if (hIS_ICD.ID != Parse.ToInt64((cboChanDoanTD.EditValue ?? "0").ToString()) || hIS_ICD.ICD_NAME != cboChanDoanTD.Text)
					{
						base.ErrorText = string.Format(ResourceMessage.MaBenhKhongKhopVoiTenBenh, txtMaChanDoanTD.Text, cboChanDoanTD.Text);
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
