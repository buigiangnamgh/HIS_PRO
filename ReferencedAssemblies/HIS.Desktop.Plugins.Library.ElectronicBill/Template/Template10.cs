using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Template
{
	internal class Template10 : IRunTemplate
	{
		private ElectronicBillDataInput DataInput;

		public Template10(ElectronicBillDataInput dataInput)
		{
			DataInput = dataInput;
		}

		public object Run()
		{
			List<ProductBase> list = new List<ProductBase>();
			if (DataInput.SereServBill != null && DataInput.SereServBill.Count > 0)
			{
				int decimals = 4;
				if (HisConfigCFG.RoundTransactionAmountOption == "1" || HisConfigCFG.RoundTransactionAmountOption == "2")
				{
					decimals = 0;
				}
				decimal num = default(decimal);
				List<HIS_SERE_SERV_BILL> list2 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 2 || o.TDL_SERVICE_TYPE_ID == 15).ToList();
				ProductBase productBase = new ProductBase();
				productBase.ProdCode = "TONG";
				productBase.ProdName = "Xét nghiệm (Medical test)";
				if (list2 != null && list2.Count > 0)
				{
					productBase.Amount = list2.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
					productBase.Amount = Math.Round(productBase.Amount, decimals, MidpointRounding.AwayFromZero);
				}
				num += productBase.Amount;
				list.Add(productBase);
				List<HIS_SERE_SERV_BILL> list3 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 3 || o.TDL_SERVICE_TYPE_ID == 5 || o.TDL_SERVICE_TYPE_ID == 10 || o.TDL_SERVICE_TYPE_ID == 9).ToList();
				ProductBase productBase2 = new ProductBase();
				productBase2.ProdCode = "TONG";
				productBase2.ProdName = "CĐHA - TDCN (Diagnostic imaging - Functional assessments)";
				if (list3 != null && list3.Count > 0)
				{
					productBase2.Amount = list3.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
					productBase2.Amount = Math.Round(productBase2.Amount, decimals, MidpointRounding.AwayFromZero);
				}
				num += productBase2.Amount;
				list.Add(productBase2);
				List<HIS_SERE_SERV_BILL> list4 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 6 || o.TDL_SERVICE_TYPE_ID == 7).ToList();
				ProductBase productBase3 = new ProductBase();
				productBase3.ProdCode = "TONG";
				productBase3.ProdName = "Thuốc - VTYT (Medication - Medical supplies)";
				if (list4 != null && list4.Count > 0)
				{
					productBase3.Amount = list4.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
					productBase3.Amount = Math.Round(productBase3.Amount, decimals, MidpointRounding.AwayFromZero);
				}
				num += productBase3.Amount;
				list.Add(productBase3);
				List<HIS_SERE_SERV_BILL> list5 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 11 || o.TDL_SERVICE_TYPE_ID == 4).ToList();
				ProductBase productBase4 = new ProductBase();
				productBase4.ProdCode = "TONG";
				productBase4.ProdName = "Thủ thuật - phẫu thuật (Medical procedures - Surgery)";
				if (list5 != null && list5.Count > 0)
				{
					productBase4.Amount = list5.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
					productBase4.Amount = Math.Round(productBase4.Amount, decimals, MidpointRounding.AwayFromZero);
				}
				num += productBase4.Amount;
				list.Add(productBase4);
				List<HIS_SERE_SERV_BILL> list6 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 1).ToList();
				ProductBase productBase5 = new ProductBase();
				productBase5.ProdCode = "TONG";
				productBase5.ProdName = "Tiền khám (Examination cost)";
				if (list6 != null && list6.Count > 0)
				{
					productBase5.Amount = list6.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
					productBase5.Amount = Math.Round(productBase5.Amount, decimals, MidpointRounding.AwayFromZero);
				}
				num += productBase5.Amount;
				list.Add(productBase5);
				List<HIS_SERE_SERV_BILL> list7 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 8).ToList();
				ProductBase productBase6 = new ProductBase();
				productBase6.ProdCode = "TONG";
				productBase6.ProdName = "Tiền giường (Hospital bed cost)";
				if (list7 != null && list7.Count > 0)
				{
					productBase6.Amount = list7.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
					productBase6.Amount = Math.Round(productBase6.Amount, decimals, MidpointRounding.AwayFromZero);
				}
				num += productBase6.Amount;
				list.Add(productBase6);
				List<long> allType = new List<long>
				{
					2L, 15L, 3L, 5L, 10L, 9L, 6L, 7L, 11L, 4L,
					1L, 8L
				};
				List<HIS_SERE_SERV_BILL> list8 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => !allType.Contains(o.TDL_SERVICE_TYPE_ID.GetValueOrDefault())).ToList();
				ProductBase productBase7 = new ProductBase();
				productBase7.ProdCode = "TONG";
				productBase7.ProdName = "Khác (Others)";
				if (list8 != null && list8.Count > 0)
				{
					productBase7.Amount = list8.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
					productBase7.Amount = Math.Round(productBase7.Amount, decimals, MidpointRounding.AwayFromZero);
				}
				num += productBase7.Amount;
				list.Add(productBase7);
				decimal d = default(decimal);
				if (DataInput.Transaction != null && DataInput.Transaction.TDL_BILL_FUND_AMOUNT.HasValue)
				{
					d = DataInput.Transaction.TDL_BILL_FUND_AMOUNT.GetValueOrDefault();
				}
				else if (DataInput.Transaction != null && DataInput.Transaction.HIS_BILL_FUND != null && DataInput.Transaction.HIS_BILL_FUND.Count > 0)
				{
					d = DataInput.Transaction.HIS_BILL_FUND.Sum((HIS_BILL_FUND s) => s.AMOUNT);
				}
				else if (DataInput.ListTransaction != null && DataInput.ListTransaction.Count > 0)
				{
					d = DataInput.ListTransaction.Sum((V_HIS_TRANSACTION o) => o.TDL_BILL_FUND_AMOUNT.GetValueOrDefault());
				}
				d = Math.Round(d, decimals, MidpointRounding.AwayFromZero);
				decimal d2 = DataInput.SereServBill.Sum((HIS_SERE_SERV_BILL o) => o.TDL_DISCOUNT.GetValueOrDefault());
				d2 = Math.Round(d2, decimals, MidpointRounding.AwayFromZero);
				ProductBase productBase8 = new ProductBase();
				productBase8.ProdCode = "0";
				productBase8.ProdName = "Tổng viện phí (Total hospital cost)";
				productBase8.Amount = num + d2;
				list.Add(productBase8);
				ProductBase productBase9 = new ProductBase();
				productBase9.ProdCode = "0";
				productBase9.ProdName = "+ Tổng thu BN (Total Patient Revenue)";
				productBase9.Amount = num - d;
				productBase9.Amount = Math.Round(productBase9.Amount, decimals, MidpointRounding.AwayFromZero);
				list.Add(productBase9);
				ProductBase productBase10 = new ProductBase();
				productBase10.ProdCode = "0";
				productBase10.ProdName = "- Cùng chi trả BH (Patient payment)";
				productBase10.Amount = DataInput.SereServBill.Sum((HIS_SERE_SERV_BILL o) => o.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault() - o.TDL_DISCOUNT.GetValueOrDefault());
				productBase10.Amount = Math.Round(productBase10.Amount, decimals, MidpointRounding.AwayFromZero);
				list.Add(productBase10);
				if (productBase10.Amount < 0m)
				{
					productBase10.Amount = 0m;
				}
				ProductBase productBase11 = new ProductBase();
				productBase11.ProdCode = "0";
				productBase11.ProdName = "- Dịch vụ (Service cost)";
				productBase11.Amount = DataInput.SereServBill.Sum((HIS_SERE_SERV_BILL o) => o.PRICE - (o.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault() - o.TDL_DISCOUNT.GetValueOrDefault()));
				productBase11.Amount = Math.Round(productBase11.Amount, decimals, MidpointRounding.AwayFromZero);
				list.Add(productBase11);
				if (productBase11.Amount < 0m)
				{
					productBase11.Amount = 0m;
				}
				if (productBase10.Amount + productBase11.Amount > productBase9.Amount)
				{
					LogSystem.Info("_____productThuBn.Amount:" + productBase9.Amount + "_____productCctbh.Amount:" + productBase10.Amount + "_____productDichVu.Amount:" + productBase11.Amount);
					if (productBase9.Amount - productBase11.Amount > 0m)
					{
						productBase10.Amount = productBase9.Amount - productBase11.Amount;
					}
					else if (productBase9.Amount - productBase10.Amount > 0m)
					{
						productBase11.Amount = productBase9.Amount - productBase10.Amount;
					}
					else
					{
						decimal num2 = productBase10.Amount + productBase11.Amount - productBase9.Amount;
						if (productBase11.Amount - num2 > 0m)
						{
							productBase11.Amount -= num2;
						}
						else if (productBase10.Amount - num2 > 0m)
						{
							productBase10.Amount -= num2;
						}
						else
						{
							productBase10.Amount = 0m;
							productBase11.Amount = productBase9.Amount;
						}
					}
				}
				ProductBase productBase12 = new ProductBase();
				productBase12.ProdCode = "0";
				productBase12.ProdName = "+ Bảo lãnh viện phí (Hospital expenses insurance)";
				productBase12.Amount = d;
				list.Add(productBase12);
				ProductBase productBase13 = new ProductBase();
				productBase13.ProdCode = "0";
				productBase13.ProdName = "+ Giảm giá (Discount)";
				productBase13.Amount = d2;
				list.Add(productBase13);
				ProductBase productBase14 = new ProductBase();
				productBase14.ProdCode = "0";
				productBase14.ProdName = "Tạm thu (Advance received)";
				if (DataInput.Transaction != null && DataInput.Transaction.KC_AMOUNT.HasValue && DataInput.Transaction.KC_AMOUNT.Value > 0m)
				{
					productBase14.Amount = DataInput.Transaction.TREATMENT_DEPOSIT_AMOUNT.GetValueOrDefault() - DataInput.Transaction.TREATMENT_REPAY_AMOUNT.GetValueOrDefault() - DataInput.Transaction.TREATMENT_TRANSFER_AMOUNT.GetValueOrDefault();
				}
				productBase14.Amount = Math.Round(productBase14.Amount, decimals, MidpointRounding.AwayFromZero);
				list.Add(productBase14);
				ProductBase productBase15 = new ProductBase();
				productBase15.ProdCode = "0";
				productBase15.ProdName = "+ Thu thêm (Additional revenue)";
				productBase15.Amount = productBase9.Amount - productBase14.Amount;
				if (productBase9.Amount - productBase14.Amount < 0m)
				{
					productBase15.ProdName = "+ Thoái trả (Change)";
					productBase15.Amount = productBase14.Amount - productBase9.Amount;
				}
				list.Add(productBase15);
			}
			return list;
		}
	}
}
