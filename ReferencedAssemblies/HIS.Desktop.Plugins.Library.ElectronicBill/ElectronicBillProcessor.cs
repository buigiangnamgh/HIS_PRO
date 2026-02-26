using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.BKAV;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.CYBERBILL;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MInvoice;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MISA;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MOBIFONE;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MOIT;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SAFECERT;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.SODR;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VIETTEL;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VNInvoice;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VNPT;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.BACHMAI;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.CTO;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill
{
	public class ElectronicBillProcessor
	{
		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		private TemplateEnum.TYPE TempType { get; set; }

		public ElectronicBillProcessor(ElectronicBillDataInput _electronicBillDataInput, TemplateEnum.TYPE _templateType)
		{
			try
			{
				LogSystem.Debug("_electronicBillDataInput: " + LogUtil.TraceData("DataA", (object)_electronicBillDataInput));
				ElectronicBillDataInput = _electronicBillDataInput;
				TempType = _templateType;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public ElectronicBillProcessor(ElectronicBillDataInput _electronicBillDataInput)
		{
			try
			{
				ElectronicBillDataInput = _electronicBillDataInput;
				TempType = TemplateEnum.TYPE.Template1;
				long num = HisConfigs.Get<long>("HIS.Desktop.Plugins.Library.ElectronicBill.Template");
				if (num != 1)
				{
					try
					{
						TempType = (TemplateEnum.TYPE)num;
						return;
					}
					catch (Exception)
					{
						TempType = TemplateEnum.TYPE.Template1;
						return;
					}
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}

		public ElectronicBillResult Run(ElectronicBillType.ENUM type)
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			try
			{
				General.DicDataBuyerInfo = null;
				string text = HisConfigs.Get<string>("HIS.DESKTOP.LIBRARY.ELECTRONIC_BILL.CONFIG");
				string accountConfig = ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT");
				LogSystem.Debug("ElectronicBillDataInput.EinvoiceTypeId: " + LogUtil.TraceData("DataA", (object)ElectronicBillDataInput.EinvoiceTypeId));
				if (ElectronicBillDataInput.EinvoiceTypeId.HasValue)
				{
					long value = ElectronicBillDataInput.EinvoiceTypeId.Value;
					long num = value;
					long num2 = num - 1;
					if ((ulong)num2 <= 8uL)
					{
						switch (num2)
						{
						case 0L:
						{
							HIS_EINVOICE_TYPE val8 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 1);
							text = string.Format("{0}|{1}", "BKAV", (val8 != null) ? val8.VALUE : "");
							accountConfig = ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__BKAV");
							break;
						}
						case 1L:
						{
							HIS_EINVOICE_TYPE val3 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 2);
							text = string.Format("{0}|{1}", "VIETEL", (val3 != null) ? val3.VALUE : "");
							accountConfig = ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__VIETEL");
							break;
						}
						case 2L:
						{
							HIS_EINVOICE_TYPE val6 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 3);
							List<string> configTotal = new List<string>();
							configTotal.Add("VNPT");
							GetConfigVnpt((val6 != null) ? val6.VALUE : "", ElectronicBillDataInput, ref configTotal);
							text = string.Join("|", configTotal);
							accountConfig = ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__VNPT");
							break;
						}
						case 3L:
						{
							HIS_EINVOICE_TYPE val9 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 4);
							text = string.Format("{0}|{1}", "CTO_PROXY", (val9 != null) ? val9.VALUE : "");
							accountConfig = "CTO_PROXY";
							break;
						}
						case 4L:
						{
							HIS_EINVOICE_TYPE val5 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 5);
							text = string.Format("{0}|{1}", "MOBIFONE", (val5 != null) ? val5.VALUE : "");
							accountConfig = ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__MOBIFONE");
							break;
						}
						case 5L:
						{
							HIS_EINVOICE_TYPE val2 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 6);
							text = string.Format("{0}|{1}", "CYBERBILL", (val2 != null) ? val2.VALUE : "");
							accountConfig = ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__CYBERBILL");
							break;
						}
						case 6L:
						{
							HIS_EINVOICE_TYPE val4 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 7);
							text = string.Format("{0}|{1}", "SODR", (val4 != null) ? val4.VALUE : "");
							accountConfig = ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__SOFTDREAM");
							break;
						}
						case 7L:
						{
							HIS_EINVOICE_TYPE val7 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 8);
							text = string.Format("{0}|{1}", "MINVOICE", (val7 != null) ? val7.VALUE : "");
							accountConfig = ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__MINVOICE");
							break;
						}
						case 8L:
						{
							HIS_EINVOICE_TYPE val = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 9);
							text = string.Format("{0}|{1}", "VNINVOICE", (val != null) ? val.VALUE : "");
							accountConfig = ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__VNINVOICE");
							break;
						}
						}
					}
				}
				HisConfigCFG.LoadConfig();
				GetCurrentPatientTypeAlter();
				if (Check(ref electronicBillResult, text, accountConfig, type))
				{
					string provider = GetProvider(text);
					IRun run = null;
					switch (provider)
					{
					case "VNPT":
						run = new VNPTBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "BKAV":
						run = new BKAVBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "VIETEL":
						run = new VIETTELBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "MOIT":
						run = new VOITBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "SODR":
						run = new SODRBehavior(ElectronicBillDataInput, text, accountConfig, ElectronicBillDataInput.EinvoiceTypeId.HasValue);
						break;
					case "MISA":
						run = new MISABehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "SAFECERT":
						run = new SAFECERTBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "CTO_PROXY":
						run = new CTOBehavior(ElectronicBillDataInput, text);
						break;
					case "BACH_MAI":
						run = new BACHMAIBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "MOBIFONE":
						run = new MIBIFONEBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "CYBERBILL":
						run = new CYBERBILLBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "MINVOICE":
						run = new MInvoiceBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					case "VNINVOICE":
						run = new VNInvoiceBehavior(ElectronicBillDataInput, text, accountConfig);
						break;
					}
					electronicBillResult = ((run != null) ? run.Run(type, TempType) : null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return electronicBillResult;
		}

		private void GetCurrentPatientTypeAlter()
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (ElectronicBillDataInput.LastPatientTypeAlter == null)
				{
					if (ElectronicBillDataInput != null && ElectronicBillDataInput.Treatment != null && ElectronicBillDataInput.Treatment.ID > 0)
					{
						CommonParam val = new CommonParam();
						ElectronicBillDataInput.LastPatientTypeAlter = ((AdapterBase)new BackendAdapter(val)).Get<V_HIS_PATIENT_TYPE_ALTER>("/api/HisPatientTypeAlter/GetViewLastByTreatmentId", ApiConsumers.MosConsumer, (object)ElectronicBillDataInput.Treatment.ID, val);
					}
					else
					{
						ElectronicBillDataInput.LastPatientTypeAlter = new V_HIS_PATIENT_TYPE_ALTER();
						ElectronicBillDataInput.LastPatientTypeAlter.TREATMENT_TYPE_ID = 1L;
						ElectronicBillDataInput.LastPatientTypeAlter.PATIENT_TYPE_ID = 0L;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void GetConfigVnpt(string config, ElectronicBillDataInput dataInput, ref List<string> configTotal)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(config))
				{
					return;
				}
				string[] array = config.Split('|');
				for (int i = 0; i < array.Length; i++)
				{
					configTotal.Add(array[i].Trim());
					if (i == 2)
					{
						configTotal.Add(dataInput.TemplateCode);
						configTotal.Add(dataInput.SymbolCode);
					}
				}
				if (configTotal.Count < 7)
				{
					for (int j = 0; j < 7 - configTotal.Count; j++)
					{
						configTotal.Add("0");
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public static bool GetInvoiceInfo(ElectronicBillDataInput _electronicBillDataInput, ref string invoiceSys, ref string invoiceCode, ref string errorMess)
		{
			bool result = false;
			try
			{
				string config = HisConfigs.Get<string>("HIS.DESKTOP.LIBRARY.ELECTRONIC_BILL.CONFIG");
				if (_electronicBillDataInput.EinvoiceTypeId.HasValue)
				{
					long value = _electronicBillDataInput.EinvoiceTypeId.Value;
					long num = value;
					long num2 = num - 1;
					if ((ulong)num2 <= 2uL)
					{
						switch (num2)
						{
						case 0L:
						{
							HIS_EINVOICE_TYPE val2 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 1);
							config = string.Format("{0}|{1}", "BKAV", (val2 != null) ? val2.VALUE : "");
							break;
						}
						case 1L:
						{
							HIS_EINVOICE_TYPE val3 = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 2);
							config = string.Format("{0}|{1}", "VIETEL", (val3 != null) ? val3.VALUE : "");
							break;
						}
						case 2L:
						{
							HIS_EINVOICE_TYPE val = BackendDataWorker.Get<HIS_EINVOICE_TYPE>().FirstOrDefault((HIS_EINVOICE_TYPE o) => o.ID == 3);
							config = string.Format("{0}|{1}", "VNPT", (val != null) ? val.VALUE : "");
							break;
						}
						}
					}
				}
				switch (GetProvider(config))
				{
				case "VNPT":
					invoiceSys = "VNPT";
					invoiceCode = _electronicBillDataInput.InvoiceCode;
					result = true;
					break;
				case "BKAV":
					invoiceSys = "BKAV";
					invoiceCode = _electronicBillDataInput.InvoiceCode;
					result = true;
					break;
				case "VIETEL":
					if (!string.IsNullOrWhiteSpace(_electronicBillDataInput.TemplateCode) && !string.IsNullOrWhiteSpace(_electronicBillDataInput.SymbolCode))
					{
						invoiceSys = "VIETEL";
						invoiceCode = string.Format("{0}|{1}", _electronicBillDataInput.InvoiceCode, GetNumOrder(_electronicBillDataInput.ENumOrder));
						result = true;
						break;
					}
					if (errorMess == null)
					{
						errorMess = "";
					}
					if (string.IsNullOrWhiteSpace(_electronicBillDataInput.TemplateCode) || string.IsNullOrWhiteSpace(_electronicBillDataInput.SymbolCode))
					{
						errorMess += "Giao dịch hiện tại thiếu thông tin ";
						if (string.IsNullOrWhiteSpace(_electronicBillDataInput.TemplateCode))
						{
							errorMess += " mẫu số,";
						}
						if (string.IsNullOrWhiteSpace(_electronicBillDataInput.SymbolCode))
						{
							errorMess += " ký hiệu.";
						}
					}
					break;
				case "MOIT":
					invoiceSys = "MOIT";
					invoiceCode = _electronicBillDataInput.InvoiceCode;
					result = true;
					break;
				case "SODR":
					invoiceSys = "SODR";
					invoiceCode = _electronicBillDataInput.InvoiceCode;
					result = true;
					break;
				case "MISA":
					invoiceSys = "MISA";
					invoiceCode = _electronicBillDataInput.InvoiceCode;
					result = true;
					break;
				case "SAFECERT":
					invoiceSys = "SAFECERT";
					invoiceCode = _electronicBillDataInput.InvoiceCode;
					result = true;
					break;
				case "CTO_PROXY":
					invoiceSys = "CTO_PROXY";
					invoiceCode = _electronicBillDataInput.InvoiceCode;
					result = true;
					break;
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Error(ex);
			}
			return result;
		}

		private bool Check(ref ElectronicBillResult electronicBillResult, string serviceConfig, string accountConfig, ElectronicBillType.ENUM type)
		{
			bool result = true;
			try
			{
				if (string.IsNullOrEmpty(serviceConfig))
				{
					LogSystem.Error("Khong tim thay thong tin cau hinh serviceConfig");
					result = false;
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không tìm thấy thông tin cấu hình serviceConfig");
				}
				else if (string.IsNullOrEmpty(accountConfig) && !serviceConfig.Contains("SAFECERT"))
				{
					LogSystem.Error("Khong tim thay thong tin cau hinh tai khoan");
					result = false;
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không tìm thấy thông tin cấu hình tài khoản");
				}
				else if (type == ElectronicBillType.ENUM.CREATE_INVOICE && ElectronicBillDataInput.Transaction == null && (ElectronicBillDataInput.ListTransaction == null || ElectronicBillDataInput.ListTransaction.Count == 0))
				{
					LogSystem.Error("Khong tim thay thong tin giao dich");
					result = false;
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không xác định được giao dịch");
				}
				else if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
				{
					List<IGrouping<long?, V_HIS_TRANSACTION>> list = (from o in ElectronicBillDataInput.ListTransaction
						group o by o.TREATMENT_ID).ToList();
					if (list.Count > 1)
					{
						LogSystem.Error("Giao dich thuoc nhieu ho so dieu tri: " + string.Join(", ", list.Select((IGrouping<long?, V_HIS_TRANSACTION> s) => s.Key)));
						result = false;
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Các giao dịch không cùng hồ sơ điều trị");
					}
					List<V_HIS_TRANSACTION> list2 = ElectronicBillDataInput.ListTransaction.Where((V_HIS_TRANSACTION o) => o.ORIGINAL_TRANSACTION_ID.HasValue).ToList();
					if (list2 != null && list2.Count > 0)
					{
						LogSystem.Error("Giao dich la thay the: " + string.Join(", ", list2.Select((V_HIS_TRANSACTION s) => s.TRANSACTION_CODE)));
						result = false;
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, string.Format("Giao dịch {0} là giao dịch thay thế không cho phép gộp", string.Join(", ", list2.Select((V_HIS_TRANSACTION s) => s.TRANSACTION_CODE))));
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private static string GetProvider(string config)
		{
			string text = null;
			try
			{
				if (string.IsNullOrEmpty(config))
				{
					throw new Exception("Khong tim thay thong tin cau hinh. Vui long kiem tra key cau hinh : HIS.DESKTOP.LIBRARY.ELECTRONIC_BILL.CONFIG");
				}
				string[] array = config.Split('|');
				if (!ProviderType.TYPE.Contains(array[0].Trim()))
				{
					throw new Exception("Khong tim thay nha cung cap dich vu tu cau hinh. Vui long kiem tra key cau hinh : HIS.DESKTOP.LIBRARY.ELECTRONIC_BILL.CONFIG");
				}
				text = array[0].Trim();
			}
			catch (Exception ex)
			{
				text = null;
				LogSystem.Warn(ex);
			}
			return text;
		}

		private static string GetNumOrder(string p)
		{
			string result = p;
			try
			{
				if (!string.IsNullOrWhiteSpace(p))
				{
					int startIndex = 0;
					int length = p.Length;
					if (p.Length > 7)
					{
						startIndex = p.Length - 7;
						length = 7;
					}
					result = p.Substring(startIndex, length);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}
	}
}
