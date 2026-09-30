using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using Inventec.Common.Logging;
using Inventec.Common.TypeConvert;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.ServiceReqList.NumCopy
{
	internal class NumCopyConfig
	{
		private const string numCopy = "HIS.Desktop.Plugins.PrintNow.NumCopy";

		public static List<NumCopyADO> NumCopys
		{
			get
			{
				List<NumCopyADO> list = new List<NumCopyADO>();
				List<string> types = GetTypes("HIS.Desktop.Plugins.PrintNow.NumCopy");
				if (types != null && types.Count > 0)
				{
					foreach (string item in types)
					{
						try
						{
							string[] tmp = item.Split(':');
							if (tmp != null && tmp.Length >= 2)
							{
								NumCopyADO numCopyADO = new NumCopyADO();
								List<HIS_PATIENT_TYPE> list2 = (from o in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
									where o.PATIENT_TYPE_CODE == tmp[0]
									select o).ToList();
								if (list2 != null && list2.Count == 1)
								{
									numCopyADO.id = list2.FirstOrDefault().ID;
									numCopyADO.num = Parse.ToInt32(tmp[1]);
									list.Add(numCopyADO);
								}
							}
						}
						catch (Exception ex)
						{
							LogSystem.Error(ex);
						}
					}
				}
				return list;
			}
		}

		private static List<string> GetTypes(string code)
		{
			List<string> list = new List<string>();
			try
			{
				string text = ConfigApplicationWorker.Get<string>(code);
				if (string.IsNullOrEmpty(text))
				{
					throw new ArgumentNullException(code);
				}
				list.AddRange(text.Split(','));
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				list = new List<string>();
			}
			return list;
		}
	}
}
