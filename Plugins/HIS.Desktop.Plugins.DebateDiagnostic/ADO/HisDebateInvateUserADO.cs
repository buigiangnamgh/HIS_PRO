using DevExpress.XtraEditors.DXErrorProvider;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.DebateDiagnostic.ADO
{
	internal class HisDebateInvateUserADO : HIS_DEBATE_INVITE_USER
	{
		public int Action { get; set; }

		public ErrorType ErrorTypeCommentDoctor { get; set; }

		public string ErrorMessageCommentDoctor { get; set; }

		public string IS_PARTICIPATION_str { get; set; }

		public bool PRESIDENT { get; set; }

		public bool SECRETARY { get; set; }
	}
}
