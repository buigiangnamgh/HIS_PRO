using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bartender.PrintTemBlood
{
    public class PrintTemBlood
    {
        private static Seagull.BarTender.Print.Engine btEngine;
        public PrintTemBlood() { } 
        public string StartPrintTemBlood(string strData)
        { 
            string result = null;
            try
            {
                string pathTempBlood = Directory.GetCurrentDirectory()+@"/Tmp/TempBartend/Mps000422/";
                Seagull.BarTender.Print.LabelFormatDocument btFormat;
                    // Run BarTender
                if (btEngine == null)
                {
                    btEngine = new Seagull.BarTender.Print.Engine();
                    btEngine.Start();
                    // Show BarTender UI for debugging purpose (Set "None" to hide)
                    btEngine.Window.VisibleWindows = Seagull.BarTender.Print.VisibleWindows.None;
                }
                   
                btFormat = btEngine.Documents.Open(pathTempBlood + "Mps000422.btw");

                try
                {
                    strData = "TDL_PATIENT_CODE,TDL_PATIENT_DOB,NAMSINH,TDL_PATIENT_GENDER_NAME,TDL_PATIENT_NAME,BLOOD_ABO_CODE,BLOOD_HR_CODE,REQUEST_DEPARTMENT_NAME,TDL_TREATMENT_CODE,BLOOD_TYPE_NAME,AMOUNT,TUI_THU\n" + strData;
                    System.IO.File.WriteAllText(pathTempBlood + "Mps000422.txt", strData, Encoding.UTF8);

                    // Assign the text database to curtent format's primary database
                    Seagull.BarTender.Print.Database.TextFile tf = new Seagull.BarTender.Print.Database.TextFile(btFormat.DatabaseConnections[0].Name);
                    tf.FileName = pathTempBlood + "Mps000422.txt";
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
                File.Delete(pathTempBlood + "Mps000422.txt");
            }
            catch (Exception ex)
            {
                result = "[ERROR]:"+ex.Message;
            }
            return result;
        }
    }
}
