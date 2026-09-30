using System;
using System.Resources;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.ServiceReqList.Base
{
	internal class ResourceLangManager
	{
		internal static ResourceManager LanguageFrmServiceReqList { get; set; }

		internal static void InitResourceLanguageManager()
		{
			try
			{
				LanguageFrmServiceReqList = new ResourceManager("HIS.Desktop.Plugins.ServiceReqList.Resources.Lang", typeof(frmServiceReqList).Assembly);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
