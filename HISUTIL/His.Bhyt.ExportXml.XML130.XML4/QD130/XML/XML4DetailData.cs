using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.XML130.XML4.QD130.XML
{
    [Serializable]
    public class XML4DetailData
    {
        [XmlElement(Order = 1)]
        public string MA_LK { get; set; }

        [XmlElement(Order = 2)]
        public int STT { get; set; }

        [XmlElement(Order = 3)]
        public string MA_DICH_VU { get; set; }

        [XmlElement(Order = 4)]
        public string MA_CHI_SO { get; set; }

        [XmlElement(Order = 5)]
        public XmlCDataSection TEN_CHI_SO { get; set; }

        [XmlElement(Order = 6)]
        public XmlCDataSection GIA_TRI { get; set; }

        [XmlElement(Order = 7)]
        public XmlCDataSection DON_VI_DO { get; set; }

        [XmlElement(Order = 8)]
        public XmlCDataSection MO_TA { get; set; }

        [XmlElement(Order = 9)]
        public XmlCDataSection KET_LUAN { get; set; }

        [XmlElement(Order = 10)]
        public string NGAY_KQ { get; set; }
        [XmlElement(Order = 11)]
        public string MA_BS_DOC_KQ { get; set; }
        [XmlElement(Order = 12)]
        public string DU_PHONG { get; set; }
    }
}
