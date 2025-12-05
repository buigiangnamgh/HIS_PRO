using System;
using HIS.Desktop.Plugins.DebateDiagnostic.DebateDiagnostic;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Core;

namespace HIS.Desktop.Plugins.DebateDiagnostic
{
	[ExtensionOf(typeof(DesktopRootExtensionPoint), "HIS.Desktop.Plugins.DebateDiagnostic", "Biên bản hội chẩn", "Common", 62, "highlightfield_16x16.png", "A", 2L, true, true)]
	public class DebateDiagnosticProcessor : ModuleBase, IDesktopRoot
	{
		private CommonParam param;

		public DebateDiagnosticProcessor()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			param = new CommonParam();
		}

		public DebateDiagnosticProcessor(CommonParam paramBusiness)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			param = (CommonParam)((paramBusiness != null) ? ((object)paramBusiness) : ((object)new CommonParam()));
		}

		object IDesktopRoot.Run(object[] args)
		{
			object obj = null;
			try
			{
				IDebateDiagnostic debateDiagnostic = DebateDiagnosticFactory.MakeIDebateDiagnostic(param, args);
				return (debateDiagnostic != null) ? debateDiagnostic.Run() : null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return null;
			}
		}

		public override bool IsEnable()
		{
			return false;
		}
	}
}
