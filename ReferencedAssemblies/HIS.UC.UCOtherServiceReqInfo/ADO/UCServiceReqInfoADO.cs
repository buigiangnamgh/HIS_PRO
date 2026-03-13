using System;

namespace HIS.UC.UCOtherServiceReqInfo.ADO
{
	public class UCServiceReqInfoADO
	{
		public string IN_CODE { get; set; }

		public long IntructionTime { get; set; }

		public long TreatmentType_ID { get; set; }

		public bool IsPriority { get; set; }

		public bool IsEmergency { get; set; }

		public bool IsNotRequireFee { get; set; }

		public bool IsChronic { get; set; }

		public long EmergencyTime_ID { get; set; }

		public long OweType_ID { get; set; }

		public long? PriorityNumber { get; set; }

		public long? PriorityType { get; set; }

		public long? TreatmentOrder { get; set; }

		public long? OTHER_PAY_SOURCE_ID { get; set; }

		public long FUND_ID { get; set; }

		public string FUND_NUMBER { get; set; }

		public decimal? FUND_BUDGET { get; set; }

		public string FUND_COMPANY_NAME { get; set; }

		public long? FUND_FROM_TIME { get; set; }

		public long? FUND_TO_TIME { get; set; }

		public long? FUND_ISSUE_TIME { get; set; }

		public string FUND_TYPE_NAME { get; set; }

		public string FUND_CUSTOMER_NAME { get; set; }

		public long? PATIENT_CLASSIFY_ID { get; set; }

		public string GUARANTEE_LOGINNAME { get; set; }

		public string GUARANTEE_USERNAME { get; set; }

		public string GUARANTEE_REASON { get; set; }

		public string NOTE { get; set; }

		public bool IsCapMaMS { get; set; }

		public string MaMS { get; set; }

		public bool IsWarningForNext { get; set; }

		public bool IsHiv { get; set; }

		public bool IsTuberCulosis { get; set; }

		public Action<object> _FocusNextUserControl { get; set; }

		public string HospitalizeReasonCode { get; set; }

		public string HospitalizeReasonName { get; set; }

		public bool IsExamOnline { get; set; }

		public string HospitalizationReason { get; set; }

		public bool IsCAPD { get; set; }

		public short? IS_CAPD { get; set; }

		public string NguonKhachCode { get; set; }

		public string NguonKhachName { get; set; }

		public string NguonKhachCTName { get; set; }

		public bool isChamSocDa { get; set; }
	}
}
