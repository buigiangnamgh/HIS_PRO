using System;
using System.Net.Http;
using System.Text;
using Inventec.Common.Logging;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.CTO
{
	internal class ApiConsumer
	{
		public static T CreateRequest<T>(string baseUri, string requestUri, object sendData)
		{
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(baseUri);
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
				httpClient.Timeout = new TimeSpan(0, 0, 90);
				HttpResponseMessage httpResponseMessage = null;
				string text = JsonConvert.SerializeObject(sendData);
				LogSystem.Info("_____sendJsonData : " + text);
				string text2 = baseUri.Substring(baseUri.IndexOf('/', baseUri.IndexOf("//") + 2));
				string requestUri2 = requestUri;
				if (!requestUri.Contains(text2))
				{
					requestUri2 = text2 + requestUri;
				}
				httpResponseMessage = httpClient.PostAsync(requestUri2, new StringContent(text, Encoding.UTF8, "application/json")).Result;
				if (httpResponseMessage == null || !httpResponseMessage.IsSuccessStatusCode)
				{
					int hashCode = httpResponseMessage.StatusCode.GetHashCode();
					throw new Exception(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", baseUri, requestUri, hashCode));
				}
				string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
				T val = JsonConvert.DeserializeObject<T>(result);
				if (val == null)
				{
					throw new Exception(string.Format("Loi khi goi API: {0}{1}. Response {2}:", baseUri, requestUri, result));
				}
				return val;
			}
		}
	}
}
