using System.Collections.Generic;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Base
{
	public class RequestUriStore
	{
		internal const string MobifoneLogin = "/api/Account/Login";

		internal const string MobifoneGetDataReferencesByRefId = "/api/System/GetDataReferencesByRefId?refId=RF00059";

		internal const string MobifoneSaveListHoadon78 = "/api/Invoice68/SaveListHoadon78";

		internal const string MobifoneSignInvoiceCertFile68 = "/api/Invoice68/SignInvoiceCertFile68";

		internal const string MobifoneInHoadon = "/api/Invoice68/inHoadon?id={0}&type=PDF&inchuyendoi={1}";

		internal const string MobifoneuploadCanceledInv = "/api/Invoice68/uploadCanceledInv?id={0}";

		internal const string BDBillLogin = "api/services/hddtws/Authentication/GetToken";

		internal const string BDBillGuiVaKyHoadonGoc = "api/services/hddtws/QuanLyHoaDon/GuiVaKyHoaDonGocHSM";

		internal const string BDBillTaiHoaDonPdfKhongCanThueDuyet = "api/services/hddtws/TraCuuHoaDon/TaiHoaDonPdfKhongCanThueDuyet";

		internal const string BDBillThayTheHoaDon = "api/services/hddtws/QuanLyHoaDon/LapHoaDonThayThe";

		internal const string BDBillTraCuuHoaDon = "api/services/hddtws/TraCuuHoaDon/TraThongTinHoaDon";

		internal const string BDBillChuyenDoiHoaDon = "api/services/hddtws/QuanLyHoaDon/ChuyenDoiHoaDon";

		internal const string BDBillHuyHoaDon = "api/services/hddtws/QuanLyHoaDon/LapHoaDonHuyBo";

		internal const string CyberbillLogin = "api/services/hddtws/Authentication/GetToken";

		internal const string CyberbillGuiHoadonGoc = "api/services/hddtws/GuiHoadon/GuiHoadonGoc";

		internal const string CyberbillKyHoaDonHSM = "api/services/hddtws/XuLyHoaDon/KyHoaDonHSM";

		internal const string CyberbillChuyenDoiHoaDon = "api/services/hddtws/QuanLyHoaDon/ChuyenDoiHoaDon";

		internal const string CyberbillChuyenDoiHoaDonV2 = "api/services/hddtws/GuiHoaDon/ChuyenDoiHoaDon";

		internal const string CyberbillTaiHoaDon = "api/services/hddtws/GuiHoaDon/TaiHoaDonPDF";

		internal const string CyberbillHuyHoaDon = "api/services/hddtws/GuiHoaDon/GuiHoadonHuyBo";

		internal const string CyberbillGuiHoadonThayThe = "api/services/hddtws/GuiHoadon/GuiHoadonThayThe";

		internal const string CyberbillGuiVaKyHoaDonGoc = "api/services/hddtws/GuiHoaDon/GuiVaKyHoadonGocHSM";

		internal static string CombileUrl(params string[] data)
		{
			string text = "";
			List<string> list = new List<string>();
			for (int i = 0; i < data.Length; i++)
			{
				list.Add(data[i].Trim('/'));
			}
			return string.Join("/", list);
		}
	}
}
