using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.XML130.XML3
{
    [XmlRoot("CHITIEU_CHITIET_DVKT_VTYT")]
    public class XML3Data
    {
        [XmlElement("DSACH_CHI_TIET_DVKT")]
        public DsChiTietDVKT DSACH_CHI_TIET_DVKT { get; set; }
        public string ToXML()
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stringwriter, this);
                return stringwriter.ToString();
            }
        }

        public static XML3Data LoadFromXMLString(string xmlText)
        {
            using (var stringReader = new System.IO.StringReader(RemoveByteOrderMark(xmlText)))
            {
                var serializer = new XmlSerializer(typeof(XML3Data), new XmlRootAttribute("CHITIEU_CHITIET_DVKT_VTYT"));
                return serializer.Deserialize(stringReader) as XML3Data;
            }
        }

        public static string RemoveByteOrderMark(string XML)
        {
            string byteOrderMark = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (XML.StartsWith(byteOrderMark))
            {
                XML = XML.Remove(0, byteOrderMark.Length);
            }
            return XML;
        }
    }
    [XmlRoot("DSACH_CHI_TIET_DVKT")]
    public class DsChiTietDVKT
    {
        [XmlElement("CHI_TIET_DVKT")]
        public List<XML3DetailData> CHI_TIET_DVKT { get; set; }
    }
}
