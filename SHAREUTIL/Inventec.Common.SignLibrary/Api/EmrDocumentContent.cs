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
    class EmrDocumentContent : BussinessBase
    {
        internal EmrDocumentContent() : base() { }
        internal EmrDocumentContent(CommonParam param) : base(param) { }

        internal List<EMR_DOCUMENT_CONTENT> Get(EmrDocumentContentFilter filter)
        {
            List<EMR_DOCUMENT_CONTENT> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_DOCUMENT_CONTENT>>("/api/EmrDocumentContent/Get", paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<EMR_DOCUMENT_CONTENT> Get()
        {
            List<EMR_DOCUMENT_CONTENT> data = null;
            try
            {
                EmrDocumentContentFilter filter = new EmrDocumentContentFilter();
                data = Get(filter);
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
