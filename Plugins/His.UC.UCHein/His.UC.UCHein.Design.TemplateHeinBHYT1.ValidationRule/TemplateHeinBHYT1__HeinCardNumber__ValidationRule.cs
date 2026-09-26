using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using His.UC.LibraryMessage;
using His.UC.UCHein.Base;
using His.UC.UCHein.Config;
using His.UC.UCHein.Utils;
using Inventec.Common.Logging;
using Inventec.Common.String;
using MOS.EFMODEL.DataModels;
using MOS.LibraryHein.Bhyt;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__HeinCardNumber__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal TextEdit txtSoThe;

		internal long PatientTypeId;

		internal CheckEdit chkHasDobCertificate;

		internal List<HIS_BHYT_BLACKLIST> BhytBlackLists;

		internal List<HIS_BHYT_WHITELIST> BhytWhiteLists;

		public override bool Validate(Control control, object value)
		{
			bool flag = true;
			try
			{
				flag = flag && txtSoThe != null;
				flag = flag && PatientTypeId > 0;
				flag = flag && chkHasDobCertificate != null;
				if (!txtSoThe.Enabled)
				{
					return flag;
				}
				HIS_PATIENT_TYPE hIS_PATIENT_TYPE = DataStore.PatientTypes.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == PatientTypeId);
				if (hIS_PATIENT_TYPE == null)
				{
					LogSystem.Debug("Khong tim thay thong tin doi tuong benh nhan theo Id doi tuong. PatientTypeId = " + PatientTypeId);
					flag = true;
				}
				else if (hIS_PATIENT_TYPE.ID == Template__HeinBHYT1.PatientTypeIdBHYT)
				{
					flag = flag && !string.IsNullOrEmpty(txtSoThe.Text) && !string.IsNullOrEmpty(txtSoThe.Text.Trim());
					if (flag)
					{
						string text = HeinUtils.TrimHeinCardNumber(txtSoThe.Text.Replace(" ", "").ToUpper());
						flag = flag && new BhytHeinProcessor().IsValidHeinCardNumber(text) && !CheckString.IsOverMaxLengthUTF8(text, 17);
						if (!flag)
						{
							base.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapSoTheBHYTKhongHopLe);
						}
						else
						{
							string heinCardNumberCode = text.Substring(0, 3).ToString();
							List<HIS_BHYT_WHITELIST> list = BhytWhiteLists.Where((HIS_BHYT_WHITELIST p) => p.BHYT_WHITELIST_CODE == heinCardNumberCode).ToList();
							if (list != null && list.Count() > 0)
							{
								if (BhytBlackLists != null)
								{
									foreach (HIS_BHYT_BLACKLIST bhytBlackList in BhytBlackLists)
									{
										if (text.StartsWith(bhytBlackList.HEIN_CARD_NUMBER))
										{
											base.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.His_UCHein__SoTheBHYTNamTrongDanhSachDenVuiLongKiemTraLai);
											flag = !flag && false;
											break;
										}
									}
								}
							}
							else
							{
								base.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapSoTheBHYTKhongHopLe);
								flag = !flag && false;
							}
						}
					}
					else
					{
						base.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
					}
				}
				else
				{
					flag = true;
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
