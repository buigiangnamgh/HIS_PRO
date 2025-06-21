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
    class EmrRelation : BussinessBase
    {
        internal EmrRelation() : base() { }
        internal EmrRelation(CommonParam param) : base(param) { }

        internal List<EMR_RELATION> Get()
        {
            return Get(new EmrRelationFilter());
        }

        internal List<EMR_RELATION> Get(EmrRelationFilter filter)
        {
            List<EMR_RELATION> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_RELATION>>(EMR.URI.EmrRelation.GET, paramCommon, filter);
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
