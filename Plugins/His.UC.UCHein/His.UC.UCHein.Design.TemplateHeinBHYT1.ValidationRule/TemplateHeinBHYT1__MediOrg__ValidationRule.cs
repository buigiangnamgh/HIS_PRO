using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using HIS.Desktop.LocalStorage.BackendData;
using His.UC.UCHein.Config;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule
{
	internal class TemplateHeinBHYT1__MediOrg__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal TextEdit txtMaDKKCBBD;

		internal GridLookUpEdit cboDKKCBBD;

		internal long PatientTypeId;

		internal CheckEdit chkHasDobCertificate;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (!txtMaDKKCBBD.Enabled && !cboDKKCBBD.Enabled)
				{
					result = true;
					return result;
				}
				if (txtMaDKKCBBD == null || cboDKKCBBD == null || chkHasDobCertificate == null || PatientTypeId <= 0)
				{
					return result;
				}
				if (DataStore.PatientTypes.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == PatientTypeId) != null && (string.IsNullOrEmpty(txtMaDKKCBBD.Text) || cboDKKCBBD.EditValue == null))
				{
					return result;
				}
				HIS_MEDI_ORG hIS_MEDI_ORG = BackendDataWorker.Get<HIS_MEDI_ORG>().FirstOrDefault((HIS_MEDI_ORG o) => o.MEDI_ORG_CODE == cboDKKCBBD.EditValue.ToString());
				if (!string.IsNullOrEmpty(BranchDataWorker.Branch.DO_NOT_ALLOW_HEIN_LEVEL_CODE) && hIS_MEDI_ORG != null && (";" + BranchDataWorker.Branch.DO_NOT_ALLOW_HEIN_LEVEL_CODE + ";").Contains(";" + hIS_MEDI_ORG.LEVEL_CODE + ";"))
				{
					base.ErrorText = string.Format("Nơi đăng ký khám chữa bệnh ban đầu thuộc tuyến {0}, không được hưởng BHYT", (hIS_MEDI_ORG.LEVEL_CODE == "1") ? "trung ương" : ((hIS_MEDI_ORG.LEVEL_CODE == "2") ? "Tỉnh" : ((hIS_MEDI_ORG.LEVEL_CODE == "3") ? "Huyện" : "Xã")));
					base.ErrorType = ErrorType.Warning;
					return false;
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
