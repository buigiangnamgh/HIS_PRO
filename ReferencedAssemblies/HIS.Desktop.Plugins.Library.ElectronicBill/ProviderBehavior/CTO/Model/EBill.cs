using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.CTO.Model
{
	internal class EBill
	{
		public string id { get; set; }

		public string status { get; set; }

		public EbTemplate template { get; set; }

		public EbInfo buyer { get; set; }

		public EbInfo company { get; set; }

		public string creator { get; set; }

		public string issued_date { get; set; }

		public string currency { get; set; }

		public string method { get; set; }

		public List<EbProduct> items { get; set; }

		public string amount_inwords { get; set; }

		public EbVat vat { get; set; }

		public decimal total_net { get; set; }

		public decimal total_gross { get; set; }

		public string patient_id { get; set; }

		public string encounter { get; set; }
	}
}
