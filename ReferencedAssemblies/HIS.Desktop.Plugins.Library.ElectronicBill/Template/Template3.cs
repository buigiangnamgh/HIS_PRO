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
	public class Template3 : IRunTemplate
	{
		private ElectronicBillDataInput DataInput;

		public Template3(ElectronicBillDataInput dataInput)
		{
			DataInput = dataInput;
		}

		public object Run()
		{
			List<ProductBase> list = new List<ProductBase>();
			try
			{
				decimal d = default(decimal);
				if (DataInput != null)
				{
					if (DataInput.Amount.HasValue)
					{
						d = DataInput.Amount.Value;
					}
					else if (DataInput.SereServBill != null)
					{
						d = DataInput.SereServBill.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
					}
				}
				d = Math.Round(d, 0);
				ProductBase productBase = new ProductBase();
				productBase.ProdName = "THANH TOÁN VIỆN PHÍ";
				productBase.ProdCode = "TTVP";
				productBase.ProdPrice = d;
				productBase.ProdQuantity = 1;
				productBase.Amount = d;
				productBase.ProdUnit = "lần";
				productBase.TaxRateID = 4;
				list.Add(productBase);
				if (list.Count > 0)
				{
					foreach (ProductBase item in list)
					{
						if (HisConfigCFG.RoundTransactionAmountOption == "1")
						{
							item.Amount = Math.Round(item.Amount, 0, MidpointRounding.AwayFromZero);
							item.ProdPrice = Math.Round(item.ProdPrice.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero);
						}
						else if (HisConfigCFG.RoundTransactionAmountOption == "2")
						{
							item.Amount = Math.Round(item.Amount, 0, MidpointRounding.AwayFromZero);
						}
						if (HisConfigCFG.IsHidePrice)
						{
							item.ProdPrice = null;
						}
						if (HisConfigCFG.IsHideQuantity)
						{
							item.ProdQuantity = null;
						}
						if (HisConfigCFG.IsHideUnitName)
						{
							item.ProdUnit = "";
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
