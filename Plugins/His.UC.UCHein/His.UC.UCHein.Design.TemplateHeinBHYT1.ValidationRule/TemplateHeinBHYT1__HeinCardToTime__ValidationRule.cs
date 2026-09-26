using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using His.UC.LibraryMessage;
using His.UC.UCHein.Base;
using His.UC.UCHein.Config;
using His.UC.UCHein.Utils;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__HeinCardToTime__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal ButtonEdit txtHeinCardFromTime;

		internal ButtonEdit txtHeinCardToTime;

		internal CheckEdit checkKhongKTHSD;

		internal string isShowCheckKhongKTHSD;

		internal bool IsEdit;

		internal long PatientTypeId;

		internal long ExceedDayAllow;

		public override bool Validate(Control control, object value)
		{
			bool flag = true;
			try
			{
				flag = flag && txtHeinCardToTime != null && txtHeinCardFromTime != null && PatientTypeId > 0;
				if (flag)
				{
					HIS_PATIENT_TYPE hIS_PATIENT_TYPE = DataStore.PatientTypes.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == PatientTypeId && o.ID == Template__HeinBHYT1.PatientTypeIdBHYT);
					if (!txtHeinCardToTime.Enabled || hIS_PATIENT_TYPE == null)
					{
						return true;
					}
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
					DateTime? dateTime2 = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
					flag = flag && dateTime2.HasValue && dateTime2.Value != DateTime.MinValue;
					flag = flag && dateTime.HasValue && dateTime.Value != DateTime.MinValue;
					if (flag && dateTime2.HasValue && dateTime2.Value > dateTime.Value)
					{
						base.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapHanTheTuPhaiNhoHonHanTheDen);
						flag = false;
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
