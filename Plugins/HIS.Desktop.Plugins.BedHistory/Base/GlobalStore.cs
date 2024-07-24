using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.BedHistory.Base
{
    class GlobalStore
    {
        public const string HIS_BED_LOG_GETVIEW = "api/HisBedLog/GetView";
        public const string HIS_BED_LOG_UPDATE = "api/HisBedLog/Update";
        public const string HIS_BED_LOG_DELETE = "/api/HisBedLog/Delete";
        public const string HIS_BED_SERVICE_REQ_CREATE = "api/HisBedServiceReq/Create";
        public const string HIS_BED_SERVICE_REQ_GETVIEW = "api/HisBedServiceReq/GetView";
        public const string HIS_BED_SERVICE_TYPE_GETVIEW = "api/HisBedServiceType/GetView";
        public const string HIS_BED_BSTY_GETVIEW = "/api/HisBedBsty/GetView";
        public const string HIS_SERVICE_REQ_GETVIEW = "api/HisServiceReq/GetView";
        public const string HIS_SERE_SERV_GETVIEW = "api/HisSereServ/GetView";
        public const string HIS_SERE_SERV_PTTT_GETVIEW = "api/HisSereServPttt/GetView";
        public const string HIS_PTTT_GROUP_GET = "/api/HisPtttGroup/Get";
        public const string HIS_BED_LOG_CREATE = "/api/HisBedLog/Create";
        public const string HIS_SERVICE_GETVIEW = "api/HisService/GetView";
        public const string HIS_SERE_SERV_GET = "api/HisSereServ/Get";

        public const string HIS_SERVICE_REQ_GET = "api/HisServiceReq/Get";

        public const string EXE_CREATE_BED_LOG_DEPARTMENT_CODES = "EXE.CREATE_BED_LOG.DEPARTMENT_CODES";//Cau hinh khoa load giuong tu dong ccc

        public const string SHOW_PRIMARY_PATIENT_TYPE = "MOS.HIS_SERE_SERV.IS_SET_PRIMARY_PATIENT_TYPE";
        public const string REQ_USER_MUST_HAVE_DIPLOMA = "MOS.HIS_SERVICE_REQ.REQ_USER_MUST_HAVE_DIPLOMA";
        public const string SHOW_REQUEST_USER = "HIS.Desktop.Plugins.AssignConfig.ShowRequestUser";
        /*
         INSERT INTO HIS_CONFIG (KEY, DEFAULT_VALUE, DESCRIPTION,CONFIG_CODE, MODULE_LINKS) 
SELECT 'HIS.Desktop.Plugins.BedHistory.OptionPreventChooseMultiBedLog', '', 
'Tùy chọn cho phép chọn nhiều lịch sử giường cùng lúc'|| CHR(13) || CHR(10) ||
'- 1: Chặn ' || CHR(13) || CHR(10) ||
'- khác 1: cho phép', 'XXXXX',  'HIS.Desktop.Plugins.BedHistory'
FROM DUAL 
WHERE NOT EXISTS (SELECT 1 FROM HIS_CONFIG WHERE KEY = 'HIS.Desktop.Plugins.BedHistory.OptionPreventChooseMultiBedLog');
commit;

         */
        public const string OptionPreventChooseMultiBedLog = "HIS.Desktop.Plugins.BedHistory.OptionPreventChooseMultiBedLog";

        public static string IsPrimaryPatientType
        {
            get
            {
                return HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(SHOW_PRIMARY_PATIENT_TYPE);
            }
        }
        public static string RequserMustHaveDiploma
        {
            get
            {
                return HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(REQ_USER_MUST_HAVE_DIPLOMA);
            }
        }
        public static string ShowRequestUser
        {
            get
            {
                return HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(SHOW_REQUEST_USER);
            }
        }
        public static string PreventChooseMultiBedLog
        {
            get
            {
                return HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(OptionPreventChooseMultiBedLog);
            }
        }

        public static List<MOS.EFMODEL.DataModels.V_HIS_PATIENT_TYPE_ALLOW> HisVPatientTypeAllows
        {
            get
            {
                return HIS.Desktop.LocalStorage.BackendData.BackendDataWorker.Get
                        <MOS.EFMODEL.DataModels.V_HIS_PATIENT_TYPE_ALLOW>().OrderByDescending(o => o.CREATE_TIME).ToList();
            }
        }

        public static List<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE> HisPatientTypes
        {
            get
            {
                return HIS.Desktop.LocalStorage.BackendData.BackendDataWorker.Get
                        <MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE>().OrderByDescending(o => o.CREATE_TIME).ToList();
            }
        }

        public static List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_PATY> HisVServicePatys
        {
            get
            {
                return HIS.Desktop.LocalStorage.BackendData.BackendDataWorker.Get
                        <MOS.EFMODEL.DataModels.V_HIS_SERVICE_PATY>().OrderByDescending(o => o.CREATE_TIME).ToList();
            }
        }

        public static long PatientTypeId__BHYT
        {
            get
            {
                var patientType = HIS.Desktop.LocalStorage.BackendData.BackendDataWorker.Get<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE>().FirstOrDefault(o => o.PATIENT_TYPE_CODE == HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(HisConfigKeys.HIS_CONFIG__PATIENT_TYPE_CODE__BHYT));
                if (patientType != null)
                {
                    return patientType.ID;
                }
                else
                    return -1;
            }
        }

        public static long PatientTypeId__VP
        {
            get
            {
                var patientType = HIS.Desktop.LocalStorage.BackendData.BackendDataWorker.Get<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE>().FirstOrDefault(o => o.PATIENT_TYPE_CODE == HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(HisConfigKeys.HIS_CONFIG__PATIENT_TYPE_CODE__VP));
                if (patientType != null)
                {
                    return patientType.ID;
                }
                else
                    return -1;
            }
        }
    }
}
