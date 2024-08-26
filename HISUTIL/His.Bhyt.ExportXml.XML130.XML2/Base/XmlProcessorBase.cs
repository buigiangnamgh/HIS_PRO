using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.XML130.XML2.Base
{
    public class XmlProcessorBase
    {
        internal XmlCDataSection ConvertStringToXmlDocument(string data)
        {
            XmlCDataSection result;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<book genre='novel' ISBN='1-861001-57-5'>" + "<title>Pride And Prejudice</title>" + "</book>");
            result = doc.CreateCDataSection(RemoveXmlCharError(data));
            return result;
        }

        public string CreatedXmlString<T>(T input)
        {
            string rs = null;
            try
            {
                var enc = Encoding.UTF8;
                using (var ms = new MemoryStream())
                {
                    var xmlNamespaces = new XmlSerializerNamespaces();
                    xmlNamespaces.Add("", "");

                    var xmlWriterSettings = new XmlWriterSettings
                    {
                        CloseOutput = false,
                        Encoding = enc,
                        OmitXmlDeclaration = false,
                        Indent = true
                    };
                    using (var xw = XmlWriter.Create(ms, xmlWriterSettings))
                    {
                        var s = new XmlSerializer(typeof(T));
                        s.Serialize(xw, input, xmlNamespaces);
                    }
                    rs = enc.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = null;
            }
            return rs;
        }

        public ResultADO CreatedXmlFile<T>(T input, bool encode, bool displayNamspacess, bool saveFile, string path)
        {
            ResultADO rs = null;
            string xmlFile = null;
            try
            {
                var enc = Encoding.UTF8;
                using (var ms = new MemoryStream())
                {
                    var xmlNamespaces = new XmlSerializerNamespaces();
                    if (displayNamspacess)
                    {
                        xmlNamespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
                        xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    }
                    else
                        xmlNamespaces.Add("", "");

                    var xmlWriterSettings = new XmlWriterSettings
                    {
                        CloseOutput = false,
                        Encoding = enc,
                        OmitXmlDeclaration = false,
                        Indent = true
                    };
                    using (var xw = XmlWriter.Create(ms, xmlWriterSettings))
                    {
                        var s = new XmlSerializer(typeof(T));
                        s.Serialize(xw, input, xmlNamespaces);
                    }
                    xmlFile = enc.GetString(ms.ToArray());
                }

                if (saveFile)//kiểm tra lưu file không
                {
                    using (var file = new StreamWriter(path))
                    {
                        file.Write(xmlFile);
                    }
                    rs = new ResultADO(true, "Luu Thanh Cong", new object[] { xmlFile });
                }

                if (encode)//kiểm tra nếu cần mã hóa file thì mã hóa sau đó trả lại cho ng dùng
                {
                    var encodeXml = EncodeBase64(Encoding.UTF8, xmlFile);
                    if (!string.IsNullOrEmpty(encodeXml))
                        rs = new ResultADO(true, "", new object[] { encodeXml });
                    else
                        rs = new ResultADO(false, "Ma hoa bang Base 64 that bai", null);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        public ResultADO CreatedXmlFileEncoding<T>(T input, bool encode, bool displayNamspacess, bool saveFile, string path)
        {
            ResultADO rs = null;
            string xmlFile = null;
            try
            {
                var enc = new UTF8Encoding(false);

                using (var ms = new MemoryStream())
                {
                    var xmlNamespaces = new XmlSerializerNamespaces();
                    if (displayNamspacess)
                    {
                        xmlNamespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
                        xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    }
                    else
                        xmlNamespaces.Add("", "");

                    var xmlWriterSettings = new XmlWriterSettings
                    {
                        CloseOutput = false,
                        Encoding = enc,
                        OmitXmlDeclaration = false,
                        Indent = true
                    };
                    using (var xw = XmlWriter.Create(ms, xmlWriterSettings))
                    {
                        var s = new XmlSerializer(typeof(T));
                        s.Serialize(xw, input, xmlNamespaces);
                    }
                    xmlFile = enc.GetString(ms.ToArray());
                }

                if (saveFile)//kiểm tra lưu file không
                {
                    using (var file = new StreamWriter(path))
                    {
                        file.Write(xmlFile);
                    }
                    rs = new ResultADO(true, "Luu Thanh Cong", new object[] { xmlFile });
                }

                if (encode)//kiểm tra nếu cần mã hóa file thì mã hóa sau đó trả lại cho ng dùng
                {
                    var encodeXml = EncodeBase64(Encoding.UTF8, xmlFile);
                    if (!string.IsNullOrEmpty(encodeXml))
                        rs = new ResultADO(true, "", new object[] { encodeXml });
                    else
                        rs = new ResultADO(false, "Ma hoa bang Base 64 that bai", null);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = new ResultADO(false, ex.Message, null);
            }
            return rs;
        }

        public MemoryStream CreatedXmlFilePlus<T>(T input, bool encode, bool displayNamspacess, bool saveFile, string path)
        {
            MemoryStream stream = null;
            string xmlFile = null;
            try
            {
                var enc = Encoding.UTF8;
                stream = new MemoryStream();
                var xmlNamespaces = new XmlSerializerNamespaces();
                if (displayNamspacess)
                {
                    xmlNamespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
                    xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                }
                else
                    xmlNamespaces.Add("", "");

                var xmlWriterSettings = new XmlWriterSettings
                {
                    CloseOutput = false,
                    Encoding = enc,
                    OmitXmlDeclaration = false,
                    Indent = true
                };
                using (var xw = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    var s = new XmlSerializer(typeof(T));
                    s.Serialize(xw, input, xmlNamespaces);
                }
                xmlFile = enc.GetString(stream.ToArray());

                //if (encode)//kiểm tra nếu cần mã hóa file thì mã hóa sau đó trả lại cho ng dùng
                //{
                //    var encodeXml = EncodeBase64(Encoding.UTF8, xmlFile);
                //    if (!string.IsNullOrEmpty(encodeXml))
                //        rs = new ResultADO(true, "", new object[] { encodeXml });
                //    else
                //        rs = new ResultADO(false, "Ma hoa bang Base 64 that bai", null);
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                stream = null;
            }
            return stream;
        }

        public MemoryStream CreatedXmlFileEncodingPlus<T>(T input, bool encode, bool displayNamspacess, bool saveFile, string path)
        {
            MemoryStream stream = null;
            string xmlFile = null;
            try
            {
                var enc = new UTF8Encoding(false);

                stream = new MemoryStream();
                var xmlNamespaces = new XmlSerializerNamespaces();
                if (displayNamspacess)
                {
                    xmlNamespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
                    xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                }
                else
                    xmlNamespaces.Add("", "");

                var xmlWriterSettings = new XmlWriterSettings
                {
                    CloseOutput = false,
                    Encoding = enc,
                    OmitXmlDeclaration = false,
                    Indent = true
                };
                using (var xw = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    var s = new XmlSerializer(typeof(T));
                    s.Serialize(xw, input, xmlNamespaces);
                }
                xmlFile = enc.GetString(stream.ToArray());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                stream = null;
            }
            return stream;
        }

        internal string EncodeBase64(Encoding encoding, string text)//Mã hóa file XML sang Base64
        {
            if (text == null)
                return null;
            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        internal bool CheckBhytNsd(List<string> listIcdCode, List<string> listIcdCodeTe, string icdCode, V_HIS_HEIN_APPROVAL hisHeinApprovalBhyt, long serviceId, List<V_HIS_SERVICE> totalSericeData, List<HIS_ICD> totalIcdData)
        {
            var result = false;
            try
            {
                if (hisHeinApprovalBhyt != null && !String.IsNullOrEmpty(hisHeinApprovalBhyt.HEIN_CARD_NUMBER) &&
                    (hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CA") ||
                    hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CY") ||
                    hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("QN")))
                {
                    return true;
                }

                List<HIS_ICD> icdNds = null;
                if (totalIcdData != null && totalIcdData.Count > 0)
                {
                    icdNds = totalIcdData.Where(o => o.IS_HEIN_NDS == 1).ToList();
                }

                V_HIS_SERVICE service = new V_HIS_SERVICE();
                if (totalSericeData != null && totalSericeData.Count > 0)
                {
                    service = totalSericeData.FirstOrDefault(o => o.ID == serviceId);
                }

                if ((listIcdCode == null || listIcdCode.Count == 0) && (listIcdCodeTe == null || listIcdCodeTe.Count == 0) && (icdNds == null || icdNds.Count == 0))
                {
                    return result;
                }

                if (service != null && service.IS_OUT_OF_DRG == 1 && !string.IsNullOrEmpty(icdCode))
                {
                    if (listIcdCode == null || listIcdCode.Count == 0)
                    {
                        listIcdCode = new List<string>();
                    }

                    if (listIcdCodeTe == null || listIcdCodeTe.Count == 0)
                    {
                        listIcdCodeTe = new List<string>();
                    }

                    if (icdNds == null || icdNds.Count == 0)
                    {
                        icdNds = new List<HIS_ICD>();
                    }

                    if ((listIcdCode.Contains(icdCode) || icdNds.Exists(o => o.ICD_CODE == icdCode)))
                        result = true;
                    else if (hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && (listIcdCodeTe.Contains(icdCode.Substring(0, 3)) || icdNds.Exists(o => o.ICD_CODE == icdCode)))
                        result = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal bool CheckBhytNsd(List<string> listIcdCode, List<string> listIcdCodeTe, V_HIS_TREATMENT_3 hisTreatment, V_HIS_HEIN_APPROVAL hisHeinApprovalBhyt)
        {
            var result = false;
            try
            {
                if (hisHeinApprovalBhyt != null && !String.IsNullOrEmpty(hisHeinApprovalBhyt.HEIN_CARD_NUMBER) &&
                    (hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CA") ||
                    hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CY") ||
                    hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("QN")))
                {
                    return true;
                }

                if ((listIcdCode == null || listIcdCode.Count == 0) && (listIcdCodeTe == null || listIcdCodeTe.Count == 0))
                {
                    return result;
                }

                if (listIcdCode == null || listIcdCode.Count == 0)
                {
                    listIcdCode = new List<string>();
                }

                if (listIcdCodeTe == null || listIcdCodeTe.Count == 0)
                {
                    listIcdCodeTe = new List<string>();
                }

                if (listIcdCode.Contains(hisTreatment.ICD_CODE))
                    result = true;
                else if (!string.IsNullOrEmpty(hisTreatment.ICD_CODE))
                    if (hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && listIcdCodeTe.Contains(hisTreatment.ICD_CODE.Substring(0, 3)))
                        result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal string RemoveXmlCharError(string data)
        {
            string result = "";
            try
            {
                StringBuilder s = new StringBuilder();
                if (!String.IsNullOrWhiteSpace(data))
                {
                    foreach (char c in data)
                    {
                        if (!System.Xml.XmlConvert.IsXmlChar(c)) continue;
                        s.Append(c);
                    }
                }

                result = s.ToString();
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        public static T Deserialize<T>(string xml)
        {
            try
            {
                if (string.IsNullOrEmpty(xml))
                {
                    return default(T);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlReaderSettings settings = new XmlReaderSettings();

                using (StringReader textReader = new StringReader(xml))
                {
                    using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                    {
                        return (T)serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return default(T);
        }

        public static string FormatString(string txt) 
        {
            string result = "";
            try
            {
                CultureInfo cultureInfo = new CultureInfo("vi-VN");

                result = ConvertDecimal(txt).ToString("#,###.##", cultureInfo);
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);                
            }
            return result;
        }

        public static decimal ConvertDecimal(string txt)
        {
            decimal rs = 0;
            try
            {
                CultureInfo culture = new CultureInfo("en-US");
                if (txt.Contains(","))
                    culture = new CultureInfo("fr-FR");
                rs = Convert.ToDecimal(txt, culture);
            }
            catch (Exception ex)
            {
                rs = 0;
                Inventec.Common.Logging.LogSystem.Error(txt + "________" + ex);
            }
            return rs;
        }

        public static string TimeNumberToTimeString(string time)
        {
            string result = null;
            try
            {
                if (time != null && time.Length >= 12)
                {
                    result = new StringBuilder().Append(time.Substring(6, 2)).Append("/").Append(time.Substring(4, 2)).Append("/").Append(time.Substring(0, 4)).Append(" ").Append(time.Substring(8, 2)).Append(":").Append(time.Substring(10, 2)).ToString();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        internal string header_xml = "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"";
    }
}
