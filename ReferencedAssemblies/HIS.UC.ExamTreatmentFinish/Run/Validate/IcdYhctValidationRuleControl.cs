using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ExamTreatmentFinish.Run.Validate
{
    class IcdYhctValidationRuleControl : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
    {
        internal DevExpress.XtraEditors.TextEdit txtIcdYhctCode;
        internal DevExpress.XtraEditors.TextEdit txtYhctMainText;
        internal int? maxLengthCode;
        internal int? maxLengthText;
        //internal bool IsObligatoryTranferMediOrg;
        internal bool IsRequired = true;
        internal List<HIS_ICD> icdYhcts;
        public override bool Validate(Control control, object value)
        {
            bool valid = false;
            try
            {
                if (txtIcdYhctCode == null
                    || txtYhctMainText == null)
                    return valid;

                if (maxLengthCode != null)
                {
                    if (Inventec.Common.String.CheckString.IsOverMaxLengthUTF8(txtIcdYhctCode.Text.Trim(), maxLengthCode ?? 0))
                    {
                        this.ErrorText = "Mã bệnh Yhct vượt quá ký tự cho phép";
                        return valid;
                    }
                }

                if (IsRequired && String.IsNullOrEmpty(txtIcdYhctCode.Text))
                {
                    this.ErrorText = "Bạn nhập mã chẩn đoán không đúng. Vui lòng kiểm tra lại";
                    return valid;
                }

                if (!String.IsNullOrEmpty(txtIcdYhctCode.Text) && !icdYhcts.Exists(o => o.ICD_CODE.Equals(txtIcdYhctCode.Text.Trim())))
                {
                    this.ErrorText = "Bạn nhập mã chẩn đoán không đúng. Vui lòng kiểm tra lại";
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
