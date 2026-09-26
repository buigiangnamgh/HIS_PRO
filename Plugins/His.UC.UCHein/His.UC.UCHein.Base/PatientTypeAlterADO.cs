using System;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.Base
{
	public class PatientTypeAlterADO : HIS_PATIENT_TYPE_ALTER
	{
		public long HEIN_CARD_TO_TIME_CAL { get; set; }

		public string RENDERER_HEIN_CARD_NUMBER { get; set; }

		public string RENDERER_FROM_DATE_TODATE { get; set; }

		public PatientTypeAlterADO(HIS_PATIENT_TYPE_ALTER data)
		{
			try
			{
				DataObjectMapper.Map<HIS_PATIENT_TYPE_ALTER>(this, data);
				if (data != null)
				{
					string text = "";
					string text2 = "";
					string text3 = "";
					string text4 = "";
					string text5 = "";
					string text6 = "";
					try
					{
						text = data.HEIN_CARD_NUMBER.Substring(0, 2);
						text2 = data.HEIN_CARD_NUMBER.Substring(2, 1);
						text3 = data.HEIN_CARD_NUMBER.Substring(3, 2);
						text4 = data.HEIN_CARD_NUMBER.Substring(5, 2);
						text5 = data.HEIN_CARD_NUMBER.Substring(7, 3);
						text6 = data.HEIN_CARD_NUMBER.Substring(10);
					}
					catch (Exception ex)
					{
						LogSystem.Warn("Gan chuoi RENDERER_HEIN_CARD_NUMBER the BHYT loi", ex);
					}
					RENDERER_HEIN_CARD_NUMBER = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", text, text2, text3, text4, text5, text6);
				}
				if (data != null && data.HEIN_CARD_FROM_TIME > 0 && data.HEIN_CARD_TO_TIME > 0)
				{
					string text7 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.HEIN_CARD_FROM_TIME.GetValueOrDefault());
					string text8 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.HEIN_CARD_TO_TIME.GetValueOrDefault());
					RENDERER_FROM_DATE_TODATE = text7 + " - " + text8;
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}
	}
}
