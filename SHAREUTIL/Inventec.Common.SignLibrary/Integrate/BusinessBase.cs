using Inventec.Common.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.Integrate
{
    internal abstract class BusinessBase : EntityBaseAdapter
    {
        internal BusinessBase()
            : base()
        {
            param = new CommonParam();
            try
            {
                //UserName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();
            }
            catch (Exception)
            {
            }
        }

        internal BusinessBase(CommonParam paramBusiness)
            : base()
        {
            param = (paramBusiness != null ? paramBusiness : new CommonParam());
            try
            {
                //UserName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();
            }
            catch (Exception)
            {
            }
        }

        protected CommonParam param { get; set; }


    }
}
