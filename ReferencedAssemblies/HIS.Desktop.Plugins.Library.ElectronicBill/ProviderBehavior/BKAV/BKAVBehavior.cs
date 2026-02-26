using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.EHoaDon;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.BKAV
{
    public class BKAVBehavior : HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun
	{
		private string config { get; set; }

		private string accountConfig { get; set; }

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		private TemplateEnum.TYPE TempType { get; set; }

		private ElectronicBillType.ENUM ElectronicBillTypeEnum { get; set; }

		public BKAVBehavior(ElectronicBillDataInput _electronicBillDataInput, string _config, string _accountConfig)
		{
			config = _config;
			accountConfig = _accountConfig;
			ElectronicBillDataInput = _electronicBillDataInput;
		}

        ElectronicBillResult HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun.Run(ElectronicBillType.ENUM _electronicBillTypeEnum, TemplateEnum.TYPE _tempType)
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Expected O, but got Unknown
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Expected O, but got Unknown
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			try
			{
				TempType = _tempType;
				ElectronicBillTypeEnum = _electronicBillTypeEnum;
				if (Check(ref electronicBillResult))
				{
					string[] array = config.Split('|');
					string text = array[1];
					if (string.IsNullOrEmpty(text))
					{
						LogSystem.Error("Khong tim thay dia chi Webservice URL");
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không tìm thấy địa chỉ Webservice URL");
						return electronicBillResult;
					}
					string[] array2 = accountConfig.Split('|');
					BkavPartner val = new BkavPartner();
					val.BkavPartnerGUID = array2[0].Trim();
					val.BkavPartnerToken = array2[1].Trim();
					if (array2[2] != null)
					{
						uint result = 0u;
						uint.TryParse(array2[2].Trim(), out result);
						val.Mode = result;
					}
					List<InvoiceDataWS> invoiceDataWSs = GetInvoiceContrBKAV(ElectronicBillDataInput);
					string cmdTypeCFG = "";
					if (array.Length > 3)
					{
						cmdTypeCFG = array[3];
					}
					if (ElectronicBillTypeEnum == ElectronicBillType.ENUM.CREATE_INVOICE && invoiceDataWSs.SelectMany((InvoiceDataWS s) => s.ListInvoiceDetailsWS).Sum((InvoiceDetailsWS s) => s.Amount) <= 0.0)
					{
						LogSystem.Error("Lỗi dữ liệu không có thành tiền dịch vụ");
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<List<InvoiceDataWS>>((Expression<Func<List<InvoiceDataWS>>>)(() => invoiceDataWSs)), (object)invoiceDataWSs));
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Tổng tiền hóa đơn phải lớn hơn 0");
						return electronicBillResult;
					}
					int cmdType = GetCmdType(cmdTypeCFG);
					EHoaDonManager val2 = new EHoaDonManager(text, val, invoiceDataWSs);
					LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<List<InvoiceDataWS>>((Expression<Func<List<InvoiceDataWS>>>)(() => invoiceDataWSs)), (object)invoiceDataWSs));
					List<InvoiceResult> list = val2.Run(cmdType);
					bool flag = false;
					if (list != null && list.Count > 0)
					{
						foreach (InvoiceResult item in list)
						{
							if (item.Status == 0)
							{
								electronicBillResult.Success = true;
								electronicBillResult.InvoiceSys = "BKAV";
								electronicBillResult.InvoiceCode = ((item.PartnerInvoiceID > 0) ? item.PartnerInvoiceID.ToString() : "");
								electronicBillResult.InvoiceLink = array[2] + "/" + item.MessLog;
								electronicBillResult.InvoiceNumOrder = item.InvoiceNo.ToString();
								electronicBillResult.InvoiceTime = Inventec.Common.DateTime.Get.Now();
								electronicBillResult.InvoiceLoginname = val.BkavPartnerGUID;
								electronicBillResult.InvoiceLookupCode = item.MTC;
								LogSystem.Debug(LogUtil.TraceData("___invoiceResults___", (object)item));
								continue;
							}
							if (ElectronicBillTypeEnum == ElectronicBillType.ENUM.GET_INVOICE_LINK)
							{
								flag = true;
								break;
							}
							ElectronicBillResultUtil.Set(ref electronicBillResult, false, item.MessLog);
							LogSystem.Error("gui thong tin hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => item.MessLog)), (object)item.MessLog) + LogUtil.TraceData(LogUtil.GetMemberName<List<InvoiceDataWS>>((Expression<Func<List<InvoiceDataWS>>>)(() => invoiceDataWSs)), (object)invoiceDataWSs));
							return electronicBillResult;
						}
					}
					if (ElectronicBillTypeEnum == ElectronicBillType.ENUM.GET_INVOICE_LINK && flag)
					{
						ServicePointManager.Expect100Continue = true;
						ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
						EHoaDonManager val3 = new EHoaDonManager(text, val, invoiceDataWSs);
						LogSystem.Info(LogUtil.TraceData("invoiceDataWSs_SHOW: ", (object)invoiceDataWSs));
						List<InvoiceResult> list2 = val3.Run(816);
						if (list2 != null && list2.Count > 0)
						{
							foreach (InvoiceResult item2 in list2)
							{
								if (item2.Status == 0)
								{
									electronicBillResult.Success = true;
									electronicBillResult.InvoiceSys = "BKAV";
									electronicBillResult.InvoiceCode = ((item2.PartnerInvoiceID > 0) ? item2.PartnerInvoiceID.ToString() : "");
									electronicBillResult.InvoiceLink = array[2] + "/" + item2.MessLog;
									electronicBillResult.InvoiceNumOrder = item2.InvoiceNo.ToString();
									electronicBillResult.InvoiceTime = Inventec.Common.DateTime.Get.Now();
									electronicBillResult.InvoiceLoginname = val.BkavPartnerGUID;
									electronicBillResult.InvoiceLookupCode = item2.MTC;
									LogSystem.Debug(LogUtil.TraceData("___invoiceResults_Show___", (object)item2));
									continue;
								}
								if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue && ElectronicBillTypeEnum == ElectronicBillType.ENUM.CREATE_INVOICE)
								{
									ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Thay thế hóa đơn thất bại. " + item2.MessLog);
								}
								else
								{
									ElectronicBillResultUtil.Set(ref electronicBillResult, false, item2.MessLog);
								}
								LogSystem.Error("gui thong tin hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<string>((Expression<Func<string>>)(() => item2.MessLog)), (object)item2.MessLog) + LogUtil.TraceData(LogUtil.GetMemberName<List<InvoiceDataWS>>((Expression<Func<List<InvoiceDataWS>>>)(() => invoiceDataWSs)), (object)invoiceDataWSs));
								return electronicBillResult;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				electronicBillResult.Success = false;
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, ex.Message);
				LogSystem.Warn(ex);
			}
			return electronicBillResult;
		}

		private int GetCmdType(string cmdTypeCFG)
		{
			int result = -1;
			try
			{
				switch (ElectronicBillTypeEnum)
				{
				case ElectronicBillType.ENUM.CREATE_INVOICE:
					result = ((ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue) ? ((!(cmdTypeCFG == "1")) ? 120 : 123) : ((!(cmdTypeCFG == "1")) ? 101 : 112));
					break;
				case ElectronicBillType.ENUM.GET_INVOICE_LINK:
					result = 804;
					break;
				case ElectronicBillType.ENUM.GET_INVOICE_SHOW:
					result = 816;
					break;
				case ElectronicBillType.ENUM.CANCEL_INVOICE:
					result = 202;
					break;
				case ElectronicBillType.ENUM.DELETE_INVOICE:
					result = 301;
					break;
				case ElectronicBillType.ENUM.CONVERT_INVOICE:
				case ElectronicBillType.ENUM.CREATE_INVOICE_DATA:
				case ElectronicBillType.ENUM.GET_INVOICE_INFO:
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private bool Check(ref ElectronicBillResult electronicBillResult)
		{
			bool result = true;
			try
			{
				string[] array = config.Split('|');
				if (array.Length < 3)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
				}
				if (array[0] != "BKAV")
				{
					throw new Exception("Không đúng cấu hình nhà cung cấp BKAV");
				}
				string[] array2 = accountConfig.Split('|');
				if (array2.Length != 3)
				{
					throw new Exception("Sai định dạng cấu hình tài khoản.");
				}
				if (ElectronicBillTypeEnum == ElectronicBillType.ENUM.CREATE_INVOICE)
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
					if (array.Length > 3)
					{
						string text = array[3];
						if (text == "1" && (string.IsNullOrWhiteSpace(ElectronicBillDataInput.TemplateCode) || string.IsNullOrWhiteSpace(ElectronicBillDataInput.SymbolCode)))
						{
							List<string> list = new List<string>();
							if (string.IsNullOrWhiteSpace(ElectronicBillDataInput.TemplateCode))
							{
								list.Add("Mẫu số");
							}
							if (string.IsNullOrWhiteSpace(ElectronicBillDataInput.SymbolCode))
							{
								list.Add("Ký hiệu");
							}
							throw new Exception(string.Format("Không có thông tin {0}.", string.Join(",", list)));
						}
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

		private List<InvoiceDataWS> GetInvoiceContrBKAV(ElectronicBillDataInput electronicBillDataInput)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			List<InvoiceDataWS> list = new List<InvoiceDataWS>();
			InvoiceDataWS val = new InvoiceDataWS();
			if (electronicBillDataInput != null)
			{
				switch (ElectronicBillTypeEnum)
				{
				case ElectronicBillType.ENUM.CREATE_INVOICE:
					val.Invoice = SetInvoice();
					val.ListInvoiceDetailsWS = GetProductElectronicBill(ElectronicBillDataInput);
					val.PartnerInvoiceID = GetTransactionCode();
					val.PartnerInvoiceStringID = "";
					list.Add(val);
					break;
				case ElectronicBillType.ENUM.GET_INVOICE_LINK:
				case ElectronicBillType.ENUM.DELETE_INVOICE:
				case ElectronicBillType.ENUM.CANCEL_INVOICE:
				case ElectronicBillType.ENUM.GET_INVOICE_SHOW:
					val.PartnerInvoiceID = electronicBillDataInput.PartnerInvoiceID;
					val.Invoice = new InvoiceWS();
					val.ListInvoiceAttachFileWS = new List<InvoiceAttachFileWS>();
					val.ListInvoiceDetailsWS = new List<InvoiceDetailsWS>();
					val.PartnerInvoiceStringID = "";
					val.Reason = GetReason();
					list.Add(val);
					break;
				}
			}
			return list;
		}

		private string GetReason()
		{
			string result = "";
			try
			{
				if (ElectronicBillDataInput == null)
				{
					return result;
				}
				LogSystem.Debug(LogUtil.TraceData("__ElectronicBillDataInput", (object)ElectronicBillDataInput));
				result = ElectronicBillDataInput.CancelReason;
			}
			catch (Exception ex)
			{
				result = "";
				LogSystem.Error(ex);
			}
			return result;
		}

		private long GetTransactionCode()
		{
			if (ElectronicBillDataInput.Transaction != null)
			{
				return long.Parse(ElectronicBillDataInput.Transaction.TRANSACTION_CODE);
			}
			if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
			{
				return long.Parse(ElectronicBillDataInput.ListTransaction.OrderByDescending((V_HIS_TRANSACTION o) => o.TRANSACTION_CODE).First().TRANSACTION_CODE);
			}
			return long.Parse(ElectronicBillDataInput.Treatment.TREATMENT_CODE + DateTime.Now.ToString("yyyyMMdd"));
		}

		private InvoiceWS SetInvoice()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Expected O, but got Unknown
			InvoiceWS val = new InvoiceWS();
			val.InvoiceTypeID = 1;
			val.InvoiceDate = DateTime.Now;
			val.InvoiceForm = ElectronicBillDataInput.TemplateCode;
			val.InvoiceSerial = ElectronicBillDataInput.SymbolCode;
			int result = 1;
			if (int.TryParse(ElectronicBillDataInput.TemplateCode, out result))
			{
				val.InvoiceTypeID = result;
			}
			InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
			val.BuyerName = data.BuyerName ?? "";
			val.BuyerBankAccount = data.BuyerAccountNumber ?? "";
			val.BuyerAddress = data.BuyerAddress ?? "";
			val.BuyerUnitName = data.BuyerOrganization ?? "";
			val.BuyerTaxCode = data.BuyerTaxCode ?? "";
			val.PayMethodID = 1;
			int payMethodID = 1;
			if (ElectronicBillDataInput.Transaction != null)
			{
				HIS_PAY_FORM val2 = new HIS_PAY_FORM();
				val2 = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == ElectronicBillDataInput.Transaction.PAY_FORM_ID);
				int result2;
				payMethodID = ((val2 == null || string.IsNullOrEmpty(val2.ELECTRONIC_PAY_FORM_NAME) || !int.TryParse(val2.ELECTRONIC_PAY_FORM_NAME, out result2)) ? 3 : result2);
			}
			else if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
			{
				HIS_PAY_FORM val3 = new HIS_PAY_FORM();
				val3 = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == ElectronicBillDataInput.ListTransaction.First().PAY_FORM_ID);
				int result3;
				payMethodID = ((val3 == null || string.IsNullOrEmpty(val3.ELECTRONIC_PAY_FORM_NAME) || !int.TryParse(val3.ELECTRONIC_PAY_FORM_NAME, out result3)) ? 3 : result3);
			}
			val.PayMethodID = payMethodID;
			val.ReceiveTypeID = 3;
			val.ReceiverEmail = data.BuyerEmail ?? "";
			val.ReceiverMobile = data.BuyerPhone ?? "";
			val.ReceiverAddress = data.BuyerAddress ?? "";
			val.ReceiverName = data.BuyerName ?? "";
			val.Note = "";
			val.BillCode = "";
			val.CurrencyID = ((!string.IsNullOrEmpty(ElectronicBillDataInput.Currency)) ? ElectronicBillDataInput.Currency : "");
			val.ExchangeRate = 1.0;
			val.InvoiceStatusID = 1;
			val.SignedDate = DateTime.Now;
			UserDefineADO userDefine = GetUserDefine(ElectronicBillDataInput.Treatment);
			if (userDefine != null)
			{
				val.UserDefine = JsonConvert.SerializeObject((object)userDefine);
				val.Note = userDefine.DEPARTMENT_NAME;
			}
			if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue)
			{
				val.Reason = ElectronicBillDataInput.Transaction.REPLACE_REASON;
				string arg = long.Parse(ElectronicBillDataInput.Transaction.TDL_ORIGINAL_EI_NUM_ORDER).ToString("D7");
				val.OriginalInvoiceIdentify = string.Format("[{0}]_[{1}]_[{2}]", ElectronicBillDataInput.TemplateCode, ElectronicBillDataInput.SymbolCode, arg);
			}
			return val;
		}

		private UserDefineADO GetUserDefine(V_HIS_TREATMENT_FEE treatment)
		{
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Expected O, but got Unknown
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			UserDefineADO userDefineADO = null;
			try
			{
				if (treatment != null)
				{
					userDefineADO = new UserDefineADO();
					userDefineADO.END_CODE = treatment.END_CODE;
					userDefineADO.EXTRA_END_CODE = treatment.EXTRA_END_CODE;
					userDefineADO.MAIN_CAUSE = treatment.MAIN_CAUSE;
					userDefineADO.OUT_CODE = treatment.OUT_CODE;
					userDefineADO.STORE_CODE = treatment.STORE_CODE;
					userDefineADO.TOTAL_BILL_AMOUNT = treatment.TOTAL_BILL_AMOUNT;
					userDefineADO.TOTAL_BILL_EXEMPTION = treatment.TOTAL_BILL_EXEMPTION;
					userDefineADO.TOTAL_BILL_FUND = treatment.TOTAL_BILL_FUND;
					userDefineADO.TOTAL_BILL_OTHER_AMOUNT = treatment.TOTAL_BILL_OTHER_AMOUNT;
					userDefineADO.TOTAL_BILL_TRANSFER_AMOUNT = treatment.TOTAL_BILL_TRANSFER_AMOUNT;
					userDefineADO.TOTAL_DEBT_AMOUNT = treatment.TOTAL_DEBT_AMOUNT;
					userDefineADO.TOTAL_DEPOSIT_AMOUNT = treatment.TOTAL_DEPOSIT_AMOUNT;
					userDefineADO.TOTAL_DISCOUNT = treatment.TOTAL_DISCOUNT;
					userDefineADO.TOTAL_HEIN_PRICE = treatment.TOTAL_HEIN_PRICE;
					userDefineADO.TOTAL_PATIENT_PRICE = treatment.TOTAL_PATIENT_PRICE;
					userDefineADO.TOTAL_PRICE = treatment.TOTAL_PRICE;
					userDefineADO.TOTAL_PRICE_EXPEND = treatment.TOTAL_PRICE_EXPEND;
					userDefineADO.TOTAL_REPAY_AMOUNT = treatment.TOTAL_REPAY_AMOUNT;
					userDefineADO.TREATMENT_CODE = treatment.TREATMENT_CODE;
					userDefineADO.TREATMENT_DAY_COUNT = treatment.TREATMENT_DAY_COUNT;
					CommonParam val = new CommonParam();
					HisDepartmentTranLastFilter val2 = new HisDepartmentTranLastFilter();
					val2.TREATMENT_ID = treatment.ID;
					V_HIS_DEPARTMENT_TRAN val3 = ((AdapterBase)new BackendAdapter(val)).Get<V_HIS_DEPARTMENT_TRAN>("/api/HisDepartmentTran/GetLastByTreatmentId", ApiConsumers.MosConsumer, (object)val2, val);
					if (val3 != null)
					{
						userDefineADO.DEPARTMENT_CODE = val3.DEPARTMENT_CODE;
						userDefineADO.DEPARTMENT_NAME = val3.DEPARTMENT_NAME;
						userDefineADO.Khoa = val3.DEPARTMENT_NAME;
					}
				}
			}
			catch (Exception ex)
			{
				userDefineADO = null;
				LogSystem.Error(ex);
			}
			return userDefineADO;
		}

		private List<InvoiceDetailsWS> GetProductElectronicBill(ElectronicBillDataInput dataInput)
		{
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			List<InvoiceDetailsWS> list = new List<InvoiceDetailsWS>();
			IRunTemplate runTemplate = TemplateFactory.MakeIRun(TempType, dataInput);
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
					InvoiceDetailsWS val = new InvoiceDetailsWS();
					val.Qty = (double)item.ProdQuantity.GetValueOrDefault();
					val.ItemName = item.ProdName ?? "";
					val.UnitName = item.ProdUnit ?? "";
					val.TaxRateID = ((item.TaxRateID > 0) ? item.TaxRateID : 4);
					val.Amount = (double)item.Amount;
					val.Price = (double)item.ProdPrice.GetValueOrDefault();
					list.Add(val);
				}
			}
			else
			{
				List<ProductBasePlus> list3 = (List<ProductBasePlus>)obj;
				if (list3 == null || list3.Count == 0)
				{
					throw new Exception("Không có thông tin chi tiết dịch vụ.");
				}
				foreach (ProductBasePlus item2 in list3)
				{
					InvoiceDetailsWS val2 = new InvoiceDetailsWS();
					val2.Qty = (double)item2.ProdQuantity.GetValueOrDefault();
					val2.ItemName = item2.ProdName ?? "";
					val2.UnitName = item2.ProdUnit ?? "";
					val2.Amount = (double)item2.AmountWithoutTax.Value;
					val2.TaxAmount = (double)item2.TaxAmount.Value;
					val2.Price = (double)item2.ProdPrice.GetValueOrDefault();
					if (!item2.TaxPercentage.HasValue)
					{
						val2.TaxRateID = 4;
					}
					else if (item2.TaxPercentage == 0)
					{
						val2.TaxRateID = 1;
					}
					else if (item2.TaxPercentage == 1)
					{
						val2.TaxRateID = 2;
					}
					else if (item2.TaxPercentage == 2)
					{
						val2.TaxRateID = 3;
					}
					else if (item2.TaxPercentage == 3)
					{
						val2.TaxRateID = 6;
					}
					else
					{
						val2.TaxRateID = 6;
					}
					list.Add(val2);
				}
			}
			return list;
		}
	}
}
