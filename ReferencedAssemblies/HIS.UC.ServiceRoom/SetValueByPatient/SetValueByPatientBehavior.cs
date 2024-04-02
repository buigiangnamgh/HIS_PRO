using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ServiceRoom.SetValueByPatient
{
    class SetValueByPatientBehavior : ISetValueByPatient
    {
        UCRoomExamService entity;
        V_HIS_PATIENT patient;

        internal SetValueByPatientBehavior(UCRoomExamService data, V_HIS_PATIENT value)
        {
            this.entity = data;
            this.patient = value;
        }

        void ISetValueByPatient.Run()
        {
            this.entity.SetValueByPatient(patient);
        }
    }
}
