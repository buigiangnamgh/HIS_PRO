using System.Collections.Generic;
using His.UC.UCHein.Base;

namespace His.UC.UCHein
{
	public class Compare : IEqualityComparer<PatientTypeAlterADO>
	{
		public bool Equals(PatientTypeAlterADO x, PatientTypeAlterADO y)
		{
			if (x.HEIN_CARD_NUMBER == y.HEIN_CARD_NUMBER && x.HEIN_CARD_FROM_TIME == y.HEIN_CARD_FROM_TIME)
			{
				return x.HEIN_CARD_TO_TIME == y.HEIN_CARD_TO_TIME;
			}
			return false;
		}

		public int GetHashCode(PatientTypeAlterADO x)
		{
			return ((!string.IsNullOrEmpty(x.HEIN_CARD_NUMBER)) ? x.HEIN_CARD_NUMBER.GetHashCode() : 0) + (x.HEIN_CARD_FROM_TIME.HasValue ? x.HEIN_CARD_FROM_TIME.GetHashCode() : 0) + (x.HEIN_CARD_TO_TIME.HasValue ? x.HEIN_CARD_TO_TIME.GetHashCode() : 0);
		}
	}
}
