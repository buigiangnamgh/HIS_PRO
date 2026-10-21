using System.Collections.Generic;
using HIS.Desktop.ModuleExt;

namespace HIS.Desktop.Plugins.MedicalStoreV2
{
	internal class CallModule
	{
		internal const string EmrDocument = "HIS.Desktop.Plugins.EmrDocument";

		public CallModule(string _moduleLink, long _roomId, long _roomTypeId, List<object> _listObj)
		{
			CallModuleProcess(_moduleLink, _roomId, _roomTypeId, _listObj);
		}

		private void CallModuleProcess(string _moduleLink, long _roomId, long _roomTypeId, List<object> _listObj)
		{
			PluginInstanceBehavior.ShowModule(_moduleLink, _roomId, _roomTypeId, _listObj);
		}
	}
}
