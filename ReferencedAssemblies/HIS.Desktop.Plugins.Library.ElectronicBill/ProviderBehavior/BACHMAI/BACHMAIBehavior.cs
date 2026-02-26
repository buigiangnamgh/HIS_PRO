using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.BACHMAI.Model;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.String;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProxyBehavior.BACHMAI
{
	internal class BACHMAIBehavior : IRun
	{
		private string ServiceConfig;

		private string AccountConfig;

		private const string CreateTotalCode = "itong_hop";

		private const string CreateDetailCode = "ichi_tiet";

		private const string DeleteTotalCode = "dtong_hop";

		private const string DeleteDetailCode = "dchi_tiet";

		private const string CreateInvoice_TT78 = "api/create";

		private const string DeleteInvoice_TT78 = "api/remove";

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		private string proxyUrl { get; set; }

		public BACHMAIBehavior(ElectronicBillDataInput _electronicBillDataInput, string serviceConfig, string accountConfig)
		{
			ElectronicBillDataInput = _electronicBillDataInput;
			ServiceConfig = serviceConfig;
			AccountConfig = accountConfig;
		}

		ElectronicBillResult IRun.Run(ElectronicBillType.ENUM _electronicBillTypeEnum, TemplateEnum.TYPE templateType)
		{
			ElectronicBillResult result = new ElectronicBillResult();
			try
			{
				if (Check(_electronicBillTypeEnum, ref result))
				{
					string[] array = ServiceConfig.Split('|');
					string value = array[1];
					if (string.IsNullOrEmpty(value))
					{
						LogSystem.Error("Khong tim thay dia chi Webservice URL");
						ElectronicBillResultUtil.Set(ref result, false, "Không tìm thấy địa chỉ Webservice URL");
						return result;
					}
					switch (_electronicBillTypeEnum)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						result = PrcessCreateInvoice(templateType);
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
						ElectronicBillResultUtil.Set(ref result, false, "Chưa tích hợp tính năng này");
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

		private ElectronicBillResult ProcessCancelInvoice()
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			string[] array = ServiceConfig.Split('|');
			string text = "";
			if (array.Count() > 2)
			{
				text = array[2];
			}
			if (text == "2")
			{
				E_BM_Data e_BM_Data = new E_BM_Data();
				e_BM_Data.ID_MASTER = ElectronicBillDataInput.TransactionCode;
				object obj = ApiConsumer.CreateRequest<object>(array[1], AccountConfig, "api/remove", "", e_BM_Data);
				electronicBillResult.Success = true;
				electronicBillResult.InvoiceSys = "BACH_MAI";
			}
			else
			{
				List<object> objData = new List<object> { ElectronicBillDataInput.TransactionCode };
				List<object> dchi_tiet = ApiConsumer.CreateRequest<List<object>>(array[1], AccountConfig, "", "dchi_tiet", objData);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<List<object>>((Expression<Func<List<object>>>)(() => dchi_tiet)), (object)dchi_tiet));
				List<object> dtong_hop = ApiConsumer.CreateRequest<List<object>>(array[1], AccountConfig, "", "dtong_hop", objData);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<List<object>>((Expression<Func<List<object>>>)(() => dtong_hop)), (object)dtong_hop));
				electronicBillResult.Success = true;
				electronicBillResult.InvoiceSys = "BACH_MAI";
			}
			return electronicBillResult;
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
				string[] array = ServiceConfig.Split('|');
				if (array.Length < 2)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
				}
				if (string.IsNullOrWhiteSpace(AccountConfig))
				{
					throw new Exception("Sai cấu hình tài khoản.");
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

		private ElectronicBillResult PrcessCreateInvoice(TemplateEnum.TYPE templateType)
		{
			ElectronicBillResult result = new ElectronicBillResult();
			try
			{
				if (ElectronicBillDataInput != null)
				{
					string[] array = ServiceConfig.Split('|');
					string idMaster = "";
					decimal discount = default(decimal);
					if (ElectronicBillDataInput.Transaction != null)
					{
						idMaster = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
						discount = ElectronicBillDataInput.Transaction.EXEMPTION.GetValueOrDefault();
					}
					else if (ElectronicBillDataInput.ListTransaction != null && ElectronicBillDataInput.ListTransaction.Count > 0)
					{
						List<string> source = ElectronicBillDataInput.ListTransaction.Select((V_HIS_TRANSACTION s) => s.TRANSACTION_CODE).ToList();
						source = source.OrderBy((string o) => o).ToList();
						idMaster = source.FirstOrDefault();
						discount = ElectronicBillDataInput.ListTransaction.Sum((V_HIS_TRANSACTION s) => s.EXEMPTION.GetValueOrDefault());
					}
					string text = "";
					if (array.Count() > 2)
					{
						text = array[2];
					}
					result = ((!(text == "2")) ? ProccessCreateV1(ElectronicBillDataInput, templateType, array[1], idMaster, discount) : ProccessCreateV2(ElectronicBillDataInput, templateType, array[1], idMaster, discount));
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private ElectronicBillResult ProccessCreateV2(ElectronicBillDataInput electronicBillDataInput, TemplateEnum.TYPE templateType, string ulr, string idMaster, decimal discount)
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			Tonghop_TT78 invoice = new Tonghop_TT78();
			if (ElectronicBillDataInput.Branch != null && !string.IsNullOrWhiteSpace(ElectronicBillDataInput.Branch.TAX_CODE))
			{
				invoice.ID_COMPANY = ElectronicBillDataInput.Branch.TAX_CODE.Replace("-", "");
			}
			invoice.ID_MASTER = idMaster;
			invoice.SODONHANG = idMaster;
			invoice.ID_MASTER_REF = idMaster;
			InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
			invoice.DIACHI = data.BuyerAddress ?? "";
			invoice.DIENTHOAINGUOIMUA = data.BuyerPhone ?? "";
			invoice.TENKHACHHANG = data.BuyerName;
			invoice.TENDONVI = data.BuyerOrganization ?? "";
			invoice.MASOTHUE = data.BuyerTaxCode ?? "";
			invoice.EMAILNGUOIMUA = data.BuyerEmail ?? "";
			invoice.FAXNGUOIMUA = "";
			invoice.MAKHACHHANG = data.BuyerCode;
			string hINHTHUCTT = data.PaymentMethod ?? "TM/CK";
			if (ElectronicBillDataInput.Transaction != null)
			{
				HIS_PAY_FORM val = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == ElectronicBillDataInput.Transaction.PAY_FORM_ID);
				if (val != null)
				{
					hINHTHUCTT = val.ELECTRONIC_PAY_FORM_NAME ?? val.PAY_FORM_NAME;
				}
			}
			invoice.HINHTHUCTT = hINHTHUCTT;
			string date = GetDate(data.TransactionTime);
			invoice.NGAYDONHANG = date;
			invoice.NOIMOTAIKHOAN = "";
			invoice.SOTAIKHOAN = "";
			invoice.LOAITIENTE = "VND";
			invoice.TYGIA = 1;
			invoice.TONGTIENCKGG = (long)Math.Round(discount, 0, MidpointRounding.AwayFromZero);
			invoice.MADICHVU = "04";
			invoice.TINHTRANGHOADON = "-3";
			invoice.DIEMTHU = "";
			if (ElectronicBillDataInput.Transaction != null)
			{
				V_HIS_CASHIER_ROOM val2 = BackendDataWorker.Get<V_HIS_CASHIER_ROOM>().FirstOrDefault((V_HIS_CASHIER_ROOM o) => o.ID == ElectronicBillDataInput.Transaction.CASHIER_ROOM_ID);
				if (val2 != null)
				{
					invoice.DIEMTHU = val2.CASHIER_ROOM_CODE ?? "";
					invoice.MACN_CH = val2.CASHIER_ROOM_CODE ?? "";
					invoice.TENCN_CH = val2.CASHIER_ROOM_NAME ?? "";
				}
				invoice.NVTHU = ElectronicBillDataInput.Transaction.CASHIER_USERNAME;
			}
			ProccessRoom(ElectronicBillDataInput, ref invoice);
			List<Chitiet_TT78> einvoiceLineV = GetEinvoiceLineV2(templateType, idMaster, ElectronicBillDataInput);
			if (invoice != null && einvoiceLineV != null && einvoiceLineV.Count > 0)
			{
				decimal d = default(decimal);
				decimal num = default(decimal);
				decimal d2 = default(decimal);
				decimal num2 = default(decimal);
				decimal d3 = default(decimal);
				decimal d4 = default(decimal);
				decimal d5 = default(decimal);
				decimal d6 = default(decimal);
				decimal d7 = default(decimal);
				decimal d8 = default(decimal);
				decimal d9 = default(decimal);
				foreach (Chitiet_TT78 item in einvoiceLineV)
				{
					if (item.THUESUAT == "5")
					{
						num += item.TIENTHUE;
						d2 += item.THANHTIEN;
					}
					else if (item.THUESUAT == "10")
					{
						num2 += item.TIENTHUE;
						d3 += item.THANHTIEN;
					}
					else if (item.THUESUAT == "8")
					{
						d9 += item.TIENTHUE;
						d8 += item.THANHTIEN;
					}
					else if (item.THUESUAT == "0")
					{
						d += item.TONGTIEN;
					}
					else if (item.THUESUAT == "KCT")
					{
						d4 += item.TONGTIEN;
					}
					d5 += item.THANHTIEN;
					d6 += item.TIENTHUE;
					d7 += item.TONGTIEN;
				}
				if (num == 0m && num2 == 0m)
				{
					d = default(decimal);
				}
				invoice.TONGTIEN0 = (long)Math.Round(d, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENVAT5 = (long)Math.Round(num, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENCHUAVAT5 = (long)Math.Round(d2, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENVAT10 = (long)Math.Round(num2, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENCHUAVAT10 = (long)Math.Round(d3, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENVATKHAC = (long)Math.Round(d9, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENCHUAVATKHAC = (long)Math.Round(d8, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENKCT = (long)Math.Round(d4, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENHANG = (long)Math.Round(d5, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENTHUE = (long)Math.Round(d6, 0, MidpointRounding.AwayFromZero);
				invoice.TONGTIENTT = (long)Math.Round(d7, 0, MidpointRounding.AwayFromZero);
				invoice.SOTIENBANGCHU = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", invoice.TONGTIENTT)) + "đồng";
			}
			E_BM_Invoice_TT78 e_BM_Invoice_TT = new E_BM_Invoice_TT78();
			e_BM_Invoice_TT.tonghop = invoice;
			e_BM_Invoice_TT.chitiet = einvoiceLineV;
			try
			{
				SendDataADO sendDataADO = ApiConsumer.CreateRequest<SendDataADO>(ulr, AccountConfig, "api/create", "", e_BM_Invoice_TT);
				if (sendDataADO != null && sendDataADO.data != null)
				{
					electronicBillResult.Success = true;
					electronicBillResult.InvoiceSys = "BACH_MAI";
					electronicBillResult.InvoiceCode = string.Format("{0}|{1}", "BACH_MAI", ElectronicBillDataInput.Transaction.NUM_ORDER);
					electronicBillResult.InvoiceNumOrder = "";
					electronicBillResult.InvoiceTime = Inventec.Common.DateTime.Get.Now();
				}
				else
				{
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, "");
					LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<Tonghop_TT78>((Expression<Func<Tonghop_TT78>>)(() => invoice)), (object)invoice));
				}
			}
			catch (Exception ex)
			{
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, ex.Message);
				LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<Tonghop_TT78>((Expression<Func<Tonghop_TT78>>)(() => invoice)), (object)invoice));
			}
			return electronicBillResult;
		}

		private void ProccessRoom(ElectronicBillDataInput inputData, ref Tonghop_TT78 tongHop)
		{
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Expected O, but got Unknown
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Expected O, but got Unknown
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (inputData.Treatment.TDL_TREATMENT_TYPE_ID == 1)
				{
					V_HIS_ROOM val = (inputData.Treatment.END_ROOM_ID.HasValue ? BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == inputData.Treatment.END_ROOM_ID) : null);
					if (val != null)
					{
						tongHop.MAKHOA = val.ROOM_CODE;
						tongHop.TENKHOA = val.ROOM_NAME;
						return;
					}
					HisSereServFilter val2 = new HisSereServFilter();
					val2.TREATMENT_ID = inputData.Treatment.ID;
					val2.TDL_SERVICE_TYPE_ID = 1L;
					List<HIS_SERE_SERV> sereServKham = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_SERE_SERV>>("api/HisSereServ/Get", ApiConsumers.MosConsumer, (object)val2, (CommonParam)null);
					if (sereServKham != null && sereServKham.Count > 0)
					{
						sereServKham = (from o in sereServKham
							orderby o.TDL_IS_MAIN_EXAM.GetValueOrDefault() descending, o.TDL_INTRUCTION_TIME descending
							select o).ToList();
						V_HIS_ROOM val3 = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == sereServKham.First().TDL_EXECUTE_ROOM_ID);
						if (val3 != null)
						{
							tongHop.MAKHOA = val3.ROOM_CODE;
							tongHop.TENKHOA = val3.ROOM_NAME;
						}
					}
				}
				else if (inputData.Treatment.LAST_DEPARTMENT_ID.HasValue)
				{
					HIS_DEPARTMENT val4 = BackendDataWorker.Get<HIS_DEPARTMENT>().FirstOrDefault((HIS_DEPARTMENT o) => o.ID == inputData.Treatment.LAST_DEPARTMENT_ID.Value);
					if (val4 != null)
					{
						tongHop.MAKHOA = val4.DEPARTMENT_CODE;
						tongHop.TENKHOA = val4.DEPARTMENT_NAME;
					}
				}
				else
				{
					HisDepartmentTranLastFilter val5 = new HisDepartmentTranLastFilter();
					val5.TREATMENT_ID = inputData.Treatment.ID;
					V_HIS_DEPARTMENT_TRAN val6 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<V_HIS_DEPARTMENT_TRAN>("api/HisDepartmentTran/GetLastByTreatmentId", ApiConsumers.MosConsumer, (object)val5, (CommonParam)null);
					if (val6 != null)
					{
						tongHop.MAKHOA = val6.DEPARTMENT_CODE;
						tongHop.TENKHOA = val6.DEPARTMENT_NAME;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private List<Chitiet_TT78> GetEinvoiceLineV2(TemplateEnum.TYPE templateType, string idMaster, ElectronicBillDataInput electronicBillDataInput)
		{
			List<Chitiet_TT78> list = new List<Chitiet_TT78>();
			try
			{
				if (templateType != TemplateEnum.TYPE.TemplateNhaThuoc)
				{
					TemplateFactory.ProcessDataSereServToSereServBill(templateType, ref electronicBillDataInput);
					IRunTemplate runTemplate = TemplateFactory.MakeIRun(templateType, electronicBillDataInput);
					object obj = runTemplate.Run();
					if (obj == null)
					{
						throw new Exception("Không có thông tin chi tiết dịch vụ.");
					}
					List<ProductBase> list2 = (List<ProductBase>)obj;
					int num = 1;
					int num2 = 1;
					int num3 = 1;
					foreach (ProductBase item in list2)
					{
						Chitiet_TT78 chitiet_TT = new Chitiet_TT78();
						chitiet_TT.ID_MASTER = idMaster;
						chitiet_TT.MAHANG = item.ProdCode ?? "";
						chitiet_TT.TENHANG = item.ProdName ?? "";
						chitiet_TT.DONVITINH = item.ProdUnit ?? "";
						chitiet_TT.SOTHUTU = num.ToString() ?? "";
						chitiet_TT.SOLUONG = Math.Round(item.ProdQuantity ?? 1m, 4, MidpointRounding.AwayFromZero);
						chitiet_TT.DONGIA = Math.Round(item.ProdPrice.GetValueOrDefault(), 4, MidpointRounding.AwayFromZero);
						chitiet_TT.THANHTIEN = Math.Round(item.Amount, 4, MidpointRounding.AwayFromZero);
						chitiet_TT.TONGTIEN = Math.Round(item.Amount, 4, MidpointRounding.AwayFromZero);
						chitiet_TT.THUESUAT = "KCT";
						if (item.IsBHYT)
						{
							chitiet_TT.IDSTT = num2;
							chitiet_TT.CHITIEU1 = "TRA_BHYT";
							chitiet_TT.MUCBHYTTRA = 1;
							chitiet_TT.CHITIEU2 = "1." + num2;
							num2++;
						}
						else
						{
							chitiet_TT.IDSTT = num3;
							chitiet_TT.CHITIEU1 = "TRA_100";
							chitiet_TT.MUCBHYTTRA = 0;
							chitiet_TT.CHITIEU2 = "2." + num3;
							num3++;
						}
						list.Add(chitiet_TT);
					}
				}
				else
				{
					IRunTemplate runTemplate2 = TemplateFactory.MakeIRun(templateType, ElectronicBillDataInput);
					object obj2 = runTemplate2.Run();
					if (obj2 == null)
					{
						throw new Exception("Loi phan tich listProductBase");
					}
					List<ProductBasePlus> list3 = (List<ProductBasePlus>)obj2;
					int num4 = 1;
					foreach (ProductBasePlus item2 in list3)
					{
						Chitiet_TT78 chitiet_TT2 = new Chitiet_TT78();
						chitiet_TT2.ID_MASTER = idMaster;
						chitiet_TT2.MAHANG = item2.ProdCode;
						chitiet_TT2.TENHANG = item2.ProdName;
						chitiet_TT2.DONVITINH = item2.ProdUnit;
						chitiet_TT2.IDSTT = num4;
						chitiet_TT2.SOTHUTU = num4.ToString() ?? "";
						if (item2.ProdQuantity.HasValue)
						{
							chitiet_TT2.SOLUONG = Math.Round(item2.ProdQuantity.Value, 4, MidpointRounding.AwayFromZero);
						}
						if (item2.ProdPrice.HasValue)
						{
							chitiet_TT2.DONGIA = Math.Round(item2.ProdPrice.Value, 2, MidpointRounding.AwayFromZero);
						}
						chitiet_TT2.TONGTIEN = Math.Round(item2.Amount, 2, MidpointRounding.AwayFromZero);
						chitiet_TT2.THUETTDB = "0";
						chitiet_TT2.TIENTHUE = Math.Round(item2.TaxAmount.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero);
						chitiet_TT2.THANHTIEN = Math.Round(item2.AmountWithoutTax.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero);
						if (item2.TaxPercentage == 1)
						{
							chitiet_TT2.THUESUAT = "5";
						}
						else if (item2.TaxPercentage == 2)
						{
							chitiet_TT2.THUESUAT = "10";
						}
						else if (item2.TaxPercentage == 3)
						{
							chitiet_TT2.THUESUAT = "8";
						}
						else if (item2.TaxPercentage == 0)
						{
							chitiet_TT2.THUESUAT = "0";
						}
						if (item2.IsBHYT)
						{
							chitiet_TT2.CHITIEU1 = "TRA_BHYT";
							chitiet_TT2.MUCBHYTTRA = 1;
						}
						else
						{
							chitiet_TT2.CHITIEU1 = "TRA_100";
							chitiet_TT2.MUCBHYTTRA = 0;
						}
						chitiet_TT2.CHITIEU2 = "1." + num4;
						list.Add(chitiet_TT2);
						num4++;
					}
				}
			}
			catch (Exception ex)
			{
				list = new List<Chitiet_TT78>();
				LogSystem.Error(ex);
			}
			return list;
		}

		private ElectronicBillDataInput MakeNewData(ElectronicBillDataInput ElectronicBillDataInput, List<HIS_SERE_SERV_BILL> listDetail)
		{
			ElectronicBillDataInput electronicBillDataInput = new ElectronicBillDataInput();
			electronicBillDataInput.SereServBill = listDetail;
			if (ElectronicBillDataInput != null)
			{
				electronicBillDataInput.Amount = listDetail.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
				electronicBillDataInput.Branch = ElectronicBillDataInput.Branch;
				electronicBillDataInput.InvoiceCode = ElectronicBillDataInput.InvoiceCode;
				electronicBillDataInput.IsTransactionList = ElectronicBillDataInput.IsTransactionList;
				electronicBillDataInput.ListTransaction = ElectronicBillDataInput.ListTransaction;
				electronicBillDataInput.PartnerInvoiceID = ElectronicBillDataInput.PartnerInvoiceID;
				electronicBillDataInput.SereServs = ElectronicBillDataInput.SereServs;
				electronicBillDataInput.SymbolCode = ElectronicBillDataInput.SymbolCode;
				electronicBillDataInput.TemplateCode = ElectronicBillDataInput.TemplateCode;
				electronicBillDataInput.Transaction = ElectronicBillDataInput.Transaction;
				electronicBillDataInput.Treatment = ElectronicBillDataInput.Treatment;
				electronicBillDataInput.LastPatientTypeAlter = ElectronicBillDataInput.LastPatientTypeAlter;
			}
			return electronicBillDataInput;
		}

		private ElectronicBillResult ProccessCreateV1(ElectronicBillDataInput electronicBillDataInput, TemplateEnum.TYPE templateType, string url, string idMaster, decimal discount)
		{
			ElectronicBillResult electronicBillResult = new ElectronicBillResult();
			E_BM_Invoice invoice = new E_BM_Invoice();
			if (ElectronicBillDataInput.Branch != null && !string.IsNullOrWhiteSpace(ElectronicBillDataInput.Branch.TAX_CODE))
			{
				invoice.id_company = ElectronicBillDataInput.Branch.TAX_CODE.Replace("-", "");
			}
			invoice.id_master = idMaster;
			invoice.sodonhang = idMaster;
			InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
			invoice.diachi = data.BuyerAddress ?? "";
			invoice.dienthoainguoimua = data.BuyerPhone ?? "";
			invoice.tenkhachhang = data.BuyerName;
			invoice.tendonvi = data.BuyerOrganization ?? "";
			invoice.masothue = data.BuyerTaxCode ?? "";
			invoice.emailnguoimua = data.BuyerEmail ?? "";
			invoice.faxnguoimua = "";
			invoice.maKhachHang = data.BuyerCode;
			invoice.hinhthuctt = data.PaymentMethod ?? "TM/CK";
			string date = GetDate(data.TransactionTime);
			invoice.ngaydonhang = date;
			invoice.noimotaikhoan = "";
			invoice.sotaikhoan = "";
			invoice.loaitiente = "VND";
			invoice.tygia = 1;
			invoice.tongtienckgg = (long)Math.Round(discount, 0, MidpointRounding.AwayFromZero);
			invoice.macn_ch = "vietsens";
			invoice.tencn_ch = "Tập đoàn công nghệ VIETSEN";
			List<E_BM_InvoiceDetail> einvoiceLine = GetEinvoiceLine(templateType, idMaster, ElectronicBillDataInput);
			ProcessTongTien(invoice, einvoiceLine);
			if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.KC_AMOUNT.HasValue && ElectronicBillDataInput.Transaction.KC_AMOUNT.Value > 0m)
			{
				invoice.tamung = (long)(ElectronicBillDataInput.Transaction.TREATMENT_DEPOSIT_AMOUNT.GetValueOrDefault() - ElectronicBillDataInput.Transaction.TREATMENT_REPAY_AMOUNT.GetValueOrDefault() - ElectronicBillDataInput.Transaction.TREATMENT_TRANSFER_AMOUNT.GetValueOrDefault());
			}
			if (invoice.tongtientt - invoice.tamung > 0)
			{
				invoice.khachhangconphaitra = invoice.tongtientt - invoice.tamung;
			}
			else if (invoice.tongtientt - invoice.tamung < 0)
			{
				invoice.khachhangnhanlai = invoice.tamung - invoice.tongtientt;
			}
			List<object> item = ProcessToObject(invoice);
			List<object> objData = new List<object>
			{
				new List<object> { item }
			};
			try
			{
				List<object> list = ApiConsumer.CreateRequest<List<object>>(url, AccountConfig, "", "itong_hop", objData);
				if (list != null && list.Count > 0)
				{
					if (list.First() != null && list.First().ToString() == "200")
					{
						List<object> list2 = new List<object>();
						foreach (E_BM_InvoiceDetail item3 in einvoiceLine)
						{
							List<object> item2 = ProcessToObject(item3);
							list2.Add(item2);
						}
						List<object> objData2 = new List<object> { list2 };
						List<object> list3 = ApiConsumer.CreateRequest<List<object>>(url, AccountConfig, "", "ichi_tiet", objData2);
						if (list3 != null && list3.Count > 0 && list3.First() != null && list3.First().ToString() == "200")
						{
							electronicBillResult.Success = true;
							electronicBillResult.InvoiceSys = "BACH_MAI";
							electronicBillResult.InvoiceCode = string.Format("{0}|{1}", "BACH_MAI", ElectronicBillDataInput.Transaction.NUM_ORDER);
							electronicBillResult.InvoiceNumOrder = "";
							electronicBillResult.InvoiceTime = Inventec.Common.DateTime.Get.Now();
						}
						else
						{
							ElectronicBillResultUtil.Set(ref electronicBillResult, false, string.Join(",", list));
							LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<E_BM_Invoice>((Expression<Func<E_BM_Invoice>>)(() => invoice)), (object)invoice));
							List<object> objData3 = new List<object> { invoice.id_master };
							List<object> delResult = ApiConsumer.CreateRequest<List<object>>(url, AccountConfig, "", "dtong_hop", objData3);
							LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<List<object>>((Expression<Func<List<object>>>)(() => delResult)), (object)delResult));
						}
					}
					else
					{
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, string.Join(",", list));
						LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<E_BM_Invoice>((Expression<Func<E_BM_Invoice>>)(() => invoice)), (object)invoice));
					}
				}
				else
				{
					ElectronicBillResultUtil.Set(ref electronicBillResult, false, "");
					LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<E_BM_Invoice>((Expression<Func<E_BM_Invoice>>)(() => invoice)), (object)invoice));
				}
			}
			catch (Exception ex)
			{
				ElectronicBillResultUtil.Set(ref electronicBillResult, false, ex.Message);
				LogSystem.Error("Tao hoa don dien tu that bai. " + LogUtil.TraceData(LogUtil.GetMemberName<E_BM_Invoice>((Expression<Func<E_BM_Invoice>>)(() => invoice)), (object)invoice));
			}
			return electronicBillResult;
		}

		private List<object> ProcessToObject<T>(T data)
		{
			List<object> list = new List<object>();
			try
			{
				Type typeFromHandle = typeof(T);
				PropertyInfo[] properties = typeFromHandle.GetProperties();
				PropertyInfo[] array = properties;
				foreach (PropertyInfo propertyInfo in array)
				{
					list.Add(propertyInfo.GetValue(data));
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return list;
		}

		private List<E_BM_InvoiceDetail> GetEinvoiceLine(TemplateEnum.TYPE templateType, string idMaster, ElectronicBillDataInput electronicBillDataInput)
		{
			List<E_BM_InvoiceDetail> list = new List<E_BM_InvoiceDetail>();
			try
			{
				if (templateType != TemplateEnum.TYPE.TemplateNhaThuoc)
				{
					TemplateFactory.ProcessDataSereServToSereServBill(templateType, ref electronicBillDataInput);
					List<HIS_SERE_SERV_BILL> list2 = electronicBillDataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
					List<HIS_SERE_SERV_BILL> list3 = electronicBillDataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
					if (list2 != null && list2.Count > 0)
					{
						var list4 = (from o in list2
							group o by new { o.TDL_SERVICE_ID, o.TDL_REAL_PRICE }).ToList();
						int num = 1;
						foreach (var item in list4)
						{
							E_BM_InvoiceDetail e_BM_InvoiceDetail = new E_BM_InvoiceDetail();
							e_BM_InvoiceDetail.id_master = idMaster;
							e_BM_InvoiceDetail.mahang = item.First().TDL_SERVICE_CODE;
							e_BM_InvoiceDetail.tenhang = item.First().TDL_SERVICE_NAME;
							HIS_SERVICE_UNIT val = BackendDataWorker.Get<HIS_SERVICE_UNIT>().FirstOrDefault((HIS_SERVICE_UNIT o) => item.First().TDL_SERVICE_UNIT_ID == o.ID);
							e_BM_InvoiceDetail.donvitinh = ((val != null) ? val.SERVICE_UNIT_NAME : "");
							e_BM_InvoiceDetail.sothutu = num;
							e_BM_InvoiceDetail.soluong = item.Sum((HIS_SERE_SERV_BILL s) => s.TDL_AMOUNT.GetValueOrDefault());
							e_BM_InvoiceDetail.dongia = Math.Round(item.First().TDL_REAL_PRICE.GetValueOrDefault(), 4, MidpointRounding.AwayFromZero);
							e_BM_InvoiceDetail.thanhtien = Math.Round(item.Sum((HIS_SERE_SERV_BILL s) => s.PRICE), 4, MidpointRounding.AwayFromZero);
							e_BM_InvoiceDetail.tongtien = Math.Round(item.Sum((HIS_SERE_SERV_BILL s) => s.PRICE), 4, MidpointRounding.AwayFromZero);
							e_BM_InvoiceDetail.chitieu1 = "TRA_BHYT";
							e_BM_InvoiceDetail.chitieu2 = "1." + num;
							e_BM_InvoiceDetail.thuesuat = null;
							e_BM_InvoiceDetail.mucbhtra = 0m;
							e_BM_InvoiceDetail.bhxhtra = (long)Math.Round(item.Sum((HIS_SERE_SERV_BILL s) => s.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault()), 4, MidpointRounding.AwayFromZero);
							list.Add(e_BM_InvoiceDetail);
							num++;
						}
					}
					if (list3 != null && list3.Count > 0)
					{
						var list5 = (from o in list3
							group o by new { o.TDL_SERVICE_ID, o.TDL_REAL_PRICE }).ToList();
						int num2 = 1;
						foreach (var item2 in list5)
						{
							E_BM_InvoiceDetail e_BM_InvoiceDetail2 = new E_BM_InvoiceDetail();
							e_BM_InvoiceDetail2.id_master = idMaster;
							e_BM_InvoiceDetail2.mahang = item2.First().TDL_SERVICE_CODE;
							e_BM_InvoiceDetail2.tenhang = item2.First().TDL_SERVICE_NAME;
							HIS_SERVICE_UNIT val2 = BackendDataWorker.Get<HIS_SERVICE_UNIT>().FirstOrDefault((HIS_SERVICE_UNIT o) => item2.First().TDL_SERVICE_UNIT_ID == o.ID);
							e_BM_InvoiceDetail2.donvitinh = ((val2 != null) ? val2.SERVICE_UNIT_NAME : "");
							e_BM_InvoiceDetail2.sothutu = num2;
							e_BM_InvoiceDetail2.soluong = Math.Round(item2.Sum((HIS_SERE_SERV_BILL s) => s.TDL_AMOUNT.GetValueOrDefault()), 4, MidpointRounding.AwayFromZero);
							e_BM_InvoiceDetail2.dongia = Math.Round(item2.First().TDL_REAL_PRICE.GetValueOrDefault(), 4, MidpointRounding.AwayFromZero);
							e_BM_InvoiceDetail2.thanhtien = Math.Round(item2.Sum((HIS_SERE_SERV_BILL s) => s.PRICE), 4, MidpointRounding.AwayFromZero);
							e_BM_InvoiceDetail2.tongtien = Math.Round(item2.Sum((HIS_SERE_SERV_BILL s) => s.PRICE), 4, MidpointRounding.AwayFromZero);
							e_BM_InvoiceDetail2.chitieu1 = "TRA_100";
							if (list2 == null || list2.Count <= 0)
							{
								e_BM_InvoiceDetail2.chitieu2 = "1." + num2;
							}
							else
							{
								e_BM_InvoiceDetail2.chitieu2 = "2." + num2;
							}
							e_BM_InvoiceDetail2.thuesuat = null;
							e_BM_InvoiceDetail2.mucbhtra = 0m;
							e_BM_InvoiceDetail2.bhxhtra = 0m;
							list.Add(e_BM_InvoiceDetail2);
							num2++;
						}
					}
				}
				else
				{
					IRunTemplate runTemplate = TemplateFactory.MakeIRun(templateType, ElectronicBillDataInput);
					object obj = runTemplate.Run();
					if (obj == null)
					{
						throw new Exception("Loi phan tich listProductBase");
					}
					List<ProductBasePlus> list6 = (List<ProductBasePlus>)obj;
					int num3 = 1;
					foreach (ProductBasePlus item3 in list6)
					{
						E_BM_InvoiceDetail e_BM_InvoiceDetail3 = new E_BM_InvoiceDetail();
						e_BM_InvoiceDetail3.id_master = idMaster;
						e_BM_InvoiceDetail3.mahang = item3.ProdCode;
						e_BM_InvoiceDetail3.tenhang = item3.ProdName;
						e_BM_InvoiceDetail3.donvitinh = item3.ProdUnit;
						e_BM_InvoiceDetail3.sothutu = num3;
						if (item3.ProdQuantity.HasValue)
						{
							e_BM_InvoiceDetail3.soluong = Math.Round(item3.ProdQuantity.Value, 4, MidpointRounding.AwayFromZero);
						}
						if (item3.ProdPrice.HasValue)
						{
							e_BM_InvoiceDetail3.dongia = Math.Round(item3.ProdPrice.Value, 2, MidpointRounding.AwayFromZero);
						}
						e_BM_InvoiceDetail3.tongtien = Math.Round(item3.Amount, 2, MidpointRounding.AwayFromZero);
						e_BM_InvoiceDetail3.thuettdb = 0m;
						e_BM_InvoiceDetail3.tienthue = Math.Round(item3.TaxAmount.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero);
						e_BM_InvoiceDetail3.thanhtien = Math.Round(item3.AmountWithoutTax.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero);
						if (item3.TaxPercentage == 1)
						{
							e_BM_InvoiceDetail3.thuesuat = "5";
						}
						else if (item3.TaxPercentage == 2)
						{
							e_BM_InvoiceDetail3.thuesuat = "10";
						}
						else if (item3.TaxPercentage == 3)
						{
							e_BM_InvoiceDetail3.thuesuat = "8";
						}
						else if (item3.TaxPercentage == 0)
						{
							e_BM_InvoiceDetail3.thuesuat = "0";
						}
						e_BM_InvoiceDetail3.chitieu1 = "TRA_100";
						e_BM_InvoiceDetail3.chitieu2 = "1." + num3;
						list.Add(e_BM_InvoiceDetail3);
						num3++;
					}
				}
			}
			catch (Exception ex)
			{
				list = new List<E_BM_InvoiceDetail>();
				LogSystem.Error(ex);
			}
			return list;
		}

		private void ProcessTongTien(E_BM_Invoice invoice, List<E_BM_InvoiceDetail> listEinvoiceLine)
		{
			try
			{
				if (invoice == null || listEinvoiceLine == null || listEinvoiceLine.Count <= 0)
				{
					return;
				}
				decimal d = default(decimal);
				decimal d2 = default(decimal);
				decimal d3 = default(decimal);
				decimal d4 = default(decimal);
				decimal d5 = default(decimal);
				decimal d6 = default(decimal);
				decimal d7 = default(decimal);
				decimal d8 = default(decimal);
				decimal d9 = default(decimal);
				foreach (E_BM_InvoiceDetail item in listEinvoiceLine)
				{
					switch (item.thuesuat)
					{
					case "0":
						d += item.tongtien;
						break;
					case "5":
						d2 += item.tongtien;
						d3 += item.thanhtien;
						break;
					case "10":
						d4 += item.tongtien;
						d5 += item.thanhtien;
						break;
					default:
						d6 += item.tongtien;
						break;
					}
					d7 += item.thanhtien;
					d8 += item.tienthue;
					d9 += item.tongtien;
				}
				invoice.tongtien0 = (long)Math.Round(d, 0, MidpointRounding.AwayFromZero);
				invoice.tongtienvat5 = (long)Math.Round(d2, 0, MidpointRounding.AwayFromZero);
				invoice.tongtienchuavat5 = (long)Math.Round(d3, 0, MidpointRounding.AwayFromZero);
				invoice.tongtienvat10 = (long)Math.Round(d4, 0, MidpointRounding.AwayFromZero);
				invoice.tongtienchuavat10 = (long)Math.Round(d5, 0, MidpointRounding.AwayFromZero);
				invoice.tongtienkct = (long)Math.Round(d6, 0, MidpointRounding.AwayFromZero);
				invoice.tongtienhang = (long)Math.Round(d7, 0, MidpointRounding.AwayFromZero);
				invoice.tongtienthue = (long)Math.Round(d8, 0, MidpointRounding.AwayFromZero);
				invoice.tongtientt = (long)Math.Round(d9, 0, MidpointRounding.AwayFromZero);
				invoice.sotienbangchu = Inventec.Common.String.Convert.CurrencyToVneseString(string.Format("{0:0.##}", invoice.tongtientt)) + "đồng";
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private string GetDate(long time)
		{
			string result = "";
			try
			{
				string text = time.ToString();
				if (text != null && text.Length >= 8)
				{
					result = new StringBuilder().Append(text.Substring(0, 4)).Append("-").Append(text.Substring(4, 2))
						.Append("-")
						.Append(text.Substring(6, 2))
						.ToString();
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
