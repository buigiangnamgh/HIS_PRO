using System;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.TypeConvert;

namespace MPS.Processor.Mps000302
{
	internal static class AgeUtil
	{
		internal static string CalculateFullAge(long ageNumber)
		{
			try
			{
				DateTime dateTime = Parse.ToDateTime(Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ageNumber));
				long ticks = (DateTime.Now - dateTime).Ticks;
				string text;
				string text2;
				if (ticks < 0)
				{
					text = "";
					text2 = "Tuổi";
					return "";
				}
				DateTime dateTime2 = new DateTime(ticks);
				int num = dateTime2.Year - 1;
				int num2 = dateTime2.Month - 1;
				int num3 = dateTime2.Day - 1;
				int hour = dateTime2.Hour;
				int minute = dateTime2.Minute;
				int second = dateTime2.Second;
				if (num > 0)
				{
					text = num.ToString();
					text2 = "Tuổi";
				}
				else if (num2 > 0)
				{
					text = num2.ToString();
					text2 = "Tháng";
				}
				else if (num3 > 0)
				{
					text = num3.ToString();
					text2 = "ngày";
				}
				else
				{
					text = "";
					text2 = "Giờ";
				}
				return text + " " + text2;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return "";
			}
		}
	}
}
