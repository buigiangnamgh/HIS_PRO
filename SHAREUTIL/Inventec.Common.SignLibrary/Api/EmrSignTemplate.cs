using EMR.EFMODEL.DataModels;
using EMR.Filter;
using Inventec.Common.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.Api
{
    class EmrSignTemplate
    {
        internal EmrSignTemplate() { }

        internal List<EMR_SIGN_TEMP> Get(EmrSignTempFilter filter)
        {
            List<EMR_SIGN_TEMP> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_SIGN_TEMP>>(EMR.URI.EmrSignTemp.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<EMR_SIGN_TEMP> Get()
        {
            List<EMR_SIGN_TEMP> data = null;
            try
            {
                EmrSignTempFilter filter = new EmrSignTempFilter();
                filter.IS_ACTIVE = 1;
                filter.ORDER_DIRECTION = "ASC";
                filter.ORDER_FIELD = "SIGN_TEMP_CODE";
                data = Get(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        //internal EMR_SIGN_TEMP GetByLoginName(string loginName)
        //{
        //    EMR_SIGN_TEMP data = null;
        //    try
        //    {
        //        if (!String.IsNullOrWhiteSpace(loginName))
        //        {
        //            EmrSignTempFilter filter = new EmrSignTempFilter();
        //            filter.LOGINNAME__EXACT = loginName;
        //            List<EMR_SIGN_TEMP> datas = Get(filter);
        //            data = datas != null ? datas.Where(o => o.LOGINNAME == loginName).FirstOrDefault() : null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Warn(ex);
        //        data = null;
        //    }

        //    return data;
        //}
    }
}
