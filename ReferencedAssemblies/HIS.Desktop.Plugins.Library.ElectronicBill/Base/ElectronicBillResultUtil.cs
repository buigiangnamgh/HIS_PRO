using System;
using System.Collections.Generic;
using System.IO;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.Base
{
	public class ElectronicBillResultUtil
	{
		public static void Set(ref ElectronicBillResult electronicBillResult, bool success, string message)
		{
			try
			{
				if (electronicBillResult == null)
				{
					electronicBillResult = new ElectronicBillResult();
				}
				if (electronicBillResult.Messages == null)
				{
					electronicBillResult.Messages = new List<string>();
				}
				electronicBillResult.Success = success;
				if (!string.IsNullOrEmpty(message))
				{
					electronicBillResult.Messages.Add(message);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static void Set(ref ElectronicBillResult electronicBillResult, bool success, List<string> messages)
		{
			try
			{
				if (electronicBillResult == null)
				{
					electronicBillResult = new ElectronicBillResult();
				}
				if (electronicBillResult.Messages == null)
				{
					electronicBillResult.Messages = new List<string>();
				}
				electronicBillResult.Success = success;
				if (messages != null && messages.Count > 0)
				{
					electronicBillResult.Messages.AddRange(messages);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static string ProcessPdfFileResult(string base64string)
		{
			string result = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(base64string))
				{
					byte[] fileToBytes = System.Convert.FromBase64String(base64string);
					result = ProcessPdfFileResult(fileToBytes);
				}
			}
			catch (Exception ex)
			{
				result = "";
				LogSystem.Error(ex);
			}
			return result;
		}

		public static string ProcessPdfFileResult(byte[] fileToBytes)
		{
			string text = "";
			try
			{
				string tempFileName = Path.GetTempFileName();
				tempFileName = tempFileName.Replace("tmp", "pdf");
				try
				{
					File.WriteAllBytes(tempFileName, fileToBytes);
					text = tempFileName;
				}
				catch (Exception ex)
				{
					text = "";
					LogSystem.Error(ex);
				}
			}
			catch (Exception ex2)
			{
				text = "";
				LogSystem.Error(ex2);
			}
			return text;
		}
	}
}
