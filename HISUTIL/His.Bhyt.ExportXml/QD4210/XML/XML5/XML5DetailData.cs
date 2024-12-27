using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD4210.XML.XML5
{
    [Serializable]
    public class XML5DetailData
    {
        [XmlElement(Order = 1)]
        public string MA_LK { get; set; }

        [XmlElement(Order = 2)]
        public int STT { get; set; }

        [XmlElement(Order = 3)]
        public XmlCDataSection DIEN_BIEN { get; set; }

        [XmlElement(Order = 4)]
        public XmlCDataSection HOI_CHAN { get; set; }

        [XmlElement(Order = 5)]
        public XmlCDataSection PHAU_THUAT { get; set; }

        [XmlElement(Order = 6)]
        public string NGAY_YL { get; set; }
    }
}
