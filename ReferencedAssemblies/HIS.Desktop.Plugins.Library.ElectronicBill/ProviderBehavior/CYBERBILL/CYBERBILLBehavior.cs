using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo;
using HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.CYBERBILL.Model;
using HIS.Desktop.Plugins.Library.ElectronicBill.Template;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.ProviderBehavior.CYBERBILL
{
	public class CYBERBILLBehavior : IRun
	{
		private const string SUCCESS_CODE = "01";

		private string apiConvertInvoice = "";

		private LoginResultCyberbill login { get; set; }

		private TemplateEnum.TYPE TempType { get; set; }

		private string serviceConfig { get; set; }

		private string accountConfig { get; set; }

		private string serviceUrl { get; set; }

		private ElectronicBillDataInput ElectronicBillDataInput { get; set; }

		private LoginDataCyberbill adoLogin { get; set; }

		private OutputElectronicBill OEBill { get; set; }

		private OutputReplaceElectronicBill ORPEBill { get; set; }

		private OutputSendAndSignElectronicBill OSASEBill { get; set; }

		private OutputConvertElectronicBill OCoEBill { get; set; }

		private OutputSignElectronicBill OSEBill { get; set; }

		private OutputCancelElectronicBill OCaEBill { get; set; }

		public CYBERBILLBehavior(ElectronicBillDataInput electronicBillDataInput, string serviceConfig, string accountConfig)
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
				string apiChuyenDoiHoaDon = null;
				if (Check(electronicBillType, ref electronicBillResult))
				{
					TempType = _templateType;
					string[] array = serviceConfig.Split('|');
					serviceUrl = array[1];
					if (string.IsNullOrEmpty(serviceUrl))
					{
						LogSystem.Error("Khong tim thay dia chi Webservice URL");
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, "Không tìm thấy địa chỉ Webservice URL");
						return electronicBillResult;
					}
					if (array.Count() >= 5 && array[4] == "1")
					{
						apiChuyenDoiHoaDon = array[3];
					}
					string[] array2 = accountConfig.Split('|');
					adoLogin = new LoginDataCyberbill();
					adoLogin.username = array2[0].Trim();
					adoLogin.password = array2[1].Trim();
					adoLogin.doanhnghiep_mst = ElectronicBillDataInput.Branch.TAX_CODE;
					login = ProcessLogin(adoLogin);
					if (login == null || login.result == null || login.error != null)
					{
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<LoginResultCyberbill>((Expression<Func<LoginResultCyberbill>>)(() => login)), (object)login));
						ElectronicBillResultUtil.Set(ref electronicBillResult, false, login.error.details);
						return electronicBillResult;
					}
					switch (electronicBillType)
					{
					case ElectronicBillType.ENUM.CREATE_INVOICE:
						if (ElectronicBillDataInput.Transaction != null && ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue)
						{
							GuiHoaDonThayThe(ref electronicBillResult);
							break;
						}
						if (array.Count() >= 3 && array[2] == "1")
						{
							GuiVaKyHoaDonGoc(ref electronicBillResult);
							break;
						}
						GuiHoaDonGoc(ref electronicBillResult);
						KyHoaDon(ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.GET_INVOICE_LINK:
						ChuyenDoiHoaDon(ref electronicBillResult, apiChuyenDoiHoaDon);
						break;
					case ElectronicBillType.ENUM.DELETE_INVOICE:
						break;
					case ElectronicBillType.ENUM.CANCEL_INVOICE:
						HuyHoaDon(ref electronicBillResult);
						break;
					case ElectronicBillType.ENUM.CONVERT_INVOICE:
						CyberbillChuyenDoiHoaDon(ref electronicBillResult, apiChuyenDoiHoaDon);
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

		private LoginResultCyberbill ProcessLogin(LoginDataCyberbill obj)
		{
			LoginResultCyberbill loginResultCyberbill = null;
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)obj);
				loginResultCyberbill = ApiConsumerV2.CreateRequest<LoginResultCyberbill>("POST", serviceUrl, "api/services/hddtws/Authentication/GetToken", null, null, sendJsonData);
			}
			catch (Exception ex)
			{
				loginResultCyberbill = null;
				LogSystem.Error(ex);
			}
			return loginResultCyberbill;
		}

		private InputElectronicBill IEBill()
		{
			InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
			InputElectronicBill inputElectronicBill = new InputElectronicBill();
			inputElectronicBill.doanhnghiep_mst = ElectronicBillDataInput.Branch.TAX_CODE;
			inputElectronicBill.loaihoadon_ma = ElectronicBillDataInput.TemplateCode;
			inputElectronicBill.mauso = ElectronicBillDataInput.TemplateCode;
			inputElectronicBill.ma_tracuu = "";
			inputElectronicBill.kyhieu = ElectronicBillDataInput.SymbolCode;
			inputElectronicBill.sophieu = "";
			inputElectronicBill.ma_hoadon = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
			inputElectronicBill.ngaylap = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			inputElectronicBill.dnmua_mst = data.BuyerTaxCode;
			inputElectronicBill.dnmua_ten = data.BuyerOrganization;
			inputElectronicBill.dnmua_tennguoimua = data.BuyerName;
			inputElectronicBill.dnmua_diachi = data.BuyerAddress;
			inputElectronicBill.dnmua_sdt = data.BuyerPhone;
			inputElectronicBill.dnmua_email = data.BuyerEmail;
			inputElectronicBill.dnmua_cccd = ((!string.IsNullOrWhiteSpace(data.BuyerIdentityNumber)) ? data.BuyerIdentityNumber : data.BuyerCCCD);
			inputElectronicBill.dnmua_mqhns = ((ElectronicBillDataInput.Transaction.BUYER_TYPE == 2) ? data.BuyerTaxCode : "");
			inputElectronicBill.dnmua_mqhns = ((!string.IsNullOrWhiteSpace(ElectronicBillDataInput.Transaction.BUYER_SOCIAL_RELATIONS_CODE)) ? ElectronicBillDataInput.Transaction.BUYER_SOCIAL_RELATIONS_CODE : "");
			inputElectronicBill.thanhtoan_phuongthuc = 3;
			inputElectronicBill.thanhtoan_phuongthuc_ten = "Tiền mặt/Chuyển khoản";
			inputElectronicBill.thanhtoan_taikhoan = data.BuyerAccountNumber;
			inputElectronicBill.thanhtoan_nganhang = "";
			inputElectronicBill.tiente_ma = "";
			inputElectronicBill.tygiangoaite = 0m;
			inputElectronicBill.tongtien_chietkhau = 0m;
			inputElectronicBill.ghichu = "";
			inputElectronicBill.tienthue = 0m;
			inputElectronicBill.nguoilap = adoLogin.username;
			inputElectronicBill.khachhang_ma = ElectronicBillDataInput.Transaction.TDL_PATIENT_CODE;
			inputElectronicBill.matracuuhtkhac = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
			inputElectronicBill.dschitiet = DSCT();
			inputElectronicBill.dsthuesuat = new List<DanhSachThue>();
			List<DanhSachThue> list = new List<DanhSachThue>();
			if (inputElectronicBill.dschitiet != null && inputElectronicBill.dschitiet.Count > 0)
			{
				inputElectronicBill.tongtien_chuavat = inputElectronicBill.dschitiet.Sum((DanhSachChiTiet o) => o.tongtien_chuathue.GetValueOrDefault());
				inputElectronicBill.tongtien_covat = inputElectronicBill.dschitiet.Sum((DanhSachChiTiet o) => o.tongtien_cothue.GetValueOrDefault());
				List<IGrouping<string, DanhSachChiTiet>> list2 = (from o in inputElectronicBill.dschitiet
					group o by o.mathue).ToList();
				foreach (IGrouping<string, DanhSachChiTiet> item in list2)
				{
					DanhSachThue danhSachThue = new DanhSachThue();
					danhSachThue.mathue = item.First().mathue;
					danhSachThue.tongtien_chiuthue = item.Sum((DanhSachChiTiet o) => o.tongtien_chuathue.GetValueOrDefault());
					danhSachThue.tongtien_thue = item.Sum((DanhSachChiTiet o) => o.tongtien_cothue.GetValueOrDefault()) - item.Sum((DanhSachChiTiet o) => o.tongtien_chuathue.GetValueOrDefault());
					list.Add(danhSachThue);
				}
			}
			inputElectronicBill.dsthuesuat = list;
			return inputElectronicBill;
		}

		private List<DanhSachChiTiet> DSCT()
		{
			List<DanhSachChiTiet> list = new List<DanhSachChiTiet>();
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
						DanhSachChiTiet danhSachChiTiet = new DanhSachChiTiet();
						danhSachChiTiet.stt = num++;
						danhSachChiTiet.hanghoa_loai = 0;
						danhSachChiTiet.khuyenmai = 0;
						danhSachChiTiet.ma = item.ProdCode;
						danhSachChiTiet.ten = item.ProdName;
						danhSachChiTiet.donvitinh = item.ProdUnit;
						if (item.ProdQuantity.HasValue)
						{
							danhSachChiTiet.soluong = Math.Round(item.ProdQuantity.Value, 0, MidpointRounding.AwayFromZero);
						}
						if (item.ProdPrice.HasValue)
						{
							danhSachChiTiet.dongia = Math.Round(item.ProdPrice.Value, 0, MidpointRounding.AwayFromZero);
						}
						danhSachChiTiet.phantram_chietkhau = 0m;
						danhSachChiTiet.tongtien_chietkhau = 0m;
						danhSachChiTiet.phikhac_tyle = 0m;
						danhSachChiTiet.phikhac_sotien = 0m;
						danhSachChiTiet.tongtien_chuathue = item.AmountWithoutTax;
						if (!item.TaxPercentage.HasValue)
						{
							danhSachChiTiet.mathue = "-1";
						}
						else if (item.TaxPercentage == 1)
						{
							danhSachChiTiet.mathue = "5";
						}
						else if (item.TaxPercentage == 2)
						{
							danhSachChiTiet.mathue = "10";
						}
						else if (item.TaxPercentage == 3)
						{
							danhSachChiTiet.mathue = "8";
						}
						else if (item.TaxPercentage == 0)
						{
							danhSachChiTiet.mathue = "0";
						}
						danhSachChiTiet.tongtien_cothue = item.Amount;
						danhSachChiTiet.tyletinhthue = 0;
						list.Add(danhSachChiTiet);
					}
				}
				else
				{
					int num2 = 1;
					List<ProductBase> list3 = (List<ProductBase>)obj;
					foreach (ProductBase item2 in list3)
					{
						DanhSachChiTiet danhSachChiTiet2 = new DanhSachChiTiet();
						danhSachChiTiet2.stt = num2++;
						danhSachChiTiet2.hanghoa_loai = 1;
						danhSachChiTiet2.khuyenmai = 0;
						danhSachChiTiet2.ma = item2.ProdCode;
						danhSachChiTiet2.ten = item2.ProdName;
						danhSachChiTiet2.donvitinh = item2.ProdUnit;
						danhSachChiTiet2.soluong = item2.ProdQuantity;
						danhSachChiTiet2.dongia = item2.ProdPrice;
						danhSachChiTiet2.phantram_chietkhau = 0m;
						danhSachChiTiet2.tongtien_chietkhau = 0m;
						danhSachChiTiet2.phikhac_tyle = 0m;
						danhSachChiTiet2.phikhac_sotien = 0m;
						danhSachChiTiet2.tongtien_chuathue = item2.Amount;
						danhSachChiTiet2.mathue = "-1";
						danhSachChiTiet2.tongtien_cothue = item2.Amount;
						danhSachChiTiet2.tyletinhthue = 0;
						list.Add(danhSachChiTiet2);
					}
				}
			}
			catch (Exception ex)
			{
				list = new List<DanhSachChiTiet>();
				LogSystem.Error(ex);
			}
			return list;
		}

		private void GuiHoaDonGoc(ref ElectronicBillResult result)
		{
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)IEBill());
				OEBill = ApiConsumerV2.CreateRequest<OutputElectronicBill>("POST", serviceUrl, "api/services/hddtws/GuiHoadon/GuiHoadonGoc", login.result.access_token, sendJsonData);
				result.InvoiceSys = "CYBERBILL";
				if (OEBill != null && OEBill.result != null)
				{
					if (OEBill.result.maketqua == "01")
					{
						result.Success = true;
						result.InvoiceCode = OEBill.result.magiaodich;
						result.InvoiceLookupCode = OEBill.result.magiaodich;
						result.InvoiceLoginname = adoLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now);
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (OEBill.result != null) ? OEBill.result.motaketqua : "Gửi hóa đơn gốc thất bại");
					}
				}
				else if (OEBill == null || (OEBill != null && OEBill.error != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (OEBill != null && OEBill.error != null) ? OEBill.error.details : "Gửi hóa đơn gốc thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private InputSignElectronicBill ISEBill()
		{
			InputSignElectronicBill inputSignElectronicBill = new InputSignElectronicBill();
			inputSignElectronicBill.doanhnghiep_mst = ElectronicBillDataInput.Branch.TAX_CODE;
			inputSignElectronicBill.magiaodich = OEBill.result.magiaodich;
			inputSignElectronicBill.ma_hoadon = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
			return inputSignElectronicBill;
		}

		private void KyHoaDon(ref ElectronicBillResult result)
		{
			try
			{
				if (!result.Success)
				{
					return;
				}
				string sendJsonData = JsonConvert.SerializeObject((object)ISEBill());
				OSEBill = ApiConsumerV2.CreateRequest<OutputSignElectronicBill>("POST", serviceUrl, "api/services/hddtws/XuLyHoaDon/KyHoaDonHSM", login.result.access_token, sendJsonData);
				result.InvoiceSys = "CYBERBILL";
				if (OSEBill != null && OSEBill.result != null)
				{
					if (OSEBill.result.maketqua == "01")
					{
						result.Success = true;
						result.InvoiceNumOrder = OSEBill.result.sohoadon;
						result.InvoiceLoginname = adoLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Get.Now();
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now);
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (OEBill.result != null) ? OEBill.result.motaketqua : "Ký hóa đơn gốc thất bại");
					}
				}
				else if (OSEBill == null || (OSEBill != null && OSEBill.error != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (OSEBill != null && OSEBill.error != null) ? OSEBill.error.details : "Ký hóa đơn HSM thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void GuiVaKyHoaDonGoc(ref ElectronicBillResult result)
		{
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)IEBill());
				OSASEBill = ApiConsumerV2.CreateRequest<OutputSendAndSignElectronicBill>("POST", serviceUrl, "api/services/hddtws/GuiHoaDon/GuiVaKyHoadonGocHSM", login.result.access_token, sendJsonData);
				result.InvoiceSys = "CYBERBILL";
				if (OSASEBill != null && OSASEBill.result != null)
				{
					if (OSASEBill.result.maketqua == "01")
					{
						result.Success = true;
						result.InvoiceCode = OSASEBill.result.magiaodich;
						result.InvoiceLookupCode = OSASEBill.result.magiaodich;
						result.InvoiceLoginname = adoLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now);
						result.InvoiceNumOrder = OSASEBill.result.sohoadon;
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (OSASEBill.result != null) ? OSASEBill.result.motaketqua : "Gửi và ký hóa đơn gốc HSM thất bại");
					}
				}
				else if (OSASEBill == null || (OSASEBill != null && OSASEBill.error != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (OSASEBill != null && OSASEBill.error != null) ? OSASEBill.error.details : "Gửi và ký hóa đơn gốc HSM thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private InputReplaceElectronicBill IReplaceBill()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			InvoiceInfoADO data = InvoiceInfoProcessor.GetData(ElectronicBillDataInput);
			InputReplaceElectronicBill inputReplaceElectronicBill = new InputReplaceElectronicBill();
			inputReplaceElectronicBill.doanhnghiep_mst = ElectronicBillDataInput.Branch.TAX_CODE;
			inputReplaceElectronicBill.loaihoadon_ma = ElectronicBillDataInput.TemplateCode;
			inputReplaceElectronicBill.mauso = ElectronicBillDataInput.TemplateCode;
			inputReplaceElectronicBill.kyhieu = ElectronicBillDataInput.SymbolCode;
			if (ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID.HasValue)
			{
				CommonParam val = new CommonParam();
				HisTransactionFilter val2 = new HisTransactionFilter();
				((FilterBase)val2).ID = ElectronicBillDataInput.Transaction.ORIGINAL_TRANSACTION_ID;
				HIS_TRANSACTION val3 = ((AdapterBase)new BackendAdapter(val)).Get<List<HIS_TRANSACTION>>("api/HisTransaction/Get", ApiConsumers.MosConsumer, (object)val2, val).ToList().FirstOrDefault();
				inputReplaceElectronicBill.hoadon_goc = val3.TRANSACTION_CODE;
			}
			inputReplaceElectronicBill.ma_hoadon = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
			inputReplaceElectronicBill.ngaylap = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			inputReplaceElectronicBill.dnmua_mst = data.BuyerTaxCode;
			inputReplaceElectronicBill.dnmua_ten = data.BuyerOrganization;
			inputReplaceElectronicBill.dnmua_cccd = ((!string.IsNullOrWhiteSpace(data.BuyerIdentityNumber)) ? data.BuyerIdentityNumber : data.BuyerCCCD);
			inputReplaceElectronicBill.dnmua_tennguoimua = data.BuyerName;
			inputReplaceElectronicBill.dnmua_diachi = data.BuyerAddress;
			inputReplaceElectronicBill.dnmua_sdt = data.BuyerPhone;
			inputReplaceElectronicBill.dnmua_email = data.BuyerEmail;
			inputReplaceElectronicBill.thanhtoan_phuongthuc = 3;
			inputReplaceElectronicBill.thanhtoan_phuongthuc_ten = "Tiền mặt/Chuyển khoản";
			inputReplaceElectronicBill.thanhtoan_taikhoan = data.BuyerAccountNumber;
			inputReplaceElectronicBill.thanhtoan_nganhang = "";
			inputReplaceElectronicBill.tiente_ma = "";
			inputReplaceElectronicBill.thanhtoan_thoihan = "";
			inputReplaceElectronicBill.ghichu = "";
			inputReplaceElectronicBill.tongtien_chuavat = 0m;
			inputReplaceElectronicBill.tienthue = 0m;
			inputReplaceElectronicBill.tongtien_covat = 0m;
			inputReplaceElectronicBill.nguoilap = adoLogin.username;
			inputReplaceElectronicBill.khachhang_ma = ElectronicBillDataInput.Transaction.TDL_PATIENT_CODE;
			inputReplaceElectronicBill.matracuuhtkhac = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
			inputReplaceElectronicBill.dschitiet = DSCT();
			inputReplaceElectronicBill.dsthuesuat = new List<DanhSachThue>();
			List<DanhSachThue> list = new List<DanhSachThue>();
			if (inputReplaceElectronicBill.dschitiet != null && inputReplaceElectronicBill.dschitiet.Count > 0)
			{
				inputReplaceElectronicBill.tongtien_chuavat = inputReplaceElectronicBill.dschitiet.Sum((DanhSachChiTiet o) => o.tongtien_chuathue.GetValueOrDefault());
				inputReplaceElectronicBill.tongtien_covat = inputReplaceElectronicBill.dschitiet.Sum((DanhSachChiTiet o) => o.tongtien_cothue.GetValueOrDefault());
				List<IGrouping<string, DanhSachChiTiet>> list2 = (from o in inputReplaceElectronicBill.dschitiet
					group o by o.mathue).ToList();
				foreach (IGrouping<string, DanhSachChiTiet> item in list2)
				{
					DanhSachThue danhSachThue = new DanhSachThue();
					danhSachThue.mathue = item.First().mathue;
					danhSachThue.tongtien_chiuthue = item.Sum((DanhSachChiTiet o) => o.tongtien_chuathue.GetValueOrDefault());
					danhSachThue.tongtien_thue = item.Sum((DanhSachChiTiet o) => o.tongtien_cothue.GetValueOrDefault()) - item.Sum((DanhSachChiTiet o) => o.tongtien_chuathue.GetValueOrDefault());
					list.Add(danhSachThue);
				}
			}
			inputReplaceElectronicBill.dsthuesuat = list;
			inputReplaceElectronicBill.hopdong_so = "";
			inputReplaceElectronicBill.hopdong_ngayky = "";
			inputReplaceElectronicBill.file_hopdong = "";
			return inputReplaceElectronicBill;
		}

		private void GuiHoaDonThayThe(ref ElectronicBillResult result)
		{
			try
			{
				string sendJsonData = JsonConvert.SerializeObject((object)IReplaceBill());
				LogSystem.Debug("INPUT ELECTRONICBILL: " + LogUtil.TraceData("Data", (object)IReplaceBill()));
				ORPEBill = ApiConsumerV2.CreateRequest<OutputReplaceElectronicBill>("POST", serviceUrl, "api/services/hddtws/GuiHoadon/GuiHoadonThayThe", login.result.access_token, sendJsonData);
				result.InvoiceSys = "CYBERBILL";
				if (ORPEBill != null && ORPEBill.result != null)
				{
					if (ORPEBill.result.maketqua == "01")
					{
						result.Success = true;
						result.InvoiceCode = ORPEBill.result.magiaodich;
						result.InvoiceLookupCode = ORPEBill.result.magiaodich;
						result.InvoiceLoginname = adoLogin.username;
						result.InvoiceTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now);
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (ORPEBill.result != null) ? ORPEBill.result.motaketqua : "Gửi hóa đơn thay thế thất bại");
					}
				}
				else if (ORPEBill == null || (ORPEBill != null && ORPEBill.error != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (ORPEBill != null && ORPEBill.error != null) ? ORPEBill.error.details : "Gửi hóa đơn thay thế thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private InputConvertElectronicBill ICoEBill()
		{
			InputConvertElectronicBill inputConvertElectronicBill = new InputConvertElectronicBill();
			inputConvertElectronicBill.doanhnghiep_mst = ElectronicBillDataInput.Branch.TAX_CODE;
			inputConvertElectronicBill.magiaodich = ElectronicBillDataInput.InvoiceCode;
			inputConvertElectronicBill.ma_hoadon = ElectronicBillDataInput.Transaction.TRANSACTION_CODE;
			return inputConvertElectronicBill;
		}

		private void ChuyenDoiHoaDon(ref ElectronicBillResult result, string apiChuyenDoiHoaDon)
		{
			try
			{
				if (ElectronicBillDataInput == null || string.IsNullOrEmpty(ElectronicBillDataInput.InvoiceCode))
				{
					return;
				}
				string text = JsonConvert.SerializeObject((object)ICoEBill());
				string requestUri = "api/services/hddtws/GuiHoaDon/TaiHoaDonPDF";
				if (!string.IsNullOrWhiteSpace(apiChuyenDoiHoaDon))
				{
					requestUri = apiChuyenDoiHoaDon.Trim();
				}
				OCoEBill = ApiConsumerV2.CreateRequest<OutputConvertElectronicBill>("POST", serviceUrl, requestUri, login.result.access_token, text);
				LogSystem.Debug(LogUtil.TraceData("CyberbillChuyenDoiHoaDon", (object)text));
				result.InvoiceSys = "CYBERBILL";
				if (OCoEBill != null && OCoEBill.result != null)
				{
					if (OCoEBill.result.maketqua == "01")
					{
						result.Success = true;
						string text2 = Application.StartupPath + "\\temp";
						if (!Directory.Exists(text2))
						{
							Directory.CreateDirectory(text2);
						}
						string text3 = text2 + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
						File.WriteAllBytes(text3, System.Convert.FromBase64String(OCoEBill.result.base64pdf));
						result.InvoiceLink = text3;
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (OCoEBill.result != null) ? OCoEBill.result.motaketqua : "Chuyển đổi hóa đơn thất bại");
					}
				}
				else if (OCoEBill == null || (OCoEBill != null && OCoEBill.error != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (OCoEBill != null && OCoEBill.error != null) ? OCoEBill.error.details : "Chuyển đổi hóa đơn thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void CyberbillChuyenDoiHoaDon(ref ElectronicBillResult result, string apiChuyenDoiHoaDon)
		{
			try
			{
				if (ElectronicBillDataInput == null || string.IsNullOrEmpty(ElectronicBillDataInput.InvoiceCode))
				{
					return;
				}
				string text = JsonConvert.SerializeObject((object)ICoEBill());
				string requestUri = "api/services/hddtws/QuanLyHoaDon/ChuyenDoiHoaDon";
				if (!string.IsNullOrWhiteSpace(apiChuyenDoiHoaDon))
				{
					requestUri = apiChuyenDoiHoaDon.Trim();
				}
				OCoEBill = ApiConsumerV2.CreateRequest<OutputConvertElectronicBill>("POST", serviceUrl, requestUri, login.result.access_token, text);
				LogSystem.Debug(LogUtil.TraceData("CyberbillChuyenDoiHoaDon", (object)text));
				result.InvoiceSys = "CYBERBILL";
				if (OCoEBill != null && OCoEBill.result != null)
				{
					if (OCoEBill.result.maketqua == "01")
					{
						result.Success = true;
						string text2 = Application.StartupPath + "\\temp";
						if (!Directory.Exists(text2))
						{
							Directory.CreateDirectory(text2);
						}
						string text3 = text2 + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                        File.WriteAllBytes(text3, System.Convert.FromBase64String(OCoEBill.result.base64pdf));
						result.InvoiceLink = text3;
					}
					else
					{
						result.Success = false;
						ElectronicBillResultUtil.Set(ref result, false, (OEBill.result != null) ? OEBill.result.motaketqua : "Chuyển đổi hóa đơn thất bại");
					}
				}
				else if (OCoEBill == null || (OCoEBill != null && OCoEBill.error != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (OCoEBill != null && OCoEBill.error != null) ? OCoEBill.error.details : "Chuyển đổi hóa đơn thất bại");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private InputCancelElectronicBill ICaBill()
		{
			InputCancelElectronicBill inputCancelElectronicBill = new InputCancelElectronicBill();
			inputCancelElectronicBill.doanhnghiep_mst = ElectronicBillDataInput.Branch.TAX_CODE;
			inputCancelElectronicBill.hoadon_loai = 7;
			inputCancelElectronicBill.loaihoadon_ma = ElectronicBillDataInput.TemplateCode;
			inputCancelElectronicBill.mauso = ElectronicBillDataInput.TemplateCode;
			inputCancelElectronicBill.kyhieu = ElectronicBillDataInput.SymbolCode;
			inputCancelElectronicBill.ma_hoadon = ElectronicBillDataInput.TransactionCode;
			inputCancelElectronicBill.hoadon_goc = ElectronicBillDataInput.TransactionCode;
			inputCancelElectronicBill.ngaylap = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			inputCancelElectronicBill.hopdong_so = "";
			inputCancelElectronicBill.hopdong_ngayky = "";
			inputCancelElectronicBill.file_hopdong = "";
			inputCancelElectronicBill.nguoilap = adoLogin.username;
			return inputCancelElectronicBill;
		}

		private void HuyHoaDon(ref ElectronicBillResult result)
		{
			try
			{
				if (ElectronicBillDataInput == null || string.IsNullOrEmpty(ElectronicBillDataInput.InvoiceCode))
				{
					return;
				}
				string sendJsonData = JsonConvert.SerializeObject((object)ICaBill());
				OCaEBill = ApiConsumerV2.CreateRequest<OutputCancelElectronicBill>("POST", serviceUrl, "api/services/hddtws/GuiHoaDon/GuiHoadonHuyBo", login.result.access_token, sendJsonData);
				result.InvoiceSys = "CYBERBILL";
				if (OCaEBill != null && OCaEBill.result != null)
				{
					if (OCaEBill.result.maketqua == "01")
					{
						result.Success = true;
						return;
					}
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (OCaEBill.result != null) ? OCaEBill.result.motaketqua : "Hủy hóa đơn thất bại");
				}
				else if (OCaEBill == null || (OCaEBill != null && OCaEBill.error != null))
				{
					result.Success = false;
					ElectronicBillResultUtil.Set(ref result, false, (OCaEBill != null && OCaEBill.error != null) ? OCaEBill.error.details : "Hủy hóa đơn thất bại");
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
				if (array.Length < 2)
				{
					throw new Exception("Sai định dạng cấu hình hệ thống.");
				}
				if (array[0] != "CYBERBILL")
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
