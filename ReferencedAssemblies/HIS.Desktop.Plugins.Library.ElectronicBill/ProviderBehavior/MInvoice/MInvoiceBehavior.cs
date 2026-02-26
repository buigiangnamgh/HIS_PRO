using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Number;
using Inventec.Common.String;
using Inventec.UC.Login.Base;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MInvoice
{
	internal class MInvoiceBehavior : IRun
	{
		private string serviceConfig { get; set; }

		private string accountConfig { get; set; }

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		public MInvoiceBehavior(ElectronicBillDataInput electronicBillDataInput, string serviceConfig, string accountConfig)
		{
			ElectronicBillDataInput = electronicBillDataInput;
			this.serviceConfig = serviceConfig;
			this.accountConfig = accountConfig;
		}

		public ElectronicBillResult Run(ElectronicBillType.ENUM electronicBillType, TemplateEnum.TYPE _templateType)
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			try
			{
				if (Check(electronicBillType, ref electronicBillResult))
				{
					string[] array = serviceConfig.Split('|');
					string text = array[1];
					string ma_dvcs = array[2];
					if (string.IsNullOrEmpty(text))
					{
						LogSystem.Error("Khong tim thay dia chi Webservice URL");
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không tìm thấy địa chỉ Webservice URL");
						return electronicBillResult;
					}
					string[] array2 = accountConfig.Split('|');
					LoginDataMinvoice loginDataMinvoice = new LoginDataMinvoice();
					loginDataMinvoice.username = array2[0].Trim();
					loginDataMinvoice.password = array2[1].Trim();
					loginDataMinvoice.ma_dvcs = ma_dvcs;
					ApiDataResult loginResult = ProcessLogin(text, loginDataMinvoice);
					if (loginResult == null || loginResult.error != null)
					{
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<ApiDataResult>((Expression<Func<ApiDataResult>>)(() => loginResult)), (object)loginResult));
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, loginResult.error);
						return electronicBillResult;
					}
					switch (electronicBillType)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue)
						{
							ThayTheHoaDon(ref electronicBillResult, text, loginDataMinvoice, loginResult, _templateType);
							break;
						}
						TaoHoaDonDienTu(ref electronicBillResult, text, loginDataMinvoice, loginResult, _templateType);
						if (electronicBillResult.InvoiceCode != null && array.Count() == 4 && array[3] == "1")
						{
							KyHoaDonDienTu(ref electronicBillResult, text, loginDataMinvoice, loginResult, _templateType);
						}
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
						InChuyenDoiHoaDon(ref electronicBillResult, text, loginDataMinvoice, loginResult, _templateType);
						break;
					default:
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Chưa tích hợp tính năng");
						break;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return electronicBillResult;
		}

		private void DeleteInvoice(ref ElectronicBillResult result, string serviceUrl, LoginDataMinvoice adoLogin, ApiDataResult loginResult)
		{
			try
			{
				DeleteInvoiceData deleteInvoiceData = new DeleteInvoiceData
				{
					editmode = "3",
					data = new List<DeleteDataItem>
					{
						new DeleteDataItem
						{
							inv_invoiceSeries = ElectronicBillDataInput.TemplateCode + ElectronicBillDataInput.SymbolCode,
							inv_invoiceAuth_Id = result.hoadon68_id.ToString()
						}
					}
				};
				string sendJsonData = JsonConvert.SerializeObject((object)deleteInvoiceData);
				DeleteInvoiceResult deleteInvoiceResult = ApiConsumerV2.CreateRequest<DeleteInvoiceResult>("POST", serviceUrl, "api/InvoiceApi78/SaveV2", loginResult.token, adoLogin.ma_dvcs, sendJsonData);
				if (deleteInvoiceResult != null && deleteInvoiceResult.ok && deleteInvoiceResult.code == "00")
				{
					LogSystem.Info(string.Format("Xóa hóa đơn thành công: inv_invoiceSeries={0}, inv_invoiceAuth_Id={1}", deleteInvoiceData.data[0].inv_invoiceSeries, deleteInvoiceData.data[0].inv_invoiceAuth_Id));
				}
				else
				{
					string arg = ((deleteInvoiceResult != null) ? (deleteInvoiceResult.message ?? "Xóa hóa đơn thất bại") : "Xóa hóa đơn thất bại");
					LogSystem.Error(string.Format("Xóa hóa đơn thất bại: {0}", arg));
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(string.Format("Lỗi khi gọi API xóa hóa đơn: {0}", ex.Message));
			}
			ElectronicBillResultUtil.Set(ref result, false, "Ký hóa đơn thất bại và đã xóa hóa đơn");
		}

		private void KyHoaDonDienTu(ref ElectronicBillResult result, string serviceUrl, LoginDataMinvoice adoLogin, ApiDataResult loginResult, TemplateEnum.TYPE templateType)
		{
			try
			{
				var anon = new
				{
					hoadon68_id = result.InvoiceCode
				};
				string sendJsonData = JsonConvert.SerializeObject((object)anon);
				ApiSignDataResult apiSignDataResult = new ApiSignDataResult();
				try
				{
					apiSignDataResult = ApiConsumerV2.CreateRequest<ApiSignDataResult>("POST", serviceUrl, "api/InvoiceApi78/Sign", loginResult.token, adoLogin.ma_dvcs, sendJsonData);
				}
				catch (Exception ex)
				{
					LogSystem.Error("Lỗi khi gọi API ký hóa đơn: " + ((ex != null) ? ex.ToString() : null));
					DeleteInvoice(ref result, serviceUrl, adoLogin, loginResult);
					return;
				}
				if (apiSignDataResult == null || apiSignDataResult.code != "00" || apiSignDataResult.data == null)
				{
					string text = ((apiSignDataResult != null) ? apiSignDataResult.message : "Ký hóa đơn thất bại");
					ElectronicBillResultUtil.Set(ref result, false, text);
					LogSystem.Error(string.Format("Ký hóa đơn thất bại: {0}", text));
					DeleteInvoice(ref result, serviceUrl, adoLogin, loginResult);
				}
				else if (apiSignDataResult.data.data == null || apiSignDataResult.data.data.tthai != "Đã gửi")
				{
					ElectronicBillResultUtil.Set(ref result, false, "Hóa đơn chưa được gửi CQT");
					LogSystem.Error("Hóa đơn chưa được gửi CQT");
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2.Message);
			}
		}

		private void TaoHoaDonDienTu(ref ElectronicBillResult result, string serviceUrl, LoginDataMinvoice adoLogin, ApiDataResult loginResult, TemplateEnum.TYPE templateType)
		{
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)CreateInvoice(templateType));
				ApiDataResult apiDataResult = ApiConsumerV2.CreateRequest<ApiDataResult>("POST", serviceUrl, "api/InvoiceApi78/SaveV2", loginResult.token, adoLogin.ma_dvcs, sendJsonData);
				result.InvoiceSys = "MINVOICE";
				if (apiDataResult != null && apiDataResult.data != null)
				{
					if (apiDataResult.code == "00")
					{
						result.Success = true;
						result.InvoiceCode = apiDataResult.data.id;
						result.InvoiceLookupCode = apiDataResult.data.sbmat;
						result.InvoiceNumOrder = apiDataResult.data.shdon.ToString();
						result.InvoiceLoginname = adoLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)apiDataResult.data.tdlap);
						result.hoadon68_id = Guid.Parse(apiDataResult.data.hoadon68_id);
					}
					else
					{
						result.Success = false;
						string message = ((apiDataResult != null) ? (apiDataResult.error ?? apiDataResult.message) : "Gửi hóa đơn gốc thất bại");
						ElectronicBillResultUtil.Set(ref result, false, message);
					}
				}
				else if (apiDataResult == null || !string.IsNullOrWhiteSpace(apiDataResult.error) || !string.IsNullOrWhiteSpace(apiDataResult.message))
				{
					result.Success = false;
					string message2 = ((apiDataResult != null) ? (apiDataResult.error ?? apiDataResult.message) : "Gửi hóa đơn gốc thất bại");
					ElectronicBillResultUtil.Set(ref result, false, message2);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private InvoiceADO CreateInvoice(TemplateEnum.TYPE templateType)
		{
			InvoiceADO invoiceADO = new InvoiceADO();
			try
			{
				InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
				invoiceADO.editmode = "1";
				invoiceADO.data = new List<InvoiceData>();
				InvoiceData invoiceData = new InvoiceData();
				invoiceData.tdlap = DateTime.Now;
				invoiceData.khieu = ElectronicBillDataInput.TemplateCode + ElectronicBillDataInput.SymbolCode;
				invoiceData.shdon = "";
				invoiceData.sdhang = "";
				invoiceData.dvtte = "VND";
				invoiceData.htttoan = ElectronicBillDataInput.PaymentMethod ?? "TM/CK";
				invoiceData.tnmua = data.BuyerName;
				invoiceData.mnmua = data.BuyerCode;
				invoiceData.ten = data.BuyerOrganization;
				invoiceData.mst = data.BuyerTaxCode;
				invoiceData.dchi = data.BuyerAddress;
				invoiceData.email = data.BuyerEmail;
				invoiceData.stknmua = data.BuyerAccountNumber;
				invoiceData.cccdan = data.BuyerCCCD;
				invoiceData.sdtnmua = data.BuyerPhone;
				invoiceData.details = GetDetail(templateType);
				invoiceData.tgtttbso = Math.Round(invoiceData.details.First().data.Sum((InvoiceItem s) => s.tgtien), 0);
				invoiceData.tgtthue = Math.Round(invoiceData.details.First().data.Sum((InvoiceItem s) => s.tthue), 0);
				invoiceData.tgtcthue = invoiceData.tgtttbso - invoiceData.tgtthue;
				if (ElectronicBillDataInput.Discount.HasValue && ElectronicBillDataInput.Discount.Value > 0m)
				{
					invoiceData.ttcktmai = ElectronicBillDataInput.Discount.GetValueOrDefault();
				}
				invoiceData.tgtttbchu = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", Inventec.Common.Number.Convert.NumberToNumberRoundMax4(invoiceData.tgtttbso))) + "đồng";
				invoiceData.nguoi_thu = ClientTokenManagerStore.ClientTokenManager.GetUserName();
				Dictionary<string, string> dictionary = General.ProcessDicValueString(ElectronicBillDataInput);
				invoiceData.thongtin_khoa = dictionary["CURRENT_ROOM_DEPARTMENT"];
				invoiceADO.data.Add(invoiceData);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return invoiceADO;
		}

		private List<InvoiceDetails> GetDetail(TemplateEnum.TYPE templateType)
		{
			List<InvoiceDetails> list = new List<InvoiceDetails>();
			try
			{
				InvoiceDetails invoiceDetails = new InvoiceDetails();
				invoiceDetails.data = new List<InvoiceItem>();
				IRunTemplate runTemplate = TemplateFactory.MakeIRun(templateType, ElectronicBillDataInput);
				object obj = runTemplate.Run();
				if (obj == null)
				{
					throw new Exception("Loi phan tich listProductBase");
				}
				int num = 1;
				if (templateType == TemplateEnum.TYPE.TemplateNhaThuoc)
				{
					List<ProductBasePlus> list2 = (List<ProductBasePlus>)obj;
					foreach (ProductBasePlus item in list2)
					{
						InvoiceItem invoiceItem = new InvoiceItem();
						invoiceItem.tchat = 1;
						invoiceItem.stt = num++.ToString("D4");
						invoiceItem.ma = item.ProdCode;
						invoiceItem.ten = item.ProdName;
						invoiceItem.dvtinh = item.ProdUnit;
						if (item.ProdQuantity.HasValue)
						{
							invoiceItem.sluong = Math.Round(item.ProdQuantity.Value, 0, MidpointRounding.AwayFromZero);
						}
						if (item.ProdPrice.HasValue)
						{
							invoiceItem.dgia = Math.Round(item.ProdPrice.Value, 0, MidpointRounding.AwayFromZero);
						}
						invoiceItem.thtien = item.AmountWithoutTax.GetValueOrDefault();
						invoiceItem.tthue = item.TaxAmount.GetValueOrDefault();
						invoiceItem.tgtien = item.Amount;
						if (!item.TaxPercentage.HasValue)
						{
							invoiceItem.tsuat = -1m;
						}
						else
						{
							invoiceItem.tsuat = item.TaxConvert;
						}
						invoiceDetails.data.Add(invoiceItem);
					}
				}
				else
				{
					List<ProductBase> list3 = (List<ProductBase>)obj;
					foreach (ProductBase item2 in list3)
					{
						InvoiceItem invoiceItem2 = new InvoiceItem();
						invoiceItem2.tchat = 1;
						invoiceItem2.stt = num++.ToString("D4");
						invoiceItem2.ma = item2.ProdCode;
						invoiceItem2.ten = item2.ProdName;
						invoiceItem2.dvtinh = item2.ProdUnit;
						if (item2.ProdQuantity.HasValue)
						{
							invoiceItem2.sluong = Math.Round(item2.ProdQuantity.Value, 0, MidpointRounding.AwayFromZero);
						}
						if (item2.ProdPrice.HasValue)
						{
							invoiceItem2.dgia = Math.Round(item2.ProdPrice.Value, 0, MidpointRounding.AwayFromZero);
						}
						invoiceItem2.thtien = item2.Amount;
						invoiceItem2.tthue = 0m;
						invoiceItem2.tgtien = item2.Amount;
						invoiceItem2.tsuat = -1m;
						invoiceDetails.data.Add(invoiceItem2);
					}
				}
				list.Add(invoiceDetails);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return list;
		}

		private void InChuyenDoiHoaDon(ref ElectronicBillResult result, string serviceUrl, LoginDataMinvoice adoLogin, ApiDataResult loginResult, TemplateEnum.TYPE templateType)
		{
			try
			{
				string requestUri = string.Format("api/InvoiceApi78/PrintInvoice?id={0}", ElectronicBillDataInput.Transaction.INVOICE_CODE);
				byte[] array = ApiConsumerV2.CreateRequestGetByte("GET", serviceUrl, requestUri, loginResult.token, adoLogin.ma_dvcs);
				if (array != null && array.Length != 0)
				{
					result.Success = true;
					result.InvoiceSys = "MINVOICE";
					result.PdfFileData = array;
					string text = Application.StartupPath + "\\temp";
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					string text2 = text + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
					File.WriteAllBytes(text2, array);
					result.InvoiceLink = text2;
					LogSystem.Info("Nhận dữ liệu PDF thành công");
				}
				else
				{
					ElectronicBillResultUtil.Set(ref result, false, "Không nhận được dữ liệu PDF từ API");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				ElectronicBillResultUtil.Set(ref result, false, "Lỗi khi gọi API in hóa đơn: " + ex.Message);
			}
		}

		private ReplaceInvoice ReplaceInvoice()
		{
			InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
			ReplaceInvoice replaceInvoice = new ReplaceInvoice();
			replaceInvoice.mode = 1;
			replaceInvoice.data = new List<RpInvoiceData>();
			RpInvoiceData rpInvoiceData = new RpInvoiceData();
			rpInvoiceData.inv_originalId = ElectronicBillDataInput.Transaction.TDL_ORIGINAL_EI_CODE;
			rpInvoiceData.ngayvb = DateTime.Now.ToString("dd/MM/yyyy");
			rpInvoiceData.inv_invoiceSeries = ElectronicBillDataInput.TemplateCode + ElectronicBillDataInput.SymbolCode;
			rpInvoiceData.inv_invoiceIssuedDate = Inventec.Common.DateTime.Convert.TimeNumberToDateString(ElectronicBillDataInput.Transaction.TRANSACTION_DATE);
			rpInvoiceData.inv_currencyCode = "VND";
			rpInvoiceData.inv_exchangeRate = 1m;
			rpInvoiceData.inv_buyerAddressLine = data.BuyerAddress;
			rpInvoiceData.inv_paymentMethodName = ElectronicBillDataInput.PaymentMethod ?? "TM / CK";
			rpInvoiceData.details = new List<RpInvoiceDetails>();
			RpInvoiceDetails rpInvoiceDetails = new RpInvoiceDetails();
			rpInvoiceDetails.data = new List<RpInvoiceItem>();
			RpInvoiceItem rpInvoiceItem = new RpInvoiceItem();
			rpInvoiceItem.stt = ElectronicBillDataInput.NumOrder.ToString();
			rpInvoiceItem.inv_itemName = data.BuyerOrganization;
			rpInvoiceItem.ma_thue = ElectronicBillDataInput.Branch.TAX_CODE;
			rpInvoiceDetails.data.Add(rpInvoiceItem);
			rpInvoiceData.details.Add(rpInvoiceDetails);
			replaceInvoice.data.Add(rpInvoiceData);
			return replaceInvoice;
		}

		private void ThayTheHoaDon(ref ElectronicBillResult result, string serviceUrl, LoginDataMinvoice adoLogin, ApiDataResult loginResult, TemplateEnum.TYPE templateType)
		{
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)ReplaceInvoice());
				ApiDataResult apiDataResult = ApiConsumerV2.CreateRequest<ApiDataResult>("POST", serviceUrl, "api/InvoiceApi78/ThayTheSaveSign", loginResult.token, adoLogin.ma_dvcs, sendJsonData);
				result.InvoiceSys = "MINVOICE";
				if (apiDataResult != null && apiDataResult.data != null)
				{
					if (apiDataResult.code == "00")
					{
						result.Success = true;
						result.InvoiceCode = apiDataResult.data.id;
						result.InvoiceLookupCode = apiDataResult.data.sbmat;
						result.InvoiceNumOrder = apiDataResult.data.shdon.ToString();
						result.InvoiceLoginname = adoLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)apiDataResult.data.tdlap);
						result.hoadon68_id = Guid.Parse(apiDataResult.data.hoadon68_id);
					}
					else
					{
						result.Success = false;
						string message = ((apiDataResult != null) ? (apiDataResult.error ?? apiDataResult.message) : "Gửi hóa đơn gốc thất bại");
						ElectronicBillResultUtil.Set(ref result, false, message);
					}
				}
				else if (apiDataResult == null || !string.IsNullOrWhiteSpace(apiDataResult.error) || !string.IsNullOrWhiteSpace(apiDataResult.message))
				{
					result.Success = false;
					string message2 = ((apiDataResult != null) ? (apiDataResult.error ?? apiDataResult.message) : "Gửi hóa đơn gốc thất bại");
					ElectronicBillResultUtil.Set(ref result, false, message2);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private ApiDataResult ProcessLogin(string serviceUrl, LoginDataMinvoice adoLogin)
		{
			ApiDataResult result = null;
			try
			{
				string requestUri = "api/Account/Login";
				string sendJsonData = JsonConvert.SerializeObject((object)adoLogin);
				result = ApiConsumerV2.CreateRequest<ApiDataResult>("POST", serviceUrl, requestUri, null, null, sendJsonData);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private bool Check(ElectronicBillType.ENUM _electronicBillTypeEnum, ref ElectronicBillResult electronicBillResult)
		{
			bool result = true;
			try
			{
				string[] array = serviceConfig.Split('|');
				if (array.Length < 3)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
				}
				if (array[0] != "MINVOICE")
				{
					throw new Exception("Không đúng cấu hình nhà cung cấp M-invoice");
				}
				string[] array2 = accountConfig.Split('|');
				if (array2.Length != 2)
				{
					throw new Exception("Sai định dạng cấu hình tài khoản.");
				}
				if (_electronicBillTypeEnum == ElectronicBillType.ENUM.CREATE_INVOICE)
				{
					if (ElectronicBillDataInput == null)
					{
						throw new Exception("Không có dữ liệu phát hành hóa đơn.");
					}
					if (ElectronicBillDataInput.Treatment == null)
					{
						throw new Exception("Không có thông tin hồ sơ điều trị.");
					}
					if (ElectronicBillDataInput.Branch == null)
					{
						throw new Exception("Không có thông tin chi nhánh.");
					}
				}
			}
			catch (Exception ex)
			{
				result = false;
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, ex.Message);
				LogSystem.Warn(ex);
			}
			return result;
		}
	}
}
