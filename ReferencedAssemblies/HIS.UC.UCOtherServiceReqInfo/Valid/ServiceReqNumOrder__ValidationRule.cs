using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using HIS.Desktop.Plugins.Library.RegisterConfig;
using HIS.UC.UCOtherServiceReqInfo.Resources;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class ServiceReqNumOrder__ValidationRule : ValidationRule
	{
		internal SpinEdit spinNumOrderPriority;

		public override bool Validate(Control control, object value)
		{
			bool flag = true;
			try
			{
				flag = flag && spinNumOrderPriority != null;
				if (flag && HisConfigCFG.ReservedNumOders != null && HisConfigCFG.ReservedNumOders.Count > 0 && spinNumOrderPriority.Value > 0m && !HisConfigCFG.ReservedNumOders.Contains(((long)spinNumOrderPriority.Value).ToString()))
				{
					flag = false;
					base.ErrorText = string.Format(ResourceMessage.SoThuTuUuTienPhaiNamTrongDanhSachCauHinhCacSoUuTien, "");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return flag;
		}
	}
}
