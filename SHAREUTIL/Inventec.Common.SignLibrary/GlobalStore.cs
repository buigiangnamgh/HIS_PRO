using EMR.URI;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary
{
    public class GlobalStore
    {
        internal const string appCode = "HIS";
        private static ApiConsumer integrateConsumer;
        internal static ApiConsumer IntegrateConsumer
        {
            get
            {
                if (integrateConsumer == null)
                {
                    integrateConsumer = new ApiConsumer(INTERGRATE_SYS_BASE_URI, appCode);
                }
                return integrateConsumer;
            }
            set
            {
                integrateConsumer = value;
            }
        }

        private static List<EMR.EFMODEL.DataModels.EMR_CONFIG> emrConfigs;
        public static List<EMR.EFMODEL.DataModels.EMR_CONFIG> EmrConfigs
        {
            get
            {
                if (emrConfigs == null)
                {
                    Inventec.Common.SignLibrary.Api.EmrConfig emrConfig = new Inventec.Common.SignLibrary.Api.EmrConfig();
                    emrConfigs = emrConfig.Get();
                }
                return emrConfigs;
            }
            set
            {
                emrConfigs = value;
            }
        }

        private static List<EMR.EFMODEL.DataModels.EMR_BUSINESS> emrBusiness;
        public static List<EMR.EFMODEL.DataModels.EMR_BUSINESS> EmrBusiness
        {
            get
            {
                if (emrBusiness == null)
                {
                    Inventec.Common.SignLibrary.Api.EmrBusiness emrbusinesss = new Inventec.Common.SignLibrary.Api.EmrBusiness();
                    emrBusiness = emrbusinesss.Get();
                }
                return emrBusiness;
            }
            set
            {
                emrBusiness = value;
            }
        }

        public static EMR.EFMODEL.DataModels.EMR_SIGNER GetByLoginName(string loginName)
        {
            EMR.EFMODEL.DataModels.EMR_SIGNER data = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(loginName))
                {
                    CommonParam param = new CommonParam();
                    EMR.Filter.EmrSignerFilter filter = new EMR.Filter.EmrSignerFilter();
                    filter.LOGINNAMEs = new List<string>() { loginName, loginName.ToLower(), loginName.ToUpper() };
                    List<EMR.EFMODEL.DataModels.EMR_SIGNER> datas = new Api.EmrSigner().Get(ref param, filter);
                    data = datas != null ? datas.FirstOrDefault() : null;
                    if (data == null)
                    {
                        MessageManager.Show(param, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        public static ApiConsumer GetSetDicConsumer(string tokenCode)
        {
            ApiConsumer rs = null;
            if (!String.IsNullOrEmpty(tokenCode))
            {
                if (!DicEmrConsumer.ContainsKey(tokenCode))
                {
                    rs = new ApiConsumer(EMR_BASE_URI, tokenCode, appCode);
                    DicEmrConsumer.Add(tokenCode, rs);
                }
                else
                    rs = DicEmrConsumer[tokenCode];
            }

            return rs;
        }

        public static Dictionary<string, ApiConsumer> GetDicEmrConsumer()
        {
            return dicemrConsumer;
        }

        private static Dictionary<string, ApiConsumer> dicemrConsumer;
        internal static Dictionary<string, ApiConsumer> DicEmrConsumer
        {
            get
            {
                if (dicemrConsumer == null)
                {
                    dicemrConsumer = new Dictionary<string, ApiConsumer>();
                }
                return dicemrConsumer;
            }
            set
            {
                dicemrConsumer = value;
            }
        }

        private static ApiConsumer emrConsumer;
        internal static ApiConsumer EmrConsumer
        {
            get
            {
                if (emrConsumer == null)
                {
                    emrConsumer = new ApiConsumer(EMR_BASE_URI, appCode);
                }
                return emrConsumer;
            }
            set
            {
                emrConsumer = value;
            }
        }

        private static ApiConsumer acsConsumer;
        internal static ApiConsumer AcsConsumer
        {
            get
            {
                if (acsConsumer == null)
                {
                    acsConsumer = new ApiConsumer(ConstanIG.ACS_BASE_URI, appCode);
                }
                return acsConsumer;
            }
            set
            {
                acsConsumer = value;
            }
        }

        internal static string INTERGRATE_SYS_BASE_URI { get; set; }
        internal static string INTERGRATE_SYS_API { get; set; }
        internal static string EMR_BASE_URI { get; set; }
        internal static string TokenCode { get; set; }
        internal static string LoginName { get; set; }
        internal static string UserName { get; set; }
        internal static EMR.EFMODEL.DataModels.EMR_SIGNER Singer { get; set; }
        internal static bool IsUseTimespan { get; set; }
        internal static string Password { get; set; }
        internal static TokenData TokenData { get; set; }
        internal static bool IsUseSendDTI { get; set; }
        internal static string PIN { get; set; }

        private static string gemBoxPdf__LicKey;
        internal static string GemBoxPdf__LicKey
        {
            get
            {
                if (String.IsNullOrEmpty(gemBoxPdf__LicKey))
                {
                    gemBoxPdf__LicKey = "ARHC-LA4K-R49S-TR4L";
                }
                return gemBoxPdf__LicKey;
            }
            set
            {
                gemBoxPdf__LicKey = value;
            }
        }

        /// <summary>
        /// Cấu hình chọn thư viện in. Đặt 1: in thư viện của Apose pdf. Giá trị 0: in thư viện của Dev. Mặc định in thư viên của devexpress.
        /// </summary>
        internal static OptionPrintType OptionPrintType = OptionPrintType.DevLib;

        private static string splitPdfHeaderKey;
        internal static string SplitPdfHeaderKey
        {
            get
            {
                if (String.IsNullOrEmpty(splitPdfHeaderKey))
                {
                    splitPdfHeaderKey = "{SignLibrary.SplitPdfHeaderKey}";
                }
                return splitPdfHeaderKey;
            }
            set
            {
                splitPdfHeaderKey = value;
            }
        }

        private static string splitPdfContentKey;
        internal static string SplitPdfContentKey
        {
            get
            {
                if (String.IsNullOrEmpty(splitPdfContentKey))
                {
                    splitPdfContentKey = "{SignLibrary.SplitPdfContentKey}";
                }
                return splitPdfContentKey;
            }
            set
            {
                splitPdfContentKey = value;
            }
        }

        internal static bool PrintUsingWaterMark = true;

        /// <summary>
        /// = True nếu ký usb token trực tiếp
        /// = False nếu ký usb token thông qua service trung gian
        /// </summary>
        public static bool IsSignUsingUsbTokenDevice = true;//TODO set value  

        /// <summary>
        /// + Nếu có giá trị 1 -> Truyền bổ sung tham số xác định là ký bằng tab thẻ.
        ///+ Nếu có giá trị 2 -> Truyền bổ sung tham số xác định là ký bằng vân tay.
        /// </summary>
        internal static string EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION = "";

        internal static string EMR_SIGN_SIGNING_OPTION = "";

        internal static string EMR_SIGN_PATIENT_SIGN_OPTION = "";

        internal static string EMR_SIGN_BOARD__OPTION = "1";

        internal static string EMR_SIGN_SIGN_DESCRIPTION_INFO = "2";

        internal static string EMR_HSM_INTEGRATE_OPTION = "1";

        internal static string EMR_EMR_SIGNER_AUTO_UPDATE_SIGN_IMAGE = "";

        internal static string EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION = "";
    }
}
