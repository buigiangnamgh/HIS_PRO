using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Desktop.LocalStorage.BackendData;
using MOS.EFMODEL.DataModels;
using Seagull.BarTender.Print;

namespace Bartender.PrintGpblServiceReq
{
    public class PrintGpblServiceReq
    {
        private static Seagull.BarTender.Print.Engine btEngine;
        public string StartPrintGpblServiceReq(string strData)
        {
            string result = null;
            try
            {
                string pathTestServiceReq = Directory.GetCurrentDirectory() + @"/Tmp/TempBartend/Mps000425/";
                Seagull.BarTender.Print.LabelFormatDocument btFormat;
                // Run BarTender
                if (btEngine == null)
                {
                    btEngine = new Seagull.BarTender.Print.Engine();
                    btEngine.Start();
                    // Show BarTender UI for debugging purpose (Set "None" to hide)
                    btEngine.Window.VisibleWindows = Seagull.BarTender.Print.VisibleWindows.None;
                }

                btFormat = btEngine.Documents.Open(pathTestServiceReq + "Mps000425.btw");

                try
                {
                    strData = "SERVICE_REQ_CODE,TDL_PATIENT_NAME,TDL_PATIENT_GENDER_NAME,TDL_PATIENT_DOB,NAMSINH,REQUEST_DEPARTMENT_NAME,REQUEST_USERNAME,INTRUCTION_TIME,TREATMENT_CODE,TEST_SAMPLE_TYPE_NAME,PARENT_SERVICE_NAME,EXECUTE_ROOM_CODE\n" + strData;
                    System.IO.File.WriteAllText(pathTestServiceReq + "Mps000425.txt", strData, Encoding.UTF8);

                    // Assign the text database to curtent format's primary database
                    Seagull.BarTender.Print.Database.TextFile tf = new Seagull.BarTender.Print.Database.TextFile(btFormat.DatabaseConnections[0].Name);
                    tf.FileName = pathTestServiceReq + "Mps000425.txt";
                    btFormat.DatabaseConnections.SetDatabaseConnection(tf);
                    btFormat.PrintSetup.ReloadTextDatabaseFields = true; // Fix when field order is different from design time
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }

                // Print
                btFormat.Print();
                btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);

                // Delete text database
                //File.Delete(pathTestServiceReq + "Mps000425.txt");
            }
            catch (Exception ex)
            {
                result = "[ERROR]:" + ex.Message;
            }
            return result;
        }

        public string GenText_Gpbl(List<HIS_SERVICE_REQ> serviceReqList, List<HIS_SERE_SERV> sereServList)
        {
            string result = null;
            try
            {
                foreach (var serviceReq in serviceReqList)
                {
                    var sereServ = sereServList.FirstOrDefault(o => o.SERVICE_REQ_ID == serviceReq.ID);
                    var service = sereServ != null ? BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault(o => o.ID == sereServ.SERVICE_ID) : null;
                    HIS_SERVICE servicePr = null;
                    if (service != null && service.PARENT_ID.HasValue && service.PARENT_ID.Value > 0)
                    {
                        servicePr = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault(o => o.ID == service.PARENT_ID);
                    }
                    var reqDepartment = BackendDataWorker.Get<HIS_DEPARTMENT>().FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID);
                    var executeRoom = BackendDataWorker.Get<HIS_EXECUTE_ROOM>().FirstOrDefault(o => o.ROOM_ID == serviceReq.EXECUTE_ROOM_ID);
                    var reqRoom = BackendDataWorker.Get<HIS_ROOM>().FirstOrDefault(o => o.ID == serviceReq.REQUEST_ROOM_ID);

                    Engine engine = new Engine(true); // bật BarTender engine
                    LabelFormatDocument format = engine.Documents.Open(Directory.GetCurrentDirectory() + @"/Tmp/TempBartend/Mps000423/Mps000423.btw");
                    // Gán dữ liệu động
                    format.SubStrings["SERVICE_REQ_CODE"].Value = serviceReq.SERVICE_REQ_CODE;
                    format.SubStrings["TDL_PATIENT_NAME"].Value = serviceReq.TDL_PATIENT_NAME;
                    format.SubStrings["TDL_PATIENT_GENDER_NAME"].Value = serviceReq.TDL_PATIENT_GENDER_NAME;
                    format.SubStrings["NAM_SINH"].Value = (serviceReq.TDL_PATIENT_DOB > 10000000000000 ? serviceReq.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "");
                    format.SubStrings["REQ_DEPARTMENT_NAME"].Value = reqDepartment.DEPARTMENT_NAME;
                    format.SubStrings["REQUEST_USERNAME"].Value = serviceReq.REQUEST_USERNAME;
                    format.SubStrings["INTRUCTION_TIME"].Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Inventec.Common.DateTime.Get.Now() ?? 0);
                    format.SubStrings["TDL_TREATMENT_CODE"].Value = serviceReq.TDL_TREATMENT_CODE;
                    format.SubStrings["TEST_SAMPLE_TYPE_NAME"].Value = "";
                    if (serviceReq.TEST_SAMPLE_TYPE_ID.HasValue && serviceReq.TEST_SAMPLE_TYPE_ID.Value > 0)
                    {
                        var testSampleType = BackendDataWorker.Get<HIS_TEST_SAMPLE_TYPE>().FirstOrDefault(o => o.ID == serviceReq.TEST_SAMPLE_TYPE_ID);
                        if (testSampleType != null)
                            format.SubStrings["TEST_SAMPLE_TYPE_NAME"].Value = testSampleType.TEST_SAMPLE_TYPE_NAME.Replace(",", ";");
                    }
                    if (servicePr != null)
                    {
                        format.SubStrings["PARENT_SERVICE_NAME"].Value = servicePr.SERVICE_NAME;
                    }

                    // In qua LAN
                    //format.PrintSetup.PrinterName = @"\\localhost\Microsoft Print to PDF";
                    format.Print();
                    result = "In thành công serviceReq " + serviceReq.SERVICE_REQ_CODE;
                    engine.Stop();
                }
            }
            catch (Exception ex)
            {
                result = "[ERROR]:" + ex.Message;
            }
            return result;
        }
    }
}
