using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.Library.ElectronicBill.Base;
using HIS.Desktop.Plugins.Library.ElectronicBill.Config;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.InvoiceInfo
{
	internal class InvoiceInfoProcessor
	{
		internal static InvoiceInfoADO GetData(ElectronicBillDataInput dataInput, bool isFillByTreatment = true)
		{
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Expected O, but got Unknown
			InvoiceInfoADO invoiceInfoADO = new InvoiceInfoADO();
			try
			{
				string text = "";
				string text2 = "";
				if (dataInput.Transaction != null)
				{
					if (dataInput.Transaction.BUYER_TYPE.HasValue)
					{
						isFillByTreatment = false;
					}
					invoiceInfoADO.BuyerOrganization = dataInput.Transaction.BUYER_ORGANIZATION;
					invoiceInfoADO.BuyerTaxCode = dataInput.Transaction.BUYER_TAX_CODE;
					invoiceInfoADO.BuyerAccountNumber = dataInput.Transaction.BUYER_ACCOUNT_NUMBER;
					invoiceInfoADO.BuyerAddress = dataInput.Transaction.BUYER_ADDRESS ?? " ";
					invoiceInfoADO.BuyerPhone = dataInput.Transaction.BUYER_PHONE;
					invoiceInfoADO.BuyerName = dataInput.Transaction.BUYER_NAME;
					invoiceInfoADO.BuyerDob = Inventec.Common.DateTime.Convert.TimeNumberToDateString(dataInput.Transaction.TDL_PATIENT_DOB.GetValueOrDefault());
					invoiceInfoADO.BuyerCode = dataInput.Transaction.TDL_PATIENT_CODE;
					invoiceInfoADO.BuyerGender = dataInput.Transaction.TDL_PATIENT_GENDER_NAME;
					invoiceInfoADO.Note = dataInput.Transaction.DESCRIPTION;
					invoiceInfoADO.TransactionTime = dataInput.Transaction.TRANSACTION_TIME;
					invoiceInfoADO.BuyerEmail = dataInput.Transaction.BUYER_EMAIL;
					invoiceInfoADO.BuyerIdentityNumber = dataInput.Transaction.BUYER_IDENTITY_NUMBER;
					invoiceInfoADO.BuyerIdentityType = dataInput.Transaction.BUYER_IDENTITY_TYPE.ToString() ?? "";
					text = dataInput.Transaction.TDL_PATIENT_CODE;
					text2 = dataInput.Transaction.TDL_TREATMENT_CODE;
					if (dataInput.Transaction.PAY_FORM_ID == 2)
					{
						invoiceInfoADO.PaymentMethod = "CK";
					}
					else if (dataInput.Transaction.PAY_FORM_ID == 1)
					{
						invoiceInfoADO.PaymentMethod = "TM";
					}
					else if (dataInput.Transaction.PAY_FORM_ID == 3)
					{
						invoiceInfoADO.PaymentMethod = "TM/CK";
					}
					else if (dataInput.Transaction.PAY_FORM_ID == 4 || dataInput.Transaction.PAY_FORM_ID == 5)
					{
						invoiceInfoADO.PaymentMethod = "THE";
					}
					else
					{
						HIS_PAY_FORM val = new HIS_PAY_FORM();
						val = BackendDataWorker.Get<HIS_PAY_FORM>().FirstOrDefault((HIS_PAY_FORM o) => o.ID == dataInput.Transaction.PAY_FORM_ID);
						if (val != null && !string.IsNullOrEmpty(val.ELECTRONIC_PAY_FORM_NAME))
						{
							invoiceInfoADO.PaymentMethod = val.ELECTRONIC_PAY_FORM_NAME;
						}
						else
						{
							invoiceInfoADO.PaymentMethod = "TM/CK";
						}
					}
				}
				else if (dataInput.ListTransaction != null && dataInput.ListTransaction.Count > 0)
				{
					V_HIS_TRANSACTION val2 = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.BUYER_ORGANIZATION)
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val2 != null)
					{
						invoiceInfoADO.BuyerOrganization = val2.BUYER_ORGANIZATION;
					}
					V_HIS_TRANSACTION val3 = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.BUYER_TAX_CODE)
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val3 != null)
					{
						invoiceInfoADO.BuyerTaxCode = val3.BUYER_TAX_CODE;
					}
					V_HIS_TRANSACTION val4 = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.BUYER_ACCOUNT_NUMBER)
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val4 != null)
					{
						invoiceInfoADO.BuyerAccountNumber = val4.BUYER_ACCOUNT_NUMBER;
					}
					V_HIS_TRANSACTION val5 = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.BUYER_ADDRESS)
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val5 != null)
					{
						invoiceInfoADO.BuyerAddress = val5.BUYER_ADDRESS ?? " ";
					}
					V_HIS_TRANSACTION val6 = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.BUYER_PHONE)
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val6 != null)
					{
						invoiceInfoADO.BuyerPhone = val2.BUYER_PHONE;
					}
					V_HIS_TRANSACTION val7 = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.BUYER_NAME)
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val7 != null)
					{
						invoiceInfoADO.BuyerName = val7.BUYER_NAME;
					}
					V_HIS_TRANSACTION val8 = (from o in dataInput.ListTransaction
						where o.TDL_PATIENT_DOB.HasValue
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val8 != null)
					{
						invoiceInfoADO.BuyerDob = Inventec.Common.DateTime.Convert.TimeNumberToDateString(val8.TDL_PATIENT_DOB.GetValueOrDefault());
					}
					V_HIS_TRANSACTION val9 = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.TDL_PATIENT_CODE)
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val9 != null)
					{
						invoiceInfoADO.BuyerCode = val9.TDL_PATIENT_CODE;
						text = val9.TDL_PATIENT_CODE;
					}
					V_HIS_TRANSACTION val10 = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.TDL_TREATMENT_CODE)
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val10 != null)
					{
						text2 = val10.TDL_TREATMENT_CODE;
					}
					V_HIS_TRANSACTION val11 = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.TDL_PATIENT_GENDER_NAME)
						orderby o.TRANSACTION_TIME descending
						select o).FirstOrDefault();
					if (val11 != null)
					{
						invoiceInfoADO.BuyerGender = val11.TDL_PATIENT_GENDER_NAME;
					}
					List<V_HIS_TRANSACTION> list = (from o in dataInput.ListTransaction
						where !string.IsNullOrWhiteSpace(o.DESCRIPTION)
						orderby o.TRANSACTION_TIME descending
						select o).ToList();
					if (list != null && list.Count > 0)
					{
						invoiceInfoADO.Note = string.Join("; ", list);
					}
					invoiceInfoADO.TransactionTime = dataInput.ListTransaction.Max((V_HIS_TRANSACTION o) => o.TRANSACTION_TIME);
				}
				if (dataInput.Treatment != null && isFillByTreatment)
				{
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerOrganization))
					{
						invoiceInfoADO.BuyerOrganization = dataInput.Treatment.TDL_PATIENT_WORK_PLACE_NAME ?? dataInput.Treatment.TDL_PATIENT_WORK_PLACE ?? "";
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerTaxCode))
					{
						invoiceInfoADO.BuyerTaxCode = dataInput.Treatment.TDL_PATIENT_TAX_CODE;
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerAccountNumber))
					{
						invoiceInfoADO.BuyerAccountNumber = dataInput.Treatment.TDL_PATIENT_ACCOUNT_NUMBER;
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerAddress))
					{
						invoiceInfoADO.BuyerAddress = dataInput.Treatment.TDL_PATIENT_ADDRESS ?? " ";
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerDob) || invoiceInfoADO.BuyerDob == "0")
					{
						invoiceInfoADO.BuyerDob = Inventec.Common.DateTime.Convert.TimeNumberToDateString(dataInput.Treatment.TDL_PATIENT_DOB);
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerCode))
					{
						invoiceInfoADO.BuyerCode = dataInput.Treatment.TDL_PATIENT_CODE;
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerName))
					{
						invoiceInfoADO.BuyerName = dataInput.Treatment.TDL_PATIENT_NAME;
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerPhone))
					{
						invoiceInfoADO.BuyerPhone = dataInput.Treatment.TDL_PATIENT_PHONE ?? dataInput.Treatment.TDL_PATIENT_MOBILE;
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerGender))
					{
						invoiceInfoADO.BuyerGender = dataInput.Treatment.TDL_PATIENT_GENDER_NAME;
					}
					if (string.IsNullOrWhiteSpace(text))
					{
						text = dataInput.Treatment.TDL_PATIENT_CODE;
					}
					if (string.IsNullOrWhiteSpace(text2))
					{
						text2 = dataInput.Treatment.TREATMENT_CODE;
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerCCCD))
					{
						invoiceInfoADO.BuyerCCCD = dataInput.Treatment.TDL_PATIENT_CCCD_NUMBER;
					}
					if (string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerCCCD))
					{
						invoiceInfoADO.BuyerCCCD = dataInput.Treatment.TDL_PATIENT_CMND_NUMBER;
					}
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerTaxCode))
				{
					invoiceInfoADO.BuyerTaxCode = invoiceInfoADO.BuyerTaxCode.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerAccountNumber))
				{
					invoiceInfoADO.BuyerAccountNumber = invoiceInfoADO.BuyerAccountNumber.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerAddress))
				{
					invoiceInfoADO.BuyerAddress = invoiceInfoADO.BuyerAddress.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerCode))
				{
					invoiceInfoADO.BuyerCode = invoiceInfoADO.BuyerCode.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerName))
				{
					invoiceInfoADO.BuyerName = invoiceInfoADO.BuyerName.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerPhone))
				{
					invoiceInfoADO.BuyerPhone = invoiceInfoADO.BuyerPhone.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerOrganization))
				{
					invoiceInfoADO.BuyerOrganization = invoiceInfoADO.BuyerOrganization.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerEmail))
				{
					invoiceInfoADO.BuyerEmail = invoiceInfoADO.BuyerEmail.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.Note))
				{
					invoiceInfoADO.Note = invoiceInfoADO.Note.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.PaymentMethod))
				{
					invoiceInfoADO.PaymentMethod = invoiceInfoADO.PaymentMethod.Trim();
				}
				if (!string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerCCCD))
				{
					invoiceInfoADO.BuyerCCCD = invoiceInfoADO.BuyerCCCD.Trim();
				}
				if (HisConfigCFG.BuyerCodeOption == "1")
				{
					invoiceInfoADO.BuyerCode = text;
				}
				else if (HisConfigCFG.BuyerCodeOption == "2")
				{
					invoiceInfoADO.BuyerCode = text2;
				}
				else
				{
					invoiceInfoADO.BuyerCode = "";
				}
				if (HisConfigCFG.BuyerOrganizationOption == "1" && string.IsNullOrWhiteSpace(invoiceInfoADO.BuyerOrganization))
				{
					invoiceInfoADO.BuyerOrganization = invoiceInfoADO.BuyerName;
				}
				else if (HisConfigCFG.BuyerOrganizationOption == "2")
				{
					Dictionary<string, string> dictionary = General.ProcessDicValueString(dataInput);
					invoiceInfoADO.BuyerOrganization = dictionary["CURRENT_ROOM_DEPARTMENT"];
				}
				if (!string.IsNullOrWhiteSpace(HisConfigCFG.BuyerNameOption))
				{
					Dictionary<string, string> dictionary2 = General.ProcessDicValueString(dataInput);
					string text3 = HisConfigCFG.BuyerNameOption;
					foreach (KeyValuePair<string, string> item in dictionary2)
					{
						text3 = text3.Replace(string.Format("%{0}%", item.Key), item.Value);
					}
					InvoiceInfoADO invoiceInfoADO2 = invoiceInfoADO;
					invoiceInfoADO2.BuyerName = invoiceInfoADO2.BuyerName + " " + text3;
				}
			}
			catch (Exception ex)
			{
				invoiceInfoADO = new InvoiceInfoADO();
				LogSystem.Error(ex);
			}
			return invoiceInfoADO;
		}
	}
}
