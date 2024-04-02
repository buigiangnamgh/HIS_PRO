using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HIS.UC.ExamTreatmentFinish.Run;

namespace HIS.UC.ExamTreatmentFinish.FocusControl
{
    class FocusControlBehavior : IFocusControl
    {
        UserControl control;
        internal FocusControlBehavior()
            : base()
        { }

        internal FocusControlBehavior(CommonParam param, UserControl uc)
            : base()
        {
            this.control = uc;
        }

        void IFocusControl.Run()
        {
            try
            {
                ((UCExamTreatmentFinish)this.control).FocusControl();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
