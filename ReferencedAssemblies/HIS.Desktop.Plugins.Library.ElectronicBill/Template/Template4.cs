using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using Inventec.Common.Logging;
using Inventec.Common.Number;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Template
{
	internal class Template4 : IRunTemplate
	{
		private ElectronicBillDataInput DataInput;

		public Template4(ElectronicBillDataInput dataInput)
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
					var list2 = (from o in DataInput.SereServBill
						group o by new { o.TDL_SERVICE_ID, o.TDL_REAL_PRICE }).ToList();
					List<V_HIS_SERVICE> list3 = (from o in BackendDataWorker.Get<V_HIS_SERVICE>()
						where DataInput.SereServBill.Exists((HIS_SERE_SERV_BILL e) => e.TDL_SERVICE_ID == o.ID)
						select o).ToList();
					foreach (var item in list2)
					{
						V_HIS_SERVICE val = ((list3 != null) ? list3.FirstOrDefault((V_HIS_SERVICE o) => o.ID == item.First().TDL_SERVICE_ID) : null);
						ProductBase productBase = new ProductBase();
						productBase.ProdName = item.First().TDL_SERVICE_NAME;
						productBase.ProdCode = item.First().TDL_SERVICE_CODE;
						productBase.ProdQuantity = item.Sum((HIS_SERE_SERV_BILL s) => s.TDL_AMOUNT.GetValueOrDefault());
						productBase.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(item.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
						productBase.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(productBase.Amount / (productBase.ProdQuantity ?? 1m));
						productBase.ProdUnit = ((val != null) ? val.SERVICE_UNIT_NAME : "");
						productBase.TaxRateID = (int)((val == null) ? 4 : (val.TAX_RATE_TYPE ?? 4));
						productBase.Type = ((item.First().TDL_SERVICE_TYPE_ID == 6) ? 1 : 0);
						if (productBase.ProdPrice.GetValueOrDefault() * productBase.ProdQuantity.GetValueOrDefault() != productBase.Amount)
						{
							productBase.ProdPrice = Math.Round(productBase.Amount / (productBase.ProdQuantity ?? 1m), 2);
						}
						list.Add(productBase);
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
