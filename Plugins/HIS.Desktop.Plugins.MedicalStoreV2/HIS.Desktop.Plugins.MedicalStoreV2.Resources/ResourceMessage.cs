using System;
using System.Reflection;
using System.Resources;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Desktop.Common.LanguageManager;

namespace HIS.Desktop.Plugins.MedicalStoreV2.Resources
{
	internal class ResourceMessage
	{
		private static ResourceManager languageMessage = new ResourceManager("HIS.Desktop.Plugins.MedicalStoreV2.Resources.Message.Lang", Assembly.GetExecutingAssembly());

		internal static string DaKetThuc
		{
			get
			{
				try
				{
					return Get.Value("DaKetThuc", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string ChuaKetThuc
		{
			get
			{
				try
				{
					return Get.Value("ChuaKetThuc", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

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

		internal static string VuiLongChonHoSoCanLuuTru
		{
			get
			{
				try
				{
					return Get.Value("VuiLongChonHoSoCanLuuTru", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TatCa
		{
			get
			{
				try
				{
					return Get.Value("TatCa", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string XuLyThanhCong
		{
			get
			{
				try
				{
					return Get.Value("XuLyThanhCong", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string SoluuTruCuHoSoLa
		{
			get
			{
				try
				{
					return Get.Value("SoluuTruCuHoSoLa", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string BanCoMuonXoaDuLieuKhong
		{
			get
			{
				try
				{
					return Get.Value("BanCoMuonXoaDuLieuKhong", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string BanCothucSuMuonHuyTuChoiDuyetBenhAn
		{
			get
			{
				try
				{
					return Get.Value("BanCothucSuMuonHuyTuChoiDuyetBenhAn", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string KhongTimThayFile
		{
			get
			{
				try
				{
					return Get.Value("KhongTimThayFile", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string BieuMauDangMoHoacKhongTonTaiFile
		{
			get
			{
				try
				{
					return Get.Value("BieuMauDangMoHoacKhongTonTaiFile", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string XuatFileThanhCong
		{
			get
			{
				try
				{
					return Get.Value("XuatFileThanhCong", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string BanCoMuonMoFile
		{
			get
			{
				try
				{
					return Get.Value("BanCoMuonMoFile", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string XuLyThatBai
		{
			get
			{
				try
				{
					return Get.Value("XuLyThatBai", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string DaKiemTra
		{
			get
			{
				try
				{
					return Get.Value("DaKiemTra", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string ChuaKiemTra
		{
			get
			{
				try
				{
					return Get.Value("ChuaKiemTra", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string XacNhanDanhDauDaKiemTra
		{
			get
			{
				try
				{
					return Get.Value("XacNhanDanhDauDaKiemTra", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string KhongDuQuyenDanhDauDaKiemTra
		{
			get
			{
				try
				{
					return Get.Value("KhongDuQuyenDanhDauDaKiemTra", languageMessage, LanguageManager.GetCulture());
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
