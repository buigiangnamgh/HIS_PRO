using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VNInvoice.Model
{
	internal class InputCreateAndSignInvoice
	{
		public int TemplateNo { get; set; }

		public string SerialNo { get; set; }

		public string erpId { get; set; }

		public string creatorErp { get; set; }

		public string transactionId { get; set; }

		public string invoiceDate { get; set; }

		public string note { get; set; }

		public string paymentMethod { get; set; }

		public string currency { get; set; }

		public double exchangeRate { get; set; }

		public decimal totalAmount { get; set; }

		public decimal totalVatAmount { get; set; }

		public decimal totalPaymentAmount { get; set; }

		public double totalDiscountAmountBeforeTax { get; set; }

		public double totalDiscountAmountAfterTax { get; set; }

		public int totalDiscountPercentAfterTax { get; set; }

		public string buyerCode { get; set; }

		public string buyerEmail { get; set; }

		public string buyerFullName { get; set; }

		public string buyerLegalName { get; set; }

		public string buyerTaxCode { get; set; }

		public string buyerAddressLine { get; set; }

		public string buyerDistrictName { get; set; }

		public string buyerCityName { get; set; }

		public string buyerCountryCode { get; set; }

		public string buyerPhoneNumber { get; set; }

		public string buyerFaxNumber { get; set; }

		public string buyerBankAccount { get; set; }

		public string buyerBankName { get; set; }

		public List<VNInvoiceDetail> invoiceDetails { get; set; }

		public List<InvoiceHeaderExtra> invoiceHeaderExtras { get; set; }

		public List<InvoiceDetailExtra> invoiceDetailExtras { get; set; }

		public List<InvoiceTaxBreakdown> invoiceTaxBreakdowns { get; set; }
	}
}
