using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HIS.Desktop.ADO;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.Core;
using Inventec.Desktop.Core.Tools;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.DebateDiagnostic.DebateDiagnostic
{
	internal class DebateDiagnosticBehavior : Tool<IDesktopToolContext>, IDebateDiagnostic
	{
		private object[] entity;

		internal DebateDiagnosticBehavior()
		{
		}

		internal DebateDiagnosticBehavior(CommonParam param, object[] data)
		{
			entity = data;
		}

		object IDebateDiagnostic.Run()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			Module val = null;
			TreatmentLogADO val2 = null;
			HIS_DEBATE val3 = new HIS_DEBATE();
			HIS_SERVICE val4 = new HIS_SERVICE();
			HIS_SERVICE_REQ val5 = new HIS_SERVICE_REQ();
			List<HIS_DEBATE> medicinePrints = null;
			long num = 0L;
			try
			{
				if (entity != null && entity.Count() > 0)
				{
					object[] array = entity;
					foreach (object obj in array)
					{
						if (obj is Module)
						{
							val = (Module)obj;
						}
						if (obj is TreatmentLogADO)
						{
							val2 = (TreatmentLogADO)obj;
						}
						if (obj is HIS_DEBATE)
						{
							val3 = (HIS_DEBATE)obj;
						}
						if (obj is HIS_SERVICE_REQ)
						{
							val5 = (HIS_SERVICE_REQ)obj;
						}
						if (obj is HIS_SERVICE)
						{
							val4 = (HIS_SERVICE)obj;
						}
						if (obj is V_HIS_SERVICE_REQ)
						{
							Mapper.CreateMap<V_HIS_SERVICE_REQ, HIS_SERVICE_REQ>();
							val5 = Mapper.Map<V_HIS_SERVICE_REQ, HIS_SERVICE_REQ>((V_HIS_SERVICE_REQ)obj);
						}
						if (obj is List<HIS_DEBATE>)
						{
							medicinePrints = (List<HIS_DEBATE>)obj;
						}
						if (obj is long)
						{
							num = (long)obj;
						}
					}
				}
				if (val != null && val2 != null)
				{
					return new FormDebateDiagnostic(val2, val, medicinePrints);
				}
				if (val != null && val4 != null && num != 0)
				{
					return new FormDebateDiagnostic(val4, medicinePrints, val, num);
				}
				if (val != null && val3 != null && val3.ID != 0)
				{
					return new FormDebateDiagnostic(val3, val, medicinePrints);
				}
				if (val != null && val5 != null)
				{
					return new FormDebateDiagnostic(val5, medicinePrints, val);
				}
				return null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return null;
			}
		}
	}
}
