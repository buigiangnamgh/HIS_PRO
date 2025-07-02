using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.Desktop.Plugins.HisProductInfo
{

    public partial class UCDocument : UserControl
    {
        public UCDocument()
        {
            InitializeComponent();
        }
        public void SetText(string text)
        {
            try
            {
                this.pdfViewer1.Text = text;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void SetRtfText(string text)
        {
            try
            {
                this.pdfViewer1.Text = text;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public string getRtfText()
        {
            try
            {
                return pdfViewer1.Text;
            }
            catch (Exception ex)
            {
                return "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void SetFont(Font font)
        {
            try
            {
                pdfViewer1.Appearance.Font = font;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public bool getLandscape(int i)
        {
            try
            {
                return 1 == 1;
            }
            catch (Exception ex)
            {
                return false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public int getWidth()
        {
            try
            {
                return pdfViewer1.Width;
            }
            catch (Exception ex)
            {
                return 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public float getPageHeight(int i)
        {
            try
            {
                return pdfViewer1.Height;
            }
            catch (Exception ex)
            {
                return 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public float getPageWidth(int i)
        {
            try
            {
                return pdfViewer1.Width;
            }
            catch (Exception ex)
            {
                return 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void SetZoomFactor(float zoom)
        {
            try
            {
                //pdfViewer1.ActiveView.ZoomFactor = zoom;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void Focus()
        {
            try
            {
                pdfViewer1.Focus();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void UCDocument_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.S)
                {
                    return;
                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void txtContent_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Control && e.KeyCode == Keys.S)
                {
                    e.SuppressKeyPress = true;
                    return;
                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        public void AllowEdit(bool allow)
        {
            try
            {
                pdfViewer1.ReadOnly = !allow;
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "PDF file(*.pdf)|*.pdf";
                if (openFile.ShowDialog() == DialogResult.OK)
                {

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }
    }
}

