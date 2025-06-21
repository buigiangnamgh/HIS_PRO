using EMR.EFMODEL.DataModels;
using EMR.Filter;
using EMR.SDO;
using EMR.TDO;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.Api
{
    class EmrDocument : BussinessBase
    {
        internal EmrDocument() : base() { }
        internal EmrDocument(CommonParam param) : base(param) { }

        internal List<EMR_DOCUMENT> Get(EmrDocumentFilter filter, CommonParam paramCommon)
        {
            List<EMR_DOCUMENT> data = null;
            try
            {
                data = GlobalStore.EmrConsumer.Get<List<EMR_DOCUMENT>>(EMR.URI.EmrDocument.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon), ex);
                data = null;
            }

            return data;
        }

        internal List<V_EMR_DOCUMENT> GetView(EmrDocumentViewFilter filter, CommonParam paramCommon)
        {
            List<V_EMR_DOCUMENT> data = null;
            try
            {
                data = GlobalStore.EmrConsumer.Get<List<V_EMR_DOCUMENT>>(EMR.URI.EmrDocument.GET_VIEW, paramCommon, filter);
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon) + "____" + GlobalStore.EmrConsumer.GetBaseUri());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon), ex);
                data = null;
            }

            return data;
        }

        internal EMR_DOCUMENT GetByCode(string code)
        {
            EMR_DOCUMENT data = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(code))
                {
                    CommonParam paramCommon = new CommonParam();
                    EmrDocumentFilter filter = new EmrDocumentFilter();
                    filter.DOCUMENT_CODE__EXACT = code;
                    List<EMR_DOCUMENT> datas = GlobalStore.EmrConsumer.Get<List<EMR_DOCUMENT>>(EMR.URI.EmrDocument.GET, paramCommon, filter);
                    data = datas != null ? datas.FirstOrDefault(o => o.DOCUMENT_CODE == code) : null;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code), ex);
                data = null;
            }

            return data;
        }

        internal List<EMR_DOCUMENT> GetByMergeCode(string mergeCode)
        {
            List<EMR_DOCUMENT> datas = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(mergeCode))
                {
                    CommonParam paramCommon = new CommonParam();
                    EmrDocumentFilter filter = new EmrDocumentFilter();
                    filter.MERGE_CODE__EXACT = mergeCode;
                    datas = GlobalStore.EmrConsumer.Get<List<EMR_DOCUMENT>>(EMR.URI.EmrDocument.GET, paramCommon, filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => mergeCode), mergeCode), ex);
                datas = null;
            }

            return datas;
        }

        /// <summary>
        /// POST: api/EmrDocument/MakeDocumentMerge
        ///input: MergeCode
        ///output: MakeDocumentMergeResultSDO
        /// </summary>
        /// <param name="mergeCode"></param>
        /// <returns></returns>
        internal byte[] GetDocumentByMergeCode(string mergeCode, string documentName, List<long> documentIds = null)
        {
            byte[] datas = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(mergeCode))
                {
                    CommonParam paramCommon = new CommonParam();

                    DocumentMergeSDO documentMergeSDO = new DocumentMergeSDO();
                    documentMergeSDO.MergeCode = mergeCode;
                    if (documentIds != null && documentIds.Count > 0)
                    {
                        documentMergeSDO.DocumentIds = documentIds;
                    }
                    if(!string.IsNullOrEmpty(documentName))
                        documentMergeSDO.DocumentName = documentName;
                    string base64PdfDocumentMerge = GlobalStore.EmrConsumer.Post<string>("api/EmrDocument/MakeDocumentMergeBySdo", paramCommon, documentMergeSDO);
                    if (!String.IsNullOrEmpty(base64PdfDocumentMerge))
                    {
                        datas = Convert.FromBase64String(base64PdfDocumentMerge);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => mergeCode), mergeCode), ex);
                datas = null;
            }

            return datas;
        }

        internal V_EMR_DOCUMENT GetViewByCode(string code)
        {
            V_EMR_DOCUMENT data = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(code))
                {
                    CommonParam paramCommon = new CommonParam();
                    EmrDocumentViewFilter filter = new EmrDocumentViewFilter();
                    filter.DOCUMENT_CODE__EXACT = code;//TODO
                    List<V_EMR_DOCUMENT> datas = GlobalStore.EmrConsumer.Get<List<V_EMR_DOCUMENT>>(EMR.URI.EmrDocument.GET_VIEW, paramCommon, filter);
                    data = datas != null ? datas.FirstOrDefault(o => o.DOCUMENT_CODE == code) : null;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code), ex);
                data = null;
            }

            return data;
        }

        internal DocumentTDO CreateByTdo(string TokenCode, DocumentTDO dataDoc)
        {
            CommonParam paramCommon = new CommonParam();
            DocumentTDO data = null;
            try
            {
                var EmrConsumer = GlobalStore.GetSetDicConsumer(TokenCode);

                var rs = EmrConsumer.PostRO<ApiResultObject<DocumentTDO>>(EMR.URI.EmrDocument.CREATE_BY_TDO, paramCommon, dataDoc);
                if (rs != null)
                {
                    data = rs.Data;
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Call api " + EMR.URI.EmrDocument.CREATE_BY_TDO + " return fail ____"
                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => TokenCode), TokenCode)
                         + Inventec.Common.Logging.LogUtil.TraceData("GlobalStore.GetDicEmrConsumer", GlobalStore.GetDicEmrConsumer())
                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rs), rs)
                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon)
                         );
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => dataDoc), dataDoc), ex);
                data = null;
            }

            return data;
        }

        internal HsmSignCreateTDO CreateAndSignHsm(string TokenCode, HsmSignCreateTDO dataSign)
        {
            CommonParam paramCommon = new CommonParam();
            HsmSignCreateTDO data = null;
            try
            {
                var EmrConsumer = GlobalStore.GetSetDicConsumer(TokenCode);
                var rs = EmrConsumer.PostRO<ApiResultObject<HsmSignCreateTDO>>(EMR.URI.EmrDocument.CREATE_AND_SIGN_HSM, paramCommon, dataSign);
                if (rs != null)
                {
                    data = rs.Data;

                    if (rs.Param != null && rs.Param.BugCodes != null && rs.Param.BugCodes.Count > 0 && rs.Param.BugCodes.Contains("EMR053"))
                    {
                        var signer = GlobalStore.GetByLoginName(GlobalStore.LoginName);
                        Popup.frmUpdateSigner frm = new Popup.frmUpdateSigner(signer, 1);
                        frm.ShowDialog();
                    }
                    else if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Call api " + EMR.URI.EmrDocument.CREATE_AND_SIGN_HSM + " return fail ____"
                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => TokenCode), TokenCode)
                         + Inventec.Common.Logging.LogUtil.TraceData("GlobalStore.GetDicEmrConsumer", GlobalStore.GetDicEmrConsumer())
                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rs), rs)
                         + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon)
                         );
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                data = null;
            }

            return data;
        }

        internal EmrSignResultSDO SignHsm(string TokenCode, EmrSignHsmSDO dataSign)
        {
            CommonParam paramCommon = new CommonParam();
            EmrSignResultSDO data = null;
            try
            {
                var EmrConsumer = GlobalStore.GetSetDicConsumer(TokenCode);
                var rs = EmrConsumer.PostRO<ApiResultObject<EmrSignResultSDO>>(EMR.URI.EmrSign.SIGN_PDF_HSM, paramCommon, dataSign);

                if (rs != null)
                {
                    data = rs.Data;

                    if (rs.Param != null && rs.Param.BugCodes != null && rs.Param.BugCodes.Count > 0 && rs.Param.BugCodes.Contains("EMR053"))
                    {
                        var signer = GlobalStore.GetByLoginName(GlobalStore.LoginName);
                        Popup.frmUpdateSigner frm = new Popup.frmUpdateSigner(signer, 1);
                        frm.ShowDialog();
                    }
                    else if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Call api " + EMR.URI.EmrSign.SIGN_PDF_HSM + " return fail ____"
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => TokenCode), TokenCode)
                        + Inventec.Common.Logging.LogUtil.TraceData("GlobalStore.GetDicEmrConsumer", GlobalStore.GetDicEmrConsumer())
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rs), rs)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon)
                        );
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                data = null;
            }

            return data;
        }

        internal EmrSignResultSDO PatientOrHomeRelativeSignHsm(string TokenCode, EmrSignHsmSDO dataSign)
        {
            CommonParam paramCommon = new CommonParam();
            EmrSignResultSDO data = null;
            try
            {
                var EmrConsumer = GlobalStore.GetSetDicConsumer(TokenCode);
                var rs = EmrConsumer.PostRO<ApiResultObject<EmrSignResultSDO>>("api/EmrSign/PatientAddAndSign", paramCommon, dataSign);

                if (rs != null)
                {
                    data = rs.Data;

                    if (rs.Param != null && rs.Param.BugCodes != null && rs.Param.BugCodes.Count > 0 && rs.Param.BugCodes.Contains("EMR053"))
                    {
                        var signer = GlobalStore.GetByLoginName(GlobalStore.LoginName);
                        Popup.frmUpdateSigner frm = new Popup.frmUpdateSigner(signer, 1);
                        frm.ShowDialog();
                    }
                    else if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Call api " + EMR.URI.EmrSign.SIGN_PDF_HSM + " return fail ____"
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => TokenCode), TokenCode)
                        + Inventec.Common.Logging.LogUtil.TraceData("GlobalStore.GetDicEmrConsumer", GlobalStore.GetDicEmrConsumer())
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rs), rs)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon)
                        );
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                data = null;
            }

            return data;
        }

        internal UsbSignCreateTDO CreateAndSignUsb(string TokenCode, UsbSignCreateTDO dataSign)
        {
            CommonParam paramCommon = new CommonParam();
            UsbSignCreateTDO data = null;
            try
            {
                var EmrConsumer = GlobalStore.GetSetDicConsumer(TokenCode);
                var rs = EmrConsumer.PostRO<ApiResultObject<UsbSignCreateTDO>>(EMR.URI.EmrDocument.CREATE_AND_SIGN_USB, paramCommon, dataSign);
                if (rs != null)
                {
                    data = rs.Data;
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Call api " + EMR.URI.EmrDocument.CREATE_AND_SIGN_USB + " return fail ____"
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => TokenCode), TokenCode)
                        + Inventec.Common.Logging.LogUtil.TraceData("GlobalStore.GetDicEmrConsumer", GlobalStore.GetDicEmrConsumer())
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rs), rs)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon)
                        );
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                data = null;
            }

            return data;
        }

        internal EmrSignResultSDO SignUsb(string TokenCode, EmrSignUsbSDO dataSign)
        {
            CommonParam paramCommon = new CommonParam();
            EmrSignResultSDO data = null;
            try
            {
                var EmrConsumer = GlobalStore.GetSetDicConsumer(TokenCode);
                var rs = EmrConsumer.PostRO<ApiResultObject<EmrSignResultSDO>>(EMR.URI.EmrSign.SIGN_PDF_USB, paramCommon, dataSign);
                if (rs != null)
                {
                    data = rs.Data;
                    if (rs.Param != null)
                    {
                        param.Messages.AddRange(rs.Param.Messages);
                        param.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Call api " + EMR.URI.EmrSign.SIGN_PDF_USB + " return fail ____"
                       + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => TokenCode), TokenCode)
                       + Inventec.Common.Logging.LogUtil.TraceData("GlobalStore.GetDicEmrConsumer", GlobalStore.GetDicEmrConsumer())
                       + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => rs), rs)
                       + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon)
                       );
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                data = null;
            }

            return data;
        }

        internal List<V_EMR_DOCUMENT> GetDocumentDependent(string dependentCode, string treatmentCode, string signer)
        {
            List<V_EMR_DOCUMENT> datas = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(dependentCode))
                {
                    CommonParam paramCommon = new CommonParam();
                    EmrDocumentViewFilter filter = new EmrDocumentViewFilter();
                    filter.PARENT_DEPENDENT_CODE__EXACT = dependentCode;
                    filter.IS_DELETE = false;
                    filter.HAS_REJECTER = false;
                    filter.TREATMENT_CODE__EXACT = treatmentCode;
                    filter.HAS_RESIGN_FAILED = false;
                    filter.HAS_NEXT_SIGNER = true;
                    filter.NEXT_SIGNER__EXACT = signer;
                    datas = GlobalStore.EmrConsumer.Get<List<V_EMR_DOCUMENT>>(EMR.URI.EmrDocument.GET_VIEW, paramCommon, filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => dependentCode), dependentCode)
                    + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatmentCode), treatmentCode), ex);
                datas = null;
            }

            return datas;
        }
    }
}
