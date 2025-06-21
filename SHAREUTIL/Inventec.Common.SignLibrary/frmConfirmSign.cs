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
    public partial class frmConfirmSign : Form
    {
        Action actNo;
        Action actYes;

        public frmConfirmSign(Action actNo, Action actYes)
        {
            InitializeComponent();
            this.actNo = actNo;
            this.actYes = actYes;
        }

        private void frmConfirmSign_Load(object sender, EventArgs e)
        {
            try
            {
                btnYes.Focus();

                lblTitle.Text = LibraryMessage.MessageUitl.GetMessage(LibraryMessage.MessageConstan.TuDongCapNhatChuKyThatBaiBanCoMuonTiepTucVaBoHienThiChuKy);

                if (!String.IsNullOrEmpty(CacheClientWorker.GetValue()))
                {
                    chkState.Checked = true;
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkState.Checked)
                {
                    CacheClientWorker.ChangeValue("0");
                }
                if (this.actNo != null)
                {
                    this.actNo();
                }
                this.Close();
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
                if (chkState.Checked)
                {
                    CacheClientWorker.ChangeValue(Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP.ToString());
                }
                if (this.actYes != null)
                {
                    this.actYes();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void chkState_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }
    }
}
