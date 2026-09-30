using HIS.Desktop.LocalStorage.BackendData;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraReports.Serialization;
using DevExpress.XtraReports.UI;

namespace ThermalPrinter.PrintTestServiceReq
{
    public class ProcessPrint
    {
        public static void PrintBarCode(bool printToPrinter, List<V_HIS_SERVICE_REQ> serviceReqList, List<V_HIS_SERE_SERV> sereServList)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("private void PrintBarCode 1");
                DataTable dTableBarCode = new DataTable();
                DataColumn BarCode = new DataColumn("BARCODE");
                DataColumn PatientName = new DataColumn("TDL_PATIENT_NAME");
                DataColumn GenderName = new DataColumn("TDL_PATIENT_GENDER_NAME");
                DataColumn Dob = new DataColumn("TDL_PATIENT_DOB");
                DataColumn RequestDepartmentName = new DataColumn("REQUEST_DEPARTMENT_NAME");
                DataColumn RequestRoom = new DataColumn("REQUEST_ROOM_NAME");
                DataColumn CurrentDate = new DataColumn("CURRENT_DATE");
                DataColumn TreatmentCode = new DataColumn("TREATMENT_CODE");
                DataColumn TestSampleName = new DataColumn("TEST_SAMPLE_TYPE_NAME");
                DataColumn ServiceName = new DataColumn("PARENT_SERVICE_NAME");
                DataColumn ExecuteRoomCode = new DataColumn("EXECUTE_ROOM_CODE");
                DataColumn DobYear = new DataColumn("TDL_PATIENT_DOB_YEAR");
                dTableBarCode.Columns.Add(BarCode);
                dTableBarCode.Columns.Add(PatientName);
                dTableBarCode.Columns.Add(GenderName);
                dTableBarCode.Columns.Add(Dob);
                dTableBarCode.Columns.Add(RequestDepartmentName);
                dTableBarCode.Columns.Add(RequestRoom);
                dTableBarCode.Columns.Add(CurrentDate);
                dTableBarCode.Columns.Add(TreatmentCode);
                dTableBarCode.Columns.Add(TestSampleName);
                dTableBarCode.Columns.Add(ServiceName);
                dTableBarCode.Columns.Add(ExecuteRoomCode);
                dTableBarCode.Columns.Add(DobYear);
                int intBarcodeW = 143;
                int intBarcodeH = 70;
                //int intBarcodeH = 70;
                Image imgBarcode = null;
                System.IO.MemoryStream ms = null;
                BarcodeLib.Barcode Code128 = new BarcodeLib.Barcode();

                if (!dTableBarCode.Columns.Contains("BarCodeImage"))
                    dTableBarCode.Columns.Add("BarCodeImage", typeof(System.Byte[]));


                foreach (var serviceReq in serviceReqList)
                {
                    DataRow dRow = dTableBarCode.NewRow();

                    var ssByServiceReq = sereServList.FirstOrDefault(o => o.SERVICE_REQ_ID.HasValue && o.SERVICE_REQ_ID.Value == serviceReq.ID);

                    if (ssByServiceReq!=null)
                    {
                        HIS_SERVICE service = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault((HIS_SERVICE o) => o.ID == ssByServiceReq.SERVICE_ID);
                        HIS_SERVICE val = null;
                        if (service != null && service.PARENT_ID.HasValue && service.PARENT_ID.Value > 0)
                        {
                            val = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault((HIS_SERVICE o) => o.ID == service.PARENT_ID);
                        }

                        if (val != null)
                        {
                            dRow.SetField("PARENT_SERVICE_NAME", val.SERVICE_NAME);
                        }
                    }
                    
                    dRow.SetField("BARCODE", serviceReq.BARCODE);
                    dRow.SetField("TDL_PATIENT_NAME", serviceReq.TDL_PATIENT_NAME);
                    dRow.SetField("TDL_PATIENT_GENDER_NAME", serviceReq.TDL_PATIENT_GENDER_NAME);
                    dRow.SetField("TDL_PATIENT_DOB", serviceReq.TDL_PATIENT_DOB);
                    dRow.SetField("TDL_PATIENT_DOB_YEAR", ((serviceReq.TDL_PATIENT_DOB > 10000000000000L) ? serviceReq.TDL_PATIENT_DOB.ToString().Substring(0, 4) : ""));
                    dRow.SetField("REQUEST_DEPARTMENT_NAME", serviceReq.REQUEST_DEPARTMENT_NAME);
                    dRow.SetField("REQUEST_ROOM_NAME", serviceReq.REQUEST_ROOM_NAME);
                    dRow.SetField("CURRENT_DATE", Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Inventec.Common.DateTime.Get.Now() ?? 0));
                    dRow.SetField("TREATMENT_CODE", serviceReq.TREATMENT_CODE);
                    if (serviceReq.TEST_SAMPLE_TYPE_ID.HasValue && serviceReq.TEST_SAMPLE_TYPE_ID.Value > 0)
                    {
                        HIS_TEST_SAMPLE_TYPE val2 = BackendDataWorker.Get<HIS_TEST_SAMPLE_TYPE>().FirstOrDefault((HIS_TEST_SAMPLE_TYPE o) => o.ID == serviceReq.TEST_SAMPLE_TYPE_ID);
                        if (val2 != null)
                        {
                            dRow.SetField("TEST_SAMPLE_TYPE_NAME", val2.TEST_SAMPLE_TYPE_NAME.Replace(",", ";"));
                        }
                    }
                   
                    Inventec.Common.Logging.LogSystem.Debug("private void PrintBarCode 2");
                    dRow.SetField("EXECUTE_ROOM_CODE", serviceReq.EXECUTE_ROOM_CODE);
                    Code128.IncludeLabel = true;
                    imgBarcode = Code128.Encode(BarcodeLib.TYPE.CODE128C, dRow["BARCODE"].ToString());

                    ms = new System.IO.MemoryStream();
                    imgBarcode.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    dRow.SetField("BarCodeImage", ms.GetBuffer());
                    ms.Flush();
                    ms.Close();
                    Inventec.Common.Logging.LogSystem.Debug("private void PrintBarCode 3");
                    dTableBarCode.Rows.Add(dRow);
                }

               
                DevExpress.XtraReports.UI.XtraReport report = new DevExpress.XtraReports.UI.XtraReport();
                string path = Environment.CurrentDirectory;
                Inventec.Common.Logging.LogSystem.Debug("rptBarcode.repx path" + path + @"\Tmp\TempBartend\XetNghiem\rptBarcode.repx");
                report.LoadLayout(path + @"\Tmp\TempBartend\XetNghiem\rptBarcode.repx");
                report.DataSource = dTableBarCode;
                Inventec.Common.Logging.LogSystem.Debug("private void PrintBarCode 4");
                try
                {
                    string PrintBarCodeName = ";";
                    PrintDocument printDocument = new PrintDocument();
                    foreach (string text2 in PrinterSettings.InstalledPrinters)
                    {
                        bool flag3 = text2.Contains(PrintBarCodeName);
                        if (flag3)
                        {
                            report.PrinterName = text2;
                            break;
                        }
                    }
                }
                catch
                {
                }
                Inventec.Common.Logging.LogSystem.Debug("private void PrintBarCode 5");
                report.CreateDocument();

                Inventec.Common.Logging.LogSystem.Debug("private void PrintBarCode 6");
                if (!printToPrinter)
                {
                    report.ShowPreview();
                }
                else
                {
                    report.Print();
                }
                Inventec.Common.Logging.LogSystem.Debug("private void PrintBarCode 7");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

        }
    }
}
