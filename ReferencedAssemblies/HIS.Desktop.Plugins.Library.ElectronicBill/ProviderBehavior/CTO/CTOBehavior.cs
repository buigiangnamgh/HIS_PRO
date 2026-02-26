using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.CTO.Model;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Number;
using Inventec.Common.String;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.CTO
{
	internal class CTOBehavior : IRun
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass14_0
		{
			public CTOBehavior _003C_003E4__this;

			public EBill data;

			public ResponeData apiresult;

			internal bool _003CPrcessCreateInvoice_003Eb__5(HIS_PAY_FORM o)
			{
				return o.ID == _003C_003E4__this.ElectronicBillDataInput.Transaction.PAY_FORM_ID;
			}
		}

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		private string proxyUrl { get; set; }

		public CTOBehavior(ElectronicBillDataInput _electronicBillDataInput, string _config)
		{
			ElectronicBillDataInput = _electronicBillDataInput;
			if (string.IsNullOrWhiteSpace(_config))
			{
				return;
			}
			string[] array = _config.Split('|');
			if (array.Count() > 1)
			{
				proxyUrl = "";
				for (int i = 1; i < array.Length; i++)
				{
					proxyUrl += array[i];
				}
			}
		}

		ElectronicBillResult IRun.Run(ElectronicBillType.ENUM _electronicBillTypeEnum, TemplateEnum.TYPE templateType)
		{
			ElectronicBillResult result = new ElectronicBillResult();
			try
			{
				if (Check(_electronicBillTypeEnum, ref result))
				{
					switch (_electronicBillTypeEnum)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						result = PrcessCreateInvoice(templateType);
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
						result = ProcessGetInvoice();
						break;
					case ElectronicBillType.ENUM.DELETE_INVOICE:
					case ElectronicBillType.ENUM.CANCEL_INVOICE:
						result = ProcessCancelInvoice();
						break;
					}
				}
			}
			catch (Exception ex)
			{
				ElectronicBillResultUtil.Set(ref result, false, ex.Message);
				LogSystem.Error(ex);
			}
			return result;
		}

		private bool Check(ElectronicBillType.ENUM _electronicBillTypeEnum, ref ElectronicBillResult result)
		{
			bool result2 = true;
			try
			{
				if (ElectronicBillDataInput == null)
				{
					throw new Exception("Không có dữ liệu phát hành");
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
					if (ElectronicBillDataInput.Transaction == null && (ElectronicBillDataInput.ListTransaction == null || ElectronicBillDataInput.ListTransaction.Count == 0))
					{
						throw new Exception("Không có dữ liệu hóa đơn");
					}
					if (ElectronicBillDataInput.Transaction == null && ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
					{
						throw new Exception("Chưa hỗ trợ xuất hóa đơn gộp");
					}
					if (ElectronicBillDataInput.Branch == null)
					{
						throw new Exception("Không có thông tin chi nhánh.");
					}
				}
			}
			catch (Exception ex)
			{
				result2 = false;
				ElectronicBillResultUtil.Set(ref result, false, ex.Message);
				LogSystem.Warn(ex);
			}
			return result2;
		}

		private ElectronicBillResult ProcessCancelInvoice()
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			CancelTransactionInvoiceRequest data = new CancelTransactionInvoiceRequest();
			data.additionalReferenceDate = ElectronicBillDataInput.CancelTime.GetValueOrDefault().ToString();
			data.additionalReferenceDesc = ElectronicBillDataInput.CancelReason;
			data.id = ElectronicBillDataInput.InvoiceCode;
			data.invoiceNo = ElectronicBillDataInput.ENumOrder;
			data.strIssueDate = ElectronicBillDataInput.TransactionTime.ToString();
			data.supplierTaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
			data.templateCode = ElectronicBillDataInput.TemplateCode;
			ResponeData responeData = ApiConsumer.CreateRequest<ResponeData>(proxyUrl, "/eb_cancel_invoice", data);
			if (responeData != null)
			{
				if (responeData.status == "success" && responeData.value != null)
				{
					string text = ProcessPdfFileResult((byte[])responeData.value);
					electronicBillResult.Success = true;
					electronicBillResult.InvoiceSys = "CTO_PROXY";
					electronicBillResult.Messages = new List<string> { responeData.message };
				}
				else
				{
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, string.Format("{0}. {1}", responeData.code, responeData.message));
					LogSystem.Error("Lay file hoa don that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<CancelTransactionInvoiceRequest>((Expression<Func<CancelTransactionInvoiceRequest>>)(() => data)), (object)data));
				}
			}
			else
			{
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không nhận được phản hồi từ hệ thống proxy");
				LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<CancelTransactionInvoiceRequest>((Expression<Func<CancelTransactionInvoiceRequest>>)(() => data)), (object)data));
			}
			return electronicBillResult;
		}

		private ElectronicBillResult ProcessGetInvoice()
		{
			LogSystem.Debug("CTOBehavior___ProcessGetInvoice()");
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			FileRequest data = new FileRequest();
			data.fileType = "PDF";
			data.id = ElectronicBillDataInput.InvoiceCode;
			data.invoiceNo = ElectronicBillDataInput.ENumOrder;
			data.supplierTaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
			data.templateCode = ElectronicBillDataInput.TemplateCode;
			ResponeData responeData = ApiConsumer.CreateRequest<ResponeData>(proxyUrl, "/eb_get_invoice_file", data);
			if (responeData != null)
			{
				if (responeData.status == "success" && responeData.value != null)
				{
					object value = responeData.value;
					LogSystem.Info("value: " + ((value != null) ? value.ToString() : null));
					string invoiceLink = ProcessPdfFileResult(System.Convert.FromBase64String(responeData.value.ToString()));
					electronicBillResult.Success = true;
					electronicBillResult.InvoiceSys = "CTO_PROXY";
					electronicBillResult.InvoiceLink = invoiceLink;
				}
				else
				{
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, string.Format("{0}. {1}", responeData.code, responeData.message));
					LogSystem.Error("Lay file hoa don that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<FileRequest>((Expression<Func<FileRequest>>)(() => data)), (object)data));
				}
			}
			else
			{
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không nhận được phản hồi từ hệ thống proxy");
				LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<FileRequest>((Expression<Func<FileRequest>>)(() => data)), (object)data));
			}
			return electronicBillResult;
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

		private ElectronicBillResult PrcessCreateInvoice(TemplateEnum.TYPE templateType)
		{
			_003C_003Ec__DisplayClass14_0 CS_0024_003C_003E8__locals61 = new _003C_003Ec__DisplayClass14_0();
			CS_0024_003C_003E8__locals61._003C_003E4__this = this;
			LogSystem.Debug("CTOBehavior___PrcessCreateInvoice()");
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			DateTime now = DateTime.Now;
			CS_0024_003C_003E8__locals61.data = new EBill();
			CS_0024_003C_003E8__locals61.data.currency = ElectronicBillDataInput.Currency;
			CS_0024_003C_003E8__locals61.data.issued_date = now.ToString("yyyy-MM-ddTHH:mm:sszzz");
			if (ElectronicBillDataInput.Treatment != null)
			{
				CS_0024_003C_003E8__locals61.data.patient_id = ElectronicBillDataInput.Treatment.TDL_PATIENT_CODE;
				CS_0024_003C_003E8__locals61.data.encounter = ElectronicBillDataInput.Treatment.TREATMENT_CODE;
			}
			string method = "";
			string creator = "";
			string id = "";
			if (ElectronicBillDataInput.Transaction != null)
			{
				HIS_PAY_FORM val = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == CS_0024_003C_003E8__locals61._003C_003E4__this.ElectronicBillDataInput.Transaction.PAY_FORM_ID);
				method = ((val != null) ? val.PAY_FORM_CODE : " ");
				creator = ElectronicBillDataInput.Transaction.CASHIER_USERNAME;
				id = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
			}
			CS_0024_003C_003E8__locals61.data.id = id;
			CS_0024_003C_003E8__locals61.data.creator = creator;
			CS_0024_003C_003E8__locals61.data.method = method;
			CS_0024_003C_003E8__locals61.data.status = "issued";
			int num = -1;
			if (ElectronicBillDataInput.Transaction != null)
			{
				num = ElectronicBillDataInput.Transaction.BUYER_TYPE ?? (-1);
			}
			else if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
			{
				V_HIS_TRANSACTION val2 = (from o in ElectronicBillDataInput.ListTransaction
					where o.BUYER_TYPE.HasValue
					orderby o.TRANSACTION_TIME descending
					select o).FirstOrDefault();
				if (val2 != null)
				{
					num = val2.BUYER_TYPE ?? (-1);
				}
			}
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = " ";
			string text6 = " ";
			InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput, false);
			text = data.BuyerAddress ?? " ";
			text2 = data.BuyerName;
			text3 = data.BuyerTaxCode ?? " ";
			text4 = data.BuyerOrganization ?? " ";
			if (num == 2)
			{
				text5 = data.BuyerAddress ?? " ";
				text6 = data.BuyerTaxCode ?? " ";
			}
			LogSystem.Debug("__buyerType: " + num);
			switch (num)
			{
			case 1:
				CS_0024_003C_003E8__locals61.data.buyer = new EbInfo();
				CS_0024_003C_003E8__locals61.data.buyer.address = text;
				CS_0024_003C_003E8__locals61.data.buyer.name = text2;
				CS_0024_003C_003E8__locals61.data.buyer.tax = text3;
				CS_0024_003C_003E8__locals61.data.company = null;
				break;
			case 2:
				CS_0024_003C_003E8__locals61.data.buyer = new EbInfo();
				CS_0024_003C_003E8__locals61.data.buyer.address = " ";
				CS_0024_003C_003E8__locals61.data.buyer.tax = " ";
				CS_0024_003C_003E8__locals61.data.buyer.name = text2;
				CS_0024_003C_003E8__locals61.data.company = new EbInfo();
				CS_0024_003C_003E8__locals61.data.company.name = text4;
				CS_0024_003C_003E8__locals61.data.company.address = text5;
				CS_0024_003C_003E8__locals61.data.company.tax = text6;
				break;
			default:
				CS_0024_003C_003E8__locals61.data.buyer = new EbInfo();
				CS_0024_003C_003E8__locals61.data.buyer.address = text;
				CS_0024_003C_003E8__locals61.data.buyer.name = text2;
				CS_0024_003C_003E8__locals61.data.buyer.tax = text3;
				if (!string.IsNullOrWhiteSpace(text4) || !string.IsNullOrWhiteSpace(text5) || !string.IsNullOrWhiteSpace(text6))
				{
					CS_0024_003C_003E8__locals61.data.company = new EbInfo();
					CS_0024_003C_003E8__locals61.data.company.name = text4;
					CS_0024_003C_003E8__locals61.data.company.address = text5;
					CS_0024_003C_003E8__locals61.data.company.tax = text6;
				}
				break;
			}
			CS_0024_003C_003E8__locals61.data.template = new EbTemplate();
			CS_0024_003C_003E8__locals61.data.template.id = ElectronicBillDataInput.TemplateCode;
			CS_0024_003C_003E8__locals61.data.template.symbol = ElectronicBillDataInput.SymbolCode;
			CS_0024_003C_003E8__locals61.data.template.type = GetInvoiceType(ElectronicBillDataInput.TemplateCode);
			CS_0024_003C_003E8__locals61.data.items = new List<EbProduct>();
			IRunTemplate runTemplate = TemplateFactory.MakeIRun(templateType, ElectronicBillDataInput);
			object obj = runTemplate.Run();
			if (obj == null)
			{
				throw new Exception("Lỗi thông tin dịch vụ");
			}
			if (obj.GetType() == typeof(List<ProductBase>))
			{
				List<ProductBase> list = (List<ProductBase>)obj;
				if (list == null || list.Count == 0)
				{
					throw new Exception("Lỗi thông tin dịch vụ");
				}
				foreach (ProductBase item in list)
				{
					EbProduct ebProduct = new EbProduct();
					ebProduct.name = item.ProdName;
					ebProduct.quantity = (long)item.ProdQuantity.GetValueOrDefault();
					ebProduct.tax = 0;
					ebProduct.unit = item.ProdUnit;
					ebProduct.amount = item.Amount;
					ebProduct.total = item.Amount;
					ebProduct.price = item.ProdPrice.GetValueOrDefault();
					CS_0024_003C_003E8__locals61.data.items.Add(ebProduct);
				}
			}
			else if (obj.GetType() == typeof(List<ProductBasePlus>))
			{
				List<ProductBasePlus> list2 = (List<ProductBasePlus>)obj;
				if (list2 == null || list2.Count == 0)
				{
					throw new Exception("Lỗi thông tin dịch vụ");
				}
				foreach (ProductBasePlus item2 in list2)
				{
					EbProduct ebProduct2 = new EbProduct();
					ebProduct2.name = item2.ProdName;
					ebProduct2.quantity = Math.Round((double)item2.ProdQuantity.GetValueOrDefault(), 4);
					ebProduct2.tax = 0;
					ebProduct2.unit = item2.ProdUnit;
					ebProduct2.amount = item2.Amount;
					ebProduct2.total = item2.Amount;
					ebProduct2.price = item2.ProdPrice.GetValueOrDefault();
					CS_0024_003C_003E8__locals61.data.items.Add(ebProduct2);
				}
			}
			CS_0024_003C_003E8__locals61.data.total_gross = CS_0024_003C_003E8__locals61.data.items.Sum((EbProduct s) => s.total);
			CS_0024_003C_003E8__locals61.data.total_net = CS_0024_003C_003E8__locals61.data.items.Sum((EbProduct s) => s.amount);
			CS_0024_003C_003E8__locals61.data.amount_inwords = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", Inventec.Common.Number.Convert.NumberToNumberRoundMax4(CS_0024_003C_003E8__locals61.data.total_gross))) + "đồng";
			CS_0024_003C_003E8__locals61.data.vat = new EbVat();
			CS_0024_003C_003E8__locals61.apiresult = ApiConsumer.CreateRequest<ResponeData>(proxyUrl, "/eb_publish_invoice", CS_0024_003C_003E8__locals61.data);
			if (CS_0024_003C_003E8__locals61.apiresult != null)
			{
				if (CS_0024_003C_003E8__locals61.apiresult.status == "success")
				{
					electronicBillResult.Success = true;
					electronicBillResult.InvoiceSys = "CTO_PROXY";
					electronicBillResult.InvoiceCode = CS_0024_003C_003E8__locals61.data.id;
					electronicBillResult.InvoiceNumOrder = ((CS_0024_003C_003E8__locals61.apiresult.value != null) ? CS_0024_003C_003E8__locals61.apiresult.value.ToString() : "");
					electronicBillResult.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)now);
				}
				else
				{
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, string.Format("{0}. {1}", CS_0024_003C_003E8__locals61.apiresult.code, CS_0024_003C_003E8__locals61.apiresult.message));

                    Inventec.Common.Logging.LogSystem.Error("tao hoa doi dien tu that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals61.data), CS_0024_003C_003E8__locals61.data) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals61.apiresult), CS_0024_003C_003E8__locals61.apiresult));
				}
			}
			else
			{
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không nhận được phản hồi từ hệ thống proxy");
				LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<EBill>((Expression<Func<EBill>>)(() => CS_0024_003C_003E8__locals61.data)), (object)CS_0024_003C_003E8__locals61.data));
			}
			return electronicBillResult;
		}

		private string GetInvoiceType(string p)
		{
			string result = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(p))
				{
					switch (p.Substring(0, 2))
					{
					case "01":
					{
						string text = p.Substring(0, 5);
						result = ((!(text == "01BLP")) ? "01GTKT" : "01BLP");
						break;
					}
					case "02":
						result = "02GTTT";
						break;
					case "03":
						result = "03XKNB";
						break;
					case "04":
						result = "04HGDL";
						break;
					case "07":
						result = "07KPTQ";
						break;
					}
				}
			}
			catch (Exception ex)
			{
				result = "";
				LogSystem.Error(ex);
			}
			return result;
		}
	}
}
