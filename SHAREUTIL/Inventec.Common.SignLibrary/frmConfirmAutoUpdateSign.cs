using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventec.Common.SignLibrary.CacheClient;

namespace Inventec.Common.SignLibrary
{
    public partial class frmConfirmAutoUpdateSign : Form
    {
        bool isYes;
        Action<bool> actYes;

        public frmConfirmAutoUpdateSign(Action<bool> actYes)
        {
            InitializeComponent();
            this.actYes = actYes;
        }

        private void frmConfirmSign_Load(object sender, EventArgs e)
        {
            try
            {
                btnYes.Focus();

                lblTitle.Text = LibraryMessage.MessageUitl.GetMessage(LibraryMessage.MessageConstan.TaiKhoanThieuThongTinAnhHeThongTuDongCapNhat);
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.actYes != null)
                {
                    this.isYes = true;
                    this.actYes(isYes);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void frmConfirmAutoUpdateSign_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (!isYes && this.actYes != null)
                {
                    this.actYes(isYes);
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }
    }
}
