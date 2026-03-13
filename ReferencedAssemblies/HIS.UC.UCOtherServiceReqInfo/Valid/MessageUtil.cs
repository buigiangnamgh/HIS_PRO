using System;
using His.UC.LibraryMessage;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
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
	}
}
