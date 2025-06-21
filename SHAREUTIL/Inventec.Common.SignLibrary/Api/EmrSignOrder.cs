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
    class EmrSignOrder
    {
        internal EmrSignOrder() { }

        internal List<EMR_SIGN_ORDER> Get(EmrSignOrderFilter filter)
        {
            List<EMR_SIGN_ORDER> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_SIGN_ORDER>>(EMR.URI.EmrSignOrder.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<EMR_SIGN_ORDER> GetByTemp(long signTemId)
        {
            List<EMR_SIGN_ORDER> data = null;
            try
            {
                EmrSignOrderFilter filter = new EmrSignOrderFilter();
                filter.IS_ACTIVE = 1;
                filter.SIGN_TEMP_ID = signTemId;
                filter.ORDER_DIRECTION = "ASC";
                filter.ORDER_FIELD = "NUM_ORDER";
                data = Get(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        //internal EMR_SIGN_ORDER GetByLoginName(string loginName)
        //{
        //    EMR_SIGN_ORDER data = null;
        //    try
        //    {
        //        if (!String.IsNullOrWhiteSpace(loginName))
        //        {
        //            EmrSignOrderFilter filter = new EmrSignOrderFilter();
        //            filter.LOGINNAME__EXACT = loginName;
        //            List<EMR_SIGN_ORDER> datas = Get(filter);
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
