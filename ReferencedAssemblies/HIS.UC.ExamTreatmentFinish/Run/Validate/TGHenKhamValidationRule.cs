using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamTreatmentFinish.Run.Validate
{
    class TGHenKhamValidationRule : DevExpress.XtraEditors.DXErrorProvider.ValidationRule
    {
        internal DevExpress.XtraEditors.DateEdit dtTGHenKham;
        internal DevExpress.XtraEditors.DateEdit dtTGRaVien;
        internal DevExpress.XtraEditors.GridLookUpEdit cboTET;
        public override bool Validate(System.Windows.Forms.Control control, object value)
        {
            bool valid = false;
            try
            {
                if (dtTGHenKham == null || dtTGRaVien == null) return valid;

                if (dtTGHenKham.Enabled)
                {
                    if (dtTGHenKham.EditValue != null && dtTGRaVien.EditValue != null && dtTGHenKham.DateTime < dtTGRaVien.DateTime)
                    {
                        this.ErrorText = "Thời gian hẹn khám không được nhỏ hơn thời gian ra viện";
                        return valid;
                    }

                    if (cboTET.EditValue != null && Inventec.Common.TypeConvert.Parse.ToInt64(cboTET.EditValue.ToString()) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN && dtTGHenKham.EditValue == null)
                    {
                        this.ErrorText = "Loại ra viện là hẹn khám bắt buộc nhập thời gian hẹn khám";
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
