using EMR.EFMODEL.DataModels;
using Inventec.Common.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.ADO
{
    class ListSignConfigADO : V_EMR_SIGN
    {
        public int Action { get; set; }
        public string SIGN_TIME_STR { get; set; }
        public string REJECT_TIME_STR { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public bool IsPatient { get; set; }
        public long IdRow { get; set; }

        public object Signer { get; set; }

        public ListSignConfigADO()
        {

        }

        public ListSignConfigADO(V_EMR_SIGN data)
        {
            if (data != null)
            {
                //Inventec.Common.Mapper.DataObjectMapper.Map<ListSignConfigADO>(this, data);

                this.ATTEMPT_NUMBER = data.ATTEMPT_NUMBER;
                this.CARD_CODE = data.CARD_CODE;
                this.CREATE_TIME = data.CREATE_TIME;
                this.CREATOR = data.CREATOR;
                this.DEPARTMENT_CODE = data.DEPARTMENT_CODE;
                this.DEPARTMENT_NAME = data.DEPARTMENT_NAME;
                this.DOCUMENT_ID = data.DOCUMENT_ID;
                this.FIRST_NAME = data.FIRST_NAME;
                this.FLOW_CODE = data.FLOW_CODE;
                this.FLOW_ID = data.FLOW_ID;
                this.FLOW_NAME = data.FLOW_NAME;
                this.GROUP_CODE = data.GROUP_CODE;
                this.ID = data.ID;
                this.IS_ACTIVE = data.IS_ACTIVE;
                this.IS_DELETE = data.IS_DELETE;
                this.IS_SIGNING = data.IS_SIGNING;
                this.LAST_NAME = data.LAST_NAME;
                this.LINK_CODE = data.LINK_CODE;
                this.LOGINNAME = data.LOGINNAME;
                this.MODIFIER = data.MODIFIER;
                this.MODIFY_TIME = data.MODIFY_TIME;
                this.NUM_ORDER = data.NUM_ORDER;
                this.PATIENT_CODE = data.PATIENT_CODE;
                this.PCA_SERIAL = data.PCA_SERIAL;
                this.REJECT_DATE = data.REJECT_DATE;
                this.REJECT_REASON = data.REJECT_REASON;
                this.REJECT_TIME = data.REJECT_TIME;
                this.RELATION_NAME = data.RELATION_NAME;
                this.RELATION_PEOPLE_NAME = data.RELATION_PEOPLE_NAME;
                this.ROOM_CODE = data.ROOM_CODE;
                this.ROOM_NAME = data.ROOM_NAME;
                this.ROOM_TYPE_CODE = data.ROOM_TYPE_CODE;
                this.SERVICE_CODE = data.SERVICE_CODE;
                this.SIGN_DATE = data.SIGN_DATE;
                this.SIGN_STT_ID = data.SIGN_STT_ID;
                this.SIGN_TIME = data.SIGN_TIME;
                this.TITLE = data.TITLE;
                this.USERNAME = data.USERNAME;
                this.VERSION_ID = data.VERSION_ID;
                this.VIR_PATIENT_NAME = data.VIR_PATIENT_NAME;

                this.SIGN_TIME_STR = data.SIGN_TIME.HasValue ? DateTimeConvert.TimeNumberToTimeString(data.SIGN_TIME.Value) : "";
                this.REJECT_TIME_STR = data.REJECT_TIME.HasValue ? DateTimeConvert.TimeNumberToTimeString(data.REJECT_TIME.Value) : "";
                if (this.FLOW_ID.HasValue)
                {
                    this.Signer = this.FLOW_NAME;
                }
                else if (!String.IsNullOrWhiteSpace(this.LOGINNAME))
                {
                    this.Signer = data.LOGINNAME;
                }
                if (!String.IsNullOrWhiteSpace(data.PATIENT_CODE))
                {
                    IsPatient = true;
                    if (!String.IsNullOrWhiteSpace(data.RELATION_NAME))
                    {
                        this.Signer = !String.IsNullOrEmpty(data.CARD_CODE) ? string.Format("{0}({1})", data.RELATION_PEOPLE_NAME, data.CARD_CODE) : string.Format("{0}", data.RELATION_PEOPLE_NAME);
                        this.TITLE = data.RELATION_NAME;
                    }
                    else
                    {
                        this.Signer = data.VIR_PATIENT_NAME;
                    }
                }

            }
        }
    }
}
