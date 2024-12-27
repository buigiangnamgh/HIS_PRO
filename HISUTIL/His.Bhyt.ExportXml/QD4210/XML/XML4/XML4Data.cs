using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD4210.XML.XML4
{
    [XmlRoot("DSACH_CHI_TIET_CLS")]
    public class XML4Data
    {
        [XmlElement("CHI_TIET_CLS")]
        public List<XML4DetailData> CHI_TIET_CLS { get; set; }
    }
}
