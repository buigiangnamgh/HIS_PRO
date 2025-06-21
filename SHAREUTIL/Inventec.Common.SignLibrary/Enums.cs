using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary
{
    public enum SignType
    {      
        [Description("Ký sử dụng USB Token")]
        USB = 1,
        [Description("Ký sử dụng HSM server")]
        HMS = 2,
        [Description("Ký sử dụng HSM server")]
        HSM = 2,
        [Description("Chọn loại ký, mặc đinh ký Usb token")]
        OptionDefaultUsb = 3,
        [Description("Chọn loại ký, mặc đinh ký Hsm")]
        OptionDefaultHsm = 4,
    }


    public enum FileType
    {
        [Description("Pdf")]
        Pdf = 0,
        [Description("Xls")]
        Xls = 1,
        [Description("Xlsx")]
        Xlsx = 2,
        [Description("Rdlc")]
        Rdlc = 3,
        [Description("Doc")]
        Doc = 4,
        [Description("Docx")]
        Docx = 5,
        [Description("Html")]
        Html = 6,
        [Description("Rtf")]
        Rtf = 7,
        [Description("Xml")]
        Xml = 8,
        [Description("Json")]
        Json = 9,
    }

    public enum OptionPrintType
    {
        [Description("Print with DevLib")]
        DevLib = 0,
        [Description("Print with AposePdf")]
        PdfAposeLib = 1,
        [Description("Print with call Exe print service")]
        CallExeLib = 2
    }

    internal enum SignBoardOption
    {
        [Description("Không sử dụng tính năng bảng ký")]
        NoUse = 1,
        [Description("Có sử dụng tính năng bảng ký")]
        Use = 2
    }
}
