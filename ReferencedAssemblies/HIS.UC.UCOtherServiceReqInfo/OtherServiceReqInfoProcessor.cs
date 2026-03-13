using System;
using HIS.UC.UCOtherServiceReqInfo.ADO;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo
{
	internal class OtherServiceReqInfoProcessor
	{
		private Action<object> dlgFocusNextControl;

		internal UCOtherServiceReqInfo ControlWorker { get; set; }

		internal OtherServiceReqInfoProcessor()
		{
			Init();
		}

		private void Init()
		{
			try
			{
				ControlWorker = new UCOtherServiceReqInfo();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetValue(UCServiceReqInfoADO dataUseSetToForm)
		{
			try
			{
				ControlWorker.SetValue(dataUseSetToForm);
				dlgFocusNextControl = dataUseSetToForm._FocusNextUserControl;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public UCServiceReqInfoADO GetValue()
		{
			UCServiceReqInfoADO uCServiceReqInfoADO = new UCServiceReqInfoADO();
			try
			{
				return ControlWorker.GetValue();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return null;
			}
		}

		public void FocusUserControl()
		{
			try
			{
				ControlWorker.FocusUserControl();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void FocusNextUserControl()
		{
			try
			{
				ControlWorker.FocusNextUserControl(dlgFocusNextControl);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
