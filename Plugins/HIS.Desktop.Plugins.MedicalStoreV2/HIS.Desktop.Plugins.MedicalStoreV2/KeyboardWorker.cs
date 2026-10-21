using Inventec.Desktop.Core;
using Inventec.Desktop.Core.Actions;
using Inventec.Desktop.Core.Tools;

namespace HIS.Desktop.Plugins.MedicalStoreV2
{
	[KeyboardAction("bbtnSEARCH", "HIS.Desktop.Plugins.MedicalStoreV2.UCMedicalStore", "bbtnSEARCH", KeyStroke = (XKeys.F | XKeys.Control))]
	[KeyboardAction("bbtnSAVE", "HIS.Desktop.Plugins.MedicalStoreV2.UCMedicalStore", "bbtnSAVE", KeyStroke = (XKeys.S | XKeys.Control))]
	[KeyboardAction("keyF2Focused", "HIS.Desktop.Plugins.MedicalStoreV2.UCMedicalStore", "keyF2Focused", KeyStroke = XKeys.F2)]
	[KeyboardAction("bbtnSEARCHMediRecord", "HIS.Desktop.Plugins.MedicalStoreV2.UCMedicalStore", "bbtnSEARCHMediRecord", KeyStroke = (XKeys.F | XKeys.Control | XKeys.Shift))]
	[KeyboardAction("keyF3Focused", "HIS.Desktop.Plugins.MedicalStoreV2.UCMedicalStore", "keyF3Focused", KeyStroke = XKeys.F3)]
	[KeyboardAction("bbtnImportStore", "HIS.Desktop.Plugins.MedicalStoreV2.UCMedicalStore", "bbtnImportStore", KeyStroke = (XKeys.I | XKeys.Control))]
	[ExtensionOf(typeof(DesktopToolExtensionPoint))]
	public sealed class KeyboardWorker : Tool<IDesktopToolContext>
	{
		public override IActionSet Actions
		{
			get
			{
				return base.Actions;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
		}
	}
}
