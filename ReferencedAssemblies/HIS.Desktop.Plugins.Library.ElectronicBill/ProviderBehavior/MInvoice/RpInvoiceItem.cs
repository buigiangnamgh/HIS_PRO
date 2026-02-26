namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MInvoice
{
	internal class RpInvoiceItem
	{
		public string stt { get; set; }

		public string ma { get; set; }

		public string inv_itemName { get; set; }

		public string inv_unitCode { get; set; }

		public decimal inv_unitPrice { get; set; }

		public decimal inv_quantity { get; set; }

		public decimal inv_TotalAmountWithoutVat { get; set; }

		public decimal inv_vatAmount { get; set; }

		public decimal inv_TotalAmount { get; set; }

		public decimal inv_promotion { get; set; }

		public decimal inv_discountPercentage { get; set; }

		public decimal inv_discountAmount { get; set; }

		public string ma_thue { get; set; }
	}
}
