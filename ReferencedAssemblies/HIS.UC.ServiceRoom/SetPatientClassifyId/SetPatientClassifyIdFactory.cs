using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.SetPatientClassifyId
{
    class SetPatientClassifyIdFactory
    {
        internal static ISetPatientClassifyId MakeISetPatientClassifyId(object data, object value)
        {
            ISetPatientClassifyId result = null;
            try
            {
                if (data is UCRoomExamService)
                {
                    result = new SetPatientClassifyIdBehavior((UCRoomExamService)data, (long?)value);
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
