using System;
using System.Resources;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using His.UC.UCHein.Resources;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Base
{
	public class ResouceManager
	{
		public static void ResourceLanguageManager()
		{
			try
			{
				His.UC.UCHein.Resources.ResourceLanguageManager.LanguageUCHeinBHYT = new ResourceManager("His.UC.UCHein.Resources.Lang", typeof(Template__HeinBHYT1).Assembly);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
