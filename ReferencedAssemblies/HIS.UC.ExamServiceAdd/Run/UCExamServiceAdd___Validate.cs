using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MOS.SDO;
using HIS.UC.ExamServiceAdd.ADO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;
using Inventec.Desktop.Common.Controls.ValidationRule;
using HIS.UC.ExamServiceAdd.ValidateRule;
using HIS.UC.HisExamServiceAdd.ValidateRule;

namespace HIS.UC.ExamServiceAdd.Run
{
    public partial class UCExamServiceAdd : UserControl
    {
        private void ValidateGridLookupWithTextEdit(GridLookUpEdit cbo, TextEdit textEdit)
        {
            try
            {
                GridLookupEditWithTextEditValidationRule validRule = new GridLookupEditWithTextEditValidationRule();
                validRule.txtTextEdit = textEdit;
                validRule.cbo = cbo;
                validRule.ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.HeThongTBTruongDuLieuBatBuocPhaiNhap);
                validRule.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
                dxValidationProvider1.SetValidationRule(textEdit, validRule);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void ValidateSingleControl(Control control)
        {
            try
            {
                Inventec.Desktop.Common.Controls.ValidationRule.ControlEditValidationRule validRule = new ControlEditValidationRule();
                validRule.editor = control;
                validRule.ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.HeThongTBTruongDuLieuBatBuocPhaiNhap);
                validRule.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
                dxValidationProvider1.SetValidationRule(control, validRule);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void ValidateFinishTimeExam(DateEdit control, long? StartTime, long? InTime, long? FinishTimeTreatment)
        {
            try
            {
                FinishTimeValidationRule validRule = new FinishTimeValidationRule();
                validRule.dtFinishTime = control;
                validRule.StartTime = StartTime;
                validRule.InTime = InTime;
                validRule.OutTime = FinishTimeTreatment;
                dxValidationProvider1.SetValidationRule(control, validRule);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void ValidationdatetimeAppointment(DateEdit control, CheckEdit chkAppointment)
        {
            try
            {
                AppointmentTimeValidationRule vali = new AppointmentTimeValidationRule();
                vali.dtAppointment = control;
                vali.chkAppointment = chkAppointment;
                vali.ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
                vali.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
                dxValidationProvider1.SetValidationRule(control, vali);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void ValidateForm()
        {
            try
            {
                ValidateSingleControl(dtIntructionTime);
                ValidateSingleControl(cboPatientType);
                ValidateGridLookupWithTextEdit(cboExamService, txtService);
                ValidateGridLookupWithTextEdit(cboExecuteRoom, txtExecuteRoom);
                //ValidateFinishTimeExam(dtFinishTime, this.examServiceAddInitADO.StartTime, examServiceAddInitADO.InTime, examServiceAddInitADO.OutTime);
                ValidationdatetimeAppointment(dtAppointment, chkAppointment);
                ValidationControlMaxLength(memNote, 2000);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        private void ValidationControlMaxLength(BaseEdit control, int? maxLength)
        {
            ControlMaxLengthValidationRule validate = new ControlMaxLengthValidationRule();
            validate.editor = control;
            validate.maxLength = maxLength;
            validate.ErrorText = String.Format("Trường dữ liệu vượt quá {0} ký tự", maxLength);
            validate.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
            this.dxValidationProvider1.SetValidationRule(control, validate);
        }

        private void RemoveValidateControl(Control control)
        {
            try
            {
                dxValidationProvider1.SetValidationRule(control, null);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
