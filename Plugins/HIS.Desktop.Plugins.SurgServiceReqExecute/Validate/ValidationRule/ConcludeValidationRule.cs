using HIS.Desktop.Plugins.SurgServiceReqExecute.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.Desktop.Plugins.SurgServiceReqExecute.Validate.ValidationRule
{
    class ConcludeValidationRule :
DevExpress.XtraEditors.DXErrorProvider.ValidationRule
    {
        internal DevExpress.XtraEditors.MemoEdit conclude;
        internal bool requiedConfig;
        internal bool is_CDHA_Type_Bhyt;

        public override bool Validate(Control control, object value)
        {
            bool valid = true;
            try
            {
                if (conclude == null)
                {
                    valid = false;
                }
                Inventec.Common.Logging.LogSystem.Info("is_CDHA_Type_Bhyt " + is_CDHA_Type_Bhyt + " requiedConfig " + requiedConfig + "conclude.Text " + conclude.Text);
                if (is_CDHA_Type_Bhyt && requiedConfig && String.IsNullOrEmpty(conclude.Text))
                {
                    valid = false;
                    this.ErrorText = "Bắt buộc nhập kết luận với loại dịch vụ BHYT là CĐHA";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return valid;
        }
    }
}
