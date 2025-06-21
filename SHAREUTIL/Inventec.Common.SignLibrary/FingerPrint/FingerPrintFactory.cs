using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.SignLibrary.FingerPrint;

namespace Inventec.Common.SignLibrary.FingerPrint
{
    internal class FingerPrintFactory
    {
        internal static IFingerPrint MakeISignBoard(CommonParam param, InputADO inputADOWorking, SignBoardOption signBoardOption)
        {
            IFingerPrint result = null;
            try
            {
                switch (signBoardOption)
                {
                    case SignBoardOption.NoUse:
                        result = new FingerPrintNoUseBehavior(param, inputADOWorking);
                        break;
                    case SignBoardOption.Use:
                        result = new FingerPrintUseBehavior(param, inputADOWorking);
                        break;
                    default:
                        break;
                }

                if (result == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Factory khong khoi tao duoc doi tuong." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => signBoardOption), signBoardOption), ex);
                result = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
