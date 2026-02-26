using System;
using System.Linq;
using System.Linq.Expressions;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Config
{
	public class HisPatientTypeCFG
	{
		private const string SDA_CONFIG__PATIENT_TYPE_CODE__IS_FREE = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.IS_FREE";

		private const string SDA_CONFIG__PATIENT_TYPE_CODE__BHYT = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";

		private const string SDA_CONFIG__PATIENT_TYPE_CODE__KSK = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.KSK";

		private const string SDA_CONFIG__PATIENT_TYPE_CODE__FEE = "MOS.HIS_PATIENT_TYPE.HOSPITAL_FEE";

		private const string SDA_CONFIG__PATIENT_TYPE_CODE__BILL_INVOICE = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BILL_INVOICE";

		private const string SDA_CONFIG__PATIENT_TYPE_CODE__SERVICE = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.SERVICE";

		private const string SDA_CONFIG__PATIENT_TYPE_CODE__THUPHI = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.THUPHI";

		private static long patientTypeIdIsService;

		private static long patientTypeIdIsThuPhi;

		private static long patientTypeIdIsFee;

		private static long patientTypeIdIsHein;

		private static long patientTypeIdIsFree;

		private static long patientTypeIdKsk;

		private static long patientTypeIdBillInvoice;

		private static string patientTypeCodeIsHein;

		private static string patientTypeCodeIsFree;

		private static string patientTypeCodeKsk;

		public static long PATIENT_TYPE_ID__IS_SERIVCE
		{
			get
			{
				if (patientTypeIdIsService == 0)
				{
					patientTypeIdIsService = GetId(HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.SERVICE"));
				}
				return patientTypeIdIsService;
			}
			set
			{
				patientTypeIdIsService = value;
			}
		}

		public static long PATIENT_TYPE_ID__IS_THUPHI
		{
			get
			{
				if (patientTypeIdIsThuPhi == 0)
				{
					patientTypeIdIsThuPhi = GetId(HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.THUPHI"));
				}
				return patientTypeIdIsThuPhi;
			}
			set
			{
				patientTypeIdIsThuPhi = value;
			}
		}

		public static long PATIENT_TYPE_ID__IS_FEE
		{
			get
			{
				if (patientTypeIdIsFee == 0)
				{
					patientTypeIdIsFee = GetId(HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.HOSPITAL_FEE"));
				}
				return patientTypeIdIsFee;
			}
			set
			{
				patientTypeIdIsFee = value;
			}
		}

		public static long PATIENT_TYPE_ID__BHYT
		{
			get
			{
				if (patientTypeIdIsHein == 0)
				{
					patientTypeIdIsHein = GetId(HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT"));
				}
				return patientTypeIdIsHein;
			}
			set
			{
				patientTypeIdIsHein = value;
			}
		}

		public static long PATIENT_TYPE_ID__IS_FREE
		{
			get
			{
				if (patientTypeIdIsFree == 0)
				{
					patientTypeIdIsFree = GetId("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.IS_FREE");
				}
				return patientTypeIdIsFree;
			}
			set
			{
				patientTypeIdIsFree = value;
			}
		}

		public static long PATIENT_TYPE_ID__KSK
		{
			get
			{
				if (patientTypeIdKsk == 0)
				{
					patientTypeIdKsk = GetId(HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.SERVICE"));
				}
				return patientTypeIdKsk;
			}
			set
			{
				patientTypeIdKsk = value;
			}
		}

		public static long PATIENT_TYPE_ID__BILL_INVOICE
		{
			get
			{
				if (patientTypeIdBillInvoice == 0)
				{
					patientTypeIdBillInvoice = GetId(HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BILL_INVOICE"));
				}
				return patientTypeIdBillInvoice;
			}
			set
			{
				patientTypeIdBillInvoice = value;
			}
		}

		public static string PATIENT_TYPE_CODE__BHYT
		{
			get
			{
				if (string.IsNullOrEmpty(patientTypeCodeIsHein))
				{
					patientTypeCodeIsHein = HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT");
				}
				return patientTypeCodeIsHein;
			}
			set
			{
				patientTypeCodeIsHein = value;
			}
		}

		public static string PATIENT_TYPE_CODE__IS_FREE
		{
			get
			{
				if (string.IsNullOrEmpty(patientTypeCodeIsFree))
				{
					patientTypeCodeIsFree = HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.IS_FREE");
				}
				return patientTypeCodeIsFree;
			}
			set
			{
				patientTypeCodeIsFree = value;
			}
		}

		public static string PATIENT_TYPE_CODE__KSK
		{
			get
			{
				if (string.IsNullOrEmpty(patientTypeCodeKsk))
				{
					patientTypeCodeKsk = HisConfigs.Get<string>("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.KSK");
				}
				return patientTypeCodeKsk;
			}
			set
			{
				patientTypeCodeKsk = value;
			}
		}

		private static long GetId(string code)
		{
			long num = 0L;
			try
			{
				HIS_PATIENT_TYPE val = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.PATIENT_TYPE_CODE == code);
				if (val == null || val.ID <= 0)
				{
					throw new ArgumentNullException(code + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => code)), (object)code));
				}
				return val.ID;
			}
			catch (Exception ex)
			{
				LogSystem.Debug(ex);
				return 0L;
			}
		}
	}
}
