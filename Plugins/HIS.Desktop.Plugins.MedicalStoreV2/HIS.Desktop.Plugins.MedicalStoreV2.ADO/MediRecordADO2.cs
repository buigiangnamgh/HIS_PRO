using System.Collections.Generic;
using Inventec.Common.DateTime;
using Inventec.Common.Mapper;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.MedicalStoreV2.ADO
{
	public class MediRecordADO2 : V_HIS_MEDI_RECORD_2
	{
		public string INTIME_SPLCONCAT { get; set; }

		public string OUTTIME_SPLCONCAT { get; set; }

		public MediRecordADO2()
		{
		}

		public MediRecordADO2(V_HIS_MEDI_RECORD_2 data)
		{
			if (data == null)
			{
				return;
			}
			DataObjectMapper.Map<MediRecordADO2>(this, data);
			if (((V_HIS_MEDI_RECORD_2)this).IN_TIME != null)
			{
				string[] array = ((V_HIS_MEDI_RECORD_2)this).IN_TIME.Split(',');
				List<string> list = new List<string>();
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(Convert.TimeNumberToTimeString(long.Parse(array[i])));
				}
				INTIME_SPLCONCAT = string.Join(", ", list);
			}
			if (((V_HIS_MEDI_RECORD_2)this).OUT_TIME != null)
			{
				string[] array2 = ((V_HIS_MEDI_RECORD_2)this).OUT_TIME.Split(',');
				List<string> list2 = new List<string>();
				for (int j = 0; j < array2.Length; j++)
				{
					list2.Add(Convert.TimeNumberToTimeString(long.Parse(array2[j])));
				}
				OUTTIME_SPLCONCAT = string.Join(", ", list2);
			}
		}
	}
}
