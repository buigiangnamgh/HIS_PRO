using EMR.EFMODEL.DataModels;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.Api;
using Inventec.Common.SignLibrary.DTO;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
using Inventec.Common.SignToolViewer.Integrate;
using Inventec.Common.SignLibrary.SignHandler;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EMR.Filter;
using EMR.SDO;

namespace Inventec.Common.SignLibrary
{
    public class DocumentManager
    {
        Inventec.Common.SignLibrary.ADO.SignToken signToken;

        public DocumentManager()
        {
            this.signToken = new SignToken();
        }

        public DocumentManager(string dti)
        {
            this.signToken = new SignToken();
            InitParam(dti);
        }

        void InitParam(string dti)
        {
            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => dti), dti));
            if (!String.IsNullOrEmpty(dti))
            {
                var arrs = dti.Split(new String[] { "|" }, StringSplitOptions.None);
                if (arrs != null && arrs.Length > 1)
                {
                    GlobalStore.IsUseSendDTI = true;

                    ConstanIG.ACS_BASE_URI = arrs[0];
                    if (arrs.Length > 1)
                        GlobalStore.EMR_BASE_URI = arrs[1];
                    if (arrs.Length > 2)
                        FssConstant.BASE_URI = arrs[2];
                    if (arrs.Length > 3)
                    {
                        GlobalStore.TokenCode = arrs[3];
                        signToken.TokenCode = arrs[3];
                    }
                    if (arrs.Length > 4)
                    {
                        GlobalStore.LoginName = arrs[4];
                        signToken.LoginName = arrs[4];
                    }
                    if (arrs.Length > 5)
                    {
                        GlobalStore.UserName = arrs[5];
                        signToken.UserName = arrs[5];
                    }
                    if (arrs.Length > 6)
                    {
                        GlobalStore.Password = arrs[6];
                        signToken.Password = arrs[6];
                    }

                    RegistryProcessor.Write("ACS_BASE_URI", ConstanIG.ACS_BASE_URI);
                    RegistryProcessor.Write("EMR_BASE_URI", GlobalStore.EMR_BASE_URI);
                    RegistryProcessor.Write("FSS_BASE_URI", FssConstant.BASE_URI);
                }
            }
        }

        string GetTokenCodeData()
        {
            if (signToken != null && !String.IsNullOrEmpty(signToken.TokenCode))
            {
                return signToken.TokenCode;
            }
            else return GlobalStore.TokenCode;
        }

        EMR_SIGNER GetSignerData()
        {
            EMR_SIGNER result = null;

            if (signToken != null && signToken.Singer != null && signToken.Singer.ID > 0)
            {
                result = signToken.Singer;
            }
            else result = GlobalStore.Singer;

            if (result != null && GlobalStore.EMR_HSM_INTEGRATE_OPTION == "5" && String.IsNullOrWhiteSpace(result.HSM_USER_CODE))
            {
                Popup.frmUpdateSigner frm = new Popup.frmUpdateSigner(result, 0);
                frm.ShowDialog();
            }

            return result;
        }

        EMR_TREATMENT GetTreatmentData()
        {
            if (signToken != null && signToken.Treatment != null && signToken.Treatment.ID > 0)
            {
                return signToken.Treatment;
            }
            return null;
        }

        public bool IsDocumentSigned(DocumentSignedDTO documentSignedDTO)
        {
            string outputFile = "";
            return IsDocumentSigned(documentSignedDTO, ref outputFile);
        }

        public bool IsDocumentSigned(DocumentSignedDTO documentSignedDTO, ref string base64FileGigned)
        {
            bool isSigned = false;
            try
            {
                if (documentSignedDTO == null) throw new ArgumentNullException("documentSignedDTO");
                if (String.IsNullOrEmpty(documentSignedDTO.DocumentTypeCode)) throw new ArgumentNullException("DocumentTypeCode");
                if (String.IsNullOrEmpty(documentSignedDTO.TreatmentCode) && String.IsNullOrEmpty(documentSignedDTO.HisCode)) throw new ArgumentNullException("TreatmentCode && HisCode");

                try
                {
                    documentSignedDTO.DocumentTypeCode = string.Format("{0:00}", documentSignedDTO.DocumentTypeCode);
                }
                catch (Exception exx)
                {
                    documentSignedDTO.DocumentTypeCode = documentSignedDTO.DocumentTypeCode.Length == 1 ? "0" + documentSignedDTO.DocumentTypeCode : documentSignedDTO.DocumentTypeCode;
                    Logging.LogSystem.Warn(exx);
                }

                InitUri();

                InputADO inputADO = new InputADO();
                inputADO.Treatment = new TreatmentDTO();
                inputADO.Treatment.TREATMENT_CODE = documentSignedDTO.TreatmentCode;
                if (CheckLogin())
                {
                    Verify.VerifyTreatmentCode(inputADO, ref  signToken);
                    var docType = new EmrDocumentType().GetByCode(documentSignedDTO.DocumentTypeCode);
                    if (docType != null)
                    {
                        base64FileGigned = "";
                        inputADO.DocumentTypeCode = documentSignedDTO.DocumentTypeCode;
                        inputADO.HisCode = documentSignedDTO.HisCode;

                        V_EMR_DOCUMENT documentData = null;

                        Verify.VerifyHisCode(inputADO, false, ref base64FileGigned, ref documentData);
                        if (!String.IsNullOrEmpty(base64FileGigned))
                        {
                            isSigned = true;
                        }
                        else
                        {
                            MessageManager.Show(MessageUitl.GetMessage(MessageConstan.KhongTimThayDuLieuDaKyCuaHoSo));
                            Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO) + "____" + Inventec.Common.Logging.LogUtil.TraceData("base64FileGigned", base64FileGigned));
                            isSigned = false;
                        }
                    }
                    else
                    {
                        MessageManager.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    }
                    Inventec.Common.Logging.LogSystem.Debug("IsDocumentSigned. Kiem tra van ban da ky chua____Input" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO) + "____output____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSigned), isSigned));
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("IsDocumentSigned - Kiem tra trang thai van ban da ky chua. Kiem tra thong tin dang nhap - token phien lam viec that bai. ____Input" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO) + "____output____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSigned), isSigned));
                }
            }
            catch (Exception ex)
            {
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                Inventec.Common.Logging.LogSystem.Debug("Loai van ban truyen vao khong hop le____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO), ex);
            }
            return isSigned;
        }

        public bool IsDocumentSigned(DocumentSignedDTO documentSignedDTO, ref string base64FileGigned, ref string documentCode)
        {
            bool isSigned = false;
            try
            {
                if (documentSignedDTO == null) throw new ArgumentNullException("documentSignedDTO");
                if (String.IsNullOrEmpty(documentSignedDTO.DocumentTypeCode)) throw new ArgumentNullException("DocumentTypeCode");
                if (String.IsNullOrEmpty(documentSignedDTO.TreatmentCode) && String.IsNullOrEmpty(documentSignedDTO.HisCode)) throw new ArgumentNullException("TreatmentCode && HisCode");

                try
                {
                    documentSignedDTO.DocumentTypeCode = string.Format("{0:00}", documentSignedDTO.DocumentTypeCode);
                }
                catch (Exception exx)
                {
                    documentSignedDTO.DocumentTypeCode = documentSignedDTO.DocumentTypeCode.Length == 1 ? "0" + documentSignedDTO.DocumentTypeCode : documentSignedDTO.DocumentTypeCode;
                    Logging.LogSystem.Warn(exx);
                }

                InitUri();

                InputADO inputADO = new InputADO();
                inputADO.Treatment = new TreatmentDTO();
                inputADO.Treatment.TREATMENT_CODE = documentSignedDTO.TreatmentCode;
                if (CheckLogin())
                {
                    Verify.VerifyTreatmentCode(inputADO, ref  signToken);
                    var docType = new EmrDocumentType().GetByCode(documentSignedDTO.DocumentTypeCode);
                    if (docType != null)
                    {
                        base64FileGigned = "";
                        inputADO.DocumentTypeCode = documentSignedDTO.DocumentTypeCode;
                        inputADO.HisCode = documentSignedDTO.HisCode;
                        V_EMR_DOCUMENT documentData = null;
                        Verify.VerifyHisCode(inputADO, false, ref base64FileGigned, ref documentData);
                        if (!String.IsNullOrEmpty(base64FileGigned))
                        {
                            isSigned = true;
                            documentCode = documentData != null ? documentData.DOCUMENT_CODE : "";
                        }
                        else
                        {
                            MessageManager.Show(MessageUitl.GetMessage(MessageConstan.KhongTimThayDuLieuDaKyCuaHoSo));
                            isSigned = false;
                        }
                    }
                    else
                    {
                        MessageManager.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    }
                    Inventec.Common.Logging.LogSystem.Debug("IsDocumentSigned. Kiem tra van ban da ky chua____Input" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO) + "____output____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSigned), isSigned));
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("IsDocumentSigned - Kiem tra trang thai van ban da ky chua. Kiem tra thong tin dang nhap - token phien lam viec that bai. ____Input" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO) + "____output____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSigned), isSigned));
                }
            }
            catch (Exception ex)
            {
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                Inventec.Common.Logging.LogSystem.Debug("Loai van ban truyen vao khong hop le____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO), ex);
            }
            return isSigned;
        }

        public bool IsDocumentSigned(DocumentSignedDTO documentSignedDTO, ref string base64FileGigned, ref string documentCode, ref InputADO inputADO)
        {
            bool isSigned = false;
            try
            {
                if (documentSignedDTO == null) throw new ArgumentNullException("documentSignedDTO");
                if (String.IsNullOrEmpty(documentSignedDTO.DocumentTypeCode)) throw new ArgumentNullException("DocumentTypeCode");
                if (String.IsNullOrEmpty(documentSignedDTO.TreatmentCode) && String.IsNullOrEmpty(documentSignedDTO.HisCode)) throw new ArgumentNullException("TreatmentCode && HisCode");

                try
                {
                    documentSignedDTO.DocumentTypeCode = string.Format("{0:00}", documentSignedDTO.DocumentTypeCode);
                }
                catch (Exception exx)
                {
                    documentSignedDTO.DocumentTypeCode = documentSignedDTO.DocumentTypeCode.Length == 1 ? "0" + documentSignedDTO.DocumentTypeCode : documentSignedDTO.DocumentTypeCode;
                    Logging.LogSystem.Warn(exx);
                }

                InitUri();

                inputADO.Treatment = new TreatmentDTO();
                inputADO.Treatment.TREATMENT_CODE = documentSignedDTO.TreatmentCode;
                if (CheckLogin())
                {
                    Verify.VerifyTreatmentCode(inputADO, ref  signToken);
                    var docType = new EmrDocumentType().GetByCode(documentSignedDTO.DocumentTypeCode);
                    if (docType != null)
                    {
                        base64FileGigned = "";
                        inputADO.DocumentTypeCode = documentSignedDTO.DocumentTypeCode;
                        inputADO.HisCode = documentSignedDTO.HisCode;
                        V_EMR_DOCUMENT documentData = null;
                        Verify.VerifyHisCode(inputADO, false, ref base64FileGigned, ref documentData);
                        if (!String.IsNullOrEmpty(base64FileGigned))
                        {
                            isSigned = true;
                            documentCode = documentData != null ? documentData.DOCUMENT_CODE : "";
                            if (documentData.WIDTH != null && documentData.HEIGHT != null && documentData.RAW_KIND != null)
                            {
                                inputADO.PaperSizeDefault = new System.Drawing.Printing.PaperSize(documentData.PAPER_NAME, (int)documentData.WIDTH, (int)documentData.HEIGHT);
                                if (documentData.RAW_KIND != null)
                                {
                                    inputADO.PaperSizeDefault.RawKind = (int)documentData.RAW_KIND;
                                }
                            }
                        }
                        else
                        {
                            MessageManager.Show(MessageUitl.GetMessage(MessageConstan.KhongTimThayDuLieuDaKyCuaHoSo));
                            isSigned = false;
                        }
                    }
                    else
                    {
                        MessageManager.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    }
                    Inventec.Common.Logging.LogSystem.Debug("IsDocumentSigned. Kiem tra van ban da ky chua____Input" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO) + "____output____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSigned), isSigned));
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("IsDocumentSigned - Kiem tra trang thai van ban da ky chua. Kiem tra thong tin dang nhap - token phien lam viec that bai. ____Input" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO) + "____output____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSigned), isSigned));
                }
            }
            catch (Exception ex)
            {
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                Inventec.Common.Logging.LogSystem.Debug("Loai van ban truyen vao khong hop le____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO), ex);
            }
            return isSigned;
        }

        internal bool CheckLogin()
        {
            if (!String.IsNullOrEmpty(GlobalStore.TokenCode))
            {
                GlobalStore.TokenData = new TokenData();
                GlobalStore.TokenData.TokenCode = GlobalStore.TokenCode;
                GlobalStore.TokenData.User = new UserData();
                GlobalStore.TokenData.User.LoginName = GlobalStore.LoginName;
                GlobalStore.TokenData.User.UserName = GlobalStore.UserName;
                GlobalStore.TokenData.User.ApplicationCode = GlobalStore.appCode;

                GlobalStore.AcsConsumer.SetTokenCode(GlobalStore.TokenData.TokenCode);
                GlobalStore.EmrConsumer.SetTokenCode(GlobalStore.TokenData.TokenCode);

                this.signToken.TokenData = GlobalStore.TokenData;
                this.signToken.UserName = GlobalStore.UserName;
                this.signToken.LoginName = GlobalStore.LoginName;

                var signer = GlobalStore.GetByLoginName(GlobalStore.LoginName);
                GlobalStore.Singer = signer;
                signToken.Singer = signer;
            }
            else
            {
                if (GlobalStore.TokenData == null)
                {
                    frmLogin frmLogin = new frmLogin(ProcessSignTokenData);
                    frmLogin.ShowDialog();
                }
            }
            if (this.signToken != null && (this.signToken.TokenData != null || !String.IsNullOrEmpty(this.signToken.TokenCode)))
            {
                return true;
            }

            return false;
        }

        public bool RefeshAfterLogout()
        {
            bool success = false;
            try
            {
                GlobalStore.TokenCode = null;
                GlobalStore.TokenData = null;
                signToken = new SignToken();

                CommonParam param = new CommonParam();
                Inventec.Common.Integrate.ClientTokenManager clientTokenManager = new Inventec.Common.Integrate.ClientTokenManager(GlobalStore.appCode, ConstanIG.ACS_BASE_URI);
                clientTokenManager.UseRegistry(true);
                clientTokenManager.Logout(param);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return success;
        }

        void ProcessSignTokenData(Inventec.Common.SignLibrary.ADO.SignToken _signToken)
        {
            this.signToken = _signToken;
            if (_signToken != null)
            {
                GlobalStore.TokenData = _signToken.TokenData;
                GlobalStore.LoginName = _signToken.LoginName;
                GlobalStore.UserName = _signToken.UserName;
                GlobalStore.TokenCode = _signToken.TokenCode;
                var signer = GlobalStore.GetByLoginName(GlobalStore.LoginName);
                GlobalStore.Singer = signer;
                signToken.Singer = signer;
            }
        }

        public void ShowDocumentSigned(string documentTypeCode, string hisCode)
        {
            ShowDocumentSigned(new DocumentSignedDTO() { DocumentTypeCode = documentTypeCode, HisCode = hisCode, IsPrintOnlyContent = true });
        }

        public void ShowDocumentSigned(DocumentSignedDTO documentSignedDTO)
        {
            try
            {
                string base64FileGigned = "";
                string documentCode = "";
                InputADO inputADO = new InputADO();
                if (IsDocumentSigned(documentSignedDTO, ref base64FileGigned, ref documentCode, ref inputADO))
                {

                    inputADO.Treatment = new TreatmentDTO();
                    inputADO.DocumentTypeCode = documentSignedDTO.DocumentTypeCode;
                    inputADO.HisCode = documentSignedDTO.HisCode;
                    inputADO.Treatment.TREATMENT_CODE = documentSignedDTO.TreatmentCode;
                    inputADO.DocumentCode = documentCode;
                    inputADO.IsPrint = true;

                    var configs = GlobalStore.EmrConfigs;
                    if (configs != null && configs.Count > 0)
                    {
                        var cfgIsNotShowSignInformations = configs.Where(o => o.KEY == EmrConfigKeys.IS_NOT_SHOWING_SIGN_INFORMATION);
                        var cfgIsNotShowSignInformation = cfgIsNotShowSignInformations != null ? cfgIsNotShowSignInformations.FirstOrDefault() : null;
                        if (cfgIsNotShowSignInformation != null)
                        {
                            string vlIsNotShowSignInformation = !String.IsNullOrEmpty(cfgIsNotShowSignInformation.VALUE) ? cfgIsNotShowSignInformation.VALUE : cfgIsNotShowSignInformation.DEFAULT_VALUE;
                            if (!String.IsNullOrEmpty(vlIsNotShowSignInformation))
                            {
                                inputADO.IsPrintOnlyContent = vlIsNotShowSignInformation == "1";
                            }
                        }
                    }
                    else
                        inputADO.IsPrintOnlyContent = documentSignedDTO.IsPrintOnlyContent ?? true;
                    var barr = Convert.FromBase64String(base64FileGigned);
                    frmPdfViewer frmPdfViewer = new frmPdfViewer(barr, FileType.Pdf, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                    frmPdfViewer.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public UserControl GetUcDocumentSigned(string documentTypeCode, string hisCode)
        {
            return GetUcDocumentSigned(new DocumentSignedDTO() { DocumentTypeCode = documentTypeCode, HisCode = hisCode, IsPrintOnlyContent = true });
        }

        public UserControl GetUcDocumentSigned(DocumentSignedDTO documentSignedDTO)
        {
            try
            {
                string base64FileGigned = "";
                if (IsDocumentSigned(documentSignedDTO, ref base64FileGigned))
                {
                    InputADO inputADO = new InputADO();
                    inputADO.Treatment = new TreatmentDTO();
                    inputADO.Treatment.TREATMENT_CODE = documentSignedDTO.TreatmentCode;
                    inputADO.DocumentTypeCode = documentSignedDTO.DocumentTypeCode;
                    inputADO.HisCode = documentSignedDTO.HisCode;
                    inputADO.IsPrint = true;
                    var configs = GlobalStore.EmrConfigs;
                    if (configs != null && configs.Count > 0)
                    {
                        var cfgIsNotShowSignInformations = configs.Where(o => o.KEY == EmrConfigKeys.IS_NOT_SHOWING_SIGN_INFORMATION);
                        var cfgIsNotShowSignInformation = cfgIsNotShowSignInformations != null ? cfgIsNotShowSignInformations.FirstOrDefault() : null;
                        if (cfgIsNotShowSignInformation != null)
                        {
                            string vlIsNotShowSignInformation = !String.IsNullOrEmpty(cfgIsNotShowSignInformation.VALUE) ? cfgIsNotShowSignInformation.VALUE : cfgIsNotShowSignInformation.DEFAULT_VALUE;
                            if (!String.IsNullOrEmpty(vlIsNotShowSignInformation))
                            {
                                inputADO.IsPrintOnlyContent = vlIsNotShowSignInformation == "1";
                            }
                        }
                    }
                    else
                        inputADO.IsPrintOnlyContent = documentSignedDTO.IsPrintOnlyContent ?? true;
                    var barr = Convert.FromBase64String(base64FileGigned);
                    return new UCViewer(barr, FileType.Pdf, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn("Xem lai file da ky that bai____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignedDTO), documentSignedDTO), ex);
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
            }
            return null;
        }

        public void GetFileDocumentMergeWithUri(V_EMR_DOCUMENT document, List<long> documentIds, ref string outPdfFile)
        {
            try
            {
                if (document == null) throw new ArgumentNullException("document");
                if (String.IsNullOrEmpty(document.MERGE_CODE)) throw new ArgumentNullException("mergeCode");
                if (String.IsNullOrEmpty(document.TREATMENT_CODE)) throw new ArgumentNullException("treatmentCode");

                InitUri();
                if (CheckLogin())
                {
                    outPdfFile = GetFileDocumentMerge((document.ORIGINAL_HIGH ?? 0), document.TREATMENT_CODE, document.MERGE_CODE, document.DOCUMENT_NAME,"", documentIds);
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        public void GetFileDocumentMergeWithUri(V_EMR_DOCUMENT document, List<long> documentIds, ref byte[] outPdfByte)
        {
            try
            {
                if (document == null) throw new ArgumentNullException("document");
                if (String.IsNullOrEmpty(document.MERGE_CODE)) throw new ArgumentNullException("mergeCode");
                if (String.IsNullOrEmpty(document.TREATMENT_CODE)) throw new ArgumentNullException("treatmentCode");

                InitUri();
                if (CheckLogin())
                {
                    var documentMergeResult = new EmrDocument().GetDocumentByMergeCode(document.MERGE_CODE, document.DOCUMENT_NAME, documentIds);
                    if (documentMergeResult != null && documentMergeResult.Length > 0)
                    {
                        outPdfByte = documentMergeResult;
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Warn("Khong tim thay van ban nao theo mergecode truyen vao____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => document), document));
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        public string GetFileDocumentMerge(decimal oginalHeight, string treatmentCode, string mergeCode, string documentName = "", string documentCode = "", List<long> documentIds = null)
        {
            string outPdfFile = "";
            try
            {
                var documentMergeResult = new EmrDocument().GetDocumentByMergeCode(mergeCode, documentName, documentIds);
                if (documentMergeResult != null && documentMergeResult.Length > 0)
                {
                    outPdfFile = Utils.GenerateTempFileWithin();
                    Utils.ByteToFile(documentMergeResult, outPdfFile);
                    //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outPdfFile), outPdfFile));
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Warn("Khong tim thay van ban nao theo mergecode truyen vao____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => mergeCode), mergeCode));
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
            return outPdfFile;
        }

        public UserControl GetUcDocumentMerge(decimal oginalHeight, string treatmentCode, string mergeCode, ref string outPdfFile)
        {
            try
            {
                if (String.IsNullOrEmpty(mergeCode)) throw new ArgumentNullException("mergeCode");
                if (String.IsNullOrEmpty(treatmentCode)) throw new ArgumentNullException("treatmentCode");
                if (oginalHeight < 0) throw new ArgumentNullException("oginalHeight");

                InitUri();
                if (CheckLogin())
                {
                    outPdfFile = GetFileDocumentMerge(oginalHeight, treatmentCode, mergeCode, "","");
                    if (!String.IsNullOrEmpty(outPdfFile) && File.Exists(outPdfFile))
                    {
                        InputADO inputADO = new InputADO();
                        inputADO.Treatment = new TreatmentDTO();
                        inputADO.Treatment.TREATMENT_CODE = treatmentCode;
                        inputADO.DocumentTypeCode = "";
                        inputADO.HisCode = "";
                        inputADO.IsPrint = true;
                        inputADO.IsPrintOnlyContent = true;
                        inputADO.IsSignConfig = false;
                        return new UCViewer(outPdfFile, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("GetUcDocumentMerge - Xem van ban gop theo mergeCode that bai. Kiem tra thong tin dang nhap - token phien lam viec that bai. ____Input" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => mergeCode), mergeCode) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => oginalHeight), oginalHeight));
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn("Xem van ban gop theo mergeCode that bai____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => mergeCode), mergeCode) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => oginalHeight), oginalHeight), ex);
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
            }
            return null;
        }

        public UserControl GetUcDocumentMerge(V_EMR_DOCUMENT document, ref string outPdfFile, bool IsMergeByName = false)
        {
            try
            {
                if (document == null) throw new ArgumentNullException("document");
                if (String.IsNullOrEmpty(document.MERGE_CODE)) throw new ArgumentNullException("mergeCode");
                if (String.IsNullOrEmpty(document.TREATMENT_CODE)) throw new ArgumentNullException("treatmentCode");
                //if (document.ORIGINAL_HIGH < 0) throw new ArgumentNullException("oginalHeight");

                InitUri();
                if (CheckLogin())
                {
                    outPdfFile = GetFileDocumentMerge((document.ORIGINAL_HIGH ?? 0), document.TREATMENT_CODE, document.MERGE_CODE,  IsMergeByName ? document.DOCUMENT_NAME : "", "");
                    if (!String.IsNullOrEmpty(outPdfFile) && File.Exists(outPdfFile))
                    {
                        InputADO inputADO = new InputADO();
                        inputADO.Treatment = new TreatmentDTO();
                        inputADO.Treatment.TREATMENT_CODE = document.TREATMENT_CODE;
                        inputADO.DocumentTypeCode = "";
                        inputADO.HisCode = "";
                        inputADO.IsPrint = true;
                        inputADO.IsPrintOnlyContent = true;
                        inputADO.IsSignConfig = false;
                        if (!String.IsNullOrEmpty(document.PAPER_NAME) && document.RAW_KIND.HasValue && document.WIDTH.HasValue && document.HEIGHT.HasValue)
                        {
                            inputADO.PaperSizeDefault = new System.Drawing.Printing.PaperSize(document.PAPER_NAME, (int)document.WIDTH, (int)document.HEIGHT);
                            inputADO.PaperSizeDefault.RawKind = document.RAW_KIND.Value;
                        }
                        return new UCViewer(outPdfFile, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("GetUcDocumentMerge - Xem van ban gop theo mergeCode that bai. Kiem tra thong tin dang nhap - token phien lam viec that bai. ____Input" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => document.MERGE_CODE), document.MERGE_CODE) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => document.TREATMENT_CODE), document.TREATMENT_CODE));
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn("Xem van ban gop theo mergeCode that bai____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => document), document), ex);
                MessageManager.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
            }
            return null;
        }

        void InitUri()
        {
            if (String.IsNullOrEmpty(ConstanIG.ACS_BASE_URI) && !String.IsNullOrEmpty((string)RegistryProcessor.Read("ACS_BASE_URI")))
            {
                ConstanIG.ACS_BASE_URI = (string)RegistryProcessor.Read("ACS_BASE_URI");
            }

            if (String.IsNullOrEmpty(GlobalStore.EMR_BASE_URI) && !String.IsNullOrEmpty((string)RegistryProcessor.Read("EMR_BASE_URI")))
            {
                GlobalStore.EMR_BASE_URI = (string)RegistryProcessor.Read("EMR_BASE_URI");
            }

            if (String.IsNullOrEmpty(FssConstant.BASE_URI) && !String.IsNullOrEmpty((string)RegistryProcessor.Read("FSS_BASE_URI")))
            {
                FssConstant.BASE_URI = (string)RegistryProcessor.Read("FSS_BASE_URI");
            }
            if (!String.IsNullOrEmpty(ConstanIG.ACS_BASE_URI) && !String.IsNullOrEmpty(GlobalStore.EMR_BASE_URI) && !String.IsNullOrEmpty(FssConstant.BASE_URI))
            {
                GlobalStore.IsUseSendDTI = true;
            }
            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ConstanIG.ACS_BASE_URI), ConstanIG.ACS_BASE_URI) + "__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => GlobalStore.EMR_BASE_URI), GlobalStore.EMR_BASE_URI) + "__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => FssConstant.BASE_URI), FssConstant.BASE_URI));
        }
    }
}
