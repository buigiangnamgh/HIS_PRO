using Inventec.Common.LocalStorage.SdaConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.TreatmentFinish.Config
{
    class TreatmentEndCFG
    {
        internal const string TREATMENT_END___APPOINTMENT_TIME_DEFAULT_KEY = "EXE.HIS_TREATMENT_END.APPOINTMENT_TIME_DEFAULT";
        internal const string PRESCRIPTION_TIME_AND_APPOINTMENT_TIME_KEY = "HIS.Desktop.Plugins.TreatmentFinish.APPOINTMENT_TIME";
        const string isPrescription = "1";

        internal static long treatmentEndAppointmentTimeDefault;
        internal static bool AppointmentTimeDefault;

        internal static void GetConfig()
        {
            try
            {
                treatmentEndAppointmentTimeDefault = HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<long>(TREATMENT_END___APPOINTMENT_TIME_DEFAULT_KEY);
                AppointmentTimeDefault = HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(PRESCRIPTION_TIME_AND_APPOINTMENT_TIME_KEY) == isPrescription;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //public static long TREATMENT_END___APPOINTMENT_TIME_DEFAULT
        //{
        //    get
        //    {
        //        if (treatmentEndAppointmentTimeDefault == null || treatmentEndAppointmentTimeDefault == 0)
        //        {
        //            treatmentEndAppointmentTimeDefault = Int64.Parse(GetValue(TREATMENT_END___APPOINTMENT_TIME_DEFAULT_KEY));
        //        }
        //        return treatmentEndAppointmentTimeDefault;
        //    }
        //    set
        //    {
        //        treatmentEndAppointmentTimeDefault = value;
        //    }
        //}

        //public static bool PRESCRIPTION_TIME_AND_APPOINTMENT_TIME
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (!AppointmentTimeDefault.HasValue)
        //            {
        //                AppointmentTimeDefault = GetData(SdaConfigs.Get<string>(PRESCRIPTION_TIME_AND_APPOINTMENT_TIME_KEY));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            AppointmentTimeDefault = false;
        //            Inventec.Common.Logging.LogSystem.Error(ex);
        //        }

        //        return AppointmentTimeDefault.Value;
        //    }
        //    set
        //    {
        //        AppointmentTimeDefault = value;
        //    }
        //}

        //private static string GetValue(string code)
        //{
        //    string result = null;
        //    try
        //    {
        //        SDA.EFMODEL.DataModels.SDA_CONFIG config = ConfigLoader.dictionaryConfig[code];
        //        if (config == null) throw new ArgumentNullException(code);
        //        result = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
        //        if (String.IsNullOrEmpty(result)) throw new ArgumentNullException(code);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = null;
        //    }
        //    return result;
        //}

        //private static bool GetData(string code)
        //{
        //    bool result = false;
        //    try
        //    {
        //        result = (isPrescription == code);
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = false;
        //    }
        //    return result;
        //}
    }
}
