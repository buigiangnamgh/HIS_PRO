using MOS.EFMODEL.DataModels;

namespace MPS.Processor.Mps000302.ADO
{
	public class PatyAlterBhytADO : HIS_PATIENT_TYPE_ALTER
	{
		public string PATIENT_TYPE_NAME { get; set; }

		public string HEIN_CARD_NUMBER_SEPARATE { get; set; }

		public string IS_HEIN { get; set; }

		public string IS_VIENPHI { get; set; }

		public string STR_HEIN_CARD_FROM_TIME { get; set; }

		public string STR_HEIN_CARD_TO_TIME { get; set; }

		public string RATIO { get; set; }

		public string HEIN_CARD_NUMBER_1 { get; set; }

		public string HEIN_CARD_NUMBER_2 { get; set; }

		public string HEIN_CARD_NUMBER_3 { get; set; }

		public string HEIN_CARD_NUMBER_4 { get; set; }

		public string HEIN_CARD_NUMBER_5 { get; set; }

		public string HEIN_CARD_NUMBER_6 { get; set; }

		public long TIME_IN_TREATMENT { get; set; }

		public string RATIO_STR { get; set; }

		public string KEY { get; set; }

		public string KBCB_TIME_FROM_STR { get; set; }

		public string KBCB_TIME_TO_STR { get; set; }

		public decimal? TOTAL_PRICE { get; set; }

		public decimal? TOTAL_PRICE_BHYT { get; set; }

		public decimal? TOTAL_PRICE_HEIN { get; set; }

		public decimal? TOTAL_PRICE_OTHER { get; set; }

		public decimal? TOTAL_PRICE_PATIENT { get; set; }

		public decimal? TOTAL_PRICE_PATIENT_SELF { get; set; }

		public decimal TOTAL_PATIENT_PRICE_LEFT { get; set; }

		public decimal TOTAL_PRICE_VP { get; set; }

		public decimal TOTAL_BHYT_PRICE { get; set; }
	}
}
