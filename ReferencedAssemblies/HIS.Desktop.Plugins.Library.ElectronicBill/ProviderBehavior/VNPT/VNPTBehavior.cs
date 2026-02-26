using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Serialization;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.DateTime;
using Inventec.Common.ElectronicBill;
using Inventec.Common.ElectronicBill.Base;
using Inventec.Common.ElectronicBill.MD;
using Inventec.Common.ElectronicBill.ModelTT78;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.Number;
using Inventec.Common.Repository;
using Inventec.Common.String;
using Inventec.Common.TypeConvert;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VNPT
{
    public class VNPTBehavior : HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun
	{
		public enum SERE_SERV_TYPE
		{
			BHYT_NOT_SERVICE_CONFIG,
			NOT_BHYT_NOT_SERVICE_CONFIG,
			SERVICE_CONFIG,
			NOT_SERVICE_CONFIG
		}

		private string serviceConfig { get; set; }

		private string accountConfig { get; set; }

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		private Dictionary<SERE_SERV_TYPE, List<V_HIS_SERE_SERV_5>> dicSereServType { get; set; }

		private TemplateEnum.TYPE TempType { get; set; }

		private ElectronicBillType.ENUM ElectronicBillTypeEnum { get; set; }

		public VNPTBehavior(ElectronicBillDataInput _electronicBillDataInput, string _serviceConfig, string _accountConfig)
		{
			serviceConfig = _serviceConfig;
			ElectronicBillDataInput = _electronicBillDataInput;
			accountConfig = _accountConfig;
		}

        HIS.Desktop.Plugins.Library.ElectronicBill.Base.ElectronicBillResult HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun.Run(ElectronicBillType.ENUM _electronicBillTypeEnum, TemplateEnum.TYPE templateType)
		{
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Expected O, but got Unknown
			//IL_0b55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b5c: Expected O, but got Unknown
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Expected O, but got Unknown
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Expected O, but got Unknown
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Expected O, but got Unknown
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Expected O, but got Unknown
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cf: Expected O, but got Unknown
			//IL_0548: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Expected O, but got Unknown
			//IL_0618: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Expected O, but got Unknown
			ElectronicBillTypeEnum = _electronicBillTypeEnum;
            HIS.Desktop.Plugins.Library.ElectronicBill.Base.ElectronicBillResult electronicBillResult = new HIS.Desktop.Plugins.Library.ElectronicBill.Base.ElectronicBillResult();
			try
			{
				TempType = templateType;
				if (Check(ref electronicBillResult))
				{
					string[] array = serviceConfig.Split('|');
					string text = array[1].Trim();
					string text2 = array[2].Trim();
					string passWord = array[3].Trim();
					string text3 = array[4].Trim();
					string serial = array[5].Trim();
					int convert = Parse.ToInt32(array[6].Trim());
					string text4 = "";
					string cancelFunc = "";
					string text5 = "";
					string text6 = "";
					if (array.Length > 7)
					{
						text4 = array[7].Trim();
					}
					if (array.Length > 8)
					{
						cancelFunc = array[8].Trim();
					}
					if (array.Length > 9)
					{
						text5 = array[9].Trim();
					}
					if (array.Length > 10)
					{
						text6 = array[10].Trim();
					}
					string[] array2 = accountConfig.Split('|');
					string text7 = array2[0].Trim();
					string acPass = array2[1].Trim();
					int cmdType = GetCmdType(text4, cancelFunc, text6);
					string tDL_PATIENT_CODE = ElectronicBillDataInput.Treatment.TDL_PATIENT_CODE;
					string tREATMENT_CODE = ElectronicBillDataInput.Treatment.TREATMENT_CODE;
					ElectronicBillInput electronicBillInput = new ElectronicBillInput();
					electronicBillInput.account = text7;
					electronicBillInput.acPass = acPass;
					electronicBillInput.convert = convert;
					if (cmdType != 100 && cmdType != 113 && ElectronicBillDataInput != null && !string.IsNullOrWhiteSpace(ElectronicBillDataInput.InvoiceCode))
					{
						electronicBillInput.fKey = ElectronicBillDataInput.InvoiceCode;
					}
					else
					{
						string arg = "";
						if (ElectronicBillDataInput.Transaction != null)
						{
							arg = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
						}
						else if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
						{
							arg = ElectronicBillDataInput.ListTransaction.OrderBy((V_HIS_TRANSACTION o) => o.TRANSACTION_CODE).First().TRANSACTION_CODE;
						}
						electronicBillInput.fKey = string.Format("{0}_{1}_{2}", tDL_PATIENT_CODE, tREATMENT_CODE, arg);
					}
					electronicBillInput.passWord = passWord;
					electronicBillInput.pattern = text3;
					electronicBillInput.serial = serial;
					electronicBillInput.serviceUrl = text;
					electronicBillInput.userName = text2;
					if (ElectronicBillTypeEnum == ElectronicBillType.ENUM.CREATE_INVOICE)
					{
						bool notShowTaxBreakdown = (CheckHoaDonBanHang(text3) ? true : false);
						if (text5 == "2")
						{
							electronicBillInput.invoiceTT78s = GetInvoiceTT78(ElectronicBillDataInput, notShowTaxBreakdown);
							LogSystem.Info(LogUtil.TraceData("_______ invoiceTT78s: ", (object)electronicBillInput.invoiceTT78s));
						}
						else
						{
							List<Invoice> invoiceContrVietSens = GetInvoiceContrVietSens(ElectronicBillDataInput, notShowTaxBreakdown);
							if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.IS_ADJUSTMENT == 1)
							{
								AdjustInvoice val = new AdjustInvoice();
								DataObjectMapper.Map<AdjustInvoice>((object)val, (object)invoiceContrVietSens.First().InvoiceDetail);
								val.key = invoiceContrVietSens.First().Key;
								((InvoiceDetail)val).Products = new List<Product>();
								foreach (Product product in invoiceContrVietSens.First().InvoiceDetail.Products)
								{
									Product val2 = new Product();
									DataObjectMapper.Map<Product>((object)val2, (object)product);
									((InvoiceDetail)val).Products.Add(val2);
								}
								electronicBillInput.fKey = ElectronicBillDataInput.Transaction.TDL_ORIGINAL_TRAN_CODE;
								electronicBillInput.adjustInvoice = val;
							}
							else if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue)
							{
								ReplaceInvoice val3 = new ReplaceInvoice();
								DataObjectMapper.Map<ReplaceInvoice>((object)val3, (object)invoiceContrVietSens.First().InvoiceDetail);
								electronicBillInput.fKey = ElectronicBillDataInput.Transaction.TDL_ORIGINAL_EI_CODE;
								val3.key = invoiceContrVietSens.First().Key;
								((InvoiceDetail)val3).Products = new List<Product>();
								foreach (Product product2 in invoiceContrVietSens.First().InvoiceDetail.Products)
								{
									Product val4 = new Product();
									DataObjectMapper.Map<Product>((object)val4, (object)product2);
									((InvoiceDetail)val3).Products.Add(val4);
								}
								electronicBillInput.replaceInvoice = val3;
							}
							else if (text4 == "1")
							{
								Invoice_BM val5 = new Invoice_BM();
								val5.Key = invoiceContrVietSens.First().Key;
								InvoiceDetail_BM val6 = new InvoiceDetail_BM();
								DataObjectMapper.Map<InvoiceDetail_BM>((object)val6, (object)invoiceContrVietSens.First().InvoiceDetail);
								val6.Products = new List<ProductBm>();
								int num = 1;
								foreach (Product product3 in invoiceContrVietSens.First().InvoiceDetail.Products)
								{
									ProductBm val7 = new ProductBm();
									DataObjectMapper.Map<ProductBm>((object)val7, (object)product3);
									val7.Extra1 = num.ToString() ?? "";
									val7.Extra2 = "0";
									val6.Products.Add(val7);
									num++;
								}
								val6.VATAmount = "";
								val6.VATRate = "-1";
								val6.KindOfService = "";
								val6.PaymentStatus = "1";
								val6.AmountValue = string.Format("{0:0.####}", ElectronicBillDataInput.Treatment.TOTAL_PATIENT_PRICE.GetValueOrDefault());
								val6.TamUng = string.Format("{0:0.####}", ElectronicBillDataInput.Treatment.TOTAL_DEPOSIT_AMOUNT.GetValueOrDefault());
								decimal num2 = ElectronicBillDataInput.Treatment.TOTAL_PATIENT_PRICE.GetValueOrDefault() - ElectronicBillDataInput.Treatment.TOTAL_DEPOSIT_AMOUNT.GetValueOrDefault();
								string text8 = string.Format("{0:0.####}", (num2 >= 0m) ? num2 : (-num2));
								val6.PayKH = ((num2 > 0m) ? text8 : "0");
								val6.RePayKH = ((num2 < 0m) ? text8 : "0");
								HIS_PATIENT_TYPE val8 = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == ElectronicBillDataInput.Treatment.TDL_PATIENT_TYPE_ID);
								val6.NoteINV = ((val8 != null) ? val8.PATIENT_TYPE_NAME : "");
								val6.CodeTNBA = ElectronicBillDataInput.Treatment.TDL_PATIENT_CODE;
								val6.PaymentMethod = GetFirstCharUpper(ElectronicBillDataInput.PaymentMethod) ?? "TM";
								val6.ArisingDate = DateTime.Now.ToString("dd/MM/yyyy");
								val5.InvoiceDetailBm = val6;
								LogSystem.Info(LogUtil.TraceData("_______ nvoiceDetail: ", (object)val6));
								electronicBillInput.invoicesBm = new List<Invoice_BM> { val5 };
							}
							else
							{
								electronicBillInput.invoices = invoiceContrVietSens;
								LogSystem.Info(LogUtil.TraceData("_______ invoices: ", (object)invoiceContrVietSens));
							}
						}
					}
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					electronicBillInput.DataXmlStringPlus = General.GenarateXmlStringFromConfig(ElectronicBillDataInput, typeof(InvoiceDetail), dictionary);
					if (dictionary.Count > 0)
					{
						PropertyInfo[] array3 = Properties.Get<InvoiceDetail>();
						if (electronicBillInput.invoices != null && electronicBillInput.invoices.Count > 0)
						{
							foreach (Invoice invoice in electronicBillInput.invoices)
							{
								PropertyInfo[] array4 = array3;
								foreach (PropertyInfo propertyInfo in array4)
								{
									if (dictionary.ContainsKey(propertyInfo.Name) && !string.IsNullOrWhiteSpace(dictionary[propertyInfo.Name]))
									{
										if (dictionary[propertyInfo.Name].StartsWith("<![CDATA[") && dictionary[propertyInfo.Name].EndsWith("]]>"))
										{
											dictionary[propertyInfo.Name] = dictionary[propertyInfo.Name].Substring(9, dictionary[propertyInfo.Name].Length - 12);
										}
										propertyInfo.SetValue(invoice.InvoiceDetail, dictionary[propertyInfo.Name]);
									}
								}
							}
						}
						if (electronicBillInput.adjustInvoice != null)
						{
							PropertyInfo[] array5 = Properties.Get<AdjustInvoice>();
							PropertyInfo[] array6 = array5;
							foreach (PropertyInfo propertyInfo2 in array6)
							{
								if (dictionary.ContainsKey(propertyInfo2.Name) && !string.IsNullOrWhiteSpace(dictionary[propertyInfo2.Name]))
								{
									if (dictionary[propertyInfo2.Name].StartsWith("<![CDATA[") && dictionary[propertyInfo2.Name].EndsWith("]]>"))
									{
										dictionary[propertyInfo2.Name] = dictionary[propertyInfo2.Name].Substring(9, dictionary[propertyInfo2.Name].Length - 12);
									}
									propertyInfo2.SetValue(electronicBillInput.adjustInvoice, dictionary[propertyInfo2.Name]);
								}
							}
						}
					}
					ElectronicBillManager val9 = new ElectronicBillManager(electronicBillInput);
					LogSystem.Debug(string.Format("{0} ,{1}, {2}", text, text7, text2));
					try
					{
						LogSystem.Debug("RegisToken.1");
						ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
						ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
						LogSystem.Debug("RegisToken.2");
					}
					catch (Exception ex)
					{
						LogSystem.Warn("ServicePointManager.ServerCertificateValidationCallback error:", ex);
					}
                    Inventec.Common.ElectronicBill.Base.ElectronicBillResult billResult = val9.Run(cmdType);
					if (billResult != null && billResult.Success)
					{
						electronicBillResult.Success = true;
						if (electronicBillInput.invoices != null && electronicBillInput.invoices.Count > 0)
						{
							electronicBillResult.InvoiceCode = electronicBillInput.invoices.First().Key;
						}
						else if (electronicBillInput.invoicesBm != null && electronicBillInput.invoicesBm.Count > 0)
						{
							electronicBillResult.InvoiceCode = electronicBillInput.invoicesBm.First().Key;
						}
						else if (electronicBillInput.invoiceTT78s != null && electronicBillInput.invoiceTT78s.Count > 0)
						{
							electronicBillResult.InvoiceCode = electronicBillInput.invoiceTT78s.First().Key;
						}
						else if (ElectronicBillTypeEnum == ElectronicBillType.ENUM.GET_INVOICE_INFO)
						{
							electronicBillResult.InvoiceCode = ElectronicBillDataInput.InvoiceCode;
						}
						else if (electronicBillInput.replaceInvoice != null)
						{
							electronicBillResult.InvoiceCode = electronicBillInput.replaceInvoice.key;
						}
						else if (electronicBillInput.adjustInvoice != null)
						{
							electronicBillResult.InvoiceCode = electronicBillInput.adjustInvoice.key;
						}
						electronicBillResult.InvoiceSys = "VNPT";
						electronicBillResult.InvoiceLink = billResult.InvoiceLink;
						if (cmdType != 112)
						{
							electronicBillResult.InvoiceNumOrder = GetNumOrder(billResult.Data);
						}
						electronicBillResult.InvoiceTime = Inventec.Common.DateTime.Get.Now();
						electronicBillResult.InvoiceLoginname = text2;
					}
					else
					{
						if (text6 == "1" && cmdType == 113)
						{
							LogSystem.Warn("Phát hành nháp VNPT thất bại");
                            Inventec.Common.ElectronicBill.Base.ElectronicBillResult val10 = val9.Run(100);
							if (val10 != null && val10.Success)
							{
								electronicBillResult.Success = true;
								if (electronicBillInput.invoices != null && electronicBillInput.invoices.Count > 0)
								{
									electronicBillResult.InvoiceCode = electronicBillInput.invoices.First().Key;
								}
								else if (electronicBillInput.invoicesBm != null && electronicBillInput.invoicesBm.Count > 0)
								{
									electronicBillResult.InvoiceCode = electronicBillInput.invoicesBm.First().Key;
								}
								else if (electronicBillInput.invoiceTT78s != null && electronicBillInput.invoiceTT78s.Count > 0)
								{
									electronicBillResult.InvoiceCode = electronicBillInput.invoiceTT78s.First().Key;
								}
								else if (ElectronicBillTypeEnum == ElectronicBillType.ENUM.GET_INVOICE_INFO)
								{
									electronicBillResult.InvoiceCode = ElectronicBillDataInput.InvoiceCode;
								}
								else if (electronicBillInput.replaceInvoice != null)
								{
									electronicBillResult.InvoiceCode = electronicBillInput.replaceInvoice.key;
								}
								else if (electronicBillInput.adjustInvoice != null)
								{
									electronicBillResult.InvoiceCode = electronicBillInput.adjustInvoice.key;
								}
								electronicBillResult.InvoiceSys = "VNPT";
								electronicBillResult.InvoiceLink = val10.InvoiceLink;
								if (cmdType != 112)
								{
									electronicBillResult.InvoiceNumOrder = GetNumOrder(val10.Data);
								}
								electronicBillResult.InvoiceTime = Inventec.Common.DateTime.Get.Now();
								electronicBillResult.InvoiceLoginname = text2;
								return electronicBillResult;
							}
						}
						if (cmdType == 103 || cmdType == 101)
						{
							LogSystem.Debug(string.Format("{0} ,{1}, {2}", text, text7, text2));
							try
							{
								LogSystem.Debug("RegisToken.1");
								ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
								ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
								LogSystem.Debug("RegisToken.2");
							}
							catch (Exception ex2)
							{
								LogSystem.Warn("ServicePointManager.ServerCertificateValidationCallback error:", ex2);
							}
                            Inventec.Common.ElectronicBill.Base.ElectronicBillResult billResult2 = val9.Run(109);
							if (billResult2 != null && billResult2.Success)
							{
								electronicBillResult.Success = true;
								if (electronicBillInput.invoices != null && electronicBillInput.invoices.Count > 0)
								{
									electronicBillResult.InvoiceCode = electronicBillInput.invoices.First().Key;
								}
								else if (electronicBillInput.invoicesBm != null && electronicBillInput.invoicesBm.Count > 0)
								{
									electronicBillResult.InvoiceCode = electronicBillInput.invoicesBm.First().Key;
								}
								else if (electronicBillInput.invoiceTT78s != null && electronicBillInput.invoiceTT78s.Count > 0)
								{
									electronicBillResult.InvoiceCode = electronicBillInput.invoiceTT78s.First().Key;
								}
								else if (ElectronicBillTypeEnum == ElectronicBillType.ENUM.GET_INVOICE_INFO)
								{
									electronicBillResult.InvoiceCode = ElectronicBillDataInput.InvoiceCode;
								}
								else if (electronicBillInput.replaceInvoice != null)
								{
									electronicBillResult.InvoiceCode = electronicBillInput.replaceInvoice.key;
								}
								electronicBillResult.InvoiceSys = "VNPT";
								electronicBillResult.InvoiceLink = billResult2.InvoiceLink;
								if (cmdType != 112)
								{
									electronicBillResult.InvoiceNumOrder = GetNumOrder(billResult2.Data);
								}
								electronicBillResult.InvoiceTime = Inventec.Common.DateTime.Get.Now();
								electronicBillResult.InvoiceLoginname = text2;
							}
							else
							{
								ElectronicBillResultUtil.Set(ref electronicBillResult, false, (billResult2 != null) ? billResult2.Messages : null);
                                LogSystem.Error("Gui thong tin hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<Inventec.Common.ElectronicBill.Base.ElectronicBillResult>((Expression<Func<Inventec.Common.ElectronicBill.Base.ElectronicBillResult>>)(() => billResult2)), (object)billResult2) + LogUtil.TraceData(LogUtil.GetMemberName<List<Invoice>>((Expression<Func<List<Invoice>>>)(() => electronicBillInput.invoices)), (object)electronicBillInput.invoices));
							}
						}
						else
						{
							ElectronicBillResultUtil.Set(ref electronicBillResult, false, (billResult != null) ? billResult.Messages : null);
                            LogSystem.Error("Gui thong tin hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<Inventec.Common.ElectronicBill.Base.ElectronicBillResult>((Expression<Func<Inventec.Common.ElectronicBill.Base.ElectronicBillResult>>)(() => billResult)), (object)billResult) + LogUtil.TraceData(LogUtil.GetMemberName<List<Invoice>>((Expression<Func<List<Invoice>>>)(() => electronicBillInput.invoices)), (object)electronicBillInput.invoices));
						}
					}
				}
			}
			catch (Exception ex3)
			{
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, ex3.Message);
				LogSystem.Warn(ex3);
			}
			return electronicBillResult;
		}

		private bool CheckHoaDonBanHang(string serial)
		{
			bool result = false;
			try
			{
				if (string.IsNullOrWhiteSpace(serial))
				{
					return result;
				}
				if (serial.Contains("2/"))
				{
					result = true;
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Warn(ex);
			}
			return result;
		}

		private string GetFirstCharUpper(string data)
		{
			string text = "";
			if (!string.IsNullOrWhiteSpace(data))
			{
				string[] array = data.Split(' ');
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!string.IsNullOrWhiteSpace(text2))
					{
						text += text2.First().ToString().ToUpper();
					}
				}
			}
			return text;
		}

		private string GetNumOrder(string p)
		{
			string result = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(p) && p.Substring(0, 2).ToLower() == "ok")
				{
					string text = p.Split(':').Last();
					if (!string.IsNullOrWhiteSpace(text))
					{
						string text2 = text.Split(';').Last();
						if (!string.IsNullOrWhiteSpace(text2))
						{
							string text3 = text2.Split('-').Last();
							if (!string.IsNullOrWhiteSpace(text3))
							{
								string text4 = text3.Split(',').Last();
								if (!string.IsNullOrWhiteSpace(text4))
								{
									result = text4.Split('_').Last();
								}
							}
						}
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

		private int GetCmdType(string TypeStr, string cancelFunc, string typeInvoice)
		{
			int result = -1;
			try
			{
				switch (ElectronicBillTypeEnum)
				{
				case ElectronicBillType.ENUM.CREATE_INVOICE:
					if (typeInvoice == "1")
					{
						ElectronicBillDataInput electronicBillDataInput = ElectronicBillDataInput;
						object value;
						if (electronicBillDataInput == null)
						{
							value = null;
						}
						else
						{
							HIS_TRANSACTION transaction = electronicBillDataInput.Transaction;
							value = ((transaction != null) ? transaction.INVOICE_CODE : null);
						}
						bool flag = !string.IsNullOrWhiteSpace((string)value);
						ElectronicBillDataInput electronicBillDataInput2 = ElectronicBillDataInput;
						object value2;
						if (electronicBillDataInput2 == null)
						{
							value2 = null;
						}
						else
						{
							HIS_TRANSACTION transaction2 = electronicBillDataInput2.Transaction;
							value2 = ((transaction2 != null) ? transaction2.EINVOICE_NUM_ORDER : null);
						}
						bool flag2 = !string.IsNullOrWhiteSpace((string)value2);
						if (!flag)
						{
							result = 112;
						}
						else if (flag && !flag2)
						{
							result = 113;
						}
					}
					else
					{
						result = 100;
						if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.IS_ADJUSTMENT == 1)
						{
							result = 111;
						}
						else if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue)
						{
							result = 110;
						}
					}
					break;
				case ElectronicBillType.ENUM.GET_INVOICE_LINK:
					result = 101;
					if (TypeStr == "1" || TypeStr == "2")
					{
						result = 103;
					}
					break;
				case ElectronicBillType.ENUM.DELETE_INVOICE:
				case ElectronicBillType.ENUM.CANCEL_INVOICE:
					result = 102;
					if (TypeStr == "1" || cancelFunc == "1")
					{
						result = 107;
					}
					break;
				case ElectronicBillType.ENUM.CONVERT_INVOICE:
					result = 106;
					break;
				case ElectronicBillType.ENUM.GET_INVOICE_INFO:
					result = 108;
					break;
				case ElectronicBillType.ENUM.CREATE_INVOICE_DATA:
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

        private bool Check(ref HIS.Desktop.Plugins.Library.ElectronicBill.Base.ElectronicBillResult electronicBillResult)
		{
			bool result = true;
			try
			{
				string[] array = serviceConfig.Split('|');
				if (array.Length < 7)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
				}
				if (string.IsNullOrEmpty(accountConfig))
				{
					throw new Exception("Không có cấu hình tài khoản");
				}
				string[] array2 = accountConfig.Split('|');
				if (array2.Length != 2)
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
					if (array.Length > 9 && array[9] == "2")
					{
						if (string.IsNullOrWhiteSpace(ElectronicBillDataInput.Branch.TAX_CODE))
						{
							throw new Exception("Không có thông tin mã số thuế của chi nhánh.");
						}
						if (string.IsNullOrWhiteSpace(ElectronicBillDataInput.Branch.ADDRESS))
						{
							throw new Exception("Không có thông tin địa chỉ của chi nhánh.");
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

		private string BillDataToXmlData(List<Invoice> _invoices)
		{
			string result = "";
			try
			{
				if (_invoices != null && _invoices.Count > 0)
				{
					XmlSerializer xmlSerializer = new XmlSerializer(_invoices.GetType(), new XmlRootAttribute("Invoices"));
					using (StringWriter stringWriter = new StringWriter())
					{
						using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
						{
							xmlSerializer.Serialize(xmlWriter, _invoices);
							result = stringWriter.ToString();
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = null;
				LogSystem.Warn(ex);
			}
			return result;
		}

		private List<Invoice> GetInvoiceContrVietSens(ElectronicBillDataInput electronicBillDataInput, bool notShowTaxBreakdown)
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Expected O, but got Unknown
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Expected O, but got Unknown
			List<Invoice> list = new List<Invoice>();
			if (electronicBillDataInput != null)
			{
				string[] array = serviceConfig.Split('|');
				if (array.Length < 7)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
				}
				if (string.IsNullOrEmpty(accountConfig))
				{
					throw new Exception("Không có cấu hình tài khoản");
				}

                string treatmentCode = ""; // nambg them
				Invoice val = new Invoice();
				string text = "";
                if (electronicBillDataInput.Treatment != null)
                {
                    treatmentCode = electronicBillDataInput.Treatment.TREATMENT_CODE;
                }
				if (electronicBillDataInput.Transaction != null && !string.IsNullOrWhiteSpace(electronicBillDataInput.Transaction.TRANSACTION_CODE))
				{
					text = electronicBillDataInput.Transaction.TRANSACTION_CODE;
				}
				else if (electronicBillDataInput.ListTransaction != null && electronicBillDataInput.ListTransaction.Count > 0)
				{
					text = ElectronicBillDataInput.ListTransaction.OrderBy((V_HIS_TRANSACTION o) => o.TRANSACTION_CODE).First().TRANSACTION_CODE;
				}
				else
				{
					text = string.Format("{0}-{1}", Inventec.Common.DateTime.Get.Now(), Guid.NewGuid().ToString());
					if (text.Length > 20)
					{
						text = text.Substring(0, 20);
					}
				}
				InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
				val.Key = text;
				val.InvoiceDetail = new InvoiceDetail();
				val.InvoiceDetail.Extra = val.Key;
                val.InvoiceDetail.CusCode = data.BuyerCode + "/" + (!String.IsNullOrEmpty(treatmentCode) ? treatmentCode : (Inventec.Common.DateTime.Get.Now() ?? 0).ToString());
				val.InvoiceDetail.CusAddress = data.BuyerAddress ?? " ";
				val.InvoiceDetail.CusPhone = data.BuyerPhone ?? "";
				val.InvoiceDetail.CusTaxCode = data.BuyerTaxCode ?? "";
				string text2 = electronicBillDataInput.PaymentMethod;
				if (electronicBillDataInput.Transaction != null)
				{
					HIS_PAY_FORM val2 = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == electronicBillDataInput.Transaction.PAY_FORM_ID);
					if (val2 != null)
					{
						text2 = val2.ELECTRONIC_PAY_FORM_NAME ?? val2.PAY_FORM_NAME;
					}
					if (array.Length > 9 && array[9] == "1")
					{
						val.InvoiceDetail.CusBankNo = electronicBillDataInput.Transaction.BUYER_ACCOUNT_NUMBER;
					}
				}
				val.InvoiceDetail.PaymentMethod = text2 ?? "";
				if (HisConfigCFG.IsSwapNameOption)
				{
					val.InvoiceDetail.Buyer = data.BuyerName ?? "";
					val.InvoiceDetail.CusName = data.BuyerOrganization ?? "";
				}
				else
				{
					val.InvoiceDetail.Buyer = data.BuyerOrganization ?? "";
					val.InvoiceDetail.CusName = data.BuyerName ?? "";
				}
				val.InvoiceDetail.CurrencyUnit = "VND";
				decimal totalAmount = default(decimal);
				decimal VATRateMax = -1m;
				decimal SumVATAmount = default(decimal);
				val.InvoiceDetail.Products = GetProductElectronicBill(electronicBillDataInput, notShowTaxBreakdown, ref totalAmount, ref SumVATAmount, ref VATRateMax);
				totalAmount = Math.Round(totalAmount - electronicBillDataInput.Discount.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero);
				val.InvoiceDetail.Total = string.Format("{0:0.####}", totalAmount - SumVATAmount);
				if (notShowTaxBreakdown)
				{
					val.InvoiceDetail.VATAmount = "";
					val.InvoiceDetail.VATRate = "-1";
				}
				else
				{
					val.InvoiceDetail.VATAmount = ((SumVATAmount > 0m) ? SumVATAmount.ToString() : "");
					val.InvoiceDetail.VATRate = VATRateMax.ToString();
				}
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

		private List<Product> GetProductElectronicBill(ElectronicBillDataInput dataInput, bool notShowTaxBreakdown, ref decimal totalAmount, ref decimal SumVATAmount, ref decimal VATRateMax)
		{
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_0618: Expected O, but got Unknown
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Expected O, but got Unknown
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Expected O, but got Unknown
			List<Product> list = new List<Product>();
			ElectronicBillDataInput electronicBillDataInput = null;
			if (HisConfigCFG.IsSplitServicesWithVat && ElectronicBillDataInput.SereServs.Exists((V_HIS_SERE_SERV_5 o) => o.VAT_RATIO > 0m))
			{
				electronicBillDataInput = new ElectronicBillDataInput(ElectronicBillDataInput);
				electronicBillDataInput.SereServs = TemplateFactory.GetSereServWithVAT(ElectronicBillDataInput);
			}
			else
			{
				dataInput.SereServs = ElectronicBillDataInput.SereServs;
				dataInput.SereServBill = ElectronicBillDataInput.SereServBill;
			}
			IRunTemplate runTemplate = TemplateFactory.MakeIRun(TempType, dataInput);
			object obj = runTemplate.Run();
			object obj2 = ((electronicBillDataInput != null) ? TemplateFactory.MakeIRun(TemplateEnum.TYPE.TemplateNhaThuoc, electronicBillDataInput).Run() : null);
			if (obj == null && obj2 == null)
			{
				throw new Exception("Không có thông tin chi tiết dịch vụ.");
			}
			if (obj != null)
			{
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
						val.ProdUnit = ((!string.IsNullOrWhiteSpace(item.ProdUnit)) ? item.ProdUnit : "");
						val.ProdQuantity = item.ProdQuantity.GetValueOrDefault().ToString();
						val.Amount = Math.Round(item.Amount, 0, MidpointRounding.AwayFromZero).ToString();
						val.VATAmount = "";
						val.VATRate = "-1";
						val.Total = Math.Round(item.Amount, 0, MidpointRounding.AwayFromZero).ToString();
						totalAmount += item.Amount;
						val.ProdPrice = string.Format("{0:0.####}", item.ProdPrice.GetValueOrDefault());
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
						Product val2 = new Product();
						val2.ProdName = item2.ProdName;
						val2.ProdUnit = ((!string.IsNullOrWhiteSpace(item2.ProdUnit)) ? item2.ProdUnit : "");
						val2.ProdQuantity = item2.ProdQuantity.GetValueOrDefault().ToString();
						val2.Amount = Math.Round(item2.Amount, 0, MidpointRounding.AwayFromZero).ToString();
						if (notShowTaxBreakdown)
						{
							val2.ProdPrice = string.Format("{0:0.####}", Math.Round(item2.Amount / item2.ProdQuantity.GetValueOrDefault(), 4).ToString() ?? "");
							val2.VATAmount = "";
							val2.VATRate = "-1";
							val2.Total = Math.Round(item2.Amount, 0).ToString();
						}
						else
						{
							val2.VATAmount = Math.Round(item2.TaxAmount.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero).ToString();
							if (!item2.TaxPercentage.HasValue)
							{
								val2.VATRate = "-1";
							}
							else if (item2.TaxPercentage == 0)
							{
								val2.VATRate = "0";
							}
							else if (item2.TaxPercentage == 1)
							{
								val2.VATRate = "5";
							}
							else if (item2.TaxPercentage == 2)
							{
								val2.VATRate = "10";
							}
							else if (item2.TaxPercentage == 3)
							{
								val2.VATRate = "8";
							}
							if (string.IsNullOrWhiteSpace(val2.VATRate))
							{
								val2.VATRate = ((long)item2.TaxConvert).ToString();
							}
							val2.ProdPrice = string.Format("{0:0.####}", item2.ProdPrice.GetValueOrDefault());
							val2.Total = Math.Round(item2.AmountWithoutTax.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero).ToString();
							SumVATAmount += item2.TaxAmount.GetValueOrDefault();
						}
						totalAmount += item2.Amount;
						list.Add(val2);
					}
				}
			}
			if (obj2 != null)
			{
				List<ProductBasePlus> list4 = (List<ProductBasePlus>)obj2;
				if (list4 == null || list4.Count == 0)
				{
					throw new Exception("Không có thông tin chi tiết dịch vụ.");
				}
				foreach (ProductBasePlus item3 in list4)
				{
					Product val3 = new Product();
					val3.ProdName = item3.ProdName;
					val3.ProdUnit = ((!string.IsNullOrWhiteSpace(item3.ProdUnit)) ? item3.ProdUnit : "");
					val3.ProdQuantity = item3.ProdQuantity.GetValueOrDefault().ToString();
					val3.Amount = Math.Round(item3.Amount, 0, MidpointRounding.AwayFromZero).ToString();
					if (notShowTaxBreakdown)
					{
						val3.ProdPrice = string.Format("{0:0.####}", Math.Round(item3.ProdPrice.GetValueOrDefault() + ((item3.ProdQuantity.HasValue && item3.ProdQuantity.Value > 0m) ? (item3.TaxAmount.GetValueOrDefault() / item3.ProdQuantity.GetValueOrDefault()) : 0m), 4).ToString() ?? "");
						val3.VATAmount = "";
						val3.VATRate = "-1";
					}
					else
					{
						val3.VATAmount = Math.Round(item3.TaxAmount.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero).ToString();
						if (!item3.TaxPercentage.HasValue)
						{
							val3.VATRate = "-1";
						}
						else if (item3.TaxPercentage == 0)
						{
							val3.VATRate = "0";
						}
						else if (item3.TaxPercentage == 1)
						{
							val3.VATRate = "5";
						}
						else if (item3.TaxPercentage == 2)
						{
							val3.VATRate = "10";
						}
						else if (item3.TaxPercentage == 3)
						{
							val3.VATRate = "8";
						}
						if (string.IsNullOrWhiteSpace(val3.VATRate))
						{
							val3.VATRate = ((long)item3.TaxConvert).ToString();
						}
						val3.ProdPrice = string.Format("{0:0.####}", item3.ProdPrice.GetValueOrDefault());
					}
					val3.Total = Math.Round(item3.Amount, 0, MidpointRounding.AwayFromZero).ToString();
					totalAmount += item3.Amount;
					SumVATAmount += item3.TaxAmount.GetValueOrDefault();
					list.Add(val3);
				}
			}
			totalAmount = Math.Round(totalAmount, 0, MidpointRounding.AwayFromZero);
			SumVATAmount = Math.Round(SumVATAmount, 0, MidpointRounding.AwayFromZero);
			VATRateMax = ((list != null && list.Count > 0) ? list.Max((Product o) => decimal.Parse(o.VATRate)) : (-1m));
			return list;
		}

		private List<HDon> GetInvoiceTT78(ElectronicBillDataInput electronicBillDataInput, bool notShowTaxBreakdown)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Expected O, but got Unknown
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Expected O, but got Unknown
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Expected O, but got Unknown
			//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dfd: Expected O, but got Unknown
			//IL_05af: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b6: Expected O, but got Unknown
			//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0add: Expected O, but got Unknown
			//IL_0927: Unknown result type (might be due to invalid IL or missing references)
			//IL_092e: Expected O, but got Unknown
			List<HDon> list = new List<HDon>();
			if (electronicBillDataInput != null)
			{
				HDon val = new HDon();
				string text = "";
				if (electronicBillDataInput.Transaction != null && !string.IsNullOrWhiteSpace(electronicBillDataInput.Transaction.TRANSACTION_CODE))
				{
					text = electronicBillDataInput.Transaction.TRANSACTION_CODE;
				}
				else if (electronicBillDataInput.ListTransaction != null && electronicBillDataInput.ListTransaction.Count > 0)
				{
					text = ElectronicBillDataInput.ListTransaction.OrderBy((V_HIS_TRANSACTION o) => o.TRANSACTION_CODE).First().TRANSACTION_CODE;
				}
				else
				{
					text = string.Format("{0}-{1}", Inventec.Common.DateTime.Get.Now(), Guid.NewGuid().ToString());
					if (text.Length > 20)
					{
						text = text.Substring(0, 20);
					}
				}
				val.Key = text;
				val.dLHDon = new DLHDon();
				val.dLHDon.tTChung = new TTChung();
				val.dLHDon.tTChung.DVTTe = "VND";
				val.dLHDon.tTChung.TGia = "1";
				string hTTToan = electronicBillDataInput.PaymentMethod;
				if (electronicBillDataInput.Transaction != null)
				{
					HIS_PAY_FORM val2 = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == electronicBillDataInput.Transaction.PAY_FORM_ID);
					if (val2 != null)
					{
						hTTToan = val2.ELECTRONIC_PAY_FORM_NAME ?? val2.PAY_FORM_NAME;
					}
				}
				val.dLHDon.tTChung.HTTToan = hTTToan;
				val.dLHDon.nDHDon = new NDHDon();
				val.dLHDon.nDHDon.nBan = new NBan();
				val.dLHDon.nDHDon.nBan.Ten = electronicBillDataInput.Branch.BRANCH_NAME;
				val.dLHDon.nDHDon.nBan.DChi = electronicBillDataInput.Branch.ADDRESS ?? ".";
				val.dLHDon.nDHDon.nBan.MST = electronicBillDataInput.Branch.TAX_CODE;
				val.dLHDon.nDHDon.nBan.SDThoai = electronicBillDataInput.Branch.PHONE ?? "";
				val.dLHDon.nDHDon.nBan.STKNHang = electronicBillDataInput.Branch.ACCOUNT_NUMBER ?? "";
				val.dLHDon.nDHDon.nBan.TNHang = electronicBillDataInput.Branch.BANK_INFO ?? "";
				InvoiceInfoADO data = InvoiceInfoProcessor.GetData(electronicBillDataInput);
				val.dLHDon.nDHDon.nMua = new NMua();
				if (HisConfigCFG.IsSwapNameOption)
				{
					val.dLHDon.nDHDon.nMua.Ten = data.BuyerName;
					val.dLHDon.nDHDon.nMua.HVTNMHang = data.BuyerOrganization;
				}
				else
				{
					val.dLHDon.nDHDon.nMua.Ten = data.BuyerOrganization;
					val.dLHDon.nDHDon.nMua.HVTNMHang = data.BuyerName;
				}
				val.dLHDon.nDHDon.nMua.DChi = data.BuyerAddress;
				val.dLHDon.nDHDon.nMua.MKHang = data.BuyerCode;
				val.dLHDon.nDHDon.nMua.MST = data.BuyerTaxCode;
				val.dLHDon.nDHDon.nMua.SDThoai = data.BuyerPhone;
				val.dLHDon.nDHDon.nMua.STKNHang = data.BuyerAccountNumber;
				val.dLHDon.nDHDon.hHDVu = new List<HHDVu>();
				decimal d = default(decimal);
				ElectronicBillDataInput electronicBillDataInput2 = null;
				if (HisConfigCFG.IsSplitServicesWithVat && ElectronicBillDataInput.SereServs.Exists((V_HIS_SERE_SERV_5 o) => o.VAT_RATIO > 0m))
				{
					electronicBillDataInput2 = new ElectronicBillDataInput(ElectronicBillDataInput);
					electronicBillDataInput2.SereServs = TemplateFactory.GetSereServWithVAT(ElectronicBillDataInput);
				}
				else
				{
					electronicBillDataInput.SereServs = ElectronicBillDataInput.SereServs;
					electronicBillDataInput.SereServBill = ElectronicBillDataInput.SereServBill;
				}
				IRunTemplate runTemplate = TemplateFactory.MakeIRun(TempType, electronicBillDataInput);
				object obj = runTemplate.Run();
				object obj2 = ((electronicBillDataInput2 != null) ? TemplateFactory.MakeIRun(TemplateEnum.TYPE.TemplateNhaThuoc, electronicBillDataInput2).Run() : null);
				if (obj == null && obj2 == null)
				{
					throw new Exception("Không có thông tin chi tiết dịch vụ.");
				}
				int num = 1;
				if (obj2 != null)
				{
					List<ProductBasePlus> list2 = (List<ProductBasePlus>)obj2;
					if (list2 == null || list2.Count == 0)
					{
						throw new Exception("Không có thông tin chi tiết dịch vụ.");
					}
					foreach (ProductBasePlus item in list2)
					{
						HHDVu val3 = new HHDVu();
						val3.STT = num.ToString() ?? "";
						val3.TChat = "1";
						val3.THHDVu = item.ProdName;
						val3.MHHDVu = item.ProdCode;
						val3.DVTinh = ((!string.IsNullOrWhiteSpace(item.ProdUnit)) ? item.ProdUnit : "");
						val3.SLuong = item.ProdQuantity.GetValueOrDefault().ToString();
						val3.DGia = string.Format("{0:0.####}", item.ProdPrice.GetValueOrDefault());
						if (notShowTaxBreakdown)
						{
							val3.ThTien = Math.Round(item.Amount, 0, MidpointRounding.AwayFromZero).ToString();
							val3.TSThue = Math.Round(item.Amount, 0, MidpointRounding.AwayFromZero).ToString();
							val3.TSuat = "";
							val3.TThue = "-1";
						}
						else
						{
							val3.ThTien = Math.Round(item.Amount, 0, MidpointRounding.AwayFromZero).ToString();
							val3.TSThue = Math.Round(item.AmountWithoutTax.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero).ToString();
							val3.TSuat = Math.Round(item.TaxAmount.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero).ToString();
							if (!item.TaxPercentage.HasValue)
							{
								val3.TThue = "-1";
							}
							else if (item.TaxPercentage == 0)
							{
								val3.TThue = "0";
							}
							else if (item.TaxPercentage == 1)
							{
								val3.TThue = "5";
							}
							else if (item.TaxPercentage == 2)
							{
								val3.TThue = "10";
							}
							else if (item.TaxPercentage == 3)
							{
								val3.TThue = "8";
							}
							if (string.IsNullOrWhiteSpace(val3.TThue))
							{
								val3.TThue = ((long)item.TaxConvert).ToString();
							}
						}
						d += item.Amount;
						val.dLHDon.nDHDon.hHDVu.Add(val3);
						num++;
					}
				}
				if (obj != null)
				{
					if (TempType != TemplateEnum.TYPE.TemplateNhaThuoc)
					{
						List<ProductBase> list3 = (List<ProductBase>)obj;
						if (list3 == null || list3.Count == 0)
						{
							throw new Exception("Không có thông tin chi tiết dịch vụ.");
						}
						foreach (ProductBase item2 in list3)
						{
							HHDVu val4 = new HHDVu();
							val4.STT = num.ToString() ?? "";
							val4.TChat = "1";
							val4.THHDVu = item2.ProdName;
							val4.MHHDVu = item2.ProdCode;
							val4.DVTinh = ((!string.IsNullOrWhiteSpace(item2.ProdUnit)) ? item2.ProdUnit : "");
							val4.SLuong = item2.ProdQuantity.GetValueOrDefault().ToString();
							val4.ThTien = Math.Round(item2.Amount, 0, MidpointRounding.AwayFromZero).ToString();
							val4.TSThue = Math.Round(item2.Amount, 0, MidpointRounding.AwayFromZero).ToString();
							val4.DGia = string.Format("{0:0.####}", item2.ProdPrice.GetValueOrDefault());
							if (notShowTaxBreakdown)
							{
								val4.TSuat = "";
								val4.TThue = "-1";
							}
							d += item2.Amount;
							val.dLHDon.nDHDon.hHDVu.Add(val4);
							num++;
						}
					}
					else
					{
						List<ProductBasePlus> list4 = (List<ProductBasePlus>)obj;
						if (list4 == null || list4.Count == 0)
						{
							throw new Exception("Không có thông tin chi tiết dịch vụ.");
						}
						foreach (ProductBasePlus item3 in list4)
						{
							HHDVu val5 = new HHDVu();
							val5.STT = num.ToString() ?? "";
							val5.TChat = "1";
							val5.THHDVu = item3.ProdName;
							val5.MHHDVu = item3.ProdCode;
							val5.DVTinh = ((!string.IsNullOrWhiteSpace(item3.ProdUnit)) ? item3.ProdUnit : "");
							val5.SLuong = item3.ProdQuantity.GetValueOrDefault().ToString();
							val5.DGia = string.Format("{0:0.####}", item3.ProdPrice.GetValueOrDefault());
							if (notShowTaxBreakdown)
							{
								val5.ThTien = Math.Round(item3.Amount, 0, MidpointRounding.AwayFromZero).ToString();
								val5.TSThue = Math.Round(item3.Amount, 0, MidpointRounding.AwayFromZero).ToString();
								val5.TSuat = "";
								val5.TThue = "-1";
							}
							else
							{
								val5.ThTien = Math.Round(item3.Amount, 0, MidpointRounding.AwayFromZero).ToString();
								val5.TSThue = Math.Round(item3.AmountWithoutTax.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero).ToString();
								val5.TSuat = Math.Round(item3.TaxAmount.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero).ToString();
								if (!item3.TaxPercentage.HasValue)
								{
									val5.TThue = "-1";
								}
								else if (item3.TaxPercentage == 0)
								{
									val5.TThue = "0";
								}
								else if (item3.TaxPercentage == 1)
								{
									val5.TThue = "5";
								}
								else if (item3.TaxPercentage == 2)
								{
									val5.TThue = "10";
								}
								else if (item3.TaxPercentage == 3)
								{
									val5.TThue = "8";
								}
								if (string.IsNullOrWhiteSpace(val5.TThue))
								{
									val5.TThue = ((long)item3.TaxConvert).ToString();
								}
							}
							d += item3.Amount;
							val.dLHDon.nDHDon.hHDVu.Add(val5);
							num++;
						}
					}
				}
				val.dLHDon.nDHDon.tToan = new TToan();
				d = Math.Round(d, 0, MidpointRounding.AwayFromZero);
				val.dLHDon.nDHDon.tToan.TgTCThue = val.dLHDon.nDHDon.hHDVu.Sum((HHDVu s) => Parse.ToDecimal(s.ThTien)).ToString() ?? "";
				if (notShowTaxBreakdown)
				{
					val.dLHDon.nDHDon.tToan.TgTThue = "-1";
				}
				else
				{
					val.dLHDon.nDHDon.tToan.TgTThue = val.dLHDon.nDHDon.hHDVu.Sum((HHDVu s) => Parse.ToDecimal(s.TThue)).ToString() ?? "";
				}
				val.dLHDon.nDHDon.tToan.TgTTTBSo = d.ToString() ?? "";
				val.dLHDon.nDHDon.tToan.TgTTTBChu = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", Inventec.Common.Number.Convert.NumberToNumberRoundMax4(d))) + "đồng";
				if (electronicBillDataInput.Discount.HasValue && electronicBillDataInput.Discount.Value > 0m)
				{
					val.dLHDon.nDHDon.tToan.TTCKTMai = electronicBillDataInput.Discount.ToString() ?? "";
				}
				list.Add(val);
			}
			return list;
		}
	}
}
