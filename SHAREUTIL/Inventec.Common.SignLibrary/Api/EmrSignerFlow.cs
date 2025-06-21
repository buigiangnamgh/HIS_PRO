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
    class EmrSignerFlow
    {
        internal EmrSignerFlow() { }

        internal List<EMR_SIGNER_FLOW> Get(EmrSignerFlowFilter filter)
        {
            List<EMR_SIGNER_FLOW> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_SIGNER_FLOW>>(EMR.URI.EmrSignerFlow.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<V_EMR_SIGNER_FLOW> GetView(EmrSignerFlowViewFilter filter)
        {
            List<V_EMR_SIGNER_FLOW> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<V_EMR_SIGNER_FLOW>>(EMR.URI.EmrSignerFlow.GET_VIEW, paramCommon, filter);
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
