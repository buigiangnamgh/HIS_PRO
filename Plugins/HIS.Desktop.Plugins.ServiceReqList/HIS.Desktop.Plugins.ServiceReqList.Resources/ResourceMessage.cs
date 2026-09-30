using System;
using System.Reflection;
using System.Resources;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Desktop.Common.LanguageManager;

namespace HIS.Desktop.Plugins.ServiceReqList.Resources
{
	internal class ResourceMessage
	{
		private static ResourceManager languageMessage = new ResourceManager("HIS.Desktop.Plugins.ServiceReqList.Resources.Message.Lang", Assembly.GetExecutingAssembly());

		internal static string ThongBao
		{
			get
			{
				try
				{
					return Get.Value("ThongBao", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TaiKhoanKhongCoQuyenThucHienChucNang
		{
			get
			{
				try
				{
					return Get.Value("TaiKhoanKhongCoQuyenThucHienChucNang", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string ChucNangDangPhatTrienVuiLongThuLaiSau
		{
			get
			{
				try
				{
					return Get.Value("ChucNangDangPhatTrienVuiLongThuLaiSau", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string HeThongTBCuaSoThongBaoBanCoMuonHuyDuLieuKhong
		{
			get
			{
				try
				{
					return Get.Value("HeThongTBCuaSoThongBaoBanCoMuonHuyDuLieuKhong", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TruongDuLieuBatBuoc
		{
			get
			{
				try
				{
					return Get.Value("TruongDuLieuBatBuoc", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string CanhBao
		{
			get
			{
				try
				{
					return Get.Value("CanhBao", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string KhongCoQuyenXoaYeuCauBenhNhan
		{
			get
			{
				try
				{
					return Get.Value("KhongCoQuyenXoaYeuCauBenhNhan", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string YeuCau
		{
			get
			{
				try
				{
					return Get.Value("YeuCau", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string Duyet
		{
			get
			{
				try
				{
					return Get.Value("Duyet", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string ThuocVatTuNgoaiDanhMuc
		{
			get
			{
				try
				{
					return Get.Value("ThuocVatTuNgoaiDanhMuc", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string BanChuaChonDichVu
		{
			get
			{
				try
				{
					return Get.Value("BanChuaChonDichVu", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string DichVuKhongCungHoSoDieuTri
		{
			get
			{
				try
				{
					return Get.Value("DichVuKhongCungHoSoDieuTri", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string DichVuKhongCungThoiGianChiDinh
		{
			get
			{
				try
				{
					return Get.Value("DichVuKhongCungThoiGianChiDinh", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string DichVuLaThuoc
		{
			get
			{
				try
				{
					return Get.Value("DichVuLaThuoc", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string DonMauKhongChoPhepSua
		{
			get
			{
				try
				{
					return Get.Value("DonMauKhongChoPhepSua", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string KhongTimThayMaXuat
		{
			get
			{
				try
				{
					return Get.Value("KhongTimThayMaXuat", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string HoSoDieuTriDangTamKhoa
		{
			get
			{
				try
				{
					return Get.Value("HoSoDieuTriDangTamKhoa", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string KhongTimThayHoSoDieuTri
		{
			get
			{
				try
				{
					return Get.Value("KhongTimThayHoSoDieuTri", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string DonKhongLayKhongChoPhepSua
		{
			get
			{
				try
				{
					return Get.Value("DonKhongLayKhongChoPhepSua", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string Ngaynhapphailonhon0
		{
			get
			{
				try
				{
					return Get.Value("Ngaynhapphailonhon0", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}
	}
}
