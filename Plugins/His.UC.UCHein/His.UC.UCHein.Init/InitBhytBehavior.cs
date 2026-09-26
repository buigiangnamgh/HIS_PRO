using System;
using System.Threading.Tasks;
using His.UC.UCHein.Data;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Init
{
	internal class InitBhytBehavior : BeanObjectBase, IInitAsync
	{
		private DataInitHeinBhyt entity;

		internal InitBhytBehavior(CommonParam param, DataInitHeinBhyt data)
			: base(param)
		{
			entity = data;
		}

		async Task<object> IInitAsync.Run()
		{
			object result = null;
			try
			{
				if (entity.Template == MainHisHeinBhyt.TEMPLATE__BHYT1)
				{
					result = new Template__HeinBHYT1(entity);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				base.param.HasException = true;
			}
			return result;
		}
	}
}
