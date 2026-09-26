using System;
using System.Reflection;
using System.Resources;
using His.UC.UCHein.Base;
using Inventec.Common.Logging;
using Inventec.Common.Resource;

namespace His.UC.UCHein
{
	internal class ResourceMessage
	{
		internal static ResourceManager languageMessage = new ResourceManager("His.UC.UCHein.Resources.Message.Lang", Assembly.GetExecutingAssembly());

		internal static string PhaiDatDu5Nam6ThangMoiCoTheChonDTMCCT
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("PhaiDatDu5Nam6ThangMoiCoTheChonDTMCCT", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string MaBenhKhongKhopVoiTenBenh
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("MaBenhKhongKhopVoiTenBenh", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string BenhKhongKhuyenKhichDungLamBenhChinh
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("BenhKhongKhuyenKhichDungLamBenhChinh", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string MaBenhChinhKhongHopLe
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("MaBenhChinhKhongHopLe", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string BatBuocNhapTenBenhVoiTruongHopBenhNhanLaDungTuyenGioiThieu
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("BatBuocNhapTenBenhVoiTruongHopBenhNhanLaDungTuyenGioiThieu", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string ThoiDiemMienCungChiTraPhaiCungNamVoiNamHienTai
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("ThoiDiemMienCungChiTraPhaiCungNamVoiNamHienTai", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string SoTheBHYTKhongHopLe
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("SoTheBHYTKhongHopLe", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string SoTheDaDuocSuDung
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("SoTheDaDuocSuDung", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string SoTienLuyKeCungChiTraVuot06ThangLuongCoSo
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("SoTienLuyKeCungChiTraVuot06ThangLuongCoSo", languageMessage, LanguageManager.GetCulture());
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
