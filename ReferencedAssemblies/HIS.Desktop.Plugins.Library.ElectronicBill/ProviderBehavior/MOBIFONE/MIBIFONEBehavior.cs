using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MOBIFONE.Model;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.DateTime;
using Inventec.Common.EBillSoftDreams.Model;
using Inventec.Common.EBillSoftDreams.ModelXml;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.Number;
using Inventec.Common.String;
using MOS.EFMODEL.DataModels;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.MOBIFONE
{
	internal class MIBIFONEBehavior : IRun
	{
		private string CurrencyCode = "VND";

		private DataReferences InvoiceBook { get; set; }

		private LoginResultData login { get; set; }

		private TemplateEnum.TYPE TempType { get; set; }

		private string serviceConfig { get; set; }

		private string accountConfig { get; set; }

		private string serviceUrl { get; set; }

		private string cer_serial { get; set; }

		private List<HoaDon78Result> HoaDonData { get; set; }

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		private LoginData adoLogin { get; set; }

		public MIBIFONEBehavior(ElectronicBillDataInput electronicBillDataInput, string serviceConfig, string accountConfig)
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
					serviceUrl = array[1];
					if (array.Length > 2)
					{
						cer_serial = array[2];
					}
					string text = "";
					if (array.Length > 3)
					{
						text = array[3];
					}
					bool flag = !string.IsNullOrWhiteSpace(cer_serial) && text != "1";
					if (string.IsNullOrEmpty(serviceUrl))
					{
						LogSystem.Error("Khong tim thay dia chi Webservice URL");
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không tìm thấy địa chỉ Webservice URL");
						return electronicBillResult;
					}
					string[] array2 = accountConfig.Split('|');
					adoLogin = new LoginData();
					adoLogin.username = array2[0].Trim();
					adoLogin.password = array2[1].Trim();
					login = ProcessLogin(adoLogin);
					if (login == null)
					{
						LogSystem.Error(string.Format("{0}, {1}__{2}__{3}", login.error, serviceUrl, adoLogin.username, adoLogin.password));
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, login.error);
						return electronicBillResult;
					}
					switch (electronicBillType)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						ProcessGetDataReferences(ref electronicBillResult);
						ProcessCreateInvoice(ref electronicBillResult);
						if (flag)
						{
							ProcessSignInvoiceCertFile(ref electronicBillResult);
						}
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
						ProcessPrintInvoice(ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.DELETE_INVOICE:
						break;
					case ElectronicBillType.ENUM.CANCEL_INVOICE:
						ProcessCancelInvoice(ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.CONVERT_INVOICE:
						break;
					case ElectronicBillType.ENUM.CREATE_INVOICE_DATA:
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_INFO:
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
			try
			{
				if (ElectronicBillDataInput == null || string.IsNullOrEmpty(ElectronicBillDataInput.InvoiceCode))
				{
					return;
				}
				CancelInvResult data = ApiConsumerV2.CreateRequest<CancelInvResult>("GET", serviceUrl, string.Format("/api/Invoice68/uploadCanceledInv?id={0}", ElectronicBillDataInput.InvoiceCode), login.token, login.ma_dvcs, null);
				if (data != null && data.ok)
				{
					result.Success = true;
					result.InvoiceSys = "MOBIFONE";
					return;
				}
				result.Success = false;
				result.InvoiceSys = "MOBIFONE";
				LogSystem.Error("Tai hoa don file PDF that bai " + LogUtil.TraceData(LogUtil.GetMemberName<CancelInvResult>((Expression<Func<CancelInvResult>>)(() => data)), (object)data));
				ElectronicBillResultUtil.Set(ref result, false, data.error);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ProcessPrintInvoice(ref ElectronicBillResult result, bool inchuyendoi = true)
		{
			try
			{
				if (ElectronicBillDataInput == null || string.IsNullOrEmpty(ElectronicBillDataInput.InvoiceCode))
				{
					return;
				}
				byte[] data = ApiConsumerV2.CreateRequestGetByte("GET", serviceUrl, string.Format("/api/Invoice68/inHoadon?id={0}&type=PDF&inchuyendoi={1}", ElectronicBillDataInput.InvoiceCode, inchuyendoi), login.token, login.ma_dvcs);
				if (data != null)
				{
					string text = Application.StartupPath + "\\temp\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
					File.WriteAllBytes(text, data);
					result.Success = true;
					result.InvoiceSys = "MOBIFONE";
					result.InvoiceLink = text;
					return;
				}
				result.Success = false;
				result.InvoiceSys = "MOBIFONE";
				LogSystem.Error("Tai hoa don file PDF that bai " + LogUtil.TraceData(LogUtil.GetMemberName<byte[]>((Expression<Func<byte[]>>)(() => data)), (object)data));
				ElectronicBillResultUtil.Set(ref result, false, "Tải hóa đơn file PDF thất bại");
				ProcessPrintInvoice(ref result, false);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ProcessSignInvoiceCertFile(ref ElectronicBillResult result)
		{
			try
			{
				if (HoaDonData == null)
				{
					return;
				}
				string sendJsonData = JsonConvert.SerializeObject((object)DataSignInvoiceCertFile68Init());
				SignInvoiceCertFile68Result data = ApiConsumerV2.CreateRequest<SignInvoiceCertFile68Result>("POST", serviceUrl, "/api/Invoice68/SignInvoiceCertFile68", login.token, login.ma_dvcs, sendJsonData);
				if (data != null && data.ok && string.IsNullOrEmpty(data.error))
				{
					result.Success = true;
					result.InvoiceSys = "MOBIFONE";
					result.InvoiceCode = HoaDonData.First().data.hdon_id;
					result.InvoiceNumOrder = HoaDonData.First().data.khieu + HoaDonData.First().data.shdon;
                    result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)HoaDonData.First().data.nlap);
					result.InvoiceLoginname = adoLogin.username;
				}
				else
				{
					result.Success = false;
					result.InvoiceSys = "MOBIFONE";
					LogSystem.Error("Ky va gui hoa don toi CQT that bai " + LogUtil.TraceData(LogUtil.GetMemberName<SignInvoiceCertFile68Result>((Expression<Func<SignInvoiceCertFile68Result>>)(() => data)), (object)data));
					ElectronicBillResultUtil.Set(ref result, false, data.error);
					ProcessCreateInvoice(ref result, 3);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private SignInvoiceCertFile68Init DataSignInvoiceCertFile68Init()
		{
			SignInvoiceCertFile68Init signInvoiceCertFile68Init = null;
			try
			{
				signInvoiceCertFile68Init = new SignInvoiceCertFile68Init();
				SignInvoiceCertFile68Data signInvoiceCertFile68Data = new SignInvoiceCertFile68Data();
				signInvoiceCertFile68Data.branch_code = login.ma_dvcs;
				signInvoiceCertFile68Data.username = adoLogin.username;
				signInvoiceCertFile68Data.lsthdon_id = HoaDonData.Select((HoaDon78Result o) => o.data.hdon_id).ToList();
				signInvoiceCertFile68Data.cer_serial = cer_serial;
				signInvoiceCertFile68Data.type_cmd = ((HoaDonData.First().data.is_hdcma == 1) ? 200.ToString() : 203.ToString());
				signInvoiceCertFile68Data.is_api = "1";
				signInvoiceCertFile68Init.data = new List<SignInvoiceCertFile68Data> { signInvoiceCertFile68Data };
			}
			catch (Exception ex)
			{
				signInvoiceCertFile68Init = null;
				LogSystem.Error(ex);
			}
			return signInvoiceCertFile68Init;
		}

		private void ProcessCreateInvoice(ref ElectronicBillResult result, int editMode = 1)
		{
			try
			{
				if (InvoiceBook == null)
				{
					return;
				}
				string sendJsonData = JsonConvert.SerializeObject((object)DataHoaDon78Init(editMode));
				HoaDonData = ApiConsumerV2.CreateRequest<List<HoaDon78Result>>("POST", serviceUrl, "/api/Invoice68/SaveListHoadon78", login.token, login.ma_dvcs, sendJsonData);
				if (HoaDonData != null && HoaDonData.Count > 0 && HoaDonData.FirstOrDefault((HoaDon78Result o) => !string.IsNullOrEmpty(o.error)) == null)
				{
					if (editMode == 1)
					{
						result.Success = true;
						result.InvoiceCode = HoaDonData.First().data.hdon_id;
						result.InvoiceNumOrder = HoaDonData.First().data.khieu + HoaDonData.First().data.shdon;
						result.InvoiceLoginname = adoLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)HoaDonData.First().data.nlap);
					}
					result.InvoiceSys = "MOBIFONE";
				}
				else
				{
					result.Success = false;
					result.InvoiceSys = "MOBIFONE";
					LogSystem.Error("Tao hoa don that bai " + LogUtil.TraceData(LogUtil.GetMemberName<List<HoaDon78Result>>((Expression<Func<List<HoaDon78Result>>>)(() => HoaDonData)), (object)HoaDonData));
					ElectronicBillResultUtil.Set(ref result, false, (HoaDonData != null && HoaDonData.Count > 0) ? string.Join(", ", HoaDonData.Select((HoaDon78Result o) => o.error)) : "Tạo hóa đơn thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private HoaDon78Init DataHoaDon78Init(int editMode)
		{
			HoaDon78Init hoaDon78Init = null;
			try
			{
				InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
				hoaDon78Init = new HoaDon78Init();
				hoaDon78Init.editmode = editMode;
				HoaDon78Data hoaDon78Data = new HoaDon78Data();
				hoaDon78Data.cctbao_id = InvoiceBook.qlkhsdung_id;
				hoaDon78Data.hdon_id = ((editMode == 1) ? null : HoaDonData.First().data.hdon_id);
				hoaDon78Data.nlap = DateTime.Now.ToString("yyyy-MM-dd");
				hoaDon78Data.dvtte = CurrencyCode;
				hoaDon78Data.tgia = 1;
				hoaDon78Data.htttoan = "Tiền mặt/Chuyển khoản";
				hoaDon78Data.tnmua = data.BuyerName;
				hoaDon78Data.mnmua = data.BuyerCode;
				hoaDon78Data.mst = data.BuyerTaxCode;
				hoaDon78Data.sdtnmua = data.BuyerPhone;
				hoaDon78Data.email = data.BuyerEmail;
				hoaDon78Data.ten = data.BuyerOrganization;
				hoaDon78Data.dchi = data.BuyerAddress;
				hoaDon78Data.details = new List<HoaDon78Details>();
				HoaDon78Details hoaDon78Details = new HoaDon78Details();
				hoaDon78Details.data = new List<HoaDon78DetailsData>();
				int num = 0;
				List<ProductBase> productBaseElectronicBill = GetProductBaseElectronicBill();
				foreach (ProductBase item in productBaseElectronicBill)
				{
					num++;
					HoaDon78DetailsData hoaDon78DetailsData = new HoaDon78DetailsData();
					hoaDon78DetailsData.stt = num;
					hoaDon78DetailsData.ma = item.ProdCode;
					hoaDon78DetailsData.ten = item.ProdName;
					hoaDon78DetailsData.mdvtinh = item.ProdUnit;
					hoaDon78DetailsData.dgia = item.ProdPrice;
					hoaDon78DetailsData.sluong = item.ProdQuantity;
					hoaDon78DetailsData.tlckhau = default(decimal);
					hoaDon78DetailsData.tthue = default(decimal);
					hoaDon78DetailsData.thtien = item.Amount;
					hoaDon78DetailsData.tgtien = item.Amount;
					hoaDon78DetailsData.kmai = 1m;
					hoaDon78DetailsData.tsuat = "-1";
					hoaDon78Details.data.Add(hoaDon78DetailsData);
				}
				hoaDon78Data.details.Add(hoaDon78Details);
				hoaDon78Data.tgtcthue = ((hoaDon78Details.data != null && hoaDon78Details.data.Count > 0) ? hoaDon78Details.data.Sum((HoaDon78DetailsData o) => o.thtien.GetValueOrDefault()) : 0m);
				hoaDon78Data.tgtthue = ((hoaDon78Details.data != null && hoaDon78Details.data.Count > 0) ? hoaDon78Details.data.Sum((HoaDon78DetailsData o) => o.tthue.GetValueOrDefault()) : 0m);
				hoaDon78Data.tgtttbso = ((hoaDon78Details.data != null && hoaDon78Details.data.Count > 0) ? hoaDon78Details.data.Sum((HoaDon78DetailsData o) => o.tgtien.GetValueOrDefault()) : 0m);
				hoaDon78Data.tgtttbso_last = ((hoaDon78Details.data != null && hoaDon78Details.data.Count > 0) ? hoaDon78Details.data.Sum((HoaDon78DetailsData o) => o.tgtien.GetValueOrDefault()) : 0m);
				hoaDon78Data.mdvi = login.ma_dvcs;
				hoaDon78Data.tthdon = 0;
				hoaDon78Data.is_hdcma = ((!InvoiceBook.hthuc.Equals("K")) ? 1 : 0);
				hoaDon78Data.hoadon68_khac = new List<HoaDon78Khac>();
				HoaDon78Khac hoaDon78Khac = new HoaDon78Khac();
				hoaDon78Khac.data = new List<HoaDon78KhacData>();
				hoaDon78Data.hoadon68_phi = new List<HoaDon78Phi>();
				HoaDon78Phi hoaDon78Phi = new HoaDon78Phi();
				hoaDon78Phi.data = new List<HoaDon78PhiData>();
				hoaDon78Init.data = new List<HoaDon78Data> { hoaDon78Data };
			}
			catch (Exception ex)
			{
				hoaDon78Init = null;
				LogSystem.Error(ex);
			}
			return hoaDon78Init;
		}

		private void ProcessGetDataReferences(ref ElectronicBillResult result)
		{
			try
			{
				List<DataReferences> data = ApiConsumerV2.CreateRequest<List<DataReferences>>("GET", serviceUrl, "/api/System/GetDataReferencesByRefId?refId=RF00059", login.token, login.ma_dvcs, null);
				if (data != null)
				{
					result.Success = true;
					result.InvoiceSys = "MOBIFONE";
					List<DataReferences> list = data.Where((DataReferences o) => o.khhdon.Equals(ElectronicBillDataInput.SymbolCode)).ToList();
					if (list == null || list.Count == 0)
					{
						ElectronicBillResultUtil.Set(ref result, false, string.Format("Không tìm thấy thông tin ký hiệu hóa đơn {0} vui lòng kiểm tra lại hệ thống hóa đơn điện tử.", ElectronicBillDataInput.SymbolCode));
					}
					else
					{
						InvoiceBook = list.FirstOrDefault();
					}
				}
				else
				{
					result.Success = false;
					result.InvoiceSys = "MOBIFONE";
					LogSystem.Error("Lay thong tin phat hanh hoa don that bai " + LogUtil.TraceData(LogUtil.GetMemberName<List<DataReferences>>((Expression<Func<List<DataReferences>>>)(() => data)), (object)data));
					ElectronicBillResultUtil.Set(ref result, false, "Lấy thông tin phát hành hóa đơn thất bại");
				}
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

		private List<ProductBase> GetProductBaseElectronicBill()
		{
			List<ProductBase> list = new List<ProductBase>();
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
					foreach (ProductBasePlus item in list2)
					{
						ProductBase productBase = new ProductBase();
						DataObjectMapper.Map<ProductBase>((object)productBase, (object)item);
						list.Add(productBase);
					}
				}
				else
				{
					list = (List<ProductBase>)obj;
				}
			}
			catch (Exception ex)
			{
				list = new List<ProductBase>();
				LogSystem.Error(ex);
			}
			return list;
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

		private LoginResultData ProcessLogin(LoginData obj)
		{
			LoginResultData loginResultData = null;
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)obj);
				loginResultData = ApiConsumerV2.CreateRequest<LoginResultData>("POST", serviceUrl, "/api/Account/Login", null, null, sendJsonData);
			}
			catch (Exception ex)
			{
				loginResultData = null;
				LogSystem.Error(ex);
			}
			return loginResultData;
		}

		private bool Check(ElectronicBillType.ENUM _electronicBillTypeEnum, ref ElectronicBillResult electronicBillResult)
		{
			bool result = true;
			try
			{
				string[] array = serviceConfig.Split('|');
				if (array.Length < 2)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
				}
				if (array[0] != "MOBIFONE")
				{
					throw new Exception("Không đúng cấu hình nhà cung cấp MOBIFONE");
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
	}
}
