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
    class EmrVersion : BussinessBase
    {
        internal EmrVersion() : base() { }
        internal EmrVersion(CommonParam param) : base(param) { }

        internal List<EMR_VERSION> Get(EmrVersionFilter filter)
        {
            List<EMR_VERSION> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_VERSION>>(EMR.URI.EmrVersion.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                data = null;
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return data;
        }

        internal EMR_VERSION GetSignedDocumentLast(long documentId)
        {
            EMR_VERSION data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                EmrVersionFilter filter = new EmrVersionFilter();
                filter.DOCUMENT_ID = documentId;
                var rs = GlobalStore.EmrConsumer.Get<List<EMR_VERSION>>(EMR.URI.EmrVersion.GET, paramCommon, filter);
                data = (rs != null && rs.Count > 0) ? rs.OrderByDescending(o => o.ID).FirstOrDefault() : null;
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
