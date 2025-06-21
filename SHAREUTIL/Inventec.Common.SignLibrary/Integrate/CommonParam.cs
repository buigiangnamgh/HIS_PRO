using System;
using System.Collections.Generic;
using System.Text;

namespace Inventec.Common.Integrate
{
    public class CommonParam
    {
        public int? Start { get; set; }
        public int? Limit { get; set; }
        public int? Count { get; set; }
        public string ModuleCode { get; set; }
        public string LanguageCode { get; set; }
        public List<string> Messages = new List<string>();
        public List<string> BugCodes = new List<string>();

        private bool hasException;
        public bool HasException
        {
            get
            {
                return hasException;
            }
            set
            {
                if (value) hasException = value; //Chi cho phep set true, ko cho phep set false
            }
        }


        public CommonParam()
        {
            Messages = new List<string>();
            BugCodes = new List<string>();
        }

        public CommonParam(int? start, int? limit, int? count)
        {
            this.Start = start;
            this.Limit = limit;
            this.Count = count;
            Messages = new List<string>();
            BugCodes = new List<string>();
        }

        public CommonParam(int? start, int? limit)
        {
            this.Start = start;
            this.Limit = limit;
        }

        public string GetMessage()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (Messages != null && Messages.Count > 0)
                {
                    foreach (string item in Messages)
                    {
                        if (!String.IsNullOrWhiteSpace(item))
                        {
                            if (item.Trim().EndsWith("."))
                            {
                                sb.Append(item.Trim()).Append(" ");
                            }
                            else
                            {
                                sb.Append(item.Trim()).Append(". ");
                            }
                        }
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                //Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        /// <summary>
        /// Phuc vu luu tru cac bug (su co) cua he thong: frontend, backend, database...
        /// Khong luu cac loi thao tac, nghiep vu cua nguoi su dung vao danh sach nay. Doi voi cac loi do nguoi su dung thi phai hien thi thong bao ro rang day du bang listMessage.
        /// </summary>

        /// <summary>
        /// Ket qua se co dang sau: [01291,09129,00292,01293,01929]
        /// </summary>
        /// <returns></returns>
        public string GetBugCode()
        {
            try
            {
                if (BugCodes != null && BugCodes.Count > 0)
                {
                    return string.Format("[{0}]", String.Join(",", BugCodes));
                }
            }
            catch (Exception ex)
            {
                //Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return "";
        }
    }
}
