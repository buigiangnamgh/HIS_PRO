using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Inventec.Common.Logging;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SAFECERT
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
				ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
				HttpResponseMessage httpResponseMessage = null;
				string text = JsonConvert.SerializeObject(sendData);
				LogSystem.Info("_____sendJsonData : " + text);
				httpResponseMessage = httpClient.PostAsync(requestUri, new StringContent(text, Encoding.UTF8, "application/json")).Result;
				if (httpResponseMessage == null || !httpResponseMessage.IsSuccessStatusCode)
				{
					int hashCode = httpResponseMessage.StatusCode.GetHashCode();
					LogSystem.Error("fullrequestUri: " + requestUri);
					throw new Exception(string.Format("Loi khi goi API: {0}{1}. StatusCode: {2}", baseUri, requestUri, hashCode));
				}
				string result = httpResponseMessage.Content.ReadAsStringAsync().Result;
				T val = JsonConvert.DeserializeObject<T>(result);
				if (val == null)
				{
					throw new Exception(string.Format("Loi khi goi API. Response {0}:", result));
				}
				return val;
			}
		}

		internal static string CombileUrl(params string[] data)
		{
			string text = "";
			List<string> list = new List<string>();
			for (int i = 0; i < data.Length; i++)
			{
				list.Add(data[i].Trim('/'));
			}
			return string.Join("/", list);
		}
	}
}
