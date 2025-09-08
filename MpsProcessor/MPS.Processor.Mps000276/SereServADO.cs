using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Processor.Mps000276
{
    public class SereServADO : MOS.EFMODEL.DataModels.V_HIS_SERE_SERV
    {
        public long PARENT_SERVICE_ID { get; set; }
        public string EXECUTE_ROOM_ADDRESS { get; set; }
        public long SERVICE_ID_LOCAL_PARENT { get; set; }
        public decimal NUM_ORDER_FIXED { get; set; }
        public long? NUM_ORDER { get; set; }
        public string BARCODE { get; set; }

        public SereServADO(MOS.EFMODEL.DataModels.V_HIS_SERE_SERV sereServ)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(sereServ)));
            }
        }
    }
}
