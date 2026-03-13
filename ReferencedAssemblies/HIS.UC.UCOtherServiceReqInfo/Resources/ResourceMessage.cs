using System;
using System.Reflection;
using System.Resources;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Desktop.Common.LanguageManager;

namespace HIS.UC.UCOtherServiceReqInfo.Resources
{
	internal class ResourceMessage
	{
		internal static ResourceManager languageMessage = new ResourceManager("HIS.UC.UCOtherServiceReqInfo.Resources.Message.Lang", Assembly.GetExecutingAssembly());

		internal static string SoThuTuUuTienPhaiNamTrongDanhSachCauHinhCacSoUuTien
		{
			get
			{
				try
				{
					return Get.Value("SoThuTuUuTienPhaiNamTrongDanhSachCauHinhCacSoUuTien", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string NhapNgayThangKhongDungDinhDang
		{
			get
			{
				try
				{
					return Get.Value("Plugin_Register_NhapNgayThangKhongDungDinhDang", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string ChuaNhapThongTinDoiTuongCungChiTra
		{
			get
			{
				try
				{
					return Get.Value("Plugin_Register_ChuaNhapThongTinDoiTuongCungChiTra", languageMessage, LanguageManager.GetCulture());
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
