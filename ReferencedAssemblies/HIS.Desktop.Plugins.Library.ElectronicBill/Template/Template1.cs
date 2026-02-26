using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using Inventec.Common.Logging;
using Inventec.Common.Number;
using MOS.EFMODEL.DataModels;
using MOS.LibraryHein.Bhyt;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Template
{
	public class Template1 : IRunTemplate
	{
		public enum SERE_SERV_TYPE
		{
			BHYT_NOT_SERVICE_CONFIG,
			NOT_BHYT_NOT_SERVICE_CONFIG,
			SERVICE_CONFIG,
			NOT_SERVICE_CONFIG
		}

		private List<HIS_SERE_SERV_BILL> sereServs;

		private ElectronicBillDataInput DataInput;

		private Dictionary<SERE_SERV_TYPE, List<HIS_SERE_SERV_BILL>> dicSereServType { get; set; }

		private long treatmentId { get; set; }

		private HIS_BRANCH branch { get; set; }

		public Template1(long _treatmentId, HIS_BRANCH _branch, List<HIS_SERE_SERV_BILL> list, ElectronicBillDataInput dataInput)
		{
			treatmentId = _treatmentId;
			branch = _branch;
			sereServs = list;
			DataInput = dataInput;
		}

		object IRunTemplate.Run()
		{
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			List<ProductBase> list = new List<ProductBase>();
			try
			{
				if (sereServs != null && sereServs.Count > 0)
				{
					if (DataInput.LastPatientTypeAlter == null)
					{
						LogSystem.Debug("Khong tim thay doi tuong benh nhan hien tai!");
						return null;
					}
					List<V_HIS_SERVICE> list2 = (from o in BackendDataWorker.Get<V_HIS_SERVICE>()
						where sereServs.Exists((HIS_SERE_SERV_BILL e) => e.TDL_SERVICE_ID == o.ID)
						select o).ToList();
					if (DataInput.LastPatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
					{
						dicSereServType = new Dictionary<SERE_SERV_TYPE, List<HIS_SERE_SERV_BILL>>();
						dicSereServType[SERE_SERV_TYPE.BHYT_NOT_SERVICE_CONFIG] = new List<HIS_SERE_SERV_BILL>();
						dicSereServType[SERE_SERV_TYPE.NOT_BHYT_NOT_SERVICE_CONFIG] = new List<HIS_SERE_SERV_BILL>();
						dicSereServType[SERE_SERV_TYPE.SERVICE_CONFIG] = new List<HIS_SERE_SERV_BILL>();
						dicSereServType[SERE_SERV_TYPE.NOT_SERVICE_CONFIG] = new List<HIS_SERE_SERV_BILL>();
						dicSereServType[SERE_SERV_TYPE.SERVICE_CONFIG] = GetListSereServByConfig(sereServs);
						dicSereServType[SERE_SERV_TYPE.NOT_SERVICE_CONFIG] = sereServs.Where((HIS_SERE_SERV_BILL o) => dicSereServType[SERE_SERV_TYPE.SERVICE_CONFIG] == null || !dicSereServType[SERE_SERV_TYPE.SERVICE_CONFIG].Contains(o)).ToList();
						dicSereServType[SERE_SERV_TYPE.BHYT_NOT_SERVICE_CONFIG] = dicSereServType[SERE_SERV_TYPE.NOT_SERVICE_CONFIG].Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
						dicSereServType[SERE_SERV_TYPE.NOT_BHYT_NOT_SERVICE_CONFIG] = dicSereServType[SERE_SERV_TYPE.NOT_SERVICE_CONFIG].Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
						if (dicSereServType[SERE_SERV_TYPE.NOT_BHYT_NOT_SERVICE_CONFIG].Count + dicSereServType[SERE_SERV_TYPE.BHYT_NOT_SERVICE_CONFIG].Count + dicSereServType[SERE_SERV_TYPE.SERVICE_CONFIG].Count != sereServs.Count)
						{
							throw new Exception("Loi : Kiem tra toan ven du lieu");
						}
						if (dicSereServType[SERE_SERV_TYPE.BHYT_NOT_SERVICE_CONFIG] != null && dicSereServType[SERE_SERV_TYPE.BHYT_NOT_SERVICE_CONFIG].Count > 0)
						{
							ProductBase productBase = new ProductBase();
							productBase.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(dicSereServType[SERE_SERV_TYPE.BHYT_NOT_SERVICE_CONFIG].Sum((HIS_SERE_SERV_BILL o) => o.PRICE));
							decimal num = new BhytHeinProcessor().GetDefaultHeinRatio(DataInput.LastPatientTypeAlter.HEIN_TREATMENT_TYPE_CODE, DataInput.LastPatientTypeAlter.HEIN_CARD_NUMBER, branch.HEIN_LEVEL_CODE, DataInput.LastPatientTypeAlter.RIGHT_ROUTE_CODE).GetValueOrDefault() * 100m;
							string text = "";
							text = ((DataInput.LastPatientTypeAlter.TREATMENT_TYPE_ID == 2) ? ((!(num != 100m)) ? "Thu viện phí - Viện phí bệnh án ngoại trú" : string.Format("{0:0.####}% bệnh nhân đồng chi trả - Viện phí bệnh án ngoại trú", 100m - num)) : ((DataInput.LastPatientTypeAlter.TREATMENT_TYPE_ID == 3) ? ((!(num != 100m)) ? "Thu viện phí - Viện phí bệnh án nội trú" : string.Format("{0:0.####}% bệnh nhân đồng chi trả - Viện phí bệnh án nội trú", 100m - num)) : ((DataInput.LastPatientTypeAlter.TREATMENT_TYPE_ID == 1) ? ((!(num != 100m)) ? "Thu viện phí - khám bệnh ngoại trú" : string.Format("{0:0.####}% bệnh nhân đồng chi trả - Khám bệnh ngoại trú", 100m - num)) : ((!(num != 100m)) ? "Thu viện phí - khám bệnh" : string.Format("{0:0.####}% bệnh nhân đồng chi trả - Khám bệnh", 100m - num)))));
							productBase.ProdUnit = "";
							productBase.ProdName = text;
							productBase.ProdCode = General.GetFirstWord(productBase.ProdName);
							productBase.Type = ((dicSereServType[SERE_SERV_TYPE.BHYT_NOT_SERVICE_CONFIG].Count() == dicSereServType[SERE_SERV_TYPE.BHYT_NOT_SERVICE_CONFIG].Count((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 6)) ? 1 : 0);
							list.Add(productBase);
						}
						if (dicSereServType[SERE_SERV_TYPE.NOT_BHYT_NOT_SERVICE_CONFIG] != null && dicSereServType[SERE_SERV_TYPE.NOT_BHYT_NOT_SERVICE_CONFIG].Count > 0)
						{
							IEnumerable<IGrouping<long?, HIS_SERE_SERV_BILL>> enumerable = from o in dicSereServType[SERE_SERV_TYPE.NOT_BHYT_NOT_SERVICE_CONFIG]
								group o by o.TDL_SERVICE_TYPE_ID;
							List<HIS_SERVICE_TYPE> source = BackendDataWorker.Get<HIS_SERVICE_TYPE>();
							foreach (IGrouping<long?, HIS_SERE_SERV_BILL> item in enumerable)
							{
								HIS_SERVICE_TYPE val = source.FirstOrDefault((HIS_SERVICE_TYPE o) => o.ID == item.First().TDL_SERVICE_TYPE_ID);
								ProductBase productBase2 = new ProductBase();
								productBase2.ProdName = ((val != null) ? val.SERVICE_TYPE_NAME : "Khác");
								productBase2.ProdCode = General.GetFirstWord(productBase2.ProdName);
								productBase2.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(item.Sum((HIS_SERE_SERV_BILL o) => o.PRICE));
								productBase2.ProdUnit = "";
								productBase2.Type = ((item.Count() == item.Count((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 6)) ? 1 : 0);
								list.Add(productBase2);
							}
						}
						if (dicSereServType[SERE_SERV_TYPE.SERVICE_CONFIG] != null && dicSereServType[SERE_SERV_TYPE.SERVICE_CONFIG].Count > 0)
						{
							foreach (HIS_SERE_SERV_BILL item2 in dicSereServType[SERE_SERV_TYPE.SERVICE_CONFIG])
							{
								V_HIS_SERVICE val2 = ((list2 != null) ? list2.FirstOrDefault((V_HIS_SERVICE o) => o.ID == item2.TDL_SERVICE_ID) : null);
								ProductBase productBase3 = new ProductBase();
								productBase3.ProdName = item2.TDL_SERVICE_NAME;
								productBase3.ProdCode = item2.TDL_SERVICE_CODE;
								productBase3.ProdQuantity = item2.TDL_AMOUNT.GetValueOrDefault();
								productBase3.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(item2.PRICE);
								productBase3.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(productBase3.Amount / (productBase3.ProdQuantity ?? 1m));
								productBase3.Type = ((item2.TDL_SERVICE_TYPE_ID == 6) ? 1 : 0);
								productBase3.ProdUnit = ((val2 != null) ? val2.SERVICE_UNIT_NAME : "");
								productBase3.TaxRateID = (int)((val2 != null) ? (val2.TAX_RATE_TYPE ?? 4) : 4);
								list.Add(productBase3);
							}
						}
					}
					else if (DataInput.LastPatientTypeAlter.TREATMENT_TYPE_ID == 1)
					{
						var list3 = (from o in sereServs
							group o by new { o.TDL_SERVICE_ID, o.TDL_REAL_PRICE }).ToList();
						foreach (var item3 in list3)
						{
							V_HIS_SERVICE val3 = ((list2 != null) ? list2.FirstOrDefault((V_HIS_SERVICE o) => o.ID == item3.First().TDL_SERVICE_ID) : null);
							ProductBase productBase4 = new ProductBase();
							productBase4.ProdName = item3.First().TDL_SERVICE_NAME;
							productBase4.ProdCode = item3.First().TDL_SERVICE_CODE;
							productBase4.ProdQuantity = item3.Sum((HIS_SERE_SERV_BILL s) => s.TDL_AMOUNT.GetValueOrDefault());
							productBase4.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(item3.Sum((HIS_SERE_SERV_BILL s) => s.PRICE));
							productBase4.ProdUnit = ((val3 != null) ? val3.SERVICE_UNIT_NAME : "");
							productBase4.ProdPrice = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(productBase4.Amount / (productBase4.ProdQuantity ?? 1m));
							productBase4.TaxRateID = (int)((val3 != null) ? (val3.TAX_RATE_TYPE ?? 4) : 4);
							productBase4.Type = ((item3.First().TDL_SERVICE_TYPE_ID == 6) ? 1 : 0);
							list.Add(productBase4);
						}
					}
					else
					{
						ProductBase productBase5 = new ProductBase();
						productBase5.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServs.Sum((HIS_SERE_SERV_BILL o) => o.PRICE));
						productBase5.ProdName = "Thu viện phí nhân dân";
						productBase5.ProdCode = General.GetFirstWord(productBase5.ProdName);
						productBase5.ProdUnit = "";
						list.Add(productBase5);
					}
				}
				if (list.Count > 0)
				{
					foreach (ProductBase item4 in list)
					{
						if (HisConfigCFG.RoundTransactionAmountOption == "1")
						{
							item4.Amount = Math.Round(item4.Amount, 0, MidpointRounding.AwayFromZero);
							item4.ProdPrice = Math.Round(item4.ProdPrice.GetValueOrDefault(), 0, MidpointRounding.AwayFromZero);
						}
						else if (HisConfigCFG.RoundTransactionAmountOption == "2")
						{
							item4.Amount = Math.Round(item4.Amount, 0, MidpointRounding.AwayFromZero);
						}
						if (HisConfigCFG.IsHidePrice)
						{
							item4.ProdPrice = null;
						}
						if (HisConfigCFG.IsHideQuantity)
						{
							item4.ProdQuantity = null;
						}
						if (HisConfigCFG.IsHideUnitName)
						{
							item4.ProdUnit = "";
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return list;
		}

		private List<HIS_SERE_SERV_BILL> GetListSereServByConfig(List<HIS_SERE_SERV_BILL> sereServs)
		{
			List<HIS_SERE_SERV_BILL> result = new List<HIS_SERE_SERV_BILL>();
			try
			{
				string text = HisConfigs.Get<string>("HIS.DESKTOP.LIBRARY.ELECTRONIC_BILL.SERVICE_INDEPENDENT_DISPLAY");
				if (string.IsNullOrEmpty(text))
				{
					return new List<HIS_SERE_SERV_BILL>();
				}
				string[] serviceIndepentStr = text.Split('|');
				if (serviceIndepentStr != null && serviceIndepentStr.Length != 0)
				{
					result = sereServs.Where((HIS_SERE_SERV_BILL o) => serviceIndepentStr.Contains(o.TDL_SERVICE_CODE)).ToList();
				}
			}
			catch (Exception ex)
			{
				result = new List<HIS_SERE_SERV_BILL>();
				LogSystem.Warn(ex);
			}
			return result;
		}
	}
}
