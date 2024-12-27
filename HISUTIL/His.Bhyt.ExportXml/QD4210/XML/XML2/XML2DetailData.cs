using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD4210.XML.XML2
{
    [Serializable]
    public class XML2DetailData
    {
        [XmlElement(Order = 1)]
        public string MA_LK { get; set; }

        [XmlElement(Order = 2)]
        public int STT { get; set; }

        [XmlElement(Order = 3)]
        public string MA_THUOC { get; set; }

        [XmlElement(Order = 4)]
        public string MA_NHOM { get; set; }

        [XmlElement(Order = 5)]
        public XmlCDataSection TEN_THUOC { get; set; }

        [XmlElement(Order = 6)]
        public string DON_VI_TINH { get; set; }

        [XmlElement(Order = 7)]
        public XmlCDataSection HAM_LUONG { get; set; }

        [XmlElement(Order = 8)]
        public string DUONG_DUNG { get; set; }

        [XmlElement(Order = 9)]
        public XmlCDataSection LIEU_DUNG { get; set; }

        [XmlElement(Order = 10)]
        public string SO_DANG_KY { get; set; }

        [XmlElement(Order = 11)]
        public string TT_THAU { get; set; }

        [XmlElement(Order = 12)]
        public int PHAM_VI { get; set; }

        [XmlElement(Order = 13)]
        public string TYLE_TT { get; set; }

        [XmlElement(Order = 14)]
        public string SO_LUONG { get; set; }

        [XmlElement(Order = 15)]
        public string DON_GIA { get; set; }

        [XmlElement(Order = 16)]
        public string THANH_TIEN { get; set; }

        [XmlElement(Order = 17)]
        public int MUC_HUONG { get; set; }

        [XmlElement(Order = 18)]
        public string T_NGUONKHAC { get; set; }

        [XmlElement(Order = 19)]
        public string T_BNTT { get; set; }

        [XmlElement(Order = 20)]
        public string T_BHTT { get; set; }

        [XmlElement(Order = 21)]
        public string T_BNCCT { get; set; }

        [XmlElement(Order = 22)]
        public string T_NGOAIDS { get; set; }

        [XmlElement(Order = 23)]
        public string MA_KHOA { get; set; }

        [XmlElement(Order = 24)]
        public string MA_BAC_SI { get; set; }

        [XmlElement(Order = 25)]
        public string MA_BENH { get; set; }

        [XmlElement(Order = 26)]
        public string NGAY_YL { get; set; }

        [XmlElement(Order = 27)]
        public int MA_PTTT { get; set; }
    }
}
