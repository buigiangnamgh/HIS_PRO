using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CV2076.XML
{
    //[XmlRoot("NOIDUNGFILE", IsNullable = true)]
    public class XMLCT07Data
    {
        [XmlElement(Order = 1)]
        public string MA_CT { get; set; }

        [XmlElement(Order = 2)]
        public string SO_SERI { get; set; }

        [XmlElement(Order = 3)]
        public string SO_KCB { get; set; }

        [XmlElement(Order = 4)]
        public string MA_BHXH { get; set; }

        [XmlElement(Order = 5)]
        public string MA_THE { get; set; }

        [XmlElement(Order = 6)]
        public XmlCDataSection HO_TEN { get; set; }

        [XmlElement(Order = 7)]
        public string NGAY_SINH { get; set; }

        [XmlElement(Order = 8)]
        public string GIOI_TINH { get; set; }

        [XmlElement(Order = 9)]
        public XmlCDataSection DON_VI { get; set; }

        [XmlElement(Order = 10)]
        public XmlCDataSection CHANDOAN_DIEUTRI { get; set; }

        [XmlElement(Order = 11)]
        public string TU_NGAY { get; set; }

        [XmlElement(Order = 12)]
        public string DEN_NGAY { get; set; }

        [XmlElement(Order = 13)]
        public XmlCDataSection HO_TEN_CHA { get; set; }

        [XmlElement(Order = 14)]
        public XmlCDataSection HO_TEN_ME { get; set; }

        [XmlElement(Order = 15)]
        public XmlCDataSection THU_TRUONG_DV { get; set; }

        [XmlElement(Order = 16)]
        public string MA_CCHN { get; set; }

        [XmlElement(Order = 17)]
        public XmlCDataSection TEN_NGUOI_HANH_NGHE { get; set; }

        [XmlElement(Order = 18)]
        public string NGAY_CHUNG_TU { get; set; }

        [XmlElement(Order = 19)]
        public string TEKT { get; set; }

        [XmlElement(Order = 20)]
        public string MAU_SO { get; set; }

    }
}
