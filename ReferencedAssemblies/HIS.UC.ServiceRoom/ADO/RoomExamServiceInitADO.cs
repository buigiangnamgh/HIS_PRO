using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Desktop.DelegateRegister;

namespace HIS.UC.ServiceRoom.ADO
{
    public enum TemplateDesign
    {
        T11,
        T20
    }

    public class RoomExamServiceInitADO
    {
        public V_HIS_PATIENT_TYPE_ALTER CurrentPatientTypeAlter { get; set; }
        public List<HIS_PATIENT_TYPE> CurrentPatientTypes { get; set; }
        public List<V_HIS_SERVICE_ROOM> HisServiceRooms { get; set; }
        public List<V_HIS_EXECUTE_ROOM_1> HisExecuteRooms { get; set; }
        public List<L_HIS_ROOM_COUNTER> LHisRoomCounters { get; set; }

        public string LciRoomName { get; set; }
        public string LciExamServiceName { get; set; }
        public string UcName { get; set; }
        public bool IsInit { get; set; }
        public bool IsFocusCombo { get; set; }
        public string UserControlItemName { get; set; }
        public V_HIS_SERE_SERV SereServExam { get; set; }
        public CultureInfo CurrentCulture { get; set; }

        public RemoveRoomExamService RemoveUC { get; set; }
        public DelegateFocusNextUserControl FocusOutUC;
        public TemplateDesign TemplateDesign { get; set; }
        public Action RegisterPatientWithRightRouteBHYT { get; set; }
        public Action ChangeRoomNotEmergency { get; set; }
        public Action<long> ChangeServiceProcessPrimaryPatientType { get; set; }
        public DelegateGetIntructionTime GetIntructionTime { get; set; }
        public MOS.SDO.HisPatientSDO patientSDO { get; set; }
        public long? PatientClassifyId { get; set; }
        public Action<bool> ChangeDisablePrimaryPatientType { get; set; }
        public RoomExamServiceInitADO() { }

        public RoomExamServiceInitADO(List<V_HIS_EXECUTE_ROOM_1> executeRooms, List<V_HIS_SERVICE_ROOM> serviceRooms)
        {
            this.HisExecuteRooms = executeRooms;
            this.HisServiceRooms = serviceRooms;
        }

        public RoomExamServiceInitADO(List<L_HIS_ROOM_COUNTER> executeRooms, List<V_HIS_SERVICE_ROOM> serviceRooms)
        {
            this.LHisRoomCounters = executeRooms;
            this.HisServiceRooms = serviceRooms;
        }
    }
}
