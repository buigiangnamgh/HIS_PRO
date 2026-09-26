using System;
using System.Windows.Forms;
using HIS.Desktop.Plugins.Library.CheckHeinGOV;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Core.SetResultDataADO
{
	internal class SetResultDataADOBehavior : BeanObjectBase, ISetResultDataADO
	{
		private UserControl UC;

		private ResultDataADO ResultDataADO;

		internal SetResultDataADOBehavior(CommonParam param, UserControl uc, ResultDataADO ResultDataADO)
			: base(param)
		{
			UC = uc;
			this.ResultDataADO = ResultDataADO;
		}

		void ISetResultDataADO.Run()
		{
			try
			{
				((Template__HeinBHYT1)UC).SetRsDataADO(ResultDataADO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				base.param.HasException = true;
			}
		}
	}
}
