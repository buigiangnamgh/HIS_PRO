namespace Inventec.Common.SignFile
{
	public class TimestampConfig
	{
		private string tsa_acc;

		private string tsa_pass;

		private string tsa_url = "";

		private bool useTimestamp;

		public string TsaAcc
		{
			get
			{
				return tsa_acc;
			}
			set
			{
				tsa_acc = value;
			}
		}

		public string TsaPass
		{
			get
			{
				return tsa_pass;
			}
			set
			{
				tsa_pass = value;
			}
		}

		public string TsaUrl
		{
			get
			{
				return tsa_url;
			}
			set
			{
				tsa_url = value;
			}
		}

		public bool UseTimestamp
		{
			get
			{
				return useTimestamp;
			}
			set
			{
				useTimestamp = value;
			}
		}
	}
}
