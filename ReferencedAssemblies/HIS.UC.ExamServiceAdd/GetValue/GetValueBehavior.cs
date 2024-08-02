using HIS.UC.ExamServiceAdd.Run;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamServiceAdd.GetValue
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
                return ((UCExamServiceAdd)this.control).GetValue();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }
    }
}
