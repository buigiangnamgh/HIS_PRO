using System;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein
{
	public abstract class BusinessBase : EntityBase
	{
		protected CommonParam param { get; set; }

		public BusinessBase()
		{
			param = new CommonParam();
		}

		public BusinessBase(CommonParam paramBusiness)
		{
			param = ((paramBusiness != null) ? paramBusiness : new CommonParam());
		}

		public bool HasException()
		{
			return param.HasException;
		}

		public void CopyCommonParamInfoGet(CommonParam paramSource)
		{
			try
			{
				param.Start = paramSource.Start;
				param.Limit = paramSource.Limit;
				param.Count = paramSource.Count;
				param.HasException = paramSource.HasException;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void CopyCommonParamInfo(CommonParam paramSource)
		{
			try
			{
				if (paramSource.BugCodes != null && paramSource.BugCodes.Count > 0)
				{
					param.BugCodes.AddRange(paramSource.BugCodes);
				}
				if (paramSource.Messages != null && paramSource.Messages.Count > 0)
				{
					param.Messages.AddRange(paramSource.Messages);
				}
				param.Start = paramSource.Start;
				param.Limit = paramSource.Limit;
				param.Count = paramSource.Count;
				param.HasException = paramSource.HasException;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void CopyCommonParamInfo(BusinessBase fromObject)
		{
			try
			{
				if (fromObject.param != null)
				{
					if (fromObject.param.BugCodes != null && fromObject.param.BugCodes.Count > 0)
					{
						param.BugCodes.AddRange(fromObject.param.BugCodes);
					}
					if (fromObject.param.Messages != null && fromObject.param.Messages.Count > 0)
					{
						param.Messages.AddRange(fromObject.param.Messages);
					}
					param.Start = fromObject.param.Start;
					param.Limit = fromObject.param.Limit;
					param.Count = fromObject.param.Count;
					param.HasException = fromObject.param.HasException;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		protected ApiResultObject<T> PackResult<T>(T resultData)
		{
			ApiResultObject<T> apiResultObject = new ApiResultObject<T>();
			try
			{
				apiResultObject.SetValue(resultData, Util.DecisionApiResult(resultData), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				LogSystem.Error(LogUtil.TraceData(LogUtil.GetMemberName(() => resultData), resultData));
				apiResultObject = new ApiResultObject<T>(default(T), false);
			}
			return apiResultObject;
		}
	}
}
