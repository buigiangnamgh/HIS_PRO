using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.XML130.XML2.QD130.XML
{
    [Serializable]
    public class XML2DetailData
    {
        [XmlElement(Order = 1)]
        public string MA_LK { get; set; }
        [XmlElement(Order = 2)]
        public string STT { get; set; }
        [XmlElement(Order = 3)]
        public string MA_THUOC { get; set; }
        [XmlElement(Order = 4)]
        public string MA_PP_CHEBIEN { get; set; }
        [XmlElement(Order = 5)]
        public string MA_CSKCB_THUOC { get; set; }
        [XmlElement(Order = 6)]
        public string MA_NHOM { get; set; }
        [XmlElement(Order = 7)]
        public XmlCDataSection TEN_THUOC { get; set; }
        [XmlElement(Order = 8)]
        public string DON_VI_TINH { get; set; }
        [XmlElement(Order = 9)]
        public XmlCDataSection HAM_LUONG { get; set; }
        [XmlElement(Order = 10)]
        public XmlCDataSection DUONG_DUNG { get; set; }
        [XmlElement(Order = 11)]
        public string DANG_BAO_CHE { get; set; }
        [XmlElement(Order = 12)]
        public XmlCDataSection LIEU_DUNG { get; set; }
        [XmlElement(Order = 13)]
        public XmlCDataSection CACH_DUNG { get; set; }
        [XmlElement(Order = 14)]
        public string SO_DANG_KY { get; set; }
        [XmlElement(Order = 15)]
        public string TT_THAU { get; set; }
        [XmlElement(Order = 16)]
        public string PHAM_VI { get; set; }
        [XmlElement(Order = 17)]
        public string TYLE_TT_BH { get; set; }
        [XmlElement(Order = 18)]
        public string SO_LUONG { get; set; }
        [XmlElement(Order = 19)]
        public string DON_GIA { get; set; }
        [XmlElement(Order = 20)]
        public string THANH_TIEN_BV { get; set; }
        [XmlElement(Order = 21)]
        public string THANH_TIEN_BH { get; set; }
        [XmlElement(Order = 22)]
        public string T_NGUONKHAC_NSNN { get; set; }
        [XmlElement(Order = 23)]
        public string T_NGUONKHAC_VTNN { get; set; }
        [XmlElement(Order = 24)]
        public string T_NGUONKHAC_VTTN { get; set; }
        [XmlElement(Order = 25)]
        public string T_NGUONKHAC_CL { get; set; }
        [XmlElement(Order = 26)]
        public string T_NGUONKHAC { get; set; }
        [XmlElement(Order = 27)]
        public string MUC_HUONG { get; set; }
        [XmlElement(Order = 28)]
        public string T_BHTT { get; set; }
        [XmlElement(Order = 29)]
        public string T_BNCCT { get; set; }
        [XmlElement(Order = 30)]
        public string T_BNTT { get; set; }
        [XmlElement(Order = 31)]
        public string MA_KHOA { get; set; }
        [XmlElement(Order = 32)]
        public string MA_BAC_SI { get; set; }
        [XmlElement(Order = 33)]
        public string MA_DICH_VU { get; set; }
        [XmlElement(Order = 34)]
        public string NGAY_YL { get; set; }
        [XmlElement(Order = 35)]
        public string NGAY_TH_YL { get; set; }
        [XmlElement(Order = 36)]
        public string MA_PTTT { get; set; }
        [XmlElement(Order = 37)]
        public string NGUON_CTRA { get; set; }
        [XmlElement(Order = 38)]
        public string VET_THUONG_TP { get; set; }
        [XmlElement(Order = 39)]
        public string DU_PHONG { get; set; }
    }
}
