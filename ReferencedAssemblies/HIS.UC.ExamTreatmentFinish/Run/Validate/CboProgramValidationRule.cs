using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamTreatmentFinish.Run.Validate
{
    class CboProgramValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
    {
        internal DevExpress.XtraEditors.CheckEdit chkDataStore;
        internal DevExpress.XtraEditors.GridLookUpEdit cboProgram;
        public override bool Validate(System.Windows.Forms.Control control, object value)
        {
            bool valid = false;
            try
            {
                if (cboProgram == null || chkDataStore == null) return valid;

                if (chkDataStore.Enabled)
                {
                    if (chkDataStore.CheckState == System.Windows.Forms.CheckState.Checked && cboProgram.EditValue == null)
                    {
                        this.ErrorText = "Bắt buộc chọn chương trình";
                        return valid;
                    }
                }
                valid = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return valid;
        }
    }
}
