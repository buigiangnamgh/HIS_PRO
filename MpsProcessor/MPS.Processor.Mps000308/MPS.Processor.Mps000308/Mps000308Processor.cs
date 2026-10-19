using System;
using System.Globalization;
using System.IO;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MPS.Processor.MPS000308.PDO;
using MPS.ProcessorBase.Core;

namespace MPS.Processor.MPS000308
{
	public class Mps000308Processor : AbstractProcessor
	{
		private Mps000308PDO rdo;

		private NumberStyles style = NumberStyles.Any;

		public Mps000308Processor(CommonParam param, PrintData printData)
			: base(param, printData)
		{
			rdo = (Mps000308PDO)rdoBase;
		}

		public override bool ProcessData()
		{
			bool flag = false;
			try
			{
				ProcessSingleTag processSingleTag = new ProcessSingleTag();
				ProcessBarCodeTag processBarCodeTag = new ProcessBarCodeTag();
				ProcessObjectTag processObjectTag = new ProcessObjectTag();
				SetSingleKey();
				ProcessPrintLogData();
				SetNumOrderKey(GetNumOrderPrint(ProcessUniqueCodeData()));
				store.ReadTemplate(Path.GetFullPath(fileName));
				processSingleTag.ProcessData(store, singleValueDictionary);
				flag = true;
			}
			catch (Exception ex)
			{
				flag = false;
				LogSystem.Error(ex);
			}
			return flag;
		}

		private void SetSingleKey()
		{
			try
			{
				if (rdo._PatientTypeAlter != null && !string.IsNullOrWhiteSpace(rdo._PatientTypeAlter.ADDRESS))
				{
					SetSingleKey(new KeyValue("TDL_HIEN_CARD_ADDRESS", rdo._PatientTypeAlter.ADDRESS));
				}
				if (rdo._Patient != null)
				{
					if (!string.IsNullOrWhiteSpace(rdo._Patient.CMND_NUMBER))
					{
						SetSingleKey(new KeyValue("CMND_NUMBER", rdo._Patient.CMND_NUMBER));
						SetSingleKey(new KeyValue("CMND_DATE_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo._Patient.CMND_DATE.GetValueOrDefault())));
						SetSingleKey(new KeyValue("CMND_PLACE", rdo._Patient.CMND_PLACE ?? ""));
					}
					else
					{
						SetSingleKey(new KeyValue("CMND_NUMBER", rdo._Patient.CCCD_NUMBER));
						SetSingleKey(new KeyValue("CMND_DATE_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo._Patient.CCCD_DATE.GetValueOrDefault())));
						SetSingleKey(new KeyValue("CMND_PLACE", rdo._Patient.CCCD_PLACE ?? ""));
					}
					SetSingleKey(new KeyValue("ETHNIC_NAME", rdo._Patient.ETHNIC_NAME ?? ""));
					SetSingleKey(new KeyValue("ADDRESS", rdo._Patient.ADDRESS ?? ""));
					SetSingleKey(new KeyValue("HT_ADDRESS", rdo._Patient.HT_ADDRESS ?? ""));
					SetSingleKey(new KeyValue("HT_COMMUNE_NAME", rdo._Patient.HT_COMMUNE_NAME ?? ""));
					SetSingleKey(new KeyValue("HT_DISTRICT_NAME", rdo._Patient.HT_DISTRICT_NAME ?? ""));
					SetSingleKey(new KeyValue("HT_PROVINCE_NAME", rdo._Patient.HT_PROVINCE_NAME ?? ""));
					SetSingleKey(new KeyValue("VIR_HT_ADDRESS", rdo._Patient.VIR_HT_ADDRESS ?? ""));
				}
				if (rdo._Treatment != null)
				{
					AddObjectKeyIntoListkey(rdo._Treatment, false);
					if (rdo._Treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == 1)
					{
						SetSingleKey(new KeyValue("DOB_STR", rdo._Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4)));
					}
					else
					{
						SetSingleKey(new KeyValue("DOB_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo._Treatment.TDL_PATIENT_DOB)));
					}
					switch (rdo._Treatment.NUMBER_OF_TESTS ?? (-1))
					{
					case 1:
						SetSingleKey(new KeyValue("NUMBER_OF_TESTS_STR", "3 lần /3 kỳ"));
						break;
					case 2:
						SetSingleKey(new KeyValue("NUMBER_OF_TESTS_STR", "≥ 4 lần / 3 kỳ"));
						break;
					default:
						SetSingleKey(new KeyValue("NUMBER_OF_TESTS_STR", ""));
						break;
					}
					switch (rdo._Treatment.TEST_HIV ?? (-1))
					{
					case 1:
						SetSingleKey(new KeyValue("TEST_HIV_STR", "Trước và trong mang thai"));
						break;
					case 2:
						SetSingleKey(new KeyValue("TEST_HIV_STR", "Trong chuyển dạ"));
						break;
					default:
						SetSingleKey(new KeyValue("TEST_HIV_STR", ""));
						break;
					}
					switch (rdo._Treatment.TEST_SYPHILIS ?? (-1))
					{
					case 1:
						SetSingleKey(new KeyValue("TEST_SYPHILIS_STR", "Trong mang thai"));
						break;
					case 2:
						SetSingleKey(new KeyValue("TEST_SYPHILIS_STR", "Trong chuyển dạ"));
						break;
					default:
						SetSingleKey(new KeyValue("TEST_SYPHILIS_STR", ""));
						break;
					}
					switch (rdo._Treatment.TEST_HEPATITIS_B ?? (-1))
					{
					case 1:
						SetSingleKey(new KeyValue("TEST_HEPATITIS_B_STR", "Trong mang thai"));
						break;
					case 2:
						SetSingleKey(new KeyValue("TEST_HEPATITIS_B_STR", "Trong chuyển dạ"));
						break;
					default:
						SetSingleKey(new KeyValue("TEST_HEPATITIS_B_STR", ""));
						break;
					}
					switch (rdo._Treatment.NEWBORN_CARE_AT_HOME ?? (-1))
					{
					case 1:
						SetSingleKey(new KeyValue("NEWBORN_CARE_AT_HOME_STR", "Tuần đầu"));
						break;
					case 2:
						SetSingleKey(new KeyValue("NEWBORN_CARE_AT_HOME_STR", "Từ tuần 2 đến hết 6"));
						break;
					default:
						SetSingleKey(new KeyValue("NEWBORN_CARE_AT_HOME_STR", ""));
						break;
					}
				}
				if (rdo._CurrentBaby != null)
				{
					short? iS_REISSUED = rdo._CurrentBaby.IS_REISSUED;
					string value = ((iS_REISSUED.GetValueOrDefault() == 0 || !iS_REISSUED.HasValue) ? "X" : "");
					string value2 = ((iS_REISSUED == 1) ? "X" : "");
					SetSingleKey(new KeyValue("IS_ISSUED_STR", value));
					SetSingleKey(new KeyValue("IS_REISSUED_STR", value2));
					AddObjectKeyIntoListkey(rdo._CurrentBaby, false);
					SetSingleKey(new KeyValue("BABY_ETHNIC_CODE", rdo._CurrentBaby.ETHNIC_CODE ?? ""));
					SetSingleKey(new KeyValue("BABY_ETHNIC_NAME", rdo._CurrentBaby.ETHNIC_NAME ?? ""));
					SetSingleKey(new KeyValue("BABY_FATHER_NAME", rdo._CurrentBaby.FATHER_NAME ?? ""));
					SetSingleKey(new KeyValue("BABY_GENDER_CODE", rdo._CurrentBaby.GENDER_CODE ?? ""));
					SetSingleKey(new KeyValue("BABY_GENDER_NAME", rdo._CurrentBaby.GENDER_NAME ?? ""));
					SetSingleKey(new KeyValue("BABY_HEAD", rdo._CurrentBaby.HEAD));
					SetSingleKey(new KeyValue("BABY_HEIGHT", rdo._CurrentBaby.HEIGHT));
					SetSingleKey(new KeyValue("BABY_MIDWIFE", rdo._CurrentBaby.MIDWIFE ?? ""));
					SetSingleKey(new KeyValue("BABY_MONTH_COUNT", rdo._CurrentBaby.MONTH_COUNT));
					SetSingleKey(new KeyValue("BABY_NAME", rdo._CurrentBaby.BABY_NAME ?? ""));
					SetSingleKey(new KeyValue("BABY_ORDER", rdo._CurrentBaby.BABY_ORDER));
					SetSingleKey(new KeyValue("BABY_WEEK_COUNT", rdo._CurrentBaby.WEEK_COUNT));
					SetSingleKey(new KeyValue("BABY_WEIGHT", rdo._CurrentBaby.WEIGHT));
					SetSingleKey(new KeyValue("BORN_POSITION_CODE", rdo._CurrentBaby.BORN_POSITION_CODE ?? ""));
					SetSingleKey(new KeyValue("BORN_POSITION_NAME", rdo._CurrentBaby.BORN_POSITION_NAME ?? ""));
					SetSingleKey(new KeyValue("BORN_RESULT_CODE", rdo._CurrentBaby.BORN_RESULT_CODE ?? ""));
					SetSingleKey(new KeyValue("BORN_RESULT_NAME", rdo._CurrentBaby.BORN_RESULT_NAME ?? ""));
					SetSingleKey(new KeyValue("BORN_TIME", rdo._CurrentBaby.BORN_TIME));
					SetSingleKey(new KeyValue("BORN_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo._CurrentBaby.BORN_TIME.GetValueOrDefault())));
					SetSingleKey(new KeyValue("BORN_TYPE_CODE", rdo._CurrentBaby.BORN_TYPE_CODE));
					SetSingleKey(new KeyValue("BORN_TYPE_NAME", rdo._CurrentBaby.BORN_TYPE_NAME));
					SetSingleKey(new KeyValue("BORN_TIME", rdo._CurrentBaby.BORN_TIME));
					SetSingleKey(new KeyValue("BORN_TIME", rdo._CurrentBaby.BORN_TIME));
					SetSingleKey(new KeyValue("CURRENT_ALIVE", rdo._CurrentBaby.CURRENT_ALIVE));
					SetSingleKey(new KeyValue("BIRTH_CERT_NUM", rdo._CurrentBaby.BIRTH_CERT_NUM));
					SetSingleKey(new KeyValue("BABY_NOTE", rdo._CurrentBaby.NOTE));
				}
				SetSingleKey(new KeyValue("BABY_COUNT", rdo._CountBaby));
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public override string ProcessPrintLogData()
		{
			string text = "";
			try
			{
				text = "TREATMENT_CODE:" + rdo._Treatment.TREATMENT_CODE;
				text = text + "BIRTH_CERT_NUM:" + rdo._CurrentBaby.BIRTH_CERT_NUM;
				text = text + "BIRTH_CERT_BOOK_ID:" + rdo._CurrentBaby.BIRTH_CERT_BOOK_ID;
				text = text + "BABY_NAME  : " + rdo._CurrentBaby.BABY_NAME;
				text = text + "BABY_NOTE:" + rdo._CurrentBaby.NOTE;
			}
			catch (Exception ex)
			{
				text = "";
				LogSystem.Error("___1 ##" + ((ex != null) ? ex.ToString() : null));
			}
			return text;
		}

		public override string ProcessUniqueCodeData()
		{
			string text = "";
			try
			{
				if (rdo != null)
				{
					text = ((rdo._Treatment == null || rdo._CurrentBaby == null) ? string.Format("{0} TREATMENT_CODE:{1} BABY_ID:{2}", printTypeCode, "", "") : string.Format("{0} TREATMENT_CODE:{1} BABY_ID:{2}", printTypeCode, rdo._Treatment.TREATMENT_CODE, rdo._CurrentBaby.ID));
					LogSystem.Info("UniqueCodeData: " + text);
				}
			}
			catch (Exception ex)
			{
				text = "";
				LogSystem.Error("___2 ##" + ((ex != null) ? ex.ToString() : null));
			}
			return text;
		}
	}
}
