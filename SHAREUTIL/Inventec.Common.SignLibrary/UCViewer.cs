using EMR.EFMODEL.DataModels;
using EMR.Filter;
using EMR.SDO;
using EMR.TDO;
using Inventec.Common.Integrate;
using Inventec.Common.SignFile;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.Api;
using Inventec.Common.SignLibrary.CacheClient;
using Inventec.Common.SignLibrary.DTO;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Printing;
using DevExpress.Pdf;
using DevExpress.XtraBars;

namespace Inventec.Common.SignLibrary
{
    public partial class UCViewer : UserControl
    {
        #region Variable
        string resultFileStore = "";
        string currentFileWorking = "";
        Stream currentStream;
        string inputFileWork = "";
        Stream inputStream;
        bool mouseButtonPressed = false;
        DevExpress.Pdf.PdfDocumentPosition startPosition;
        DevExpress.Pdf.PdfDocumentPosition endPosition;
        bool isShowImage = false;
        bool isShowRangtax = true;
        const float dpi = 72;
        const int lcStep = 10;
        PointF startPoint1;
        PointF endPoint1;
        float xImg, yImg;
        List<VerifierADO> verifiers;
        PdfReader readerWorking;
        Action<bool> closeAfterSign;
        Action<bool> dlgCloseAfterSignCheckedChanged;
        Action<bool> dlgOptionSignTypeCheckedChanged;
        bool? isCloseAfterSign = false;
        bool? isOptionSignType = false;
        bool isInitForm = false;
        Action<float, float> dlgChoosePoint;
        Action<DocumentSignedUpdateIGSysResultDTO> dlgSendResultSigned;
        bool isSelectRangeRectangle;
        List<string> watermarks;
        int pageNumberCurrent = 0;
        int totalPageNumber = 0;
        SignType signType;
        TreatmentDTO treatment;
        string treatmentCode;
        string signName;
        string signReason;
        string documentName;
        string businessCode;
        List<string> printTypeBusinessCodes;
        string roomCode;
        string roomTypeCode;
        DocumentTDO currentDocument;
        Action<DocumentTDO> dlgOpenModuleConfig;
        InputADO inputADOWorking;
        List<SignTDO> listSign;
        bool isSigning;
        string rejectReason;
        EMR_SIGN signSelected;
        EMR_SIGN signSelectedByUser;
        bool isMultiSign;
        bool isMultiSignByType = false;
        bool isPatientSign;
        bool isHomeRelativeSign;
        bool isPrintOnlyContent;
        long? DocumentTypeId;
        string hisCode;
        List<SignPositionADO> signPositionADOs;
        List<SignPositionADO> signAutoPositionADOs;
        int signedCount;
        bool hasNextSignPosition;
        SignPositionADO nextSignPosition;
        bool isSignNow;
        bool isPrintDocSignedNow;
        Action<string> actionAfterSigned;
        int plusNumberWithShowingSignInformation = 0;
        List<EMR_SIGNER> signers;
        short printNumberCopies;
        int typeDisplayOption = -1;
        System.Drawing.Printing.PageSettings currentPageSettings;
        string vlViewPACSUrlFormat = "";
        EMR.EFMODEL.DataModels.EMR_SIGNER Signer { get; set; }
        EMR.EFMODEL.DataModels.EMR_TREATMENT Treatment { get; set; }
        string TokenCode { get; set; }
        public bool IsLoadFirst = true;

        const short IS_SIGN_ELECTRONIC_VALUE = 1;
        public int Widths;
        public int Heights;
        FileType fileType;
        Action<bool> actChangeUsingSignPad;
        bool isUsingSignPad;
        FileADO fileADOJson = null;
        FileADO fileADOXml = null;
        FileADO fileADOMain = null;

        Action reload = null;

        Pen penDrawSignal = new Pen(Color.Aqua);
        System.Drawing.Image image = null;
        long total = 0;
        long sizeToSignAndDeskcription = 0;
        long sizeToRelativeHomeSign = 0;
        bool IsNotCaculateSignAndDeskcription = false;
        bool IsNotCaculateRelativeHomeSign = false;
        V_EMR_DOCUMENT currentEmrDocument { get; set; }
        public EMR_RELATION Relation { get; set; }
        public string RelationPeopleName { get; set; }
        #endregion

        public UCViewer()
        {
            InitializeComponent();
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.Nam.1");
        }

        private UCViewer(InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode)
        {
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.1");
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.Nam.2");
            InitializeComponent();
            if (inputADO.IsUsingSignPad.HasValue && inputADO.IsUsingSignPad.Value)
            {
                isUsingSignPad = true;
                barCheckUsingSignPad.Checked = true;
            }
            if (GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "3" || GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "2")
            {
                isUsingSignPad = true;
                barCheckUsingSignPad.Checked = true;
            }
            isInitForm = true;
            this.inputADOWorking = inputADO;
            this.Signer = signer;
            this.Treatment = treatment;
            this.TokenCode = tokenCode;
            if (inputADO != null)
            {
                this.watermarks = inputADO.Watermarks;
                this.signType = SignType.HSM;
                if (inputADO.SignType == SignType.OptionDefaultHsm || inputADO.SignType == SignType.OptionDefaultUsb)
                {
                    bbtnSignType.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    if (inputADO.IsOptionSignType.HasValue)
                    {
                        this.signType = inputADO.IsOptionSignType.Value ? SignType.USB : SignType.HSM;
                    }
                    else if (inputADO.SignType == SignType.OptionDefaultUsb)
                    {
                        this.signType = SignType.USB;
                    }
                    else
                    {
                        this.signType = SignType.HSM;
                    }

                    bbtnSignType.Checked = (this.signType == SignType.USB);
                }
                else
                {
                    this.signType = inputADO.SignType;
                }

                if (inputADO.IsUsingSignPad.HasValue && inputADO.IsUsingSignPad.Value)
                {
                    isUsingSignPad = true;
                    barCheckUsingSignPad.Checked = true;
                }
                this.actChangeUsingSignPad = inputADO.ActChangeUsingSignPad;
                this.Relation = inputADO.Relation;
                this.RelationPeopleName = inputADO.RelationPeopleName;
                this.dlgChoosePoint = inputADO.DlgChoosePoint;
                this.dlgSendResultSigned = inputADO.DlgSendResultSigned;
                this.isSelectRangeRectangle = inputADO.IsSelectRangeRectangle;
                this.dlgOpenModuleConfig = inputADO.DlgOpenModuleConfig;
                this.dlgCloseAfterSignCheckedChanged = inputADO.DlgCloseAfterSign;
                this.dlgOptionSignTypeCheckedChanged = inputADO.DlgChangeOptionSignType;
                this.treatment = inputADO.Treatment;
                this.isPrintOnlyContent = inputADO.IsPrintOnlyContent;
                this.plusNumberWithShowingSignInformation = (this.isPrintOnlyContent ? 0 : 1);
                this.treatmentCode = (inputADO.Treatment != null ? inputADO.Treatment.TREATMENT_CODE : "");
                this.documentName = inputADO.DocumentName;
                this.signName = (String.IsNullOrEmpty(GlobalStore.UserName) ? GlobalStore.LoginName : GlobalStore.LoginName + " (" + GlobalStore.UserName + ")");
                this.hisCode = inputADO.HisCode;
                this.signReason = inputADO.SignReason;
                this.businessCode = inputADO.BusinessCode;
                this.printTypeBusinessCodes = inputADO.PrintTypeBusinessCodes;
                this.roomCode = inputADO.RoomCode;
                this.roomTypeCode = inputADO.RoomTypeCode;
                GlobalStore.IsUseTimespan = inputADO.IsUseTimespan;
                this.printNumberCopies = inputADO.PrintNumberCopies.HasValue ? inputADO.PrintNumberCopies.Value : (short)1;
                this.isCloseAfterSign = inputADO.IsCloseAfterSign;
                this.isOptionSignType = inputADO.IsOptionSignType;
                this.reload = inputADO.reload;
                //if (inputADO.IsSave)
                //{
                //    this.pdfFileSaveAsBarItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //}
                if (inputADO.IsReject)
                {
                    this.bbtnRejectSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                //if (inputADO.IsExport)
                //{
                //    this.pdfExportFormDataBarItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    this.pdfImportFormDataBarItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //}

                if (inputADO.IsSign)
                {
                    this.bbtnSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnConfigSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnSendERM.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnPatientSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnSignAndDeskcription.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnRelativeHomeSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnPatientSign.Enabled = true;
                }

                if (inputADO.IsSignConfig.HasValue)
                {
                    this.bbtnConfigSign.Visibility = inputADO.IsSignConfig.Value ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                    this.bbtnConfigBussinessMenu1.Visibility = inputADO.IsSignConfig.Value ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    this.bbtnConfigBussinessMenu1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                bool isSignParanel = false;
                if (!String.IsNullOrEmpty(inputADO.DocumentTypeCode))
                {
                    try
                    {
                        inputADO.DocumentTypeCode = string.Format("{0:00}", inputADO.DocumentTypeCode);
                    }
                    catch (Exception exx)
                    {
                        Logging.LogSystem.Warn(exx);
                    }

                    new EmrDocumentType().DocumentTypeProperty(inputADO.DocumentTypeCode, ref isMultiSignByType, ref isSignParanel, ref DocumentTypeId);
                    this.isMultiSign = this.isMultiSignByType;
                    if (inputADO.IsMultiSign.HasValue)
                    {
                        this.isMultiSign = this.isMultiSign && inputADO.IsMultiSign.Value;
                    }
                }

                this.currentDocument = (!String.IsNullOrEmpty(inputADO.DocumentCode)) ? GenerateByDocumentCode(inputADO.DocumentCode) : null;

                if (this.currentDocument != null && !String.IsNullOrEmpty(this.currentDocument.DocumentCode))
                {
                    if (inputADO.IsPrint)
                    {
                        this.bbtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        if (inputADO.IsEnableButtonPrint.HasValue)
                        {
                            this.bbtnPrint.Enabled = inputADO.IsEnableButtonPrint.Value;
                        }
                    }

                    this.bbtnAttackMentsMenu1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnAttackMentsMenu1.Enabled = true;
                    this.bbtnConfigBussinessMenu2.Enabled = false;
                    this.bbtnSendERM.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    this.bbtnListSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                    isSignParanel = this.currentDocument.IsSignParallel.HasValue ? this.currentDocument.IsSignParallel.Value : false;
                    this.bbtnchkSignParanel.Enabled = false;

                    var configs = GlobalStore.EmrConfigs;
                    if (configs != null && configs.Count > 0)
                    {
                        var cfgViewPACSUrlFormats = configs.Where(o => o.KEY == EmrConfigKeys.EMR_VIEW_PACS_URL_FORMAT);
                        var cfgViewPACSUrlFormat = cfgViewPACSUrlFormats != null ? cfgViewPACSUrlFormats.FirstOrDefault() : null;
                        if (cfgViewPACSUrlFormat != null)
                        {
                            this.vlViewPACSUrlFormat = !String.IsNullOrEmpty(cfgViewPACSUrlFormat.VALUE) ? cfgViewPACSUrlFormat.VALUE : cfgViewPACSUrlFormat.DEFAULT_VALUE;
                            if (!String.IsNullOrEmpty(this.vlViewPACSUrlFormat))
                            {
                                this.btnViewPACSImage.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            }
                        }
                    }
                }
                else
                {
                    this.bbtnchkSignParanel.Enabled = isSignParanel && inputADO.IsSign;

                    this.bbtnPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnPrint.Enabled = true;
                }

                if (inputADO.IsSign)
                    this.bbtnChkCloseAfterSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                this.bbtnchkSignParanel.Checked = isSignParanel;

                if (this.isCloseAfterSign.HasValue && this.isCloseAfterSign.Value)
                {
                    bbtnChkCloseAfterSign.Checked = true;
                }

                this.bbtnSignEnd.Visibility = (this.isMultiSign ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);
                this.bbtnSignEnd.Enabled = false;

                dockPanel1.AllowDrop = false;
                txtSignDescriptionList.AllowDrop = false;

                if (GlobalStore.EMR_SIGN_BOARD__OPTION == "2")
                {
                    this.barCheckUsingSignPad.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                if (GlobalStore.EMR_SIGN_SIGN_DESCRIPTION_INFO == "2")
                {
                    bbtnSignAndDeskcription.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    if (dockPanel1.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Visible)
                        dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                }
                else
                {
                    bbtnSignAndDeskcription.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                }

                if (!String.IsNullOrEmpty(inputADO.BusinessCode))
                {
                    this.bbtnConfigSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    if (inputADO.SignerConfigs != null && inputADO.SignerConfigs.Count > 0)
                    {
                        inputADO.SignerConfigs = inputADO.SignerConfigs.OrderBy(o => o.NumOrder).ToList();
                        this.signers = new EmrSigner().Get().OrderBy(o => o.USERNAME).ThenBy(o => o.NUM_ORDER).ToList();
                        this.listSign = new List<SignTDO>();

                        foreach (var scf in inputADO.SignerConfigs)
                        {
                            SignTDO sign = new SignTDO();
                            EMR_SIGNER signerFind = GetSignerByLoginname(scf.Loginname);
                            if (signerFind != null)
                            {
                                sign.SignerId = signerFind.ID;
                                sign.Loginname = signerFind.LOGINNAME;
                                sign.Username = signerFind.USERNAME;
                                sign.FullName = signerFind.USERNAME;
                                sign.FirstName = signerFind.USERNAME;
                                if (scf.NumOrder > 0)
                                    sign.NumOrder = scf.NumOrder;
                                else
                                    sign.NumOrder = GetMaxNumOrder();
                                sign.Title = signerFind.TITLE;
                                sign.DepartmentCode = signerFind.DEPARTMENT_CODE;
                                sign.DepartmentName = signerFind.DEPARTMENT_NAME;
                                sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                                this.listSign.Add(sign);
                            }
                        }

                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listSign), listSign));
                    }
                }

                if (this.bbtnConfigBussinessMenu1.Visibility == DevExpress.XtraBars.BarItemVisibility.Never && this.bbtnAttackMentsMenu1.Visibility == DevExpress.XtraBars.BarItemVisibility.Never)
                {
                    VisibleButonOrther(false);
                }
                else
                {
                    VisibleButonOrther(true);
                }

                this.pdfViewer1.NavigationPaneInitialVisibility = DevExpress.XtraPdfViewer.PdfNavigationPaneVisibility.Hidden;
                this.pdfViewer1.NavigationPaneVisibility = DevExpress.XtraPdfViewer.PdfNavigationPaneVisibility.Hidden;

                isInitForm = false;
            }
            if (GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "3")
            {
                isUsingSignPad = true;
                barCheckUsingSignPad.Checked = true;
            }
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.2");
        }

        internal UCViewer(string inputFile, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode)
            : this(inputFile, FileType.Pdf, inputADO, signer, treatment, tokenCode, null, false)
        {
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.Nam.3");
        }

        internal UCViewer(string inputFile, FileType fileType, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode, Action<string> _actionAfterSigned, bool _isSignNow, bool _isPrintDocSignedNow = false, Action<bool> _dlgCloseAfterSign = null)
            : this(inputADO, signer, treatment, tokenCode)
        {
            this.fileType = fileType;
            this.isSignNow = _isSignNow;
            this.isPrintDocSignedNow = _isPrintDocSignedNow;
            this.actionAfterSigned = _actionAfterSigned;
            this.closeAfterSign = _dlgCloseAfterSign;
            if (!String.IsNullOrEmpty(inputFile))
            {
                if (fileType != FileType.Json && fileType != FileType.Xml)
                {
                    string ext = Utils.GetExtByFileType(fileType);
                    Utils.ProcessFileInput(inputFile, ext, ref inputFileWork);
                    this.ProcessCommentKey();
                    this.ProcessStoreCurrentFileToPrint(this.inputFileWork);
                    this.readerWorking = new PdfReader(this.inputFileWork);
                }
                EnableSignButton(inputADO.IsSign);
                if (this.readerWorking != null)
                {
                    ProcessSignPdf();
                }
            }
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.Nam.4");
        }

        internal UCViewer(Stream inputStream, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode)
            : this(inputADO, signer, treatment, tokenCode)
        {
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.Nam.5");
            this.inputStream = inputStream;
            this.readerWorking = new PdfReader(inputStream);
            EnableSignButton(true);
            ProcessSignPdf();
        }

        internal UCViewer(byte[] inputByte, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode)
            : this(inputADO, signer, treatment, tokenCode)
        {
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.Nam.6");
            this.fileType = FileType.Pdf;
            this.inputFileWork = Utils.GenerateTempFileWithin();
            Utils.ByteToFile(inputByte, this.inputFileWork);
            this.ProcessCommentKey();
            this.ProcessStoreCurrentFileToPrint(this.inputFileWork);
            this.readerWorking = new PdfReader(this.inputFileWork);
            EnableSignButton(inputADO.IsSign);
            ProcessSignPdf();
        }

        internal UCViewer(byte[] inputByte, FileType fileType, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode)
            : this(inputADO, signer, treatment, tokenCode)
        {
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.Nam.7");
            if (inputADO.IsUsingSignPad.HasValue && inputADO.IsUsingSignPad.Value)
            {
                isUsingSignPad = true;
                barCheckUsingSignPad.Checked = true;
            }
            if (GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "3" || GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "2")
            {
                isUsingSignPad = true;
                barCheckUsingSignPad.Checked = true;
            }
            this.fileType = fileType;
            string ext = Utils.GetExtByFileType(fileType);
            Utils.ProcessFileInput(inputByte, ext, ref inputFileWork, inputADO.DocumentTypeCode);
            if (fileType != FileType.Json && fileType != FileType.Xml)
            {
                this.ProcessCommentKey();
                this.ProcessStoreCurrentFileToPrint(this.inputFileWork);
                this.readerWorking = new PdfReader(this.inputFileWork);
            }

            EnableSignButton(inputADO.IsSign);
            if (this.readerWorking != null)
            {
                ProcessSignPdf();
            }
        }

        internal UCViewer(byte[] inputByte, FileType fileType, InputADO inputADO, EMR_SIGNER signer, EMR_TREATMENT treatment, string tokenCode, Action<string> _actionAfterSigned, bool _isSignNow, bool _isPrintDocSignedNow = false, Action<bool> _dlgCloseAfterSign = null)
            : this(inputByte, fileType, inputADO, signer, treatment, tokenCode)
        {
            Inventec.Common.Logging.LogSystem.Debug("UCViewer.InitializeComponent.Nam.8");
            this.isSignNow = _isSignNow;
            this.isPrintDocSignedNow = _isPrintDocSignedNow;
            this.actionAfterSigned = _actionAfterSigned;
            this.closeAfterSign = _dlgCloseAfterSign;
            if (inputADO.IsUsingSignPad.HasValue && inputADO.IsUsingSignPad.Value)
            {
                isUsingSignPad = true;
                barCheckUsingSignPad.Checked = true;
            }
            if (GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "3" || GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "2")
            {
                isUsingSignPad = true;
                barCheckUsingSignPad.Checked = true;
            }
        }

        private void UCViewer1_Load(object sender, EventArgs e)
        {
            try
            {
                string vlState = CacheClientWorker.GetValue();
                if (!String.IsNullOrEmpty(vlState))
                {
                    Inventec.Common.Logging.LogSystem.Info("Nguoi dung da luu lai trang thai cua lua chon khi vao th nguoi ky thieu anh chu ky & cau hinh EMR.EMR_SIGN.SIGN_DISPLAY_OPTION = 2" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => vlState), vlState));
                }
                ProcessBussinessCFG();
                ProcessMemoryUsageuser();
                Disposed += DisposeVariable;

                this.currentPageSettings = PdfDocumentProcess.GetPaperSize(currentFileWorking);

                InitSignByDocument();

                // Handle the QueryPageSettings event to customize settings for a page to be printed.
                pdfViewer1.QueryPageSettings += OnQueryPageSettings;
                pdfViewer1.Paint += pdfViewer1_Paint;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        internal void UpdateExtFileType(FileADO _fileADOMain, FileADO _fileADOJson, FileADO _fileADOXml)
        {
            this.fileADOMain = _fileADOMain;
            this.fileADOJson = _fileADOJson;
            this.fileADOXml = _fileADOXml;
        }

        internal void DisposeVariable(object sender, EventArgs e)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("DisposeVariable.1");
                this.resultFileStore = "";
                this.currentFileWorking = "";
                this.signAutoPositionADOs = null;
                try
                {
                    if (this.currentStream != null)
                    {
                        Utils.DisposeStream(this.currentStream);
                    }
                }
                catch { }

                try
                {
                    if (this.inputStream != null)
                    {
                        Utils.DisposeStream(this.inputStream);
                    }
                }
                catch { }

                this.startPosition = null;
                this.endPosition = null;
                this.verifiers = null;
                if (this.readerWorking != null)
                    this.readerWorking.Close();
                this.readerWorking = null;
                this.treatment = null;
                this.printTypeBusinessCodes = null;

                this.resultFileStore = "";
                this.currentFileWorking = "";
                this.inputFileWork = "";

                this.treatmentCode = null;
                this.signName = null;
                this.signReason = null;
                this.documentName = null;
                this.businessCode = null;
                this.dlgOpenModuleConfig = null;
                this.currentDocument = null;
                this.inputADOWorking = null;
                this.listSign = null;
                this.signSelected = null;
                this.signSelectedByUser = null;
                this.TokenCode = null;
                this.rejectReason = null;
                this.hisCode = null;
                this.signPositionADOs = null;
                this.nextSignPosition = null;
                this.signers = null;
                this.dlgSendResultSigned = null;
                this.dlgChoosePoint = null;
                this.dlgOptionSignTypeCheckedChanged = null;
                this.dlgCloseAfterSignCheckedChanged = null;
                this.closeAfterSign = null;
                pdfViewer1.QueryPageSettings -= OnQueryPageSettings;
                this.penDrawSignal.Dispose();
                if (image != null)
                    image.Dispose();

                Inventec.Common.Logging.LogSystem.Debug("DisposeVariable.2");
                this.Dispose(true);
                Inventec.Common.Logging.LogSystem.Debug("DisposeVariable.3");
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void VisibleButonOrther(bool visible)
        {
            bbtnConfigBussinessMenu1.Visibility = (visible ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);
            bbtnAttackMentsMenu1.Visibility = (visible ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);
        }

        private void InitSignByDocument()
        {
            try
            {
                EmrDocumentTypeFilter emr = new EmrDocumentTypeFilter();
                if (currentDocument != null && this.currentDocument.DocumentTypeId > 0)
                {
                    emr.ID = this.currentDocument.DocumentTypeId;
                }
                if (inputADOWorking != null && !string.IsNullOrEmpty(inputADOWorking.DocumentTypeCode))
                {
                    emr.DOCUMENT_TYPE_CODE__EXACT = inputADOWorking.DocumentTypeCode;
                }
                var patientMustTypeDocument = new EmrDocumentType().Get(emr);
                if (patientMustTypeDocument != null && patientMustTypeDocument.Count > 0)
                {
                    IsAddPatientSign = patientMustTypeDocument.FirstOrDefault().PATIENT_MUST_SIGN == 1;
                }

                if (this.currentDocument != null && !String.IsNullOrEmpty(this.currentDocument.DocumentCode) && this.inputADOWorking.IsSign)
                {
                    this.signSelectedByUser = new EmrSign().GetSignDocumentFirst(this.currentDocument.DocumentCode, Signer, Treatment, this.isMultiSign, true);
                    this.signSelected = new EmrSign().GetSignDocumentFirst(this.currentDocument.DocumentCode, Signer, Treatment, this.isMultiSign, true);
                    if (signSelected == null && this.inputADOWorking.IsShowPatientSign)
                    {
                        this.signSelected = new EmrSign().GetSignDocumentFirst(this.currentDocument.DocumentCode, null, Treatment, this.isMultiSign, true);
                    }
                    this.bbtnSignEnd.Enabled = (this.signSelected != null && this.signSelected.IS_SIGNING == 1);
                    if (this.signSelected != null && this.signSelected.FLOW_ID > 0)
                    {
                        this.bbtnConfigSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    }
                }
                UpdateAfterAddSignThread(this.listSign);
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void OnQueryPageSettings(object sender, DevExpress.Pdf.PdfQueryPageSettingsEventArgs e)
        {
            try
            {
                //'set current print page size to defined parameters  
                //e.PageSettings.PaperSize = this.currentPageSettings.PaperSize;
                Widths = (int)e.PageSize.Width;
                Heights = (int)e.PageSize.Height;
                currentPageSettings.PaperSize.Width = (int)e.PageSize.Width;
                currentPageSettings.PaperSize.Height = (int)e.PageSize.Height;

            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        void ProcessMemoryUsageuser()
        {
            try
            {
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
                //// 1. Obtain the current application process
                //Process currentProcess = Process.GetCurrentProcess();
                ////currentProcess.Refresh();
                //// 2. Obtain the used memory by the process
                //long memoryUsageusers = currentProcess.PrivateMemorySize64 / (1024 * 1024);


                //// 3. Display value in the terminal output
                ////Console.WriteLine(usedMemory);

                //long memoryUsageusersByGC = GC.GetTotalMemory(true) / (1024 * 1024);
                //Inventec.Common.Logging.LogSystem.Debug("ProcessMemoryUsageuser: Dung luong RAM phan mem dang su dung: " + memoryUsageusers + "Mbs|GC:" + memoryUsageusersByGC);
                ////long memoryUsageusers = (GC.GetTotalMemory(true) - startBytes) / (1024 * 1024);
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        void ProcessStoreCurrentFileToPrint(string filename)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.inputADOWorking.DocumentCode) && !this.inputADOWorking.IsSign)
                {
                    var document = new EmrDocument().GetByCode(this.inputADOWorking.DocumentCode);
                    if (document != null && document.ATTACHMENT_COUNT > 0)
                    {
                        string joinFileResult = Utils.GenerateTempFileWithin();
                        var lastVersionSigned = new EmrVersion().GetSignedDocumentLast(document.ID);
                        var streamSource = FssFileDownload.GetFile(document.LAST_VERSION_URL);
                        streamSource.Position = 0;
                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("đây là dữ liệu: " + Inventec.Common.Logging.LogUtil.GetMemberName(() => streamSource.Length), streamSource.Length));
                        List<string> joinStreams = new List<string>();

                        CommonParam paramCommon = new CommonParam();
                        EmrAttachmentFilter filter = new EmrAttachmentFilter();
                        filter.DOCUMENT_ID = document.ID;
                        filter.ORDER_DIRECTION = "DESC";
                        filter.ORDER_FIELD = "ID";
                        List<EMR_ATTACHMENT> apiResult = new EmrAttachment().Get(filter);
                        if (apiResult != null && apiResult.Count > 0)
                        {
                            joinStreams = apiResult.Select(o => o.URL).ToList();
                            PdfDocumentProcess.InsertPage(streamSource, joinStreams, joinFileResult);

                            filename = joinFileResult;

                            this.inputFileWork = joinFileResult;
                        }
                    }
                }

                this.currentFileWorking = Utils.GenerateTempFileWithin();
                File.Copy(filename, this.currentFileWorking, true);
            }
            catch (Exception ex1)
            {
                Logging.LogSystem.Warn(ex1);
            }
        }

        void ProcessCommentKey()
        {
            try
            {
                if (inputADOWorking != null && inputADOWorking.IsSign)
                {
                    string outFile = Utils.GenerateTempFileWithin();
                    PdfCommentKeyProcess pdfCommentKeyProcess = new PdfCommentKeyProcess();
                    var signPosition1s = pdfCommentKeyProcess.Run(this.inputFileWork, ref outFile);
                    if (signPosition1s != null && signPosition1s.Count > 0 && !String.IsNullOrEmpty(outFile) && File.Exists(outFile))
                    {
                        try
                        {
                            File.Delete(this.inputFileWork);
                        }
                        catch { }
                        this.inputFileWork = outFile;
                        if (this.signPositionADOs == null)
                        {
                            this.signPositionADOs = new List<SignPositionADO>();
                        }
                        this.signPositionADOs.AddRange(signPosition1s);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        void ProcessBussinessCFG()
        {
            try
            {
                if (String.IsNullOrEmpty(inputADOWorking.BusinessCode) && inputADOWorking.IsAutoChooseBusiness.HasValue && inputADOWorking.IsAutoChooseBusiness.Value)
                {
                    bbtnConfigBussinessMenu_ItemClick(null, null);
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private EMR.EFMODEL.DataModels.EMR_SIGNER GetSignerByLoginname(string loginname)
        {
            return this.signers != null ? this.signers.FirstOrDefault(o => o.LOGINNAME == loginname) : null;
        }

        long GetMaxNumOrder()
        {
            long max = frmSignerAdd.stepNumOrder;
            if (this.listSign != null && this.listSign.Count > 0)
            {
                max = this.listSign.Max(o => o.NumOrder) + frmSignerAdd.stepNumOrder;
            }
            return max;
        }

        long? GetDocumentTypeId(string code)
        {
            long? rs = null;
            try
            {
                var dt = new EmrDocumentType().GetByCode(code);
                if (dt != null)
                {
                    rs = dt.ID;
                }
                else
                {
                    Logging.LogSystem.Warn("Ma loai van ban truyen vao khong hop le. DocumentTypeCode = " + code);
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
            return rs;
        }

        DocumentTDO GenerateByDocumentCode(string documentCode)
        {
            DocumentTDO documentTDO = new DocumentTDO();
            try
            {
                var doc = new EmrDocument().GetByCode(documentCode);
                documentTDO.DocumentCode = doc.DOCUMENT_CODE;
                documentTDO.DocumentName = doc.DOCUMENT_NAME;
                documentTDO.DocumentTypeId = doc.DOCUMENT_TYPE_ID;
                documentTDO.TreatmentCode = doc.TREATMENT_CODE;
                documentTDO.OriginalVersion = new VersionTDO();
                documentTDO.OriginalVersion.DocumentCode = doc.DOCUMENT_CODE;
                documentTDO.HisCode = doc.HIS_CODE;
                documentTDO.DocumentGroupId = doc.DOCUMENT_GROUP_ID;
                documentTDO.DocumentTime = doc.DOCUMENT_TIME;
                documentTDO.IsCapture = doc.IS_CAPTURE == 1;
                documentTDO.IsSignParallel = doc.IS_SIGN_PARALLEL == 1;
                documentTDO.MergeCode = doc.MERGE_CODE;
                documentTDO.DependentCode = doc.DEPENDENT_CODE;
                documentTDO.ParentDependentCode = doc.PARENT_DEPENDENT_CODE;
                documentTDO.AttachmentCount = doc.ATTACHMENT_COUNT;
                documentTDO.PaperName = doc.PAPER_NAME;
                documentTDO.RawKind = doc.RAW_KIND;
                documentTDO.Width = doc.WIDTH;
                documentTDO.Height = doc.HEIGHT;

                this.isMultiSign = (doc.IS_MULTI_SIGN == 1);
            }
            catch
            {
                documentTDO = null;
            }

            return documentTDO;
        }

        private void EnableSignButton(bool enable)
        {
            enable = inputADOWorking.IsShowPatientSign ? true : enable;
            this.bbtnSign.Enabled = enable;
            this.bbtnPatientSign.Enabled = enable;
            this.bbtnConfigSign.Enabled = enable;
            this.bbtnSendERM.Enabled = enable;
            this.bbtnRejectSign.Enabled = enable;
            this.bbtnRelativeHomeSign.Enabled = enable;
            this.bbtnSignAndDeskcription.Enabled = enable;
        }

        private void EnableSignButton(bool enableSign, bool enableConfigSign, bool enableSendERM, bool enableRejectSign, bool enablePrint, bool enableSignEnd)
        {
            this.bbtnSign.Enabled = enableSign;
            this.bbtnPatientSign.Enabled = enableSign;
            this.bbtnRelativeHomeSign.Enabled = enableSign;
            this.bbtnConfigSign.Enabled = enableConfigSign;
            this.bbtnSendERM.Enabled = enableSendERM;
            this.bbtnRejectSign.Enabled = enableRejectSign;
            this.bbtnPrint.Enabled = enablePrint;
            this.bbtnSignEnd.Enabled = enableSignEnd;
            this.bbtnSignAndDeskcription.Enabled = enableSign;

            this.bbtnSignEnd.Visibility = (this.isMultiSign ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);
            if (this.bbtnPrint.Visibility == DevExpress.XtraBars.BarItemVisibility.Never)
                this.bbtnPrint.Visibility = (enablePrint ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);
        }

        private void CancelSign()
        {
            try
            {
                this.typeDisplayOption = -1;
                this.isSigning = false;
                this.bbtnPatientSign.Caption = "Bệnh nhân Ký";
                this.bbtnRelativeHomeSign.Caption = "Người nhà ký";
                this.bbtnSign.Caption = "Ký";
                this.pdfViewer1.MouseDown -= pdfViewer1_MouseDown;
                this.pdfViewer1.MouseMove -= pdfViewer1_MouseMove;
                this.pdfViewer1.MouseUp -= pdfViewer1_MouseUp;
                //this.pdfViewer1.Paint -= pdfViewer1_Paint;
                this.startPosition = null;
                this.endPosition = null;
                this.pdfViewer1.Cursor = Cursors.Hand;
                this.pdfViewer1.Refresh();
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private string GetOutputNameSafe(string file)
        {
            var ext = Path.GetExtension(file);
            var path = Path.GetDirectoryName(file);
            var fileNameWithoutExtensioni = Path.GetFileNameWithoutExtension(file);

            string result;
            var num = string.Empty;
            var cnt = 0;
            while (File.Exists(result = path + "\\" + fileNameWithoutExtensioni + "-signed" + num + ext))
            {
                cnt++;
                num = "(" + cnt + ")";
            }
            return result;
        }

        private void ProcessPositionSigned()
        {
            try
            {
                if (isPatientSign)
                {
                    this.signPositionADOs = Utils.GetPdfPatientSignPosition(this.readerWorking);
                    if (this.signPositionADOs != null && this.signPositionADOs.Count > 0)
                    {
                        this.nextSignPosition = signPositionADOs.FirstOrDefault();
                        this.hasNextSignPosition = (this.nextSignPosition != null);
                        this.signAutoPositionADOs = this.hasNextSignPosition ? this.signPositionADOs.Where(o => o.Text == this.nextSignPosition.Text).ToList() : null;
                    }
                    if (signAutoPositionADOs != null && signAutoPositionADOs.Count > 0)
                        return;
                }
                this.signPositionADOs = Utils.GetPdfSignPosition(this.readerWorking);
                this.signedCount = Utils.GetSignedCount(this.readerWorking);
                if (this.signPositionADOs != null && this.signPositionADOs.Count > 0)
                {
                    this.signPositionADOs = this.signPositionADOs.OrderBy(o => VerifySign.GetNumOderByCommentText(o.Text)).ToList();

                    //gán bệnh nhân, người nhà thứ tự -1 để luôn chọn vị trí ký.
                    int nextNum = !this.isPatientSign && !this.isHomeRelativeSign ? 0 : -1;

                    var positions = this.signPositionADOs.Where(o => VerifySign.GetNumOderByCommentText(o.Text) == VerifySign.GetNumOrderBySignOrDefault(signSelectedByUser, Signer, Treatment, this.listSign, this.isPatientSign, this.isHomeRelativeSign, nextNum)).ToList();
                    this.nextSignPosition = (positions != null && positions.Count > 0) ? positions.FirstOrDefault() : null;
                    this.hasNextSignPosition = (this.nextSignPosition != null);
                    this.signAutoPositionADOs = this.hasNextSignPosition ? this.signPositionADOs.Where(o => o.Text == this.nextSignPosition.Text).ToList() : null;
                }
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.signedCount), this.signedCount) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => hasNextSignPosition), hasNextSignPosition) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nextSignPosition), nextSignPosition));
            }
            catch (Exception ex)
            {
                this.nextSignPosition = null;
                this.hasNextSignPosition = false;
                this.signAutoPositionADOs = null;
                Logging.LogSystem.Warn(ex);
            }
        }

        private void ProcessSignPdf()
        {
            string messError = "";
            if (!this.isPrintOnlyContent)
                VerifyPdfInputFile(ref messError);

            isShowImage = false;
            xImg = 0;
            yImg = 0;
            int pageCount = 1;
            iTextSharp.text.Rectangle pageSize;
            pageCount = this.readerWorking.NumberOfPages;
            pageSize = this.readerWorking.GetPageSizeWithRotation(this.readerWorking.NumberOfPages);
            totalPageNumber = this.readerWorking.NumberOfPages;

            ////////////////////////
            string outputPdfPathTemp = Utils.GenerateTempFileWithin();
            string outputPdfPath = "";
            ProcessInsertSignInformationPage(outputPdfPathTemp, ref outputPdfPath, ref pageCount);

            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.currentFileWorking), this.currentFileWorking));

            String temFileWaterMark = Utils.GenerateTempFileWithin();

            List<EMR_SIGN> signAlls = (this.currentDocument != null && !String.IsNullOrEmpty(this.currentDocument.DocumentCode)) ? new EmrSign().GetSignDocumentForDocument(currentDocument) : null;
            currentEmrDocument = null;

            try
            {
                if (this.currentDocument != null && !String.IsNullOrEmpty(this.currentDocument.DocumentCode) && signAlls != null && signAlls.Count > 0)
                {
                    currentEmrDocument = new EmrDocument().GetViewByCode(this.currentDocument.DocumentCode);
                    if (currentEmrDocument == null)
                    {
                        currentEmrDocument = new V_EMR_DOCUMENT() { DOCUMENT_CODE = this.currentDocument.DocumentCode, DOCUMENT_NAME = this.currentDocument.DocumentName ?? this.documentName, TREATMENT_CODE = this.currentDocument.TreatmentCode ?? this.treatmentCode, CREATE_TIME = Utils.GetTimeNow() };
                    }
                    WaterMarkProcess.ProcessInsertWaterMark(this.readerWorking, temFileWaterMark, currentEmrDocument, signAlls, (!this.isPrintOnlyContent && this.verifiers != null && this.verifiers.Count > 0), ref txtSignDescriptionList);
                }
                else// if (GlobalStore.PrintUsingWaterMark)
                {
                    currentEmrDocument = new V_EMR_DOCUMENT() { CREATE_TIME = Utils.GetTimeNow() };
                    if (this.currentDocument != null)
                    {
                        currentEmrDocument.DOCUMENT_CODE = this.currentDocument.DocumentCode;
                        currentEmrDocument.DOCUMENT_NAME = this.currentDocument.DocumentName ?? this.documentName;
                        currentEmrDocument.TREATMENT_CODE = this.currentDocument.TreatmentCode ?? this.treatmentCode;
                    }
                    WaterMarkProcess.ProcessInsertWaterMark(this.readerWorking, temFileWaterMark, currentEmrDocument, null, false, ref txtSignDescriptionList);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }


            try
            {
                this.readerWorking.Close();
            }
            catch { }

            if (File.Exists(temFileWaterMark))
            {
                this.currentFileWorking = Utils.GenerateTempFileWithin();
                File.Copy(temFileWaterMark, this.currentFileWorking, true);
            }
            else if (!String.IsNullOrEmpty(outputPdfPath) && File.Exists(outputPdfPath))
            {
                this.currentFileWorking = outputPdfPath;
            }
            else
            {

            }

            if (String.IsNullOrEmpty(temFileWaterMark) || !File.Exists(temFileWaterMark))
            {
                temFileWaterMark = this.currentFileWorking;
            }
            this.pdfViewer1.DetachStreamAfterLoadComplete = true;
            this.pdfViewer1.LoadDocument(temFileWaterMark);
            this.timerLoadSinglePage.Start();
            try
            {
                if (File.Exists(temFileWaterMark)) File.Delete(temFileWaterMark);
            }
            catch (Exception ex1)
            {
                Logging.LogSystem.Warn(ex1);
            }

            try
            {
                if (File.Exists(outputPdfPath)) File.Delete(outputPdfPath);
            }
            catch { }

            try
            {
                if (File.Exists(outputPdfPathTemp)) File.Delete(outputPdfPathTemp);
            }
            catch { }
        }

        private void ProcessInsertSignInformationPage(string outputPdfPathTemp, ref string outputPdfPath, ref int pageCount)
        {
            try
            {
                if (!this.isPrintOnlyContent && this.verifiers != null && this.verifiers.Count > 0)
                {
                    FileStream fsTemp = File.Open(outputPdfPathTemp, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    Document doc = new Document(this.readerWorking.GetPageSizeWithRotation(this.readerWorking.NumberOfPages));
                    PdfWriter writer = PdfWriter.GetInstance(doc, fsTemp);
                    doc.Open();
                    //adding my table
                    PdfPTable t = AddPdfPTable();
                    doc.Add(t);
                    doc.Close();

                    var pages = new List<int>();
                    for (int i = 0; i <= this.readerWorking.NumberOfPages; i++)
                    {
                        pages.Add(i);
                    }
                    outputPdfPath = Utils.GenerateTempFileWithin();
                    currentStream = File.Open(outputPdfPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    var pdfConcat = new PdfConcatenate(currentStream);

                    PdfReader pdfReader = null;
                    if (!String.IsNullOrEmpty(this.inputFileWork))
                    {
                        pdfReader = new PdfReader(this.inputFileWork);
                    }
                    else
                    {
                        pdfReader = new PdfReader(this.inputStream);
                    }

                    pdfReader.SelectPages(pages);
                    pdfConcat.AddPages(pdfReader);
                    pdfReader.Close();

                    pdfReader = new PdfReader(outputPdfPathTemp);
                    pdfReader.SelectPages(new List<int>() { 0, 1 });
                    pdfConcat.AddPages(pdfReader);
                    //pdfReader.Close();

                    try
                    {
                        fsTemp.Close();
                        fsTemp.Dispose();
                    }
                    catch { }

                    try
                    {
                        pdfReader.Close();
                    }
                    catch { }

                    try
                    {
                        pdfConcat.Close();
                    }
                    catch { }

                    try
                    {
                        this.readerWorking.Close();
                    }
                    catch { }

                    try
                    {
                        if (File.Exists(outputPdfPathTemp))
                            File.Delete(outputPdfPathTemp);
                    }
                    catch { }

                    this.readerWorking = new PdfReader(outputPdfPath);

                    pageCount += 1;
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private bool VerifyPdfInputFile(ref string message)
        {
            VerifyPdfFileHandle verifyPdfFile = new VerifyPdfFileHandle();
            try
            {
                this.verifiers = verifyPdfFile.verify(readerWorking).OrderBy(o => o.Date).ToList();
            }
            catch { this.verifiers = null; }

            if (this.verifiers == null)
            {
                message = "File cần xác thực không hợp lệ";
                return false;
            }
            else if (this.verifiers.Count == 0)
            {
                message = "File đã được chọn không tìm thấy chữ ký số";
                return false;
            }

            bool fileStatus = true;
            String reason = "";

            for (int i = this.verifiers.Count; i > 0; i--)
            {
                VerifierADO verify = this.verifiers[i - 1];
                if (verify != null)
                {
                    if (verify.Modified)
                    {
                        fileStatus = false;
                        if (reason.Length == 0)
                        {
                            reason = "Chữ ký số không hợp lệ.";
                        }
                        else
                        {
                            reason += "\nChữ ký số không hợp lệ.";
                        }
                    }

                    if (!verify.Valid)
                    {
                        fileStatus = false;
                        if (verify.InvalidReasonList != null && verify.InvalidReasonList.Count > 0)
                        {
                            foreach (InvalidReasonADO invalidReason in verify.InvalidReasonList)
                            {
                                if (reason.Length == 0)
                                {
                                    reason = invalidReason.ReasonVnLang;
                                }
                                else
                                {
                                    if (reason.IndexOf(invalidReason.ReasonVnLang) != -1)
                                    {
                                        reason += "\n" + invalidReason.ReasonVnLang;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (reason.Length == 0)
                            {
                                reason = "File chứa CTS không hợp lệ.";
                            }
                            else
                            {
                                reason += "\n" + "File chứa CTS không hợp lệ.";
                            }
                        }
                    }
                }
            }

            if (fileStatus)
            {
                message = "Hợp lệ";
            }
            else
            {
                message = reason;
            }
            return fileStatus;
        }

        private bool VerifyWithExistsDocument(DocumentTDO document, bool checkSigner = true)
        {
            if (document != null && !String.IsNullOrEmpty(document.DocumentCode) && checkSigner)
            {
                if (String.IsNullOrEmpty(inputADOWorking.BusinessCode))
                {
                    EMR_SIGN signVerify = null;
                    if (GlobalStore.EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION == "1" && IsAddPatientSign && (isPatientSign || isHomeRelativeSign))
                    {
                        signVerify = new EmrSign().GetSignDocumentFirst(document.DocumentCode, (isPatientSign || isHomeRelativeSign) ? null : Signer, Treatment, this.isMultiSign, true);
                    }
                    else
                        signVerify = this.signSelectedByUser != null ? this.signSelectedByUser : new EmrSign().GetSignDocumentFirst(document.DocumentCode, (isPatientSign || isHomeRelativeSign) ? null : Signer, Treatment, this.isMultiSign, true);

                    if (signVerify == null || signVerify.ID == 0 || (GlobalStore.EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION == "1" && IsAddPatientSign && (isPatientSign || isHomeRelativeSign) && string.IsNullOrEmpty(signVerify.PATIENT_CODE)))
                    {
                        MessageManager.Show(MessageUitl.GetMessage(MessageConstan.PhaiTaoLuongKyChoVanBanDaCoTrenHeThong));
                        Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.PhaiTaoLuongKyChoVanBanDaCoTrenHeThong));
                        if (this.dlgOpenModuleConfig != null)
                            this.dlgOpenModuleConfig(document);

                        return false;
                    }
                }
            }

            this.ProcessPositionSigned();

            return true;
        }

        private bool VerifySignPad()
        {
            bool valid = true;
            try
            {
                if (this.isUsingSignPad && GlobalStore.EMR_SIGN_BOARD__OPTION == "2"
                && (new Inventec.Common.SignLibrary.SignBoard.SignBoardUseBehavior(null, null).IsProcessOpen("Inventec.SignPadManager")
                || new Inventec.Common.SignLibrary.FingerPrint.FingerPrintUseBehavior(null, null).IsProcessOpen("Inventec.FingerPrintManager")))
                {
                    valid = false;
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }

            return valid;
        }

        private PdfPTable AddPdfPTable()
        {
            PdfPTable t = new PdfPTable(7);
            t.SetTotalWidth(new float[] { 7f, 20f, 20f, 15f, 30f, 30f, 30f });

            iTextSharp.text.Font fContentNormal1 = new iTextSharp.text.Font(Utils.GetBaseFont(), 9, iTextSharp.text.Font.NORMAL);

            iTextSharp.text.Font fContentBold1 = new iTextSharp.text.Font(Utils.GetBaseFont(), 9, iTextSharp.text.Font.BOLD);

            //One row added

            PdfPCell cell21 = new PdfPCell();
            cell21.AddElement(new Paragraph("STT", fContentBold1));
            t.AddCell(cell21);

            PdfPCell cell22 = new PdfPCell();
            cell22.AddElement(new Paragraph("Người ký", fContentBold1));
            t.AddCell(cell22);

            PdfPCell cell23 = new PdfPCell();
            cell23.AddElement(new Paragraph("Thời gian ký", fContentBold1));
            t.AddCell(cell23);

            PdfPCell cell24 = new PdfPCell();
            cell24.AddElement(new Paragraph("Hạn CT", fContentBold1));
            t.AddCell(cell24);

            PdfPCell cell25 = new PdfPCell();
            cell25.AddElement(new Paragraph("Đơn vị", fContentBold1));
            t.AddCell(cell25);

            PdfPCell cell25a = new PdfPCell();
            cell25a.AddElement(new Paragraph("Chức danh", fContentBold1));
            t.AddCell(cell25a);

            PdfPCell cell26 = new PdfPCell();
            cell26.AddElement(new Paragraph("Ý kiến của người ký", fContentBold1));
            t.AddCell(cell26);

            int stt = 1;
            if (this.verifiers != null && this.verifiers.Count > 0)
                foreach (var dr in this.verifiers)
                {
                    PdfPCell c = new PdfPCell();
                    c.AddElement(new Chunk((stt.ToString()), fContentNormal1));
                    t.AddCell(c);

                    PdfPCell c1 = new PdfPCell();
                    c1.AddElement(new Chunk((dr.SignerName), fContentNormal1));
                    t.AddCell(c1);

                    PdfPCell c2 = new PdfPCell();
                    c2.AddElement(new Chunk(dr.Date.ToString("dd/MM/yyyy HH:mm:ss"), fContentNormal1));
                    t.AddCell(c2);

                    PdfPCell c3 = new PdfPCell();
                    c3.AddElement(new Chunk((dr.NotAfter.ToString("dd/MM/yyyy")), fContentNormal1));
                    t.AddCell(c3);

                    string donvi = "", chucdanh = "";
                    if (!String.IsNullOrEmpty(dr.Location))
                    {
                        var larr = dr.Location.Split(new string[] { "|" }, StringSplitOptions.None);
                        if (larr.Length == 2)
                        {
                            donvi = larr[0];
                            chucdanh = larr[1];
                        }
                    }

                    PdfPCell c4 = new PdfPCell();
                    c4.AddElement(new Chunk(donvi, fContentNormal1));
                    t.AddCell(c4);

                    PdfPCell c4a = new PdfPCell();
                    c4a.AddElement(new Chunk(chucdanh, fContentNormal1));
                    t.AddCell(c4a);

                    PdfPCell c5 = new PdfPCell();
                    c5.AddElement(new Chunk(dr.Comment, fContentNormal1));
                    t.AddCell(c5);

                    //Add raw data sign
                    stt += 1;
                }
            return t;
        }

        private bool SignDigital(float _x, float _y, int _pageNumberCurrent, int _totalPageNumber, DisplayConfigDTO displayConfigDTO, bool? isMultiSignForProcess = null)
        {
            bool success = false;
            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => _x), _x) + "____"
                + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => _y), _y) + "____"
                + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => _pageNumberCurrent), _pageNumberCurrent) + "____"
                + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => _totalPageNumber), _totalPageNumber) + "____"
                + "SignDigital:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => Signer.LOGINNAME), Signer.LOGINNAME)
                + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => Treatment.TREATMENT_CODE), Treatment.TREATMENT_CODE)
                + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfigDTO), displayConfigDTO)
                + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForProcess), isMultiSignForProcess));

            WaitingManager.Show();
            if (!bbtnSign.Enabled || !bbtnPatientSign.Enabled)
            {
                this.CancelSign();
                return false;
            }

            if (this.dlgChoosePoint != null)
                this.dlgChoosePoint(_x, _y);

            if ((this.startPosition != null && this.endPosition != null) || this.nextSignPosition != null || this.fileType == FileType.Xml || this.fileType == FileType.Json)
            {
                SignHandle signProcessor;
                CommonParam param = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFileWork), inputFileWork));

                if (!String.IsNullOrEmpty(this.inputFileWork))
                {
                    signProcessor = new SignHandle(this.inputFileWork, _x, _y, _pageNumberCurrent, _totalPageNumber, this.CancelSign, this.dlgOpenModuleConfig, this.documentName, this.treatmentCode, this.signName, this.signReason, this.verifiers, this.signType, this.listSign, this.signSelected, this.isPatientSign, this.isHomeRelativeSign, this.DocumentTypeId, (isMultiSignForProcess.HasValue ? isMultiSignForProcess.Value : this.isMultiSign), this.hisCode, this.inputADOWorking, displayConfigDTO, param, this.ParentForm, GetCheckSignParanel(), Treatment, Signer, TokenCode);
                }
                else
                {
                    signProcessor = new SignHandle(this.inputStream, _x, _y, _pageNumberCurrent, _totalPageNumber, this.CancelSign, this.dlgOpenModuleConfig, this.documentName, this.treatmentCode, this.signName, this.signReason, this.verifiers, this.signType, this.listSign, this.signSelected, this.isPatientSign, this.isHomeRelativeSign, this.DocumentTypeId, (isMultiSignForProcess.HasValue ? isMultiSignForProcess.Value : this.isMultiSign), this.hisCode, this.inputADOWorking, displayConfigDTO, param, this.ParentForm, GetCheckSignParanel(), Treatment, Signer, TokenCode);
                }

                string messageErr = "";

                if (!signProcessor.VerifyFile(this.currentDocument, this.signedCount, ref messageErr))
                {
                    WaitingManager.Hide();
                    this.CancelSign();
                    return false;
                }

                string outputFile = this.resultFileStore;
                try
                {
                    if (this.currentDocument == null)
                        this.currentDocument = new DocumentTDO();

                    signProcessor.SetUsingSignPad(this.isUsingSignPad);
                    signProcessor.SetFileType(this.fileType);

                    currentDocument.OriginalVersion = new VersionTDO();
                    if (fileADOMain != null && !String.IsNullOrEmpty(fileADOMain.Base64FileContent))
                        currentDocument.OriginalVersion.Base64Data = fileADOMain.Base64FileContent;
                    if (fileADOXml != null && !String.IsNullOrEmpty(fileADOXml.Base64FileContent))
                        currentDocument.OriginalVersion.Base64DataXml = fileADOXml.Base64FileContent;
                    if (fileADOJson != null && !String.IsNullOrEmpty(fileADOJson.Base64FileContent))
                        currentDocument.OriginalVersion.Base64DataJson = fileADOJson.Base64FileContent;

                    success = signProcessor.SignFile(this.currentDocument, ref outputFile);
                    Inventec.Common.Logging.LogSystem.Info("SignDigital__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile));
                    if (success)
                    {
                        if (dlgSendResultSigned != null)
                        {
                            dlgSendResultSigned(new DocumentSignedUpdateIGSysResultDTO() { DocumentCode = currentDocument.DocumentCode });
                        }
                        try
                        {
                            if (this.readerWorking != null) this.readerWorking.Close();
                            if (File.Exists(this.inputFileWork)) File.Delete(this.inputFileWork);
                        }
                        catch { }


                        this.ProcessDependentCodeAfterSigned();

                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isCloseAfterSign), isCloseAfterSign)
                               + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForProcess), isMultiSignForProcess));

                        if (this.isSignNow && this.actionAfterSigned != null)
                        {
                            WaitingManager.Hide();
                            this.actionAfterSigned(outputFile);

                            return success;
                        }
                        else if (isCloseAfterSign.HasValue && isCloseAfterSign.Value && closeAfterSign != null && (isMultiSignForProcess.HasValue ? isMultiSignForProcess.Value : false) == false)
                        {
                            WaitingManager.Hide();
                            this.closeAfterSign(success);

                            return success;
                        }

                        this.inputFileWork = outputFile;
                        try
                        {
                            if (inputStream != null)
                            {
                                inputStream.Flush();
                                inputStream.Dispose();
                                inputStream = null;
                            }
                        }
                        catch (Exception exx)
                        {
                            Logging.LogSystem.Warn(exx);
                        }
                        this.ProcessStoreCurrentFileToPrint(outputFile);
                        if (this.fileType != FileType.Xml && this.fileType != FileType.Json)
                        {
                            this.readerWorking = new PdfReader(this.inputFileWork);
                            this.ProcessSignPdf();

                            this.currentPageSettings = PdfDocumentProcess.GetPaperSize(this.inputFileWork);

                            this.pdfViewer1.CurrentPageNumber = _pageNumberCurrent + plusNumberWithShowingSignInformation;

                            if ((!this.hasNextSignPosition || this.nextSignPosition == null) && this.signedCount > 0)
                            {
                                int x = (int)((_x / (pdfViewer1.ZoomFactor * 0.01)) - (pdfViewer1.ClientRectangle.Width / (pdfViewer1.ZoomFactor * 0.01) / 2));
                                int y = (int)((_y / (pdfViewer1.ZoomFactor * 0.01)) - (pdfViewer1.ClientRectangle.Height / (pdfViewer1.ZoomFactor * 0.01) / 2));

                                this.pdfViewer1.ScrollHorizontal(x);
                                this.pdfViewer1.ScrollVertical(y);
                            }
                            else
                            {
                                this.pdfViewer1.HorizontalScrollPosition = _x;
                                this.pdfViewer1.VerticalScrollPosition = _y;
                            }
                        }

                        this.EnableSignButton((isMultiSignForProcess.HasValue ? isMultiSignForProcess.Value : this.isMultiSign), true, false, false, true, (isMultiSignForProcess.HasValue ? isMultiSignForProcess.Value : this.isMultiSign));
                        if ((isMultiSignForProcess.HasValue ? isMultiSignForProcess.Value : this.isMultiSign))
                        {
                            this.signSelected = new EmrSign().GetSignDocumentFirst(this.currentDocument.DocumentCode, ((this.isPatientSign || this.isHomeRelativeSign) ? null : Signer), Treatment, (isMultiSignForProcess.HasValue ? isMultiSignForProcess.Value : this.isMultiSign), false);
                            bbtnSignEnd.Enabled = (this.signSelected != null && this.signSelected.IS_SIGNING == 1);
                        }

                        if (this.bbtnConfigBussinessMenu1.Visibility == DevExpress.XtraBars.BarItemVisibility.Never && this.bbtnAttackMentsMenu1.Visibility == DevExpress.XtraBars.BarItemVisibility.Never)
                        {
                            VisibleButonOrther(false);
                            //bbtnOther.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        }
                        else
                        {
                            VisibleButonOrther(true);
                            //bbtnOther.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                    }
                    else
                    {
                        //Rollback data file
                        try
                        {
                            //if (File.Exists(outputFile)) File.Delete(outputFile);
                        }
                        catch { }

                        try
                        {
                            //if (File.Exists(this.inputFileWork)) File.Delete(this.inputFileWork);
                        }
                        catch { }

                        try
                        {
                            //if (File.Exists(this.temFile1)) File.Delete(this.temFile1);
                        }
                        catch { }
                    }

                    this.UpdateStateIGSys(success ? (((isMultiSignForProcess.HasValue ? isMultiSignForProcess.Value : this.isMultiSign) || (listSign != null && listSign.Count > 1)) ? SignStateCode.DOCUMENT_HAS_SIGN_CONFIG_UN_FINAL : SignStateCode.SUCCESS) : SignStateCode.FAIL);
                    this.CancelSign();
                    WaitingManager.Hide();
                }
                catch (Exception ex)
                {
                    Logging.LogSystem.Warn(ex);
                    WaitingManager.Hide();
                    MessageBox.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    this.CancelSign();
                }
            }
            return success;
        }

        /// <summary>
        /// Sửa thư viện ký: Sau khi gọi thực hiện ký thành công:
        ///- Kiểm tra văn bản có trường mã phụ thuộc (DependentCode) không.
        ///- Nếu không có thì không làm gì.
        ///- Nếu có thì gọi api get văn bản có trường mã phụ thuộc cha (ParentDependentCode) bằng mã phụ thuộc của văn bản vừa ký (DependentCode) và cùng hồ sơ điều trị. Chỉ lấy văn bản chưa bị xóa (IS_DELETE = 0), chưa bị từ chối ký (REJECTER IS NULL), không bị ký nguội thất bại (COUNT_RESIGN_FAILED NULL hoặc = 0).
        ///- Nếu không lấy được văn bản nào thì không làm gì.
        ///- Nếu lấy được nhiều hơn 1 văn bản thì show ds văn bản vừa lấy được cho người dùng chọn để ký:
        ///+ Tiêu đề "Ký văn bản phụ thuộc" .
        ///+ 1 lable: "Vui lòng chọn văn bản phụ thuộc để thực hiện ký".
        ///+ Danh sách gồm: Mã văn bản, tên văn bản, Loại văn bản, người tạo, tg tạo.
        ///+ Nếu không chọn thì không làm gì. Nếu chọn thì thực hiện bước tiếp tiếp theo.
        ///- Nếu có 1 văn bản thì thực hiện ký văn bản đấy:
        ///+ Nếu người ký của văn bản được chọn không phải là người dùng thì không thực hiện gì và thông báo "Ký văn bản phụ thuộc (Mã văn bản - Tên văn bản) thất bại. Không phải lượt ký của bản."
        ///+ Nếu văn bản có thiết lập vị trí ký thì thực hiện ký luôn.
        ///+ Nếu văn bản không thiết lập vị trí ký thì show văn bản cho người dùng chọn vị trí ký.
        ///+ Trường hợp ký văn bản phụ thuộc thất bại thì show thông báo "Ký văn bản phụ thuộc (Mã văn bản - Tên văn bản) thất bại. Lý do lỗi."
        ///
        ///4. Sửa Mps000020, Mps000019:
        ///- Bổ sung key DEPATE_ID giá trị là ID của Biên bản hội chẩn (HIS_DEPATE).
        ///P/S: Để thực hiện tự động ký được trích biên bản hội chẩn (Mps000019) Khi thực hiện ký Biên bản hội chẩn (Mps000020). Thì cần thực hiện:
        ///- Mps000019: Ánh xạ key DEPATE_ID của mps sang cột ParentDependentCode của văn bản EMR.
        ///- Mps000020: Ánh xạ key DEPATE_ID của mps sang cột DependentCode của văn bản EMR.
        /// </summary>
        private void ProcessDependentCodeAfterSigned()
        {
            try
            {
                if (this.currentDocument != null && !String.IsNullOrEmpty(this.currentDocument.DocumentCode) && !String.IsNullOrEmpty(this.currentDocument.DependentCode))
                {
                    string loginname = this.Signer != null ? this.Signer.LOGINNAME : null;
                    var documentDependents = new EmrDocument().GetDocumentDependent(this.currentDocument.DependentCode, this.currentDocument.TreatmentCode, loginname);
                    if (documentDependents != null && documentDependents.Count > 0)
                    {
                        if (documentDependents.Count == 1)
                        {
                            if (this.currentDocument.DocumentCode == documentDependents[0].DOCUMENT_CODE)
                                return;
                            ProcessSignOneDocumentDependent(documentDependents[0]);
                        }
                        else
                        {
                            //Show form chọn văn bản
                            frmChooseDocumentDependent frmChooseDocumentDependent = new frmChooseDocumentDependent(ProcessSignOneDocumentDependent, documentDependents, "");
                            frmChooseDocumentDependent.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void ProcessSignOneDocumentDependent(V_EMR_DOCUMENT doc)
        {
            try
            {
                if (doc != null && ((isPatientSign || isHomeRelativeSign) ? doc.PATIENT_CODE == Treatment.PATIENT_CODE : (((doc.IS_SIGN_PARALLEL ?? 0) != 1 && doc.NEXT_SIGNER == Signer.LOGINNAME) || (doc.IS_SIGN_PARALLEL == 1 && !String.IsNullOrEmpty(doc.UN_SIGNERS) && (String.Format(",{0},", doc.UN_SIGNERS)).Contains(String.Format(",{0},", Signer.LOGINNAME))))))
                {
                    EMR.EFMODEL.DataModels.EMR_VERSION version = new EmrVersion().GetSignedDocumentLast(doc.ID);
                    if (version != null && !String.IsNullOrWhiteSpace(version.URL))
                    {
                        //goi tool view
                        String temFile = Path.GetTempFileName();
                        temFile = temFile.Replace(".tmp", ".pdf");
                        using (MemoryStream stream = FssFileDownload.GetFile(version.URL))
                        {
                            if (stream != null)
                            {
                                using (var fileStream = new FileStream(temFile, FileMode.Create, FileAccess.Write))
                                {
                                    stream.CopyTo(fileStream);
                                }
                            }
                            else
                            {
                                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                            }
                        }

                        SignLibraryGUIProcessor libraryProcessor = new SignLibraryGUIProcessor();

                        InputADO inputADO = new InputADO();
                        inputADO.DTI = inputADOWorking.DTI;
                        inputADO.IsSave = false;
                        inputADO.IsSign = true;
                        inputADO.IsReject = false;
                        inputADO.IsPrint = false;
                        inputADO.IsExport = false;
                        inputADO.IsPrintOnlyContent = inputADOWorking.IsPrintOnlyContent;
                        inputADO.SignType = inputADOWorking.SignType;
                        inputADO.Treatment = inputADOWorking.Treatment;
                        inputADO.DocumentCode = doc.DOCUMENT_CODE;
                        inputADO.DocumentName = doc.DOCUMENT_NAME;
                        inputADO.DlgOpenModuleConfig = inputADOWorking.DlgOpenModuleConfig;
                        inputADO.RoomCode = inputADOWorking.RoomCode;
                        inputADO.RoomName = inputADOWorking.RoomName;
                        inputADO.RoomTypeCode = inputADOWorking.RoomTypeCode;
                        inputADO.DlgCloseAfterSign = ProcessDlgCloseAfterSign;
                        inputADO.IsCloseAfterSign = true;
                        inputADO.DepartmentCode = inputADOWorking.DepartmentCode;
                        inputADO.DepartmentName = inputADOWorking.DepartmentName;
                        inputADO.DependentCode = inputADOWorking.DependentCode;
                        inputADO.ParentDependentCode = inputADOWorking.ParentDependentCode;
                        inputADO.IsOptionSignType = inputADOWorking.IsOptionSignType;
                        inputADO.DlgChangeOptionSignType = inputADOWorking.DlgChangeOptionSignType;
                        inputADO.PaperSizeDefault = inputADOWorking.PaperSizeDefault;
                        inputADO.PrinterDefault = inputADOWorking.PrinterDefault;

                        if (!String.IsNullOrWhiteSpace(temFile) && File.Exists(temFile))
                            libraryProcessor.SignNow(Utils.FileToBase64String(temFile), FileType.Pdf, inputADO);
                        else
                        {
                            MessageBox.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                        }

                        if (File.Exists(temFile)) File.Delete(temFile);
                    }
                    else
                    {
                        MessageBox.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    }
                }
                else
                {
                    MessageBox.Show(String.Format(MessageUitl.GetMessage(MessageConstan.KyVanBanPhuThocThatBai__KhongPhaiLuotKyCuaBan), doc.DOCUMENT_CODE, doc.DOCUMENT_NAME));
                    Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => doc.NEXT_SIGNER), doc.NEXT_SIGNER)
                          + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => Signer.LOGINNAME), Signer.LOGINNAME)
                          + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => Treatment.PATIENT_CODE), Treatment.PATIENT_CODE)
                          + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isPatientSign), isPatientSign)
                          + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isHomeRelativeSign), isHomeRelativeSign));
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        void ProcessDlgCloseAfterSign(bool success)
        {
            this.ParentForm.Close();
        }

        private void UpdateStateIGSys(string code)
        {
            try
            {
                DocumentSignedUpdateIGSysResultDTO dataSigned = new DocumentSignedUpdateIGSysResultDTO();
                dataSigned.DocumentName = documentName;
                dataSigned.DocumentCode = this.currentDocument != null ? this.currentDocument.DocumentCode : "";
                dataSigned.DocumentTypeCode = inputADOWorking.DocumentTypeCode;
                dataSigned.HisCode = inputADOWorking.HisCode;
                dataSigned.TREATMENT_CODE = treatmentCode;
                dataSigned.NgayKy = (string)(Inventec.Common.Integrate.DateTimeConvert.SystemDateTimeToTimeNumber(DateTime.Now) + "");
                dataSigned.NguoiKy = this.signName;
                dataSigned.MaLoi = code;
                dataSigned.token = !String.IsNullOrEmpty(TokenCode) ? TokenCode : (GlobalStore.TokenData != null ? GlobalStore.TokenData.TokenCode : GlobalStore.TokenCode);

                if (this.dlgSendResultSigned != null)
                {
                    this.dlgSendResultSigned(dataSigned);
                }

                if (!String.IsNullOrEmpty(GlobalStore.INTERGRATE_SYS_BASE_URI))
                {
                    CommonParam paramIG = new CommonParam();
                    IGDocumentState igDocumentState = new IGDocumentState(paramIG);
                    igDocumentState.SendSignedInfoToIGSys(dataSigned);
                }
            }
            catch (Exception exx)
            {
                Logging.LogSystem.Warn(exx);
            }
        }

        private void UpdateAfterAddSignThread(List<SignTDO> signs)
        {
            try
            {
                this.listSign = signs;
                if (GlobalStore.EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION == "1" && IsAddPatientSign)
                {
                    this.bbtnchkSignParanel.Checked = this.bbtnchkSignParanel.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        private void UpdateRejectInfo(string rejectReason)
        {
            this.rejectReason = rejectReason;
        }

        internal DocumentTDO GetCurrentDocument()
        {
            return currentDocument;
        }

        internal void Print()
        {
            try
            {
                if (GlobalStore.OptionPrintType == OptionPrintType.PdfAposeLib)
                {
                    PrintAposeLib();
                }
                //else if (GlobalStore.OptionPrintType == OptionPrintType.CallExeLib && PrintLibProcess.ValidExistsExecutePrintCallExeService())
                //{
                //    PrintExeServiceLib();
                //}
                else
                {
                    PrintDevLib();
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        void PrintExeServiceLib()
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("PrintExeServiceLib.1");
                bool printSuccess = false;
                string fileToPrint = "";
                if (File.Exists(this.currentFileWorking))//this.inputFileWork
                {
                    Inventec.Common.Logging.LogSystem.Debug("PrintExeServiceLib.2");
                    fileToPrint = this.currentFileWorking;
                    Inventec.Common.Logging.LogSystem.Debug("Print currentFileWorking exists: " + currentFileWorking);

                }
                else if (File.Exists(this.inputFileWork))
                {
                    Inventec.Common.Logging.LogSystem.Debug("PrintExeServiceLib.3");
                    fileToPrint = this.inputFileWork;
                    Inventec.Common.Logging.LogSystem.Debug("Print currentFileWorking not exists, replace with inputFileWork: " + inputFileWork);
                }

                PrintLibProcess.ExecutePrintCallExeService(fileToPrint, printNumberCopies, inputADOWorking.PrinterDefault, inputADOWorking.PaperSizeDefault);

                Inventec.Common.Logging.LogSystem.Debug("PrintExeServiceLib.4");
                printSuccess = true;
                if (printSuccess && inputADOWorking.ActPrintSuccess != null)
                {
                    Inventec.Common.Logging.LogSystem.Debug("PrintExeServiceLib.5");
                    Inventec.Common.Logging.LogSystem.Debug("inputADOWorking.ActPrintSuccess != null: " + (inputADOWorking.ActPrintSuccess != null));
                    try
                    {
                        inputADOWorking.ActPrintSuccess();
                    }
                    catch (Exception exx)
                    {
                        Inventec.Common.Logging.LogSystem.Warn(exx);
                    }
                }
                Inventec.Common.Logging.LogSystem.Debug("PrintExeServiceLib.6");
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        void PrintAposeLib()
        {
            try
            {
                bool printSuccess = false;
                string fileToPrint = "";
                if (File.Exists(this.currentFileWorking))//this.inputFileWork
                {
                    fileToPrint = this.currentFileWorking;
                    Inventec.Common.Logging.LogSystem.Debug("Print currentFileWorking exists: " + currentFileWorking);

                }
                else if (File.Exists(this.inputFileWork))
                {
                    fileToPrint = this.inputFileWork;
                    Inventec.Common.Logging.LogSystem.Debug("Print currentFileWorking not exists, replace with inputFileWork: " + inputFileWork);
                }
                printSuccess = PrintLibProcess.SimplePrint(fileToPrint, this.printNumberCopies, this.inputADOWorking.PrinterDefault, this.inputADOWorking.PaperSizeDefault);

                if (printSuccess && inputADOWorking.ActPrintSuccess != null)
                {
                    Inventec.Common.Logging.LogSystem.Debug("inputADOWorking.ActPrintSuccess != null: " + (inputADOWorking.ActPrintSuccess != null));
                    try
                    {
                        inputADOWorking.ActPrintSuccess();
                    }
                    catch (Exception exx)
                    {
                        Inventec.Common.Logging.LogSystem.Warn(exx);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        void PrintDevLib()
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("PrintDevLib.1");
                string fileToPrint = "";
                if (File.Exists(this.currentFileWorking))//this.inputFileWork
                {
                    Inventec.Common.Logging.LogSystem.Debug("PrintDevLib.2");
                    fileToPrint = currentFileWorking;
                    Inventec.Common.Logging.LogSystem.Debug("Print currentFileWorking exists: " + currentFileWorking);
                }
                else if (File.Exists(this.inputFileWork))
                {
                    Inventec.Common.Logging.LogSystem.Debug("PrintDevLib.3");
                    fileToPrint = inputFileWork;
                    Inventec.Common.Logging.LogSystem.Debug("Print currentFileWorking not exists, replace with inputFileWork: " + inputFileWork);
                }

                if (PrintLibProcess.SimplePrintDevLib(fileToPrint, printNumberCopies, this.inputADOWorking.PrinterDefault, this.inputADOWorking.PaperSizeDefault))
                {
                    Inventec.Common.Logging.LogSystem.Debug("PrintDevLib.4");
                    if (inputADOWorking.ActPrintSuccess != null)
                    {
                        Inventec.Common.Logging.LogSystem.Debug("inputADOWorking.ActPrintSuccess != null: " + (inputADOWorking.ActPrintSuccess != null));
                        try
                        {
                            inputADOWorking.ActPrintSuccess();
                        }
                        catch (Exception exx)
                        {
                            Inventec.Common.Logging.LogSystem.Warn(exx);
                        }
                    }
                    Inventec.Common.Logging.LogSystem.Debug("PrintDevLib.5");
                }
                Inventec.Common.Logging.LogSystem.Debug("PrintDevLib.6");
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void UpdateNote(string _note)
        {
            this.signReason = _note;
        }

        void ChooseBusinessClick(EMR.EFMODEL.DataModels.EMR_BUSINESS dataBusiness)
        {
            try
            {
                this.businessCode = dataBusiness != null ? dataBusiness.BUSINESS_CODE : "";
                if (!String.IsNullOrEmpty(this.businessCode))
                {
                    bbtnConfigSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    inputADOWorking.BusinessCode = this.businessCode;

                    Inventec.Common.SignLibrary.Api.EmrSignerFlow emrSignerFlow = new Inventec.Common.SignLibrary.Api.EmrSignerFlow();
                    var emrSignerFlowData = Signer != null ? emrSignerFlow.GetView(new EmrSignerFlowViewFilter() { IS_ACTIVE = 1, BUSINESS_CODE__EXACT = this.businessCode, LOGINNAME__EXACT = Signer.LOGINNAME }) : null;
                    if (emrSignerFlowData != null && emrSignerFlowData.Count > 0)
                    {
                        inputADOWorking.RoomCode = emrSignerFlowData.FirstOrDefault().ROOM_CODE;
                        inputADOWorking.RoomTypeCode = emrSignerFlowData.FirstOrDefault().ROOM_TYPE_CODE;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void SignParanelEditValueChanged(bool issignparanel)
        {
            try
            {
                bbtnchkSignParanel.Checked = issignparanel;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        bool GetCheckSignParanel()
        {
            return (bool)bbtnchkSignParanel.Checked;
        }

        private void pdfViewer1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!pdfViewer1.IsDocumentOpened) return;

                isShowRangtax = true;
                startPosition = pdfViewer1.GetDocumentPosition(e.Location);
                if (!this.isSelectRangeRectangle)
                    endPosition = pdfViewer1.GetDocumentPosition(new PointF(e.Location.X + lcStep, e.Location.Y + lcStep));

                mouseButtonPressed = true;
                pdfViewer1.Invalidate();
            }
        }

        private void pdfViewer1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!pdfViewer1.IsDocumentOpened) return;

            if (mouseButtonPressed)
            {
                //isShowImage = false;
                pdfViewer1.Cursor = Cursors.Cross;
                if (this.isSelectRangeRectangle)
                {
                    endPosition = pdfViewer1.GetDocumentPosition(e.Location);

                    startPoint1 = pdfViewer1.GetClientPoint(startPosition);
                    endPoint1 = pdfViewer1.GetClientPoint(endPosition);
                    xImg = (startPoint1.X + endPoint1.X) / 2;
                    yImg = (startPoint1.Y + endPoint1.Y) / 2;
                }
                else
                {
                    startPosition = pdfViewer1.GetDocumentPosition(e.Location);
                    endPosition = pdfViewer1.GetDocumentPosition(new PointF(e.Location.X + lcStep, e.Location.Y + lcStep));
                    startPoint1 = pdfViewer1.GetClientPoint(startPosition);
                    endPoint1 = pdfViewer1.GetClientPoint(endPosition);
                    xImg = (startPoint1.X + endPoint1.X) / 2;
                    yImg = (startPoint1.Y + endPoint1.Y) / 2;
                }

                pdfViewer1.Invalidate();
            }
            else
            {
                //isShowImage = true;
                if (this.isSigning)
                {
                    startPosition = pdfViewer1.GetDocumentPosition(e.Location);
                    endPosition = pdfViewer1.GetDocumentPosition(new PointF(e.Location.X + lcStep, e.Location.Y + lcStep));
                    startPoint1 = pdfViewer1.GetClientPoint(startPosition);
                    endPoint1 = pdfViewer1.GetClientPoint(endPosition);
                    xImg = (startPoint1.X + endPoint1.X) / 2;
                    yImg = (startPoint1.Y + endPoint1.Y) / 2;

                    pdfViewer1.Refresh();
                }
            }
        }

        private void pdfViewer1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseButtonPressed = false;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                pdfViewer1.Cursor = Cursors.Hand;
                if (startPosition != null && endPosition != null && isShowRangtax)
                {
                    startPoint1 = pdfViewer1.GetClientPoint(startPosition);
                    endPoint1 = pdfViewer1.GetClientPoint(endPosition);
                    pageNumberCurrent = startPosition.PageNumber;

                    xImg = (float)(startPosition.Point.X + endPosition.Point.X) / 2;
                    yImg = (float)(startPosition.Point.Y + endPosition.Point.Y) / 2;
                }
                float xImgM = 0, yImgM = 0;
                if (yImg > 0 && xImg > 0)
                {
                    xImgM = (float)(xImg);// - ((150) / 2)
                    if (xImgM < 0)
                    {
                        xImgM = 0;
                    }

                    yImgM = (float)(yImg);// - ((60) / 2)
                    if (yImgM < 0)
                    {
                        yImgM = 0;
                    }

                    if ((!this.hasNextSignPosition || this.nextSignPosition == null) && this.signedCount > 0)
                    {
                        Inventec.Common.Logging.LogSystem.Debug("Truong hop khong co toa do ky fix trong file, va so luong chu ky da ky > 0 ==> giam pageNumberCurrent ve dung gia tri trang hien tai (do co them trang ky o dau tien): pageNumberCurrent = pageNumberCurrent - 1.____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => pageNumberCurrent), pageNumberCurrent));
                    }

                    DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault();

                    bool success = SignDigital(xImgM, yImgM, pageNumberCurrent, totalPageNumber, displayConfigParam);
                    if (success)
                    {
                        this.signReason = "";
                        this.txtSignDescription.Text = "";
                    }
                }
            }
        }

        private void pdfViewer1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!pdfViewer1.IsDocumentOpened) return;

                Graphics g = e.Graphics;
                if (startPosition != null && endPosition != null && isShowRangtax && (this.isPatientSign || this.isHomeRelativeSign))
                {
                    startPoint1 = pdfViewer1.GetClientPoint(startPosition);
                    endPoint1 = pdfViewer1.GetClientPoint(endPosition);
                    pageNumberCurrent = startPosition.PageNumber;
                    g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.Aqua)),
                        RectangleF.FromLTRB(Math.Min(startPoint1.X, endPoint1.X), Math.Min(startPoint1.Y, endPoint1.Y),
                        Math.Max(startPoint1.X, endPoint1.X), Math.Max(startPoint1.Y, endPoint1.Y)));

                    xImg = (startPoint1.X + endPoint1.X) / 2;
                    yImg = (startPoint1.Y + endPoint1.Y) / 2;
                }
                if (this.verifiers != null && this.verifiers.Count > 0 && pageNumberCurrent == 1)
                {
                    return;
                }
                float xImgM = 0, yImgM = 0;
                if (yImg > 0 && xImg > 0)
                {
                    if (this.isSigning && !(this.isPatientSign || this.isHomeRelativeSign))
                    {
                        byte[] imageData = null;
                        float SignaltureImageWidth = 0;
                        iTextSharp.text.Image instance = null;
                        int iw = 0;
                        int ih = 0;

                        int typeDisplayOption__Tmp = 0;
                        if (this.typeDisplayOption > 0)
                        {
                            typeDisplayOption__Tmp = this.typeDisplayOption;
                        }
                        else
                        {
                            typeDisplayOption__Tmp = (inputADOWorking.DisplayConfigDTO.TypeDisplay ?? 0);
                        }

                        if (Signer != null && Signer.SIGN_IMAGE != null)
                        {
                            instance = iTextSharp.text.Image.GetInstance(Signer.SIGN_IMAGE);
                            imageData = Signer.SIGN_IMAGE;
                        }
                        else if (File.Exists(Path.Combine(Utils.SignatureFolder(), "NotImage.jpg")))
                        {
                            //image = System.Drawing.Image.FromFile(Path.Combine(Utils.SignatureFolder(), "NotImage.jpg"));
                            instance = iTextSharp.text.Image.GetInstance(Path.Combine(Utils.SignatureFolder(), "NotImage.jpg"));
                            imageData = Utils.FileToByte(Path.Combine(Utils.SignatureFolder(), "NotImage.jpg"));
                        }

                        if (imageData != null && instance != null)
                        {
                            using (var ms = new MemoryStream(imageData))
                            {
                                if (Signer != null && Signer.SIGNALTURE_IMAGE_WIDTH.HasValue && Signer.SIGNALTURE_IMAGE_WIDTH > 0)
                                {
                                    SignaltureImageWidth = (float)Signer.SIGNALTURE_IMAGE_WIDTH.Value;
                                }

                                float plusH = SignPdfAsynchronous.ProcessHeightPlus(100, inputADOWorking.DisplayConfigDTO.WidthRectangle ?? 0);
                                var iWidthPercentage = SharedUtils.CalculateWidthPercent(inputADOWorking.DisplayConfigDTO.WidthRectangle ?? 0, inputADOWorking.DisplayConfigDTO.HeightRectangle ?? 0, instance, SignaltureImageWidth, 100, plusH);
                                float iHeightPercentage = 0;

                                int iw1, ih1;
                                if (SignaltureImageWidth > 0)
                                {
                                    iw1 = (int)(SignaltureImageWidth);
                                    ih1 = (int)((inputADOWorking.DisplayConfigDTO.HeightRectangle ?? 0) * ((SignaltureImageWidth / inputADOWorking.DisplayConfigDTO.WidthRectangle ?? 0)));
                                }
                                else
                                {
                                    iw1 = (int)(inputADOWorking.DisplayConfigDTO.WidthRectangle ?? 0);
                                    ih1 = (int)(inputADOWorking.DisplayConfigDTO.HeightRectangle ?? 0);
                                }

                                Size sizenew, sizenew1;

                                if (typeDisplayOption__Tmp == Constans.DISPLAY_IMAGE_STAMP)
                                {
                                    sizenew = ResizeFit(new Size((int)instance.Width, (int)instance.Height), new Size((int)(iw1) - 10, (int)(ih1) - 10));
                                    sizenew1 = ConstrainVerbose((int)instance.Width, (int)instance.Height, (int)(iw1) - 10, (int)(ih1) - 10);
                                }
                                else
                                {
                                    sizenew = ResizeFit(new Size((int)instance.Width, (int)instance.Height), new Size((int)(iw1) + 10, (int)(ih1) + 10));
                                    sizenew1 = ConstrainVerbose((int)instance.Width, (int)instance.Height, (int)(iw1) + 10, (int)(ih1) + 10);
                                }

                                ih = sizenew.Height;
                                iw = sizenew.Width;

                                image = new Bitmap(new Bitmap(ms), new Size(iw, ih));
                                xImgM = (float)(xImg - ((image.Width) / 2));
                                if (xImgM < 0)
                                {
                                    xImgM = 0;
                                }
                                yImgM = (float)(yImg - ((image.Height) / 2));
                                if (yImgM < 0)
                                {
                                    yImgM = 0;
                                }
                                var r = new RectangleF(xImgM, yImgM, image.Width, image.Height);
                                g.DrawImage(image, r);
                                g.DrawRectangle(penDrawSignal, xImgM, yImgM, image.Width, image.Height);
                            }

                            instance = null;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        Size ConstrainVerbose(int imageWidth, int imageHeight, int maxWidth, int maxHeight)
        {
            // Coalculate the aspect ratios of the image and bounding box
            var maxAspect = (float)maxWidth / (float)maxHeight;
            var aspect = (float)imageWidth / (float)imageHeight;
            // Bounding box aspect is narrower
            if (maxAspect <= aspect && imageWidth > maxWidth)
            {
                // Use the width bound and calculate the height
                return new Size((int)maxWidth, (int)Math.Min(maxHeight, maxWidth / aspect));
            }
            else if (maxAspect > aspect && imageHeight > maxHeight)
            {
                // Use the height bound and calculate the width
                return new Size((int)Math.Min(maxWidth, maxHeight * aspect), (int)maxHeight);
            }
            else
            {
                return new Size(imageWidth, imageHeight);
            }
        }

        private Size ResizeFit(Size originalSize, Size maxSize)
        {
            var widthRatio = (double)maxSize.Width / (double)originalSize.Width;
            var heightRatio = (double)maxSize.Height / (double)originalSize.Height;
            var iAspectRatio = Math.Min(widthRatio, heightRatio);
            //if (iAspectRatio > 1)
            //    return originalSize;           
            return new Size((int)(originalSize.Width * iAspectRatio), (int)(originalSize.Height * iAspectRatio));
        }

        private void pdfViewer1_PageSetupDialogShowing(object sender, DevExpress.XtraPdfViewer.PdfPageSetupDialogShowingEventArgs e)
        {
            try
            {
                e.FormStartPosition = FormStartPosition.CenterScreen;
                int w = 600;
                int h = 400;
                if (Screen.PrimaryScreen != null)
                {
                    w = Screen.PrimaryScreen.WorkingArea.Width > 400 ? Screen.PrimaryScreen.WorkingArea.Width - 400 : 100;
                    h = Screen.PrimaryScreen.WorkingArea.Height > 100 ? Screen.PrimaryScreen.WorkingArea.Height - 100 : 50;
                }
                e.FormSize = new System.Drawing.Size(w, h);

                if (this.printNumberCopies > 1)
                {
                    e.PrinterSettings.Settings.Copies = this.printNumberCopies;
                }

                if (!String.IsNullOrEmpty(this.inputADOWorking.PrinterDefault))
                {
                    e.PrinterSettings.Settings.PrinterName = this.inputADOWorking.PrinterDefault;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void pdfViewer1_PopupMenuShowing(object sender, DevExpress.XtraPdfViewer.PdfPopupMenuShowingEventArgs e)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("pdfViewer1_PopupMenuShowing" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.inputADOWorking.IsPrint), this.inputADOWorking.IsPrint));
                if (this.bbtnPrint.Visibility == DevExpress.XtraBars.BarItemVisibility.Always && this.bbtnPrint.Enabled == true)
                {
                    //Nothing
                    //Continue show menu...
                    e.Menu.BeginUpdate();

                    for (int i = e.Menu.ItemLinks.Count - 1; i >= 0; i--)
                    {
                        if (e.Menu.ItemLinks[i].Caption == "Print..." || e.Menu.ItemLinks[i].Caption == "Print")
                            e.Menu.ItemLinks.Remove(e.Menu.ItemLinks[i]);
                    }

                    e.Menu.ItemLinks.Insert(3, new DevExpress.XtraBars.BarButtonItem());
                    e.Menu.ItemLinks[3].Caption = "Print";
                    e.Menu.ItemLinks[3].Item.Name = "PrintReport";
                    e.Menu.ItemLinks[3].Item.ItemShortcut = new DevExpress.XtraBars.BarShortcut(Keys.Alt | Keys.P);
                    e.Menu.ItemLinks[3].Item.ItemClick += bbtnPrint_ItemClick;
                    e.Menu.ItemLinks[3].Item.Glyph = imageCollection1.Images[0];

                    e.Menu.EndUpdate();
                }
                else
                {
                    for (int i = e.Menu.ItemLinks.Count - 1; i >= 0; i--)
                    {
                        if (e.Menu.ItemLinks[i].Caption == "Print..." || e.Menu.ItemLinks[i].Caption == "Print")
                            e.Menu.ItemLinks.Remove(e.Menu.ItemLinks[i]);
                    }

                    //Stop event, no show menu
                    //e.ItemLinks.Clear();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        /// <summary>
        /// Hàm xử lý kiểm tra giá trị của dữ liệu vị trí tự động ký hoặc cấu hình vị trí thiết lập chung
        /// - Nếu có vị trí tự động ký và giá trị tham số có giá trị thì lấy
        /// - Nếu không có vị trí tự động ký mà có vị trí thiết lập chung(có các tham số như vị trí tự động ký nhưng không có số tứ tự ký) và giá trị tham số có giá trị thì lấy
        /// - Ngược lại thì lấy giá trị theo cấu hình
        /// </summary>
        /// <param name="autoSP"></param>
        /// <returns></returns>
        private DisplayConfigDTO GetDisplayConfigByCommentOrDefault(SignPositionADO autoSP = null)
        {
            SignPositionADO positionNotNumOrder = null;
            if (this.signPositionADOs != null && this.signPositionADOs.Count > 0)
            {
                positionNotNumOrder = this.signPositionADOs.FirstOrDefault(o => VerifySign.GetNumOderByCommentText(o.Text) == 0);
            }

            DisplayConfigDTO configParam = new DisplayConfigDTO()
            {
                HeightRectangle = (
                    autoSP != null && autoSP.HeightRectangle > 0
                    ? autoSP.HeightRectangle :
                    (
                        positionNotNumOrder != null && positionNotNumOrder.HeightRectangle > 0
                        ? positionNotNumOrder.HeightRectangle :
                        inputADOWorking.DisplayConfigDTO != null
                            ? inputADOWorking.DisplayConfigDTO.HeightRectangle
                            : null
                    )
                ),
                WidthRectangle = (
                    autoSP != null && autoSP.WidthRectangle > 0
                    ? autoSP.WidthRectangle :
                    (
                        positionNotNumOrder != null && positionNotNumOrder.WidthRectangle > 0
                        ? positionNotNumOrder.WidthRectangle :
                        inputADOWorking.DisplayConfigDTO != null
                            ? inputADOWorking.DisplayConfigDTO.WidthRectangle
                            : null
                    )
                ),
                SizeFont = (
                    autoSP != null && autoSP.SizeFont > 0
                    ? autoSP.SizeFont :
                    (
                        positionNotNumOrder != null && positionNotNumOrder.SizeFont > 0
                        ? positionNotNumOrder.SizeFont :
                        inputADOWorking.DisplayConfigDTO != null
                            ? inputADOWorking.DisplayConfigDTO.SizeFont
                            : null
                    )
                ),
                TextPosition = (
                    autoSP != null && autoSP.TextPosition > 0
                    ? (int?)autoSP.TextPosition :
                    (
                        positionNotNumOrder != null && positionNotNumOrder.TextPosition > 0
                        ? (int?)positionNotNumOrder.TextPosition :
                        inputADOWorking.DisplayConfigDTO != null
                            ? inputADOWorking.DisplayConfigDTO.TextPosition
                            : null
                    )
                ),
                TypeDisplay = (
                    typeDisplayOption > 0
                    ? typeDisplayOption :
                    (
                        autoSP != null && autoSP.TypeDisplay > 0
                        ? autoSP.TypeDisplay :
                        (
                            positionNotNumOrder != null && positionNotNumOrder.TypeDisplay > 0
                            ? (int?)positionNotNumOrder.TypeDisplay :
                            inputADOWorking.DisplayConfigDTO != null
                                ? inputADOWorking.DisplayConfigDTO.TypeDisplay
                                : null
                        )
                    )
                ),
                IsDisplaySignature = (
                    autoSP != null && autoSP.IsDisplaySignature.HasValue
                    ? autoSP.IsDisplaySignature :
                    (
                        positionNotNumOrder != null && positionNotNumOrder.IsDisplaySignature.HasValue
                        ? positionNotNumOrder.IsDisplaySignature :
                        inputADOWorking.DisplayConfigDTO != null
                            ? inputADOWorking.DisplayConfigDTO.IsDisplaySignature
                            : null
                    )
                ),
                FormatRectangleText = (
                    (inputADOWorking.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADOWorking.DisplayConfigDTO.FormatRectangleText))
                    ? inputADOWorking.DisplayConfigDTO.FormatRectangleText
                    : null
                ),
                Location = (
                    (inputADOWorking.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADOWorking.DisplayConfigDTO.Location))
                    ? inputADOWorking.DisplayConfigDTO.Location
                    : null
                ),
                Alignment = (
                    (inputADOWorking.DisplayConfigDTO != null && inputADOWorking.DisplayConfigDTO.Alignment.HasValue)
                    ? inputADOWorking.DisplayConfigDTO.Alignment
                    : null
                ),
                IsBold = (
                    (inputADOWorking.DisplayConfigDTO != null && inputADOWorking.DisplayConfigDTO.IsBold.HasValue)
                    ? inputADOWorking.DisplayConfigDTO.IsBold
                    : null
                ),
                IsItalic = (
                    (inputADOWorking.DisplayConfigDTO != null && inputADOWorking.DisplayConfigDTO.IsItalic.HasValue)
                    ? inputADOWorking.DisplayConfigDTO.IsItalic
                    : null
                ),
                IsUnderlined = (
                    (inputADOWorking.DisplayConfigDTO != null && inputADOWorking.DisplayConfigDTO.IsUnderlined.HasValue)
                    ? inputADOWorking.DisplayConfigDTO.IsUnderlined
                    : null
                ),
                FontName = (
                    (inputADOWorking.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADOWorking.DisplayConfigDTO.FontName))
                    ? inputADOWorking.DisplayConfigDTO.FontName
                    : null
                )
            };

            return configParam;
        }

        private void bbtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!bbtnPrint.Enabled || this.bbtnPrint.Visibility == DevExpress.XtraBars.BarItemVisibility.Never)
                {
                    return;
                }
                if (pdfViewer1.IsDocumentOpened)
                {
                    Print();
                }
                else
                {
                    MessageBox.Show(MessageUitl.GetMessage(MessageConstan.KhongTimThayVanBanDeIn));
                    Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.KhongTimThayVanBanDeIn));
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
            }
        }

        private void bbtnSendERM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SignHandle signProcessor;
                CommonParam param = new CommonParam();
                if (!String.IsNullOrEmpty(this.inputFileWork))
                {
                    signProcessor = new SignHandle(this.inputFileWork, 0, 0, 0, 0, this.CancelSign, null, this.documentName, this.treatmentCode, this.signName, this.signReason, this.verifiers, this.signType, this.listSign, null, this.isPatientSign, this.isHomeRelativeSign, this.DocumentTypeId, this.isMultiSign, this.hisCode, this.inputADOWorking, null, param, this.ParentForm, GetCheckSignParanel(), Treatment, Signer, TokenCode);
                }
                else
                {
                    signProcessor = new SignHandle(this.inputStream, 0, 0, 0, 0, this.CancelSign, null, this.documentName, this.treatmentCode, this.signName, this.signReason, this.verifiers, this.signType, this.listSign, null, this.isPatientSign, this.isHomeRelativeSign, this.DocumentTypeId, this.isMultiSign, this.hisCode, this.inputADOWorking, null, param, this.ParentForm, GetCheckSignParanel(), Treatment, Signer, TokenCode);
                }
                DocumentTDO document = new DocumentTDO();
                document.IsSignParallel = GetCheckSignParanel();
                signProcessor.SetFileType(this.fileType);
                document.OriginalVersion = new VersionTDO();
                if (fileADOMain != null && !String.IsNullOrEmpty(fileADOMain.Base64FileContent))
                    document.OriginalVersion.Base64Data = fileADOMain.Base64FileContent;
                if (fileADOXml != null && !String.IsNullOrEmpty(fileADOXml.Base64FileContent))
                    document.OriginalVersion.Base64DataXml = fileADOXml.Base64FileContent;
                if (fileADOJson != null && !String.IsNullOrEmpty(fileADOJson.Base64FileContent))
                    document.OriginalVersion.Base64DataJson = fileADOJson.Base64FileContent;

                this.currentDocument = signProcessor.SendDocument(document);
                if (this.currentDocument != null && !String.IsNullOrEmpty(this.currentDocument.DocumentCode))
                {
                    Inventec.Common.Logging.LogSystem.Debug("documentCode: " + this.currentDocument.DocumentCode);
                    this.UpdateStateIGSys((this.listSign != null && this.listSign.Count > 0) ? SignStateCode.DOCUMENT_HAS_SIGN_CONFIG_UN_FINAL : SignStateCode.DOCUMENT_CREATE_NEW);
                    this.listSign = new List<SignTDO>();
                    this.bbtnSendERM.Enabled = false;
                    this.bbtnchkSignParanel.Enabled = false;
                    this.bbtnListSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnAttackMentsMenu1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.bbtnAttackMentsMenu1.Enabled = true;
                    if (String.IsNullOrEmpty(inputADOWorking.BusinessCode))
                    {
                        if (this.currentDocument.Signs != null && this.currentDocument.Signs.Count > 0)
                        {
                            this.signSelected = new EmrSign().GetSignDocumentFirst(this.currentDocument.DocumentCode, Signer, Treatment, this.isMultiSign, false);
                            bbtnSignEnd.Enabled = (this.signSelected != null && this.signSelected.IS_SIGNING == 1);
                            //bbtnRejectSign.Enabled = true;
                            //bbtnRejectSign.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                        else
                        {
                            if (DevExpress.XtraEditors.XtraMessageBox.Show(
                            MessageUitl.GetMessage(MessageConstan.BanCoMuonTaoThemLuongKy),
                            MessageUitl.GetMessage(MessageConstan.ThongBao),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if (this.dlgOpenModuleConfig != null)
                                    this.dlgOpenModuleConfig(this.currentDocument);
                            }
                        }
                    }

                    if (this.bbtnConfigBussinessMenu1.Visibility == DevExpress.XtraBars.BarItemVisibility.Never && this.bbtnAttackMentsMenu1.Visibility == DevExpress.XtraBars.BarItemVisibility.Never)
                    {
                        VisibleButonOrther(false);
                        //bbtnOther.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    }
                    else
                    {
                        VisibleButonOrther(true);
                        //bbtnOther.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }

                    MessageManager.Show(param, true);
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Khong tao duoc documentCode");
                    MessageManager.Show(param, false);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnSign_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                bool success = false;
                this.isPatientSign = false;
                this.isHomeRelativeSign = false;
                this.typeDisplayOption = -1;

                if (this.isSigning)
                {
                    CancelSign();
                    return;
                }

                this.signReason = this.txtSignDescription.Text;

                if (!VerifyWithExistsDocument(this.currentDocument))
                {
                    CancelSign();
                    return;
                }
                bool? vOptionSign = VerifySign.VerifySignImageWithOption(this.inputADOWorking, Signer, this.hasNextSignPosition, this.nextSignPosition, this.isMultiSign);
                if (vOptionSign.HasValue)
                {
                    if (vOptionSign.Value)
                    {
                        this.typeDisplayOption = Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT;
                    }
                    else
                    {
                        CancelSign();
                        return;
                    }
                }

                if (this.hasNextSignPosition && this.nextSignPosition != null && (this.signSelected == null || (this.signSelected != null && (this.signSelected.SIGN_TIME ?? 0) <= 0)))
                {
                    if (this.signAutoPositionADOs != null && this.signAutoPositionADOs.Count > 0)//this.isMultiSign && 
                    {
                        var SignPositionAutoForAdds = this.signAutoPositionADOs.OrderBy(o => o.Text).ToList();
                        bool isMultiSignForAuto = false;
                        if (!isMultiSign && SignPositionAutoForAdds != null && SignPositionAutoForAdds.Count >= 2)
                            isMultiSignForAuto = true;

                        int demKey = 1;
                        foreach (var nSp in SignPositionAutoForAdds)
                        {
                            bool isMultiSignForProcess = isMultiSign;
                            if (isMultiSignForAuto && demKey == SignPositionAutoForAdds.Count)
                            {
                                isMultiSignForProcess = false;
                            }
                            else if (isMultiSignForAuto)
                            {
                                isMultiSignForProcess = true;
                            }
                            else
                            {
                                isMultiSignForProcess = isMultiSign;
                            }

                            Inventec.Common.Logging.LogSystem.Info("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp), nSp)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForProcess), isMultiSignForProcess)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => SignPositionAutoForAdds.Count), SignPositionAutoForAdds.Count)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => demKey), demKey)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForAuto), isMultiSignForAuto));

                            float xs = (nSp.Reactanle.Left);
                            xs = xs < 0 ? 0 : xs;
                            float ys = (nSp.Reactanle.Bottom);
                            ys = ys < 0 ? 0 : ys;

                            DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault(nSp);

                            success = SignDigital(xs, ys, nSp.PageNUm, this.totalPageNumber, displayConfigParam, isMultiSignForProcess);
                            demKey++;
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nextSignPosition), nextSignPosition));
                        float xs = (this.nextSignPosition.Reactanle.Left);//FIX// - 150 / 2
                        xs = xs < 0 ? 0 : xs;
                        float ys = (this.nextSignPosition.Reactanle.Bottom);//FIX// - 60 / 2
                        ys = ys < 0 ? 0 : ys;
                        DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault(this.nextSignPosition);
                        success = SignDigital(xs, ys, this.nextSignPosition.PageNUm, this.totalPageNumber, displayConfigParam);
                    }

                    if (success)
                    {
                        this.signReason = "";
                        this.txtSignDescription.Text = "";
                    }
                }
                else
                {
                    if (this.fileType == FileType.Json || this.fileType == FileType.Xml)
                    {
                        DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault();

                        success = SignDigital(1, 1, 1, 1, displayConfigParam);
                        if (success)
                        {
                            this.signReason = "";
                            this.txtSignDescription.Text = "";
                        }
                    }
                    else
                    {
                        this.isSigning = true;
                        this.bbtnSign.Caption = "Hủy";
                        pdfViewer1.MouseDown += pdfViewer1_MouseDown;
                        pdfViewer1.MouseMove += pdfViewer1_MouseMove;
                        pdfViewer1.MouseUp += pdfViewer1_MouseUp;
                        //pdfViewer1.Paint += pdfViewer1_Paint;
                        pdfViewer1.Cursor = Cursors.Cross;
                    }
                }
                Inventec.Common.Logging.LogSystem.Debug("bbtnSign_ItemClick____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success));

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        /// <summary>
        ///  Nghiệp vụ nút "Bệnh nhân ký":
        ///- Nghiệp vụ check đúng sai thứ tự ký hay chọn điểm ký xử lý như hiện tại.
        ///- Nếu bệnh nhân không có số thẻ KCB (CARD_CODE trong EMR_TREATMENT): Chặn và thông báo "Bệnh nhân không có số thẻ KCB.".
        ///- Nếu bệnh nhân có số thẻ KCB thì thực hiện gọi WCF qua phần mềm thẻ truyền vào số thẻ của bệnh nhân.
        ///- Nếu pm thẻ trả kết quả xác thực thất bại -> Thông báo
        ///- Nếu kết quả pm thẻ trả về là xác thực thành công thì sẽ có 2 trường hợp:
        ///+ Nếu pm thẻ trả về báo thẻ bệnh nhân ký là thẻ định danh -> thực hiện gọi api Emr ký HSM như hiện tại.
        ///+ Nếu pm thẻ trả về báo thẻ bệnh nhân ký là thẻ không định danh:
        ///-> gọi api emr thực hiện đánh dấu bệnh nhân đã ký (lưu điểm ký và đánh dấu bệnh nhân ký điện tử (IS_SIGN_ELECTRONIC = 1) vào EMR_SIGN của bệnh nhân).
        ///-> Khi hiển thị văn bản mà có bệnh nhân ký điện tử -> hiển thị vào điểm bệnh nhân ký 2 thông tin "Tên bệnh nhân - Đã ký".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbtnPatientSign_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                bool success = false;
                this.isPatientSign = true;
                this.isHomeRelativeSign = false;
                if (this.isSigning)
                {
                    CancelSign();
                }
                else
                {
                    if (!VerifyWithExistsDocument(this.currentDocument, GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION != "3" || GlobalStore.EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION == "1"))
                    {
                        return;
                    }
                    if (!VerifySignPad())
                    {
                        return;
                    }

                    this.signReason = this.txtSignDescription.Text;

                    if (this.hasNextSignPosition && this.nextSignPosition != null && (this.signSelected == null || (this.signSelected != null && (this.signSelected.SIGN_TIME ?? 0) <= 0)))
                    {
                        if (this.signAutoPositionADOs != null && this.signAutoPositionADOs.Count > 0)
                        {
                            var SignPositionAutoForAdds = this.signAutoPositionADOs.OrderBy(o => o.Text).ToList();
                            bool isMultiSignForAuto = false;
                            if (!isMultiSign && SignPositionAutoForAdds != null && SignPositionAutoForAdds.Count >= 2)
                                isMultiSignForAuto = true;

                            int demKey = 1;
                            foreach (var nSp in SignPositionAutoForAdds)
                            {
                                bool isMultiSignForProcess = isMultiSign;
                                if (isMultiSignForAuto && demKey == SignPositionAutoForAdds.Count)
                                {
                                    isMultiSignForProcess = false;
                                }
                                else if (isMultiSignForAuto)
                                {
                                    isMultiSignForProcess = isMultiSignForAuto;
                                }
                                else
                                {
                                    isMultiSignForProcess = isMultiSign;
                                }

                                Inventec.Common.Logging.LogSystem.Debug("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp), nSp)
     + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForProcess), isMultiSignForProcess)
     + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => SignPositionAutoForAdds.Count), SignPositionAutoForAdds.Count)
     + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => demKey), demKey)
     + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForAuto), isMultiSignForAuto));

                                float xs = (nSp.Reactanle.Left);
                                xs = xs < 0 ? 0 : xs;
                                float ys = (nSp.Reactanle.Bottom);
                                ys = ys < 0 ? 0 : ys;

                                DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault(nSp);
                                success = SignDigital(xs, ys, nSp.PageNUm, this.totalPageNumber, displayConfigParam, isMultiSignForProcess);
                                demKey++;
                            }
                        }
                        else
                        {
                            Inventec.Common.Logging.LogSystem.Debug("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nextSignPosition), nextSignPosition));
                            float xs = (this.nextSignPosition.Reactanle.Left);//FIX// - 150 / 2
                            xs = xs < 0 ? 0 : xs;
                            float ys = (this.nextSignPosition.Reactanle.Bottom);//FIX// - 60 / 2
                            ys = ys < 0 ? 0 : ys;

                            DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault(this.nextSignPosition);
                            success = SignDigital(xs, ys, this.nextSignPosition.PageNUm, this.totalPageNumber, displayConfigParam);
                        }

                        if (success)
                        {
                            this.signReason = "";
                            this.txtSignDescription.Text = "";
                        }
                    }
                    else
                    {
                        if (this.fileType == FileType.Json || this.fileType == FileType.Xml)
                        {
                            DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault();

                            success = SignDigital(1, 1, 1, 1, displayConfigParam);
                            if (success)
                            {
                                this.signReason = "";
                                this.txtSignDescription.Text = "";
                            }
                        }
                        else
                        {
                            this.isSigning = true;
                            this.bbtnPatientSign.Caption = "Hủy";
                            pdfViewer1.MouseDown += pdfViewer1_MouseDown;
                            pdfViewer1.MouseMove += pdfViewer1_MouseMove;
                            pdfViewer1.MouseUp += pdfViewer1_MouseUp;
                            //pdfViewer1.Paint += pdfViewer1_Paint;
                            pdfViewer1.Cursor = Cursors.Cross;
                        }
                    }
                    if (success && this.reload != null) this.reload();

                }
                //Inventec.Common.Logging.LogSystem.Debug("bbtnPatientSign_ItemClick____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
            }
        }

        /// <summary>
        /// b. Nghiệp vụ nút "Người nhà ký":
        ///- Nghiệp vụ check đúng sai thứ tự ký hay chọn điểm ký xử lý như hiện tại (Check như ký bệnh nhân).
        ///- Thực hiện gọi WCF qua phần mềm thẻ (Vẫn dùng Service cho ký bệnh nhân bổ sung thêm đầu vào đánh dấu là người nhà ký - IsHomieSign).
        ///- Nếu pm thẻ trả kết quả xác thực thất bại -> Thông báo
        ///- Nếu kết quả pm thẻ trả về là xác thực thành công, tên và quan hệ của người ký với bệnh nhân (cần lưu thông tin vào EMR_SIGN) thì sẽ có 2 trường hợp:
        ///+ Nếu pm thẻ trả về báo thẻ người nhà bệnh nhân ký là thẻ định danh -> thực hiện gọi api Emr ký HSM như hiện tại.
        ///+ Nếu pm thẻ trả về báo thẻ người nhà bệnh nhân ký là thẻ không định danh:
        ///-> gọi api emr thực hiện đánh dấu bệnh nhân đã ký (lưu điểm ký và đánh dấu bệnh nhân ký điện tử (IS_SIGN_ELECTRONIC = 1) vào EMR_SIGN của bệnh nhân).
        ///-> Khi hiển thị văn bản mà có bệnh nhân ký điện tử -> hiển thị vào điểm bệnh nhân ký 2 thông tin "Tên bệnh nhân - Đã ký".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbtnRelativeHomeSign_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                bool success = false;
                this.isHomeRelativeSign = true;
                this.isPatientSign = false;
                if (this.isSigning)
                {
                    CancelSign();
                }
                else
                {
                    if (!VerifyWithExistsDocument(this.currentDocument, GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION != "3" || GlobalStore.EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION == "1"))
                    {
                        return;
                    }

                    this.signReason = this.txtSignDescription.Text;

                    if (this.hasNextSignPosition && this.nextSignPosition != null && (this.signSelected == null || (this.signSelected != null && (this.signSelected.SIGN_TIME ?? 0) <= 0)))
                    {
                        if (this.signAutoPositionADOs != null && this.signAutoPositionADOs.Count > 0)
                        {
                            var SignPositionAutoForAdds = this.signAutoPositionADOs.OrderBy(o => o.Text).ToList();
                            bool isMultiSignForAuto = false;
                            if (!isMultiSign && SignPositionAutoForAdds != null && SignPositionAutoForAdds.Count >= 2)
                                isMultiSignForAuto = true;

                            int demKey = 1;
                            foreach (var nSp in SignPositionAutoForAdds)
                            {
                                bool isMultiSignForProcess = isMultiSign;
                                if (isMultiSignForAuto && demKey == SignPositionAutoForAdds.Count)
                                {
                                    isMultiSignForProcess = false;
                                }
                                else if (isMultiSignForAuto)
                                {
                                    isMultiSignForProcess = isMultiSignForAuto;
                                }
                                else
                                {
                                    isMultiSignForProcess = isMultiSign;
                                }

                                Inventec.Common.Logging.LogSystem.Debug("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp), nSp)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForProcess), isMultiSignForProcess)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => SignPositionAutoForAdds.Count), SignPositionAutoForAdds.Count)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => demKey), demKey)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForAuto), isMultiSignForAuto));

                                float xs = (nSp.Reactanle.Left);
                                xs = xs < 0 ? 0 : xs;
                                float ys = (nSp.Reactanle.Bottom);
                                ys = ys < 0 ? 0 : ys;

                                DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault(nSp);
                                success = SignDigital(xs, ys, nSp.PageNUm, this.totalPageNumber, displayConfigParam, isMultiSignForProcess);

                                demKey++;
                            }
                        }
                        else
                        {
                            Inventec.Common.Logging.LogSystem.Debug("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nextSignPosition), nextSignPosition));
                            float xs = (this.nextSignPosition.Reactanle.Left);//FIX// - 150 / 2
                            xs = xs < 0 ? 0 : xs;
                            float ys = (this.nextSignPosition.Reactanle.Bottom);//FIX// - 60 / 2
                            ys = ys < 0 ? 0 : ys;

                            DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault(this.nextSignPosition);
                            success = SignDigital(xs, ys, this.nextSignPosition.PageNUm, this.totalPageNumber, displayConfigParam);
                        }
                    }
                    else
                    {
                        if (this.fileType == FileType.Json || this.fileType == FileType.Xml)
                        {
                            DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault();

                            success = SignDigital(1, 1, 1, 1, displayConfigParam);
                            if (success)
                            {
                                this.signReason = "";
                                this.txtSignDescription.Text = "";
                            }
                        }
                        else
                        {
                            this.isSigning = true;
                            this.bbtnRelativeHomeSign.Caption = "Hủy";
                            pdfViewer1.MouseDown += pdfViewer1_MouseDown;
                            pdfViewer1.MouseMove += pdfViewer1_MouseMove;
                            pdfViewer1.MouseUp += pdfViewer1_MouseUp;
                            //pdfViewer1.Paint += pdfViewer1_Paint;
                            pdfViewer1.Cursor = Cursors.Cross;
                        }
                    }
                }

                if (success)
                {
                    this.signReason = "";
                    this.txtSignDescription.Text = "";
                    if (this.reload != null) this.reload();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
            }
        }
        bool IsAddPatientSign = false;
        private void bbtnConfigSign_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.currentDocument != null && !String.IsNullOrEmpty(this.currentDocument.DocumentCode))
                {
                    if (this.dlgOpenModuleConfig != null)
                        this.dlgOpenModuleConfig(this.currentDocument);
                    UpdateAfterAddSignThread(this.listSign);
                }
                else
                {
                    frmSignerAdd frmAddSigner = new frmSignerAdd(listSign, UpdateAfterAddSignThread, SignParanelEditValueChanged, (bool)bbtnchkSignParanel.Checked, this.bbtnchkSignParanel.Enabled, "", Treatment, Signer, IsAddPatientSign);
                    frmAddSigner.ShowDialog(this.ParentForm);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnRejectSign_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.currentDocument == null || String.IsNullOrEmpty(this.currentDocument.DocumentCode))
                {
                    MessageManager.Show(MessageUitl.GetMessage(MessageConstan.KhongTonTaiVanBanDeHuyKy));
                    return;
                }

                var sign = new EmrSign().GetSignDocumentFirst(this.currentDocument.DocumentCode, Signer, Treatment, this.isMultiSign, true);
                if (sign == null)
                {
                    MessageManager.Show(MessageUitl.GetMessage(MessageConstan.KhongXacDinhDuocDuLieuDeHuyKy));
                    return;
                }

                frmRejectInfo frmRejectInfo = new SignLibrary.frmRejectInfo(UpdateRejectInfo);
                frmRejectInfo.ShowDialog();

                if (!String.IsNullOrEmpty(this.rejectReason))
                {
                    CommonParam param = new CommonParam();
                    EmrSign emrSign = new EmrSign(param);
                    EmrSignRejectSDO signRejectSDO = new EMR.SDO.EmrSignRejectSDO();

                    signRejectSDO.EmrSignId = (sign != null ? sign.ID : 0);
                    signRejectSDO.RejectReason = this.rejectReason;
                    signRejectSDO.RejectTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    signRejectSDO.RoomCode = this.roomCode;
                    signRejectSDO.RoomTypeCode = this.roomTypeCode;
                    var successRS = emrSign.Reject(TokenCode, signRejectSDO);
                    MessageManager.Show(param, successRS);
                    if (successRS)
                    {
                        this.EnableSignButton(false);
                        this.UpdateStateIGSys(SignStateCode.REJECT);

                        this.inputStream = null;
                        this.ProcessStoreCurrentFileToPrint(this.inputFileWork);
                        this.readerWorking = new PdfReader(this.inputFileWork);
                        this.ProcessSignPdf();

                        this.currentPageSettings = PdfDocumentProcess.GetPaperSize(this.inputFileWork);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnSignEnd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CommonParam param = new CommonParam();
                var rs = new EmrSign(param).SignEnd(TokenCode, this.signSelected.ID);
                bool success = (rs != null);
                if (success)
                {
                    EnableSignButton(false);
                    bbtnSignEnd.Enabled = false;
                    bbtnPatientSign.Enabled = inputADOWorking.IsShowPatientSign ? true : false;
                    this.UpdateStateIGSys(SignStateCode.SUCCESS);
                }
                MessageManager.Show(param, success);
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Error(ex);
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
            }
        }

        private void bbtnListSign_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.currentDocument != null && !String.IsNullOrEmpty(this.currentDocument.DocumentCode))
                {
                    FormEmrSign formEmrSign = new FormEmrSign(this.currentDocument.DocumentCode);
                    formEmrSign.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnResetChkState_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CacheClientWorker.ChangeValue("");
                MessageBox.Show(LibraryMessage.MessageUitl.GetMessage(LibraryMessage.MessageConstan.ResetTrangThaiNguoiDungDaLuuTaiMayTram));

                this.typeDisplayOption = -1;
                GlobalStore.EmrConfigs = null;
                GlobalStore.EmrBusiness = null;
                var rsCfg = GlobalStore.EmrConfigs;
                var rsBus = GlobalStore.EmrBusiness;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void btnViewPACSImage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.vlViewPACSUrlFormat))
                {
                    //a. Bổ sung các key sau (khi nhấn nút, thì thực hiện replace các giá trị tương ứng của văn bản vào chuỗi url cấu hình (theo key cấu hình trên):
                    //- <#SERE_SERV_ID;>
                    //- <#SERVICE_REQ_CODE;>
                    //Giá trị của 2 key trên được lấy bằng cách gọi xử lý giá trị của HIS_CODE. Cụ thể HIS_CODE của các văn bản pacs sẽ có dạng:
                    //"SERVICE_REQ_CODE:XXXXX SER_SERV_ID:YYYYY", khi đó:
                    //<#SERE_SERV_ID;> có giá trị là YYYYY
                    //<#SERVICE_REQ_CODE;> có giá trị là XXXXX
                    string strSERE_SERV_ID = "";
                    string strSERVICE_REQ_CODE = "";
                    if (!String.IsNullOrEmpty(currentDocument.HisCode))
                    {
                        var arr = currentDocument.HisCode.Split(new string[] { " " }, StringSplitOptions.None);
                        if (arr != null && arr.Length > 0)
                        {
                            foreach (var item in arr)
                            {
                                var arrRow = item.Split(new string[] { ":" }, StringSplitOptions.None);
                                if (item.Contains("SERVICE_REQ_CODE:"))
                                {
                                    if (arrRow != null && arrRow.Length > 1)
                                    {
                                        strSERVICE_REQ_CODE = arrRow[1];
                                    }
                                }
                                else if (item.Contains("SER_SERV_ID:"))
                                {
                                    if (arrRow != null && arrRow.Length > 1)
                                    {
                                        strSERE_SERV_ID = arrRow[1];
                                    }
                                }
                            }
                        }
                    }

                    string vlViewPACSUrlFormatWithPatietnInfo = vlViewPACSUrlFormat
                        .Replace("<#TREATMENT_CODE;>", Treatment.TREATMENT_CODE)
                        .Replace("<#DOCUMENT_CODE;>", currentDocument.DocumentCode)
                        .Replace("<#PATIENT_CODE;>", Treatment.PATIENT_CODE)
                        .Replace("<#HIS_CODE;>", currentDocument.HisCode)
                        .Replace("<#SERE_SERV_ID;>", strSERE_SERV_ID)
                        .Replace("<#SERVICE_REQ_CODE;>", strSERVICE_REQ_CODE)
                        .Replace("<#DOCUMENT_TIME;>", currentDocument.DocumentTime.HasValue ? currentDocument.DocumentTime.ToString() : "")
                        .Replace("<#DOCUMENT_DATE;>", currentDocument.DocumentTime.HasValue ? currentDocument.DocumentTime.ToString().Substring(0, 8) : "");
                    new LaunchBrowse().Launch(Uri.EscapeUriString(vlViewPACSUrlFormatWithPatietnInfo));
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnCtrlShiftU_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                MessageBox.Show("LOGINNAME:" + Signer.LOGINNAME + ", TREATMENT_CODE:" + Treatment.TREATMENT_CODE + ", TokenCode:" + TokenCode);
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnChkCloseAfterSign_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (isInitForm)
                {
                    return;
                }

                if (dlgCloseAfterSignCheckedChanged != null)
                    dlgCloseAfterSignCheckedChanged(bbtnChkCloseAfterSign.Checked);
                isCloseAfterSign = bbtnChkCloseAfterSign.Checked;
                this.inputADOWorking.IsCloseAfterSign = bbtnChkCloseAfterSign.Checked;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnConfigBussinessMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                List<EMR_BUSINESS> businesss = null;
                if (GlobalStore.EmrBusiness != null && GlobalStore.EmrBusiness.Count > 0)
                {
                    if (this.printTypeBusinessCodes != null && this.printTypeBusinessCodes.Count > 0)
                    {
                        businesss = GlobalStore.EmrBusiness.Where(o => this.printTypeBusinessCodes.Contains(o.BUSINESS_CODE) || (o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.CREATOR == Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetLoginName())).ToList();
                    }
                    else
                    {
                        businesss = GlobalStore.EmrBusiness.Where(o => o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.CREATOR == Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetLoginName()).ToList();
                    }
                }

                //if (businesss == null || businesss.Count == 0
                //    )
                //{
                //    MessageBox.Show(LibraryMessage.MessageUitl.GetMessage(LibraryMessage.MessageConstan.VanBanChuaDuocThietLapNghiepVuKy));
                //    Inventec.Common.Logging.LogSystem.Warn(LibraryMessage.MessageUitl.GetMessage(LibraryMessage.MessageConstan.VanBanChuaDuocThietLapNghiepVuKy) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => businesss), businesss) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentName), documentName));
                //    return;
                //}
                if (businesss != null && businesss.Count > 0)
                    businesss = businesss.OrderByDescending(o => o.CREATE_TIME).ToList();
                frmChooseBusiness frmChooseBusiness = new frmChooseBusiness(ChooseBusinessClick, businesss, this.businessCode, o =>
                {
                    if (o)
                        GlobalStore.EmrBusiness = null;
                });
                frmChooseBusiness.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnAttackMentsMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.currentDocument != null && !String.IsNullOrEmpty(this.currentDocument.DocumentCode))
                {
                    frmAttachMents frmAttachMents = new frmAttachMents(this.currentDocument.DocumentCode);
                    frmAttachMents.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnSignType_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (isInitForm || bbtnSignType.Visibility == DevExpress.XtraBars.BarItemVisibility.Never)
                {
                    return;
                }

                //if (dlgOptionSignTypeCheckedChanged != null)
                //    dlgOptionSignTypeCheckedChanged(bbtnSignType.Checked);
                //else
                //{
                CacheClientWorker.ChangeValue(RegistryConstant.SIGN_TYPE_OPTION_KEY, bbtnSignType.Checked ? "1" : "0");
                //}
                isOptionSignType = bbtnSignType.Checked;
                this.inputADOWorking.IsOptionSignType = bbtnSignType.Checked;
                this.signType = bbtnSignType.Checked ? SignType.USB : SignType.HSM;
                this.inputADOWorking.SignType = this.signType;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void barCheckUsingSignPad_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.isUsingSignPad = barCheckUsingSignPad.Checked;
                if (this.actChangeUsingSignPad != null)
                {
                    this.actChangeUsingSignPad(barCheckUsingSignPad.Checked);
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void UCViewer_Resize(object sender, EventArgs e)
        {
            try
            {
                if (IsLoadFirst)
                    CaculateSizeItemBarmanager();
                IsLoadFirst = false;
                if (this.Size.Width - 50 <= sizeToSignAndDeskcription)
                {
                    bbtnSignAndDeskcription.Visibility = BarItemVisibility.Never;
                    bbtnSignAndDeskcriptionOther.Visibility = BarItemVisibility.Always;
                    bbtnOther.Visibility = BarItemVisibility.Always;
                }
                else
                {
                    bbtnSignAndDeskcription.Visibility = BarItemVisibility.Always;
                    bbtnSignAndDeskcriptionOther.Visibility = BarItemVisibility.Never;
                    bbtnOther.Visibility = BarItemVisibility.Never;
                }
                if (this.Size.Width - 50 <= sizeToRelativeHomeSign)
                {
                    bbtnRelativeHomeSign.Visibility = BarItemVisibility.Never;
                    bbtnRelativeHomeSignOther.Visibility = BarItemVisibility.Always;
                    bbtnOther.Visibility = BarItemVisibility.Always;
                }
                else
                {
                    bbtnRelativeHomeSign.Visibility = BarItemVisibility.Always;
                    bbtnRelativeHomeSignOther.Visibility = BarItemVisibility.Never;
                    bbtnOther.Visibility = BarItemVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void CaculateSizeItemBarmanager()
        {
            try
            {
                foreach (var item in pdfCommandBar1.ItemLinks)
                {
                    if (item.GetType().Equals(typeof(BarButtonItemLink)))
                    {
                        var BarItem = (item as BarButtonItemLink);
                        total += BarItem.Bounds.Width;
                        if (!IsNotCaculateSignAndDeskcription)
                            sizeToSignAndDeskcription += BarItem.Bounds.Width;
                        if (!IsNotCaculateRelativeHomeSign)
                            sizeToRelativeHomeSign += BarItem.Bounds.Width;
                        if (BarItem.ItemId == bbtnSignAndDeskcription.Id)
                            IsNotCaculateSignAndDeskcription = true;
                        if (BarItem.ItemId == bbtnRelativeHomeSign.Id)
                            IsNotCaculateRelativeHomeSign = true;
                    }
                    else if (item.GetType().Equals(typeof(BarSubItemLink)))
                    {
                        var BarItem = (item as BarSubItemLink);
                        total += BarItem.Bounds.Width;
                        if (!IsNotCaculateSignAndDeskcription)
                            sizeToSignAndDeskcription += BarItem.Bounds.Width;
                        if (!IsNotCaculateRelativeHomeSign)
                            sizeToRelativeHomeSign += BarItem.Bounds.Width;
                        if (BarItem.ItemId == bbtnSignAndDeskcription.Id)
                            IsNotCaculateSignAndDeskcription = true;
                        if (BarItem.ItemId == bbtnRelativeHomeSign.Id)
                            IsNotCaculateRelativeHomeSign = true;
                    }
                    else if (item.GetType().Equals(typeof(BarCheckItemLink)))
                    {
                        var BarItem = (item as BarCheckItemLink);
                        total += BarItem.Bounds.Width;
                        if (!IsNotCaculateSignAndDeskcription)
                            sizeToSignAndDeskcription += BarItem.Bounds.Width;
                        if (!IsNotCaculateRelativeHomeSign)
                            sizeToRelativeHomeSign += BarItem.Bounds.Width;
                        if (BarItem.ItemId == bbtnSignAndDeskcription.Id)
                            IsNotCaculateSignAndDeskcription = true;
                        if (BarItem.ItemId == bbtnRelativeHomeSign.Id)
                            IsNotCaculateRelativeHomeSign = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timerLoadSinglePage.Stop();
                pdfViewer1.ZoomMode = DevExpress.XtraPdfViewer.PdfZoomMode.ActualSize;
                if (currentEmrDocument != null && currentEmrDocument.VIEW_ZOOM_PERCENT.HasValue)
                    pdfViewer1.ZoomFactor = (float)currentEmrDocument.VIEW_ZOOM_PERCENT;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnSignAndDeskcription_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                bool success = false;
                this.isPatientSign = false;
                this.isHomeRelativeSign = false;
                this.typeDisplayOption = -1;

                if (this.isSigning)
                {
                    CancelSign();
                    return;
                }

                this.signReason = "";
                frmAddNote frmAddNote = new frmAddNote(UpdateNote);
                frmAddNote.ShowDialog();

                if (String.IsNullOrWhiteSpace(this.signReason))
                {
                    MessageManager.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    CancelSign();
                    return;
                }

                if (!VerifyWithExistsDocument(this.currentDocument))
                {
                    CancelSign();
                    return;
                }
                bool? vOptionSign = VerifySign.VerifySignImageWithOption(this.inputADOWorking, Signer, this.hasNextSignPosition, this.nextSignPosition, this.isMultiSign);
                if (vOptionSign.HasValue)
                {
                    if (vOptionSign.Value)
                    {
                        this.typeDisplayOption = Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT;
                    }
                    else
                    {
                        CancelSign();
                        return;
                    }
                }

                if (this.hasNextSignPosition && this.nextSignPosition != null && (this.signSelected == null || (this.signSelected != null && (this.signSelected.SIGN_TIME ?? 0) <= 0)))
                {
                    if (this.signAutoPositionADOs != null && this.signAutoPositionADOs.Count > 0)//this.isMultiSign && 
                    {
                        var SignPositionAutoForAdds = this.signAutoPositionADOs.OrderBy(o => o.Text).ToList();
                        bool isMultiSignForAuto = false;
                        if (!isMultiSign && SignPositionAutoForAdds != null && SignPositionAutoForAdds.Count >= 2)
                            isMultiSignForAuto = true;

                        int demKey = 1;
                        foreach (var nSp in SignPositionAutoForAdds)
                        {
                            bool isMultiSignForProcess = isMultiSign;
                            if (isMultiSignForAuto && demKey == SignPositionAutoForAdds.Count)
                            {
                                isMultiSignForProcess = false;
                            }
                            else if (isMultiSignForAuto)
                            {
                                isMultiSignForProcess = true;
                            }
                            else
                            {
                                isMultiSignForProcess = isMultiSign;
                            }

                            Inventec.Common.Logging.LogSystem.Debug("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp), nSp)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForProcess), isMultiSignForProcess)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => SignPositionAutoForAdds.Count), SignPositionAutoForAdds.Count)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => demKey), demKey)
      + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isMultiSignForAuto), isMultiSignForAuto));

                            float xs = (nSp.Reactanle.Left);
                            xs = xs < 0 ? 0 : xs;
                            float ys = (nSp.Reactanle.Bottom);
                            ys = ys < 0 ? 0 : ys;

                            DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault(nSp);

                            success = SignDigital(xs, ys, nSp.PageNUm, this.totalPageNumber, displayConfigParam, isMultiSignForProcess);
                            demKey++;
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Debug("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nextSignPosition), nextSignPosition));
                        float xs = (this.nextSignPosition.Reactanle.Left);//FIX// - 150 / 2
                        xs = xs < 0 ? 0 : xs;
                        float ys = (this.nextSignPosition.Reactanle.Bottom);//FIX// - 60 / 2
                        ys = ys < 0 ? 0 : ys;
                        DisplayConfigDTO displayConfigParam = GetDisplayConfigByCommentOrDefault(this.nextSignPosition);
                        success = SignDigital(xs, ys, this.nextSignPosition.PageNUm, this.totalPageNumber, displayConfigParam);
                    }

                    if (success)
                    {
                        this.signReason = "";
                        this.txtSignDescription.Text = "";
                    }
                }
                else
                {
                    this.isSigning = true;
                    this.bbtnSign.Caption = "Hủy";
                    pdfViewer1.MouseDown += pdfViewer1_MouseDown;
                    pdfViewer1.MouseMove += pdfViewer1_MouseMove;
                    pdfViewer1.MouseUp += pdfViewer1_MouseUp;
                    //pdfViewer1.Paint += pdfViewer1_Paint;
                    pdfViewer1.Cursor = Cursors.Cross;
                }
                Inventec.Common.Logging.LogSystem.Debug("bbtnSign_ItemClick____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success));

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

    }
}
