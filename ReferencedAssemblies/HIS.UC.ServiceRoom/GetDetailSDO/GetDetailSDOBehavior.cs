using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.GetDetailSDO
{
    class GetDetailSDOBehavior : IGetDetailSDO
    {
        UCRoomExamService entity;

        internal GetDetailSDOBehavior(UCRoomExamService data)
        {
            this.entity = data;
        }

        object IGetDetailSDO.Run()
        {
            return entity.GetDetailSDO();
        }
    }
}
