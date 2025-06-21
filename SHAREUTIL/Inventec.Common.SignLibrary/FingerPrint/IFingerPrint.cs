using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.FingerPrint
{
    interface IFingerPrint
    {
        byte[] Run();
    }
}
