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
    class EmrTreatment : BussinessBase
    {
        internal EmrTreatment() : base() { }
        internal EmrTreatment(CommonParam param) : base(param) { }

        internal List<EMR_TREATMENT> Get(EmrTreatmentFilter filter)
        {
            List<EMR_TREATMENT> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_TREATMENT>>(EMR.URI.EmrTreatment.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                string baseUri = GlobalStore.EmrConsumer.GetBaseUri();
                Inventec.Common.Logging.LogSystem.Warn("EmrTreatment.Get:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => GlobalStore.EMR_BASE_URI), GlobalStore.EMR_BASE_URI) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => baseUri), baseUri) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => EMR.URI.EmrTreatment.GET), EMR.URI.EmrTreatment.GET));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return data;
        }

        internal EMR_TREATMENT GetByCode(string code)
        {
            EMR_TREATMENT data = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(code))
                {
                    CommonParam paramCommon = new CommonParam();
                    EmrTreatmentFilter filter = new EmrTreatmentFilter();
                    filter.TREATMENT_CODE__EXACT = code;
                    var rs = GlobalStore.EmrConsumer.Get<List<EMR_TREATMENT>>(EMR.URI.EmrTreatment.GET, paramCommon, filter);
                    data = rs != null && rs.Count > 0 ? rs.FirstOrDefault() : null;
                    if (data == null)
                    {
                        Inventec.Common.Logging.LogSystem.Warn("Tìm hồ sơ điều trị theo mã kết quả không tìm thấy hồ sơ nào____TreatmentCode:" + code + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rs), rs));
                    }
                }
            }
            catch (Exception ex)
            {
                string baseUri = GlobalStore.EmrConsumer.GetBaseUri();
                Inventec.Common.Logging.LogSystem.Warn("EMR_TREATMENT GetByCode:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => GlobalStore.EMR_BASE_URI), GlobalStore.EMR_BASE_URI) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => baseUri), baseUri) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => EMR.URI.EmrTreatment.GET), EMR.URI.EmrTreatment.GET));
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return data;
        }
    }
}
