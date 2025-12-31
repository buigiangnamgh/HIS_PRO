using System;
using System.Runtime.Serialization;

namespace Inventec.Common.SignFile.XmlProcess.Common.Exceptions
{
	[Serializable]
	internal class InvalidDocumentException : Exception
	{
		public InvalidDocumentException()
		{
		}

		public InvalidDocumentException(string message)
			: base(message)
		{
		}

		public InvalidDocumentException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected InvalidDocumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
