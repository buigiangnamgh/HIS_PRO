using System;
using System.Linq;
using HIS.Desktop.ADO;
using HIS.Desktop.Common;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.Core;
using Inventec.Desktop.Core.Tools;

namespace HIS.Desktop.Plugins.CheckInfoBHYT.CheckInfoBHYT
{
	public sealed class CheckInfoBHYTBehavior : Tool<IDesktopToolContext>, ICheckInfoBHYT
	{
		private object[] entity;

		private Module currentModule;

		public CheckInfoBHYTBehavior()
		{
		}

		public CheckInfoBHYTBehavior(CommonParam param, object[] filter)
		{
			entity = filter;
		}

		object ICheckInfoBHYT.Run()
		{
			object result = null;
			try
			{
				if (entity != null && entity.Count() > 0)
				{
					long num = 0L;
					CheckInfoBhytADO checkInfoBhytADO = null;
					DelegateRefreshData delegateRefreshData = null;
					object[] array = entity;
					foreach (object obj in array)
					{
						if (obj is Module)
						{
							currentModule = (Module)obj;
						}
						else if (obj is long)
						{
							num = (long)obj;
						}
						else if (obj is CheckInfoBhytADO)
						{
							checkInfoBhytADO = (CheckInfoBhytADO)obj;
						}
						else if (obj is DelegateRefreshData)
						{
							delegateRefreshData = (DelegateRefreshData)obj;
						}
					}
					if (currentModule != null && num > 0)
					{
						result = ((delegateRefreshData == null) ? new frmCheckInfoBHYT(currentModule, num) : new frmCheckInfoBHYT(currentModule, num));
					}
					else if (currentModule != null && checkInfoBhytADO != null)
					{
						result = ((delegateRefreshData == null) ? new frmCheckInfoBHYT(currentModule, checkInfoBhytADO) : new frmCheckInfoBHYT(currentModule, checkInfoBhytADO));
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				result = null;
			}
			return result;
		}
	}
}
