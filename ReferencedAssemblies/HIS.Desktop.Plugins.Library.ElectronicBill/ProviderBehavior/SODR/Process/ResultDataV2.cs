using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR.Process
{
	public class ResultDataV2
	{
		public string Status { get; set; }

		public string Message { get; set; }

		public string Data { get; set; }

		public List<InvoiceRs> InvoiceResult { get; set; }

		public string AccessToken { get; set; }
	}
}
