using EMR.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.ADO
{
    public class AttackADO : EMR_ATTACHMENT
    {
        public string FILE_NAME { get; set; }

        public string Base64Data { get; set; }
        public long DocumentId { get; set; }
        public string Extension { get; set; }
        public string FullName { get; set; }

        public AttackADO(){}
    }
}
