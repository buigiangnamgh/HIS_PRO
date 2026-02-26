namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.CTO.Model
{
	internal class EbProduct
	{
		public string name { get; set; }

		public string unit { get; set; }

		public double quantity { get; set; }

		public decimal price { get; set; }

		public decimal amount { get; set; }

		public int tax { get; set; }

		public decimal total { get; set; }
	}
}
