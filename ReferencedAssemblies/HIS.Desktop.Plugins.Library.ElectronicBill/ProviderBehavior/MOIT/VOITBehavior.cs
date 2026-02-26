using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.DateTime;
using Inventec.Common.ElectronicBillMoit;
using Inventec.Common.ElectronicBillMoit.ModelXml;
using Inventec.Common.Logging;
using Inventec.Common.Number;
using Inventec.Common.String;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MOIT
{
	public class VOITBehavior : IRun
	{
		private int convert;

		private string Version = "";

		private string serviceConfig { get; set; }

		private string accountConfig { get; set; }

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		private TemplateEnum.TYPE TempType { get; set; }

		public VOITBehavior(ElectronicBillDataInput _electronicBillDataInput, string _serviceConfig, string _accountConfig)
		{
			serviceConfig = _serviceConfig;
			ElectronicBillDataInput = _electronicBillDataInput;
			accountConfig = _accountConfig;
		}

		ElectronicBillResult IRun.Run(ElectronicBillType.ENUM _electronicBillTypeEnum, TemplateEnum.TYPE _tempType)
		{
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			try
			{
				if (Check(ref electronicBillResult))
				{
					TempType = _tempType;
					string[] array = serviceConfig.Split('|');
					string text = array[1];
					if (string.IsNullOrEmpty(text))
					{
						LogSystem.Error("Khong tim thay dia chi Webservice URL");
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không tìm thấy địa chỉ Webservice URL");
						return electronicBillResult;
					}
					convert = int.Parse(array[2].Trim());
					if (array.Count() > 3)
					{
						Version = array[3];
					}
					string[] array2 = accountConfig.Split('|');
					DataInitApi val = new DataInitApi();
					val.Address = text;
					val.User = array2[0].Trim();
					val.Pass = array2[1].Trim();
					val.SupplierTaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
					switch (_electronicBillTypeEnum)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						ProcessCreateInvoice(val, ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
						ProcessGetInvoice(val, ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.DELETE_INVOICE:
					case ElectronicBillType.ENUM.CANCEL_INVOICE:
						ProcessCancelInvoice(val, ref electronicBillResult);
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

		private bool Check(ref ElectronicBillResult electronicBillResult)
		{
			bool result = true;
			try
			{
				string[] array = serviceConfig.Split('|');
				if (array.Length < 3)
				{
					throw new Exception("Sai định dạng cấu hình .");
				}
				if (array[0] != "MOIT")
				{
					throw new Exception("Khong dung cau hinh nha cung cap Bo Cong Thuong");
				}
				string[] array2 = accountConfig.Split('|');
				if (array2.Length != 2)
				{
					throw new Exception("Sai định dạng cấu hình tai khoan .");
				}
			}
			catch (Exception ex)
			{
				result = false;
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, "");
				LogSystem.Warn(ex);
			}
			return result;
		}

		private void ProcessCancelInvoice(DataInitApi login, ref ElectronicBillResult result)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			try
			{
				InvoiceResult response = null;
				BillMoitManager val = new BillMoitManager(login);
				if (val != null)
				{
					if (Version.Trim() == "2")
					{
						response = val.CancelInvoiceTT78(ElectronicBillDataInput.TemplateCode, ElectronicBillDataInput.SymbolCode, ElectronicBillDataInput.InvoiceCode);
					}
					else
					{
						response = val.CancelInvoice(ElectronicBillDataInput.TemplateCode, ElectronicBillDataInput.InvoiceCode);
					}
				}
				if (response != null && response.Status)
				{
					result.Success = true;
					result.InvoiceSys = "MOIT";
					result.Messages = new List<string> { response.MessLog };
					return;
				}
				result.Success = false;
				result.InvoiceSys = "MOIT";
				result.Messages = new List<string> { response.MessLog };
                Inventec.Common.Logging.LogSystem.Error("Huy hoa don dien tu that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => response), response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ElectronicBillDataInput), ElectronicBillDataInput));
				ElectronicBillResultUtil.Set(ref result, false, response.MessLog);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ProcessGetInvoice(DataInitApi login, ref ElectronicBillResult result)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			try
			{
				InvoiceResult response = null;
				BillMoitManager val = new BillMoitManager(login);
				if (val != null)
				{
					if (Version.Trim() == "2")
					{
						response = val.GetFileInvoiceTT78(ElectronicBillDataInput.TemplateCode, ElectronicBillDataInput.SymbolCode, ElectronicBillDataInput.InvoiceCode);
					}
					else
					{
						response = val.GetFileInvoice(ElectronicBillDataInput.TemplateCode, ElectronicBillDataInput.InvoiceCode);
					}
				}
				if (response != null && response.Status)
				{
					string invoiceLink = ProcessPdfFileResult(response.PdfFile);
					result.Success = true;
					result.InvoiceSys = "MOIT";
					result.Messages = new List<string> { response.MessLog };
					result.InvoiceLink = invoiceLink;
					return;
				}
				result.Success = false;
				result.InvoiceSys = "MOIT";
				result.Messages = new List<string> { response.MessLog };
                Inventec.Common.Logging.LogSystem.Error("lay file chuyen doi that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => response), response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ElectronicBillDataInput), ElectronicBillDataInput));
				ElectronicBillResultUtil.Set(ref result, false, response.MessLog);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private string ProcessPdfFileResult(string base64string)
		{
			string result = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(base64string))
				{
					byte[] fileToBytes = System.Convert.FromBase64String(base64string);
					result = ProcessPdfFileResult(fileToBytes);
				}
			}
			catch (Exception ex)
			{
				result = "";
				LogSystem.Error(ex);
			}
			return result;
		}

		private string ProcessPdfFileResult(byte[] fileToBytes)
		{
			string text = "";
			try
			{
				string tempFileName = Path.GetTempFileName();
				tempFileName = tempFileName.Replace("tmp", "pdf");
				try
				{
					File.WriteAllBytes(tempFileName, fileToBytes);
					text = tempFileName;
				}
				catch (Exception ex)
				{
					text = "";
					LogSystem.Error(ex);
				}
			}
			catch (Exception ex2)
			{
				text = "";
				LogSystem.Error(ex2);
			}
			return text;
		}

		private void ProcessCreateInvoice(DataInitApi login, ref ElectronicBillResult result)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			try
			{
				List<Invoice> invoiceContrVietSens = GetInvoiceContrVietSens(ElectronicBillDataInput);
				string[] array = serviceConfig.Split('|');
				InvoiceResult response = null;
				BillMoitManager val = new BillMoitManager(login);
				if (val != null)
				{
					if (Version.Trim() == "2")
					{
						response = val.CreateInvoiceTT78(invoiceContrVietSens, ElectronicBillDataInput.TemplateCode, ElectronicBillDataInput.SymbolCode, convert);
					}
					else
					{
						response = val.CreateInvoice(invoiceContrVietSens, ElectronicBillDataInput.TemplateCode, ElectronicBillDataInput.SymbolCode, convert);
					}
				}
				if (response != null && response.Status)
				{
					result.Success = true;
					result.InvoiceSys = "MOIT";
					result.Messages = new List<string> { response.MessLog };
					result.InvoiceCode = response.fKey;
					result.InvoiceNumOrder = response.NumOrder;
					result.InvoiceTime =Inventec.Common.DateTime.Get.Now();
					result.InvoiceLoginname = login.User;
					LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<InvoiceResult>((Expression<Func<InvoiceResult>>)(() => response)), (object)response));
				}
				else
				{
					result.Success = false;
					result.InvoiceSys = "MOIT";
					result.Messages = new List<string> { response.MessLog };
                    Inventec.Common.Logging.LogSystem.Error("tao hoa doi dien tu that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => response), response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ElectronicBillDataInput), ElectronicBillDataInput));
					ElectronicBillResultUtil.Set(ref result, false, response.MessLog);
				}
			}
			catch (Exception ex)
			{
				ElectronicBillResultUtil.Set(ref result, false, ex.Message);
				LogSystem.Error(ex);
			}
		}

		private List<Invoice> GetInvoiceContrVietSens(ElectronicBillDataInput electronicBillDataInput)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Expected O, but got Unknown
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Expected O, but got Unknown
			List<Invoice> list = new List<Invoice>();
			if (electronicBillDataInput != null)
			{
				Invoice val = new Invoice();
				string text = "";
				if (electronicBillDataInput.Transaction != null)
				{
					text = electronicBillDataInput.Transaction.TRANSACTION_CODE;
				}
				else if (electronicBillDataInput.ListTransaction != null && electronicBillDataInput.ListTransaction.Count > 0)
				{
					text = (from s in ElectronicBillDataInput.ListTransaction
						select s.TRANSACTION_CODE into o
						orderby o
						select o).FirstOrDefault();
				}
				else
				{
					string arg = ((electronicBillDataInput.TransactionTime > 0) ? electronicBillDataInput.TransactionTime.ToString() : Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now).ToString());
					text = string.Format("{0}-{1}", arg, Guid.NewGuid().ToString());
					if (text.Length > 20)
					{
						text = text.Substring(0, 20);
					}
				}
				val.Key = text;
				val.InvoiceDetail = new InvoiceDetail();
				string paymentMethod = electronicBillDataInput.PaymentMethod;
				if (electronicBillDataInput.Transaction != null)
				{
					HIS_PAY_FORM val2 = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == electronicBillDataInput.Transaction.PAY_FORM_ID);
					if (val2 != null)
					{
						paymentMethod = val2.ELECTRONIC_PAY_FORM_NAME ?? val2.PAY_FORM_NAME;
					}
				}
				val.InvoiceDetail.PaymentMethod = paymentMethod;
				InvoiceInfoADO data = InvoiceInfoProcessor.GetData(electronicBillDataInput);
				val.InvoiceDetail.CusCode = data.BuyerCode;
				val.InvoiceDetail.CusAddress = data.BuyerAddress ?? " ";
				val.InvoiceDetail.CusName = data.BuyerName;
				val.InvoiceDetail.CusTaxCode = data.BuyerTaxCode;
				val.InvoiceDetail.CusPhone = data.BuyerPhone;
				string buyerName = data.BuyerName;
				if (HisConfigCFG.IsSwapNameOption)
				{
					val.InvoiceDetail.Buyer = buyerName;
					val.InvoiceDetail.CusName = "";
				}
				else
				{
					val.InvoiceDetail.Buyer = "";
					val.InvoiceDetail.CusName = buyerName;
				}
				decimal totalAmount = default(decimal);
				val.InvoiceDetail.Products = GetProductElectronicBill(ref totalAmount);
				totalAmount = Math.Round(totalAmount - electronicBillDataInput.Discount.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero);
				val.InvoiceDetail.Total = string.Format("{0:0.####}", totalAmount);
				val.InvoiceDetail.Amount = string.Format("{0:0.####}", Inventec.Common.Number.Convert.NumberToNumberRoundMax4(totalAmount));
				val.InvoiceDetail.AmountInWords = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", Inventec.Common.Number.Convert.NumberToNumberRoundMax4(totalAmount))) + "đồng";
				if (electronicBillDataInput.Discount.HasValue && electronicBillDataInput.Discount.Value > 0m)
				{
					Product val3 = new Product();
					val3.ProdName = "Tiền chiết khấu";
					val3.ProdPrice = "0";
					val3.ProdQuantity = "0";
					val3.Amount = string.Format("{0:0.####}", electronicBillDataInput.Discount.Value);
					val3.Total = Math.Round(electronicBillDataInput.Discount.Value, 0).ToString();
					val3.VATAmount = "";
					val3.VATRate = "-1";
					val.InvoiceDetail.Products.Add(val3);
				}
				list.Add(val);
			}
			return list;
		}

		private List<Product> GetProductElectronicBill(ref decimal totalAmount)
		{
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			List<Product> list = new List<Product>();
			IRunTemplate runTemplate = TemplateFactory.MakeIRun(TempType, ElectronicBillDataInput);
			object obj = runTemplate.Run();
			if (obj == null)
			{
				throw new Exception("Không có thông tin chi tiết dịch vụ.");
			}
			if (TempType != TemplateEnum.TYPE.TemplateNhaThuoc)
			{
				List<ProductBase> list2 = (List<ProductBase>)obj;
				if (list2 == null || list2.Count == 0)
				{
					throw new Exception("Không có thông tin chi tiết dịch vụ.");
				}
				foreach (ProductBase item in list2)
				{
					Product val = new Product();
					val.ProdName = item.ProdName;
					val.ProdUnit = item.ProdUnit;
					val.ProdQuantity = item.ProdQuantity.GetValueOrDefault().ToString();
					val.Amount = string.Format("{0:0.####}", item.Amount);
					val.ProdPrice = string.Format("{0:0.####}", item.ProdPrice.GetValueOrDefault());
					totalAmount += item.Amount;
					list.Add(val);
				}
			}
			return list;
		}
	}
}
