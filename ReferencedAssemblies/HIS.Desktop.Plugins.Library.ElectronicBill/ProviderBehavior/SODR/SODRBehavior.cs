using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR.Process;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.DateTime;
using Inventec.Common.EBillSoftDreams;
using Inventec.Common.EBillSoftDreams.Model;
using Inventec.Common.EBillSoftDreams.ModelXml;
using Inventec.Common.Logging;
using Inventec.Common.Number;
using Inventec.Common.String;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR
{
    internal class SODRBehavior : HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass32_0
		{
			public ResultDataV2 response;

			public ReplaceInvoiceV2 input;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass33_0
		{
			public ResultDataV2 response;

			public IssueCreateV2 invoice;
		}

		private string serviceConfig { get; set; }

		private string accountConfig { get; set; }

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		private TemplateEnum.TYPE TempType { get; set; }

		private SoftDreamsProcessor processV2 { get; set; }

		private bool IsV2 { get; set; }

		public SODRBehavior(ElectronicBillDataInput _electronicBillDataInput, string _serviceConfig, string _accountConfig, bool isV2 = false)
		{
			serviceConfig = _serviceConfig;
			ElectronicBillDataInput = _electronicBillDataInput;
			accountConfig = _accountConfig;
			IsV2 = isV2;
		}

        ElectronicBillResult HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun.Run(ElectronicBillType.ENUM _electronicBillTypeEnum, TemplateEnum.TYPE _tempType)
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
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
					string[] array2 = accountConfig.Split('|');
					DataInitApi val = new DataInitApi();
					val.Address = text;
					val.User = array2[0].Trim();
					val.Pass = array2[1].Trim();
					if (IsV2)
					{
						processV2 = new SoftDreamsProcessor(text, array2[0].Trim(), array2[1].Trim());
						if (processV2 != null)
						{
							switch (_electronicBillTypeEnum)
							{
							case ElectronicBillType.ENUM.CREATE_INVOICE:
								electronicBillResult = ((ElectronicBillDataInput.Transaction == null || !ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue) ? ProcessCreateInvoiceV2() : ProcessReplaceInvoiceV2());
								break;
							case ElectronicBillType.ENUM.GET_INVOICE_LINK:
								electronicBillResult = ProcessGetInvoiceV2();
								break;
							case ElectronicBillType.ENUM.DELETE_INVOICE:
							case ElectronicBillType.ENUM.CANCEL_INVOICE:
								electronicBillResult = ProcessCancelInvoiceV2();
								break;
							case ElectronicBillType.ENUM.REPLACE_INVOICE:
								break;
							case ElectronicBillType.ENUM.CONVERT_INVOICE:
							case ElectronicBillType.ENUM.CREATE_INVOICE_DATA:
							case ElectronicBillType.ENUM.GET_INVOICE_INFO:
							case ElectronicBillType.ENUM.GET_INVOICE_SHOW:
								break;
							}
						}
						else
						{
							electronicBillResult.InvoiceSys = "SODR";
							ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Lỗi khởi tạo");
						}
					}
					else
					{
						switch (_electronicBillTypeEnum)
						{
						case ElectronicBillType.ENUM.CREATE_INVOICE:
							ProcessCreateInvoice(val, ref electronicBillResult);
							break;
						case ElectronicBillType.ENUM.GET_INVOICE_LINK:
							break;
						case ElectronicBillType.ENUM.DELETE_INVOICE:
						case ElectronicBillType.ENUM.CANCEL_INVOICE:
							break;
						}
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
				if (array.Length != 2)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
				}
				string[] array2 = accountConfig.Split('|');
				if (array2.Length != 2)
				{
					throw new Exception("Sai định dạng cấu hình tài khoản.");
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

		private void ProcessCreateInvoice(DataInitApi login, ref ElectronicBillResult result)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			try
			{
				InvCreate invoices = GetInvoice(ElectronicBillDataInput);
				Response response = null;
				LogSystem.Info("SendData" + LogUtil.TraceData(LogUtil.GetMemberName<InvCreate>((Expression<Func<InvCreate>>)(() => invoices)), (object)invoices));
				EBillSoftDreamsManager val = new EBillSoftDreamsManager(login);
				if (val != null)
				{
					response = val.Run((object)invoices);
				}
				LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<Response>((Expression<Func<Response>>)(() => response)), (object)response));
				if (response != null && response.Success)
				{
					result.InvoiceCode = response.Ikey;
					result.InvoiceNumOrder = response.invoiceNo;
					result.InvoiceTime = Inventec.Common.DateTime.Get.Now();
					result.InvoiceLoginname = login.User;
					result.InvoiceLookupCode = response.LookupCode;
				}
				result.InvoiceSys = "SODR";
				ElectronicBillResultUtil.Set(ref result, response.Success, response.Messages);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private InvCreate GetInvoice(ElectronicBillDataInput electronicBillDataInput)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			InvCreate result = new InvCreate();
			try
			{
				if (electronicBillDataInput != null)
				{
					InvCreate val = new InvCreate();
					val.Pattern = electronicBillDataInput.TemplateCode;
					val.Serial = electronicBillDataInput.SymbolCode;
					val.Inv = GetDataInv(electronicBillDataInput);
					result = val;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private Inv GetDataInv(ElectronicBillDataInput electronicBillDataInput)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Expected O, but got Unknown
			Inv val = new Inv();
			try
			{
				Invoice val2 = new Invoice();
				if (electronicBillDataInput != null)
				{
					string text = "";
					if (ElectronicBillDataInput.Transaction != null)
					{
						text = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
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
						text = string.Format("{0}-{1}", arg, Guid.NewGuid().ToString("N"));
						if (text.Length > 20)
						{
							text = text.Substring(0, 20);
						}
					}
					val2.Ikey = text;
					val2.ArisingDate = Inventec.Common.DateTime.Convert.TimeNumberToDateString(electronicBillDataInput.TransactionTime);
					string paymentMethod = "T/M";
					if (electronicBillDataInput.Transaction != null)
					{
						if (electronicBillDataInput.Transaction.PAY_FORM_ID == 2)
						{
							paymentMethod = "C/K";
						}
						else if (electronicBillDataInput.Transaction.PAY_FORM_ID == 5)
						{
							paymentMethod = "TT/D";
						}
						else if (electronicBillDataInput.Transaction.PAY_FORM_ID == 3)
						{
							paymentMethod = "TM/CK";
						}
					}
					else if (electronicBillDataInput.ListTransaction != null && electronicBillDataInput.ListTransaction.Count > 0)
					{
						if (electronicBillDataInput.ListTransaction.First().PAY_FORM_ID == 2)
						{
							paymentMethod = "C/K";
						}
						else if (electronicBillDataInput.ListTransaction.First().PAY_FORM_ID == 5)
						{
							paymentMethod = "TT/D";
						}
						else if (electronicBillDataInput.ListTransaction.First().PAY_FORM_ID == 3)
						{
							paymentMethod = "TM/CK";
						}
					}
					val2.PaymentMethod = paymentMethod;
					InvoiceInfoADO data = InvoiceInfoProcessor.GetData(electronicBillDataInput);
					val2.CusCode = data.BuyerCode;
					val2.CusAddress = data.BuyerAddress ?? " ";
					val2.CusName = data.BuyerName;
					val2.CusTaxCode = data.BuyerTaxCode;
					val2.CusPhone = data.BuyerPhone;
					string buyerName = data.BuyerName;
					if (HisConfigCFG.IsSwapNameOption)
					{
						val2.Buyer = buyerName;
						val2.CusName = "";
					}
					else
					{
						val2.Buyer = "";
						val2.CusName = buyerName;
					}
					val2.Total = Math.Round(electronicBillDataInput.Amount.GetValueOrDefault(), 0);
					val2.VATRate = -1;
					val2.VATAmount = 0m;
					val2.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(electronicBillDataInput.Amount.GetValueOrDefault());
					val2.AmountInWords = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", val2.Total)) + "đồng";
					val2.Products = GetProductElectronicBill();
					if (electronicBillDataInput.Discount.HasValue)
					{
						Product val3 = new Product();
						val3.ProdName = "Chiết khấu";
						val3.ProdPrice -= (decimal?)electronicBillDataInput.Discount.Value;
						val3.Amount -= electronicBillDataInput.Discount.Value;
						val3.Total -= electronicBillDataInput.Discount.Value;
						val3.Extra = "{\"Pos\":\"\"}";
						val2.Products.Add(val3);
					}
				}
				val.Invoice = val2;
			}
			catch (Exception ex)
			{
				val = null;
				LogSystem.Error(ex);
			}
			return val;
		}

		private List<Product> GetProductElectronicBill()
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			List<Product> list = new List<Product>();
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
					if (list2 == null || list2.Count == 0)
					{
						throw new Exception("Loi phan tich listProductBase");
					}
					foreach (ProductBase item in list2)
					{
						Product val = new Product();
						val.ProdName = item.ProdName;
						val.ProdUnit = item.ProdUnit;
						val.ProdQuantity = item.ProdQuantity;
						val.Amount = item.Amount;
						val.Total = item.Amount;
						val.ProdPrice = item.ProdPrice;
						list.Add(val);
					}
				}
			}
			catch (Exception ex)
			{
				list = null;
				LogSystem.Error(ex);
			}
			return list;
		}

		private ReplaceInvoiceV2 GetDataReplace()
		{
			ReplaceInvoiceV2 replaceInvoiceV = new ReplaceInvoiceV2();
			try
			{
				IssueCreateV2 invoiceDetailV = GetInvoiceDetailV2();
				replaceInvoiceV.Pattern = invoiceDetailV.Pattern;
				replaceInvoiceV.Invoices = invoiceDetailV.Invoices;
				string tDL_ORIGINAL_EI_CODE = ElectronicBillDataInput.Transaction.TDL_ORIGINAL_EI_CODE;
				replaceInvoiceV.Ikey = ((tDL_ORIGINAL_EI_CODE != null) ? tDL_ORIGINAL_EI_CODE.ToString() : null);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				throw;
			}
			return replaceInvoiceV;
		}

		private ElectronicBillResult ProcessReplaceInvoiceV2()
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult
			{
				InvoiceSys = "SODR"
			};
			try
			{
				_003C_003Ec__DisplayClass32_0 CS_0024_003C_003E8__locals12 = new _003C_003Ec__DisplayClass32_0();
				CS_0024_003C_003E8__locals12.input = GetDataReplace();
				CS_0024_003C_003E8__locals12.response = processV2.ReplaceInvoice(CS_0024_003C_003E8__locals12.input);
				if (CS_0024_003C_003E8__locals12.response != null && CS_0024_003C_003E8__locals12.response.InvoiceResult != null && CS_0024_003C_003E8__locals12.response.InvoiceResult.Count == 1)
				{
					electronicBillResult.Success = true;
					electronicBillResult.InvoiceCode = CS_0024_003C_003E8__locals12.response.InvoiceResult.First().Ikey;
					electronicBillResult.InvoiceNumOrder = CS_0024_003C_003E8__locals12.response.InvoiceResult.First().No;
				}
				else
				{
                    Inventec.Common.Logging.LogSystem.Error("Thay the va phat hanh hoa don that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals12.response), CS_0024_003C_003E8__locals12.response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals12.input), CS_0024_003C_003E8__locals12.input));
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Thay thế và phát hành hóa thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, ex.Message);
			}
			return electronicBillResult;
		}

		private ElectronicBillResult ProcessCreateInvoiceV2()
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult
			{
				InvoiceSys = "SODR"
			};
			try
			{
				_003C_003Ec__DisplayClass33_0 CS_0024_003C_003E8__locals12 = new _003C_003Ec__DisplayClass33_0();
				string text = ElectronicBillDataInput.TemplateCode + ElectronicBillDataInput.SymbolCode;
				CS_0024_003C_003E8__locals12.invoice = GetInvoiceDetailV2();
				CS_0024_003C_003E8__locals12.response = processV2.CreateInvoice(CS_0024_003C_003E8__locals12.invoice);
				if (CS_0024_003C_003E8__locals12.response != null && CS_0024_003C_003E8__locals12.response.InvoiceResult != null && CS_0024_003C_003E8__locals12.response.InvoiceResult.Count == 1)
				{
					electronicBillResult.Success = true;
					electronicBillResult.InvoiceCode = CS_0024_003C_003E8__locals12.response.InvoiceResult.First().Ikey;
					electronicBillResult.InvoiceNumOrder = CS_0024_003C_003E8__locals12.response.InvoiceResult.First().No;
				}
				else
				{
					
                    Inventec.Common.Logging.LogSystem.Error("tao hoa doi dien tu that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals12.response), CS_0024_003C_003E8__locals12.response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals12.invoice), CS_0024_003C_003E8__locals12.invoice));
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Tạo và phát hành hóa thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, ex.Message);
			}
			return electronicBillResult;
		}

		private IssueCreateV2 GetInvoiceDetailV2()
		{
			IssueCreateV2 issueCreateV = new IssueCreateV2();
			try
			{
				if (ElectronicBillDataInput != null)
				{
					issueCreateV.Pattern = ElectronicBillDataInput.TemplateCode + ElectronicBillDataInput.SymbolCode;
					issueCreateV.Invoices = new List<InvoiceV2>();
					InvoiceV2 invoiceV = new InvoiceV2();
					InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput, TempType != TemplateEnum.TYPE.Template10);
					invoiceV.CusAddress = data.BuyerAddress;
					invoiceV.CusBankName = "";
					invoiceV.CusBankNo = data.BuyerAccountNumber;
					invoiceV.CusPhone = data.BuyerPhone;
					invoiceV.CusCode = data.BuyerCode;
					string text = data.BuyerName ?? data.BuyerOrganization;
					HisConfigCFG hisConfigCFG = new HisConfigCFG();
					if (HisConfigCFG.IsSwapNameOption)
					{
						invoiceV.BuyerName = text;
						invoiceV.CusName = "";
					}
					else
					{
						invoiceV.BuyerName = "";
						invoiceV.CusName = text;
					}
					invoiceV.CusTaxCode = data.BuyerTaxCode;
					invoiceV.Email = data.BuyerEmail;
					invoiceV.PaymentMethod = ElectronicBillDataInput.PaymentMethod;
					if (ElectronicBillDataInput.Transaction != null && !string.IsNullOrEmpty(ElectronicBillDataInput.Transaction.TRANSACTION_CODE))
					{
						invoiceV.Ikey = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
					}
					else if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
					{
						invoiceV.Ikey = (from s in ElectronicBillDataInput.ListTransaction
							select s.TRANSACTION_CODE into o
							orderby o
							select o).FirstOrDefault();
					}
					invoiceV.Products = new List<ProductV2>();
					IRunTemplate runTemplate = TemplateFactory.MakeIRun(TempType, ElectronicBillDataInput);
					object obj = runTemplate.Run();
					int num = 1;
					if (obj == null)
					{
						throw new Exception("Không có thông tin chi tiết dịch vụ.");
					}
					if (TempType > TemplateEnum.TYPE.TemplateNhaThuoc)
					{
						List<ProductBase> list = (List<ProductBase>)obj;
						if (list == null || list.Count == 0)
						{
							throw new Exception("Không có thông tin chi tiết dịch vụ.");
						}
						foreach (ProductBase item4 in list)
						{
							ProductV2 item = new ProductV2
							{
								No = (num.ToString() ?? ""),
								Feature = "HHDV",
								Code = item4.ProdCode,
								Name = item4.ProdName,
								Unit = item4.ProdUnit,
								Quantity = item4.ProdQuantity.GetValueOrDefault(),
								Price = item4.ProdPrice.GetValueOrDefault(),
								Total = item4.Amount,
								VATAmount = 0m,
								VATRate = -1f,
								Amount = item4.Amount
							};
							invoiceV.Products.Add(item);
							num++;
						}
					}
					else
					{
						List<ProductBasePlus> list2 = (List<ProductBasePlus>)obj;
						if (list2 == null || list2.Count == 0)
						{
							throw new Exception("Không có thông tin chi tiết dịch vụ.");
						}
						foreach (ProductBasePlus item5 in list2)
						{
							ProductV2 item2 = new ProductV2
							{
								No = (num.ToString() ?? ""),
								Feature = "HHDV",
								Code = item5.ProdCode,
								Name = item5.ProdName,
								Unit = item5.ProdUnit,
								Quantity = item5.ProdQuantity.GetValueOrDefault(),
								Price = item5.ProdPrice.GetValueOrDefault(),
								Total = item5.AmountWithoutTax.GetValueOrDefault(),
								VATAmount = item5.TaxAmount.GetValueOrDefault(),
								VATRate = (float)item5.TaxConvert,
								Amount = item5.Amount
							};
							invoiceV.Products.Add(item2);
							num++;
						}
					}
					if (ElectronicBillDataInput.Transaction != null)
					{
						decimal? eXEMPTION = ElectronicBillDataInput.Transaction.EXEMPTION;
						if (((eXEMPTION.GetValueOrDefault() > default(decimal)) & eXEMPTION.HasValue) && TempType != TemplateEnum.TYPE.Template10)
						{
							ProductV2 item3 = new ProductV2
							{
								No = (num.ToString() ?? ""),
								Feature = "CK",
								Code = "",
								Name = "Chiết khấu",
								Unit = "",
								Quantity = 0m,
								Price = 0m,
								Total = ElectronicBillDataInput.Transaction.EXEMPTION.GetValueOrDefault(),
								VATAmount = 0m,
								VATRate = -1f,
								Amount = ElectronicBillDataInput.Transaction.EXEMPTION.GetValueOrDefault(),
								Discount = 0m,
								DiscountAmount = 0m
							};
							invoiceV.Products.Add(item3);
						}
					}
					issueCreateV.Invoices.Add(invoiceV);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return issueCreateV;
		}

		private ElectronicBillResult ProcessCancelInvoiceV2()
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult
			{
				InvoiceSys = "SODR"
			};
			try
			{
				string pattern = ElectronicBillDataInput.TemplateCode + ElectronicBillDataInput.SymbolCode;
				bool response = processV2.DeleteInvoice(pattern, ElectronicBillDataInput.InvoiceCode);
				if (response)
				{
					electronicBillResult.Success = true;
				}
				else
				{
					
                    LogSystem.Error("Huy hoa don that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<bool>(() => response), response) + LogUtil.TraceData(LogUtil.GetMemberName<ElectronicBillDataInput>(() => ElectronicBillDataInput), ElectronicBillDataInput));
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Hủy hóa đơn thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, ex.Message);
			}
			return electronicBillResult;
		}

		private ElectronicBillResult ProcessGetInvoiceV2()
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult
			{
				InvoiceSys = "SODR"
			};
			try
			{
				string pattern = ElectronicBillDataInput.TemplateCode + ElectronicBillDataInput.SymbolCode;
				string response = processV2.GetInvoice(pattern, ElectronicBillDataInput.InvoiceCode);
				if (!string.IsNullOrWhiteSpace(response))
				{
					string invoiceLink = ElectronicBillResultUtil.ProcessPdfFileResult(response);
					electronicBillResult.Success = true;
					electronicBillResult.InvoiceLink = invoiceLink;
				}
				else
				{
                    Inventec.Common.Logging.LogSystem.Error("lay file chuyen doi that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => response), response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ElectronicBillDataInput), ElectronicBillDataInput));
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Lấy file chuyển đổi thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, ex.Message);
			}
			return electronicBillResult;
		}
	}
}
