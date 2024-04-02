
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.UC.ServiceRoom.Dispose
{
    public sealed class DisposeBehavior : IDispose
    {
        UserControl control;
        public DisposeBehavior()
            : base()
        {
        }

        public DisposeBehavior(UserControl uc)
            : base()
        {
            this.control = uc;
        }

        void IDispose.Run()
        {
            try
            {

                ((UCRoomExamService)this.control).DisposeControl();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
