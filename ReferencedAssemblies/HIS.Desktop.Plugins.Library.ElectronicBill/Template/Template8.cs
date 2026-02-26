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
using Newtonsoft.Json;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Template
{
	internal class Template8 : IRunTemplate
	{
		private ElectronicBillDataInput DataInput;

		public Template8(ElectronicBillDataInput dataInput)
		{
			DataInput = dataInput;
		}

		public object Run()
		{
			//IL_0e87: Unknown result type (might be due to invalid IL or missing references)
			List<ProductBase> list = new List<ProductBase>();
			if (DataInput.SereServBill != null && DataInput.SereServBill.Count > 0)
			{
				List<HIS_SERE_SERV_BILL> list2 = new List<HIS_SERE_SERV_BILL>();
				list2.AddRange(DataInput.SereServBill);
				bool flag = false;
				bool flag2 = false;
				if (DataInput.Transaction != null && DataInput.Transaction.BILL_TYPE_ID == 2)
				{
					flag2 = true;
				}
				string text = HisConfigs.Get<string>("HIS.Desktop.Plugins.Library.ElectronicBill.Template.Detail");
				List<TemplateDetailADO> list3 = JsonConvert.DeserializeObject<List<TemplateDetailADO>>(text);
				if (list3 == null || list3.Count <= 0)
				{
					throw new Exception("Sai cấu hình chi tiết hóa đơn");
				}
				list3 = list3.OrderBy((TemplateDetailADO o) => o.NumOrder ?? 9999).ToList();
				foreach (TemplateDetailADO item2 in list3)
				{
					if (!string.IsNullOrWhiteSpace(item2.HeinServiceTypeCodes))
					{
						List<string> heinServiceTypeCodes = item2.HeinServiceTypeCodes.Split('|').ToList();
						item2.HeinServiceTypeIds = (from s in BackendDataWorker.Get<HIS_HEIN_SERVICE_TYPE>()
							where heinServiceTypeCodes.Contains(s.HEIN_SERVICE_TYPE_CODE)
							select s.ID).ToList();
					}
					if (!string.IsNullOrWhiteSpace(item2.ServiceTypeCodes))
					{
						List<string> serviceTypeCodes = item2.ServiceTypeCodes.Split('|').ToList();
						item2.ServiceTypeIds = (from s in BackendDataWorker.Get<HIS_SERVICE_TYPE>()
							where serviceTypeCodes.Contains(s.SERVICE_TYPE_CODE)
							select s.ID).ToList();
					}
					if (!string.IsNullOrWhiteSpace(item2.ParentServiceCodes))
					{
						List<string> parentServiceCodes = item2.ParentServiceCodes.Split('|').ToList();
						item2.ParentServiceIds = (from s in BackendDataWorker.Get<V_HIS_SERVICE>()
							where parentServiceCodes.Contains(s.SERVICE_CODE)
							select s.ID).ToList();
					}
					if (!string.IsNullOrWhiteSpace(item2.TreatmentTypeCodes))
					{
						List<string> treatmentTypeCodes = item2.TreatmentTypeCodes.Split('|').ToList();
						item2.TreatmentTypeIds = (from s in BackendDataWorker.Get<HIS_TREATMENT_TYPE>()
							where treatmentTypeCodes.Contains(s.TREATMENT_TYPE_CODE)
							select s.ID).ToList();
					}
					if (!string.IsNullOrWhiteSpace(item2.PatientTypeCodes))
					{
						List<string> patientTypeCodes = item2.PatientTypeCodes.Split('|').ToList();
						item2.PatientTypeIds = (from s in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
							where patientTypeCodes.Contains(s.PATIENT_TYPE_CODE)
							select s.ID).ToList();
					}
				}
				Dictionary<string, string> dictionary = General.ProcessDicValueString(DataInput);
				foreach (TemplateDetailADO detail in list3)
				{
					if (list2.Count <= 0)
					{
						break;
					}
					if ((string.IsNullOrWhiteSpace(detail.Display) && string.IsNullOrWhiteSpace(detail.ServiceTypeCodes) && string.IsNullOrWhiteSpace(detail.ServiceCodes) && string.IsNullOrWhiteSpace(detail.HeinServiceTypeCodes) && string.IsNullOrWhiteSpace(detail.TreatmentTypeCodes) && string.IsNullOrWhiteSpace(detail.ParentServiceCodes) && string.IsNullOrWhiteSpace(detail.PatientTypeCodes) && !detail.IsSplit.HasValue && !detail.IsBHYT.HasValue) || (detail.IsBHYT == 1 && detail.IsSplitBhytPrice == 1 && flag2) || (!string.IsNullOrWhiteSpace(detail.TreatmentTypeCodes) && detail.TreatmentTypeIds != null && detail.TreatmentTypeIds.Count > 0 && (DataInput.Treatment == null || !detail.TreatmentTypeIds.Contains(DataInput.Treatment.TDL_TREATMENT_TYPE_ID.GetValueOrDefault()))))
					{
						continue;
					}
					if (!string.IsNullOrWhiteSpace(detail.Display))
					{
						foreach (KeyValuePair<string, string> item3 in dictionary)
						{
							detail.Display = detail.Display.Replace(string.Format("<#{0};>", item3.Key), item3.Value);
						}
					}
					List<HIS_SERE_SERV_BILL> sereServs = new List<HIS_SERE_SERV_BILL>();
					sereServs.AddRange(list2);
					if (detail.IsBHYT == 1)
					{
						sereServs = sereServs.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
						if (detail.IsSplitBhytPrice == 1 && detail.IsSplit != 1)
						{
							flag = true;
							sereServs = sereServs.Where((HIS_SERE_SERV_BILL o) => o.PRICE - o.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault() >= 0m).ToList();
						}
					}
					else
					{
						if (!string.IsNullOrWhiteSpace(detail.PatientTypeCodes))
						{
							sereServs = ((detail.PatientTypeIds != null) ? sereServs.Where((HIS_SERE_SERV_BILL o) => detail.PatientTypeIds.Contains(o.TDL_PATIENT_TYPE_ID.GetValueOrDefault()) || detail.PatientTypeIds.Contains(o.TDL_PRIMARY_PATIENT_TYPE_ID.GetValueOrDefault())).ToList() : new List<HIS_SERE_SERV_BILL>());
						}
						if (!string.IsNullOrWhiteSpace(detail.HeinServiceTypeCodes))
						{
							sereServs = ((detail.HeinServiceTypeIds != null) ? sereServs.Where((HIS_SERE_SERV_BILL o) => detail.HeinServiceTypeIds.Contains(o.TDL_HEIN_SERVICE_TYPE_ID.GetValueOrDefault())).ToList() : new List<HIS_SERE_SERV_BILL>());
						}
						if (!string.IsNullOrWhiteSpace(detail.ServiceTypeCodes))
						{
							sereServs = ((detail.ServiceTypeIds != null) ? sereServs.Where((HIS_SERE_SERV_BILL o) => detail.ServiceTypeIds.Contains(o.TDL_SERVICE_TYPE_ID.GetValueOrDefault())).ToList() : new List<HIS_SERE_SERV_BILL>());
						}
						if (!string.IsNullOrWhiteSpace(detail.ParentServiceCodes))
						{
							List<long> ServiceIds = ((detail.ParentServiceIds != null) ? (from s in BackendDataWorker.Get<V_HIS_SERVICE>()
								where detail.ParentServiceIds.Contains(s.PARENT_ID.GetValueOrDefault())
								select s.ID).ToList() : new List<long>());
							sereServs = sereServs.Where((HIS_SERE_SERV_BILL o) => ServiceIds.Contains(o.TDL_SERVICE_ID.GetValueOrDefault())).ToList();
						}
						if (!string.IsNullOrWhiteSpace(detail.ServiceCodes))
						{
							List<string> serviceCodes = detail.ServiceCodes.Split('|').ToList();
							sereServs = sereServs.Where((HIS_SERE_SERV_BILL o) => serviceCodes.Contains(o.TDL_SERVICE_CODE)).ToList();
						}
					}
					int num = 0;
					if (sereServs == null || sereServs.Count <= 0)
					{
						continue;
					}
					if (detail.IsSplit == 1)
					{
						var list4 = (from o in sereServs
							group o by new { o.TDL_SERVICE_ID, o.TDL_REAL_PRICE }).ToList();
						List<V_HIS_SERVICE> list5 = (from o in BackendDataWorker.Get<V_HIS_SERVICE>()
							where sereServs.Exists((HIS_SERE_SERV_BILL e) => e.TDL_SERVICE_ID == o.ID)
							select o).ToList();
						foreach (var item in list4)
						{
							decimal num2 = default(decimal);
							if (flag)
							{
								num2 += item.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
								num2 += item.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum((HIS_SERE_SERV_BILL s) => s.PRICE - s.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault());
							}
							else
							{
								num2 += item.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
							}
							if (!(num2 == 0m))
							{
								V_HIS_SERVICE val = ((list5 != null) ? list5.FirstOrDefault((V_HIS_SERVICE o) => o.ID == item.First().TDL_SERVICE_ID) : null);
								ProductBase productBase = new ProductBase();
								productBase.ProdName = item.First().TDL_SERVICE_NAME;
								productBase.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(num2);
								productBase.ProdUnit = ((DataInput.SereServs != null) ? DataInput.SereServs[num].SERVICE_UNIT_NAME : "");
								num++;
								productBase.TaxRateID = (int)((val == null) ? 4 : (val.TAX_RATE_TYPE ?? 4));
								productBase.ProdCode = item.First().TDL_SERVICE_CODE;
								productBase.Type = ((item.First().TDL_SERVICE_TYPE_ID == 6) ? 1 : 0);
								productBase.ProdQuantity = item.Sum((HIS_SERE_SERV_BILL o) => o.TDL_AMOUNT);
								productBase.ProdPrice = Math.Round(productBase.Amount / (productBase.ProdQuantity ?? 1m), 0);
								if (!string.IsNullOrWhiteSpace(detail.Unit))
								{
									productBase.ProdUnit = detail.Unit;
									productBase.ProdQuantity = 1;
									productBase.ProdPrice = productBase.Amount;
								}
								productBase.Stt = detail.Stt.GetValueOrDefault();
								list.Add(productBase);
							}
						}
						LogSystem.Debug("_______RESULT:" + LogUtil.TraceData("result", (object)list));
					}
					else if (detail.IsBHYT == 1)
					{
						if (DataInput.LastPatientTypeAlter == null)
						{
							LogSystem.Debug("Khong tim thay doi tuong benh nhan hien tai!");
							continue;
						}
						ProductBase productBase2 = new ProductBase();
						if (flag)
						{
							productBase2.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServs.Sum((HIS_SERE_SERV_BILL o) => o.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault()));
						}
						else
						{
							productBase2.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(sereServs.Sum((HIS_SERE_SERV_BILL o) => o.PRICE));
						}
						if (productBase2.Amount == 0m)
						{
							continue;
						}
						decimal num3 = new BhytHeinProcessor().GetDefaultHeinRatio(DataInput.LastPatientTypeAlter.HEIN_TREATMENT_TYPE_CODE, DataInput.LastPatientTypeAlter.HEIN_CARD_NUMBER, DataInput.LastPatientTypeAlter.LEVEL_CODE, DataInput.LastPatientTypeAlter.RIGHT_ROUTE_CODE).GetValueOrDefault() * 100m;
						string text2 = "";
						text2 = ((!(Math.Round(100m - num3, 0, MidpointRounding.AwayFromZero) == 0m)) ? detail.Display.Split('|').First() : detail.Display.Split('|').Last());
						productBase2.ProdName = string.Format(text2, Math.Round(100m - num3, 0, MidpointRounding.AwayFromZero));
						productBase2.ProdCode = General.GetFirstWord(productBase2.ProdName);
						productBase2.ProdUnit = detail.Unit;
						productBase2.Stt = detail.Stt.GetValueOrDefault();
						productBase2.IsBHYT = true;
						list.Add(productBase2);
					}
					else
					{
						decimal num4 = default(decimal);
						if (flag)
						{
							num4 += sereServs.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
							num4 += sereServs.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum((HIS_SERE_SERV_BILL s) => s.PRICE - s.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault());
						}
						else
						{
							num4 += sereServs.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
						}
						ProductBase productBase3 = new ProductBase();
						productBase3.ProdName = detail.Display;
						productBase3.ProdUnit = detail.Unit;
						productBase3.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(num4);
						productBase3.ProdCode = General.GetFirstWord(productBase3.ProdName);
						productBase3.Type = ((sereServs.Count() == sereServs.Count((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 6)) ? 1 : 0);
						productBase3.Stt = detail.Stt.GetValueOrDefault();
						list.Add(productBase3);
					}
					if (flag && detail.IsBHYT == 1)
					{
						List<long> priceBhytIds = (from s in sereServs
							where s.PRICE - s.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault() == 0m
							select s.ID).ToList();
						list2 = list2.Where((HIS_SERE_SERV_BILL o) => !priceBhytIds.Contains(o.ID)).ToList();
						continue;
					}
					list2 = list2.Where((HIS_SERE_SERV_BILL o) => !sereServs.Select((HIS_SERE_SERV_BILL s) => s.ID).Contains(o.ID)).ToList();
				}
				if (list2 != null && list2.Count > 0)
				{
					decimal num5 = default(decimal);
					if (flag)
					{
						num5 = list2.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
						num5 += list2.Where((HIS_SERE_SERV_BILL o) => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum((HIS_SERE_SERV_BILL s) => s.PRICE - s.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault());
					}
					else
					{
						num5 = list2.Sum((HIS_SERE_SERV_BILL s) => s.PRICE);
					}
					if (num5 != 0m)
					{
						ProductBase productBase4 = new ProductBase();
						productBase4.ProdName = "Dịch vụ khác";
						productBase4.ProdUnit = "Lần";
						productBase4.ProdCode = "DVK";
						productBase4.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(num5);
						productBase4.Type = ((list2.Count() == list2.Count((HIS_SERE_SERV_BILL o) => o.TDL_SERVICE_TYPE_ID == 6)) ? 1 : 0);
						productBase4.Stt = long.MaxValue;
						list.Add(productBase4);
					}
				}
				if (list.Count > 0)
				{
					list = list.OrderBy((ProductBase o) => o.Stt).ToList();
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
			return list;
		}
	}
}
