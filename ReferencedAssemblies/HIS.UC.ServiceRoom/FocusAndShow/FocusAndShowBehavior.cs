using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.FocusAndShow
{
    class FocusAndShowBehavior : IFocusAndShow
    {
        UCRoomExamService entity;

        internal FocusAndShowBehavior(UCRoomExamService data)
        {
            this.entity = data;
        }

        void IFocusAndShow.Run()
        {
            this.entity.FocusUserControl();
        }
    }
}
