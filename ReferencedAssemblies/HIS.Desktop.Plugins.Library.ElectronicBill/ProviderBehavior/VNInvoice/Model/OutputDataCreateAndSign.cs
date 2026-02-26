namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VNInvoice.Model
{
	public class OutputDataCreateAndSign
	{
		public string id { get; set; }

		public string erpId { get; set; }

		public string transactionId { get; set; }

		public int templateNo { get; set; }

		public string serialNo { get; set; }

		public string invoiceNo { get; set; }

		public int invoiceStatus { get; set; }

		public int signStatus { get; set; }
	}
}
