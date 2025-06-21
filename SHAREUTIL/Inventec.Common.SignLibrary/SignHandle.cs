using EMR.EFMODEL.DataModels;
using Inventec.Common.SignLibrary.Api;
using Inventec.Common.Integrate;
using Inventec.Common.SignFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using EMR.TDO;
using System.Linq;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.SignHandler;
using Inventec.Common.SignLibrary.LibraryMessage;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.DTO;

namespace Inventec.Common.SignLibrary
{
    internal class SignHandle : BussinessBase
    {
        #region Variable
        float x, y;
        int pageNumberCurrent;
        int totalPageNumber;
        Stream stream;
        Action dlgCancel;
        Action<DocumentTDO> dlgOpenModuleConfig;
        string signName;
        string signDescription;
        string documentName;
        string treatmentCode;
        List<VerifierADO> verifiers;
        SignType signType;
        List<SignTDO> tempSigns;
        EMR_SIGN signSelected;
        bool isPatientSign;
        bool isHomeRelativeSign;
        long? documentTypeId;
        bool isMultiSign;
        string hisCode;
        Form parentForm;
        DisplayConfigDTO displayConfigParam;

        EMR_RELATION relation;
        string relationShipName = "";

        string SplitFileHeader = "";
        string SplitFileContent = "";
        double oginalHeight = 0;
        FileType fileType;

        #endregion

        #region Construct
        internal SignHandle(string _src, float _x, float _y, int _pageNumberCurrent, int _totalPageNumber, Action _dlgCancel, Action<DocumentTDO> _dlgOpenModuleConfig, string _documentName, string _treatmentCode, string _signName, string _signReason, List<VerifierADO> _verifiers, SignType _signType, List<SignTDO> _signTDOs, EMR_SIGN _signSelected, bool _isPatientSign, bool _isHomeRelativeSign, long? _documentTypeId, bool _isMultiSign, string hisCode, InputADO inputADOWorking, DisplayConfigDTO displayConfigDTO, CommonParam param, Form parentForm, bool isSignParanel, EMR_TREATMENT treatment, EMR.EFMODEL.DataModels.EMR_SIGNER singer, string tokenCode)
            : base(param)
        {
            this.x = _x;
            this.y = _y;
            this.pageNumberCurrent = _pageNumberCurrent;
            this.totalPageNumber = _totalPageNumber;
            this.dlgCancel = _dlgCancel;
            this.dlgOpenModuleConfig = _dlgOpenModuleConfig;
            this.Src = _src;
            this.signName = _signName;
            this.signDescription = _signReason;
            this.verifiers = _verifiers;
            this.signType = _signType;
            this.tempSigns = _signTDOs;
            this.documentName = _documentName;
            this.treatmentCode = _treatmentCode;
            this.signSelected = _signSelected;
            this.isPatientSign = _isPatientSign;
            this.isHomeRelativeSign = _isHomeRelativeSign;
            this.documentTypeId = _documentTypeId;
            this.isMultiSign = _isMultiSign;
            this.hisCode = hisCode;
            this.inputADOWorking = inputADOWorking;
            this.relationShipName = inputADOWorking.RelationPeopleName;
            this.relation = inputADOWorking.Relation;
            this.displayConfigParam = (displayConfigDTO != null ? displayConfigDTO : inputADOWorking.DisplayConfigDTO);
            this.parentForm = parentForm;
            this.IsSignParanel = isSignParanel;
            this.Treatment = treatment;
            this.Signer = singer;
            this.TokenCode = tokenCode;
        }

        internal SignHandle(Stream _stream, float _x, float _y, int _pageNumberCurrent, int _totalPageNumber, Action _dlgCancel, Action<DocumentTDO> _dlgOpenModuleConfig, string _documentName, string _treatmentCode, string _signName, string _signReason, List<VerifierADO> _verifiers, SignType _signType, List<SignTDO> _signTDOs, EMR_SIGN _signSelected, bool _isPatientSign, bool _isHomeRelativeSign, long? _documentTypeId, bool _isMultiSign, string hisCode, InputADO inputADOWorking, DisplayConfigDTO displayConfigDTO, CommonParam param, Form parentForm, bool isSignParanel, EMR_TREATMENT treatment, EMR.EFMODEL.DataModels.EMR_SIGNER singer, string tokenCode)
            : base(param)
        {
            this.x = _x;
            this.y = _y;
            this.pageNumberCurrent = _pageNumberCurrent;
            this.totalPageNumber = _totalPageNumber;
            this.dlgCancel = _dlgCancel;
            this.dlgOpenModuleConfig = _dlgOpenModuleConfig;
            this.stream = _stream;
            this.signName = _signName;
            this.signDescription = _signReason;
            this.verifiers = _verifiers;
            this.signType = _signType;
            this.tempSigns = _signTDOs;
            this.documentName = _documentName;
            this.treatmentCode = _treatmentCode;
            this.signSelected = _signSelected;
            this.isPatientSign = _isPatientSign;
            this.isHomeRelativeSign = _isHomeRelativeSign;
            this.documentTypeId = _documentTypeId;
            this.isMultiSign = _isMultiSign;
            this.hisCode = hisCode;
            this.inputADOWorking = inputADOWorking;
            this.relationShipName = inputADOWorking.RelationPeopleName;
            this.relation = inputADOWorking.Relation;
            this.displayConfigParam = (displayConfigDTO != null ? displayConfigDTO : inputADOWorking.DisplayConfigDTO);
            this.parentForm = parentForm;
            this.IsSignParanel = isSignParanel;
            this.Treatment = treatment;
            this.Signer = singer;
            this.TokenCode = tokenCode;
        }
        #endregion

        #region Method

        internal void SetUsingSignPad(bool isUsingSignPad)
        {
            this.IsUsingSignPad = isUsingSignPad;
        }
        internal void SetSignPadBefore(byte[] SignPadImageData)
        {
            this.IsUsingSignPadBefore = SignPadImageData != null && SignPadImageData.Length > 0;
            this.SignPadImageData = SignPadImageData;
        }
        internal void SetFileType(FileType _FileType)
        {
            this.fileType = _FileType;
        }

        internal bool SignFile(DocumentTDO document, ref string outputFile)
        {
            bool result = false;
            bool successRS = false;
            string cmnd = "", cardCode = "", serviceCode = "", linkCode = "", relationName = "", relationPeopleName = "";
            byte[] signedImageData = SignPadImageData;
            long? relationId = null;

            this.SplitFileProcess();
            if (this.isPatientSign || this.isHomeRelativeSign || this.signType == SignType.HMS)
            {
                List<SignTDO> signs = null;
                bool isCardAnonymous = false;
                SignHSMHandler signHSMHandler = new SignHSMHandler(param, inputADOWorking, this.Src, this.IsSignParanel, Treatment, Signer, TokenCode);

                //a. Nghiệp vụ nút "Bệnh nhân ký":
                //- Nghiệp vụ check đúng sai thứ tự ký hay chọn điểm ký xử lý như hiện tại.
                //- Nếu bệnh nhân không có số thẻ KCB (CARD_CODE trong EMR_TREATMENT): Chặn và thông báo "Bệnh nhân không có số thẻ KCB.".
                //- Nếu bệnh nhân có số thẻ KCB thì thực hiện gọi WCF qua phần mềm thẻ truyền vào số thẻ của bệnh nhân.
                //- Nếu pm thẻ trả kết quả xác thực thất bại -> Thông báo
                //- Nếu kết quả pm thẻ trả về là xác thực thành công thì sẽ có 2 trường hợp:
                //+ Nếu pm thẻ trả về báo thẻ bệnh nhân ký là thẻ định danh -> thực hiện gọi api Emr ký HSM như hiện tại.
                //+ Nếu pm thẻ trả về báo thẻ bệnh nhân ký là thẻ không định danh:
                //-> gọi api emr thực hiện đánh dấu bệnh nhân đã ký (lưu điểm ký và đánh dấu bệnh nhân ký điện tử (IS_SIGN_ELECTRONIC = 1) vào EMR_SIGN của bệnh nhân).
                //-> Khi hiển thị văn bản mà có bệnh nhân ký điện tử -> hiển thị vào điểm bệnh nhân ký 2 thông tin "Tên bệnh nhân - Đã ký".
                if (this.isPatientSign || this.isHomeRelativeSign)
                {
                    string err = "";

                    if ((this.tempSigns == null || this.tempSigns.Count == 0))
                    {
                        tempSigns = (this.signSelected == null || (this.inputADOWorking != null && String.IsNullOrEmpty(this.inputADOWorking.BusinessCode))) ? SignStrategy(SignType.HMS, document) : null;
                    }

                    if (!Verify.VerifySigner(this.tempSigns, document, Treatment, ref err, true))
                    {
                        if (!String.IsNullOrEmpty(err))
                            MessageBox.Show(err);
                        Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => err), err));
                        return false;
                    }

                    if (String.IsNullOrEmpty(Treatment.CARD_CODE))
                    {
                        if (inputADOWorking.DlgGetTreatment != null)
                        {
                            var treatmentDTOHis = inputADOWorking.DlgGetTreatment(Treatment.TREATMENT_CODE);
                            if (treatmentDTOHis != null && !String.IsNullOrEmpty(treatmentDTOHis.CARD_CODE))
                            {
                                Inventec.Common.Logging.LogSystem.Info("Truong hop khong co thong tin CardCode trong EMR_TREATMENT & trong input HIS gui sang ==> goi delegate lay cardcode theo treatmentCode:" + Treatment.TREATMENT_CODE + ", " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatmentDTOHis), treatmentDTOHis));
                                Treatment.CARD_CODE = treatmentDTOHis.CARD_CODE;
                            }
                        }
                    }

                    if (GlobalStore.EMR_SIGN_BOARD__OPTION == "2" && this.IsUsingSignPad)
                    {
                        WaitingManager.Hide();

                        isCardAnonymous = true;
                        //if (String.IsNullOrEmpty(Treatment.CARD_CODE))
                        //{
                        if (this.isHomeRelativeSign)
                        {
                            if (String.IsNullOrWhiteSpace(this.relationShipName) || this.relation == null || (this.relation != null && string.IsNullOrEmpty(relation.RELATION_NAME)))
                            {
                                frmChoicePatientRelation frm = new frmChoicePatientRelation(ActSelectRelationShip);
                                //frm.TopMost = true;
                                frm.ShowDialog();
                            }

                            if (String.IsNullOrWhiteSpace(this.relationShipName) || this.relation == null)
                            {
                                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.KhongNhapTTDinhDanhHoacChonTheKy));
                                return false;
                            }
                            else
                            {
                                relationName = (this.relation != null) ? (string)this.relation.RELATION_NAME : "";
                                relationId = (this.relation != null) ? (long?)this.relation.ID : null;

                                relationPeopleName = this.relationShipName;

                                if (tempSigns != null && tempSigns.Count > 0)
                                {
                                    foreach (var tsn in tempSigns)
                                    {
                                        if (!String.IsNullOrEmpty(tsn.PatientCode))
                                        {
                                            tsn.CmndNumber = cmnd;
                                            tsn.CardCode = cardCode;
                                            tsn.ServiceCode = serviceCode;
                                            tsn.LinkCode = linkCode;
                                            tsn.RelationId = (this.relation != null) ? (long?)this.relation.ID : null;
                                            tsn.RelationName = (this.relation != null) ? (string)this.relation.RELATION_NAME : "";
                                            tsn.RelationPeopleName = this.relationShipName;
                                        }
                                    }
                                }
                                else if (signs != null && signs.Count > 0)
                                {
                                    foreach (var tsn in signs)
                                    {
                                        if (!String.IsNullOrEmpty(tsn.PatientCode))
                                        {
                                            tsn.CmndNumber = cmnd;
                                            tsn.CardCode = cardCode;
                                            tsn.ServiceCode = serviceCode;
                                            tsn.LinkCode = linkCode;
                                            tsn.RelationId = (this.relation != null) ? (long?)this.relation.ID : null;
                                            tsn.RelationName = (this.relation != null) ? (string)this.relation.RELATION_NAME : "";
                                            tsn.RelationPeopleName = this.relationShipName;
                                        }
                                    }
                                }
                            }
                        }
                        //}

                        Inventec.Common.Logging.LogSystem.Info("Vào TH BN hoặc người nhà ký & BN không có thẻ kcb => không kiểm tra xác thực vân tay:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => Treatment.CARD_CODE), Treatment.CARD_CODE) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => relationName), relationName) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => relationPeopleName), relationPeopleName));

                        signHSMHandler.SetUsingSignPadData(this.IsUsingSignPad);
                        signHSMHandler.SetSignPadBefore(this.IsUsingSignPadBefore);
                        WaitingManager.Show();
                    }
                    else
                    {
                        if (!FingerPrintClientService.Valid(this.isHomeRelativeSign, ref cmnd, ref cardCode, ref serviceCode, ref linkCode, ref relationName, ref relationPeopleName, ref signedImageData, ref isCardAnonymous, ref this.tempSigns, Treatment))
                            return false;
                    }
                }
                else
                {
                    signs = (this.signSelected == null || (this.inputADOWorking != null && String.IsNullOrEmpty(this.inputADOWorking.BusinessCode))) ? SignStrategy(SignType.HMS, document) : null;
                }
                Stream outputStream = null;

                signHSMHandler.SetFileType(this.fileType);

                if (document != null && !String.IsNullOrEmpty(document.DocumentCode))
                {
                    if (GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "3" && (this.isPatientSign || this.isHomeRelativeSign) && (signSelected == null || string.IsNullOrEmpty(this.signSelected.PATIENT_CODE)))
                    {
                        //goi api emr chi thuc hien ky
                        successRS = signHSMHandler.PatientOrHomeRelativeSignOnly(ref document, signs, this.tempSigns, GetPointSign(), this.signDescription, this.isPatientSign, isHomeRelativeSign, this.isMultiSign, cmnd, cardCode, serviceCode, isCardAnonymous, relationId, relationName, relationPeopleName, ref outputStream, this.signSelected, this.inputADOWorking.MergeCode, linkCode, signedImageData, !string.IsNullOrEmpty(this.inputADOWorking.BusinessCode));
                    }
                    else
                    {
                        //goi api emr chi thuc hien ky
                        successRS = signHSMHandler.SignOnly(ref document, signs, this.tempSigns, GetPointSign(), this.signDescription, (this.isPatientSign || isHomeRelativeSign), this.isMultiSign, cmnd, cardCode, serviceCode, isCardAnonymous, relationId, relationName, relationPeopleName, ref outputStream, this.signSelected, this.inputADOWorking.MergeCode, linkCode, signedImageData);
                    }
                }
                else
                {
                    //goi api emr vua thuc hien tao van ban va ky tren van ban vua tao
                    successRS = signHSMHandler.SignWithCreateDoc(outputFile, ref document, this.documentName, treatmentCode, signs, this.tempSigns, GetBase64FileData(), GetBase64HeaderFileData(), GetPointSign(), this.signDescription, (this.isPatientSign || isHomeRelativeSign), this.documentTypeId, this.isMultiSign, this.hisCode, isCardAnonymous, ref outputStream, this.signSelected, this.inputADOWorking.MergeCode, this.oginalHeight, signedImageData);
                }
                if (!(this.isMultiSign && successRS))
                {
                    WaitingManager.Hide();
                    MessageManager.Show(this.parentForm, param, successRS);
                }
                if (successRS && outputStream != null && outputStream.Length > 0)
                {
                    outputStream.Position = 0;
                    outputFile = Utils.GenerateTempFileWithin();
                    using (var fileStream = File.OpenWrite(outputFile))
                    {
                        outputStream.Seek(0, SeekOrigin.Begin);
                        outputStream.CopyTo(fileStream);
                    }
                    try
                    {
                        outputStream.Close();
                        outputStream.Dispose();
                        outputStream = null;
                    }
                    catch (Exception exx)
                    {
                        Inventec.Common.Logging.LogSystem.Warn(exx);
                    }

                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else if (this.signType == SignType.USB)
            {
                bool success = false;
                SignUSBHandler signUSBHandler = new SignUSBHandler(param, inputADOWorking, this.IsSignParanel, Treatment, Signer, TokenCode);
                signUSBHandler.SetFileType(this.fileType);
                if (!GlobalStore.IsSignUsingUsbTokenDevice && signUSBHandler.VerifyServiceSignProcessorIsRunning())
                {
                    success = signUSBHandler.SignFileWithUSBTokenTSAWithCallService(ref outputFile, this.Src, stream, x, y, pageNumberCurrent, totalPageNumber, signDescription, displayConfigParam, dlgCancel);
                }
                else
                {
                    success = signUSBHandler.SignFileWithUSBTokenTSAWithUsingUsbTokenDevice(ref outputFile, this.Src, stream, x, y, pageNumberCurrent, totalPageNumber, signDescription, displayConfigParam, dlgCancel);
                }

                if (success && !String.IsNullOrEmpty(outputFile) && File.Exists(outputFile))
                {
                    var signs = this.signSelected == null ? SignStrategy(SignType.USB, document) : null;
                    if (document != null && !String.IsNullOrEmpty(document.DocumentCode))
                    {
                        //goi api emr chi thuc hien ky
                        successRS = signUSBHandler.SignOnly(ref document, this.isMultiSign, signs, this.tempSigns, GetBase64FileData(outputFile), this.signSelected, signDescription, this.inputADOWorking.MergeCode);
                    }
                    else
                    {
                        //goi api emr vua thuc hien tao van ban va ky tren van ban vua tao
                        successRS = signUSBHandler.SignWithCreateDoc(outputFile, ref document, this.documentName, treatmentCode, signs, this.tempSigns, GetBase64FileData(outputFile), GetBase64FileData(), this.documentTypeId, this.isMultiSign, this.hisCode, this.signSelected, signDescription, this.inputADOWorking.MergeCode);
                    }
                    if (!(this.isMultiSign && successRS))
                    {
                        WaitingManager.Hide();
                        MessageManager.Show(this.parentForm, param, successRS);
                    }
                    if (successRS)
                    {
                        try
                        {
                            //DevExpress.XtraBars.MessageFilter.BarManagerHook BarManagerHook = new DevExpress.XtraBars.MessageFilter.BarManagerHook();                        
                            //DevExpress.XtraBars.MessageFilter.BarManagerHook.FilterMouseEvents();
                        }
                        catch (Exception exx)
                        {
                            Inventec.Common.Logging.LogSystem.Warn(exx);
                        }

                        if (this.stream != null) this.stream.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Loại ký này chưa được hỗ trợ (SignType: " + this.signType + ")");
                Inventec.Common.Logging.LogSystem.Warn("Loại ký này chưa được hỗ trợ (SignType: " + this.signType + ")");
            }
            return result;
        }

        private void ActSelectRelationShip(EMR_RELATION _relation, string _relationShipName)
        {
            try
            {
                this.relation = _relation;
                this.relationShipName = _relationShipName;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void ActSelectDevice(string deviceName)
        {
            try
            {
                this.deviceSignPadName = deviceName;
                if (inputADOWorking.ActSelectDevice != null)
                {
                    inputADOWorking.ActSelectDevice(deviceName);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void GetSignImageFIle(System.Drawing.Bitmap bmpSignImage)
        {
            try
            {
                if (bmpSignImage != null)
                {
                    this.SignPadImageData = Utils.ImageToByte(bmpSignImage);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }


        void SplitFileProcess()
        {
            try
            {
                this.SplitFileHeader = "";
                this.SplitFileContent = "";
                this.oginalHeight = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        internal DocumentTDO SendDocument(DocumentTDO document)
        {
            DocumentTDO doc = new DocumentTDO();
            doc.TreatmentCode = treatmentCode;
            doc.DocumentName = (String.IsNullOrEmpty(this.documentName) ? ("Ký điện tử cho hồ sơ có mã " + treatmentCode + " ngày " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")) : this.documentName);

            List<SignTDO> signs = null;// SignStrategy(SignType.USB, document);

            doc.Signs = this.tempSigns;
            if (signs != null && signs.Count > 0)
            {
                if (doc.Signs == null)
                {
                    doc.Signs = new List<SignTDO>();
                }
                doc.Signs.AddRange(signs);
            }

            if (doc.Signs != null && doc.Signs.Count > 0)
                foreach (var sg in doc.Signs)
                {
                    sg.SignTime = null;
                }


            doc.BusinessCode = inputADOWorking.BusinessCode;
            doc.HisCode = this.hisCode;
            doc.DocumentTypeId = this.documentTypeId;
            doc.DependentCode = inputADOWorking.DependentCode;
            doc.ParentDependentCode = inputADOWorking.ParentDependentCode;
            doc.HisOrder = inputADOWorking.HisOrder;
            if (!String.IsNullOrEmpty(this.inputADOWorking.DocumentGroupCode))
            {
                var docGroup = new EmrDocumentGroup().GetByCode(this.inputADOWorking.DocumentGroupCode);
                doc.DocumentGroupId = docGroup != null ? (long?)docGroup.ID : null;
            }
            doc.MergeCode = this.inputADOWorking.MergeCode;
            if (this.inputADOWorking.DocumentTime != null && this.inputADOWorking.DocumentTime.Value != DateTime.MinValue)
            {
                doc.DocumentTime = DateTimeConvert.SystemDateTimeToTimeNumber(this.inputADOWorking.DocumentTime);//TODO
            }

            if (this.inputADOWorking.PaperSizeDefault != null)
            {
                doc.PaperName = inputADOWorking.PaperSizeDefault.PaperName;
                if (String.IsNullOrEmpty(doc.PaperName))
                {
                    doc.PaperName = this.inputADOWorking.PaperSizeDefault.Kind.ToString();
                }
                doc.Width = this.inputADOWorking.PaperSizeDefault.Width;
                doc.Height = this.inputADOWorking.PaperSizeDefault.Height;
                doc.RawKind = this.inputADOWorking.PaperSizeDefault.RawKind;
            }

            doc.IsSignParallel = document != null ? document.IsSignParallel : false;
            this.SplitFileProcess();
            if (!String.IsNullOrEmpty(SplitFileHeader) && File.Exists(SplitFileHeader))
            {
                doc.Base64Header = Utils.FileToBase64String(this.SplitFileHeader);
            }
            doc.OriginalVersion = new VersionTDO();

            if (document.OriginalVersion != null && !String.IsNullOrEmpty(document.OriginalVersion.Base64Data))
            {
                doc.OriginalVersion.Base64Data = document.OriginalVersion.Base64Data;
                doc.OriginalVersion.Base64DataJson = document.OriginalVersion.Base64DataJson;
                doc.OriginalVersion.Base64DataXml = document.OriginalVersion.Base64DataXml;
            }
            else
            {
                doc.OriginalVersion.Base64Data = GetBase64FileData();
            }

            if (this.FileType == SignLibrary.FileType.Xml)
            {
                doc.FileType = EMR.TDO.FileType.XML;
            }
            else if (this.FileType == SignLibrary.FileType.Json)
            {
                doc.FileType = EMR.TDO.FileType.JSON;
            }
            else
            {
                doc.FileType = EMR.TDO.FileType.PDF;
            }

            CommonParam commonParam = new Common.Integrate.CommonParam();
            EmrDocument emrDocument = new EmrDocument(commonParam);
            DocumentTDO documentRS = emrDocument.CreateByTdo(TokenCode, doc);
            param.Messages = commonParam.Messages;
            param.BugCodes = commonParam.BugCodes;
            return documentRS;
        }

        internal bool VerifyFile(DocumentTDO document, int signedCount, ref string message)
        {
            try
            {
                if (signedCount > 0 && pageNumberCurrent < 1)
                {
                    WaitingManager.Hide();
                    message = MessageUitl.GetMessage(MessageConstan.BanKhongTheThucHienKyTaiTrangKyVuiLongChonKyTuTrangThu2TroDi);
                    MessageManager.Show(message);

                    return false;
                }

                //if (document != null && !String.IsNullOrEmpty(document.DocumentCode) && this.signSelected == null)
                //{
                //    this.signSelected = new EmrSign().GetSignDocumentFirst(document.DocumentCode, isPatientSign ? null : GlobalStore.Singer);

                //    if (this.signSelected == null || this.signSelected.ID == 0)
                //    {
                //        MessageManager.Show(MessageUitl.GetMessage(MessageConstan.PhaiTaoLuongKyChoVanBanDaCoTrenHeThong));
                //        if (this.dlgOpenModuleConfig != null)
                //            this.dlgOpenModuleConfig(document);

                //        return false;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
            return true;
        }

        private List<SignTDO> SignStrategy(SignType signType, DocumentTDO document)
        {
            SignTDO sign = null;
            List<SignTDO> signs = new List<SignTDO>();
            try
            {
                //var signerBYLogins = new EmrSigner().GetByLoginName(Singer.LOGINNAME);
                if (Signer == null)
                {
                    MessageBox.Show("Dữ liệu người dùng không hợp lệ, không tìm thấy thông tin người ký trên hệ thống EMR");
                    Inventec.Common.Logging.LogSystem.Warn("Dữ liệu người dùng không hợp lệ, không tìm thấy thông tin người ký trên hệ thống EMR");
                    return null;
                }

                string docuemntCode = "";
                if (document != null)
                {
                    docuemntCode = document.DocumentCode;
                }

                //Nếu là document mới (chưa có trong EMR_DOCUMENT) thì duyệt danh sách luồng ký lưu tạm
                //lấy ra luồng ký đầu tiên
                if (this.tempSigns != null && this.tempSigns.Count > 0)
                {
                    sign = this.tempSigns.Where(o => o.Loginname == Signer.LOGINNAME).OrderBy(o => o.NumOrder).FirstOrDefault();
                    if (sign != null)
                    {
                        sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                        sign.SignerId = Signer.ID;
                        sign.NumOrder = (sign.NumOrder == 0 ? frmSignerAdd.stepNumOrder : sign.NumOrder);
                    }
                }
                else
                {
                    sign = new EmrSign().GetSignDocumentFirstByLoginName(docuemntCode, Signer);
                    if (sign != null)
                    {
                        sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                        sign.SignerId = Signer.ID;
                        sign.NumOrder = (sign.NumOrder == 0 ? frmSignerAdd.stepNumOrder : sign.NumOrder);//TODO
                        signs.Add(sign);
                    }
                }

                //Trường hợp không tìm thấy người ký thì tự động thêm người ký vào & thêm luồng ký với người ký này vào danh sách luồng ký cần tạo
                if (sign == null)
                {
                    sign = new SignTDO();
                    if (this.isPatientSign || this.isHomeRelativeSign)
                    {
                        if (this.tempSigns == null || this.tempSigns.Count == 0)
                        {
                            sign.Username = Treatment.VIR_PATIENT_NAME;
                            sign.NumOrder = frmSignerAdd.stepNumOrder;
                            sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            sign.PatientCode = Treatment.PATIENT_CODE;
                            sign.FirstName = Treatment.FIRST_NAME;
                            sign.LastName = Treatment.LAST_NAME;
                            sign.FullName = Treatment.VIR_PATIENT_NAME;

                            signs.Add(sign);
                        }
                    }
                    else
                    {
                        sign.Loginname = Signer.LOGINNAME;
                        sign.Username = Signer.USERNAME;
                        sign.FirstName = Signer.USERNAME;
                        sign.Title = Signer.TITLE;
                        sign.DepartmentCode = Signer.DEPARTMENT_CODE;
                        sign.DepartmentName = Signer.DEPARTMENT_NAME;
                        sign.NumOrder = frmSignerAdd.stepNumOrder;
                        sign.SignerId = Signer.ID;
                        sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                        signs.Add(sign);
                    }
                }
            }
            catch (Exception ex)
            {
                signs = null;
            }

            return signs;
        }

        private bool VerifyDataPreCallApi(HsmSignCreateTDO doc)
        {
            bool valid = true;

            valid = valid && (doc != null);
            valid = valid && (doc.OriginalVersion != null);
            //valid = valid && (doc.DocumentName != null);
            valid = valid && (doc.TreatmentCode != null);

            return valid;
        }

        private bool VerifyDataPreCallApi(UsbSignCreateTDO doc)
        {
            bool valid = true;

            valid = valid && (doc != null);
            valid = valid && (doc.OriginalVersion != null);
            //valid = valid && (doc.DocumentName != null);
            valid = valid && (doc.TreatmentCode != null);

            return valid;
        }

        private string GetBase64FileData()
        {
            string b64Data = "";
            MemoryStream streamData = new MemoryStream();
            if (!String.IsNullOrEmpty(this.SplitFileContent) && File.Exists(this.SplitFileContent))
            {
                b64Data = GetBase64FileData(this.SplitFileContent);
            }
            else
            {
                if (!String.IsNullOrEmpty(this.Src))
                {
                    b64Data = GetBase64FileData(this.Src);
                }
                else
                {
                    this.stream.CopyTo(streamData);
                    streamData.Position = 0;
                    b64Data = Convert.ToBase64String(streamData.ToArray());
                }
            }

            return b64Data;
        }

        private string GetBase64HeaderFileData()
        {
            string b64Data = "";
            if (!String.IsNullOrEmpty(this.SplitFileHeader) && File.Exists(this.SplitFileHeader))
            {
                b64Data = GetBase64FileData(this.SplitFileHeader);
            }

            return b64Data;
        }

        private string GetBase64FileData(string outFile)
        {
            string b64Data = "";
            MemoryStream streamData = new MemoryStream();
            if (!String.IsNullOrEmpty(outFile))
            {
                using (FileStream file = new FileStream(outFile, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    streamData.Write(bytes, 0, (int)file.Length);
                }
            }

            streamData.Position = 0;
            b64Data = Convert.ToBase64String(streamData.ToArray());
            return b64Data;
        }

        private PointSignTDO GetPointSign()
        {
            PointSignTDO pointSignTDO = new PointSignTDO();
            pointSignTDO.CoorXRectangle = x;
            pointSignTDO.CoorYRectangle = y;
            pointSignTDO.MaxPageNumber = totalPageNumber;
            pointSignTDO.PageNumber = pageNumberCurrent;

            if (this.displayConfigParam != null)
            {
                if (this.displayConfigParam.WidthRectangle.HasValue)
                {
                    pointSignTDO.WidthRectangle = this.displayConfigParam.WidthRectangle.Value;
                }
                if (this.displayConfigParam.HeightRectangle.HasValue)
                {
                    pointSignTDO.HeightRectangle = this.displayConfigParam.HeightRectangle.Value;
                }
                if (this.displayConfigParam.SizeFont.HasValue)
                {
                    pointSignTDO.SizeFont = this.displayConfigParam.SizeFont.Value;
                }
                if (this.displayConfigParam.TextPosition.HasValue)
                {
                    pointSignTDO.TextPosition = this.displayConfigParam.TextPosition.Value;
                }
                if (this.displayConfigParam.TypeDisplay.HasValue)
                {
                    pointSignTDO.TypeDisplay = this.displayConfigParam.TypeDisplay.Value;
                }
                if (this.displayConfigParam.IsDisplaySignature.HasValue)
                {
                    pointSignTDO.IsDisplaySignature = this.displayConfigParam.IsDisplaySignature.Value;
                }
                if (!String.IsNullOrEmpty(this.displayConfigParam.FormatRectangleText))
                {
                    pointSignTDO.FormatRectangleText = this.displayConfigParam.FormatRectangleText;
                }
                if (this.displayConfigParam.Alignment.HasValue)
                    pointSignTDO.Alignment = this.displayConfigParam.Alignment.Value;
                if (this.displayConfigParam.IsBold.HasValue)
                    pointSignTDO.IsBold = this.displayConfigParam.IsBold.Value;
                if (this.displayConfigParam.IsUnderlined.HasValue)
                    pointSignTDO.IsUnderlined = this.displayConfigParam.IsUnderlined.Value;
                if (this.displayConfigParam.IsItalic.HasValue)
                    pointSignTDO.IsItalic = this.displayConfigParam.IsItalic.Value;
                if (!String.IsNullOrEmpty(this.displayConfigParam.FontName))
                    pointSignTDO.FontName = this.displayConfigParam.FontName;
            }

            return pointSignTDO;
        }

        #endregion
    }
}
