using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamTreatmentFinish.EndTypeForm.ValidForm
{
    class TranPatiTechValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
    {
        internal DevExpress.XtraEditors.TextEdit txtTranPatiTech;
        internal DevExpress.XtraEditors.GridLookUpEdit cboTranPatiTech;
        internal DevExpress.XtraLayout.LayoutControlItem layoutControlTranPatiTech;
        internal string TranPatiReason_code;

        public override bool Validate(Control control, object value)
        {
            bool valid = false;
            try
            {
                if (txtTranPatiTech == null || cboTranPatiTech == null) return valid;

                if (TranPatiReason_code == "01")
                {
                   
                    if (String.IsNullOrEmpty(txtTranPatiTech.Text) || cboTranPatiTech.EditValue == null)
                        return valid;
                }
                else
                {
                    txtTranPatiTech.Enabled = false;
                    cboTranPatiTech.Enabled = false;
                    layoutControlTranPatiTech.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
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
