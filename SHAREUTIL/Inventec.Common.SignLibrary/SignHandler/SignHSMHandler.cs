using EMR.EFMODEL.DataModels;
using EMR.SDO;
using EMR.TDO;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.Api;
using Inventec.Common.SignLibrary.FingerPrint;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
using Inventec.Common.SignLibrary.SignBoard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary.SignHandler
{
    internal class SignHSMHandler : BussinessBase
    {
        internal SignHSMHandler() : base() { }

        internal SignHSMHandler(CommonParam param, InputADO inputADOWorking) : base(param, inputADOWorking) { }
        internal SignHSMHandler(CommonParam param, InputADO inputADOWorking, string src) : base(param, inputADOWorking, src) { }
        internal SignHSMHandler(CommonParam param, InputADO inputADOWorking, string src, bool isSignParanel) : base(param, inputADOWorking, src, isSignParanel) { }
        internal SignHSMHandler(CommonParam param, InputADO inputADOWorking, string src, bool isSignParanel, EMR.EFMODEL.DataModels.EMR_TREATMENT treatment, EMR.EFMODEL.DataModels.EMR_SIGNER singer, string tokenCode) : base(param, inputADOWorking, src, isSignParanel, treatment, singer, tokenCode) { }

        internal bool SignWithCreateDoc(string outputFile, ref DocumentTDO document, string documentName, string treatmentCode, List<SignTDO> signStrategys, List<SignTDO> signTemps, string GetBase64OriginalFileData, string GetBase64HeaderFileData, PointSignTDO pointSignTDO, string signDescription, bool isPatientSignOrHomeRelativeSign, long? documentTypeId, bool isMultiSign, string hisCode, bool isCardAnonymous, ref Stream output, EMR_SIGN _signSelected, string mergeCode = "", double oginalHeight = 0, byte[] signedImageData = null)
        {
            bool success = false;

            //+ Nếu pm thẻ trả về báo thẻ bệnh nhân ký là thẻ định danh -> thực hiện gọi api Emr ký HSM như hiện tại.
            //+ Nếu pm thẻ trả về báo thẻ bệnh nhân ký là thẻ không định danh:
            //-> gọi api emr thực hiện đánh dấu bệnh nhân đã ký (lưu điểm ký và đánh dấu bệnh nhân ký điện tử (IS_SIGN_ELECTRONIC = 1) vào EMR_SIGN của bệnh nhân).
            //-> Khi hiển thị văn bản mà có bệnh nhân ký điện tử -> hiển thị vào điểm bệnh nhân ký 2 thông tin "Tên bệnh nhân - Đã ký".

            if (ValidSignBoard(signedImageData))
            {
                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 1");
                HsmSignCreateTDO hsmSignCreateTDO = new HsmSignCreateTDO();
                hsmSignCreateTDO.Signs = String.IsNullOrEmpty(inputADOWorking.BusinessCode) ? signTemps : null;
                if (String.IsNullOrEmpty(inputADOWorking.BusinessCode) && signStrategys != null && signStrategys.Count > 0)
                {
                    if (hsmSignCreateTDO.Signs == null)
                    {
                        hsmSignCreateTDO.Signs = new List<SignTDO>();
                    }
                    hsmSignCreateTDO.Signs.AddRange(signStrategys);
                }

                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 1.1");

                if (hsmSignCreateTDO.Signs != null)
                {
                    foreach (var tsn in hsmSignCreateTDO.Signs)
                    {
                        if (!String.IsNullOrEmpty(tsn.PatientCode))
                        {
                            tsn.SignedImageData = this.IsUsingSignPad ? this.SignPadImageData : signedImageData;
                            if (isPatientSignOrHomeRelativeSign)
                            {
                                tsn.Description = signDescription;
                            }
                        }
                        else if (tsn.Loginname == Signer.LOGINNAME)
                        {
                            tsn.Description = signDescription;
                        }
                    }
                }

                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 1.2");

                if (isCardAnonymous)
                {
                    hsmSignCreateTDO.IsSignElectronic = true;
                }

                hsmSignCreateTDO.DependentCode = inputADOWorking.DependentCode;
                hsmSignCreateTDO.ParentDependentCode = inputADOWorking.ParentDependentCode;
                hsmSignCreateTDO.RoomCode = inputADOWorking.RoomCode;
                hsmSignCreateTDO.RoomTypeCode = inputADOWorking.RoomTypeCode;
                hsmSignCreateTDO.WorkingDepartmentName = inputADOWorking.DepartmentName;
                hsmSignCreateTDO.IsSignParallel = this.IsSignParanel;
                hsmSignCreateTDO.MergeCode = mergeCode;
                hsmSignCreateTDO.Base64Header = GetBase64HeaderFileData;
                if (oginalHeight > 0)
                    hsmSignCreateTDO.OriginalHigh = (decimal)oginalHeight;
                hsmSignCreateTDO.PointSign = pointSignTDO;
                hsmSignCreateTDO.Description = signDescription;
                hsmSignCreateTDO.HisOrder = inputADOWorking.HisOrder;
                hsmSignCreateTDO.OriginalVersion = new VersionTDO();
                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 1.3");
                if (document != null && !String.IsNullOrEmpty(document.DocumentCode))
                {
                    hsmSignCreateTDO.DocumentCode = document.DocumentCode;
                    hsmSignCreateTDO.DocumentName = document.DocumentName;
                    if (document.DocumentTypeId.HasValue && document.DocumentTypeId.Value > 0)
                    {
                        hsmSignCreateTDO.DocumentTypeId = document.DocumentTypeId;
                    }
                    hsmSignCreateTDO.OriginalVersion.DocumentCode = document.DocumentCode;
                    hsmSignCreateTDO.HisCode = document.HisCode;
                }
                else
                {
                    hsmSignCreateTDO.DocumentName = (String.IsNullOrEmpty(documentName) ? ("Ký điện tử cho hồ sơ có mã " + treatmentCode + " ngày " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")) : documentName);
                    if (documentTypeId.HasValue && documentTypeId.Value > 0)
                    {
                        hsmSignCreateTDO.DocumentTypeId = documentTypeId;
                    }
                    hsmSignCreateTDO.HisCode = hisCode;
                    if (this.inputADOWorking.DocumentTime != null && this.inputADOWorking.DocumentTime.Value != DateTime.MinValue)
                    {
                        hsmSignCreateTDO.DocumentTime = DateTimeConvert.SystemDateTimeToTimeNumber(this.inputADOWorking.DocumentTime);
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 1.4");
                if (!String.IsNullOrEmpty(this.inputADOWorking.DocumentGroupCode))
                {
                    var docGroup = new EmrDocumentGroup().GetByCode(this.inputADOWorking.DocumentGroupCode);
                    hsmSignCreateTDO.DocumentGroupId = docGroup != null ? (long?)docGroup.ID : null;
                }
                hsmSignCreateTDO.IsFinishSign = !isMultiSign;
                hsmSignCreateTDO.IsSigning = isMultiSign;
                hsmSignCreateTDO.TreatmentCode = treatmentCode;
                if (document != null && document.OriginalVersion != null && !String.IsNullOrEmpty(document.OriginalVersion.Base64Data))
                {
                    hsmSignCreateTDO.OriginalVersion.Base64Data = document.OriginalVersion.Base64Data;
                    hsmSignCreateTDO.OriginalVersion.Base64DataJson = document.OriginalVersion.Base64DataJson;
                    hsmSignCreateTDO.OriginalVersion.Base64DataXml = document.OriginalVersion.Base64DataXml;
                }
                else
                    hsmSignCreateTDO.OriginalVersion.Base64Data = GetBase64OriginalFileData;

                hsmSignCreateTDO.BusinessCode = inputADOWorking.BusinessCode;
                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 1.5");
                if (this.FileType == SignLibrary.FileType.Xml)
                {
                    hsmSignCreateTDO.FileType = EMR.TDO.FileType.XML;
                }
                else if (this.FileType == SignLibrary.FileType.Json)
                {
                    hsmSignCreateTDO.FileType = EMR.TDO.FileType.JSON;
                }
                else
                {
                    hsmSignCreateTDO.FileType = EMR.TDO.FileType.PDF;
                }
                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 1.6");
                if (inputADOWorking.PaperSizeDefault != null)
                {
                    hsmSignCreateTDO.PaperName = inputADOWorking.PaperSizeDefault.PaperName;
                    if (String.IsNullOrEmpty(hsmSignCreateTDO.PaperName))
                    {
                        hsmSignCreateTDO.PaperName = this.inputADOWorking.PaperSizeDefault.Kind.ToString();
                    }
                    hsmSignCreateTDO.Width = inputADOWorking.PaperSizeDefault.Width;
                    hsmSignCreateTDO.Height = inputADOWorking.PaperSizeDefault.Height;
                    hsmSignCreateTDO.RawKind = inputADOWorking.PaperSizeDefault.RawKind;
                }
                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 1.7");
                if (!VerifyDataPreCallApi(hsmSignCreateTDO))
                {
                    param.Messages.Add(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    MessageManager.Show(param, false);
                    return false;
                }
                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 2");
                CommonParam paramCreate = new CommonParam();
                var rs = new EmrDocument(paramCreate).CreateAndSignHsm(TokenCode, hsmSignCreateTDO);
                if (rs != null)
                {
                    Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 3");
                    success = true;
                    document.DocumentCode = rs.DocumentCode;
                    document.DocumentName = rs.DocumentName;
                    document.DocumentTypeId = rs.DocumentTypeId;
                    document.MergeCode = rs.MergeCode;
                    document.TreatmentCode = rs.TreatmentCode;
                    document.DependentCode = rs.DependentCode;
                    document.ParentDependentCode = rs.ParentDependentCode;
                    document.OriginalVersion = rs.OriginalVersion;

                    document.PaperName = rs.PaperName;
                    document.Width = rs.Width;
                    document.Height = rs.Height;
                    document.RawKind = rs.RawKind;

                    if (rs.Signs != null && rs.Signs.Count > 0)
                    {
                        foreach (var sn in rs.Signs)
                        {
                            if (sn != null && sn.Version != null && !String.IsNullOrEmpty(sn.Version.Url))
                            {
                                output = FssFileDownload.GetFile(sn.Version.Url);
                                Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 4. output.Length =" + output.Length);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (paramCreate.Messages != null && paramCreate.Messages.Count > 0)
                        this.param.Messages.AddRange(paramCreate.Messages);
                    if (paramCreate.BugCodes != null && paramCreate.BugCodes.Count > 0)
                        this.param.BugCodes.AddRange(paramCreate.BugCodes);
                    Inventec.Common.Logging.LogSystem.Info("SignWithCreateDoc => 5" + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rs), rs) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => param), param)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isCardAnonymous), isCardAnonymous)
                        );
                }
            }
            return success;
        }

        internal bool SignOnly(ref DocumentTDO document, List<SignTDO> signStrategys, List<SignTDO> signTemps, PointSignTDO pointSignTDO, string signDescription, bool isPatientSignOrHomeRelativeSign, bool isMultiSign, string cmnd, string cardCode, string serviceCode, bool isCardAnonymous, long? relationId, string relationName, string relationPeopleName, ref Stream outputStream, EMR_SIGN _signSelected, string mergeCode = "", string linkCode = "", byte[] signedImageData = null)
        {
            bool success = false;
            try
            {
                if (ValidSignBoard(signedImageData))
                {
                    EmrSignHsmSDO emrSignHsmSDO = new EmrSignHsmSDO();

                    var doc = new EmrDocument().GetViewByCode(document.DocumentCode);
                    emrSignHsmSDO.EmrDocumentId = doc.ID;
                    emrSignHsmSDO.CmndNumber = cmnd;
                    emrSignHsmSDO.CardCode = cardCode;
                    emrSignHsmSDO.ServiceCode = serviceCode;
                    emrSignHsmSDO.IsFinishSign = !isMultiSign;
                    emrSignHsmSDO.IsSigning = isMultiSign;
                    //emrSignHsmSDO.MergeCode = mergeCode;
                    if (isCardAnonymous)
                    {
                        emrSignHsmSDO.IsSignElectronic = true;
                    }
                    emrSignHsmSDO.RelationName = relationName;
                    emrSignHsmSDO.RelationPeopleName = relationPeopleName;
                    emrSignHsmSDO.RelationId = relationId;
                    emrSignHsmSDO.LinkCode = linkCode;
                    var sign = _signSelected != null ? _signSelected : new EmrSign().GetSignDocumentFirst(doc, (isPatientSignOrHomeRelativeSign ? null : Signer), Treatment, true);
                    if (sign != null && sign.ID > 0)
                        emrSignHsmSDO.EmrSignId = sign.ID;
                    emrSignHsmSDO.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    emrSignHsmSDO.PointSign = new EmrPointSignSDO()
                    {
                        CoorXRectangle = pointSignTDO.CoorXRectangle,
                        CoorYRectangle = pointSignTDO.CoorYRectangle,
                        MaxPageNumber = pointSignTDO.MaxPageNumber,
                        PageNumber = pointSignTDO.PageNumber,
                        SizeFont = pointSignTDO.SizeFont,
                        TextPosition = pointSignTDO.TextPosition,
                        HeightRectangle = pointSignTDO.HeightRectangle,
                        WidthRectangle = pointSignTDO.WidthRectangle,
                        TypeDisplay = pointSignTDO.TypeDisplay,
                        FormatRectangleText = pointSignTDO.FormatRectangleText,
                        Alignment = pointSignTDO.Alignment,
                        IsBold = pointSignTDO.IsBold,
                        IsItalic = pointSignTDO.IsItalic,
                        IsUnderlined = pointSignTDO.IsUnderlined,
                        FontName = pointSignTDO.FontName
                    };
                    emrSignHsmSDO.Description = signDescription;
                    emrSignHsmSDO.RoomCode = inputADOWorking.RoomCode;
                    emrSignHsmSDO.RoomTypeCode = inputADOWorking.RoomTypeCode;
                    //emrSignHsmSDO.DependentCode = inputADOWorking.DependentCode;
                    //emrSignHsmSDO.ParentDependentCode = inputADOWorking.ParentDependentCode;
                    emrSignHsmSDO.WorkingDepartmentName = inputADOWorking.DepartmentName;
                    emrSignHsmSDO.SignedImageData = this.IsUsingSignPad ? this.SignPadImageData : signedImageData;

                    CommonParam paramCreate = new CommonParam();

                    Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => emrSignHsmSDO), emrSignHsmSDO));
                    var rs = new EmrDocument(paramCreate).SignHsm(TokenCode, emrSignHsmSDO);
                    if (rs != null)
                    {
                        success = true;
                        if (rs.EmrVersion != null && rs.EmrSign != null)
                        {
                            if (!String.IsNullOrEmpty(rs.EmrVersion.URL))
                            {
                                outputStream = FssFileDownload.GetFile(rs.EmrVersion.URL);
                            }
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => emrSignHsmSDO), emrSignHsmSDO));
                        if (paramCreate.Messages != null && paramCreate.Messages.Count > 0)
                            this.param.Messages.AddRange(paramCreate.Messages);
                        if (paramCreate.BugCodes != null && paramCreate.BugCodes.Count > 0)
                            this.param.BugCodes.AddRange(paramCreate.BugCodes);
                    }
                }
            }
            catch (Exception ex)
            {
                param.Messages.Add(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
                success = false;
            }

            return success;
        }

        internal bool PatientOrHomeRelativeSignOnly(ref DocumentTDO document, List<SignTDO> signStrategys, List<SignTDO> signTemps, PointSignTDO pointSignTDO, string signDescription, bool isPatientSign, bool isHomeRelativeSign, bool isMultiSign, string cmnd, string cardCode, string serviceCode, bool isCardAnonymous, long? relationId, string relationName, string relationPeopleName, ref Stream outputStream, EMR_SIGN _signSelected, string mergeCode = "", string linkCode = "", byte[] signedImageData = null,bool IsHasBusinessCode = false)
        {
            bool success = false;
            try
            {
                if (ValidSignBoard(signedImageData))
                {
                    EmrSignHsmSDO emrSignHsmSDO = new EmrSignHsmSDO();

                    var doc = new EmrDocument().GetViewByCode(document.DocumentCode);
                    emrSignHsmSDO.EmrDocumentId = doc.ID;
                    emrSignHsmSDO.CmndNumber = cmnd;
                    emrSignHsmSDO.CardCode = cardCode;
                    emrSignHsmSDO.ServiceCode = serviceCode;
                    emrSignHsmSDO.IsFinishSign = !isMultiSign;
                    emrSignHsmSDO.IsSigning = isMultiSign;
                    //emrSignHsmSDO.MergeCode = mergeCode;
                    if (isCardAnonymous)
                    {
                        emrSignHsmSDO.IsSignElectronic = true;
                    }
                    emrSignHsmSDO.RelationName = relationName;
                    emrSignHsmSDO.RelationPeopleName = relationPeopleName;
                    emrSignHsmSDO.RelationId = relationId;
                    emrSignHsmSDO.LinkCode = linkCode;
                    var sign = _signSelected != null ? _signSelected : new EmrSign().GetSignDocumentFirst(doc, (isPatientSign || isHomeRelativeSign) ? null : Signer, Treatment, true);
                    if (sign != null && sign.ID > 0 && ((_signSelected == null && !IsHasBusinessCode) || (_signSelected != null && IsHasBusinessCode)))
                        emrSignHsmSDO.EmrSignId = sign.ID;
                    emrSignHsmSDO.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    emrSignHsmSDO.PointSign = new EmrPointSignSDO()
                    {
                        CoorXRectangle = pointSignTDO.CoorXRectangle,
                        CoorYRectangle = pointSignTDO.CoorYRectangle,
                        MaxPageNumber = pointSignTDO.MaxPageNumber,
                        PageNumber = pointSignTDO.PageNumber,
                        SizeFont = pointSignTDO.SizeFont,
                        TextPosition = pointSignTDO.TextPosition,
                        HeightRectangle = pointSignTDO.HeightRectangle,
                        WidthRectangle = pointSignTDO.WidthRectangle,
                        TypeDisplay = pointSignTDO.TypeDisplay,
                        FormatRectangleText = pointSignTDO.FormatRectangleText,
                        Alignment = pointSignTDO.Alignment,
                        IsBold = pointSignTDO.IsBold,
                        IsItalic = pointSignTDO.IsItalic,
                        IsUnderlined = pointSignTDO.IsUnderlined,
                        FontName = pointSignTDO.FontName
                    };
                    emrSignHsmSDO.Description = signDescription;
                    emrSignHsmSDO.RoomCode = inputADOWorking.RoomCode;
                    emrSignHsmSDO.RoomTypeCode = inputADOWorking.RoomTypeCode;
                    //emrSignHsmSDO.DependentCode = inputADOWorking.DependentCode;
                    //emrSignHsmSDO.ParentDependentCode = inputADOWorking.ParentDependentCode;
                    emrSignHsmSDO.WorkingDepartmentName = inputADOWorking.DepartmentName;
                    emrSignHsmSDO.SignedImageData = this.IsUsingSignPad ? this.SignPadImageData : signedImageData;

                    CommonParam paramCreate = new CommonParam();
                    var rs = new EmrDocument(paramCreate).PatientOrHomeRelativeSignHsm(TokenCode, emrSignHsmSDO);
                    if (rs != null)
                    {
                        success = true;
                        if (rs.EmrVersion != null && rs.EmrSign != null)
                        {
                            if (!String.IsNullOrEmpty(rs.EmrVersion.URL))
                            {
                                outputStream = FssFileDownload.GetFile(rs.EmrVersion.URL);
                            }
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => emrSignHsmSDO), emrSignHsmSDO));
                        if (paramCreate.Messages != null && paramCreate.Messages.Count > 0)
                            this.param.Messages.AddRange(paramCreate.Messages);
                        if (paramCreate.BugCodes != null && paramCreate.BugCodes.Count > 0)
                            this.param.BugCodes.AddRange(paramCreate.BugCodes);
                    }
                }
            }
            catch (Exception ex)
            {
                param.Messages.Add(MessageUitl.GetMessage(MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro));
                Inventec.Common.Logging.LogSystem.Warn(ex);
                success = false;
            }

            return success;
        }

        private bool ValidSignBoard(byte[] signedImageData)
        {
            bool valid = true;
            try
            {
                if (this.IsUsingSignPad && (GlobalStore.EMR_SIGN_BOARD__OPTION == "2"))
                {
                    if (IsUsingSignPadBefore && signedImageData != null && signedImageData.Length > 0)
                    {
                        SignPadImageData = signedImageData;
                        return valid;
                    }
                    IFingerPrint behavior = FingerPrintFactory.MakeISignBoard(param, inputADOWorking, SignBoardOption.Use);
                    SignPadImageData = behavior != null ? (behavior.Run()) : null;
                    Utils.SignPadImageData = SignPadImageData;

                    //byte[] imageBytes = File.ReadAllBytes("D:\\MyFile2.Png");

                    //frmConfigFingerPrint frm = new frmConfigFingerPrint(UpdateSignerPad);
                    //frm.ShowDialog();

                    //SignPadImageData = imageBytes;

                    if (SignPadImageData == null || SignPadImageData.Length == 0)
                    {
                        this.param.Messages.Add(MessageUitl.GetMessage(MessageConstan.KhongTimThayChuKyKhiSuDungBangKy));
                        valid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.param.Messages.Add(MessageUitl.GetMessage(MessageConstan.TinhNangKySuDungBangKyChuaDuocHoTro));
                valid = false;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return valid;
        }

        private void UpdateSignerPad(Byte[] _image)
        {
            this.SignPadImageData = _image;
        }

        internal void SetSigmImageData(byte[] bmpSignImage)
        {
            this.SignPadImageData = bmpSignImage;
        }

        internal void SetUsingSignPadData(bool isUsingSignPad)
        {
            this.IsUsingSignPad = isUsingSignPad;
        }
        internal void SetSignPadBefore(bool IsUsingSignPadBefore)
        {
            this.IsUsingSignPadBefore = IsUsingSignPadBefore;
        }
        internal void SetFileType(FileType _fileType)
        {
            this.FileType = _fileType;
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
