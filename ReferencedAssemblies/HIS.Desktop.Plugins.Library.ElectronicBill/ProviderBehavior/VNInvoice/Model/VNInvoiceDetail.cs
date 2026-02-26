namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VNInvoice.Model
{
	public class VNInvoiceDetail
	{
		public int index { get; set; }

		public decimal discountAmountBeforeTax { get; set; }

		public double discountPercentBeforeTax { get; set; }

		public decimal? paymentAmount { get; set; }

		public string productCode { get; set; }

		public int productType { get; set; }

		public string productName { get; set; }

		public string unitName { get; set; }

		public decimal? unitPrice { get; set; }

		public double quantity { get; set; }

		public decimal? amount { get; set; }

		public int vatPercent { get; set; }

		public decimal vatAmount { get; set; }

		public string note { get; set; }
	}
}
