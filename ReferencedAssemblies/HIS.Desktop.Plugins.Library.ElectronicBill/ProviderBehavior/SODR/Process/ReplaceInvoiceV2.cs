using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR.Process
{
	public class ReplaceInvoiceV2
	{
		public string Pattern { get; set; }

		public string Ikey { get; set; }

		public List<InvoiceV2> Invoices { get; set; }
	}
}
