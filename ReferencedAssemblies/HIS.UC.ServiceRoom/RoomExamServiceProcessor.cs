using HIS.UC.ServiceRoom.Dispose;
using HIS.UC.ServiceRoom.FocusAndShow;
using HIS.UC.ServiceRoom.FocusService;
using HIS.UC.ServiceRoom.GetDetailSDO;
using HIS.UC.ServiceRoom.Run;
using HIS.UC.ServiceRoom.SetPatientClassifyId;
using HIS.UC.ServiceRoom.SetValueByPatient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ServiceRoom
{
    public partial class RoomExamServiceProcessor
    {
        public RoomExamServiceProcessor() { }
        public object Run(object data)
        {
            object result = null;
            try
            {
                IRun behavior = RunBehaviorFactory.MakeIRoomExamService(data);
                result = behavior != null ? (behavior.Run()) : null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public object GetDetailSDO(object uc)
        {
            object result = null;
            try
            {
                IGetDetailSDO behavior = GetDetailSDOBehaviorFactory.MakeIGetDetailSDO(uc);
                result = behavior != null ? behavior.Run() : null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public void FocusAndShow(object uc)
        {
            try
            {
                IFocusAndShow behavior = FocusAndShowBehaviorFactory.MakeIFocusAndShow(uc);
                if (behavior != null) behavior.Run();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void FocusService(object uc)
        {
            try
            {
                IFocusService behavior = FocusServiceBehaviorFactory.MakeIFocusService(uc);
                if (behavior != null) behavior.Run();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void SetValueByPatient(object uc, object patient)
        {
            try
            {
                ISetValueByPatient behavior = SetValueByPatientBehaviorFactory.MakeISetValueByPatient(uc, patient);
                if (behavior != null) behavior.Run();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        public void SetPatientClassifyId(object uc, object patientClassifyId)
        {
            try
            {
                ISetPatientClassifyId behavior = SetPatientClassifyIdFactory.MakeISetPatientClassifyId(uc, patientClassifyId);
                if (behavior != null) behavior.Run();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        public void DisposeControl(object uc)
        {
            try
            {
                IDispose behavior = DisposeFactory.MakeIDispose((UserControl)uc);
                if (behavior != null) behavior.Run();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void InitComboRoom(object uc, List<MOS.EFMODEL.DataModels.L_HIS_ROOM_COUNTER> executeRooms)
        {
            try
            {
                ((UCRoomExamService)uc).InitComboRoom(executeRooms);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void InitComboRoom(object uc, List<MOS.EFMODEL.DataModels.L_HIS_ROOM_COUNTER> executeRooms, bool isSync)
        {
            try
            {
                ((UCRoomExamService)uc).InitComboRoom(executeRooms, isSync);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void InitComboRoom(object uc, List<MOS.EFMODEL.DataModels.V_HIS_EXECUTE_ROOM_1> executeRooms)
        {
            try
            {
                ((UCRoomExamService)uc).InitComboRoom(executeRooms);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
