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
    public class XMLCT05Data
    {
        [XmlElement(Order = 1)]
        public string MA_CT { get; set; }

        [XmlElement(Order = 2)]
        public string SO_SERI { get; set; }

        [XmlElement(Order = 3)]
        public string MA_BHXH_NND { get; set; }

        [XmlElement(Order = 4)]
        public string MA_THE_NND { get; set; }

        [XmlElement(Order = 5)]
        public XmlCDataSection HO_TEN_NND { get; set; }

        [XmlElement(Order = 6)]
        public string NGAYSINH_NND { get; set; }

        [XmlElement(Order = 7)]
        public string MA_DANTOC_NND { get; set; }

        [XmlElement(Order = 8)]
        public string SO_CMND_NND { get; set; }

        [XmlElement(Order = 9)]
        public string NGAYCAP_CMND_NND { get; set; }

        [XmlElement(Order = 10)]
        public XmlCDataSection NOICAP_CMND_NND { get; set; }

        [XmlElement(Order = 11)]
        public XmlCDataSection NOI_DK_THUONGTRU_NND { get; set; }

        [XmlElement(Order = 12)]
        public XmlCDataSection HO_TEN_CHA { get; set; }

        [XmlElement(Order = 13)]
        public XmlCDataSection TEN_CON { get; set; }

        [XmlElement(Order = 14)]
        public string GOI_TINH_CON { get; set; }

        [XmlElement(Order = 15)]
        public string SO_CON { get; set; }

        [XmlElement(Order = 16)]
        public string CAN_NANG_CON { get; set; }

        [XmlElement(Order = 17)]
        public string NGAY_SINH_CON { get; set; }

        [XmlElement(Order = 18)]
        public XmlCDataSection NOI_SINH_CON { get; set; }

        [XmlElement(Order = 19)]
        public XmlCDataSection TINH_TRANG_CON { get; set; }

        [XmlElement(Order = 20)]
        public string SINHCON_PHAUTHUAT { get; set; }

        [XmlElement(Order = 21)]
        public string SINHCON_DUOI32TUAN { get; set; }

        [XmlElement(Order = 22)]
        public XmlCDataSection GHI_CHU { get; set; }

        [XmlElement(Order = 23)]
        public XmlCDataSection NGUOI_DO_DE { get; set; }

        [XmlElement(Order = 24)]
        public XmlCDataSection NGUOI_GHI_PHIEU { get; set; }

        [XmlElement(Order = 25)]
        public XmlCDataSection THU_TRUONG_DVI { get; set; }

        [XmlElement(Order = 26)]
        public string NGAY_CT { get; set; }

        [XmlElement(Order = 27)]
        public string SO { get; set; }

        [XmlElement(Order = 28)]
        public XmlCDataSection QUYEN_SO { get; set; }
    }
}
