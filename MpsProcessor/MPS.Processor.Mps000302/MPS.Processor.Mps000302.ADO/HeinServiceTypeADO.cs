namespace MPS.Processor.Mps000302.ADO
{
	public class HeinServiceTypeADO
	{
		public long? ID { get; set; }

		public int ROW_POS { get; set; }

		public string HEIN_SERVICE_TYPE_CODE { get; set; }

		public string HEIN_SERVICE_TYPE_NAME { get; set; }

		public decimal? TOTAL_PRICE_BHYT_HEIN_SERVICE_TYPE { get; set; }

		public decimal? TOTAL_PRICE_HEIN_SERVICE_TYPE { get; set; }

		public decimal? TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE { get; set; }

		public decimal? TOTAL_PATIENT_PRICE_VIR_HEIN_SERVICE_TYPE { get; set; }

		public decimal? TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE { get; set; }

		public decimal? TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE { get; set; }

		public decimal? NUM_ORDER { get; set; }

		public string KEY_PATY_ALTER { get; set; }

		public long? PARENT_ID { get; set; }

		public long? MEDICINE_LINE_ID { get; set; }

		public decimal? OTHER_SOURCE_PRICE { get; set; }

		public long? HEIN_SERVICE_TYPE_CHILD_NUM_ORDER { get; set; }

		public decimal? TOTAL_BHYT_PRICE { get; set; }

		public decimal? TOTAL_PRICE { get; set; }

		public decimal? TOTAL_HEIN_PRICE { get; set; }

		public decimal? TOTAL_PATIENT_PRICE_SELF { get; set; }

		public decimal TOTAL_PATIENT_PRICE_LEFT { get; set; }

		public decimal TOTAL_PRICE_VP { get; set; }

        public long GROUP_DEPARTMENT_ID { get; set; }
	}
}
