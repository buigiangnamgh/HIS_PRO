using HIS.UC.ExamTreatmentFinish.Run;
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
    public sealed class SetValueV2Behavior : ISetValueV2
    {
        UserControl control;
        MOS.SDO.HisServiceReqExamUpdateResultSDO entity;
        Inventec.Desktop.Common.Modules.Module currentModule;
        public SetValueV2Behavior()
            : base()
        {
        }

        public SetValueV2Behavior(CommonParam param, UserControl uc, Inventec.Desktop.Common.Modules.Module currentModule, MOS.SDO.HisServiceReqExamUpdateResultSDO data)
            : base()
        {
            this.control = uc;
            this.entity = data;
            this.currentModule = currentModule;
        }

        void ISetValueV2.Run()
        {
            try
            {
                ((UCExamTreatmentFinish)this.control).SetValueV2(currentModule, entity, this.currentModule.RoomId);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
