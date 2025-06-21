using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using EMR.EFMODEL.DataModels;
using Inventec.Common.Integrate;
using Inventec.Common.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary
{
    public partial class frmChooseDocumentDependent : Form
    {
        Action<V_EMR_DOCUMENT> actChoose;
        List<EMR.EFMODEL.DataModels.V_EMR_DOCUMENT> emrDocument;
        string currentDocumentCode;
        public frmChooseDocumentDependent(Action<V_EMR_DOCUMENT> _actChoose, List<EMR.EFMODEL.DataModels.V_EMR_DOCUMENT> _emrDocument, string _currentDocumentCode)
        {
            InitializeComponent();
            this.actChoose = _actChoose;
            this.emrDocument = _emrDocument;
            this.currentDocumentCode = _currentDocumentCode;
        }

        private void frmChooseBusiness_Load(object sender, EventArgs e)
        {
            try
            {
                int focusedRowHandle = 0;
                gridControl1.DataSource = this.emrDocument;
                if (!String.IsNullOrEmpty(this.currentDocumentCode) && this.emrDocument != null && this.emrDocument.Count > 0)
                {
                    for (int i = 0; i < this.emrDocument.Count; i++)
                    {
                        if (this.emrDocument[i].DOCUMENT_CODE == this.currentDocumentCode)
                        {
                            focusedRowHandle = i;
                            break;
                        }
                    }
                }

                gridView1.FocusedRowHandle = focusedRowHandle;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Space)
                {
                    if (this.gridView1.IsEditing)
                        this.gridView1.CloseEditor();

                    if (this.gridView1.FocusedRowModified)
                        this.gridView1.UpdateCurrentRow();

                    btnChoose_Click(null, null);
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    btnChoose_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                btnChoose_Click(null, null);
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
                var data = (V_EMR_DOCUMENT)this.gridView1.GetFocusedRow();
                if (data != null)
                {
                    this.actChoose(data);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    V_EMR_DOCUMENT docData = (V_EMR_DOCUMENT)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (docData != null)
                    {
                        if (e.Column.FieldName == "CREAT_TIME_DISPLAY")
                        {
                            e.Value = DateTimeConvert.TimeNumberToTimeString(docData.CREATE_TIME ?? 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

    }
}
