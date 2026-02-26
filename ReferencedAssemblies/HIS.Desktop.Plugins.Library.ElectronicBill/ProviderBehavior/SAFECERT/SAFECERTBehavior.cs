using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SAFECERT.ADO;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.String;
using Inventec.Common.TypeConvert;
using MOS.EFMODEL.DataModels;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SAFECERT
{
	internal class SAFECERTBehavior : IRun
	{
		private class ServiceConfig
		{
			public string BaseUrl { get; set; }

			public string RequestUrl { get; set; }

			public string KeyTrans { get; set; }
		}

		private ElectronicBillDataInput ElectronicBillDataInput;

		private string serviceConfigStr;

		private string accountConfigStr;

		private static ServiceConfig serviceConfig = new ServiceConfig();

		private TemplateEnum.TYPE TempType;

		public SAFECERTBehavior(ElectronicBillDataInput electronicBillDataInput, string serviceConfig, string accountConfig)
		{
			ElectronicBillDataInput = electronicBillDataInput;
			serviceConfigStr = serviceConfig;
			accountConfigStr = accountConfig;
		}

		ElectronicBillResult IRun.Run(ElectronicBillType.ENUM _electronicBillTypeEnum, TemplateEnum.TYPE templateType)
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			try
			{
				TempType = templateType;
				if (Check(ref electronicBillResult))
				{
					switch (_electronicBillTypeEnum)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						ProcessCreateInvoice(ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
					case ElectronicBillType.ENUM.DELETE_INVOICE:
					case ElectronicBillType.ENUM.CANCEL_INVOICE:
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Chưa tích hợp tính năng này");
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

		private void ProcessCreateInvoice(ref ElectronicBillResult billResult)
		{
			try
			{
				if (ElectronicBillDataInput == null)
				{
					return;
				}
				string[] array = serviceConfigStr.Split('|');
				string text = "";
				if (ElectronicBillDataInput.Transaction != null)
				{
					text = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
				}
				else if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
				{
					List<string> source = ElectronicBillDataInput.ListTransaction.Select((V_HIS_TRANSACTION s) => s.TRANSACTION_CODE).ToList();
					text = source.OrderBy((string o) => o).First();
				}
				EInvoice invoice = new EInvoice();
				invoice.idCompany = ElectronicBillDataInput.Branch.TAX_CODE.Replace("-", "");
				invoice.idMaster = text;
				invoice.sodonhang = text;
				InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
				invoice.diachi = data.BuyerAddress ?? "";
				invoice.dienthoainguoimua = data.BuyerPhone ?? "";
				invoice.tenkhachhang = data.BuyerName;
				invoice.tendonvi = data.BuyerOrganization ?? "";
				invoice.masothue = data.BuyerTaxCode ?? "";
				invoice.emailnguoimua = data.BuyerEmail ?? "";
				invoice.faxnguoimua = "";
				invoice.maKhachHang = data.BuyerCode;
				invoice.hinhthuctt = data.PaymentMethod ?? "TM/CK";
				string date = GetDate(data.TransactionTime);
				invoice.ngaydonhang = date;
				invoice.ngayhopdong = date;
				invoice.noimotaikhoan = "";
				invoice.socontainer = "";
				invoice.sohopdong = "";
				invoice.sotaikhoan = "";
				invoice.sovandon = "";
				if (ElectronicBillDataInput.Transaction != null)
				{
					V_HIS_CASHIER_ROOM val = BackendDataWorker.Get<V_HIS_CASHIER_ROOM>().FirstOrDefault((V_HIS_CASHIER_ROOM o) => o.ID == ElectronicBillDataInput.Transaction.CASHIER_ROOM_ID);
					if (val != null)
					{
						invoice.macnCh = val.EINVOICE_ROOM_CODE ?? "";
						invoice.tencnCh = val.EINVOICE_ROOM_NAME ?? "";
					}
				}
				invoice.hoaDonMoRong = "";
				string cCCDAN = "";
				string sOHOCHIEU = "";
				string mDVQHNSACH = "";
				if (ElectronicBillDataInput.Transaction != null)
				{
					HIS_TRANSACTION transaction = ElectronicBillDataInput.Transaction;
					if (!string.IsNullOrWhiteSpace(transaction.BUYER_SOCIAL_RELATIONS_CODE))
					{
						mDVQHNSACH = transaction.BUYER_SOCIAL_RELATIONS_CODE;
					}
					if (!string.IsNullOrWhiteSpace(transaction.BUYER_IDENTITY_NUMBER))
					{
						if (transaction.BUYER_IDENTITY_TYPE == 1 || transaction.BUYER_IDENTITY_TYPE == 2 || !transaction.BUYER_IDENTITY_TYPE.HasValue)
						{
							cCCDAN = transaction.BUYER_IDENTITY_NUMBER;
						}
						else if (transaction.BUYER_IDENTITY_TYPE == 3)
						{
							sOHOCHIEU = transaction.BUYER_IDENTITY_NUMBER;
						}
					}
				}
				HoaDonMoRong hoaDonMoRong = new HoaDonMoRong
				{
					CCCDAN = cCCDAN,
					SOHOCHIEU = sOHOCHIEU,
					MDVQHNSACH = mDVQHNSACH
				};
				invoice.hoaDonMoRong = JsonConvert.SerializeObject((object)hoaDonMoRong);
				invoice.loaiTienTe = "VND";
				invoice.tyGia = 1m;
				invoice.soHieuBangKe = "";
				invoice.listEinvoiceLine = GetEinvoiceLine(text, serviceConfig.KeyTrans);
				ProcessTongTien(invoice);
				ApiObject apiObject = new ApiObject();
				apiObject.einvoiceHeaderObj = invoice;
				apiObject.keyTrans = serviceConfig.KeyTrans;
				apiObject.masoThue = ElectronicBillDataInput.Branch.TAX_CODE.Replace("-", "");
				try
				{
					ApiResultObject apiResultObject = ApiConsumer.CreateRequest<ApiResultObject>(serviceConfig.BaseUrl, serviceConfig.RequestUrl, apiObject);
					if (apiResultObject != null)
					{
						if (apiResultObject.maKetQua == "XHD_1111")
						{
							billResult.Success = true;
							billResult.InvoiceSys = "SAFECERT";
							billResult.InvoiceCode = string.Format("{0}|{1}", "SAFECERT", ElectronicBillDataInput.Transaction.NUM_ORDER);
							billResult.InvoiceNumOrder = "";
							billResult.InvoiceTime = Inventec.Common.DateTime.Get.Now();
						}
						else
						{
							ElectronicBillResultUtil.Set(ref billResult, false, string.Format("{0}. {1}", apiResultObject.maKetQua, apiResultObject.moTaKetQua));
							LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<EInvoice>((Expression<Func<EInvoice>>)(() => invoice)), (object)invoice));
						}
					}
					else
					{
						ElectronicBillResultUtil.Set(ref billResult, false, "");
						LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<EInvoice>((Expression<Func<EInvoice>>)(() => invoice)), (object)invoice));
					}
				}
				catch (Exception ex)
				{
					ElectronicBillResultUtil.Set(ref billResult, false, ex.Message);
					LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<EInvoice>((Expression<Func<EInvoice>>)(() => invoice)), (object)invoice));
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		private void ProcessTongTien(EInvoice invoice)
		{
			try
			{
				if (invoice == null || invoice.listEinvoiceLine == null || invoice.listEinvoiceLine.Count <= 0)
				{
					return;
				}
				invoice.tongsauvat05 = "";
				invoice.tongtruocvat05 = "";
				invoice.tongvat05 = "";
				invoice.tongsauvat10 = "";
				invoice.tongtruocvat10 = "";
				invoice.tongvat10 = "";
				foreach (EinvoiceLine item in invoice.listEinvoiceLine)
				{
					if (!string.IsNullOrWhiteSpace(item.thuesuat))
					{
						if (string.IsNullOrWhiteSpace(invoice.tongsauvat05))
						{
							invoice.tongsauvat05 = "0";
						}
						if (string.IsNullOrWhiteSpace(invoice.tongtruocvat05))
						{
							invoice.tongtruocvat05 = "0";
						}
						if (string.IsNullOrWhiteSpace(invoice.tongvat05))
						{
							invoice.tongvat05 = "0";
						}
						if (string.IsNullOrWhiteSpace(invoice.tongsauvat10))
						{
							invoice.tongsauvat10 = "0";
						}
						if (string.IsNullOrWhiteSpace(invoice.tongtruocvat10))
						{
							invoice.tongtruocvat10 = "0";
						}
						if (string.IsNullOrWhiteSpace(invoice.tongvat10))
						{
							invoice.tongvat10 = "0";
						}
						string thuesuat = item.thuesuat;
						string text = thuesuat;
						if (!(text == "5"))
						{
							if (text == "10")
							{
								invoice.tongsauvat10 = Math.Round(Parse.ToDecimal(item.tongtien) + Parse.ToDecimal(invoice.tongsauvat10), 0).ToString() ?? "";
								invoice.tongtruocvat10 = Math.Round(Parse.ToDecimal(item.thanhtien) + Parse.ToDecimal(invoice.tongtruocvat10), 0).ToString() ?? "";
								invoice.tongvat10 = Math.Round(Parse.ToDecimal(item.tienthue) + Parse.ToDecimal(invoice.tongvat10), 0).ToString() ?? "";
							}
						}
						else
						{
							invoice.tongsauvat05 = Math.Round(Parse.ToDecimal(item.tongtien) + Parse.ToDecimal(invoice.tongsauvat05), 0).ToString() ?? "";
							invoice.tongtruocvat05 = Math.Round(Parse.ToDecimal(item.thanhtien) + Parse.ToDecimal(invoice.tongtruocvat05), 0).ToString() ?? "";
							invoice.tongvat05 = Math.Round(Parse.ToDecimal(item.tienthue) + Parse.ToDecimal(invoice.tongvat05), 0).ToString() ?? "";
						}
					}
					else
					{
						invoice.tongkhongchiuvat = Math.Round(Parse.ToDecimal(item.tongtien) + Parse.ToDecimal(invoice.tongkhongchiuvat), 0).ToString() ?? "";
					}
					invoice.tongtienchuathue = Math.Round(Parse.ToDecimal(item.thanhtien) + Parse.ToDecimal(invoice.tongtienchuathue), 0).ToString() ?? "";
					invoice.tongtienthue = Math.Round(Parse.ToDecimal(item.tienthue) + Parse.ToDecimal(invoice.tongtienthue), 0).ToString() ?? "";
					invoice.tongtientt = Math.Round(Parse.ToDecimal(item.tongtien) + Parse.ToDecimal(invoice.tongtientt), 0).ToString() ?? "";
				}
				invoice.tienchiphikhac = "";
				invoice.tongtienckgg = "";
                invoice.sotienbangchu = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", invoice.tongtientt)) + "đồng";
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private List<EinvoiceLine> GetEinvoiceLine(string idMaster, string keytrans)
		{
			List<EinvoiceLine> list = new List<EinvoiceLine>();
			try
			{
				IRunTemplate runTemplate = TemplateFactory.MakeIRun(TempType, ElectronicBillDataInput);
				object obj = runTemplate.Run();
				if (obj == null)
				{
					throw new Exception("Loi phan tich listProductBase");
				}
				if (TempType != TemplateEnum.TYPE.TemplateNhaThuoc)
				{
					List<ProductBase> list2 = (List<ProductBase>)obj;
					int num = 1;
					foreach (ProductBase item in list2)
					{
						EinvoiceLine einvoiceLine = new EinvoiceLine();
						einvoiceLine.idMaster = idMaster;
						einvoiceLine.mahang = item.ProdCode;
						einvoiceLine.tenhang = item.ProdName;
						einvoiceLine.donvitinh = item.ProdUnit;
						einvoiceLine.sothutu = num.ToString();
						einvoiceLine.sothutuIdx = num;
						if (item.ProdQuantity.HasValue)
						{
							einvoiceLine.soluong = Math.Round(item.ProdQuantity.Value, 0, MidpointRounding.AwayFromZero).ToString() ?? "";
						}
						if (item.ProdPrice.HasValue)
						{
							einvoiceLine.dongia = Math.Round(item.ProdPrice.Value, 0, MidpointRounding.AwayFromZero).ToString() ?? "";
						}
						einvoiceLine.thanhtien = Math.Round(item.Amount, 0, MidpointRounding.AwayFromZero).ToString() ?? "";
						einvoiceLine.tongtien = Math.Round(item.Amount, 0, MidpointRounding.AwayFromZero).ToString() ?? "";
						einvoiceLine.loaihhdv = ((item.Type == 1) ? "THUOC" : "KCB");
						einvoiceLine.thuesuat = "KCT";
						einvoiceLine.tienthue = "0";
						einvoiceLine.thuettdb = "0";
						einvoiceLine.tinhchat = 1;
						list.Add(einvoiceLine);
						num++;
					}
				}
				else
				{
					List<ProductBasePlus> list3 = (List<ProductBasePlus>)obj;
					int num2 = 1;
					foreach (ProductBasePlus item2 in list3)
					{
						EinvoiceLine einvoiceLine2 = new EinvoiceLine();
						einvoiceLine2.idMaster = idMaster;
						einvoiceLine2.mahang = item2.ProdCode;
						einvoiceLine2.tenhang = item2.ProdName;
						einvoiceLine2.donvitinh = item2.ProdUnit;
						einvoiceLine2.sothutu = num2.ToString();
						einvoiceLine2.sothutuIdx = num2;
						einvoiceLine2.loaihhdv = "THUOC";
						if (item2.ProdQuantity.HasValue)
						{
							einvoiceLine2.soluong = Math.Round(item2.ProdQuantity.Value, 0, MidpointRounding.AwayFromZero).ToString() ?? "";
						}
						if (item2.ProdPrice.HasValue)
						{
							einvoiceLine2.dongia = Math.Round(item2.ProdPrice.Value, 0, MidpointRounding.AwayFromZero).ToString() ?? "";
						}
						einvoiceLine2.tongtien = Math.Round(item2.Amount, 0, MidpointRounding.AwayFromZero).ToString() ?? "";
						einvoiceLine2.thuettdb = "0";
						einvoiceLine2.tienthue = Math.Round(item2.TaxAmount.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero).ToString() ?? "";
						einvoiceLine2.thanhtien = Math.Round(item2.AmountWithoutTax.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero).ToString() ?? "";
						if (!item2.TaxPercentage.HasValue)
						{
							einvoiceLine2.thuesuat = "KCT";
						}
						else if (item2.TaxPercentage == 1)
						{
							einvoiceLine2.thuesuat = "5";
						}
						else if (item2.TaxPercentage == 2)
						{
							einvoiceLine2.thuesuat = "10";
						}
						else if (item2.TaxPercentage == 3)
						{
							einvoiceLine2.thuesuat = "8";
						}
						else if (item2.TaxPercentage == 0)
						{
							einvoiceLine2.thuesuat = "0";
						}
						einvoiceLine2.tinhchat = 1;
						list.Add(einvoiceLine2);
						num2++;
					}
				}
			}
			catch (Exception ex)
			{
				list = new List<EinvoiceLine>();
				LogSystem.Error(ex);
			}
			return list;
		}

		private string GetDate(long time)
		{
			string result = "";
			try
			{
				string text = time.ToString();
				if (text != null && text.Length >= 8)
				{
					result = new StringBuilder().Append(text.Substring(0, 4)).Append("-").Append(text.Substring(4, 2))
						.Append("-")
						.Append(text.Substring(6, 2))
						.ToString();
				}
			}
			catch (Exception ex)
			{
				result = "";
				LogSystem.Error(ex);
			}
			return result;
		}

		private bool Check(ref ElectronicBillResult electronicBillResult)
		{
			bool result = true;
			try
			{
				string[] array = serviceConfigStr.Split('|');
				if (array.Length != 3)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống");
				}
				if (array[0] != "SAFECERT")
				{
					throw new Exception("Không đúng cấu hình nhà cung cấp safecert");
				}
				if (string.IsNullOrEmpty(array[1]))
				{
					throw new Exception("Chưa khai báo địa chỉ phát hành");
				}
				string[] array2 = accountConfigStr.Split('|');
				if (array2.Length != 2 || array2.Any((string o) => string.IsNullOrWhiteSpace(o)))
				{
					throw new Exception("Sai thông tin cấu hình tài khoản phát hành hóa đơn");
				}
				if (ElectronicBillDataInput.Branch == null || string.IsNullOrWhiteSpace(ElectronicBillDataInput.Branch.TAX_CODE))
				{
					throw new Exception("Chưa khai báo thông tin mã số thuế của viện");
				}
				string text = array[1].Replace("<#USER;>", array2[0]).Replace("<#PASS;>", array2[1]).Replace("<#TAX_CODE;>", ElectronicBillDataInput.Branch.TAX_CODE);
				Uri result2;
				if (!Uri.TryCreate(text, UriKind.Absolute, out result2))
				{
					throw new Exception("Sai định dạng cấu hình hệ thống địa chỉ phát hành");
				}
				string leftPart = result2.GetLeftPart(UriPartial.Authority);
				if (string.IsNullOrEmpty(leftPart))
				{
					throw new Exception("Không tìm thấy địa chỉ Webservice URL");
				}
				serviceConfig.BaseUrl = leftPart;
				serviceConfig.RequestUrl = text.Substring(leftPart.Length);
				serviceConfig.KeyTrans = array[2];
				if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
				{
					List<V_HIS_TRANSACTION> list = ElectronicBillDataInput.ListTransaction.Where((V_HIS_TRANSACTION o) => string.IsNullOrWhiteSpace(o.SYMBOL_CODE) || string.IsNullOrWhiteSpace(o.TEMPLATE_CODE)).ToList();
					if (list != null && list.Count > 0)
					{
						throw new Exception(string.Format("Các sổ {0} chưa có thông tin mẫu số, ký hiệu", string.Join(", ", list.Select((V_HIS_TRANSACTION s) => s.ACCOUNT_BOOK_NAME).Distinct().ToList())));
					}
					List<V_HIS_TRANSACTION> list2 = ElectronicBillDataInput.ListTransaction.Where((V_HIS_TRANSACTION o) => o.TEMPLATE_CODE != ElectronicBillDataInput.TemplateCode).ToList();
					if (list2 != null && list2.Count > 0)
					{
						string text2 = "";
						List<string> list3 = new List<string>();
						foreach (V_HIS_TRANSACTION item in list2)
						{
							list3.Add(string.Format("{0}({1})", item.ACCOUNT_BOOK_NAME, item.TEMPLATE_CODE));
						}
						text2 = string.Join(", ", list3.Distinct().ToList());
						throw new Exception(string.Format("Các sổ {0} có thông tin mẫu số khác nhau. Vui lòng kiểm tra lại.", text2));
					}
					List<V_HIS_TRANSACTION> list4 = ElectronicBillDataInput.ListTransaction.Where((V_HIS_TRANSACTION o) => o.SYMBOL_CODE != ElectronicBillDataInput.SymbolCode).ToList();
					if (list4 != null && list4.Count > 0)
					{
						string text3 = "";
						List<string> list5 = new List<string>();
						foreach (V_HIS_TRANSACTION item2 in list4)
						{
							list5.Add(string.Format("{0}({1})", item2.ACCOUNT_BOOK_NAME, item2.SYMBOL_CODE));
						}
						text3 = string.Join(", ", list5.Distinct().ToList());
						throw new Exception(string.Format("Các sổ {0} có thông tin ký hiệu khác nhau. Vui lòng kiểm tra lại.", text3));
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
