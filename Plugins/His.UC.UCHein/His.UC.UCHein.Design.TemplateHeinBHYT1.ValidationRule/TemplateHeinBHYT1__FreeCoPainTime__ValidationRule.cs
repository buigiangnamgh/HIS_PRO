using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using His.UC.LibraryMessage;
using His.UC.UCHein.Base;
using His.UC.UCHein.Utils;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__FreeCoPainTime__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal ButtonEdit txtFreeCoPainTime;

		internal CheckEdit chkJoin5Year;

		internal CheckEdit chkPaid6Month;

		public override bool Validate(Control control, object value)
		{
			bool flag = true;
			try
			{
				flag = flag && txtFreeCoPainTime != null && chkJoin5Year != null && chkPaid6Month != null;
				if (flag)
				{
					string text = txtFreeCoPainTime.Text.Trim();
					if (chkJoin5Year.Checked && chkPaid6Month.Checked && string.IsNullOrEmpty(text))
					{
						base.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
						return false;
					}
					if (!string.IsNullOrEmpty(text))
					{
						if (text.Length == 8)
						{
							text = text.Substring(0, 2) + "/" + text.Substring(2, 2) + "/" + text.Substring(4, 4);
						}
						DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(text);
						if (!dateTime.HasValue || dateTime.Value == DateTime.MinValue)
						{
							base.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapNgayKhongHopLe);
							return false;
						}
						if (dateTime.Value.Year != DateTime.Now.Year)
						{
							base.ErrorText = ResourceMessage.ThoiDiemMienCungChiTraPhaiCungNamVoiNamHienTai;
							return false;
						}
						if (!chkJoin5Year.Checked || !chkPaid6Month.Checked)
						{
							base.ErrorText = ResourceMessage.PhaiDatDu5Nam6ThangMoiCoTheChonDTMCCT;
							return false;
						}
					}
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
