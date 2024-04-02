using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;

namespace HIS.Desktop.Plugins.ExamServiceReqExecute.Validate.ValidateRule
{
    internal class WeightValidaionRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
    {
        internal DevExpress.XtraEditors.SpinEdit spn;
        internal bool IsRequired;
        internal long IsRequiredWeightOption;
        public override bool Validate(Control control, object value)
        {
            bool valid = false;
            try
            {
                if (spn == null) return valid;
                if (IsRequired && spn.EditValue == null)
                {
                    if (this.IsRequiredWeightOption == 1)
                    {
                        this.ErrorText = "Bạn cần nhập cân nặng cho trẻ dưới 12 tháng tuổi";
                    }
                    else if (this.IsRequiredWeightOption == 2)
                    {
                        this.ErrorText = "Bạn cần nhập cân nặng cho trẻ dưới 72 tháng tuổi";
                    }
                    else
                    {
                        this.ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.HeThongTBTruongDuLieuBatBuocPhaiNhap);
                    }
                    this.ErrorType = ErrorType.Warning;
                    return valid;
                }
                if (spn.EditValue != null && spn.Value <= 0)
                {

                    this.ErrorText = "Cần nhập giá trị lớn hơn 0";
                    this.ErrorType = ErrorType.Warning;
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
