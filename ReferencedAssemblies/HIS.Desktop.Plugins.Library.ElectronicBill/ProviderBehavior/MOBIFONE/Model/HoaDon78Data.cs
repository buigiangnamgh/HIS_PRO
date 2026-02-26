using System;
using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MOBIFONE.Model
{
	public class HoaDon78Data
	{
		public string cctbao_id { get; set; }

		public string hdon_id { get; set; }

		public string nlap { get; set; }

		public string sdhang { get; set; }

		public string dvtte { get; set; }

		public int tgia { get; set; }

		public string htttoan { get; set; }

		public string stknban { get; set; }

		public string tnhban { get; set; }

		public string mnmua { get; set; }

		public string mst { get; set; }

		public string tnmua { get; set; }

		public string email { get; set; }

		public string ten { get; set; }

		public string dchi { get; set; }

		public string stknmua { get; set; }

		public string sdtnmua { get; set; }

		public string tnhmua { get; set; }

		public decimal tgtcthue { get; set; }

		public decimal tgtthue { get; set; }

		public decimal tgtttbso { get; set; }

		public decimal tgtttbso_last { get; set; }

		public decimal tkcktmn { get; set; }

		public decimal ttcktmai { get; set; }

		public decimal tgtphi { get; set; }

		public string mdvi { get; set; }

		public int tthdon { get; set; }

		public int is_hdcma { get; set; }

		public string hdon_id_old { get; set; }

		public int lhdclquan { get; set; }

		public string khmshdclquan { get; set; }

		public string khhdclquan { get; set; }

		public string shdclquan { get; set; }

		public DateTime nlhdclquan { get; set; }

		public List<HoaDon78Details> details { get; set; }

		public List<HoaDon78Phi> hoadon68_phi { get; set; }

		public List<HoaDon78Khac> hoadon68_khac { get; set; }
	}
}
