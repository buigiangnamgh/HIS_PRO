using HIS.UC.ExamTreatmentFinish.Run;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamTreatmentFinish.GetIcd
{
    public sealed class GetIcdBehavior : IGetIcd
    {
        UserControl control;
        public GetIcdBehavior()
            : base()
        {
        }

        public GetIcdBehavior(CommonParam param, UserControl uc)
            : base()
        {
            this.control = uc;
        }

        object IGetIcd.Run()
        {
            try
            {
                return ((UCExamTreatmentFinish)this.control).GetIcd();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }
    }
}
