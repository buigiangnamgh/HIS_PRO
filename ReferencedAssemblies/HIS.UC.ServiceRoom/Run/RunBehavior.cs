using HIS.UC.ServiceRoom.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.Run
{
    class RunBehavior : IRun
    {
        RoomExamServiceInitADO entity;

        internal RunBehavior(RoomExamServiceInitADO data)
        {
            this.entity = data;
        }

        object IRun.Run()
        {
            object result = null;
            result = new UCRoomExamService(entity);
            return result;
        }
    }
}
