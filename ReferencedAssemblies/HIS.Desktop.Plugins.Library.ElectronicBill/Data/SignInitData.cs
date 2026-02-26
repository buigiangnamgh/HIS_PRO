using HIS.Desktop.Plugins.Library.ElectronicBill.Base;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Data
{
	public class SignInitData
	{
		public DelegateSignAndRelease SignAndRelease { get; set; }

		public string ContentSign { get; set; }

		public byte[] fileToBytes { get; set; }

		public string FileDownload { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }
	}
}
