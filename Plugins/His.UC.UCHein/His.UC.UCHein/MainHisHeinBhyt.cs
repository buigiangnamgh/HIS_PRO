using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using HIS.Desktop.Plugins.Library.CheckHeinGOV;
using His.UC.UCHein.Base;
using His.UC.UCHein.Core.ResetValidationControl;
using His.UC.UCHein.Core.SetResultDataADO;
using His.UC.UCHein.Core.UpdateDataFormIntoPatientProfile;
using His.UC.UCHein.Core.UpdateDataFormIntoPatientTypeAlter;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using His.UC.UCHein.Dispose;
using His.UC.UCHein.FillDataToHeinInsuranceInfoByOldPatient;
using His.UC.UCHein.FillDataTranPatiInForm;
using His.UC.UCHein.Get.ExpriedTimeHeinCardBhyt;
using His.UC.UCHein.Init;
using His.UC.UCHein.Set.DefaultFocusUserControl;
using His.UC.UCHein.Set.FocusHeinCardFromTime;
using His.UC.UCHein.Set.InitValidationControl;
using His.UC.UCHein.Set.ResetValueControl;
using His.UC.UCHein.Set.SetFocusUserByLiveAreaCode;
using Inventec.Common.Logging;
using Inventec.Common.QrCodeBHYT;
using Inventec.Common.WebApiClient;
using MOS.EFMODEL.DataModels;
using MOS.SDO;

namespace His.UC.UCHein
{
	public class MainHisHeinBhyt : BusinessBase
	{
		public static string TEMPLATE__BHYT1 = "TemplateBHYT1";

		public void SetValueTreatmentType(UserControl uc, long TreatmentTypeId)
		{
			try
			{
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).SetTreatmentType(TreatmentTypeId);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void ShowComboSoThe(UserControl uc)
		{
			try
			{
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).ShowComboSoThe();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void SetValueLogTime(UserControl uc, long ltime)
		{
			try
			{
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).SetLogTime(ltime);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void SetValueAddress(UserControl uc, string address)
		{
			try
			{
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).SetValueAddress(address);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void FillDataAfterFindQrCode(UserControl uc, HeinCardData heinCardData)
		{
			try
			{
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).FillDataAfterFindQrCode(heinCardData);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void FillDataAfterCheckBHYT(UserControl uc, HeinCardData heinCardData)
		{
			try
			{
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).FillDataAfterCheckBHYT(heinCardData);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void UpdateHasDobCertificateEnable(UserControl uc, bool hasDobCertificate)
		{
			try
			{
				LogSystem.Debug("UpdateHasDobCertificateEnable t1. begin ep kieu");
				if (uc is Template__HeinBHYT1)
				{
					LogSystem.Debug("UpdateHasDobCertificateEnable t1. end ep kieu");
					LogSystem.Debug("UpdateHasDobCertificateEnable t2. begin update checkbox");
					((Template__HeinBHYT1)uc).UpdateHasDobCertificateEnable(hasDobCertificate);
					LogSystem.Debug("UpdateHasDobCertificateEnable t2. end update checkbox");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public CheckEdit GetchkHasDobCertificate(UserControl uc)
		{
			try
			{
				if (uc is Template__HeinBHYT1)
				{
					return ((Template__HeinBHYT1)uc).chkHasDobCertificate;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return null;
		}

		public void FillDataToHeinInsuranceInfoByOldPatient(UserControl uc, HisPatientSDO patient)
		{
			try
			{
				IRun run = FillDataToHeinInsuranceInfoByOldPatientFactory.MakeIFillDataToHeinInsuranceInfoByOldPatient(base.param, patient, uc);
				if (run != null)
				{
					run.Run();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void FillDataTranPatiInForm(UserControl uc, long treatmentId)
		{
			try
			{
				IRun run = FillDataTranPatiInFormFactory.MakeIFillDataTranPatiInForm(base.param, treatmentId, uc);
				if (run != null)
				{
					run.Run();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SelectMediOrgForSearch(UserControl uc, bool isSearch)
		{
			try
			{
				if (uc.GetType() == typeof(Template__HeinBHYT1))
				{
					((Template__HeinBHYT1)uc).SelectMediOrgForSearch(isSearch);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void FocusHeinCardFromTime(UserControl uc)
		{
			try
			{
				FocusHeinCardFromTimeFactory.MakeIFocusHeinCardFromTime(base.param, uc).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public long AlertExpriedTimeHeinCardBhyt(UserControl uc, long alertExpriedTimeHeinCardBhyt, ref long resultDayAlert)
		{
			try
			{
				return ExpriedTimeHeinCardBhytFactory.MakeIExpriedTimeHeinCardBhyt(base.param, uc, alertExpriedTimeHeinCardBhyt, ref resultDayAlert).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return 0L;
		}

		public void InitOldPatientData(UserControl uc, long? patientId, string heinCardNumber)
		{
			try
			{
				if (uc.GetType() == typeof(Template__HeinBHYT1))
				{
					((Template__HeinBHYT1)uc).InitOldPatientData(patientId.GetValueOrDefault(), heinCardNumber);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public UserControl InitUC(object data, ApiConsumer mosConsumer, string language)
		{
			UserControl userControl = null;
			TokenStore.language = language;
			try
			{
				ApiConsumerStore.MosConsumer = mosConsumer;
				return (UserControl)InitFactory.MakeIInitUC(base.param, data).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return null;
			}
		}

		public void ResetValue(UserControl UC)
		{
			try
			{
				ResetValueControlFactory.MakeIResetValueControl(base.param, UC).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void SetResultDataADOBhyt(UserControl UC, ResultDataADO ResultDataADO)
		{
			try
			{
				SetResultDataADOFactory.MakeISetResultDataADO(base.param, UC, ResultDataADO).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void UpdateDataFormIntoPatientProfile(UserControl UC, HisPatientProfileSDO patientProfileSDO)
		{
			try
			{
				UpdateDataFormIntoPatientProfileFactory.MakeIUpdateDataFormIntoPatientProfile(base.param, UC, patientProfileSDO).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void UpdateDataFormIntoPatientTypeAlter(UserControl UC, HisPatientProfileSDO patientProfileSDO)
		{
			try
			{
				UpdateDataFormIntoPatientTypeAlterFactory.MakeIUpdateDataFormIntoPatientProfile(base.param, UC, patientProfileSDO).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void ResetValidationControl(UserControl uc)
		{
			try
			{
				ResetValidationControlFactory.MakeIResetValidationControl(base.param, uc).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public bool FillDataHeinInsuranceInfoByPatientTypeAlter(UserControl uc, HIS_PATIENT_TYPE_ALTER patientTypeAlter)
		{
			bool result = false;
			try
			{
				if (uc == null)
				{
					throw new ArgumentNullException("ucHein__BHYT is null");
				}
				if (patientTypeAlter == null)
				{
					throw new ArgumentNullException("patientTypeAlter is null");
				}
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).ChangeDataHeinInsuranceInfoByPatientTypeAlter(patientTypeAlter);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		public bool GoiUCTuManHinhTiepDon(UserControl uc, bool _isCallByRegistor)
		{
			bool result = false;
			try
			{
				if (uc == null)
				{
					throw new ArgumentNullException("ucHein__BHYT is null");
				}
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).CoPhaiUCDuocGoiTuModuleTiepDonHayKhong(_isCallByRegistor);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		public void DefaultFocusUserControl(UserControl uc)
		{
			try
			{
				DefaultFocusUserControlFactory.MakeIDefaultFocusUserControl(base.param, uc).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void DisposeControl(UserControl uc)
		{
			try
			{
				DisposeFactory.MakeIDispose(base.param, uc).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public bool GetInvalidControls(UserControl uc)
		{
			try
			{
				return InitValidationControlFactory.MakeIInitValidationControl(base.param, uc).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return false;
		}

		public void SetFocusUserByLiveAreaCode(UserControl uc)
		{
			try
			{
				SetFocusUserByLiveAreaCodeFactory.MakeISetFocusUserByLiveAreaCode(base.param, uc).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void AutoCheckRightRoute(UserControl uc, bool IsDungTuyenCapCuu)
		{
			try
			{
				if (uc == null)
				{
					throw new ArgumentNullException("ucHein__BHYT is null");
				}
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).AutoCheckRightRoute(IsDungTuyenCapCuu);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void ChangeRoomNotEmergency(UserControl uc)
		{
			try
			{
				if (uc == null)
				{
					throw new ArgumentNullException("ucHein__BHYT is null");
				}
				if (uc is Template__HeinBHYT1)
				{
					((Template__HeinBHYT1)uc).ChangeRoomNotEmergency();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void PatientOldUnder6(UserControl UC, bool IsChild)
		{
			try
			{
				PatientOldUnder6Factory.MakeIPatientOldUnder6(base.param, UC, IsChild).Run();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
