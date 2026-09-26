using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Dispose
{
	public sealed class DisposeBehavior : IDispose
	{
		private UserControl control;

		public DisposeBehavior()
		{
		}

		public DisposeBehavior(CommonParam param, UserControl uc)
		{
			control = uc;
		}

		void IDispose.Run()
		{
			try
			{
				((Template__HeinBHYT1)control).DisposeControl();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
