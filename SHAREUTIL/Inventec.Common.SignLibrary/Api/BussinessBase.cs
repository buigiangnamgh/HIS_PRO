using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.Api
{
    internal abstract class BussinessBase
    {
        protected CommonParam param { get; set; }
        protected InputADO inputADOWorking { get; set; }
        protected bool IsSignParanel { get; set; }
        protected string Src { get; set; }
        protected EMR.EFMODEL.DataModels.EMR_TREATMENT Treatment { get; set; }
        protected EMR.EFMODEL.DataModels.EMR_SIGNER Signer { get; set; }
        protected byte[] SignPadImageData { get; set; }
        protected string deviceSignPadName = "";
        protected bool IsUsingSignPad { get; set; }
        protected string TokenCode { get; set; }
        protected FileType FileType { get; set; }
        protected bool IsUsingSignPadBefore { get; set; }
        internal BussinessBase()
            : base()
        {
            param = new CommonParam();
        }

        internal BussinessBase(CommonParam paramBusiness)
            : base()
        {
            param = (paramBusiness != null ? paramBusiness : new CommonParam());
        }
        internal BussinessBase(bool isSignParanel)
            : base()
        {
            IsSignParanel = isSignParanel;
        }
        internal BussinessBase(CommonParam paramBusiness, InputADO inputADO)
            : base()
        {
            param = (paramBusiness != null ? paramBusiness : new CommonParam());
            inputADOWorking = inputADO;
        }

        internal BussinessBase(CommonParam paramBusiness, InputADO inputADO, string src)
            : base()
        {
            param = (paramBusiness != null ? paramBusiness : new CommonParam());
            inputADOWorking = inputADO;
            Src = src;
        }

        internal BussinessBase(CommonParam paramBusiness, InputADO inputADO, string src, bool isSignParanel)
            : base()
        {
            param = (paramBusiness != null ? paramBusiness : new CommonParam());
            inputADOWorking = inputADO;
            Src = src;
            IsSignParanel = isSignParanel;
        }

        internal BussinessBase(CommonParam paramBusiness, InputADO inputADO, string src, bool isSignParanel, EMR.EFMODEL.DataModels.EMR_TREATMENT treatment, EMR.EFMODEL.DataModels.EMR_SIGNER singer, string tokenCode)
            : base()
        {
            param = (paramBusiness != null ? paramBusiness : new CommonParam());
            inputADOWorking = inputADO;
            Src = src;
            IsSignParanel = isSignParanel;
            Treatment = treatment;
            Signer = singer;
            TokenCode = tokenCode;
        }
    }
}
