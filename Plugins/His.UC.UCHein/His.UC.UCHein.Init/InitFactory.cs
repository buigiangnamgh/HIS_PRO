using System;
using His.UC.UCHein.Data;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Init
{
	internal class InitFactory
	{
		internal static IInitAsync MakeIInit(CommonParam param, object data)
		{
			IInitAsync initAsync = null;
			try
			{
				if (data.GetType() == typeof(DataInitHeinBhyt) && ((DataInitHeinBhyt)data).Template == MainHisHeinBhyt.TEMPLATE__BHYT1)
				{
					initAsync = new InitBhytBehavior(param, (DataInitHeinBhyt)data);
				}
				if (initAsync == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				initAsync = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				initAsync = null;
			}
			return initAsync;
		}

		internal static IInit MakeIInitUC(CommonParam param, object data)
		{
			IInit init = null;
			try
			{
				if (data.GetType() == typeof(DataInitHeinBhyt) && ((DataInitHeinBhyt)data).Template == MainHisHeinBhyt.TEMPLATE__BHYT1)
				{
					init = new InitBhytUCBehavior(param, (DataInitHeinBhyt)data);
				}
				if (init == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				init = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				init = null;
			}
			return init;
		}
	}
}
