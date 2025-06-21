using EMR.EFMODEL.DataModels;
using EMR.TDO;
using Inventec.Common.SignLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.ADO
{
    public class InputADO
    {
        public bool IsUseTimespan { get; set; }
        public bool IsPatientSign { get; set; }
        public bool? IsHomeRelativeSign { get; set; }
        public bool IsSign { get; set; }
        public bool IsShowPatientSign { get; set; }
        public bool? IsMultiSign { get; set; }
        public bool IsPrint { get; set; }
        public bool IsSave { get; set; }
        public bool IsReject { get; set; }
        public bool IsExport { get; set; }
        public bool? IsSignConfig { get; set; }
        public bool? IsShowWatermark { get; set; }
        public bool? IsEnableButtonPrint { get; set; }
        /// <summary>
        /// Cấu hình ẩn trang thông tin ký. Nếu có key cấu hình EMR.IS_NOT_SHOWING_SIGN_INFORMATION trong bảng cấu hình bên EMR thì sẽ tool ký tự gán giá trị theo key này. Ngược lại lấy giá trị bên ngoài truyền vào
        /// </summary>
        public bool IsPrintOnlyContent { get; set; }
        public Action<float, float> DlgChoosePoint { get; set; }
        public Action<DocumentTDO> DlgOpenModuleConfig { get; set; }
        public Action<DocumentSignedUpdateIGSysResultDTO> DlgSendResultSigned { get; set; }
        public DlgGetTreatment DlgGetTreatment { get; set; }
        public bool? IsCloseAfterSign { get; set; }
        public Action<bool> DlgCloseAfterSign { get; set; }
        public bool? IsOptionSignType { get; set; }
        public Action<bool> DlgChangeOptionSignType { get; set; }
        public bool IsSelectRangeRectangle { get; set; }
        public List<string> Watermarks { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentGroupCode { get; set; }
        public string DocumentName { get; set; }
        public string DocumentTypeCode { get; set; }
        public string HisCode { get; set; }
        public SignType SignType { get; set; }
        public TreatmentDTO Treatment { get; set; }
        public string SignReason { get; set; }
        public string BusinessCode { get; set; }
        public List<string> PrintTypeBusinessCodes { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public string RoomTypeCode { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DTI { get; set; }
        public string HisUriUpdateSignedState { get; set; }
        public List<SignerConfigDTO> SignerConfigs { get; set; }
        public DisplayConfigDTO DisplayConfigDTO { get; set; }
        public short? PrintNumberCopies { get; set; }
        public bool? IsAutoChooseBusiness { get; set; }
        public string MergeCode { get; set; }
        public DateTime? DocumentTime { get; set; }
        public string DependentCode { get; set; }
        public string ParentDependentCode { get; set; }
        public string HisOrder { get; set; }
        public string PrinterDefault { get; set; }
        public System.Drawing.Printing.PaperSize PaperSizeDefault { get; set; }
        public Action ActPrintSuccess { get; set; }
        public Action<string> ActSelectDevice { get; set; }
        public string DeviceSignPadName { get; set; }
        public bool? IsUsingSignPad { get; set; }
        public bool IsRemoveSignPadBefore { get; set; }
        public Action<bool> ActChangeUsingSignPad { get; set; }
        public string RelationPeopleName { get; set; }
        public EMR_RELATION Relation { get; set; }
        public Action reload { get; set; }

        public InputADO() { }
        public InputADO(Action<float, float> dlgChoosePoint, bool isSelectRangeRectangle, List<string> watermarks, SignType signType)
        {
            this.DlgChoosePoint = dlgChoosePoint;
            this.IsSelectRangeRectangle = isSelectRangeRectangle;
            this.Watermarks = watermarks;
            this.SignType = signType;
        }
        public InputADO(Action<float, float> dlgChoosePoint, bool isSelectRangeRectangle, List<string> watermarks, SignType signType, Action _reload)
        {
            this.DlgChoosePoint = dlgChoosePoint;
            this.IsSelectRangeRectangle = isSelectRangeRectangle;
            this.Watermarks = watermarks;
            this.SignType = signType;
            this.reload = _reload;
        }
    }
}
