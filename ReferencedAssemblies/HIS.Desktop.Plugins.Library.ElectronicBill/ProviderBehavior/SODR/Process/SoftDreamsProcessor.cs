using System;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR.Process
{
	internal class SoftDreamsProcessor
	{
		private string Token;

		private string Api;

		public SoftDreamsProcessor(string api, string user, string pass)
		{
			Api = api;
			LoginData(api, user, pass);
		}

		private void LoginData(string api, string user, string pass)
		{
			if (string.IsNullOrEmpty(api) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
			{
				throw new Exception("Thông tin đăng nhập không hợp lệ vui lòng kiểm tra lại.");
			}
			var objData = new
			{
				Username = user,
				Password = pass
			};
			string fullapi = RequestUriStore.CombileUrl(api, UriStore.LoginUrl);
			ResultDataV2 resultDataV = ApiConsumer.CreateRequest<ResultDataV2>(fullapi, null, objData);
			if (resultDataV != null && !string.IsNullOrEmpty(resultDataV.AccessToken))
			{
				Token = resultDataV.AccessToken;
				return;
			}
			throw new Exception("Thông tin đăng nhập không hợp lệ vui lòng kiểm tra lại.");
		}

		public ResultDataV2 CreateInvoice(IssueCreateV2 data)
		{
			if (data == null)
			{
				throw new Exception("Thông tin hóa đơn không hợp lệ vui lòng kiểm tra lại.");
			}
			string fullapi = RequestUriStore.CombileUrl(Api, UriStore.EinvoiceIssueUrl);
			ResultDataV2 resultDataV = ApiConsumer.CreateRequest<ResultDataV2>(fullapi, Token, data);
			if (resultDataV != null && resultDataV.Status == "1" && resultDataV.InvoiceResult != null)
			{
				return resultDataV;
			}
			throw new Exception("Tạo hóa đơn điện tử thất bại");
		}

		public ResultDataV2 ReplaceInvoice(ReplaceInvoiceV2 data)
		{
			if (data == null)
			{
				throw new Exception("Thông tin hóa đơn không hợp lệ vui lòng kiểm tra lại.");
			}
			string fullapi = RequestUriStore.CombileUrl(Api, UriStore.EinvoiceReplace);
			ResultDataV2 resultDataV = ApiConsumer.CreateRequest<ResultDataV2>(fullapi, Token, data);
			if (resultDataV != null && resultDataV.Status == "1" && resultDataV.InvoiceResult != null)
			{
				return resultDataV;
			}
			string message = ((resultDataV != null) ? resultDataV.Message : null) ?? "Thay thế hóa đơn điện tử thất bại";
			throw new Exception(message);
		}

		public bool DeleteInvoice(string pattern, string key)
		{
			if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(key))
			{
				throw new Exception("Thông tin hủy hóa đơn không hợp lệ vui lòng kiểm tra lại.");
			}
			var objData = new
			{
				Pattern = pattern,
				Ikey = key
			};
			string fullapi = RequestUriStore.CombileUrl(Api, UriStore.EinvoiceCancel);
			ResultDataV2 resultDataV = ApiConsumer.CreateRequest<ResultDataV2>(fullapi, Token, objData);
			if (resultDataV != null && resultDataV.Status == "1")
			{
				return true;
			}
			throw new Exception("Hủy hóa đơn điện tử thất bại");
		}

		public string GetInvoice(string pattern, string key)
		{
			if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(key))
			{
				throw new Exception("Thông tin tải hóa đơn không hợp lệ vui lòng kiểm tra lại.");
			}
			var objData = new
			{
				Pattern = pattern,
				Ikey = key,
				Type = "PDF"
			};
			string fullapi = RequestUriStore.CombileUrl(Api, UriStore.EinvoiceDownload);
			ResultDataV2 resultDataV = ApiConsumer.CreateRequest<ResultDataV2>(fullapi, Token, objData);
			if (resultDataV != null && resultDataV.Status == "1" && !string.IsNullOrEmpty(resultDataV.Data))
			{
				return resultDataV.Data;
			}
			throw new Exception("Tải hóa đơn điện tử thất bại");
		}
	}
}
