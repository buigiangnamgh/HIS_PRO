using DevExpress.XtraEditors.Controls;

namespace His.UC.UCHein.Base
{
	public class NewLocalizerDX : Localizer
	{
		public override string GetLocalizedString(StringId id)
		{
			switch (id)
			{
			case StringId.XtraMessageBoxYesButtonText:
				return MessageBoxManager.Yes;
			case StringId.XtraMessageBoxNoButtonText:
				return MessageBoxManager.No;
			case StringId.XtraMessageBoxOkButtonText:
				return MessageBoxManager.OK;
			case StringId.XtraMessageBoxCancelButtonText:
				return MessageBoxManager.Cancel;
			default:
				return base.GetLocalizedString(id);
			}
		}
	}
}
