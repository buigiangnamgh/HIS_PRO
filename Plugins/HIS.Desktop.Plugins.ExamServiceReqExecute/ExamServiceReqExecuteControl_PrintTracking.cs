using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EMR.EFMODEL.DataModels;
using EMR.Filter;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.Plugins.HisTrackingList.Event;
using HIS.Desktop.Print;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Core;
using Inventec.Desktop.Common.Message;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using SDA.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.ExamServiceReqExecute
{
    public partial class ExamServiceReqExecuteControl : UserControlBase
    {
        void PrintProcess62(PrintType printType)
        {
            try
            {
                Inventec.Common.RichEditor.RichEditorStore richEditorMain = new Inventec.Common.RichEditor.RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, HIS.Desktop.LocalStorage.ConfigSystem.ConfigSystems.URI_API_SAR, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetLanguage(), HIS.Desktop.LocalStorage.Location.PrintStoreLocation.PrintTemplatePath);

                switch (printType)
                {
                    case PrintType.IN_TO_DIEU_TRI:
                        richEditorMain.RunPrintTemplate(PrintTypeCodeStore.PRINT_TYPE_CODE__BIEUMAU__PHIEU_YEU_CAU_IN_TO_DIEU_TRI__MPS000062, null, DelegateRunPrinter62, true);
                        break;
                    default:
                        break;
                }
                //Inventec.Desktop.Common.Message.MessageManager.Show(this.ParentForm, new CommonParam(), true);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        bool DelegateRunPrinter62(string printTypeCode, string fileName)
        {
            bool result = false;
            try
            {
                switch (printTypeCode)
                {
                    case PrintTypeCodeStore.PRINT_TYPE_CODE__BIEUMAU__PHIEU_YEU_CAU_IN_TO_DIEU_TRI__MPS000062:
                        Mps000062(printTypeCode, fileName, ref result);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal HIS_TRACKING currentTracking { get; set; }

        List<V_HIS_TREATMENT_BED_ROOM> _TreatmentBedRooms { get; set; }

        List<HIS_SERVICE_REQ> _ServiceReqs { get; set; }
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReqs { get; set; }

        List<HIS_SERE_SERV> _SereServs { get; set; }
        Dictionary<long, List<HIS_SERE_SERV>> dicSereServs { get; set; }

        List<HIS_EXP_MEST> _ExpMests { get; set; }
        Dictionary<long, HIS_EXP_MEST> dicExpMests { get; set; }

        List<HIS_EXP_MEST_MEDICINE> _ExpMestMedicines { get; set; }
        Dictionary<long, List<HIS_EXP_MEST_MEDICINE>> dicExpMestMedicines { get; set; }

        List<HIS_EXP_MEST_MATERIAL> _ExpMestMaterials { get; set; }
        Dictionary<long, List<HIS_EXP_MEST_MATERIAL>> dicExpMestMaterials { get; set; }

        Dictionary<long, List<HIS_SERVICE_REQ_METY>> dicServiceReqMetys { get; set; }
        Dictionary<long, List<HIS_SERVICE_REQ_MATY>> dicServiceReqMatys { get; set; }

        List<HIS_SERE_SERV_EXT> _SereServExts { get; set; }
        //suat an
        List<V_HIS_SERE_SERV_RATION> _SereServRation { get; set; }

        //thuốc, vật tư trả lại
        internal List<V_HIS_IMP_MEST_2> _MobaImpMests { get; set; }
        internal List<V_HIS_IMP_MEST_MEDICINE> _ImpMestMedicines_TL { get; set; }
        internal List<V_HIS_IMP_MEST_MATERIAL> _ImpMestMaterial_TL { get; set; }
        internal List<V_HIS_IMP_MEST_BLOOD> _ImpMestBlood_TL { get; set; }

        List<V_HIS_EXP_MEST_BLTY_REQ_2> HisExpMestBltyReq2 { get; set; }

        bool IsNotShowOutMediAndMate = false;
        bool BloodPresOption { get; set; }

        List<V_HIS_TRACKING> _TrackingPrintsProcesss;

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
                _TrackingPrintsProcesss = new List<V_HIS_TRACKING>();
                _TrackingPrintsProcesss.Add(currentTrackingForPrint);

                _TreatmentBedRooms = new List<V_HIS_TREATMENT_BED_ROOM>();

                _ServiceReqs = new List<HIS_SERVICE_REQ>();
                dicServiceReqs = new Dictionary<long, HIS_SERVICE_REQ>();

                _SereServs = new List<HIS_SERE_SERV>();
                dicSereServs = new Dictionary<long, List<HIS_SERE_SERV>>();

                _ExpMests = new List<HIS_EXP_MEST>();
                dicExpMests = new Dictionary<long, HIS_EXP_MEST>();

                _ExpMestMedicines = new List<HIS_EXP_MEST_MEDICINE>();
                dicExpMestMedicines = new Dictionary<long, List<HIS_EXP_MEST_MEDICINE>>();

                _ExpMestMaterials = new List<HIS_EXP_MEST_MATERIAL>();
                dicExpMestMaterials = new Dictionary<long, List<HIS_EXP_MEST_MATERIAL>>();

                dicServiceReqMetys = new Dictionary<long, List<HIS_SERVICE_REQ_METY>>();

                dicServiceReqMatys = new Dictionary<long, List<HIS_SERVICE_REQ_MATY>>();

                HisExpMestBltyReq2 = new List<V_HIS_EXP_MEST_BLTY_REQ_2>();

                IsNotShowOutMediAndMate = (HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>("HIS.Desktop.Plugins.TrackingPrint.IsNotShowOutMediAndMate") == "1");

                this._SereServExts = new List<HIS_SERE_SERV_EXT>();

                this._SereServRation = new List<V_HIS_SERE_SERV_RATION>();

                //thuốc, vật tư trả lại
                this._MobaImpMests = new List<V_HIS_IMP_MEST_2>();
                this._ImpMestMedicines_TL = new List<V_HIS_IMP_MEST_MEDICINE>();
                this._ImpMestMaterial_TL = new List<V_HIS_IMP_MEST_MATERIAL>();
                this._ImpMestBlood_TL = new List<V_HIS_IMP_MEST_BLOOD>();

                if (this.treatmentId > 0)
                {
                    CreateThreadLoadData62(this.treatmentId);
                }

                int start = 0;
                int count = this._ServiceReqs.Count;
                while (count > 0)
                {
                    int limit = (count <= 100) ? count : 100;
                    var listSub = this._ServiceReqs.Skip(start).Take(limit).ToList();
                    List<long> _serviceReqIds = new List<long>();
                    _serviceReqIds = listSub.Select(p => p.ID).Distinct().ToList();

                    CreateThreadByServiceReq62(_serviceReqIds);

                    start += 100;
                    count -= 100;
                }

                if (this._ExpMests != null && this._ExpMests.Count > 0)
                {
                    int startExpMest = 0;
                    int countExpMest = _ExpMests.Count;
                    while (countExpMest > 0)
                    {
                        int limit = (countExpMest <= 100) ? countExpMest : 100;
                        var listSub = this._ExpMests.Skip(startExpMest).Take(limit).ToList();
                        List<long> _serviceReqIds = new List<long>();
                        CreateThreadLoadDataExpMest62(listSub.Select(p => p.ID).Distinct().ToList());
                        startExpMest += 100;
                        countExpMest -= 100;
                    }
                }

                if (this._SereServs != null && this._SereServs.Count > 0)
                {
                    int startSS = 0;
                    int countSS = this._SereServs.Count;
                    while (countSS > 0)
                    {
                        int limit = (countSS <= 100) ? countSS : 100;
                        var listSub = this._SereServs.Skip(startSS).Take(limit).ToList();
                        List<long> _sereServIds = new List<long>();
                        _sereServIds = listSub.Select(p => p.ID).Distinct().ToList();

                        //Get SERE_SERV_EXT
                        MOS.Filter.HisSereServExtFilter sereServExtFilter = new MOS.Filter.HisSereServExtFilter();
                        sereServExtFilter.SERE_SERV_IDs = _sereServIds;

                        var dataSS_EXTs = new BackendAdapter(param).Get<List<HIS_SERE_SERV_EXT>>("/api/HisSereServExt/Get", ApiConsumers.MosConsumer, sereServExtFilter, param);
                        if (dataSS_EXTs != null && dataSS_EXTs.Count > 0)
                        {
                            this._SereServExts.AddRange(dataSS_EXTs);
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
                this._SereServRation.AddRange(ssRation);

                #endregion

                #region Máu chỉ định

                if (BloodPresOption)
                {
                    CommonParam param_ = new CommonParam();
                    MOS.Filter.HisExpMestBltyReqView2Filter filter = new HisExpMestBltyReqView2Filter();
                    filter.TDL_TREATMENT_ID = this.treatmentId;
                    filter.TRACKING_ID = currentTracking.ID;
                    HisExpMestBltyReq2 = new BackendAdapter(param_).Get<List<V_HIS_EXP_MEST_BLTY_REQ_2>>("/api/HisExpMestBltyReq/GetView2", ApiConsumers.MosConsumer, filter, HIS.Desktop.Controls.Session.SessionManager.ActionLostToken, param_);
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
                Inventec.Common.Logging.LogSystem.Debug("Begin EmrGenerateProcessor");
                Inventec.Common.SignLibrary.ADO.InputADO inputADO = new HIS.Desktop.Plugins.Library.EmrGenerate.EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((this.treatment != null ? this.treatment.TREATMENT_CODE : ""), printTypeCode, this.moduleData != null ? this.moduleData.RoomId : 0);
                Inventec.Common.Logging.LogSystem.Debug("end EmrGenerateProcessor");
                long keyPrintMerge = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKING_IS_PRINT_MERGE));

                if (keyPrintMerge == 1)
                {
                    string uniqueTime = "";// (_TrackingPrints != null && _TrackingPrints.Count > 0) ? _TrackingPrints[0].TRACKING_TIME + "" : "";
                    inputADO.MergeCode = String.Format("{0}_{1}_{2}", printTypeCode, uniqueTime, (this.treatment != null ? this.treatment.TREATMENT_CODE : ""));//TODO
                }


                if (_TrackingPrintsProcesss != null && _TrackingPrintsProcesss.Count > 0 && _TrackingPrintsProcesss[0].SHEET_ORDER != null)
                {
                    inputADO.DocumentName += " " + "(" + _TrackingPrintsProcesss[0].SHEET_ORDER.ToString() + ")";
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
                Inventec.Common.Logging.LogSystem.Debug("begin Mps000062PDO");
                MPS.Processor.Mps000062.PDO.Mps000062PDO mps000062RDO = new MPS.Processor.Mps000062.PDO.Mps000062PDO(
                this.treatment,
                _TreatmentBedRooms,
                _TrackingPrintsProcesss,
                _Dhsts,
                dicServiceReqs,
                dicSereServs,
                dicExpMests,
                dicExpMestMedicines,
                dicExpMestMaterials,
                dicServiceReqMetys,
                dicServiceReqMatys,
                _Cares,
                _CareDetails,
                singleKey,
                BackendDataWorker.Get<HIS_ICD>(),
                BackendDataWorker.Get<V_HIS_MEDICINE_TYPE>(),
                BackendDataWorker.Get<V_HIS_MATERIAL_TYPE>(),
                BackendDataWorker.Get<HIS_SERVICE_TYPE>(),
                this._SereServExts,
                BackendDataWorker.Get<HIS_MEDICINE_USE_FORM>(),
                this._SereServRation,
                this._MobaImpMests,
                this._ImpMestMedicines_TL,
                this._ImpMestMaterial_TL,
                HisExpMestBltyReq2,
                BackendDataWorker.Get<V_HIS_SERVICE>().Where(o => this._SereServs.Select(p => p.SERVICE_ID).Contains(o.ID)).ToList(),
                this._ImpMestBlood_TL,
                PatientTypeAlter,
                listMedicineLine,
                listDosage,
                selectedBedLog
                );
                Inventec.Common.Logging.LogSystem.Debug("End Mps000062PDO");
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
                if (documents != null && documents.Count() > 0)
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
                WaitingManager.Hide();
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

        private void CreateThreadLoadData62(object param)
        {
            Thread threadTreatment = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataTreatmentNewThread62));
            Thread threadServiceReq = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataServiceReqNewThread));
            Thread threadMobaImpMests = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataMobaImpMestsNewThread));

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

        private void LoadDataTreatmentNewThread62(object param)
        {
            try
            {
                LoadDataTreatment62((long)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataTreatment62(long treatmentId)
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
                    if (this._TrackingPrintsProcesss != null && this._TrackingPrintsProcesss.Count() > 0)
                    {
                        foreach (var item in this._TrackingPrintsProcesss)
                        {
                            var treatmentBedRoom = treatmentBedRooms.Where(o => o.ADD_TIME <= item.TRACKING_TIME && o.ROOM_ID == item.ROOM_ID).OrderByDescending(o => o.ADD_TIME).FirstOrDefault();
                            if (treatmentBedRoom != null)
                                _TreatmentBedRooms.Add(treatmentBedRoom);
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


        private void LoadDataMobaImpMestsNewThread(object param)
        {
            try
            {
                LoadDataMobaImpMests((long)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataMobaImpMests(long treatmentId)
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
                this._MobaImpMests = new Inventec.Common.Adapter.BackendAdapter(paramCommon).Get<List<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_2>>
                    ("api/HisImpMest/GetView2", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, paramCommon);

                if (this._MobaImpMests != null && this._MobaImpMests.Count > 0)
                {
                    int start = 0;
                    int count = this._MobaImpMests.Count;
                    //Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => count), count));
                    while (count > 0)
                    {
                        int limit = (count <= 100) ? count : 100;
                        var listSub = this._MobaImpMests.Skip(start).Take(limit).ToList();
                        List<long> _MobaImpMestsIds = new List<long>();
                        _MobaImpMestsIds = listSub.Select(p => p.ID).Distinct().ToList();
                        MOS.Filter.HisImpMestMedicineViewFilter impMestMedicineViewFilter = new MOS.Filter.HisImpMestMedicineViewFilter();
                        impMestMedicineViewFilter.IMP_MEST_IDs = _MobaImpMestsIds;
                        var impMestMed = new BackendAdapter(paramCommon).Get<List<V_HIS_IMP_MEST_MEDICINE>>("api/HisImpMestMedicine/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, impMestMedicineViewFilter, paramCommon);
                        if (impMestMed != null && impMestMed.Count > 0)
                        {
                            this._ImpMestMedicines_TL.AddRange(impMestMed);
                        }

                        long configQY7 = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKING_IS_MATERIAL));
                        if (configQY7 == 1)
                        {
                            MOS.Filter.HisImpMestMaterialViewFilter impMestMaterialViewFilter = new MOS.Filter.HisImpMestMaterialViewFilter();
                            impMestMaterialViewFilter.IMP_MEST_IDs = _MobaImpMestsIds.ToList();
                            var impMestMart = new BackendAdapter(paramCommon).Get<List<V_HIS_IMP_MEST_MATERIAL>>("api/HisImpMestMaterial/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, impMestMaterialViewFilter, paramCommon);
                            if (impMestMart != null && impMestMart.Count > 0)
                            {
                                this._ImpMestMaterial_TL.AddRange(impMestMart);
                            }
                        }

                        MOS.Filter.HisImpMestBloodViewFilter impMestBloodViewFilter = new MOS.Filter.HisImpMestBloodViewFilter();
                        impMestBloodViewFilter.IMP_MEST_IDs = _MobaImpMestsIds.ToList();
                        this._ImpMestBlood_TL = new BackendAdapter(paramCommon).Get<List<V_HIS_IMP_MEST_BLOOD>>("api/HisImpMestBlood/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, impMestBloodViewFilter, paramCommon);

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
        private void CreateThreadLoadDataExpMest62(object param)
        {
            Thread threadMedicine = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataExpMestMedicineNewThread62));
            Thread threadMaterial = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(LoadDataExpMestMaterialNewThread62));

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
        private void LoadDataExpMestMedicineNewThread62(object param)
        {
            try
            {
                LoadDataExpMestMedicine62((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataExpMestMedicine62(List<long> _expMestIds)
        {
            try
            {
                CommonParam param = new CommonParam();
                MOS.Filter.HisExpMestMedicineFilter filter = new HisExpMestMedicineFilter();
                filter.EXP_MEST_IDs = _expMestIds;
                this._ExpMestMedicines = new BackendAdapter(param).Get<List<HIS_EXP_MEST_MEDICINE>>("api/HisExpMestMedicine/Get", ApiConsumers.MosConsumer, filter, param);
                if (this._ExpMestMedicines != null && this._ExpMestMedicines.Count > 0)
                {
                    var dataGroups = this._ExpMestMedicines.Where(p => p.IS_NOT_PRES != 1).GroupBy(p => new { p.TDL_MEDICINE_TYPE_ID, p.EXP_MEST_ID, p.TUTORIAL }).Select(p => p.ToList()).ToList();
                    foreach (var item in dataGroups)
                    {
                        HIS_EXP_MEST_MEDICINE ado = new HIS_EXP_MEST_MEDICINE();
                        Inventec.Common.Mapper.DataObjectMapper.Map<HIS_EXP_MEST_MEDICINE>(ado, item[0]);
                        ado.AMOUNT = item.Sum(p => p.AMOUNT);
                        ado.TH_AMOUNT = item.Sum(p => p.TH_AMOUNT);
                        if (!dicExpMestMedicines.ContainsKey(ado.EXP_MEST_ID ?? 0))
                        {
                            dicExpMestMedicines[ado.EXP_MEST_ID ?? 0] = new List<HIS_EXP_MEST_MEDICINE>();
                            dicExpMestMedicines[ado.EXP_MEST_ID ?? 0].Add(ado);
                        }
                        else
                            dicExpMestMedicines[item[0].EXP_MEST_ID ?? 0].Add(ado);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        //VT Trong Kho
        private void LoadDataExpMestMaterialNewThread(object param)
        {
            try
            {
                LoadDataExpMestMaterial((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataExpMestMaterial(List<long> _expMestIds)
        {
            try
            {
                if (this._IsMaterial)
                    return;
                CommonParam param = new CommonParam();
                MOS.Filter.HisExpMestMaterialFilter filter = new HisExpMestMaterialFilter();
                filter.EXP_MEST_IDs = _expMestIds;
                this._ExpMestMaterials = new BackendAdapter(param).Get<List<HIS_EXP_MEST_MATERIAL>>("api/HisExpMestMaterial/Get", ApiConsumers.MosConsumer, filter, param);
                long keyPrintDoNotShowExpendMaterial = Inventec.Common.TypeConvert.Parse.ToInt64(HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(ConfigKeyss.DBCODE__HIS_DESKTOP_PLUGINS_TRACKING_CRETATE_DoNotShowExpendMaterial));
                if (keyPrintDoNotShowExpendMaterial == 1)
                {
                    if (_ExpMestMaterials != null && _ExpMestMaterials.Count > 0)
                    {
                        _ExpMestMaterials = _ExpMestMaterials.Where(o => o.IS_EXPEND != 1).ToList();
                    }
                }

                if (this._ExpMestMaterials != null && this._ExpMestMaterials.Count > 0)
                {
                    var dataGroups = this._ExpMestMaterials.Where(p => p.IS_NOT_PRES != 1).GroupBy(p => new { p.TDL_MATERIAL_TYPE_ID, p.EXP_MEST_ID }).Select(p => p.ToList()).ToList();
                    foreach (var item in dataGroups)
                    {
                        HIS_EXP_MEST_MATERIAL ado = new HIS_EXP_MEST_MATERIAL();
                        Inventec.Common.Mapper.DataObjectMapper.Map<HIS_EXP_MEST_MATERIAL>(ado, item[0]);
                        ado.AMOUNT = item.Sum(p => p.AMOUNT);
                        ado.TH_AMOUNT = item.Sum(p => p.TH_AMOUNT);
                        if (!dicExpMestMaterials.ContainsKey(ado.EXP_MEST_ID ?? 0))
                        {
                            dicExpMestMaterials[ado.EXP_MEST_ID ?? 0] = new List<HIS_EXP_MEST_MATERIAL>();
                            dicExpMestMaterials[ado.EXP_MEST_ID ?? 0].Add(ado);
                        }
                        else
                            dicExpMestMaterials[item[0].EXP_MEST_ID ?? 0].Add(ado);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        bool _IsMaterial;

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

        /// <summary>
        ///Thread EXP_MEST && SERVICE_REQ_METY
        /// </summary>
        /// <param name="param"></param>
        private void CreateThreadByServiceReq62(object param)
        {
            Thread threadExpMest = new Thread(new ParameterizedThreadStart(LoadDataExpMestNewThread62));
            Thread threadServiceReqMety = new Thread(new ParameterizedThreadStart(LoadDataServiceReqMetyNewThread));
            Thread threadServiceReqMaty = new Thread(new ParameterizedThreadStart(LoadDataServiceReqMatyNewThread));
            Thread threadSereServ = new Thread(new ParameterizedThreadStart(LoadDataSereServNewThread62));

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
        private void LoadDataExpMestNewThread62(object param)
        {
            try
            {
                LoadDataExpMest62((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataExpMest62(List<long> _serviceReqIds)
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
                        if (!dicExpMests.ContainsKey(item.SERVICE_REQ_ID ?? 0))
                        {
                            dicExpMests[item.SERVICE_REQ_ID ?? 0] = new HIS_EXP_MEST();
                            dicExpMests[item.SERVICE_REQ_ID ?? 0] = (item);
                        }
                        else
                            dicExpMests[item.SERVICE_REQ_ID ?? 0] = (item);
                    }
                }
                this._ExpMests.AddRange(expMestDatas);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void LoadDataSereServNewThread62(object param)
        {
            try
            {
                LoadDataSereServ62((List<long>)param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataSereServ62(List<long> _serviceReqIds)
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
                    if (BloodPresOption)
                    {
                        datas = datas.Where(o => o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).ToList();
                    }
                }

                if (datas != null && datas.Count > 0)
                {
                    this._SereServs.AddRange(datas);

                    if (hiservice != null && hiservice.Count() > 0)
                    {
                        datas = datas.Where(o => hiservice.All(p => p.ID != o.SERVICE_ID)).ToList();
                    }
                    foreach (var item in datas)
                    {
                        if (!dicSereServs.ContainsKey(item.SERVICE_REQ_ID ?? 0))
                        {
                            dicSereServs[item.SERVICE_REQ_ID ?? 0] = new List<HIS_SERE_SERV>();
                            dicSereServs[item.SERVICE_REQ_ID ?? 0].Add(item);
                        }
                        else
                            dicSereServs[item.SERVICE_REQ_ID ?? 0].Add(item);
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
