using System.Collections.Generic;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Base
{
	public class ElectronicBillDataInput
	{
		public string PaymentMethod { get; set; }

		public string Currency { get; set; }

		public string Converter { get; set; }

		public decimal? Discount { get; set; }

		public decimal? DiscountRatio { get; set; }

		public decimal? Amount { get; set; }

		public long PartnerInvoiceID { get; set; }

		public long? EinvoiceTypeId { get; set; }

		public string TemplateCode { get; set; }

		public string SymbolCode { get; set; }

		public long NumOrder { get; set; }

		public long TransactionTime { get; set; }

		public long? CancelTime { get; set; }

		public string CancelReason { get; set; }

		public string CancelUsername { get; set; }

		public string InvoiceCode { get; set; }

		public string ENumOrder { get; set; }

		public string TransactionCode { get; set; }

		public HIS_BRANCH Branch { get; set; }

		public V_HIS_TREATMENT_FEE Treatment { get; set; }

		public HIS_TRANSACTION Transaction { get; set; }

		public List<V_HIS_TRANSACTION> ListTransaction { get; set; }

		public List<V_HIS_SERE_SERV_5> SereServs { get; set; }

		public List<HIS_SERE_SERV_BILL> SereServBill { get; set; }

		public V_HIS_PATIENT_TYPE_ALTER LastPatientTypeAlter { get; set; }

		public bool IsTransactionList { get; set; }

		public string SaveFileName { get; set; }

		public ElectronicBillDataInput()
		{
		}

		public ElectronicBillDataInput(ElectronicBillDataInput data)
		{
			if (data != null)
			{
				Amount = data.Amount;
				Discount = data.Discount;
				DiscountRatio = data.DiscountRatio;
				PaymentMethod = data.PaymentMethod;
				Currency = data.Currency;
				SymbolCode = data.SymbolCode;
				TemplateCode = data.TemplateCode;
				EinvoiceTypeId = data.EinvoiceTypeId;
				TransactionTime = data.TransactionTime;
				Transaction = data.Transaction;
				ListTransaction = data.ListTransaction;
				Branch = data.Branch;
				Treatment = data.Treatment;
				LastPatientTypeAlter = data.LastPatientTypeAlter;
			}
		}
	}
}
