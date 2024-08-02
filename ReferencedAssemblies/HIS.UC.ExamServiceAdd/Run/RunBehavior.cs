
using HIS.UC.ExamServiceAdd.ADO;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamServiceAdd.Run
{
    public sealed class RunBehavior : IRun
    {
        ExamServiceAddInitADO entity;
        public RunBehavior()
            : base()
        {
        }

        public RunBehavior(CommonParam param, ExamServiceAddInitADO data)
            : base()
        {
            this.entity = data;
        }

        object IRun.Run()
        {
            try
            {
                return new UCExamServiceAdd(entity);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }
    }
}
