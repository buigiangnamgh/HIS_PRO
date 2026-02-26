namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.CYBERBILL.Model
{
	public class ResultCyberbill
	{
		public string access_token { get; set; }

		public string encrypted_access_token { get; set; }

		public int expire_in_seconds { get; set; }

		public string token_type { get; set; }
	}
}
