using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary
{
    public class SignStateCode
    {
        /// <summary>
        /// Ký thành công
        /// </summary>
        public const string SUCCESS = "00";
        /// <summary>
        /// Ký thất bại
        /// </summary>
        public const string FAIL = "01";
        /// <summary>
        /// Từ chối ký
        /// </summary>
        public const string REJECT = "02";
        /// <summary>
        /// Văn bản đã thiết lập ký
        /// </summary>
        public const string DOCUMENT_HAS_SIGN_CONFIG = "04";
        /// <summary>
        /// Văn bản chưa thiết lập ký
        /// </summary>
        public const string DOCUMENT_CREATE_NEW = "03";
        /// <summary>
        /// Văn bản đã thiết lập ký, nhưng chưa hoàn thành.
        /// </summary>
        public const string DOCUMENT_HAS_SIGN_CONFIG_UN_FINAL = "05";
        /// <summary>
        /// Văn bản hủy
        /// </summary>
        public const string DOCUMENT_DELETE = "06";
    }
}
