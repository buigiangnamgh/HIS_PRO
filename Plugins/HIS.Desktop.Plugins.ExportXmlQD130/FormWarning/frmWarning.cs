using HIS.Desktop.Plugins.ExportXmlQD130.ADO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.Desktop.Plugins.ExportXmlQD130.FormWarning
{

    public partial class frmWarning : Form
    {
        List<WarningADO> _listWarningADO;
        #region Construct
        public frmWarning()
        {
            InitializeComponent();
            try
            {
                SetIcon();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public frmWarning(List<WarningADO> ListWarningADO)
            :this()
        {
            try
            {
                this._listWarningADO = ListWarningADO;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void SetIcon()
        {
            try
            {
                this.Icon = Icon.ExtractAssociatedIcon(System.IO.Path.Combine(LocalStorage.Location.ApplicationStoreLocation.ApplicationDirectory, System.Configuration.ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        #endregion

        private void frmWarning_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataToGridWarning(this._listWarningADO);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        #region Private method

        private void LoadDataToGridWarning(List<WarningADO> data)
        {
            try
            {
                gridControlWarning.BeginUpdate();
                gridControlWarning.DataSource = null;
                gridControlWarning.DataSource = data;
                gridControlWarning.EndUpdate();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
