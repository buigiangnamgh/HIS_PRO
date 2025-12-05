using System;
using System.Resources;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.DebateDiagnostic.Resources
{
	internal class ResourceLanguageManager
	{
		public static ResourceManager LanguageFormDebateDiagnostic { get; set; }

		internal static void InitResourceLanguageManager()
		{
			try
			{
				LanguageFormDebateDiagnostic = new ResourceManager("HIS.Desktop.Plugins.DebateDiagnostic.Resources.Lang", typeof(FormDebateDiagnostic).Assembly);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
