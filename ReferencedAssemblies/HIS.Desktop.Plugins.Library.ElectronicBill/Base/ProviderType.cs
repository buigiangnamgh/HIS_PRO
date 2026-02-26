using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Base
{
	public class ProviderType
	{
		public const string VNPT = "VNPT";

		public const string VIETSENS = "VIETSENS";

		public const string BKAV = "BKAV";

		public const string VIETTEL = "VIETEL";

		public const string CongThuong = "MOIT";

		public const string SoftDream = "SODR";

		public const string MISA = "MISA";

		public const string safecert = "SAFECERT";

		public const string CTO = "CTO_PROXY";

		public const string BACH_MAI = "BACH_MAI";

		public const string MOBIFONE = "MOBIFONE";

		public const string CYBERBILL = "CYBERBILL";

		public const string MINVOICE = "MINVOICE";

		public const string VNINVOICE = "VNINVOICE";

		internal const int tax_0 = 1;

		internal const int tax_5 = 2;

		internal const int tax_10 = 3;

		internal const int tax_KCT = 4;

		internal const int tax_KKKT = 5;

		internal const int tax_K = 6;

		public static List<string> TYPE
		{
			get
			{
				return new List<string>
				{
					"VNPT", "VIETSENS", "BKAV", "VIETEL", "MOIT", "SODR", "MISA", "SAFECERT", "CTO_PROXY", "BACH_MAI",
					"MOBIFONE", "CYBERBILL", "MINVOICE", "VNINVOICE"
				};
			}
		}
	}
}
