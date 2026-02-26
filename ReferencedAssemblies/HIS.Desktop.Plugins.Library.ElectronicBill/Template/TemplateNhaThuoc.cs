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
	internal class TemplateNhaThuoc : IRunTemplate
	{
		private ElectronicBillDataInput DataInput;

		public TemplateNhaThuoc(ElectronicBillDataInput dataInput)
		{
			DataInput = dataInput;
		}

		public object Run()
		{
			List<ProductBasePlus> list = new List<ProductBasePlus>();
			try
			{
				if (DataInput.SereServs != null && DataInput.SereServs.Count > 0)
				{
					List<V_HIS_MEDICINE_TYPE> list2 = HisConfigCFG.V_HIS_MEDICINE_TYPEs.Where((V_HIS_MEDICINE_TYPE o) => DataInput.SereServs.Exists((V_HIS_SERE_SERV_5 e) => e.MEDICINE_ID == o.ID)).ToList();
					List<V_HIS_MATERIAL_TYPE> list3 = HisConfigCFG.V_HIS_MATERIAL_TYPEs.Where((V_HIS_MATERIAL_TYPE o) => DataInput.SereServs.Exists((V_HIS_SERE_SERV_5 e) => e.MATERIAL_ID == o.ID)).ToList();
					List<V_HIS_NONE_MEDI_SERVICE> list4 = HisConfigCFG.V_HIS_NONE_MEDI_SERVICEs.Where((V_HIS_NONE_MEDI_SERVICE o) => DataInput.SereServs.Exists((V_HIS_SERE_SERV_5 e) => e.OTHER_PAY_SOURCE_ID == o.ID)).ToList();
					foreach (V_HIS_SERE_SERV_5 item in DataInput.SereServs)
					{
						V_HIS_MEDICINE_TYPE val = ((list2 != null) ? list2.FirstOrDefault((V_HIS_MEDICINE_TYPE o) => o.ID == item.MEDICINE_ID) : null);
						V_HIS_MATERIAL_TYPE val2 = ((list3 != null) ? list3.FirstOrDefault((V_HIS_MATERIAL_TYPE o) => o.ID == item.MATERIAL_ID) : null);
						V_HIS_NONE_MEDI_SERVICE val3 = ((list4 != null) ? list4.FirstOrDefault((V_HIS_NONE_MEDI_SERVICE o) => o.ID == item.OTHER_PAY_SOURCE_ID) : null);
						string sERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
						string text = item.TDL_SERVICE_CODE;
						if (string.IsNullOrWhiteSpace(sERVICE_UNIT_NAME))
						{
							if (val != null)
							{
								sERVICE_UNIT_NAME = val.SERVICE_UNIT_NAME;
							}
							else if (val2 != null)
							{
								sERVICE_UNIT_NAME = val2.SERVICE_UNIT_NAME;
							}
							else if (val3 != null)
							{
								sERVICE_UNIT_NAME = val3.SERVICE_UNIT_NAME;
							}
						}
						if (string.IsNullOrWhiteSpace(text))
						{
							if (val != null)
							{
								text = val.MEDICINE_TYPE_CODE;
							}
							else if (val2 != null)
							{
								text = val2.MATERIAL_TYPE_CODE;
							}
							else if (val3 != null)
							{
								text = val3.NONE_MEDI_SERVICE_CODE;
							}
						}
						if (HisConfigCFG.VatOption == "2")
						{
							item.PRICE *= 1m + item.VAT_RATIO;
							item.VAT_RATIO = 0m;
						}
						ProductBasePlus productBasePlus = new ProductBasePlus();
						productBasePlus.ProdName = item.TDL_SERVICE_NAME;
						productBasePlus.ProdUnit = sERVICE_UNIT_NAME;
						productBasePlus.ProdCode = text;
						productBasePlus.ProdQuantity = item.AMOUNT;
						productBasePlus.TaxConvert = item.VAT_RATIO * 100m;
						productBasePlus.ProdPrice = Math.Round(item.PRICE, 4);
						productBasePlus.TaxAmount = Math.Round(item.PRICE * item.AMOUNT * item.VAT_RATIO, 4);
						if (HisConfigCFG.dicVatConvert != null && HisConfigCFG.dicVatConvert.Count > 0)
						{
							decimal key = Math.Round(item.VAT_RATIO * 100m, 0);
							if (HisConfigCFG.dicVatConvert.ContainsKey(key))
							{
								productBasePlus.TaxConvert = HisConfigCFG.dicVatConvert[key];
								productBasePlus.ProdPrice = Math.Round(item.PRICE * (1m + item.VAT_RATIO) / (1m + productBasePlus.TaxConvert / 100m), 4);
								productBasePlus.TaxAmount = Math.Round(item.PRICE * (1m + item.VAT_RATIO) / (1m + productBasePlus.TaxConvert / 100m) * item.AMOUNT * (productBasePlus.TaxConvert / 100m), 0);
							}
						}
						productBasePlus.AmountWithoutTax = Math.Round(productBasePlus.ProdPrice.GetValueOrDefault() * productBasePlus.ProdQuantity.GetValueOrDefault(), 4) - item.DISCOUNT.GetValueOrDefault();
						productBasePlus.Amount = productBasePlus.AmountWithoutTax.GetValueOrDefault() + productBasePlus.TaxAmount.GetValueOrDefault();
						if (HisConfigCFG.VatOption == "3")
						{
							productBasePlus.AmountWithoutTax = productBasePlus.Amount;
							productBasePlus.ProdPrice = Math.Round(productBasePlus.AmountWithoutTax.GetValueOrDefault() / productBasePlus.ProdQuantity.GetValueOrDefault(), 4);
							productBasePlus.TaxAmount = default(decimal);
							productBasePlus.TaxPercentage = null;
						}
						else if (productBasePlus.TaxConvert == 0m)
						{
							productBasePlus.TaxPercentage = 0;
						}
						else if (productBasePlus.TaxConvert == 5m)
						{
							productBasePlus.TaxPercentage = 1;
						}
						else if (productBasePlus.TaxConvert == 10m)
						{
							productBasePlus.TaxPercentage = 2;
						}
						else if (productBasePlus.TaxConvert == 8m)
						{
							productBasePlus.TaxPercentage = 3;
						}
						else if (productBasePlus.TaxConvert > 0m)
						{
							productBasePlus.TaxPercentage = 99999;
						}
						if (HisConfigCFG.RoundTransactionAmountOption == "1" || HisConfigCFG.RoundTransactionAmountOption == "2")
						{
							if (HisConfigCFG.RoundTransactionAmountOption == "1")
							{
								productBasePlus.ProdPrice = Math.Round(productBasePlus.ProdPrice.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero);
							}
							productBasePlus.Amount = Math.Round(productBasePlus.Amount, 0, MidpointRounding.AwayFromZero);
							productBasePlus.TaxAmount = Math.Round(productBasePlus.TaxAmount.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero);
							productBasePlus.AmountWithoutTax = Math.Round(productBasePlus.AmountWithoutTax.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero);
						}
						list.Add(productBasePlus);
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
