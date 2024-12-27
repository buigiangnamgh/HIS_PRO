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
    public class XMLCT04Data
    {
        [XmlElement(Order = 1)]
        public string MA_CT { get; set; }

        [XmlElement(Order = 2)]
        public string SO_SERI { get; set; }

        [XmlElement(Order = 3)]
        public string MA_BHXH { get; set; }

        [XmlElement(Order = 4)]
        public string MA_THE { get; set; }

        [XmlElement(Order = 5)]
        public XmlCDataSection HO_TEN { get; set; }

        [XmlElement(Order = 6)]
        public string NGAY_SINH { get; set; }

        [XmlElement(Order = 7)]
        public string GIOI_TINH { get; set; }

        [XmlElement(Order = 8)]
        public string MA_DANTOC { get; set; }

        [XmlElement(Order = 9)]
        public XmlCDataSection DIA_CHI { get; set; }

        [XmlElement(Order = 10)]
        public XmlCDataSection NGHE_NGHIEP { get; set; }

        [XmlElement(Order = 11)]
        public XmlCDataSection HO_TEN_CHA { get; set; }

        [XmlElement(Order = 12)]
        public XmlCDataSection HO_TEN_ME { get; set; }

        [XmlElement(Order = 13)]
        public XmlCDataSection NGUOI_GIAM_HO { get; set; }

        [XmlElement(Order = 14)]
        public XmlCDataSection TEN_DONVI { get; set; }

        [XmlElement(Order = 15)]
        public string NGAY_VAO { get; set; }

        [XmlElement(Order = 16)]
        public string NGAY_RA { get; set; }

        [XmlElement(Order = 17)]
        public XmlCDataSection CHAN_DOAN_VAO { get; set; }

        [XmlElement(Order = 18)]
        public XmlCDataSection CHAN_DOAN_RA { get; set; }

        [XmlElement(Order = 19)]
        public XmlCDataSection QT_BENHLY { get; set; }

        [XmlElement(Order = 20)]
        public XmlCDataSection TOMTAT_KQ { get; set; }

        [XmlElement(Order = 21)]
        public XmlCDataSection PP_DIEUTRI { get; set; }

        [XmlElement(Order = 22)]
        public string NGAY_SINHCON { get; set; }

        [XmlElement(Order = 23)]
        public string NGAY_CHETCON { get; set; }

        [XmlElement(Order = 24)]
        public string SO_CONCHET { get; set; }

        [XmlElement(Order = 25)]
        public string TT_RAVIEN { get; set; }

        [XmlElement(Order = 26)]
        public XmlCDataSection GHI_CHU { get; set; }

        [XmlElement(Order = 27)]
        public XmlCDataSection NGUOI_DAI_DIEN { get; set; }

        [XmlElement(Order = 28)]
        public string NGAY_CT { get; set; }

        [XmlElement(Order = 29)]
        public string TEKT { get; set; }
    }
}
