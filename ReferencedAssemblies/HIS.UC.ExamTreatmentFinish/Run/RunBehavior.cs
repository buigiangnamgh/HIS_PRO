
using HIS.UC.ExamTreatmentFinish.ADO;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamTreatmentFinish.Run
{
    public sealed class RunBehavior : IRun
    {
        TreatmentFinishInitADO entity;
        public RunBehavior()
            : base()
        {
        }

        public RunBehavior(CommonParam param, TreatmentFinishInitADO data)
            : base()
        {
            this.entity = data;
        }

        object IRun.Run()
        {
            try
            {
                return new UCExamTreatmentFinish(entity);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }
    }
}
