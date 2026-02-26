namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.CYBERBILL.Model
{
	public class OutputConvertElectronicBill
	{
		public ResultConvertElectronicBill result { get; set; }

		public string targetUrl { get; set; }

		public bool success { get; set; }

		public ErrorCyberbill error { get; set; }

		public bool unAuthorizedRequest { get; set; }

		public bool __abp { get; set; }
	}
}
