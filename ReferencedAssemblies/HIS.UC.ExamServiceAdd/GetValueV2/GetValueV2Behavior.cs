using HIS.UC.ExamServiceAdd.Run;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamServiceAdd.GetValueV2
{
    public sealed class GetValueV2Behavior : IGetValueV2
    {
        UserControl control;
        public GetValueV2Behavior()
            : base()
        {
        }

        public GetValueV2Behavior(CommonParam param, UserControl uc)
            : base()
        {
            this.control = uc;
        }

        object IGetValueV2.Run()
        {
            try
            {
                return ((UCExamServiceAdd)this.control).GetValueV2();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }
    }
}
