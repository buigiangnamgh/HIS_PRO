using System;

namespace Inventec.Common.SignFile.XmlProcess.Common.Exceptions
{
	public class InvalidSignedDocumentException : Exception
	{
		public InvalidSignedDocumentException(string message)
			: base(message)
		{
		}
	}
}
