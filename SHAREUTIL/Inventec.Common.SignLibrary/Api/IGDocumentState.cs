using EMR.EFMODEL.DataModels;
using EMR.Filter;
using EMR.SDO;
using EMR.TDO;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.Api
{
    class IGDocumentState : BussinessBase
    {
        internal IGDocumentState() : base() { }
        internal IGDocumentState(CommonParam param) : base(param) { }

        internal async Task<bool> SendSignedInfoToIGSys(DocumentSignedUpdateIGSysResultDTO dataSigned)//TODO change return type
        {
            CommonParam paramCommon = new CommonParam();
            bool success = false;
            try
            {
                var rs = await GlobalStore.IntegrateConsumer.PostWithouApiParamAsync<EmrSignResultSDO>(GlobalStore.INTERGRATE_SYS_API, dataSigned, 0);
                if (rs != null)
                {
                    success = true;
                }
                Inventec.Common.Logging.LogSystem.Debug("SendSignedInfoToIGSys:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => dataSigned), dataSigned)
                    + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => GlobalStore.INTERGRATE_SYS_BASE_URI), GlobalStore.INTERGRATE_SYS_BASE_URI)
                    + "____" + Inventec.Common.Logging.LogUtil.TraceData("GetBaseUri()", GlobalStore.IntegrateConsumer.GetBaseUri())
                    + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => GlobalStore.INTERGRATE_SYS_API), GlobalStore.INTERGRATE_SYS_API) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rs), rs)
                    + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                success = false;
            }

            return success;
        }

    }
}
