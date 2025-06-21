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
    class EmrSigner
    {
        internal EmrSigner() { }

        internal List<EMR_SIGNER> Get(EmrSignerFilter filter)
        {
            List<EMR_SIGNER> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_SIGNER>>(EMR.URI.EmrSigner.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }
        internal List<EMR_SIGNER> Get(ref CommonParam paramCommon, EmrSignerFilter filter)
        {
            List<EMR_SIGNER> data = null;
            try
            {
                data = GlobalStore.EmrConsumer.Get<List<EMR_SIGNER>>(EMR.URI.EmrSigner.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }
        internal List<EMR_SIGNER> Get()
        {
            List<EMR_SIGNER> data = null;
            try
            {
                EmrSignerFilter filter = new EmrSignerFilter();
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
