using HIS.UC.ServiceRoom.SetPatientClassifyId;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.SetPatientClassifyId
{
    class SetPatientClassifyIdBehavior : ISetPatientClassifyId
    {
        UCRoomExamService entity;
        long? patientClassifyId;

        internal SetPatientClassifyIdBehavior(UCRoomExamService data, long? patientClassifyId)
        {
            this.entity = data;
            this.patientClassifyId = patientClassifyId;
        }

        void ISetPatientClassifyId.Run()
        {
            this.entity.SetPatientClassifyId(patientClassifyId);
        }
    }
}
