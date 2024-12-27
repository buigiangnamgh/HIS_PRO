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
    public class XMLCT06Data
    {
        [XmlElement(Order = 1)]
        public string SO_SERI { get; set; }

        [XmlElement(Order = 2)]
        public string MA_CT { get; set; }

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
        public XmlCDataSection TEN_DVI { get; set; }

        [XmlElement(Order = 9)]
        public XmlCDataSection CHAN_DOAN { get; set; }

        [XmlElement(Order = 10)]
        public string NGAY_VAO { get; set; }

        [XmlElement(Order = 11)]
        public string NGAY_RA { get; set; }

        [XmlElement(Order = 12)]
        public XmlCDataSection NGUOI_DAI_DIEN { get; set; }

        [XmlElement(Order = 13)]
        public XmlCDataSection TEN_BS { get; set; }

        [XmlElement(Order = 14)]
        public string MA_BS { get; set; }

        [XmlElement(Order = 15)]
        public string NGAY_CT { get; set; }
    }
}
