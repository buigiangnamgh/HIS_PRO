using EMR.EFMODEL.DataModels;
using EMR.TDO;
using Inventec.Common.SignLibrary.LibraryMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary
{
    class FingerPrintClientService
    {
        static EMR_RELATION Relation;
        static bool? isMe;
        /// <summary>
        /// #20291
        /// Sửa lại form ký điện tử để đáp ứng quy trình mới, cụ thể:
        ///- Khi gọi sang The.desktop ko cần gửi thông tin card_code (gọi theo api mới mà the.desktop cung cấp)
        ///- Khi The.desktop trả lại thông tin thẻ, form ký kiểm tra:
        ///+ Nếu hồ sơ điều trị có thông tin card_code, và giống với thông tin card_code mà The.desktop trả về ở bước 6 (trong mo_hinh_moi.png), thì xử lý như hiện tại.
        ///+ Nếu hồ sơ điều trị ko có thông tin card_code hoặc có thông tin card_code khác với thông tin card_code mà The.desktop trả về ở bước 6 (trong mo_hinh_moi.png), thì xử lý hiển thị form cho phép người dùng chọn "Quan hệ". Form ký gửi thông tin ký + thông tin quan hệ lên backend EMR để thực hiện ký
        ///- Danh sách "quan hệ" cho người dùng chọn, được lấy từ danh mục quan hệ của EMR (EMR_RELATION), và front-end tự động bổ sung thêm 1 dòng "Là tôi". Khi người dùng chọn "Là tôi" thì lúc ký không gửi lên thông tin "quan hệ"
        /// </summary>
        /// <param name="cmnd"></param>
        /// <param name="cardCode"></param>
        /// <param name="tempSigns"></param>
        /// <returns></returns>
        internal static bool Valid(bool isHomeRelativeSign, ref string cmnd, ref string cardCode, ref string serviceCode, ref string linkCode, ref string relativeName, ref string relationPeopleName, ref byte[] signedImageData, ref bool isCardAnonymous, ref List<SignTDO> tempSigns, EMR.EFMODEL.DataModels.EMR_TREATMENT treatment)
        {
            try
            {                
                if (!isHomeRelativeSign && (treatment == null || String.IsNullOrEmpty(treatment.CARD_CODE)))
                {
                    Inventec.Common.SignLibrary.Integrate.MessageManager.Show(MessageUitl.GetMessage(MessageConstan.BenhNhanKhongCoTheKCB));
                    Inventec.Common.Logging.LogSystem.Warn(MessageUitl.GetMessage(MessageConstan.BenhNhanKhongCoTheKCB));
                    return false;
                }

                CARD.WCF.DCO.WcfFingerprintDCO fingerprintDCO = new CARD.WCF.DCO.WcfFingerprintDCO();
                fingerprintDCO.CardCode = treatment != null ? treatment.CARD_CODE : "";
                if (isHomeRelativeSign)
                {
                    fingerprintDCO.IsHomieSign = true;
                }

                if (GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "1")
                {
                    fingerprintDCO.AuthenType = 1;
                }
                else if (GlobalStore.EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION == "2")
                {
                    fingerprintDCO.AuthenType = 2;
                }

                int demLanGoi = 0;
                var wcfRs = VerifyFingerAuthen(fingerprintDCO, ref demLanGoi);

                //rs.ResultCode = "45";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__VanTayKhongHopLe;||ResourceMessage.Fingerprint__QuaThoiGianLayVanTay;

                //rs.ResultCode = "44";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__ChuTheChuaDuocThietLapVanTay;

                //rs.ResultCode = "40";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__PhanMemChuaKetNoiThietBi;

                //rs.ResultCode = "42";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__KhongTruyenSoTheCuaNguoiXacThuc;

                //rs.ResultCode = "46";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__KhongDocDuocThongTinThe;

                //rs.ResultCode = "47";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__DauVaoKhongHopLe;

                //rs.ResultCode = "48";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__KhongNhapTTDinhDanhHoacChonTheKy||ResourceMessage.Fingerprint__QuaThoiGianTapThe;

                //rs.ResultCode = "43";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__SoTheKhongKhop;||ResourceMessage.Fingerprint__SoTheKhongChinhXac

                //rs.ResultCode = "42";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__SoTheKhongChinhXac;

                //rs.ResultCode = "99";
                //rs.ResultDescBase64 = ResourceMessage.Fingerprint__LoiKhongXacDinh;

                if (wcfRs == null || wcfRs.ResultCode != "00" || String.IsNullOrEmpty(wcfRs.CmndNumber) || String.IsNullOrEmpty(wcfRs.CardCode))
                {
                    Inventec.Common.Logging.LogSystem.Warn("FingerPrintClientService.Valid = false. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => wcfRs), wcfRs));
                    return false;
                }

                isCardAnonymous = wcfRs.IsCardAnonymous;

                relationPeopleName = !String.IsNullOrEmpty(wcfRs.PeopleNameBase64) ? Utils.Base64Decode(wcfRs.PeopleNameBase64) : "";
                string _relationName = !String.IsNullOrEmpty(wcfRs.RelationNameBase64) ? Utils.Base64Decode(wcfRs.RelationNameBase64) : "";
                relativeName = _relationName;
                cmnd = wcfRs.CmndNumber;
                cardCode = wcfRs.CardCode;
                serviceCode = wcfRs.ServiceCode;
                linkCode = wcfRs.LinkCode;
                signedImageData = !String.IsNullOrEmpty(wcfRs.FingerImageBase64) ? Convert.FromBase64String(wcfRs.FingerImageBase64) : null;
                if (isHomeRelativeSign)
                {
                    var relationDatas = new Inventec.Common.SignLibrary.Api.EmrRelation().Get();
                    Relation = (relationDatas != null && relationDatas.Count > 0) ? relationDatas.FirstOrDefault(o => o.RELATION_NAME == _relationName) : null;

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
                                tsn.RelationId = (Relation != null) ? (long?)Relation.ID : null;
                                tsn.RelationName = _relationName;
                                tsn.RelationPeopleName = relationPeopleName;
                                tsn.SignedImageData = signedImageData;
                            }
                        }
                    }
                }
                else
                {
                    if (tempSigns != null && tempSigns.Count > 0)
                    {
                        foreach (var tsn in tempSigns)
                        {
                            if (!String.IsNullOrEmpty(tsn.PatientCode))
                            {
                                tsn.CardCode = cardCode;
                                tsn.ServiceCode = serviceCode;
                                tsn.CmndNumber = cmnd;
                                tsn.LinkCode = linkCode;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn("Loi goi service xac thuc van tay", ex);
                MessageBox.Show(MessageUitl.GetMessage(MessageConstan.LoiGoiServiceXacThucVanTay));
                return false;
            }

            return true;
        }

        private static CARD.WCF.DCO.WcfFingerprintDCO VerifyFingerAuthen(CARD.WCF.DCO.WcfFingerprintDCO fingerprintDCO, ref int demLanGoi)
        {
            CARD.WCF.DCO.WcfFingerprintDCO wcfRs = null;
            CARD.WCF.Client.FingerprintClient.FingerprintClientManager fingerprintClientManager = new CARD.WCF.Client.FingerprintClient.FingerprintClientManager();
            wcfRs = fingerprintClientManager.Fingerprint(fingerprintDCO);
            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => wcfRs), wcfRs));
            if (wcfRs.ResultCode != "00")
            {
                string messageFinger = "";
                if (!String.IsNullOrEmpty(wcfRs.ResultDescBase64) && !String.IsNullOrEmpty(Utils.Base64Decode(wcfRs.ResultDescBase64)))
                {
                    messageFinger += Utils.Base64Decode(wcfRs.ResultDescBase64);
                }

                Inventec.Common.Logging.LogSystem.Warn("Goi service xac thuc van tay. Ket qua tra ve that bai___" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => messageFinger), messageFinger));
                demLanGoi++;
                if (wcfRs.ResultCode == "45" && demLanGoi <= 3)
                {
                    messageFinger += String.Format("({0}). ", wcfRs.ResultCode);
                    if (MessageBox.Show(messageFinger + "Bạn có muốn thử lại?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        return VerifyFingerAuthen(fingerprintDCO, ref demLanGoi);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    messageFinger += String.Format("({0}). Xử lý thất bại.", wcfRs.ResultCode);

                    MessageBox.Show(messageFinger);
                    return null;
                }
            }

            return wcfRs;
        }
    }
}
