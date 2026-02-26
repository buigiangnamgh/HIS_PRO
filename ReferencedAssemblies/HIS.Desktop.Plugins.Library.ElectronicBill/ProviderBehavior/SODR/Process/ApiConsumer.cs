using System;
using System.Net.Http;
using System.Text;
using Inventec.Common.Logging;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR.Process
{
	internal class ApiConsumer
	{
		public static T CreateRequest<T>(string fullapi, string token, object objData)
		{
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
				if (!string.IsNullOrWhiteSpace(token))
				{
					httpClient.DefaultRequestHeaders.Add("Authorization", token);
				}
				httpClient.Timeout = new TimeSpan(0, 0, 90);
				string text = JsonConvert.SerializeObject(objData);
				LogSystem.Info("_____sendJsonData : " + text);
				HttpResponseMessage result = httpClient.PostAsync(fullapi, new StringContent(text, Encoding.UTF8, "application/json")).Result;
				if (result == null || !result.IsSuccessStatusCode)
				{
					int hashCode = result.StatusCode.GetHashCode();
					throw new Exception(string.Format("Loi khi goi API: {0}. StatusCode: {1}", fullapi, hashCode));
				}
				string result2 = result.Content.ReadAsStringAsync().Result;
				LogSystem.Info("__________________api responseData: " + result2);
				T val = default(T);
				try
				{
					val = JsonConvert.DeserializeObject<T>(result2);
					if (val == null)
					{
						throw new Exception(string.Format("Loi khi goi API. Response {0}:", result2));
					}
				}
				catch (Exception ex)
				{
					LogSystem.Error(ex);
					throw new Exception(result2);
				}
				return val;
			}
		}
	}
}
