using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.Base
{
    public class ServerInfo
    {
        public const string CodeLoiCauHinhHeThong = "01";
        public const string CodeLoiKetNoiHeThongBHYT = "02";
        public const string CodeTaiKhoanKhongHopLe = "03";
        public const string CodeLoiTaoDuLieuXML = "04";
        public const string CodeLoiDoHeThongBHYTTraVe = "05";
        public const string CodeKhongXacDinh = "99";
        public const string CodeLoiGoiApiGuiHoSo = "06";
        public const string Text01 = "Lỗi cấu hình hệ thống";
        public const string Text02 = "Lỗi kết nối hệ thống BHYT";
        public const string Text03 = "Tài khoản không hợp lệ";
        public const string Text04 = "Lỗi tạo dữ liệu XML";
        public const string Text05 = "Lỗi do hệ thống BHYT trả lại";
        public const string Text06 = "Lỗi gọi api gửi hồ sơ";
        public const string Text99 = "Không xác định";
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string checkinApi { get; set; }
        public ServerInfo() { }
        public ServerInfo(string Username, string Password, string Address, string checkinApi)
        {
            this.Username = Username;
            this.Password = Password;
            this.Address = Address;
            this.checkinApi = checkinApi;
        }
    }
}
