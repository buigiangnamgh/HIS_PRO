using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ACS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Desktop.Common.Modules;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.DebateDiagnostic.UcDebateDetail
{
	internal class DetailProcessor
	{
		private UcPttt Pttt;

		private UcOther Thuoc;

		private UcOther Khac;

		private long TreatmentId;

		private long RoomId;

		private long RoomTypeId;

		private HIS_SERVICE hisService;

		private bool IsSetPttt;

		private bool IsSetThuoc;

		private bool IsSetKhac;

		private Module module;

		public List<ACS_USER> UserList;

		public List<V_HIS_EMPLOYEE> EmployeeList;

		public List<HIS_DEPARTMENT> DepartmentList;

		public List<HIS_EXECUTE_ROLE> ExecuteRoleList;

		public DetailProcessor(long treatmentId, long roomId, long roomTypeId, Module module)
		{
			TreatmentId = treatmentId;
			RoomId = roomId;
			RoomTypeId = roomTypeId;
			this.module = module;
		}

		public DetailProcessor(long treatmentId, long roomId, long roomTypeId, HIS_SERVICE hisServicee, Module module)
		{
			TreatmentId = treatmentId;
			RoomId = roomId;
			RoomTypeId = roomTypeId;
			hisService = hisService;
			this.module = module;
		}

		public UserControl GetControl(DetailEnum type)
		{
			UserControl result = null;
			try
			{
				switch (type)
				{
				case DetailEnum.Thuoc:
					if (Thuoc == null)
					{
						Thuoc = new UcOther(TreatmentId, RoomId, RoomTypeId, false, module);
					}
					result = Thuoc;
					break;
				case DetailEnum.Pttt:
					if (Pttt == null)
					{
						Pttt = new UcPttt(TreatmentId, RoomId, RoomTypeId, UserList, EmployeeList, DepartmentList, ExecuteRoleList);
					}
					result = Pttt;
					break;
				case DetailEnum.Khac:
					if (Khac == null)
					{
						if (hisService != null)
						{
							Khac = new UcOther(TreatmentId, RoomId, RoomTypeId, true, hisService, module);
						}
						else
						{
							Khac = new UcOther(TreatmentId, RoomId, RoomTypeId, true, module);
						}
					}
					else if (hisService != null)
					{
						Khac = new UcOther(TreatmentId, RoomId, RoomTypeId, true, hisService, module);
					}
					result = Khac;
					break;
				}
			}
			catch (Exception ex)
			{
				result = null;
				LogSystem.Error(ex);
			}
			return result;
		}

		public void GetData(DetailEnum type, ref HIS_DEBATE saveData)
		{
			try
			{
				switch (type)
				{
				case DetailEnum.Thuoc:
					if (Thuoc != null)
					{
						Thuoc.GetData(ref saveData);
					}
					break;
				case DetailEnum.Pttt:
					if (Pttt != null)
					{
						Pttt.GetData(ref saveData);
					}
					break;
				case DetailEnum.Khac:
					if (Khac != null)
					{
						Khac.GetData(ref saveData);
					}
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void SetDataDiscussion(DetailEnum type, string content)
		{
			try
			{
				switch (type)
				{
				case DetailEnum.Thuoc:
					if (Thuoc != null)
					{
						Thuoc.SetContent(content);
					}
					break;
				case DetailEnum.Khac:
					if (Khac != null)
					{
						Khac.SetContent(content);
					}
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public bool ValidateControl(DetailEnum type)
		{
			bool result = false;
			try
			{
				switch (type)
				{
				case DetailEnum.Thuoc:
					if (Thuoc == null)
					{
						return false;
					}
					result = Thuoc.ValidControl();
					break;
				case DetailEnum.Pttt:
					if (Pttt == null)
					{
						return false;
					}
					result = Pttt.ValidControl();
					break;
				case DetailEnum.Khac:
					if (Khac == null)
					{
						return false;
					}
					result = Khac.ValidControl();
					break;
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Error(ex);
			}
			return result;
		}

		public void DisableControlItem(DetailEnum type)
		{
			try
			{
				switch (type)
				{
				case DetailEnum.Thuoc:
					if (Thuoc != null)
					{
						Thuoc.DisableControlItem();
					}
					break;
				case DetailEnum.Pttt:
					if (Pttt != null)
					{
						Pttt.DisableControlItem();
					}
					break;
				case DetailEnum.Khac:
					if (Khac != null)
					{
						Khac.DisableControlItem();
					}
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void SetData(DetailEnum type, object data)
		{
			try
			{
				switch (type)
				{
				case DetailEnum.Thuoc:
					if (Thuoc != null)
					{
						Thuoc.SetData(data);
					}
					break;
				case DetailEnum.Pttt:
					if (Pttt != null)
					{
						Pttt.SetData(data);
					}
					break;
				case DetailEnum.Khac:
					if (Khac != null)
					{
						Khac.SetData(data);
					}
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void SetDataMedicine(HIS_DEBATE data)
		{
			try
			{
				LogSystem.Debug("SetDataMedicine.1");
				if (Thuoc != null)
				{
					Thuoc.SetDataMedicine(data);
					LogSystem.Debug("SetDataMedicine.2");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
