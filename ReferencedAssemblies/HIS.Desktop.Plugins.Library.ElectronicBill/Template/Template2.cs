using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using Inventec.Common.Logging;
using Inventec.Common.Number;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Template
{
	public class Template2 : IRunTemplate
	{
		private decimal Amount;

		public Template2(decimal amount)
		{
			Amount = amount;
		}

		public object Run()
		{
			List<ProductBase> list = new List<ProductBase>();
			try
			{
				LogSystem.Debug("Tram nay do co ma RegisterNumber:" + LogUtil.TraceData(LogUtil.GetMemberName<decimal>((Expression<Func<decimal>>)(() => Amount)), (object)Amount));
				ProductBase productBase = new ProductBase();
				productBase.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(Amount);
				productBase.ProdName = "V/P";
				productBase.ProdCode = "VP";
				productBase.ProdUnit = "";
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
				LogSystem.Error(ex);
				list = null;
			}
			return list;
		}
	}
}
