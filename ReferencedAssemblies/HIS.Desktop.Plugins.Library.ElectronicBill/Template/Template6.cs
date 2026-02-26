using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using Inventec.Common.Logging;
using Inventec.Common.Number;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Template
{
	internal class Template6 : IRunTemplate
	{
		private ElectronicBillDataInput DataInput;

		public Template6(ElectronicBillDataInput dataInput)
		{
			DataInput = dataInput;
		}

		public object Run()
		{
			List<ProductBase> list = new List<ProductBase>();
			try
			{
				if (DataInput.SereServBill != null && DataInput.SereServBill.Count > 0)
				{
					List<HIS_SERE_SERV_BILL> sereServExam = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 1).ToList();
					List<HIS_SERE_SERV_BILL> sereServSubclinical = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 2 || o.TDL_SERVICE_TYPE_ID == 3 || o.TDL_SERVICE_TYPE_ID == 5 || o.TDL_SERVICE_TYPE_ID == 9 || o.TDL_SERVICE_TYPE_ID == 10 || o.TDL_SERVICE_TYPE_ID == 15).ToList();
					List<HIS_SERE_SERV_BILL> sereServPttt = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 11 || o.TDL_SERVICE_TYPE_ID == 4 || o.TDL_SERVICE_TYPE_ID == 13).ToList();
					List<HIS_SERE_SERV_BILL> sereServBed = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 8).ToList();
					List<HIS_SERE_SERV_BILL> sereServMediMate = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 6 || o.TDL_SERVICE_TYPE_ID == 7 || o.TDL_SERVICE_TYPE_ID == 14).ToList();
					List<HIS_SERE_SERV_BILL> list2 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => (sereServExam == null || !sereServExam.Contains(o)) && (sereServSubclinical == null || !sereServSubclinical.Contains(o)) && (sereServPttt == null || !sereServPttt.Contains(o)) && (sereServBed == null || !sereServBed.Contains(o)) && (sereServMediMate == null || !sereServMediMate.Contains(o))).ToList();
					if (sereServExam != null && sereServExam.Count > 0)
					{
						ProductBase productBase = new ProductBase();
						productBase.ProdName = "Khám bệnh";
						productBase.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServExam.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase.ProdQuantity = 1;
						productBase.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServExam.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase.ProdUnit = " ";
						productBase.TaxRateID = 4;
						productBase.ProdCode = "KB";
						list.Add(productBase);
					}
					if (sereServSubclinical != null && sereServSubclinical.Count > 0)
					{
						ProductBase productBase2 = new ProductBase();
						productBase2.ProdName = "Cận lâm sàng";
						productBase2.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServSubclinical.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase2.ProdQuantity = 1;
						productBase2.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServSubclinical.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase2.ProdUnit = " ";
						productBase2.TaxRateID = 4;
						productBase2.ProdCode = "CLS";
						list.Add(productBase2);
					}
					if (sereServPttt != null && sereServPttt.Count > 0)
					{
						ProductBase productBase3 = new ProductBase();
						productBase3.ProdName = "Phẫu thuật, thủ thuật";
						productBase3.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServPttt.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase3.ProdQuantity = 1;
						productBase3.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServPttt.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase3.ProdUnit = " ";
						productBase3.TaxRateID = 4;
						productBase3.ProdCode = "PTTT";
						list.Add(productBase3);
					}
					if (sereServBed != null && sereServBed.Count > 0)
					{
						ProductBase productBase4 = new ProductBase();
						productBase4.ProdName = "Giường bệnh";
						productBase4.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServBed.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase4.ProdQuantity = 1;
						productBase4.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServBed.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase4.ProdUnit = " ";
						productBase4.TaxRateID = 4;
						productBase4.ProdCode = "GB";
						list.Add(productBase4);
					}
					if (sereServMediMate != null && sereServMediMate.Count > 0)
					{
						ProductBase productBase5 = new ProductBase();
						productBase5.ProdName = "Thuốc, VTYT";
						productBase5.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServMediMate.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase5.ProdQuantity = 1;
						productBase5.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServMediMate.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase5.ProdUnit = " ";
						productBase5.TaxRateID = 4;
						productBase5.ProdCode = "TH";
						productBase5.Type = 1;
						list.Add(productBase5);
					}
					if (list2 != null && list2.Count > 0)
					{
						if ((DataInput.Transaction != null && DataInput.Transaction.SALE_TYPE_ID == 2) || (DataInput.ListTransaction != null && DataInput.ListTransaction.Count > 0 && DataInput.ListTransaction.Exists((V_HIS_TRANSACTION o) => o.SALE_TYPE_ID == 2)))
						{
							List<HIS_SERE_SERV_BILL> list3 = list2.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 0).ToList();
							foreach (HIS_SERE_SERV_BILL item in list3)
							{
								ProductBase productBase6 = new ProductBase();
								productBase6.ProdName = item.TDL_SERVICE_NAME;
								productBase6.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(item.PRICE);
								productBase6.ProdQuantity = 1;
								productBase6.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(item.PRICE);
								productBase6.ProdUnit = " ";
								productBase6.TaxRateID = 4;
								productBase6.ProdCode = item.TDL_SERVICE_CODE;
								productBase6.Type = ((item.TDL_SERVICE_TYPE_ID == 6) ? 1 : 0);
								list.Add(productBase6);
							}
							List<HIS_SERE_SERV_BILL> list4 = list2.Where((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID > 0).ToList();
							if (list4 != null && list4.Count > 0)
							{
								ProductBase productBase7 = new ProductBase();
								productBase7.ProdName = "Dịch vụ khác";
								productBase7.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(list4.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
								productBase7.ProdQuantity = 1;
								productBase7.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(list4.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
								productBase7.ProdUnit = " ";
								productBase7.TaxRateID = 4;
								productBase7.ProdCode = "DVKH";
								productBase7.Type = ((list4.Count() == list4.Count((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 6)) ? 1 : 0);
								list.Add(productBase7);
							}
						}
						else
						{
							ProductBase productBase8 = new ProductBase();
							productBase8.ProdName = "Dịch vụ khác";
							productBase8.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(list2.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
							productBase8.ProdQuantity = 1;
							productBase8.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(list2.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
							productBase8.ProdUnit = " ";
							productBase8.TaxRateID = 4;
							productBase8.ProdCode = "DVKH";
							productBase8.Type = ((list2.Count() == list2.Count((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 6)) ? 1 : 0);
							list.Add(productBase8);
						}
					}
				}
				if (list.Count > 0)
				{
					foreach (ProductBase item2 in list)
					{
						if (HisConfigCFG.RoundTransactionAmountOption == "1")
						{
							item2.Amount = Math.Round(item2.Amount, 0, MidpointRounding.AwayFromZero);
							item2.ProdPrice = Math.Round(item2.ProdPrice.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero);
						}
						else if (HisConfigCFG.RoundTransactionAmountOption == "2")
						{
							item2.Amount = Math.Round(item2.Amount, 0, MidpointRounding.AwayFromZero);
						}
						if (HisConfigCFG.IsHidePrice)
						{
							item2.ProdPrice = null;
						}
						if (HisConfigCFG.IsHideQuantity)
						{
							item2.ProdQuantity = null;
						}
						if (HisConfigCFG.IsHideUnitName)
						{
							item2.ProdUnit = "";
						}
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
	}
}
