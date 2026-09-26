using System;
using System.Globalization;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Base
{
	public class LanguageManager
	{
		public CultureInfo Language { get; set; }

		private static CultureInfo cultureInfo { get; set; }

		public LanguageManager(CultureInfo Language)
		{
			this.Language = Language;
			Init(this.Language);
		}

		public static bool Init(CultureInfo current)
		{
			bool result = false;
			try
			{
				cultureInfo = current;
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Error(ex);
			}
			return result;
		}

		public static CultureInfo GetCulture()
		{
			CultureInfo cultureInfo = null;
			try
			{
				if (LanguageManager.cultureInfo == null)
				{
					LanguageManager.cultureInfo = new CultureInfo("vi");
				}
				cultureInfo = LanguageManager.cultureInfo;
			}
			catch (Exception ex)
			{
				cultureInfo = null;
				LogSystem.Error(ex);
			}
			return cultureInfo;
		}
	}
}
