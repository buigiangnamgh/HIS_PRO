using System;
using ACS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;

namespace HIS.Desktop.Plugins.DebateDiagnostic.ADO
{
	public class AcsUserADO : ACS_USER
	{
		public string DEPARTMENT_NAME { get; set; }

		public long? DOB { get; set; }

		public string DOB_STR { get; set; }

		public string DIPLOMA { get; set; }

		public string DEPARTMENT_CODE { get; set; }

		public long? DEPARTMENT_ID { get; set; }

		public AcsUserADO()
		{
		}

		public AcsUserADO(ACS_USER data)
		{
			try
			{
				if (data != null)
				{
					DataObjectMapper.Map<AcsUserADO>((object)this, (object)data);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
