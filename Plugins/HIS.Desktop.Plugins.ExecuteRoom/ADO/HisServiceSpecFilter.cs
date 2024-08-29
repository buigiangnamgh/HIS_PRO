using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.ExecuteRoom.ADO
{
    class HisServiceSpecFilter
    {

        public long? SERVICE_ID { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public long? SPECIALITY_ID { get; set; }
        public List<long> SPECIALITY_IDs { get; set; }
    }
}
