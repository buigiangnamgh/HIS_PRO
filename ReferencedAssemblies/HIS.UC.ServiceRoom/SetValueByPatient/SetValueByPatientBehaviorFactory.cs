using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.SetValueByPatient
{
    class SetValueByPatientBehaviorFactory
    {
        internal static ISetValueByPatient MakeISetValueByPatient(object data, object value)
        {
            ISetValueByPatient result = null;
            try
            {
                if (data is UCRoomExamService && value is V_HIS_PATIENT)
                {
                    result = new SetValueByPatientBehavior((UCRoomExamService)data, (V_HIS_PATIENT)value);
                }
                if (result == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data), ex);
                result = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
