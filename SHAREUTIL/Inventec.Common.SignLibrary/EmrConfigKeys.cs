using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aspose.Pdf.Operator;

namespace Inventec.Common.SignLibrary
{
    internal class EmrConfigKeys
    {
        /// <summary>
        /// "Tùy chọn ký với chế độ ký USB token:
        ///1 - Ký usb token sử dụng service(khắc phục lỗi sau khi ký bị mất menu chuột phải trên phần mềm).
        ///Khác 1 - Ký usb token trực tiếp trên HIS."
        /// </summary>
        internal const string EMR__EMR_SIGN__USING_USBTOKENDEVICE__OPTION = "EMR.EMR_SIGN.USING_USBTOKENDEVICE.OPTION";

        /// <summary>
        /// + Cấu hình loại ký sử dụng USB token hay sử dụng server HSM:
        /// + Đặt 1 là sử dụng USB token ký.
        /// + Đặt 2 là sử dụng HSM server để ký.
        /// + Bổ sung thêm option 3: Ký theo cả 2 hình thức vừa HSM, vừa USB token. Mặc định USB token.
        /// + Bổ sung thêm option 4: Ký theo cả 2 hình thức vừa HSM, vừa USB token. Mặc định HSM.
        /// </summary>
        internal const string LIBRARY__EMRGENERATE__SIGNTYPE = "HIS.Desktop.Plugins.Library.EmrGenerate.SignType";

        /// <summary>
        /// - "Cấu hình tùy chọn các chế độ ký trên hệ thống
        ///1: Chỉ cho phép ký số sử dụng chứng thư số (HSM, Usb Token hoặc Sim CA)
        ///2: Cho phép ký số đối với các tài khoản đã được cấp chứng thư số (HSM, Usb Token hoặc Sim CA) và cả chữ ký điện tử đối với các tài khoản không có chứng thư số nhưng có ảnh chữ ký"
        /// </summary>
        internal const string EMR__EMR_SIGN_SIGNING_OPTION = "EMR.EMR_SIGN.SIGNING_OPTION";

        /// <summary>
        /// - + Nếu có giá trị 1 -> Truyền bổ sung tham số xác định là ký bằng tab thẻ.
        ///   + Nếu có giá trị 2 -> Truyền bổ sung tham số xác định là ký bằng vân tay.
        /// </summary>
        internal const string EMR__EMR_DOCUMENT__PATIENT_SIGN__OPTION = "EMR.EMR_DOCUMENT.PATIENT_SIGN.OPTION";

        /// <summary>
        ///  Tùy chọn cách lấy địa điểm ký:
        ///1 - Lấy theo thông tin "đơn vị" trong dữ liệu "người ký" (DEPARTMENT_NAME trong EMR_SIGNER).
        ///2 - Lấy theo thông tin Khoa người dùng đang làm việc.
        ///3 - Nếu có thông tin Khoa người dùng đang làm việc thì lấy, còn không có thì lấy theo thông tin "Đơn vị" trong "người ký" (DEPARTMENT_NAME trong EMR_SIGNER).
        ///Mặc định 1.
        /// </summary>
        internal const string EMR__EMR_SIGN__PLACE_SIGN_OPTION = "EMR.EMR_SIGN.PLACE_SIGN_OPTION";

        /// <summary>
        /// Định dạng URL hiển thị ảnh PACS, khai báo dưới theo định dạng:
        ///http://patientpacs.gensign.vn:1111/Worklist.aspx?doctor=view&Ma_benh_nhan=<#PATIENT_CODE;>&AccessNumber=<#HIS_CODE;>&Bat_dau=<#DOCUMENT_TIME;>
        /// </summary>
        internal const string EMR_VIEW_PACS_URL_FORMAT = "EMR.VIEW_PACS_URL_FORMAT";

        /// <summary>
        /// Tùy chọn khi mở 1 văn bản có HIS_CODE đã tồn tại trên EMR. 0 - Cảnh báo. 1 - Chặn (mở văn bản cũ).
        /// 2: PM xử lý là văn bản đã ký hoặc đã thiết lập ký thì mới ko cho tạo lại. Còn Chỉ tạo văn bản mà không có thiết lập ký hay hủy thì cho tạo bình thường.
        /// Mặc định 0.
        /// </summary>
        internal const string EMR__EMR_DOCUMENT__DULICATE_HIS_CODE__WARNING_OPTION = "EMR.EMR_DOCUMENT.DULICATE_HIS_CODE.WARNING_OPTION";

        /// <summary>
        /// Tùy chọn in văn bản sử dụng wartermark. 1 - Có sử dụng. 0 - Không sử dụng. Mặc định là 1.
        /// </summary>
        internal const string PRINT_USING_WARTERMARK__OPTION = "EMR.DOCUMENT.PRINT_USING_WARTERMARK.OPTION";

        /// <summary>
        /// Cấu hình khai báo nội dung watermark theo các key <#DOCUMENT_CODE;> <#DOCUMENT_NAME;> <#TREATMENT_CODE;> <#LOGINNAME;> <#USER_NAME;> <#TIME_NOW;> <#CREATE_TIME_STR;>
        /// </summary>
        internal const string WARTERMARK__VALUE_OPTION = "EMR.EMR_DOCUMENT.WARTERMARK.VALUE_OPTION";

        /// <summary>
        /// 1: Chỉ hiển thị thông tin ký (người ký, thời gian ký, ...), không hiển thị ảnh chữ ký. 2: Chỉ hiển thị hiển thị ảnh chữ ký, không thông tin ký (người ký, thời gian ký, ...).3: Không hiển thị (cả thông tin ký và ảnh chữ ký). Khác: Hiển thị cả thông tin ký và ảnh chữ ký
        /// </summary>
        internal const string SIGN_DISPLAY_OPTION = "EMR.EMR_SIGN.SIGN_DISPLAY_OPTION";

        /// <summary>
        /// Cấu hình mặc định hiện nút in. Đặt 1: mặc định hiển thị. Khác 1: hiển thị theo tham số HIS truyền
        /// </summary>
        internal const string PRINT_OPTION = "EMR.EMR_SIGN.PRINT_OPTION";

        /// <summary>
        /// Cấu hình sử dụng thư viện in văn bản. Giá trị 1: in thư viện của Apose pdf. Giá trị 0: in thư viện của Dev. Mặc định in thư viên của devexpress.
        /// </summary>
        internal const string PRINT_LIBIARY_OPTION = "EMR.EMR_SIGN.PRINT_LIBIARY_OPTION";

        /// <summary>
        /// "Cấu hình tùy chỉnh các tham số vùng hiển thị chữ ký. Vd: p:5|f:7|w:100|h:40|a:2|fs:B. Trong đó p:5          : 5 là tỉ lệ kích thước text 70% x ảnh 30%
        ///         Có các loại tỉ lệ kích thước ảnh và text như sau:
        ///    x100 = 0 ==> ảnh và text trong cùng 1 ô, ảnh ở trên text ở dưới
        ///    x50x50 = 1, ==> ảnh 50% - text 50% ảnh trước text
        ///    x40x60 = 2, ==> ảnh 40% - text 60% ảnh trước text
        ///    x60x40 = 3, ==> text 60% - ảnh 40% ảnh sau text
        ///    x30x70 = 4, ==> ảnh 30% - text 70% ảnh trước text
        ///    x70x30 = 5, ==> text 70% - ảnh 30% ảnh sau text
        ///    x25x75 = 6, ==> ảnh 25% - text 75% ảnh trước text
        ///    x75x25 = 7, ==> text 75% - ảnh 25% ảnh sau text 
        ///f:7          : 7 là cỡ chữ của chữ ký
        ///w:100        : 100 là chiều dài vùng hiển thị chữ ký
        ///h:40         : 40 là chiều cao vùng hiển thị chữ ký
        ///a:1 với 1 là giá trị căn chỉnh vị trí text trong chữ ký. Có cá loại căn chỉnh như sau:
        //1: Căn trái
        //2: Căn giữa
        //3: Căn phải
        //4: Căn đều
        //fs:BIU với BIU là giá trị kiểu chữ.Chỉ nhận các giá trị B, I, U ứng với in đậm, in nghiêng, gạch chân. Có nhiều kiểu thì khai báo liền nhau
        //fn:Arial với Arial là giá trị tên font chữ(Khuyến cáo không nên đặt tham số này)."
        /// </summary>
        internal const string SIGNATURE_APPEARANCE_OPTION = "EMR.EMR_SIGN.SIGNATURE_APPEARANCE_OPTION";

        /// <summary>
        /// Cấu hình ẩn trang thông tin đã ký. Đặt 1: ẩn. Khác: hiển thị.
        /// </summary>
        internal const string SPLIT_PDF_KEY = "EMR.EMR_SIGN.SPLIT_PDF_KEY";

        /// <summary>
        /// Cấu hình ẩn trang thông tin đã ký. Đặt 1: ẩn. Giá trị khác: hiển thị. Nếu không có cấu hình hoặc cấu hình không có giá trị tool ký sẽ lấy giá trị biến bên HIS truyền vào
        /// </summary>
        internal const string IS_NOT_SHOWING_SIGN_INFORMATION = "EMR.IS_NOT_SHOWING_SIGN_INFORMATION";

        /// <summary>
        /// Đặt 1: hiển thị watermark, 0: không hiển thị. Không cấu hình măc định sẽ hiển thị
        /// </summary>
        internal const string IS_SHOW_WATER_MARK = "EMR.EMR_SIGN.IS_SHOW_WATER_MARK";

        /// <summary>
        /// 1 - Hiển thị đầy đủ "Tên người ký", "Thời gian ký", "Địa điểm ký", "Chức danh"
        ///2 - Chỉ hiển thị "Tên người ký", "Chức danh" (hiển thị trên 2 dòng)
        ///3 - Chỉ hiển thị "Tên người ký", "Thời gian ký", "Địa điểm ký"
        /// </summary>
        internal const string SIGN_INFO_DISPLAY_OPTION = "EMR.EMR_SIGN.SIGN_INFO_DISPLAY_OPTION";

        internal const string INTERGRATE_SYS_BASE_URI = "EMR.INTERGRATE_SYS_BASE_URI";

        internal const string INTERGRATE_SYS_API = "EMR.INTERGRATE_SYS_API";

        /// <summary>
        /// Aspose.Total for .NET 2015 license
        /// </summary>
        internal const string LICENSE_KEY__APOSE = "LICENSE_KEY__APOSE";

        /// <summary>
        /// Cấu hình tùy chọn ký bệnh nhân
        /// - + Nếu có giá trị 1 => luôn ký điện tử, không quan tâm được cấp phát CTS hay không.
        ///   + Nếu có giá trị 2 => chỉ ký điện tử khi thẻ kcb của bệnh nhân chưa được cấp phát chứng thư số.
        ///   + Mặc định giá trị là 2
        /// </summary>
        internal const string EMR__EMR_SIGN__PATIENT_SIGN__OPTION = "EMR.EMR_SIGN.PATIENT_SIGN.OPTION";

        /// <summary>
        /// Cấu hình tùy chọn sử dụng thư viện ký có tính năng ký bằng bảng ký hay không.
        ///   1: Ký không sử dụng tính năng bảng ký
        ///   2: Ký có sử dụng tính năng bảng ký.
        ///   Mặc định chạy với cấu hình giá trị 1 khi không có cấu hình hoặc cấu hình không đặt giá trị
        /// </summary>
        internal const string EMR__EMR_SIGN__SIGN_BOARD__OPTION = "EMR.EMR_SIGN.SIGN_BOARD.OPTION";

        internal const string EMR__EMR_SIGN__SIGN_DESCRIPTION_INFO__OPTION = "EMR.EMR_SIGN.SIGN_DESCRIPTION_INFO.OPTION";

        internal const string EMR_HSM_INTEGRATE_OPTION = "EMR.HSM.INTEGRATE_OPTION";

        internal const string EMR_EMR_SIGNER_AUTO_UPDATE_SIGN_IMAGE = "EMR.EMR_SIGNER.AUTO_UPDATE_SIGN_IMAGE";

        /// <summary>
        /// Cấu hình hệ thống bắt buộc bệnh nhân ký đầu tiên nếu có khai báo bệnh nhân ký.
        /// Giá trị 1: Bắt buộc
        /// Khác 1: Không bắt buộc
        /// </summary>
        internal const string EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION = "EMR.EMR_DOCUMENT.PATIENT_SIGN_FIRST.OPTION";

        internal static string EMR_EMR_SIGN_CONNECT_DEVICE_TYPE_OPTION = "EMR.EMR_SIGN.CONNECT_DEVICE_TYPE_OPTION";
    }
}
