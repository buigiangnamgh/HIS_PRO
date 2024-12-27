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
    public class XMLCT03Data
    {
        [XmlElement(Order = 1)]
        public string SO_LUU_TRU { get; set; }

        [XmlElement(Order = 2)]
        public string MA_YTE { get; set; }

        [XmlElement(Order = 3)]
        public string MA_KHOA { get; set; }

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
        public string MA_DANTOC { get; set; }

        [XmlElement(Order = 10)]
        public XmlCDataSection NGHE_NGHIEP { get; set; }

        [XmlElement(Order = 11)]
        public XmlCDataSection DIA_CHI { get; set; }

        [XmlElement(Order = 12)]
        public string NGAY_VAO { get; set; }

        [XmlElement(Order = 13)]
        public string NGAY_RA { get; set; }

        [XmlElement(Order = 14)]
        public string DINH_CHI_THAI_NGHEN { get; set; }

        [XmlElement(Order = 15)]
        public string TUOI_THAI { get; set; }

        [XmlElement(Order = 16)]
        public XmlCDataSection CHAN_DOAN { get; set; }

        [XmlElement(Order = 17)]
        public XmlCDataSection PP_DIEUTRI { get; set; }

        [XmlElement(Order = 18)]
        public XmlCDataSection GHI_CHU { get; set; }

        [XmlElement(Order = 19)]
        public XmlCDataSection THU_TRUONG_DVI { get; set; }

        [XmlElement(Order = 20)]
        public string MA_CCHN_TRUONGKHOA { get; set; }

        [XmlElement(Order = 21)]
        public XmlCDataSection TEN_TRUONGKHOA { get; set; }

        [XmlElement(Order = 22)]
        public string NGAY_CHUNG_TU { get; set; }

        [XmlElement(Order = 23)]
        public string TEKT { get; set; }

        [XmlElement(Order = 24)]
        public XmlCDataSection HO_TEN_CHA { get; set; }

        [XmlElement(Order = 25)]
        public XmlCDataSection HO_TEN_ME { get; set; }

        [XmlElement(Order = 26)]
        public string NGOAITRU_TUNGAY { get; set; }

        [XmlElement(Order = 27)]
        public string NGOAITRU_DENNGAY { get; set; }

    }
}
