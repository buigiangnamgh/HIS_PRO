using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MInvoice
{
	internal class RpInvoiceData
	{
		public string inv_originalId { get; set; }

		public string ngayvb { get; set; }

		public string ghi_chu { get; set; }

		public string sovb { get; set; }

		public string inv_invoiceSeries { get; set; }

		public string inv_invoiceIssuedDate { get; set; }

		public string inv_currencyCode { get; set; }

		public decimal inv_exchangeRate { get; set; }

		public string inv_buyerDisplayName { get; set; }

		public string inv_buyerLegalName { get; set; }

		public string inv_buyerTaxCode { get; set; }

		public string inv_buyerAddressLine { get; set; }

		public string inv_buyerEmail { get; set; }

		public string inv_buyerBankAccount { get; set; }

		public string inv_buyerBankName { get; set; }

		public string inv_paymentMethodName { get; set; }

		public List<RpInvoiceDetails> details { get; set; }
	}
}
