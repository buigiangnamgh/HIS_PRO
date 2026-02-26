using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.SignInvoice;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.DateTime;
using Inventec.Common.ElectronicBill.Misa;
using Inventec.Common.ElectronicBill.Misa.Model;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.Repository;
using Inventec.Common.String;
using Inventec.UC.Login.Base;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MISA
{
    internal class MISABehaviorV2 : HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun
	{
		private enum VATType
		{
			BanHang,
			GiaTriGiaTang
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass14_0
		{
			public Response response;

			public DeleteInvoiceV2 dInvoice;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass15_0
		{
			public Response response;

			public GetInvoiceV2 invoices;
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass17_0
		{
			public CreateInvoiceV2 invoices;
		}

		private ElectronicBillDataInput ElectronicBillDataInput;

		private string serviceConfig;

		private string accountConfig;

		private TemplateEnum.TYPE TempType;

		private DataInit DataInit;

		private bool IsRelease;

		private Response InvoiceRelease;

		private Response InvoiceCreate;

		private bool IsAutoSign;

		private bool InChuyenDoi = false;

		private VATType LoaiHoaDon = VATType.BanHang;

		public MISABehaviorV2(ElectronicBillDataInput electronicBillDataInput, string serviceConfig, string accountConfig)
		{
			ElectronicBillDataInput = electronicBillDataInput;
			this.serviceConfig = serviceConfig;
			this.accountConfig = accountConfig;
		}

        ElectronicBillResult HIS.Desktop.Plugins.Library.ElectronicBill.Base.IRun.Run(ElectronicBillType.ENUM _electronicBillTypeEnum, TemplateEnum.TYPE _tempType)
		{
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Expected O, but got Unknown
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			try
			{
				if (Check(_electronicBillTypeEnum, ref electronicBillResult))
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
					string text2 = array[2];
					if (string.IsNullOrEmpty(text2))
					{
						LogSystem.Error("Khong co thong tin id ung dung");
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không có thông tin id ứng dụng");
						return electronicBillResult;
					}
					string text3 = array[3];
					if (string.IsNullOrEmpty(text3))
					{
						LogSystem.Error("Khong co thong tin dia chi may chu ky so");
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không có thông tin địa chỉ máy chủ ký số");
						return electronicBillResult;
					}
					if (array.Count() > 5)
					{
						string text4 = array[5];
						IsAutoSign = text4 == "1";
					}
					string downloadUrl = "";
					if (array.Count() > 6)
					{
						downloadUrl = array[6];
					}
					if (array.Count() > 8)
					{
						InChuyenDoi = array[8].Trim() == "1";
					}
					if (array.Count() > 9 && array[9].Trim() == "1")
					{
						LoaiHoaDon = VATType.GiaTriGiaTang;
					}
					string[] array2 = accountConfig.Split('|');
					DataInit = new DataInit();
					DataInit.BaseUrl = text;
					DataInit.AppID = text2;
					DataInit.SignUrl = text3;
					DataInit.TaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
					DataInit.User = array2[0].Trim();
					DataInit.Pass = array2[1].Trim();
					DataInit.DownloadUrl = downloadUrl;
					switch (_electronicBillTypeEnum)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						ProcessCreateInvoice(ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
						ProcessGetInvoice(ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.DELETE_INVOICE:
					case ElectronicBillType.ENUM.CANCEL_INVOICE:
						ProcessCancelInvoice(ref electronicBillResult);
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

		private void ProcessCancelInvoice(ref ElectronicBillResult result)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			try
			{
				_003C_003Ec__DisplayClass14_0 CS_0024_003C_003E8__locals20 = new _003C_003Ec__DisplayClass14_0();
				CS_0024_003C_003E8__locals20.dInvoice = new DeleteInvoiceV2();
				CS_0024_003C_003E8__locals20.dInvoice.CancelReason = ElectronicBillDataInput.CancelReason;
				CS_0024_003C_003E8__locals20.dInvoice.RefDate = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ElectronicBillDataInput.CancelTime.GetValueOrDefault()) ?? DateTime.Now;
				CS_0024_003C_003E8__locals20.dInvoice.RefID = Guid.NewGuid().ToString();
				CS_0024_003C_003E8__locals20.dInvoice.InvNo = ElectronicBillDataInput.ENumOrder;
				CS_0024_003C_003E8__locals20.dInvoice.InvDate = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ElectronicBillDataInput.TransactionTime) ?? DateTime.Now;
				CS_0024_003C_003E8__locals20.dInvoice.TransactionID = ElectronicBillDataInput.InvoiceCode;
				CS_0024_003C_003E8__locals20.response = null;
				DataInit.DataDelete = CS_0024_003C_003E8__locals20.dInvoice;
				ElectronicBillMisaManager val = new ElectronicBillMisaManager(DataInit);
				if (val != null)
				{
                    CS_0024_003C_003E8__locals20.response = val.Run((Inventec.Common.ElectronicBill.Misa.Type)5);
				}
				LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<Response>((Expression<Func<Response>>)(() => CS_0024_003C_003E8__locals20.response)), (object)CS_0024_003C_003E8__locals20.response));
				if (CS_0024_003C_003E8__locals20.response != null && string.IsNullOrWhiteSpace(CS_0024_003C_003E8__locals20.response.description))
				{
					result.Success = true;
					result.InvoiceSys = "MISA";
					result.Messages = new List<string> { CS_0024_003C_003E8__locals20.response.description };
					return;
				}
				result.Success = false;
				result.InvoiceSys = "MISA";
                Inventec.Common.Logging.LogSystem.Error("Huy hoa don dien tu that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals20.response), CS_0024_003C_003E8__locals20.response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals20.dInvoice), CS_0024_003C_003E8__locals20.dInvoice));
				ElectronicBillResultUtil.Set(ref result, false, CS_0024_003C_003E8__locals20.response.description);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ProcessGetInvoice(ref ElectronicBillResult result)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			try
			{
				_003C_003Ec__DisplayClass15_0 CS_0024_003C_003E8__locals17 = new _003C_003Ec__DisplayClass15_0();
				CS_0024_003C_003E8__locals17.invoices = new GetInvoiceV2();
				string converter = ClientTokenManagerStore.ClientTokenManager.GetUserName();
				if (!string.IsNullOrWhiteSpace(ElectronicBillDataInput.Converter))
				{
					converter = ElectronicBillDataInput.Converter;
				}
				CS_0024_003C_003E8__locals17.invoices.Converter = converter;
				CS_0024_003C_003E8__locals17.invoices.TransactionID = ElectronicBillDataInput.InvoiceCode;
				CS_0024_003C_003E8__locals17.response = null;
				DataInit.DataGet = CS_0024_003C_003E8__locals17.invoices;
				ElectronicBillMisaManager val = new ElectronicBillMisaManager(DataInit);
				if (val != null)
				{
					if (InChuyenDoi)
					{
                        CS_0024_003C_003E8__locals17.response = val.Run((Inventec.Common.ElectronicBill.Misa.Type)6);
					}
					else
					{
                        CS_0024_003C_003E8__locals17.response = val.Run((Inventec.Common.ElectronicBill.Misa.Type)4);
					}
				}
				LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<Response>((Expression<Func<Response>>)(() => CS_0024_003C_003E8__locals17.response)), (object)CS_0024_003C_003E8__locals17.response));
				if (CS_0024_003C_003E8__locals17.response != null && !string.IsNullOrWhiteSpace(CS_0024_003C_003E8__locals17.response.fileDownload))
				{
					string fileDownload = CS_0024_003C_003E8__locals17.response.fileDownload;
					result.Success = true;
					result.InvoiceSys = "MISA";
					result.InvoiceLink = fileDownload;
					LogSystem.Debug("_____PDF_FILE_NAME: " + fileDownload);
				}
				else
				{
					result.Success = false;
					result.InvoiceSys = "MISA";
					
                    Inventec.Common.Logging.LogSystem.Error("lay file chuyen doi that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals17.response), CS_0024_003C_003E8__locals17.response) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CS_0024_003C_003E8__locals17.invoices), CS_0024_003C_003E8__locals17.invoices));
					ElectronicBillResultUtil.Set(ref result, false, CS_0024_003C_003E8__locals17.response.description);
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
				if (array.Length < 5)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
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

		private void ProcessCreateInvoice(ref ElectronicBillResult result)
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			try
			{
				_003C_003Ec__DisplayClass17_0 _003C_003Ec__DisplayClass17_ = new _003C_003Ec__DisplayClass17_0();
				if (ElectronicBillDataInput.Transaction == null || ElectronicBillDataInput.Transaction.ID <= 0)
				{
					result.Success = false;
					result.InvoiceSys = "MISA";
					ElectronicBillResultUtil.Set(ref result, false, "Chưa hỗ trợ xuất hóa đơn gộp!");
					throw new Exception("Chưa hỗ trợ xuất hóa đơn gộp!");
				}
				DataInit val = new DataInit();
				DataObjectMapper.Map<DataInit>((object)val, (object)DataInit);
				_003C_003Ec__DisplayClass17_.invoices = GetInvoice(ElectronicBillDataInput, ref result);
				Response val2 = null;
				val.DataCreate = new List<CreateInvoiceV2> { _003C_003Ec__DisplayClass17_.invoices };
				val.DataPreview = _003C_003Ec__DisplayClass17_.invoices;
				ElectronicBillMisaManager val3 = new ElectronicBillMisaManager(val);
				if (val3 != null)
				{
                    InvoiceCreate = val3.Run((Inventec.Common.ElectronicBill.Misa.Type)0);
					if (!IsAutoSign)
					{
                        val2 = val3.Run((Inventec.Common.ElectronicBill.Misa.Type)1);
					}
				}
				LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName<Response>((Expression<Func<Response>>)(() => InvoiceCreate)), (object)InvoiceCreate));
				if (InvoiceCreate != null && string.IsNullOrWhiteSpace(InvoiceCreate.description) && InvoiceCreate.resultV2 != null)
				{
					if (IsAutoSign)
					{
						string errorMessage = "";
						if (DoSignAndReleaseInvoice(null, ref errorMessage) && InvoiceRelease != null)
						{
							result.InvoiceSys = "MISA";
							result.InvoiceCode = InvoiceRelease.resultV2.First().TransactionID;
							result.InvoiceNumOrder = InvoiceRelease.resultV2.First().InvNo;
							result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(InvoiceRelease.resultV2.First().InvDate);
							result.InvoiceLoginname = val.User;
							ElectronicBillResultUtil.Set(ref result, string.IsNullOrWhiteSpace(InvoiceRelease.description), InvoiceRelease.description);
						}
						else
						{
							ElectronicBillResultUtil.Set(ref result, false, errorMessage);
						}
					}
					else if (string.IsNullOrWhiteSpace(val2.description) && (val2.fileToBytes != null || !string.IsNullOrWhiteSpace(val2.fileDownload)))
					{
						SignInitData signInitData = new SignInitData();
						signInitData.ContentSign = InvoiceCreate.resultV2.First().InvoiceData;
						signInitData.fileToBytes = val2.fileToBytes;
						signInitData.FileDownload = val2.fileDownload;
						signInitData.SignAndRelease = DoSignAndReleaseInvoice;
						FormSignInvoice formSignInvoice = new FormSignInvoice(signInitData);
						formSignInvoice.ShowDialog();
						if (IsRelease && InvoiceRelease != null)
						{
							result.InvoiceSys = "MISA";
							result.InvoiceCode = InvoiceRelease.resultV2.First().TransactionID;
							result.InvoiceNumOrder = InvoiceRelease.resultV2.First().InvNo;
							result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(InvoiceRelease.resultV2.First().InvDate);
							result.InvoiceLoginname = val.User;
							ElectronicBillResultUtil.Set(ref result, string.IsNullOrWhiteSpace(InvoiceRelease.description), InvoiceRelease.description);
						}
					}
					else
					{
						result.Success = false;
						result.InvoiceSys = "MISA";

                        Inventec.Common.Logging.LogSystem.Error("tao hoa doi dien tu that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => InvoiceCreate), InvoiceCreate) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => _003C_003Ec__DisplayClass17_.invoices), _003C_003Ec__DisplayClass17_.invoices));
						ElectronicBillResultUtil.Set(ref result, false, (InvoiceCreate != null) ? InvoiceCreate.description : ((val2 != null) ? val2.description : ""));
					}
				}
				else
				{
					result.Success = false;
					result.InvoiceSys = "MISA";

                    Inventec.Common.Logging.LogSystem.Error("tao hoa doi dien tu that bai. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => InvoiceCreate), InvoiceCreate) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => _003C_003Ec__DisplayClass17_.invoices), _003C_003Ec__DisplayClass17_.invoices));
					ElectronicBillResultUtil.Set(ref result, false, (InvoiceCreate != null) ? InvoiceCreate.description : "");
				}
			}
			catch (Exception ex)
			{
				ElectronicBillResultUtil.Set(ref result, false, ex.Message);
				LogSystem.Error(ex);
			}
		}

		private bool DoSignAndReleaseInvoice(SignDelegate data, ref string errorMessage)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Expected O, but got Unknown
			bool result = true;
			try
			{
				if (InvoiceCreate != null && string.IsNullOrWhiteSpace(InvoiceCreate.description) && InvoiceCreate.resultV2 != null)
				{
					string[] array = serviceConfig.Split('|');
					DataInit val = new DataInit();
					DataObjectMapper.Map<DataInit>((object)val, (object)DataInit);
					Response val2 = null;
					SignXml val3 = new SignXml();
					val3.PinCode = array[4];
					val3.XmlContent = InvoiceCreate.resultV2.First().InvoiceData;
					val.DataSign = val3;
					ElectronicBillMisaManager val4 = new ElectronicBillMisaManager(val);
					if (val4 != null)
					{
                        val2 = val4.Run((Inventec.Common.ElectronicBill.Misa.Type)2);
						if (val2 != null && !string.IsNullOrWhiteSpace(val2.XmlData))
						{
							ReleaseInvoiceV2 val5 = new ReleaseInvoiceV2();
							val5.InvoiceData = val2.XmlData;
							val5.RefID = InvoiceCreate.resultV2.First().RefID;
							val5.TransactionID = InvoiceCreate.resultV2.First().TransactionID;
							if (data != null)
							{
								val5.ReceiverEmail = data.Email;
								val5.ReceiverName = data.Name;
								val5.IsSendEmail = !string.IsNullOrWhiteSpace(data.Name) && !string.IsNullOrWhiteSpace(data.Email);
							}
							val.DataRelease = new List<ReleaseInvoiceV2> { val5 };
							ElectronicBillMisaManager val6 = new ElectronicBillMisaManager(val);
							if (val6 != null)
							{
                                InvoiceRelease = val6.Run((Inventec.Common.ElectronicBill.Misa.Type)3);
								if (InvoiceRelease != null && string.IsNullOrWhiteSpace(InvoiceRelease.description) && InvoiceRelease.resultV2 != null)
								{
									IsRelease = true;
								}
								else
								{
									errorMessage = "Phát hành thất bại. " + ((InvoiceRelease != null) ? InvoiceRelease.description : "");
									result = false;
								}
							}
						}
						else
						{
							errorMessage = ((val2 != null) ? val2.description : "");
							result = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Error(ex);
			}
			return result;
		}

		private CreateInvoiceV2 GetInvoice(ElectronicBillDataInput electronicBillDataInput, ref ElectronicBillResult dataResult)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_064c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Expected O, but got Unknown
			CreateInvoiceV2 val = null;
			if (electronicBillDataInput != null)
			{
				val = new CreateInvoiceV2();
				InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput, false);
				val.BuyerAddress = data.BuyerAddress;
				val.BuyerBankAccount = data.BuyerAccountNumber;
				val.BuyerBankName = "";
				val.BuyerFullName = data.BuyerName;
				val.BuyerEmail = data.BuyerEmail;
				val.BuyerLegalName = data.BuyerOrganization;
				val.BuyerPhoneNumber = data.BuyerPhone;
				val.BuyerTaxCode = data.BuyerTaxCode;
				if (electronicBillDataInput.Transaction != null)
				{
					val.RefID = electronicBillDataInput.Transaction.TRANSACTION_CODE;
				}
				else if (electronicBillDataInput.ListTransaction != null && electronicBillDataInput.ListTransaction.Count > 0)
				{
					val.RefID = electronicBillDataInput.ListTransaction.First().TRANSACTION_CODE;
				}
				val.InvDate = DateTime.Now;
				val.InvoiceName = "HÓA ĐƠN GIÁ TRỊ GIA TĂNG";
				val.CurrencyCode = "VND";
				val.ExchangeRate = 1;
				if (TempType != TemplateEnum.TYPE.TemplateNhaThuoc)
				{
					val.InvoiceNote = "Thanh toán viện phí";
				}
				else
				{
					val.InvoiceNote = "Hóa đơn bán lẻ";
				}
				val.InvSeries = electronicBillDataInput.SymbolCode;
				string paymentMethodName = electronicBillDataInput.PaymentMethod;
				if (electronicBillDataInput.Transaction != null)
				{
					HIS_PAY_FORM val2 = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == electronicBillDataInput.Transaction.PAY_FORM_ID);
					if (val2 != null)
					{
						paymentMethodName = val2.ELECTRONIC_PAY_FORM_NAME ?? val2.PAY_FORM_NAME;
					}
				}
				val.PaymentMethodName = paymentMethodName;
				if (ElectronicBillDataInput.Branch != null)
				{
					val.SellerAddress = ElectronicBillDataInput.Branch.ADDRESS;
					val.SellerBankAccount = ElectronicBillDataInput.Branch.ACCOUNT_NUMBER;
					val.SellerBankName = ElectronicBillDataInput.Branch.BANK_INFO;
					val.SellerLegalName = ElectronicBillDataInput.Branch.BRANCH_NAME;
					val.SellerTaxCode = ElectronicBillDataInput.Branch.TAX_CODE;
				}
				val.ExchangeRate = 1;
				val.OptionUserDefined = ProcessUserDefined();
				val.CustomField1 = ElectronicBillDataInput.Transaction.TDL_TREATMENT_CODE;
				if (!string.IsNullOrWhiteSpace(HisConfigCFG.ElectronicBillXmlInvoicePlus))
				{
					string[] array = HisConfigCFG.ElectronicBillXmlInvoicePlus.Split('|');
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					string[] array2 = array;
					foreach (string text in array2)
					{
						string[] array3 = text.Split(':');
						if (array3.Count() > 1)
						{
							string value = text.Replace(array3[0] + ":", "");
							if (!string.IsNullOrEmpty(value))
							{
								dictionary[array3[0]] = value;
							}
							else
							{
								dictionary[array3[0]] = null;
							}
						}
					}
					Dictionary<string, string> dictionary2 = General.ProcessDicValueString(ElectronicBillDataInput);
					PropertyInfo[] array4 = Properties.Get(typeof(CreateInvoiceV2));
					foreach (KeyValuePair<string, string> item in dictionary)
					{
						string text2 = item.Value;
						if (!string.IsNullOrWhiteSpace(text2))
						{
							foreach (KeyValuePair<string, string> item2 in dictionary2)
							{
								text2 = text2.Replace(item2.Key, item2.Value);
							}
						}
						PropertyInfo[] array5 = array4;
						foreach (PropertyInfo propertyInfo in array5)
						{
							if (propertyInfo.Name == item.Key)
							{
								propertyInfo.SetValue(val, text2);
								break;
							}
						}
					}
				}
				List<InvoiceDetailV2> productElectronicBill = GetProductElectronicBill();
				if (productElectronicBill != null && productElectronicBill.Count > 0)
				{
					val.OriginalInvoiceDetail = productElectronicBill;
					if (productElectronicBill != null && productElectronicBill.Count > 0)
					{
						decimal num3 = productElectronicBill.Sum((InvoiceDetailV2 s) => s.AmountWithoutVATOC.GetValueOrDefault());
						decimal num4 = productElectronicBill.Sum((InvoiceDetailV2 s) => s.Amount);
						decimal num5 = productElectronicBill.Sum((InvoiceDetailV2 s) => s.VatAmount);
						val.TotalSaleAmountOC = num4;
						val.TotalVATAmountOC = num5;
						val.TotalAmountOC = num4;
						val.PaymentMethodName = "TM/CK";
						val.TotalAmountInWords = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", num4)) + "đồng";
						val.TotalSaleAmount = num4;
						val.TotalVATAmount = num5;
						val.TotalAmount = num4;
						val.TotalAmountWithoutVAT = num3;
						val.TotalAmountWithoutVATOC = num3;
						List<IGrouping<string, InvoiceDetailV2>> list = (from o in productElectronicBill
							group o by o.VATRateName).ToList();
						foreach (IGrouping<string, InvoiceDetailV2> item3 in list)
						{
							if (!string.IsNullOrWhiteSpace(item3.Key))
							{
								if (val.TaxRateInfo == null)
								{
									val.TaxRateInfo = new List<TaxRateInfo>();
								}
								TaxRateInfo val3 = new TaxRateInfo();
								val3.VATRateName = item3.Key;
								val3.AmountWithoutVATOC = item3.Sum((InvoiceDetailV2 s) => s.AmountWithoutVATOC.GetValueOrDefault());
								val3.VATAmountOC = item3.Sum((InvoiceDetailV2 s) => s.VATAmountOC.GetValueOrDefault());
								val.TaxRateInfo.Add(val3);
							}
						}
					}
				}
			}
			return val;
		}

		private UserDefined ProcessUserDefined()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			UserDefined val = new UserDefined();
			val.MainCurrency = "VND";
			val.AmountDecimalDigits = "0";
			val.AmountOCDecimalDigits = "0";
			val.CoefficientDecimalDigits = "0";
			val.ExchangRateDecimalDigits = "0";
			val.QuantityDecimalDigits = "0";
			val.UnitPriceDecimalDigits = "0";
			val.UnitPriceOCDecimalDigits = "0";
			return val;
		}

		private List<InvoiceDetailV2> GetProductElectronicBill()
		{
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Expected O, but got Unknown
			string text = HisConfigs.Get<string>("HIS.DESKTOP.LIBRARY.ELECTRONIC_BILL.CONFIG");
			bool flag = text.StartsWith("MISA");
			bool flag2 = false;
			if (flag)
			{
				string[] array = text.Split('|');
				if (array.Length >= 9 && array[7] == "1")
				{
					flag2 = true;
				}
			}
			List<InvoiceDetailV2> list = new List<InvoiceDetailV2>();
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
				int num = 1;
				foreach (ProductBase item in list2)
				{
					InvoiceDetailV2 val = new InvoiceDetailV2();
					val.ItemType = 1;
					val.LineNumber = num;
					val.ItemCode = ((!string.IsNullOrWhiteSpace(item.ProdCode)) ? item.ProdCode : (GetFirstWord(item.ProdName) ?? " "));
					val.ItemName = item.ProdName;
					val.UnitName = item.ProdUnit;
					val.Quantity = item.ProdQuantity.GetValueOrDefault();
					val.Amount = item.Amount;
					val.UnitPrice = item.ProdPrice.GetValueOrDefault();
					val.VatAmount = 0m;
					val.AmountOC = item.Amount;
					if (LoaiHoaDon == VATType.GiaTriGiaTang)
					{
						val.VATRateName = "KCT";
						val.AmountWithoutVATOC = item.Amount;
					}
					else
					{
						val.AmountWithoutVATOC = default(decimal);
					}
					val.VATAmountOC = default(decimal);
					val.SortOrder = num;
					list.Add(val);
					num++;
				}
			}
			else if (flag2 && obj.GetType() == typeof(List<ProductBasePlus>))
			{
				List<ProductBasePlus> list3 = (List<ProductBasePlus>)obj;
				if (list3 == null || list3.Count == 0)
				{
					throw new Exception("Không có thông tin chi tiết dịch vụ.");
				}
				int num2 = 1;
				string vATRateName = "";
				foreach (ProductBasePlus item2 in list3)
				{
					InvoiceDetailV2 val2 = new InvoiceDetailV2();
					val2.ItemType = 1;
					val2.LineNumber = num2;
					val2.ItemCode = item2.ProdCode;
					val2.ItemName = item2.ProdName;
					val2.UnitName = item2.ProdUnit;
					val2.Quantity = item2.ProdQuantity.GetValueOrDefault();
					val2.UnitPrice = item2.ProdPrice.GetValueOrDefault();
					val2.VatAmount = item2.TaxAmount.GetValueOrDefault();
					val2.VATAmountOC = item2.TaxAmount.GetValueOrDefault();
					if (item2.TaxPercentage.HasValue)
					{
						switch (item2.TaxPercentage)
						{
						case 0:
							vATRateName = "0%";
							break;
						case 1:
							vATRateName = "5%";
							break;
						case 2:
							vATRateName = "10%";
							break;
						case 3:
							vATRateName = "8%";
							break;
						default:
							vATRateName = item2.TaxPercentage + "%";
							break;
						}
					}
					val2.VATRateName = vATRateName;
					val2.AmountWithoutVATOC = item2.AmountWithoutTax.GetValueOrDefault();
					val2.Amount = item2.Amount;
					val2.AmountOC = item2.Amount;
					val2.AmountAfterTax = item2.Amount;
					val2.SortOrder = num2;
					list.Add(val2);
					num2++;
				}
			}
			return list;
		}

		private string GetFirstWord(string name)
		{
			string text = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(name))
				{
					List<string> list = name.Split(' ', ',', '.').ToList();
					foreach (string item in list)
					{
						if (!string.IsNullOrWhiteSpace(item))
						{
							text += item[0].ToString().ToUpper();
						}
					}
				}
			}
			catch (Exception ex)
			{
				text = name.Split(' ', ',', '.').FirstOrDefault();
				LogSystem.Error(ex);
			}
			return text;
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
	}
}
