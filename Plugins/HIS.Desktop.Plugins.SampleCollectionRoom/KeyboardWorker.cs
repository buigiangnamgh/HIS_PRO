using Inventec.Desktop.Core;
using Inventec.Desktop.Core.Actions;
using Inventec.Desktop.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.SampleCollectionRoom
{
    [KeyboardAction("FocusF1", "HIS.Desktop.Plugins.SampleCollectionRoom.SampleCollectionRoomUC", "FocusF1", KeyStroke = XKeys.F1)]
    [KeyboardAction("FocusF2", "HIS.Desktop.Plugins.SampleCollectionRoom.SampleCollectionRoomUC", "FocusF2", KeyStroke = XKeys.F2)]
    [KeyboardAction("CallFocusPatient", "HIS.Desktop.Plugins.SampleCollectionRoom.SampleCollectionRoomUC", "CallFocusPatient", KeyStroke = XKeys.F3)]
    [KeyboardAction("ShortCutSaveAndPrint", "HIS.Desktop.Plugins.SampleCollectionRoom.SampleCollectionRoomUC", "ShortCutSaveAndPrint", KeyStroke = XKeys.F4)]
    [KeyboardAction("PrintBarcode", "HIS.Desktop.Plugins.SampleCollectionRoom.SampleCollectionRoomUC", "PrintBarcode", KeyStroke = XKeys.Control | XKeys.P)]
    [KeyboardAction("ShotcurtCall", "HIS.Desktop.Plugins.SampleCollectionRoom.SampleCollectionRoomUC", "ShotcurtCall", KeyStroke = XKeys.F6)]
    [KeyboardAction("ShotcurtPrint", "HIS.Desktop.Plugins.SampleCollectionRoom.SampleCollectionRoomUC", "ShotcurtPrint", KeyStroke = XKeys.F7)]
    [KeyboardAction("ShortcutSave", "HIS.Desktop.Plugins.SampleCollectionRoom.SampleCollectionRoomUC", "ShortcutSave", KeyStroke = XKeys.F5)]
    [ExtensionOf(typeof(DesktopToolExtensionPoint))]
    public sealed class KeyboardWorker : Tool<IDesktopToolContext>
    {
        public KeyboardWorker() : base() { }

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
