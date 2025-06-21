using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.FingerPrint
{
    class FingerPrintNoUseBehavior : BusinessBase, IFingerPrint
    {
        InputADO entity;
        internal FingerPrintNoUseBehavior(CommonParam param, InputADO inputADOWorking)
            : base()
        {
            this.entity = inputADOWorking;
        }

        byte[] IFingerPrint.Run()
        {
            try
            {
                //Inventec.Desktop.Common.Modules.Module module = null;
                //RefeshReference refeshReference = null;
                //foreach (var item in entity)
                //{
                //    if (item is Inventec.Desktop.Common.Modules.Module)
                //    {
                //        module = (Inventec.Desktop.Common.Modules.Module)item;
                //    }
                //    if (item is RefeshReference)
                //    {
                //        refeshReference = (RefeshReference)item;
                //    }
                //}
                //return new frmSignBoard(module, refeshReference);

                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
