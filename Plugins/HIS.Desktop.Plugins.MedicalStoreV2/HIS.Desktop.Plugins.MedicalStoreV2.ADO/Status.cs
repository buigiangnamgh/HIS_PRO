namespace HIS.Desktop.Plugins.MedicalStoreV2.ADO
{
	public class Status
	{
		public long id { get; set; }

		public string statusName { get; set; }

		public Status(long id, string statusName)
		{
			this.id = id;
			this.statusName = statusName;
		}
	}
}
