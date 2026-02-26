using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR.Process
{
	public class InvoiceV2
	{
		public string TaxAuthorityCode { get; set; }

		public string ArisingDate { get; set; }

		public string CusAddress { get; set; }

		public string CusName { get; set; }

		public string CusPhone { get; set; }

		public string CusTaxCode { get; set; }

		public string CusBankName { get; set; }

		public string CusBankNo { get; set; }

		public string Email { get; set; }

		public string Extra { get; set; }

		public string Ikey { get; set; }

		public string PaymentMethod { get; set; }

		public List<ProductV2> Products { get; set; }

		public string BuyerName { get; set; }

		public string CusCode { get; set; }
	}
}
