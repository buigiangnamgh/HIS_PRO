namespace HIS.Desktop.Plugins.ServiceReqList.ADO
{
	internal class ConfigADO
	{
		public enum RowConfigID
		{
			KhongHienThiDonKhongLayODonThuocTH = 1
		}

		public long ID { get; set; }

		public string NAME { get; set; }

		public bool IsChecked { get; set; }
	}
}
