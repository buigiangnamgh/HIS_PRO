using EMR.EFMODEL.DataModels;
using Inventec.Common.Logging;
using System;
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
    public partial class frmChooseBusiness : Form
    {
        Action<bool> IsReloadEmrBusiness;
        Action<EMR_BUSINESS> actChoose;
        List<EMR.EFMODEL.DataModels.EMR_BUSINESS> emrBusiness;
        string currentBusinessCode;
        public frmChooseBusiness(Action<EMR_BUSINESS> _actChoose, List<EMR.EFMODEL.DataModels.EMR_BUSINESS> _emrBusiness, string _currentBusinessCode, Action<bool> IsReloadEmrBusiness)
        {
            InitializeComponent();
            this.IsReloadEmrBusiness = IsReloadEmrBusiness;
            this.actChoose = _actChoose;
            this.emrBusiness = _emrBusiness;
            this.currentBusinessCode = _currentBusinessCode;
        }

        private void frmChooseBusiness_Load(object sender, EventArgs e)
        {
            try
            {
                int focusedRowHandle = 0;
                gridControl1.DataSource = this.emrBusiness;
                if (!String.IsNullOrEmpty(this.currentBusinessCode) && this.emrBusiness != null && this.emrBusiness.Count > 0)
                {
                    for (int i = 0; i < this.emrBusiness.Count; i++)
                    {
                        if (this.emrBusiness[i].BUSINESS_CODE == this.currentBusinessCode)
                        {
                            focusedRowHandle = i;
                            break;
                        }
                    }
                }
                if (emrBusiness == null || emrBusiness.Count == 0)
                {
                    btnChoose.Enabled = false;
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
                var data = (EMR_BUSINESS)this.gridView1.GetFocusedRow();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.emrBusiness == null)
                    this.emrBusiness = new List<EMR_BUSINESS>();
                frmCreateEmrBusiness frm = new frmCreateEmrBusiness(emrBusiness, o =>
                {
                    if (o != null)
                    {
                        emrBusiness.Add(o);
                        gridControl1.DataSource = null;
                        emrBusiness = emrBusiness.OrderByDescending(p => p.CREATE_TIME).ToList();
                        gridControl1.DataSource = emrBusiness;
                        gridView1.FocusedRowHandle = 0;
                        IsReloadEmrBusiness(true);
                        btnChoose.Enabled = true;
                    }
                });
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(btnChoose.Enabled)
                btnChoose_Click(null, null);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnAdd_Click(null,null);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnSearch_Click(null,null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                {
                    var dataSearch = emrBusiness.Where(o => o.BUSINESS_CODE.ToLower().Contains(txtSearch.Text.Trim().ToLower()) 
                                                                || o.BUSINESS_NAME.ToLower().Contains(txtSearch.Text.Trim().ToLower()));
                    gridControl1.DataSource = dataSearch;
                }else
                    gridControl1.DataSource = emrBusiness;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void txtSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    btnSearch_Click(null,null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
