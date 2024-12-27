using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD917.XML.XML2
{
    [XmlRoot("DSACH_CHI_TIET_THUOC")]
    public class XML2ListDetailData
    {
        [XmlElement("CHI_TIET_THUOC")]
        public List<XML2DetailData> CHI_TIET_THUOC { get; set; }
    }
}
