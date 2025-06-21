using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.Integrate
{
    public class ClientTokenManager
    {
        private TokenData token;
        private string applicationCode;
        private string loginName = "";
        private string baseUriAcs;
        private bool useRegistry = false;//default

        public ClientTokenManager(string applicationCode)
        {
            this.applicationCode = applicationCode;
            this.baseUriAcs = ConstanIG.ACS_BASE_URI;
        }

        public ClientTokenManager(string applicationCode, string baseUri)
        {
            this.applicationCode = applicationCode;
            this.baseUriAcs = baseUri;
        }

        public void UseRegistry(bool useRegistry)
        {
            this.useRegistry = useRegistry;
        }

        /// <summary>
        /// Khoi tao thong tin token (dua vao registry)
        /// - Neu token bi expire thi thuc hien renew
        /// - Neu token chua expire thi thuc hien lay thong tin tu "authen server"
        /// - Thuc hien cap nhat thong tin lai vao registry
        /// </summary>
        /// <param name="commonParam"></param>
        /// <returns></returns>
        public TokenData Init(CommonParam commonParam)
        {
            this.ReadFromRegistry();
            if (this.token != null)
            {
                if (this.token.ExpireTime != null && this.token.ExpireTime <= DateTime.Now)
                {
                    this.Renew(commonParam);
                }
                else
                {
                    this.token = this.GetAuthenticated(commonParam, this.token.TokenCode);
                }
                this.WriteToRegistry();
            }
            return this.token;
        }

        /// <summary>
        /// Dang nhap he thong de lay token. Neu thanh cong thi cap nhat gia tri token vao registry
        /// </summary>
        /// <param name="commonParam"></param>
        /// <param name="applicationCode">Ma ung dung</param>
        /// <param name="loginName">Ten dang nhap</param>
        /// <param name="password">Mat khau</param>
        /// <returns></returns>
        public TokenData Login(CommonParam commonParam, string loginName, string password)
        {
            TokenData result = null;
            try
            {
                string authInfo = string.Format("{0}:{1}:{2}", this.applicationCode, loginName, password);
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                result = this.CreateRequest<TokenData>(ConstanIG.LOGIN_URI, HttpMethod.Get, commonParam,
                    HeaderConstants.BASIC_AUTH_HEADER, string.Format("{0} {1}", HeaderConstants.BASIC_AUTH_SCHEME, authInfo));
                if (result != null)
                {
                    this.token = result;
                    this.WriteToRegistry();
                    this.loginName = loginName;
                }
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
            return result;
        }

        /// <summary>
        /// Dang nhap he thong de lay token. Neu thanh cong thi cap nhat gia tri token vao registry
        /// </summary>
        /// <param name="commonParam"></param>
        /// <param name="applicationCode">Ma ung dung</param>
        /// <param name="loginName">Ten dang nhap</param>
        /// <param name="password">Mat khau</param>
        /// <returns></returns>
        public TokenData Login(CommonParam commonParam, string loginName, string password, string versionApp)
        {
            TokenData result = null;
            try
            {
                string authInfo = string.Format("{0}:{1}:{2}:{3}:{4}", this.applicationCode, loginName, password, versionApp, Environment.MachineName);
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                result = this.CreateRequest<TokenData>(ConstanIG.LOGIN_URI, HttpMethod.Get, commonParam,
                    HeaderConstants.BASIC_AUTH_HEADER, string.Format("{0} {1}", HeaderConstants.BASIC_AUTH_SCHEME, authInfo));
                if (result != null)
                {
                    this.token = result;
                    this.WriteToRegistry();
                    this.loginName = loginName;
                }
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
            return result;
        }

        /// <summary>
        /// Thuc hien renew token dua vao token co san
        /// </summary>
        /// <param name="commonParam"></param>
        /// <returns></returns>
        public TokenData Renew(CommonParam commonParam)
        {
            TokenData result = null;
            try
            {
                result = this.CreateRequest<TokenData>(ConstanIG.RENEW_URI, HttpMethod.Get, commonParam,
                   HeaderConstants.RENEW_PARAM, this.token.RenewCode);
                if (result != null)
                {
                    this.token = result;
                    this.WriteToRegistry();
                }
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
            return result;
        }

        /// <summary>
        /// Lay thong tin token
        /// </summary>
        /// <returns></returns>
        public TokenData GetTokenData()
        {
            return this.token;
        }

        /// <summary>
        /// Lay thong tin nguoi dung
        /// </summary>
        /// <returns></returns>
        public UserData GetUserData()
        {
            TokenData tokenInfo = GetTokenData();
            return tokenInfo != null ? tokenInfo.User : null;
        }

        public string GetLoginName()
        {
            UserData user = GetUserData();
            return user != null ? user.LoginName : null;
        }

        public string GetUserName()
        {
            UserData user = GetUserData();
            return user != null ? user.UserName : null;
        }

        public string GetGroupCode()
        {
            UserData user = GetUserData();
            return user != null ? user.GCode : null;
        }

        public string GetLoginAddress()
        {
            TokenData tokenInfo = GetTokenData();
            return tokenInfo != null ? tokenInfo.LoginAddress : null;
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public void Logout(CommonParam commonParam)
        {
            try
            {
                this.ClearRegistry();
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
            try
            {
                this.CreateRequest<bool>(ConstanIG.LOGOUT_URI, HttpMethod.Post, commonParam, HeaderConstants.TOKEN_PARAM, this.token.TokenCode);
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
        }

        /// <summary>
        /// Goi den API do server back-end cung cap de doi mat khau
        /// </summary>
        /// <param name="oldPassword">Mat khau cu</param>
        /// <param name="newPassword">Mat khau moi</param>
        /// <returns></returns>
        public bool ChangePassword(CommonParam commonParam, string oldPassword, string newPassword)
        {
            bool result = false;
            try
            {
                if (this.token != null && this.token.ExpireTime >= DateTime.Now)
                {
                    string passInfo = string.Format("{0}:{1}", oldPassword, newPassword);
                    passInfo = Convert.ToBase64String(Encoding.Default.GetBytes(passInfo));

                    result = this.CreateRequest<bool>(ConstanIG.CHANGE_PASS_URI, HttpMethod.Post, commonParam,
                        HeaderConstants.TOKEN_PARAM, this.token.TokenCode,
                        HeaderConstants.PASS_PARAM, passInfo);
                }
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
            return result;
        }

        private TokenData GetAuthenticated(CommonParam commonParam, string tokenCode)
        {
            TokenData result = null;
            try
            {
                result = this.CreateRequest<TokenData>(ConstanIG.GET_AUTHENTICATED_URI, HttpMethod.Get, commonParam,
                    HeaderConstants.TOKEN_PARAM, tokenCode,
                    HeaderConstants.APPLICATION_CODE_PARAM, this.applicationCode);
                if (result != null)
                {
                    this.token = result;
                }
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
            return result;
        }

        private T CreateRequest<T>(string requestUri, HttpMethod method, CommonParam commonParam, params string[] headerParams)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.baseUriAcs);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = new TimeSpan(0, 0, ConstanIG.TIME_OUT);
                if (headerParams != null && headerParams.Length % 2 == 0)
                {
                    for (int i = 0; i < headerParams.Length; )
                    {
                        client.DefaultRequestHeaders.Add(headerParams[i], headerParams[i + 1]);
                        i = i + 2;
                    }
                }

                commonParam = commonParam == null ? new CommonParam() : commonParam;

                HttpResponseMessage resp = null;
                try
                {
                    if (method == HttpMethod.Get)
                    {
                        resp = client.GetAsync(requestUri).Result;
                    }
                    else if (method == HttpMethod.Post)
                    {
                        resp = client.PostAsJsonAsync(requestUri, "").Result;
                    }
                }
                catch (Exception ex)
                {
                    commonParam.Messages.Add(string.Format("Không thể truy cập tới máy chủ. (Địa chỉ: {0}{1})", this.baseUriAcs, requestUri));
                    throw ex;
                }

                if (resp == null || !resp.IsSuccessStatusCode)
                {
                    int statusCode = resp.StatusCode.GetHashCode();
                    commonParam.Messages.Add(string.Format("Không thể truy cập tới máy chủ. (Địa chỉ: {0}{1}. Mã: {2})", this.baseUriAcs, requestUri, statusCode));
                    throw new Exception(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", this.baseUriAcs, requestUri, statusCode));
                }
                string responseData = resp.Content.ReadAsStringAsync().Result;
                ApiResultObject<T> data = JsonConvertUtil.DeserializeObject<ApiResultObject<T>>(responseData);
                if (data != null && data.Param != null)
                {
                    if (data.Param.BugCodes != null)
                    {
                        commonParam.BugCodes.AddRange(data.Param.BugCodes);
                    }
                    if (data.Param.Messages != null)
                    {
                        commonParam.Messages.AddRange(data.Param.Messages);
                    }
                }
                if (data == null || !data.Success)
                {
                    throw new Exception(string.Format("Loi khi goi API. Response {0}:", responseData));
                }
                return data.Data;
            }
        }

        private void WriteToRegistry()
        {
            try
            {
                if (this.useRegistry && this.token != null)
                {
                    long? expireTime = DateTimeConvert.SystemDateTimeToTimeNumber(this.token.ExpireTime);
                    RegistryProcessor.Write(ConstanIG.REGISTRY_TOKEN_CODE, token.TokenCode, ConstanIG.REGISTRY_SUBFOLDER);
                    RegistryProcessor.Write(ConstanIG.REGISTRY_RENEW_CODE, token.RenewCode, ConstanIG.REGISTRY_SUBFOLDER);
                    RegistryProcessor.Write(ConstanIG.REGISTRY_EXPIRE_TIME, expireTime.Value, ConstanIG.REGISTRY_SUBFOLDER);
                }
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
        }

        private void ClearRegistry()
        {
            try
            {
                if (this.useRegistry)
                {
                    RegistryProcessor.DeleteValue(ConstanIG.REGISTRY_TOKEN_CODE, ConstanIG.REGISTRY_SUBFOLDER);
                    RegistryProcessor.DeleteValue(ConstanIG.REGISTRY_RENEW_CODE, ConstanIG.REGISTRY_SUBFOLDER);
                    RegistryProcessor.DeleteValue(ConstanIG.REGISTRY_EXPIRE_TIME, ConstanIG.REGISTRY_SUBFOLDER);
                }
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
        }

        private void ReadFromRegistry()
        {
            try
            {
                if (this.useRegistry)
                {
                    string tokenCode = (string)RegistryProcessor.Read(ConstanIG.REGISTRY_TOKEN_CODE, ConstanIG.REGISTRY_SUBFOLDER);
                    string renewCode = (string)RegistryProcessor.Read(ConstanIG.REGISTRY_RENEW_CODE, ConstanIG.REGISTRY_SUBFOLDER);
                    string expireTimeNumberStr = (string)RegistryProcessor.Read(ConstanIG.REGISTRY_EXPIRE_TIME, ConstanIG.REGISTRY_SUBFOLDER);
                    long expireTimeNumber = !string.IsNullOrWhiteSpace(expireTimeNumberStr) ? long.Parse(expireTimeNumberStr) : 0;

                    if (!string.IsNullOrWhiteSpace(tokenCode) && expireTimeNumber > 0)
                    {
                        this.token = new TokenData();
                        token.TokenCode = tokenCode;
                        token.RenewCode = renewCode;
                        token.ExpireTime = DateTimeConvert.TimeNumberToSystemDateTime(expireTimeNumber).Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //LogSystem.Error(this.loginName, ex);
            }
        }
    }
}
