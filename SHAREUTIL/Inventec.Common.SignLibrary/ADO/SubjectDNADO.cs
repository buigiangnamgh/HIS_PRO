using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary
{
    //SubjectDN = {C=VN,O=Ban Cơ yếu Chính phủ,OU=Cục Chứng thực số và Bảo mật Thông tin,L=Hà Nội,CN=Phạm Công Thảo}
    //SubjectDN = {C=US,ST=CA,L=San Jose,O=Adobe Systems Incorporated,CN=John B Harris,E=jbharris@adobe.com}
    internal class SubjectDNADO
    {
        public SubjectDNADO() { }
        public SubjectDNADO(string inputSubjectDN)
        {
            try
            {
                if (!String.IsNullOrEmpty(inputSubjectDN))
                {
                    var spTokens = inputSubjectDN.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (spTokens != null && spTokens.Length > 0)
                    {
                        foreach (var otk in spTokens)
                        {
                            if (!String.IsNullOrEmpty(otk))
                            {
                                var otkTokens = otk.Split(new string[] { "=" }, StringSplitOptions.None);
                                if (otkTokens != null && otkTokens.Length > 1)
                                {
                                    if (otkTokens[0] == "C")
                                    {
                                        this.C = otkTokens[1];
                                    }
                                    else if (otkTokens[0] == "ST")
                                    {
                                        this.ST = otkTokens[1];
                                    }
                                    else if (otkTokens[0] == "L")
                                    {
                                        this.L = otkTokens[1];
                                    }
                                    else if (otkTokens[0] == "O")
                                    {
                                        this.O = otkTokens[1];
                                    }
                                    else if (otkTokens[0] == "CN")
                                    {
                                        this.CN = otkTokens[1];
                                    }
                                    else if (otkTokens[0] == "E")
                                    {
                                        this.E = otkTokens[1];
                                    }
                                    else if (otkTokens[0] == "OU")
                                    {
                                        this.OU = otkTokens[1];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string ST { get; set; }
        public string E { get; set; }
        public string C { get; set; }
        public string O { get; set; }
        public string OU { get; set; }
        public string L { get; set; }
        public string CN { get; set; }


    }
}
