using System;
using System.Net.Http;
using System.Text;
using Inventec.Common.Logging;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.BACHMAI
{
	internal class ApiConsumer
	{
		public static T CreateRequest<T>(string baseUri, string token, string requestUri, string tid, object objData)
		{
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(baseUri);
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
				if (!string.IsNullOrWhiteSpace(token))
				{
					httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
				}
				httpClient.Timeout = new TimeSpan(0, 0, 90);
				HttpResponseMessage httpResponseMessage = null;
				string text = null;
				if (!string.IsNullOrWhiteSpace(tid))
				{
					SendDataADO sendDataADO = new SendDataADO();
					sendDataADO.tid = tid;
					sendDataADO.data = objData;
					text = JsonConvert.SerializeObject((object)sendDataADO);
				}
				else if (!string.IsNullOrWhiteSpace(requestUri))
				{
					text = JsonConvert.SerializeObject(objData);
				}
				LogSystem.Info("_____sendJsonData : " + text);
				string requestUri2 = requestUri;
				int num = baseUri.IndexOf('/', baseUri.IndexOf("//") + 2);
				if (num > 0)
				{
					string text2 = baseUri.Substring(num);
					if (!requestUri.Contains(text2))
					{
						requestUri2 = text2 + requestUri;
					}
				}
				httpResponseMessage = httpClient.PostAsync(requestUri2, new StringContent(text, Encoding.UTF8, "application/json")).Result;
				if (httpResponseMessage == null || !httpResponseMessage.IsSuccessStatusCode)
				{
					int hashCode = httpResponseMessage.StatusCode.GetHashCode();
					throw new Exception(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", baseUri, requestUri, hashCode));
				}
				string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
				LogSystem.Info("__________________api responseData: " + result);
				T val = default(T);
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
	}
}
