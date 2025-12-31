using System;
using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;

namespace Inventec.Common.SignFile
{
	public class XmlConfig
	{
		public string NodeToSign { get; set; }

		public string Reason { get; set; }

		public string NameIDTimeSignature { get; set; }

		public XmlDsigSignatureFormat SigningType { get; set; }

		public bool IncludeTimestamp { get; set; }

		public DateTime SigningTime { get; set; }
	}
}
