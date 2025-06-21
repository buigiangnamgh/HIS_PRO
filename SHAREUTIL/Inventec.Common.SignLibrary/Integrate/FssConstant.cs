using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignToolViewer.Integrate
{
    internal class FssConstant
    {
        internal static string BASE_URI = "";//ConfigurationManager.AppSettings["fss.uri.base"];
        internal static string UPLOAD_URI = "api/File/Upload";
        internal static int TIME_OUT = 300;
        internal const string HEADER_CLIENT_CODE = "fss-client-code";
        internal const string HEADER_FILE_STORE_LOCATION = "fss-file-store-location";
    }
}
