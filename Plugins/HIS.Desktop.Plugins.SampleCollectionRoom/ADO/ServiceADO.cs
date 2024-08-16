using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.SampleCollectionRoom.ADO
{
    public class ServiceADO
    {
        public long IdService { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public Nullable<long> ParentServiceId { get; set; }
    }
}
