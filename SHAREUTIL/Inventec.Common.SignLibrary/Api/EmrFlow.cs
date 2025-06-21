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
    class EmrFlow
    {
        internal EmrFlow() { }

        internal List<EMR_FLOW> Get(EmrFlowFilter filter)
        {
            List<EMR_FLOW> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_FLOW>>(EMR.URI.EmrFlow.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<V_EMR_FLOW> GetView(EmrFlowViewFilter filter)
        {
            List<V_EMR_FLOW> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<V_EMR_FLOW>>(EMR.URI.EmrFlow.GET_VIEW, paramCommon, filter);
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
