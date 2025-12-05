using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.DebateDiagnostic.ADO
{
	internal class HisDebateUserADO : HIS_DEBATE_USER
	{
		public int Action { get; set; }

		public bool PRESIDENT { get; set; }

		public bool SECRETARY { get; set; }

		public bool OUT_OF_HOSPITAL { get; set; }
	}
}
