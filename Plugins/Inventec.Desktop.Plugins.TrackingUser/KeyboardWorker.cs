using Inventec.Desktop.Core;
using Inventec.Desktop.Core.Actions;
using Inventec.Desktop.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Desktop.Plugins.TrackingUser
{
    [KeyboardAction("Search", "Inventec.Desktop.Plugins.TrackingUser.UCTrackingMonitor", "Search", KeyStroke = XKeys.Control | XKeys.F)]
    [KeyboardAction("Export", "Inventec.Desktop.Plugins.TrackingUser.UCTrackingMonitor", "Export", KeyStroke = XKeys.Control | XKeys.E)]

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
