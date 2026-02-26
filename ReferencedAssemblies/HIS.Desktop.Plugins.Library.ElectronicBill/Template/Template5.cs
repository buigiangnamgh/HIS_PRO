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
using MOS.LibraryHein.Bhyt;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Template
{
	internal class Template5 : IRunTemplate
	{
		private ElectronicBillDataInput DataInput;

		public Template5(ElectronicBillDataInput dataInput)
		{
			DataInput = dataInput;
		}

		public object Run()
		{
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			List<ProductBase> list = new List<ProductBase>();
			try
			{
				if (DataInput.SereServBill != null && DataInput.SereServBill.Count > 0)
				{
					List<V_HIS_SERVICE> list2 = (from o in BackendDataWorker.Get<V_HIS_SERVICE>()
						where DataInput.SereServBill.Exists((HIS_SERE_SERV_BILL e) => e.TDL_SERVICE_ID == o.ID)
						select o).ToList();
					if (DataInput.Treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
					{
						List<HIS_SERE_SERV_BILL> list3 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
						List<HIS_SERE_SERV_BILL> list4 = DataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
						if (list3 != null && list3.Count > 0)
						{
							if (DataInput.LastPatientTypeAlter == null)
							{
								LogSystem.Debug("Khong tim thay doi tuong benh nhan hien tai!");
								return null;
							}
							ProductBase productBase = new ProductBase();
							productBase.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(list3.Sum((HIS_SERE_SERV_BILL o) => o.PRICE));
							decimal num = new BhytHeinProcessor().GetDefaultHeinRatio(DataInput.LastPatientTypeAlter.HEIN_TREATMENT_TYPE_CODE, DataInput.LastPatientTypeAlter.HEIN_CARD_NUMBER, DataInput.Branch.HEIN_LEVEL_CODE, DataInput.LastPatientTypeAlter.RIGHT_ROUTE_CODE).GetValueOrDefault() * 100m;
							string text = "";
							text = ((DataInput.LastPatientTypeAlter.TREATMENT_TYPE_ID == 2) ? ((!(num != 100m)) ? "Thu viện phí - Viện phí bệnh án ngoại trú" : string.Format("{0:0.####}% bệnh nhân đồng chi trả - Viện phí bệnh án ngoại trú", 100m - num)) : ((DataInput.LastPatientTypeAlter.TREATMENT_TYPE_ID == 3) ? ((!(num != 100m)) ? "Thu viện phí - Viện phí bệnh án nội trú" : string.Format("{0:0.####}% bệnh nhân đồng chi trả - Viện phí bệnh án nội trú", 100m - num)) : ((DataInput.LastPatientTypeAlter.TREATMENT_TYPE_ID == 1) ? ((!(num != 100m)) ? "Thu viện phí - khám bệnh ngoại trú" : string.Format("{0:0.####}% bệnh nhân đồng chi trả - Khám bệnh ngoại trú", 100m - num)) : ((!(num != 100m)) ? "Thu viện phí - khám bệnh" : string.Format("{0:0.####}% bệnh nhân đồng chi trả - Khám bệnh", 100m - num)))));
							productBase.ProdUnit = "";
							productBase.ProdName = text;
							productBase.TaxRateID = 4;
							productBase.ProdCode = General.GetFirstWord(productBase.ProdName);
							productBase.Type = ((list3.Count() == list3.Count((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 6)) ? 1 : 0);
							list.Add(productBase);
						}
						if (list4 != null && list4.Count > 0)
						{
							var list5 = (from o in list4
								group o by new { o.TDL_SERVICE_ID, o.TDL_REAL_PRICE }).ToList();
							foreach (var item in list5)
							{
								V_HIS_SERVICE val = ((list2 != null) ? list2.FirstOrDefault((V_HIS_SERVICE o) => o.ID == item.First().TDL_SERVICE_ID) : null);
								ProductBase productBase2 = new ProductBase();
								productBase2.ProdName = item.First().TDL_SERVICE_NAME;
								productBase2.ProdQuantity = item.Sum((HIS_SERE_SERV_BILL s) => s.TDL_AMOUNT.GetValueOrDefault());
								productBase2.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(item.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
								productBase2.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(productBase2.Amount / (productBase2.ProdQuantity ?? 1m));
								productBase2.ProdUnit = ((val != null) ? val.SERVICE_UNIT_NAME : "");
								productBase2.TaxRateID = (int)((val != null) ? (val.TAX_RATE_TYPE ?? 4) : 4);
								productBase2.ProdCode = item.First().TDL_SERVICE_CODE;
								productBase2.Type = ((item.First().TDL_SERVICE_TYPE_ID == 6) ? 1 : 0);
								list.Add(productBase2);
							}
						}
					}
					else
					{
						var list6 = (from o in DataInput.SereServBill
							group o by new { o.TDL_SERVICE_ID, o.TDL_REAL_PRICE }).ToList();
						foreach (var item2 in list6)
						{
							V_HIS_SERVICE val2 = ((list2 != null) ? list2.FirstOrDefault((V_HIS_SERVICE o) => o.ID == item2.First().TDL_SERVICE_ID) : null);
							ProductBase productBase3 = new ProductBase();
							productBase3.ProdName = item2.First().TDL_SERVICE_NAME;
							productBase3.ProdQuantity = item2.Sum((HIS_SERE_SERV_BILL s) => s.TDL_AMOUNT.GetValueOrDefault());
							productBase3.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(item2.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
							productBase3.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(productBase3.Amount / (productBase3.ProdQuantity ?? 1m));
							productBase3.ProdUnit = ((val2 != null) ? val2.SERVICE_UNIT_NAME : "");
							productBase3.TaxRateID = (int)((val2 != null) ? (val2.TAX_RATE_TYPE ?? 4) : 4);
							productBase3.ProdCode = item2.First().TDL_SERVICE_CODE;
							productBase3.Type = ((item2.First().TDL_SERVICE_TYPE_ID == 6) ? 1 : 0);
							list.Add(productBase3);
						}
					}
				}
				if (list.Count > 0)
				{
					foreach (ProductBase item3 in list)
					{
						if (HisConfigCFG.RoundTransactionAmountOption == "1")
						{
							item3.Amount = Math.Round(item3.Amount, 0, MidpointRounding.AwayFromZero);
							item3.ProdPrice = Math.Round(item3.ProdPrice.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero);
						}
						else if (HisConfigCFG.RoundTransactionAmountOption == "2")
						{
							item3.Amount = Math.Round(item3.Amount, 0, MidpointRounding.AwayFromZero);
						}
						if (HisConfigCFG.IsHidePrice)
						{
							item3.ProdPrice = null;
						}
						if (HisConfigCFG.IsHideQuantity)
						{
							item3.ProdQuantity = null;
						}
						if (HisConfigCFG.IsHideUnitName)
						{
							item3.ProdUnit = "";
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
