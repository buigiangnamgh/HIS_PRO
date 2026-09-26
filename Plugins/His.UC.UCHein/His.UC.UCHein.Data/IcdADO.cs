using System;
using Inventec.Common.Logging;
using Inventec.Common.String;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.Data
{
	public class IcdADO : HIS_ICD
	{
		public string ICD_NAME_UNSIGNED { get; set; }

		public IcdADO()
		{
		}

		public IcdADO(HIS_ICD data)
		{
			try
			{
				base.BYT_REPORT_CODE = data.BYT_REPORT_CODE;
				base.CHAPTER_CODE = data.CHAPTER_CODE;
				base.CHAPTER_NAME = data.CHAPTER_NAME;
				base.CHAPTER_NAME_EN = data.CHAPTER_NAME_EN;
				base.CREATE_TIME = data.CREATE_TIME;
				base.CREATOR = data.CREATOR;
				base.GROUP_CODE = data.GROUP_CODE;
				base.ICD_CHAPTER_ID = data.ICD_CHAPTER_ID;
				base.ICD_CODE = data.ICD_CODE;
				base.ICD_GROUP_ID = data.ICD_GROUP_ID;
				base.ICD_NAME = data.ICD_NAME;
				base.ICD_NAME_COMMON = data.ICD_NAME_COMMON;
				base.ICD_NAME_EN = data.ICD_NAME_EN;
				base.ID = data.ID;
				base.IS_ACTIVE = data.IS_ACTIVE;
				base.IS_DELETE = data.IS_DELETE;
				base.IS_HEIN_NDS = data.IS_HEIN_NDS;
				base.MODIFIER = data.MODIFIER;
				base.MODIFY_TIME = data.MODIFY_TIME;
				base.SUB_CODE = data.SUB_CODE;
				base.SUB_CODE_1 = data.SUB_CODE_1;
				base.SUB_CODE_2 = data.SUB_CODE_2;
				base.SUB_NAME = data.SUB_NAME;
				base.SUB_NAME_1 = data.SUB_NAME_1;
				base.SUB_NAME_1_EN = data.SUB_NAME_1_EN;
				base.SUB_NAME_2 = data.SUB_NAME_2;
				base.SUB_NAME_2_EN = data.SUB_NAME_2_EN;
				base.TYPE_CODE = data.TYPE_CODE;
				base.TYPE_NAME_EN = data.TYPE_NAME_EN;
				base.IS_REQUIRE_CAUSE = data.IS_REQUIRE_CAUSE;
				ICD_NAME_UNSIGNED = Inventec.Common.String.Convert.UnSignVNese2(base.ICD_NAME);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
