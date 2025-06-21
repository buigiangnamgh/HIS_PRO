using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.Api;
using Inventec.Common.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Inventec.Common.SignToolViewer.Integrate;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
using System.Configuration;
using EMR.TDO;
using Inventec.Common.SignFile;
using Inventec.Common.SignLibrary.DTO;
using EMR.EFMODEL.DataModels;
using System.Diagnostics;
using Inventec.Common.SignLibrary.CacheClient;
using System.Drawing.Printing;
namespace Inventec.Common.SignLibrary
{
    public class SignLibraryGUIProcessor
    {
        string outputSignedFileResult;
        bool iSPrintNow;
        bool iSPrintPreview;
        short printNumberCopies = 1;
        InputADO inputADOWorking;
        Inventec.Common.SignLibrary.ADO.SignToken signToken = new SignToken();
        System.Drawing.Printing.PageSettings currentPageSettings;
        DevExpress.Pdf.PdfPrinterSettings pdfPrinterSettings = null;
        PrinterSettings printerSettings;
        int Width_;
        int Height_;
        public SignLibraryGUIProcessor()
        {
            try
            {
                this.iSPrintNow = false;
                this.outputSignedFileResult = "";
                this.printNumberCopies = 1;
                this.inputADOWorking = null;
                this.signToken = new SignToken();
                ProcessMemoryUsageuser();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private bool ValidParam(InputADO inputADO)
        {
            bool isShowSignedFile = true;
            string baseFileGigned = "";
            V_EMR_DOCUMENT documentData = new V_EMR_DOCUMENT();
            return ValidParam(inputADO, isShowSignedFile, ref baseFileGigned, ref documentData);
        }

        private bool ValidParamCommon(InputADO inputADO)
        {
            bool valid = false;
            try
            {
                if (inputADO == null)
                {
                    MessageBox.Show(MessageUitl.GetMessage(MessageConstan.TinhNangChiDanhChoBenhAnDienTu));
                    Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.TinhNangChiDanhChoBenhAnDienTu) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO));
                    return valid;
                }
                if (inputADO.Treatment == null || String.IsNullOrEmpty(inputADO.Treatment.TREATMENT_CODE))//DocumentName
                {
                    MessageBox.Show(String.Format(MessageUitl.GetMessage(MessageConstan.TinhNangChiDanhChoBenhAnDienTuCoThamSo), (inputADO.Treatment != null ? inputADO.Treatment.TREATMENT_CODE : ""), inputADO.DocumentName));
                    Inventec.Common.Logging.LogSystem.Warn(String.Format(MessageUitl.GetMessage(MessageConstan.TinhNangChiDanhChoBenhAnDienTuCoThamSo), (inputADO.Treatment != null ? inputADO.Treatment.TREATMENT_CODE : ""), inputADO.DocumentName) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO));
                    return valid;
                }
                InitUri();
                InitParam(inputADO.DTI);
                try
                {
                    if (!String.IsNullOrEmpty(inputADO.HisUriUpdateSignedState))
                    {

                        var arrUriParam = inputADO.HisUriUpdateSignedState.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arrUriParam.Count() > 1)
                        {
                            GlobalStore.INTERGRATE_SYS_BASE_URI = arrUriParam[0];
                            GlobalStore.INTERGRATE_SYS_API = arrUriParam[1];
                        }
                    }
                    else
                    {
                        GlobalStore.INTERGRATE_SYS_BASE_URI = (ConfigurationManager.AppSettings["INTERGRATE_SYS_BASE_URI"] ?? "").ToString();
                        GlobalStore.INTERGRATE_SYS_API = (ConfigurationManager.AppSettings["INTERGRATE_SYS_API"] ?? "").ToString();

                    }
                }
                catch (Exception exx)
                {
                    Inventec.Common.Logging.LogSystem.Warn(exx);
                }
                valid = (CheckLogin(inputADO));
                valid = valid && Verify.VerifyTreatmentCode(inputADO, ref signToken);

                if (valid)
                    InitConfig(inputADO);
                this.inputADOWorking = inputADO;
            }
            catch (Exception ex)
            {
                valid = false;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return valid;
        }

        private void InitConfig(InputADO inputADO)
        {
            try
            {
                var signer = GetSignerData();

                if (inputADO.DisplayConfigDTO == null)
                    inputADO.DisplayConfigDTO = new DisplayConfigDTO();

                if (signer != null)
                {
                    List<string> titles = new List<string>();
                    titles.Add(signer.TITLE);
                    inputADO.DisplayConfigDTO.Titles = titles.ToArray();

                    if (!String.IsNullOrEmpty(inputADO.BusinessCode) && (String.IsNullOrEmpty(inputADO.RoomCode) || String.IsNullOrEmpty(inputADO.RoomTypeCode)))
                    {
                        Inventec.Common.SignLibrary.Api.EmrSignerFlow emrSignerFlow = new Inventec.Common.SignLibrary.Api.EmrSignerFlow();
                        var emrSignerFlowData = emrSignerFlow.GetView(new EMR.Filter.EmrSignerFlowViewFilter() { IS_ACTIVE = 1, BUSINESS_CODE__EXACT = inputADO.BusinessCode, LOGINNAME__EXACT = signer.LOGINNAME });
                        if (emrSignerFlowData != null && emrSignerFlowData.Count > 0)
                        {
                            inputADO.RoomCode = emrSignerFlowData.FirstOrDefault().ROOM_CODE;
                            inputADO.RoomTypeCode = emrSignerFlowData.FirstOrDefault().ROOM_TYPE_CODE;
                        }
                    }
                }

                var configs = GlobalStore.EmrConfigs;
                if (configs != null && configs.Count > 0)
                {

                    var cfgSignDisplayOptions = configs.Where(o => o.KEY == EmrConfigKeys.SIGN_DISPLAY_OPTION);
                    var cfgSignDisplayOption = cfgSignDisplayOptions != null ? cfgSignDisplayOptions.FirstOrDefault() : null;
                    if (cfgSignDisplayOption != null)
                    {
                        if (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.TypeDisplay.HasValue)
                        {
                            Inventec.Common.Logging.LogSystem.Debug("InitConfig___His truyen vao loai hien thi chu ky so==>Emr su dung luon & bo qua cau hinh ben emr(neu co)");
                        }
                        else
                        {
                            string vlOption = !String.IsNullOrEmpty(cfgSignDisplayOption.VALUE) ? cfgSignDisplayOption.VALUE : cfgSignDisplayOption.DEFAULT_VALUE;
                            if (String.IsNullOrEmpty(vlOption))
                            {
                                Inventec.Common.Logging.LogSystem.Debug("InitConfig___Co thiet lap cau hinh hien thi chu ky so nhung gia tri dang null ==> bo qua cau hinh khong xu ly gi");

                                if (signer.SIGN_IMAGE == null || signer.SIGN_IMAGE.Length == 0)
                                {
                                    if (inputADO.DisplayConfigDTO == null)
                                        inputADO.DisplayConfigDTO = new DisplayConfigDTO();
                                    inputADO.DisplayConfigDTO.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT;
                                    Inventec.Common.Logging.LogSystem.Debug("InitConfig___Truong hop khong co gia tri cau hinh EMR.EMR_SIGN.SIGN_DISPLAY_OPTION & nguoi ky khong co anh chu ky ==> tu dong chuyen doi loai hien thi chu ky thanh dang chi hien thi text: DisplayConfigDTO.TypeDisplay =" + Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT);
                                }
                            }
                            else
                            {
                                if (inputADO.DisplayConfigDTO == null)
                                    inputADO.DisplayConfigDTO = new DisplayConfigDTO();
                                if (vlOption == "1")
                                {
                                    inputADO.DisplayConfigDTO.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT;
                                }
                                else if (vlOption == "2")
                                {
                                    inputADO.DisplayConfigDTO.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP;
                                }
                                else if (vlOption == "3")
                                {
                                    inputADO.DisplayConfigDTO.IsDisplaySignature = false;
                                }
                                else
                                {
                                    inputADO.DisplayConfigDTO.TypeDisplay = Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT;
                                }
                            }
                        }
                    }

                    var cfgOptionPrints = configs.Where(o => o.KEY == EmrConfigKeys.PRINT_OPTION);
                    var cfgOptionPrint = cfgOptionPrints != null ? cfgOptionPrints.FirstOrDefault() : null;
                    if (cfgOptionPrint != null)
                    {
                        string vlOptionPrint = !String.IsNullOrEmpty(cfgOptionPrint.VALUE) ? cfgOptionPrint.VALUE : cfgOptionPrint.DEFAULT_VALUE;
                        if (vlOptionPrint == "1")
                        {
                            inputADO.IsPrint = true;
                        }
                    }

                    var cfgSplitPdfKeys = configs.Where(o => o.KEY == EmrConfigKeys.SPLIT_PDF_KEY);
                    var cfgSplitPdfKey = cfgSplitPdfKeys != null ? cfgSplitPdfKeys.FirstOrDefault() : null;
                    if (cfgSplitPdfKey != null)
                    {
                        string vlSplitPdfKey = !String.IsNullOrEmpty(cfgSplitPdfKey.VALUE) ? cfgSplitPdfKey.VALUE : cfgSplitPdfKey.DEFAULT_VALUE;

                        if (!String.IsNullOrEmpty(vlSplitPdfKey))
                        {
                            var arrSplit = vlSplitPdfKey.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            if (arrSplit != null && arrSplit.Count() == 2)
                            {
                                GlobalStore.SplitPdfHeaderKey = arrSplit[0];
                                GlobalStore.SplitPdfContentKey = arrSplit[1];
                            }
                        }
                    }


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


                    var cfgAppearanceOptions = configs.Where(o => o.KEY == EmrConfigKeys.SIGNATURE_APPEARANCE_OPTION);
                    var cfgAppearanceOption = cfgAppearanceOptions != null ? cfgAppearanceOptions.FirstOrDefault() : null;
                    if (cfgAppearanceOption != null)
                    {
                        try
                        {
                            string vlAppearanceOption = !String.IsNullOrEmpty(cfgAppearanceOption.VALUE) ? cfgAppearanceOption.VALUE : cfgAppearanceOption.DEFAULT_VALUE;
                            //vd: p:0|f:11|w:320|h:140
                            if (inputADO.DisplayConfigDTO == null)
                                inputADO.DisplayConfigDTO = new DisplayConfigDTO();
                            var arrOT = vlAppearanceOption.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                            if (arrOT != null && arrOT.Count() > 0)
                            {
                                foreach (string vop in arrOT)
                                {
                                    if (!String.IsNullOrEmpty(vop))
                                    {
                                        var arrOTDetail = vop.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                        if (arrOTDetail != null && arrOTDetail.Count() > 1)
                                        {
                                            if (!String.IsNullOrEmpty(arrOTDetail[1]))
                                            {
                                                string k = arrOTDetail[0].ToLower();
                                                switch (k)
                                                {
                                                    case "p":
                                                        int p = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrOTDetail[1]);
                                                        if (p >= 0 && !inputADO.DisplayConfigDTO.TextPosition.HasValue)
                                                        {
                                                            inputADO.DisplayConfigDTO.TextPosition = p;
                                                        }
                                                        break;
                                                    case "f":
                                                        int f = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrOTDetail[1]);
                                                        if (f > 0 && !inputADO.DisplayConfigDTO.SizeFont.HasValue)
                                                        {
                                                            inputADO.DisplayConfigDTO.SizeFont = f;
                                                        }
                                                        break;
                                                    case "w":
                                                        int w = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrOTDetail[1]);
                                                        if (w > 0 && !inputADO.DisplayConfigDTO.WidthRectangle.HasValue)
                                                        {
                                                            inputADO.DisplayConfigDTO.WidthRectangle = w;
                                                        }
                                                        break;
                                                    case "h":
                                                        int h = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrOTDetail[1]);
                                                        if (h > 0 && !inputADO.DisplayConfigDTO.HeightRectangle.HasValue)
                                                        {
                                                            inputADO.DisplayConfigDTO.HeightRectangle = h;
                                                        }
                                                        break;
                                                    case "a":
                                                        int a = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrOTDetail[1]);
                                                        if (a > 0)
                                                        {
                                                            inputADO.DisplayConfigDTO.Alignment = a;
                                                        }
                                                        break;
                                                    case "fs":
                                                        string fs = (arrOTDetail[1]).ToUpper();
                                                        if (!string.IsNullOrEmpty(fs))
                                                        {
                                                            if (fs.Contains("B") && !inputADO.DisplayConfigDTO.IsBold.HasValue)
                                                                inputADO.DisplayConfigDTO.IsBold = true;
                                                            if (fs.Contains("I") && !inputADO.DisplayConfigDTO.IsItalic.HasValue)
                                                                inputADO.DisplayConfigDTO.IsItalic = true;
                                                            if (fs.Contains("U") && !inputADO.DisplayConfigDTO.IsUnderlined.HasValue)
                                                                inputADO.DisplayConfigDTO.IsUnderlined = true;
                                                        }
                                                        break;
                                                    case "fn":
                                                        string fn = arrOTDetail[1];
                                                        if (!string.IsNullOrEmpty(fn) && string.IsNullOrEmpty(inputADO.DisplayConfigDTO.FontName))
                                                        {
                                                            inputADO.DisplayConfigDTO.FontName = fn;
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex1)
                        {
                            Inventec.Common.Logging.LogSystem.Warn(ex1);
                        }
                    }

                    EMR.EFMODEL.DataModels.EMR_CONFIG cfgLisenceKeyApose = null;
                    var cfgLisenceKeyAposes = configs.Where(o => o.KEY == EmrConfigKeys.LICENSE_KEY__APOSE);
                    cfgLisenceKeyApose = cfgLisenceKeyAposes != null ? cfgLisenceKeyAposes.FirstOrDefault() : null;
                    string vlLisenceKeyApose = cfgLisenceKeyApose != null ? (!String.IsNullOrEmpty(cfgLisenceKeyApose.VALUE) ? cfgLisenceKeyApose.VALUE : cfgLisenceKeyApose.DEFAULT_VALUE) : "";
                    if (!String.IsNullOrEmpty(vlLisenceKeyApose))
                    {
                        Inventec.Common.SignLibrary.License.Licenses.Aspose_Key = vlLisenceKeyApose;
                    }

                    EMR.EFMODEL.DataModels.EMR_CONFIG cfgPrintUsingWaterMark = null;
                    var cfgPrintUsingWaterMarks = configs.Where(o => o.KEY == EmrConfigKeys.PRINT_USING_WARTERMARK__OPTION);
                    cfgPrintUsingWaterMark = cfgPrintUsingWaterMarks != null ? cfgPrintUsingWaterMarks.FirstOrDefault() : null;
                    string vlPrintUsingWaterMark = cfgPrintUsingWaterMark != null ? (!String.IsNullOrEmpty(cfgPrintUsingWaterMark.VALUE) ? cfgPrintUsingWaterMark.VALUE : cfgPrintUsingWaterMark.DEFAULT_VALUE) : "";
                    if (!inputADO.IsShowWatermark.HasValue)
                    {
                        if (!String.IsNullOrEmpty(vlPrintUsingWaterMark))
                        {
                            GlobalStore.PrintUsingWaterMark = vlPrintUsingWaterMark == "1";
                        }
                    }
                    else
                    {
                        GlobalStore.PrintUsingWaterMark = inputADO.IsShowWatermark.Value;
                    }

                    var cfgSignInfoDisplayOptions = configs.Where(o => o.KEY == EmrConfigKeys.SIGN_INFO_DISPLAY_OPTION);
                    var cfgSignInfoDisplayOption = cfgSignInfoDisplayOptions != null ? cfgSignInfoDisplayOptions.FirstOrDefault() : null;
                    if (cfgSignInfoDisplayOption != null)
                    {
                        string vlSignInfoDisplayOption = !String.IsNullOrEmpty(cfgSignInfoDisplayOption.VALUE) ? cfgSignInfoDisplayOption.VALUE : cfgSignInfoDisplayOption.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlSignInfoDisplayOption))
                        {
                            if (vlSignInfoDisplayOption == "1")
                            {
                                inputADO.DisplayConfigDTO.FormatRectangleText = Inventec.Common.SignFile.Constans.SIGN_TEXT_FORMAT_3_1;
                            }
                            else if (vlSignInfoDisplayOption == "2")
                            {
                                inputADO.DisplayConfigDTO.FormatRectangleText = Inventec.Common.SignFile.Constans.SIGN_TEXT_FORMAT_3__NO_DATE;
                            }
                            else if (vlSignInfoDisplayOption == "3")
                            {
                                inputADO.DisplayConfigDTO.FormatRectangleText = Inventec.Common.SignFile.Constans.SIGN_TEXT_FORMAT_3__NO_TITLE;
                            }
                            else
                            {
                                inputADO.DisplayConfigDTO.FormatRectangleText = vlSignInfoDisplayOption;
                            }
                        }
                    }

                    var cfgIntegrateSysBaseUris = configs.Where(o => o.KEY == EmrConfigKeys.INTERGRATE_SYS_BASE_URI);
                    var cfgIntegrateSysBaseUri = cfgIntegrateSysBaseUris != null ? cfgIntegrateSysBaseUris.FirstOrDefault() : null;
                    if (cfgIntegrateSysBaseUri != null)
                    {
                        string vlIntegrateSysBaseUri = !String.IsNullOrEmpty(cfgIntegrateSysBaseUri.VALUE) ? cfgIntegrateSysBaseUri.VALUE : cfgIntegrateSysBaseUri.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlIntegrateSysBaseUri))
                        {
                            GlobalStore.INTERGRATE_SYS_BASE_URI = vlIntegrateSysBaseUri;
                        }
                    }

                    var cfgIntegrateSysApis = configs.Where(o => o.KEY == EmrConfigKeys.INTERGRATE_SYS_API);
                    var cfgIntegrateSysApi = cfgIntegrateSysApis != null ? cfgIntegrateSysApis.FirstOrDefault() : null;
                    if (cfgIntegrateSysApi != null)
                    {
                        string vlIntegrateSysApi = !String.IsNullOrEmpty(cfgIntegrateSysApi.VALUE) ? cfgIntegrateSysApi.VALUE : cfgIntegrateSysApi.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlIntegrateSysApi))
                        {
                            GlobalStore.INTERGRATE_SYS_API = vlIntegrateSysApi;
                        }
                    }


                    var cfgPrintLibraryOptions = configs.Where(o => o.KEY == EmrConfigKeys.PRINT_LIBIARY_OPTION);
                    var cfgPrintLibraryOption = cfgPrintLibraryOptions != null ? cfgPrintLibraryOptions.FirstOrDefault() : null;
                    if (cfgPrintLibraryOption != null)
                    {
                        string vlPrintLibraryOption = !String.IsNullOrEmpty(cfgPrintLibraryOption.VALUE) ? cfgPrintLibraryOption.VALUE : cfgPrintLibraryOption.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlPrintLibraryOption))
                        {
                            if (vlPrintLibraryOption == "1")
                            {
                                GlobalStore.OptionPrintType = OptionPrintType.PdfAposeLib;
                            }
                            else if (vlPrintLibraryOption == "2")
                            {
                                GlobalStore.OptionPrintType = OptionPrintType.CallExeLib;
                            }
                            else if (vlPrintLibraryOption == "0")
                            {
                                GlobalStore.OptionPrintType = OptionPrintType.DevLib;
                            }
                            else
                            {
                                GlobalStore.OptionPrintType = OptionPrintType.DevLib;
                            }
                        }
                    }
                    Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => GlobalStore.OptionPrintType), GlobalStore.OptionPrintType));

                    var cfgPlaceSignOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR__EMR_SIGN__PLACE_SIGN_OPTION);
                    var cfgPlaceSignOption = cfgPlaceSignOptions != null ? cfgPlaceSignOptions.FirstOrDefault() : null;
                    if (cfgPlaceSignOption != null)
                    {
                        string vlPlaceSignOption = !String.IsNullOrEmpty(cfgPlaceSignOption.VALUE) ? cfgPlaceSignOption.VALUE : cfgPlaceSignOption.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlPlaceSignOption))
                        {
                            switch (vlPlaceSignOption)
                            {
                                case "1":
                                    inputADO.DisplayConfigDTO.Location = (signer != null ? (signer.DEPARTMENT_NAME + "|" + signer.TITLE) : "");
                                    break;
                                case "2":
                                    inputADO.DisplayConfigDTO.Location = (signer != null ? (inputADO.DepartmentName + "|" + signer.TITLE) : inputADO.DepartmentName);
                                    break;
                                case "3":
                                    inputADO.DisplayConfigDTO.Location = (!String.IsNullOrEmpty(inputADO.DepartmentName) ? inputADO.DepartmentName : (signer != null ? signer.DEPARTMENT_NAME : "")) + (signer != null ? ("|" + signer.TITLE) : "");
                                    break;
                                default:
                                    break;
                            }
                        }
                    }


                    var cfgPatientSignOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION);
                    var cfgPatientSignOption = cfgPatientSignOptions != null ? cfgPatientSignOptions.FirstOrDefault() : null;
                    if (cfgPatientSignOption != null)
                    {
                        string vlPatientSignOption = !String.IsNullOrEmpty(cfgPatientSignOption.VALUE) ? cfgPatientSignOption.VALUE : cfgPatientSignOption.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlPatientSignOption))
                        {
                            GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION = vlPatientSignOption;
                        }
                    }

                    var cfgPatientSignFisrtOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION);
                    var cfgPatientSignFisrtOption = cfgPatientSignFisrtOptions != null ? cfgPatientSignFisrtOptions.FirstOrDefault() : null;
                    if (cfgPatientSignFisrtOption != null)
                    {
                        string vlPatientSignOption = !String.IsNullOrEmpty(cfgPatientSignFisrtOption.VALUE) ? cfgPatientSignFisrtOption.VALUE : cfgPatientSignFisrtOption.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlPatientSignOption))
                        {
                            GlobalStore.EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION = vlPatientSignOption;
                        }
                    }

                    string vlSignTypeCacheLocal = CacheClientWorker.GetValue(RegistryConstant.SIGN_TYPE_OPTION_KEY);


                    var cfgSignTypes = configs.Where(o => o.KEY == EmrConfigKeys.LIBRARY__EMRGENERATE__SIGNTYPE);
                    var cfgSignType = cfgSignTypes != null ? cfgSignTypes.FirstOrDefault() : null;
                    if (cfgSignType != null)
                    {
                        string vlSignType = !String.IsNullOrEmpty(cfgSignType.VALUE) ? cfgSignType.VALUE : cfgSignType.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlSignType))
                        {
                            switch (vlSignType)
                            {
                                case "1":
                                    inputADO.SignType = SignType.USB;
                                    break;
                                case "2":
                                    inputADO.SignType = SignType.HSM;
                                    break;
                                case "3":
                                    inputADO.SignType = SignType.OptionDefaultUsb;
                                    if (!String.IsNullOrEmpty(vlSignTypeCacheLocal))
                                    {
                                        inputADO.IsOptionSignType = (vlSignTypeCacheLocal == "1");
                                    }
                                    break;
                                case "4":
                                    inputADO.SignType = SignType.OptionDefaultHsm;
                                    if (!String.IsNullOrEmpty(vlSignTypeCacheLocal))
                                    {
                                        inputADO.IsOptionSignType = (vlSignTypeCacheLocal == "1");
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }


                    var cfgSignUsingUsbTokenDeviceOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR__EMR_SIGN__USING_USBTOKENDEVICE__OPTION);
                    var cfgSignUsingUsbTokenDeviceOption = cfgSignUsingUsbTokenDeviceOptions != null ? cfgSignUsingUsbTokenDeviceOptions.FirstOrDefault() : null;
                    if (cfgSignUsingUsbTokenDeviceOption != null)
                    {
                        string vlSignUsingUsbTokenDeviceOption = !String.IsNullOrEmpty(cfgSignUsingUsbTokenDeviceOption.VALUE) ? cfgSignUsingUsbTokenDeviceOption.VALUE : cfgSignUsingUsbTokenDeviceOption.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlSignUsingUsbTokenDeviceOption))
                        {
                            GlobalStore.IsSignUsingUsbTokenDevice = (vlSignUsingUsbTokenDeviceOption != "1");
                        }
                    }

                    var cfgSignPatientOptionOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR__EMR_SIGN__PATIENT_SIGN__OPTION);
                    var cfgSignPatientOptionOption = cfgSignPatientOptionOptions != null ? cfgSignPatientOptionOptions.FirstOrDefault() : null;
                    if (cfgSignPatientOptionOption != null)
                    {
                        string vlSignPatientOptionOption = !String.IsNullOrEmpty(cfgSignPatientOptionOption.VALUE) ? cfgSignPatientOptionOption.VALUE : cfgSignUsingUsbTokenDeviceOption.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlSignPatientOptionOption))
                        {
                            GlobalStore.EMR_SIGN_PATIENT_SIGN_OPTION = vlSignPatientOptionOption;
                        }
                    }


                    var cfgSignBoardOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR__EMR_SIGN__SIGN_BOARD__OPTION);
                    var cfgSignBoardOption = cfgSignBoardOptions != null ? cfgSignBoardOptions.FirstOrDefault() : null;
                    if (cfgSignBoardOption != null)
                    {
                        string vlSignBoardOption = !String.IsNullOrEmpty(cfgSignBoardOption.VALUE) ? cfgSignBoardOption.VALUE : cfgSignBoardOption.DEFAULT_VALUE;
                        if (!String.IsNullOrEmpty(vlSignBoardOption))
                        {
                            GlobalStore.EMR_SIGN_BOARD__OPTION = vlSignBoardOption;
                        }
                    }

                    var cfgSignDescriptionInfoOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR__EMR_SIGN__SIGN_DESCRIPTION_INFO__OPTION);
                    var cfgSignDescriptionInfoOption = cfgSignDescriptionInfoOptions != null ? cfgSignDescriptionInfoOptions.FirstOrDefault() : null;
                    if (cfgSignDescriptionInfoOption != null)
                    {
                        string vlSignDescriptionInfoOption = !String.IsNullOrEmpty(cfgSignDescriptionInfoOption.VALUE) ? cfgSignDescriptionInfoOption.VALUE : cfgSignDescriptionInfoOption.DEFAULT_VALUE;
                        GlobalStore.EMR_SIGN_SIGN_DESCRIPTION_INFO = vlSignDescriptionInfoOption;
                    }

                    var cfgEmrHsmIntegrateOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR_HSM_INTEGRATE_OPTION);
                    var cfgEmrHsmIntegrateOption = cfgEmrHsmIntegrateOptions != null ? cfgEmrHsmIntegrateOptions.FirstOrDefault() : null;
                    if (cfgEmrHsmIntegrateOption != null)
                    {
                        string vlSignDescriptionInfoOption = !String.IsNullOrEmpty(cfgEmrHsmIntegrateOption.VALUE) ? cfgEmrHsmIntegrateOption.VALUE : cfgEmrHsmIntegrateOption.DEFAULT_VALUE;
                        GlobalStore.EMR_HSM_INTEGRATE_OPTION = vlSignDescriptionInfoOption;
                    }

                    Inventec.Common.Logging.LogSystem.Info("EMR_HSM_INTEGRATE_OPTION: " + GlobalStore.EMR_HSM_INTEGRATE_OPTION);

                    var cfgEmrHsmAutoUpdateSignImages = configs.Where(o => o.KEY == EmrConfigKeys.EMR_EMR_SIGNER_AUTO_UPDATE_SIGN_IMAGE);
                    var cfgEmrHsmAutoUpdateSignImage = cfgEmrHsmAutoUpdateSignImages != null ? cfgEmrHsmAutoUpdateSignImages.FirstOrDefault() : null;
                    if (cfgEmrHsmAutoUpdateSignImage != null)
                    {
                        string vlSignDescriptionInfoOption = !String.IsNullOrEmpty(cfgEmrHsmAutoUpdateSignImage.VALUE) ? cfgEmrHsmAutoUpdateSignImage.VALUE : cfgEmrHsmAutoUpdateSignImage.DEFAULT_VALUE;
                        GlobalStore.EMR_EMR_SIGNER_AUTO_UPDATE_SIGN_IMAGE = vlSignDescriptionInfoOption;
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private bool ValidParam(InputADO inputADO, bool isShowSignedFile, ref string base64FileGigned, ref V_EMR_DOCUMENT documentData)
        {
            bool valid = true;
            try
            {
                valid = ValidParam(inputADO, isShowSignedFile, false, ref base64FileGigned, ref documentData);

            }
            catch (Exception ex)
            {
                valid = false;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return valid;
        }

        private bool ValidParam(InputADO inputADO, bool isShowSignedFile, bool isValidExistsDoc, ref string base64FileGigned, ref V_EMR_DOCUMENT documentData)
        {
            bool valid = true;
            try
            {
                valid = valid && ValidParamCommon(inputADO);
                if (inputADO.IsSign && String.IsNullOrEmpty(inputADO.DocumentCode))
                {
                    valid = valid && Verify.VerifyHisCode(inputADO, isShowSignedFile, isValidExistsDoc, ref base64FileGigned, ref documentData);
                }
            }
            catch (Exception ex)
            {
                valid = false;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return valid;
        }

        private bool ValidParam(InputADO inputADO, bool isShowSignedFile, ref byte[] inputByte)
        {
            bool valid = true;
            try
            {
                valid = valid && ValidParamCommon(inputADO);
                valid = valid && Verify.VerifyHisCode(inputADO, isShowSignedFile, ref inputByte);
            }
            catch (Exception ex)
            {
                valid = false;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return valid;
        }

        private void InitUri()
        {
            if (!String.IsNullOrEmpty((string)RegistryProcessor.Read("ACS_BASE_URI")))
            {
                ConstanIG.ACS_BASE_URI = (string)RegistryProcessor.Read("ACS_BASE_URI");
            }

            if (!String.IsNullOrEmpty((string)RegistryProcessor.Read("EMR_BASE_URI")))
            {
                GlobalStore.EMR_BASE_URI = (string)RegistryProcessor.Read("EMR_BASE_URI");
            }

            if (!String.IsNullOrEmpty((string)RegistryProcessor.Read("FSS_BASE_URI")))
            {
                FssConstant.BASE_URI = (string)RegistryProcessor.Read("FSS_BASE_URI");
            }
        }

        private string GetTokenCodeData()
        {
            if (signToken != null && !String.IsNullOrEmpty(signToken.TokenCode))
            {
                return signToken.TokenCode;
            }
            else return GlobalStore.TokenCode;
        }

        private EMR_SIGNER GetSignerData()
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

        private EMR_TREATMENT GetTreatmentData()
        {
            if (signToken != null && signToken.Treatment != null && signToken.Treatment.ID > 0)
            {
                return signToken.Treatment;
            }
            return null;
        }

        public void ShowPopup(string inputFile, InputADO inputADO)
        {
            try
            {
                string base64FileSigned = "";
                V_EMR_DOCUMENT documentData = new V_EMR_DOCUMENT();
                if (!ValidParam(inputADO, true, ref base64FileSigned, ref documentData))
                {
                    return;
                }

                if (!String.IsNullOrEmpty(base64FileSigned))
                {
                    var barr = Convert.FromBase64String(base64FileSigned);
                    InputADO inputTmpADO = CopyInputADO(inputADO);
                    inputTmpADO.IsExport = inputTmpADO.IsReject = inputTmpADO.IsSave = inputTmpADO.IsSign = false;
                    inputTmpADO.DocumentCode = documentData != null ? documentData.DOCUMENT_CODE : "";
                    frmPdfViewer frmPdfViewer = new frmPdfViewer(barr, FileType.Pdf, inputTmpADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                    frmPdfViewer.ShowDialog();
                }
                else
                {
                    if (!File.Exists(inputFile))
                    {
                        MessageBox.Show(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                        Inventec.Common.Logging.LogSystem.Warn("File không tồn tại. inputFile = " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFile), inputFile));
                        return;
                    }

                    frmPdfViewer frmPdfViewer = new frmPdfViewer(inputFile, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                    frmPdfViewer.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public Form GetForm(string inputFile, InputADO inputADO)
        {
            try
            {
                if (!ValidParam(inputADO))
                {
                    return null;
                }

                if (!File.Exists(inputFile))
                {
                    Inventec.Common.Logging.LogSystem.Warn("File không tồn tại. inputFile = " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFile), inputFile));
                    MessageBox.Show("File không tồn tại. inputFile = " + inputFile);
                    return null;
                }

                return new frmPdfViewer(inputFile, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return null;
        }

        public UserControl GetUC(string inputFile, InputADO inputADO)
        {
            try
            {
                if (!ValidParam(inputADO))
                {
                    return null;
                }

                if (!File.Exists(inputFile))
                {
                    Inventec.Common.Logging.LogSystem.Warn("File không tồn tại. inputFile = " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFile), inputFile));
                    MessageBox.Show("File không tồn tại. Đường dẫn file truyền vào: " + inputFile);
                    return null;
                }

                return new UCViewer(inputFile, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return null;
        }

        public void ShowPopup(Stream inputStream, InputADO inputADO)
        {
            try
            {
                if (!ValidParam(inputADO))
                {
                    return;
                }

                if (inputStream == null || inputStream.Length == 0)
                {
                    Inventec.Common.Logging.LogSystem.Warn("inputStream không hợp lệ____inputStream.length=" + (inputStream != null ? inputStream.Length : 0));
                    MessageBox.Show("FileStream không hợp lệ.");
                    return;
                }

                inputStream.Position = 0;
                var barr = Utils.StreamToByte(inputStream);

                frmPdfViewer frmPdfViewer = new frmPdfViewer(barr, FileType.Xlsx, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                frmPdfViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public void ShowPopup(string base64FileContent, FileType fileType, InputADO inputADO)
        {
            try
            {
                if (String.IsNullOrEmpty(base64FileContent))
                {
                    MessageBox.Show("Dữ liệu file không hợp lệ.");
                    return;
                }
                V_EMR_DOCUMENT documentData = null;
                string base64FileSigned = "";
                if (!ValidParam(inputADO, true, ref base64FileSigned, ref documentData))
                {
                    return;
                }
                if (!String.IsNullOrEmpty(base64FileSigned))
                {
                    var barr = Convert.FromBase64String(base64FileSigned);
                    InputADO inputTmpADO = CopyInputADO(inputADO);
                    inputTmpADO.IsExport = inputTmpADO.IsReject = inputTmpADO.IsSave = inputTmpADO.IsSign = false;

                    inputTmpADO.DocumentCode = documentData != null ? documentData.DOCUMENT_CODE : "";
                    frmPdfViewer frmPdfViewer = new frmPdfViewer(barr, FileType.Pdf, inputTmpADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                    frmPdfViewer.ShowDialog();
                }
                else
                {
                    var barr = Convert.FromBase64String(base64FileContent);
                    frmPdfViewer frmPdfViewer = new frmPdfViewer(barr, fileType, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                    frmPdfViewer.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public void ShowPopup(List<FileADO> fileADOs, InputADO inputADO)
        {
            try
            {
                FileADO fileADOMain = null;
                FileADO fileADOJson = null;
                FileADO fileADOXml = null;
                foreach (var fileSign in fileADOs)
                {
                    if (String.IsNullOrEmpty(fileSign.Base64FileContent))
                    {
                        Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe) + ". Du lieu file truyen vao khong hop le:____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO));
                        MessageBox.Show("Dữ liệu file không hợp lệ.");
                        return;
                    }
                    if (fileSign.IsMain.HasValue && fileSign.IsMain.Value)
                    {
                        fileADOMain = fileSign;
                    }
                    else if (fileSign.FileType == FileType.Json)
                    {
                        fileADOJson = fileSign;
                    }
                    else if (fileSign.FileType == FileType.Xml)
                    {
                        fileADOXml = fileSign;
                    }
                }

                if (fileADOMain == null && fileADOs.Count > 1)
                {
                    fileADOMain = fileADOs.FirstOrDefault();
                }
                else if (fileADOMain == null && fileADOs.Count == 1)
                {
                    fileADOMain = fileADOs.FirstOrDefault();
                    fileADOJson = null;
                    fileADOXml = null;
                }

                V_EMR_DOCUMENT documentData = null;
                string base64FileSigned = "";
                if (!ValidParam(inputADO, true, ref base64FileSigned, ref documentData))
                {
                    return;
                }
                if (!String.IsNullOrEmpty(base64FileSigned))
                {
                    var barr = Convert.FromBase64String(base64FileSigned);
                    InputADO inputTmpADO = CopyInputADO(inputADO);
                    inputTmpADO.IsExport = inputTmpADO.IsReject = inputTmpADO.IsSave = inputTmpADO.IsSign = false;

                    inputTmpADO.DocumentCode = documentData != null ? documentData.DOCUMENT_CODE : "";
                    frmPdfViewer frmPdfViewer = new frmPdfViewer(barr, FileType.Pdf, inputTmpADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                    frmPdfViewer.ShowDialog();
                }
                else
                {
                    var barr = Convert.FromBase64String(fileADOMain.Base64FileContent);
                    frmPdfViewer frmPdfViewer = new frmPdfViewer(barr, fileADOMain.FileType, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                    frmPdfViewer.UpdateExtFileType(fileADOMain, fileADOJson, fileADOXml);
                    frmPdfViewer.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public Form GetForm(Stream inputStream, InputADO inputADO)
        {
            try
            {
                if (!ValidParam(inputADO))
                {
                    return null;
                }

                if (inputStream == null || inputStream.Length == 0)
                {
                    Inventec.Common.Logging.LogSystem.Warn("inputStream không hợp lệ____inputStream.length=" + (inputStream != null ? inputStream.Length : 0) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO));
                    MessageBox.Show("FileStream không hợp lệ.");
                    return null;
                }

                inputStream.Position = 0;
                var barr = Utils.StreamToByte(inputStream);

                return new frmPdfViewer(barr, FileType.Xlsx, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return null;
        }

        public UserControl GetUC(string base64FileContent, FileType fileType, InputADO inputADO)
        {
            return GetUC(base64FileContent, fileType, inputADO, "");
        }

        public UserControl GetUC(string base64FileContent, FileType fileType, InputADO inputADO, string pin)
        {
            try
            {
                GlobalStore.PIN = pin;
                if (String.IsNullOrEmpty(base64FileContent))
                {
                    MessageBox.Show("Dữ liệu base64 file không hợp lệ.");
                    Inventec.Common.Logging.LogSystem.Info("Du lieu dau vao khong hop le____" + Inventec.Common.Logging.LogUtil.TraceData("base64FileContent", base64FileContent) + "____" + Inventec.Common.Logging.LogUtil.TraceData("fileType", fileType) + "____" + Inventec.Common.Logging.LogUtil.TraceData("inputADO", inputADO));
                    return null;
                }
                V_EMR_DOCUMENT documentData = new V_EMR_DOCUMENT();
                string base64FileSigned = "";
                if (!ValidParam(inputADO, true, ref base64FileSigned, ref documentData))
                {
                    return null;
                }
                if (!String.IsNullOrEmpty(base64FileSigned))
                {
                    var barr = Convert.FromBase64String(base64FileSigned);
                    InputADO inputTmpADO = CopyInputADO(inputADO);
                    inputTmpADO.IsExport = inputTmpADO.IsReject = inputTmpADO.IsSave = inputTmpADO.IsSign = false;
                    inputTmpADO.DocumentCode = documentData != null ? documentData.DOCUMENT_CODE : "";
                    return new UCViewer(barr, FileType.Pdf, inputTmpADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                }
                else
                {
                    var barr = Convert.FromBase64String(base64FileContent);
                    return new UCViewer(barr, fileType, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
                Inventec.Common.Logging.LogSystem.Info("Co loi xay ra. Du lieu dau vao:____" + Inventec.Common.Logging.LogUtil.TraceData("base64FileContent", base64FileContent) + "____" + Inventec.Common.Logging.LogUtil.TraceData("fileType", fileType) + "____" + Inventec.Common.Logging.LogUtil.TraceData("inputADO", inputADO));
            }

            return null;
        }

        public UserControl GetUC(Stream inputStream, InputADO inputADO)
        {
            try
            {
                if (!ValidParam(inputADO))
                {
                    return null;
                }

                if (inputStream == null || inputStream.Length == 0)
                {
                    Inventec.Common.Logging.LogSystem.Warn("inputStream không hợp lệ____inputStream.length=" + (inputStream != null ? inputStream.Length : 0));
                    MessageBox.Show("FileStream không hợp lệ.");
                    return null;
                }

                var barr = Utils.StreamToByte(inputStream);

                inputStream.Position = 0;

                return new UCViewer(barr, FileType.Xlsx, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return null;
        }

        public void ShowPopup(byte[] inputByte, InputADO inputADO)
        {
            try
            {
                if (!ValidParam(inputADO))
                {
                    return;
                }

                if (inputByte == null || inputByte.Length == 0)
                {
                    Inventec.Common.Logging.LogSystem.Warn("FileByte không hợp lệ____");
                    MessageBox.Show("FileByte không hợp lệ.");
                    return;
                }

                frmPdfViewer frmPdfViewer = new frmPdfViewer(inputByte, FileType.Xlsx, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                frmPdfViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public Form GetForm(byte[] inputByte, InputADO inputADO)
        {
            try
            {
                if (!ValidParam(inputADO))
                {
                    return null;
                }

                if (inputByte == null || inputByte.Length == 0)
                {
                    Inventec.Common.Logging.LogSystem.Warn("FileByte không hợp lệ____");
                    MessageBox.Show("FileByte không hợp lệ.");
                    return null;
                }

                return new frmPdfViewer(inputByte, FileType.Xlsx, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return null;
        }

        public UserControl GetUC(byte[] inputByte, InputADO inputADO)
        {
            try
            {
                if (!ValidParam(inputADO))
                {
                    return null;
                }

                if (inputByte == null || inputByte.Length == 0)
                {
                    Inventec.Common.Logging.LogSystem.Warn("FileByte không hợp lệ____");
                    MessageBox.Show("FileByte không hợp lệ.");
                    return null;
                }

                return new UCViewer(inputByte, FileType.Xlsx, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return null;
        }

        public UserControl GetUC(byte[] inputByte, FileType fileType, InputADO inputADO, string pin)
        {
            try
            {
                GlobalStore.PIN = pin;

                if (inputByte == null || inputByte.Length == 0)
                {
                    Inventec.Common.Logging.LogSystem.Warn("FileByte không hợp lệ____");
                    MessageBox.Show("FileByte không hợp lệ.");
                    return null;
                }
                byte[] inputByteTmp = null;
                if (!ValidParam(inputADO, true, ref inputByteTmp))
                {
                    return null;
                }

                if (inputByteTmp != null && inputByteTmp.Length > 0)
                {
                    InputADO inputTmpADO = CopyInputADO(inputADO);
                    inputTmpADO.IsExport = inputTmpADO.IsPrint = inputTmpADO.IsReject = inputTmpADO.IsSave = inputTmpADO.IsSign = false;
                    //inputADO.IsExport = inputADO.IsPrint = inputADO.IsReject = inputADO.IsSave = inputADO.IsSign = false;
                    return new UCViewer(inputByteTmp, FileType.Pdf, inputTmpADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                }
                else
                {
                    return new UCViewer(inputByte, fileType, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Info("Co loi xay ra. Du lieu dau vao:____" + Inventec.Common.Logging.LogUtil.TraceData("base64FileContent", Convert.ToBase64String(inputByte)) + "____" + Inventec.Common.Logging.LogUtil.TraceData("fileType", fileType) + "____" + Inventec.Common.Logging.LogUtil.TraceData("inputADO", inputADO));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return null;
        }

        public UserControl GetUC(byte[] inputByte, FileType fileType, InputADO inputADO)
        {
            return GetUC(inputByte, fileType, inputADO, "");
        }

        public bool RefeshAfterLogout()
        {
            bool success = false;
            try
            {
                GlobalStore.TokenCode = null;
                GlobalStore.TokenData = null;
                //GlobalStore.EmrConsumer = null;
                //GlobalStore.AcsConsumer = null;
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

        public void SetPIN(string pin)
        {
            GlobalStore.PIN = pin;
        }

        public DocumentSignedResultDTO SignNow(string base64FileContent, FileType fileType, InputADO inputADO)
        {
            return ProcessSignPrintNow(base64FileContent, fileType, inputADO, false);
        }

        public DocumentSignedResultDTO SignNow(string base64FileContent, FileType fileType, InputADO inputADO, bool isSignOnlyWithHasAutoPosition = false)
        {
            return ProcessSignPrintNow(base64FileContent, fileType, inputADO, false, false, isSignOnlyWithHasAutoPosition);
        }

        public DocumentSignedResultDTO SignAndPrintNow(string base64FileContent, FileType fileType, InputADO inputADO)
        {
            return ProcessSignPrintNow(base64FileContent, fileType, inputADO, true);
        }

        public DocumentSignedResultDTO SignAndShowPrintPreview(string base64FileContent, FileType fileType, InputADO inputADO)
        {
            return ProcessSignPrintNow(base64FileContent, fileType, inputADO, false, true);
        }

        private DocumentSignedResultDTO ProcessSignPrintNow(string base64FileContent, FileType fileType, InputADO inputADO, bool isPrintNow, bool isPrintPreview = false, bool isSignOnlyWithHasAutoPosition = false)
        {
            Inventec.Common.Logging.LogSystem.Info("ProcessSignPrint" + (isPrintPreview ? "Preview" : "Now") + ". 1");
            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isPrintNow), isPrintNow)
                + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isPrintPreview), isPrintPreview)
                 + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSignOnlyWithHasAutoPosition), isSignOnlyWithHasAutoPosition)
                 + Inventec.Common.Logging.LogUtil.TraceData("fileType", fileType));
            this.outputSignedFileResult = "";
            this.iSPrintNow = isPrintNow;
            this.iSPrintPreview = isPrintPreview;
            int typeDisplayOption = 0;
            DocumentSignedResultDTO rsData = new DocumentSignedResultDTO();
            try
            {
                if (String.IsNullOrEmpty(base64FileContent))
                {
                    rsData.Success = false;
                    rsData.Message = MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe);
                    Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsData), rsData));
                    MessageBox.Show(rsData.Message);
                    return rsData;
                }

                V_EMR_DOCUMENT documentData = new V_EMR_DOCUMENT();
                if (!ValidParam(inputADO, true, true, ref base64FileContent, ref documentData))
                {
                    rsData.Success = false;
                    rsData.Message = MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe);
                    Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsData), rsData));
                    return rsData;
                }
                   
                string inputFileWork = "";
                string ext = Utils.GetExtByFileType(fileType);
                var inputByte = Convert.FromBase64String(base64FileContent);
                string inputFileTemp = Utils.GenerateTempFileWithin(ext);
                Utils.ByteToFile(inputByte, inputFileTemp);
                Utils.ProcessFileInput(inputFileTemp, ext, ref inputFileWork);

                //Xử lý 2 trường hợp:
                //---Có khai báo tọa độ ký trong file template ==> ký & trả kết quả luôn 
                //---Không có khai báo tọa dộ ký trong file template ==> show lên form file ký & chọn điểm ký ==> ký ==> tự động tắt form trả kết quả 
                 
                bool success = false;
                bool isMultiSign = false;
                bool isMultiSignForAuto = false;
                bool isSignParallel = false;
                EMR.TDO.DocumentTDO currentDocument = null;
                int signedCount = 0;
                EMR.EFMODEL.DataModels.EMR_SIGN signSelected = null;
                List<SignPositionADO> nextSignPositions = null;
                iTextSharp.text.pdf.PdfReader readerWorking = null;
                if (!String.IsNullOrEmpty(inputADO.DocumentCode))
                {
                    signSelected = new EmrSign().GetSignDocumentFirst(inputADO.DocumentCode, GetSignerData(), GetTreatmentData(), isMultiSign, true);
                }
                this.printNumberCopies = inputADO.PrintNumberCopies.HasValue ? inputADO.PrintNumberCopies.Value : (short)1;
                bool isSignPatient = inputADO.IsPatientSign;
                bool isSignHomeRelation = inputADO.IsHomeRelativeSign.HasValue ? inputADO.IsHomeRelativeSign.Value : false;

                if (fileType != FileType.Xml && fileType != FileType.Json)
                {
                    readerWorking = new iTextSharp.text.pdf.PdfReader(inputFileWork);
                    ProcessCommentKey(inputADO, ref inputFileWork, ref nextSignPositions);

                    if (nextSignPositions != null && nextSignPositions.Count > 0)
                    {
                        readerWorking.Close();
                        readerWorking = new iTextSharp.text.pdf.PdfReader(inputFileWork);
                    }
                    nextSignPositions = GetNextPositionSigned(inputADO.IsPatientSign || (inputADO.IsHomeRelativeSign ?? false), readerWorking, signSelected, ref signedCount);
                    Inventec.Common.Logging.LogSystem.Debug("ProcessSignPrintNow____nextSignPositions.count=" + (nextSignPositions != null ? nextSignPositions.Count : 0));
                }

                List<SignTDO> listSign = null;
                string outputFile = "";

                if ((nextSignPositions != null && nextSignPositions.Count > 0) || (fileType == FileType.Xml || fileType == FileType.Json))
                {
                    if (inputADO.SignerConfigs != null && inputADO.SignerConfigs.Count > 0)
                    {
                        inputADO.SignerConfigs = inputADO.SignerConfigs.OrderBy(o => o.NumOrder).ToList();
                        listSign = new List<SignTDO>();

                        foreach (var scf in inputADO.SignerConfigs)
                        {
                            SignTDO sign = new SignTDO();
                            EMR_SIGNER signerFind = GlobalStore.GetByLoginName(scf.Loginname);
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
                                    sign.NumOrder = GetMaxNumOrder(listSign);
                                sign.Title = signerFind.TITLE;
                                sign.DepartmentCode = signerFind.DEPARTMENT_CODE;
                                sign.DepartmentName = signerFind.DEPARTMENT_NAME;
                                sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                                listSign.Add(sign);
                            }
                        }
                    }
                }

                string signName = (String.IsNullOrEmpty(GlobalStore.UserName) ? GlobalStore.LoginName : GlobalStore.LoginName + " (" + GlobalStore.UserName + ")");
                long? documentTypeId = GetDocumentTypeId(inputADO.DocumentTypeCode);

                if (nextSignPositions != null && nextSignPositions.Count > 0)
                {
                    isMultiSign = GetMultiSignDoc(inputADO, ref isSignParallel);
                    if (!String.IsNullOrEmpty(inputADO.DocumentCode))
                    {
                        currentDocument = GenerateByDocumentCode(inputADO.DocumentCode, ref isMultiSign);
                        isSignParallel = currentDocument != null && currentDocument.IsSignParallel.HasValue ? currentDocument.IsSignParallel.Value : false;//TODO
                    }
                    SignPositionADO nextSignPosition = nextSignPositions[0];
                    if (nextSignPositions.Count > 1)
                    {
                        nextSignPosition.SignPositionAutos = nextSignPositions;
                    }

                    bool? vOptionSign = VerifySign.VerifySignImageWithOption(inputADO, GetSignerData(), (nextSignPositions != null && nextSignPositions.Count > 0), nextSignPosition, isMultiSign);
                    if (vOptionSign.HasValue)
                    {
                        if (vOptionSign.Value)
                        {
                            typeDisplayOption = Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT;
                        }
                        else
                        {
                            rsData.Success = false;
                            rsData.Message = MessageUitl.GetMessage(MessageConstan.TaiKhoanThieuThongTinAnh);
                            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsData), rsData));
                            return rsData;
                        }
                    }

                    isMultiSignForAuto = false;
                    if (!isMultiSign && nextSignPositions != null && nextSignPositions.Count >= 2)
                    {
                        isMultiSignForAuto = true;
                    }

                    int demKey = 1;
                    foreach (var nSp in nextSignPositions)
                    {
                        if (currentDocument != null && !String.IsNullOrEmpty(currentDocument.DocumentCode) && nextSignPositions != null && nextSignPositions.Count >= 2)
                        {
                            signSelected = new EmrSign().GetSignDocumentFirst(currentDocument.DocumentCode, GetSignerData(), GetTreatmentData(), (isMultiSignForAuto || isMultiSign), false);
                        }


                        //Tìm thấy tọa độ cần ký tiếp theo trong template                       
                        Inventec.Common.Logging.LogSystem.Debug("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.WidthRectangle), nSp.WidthRectangle) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.HeightRectangle), nSp.HeightRectangle) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.PageNUm), nSp.PageNUm) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.Text), nSp.Text));
                        float _x = (nSp.Reactanle.Left);
                        _x = _x < 0 ? 0 : _x;
                        float _y = (nSp.Reactanle.Bottom);
                        _y = _y < 0 ? 0 : _y;

                        int totalPageNumber = readerWorking.NumberOfPages;
                        VerifyPdfFileHandle verifyPdfFile = new VerifyPdfFileHandle();
                        List<VerifierADO> verifiers = verifyPdfFile.verify(readerWorking).OrderBy(o => o.Date).ToList();

                        SignHandle signProcessor = null;
                        CommonParam param = new CommonParam();

                        DisplayConfigDTO displayConfigParam = new DisplayConfigDTO()
                        {
                            HeightRectangle = (nSp.HeightRectangle > 0
                                ? nSp.HeightRectangle :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.HeightRectangle
                                    : null),
                            WidthRectangle = (nSp.WidthRectangle > 0
                                ? nSp.WidthRectangle :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.WidthRectangle
                                    : null),
                            SizeFont = (nSp.SizeFont > 0
                                ? nSp.SizeFont :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.SizeFont
                                    : null),
                            TextPosition = (nSp.TextPosition > 0
                                ? (int?)nSp.TextPosition :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.TextPosition
                                    : null),
                            TypeDisplay = (typeDisplayOption > 0 ? typeDisplayOption : (nSp.TypeDisplay > 0
                                ? nSp.TypeDisplay :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.TypeDisplay
                                    : null)),
                            IsDisplaySignature = (nSp.IsDisplaySignature.HasValue
                                ? nSp.IsDisplaySignature :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.IsDisplaySignature
                                    : null),
                            FormatRectangleText =
                                (inputADO.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADO.DisplayConfigDTO.FormatRectangleText)
                                ? inputADO.DisplayConfigDTO.FormatRectangleText : String.Empty),
                            Location =
                            (inputADO.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADO.DisplayConfigDTO.Location)
                            ? inputADO.DisplayConfigDTO.Location : String.Empty),
                            Alignment = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.Alignment.HasValue)
                    ? inputADO.DisplayConfigDTO.Alignment
                    : null
                ),
                            IsBold = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.IsBold.HasValue)
                    ? inputADO.DisplayConfigDTO.IsBold
                    : null
                ),
                            IsItalic = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.IsItalic.HasValue)
                    ? inputADO.DisplayConfigDTO.IsItalic
                    : null
                ),
                            IsUnderlined = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.IsUnderlined.HasValue)
                    ? inputADO.DisplayConfigDTO.IsUnderlined
                    : null
                ),
                            FontName = (
                    (inputADO.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADO.DisplayConfigDTO.FontName))
                    ? inputADO.DisplayConfigDTO.FontName
                    : null
                )
                        };

                        SignType optionSignType = GetSignTypeConfig(inputADO);

                        bool isMultiSignForProcess = isMultiSign;
                        if (isMultiSignForAuto && demKey == nextSignPositions.Count)
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


                        signProcessor = new SignHandle(inputFileWork, _x, _y, nSp.PageNUm, totalPageNumber, null, inputADO.DlgOpenModuleConfig, inputADO.DocumentName, inputADO.Treatment.TREATMENT_CODE, signName, inputADO.SignReason, verifiers, optionSignType, listSign, signSelected, isSignPatient, isSignHomeRelation, documentTypeId, isMultiSignForProcess, inputADO.HisCode, inputADO, displayConfigParam, param, null, isSignParallel, GetTreatmentData(), GetSignerData(), GetTokenCodeData());

                        bool valid = true;
                        string messageErr = "";
                        valid = signProcessor.VerifyFile(currentDocument, signedCount, ref messageErr);
                        if (!valid)
                        {
                            rsData.Success = false;
                            rsData.Message = messageErr;
                        }
                        else
                        {
                            if (currentDocument == null)
                                currentDocument = new DocumentTDO();
                            if (inputADO.IsUsingSignPad.HasValue && (isSignPatient || isSignHomeRelation))
                            {
                                signProcessor.SetSignPadBefore(Utils.SignPadImageData);
                                signProcessor.SetUsingSignPad(inputADO.IsUsingSignPad.Value);
                            }
                            else
                            {
                                signProcessor.SetSignPadBefore(null);
                            }
                            signProcessor.SetFileType(fileType);
                            bool rsSuccess = signProcessor.SignFile(currentDocument, ref outputFile);
                            Inventec.Common.Logging.LogSystem.Info("SignDigital__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsSuccess), rsSuccess)
                                + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile)
                               + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFileWork), inputFileWork)
                                );
                            if (inputADO.IsRemoveSignPadBefore)
                                Utils.SignPadImageData = null;
                            if (rsSuccess)
                            {
                                success = rsSuccess;
                                try
                                {
                                    if (readerWorking != null) readerWorking.Close();
                                    if (File.Exists(inputFileWork)) File.Delete(inputFileWork);
                                }
                                catch { }

                                rsData.Message = "OK";
                                rsData.Base64FileSigned = Utils.FileToBase64String(outputFile);
                                rsData.DocumentCode = currentDocument.DocumentCode;

                                inputFileWork = outputFile;
                                readerWorking = new iTextSharp.text.pdf.PdfReader(inputFileWork);
                            }
                            rsData.Success = success;
                        }

                        demKey++;
                    }

                    if (nextSignPositions != null && nextSignPositions.Count > 0 && success && File.Exists(outputFile))
                    {
                        Inventec.Common.Logging.LogSystem.Info("SignDigital__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile));
                        try
                        {
                            if (readerWorking != null) readerWorking.Close();
                            //if (File.Exists(inputFileWork)) File.Delete(inputFileWork);
                        }
                        catch { }

                        if (!isSignOnlyWithHasAutoPosition)
                        {
                            rsData.Message = "OK";
                            rsData.Base64FileSigned = Utils.FileToBase64String(outputFile);

                            if (isPrintNow)
                            {
                                PrintNowProcess(outputFile);
                            }
                            else if (isPrintPreview)
                            {
                                PrintPreviewProcess(outputFile);
                            }
                        }

                        rsData.Success = success;
                    }
                }
                else if (fileType == FileType.Xml || fileType == FileType.Json)
                {
                    SignHandle signProcessor = null;
                    CommonParam param = new CommonParam();
                    DisplayConfigDTO displayConfigParam = inputADO.DisplayConfigDTO;

                    SignType optionSignType = GetSignTypeConfig(inputADO);

                    bool isMultiSignForProcess = isMultiSign;

                    signProcessor = new SignHandle(inputFileWork, 1, 1, 1, 1, null, inputADO.DlgOpenModuleConfig, inputADO.DocumentName, inputADO.Treatment.TREATMENT_CODE, signName, inputADO.SignReason, null, optionSignType, listSign, signSelected, isSignPatient, isSignHomeRelation, documentTypeId, isMultiSignForProcess, inputADO.HisCode, inputADO, displayConfigParam, param, null, isSignParallel, GetTreatmentData(), GetSignerData(), GetTokenCodeData());

                    bool valid = true;
                    string messageErr = "";
                    valid = signProcessor.VerifyFile(currentDocument, signedCount, ref messageErr);
                    if (!valid)
                    {
                        rsData.Success = false;
                        rsData.Message = messageErr;
                    }
                    else
                    {
                        if (currentDocument == null)
                            currentDocument = new DocumentTDO();

                        signProcessor.SetFileType(fileType);

                        bool rsSuccess = signProcessor.SignFile(currentDocument, ref outputFile);
                        Inventec.Common.Logging.LogSystem.Info("SignDigital__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsSuccess), rsSuccess)
                            + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile)
                           + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFileWork), inputFileWork)
                            );
                        if (rsSuccess)
                        {
                            success = rsSuccess;
                            try
                            {
                                if (File.Exists(inputFileWork)) File.Delete(inputFileWork);
                            }
                            catch { }

                            rsData.Message = "OK";
                            rsData.Base64FileSigned = Utils.FileToBase64String(outputFile);
                            rsData.DocumentCode = currentDocument.DocumentCode;

                            inputFileWork = outputFile;
                        }
                        rsData.Success = success;
                    }
                }
                else if (!isSignOnlyWithHasAutoPosition)
                {
                    //Không tìm thấy tọa độ cần ký tiếp theo trong template
                    frmPdfViewer frmPdfViewer = new frmPdfViewer(inputFileWork, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData(), true, ActionAfterSignedProcess, isPrintNow);
                    frmPdfViewer.ShowDialog();

                    rsData.Success = !String.IsNullOrEmpty(this.outputSignedFileResult);
                    if (rsData.Success)
                    {
                        rsData.Message = "OK";
                        rsData.Base64FileSigned = Utils.FileToBase64String(this.outputSignedFileResult);
                        rsData.DocumentCode = inputADO.DocumentCode;
                    }
                }
                else
                {
                    rsData.Success = false;
                    rsData.Message = "Văn bản mã " + (!String.IsNullOrWhiteSpace(inputADO.DocumentCode) ? inputADO.DocumentCode : rsData.DocumentCode) + " chưa được thiết lập vị trí ký";
                }

                Inventec.Common.Logging.LogSystem.Info("ProcessSignPrint" + (isPrintPreview ? "Preview" : "Now") + ". 2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn("Co loi xay ra. Du lieu dau vao:____" + Inventec.Common.Logging.LogUtil.TraceData("base64FileContent", base64FileContent) + "____" + Inventec.Common.Logging.LogUtil.TraceData("fileType", fileType) + "____" + Inventec.Common.Logging.LogUtil.TraceData("inputADO", inputADO));
                Inventec.Common.Logging.LogSystem.Error(ex);
                rsData.Success = false;
            }

            try
            {
                outputSignedFileResult = null;
                inputADOWorking = null;
                signToken = null;
            }
            catch (Exception exx)
            {
                Inventec.Common.Logging.LogSystem.Warn(exx);
            }

            return rsData;
        }

        public DocumentSignedResultDTO ProcessSignPrintNow(List<FileADO> fileADOs, InputADO inputADO, bool isPrintNow, bool isPrintPreview = false, bool isSignOnlyWithHasAutoPosition = false)
        {
            Inventec.Common.Logging.LogSystem.Info("ProcessSignPrint" + (isPrintPreview ? "Preview" : "Now") + ". 1");
            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isPrintNow), isPrintNow)
                + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isPrintPreview), isPrintPreview)
                 + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isSignOnlyWithHasAutoPosition), isSignOnlyWithHasAutoPosition)
                 + Inventec.Common.Logging.LogUtil.TraceData("fileADOs.Count", (fileADOs != null ? fileADOs.Count : 0)));
            this.outputSignedFileResult = "";
            this.iSPrintNow = isPrintNow;
            this.iSPrintPreview = isPrintPreview;
            int typeDisplayOption = 0;
            DocumentSignedResultDTO rsData = new DocumentSignedResultDTO();
            try
            {
                string error = "";
                if (!Verify.VerifySignPrintNow(fileADOs, ref error))
                {
                    rsData.Success = false;
                    rsData.Message = MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe);
                    Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsData), rsData));
                    MessageBox.Show(rsData.Message);
                    return rsData;
                }

                FileADO fileADOMain = null;
                FileADO fileADOJson = null;
                FileADO fileADOXml = null;

                foreach (var fileSign in fileADOs)
                {
                    if (String.IsNullOrEmpty(fileSign.Base64FileContent))
                    {
                        rsData.Success = false;
                        Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe) + ". Du lieu file truyen vao khong hop le:____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO));
                        return rsData;
                    }
                    if (fileSign.IsMain.HasValue && fileSign.IsMain.Value)
                    {
                        fileADOMain = fileSign;
                    }
                    else if (fileSign.FileType == FileType.Json)
                    {
                        fileADOJson = fileSign;
                    }
                    else if (fileSign.FileType == FileType.Xml)
                    {
                        fileADOXml = fileSign;
                    }
                }

                if (fileADOMain == null && fileADOs.Count > 1)
                {
                    fileADOMain = fileADOs.FirstOrDefault();
                }
                else if (fileADOMain == null && fileADOs.Count == 1)
                {
                    fileADOMain = fileADOs.FirstOrDefault();
                    fileADOJson = null;
                    fileADOXml = null;
                }


                //foreach (var fileSign in fileADOs)
                //{
                string base64FileContent = fileADOMain.Base64FileContent;
                V_EMR_DOCUMENT documentData = new V_EMR_DOCUMENT();
                if (!ValidParam(inputADO, true, true, ref base64FileContent, ref documentData))
                {
                    rsData.Success = false;
                    rsData.Message = MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe);
                    Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsData), rsData));
                    return rsData;
                }

                string inputFileWork = "";
                string ext = Utils.GetExtByFileType(fileADOMain.FileType);
                var inputByte = Convert.FromBase64String(base64FileContent);
                string inputFileTemp = Utils.GenerateTempFileWithin(ext);
                Utils.ByteToFile(inputByte, inputFileTemp);
                Utils.ProcessFileInput(inputFileTemp, ext, ref inputFileWork);

                //Xử lý 2 trường hợp:
                //---Có khai báo tọa độ ký trong file template ==> ký & trả kết quả luôn 
                //---Không có khai báo tọa dộ ký trong file template ==> show lên form file ký & chọn điểm ký ==> ký ==> tự động tắt form trả kết quả

                bool success = false;
                bool isMultiSign = false;
                bool isMultiSignForAuto = false;
                bool isSignParallel = false;
                EMR.TDO.DocumentTDO currentDocument = null;
                int signedCount = 0;
                EMR.EFMODEL.DataModels.EMR_SIGN signSelected = null;
                List<SignPositionADO> nextSignPositions = null;
                iTextSharp.text.pdf.PdfReader readerWorking = null;
                if (!String.IsNullOrEmpty(inputADO.DocumentCode))
                {
                    signSelected = new EmrSign().GetSignDocumentFirst(inputADO.DocumentCode, GetSignerData(), GetTreatmentData(), isMultiSign, true);
                }
                this.printNumberCopies = inputADO.PrintNumberCopies.HasValue ? inputADO.PrintNumberCopies.Value : (short)1;
                bool isSignPatient = inputADO.IsPatientSign;
                bool isSignHomeRelation = inputADO.IsHomeRelativeSign.HasValue ? inputADO.IsHomeRelativeSign.Value : false;

                if (fileADOMain.FileType != FileType.Xml && fileADOMain.FileType != FileType.Json)
                {
                    readerWorking = new iTextSharp.text.pdf.PdfReader(inputFileWork);
                    ProcessCommentKey(inputADO, ref inputFileWork, ref nextSignPositions);

                    if (nextSignPositions != null && nextSignPositions.Count > 0)
                    {
                        readerWorking.Close();
                        readerWorking = new iTextSharp.text.pdf.PdfReader(inputFileWork);
                    }
                    nextSignPositions = GetNextPositionSigned(inputADO.IsPatientSign || (inputADO.IsHomeRelativeSign ?? false), readerWorking, signSelected, ref signedCount);
                    Inventec.Common.Logging.LogSystem.Debug("ProcessSignPrintNow____nextSignPositions.count=" + (nextSignPositions != null ? nextSignPositions.Count : 0));
                }

                List<SignTDO> listSign = null;
                string outputFile = "";

                if ((nextSignPositions != null && nextSignPositions.Count > 0) || (fileADOMain.FileType == FileType.Xml || fileADOMain.FileType == FileType.Json))
                {
                    if (inputADO.SignerConfigs != null && inputADO.SignerConfigs.Count > 0)
                    {
                        inputADO.SignerConfigs = inputADO.SignerConfigs.OrderBy(o => o.NumOrder).ToList();
                        listSign = new List<SignTDO>();

                        foreach (var scf in inputADO.SignerConfigs)
                        {
                            SignTDO sign = new SignTDO();
                            EMR_SIGNER signerFind = GlobalStore.GetByLoginName(scf.Loginname);
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
                                    sign.NumOrder = GetMaxNumOrder(listSign);
                                sign.Title = signerFind.TITLE;
                                sign.DepartmentCode = signerFind.DEPARTMENT_CODE;
                                sign.DepartmentName = signerFind.DEPARTMENT_NAME;
                                sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                                listSign.Add(sign);
                            }
                        }
                    }
                }

                string signName = (String.IsNullOrEmpty(GlobalStore.UserName) ? GlobalStore.LoginName : GlobalStore.LoginName + " (" + GlobalStore.UserName + ")");
                long? documentTypeId = GetDocumentTypeId(inputADO.DocumentTypeCode);

                if (nextSignPositions != null && nextSignPositions.Count > 0)
                {
                    isMultiSign = GetMultiSignDoc(inputADO, ref isSignParallel);
                    if (!String.IsNullOrEmpty(inputADO.DocumentCode))
                    {
                        currentDocument = GenerateByDocumentCode(inputADO.DocumentCode, ref isMultiSign);
                        isSignParallel = currentDocument != null && currentDocument.IsSignParallel.HasValue ? currentDocument.IsSignParallel.Value : false;//TODO
                    }
                    SignPositionADO nextSignPosition = nextSignPositions[0];
                    if (nextSignPositions.Count > 1)
                    {
                        nextSignPosition.SignPositionAutos = nextSignPositions;
                    }

                    bool? vOptionSign = VerifySign.VerifySignImageWithOption(inputADO, GetSignerData(), (nextSignPositions != null && nextSignPositions.Count > 0), nextSignPosition, isMultiSign);
                    if (vOptionSign.HasValue)
                    {
                        if (vOptionSign.Value)
                        {
                            typeDisplayOption = Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT;
                        }
                        else
                        {
                            rsData.Success = false;
                            rsData.Message = MessageUitl.GetMessage(MessageConstan.TaiKhoanThieuThongTinAnh);
                            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsData), rsData));
                            return rsData;
                        }
                    }

                    isMultiSignForAuto = false;
                    if (!isMultiSign && nextSignPositions != null && nextSignPositions.Count >= 2)
                    {
                        isMultiSignForAuto = true;
                    }

                    int demKey = 1;
                    foreach (var nSp in nextSignPositions)
                    {
                        if (currentDocument != null && !String.IsNullOrEmpty(currentDocument.DocumentCode) && nextSignPositions != null && nextSignPositions.Count >= 2)
                        {
                            signSelected = new EmrSign().GetSignDocumentFirst(currentDocument.DocumentCode, GetSignerData(), GetTreatmentData(), (isMultiSignForAuto || isMultiSign), false);
                        }


                        //Tìm thấy tọa độ cần ký tiếp theo trong template                       
                        Inventec.Common.Logging.LogSystem.Debug("Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.WidthRectangle), nSp.WidthRectangle) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.HeightRectangle), nSp.HeightRectangle) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.PageNUm), nSp.PageNUm) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.Text), nSp.Text));
                        float _x = (nSp.Reactanle.Left);
                        _x = _x < 0 ? 0 : _x;
                        float _y = (nSp.Reactanle.Bottom);
                        _y = _y < 0 ? 0 : _y;

                        int totalPageNumber = readerWorking.NumberOfPages;
                        VerifyPdfFileHandle verifyPdfFile = new VerifyPdfFileHandle();
                        List<VerifierADO> verifiers = verifyPdfFile.verify(readerWorking).OrderBy(o => o.Date).ToList();

                        SignHandle signProcessor = null;
                        CommonParam param = new CommonParam();

                        DisplayConfigDTO displayConfigParam = new DisplayConfigDTO()
                        {
                            HeightRectangle = (nSp.HeightRectangle > 0
                                ? nSp.HeightRectangle :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.HeightRectangle
                                    : null),
                            WidthRectangle = (nSp.WidthRectangle > 0
                                ? nSp.WidthRectangle :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.WidthRectangle
                                    : null),
                            SizeFont = (nSp.SizeFont > 0
                                ? nSp.SizeFont :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.SizeFont
                                    : null),
                            TextPosition = (nSp.TextPosition > 0
                                ? (int?)nSp.TextPosition :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.TextPosition
                                    : null),
                            TypeDisplay = (typeDisplayOption > 0 ? typeDisplayOption : (nSp.TypeDisplay > 0
                                ? nSp.TypeDisplay :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.TypeDisplay
                                    : null)),
                            IsDisplaySignature = (nSp.IsDisplaySignature.HasValue
                                ? nSp.IsDisplaySignature :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.IsDisplaySignature
                                    : null),
                            FormatRectangleText =
                                (inputADO.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADO.DisplayConfigDTO.FormatRectangleText)
                                ? inputADO.DisplayConfigDTO.FormatRectangleText : String.Empty),
                            Location =
                            (inputADO.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADO.DisplayConfigDTO.Location)
                            ? inputADO.DisplayConfigDTO.Location : String.Empty)
                            ,
                            Alignment = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.Alignment.HasValue)
                    ? inputADO.DisplayConfigDTO.Alignment
                    : null
                ),
                            IsBold = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.IsBold.HasValue)
                    ? inputADO.DisplayConfigDTO.IsBold
                    : null
                ),
                            IsItalic = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.IsItalic.HasValue)
                    ? inputADO.DisplayConfigDTO.IsItalic
                    : null
                ),
                            IsUnderlined = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.IsUnderlined.HasValue)
                    ? inputADO.DisplayConfigDTO.IsUnderlined
                    : null
                ),
                            FontName = (
                    (inputADO.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADO.DisplayConfigDTO.FontName))
                    ? inputADO.DisplayConfigDTO.FontName
                    : null
                )
                        };

                        SignType optionSignType = GetSignTypeConfig(inputADO);

                        bool isMultiSignForProcess = isMultiSign;
                        if (isMultiSignForAuto && demKey == nextSignPositions.Count)
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

                        signProcessor = new SignHandle(inputFileWork, _x, _y, nSp.PageNUm, totalPageNumber, null, inputADO.DlgOpenModuleConfig, inputADO.DocumentName, inputADO.Treatment.TREATMENT_CODE, signName, inputADO.SignReason, verifiers, optionSignType, listSign, signSelected, isSignPatient, isSignHomeRelation, documentTypeId, isMultiSignForProcess, inputADO.HisCode, inputADO, displayConfigParam, param, null, isSignParallel, GetTreatmentData(), GetSignerData(), GetTokenCodeData());

                        bool valid = true;
                        string messageErr = "";
                        valid = signProcessor.VerifyFile(currentDocument, signedCount, ref messageErr);
                        if (!valid)
                        {
                            rsData.Success = false;
                            rsData.Message = messageErr;
                        }
                        else
                        {
                            if (currentDocument == null)
                                currentDocument = new DocumentTDO();

                            currentDocument.OriginalVersion = new VersionTDO();
                            if (fileADOMain != null && !String.IsNullOrEmpty(fileADOMain.Base64FileContent))
                                currentDocument.OriginalVersion.Base64Data = fileADOMain.Base64FileContent;
                            if (fileADOXml != null && !String.IsNullOrEmpty(fileADOXml.Base64FileContent))
                                currentDocument.OriginalVersion.Base64DataXml = fileADOXml.Base64FileContent;
                            if (fileADOJson != null && !String.IsNullOrEmpty(fileADOJson.Base64FileContent))
                                currentDocument.OriginalVersion.Base64DataJson = fileADOJson.Base64FileContent;

                            bool rsSuccess = signProcessor.SignFile(currentDocument, ref outputFile);
                            Inventec.Common.Logging.LogSystem.Info("SignDigital__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsSuccess), rsSuccess)
                                + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile)
                               + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFileWork), inputFileWork)
                                );
                            if (rsSuccess)
                            {
                                success = rsSuccess;
                                try
                                {
                                    if (readerWorking != null) readerWorking.Close();
                                    if (File.Exists(inputFileWork)) File.Delete(inputFileWork);
                                }
                                catch { }

                                rsData.Message = "OK";
                                rsData.Base64FileSigned = Utils.FileToBase64String(outputFile);
                                rsData.DocumentCode = currentDocument.DocumentCode;

                                inputFileWork = outputFile;
                            }
                            rsData.Success = success;
                        }

                        demKey++;
                    }

                    if (nextSignPositions != null && nextSignPositions.Count > 0 && success && File.Exists(outputFile))
                    {
                        Inventec.Common.Logging.LogSystem.Info("SignDigital__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile));
                        try
                        {
                            if (readerWorking != null) readerWorking.Close();
                            //if (File.Exists(inputFileWork)) File.Delete(inputFileWork);
                        }
                        catch { }

                        if (!isSignOnlyWithHasAutoPosition)
                        {
                            rsData.Message = "OK";
                            rsData.Base64FileSigned = Utils.FileToBase64String(outputFile);

                            if (fileADOMain.FileType != FileType.Xml && fileADOMain.FileType != FileType.Json)
                            {
                                if (isPrintNow)
                                {
                                    PrintNowProcess(outputFile);
                                }
                                else if (isPrintPreview)
                                {
                                    PrintPreviewProcess(outputFile);
                                }
                            }
                        }

                        rsData.Success = success;
                    }
                }
                else if (fileADOMain.FileType == FileType.Xml || fileADOMain.FileType == FileType.Json)
                {
                    SignHandle signProcessor = null;
                    CommonParam param = new CommonParam();
                    DisplayConfigDTO displayConfigParam = inputADO.DisplayConfigDTO;

                    SignType optionSignType = GetSignTypeConfig(inputADO);

                    bool isMultiSignForProcess = isMultiSign;

                    signProcessor = new SignHandle(inputFileWork, 1, 1, 1, 1, null, inputADO.DlgOpenModuleConfig, inputADO.DocumentName, inputADO.Treatment.TREATMENT_CODE, signName, inputADO.SignReason, null, optionSignType, listSign, signSelected, isSignPatient, isSignHomeRelation, documentTypeId, isMultiSignForProcess, inputADO.HisCode, inputADO, displayConfigParam, param, null, isSignParallel, GetTreatmentData(), GetSignerData(), GetTokenCodeData());

                    bool valid = true;
                    string messageErr = "";
                    valid = signProcessor.VerifyFile(currentDocument, signedCount, ref messageErr);
                    if (!valid)
                    {
                        rsData.Success = false;
                        rsData.Message = messageErr;
                    }
                    else
                    {
                        if (currentDocument == null)
                            currentDocument = new DocumentTDO();

                        currentDocument.OriginalVersion = new VersionTDO();
                        if (fileADOMain != null && !String.IsNullOrEmpty(fileADOMain.Base64FileContent))
                            currentDocument.OriginalVersion.Base64Data = fileADOMain.Base64FileContent;
                        if (fileADOXml != null && !String.IsNullOrEmpty(fileADOXml.Base64FileContent))
                            currentDocument.OriginalVersion.Base64DataXml = fileADOXml.Base64FileContent;
                        if (fileADOJson != null && !String.IsNullOrEmpty(fileADOJson.Base64FileContent))
                            currentDocument.OriginalVersion.Base64DataJson = fileADOJson.Base64FileContent;

                        signProcessor.SetFileType(fileADOMain.FileType);

                        bool rsSuccess = signProcessor.SignFile(currentDocument, ref outputFile);
                        Inventec.Common.Logging.LogSystem.Info("SignDigital__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsSuccess), rsSuccess)
                            + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile)
                           + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFileWork), inputFileWork)
                            );
                        if (rsSuccess)
                        {
                            success = rsSuccess;
                            try
                            {
                                if (File.Exists(inputFileWork)) File.Delete(inputFileWork);
                            }
                            catch { }

                            rsData.Message = "OK";
                            rsData.Base64FileSigned = Utils.FileToBase64String(outputFile);
                            rsData.DocumentCode = currentDocument.DocumentCode;

                            inputFileWork = outputFile;
                        }
                        rsData.Success = success;
                    }
                }
                else if (!isSignOnlyWithHasAutoPosition)
                {
                    //Không tìm thấy tọa độ cần ký tiếp theo trong template
                    frmPdfViewer frmPdfViewer = new frmPdfViewer(inputFileWork, inputADO, GetSignerData(), GetTreatmentData(), GetTokenCodeData(), true, ActionAfterSignedProcess, isPrintNow);
                    frmPdfViewer.UpdateExtFileType(fileADOMain, fileADOJson, fileADOXml);
                    frmPdfViewer.ShowDialog();

                    rsData.Success = !String.IsNullOrEmpty(this.outputSignedFileResult);
                    if (rsData.Success)
                    {
                        rsData.Message = "OK";
                        rsData.Base64FileSigned = Utils.FileToBase64String(this.outputSignedFileResult);
                        rsData.DocumentCode = inputADO.DocumentCode;
                    }
                }
                else
                {
                    rsData.Success = false;
                    rsData.Message = "Văn bản mã " + (!String.IsNullOrWhiteSpace(inputADO.DocumentCode) ? inputADO.DocumentCode : rsData.DocumentCode) + " chưa được thiết lập vị trí ký";
                }

                Inventec.Common.Logging.LogSystem.Info("ProcessSignPrint" + (isPrintPreview ? "Preview" : "Now") + ". 2");
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn("Co loi xay ra. Du lieu dau vao:____" + Inventec.Common.Logging.LogUtil.TraceData("fileADOs", fileADOs) + "____" + Inventec.Common.Logging.LogUtil.TraceData("inputADO", inputADO));
                Inventec.Common.Logging.LogSystem.Error(ex);
                rsData.Success = false;
            }

            try
            {
                outputSignedFileResult = null;
                inputADOWorking = null;
                signToken = null;
            }
            catch (Exception exx)
            {
                Inventec.Common.Logging.LogSystem.Warn(exx);
            }

            return rsData;
        }

        private SignType GetSignTypeConfig(InputADO inputADO)
        {
            SignType optionSignType = SignType.HSM;
            if (inputADO.SignType == SignType.OptionDefaultHsm || inputADO.SignType == SignType.OptionDefaultUsb)
            {
                string vlSignType = CacheClientWorker.GetValue(RegistryConstant.SIGN_TYPE_OPTION_KEY);
                if (!String.IsNullOrEmpty(vlSignType))
                {
                    optionSignType = vlSignType == "1" ? SignType.USB : SignType.HSM;
                }
                else
                {
                    if (inputADO.IsOptionSignType.HasValue)
                    {
                        optionSignType = inputADO.IsOptionSignType.Value ? SignType.USB : SignType.HSM;
                    }
                    else if (inputADO.SignType == SignType.OptionDefaultUsb)
                    {
                        optionSignType = SignType.USB;
                    }
                    else
                    {
                        optionSignType = SignType.HSM;
                    }
                }
            }
            else
            {
                optionSignType = inputADO.SignType;
            }
            return optionSignType;
        }

        public DocumentSignedResultDTO SignWithListKeyAndUser(string base64FileContent, FileType fileType, InputADO inputADO)
        {
            DocumentSignedResultDTO rsData = new DocumentSignedResultDTO();
            this.outputSignedFileResult = "";
            this.iSPrintNow = false;
            this.iSPrintPreview = false;
            int typeDisplayOption = 0;
            try
            {
                if (String.IsNullOrEmpty(base64FileContent))
                {
                    rsData.Success = false;
                    rsData.Message = MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe);
                    Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsData), rsData));
                    MessageBox.Show(rsData.Message);
                    return rsData;
                }


                V_EMR_DOCUMENT documentData = new V_EMR_DOCUMENT();
                if (!ValidParam(inputADO, true, true, ref base64FileContent, ref documentData) || (inputADO.SignerConfigs == null || inputADO.SignerConfigs.Count == 0))
                {
                    rsData.Success = false;
                    rsData.Message = MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe);
                    Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsData), rsData));
                    return rsData;
                }


                string inputFileWork = "";
                string ext = Utils.GetExtByFileType(fileType);
                var inputByte = Convert.FromBase64String(base64FileContent);
                string inputFileTemp = Utils.GenerateTempFileWithin(ext);
                Utils.ByteToFile(inputByte, inputFileTemp);
                Utils.ProcessFileInput(inputFileTemp, ext, ref inputFileWork);

                bool success = false;
                bool isMultiSign = false;
                bool isMultiSignForAuto = false;
                bool isSignParallel = false;
                EMR.TDO.DocumentTDO currentDocument = null;
                int signedCount = 0;
                EMR.EFMODEL.DataModels.EMR_SIGN signSelected = null;

                iTextSharp.text.pdf.PdfReader readerWorking = new iTextSharp.text.pdf.PdfReader(inputFileWork);

                //if (!String.IsNullOrEmpty(inputADO.DocumentCode))
                //{
                //    signSelected = new EmrSign().GetSignDocumentFirst(inputADO.DocumentCode, GetSignerData(), GetTreatmentData(), isMultiSign, true);
                //}

                List<SignPositionADO> nextSignPositions = null;
                ProcessListSignWithUserKey(inputADO, ref inputFileWork, ref nextSignPositions);

                if (nextSignPositions != null && nextSignPositions.Count > 0)
                {
                    readerWorking.Close();
                    readerWorking = new iTextSharp.text.pdf.PdfReader(inputFileWork);
                }

                nextSignPositions = GetNextPositionSigned(inputADO.IsPatientSign || (inputADO.IsHomeRelativeSign ?? false) , readerWorking, signSelected, ref signedCount);

                this.printNumberCopies = inputADO.PrintNumberCopies.HasValue ? inputADO.PrintNumberCopies.Value : (short)1;

                if (nextSignPositions != null && nextSignPositions.Count > 0)
                {
                    isMultiSign = GetMultiSignDoc(inputADO, ref isSignParallel);
                    if (!String.IsNullOrEmpty(inputADO.DocumentCode))
                    {
                        currentDocument = GenerateByDocumentCode(inputADO.DocumentCode, ref isMultiSign);
                        isSignParallel = currentDocument != null && currentDocument.IsSignParallel.HasValue ? currentDocument.IsSignParallel.Value : false;//TODO
                    }
                    SignPositionADO nextSignPosition = nextSignPositions[0];
                    if (nextSignPositions.Count > 1)
                    {
                        nextSignPosition.SignPositionAutos = nextSignPositions;
                    }

                    List<SignTDO> listSign = null;
                    isSignParallel = true;//Fix

                    if (inputADO.SignerConfigs != null && inputADO.SignerConfigs.Count > 0)
                    {
                        inputADO.SignerConfigs = inputADO.SignerConfigs.OrderBy(o => o.NumOrder).ToList();
                        listSign = new List<SignTDO>();

                        foreach (var scf in inputADO.SignerConfigs)
                        {
                            SignTDO sign = new SignTDO();
                            EMR_SIGNER signerFind = GlobalStore.GetByLoginName(scf.Loginname);
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
                                    sign.NumOrder = GetMaxNumOrder(listSign);
                                sign.Title = signerFind.TITLE;
                                sign.DepartmentCode = signerFind.DEPARTMENT_CODE;
                                sign.DepartmentName = signerFind.DEPARTMENT_NAME;
                                sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                                listSign.Add(sign);
                            }
                        }
                    }


                    bool isSignPatient = inputADO.IsPatientSign;
                    bool isSignHomeRelation = inputADO.IsHomeRelativeSign.HasValue ? inputADO.IsHomeRelativeSign.Value : false;

                    string outputFile = "";
                    isMultiSignForAuto = false;
                    if (!isMultiSign && nextSignPositions != null && nextSignPositions.Count >= 2)
                    {
                        isMultiSignForAuto = true;
                    }
                    int demKey = 1;
                    foreach (var nSp in nextSignPositions)
                    {
                        var scf = inputADO.SignerConfigs.Where(o => o.NumOrder == VerifySign.GetNumOderByCommentText(nSp.Text)).FirstOrDefault();
                        EMR_SIGNER signerFind = GlobalStore.GetByLoginName(scf.Loginname);

                        bool? vOptionSign = VerifySign.VerifySignImageWithOption(inputADO, signerFind, (nextSignPositions != null && nextSignPositions.Count > 0), nextSignPosition, isMultiSign);
                        if (vOptionSign.HasValue)
                        {
                            if (vOptionSign.Value)
                            {
                                typeDisplayOption = Inventec.Common.SignFile.Constans.DISPLAY_RECTANGLE_TEXT;
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.TaiKhoanThieuThongTinAnh) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => vOptionSign), vOptionSign) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => scf), scf));
                                continue;
                            }
                        }


                        if (currentDocument != null && !String.IsNullOrEmpty(currentDocument.DocumentCode) && nextSignPositions != null && nextSignPositions.Count >= 2)
                        {
                            signSelected = new EmrSign().GetSignDocumentFirst(currentDocument.DocumentCode, signerFind, GetTreatmentData(), (isMultiSignForAuto || isMultiSign), false);
                        }

                        //Tìm thấy tọa độ cần ký tiếp theo trong template                       
                        Inventec.Common.Logging.LogSystem.Debug("SignWithListKeyAndUser: Truong hop van ban ky co comment danh dau vi tri can ky." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.WidthRectangle), nSp.WidthRectangle) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.HeightRectangle), nSp.HeightRectangle) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.PageNUm), nSp.PageNUm) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => nSp.Text), nSp.Text));
                        float _x = (nSp.Reactanle.Left);
                        _x = _x < 0 ? 0 : _x;
                        float _y = (nSp.Reactanle.Bottom);
                        _y = _y < 0 ? 0 : _y;

                        int totalPageNumber = readerWorking.NumberOfPages;
                        string signName = (String.IsNullOrEmpty(GlobalStore.UserName) ? GlobalStore.LoginName : GlobalStore.LoginName + " (" + GlobalStore.UserName + ")");
                        VerifyPdfFileHandle verifyPdfFile = new VerifyPdfFileHandle();
                        List<VerifierADO> verifiers = verifyPdfFile.verify(readerWorking).OrderBy(o => o.Date).ToList();
                        long? documentTypeId = GetDocumentTypeId(inputADO.DocumentTypeCode);

                        SignHandle signProcessor = null;
                        CommonParam param = new CommonParam();

                        DisplayConfigDTO displayConfigParam = new DisplayConfigDTO()
                        {
                            HeightRectangle = (nSp.HeightRectangle > 0
                                ? nSp.HeightRectangle :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.HeightRectangle
                                    : null),
                            WidthRectangle = (nSp.WidthRectangle > 0
                                ? nSp.WidthRectangle :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.WidthRectangle
                                    : null),
                            SizeFont = (nSp.SizeFont > 0
                                ? nSp.SizeFont :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.SizeFont
                                    : null),
                            TextPosition = (nSp.TextPosition > 0
                                ? (int?)nSp.TextPosition :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.TextPosition
                                    : null),
                            TypeDisplay = (typeDisplayOption > 0 ? typeDisplayOption : (nSp.TypeDisplay > 0
                                ? nSp.TypeDisplay :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.TypeDisplay
                                    : null)),
                            IsDisplaySignature = (nSp.IsDisplaySignature.HasValue
                                ? nSp.IsDisplaySignature :
                                inputADO.DisplayConfigDTO != null
                                    ? inputADO.DisplayConfigDTO.IsDisplaySignature
                                    : null),
                            FormatRectangleText =
                                (inputADO.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADO.DisplayConfigDTO.FormatRectangleText)
                                ? inputADO.DisplayConfigDTO.FormatRectangleText : String.Empty),
                            Location =
                            (inputADO.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADO.DisplayConfigDTO.Location)
                            ? inputADO.DisplayConfigDTO.Location : String.Empty),
                            Alignment = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.Alignment.HasValue)
                    ? inputADO.DisplayConfigDTO.Alignment
                    : null
                ),
                            IsBold = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.IsBold.HasValue)
                    ? inputADO.DisplayConfigDTO.IsBold
                    : null
                ),
                            IsItalic = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.IsItalic.HasValue)
                    ? inputADO.DisplayConfigDTO.IsItalic
                    : null
                ),
                            IsUnderlined = (
                    (inputADO.DisplayConfigDTO != null && inputADO.DisplayConfigDTO.IsUnderlined.HasValue)
                    ? inputADO.DisplayConfigDTO.IsUnderlined
                    : null
                ),
                            FontName = (
                    (inputADO.DisplayConfigDTO != null && !String.IsNullOrEmpty(inputADO.DisplayConfigDTO.FontName))
                    ? inputADO.DisplayConfigDTO.FontName
                    : null
                )
                        };

                        SignType optionSignType = SignType.HSM;

                        if (inputADO.SignType == SignType.OptionDefaultHsm || inputADO.SignType == SignType.OptionDefaultUsb)
                        {
                            string vlSignType = CacheClientWorker.GetValue(RegistryConstant.SIGN_TYPE_OPTION_KEY);
                            if (!String.IsNullOrEmpty(vlSignType))
                            {
                                optionSignType = vlSignType == "1" ? SignType.USB : SignType.HSM;
                            }
                            else
                            {
                                if (inputADO.IsOptionSignType.HasValue)
                                {
                                    optionSignType = inputADO.IsOptionSignType.Value ? SignType.USB : SignType.HSM;
                                }
                                else if (inputADO.SignType == SignType.OptionDefaultUsb)
                                {
                                    optionSignType = SignType.USB;
                                }
                                else
                                {
                                    optionSignType = SignType.HSM;
                                }
                            }
                        }
                        else
                        {
                            optionSignType = inputADO.SignType;
                        }

                        bool isMultiSignForProcess = isMultiSign;
                        if (isMultiSignForAuto && demKey == nextSignPositions.Count)
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


                        signProcessor = new SignHandle(inputFileWork, _x, _y, nSp.PageNUm, totalPageNumber, null, inputADO.DlgOpenModuleConfig, inputADO.DocumentName, inputADO.Treatment.TREATMENT_CODE, signName, inputADO.SignReason, verifiers, optionSignType, listSign, signSelected, isSignPatient, isSignHomeRelation, documentTypeId, isMultiSignForProcess, inputADO.HisCode, inputADO, displayConfigParam, param, null, isSignParallel, GetTreatmentData(), signerFind, GetTokenCodeData());

                        bool valid = true;
                        string messageErr = "";
                        valid = signProcessor.VerifyFile(currentDocument, signedCount, ref messageErr);
                        if (!valid)
                        {
                            rsData.Success = false;
                            rsData.Message = messageErr;
                        }
                        else
                        {
                            if (currentDocument == null)
                                currentDocument = new DocumentTDO();

                            bool rsSuccess = signProcessor.SignFile(currentDocument, ref outputFile);
                            Inventec.Common.Logging.LogSystem.Info("SignWithListKeyAndUser.SignDigital__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rsSuccess), rsSuccess)
                                + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile)
                               + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFileWork), inputFileWork)
                                );
                            if (rsSuccess)
                            {
                                success = rsSuccess;
                                try
                                {
                                    if (readerWorking != null) readerWorking.Close();
                                    if (File.Exists(inputFileWork)) File.Delete(inputFileWork);
                                }
                                catch { }

                                rsData.Message = "OK";
                                rsData.Base64FileSigned = Utils.FileToBase64String(outputFile);
                                rsData.DocumentCode = currentDocument.DocumentCode;

                                inputFileWork = outputFile;
                            }
                            rsData.Success = success;
                        }

                        demKey++;
                    }

                    if (nextSignPositions != null && nextSignPositions.Count > 0 && success && File.Exists(outputFile))
                    {
                        Inventec.Common.Logging.LogSystem.Info("SignWithListKeyAndUser.SignDigital__" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile));
                        try
                        {
                            if (readerWorking != null) readerWorking.Close();
                            //if (File.Exists(inputFileWork)) File.Delete(inputFileWork);
                        }
                        catch { }

                        rsData.Success = success;
                    }
                }
                else
                {
                    rsData.Success = false;
                    rsData.Message = "Văn bản mã " + (!String.IsNullOrWhiteSpace(inputADO.DocumentCode) ? inputADO.DocumentCode : rsData.DocumentCode) + " chưa được thiết lập vị trí ký";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Error(ex);
                rsData.Success = false;
            }
            return rsData;
        }


        private void ProcessCommentKey(InputADO inputADOWorking, ref string inputFileWork, ref List<SignPositionADO> signPositionADOs)
        {
            try
            {
                if (inputADOWorking != null && inputADOWorking.IsSign)
                {
                    string outFile = Utils.GenerateTempFileWithin();
                    PdfCommentKeyProcess pdfCommentKeyProcess = new PdfCommentKeyProcess();
                    var signPosition1s = pdfCommentKeyProcess.Run(inputFileWork, ref outFile);
                    if (signPosition1s != null && signPosition1s.Count > 0 && !String.IsNullOrEmpty(outFile) && File.Exists(outFile))
                    {
                        try
                        {
                            File.Delete(inputFileWork);
                        }
                        catch { }
                        inputFileWork = outFile;
                        if (signPositionADOs == null)
                        {
                            signPositionADOs = new List<SignPositionADO>();
                        }
                        signPositionADOs.AddRange(signPosition1s);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void ProcessListSignWithUserKey(InputADO inputADOWorking, ref string inputFileWork, ref List<SignPositionADO> signPositionADOs)
        {
            try
            {
                if (inputADOWorking != null && inputADOWorking.IsSign)
                {
                    string outFile = Utils.GenerateTempFileWithin();
                    PdfCommentKeyProcess pdfCommentKeyProcess = new PdfCommentKeyProcess();
                    var signPosition1s = pdfCommentKeyProcess.RunWithUserKey(inputFileWork, ref outFile);
                    if (signPosition1s != null && signPosition1s.Count > 0 && !String.IsNullOrEmpty(outFile) && File.Exists(outFile))
                    {
                        try
                        {
                            File.Delete(inputFileWork);
                        }
                        catch { }
                        inputFileWork = outFile;
                        if (signPositionADOs == null)
                        {
                            signPositionADOs = new List<SignPositionADO>();
                        }
                        signPositionADOs.AddRange(signPosition1s);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        public bool CreateDocument(InputADO inputADO, string base64FileContent)
        {
            bool success = false;
            try
            {
                if (!ValidParam(inputADO))
                {
                    success = false;
                    Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    return success;
                }


                if (String.IsNullOrEmpty(base64FileContent))
                {
                    success = false;
                    Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe) + ". Du lieu file truyen vao khong hop le:____");
                    return success;
                }

                //V_EMR_DOCUMENT documentData = new V_EMR_DOCUMENT();
                //if (!ValidParam(inputADO, true, true, ref base64FileContent, ref documentData))
                //{
                //    return false;
                //}

                string inputFileWork = "";
                var inputByte = Convert.FromBase64String(base64FileContent);
                string ext = Utils.GetExtByFileType(FileType.Pdf);
                string inputFileTemp = Utils.GenerateTempFileWithin(ext);
                Utils.ByteToFile(inputByte, inputFileTemp);
                Utils.ProcessFileInput(inputFileTemp, ext, ref inputFileWork);
                string signName = (String.IsNullOrEmpty(GlobalStore.UserName) ? GlobalStore.LoginName : GlobalStore.LoginName + " (" + GlobalStore.UserName + ")");
                List<SignTDO> listSign = null;
                if (inputADO.SignerConfigs != null && inputADO.SignerConfigs.Count > 0)
                {
                    inputADO.SignerConfigs = inputADO.SignerConfigs.OrderBy(o => o.NumOrder).ToList();
                    listSign = new List<SignTDO>();

                    foreach (var scf in inputADO.SignerConfigs)
                    {
                        SignTDO sign = new SignTDO();
                        EMR_SIGNER signerFind = GlobalStore.GetByLoginName(scf.Loginname);
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
                                sign.NumOrder = GetMaxNumOrder(listSign);
                            sign.Title = signerFind.TITLE;
                            sign.DepartmentCode = signerFind.DEPARTMENT_CODE;
                            sign.DepartmentName = signerFind.DEPARTMENT_NAME;
                            sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                            listSign.Add(sign);
                        }
                    }
                }
                bool isSignParanel = false, isMultiSign = false, isMultiSignByType = false;
                var docType = new EmrDocumentType().DocumentTypeProperty(inputADO.DocumentTypeCode);
                isMultiSign = (docType != null && docType.IS_MULTI_SIGN == 1);
                isSignParanel = (docType != null && docType.IS_SIGN_PARALLEL == 1);
                isMultiSign = isMultiSignByType;
                long? documentTypeId = (docType != null ? (long?)docType.ID : null);
                if (inputADO.IsMultiSign.HasValue)
                {
                    isMultiSign = isMultiSign && inputADO.IsMultiSign.Value;
                }
                CommonParam param = new CommonParam();
                SignHandle signProcessor = new SignHandle(inputFileWork, 0, 0, 0, 0, null, null, inputADO.DocumentName, inputADO.Treatment.TREATMENT_CODE, signName, inputADO.SignReason, null, inputADO.SignType, listSign, null, inputADO.IsPatientSign, false, documentTypeId, isMultiSign, inputADO.HisCode, inputADO, null, param, null, isSignParanel, GetTreatmentData(), GetSignerData(), GetTokenCodeData());
                DocumentTDO document = new DocumentTDO();
                document.IsSignParallel = isSignParanel;

                var currentDocument = signProcessor.SendDocument(document);
                if (currentDocument != null && !String.IsNullOrEmpty(currentDocument.DocumentCode))
                {
                    success = true;

                    inputADO.DocumentCode = currentDocument.DocumentCode;
                    //TODO
                }
                else
                {
                    param.Messages.Add("Xử lý thất bại");
                    MessageManager.Show(param, false);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn("Co loi xay ra. Du lieu dau vao:____" + Inventec.Common.Logging.LogUtil.TraceData("inputADO", inputADO));
                Inventec.Common.Logging.LogSystem.Error(ex);
                success = false;
            }
            return success;
        }

        public bool CreateDocument(InputADO inputADO, FileType fileType, string base64FileContent)
        {
            bool success = false;
            try
            {
                if (!ValidParam(inputADO))
                {
                    success = false;
                    Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe) + ". Du lieu dau vao:____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO));
                    return success;
                }


                if (String.IsNullOrEmpty(base64FileContent))
                {
                    success = false;
                    Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe) + ". Du lieu file truyen vao khong hop le:____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO));
                    return success;
                }


                string inputFileWork = "";
                var inputByte = Convert.FromBase64String(base64FileContent);
                string ext = Utils.GetExtByFileType(fileType);
                string inputFileTemp = Utils.GenerateTempFileWithin(ext);
                Utils.ByteToFile(inputByte, inputFileTemp);
                Utils.ProcessFileInput(inputFileTemp, ext, ref inputFileWork);
                string signName = (String.IsNullOrEmpty(GlobalStore.UserName) ? GlobalStore.LoginName : GlobalStore.LoginName + " (" + GlobalStore.UserName + ")");
                List<SignTDO> listSign = null;
                if (inputADO.SignerConfigs != null && inputADO.SignerConfigs.Count > 0)
                {
                    inputADO.SignerConfigs = inputADO.SignerConfigs.OrderBy(o => o.NumOrder).ToList();
                    listSign = new List<SignTDO>();

                    foreach (var scf in inputADO.SignerConfigs)
                    {
                        SignTDO sign = new SignTDO();
                        EMR_SIGNER signerFind = GlobalStore.GetByLoginName(scf.Loginname);
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
                                sign.NumOrder = GetMaxNumOrder(listSign);
                            sign.Title = signerFind.TITLE;
                            sign.DepartmentCode = signerFind.DEPARTMENT_CODE;
                            sign.DepartmentName = signerFind.DEPARTMENT_NAME;
                            sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                            listSign.Add(sign);
                        }
                    }
                }
                bool isSignParanel = false, isMultiSign = false, isMultiSignByType = false;
                var docType = new EmrDocumentType().DocumentTypeProperty(inputADO.DocumentTypeCode);
                isMultiSign = (docType != null && docType.IS_MULTI_SIGN == 1);
                isSignParanel = (docType != null && docType.IS_SIGN_PARALLEL == 1);
                isMultiSign = isMultiSignByType;
                long? documentTypeId = (docType != null ? (long?)docType.ID : null);
                if (inputADO.IsMultiSign.HasValue)
                {
                    isMultiSign = isMultiSign && inputADO.IsMultiSign.Value;
                }
                CommonParam param = new CommonParam();
                SignHandle signProcessor = new SignHandle(inputFileWork, 0, 0, 0, 0, null, null, inputADO.DocumentName, inputADO.Treatment.TREATMENT_CODE, signName, inputADO.SignReason, null, inputADO.SignType, listSign, null, inputADO.IsPatientSign, false, documentTypeId, isMultiSign, inputADO.HisCode, inputADO, null, param, null, isSignParanel, GetTreatmentData(), GetSignerData(), GetTokenCodeData());
                DocumentTDO document = new DocumentTDO();
                document.IsSignParallel = isSignParanel;
                signProcessor.SetFileType(fileType);

                var currentDocument = signProcessor.SendDocument(document);
                if (currentDocument != null && !String.IsNullOrEmpty(currentDocument.DocumentCode))
                {
                    success = true;

                    inputADO.DocumentCode = currentDocument.DocumentCode;
                }
                else
                {
                    param.Messages.Add("Xử lý thất bại");
                    MessageManager.Show(param, false);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn("Co loi xay ra. Du lieu dau vao:____" + Inventec.Common.Logging.LogUtil.TraceData("inputADO", inputADO));
                Inventec.Common.Logging.LogSystem.Error(ex);
                success = false;
            }
            return success;
        }

        public bool CreateDocument(InputADO inputADO, List<FileADO> fileADOs)
        {
            return CreateDocument(inputADO, fileADOs, true);
        }

        public bool CreateDocument(InputADO inputADO, List<FileADO> fileADOs, bool isShowMessage)
        {
            bool success = false;
            try
            {
                string error = "";
                if (!Verify.VerifySignPrintNow(fileADOs, ref error))
                {
                    success = false;
                    Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    if (isShowMessage)
                        MessageManager.ShowAlert(Form.ActiveForm, MessageUitl.GetMessage(MessageConstan.ThongBao), MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    return success;
                }
                FileADO fileADOMain = null;
                FileADO fileADOJson = null;
                FileADO fileADOXml = null;
                if (!ValidParam(inputADO))
                {
                    success = false;
                    Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe) + ". Du lieu dau vao:____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO));
                    if (isShowMessage)
                        MessageManager.ShowAlert(Form.ActiveForm, MessageUitl.GetMessage(MessageConstan.ThongBao), MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    return success;
                }

                CommonParam param = new CommonParam();
                foreach (var fileSign in fileADOs)
                {
                    if (String.IsNullOrEmpty(fileSign.Base64FileContent))
                    {
                        success = false;
                        Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe) + ". Du lieu file truyen vao khong hop le:____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO), inputADO));
                        return success;
                    }
                    if (fileSign.IsMain.HasValue && fileSign.IsMain.Value)
                    {
                        fileADOMain = fileSign;
                    }
                    else if (fileSign.FileType == FileType.Json)
                    {
                        fileADOJson = fileSign;
                    }
                    else if (fileSign.FileType == FileType.Xml)
                    {
                        fileADOXml = fileSign;
                    }
                }

                if (fileADOMain == null && fileADOs.Count > 1)
                {
                    fileADOMain = fileADOs.FirstOrDefault();
                }
                else if (fileADOMain == null && fileADOs.Count == 1)
                {
                    fileADOMain = fileADOs.FirstOrDefault();
                    fileADOJson = null;
                    fileADOXml = null;
                }

                string inputFileWork = "";
                var inputByte = Convert.FromBase64String(fileADOMain.Base64FileContent);
                string ext = Utils.GetExtByFileType(fileADOMain.FileType);
                string inputFileTemp = Utils.GenerateTempFileWithin(ext);
                Utils.ByteToFile(inputByte, inputFileTemp);
                Utils.ProcessFileInput(inputFileTemp, ext, ref inputFileWork);
                string signName = (String.IsNullOrEmpty(GlobalStore.UserName) ? GlobalStore.LoginName : GlobalStore.LoginName + " (" + GlobalStore.UserName + ")");
                List<SignTDO> listSign = null;
                if (inputADO.SignerConfigs != null && inputADO.SignerConfigs.Count > 0)
                {
                    inputADO.SignerConfigs = inputADO.SignerConfigs.OrderBy(o => o.NumOrder).ToList();
                    listSign = new List<SignTDO>();

                    foreach (var scf in inputADO.SignerConfigs)
                    {
                        SignTDO sign = new SignTDO();
                        EMR_SIGNER signerFind = GlobalStore.GetByLoginName(scf.Loginname);
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
                                sign.NumOrder = GetMaxNumOrder(listSign);
                            sign.Title = signerFind.TITLE;
                            sign.DepartmentCode = signerFind.DEPARTMENT_CODE;
                            sign.DepartmentName = signerFind.DEPARTMENT_NAME;
                            sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                            listSign.Add(sign);
                        }
                    }
                }
                bool isSignParanel = false, isMultiSign = false, isMultiSignByType = false;
                var docType = new EmrDocumentType().DocumentTypeProperty(inputADO.DocumentTypeCode);
                isMultiSign = (docType != null && docType.IS_MULTI_SIGN == 1);
                isSignParanel = (docType != null && docType.IS_SIGN_PARALLEL == 1);
                isMultiSign = isMultiSignByType;
                long? documentTypeId = (docType != null ? (long?)docType.ID : null);
                if (inputADO.IsMultiSign.HasValue)
                {
                    isMultiSign = isMultiSign && inputADO.IsMultiSign.Value;
                }
                CommonParam commonParam = new CommonParam();
                SignHandle signProcessor = new SignHandle(inputFileWork, 0, 0, 0, 0, null, null, inputADO.DocumentName, inputADO.Treatment.TREATMENT_CODE, signName, inputADO.SignReason, null, inputADO.SignType, listSign, null, inputADO.IsPatientSign, false, documentTypeId, isMultiSign, inputADO.HisCode, inputADO, null, commonParam, null, isSignParanel, GetTreatmentData(), GetSignerData(), GetTokenCodeData());
                DocumentTDO document = new DocumentTDO();
                document.IsSignParallel = isSignParanel;
                signProcessor.SetFileType(fileADOMain.FileType);

                document.OriginalVersion = new VersionTDO();
                if (fileADOMain != null && !String.IsNullOrEmpty(fileADOMain.Base64FileContent))
                    document.OriginalVersion.Base64Data = fileADOMain.Base64FileContent;
                if (fileADOXml != null && !String.IsNullOrEmpty(fileADOXml.Base64FileContent))
                    document.OriginalVersion.Base64DataXml = fileADOXml.Base64FileContent;
                if (fileADOJson != null && !String.IsNullOrEmpty(fileADOJson.Base64FileContent))
                    document.OriginalVersion.Base64DataJson = fileADOJson.Base64FileContent;

                var currentDocument = signProcessor.SendDocument(document);
                if (currentDocument != null && !String.IsNullOrEmpty(currentDocument.DocumentCode))
                {
                    success = true;
                    inputADO.DocumentCode = currentDocument.DocumentCode;
                }
                if (commonParam.Messages != null && commonParam.Messages.Count > 0)
                {
                    param.Messages.AddRange(commonParam.Messages);
                }
                if (commonParam.BugCodes != null && commonParam.BugCodes.Count > 0)
                {
                    param.BugCodes.AddRange(commonParam.BugCodes);
                }

                if (!success)
                {
                    param.Messages.Add("Xử lý thất bại");
                }
                if (isShowMessage)
                {
                    MessageManager.ShowAlert(Form.ActiveForm, param, success);
                }
                Inventec.Common.Logging.LogSystem.Info("param.GetMessage:" + param.GetMessage() + "____param.GetBugCode:" + param.GetBugCode());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn("Co loi xay ra. Du lieu dau vao:____" + Inventec.Common.Logging.LogUtil.TraceData("inputADO", inputADO));
                Inventec.Common.Logging.LogSystem.Error(ex);
                success = false;
            }
            return success;
        }

        private long GetMaxNumOrder(List<SignTDO> listSign)
        {
            long max = frmSignerAdd.stepNumOrder;
            if (listSign != null && listSign.Count > 0)
            {
                max = listSign.Max(o => o.NumOrder) + frmSignerAdd.stepNumOrder;
            }
            return max;
        }

        private bool CheckLogin(InputADO inputADO)
        {
            if (!String.IsNullOrEmpty(this.signToken.TokenCode) && !String.IsNullOrEmpty(this.signToken.LoginName))
            {
                GlobalStore.TokenData = new TokenData();
                GlobalStore.TokenData.TokenCode = this.signToken.TokenCode;
                GlobalStore.TokenData.User = new UserData();
                GlobalStore.TokenData.User.LoginName = this.signToken.LoginName;
                GlobalStore.TokenData.User.UserName = this.signToken.UserName;
                GlobalStore.TokenData.User.ApplicationCode = GlobalStore.appCode;

                GlobalStore.AcsConsumer.SetTokenCode(this.signToken.TokenCode);
                GlobalStore.EmrConsumer.SetTokenCode(this.signToken.TokenCode);
                GlobalStore.GetSetDicConsumer(signToken.TokenCode);

                this.signToken.TokenData = new TokenData();
                signToken.TokenData.User = new UserData();
                signToken.TokenData.User.LoginName = this.signToken.LoginName;
                signToken.TokenData.User.UserName = this.signToken.UserName;
                signToken.TokenData.User.ApplicationCode = GlobalStore.appCode;

                var signer = GlobalStore.GetByLoginName(signToken.LoginName);
                GlobalStore.Singer = signer;
                signToken.Singer = signer;
            }
            else if (!String.IsNullOrEmpty(GlobalStore.TokenCode))
            {
                GlobalStore.TokenData = new TokenData();
                GlobalStore.TokenData.TokenCode = GlobalStore.TokenCode;
                GlobalStore.TokenData.User = new UserData();
                GlobalStore.TokenData.User.LoginName = GlobalStore.LoginName;
                GlobalStore.TokenData.User.UserName = GlobalStore.UserName;
                GlobalStore.TokenData.User.ApplicationCode = GlobalStore.appCode;

                GlobalStore.AcsConsumer.SetTokenCode(GlobalStore.TokenData.TokenCode);
                GlobalStore.EmrConsumer.SetTokenCode(GlobalStore.TokenData.TokenCode);
                GlobalStore.GetSetDicConsumer(GlobalStore.TokenData.TokenCode);

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

            if (signToken.Singer != null)
            {
                return true;
            }
            else
            {
                if (inputADO.IsSign)
                {
                    return ValidSignerByLogin();
                }
                else if (this.signToken != null && !String.IsNullOrEmpty(this.signToken.TokenCode))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ValidSignerByLogin()
        {
            if (GlobalStore.TokenData != null && !String.IsNullOrEmpty(GlobalStore.TokenData.TokenCode))
            {
                var signer = GlobalStore.GetByLoginName(GlobalStore.LoginName);
                GlobalStore.Singer = signer;
                signToken.Singer = signer;
                if (GlobalStore.Singer != null)
                {
                    return true;
                }
                else
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show(
                    String.Format(MessageUitl.GetMessage(MessageConstan.TaiKhoanChuaDuocTaoNguoiKyTrenHeThongEMR), GlobalStore.LoginName),
                    MessageUitl.GetMessage(MessageConstan.ThongBao),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        RefeshAfterLogout();

                        frmLogin frmLogin = new frmLogin(ProcessSignTokenData);
                        frmLogin.ShowDialog();
                        return ValidSignerByLogin();
                    }
                }
            }

            return false;
        }

        private void ProcessSignTokenData(Inventec.Common.SignLibrary.ADO.SignToken _signToken)
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

        private void PrintNowProcess(string outputFile)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("PrintNowProcess.1" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile)
                    + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => GlobalStore.PrintUsingWaterMark), GlobalStore.PrintUsingWaterMark)
                    + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => GlobalStore.OptionPrintType), GlobalStore.OptionPrintType));
                if (GlobalStore.PrintUsingWaterMark)
                {
                    String temFileWaterMark = Utils.GenerateTempFileWithin();
                    var document = new EmrDocument().GetViewByCode(inputADOWorking.DocumentCode);
                    if (document == null)
                    {
                        document = new V_EMR_DOCUMENT() { DOCUMENT_CODE = inputADOWorking.DocumentCode, DOCUMENT_NAME = inputADOWorking.DocumentName, TREATMENT_CODE = inputADOWorking.Treatment != null ? inputADOWorking.Treatment.TREATMENT_CODE : null, CREATE_TIME = Utils.GetTimeNow() };
                    }
                    WaterMarkProcess.ProcessInsertWaterMark(outputFile, temFileWaterMark, document);
                    try
                    {
                        if (File.Exists(outputFile)) File.Delete(outputFile);
                    }
                    catch { }
                    outputFile = temFileWaterMark;
                    this.outputSignedFileResult = outputFile;
                }

                if (GlobalStore.OptionPrintType == OptionPrintType.PdfAposeLib)
                {
                    PrintLibProcess.ExecutePrintNowJob(outputFile, printNumberCopies, inputADOWorking.PrinterDefault, inputADOWorking.PaperSizeDefault);
                }
                else if (GlobalStore.OptionPrintType == OptionPrintType.CallExeLib && PrintLibProcess.ValidExistsExecutePrintCallExeService())
                {
                    PrintLibProcess.ExecutePrintCallExeService(outputFile, printNumberCopies, inputADOWorking.PrinterDefault, inputADOWorking.PaperSizeDefault);
                }
                else
                {
                    PrintLibProcess.SimplePrintNowDevLib(outputFile, printNumberCopies, inputADOWorking.PrinterDefault, inputADOWorking.PaperSizeDefault);
                    //this.currentPageSettings = PdfDocumentProcess.GetPaperSize(outputFile);

                    //DevExpress.XtraPdfViewer.PdfViewer pdfViewer1 = new DevExpress.XtraPdfViewer.PdfViewer();
                    //pdfViewer1.Name = "pdfViewer1";
                    //pdfViewer1.PageSetupDialogShowing += new DevExpress.XtraPdfViewer.PdfPageSetupDialogShowingEventHandler(pdfViewer1_PageSetupDialogShowing);
                    //pdfViewer1.DetachStreamAfterLoadComplete = true;
                    //pdfViewer1.LoadDocument(outputFile);
                    ////pdfViewer1.Print();
                    //var currentPageSettingData = pdfViewer1.GetPageSize(1);
                    //printerSettings = new System.Drawing.Printing.PrinterSettings();
                    //printerSettings.Copies = printNumberCopies;
                    ////printerSettings.DefaultPageSettings.PaperSize = this.currentPageSettings.PaperSize;                    
                    ////  printerSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", currentPageSettingData.PaperSize.Width, currentPageSettingData.PaperSize.Height);

                    //if (inputADOWorking != null && inputADOWorking.PaperSizeDefault != null)
                    //{
                    //    printerSettings.DefaultPageSettings.PaperSize = inputADOWorking.PaperSizeDefault;
                    //}
                    //else
                    //{
                    //    if ((Width_ == 582 && Height_ == 826) || ((int)(currentPageSettingData.Width * 100) == 582 && (int)(currentPageSettingData.Height * 100) == 826) || (Width_ == 826 && Height_ == 582) || ((int)(currentPageSettingData.Width * 100) == 826 && (int)(currentPageSettingData.Height * 100) == 582))
                    //    {
                    //        IEnumerable<PaperSize> paperSizes = printerSettings.PaperSizes.Cast<PaperSize>();
                    //        PaperSize sizes = paperSizes.FirstOrDefault<PaperSize>(size => size.Kind == PaperKind.A5);
                    //        printerSettings.DefaultPageSettings.PaperSize = sizes;
                    //    }
                    //    else if ((Width_ == 826 && Height_ == 1169) || (Width_ == 1169 && Height_ == 826) || ((int)(currentPageSettingData.Width * 100) == 826 && (int)(currentPageSettingData.Height * 100) == 1169) || ((int)(currentPageSettingData.Width * 100) == 1169 && (int)(currentPageSettingData.Height * 100) == 826))
                    //    {
                    //        IEnumerable<PaperSize> paperSizes = printerSettings.PaperSizes.Cast<PaperSize>();
                    //        PaperSize sizes = paperSizes.FirstOrDefault<PaperSize>(size => size.Kind == PaperKind.A4);
                    //        printerSettings.DefaultPageSettings.PaperSize = sizes;
                    //    }
                    //    else
                    //    {
                    //        printerSettings.DefaultPageSettings.PaperSize = this.currentPageSettings.PaperSize;
                    //    }
                    //}


                    //// Declare the PDF printer settings.
                    //// If required, pass the system settings to the PDF printer settings constructor.
                    //this.pdfPrinterSettings = new DevExpress.Pdf.PdfPrinterSettings(printerSettings);

                    //// Specify the PDF printer settings.
                    //this.pdfPrinterSettings.PageOrientation = this.currentPageSettings.Landscape ? DevExpress.Pdf.PdfPrintPageOrientation.Landscape : DevExpress.Pdf.PdfPrintPageOrientation.Portrait;
                    //this.pdfPrinterSettings.ScaleMode = DevExpress.Pdf.PdfPrintScaleMode.ActualSize;

                    //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile));
                    //pdfViewer1.QueryPageSettings += OnQueryPageSettings;

                    //pdfViewer1.Print(this.pdfPrinterSettings);
                    //pdfViewer1.QueryPageSettings -= OnQueryPageSettings;
                }
                Inventec.Common.Logging.LogSystem.Info("PrintNowProcess.2");
            }
            catch (Exception exx)
            {
                Logging.LogSystem.Warn(exx);
            }
        }

        private void PrintPreviewProcess(string outputFile)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("PrintPreviewProcess.1" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => outputFile), outputFile)
                    + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => GlobalStore.PrintUsingWaterMark), GlobalStore.PrintUsingWaterMark));
                if (GlobalStore.PrintUsingWaterMark)
                {
                    String temFileWaterMark = Utils.GenerateTempFileWithin();
                    var document = new EmrDocument().GetViewByCode(inputADOWorking.DocumentCode);
                    if (document == null)
                    {
                        document = new V_EMR_DOCUMENT() { DOCUMENT_CODE = inputADOWorking.DocumentCode, DOCUMENT_NAME = inputADOWorking.DocumentName, TREATMENT_CODE = inputADOWorking.Treatment != null ? inputADOWorking.Treatment.TREATMENT_CODE : null, CREATE_TIME = Utils.GetTimeNow() };
                    }
                    WaterMarkProcess.ProcessInsertWaterMark(outputFile, temFileWaterMark, document);
                    try
                    {
                        if (File.Exists(outputFile)) File.Delete(outputFile);
                    }
                    catch { }
                    outputFile = temFileWaterMark;
                    this.outputSignedFileResult = outputFile;
                }

                frmShowAndPrintNow frmShowAndPrintNow = new frmShowAndPrintNow(outputFile, this.inputADOWorking, printNumberCopies);
                frmShowAndPrintNow.ShowDialog();
            }
            catch (Exception exx)
            {
                Logging.LogSystem.Warn(exx);
            }
        }

        private void OnQueryPageSettings(object sender, DevExpress.Pdf.PdfQueryPageSettingsEventArgs e)
        {
            try
            {
                //'set current print page size to defined parameters  

                Width_ = (int)e.PageSize.Width;
                Height_ = (int)e.PageSize.Height;
                this.currentPageSettings.PaperSize.Width = (int)e.PageSize.Width;
                this.currentPageSettings.PaperSize.Height = (int)e.PageSize.Height;
                this.printerSettings.DefaultPageSettings.PaperSize = this.currentPageSettings.PaperSize;
                this.pdfPrinterSettings = new DevExpress.Pdf.PdfPrinterSettings(this.printerSettings);

                // Specify the PDF printer settings.
                this.pdfPrinterSettings.PageOrientation = this.currentPageSettings.Landscape ? DevExpress.Pdf.PdfPrintPageOrientation.Landscape : DevExpress.Pdf.PdfPrintPageOrientation.Portrait;
                this.pdfPrinterSettings.ScaleMode = DevExpress.Pdf.PdfPrintScaleMode.ActualSize;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void pdfViewer1_PageSetupDialogShowing(object sender, DevExpress.XtraPdfViewer.PdfPageSetupDialogShowingEventArgs e)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Debug("pdfViewer1_PageSetupDialogShowing.1____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => printNumberCopies), printNumberCopies));
                e.FormStartPosition = FormStartPosition.CenterScreen;
                int w = 600;
                int h = 400;
                if (Screen.PrimaryScreen != null)
                {
                    w = Screen.PrimaryScreen.WorkingArea.Width > 400 ? Screen.PrimaryScreen.WorkingArea.Width - 400 : 100;
                    h = Screen.PrimaryScreen.WorkingArea.Height > 100 ? Screen.PrimaryScreen.WorkingArea.Height - 100 : 50;
                }
                e.FormSize = new System.Drawing.Size(w, h);
                Inventec.Common.Logging.LogSystem.Debug("pdfViewer1_PageSetupDialogShowing.2");

                if (this.printNumberCopies > 1)
                {
                    e.PrinterSettings.Settings.Copies = this.printNumberCopies;
                    Inventec.Common.Logging.LogSystem.Debug("pdfViewer1_PageSetupDialogShowing.3");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ActionAfterSignedProcess(string outputFile)
        {
            try
            {
                if (String.IsNullOrEmpty(outputFile))
                    throw new ArgumentNullException("outputFile");

                this.outputSignedFileResult = outputFile;
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.outputSignedFileResult), this.outputSignedFileResult)
                    + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.iSPrintNow), this.iSPrintNow));
                if (this.iSPrintNow)
                {
                    PrintNowProcess(outputFile);
                }
                else if (this.iSPrintPreview)
                {
                    PrintPreviewProcess(outputFile);
                }
            }
            catch (Exception exx)
            {
                this.outputSignedFileResult = "";
                Logging.LogSystem.Warn(exx);
            }
        }

        private bool GetMultiSignDoc(InputADO inputADO, ref bool isSignParanel)
        {
            bool isMultiSign = false;
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

                inputADO.DocumentTypeCode = inputADO.DocumentTypeCode.Length == 1 ? "0" + inputADO.DocumentTypeCode : inputADO.DocumentTypeCode;
                bool isMultiSignByType = false;
                new EmrDocumentType().DocumentTypeProperty(inputADO.DocumentTypeCode, ref isMultiSignByType, ref isSignParanel);
                isMultiSign = isMultiSignByType;
                if (inputADO.IsMultiSign.HasValue)
                {
                    isMultiSign = isMultiSign && inputADO.IsMultiSign.Value;
                }
            }
            return isMultiSign;
        }

        private long? GetDocumentTypeId(string code)
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

        private DocumentTDO GenerateByDocumentCode(string documentCode, ref bool isMultiSign)
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
                documentTDO.AttachmentCount = doc.ATTACHMENT_COUNT;
                documentTDO.ParentDependentCode = doc.PARENT_DEPENDENT_CODE;
                isMultiSign = (doc.IS_MULTI_SIGN == 1);
            }
            catch
            {
                documentTDO = null;
            }

            return documentTDO;
        }

        private List<SignPositionADO> GetNextPositionSigned(bool isPatientSign, iTextSharp.text.pdf.PdfReader readerWorking, EMR.EFMODEL.DataModels.EMR_SIGN signSelected, ref int signedCount)
        {
            List<SignPositionADO> nextSignPositions = null;
            List<SignPositionADO> signPositionADOs = null;
            SignPositionADO nextSignPosition = null;

            try
            {
                bool hasNextSignPosition = false;
                if (isPatientSign)
                {
                    signPositionADOs = Utils.GetPdfPatientSignPosition(readerWorking);
                    if (signPositionADOs != null && signPositionADOs.Count > 0)
                    {
                        nextSignPosition = signPositionADOs.FirstOrDefault();
                        hasNextSignPosition = (nextSignPosition != null);
                        nextSignPositions = hasNextSignPosition ? signPositionADOs.Where(o => o.Text == nextSignPosition.Text).ToList() : null;
                    }
                    if (nextSignPositions != null && nextSignPositions.Count > 0)
                        return nextSignPositions;
                }
                signPositionADOs = Utils.GetPdfSignPosition(readerWorking);
                signedCount = Utils.GetSignedCount(readerWorking);
                if (signPositionADOs != null && signPositionADOs.Count > 0)
                {
                    signPositionADOs = signPositionADOs.OrderBy(o => VerifySign.GetNumOderByCommentText(o.Text)).ToList();

                    var positions = signPositionADOs.Where(o => VerifySign.GetNumOderByCommentText(o.Text) == VerifySign.GetNumOrderBySignOrDefault(signSelected, GetSignerData(), GetTreatmentData())).ToList();
                    nextSignPosition = (positions != null && positions.Count > 0) ? positions.FirstOrDefault() : null;
                    hasNextSignPosition = (nextSignPosition != null);
                    var nextpAutoSignPositionList = hasNextSignPosition ? signPositionADOs.Where(o => o.Text == nextSignPosition.Text).ToList() : null;
                    if (nextpAutoSignPositionList != null && nextpAutoSignPositionList.Count > 0)
                    {
                        nextSignPositions = new List<SignPositionADO>();
                        nextSignPositions.AddRange(nextpAutoSignPositionList);
                    }
                    Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("nextSignPositions", nextSignPositions) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => hasNextSignPosition), hasNextSignPosition));
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
            return nextSignPositions;
        }

        private void InitParam(string dti)
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

        private InputADO CopyInputADO(InputADO data)
        {
            InputADO result = new InputADO();
            try
            {
                result.BusinessCode = data.BusinessCode;
                result.DisplayConfigDTO = data.DisplayConfigDTO;
                result.DlgChoosePoint = data.DlgChoosePoint;
                result.DlgOpenModuleConfig = data.DlgOpenModuleConfig;
                result.DlgGetTreatment = data.DlgGetTreatment;
                result.DocumentCode = data.DocumentCode;
                result.DocumentName = data.DocumentName;
                result.DocumentTypeCode = data.DocumentTypeCode;
                result.DTI = data.DTI;
                result.HisCode = data.HisCode;
                result.HisUriUpdateSignedState = data.HisUriUpdateSignedState;
                result.IsExport = data.IsExport;
                result.IsMultiSign = data.IsMultiSign;
                result.IsPatientSign = data.IsPatientSign;
                result.IsPrint = data.IsPrint;
                result.IsPrintOnlyContent = data.IsPrintOnlyContent;
                result.IsReject = data.IsReject;
                result.IsSave = data.IsSave;
                result.IsSelectRangeRectangle = data.IsSelectRangeRectangle;
                result.IsSign = data.IsSign;
                result.IsUseTimespan = data.IsUseTimespan;
                result.RoomCode = data.RoomCode;
                result.RoomName = data.RoomName;
                result.RoomTypeCode = data.RoomTypeCode;
                result.SignReason = data.SignReason;
                result.SignType = data.SignType;
                result.Treatment = data.Treatment;
                result.Watermarks = data.Watermarks;
                result.SignerConfigs = data.SignerConfigs;
                result.IsAutoChooseBusiness = data.IsAutoChooseBusiness;
                result.PrintNumberCopies = data.PrintNumberCopies;
                result.PrintTypeBusinessCodes = data.PrintTypeBusinessCodes;
                result.MergeCode = data.MergeCode;
                result.DocumentTime = data.DocumentTime;
                result.DocumentGroupCode = data.DocumentGroupCode;
                result.DepartmentCode = data.DepartmentCode;
                result.DepartmentName = data.DepartmentName;
                result.DependentCode = data.DependentCode;
                result.ParentDependentCode = data.ParentDependentCode;
                result.PaperSizeDefault = data.PaperSizeDefault;
                result.PrinterDefault = data.PrinterDefault;
                result.IsEnableButtonPrint = data.IsEnableButtonPrint;

                result.DeviceSignPadName = data.DeviceSignPadName;
                result.ActSelectDevice = data.ActSelectDevice;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }

            return result;
        }

        private void ProcessMemoryUsageuser()
        {
            try
            {
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
                //// 1. Obtain the current application process
                //Process currentProcess = Process.GetCurrentProcess();
                //currentProcess.Refresh();
                //// 2. Obtain the used memory by the process
                //long memoryUsageusers = currentProcess.PrivateMemorySize64 / (1024 * 1024);

                //// 3. Display value in the terminal output
                ////Console.WriteLine(usedMemory);

                //long memoryUsageusersByGC = GC.GetTotalMemory(true) / (1024 * 1024);
                //Inventec.Common.Logging.LogSystem.Debug("ProcessMemoryUsageuser: Dung luong RAM phan mem dang su dung: " + memoryUsageusers + "Mbs|GC:" + memoryUsageusersByGC);
                //long memoryUsageusers = (GC.GetTotalMemory(true) - startBytes) / (1024 * 1024);
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }
    }
}
