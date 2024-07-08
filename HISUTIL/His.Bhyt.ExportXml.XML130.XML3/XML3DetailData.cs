using System;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.XML130.XML3
{
    [Serializable]
    public class XML3DetailData
    {
        [XmlElement(Order = 1)]
        public string MA_LK { get; set; }

        [XmlElement(Order = 2)]
        public int STT { get; set; }

        [XmlElement(Order = 3)]
        public string MA_DICH_VU { get; set; }

        [XmlElement(Order = 4)]
        public string MA_PTTT_QT { get; set; }

        [XmlElement(Order = 5)]
        public string MA_VAT_TU { get; set; }

        [XmlElement(Order = 6)]
        public string MA_NHOM { get; set; }

        [XmlElement(Order = 7)]
        public string GOI_VTYT { get; set; }

        [XmlElement(Order = 8)]
        public XmlCDataSection TEN_VAT_TU { get; set; }

        [XmlElement(Order = 9)]
        public XmlCDataSection TEN_DICH_VU { get; set; }

        [XmlElement(Order = 10)]
        public string MA_XANG_DAU { get; set; }

        [XmlElement(Order = 11)]
        public string DON_VI_TINH { get; set; }

        [XmlElement(Order = 12)]
        public string PHAM_VI { get; set; }

        [XmlElement(Order = 13)]
        public string SO_LUONG { get; set; }

        [XmlElement(Order = 14)]
        public string DON_GIA_BV { get; set; }

        [XmlElement(Order = 15)]
        public string DON_GIA_BH { get; set; }

        [XmlElement(Order = 16)]
        public XmlCDataSection TT_THAU { get; set; }

        [XmlElement(Order = 17)]
        public string TYLE_TT_DV { get; set; }

        [XmlElement(Order = 18)]
        public string TYLE_TT_BH { get; set; }

        [XmlElement(Order = 19)]
        public string THANH_TIEN_BV { get; set; }

        [XmlElement(Order = 20)]
        public string THANH_TIEN_BH { get; set; }

        [XmlElement(Order = 21)]
        public string T_TRANTT { get; set; }

        [XmlElement(Order = 22)]
        public int MUC_HUONG { get; set; }

        [XmlElement(Order = 23)]
        public string T_NGUONKHAC_NSNN { get; set; }

        [XmlElement(Order = 24)]
        public string T_NGUONKHAC_VTNN { get; set; }

        [XmlElement(Order = 25)]
        public string T_NGUONKHAC_VTTN { get; set; }

        [XmlElement(Order = 26)]
        public string T_NGUONKHAC_CL { get; set; }

        [XmlElement(Order = 27)]
        public string T_NGUONKHAC { get; set; }

        [XmlElement(Order = 28)]
        public string T_BHTT { get; set; }

        [XmlElement(Order = 29)]
        public string T_BNTT { get; set; }

        [XmlElement(Order = 30)]
        public string T_BNCCT { get; set; }

        [XmlElement(Order = 31)]
        public string MA_KHOA { get; set; }

        [XmlElement(Order = 32)]
        public string MA_GIUONG { get; set; }

        [XmlElement(Order = 33)]
        public string MA_BAC_SI { get; set; }

        [XmlElement(Order = 34)]
        public string NGUOI_THUC_HIEN { get; set; }

        [XmlElement(Order = 35)]
        public string MA_BENH { get; set; }

        [XmlElement(Order = 36)]
        public string MA_BENH_YHCT { get; set; }

        [XmlElement(Order = 37)]
        public string NGAY_YL { get; set; }

        [XmlElement(Order = 38)]
        public string NGAY_TH_YL { get; set; }

        [XmlElement(Order = 39)]
        public string NGAY_KQ { get; set; }

        [XmlElement(Order = 40)]
        public int MA_PTTT { get; set; }

        [XmlElement(Order = 41)]
        public string VET_THUONG_TP { get; set; }

        [XmlElement(Order = 42)]
        public string PP_VO_CAM { get; set; }

        [XmlElement(Order = 43)]
        public string VI_TRI_TH_DVKT { get; set; }

        [XmlElement(Order = 44)]
        public string MA_MAY { get; set; }

        [XmlElement(Order = 45)]
        public string MA_HIEU_SP { get; set; }

        [XmlElement(Order = 46)]
        public string TAI_SU_DUNG { get; set; }

        [XmlElement(Order = 47)]
        public string DU_PHONG { get; set; }
    }
}
