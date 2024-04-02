using HIS.UC.ExamTreatmentFinish.Run;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamTreatmentFinish.GetValue
{
    public sealed class GetValueBehavior : IGetValue
    {
        UserControl control;
        public GetValueBehavior()
            : base()
        {
        }

        public GetValueBehavior(CommonParam param, UserControl uc)
            : base()
        {
            this.control = uc;
        }

        object IGetValue.Run()
        {
            try
            {
                return ((UCExamTreatmentFinish)this.control).GetValue();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }
    }
}
