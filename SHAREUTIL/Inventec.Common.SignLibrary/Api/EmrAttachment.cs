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
    class EmrAttachment : BussinessBase
    {
        internal EmrAttachment() : base() { }
        internal EmrAttachment(CommonParam param) : base(param) { }

        internal List<EMR_ATTACHMENT> Get(EmrAttachmentFilter filter)
        {
            List<EMR_ATTACHMENT> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_ATTACHMENT>>(EMR.URI.EmrAttachment.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                data = null;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return data;
        }

    }
}
