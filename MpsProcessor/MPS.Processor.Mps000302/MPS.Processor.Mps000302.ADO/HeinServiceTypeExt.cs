using System.Collections.Generic;

namespace MPS.Processor.Mps000302.ADO
{
	public class HeinServiceTypeExt
	{
		public const long THUOC_TRUYENDICH__ID = 123L;

		public const string THUOC_TRUYENDICH__NAME = "Thuốc, dịch truyền";

		public const long VT_Y_TE__ID = 124L;

		public const string VT_Y_TE__NAME = "Vật tư y tế";

		public const long BED__ID = 125L;

		public const string BED__NAME = "Giường";

		public const long GOI_VT_Y_TE__ID = 126L;

		public const string GOI_VT_Y_TE__NAME = "Gói vật tư y tế";

		public static readonly List<long> HEIN_BED__IDs = new List<long> { 19L, 20L, 3L, 4L };
	}
}
