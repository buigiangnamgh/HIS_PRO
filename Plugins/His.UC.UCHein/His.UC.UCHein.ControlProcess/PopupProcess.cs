using System;
using DevExpress.Utils.Win;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Popup;
using Inventec.Common.Logging;

namespace His.UC.UCHein.ControlProcess
{
	public class PopupProcess
	{
		public static void SelectFirstRowPopup(LookUpEdit cbo)
		{
			try
			{
				if (cbo != null && cbo.IsPopupOpen)
				{
					PopupLookUpEditForm popupLookUpEditForm = ((IPopupControl)cbo).PopupWindow as PopupLookUpEditForm;
					if (popupLookUpEditForm != null)
					{
						popupLookUpEditForm.SelectedIndex = 0;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static void SelectFirstRowPopup(GridLookUpEdit cbo)
		{
			try
			{
				if (cbo != null && cbo.IsPopupOpen)
				{
					PopupLookUpEditForm popupLookUpEditForm = ((IPopupControl)cbo).PopupWindow as PopupLookUpEditForm;
					if (popupLookUpEditForm != null)
					{
						popupLookUpEditForm.SelectedIndex = 0;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
