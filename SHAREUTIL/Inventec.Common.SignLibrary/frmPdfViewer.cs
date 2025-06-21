using EMR.EFMODEL.DataModels;
using Inventec.Common.SignLibrary.ADO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary
{
    internal partial class frmPdfViewer : Form
    {
        InputADO inputADO;
        string inputFile;
        Stream inputStream;
        byte[] inputByte;
        FileType fileType;
        bool isSignNow;
        bool isPrintNow;
        Action<string> actionAfterSigned;
        UCViewer ucViewer;
        EMR.EFMODEL.DataModels.EMR_SIGNER Signer { get; set; }
        EMR.EFMODEL.DataModels.EMR_TREATMENT Treatment { get; set; }
        string TokenCode { get; set; }
        FileADO fileADOMain = null;
        FileADO fileADOJson = null;
        FileADO fileADOXml = null;

        public frmPdfViewer(string inputFile, FileType fileType, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode)
        {
            InitializeComponent();
            this.inputADO = inputADO;
            this.inputFile = inputFile;
            this.fileType = fileType;
            this.Signer = signer;
            this.Treatment = treatment;
            this.TokenCode = tokenCode;
        }

        public frmPdfViewer(string inputFile, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode)
        {
            InitializeComponent();
            this.inputADO = inputADO;
            this.inputFile = inputFile;
            this.fileType = FileType.Pdf;
            this.Signer = signer;
            this.Treatment = treatment;
            this.TokenCode = tokenCode;
        }

        public frmPdfViewer(Stream inputStream, InputADO inputADO, EMR_SIGNER singer, EMR_TREATMENT treatment, string tokenCode)
        {
            InitializeComponent();
            this.inputADO = inputADO;
            this.inputStream = inputStream;
            this.fileType = FileType.Pdf;
            this.Signer = singer;
            this.Treatment = treatment;
            this.TokenCode = tokenCode;
        }

        public frmPdfViewer(byte[] inputByte, FileType fileType, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode)
        {
            InitializeComponent();
            this.inputADO = inputADO;
            this.inputByte = inputByte;
            this.fileType = fileType;
            this.Signer = signer;
            this.Treatment = treatment;
            this.TokenCode = tokenCode;
        }

        public frmPdfViewer(byte[] inputByte, FileType fileType, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode, bool isSignNow, Action<string> actionAfterSigned, bool printNow = false)
        {
            InitializeComponent();
            this.inputADO = inputADO;
            this.Signer = signer;
            this.Treatment = treatment;
            this.inputByte = inputByte;
            this.fileType = fileType;
            this.isSignNow = isSignNow;
            this.isPrintNow = printNow;
            this.actionAfterSigned = actionAfterSigned;
            this.TokenCode = tokenCode;
        }

        public frmPdfViewer(string inputFile, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode, bool isSignNow, Action<string> actionAfterSigned, bool printNow = false)
        {
            InitializeComponent();
            this.inputADO = inputADO;
            this.inputFile = inputFile;
            this.fileType = FileType.Pdf;
            this.isSignNow = isSignNow;
            this.isPrintNow = printNow;
            this.Signer = signer;
            this.Treatment = treatment;
            this.actionAfterSigned = actionAfterSigned;
            this.TokenCode = tokenCode;
        }


        public void UpdateExtFileType(FileADO _fileADOMain, FileADO _fileADOJson, FileADO _fileADOXml)
        {
            this.fileADOMain = _fileADOMain;
            this.fileADOJson = _fileADOJson;
            this.fileADOXml = _fileADOXml;
        }

        private void frmPdfViewer_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(this.inputFile))
            {
                ucViewer = new UCViewer(inputFile, this.fileType, inputADO, Signer, Treatment, TokenCode, CloseFormProcess, this.isSignNow, this.isPrintNow, CloseFormAfterSign);
                ucViewer.UpdateExtFileType(fileADOMain, fileADOJson, fileADOXml);
                ucViewer.Dock = DockStyle.Fill;
                this.Controls.Add(ucViewer);
            }
            else if (this.inputStream != null && this.inputStream.Length > 0)
            {
                ucViewer = new UCViewer(this.inputStream, inputADO, Signer, Treatment, TokenCode);
                ucViewer.UpdateExtFileType(fileADOMain, fileADOJson, fileADOXml);
                ucViewer.Dock = DockStyle.Fill;
                this.Controls.Add(ucViewer);
            }
            else if (this.inputByte != null && this.inputByte.Length > 0)
            {
                ucViewer = new UCViewer(this.inputByte, this.fileType, inputADO, Signer, Treatment, TokenCode, CloseFormProcess, this.isSignNow, this.isPrintNow, CloseFormAfterSign);
                ucViewer.UpdateExtFileType(fileADOMain, fileADOJson, fileADOXml);
                ucViewer.Dock = DockStyle.Fill;
                this.Controls.Add(ucViewer);
            }
        }

        internal EMR.TDO.DocumentTDO GetCurrentDocument()
        {
            return ucViewer != null ? ucViewer.GetCurrentDocument() : null;
        }

        void CloseFormProcess(string outputFile)
        {
            if (this.actionAfterSigned != null)
                this.actionAfterSigned(outputFile);
            this.Close();
        }

        void CloseFormAfterSign(bool signed)
        {
            this.Close();
        }

        void DisposeVariable()
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("frmPdfViewer.DisposeVariable.1");
                try
                {
                    if (ucViewer != null)
                    {
                        try
                        {
                            if (GetCurrentDocument() != null)
                                this.inputADO.DocumentCode = GetCurrentDocument().DocumentCode;
                        }
                        catch (Exception exx)
                        {
                            Inventec.Common.Logging.LogSystem.Warn(exx);
                        }

                        ucViewer.DisposeVariable(null, null);
                    }
                    ucViewer = null;
                }
                catch (Exception ex1)
                {
                    Logging.LogSystem.Warn(ex1);
                }

                this.Dispose(true);
                Inventec.Common.Logging.LogSystem.Debug("frmPdfViewer.DisposeVariable.2");
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }

            try
            {
                inputADO = null;
                inputFile = null;
                if (this.inputStream != null)
                    this.inputStream.Close();
                this.inputStream = null;
                inputByte = null;
                actionAfterSigned = null;

                Signer = null;
                Treatment = null;
                TokenCode = null;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void frmPdfViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DisposeVariable();
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }
    }
}
