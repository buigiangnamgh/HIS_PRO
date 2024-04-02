using HIS.UC.ExamTreatmentFinish.ADO;
using HIS.UC.ExamTreatmentFinish.Run;
//using HIS.UC.Hospitalize.ADO;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamTreatmentFinish.Reload
{
    class ReloadBehavior : IReload
    {
        UserControl control;
        TreatmentFinishInitADO entity;
        public ReloadBehavior()
            : base()
        {
        }

        public ReloadBehavior(CommonParam param, UserControl uc, TreatmentFinishInitADO data)
            : base()
        {
            this.control = uc;
            this.entity = data;
        }

        void IReload.Run()
        {
            try
            {
                ((UCExamTreatmentFinish)this.control).Reload(entity);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
