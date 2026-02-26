namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MInvoice
{
	public class DeleteInvoiceResult
	{
		public string code { get; set; }

		public string message { get; set; }

		public bool ok { get; set; }

		public InvoiceDataDelete data { get; set; }
	}
}
