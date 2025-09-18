using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bartender.PrintBloodServiceReq
{
    public class PrintBloodServiceReq
    {
        private static Seagull.BarTender.Print.Engine btEngine;
        public string StartPrintBloodServiceReq(string strData)
        { 
            string result = null;
            try
            {
                string pathTestServiceReq = Directory.GetCurrentDirectory()+@"/Tmp/TempBartend/Mps000424/";
                Seagull.BarTender.Print.LabelFormatDocument btFormat;
                    // Run BarTender
                if (btEngine == null)
                {
                    btEngine = new Seagull.BarTender.Print.Engine();
                    btEngine.Start();
                    // Show BarTender UI for debugging purpose (Set "None" to hide)
                    btEngine.Window.VisibleWindows = Seagull.BarTender.Print.VisibleWindows.None;
                }
                   
                btFormat = btEngine.Documents.Open(pathTestServiceReq + "Mps000424.btw");

                try
                {
                    strData = "EXP_MEST_CODE,TDL_PATIENT_NAME,TDL_PATIENT_GENDER_NAME,TDL_PATIENT_DOB,NAMSINH,REQUEST_DEPARTMENT_NAME,REQUEST_ROOM_NAME,INTRUCTION_TIME,TREATMENT_CODE\n" + strData;
                    System.IO.File.WriteAllText(pathTestServiceReq + "Mps000424.txt", strData, Encoding.UTF8);

                    // Assign the text database to curtent format's primary database
                    Seagull.BarTender.Print.Database.TextFile tf = new Seagull.BarTender.Print.Database.TextFile(btFormat.DatabaseConnections[0].Name);
                    tf.FileName = pathTestServiceReq + "Mps000424.txt";
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
    }
}
