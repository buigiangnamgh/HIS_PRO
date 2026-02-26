namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.CTO.Model
{
	internal class CancelTransactionInvoiceRequest
	{
		public string id { get; set; }

		public string supplierTaxCode { get; set; }

		public string invoiceNo { get; set; }

		public string templateCode { get; set; }

		public string strIssueDate { get; set; }

		public string additionalReferenceDesc { get; set; }

		public string additionalReferenceDate { get; set; }
	}
}
