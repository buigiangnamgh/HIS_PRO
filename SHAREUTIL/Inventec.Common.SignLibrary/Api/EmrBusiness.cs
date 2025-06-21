using EMR.EFMODEL.DataModels;
using EMR.Filter;
using EMR.SDO;
using EMR.TDO;
using Inventec.Common.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.Api
{
    class EmrBusiness : BussinessBase
    {
        internal EmrBusiness() : base() { }
        internal EmrBusiness(CommonParam param) : base(param) { }

        internal List<EMR_BUSINESS> Get()
        {
            return Get(new EmrBusinessFilter());
        }

        internal List<EMR_BUSINESS> Get(EmrBusinessFilter filter)
        {
            List<EMR_BUSINESS> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_BUSINESS>>(EMR.URI.EmrBusiness.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<EMR_BUSINESS> GetByKey(string key)
        {
            List<EMR_BUSINESS> data = null;
            try
            {
                EmrBusinessFilter filter = new EmrBusinessFilter() { BUSINESS_CODE__EXACT = key };
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_BUSINESS>>(EMR.URI.EmrBusiness.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }
    }
}
