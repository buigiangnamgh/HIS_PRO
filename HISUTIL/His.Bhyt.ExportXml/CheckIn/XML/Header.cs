using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CheckIn.XML
{
    [XmlRoot("HEADER")]
    public class Header
    {
        [XmlElement("MESSAGE_VERSION")]
        public string MESSAGE_VERSION { get; set; }

        [XmlElement("SENDER_CODE")]
        public string SENDER_CODE { get; set; }

        [XmlElement("SENDER_NAME")]
        public string SENDER_NAME { get; set; }

        [XmlElement("TRANSACTION_TYPE")]
        public string TRANSACTION_TYPE { get; set; }

        [XmlElement("TRANSACTION_NAME")]
        public string TRANSACTION_NAME { get; set; }

        [XmlElement("TRANSACTION_DATE")]
        public string TRANSACTION_DATE { get; set; }

        [XmlElement("TRANSACTION_ID")]
        public string TRANSACTION_ID { get; set; }

        [XmlElement("REQUEST_ID")]
        public string REQUEST_ID { get; set; }

        [XmlElement("ACTION_TYPE")]
        public int ACTION_TYPE { get; set; }
    }
}
