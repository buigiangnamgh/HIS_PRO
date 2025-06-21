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
    class EmrDocumentGroup : BussinessBase
    {
        internal EmrDocumentGroup() : base() { }
        internal EmrDocumentGroup(CommonParam param) : base(param) { }

        internal List<EMR_DOCUMENT_GROUP> Get(EmrDocumentGroupFilter filter)
        {
            List<EMR_DOCUMENT_GROUP> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_DOCUMENT_GROUP>>(EMR.URI.EmrDocumentGroup.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<EMR_DOCUMENT_GROUP> Get()
        {
            List<EMR_DOCUMENT_GROUP> data = null;
            try
            {
                EmrDocumentGroupFilter filter = new EmrDocumentGroupFilter();
                data = Get(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal EMR_DOCUMENT_GROUP GetByCode(string code)
        {
            EMR_DOCUMENT_GROUP data = null;
            try
            {
                EmrDocumentGroupFilter filter = new EmrDocumentGroupFilter() { DOCUMENT_GROUP_CODE__EXACT = code };
                data = Get(filter).FirstOrDefault();
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
