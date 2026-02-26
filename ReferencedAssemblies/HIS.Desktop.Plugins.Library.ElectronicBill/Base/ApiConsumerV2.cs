using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Inventec.Common.Logging;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Base
{
	internal class ApiConsumerV2
	{
		private static int TIME_OUT = 90;

		public static T CallWebRequest<T>(string method, string api, string username, string password, Dictionary<string, string> headers, string contentType, string parameter)
		{
			T result = default(T);
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			if (api.EndsWith("&"))
			{
				api = api.Substring(0, api.Length - 1);
			}
			LogSystem.Debug(LogUtil.TraceData("FULL API:", (object)api));
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(api);
			httpWebRequest.Method = method;
			httpWebRequest.KeepAlive = true;
			httpWebRequest.Timeout = TIME_OUT * 1000;
			httpWebRequest.ContentType = contentType;
			if (headers != null)
			{
				foreach (KeyValuePair<string, string> header in headers)
				{
					httpWebRequest.Headers.Add(header.Key, header.Value);
				}
			}
			if (method.ToLower() != "GET".ToLower() && !string.IsNullOrWhiteSpace(parameter))
			{
				byte[] bytes = new UTF8Encoding().GetBytes(parameter);
				httpWebRequest.ContentLength = bytes.Length;
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(bytes, 0, bytes.Length);
				}
			}
			try
			{
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					if (httpWebResponse.StatusCode == HttpStatusCode.OK)
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							string text = streamReader.ReadToEnd();
							if (!string.IsNullOrWhiteSpace(text))
							{
								result = JsonConvert.DeserializeObject<T>(text);
								return result;
							}
						}
					}
					else if (httpWebResponse.StatusCode == HttpStatusCode.NotFound)
					{
						LogSystem.Debug(string.Format("{0} {1} {2}: {3} - {4}", "_______Api Response Result: ", httpWebResponse.StatusCode.ToString(), api, username, password));
					}
				}
			}
			catch (WebException ex)
			{
				using (WebResponse webResponse = ex.Response)
				{
					HttpWebResponse httpWebResponse2 = (HttpWebResponse)webResponse;
					using (Stream stream2 = webResponse.GetResponseStream())
					{
						using (StreamReader streamReader2 = new StreamReader(stream2))
						{
							string message = streamReader2.ReadToEnd();
							throw new Exception(message);
						}
					}
				}
			}
			return result;
		}

		public static T CreateRequest<T>(string method, string baseUri, string requestUri, string token, string maDvcs, string sendJsonData)
		{
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(baseUri);
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
				if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(maDvcs))
				{
					httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bear {0};{1}", token, maDvcs));
				}
				else if (!string.IsNullOrWhiteSpace(token))
				{
					httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
				}
				httpClient.Timeout = new TimeSpan(0, 0, 90);
				HttpResponseMessage httpResponseMessage = null;
				string text = requestUri;
				int num = baseUri.IndexOf('/', baseUri.IndexOf("//") + 2);
				if (num > 0)
				{
					string text2 = baseUri.Substring(num);
					if (!requestUri.Contains(text2))
					{
						text = text2 + requestUri;
					}
				}
				LogSystem.Info(string.Format("API: {0}. sendJsonData: {1}", text, sendJsonData));
				httpResponseMessage = ((!method.Equals("GET")) ? httpClient.PostAsync(text, new StringContent(sendJsonData, Encoding.UTF8, "application/json")).Result : httpClient.GetAsync(text).Result);
				if (httpResponseMessage == null || !httpResponseMessage.IsSuccessStatusCode)
				{
					int hashCode = httpResponseMessage.StatusCode.GetHashCode();
					throw new Exception(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", baseUri, requestUri, hashCode));
				}
				T val = default(T);
				string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
				LogSystem.Info("__________________api responseData: " + result);
				try
				{
					val = JsonConvert.DeserializeObject<T>(result);
					if (val == null)
					{
						throw new Exception(string.Format("Loi khi goi API. Response {0}:", result));
					}
				}
				catch (Exception ex)
				{
					LogSystem.Error(ex);
					throw new Exception(result);
				}
				return val;
			}
		}

		public static T CreateRequest<T>(string method, string baseUri, string requestUri, string token, string sendJsonData)
		{
			return CreateRequest<T>(method, baseUri, requestUri, token, null, sendJsonData);
		}

		public static byte[] CreateRequestGetByte(string method, string baseUri, string requestUri, string token, string maDvcs)
		{
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(baseUri);
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
				if (!string.IsNullOrWhiteSpace(token))
				{
					httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bear {0};{1}", token, maDvcs));
				}
				httpClient.Timeout = new TimeSpan(0, 0, 90);
				HttpResponseMessage httpResponseMessage = null;
				string requestUri2 = requestUri;
				int num = baseUri.IndexOf('/', baseUri.IndexOf("//") + 2);
				if (num > 0)
				{
					string text = baseUri.Substring(num);
					if (!requestUri.Contains(text))
					{
						requestUri2 = text + requestUri;
					}
				}
				if (method.Equals("GET"))
				{
					httpResponseMessage = httpClient.GetAsync(requestUri2).Result;
				}
				if (httpResponseMessage == null || !httpResponseMessage.IsSuccessStatusCode)
				{
					int hashCode = httpResponseMessage.StatusCode.GetHashCode();
					throw new Exception(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", baseUri, requestUri, hashCode));
				}
				return httpResponseMessage.Content.ReadAsByteArrayAsync().Result;
			}
		}
	}
}
