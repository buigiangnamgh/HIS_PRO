using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD917.XML.XML3
{
    [XmlRoot("DSACH_CHI_TIET_DVKT")]
    public class XML3ListDetailData
    {
        [XmlElement("CHI_TIET_DVKT")]
        public List<XML3DetailData> CHI_TIET_DVKT { get; set; }
    }
}
