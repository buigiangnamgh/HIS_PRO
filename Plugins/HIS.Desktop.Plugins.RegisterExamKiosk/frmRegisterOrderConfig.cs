using AutoMapper;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LibraryMessage;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.RegisterExamKiosk.ADO;
using HIS.Desktop.Plugins.RegisterExamKiosk;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Core;
using Inventec.Desktop.Common.Message;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;
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

namespace HIS.Desktop.Plugins.RegisterExamKiosk
{
    public partial class frmRegisterOrderConfig : FormBase
    {
        HIS.Desktop.Library.CacheClient.ControlStateWorker controlStateWorker;
        List<HIS.Desktop.Library.CacheClient.ControlStateRDO> currentControlStateRDO;
        const string moduleLink = "HIS.Desktop.Plugins.RegisterExamKiosk";
        private Inventec.Desktop.Common.Modules.Module module;
        const int ConfigTimeForAutoOpen = 500;

        List<long> lstId = new List<long>();
        public frmRegisterOrderConfig(Inventec.Desktop.Common.Modules.Module moduleData)
            : base()
        {
            InitializeComponent();
            this.module = moduleData;
            this.SetIcon();
        }

        private void SetIcon()
        {

            try
            {
                string iconPath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, System.Configuration.ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]);
                this.Icon = Icon.ExtractAssociatedIcon(iconPath);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

        }

        private void frmRegisterOrderConfig_Load(object sender, EventArgs e)
        {
            try
            {
                WaitingManager.Show();
                SetDefaultSetting();
                InitControlState();
                if (chkAutoOpen.Checked)
                {
                    CreateThreadAutoOpen();
                }
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            finally 
            {
            }
        }


        private void CreateThreadAutoOpen()
        {
            try
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadAutoOpen));
                //thread.Priority = System.Threading.ThreadPriority.Highest;
                thread.IsBackground = true;
                thread.SetApartmentState(System.Threading.ApartmentState.STA);
                try
                {
                    thread.Start();
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                    thread.Abort();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ThreadAutoOpen()
        {
            try
            {
                System.Threading.Thread.Sleep(ConfigTimeForAutoOpen);
                ProcessThietLap();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetDefaultSetting()
        {
            try
            {
                txtColumn_pk.Text = "4";
                txtTitleSize_pk.Text = "10";
                txtSizeItem_pk.Text = "120";

                txtColumn_dv.Text = "4";
                txtTitleSize_dv.Text = "10";
                txtSizeItem_dv.Text = "120";
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void InitControlState()
        {
            try
            {
                this.controlStateWorker = new HIS.Desktop.Library.CacheClient.ControlStateWorker();
                this.currentControlStateRDO = controlStateWorker.GetData(moduleLink);
                if (this.currentControlStateRDO != null && this.currentControlStateRDO.Count > 0)
                {
                    foreach (var item in this.currentControlStateRDO)
                    {
                        if (item.KEY == "HisRegisterGateADO")
                        {
                            var arrStr = item.VALUE.Split(';');
                            for (int i = 0; i < arrStr.Length; i++)
                            {
                                lstId.Add(Int64.Parse(arrStr[i]));
                            }
                        }
                        else if (item.KEY == txtColumn_pk.Name)
                        {
                            txtColumn_pk.Text = item.VALUE;
                        }
                        else if (item.KEY == txtTitleSize_pk.Name)
                        {
                            txtTitleSize_pk.Text = item.VALUE;
                        }
                        else if (item.KEY == txtSizeItem_pk.Name)
                        {
                            txtSizeItem_pk.Text = item.VALUE;
                        }
                        else if (item.KEY == txtColumn_dv.Name)
                        {
                            txtColumn_dv.Text = item.VALUE;
                        }
                        else if (item.KEY == txtTitleSize_dv.Name)
                        {
                            txtTitleSize_dv.Text = item.VALUE;
                        }
                        else if (item.KEY == txtSizeItem_dv.Name)
                        {
                            txtSizeItem_dv.Text = item.VALUE;
                        }
                        else if (item.KEY == chkAutoOpen.Name)
                        {
                            chkAutoOpen.Checked = item.VALUE == true.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnConfig.Enabled) return;
                ProcessThietLap();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessThietLap()
        {
            try
            {

                SettingADO stAdo = new SettingADO();
                stAdo.Pk_Columns = !string.IsNullOrEmpty(txtColumn_pk.Text) ? Int64.Parse(txtColumn_pk.Text.Trim()) : 4;
                stAdo.Pk_SizeItem = !string.IsNullOrEmpty(txtSizeItem_pk.Text) ? Int64.Parse(txtSizeItem_pk.Text.Trim()) : 120;
                stAdo.Pk_SizeTitle = !string.IsNullOrEmpty(txtTitleSize_pk.Text) ? Int64.Parse(txtTitleSize_pk.Text.Trim()) : 10;
                stAdo.Dv_Columns = !string.IsNullOrEmpty(txtColumn_dv.Text) ? Int64.Parse(txtColumn_dv.Text.Trim()) : 4;
                stAdo.Dv_SizeItem = !string.IsNullOrEmpty(txtSizeItem_dv.Text) ? Int64.Parse(txtSizeItem_dv.Text.Trim()) : 120;
                stAdo.Dv_SizeTitle = !string.IsNullOrEmpty(txtTitleSize_dv.Text) ? Int64.Parse(txtTitleSize_dv.Text.Trim()) : 10;

                frmWaitingScreen frm = new frmWaitingScreen(this.module, stAdo);

                SetControlState(txtColumn_pk);
                SetControlState(txtSizeItem_pk);
                SetControlState(txtTitleSize_pk);

                SetControlState(txtColumn_dv);
                SetControlState(txtSizeItem_dv);
                SetControlState(txtTitleSize_dv);

                SetControlState(chkAutoOpen);

                frm.ShowDialog();
                
                //this.Close();

            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        private void SetControlState_HisRegisterGateADO(List<HisRegisterGateADO> lstSend)
        {
            try
            {
                WaitingManager.Show();
                HIS.Desktop.Library.CacheClient.ControlStateRDO csAddOrUpdate = (this.currentControlStateRDO != null && this.currentControlStateRDO.Count > 0) ? this.currentControlStateRDO.Where(o => o.KEY == "HisRegisterGateADO" && o.MODULE_LINK == moduleLink).FirstOrDefault() : null;
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
                if (csAddOrUpdate != null)
                {
                    csAddOrUpdate.VALUE = string.Join(";", lstSend.Select(o => o.ID).ToList());
                }
                else
                {
                    csAddOrUpdate = new HIS.Desktop.Library.CacheClient.ControlStateRDO();
                    csAddOrUpdate.KEY = "HisRegisterGateADO";
                    csAddOrUpdate.VALUE = string.Join(";", lstSend.Select(o => o.ID).ToList());
                    csAddOrUpdate.MODULE_LINK = moduleLink;
                    if (this.currentControlStateRDO == null)
                        this.currentControlStateRDO = new List<HIS.Desktop.Library.CacheClient.ControlStateRDO>();
                    this.currentControlStateRDO.Add(csAddOrUpdate);
                }
                this.controlStateWorker.SetData(this.currentControlStateRDO);

                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetControlState(TextEdit txt)
        {
            try
            {
                WaitingManager.Show();
                HIS.Desktop.Library.CacheClient.ControlStateRDO csAddOrUpdate = (this.currentControlStateRDO != null && this.currentControlStateRDO.Count > 0) ? this.currentControlStateRDO.Where(o => o.KEY == txt.Name && o.MODULE_LINK == moduleLink).FirstOrDefault() : null;
                if (csAddOrUpdate != null)
                {
                    csAddOrUpdate.VALUE = txt.Text;
                }
                else
                {
                    csAddOrUpdate = new HIS.Desktop.Library.CacheClient.ControlStateRDO();
                    csAddOrUpdate.KEY = txt.Name;
                    csAddOrUpdate.VALUE = txt.Text;
                    csAddOrUpdate.MODULE_LINK = moduleLink;
                    if (this.currentControlStateRDO == null)
                        this.currentControlStateRDO = new List<HIS.Desktop.Library.CacheClient.ControlStateRDO>();
                    this.currentControlStateRDO.Add(csAddOrUpdate);
                }
                this.controlStateWorker.SetData(this.currentControlStateRDO);

                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetControlState(CheckEdit chk)
        {
            try
            {
                WaitingManager.Show();
                HIS.Desktop.Library.CacheClient.ControlStateRDO csAddOrUpdate = (this.currentControlStateRDO != null && this.currentControlStateRDO.Count > 0) ? this.currentControlStateRDO.Where(o => o.KEY == chk.Name && o.MODULE_LINK == moduleLink).FirstOrDefault() : null;
                if (csAddOrUpdate != null)
                {
                    csAddOrUpdate.VALUE = chk.Checked.ToString();
                }
                else
                {
                    csAddOrUpdate = new HIS.Desktop.Library.CacheClient.ControlStateRDO();
                    csAddOrUpdate.KEY = chk.Name;
                    csAddOrUpdate.VALUE = chk.Checked.ToString();
                    csAddOrUpdate.MODULE_LINK = moduleLink;
                    if (this.currentControlStateRDO == null)
                        this.currentControlStateRDO = new List<HIS.Desktop.Library.CacheClient.ControlStateRDO>();
                    this.currentControlStateRDO.Add(csAddOrUpdate);
                }
                this.controlStateWorker.SetData(this.currentControlStateRDO);

                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void gridViewRegisterGate_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    HisRegisterGateADO data = (HisRegisterGateADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (data != null)
                    {
                        if (e.Column.FieldName == "STT")
                        {
                            e.Value = e.ListSourceRowIndex + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void txtColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void txtSizeItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void txtTitleSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void txtSttSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

    }
}
