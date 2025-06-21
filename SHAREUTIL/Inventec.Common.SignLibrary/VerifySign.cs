using EMR.EFMODEL.DataModels;
using EMR.TDO;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.CacheClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary
{
    internal class VerifySign
    {
        static bool? optionChoice = null;

        internal static long GetNumOderByCommentText(string commentText)
        {
            long value = 0;
            try
            {
                var arrSps = commentText.Split(new string[] { "__" }, StringSplitOptions.RemoveEmptyEntries);
                if (arrSps != null && arrSps.Count() > 0)
                {
                    string txtComment = arrSps[0].Replace("$", "").Replace("\n", "");
                    value = TypeConvertParse.ToInt64(txtComment);
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
            return value;
        }

        internal static long GetNumOrderBySignOrDefault(EMR.EFMODEL.DataModels.EMR_SIGN signSelected, EMR.EFMODEL.DataModels.EMR_SIGNER singer, EMR.EFMODEL.DataModels.EMR_TREATMENT treatment, List<SignTDO> listSign = null, bool isPatientSign = false, bool isHomeRelativeSign = false, int nextNum = 0)
        {
            long num = -1;
            try
            {
                if (signSelected != null)
                    num = signSelected.NUM_ORDER;
                else if (listSign != null && listSign.Count > 0)
                {
                    List<SignTDO> datas = null;
                    if (isPatientSign || isHomeRelativeSign)
                    {
                        datas = listSign.Where(o => o.PatientCode == treatment.PATIENT_CODE).ToList();
                    }
                    else
                    {
                        datas = listSign.Where(o => o.Loginname == singer.LOGINNAME).ToList();
                    }

                    num = (datas != null && datas.Count > 0) ? datas.FirstOrDefault().NumOrder : -1;
                }
                else
                    num = nextNum + frmSignerAdd.stepNumOrder;
            }
            catch (Exception ex)
            {
                num = -1;
                Logging.LogSystem.Warn(ex);
            }
            Inventec.Common.Logging.LogSystem.Debug("GetNumOrderBySignOrDefault: " + num);
            return num;
        }

        /// <summary>
        /// Trường hợp thực hiện ký với option EMR.EMR_SIGN.SIGN_DISPLAY_OPTION = 2 (chỉ hiển thị ảnh chữ ký, không thông tin ký)
        /// Nếu tài khoản chưa có ảnh chữ ký thì hiển thị thông báo là "Tài khoản thiếu thông tin ảnh, bạn có muốn bỏ hiển thị ảnh chữ ký không"
        ///- Nếu đồng ý thì ký ra thông tin ký (chỉ hiển thị người ký, thời gian ký, ...và không hiển thị ảnh chữ ký)
        ///- Nếu từ chối thì kết thúc thực hiện (ko thực hiện ký)        
        /// </summary>
        /// <returns></returns>
        internal static bool? VerifySignImageWithOption(Inventec.Common.SignLibrary.ADO.InputADO inputADO, EMR.EFMODEL.DataModels.EMR_SIGNER signer, bool hasNextSignPosition, Inventec.Common.SignLibrary.ADO.SignPositionADO nextSignPosition, bool isMultiSign)
        {
            bool? vOption = null;
            bool? isShowMessageAlert = null;
            optionChoice = null;
            try
            {
                if (signer != null)
                {
                    if (GlobalStore.EMR_EMR_SIGNER_AUTO_UPDATE_SIGN_IMAGE == "2")
                        return vOption;
                    if (signer.SIGN_IMAGE == null || signer.SIGN_IMAGE.Length == 0)
                    {
                        var cfgSignDisplayOption = GlobalStore.EmrConfigs.Where(o => o.KEY == EmrConfigKeys.SIGN_DISPLAY_OPTION).FirstOrDefault();

                        string vlOptionCfg = cfgSignDisplayOption != null ? (!String.IsNullOrEmpty(cfgSignDisplayOption.VALUE) ? cfgSignDisplayOption.VALUE : cfgSignDisplayOption.DEFAULT_VALUE) : "";

                        if (hasNextSignPosition && nextSignPosition != null)
                        {
                            if (isMultiSign && nextSignPosition.SignPositionAutos != null && nextSignPosition.SignPositionAutos.Count > 0)
                            {
                                var SignPositionAutoForAdds = nextSignPosition.SignPositionAutos.OrderBy(o => VerifySign.GetNumOderByCommentText(o.Text)).ToList();
                                foreach (var nSp in SignPositionAutoForAdds)
                                {
                                    if (nSp.TypeDisplay > 0)
                                    {
                                        isShowMessageAlert = (nSp.TypeDisplay == Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP || nSp.TypeDisplay == Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (nextSignPosition.TypeDisplay > 0)
                                {
                                    isShowMessageAlert = (nextSignPosition.TypeDisplay == Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP || nextSignPosition.TypeDisplay == Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT);
                                }
                            }
                        }

                        if (isShowMessageAlert == null && (vlOptionCfg == "2" || (!String.IsNullOrEmpty(vlOptionCfg) && vlOptionCfg != "1" && vlOptionCfg != "3" && vlOptionCfg != "2")))
                        {
                            isShowMessageAlert = true;
                        }
                        if (isShowMessageAlert.HasValue && isShowMessageAlert.Value)
                        {
                            string vlState = CacheClientWorker.GetValue();
                            if (!String.IsNullOrEmpty(vlState))
                            {
                                vOption = (vlState == Inventec.Common.SignFile.Constans.DISPLAY_IMAGE_STAMP.ToString());
                                if (vOption == false)
                                {
                                    MessageBox.Show(LibraryMessage.MessageUitl.GetMessage(LibraryMessage.MessageConstan.TaiKhoanThieuThongTinAnh));
                                }
                            }
                            else
                            {
                                bool IsReturn = false;
                                if (!String.IsNullOrEmpty(vlOptionCfg) && vlOptionCfg != "1" && vlOptionCfg != "3" && GlobalStore.EMR_EMR_SIGNER_AUTO_UPDATE_SIGN_IMAGE == "1" && GlobalStore.EMR_HSM_INTEGRATE_OPTION == "4")
                                {
                                    frmConfirmAutoUpdateSign frm = new frmConfirmAutoUpdateSign(confirm =>
                                    {
                                        if (confirm)
                                        {
                                            CommonParam param = new CommonParam();
                                            EMR_SIGNER SignerRs = GlobalStore.EmrConsumer.Post<EMR_SIGNER>("api/EmrSigner/AutoUpdateSignImage", param, signer.ID);
                                            if (SignerRs != null && SignerRs.SIGN_IMAGE != null && SignerRs.SIGN_IMAGE.Length > 0)
                                            {
                                                signer = SignerRs;
                                                IsReturn = true;
                                            }
                                        }
                                    });
                                    frm.ShowDialog();
                                }
                                if (!IsReturn)
                                {
                                    frmConfirmSign frmConfirmSign = new frmConfirmSign(ActNo, ActYes);
                                    frmConfirmSign.ShowDialog();
                                    vOption = (optionChoice != null && optionChoice.Value);
                                }
                            }

                            Inventec.Common.Logging.LogSystem.Info(LibraryMessage.MessageUitl.GetMessage(LibraryMessage.MessageConstan.TaiKhoanThieuThongTinAnhBanCoMuonBoHienThiAnh) + "____nguoi dung chon " + ((vOption.HasValue && vOption.Value) ? "bo hien thi anh chi hien thi thong tin dang text" : "tu choi & ket thuc khong ky____") + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => vlOptionCfg), vlOptionCfg) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => optionChoice), optionChoice) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => vlState), vlState) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => vOption), vOption));
                        }
                        else
                        {
                            Inventec.Common.Logging.LogSystem.Warn("Khong hien thi thong bao do du lieu hop le____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => isShowMessageAlert), isShowMessageAlert) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => vOption), vOption));
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Debug("GlobalStore.Singer.SIGN_IMAGE has value");
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Warn("Khong tim thay thong tin nguoi ky tren emr tuong ung voi tai khoan dang ky" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => signer), signer) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => vOption), vOption));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return vOption;
        }

        static void ActYes()
        {
            optionChoice = true;
        }

        static void ActNo()
        {
            optionChoice = false;
        }
    }
}
