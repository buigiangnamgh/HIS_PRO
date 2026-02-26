using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VNInvoice.Model
{
	internal class OutPutCreateInvoice
	{
		public List<OutputDataCreateAndSign> data { get; set; }

		public bool succeeded { get; set; }

		public int code { get; set; }

		public string message { get; set; }

		public string errors { get; set; }
	}
}
