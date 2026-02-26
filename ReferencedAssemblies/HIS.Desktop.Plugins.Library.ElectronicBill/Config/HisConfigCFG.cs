using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;
using Inventec.Common.TypeConvert;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Config
{
	internal class HisConfigCFG
	{
		internal enum TaxBreakdownOption
		{
			Null,
			CoThongTinThue,
			KhongHienThiThongTinThue
		}

		internal const string ELECTRONIC_BILL__CONFIG = "HIS.DESKTOP.LIBRARY.ELECTRONIC_BILL.CONFIG";

		internal const string SERVICE_INDEPENDENT_DISPLAY = "HIS.DESKTOP.LIBRARY.ELECTRONIC_BILL.SERVICE_INDEPENDENT_DISPLAY";

		internal const string His_Desktop_plugins_ElectriconicBill_Viettel_TaxBreakdown = "HIS.Desktop.Plugins.Library.ElectronicBill.Viettel.TaxBreakdown";

		internal const string His_Desktop_plugins_ElectriconicBill_Viettel_Metadata = "HIS.Desktop.Plugins.Library.ElectronicBill.Viettel.Metadata";

		private const string His_Desktop_plugins_ElectriconicBill_NameOption = "HIS.Desktop.Plugins.Library.ElectronicBill.NameOption";

		private const string AutoPrintTypeCFG = "HIS.Desktop.Plugins.TransactionBill.ElectronicBill.AutoPrintType";

		private const string BuyerOrganizationOptionCFG = "HIS.Desktop.Plugins.TransactionBill.ElectronicBill.AutoFill.BuyerOrganizationOption";

		private const string BuyerNameOptionCFG = "HIS.Desktop.Plugins.Library.ElectronicBill.BuyerNameOption";

		private const string BuyerCodeOptionCFG = "HIS.Desktop.Plugins.Library.ElectronicBill.BuyerCodeOption";

		private const string ElectronicBillXmlInvoicePlusCFG = "HIS.Desktop.Plugins.Library.ElectronicBill.XmlInvoicePlus";

		internal const string His_Desktop_plugins_ElectriconicBill_Template = "HIS.Desktop.Plugins.Library.ElectronicBill.Template";

		internal const string TemplateDetail = "HIS.Desktop.Plugins.Library.ElectronicBill.Template.Detail";

		internal const string TemplateSplitVat = "HIS.Desktop.Plugins.Library.ElectronicBill.Template.SplitServicesWithVat";

		private const string splitDetai = "HIS.Desktop.Plugins.Library.ElectronicBill.TempalteSymbol.SplitDetail";

		private const string DetailInfoOptionCFG = "HIS.Desktop.Plugins.Library.ElectronicBill.DetailInfoOption";

		private const string ConvertVatRatioCFG = "HIS.Desktop.Plugins.Library.ElectronicBill.ConvertVatRatio";

		private const string RoundTransactionAmountCFG = "HIS.Desktop.Plugins.Library.ElectronicBill.RoundTransactionAmount";

		private const string VatOptionCFG = "HIS.Desktop.Plugins.TransactionList.ElectronicBill.VatOption";

		internal static bool IsHideUnitName;

		internal static bool IsHideQuantity;

		internal static bool IsHidePrice;

		internal static bool IsSwapNameOption;

		internal static bool IsPrintNormal;

		internal static bool IsSplitServicesWithVat;

		internal static List<string> listTempalteSymbol;

		internal static string ElectronicBillXmlInvoicePlus;

		internal static string BuyerNameOption;

		internal static string BuyerCodeOption;

		internal static string BuyerOrganizationOption;

		internal static string RoundTransactionAmountOption;

		internal static string VatOption;

		internal static Dictionary<decimal, decimal> dicVatConvert = new Dictionary<decimal, decimal>();

		internal static TaxBreakdownOption Viettel_TaxBreakdownOption;

		internal static List<V_HIS_NONE_MEDI_SERVICE> V_HIS_NONE_MEDI_SERVICEs
		{
			get
			{
				List<V_HIS_NONE_MEDI_SERVICE> list = new List<V_HIS_NONE_MEDI_SERVICE>();
				try
				{
					list = BackendDataWorker.Get<V_HIS_NONE_MEDI_SERVICE>();
				}
				catch (Exception ex)
				{
					list = new List<V_HIS_NONE_MEDI_SERVICE>();
					LogSystem.Error(ex);
				}
				if (list == null)
				{
					list = new List<V_HIS_NONE_MEDI_SERVICE>();
				}
				return list;
			}
		}

		internal static List<V_HIS_MATERIAL_TYPE> V_HIS_MATERIAL_TYPEs
		{
			get
			{
				List<V_HIS_MATERIAL_TYPE> list = new List<V_HIS_MATERIAL_TYPE>();
				try
				{
					list = BackendDataWorker.Get<V_HIS_MATERIAL_TYPE>();
				}
				catch (Exception ex)
				{
					list = new List<V_HIS_MATERIAL_TYPE>();
					LogSystem.Error(ex);
				}
				if (list == null)
				{
					list = new List<V_HIS_MATERIAL_TYPE>();
				}
				return list;
			}
		}

		internal static List<V_HIS_MEDICINE_TYPE> V_HIS_MEDICINE_TYPEs
		{
			get
			{
				List<V_HIS_MEDICINE_TYPE> list = new List<V_HIS_MEDICINE_TYPE>();
				try
				{
					list = BackendDataWorker.Get<V_HIS_MEDICINE_TYPE>();
				}
				catch (Exception ex)
				{
					list = new List<V_HIS_MEDICINE_TYPE>();
					LogSystem.Error(ex);
				}
				if (list == null)
				{
					list = new List<V_HIS_MEDICINE_TYPE>();
				}
				return list;
			}
		}

		internal static void LoadConfig()
		{
			try
			{
				LogSystem.Debug("LoadConfig => 1");
				IsSwapNameOption = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.NameOption") == "1";
				IsPrintNormal = GetValue("HIS.Desktop.Plugins.TransactionBill.ElectronicBill.AutoPrintType") == "1";
				IsSplitServicesWithVat = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.Template.SplitServicesWithVat") == "1";
				ElectronicBillXmlInvoicePlus = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.XmlInvoicePlus");
				BuyerNameOption = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.BuyerNameOption");
				BuyerCodeOption = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.BuyerCodeOption");
				BuyerOrganizationOption = GetValue("HIS.Desktop.Plugins.TransactionBill.ElectronicBill.AutoFill.BuyerOrganizationOption");
				VatOption = GetValue("HIS.Desktop.Plugins.TransactionList.ElectronicBill.VatOption");
				string value = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.Viettel.TaxBreakdown");
				string text = value;
				string text2 = text;
				if (!(text2 == "1"))
				{
					if (text2 == "2")
					{
						Viettel_TaxBreakdownOption = TaxBreakdownOption.KhongHienThiThongTinThue;
					}
					else
					{
						Viettel_TaxBreakdownOption = TaxBreakdownOption.Null;
					}
				}
				else
				{
					Viettel_TaxBreakdownOption = TaxBreakdownOption.CoThongTinThue;
				}
				LogSystem.Debug("LoadConfig => 2");
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal static void LoadConfigForDetail()
		{
			try
			{
				listTempalteSymbol = new List<string>();
				string value = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.TempalteSymbol.SplitDetail");
				if (!string.IsNullOrWhiteSpace(value))
				{
					listTempalteSymbol = value.Split('|').ToList();
				}
				RoundTransactionAmountOption = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.RoundTransactionAmount");
				IsHideUnitName = false;
				IsHideQuantity = false;
				IsHidePrice = false;
				string value2 = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.DetailInfoOption");
				if (!string.IsNullOrWhiteSpace(value2))
				{
					string[] array = value2.Split('|');
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] == "1")
						{
							switch (i)
							{
							case 0:
								IsHideUnitName = true;
								break;
							case 1:
								IsHideQuantity = true;
								break;
							case 2:
								IsHidePrice = true;
								break;
							}
						}
					}
				}
				dicVatConvert = new Dictionary<decimal, decimal>();
				string value3 = GetValue("HIS.Desktop.Plugins.Library.ElectronicBill.ConvertVatRatio");
				if (string.IsNullOrWhiteSpace(value3))
				{
					return;
				}
				string[] array2 = value3.Split('|');
				string[] array3 = array2;
				foreach (string text in array3)
				{
					string[] separator = new string[1] { "->" };
					string[] array4 = text.Split(separator, StringSplitOptions.None);
					if (array4.Length >= 2)
					{
						decimal num = Parse.ToDecimal(array4[0]);
						decimal num2 = Parse.ToDecimal(array4[1]);
						if (true && (num > 0m || (num == 0m && array4[0] == "0")) && (num2 > 0m || (num2 == 0m && array4[1] == "0")))
						{
							dicVatConvert[num] = num2;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private static string GetValue(string key)
		{
			try
			{
				return HisConfigs.Get<string>(key);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return "";
		}
	}
}
