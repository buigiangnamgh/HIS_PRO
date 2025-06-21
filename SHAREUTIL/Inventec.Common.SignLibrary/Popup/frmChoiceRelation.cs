using Inventec.Common.Logging;
using Inventec.Common.SignLibrary.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventec.Common.Integrate.EditorLoader;
using EMR.EFMODEL.DataModels;
using Inventec.Common.SignLibrary.LibraryMessage;
using Inventec.Common.SignLibrary.Integrate;

namespace Inventec.Common.SignLibrary
{
    public partial class frmChoiceRelation : Form
    {
        Action<EMR_RELATION, bool> actChoose;
        List<EMR_RELATION> relationDatas;
        public frmChoiceRelation(Action<EMR_RELATION, bool> _actChoose)
        {
            InitializeComponent();
            this.actChoose = _actChoose;
        }

        private void frmChoiceRelation_Load(object sender, EventArgs e)
        {
            try
            {
                this.relationDatas = new EmrRelation().Get();
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();
                columnInfos.Add(new ColumnInfo("RELATION_NAME", "", 250, 2));
                ControlEditorADO controlEditorADO = new ControlEditorADO("RELATION_NAME", "ID", columnInfos, false, 250);
                ControlEditorLoader.Load(cboRelation, this.relationDatas, controlEditorADO);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            try
            {
                var data = cboRelation.EditValue != null ? this.relationDatas.Where(o => o.ID == (long)cboRelation.EditValue).FirstOrDefault() : null;
                if (data == null && !chkIsPatientSign.Checked)
                {
                    Inventec.Common.SignLibrary.Integrate.MessageManager.Show(MessageUitl.GetMessage(MessageConstan.ChuaChonMoiQuanHeVoiBenhNhan));
                    return;
                }

                this.actChoose(data, chkIsPatientSign.Checked);
                this.Close();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void chkIsPatientSign_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsPatientSign.Checked)
            {
                cboRelation.EditValue = null;
                lciForcboRelation.Enabled = false;
            }
            else
            {
                lciForcboRelation.Enabled = true;
            }
        }

    }
}
