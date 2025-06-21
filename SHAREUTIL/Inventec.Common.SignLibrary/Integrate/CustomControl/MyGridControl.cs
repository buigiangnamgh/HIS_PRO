using System;

using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Registrator;

namespace Inventec.Common.Integrate.CustomControl
{
    internal class MyGridControl
		: GridControl
	{
		protected override void RegisterAvailableViewsCore(InfoCollection collection)
		{
			base.RegisterAvailableViewsCore(collection);
			collection.Add(new MyGridInfoRegistrator());
		}
	}
}