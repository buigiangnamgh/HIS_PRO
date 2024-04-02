using HIS.UC.ExamTreatmentFinish.Run;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamTreatmentFinish.SetValue
{
    public sealed class SetValueBehavior : ISetValue
    {
        UserControl control;
        HIS_TREATMENT entity;
        Inventec.Desktop.Common.Modules.Module currentModule;
        public SetValueBehavior()
            : base()
        {
        }

        public SetValueBehavior(CommonParam param, UserControl uc,Inventec.Desktop.Common.Modules.Module currentModule, HIS_TREATMENT data)
            : base()
        {
            this.control = uc;
            this.entity = data;
            this.currentModule = currentModule;
        }

        void ISetValue.Run()
        {
            try
            {
                ((UCExamTreatmentFinish)this.control).SetValue(currentModule,entity);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
