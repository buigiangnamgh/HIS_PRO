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
	internal class TemplateHeinBHYT1__HeinCardFromTime__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal ButtonEdit txtHeinCardFromTime;

		internal long PatientTypeId;

		public override bool Validate(Control control, object value)
		{
			bool flag = true;
			try
			{
				flag = flag && txtHeinCardFromTime != null && PatientTypeId > 0;
				if (flag)
				{
					HIS_PATIENT_TYPE hIS_PATIENT_TYPE = DataStore.PatientTypes.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == PatientTypeId && o.ID == Template__HeinBHYT1.PatientTypeIdBHYT);
					if (!txtHeinCardFromTime.Enabled || hIS_PATIENT_TYPE == null)
					{
						return true;
					}
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
					flag = flag && dateTime.HasValue && dateTime.Value != DateTime.MinValue;
					if (flag && dateTime.Value.Date > DateTime.Now.Date)
					{
						base.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapNgayPhaiNhoHonNgayHienTai);
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
