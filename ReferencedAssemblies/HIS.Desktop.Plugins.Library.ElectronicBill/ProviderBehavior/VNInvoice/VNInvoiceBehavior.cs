using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VNInvoice.Model;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VNInvoice
{
	internal class VNInvoiceBehavior : IRun
	{
		private const int SUCCESS_CODE = 0;

		private InputLoginVNInvoice inputLogin = new InputLoginVNInvoice();

		private string serviceConfig { get; set; }

		private string accountConfig { get; set; }

		private TemplateEnum.TYPE TempType { get; set; }

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		public VNInvoiceBehavior(ElectronicBillDataInput electronicBillDataInput, string serviceConfig, string accountConfig)
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
					TempType = _templateType;
					string[] array = serviceConfig.Split('|');
					string text = array[1];
					if (string.IsNullOrEmpty(text))
					{
						LogSystem.Error("Khong tim thay dia chi Webservice URL");
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không tìm thấy địa chỉ Webservice URL");
						return electronicBillResult;
					}
					string[] array2 = accountConfig.Split('|');
					inputLogin.username = array2[0].Trim();
					inputLogin.password = array2[1].Trim();
					inputLogin.taxCode = ElectronicBillDataInput.Branch.TAX_CODE;
					OutputLoginVNInvoice outputLogin = new OutputLoginVNInvoice();
					outputLogin = ProcessLogin(text, inputLogin);
					if (outputLogin == null)
					{
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<OutputLoginVNInvoice>((Expression<Func<OutputLoginVNInvoice>>)(() => outputLogin)), (object)outputLogin));
						return electronicBillResult;
					}
					switch (electronicBillType)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue)
						{
							if (ElectronicBillDataInput.TemplateCode == "1")
							{
								ThayTheHoaDonGTGT(ref electronicBillResult, text, inputLogin, outputLogin, _templateType);
							}
							else if (ElectronicBillDataInput.TemplateCode == "2")
							{
								ThayTheHoaDonBH(ref electronicBillResult, text, inputLogin, outputLogin, _templateType);
							}
						}
						else if (array.Count() == 3 && array[2] == "1")
						{
							if (ElectronicBillDataInput.TemplateCode == "1")
							{
								TaoVaKyHoaDonDienTuGTGT(ref electronicBillResult, text, inputLogin, outputLogin, _templateType);
							}
							else if (ElectronicBillDataInput.TemplateCode == "2")
							{
								TaoVaKyHoaDonDienTuBH(ref electronicBillResult, text, inputLogin, outputLogin, _templateType);
							}
						}
						else if (ElectronicBillDataInput.TemplateCode == "1")
						{
							TaoHoaDonDienTuGTGT(ref electronicBillResult, text, inputLogin, outputLogin, _templateType);
						}
						else if (ElectronicBillDataInput.TemplateCode == "2")
						{
							TaoHoaDonDienTuBH(ref electronicBillResult, text, inputLogin, outputLogin, _templateType);
						}
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
						if (ElectronicBillDataInput.TemplateCode == "1")
						{
							TaiHoaDonChuyenDoiGTGT(ref electronicBillResult, text, inputLogin, outputLogin, _templateType);
						}
						else if (ElectronicBillDataInput.TemplateCode == "2")
						{
							TaiHoaDonChuyenDoiBH(ref electronicBillResult, text, inputLogin, outputLogin, _templateType);
						}
						break;
					case ElectronicBillType.ENUM.CANCEL_INVOICE:
						break;
					case ElectronicBillType.ENUM.CONVERT_INVOICE:
						break;
					case ElectronicBillType.ENUM.CREATE_INVOICE_DATA:
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_INFO:
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

		private InputCreateInvoice ICEBill()
		{
			InputCreateInvoice inputCreateInvoice = new InputCreateInvoice();
			try
			{
				inputCreateInvoice.TemplateNo = System.Convert.ToInt32(ElectronicBillDataInput.TemplateCode);
				inputCreateInvoice.SerialNo = ElectronicBillDataInput.SymbolCode;
				inputCreateInvoice.erpId = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
				inputCreateInvoice.creatorErp = inputLogin.username;
				inputCreateInvoice.transactionId = ElectronicBillDataInput.Transaction.ID.ToString();
				inputCreateInvoice.invoiceDate = DateTime.Now.ToString("yyyy-MM-dd");
				inputCreateInvoice.paymentMethod = ElectronicBillDataInput.PaymentMethod ?? "TM / CK";
				inputCreateInvoice.currency = "VND";
				inputCreateInvoice.exchangeRate = 1.0;
				inputCreateInvoice.totalAmount = 0m;
				inputCreateInvoice.totalVatAmount = 0m;
				inputCreateInvoice.totalPaymentAmount = 0m;
				inputCreateInvoice.buyerCode = ElectronicBillDataInput.Transaction.TDL_PATIENT_CODE;
				inputCreateInvoice.buyerEmail = ElectronicBillDataInput.Transaction.BUYER_EMAIL;
				inputCreateInvoice.buyerFullName = ElectronicBillDataInput.Transaction.BUYER_NAME;
				inputCreateInvoice.buyerAddressLine = ElectronicBillDataInput.Transaction.BUYER_ADDRESS;
				inputCreateInvoice.invoiceDetails = DSCT();
				List<InvoiceTaxBreakdown> invoiceTaxBreakdowns = new List<InvoiceTaxBreakdown>();
				inputCreateInvoice.invoiceTaxBreakdowns = invoiceTaxBreakdowns;
				inputCreateInvoice.invoiceSpecificProductExtras = new List<InvoiceSpecificProductExtra>();
				if (inputCreateInvoice.invoiceDetails != null && inputCreateInvoice.invoiceDetails.Count > 0)
				{
					inputCreateInvoice.totalAmount = inputCreateInvoice.invoiceDetails.Sum((VNInvoiceDetail o) => o.amount.GetValueOrDefault());
					inputCreateInvoice.totalPaymentAmount = inputCreateInvoice.invoiceDetails.Sum((VNInvoiceDetail o) => o.paymentAmount.GetValueOrDefault());
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return inputCreateInvoice;
		}

		private void TaoHoaDonDienTuBH(ref ElectronicBillResult result, string serviceUrl, InputLoginVNInvoice inputLogin, OutputLoginVNInvoice outputLogin, TemplateEnum.TYPE templateType)
		{
			try
			{
				List<InputCreateInvoice> list = new List<InputCreateInvoice> { ICEBill() };
				string sendJsonData = JsonConvert.SerializeObject((object)list);
				OutPutCreateInvoice outPutCreateInvoice = ApiConsumerV2.CreateRequest<OutPutCreateInvoice>("POST", serviceUrl, string.Format("/02gttt/create-batch?TemplateNo={0}&serialNo={1}", ICEBill().TemplateNo, ICEBill().SerialNo), outputLogin.accessToken, null, sendJsonData);
				result.InvoiceSys = "VNINVOICE";
				if (outPutCreateInvoice != null)
				{
					if (outPutCreateInvoice.code == 0)
					{
						result.Success = true;
						result.InvoiceCode = outPutCreateInvoice.data[0].transactionId;
						result.InvoiceLoginname = inputLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now);
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (outPutCreateInvoice.message != null) ? outPutCreateInvoice.message : "Tạo hóa đơn bán hàng thất bại");
					}
				}
				else if (outPutCreateInvoice == null || (outPutCreateInvoice != null && outPutCreateInvoice.errors != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (outPutCreateInvoice != null && outPutCreateInvoice.errors != null) ? outPutCreateInvoice.errors : "Tạo hóa đơn bán hàng thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void TaoHoaDonDienTuGTGT(ref ElectronicBillResult result, string serviceUrl, InputLoginVNInvoice inputLogin, OutputLoginVNInvoice outputLogin, TemplateEnum.TYPE templateType)
		{
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)ICEBill());
				OutPutCreateInvoice outPutCreateInvoice = ApiConsumerV2.CreateRequest<OutPutCreateInvoice>("POST", serviceUrl, string.Format("/01gtkt/create-batch?TemplateNo={0}&SerialNo={1}", ICEBill().TemplateNo, ICEBill().SerialNo), outputLogin.accessToken, null, sendJsonData);
				result.InvoiceSys = "VNINVOICE";
				if (outPutCreateInvoice != null && outPutCreateInvoice.data != null)
				{
					if (outPutCreateInvoice.code == 0)
					{
						result.Success = true;
						result.InvoiceCode = outPutCreateInvoice.data[0].transactionId;
						result.InvoiceLoginname = inputLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now);
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (outPutCreateInvoice.message != null) ? outPutCreateInvoice.message : "Tạo hóa đơn giá trị gia tăng thất bại");
					}
				}
				else if (outPutCreateInvoice == null || (outPutCreateInvoice != null && outPutCreateInvoice.errors != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (outPutCreateInvoice != null && outPutCreateInvoice.errors != null) ? outPutCreateInvoice.errors : "Tạo hóa đơn giá trị gia tăng thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private InputCreateAndSignInvoice IEBill()
		{
			InputCreateAndSignInvoice inputCreateAndSignInvoice = new InputCreateAndSignInvoice();
			try
			{
                inputCreateAndSignInvoice.TemplateNo = System.Convert.ToInt32(ElectronicBillDataInput.TemplateCode);
				inputCreateAndSignInvoice.SerialNo = ElectronicBillDataInput.SymbolCode;
				inputCreateAndSignInvoice.erpId = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
				inputCreateAndSignInvoice.creatorErp = inputLogin.username;
				inputCreateAndSignInvoice.transactionId = ElectronicBillDataInput.Transaction.ID.ToString();
				inputCreateAndSignInvoice.invoiceDate = DateTime.Now.ToString("yyyy-MM-dd");
				inputCreateAndSignInvoice.paymentMethod = ElectronicBillDataInput.PaymentMethod ?? "TM / CK";
				inputCreateAndSignInvoice.currency = "VND";
				inputCreateAndSignInvoice.exchangeRate = 1.0;
				inputCreateAndSignInvoice.totalAmount = 0m;
				inputCreateAndSignInvoice.totalVatAmount = 0m;
				inputCreateAndSignInvoice.totalPaymentAmount = 0m;
				inputCreateAndSignInvoice.buyerCode = ElectronicBillDataInput.Transaction.TDL_PATIENT_CODE;
				inputCreateAndSignInvoice.buyerEmail = ElectronicBillDataInput.Transaction.BUYER_EMAIL;
				inputCreateAndSignInvoice.buyerFullName = ElectronicBillDataInput.Transaction.BUYER_NAME;
				inputCreateAndSignInvoice.buyerAddressLine = ElectronicBillDataInput.Transaction.BUYER_ADDRESS;
				inputCreateAndSignInvoice.invoiceDetails = DSCT();
				List<InvoiceTaxBreakdown> invoiceTaxBreakdowns = new List<InvoiceTaxBreakdown>();
				inputCreateAndSignInvoice.invoiceTaxBreakdowns = invoiceTaxBreakdowns;
				if (inputCreateAndSignInvoice.invoiceDetails != null && inputCreateAndSignInvoice.invoiceDetails.Count > 0)
				{
					inputCreateAndSignInvoice.totalAmount = inputCreateAndSignInvoice.invoiceDetails.Sum((VNInvoiceDetail o) => o.amount.GetValueOrDefault());
					inputCreateAndSignInvoice.totalPaymentAmount = inputCreateAndSignInvoice.invoiceDetails.Sum((VNInvoiceDetail o) => o.paymentAmount.GetValueOrDefault());
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return inputCreateAndSignInvoice;
		}

		private void TaoVaKyHoaDonDienTuGTGT(ref ElectronicBillResult result, string serviceUrl, InputLoginVNInvoice inputLogin, OutputLoginVNInvoice outputLogin, TemplateEnum.TYPE templateType)
		{
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)IEBill());
				OutputCreateAndSignInvoice outputCreateAndSignInvoice = ApiConsumerV2.CreateRequest<OutputCreateAndSignInvoice>("POST", serviceUrl, string.Format("/01gtkt/create-batch-and-sign?TemplateNo={0}&serialNo={1}", IEBill().TemplateNo, IEBill().SerialNo), outputLogin.accessToken, null, sendJsonData);
				result.InvoiceSys = "VNINVOICE";
				if (outputCreateAndSignInvoice != null && outputCreateAndSignInvoice.data != null)
				{
					if (outputCreateAndSignInvoice.code == 0)
					{
						result.Success = true;
						result.InvoiceCode = outputCreateAndSignInvoice.data[0].transactionId;
						result.InvoiceLoginname = inputLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now);
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (outputCreateAndSignInvoice.message != null) ? outputCreateAndSignInvoice.message : "Tạo và ký hóa đơn giá trị gia tăng thất bại");
					}
				}
				else if (outputCreateAndSignInvoice == null || (outputCreateAndSignInvoice != null && outputCreateAndSignInvoice.errors != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (outputCreateAndSignInvoice != null && outputCreateAndSignInvoice.errors != null) ? outputCreateAndSignInvoice.errors : "Tạo và ký hóa đơn giá trị gia tăng thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void TaoVaKyHoaDonDienTuBH(ref ElectronicBillResult result, string serviceUrl, InputLoginVNInvoice inputLogin, OutputLoginVNInvoice outputLogin, TemplateEnum.TYPE templateType)
		{
			try
			{
				List<InputCreateAndSignInvoice> list = new List<InputCreateAndSignInvoice> { IEBill() };
				string sendJsonData = JsonConvert.SerializeObject((object)list);
				OutputCreateAndSignInvoice outputCreateAndSignInvoice = ApiConsumerV2.CreateRequest<OutputCreateAndSignInvoice>("POST", serviceUrl, string.Format("/02gttt/create-batch-and-sign?TemplateNo={0}&serialNo={1}", IEBill().TemplateNo, IEBill().SerialNo), outputLogin.accessToken, null, sendJsonData);
				result.InvoiceSys = "VNINVOICE";
				if (outputCreateAndSignInvoice != null)
				{
					if (outputCreateAndSignInvoice.code == 0)
					{
						result.Success = true;
						result.InvoiceCode = outputCreateAndSignInvoice.data[0].transactionId;
						result.InvoiceLoginname = inputLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now);
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (outputCreateAndSignInvoice.message != null) ? outputCreateAndSignInvoice.message : "Tạo và ký hóa đơn bán hàng thất bại");
					}
				}
				else if (outputCreateAndSignInvoice == null || (outputCreateAndSignInvoice != null && outputCreateAndSignInvoice.errors != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (outputCreateAndSignInvoice != null && outputCreateAndSignInvoice.errors != null) ? outputCreateAndSignInvoice.errors : "Tạo và ký hóa đơn bán hàng thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void TaiHoaDonChuyenDoiGTGT(ref ElectronicBillResult result, string serviceUrl, InputLoginVNInvoice inputLogin, OutputLoginVNInvoice outputLogin, TemplateEnum.TYPE templateType)
		{
			try
			{
				if (ElectronicBillDataInput != null && !string.IsNullOrEmpty(ElectronicBillDataInput.InvoiceCode))
				{
					string tRANSACTION_CODE = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
					OutputConvertInvoice outputConvertInvoice = ApiConsumerV2.CreateRequest<OutputConvertInvoice>("POST", serviceUrl, string.Format("/01gtkt/official/{0}", tRANSACTION_CODE), outputLogin.accessToken, "");
					result.InvoiceSys = "VNINVOICE";
					if (outputConvertInvoice != null && outputConvertInvoice.data != null && outputConvertInvoice.id != null)
					{
						result.Success = true;
						return;
					}
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, "Tính năng này chưa được hỗ trợ. Vui lòng lên website để thực hiện.");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void TaiHoaDonChuyenDoiBH(ref ElectronicBillResult result, string serviceUrl, InputLoginVNInvoice inputLogin, OutputLoginVNInvoice outputLogin, TemplateEnum.TYPE templateType)
		{
			try
			{
				if (ElectronicBillDataInput != null && !string.IsNullOrEmpty(ElectronicBillDataInput.InvoiceCode))
				{
					string tRANSACTION_CODE = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
					OutputConvertInvoice outputConvertInvoice = ApiConsumerV2.CreateRequest<OutputConvertInvoice>("POST", serviceUrl, string.Format("/02gttt/official/{0}", tRANSACTION_CODE), outputLogin.accessToken, "");
					result.InvoiceSys = "VNINVOICE";
					if (outputConvertInvoice != null && outputConvertInvoice.data != null && outputConvertInvoice.id != null)
					{
						result.Success = true;
						return;
					}
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, "Tính năng này chưa được hỗ trợ. Vui lòng lên website để thực hiện.");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private InputReplaceInvoice ReplaceInvoice()
		{
			InputReplaceInvoice inputReplaceInvoice = new InputReplaceInvoice();
			try
			{
				inputReplaceInvoice.erpIdReference = ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.ToString();
				inputReplaceInvoice.templateNo = System.Convert.ToInt32(ElectronicBillDataInput.TemplateCode);
				inputReplaceInvoice.serialNo = ElectronicBillDataInput.SymbolCode;
				inputReplaceInvoice.invoiceNo = ElectronicBillDataInput.NumOrder.ToString();
				inputReplaceInvoice.erpId = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
				inputReplaceInvoice.creatorErp = inputLogin.username;
				inputReplaceInvoice.invoiceDate = DateTime.Now.ToString("yyyy-MM-dd");
				inputReplaceInvoice.note = "";
				inputReplaceInvoice.paymentMethod = ElectronicBillDataInput.PaymentMethod ?? "TM / CK";
				inputReplaceInvoice.currency = "VND";
				inputReplaceInvoice.exchangeRate = 1.0;
				inputReplaceInvoice.totalAmount = 0m;
				inputReplaceInvoice.totalVatAmount = 0m;
				inputReplaceInvoice.totalPaymentAmount = 0m;
				inputReplaceInvoice.buyerCode = ElectronicBillDataInput.Transaction.TDL_PATIENT_CODE;
				inputReplaceInvoice.buyerEmail = ElectronicBillDataInput.Transaction.BUYER_EMAIL;
				inputReplaceInvoice.buyerFullName = ElectronicBillDataInput.Transaction.BUYER_NAME;
				inputReplaceInvoice.buyerAddressLine = ElectronicBillDataInput.Transaction.BUYER_ADDRESS;
				inputReplaceInvoice.invoiceDetails = DSCT();
				List<InvoiceTaxBreakdown> list = new List<InvoiceTaxBreakdown>();
				if (inputReplaceInvoice.invoiceDetails != null && inputReplaceInvoice.invoiceDetails.Count > 0)
				{
					inputReplaceInvoice.totalAmount = inputReplaceInvoice.invoiceDetails.Sum((VNInvoiceDetail o) => o.amount.GetValueOrDefault());
					inputReplaceInvoice.totalPaymentAmount = inputReplaceInvoice.invoiceDetails.Sum((VNInvoiceDetail o) => o.paymentAmount.GetValueOrDefault());
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return inputReplaceInvoice;
		}

		private List<VNInvoiceDetail> DSCT()
		{
			List<VNInvoiceDetail> list = new List<VNInvoiceDetail>();
			try
			{
				IRunTemplate runTemplate = TemplateFactory.MakeIRun(TempType, ElectronicBillDataInput);
				object obj = runTemplate.Run();
				if (obj == null)
				{
					throw new Exception("Loi phan tich listProductBase");
				}
				if (TempType == TemplateEnum.TYPE.TemplateNhaThuoc)
				{
					List<ProductBasePlus> list2 = (List<ProductBasePlus>)obj;
					int num = 1;
					foreach (ProductBasePlus item in list2)
					{
						VNInvoiceDetail vNInvoiceDetail = new VNInvoiceDetail();
						vNInvoiceDetail.index = num++;
						vNInvoiceDetail.productType = 1;
						vNInvoiceDetail.productCode = item.ProdCode;
						vNInvoiceDetail.productName = item.ProdName;
						vNInvoiceDetail.unitName = item.ProdUnit;
						if (item.ProdQuantity.HasValue)
						{
							vNInvoiceDetail.quantity = (double)Math.Round(item.ProdQuantity.Value, 0, MidpointRounding.AwayFromZero);
						}
						if (item.ProdPrice.HasValue)
						{
							vNInvoiceDetail.unitPrice = Math.Round(item.ProdPrice.Value, 0, MidpointRounding.AwayFromZero);
						}
						vNInvoiceDetail.discountPercentBeforeTax = 0.0;
						vNInvoiceDetail.discountAmountBeforeTax = 0m;
						vNInvoiceDetail.paymentAmount = item.Amount;
						vNInvoiceDetail.amount = item.AmountWithoutTax;
						vNInvoiceDetail.vatPercent = 0;
						vNInvoiceDetail.vatAmount = 0m;
						list.Add(vNInvoiceDetail);
					}
				}
				else
				{
					int num2 = 1;
					List<ProductBase> list3 = (List<ProductBase>)obj;
					foreach (ProductBase item2 in list3)
					{
						VNInvoiceDetail vNInvoiceDetail2 = new VNInvoiceDetail();
						vNInvoiceDetail2.index = num2++;
						vNInvoiceDetail2.productType = 1;
						vNInvoiceDetail2.productCode = item2.ProdCode;
						vNInvoiceDetail2.productName = item2.ProdName;
						vNInvoiceDetail2.unitName = item2.ProdUnit;
						vNInvoiceDetail2.quantity = (double)item2.ProdQuantity.Value;
						vNInvoiceDetail2.unitPrice = item2.ProdPrice;
						vNInvoiceDetail2.discountPercentBeforeTax = 0.0;
						vNInvoiceDetail2.discountAmountBeforeTax = 0m;
						vNInvoiceDetail2.amount = item2.Amount;
						vNInvoiceDetail2.paymentAmount = item2.Amount;
						vNInvoiceDetail2.vatPercent = 0;
						list.Add(vNInvoiceDetail2);
					}
				}
			}
			catch (Exception ex)
			{
				list = new List<VNInvoiceDetail>();
				LogSystem.Error(ex);
			}
			return list;
		}

		private void ThayTheHoaDonGTGT(ref ElectronicBillResult result, string serviceUrl, InputLoginVNInvoice inputLogin, OutputLoginVNInvoice outputLogin, TemplateEnum.TYPE templateType)
		{
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)ReplaceInvoice());
				OutputReplaceInvoice outputReplaceInvoice = ApiConsumerV2.CreateRequest<OutputReplaceInvoice>("POST", serviceUrl, "/01gtkt/replace", outputLogin.accessToken, null, sendJsonData);
				result.InvoiceSys = "VNINVOICE";
				if (outputReplaceInvoice != null && outputReplaceInvoice.data != null)
				{
					if (outputReplaceInvoice.code == 0)
					{
						result.Success = true;
						result.InvoiceCode = outputReplaceInvoice.data[0].erpId;
						result.InvoiceNumOrder = outputReplaceInvoice.data[0].invoiceNo.ToString();
					}
					else
					{
						result.Success = false;
						string message = ((outputReplaceInvoice != null) ? (outputReplaceInvoice.errors ?? outputReplaceInvoice.message) : "Thay thế hóa đơn GTGT thất bại");
						ElectronicBillResultUtil.Set(ref result, false, message);
					}
				}
				else if (outputReplaceInvoice == null || !string.IsNullOrWhiteSpace(outputReplaceInvoice.errors) || !string.IsNullOrWhiteSpace(outputReplaceInvoice.message))
				{
					result.Success = false;
					string message2 = ((outputReplaceInvoice != null) ? (outputReplaceInvoice.errors ?? outputReplaceInvoice.message) : "Thay thế hóa đơn GTGT thất bại");
					ElectronicBillResultUtil.Set(ref result, false, message2);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ThayTheHoaDonBH(ref ElectronicBillResult result, string serviceUrl, InputLoginVNInvoice inputLogin, OutputLoginVNInvoice outputLogin, TemplateEnum.TYPE templateType)
		{
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)ReplaceInvoice());
				OutputReplaceInvoice outputReplaceInvoice = ApiConsumerV2.CreateRequest<OutputReplaceInvoice>("POST", serviceUrl, "/02gttt/replace", outputLogin.accessToken, null, sendJsonData);
				result.InvoiceSys = "VNINVOICE";
				if (outputReplaceInvoice != null && outputReplaceInvoice.data != null)
				{
					if (outputReplaceInvoice.code == 0)
					{
						result.Success = true;
						result.InvoiceCode = outputReplaceInvoice.data[0].erpId;
						result.InvoiceNumOrder = outputReplaceInvoice.data[0].invoiceNo.ToString();
					}
					else
					{
						result.Success = false;
						string message = ((outputReplaceInvoice != null) ? (outputReplaceInvoice.errors ?? outputReplaceInvoice.message) : "Thay thế hóa đơn bán hàng thất bại");
						ElectronicBillResultUtil.Set(ref result, false, message);
					}
				}
				else if (outputReplaceInvoice == null || !string.IsNullOrWhiteSpace(outputReplaceInvoice.errors) || !string.IsNullOrWhiteSpace(outputReplaceInvoice.message))
				{
					result.Success = false;
					string message2 = ((outputReplaceInvoice != null) ? (outputReplaceInvoice.errors ?? outputReplaceInvoice.message) : "Thay thế hóa đơn bán hàng thất bại");
					ElectronicBillResultUtil.Set(ref result, false, message2);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private bool Check(ElectronicBillType.ENUM _electronicBillTypeEnum, ref ElectronicBillResult electronicBillResult)
		{
			bool result = true;
			try
			{
				string[] array = serviceConfig.Split('|');
				if (array[0] != "VNINVOICE")
				{
					throw new Exception("Không đúng cấu hình nhà cung cấp VN-invoice");
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

		private OutputLoginVNInvoice ProcessLogin(string serviceUrl, InputLoginVNInvoice inputLogin)
		{
			OutputLoginVNInvoice result = null;
			try
			{
				string requestUri = "/system/account/login";
				string sendJsonData = JsonConvert.SerializeObject((object)inputLogin);
				result = ApiConsumerV2.CreateRequest<OutputLoginVNInvoice>("POST", serviceUrl, requestUri, null, null, sendJsonData);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}
	}
}
