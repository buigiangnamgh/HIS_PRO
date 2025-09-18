using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Desktop.LocalStorage.BackendData;
using MOS.EFMODEL.DataModels;

namespace Bartender.PrintTestServiceReq
{
    public class PrintTestServiceReq
    {
        private static Seagull.BarTender.Print.Engine btEngine;
        public string StartPrintTestServiceReq(string strData)
        { 
            string result = null;
            try
            {
                string pathTestServiceReq = Directory.GetCurrentDirectory()+@"/Tmp/TempBartend/Mps000423/";
                Seagull.BarTender.Print.LabelFormatDocument btFormat;
                    // Run BarTender
                if (btEngine == null)
                {
                    btEngine = new Seagull.BarTender.Print.Engine();
                    btEngine.Start();
                    // Show BarTender UI for debugging purpose (Set "None" to hide)
                    btEngine.Window.VisibleWindows = Seagull.BarTender.Print.VisibleWindows.None;
                }
                   
                btFormat = btEngine.Documents.Open(pathTestServiceReq + "Mps000423.btw");

                try
                {
                    strData = "BARCODE,TDL_PATIENT_NAME,TDL_PATIENT_GENDER_NAME,TDL_PATIENT_DOB,NAMSINH,REQUEST_DEPARTMENT_NAME,REQUEST_ROOM_NAME,INTRUCTION_TIME,TREATMENT_CODE,TEST_SAMPLE_TYPE_NAME,PARENT_SERVICE_NAME,EXECUTE_ROOM_CODE\n" + strData;
                    System.IO.File.WriteAllText(pathTestServiceReq + "Mps000423.txt", strData, Encoding.UTF8);

                    // Assign the text database to curtent format's primary database
                    Seagull.BarTender.Print.Database.TextFile tf = new Seagull.BarTender.Print.Database.TextFile(btFormat.DatabaseConnections[0].Name);
                    tf.FileName = pathTestServiceReq + "Mps000423.txt";
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
                //File.Delete(pathTestServiceReq + "Mps000423.txt");
            }
            catch (Exception ex)
            {
                result = "[ERROR]:"+ex.Message;
            }
            return result;
        }

        private void GenText(HIS_SERVICE_REQ serviceReq, HIS_SERE_SERV sereServ, ref string txt)
        {
            var service = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault(o => o.ID == sereServ.SERVICE_ID);
            HIS_SERVICE servicePr = null;
            if (service != null && service.PARENT_ID.HasValue && service.PARENT_ID.Value > 0)
            {
                servicePr = BackendDataWorker.Get<HIS_SERVICE>().FirstOrDefault(o => o.ID == service.PARENT_ID);
            }
            HIS_DEPARTMENT department = BackendDataWorker.Get<HIS_DEPARTMENT>().FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID);
            V_HIS_ROOM room = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault(o => o.ID == serviceReq.REQUEST_ROOM_ID);
            HIS_EXECUTE_ROOM executeRoom = BackendDataWorker.Get<HIS_EXECUTE_ROOM>().FirstOrDefault(o => o.ROOM_ID == serviceReq.EXECUTE_ROOM_ID);
            txt += serviceReq.BARCODE;
            txt += "," + serviceReq.TDL_PATIENT_NAME;
            txt += "," + serviceReq.TDL_PATIENT_GENDER_NAME;
            txt += "," + serviceReq.TDL_PATIENT_DOB;
            txt += "," + (serviceReq.TDL_PATIENT_DOB > 10000000000000 ? serviceReq.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "");
            txt += "," + department.DEPARTMENT_NAME;
            txt += "," + room.ROOM_NAME;
            txt += "," + Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(Inventec.Common.DateTime.Get.Now() ?? 0);
            txt += "," + serviceReq.TDL_TREATMENT_CODE;

            if (serviceReq.TEST_SAMPLE_TYPE_ID.HasValue && serviceReq.TEST_SAMPLE_TYPE_ID.Value > 0)
            {
                var testSampleType = BackendDataWorker.Get<HIS_TEST_SAMPLE_TYPE>().FirstOrDefault(o => o.ID == serviceReq.TEST_SAMPLE_TYPE_ID);
                if (testSampleType != null)
                {
                    txt += "," + testSampleType.TEST_SAMPLE_TYPE_NAME.Replace(",", ";");
                }
            }
            else
            {
                txt += ",";
            }

            if (servicePr != null)
            {
                txt += "," + servicePr.SERVICE_NAME;
            }
            else
            {
                txt += ",";
            }
            txt += "," + executeRoom.EXECUTE_ROOM_CODE;

            // xuong dong (1 row trong db)
            txt += "\n";
        }
    }
}
