using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Inventec.Common.Logging;

namespace His.UC.UCHein.Utils
{
	internal class HeinUtils
	{
		internal static string SetHeinCardNumberDisplayByNumber(string heinCardNumber)
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

		internal static string TrimHeinCardNumber(string chucodau)
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

		internal static DateTime? ConvertDateStringToSystemDate(string date)
		{
			DateTime? result = null;
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

		internal static string DateToDateRaw(string date)
		{
			string text = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(date))
				{
					char[] array = date.ToArray();
					for (int i = 0; i < array.Length; i++)
					{
						char c = array[i];
						if (c != '/')
						{
							text += c;
						}
					}
				}
			}
			catch (Exception ex)
			{
				text = "";
				LogSystem.Warn(ex);
			}
			return text;
		}
	}
}
