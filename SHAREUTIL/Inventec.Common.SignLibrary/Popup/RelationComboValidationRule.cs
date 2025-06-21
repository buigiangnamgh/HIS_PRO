using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.Validation
{
    class RelationComboValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
    {
        internal DevExpress.XtraEditors.GridLookUpEdit cboRelation;

        public override bool Validate(System.Windows.Forms.Control control, object value)
        {
            bool valid = false;
            try
            {
                if (cboRelation == null) return valid;
                if (cboRelation.EditValue == null || TypeConvertParse.ToInt64(cboRelation.EditValue.ToString()) <= 0)
                {
                    ErrorText = MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe);
                    ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
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
