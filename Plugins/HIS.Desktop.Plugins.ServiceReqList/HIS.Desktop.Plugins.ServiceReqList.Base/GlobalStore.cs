using System;
using System.Collections.Generic;
using HIS.Desktop.Plugins.ServiceReqList.Resources;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Desktop.Common.LanguageManager;

namespace HIS.Desktop.Plugins.ServiceReqList.Base
{
	internal class GlobalStore
	{
		internal const string HIS_SERVICE_REQ_GET = "/api/HisServiceReq/Get";

		internal const string HIS_SERE_SERV_GET = "api/HisSereServ/Get";

		internal const string HIS_SERE_SERV_GETVIEW = "api/HisSereServ/GetView";

		internal const string HIS_SERE_SERV_GETVIEW_12 = "api/HisSereServ/GetView12";

		internal const short IS_TRUE = 1;

		internal static List<string> TypeFilters = new List<string>();

		internal string TOI_TAO
		{
			get
			{
				return Get.Value("frmServiceReqList.ToiTao", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
			}
		}

		internal string PHONG_CHI_DINH
		{
			get
			{
				return Get.Value("frmServiceReqList.PhongChiDinh", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
			}
		}

		internal string HO_SO_DIEU_TRI
		{
			get
			{
				return Get.Value("frmServiceReqList.HoSoDieuTri", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
			}
		}

		internal string BENH_NHAN
		{
			get
			{
				return Get.Value("frmServiceReqList.BenhNhan", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
			}
		}

		internal string TAT_CA
		{
			get
			{
				return Get.Value("frmServiceReqList.TatCa", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
			}
		}

		internal string KHOA_CHI_DINH
		{
			get
			{
				return Get.Value("frmServiceReqList.KhoaChiDinh", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
			}
		}

		internal string KHOA_THUC_HIEN
		{
			get
			{
				return Get.Value("frmServiceReqList.KhoaThucHien", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture());
			}
		}

		internal void LoadTypeFilter()
		{
			try
			{
				TypeFilters = new List<string>();
				TypeFilters.Add(TOI_TAO);
				TypeFilters.Add(PHONG_CHI_DINH);
				TypeFilters.Add(KHOA_CHI_DINH);
				TypeFilters.Add(KHOA_THUC_HIEN);
				TypeFilters.Add(TAT_CA);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
