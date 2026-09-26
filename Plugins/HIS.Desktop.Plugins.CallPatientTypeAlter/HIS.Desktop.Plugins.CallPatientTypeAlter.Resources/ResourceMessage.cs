using System;
using System.Reflection;
using System.Resources;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Desktop.Common.LanguageManager;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter.Resources
{
	internal class ResourceMessage
	{
		internal static ResourceManager languageMessage = new ResourceManager("HIS.Desktop.Plugins.CallPatientTypeAlter.Resources.Message.Lang", Assembly.GetExecutingAssembly());

		internal static string RaVien
		{
			get
			{
				try
				{
					return Get.Value("RaVien", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string ChuyenVien
		{
			get
			{
				try
				{
					return Get.Value("ChuyenVien", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TrongVien
		{
			get
			{
				try
				{
					return Get.Value("TrongVien", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string XinRaVien
		{
			get
			{
				try
				{
					return Get.Value("XinRaVien", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string Khoi
		{
			get
			{
				try
				{
					return Get.Value("Khoi", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string Do
		{
			get
			{
				try
				{
					return Get.Value("Do", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string KhongThayDoi
		{
			get
			{
				try
				{
					return Get.Value("KhongThayDoi", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string NangHon
		{
			get
			{
				try
				{
					return Get.Value("NangHon", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TuVong
		{
			get
			{
				try
				{
					return Get.Value("TuVong", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string Ma
		{
			get
			{
				try
				{
					return Get.Value("Ma", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string Ten
		{
			get
			{
				try
				{
					return Get.Value("Ten", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TLTT
		{
			get
			{
				try
				{
					return Get.Value("TLTT", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string DichVuBatBuocChonDieuKien
		{
			get
			{
				try
				{
					return Get.Value("DichVuBatBuocChonDieuKien", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TheNayDaDuocSD
		{
			get
			{
				try
				{
					return Get.Value("TheNayDaDuocSD", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TongSoTienCuaCacDichVu
		{
			get
			{
				try
				{
					return Get.Value("TongSoTienCuaCacDichVu", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string SuaTTBN
		{
			get
			{
				try
				{
					return Get.Value("SuaTTBN", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string DVKhongTheChuyenDoi
		{
			get
			{
				try
				{
					return Get.Value("DVKhongTheChuyenDoi", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string CapNhatTTDTTB
		{
			get
			{
				try
				{
					return Get.Value("CapNhatTTDTTB", languageMessage, LanguageManager.GetCulture());
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

		internal static string DaCoThongTinDoiTuongBHYT
		{
			get
			{
				try
				{
					return Get.Value("DaCoThongTinDoiTuongBHYT", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string BNCoTTDienDoiTuong
		{
			get
			{
				try
				{
					return Get.Value("BNCoTTDienDoiTuong", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string BNTreEmCanNhapDuTT
		{
			get
			{
				try
				{
					return Get.Value("BNTreEmCanNhapDuTT", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string DoiTuongKhamSucKhoe
		{
			get
			{
				try
				{
					return Get.Value("DoiTuongKhamSucKhoe", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TheSapHetHan
		{
			get
			{
				try
				{
					return Get.Value("TheSapHetHan", languageMessage, LanguageManager.GetCulture());
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

		internal static string TheBhytKhongHopLeBanCoMuonSuDung
		{
			get
			{
				try
				{
					return Get.Value("TheBhytKhongHopLeBanCoMuonSuDung", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TheBhytKhongHopLeKhongChoPhepDangKy
		{
			get
			{
				try
				{
					return Get.Value("TheBhytKhongHopLeKhongChoPhepDangKy", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string GoiSangCongBHXHTraVeMaLoi
		{
			get
			{
				try
				{
					return Get.Value("GoiSangCongBHXHTraVeMaLoi", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TheSaiNgaySinhGov070
		{
			get
			{
				try
				{
					return Get.Value("Gov070", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TheSaiHTenGov060
		{
			get
			{
				try
				{
					return Get.Value("Gov060", languageMessage, LanguageManager.GetCulture());
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
