using AutoMapper;
using Inventec.Common.String;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.DebateDiagnostic.ADO
{
	internal class MedicineTypeADO : V_HIS_MEDICINE_TYPE
	{
		public bool IsChecked { get; set; }

		public string MEDICINE_TYPE_NAME__UNSIGN { get; set; }

		public MedicineTypeADO(V_HIS_MEDICINE_TYPE item)
		{
			Mapper.CreateMap<V_HIS_MEDICINE_TYPE, MedicineTypeADO>();
			Mapper.Map<V_HIS_MEDICINE_TYPE, MedicineTypeADO>(item, this);
			MEDICINE_TYPE_NAME__UNSIGN = Convert.UnSignVNese(((V_HIS_MEDICINE_TYPE)this).MEDICINE_TYPE_NAME);
		}
	}
}
