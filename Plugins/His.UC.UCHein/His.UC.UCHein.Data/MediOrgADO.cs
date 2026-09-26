using System;
using Inventec.Common.Logging;
using Inventec.Common.String;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.Data
{
	public class MediOrgADO : HIS_MEDI_ORG
	{
		public string MEDI_ORG_NAME_UNSIGNED { get; set; }

		public MediOrgADO()
		{
		}

		public MediOrgADO(HIS_MEDI_ORG data)
		{
			try
			{
				base.ADDRESS = data.ADDRESS;
				base.CREATE_TIME = data.CREATE_TIME;
				base.CREATOR = data.CREATOR;
				base.GROUP_CODE = data.GROUP_CODE;
				base.ID = data.ID;
				base.IS_ACTIVE = data.IS_ACTIVE;
				base.IS_DELETE = data.IS_DELETE;
				base.LEVEL_CODE = data.LEVEL_CODE;
				base.MEDI_ORG_CODE = data.MEDI_ORG_CODE;
				base.MEDI_ORG_NAME = data.MEDI_ORG_NAME;
				base.MODIFIER = data.MODIFIER;
				base.MODIFY_TIME = data.MODIFY_TIME;
				base.PROVINCE_CODE = data.PROVINCE_CODE;
				base.RANK_CODE = data.RANK_CODE;
				MEDI_ORG_NAME_UNSIGNED = Inventec.Common.String.Convert.UnSignVNese2(base.MEDI_ORG_NAME);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
