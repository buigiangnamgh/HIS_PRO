using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Common.WebApiClient;
using Inventec.Core;
using Microsoft.CSharp.RuntimeBinder;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.ControlProcess
{
	public class TranPatiReasonProcess
	{
		[CompilerGenerated]
		private static class _003C_003Eo__0
		{
			public static CallSite<Func<CallSite, BackendAdapter, string, ApiConsumer, object, CommonParam, object>> _003C_003Ep__0;

			public static CallSite<Func<CallSite, object, List<HIS_TRAN_PATI_REASON>>> _003C_003Ep__1;
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CLoadDataToComboLyDoChuyen_003Ed__0 : IAsyncStateMachine
		{
			private static class _003C_003Eo__0
			{
				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__0;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__1;

				public static CallSite<Func<CallSite, object, bool>> _003C_003Ep__2;

				public static CallSite<Func<CallSite, object, object>> _003C_003Ep__3;
			}

			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder _003C_003Et__builder;

			public List<HIS_TRAN_PATI_REASON> data;

			public LookUpEdit cboLyDoChuyen;

			private Func<CallSite, object, List<HIS_TRAN_PATI_REASON>> _003C_003E7__wrap1;

			private CallSite<Func<CallSite, object, List<HIS_TRAN_PATI_REASON>>> _003C_003E7__wrap2;

			private object _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				try
				{
					try
					{
						dynamic val2;
						if (num != 0)
						{
							if (data == null)
							{
								if (!BackendDataWorker.IsExistsKey<HIS_TRAN_PATI_REASON>())
								{
									CommonParam commonParam = new CommonParam();
									dynamic val = new ExpandoObject();
									if (TranPatiReasonProcess._003C_003Eo__0._003C_003Ep__1 == null)
									{
										TranPatiReasonProcess._003C_003Eo__0._003C_003Ep__1 = CallSite<Func<CallSite, object, List<HIS_TRAN_PATI_REASON>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(List<HIS_TRAN_PATI_REASON>), typeof(TranPatiReasonProcess)));
									}
									_003C_003E7__wrap1 = TranPatiReasonProcess._003C_003Eo__0._003C_003Ep__1.Target;
									_003C_003E7__wrap2 = TranPatiReasonProcess._003C_003Eo__0._003C_003Ep__1;
									val2 = new BackendAdapter(commonParam).GetAsync<List<HIS_TRAN_PATI_REASON>>("api/HisTranpatiReason/Get", ApiConsumers.MosConsumer, val, commonParam).GetAwaiter();
									if (!(bool)val2.IsCompleted)
									{
										num = (_003C_003E1__state = 0);
										_003C_003Eu__1 = val2;
										ICriticalNotifyCompletion awaiter = val2 as ICriticalNotifyCompletion;
										if (awaiter == null)
										{
											INotifyCompletion awaiter2 = (INotifyCompletion)(object)val2;
											_003C_003Et__builder.AwaitOnCompleted(ref awaiter2, ref this);
											awaiter2 = null;
										}
										else
										{
											_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
										}
										awaiter = null;
										return;
									}
									goto IL_025e;
								}
								data = BackendDataWorker.Get<HIS_TRAN_PATI_REASON>();
							}
							goto IL_0308;
						}
						val2 = _003C_003Eu__1;
						_003C_003Eu__1 = null;
						num = (_003C_003E1__state = -1);
						goto IL_025e;
						IL_0308:
						cboLyDoChuyen.Properties.DataSource = data;
						cboLyDoChuyen.Properties.DisplayMember = "TRAN_PATI_REASON_NAME";
						cboLyDoChuyen.Properties.ValueMember = "ID";
						cboLyDoChuyen.Properties.ForceInitialize();
						cboLyDoChuyen.Properties.Columns.Clear();
						cboLyDoChuyen.Properties.Columns.Add(new LookUpColumnInfo("TRAN_PATI_REASON_CODE", "", 50));
						cboLyDoChuyen.Properties.Columns.Add(new LookUpColumnInfo("TRAN_PATI_REASON_NAME", "", 400));
						cboLyDoChuyen.Properties.ShowHeader = false;
						cboLyDoChuyen.Properties.ImmediatePopup = true;
						cboLyDoChuyen.Properties.DropDownRows = 20;
						cboLyDoChuyen.Properties.PopupWidth = 450;
						goto end_IL_000a;
						IL_025e:
						object result = val2.GetResult();
						data = _003C_003E7__wrap1(_003C_003E7__wrap2, result);
						_003C_003E7__wrap1 = null;
						_003C_003E7__wrap2 = null;
						if (data != null)
						{
							BackendDataWorker.UpdateToRam(typeof(HIS_TRAN_PATI_REASON), data, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
						}
						goto IL_0308;
						end_IL_000a:;
					}
					catch (Exception ex)
					{
						LogSystem.Warn(ex);
					}
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
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
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

        public static async Task LoadDataToComboLyDoChuyen(
    LookUpEdit cboLyDoChuyen,
    List<HIS_TRAN_PATI_REASON> data)
        {
            try
            {
                if (data == null)
                {
                    if (BackendDataWorker.IsExistsKey<HIS_TRAN_PATI_REASON>())
                    {
                        data = BackendDataWorker.Get<HIS_TRAN_PATI_REASON>();
                    }
                    else
                    {
                        CommonParam commonParam = new CommonParam();

                        List<HIS_TRAN_PATI_REASON> dataApi =
                            await new BackendAdapter(commonParam).GetAsync<List<HIS_TRAN_PATI_REASON>>(
                                "api/HisTranpatiReason/Get",
                                ApiConsumers.MosConsumer,
                                null,
                                commonParam);

                        data = dataApi;

                        if (data != null)
                        {
                            BackendDataWorker.UpdateToRam(
                                typeof(HIS_TRAN_PATI_REASON),
                                data,
                                long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
                        }
                    }
                }

                cboLyDoChuyen.Properties.DataSource = data;
                cboLyDoChuyen.Properties.DisplayMember = "TRAN_PATI_REASON_NAME";
                cboLyDoChuyen.Properties.ValueMember = "ID";

                cboLyDoChuyen.Properties.ForceInitialize();
                cboLyDoChuyen.Properties.Columns.Clear();

                cboLyDoChuyen.Properties.Columns.Add(
                    new LookUpColumnInfo("TRAN_PATI_REASON_CODE", "", 50));

                cboLyDoChuyen.Properties.Columns.Add(
                    new LookUpColumnInfo("TRAN_PATI_REASON_NAME", "", 400));

                cboLyDoChuyen.Properties.ShowHeader = false;
                cboLyDoChuyen.Properties.ImmediatePopup = true;
                cboLyDoChuyen.Properties.DropDownRows = 20;
                cboLyDoChuyen.Properties.PopupWidth = 450;
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }
	}
}
