using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CheckIn.XML
{
    [XmlRoot("CHECKIN")]
    public class CheckIn
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
        public int GIOI_TINH { get; set; }

        [XmlElement(Order = 8)]
        public string MA_THE_BHYT { get; set; }

        [XmlElement(Order = 9)]
        public string MA_DKBD { get; set; }

        [XmlElement(Order = 10)]
        public string GT_THE_TU { get; set; }

        [XmlElement(Order = 11)]
        public string GT_THE_DEN { get; set; }

        [XmlElement(Order = 12)]
        public string MA_DOITUONG_KCB { get; set; }

        [XmlElement(Order = 13)]
        public string NGAY_VAO { get; set; }

        [XmlElement(Order = 14)]
        public string NGAY_VAO_NOI_TRU { get; set; }

        [XmlElement(Order = 15)]
        public XmlCDataSection LY_DO_VNT { get; set; }

        [XmlElement(Order = 16)]
        public string MA_LY_DO_VNT { get; set; }

        [XmlElement(Order = 17)]
        public string MA_LOAI_KCB { get; set; }

        [XmlElement(Order = 18)]
        public string MA_CSKCB { get; set; }

        [XmlElement(Order = 19)]
        public string MA_DICH_VU { get; set; }

        [XmlElement(Order = 20)]
        public XmlCDataSection TEN_DICH_VU { get; set; }

        [XmlElement(Order = 21)]
        public string NGAY_YL { get; set; }

        [XmlElement(Order = 22)]
        public string DU_PHONG { get; set; }

    }
}
