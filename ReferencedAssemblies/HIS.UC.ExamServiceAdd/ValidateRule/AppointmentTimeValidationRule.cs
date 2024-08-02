using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.HisExamServiceAdd.ValidateRule
{
    class AppointmentTimeValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
    {
        internal DevExpress.XtraEditors.CheckEdit chkAppointment;
        internal DevExpress.XtraEditors.DateEdit dtAppointment;
        internal bool IsMustSetProgram;
        internal long? TreatmentTypeId;
        public override bool Validate(System.Windows.Forms.Control control, object value)
        {
            bool valid = false;
            try
            {
                if ((chkAppointment.Visible && chkAppointment.Enabled && chkAppointment.CheckState == System.Windows.Forms.CheckState.Checked)
                    && dtAppointment.EditValue == null)
                {
                    return valid;
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
