using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD917.XML.XML3
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
        public string MA_VAT_TU { get; set; }

        [XmlElement(Order = 5)]
        public string MA_NHOM { get; set; }

        [XmlElement(Order = 6)]
        public XmlCDataSection TEN_DICH_VU { get; set; }

        [XmlElement(Order = 7)]
        public string DON_VI_TINH { get; set; }

        [XmlElement(Order = 8)]
        public string SO_LUONG { get; set; }

        [XmlElement(Order = 9)]
        public string DON_GIA { get; set; }

        [XmlElement(Order = 10)]
        public string TYLE_TT { get; set; }

        [XmlElement(Order = 11)]
        public string THANH_TIEN { get; set; }

        [XmlElement(Order = 12)]
        public string MA_KHOA { get; set; }

        [XmlElement(Order = 13)]
        public string MA_BAC_SI { get; set; }

        [XmlElement(Order = 14)]
        public string MA_BENH { get; set; }

        [XmlElement(Order = 15)]
        public string NGAY_YL { get; set; }

        [XmlElement(Order = 16)]
        public string NGAY_KQ { get; set; }

        [XmlElement(Order = 17)]
        public int MA_PTTT { get; set; }
    }
}
