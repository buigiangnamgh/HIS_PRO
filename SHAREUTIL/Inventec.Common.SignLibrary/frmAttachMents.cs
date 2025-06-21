using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using EMR.EFMODEL.DataModels;
using EMR.Filter;
using Inventec.Common.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Inventec.Common.SignLibrary;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.ADO;
using EMR.SDO;
using Inventec.Common.SignLibrary.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Inventec.Common.SignLibrary
{
    public partial class frmAttachMents : Form
    {
        #region Reclare
        private string DocumentCode;
        private string LoginName;
        V_EMR_DOCUMENT Document;
        string[] fullfileNameAttack;
        AttackADO fileNameAttack;
        List<AttackADO> ListfileNameAttack = new List<AttackADO>();
        #endregion

        #region Construct
        public frmAttachMents()
        {
            InitializeComponent();
        }

        public frmAttachMents(string document)
        {
            InitializeComponent();
            try
            {
                this.DocumentCode = document;
                this.LoginName = GlobalStore.LoginName;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        #endregion

        #region Private Method
        private void frmAttachMents_Load(object sender, EventArgs e)
        {
            try
            {
                LoadKeysFromlanguage();
                SetDefaultData();
                loadgridView2(this.Document.ID);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void SetDefaultData()
        {
            try
            {

                Inventec.Common.Integrate.CommonParam commonParam = new Inventec.Common.Integrate.CommonParam();
                EMR.Filter.EmrDocumentViewFilter documentFilter = new EmrDocumentViewFilter();
                documentFilter.DOCUMENT_CODE__EXACT = this.DocumentCode;
                var apiData = GlobalStore.EmrConsumer.Get<List<V_EMR_DOCUMENT>>(EMR.URI.EmrDocument.GET_VIEW, commonParam, documentFilter);
                //var apiData = new BackendAdapter(new CommonParam()).Get<List<V_EMR_DOCUMENT>>(EMR.URI.EmrDocument.GET_VIEW, GlobalStore.EmrConsumer, documentFilter, commonParam);
                if (apiData != null && apiData.Count > 0)
                {
                    this.Document = apiData.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadKeysFromlanguage()
        {
            //TODO
        }

        private void loadgridView2(long documentID)
        {
            try
            {
                Inventec.Common.Integrate.CommonParam commonParam = new Inventec.Common.Integrate.CommonParam();
                //List<EMR_ATTACHMENT> EmrAttachmentList = null;
                EmrAttachmentFilter documentFilter = new EmrAttachmentFilter();

                documentFilter.ORDER_DIRECTION = "ASC";
                documentFilter.ORDER_FIELD = "NUM_ORDER";
                documentFilter.DOCUMENT_ID = documentID;

                var apiData = GlobalStore.EmrConsumer.Get<List<EMR_ATTACHMENT>>(EMR.URI.EmrAttachment.GET, commonParam, documentFilter);
                //EmrAttachmentList = new BackendAdapter(paramCommon).Get<List<EMR_ATTACHMENT>>(EMR.URI.EmrAttachment.GET, ApiConsumers.EmrConsumer, filter, paramCommon);

                WaitingManager.Hide();
                gridView2.BeginUpdate();
                gridView2.GridControl.DataSource = null;
                gridView2.GridControl.DataSource = apiData;
                gridView2.EndUpdate();
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        #endregion

        private void gridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                EMR_ATTACHMENT pData = (EMR_ATTACHMENT)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                if (e.IsGetData && e.Column.UnboundType != DevExpress.Data.UnboundColumnType.Bound)
                {
                    if (pData != null)
                    {
                        if (e.Column.FieldName == "STT")
                        {
                            e.Value = e.ListSourceRowIndex + 1;
                        }
                        else if (e.Column.FieldName == "CREATE_TIME_STR")
                        {
                            try
                            {
                                e.Value = DateTimeConvert.TimeNumberToTimeString(pData.CREATE_TIME ?? 0);
                            }
                            catch (Exception ex)
                            {
                                Inventec.Common.Logging.LogSystem.Error(ex);
                            }
                        }
                    }
                }

                gridControl2.RefreshDataSource();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void btnG_DELETE_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                Inventec.Common.Integrate.CommonParam param = new Inventec.Common.Integrate.CommonParam();
                var rowData = (EMR_ATTACHMENT)gridView2.GetFocusedRow();
                if (MessageBox.Show("Bạn có muốn xóa dữ liệu", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (rowData != null)
                    {
                        bool success = false;
                        success = GlobalStore.EmrConsumer.Post<bool>(EMR.URI.EmrAttachment.DELETE, param, rowData.ID);
                        //success = new BackendAdapter(param).Post<bool>(EMR.URI.EmrAttachment.DELETE, ApiConsumers.MosConsumer, rowData.ID, param);
                        if (success)
                        {
                            loadgridView2(this.Document.ID);

                        }
                        MessageManager.Show(this, param, success);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }


        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                //List<string> fileExceed = new List<string>();
                //string fileExceed = "";
                OpenFileDialog openFile = new OpenFileDialog();

                openFile.Multiselect = true;
                openFile.Filter = "Ảnh(*.jpg, *.Png, *.jpeg, *.bmp)|*.jpg;*.png;*.jpeg;*.bmp|pdf(*.pdf)|*.pdf";
                openFile.DefaultExt = ".jpg;.png;.jpeg;.bmp";

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    this.fullfileNameAttack = openFile.FileNames;
                    if (this.fullfileNameAttack != null)
                    {
                        foreach (var item in this.fullfileNameAttack)
                        {                           
                            int lIndex = item.LastIndexOf("\\");
                            int lIndex1 = item.LastIndexOf(".");
                            this.fileNameAttack = new AttackADO();
                            this.fileNameAttack.FILE_NAME = item.Substring(lIndex > 0 ? lIndex + 1 : lIndex);
                            this.fileNameAttack.EXTENSION = item.Substring(lIndex1 > 0 ? lIndex1 + 1 : lIndex1);
                            this.fileNameAttack.Base64Data = Utils.FileToBase64String(item);
                            this.fileNameAttack.FullName = item;

                            this.ListfileNameAttack.Add(fileNameAttack);
                        }
                    }
                }
                gridView1.BeginUpdate();
                this.gridView1.GridControl.DataSource = ListfileNameAttack;
                gridView1.EndUpdate();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridView1_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    AttackADO AttackTDO = (AttackADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (AttackTDO != null)
                    {
                        if (e.Column.FieldName == "STT")
                        {
                            e.Value = e.ListSourceRowIndex + 1;
                        }
                        else if (e.Column.FieldName == "FILE_NAME")
                        {
                            e.Value = AttackTDO.FILE_NAME;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void btnGDELETE_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                AttackADO data = (AttackADO)gridView1.GetFocusedRow();
                if (MessageBox.Show("Bạn có muốn xóa dữ liệu không", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.ListfileNameAttack.Remove(data);

                    gridView1.BeginUpdate();
                    gridView1.GridControl.DataSource = (this.ListfileNameAttack != null ? this.ListfileNameAttack.ToList() : null);
                    gridView1.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<AttackADO> listError = new List<AttackADO>();
                if (this.ListfileNameAttack != null && ListfileNameAttack.Count > 0)
                {
                    string fileError = "";
                    foreach (var data1 in this.ListfileNameAttack)
                    {
                        EmrAttachmentSDO data = new EmrAttachmentSDO();
                        data.DocumentId = this.Document.ID;
                        data.Extension = data1.EXTENSION;
                        data.Base64Data = data1.Base64Data;
                        data.AttachmentName = data1.FILE_NAME;

                        string output = GeneratePdfFileFromImage(data1.FullName);

                        FileHolder file = new FileHolder();
                        file.FileName = output;
                        file.Content = GetMemoryStreamFileData(output);

                        if (data != null)
                        {
                            Inventec.Common.Integrate.CommonParam commonParam = new Inventec.Common.Integrate.CommonParam();

                            var apiData = GlobalStore.EmrConsumer.PostWithFile<EMR_ATTACHMENT>(EMR.URI.EmrAttachment.CREATE_WITH_FILE, commonParam, data, new List<FileHolder>() { file });
                          
                            
                            if (apiData == null)
                            {
                                listError.Add(data1);
                                fileError += data.AttachmentName + ",";

                                Inventec.Common.Logging.LogSystem.Debug("Goi api tao van ban " + (apiData != null ? "thanh cong" : "that bai") + "____Du lieu dau vao:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data) + "____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => output), output) + "____Ket qua tra ve:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => apiData), apiData) + "___" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => commonParam), commonParam));

                            }                            
                        }
                    }
                    this.ListfileNameAttack = listError;
                    gridView1.BeginUpdate();
                    gridView1.GridControl.DataSource = this.ListfileNameAttack;
                    gridView1.EndUpdate();
                    loadgridView2(this.Document.ID);
                    if (!string.IsNullOrEmpty(fileError))
                    {
                        MessageBox.Show("Các tập tin đính kèm sau không lưu được: " + fileError);
                    }

                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn tập tin đính kèm");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private MemoryStream GetMemoryStreamFileData(string outFile)
        {
            MemoryStream streamData = null;
            try
            {
                if (!String.IsNullOrEmpty(outFile))
                {
                    streamData = new MemoryStream();
                    using (FileStream file = new FileStream(outFile, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        streamData.Write(bytes, 0, (int)file.Length);
                    }
                    streamData.Position = 0;
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                streamData = null;
            }
            return streamData;
        }

        private string GeneratePdfFileFromImage(string filename)
        {
            string output = filename;
            try
            {


                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromFile(filename), BaseColor.BLACK);
                using (FileStream fs = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (Document doc = new Document(image))
                    {
                        using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
                        {
                            doc.Open();
                            image.SetAbsolutePosition(0, 0);
                            writer.DirectContent.AddImage(image);
                            doc.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return output;
        }

        private void bbtnChooseFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                btnChooseFile_Click(null, null);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void bbtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                btnSave_Click(null, null);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
