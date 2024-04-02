using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamTreatmentFinish.SetValueV2
{
    class SetValueV2Factory
    {
        internal static ISetValueV2 MakeISetValue(CommonParam param, UserControl uc, Inventec.Desktop.Common.Modules.Module currentModule, MOS.SDO.HisServiceReqExamUpdateResultSDO treatment)
        {
            ISetValueV2 result = null;
            try
            {
                if (uc is UserControl)
                {
                    result = new SetValueV2Behavior(param, (UserControl)uc,currentModule, treatment);
                }
                if (result == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Factory khong khoi tao duoc doi tuong." + uc.GetType().ToString() + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => uc), uc), ex);
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
