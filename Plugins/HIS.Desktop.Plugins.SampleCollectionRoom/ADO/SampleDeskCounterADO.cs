using LIS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.SampleCollectionRoom.ADO
{
    public class SampleDeskCounterADO : HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.L_HIS_SAMPLE_DESK_COUNTER
    {
        public SampleDeskCounterADO()
        {

        }

        public SampleDeskCounterADO(HIS.Desktop.LocalStorage.BackendData.V2.EFMODEL.L_HIS_SAMPLE_DESK_COUNTER data)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<SampleDeskCounterADO>(this, data);
        }
        public bool IsChecked { get; set; }
        public bool IsPriority { get; set; }
    }
}
