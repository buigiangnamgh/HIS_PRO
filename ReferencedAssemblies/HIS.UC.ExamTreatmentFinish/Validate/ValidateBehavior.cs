using HIS.UC.ExamTreatmentFinish.Run;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamTreatmentFinish.Validate
{
    public sealed class ValidateBehavior : IValidate
    {
        UserControl control;
        bool IsNotCheckValidateIcdUC;
        public ValidateBehavior()
            : base()
        {
        }

        public ValidateBehavior(CommonParam param, UserControl uc, bool IsNotCheckValidateIcdUC)
            : base()
        {
            this.control = uc;
            this.IsNotCheckValidateIcdUC = IsNotCheckValidateIcdUC;
        }

        bool IValidate.Run()
        {
            try
            {
                return ((UCExamTreatmentFinish)this.control).ValidateControl(IsNotCheckValidateIcdUC);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
        }
    }
}
