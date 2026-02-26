namespace HIS.Desktop.Plugins.Library.ElectronicBill.Data
{
	public class ProductBase
	{
		public string ProdCode { get; set; }

		public string ProdName { get; set; }

		public string ProdUnit { get; set; }

		public decimal? ProdQuantity { get; set; }

		public decimal? ProdPrice { get; set; }

		public decimal Amount { get; set; }

		public int Type { get; set; }

		public int TaxRateID { get; set; }

		public long Stt { get; set; }

		public bool IsBHYT { get; set; }
	}
}
