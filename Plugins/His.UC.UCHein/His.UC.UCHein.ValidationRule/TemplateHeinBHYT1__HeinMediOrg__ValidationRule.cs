using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;
using HIS.Desktop.LocalStorage.BackendData;
using Inventec.Common.Logging;
using Inventec.Desktop.CustomControl;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.ValidationRule
{
	internal class TemplateHeinBHYT1__HeinMediOrg__ValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
	{
		internal CustomGridLookUpEditWithFilterMultiColumn cbo;

		public override bool Validate(Control control, object value)
		{
			bool result = false;
			try
			{
				if (cbo.EditValue == null)
				{
					return false;
				}
				HIS_MEDI_ORG hIS_MEDI_ORG = BackendDataWorker.Get<HIS_MEDI_ORG>().FirstOrDefault((HIS_MEDI_ORG o) => o.MEDI_ORG_CODE == cbo.EditValue.ToString());
				if (!string.IsNullOrEmpty(BranchDataWorker.Branch.DO_NOT_ALLOW_HEIN_LEVEL_CODE) && hIS_MEDI_ORG != null && (";" + BranchDataWorker.Branch.DO_NOT_ALLOW_HEIN_LEVEL_CODE + ";").Contains(";" + hIS_MEDI_ORG.LEVEL_CODE + ";"))
				{
					base.ErrorText = string.Format("Nơi đăng ký khám chữa bệnh ban đầu thuộc tuyến {0}, không được hưởng BHYT", (hIS_MEDI_ORG.LEVEL_CODE == "1") ? "trung ương" : ((hIS_MEDI_ORG.LEVEL_CODE == "2") ? "Tỉnh" : ((hIS_MEDI_ORG.LEVEL_CODE == "3") ? "Huyện" : "Xã")));
					base.ErrorType = ErrorType.Warning;
					result = false;
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
