using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACS.EFMODEL.DataModels;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Library.CacheClient;
using HIS.Desktop.LibraryMessage;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.Plugins.Library.RegisterConfig;
using HIS.Desktop.Utilities.Extensions;
using HIS.Desktop.Utility;
using HIS.UC.UCOtherServiceReqInfo.ADO;
using HIS.UC.UCOtherServiceReqInfo.Config;
using HIS.UC.UCOtherServiceReqInfo.FUN;
using HIS.UC.UCOtherServiceReqInfo.Resources;
using HIS.UC.UCOtherServiceReqInfo.Valid;
using Inventec.Common.Adapter;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.Controls.PopupLoader;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Common.TypeConvert;
using Inventec.Common.WebApiClient;
using Inventec.Core;
using Inventec.Desktop.Common.Controls.ValidationRule;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Controls;
using Inventec.Desktop.CustomControl.CustomGrid;
using Microsoft.CSharp.RuntimeBinder;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;

namespace HIS.UC.UCOtherServiceReqInfo
{
	public class UCOtherServiceReqInfo : UserControlBase
	{
		[CompilerGenerated]
		private static class _003C_003Eo__55
		{
			public static CallSite<Func<CallSite, BackendAdapter, string, ApiConsumer, object, CommonParam, object>> _003C_003Ep__0;

			public static CallSite<Func<CallSite, object, List<HIS_PRIORITY_TYPE>>> _003C_003Ep__1;
		}

		[CompilerGenerated]
		private static class _003C_003Eo__59
		{
			public static CallSite<Func<CallSite, BackendAdapter, string, ApiConsumer, object, CommonParam, object>> _003C_003Ep__0;

			public static CallSite<Func<CallSite, object, List<HIS_EMERGENCY_WTIME>>> _003C_003Ep__1;
		}

		[CompilerGenerated]
		private static class _003C_003Eo__60
		{
			public static CallSite<Func<CallSite, BackendAdapter, string, ApiConsumer, object, CommonParam, object>> _003C_003Ep__0;

			public static CallSite<Func<CallSite, object, List<HIS_TREATMENT_TYPE>>> _003C_003Ep__1;
		}

		[CompilerGenerated]
		private static class _003C_003Eo__61
		{
			public static CallSite<Func<CallSite, BackendAdapter, string, ApiConsumer, object, CommonParam, object>> _003C_003Ep__0;

			public static CallSite<Func<CallSite, object, List<HIS_OWE_TYPE>>> _003C_003Ep__1;
		}

		[CompilerGenerated]
		private static class _003C_003Eo__62
		{
			public static CallSite<Func<CallSite, BackendAdapter, string, ApiConsumer, object, CommonParam, object>> _003C_003Ep__0;

			public static CallSite<Func<CallSite, object, List<HIS_FUND>>> _003C_003Ep__1;
		}

		[CompilerGenerated]
		private static class _003C_003Eo__63
		{
			public static CallSite<Func<CallSite, BackendAdapter, string, ApiConsumer, object, CommonParam, object>> _003C_003Ep__0;

			public static CallSite<Func<CallSite, object, List<HIS_PATIENT_CLASSIFY>>> _003C_003Ep__1;
		}

		[CompilerGenerated]
		private static class _003C_003Eo__64
		{
			public static CallSite<Func<CallSite, BackendAdapter, string, ApiConsumer, object, CommonParam, object>> _003C_003Ep__0;

			public static CallSite<Func<CallSite, object, List<ACS_USER>>> _003C_003Ep__1;
		}

		[CompilerGenerated]
		private sealed class _003CLoadEmergencyWtimes_003Ed__59 : IAsyncStateMachine
		{
			private static class _003C_003Eo__59
			{
				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__1;

				public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__2;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__3;
			}

			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public UCOtherServiceReqInfo _003C_003E4__this;

			private List<HIS_EMERGENCY_WTIME> _003CdataEmergencyWtimes_003E5__1;

			private CommonParam _003CparamCommon_003E5__2;

			private object _003Cfilter_003E5__3;

			private Func<CallSite, object, List<HIS_EMERGENCY_WTIME>> _003C_003Es__4;

			private CallSite<Func<CallSite, object, List<HIS_EMERGENCY_WTIME>>> _003C_003Es__5;

			private object _003C_003Es__6;

			private Exception _003Cex_003E5__7;

			private object _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				try
				{
					if (num != 0)
					{
					}
					try
					{
						dynamic val;
						if (num != 0)
						{
							_003CdataEmergencyWtimes_003E5__1 = null;
							if (BackendDataWorker.IsExistsKey<HIS_EMERGENCY_WTIME>())
							{
								_003CdataEmergencyWtimes_003E5__1 = BackendDataWorker.Get<HIS_EMERGENCY_WTIME>();
								goto IL_035d;
							}
							_003CparamCommon_003E5__2 = new CommonParam();
							_003Cfilter_003E5__3 = new ExpandoObject();
							if (UCOtherServiceReqInfo._003C_003Eo__59._003C_003Ep__1 == null)
							{
								UCOtherServiceReqInfo._003C_003Eo__59._003C_003Ep__1 = CallSite<Func<CallSite, object, List<HIS_EMERGENCY_WTIME>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(List<HIS_EMERGENCY_WTIME>), typeof(UCOtherServiceReqInfo)));
							}
							_003C_003Es__4 = UCOtherServiceReqInfo._003C_003Eo__59._003C_003Ep__1.Target;
							_003C_003Es__5 = UCOtherServiceReqInfo._003C_003Eo__59._003C_003Ep__1;
							val = new BackendAdapter(_003CparamCommon_003E5__2).GetAsync<List<HIS_EMERGENCY_WTIME>>("api/HisEmergencyWtime/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (dynamic)_003Cfilter_003E5__3, _003CparamCommon_003E5__2).GetAwaiter();
							if (!(bool)val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
								_003CLoadEmergencyWtimes_003Ed__59 stateMachine = this;
								if (awaiter == null)
								{
									INotifyCompletion awaiter2 = (INotifyCompletion)(object)val;
									_003C_003Et__builder.AwaitOnCompleted(ref awaiter2, ref stateMachine);
									awaiter2 = null;
								}
								else
								{
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
								}
								awaiter = null;
								return;
							}
						}
						else
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = null;
							num = (_003C_003E1__state = -1);
						}
						_003C_003Es__6 = val.GetResult();
						_003CdataEmergencyWtimes_003E5__1 = _003C_003Es__4(_003C_003Es__5, _003C_003Es__6);
						_003C_003Es__4 = null;
						_003C_003Es__5 = null;
						_003C_003Es__6 = null;
						if (_003CdataEmergencyWtimes_003E5__1 != null)
						{
							BackendDataWorker.UpdateToRam(typeof(HIS_EMERGENCY_WTIME), _003CdataEmergencyWtimes_003E5__1, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
						}
						_003CparamCommon_003E5__2 = null;
						_003Cfilter_003E5__3 = null;
						goto IL_035d;
						IL_035d:
						if (_003CdataEmergencyWtimes_003E5__1 != null && _003CdataEmergencyWtimes_003E5__1.Count > 0)
						{
							_003CdataEmergencyWtimes_003E5__1 = _003CdataEmergencyWtimes_003E5__1.Where((HIS_EMERGENCY_WTIME p) => p.IS_ACTIVE == 1).ToList();
						}
						_003C_003E4__this.InitComboCommon(_003C_003E4__this.cboEmergencyTime, _003CdataEmergencyWtimes_003E5__1, "ID", "EMERGENCY_WTIME_NAME", "EMERGENCY_WTIME_CODE");
						_003CdataEmergencyWtimes_003E5__1 = null;
					}
					catch (Exception ex)
					{
						_003Cex_003E5__7 = ex;
						LogSystem.Error(_003Cex_003E5__7);
					}
				}
				catch (Exception ex)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(ex);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadFunds_003Ed__62 : IAsyncStateMachine
		{
			private static class _003C_003Eo__62
			{
				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__1;

				public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__2;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__3;
			}

			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public UCOtherServiceReqInfo _003C_003E4__this;

			private List<HIS_FUND> _003CdataFunds_003E5__1;

			private CommonParam _003CparamCommon_003E5__2;

			private object _003Cfilter_003E5__3;

			private Func<CallSite, object, List<HIS_FUND>> _003C_003Es__4;

			private CallSite<Func<CallSite, object, List<HIS_FUND>>> _003C_003Es__5;

			private object _003C_003Es__6;

			private Exception _003Cex_003E5__7;

			private object _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				try
				{
					if (num != 0)
					{
					}
					try
					{
						dynamic val;
						if (num != 0)
						{
							_003CdataFunds_003E5__1 = null;
							if (BackendDataWorker.IsExistsKey<HIS_FUND>())
							{
								_003CdataFunds_003E5__1 = BackendDataWorker.Get<HIS_FUND>();
								goto IL_035d;
							}
							_003CparamCommon_003E5__2 = new CommonParam();
							_003Cfilter_003E5__3 = new ExpandoObject();
							if (UCOtherServiceReqInfo._003C_003Eo__62._003C_003Ep__1 == null)
							{
								UCOtherServiceReqInfo._003C_003Eo__62._003C_003Ep__1 = CallSite<Func<CallSite, object, List<HIS_FUND>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(List<HIS_FUND>), typeof(UCOtherServiceReqInfo)));
							}
							_003C_003Es__4 = UCOtherServiceReqInfo._003C_003Eo__62._003C_003Ep__1.Target;
							_003C_003Es__5 = UCOtherServiceReqInfo._003C_003Eo__62._003C_003Ep__1;
							val = new BackendAdapter(_003CparamCommon_003E5__2).GetAsync<List<HIS_FUND>>("api/HisFund/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (dynamic)_003Cfilter_003E5__3, _003CparamCommon_003E5__2).GetAwaiter();
							if (!(bool)val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
								_003CLoadFunds_003Ed__62 stateMachine = this;
								if (awaiter == null)
								{
									INotifyCompletion awaiter2 = (INotifyCompletion)(object)val;
									_003C_003Et__builder.AwaitOnCompleted(ref awaiter2, ref stateMachine);
									awaiter2 = null;
								}
								else
								{
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
								}
								awaiter = null;
								return;
							}
						}
						else
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = null;
							num = (_003C_003E1__state = -1);
						}
						_003C_003Es__6 = val.GetResult();
						_003CdataFunds_003E5__1 = _003C_003Es__4(_003C_003Es__5, _003C_003Es__6);
						_003C_003Es__4 = null;
						_003C_003Es__5 = null;
						_003C_003Es__6 = null;
						if (_003CdataFunds_003E5__1 != null)
						{
							BackendDataWorker.UpdateToRam(typeof(HIS_FUND), _003CdataFunds_003E5__1, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
						}
						_003CparamCommon_003E5__2 = null;
						_003Cfilter_003E5__3 = null;
						goto IL_035d;
						IL_035d:
						if (_003CdataFunds_003E5__1 != null && _003CdataFunds_003E5__1.Count > 0)
						{
							_003CdataFunds_003E5__1 = _003CdataFunds_003E5__1.Where((HIS_FUND o) => o.IS_ACTIVE == 1).ToList();
						}
						_003C_003E4__this.InitComboCommon(_003C_003E4__this.cboCTT, _003CdataFunds_003E5__1, "ID", "FUND_NAME", "FUND_CODE");
						_003CdataFunds_003E5__1 = null;
					}
					catch (Exception ex)
					{
						_003Cex_003E5__7 = ex;
						LogSystem.Error(_003Cex_003E5__7);
					}
				}
				catch (Exception ex)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(ex);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadGuarantee_003Ed__64 : IAsyncStateMachine
		{
			private static class _003C_003Eo__64
			{
				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__1;

				public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__2;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__3;
			}

			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public UCOtherServiceReqInfo _003C_003E4__this;

			private List<ACS_USER> _003CdataUser_003E5__1;

			private CommonParam _003CparamCommon_003E5__2;

			private object _003Cfilter_003E5__3;

			private Func<CallSite, object, List<ACS_USER>> _003C_003Es__4;

			private CallSite<Func<CallSite, object, List<ACS_USER>>> _003C_003Es__5;

			private object _003C_003Es__6;

			private Exception _003Cex_003E5__7;

			private object _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				try
				{
					if (num != 0)
					{
					}
					try
					{
						dynamic val;
						if (num != 0)
						{
							_003CdataUser_003E5__1 = null;
							if (BackendDataWorker.IsExistsKey<ACS_USER>())
							{
								_003CdataUser_003E5__1 = BackendDataWorker.Get<ACS_USER>();
								goto IL_035d;
							}
							_003CparamCommon_003E5__2 = new CommonParam();
							_003Cfilter_003E5__3 = new ExpandoObject();
							if (UCOtherServiceReqInfo._003C_003Eo__64._003C_003Ep__1 == null)
							{
								UCOtherServiceReqInfo._003C_003Eo__64._003C_003Ep__1 = CallSite<Func<CallSite, object, List<ACS_USER>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(List<ACS_USER>), typeof(UCOtherServiceReqInfo)));
							}
							_003C_003Es__4 = UCOtherServiceReqInfo._003C_003Eo__64._003C_003Ep__1.Target;
							_003C_003Es__5 = UCOtherServiceReqInfo._003C_003Eo__64._003C_003Ep__1;
							val = new BackendAdapter(_003CparamCommon_003E5__2).GetAsync<List<ACS_USER>>("api/AcsUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.AcsConsumer, (dynamic)_003Cfilter_003E5__3, _003CparamCommon_003E5__2).GetAwaiter();
							if (!(bool)val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
								_003CLoadGuarantee_003Ed__64 stateMachine = this;
								if (awaiter == null)
								{
									INotifyCompletion awaiter2 = (INotifyCompletion)(object)val;
									_003C_003Et__builder.AwaitOnCompleted(ref awaiter2, ref stateMachine);
									awaiter2 = null;
								}
								else
								{
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
								}
								awaiter = null;
								return;
							}
						}
						else
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = null;
							num = (_003C_003E1__state = -1);
						}
						_003C_003Es__6 = val.GetResult();
						_003CdataUser_003E5__1 = _003C_003Es__4(_003C_003Es__5, _003C_003Es__6);
						_003C_003Es__4 = null;
						_003C_003Es__5 = null;
						_003C_003Es__6 = null;
						if (_003CdataUser_003E5__1 != null)
						{
							BackendDataWorker.UpdateToRam(typeof(ACS_USER), _003CdataUser_003E5__1, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
						}
						_003CparamCommon_003E5__2 = null;
						_003Cfilter_003E5__3 = null;
						goto IL_035d;
						IL_035d:
						if (_003CdataUser_003E5__1 != null && _003CdataUser_003E5__1.Count > 0)
						{
							_003CdataUser_003E5__1 = _003CdataUser_003E5__1.Where((ACS_USER o) => o.IS_ACTIVE == 1).ToList();
						}
						_003C_003E4__this.InitComboCommon(_003C_003E4__this.cboGuaranteeUsername, _003CdataUser_003E5__1, "LOGINNAME", "USERNAME", "LOGINNAME");
						_003C_003E4__this.cboGuaranteeUsername.Properties.ImmediatePopup = true;
						_003CdataUser_003E5__1 = null;
					}
					catch (Exception ex)
					{
						_003Cex_003E5__7 = ex;
						LogSystem.Error(_003Cex_003E5__7);
					}
				}
				catch (Exception ex)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(ex);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadOweTypes_003Ed__61 : IAsyncStateMachine
		{
			private static class _003C_003Eo__61
			{
				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__1;

				public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__2;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__3;
			}

			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public UCOtherServiceReqInfo _003C_003E4__this;

			private List<HIS_OWE_TYPE> _003CdataOweTypes_003E5__1;

			private CommonParam _003CparamCommon_003E5__2;

			private object _003Cfilter_003E5__3;

			private Func<CallSite, object, List<HIS_OWE_TYPE>> _003C_003Es__4;

			private CallSite<Func<CallSite, object, List<HIS_OWE_TYPE>>> _003C_003Es__5;

			private object _003C_003Es__6;

			private HIS_OWE_TYPE _003CoweType_003E5__7;

			private Exception _003Cex_003E5__8;

			private object _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				try
				{
					if (num != 0)
					{
					}
					try
					{
						dynamic val;
						if (num != 0)
						{
							_003CdataOweTypes_003E5__1 = null;
							if (BackendDataWorker.IsExistsKey<HIS_OWE_TYPE>())
							{
								_003CdataOweTypes_003E5__1 = BackendDataWorker.Get<HIS_OWE_TYPE>();
								goto IL_035d;
							}
							_003CparamCommon_003E5__2 = new CommonParam();
							_003Cfilter_003E5__3 = new ExpandoObject();
							if (UCOtherServiceReqInfo._003C_003Eo__61._003C_003Ep__1 == null)
							{
								UCOtherServiceReqInfo._003C_003Eo__61._003C_003Ep__1 = CallSite<Func<CallSite, object, List<HIS_OWE_TYPE>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(List<HIS_OWE_TYPE>), typeof(UCOtherServiceReqInfo)));
							}
							_003C_003Es__4 = UCOtherServiceReqInfo._003C_003Eo__61._003C_003Ep__1.Target;
							_003C_003Es__5 = UCOtherServiceReqInfo._003C_003Eo__61._003C_003Ep__1;
							val = new BackendAdapter(_003CparamCommon_003E5__2).GetAsync<List<HIS_OWE_TYPE>>("api/HisOweType/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (dynamic)_003Cfilter_003E5__3, _003CparamCommon_003E5__2).GetAwaiter();
							if (!(bool)val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
								_003CLoadOweTypes_003Ed__61 stateMachine = this;
								if (awaiter == null)
								{
									INotifyCompletion awaiter2 = (INotifyCompletion)(object)val;
									_003C_003Et__builder.AwaitOnCompleted(ref awaiter2, ref stateMachine);
									awaiter2 = null;
								}
								else
								{
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
								}
								awaiter = null;
								return;
							}
						}
						else
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = null;
							num = (_003C_003E1__state = -1);
						}
						_003C_003Es__6 = val.GetResult();
						_003CdataOweTypes_003E5__1 = _003C_003Es__4(_003C_003Es__5, _003C_003Es__6);
						_003C_003Es__4 = null;
						_003C_003Es__5 = null;
						_003C_003Es__6 = null;
						if (_003CdataOweTypes_003E5__1 != null)
						{
							BackendDataWorker.UpdateToRam(typeof(HIS_OWE_TYPE), _003CdataOweTypes_003E5__1, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
						}
						_003CparamCommon_003E5__2 = null;
						_003Cfilter_003E5__3 = null;
						goto IL_035d;
						IL_035d:
						if (_003CdataOweTypes_003E5__1 != null && _003CdataOweTypes_003E5__1.Count > 0)
						{
							_003CdataOweTypes_003E5__1 = _003CdataOweTypes_003E5__1.Where((HIS_OWE_TYPE p) => p.IS_ACTIVE == 1).ToList();
						}
						_003C_003E4__this.InitComboCommon(_003C_003E4__this.cboOweType, _003CdataOweTypes_003E5__1, "ID", "OWE_TYPE_NAME", "OWE_TYPE_CODE");
						if (!string.IsNullOrEmpty(AppConfigs.OweTypeDefault) && _003CdataOweTypes_003E5__1 != null && _003CdataOweTypes_003E5__1.Count > 0)
						{
							_003CoweType_003E5__7 = _003CdataOweTypes_003E5__1.FirstOrDefault((HIS_OWE_TYPE o) => o.OWE_TYPE_CODE == AppConfigs.OweTypeDefault);
							if (_003CoweType_003E5__7 == null)
							{
								throw new ArgumentNullException("Khong tim thay HIS_OWE_TYPE theo OWE_TYPE_CODE = " + AppConfigs.OweTypeDefault);
							}
							_003C_003E4__this.cboOweType.EditValue = _003CoweType_003E5__7.ID;
							_003CoweType_003E5__7 = null;
						}
						_003CdataOweTypes_003E5__1 = null;
					}
					catch (Exception ex)
					{
						_003Cex_003E5__8 = ex;
						LogSystem.Error(_003Cex_003E5__8);
					}
				}
				catch (Exception ex)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(ex);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadPatientClassify_003Ed__63 : IAsyncStateMachine
		{
			private static class _003C_003Eo__63
			{
				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__1;

				public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__2;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__3;
			}

			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public UCOtherServiceReqInfo _003C_003E4__this;

			private CommonParam _003CparamCommon_003E5__1;

			private object _003Cfilter_003E5__2;

			private Func<CallSite, object, List<HIS_PATIENT_CLASSIFY>> _003C_003Es__3;

			private CallSite<Func<CallSite, object, List<HIS_PATIENT_CLASSIFY>>> _003C_003Es__4;

			private object _003C_003Es__5;

			private Exception _003Cex_003E5__6;

			private object _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				try
				{
					if (num != 0)
					{
					}
					try
					{
						dynamic val;
						if (num != 0)
						{
							if (BackendDataWorker.IsExistsKey<HIS_PATIENT_CLASSIFY>())
							{
								_003C_003E4__this.dataClassify = BackendDataWorker.Get<HIS_PATIENT_CLASSIFY>();
								goto IL_036a;
							}
							_003CparamCommon_003E5__1 = new CommonParam();
							_003Cfilter_003E5__2 = new ExpandoObject();
							if (UCOtherServiceReqInfo._003C_003Eo__63._003C_003Ep__1 == null)
							{
								UCOtherServiceReqInfo._003C_003Eo__63._003C_003Ep__1 = CallSite<Func<CallSite, object, List<HIS_PATIENT_CLASSIFY>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(List<HIS_PATIENT_CLASSIFY>), typeof(UCOtherServiceReqInfo)));
							}
							_003C_003Es__3 = UCOtherServiceReqInfo._003C_003Eo__63._003C_003Ep__1.Target;
							_003C_003Es__4 = UCOtherServiceReqInfo._003C_003Eo__63._003C_003Ep__1;
							val = new BackendAdapter(_003CparamCommon_003E5__1).GetAsync<List<HIS_PATIENT_CLASSIFY>>("api/HisPatientClassify/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (dynamic)_003Cfilter_003E5__2, _003CparamCommon_003E5__1).GetAwaiter();
							if (!(bool)val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
								_003CLoadPatientClassify_003Ed__63 stateMachine = this;
								if (awaiter == null)
								{
									INotifyCompletion awaiter2 = (INotifyCompletion)(object)val;
									_003C_003Et__builder.AwaitOnCompleted(ref awaiter2, ref stateMachine);
									awaiter2 = null;
								}
								else
								{
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
								}
								awaiter = null;
								return;
							}
						}
						else
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = null;
							num = (_003C_003E1__state = -1);
						}
						_003C_003Es__5 = val.GetResult();
						_003C_003E4__this.dataClassify = _003C_003Es__3(_003C_003Es__4, _003C_003Es__5);
						_003C_003Es__3 = null;
						_003C_003Es__4 = null;
						_003C_003Es__5 = null;
						if (_003C_003E4__this.dataClassify != null)
						{
							BackendDataWorker.UpdateToRam(typeof(HIS_PATIENT_CLASSIFY), _003C_003E4__this.dataClassify, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
						}
						_003CparamCommon_003E5__1 = null;
						_003Cfilter_003E5__2 = null;
						goto IL_036a;
						IL_036a:
						if (_003C_003E4__this.dataClassify != null && _003C_003E4__this.dataClassify.Count > 0)
						{
							_003C_003E4__this.dataClassify = _003C_003E4__this.dataClassify.Where((HIS_PATIENT_CLASSIFY o) => o.IS_ACTIVE == 1).ToList();
						}
						_003C_003E4__this.InitComboCommon(_003C_003E4__this.cboPatientClassify, _003C_003E4__this.dataClassify, "ID", "PATIENT_CLASSIFY_NAME", "PATIENT_CLASSIFY_CODE");
					}
					catch (Exception ex)
					{
						_003Cex_003E5__6 = ex;
						LogSystem.Error(_003Cex_003E5__6);
					}
				}
				catch (Exception ex)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(ex);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadPriorityType_003Ed__55 : IAsyncStateMachine
		{
			private static class _003C_003Eo__55
			{
				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__1;

				public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__2;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__3;
			}

			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public UCOtherServiceReqInfo _003C_003E4__this;

			private List<HIS_PRIORITY_TYPE> _003CdataPriorityTypes_003E5__1;

			private CommonParam _003CparamCommon_003E5__2;

			private object _003Cfilter_003E5__3;

			private Func<CallSite, object, List<HIS_PRIORITY_TYPE>> _003C_003Es__4;

			private CallSite<Func<CallSite, object, List<HIS_PRIORITY_TYPE>>> _003C_003Es__5;

			private object _003C_003Es__6;

			private Exception _003Cex_003E5__7;

			private object _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				try
				{
					if (num != 0)
					{
					}
					try
					{
						dynamic val;
						if (num != 0)
						{
							_003CdataPriorityTypes_003E5__1 = null;
							if (BackendDataWorker.IsExistsKey<HIS_PRIORITY_TYPE>())
							{
								_003CdataPriorityTypes_003E5__1 = BackendDataWorker.Get<HIS_PRIORITY_TYPE>();
								goto IL_035d;
							}
							_003CparamCommon_003E5__2 = new CommonParam();
							_003Cfilter_003E5__3 = new ExpandoObject();
							if (UCOtherServiceReqInfo._003C_003Eo__55._003C_003Ep__1 == null)
							{
								UCOtherServiceReqInfo._003C_003Eo__55._003C_003Ep__1 = CallSite<Func<CallSite, object, List<HIS_PRIORITY_TYPE>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(List<HIS_PRIORITY_TYPE>), typeof(UCOtherServiceReqInfo)));
							}
							_003C_003Es__4 = UCOtherServiceReqInfo._003C_003Eo__55._003C_003Ep__1.Target;
							_003C_003Es__5 = UCOtherServiceReqInfo._003C_003Eo__55._003C_003Ep__1;
							val = new BackendAdapter(_003CparamCommon_003E5__2).GetAsync<List<HIS_PRIORITY_TYPE>>("api/HisPriorityType/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (dynamic)_003Cfilter_003E5__3, _003CparamCommon_003E5__2).GetAwaiter();
							if (!(bool)val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
								_003CLoadPriorityType_003Ed__55 stateMachine = this;
								if (awaiter == null)
								{
									INotifyCompletion awaiter2 = (INotifyCompletion)(object)val;
									_003C_003Et__builder.AwaitOnCompleted(ref awaiter2, ref stateMachine);
									awaiter2 = null;
								}
								else
								{
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
								}
								awaiter = null;
								return;
							}
						}
						else
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = null;
							num = (_003C_003E1__state = -1);
						}
						_003C_003Es__6 = val.GetResult();
						_003CdataPriorityTypes_003E5__1 = _003C_003Es__4(_003C_003Es__5, _003C_003Es__6);
						_003C_003Es__4 = null;
						_003C_003Es__5 = null;
						_003C_003Es__6 = null;
						if (_003CdataPriorityTypes_003E5__1 != null)
						{
							BackendDataWorker.UpdateToRam(typeof(HIS_PRIORITY_TYPE), _003CdataPriorityTypes_003E5__1, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
						}
						_003CparamCommon_003E5__2 = null;
						_003Cfilter_003E5__3 = null;
						goto IL_035d;
						IL_035d:
						_003C_003E4__this.InitComboCommon(_003C_003E4__this.cboPriorityType, _003CdataPriorityTypes_003E5__1, "ID", "PRIORITY_TYPE_NAME", "PRIORITY_TYPE_CODE");
						_003CdataPriorityTypes_003E5__1 = null;
					}
					catch (Exception ex)
					{
						_003Cex_003E5__7 = ex;
						LogSystem.Error(_003Cex_003E5__7);
					}
				}
				catch (Exception ex)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(ex);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadTreatmentTypes_003Ed__60 : IAsyncStateMachine
		{
			private static class _003C_003Eo__60
			{
				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__1;

				public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__2;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__3;
			}

			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public UCOtherServiceReqInfo _003C_003E4__this;

			private List<HIS_TREATMENT_TYPE> _003CdataTreatmentTypes_003E5__1;

			private CommonParam _003CparamCommon_003E5__2;

			private object _003Cfilter_003E5__3;

			private Func<CallSite, object, List<HIS_TREATMENT_TYPE>> _003C_003Es__4;

			private CallSite<Func<CallSite, object, List<HIS_TREATMENT_TYPE>>> _003C_003Es__5;

			private object _003C_003Es__6;

			private Exception _003Cex_003E5__7;

			private object _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				try
				{
					if (num != 0)
					{
					}
					try
					{
						dynamic val;
						if (num != 0)
						{
							_003CdataTreatmentTypes_003E5__1 = null;
							if (BackendDataWorker.IsExistsKey<HIS_TREATMENT_TYPE>())
							{
								_003CdataTreatmentTypes_003E5__1 = BackendDataWorker.Get<HIS_TREATMENT_TYPE>();
								goto IL_035d;
							}
							_003CparamCommon_003E5__2 = new CommonParam();
							_003Cfilter_003E5__3 = new ExpandoObject();
							if (UCOtherServiceReqInfo._003C_003Eo__60._003C_003Ep__1 == null)
							{
								UCOtherServiceReqInfo._003C_003Eo__60._003C_003Ep__1 = CallSite<Func<CallSite, object, List<HIS_TREATMENT_TYPE>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(List<HIS_TREATMENT_TYPE>), typeof(UCOtherServiceReqInfo)));
							}
							_003C_003Es__4 = UCOtherServiceReqInfo._003C_003Eo__60._003C_003Ep__1.Target;
							_003C_003Es__5 = UCOtherServiceReqInfo._003C_003Eo__60._003C_003Ep__1;
							val = new BackendAdapter(_003CparamCommon_003E5__2).GetAsync<List<HIS_TREATMENT_TYPE>>("api/HisTreatmentType/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (dynamic)_003Cfilter_003E5__3, _003CparamCommon_003E5__2).GetAwaiter();
							if (!(bool)val.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = val;
								ICriticalNotifyCompletion awaiter = val as ICriticalNotifyCompletion;
								_003CLoadTreatmentTypes_003Ed__60 stateMachine = this;
								if (awaiter == null)
								{
									INotifyCompletion awaiter2 = (INotifyCompletion)(object)val;
									_003C_003Et__builder.AwaitOnCompleted(ref awaiter2, ref stateMachine);
									awaiter2 = null;
								}
								else
								{
									_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
								}
								awaiter = null;
								return;
							}
						}
						else
						{
							val = _003C_003Eu__1;
							_003C_003Eu__1 = null;
							num = (_003C_003E1__state = -1);
						}
						_003C_003Es__6 = val.GetResult();
						_003CdataTreatmentTypes_003E5__1 = _003C_003Es__4(_003C_003Es__5, _003C_003Es__6);
						_003C_003Es__4 = null;
						_003C_003Es__5 = null;
						_003C_003Es__6 = null;
						if (_003CdataTreatmentTypes_003E5__1 != null)
						{
							BackendDataWorker.UpdateToRam(typeof(HIS_TREATMENT_TYPE), _003CdataTreatmentTypes_003E5__1, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
						}
						_003CparamCommon_003E5__2 = null;
						_003Cfilter_003E5__3 = null;
						goto IL_035d;
						IL_035d:
						if (_003CdataTreatmentTypes_003E5__1 != null && _003CdataTreatmentTypes_003E5__1.Count > 0)
						{
							_003CdataTreatmentTypes_003E5__1 = _003CdataTreatmentTypes_003E5__1.Where((HIS_TREATMENT_TYPE p) => p.IS_ACTIVE == 1 && p.IS_ALLOW_RECEPTION == 1).ToList();
						}
						_003C_003E4__this.InitComboCommon(_003C_003E4__this.cboTreatmentType, _003CdataTreatmentTypes_003E5__1, "ID", "TREATMENT_TYPE_NAME", 70, "TREATMENT_TYPE_CODE", 30);
						_003C_003E4__this.cboTreatmentType.EditValue = (long)((_003CdataTreatmentTypes_003E5__1 != null && _003CdataTreatmentTypes_003E5__1.Count > 0 && _003CdataTreatmentTypes_003E5__1.FirstOrDefault((HIS_TREATMENT_TYPE p) => p.ID == 1) != null) ? 1 : 0);
						_003CdataTreatmentTypes_003E5__1 = null;
					}
					catch (Exception ex)
					{
						_003Cex_003E5__7 = ex;
						LogSystem.Error(_003Cex_003E5__7);
					}
				}
				catch (Exception ex)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(ex);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		private Action<object> dlgFocusNextUserControl;

		private Action<bool> dlgHeinRightRouteType;

		private Action<long?> dlgPriorityNumberChanged;

		private Action<string> dlgGetTreatmentTypeId;

		private Action<string> dlgGetTreatmentTypeIdForUcHeinInfo;

		internal HIS_TREATMENT _HisTreatment = null;

		private HIS_PATIENT_TYPE workingPatientType;

		private bool _IsUserBranchTime = false;

		private List<HIS_BRANCH_TIME> _BranchTimes = null;

		private bool _IsAutoSetOweType = false;

		private bool hasDataAutoCheckPriority = false;

		private bool IsChangeFromClassify = false;

		private List<HIS_OTHER_PAY_SOURCE> dataOtherPayTemp = null;

		private List<HIS_PATIENT_CLASSIFY> dataClassify = null;

		private ControlStateWorker controlStateWorker;

		private List<ControlStateRDO> currentControlStateRDO;

		private string moduleLink = "HIS.UC.UCOtherServiceReqInfo";

		private string treatmentTypeId;

		private List<otherPaySourceDetailADO> listConfigDefault = new List<otherPaySourceDetailADO>();

		private IContainer components = null;

		private LayoutControl lcUCOtherServiceReqInfo;

		private LayoutControlGroup lcgOtherRequest;

		internal CheckEdit chkIsChronic;

		internal LookUpEdit cboOweType;

		internal LookUpEdit cboTreatmentType;

		internal CheckEdit chkIsNotRequireFee;

		internal CheckEdit chkPriority;

		internal CheckEdit chkEmergency;

		internal LookUpEdit cboEmergencyTime;

		private LayoutControlItem lciIsChronic;

		private LayoutControlItem lciOweType;

		private LayoutControlItem lciTreatmentType;

		private LayoutControlItem lciIsNotRequireFee;

		private LayoutControlItem lciPriority;

		private LayoutControlItem lciEmergency;

		private LayoutControlItem lciEmergencyTime;

		private Panel panel1;

		private LayoutControlItem lciIntructionTime;

		private ButtonEdit txtIntructionTime;

		private DateEdit dtIntructionTime;

		private DXValidationProvider dxValidationUCOtherReqInfo;

		private DXErrorProvider dxErrorProviderControl;

		private SimpleButton btnAddCTT;

		private GridLookUpEdit cboCTT;

		private GridView gridLookUpEdit1View;

		private LayoutControlItem lciCboCTT;

		private LayoutControlItem layoutControlItem2;

		private SpinEdit txtSTTPriority;

		private LayoutControlItem lciFortxtSTTPriority;

		private System.Windows.Forms.Timer timerInitForm;

		private TextEdit txtTreatmentOrder;

		private LayoutControlItem lciTreatmentOrder;

		internal LookUpEdit cboPriorityType;

		private LayoutControlItem layoutControlItem1;

		private TextEdit txtMaMS;

		private CheckEdit chkCapMaMS;

		private LayoutControlItem lciForchkCapMaMS;

		private LayoutControlItem lciFortxtMaMS;

		private GridLookUpEdit cboOtherPaySource;

		private GridView gridView1;

		private LayoutControlItem layoutControlItem3;

		private TextEdit txtIncode;

		private LayoutControlItem lciFortxtIncode;

		private GridLookUpEdit cboPatientClassify;

		private GridView gridView2;

		private LayoutControlItem lciPatientClassify;

		private TextEdit txtGuaranteeLoginname;

		private LayoutControlItem lciGuaranteeLoginname;

		private GridLookUpEdit cboGuaranteeUsername;

		private GridView gridView3;

		private LayoutControlItem lciGuaranteeUsername;

		private TextEdit txtGuaranteeReason;

		private LayoutControlItem lciGuaranteeReason;

		private CheckEdit chkTuberculosis;

		private LayoutControlItem lciTuberculosis;

		private ToolTipItem toolTipItem1;

		private ToolTipItem toolTipItem2;

		private MemoEdit txtNote;

		private LayoutControlItem layoutControlItem4;

		private CheckEdit chkWNext;

		private LayoutControlItem layoutControlItem5;

		private CheckEdit chkIsHiv;

		private LayoutControlItem layoutControlItem6;

		private CheckEdit chkExamOnline;

		private LayoutControlItem layoutControlItem7;

		private TextEdit txtHosReason;

		private LayoutControlItem layoutControlItem8;

		private Panel panel2;

		private LayoutControlItem lciHosReason;

		private Inventec.Desktop.CustomControl.CustomGrid.CustomGridLookUpEdit cboHosReason;

		private Inventec.Desktop.CustomControl.CustomGrid.CustomGridView customGridLookUpEdit1View;

		private ButtonEdit txtHosReasonNt;

		private CheckEdit chkCAPD;

		private EmptySpaceItem emptySpaceItem1;

		private LayoutControlItem layoutControlItem9;

		private GridView gridView4;

		private LayoutControlItem layoutControlItem10;

		private LayoutControlItem layoutControlItem11;

		internal GridLookUpEdit cboNguonKhach;

		internal TextEdit txtNguonKhach;

		private GridLookUpEdit cboNguonKhachCT;

		private GridView gridView5;

		private LayoutControlItem layoutControlItem13;

		private CheckEdit chkChamSocDa;

		private LayoutControlItem layoutControlItem12;

		private EmptySpaceItem emptySpaceItem2;

		private string _PatientName = "";

		public short? IS_CAPD { get; set; }

		public bool IsCAPD { get; set; }

		public string HospitalizeReasonCode { get; private set; }

		public string HospitalizeReasonName { get; private set; }

		private List<HIS_CUSTOMER_SOURCE_DT> lstOtherDetail { get; set; }

		private List<HIS_CUSTOMER_SOURCE_DT> lstOtherDetailDefault { get; set; }

		private HisPatientSDO patientSdo { get; set; }

		private HIS_TREATMENT TreatmentByPatientSdo { get; set; }

		public UCOtherServiceReqInfo()
			: base("HIS.Desktop.Plugins.RegisterV2", "UCOtherServiceReqInfo")
		{
			LogSystem.Debug("UCOtherServiceReqInfo .1");
			InitializeComponent();
			try
			{
				HisConfig.LoadConfig();
				LogSystem.Debug("UCOtherServiceReqInfo .2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void UCOtherServiceReqInfo_Load(object sender, EventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			try
			{
				LogSystem.Debug("UCOtherServiceReqInfo_Load .1");
				_HisTreatment = new HIS_TREATMENT();
				txtIntructionTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
				dtIntructionTime.EditValue = DateTime.Now;
				txtSTTPriority.EditValue = null;
				BackendDataWorker.Reset<HIS_OTHER_PAY_SOURCE>();
				_IsAutoSetOweType = HisConfigs.Get<string>("HIS.Desktop.Plugins.Register.IsAutoSetOweTypeInCaseOfUsingFund").Trim() == "1";
				InitControlState();
				LoadBranch();
				SetCaptionByLanguageKeyNew();
				ValidateIntructionTime();
				ValidateFrmFun();
				ValidateTreatmentType();
				ValidateNumOrderPriority();
				ValidateMaxlength(txtGuaranteeReason, 500);
				ValidateMaxlength(txtNote, 1000);
				if (HisConfig.RequestSkinCare != "1" && HisConfig.RequestSkinCare != "2")
				{
					layoutControlItem12.Visibility = LayoutVisibility.Never;
				}
				else
				{
					layoutControlItem12.Visibility = LayoutVisibility.Always;
					if (HisConfig.RequestSkinCare == "2")
					{
						chkChamSocDa.Checked = true;
					}
				}
				LogSystem.Debug("UCOtherServiceReqInfo_Load .2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public async Task InitFieldFromAsync()
		{
			try
			{
				LogSystem.Debug("UCOtherServiceReqInfo.InitFieldFromAsync .1");
				await LoadEmergencyWtimes();
				await LoadPriorityType();
				await LoadTreatmentTypes();
				await LoadOweTypes();
				await LoadFunds();
				await LoadPatientClassify();
				await LoadGuarantee();
				LoadNguonKhach();
				LoadOtherPaySource();
				InitComboHisHospitalizeReason();
				SetHeinRighRouteTypeByTime();
				LoadNguonKhachCT();
				LogSystem.Debug("UCOtherServiceReqInfo.InitFieldFromAsync .2");
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private string convertToUnSign3(string s)
		{
			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string input = s.Normalize(NormalizationForm.FormD);
			return regex.Replace(input, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
		}

		private void LoadNguonKhachCT()
		{
			try
			{
				List<otherPaySourceDetailADO> list = new List<otherPaySourceDetailADO>();
				lstOtherDetail = (from o in BackendDataWorker.Get<HIS_CUSTOMER_SOURCE_DT>()
					where o.IS_ACTIVE == 1
					select o).ToList();
				foreach (HIS_CUSTOMER_SOURCE_DT item in lstOtherDetail)
				{
					otherPaySourceDetailADO otherPaySourceDetailADO = new otherPaySourceDetailADO();
					((HIS_CUSTOMER_SOURCE_DT)otherPaySourceDetailADO).ID = item.ID;
					((HIS_CUSTOMER_SOURCE_DT)otherPaySourceDetailADO).LOGINNAME = item.LOGINNAME;
					((HIS_CUSTOMER_SOURCE_DT)otherPaySourceDetailADO).USERNAME = item.USERNAME;
					otherPaySourceDetailADO.USERNAME_UNSIGN = convertToUnSign3(item.USERNAME);
					list.Add(otherPaySourceDetailADO);
				}
				InitComboOtherDetail(list);
				InitComboOtherDetailCheck();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void InitComboOtherDetailCheck()
		{
			try
			{
				HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection = new HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection(cboNguonKhachCT.Properties);
				gridCheckMarksSelection.SelectionChanged += Event_Check_OtherDetail;
				cboNguonKhachCT.Properties.Tag = gridCheckMarksSelection;
				cboNguonKhachCT.Properties.View.OptionsSelection.MultiSelect = true;
				HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection2 = cboNguonKhachCT.Properties.Tag as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection;
				if (gridCheckMarksSelection2 != null)
				{
					gridCheckMarksSelection2.ClearSelection(cboNguonKhachCT.Properties.View);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void Event_Check_OtherDetail(object sender, EventArgs e)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection = sender as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection;
				lstOtherDetail = new List<HIS_CUSTOMER_SOURCE_DT>();
				if (gridCheckMarksSelection != null)
				{
					List<HIS_CUSTOMER_SOURCE_DT> list = new List<HIS_CUSTOMER_SOURCE_DT>();
					foreach (HIS_CUSTOMER_SOURCE_DT item in (sender as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection).Selection)
					{
						HIS_CUSTOMER_SOURCE_DT val = item;
						if (val != null)
						{
							if (stringBuilder.ToString().Length > 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append(val.USERNAME);
							list.Add(val);
						}
					}
					lstOtherDetail = new List<HIS_CUSTOMER_SOURCE_DT>();
					lstOtherDetail.AddRange(list);
				}
				cboNguonKhachCT.Text = stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboOtherDetail(List<otherPaySourceDetailADO> listADO)
		{
			cboNguonKhachCT.Properties.DataSource = listADO;
			cboNguonKhachCT.Properties.DisplayMember = "USERNAME";
			cboNguonKhachCT.Properties.ValueMember = "LOGINNAME";
			cboNguonKhachCT.Properties.NullText = "";
			cboNguonKhachCT.Properties.AllowNullInput = DefaultBoolean.True;
			cboNguonKhachCT.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
			cboNguonKhachCT.Properties.View.OptionsView.ShowDetailButtons = false;
			cboNguonKhachCT.Properties.View.OptionsView.ShowGroupPanel = false;
			cboNguonKhachCT.Properties.View.OptionsView.ShowIndicator = false;
			if (cboNguonKhachCT.Properties.View.Columns.Count == 0)
			{
				GridColumn gridColumn = cboNguonKhachCT.Properties.View.Columns.AddField("LOGINNAME");
				gridColumn.Caption = "Mã";
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 60;
				gridColumn.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
				gridColumn.OptionsFilter.FilterPopupMode = FilterPopupMode.Default;
				GridColumn gridColumn2 = cboNguonKhachCT.Properties.View.Columns.AddField("USERNAME");
				gridColumn2.Caption = "Tên";
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 2;
				gridColumn2.Width = 200;
				GridColumn gridColumn3 = cboNguonKhachCT.Properties.View.Columns.AddField("USERNAME_UNSIGN");
				gridColumn3.Visible = true;
				gridColumn3.VisibleIndex = -1;
				gridColumn3.Width = 340;
				cboNguonKhachCT.Properties.View.Columns["USERNAME_UNSIGN"].Width = 0;
				cboNguonKhachCT.Properties.View.OptionsView.ShowColumnHeaders = true;
				cboNguonKhachCT.Properties.View.OptionsSelection.MultiSelect = true;
			}
		}

		private void UpdateComboOtherDetailDataSource(List<otherPaySourceDetailADO> listADO)
		{
			try
			{
				cboNguonKhachCT.Properties.DataSource = listADO;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ProcessSelectOtherPaySourceDetail(string p, HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMark)
		{
			try
			{
				List<otherPaySourceDetailADO> list = cboNguonKhachCT.Properties.DataSource as List<otherPaySourceDetailADO>;
				if (list == null || list.Count == 0)
				{
					cboNguonKhachCT.Properties.DataSource = list;
				}
				string[] array = p.Split(',');
				if (array == null || array.Length == 0)
				{
					return;
				}
				List<otherPaySourceDetailADO> list2 = new List<otherPaySourceDetailADO>();
				string[] array2 = array;
				foreach (string text in array2)
				{
					string nameTrim = text.Trim();
					otherPaySourceDetailADO otherPaySourceDetailADO = list.FirstOrDefault((otherPaySourceDetailADO o) => ((((HIS_CUSTOMER_SOURCE_DT)o).LOGINNAME != null) ? ((HIS_CUSTOMER_SOURCE_DT)o).LOGINNAME.Trim() : "") == nameTrim);
					if (otherPaySourceDetailADO != null)
					{
						list2.Add(otherPaySourceDetailADO);
						listConfigDefault.Add(otherPaySourceDetailADO);
					}
				}
				gridCheckMark.SelectAll(list2);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetCaptionByLanguageKeyNew()
		{
			try
			{
				ResourceLanguageManager.LanguageResource = new ResourceManager("HIS.UC.UCOtherServiceReqInfo.Resources.Lang", typeof(UCOtherServiceReqInfo).Assembly);
				lcUCOtherServiceReqInfo.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lcUCOtherServiceReqInfo.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				chkTuberculosis.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkTuberculosis.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboGuaranteeUsername.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboGuaranteeUsername.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboPatientClassify.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboPatientClassify.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboOtherPaySource.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboOtherPaySource.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				chkCapMaMS.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkCapMaMS.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboPriorityType.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboPriorityType.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboPriorityType.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboPriorityType.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnAddCTT.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.btnAddCTT.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboCTT.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboCTT.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				chkIsChronic.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkIsChronic.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboOweType.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboOweType.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboTreatmentType.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboTreatmentType.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				chkIsNotRequireFee.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkIsNotRequireFee.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				chkPriority.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkPriority.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				chkEmergency.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkEmergency.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboEmergencyTime.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboEmergencyTime.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lcgOtherRequest.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lcgOtherRequest.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciTreatmentType.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciTreatmentType.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciTreatmentType.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciTreatmentType.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciIsNotRequireFee.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciIsNotRequireFee.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciIsNotRequireFee.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciIsNotRequireFee.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciOweType.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciOweType.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciOweType.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciOweType.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciEmergency.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciEmergency.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciIntructionTime.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciIntructionTime.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciFortxtMaMS.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciFortxtMaMS.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciForchkCapMaMS.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciForchkCapMaMS.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciPriority.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciPriority.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem1.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.layoutControlItem1.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.layoutControlItem1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciFortxtIncode.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciFortxtIncode.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciPatientClassify.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciPatientClassify.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciPatientClassify.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciPatientClassify.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciTuberculosis.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciTuberculosis.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciTuberculosis.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciTuberculosis.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciEmergencyTime.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciEmergencyTime.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciEmergencyTime.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciEmergencyTime.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciCboCTT.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciCboCTT.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciCboCTT.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciCboCTT.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem3.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.layoutControlItem3.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem3.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.layoutControlItem3.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciGuaranteeLoginname.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciGuaranteeLoginname.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciGuaranteeLoginname.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciGuaranteeLoginname.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciIsChronic.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciIsChronic.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciFortxtSTTPriority.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciFortxtSTTPriority.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciFortxtSTTPriority.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciFortxtSTTPriority.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciTreatmentOrder.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciTreatmentOrder.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciTreatmentOrder.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciTreatmentOrder.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciGuaranteeReason.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciGuaranteeReason.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciGuaranteeReason.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciGuaranteeReason.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateMaxlength(BaseEdit control, int maxlenght)
		{
			try
			{
				ControlMaxLengthValidationRule controlMaxLengthValidationRule = new ControlMaxLengthValidationRule();
				controlMaxLengthValidationRule.editor = control;
				controlMaxLengthValidationRule.maxLength = maxlenght;
				controlMaxLengthValidationRule.IsRequired = false;
				controlMaxLengthValidationRule.ErrorText = string.Format("Nhập quá kí tự cho phép ({0})", maxlenght);
				controlMaxLengthValidationRule.ErrorType = ErrorType.Warning;
				dxValidationUCOtherReqInfo.SetValidationRule(control, controlMaxLengthValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

        private async Task LoadPriorityType()
        {
            try
            {
                List<HIS_PRIORITY_TYPE> dataPriorityTypes = null;

                if (BackendDataWorker.IsExistsKey<HIS_PRIORITY_TYPE>())
                {
                    dataPriorityTypes = BackendDataWorker.Get<HIS_PRIORITY_TYPE>();
                }
                else
                {
                    CommonParam paramCommon = new CommonParam();
                    dynamic filter = new ExpandoObject();

                    dataPriorityTypes = await new BackendAdapter(paramCommon)
                        .GetAsync<List<HIS_PRIORITY_TYPE>>(
                            "api/HisPriorityType/Get",
                            HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer,
                            filter,
                            paramCommon);

                    if (dataPriorityTypes != null)
                    {
                        BackendDataWorker.UpdateToRam(
                            typeof(HIS_PRIORITY_TYPE),
                            dataPriorityTypes,
                            long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
                    }
                }

                InitComboCommon(
                    cboPriorityType,
                    dataPriorityTypes,
                    "ID",
                    "PRIORITY_TYPE_NAME",
                    "PRIORITY_TYPE_CODE");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

		public void AutoCheckPriorityByPriorityType(long patientDob, string heinCardNumber)
		{
			long patientAge = 0L;
			try
			{
				List<HIS_PRIORITY_TYPE> list = null;
				List<HIS_PRIORITY_TYPE> list2 = (from o in BackendDataWorker.Get<HIS_PRIORITY_TYPE>()
					where o.IS_FOR_EXAM_SUBCLINICAL == 1
					select o).ToList();
				if (list2 != null && list2.Count > 0 && (patientDob > 0 || !string.IsNullOrEmpty(heinCardNumber)))
				{
					DateTime value = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(patientDob).Value;
					long ticks = (DateTime.Now - value).Ticks;
					int num = new DateTime(ticks).Year - 1;
					patientAge = ((num == 0) ? 1 : num);
					list = list2.Where((HIS_PRIORITY_TYPE o) => (!o.AGE_FROM.HasValue || o.AGE_FROM <= patientAge) && (!o.AGE_TO.HasValue || o.AGE_TO >= patientAge) && (string.IsNullOrEmpty(o.BHYT_PREFIXS) || (!string.IsNullOrEmpty(o.BHYT_PREFIXS) && StartIn(o.BHYT_PREFIXS, heinCardNumber))) && ((o.AGE_FROM.HasValue && o.AGE_FROM > 0) || (o.AGE_TO.HasValue && o.AGE_TO > 0) || !string.IsNullOrEmpty(o.BHYT_PREFIXS))).ToList();
					hasDataAutoCheckPriority = list != null && list.Count > 0;
					chkPriority.Checked = hasDataAutoCheckPriority;
					if (hasDataAutoCheckPriority && list != null && list.Count > 0)
					{
						cboPriorityType.EditValue = list.FirstOrDefault().ID;
						lciPriority.AppearanceItemCaption.ForeColor = Color.Maroon;
						ValidatePriorityType();
					}
					else
					{
						lciPriority.AppearanceItemCaption.ForeColor = Color.Black;
						cboPriorityType.EditValue = null;
						dxValidationUCOtherReqInfo.SetValidationRule(chkPriority, null);
						dxValidationUCOtherReqInfo.SetValidationRule(cboPriorityType, null);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void ChangePatientType(long patientTypeId)
		{
			try
			{
				if (IsChangeFromClassify)
				{
					return;
				}
				string text = ConfigApplicationWorker.Get<string>("CONFIG_KEY__DEFAULT_CONFIG_IS_NOT_REQUIRE_FEE");
				LogSystem.Debug("CONFIG_KEY__DEFAULT_CONFIG_IS_NOT_REQUIRE_FEE" + text);
				LogSystem.Debug("ChangePatientType patientTypeId" + patientTypeId);
				if (patientTypeId <= 0)
				{
					return;
				}
				workingPatientType = (from o in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
					where o.ID == patientTypeId
					select o).FirstOrDefault();
				if (workingPatientType == null)
				{
					return;
				}
				if (!string.IsNullOrEmpty(text))
				{
					List<string> list = text.Split(',').ToList();
					chkIsNotRequireFee.Checked = list != null && list.Count > 0 && list.Contains(workingPatientType.PATIENT_TYPE_CODE);
				}
				List<HIS_OTHER_PAY_SOURCE> list2 = BackendDataWorker.Get<HIS_OTHER_PAY_SOURCE>();
				if (!string.IsNullOrEmpty(workingPatientType.OTHER_PAY_SOURCE_IDS))
				{
					list2 = ((list2 != null) ? list2.Where((HIS_OTHER_PAY_SOURCE o) => o.IS_ACTIVE == 1 && ("," + workingPatientType.OTHER_PAY_SOURCE_IDS + ",").Contains("," + o.ID + ",")).ToList() : null);
					InitComboCommon(cboOtherPaySource, list2, "ID", "OTHER_PAY_SOURCE_NAME", "OTHER_PAY_SOURCE_CODE");
					dataOtherPayTemp = list2;
					if (list2 != null && list2.Count == 1)
					{
						cboOtherPaySource.EditValue = list2[0].ID;
					}
					else
					{
						cboOtherPaySource.EditValue = null;
					}
				}
				else
				{
					list2 = ((list2 != null) ? list2.Where((HIS_OTHER_PAY_SOURCE o) => o.IS_ACTIVE == 1).ToList() : null);
					InitComboCommon(cboOtherPaySource, list2, "ID", "OTHER_PAY_SOURCE_NAME", "OTHER_PAY_SOURCE_CODE");
					dataOtherPayTemp = list2;
					cboOtherPaySource.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool StartIn(string BHYT_PREFIXS, string heincardnumber)
		{
			bool result = false;
			try
			{
				List<string> list = null;
				if (!string.IsNullOrEmpty(BHYT_PREFIXS) && !string.IsNullOrEmpty(heincardnumber))
				{
					string[] array = BHYT_PREFIXS.Split(new string[2] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
					if (array != null && array.Count() > 0)
					{
						list = (from o in array.ToList()
							where heincardnumber.StartsWith(o)
							select o).ToList();
						result = ((list != null && list.Count > 0) ? true : false);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

        private async Task LoadEmergencyWtimes()
        {
            try
            {
                List<HIS_EMERGENCY_WTIME> dataEmergencyWtimes = null;

                if (BackendDataWorker.IsExistsKey<HIS_EMERGENCY_WTIME>())
                {
                    dataEmergencyWtimes = BackendDataWorker.Get<HIS_EMERGENCY_WTIME>();
                }
                else
                {
                    CommonParam paramCommon = new CommonParam();
                    dynamic filter = new ExpandoObject();

                    dataEmergencyWtimes = await new BackendAdapter(paramCommon)
                        .GetAsync<List<HIS_EMERGENCY_WTIME>>(
                            "api/HisEmergencyWtime/Get",
                            HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer,
                            filter,
                            paramCommon);

                    if (dataEmergencyWtimes != null)
                    {
                        BackendDataWorker.UpdateToRam(
                            typeof(HIS_EMERGENCY_WTIME),
                            dataEmergencyWtimes,
                            long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
                    }
                }

                if (dataEmergencyWtimes != null && dataEmergencyWtimes.Count > 0)
                {
                    dataEmergencyWtimes = dataEmergencyWtimes
                        .Where(p => p.IS_ACTIVE == 1)
                        .ToList();
                }

                InitComboCommon(
                    cboEmergencyTime,
                    dataEmergencyWtimes,
                    "ID",
                    "EMERGENCY_WTIME_NAME",
                    "EMERGENCY_WTIME_CODE");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private async Task LoadTreatmentTypes()
        {
            try
            {
                List<HIS_TREATMENT_TYPE> dataTreatmentTypes = null;

                if (BackendDataWorker.IsExistsKey<HIS_TREATMENT_TYPE>())
                {
                    dataTreatmentTypes = BackendDataWorker.Get<HIS_TREATMENT_TYPE>();
                }
                else
                {
                    CommonParam paramCommon = new CommonParam();
                    dynamic filter = new ExpandoObject();

                    dataTreatmentTypes = await new BackendAdapter(paramCommon)
                        .GetAsync<List<HIS_TREATMENT_TYPE>>(
                            "api/HisTreatmentType/Get",
                            HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer,
                            filter,
                            paramCommon);

                    if (dataTreatmentTypes != null)
                    {
                        BackendDataWorker.UpdateToRam(
                            typeof(HIS_TREATMENT_TYPE),
                            dataTreatmentTypes,
                            long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
                    }
                }

                if (dataTreatmentTypes != null && dataTreatmentTypes.Count > 0)
                {
                    dataTreatmentTypes = dataTreatmentTypes
                        .Where(p => p.IS_ACTIVE == 1 && p.IS_ALLOW_RECEPTION == 1)
                        .ToList();
                }

                InitComboCommon(
                    cboTreatmentType,
                    dataTreatmentTypes,
                    "ID",
                    "TREATMENT_TYPE_NAME",
                    70,
                    "TREATMENT_TYPE_CODE",
                    30);

                cboTreatmentType.EditValue =
                    (dataTreatmentTypes != null &&
                     dataTreatmentTypes.Count > 0 &&
                     dataTreatmentTypes.FirstOrDefault(p => p.ID == 1) != null)
                    ? 1L
                    : 0L;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private async Task LoadOweTypes()
        {
            try
            {
                List<HIS_OWE_TYPE> dataOweTypes = null;

                if (BackendDataWorker.IsExistsKey<HIS_OWE_TYPE>())
                {
                    dataOweTypes = BackendDataWorker.Get<HIS_OWE_TYPE>();
                }
                else
                {
                    CommonParam paramCommon = new CommonParam();
                    dynamic filter = new ExpandoObject();

                    dataOweTypes = await new BackendAdapter(paramCommon)
                        .GetAsync<List<HIS_OWE_TYPE>>(
                            "api/HisOweType/Get",
                            HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer,
                            filter,
                            paramCommon);

                    if (dataOweTypes != null)
                    {
                        BackendDataWorker.UpdateToRam(
                            typeof(HIS_OWE_TYPE),
                            dataOweTypes,
                            long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
                    }
                }

                if (dataOweTypes != null && dataOweTypes.Count > 0)
                {
                    dataOweTypes = dataOweTypes
                        .Where(p => p.IS_ACTIVE == 1)
                        .ToList();
                }

                InitComboCommon(
                    cboOweType,
                    dataOweTypes,
                    "ID",
                    "OWE_TYPE_NAME",
                    "OWE_TYPE_CODE");

                if (!string.IsNullOrEmpty(AppConfigs.OweTypeDefault)
                    && dataOweTypes != null
                    && dataOweTypes.Count > 0)
                {
                    HIS_OWE_TYPE oweType = dataOweTypes
                        .FirstOrDefault(o => o.OWE_TYPE_CODE == AppConfigs.OweTypeDefault);

                    if (oweType == null)
                    {
                        throw new ArgumentNullException(
                            "Khong tim thay HIS_OWE_TYPE theo OWE_TYPE_CODE = " + AppConfigs.OweTypeDefault);
                    }

                    cboOweType.EditValue = oweType.ID;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private async Task LoadFunds()
        {
            try
            {
                List<HIS_FUND> dataFunds = null;

                if (BackendDataWorker.IsExistsKey<HIS_FUND>())
                {
                    dataFunds = BackendDataWorker.Get<HIS_FUND>();
                }
                else
                {
                    CommonParam paramCommon = new CommonParam();
                    dynamic filter = new ExpandoObject();

                    dataFunds = await new BackendAdapter(paramCommon)
                        .GetAsync<List<HIS_FUND>>(
                            "api/HisFund/Get",
                            HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer,
                            filter,
                            paramCommon);

                    if (dataFunds != null)
                    {
                        BackendDataWorker.UpdateToRam(
                            typeof(HIS_FUND),
                            dataFunds,
                            long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
                    }
                }

                if (dataFunds != null && dataFunds.Count > 0)
                {
                    dataFunds = dataFunds
                        .Where(o => o.IS_ACTIVE == 1)
                        .ToList();
                }

                InitComboCommon(
                    cboCTT,
                    dataFunds,
                    "ID",
                    "FUND_NAME",
                    "FUND_CODE");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private async Task LoadPatientClassify()
        {
            try
            {
                if (BackendDataWorker.IsExistsKey<HIS_PATIENT_CLASSIFY>())
                {
                    dataClassify = BackendDataWorker.Get<HIS_PATIENT_CLASSIFY>();
                }
                else
                {
                    CommonParam paramCommon = new CommonParam();
                    dynamic filter = new ExpandoObject();

                    dataClassify = await new BackendAdapter(paramCommon)
                        .GetAsync<List<HIS_PATIENT_CLASSIFY>>(
                            "api/HisPatientClassify/Get",
                            HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer,
                            filter,
                            paramCommon);

                    if (dataClassify != null)
                    {
                        BackendDataWorker.UpdateToRam(
                            typeof(HIS_PATIENT_CLASSIFY),
                            dataClassify,
                            long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
                    }
                }

                if (dataClassify != null && dataClassify.Count > 0)
                {
                    dataClassify = dataClassify
                        .Where(o => o.IS_ACTIVE == 1)
                        .ToList();
                }

                InitComboCommon(
                    cboPatientClassify,
                    dataClassify,
                    "ID",
                    "PATIENT_CLASSIFY_NAME",
                    "PATIENT_CLASSIFY_CODE");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private async Task LoadGuarantee()
        {
            try
            {
                List<ACS_USER> dataUser = null;

                if (BackendDataWorker.IsExistsKey<ACS_USER>())
                {
                    dataUser = BackendDataWorker.Get<ACS_USER>();
                }
                else
                {
                    CommonParam paramCommon = new CommonParam();
                    dynamic filter = new ExpandoObject();

                    dataUser = await new BackendAdapter(paramCommon)
                        .GetAsync<List<ACS_USER>>(
                            "api/AcsUser/Get",
                            HIS.Desktop.ApiConsumer.ApiConsumers.AcsConsumer,
                            filter,
                            paramCommon);

                    if (dataUser != null)
                    {
                        BackendDataWorker.UpdateToRam(
                            typeof(ACS_USER),
                            dataUser,
                            long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
                    }
                }

                if (dataUser != null && dataUser.Count > 0)
                {
                    dataUser = dataUser
                        .Where(o => o.IS_ACTIVE == 1)
                        .ToList();
                }

                InitComboCommon(
                    cboGuaranteeUsername,
                    dataUser,
                    "LOGINNAME",
                    "USERNAME",
                    "LOGINNAME");

                cboGuaranteeUsername.Properties.ImmediatePopup = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

		private void LoadNguonKhach()
		{
			try
			{
				List<HIS_CUSTOMER_SOURCE> list = BackendDataWorker.Get<HIS_CUSTOMER_SOURCE>();
				list = ((list != null) ? list.Where((HIS_CUSTOMER_SOURCE o) => o.IS_ACTIVE == 1).ToList() : null);
				InitComboCommon(cboNguonKhach, list, "CUSTOMER_SOURCE_CODE", "CUSTOMER_SOURCE_NAME", "CUSTOMER_SOURCE_CODE");
				cboNguonKhach.Properties.ImmediatePopup = true;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadOtherPaySource()
		{
			try
			{
				List<HIS_OTHER_PAY_SOURCE> list = BackendDataWorker.Get<HIS_OTHER_PAY_SOURCE>();
				list = ((list != null) ? list.Where((HIS_OTHER_PAY_SOURCE o) => o.IS_ACTIVE == 1).ToList() : null);
				InitComboCommon(cboOtherPaySource, list, "ID", "OTHER_PAY_SOURCE_NAME", "OTHER_PAY_SOURCE_CODE");
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetCaptionByLanguageKey()
		{
			try
			{
				ResourceLanguageManager.ResourceUCOtherServiceReqInfo = new ResourceManager("HIS.UC.UCOtherServiceReqInfo.Resources.Lang", typeof(UCOtherServiceReqInfo).Assembly);
				lcUCOtherServiceReqInfo.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.layoutControl1.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				chkIsChronic.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkIsChronic.Properties.Caption", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				cboOweType.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboOweType.Properties.NullText", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				cboTreatmentType.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboTreatmentType.Properties.NullText", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				chkIsNotRequireFee.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkIsNotRequireFee.Properties.Caption", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				chkPriority.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkPriority.Properties.Caption", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				chkEmergency.Properties.Caption = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.chkEmergency.Properties.Caption", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				cboEmergencyTime.Properties.NullText = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.cboEmergencyTime.Properties.NullText", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				lcgOtherRequest.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lcgOtherRequest.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				lciTreatmentType.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciTreatmentType.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				lciPriority.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciPriority.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				lciIsNotRequireFee.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciIsNotRequireFee.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				lciIsChronic.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciIsChronic.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				lciEmergencyTime.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciEmergencyTime.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				lciOweType.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciOweType.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				lciEmergency.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciEmergency.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
				lciIntructionTime.Text = Inventec.Common.Resource.Get.Value("UCOtherServiceReqInfo.lciIntructionTime.Text", ResourceLanguageManager.ResourceUCOtherServiceReqInfo, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboTreatmentType_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					FocusTochkPriority();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkPriority_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					FocusToPriorityType();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkEmergency_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkEmergency.Checked)
				{
					lciEmergencyTime.Enabled = true;
				}
				else
				{
					lciEmergencyTime.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkEmergency_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					FocusTochkIsNotRequireFee();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkIsNotRequireFee_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					FocusTochkIsChronic();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkIsChronic_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					FocusToEmergencyTime();
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtIntructionTime_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Down)
				{
					DateTime? dateTime = DateTimeHelper.ConvertDateTimeStringToSystemTime(txtIntructionTime.Text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
					{
						dtIntructionTime.EditValue = dateTime;
						dtIntructionTime.Update();
					}
					dtIntructionTime.Visible = true;
					dtIntructionTime.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtIntructionTime_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Down)
				{
					dtIntructionTime.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtIntructionTime_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					cboTreatmentType.Focus();
					cboTreatmentType.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtIntructionTime_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					dtIntructionTime.Update();
					dtIntructionTime.Visible = false;
					txtIntructionTime.Text = dtIntructionTime.DateTime.ToString("dd/MM/yyyy HH:mm");
					cboTreatmentType.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtIntructionTime_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtIntructionTime.Visible = true;
					dtIntructionTime.Update();
					dtIntructionTime.Text = DateTime.Now.ToString("dd/MM/yyyy");
					Thread.Sleep(100);
					chkIsNotRequireFee.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtIntructionTime_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '/')
				{
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboEmergencyTime_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				FocusToOweType();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboOweType_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				cboCTT.Focus();
				cboCTT.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtIntructionTime_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				SetHeinRighRouteTypeByTime();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void SetHeinRighRouteTypeByTime()
		{
			try
			{
				if (string.IsNullOrEmpty(txtIntructionTime.Text))
				{
					return;
				}
				try
				{
					DateTime.ParseExact(txtIntructionTime.Text, "dd/MM/yyyy HH:mm", null);
				}
				catch (Exception ex)
				{
					LogSystem.Error(ex);
					return;
				}
				bool flag = true;
				if (_IsUserBranchTime)
				{
					if (_BranchTimes != null && _BranchTimes.Count > 0)
					{
						DateTime? dateTime = DateTimeHelper.ConvertDateTimeStringToSystemTime(txtIntructionTime.Text);
						if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
						{
							dtIntructionTime.EditValue = dateTime;
							dtIntructionTime.Update();
							if (dtIntructionTime.EditValue != null)
							{
								int day = (int)dtIntructionTime.DateTime.DayOfWeek;
								string inputValue = dtIntructionTime.DateTime.ToString("HHmmss");
								List<HIS_BRANCH_TIME> list = _BranchTimes.Where((HIS_BRANCH_TIME p) => p.DAY == day + 1).ToList();
								if (list != null && list.Count > 0)
								{
									foreach (HIS_BRANCH_TIME item in list)
									{
										long num = Parse.ToInt64(item.FROM_TIME);
										long num2 = Parse.ToInt64(item.TO_TIME);
										if (num <= Parse.ToInt64(inputValue) && Parse.ToInt64(inputValue) <= num2)
										{
											flag = false;
											break;
										}
									}
								}
							}
						}
					}
					chkEmergency.Checked = flag;
					if (dlgHeinRightRouteType != null)
					{
						dlgHeinRightRouteType(flag);
					}
				}
				else
				{
					chkEmergency.Checked = false;
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		public void FillDataOweTypeDefault()
		{
			try
			{
				List<HIS_OWE_TYPE> list = BackendDataWorker.Get<HIS_OWE_TYPE>();
				if (!string.IsNullOrEmpty(AppConfigs.OweTypeDefault) && list != null && list.Count > 0)
				{
					HIS_OWE_TYPE val = list.SingleOrDefault((HIS_OWE_TYPE o) => o.OWE_TYPE_CODE == AppConfigs.OweTypeDefault);
					if (val == null)
					{
						throw new ArgumentNullException("Khong tim thay HIS_OWE_TYPE theo OWE_TYPE_CODE = " + AppConfigs.OweTypeDefault);
					}
					cboOweType.EditValue = val.ID;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void ReloadValidation(bool _isValid)
		{
			try
			{
				if (_isValid)
				{
					ValidHrmKskCode();
					return;
				}
				ControlWorker.ValidationProviderRemoveControlError(dxValidationUCOtherReqInfo, dxErrorProviderControl);
				lciGuaranteeLoginname.AppearanceItemCaption.ForeColor = Color.Black;
				dxValidationUCOtherReqInfo.SetValidationRule(txtGuaranteeLoginname, null);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void RefreshData()
		{
			try
			{
				txtGuaranteeLoginname.Text = "";
				cboGuaranteeUsername.EditValue = null;
				ControlWorker.ValidationProviderRemoveControlError(dxValidationUCOtherReqInfo, dxErrorProviderControl);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidHrmKskCode()
		{
			try
			{
				lciGuaranteeLoginname.AppearanceItemCaption.ForeColor = Color.Maroon;
				Combo___ValidationRule combo___ValidationRule = new Combo___ValidationRule();
				combo___ValidationRule.txt = txtGuaranteeLoginname;
				combo___ValidationRule.cbo = cboGuaranteeUsername;
				combo___ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationUCOtherReqInfo.SetValidationRule(txtGuaranteeLoginname, combo___ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void CheckExamOnline(bool isChecked)
		{
			chkExamOnline.Checked = isChecked;
		}

		public void ShowOrtherPay(long payId)
		{
			try
			{
				if (payId > 0)
				{
					LoadOtherPaySource();
					cboOtherPaySource.EditValue = payId;
					cboOtherPaySource.Enabled = false;
					IsChangeFromClassify = true;
				}
				else
				{
					IsChangeFromClassify = false;
					InitComboCommon(cboOtherPaySource, dataOtherPayTemp, "ID", "OTHER_PAY_SOURCE_NAME", "OTHER_PAY_SOURCE_CODE");
					cboOtherPaySource.EditValue = null;
					cboOtherPaySource.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnAddCTT_Click(object sender, EventArgs e)
		{
			try
			{
				if (cboCTT.EditValue == null)
				{
					return;
				}
				if (_IsAutoSetOweType)
				{
					HIS_OWE_TYPE val = BackendDataWorker.Get<HIS_OWE_TYPE>().FirstOrDefault((HIS_OWE_TYPE p) => p.IS_ACTIVE == 1 && p.ID == 3);
					if (val != null)
					{
						cboOweType.EditValue = 3L;
					}
				}
				frmFun frmFun = new frmFun(_HisTreatment);
				frmFun.MyGetData = GetInfoHisFun;
				frmFun.ShowDialog();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void GetInfoHisFun(HIS_TREATMENT _hisTreatment)
		{
			try
			{
				_HisTreatment = _hisTreatment;
				if (dlgFocusNextUserControl != null)
				{
					dlgFocusNextUserControl(null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboCTT_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
					return;
				}
				if (cboCTT.EditValue != null)
				{
					btnAddCTT.Enabled = true;
					btnAddCTT_Click(null, null);
					cboCTT.Properties.Buttons[1].Visible = true;
					return;
				}
				btnAddCTT.Enabled = false;
				if (dlgFocusNextUserControl != null)
				{
					dlgFocusNextUserControl(null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboCTT_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboCTT.EditValue = null;
					cboCTT.Properties.Buttons[1].Visible = false;
					_HisTreatment = new HIS_TREATMENT();
					_HisTreatment.FUND_CUSTOMER_NAME = _PatientName;
					if (_IsAutoSetOweType && cboOweType.EditValue != null && (long)cboOweType.EditValue == 3)
					{
						cboOweType.EditValue = null;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtSTTPriority_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (dlgPriorityNumberChanged != null)
				{
					dlgPriorityNumberChanged((txtSTTPriority.EditValue != null) ? new long?((long)txtSTTPriority.Value) : ((long?)null));
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtTreatmentOrder_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
				{
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPriorityType_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal || e.CloseMode == PopupCloseMode.Immediate)
				{
					if (lciFortxtIncode.Visibility == LayoutVisibility.Always)
					{
						FocusToIncode();
					}
					else if (cboEmergencyTime.Enabled)
					{
						FocusToEmergencyTime();
					}
					else
					{
						FocusToOweType();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPriorityType_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Clear || e.Button.Kind == ButtonPredefines.Delete)
				{
					cboPriorityType.EditValue = null;
					cboPriorityType.Properties.Buttons[1].Visible = false;
					chkPriority.Checked = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPriorityType_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				cboPriorityType.Properties.Buttons[1].Visible = cboPriorityType.EditValue != null;
				if (cboPriorityType.EditValue != null)
				{
					chkPriority.Checked = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPriorityType_KeyUp(object sender, KeyEventArgs e)
		{
		}

		private void cboPriorityType_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
		}

		private void cboPriorityType_KeyDown(object sender, KeyEventArgs e)
		{
		}

		private void cboOtherPaySource_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboOtherPaySource.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboOtherPaySource_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				cboOtherPaySource.Properties.Buttons[1].Visible = cboOtherPaySource.EditValue != null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void GetTreatmentTypeId(Action<string> _dlgGetTreatmentTypeId)
		{
			dlgGetTreatmentTypeId = _dlgGetTreatmentTypeId;
		}

		public void GetTreatmentTypeIdForUcHeinInfo(Action<string> _dlgGetTreatmentTypeId)
		{
			dlgGetTreatmentTypeIdForUcHeinInfo = _dlgGetTreatmentTypeId;
		}

		public void ReceiveTreatmentTypeId(string treatmentTypeId)
		{
			this.treatmentTypeId = treatmentTypeId;
			cboTreatmentType.EditValue = long.Parse(this.treatmentTypeId);
		}

		private void cboTreatmentType_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (dlgGetTreatmentTypeId != null)
				{
					dlgGetTreatmentTypeId(cboTreatmentType.EditValue.ToString());
				}
				if (dlgGetTreatmentTypeIdForUcHeinInfo != null)
				{
					dlgGetTreatmentTypeIdForUcHeinInfo(cboTreatmentType.EditValue.ToString());
				}
				long treatmentTypeId = ((cboTreatmentType.EditValue != null) ? Parse.ToInt64((cboTreatmentType.EditValue ?? "").ToString()) : 0);
				if (treatmentTypeId == 2 || treatmentTypeId == 3)
				{
					lciFortxtIncode.Visibility = LayoutVisibility.Always;
					if (HisConfig.IsManualInCode)
					{
						lciFortxtIncode.AppearanceItemCaption.ForeColor = Color.Maroon;
						lciFortxtIncode.Enabled = true;
						txtIncode.Enabled = true;
						ValidateFrmInCode();
					}
					else
					{
						lciFortxtIncode.Enabled = false;
						txtIncode.Enabled = false;
						dxValidationUCOtherReqInfo.SetValidationRule(txtIncode, null);
					}
				}
				else
				{
					lciFortxtIncode.AppearanceItemCaption.ForeColor = Color.Black;
					lciFortxtIncode.Visibility = LayoutVisibility.Never;
					dxValidationUCOtherReqInfo.SetValidationRule(txtIncode, null);
				}
				cboHosReason.EditValue = null;
				dxValidationUCOtherReqInfo.SetValidationRule(txtHosReason, null);
				dxValidationUCOtherReqInfo.SetValidationRule(txtHosReasonNt, null);
				lciHosReason.Visibility = LayoutVisibility.Never;
				lciHosReason.AppearanceItemCaption.ForeColor = Color.Black;
				if (cboTreatmentType.EditValue != null)
				{
					HIS_TREATMENT_TYPE val = BackendDataWorker.Get<HIS_TREATMENT_TYPE>().FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == treatmentTypeId);
					LayoutControlItem layoutControlItem = lciHosReason;
					LayoutVisibility visibility = (layoutControlItem8.Visibility = ((val == null || (val.ID != 2 && val.ID != 4 && val.ID != 3)) ? LayoutVisibility.Never : LayoutVisibility.Always));
					layoutControlItem.Visibility = visibility;
					if (layoutControlItem8.Visible)
					{
						ValidateTextHosReason();
					}
					if (layoutControlItem8.Visible && lciHosReason.Visible && ((TreatmentByPatientSdo != null && TreatmentByPatientSdo.IS_CHRONIC == 1) || (patientSdo != null && !string.IsNullOrEmpty(patientSdo.AppointmentCode))))
					{
						txtHosReason.Text = TreatmentByPatientSdo.HOSPITALIZATION_REASON;
						txtHosReasonNt.Text = TreatmentByPatientSdo.ICD_NAME;
					}
					if (HisConfigCFG.InHospitalizationReasonRequired && lciHosReason.Visible)
					{
						lciHosReason.AppearanceItemCaption.ForeColor = Color.Maroon;
						ValidateComboHosspitalizeReason();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidateComboHosspitalizeReason()
		{
			try
			{
				ControlEditValidationRule controlEditValidationRule = new ControlEditValidationRule();
				controlEditValidationRule.editor = txtHosReasonNt;
				controlEditValidationRule.ErrorText = "Trường dữ liệu bắt buộc";
				controlEditValidationRule.ErrorType = ErrorType.Warning;
				dxValidationUCOtherReqInfo.SetValidationRule(txtHosReasonNt, controlEditValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtIncode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					FocusTochkEmergency();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPatientClassify_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboPatientClassify.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboGuaranteeUsername_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboGuaranteeUsername.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboGuaranteeUsername_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (lciGuaranteeReason.Visibility == LayoutVisibility.Always)
				{
					txtGuaranteeReason.Focus();
					txtGuaranteeReason.SelectAll();
				}
				else if (dlgFocusNextUserControl != null)
				{
					dlgFocusNextUserControl(null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboGuaranteeUsername_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				txtGuaranteeLoginname.Text = "";
				if (cboGuaranteeUsername.EditValue != null)
				{
					ACS_USER aCS_USER = BackendDataWorker.Get<ACS_USER>().FirstOrDefault((ACS_USER o) => o.LOGINNAME == cboGuaranteeUsername.EditValue.ToString());
					if (aCS_USER != null)
					{
						txtGuaranteeLoginname.Text = aCS_USER.LOGINNAME;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtGuaranteeReason_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return && dlgFocusNextUserControl != null)
				{
					dlgFocusNextUserControl(null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtGuaranteeLoginname_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				string searchCode = (sender as TextEdit).Text;
				if (string.IsNullOrEmpty(searchCode))
				{
					cboGuaranteeUsername.EditValue = null;
					cboGuaranteeUsername.Focus();
					cboGuaranteeUsername.ShowPopup();
					return;
				}
				List<ACS_USER> list = (from o in BackendDataWorker.Get<ACS_USER>()
					where o.LOGINNAME.Contains(searchCode) || o.LOGINNAME.Contains(searchCode.ToLower())
					select o).ToList();
				if (list == null)
				{
					return;
				}
				if (list.Count == 1)
				{
					cboGuaranteeUsername.EditValue = list[0].LOGINNAME;
					txtGuaranteeLoginname.Text = list[0].LOGINNAME;
					cboGuaranteeUsername_Closed(null, null);
					return;
				}
				ACS_USER aCS_USER = BackendDataWorker.Get<ACS_USER>().FirstOrDefault((ACS_USER o) => o.LOGINNAME.ToLower().Equals(searchCode.ToLower()));
				if (aCS_USER != null)
				{
					cboGuaranteeUsername.EditValue = aCS_USER.LOGINNAME;
					txtGuaranteeLoginname.Text = aCS_USER.LOGINNAME;
					cboGuaranteeUsername_Closed(null, null);
				}
				else
				{
					cboGuaranteeUsername.EditValue = null;
					cboGuaranteeUsername.Focus();
					cboGuaranteeUsername.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboGuaranteeUsername_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					cboGuaranteeUsername_Closed(null, null);
				}
				else if (e.KeyCode == Keys.Down)
				{
					cboGuaranteeUsername.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPatientClassify_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (cboPatientClassify == null)
				{
					return;
				}
				HIS_PATIENT_CLASSIFY patientClassify = dataClassify.FirstOrDefault((HIS_PATIENT_CLASSIFY o) => o.ID == long.Parse(cboPatientClassify.EditValue.ToString()));
				if (patientClassify == null || !patientClassify.OTHER_PAY_SOURCE_ID.HasValue)
				{
					return;
				}
				HIS_OTHER_PAY_SOURCE val = dataOtherPayTemp.FirstOrDefault((HIS_OTHER_PAY_SOURCE o) => o.ID == patientClassify.OTHER_PAY_SOURCE_ID);
				if (val != null)
				{
					cboOtherPaySource.EditValue = patientClassify.OTHER_PAY_SOURCE_ID;
					return;
				}
				List<HIS_OTHER_PAY_SOURCE> list = dataOtherPayTemp;
				HIS_OTHER_PAY_SOURCE val2 = (from o in BackendDataWorker.Get<HIS_OTHER_PAY_SOURCE>()
					where o.ID == patientClassify.OTHER_PAY_SOURCE_ID
					select o).FirstOrDefault();
				if (val2 != null)
				{
					list.Add(val2);
				}
				InitComboCommon(cboOtherPaySource, list, "ID", "OTHER_PAY_SOURCE_NAME", "OTHER_PAY_SOURCE_CODE");
				cboOtherPaySource.EditValue = patientClassify.OTHER_PAY_SOURCE_ID;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void chkWNext_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				WaitingManager.Show();
				ControlStateRDO controlStateRDO = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkWNext.Name && o.MODULE_LINK == moduleLink).FirstOrDefault() : null);
				if (controlStateRDO != null)
				{
					controlStateRDO.VALUE = (chkWNext.Checked ? "1" : "");
				}
				else
				{
					controlStateRDO = new ControlStateRDO();
					controlStateRDO.KEY = chkWNext.Name;
					controlStateRDO.VALUE = (chkWNext.Checked ? "1" : "");
					controlStateRDO.MODULE_LINK = moduleLink;
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(controlStateRDO);
				}
				controlStateWorker.SetData(currentControlStateRDO);
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private void InitControlState()
		{
			try
			{
				controlStateWorker = new ControlStateWorker();
				currentControlStateRDO = controlStateWorker.GetData(moduleLink);
				if (currentControlStateRDO == null || currentControlStateRDO.Count <= 0)
				{
					return;
				}
				foreach (ControlStateRDO item in currentControlStateRDO)
				{
					if (item.KEY == chkWNext.Name)
					{
						chkWNext.Checked = item.VALUE == "1";
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkIsHiv_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				WaitingManager.Show();
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private async Task InitComboHisHospitalizeReason()
		{
			try
			{
				List<HIS_HOSPITALIZE_REASON> datas = (from o in BackendDataWorker.Get<HIS_HOSPITALIZE_REASON>()
					where o.IS_ACTIVE == 1
					select o).ToList();
				await InitComboHisHospitalizeReason(datas);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private async Task InitComboHisHospitalizeReason(List<HIS_HOSPITALIZE_REASON> data)
		{
			try
			{
				try
				{
					cboHosReason.Properties.DataSource = data;
					cboHosReason.Properties.DisplayMember = "HOSPITALIZE_REASON_NAME";
					cboHosReason.Properties.ValueMember = "ID";
					cboHosReason.Properties.View.OptionsView.GroupDrawMode = GroupDrawMode.Office;
					cboHosReason.Properties.View.OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.SmartTag;
					cboHosReason.Properties.View.OptionsView.ShowAutoFilterRow = true;
					cboHosReason.Properties.View.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
					cboHosReason.Properties.View.OptionsView.ShowDetailButtons = false;
					cboHosReason.Properties.View.OptionsView.ShowGroupPanel = false;
					cboHosReason.Properties.View.OptionsView.ShowIndicator = false;
					cboHosReason.Properties.View.RowCellClick += View_RowCellClick;
					GridColumn column = cboHosReason.Properties.View.Columns.AddField("HOSPITALIZE_REASON_CODE");
					column.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
					column.OptionsFilter.FilterBySortField = DefaultBoolean.True;
					column.VisibleIndex = 1;
					column.Width = 150;
					column.Caption = "Mã";
					GridColumn column2 = cboHosReason.Properties.View.Columns.AddField("HOSPITALIZE_REASON_NAME");
					column2.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
					column2.OptionsFilter.FilterBySortField = DefaultBoolean.True;
					column2.VisibleIndex = 2;
					column2.Width = 250;
					column2.Caption = "Tên";
					cboHosReason.Properties.View.OptionsView.ShowColumnHeaders = true;
					cboHosReason.Properties.View.OptionsSelection.MultiSelect = true;
					cboHosReason.Properties.ImmediatePopup = true;
				}
				catch (Exception ex)
				{
					Exception ex2 = ex;
					LogSystem.Warn(ex2);
				}
			}
			catch (Exception ex)
			{
				Exception ex3 = ex;
				LogSystem.Warn(ex3);
			}
		}

		private void View_RowCellClick(object sender, RowCellClickEventArgs e)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				cboHosReason.EditValue = ((HIS_HOSPITALIZE_REASON)cboHosReason.Properties.View.GetFocusedRow()).ID;
				cboHosReason.ClosePopup();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboHosReason_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboHosReason.Text = null;
					cboHosReason.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtHosReasonNt_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboHosReason.EditValue = null;
					txtHosReasonNt.Text = null;
				}
				else if (e.Button.Kind == ButtonPredefines.Combo)
				{
					cboHosReason.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboHosReason_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (cboHosReason.EditValue != null)
				{
					txtHosReasonNt.Text = (cboHosReason.Properties.DataSource as List<HIS_HOSPITALIZE_REASON>).FirstOrDefault((HIS_HOSPITALIZE_REASON o) => o.ID == long.Parse(cboHosReason.EditValue.ToString())).HOSPITALIZE_REASON_NAME;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtHosReasonNt_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				cboHosReason.ShowPopup();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboNguonKhach_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				txtNguonKhach.Text = "";
				if (cboNguonKhach.EditValue != null)
				{
					HIS_CUSTOMER_SOURCE user = BackendDataWorker.Get<HIS_CUSTOMER_SOURCE>().FirstOrDefault((HIS_CUSTOMER_SOURCE o) => o.CUSTOMER_SOURCE_CODE == cboNguonKhach.EditValue.ToString());
					HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection = cboNguonKhachCT.Properties.Tag as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection;
					if (user == null)
					{
						return;
					}
					txtNguonKhach.Text = user.CUSTOMER_SOURCE_CODE;
					List<HIS_CUSTOMER_SOURCE_DT> list = (from o in BackendDataWorker.Get<HIS_CUSTOMER_SOURCE_DT>()
						where o.IS_ACTIVE == 1 && o.CUSTOMER_SOURCE_ID == user.ID
						select o).ToList();
					if (!string.IsNullOrEmpty(user.DEFAULT_DETAIL_LOGINNAMES))
					{
						List<string> defaultLoginNames = (from x in user.DEFAULT_DETAIL_LOGINNAMES.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries)
							select x.Trim()).ToList();
						List<HIS_CUSTOMER_SOURCE_DT> second = (from o in BackendDataWorker.Get<HIS_CUSTOMER_SOURCE_DT>()
							where o.IS_ACTIVE == 1 && defaultLoginNames.Contains((o.LOGINNAME != null) ? o.LOGINNAME.Trim() : "")
							select o).ToList();
						list = (from o in list.Union(second)
							group o by (o.LOGINNAME != null) ? o.LOGINNAME.Trim() : "" into g
							select g.First()).ToList();
					}
					else
					{
						cboNguonKhachCT.EditValue = null;
						HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection2 = cboNguonKhachCT.Properties.Tag as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection;
						if (gridCheckMarksSelection2 != null)
						{
							gridCheckMarksSelection2.ClearSelection(cboNguonKhachCT.Properties.View);
						}
					}
					List<otherPaySourceDetailADO> list2 = new List<otherPaySourceDetailADO>();
					foreach (HIS_CUSTOMER_SOURCE_DT item in list)
					{
						otherPaySourceDetailADO otherPaySourceDetailADO = new otherPaySourceDetailADO();
						((HIS_CUSTOMER_SOURCE_DT)otherPaySourceDetailADO).ID = item.ID;
						((HIS_CUSTOMER_SOURCE_DT)otherPaySourceDetailADO).LOGINNAME = item.LOGINNAME;
						((HIS_CUSTOMER_SOURCE_DT)otherPaySourceDetailADO).USERNAME = item.USERNAME;
						otherPaySourceDetailADO.USERNAME_UNSIGN = convertToUnSign3(item.USERNAME);
						list2.Add(otherPaySourceDetailADO);
					}
					UpdateComboOtherDetailDataSource(list2);
					if (!string.IsNullOrEmpty(user.DEFAULT_DETAIL_LOGINNAMES) && gridCheckMarksSelection != null)
					{
						ProcessSelectOtherPaySourceDetail(user.DEFAULT_DETAIL_LOGINNAMES, gridCheckMarksSelection);
					}
					return;
				}
				List<otherPaySourceDetailADO> list3 = new List<otherPaySourceDetailADO>();
				List<HIS_CUSTOMER_SOURCE_DT> list4 = (from o in BackendDataWorker.Get<HIS_CUSTOMER_SOURCE_DT>()
					where o.IS_ACTIVE == 1
					select o).ToList();
				foreach (HIS_CUSTOMER_SOURCE_DT item2 in list4)
				{
					otherPaySourceDetailADO otherPaySourceDetailADO2 = new otherPaySourceDetailADO();
					((HIS_CUSTOMER_SOURCE_DT)otherPaySourceDetailADO2).ID = item2.ID;
					((HIS_CUSTOMER_SOURCE_DT)otherPaySourceDetailADO2).LOGINNAME = item2.LOGINNAME;
					((HIS_CUSTOMER_SOURCE_DT)otherPaySourceDetailADO2).USERNAME = item2.USERNAME;
					otherPaySourceDetailADO2.USERNAME_UNSIGN = convertToUnSign3(item2.USERNAME);
					list3.Add(otherPaySourceDetailADO2);
				}
				UpdateComboOtherDetailDataSource(list3);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboNguonKhach_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboNguonKhach.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboNguonKhachCT_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
		{
			try
			{
				string text = "";
				if (lstOtherDetail != null && lstOtherDetail.Count > 0)
				{
					foreach (HIS_CUSTOMER_SOURCE_DT item in lstOtherDetail)
					{
						text = text + item.USERNAME + ",";
					}
					if (text.EndsWith(","))
					{
						text = text.Substring(0, text.Length - 1);
					}
				}
				e.DisplayText = text;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboNguonKhachCT_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboNguonKhachCT.EditValue = null;
					HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection = cboNguonKhachCT.Properties.Tag as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection;
					if (gridCheckMarksSelection != null)
					{
						gridCheckMarksSelection.ClearSelection(cboNguonKhachCT.Properties.View);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCOtherServiceReqInfo));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject12 = new DevExpress.Utils.SerializableAppearanceObject();
            this.lcUCOtherServiceReqInfo = new DevExpress.XtraLayout.LayoutControl();
            this.chkChamSocDa = new DevExpress.XtraEditors.CheckEdit();
            this.cboNguonKhachCT = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView5 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cboNguonKhach = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtNguonKhach = new DevExpress.XtraEditors.TextEdit();
            this.chkCAPD = new DevExpress.XtraEditors.CheckEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtHosReasonNt = new DevExpress.XtraEditors.ButtonEdit();
            this.cboHosReason = new Inventec.Desktop.CustomControl.CustomGrid.CustomGridLookUpEdit();
            this.customGridLookUpEdit1View = new Inventec.Desktop.CustomControl.CustomGrid.CustomGridView();
            this.txtHosReason = new DevExpress.XtraEditors.TextEdit();
            this.chkExamOnline = new DevExpress.XtraEditors.CheckEdit();
            this.chkIsHiv = new DevExpress.XtraEditors.CheckEdit();
            this.chkWNext = new DevExpress.XtraEditors.CheckEdit();
            this.txtNote = new DevExpress.XtraEditors.MemoEdit();
            this.chkTuberculosis = new DevExpress.XtraEditors.CheckEdit();
            this.txtGuaranteeReason = new DevExpress.XtraEditors.TextEdit();
            this.cboGuaranteeUsername = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtGuaranteeLoginname = new DevExpress.XtraEditors.TextEdit();
            this.cboPatientClassify = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtIncode = new DevExpress.XtraEditors.TextEdit();
            this.cboOtherPaySource = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtMaMS = new DevExpress.XtraEditors.TextEdit();
            this.chkCapMaMS = new DevExpress.XtraEditors.CheckEdit();
            this.cboPriorityType = new DevExpress.XtraEditors.LookUpEdit();
            this.txtTreatmentOrder = new DevExpress.XtraEditors.TextEdit();
            this.txtSTTPriority = new DevExpress.XtraEditors.SpinEdit();
            this.btnAddCTT = new DevExpress.XtraEditors.SimpleButton();
            this.cboCTT = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtIntructionTime = new DevExpress.XtraEditors.ButtonEdit();
            this.dtIntructionTime = new DevExpress.XtraEditors.DateEdit();
            this.chkIsChronic = new DevExpress.XtraEditors.CheckEdit();
            this.cboOweType = new DevExpress.XtraEditors.LookUpEdit();
            this.cboTreatmentType = new DevExpress.XtraEditors.LookUpEdit();
            this.chkIsNotRequireFee = new DevExpress.XtraEditors.CheckEdit();
            this.chkPriority = new DevExpress.XtraEditors.CheckEdit();
            this.chkEmergency = new DevExpress.XtraEditors.CheckEdit();
            this.cboEmergencyTime = new DevExpress.XtraEditors.LookUpEdit();
            this.lcgOtherRequest = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciTreatmentType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciIsNotRequireFee = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciOweType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciEmergency = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciIntructionTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciFortxtMaMS = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciForchkCapMaMS = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPriority = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciFortxtIncode = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPatientClassify = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTuberculosis = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciEmergencyTime = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciCboCTT = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciGuaranteeLoginname = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciGuaranteeUsername = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciIsChronic = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciFortxtSTTPriority = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTreatmentOrder = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciGuaranteeReason = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciHosReason = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.dxValidationUCOtherReqInfo = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            this.dxErrorProviderControl = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.timerInitForm = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.lcUCOtherServiceReqInfo)).BeginInit();
            this.lcUCOtherServiceReqInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkChamSocDa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboNguonKhachCT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboNguonKhach.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNguonKhach.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCAPD.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHosReasonNt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboHosReason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHosReason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkExamOnline.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsHiv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWNext.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTuberculosis.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGuaranteeReason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboGuaranteeUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGuaranteeLoginname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPatientClassify.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIncode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOtherPaySource.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaMS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCapMaMS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPriorityType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentOrder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSTTPriority.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCTT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIntructionTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtIntructionTime.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtIntructionTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsChronic.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOweType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTreatmentType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsNotRequireFee.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPriority.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEmergency.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEmergencyTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOtherRequest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIsNotRequireFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOweType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEmergency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIntructionTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFortxtMaMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciForchkCapMaMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFortxtIncode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPatientClassify)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTuberculosis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEmergencyTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCboCTT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGuaranteeLoginname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGuaranteeUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIsChronic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFortxtSTTPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGuaranteeReason)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciHosReason)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationUCOtherReqInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProviderControl)).BeginInit();
            this.SuspendLayout();
            // 
            // lcUCOtherServiceReqInfo
            // 
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkChamSocDa);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboNguonKhachCT);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboNguonKhach);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.txtNguonKhach);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkCAPD);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.panel2);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.txtHosReason);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkExamOnline);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkIsHiv);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkWNext);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.txtNote);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkTuberculosis);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.txtGuaranteeReason);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboGuaranteeUsername);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.txtGuaranteeLoginname);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboPatientClassify);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.txtIncode);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboOtherPaySource);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.txtMaMS);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkCapMaMS);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboPriorityType);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.txtTreatmentOrder);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.txtSTTPriority);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.btnAddCTT);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboCTT);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.panel1);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkIsChronic);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboOweType);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboTreatmentType);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkIsNotRequireFee);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkPriority);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.chkEmergency);
            this.lcUCOtherServiceReqInfo.Controls.Add(this.cboEmergencyTime);
            this.lcUCOtherServiceReqInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lcUCOtherServiceReqInfo.Location = new System.Drawing.Point(0, 0);
            this.lcUCOtherServiceReqInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lcUCOtherServiceReqInfo.Name = "lcUCOtherServiceReqInfo";
            this.lcUCOtherServiceReqInfo.Root = this.lcgOtherRequest;
            this.lcUCOtherServiceReqInfo.Size = new System.Drawing.Size(759, 382);
            this.lcUCOtherServiceReqInfo.TabIndex = 0;
            this.lcUCOtherServiceReqInfo.Text = "layoutControl1";
            // 
            // chkChamSocDa
            // 
            this.chkChamSocDa.Location = new System.Drawing.Point(101, 138);
            this.chkChamSocDa.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkChamSocDa.Name = "chkChamSocDa";
            this.chkChamSocDa.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.chkChamSocDa.Properties.Appearance.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.chkChamSocDa.Properties.Appearance.Options.UseBackColor = true;
            this.chkChamSocDa.Properties.Appearance.Options.UseForeColor = true;
            this.chkChamSocDa.Properties.Caption = "Nhận chỉ dẫn hỗ trợ điều trị bằng các sản phẩm chăm sóc da";
            this.chkChamSocDa.Size = new System.Drawing.Size(539, 21);
            this.chkChamSocDa.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkChamSocDa.TabIndex = 57;
            // 
            // cboNguonKhachCT
            // 
            this.cboNguonKhachCT.Location = new System.Drawing.Point(450, 328);
            this.cboNguonKhachCT.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboNguonKhachCT.Name = "cboNguonKhachCT";
            this.cboNguonKhachCT.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboNguonKhachCT.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboNguonKhachCT.Properties.NullText = "";
            this.cboNguonKhachCT.Properties.View = this.gridView5;
            this.cboNguonKhachCT.Size = new System.Drawing.Size(305, 22);
            this.cboNguonKhachCT.StyleController = this.lcUCOtherServiceReqInfo;
            this.cboNguonKhachCT.TabIndex = 56;
            this.cboNguonKhachCT.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboNguonKhachCT_ButtonClick);
            this.cboNguonKhachCT.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.cboNguonKhachCT_CustomDisplayText);
            // 
            // gridView5
            // 
            this.gridView5.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView5.Name = "gridView5";
            this.gridView5.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView5.OptionsView.ShowGroupPanel = false;
            // 
            // cboNguonKhach
            // 
            this.cboNguonKhach.Location = new System.Drawing.Point(213, 327);
            this.cboNguonKhach.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboNguonKhach.Name = "cboNguonKhach";
            this.cboNguonKhach.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboNguonKhach.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboNguonKhach.Properties.NullText = "";
            this.cboNguonKhach.Properties.View = this.gridView4;
            this.cboNguonKhach.Size = new System.Drawing.Size(157, 22);
            this.cboNguonKhach.StyleController = this.lcUCOtherServiceReqInfo;
            this.cboNguonKhach.TabIndex = 54;
            this.cboNguonKhach.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboNguonKhach_ButtonClick);
            this.cboNguonKhach.EditValueChanged += new System.EventHandler(this.cboNguonKhach_EditValueChanged);
            // 
            // gridView4
            // 
            this.gridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            // 
            // txtNguonKhach
            // 
            this.txtNguonKhach.Location = new System.Drawing.Point(88, 327);
            this.txtNguonKhach.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNguonKhach.Name = "txtNguonKhach";
            this.txtNguonKhach.Size = new System.Drawing.Size(125, 22);
            this.txtNguonKhach.StyleController = this.lcUCOtherServiceReqInfo;
            this.txtNguonKhach.TabIndex = 53;
            // 
            // chkCAPD
            // 
            this.chkCAPD.Location = new System.Drawing.Point(659, 138);
            this.chkCAPD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCAPD.Name = "chkCAPD";
            this.chkCAPD.Properties.Caption = ":BN CAPD";
            this.chkCAPD.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkCAPD.Size = new System.Drawing.Size(96, 21);
            this.chkCAPD.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkCAPD.TabIndex = 52;
            this.chkCAPD.ToolTip = "Bệnh nhân điều trị lọc máu màng bụng";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtHosReasonNt);
            this.panel2.Controls.Add(this.cboHosReason);
            this.panel2.Location = new System.Drawing.Point(450, 55);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(305, 22);
            this.panel2.TabIndex = 51;
            // 
            // txtHosReasonNt
            // 
            this.txtHosReasonNt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHosReasonNt.Location = new System.Drawing.Point(0, 0);
            this.txtHosReasonNt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtHosReasonNt.Name = "txtHosReasonNt";
            this.txtHosReasonNt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.txtHosReasonNt.Size = new System.Drawing.Size(305, 22);
            this.txtHosReasonNt.TabIndex = 1;
            this.txtHosReasonNt.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtHosReasonNt_ButtonClick);
            this.txtHosReasonNt.DoubleClick += new System.EventHandler(this.txtHosReasonNt_DoubleClick);
            // 
            // cboHosReason
            // 
            this.cboHosReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboHosReason.Location = new System.Drawing.Point(0, 0);
            this.cboHosReason.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboHosReason.Name = "cboHosReason";
            this.cboHosReason.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboHosReason.Properties.AutoComplete = false;
            this.cboHosReason.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboHosReason.Properties.NullText = "";
            this.cboHosReason.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cboHosReason.Properties.View = this.customGridLookUpEdit1View;
            this.cboHosReason.Size = new System.Drawing.Size(305, 22);
            this.cboHosReason.TabIndex = 0;
            this.cboHosReason.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboHosReason_ButtonClick);
            this.cboHosReason.EditValueChanged += new System.EventHandler(this.cboHosReason_EditValueChanged);
            // 
            // customGridLookUpEdit1View
            // 
            this.customGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.customGridLookUpEdit1View.Name = "customGridLookUpEdit1View";
            this.customGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.customGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // txtHosReason
            // 
            this.txtHosReason.Location = new System.Drawing.Point(89, 55);
            this.txtHosReason.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtHosReason.Name = "txtHosReason";
            this.txtHosReason.Size = new System.Drawing.Size(280, 22);
            this.txtHosReason.StyleController = this.lcUCOtherServiceReqInfo;
            this.txtHosReason.TabIndex = 50;
            // 
            // chkExamOnline
            // 
            this.chkExamOnline.Enabled = false;
            this.chkExamOnline.Location = new System.Drawing.Point(324, 356);
            this.chkExamOnline.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkExamOnline.Name = "chkExamOnline";
            this.chkExamOnline.Properties.Caption = "Khám trực tuyến";
            this.chkExamOnline.Properties.ReadOnly = true;
            this.chkExamOnline.Size = new System.Drawing.Size(145, 21);
            this.chkExamOnline.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkExamOnline.TabIndex = 49;
            // 
            // chkIsHiv
            // 
            this.chkIsHiv.Location = new System.Drawing.Point(570, 111);
            this.chkIsHiv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkIsHiv.Name = "chkIsHiv";
            this.chkIsHiv.Properties.Caption = "BN HIV/AIDS";
            this.chkIsHiv.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkIsHiv.Size = new System.Drawing.Size(185, 21);
            this.chkIsHiv.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkIsHiv.TabIndex = 47;
            this.chkIsHiv.ToolTip = "Bệnh nhân mắc bệnh HIV/AIDS";
            this.chkIsHiv.CheckedChanged += new System.EventHandler(this.chkIsHiv_CheckedChanged);
            // 
            // chkWNext
            // 
            this.chkWNext.Location = new System.Drawing.Point(79, 356);
            this.chkWNext.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkWNext.Name = "chkWNext";
            this.chkWNext.Properties.Caption = "Cảnh báo cho lần khám sau";
            this.chkWNext.Size = new System.Drawing.Size(239, 21);
            this.chkWNext.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkWNext.TabIndex = 46;
            this.chkWNext.CheckedChanged += new System.EventHandler(this.chkWNext_CheckedChanged);
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(89, 165);
            this.txtNote.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(666, 40);
            this.txtNote.StyleController = this.lcUCOtherServiceReqInfo;
            this.txtNote.TabIndex = 45;
            // 
            // chkTuberculosis
            // 
            this.chkTuberculosis.Location = new System.Drawing.Point(480, 111);
            this.chkTuberculosis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkTuberculosis.Name = "chkTuberculosis";
            this.chkTuberculosis.Properties.Caption = "";
            this.chkTuberculosis.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.chkTuberculosis.Size = new System.Drawing.Size(84, 19);
            this.chkTuberculosis.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkTuberculosis.TabIndex = 44;
            // 
            // txtGuaranteeReason
            // 
            this.txtGuaranteeReason.Location = new System.Drawing.Point(450, 300);
            this.txtGuaranteeReason.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGuaranteeReason.Name = "txtGuaranteeReason";
            this.txtGuaranteeReason.Size = new System.Drawing.Size(305, 22);
            this.txtGuaranteeReason.StyleController = this.lcUCOtherServiceReqInfo;
            this.txtGuaranteeReason.TabIndex = 43;
            this.txtGuaranteeReason.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtGuaranteeReason_PreviewKeyDown);
            // 
            // cboGuaranteeUsername
            // 
            this.cboGuaranteeUsername.Location = new System.Drawing.Point(220, 299);
            this.cboGuaranteeUsername.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboGuaranteeUsername.Name = "cboGuaranteeUsername";
            this.cboGuaranteeUsername.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboGuaranteeUsername.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboGuaranteeUsername.Properties.ImmediatePopup = true;
            this.cboGuaranteeUsername.Properties.NullText = "";
            this.cboGuaranteeUsername.Properties.View = this.gridView3;
            this.cboGuaranteeUsername.Size = new System.Drawing.Size(150, 22);
            this.cboGuaranteeUsername.StyleController = this.lcUCOtherServiceReqInfo;
            this.cboGuaranteeUsername.TabIndex = 42;
            this.cboGuaranteeUsername.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboGuaranteeUsername_Closed);
            this.cboGuaranteeUsername.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboGuaranteeUsername_ButtonClick);
            this.cboGuaranteeUsername.EditValueChanged += new System.EventHandler(this.cboGuaranteeUsername_EditValueChanged);
            this.cboGuaranteeUsername.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cboGuaranteeUsername_PreviewKeyDown);
            // 
            // gridView3
            // 
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // txtGuaranteeLoginname
            // 
            this.txtGuaranteeLoginname.Location = new System.Drawing.Point(88, 299);
            this.txtGuaranteeLoginname.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGuaranteeLoginname.Name = "txtGuaranteeLoginname";
            this.txtGuaranteeLoginname.Size = new System.Drawing.Size(132, 22);
            this.txtGuaranteeLoginname.StyleController = this.lcUCOtherServiceReqInfo;
            this.txtGuaranteeLoginname.TabIndex = 41;
            this.txtGuaranteeLoginname.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtGuaranteeLoginname_PreviewKeyDown);
            // 
            // cboPatientClassify
            // 
            this.cboPatientClassify.Location = new System.Drawing.Point(450, 272);
            this.cboPatientClassify.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboPatientClassify.Name = "cboPatientClassify";
            this.cboPatientClassify.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboPatientClassify.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboPatientClassify.Properties.NullText = "";
            this.cboPatientClassify.Properties.View = this.gridView2;
            this.cboPatientClassify.Size = new System.Drawing.Size(305, 22);
            this.cboPatientClassify.StyleController = this.lcUCOtherServiceReqInfo;
            this.cboPatientClassify.TabIndex = 40;
            this.cboPatientClassify.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboPatientClassify_ButtonClick);
            this.cboPatientClassify.EditValueChanged += new System.EventHandler(this.cboPatientClassify_EditValueChanged);
            // 
            // gridView2
            // 
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // txtIncode
            // 
            this.txtIncode.Location = new System.Drawing.Point(437, 83);
            this.txtIncode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIncode.Name = "txtIncode";
            this.txtIncode.Size = new System.Drawing.Size(318, 22);
            this.txtIncode.StyleController = this.lcUCOtherServiceReqInfo;
            this.txtIncode.TabIndex = 5;
            this.txtIncode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtIncode_PreviewKeyDown);
            // 
            // cboOtherPaySource
            // 
            this.cboOtherPaySource.Location = new System.Drawing.Point(89, 272);
            this.cboOtherPaySource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboOtherPaySource.Name = "cboOtherPaySource";
            this.cboOtherPaySource.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboOtherPaySource.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, true)});
            this.cboOtherPaySource.Properties.NullText = "";
            this.cboOtherPaySource.Properties.View = this.gridView1;
            this.cboOtherPaySource.Size = new System.Drawing.Size(280, 22);
            this.cboOtherPaySource.StyleController = this.lcUCOtherServiceReqInfo;
            this.cboOtherPaySource.TabIndex = 15;
            this.cboOtherPaySource.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboOtherPaySource_ButtonClick);
            this.cboOtherPaySource.EditValueChanged += new System.EventHandler(this.cboOtherPaySource_EditValueChanged);
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // txtMaMS
            // 
            this.txtMaMS.Enabled = false;
            this.txtMaMS.Location = new System.Drawing.Point(665, 356);
            this.txtMaMS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMaMS.Name = "txtMaMS";
            this.txtMaMS.Properties.ReadOnly = true;
            this.txtMaMS.Size = new System.Drawing.Size(90, 22);
            this.txtMaMS.StyleController = this.lcUCOtherServiceReqInfo;
            this.txtMaMS.TabIndex = 17;
            // 
            // chkCapMaMS
            // 
            this.chkCapMaMS.Location = new System.Drawing.Point(550, 356);
            this.chkCapMaMS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCapMaMS.Name = "chkCapMaMS";
            this.chkCapMaMS.Properties.Caption = "";
            this.chkCapMaMS.Size = new System.Drawing.Size(54, 19);
            this.chkCapMaMS.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkCapMaMS.TabIndex = 16;
            // 
            // cboPriorityType
            // 
            this.cboPriorityType.Location = new System.Drawing.Point(114, 83);
            this.cboPriorityType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboPriorityType.Name = "cboPriorityType";
            this.cboPriorityType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboPriorityType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, true)});
            this.cboPriorityType.Properties.NullText = "";
            this.cboPriorityType.Size = new System.Drawing.Size(242, 22);
            this.cboPriorityType.StyleController = this.lcUCOtherServiceReqInfo;
            toolTipItem1.Text = "Trường hợp ưu tiên";
            superToolTip1.Items.Add(toolTipItem1);
            this.cboPriorityType.SuperTip = superToolTip1;
            this.cboPriorityType.TabIndex = 4;
            this.cboPriorityType.ToolTip = "Trường hợp ưu tiên";
            this.cboPriorityType.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboPriorityType_Closed);
            this.cboPriorityType.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboPriorityType_ButtonClick);
            this.cboPriorityType.EditValueChanged += new System.EventHandler(this.cboPriorityType_EditValueChanged);
            this.cboPriorityType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboPriorityType_KeyDown);
            this.cboPriorityType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cboPriorityType_KeyUp);
            this.cboPriorityType.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cboPriorityType_PreviewKeyDown);
            // 
            // txtTreatmentOrder
            // 
            this.txtTreatmentOrder.Location = new System.Drawing.Point(615, 239);
            this.txtTreatmentOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTreatmentOrder.Name = "txtTreatmentOrder";
            this.txtTreatmentOrder.Size = new System.Drawing.Size(140, 22);
            this.txtTreatmentOrder.StyleController = this.lcUCOtherServiceReqInfo;
            this.txtTreatmentOrder.TabIndex = 14;
            this.txtTreatmentOrder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTreatmentOrder_KeyPress);
            // 
            // txtSTTPriority
            // 
            this.txtSTTPriority.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtSTTPriority.Location = new System.Drawing.Point(450, 239);
            this.txtSTTPriority.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSTTPriority.Name = "txtSTTPriority";
            this.txtSTTPriority.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtSTTPriority.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtSTTPriority.Properties.MaxValue = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            0});
            this.txtSTTPriority.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtSTTPriority.Size = new System.Drawing.Size(114, 22);
            this.txtSTTPriority.StyleController = this.lcUCOtherServiceReqInfo;
            toolTipItem2.Text = "Số thứ tự ưu tiên dành cho bệnh nhân đăng ký khám qua tổng đài";
            superToolTip2.Items.Add(toolTipItem2);
            this.txtSTTPriority.SuperTip = superToolTip2;
            this.txtSTTPriority.TabIndex = 13;
            this.txtSTTPriority.EditValueChanged += new System.EventHandler(this.txtSTTPriority_EditValueChanged);
            // 
            // btnAddCTT
            // 
            this.btnAddCTT.Enabled = false;
            this.btnAddCTT.Image = ((System.Drawing.Image)(resources.GetObject("btnAddCTT.Image")));
            this.btnAddCTT.Location = new System.Drawing.Point(324, 239);
            this.btnAddCTT.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddCTT.Name = "btnAddCTT";
            this.btnAddCTT.Size = new System.Drawing.Size(45, 27);
            this.btnAddCTT.StyleController = this.lcUCOtherServiceReqInfo;
            this.btnAddCTT.TabIndex = 12;
            this.btnAddCTT.ToolTip = "Nhập thông tin bổ sung";
            this.btnAddCTT.Click += new System.EventHandler(this.btnAddCTT_Click);
            // 
            // cboCTT
            // 
            this.cboCTT.Location = new System.Drawing.Point(89, 239);
            this.cboCTT.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboCTT.Name = "cboCTT";
            this.cboCTT.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboCTT.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject9, serializableAppearanceObject10, serializableAppearanceObject11, serializableAppearanceObject12, "", null, null, true)});
            this.cboCTT.Properties.NullText = "";
            this.cboCTT.Properties.View = this.gridLookUpEdit1View;
            this.cboCTT.Size = new System.Drawing.Size(229, 22);
            this.cboCTT.StyleController = this.lcUCOtherServiceReqInfo;
            this.cboCTT.TabIndex = 11;
            this.cboCTT.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboCTT_Closed);
            this.cboCTT.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboCTT_ButtonClick);
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtIntructionTime);
            this.panel1.Controls.Add(this.dtIntructionTime);
            this.panel1.Location = new System.Drawing.Point(89, 27);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 22);
            this.panel1.TabIndex = 39;
            // 
            // txtIntructionTime
            // 
            this.txtIntructionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIntructionTime.Location = new System.Drawing.Point(0, 0);
            this.txtIntructionTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIntructionTime.Name = "txtIntructionTime";
            this.txtIntructionTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)});
            this.txtIntructionTime.Properties.Mask.EditMask = "\\d{2}/\\d{2}/\\d{4} \\d{2}:\\d{2}";
            this.txtIntructionTime.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtIntructionTime.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtIntructionTime.Size = new System.Drawing.Size(280, 22);
            this.txtIntructionTime.TabIndex = 1;
            this.txtIntructionTime.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtIntructionTime_ButtonClick);
            this.txtIntructionTime.EditValueChanged += new System.EventHandler(this.txtIntructionTime_EditValueChanged);
            this.txtIntructionTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIntructionTime_KeyDown);
            this.txtIntructionTime.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtIntructionTime_PreviewKeyDown);
            // 
            // dtIntructionTime
            // 
            this.dtIntructionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtIntructionTime.EditValue = null;
            this.dtIntructionTime.Location = new System.Drawing.Point(0, 0);
            this.dtIntructionTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtIntructionTime.Name = "dtIntructionTime";
            this.dtIntructionTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtIntructionTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtIntructionTime.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "d";
            this.dtIntructionTime.Properties.CalendarTimeProperties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtIntructionTime.Properties.CalendarTimeProperties.EditFormat.FormatString = "HH:mm";
            this.dtIntructionTime.Properties.CalendarTimeProperties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtIntructionTime.Properties.CalendarTimeProperties.Mask.EditMask = "HH:mm";
            this.dtIntructionTime.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            this.dtIntructionTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
            this.dtIntructionTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtIntructionTime.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm";
            this.dtIntructionTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtIntructionTime.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm";
            this.dtIntructionTime.Properties.NullValuePromptShowForEmptyValue = true;
            this.dtIntructionTime.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.dtIntructionTime.Size = new System.Drawing.Size(280, 22);
            this.dtIntructionTime.TabIndex = 0;
            this.dtIntructionTime.Visible = false;
            this.dtIntructionTime.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.dtIntructionTime_Closed);
            this.dtIntructionTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtIntructionTime_KeyDown);
            this.dtIntructionTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtIntructionTime_KeyPress);
            // 
            // chkIsChronic
            // 
            this.chkIsChronic.Location = new System.Drawing.Point(336, 111);
            this.chkIsChronic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkIsChronic.Name = "chkIsChronic";
            this.chkIsChronic.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
            this.chkIsChronic.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.chkIsChronic.Properties.Caption = "";
            this.chkIsChronic.Properties.FullFocusRect = true;
            this.chkIsChronic.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.chkIsChronic.Size = new System.Drawing.Size(93, 19);
            this.chkIsChronic.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkIsChronic.TabIndex = 8;
            this.chkIsChronic.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkIsChronic_KeyDown);
            // 
            // cboOweType
            // 
            this.cboOweType.Location = new System.Drawing.Point(450, 211);
            this.cboOweType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboOweType.Name = "cboOweType";
            this.cboOweType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboOweType.Properties.NullText = "";
            this.cboOweType.Size = new System.Drawing.Size(305, 22);
            this.cboOweType.StyleController = this.lcUCOtherServiceReqInfo;
            this.cboOweType.TabIndex = 10;
            this.cboOweType.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboOweType_Closed);
            // 
            // cboTreatmentType
            // 
            this.cboTreatmentType.Location = new System.Drawing.Point(450, 27);
            this.cboTreatmentType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboTreatmentType.Name = "cboTreatmentType";
            this.cboTreatmentType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboTreatmentType.Properties.NullText = "";
            this.cboTreatmentType.Size = new System.Drawing.Size(305, 22);
            this.cboTreatmentType.StyleController = this.lcUCOtherServiceReqInfo;
            this.cboTreatmentType.TabIndex = 2;
            this.cboTreatmentType.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboTreatmentType_Closed);
            this.cboTreatmentType.EditValueChanged += new System.EventHandler(this.cboTreatmentType_EditValueChanged);
            // 
            // chkIsNotRequireFee
            // 
            this.chkIsNotRequireFee.Location = new System.Drawing.Point(176, 111);
            this.chkIsNotRequireFee.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkIsNotRequireFee.Name = "chkIsNotRequireFee";
            this.chkIsNotRequireFee.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
            this.chkIsNotRequireFee.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.chkIsNotRequireFee.Properties.Caption = "";
            this.chkIsNotRequireFee.Properties.FullFocusRect = true;
            this.chkIsNotRequireFee.Size = new System.Drawing.Size(92, 19);
            this.chkIsNotRequireFee.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkIsNotRequireFee.TabIndex = 7;
            this.chkIsNotRequireFee.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.chkIsNotRequireFee_PreviewKeyDown);
            // 
            // chkPriority
            // 
            this.chkPriority.Location = new System.Drawing.Point(76, 83);
            this.chkPriority.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkPriority.Name = "chkPriority";
            this.chkPriority.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkPriority.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chkPriority.Properties.Appearance.Options.UseFont = true;
            this.chkPriority.Properties.Appearance.Options.UseForeColor = true;
            this.chkPriority.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
            this.chkPriority.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.chkPriority.Properties.Caption = "";
            this.chkPriority.Properties.FullFocusRect = true;
            this.chkPriority.Size = new System.Drawing.Size(32, 19);
            this.chkPriority.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkPriority.TabIndex = 3;
            this.chkPriority.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.chkPriority_PreviewKeyDown);
            // 
            // chkEmergency
            // 
            this.chkEmergency.Location = new System.Drawing.Point(76, 111);
            this.chkEmergency.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkEmergency.Name = "chkEmergency";
            this.chkEmergency.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
            this.chkEmergency.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.chkEmergency.Properties.Caption = "";
            this.chkEmergency.Properties.FullFocusRect = true;
            this.chkEmergency.Size = new System.Drawing.Size(32, 19);
            this.chkEmergency.StyleController = this.lcUCOtherServiceReqInfo;
            this.chkEmergency.TabIndex = 6;
            this.chkEmergency.EditValueChanged += new System.EventHandler(this.chkEmergency_EditValueChanged);
            this.chkEmergency.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.chkEmergency_PreviewKeyDown);
            // 
            // cboEmergencyTime
            // 
            this.cboEmergencyTime.Location = new System.Drawing.Point(89, 211);
            this.cboEmergencyTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboEmergencyTime.Name = "cboEmergencyTime";
            this.cboEmergencyTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboEmergencyTime.Properties.NullText = "";
            this.cboEmergencyTime.Size = new System.Drawing.Size(280, 22);
            this.cboEmergencyTime.StyleController = this.lcUCOtherServiceReqInfo;
            this.cboEmergencyTime.TabIndex = 9;
            this.cboEmergencyTime.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboEmergencyTime_Closed);
            // 
            // lcgOtherRequest
            // 
            this.lcgOtherRequest.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.lcgOtherRequest.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciTreatmentType,
            this.lciIsNotRequireFee,
            this.lciOweType,
            this.lciEmergency,
            this.lciIntructionTime,
            this.lciFortxtMaMS,
            this.lciForchkCapMaMS,
            this.lciPriority,
            this.layoutControlItem1,
            this.lciFortxtIncode,
            this.lciPatientClassify,
            this.lciTuberculosis,
            this.lciEmergencyTime,
            this.lciCboCTT,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.lciGuaranteeLoginname,
            this.lciGuaranteeUsername,
            this.lciIsChronic,
            this.lciFortxtSTTPriority,
            this.lciTreatmentOrder,
            this.lciGuaranteeReason,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.lciHosReason,
            this.emptySpaceItem1,
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem13,
            this.layoutControlItem12,
            this.emptySpaceItem2,
            this.layoutControlItem4});
            this.lcgOtherRequest.Location = new System.Drawing.Point(0, 0);
            this.lcgOtherRequest.Name = "lcgOtherRequest";
            this.lcgOtherRequest.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lcgOtherRequest.Size = new System.Drawing.Size(759, 382);
            this.lcgOtherRequest.Text = "Yêu cầu khác (F6)";
            // 
            // lciTreatmentType
            // 
            this.lciTreatmentType.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
            this.lciTreatmentType.AppearanceItemCaption.Options.UseForeColor = true;
            this.lciTreatmentType.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciTreatmentType.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciTreatmentType.Control = this.cboTreatmentType;
            this.lciTreatmentType.Location = new System.Drawing.Point(371, 0);
            this.lciTreatmentType.Name = "lciTreatmentType";
            this.lciTreatmentType.OptionsToolTip.ToolTip = "Diện điều trị";
            this.lciTreatmentType.Size = new System.Drawing.Size(386, 28);
            this.lciTreatmentType.Text = "Diện ĐT:";
            this.lciTreatmentType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciTreatmentType.TextSize = new System.Drawing.Size(70, 20);
            this.lciTreatmentType.TextToControlDistance = 5;
            // 
            // lciIsNotRequireFee
            // 
            this.lciIsNotRequireFee.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciIsNotRequireFee.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciIsNotRequireFee.Control = this.chkIsNotRequireFee;
            this.lciIsNotRequireFee.Location = new System.Drawing.Point(110, 84);
            this.lciIsNotRequireFee.Name = "lciIsNotRequireFee";
            this.lciIsNotRequireFee.OptionsToolTip.ToolTip = "Khám thu sau";
            this.lciIsNotRequireFee.Size = new System.Drawing.Size(160, 27);
            this.lciIsNotRequireFee.Text = "Thu sau:";
            this.lciIsNotRequireFee.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciIsNotRequireFee.TextSize = new System.Drawing.Size(60, 20);
            this.lciIsNotRequireFee.TextToControlDistance = 2;
            // 
            // lciOweType
            // 
            this.lciOweType.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciOweType.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciOweType.Control = this.cboOweType;
            this.lciOweType.Location = new System.Drawing.Point(371, 184);
            this.lciOweType.Name = "lciOweType";
            this.lciOweType.OptionsToolTip.ToolTip = "Nợ viện phí";
            this.lciOweType.Size = new System.Drawing.Size(386, 28);
            this.lciOweType.Text = "Nợ VP:";
            this.lciOweType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciOweType.TextSize = new System.Drawing.Size(70, 20);
            this.lciOweType.TextToControlDistance = 5;
            // 
            // lciEmergency
            // 
            this.lciEmergency.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciEmergency.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciEmergency.Control = this.chkEmergency;
            this.lciEmergency.Location = new System.Drawing.Point(0, 84);
            this.lciEmergency.MaxSize = new System.Drawing.Size(110, 24);
            this.lciEmergency.MinSize = new System.Drawing.Size(95, 24);
            this.lciEmergency.Name = "lciEmergency";
            this.lciEmergency.Size = new System.Drawing.Size(110, 27);
            this.lciEmergency.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciEmergency.Text = "Cấp cứu:";
            this.lciEmergency.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciEmergency.TextSize = new System.Drawing.Size(70, 20);
            this.lciEmergency.TextToControlDistance = 2;
            // 
            // lciIntructionTime
            // 
            this.lciIntructionTime.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
            this.lciIntructionTime.AppearanceItemCaption.Options.UseForeColor = true;
            this.lciIntructionTime.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciIntructionTime.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciIntructionTime.Control = this.panel1;
            this.lciIntructionTime.Location = new System.Drawing.Point(0, 0);
            this.lciIntructionTime.Name = "lciIntructionTime";
            this.lciIntructionTime.Size = new System.Drawing.Size(371, 28);
            this.lciIntructionTime.Text = "Thời gian:";
            this.lciIntructionTime.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciIntructionTime.TextSize = new System.Drawing.Size(80, 20);
            this.lciIntructionTime.TextToControlDistance = 5;
            // 
            // lciFortxtMaMS
            // 
            this.lciFortxtMaMS.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciFortxtMaMS.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciFortxtMaMS.Control = this.txtMaMS;
            this.lciFortxtMaMS.Location = new System.Drawing.Point(606, 329);
            this.lciFortxtMaMS.Name = "lciFortxtMaMS";
            this.lciFortxtMaMS.Size = new System.Drawing.Size(151, 28);
            this.lciFortxtMaMS.Text = "Mã MS:";
            this.lciFortxtMaMS.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciFortxtMaMS.TextSize = new System.Drawing.Size(50, 20);
            this.lciFortxtMaMS.TextToControlDistance = 5;
            this.lciFortxtMaMS.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lciForchkCapMaMS
            // 
            this.lciForchkCapMaMS.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciForchkCapMaMS.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciForchkCapMaMS.Control = this.chkCapMaMS;
            this.lciForchkCapMaMS.Location = new System.Drawing.Point(471, 329);
            this.lciForchkCapMaMS.Name = "lciForchkCapMaMS";
            this.lciForchkCapMaMS.Size = new System.Drawing.Size(135, 28);
            this.lciForchkCapMaMS.Text = "Cấp mã MS:";
            this.lciForchkCapMaMS.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciForchkCapMaMS.TextSize = new System.Drawing.Size(70, 20);
            this.lciForchkCapMaMS.TextToControlDistance = 5;
            this.lciForchkCapMaMS.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lciPriority
            // 
            this.lciPriority.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciPriority.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciPriority.Control = this.chkPriority;
            this.lciPriority.Location = new System.Drawing.Point(0, 56);
            this.lciPriority.MaxSize = new System.Drawing.Size(110, 24);
            this.lciPriority.MinSize = new System.Drawing.Size(95, 24);
            this.lciPriority.Name = "lciPriority";
            this.lciPriority.Size = new System.Drawing.Size(110, 28);
            this.lciPriority.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciPriority.Text = "Ưu tiên:";
            this.lciPriority.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciPriority.TextSize = new System.Drawing.Size(70, 20);
            this.lciPriority.TextToControlDistance = 2;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.cboPriorityType;
            this.layoutControlItem1.Location = new System.Drawing.Point(110, 56);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsToolTip.ToolTip = "Trường hợp ưu tiên";
            this.layoutControlItem1.Size = new System.Drawing.Size(248, 28);
            this.layoutControlItem1.Text = "TH ưu tiên:";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // lciFortxtIncode
            // 
            this.lciFortxtIncode.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciFortxtIncode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciFortxtIncode.Control = this.txtIncode;
            this.lciFortxtIncode.Location = new System.Drawing.Point(358, 56);
            this.lciFortxtIncode.Name = "lciFortxtIncode";
            this.lciFortxtIncode.Size = new System.Drawing.Size(399, 28);
            this.lciFortxtIncode.Text = "Số vào viện:";
            this.lciFortxtIncode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciFortxtIncode.TextSize = new System.Drawing.Size(70, 20);
            this.lciFortxtIncode.TextToControlDistance = 5;
            this.lciFortxtIncode.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lciPatientClassify
            // 
            this.lciPatientClassify.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciPatientClassify.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciPatientClassify.Control = this.cboPatientClassify;
            this.lciPatientClassify.Location = new System.Drawing.Point(371, 245);
            this.lciPatientClassify.Name = "lciPatientClassify";
            this.lciPatientClassify.OptionsToolTip.ToolTip = "Phân loại bệnh nhân";
            this.lciPatientClassify.Size = new System.Drawing.Size(386, 28);
            this.lciPatientClassify.Text = "Phân loại BN:";
            this.lciPatientClassify.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciPatientClassify.TextSize = new System.Drawing.Size(70, 20);
            this.lciPatientClassify.TextToControlDistance = 5;
            // 
            // lciTuberculosis
            // 
            this.lciTuberculosis.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciTuberculosis.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciTuberculosis.Control = this.chkTuberculosis;
            this.lciTuberculosis.Location = new System.Drawing.Point(431, 84);
            this.lciTuberculosis.Name = "lciTuberculosis";
            this.lciTuberculosis.OptionsToolTip.ToolTip = "Bệnh nhân thuộc chương trình phòng chống Lao quốc gia";
            this.lciTuberculosis.Size = new System.Drawing.Size(135, 27);
            this.lciTuberculosis.Text = "BN lao:";
            this.lciTuberculosis.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciTuberculosis.TextSize = new System.Drawing.Size(40, 20);
            this.lciTuberculosis.TextToControlDistance = 5;
            // 
            // lciEmergencyTime
            // 
            this.lciEmergencyTime.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciEmergencyTime.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciEmergencyTime.Control = this.cboEmergencyTime;
            this.lciEmergencyTime.Enabled = false;
            this.lciEmergencyTime.Location = new System.Drawing.Point(0, 184);
            this.lciEmergencyTime.Name = "lciEmergencyTime";
            this.lciEmergencyTime.OptionsToolTip.ToolTip = "Thời gian đau";
            this.lciEmergencyTime.Size = new System.Drawing.Size(371, 28);
            this.lciEmergencyTime.Text = "TG đau:";
            this.lciEmergencyTime.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciEmergencyTime.TextSize = new System.Drawing.Size(80, 13);
            this.lciEmergencyTime.TextToControlDistance = 5;
            // 
            // lciCboCTT
            // 
            this.lciCboCTT.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciCboCTT.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciCboCTT.Control = this.cboCTT;
            this.lciCboCTT.Location = new System.Drawing.Point(0, 212);
            this.lciCboCTT.Name = "lciCboCTT";
            this.lciCboCTT.OptionsToolTip.ToolTip = "Đơn vị cùng chi trả viện phí";
            this.lciCboCTT.Size = new System.Drawing.Size(320, 33);
            this.lciCboCTT.Text = "Đơn vị CCT:";
            this.lciCboCTT.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciCboCTT.TextSize = new System.Drawing.Size(80, 20);
            this.lciCboCTT.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnAddCTT;
            this.layoutControlItem2.Location = new System.Drawing.Point(320, 212);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(51, 33);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem3.Control = this.cboOtherPaySource;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 245);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.OptionsToolTip.ToolTip = "Nguồn chi trả khác";
            this.layoutControlItem3.Size = new System.Drawing.Size(371, 28);
            this.layoutControlItem3.Text = "Nguồn CTK:";
            this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(80, 20);
            this.layoutControlItem3.TextToControlDistance = 5;
            // 
            // lciGuaranteeLoginname
            // 
            this.lciGuaranteeLoginname.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
            this.lciGuaranteeLoginname.AppearanceItemCaption.Options.UseForeColor = true;
            this.lciGuaranteeLoginname.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciGuaranteeLoginname.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciGuaranteeLoginname.Control = this.txtGuaranteeLoginname;
            this.lciGuaranteeLoginname.Location = new System.Drawing.Point(0, 273);
            this.lciGuaranteeLoginname.MaxSize = new System.Drawing.Size(0, 24);
            this.lciGuaranteeLoginname.MinSize = new System.Drawing.Size(80, 24);
            this.lciGuaranteeLoginname.Name = "lciGuaranteeLoginname";
            this.lciGuaranteeLoginname.OptionsToolTip.ToolTip = "Người bảo lãnh";
            this.lciGuaranteeLoginname.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
            this.lciGuaranteeLoginname.Size = new System.Drawing.Size(219, 28);
            this.lciGuaranteeLoginname.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciGuaranteeLoginname.Text = "Bảo lãnh:";
            this.lciGuaranteeLoginname.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciGuaranteeLoginname.TextSize = new System.Drawing.Size(80, 20);
            this.lciGuaranteeLoginname.TextToControlDistance = 5;
            // 
            // lciGuaranteeUsername
            // 
            this.lciGuaranteeUsername.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciGuaranteeUsername.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciGuaranteeUsername.Control = this.cboGuaranteeUsername;
            this.lciGuaranteeUsername.Location = new System.Drawing.Point(219, 273);
            this.lciGuaranteeUsername.Name = "lciGuaranteeUsername";
            this.lciGuaranteeUsername.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
            this.lciGuaranteeUsername.Size = new System.Drawing.Size(152, 28);
            this.lciGuaranteeUsername.TextSize = new System.Drawing.Size(0, 0);
            this.lciGuaranteeUsername.TextVisible = false;
            // 
            // lciIsChronic
            // 
            this.lciIsChronic.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciIsChronic.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciIsChronic.Control = this.chkIsChronic;
            this.lciIsChronic.Location = new System.Drawing.Point(270, 84);
            this.lciIsChronic.Name = "lciIsChronic";
            this.lciIsChronic.Size = new System.Drawing.Size(161, 27);
            this.lciIsChronic.Text = "Mãn tính:";
            this.lciIsChronic.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciIsChronic.TextSize = new System.Drawing.Size(60, 20);
            this.lciIsChronic.TextToControlDistance = 2;
            // 
            // lciFortxtSTTPriority
            // 
            this.lciFortxtSTTPriority.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciFortxtSTTPriority.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciFortxtSTTPriority.Control = this.txtSTTPriority;
            this.lciFortxtSTTPriority.Location = new System.Drawing.Point(371, 212);
            this.lciFortxtSTTPriority.MaxSize = new System.Drawing.Size(0, 24);
            this.lciFortxtSTTPriority.MinSize = new System.Drawing.Size(108, 24);
            this.lciFortxtSTTPriority.Name = "lciFortxtSTTPriority";
            this.lciFortxtSTTPriority.OptionsToolTip.ToolTip = "Số thứ tự ưu tiên dành cho bệnh nhân đăng ký khám qua tổng đài";
            this.lciFortxtSTTPriority.Size = new System.Drawing.Size(195, 33);
            this.lciFortxtSTTPriority.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciFortxtSTTPriority.Text = "STT ƯT:";
            this.lciFortxtSTTPriority.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciFortxtSTTPriority.TextSize = new System.Drawing.Size(70, 20);
            this.lciFortxtSTTPriority.TextToControlDistance = 5;
            // 
            // lciTreatmentOrder
            // 
            this.lciTreatmentOrder.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciTreatmentOrder.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciTreatmentOrder.Control = this.txtTreatmentOrder;
            this.lciTreatmentOrder.Location = new System.Drawing.Point(566, 212);
            this.lciTreatmentOrder.MaxSize = new System.Drawing.Size(0, 24);
            this.lciTreatmentOrder.MinSize = new System.Drawing.Size(90, 24);
            this.lciTreatmentOrder.Name = "lciTreatmentOrder";
            this.lciTreatmentOrder.OptionsToolTip.ToolTip = "Số thứ tự hồ sơ";
            this.lciTreatmentOrder.Size = new System.Drawing.Size(191, 33);
            this.lciTreatmentOrder.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lciTreatmentOrder.Text = "STT HS:";
            this.lciTreatmentOrder.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciTreatmentOrder.TextSize = new System.Drawing.Size(40, 20);
            this.lciTreatmentOrder.TextToControlDistance = 5;
            // 
            // lciGuaranteeReason
            // 
            this.lciGuaranteeReason.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciGuaranteeReason.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciGuaranteeReason.Control = this.txtGuaranteeReason;
            this.lciGuaranteeReason.Location = new System.Drawing.Point(371, 273);
            this.lciGuaranteeReason.Name = "lciGuaranteeReason";
            this.lciGuaranteeReason.OptionsToolTip.ToolTip = "Lý do bảo lãnh";
            this.lciGuaranteeReason.Size = new System.Drawing.Size(386, 28);
            this.lciGuaranteeReason.Text = "Lý do:";
            this.lciGuaranteeReason.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciGuaranteeReason.TextSize = new System.Drawing.Size(70, 20);
            this.lciGuaranteeReason.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem4.Control = this.txtNote;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 138);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(757, 46);
            this.layoutControlItem4.Text = "Ghi chú:";
            this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(80, 20);
            this.layoutControlItem4.TextToControlDistance = 5;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.chkWNext;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 329);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(320, 28);
            this.layoutControlItem5.Text = " ";
            this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(70, 20);
            this.layoutControlItem5.TextToControlDistance = 5;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.chkIsHiv;
            this.layoutControlItem6.Location = new System.Drawing.Point(566, 84);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(191, 27);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.chkExamOnline;
            this.layoutControlItem7.Location = new System.Drawing.Point(320, 329);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(151, 28);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
            this.layoutControlItem8.AppearanceItemCaption.Options.UseForeColor = true;
            this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem8.Control = this.txtHosReason;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 28);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.OptionsToolTip.ToolTip = "Lý do vào viện";
            this.layoutControlItem8.Size = new System.Drawing.Size(371, 28);
            this.layoutControlItem8.Text = "Lý do VV:";
            this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(80, 20);
            this.layoutControlItem8.TextToControlDistance = 5;
            this.layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lciHosReason
            // 
            this.lciHosReason.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciHosReason.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciHosReason.Control = this.panel2;
            this.lciHosReason.Location = new System.Drawing.Point(371, 28);
            this.lciHosReason.Name = "lciHosReason";
            this.lciHosReason.OptionsToolTip.ToolTip = "Lý do vào nội trú";
            this.lciHosReason.Size = new System.Drawing.Size(386, 28);
            this.lciHosReason.Text = "Lý do vào NT:";
            this.lciHosReason.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciHosReason.TextSize = new System.Drawing.Size(70, 20);
            this.lciHosReason.TextToControlDistance = 5;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(642, 111);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(13, 27);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.chkCAPD;
            this.layoutControlItem9.Location = new System.Drawing.Point(655, 111);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(102, 27);
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem10.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem10.Control = this.txtNguonKhach;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 301);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
            this.layoutControlItem10.Size = new System.Drawing.Size(212, 28);
            this.layoutControlItem10.Text = "Nguồn khách:";
            this.layoutControlItem10.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem10.TextSize = new System.Drawing.Size(80, 20);
            this.layoutControlItem10.TextToControlDistance = 5;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.cboNguonKhach;
            this.layoutControlItem11.Location = new System.Drawing.Point(212, 301);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
            this.layoutControlItem11.Size = new System.Drawing.Size(159, 28);
            this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem11.TextVisible = false;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem13.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem13.Control = this.cboNguonKhachCT;
            this.layoutControlItem13.Location = new System.Drawing.Point(371, 301);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.OptionsToolTip.ToolTip = "Nguồn khách chi tiết";
            this.layoutControlItem13.Size = new System.Drawing.Size(386, 28);
            this.layoutControlItem13.Text = "NK chi tiết:";
            this.layoutControlItem13.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem13.TextSize = new System.Drawing.Size(70, 20);
            this.layoutControlItem13.TextToControlDistance = 5;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem12.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem12.Control = this.chkChamSocDa;
            this.layoutControlItem12.Location = new System.Drawing.Point(97, 111);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(545, 27);
            this.layoutControlItem12.Text = "Hướng dẫn chăm sóc da:";
            this.layoutControlItem12.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem12.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem12.TextToControlDistance = 0;
            this.layoutControlItem12.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 111);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(97, 27);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // dxErrorProviderControl
            // 
            this.dxErrorProviderControl.ContainerControl = this;
            // 
            // timerInitForm
            // 
            this.timerInitForm.Interval = 500;
            // 
            // UCOtherServiceReqInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lcUCOtherServiceReqInfo);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UCOtherServiceReqInfo";
            this.Size = new System.Drawing.Size(759, 382);
            this.Load += new System.EventHandler(this.UCOtherServiceReqInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lcUCOtherServiceReqInfo)).EndInit();
            this.lcUCOtherServiceReqInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkChamSocDa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboNguonKhachCT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboNguonKhach.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNguonKhach.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCAPD.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtHosReasonNt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboHosReason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHosReason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkExamOnline.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsHiv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWNext.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTuberculosis.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGuaranteeReason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboGuaranteeUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGuaranteeLoginname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPatientClassify.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIncode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOtherPaySource.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaMS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCapMaMS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPriorityType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentOrder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSTTPriority.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCTT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtIntructionTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtIntructionTime.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtIntructionTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsChronic.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOweType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTreatmentType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsNotRequireFee.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPriority.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEmergency.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEmergencyTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcgOtherRequest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIsNotRequireFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOweType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEmergency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIntructionTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFortxtMaMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciForchkCapMaMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFortxtIncode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPatientClassify)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTuberculosis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEmergencyTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciCboCTT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGuaranteeLoginname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGuaranteeUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciIsChronic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFortxtSTTPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciGuaranteeReason)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciHosReason)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationUCOtherReqInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProviderControl)).EndInit();
            this.ResumeLayout(false);

		}

		public void DisposeControl()
		{
			try
			{
				dataClassify = null;
				dataOtherPayTemp = null;
				IsChangeFromClassify = false;
				hasDataAutoCheckPriority = false;
				_IsAutoSetOweType = false;
				_BranchTimes = null;
				_IsUserBranchTime = false;
				workingPatientType = null;
				_HisTreatment = null;
				dlgPriorityNumberChanged = null;
				dlgFocusNextUserControl = null;
				_PatientName = null;
				txtGuaranteeReason.PreviewKeyDown -= txtGuaranteeReason_PreviewKeyDown;
				cboGuaranteeUsername.Closed -= cboGuaranteeUsername_Closed;
				cboGuaranteeUsername.ButtonClick -= cboGuaranteeUsername_ButtonClick;
				cboGuaranteeUsername.EditValueChanged -= cboGuaranteeUsername_EditValueChanged;
				cboGuaranteeUsername.PreviewKeyDown -= cboGuaranteeUsername_PreviewKeyDown;
				txtGuaranteeLoginname.PreviewKeyDown -= txtGuaranteeLoginname_PreviewKeyDown;
				cboPatientClassify.ButtonClick -= cboPatientClassify_ButtonClick;
				cboPatientClassify.EditValueChanged -= cboPatientClassify_EditValueChanged;
				txtIncode.PreviewKeyDown -= txtIncode_PreviewKeyDown;
				cboOtherPaySource.ButtonClick -= cboOtherPaySource_ButtonClick;
				cboOtherPaySource.EditValueChanged -= cboOtherPaySource_EditValueChanged;
				cboPriorityType.Closed -= cboPriorityType_Closed;
				cboPriorityType.ButtonClick -= cboPriorityType_ButtonClick;
				cboPriorityType.EditValueChanged -= cboPriorityType_EditValueChanged;
				cboPriorityType.KeyDown -= cboPriorityType_KeyDown;
				cboPriorityType.KeyUp -= cboPriorityType_KeyUp;
				cboPriorityType.PreviewKeyDown -= cboPriorityType_PreviewKeyDown;
				txtTreatmentOrder.KeyPress -= txtTreatmentOrder_KeyPress;
				txtSTTPriority.EditValueChanged -= txtSTTPriority_EditValueChanged;
				btnAddCTT.Click -= btnAddCTT_Click;
				cboCTT.Closed -= cboCTT_Closed;
				cboCTT.ButtonClick -= cboCTT_ButtonClick;
				txtIntructionTime.ButtonClick -= txtIntructionTime_ButtonClick;
				txtIntructionTime.EditValueChanged -= txtIntructionTime_EditValueChanged;
				txtIntructionTime.KeyDown -= txtIntructionTime_KeyDown;
				txtIntructionTime.PreviewKeyDown -= txtIntructionTime_PreviewKeyDown;
				dtIntructionTime.Closed -= dtIntructionTime_Closed;
				dtIntructionTime.KeyDown -= dtIntructionTime_KeyDown;
				dtIntructionTime.KeyPress -= dtIntructionTime_KeyPress;
				chkIsChronic.KeyDown -= chkIsChronic_KeyDown;
				cboOweType.Closed -= cboOweType_Closed;
				cboTreatmentType.Closed -= cboTreatmentType_Closed;
				cboTreatmentType.EditValueChanged -= cboTreatmentType_EditValueChanged;
				chkIsNotRequireFee.PreviewKeyDown -= chkIsNotRequireFee_PreviewKeyDown;
				chkPriority.PreviewKeyDown -= chkPriority_PreviewKeyDown;
				chkEmergency.EditValueChanged -= chkEmergency_EditValueChanged;
				chkEmergency.PreviewKeyDown -= chkEmergency_PreviewKeyDown;
				cboEmergencyTime.Closed -= cboEmergencyTime_Closed;
				base.Load -= UCOtherServiceReqInfo_Load;
				gridView3.GridControl.DataSource = null;
				gridView2.GridControl.DataSource = null;
				gridView1.GridControl.DataSource = null;
				gridLookUpEdit1View.GridControl.DataSource = null;
				layoutControlItem4 = null;
				txtNote = null;
				toolTipItem2 = null;
				toolTipItem1 = null;
				lciTuberculosis = null;
				chkTuberculosis = null;
				lciGuaranteeReason = null;
				txtGuaranteeReason = null;
				lciGuaranteeUsername = null;
				gridView3 = null;
				cboGuaranteeUsername = null;
				lciGuaranteeLoginname = null;
				txtGuaranteeLoginname = null;
				lciPatientClassify = null;
				gridView2 = null;
				cboPatientClassify = null;
				lciFortxtIncode = null;
				txtIncode = null;
				layoutControlItem3 = null;
				gridView1 = null;
				cboOtherPaySource = null;
				lciFortxtMaMS = null;
				lciForchkCapMaMS = null;
				chkCapMaMS = null;
				txtMaMS = null;
				layoutControlItem1 = null;
				cboPriorityType = null;
				lciTreatmentOrder = null;
				txtTreatmentOrder = null;
				timerInitForm = null;
				lciFortxtSTTPriority = null;
				txtSTTPriority = null;
				layoutControlItem2 = null;
				lciCboCTT = null;
				gridLookUpEdit1View = null;
				cboCTT = null;
				btnAddCTT = null;
				dxErrorProviderControl = null;
				dxValidationUCOtherReqInfo = null;
				dtIntructionTime = null;
				txtIntructionTime = null;
				lciIntructionTime = null;
				panel1 = null;
				lciEmergencyTime = null;
				lciEmergency = null;
				lciPriority = null;
				lciIsNotRequireFee = null;
				lciTreatmentType = null;
				lciOweType = null;
				lciIsChronic = null;
				cboEmergencyTime = null;
				chkEmergency = null;
				chkPriority = null;
				chkIsNotRequireFee = null;
				cboTreatmentType = null;
				cboOweType = null;
				chkIsChronic = null;
				lcgOtherRequest = null;
				lcUCOtherServiceReqInfo = null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public UCServiceReqInfoADO GetValue()
		{
			UCServiceReqInfoADO dataServiceReqInfoADO = new UCServiceReqInfoADO();
			try
			{
				dataServiceReqInfoADO.HospitalizationReason = ((!string.IsNullOrEmpty(txtHosReason.Text.Trim())) ? txtHosReason.Text.Trim() : null);
				dataServiceReqInfoADO.IntructionTime = Parse.ToInt64(DateTimeHelper.ConvertDateTimeStringToSystemTime(txtIntructionTime.Text).Value.ToString("yyyyMMddHHmm") + "00");
				dataServiceReqInfoADO.IsCAPD = chkCAPD.Checked;
				dataServiceReqInfoADO.IS_CAPD = (chkCAPD.Checked ? new short?(1) : ((short?)null));
				dataServiceReqInfoADO.NguonKhachCode = txtNguonKhach.Text.Trim();
				dataServiceReqInfoADO.NguonKhachName = cboNguonKhach.Text;
				if (lstOtherDetail != null && lstOtherDetail.Count > 0)
				{
					dataServiceReqInfoADO.NguonKhachCTName = string.Join(",", lstOtherDetail.Select((HIS_CUSTOMER_SOURCE_DT e) => e.LOGINNAME));
				}
				if (chkCapMaMS.Checked)
				{
					dataServiceReqInfoADO.IsCapMaMS = true;
				}
				else
				{
					dataServiceReqInfoADO.IsCapMaMS = false;
				}
				if (chkEmergency.Checked)
				{
					dataServiceReqInfoADO.IsEmergency = true;
				}
				else
				{
					dataServiceReqInfoADO.IsEmergency = false;
				}
				if (chkIsChronic.Checked)
				{
					dataServiceReqInfoADO.IsChronic = true;
				}
				else
				{
					dataServiceReqInfoADO.IsChronic = false;
				}
				if (chkIsNotRequireFee.Checked)
				{
					dataServiceReqInfoADO.IsNotRequireFee = true;
				}
				else
				{
					dataServiceReqInfoADO.IsNotRequireFee = false;
				}
				if (chkPriority.Checked)
				{
					dataServiceReqInfoADO.IsPriority = true;
				}
				else
				{
					dataServiceReqInfoADO.IsPriority = false;
				}
				if (chkChamSocDa.Checked)
				{
					dataServiceReqInfoADO.isChamSocDa = true;
				}
				else
				{
					dataServiceReqInfoADO.isChamSocDa = false;
				}
				if (cboOweType.EditValue != null)
				{
					dataServiceReqInfoADO.OweType_ID = (long)cboOweType.EditValue;
					HIS_OWE_TYPE val = BackendDataWorker.Get<HIS_OWE_TYPE>().FirstOrDefault((HIS_OWE_TYPE p) => p.IS_ACTIVE == 1 && p.ID == dataServiceReqInfoADO.OweType_ID);
					if (val == null || val.ID <= 0)
					{
						dataServiceReqInfoADO.OweType_ID = 0L;
					}
				}
				if (cboTreatmentType.EditValue != null)
				{
					dataServiceReqInfoADO.TreatmentType_ID = (long)cboTreatmentType.EditValue;
				}
				if (cboOtherPaySource.EditValue != null)
				{
					dataServiceReqInfoADO.OTHER_PAY_SOURCE_ID = (long)cboOtherPaySource.EditValue;
				}
				if (cboPriorityType.EditValue != null)
				{
					dataServiceReqInfoADO.PriorityType = (long)cboPriorityType.EditValue;
				}
				else
				{
					dataServiceReqInfoADO.PriorityType = null;
				}
				if (cboEmergencyTime.EditValue != null)
				{
					dataServiceReqInfoADO.EmergencyTime_ID = (long)cboEmergencyTime.EditValue;
				}
				if (txtSTTPriority.EditValue != null)
				{
					dataServiceReqInfoADO.PriorityNumber = (long)txtSTTPriority.Value;
				}
				if (cboCTT.EditValue != null && _HisTreatment != null)
				{
					dataServiceReqInfoADO.FUND_ID = (long)cboCTT.EditValue;
					decimal? fUND_BUDGET = _HisTreatment.FUND_BUDGET;
					if ((fUND_BUDGET.GetValueOrDefault() > default(decimal)) & fUND_BUDGET.HasValue)
					{
						dataServiceReqInfoADO.FUND_BUDGET = _HisTreatment.FUND_BUDGET.GetValueOrDefault();
					}
					else
					{
						dataServiceReqInfoADO.FUND_BUDGET = null;
					}
					dataServiceReqInfoADO.FUND_COMPANY_NAME = _HisTreatment.FUND_COMPANY_NAME;
					if (_HisTreatment.FUND_FROM_TIME > 0)
					{
						dataServiceReqInfoADO.FUND_FROM_TIME = _HisTreatment.FUND_FROM_TIME.GetValueOrDefault();
					}
					else
					{
						dataServiceReqInfoADO.FUND_FROM_TIME = null;
					}
					if (_HisTreatment.FUND_TO_TIME > 0)
					{
						dataServiceReqInfoADO.FUND_TO_TIME = _HisTreatment.FUND_TO_TIME.GetValueOrDefault();
					}
					else
					{
						dataServiceReqInfoADO.FUND_TO_TIME = null;
					}
					dataServiceReqInfoADO.FUND_ISSUE_TIME = _HisTreatment.FUND_ISSUE_TIME;
					dataServiceReqInfoADO.FUND_NUMBER = _HisTreatment.FUND_NUMBER;
					dataServiceReqInfoADO.FUND_TYPE_NAME = _HisTreatment.FUND_TYPE_NAME;
					dataServiceReqInfoADO.FUND_CUSTOMER_NAME = _HisTreatment.FUND_CUSTOMER_NAME;
				}
				dataServiceReqInfoADO.IN_CODE = txtIncode.Text;
				dataServiceReqInfoADO.MaMS = txtMaMS.Text;
				if (!string.IsNullOrWhiteSpace(txtTreatmentOrder.Text))
				{
					dataServiceReqInfoADO.TreatmentOrder = System.Convert.ToInt64(txtTreatmentOrder.Text.Trim());
				}
				else
				{
					dataServiceReqInfoADO.TreatmentOrder = null;
				}
				if (cboPatientClassify.EditValue != null)
				{
					dataServiceReqInfoADO.PATIENT_CLASSIFY_ID = Parse.ToInt64(cboPatientClassify.EditValue.ToString());
				}
				if (cboGuaranteeUsername.EditValue != null)
				{
					dataServiceReqInfoADO.GUARANTEE_LOGINNAME = txtGuaranteeLoginname.Text.Trim();
					dataServiceReqInfoADO.GUARANTEE_USERNAME = cboGuaranteeUsername.Text.Trim();
				}
				dataServiceReqInfoADO.GUARANTEE_REASON = txtGuaranteeReason.Text.Trim();
				dataServiceReqInfoADO.NOTE = txtNote.Text.Trim();
				dataServiceReqInfoADO.IsTuberCulosis = chkTuberculosis.Checked;
				dataServiceReqInfoADO.IsWarningForNext = chkWNext.Checked;
				dataServiceReqInfoADO.IsHiv = chkIsHiv.Checked;
				if (cboHosReason.EditValue != null)
				{
					HIS_HOSPITALIZE_REASON val2 = (from o in BackendDataWorker.Get<HIS_HOSPITALIZE_REASON>()
						where o.IS_ACTIVE == 1
						select o).ToList().FirstOrDefault((HIS_HOSPITALIZE_REASON o) => o.ID == long.Parse(cboHosReason.EditValue.ToString()));
					if (val2 != null && val2.HOSPITALIZE_REASON_NAME == txtHosReasonNt.Text.Trim())
					{
						HospitalizeReasonCode = val2.HOSPITALIZE_REASON_CODE;
						HospitalizeReasonName = val2.HOSPITALIZE_REASON_NAME;
					}
					else
					{
						HospitalizeReasonCode = null;
						HospitalizeReasonName = txtHosReasonNt.Text.Trim();
					}
				}
				else
				{
					HospitalizeReasonCode = null;
					HospitalizeReasonName = txtHosReasonNt.Text.Trim();
				}
				dataServiceReqInfoADO.HospitalizeReasonCode = HospitalizeReasonCode;
				dataServiceReqInfoADO.HospitalizeReasonName = HospitalizeReasonName;
				dataServiceReqInfoADO.IsExamOnline = chkExamOnline.Checked;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				dataServiceReqInfoADO = null;
			}
			return dataServiceReqInfoADO;
		}

		public void SetValue(UCServiceReqInfoADO dataServiceReqInfoADO)
		{
			try
			{
				if (dataServiceReqInfoADO._FocusNextUserControl != null)
				{
					dlgFocusNextUserControl = dataServiceReqInfoADO._FocusNextUserControl;
				}
				if (dataServiceReqInfoADO == null)
				{
					return;
				}
				txtHosReason.Text = dataServiceReqInfoADO.HospitalizationReason;
				if (dataServiceReqInfoADO.IsCapMaMS)
				{
					chkCapMaMS.Checked = true;
				}
				else
				{
					chkCapMaMS.Checked = false;
				}
				if (dataServiceReqInfoADO.IsChronic)
				{
					chkIsChronic.Checked = true;
				}
				else
				{
					chkIsChronic.Checked = false;
				}
				if (dataServiceReqInfoADO.IsNotRequireFee)
				{
					chkIsNotRequireFee.Checked = true;
				}
				else
				{
					chkIsNotRequireFee.Checked = false;
				}
				if (dataServiceReqInfoADO.IsPriority)
				{
					chkPriority.Checked = true;
					lciPriority.AppearanceItemCaption.ForeColor = Color.Maroon;
				}
				else
				{
					chkPriority.Checked = false;
					lciPriority.AppearanceItemCaption.ForeColor = Color.Black;
				}
				if (dataServiceReqInfoADO.IsEmergency)
				{
					chkEmergency.Checked = true;
				}
				else
				{
					chkEmergency.Checked = false;
				}
				txtMaMS.Text = dataServiceReqInfoADO.MaMS;
				txtIntructionTime.Text = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(dataServiceReqInfoADO.IntructionTime);
				if (dataServiceReqInfoADO.TreatmentType_ID > 0)
				{
					HIS_TREATMENT_TYPE val = null;
					val = BackendDataWorker.Get<HIS_TREATMENT_TYPE>().FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == dataServiceReqInfoADO.TreatmentType_ID);
					if (val != null)
					{
						cboTreatmentType.EditValue = val.ID;
					}
				}
				else
				{
					cboTreatmentType.EditValue = null;
				}
				if (dataServiceReqInfoADO.PriorityType.HasValue && dataServiceReqInfoADO.PriorityType > 0)
				{
					HIS_PRIORITY_TYPE val2 = null;
					val2 = BackendDataWorker.Get<HIS_PRIORITY_TYPE>().FirstOrDefault((HIS_PRIORITY_TYPE o) => o.ID == dataServiceReqInfoADO.PriorityType);
					if (val2 != null)
					{
						cboPriorityType.EditValue = val2.ID;
					}
				}
				else
				{
					cboPriorityType.EditValue = null;
				}
				if (dataServiceReqInfoADO.EmergencyTime_ID > 0)
				{
					HIS_EMERGENCY_WTIME val3 = BackendDataWorker.Get<HIS_EMERGENCY_WTIME>().FirstOrDefault((HIS_EMERGENCY_WTIME o) => o.ID == dataServiceReqInfoADO.EmergencyTime_ID);
					if (val3 != null)
					{
						cboEmergencyTime.EditValue = val3.EMERGENCY_WTIME_NAME;
					}
				}
				else
				{
					cboEmergencyTime.EditValue = null;
				}
				if (dataServiceReqInfoADO.OweType_ID > 0)
				{
					HIS_OWE_TYPE val4 = BackendDataWorker.Get<HIS_OWE_TYPE>().FirstOrDefault((HIS_OWE_TYPE o) => o.ID == dataServiceReqInfoADO.OweType_ID);
					if (val4 != null)
					{
						cboOweType.EditValue = val4.OWE_TYPE_NAME;
					}
				}
				else
				{
					cboOweType.EditValue = null;
				}
				if (dataServiceReqInfoADO.OTHER_PAY_SOURCE_ID > 0)
				{
					cboOtherPaySource.EditValue = dataServiceReqInfoADO.OTHER_PAY_SOURCE_ID;
				}
				else
				{
					cboOtherPaySource.EditValue = null;
				}
				if (dataServiceReqInfoADO.PriorityNumber.HasValue)
				{
					txtSTTPriority.EditValue = dataServiceReqInfoADO.PriorityNumber;
				}
				else
				{
					txtSTTPriority.EditValue = null;
				}
				txtIncode.Text = dataServiceReqInfoADO.IN_CODE;
				cboPatientClassify.EditValue = dataServiceReqInfoADO.PATIENT_CLASSIFY_ID;
				if (!string.IsNullOrEmpty(dataServiceReqInfoADO.GUARANTEE_LOGINNAME))
				{
					cboGuaranteeUsername.EditValue = dataServiceReqInfoADO.GUARANTEE_LOGINNAME;
				}
				else
				{
					cboGuaranteeUsername.EditValue = null;
				}
				txtGuaranteeReason.Text = dataServiceReqInfoADO.GUARANTEE_REASON;
				txtNote.Text = dataServiceReqInfoADO.NOTE;
				chkIsHiv.Checked = dataServiceReqInfoADO.IsHiv;
				if (dataServiceReqInfoADO.IsTuberCulosis)
				{
					chkTuberculosis.Checked = true;
				}
				else
				{
					chkTuberculosis.Checked = false;
				}
				if (!string.IsNullOrEmpty(dataServiceReqInfoADO.HospitalizeReasonCode))
				{
					List<HIS_HOSPITALIZE_REASON> list = cboHosReason.Properties.DataSource as List<HIS_HOSPITALIZE_REASON>;
					if (list != null && list.Count > 0 && list.FirstOrDefault((HIS_HOSPITALIZE_REASON o) => o.HOSPITALIZE_REASON_CODE == dataServiceReqInfoADO.HospitalizeReasonCode) != null)
					{
						cboHosReason.EditValue = list.FirstOrDefault((HIS_HOSPITALIZE_REASON o) => o.HOSPITALIZE_REASON_CODE == dataServiceReqInfoADO.HospitalizeReasonCode).ID;
					}
					else
					{
						txtHosReasonNt.Text = dataServiceReqInfoADO.HospitalizeReasonName;
					}
				}
				else
				{
					txtHosReasonNt.Text = dataServiceReqInfoADO.HospitalizeReasonName;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetValueIncode(string _inCode)
		{
			try
			{
				txtIncode.Text = _inCode;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetValueChronic(bool _isChronic)
		{
			try
			{
				chkIsChronic.Checked = _isChronic;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetValueByPatientInfo(HisPatientSDO data)
		{
			try
			{
				patientSdo = data;
				chkIsChronic.Checked = ((HIS_PATIENT)data).IS_CHRONIC == 1;
				chkTuberculosis.Checked = ((HIS_PATIENT)data).IS_TUBERCULOSIS == 1;
				cboPatientClassify.EditValue = null;
				chkCAPD.Checked = ((HIS_PATIENT)data).IS_CAPD == 1;
				cboPatientClassify.EditValue = ((HIS_PATIENT)data).PATIENT_CLASSIFY_ID;
				chkIsHiv.Checked = ((HIS_PATIENT)data).IS_HIV == 1;
				GetTreatment();
				cboTreatmentType_EditValueChanged(null, null);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void GetTreatment()
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			try
			{
				if (patientSdo == null || !patientSdo.TreatmentId.HasValue)
				{
					TreatmentByPatientSdo = null;
					return;
				}
				try
				{
					if (patientSdo.LastTreatmentFee != null && patientSdo.LastTreatmentFee.ID > 0)
					{
						TreatmentByPatientSdo = new HIS_TREATMENT
						{
							HOSPITALIZATION_REASON = patientSdo.LastTreatmentFee.HOSPITALIZATION_REASON,
							ICD_NAME = patientSdo.LastTreatmentFee.ICD_NAME,
							IS_CHRONIC = patientSdo.LastTreatmentFee.IS_CHRONIC
						};
					}
				}
				catch (Exception ex)
				{
					LogSystem.Error(ex);
				}
				HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
				hisTreatmentFilter.ID = patientSdo.TreatmentId;
				TreatmentByPatientSdo = new BackendAdapter(new CommonParam()).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, null).FirstOrDefault();
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		public void SetMaMS(string msCode)
		{
			txtMaMS.Text = msCode;
		}

		public void SetCapMaMsLayout(bool isEnable)
		{
			try
			{
				chkCapMaMS.Enabled = isEnable;
				chkCapMaMS.ReadOnly = !isEnable;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetEnableChkExamOnline(bool IsEnable)
		{
			chkExamOnline.Enabled = IsEnable;
			if (!IsEnable)
			{
				chkExamOnline.Checked = false;
			}
			chkExamOnline.ReadOnly = !IsEnable;
		}

		public void RefreshUserControl()
		{
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Expected O, but got Unknown
			try
			{
				patientSdo = null;
				dtIntructionTime.EditValue = DateTime.Now;
				cboTreatmentType.EditValue = 1L;
				cboEmergencyTime.EditValue = null;
				cboOweType.EditValue = null;
				chkIsChronic.Checked = false;
				chkEmergency.Checked = false;
				chkIsNotRequireFee.Checked = false;
				string text = ConfigApplicationWorker.Get<string>("CONFIG_KEY__DEFAULT_CONFIG_IS_NOT_REQUIRE_FEE");
				if (workingPatientType != null && !string.IsNullOrEmpty(text))
				{
					List<string> list = text.Split(',').ToList();
					chkIsNotRequireFee.Checked = list != null && list.Count > 0 && list.Contains(workingPatientType.PATIENT_TYPE_CODE);
				}
				chkPriority.Checked = false;
				cboPriorityType.EditValue = null;
				cboOtherPaySource.EditValue = null;
				txtNguonKhach.Text = null;
				cboNguonKhach.EditValue = null;
				cboNguonKhachCT.EditValue = null;
				cboEmergencyTime.Enabled = false;
				chkIsHiv.Checked = false;
				cboHosReason.EditValue = null;
				FillDataOweTypeDefault();
				txtIntructionTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
				cboCTT.EditValue = null;
				txtSTTPriority.EditValue = null;
				_HisTreatment = new HIS_TREATMENT();
				_PatientName = "";
				txtTreatmentOrder.Text = "";
				txtMaMS.Text = "";
				chkCapMaMS.Checked = false;
				chkExamOnline.Checked = false;
				txtIncode.Text = "";
				lciPriority.AppearanceItemCaption.ForeColor = Color.Black;
				cboPatientClassify.EditValue = null;
				cboGuaranteeUsername.EditValue = null;
				txtGuaranteeReason.Text = "";
				txtNote.Text = "";
				chkTuberculosis.Checked = false;
				txtHosReason.Text = null;
				txtHosReasonNt.Text = null;
				chkCAPD.Checked = false;
				chkChamSocDa.Checked = false;
				cboNguonKhachCT.EditValue = null;
				HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection = cboNguonKhachCT.Properties.Tag as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection;
				if (gridCheckMarksSelection != null)
				{
					gridCheckMarksSelection.ClearSelection(cboNguonKhachCT.Properties.View);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void FocusUserControl()
		{
			try
			{
				txtIntructionTime.Focus();
				if (txtIntructionTime.Text.Trim().Length >= 4)
				{
					txtIntructionTime.SelectAll();
				}
				else
				{
					txtIntructionTime_ButtonClick(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void FocusNextUserControl(Action<object> _dlgFocusNextUserControl)
		{
			try
			{
				if (_dlgFocusNextUserControl != null)
				{
					dlgFocusNextUserControl = _dlgFocusNextUserControl;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void FocusGuarantee()
		{
			try
			{
				txtGuaranteeLoginname.Focus();
				txtGuaranteeLoginname.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusToPriorityType()
		{
			try
			{
				FocusShowpopup(cboPriorityType, false);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusToTreatmentType()
		{
			try
			{
				FocusShowpopup(cboTreatmentType, false);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusToEmergencyTime()
		{
			try
			{
				FocusShowpopup(cboEmergencyTime, false);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusToIncode()
		{
			try
			{
				txtIncode.Focus();
				txtIncode.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusToOweType()
		{
			try
			{
				FocusShowpopup(cboOweType, false);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusTochkPriority()
		{
			try
			{
				chkPriority.Focus();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusTochkEmergency()
		{
			try
			{
				chkEmergency.Focus();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusTochkIsNotRequireFee()
		{
			try
			{
				chkIsNotRequireFee.Focus();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusTochkIsChronic()
		{
			try
			{
				chkIsChronic.Focus();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetEnableControl(bool isEnable)
		{
			try
			{
				chkEmergency.Checked = isEnable;
				lciEmergencyTime.Enabled = isEnable;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetDelegateHeinRightRouteType(Action<bool> isOutTime)
		{
			try
			{
				if (isOutTime != null)
				{
					dlgHeinRightRouteType = isOutTime;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetDelegatePriorityNumberChanged(Action<long?> priorityNumberChanged)
		{
			try
			{
				if (priorityNumberChanged != null)
				{
					dlgPriorityNumberChanged = priorityNumberChanged;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetBranchTime(List<HIS_BRANCH_TIME> _branchTimes, bool _isUserBranchTime)
		{
			try
			{
				_BranchTimes = _branchTimes;
				_IsUserBranchTime = _isUserBranchTime;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetPatientName(string _patientName)
		{
			try
			{
				_PatientName = _patientName;
				_HisTreatment.FUND_CUSTOMER_NAME = _patientName;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusShowpopup(LookUpEdit cboEditor, bool isSelectFirstRow)
		{
			try
			{
				cboEditor.Focus();
				cboEditor.ShowPopup();
				if (isSelectFirstRow)
				{
					PopupLoader.SelectFirstRowPopup(cboEditor);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboCommon(Control cboEditor, object data, string valueMember, string displayMember, string displayMemberCode)
		{
			try
			{
				InitComboCommon(cboEditor, data, valueMember, displayMember, 0, displayMemberCode, 0);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboCommon(Control cboEditor, object data, string valueMember, string displayMember, int displayMemberWidth, string displayMemberCode, int displayMemberCodeWidth)
		{
			try
			{
				int num = 0;
				List<ColumnInfo> list = new List<ColumnInfo>();
				if (!string.IsNullOrEmpty(displayMemberCode))
				{
					list.Add(new ColumnInfo(displayMemberCode, "", (displayMemberCodeWidth > 0) ? displayMemberCodeWidth : 100, 1));
					num += ((displayMemberCodeWidth > 0) ? displayMemberCodeWidth : 100);
				}
				if (!string.IsNullOrEmpty(displayMember))
				{
					list.Add(new ColumnInfo(displayMember, "", (displayMemberWidth > 0) ? displayMemberWidth : 250, 2));
					num += ((displayMemberWidth > 0) ? displayMemberWidth : 250);
				}
				ControlEditorADO controlEditorADO = new ControlEditorADO(displayMember, valueMember, list, false, num);
				ControlEditorLoader.Load(cboEditor, data, controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadBranch()
		{
			try
			{
				HIS_BRANCH branch = BranchDataWorker.Branch;
				_BranchTimes = new List<HIS_BRANCH_TIME>();
				if (branch != null && branch.IS_USE_BRANCH_TIME == 1)
				{
					_IsUserBranchTime = true;
					CommonParam commonParam = new CommonParam();
					HisBranchTimeFilter hisBranchTimeFilter = new HisBranchTimeFilter();
					hisBranchTimeFilter.BRANCH_ID = branch.ID;
					_BranchTimes = new BackendAdapter(commonParam).Get<List<HIS_BRANCH_TIME>>("api/HisBranchTime/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisBranchTimeFilter, commonParam);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void ShowValidation(bool _isShow)
		{
			try
			{
				ReloadValidation(_isShow);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public bool ValidateRequiredField()
		{
			bool flag = true;
			try
			{
				flag = dxValidationUCOtherReqInfo.Validate();
				if (HisConfig.IsRequiredPriorityTypeInCaseOfCheckingPriority && chkPriority.Checked && cboPriorityType.EditValue == null)
				{
					flag = false;
					XtraMessageBox.Show("Bạn cần chọn trường hợp ưu tiên");
				}
				if (workingPatientType != null && !string.IsNullOrEmpty(workingPatientType.OTHER_PAY_SOURCE_IDS) && (cboOtherPaySource.EditValue == null || (cboOtherPaySource.EditValue ?? "").ToString() == "0"))
				{
					flag = false;
					XtraMessageBox.Show("Bạn cần chọn nguồn chi trả khác");
					cboOtherPaySource.Focus();
				}
			}
			catch (Exception ex)
			{
				flag = false;
				LogSystem.Warn(ex);
			}
			return flag;
		}

		public void ResetRequiredField()
		{
			try
			{
				ControlWorker.ValidationProviderRemoveControlError(dxValidationUCOtherReqInfo, dxErrorProviderControl);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateIntructionTime()
		{
			IntructionTime__ValidationRule intructionTime__ValidationRule = new IntructionTime__ValidationRule();
			intructionTime__ValidationRule.txtIntructionTime = txtIntructionTime;
			intructionTime__ValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
			intructionTime__ValidationRule.ErrorType = ErrorType.Warning;
			dxValidationUCOtherReqInfo.SetValidationRule(txtIntructionTime, intructionTime__ValidationRule);
		}

		private void ValidateTextHosReason()
		{
			TextValidationRule textValidationRule = new TextValidationRule();
			textValidationRule.txtText = txtHosReason;
			textValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
			textValidationRule.ErrorType = ErrorType.Warning;
			dxValidationUCOtherReqInfo.SetValidationRule(txtHosReason, textValidationRule);
		}

		private void ValidateFrmFun()
		{
			try
			{
				FrmFunValidationRule frmFunValidationRule = new FrmFunValidationRule();
				frmFunValidationRule.cboCCT = cboCTT;
				frmFunValidationRule.frm = this;
				frmFunValidationRule.ErrorText = ResourceMessage.ChuaNhapThongTinDoiTuongCungChiTra;
				frmFunValidationRule.ErrorType = ErrorType.Warning;
				dxValidationUCOtherReqInfo.SetValidationRule(cboCTT, frmFunValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidateFrmInCode()
		{
			try
			{
				TextEditMaxLengthValidationRule textEditMaxLengthValidationRule = new TextEditMaxLengthValidationRule();
				textEditMaxLengthValidationRule.txtEdit = txtIncode;
				textEditMaxLengthValidationRule.maxlength = 10;
				textEditMaxLengthValidationRule.isVali = true;
				textEditMaxLengthValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				textEditMaxLengthValidationRule.ErrorType = ErrorType.Warning;
				dxValidationUCOtherReqInfo.SetValidationRule(txtIncode, textEditMaxLengthValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidatePriorityType()
		{
			try
			{
				ValidatecboPriorityType();
				ValidatechkPriority();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidatecboPriorityType()
		{
			try
			{
				PriorityType__ValidationRule priorityType__ValidationRule = new PriorityType__ValidationRule();
				priorityType__ValidationRule.cboPriorityType = cboPriorityType;
				priorityType__ValidationRule.hasDataAutoCheckPriority = hasDataAutoCheckPriority;
				priorityType__ValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				priorityType__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationUCOtherReqInfo.SetValidationRule(cboPriorityType, priorityType__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidatechkPriority()
		{
			try
			{
				Priority__ValidationRule priority__ValidationRule = new Priority__ValidationRule();
				priority__ValidationRule.chkPriority = chkPriority;
				priority__ValidationRule.hasDataAutoCheckPriority = hasDataAutoCheckPriority;
				priority__ValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				priority__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationUCOtherReqInfo.SetValidationRule(chkPriority, priority__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidateTreatmentType()
		{
			try
			{
				TreatmentType__ValidationRule treatmentType__ValidationRule = new TreatmentType__ValidationRule();
				treatmentType__ValidationRule.cboTreatmentType = cboTreatmentType;
				treatmentType__ValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				treatmentType__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationUCOtherReqInfo.SetValidationRule(cboTreatmentType, treatmentType__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateNumOrderPriority()
		{
			try
			{
				ServiceReqNumOrder__ValidationRule serviceReqNumOrder__ValidationRule = new ServiceReqNumOrder__ValidationRule();
				serviceReqNumOrder__ValidationRule.spinNumOrderPriority = txtSTTPriority;
				serviceReqNumOrder__ValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				serviceReqNumOrder__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationUCOtherReqInfo.SetValidationRule(txtSTTPriority, serviceReqNumOrder__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
