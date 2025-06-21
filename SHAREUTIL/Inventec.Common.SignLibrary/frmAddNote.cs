using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
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
    public partial class frmAddNote : Form
    {
        Action<string> actAddNote;
        public frmAddNote(Action<string> _actAddNote)
        {
            try
            {
                InitializeComponent();
                this.actAddNote = _actAddNote;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtNote.Text))
                {
                    MessageBox.Show("Chưa nhập ý kiến người ký");
                    return;
                }

                if (this.actAddNote != null)
                {
                    this.actAddNote(txtNote.Text.Trim());
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
            }
        }
    }
}
