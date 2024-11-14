using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.LocalStorage.LocalData;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Common.Message;
using MOS.EFMODEL.DataModels;
using MOS.SDO;
using MPS.ProcessorBase.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.Desktop.Plugins.RegisterExamKiosk
{
    public class PrintKiosk
    {
        V_HIS_PATIENT_TYPE_ALTER patyAlter { get; set; }
        V_HIS_PATIENT patient { get; set; }
        V_HIS_SERVICE_REQ ServiceReqPrint { get; set; }
        List<V_HIS_SERE_SERV> sereServs { get; set; }
        DelegateReturnSuccess success { get; set; }
        string printTypeCode { get; set; }
        string fileName { get; set; }
        HIS_TREATMENT treatmentPrint { get; set; }
        List<HIS_SERE_SERV_DEPOSIT> ListSereServDeposit { get; set; }
        List<HIS_SERE_SERV_BILL> ListSereServBill { get; set; }
        List<V_HIS_TRANSACTION> ListTransaction { get; set; }
        bool checkPrintAgain { get; set; }
        List<HIS_CARD> listHisCard { get; set; }
        Inventec.Desktop.Common.Modules.Module currentModule { get; set; }

        public PrintKiosk(Inventec.Desktop.Common.Modules.Module currentModule, V_HIS_PATIENT_TYPE_ALTER patyAlter, V_HIS_SERVICE_REQ ServiceReqPrint, List<V_HIS_SERE_SERV> sereServs, DelegateReturnSuccess success, string printTypeCode, string fileName, HIS_TREATMENT treatmentPrint, List<HIS_SERE_SERV_DEPOSIT> _ListSereServDeposit, List<HIS_SERE_SERV_BILL> _ListSereServBill, List<V_HIS_TRANSACTION> _ListTransaction, bool _checkPrintAgain)
        {
            this.currentModule = currentModule;
            this.patient = patient;
            this.patyAlter = patyAlter;
            this.ServiceReqPrint = ServiceReqPrint;
            this.sereServs = sereServs;
            this.success = success;
            this.printTypeCode = printTypeCode;
            this.fileName = fileName;
            this.treatmentPrint = treatmentPrint;
            this.ListSereServDeposit = _ListSereServDeposit;
            this.ListSereServBill = _ListSereServBill;
            this.ListTransaction = _ListTransaction;
            this.checkPrintAgain = _checkPrintAgain;
        }
        public PrintKiosk(Inventec.Desktop.Common.Modules.Module currentModule, V_HIS_PATIENT_TYPE_ALTER patyAlter, V_HIS_SERVICE_REQ ServiceReqPrint, List<V_HIS_SERE_SERV> sereServs, DelegateReturnSuccess success, string printTypeCode, string fileName, HIS_TREATMENT treatmentPrint, List<HIS_SERE_SERV_DEPOSIT> _ListSereServDeposit, List<HIS_SERE_SERV_BILL> _ListSereServBill, List<V_HIS_TRANSACTION> _ListTransaction, bool _checkPrintAgain, List<HIS_CARD> _listHisCard)
        {
            this.currentModule = currentModule;
            this.patient = patient;
            this.patyAlter = patyAlter;
            this.ServiceReqPrint = ServiceReqPrint;
            this.sereServs = sereServs;
            this.success = success;
            this.printTypeCode = printTypeCode;
            this.fileName = fileName;
            this.treatmentPrint = treatmentPrint;
            this.ListSereServDeposit = _ListSereServDeposit;
            this.ListSereServBill = _ListSereServBill;
            this.ListTransaction = _ListTransaction;
            this.checkPrintAgain = _checkPrintAgain;
            this.listHisCard = _listHisCard;
        }
        //public void RunPrintHasCard()
        //{
        //    try
        //    {
        //        MPS.Processor.Mps000001.PDO.Mps000001PDO Mps000001PDO = new MPS.Processor.Mps000001.PDO.Mps000001PDO(this.ListSereServDeposit, this.ListSereServBill, this.ListTransaction, this.ServiceReqPrint, this.sereServs, this.patyAlter, this.patyAlter.PATIENT_TYPE_NAME, treatmentPrint, this.listHisCard);
        //        LogSystem.Debug(LogUtil.TraceData("Du lieu Mps000001PDO khi dang ky kham", Mps000001PDO));
        //        WaitingManager.Hide();

        //        string templateFile = Application.StartupPath + "\\Mps000025__Temp__" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
        //        if (File.Exists(templateFile))
        //        {
        //            try
        //            {
        //                File.Delete(templateFile);
        //            }
        //            catch { }
        //        }

        //        LogSystem.Info(LogUtil.TraceData("Du lieu templateFile truyen vao", templateFile));

        //        PrintData printData = new PrintData(printTypeCode, fileName, Mps000001PDO, MPS.ProcessorBase.PrintConfig.PreviewType.SaveFile, "", 1, templateFile);

        //        bool printSuccess = MPS.MpsPrinter.Run(printData);
        //        if (printSuccess && File.Exists(templateFile))
        //        {
        //            Inventec.Common.MSOfficePrint.MSOfficePrintProcessor printProcessor = new Inventec.Common.MSOfficePrint.MSOfficePrintProcessor(templateFile, null, null, printData.numCopy, false, "");

        //            if (!printProcessor.Print())
        //            {
        //                Inventec.Common.Logging.LogSystem.Warn("Call printProcessor.Print fail.");
        //            }
        //            else
        //            {
        //                if (this.checkPrintAgain)
        //                {
        //                    if (this.sereServs != null && this.sereServs.Count > 0)
        //                    {
        //                        AutoMapper.Mapper.CreateMap<V_HIS_SERE_SERV, HIS_SERE_SERV>();
        //                        var servSereApi = AutoMapper.Mapper.Map<List<HIS_SERE_SERV>>(this.sereServs);

        //                        if (servSereApi != null && servSereApi.Count > 0)
        //                        {
        //                            foreach (var item in servSereApi)
        //                            {
        //                                if (item.IS_NO_EXECUTE == 1)
        //                                    item.IS_NO_EXECUTE = null;
        //                            }
        //                        }

        //                        LogSystem.Debug(LogUtil.TraceData("api cap nhat servsere truyen len", servSereApi));

        //                        CommonParam param = new CommonParam();
        //                        HisSereServPayslipSDO sdo = new HisSereServPayslipSDO();
        //                        sdo.SereServs = servSereApi;
        //                        sdo.Field = MOS.SDO.UpdateField.IS_NO_EXECUTE;
        //                        sdo.TreatmentId = this.ServiceReqPrint.TREATMENT_ID;
        //                        LogSystem.Info(LogUtil.TraceData("api cap nhat servsere truyen len", sdo));

        //                        var updateSs = new Inventec.Common.Adapter.BackendAdapter(param).Post<List<HIS_SERE_SERV>>("api/HisSereServ/UpdatePayslipInfo", ApiConsumers.MosConsumer, sdo, param);
        //                        LogSystem.Info(LogUtil.TraceData("api cap nhat servsere tra ve", updateSs));
        //                    }
        //                }
        //            }

        //            if (this.success != null)
        //            {
        //                this.success(true);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Warn(ex);
        //    }
        //}
        //public void RunPrint()
        //{
        //    try
        //    {
        //        List<MPS.Processor.Mps000001.PDO.Mps000001_ListSereServs> ListSereServs = new List<MPS.Processor.Mps000001.PDO.Mps000001_ListSereServs>();
        //        MPS.Processor.Mps000001.PDO.Mps000001ADO ado = new MPS.Processor.Mps000001.PDO.Mps000001ADO();
        //        //MPS.Processor.Mps000001.PDO.Mps000001PDO Mps000001PDO = new MPS.Processor.Mps000001.PDO.Mps000001PDO(this.ListSereServDeposit, this.ListSereServBill, this.ListTransaction, this.ServiceReqPrint, this.sereServs, this.patyAlter, this.patyAlter.PATIENT_TYPE_NAME, treatmentPrint);
        //        MPS.Processor.Mps000001.PDO.Mps000001PDO Mps000001PDO = new MPS.Processor.Mps000001.PDO.Mps000001PDO(this.ServiceReqPrint, this.patyAlter, this.patient, ListSereServs, treatmentPrint, ado);
        //        LogSystem.Debug(LogUtil.TraceData("Du lieu Mps000001PDO khi dang ky kham", Mps000001PDO));
        //        WaitingManager.Hide();

        //        string templateFile = Application.StartupPath + "\\Mps000025__Temp__" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
        //        if (File.Exists(templateFile))
        //        {
        //            try
        //            {
        //                File.Delete(templateFile);
        //            }
        //            catch { }
        //        }

        //        LogSystem.Info(LogUtil.TraceData("Du lieu templateFile truyen vao", templateFile));

        //        PrintData printData = new PrintData(printTypeCode, fileName, Mps000001PDO, MPS.ProcessorBase.PrintConfig.PreviewType.SaveFile, "", 1, templateFile);

        //        bool printSuccess = MPS.MpsPrinter.Run(printData);
        //        if (printSuccess && File.Exists(templateFile))
        //        {
        //            Inventec.Common.MSOfficePrint.MSOfficePrintProcessor printProcessor = new Inventec.Common.MSOfficePrint.MSOfficePrintProcessor(templateFile, null, null, printData.numCopy, false, "");

        //            if (!printProcessor.Print())
        //            {
        //                Inventec.Common.Logging.LogSystem.Warn("Call printProcessor.Print fail.");
        //            }
        //            else
        //            {
        //                if (this.checkPrintAgain)
        //                {
        //                    if (this.sereServs != null && this.sereServs.Count > 0)
        //                    {
        //                        AutoMapper.Mapper.CreateMap<V_HIS_SERE_SERV, HIS_SERE_SERV>();
        //                        var servSereApi = AutoMapper.Mapper.Map<List<HIS_SERE_SERV>>(this.sereServs);

        //                        if (servSereApi != null && servSereApi.Count > 0)
        //                        {
        //                            foreach (var item in servSereApi)
        //                            {
        //                                if (item.IS_NO_EXECUTE == 1)
        //                                    item.IS_NO_EXECUTE = null;
        //                            }
        //                        }

        //                        LogSystem.Debug(LogUtil.TraceData("api cap nhat servsere truyen len", servSereApi));

        //                        CommonParam param = new CommonParam();
        //                        HisSereServPayslipSDO sdo = new HisSereServPayslipSDO();
        //                        sdo.SereServs = servSereApi;
        //                        sdo.Field = MOS.SDO.UpdateField.IS_NO_EXECUTE;
        //                        sdo.TreatmentId = this.ServiceReqPrint.TREATMENT_ID;
        //                        LogSystem.Info(LogUtil.TraceData("api cap nhat servsere truyen len", sdo));

        //                        var updateSs = new Inventec.Common.Adapter.BackendAdapter(param).Post<List<HIS_SERE_SERV>>("api/HisSereServ/UpdatePayslipInfo", ApiConsumers.MosConsumer, sdo, param);
        //                        LogSystem.Info(LogUtil.TraceData("api cap nhat servsere tra ve", updateSs));
        //                    }
        //                }
        //            }

        //            if (this.success != null)
        //            {
        //                this.success(true);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Warn(ex);
        //    }
        //}

        public void PrintProcess()
        {
            try
            {
                HisServiceReqListResultSDO HisServiceReqSDO = new HisServiceReqListResultSDO();
                HisServiceReqSDO.SereServs = this.sereServs;
                HisServiceReqSDO.ServiceReqs = new List<V_HIS_SERVICE_REQ> { this.ServiceReqPrint };
                HisServiceReqSDO.SereServBills = ListSereServBill;
                HisServiceReqSDO.SereServDeposits = ListSereServDeposit;
                HisServiceReqSDO.Transactions = this.ListTransaction;
                //HisServiceReqSDO.DepositedSereServs = this.currentHisExamServiceReqResultSDO.DepositedSereServs;

                HisTreatmentWithPatientTypeInfoSDO HisTreatment = new HisTreatmentWithPatientTypeInfoSDO();
                CommonParam param = new CommonParam();
                MOS.Filter.HisTreatmentWithPatientTypeInfoFilter filter = new MOS.Filter.HisTreatmentWithPatientTypeInfoFilter();
                filter.TREATMENT_ID = treatmentPrint.ID;
                filter.INTRUCTION_TIME = null;
                var hisTreatments = new BackendAdapter(param).Get<List<HisTreatmentWithPatientTypeInfoSDO>>("api/HisTreatment/GetTreatmentWithPatientTypeInfoSdo", ApiConsumers.MosConsumer, filter, param);
                if (hisTreatments != null && hisTreatments.Count > 0)
                {
                    HisTreatment = hisTreatments.FirstOrDefault();
                }
                if (HisTreatment.TDL_PATIENT_TYPE_ID != null && string.IsNullOrEmpty(HisTreatment.PATIENT_TYPE_CODE))
                {

                    HisTreatment.PATIENT_TYPE_CODE = LocalStorage.BackendData.BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault(o => o.ID == HisTreatment.TDL_PATIENT_TYPE_ID).PATIENT_TYPE_CODE;
                    HisTreatment.HEIN_CARD_FROM_TIME = HisTreatment.TDL_HEIN_CARD_FROM_TIME ?? 0;
                    HisTreatment.HEIN_CARD_NUMBER = HisTreatment.TDL_HEIN_CARD_NUMBER;
                    HisTreatment.HEIN_CARD_TO_TIME = HisTreatment.TDL_HEIN_CARD_TO_TIME ?? 0;
                    HisTreatment.HEIN_MEDI_ORG_CODE = HisTreatment.TDL_HEIN_MEDI_ORG_CODE;
                    HisTreatment.LEVEL_CODE = patyAlter.LEVEL_CODE;
                    HisTreatment.RIGHT_ROUTE_CODE = patyAlter.RIGHT_ROUTE_CODE;
                    HisTreatment.RIGHT_ROUTE_TYPE_CODE = patyAlter.RIGHT_ROUTE_TYPE_CODE;
                    HisTreatment.TREATMENT_TYPE_CODE = LocalStorage.BackendData.BackendDataWorker.Get<HIS_TREATMENT_TYPE>().FirstOrDefault(o => o.ID == HisTreatment.TDL_TREATMENT_TYPE_ID).TREATMENT_TYPE_CODE;
                    HisTreatment.HEIN_CARD_ADDRESS = patyAlter.ADDRESS;
                }

                MPS.ProcessorBase.PrintConfig.PreviewType previewType = MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow;

                var PrintServiceReqProcessor = new Library.PrintServiceReq.PrintServiceReqProcessor(HisServiceReqSDO, HisTreatment, null, currentModule != null ? currentModule.RoomId : 0, null, previewType);
                PrintServiceReqProcessor.Print("Mps000001", true);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
