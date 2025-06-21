using EMR.EFMODEL.DataModels;
using EMR.Filter;
using Inventec.Common.Integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.Api
{
    class EmrDocumentType : BussinessBase
    {
        internal EmrDocumentType() : base() { }
        internal EmrDocumentType(CommonParam param) : base(param) { }

        internal List<EMR_DOCUMENT_TYPE> Get(EmrDocumentTypeFilter filter)
        {
            List<EMR_DOCUMENT_TYPE> data = null;
            try
            {
                CommonParam paramCommon = new CommonParam();
                data = GlobalStore.EmrConsumer.Get<List<EMR_DOCUMENT_TYPE>>(EMR.URI.EmrDocumentType.GET, paramCommon, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal List<EMR_DOCUMENT_TYPE> Get()
        {
            List<EMR_DOCUMENT_TYPE> data = null;
            try
            {
                EmrDocumentTypeFilter filter = new EmrDocumentTypeFilter();
                data = Get(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal EMR_DOCUMENT_TYPE GetByCode(string code)
        {
            EMR_DOCUMENT_TYPE data = null;
            try
            {
                if (!String.IsNullOrEmpty(code))
                {
                    EmrDocumentTypeFilter filter = new EmrDocumentTypeFilter() { DOCUMENT_TYPE_CODE__EXACT = code };
                    data = Get(filter).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                data = null;
            }

            return data;
        }

        internal bool IsMultiSign(string documentTypeCode)
        {
            bool valid = false;
            try
            {
                var docType = (!String.IsNullOrEmpty(documentTypeCode)) ? DocumentTypeProperty(documentTypeCode) : null;
                valid = (docType != null && docType.IS_MULTI_SIGN == 1);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                valid = false;
            }

            return valid;
        }

        internal void DocumentTypeProperty(string documentTypeCode, ref bool isMultiSign, ref bool isSignParanel)
        {
            try
            {
                var docType = (!String.IsNullOrEmpty(documentTypeCode)) ? DocumentTypeProperty(documentTypeCode) : null;
                isMultiSign = (docType != null && docType.IS_MULTI_SIGN == 1);
                isSignParanel = (docType != null && docType.IS_SIGN_PARALLEL == 1);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        internal void DocumentTypeProperty(string documentTypeCode, ref bool isMultiSign, ref bool isSignParanel, ref long? documentTypeId)
        {
            try
            {
                var docType = (!String.IsNullOrEmpty(documentTypeCode)) ? DocumentTypeProperty(documentTypeCode) : null;
                isMultiSign = (docType != null && docType.IS_MULTI_SIGN == 1);
                isSignParanel = (docType != null && docType.IS_SIGN_PARALLEL == 1);
                documentTypeId = (docType != null ? (long?)docType.ID : null);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        internal EMR_DOCUMENT_TYPE DocumentTypeProperty(string documentTypeCode)
        {
            try
            {
                EmrDocumentTypeFilter filter = new EmrDocumentTypeFilter() { DOCUMENT_TYPE_CODE__EXACT = documentTypeCode };
                var docTypes = Get(filter);
                var docType = (docTypes != null && docTypes.Count > 0) ? docTypes.FirstOrDefault() : null;
                return docType;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return null;
        }
    }
}
