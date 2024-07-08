using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.XML130.XML1.QD130.XML
{
    [XmlRoot("TONG_HOP", IsNullable = true)]
    public class XML1Data
    {
        [XmlElement(Order = 1)]
        public string MA_LK { get; set; }

        [XmlElement(Order = 2)]
        public int STT { get; set; }

        [XmlElement(Order = 3)]
        public string MA_BN { get; set; }

        [XmlElement(Order = 4)]
        public XmlCDataSection HO_TEN { get; set; }
        [XmlElement(Order = 5)]
        public string SO_CCCD { get; set; }

        [XmlElement(Order = 6)]
        public string NGAY_SINH { get; set; }

        [XmlElement(Order = 7)]
        public string GIOI_TINH { get; set; }

        [XmlElement(Order = 8)]
        public string NHOM_MAU { get; set; }

        [XmlElement(Order = 9)]
        public string MA_QUOCTICH { get; set; }

        [XmlElement(Order = 10)]
        public string MA_DANTOC { get; set; }

        [XmlElement(Order = 11)]
        public string MA_NGHE_NGHIEP { get; set; }

        [XmlElement(Order = 12)]
        public XmlCDataSection DIA_CHI { get; set; }

        [XmlElement(Order = 13)]
        public string MATINH_CU_TRU { get; set; }

        [XmlElement(Order = 14)]
        public string MAHUYEN_CU_TRU { get; set; }

        [XmlElement(Order = 15)]
        public string MAXA_CU_TRU { get; set; }

        [XmlElement(Order = 16)]
        public string DIEN_THOAI { get; set; }

        [XmlElement(Order = 17)]
        public string MA_THE_BHYT { get; set; }

        [XmlElement(Order = 18)]
        public string MA_DKBD { get; set; }

        [XmlElement(Order = 19)]
        public string GT_THE_TU { get; set; }

        [XmlElement(Order = 20)]
        public string GT_THE_DEN { get; set; }

        [XmlElement(Order = 21)]
        public string NGAY_MIEN_CCT { get; set; }

        [XmlElement(Order = 22)]
        public XmlCDataSection LY_DO_VV { get; set; }

        [XmlElement(Order = 23)]
        public XmlCDataSection LY_DO_VNT { get; set; }

        [XmlElement(Order = 24)]
        public string MA_LY_DO_VNT { get; set; }

        [XmlElement(Order = 25)]
        public XmlCDataSection CHAN_DOAN_VAO { get; set; }

        [XmlElement(Order = 26)]
        public XmlCDataSection CHAN_DOAN_RV { get; set; }

        [XmlElement(Order = 27)]
        public string MA_BENH_CHINH { get; set; }

        [XmlElement(Order = 28)]
        public string MA_BENH_KT { get; set; }

        [XmlElement(Order = 29)]
        public string MA_BENH_YHCT { get; set; }

        [XmlElement(Order = 30)]
        public string MA_PTTT_QT { get; set; }

        [XmlElement(Order = 31)]
        public string MA_DOITUONG_KCB { get; set; }

        [XmlElement(Order = 32)]
        public string MA_NOI_DI { get; set; }

        [XmlElement(Order = 33)]
        public string MA_NOI_DEN { get; set; }

        [XmlElement(Order = 34)]
        public string MA_TAI_NAN { get; set; }

        [XmlElement(Order = 35)]
        public string NGAY_VAO { get; set; }

        [XmlElement(Order = 36)]
        public string NGAY_VAO_NOI_TRU { get; set; }

        [XmlElement(Order = 37)]
        public string NGAY_RA { get; set; }

        [XmlElement(Order = 38)]
        public string GIAY_CHUYEN_TUYEN { get; set; }

        [XmlElement(Order = 39)]
        public long SO_NGAY_DTRI { get; set; }

        [XmlElement(Order = 40)]
        public string PP_DIEU_TRI { get; set; }

        [XmlElement(Order = 41)]
        public string KET_QUA_DTRI { get; set; }

        [XmlElement(Order = 42)]
        public string MA_LOAI_RV { get; set; }

        [XmlElement(Order = 43)]
        public XmlCDataSection GHI_CHU { get; set; }

        [XmlElement(Order = 44)]
        public string NGAY_TTOAN { get; set; }

        [XmlElement(Order = 45)]
        public string T_THUOC { get; set; }

        [XmlElement(Order = 46)]
        public string T_VTYT { get; set; }

        [XmlElement(Order = 47)]
        public string T_TONGCHI_BV { get; set; }

        [XmlElement(Order = 48)]
        public string T_TONGCHI_BH { get; set; }

        [XmlElement(Order = 49)]
        public string T_BNTT { get; set; }

        [XmlElement(Order = 50)]
        public string T_BNCCT { get; set; }

        [XmlElement(Order = 51)]
        public string T_BHTT { get; set; }

        [XmlElement(Order = 52)]
        public string T_NGUONKHAC { get; set; }

        [XmlElement(Order = 53)]
        public string T_BHTT_GDV { get; set; }

        [XmlElement(Order = 54)]
        public string NAM_QT { get; set; }

        [XmlElement(Order = 55)]
        public string THANG_QT { get; set; }

        [XmlElement(Order = 56)]
        public string MA_LOAI_KCB { get; set; }

        [XmlElement(Order = 57)]
        public string MA_KHOA { get; set; }

        [XmlElement(Order = 58)]
        public string MA_CSKCB { get; set; }

        [XmlElement(Order = 59)]
        public string MA_KHUVUC { get; set; }

        [XmlElement(Order = 60)]
        public string CAN_NANG { get; set; }

        [XmlElement(Order = 61)]
        public string CAN_NANG_CON { get; set; }

        [XmlElement(Order = 62)]
        public string NAM_NAM_LIEN_TUC { get; set; }

        [XmlElement(Order = 63)]
        public string NGAY_TAI_KHAM { get; set; }

        [XmlElement(Order = 64)]
        public string MA_HSBA { get; set; }

        [XmlElement(Order = 65)]
        public string MA_TTDV { get; set; }

        [XmlElement(Order = 66)]
        public string DU_PHONG { get; set; }
    }
}
