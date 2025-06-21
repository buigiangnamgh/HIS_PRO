using EMR.EFMODEL.DataModels;
using EMR.SDO;
using EMR.TDO;
using EMR.WCF.DCO;
using Inventec.Common.Integrate;
using Inventec.Common.SignFile;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.Api;
using Inventec.Common.SignLibrary.DTO;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
using Inventec.Common.SignLibrary.ServiceSign;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary.SignHandler
{
    internal class SignUSBHandler : BussinessBase
    {
        internal SignUSBHandler() : base() { }
        internal SignUSBHandler(CommonParam param, InputADO inputADOWorking, bool isSignParanel, EMR.EFMODEL.DataModels.EMR_TREATMENT treatment, EMR.EFMODEL.DataModels.EMR_SIGNER singer, string tokenCode) : base(param, inputADOWorking, "", isSignParanel, treatment, singer, tokenCode) { }

        internal bool SignWithCreateDoc(string outputFile, ref DocumentTDO document, string documentName, string treatmentCode, List<SignTDO> signStrategys, List<SignTDO> signTemps, string base64SignedFileData, string GetBase64OriginalFileData, long? documentTypeId, bool isMultiSign, string hisCode, EMR_SIGN _signSelected, string signDescription, string mergeCode = "")
        {
            bool success = false;

            UsbSignCreateTDO usbSignCreateTDO = new UsbSignCreateTDO();
            usbSignCreateTDO.Signs = String.IsNullOrEmpty(inputADOWorking.BusinessCode) ? signTemps : null;
            if (String.IsNullOrEmpty(inputADOWorking.BusinessCode) && signStrategys != null && signStrategys.Count > 0)
            {
                if (usbSignCreateTDO.Signs == null)
                {
                    usbSignCreateTDO.Signs = new List<SignTDO>();
                }
                usbSignCreateTDO.Signs.AddRange(signStrategys);
            }

            if (usbSignCreateTDO.Signs != null)
            {
                if (usbSignCreateTDO.Signs.FirstOrDefault() != null && !string.IsNullOrEmpty(usbSignCreateTDO.Signs[0].PatientCode))
                {
                    this.param.Messages.Add("Bạn chưa đến lượt ký.");
                    return success;
                }
                foreach (var sg in usbSignCreateTDO.Signs)
                {
                    sg.Version = new VersionTDO();
                    sg.Version.Base64Data = base64SignedFileData;
                    if (Signer != null && (sg.Loginname == Signer.LOGINNAME))
                    {
                        sg.Description = signDescription;
                    }
                }
            }
            if (this.FileType == SignLibrary.FileType.Xml)
            {
                usbSignCreateTDO.FileType = EMR.TDO.FileType.XML;
            }
            else if (this.FileType == SignLibrary.FileType.Json)
            {
                usbSignCreateTDO.FileType = EMR.TDO.FileType.JSON;
            }
            else
            {
                usbSignCreateTDO.FileType = EMR.TDO.FileType.PDF;
            }
            usbSignCreateTDO.MergeCode = mergeCode;
            usbSignCreateTDO.RoomCode = inputADOWorking.RoomCode;
            usbSignCreateTDO.RoomTypeCode = inputADOWorking.RoomTypeCode;
            usbSignCreateTDO.DependentCode = inputADOWorking.DependentCode;
            usbSignCreateTDO.ParentDependentCode = inputADOWorking.ParentDependentCode;
            usbSignCreateTDO.IsSignParallel = this.IsSignParanel;
            usbSignCreateTDO.OriginalVersion = new VersionTDO();

            if (document != null && !String.IsNullOrEmpty(document.DocumentCode))
            {
                usbSignCreateTDO.DocumentCode = document.DocumentCode;
                usbSignCreateTDO.DocumentName = document.DocumentName;
                if (document.DocumentTypeId.HasValue && document.DocumentTypeId.Value > 0)
                {
                    usbSignCreateTDO.DocumentTypeId = document.DocumentTypeId;
                }
                usbSignCreateTDO.OriginalVersion.DocumentCode = document.DocumentCode;
            }
            else
            {
                usbSignCreateTDO.DocumentName = (String.IsNullOrEmpty(documentName) ? ("Ký điện tử cho hồ sơ có mã " + treatmentCode + " ngày " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")) : documentName);
                if (documentTypeId.HasValue && documentTypeId.Value > 0)
                {
                    usbSignCreateTDO.DocumentTypeId = documentTypeId;
                }
                usbSignCreateTDO.HisCode = hisCode;
                if (this.inputADOWorking.DocumentTime != null && this.inputADOWorking.DocumentTime.Value != DateTime.MinValue)
                {
                    usbSignCreateTDO.DocumentTime = DateTimeConvert.SystemDateTimeToTimeNumber(this.inputADOWorking.DocumentTime);
                }
            }
            if (!String.IsNullOrEmpty(this.inputADOWorking.DocumentGroupCode))
            {
                var docGroup = new EmrDocumentGroup().GetByCode(this.inputADOWorking.DocumentGroupCode);
                usbSignCreateTDO.DocumentGroupId = docGroup != null ? (long?)docGroup.ID : null;
            }
            usbSignCreateTDO.TreatmentCode = treatmentCode;
            usbSignCreateTDO.OriginalVersion.Base64Data = GetBase64OriginalFileData;
            usbSignCreateTDO.IsFinishSign = !isMultiSign;
            usbSignCreateTDO.IsSigning = isMultiSign;
            usbSignCreateTDO.BusinessCode = inputADOWorking.BusinessCode;
            usbSignCreateTDO.HisOrder = inputADOWorking.HisOrder;

            if (inputADOWorking.PaperSizeDefault != null)
            {
                usbSignCreateTDO.PaperName = inputADOWorking.PaperSizeDefault.PaperName;
                if (String.IsNullOrEmpty(usbSignCreateTDO.PaperName))
                {
                    usbSignCreateTDO.PaperName = this.inputADOWorking.PaperSizeDefault.Kind.ToString();
                }
                usbSignCreateTDO.Width = inputADOWorking.PaperSizeDefault.Width;
                usbSignCreateTDO.Height = inputADOWorking.PaperSizeDefault.Height;
                usbSignCreateTDO.RawKind = inputADOWorking.PaperSizeDefault.RawKind;
            }

            if (!VerifyDataPreCallApi(usbSignCreateTDO))
            {
                param.Messages.Add(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                MessageManager.Show(param, false);
                return false;
            }

            CommonParam paramCreate = new CommonParam();
            var rs = new EmrDocument(paramCreate).CreateAndSignUsb(TokenCode, usbSignCreateTDO);
            if (rs != null)
            {
                success = true;
                document.DocumentCode = rs.DocumentCode;
                document.DocumentName = rs.DocumentName;
                document.DocumentTypeId = rs.DocumentTypeId;
                document.TreatmentCode = rs.TreatmentCode;
                document.DependentCode = rs.DependentCode;
                document.ParentDependentCode = rs.ParentDependentCode;

                document.PaperName = rs.PaperName;
                document.Width = rs.Width;
                document.Height = rs.Height;
                document.RawKind = rs.RawKind;
            }
            else
            {
                if (paramCreate.Messages != null && paramCreate.Messages.Count > 0)
                    this.param.Messages.AddRange(paramCreate.Messages);
                if (paramCreate.BugCodes != null && paramCreate.BugCodes.Count > 0)
                    this.param.BugCodes.AddRange(paramCreate.BugCodes);
            }
            return success;
        }

        internal bool SignOnly(ref DocumentTDO document, bool isMultiSign, List<SignTDO> signStrategys, List<SignTDO> signTemps, string base64SignedFileData, EMR_SIGN _signSelected, string signReason, string mergeCode = "")
        {
            bool success = false;
            try
            {
                EmrSignUsbSDO emrSignUsbSDO = new EmrSignUsbSDO();

                var doc = new EmrDocument().GetViewByCode(document.DocumentCode);
                emrSignUsbSDO.EmrDocumentId = doc.ID;

                var sign = _signSelected != null ? _signSelected : new EmrSign().GetSignDocumentFirst(doc, Signer, Treatment, true);
                emrSignUsbSDO.EmrSignId = sign.ID;
                emrSignUsbSDO.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                emrSignUsbSDO.Base64FileData = base64SignedFileData;
                emrSignUsbSDO.IsFinishSign = !isMultiSign;
                emrSignUsbSDO.IsSigning = isMultiSign;
                emrSignUsbSDO.RoomCode = inputADOWorking.RoomCode;
                emrSignUsbSDO.RoomTypeCode = inputADOWorking.RoomTypeCode;
                emrSignUsbSDO.Description = signReason;

                //TODO
                //if (this.FileType == SignLibrary.FileType.Xml)
                //{
                //    emrSignUsbSDO.FileType = EMR.TDO.FileType.XML;
                //}
                //else if (this.FileType == SignLibrary.FileType.Json)
                //{
                //    emrSignUsbSDO.FileType = EMR.TDO.FileType.JSON;
                //}
                //else
                //{
                //    emrSignUsbSDO.FileType = EMR.TDO.FileType.PDF;
                //}
                CommonParam paramCreate = new CommonParam();
                var rs = new EmrDocument(paramCreate).SignUsb(TokenCode, emrSignUsbSDO);
                if (rs != null)
                {
                    success = true;
                }
                else
                {
                    if (paramCreate.Messages != null && paramCreate.Messages.Count > 0)
                        this.param.Messages.AddRange(paramCreate.Messages);
                    if (paramCreate.BugCodes != null && paramCreate.BugCodes.Count > 0)
                        this.param.BugCodes.AddRange(paramCreate.BugCodes);
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
                param.Messages.Add(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                success = false;
            }

            return success;
        }

        private IntPtr? ActiveTopFormHandle()
        {
            IntPtr? handle = null;
            try
            {
                Form active = null;
                var a = Application.OpenForms.Cast<Form>().ToList();//.First(x => x.Focused);
                if (a != null && a.Count > 0)
                {
                    for (int i = (Application.OpenForms.Count - 1); i >= 0; i--)
                    {
                        if (Application.OpenForms[i].Name == "frmWaitForm" || String.IsNullOrEmpty(Application.OpenForms[i].Name)) continue;
                        active = Application.OpenForms[i];
                        break;
                    }
                }


                if (active != null)
                {
                    active.Activate();
                    handle = active.Handle;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return handle;
        }

        internal bool SignFileWithUSBTokenTSAWithCallService(ref string outFileName, string src, Stream stream, float x, float y, int pageNumberCurrent, int totalPageNumber, string signReason, DisplayConfigDTO displayConfigParam, Action dlgCancel)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("SignFileWithUSBTokenTSAWithCallService.1");
                string errMessage = "";
                string outputName = Utils.GenerateTempFileWithin();

                TimestampConfig timestampConfig = new TimestampConfig();
                timestampConfig.UseTimestamp = GlobalStore.IsUseTimespan;
                timestampConfig.TsaUrl = "http://ca.gov.vn/tsa";

                DisplayConfig displayConfig = new DisplayConfig() { CoorXRectangle = x, CoorYRectangle = y, NumberPageSign = pageNumberCurrent, MaxPageSign = totalPageNumber, Location = ((displayConfigParam != null && !String.IsNullOrEmpty(displayConfigParam.Location)) ? displayConfigParam.Location : (Signer != null ? (Signer.DEPARTMENT_NAME + "|" + Signer.TITLE) : "")) };

                if (Signer != null && Signer.SIGN_IMAGE != null)
                {
                    displayConfig.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT;
                    displayConfig.BImage = Signer.SIGN_IMAGE;
                }

                if (displayConfigParam != null)
                {
                    if (displayConfigParam.WidthRectangle.HasValue)
                    {
                        displayConfig.WidthRectangle = displayConfigParam.WidthRectangle.Value;
                    }
                    if (displayConfigParam.HeightRectangle.HasValue)
                    {
                        displayConfig.HeightRectangle = displayConfigParam.HeightRectangle.Value;
                    }
                    if (displayConfigParam.SizeFont.HasValue)
                    {
                        displayConfig.SizeFont = displayConfigParam.SizeFont.Value;
                    }
                    if (displayConfigParam.TextPosition.HasValue)
                    {
                        displayConfig.TextPosition = (Inventec.Common.SignFile.Constans.TEXT_POSITON)displayConfigParam.TextPosition.Value;
                    }
                    if (displayConfigParam.TypeDisplay.HasValue)
                    {
                        displayConfig.TypeDisplay = displayConfigParam.TypeDisplay.Value;
                    }
                    if (displayConfigParam.IsDisplaySignature.HasValue)
                    {
                        displayConfig.IsDisplaySignature = displayConfigParam.IsDisplaySignature.Value;
                    }
                    if (!String.IsNullOrEmpty(displayConfigParam.FormatRectangleText))
                    {
                        displayConfig.FormatRectangleText = displayConfigParam.FormatRectangleText;
                    }
                    if (displayConfigParam.Titles != null)
                    {
                        displayConfig.Titles = displayConfigParam.Titles;
                    }
                    if (displayConfig.TextFormat == null)
                        displayConfig.TextFormat = new FontConfig();
                    if (displayConfigParam.Alignment != null)
                    {
                        displayConfig.TextFormat.Alignment = (Inventec.Common.SignFile.ALIGNMENT_OPTION)displayConfigParam.Alignment;
                    }
                    if (displayConfigParam.IsBold != null)
                    {
                        displayConfig.TextFormat.IsBold = displayConfigParam.IsBold.Value;
                    }
                    if (displayConfigParam.IsItalic != null)
                    {
                        displayConfig.TextFormat.IsItalic = displayConfigParam.IsItalic.Value;
                    }
                    if (displayConfigParam.IsUnderlined != null)
                    {
                        displayConfig.TextFormat.IsUnderlined = displayConfigParam.IsUnderlined.Value;
                    }
                    if (!string.IsNullOrEmpty(displayConfigParam.FontName))
                    {
                        displayConfig.TextFormat.FontName = displayConfigParam.FontName;
                    }
                }
                displayConfig.PageSize = null;//Cần gán về null đề khi gọi service sign truyền dữ liệu sang không bị lỗi không DeserializeObject do class DisplayConfig co chứa kiểu dữ liệu đặc biệt

                if (GlobalStore.EMR_SIGN_SIGN_DESCRIPTION_INFO == "1")
                {
                    displayConfig.IsDisplaySignNote = true;
                }

                WcfSignDCO wcfSignDCO = new WcfSignDCO();
                wcfSignDCO.IsUseTimespan = GlobalStore.IsUseTimespan;
                wcfSignDCO.OutputFile = StringCompressorParse.CompressString(outputName);
                wcfSignDCO.PIN = GlobalStore.PIN;
                wcfSignDCO.SignReason = StringCompressorParse.CompressString(signReason);
                wcfSignDCO.SourceFile = StringCompressorParse.CompressString(src);
                wcfSignDCO.HwndParent = ActiveTopFormHandle();
                var configs = GlobalStore.EmrConfigs;
                if (configs != null && configs.Count > 0)
                {
                    var cfgSignOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR__EMR_SIGN_SIGNING_OPTION);
                    var cfgSignOption = cfgSignOptions != null ? cfgSignOptions.FirstOrDefault() : null;
                    if (cfgSignOption != null)
                    {
                        string vlSignOption = !String.IsNullOrEmpty(cfgSignOption.VALUE) ? cfgSignOption.VALUE : cfgSignOption.DEFAULT_VALUE;
                        wcfSignDCO.EmrSigningOption = vlSignOption;
                    }
                }

                if (wcfSignDCO.EmrSigningOption == "2")
                {
                    if (Signer != null && Signer.SIGN_IMAGE != null && String.IsNullOrEmpty(Signer.PCA_SERIAL))
                    {
                        displayConfig.Contact = (Signer != null ? (Signer.USERNAME) : "");
                    }
                    else
                    {
                        wcfSignDCO.EmrSigningOption = "";
                    }
                }

                if (Signer != null && Signer.SIGNATURE_DISPLAY_TYPE != null)
                {
                    //0: không hiển thị chữ ký, 1: chỉ hiển thị thông tin ký, 2: chỉ hiển thị ảnh chữ ký, 3: hiển thị cả ảnh & thông tin ký
                    long dType = Signer.SIGNATURE_DISPLAY_TYPE.Value;
                    switch (dType)
                    {
                        case 0:
                            displayConfig.IsDisplaySignature = false;
                            break;
                        case 1:
                            displayConfig.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT;
                            break;
                        case 2:
                            if (Signer != null && Signer.SIGN_IMAGE != null)
                            {
                                displayConfig.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP;
                            }
                            else
                            {
                                displayConfig.IsDisplaySignature = false;
                            }
                            break;
                        case 3:
                            if (Signer != null && Signer.SIGN_IMAGE != null)
                            {
                                displayConfig.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT;
                            }
                            else
                            {
                                displayConfig.IsDisplaySignature = false;
                            }
                            break;
                        default:
                            break;
                    }

                }

                if (Signer != null && Signer.SIGNALTURE_IMAGE_WIDTH.HasValue && Signer.SIGNALTURE_IMAGE_WIDTH > 0)
                {
                    displayConfig.SignaltureImageWidth = (float)Signer.SIGNALTURE_IMAGE_WIDTH.Value;
                }

                wcfSignDCO.DisplayConfig = StringCompressorParse.CompressString(Newtonsoft.Json.JsonConvert.SerializeObject(displayConfig));


                string jsonData = JsonConvert.SerializeObject(wcfSignDCO);
                SignProcessorClient signProcessorClient = new SignProcessorClient();
                var wcfSignResultDCO = signProcessorClient.SignExecute(jsonData);
                if (wcfSignResultDCO != null)
                {
                    success = wcfSignResultDCO.Success;
                    outFileName = wcfSignResultDCO.Success ? StringCompressorParse.DecompressString(wcfSignResultDCO.OutputFile) : "";
                    errMessage = StringCompressorParse.DecompressString(wcfSignResultDCO.Message);
                }

                if (!success)
                {
                    CommonParam commonParam = new CommonParam();
                    commonParam.Messages = new List<string>();
                    if (!String.IsNullOrEmpty(errMessage))
                    {
                        commonParam.Messages.Add(errMessage);
                    }
                    commonParam.Messages.Add(MessageUitl.GetMessage(MessageConstan.KySuDungUSBTokenThatBai));
                    MessageManager.Show(commonParam, false);
                }

                if (dlgCancel != null) dlgCancel();

                Inventec.Common.Logging.LogSystem.Info("SignFileWithUSBTokenTSAWithCallService.2");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }

        internal bool SignFileWithUSBTokenTSAWithUsingUsbTokenDevice(ref string outFileName, string src, Stream stream, float x, float y, int pageNumberCurrent, int totalPageNumber, string signReason, DisplayConfigDTO displayConfigParam, Action dlgCancel)
        {
            bool success = false;
            try
            {
                SignPdfFile signer = new SignPdfFile();
                string errMessage = "";
                if (signer != null)
                {
                    bool isSignElectronic = false;
                    var configs = GlobalStore.EmrConfigs;
                    if (configs != null && configs.Count > 0)
                    {
                        var cfgSignOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR__EMR_SIGN_SIGNING_OPTION);
                        var cfgSignOption = cfgSignOptions != null ? cfgSignOptions.FirstOrDefault() : null;
                        if (cfgSignOption != null)
                        {
                            string vlSignOption = !String.IsNullOrEmpty(cfgSignOption.VALUE) ? cfgSignOption.VALUE : cfgSignOption.DEFAULT_VALUE;
                            if (vlSignOption == "2")
                            {
                                isSignElectronic = true;
                            }
                        }
                    }

                    DisplayConfig displayConfig = new DisplayConfig()
                    {
                        CoorXRectangle = x,
                        CoorYRectangle = y,
                        NumberPageSign = pageNumberCurrent,
                        MaxPageSign = totalPageNumber,
                        Location = ((displayConfigParam != null && !String.IsNullOrEmpty(displayConfigParam.Location)) ? displayConfigParam.Location : (Signer != null ? (Signer.DEPARTMENT_NAME + "|" + Signer.TITLE) : ""))
                    };

                    if (Signer != null && Signer.SIGN_IMAGE != null)
                    {
                        displayConfig.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT;
                        displayConfig.BImage = Signer.SIGN_IMAGE;
                    }

                    if (displayConfigParam != null)
                    {
                        if (displayConfigParam.WidthRectangle.HasValue)
                        {
                            displayConfig.WidthRectangle = displayConfigParam.WidthRectangle.Value;
                        }
                        if (displayConfigParam.HeightRectangle.HasValue)
                        {
                            displayConfig.HeightRectangle = displayConfigParam.HeightRectangle.Value;
                        }
                        if (displayConfigParam.SizeFont.HasValue)
                        {
                            displayConfig.SizeFont = displayConfigParam.SizeFont.Value;
                        }
                        if (displayConfigParam.TextPosition.HasValue)
                        {
                            displayConfig.TextPosition = (Inventec.Common.SignFile.Constans.TEXT_POSITON)displayConfigParam.TextPosition.Value;
                        }
                        if (displayConfigParam.TypeDisplay.HasValue)
                        {
                            displayConfig.TypeDisplay = displayConfigParam.TypeDisplay.Value;
                        }
                        if (displayConfigParam.IsDisplaySignature.HasValue)
                        {
                            displayConfig.IsDisplaySignature = displayConfigParam.IsDisplaySignature.Value;
                        }
                        if (!String.IsNullOrEmpty(displayConfigParam.FormatRectangleText))
                        {
                            displayConfig.FormatRectangleText = displayConfigParam.FormatRectangleText;
                        }
                        if (displayConfigParam.Titles != null)
                        {
                            displayConfig.Titles = displayConfigParam.Titles;
                        }
                        if (displayConfig.TextFormat == null)
                            displayConfig.TextFormat = new FontConfig();
                        if (displayConfigParam.Alignment != null)
                        {
                            displayConfig.TextFormat.Alignment = (Inventec.Common.SignFile.ALIGNMENT_OPTION)displayConfigParam.Alignment;
                        }
                        if (displayConfigParam.IsBold != null)
                        {
                            displayConfig.TextFormat.IsBold = displayConfigParam.IsBold.Value;
                        }
                        if (displayConfigParam.IsItalic != null)
                        {
                            displayConfig.TextFormat.IsItalic = displayConfigParam.IsItalic.Value;
                        }
                        if (displayConfigParam.IsUnderlined != null)
                        {
                            displayConfig.TextFormat.IsUnderlined = displayConfigParam.IsUnderlined.Value;
                        }
                        if (!string.IsNullOrEmpty(displayConfigParam.FontName))
                        {
                            displayConfig.TextFormat.FontName = displayConfigParam.FontName;
                        }
                    }

                    if (Signer != null && Signer.SIGNATURE_DISPLAY_TYPE != null)
                    {
                        //0: không hiển thị chữ ký, 1: chỉ hiển thị thông tin ký, 2: chỉ hiển thị ảnh chữ ký, 3: hiển thị cả ảnh & thông tin ký
                        long dType = Signer.SIGNATURE_DISPLAY_TYPE.Value;
                        switch (dType)
                        {
                            case 0:
                                displayConfig.IsDisplaySignature = false;
                                break;
                            case 1:
                                displayConfig.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT;
                                break;
                            case 2:
                                if (Signer != null && Signer.SIGN_IMAGE != null)
                                {
                                    displayConfig.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP;
                                }
                                else
                                {
                                    displayConfig.IsDisplaySignature = false;
                                }
                                break;
                            case 3:
                                if (Signer != null && Signer.SIGN_IMAGE != null)
                                {
                                    displayConfig.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT;
                                }
                                else
                                {
                                    displayConfig.IsDisplaySignature = false;
                                }
                                break;
                            default:
                                break;
                        }

                    }

                    if (Signer != null && Signer.SIGNALTURE_IMAGE_WIDTH.HasValue && Signer.SIGNALTURE_IMAGE_WIDTH > 0)
                    {
                        displayConfig.SignaltureImageWidth = (float)Signer.SIGNALTURE_IMAGE_WIDTH.Value;
                    }

                    string outputName = Utils.GenerateTempFileWithin();

                    if (GlobalStore.EMR_SIGN_SIGN_DESCRIPTION_INFO == "1")
                    {
                        displayConfig.IsDisplaySignNote = true;
                    }

                    bool hasCertificate = CertUtil.CheckHasCertificate(requirePrivateKey: true, validOnly: false);
                    if (hasCertificate)
                    {
                        var cert = CertUtil.GetByDialog(requirePrivateKey: true, validOnly: false);
                        if (cert != null)
                        {
                            TimestampConfig timestampConfig = new TimestampConfig();
                            timestampConfig.UseTimestamp = GlobalStore.IsUseTimespan;
                            timestampConfig.TsaUrl = "http://ca.gov.vn/tsa";


                            if (!String.IsNullOrEmpty(src))
                            {
                                //success = signer.SignPDF(cert, src, outputName, signReason, "", timestampConfig, displayConfig, null, ref errMessage, GlobalStore.PIN);
                                if (this.FileType == SignLibrary.FileType.Xml)
                                {
                                    byte[] resultFileSigned = null;
                                    XmlConfig xmlConfig = new XmlConfig();
                                    xmlConfig.Reason = signReason;
                                    success = signer.SignXml(cert, File.ReadAllBytes(src), ref resultFileSigned, xmlConfig, null, ref errMessage, GlobalStore.PIN);

                                }
                                else if (this.FileType == SignLibrary.FileType.Json)
                                {
                                    using (var memoStream = new MemoryStream())
                                    {
                                        success = signer.SignJson(cert, src, memoStream, signReason, "", displayConfig, null, ref errMessage, GlobalStore.PIN);
                                    }
                                }
                                else
                                {
                                    success = signer.SignPDF(cert, src, outputName, signReason, "", timestampConfig, displayConfig, null, ref errMessage, GlobalStore.PIN);
                                }
                            }
                            else
                            {
                                //if (this.FileType == SignLibrary.FileType.Xml)
                                //{
                                //    byte[] resultFileSigned = null;
                                //    XmlConfig xmlConfig = new XmlConfig();
                                //    xmlConfig.Reason = signReason;
                                //    success = signer.SignXml(cert, File.ReadAllBytes(src), ref resultFileSigned, xmlConfig, null, ref errMessage, GlobalStore.PIN);

                                //}
                                //else if (this.FileType == SignLibrary.FileType.Json)
                                //{
                                //    using (var memoStream = new MemoryStream())
                                //    {
                                //        success = signer.SignJson(cert, src, memoStream, signReason, "", displayConfig, null, ref errMessage, GlobalStore.PIN);
                                //    }
                                //}
                                //else
                                //{
                                success = signer.SignPDF(cert, stream, outputName, signReason, "", timestampConfig, displayConfig, null, ref errMessage, false);
                                //}
                            }

                            if (success)
                            {
                                outFileName = outputName;
                            }
                        }
                    }
                    else
                    {
                        if (isSignElectronic && Signer != null && Signer.SIGN_IMAGE != null && String.IsNullOrEmpty(Signer.PCA_SERIAL))
                        {
                            displayConfig.Contact = (Signer != null ? (Signer.USERNAME) : "");
                            if (!String.IsNullOrEmpty(src))
                            {
                                success = signer.SignPDF(null, src, outputName, signReason, "", null, displayConfig, null, ref errMessage, GlobalStore.PIN, isSignElectronic);
                            }
                            else
                            {
                                success = signer.SignPDF(null, stream, outputName, signReason, "", null, displayConfig, null, ref errMessage, isSignElectronic);
                            }

                            if (success)
                            {
                                outFileName = outputName;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not found Certificate");
                            Inventec.Common.Logging.LogSystem.Warn("Not found Certificate");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Only documents of type pdf, docx, xlsx or pptx can be digitally signed");
                    Inventec.Common.Logging.LogSystem.Warn("Only documents of type pdf, docx, xlsx or pptx can be digitally signed");
                }
                if (dlgCancel != null) dlgCancel();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }

        internal void SetFileType(FileType _fileType)
        {
            this.FileType = _fileType;
        }

        internal bool VerifyServiceSignProcessorIsRunning()
        {
            bool valid = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("VerifyServiceSignProcessorIsRunning.1");
                string exeSignPath = Utils.AppFilePathSignService();
                if (File.Exists(exeSignPath))
                {
                    if (IsProcessOpen("EMR.SignProcessor"))
                    {
                        Inventec.Common.Logging.LogSystem.Debug("VerifyServiceSignProcessorIsRunning.2");
                        valid = true;
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Debug("VerifyServiceSignProcessorIsRunning.3");
                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => exeSignPath), exeSignPath));
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = exeSignPath;
                        try
                        {
                            Process.Start(startInfo);
                            Inventec.Common.Logging.LogSystem.Debug("VerifyServiceSignProcessorIsRunning.4");
                            Thread.Sleep(500);
                            valid = true;
                            Inventec.Common.Logging.LogSystem.Debug("VerifyServiceSignProcessorIsRunning.5");
                        }
                        catch (Exception exx)
                        {
                            Logging.LogSystem.Warn(exx);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
            return valid;
        }

        private bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName == name || clsProcess.ProcessName == String.Format("{0}.exe", name) || clsProcess.ProcessName == String.Format("{0} (32 bit)", name) || clsProcess.ProcessName == String.Format("{0}.exe (32 bit)", name))
                {
                    return true;
                }
            }

            return false;
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
    }
}
