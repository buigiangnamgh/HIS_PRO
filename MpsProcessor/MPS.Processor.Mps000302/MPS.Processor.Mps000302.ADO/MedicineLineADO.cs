namespace MPS.Processor.Mps000302.ADO
{
	public class MedicineLineADO
	{
		public long? ID { get; set; }

		public int ROW_POS { get; set; }

		public string MEDICINE_LINE_CODE { get; set; }

		public string MEDICINE_LINE_NAME { get; set; }

		public decimal? TOTAL_PRICE_MEDICINE_LINE { get; set; }

		public decimal? TOTAL_HEIN_PRICE_MEDICINE_LINE { get; set; }

		public decimal? TOTAL_PATIENT_PRICE_MEDICINE_LINE { get; set; }

		public decimal? TOTAL_PATIENT_PRICE_SELF_MEDICINE_LINE { get; set; }

		public long? HEIN_SERVICE_TYPE_ID { get; set; }

		public long? REMEDY_COUNT { get; set; }

		public string KEY_PATY_ALTER { get; set; }

        public long GROUP_DEPARTMENT_ID { get; set; }
	}
}
