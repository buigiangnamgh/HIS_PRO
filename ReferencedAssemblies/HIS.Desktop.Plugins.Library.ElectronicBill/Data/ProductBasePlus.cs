namespace HIS.Desktop.Plugins.Library.ElectronicBill.Data
{
	public class ProductBasePlus : ProductBase
	{
		public decimal? TaxAmount { get; set; }

		public decimal? AmountWithoutTax { get; set; }

		public int? TaxPercentage { get; set; }

		public decimal TaxConvert { get; set; }
	}
}
