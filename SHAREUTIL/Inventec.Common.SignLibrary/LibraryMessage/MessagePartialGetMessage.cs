using System;
namespace Inventec.Common.SignLibrary.LibraryMessage
{
    internal class MessageUitl
    {
        internal static string GetMessage(string enumBC)
        {
            string message = "";
            switch (enumBC)
            {
                case MessageConstan.BugCode40: message = MessageViResource.BugCode40; break;
                case MessageConstan.BugCode41: message = MessageViResource.BugCode41; break;
                case MessageConstan.BugCode42: message = MessageViResource.BugCode42; break;
                case MessageConstan.BugCode43: message = MessageViResource.BugCode43; break;
                case MessageConstan.BugCode44: message = MessageViResource.BugCode44; break;
                case MessageConstan.BugCode45: message = MessageViResource.BugCode45; break;
                case MessageConstan.BugCode46: message = MessageViResource.BugCode46; break;
                case MessageConstan.BugCode47: message = MessageViResource.BugCode47; break;
                case MessageConstan.BugCode48: message = MessageViResource.BugCode48; break;
                case MessageConstan.BugCode49: message = MessageViResource.BugCode49; break;
                case MessageConstan.BugCode99: message = MessageViResource.BugCode99; break;

                case MessageConstan.BanCoMuonMoFileVuaKy: message = MessageViResource.BanCoMuonMoFileVuaKy; break;
                case MessageConstan.BanCoMuonTaoThemLuongKy: message = MessageViResource.BanCoMuonTaoThemLuongKy; break;
                case MessageConstan.BanKhongTheThucHienKyTaiTrangKyVuiLongChonKyTuTrangThu2TroDi: message = MessageViResource.BanKhongTheThucHienKyTaiTrangKyVuiLongChonKyTuTrangThu2TroDi; break;
                case MessageConstan.BenhNhanChuaCoTheThongMinh: message = MessageViResource.BenhNhanChuaCoTheThongMinh; break;
                case MessageConstan.BenhNhanDaCoTrongLuongKy: message = MessageViResource.BenhNhanDaCoTrongLuongKy; break;
                case MessageConstan.BenhNhanKhongCoTrongLuongKy: message = MessageViResource.BenhNhanKhongCoTrongLuongKy; break;
                case MessageConstan.BenhNhanKhongPhaiNguoiKyTiepTheoVuiLongKiemTraLai: message = MessageViResource.BenhNhanKhongPhaiNguoiKyTiepTheoVuiLongKiemTraLai; break;
                case MessageConstan.CoSuCoKhiTaiFileDaKyCuaBenhNhan: message = MessageViResource.CoSuCoKhiTaiFileDaKyCuaBenhNhan; break;
                case MessageConstan.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro: message = MessageViResource.CoSuCoXayRaVuiLongKiemTraLaiHoacLienHeVoiQuanTriHeThongDeDuocHoTro; break;
                case MessageConstan.DuLieuKhongHopLe: message = MessageViResource.DuLieuKhongHopLe; break;
                case MessageConstan.GuiVanBanLenHeThongEMRThatBai: message = MessageViResource.GuiVanBanLenHeThongEMRThatBai; break;
                case MessageConstan.KhongTimThayDuLieuDaKyCuaHoSo: message = MessageViResource.KhongTimThayDuLieuDaKyCuaHoSo; break;
                case MessageConstan.KhongTimThayFileDaKyCuaHoSoTrenHeThongFss: message = MessageViResource.KhongTimThayFileDaKyCuaHoSoTrenHeThongFss; break;
                case MessageConstan.KhongTimThayVanBanDeIn: message = MessageViResource.KhongTimThayVanBanDeIn; break;
                case MessageConstan.KhongTonTaiVanBanDeHuyKy: message = MessageViResource.KhongTonTaiVanBanDeHuyKy; break;
                case MessageConstan.KhongXacDinhDuocDuLieuDeHuyKy: message = MessageViResource.KhongXacDinhDuocDuLieuDeHuyKy; break;
                case MessageConstan.MaHoSoDieuTriKhongHopLe: message = MessageViResource.MaHoSoDieuTriKhongHopLe; break;
                case MessageConstan.PhaiTaoLuongKyChoVanBanDaCoTrenHeThong: message = MessageViResource.PhaiTaoLuongKyChoVanBanDaCoTrenHeThong; break;
                case MessageConstan.TaiKhoanChuaDuocTaoNguoiKyTrenHeThongEMR: message = MessageViResource.TaiKhoanChuaDuocTaoNguoiKyTrenHeThongEMR; break;
                case MessageConstan.ThongBao: message = MessageViResource.ThongBao; break;
                case MessageConstan.TinhNangChiDanhChoBenhAnDienTu: message = MessageViResource.TinhNangChiDanhChoBenhAnDienTu; break;
                case MessageConstan.TinhNangChiDanhChoBenhAnDienTuCoThamSo: message = MessageViResource.TinhNangChiDanhChoBenhAnDienTuCoThamSo; break;
                case MessageConstan.VanBanCoTheDaTonTaiTrenHeThongEMRBanCoThucHienKhong: message = MessageViResource.VanBanCoTheDaTonTaiTrenHeThongEMRBanCoThucHienKhong; break;
                case MessageConstan.ChuaChonMoiQuanHeVoiBenhNhan: message = MessageViResource.ChuaChonMoiQuanHeVoiBenhNhan; break;
                case MessageConstan.LoiGoiServiceXacThucVanTay: message = MessageViResource.LoiGoiServiceXacThucVanTay; break;
                case MessageConstan.TaiKhoanThieuThongTinAnh: message = MessageViResource.TaiKhoanThieuThongTinAnh; break;
                case MessageConstan.TaiKhoanThieuThongTinAnhBanCoMuonBoHienThiAnh: message = MessageViResource.TaiKhoanThieuThongTinAnhBanCoMuonBoHienThiAnh; break;
                case MessageConstan.TuDongCapNhatChuKyThatBaiBanCoMuonTiepTucVaBoHienThiChuKy: message = MessageViResource.TuDongCapNhatChuKyThatBaiBanCoMuonTiepTucVaBoHienThiChuKy; break;
                case MessageConstan.TaiKhoanThieuThongTinAnhHeThongTuDongCapNhat: message = MessageViResource.TaiKhoanThieuThongTinAnhHeThongTuDongCapNhat; break;
                case MessageConstan.ResetTrangThaiNguoiDungDaLuuTaiMayTram: message = MessageViResource.ResetTrangThaiNguoiDungDaLuuTaiMayTram; break;
                case MessageConstan.VanBanChuaDuocThietLapNghiepVuKy: message = MessageViResource.VanBanChuaDuocThietLapNghiepVuKy; break;
                case MessageConstan.VanBanDaTonTaiPhanMemSeHienThiVanBanCu: message = MessageViResource.VanBanDaTonTaiPhanMemSeHienThiVanBanCu; break;
                case MessageConstan.KySuDungUSBTokenThatBai: message = MessageViResource.KySuDungUSBTokenThatBai; break;
                case MessageConstan.BenhNhanKhongCoTheKCB: message = MessageViResource.BenhNhanKhongCoTheKCB; break;
                case MessageConstan.ChuKyDienTuBenhNhanDaKy: message = MessageViResource.ChuKyDienTuBenhNhanDaKy; break;
                case MessageConstan.KyVanBanPhuThocThatBai__KhongPhaiLuotKyCuaBan: message = MessageViResource.KyVanBanPhuThocThatBai__KhongPhaiLuotKyCuaBan; break;
                case MessageConstan.VanBanDaTonTaiKhongTheKyTiepDoThietLapDangChan: message = MessageViResource.VanBanDaTonTaiKhongTheKyTiepDoThietLapDangChan; break;
                case MessageConstan.KhongNhapTTDinhDanhHoacChonTheKy: message = MessageViResource.KhongNhapTTDinhDanhHoacChonTheKy; break;
                case MessageConstan.KhongTimThayChuKyKhiSuDungBangKy: message = MessageViResource.KhongTimThayChuKyKhiSuDungBangKy; break;
                case MessageConstan.TinhNangKySuDungBangKyChuaDuocHoTro: message = MessageViResource.TinhNangKySuDungBangKyChuaDuocHoTro; break;
                default: break;
            }

            return message;
        }
    }
}
