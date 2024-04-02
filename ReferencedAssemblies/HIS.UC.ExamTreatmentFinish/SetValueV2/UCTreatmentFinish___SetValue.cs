using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Controls.EditorLoader;
using HIS.UC.ExamTreatmentFinish.Config;
using Inventec.Common.WebApiClient;
using HIS.Desktop.ApiConsumer;
using HIS.UC.ExamTreatmentFinish.ADO;
using MOS.SDO;

namespace HIS.UC.ExamTreatmentFinish.Run
{
    public partial class UCExamTreatmentFinish : UserControl
    {
        HisServiceReqExamUpdateResultSDO HisServiceReqResult;

        public void SetValueV2(Inventec.Desktop.Common.Modules.Module _currentModule, HisServiceReqExamUpdateResultSDO _HisServiceReqResult, long currentRoomId)
        {
            try
            {
                this.HisServiceReqResult = _HisServiceReqResult;
                this.currentModule = _currentModule;
                this.treatment = _HisServiceReqResult.TreatmentFinishResult;
                if (treatment != null)
                {
                    lblEndCode.Text = treatment.END_CODE;
                    lblOutCode.Text = treatment.OUT_CODE;
                    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN)
                    {
                        lciChiDinhDichVuhenKham.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        //lciChiDinhDichVuhenKham.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        //if (this._ucAdvise != null)
                        //{
                        //    AdviseADO ado = new AdviseADO()
                        //    {
                        //        Advise = treatment.ADVISE,
                        //        AppointmentExamRoomIds = treatment.APPOINTMENT_EXAM_ROOM_IDS
                        //    };
                        //    ado.currentRoomId = currentRoomId;
                        //    this._ucAdvise.SetValue(ado);
                        //}

                        if (currentTreatmentFinishSDO != null)
                        {
                            currentTreatmentFinishSDO.Advise = treatment.ADVISE;
                            currentTreatmentFinishSDO.AppointmentTime = treatment.APPOINTMENT_TIME;
                            if (!String.IsNullOrWhiteSpace(treatment.APPOINTMENT_EXAM_ROOM_IDS))
                            {
                                currentTreatmentFinishSDO.AppointmentExamRoomIds = new List<long>();
                                var ids = treatment.APPOINTMENT_EXAM_ROOM_IDS.Split(',').ToList();
                                foreach (var item in ids)
                                {
                                    currentTreatmentFinishSDO.AppointmentExamRoomIds.Add(Inventec.Common.TypeConvert.Parse.ToInt64(item));
                                }
                            }
                        }
                    }
                    else
                    {
                        lciChiDinhDichVuhenKham.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }

                    if (treatment.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_OM || treatment.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_DUONG_THAI)
                    {
                        lciChiDinhDichVuhenKham.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        if (this.ucSick != null)
                        {
                            this.sickProcessor.Reload(this.ucSick, treatment);
                        }
                    }
                    else if (treatment.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__HEN_MO)
                    {
                        //TODO
                    }

                    //if (treatment.MEDI_RECORD_ID.HasValue)
                    //{
                    //    MOS.Filter.HisMediRecordFilter filter = new MOS.Filter.HisMediRecordFilter();
                    //    filter.ID = this.treatment.MEDI_RECORD_ID.Value;
                    //    var mediRecord = new Inventec.Common.Adapter.BackendAdapter(new Inventec.Core.CommonParam()).Get<List<HIS_MEDI_RECORD>>("api/HisMediRecord/Get", ApiConsumers.MosConsumer, filter, null).FirstOrDefault();
                    //    lblSoLuuTruBA.Text = mediRecord != null ? mediRecord.STORE_CODE : "";
                    //}

                    LoadIcdToControl(treatment.ICD_CODE, treatment.ICD_NAME);
                    LoaducSecondaryIcd(treatment.ICD_SUB_CODE, treatment.ICD_TEXT);
                }
                else
                {
                    lblEndCode.Text = "";
                    lblOutCode.Text = "";
                    lciChiDinhDichVuhenKham.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }

                if (_HisServiceReqResult.MediRecord != null)
                {
                    lblSoLuuTruBA.Text = _HisServiceReqResult.MediRecord.STORE_CODE;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
