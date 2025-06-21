using EMR.EFMODEL.DataModels;
using EMR.Filter;
using EMR.TDO;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.Api;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary
{
    class Verify
    {
        const string SplitTagPatientCode = "#@!@#";
        const int IS_HAS_ONE = 1;

        internal static bool VerifyHisCode(InputADO inputADO, bool isShowSignedFile, ref string base64FileSigned, ref V_EMR_DOCUMENT documentData)
        {
            return VerifyHisCode(inputADO, isShowSignedFile, false, ref base64FileSigned, ref documentData);
        }

        /// <summary>
        /// Vướng mắc hiện tại: 1 văn bản (trên HIS) nsd không nhớ rõ đã đẩy ký trên EMR hay chưa --> phải vào EMR để tìm rất mất thời gian. Có tình huống vô tình đẩy lại 1 văn bản đã đẩy ký dẫn đến mọi người phải xử lý (ký) thừa.
        ///Giải pháp: cảnh báo khi nsd bấm nút "EMR" trên các tool print preview.
        ///Cụ thể: API dll cung cấp cho tool print preview bổ sung thêm trường HIS_CODE (mã định danh trên HIS). Ví dụ mã này chính là mã đơn thuốc, mã phiếu chỉ định... HIS có thể truyền giá trị vào trường này hoặc không.
        ///EMR (tool view) sẽ xử lý như sau:
        ///1 - Đầu tiên kiểm tra loại văn bản mà HIS gọi truyền là gì. Nếu loại đó có giá trị emr-document-type.is-has-one = 1 thì kiểm tra theo treatment, đã tồn tại văn bản nào thuộc loại đó hay chưa --> nếu đã tồn tại thì cảnh báo. Những loại văn bản thuộc dạng này như: Vỏ bệnh án, Phiếu khám bệnh vào viện, Giấy chuyển viện, Giấy hẹn khám...
        ///2 - Nếu không phải trường hợp 1, mà HIS truyền vào HIS_CODE. Thì kiểm tra với HIS_CODE đó đã tồn tại hay chưa, nếu tồn tại rồi thì cảnh báo. Trường hợp nếu HIS truyền cả DOCUMENT_TYPE_CODE thì kiểm tra HIS_CODE trong DOCUMENT_TYPE_CODE mà thôi vì nhiều loại văn bản khác nhau có thể trùng mã là bình thường.
        ///Lưu ý: việc tìm kiếm cần bỏ qua các văn bản đã bị "Xóa" trên EMR.
        ///Câu cảnh báo: "Văn bản có thể đã tồn tại trên hệ thống EMR. Bạn có chắc chắn thực hiện không?" --> Có - Không.
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="hisCode"></param>
        /// <returns></returns>
        internal static bool VerifyHisCode(InputADO inputADO, bool isShowSignedFile, bool isValidExistsDoc, ref string base64FileSigned, ref V_EMR_DOCUMENT documentData)
        {
            bool isExistsDoc = false;
            long documentSignId = 0;
            string mergeCode = "";
            bool isAllowDuplicateHisCode = false;
            if (!String.IsNullOrEmpty(inputADO.DocumentTypeCode))
            {
                var docType = new EmrDocumentType().GetByCode(inputADO.DocumentTypeCode);
                isAllowDuplicateHisCode = docType != null ? docType.IS_ALLOW_DUPLICATE_HIS_CODE == 1 : false;
                if (docType != null && docType.IS_HAS_ONE == IS_HAS_ONE && !String.IsNullOrEmpty(inputADO.Treatment.TREATMENT_CODE))
                {
                    var docWithTypes = new EmrDocument().GetView(new EmrDocumentViewFilter() { TREATMENT_CODE__EXACT = inputADO.Treatment.TREATMENT_CODE, DOCUMENT_TYPE_CODE__EXACT = inputADO.DocumentTypeCode, IS_DELETE = false }, new Common.Integrate.CommonParam());
                    isExistsDoc = (docWithTypes != null && docWithTypes.Count > 0);
                    if (isExistsDoc)
                    {
                        documentSignId = docWithTypes[0].ID;
                        mergeCode = docWithTypes[0].MERGE_CODE;
                        documentData = docWithTypes[0];
                    }
                }
            }

            if (documentSignId == 0 && !String.IsNullOrEmpty(inputADO.HisCode))
            {
                var docWithHisCodes = new EmrDocument().GetView(new EmrDocumentViewFilter()
                {
                    TREATMENT_CODE__EXACT = inputADO.Treatment.TREATMENT_CODE,
                    //HIS_CODE__EXACT = inputADO.HisCode, 
                    DOCUMENT_TYPE_CODE__EXACT = inputADO.DocumentTypeCode,
                    ORDER_FIELD = "ID",
                    ORDER_DIRECTION = "DESC",
                    IS_DELETE = false
                }, new Common.Integrate.CommonParam());
                docWithHisCodes = docWithHisCodes != null ? docWithHisCodes.Where(o => o.HIS_CODE == inputADO.HisCode).ToList() : null;
                isExistsDoc = (docWithHisCodes != null && docWithHisCodes.Count > 0);
                if (isExistsDoc)
                {
                    documentSignId = docWithHisCodes[0].ID;
                    mergeCode = docWithHisCodes[0].MERGE_CODE;
                    documentData = docWithHisCodes[0];
                }

                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => docWithHisCodes), docWithHisCodes));
            }
            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignId), documentSignId));
            if (isExistsDoc)
            {
                if (isAllowDuplicateHisCode)
                {
                    isShowSignedFile = true;
                }
                else
                {
                    //#39443 
                    //2. Sửa chức năng ký Văn bản điển tử:
                    //- Khi mở một văn bản mới kiểm tra HIS_CODE của văn bản mới đã tồn tại trên EMR chưa. Nếu đã tồn tại thì kiểm tra key cấu hình EMR.EMR_DOCUMENT.DULICATE_HIS_CODE.WARNING_OPTION
                    //- Nếu cấu hình có giá trị 0: thì cảnh báo như hiện tại "Văn bản đã tồn tại trên hệ thống EMR. Bạn có muốn tiếp tục không? => Có thì mở văn bản mới, Không thì mở văn bản cũ (đã có trên EMR).
                    //- Nếu cấu hình có giá trị 1: Thì mở văn bản cũ lên và hiển thị cảnh báo "Vản bản đã tồn tại, phần mềm sẽ hiển thị văn bản cũ".
                    //- Nếu cấu hình có giá trị 2: Văn bản đã ký hoặc đã thiết lập ký thì mới ko cho tạo lại. Còn Chỉ tạo văn bản mà không có thiết lập ký hay hủy thì cho tạo bình thường.
                    var configs = GlobalStore.EmrConfigs;
                    if (configs != null && configs.Count > 0)
                    {
                        var cfgSignDisplayOptions = configs.Where(o => o.KEY == EmrConfigKeys.EMR__EMR_DOCUMENT__DULICATE_HIS_CODE__WARNING_OPTION);
                        var cfgSignDisplayOption = cfgSignDisplayOptions != null ? cfgSignDisplayOptions.FirstOrDefault() : null;
                        if (cfgSignDisplayOption != null)
                        {
                            string vlOption = !String.IsNullOrEmpty(cfgSignDisplayOption.VALUE) ? cfgSignDisplayOption.VALUE : cfgSignDisplayOption.DEFAULT_VALUE;
                            if (vlOption == "1" && documentSignId > 0)
                            {
                                isShowSignedFile = false;
                                if (isValidExistsDoc)
                                {
                                    Inventec.Common.Logging.LogSystem.Info(MessageUitl.GetMessage(MessageConstan.VanBanDaTonTaiKhongTheKyTiepDoThietLapDangChan) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isValidExistsDoc), isValidExistsDoc));
                                    DevExpress.XtraEditors.XtraMessageBox.Show(MessageUitl.GetMessage(MessageConstan.VanBanDaTonTaiKhongTheKyTiepDoThietLapDangChan), MessageUitl.GetMessage(MessageConstan.ThongBao));
                                    documentData = null;
                                    return false;
                                }
                                else
                                {
                                    Inventec.Common.Logging.LogSystem.Info(MessageUitl.GetMessage(MessageConstan.VanBanDaTonTaiPhanMemSeHienThiVanBanCu) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isShowSignedFile), isShowSignedFile));
                                    DevExpress.XtraEditors.XtraMessageBox.Show(MessageUitl.GetMessage(MessageConstan.VanBanDaTonTaiPhanMemSeHienThiVanBanCu), MessageUitl.GetMessage(MessageConstan.ThongBao));
                                }
                            }
                            else if (vlOption == "2" && documentSignId > 0)
                            {
                                //TODO
                                EmrSign emrSign = new EmrSign();
                                EmrSignFilter filter = new EmrSignFilter();
                                filter.DOCUMENT_ID = documentSignId;
                                var signExists = emrSign.Get(filter);

                                if (signExists != null && signExists.Count > 0 && signExists.Exists(o => o.REJECT_TIME == null))
                                {
                                    isShowSignedFile = false;
                                    Inventec.Common.Logging.LogSystem.Info("TH có key cau hinh " + EmrConfigKeys.EMR__EMR_DOCUMENT__DULICATE_HIS_CODE__WARNING_OPTION + " gia tri = " + vlOption + " " + MessageUitl.GetMessage(MessageConstan.VanBanDaTonTaiPhanMemSeHienThiVanBanCu)
                                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isShowSignedFile), isShowSignedFile)
                                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => signExists), signExists)
                                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isAllowDuplicateHisCode), isAllowDuplicateHisCode)
                                        );
                                    DevExpress.XtraEditors.XtraMessageBox.Show(MessageUitl.GetMessage(MessageConstan.VanBanDaTonTaiPhanMemSeHienThiVanBanCu), MessageUitl.GetMessage(MessageConstan.ThongBao));

                                }
                                else
                                {
                                    Inventec.Common.Logging.LogSystem.Info("TH có key cau hinh " + EmrConfigKeys.EMR__EMR_DOCUMENT__DULICATE_HIS_CODE__WARNING_OPTION + " gia tri = " + vlOption + " , nhung khong tim thay emr_sign nao thoa man: \"Văn bản đã ký hoặc đã thiết lập ký\"____"
                                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => signExists), signExists)
                                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isAllowDuplicateHisCode), isAllowDuplicateHisCode)
                                        );
                                }
                            }
                        }
                    }
                }

                if (isShowSignedFile)
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show(
                       MessageUitl.GetMessage(MessageConstan.VanBanCoTheDaTonTaiTrenHeThongEMRBanCoThucHienKhong),
                       MessageUitl.GetMessage(MessageConstan.ThongBao),
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        Inventec.Common.Logging.LogSystem.Info("Ton tai van ban da co tren he thong cua ho so & loai van ban ==> show thong bao co muon thuc hien ==> chon khong muon thuc hien ==> he thong khong tim duoc file da ky truoc do de view ");
                        return false;
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Ton tai van ban da co tren he thong cua ho so & loai van ban ==> show thong bao co muon thuc hien ==> chon co muon thuc hien tiep ==> mo xem ky nhu binh thuong");
                        documentData = null;
                        return true;
                    }
                }
                string urlLastVersion = "";
                if (documentData != null && !String.IsNullOrEmpty(documentData.LAST_VERSION_URL))
                {
                    urlLastVersion = documentData.LAST_VERSION_URL;
                }
                else
                {
                    var lastVersionSigned = documentSignId > 0 ? new EmrVersion().GetSignedDocumentLast(documentSignId) : null;
                    if (lastVersionSigned != null)
                    {
                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignId), documentSignId) + "" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => lastVersionSigned.URL), lastVersionSigned.URL));
                        urlLastVersion = lastVersionSigned.URL;
                    }
                }
                var outputStream = Inventec.Common.Integrate.FssFileDownload.GetFile(urlLastVersion);
                if (outputStream != null && outputStream.Length > 0)
                {
                    outputStream.Position = 0;
                    string f64 = "";
                    if (!String.IsNullOrEmpty(mergeCode))
                    {
                        string fileMerge = new DocumentManager().GetFileDocumentMerge(0, inputADO.Treatment.TREATMENT_CODE, mergeCode);
                        if (!String.IsNullOrEmpty(fileMerge))
                        {
                            f64 = Convert.ToBase64String(Utils.FileToByte(fileMerge));
                        }
                    }

                    if (String.IsNullOrEmpty(f64))
                    {
                        f64 = Convert.ToBase64String(Utils.StreamToByte(outputStream));
                    }
                    if (!String.IsNullOrEmpty(f64))
                    {
                        base64FileSigned = f64;
                    }
                    else
                    {
                        documentData = null;
                    }
                    Inventec.Common.Logging.LogSystem.Info("Ton tai van ban da co tren he thong cua ho so & loai van ban ==> show thong bao co muon thuc hien ==> chon khong muon thuc hien ==> he thong tim duoc file da ky truoc do cua ho so de show len voi che do chi xem");
                    return true;
                }
            }
            else
            {
                Inventec.Common.Logging.LogSystem.Debug("Khong Ton tai van ban da co tren he thong cua ho so & loai van ban");
            }

            return true;
        }

        internal static bool VerifyHisCode(InputADO inputADO, bool isShowSignedFile, ref byte[] inputByte)
        {
            bool isExistsDoc = false;
            long documentSignId = 0;
            if (!String.IsNullOrEmpty(inputADO.DocumentTypeCode))
            {
                var docType = new EmrDocumentType().GetByCode(inputADO.DocumentTypeCode);
                if (docType != null && docType.IS_HAS_ONE == IS_HAS_ONE)
                {
                    var docWithTypes = new EmrDocument().GetView(new EmrDocumentViewFilter() { TREATMENT_CODE__EXACT = inputADO.Treatment.TREATMENT_CODE, DOCUMENT_TYPE_CODE__EXACT = inputADO.DocumentTypeCode }, new Common.Integrate.CommonParam());
                    isExistsDoc = (docWithTypes != null && docWithTypes.Count > 0);
                    if (isExistsDoc)
                    {
                        documentSignId = docWithTypes[0].ID;
                    }
                }
            }

            if (documentSignId == 0 && !String.IsNullOrEmpty(inputADO.HisCode))
            {
                var docWithHisCodes = new EmrDocument().GetView(new EmrDocumentViewFilter()
                {
                    TREATMENT_CODE__EXACT = inputADO.Treatment.TREATMENT_CODE,
                    //HIS_CODE__EXACT = inputADO.HisCode, 
                    DOCUMENT_TYPE_CODE__EXACT = (!String.IsNullOrEmpty(inputADO.DocumentTypeCode) && inputADO.DocumentTypeCode.Length == 2 ? inputADO.DocumentTypeCode : ""),
                    ORDER_FIELD = "ID",
                    ORDER_DIRECTION = "DESC"
                },
                    new Common.Integrate.CommonParam());
                docWithHisCodes = docWithHisCodes != null ? docWithHisCodes.Where(o => o.HIS_CODE == inputADO.HisCode).ToList() : null;
                isExistsDoc = (docWithHisCodes != null && docWithHisCodes.Count > 0);
                if (isExistsDoc)
                {
                    documentSignId = docWithHisCodes[0].ID;
                }
            }

            if (isExistsDoc)
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show(
                       MessageUitl.GetMessage(MessageConstan.VanBanCoTheDaTonTaiTrenHeThongEMRBanCoThucHienKhong),
                       MessageUitl.GetMessage(MessageConstan.ThongBao),
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isShowSignedFile), isShowSignedFile));
                    if (isShowSignedFile)
                    {
                        var sign = new EmrVersion().GetSignedDocumentLast(documentSignId);
                        if (sign != null)
                        {
                            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => documentSignId), documentSignId) + "" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sign.URL), sign.URL));
                            var outputStream = Inventec.Common.Integrate.FssFileDownload.GetFile(sign.URL);
                            if (outputStream != null && outputStream.Length > 0)
                            {
                                outputStream.Position = 0;
                                var bOut = Utils.StreamToByte(outputStream);
                                if (bOut != null && bOut.Length > 0)
                                {
                                    inputByte = bOut;

                                    Inventec.Common.Logging.LogSystem.Info("Ton tai van ban da co tren he thong cua ho so & loai van ban ==> show thong bao co muon thuc hien ==> chon khong muon thuc hien ==> he thong tim duoc file da ky truoc do cua ho so de show len voi che do chi xem");
                                    return true;
                                }
                            }
                        }
                    }

                    Inventec.Common.Logging.LogSystem.Info("Ton tai van ban da co tren he thong cua ho so & loai van ban ==> show thong bao co muon thuc hien ==> chon khong muon thuc hien ==> he thong khong tim duoc file da ky truoc do de view");
                    return false;
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("Ton tai van ban da co tren he thong cua ho so & loai van ban ==> show thong bao co muon thuc hien ==> chon co muon thuc hien tiep ==> mo xem ky nhu binh thuong");
                }
            }

            return true;
        }

        internal static bool VerifyTreatmentCode(InputADO inputADO, ref Inventec.Common.SignLibrary.ADO.SignToken signToken)
        {
            bool isValid = false;
            try
            {
                if (!String.IsNullOrEmpty(inputADO.Treatment.TREATMENT_CODE))
                {
                    if (signToken == null)
                        signToken = new SignToken();

                    signToken.Treatment = new EmrTreatment().GetByCode(inputADO.Treatment.TREATMENT_CODE);
                    isValid = (signToken.Treatment != null && signToken.Treatment.ID > 0);
                }

                if (!isValid)
                {
                    MessageManager.Show(String.Format(MessageUitl.GetMessage(MessageConstan.MaHoSoDieuTriKhongHopLe), inputADO.Treatment.TREATMENT_CODE));
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return isValid;
        }

        internal static bool VerifySigner(List<SignTDO> signs, DocumentTDO document, EMR.EFMODEL.DataModels.EMR_TREATMENT treatment, ref string err, bool isPatientOrHomeRelativeSign = false)
        {
            bool isValid = true;
            try
            {
                if (GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "3" && isPatientOrHomeRelativeSign) return isValid;
                if (document != null && !String.IsNullOrEmpty(document.DocumentCode))
                {
                    var doc = new EmrDocument().GetViewByCode(document.DocumentCode);
                    if (doc != null && !String.IsNullOrEmpty(doc.NEXT_SIGNER) && doc.NEXT_SIGNER.Replace(SplitTagPatientCode, "") == treatment.PATIENT_CODE)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                        err += String.Format(MessageUitl.GetMessage(MessageConstan.BenhNhanKhongPhaiNguoiKyTiepTheoVuiLongKiemTraLai), treatment.VIR_PATIENT_NAME);
                        err += " | PATIENT_CODE = " + treatment.PATIENT_CODE;
                    }
                }
                else
                {
                    isValid = (signs != null && signs.Count > 0);
                    if (isValid)
                    {
                        var signBN = signs.Where(o => o.SignerId == null && o.PatientCode == treatment.PATIENT_CODE).FirstOrDefault();
                        if (signBN != null)
                        {
                            long minNumOrder = signs.Min(o => o.NumOrder);
                            isValid = isValid && signs.Any(o => o.SignerId == null && o.NumOrder == minNumOrder);
                            if (!isValid)
                                err += String.Format(MessageUitl.GetMessage(MessageConstan.BenhNhanKhongPhaiNguoiKyTiepTheoVuiLongKiemTraLai), treatment.VIR_PATIENT_NAME);
                        }
                        else
                        {
                            isValid = false;
                            err += String.Format(MessageUitl.GetMessage(MessageConstan.BenhNhanKhongCoTrongLuongKy), treatment.VIR_PATIENT_NAME);
                        }
                    }
                    else
                    {
                        err += String.Format(MessageUitl.GetMessage(MessageConstan.BenhNhanKhongCoTrongLuongKy), treatment.VIR_PATIENT_NAME);
                    }
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }
            return isValid;
        }

        internal static bool VerifySignPrintNow(List<FileADO> filesigns, ref string err)
        {
            bool isValid = true;
            try
            {
                isValid = (filesigns != null && filesigns.Count > 0);
                if (isValid)
                {
                    var isSignEmptyContent = filesigns.Exists(o => String.IsNullOrEmpty(o.Base64FileContent));
                    if (isSignEmptyContent)
                    {
                        isValid = false;
                        err += String.Format(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                    }
                }
                else
                {
                    err += String.Format(MessageUitl.GetMessage(MessageConstan.DuLieuKhongHopLe));
                }
            }
            catch (Exception ex)
            {
                isValid = false;
            }
            return isValid;
        }

    }
}
