using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using Inventec.Common.Number;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Template
{
	internal class Template11 : IRunTemplate
	{
		private ElectronicBillDataInput DataInput;

		public Template11(ElectronicBillDataInput dataInput)
		{
			DataInput = dataInput;
		}

		public object Run()
		{
			List<ProductBase> list = new List<ProductBase>();
			IRunTemplate runTemplate = TemplateFactory.MakeIRun(TemplateEnum.TYPE.Template8, DataInput);
			object obj = runTemplate.Run();
			if (obj == null)
			{
				throw new Exception("Không có thông tin chi tiết dịch vụ.");
			}
			if (obj.GetType() == typeof(List<ProductBase>))
			{
				decimal num = default(decimal);
				List<string> list2 = new List<string>();
				List<ProductBase> list3 = (List<ProductBase>)obj;
				foreach (ProductBase item in list3)
				{
					list2.Add(string.Format("{0}({1})", item.ProdName, Inventec.Common.Number.Convert.NumberToStringRoundMax4(item.Amount)));
					num += item.Amount;
				}
				if (list2.Count > 0)
				{
					ProductBase productBase = new ProductBase();
					productBase.ProdName = string.Join("; ", list2);
					productBase.ProdCode = "TTVP";
					productBase.ProdPrice = num;
					productBase.ProdQuantity = 1;
					productBase.Amount = num;
					productBase.TaxRateID = 4;
					list.Add(productBase);
				}
			}
			if (list.Count > 0)
			{
				list = list.OrderBy((ProductBase o) => o.Stt).ToList();
				foreach (ProductBase item2 in list)
				{
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
