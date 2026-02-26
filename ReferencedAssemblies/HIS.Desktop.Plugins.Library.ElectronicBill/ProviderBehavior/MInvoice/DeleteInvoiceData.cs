using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MInvoice
{
	public class DeleteInvoiceData
	{
		public string editmode { get; set; }

		public List<DeleteDataItem> data { get; set; }
	}
}
