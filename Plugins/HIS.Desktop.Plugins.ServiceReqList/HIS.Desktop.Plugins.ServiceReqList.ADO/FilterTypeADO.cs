namespace HIS.Desktop.Plugins.ServiceReqList.ADO
{
	internal class FilterTypeADO
	{
		public long ID { get; set; }

		public string FilterTypeName { get; set; }

		public FilterTypeADO(long id, string filterTypeName)
		{
			ID = id;
			FilterTypeName = filterTypeName;
		}
	}
}
