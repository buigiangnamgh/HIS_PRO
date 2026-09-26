using System;
using HIS.Desktop.Plugins.Library.CheckHeinGOV;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.CheckInfoBHYT.ADO
{
	public class PatientTypeAlterADO : V_HIS_PATIENT_TYPE_ALTER
	{
		public ResultDataADO ResultDataADO { get; set; }

		public PatientTypeAlterADO()
		{
		}

		public PatientTypeAlterADO(V_HIS_PATIENT_TYPE_ALTER data)
		{
			try
			{
				if (data != null)
				{
					DataObjectMapper.Map<PatientTypeAlterADO>(this, data);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
