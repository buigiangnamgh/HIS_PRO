using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.ElectronicBillViettel;
using Inventec.Common.ElectronicBillViettel.Model;
using Inventec.Common.Logging;
using Inventec.Common.Number;
using Inventec.Common.String;
using Inventec.Core;
using Inventec.Fss.Client;
using Inventec.Fss.Utility;
using Inventec.UC.Login.Base;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;
using Newtonsoft.Json;
using SDA.EFMODEL.DataModels;
using SDA.Filter;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.VIETTEL
{
    internal class VIETTELBehavior : HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass7_0
		{
			public Response response;

			public CancelInvoice invoiceData;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass8_0
		{
			public Response response;

			public GetFile invoiceData;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass10_0
		{
			public Response response;

			public GetInvoiceRepresentationFileData invoiceData;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass17_0
		{
			public DataCreateInvoice invoiceData;

			public Response response;
		}

		private ElectronicBillDataInput ElectronicBillDataInput;

		private string ServiceConfig;

		private string AccountConfig;

		private TemplateEnum.TYPE TempType;

		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public VIETTELBehavior(ElectronicBillDataInput electronicBillDataInput, string serviceConfig, string accountConfig)
		{
			ElectronicBillDataInput = electronicBillDataInput;
			ServiceConfig = serviceConfig;
			AccountConfig = accountConfig;
		}

        ElectronicBillResult HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun.Run(ElectronicBillType.ENUM _electronicBillTypeEnum, TemplateEnum.TYPE _tempType)
		{
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			try
			{
				if (Check(ref electronicBillResult))
				{
					TempType = _tempType;
					string[] array = ServiceConfig.Split('|');
					string text = array[1];
					if (string.IsNullOrEmpty(text))
					{
						LogSystem.Error("Khong tim thay dia chi Webservice URL");
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không tìm thấy địa chỉ Webservice URL");
						return electronicBillResult;
					}
					string text2 = "";
					if (array.Count() > 2)
					{
						text2 = array[2];
					}
					bool flag = false;
					if (array.Count() > 3)
					{
						flag = array[3] == "1";
						if (flag && _electronicBillTypeEnum != ElectronicBillType.ENUM.CREATE_INVOICE)
						{
							LogSystem.Error("Tính năng không được hỗ trợ khi phát hành hóa đơn nháp!");
							ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Tính năng không được hỗ trợ khi phát hành hóa đơn nháp!");
							return electronicBillResult;
						}
					}
					string[] array2 = AccountConfig.Split('|');
					DataInitApi val = new DataInitApi();
					val.VIETTEL_Address = text;
					val.User = array2[0].Trim();
					val.Pass = array2[1].Trim();
					val.IsTemp = flag;
					val.SupplierTaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
					if (text2 == "2")
					{
                        val.Version = (Inventec.Common.ElectronicBillViettel.Version)1;
					}
					switch (_electronicBillTypeEnum)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						ProcessCreateInvoice(val, ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
						if (HisConfigCFG.IsPrintNormal)
						{
							ProcessGetInvoiceNormal(val, ref electronicBillResult);
						}
						else
						{
							ProcessGetInvoice(val, ref electronicBillResult);
						}
						break;
					case ElectronicBillType.ENUM.DELETE_INVOICE:
					case ElectronicBillType.ENUM.CANCEL_INVOICE:
						ProcessCancelInvoice(val, ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.CREATE_INVOICE_DATA:
						ProcessCreateInvoiceSaveData(val, ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.CONVERT_INVOICE:
						ProcessGetInvoice(val, ref electronicBillResult);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				electronicBillResult.Success = false;
				LogSystem.Warn(ex);
			}
			return electronicBillResult;
		}

		private void ProcessCreateInvoiceSaveData(DataInitApi viettelLogin, ref ElectronicBillResult result)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(ElectronicBillDataInput.SaveFileName))
				{
					if (File.Exists(ElectronicBillDataInput.SaveFileName))
					{
						File.Delete(ElectronicBillDataInput.SaveFileName);
					}
					DataCreateInvoice val = CreateInvoice(ElectronicBillDataInput, viettelLogin);
					string value = JsonConvert.SerializeObject((object)val);
					using (StreamWriter streamWriter = new StreamWriter(ElectronicBillDataInput.SaveFileName))
					{
						streamWriter.Write(value);
					}
					ElectronicBillResultUtil.Set(ref result, true, "");
				}
				else
				{
					ElectronicBillResultUtil.Set(ref result, false, "Không có đường dẫn lưu file");
				}
			}
			catch (Exception ex)
			{
				result.Success = false;
				LogSystem.Error(ex);
			}
		}

		private void ProcessCancelInvoice(DataInitApi viettelLogin, ref ElectronicBillResult result)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Invalid comparison between Unknown and I4
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Expected O, but got Unknown
			try
			{
				_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals23 = new _003C_003Ec__DisplayClass7_0();
				CS_0024_003C_003E8__locals23.invoiceData = new CancelInvoice();
				CS_0024_003C_003E8__locals23.invoiceData.additionalReferenceDesc = ElectronicBillDataInput.CancelReason;
				if (!string.IsNullOrWhiteSpace(ElectronicBillDataInput.ENumOrder) && ElectronicBillDataInput.ENumOrder.Length >= 7)
				{
					CS_0024_003C_003E8__locals23.invoiceData.invoiceNo = ElectronicBillDataInput.ENumOrder;
				}
				else
				{
					CS_0024_003C_003E8__locals23.invoiceData.invoiceNo = ElectronicBillDataInput.SymbolCode + GetVirNumOrder(long.Parse(ElectronicBillDataInput.InvoiceCode.Split('|').Last()));
				}
				if ((int)viettelLogin.Version == 1)
				{
					CS_0024_003C_003E8__locals23.invoiceData.strIssueDate = GetTimeMilliseconds(ElectronicBillDataInput.TransactionTime).ToString() ?? "";
					CS_0024_003C_003E8__locals23.invoiceData.additionalReferenceDate = GetTimeMilliseconds(ElectronicBillDataInput.CancelTime.GetValueOrDefault()).ToString() ?? "";
				}
				else
				{
					CS_0024_003C_003E8__locals23.invoiceData.additionalReferenceDate = ElectronicBillDataInput.CancelTime.ToString();
					CS_0024_003C_003E8__locals23.invoiceData.strIssueDate = ElectronicBillDataInput.TransactionTime.ToString();
				}
				CS_0024_003C_003E8__locals23.invoiceData.supplierTaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
				CS_0024_003C_003E8__locals23.invoiceData.templateCode = ElectronicBillDataInput.TemplateCode;
				CS_0024_003C_003E8__locals23.response = null;
				ElectronicBillViettelManager val = new ElectronicBillViettelManager(viettelLogin);
				if (val != null)
				{
					CS_0024_003C_003E8__locals23.response = val.Run((object)CS_0024_003C_003E8__locals23.invoiceData);
				}
				if (CS_0024_003C_003E8__locals23.response != null && string.IsNullOrWhiteSpace(CS_0024_003C_003E8__locals23.response.errorCode))
				{
					result.Success = true;
					result.InvoiceSys = "VIETEL";
					result.Messages = new List<string> { CS_0024_003C_003E8__locals23.response.description };
					return;
				}
				result.Success = false;
				result.InvoiceSys = "VIETEL";
				if (CS_0024_003C_003E8__locals23.response != null)
				{
					result.Messages = new List<string> { CS_0024_003C_003E8__locals23.response.description };
                    Inventec.Common.Logging.LogSystem.Error("Huy hoa don dien tu that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals23.response), CS_0024_003C_003E8__locals23.response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals23.invoiceData), CS_0024_003C_003E8__locals23.invoiceData));
					ElectronicBillResultUtil.Set(ref result, false, CS_0024_003C_003E8__locals23.response.description);
				}
				else
				{
					ElectronicBillResultUtil.Set(ref result, false, "");
				}
			}
			catch (Exception ex)
			{
				result.Success = false;
				LogSystem.Error(ex);
			}
		}

		private void ProcessGetInvoice(DataInitApi viettelLogin, ref ElectronicBillResult result)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Invalid comparison between Unknown and I4
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Expected O, but got Unknown
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Invalid comparison between Unknown and I4
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0553: Invalid comparison between Unknown and I4
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0571: Expected O, but got Unknown
			try
			{
				_003C_003Ec__DisplayClass8_0 CS_0024_003C_003E8__locals34 = new _003C_003Ec__DisplayClass8_0();
				CS_0024_003C_003E8__locals34.invoiceData = new GetFile();
				string exchangeUser = ClientTokenManagerStore.ClientTokenManager.GetUserName();
				if (!string.IsNullOrWhiteSpace(ElectronicBillDataInput.Converter))
				{
					exchangeUser = ElectronicBillDataInput.Converter;
				}
				CS_0024_003C_003E8__locals34.invoiceData.exchangeUser = exchangeUser;
				if ((int)viettelLogin.Version == 1)
				{
					CS_0024_003C_003E8__locals34.invoiceData.strIssueDate = GetTimeMilliseconds(ElectronicBillDataInput.TransactionTime).ToString() ?? "";
				}
				else
				{
					CS_0024_003C_003E8__locals34.invoiceData.strIssueDate = ElectronicBillDataInput.TransactionTime.ToString();
				}
				CS_0024_003C_003E8__locals34.invoiceData.supplierTaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
				CS_0024_003C_003E8__locals34.invoiceData.templateCode = ElectronicBillDataInput.TemplateCode;
				if (!string.IsNullOrWhiteSpace(ElectronicBillDataInput.ENumOrder) && ElectronicBillDataInput.ENumOrder.Length >= 7)
				{
					CS_0024_003C_003E8__locals34.invoiceData.invoiceNo = ElectronicBillDataInput.ENumOrder;
				}
				else
				{
					CS_0024_003C_003E8__locals34.invoiceData.invoiceNo = ElectronicBillDataInput.SymbolCode + GetVirNumOrder(long.Parse(ElectronicBillDataInput.InvoiceCode.Split('|').Last()));
				}
				CS_0024_003C_003E8__locals34.response = null;
				ElectronicBillViettelManager val = new ElectronicBillViettelManager(viettelLogin);
				if (val != null)
				{
					CS_0024_003C_003E8__locals34.response = val.Run((object)CS_0024_003C_003E8__locals34.invoiceData);
				}
				string text = "";
				if (CS_0024_003C_003E8__locals34.response != null && string.IsNullOrWhiteSpace(CS_0024_003C_003E8__locals34.response.errorCode))
				{
					string text2 = ProcessPdfFileResult(CS_0024_003C_003E8__locals34.response.fileToBytes);
					LogSystem.Debug("_____PDF_FILE_NAME: " + text2);
					result.Success = true;
					result.InvoiceSys = "VIETEL";
					result.InvoiceLink = text2;
					if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.EINVOICE_LOGINNAME != viettelLogin.User)
					{
						text = viettelLogin.User;
					}
				}
				else if ((int)viettelLogin.Version == 1)
				{
					if (ElectronicBillDataInput.Transaction != null && !string.IsNullOrWhiteSpace(ElectronicBillDataInput.Transaction.EINVOICE_LOGINNAME))
					{
						CS_0024_003C_003E8__locals34.invoiceData.exchangeUser = ElectronicBillDataInput.Transaction.EINVOICE_LOGINNAME;
					}
					else if (ElectronicBillDataInput.Transaction != null)
					{
						CS_0024_003C_003E8__locals34.invoiceData.exchangeUser = GetExchangeUser(ElectronicBillDataInput.Transaction.CASHIER_LOGINNAME);
					}
					if (string.IsNullOrWhiteSpace(CS_0024_003C_003E8__locals34.invoiceData.exchangeUser))
					{
						CS_0024_003C_003E8__locals34.invoiceData.exchangeUser = viettelLogin.User;
					}
					CS_0024_003C_003E8__locals34.response = val.Run((object)CS_0024_003C_003E8__locals34.invoiceData);
					if (CS_0024_003C_003E8__locals34.response != null && string.IsNullOrWhiteSpace(CS_0024_003C_003E8__locals34.response.errorCode))
					{
						string text3 = ProcessPdfFileResult(CS_0024_003C_003E8__locals34.response.fileToBytes);
						LogSystem.Debug("_____PDF_FILE_NAME: " + text3);
						result.Success = true;
						result.InvoiceSys = "VIETEL";
						result.InvoiceLink = text3;
						if (ElectronicBillDataInput.Transaction == null || ElectronicBillDataInput.Transaction.EINVOICE_LOGINNAME != viettelLogin.User)
						{
							text = CS_0024_003C_003E8__locals34.invoiceData.exchangeUser;
						}
					}
					else
					{
						result.Success = false;
						result.InvoiceSys = "VIETEL";
                        Inventec.Common.Logging.LogSystem.Error("lay file chuyen doi that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals34.response), CS_0024_003C_003E8__locals34.response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals34.invoiceData), CS_0024_003C_003E8__locals34.invoiceData));
						ElectronicBillResultUtil.Set(ref result, false, CS_0024_003C_003E8__locals34.response.description);
					}
				}
				else
				{
					result.Success = false;
					result.InvoiceSys = "VIETEL";

                    Inventec.Common.Logging.LogSystem.Error("lay file chuyen doi that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals34.response), CS_0024_003C_003E8__locals34.response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals34.invoiceData), CS_0024_003C_003E8__locals34.invoiceData));
					ElectronicBillResultUtil.Set(ref result, false, CS_0024_003C_003E8__locals34.response.description);
				}
				if (result.Success = !string.IsNullOrWhiteSpace(text) && (int)viettelLogin.Version == 1)
				{
					HisTransactionInvoiceUrlSDO val2 = new HisTransactionInvoiceUrlSDO();
					val2.InvoiceCode = ElectronicBillDataInput.InvoiceCode;
					val2.Loginname = text;
					CreatThreadUpdateDataLoginname(val2);
				}
			}
			catch (Exception ex)
			{
				result.Success = false;
				LogSystem.Error(ex);
			}
		}

		private string GetExchangeUser(string cashierLoginname)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			string result = "";
			try
			{
				CommonParam val = new CommonParam();
				SdaConfigAppUserViewFilter val2 = new SdaConfigAppUserViewFilter();
				val2.LOGINNAME_EXACT = cashierLoginname;
				if (ElectronicBillDataInput.EinvoiceTypeId == 2)
				{
					val2.KEY__EXACT = "CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__VIETEL";
				}
				else
				{
					val2.KEY__EXACT = "CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT";
				}
				List<V_SDA_CONFIG_APP_USER> list = ((AdapterBase)new BackendAdapter(val)).Get<List<V_SDA_CONFIG_APP_USER>>("/api/SdaConfigAppUser/GetView", ApiConsumers.SdaConsumer, (object)val2, val);
				if (list != null && list.Count > 0)
				{
					foreach (V_SDA_CONFIG_APP_USER item in list)
					{
						string text = item.VALUE ?? item.DEFAULT_VALUE;
						if ((item.KEY == "CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT__VIETEL" || item.KEY == "CONFIG_KEY__HIS_DESKTOP__ELECTRONIC_BILL__ACCOUNT") && !string.IsNullOrWhiteSpace(text))
						{
							string[] array = text.Split('|');
							if (array.Length >= 2)
							{
								result = array[0].Trim();
								break;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private void ProcessGetInvoiceNormal(DataInitApi viettelLogin, ref ElectronicBillResult result)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			try
			{
				_003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals17 = new _003C_003Ec__DisplayClass10_0();
				CS_0024_003C_003E8__locals17.invoiceData = new GetInvoiceRepresentationFileData();
				CS_0024_003C_003E8__locals17.invoiceData.fileType = "PDF";
				CS_0024_003C_003E8__locals17.invoiceData.supplierTaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
				CS_0024_003C_003E8__locals17.invoiceData.templateCode = ElectronicBillDataInput.TemplateCode;
				if (!string.IsNullOrWhiteSpace(ElectronicBillDataInput.ENumOrder) && ElectronicBillDataInput.ENumOrder.Length >= 7)
				{
					CS_0024_003C_003E8__locals17.invoiceData.invoiceNo = ElectronicBillDataInput.ENumOrder;
				}
				else
				{
					CS_0024_003C_003E8__locals17.invoiceData.invoiceNo = ElectronicBillDataInput.SymbolCode + GetVirNumOrder(long.Parse(ElectronicBillDataInput.InvoiceCode.Split('|').Last()));
				}
				CS_0024_003C_003E8__locals17.response = null;
				ElectronicBillViettelManager val = new ElectronicBillViettelManager(viettelLogin);
				if (val != null)
				{
					CS_0024_003C_003E8__locals17.response = val.Run((object)CS_0024_003C_003E8__locals17.invoiceData);
				}
				if (CS_0024_003C_003E8__locals17.response != null && string.IsNullOrWhiteSpace(CS_0024_003C_003E8__locals17.response.errorCode))
				{
					string text = ProcessPdfFileResult(CS_0024_003C_003E8__locals17.response.fileToBytes);
					LogSystem.Debug("_____PDF_FILE_NAME: " + text);
					result.Success = true;
					result.InvoiceSys = "VIETEL";
					result.InvoiceLink = text;
				}
				else
				{
					result.Success = false;
					result.InvoiceSys = "VIETEL";
					
                    Inventec.Common.Logging.LogSystem.Error("lay file that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals17.response), CS_0024_003C_003E8__locals17.response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals17.invoiceData), CS_0024_003C_003E8__locals17.invoiceData));
					ElectronicBillResultUtil.Set(ref result, false, CS_0024_003C_003E8__locals17.response.description);
				}
			}
			catch (Exception ex)
			{
				result.Success = false;
				LogSystem.Error(ex);
			}
		}

		private void CreatThreadUpdateDataLoginname(HisTransactionInvoiceUrlSDO sdo)
		{
			Thread thread = new Thread(ProcessUpdateDataLoginname);
			try
			{
				thread.Start(sdo);
			}
			catch (Exception ex)
			{
				thread.Abort();
				LogSystem.Error(ex);
			}
		}

		private void ProcessUpdateDataLoginname(object obj)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (obj == null)
				{
					return;
				}
				CommonParam param = new CommonParam();
				if (!((AdapterBase)new BackendAdapter(param)).Post<bool>("/api/HisTransaction/UpdateEInvoiceUrl", ApiConsumers.MosConsumer, obj, param))
				{
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<object>((Expression<Func<object>>)(() => obj)), obj));
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<CommonParam>((Expression<Func<CommonParam>>)(() => param)), (object)param));
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void CreatThreadSaveData(string p, Response response)
		{
			Thread thread = new Thread(ProcessSaveFile);
			List<object> list = new List<object>();
			list.Add(p);
			list.Add(response);
			try
			{
				thread.Start(list);
			}
			catch (Exception ex)
			{
				thread.Abort();
				LogSystem.Error(ex);
			}
		}

		private void ProcessSaveFile(object obj)
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			if (obj == null)
			{
				return;
			}
			bool flag = false;
			List<object> list = obj as List<object>;
			string invoiceCode = "";
			Response val = null;
			foreach (object item in list)
			{
				if (item.GetType() == typeof(string))
				{
					invoiceCode = (string)item;
				}
				else if (item.GetType() == typeof(Response))
				{
					val = (Response)item;
				}
			}
			try
			{
				MemoryStream memoryStream = new MemoryStream(val.fileToBytes);
				FileUploadInfo val2 = FileUpload.UploadFile(GlobalVariables.APPLICATION_CODE, "INVOICE", memoryStream, val.fileName, true);
				if (val2 != null)
				{
					CommonParam param = new CommonParam();
					HisTransactionInvoiceUrlSDO sdo = new HisTransactionInvoiceUrlSDO();
					sdo.InvoiceCode = invoiceCode;
					sdo.Url = val2.Url;
					if (!((AdapterBase)new BackendAdapter(param)).Post<bool>("/api/HisTransaction/UpdateEInvoiceUrl", ApiConsumers.MosConsumer, (object)sdo, param))
					{
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<HisTransactionInvoiceUrlSDO>((Expression<Func<HisTransactionInvoiceUrlSDO>>)(() => sdo)), (object)sdo));
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<CommonParam>((Expression<Func<CommonParam>>)(() => param)), (object)param));
					}
				}
				else
				{
					flag = true;
				}
			}
			catch (Exception ex)
			{
				flag = true;
				LogSystem.Error(ex);
			}
			if (flag)
			{
				LogSystem.Error("Đẩy file lên fss thất bại. Lưu lại file trên máy người dùng");
				string text = ProcessPdfFileResult(val.fileToBytes);
				LogSystem.Error(text);
			}
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

		private bool WriteByteArrayToFile(byte[] byData, string fileName)
		{
			bool result = true;
			try
			{
				using (FileStream fileStream = File.Open(fileName, FileMode.CreateNew))
				{
					fileStream.Write(byData, 0, byData.Length);
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Error(ex);
			}
			return result;
		}

		private void ProcessCreateInvoice(DataInitApi viettelLogin, ref ElectronicBillResult result)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Invalid comparison between Unknown and I4
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Expected O, but got Unknown
			try
			{
				_003C_003Ec__DisplayClass17_0 CS_0024_003C_003E8__locals27 = new _003C_003Ec__DisplayClass17_0();
				CS_0024_003C_003E8__locals27.invoiceData = CreateInvoice(ElectronicBillDataInput, viettelLogin);
				CS_0024_003C_003E8__locals27.response = null;
				LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<DataCreateInvoice>((Expression<Func<DataCreateInvoice>>)(() => CS_0024_003C_003E8__locals27.invoiceData)), (object)CS_0024_003C_003E8__locals27.invoiceData));
				ElectronicBillViettelManager val = new ElectronicBillViettelManager(viettelLogin);
				if (val != null)
				{
					CS_0024_003C_003E8__locals27.response = val.Run((object)CS_0024_003C_003E8__locals27.invoiceData);
				}
				if (CS_0024_003C_003E8__locals27.response != null && string.IsNullOrWhiteSpace(CS_0024_003C_003E8__locals27.response.errorCode) && CS_0024_003C_003E8__locals27.response.result != null)
				{
					if (viettelLogin != null && (int)viettelLogin.Version == 1)
					{
						Thread.Sleep(500);
						LogSystem.Debug("Gọi lại api lấy thông tin hóa đơn sau 0.5s");
						GetFile val2 = new GetFile();
						val2.supplierTaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
						val2.transactionUuid = CS_0024_003C_003E8__locals27.invoiceData.generalInvoiceInfo.transactionUuid;
						Response val3 = null;
						if (val != null)
						{
							val3 = val.Run((object)val2);
						}
						if (val3 != null && string.IsNullOrWhiteSpace(val3.errorCode) && val3.result != null)
						{
							long numOrder = GetNumOrder(val3.result.invoiceNo);
							result.Success = true;
							result.InvoiceSys = "VIETEL";
							result.InvoiceCode = string.Format("{0}|{1}", val3.result.reservationCode ?? CS_0024_003C_003E8__locals27.response.result.reservationCode, numOrder);
							result.InvoiceNumOrder = val3.result.invoiceNo;
							result.InvoiceLoginname = viettelLogin.User;
							if (val3.result.issueDate > 0.0)
							{
								result.InvoiceTime = GetTimefromMilliseconds(val3.result.issueDate);
							}
							else
							{
								result.InvoiceTime = GetInvoiceTimeByResult(val, viettelLogin, val3.result, CS_0024_003C_003E8__locals27.invoiceData);
							}
						}
						else
						{
							long numOrder2 = GetNumOrder(CS_0024_003C_003E8__locals27.response.result.invoiceNo);
							result.Success = true;
							result.InvoiceSys = "VIETEL";
							result.InvoiceCode = string.Format("{0}|{1}", CS_0024_003C_003E8__locals27.response.result.reservationCode ?? "", numOrder2);
							result.InvoiceNumOrder = CS_0024_003C_003E8__locals27.response.result.invoiceNo;
							result.InvoiceTime = GetInvoiceTimeByResult(val, viettelLogin, CS_0024_003C_003E8__locals27.response.result, CS_0024_003C_003E8__locals27.invoiceData);
							result.InvoiceLoginname = viettelLogin.User;
						}
					}
					else
					{
						long numOrder3 = GetNumOrder(CS_0024_003C_003E8__locals27.response.result.invoiceNo);
						result.Success = true;
						result.InvoiceSys = "VIETEL";
						result.InvoiceCode = string.Format("{0}|{1}", CS_0024_003C_003E8__locals27.response.result.reservationCode ?? "", numOrder3);
						result.InvoiceNumOrder = CS_0024_003C_003E8__locals27.response.result.invoiceNo;
						result.InvoiceTime = GetInvoiceTimeByResult(val, viettelLogin, CS_0024_003C_003E8__locals27.response.result, CS_0024_003C_003E8__locals27.invoiceData);
						result.InvoiceLoginname = viettelLogin.User;
					}
				}
				else
				{
                    Inventec.Common.Logging.LogSystem.Error("tao hoa doi dien tu that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals27.response), CS_0024_003C_003E8__locals27.response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals27.invoiceData), CS_0024_003C_003E8__locals27.invoiceData));
					ElectronicBillResultUtil.Set(ref result, false, CS_0024_003C_003E8__locals27.response.description);
				}
			}
			catch (Exception ex)
			{
				result.Success = false;
				LogSystem.Error(ex);
			}
		}

		private long GetInvoiceTimeByResult(ElectronicBillViettelManager eViettel, DataInitApi viettelLogin, ResponseResult responseResult, DataCreateInvoice invoiceData)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			long result = Inventec.Common.DateTime.Get.Now().GetValueOrDefault();
			try
			{
				if (eViettel != null && viettelLogin != null && (int)viettelLogin.Version == 1)
				{
					if (invoiceData != null && invoiceData.generalInvoiceInfo != null && !string.IsNullOrWhiteSpace(invoiceData.generalInvoiceInfo.invoiceIssuedDate))
					{
						result = GetTimefromMilliseconds(double.Parse(invoiceData.generalInvoiceInfo.invoiceIssuedDate));
					}
					else
					{
						GetInvoiceInfoFilter val = new GetInvoiceInfoFilter();
						val.invoiceNo = responseResult.invoiceNo;
						val.pageNum = 1L;
						val.rowPerPage = 10L;
						val.endDate = DateTime.Now;
						val.startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
						Response val2 = eViettel.Run((object)val);
						if (val2 != null && val2.invoices != null && val2.invoices.Count > 0)
						{
							if (val2.invoices.First().issueDate.HasValue)
							{
								result = GetTimefromMilliseconds(val2.invoices.First().issueDate.GetValueOrDefault());
							}
							else if (!string.IsNullOrWhiteSpace(val2.invoices.First().issueDateStr))
							{
								DateTime value = DateTime.Parse(val2.invoices.First().issueDateStr);
								result = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)value).GetValueOrDefault();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = Inventec.Common.DateTime.Get.Now().GetValueOrDefault();
				LogSystem.Error(ex);
			}
			return result;
		}

		private long GetNumOrder(string p)
		{
			long result = 0L;
			try
			{
				if (!string.IsNullOrWhiteSpace(p))
				{
					string text = "";
					int num = p.Length - 1;
					while (num > 0 && char.IsDigit(p[num]))
					{
						text = p[num] + text;
						num--;
					}
					result = long.Parse(text);
				}
			}
			catch (Exception ex)
			{
				result = 0L;
				LogSystem.Error(ex);
			}
			return result;
		}

		private DataCreateInvoice CreateInvoice(ElectronicBillDataInput electronicBillDataInput, DataInitApi viettelLogin)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			DataCreateInvoice val = new DataCreateInvoice();
			try
			{
				if (electronicBillDataInput != null)
				{
					val.buyerInfo = GetBuyerInfo(electronicBillDataInput);
					val.generalInvoiceInfo = GetGeneralInvoiceInfo(viettelLogin);
					val.payments = GetPayments(electronicBillDataInput);
					val.sellerInfo = GetSellerInfo(electronicBillDataInput);
					val.itemInfo = GetItemInfo();
					val.summarizeInfo = GetSummarizeInfo(val.itemInfo);
					val.taxBreakdowns = new List<TaxBreakdowns>();
					if (HisConfigCFG.Viettel_TaxBreakdownOption == HisConfigCFG.TaxBreakdownOption.CoThongTinThue)
					{
						val.taxBreakdowns = GetTaxBreakdowns(val.itemInfo);
					}
					val.metadata = GetMetadata(val.summarizeInfo);
				}
			}
			catch (Exception ex)
			{
				val = null;
				LogSystem.Error(ex);
			}
			return val;
		}

        private List<Inventec.Common.ElectronicBillViettel.Model.Metadata> GetMetadata(SummarizeInfo summarizeInfo)
		{
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Expected O, but got Unknown
			//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Expected O, but got Unknown
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Expected O, but got Unknown
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Expected O, but got Unknown
			//IL_065c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0663: Expected O, but got Unknown
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Expected O, but got Unknown
            List<Inventec.Common.ElectronicBillViettel.Model.Metadata> list = new List<Inventec.Common.ElectronicBillViettel.Model.Metadata>();
			try
			{
				if (ElectronicBillDataInput != null)
				{
					string text = HisConfigs.Get<string>("HIS.Desktop.Plugins.Library.ElectronicBill.Viettel.Metadata");
					if (string.IsNullOrWhiteSpace(text))
					{
						return list;
					}
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					List<string> list2 = text.Split('|').ToList();
					if (list2.Count > 5)
					{
						dictionary = General.ProcessDicValueString(ElectronicBillDataInput);
					}
					string text2 = "sotien0";
					string text3 = "";
					string text4 = "sotien1";
					string text5 = "";
					string text6 = "sotien2";
					string text7 = "";
					string text8 = "sotien3";
					string text9 = "";
					string text10 = "sotien4";
					string text11 = "";
					for (int i = 0; i < list2.Count; i++)
					{
						switch (i)
						{
						case 0:
						{
							text2 = list2[0];
							string[] array = list2[i].Split('#');
							if (array.Length > 2)
							{
								text2 = array[0];
								text3 = array[1];
							}
							continue;
						}
						case 1:
						{
							text4 = list2[1];
							string[] array2 = list2[i].Split('#');
							if (array2.Length > 2)
							{
								text4 = array2[0];
								text5 = array2[1];
							}
							continue;
						}
						case 2:
						{
							text6 = list2[2];
							string[] array3 = list2[i].Split('#');
							if (array3.Length > 2)
							{
								text6 = array3[0];
								text7 = array3[1];
							}
							continue;
						}
						case 3:
						{
							text8 = list2[3];
							string[] array4 = list2[i].Split('#');
							if (array4.Length > 2)
							{
								text8 = array4[0];
								text9 = array4[1];
							}
							continue;
						}
						case 4:
						{
							text10 = list2[4];
							string[] array5 = list2[i].Split('#');
							if (array5.Length > 2)
							{
								text10 = array5[0];
								text11 = array5[1];
							}
							continue;
						}
						}
						string[] array6 = list2[i].Split('#');
						if (array6.Length > 2)
						{
                            Inventec.Common.ElectronicBillViettel.Model.Metadata val = new Inventec.Common.ElectronicBillViettel.Model.Metadata();
							val.invoiceCustomFieldId = i + 1;
							val.keyLabel = array6[1];
							val.keyTag = array6[0];
							if (dictionary.ContainsKey(array6[2]))
							{
								val.stringValue = dictionary[array6[2]];
							}
							else
							{
								val.stringValue = array6[2];
							}
							val.valueType = "text";
							list.Add(val);
						}
					}
					if (!string.IsNullOrWhiteSpace(text2))
					{
                        Inventec.Common.ElectronicBillViettel.Model.Metadata val2 = new Inventec.Common.ElectronicBillViettel.Model.Metadata();
						val2.invoiceCustomFieldId = 1L;
						val2.keyLabel = ((!string.IsNullOrWhiteSpace(text3)) ? text3 : "Tổng chi phí");
						val2.keyTag = text2;
						val2.numberValue = (long)Math.Round(ElectronicBillDataInput.Treatment.TOTAL_PRICE.GetValueOrDefault(), 0);
						val2.valueType = "number";
						list.Add(val2);
					}
					if (!string.IsNullOrWhiteSpace(text4))
					{
                        Inventec.Common.ElectronicBillViettel.Model.Metadata val3 = new Inventec.Common.ElectronicBillViettel.Model.Metadata();
						val3.invoiceCustomFieldId = 2L;
						val3.keyLabel = ((!string.IsNullOrWhiteSpace(text5)) ? text5 : "Số tiền BHYT");
						val3.keyTag = text4;
						val3.numberValue = (long)Math.Round(ElectronicBillDataInput.Treatment.TOTAL_HEIN_PRICE.GetValueOrDefault(), 0);
						val3.valueType = "number";
						list.Add(val3);
					}
					if (!string.IsNullOrWhiteSpace(text6))
					{
                        Inventec.Common.ElectronicBillViettel.Model.Metadata val4 = new Inventec.Common.ElectronicBillViettel.Model.Metadata();
						val4.invoiceCustomFieldId = 3L;
						val4.keyLabel = ((!string.IsNullOrWhiteSpace(text7)) ? text7 : "Trong đó Số tiền BN cùng chi trả BHYT");
						val4.keyTag = text6;
						decimal num = Math.Round(ElectronicBillDataInput.Treatment.TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault(), 0);
						val4.numberValue = (long)num;
						val4.valueType = "number";
						list.Add(val4);
					}
					if (!string.IsNullOrWhiteSpace(text8))
					{
                        Inventec.Common.ElectronicBillViettel.Model.Metadata val5 = new Inventec.Common.ElectronicBillViettel.Model.Metadata();
						val5.invoiceCustomFieldId = 4L;
						val5.keyLabel = ((!string.IsNullOrWhiteSpace(text9)) ? text9 : "Số tiền các quỹ tài trợ khác");
						val5.keyTag = text8;
						decimal num2 = default(decimal);
						if (!ElectronicBillDataInput.IsTransactionList)
						{
							if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.TDL_BILL_FUND_AMOUNT.HasValue)
							{
								num2 = ElectronicBillDataInput.Transaction.TDL_BILL_FUND_AMOUNT.Value;
							}
							else if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.HIS_BILL_FUND != null && ElectronicBillDataInput.Transaction.HIS_BILL_FUND.Count > 0)
							{
								num2 = ElectronicBillDataInput.Transaction.HIS_BILL_FUND.Sum((HIS_BILL_FUND s) => s.AMOUNT);
							}
						}
						val5.numberValue = (long)Math.Round(ElectronicBillDataInput.Treatment.TOTAL_BILL_FUND.GetValueOrDefault() + num2, 0);
						val5.valueType = "number";
						list.Add(val5);
					}
					if (!string.IsNullOrWhiteSpace(text10))
					{
                        Inventec.Common.ElectronicBillViettel.Model.Metadata val6 = new Inventec.Common.ElectronicBillViettel.Model.Metadata();
						val6.invoiceCustomFieldId = 5L;
						val6.keyLabel = ((!string.IsNullOrWhiteSpace(text11)) ? text11 : "Số tiền BN thanh toán");
						val6.keyTag = text10;
						val6.numberValue = (long)Math.Round(summarizeInfo.totalAmountWithTax, 0);
						val6.valueType = "number";
						list.Add(val6);
					}
				}
			}
			catch (Exception ex)
			{
                list = new List<Inventec.Common.ElectronicBillViettel.Model.Metadata>();
				LogSystem.Error(ex);
			}
			return list;
		}

		private bool CheckTransactionCreateTime(HIS_TRANSACTION transaction)
		{
			bool result = false;
			try
			{
				if (transaction != null && transaction.CREATE_TIME.HasValue && Inventec.Common.DateTime.Get.Now().GetValueOrDefault() - transaction.CREATE_TIME.Value <= 10)
				{
					result = true;
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Error(ex);
			}
			return result;
		}

		private List<TaxBreakdowns> GetTaxBreakdowns(List<ItemInfo> list)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			List<TaxBreakdowns> list2 = new List<TaxBreakdowns>();
			try
			{
				if (list != null && list.Count > 0)
				{
					List<IGrouping<long?, ItemInfo>> list3 = (from o in list
						group o by o.taxPercentage).ToList();
					foreach (IGrouping<long?, ItemInfo> item in list3)
					{
						TaxBreakdowns val = new TaxBreakdowns();
						val.taxPercentage = item.First().taxPercentage;
						val.taxAmount = item.Sum((ItemInfo s) => s.taxAmount.GetValueOrDefault());
						val.taxableAmount = item.Sum((ItemInfo s) => s.itemTotalAmountWithoutTax.GetValueOrDefault());
						list2.Add(val);
					}
					if (TempType == TemplateEnum.TYPE.Template4)
					{
						list2.First().taxPercentage = 0L;
					}
				}
			}
			catch (Exception ex)
			{
				list2 = new List<TaxBreakdowns>();
				LogSystem.Error(ex);
			}
			return list2;
		}

		private SummarizeInfo GetSummarizeInfo(List<ItemInfo> listItem)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			SummarizeInfo val = null;
			try
			{
				if (listItem != null)
				{
					val = new SummarizeInfo();
					decimal num = default(decimal);
					decimal num2 = default(decimal);
					if (TempType == TemplateEnum.TYPE.Template10)
					{
						List<ItemInfo> list = listItem.Where((ItemInfo o) => o.itemCode.Equals("TONG")).ToList();
						if (list != null && list.Count > 0)
						{
							num = Math.Round(list.Sum((ItemInfo s) => s.itemTotalAmountWithTax.GetValueOrDefault()), 0);
							num2 = Math.Round(list.Sum((ItemInfo s) => s.itemTotalAmountWithoutTax.GetValueOrDefault()), 0);
						}
					}
					else
					{
						num = Math.Round(listItem.Where((ItemInfo s) => s.selection == 1).Sum((ItemInfo s) => s.itemTotalAmountWithTax.GetValueOrDefault()), 0);
						num2 = Math.Round(listItem.Where((ItemInfo s) => s.selection == 1).Sum((ItemInfo s) => s.itemTotalAmountWithoutTax.GetValueOrDefault()), 0);
					}
					val.discountAmount = Math.Round(listItem.Sum((ItemInfo s) => s.discount.GetValueOrDefault()) + ((ElectronicBillDataInput.Transaction != null) ? ElectronicBillDataInput.Transaction.EXEMPTION.GetValueOrDefault() : 0m), 0);
					val.sumOfTotalLineAmountWithoutTax = num2;
					val.totalAmountWithoutTax = num2;
					val.totalAmountWithTax = num;
					val.totalAmountWithTaxInWords = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", Inventec.Common.Number.Convert.NumberToNumberRoundMax4(num))) + "đồng";
					if (HisConfigCFG.Viettel_TaxBreakdownOption == HisConfigCFG.TaxBreakdownOption.KhongHienThiThongTinThue)
					{
						val.totalTaxAmount = null;
					}
					else
					{
						val.totalTaxAmount = Math.Round(listItem.Sum((ItemInfo s) => s.taxAmount.GetValueOrDefault()), 0);
					}
					val.totalAmountAfterDiscount = Math.Round(num2 - val.discountAmount, 0);
				}
			}
			catch (Exception ex)
			{
				val = null;
				LogSystem.Error(ex);
			}
			return val;
		}

		private SellerInfo GetSellerInfo(ElectronicBillDataInput electronicBillDataInput)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			SellerInfo val = null;
			try
			{
				if (electronicBillDataInput != null)
				{
					val = new SellerInfo();
					val.sellerAddressLine = electronicBillDataInput.Branch.ADDRESS;
					val.sellerBankAccount = electronicBillDataInput.Branch.ACCOUNT_NUMBER;
					val.sellerBankName = null;
					val.sellerCityName = null;
					val.sellerCode = electronicBillDataInput.Branch.HEIN_MEDI_ORG_CODE;
					val.sellerCountryCode = null;
					val.sellerDistrictName = null;
					val.sellerEmail = null;
					val.sellerFaxNumber = null;
					val.sellerLegalName = electronicBillDataInput.Branch.BRANCH_NAME;
					val.sellerPhoneNumber = electronicBillDataInput.Branch.PHONE;
					val.sellerWebsite = null;
				}
			}
			catch (Exception ex)
			{
				val = null;
				LogSystem.Error(ex);
			}
			return val;
		}

		private List<Payments> GetPayments(ElectronicBillDataInput electronicBillDataInput)
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			List<Payments> list = null;
			try
			{
				list = new List<Payments>();
				string paymentMethodName = electronicBillDataInput.PaymentMethod;
				if (electronicBillDataInput.Transaction != null)
				{
					HIS_PAY_FORM val = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == electronicBillDataInput.Transaction.PAY_FORM_ID);
					if (val != null)
					{
						paymentMethodName = val.ELECTRONIC_PAY_FORM_NAME ?? val.PAY_FORM_NAME;
					}
				}
				Payments item = new Payments
				{
					paymentMethodName = paymentMethodName
				};
				list.Add(item);
			}
			catch (Exception ex)
			{
				list = null;
				LogSystem.Error(ex);
			}
			return list;
		}

		private List<ItemInfo> GetItemInfo()
		{
			List<ItemInfo> list = new List<ItemInfo>();
			TemplateFactory.ProcessDataSereServToSereServBill(TempType, ref ElectronicBillDataInput);
			int i = 1;
			ElectronicBillDataInput electronicBillDataInput = new ElectronicBillDataInput(ElectronicBillDataInput);
			ElectronicBillDataInput electronicBillDataInput2 = null;
			if (HisConfigCFG.IsSplitServicesWithVat && ElectronicBillDataInput.SereServBill.Exists(delegate(HIS_SERE_SERV_BILL o)
			{
				int result;
				if (o.TDL_VAT_RATIO.HasValue)
				{
					decimal? tDL_VAT_RATIO = o.TDL_VAT_RATIO;
					result = (((tDL_VAT_RATIO.GetValueOrDefault() > default(decimal)) & tDL_VAT_RATIO.HasValue) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}) && TempType != TemplateEnum.TYPE.Template10 && TempType != TemplateEnum.TYPE.TemplateNhaThuoc)
			{
				electronicBillDataInput.SereServBill = ElectronicBillDataInput.SereServBill.Where(delegate(HIS_SERE_SERV_BILL o)
				{
					int result;
					if (o.TDL_VAT_RATIO.HasValue)
					{
						decimal? tDL_VAT_RATIO = o.TDL_VAT_RATIO;
						result = (((tDL_VAT_RATIO.GetValueOrDefault() <= default(decimal)) & tDL_VAT_RATIO.HasValue) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					return (byte)result != 0;
				}).ToList();
				electronicBillDataInput2 = new ElectronicBillDataInput(ElectronicBillDataInput);
				electronicBillDataInput2.SereServs = TemplateFactory.GetSereServWithVAT(ElectronicBillDataInput);
			}
			else
			{
				electronicBillDataInput.SereServs = ElectronicBillDataInput.SereServs;
				electronicBillDataInput.SereServBill = ElectronicBillDataInput.SereServBill;
			}
			IRunTemplate runTemplate = TemplateFactory.MakeIRun(TempType, electronicBillDataInput);
			object obj = (((electronicBillDataInput.SereServs != null && electronicBillDataInput.SereServs.Count > 0) || (electronicBillDataInput.SereServBill != null && electronicBillDataInput.SereServBill.Count > 0)) ? runTemplate.Run() : null);
			object obj2 = ((electronicBillDataInput2 != null) ? TemplateFactory.MakeIRun(TemplateEnum.TYPE.TemplateNhaThuoc, electronicBillDataInput2).Run() : null);
			if (obj == null && obj2 == null)
			{
				throw new Exception("Không có thông tin chi tiết dịch vụ.");
			}
			if (obj != null)
			{
				if (obj.GetType() == typeof(List<ProductBase>))
				{
					List<ProductBase> list2 = (List<ProductBase>)obj;
					if (list2 == null || list2.Count == 0)
					{
						throw new Exception("Không có thông tin chi tiết dịch vụ.");
					}
					list.AddRange(GetListProduct(list2, ref i));
				}
				else
				{
					if (!(obj.GetType() == typeof(List<ProductBasePlus>)))
					{
						throw new Exception("Không có thông tin chi tiết dịch vụ.");
					}
					List<ProductBasePlus> list3 = (List<ProductBasePlus>)obj;
					if (list3 == null || list3.Count == 0)
					{
						throw new Exception("Không có thông tin chi tiết dịch vụ.");
					}
					list.AddRange(GetVatProduct(list3, ref i));
				}
			}
			if (obj2 != null)
			{
				List<ProductBasePlus> list4 = (List<ProductBasePlus>)obj2;
				if (list4 == null || list4.Count == 0)
				{
					throw new Exception("Không có thông tin chi tiết dịch vụ.");
				}
				list.AddRange(GetVatProduct(list4, ref i));
			}
			return list;
		}

		private List<ItemInfo> GetListProduct(List<ProductBase> listProduct, ref int i)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Expected O, but got Unknown
			List<ItemInfo> list = new List<ItemInfo>();
			foreach (ProductBase item in listProduct)
			{
				ItemInfo val = new ItemInfo();
				val.lineNumber = i;
				val.itemCode = item.ProdCode;
				val.itemName = item.ProdName;
				val.quantity = item.ProdQuantity;
				val.unitName = item.ProdUnit;
				val.discount = null;
				val.itemDiscount = null;
				val.selection = 1;
				if (TempType == TemplateEnum.TYPE.Template10 && i > 7)
				{
					val.selection = 2;
				}
				val.itemTotalAmountWithTax = item.Amount;
				val.itemTotalAmountWithoutTax = item.Amount;
				val.unitPrice = item.ProdPrice;
				if (HisConfigCFG.Viettel_TaxBreakdownOption == HisConfigCFG.TaxBreakdownOption.KhongHienThiThongTinThue)
				{
					val.taxPercentage = null;
					val.taxAmount = null;
				}
				else
				{
					val.taxPercentage = -2L;
					val.taxAmount = default(decimal);
				}
				list.Add(val);
				i++;
			}
			if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.EXEMPTION.HasValue)
			{
				decimal? eXEMPTION = ElectronicBillDataInput.Transaction.EXEMPTION;
				if (((eXEMPTION.GetValueOrDefault() > default(decimal)) & eXEMPTION.HasValue) && TempType != TemplateEnum.TYPE.Template10)
				{
					ItemInfo val2 = new ItemInfo();
					val2.lineNumber = i;
					val2.selection = 3;
					val2.itemCode = "Chiet_Khau";
					val2.itemName = "Miễn giảm";
					val2.itemTotalAmountWithoutTax = ElectronicBillDataInput.Transaction.EXEMPTION;
					val2.itemTotalAmountAfterDiscount = ElectronicBillDataInput.Transaction.EXEMPTION;
					val2.itemTotalAmountWithTax = ElectronicBillDataInput.Transaction.EXEMPTION;
					list.Add(val2);
					i++;
				}
			}
			if ((ElectronicBillDataInput.Transaction == null || !ElectronicBillDataInput.Transaction.EXEMPTION.HasValue) && TempType == TemplateEnum.TYPE.Template3)
			{
				decimal value = default(decimal);
				if (!ElectronicBillDataInput.IsTransactionList)
				{
					if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.TDL_BILL_FUND_AMOUNT.HasValue)
					{
						value = ElectronicBillDataInput.Transaction.TDL_BILL_FUND_AMOUNT.Value;
					}
					else if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.HIS_BILL_FUND != null && ElectronicBillDataInput.Transaction.HIS_BILL_FUND.Count > 0)
					{
						value = ElectronicBillDataInput.Transaction.HIS_BILL_FUND.Sum((HIS_BILL_FUND s) => s.AMOUNT);
					}
				}
				value += ElectronicBillDataInput.Treatment.TOTAL_BILL_FUND.GetValueOrDefault();
				ItemInfo obj = list.First();
				obj.itemTotalAmountWithoutTax -= (decimal?)value;
				ItemInfo obj2 = list.First();
				obj2.itemTotalAmountWithTax -= (decimal?)value;
				ItemInfo obj3 = list.First();
				obj3.unitPrice -= (decimal?)value;
			}
			return list;
		}

		private List<ItemInfo> GetVatProduct(List<ProductBasePlus> listProduct, ref int i)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			List<ItemInfo> list = new List<ItemInfo>();
			foreach (ProductBasePlus item in listProduct)
			{
				ItemInfo val = new ItemInfo();
				val.lineNumber = i;
				val.itemName = item.ProdName;
				val.unitPrice = item.ProdPrice;
				val.quantity = item.ProdQuantity;
				val.unitName = item.ProdUnit;
				if (HisConfigCFG.Viettel_TaxBreakdownOption == HisConfigCFG.TaxBreakdownOption.KhongHienThiThongTinThue)
				{
					val.taxPercentage = null;
					val.itemTotalAmountWithTax = item.Amount;
					val.itemTotalAmountWithoutTax = item.Amount;
					val.taxAmount = null;
				}
				else
				{
					if (item.TaxPercentage.HasValue)
					{
						val.taxPercentage = (long)Math.Round(item.TaxConvert, 0);
					}
					else
					{
						val.taxPercentage = -2L;
					}
					val.itemTotalAmountWithTax = item.Amount;
					val.itemTotalAmountWithoutTax = item.AmountWithoutTax;
					val.taxAmount = item.TaxAmount;
				}
				val.discount = null;
				val.itemDiscount = null;
				val.selection = 1;
				list.Add(val);
				i++;
			}
			if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.EXEMPTION.HasValue)
			{
				decimal? eXEMPTION = ElectronicBillDataInput.Transaction.EXEMPTION;
				if (((eXEMPTION.GetValueOrDefault() > default(decimal)) & eXEMPTION.HasValue) && TempType != TemplateEnum.TYPE.Template10)
				{
					ItemInfo val2 = new ItemInfo();
					val2.lineNumber = i;
					val2.selection = 3;
					val2.itemCode = "Chiet_Khau";
					val2.itemName = "Miễn giảm";
					val2.itemTotalAmountWithoutTax = ElectronicBillDataInput.Transaction.EXEMPTION;
					val2.itemTotalAmountAfterDiscount = ElectronicBillDataInput.Transaction.EXEMPTION;
					val2.itemTotalAmountWithTax = ElectronicBillDataInput.Transaction.EXEMPTION;
					list.Add(val2);
					i++;
				}
			}
			return list;
		}

		private GeneralInvoiceInfo GetGeneralInvoiceInfo(DataInitApi viettelLogin)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Invalid comparison between Unknown and I4
			GeneralInvoiceInfo val = null;
			try
			{
				if (ElectronicBillDataInput != null)
				{
					val = new GeneralInvoiceInfo();
					val.additionalReferenceDate = null;
					val.additionalReferenceDesc = "";
					val.adjustmentInvoiceType = "";
					val.adjustmentType = "1";
					val.certificateSerial = "";
					val.currencyCode = ElectronicBillDataInput.Currency;
					val.cusGetInvoiceRight = true;
					val.paymentStatus = true;
					val.transactionUuid = "";
					if (ElectronicBillDataInput.Transaction != null && !string.IsNullOrEmpty(ElectronicBillDataInput.Transaction.TRANSACTION_CODE))
					{
						val.transactionUuid = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
					}
					else if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
					{
						val.transactionUuid = (from s in ElectronicBillDataInput.ListTransaction
							select s.TRANSACTION_CODE into o
							orderby o
							select o).FirstOrDefault();
					}
					if (ElectronicBillDataInput.Transaction != null)
					{
						val.userName = ElectronicBillDataInput.Transaction.CASHIER_USERNAME;
					}
					else if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
					{
						val.userName = string.Join(", ", ElectronicBillDataInput.ListTransaction.Select((V_HIS_TRANSACTION s) => s.CASHIER_USERNAME).Distinct());
					}
					val.templateCode = ElectronicBillDataInput.TemplateCode;
					val.invoiceSeries = ElectronicBillDataInput.SymbolCode;
					val.invoiceType = GetInvoiceType(ElectronicBillDataInput.TemplateCode);
					string paymentTypeName = ElectronicBillDataInput.PaymentMethod;
					if (ElectronicBillDataInput.Transaction != null)
					{
						HIS_PAY_FORM val2 = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == ElectronicBillDataInput.Transaction.PAY_FORM_ID);
						if (val2 != null)
						{
							paymentTypeName = val2.ELECTRONIC_PAY_FORM_NAME ?? val2.PAY_FORM_NAME;
						}
					}
					val.paymentTypeName = paymentTypeName;
					if (viettelLogin != null && (int)viettelLogin.Version == 1)
					{
						val.invoiceIssuedDate = GetTimeMilliseconds(Inventec.Common.DateTime.Get.Now().GetValueOrDefault()).ToString() ?? "";
					}
					if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue)
					{
						val.adjustmentType = "3";
						val.originalInvoiceId = ElectronicBillDataInput.Transaction.TDL_ORIGINAL_EI_NUM_ORDER;
						val.originalInvoiceIssueDate = GetTimeMilliseconds(ElectronicBillDataInput.Transaction.TDL_ORIGINAL_EI_TIME.GetValueOrDefault()).ToString() ?? "";
						val.additionalReferenceDesc = ElectronicBillDataInput.Transaction.REPLACE_REASON;
						val.additionalReferenceDate = GetTimeMilliseconds(ElectronicBillDataInput.Transaction.REPLACE_TIME.GetValueOrDefault()).ToString() ?? "";
						val.adjustedNote = ElectronicBillDataInput.Transaction.REPLACE_REASON;
					}
				}
			}
			catch (Exception ex)
			{
				val = null;
				LogSystem.Error(ex);
			}
			return val;
		}

		private string GetInvoiceType(string p)
		{
			string result = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(p))
				{
					string[] array = p.Split('/');
					result = ((array[0].Length <= 6) ? array[0] : array[0].Substring(0, 6));
				}
			}
			catch (Exception ex)
			{
				result = "";
				LogSystem.Error(ex);
			}
			return result;
		}

		private BuyerInfo GetBuyerInfo(ElectronicBillDataInput electronicBillDataInput)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			BuyerInfo val = new BuyerInfo();
			try
			{
				InvoiceInfoADO data = InvoiceInfoProcessor.GetData(electronicBillDataInput, TempType != TemplateEnum.TYPE.Template10);
				val.buyerLegalName = data.BuyerOrganization;
				val.buyerTaxCode = data.BuyerTaxCode;
				val.buyerBankAccount = data.BuyerAccountNumber;
				val.buyerName = data.BuyerName;
				val.buyerCode = data.BuyerCode;
				val.buyerBirthDay = data.BuyerDob;
				val.buyerIdNo = data.BuyerIdentityNumber;
				if (data.BuyerIdentityType == "1" || data.BuyerIdentityType == "2")
				{
					val.buyerIdType = "1";
				}
				else if (data.BuyerIdentityType == "3")
				{
					val.buyerIdType = "2";
				}
				else
				{
					val.buyerIdType = data.BuyerIdentityType;
				}
				HIS_TRANSACTION transaction = electronicBillDataInput.Transaction;
				if (!string.IsNullOrWhiteSpace((transaction != null) ? transaction.BUYER_IDENTITY_NUMBER : null))
				{
					val.buyerIdNo = electronicBillDataInput.Transaction.BUYER_IDENTITY_NUMBER.Trim();
				}
				else if (!string.IsNullOrWhiteSpace(data.BuyerCCCD))
				{
					val.buyerIdNo = data.BuyerCCCD;
					val.buyerIdType = "1";
				}
				HIS_TRANSACTION transaction2 = electronicBillDataInput.Transaction;
				if (!string.IsNullOrWhiteSpace((transaction2 != null) ? transaction2.BUYER_SOCIAL_RELATIONS_CODE : null))
				{
					val.buyerBudgetCode = electronicBillDataInput.Transaction.BUYER_SOCIAL_RELATIONS_CODE.Trim();
				}
				val.buyerAddressLine = ((!string.IsNullOrWhiteSpace(data.BuyerAddress)) ? data.BuyerAddress : ".");
				val.buyerPhoneNumber = data.BuyerPhone;
				val.buyerEmail = data.BuyerEmail;
				LogSystem.Debug("API Create Result: " + LogUtil.TraceData("RESULT", (object)val));
			}
			catch (Exception ex)
			{
				val = null;
				LogSystem.Error(ex);
			}
			return val;
		}

		private V_HIS_PATIENT GetPatientbyId(long p)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			V_HIS_PATIENT result = null;
			try
			{
				HisPatientViewFilter val = new HisPatientViewFilter();
				((MOS.Filter.FilterBase)val).ID = p;
				List<V_HIS_PATIENT> list = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_PATIENT>>("/api/HisPatient/GetView", ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
				if (list != null)
				{
					result = list.FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				result = null;
				LogSystem.Error(ex);
			}
			return result;
		}

		private bool Check(ref ElectronicBillResult electronicBillResult)
		{
			bool result = true;
			try
			{
				string[] array = ServiceConfig.Split('|');
				if (array.Length < 2)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
				}
				if (array[0] != "VIETEL")
				{
					throw new Exception("Không đúng cấu hình nhà cung cấp VIETTEL");
				}
				string[] array2 = AccountConfig.Split('|');
				if (array2.Length < 2)
				{
					throw new Exception("Sai định dạng cấu hình tai khoản.");
				}
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
						string text = "";
						List<string> list3 = new List<string>();
						foreach (V_HIS_TRANSACTION item in list2)
						{
							list3.Add(string.Format("{0}({1})", item.ACCOUNT_BOOK_NAME, item.TEMPLATE_CODE));
						}
						text = string.Join(", ", list3.Distinct().ToList());
						throw new Exception(string.Format("Các sổ {0} có thông tin mẫu số khác nhau. Vui lòng kiểm tra lại.", text));
					}
					List<V_HIS_TRANSACTION> list4 = ElectronicBillDataInput.ListTransaction.Where((V_HIS_TRANSACTION o) => o.SYMBOL_CODE != ElectronicBillDataInput.SymbolCode).ToList();
					if (list4 != null && list4.Count > 0)
					{
						string text2 = "";
						List<string> list5 = new List<string>();
						foreach (V_HIS_TRANSACTION item2 in list4)
						{
							list5.Add(string.Format("{0}({1})", item2.ACCOUNT_BOOK_NAME, item2.SYMBOL_CODE));
						}
						text2 = string.Join(", ", list5.Distinct().ToList());
						throw new Exception(string.Format("Các sổ {0} có thông tin ký hiệu khác nhau. Vui lòng kiểm tra lại.", text2));
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

		private string GetVirNumOrder(long numOrder)
		{
			return string.Format("{0:0000000}", numOrder);
		}

		private double GetTimeMilliseconds(long time)
		{
			return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(time) ?? DateTime.Now).ToUniversalTime().Subtract(UnixEpoch).TotalMilliseconds;
		}

		private long GetTimefromMilliseconds(double milisecondTime)
		{
			DateTime time = UnixEpoch.AddMilliseconds(milisecondTime);
			TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
			time = currentTimeZone.ToLocalTime(time);
			return Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)time).GetValueOrDefault();
		}
	}
}
