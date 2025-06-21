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
    class EmrConfig : BussinessBase
    {
        internal EmrConfig() : base() { }
        internal EmrConfig(CommonParam param) : base(param) { }

        internal List<EMR_CONFIG> Get()
        {
            return Get(new EmrConfigFilter());
        }

        internal List<EMR_CONFIG> Get(EmrConfigFilter filter)
        {
            List<EMR_CONFIG> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_CONFIG>>(EMR.URI.EmrConfig.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<EMR_CONFIG> GetByKey(string key)
        {
            List<EMR_CONFIG> data = null;
            try
            {
                EmrConfigFilter filter = new EmrConfigFilter() { KEY__EXACT = key };
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_CONFIG>>(EMR.URI.EmrConfig.GET, paramCommon, filter);
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
