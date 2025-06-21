using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Inventec.Common.Integrate
{
    class JsonConvertUtil
    {
        internal static string SerializeObjectJS(object data)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(data);
        }

        internal static T DeserializeObjectJS<T>(string data)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(data);
        }

        //internal static string SerializeObjectSS(object data)
        //{
        //    return ServiceStack.Text.JsonSerializer.SerializeToString(data);
        //}

        //internal static T DeserializeObjectSS<T>(string data)
        //{
        //    return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(data);
        //}

        internal static string SerializeObject(object data)
        {

            string jsonIgnoreNullValues = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
            });
            //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => jsonIgnoreNullValues), jsonIgnoreNullValues));
            return jsonIgnoreNullValues;
        }

        internal static T DeserializeObject<T>(string data)
        {
            //return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(data);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
        }
    }
}
