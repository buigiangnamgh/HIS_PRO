using System;
using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Base
{
	public class ElectronicBillResult
	{
		public bool Success { get; set; }

		public string InvoiceCode { get; set; }

		public string InvoiceSys { get; set; }

		public List<string> Messages { get; set; }

		public string InvoiceLink { get; set; }

		public string InvoiceNumOrder { get; set; }

		public string InvoiceLoginname { get; set; }

		public string InvoiceLookupCode { get; set; }

		public long? InvoiceTime { get; set; }

		public byte[] PdfFileData { get; set; }

		public Guid hoadon68_id { get; set; }

		public string Status { get; set; }
	}
}
