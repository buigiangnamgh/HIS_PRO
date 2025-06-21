using EMR.EFMODEL.DataModels;
using EMR.Filter;
using EMR.SDO;
using EMR.TDO;
using Inventec.Common.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.Api
{
    class EmrSign : BussinessBase
    {
        internal EmrSign() : base() { }
        internal EmrSign(CommonParam param) : base(param) { }

        internal List<EMR_SIGN> Get(EmrSignFilter filter)
        {
            List<EMR_SIGN> data = null;
            try
            {

                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_SIGN>>(EMR.URI.EmrSign.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal SignTDO GetSignDocumentFirstByLoginName(string documentCode, EMR_SIGNER signer)
        {
            SignTDO data = null;
            try
            {
                if (!String.IsNullOrEmpty(documentCode) && signer != null)
                {
                    var doc = new EmrDocument().GetByCode(documentCode);

                    CommonParam paramCommon = new CommonParam();
                    EmrSignFilter filter = new EmrSignFilter();
                    filter.LOGINNAME__EXACT = signer.LOGINNAME;
                    filter.DOCUMENT_ID = doc.ID;
                    filter.HAS_SIGN_TIME = false;
                    var rs = GlobalStore.EmrConsumer.Get<List<EMR_SIGN>>(EMR.URI.EmrSign.GET, paramCommon, filter).OrderBy(o => o.NUM_ORDER).FirstOrDefault();

                    if (rs != null)
                    {
                        data = new SignTDO()
                        {
                            DepartmentCode = rs.DEPARTMENT_CODE,
                            DepartmentName = rs.DEPARTMENT_NAME,
                            FirstName = rs.FIRST_NAME,
                            FullName = rs.VIR_PATIENT_NAME,
                            LastName = rs.LAST_NAME,
                            Loginname = rs.LOGINNAME,
                            NumOrder = rs.NUM_ORDER,
                            PatientCode = rs.PATIENT_CODE,
                            SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")), //rs.SIGN_TIME,
                            SignerId = signer.ID,

                            DocumentCode = documentCode,
                            Title = rs.TITLE,
                            Username = rs.USERNAME,

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal EMR_SIGN GetSignDocumentFirst(string documentCode, EMR_SIGNER signer, EMR.EFMODEL.DataModels.EMR_TREATMENT treatment, bool isMultiSign, bool isGetOtherSignTimeNull)
        {
            EMR_SIGN data = null;
            try
            {
                document = GetDocumentView(documentCode);
                data = document != null ? GetSignDocumentFirst(document, signer, treatment, isGetOtherSignTimeNull, isMultiSign) : null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }
        private V_EMR_DOCUMENT document { get; set; }
        internal V_EMR_DOCUMENT GetDocumentView(string documentCode)
        {
            return document = new EmrDocument().GetViewByCode(documentCode);
        }

        internal EMR_SIGN GetSignDocumentFirst(V_EMR_DOCUMENT document, EMR_SIGNER signer, EMR.EFMODEL.DataModels.EMR_TREATMENT treatment, bool isGetOtherSignTimeNull = false, bool? isMultiSign = null)
        {
            EMR_SIGN data = null;
            try
            {
                bool _isMultiSign = isMultiSign.HasValue ? isMultiSign.Value : (document.IS_MULTI_SIGN == 1);
                CommonParam paramCommon = new CommonParam();
                EmrSignFilter filter = new EmrSignFilter();
                filter.DOCUMENT_ID = document.ID;
                var datas = GlobalStore.EmrConsumer.Get<List<EMR_SIGN>>(EMR.URI.EmrSign.GET, paramCommon, filter);
                if (datas != null && datas.Count > 0)
                {
                    bool isUseFlow = datas.Any(o => o.FLOW_ID.HasValue && o.FLOW_ID > 0 && (o.IS_SIGNING == 1 || (o.IS_SIGNING != 1 && (o.SIGN_TIME ?? 0) <= 0)));
                    if (isUseFlow)
                    {                        
                        data = (datas.Any(o => o.FLOW_ID.HasValue && o.FLOW_ID > 0 && o.IS_SIGNING == 1) ? datas.FirstOrDefault(o => o.IS_SIGNING == 1 && o.FLOW_ID.HasValue && o.FLOW_ID > 0) : datas.Where(o => o.FLOW_ID.HasValue && o.FLOW_ID > 0 && (o.SIGN_TIME ?? 0) <= 0 && o.IS_SIGNING != 1).OrderBy(o => o.NUM_ORDER).FirstOrDefault());
                    }
                    else
                    {
                        if (signer != null)
                        {
                            datas = datas.Where(o => o.LOGINNAME == signer.LOGINNAME).ToList();
                        }
                        else
                        {                         
                            datas = datas.Where(o => o.PATIENT_CODE == treatment.PATIENT_CODE).ToList();
                        }

                        data = _isMultiSign ?
                            (datas.Any(o => o.IS_SIGNING == 1) ? datas.FirstOrDefault(o => o.IS_SIGNING == 1)
                                : isGetOtherSignTimeNull ?
                                    datas.Where(o => o.SIGN_TIME == null).OrderBy(o => o.NUM_ORDER).FirstOrDefault()
                                    : null) :
                            isGetOtherSignTimeNull ?
                                datas.Where(o => o.SIGN_TIME == null).OrderBy(o => o.NUM_ORDER).FirstOrDefault()
                                : null;//Tìm bản ghi của văn ban ở trạng thái đang ký mà được ký nhiều lần -> nếu không thấy thì tìm bản ghi của văn bản ở trạng thái chưa được ký      
                        data = ((signer == null && data == null) ? datas.Where(o => o.SIGN_TIME == null).OrderBy(o => o.NUM_ORDER).FirstOrDefault() : data);
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Warn("Ky voi van ban da ton tai, khong tim thay sign cua nguoi ky de ky, khong the tiep tuc luong ky____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => document), document));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn("Khong tim thay ban ghi EMR_SIGN nao thoa man____Input data:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => document), document) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => signer), signer), ex);
                data = null;
            }

            return data;
        }


        internal List<EMR_SIGN> GetSignDocumentFirstForSignElectronic(DocumentTDO document)
        {
            List<EMR_SIGN> data = null;
            try
            {
                var doc = new EmrDocument().GetViewByCode(document.DocumentCode);
                CommonParam paramCommon = new CommonParam();
                EmrSignFilter filter = new EmrSignFilter();
                filter.DOCUMENT_ID = doc.ID;
                var datas = GlobalStore.EmrConsumer.Get<List<EMR_SIGN>>(EMR.URI.EmrSign.GET, paramCommon, filter);
                if (datas != null && datas.Count > 0)
                {
                    data = datas.Where(o => o.IS_SIGN_ELECTRONIC == 1 && ((o.IS_SIGN_BOARD ?? 0) != 1) && (o.REJECT_TIME ?? 0) == 0).ToList();
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Warn("Ky voi van ban da ton tai, khong tim thay sign cua nguoi ky de ky, khong the tiep tuc luong ky____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => document), document));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<EMR_SIGN> GetSignDocumentForDocument(DocumentTDO document)
        {
            List<EMR_SIGN> data = null;
            try
            {
                var doc = new EmrDocument().GetViewByCode(document.DocumentCode);
                CommonParam paramCommon = new CommonParam();
                EmrSignFilter filter = new EmrSignFilter();
                filter.DOCUMENT_ID = doc.ID;
                data = GlobalStore.EmrConsumer.Get<List<EMR_SIGN>>(EMR.URI.EmrSign.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal bool Reject(string TokenCode, EmrSignRejectSDO signRejectSDO)
        {
            CommonParam paramCommon = new CommonParam();
            bool data = false;
            try
            {
                var EmrConsumer = GlobalStore.GetSetDicConsumer(TokenCode);
                var rs = EmrConsumer.PostRO<ApiResultObject<EMR_SIGN>>(EMR.URI.EmrSign.REJECT, paramCommon, signRejectSDO);
                if (rs != null)
                {
                    data = rs.Success;
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = false;
            }

            return data;
        }

        internal EMR_SIGN SignEnd(string TokenCode, long signId)//TODO change return type
        {
            CommonParam paramCommon = new CommonParam();
            EMR_SIGN data = null;
            try
            {
                var EmrConsumer = GlobalStore.GetSetDicConsumer(TokenCode);
                var rs = EmrConsumer.PostRO<ApiResultObject<EMR_SIGN>>("api/EmrSign/Finish", paramCommon, signId);
                if (rs != null)
                {
                    data = rs.Data;
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }
    }
}
