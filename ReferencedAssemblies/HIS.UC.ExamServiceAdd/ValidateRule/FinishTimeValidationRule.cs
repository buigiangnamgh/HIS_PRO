using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamServiceAdd.ValidateRule
{
    class FinishTimeValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
    {
        internal DevExpress.XtraEditors.DateEdit dtFinishTime;
        internal long? StartTime { get; set; } // bat dau kham
        internal long? InTime { get; set; } // vao vien
        internal long? OutTime { get; set; } // TG ket thuc dieu tri

        public override bool Validate(System.Windows.Forms.Control control, object value)
        {
            bool valid = false;
            try
            {
                if (dtFinishTime == null)
                {
                    valid = true;
                    return valid;
                }

                if (dtFinishTime.EditValue == null || dtFinishTime.DateTime == DateTime.MinValue)
                {
                    valid = true;
                    return valid;
                }

                long FinishTime = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(dtFinishTime.DateTime) ?? 0;
                if (InTime.HasValue && FinishTime < InTime)
                {
                    ErrorText = "Thời gian vào viện <= thời gian kết thúc khám";
                    ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
                    return valid;
                }
                if (StartTime.HasValue && FinishTime < StartTime)
                {
                    ErrorText = "Thời gian bắt đầu khám <= thời gian kết thúc khám";
                    ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
                    return valid;
                }
                if (OutTime.HasValue && FinishTime > OutTime)
                {
                    ErrorText = "Thời gian kết thúc khám <= thời gian kết thúc điều trị";
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
