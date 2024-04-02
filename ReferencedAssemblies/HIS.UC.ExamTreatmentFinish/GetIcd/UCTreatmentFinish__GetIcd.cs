using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MOS.EFMODEL.DataModels;
using HIS.UC.Death;
using HIS.UC.TranPati;
using HIS.UC.TranPati.ADO;
using HIS.UC.Death.ADO;
using HIS.UC.ExamTreatmentFinish.ADO;
using MOS.SDO;
using HIS.UC.SecondaryIcd.ADO;
using HIS.UC.SurgeryAppointment.ADO;
using Inventec.Common.Logging;

namespace HIS.UC.ExamTreatmentFinish.Run
{
    public partial class UCExamTreatmentFinish : UserControl
    {
        public object GetIcd()
        {
            ExamTreatmentFinishResult ExamTreatmentFinish = new ExamTreatmentFinishResult();
            try
            {               
                ExamTreatmentFinish.icdADOInTreatment = this.UcIcdGetValue() as HIS.UC.Icd.ADO.IcdInputADO;   
                if (ucSecondaryIcd != null)
                {
                    var subIcd = subIcdProcessor.GetValue(ucSecondaryIcd) as SecondaryIcdDataADO;
                    if (subIcd != null )
                    {
                        ExamTreatmentFinish.TreatmentFinishSDO = new HisTreatmentFinishSDO();
                        ExamTreatmentFinish.TreatmentFinishSDO.IcdSubCode = subIcd.ICD_SUB_CODE;
                        ExamTreatmentFinish.TreatmentFinishSDO.IcdText = subIcd.ICD_TEXT;
                    }
                }
                LogSystem.Debug("UCExamTreatmentFinish.GetIcd. End: \n" + LogUtil.TraceData("ExamTreatmentFinish", ExamTreatmentFinish));
            }
            catch (Exception ex)
            {
                ExamTreatmentFinish = null;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return ExamTreatmentFinish;
        }
    }
}
