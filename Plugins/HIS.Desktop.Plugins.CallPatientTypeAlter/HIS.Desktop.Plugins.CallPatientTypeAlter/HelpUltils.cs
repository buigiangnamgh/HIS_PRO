using System;
using System.Text;
using System.Text.RegularExpressions;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	public class HelpUltils
	{
		public static string CalculateAgeFromYear(long ageYearNumber)
		{
			string text = "";
			try
			{
				DateTime value = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageYearNumber).Value;
				long ticks = (DateTime.Now - value).Ticks;
				if (ticks < 0)
				{
					text = "";
					return "";
				}
				DateTime dateTime = new DateTime(ticks);
				int num = dateTime.Year - 1;
				int num2 = dateTime.Month - 1;
				int num3 = dateTime.Day - 1;
				int hour = dateTime.Hour;
				int minute = dateTime.Minute;
				int second = dateTime.Second;
				long num4 = 0L;
				if (num > 0)
				{
					return string.Format("{0:00}", (long)num + " tuổi");
				}
				if (num2 > 0)
				{
					return string.Format("{0:00}", (long)num2 + " tháng");
				}
				if (num3 > 0)
				{
					return string.Format("{0:00}", (long)num3 + " ngày");
				}
				return string.Format("{0:00}", (long)hour + " giờ");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return "";
			}
		}

		public static string SetHeinCardNumberDisplayByNumber(string heinCardNumber)
		{
			string text = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(heinCardNumber) && (heinCardNumber.Length == 15 || heinCardNumber.Length == 17))
				{
					string value = "-";
					return (heinCardNumber.Length == 17) ? new StringBuilder().Append(heinCardNumber.Substring(0, 2)).Append(value).Append(heinCardNumber.Substring(2, 1))
						.Append(value)
						.Append(heinCardNumber.Substring(3, 2))
						.Append(value)
						.Append(heinCardNumber.Substring(5))
						.ToString() : new StringBuilder().Append(heinCardNumber.Substring(0, 2)).Append(value).Append(heinCardNumber.Substring(2, 1))
						.Append(value)
						.Append(heinCardNumber.Substring(3, 2))
						.Append(value)
						.Append(heinCardNumber.Substring(5, 2))
						.Append(value)
						.Append(heinCardNumber.Substring(7, 3))
						.Append(value)
						.Append(heinCardNumber.Substring(10, 5))
						.ToString();
				}
				return heinCardNumber;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return heinCardNumber;
			}
		}

		public static string TrimHeinCardNumber(string chucodau)
		{
			string result = "";
			try
			{
				result = Regex.Replace(chucodau, "[-,_ ]|[_]{2}|[_]{3}|[_]{4}|[_]{5}", "").ToUpper();
			}
			catch (Exception)
			{
			}
			return result;
		}

		public static DateTime? ConvertDateStringToSystemDate(string date)
		{
			DateTime? result = DateTime.MinValue;
			try
			{
				if (!string.IsNullOrEmpty(date))
				{
					date = date.Replace(" ", "");
					int day = short.Parse(date.Substring(0, 2));
					int month = short.Parse(date.Substring(3, 2));
					int year = short.Parse(date.Substring(6, 4));
					return new DateTime(year, month, day);
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		public static DateTime? ConvertDateTimeStringToSystemTime(string datetime)
		{
			DateTime? result = DateTime.MinValue;
			try
			{
				if (!string.IsNullOrEmpty(datetime))
				{
					int day = short.Parse(datetime.Substring(0, 2));
					int month = short.Parse(datetime.Substring(3, 2));
					int year = short.Parse(datetime.Substring(6, 4));
					int hour = short.Parse(datetime.Substring(11, 2));
					int minute = short.Parse(datetime.Substring(14, 2));
					return new DateTime(year, month, day, hour, minute, 0);
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}
	}
}
