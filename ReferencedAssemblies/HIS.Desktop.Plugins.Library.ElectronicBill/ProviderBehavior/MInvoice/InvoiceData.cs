using System;
using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MInvoice
{
	internal class InvoiceData
	{
		public DateTime tdlap { get; set; }

		public string khieu { get; set; }

		public string shdon { get; set; }

		public string sdhang { get; set; }

		public string dvtte { get; set; }

		public decimal tgia { get; set; }

		public string htttoan { get; set; }

		public string tnmua { get; set; }

		public string mnmua { get; set; }

		public string ten { get; set; }

		public string cccdan { get; set; }

		public string sdtnmua { get; set; }

		public string mst { get; set; }

		public string dchi { get; set; }

		public string email { get; set; }

		public string stknmua { get; set; }

		public string nganhang_ngmua { get; set; }

		public decimal ttcktmai { get; set; }

		public decimal tgtcthue { get; set; }

		public decimal tgtthue { get; set; }

		public decimal tgtttbso { get; set; }

		public string tgtttbchu { get; set; }

		public string nguoi_thu { get; set; }

		public string thongtin_khoa { get; set; }

		public List<InvoiceDetails> details { get; set; }
	}
}
