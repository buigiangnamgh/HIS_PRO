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
	internal class Template9 : IRunTemplate
	{
		private ElectronicBillDataInput DataInput;

		public Template9(ElectronicBillDataInput dataInput)
		{
			DataInput = dataInput;
		}

		public object Run()
		{
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			List<ProductBase> list = new List<ProductBase>();
			if (DataInput.SereServBill != null && DataInput.SereServBill.Count > 0)
			{
				List<IGrouping<long, HIS_SERE_SERV_BILL>> list2 = (from o in DataInput.SereServBill
					group o by o.TDL_PATIENT_TYPE_ID.GetValueOrDefault()).ToList();
				decimal num = default(decimal);
				decimal num2 = default(decimal);
				List<ProductBase> list3 = new List<ProductBase>();
				foreach (IGrouping<long, HIS_SERE_SERV_BILL> detail in list2)
				{
					if (detail.Key == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
					{
						num2 = detail.Sum((HIS_SERE_SERV_BILL o) => o.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault());
						num += detail.Sum((HIS_SERE_SERV_BILL o) => o.PRICE - o.TDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault());
						continue;
					}
					string prodName = "Tiền đối tượng khác";
					HIS_PATIENT_TYPE val = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == detail.Key);
					if (val != null)
					{
						prodName = "Tiền " + val.PATIENT_TYPE_NAME;
					}
					decimal num3 = detail.Sum((HIS_SERE_SERV_BILL o) => o.PRICE);
					decimal num4 = detail.Sum((HIS_SERE_SERV_BILL o) => o.TDL_AMOUNT.GetValueOrDefault() * o.TDL_ORIGINAL_PRICE.GetValueOrDefault() * (1m + o.TDL_VAT_RATIO.GetValueOrDefault()) - o.TDL_DISCOUNT.GetValueOrDefault());
					if (num4 > 0m && num3 > num4)
					{
						num += num3 - num4;
						num3 = num4;
					}
					ProductBase productBase = new ProductBase();
					productBase.ProdName = prodName;
					productBase.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(num3);
					productBase.ProdUnit = " ";
					productBase.ProdCode = General.GetFirstWord(productBase.ProdName);
					list3.Add(productBase);
				}
				if (num2 > 0m)
				{
					if (DataInput.LastPatientTypeAlter == null)
					{
						LogSystem.Debug("Khong tim thay doi tuong benh nhan hien tai!");
						throw new Exception("Không tìm thấy đối tượng bệnh nhân hiện tại");
					}
					decimal num5 = new BhytHeinProcessor().GetDefaultHeinRatio(DataInput.LastPatientTypeAlter.HEIN_TREATMENT_TYPE_CODE, DataInput.LastPatientTypeAlter.HEIN_CARD_NUMBER, DataInput.LastPatientTypeAlter.LEVEL_CODE, DataInput.LastPatientTypeAlter.RIGHT_ROUTE_CODE).GetValueOrDefault() * 100m;
					ProductBase productBase2 = new ProductBase();
					productBase2.ProdName = string.Format("Đồng chi trả bảo hiểm {0}%", Math.Round(100m - num5, 0, MidpointRounding.AwayFromZero));
					productBase2.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(num2);
					productBase2.ProdUnit = " ";
					productBase2.ProdCode = General.GetFirstWord(productBase2.ProdName);
					list.Add(productBase2);
				}
				if (list3 != null && list3.Count > 0)
				{
					list3 = list3.OrderBy((ProductBase o) => o.ProdName).ToList();
					foreach (ProductBase item in list3)
					{
						list.Add(item);
					}
				}
				if (num > 0m)
				{
					ProductBase productBase3 = new ProductBase();
					productBase3.ProdName = "Tiền phụ thu";
					productBase3.Amount = Inventec.Common.Number.Convert.NumberToNumberRoundMax4(num);
					productBase3.ProdUnit = " ";
					productBase3.ProdCode = General.GetFirstWord(productBase3.ProdName);
					list.Add(productBase3);
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
			return list;
		}
	}
}
