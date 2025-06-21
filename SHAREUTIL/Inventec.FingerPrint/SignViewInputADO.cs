using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.FingerPrint
{
    public class SignViewInputADO
    {
        public string DriverName { get; set; }
        
        public Action<Bitmap> ActGetSignImageFile { get; set; }

        public Action<String> ActSelectDevice { get; set; }
    }
}
