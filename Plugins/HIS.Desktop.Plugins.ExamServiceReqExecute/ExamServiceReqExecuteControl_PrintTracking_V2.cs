/* IVT
 * @Project : hisnguonmo
 * Copyright (C) 2017 INVENTEC
 *  
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 *  
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */
using AutoMapper;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.Print;
using HIS.Desktop.Utility;
using HIS.UC.Icd;
using HIS.UC.Icd.ADO;
using Inventec.Common.Adapter;
using Inventec.Core;
using Inventec.Desktop.Common.Message;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;
using MPS.ADO;
using MPS.ADO.TrackingPrint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EMR.Filter;
using EMR.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.ExamServiceReqExecute
{
    public partial class ExamServiceReqExecuteControl : UserControlBase
    {

        internal HIS_TRACKING currentTracking { get; set; }

        internal bool _IsMaterial { get; set; }
               
        List<V_HIS_TREATMENT_BED_ROOM> _TreatmentBedRooms_Fast { get; set; }

        List<HIS_SERVICE_REQ> _ServiceReqs_Fast { get; set; }
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReqs_Fast { get; set; }

        List<HIS_SERE_SERV> _SereServs_Fast { get; set; }
        Dictionary<long, List<HIS_SERE_SERV>> dicSereServs_Fast { get; set; }

        List<HIS_EXP_MEST> _ExpMests_Fast { get; set; }
        Dictionary<long, HIS_EXP_MEST> dicExpMests_Fast { get; set; }

        List<HIS_EXP_MEST_MEDICINE> _ExpMestMedicines_Fast { get; set; }
        Dictionary<long, List<HIS_EXP_MEST_MEDICINE>> dicExpMestMedicines_Fast { get; set; }

        List<HIS_EXP_MEST_MATERIAL> _ExpMestMaterials_Fast { get; set; }
        Dictionary<long, List<HIS_EXP_MEST_MATERIAL>> dicExpMestMaterials_Fast { get; set; }

        Dictionary<long, List<HIS_SERVICE_REQ_METY>> dicServiceReqMetys_Fast { get; set; }
        Dictionary<long, List<HIS_SERVICE_REQ_MATY>> dicServiceReqMatys_Fast { get; set; }

        List<HIS_SERE_SERV_EXT> _SereServExts_Fast { get; set; }
        //suat an
        List<V_HIS_SERE_SERV_RATION> _SereServRation_Fast { get; set; }

        //thuốc, vật tư trả lại
        internal List<V_HIS_IMP_MEST_2> _MobaImpMests_Fast { get; set; }
        internal List<V_HIS_IMP_MEST_MEDICINE> _ImpMestMedicines_TL_Fast { get; set; }
        internal List<V_HIS_IMP_MEST_MATERIAL> _ImpMestMaterial_TL_Fast { get; set; }
        internal List<V_HIS_IMP_MEST_BLOOD> _ImpMestBlood_TL_Fast { get; set; }

        List<V_HIS_EXP_MEST_BLTY_REQ_2> HisExpMestBltyReq2_Fast { get; set; }

        bool IsNotShowOutMediAndMate_Fast = false;
        bool BloodPresOption_Fast { get; set; }

        List<V_HIS_TRACKING> _TrackingPrintsProcesss_Fast;

        private void Mps000062(string printTypeCode, string fileName, ref bool result, bool resultPrintAndSign = false)
        {
            try
            {
                #region ----
                WaitingManager.Show();
                if (this.currentTracking == null)
                {
                    return;
                }
                V_HIS_TRACKING currentTrackingForPrint = GetVHisTrackingByID(this.currentTracking.ID);
                CommonParam param = new CommonParam();
                _TrackingPrintsProcesss_Fast = new List<V_HIS_TRACKING>();
                _TrackingPrintsProcesss_Fast.Add(currentTrackingForPrint);

                _TreatmentBedRooms_Fast = new List<V_HIS_TREATMENT_BED_ROOM>();

                _ServiceReqs_Fast = new List<HIS_SERVICE_REQ>();
                dicServiceReqs_Fast = new Dictionary<long, HIS_SERVICE_REQ>();

                _SereServs_Fast = new List<HIS_SERE_SERV>();
                dicSereServs_Fast = new Dictionary<long, List<HIS_SERE_SERV>>();

                _ExpMests_Fast = new List<HIS_EXP_MEST>();
                dicExpMests_Fast = new Dictionary<long, HIS_EXP_MEST>();

                _ExpMestMedicines_Fast = new List<HIS_EXP_MEST_MEDICINE>();
                dicExpMestMedicines_Fast = new Dictionary<long, List<HIS_EXP_MEST_MEDICINE>>();

                _ExpMestMaterials_Fast = new List<HIS_EXP_MEST_MATERIAL>();
                dicExpMestMaterials_Fast = new Dictionary<long, List<HIS_EXP_MEST_MATERIAL>>();

                dicServiceReqMetys_Fast = new Dictionary<long, List<HIS_SERVICE_REQ_METY>>();

                dicServiceReqMatys_Fast = new Dictionary<long, List<HIS_SERVICE_REQ_MATY>>();

                HisExpMestBltyReq2_Fast = new List<V_HIS_EXP_MEST_BLTY_REQ_2>();

                IsNotShowOutMediAndMate_Fast = (HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>("HIS.Desktop.Plugins.TrackingPrint.IsNotShowOutMediAndMate") == "1");

                this._SereServExts_Fast = new List<HIS_SERE_SERV_EXT>();

                this._SereServRation_Fast = new List<V_HIS_SERE_SERV_RATION>();

                //thuốc, vật tư trả lại
                this._MobaImpMests_Fast = new List<V_HIS_IMP_MEST_2>();
                this._ImpMestMedicines_TL_Fast = new List<V_HIS_IMP_MEST_MEDICINE>();
                this._ImpMestMaterial_TL_Fast = new List<V_HIS_IMP_MEST_MATERIAL>();
                this._ImpMestBlood_TL_Fast = new List<V_HIS_IMP_MEST_BLOOD>();

                if (this.treatmentId > 0)
                {
                    CreateThreadLoadData_Fast(this.treatmentId);
                }

                int start = 0;
                int count = this._ServiceReqs_Fast.Count;
                while (count > 0)
                {
                    int limit = (count <= 100) ? count : 100;
                    var listSub = this._ServiceReqs_Fast.Skip(start).Take(limit).ToList();
                    List<long> _serviceReqIds = new List<long>();
                    _serviceReqIds = listSub.Select(p => p.ID).Distinct().ToList();

                    CreateThreadByServiceReq_Fast(_serviceReqIds);

                    start += 100;
                    count -= 100;
                }

                if (this._ExpMests_Fast != null && this._ExpMests_Fast.Count > 0)
                {
                    int startExpMest = 0;
                    int countExpMest = _ExpMests_Fast.Count;
                    while (countExpMest > 0)
                    {
                        int limit = (countExpMest <= 100) ? countExpMest : 100;
                        var listSub = this._ExpMests_Fast.Skip(startExpMest).Take(limit).ToList();
                        List<long> _serviceReqIds = new List<long>();
                        CreateThreadLoadDataExpMest_Fast(listSub.Select(p => p.ID).Distinct().ToList());
                        startExpMest += 100;
                        countExpMest -= 100;
                    }
                }

                if (this._SereServs_Fast != null && this._SereServs_Fast.Count > 0)
                {
                    int startSS = 0;
                    int countSS = this._SereServs_Fast.Count;
                    while (countSS > 0)
                    {
                        int limit = (countSS <= 100) ? countSS : 100;
                        var listSub = this._SereServs_Fast.Skip(startSS).Take(limit).ToList();
                        List<long> _sereServIds = new List<long>();
                        _sereServIds = listSub.Select(p => p.ID).Distinct().ToList();

                        //Get SERE_SERV_EXT
                        MOS.Filter.HisSereServExtFilter sereServExtFilter = new MOS.Filter.HisSereServExtFilter();
                        sereServExtFilter.SERE_SERV_IDs = _sereServIds;

                        var dataSS_EXTs = new BackendAdapter(param).Get<List<HIS_SERE_SERV_EXT>>("/api/HisSereServExt/Get", ApiConsumers.MosConsumer, sereServExtFilter, param);
                        if (dataSS_EXTs != null && dataSS_EXTs.Count > 0)
                        {
                            this._SereServExts_Fast.AddRange(dataSS_EXTs);
                        }

                        startSS += 100;
                        countSS -= 100;
                    }
                }

                #endregion

                #region Dấu hiệu sinh tồn
                MOS.Filter.HisDhstFilter dhstFilter = new HisDhstFilter();
                dhstFilter.TRACKING_ID = currentTracking.ID;
                var _Dhsts = new BackendAdapter(param).Get<List<HIS_DHST>>(HisRequestUriStore.HIS_DHST_GET, ApiConsumers.MosConsumer, dhstFilter, param);
                #endregion

                #region Danh sách chăm sóc
                List<HIS_CARE> _Cares = new List<HIS_CARE>();
                List<V_HIS_CARE_DETAIL> _CareDetails = new List<V_HIS_CARE_DETAIL>();

                MOS.Filter.HisCareFilter careFilter = new HisCareFilter();
                careFilter.TREATMENT_ID = treatmentId;
                careFilter.TRACKING_ID = currentTracking.ID;
                var care = new BackendAdapter(param).Get<List<HIS_CARE>>(HisRequestUriStore.HIS_CARE_GET, ApiConsumers.MosConsumer, careFilter, param).FirstOrDefault();
                if (care != null)
                {
                    _Cares.Add(care);
                    MOS.Filter.HisCareDetailViewFilter careDetailFilter = new HisCareDetailViewFilter();
                    careDetailFilter.CARE_ID = care.ID;
                    var careDetail = new BackendAdapter(param).Get<List<V_HIS_CARE_DETAIL>>(HisRequestUriStore.HIS_CARE_DETAIL_GETVIEW, ApiConsumers.MosConsumer, careDetailFilter, param);
                    _CareDetails.AddRange(careDetail);
                }
                #endregion

                #region Thông tin khoa phòng hiện tại
                MOS.SDO.WorkPlaceSDO _workPlaceSDO = WorkPlace.WorkPlaceSDO.SingleOrDefault(p => p.RoomId == this.moduleData.RoomId);
                MPS.Processor.Mps000062.PDO.Mps000062SingleKey singleKey = new MPS.Processor.Mps000062.PDO.Mps000062SingleKey(_workPlaceSDO);
                singleKey.LOGIN_NAME = Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetLoginName();
                singleKey.USER_NAME = Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetUserName();
                #endregion

                #region suất ăn
                HisSereServRationViewFilter filterRation = new HisSereServRationViewFilter();
                filterRation.TRACKING_ID = currentTracking.ID;
                var ssRation = new BackendAdapter(param).Get<List<V_HIS_SERE_SERV_RATION>>("api/HisSereServRation/GetView", ApiConsumers.MosConsumer, filterRation, param);
                this._SereServRation_Fast.AddRange(ssRation);

                #endregion

                #region Máu chỉ định

                if (BloodPresOption_Fast)
                {
                    CommonParam param_ = new CommonParam();
                    MOS.Filter.HisExpMestBltyReqView2Filter filter = new HisExpMestBltyReqView2Filter();
                    filter.TDL_TREATMENT_ID = this.treatmentId;
                    filter.TRACKING_ID = currentTracking.ID;
                    HisExpMestBltyReq2_Fast = new BackendAdapter(param_).Get<List<V_HIS_EXP_MEST_BLTY_REQ_2>>("/api/HisExpMestBltyReq/GetView2", ApiConsumers.MosConsumer, filter, HIS.Desktop.Controls.Session.SessionManager.ActionLostToken, param_);
                }
                #endregion

                #region Danh sách dịch vụ
                #endregion

                #region key cấu hiển thị key stt
                singleKey.IsShowMedicineLine = (HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>("HIS.Desktop.Plugins.Library.Bordereau.IsShowMedicineLine") == "1");
                singleKey.IsOrderByType = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>("HIS.Desktop.Plugins.TrackingPrint.OderOption"));
                singleKey.keyVienTim = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKING_IS_NUMBER_BY_MEDICINE_TYPE));
                singleKey.UsedDayCountingOption = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKINGPRINT_USED_DAY_COUNTING_OPTION));
                singleKey.UsedDayCountingFormatOption = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKINGPRINT_USED_DAY_COUNTING_FORMAT_OPTION));
                singleKey.UsedDayCountingOutStockOption = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKINGPRINT_USED_DAY_COUNTING_OUT_STOCK_OPTION));
                #endregion

                #region Dòng thuốc
                List<HIS_MEDICINE_LINE> listMedicineLine = new List<HIS_MEDICINE_LINE>();
                listMedicineLine = BackendDataWorker.Get<HIS_MEDICINE_LINE>();
                #endregion
                #region Dạng bào chế
                List<HIS_DOSAGE_FORM> listDosage = new List<HIS_DOSAGE_FORM>();
                listDosage = BackendDataWorker.Get<HIS_DOSAGE_FORM>();
                #endregion
                #region Mps000062
                Inventec.Common.SignLibrary.ADO.InputADO inputADO = new HIS.Desktop.Plugins.Library.EmrGenerate.EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((this.treatment != null ? this.treatment.TREATMENT_CODE : ""), printTypeCode, this.moduleData != null ? this.moduleData.RoomId : 0);

                long keyPrintMerge = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKING_IS_PRINT_MERGE));

                if (keyPrintMerge == 1)
                {
                    string uniqueTime = "";// (_TrackingPrints != null && _TrackingPrints.Count > 0) ? _TrackingPrints[0].TRACKING_TIME + "" : "";
                    inputADO.MergeCode = String.Format("{0}_{1}_{2}", printTypeCode, uniqueTime, (this.treatment != null ? this.treatment.TREATMENT_CODE : ""));//TODO
                }


                if (_TrackingPrintsProcesss_Fast != null && _TrackingPrintsProcesss_Fast.Count > 0 && _TrackingPrintsProcesss_Fast[0].SHEET_ORDER != null)
                {
                    inputADO.DocumentName += " " + "(" + _TrackingPrintsProcesss_Fast[0].SHEET_ORDER.ToString() + ")";
                }

                //Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputADO.MergeCode), inputADO.MergeCode));
                var PatientTypeAlter = new BackendAdapter(new CommonParam()).Get<HIS_PATIENT_TYPE_ALTER>("api/HisPatientTypeAlter/GetLastByTreatmentId", ApiConsumers.MosConsumer, treatmentId, null);

                V_HIS_BED_LOG selectedBedLog = null;
                MOS.Filter.HisBedLogViewFilter bedLogViewFilter = new MOS.Filter.HisBedLogViewFilter();
                bedLogViewFilter.TREATMENT_ID = this.treatment.ID;
                var bedLogs = new Inventec.Common.Adapter.BackendAdapter(param).Get<List<V_HIS_BED_LOG>>("api/HisBedLog/GetView", ApiConsumer.ApiConsumers.MosConsumer, bedLogViewFilter, param);
                if (bedLogs != null)
                {
                    var minTrackingTime = this.currentTracking.TRACKING_TIME;
                    var filteredBedLogs = bedLogs
                        .Where(o => o.BED_ID != 0
                            && (o.START_TIME <= minTrackingTime && (o.FINISH_TIME == null || minTrackingTime <= o.FINISH_TIME))
                        )
                        .ToList();
                    selectedBedLog = filteredBedLogs
                        .OrderByDescending(o => o.START_TIME)
                        .ThenByDescending(o => o.ID)
                        .FirstOrDefault();
                }

                MPS.Processor.Mps000062.PDO.Mps000062PDO mps000062RDO = new MPS.Processor.Mps000062.PDO.Mps000062PDO(
                this.treatment,
                _TreatmentBedRooms_Fast,
                _TrackingPrintsProcesss_Fast,
                _Dhsts,
                dicServiceReqs_Fast,
                dicSereServs_Fast,
                dicExpMests_Fast,
                dicExpMestMedicines_Fast,
                dicExpMestMaterials_Fast,
                dicServiceReqMetys_Fast,
                dicServiceReqMatys_Fast,
                _Cares,
                _CareDetails,
                singleKey,
                BackendDataWorker.Get<HIS_ICD>(),
                BackendDataWorker.Get<V_HIS_MEDICINE_TYPE>(),
                BackendDataWorker.Get<V_HIS_MATERIAL_TYPE>(),
                BackendDataWorker.Get<HIS_SERVICE_TYPE>(),
                this._SereServExts_Fast,
                BackendDataWorker.Get<HIS_MEDICINE_USE_FORM>(),
                this._SereServRation_Fast,
                this._MobaImpMests_Fast,
                this._ImpMestMedicines_TL_Fast,
                this._ImpMestMaterial_TL_Fast,
                HisExpMestBltyReq2_Fast,
                BackendDataWorker.Get<V_HIS_SERVICE>().Where(o => this._SereServs_Fast.Select(p => p.SERVICE_ID).Contains(o.ID)).ToList(),
                this._ImpMestBlood_TL_Fast,
                PatientTypeAlter,
                listMedicineLine,
                listDosage,
                selectedBedLog
                );
                WaitingManager.Hide();
                MPS.ProcessorBase.Core.PrintData PrintData = null;

                MOS.Filter.HisTreatmentFilter hisTreatmentFilter = new MOS.Filter.HisTreatmentFilter();
                hisTreatmentFilter.ID = treatmentId;
                List<HIS_TREATMENT> LstTreatment = new List<HIS_TREATMENT>();
                LstTreatment = new BackendAdapter(new CommonParam()).Get<List<HIS_TREATMENT>>(HisRequestUriStore.HIS_TREATMENT_GET, ApiConsumers.MosConsumer, hisTreatmentFilter, null);

                string saveFilePath = "";
                string ext = Path.GetExtension(fileName);
                if (ext == ".doc" || ext == ".docx")
                {
                    saveFilePath = GenerateTempFileWithin("", ".docx");
                }

                PrintData = new MPS.ProcessorBase.Core.PrintData(printTypeCode, fileName, mps000062RDO, MPS.ProcessorBase.PrintConfig.PreviewType.EmrSignNow, "", 1, saveFilePath) { EmrInputADO = inputADO };

                WaitingManager.Hide();

                EmrDocumentViewFilter emrDocumentFilter = new EmrDocumentViewFilter();
                emrDocumentFilter.TREATMENT_CODE__EXACT = this.treatment.TREATMENT_CODE;
                emrDocumentFilter.IS_DELETE = false;
                var documents = new BackendAdapter(new CommonParam()).Get<List<V_EMR_DOCUMENT>>("api/EmrDocument/GetView", ApiConsumers.EmrConsumer, emrDocumentFilter, null);
                Inventec.Common.Logging.LogSystem.Debug("documents_________________ " + Inventec.Common.Logging.LogUtil.TraceData("documents ", documents));
                if (documents != null && documents.Count > 0)
                {
                    var checkServiceReqCode = "HIS_TRACKING:" + currentTracking.ID;
                    var resultEmrDocumentLast = documents.Where(o => o.DOCUMENT_TYPE_ID == IMSys.DbConfig.EMR_RS.EMR_DOCUMENT_TYPE.ID__TRACKING && !string.IsNullOrEmpty(o.HIS_CODE) && o.HIS_CODE.Contains(checkServiceReqCode));
                    if (resultEmrDocumentLast != null && resultEmrDocumentLast.Count() > 0)
                    {
                        if (DevExpress.XtraEditors.XtraMessageBox.Show("Tờ điều trị đã tồn tại văn bản ký, tiếp tục sẽ tự động Xóa văn bản ký hiện tại. Bạn có muốn tiếp tục?", Resources.ResourceMessage.ThongBao, MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            WaitingManager.Hide();
                            return;
                        }

                        foreach (var item in resultEmrDocumentLast)
                        {
                            var resultEmr = new BackendAdapter(new CommonParam()).Post<bool>("api/EmrDocument/Delete", ApiConsumers.EmrConsumer, item.ID, null);
                        }
                    }
                }

                result = MPS.MpsPrinter.Run(PrintData);
                #endregion
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        internal static string GenerateTempFileWithin(string fileName, string _extension = "")
        {
            try
            {
                string extension = !System.String.IsNullOrEmpty(_extension) ? _extension : Path.GetExtension(fileName);
                string pathDic = Path.Combine(Path.Combine(Path.Combine(Inventec.Common.TemplaterExport.ApplicationLocationStore.ApplicationPathLocal, "temp"), DateTime.Now.ToString("ddMMyyyy")), "Templates");
                if (!Directory.Exists(pathDic))
                {
                    Directory.CreateDirectory(pathDic);
                }
                return Path.Combine(pathDic, Guid.NewGuid().ToString() + extension);
            }
            catch (IOException exception)
            {
                Inventec.Common.Logging.LogSystem.Warn("Error create temp file: " + exception.Message, exception);
                return System.String.Empty;
            }
        }

        private V_HIS_TRACKING GetVHisTrackingByID(long trackingID)
        {
            V_HIS_TRACKING result = null;
            try
            {
                CommonParam param = new CommonParam();
                MOS.Filter.HisTrackingViewFilter filter = new MOS.Filter.HisTrackingViewFilter();
                filter.ID = trackingID;
                var apiResult = new BackendAdapter(param).Get<List<V_HIS_TRACKING>>(HisRequestUriStore.HIS_TRACKING_GETVIEW, ApiConsumers.MosConsumer, filter, param);
                if (apiResult != null)
                    result = apiResult.FirstOrDefault();
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void CreateThreadLoadData_Fast(object param)
        {
            Thread threadTreatment = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataTreatmentNewThread_Fast));
            Thread threadServiceReq = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataServiceReqNewThread_Fast));
            Thread threadMobaImpMests = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataMobaImpMestsNewThread_Fast));

            // threadTreatment.Priority = ThreadPriority.Normal;
            //threadServiceReq.Priority = ThreadPriority.Highest;

            try
            {
                threadServiceReq.Start(param);
                threadTreatment.Start(param);
                threadMobaImpMests.Start(param);

                threadTreatment.Join();
                threadServiceReq.Join();
                threadMobaImpMests.Join();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                threadTreatment.Abort();
                threadServiceReq.Abort();
                threadMobaImpMests.Abort();
            }
        }

        private void LoadDataTreatmentNewThread_Fast(object param)
        {
            try
            {
                LoadDataTreatment_Fast((long)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataTreatment_Fast(long treatmentId)
        {
            try
            {
                CommonParam param = new CommonParam();
                MOS.Filter.HisTreatmentBedRoomViewFilter bedFilter = new HisTreatmentBedRoomViewFilter();
                bedFilter.TREATMENT_ID = treatmentId;
                bedFilter.ORDER_FIELD = "CREATE_TIME";
                bedFilter.ORDER_DIRECTION = "DESC";
                var treatmentBedRooms = new BackendAdapter(param).Get<List<V_HIS_TREATMENT_BED_ROOM>>(HisRequestUriStore.HIS_TREATMENT_BED_ROOM_GETVIEW, ApiConsumers.MosConsumer, bedFilter, param);
                if (treatmentBedRooms != null && treatmentBedRooms.Count > 0)
                {
                    if (this._TrackingPrintsProcesss_Fast != null && this._TrackingPrintsProcesss_Fast.Count() > 0)
                    {
                        foreach (var item in this._TrackingPrintsProcesss_Fast)
                        {
                            var treatmentBedRoom = treatmentBedRooms.Where(o => o.ADD_TIME <= item.TRACKING_TIME && o.ROOM_ID == item.ROOM_ID).OrderByDescending(o => o.ADD_TIME).FirstOrDefault();
                            if (treatmentBedRoom != null)
                                _TreatmentBedRooms_Fast.Add(treatmentBedRoom);
                        }
                        //Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("_TreatmentBedRooms:", _TreatmentBedRooms));
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataServiceReqNewThread_Fast(object param)
        {
            try
            {
                LoadDataServiceReq_Fast((long)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataServiceReq_Fast(long treatmentId)
        {
            try
            {
                CommonParam param = new CommonParam();
                //danh sach yeu cau
                MOS.Filter.HisServiceReqFilter serviceReqFilterVT = new MOS.Filter.HisServiceReqFilter();
                serviceReqFilterVT.TREATMENT_ID = treatmentId;
                this._ServiceReqs_Fast = new BackendAdapter(param).Get<List<HIS_SERVICE_REQ>>(HisRequestUriStore.HIS_SERVICE_REQ_GET, ApiConsumers.MosConsumer, serviceReqFilterVT, param);
                if (this._ServiceReqs_Fast != null && this._ServiceReqs_Fast.Count > 0)
                {
                    foreach (var item in this._ServiceReqs_Fast)
                    {
                        if (!dicServiceReqs_Fast.ContainsKey(item.ID))
                        {
                            dicServiceReqs_Fast[item.ID] = new HIS_SERVICE_REQ();
                            dicServiceReqs_Fast[item.ID] = item;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataMobaImpMestsNewThread_Fast(object param)
        {
            try
            {
                LoadDataMobaImpMests_Fast((long)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataMobaImpMests_Fast(long treatmentId)
        {
            try
            {
                long keyViewMediMateTH = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKING_IS_MEDI_MATE_TH));
                if (keyViewMediMateTH != 1)
                    return;
                CommonParam paramCommon = new CommonParam();
                MOS.Filter.HisImpMestView2Filter filter = new MOS.Filter.HisImpMestView2Filter();

                filter.TDL_TREATMENT_ID = treatmentId;
                filter.IMP_MEST_TYPE_IDs = new List<long>(){
                        //IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL,
                       // IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL,
                        //IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL
                    };
                filter.ORDER_FIELD = "MODIFY_TIME";
                filter.ORDER_DIRECTION = "DESC";
                this._MobaImpMests_Fast = new Inventec.Common.Adapter.BackendAdapter(paramCommon).Get<List<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_2>>
                    ("api/HisImpMest/GetView2", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, paramCommon);

                if (this._MobaImpMests_Fast != null && this._MobaImpMests_Fast.Count > 0)
                {
                    int start = 0;
                    int count = this._MobaImpMests_Fast.Count;
                    //Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => count), count));
                    while (count > 0)
                    {
                        int limit = (count <= 100) ? count : 100;
                        var listSub = this._MobaImpMests_Fast.Skip(start).Take(limit).ToList();
                        List<long> _MobaImpMestsIds = new List<long>();
                        _MobaImpMestsIds = listSub.Select(p => p.ID).Distinct().ToList();
                        MOS.Filter.HisImpMestMedicineViewFilter impMestMedicineViewFilter = new MOS.Filter.HisImpMestMedicineViewFilter();
                        impMestMedicineViewFilter.IMP_MEST_IDs = _MobaImpMestsIds;
                        var impMestMed = new BackendAdapter(paramCommon).Get<List<V_HIS_IMP_MEST_MEDICINE>>("api/HisImpMestMedicine/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, impMestMedicineViewFilter, paramCommon);
                        if (impMestMed != null && impMestMed.Count > 0)
                        {
                            this._ImpMestMedicines_TL_Fast.AddRange(impMestMed);
                        }

                        long configQY7 = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKING_IS_MATERIAL));
                        if (configQY7 == 1)
                        {
                            MOS.Filter.HisImpMestMaterialViewFilter impMestMaterialViewFilter = new MOS.Filter.HisImpMestMaterialViewFilter();
                            impMestMaterialViewFilter.IMP_MEST_IDs = _MobaImpMestsIds.ToList();
                            var impMestMart = new BackendAdapter(paramCommon).Get<List<V_HIS_IMP_MEST_MATERIAL>>("api/HisImpMestMaterial/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, impMestMaterialViewFilter, paramCommon);
                            if (impMestMart != null && impMestMart.Count > 0)
                            {
                                this._ImpMestMaterial_TL_Fast.AddRange(impMestMart);
                            }
                        }

                        MOS.Filter.HisImpMestBloodViewFilter impMestBloodViewFilter = new MOS.Filter.HisImpMestBloodViewFilter();
                        impMestBloodViewFilter.IMP_MEST_IDs = _MobaImpMestsIds.ToList();
                        this._ImpMestBlood_TL_Fast = new BackendAdapter(paramCommon).Get<List<V_HIS_IMP_MEST_BLOOD>>("api/HisImpMestBlood/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, impMestBloodViewFilter, paramCommon);

                        start += 100;
                        count -= 100;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        /// <summary>
        /// Thuoc
        /// </summary>
        /// <param name="param"></param>
        private void CreateThreadLoadDataExpMest_Fast(object param)
        {
            Thread threadMedicine = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataExpMestMedicineNewThread_Fast));
            Thread threadMaterial = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataExpMestMaterialNewThread_Fast));

            //threadMedicine.Priority = ThreadPriority.Highest;
            try
            {
                threadMedicine.Start(param);
                threadMaterial.Start(param);

                threadMedicine.Join();
                threadMaterial.Join();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                threadMedicine.Abort();
                threadMaterial.Abort();
            }
        }

        //Thuoc trong kho
        private void LoadDataExpMestMedicineNewThread_Fast(object param)
        {
            try
            {
                LoadDataExpMestMedicine_Fast((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataExpMestMedicine_Fast(List<long> _expMestIds)
        {
            try
            {
                CommonParam param = new CommonParam();
                MOS.Filter.HisExpMestMedicineFilter filter = new HisExpMestMedicineFilter();
                filter.EXP_MEST_IDs = _expMestIds;
                this._ExpMestMedicines_Fast = new BackendAdapter(param).Get<List<HIS_EXP_MEST_MEDICINE>>("api/HisExpMestMedicine/Get", ApiConsumers.MosConsumer, filter, param);
                if (this._ExpMestMedicines_Fast != null && this._ExpMestMedicines_Fast.Count > 0)
                {
                    var dataGroups = this._ExpMestMedicines_Fast.Where(p => p.IS_NOT_PRES != 1).GroupBy(p => new { p.TDL_MEDICINE_TYPE_ID, p.EXP_MEST_ID, p.TUTORIAL }).Select(p => p.ToList()).ToList();
                    foreach (var item in dataGroups)
                    {
                        HIS_EXP_MEST_MEDICINE ado = new HIS_EXP_MEST_MEDICINE();
                        Inventec.Common.Mapper.DataObjectMapper.Map<HIS_EXP_MEST_MEDICINE>(ado, item[0]);
                        ado.AMOUNT = item.Sum(p => p.AMOUNT);
                        ado.TH_AMOUNT = item.Sum(p => p.TH_AMOUNT);
                        if (!dicExpMestMedicines_Fast.ContainsKey(ado.EXP_MEST_ID ?? 0))
                        {
                            dicExpMestMedicines_Fast[ado.EXP_MEST_ID ?? 0] = new List<HIS_EXP_MEST_MEDICINE>();
                            dicExpMestMedicines_Fast[ado.EXP_MEST_ID ?? 0].Add(ado);
                        }
                        else
                            dicExpMestMedicines_Fast[item[0].EXP_MEST_ID ?? 0].Add(ado);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        //VT Trong Kho
        private void LoadDataExpMestMaterialNewThread_Fast(object param)
        {
            try
            {
                LoadDataExpMestMaterial_Fast((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        void CheckConfigIsMaterial()
        {
            try
            {
                long configQY7 = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKING_IS_MATERIAL));
                if (configQY7 != 1)
                {
                    this._IsMaterial = true;
                }
                else
                {
                    this._IsMaterial = false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataExpMestMaterial_Fast(List<long> _expMestIds)
        {
            try
            {
                if (this._IsMaterial)
                    return;
                CommonParam param = new CommonParam();
                MOS.Filter.HisExpMestMaterialFilter filter = new HisExpMestMaterialFilter();
                filter.EXP_MEST_IDs = _expMestIds;
                this._ExpMestMaterials_Fast = new BackendAdapter(param).Get<List<HIS_EXP_MEST_MATERIAL>>("api/HisExpMestMaterial/Get", ApiConsumers.MosConsumer, filter, param);
                long keyPrintDoNotShowExpendMaterial = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKING_CRETATE_DoNotShowExpendMaterial));
                if (keyPrintDoNotShowExpendMaterial == 1)
                {
                    if (_ExpMestMaterials_Fast != null && _ExpMestMaterials_Fast.Count > 0)
                    {
                        _ExpMestMaterials_Fast = _ExpMestMaterials_Fast.Where(o => o.IS_EXPEND != 1).ToList();
                    }
                }

                if (this._ExpMestMaterials_Fast != null && this._ExpMestMaterials_Fast.Count > 0)
                {
                    var dataGroups = this._ExpMestMaterials_Fast.Where(p => p.IS_NOT_PRES != 1).GroupBy(p => new { p.TDL_MATERIAL_TYPE_ID, p.EXP_MEST_ID }).Select(p => p.ToList()).ToList();
                    foreach (var item in dataGroups)
                    {
                        HIS_EXP_MEST_MATERIAL ado = new HIS_EXP_MEST_MATERIAL();
                        Inventec.Common.Mapper.DataObjectMapper.Map<HIS_EXP_MEST_MATERIAL>(ado, item[0]);
                        ado.AMOUNT = item.Sum(p => p.AMOUNT);
                        ado.TH_AMOUNT = item.Sum(p => p.TH_AMOUNT);
                        if (!dicExpMestMaterials_Fast.ContainsKey(ado.EXP_MEST_ID ?? 0))
                        {
                            dicExpMestMaterials_Fast[ado.EXP_MEST_ID ?? 0] = new List<HIS_EXP_MEST_MATERIAL>();
                            dicExpMestMaterials_Fast[ado.EXP_MEST_ID ?? 0].Add(ado);
                        }
                        else
                            dicExpMestMaterials_Fast[item[0].EXP_MEST_ID ?? 0].Add(ado);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        /// <summary>
        ///Thread EXP_MEST && SERVICE_REQ_METY
        /// </summary>
        /// <param name="param"></param>
        private void CreateThreadByServiceReq_Fast(object param)
        {
            Thread threadExpMest = new Thread(new ParameterizedThreadStart(LoadDataExpMestNewThread_Fast));
            Thread threadServiceReqMety = new Thread(new ParameterizedThreadStart(LoadDataServiceReqMetyNewThread_Fast));
            Thread threadServiceReqMaty = new Thread(new ParameterizedThreadStart(LoadDataServiceReqMatyNewThread_Fast));
            Thread threadSereServ = new Thread(new ParameterizedThreadStart(LoadDataSereServNewThread_Fast));

            //threadExpMest.Priority = ThreadPriority.Normal;
            //threadServiceReqMety.Priority = ThreadPriority.Normal;
            //threadSereServ.Priority = ThreadPriority.Highest;
            try
            {
                threadExpMest.Start(param);
                threadServiceReqMety.Start(param);
                threadServiceReqMaty.Start(param);
                threadSereServ.Start(param);

                threadExpMest.Join();
                threadServiceReqMety.Join();
                threadServiceReqMaty.Join();
                threadSereServ.Join();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                threadExpMest.Abort();
                threadServiceReqMety.Abort();
                threadServiceReqMaty.Abort();
                threadSereServ.Abort();
            }
        }

        //Exp_mest
        private void LoadDataExpMestNewThread_Fast(object param)
        {
            try
            {
                LoadDataExpMest_Fast((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataExpMest_Fast(List<long> _serviceReqIds)
        {
            try
            {
                CommonParam param = new CommonParam();

                int startExpMest = 0;
                int countServiceReqId_ExpMest = _serviceReqIds.Count;
                List<HIS_EXP_MEST> expMestDatas = new List<HIS_EXP_MEST>();
                while (countServiceReqId_ExpMest > 0)
                {
                    int limit = (countServiceReqId_ExpMest <= 100) ? countServiceReqId_ExpMest : 100;
                    var listSub = _serviceReqIds.Skip(startExpMest).Take(limit).ToList();

                    MOS.Filter.HisExpMestFilter expMestFilter = new HisExpMestFilter();
                    expMestFilter.SERVICE_REQ_IDs = listSub;
                    var ssExpMestData = new BackendAdapter(param).Get<List<HIS_EXP_MEST>>(HisRequestUriStore.HIS_EXP_MEST_GET, ApiConsumers.MosConsumer, expMestFilter, param);
                    expMestDatas.AddRange(ssExpMestData);
                    startExpMest += 100;
                    countServiceReqId_ExpMest -= 100;
                }

                if (expMestDatas != null && expMestDatas.Count > 0)
                {
                    foreach (var item in expMestDatas)
                    {
                        if (!dicExpMests_Fast.ContainsKey(item.SERVICE_REQ_ID ?? 0))
                        {
                            dicExpMests_Fast[item.SERVICE_REQ_ID ?? 0] = new HIS_EXP_MEST();
                            dicExpMests_Fast[item.SERVICE_REQ_ID ?? 0] = (item);
                        }
                        else
                            dicExpMests_Fast[item.SERVICE_REQ_ID ?? 0] = (item);
                    }
                }
                this._ExpMests_Fast.AddRange(expMestDatas);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        //Thuoc Ngoai Kho
        private void LoadDataServiceReqMetyNewThread_Fast(object param)
        {
            try
            {
                LoadDataServiceReqMety_Fast((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataServiceReqMety_Fast(List<long> _serviceReqIds)
        {
            try
            {
                if (IsNotShowOutMediAndMate_Fast)
                {
                    CommonParam param = new CommonParam();
                    MOS.Filter.HisServiceReqMetyFilter metyFIlter = new HisServiceReqMetyFilter();
                    metyFIlter.SERVICE_REQ_IDs = _serviceReqIds;
                    var metyDatas = new BackendAdapter(param).Get<List<HIS_SERVICE_REQ_METY>>("api/HisServiceReqMety/Get", ApiConsumers.MosConsumer, metyFIlter, param);
                    if (metyDatas != null && metyDatas.Count > 0)
                    {
                        foreach (var item in metyDatas)
                        {
                            if (!dicServiceReqMetys_Fast.ContainsKey(item.SERVICE_REQ_ID))
                            {
                                dicServiceReqMetys_Fast[item.SERVICE_REQ_ID] = new List<HIS_SERVICE_REQ_METY>();
                                dicServiceReqMetys_Fast[item.SERVICE_REQ_ID].Add(item);
                            }
                            else
                                dicServiceReqMetys_Fast[item.SERVICE_REQ_ID].Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        //VT Ngoai Kho
        private void LoadDataServiceReqMatyNewThread_Fast(object param)
        {
            try
            {
                LoadDataServiceReqMaty_Fast((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataServiceReqMaty_Fast(List<long> _serviceReqIds)
        {
            try
            {
                if (IsNotShowOutMediAndMate_Fast)
                {
                    CommonParam param = new CommonParam();
                    MOS.Filter.HisServiceReqMatyFilter matyFIlter = new HisServiceReqMatyFilter();
                    matyFIlter.SERVICE_REQ_IDs = _serviceReqIds;
                    var matyDatas = new BackendAdapter(param).Get<List<HIS_SERVICE_REQ_MATY>>("api/HisServiceReqMaty/Get", ApiConsumers.MosConsumer, matyFIlter, param);
                    if (matyDatas != null && matyDatas.Count > 0)
                    {
                        foreach (var item in matyDatas)
                        {
                            if (!dicServiceReqMatys_Fast.ContainsKey(item.SERVICE_REQ_ID))
                            {
                                dicServiceReqMatys_Fast[item.SERVICE_REQ_ID] = new List<HIS_SERVICE_REQ_MATY>();
                                dicServiceReqMatys_Fast[item.SERVICE_REQ_ID].Add(item);
                            }
                            else
                                dicServiceReqMatys_Fast[item.SERVICE_REQ_ID].Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void LoadDataSereServNewThread_Fast(object param)
        {
            try
            {
                LoadDataSereServ_Fast((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataSereServ_Fast(List<long> _serviceReqIds)
        {
            try
            {
                if (_serviceReqIds == null || _serviceReqIds.Count <= 0)
                    return;
                List<V_HIS_SERVICE> hiservice = BackendDataWorker.Get<V_HIS_SERVICE>().Where(o => o.IS_NOT_SHOW_TRACKING == 1).ToList();
                CommonParam param = new CommonParam();
                MOS.Filter.HisSereServFilter hisSereServFilterVT = new MOS.Filter.HisSereServFilter();
                hisSereServFilterVT.SERVICE_REQ_IDs = _serviceReqIds;
                var datas = new BackendAdapter(param).Get<List<HIS_SERE_SERV>>(HisRequestUriStore.HIS_SERE_SERV_GET, ApiConsumers.MosConsumer, hisSereServFilterVT, param);

                if (datas != null && datas.Count > 0)
                {
                    if (hiservice != null && hiservice.Count() > 0)
                    {
                        datas = datas.Where(o => hiservice.All(p => p.ID != o.SERVICE_ID)).ToList();
                    }
                    if (BloodPresOption_Fast)
                    {
                        datas = datas.Where(o => o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).ToList();
                    }
                }

                if (datas != null && datas.Count > 0)
                {
                    this._SereServs_Fast.AddRange(datas);

                    if (hiservice != null && hiservice.Count() > 0)
                    {
                        datas = datas.Where(o => hiservice.All(p => p.ID != o.SERVICE_ID)).ToList();
                    }
                    foreach (var item in datas)
                    {
                        if (!dicSereServs_Fast.ContainsKey(item.SERVICE_REQ_ID ?? 0))
                        {
                            dicSereServs_Fast[item.SERVICE_REQ_ID ?? 0] = new List<HIS_SERE_SERV>();
                            dicSereServs_Fast[item.SERVICE_REQ_ID ?? 0].Add(item);
                        }
                        else
                            dicSereServs_Fast[item.SERVICE_REQ_ID ?? 0].Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
