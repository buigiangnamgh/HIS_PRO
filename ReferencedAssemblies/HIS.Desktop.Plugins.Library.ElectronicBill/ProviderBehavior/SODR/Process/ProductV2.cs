namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR.Process
{
	public class ProductV2
	{
		public string Code { get; set; }

		public string Feature { get; set; }

		public string No { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public decimal Quantity { get; set; }

		public string Unit { get; set; }

		public decimal Discount { get; set; }

		public decimal DiscountAmount { get; set; }

		public decimal Total { get; set; }

		public decimal VATAmount { get; set; }

		public float VATRate { get; set; }

		public decimal Amount { get; set; }

		public string Extra { get; set; }
	}
}
