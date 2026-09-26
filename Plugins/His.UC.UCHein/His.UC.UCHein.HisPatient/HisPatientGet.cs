using System;
using System.Collections.Generic;
using His.UC.UCHein.Base;
using His.UC.UCHein.Utils;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.Filter;
using MOS.SDO;

namespace His.UC.UCHein.HisPatient
{
	internal class HisPatientGet
	{
		internal static List<HisPatientSDO> GetSDO(string heinCardNumber)
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				HisPatientAdvanceFilter hisPatientAdvanceFilter = new HisPatientAdvanceFilter();
				hisPatientAdvanceFilter.HEIN_CARD_NUMBER__EXACT = HeinUtils.TrimHeinCardNumber(heinCardNumber.Replace(" ", "").Replace("  ", "").ToUpper()
					.Trim());
				return new BackendAdapter(commonParam).Get<List<HisPatientSDO>>("api/HisPatient/GetSdoAdvance", ApiConsumerStore.MosConsumer, hisPatientAdvanceFilter, commonParam);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return null;
		}
	}
}
