using System;
using System.Reflection;
using System.Resources;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Desktop.Common.LanguageManager;

namespace HIS.Desktop.Plugins.DebateDiagnostic.Resources
{
	internal class ResourceMessage
	{
		private static ResourceManager languageMessage = new ResourceManager("HIS.Desktop.Plugins.DebateDiagnostic.Resources.Message.Lang", Assembly.GetExecutingAssembly());

		internal static string ThongBao
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("ThongBao", languageMessage, LanguageManager.GetCulture());
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
					return Inventec.Common.Resource.Get.Value("TaiKhoanKhongCoQuyenThucHienChucNang", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string KhongTimThayIcdTuongUngVoiCacMaSau
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("KhongTimThayIcdTuongUngVoiCacMaSau", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string ThoiGianHoiChanKhongDuocNhoHonThoiGianVao
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("ThoiGianHoiChanKhongDuocNhoHonThoiGianVao", languageMessage, LanguageManager.GetCulture());
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				return "";
			}
		}

		internal static string TaiKhoanBilap
		{
			get
			{
				try
				{
					return Inventec.Common.Resource.Get.Value("TaiKhoanBilap", languageMessage, LanguageManager.GetCulture());
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
