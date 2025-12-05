namespace HIS.Desktop.Plugins.DebateDiagnostic.ADO
{
	public class InvateADO
	{
		public long? ID { get; set; }

		public string NAME { get; set; }

		public InvateADO(long? v1, string v2)
		{
			ID = v1;
			NAME = v2;
		}
	}
}
