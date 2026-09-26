using System;
using DevExpress.XtraEditors;
using His.UC.UCHein.ControlProcess;
using Inventec.Common.Logging;

namespace His.UC.UCHein
{
	internal class ResetEditorControl
	{
		internal static void Reset(BaseEdit editor)
		{
			try
			{
				editor.Reset();
				editor.EditValue = null;
				editor.Text = "";
				if (editor is LookUpEdit)
				{
					if (((LookUpEdit)editor).Properties.Buttons.Count > 1)
					{
						((LookUpEdit)editor).Properties.Buttons[1].Visible = false;
					}
				}
				else if (editor is GridLookUpEdit && ((GridLookUpEdit)editor).Properties.Buttons.Count > 1)
				{
					((GridLookUpEdit)editor).Properties.Buttons[1].Visible = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal static void ResetAndFocus(LookUpEdit cboEditor, bool isVisibleButtonDel)
		{
			try
			{
				cboEditor.EditValue = null;
				if (isVisibleButtonDel && cboEditor.Properties.Buttons.Count > 1)
				{
					cboEditor.Properties.Buttons[1].Visible = false;
				}
				cboEditor.Focus();
				cboEditor.ShowPopup();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal static void ResetAndFocus(GridLookUpEdit cboEditor, bool isVisibleButtonDel)
		{
			try
			{
				cboEditor.EditValue = null;
				if (isVisibleButtonDel && cboEditor.Properties.Buttons.Count > 1)
				{
					cboEditor.Properties.Buttons[1].Visible = false;
				}
				cboEditor.Focus();
				cboEditor.ShowPopup();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal static void ResetAndFocus(LookUpEdit cboEditor, bool isVisibleButtonDel, bool isSelectFirstRowPopup)
		{
			try
			{
				cboEditor.EditValue = null;
				if (isVisibleButtonDel && cboEditor.Properties.Buttons.Count > 1)
				{
					cboEditor.Properties.Buttons[1].Visible = false;
				}
				cboEditor.Focus();
				cboEditor.ShowPopup();
				if (isSelectFirstRowPopup)
				{
					PopupProcess.SelectFirstRowPopup(cboEditor);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal static void ResetAndFocus(GridLookUpEdit cboEditor, bool isVisibleButtonDel, bool isSelectFirstRowPopup)
		{
			try
			{
				cboEditor.EditValue = null;
				if (isVisibleButtonDel && cboEditor.Properties.Buttons.Count > 1)
				{
					cboEditor.Properties.Buttons[1].Visible = false;
				}
				cboEditor.Focus();
				cboEditor.ShowPopup();
				if (isSelectFirstRowPopup)
				{
					PopupProcess.SelectFirstRowPopup(cboEditor);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
