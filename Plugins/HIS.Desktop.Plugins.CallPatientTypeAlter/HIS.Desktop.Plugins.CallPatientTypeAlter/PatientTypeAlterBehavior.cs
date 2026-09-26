using System;
using System.Collections.Generic;
using HIS.Desktop.ADO;
using HIS.Desktop.Common;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.Core;
using Inventec.Desktop.Core.Tools;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	public sealed class PatientTypeAlterBehavior : Tool<IDesktopToolContext>, IPatientTypeAlter
	{
		private Module module;

		private PatientTypeDepartmentADO HisTreatmentLogSDO;

		private long treatmentId;

		private bool? isView;

		private RefeshReference RefeshReference;

		private List<PatientTypeDepartmentADO> lstTreatmentLog;

		public PatientTypeAlterBehavior()
		{
		}

		public PatientTypeAlterBehavior(CommonParam param, Module module, PatientTypeDepartmentADO HisTreatmentLogSDO, long treatmentId, bool? isView, List<PatientTypeDepartmentADO> _lstTreatmentLog, RefeshReference RefeshReference)
		{
			this.module = module;
			this.HisTreatmentLogSDO = HisTreatmentLogSDO;
			this.treatmentId = treatmentId;
			this.isView = isView;
			this.RefeshReference = RefeshReference;
			lstTreatmentLog = _lstTreatmentLog;
		}

		object IPatientTypeAlter.Run()
		{
			try
			{
				if (treatmentId != 0L)
				{
					return new frmPatientTypeAlter(module, treatmentId, isView, lstTreatmentLog, RefeshReference);
				}
				return new frmPatientTypeAlter(module, HisTreatmentLogSDO, isView, lstTreatmentLog, RefeshReference);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return null;
			}
		}
	}
}
