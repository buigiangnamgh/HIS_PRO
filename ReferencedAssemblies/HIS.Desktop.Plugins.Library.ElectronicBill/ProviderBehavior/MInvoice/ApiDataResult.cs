namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MInvoice
{
	internal class ApiDataResult
	{
		public string code { get; set; }

		public string message { get; set; }

		public InvoiceResult data { get; set; }

		public string error { get; set; }

		public string token { get; set; }
	}
}
