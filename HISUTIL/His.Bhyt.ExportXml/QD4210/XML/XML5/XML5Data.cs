using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD4210.XML.XML5
{
    [XmlRoot("DSACH_CHI_TIET_DIEN_BIEN_BENH")]
    public class XML5Data
    {
        [XmlElement("CHI_TIET_DIEN_BIEN_BENH")]
        public List<XML5DetailData> CHI_TIET_DIEN_BIEN_BENH { get; set; }
    }
}
