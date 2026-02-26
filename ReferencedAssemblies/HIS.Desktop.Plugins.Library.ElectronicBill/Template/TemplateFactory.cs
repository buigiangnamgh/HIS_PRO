using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Template
{
	public class TemplateFactory
	{
		public static IRunTemplate MakeIRun(TemplateEnum.TYPE tempType, ElectronicBillDataInput dataInput)
		{
			IRunTemplate runTemplate = null;
			try
			{
				if (dataInput == null)
				{
					throw new NullReferenceException();
				}
				HisConfigCFG.LoadConfigForDetail();
				ProcessDataSereServToSereServBill(tempType, ref dataInput);
				if (tempType != TemplateEnum.TYPE.TemplateNhaThuoc && tempType != TemplateEnum.TYPE.Template10 && HisConfigCFG.listTempalteSymbol != null && HisConfigCFG.listTempalteSymbol.Count > 0 && !string.IsNullOrWhiteSpace(dataInput.SymbolCode) && !string.IsNullOrWhiteSpace(dataInput.TemplateCode) && HisConfigCFG.listTempalteSymbol.Contains(string.Format("{0}-{1}", dataInput.TemplateCode, dataInput.SymbolCode)))
				{
					tempType = TemplateEnum.TYPE.Template4;
				}
				switch (tempType)
				{
				case TemplateEnum.TYPE.TemplateNhaThuoc:
					runTemplate = new TemplateNhaThuoc(dataInput);
					break;
				case TemplateEnum.TYPE.Template1:
					runTemplate = new Template1(dataInput.Treatment.ID, dataInput.Branch, dataInput.SereServBill, dataInput);
					break;
				case TemplateEnum.TYPE.Template2:
					runTemplate = new Template2(dataInput.Amount.GetValueOrDefault());
					break;
				case TemplateEnum.TYPE.Template3:
					runTemplate = new Template3(dataInput);
					break;
				case TemplateEnum.TYPE.Template4:
					runTemplate = new Template4(dataInput);
					break;
				case TemplateEnum.TYPE.Template5:
					runTemplate = new Template5(dataInput);
					break;
				case TemplateEnum.TYPE.Template6:
					runTemplate = new Template6(dataInput);
					break;
				case TemplateEnum.TYPE.Template7:
					runTemplate = new Template7(dataInput);
					break;
				case TemplateEnum.TYPE.Template8:
					runTemplate = new Template8(dataInput);
					break;
				case TemplateEnum.TYPE.Template9:
					runTemplate = new Template9(dataInput);
					break;
				case TemplateEnum.TYPE.Template10:
					runTemplate = new Template10(dataInput);
					break;
				case TemplateEnum.TYPE.Template11:
					runTemplate = new Template11(dataInput);
					break;
				}
				if (runTemplate == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + LogUtil.TraceData("dataInput", (object)dataInput), (Exception)ex);
				runTemplate = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				runTemplate = null;
			}
			return runTemplate;
		}

		internal static void ProcessDataSereServToSereServBill(TemplateEnum.TYPE tempType, ref ElectronicBillDataInput dataInput)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			try
			{
				if (dataInput != null && (dataInput.SereServBill == null || dataInput.SereServBill.Count <= 0) && dataInput.SereServs != null && dataInput.SereServs.Count > 0)
				{
					dataInput.SereServBill = new List<HIS_SERE_SERV_BILL>();
					foreach (V_HIS_SERE_SERV_5 sereServ in dataInput.SereServs)
					{
						HIS_SERE_SERV_BILL val = new HIS_SERE_SERV_BILL();
						DataObjectMapper.Map<HIS_SERE_SERV_BILL>((object)val, (object)sereServ);
						val.SERE_SERV_ID = sereServ.ID;
						val.PRICE = sereServ.VIR_TOTAL_PATIENT_PRICE.GetValueOrDefault();
						val.TDL_TREATMENT_ID = sereServ.TDL_TREATMENT_ID.GetValueOrDefault();
						val.TDL_ADD_PRICE = sereServ.ADD_PRICE;
						val.TDL_AMOUNT = sereServ.AMOUNT;
						val.TDL_DISCOUNT = sereServ.DISCOUNT;
						val.TDL_EXECUTE_DEPARTMENT_ID = sereServ.TDL_EXECUTE_DEPARTMENT_ID;
						val.TDL_HEIN_LIMIT_PRICE = sereServ.HEIN_LIMIT_PRICE;
						val.TDL_HEIN_LIMIT_RATIO = sereServ.HEIN_LIMIT_RATIO;
						val.TDL_HEIN_NORMAL_PRICE = sereServ.HEIN_NORMAL_PRICE;
						val.TDL_HEIN_PRICE = sereServ.HEIN_PRICE;
						val.TDL_HEIN_RATIO = sereServ.HEIN_RATIO;
						val.TDL_HEIN_SERVICE_TYPE_ID = sereServ.TDL_HEIN_SERVICE_TYPE_ID;
						val.TDL_IS_OUT_PARENT_FEE = sereServ.IS_OUT_PARENT_FEE;
						val.TDL_LIMIT_PRICE = sereServ.LIMIT_PRICE;
						val.TDL_ORIGINAL_PRICE = sereServ.ORIGINAL_PRICE;
						val.TDL_OTHER_SOURCE_PRICE = sereServ.OTHER_SOURCE_PRICE;
						val.TDL_OVERTIME_PRICE = sereServ.OVERTIME_PRICE;
						val.TDL_PATIENT_TYPE_ID = sereServ.PATIENT_TYPE_ID;
						val.TDL_PRICE = sereServ.PRICE;
						val.TDL_PRIMARY_PRICE = sereServ.PRIMARY_PRICE;
						val.TDL_REQUEST_DEPARTMENT_ID = sereServ.TDL_REQUEST_DEPARTMENT_ID;
						val.TDL_SERE_SERV_PARENT_ID = sereServ.PARENT_ID;
						val.TDL_SERVICE_CODE = sereServ.TDL_SERVICE_CODE;
						val.TDL_SERVICE_ID = sereServ.SERVICE_ID;
						val.TDL_SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
						val.TDL_SERVICE_TYPE_ID = sereServ.TDL_SERVICE_TYPE_ID;
						val.TDL_SERVICE_UNIT_ID = sereServ.TDL_SERVICE_UNIT_ID;
						val.TDL_TOTAL_HEIN_PRICE = sereServ.VIR_TOTAL_HEIN_PRICE;
						val.TDL_TOTAL_PATIENT_PRICE = sereServ.VIR_TOTAL_PATIENT_PRICE;
						val.TDL_TOTAL_PATIENT_PRICE_BHYT = sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT;
						val.TDL_USER_PRICE = sereServ.USER_PRICE;
						val.TDL_VAT_RATIO = sereServ.VAT_RATIO;
						val.TDL_REAL_HEIN_PRICE = sereServ.VIR_HEIN_PRICE;
						val.TDL_REAL_PATIENT_PRICE = sereServ.VIR_PATIENT_PRICE;
						val.TDL_REAL_PRICE = sereServ.VIR_PRICE;
						val.TDL_SERVICE_REQ_ID = sereServ.SERVICE_REQ_ID;
						val.TDL_PRIMARY_PATIENT_TYPE_ID = sereServ.PRIMARY_PATIENT_TYPE_ID;
						val.VAT_RATIO = sereServ.VAT_RATIO;
						if (tempType == TemplateEnum.TYPE.TemplateNhaThuoc && sereServ.PRICE <= 0m && sereServ.VIR_PRICE.GetValueOrDefault() > 0m)
						{
							val.TDL_PRICE = sereServ.VIR_PRICE.GetValueOrDefault();
						}
						else if (tempType == TemplateEnum.TYPE.TemplateNhaThuoc)
						{
							val.PRICE = sereServ.PRICE * sereServ.AMOUNT * (1m + sereServ.VAT_RATIO);
							val.TDL_SERVICE_TYPE_ID = 6L;
						}
						dataInput.SereServBill.Add(val);
					}
				}
				dataInput.SereServBill = dataInput.SereServBill.Where((HIS_SERE_SERV_BILL o) => o.PRICE != 0m).ToList();
				dataInput.SereServBill.ForEach(delegate(HIS_SERE_SERV_BILL o)
				{
					decimal? tDL_TOTAL_PATIENT_PRICE_BHYT2;
					if (o.TDL_TOTAL_PATIENT_PRICE_BHYT.HasValue)
					{
						decimal? tDL_TOTAL_PATIENT_PRICE_BHYT = o.TDL_TOTAL_PATIENT_PRICE_BHYT;
						decimal pRICE = o.PRICE;
						if ((tDL_TOTAL_PATIENT_PRICE_BHYT.GetValueOrDefault() > pRICE) & tDL_TOTAL_PATIENT_PRICE_BHYT.HasValue)
						{
							tDL_TOTAL_PATIENT_PRICE_BHYT2 = o.PRICE;
							goto IL_0049;
						}
					}
					tDL_TOTAL_PATIENT_PRICE_BHYT2 = o.TDL_TOTAL_PATIENT_PRICE_BHYT;
					goto IL_0049;
					IL_0049:
					o.TDL_TOTAL_PATIENT_PRICE_BHYT = tDL_TOTAL_PATIENT_PRICE_BHYT2;
				});
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal static List<V_HIS_SERE_SERV_5> GetSereServWithVAT(ElectronicBillDataInput dataInput)
		{
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			List<V_HIS_SERE_SERV_5> list = null;
			try
			{
				if (dataInput != null)
				{
					if (dataInput.SereServs != null && dataInput.SereServs.Count > 0)
					{
						list = dataInput.SereServs.Where((V_HIS_SERE_SERV_5 o) => o.VAT_RATIO > 0m).ToList();
					}
					else if (dataInput.SereServBill != null && dataInput.SereServBill.Count > 0)
					{
						list = new List<V_HIS_SERE_SERV_5>();
						foreach (HIS_SERE_SERV_BILL item in dataInput.SereServBill)
						{
							if (item.TDL_VAT_RATIO.HasValue && !(item.TDL_VAT_RATIO.Value <= 0m))
							{
								V_HIS_SERE_SERV_5 val = new V_HIS_SERE_SERV_5();
								val.ID = item.SERE_SERV_ID;
								val.TDL_TREATMENT_ID = item.TDL_TREATMENT_ID;
								val.ADD_PRICE = item.TDL_ADD_PRICE;
								val.AMOUNT = item.TDL_AMOUNT.GetValueOrDefault();
								val.DISCOUNT = item.TDL_DISCOUNT;
								val.TDL_EXECUTE_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID.GetValueOrDefault();
								val.HEIN_LIMIT_PRICE = item.TDL_HEIN_LIMIT_PRICE;
								val.HEIN_LIMIT_RATIO = item.TDL_HEIN_LIMIT_RATIO;
								val.HEIN_NORMAL_PRICE = item.TDL_HEIN_NORMAL_PRICE;
								val.HEIN_PRICE = item.TDL_HEIN_PRICE;
								val.HEIN_RATIO = item.TDL_HEIN_RATIO;
								val.TDL_HEIN_SERVICE_TYPE_ID = item.TDL_HEIN_SERVICE_TYPE_ID;
								val.IS_OUT_PARENT_FEE = item.TDL_IS_OUT_PARENT_FEE;
								val.LIMIT_PRICE = item.TDL_LIMIT_PRICE;
								val.ORIGINAL_PRICE = item.TDL_ORIGINAL_PRICE.GetValueOrDefault();
								val.OTHER_SOURCE_PRICE = item.TDL_OTHER_SOURCE_PRICE;
								val.OVERTIME_PRICE = item.TDL_OVERTIME_PRICE;
								val.PATIENT_TYPE_ID = item.TDL_PATIENT_TYPE_ID.GetValueOrDefault();
								val.PRICE = item.TDL_PRICE.GetValueOrDefault();
								val.PRIMARY_PRICE = item.TDL_PRIMARY_PRICE;
								val.TDL_REQUEST_DEPARTMENT_ID = item.TDL_REQUEST_DEPARTMENT_ID.GetValueOrDefault();
								val.PARENT_ID = item.TDL_SERE_SERV_PARENT_ID;
								val.TDL_SERVICE_CODE = item.TDL_SERVICE_CODE;
								val.SERVICE_ID = item.TDL_SERVICE_ID.GetValueOrDefault();
								val.TDL_SERVICE_NAME = item.TDL_SERVICE_NAME;
								val.TDL_SERVICE_TYPE_ID = item.TDL_SERVICE_TYPE_ID.GetValueOrDefault();
								val.TDL_SERVICE_UNIT_ID = item.TDL_SERVICE_UNIT_ID.GetValueOrDefault();
								val.VIR_TOTAL_HEIN_PRICE = item.TDL_TOTAL_HEIN_PRICE;
								val.VIR_TOTAL_PATIENT_PRICE = item.TDL_TOTAL_PATIENT_PRICE;
								val.VIR_TOTAL_PATIENT_PRICE_BHYT = item.TDL_TOTAL_PATIENT_PRICE_BHYT;
								val.USER_PRICE = item.TDL_USER_PRICE;
								val.VAT_RATIO = item.TDL_VAT_RATIO.GetValueOrDefault();
								val.VIR_HEIN_PRICE = item.TDL_REAL_HEIN_PRICE;
								val.VIR_PATIENT_PRICE = item.TDL_REAL_PATIENT_PRICE;
								val.VIR_PRICE = item.TDL_REAL_PRICE;
								val.SERVICE_REQ_ID = item.TDL_SERVICE_REQ_ID;
								val.PRIMARY_PATIENT_TYPE_ID = item.TDL_PRIMARY_PATIENT_TYPE_ID;
								list.Add(val);
							}
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
