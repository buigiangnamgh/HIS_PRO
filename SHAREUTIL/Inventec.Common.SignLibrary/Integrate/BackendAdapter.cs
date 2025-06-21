using Inventec.Common.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Inventec.Common.Integrate
{
    public class BackendAdapter : EntityBaseAdapter
    {
        protected CommonParam param { get; set; }
        string errorFormat = "Call API \"{0}/{1}\":";
        static string STR_CANNOT_CONNECT_TO_SERVER = "Không kết nối được máy chủ";
        static string STR_SESSION_TIMEOUT = "Phiên làm việc đã hết hiệu lực";
        static string LanguageCode = "VI";

        public BackendAdapter()
            : base()
        {
            param = new CommonParam();
        }

        public BackendAdapter(CommonParam paramBusiness)
            : base()
        {
            param = (paramBusiness != null ? paramBusiness : new CommonParam());
        }

        #region Get
        public T Get<T>(string requestUri, ApiConsumer consumer, object filter, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Get<T>(requestUri, consumer, commonParam, filter, 0, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Get<T>(string requestUri, ApiConsumer consumer, object filter, int userTimeout, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Get<T>(requestUri, consumer, commonParam, filter, userTimeout, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Get<T>(string requestUri, ApiConsumer consumer, object filter, Action action, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Get<T>(requestUri, consumer, commonParam, filter, 0, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Get<T>(string requestUri, ApiConsumer consumer, object filter, int userTimeout, Action action, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Get<T>(requestUri, consumer, commonParam, filter, userTimeout, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Get<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object filter, Action action, params object[] listParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Get<T>(requestUri, consumer, commonParam, filter, 0, action, listParam);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Get<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object filter, int userTimeout, Action action, params object[] listParam)
        {
            T result = default(T);
            try
            {
                ApiResultObject<T> rs = null;
                if (commonParam != null)
                    commonParam.LanguageCode = LanguageCode;
                if (listParam != null && listParam.Length > 0)
                {
                    rs = consumer.Get<ApiResultObject<T>>(requestUri, commonParam, filter, userTimeout, listParam);
                }
                else
                {
                    rs = consumer.Get<ApiResultObject<T>>(requestUri, commonParam, filter, userTimeout);
                }
                if (rs != null)
                {
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                    result = (rs.Data);
                }

                if (rs == null || !rs.Success || result == null)
                {
                    Input = (Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => commonParam), commonParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listParam), listParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => userTimeout), userTimeout));
                    ErrorFormat = String.Format(errorFormat, consumer.GetBaseUri(), requestUri);
                    LogInOut(JsonConvert.SerializeObject(rs), LogType.Error);
                }
            }
            catch (ApiException ex)
            {
                LogSystem.Info(LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ex.StatusCode), ex.StatusCode));
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    param.Messages.Add(STR_SESSION_TIMEOUT);
                    param.HasException = true;
                    if (action != null)
                    {
                        action();
                    }
                }
            }
            catch (AggregateException ex)
            {
                LogSystem.Error(ex);
                param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        #endregion

        #region GetAsync
        public async Task<T> GetAsync<T>(string requestUri, ApiConsumer consumer, object filter, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await GetAsync<T>(requestUri, consumer, commonParam, filter, 0, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> GetAsync<T>(string requestUri, ApiConsumer consumer, object filter, int userTimeout, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await GetAsync<T>(requestUri, consumer, commonParam, filter, userTimeout, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> GetAsync<T>(string requestUri, ApiConsumer consumer, object filter, Action action, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await GetAsync<T>(requestUri, consumer, commonParam, filter, 0, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> GetAsync<T>(string requestUri, ApiConsumer consumer, object filter, int userTimeout, Action action, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await GetAsync<T>(requestUri, consumer, commonParam, filter, userTimeout, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> GetAsync<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object filter, Action action, params object[] listParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await GetAsync<T>(requestUri, consumer, commonParam, filter, 0, action, listParam);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> GetAsync<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object filter, int userTimeout, Action action, params object[] listParam)
        {
            T result = default(T);
            ApiResultObject<T> rs = null;
            try
            {
                if (commonParam != null)
                    commonParam.LanguageCode = LanguageCode;
                if (listParam != null && listParam.Length > 0)
                {
                    rs = await consumer.GetAsync<ApiResultObject<T>>(requestUri, commonParam, filter, userTimeout, listParam).ConfigureAwait(false);
                }
                else
                {
                    rs = await consumer.GetAsync<ApiResultObject<T>>(requestUri, commonParam, filter, userTimeout).ConfigureAwait(false);
                }

                if (rs != null)
                {
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                    result = (rs.Data);
                }

                if (rs == null || !rs.Success || result == null)
                {
                    Input = (Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => commonParam), commonParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listParam), listParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => userTimeout), userTimeout));
                    ErrorFormat = String.Format(errorFormat, consumer.GetBaseUri(), requestUri);
                    LogInOut(JsonConvert.SerializeObject(rs), LogType.Error);
                }
            }
            catch (ApiException ex)
            {
                LogSystem.Info(LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ex.StatusCode), ex.StatusCode));
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    param.Messages.Add(STR_SESSION_TIMEOUT);
                    param.HasException = true;
                    if (action != null)
                    {
                        action();
                    }
                }
            }
            catch (AggregateException ex)
            {
                LogSystem.Error(ex);
                param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<ApiResultObject<T>> GetAsyncRO<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object filter, int userTimeout, Action action, params object[] listParam)
        {
            ApiResultObject<T> rs = null;
            try
            {
                if (commonParam != null)
                    commonParam.LanguageCode = LanguageCode;
                if (listParam != null && listParam.Length > 0)
                {
                    rs = await consumer.GetAsync<ApiResultObject<T>>(requestUri, commonParam, filter, userTimeout, listParam).ConfigureAwait(false);
                }
                else
                {
                    rs = await consumer.GetAsync<ApiResultObject<T>>(requestUri, commonParam, filter, userTimeout).ConfigureAwait(false);
                }

                if (rs != null)
                {
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                }

                if (rs == null || !rs.Success)
                {
                    Input = (Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => commonParam), commonParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listParam), listParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => userTimeout), userTimeout));
                    ErrorFormat = String.Format(errorFormat, consumer.GetBaseUri(), requestUri);
                    LogInOut(JsonConvert.SerializeObject(rs), LogType.Error);
                }
            }
            catch (ApiException ex)
            {
                LogSystem.Info(LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ex.StatusCode), ex.StatusCode));
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    param.Messages.Add(STR_SESSION_TIMEOUT);
                    param.HasException = true;
                    if (action != null)
                    {
                        action();
                    }
                }
            }
            catch (AggregateException ex)
            {
                LogSystem.Error(ex);
                param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return rs;
        }
        #endregion
        
        #region GetRO
        public ApiResultObject<T> GetRO<T>(string requestUri, ApiConsumer consumer, object filter, CommonParam commonParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = GetRO<T>(requestUri, consumer, commonParam, filter, 0, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> GetRO<T>(string requestUri, ApiConsumer consumer, object filter, int userTimeout, CommonParam commonParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = GetRO<T>(requestUri, consumer, commonParam, filter, userTimeout, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> GetRO<T>(string requestUri, ApiConsumer consumer, object filter, Action action, CommonParam commonParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = GetRO<T>(requestUri, consumer, commonParam, filter, 0, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> GetRO<T>(string requestUri, ApiConsumer consumer, object filter, int userTimeout, Action action, CommonParam commonParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = GetRO<T>(requestUri, consumer, commonParam, filter, userTimeout, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> GetRO<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object filter, Action action, params object[] listParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = GetRO<T>(requestUri, consumer, commonParam, filter, 0, action, listParam);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> GetRO<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object filter, int userTimeout, Action action, params object[] listParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                if (commonParam != null)
                    commonParam.LanguageCode = LanguageCode;
                if (listParam != null && listParam.Length > 0)
                {
                    result = consumer.Get<ApiResultObject<T>>(requestUri, commonParam, filter, userTimeout, listParam);
                }
                else
                {
                    result = consumer.Get<ApiResultObject<T>>(requestUri, commonParam, filter, userTimeout);
                }

                if (result != null)
                {
                    if (result.Param != null)
                    {
                        param.Messages.AddRange(result.Param.Messages);
                        param.BugCodes.AddRange(result.Param.BugCodes);
                    }
                }

                if (result == null || !result.Success || result.Data == null)
                {
                    Input = (Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => commonParam), commonParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listParam), listParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => userTimeout), userTimeout));
                    ErrorFormat = String.Format(errorFormat, consumer.GetBaseUri(), requestUri);
                    LogInOut(JsonConvert.SerializeObject(result), LogType.Error);
                }
            }
            catch (ApiException ex)
            {
                LogSystem.Info(LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ex.StatusCode), ex.StatusCode));
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    param.Messages.Add(STR_SESSION_TIMEOUT);
                    param.HasException = true;
                    if (action != null)
                    {
                        action();
                    }
                }
            }
            catch (AggregateException ex)
            {
                LogSystem.Error(ex);
                param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        #endregion

        #region Post
        public T Post<T>(string requestUri, ApiConsumer consumer, object data, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Post<T>(requestUri, consumer, commonParam, data, 0, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Post<T>(string requestUri, ApiConsumer consumer, object data, int userTimeout, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Post<T>(requestUri, consumer, commonParam, data, userTimeout, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Post<T>(string requestUri, ApiConsumer consumer, object data, Action action, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Post<T>(requestUri, consumer, commonParam, data, 0, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Post<T>(string requestUri, ApiConsumer consumer, object data, int userTimeout, Action action, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Post<T>(requestUri, consumer, commonParam, data, userTimeout, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Post<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object data, Action action, params object[] listParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = Post<T>(requestUri, consumer, commonParam, data, 0, action, listParam);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public T Post<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object data, int userTimeout, Action action, params object[] listParam)
        {
            T result = default(T);
            try
            {
                ApiResultObject<T> rs = null;
                if (commonParam != null)
                    commonParam.LanguageCode = LanguageCode;
                if (listParam != null && listParam.Length > 0)
                {
                    rs = consumer.Post<ApiResultObject<T>>(requestUri, commonParam, data, userTimeout, listParam);
                }
                else
                {
                    rs = consumer.Post<ApiResultObject<T>>(requestUri, commonParam, data, userTimeout);
                }

                if (rs != null)
                {
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                    result = (rs.Data);
                }

                if (rs == null || !rs.Success || result == null)
                {
                    Input = (Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => commonParam), commonParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listParam), listParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => userTimeout), userTimeout));
                    ErrorFormat = String.Format(errorFormat, consumer.GetBaseUri(), requestUri);
                    LogInOut(JsonConvert.SerializeObject(rs), LogType.Error);
                }
            }
            catch (ApiException ex)
            {
                LogSystem.Info(LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ex.StatusCode), ex.StatusCode));
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    param.Messages.Add(STR_SESSION_TIMEOUT);
                    param.HasException = true;
                    if (action != null)
                    {
                        action();
                    }
                }
            }
            catch (AggregateException ex)
            {
                LogSystem.Error(ex);
                param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        #endregion

        #region PostRO
        public ApiResultObject<T> PostRO<T>(string requestUri, ApiConsumer consumer, object data, CommonParam commonParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = PostRO<T>(requestUri, consumer, commonParam, data, 0, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> PostRO<T>(string requestUri, ApiConsumer consumer, object data, int userTimeout, CommonParam commonParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = PostRO<T>(requestUri, consumer, commonParam, data, userTimeout, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> PostRO<T>(string requestUri, ApiConsumer consumer, object data, Action action, CommonParam commonParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = PostRO<T>(requestUri, consumer, commonParam, data, 0, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> PostRO<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object data, Action action, params object[] listParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = PostRO<T>(requestUri, consumer, commonParam, data, 0, action, listParam);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> PostRO<T>(string requestUri, ApiConsumer consumer, object data, int userTimeout, Action action, CommonParam commonParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                FrameIndex = 1;
                result = PostRO<T>(requestUri, consumer, commonParam, data, userTimeout, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public ApiResultObject<T> PostRO<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object data, int userTimeout, Action action, params object[] listParam)
        {
            ApiResultObject<T> result = null;
            try
            {
                if (commonParam != null)
                    commonParam.LanguageCode = LanguageCode;
                if (listParam != null && listParam.Length > 0)
                {
                    result = consumer.Post<ApiResultObject<T>>(requestUri, commonParam, data, userTimeout, listParam);
                }
                else
                {
                    result = consumer.Post<ApiResultObject<T>>(requestUri, commonParam, data, userTimeout);
                }

                if (result != null)
                {
                    if (result.Param != null)
                    {
                        param.Messages.AddRange(result.Param.Messages);
                        param.BugCodes.AddRange(result.Param.BugCodes);
                    }
                }

                if (result == null || !result.Success || result.Data == null)
                {
                    Input = (Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => commonParam), commonParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listParam), listParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => userTimeout), userTimeout));
                    ErrorFormat = String.Format(errorFormat, consumer.GetBaseUri(), requestUri);
                    LogInOut(JsonConvert.SerializeObject(result), LogType.Error);
                }
            }
            catch (ApiException ex)
            {
                LogSystem.Info(LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ex.StatusCode), ex.StatusCode));
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    param.Messages.Add(STR_SESSION_TIMEOUT);
                    param.HasException = true;
                    if (action != null)
                    {
                        action();
                    }
                }
            }
            catch (AggregateException ex)
            {
                LogSystem.Error(ex);
                param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        #endregion

        #region PostAsync
        public async Task<T> PostAsync<T>(string requestUri, ApiConsumer consumer, object data, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await PostAsync<T>(requestUri, consumer, commonParam, data, 0, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> PostAsync<T>(string requestUri, ApiConsumer consumer, object data, int userTimeout, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await PostAsync<T>(requestUri, consumer, commonParam, data, userTimeout, null, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> PostAsync<T>(string requestUri, ApiConsumer consumer, object data, Action action, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await PostAsync<T>(requestUri, consumer, commonParam, data, 0, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> PostAsync<T>(string requestUri, ApiConsumer consumer, object data, int userTimeout, Action action, CommonParam commonParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await PostAsync<T>(requestUri, consumer, commonParam, data, userTimeout, action, null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> PostAsync<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object data, Action action, params object[] listParam)
        {
            T result = default(T);
            try
            {
                FrameIndex = 1;
                result = await PostAsync<T>(requestUri, consumer, commonParam, data, 0, action, listParam);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public async Task<T> PostAsync<T>(string requestUri, ApiConsumer consumer, CommonParam commonParam, object data, int userTimeout, Action action, params object[] listParam)
        {
            T result = default(T);
            try
            {
                ApiResultObject<T> rs = null;
                if (commonParam != null)
                    commonParam.LanguageCode = LanguageCode;
                if (listParam != null && listParam.Length > 0)
                {
                    rs = await consumer.PostAsync<ApiResultObject<T>>(requestUri, commonParam, data, userTimeout, listParam);
                }
                else
                {
                    rs = await consumer.PostAsync<ApiResultObject<T>>(requestUri, commonParam, data, userTimeout);
                }

                if (rs != null)
                {
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                    result = (rs.Data);
                }

                if (rs == null || !rs.Success || result == null)
                {
                    Input = (Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => commonParam), commonParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listParam), listParam) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => userTimeout), userTimeout));
                    ErrorFormat = String.Format(errorFormat, consumer.GetBaseUri(), requestUri);
                    LogInOut(JsonConvert.SerializeObject(result), LogType.Error);
                }
            }
            catch (ApiException ex)
            {
                LogSystem.Info(LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ex.StatusCode), ex.StatusCode));
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    param.Messages.Add(STR_SESSION_TIMEOUT);
                    param.HasException = true;
                    if (action != null)
                    {
                        action();
                    }
                }
            }
            catch (AggregateException ex)
            {
                LogSystem.Error(ex);
                param.Messages.Add(STR_CANNOT_CONNECT_TO_SERVER);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        #endregion

    }
}
