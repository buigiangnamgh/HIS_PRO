using Inventec.Common.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.ADO
{
    internal class SignToken
    {
        internal SignToken() { }

        public string TokenCode { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public EMR.EFMODEL.DataModels.EMR_SIGNER Singer { get; set; }
        public EMR.EFMODEL.DataModels.EMR_TREATMENT Treatment { get; set; }
        public bool IsUseTimespan { get; set; }
        public string Password { get; set; }
        public TokenData TokenData { get; set; }
        public ApiConsumer EmrConsumer { get; set; }
        public string PIN { get; set; }
    }
}
