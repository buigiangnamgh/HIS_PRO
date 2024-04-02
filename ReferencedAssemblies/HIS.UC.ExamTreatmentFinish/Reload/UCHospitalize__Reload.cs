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
using DevExpress.XtraEditors.ViewInfo;
using MOS.EFMODEL.DataModels;
//using HIS.UC.Hospitalize.ADO;
using HIS.UC.ExamTreatmentFinish.ADO;

namespace HIS.UC.ExamTreatmentFinish.Run
{
    public partial class UCExamTreatmentFinish : UserControl
    {
        public void Reload(TreatmentFinishInitADO hospitalize)
        {
            try
            {
                if (hospitalize != null)
                {
                    LoadIcdToControl(hospitalize.IcdCode, hospitalize.IcdName);
                    LoaducSecondaryIcd(hospitalize.IcdSubCode, hospitalize.IcdText);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
