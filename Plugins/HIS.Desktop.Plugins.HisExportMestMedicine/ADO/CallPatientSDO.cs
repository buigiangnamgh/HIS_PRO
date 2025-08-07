using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.HisExportMestMedicine.ADO
{
    public class CallPatientSDO
    {
        public long? NUM_ORDER { get; set; }

        public decimal? EXECUTE_ROOM_ID { get; set; }

        public string TDL_PATIENT_NAME { get; set; }
    }
}
