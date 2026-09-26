using System;
using His.UC.LibraryMessage;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Base
{
	public class MessageUtil
	{
		public static string GetMessage(Message.Enum MessageCaseEnum)
		{
			string result = "";
			try
			{
				Message message = FontendMessage.Get(TokenStore.language, MessageCaseEnum);
				if (message != null)
				{
					result = message.message;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error("Co exception khi GetMessage.", ex);
			}
			return result;
		}

		public static string GetMessage(Message.Enum MessageCaseEnum, string[] extraMessage)
		{
			string result = "";
			try
			{
				Message message = FontendMessage.Get(TokenStore.language, MessageCaseEnum);
				if (message != null)
				{
					try
					{
						result = string.Format(message.message, extraMessage);
					}
					catch (Exception ex)
					{
						LogSystem.Error("Co exception khi set message vao param.listMessage co tham so phu.", ex);
						result = string.Format(message.message);
					}
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error("Co exception khi SetParam co tham so phu.", ex2);
			}
			return result;
		}

		public static void SetMessage(CommonParam param, Message.Enum en)
		{
			try
			{
				Message message = FontendMessage.Get(TokenStore.language, en);
				if (message != null)
				{
					param.Messages.Add(message.message);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error("Co exception khi SetParam.", ex);
			}
		}

		public static void SetMessage(CommonParam param, Message.Enum en, string extraMessage)
		{
			try
			{
				Message message = FontendMessage.Get(TokenStore.language, en);
				if (message != null)
				{
					try
					{
						param.Messages.Add(string.Format(message.message, extraMessage));
						return;
					}
					catch (Exception ex)
					{
						LogSystem.Error("Co exception khi set message vao param.Messages co tham so phu.", ex);
						param.Messages.Add(message.message);
						return;
					}
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error("Co exception khi SetParam co tham so phu.", ex2);
			}
		}

		public static void SetMessage(CommonParam param, Message.Enum en, string[] extraMessage)
		{
			try
			{
				Message message = FontendMessage.Get(TokenStore.language, en);
				if (message != null)
				{
					try
					{
						param.Messages.Add(string.Format(message.message, extraMessage));
						return;
					}
					catch (Exception ex)
					{
						LogSystem.Error("Co exception khi set message vao param.Messages co tham so phu.", ex);
						param.Messages.Add(message.message);
						return;
					}
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error("Co exception khi SetParam co tham so phu.", ex2);
			}
		}

		public static void SetParam(CommonParam param, Message.Enum MessageCaseEnum)
		{
			try
			{
				Message message = FontendMessage.Get(TokenStore.language, MessageCaseEnum);
				if (message != null)
				{
					param.Messages.Add(message.message);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error("Co exception khi SetParam.", ex);
			}
		}

		public static void SetParamFirstPostion(CommonParam param, Message.Enum MessageCaseEnum)
		{
			try
			{
				Message message = FontendMessage.Get(TokenStore.language, MessageCaseEnum);
				if (message != null)
				{
					param.Messages.Insert(0, message.message);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error("Co exception khi SetParam.", ex);
			}
		}

		public static void SetParam(CommonParam param, Message.Enum MessageCaseEnum, string[] extraMessage)
		{
			try
			{
				Message message = FontendMessage.Get(TokenStore.language, MessageCaseEnum);
				if (message != null)
				{
					try
					{
						param.Messages.Add(string.Format(message.message, extraMessage));
						return;
					}
					catch (Exception ex)
					{
						LogSystem.Error("Co exception khi set message vao param.listMessage co tham so phu.", ex);
						param.Messages.Add(message.message);
						return;
					}
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error("Co exception khi SetParam co tham so phu.", ex2);
			}
		}

		public static string GetMessageAlert(CommonParam param)
		{
			string text = "";
			try
			{
				if (param.Messages != null && param.Messages.Count > 0)
				{
					text += param.GetMessage();
				}
				if (param.BugCodes != null && param.BugCodes.Count > 0)
				{
					text = text + "\r\nMã sự cố: " + param.GetBugCode();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error("Co exception khi GetMessageAlert.", ex);
			}
			return text;
		}

		public static void SetResultParam(CommonParam param, bool success)
		{
			try
			{
				if (success)
				{
					SetParamFirstPostion(param, Message.Enum.HeThongTBKQXLYCCuaFrontendThanhCong);
				}
				else
				{
					SetParamFirstPostion(param, Message.Enum.HeThongTBKQXLYCCuaFrontendThatBai);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error("Co exception khi SetResultParam.", ex);
			}
		}
	}
}
