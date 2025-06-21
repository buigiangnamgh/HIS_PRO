using Inventec.Common.SignToolViewer.Integrate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.Integrate
{
    class FssFileDownload
    {
        internal static MemoryStream GetFile(string fileUrl)
        {
            try
            {
                return GetFile(fileUrl, null);
            }
            catch (FileUploadException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FileUploadException("Exception when uploading file", ex);
            }
        }

        internal static MemoryStream GetFile(string fileUrl, string baseUri)
        {
            string urlFile = "";
            try
            {
                using (var client = new HttpClient())
                {
                    urlFile = ((!String.IsNullOrEmpty(baseUri) ? baseUri : FssConstant.BASE_URI) + ResolveUrl(fileUrl));
                    HttpResponseMessage resp = client.GetAsync(urlFile).Result;
                    if (!resp.IsSuccessStatusCode)
                    {
                        throw new FileDownloadException(resp.StatusCode, resp.ReasonPhrase);
                    }
                    if (resp.Content != null)
                    {
                        return new MemoryStream(resp.Content.ReadAsByteArrayAsync().Result);
                    }
                    return null;
                }
            }
            catch (FileUploadException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FileUploadException("Exception when uploading file " + "____urlFile:" + urlFile + "____" + ex.Message, ex);
            }
        }

        static string ResolveUrl(string urlFile)
        {
            if (!String.IsNullOrEmpty(urlFile))
            {
                urlFile = urlFile.Replace("\\", "//");
                urlFile = urlFile.Replace("//", "/");
            }

            return urlFile;
        }
    }

    internal class FileUploadException : Exception
    {
        internal HttpStatusCode StatusCode { get; set; }

        internal FileUploadException()
        {
        }

        internal FileUploadException(string message, Exception inner)
            : base(message, inner)
        {
        }

        internal FileUploadException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.StatusCode = statusCode;
        }
    }

    internal class FileDownloadException : Exception
    {
        internal HttpStatusCode StatusCode { get; set; }

        internal FileDownloadException()
        {
        }

        internal FileDownloadException(string message, Exception inner)
            : base(message, inner)
        {
        }

        internal FileDownloadException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
