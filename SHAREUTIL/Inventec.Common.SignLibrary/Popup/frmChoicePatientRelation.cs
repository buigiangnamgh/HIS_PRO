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
using Inventec.Common.SignLibrary.Validation;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;

namespace Inventec.Common.SignLibrary
{
    public partial class frmChoicePatientRelation : Form
    {
        Action<EMR_RELATION, string> actChoose;
        List<EMR_RELATION> relationDatas;
        private int positionHandleControl = -1;

        internal EMR_RELATION relation;
        internal string relationName;

        public frmChoicePatientRelation()
        {
            InitializeComponent();
        }

        public frmChoicePatientRelation(Action<EMR_RELATION, string> _actChoose)
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

                this.ValidRelationName();
                this.ValidRelationCombo();
                txtRelationName.Focus();
                txtRelationName.SelectAll();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ValidRelationName()
        {
            try
            {
                RelationNameValidationRule rule = new RelationNameValidationRule();
                rule.txtRelationName = txtRelationName;
                dxValidationProvider1.SetValidationRule(txtRelationName, rule);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ValidRelationCombo()
        {
            try
            {
                RelationComboValidationRule rule = new RelationComboValidationRule();
                rule.cboRelation = cboRelation;
                dxValidationProvider1.SetValidationRule(cboRelation, rule);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            try
            {
                if (!dxValidationProvider1.Validate())
                {
                    return;
                }

                this.relation = cboRelation.EditValue != null ? this.relationDatas.Where(o => o.ID == (long)cboRelation.EditValue).FirstOrDefault() : null;
                this.relationName = txtRelationName.Text;

                this.actChoose(this.relation, txtRelationName.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void dxValidationProvider1_ValidationFailed(object sender, DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventArgs e)
        {
            try
            {
                BaseEdit edit = e.InvalidControl as BaseEdit;
                if (edit == null)
                    return;
                BaseEditViewInfo viewInfo = edit.GetViewInfo() as BaseEditViewInfo;
                if (viewInfo == null)
                    return;
                if (positionHandleControl == -1)
                {
                    positionHandleControl = edit.TabIndex;
                    if (edit.Visible)
                    {
                        edit.SelectAll();
                        edit.Focus();
                    }
                }
                if (positionHandleControl > edit.TabIndex)
                {
                    positionHandleControl = edit.TabIndex;
                    if (edit.Visible)
                    {
                        edit.Focus();
                        edit.SelectAll();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void txtRelationName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    cboRelation.Focus();
                    cboRelation.ShowPopup();
                    Inventec.Common.Integrate.PopupLoader.SelectFirstRowPopup(cboRelation);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void barBtnAccept_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (btnChoose.Enabled)
                {
                    btnChoose_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

    }
}
