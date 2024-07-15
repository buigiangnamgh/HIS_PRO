using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.XML130.XML7
{
    [XmlRoot("CHI_TIEU_DU_LIEU_GIAY_RA_VIEN", IsNullable = true)]
    public class XML7Data
    {
        [XmlElement(Order = 1)]
        public string MA_LK { get; set; }

        [XmlElement(Order = 2)]
        public string SO_LUU_TRU { get; set; }

        [XmlElement(Order = 3)]
        public string MA_YTE { get; set; }

        [XmlElement(Order = 4)]
        public string MA_KHOA_RV { get; set; }

        [XmlElement(Order = 5)]
        public string NGAY_VAO { get; set; }

        [XmlElement(Order = 6)]
        public string NGAY_RA { get; set; }

        [XmlElement(Order = 7)]
        public string MA_DINH_CHI_THAI { get; set; }

        [XmlElement(Order = 8)]
        public string NGUYENNHAN_DINHCHI { get; set; }

        [XmlElement(Order = 9)]
        public string THOIGIAN_DINHCHI { get; set; }

        [XmlElement(Order = 10)]
        public string TUOI_THAI { get; set; }

        [XmlElement(Order = 11)]
        public string CHAN_DOAN_RV { get; set; }

        [XmlElement(Order = 12)]
        public string PP_DIEUTRI { get; set; }

        [XmlElement(Order = 13)]
        public string GHI_CHU { get; set; }

        [XmlElement(Order = 14)]
        public string MA_TTDV { get; set; }

        [XmlElement(Order = 15)]
        public string MA_BS { get; set; }

        [XmlElement(Order = 16)]
        public string TEN_BS { get; set; }

        [XmlElement(Order = 17)]
        public string NGAY_CT { get; set; }

        [XmlElement(Order = 18)]
        public string MA_CHA { get; set; }

        [XmlElement(Order = 19)]
        public string MA_ME { get; set; }

        [XmlElement(Order = 20)]
        public string MA_THE_TAM { get; set; }

        [XmlElement(Order = 21)]
        public string HO_TEN_CHA { get; set; }

        [XmlElement(Order = 22)]
        public string HO_TEN_ME { get; set; }

        [XmlElement(Order = 23)]
        public int SO_NGAY_NGHI { get; set; }

        [XmlElement(Order = 24)]
        public string NGOAITRU_TUNGAY { get; set; }

        [XmlElement(Order = 25)]
        public string NGOAITRU_DENNGAY { get; set; }

        [XmlElement(Order = 26)]
        public string DU_PHONG { get; set; }

        public string ToXML()
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stringwriter, this);
                return stringwriter.ToString();
            }
        }

        public static XML7Data LoadFromXMLString(string xmlText)
        {
            using (var stringReader = new System.IO.StringReader(RemoveByteOrderMark(xmlText)))
            {
                var serializer = new XmlSerializer(typeof(XML7Data));
                return serializer.Deserialize(stringReader) as XML7Data;
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
}
