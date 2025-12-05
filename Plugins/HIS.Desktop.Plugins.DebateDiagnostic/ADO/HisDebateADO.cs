using AutoMapper;
using Inventec.Common.DateTime;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.DebateDiagnostic.ADO
{
	internal class HisDebateADO : HIS_DEBATE
	{
		public string HisDebateTimeString { get; set; }

		public string ContentTypeName { get; set; }

		public HisDebateADO(HIS_DEBATE item)
		{
			Mapper.CreateMap<HIS_DEBATE, HisDebateADO>();
			Mapper.Map<HIS_DEBATE, HisDebateADO>(item, this);
			if (((HIS_DEBATE)this).CONTENT_TYPE == 1)
			{
				ContentTypeName = "Hội chẩn khác";
			}
			else if (((HIS_DEBATE)this).CONTENT_TYPE == 2)
			{
				ContentTypeName = "Hội chẩn thuốc";
			}
			else
			{
				ContentTypeName = "Hội chẩn trước phẫu thuật";
			}
			HisDebateTimeString = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((HIS_DEBATE)this).DEBATE_TIME.GetValueOrDefault());
		}
	}
}
