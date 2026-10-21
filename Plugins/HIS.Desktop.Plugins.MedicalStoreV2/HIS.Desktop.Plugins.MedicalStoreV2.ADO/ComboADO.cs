namespace HIS.Desktop.Plugins.MedicalStoreV2.ADO
{
	public class ComboADO
	{
		public long ID { get; set; }

		public string NAME { get; set; }

		public ComboADO()
		{
		}

		public ComboADO(long id, string name)
		{
			ID = id;
			NAME = name;
		}
	}
}
