using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using HIS.Desktop.Utility;
using HIS.UC.UCOtherServiceReqInfo.Resources;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Valid
{
	internal class IntructionTime__ValidationRule : ValidationRule
	{
		internal TextEdit txtIntructionTime;

		public override bool Validate(Control control, object value)
		{
			bool flag = true;
			try
			{
				flag = flag && txtIntructionTime != null;
				if (flag)
				{
					string text = "";
					if (string.IsNullOrEmpty(txtIntructionTime.Text))
					{
						return false;
					}
					DateTime? dateTime = DateTimeHelper.ConvertDateStringToSystemDate(txtIntructionTime.Text);
					if (!dateTime.HasValue || dateTime.Value == DateTime.MinValue)
					{
						flag = false;
						text = ResourceMessage.NhapNgayThangKhongDungDinhDang;
					}
					else if (txtIntructionTime.Text.ToString().Substring(6, 1) == "0")
					{
						flag = false;
						text = ResourceMessage.NhapNgayThangKhongDungDinhDang;
					}
					else
					{
						try
						{
							DateTime.ParseExact(txtIntructionTime.Text, "dd/MM/yyyy HH:mm", null);
						}
						catch (Exception ex)
						{
							flag = false;
							text = ResourceMessage.NhapNgayThangKhongDungDinhDang;
							LogSystem.Error(ex);
						}
					}
					base.ErrorText = text;
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
			return flag;
		}
	}
}
