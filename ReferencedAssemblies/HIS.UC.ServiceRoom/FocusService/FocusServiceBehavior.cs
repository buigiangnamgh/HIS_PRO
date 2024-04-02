using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.FocusService
{
    class FocusServiceBehavior : IFocusService
    {
        UCRoomExamService entity;

        internal FocusServiceBehavior(UCRoomExamService data)
        {
            this.entity = data;
        }

        void IFocusService.Run()
        {
            this.entity.FocusTotxtExamServiceCode();
        }
    }
}
