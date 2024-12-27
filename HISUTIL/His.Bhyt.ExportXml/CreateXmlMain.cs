using His.Bhyt.ExportXml.Base;
using His.Bhyt.ExportXml.CheckIn.XML;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml
{
    public class CreateXmlMain
    {
        InputADO entity;
        InputGroupADO groupData;
        QD917.Processor.HoSoProcessor _HoSoProcessor917;
        QD4210.Processor.HoSoProcessor _HoSoProcessor4210;
        CV2076.Processor.HoSoChungTuProcessor _HoSoProcessor2076;
        CheckIn.Processor.CheckInProcessor _CheckInProcessor;
        QD4210.Processor.HoSoGroupProcessor _HoSoGroupProcessor4210;
        string MessageError;
        string CodeError;
        bool IsCollinearNoLockFee;

        public CreateXmlMain(InputADO data)
        {
            this.entity = data;
        }

        public CreateXmlMain(InputGroupADO data)
        {
            this.groupData = data;
        }

        public bool Run917()
        {
            bool result = false;
            try
            {
                if (Check(true, true))
                {
                    this._HoSoProcessor917 = new QD917.Processor.HoSoProcessor(this.entity);
                    result = this._HoSoProcessor917.Processor();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;

            }
            return result;
        }

        public MemoryStream Run917Plus()
        {
            MemoryStream result = null;
            try
            {
                if (Check(true, false))
                {
                    this._HoSoProcessor917 = new QD917.Processor.HoSoProcessor(this.entity);
                    result = this._HoSoProcessor917.ProcessorPlus();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;

            }
            return result;
        }

        public bool Run4210()
        {
            bool result = false;
            try
            {
                string error = "";
                result = Run4210(ref error);
                if (!String.IsNullOrWhiteSpace(error))
                {
                    Inventec.Common.Logging.LogSystem.Error("Run4210: " + error);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool Run4210(ref string messageError)
        {
            bool result = false;
            try
            {
                bool isCheckLatentTuberculosis = true;
                if (Check(false, true, isCheckLatentTuberculosis))
                {
                    this._HoSoProcessor4210 = new QD4210.Processor.HoSoProcessor(this.entity);
                    result = this._HoSoProcessor4210.Processor();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(MessageError))
                {
                    string codeError = "";
                    if (!String.IsNullOrWhiteSpace(CodeError))
                    {
                        codeError = string.Format("(Mã y lệnh: {0})", CodeError);
                    }
                    messageError = string.Format("{0}{1}", MessageError, codeError);
                    Inventec.Common.Logging.LogSystem.Error(string.Format("Run4210: {0}", messageError));
                }
            }
            return result;
        }

        public MemoryStream Run4210Plus()
        {
            MemoryStream result = null;
            try
            {
                string error = "";
                result = Run4210Plus(ref error);
                if (!String.IsNullOrWhiteSpace(error))
                {
                    Inventec.Common.Logging.LogSystem.Error("Run4210: " + error);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public MemoryStream Run4210Plus(ref string messageError)
        {
            MemoryStream result = null;
            try
            {
                bool isCheckLatentTuberculosis = true;
                if (Check(false, false, isCheckLatentTuberculosis))
                {
                    this._HoSoProcessor4210 = new QD4210.Processor.HoSoProcessor(this.entity);
                    result = this._HoSoProcessor4210.ProcessorPlus();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(MessageError))
                {
                    string codeError = "";
                    if (!String.IsNullOrWhiteSpace(CodeError))
                    {
                        codeError = string.Format("(Mã y lệnh: {0})", CodeError);
                    }
                    messageError = string.Format("{0}{1}", MessageError, codeError);
                    Inventec.Common.Logging.LogSystem.Error(string.Format("Run4210: {0}", messageError));
                }
            }
            return result;
        }

        public MemoryStream Run4210PlusCollinear(ref string messageError)
        {
            MemoryStream result = null;
            try
            {
                bool isCheckLatentTuberculosis = true;
                IsCollinearNoLockFee = true;
                if (Check(false, false, isCheckLatentTuberculosis))
                {
                    this._HoSoProcessor4210 = new QD4210.Processor.HoSoProcessor(this.entity);
                    result = this._HoSoProcessor4210.ProcessorPlus();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(MessageError))
                {
                    string codeError = "";
                    if (!String.IsNullOrWhiteSpace(CodeError))
                    {
                        codeError = string.Format("(Mã y lệnh: {0})", CodeError);
                    }
                    messageError = string.Format("{0}{1}", MessageError, codeError);
                    Inventec.Common.Logging.LogSystem.Error(string.Format("Run4210: {0}", messageError));
                }
            }
            return result;
        }

        public string Run4210Path()
        {
            string result = "";
            try
            {
                string error = "";
                result = Run4210Path(ref error);
                if (!String.IsNullOrWhiteSpace(error))
                {
                    Inventec.Common.Logging.LogSystem.Error("Run4210Path: " + error);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        public string Run4210Path(ref string messageError)
        {
            string result = "";
            try
            {
                string error = "";
                string code = "";
                result = Run4210Path(ref error, ref code);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(MessageError))
                {
                    string codeError = "";
                    if (!String.IsNullOrWhiteSpace(CodeError))
                    {
                        codeError = string.Format("(Mã y lệnh: {0})", CodeError);
                    }
                    messageError = string.Format("{0}{1}", MessageError, codeError);
                    Inventec.Common.Logging.LogSystem.Error(string.Format("Run4210: {0}", messageError));
                }
            }
            return result;
        }

        public string Run4210Path(ref string messageError, ref string codeError)
        {
            string result = "";
            try
            {
                bool isCheckLatentTuberculosis = true;
                if (Check(false, true, isCheckLatentTuberculosis))
                {
                    this._HoSoProcessor4210 = new QD4210.Processor.HoSoProcessor(this.entity);
                    result = this._HoSoProcessor4210.ProcessorPath();

                    if (String.IsNullOrWhiteSpace(result))
                    {
                        messageError = "Lỗi trong quá trình tạo file xml";
                    }
                }

                messageError = MessageError;
                if (!String.IsNullOrWhiteSpace(CodeError))
                {
                    codeError = "Mã y lệnh:" + CodeError;
                }
            }
            catch (Exception ex)
            {
                messageError = MessageError;
                codeError = CodeError;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        public string Run4210PathCollinear(ref string messageError)
        {
            string result = "";
            try
            {
                IsCollinearNoLockFee = true;
                string error = "";
                string code = "";
                result = Run4210Path(ref error, ref code);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(MessageError))
                {
                    string codeError = "";
                    if (!String.IsNullOrWhiteSpace(CodeError))
                    {
                        codeError = string.Format("(Mã y lệnh: {0})", CodeError);
                    }
                    messageError = string.Format("{0}{1}", MessageError, codeError);
                    Inventec.Common.Logging.LogSystem.Error(string.Format("Run4210: {0}", messageError));
                }
            }
            return result;
        }

        private bool Check(bool isCheckHeinApprovalSereServ, bool checkPath, bool isCheckLatentTuberculosis = false)
        {
            bool valid = true;
            try
            {
                if (entity == null)
                {
                    MessageError = "Lỗi dữ liệu khởi tạo";
                    throw new NullReferenceException("InputADO truyen vao null: ");
                }

                if (entity.HeinApproval == null && (entity.HeinApprovals == null || entity.HeinApprovals.Count == 0))
                {
                    MessageError = "Hồ sơ chưa giám định bhyt";
                    throw new NullReferenceException("HeinApprovalBhyt and entity.HeinApprovals is null: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => entity.Treatment), entity.Treatment));
                }

                //luôn sắp xếp lại để lấy theo thông tin diện điều trị cuối cùng
                //if (entity.HeinApproval == null)
                entity.HeinApproval = entity.HeinApprovals.OrderByDescending(o => o.EXECUTE_TIME ?? 0).ThenByDescending(o => o.ID).FirstOrDefault();

                if (entity.ListSereServ == null || entity.ListSereServ.Count == 0)
                {
                    MessageError = "Hồ sơ không có dịch vụ";
                    throw new NullReferenceException("ListSereServ is null or empty: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => entity.Treatment), entity.Treatment));
                }

                if (entity.Treatment == null)
                {
                    MessageError = "Không xác định được hồ sơ điều trị";
                    throw new NullReferenceException("Treatment is null: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => entity.Treatment), entity.Treatment));
                }

                if (!IsCollinearNoLockFee)
                {
                    if (!entity.HeinApproval.EXECUTE_TIME.HasValue)
                    {
                        MessageError = "Hồ sơ chưa có thời gian giám định bhyt";
                        throw new NullReferenceException("HeinApprovalBhyt.ExecuteTime null");
                    }

                    if (!entity.Treatment.FEE_LOCK_TIME.HasValue)
                    {
                        MessageError = "Hồ sơ chưa có thời gian khóa viện phí";
                        throw new NullReferenceException("Treatment.FeeLockTime null");
                    }

                    if (entity.Treatment.FEE_LOCK_TIME.Value > entity.HeinApproval.EXECUTE_TIME.Value)
                    {
                        MessageError = "Thời gian duyệt khóa viện phí lớn hơn thời gian duyệt giám định BHYT";
                        throw new NullReferenceException("Thoi gian duyet khoa vien phi lon hon thoi gia duyet giam dinh Bhyt");
                    }

                    if (!entity.Treatment.OUT_TIME.HasValue)
                    {
                        MessageError = "Hồ sơ chưa có thời gian kết thúc điều trị";
                        throw new NullReferenceException("Treatment.OUT_TIME null");
                    }

                    if (entity.Treatment.FEE_LOCK_TIME.Value < entity.Treatment.OUT_TIME.Value)
                    {
                        MessageError = "Thời gian ra viện lớn hơn thời gian thanh toán";
                        throw new NullReferenceException("Treatment.OUT_TIME > Treatment.FEE_LOCK_TIME");
                    }

                    if (!entity.Treatment.END_DEPARTMENT_ID.HasValue)
                    {
                        MessageError = "Hồ sơ điểu trị không có khoa kết thúc điều trị";
                        throw new NullReferenceException("Khoa ket thuc dieu tri khong co");
                    }

                    if (!entity.Treatment.TREATMENT_END_TYPE_ID.HasValue)
                    {
                        MessageError = "Hồ sơ điểu trị không có loại ra viện";
                        throw new NullReferenceException("Ho so dieu tri khong co TreatmentEndTypeId");
                    }

                    if (!entity.Treatment.TREATMENT_RESULT_ID.HasValue)
                    {
                        MessageError = "Hồ sơ điểu trị không có kết quả";
                        throw new NullReferenceException("Ho so dieu tri khong co TreatmentResultId");
                    }
                }

                if (String.IsNullOrEmpty(entity.HeinApproval.RIGHT_ROUTE_CODE))
                {
                    MessageError = "Không xác định được diện đúng tuyến, trái tuyến";
                    throw new NullReferenceException("Khong Co HeinApprovalBhyt.RIGHT_ROUTE_CODE");
                }

                if (String.IsNullOrWhiteSpace(entity.Treatment.ICD_CODE))
                {
                    MessageError = "Hồ sơ không có thông tin bệnh";
                    throw new NullReferenceException("Khong Co Treatment.ICD_CODE");
                }

                HIS_ICD icd = entity.TotalIcdData != null ? entity.TotalIcdData.Where(o => o.ICD_CODE == entity.Treatment.ICD_CODE).FirstOrDefault() : null;
                if (icd != null && icd.DO_NOT_USE_HEIN == 1)
                {
                    MessageError = String.Format("Mã bệnh {0} không được bảo hiểm y tế thanh toán", entity.Treatment.ICD_CODE);
                    throw new NullReferenceException(String.Format("Ma benh {0} khong duoc bao hiem y te thanh toan ", entity.Treatment.ICD_CODE));
                }

                if (entity.HeinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT && (!entity.Treatment.CLINICAL_IN_TIME.HasValue))
                {
                    MessageError = "Hồ sơ điều trị nội trú nhưng không có thời gian vào điều trị";
                    throw new NullReferenceException("Ho so dieu tri noi tru nhung khong co thoi gian vao dieu tri:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => entity.Treatment.CLINICAL_IN_TIME), entity.Treatment.CLINICAL_IN_TIME));
                }

                if (isCheckLatentTuberculosis && Base.Checker.TreatmentChecker.IsLatentTuberculosis(entity.TotalIcdData, entity.Treatment.ICD_CODE))
                {
                    if (String.IsNullOrWhiteSpace(entity.Treatment.TUBERCULOSIS_ISSUED_ORG_NAME)
                        || !(entity.Treatment.TUBERCULOSIS_ISSUED_DATE > 0))
                    {
                        MessageError = "Thiếu thông tin cấp giấy xác nhận điều trị Lao";
                        throw new NullReferenceException("Khong Co Treatment.TUBERCULOSIS_ISSUED_ORG_NAME hoac Treatment.TUBERCULOSIS_ISSUED_DATE");
                    }
                }

                valid = valid && this.CheckConfigData(checkPath, ref MessageError);

                if (valid)
                {
                    string tNguonkhacOption = "";
                    if (entity.ConfigData != null && entity.ConfigData.Count > 0)
                    {
                        tNguonkhacOption = HisConfigKey.GetConfigData(entity.ConfigData, HisConfigKey.TNguonkhacOptionCFG);
                    }

                    if (isCheckHeinApprovalSereServ)
                    {
                        entity.ListSereServ = entity.ListSereServ.Where(o =>
                            o.TDL_HEIN_SERVICE_TYPE_ID.HasValue &&
                            o.HEIN_APPROVAL_ID == entity.HeinApproval.ID &&
                            o.PRICE > 0 && o.AMOUNT > 0 &&
                            o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                            o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                            o.VIR_TOTAL_HEIN_PRICE.HasValue &&
                            (o.VIR_TOTAL_HEIN_PRICE > 0 || (tNguonkhacOption == "2" && o.VIR_TOTAL_HEIN_PRICE == 0 && o.OTHER_SOURCE_PRICE.HasValue && o.OTHER_SOURCE_PRICE.Value > 0))).ToList();
                    }
                    else if (!IsCollinearNoLockFee)
                    {
                        var notHasHeinApprovals = entity.ListSereServ.Where(o =>
                            o.TDL_HEIN_SERVICE_TYPE_ID.HasValue &&
                            o.PRICE > 0 &&
                            o.AMOUNT > 0 &&
                            o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                            o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                            !o.HEIN_APPROVAL_ID.HasValue).ToList();

                        if (notHasHeinApprovals != null && notHasHeinApprovals.Count > 0)
                        {
                            MessageError = "Tồn tại dịch vụ BHYT chưa được duyệt giám định";
                            throw new Exception("Ton tai dich vu BHYT chu duoc duyet giam dinh" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => notHasHeinApprovals), notHasHeinApprovals));
                        }

                        entity.ListSereServ = entity.ListSereServ.Where(o =>
                            o.TDL_HEIN_SERVICE_TYPE_ID.HasValue &&
                            o.HEIN_APPROVAL_ID.HasValue &&
                            o.PRICE > 0 &&
                            o.AMOUNT > 0 &&
                            o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                            o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                            o.VIR_TOTAL_HEIN_PRICE.HasValue &&
                            (o.VIR_TOTAL_HEIN_PRICE > 0 || (tNguonkhacOption == "2" && o.VIR_TOTAL_HEIN_PRICE == 0 && o.OTHER_SOURCE_PRICE.HasValue && o.OTHER_SOURCE_PRICE.Value > 0))).ToList();
                    }
                    else
                    {
                        entity.ListSereServ = entity.ListSereServ.Where(o =>
                            o.TDL_HEIN_SERVICE_TYPE_ID.HasValue &&
                            o.PRICE > 0 &&
                            o.AMOUNT > 0 &&
                            o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                            o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&
                            o.VIR_TOTAL_HEIN_PRICE.HasValue &&
                            (o.VIR_TOTAL_HEIN_PRICE > 0 || (tNguonkhacOption == "2" && o.VIR_TOTAL_HEIN_PRICE == 0 && o.OTHER_SOURCE_PRICE.HasValue && o.OTHER_SOURCE_PRICE.Value > 0))).ToList();
                    }

                    if (entity.ListSereServ == null || entity.ListSereServ.Count == 0)
                    {
                        MessageError = "Hồ sơ không có dịch vụ BHYT thanh toán";
                        throw new NullReferenceException("ListSereServ is null or empty sau khi da loc HeinServiceType,IsExpend: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => entity.ListSereServ), entity.ListSereServ));
                    }

                    CheckDetailData(entity.ListSereServ, ref MessageError);

                    if (IsCollinearNoLockFee)
                    {
                        string collinearThoiGianQt = "";
                        string collinearNgayTToan = "";
                        if (this.entity.ConfigData != null && this.entity.ConfigData.Count > 0)
                        {
                            collinearThoiGianQt = HisConfigKey.GetConfigData(this.entity.ConfigData, HisConfigKey.CollinearThoiGianQt);
                            collinearNgayTToan = HisConfigKey.GetConfigData(this.entity.ConfigData, HisConfigKey.CollinearNgayTToan);
                        }

                        if (collinearThoiGianQt != "1")
                        {
                            if (this.entity.HeinApproval != null && this.entity.Treatment != null)
                            {
                                this.entity.HeinApproval.EXECUTE_TIME = this.entity.Treatment.OUT_TIME;
                            }
                        }

                        if (collinearNgayTToan == "1")
                        {
                            this.entity.Treatment.FEE_LOCK_TIME = this.entity.Treatment.OUT_TIME;
                        }
                        else if (collinearNgayTToan == "2")
                        {
                            this.entity.Treatment.FEE_LOCK_TIME = Inventec.Common.DateTime.Get.Now();
                        }
                    }
                    else
                    {
                        string thoiGianQt = "";
                        if (this.entity.ConfigData != null && this.entity.ConfigData.Count > 0)
                        {
                            thoiGianQt = HisConfigKey.GetConfigData(this.entity.ConfigData, HisConfigKey.ThoiGianQtCFG);
                        }

                        if (thoiGianQt != "1")
                        {
                            if (this.entity.HeinApproval != null && this.entity.Treatment != null)
                            {
                                this.entity.HeinApproval.EXECUTE_TIME = this.entity.Treatment.OUT_TIME;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private void CheckDetailData(List<MOS.EFMODEL.DataModels.V_HIS_SERE_SERV_2> listSereServ, ref string messageError)
        {
            List<string> serviceReqCodeBs = new List<string>();
            List<string> serviceReqCodeLieuDung = new List<string>();
            List<string> codeNoTutorial = new List<string>();

            string HeinServiceTypeCodeNoTutorial = "";
            if (entity.ConfigData != null && entity.ConfigData.Count > 0)
            {
                HeinServiceTypeCodeNoTutorial = HisConfigKey.GetConfigData(entity.ConfigData, HisConfigKey.MaThuocOption);
            }
            else
            {
                HeinServiceTypeCodeNoTutorial = entity.HeinServiceTypeCodeNoTutorial;
            }

            if (!String.IsNullOrWhiteSpace(HeinServiceTypeCodeNoTutorial))
            {
                codeNoTutorial = HeinServiceTypeCodeNoTutorial.Split('|').ToList();
            }

            var listHeinServiceTypeTh = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM
                };

            if (listSereServ.Exists(o => String.IsNullOrWhiteSpace(o.ICD_CODE)))
            {
                messageError = "thiếu thông tin mã bệnh";
                CodeError = string.Join(",", listSereServ.Where(o => String.IsNullOrWhiteSpace(o.ICD_CODE)).Select(s => s.TDL_SERVICE_REQ_CODE).Distinct().ToList());
                throw new NullReferenceException("Tồn tại dịch vụ không có mã bệnh chính: " + CodeError);
            }

            //xml thông tuyến bỏ check thời gian kết thúc nên bỏ check thời gian chỉ định so với thời gian kết thúc
            if (entity.Treatment.OUT_TIME.HasValue && listSereServ.Exists(o => o.TDL_INTRUCTION_TIME > entity.Treatment.OUT_TIME))
            {
                messageError = "thời gian chỉ định sau thời gian ra viện";
                CodeError = string.Join(",", listSereServ.Where(o => o.TDL_INTRUCTION_TIME > entity.Treatment.OUT_TIME).Select(s => s.TDL_SERVICE_REQ_CODE).Distinct().ToList());
                throw new NullReferenceException("Tồn tại dịch vụ có thời gian y lệnh lớn hơn thời gian ra viện: " + CodeError);
            }

            if (listSereServ.Exists(o => o.TDL_INTRUCTION_TIME < entity.Treatment.IN_TIME))
            {
                messageError = "thời gian chỉ định trước thời gian vào viện";
                CodeError = string.Join(",", listSereServ.Where(o => o.TDL_INTRUCTION_TIME < entity.Treatment.IN_TIME).Select(s => s.TDL_SERVICE_REQ_CODE).Distinct().ToList());
                throw new NullReferenceException("Tồn tại dịch vụ có thời gian chỉ định trước thời gian vào viện: " + CodeError);
            }

            if (!IsCollinearNoLockFee && listSereServ.Exists(o => o.TDL_INTRUCTION_TIME > entity.Treatment.FEE_LOCK_TIME))
            {
                messageError = "thời gian chỉ định sau thời gian thanh toán";
                CodeError = string.Join(",", listSereServ.Where(o => o.TDL_INTRUCTION_TIME > entity.Treatment.FEE_LOCK_TIME).Select(s => s.TDL_SERVICE_REQ_CODE).Distinct().ToList());
                throw new NullReferenceException("Tồn tại dịch vụ có thời gian y lệnh lớn hơn thời gian khóa viện phí: " + CodeError);
            }

            var MaBacSiHeinServiceType = HisConfigKey.GetConfigData(entity.ConfigData, HisConfigKey.MA_BAC_SI_HEIN_SERVICE_TYPE);
            foreach (var item in listSereServ)
            {
                if (listHeinServiceTypeTh.Contains(item.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
                {
                    if (!codeNoTutorial.Contains(item.ACTIVE_INGR_BHYT_CODE) && !codeNoTutorial.Contains(item.TDL_HEIN_SERVICE_BHYT_CODE))
                    {
                        if (String.IsNullOrWhiteSpace(item.TUTORIAL) && (item.TDL_HEIN_SERVICE_TYPE_ID ?? 0) != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU && (item.TDL_HEIN_SERVICE_TYPE_ID ?? 0) != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
                        {
                            serviceReqCodeLieuDung.Add(item.TDL_SERVICE_REQ_CODE);
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(MaBacSiHeinServiceType) || (!String.IsNullOrEmpty(MaBacSiHeinServiceType) && entity.ListHeinServiceType != null && entity.ListHeinServiceType.Count > 0 && item.TDL_HEIN_SERVICE_TYPE_ID.HasValue && entity.ListHeinServiceType.FirstOrDefault(o => o.ID == item.TDL_HEIN_SERVICE_TYPE_ID) != null && entity.ListHeinServiceType.FirstOrDefault(o => o.ID == item.TDL_HEIN_SERVICE_TYPE_ID).HEIN_SERVICE_TYPE_CODE != MaBacSiHeinServiceType))
                    {
                        if (String.IsNullOrWhiteSpace(item.EXECUTE_LOGINNAME) && String.IsNullOrWhiteSpace(item.REQUEST_LOGINNAME))
                        {
                            serviceReqCodeBs.Add(item.TDL_SERVICE_REQ_CODE);
                        }
                        else
                        {
                            var executeName = GetMaBacSi(item.EXECUTE_LOGINNAME);
                            var reqName = GetMaBacSi(item.REQUEST_LOGINNAME);
                            if (String.IsNullOrWhiteSpace(executeName) && String.IsNullOrWhiteSpace(reqName))
                            {
                                serviceReqCodeBs.Add(item.TDL_SERVICE_REQ_CODE);
                            }
                        }
                    }
                }
            }

            if (serviceReqCodeBs != null && serviceReqCodeBs.Count > 0)
            {
                messageError = "thiếu thông tin bác sĩ";
                serviceReqCodeBs = serviceReqCodeBs.Distinct().ToList();
                CodeError = string.Join(",", serviceReqCodeBs);
                throw new NullReferenceException("khong co ma bac si: " + CodeError);
            }

            if (serviceReqCodeLieuDung != null && serviceReqCodeLieuDung.Count > 0)
            {
                messageError = "thiếu thông tin liều dùng";
                serviceReqCodeLieuDung = serviceReqCodeLieuDung.Distinct().ToList();
                CodeError = string.Join(",", serviceReqCodeLieuDung);
                throw new NullReferenceException("khong co ma lieu dung: " + CodeError);
            }
        }

        private string GetMaBacSi(string loginName)
        {
            string result = "";
            try
            {
                if (GlobalConfigStore.ListEmployees != null)
                {
                    var dataEmployee = GlobalConfigStore.ListEmployees.FirstOrDefault(p => p.LOGINNAME == loginName);
                    if (dataEmployee != null)
                    {
                        result = dataEmployee.DIPLOMA;
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool CheckConfigData(bool checkPath, ref string messageError)
        {
            bool valid = true;
            try
            {
                if (entity.Branch == null && GlobalConfigStore.Branch == null)
                {
                    messageError = "Không xác định được chi nhánh của hồ sơ";
                    throw new NullReferenceException("Chua Xet Branch (Chi nhanh) cho LocalStore hoac InputADO");
                }

                if (checkPath && String.IsNullOrEmpty(GlobalConfigStore.PathSaveXml))
                {
                    messageError = "Không xác định được thư mục lưu file xml kết quả";
                    throw new NullReferenceException("Chua xet thu muc luu xml (PathSaveXml) cho LocalStore");
                }
            }
            catch (Exception ex)
            {
                valid = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return valid;
        }

        public string Run2076Path(ref string messageError)
        {
            string result = "";
            try
            {
                string error = "";
                string code = "";
                result = Run2076Path(ref error, ref code);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(MessageError))
                {
                    string codeError = "";
                    if (!String.IsNullOrWhiteSpace(CodeError))
                    {
                        codeError = string.Format("(Mã y lệnh: {0})", CodeError);
                    }
                    messageError = string.Format("{0}{1}", MessageError, codeError);
                    Inventec.Common.Logging.LogSystem.Error(string.Format("Run2076Path: {0}", messageError));
                }
            }
            return result;
        }

        public string Run2076Path(ref string messageError, ref string codeError)
        {
            string result = "";
            try
            {
                if (Check2076(true))
                {
                    this._HoSoProcessor2076 = new CV2076.Processor.HoSoChungTuProcessor(this.entity);
                    result = this._HoSoProcessor2076.ProcessorPath();

                    if (String.IsNullOrWhiteSpace(result))
                    {
                        messageError = "Lỗi trong quá trình tạo file xml2076";
                    }
                }

                messageError = MessageError;
                if (!String.IsNullOrWhiteSpace(CodeError))
                {
                    codeError = "Mã y lệnh:" + CodeError;
                }
            }
            catch (Exception ex)
            {
                messageError = MessageError;
                codeError = CodeError;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        public MemoryStream Run2076Plus()
        {
            MemoryStream result = null;
            try
            {
                string error = "";
                result = Run2076Plus(ref error);
                if (!String.IsNullOrWhiteSpace(error))
                {
                    Inventec.Common.Logging.LogSystem.Error("Run2076Plus: " + error);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public MemoryStream Run2076Plus(ref string messageError)
        {
            MemoryStream result = null;
            try
            {
                if (Check2076(false))
                {
                    this._HoSoProcessor2076 = new CV2076.Processor.HoSoChungTuProcessor(this.entity);
                    result = this._HoSoProcessor2076.ProcessorPlus();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(MessageError))
                {
                    string codeError = "";
                    if (!String.IsNullOrWhiteSpace(CodeError))
                    {
                        codeError = string.Format("(Mã y lệnh: {0})", CodeError);
                    }
                    messageError = string.Format("{0}{1}", MessageError, codeError);
                    Inventec.Common.Logging.LogSystem.Error(string.Format("Run2076Plus: {0}", messageError));
                }
            }
            return result;
        }

        private bool Check2076(bool checkPath)
        {
            bool valid = true;
            try
            {
                if (entity == null)
                {
                    MessageError = "Lỗi dữ liệu khởi tạo";
                    throw new NullReferenceException("InputADO truyen vao null: ");
                }

                if (entity.Treatment2076 == null)
                {
                    MessageError = "Không xác định được hồ sơ điều trị";
                    throw new NullReferenceException("Treatment2076 is null: " + LogUtil.TraceData("entity", entity));
                }

                if (entity.Treatment2076.IS_PAUSE != (short)1)
                {
                    MessageError = "Hồ sơ chưa kết thúc điều trị";
                    throw new NullReferenceException("Treatment2076.IS_PAUSE is invalid");
                }

                if (!entity.Treatment2076.OUT_TIME.HasValue)
                {
                    MessageError = "Hồ sơ chưa có thời gian kết thúc điều trị";
                    throw new NullReferenceException("Treatment2076.OUT_TIME null");
                }

                if (!entity.Treatment2076.END_DEPARTMENT_ID.HasValue)
                {
                    MessageError = "Hồ sơ điểu trị không có khoa kết thúc điều trị";
                    throw new NullReferenceException("Khoa ket thuc dieu tri khong co");
                }

                if (!entity.Treatment2076.TREATMENT_END_TYPE_ID.HasValue)
                {
                    MessageError = "Hồ sơ điểu trị không có loại ra viện";
                    throw new NullReferenceException("Ho so dieu tri khong co TreatmentEndTypeId");
                }

                if (!entity.Treatment2076.TREATMENT_RESULT_ID.HasValue)
                {
                    MessageError = "Hồ sơ điểu trị không có kết quả";
                    throw new NullReferenceException("Ho so dieu tri khong co TreatmentResultId");
                }

                if (String.IsNullOrEmpty(entity.Treatment2076.TDL_HEIN_CARD_NUMBER) & String.IsNullOrEmpty(entity.Treatment2076.SICK_HEIN_CARD_NUMBER) && String.IsNullOrEmpty(entity.Treatment2076.TDL_SOCIAL_INSURANCE_NUMBER))
                {
                    MessageError = "Không có thông tin mã BHXH";
                    throw new NullReferenceException("Khong Co Treatment2076.SICK_HEIN_CARD_NUMBER và Treatment2076.TDL_SOCIAL_INSURANCE_NUMBER ");
                }

                valid = valid && this.CheckConfigData(checkPath, ref MessageError);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        public string RunCheckInPath(ref string messageError, ref string codeError)
        {
            string result = "";
            try
            {
                if (CheckCheckIn(true))
                {
                    this._CheckInProcessor = new CheckIn.Processor.CheckInProcessor(this.entity);
                    result = this._CheckInProcessor.ProcessorPath();

                    if (String.IsNullOrWhiteSpace(result))
                    {
                        messageError = "Lỗi trong quá trình tạo file xml CheckIn";
                    }
                }

                messageError = MessageError;
                if (!String.IsNullOrWhiteSpace(CodeError))
                {
                    codeError = "Mã y lệnh:" + CodeError;
                }
            }
            catch (Exception ex)
            {
                messageError = MessageError;
                codeError = CodeError;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        public MemoryStream RunCheckInPlus(ref string messageError, ref string fileName)
        {
            MemoryStream result = null;
            try
            {
                if (CheckCheckIn(false))
                {
                    this._CheckInProcessor = new CheckIn.Processor.CheckInProcessor(this.entity);
                    result = this._CheckInProcessor.ProcessorPlus(ref fileName);
                    if (result == null)
                    {
                        messageError = "Lỗi trong quá trình tạo file xml CheckIn";
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(MessageError))
                {
                    string codeError = "";
                    if (!String.IsNullOrWhiteSpace(CodeError))
                    {
                        codeError = string.Format("(Mã y lệnh: {0})", CodeError);
                    }
                    messageError = string.Format("{0}{1}", MessageError, codeError);
                    Inventec.Common.Logging.LogSystem.Error(string.Format("RunCheckIn: {0}", messageError));
                }
            }
            return result;
        }
        public MemoryStream RunQd130CheckInPlus(ref string messageError, ref string fileName)
        {
            MemoryStream result = null;
            try
            {
                if (CheckCheckIn(false))
                {
                    this._CheckInProcessor = new CheckIn.Processor.CheckInProcessor(this.entity);
                    result = this._CheckInProcessor.ProcessorQd130CheckIn(ref fileName);
                    if (result == null)
                    {
                        messageError = "Lỗi trong quá trình tạo file xml CheckIn";
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            finally
            {
                if (!String.IsNullOrWhiteSpace(MessageError))
                {
                    string codeError = "";
                    if (!String.IsNullOrWhiteSpace(CodeError))
                    {
                        codeError = string.Format("(Mã y lệnh: {0})", CodeError);
                    }
                    messageError = string.Format("{0}{1}", MessageError, codeError);
                    Inventec.Common.Logging.LogSystem.Error(string.Format("RunCheckIn: {0}", messageError));
                }
            }
            return result;
        }

        private bool CheckCheckIn(bool checkPath)
        {
            bool valid = true;
            try
            {
                if (entity == null)
                {
                    MessageError = "Lỗi dữ liệu khởi tạo";
                    throw new NullReferenceException("InputADO truyen vao null: ");
                }

                if (entity.Treatment == null)
                {
                    MessageError = "Không xác định được hồ sơ điều trị";
                    throw new NullReferenceException("Treatment is null: " + LogUtil.TraceData("entity", entity));
                }

                if (entity.PatientTypeAlter == null)
                {
                    MessageError = "Không xác định được diện điều trị";
                    throw new NullReferenceException("PatientTypeAlter is null: " + LogUtil.TraceData("entity", entity));
                }
                valid = valid && this.CheckConfigData(checkPath, ref MessageError);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        public string Run4210GroupPath(ref string messageError)
        {
            string result = "";
            try
            {
                if (this.groupData != null && this.groupData.Treatments != null && this.groupData.Treatments.Count > 0)
                {
                    this._HoSoGroupProcessor4210 = new QD4210.Processor.HoSoGroupProcessor(this.groupData);
                    result = this._HoSoGroupProcessor4210.ProcessorPath(ref messageError);
                }
                else
                {
                    messageError = "Lỗi dữ liệu khởi tạo";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        public async Task<SyncResultADO> SendXmlCheckIn()
        {
            try
            {
                this._CheckInProcessor = new CheckIn.Processor.CheckInProcessor(this.entity);
                return await _CheckInProcessor.SendXmlCheckIn(entity);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return null;
        }
        public DataCheckIn GetXmlCheckInData(string xmlString)
        {
            DataCheckIn result = null;
            try
            {
                this._CheckInProcessor = new CheckIn.Processor.CheckInProcessor(xmlString);
                result = _CheckInProcessor.GetXmlCheckInData();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        public DataQd130CheckIn GetXmlQd130CheckInData(string xmlString)
        {
            DataQd130CheckIn result = null;
            try
            {
                this._CheckInProcessor = new CheckIn.Processor.CheckInProcessor(xmlString);
                result = _CheckInProcessor.GetXmlQd130CheckInData();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
