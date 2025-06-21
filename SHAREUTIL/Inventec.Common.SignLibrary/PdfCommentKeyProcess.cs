using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary
{
    internal class PdfCommentKeyProcess
    {
        public PdfCommentKeyProcess()
        {
        }

        internal List<SignPositionADO> Run(string src, ref string outFile)
        {
            List<SignPositionADO> signPositionAutos = new List<SignPositionADO>();
            try
            {              
                File.Copy(src, outFile, true);
            
                signPositionAutos = PdfDocumentProcess.GetPositionWithAutoAddAnnotationBySearchKey(src, outFile, "<SINGLE_KEY__COMMENT_SIGN__");

                //GemBox.Pdf.ComponentInfo.SetLicense("ARHC-LA4K-R49S-TR4L");
                //using (var document = GemBox.Pdf.PdfDocument.Load(src))
                //{
                //    int p = 1;
                //    foreach (var page in document.Pages)
                //    {
                //        foreach (var textElement in page.Content.Elements.All()
                //        .Where(element => element.ElementType == GemBox.Pdf.Content.PdfContentElementType.Text)
                //        .Cast<GemBox.Pdf.Content.PdfTextContent>())
                //        {
                //            string text = textElement.ToString();
                //            var font = textElement.Format.Text.Font;
                //            var color = textElement.Format.Fill.Color;
                //            var location = textElement.Location;

                //            if (text.Contains("<SINGLE_KEY__COMMENT_SIGN__"))
                //            {
                //                iTextSharp.text.Rectangle stickyRectangle = new iTextSharp.text.Rectangle(
                //                               (float)location.X,
                //                               (float)location.Y,
                //                               (float)location.X + 5,
                //                               (float)location.Y + 5
                //                           );

                //                signPositionAutos.Add(new SignPositionADO()
                //                {
                //                    PageNUm = p,
                //                    Text = text,
                //                    Reactanle = stickyRectangle
                //                });
                //                Utils.AddTextAnnotation(outFile, "$1", p, location.X, location.Y, 5, 5);
                //            }
                //        }
                //        p++;
                //    }
                //}
              
                if (signPositionAutos != null && signPositionAutos.Count > 0)
                {
                    Inventec.Common.Logging.LogSystem.Info("Toa do vi tri key tu dong key trong file template theo key <SINGLE_KEY__COMMENT_SIGN__: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => signPositionAutos), signPositionAutos) + "____outFile:" + outFile);
                    PdfDocumentProcess.ReplaceText(signPositionAutos.Select(o => o.Text).ToList(), outFile);
                }              
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return signPositionAutos;
        }

        internal List<SignPositionADO> RunWithUserKey(string src, ref string outFile)
        {
            List<SignPositionADO> signPositionAutos = new List<SignPositionADO>();
            try
            {                
                File.Copy(src, outFile, true);             
                signPositionAutos = PdfDocumentProcess.GetPositionWithAutoAddAnnotationBySearchKey(src, outFile, "{USER_SIGN_KEY__");
                if (signPositionAutos != null && signPositionAutos.Count > 0)
                {
                    Inventec.Common.Logging.LogSystem.Info("Toa do vi tri key tu dong key trong file template theo key {USER_SIGN_KEY__: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => signPositionAutos), signPositionAutos) + "____outFile:" + outFile);
                    PdfDocumentProcess.ReplaceText(signPositionAutos.Select(o => o.Text).ToList(), outFile);
                }             
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return signPositionAutos;
        }
    }
}
