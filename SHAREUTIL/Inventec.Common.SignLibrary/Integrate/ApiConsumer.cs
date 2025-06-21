using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Inventec.Common.Integrate
{
    /// <summary>
    /// Class de goi cac API do server back-end cung cap
    /// </summary>
    public class ApiConsumer
    {
        private const string API_PARAM = "param";
        private int TIME_OUT = int.Parse(ConfigurationManager.AppSettings["Inventec.Common.WebApiClient.Timeout"] ?? "60");//thoi gian timeout tinh theo seconds (60s)
        private string baseUri = null;
        private string token = null;
        private string applicationCode = null;

        /// <summary>
        /// Khoi tao voi tham so truyen vao la base uri cua api do server back-end cung cap
        /// </summary>
        /// <param name="baseUri">BaseUri cua server back-end</param>
        public ApiConsumer(string baseUri, string applicationCode)
        {
            this.Init(baseUri, null, applicationCode);
        }

        /// <summary>
        /// Khoi tao voi tham so truyen vao la base uri cua api do server back-end cung cap va tokenCode cua front-end
        /// </summary>
        /// <param name="baseUri">BaseUri cua server back-end</param>
        public ApiConsumer(string baseUri, string tokenCode, string applicationCode)
        {
            this.Init(baseUri, tokenCode, applicationCode);
        }

        public string GetBaseUri()
        {
            return this.baseUri;
        }

        private void Init(string baseUri, string tokenCode, string applicationCode)
        {
            this.baseUri = baseUri;
            this.token = tokenCode;
            this.applicationCode = applicationCode;
        }

        /// <summary>
        /// Cap nhat tokenCode
        /// </summary>
        /// <param name="tokenCode"></param>
        public void SetTokenCode(string tokenCode)
        {
            this.token = tokenCode;
        }

        /// <summary>
        /// Cap nhat tokenCode
        /// </summary>
        /// <param name="tokenCode"></param>
        public void SetBaseUri(string baseUri)
        {
            this.baseUri = baseUri;
        }

        /// <summary>
        /// Get du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="filter">Du lieu Filter</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public T Get<T>(string uri, object commonParam, object filter, params object[] listParam)
        {
            return Get<T>(uri, commonParam, filter, 0, listParam);
        }

        /// <summary>
        /// Get du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="filter">Du lieu Filter</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public T Get<T>(string uri, object commonParam, object filter, int userTimeout, params object[] listParam)
        {
            T result = default(T);

            using (var client = new HttpClient())
            {
                string requestedUrl = "";
                this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);
                if (filter != null || commonParam != null)
                {
                    ApiParam apiParam = new ApiParam();
                    apiParam.CommonParam = commonParam;
                    apiParam.ApiData = filter;
                    string param = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvertUtil.SerializeObject(apiParam)));


                    requestedUrl += string.Format("{0}={1}", API_PARAM, param);
                }         
                HttpResponseMessage resp = client.GetAsync(requestedUrl).Result;
                if (!resp.IsSuccessStatusCode)
                {
                    Inventec.Common.Logging.LogSystem.Warn(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}. Input: {3}{4}.", client.BaseAddress.AbsoluteUri, uri, resp.StatusCode.GetHashCode(), JsonConvertUtil.SerializeObject(filter), JsonConvertUtil.SerializeObject(commonParam)));                  
                }

                string responseData = resp.Content.ReadAsStringAsync().Result;
                var rs = JsonConvertUtil.DeserializeObject<ApiResultObject<T>>(responseData);
                if (rs != null)
                {
                    result = rs.Data;
                    commonParam = rs.Param;
                }
            }
            return result;
        }

        public T GetRO<T>(string uri, object commonParam, object filter, int userTimeout, params object[] listParam)
        {
            T result = default(T);

            using (var client = new HttpClient())
            {
                string requestedUrl = "";
                this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);
                if (filter != null || commonParam != null)
                {
                    ApiParam apiParam = new ApiParam();
                    apiParam.CommonParam = commonParam;
                    apiParam.ApiData = filter;
                    string param = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvertUtil.SerializeObject(apiParam)));
                    requestedUrl += string.Format("{0}={1}", API_PARAM, param);
                }               
                HttpResponseMessage resp = client.GetAsync(requestedUrl).Result;             
                if (!resp.IsSuccessStatusCode)
                {
                    Inventec.Common.Logging.LogSystem.Warn(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}. Input: {3}{4}.", client.BaseAddress.AbsoluteUri, uri, resp.StatusCode.GetHashCode(), JsonConvertUtil.SerializeObject(filter), JsonConvertUtil.SerializeObject(commonParam)));                   
                }

                string responseData = resp.Content.ReadAsStringAsync().Result;
                result = JsonConvertUtil.DeserializeObject<T>(responseData);               
            }
            return result;
        }
        /// <summary>
        /// Get du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="filter">Du lieu Filter</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public async Task<T> GetAsync<T>(string uri, object commonParam, object filter, params object[] listParam)
        {
            return await GetAsync<T>(uri, commonParam, filter, 0);
        }

        /// <summary>
        /// Get du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="filter">Du lieu Filter</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public async Task<T> GetAsync<T>(string uri, object commonParam, object filter, int userTimeout, params object[] listParam)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                string requestedUrl = "";
                this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);
                if (filter != null || commonParam != null)
                {
                    ApiParam apiParam = new ApiParam();
                    apiParam.CommonParam = commonParam;
                    apiParam.ApiData = filter;
                    string param = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvertUtil.SerializeObject(apiParam)));
                    requestedUrl += string.Format("{0}={1}", API_PARAM, param);
                }
                HttpResponseMessage resp = await client.GetAsync(requestedUrl).ConfigureAwait(false);

                if (!resp.IsSuccessStatusCode)
                {
                    Inventec.Common.Logging.LogSystem.Warn(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}. Input: {3}{4}.", client.BaseAddress.AbsoluteUri, uri, resp.StatusCode.GetHashCode(), JsonConvertUtil.SerializeObject(filter), JsonConvertUtil.SerializeObject(commonParam)));
                    //throw new ApiException(resp.StatusCode);
                }

                string responseData = await resp.Content.ReadAsStringAsync();
                result = JsonConvertUtil.DeserializeObject<T>(responseData);
                return result;
            }
        }

        ///// <summary>
        ///// Get du lieu
        ///// </summary>
        ///// <param name="uri">Uri cua API can goi</param>
        ///// <param name="commonParam">Doi tuong commonParam</param>
        ///// <param name="filter">Du lieu Filter</param>
        ///// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        ///// <returns></returns>
        ///// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        //public FileHolder GetFile(string uri, object commonParam, object filter, params object[] listParam)
        //{
        //    return GetFile(uri, commonParam, filter, 0, listParam);
        //}

        ///// <summary>
        ///// Get du lieu
        ///// </summary>
        ///// <param name="uri">Uri cua API can goi</param>
        ///// <param name="commonParam">Doi tuong commonParam</param>
        ///// <param name="filter">Du lieu Filter</param>
        ///// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        ///// <returns></returns>
        ///// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        //public FileHolder GetFile(string uri, object commonParam, object filter, int userTimeout, params object[] listParam)
        //{
        //    FileHolder result = null;
        //    using (var client = new HttpClient())
        //    {
        //        string requestedUrl = "";
        //        this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);
        //        if (filter != null || commonParam != null)
        //        {
        //            ApiParam apiParam = new ApiParam();
        //            apiParam.CommonParam = commonParam;
        //            apiParam.ApiData = filter;
        //            requestedUrl += string.Format("{0}={1}&", API_PARAM, Uri.EscapeDataString(JsonConvertUtil.SerializeObject(apiParam)));
        //        }
        //        HttpResponseMessage resp = client.GetAsync(requestedUrl).Result;
        //        if (!resp.IsSuccessStatusCode)
        //        {
        //            LogSystem.Error(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", this.baseUri, uri, resp.StatusCode.GetHashCode()));
        //            //throw new ApiException(resp.StatusCode);
        //        }
        //        if (resp.Content != null && resp.Content.Headers != null && resp.Content.Headers.ContentDisposition != null)
        //        {
        //            result = new FileHolder();
        //            result.FileName = resp.Content.Headers.ContentDisposition.FileName;
        //            result.Content = new MemoryStream(resp.Content.ReadAsByteArrayAsync().Result);
        //        }
        //    }
        //    return result;
        //}

        /// <summary>
        /// Post du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="data">Du lieu can post</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public T Post<T>(string uri, CommonParam commonParam, object data, params object[] listParam)
        {
            return Post<T>(uri, commonParam, data, 0, listParam);
        }

        /// <summary>
        /// Post du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="data">Du lieu can post</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public T Post<T>(string uri, CommonParam commonParam, object data, int userTimeout, params object[] listParam)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                Inventec.Common.Logging.LogSystem.Debug("Post.1");
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => uri), uri) 
                    + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data));
                string requestedUrl = "";
                this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);
                ApiParam apiParam = new ApiParam();
                if (data != null || commonParam != null)
                {
                    apiParam.CommonParam = commonParam;
                    apiParam.ApiData = data;
                }
                Inventec.Common.Logging.LogSystem.Debug("Post.2");
                HttpResponseMessage resp = client.PostAsJsonAsync(requestedUrl, apiParam).Result;
                if (!resp.IsSuccessStatusCode)
                {
                    Inventec.Common.Logging.LogSystem.Warn(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}. Input: {3}.", client.BaseAddress.AbsoluteUri, uri, resp.StatusCode.GetHashCode(), JsonConvertUtil.SerializeObject(data)));
                    //throw new ApiException(resp.StatusCode);
                }
                Inventec.Common.Logging.LogSystem.Debug("Post.3");
                string responseData = resp.Content.ReadAsStringAsync().Result;
                try
                {
                    var rs = JsonConvertUtil.DeserializeObject<ApiResultObject<T>>(responseData);
                    if (rs != null)
                    {
                        result = rs.Data;
                        commonParam = rs.Param;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                Inventec.Common.Logging.LogSystem.Debug("Post.4");
            }
            return result;
        }

        public T PostRO<T>(string uri, CommonParam commonParam, object data, params object[] listParam)
        {
            return PostRO<T>(uri, commonParam, data, 0);
        }

        public T PostRO<T>(string uri, CommonParam commonParam, object data, int userTimeout, params object[] listParam)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                string requestedUrl = "";
                this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);
                ApiParam apiParam = new ApiParam();
                if (data != null || commonParam != null)
                {
                    apiParam.CommonParam = commonParam;
                    apiParam.ApiData = data;
                }
                HttpResponseMessage resp = client.PostAsJsonAsync(requestedUrl, apiParam).Result;
                if (!resp.IsSuccessStatusCode)
                {
                    Inventec.Common.Logging.LogSystem.Warn(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}. Input: {3}.", client.BaseAddress.AbsoluteUri, uri, resp.StatusCode.GetHashCode(), JsonConvertUtil.SerializeObject(data)));
                    //throw new ApiException(resp.StatusCode);
                }
                string responseData = resp.Content.ReadAsStringAsync().Result;
                try
                {
                    result = JsonConvertUtil.DeserializeObject<T>(responseData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// Post du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="data">Du lieu can post</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public T PostWithouApiParam<T>(string uri, object data, int userTimeout, params object[] listParam)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                string requestedUrl = "";
                this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);

                HttpResponseMessage resp = client.PostAsJsonAsync(requestedUrl, data).Result;
                if (!resp.IsSuccessStatusCode)
                {
                    Inventec.Common.Logging.LogSystem.Warn(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", this.baseUri, uri, resp.StatusCode.GetHashCode()));
                    //throw new ApiException(resp.StatusCode);
                }
                string responseData = resp.Content.ReadAsStringAsync().Result;
                Console.WriteLine(string.Format("responseData: {0}", responseData));
                result = JsonConvertUtil.DeserializeObject<T>(responseData);
            }
            return result;
        }

        /// <summary>
        /// Post du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="data">Du lieu can post</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public async Task<T> PostWithouApiParamAsync<T>(string uri, object data, int userTimeout, params object[] listParam)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                string requestedUrl = "";
                this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);

                HttpResponseMessage resp = await client.PostAsJsonAsync(requestedUrl, data).ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode)
                {
                    Inventec.Common.Logging.LogSystem.Warn(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", this.baseUri, uri, resp.StatusCode.GetHashCode()));
                    //throw new ApiException(resp.StatusCode);
                }
                string responseData = await resp.Content.ReadAsStringAsync();

                result = JsonConvertUtil.DeserializeObject<T>(responseData);
            }
            return result;
        }

        /// <summary>
        /// Post du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="data">Du lieu can post</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public async Task<T> PostAsync<T>(string uri, object commonParam, object data, params object[] listParam)
        {
            return await PostAsync<T>(uri, commonParam, data, 0);
        }

        /// <summary>
        /// Post du lieu
        /// </summary>
        /// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        /// <param name="uri">Uri cua API can goi</param>
        /// <param name="commonParam">Doi tuong commonParam</param>
        /// <param name="data">Du lieu can post</param>
        /// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        /// <returns></returns>
        /// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public async Task<T> PostAsync<T>(string uri, object commonParam, object data, int userTimeout, params object[] listParam)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                string requestedUrl = "";
                this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);

                ApiParam apiParam = new ApiParam();
                if (data != null || commonParam != null)
                {
                    apiParam.CommonParam = commonParam;
                    apiParam.ApiData = data;
                }

                HttpResponseMessage resp = await client.PostAsJsonAsync(requestedUrl, apiParam).ConfigureAwait(false);
                if (!resp.IsSuccessStatusCode)
                {
                    Inventec.Common.Logging.LogSystem.Warn(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}. Input: {3}.", client.BaseAddress.AbsoluteUri, uri, resp.StatusCode.GetHashCode(), JsonConvertUtil.SerializeObject(data)));
                    //throw new ApiException(resp.StatusCode);
                }
                string responseData = await resp.Content.ReadAsStringAsync();
                result = JsonConvertUtil.DeserializeObject<T>(responseData);
            }
            return result;
        }

        ///// <summary>
        ///// Post du lieu co kem file
        ///// </summary>
        ///// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        ///// <param name="uri">Uri cua API can goi</param>
        ///// <param name="commonParam">Doi tuong commonParam</param>
        ///// <param name="data">Du lieu can post</param>
        ///// <param name="files">Danh sach file can post</param>
        ///// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        ///// <returns></returns>
        ///// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public T PostWithFile<T>(string uri, object commonParam, object data, List<Inventec.Common.SignLibrary.DTO.FileHolder> files, params object[] listParam)
        {
            return PostWithFile<T>(uri, commonParam, data, files, 0, listParam);
        }

        ///// <summary>
        ///// Post du lieu co kem file
        ///// </summary>
        ///// <typeparam name="T">Kieu du lieu mong muon tra ve</typeparam>
        ///// <param name="uri">Uri cua API can goi</param>
        ///// <param name="commonParam">Doi tuong commonParam</param>
        ///// <param name="data">Du lieu can post</param>
        ///// <param name="files">Danh sach file can post</param>
        ///// <param name="listParam">Danh sach cac tham so bo sung. Kieu du lieu phai la primitive</param>
        ///// <returns></returns>
        ///// <exceptions>Exception, ArgumentException, ApiException</exceptions>
        public T PostWithFile<T>(string uri, object commonParam, object data, List<Inventec.Common.SignLibrary.DTO.FileHolder> files, int userTimeout, params object[] listParam)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                string requestedUrl = "";
                this.HttpRequestBuilder(client, uri, ref requestedUrl, userTimeout, listParam);

                ApiParam apiParam = new ApiParam();
                if (data != null || commonParam != null)
                {
                    apiParam.CommonParam = commonParam;
                    apiParam.ApiData = data;
                }
                using (var content = new MultipartFormDataContent())
                {
                    foreach (Inventec.Common.SignLibrary.DTO.FileHolder file in files)
                    {
                        content.Add(new StreamContent(file.Content), "File", file.FileName);
                    }
                    HttpContent stringContent = new StringContent(JsonConvertUtil.SerializeObject(apiParam));
                    content.Add(stringContent, "Data", "Data");

                    using (HttpResponseMessage resp = client.PostAsync(requestedUrl, content).Result)
                    {
                        if (!resp.IsSuccessStatusCode)
                        {
                            Inventec.Common.Logging.LogSystem.Error(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", this.baseUri, uri, resp.StatusCode.GetHashCode()));
                            //throw new ApiException(resp.StatusCode);
                        }
                        string responseData = resp.Content.ReadAsStringAsync().Result;
                        result = JsonConvertUtil.DeserializeObject<T>(responseData);
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Tao HttpRequest voi cac tham so cho truoc
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="requestedUrl"></param>
        /// <param name="listParam"></param>
        private void HttpRequestBuilder(HttpClient client, string uri, ref string requestedUrl, int userTimeout, params object[] listParam)
        {
            client.DefaultRequestHeaders.Add(HeaderConstants.TOKEN_PARAM, this.token);
            client.DefaultRequestHeaders.Add(HeaderConstants.APPLICATION_CODE_PARAM, this.applicationCode);
            client.BaseAddress = new Uri(this.baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.Timeout = new TimeSpan(0, 0, (userTimeout > 0 ? userTimeout : TIME_OUT));

            requestedUrl = string.Format("{0}?", uri);
            if (listParam != null && listParam.Length > 0)
            {
                if (listParam.Length % 2 != 0)
                {
                    throw new ArgumentException("Danh sach param khong hop le. So luong param phai la so chan");
                }
                for (int i = 0; i < listParam.Length; )
                {
                    requestedUrl += string.Format("{0}={1}&", HttpUtility.UrlEncode(listParam[i] + ""), HttpUtility.UrlEncode(listParam[i + 1] + ""));
                    i = i + 2;
                }
            }
        }

    }
}
