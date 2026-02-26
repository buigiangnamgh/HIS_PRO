using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Common.String;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein.Bhyt;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Base
{
	internal class General
	{
		internal static Dictionary<string, string> DicDataBuyerInfo;

		internal static string GetFirstWord(string name)
		{
			string text = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(name))
				{
					List<string> list = name.Split(' ', ',', '.', '/').ToList();
					foreach (string item in list)
					{
						if (!string.IsNullOrWhiteSpace(item) && char.IsLetter(item[0]))
						{
							text += item[0].ToString().ToUpper();
						}
					}
				}
				if (!string.IsNullOrWhiteSpace(text))
				{
					text = Inventec.Common.String.Convert.UnSignVNese2(text);
				}
			}
			catch (Exception ex)
			{
				text = name.Split(' ', ',', '.').FirstOrDefault();
				LogSystem.Error(ex);
			}
			return text;
		}

		internal static string GenarateXmlStringFromConfig(ElectronicBillDataInput inputData, Type dataType, Dictionary<string, string> dicReplateValue)
		{
			string result = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(HisConfigCFG.ElectronicBillXmlInvoicePlus) && inputData != null)
				{
					string[] array = HisConfigCFG.ElectronicBillXmlInvoicePlus.Split('|');
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					string[] array2 = array;
					foreach (string text in array2)
					{
						string[] array3 = text.Split(':');
						if (array3.Count() <= 1)
						{
							continue;
						}
						string text2 = text.Replace(array3[0] + ":", "");
						if (!string.IsNullOrEmpty(text2))
						{
							char c = text2.FirstOrDefault((char ch) => !XmlConvert.IsXmlChar(ch));
							if (!char.IsWhiteSpace(c))
							{
								dictionary[array3[0]] = string.Format("<![CDATA[{0}]]>", text2);
							}
							else
							{
								dictionary[array3[0]] = text2;
							}
						}
						else
						{
							dictionary[array3[0]] = null;
						}
					}
					Dictionary<string, string> dictionary2 = ProcessDicValueString(inputData);
					PropertyInfo[] array4 = Properties.Get(dataType);
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, string> item in dictionary)
					{
						string text3 = item.Value;
						if (!string.IsNullOrWhiteSpace(text3))
						{
							foreach (KeyValuePair<string, string> item2 in dictionary2)
							{
								text3 = text3.Replace(item2.Key, item2.Value);
							}
						}
						bool flag = false;
						PropertyInfo[] array5 = array4;
						foreach (PropertyInfo propertyInfo in array5)
						{
							if (propertyInfo.Name == item.Key)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							dicReplateValue[item.Key] = text3;
						}
						else
						{
							list.Add(string.Format("<{0}>{1}</{0}>", item.Key, text3));
						}
					}
					result = string.Join("", list);
				}
			}
			catch (Exception ex)
			{
				result = "";
				LogSystem.Error(ex);
			}
			return result;
		}

		internal static Dictionary<string, string> ProcessDicValueString(ElectronicBillDataInput inputData)
		{
			//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_0468: Expected O, but got Unknown
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Expected O, but got Unknown
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Expected O, but got Unknown
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Expected O, but got Unknown
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			try
			{
				if (DicDataBuyerInfo != null && DicDataBuyerInfo.Count > 0)
				{
					return DicDataBuyerInfo;
				}
				string value = "";
				string value2 = "";
				string value3 = "";
				string value4 = "";
				string value5 = "";
				string value6 = "";
				string value7 = "";
				string value8 = "";
				string value9 = "";
				string value10 = "";
				string value11 = "";
				string value12 = "";
				string value13 = "";
				string value14 = "";
				string value15 = "";
				string value16 = "";
				string value17 = "";
				if (inputData.Treatment != null)
				{
					long tDL_PATIENT_DOB = inputData.Treatment.TDL_PATIENT_DOB;
					long num = 0L;
					if (tDL_PATIENT_DOB > 0)
					{
						value = tDL_PATIENT_DOB.ToString().Substring(0, 4);
						num = ((inputData.Treatment != null) ? inputData.Treatment.IN_TIME : Inventec.Common.DateTime.Get.Now().GetValueOrDefault());
						value2 = ((tDL_PATIENT_DOB > 0 && num > 0) ? Calculation.AgeString(tDL_PATIENT_DOB, "", "tháng", "ngày", "giờ", (long?)num) : "");
						value3 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(tDL_PATIENT_DOB);
					}
					value14 = ((num > 0) ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(num) : "");
					if (inputData.Treatment.CLINICAL_IN_TIME.HasValue)
					{
						value16 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(inputData.Treatment.CLINICAL_IN_TIME.Value);
					}
					value7 = inputData.Treatment.TREATMENT_CODE;
					value8 = inputData.Treatment.TDL_PATIENT_CODE;
					value4 = inputData.Treatment.TDL_PATIENT_GENDER_NAME;
					value5 = inputData.Treatment.TDL_PATIENT_CCCD_NUMBER;
					value6 = inputData.Treatment.TDL_PATIENT_CMND_NUMBER;
					HIS_PATIENT_TYPE val = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == inputData.Treatment.TDL_PATIENT_TYPE_ID);
					value9 = ((val != null) ? val.PATIENT_TYPE_NAME : "");
					HIS_TREATMENT_TYPE val2 = BackendDataWorker.Get<HIS_TREATMENT_TYPE>().FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == inputData.Treatment.TDL_TREATMENT_TYPE_ID);
					value10 = ((val2 != null) ? val2.TREATMENT_TYPE_NAME : "");
					if (inputData.Treatment.TDL_TREATMENT_TYPE_ID == 1)
					{
						V_HIS_ROOM val3 = (inputData.Treatment.END_ROOM_ID.HasValue ? BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == inputData.Treatment.END_ROOM_ID) : null);
						if (val3 != null)
						{
							value11 = val3.ROOM_NAME;
						}
						else
						{
							HisSereServFilter val4 = new HisSereServFilter();
							val4.TREATMENT_ID = inputData.Treatment.ID;
							val4.TDL_SERVICE_TYPE_ID = 1L;
							List<HIS_SERE_SERV> sereServKham = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_SERE_SERV>>("api/HisSereServ/Get", ApiConsumers.MosConsumer, (object)val4, (CommonParam)null);
							if (sereServKham != null && sereServKham.Count > 0)
							{
								sereServKham = (from o in sereServKham
									orderby o.TDL_IS_MAIN_EXAM.GetValueOrDefault() descending, o.TDL_INTRUCTION_TIME descending
									select o).ToList();
								V_HIS_ROOM val5 = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == sereServKham.First().TDL_EXECUTE_ROOM_ID);
								if (val5 != null)
								{
									value11 = val5.ROOM_NAME;
								}
							}
						}
					}
					else if (inputData.Treatment.LAST_DEPARTMENT_ID.HasValue)
					{
						HIS_DEPARTMENT val6 = BackendDataWorker.Get<HIS_DEPARTMENT>().FirstOrDefault((HIS_DEPARTMENT o) => o.ID == inputData.Treatment.LAST_DEPARTMENT_ID.Value);
						if (val6 != null)
						{
							value11 = val6.DEPARTMENT_NAME;
						}
					}
					else
					{
						HisDepartmentTranLastFilter val7 = new HisDepartmentTranLastFilter();
						val7.TREATMENT_ID = inputData.Treatment.ID;
						V_HIS_DEPARTMENT_TRAN val8 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<V_HIS_DEPARTMENT_TRAN>("api/HisDepartmentTran/GetLastByTreatmentId", ApiConsumers.MosConsumer, (object)val7, (CommonParam)null);
						if (val8 != null)
						{
							value11 = val8.DEPARTMENT_NAME;
						}
					}
					if (inputData.Treatment.OUT_TIME.HasValue)
					{
						value15 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(inputData.Treatment.OUT_TIME.Value);
					}
				}
				if (inputData.Transaction != null)
				{
					value13 = inputData.Transaction.CASHIER_LOGINNAME;
					value12 = inputData.Transaction.CASHIER_USERNAME;
					value17 = inputData.Transaction.NUM_ORDER.ToString() ?? "";
				}
				else if (inputData.ListTransaction != null && inputData.ListTransaction.Count > 0)
				{
					V_HIS_TRANSACTION val9 = inputData.ListTransaction.OrderByDescending((V_HIS_TRANSACTION o) => o.ID).FirstOrDefault();
					value13 = val9.CASHIER_LOGINNAME;
					value12 = val9.CASHIER_USERNAME;
				}
				string value18 = "Thu viện phí";
				if (inputData.Transaction != null && (!string.IsNullOrWhiteSpace(inputData.Transaction.EXEMPTION_REASON) || !string.IsNullOrWhiteSpace(inputData.Transaction.DESCRIPTION)))
				{
					if (!string.IsNullOrWhiteSpace(inputData.Transaction.EXEMPTION_REASON))
					{
						value18 = inputData.Transaction.EXEMPTION_REASON;
					}
					else if (!string.IsNullOrWhiteSpace(inputData.Transaction.DESCRIPTION))
					{
						value18 = inputData.Transaction.DESCRIPTION;
					}
				}
				string value19 = "";
				string value20 = "";
				if (inputData.LastPatientTypeAlter != null)
				{
					value19 = new BhytHeinProcessor().GetDefaultHeinRatio(inputData.LastPatientTypeAlter.HEIN_TREATMENT_TYPE_CODE, inputData.LastPatientTypeAlter.HEIN_CARD_NUMBER, inputData.LastPatientTypeAlter.LEVEL_CODE, inputData.LastPatientTypeAlter.RIGHT_ROUTE_CODE).GetValueOrDefault() * 100m + "%";
					value20 = inputData.LastPatientTypeAlter.HEIN_CARD_NUMBER;
				}
				dictionary["YOB"] = value;
				dictionary["AGE"] = value2;
				dictionary["TREATMENT_CODE"] = value7;
				dictionary["PATIENT_CODE"] = value8;
				dictionary["PATIENT_TYPE_NAME"] = value9;
				dictionary["TREATMENT_TYPE_NAME"] = value10;
				dictionary["CURRENT_ROOM_DEPARTMENT"] = value11;
				dictionary["CASHIER_LOGINNAME"] = value13;
				dictionary["CASHIER_USERNAME"] = value12;
				dictionary["IN_TIME"] = value14;
				dictionary["OUT_TIME"] = value15;
				dictionary["REASON"] = value18;
				dictionary["TRANSACTION_NUM_ORDER"] = value17;
				dictionary["GENDER_NAME"] = value4;
				dictionary["CLINICAL_IN_TIME"] = value16;
				dictionary["HEIN_RATIO"] = value19;
				dictionary["DOB_STR"] = value3;
				dictionary["HEIN_CARD_NUMBER"] = value20;
				dictionary["CCCD_NUMBER"] = value5;
				dictionary["CMND_NUMBER"] = value6;
			}
			catch (Exception ex)
			{
				dictionary = new Dictionary<string, string>();
				LogSystem.Error(ex);
			}
			finally
			{
				DicDataBuyerInfo = dictionary;
			}
			return dictionary;
		}
	}
}
