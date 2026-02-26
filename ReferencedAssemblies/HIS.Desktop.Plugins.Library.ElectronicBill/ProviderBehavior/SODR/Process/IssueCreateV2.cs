using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR.Process
{
	public class IssueCreateV2
	{
		public string Pattern { get; set; }

		public List<InvoiceV2> Invoices { get; set; }
	}
}
