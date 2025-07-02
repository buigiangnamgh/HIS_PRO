using DevExpress.XtraPdfViewer.Commands;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Common.Message;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIS.Desktop.Plugins.HisProductInfo
{
    public partial class frmProductInfo : FormBase
    {
        Inventec.Desktop.Common.Modules.Module module = null;
        ProductInfoADO data = null;
        OpenFileDialog openFile = new OpenFileDialog();
        HIS_PRODUCT_INFO currentProductInfo = null;
        string fileName = "";
        public frmProductInfo()
        {
            InitializeComponent();
            string iconPath = System.IO.Path.Combine(HIS.Desktop.LocalStorage.Location.ApplicationStoreLocation.ApplicationStartupPath, System.Configuration.ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]);
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(iconPath);
        }
        public frmProductInfo(Inventec.Desktop.Common.Modules.Module _module, ProductInfoADO ado)
            : base(_module)
        {
            InitializeComponent();
            string iconPath = System.IO.Path.Combine(HIS.Desktop.LocalStorage.Location.ApplicationStoreLocation.ApplicationStartupPath, System.Configuration.ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]);
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(iconPath);
            this.module = _module;
            this.data = ado;
            this.fileName = System.Guid.NewGuid().ToString() + Inventec.Common.DateTime.Get.Now() + ".pdf";
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                openFile.Filter = "PDF file(*.pdf)|*.pdf";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    pdfViewer1.DocumentFilePath = openFile.FileName;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void frmProductInfo_Load(object sender, EventArgs e)
        {
            try
            {
                SetDefaultValue();
                CommonParam param = new CommonParam();
                MOS.Filter.HisProductInfoFilter filter = new MOS.Filter.HisProductInfoFilter();
                filter.MEDICINE_TYPE_ID = data.MedicineTypeId;
                var rs = new BackendAdapter(param).Get<List<MOS.EFMODEL.DataModels.HIS_PRODUCT_INFO>>("api/HisProductInfo/Get", ApiConsumers.MosConsumer, filter, param);
                if (rs != null)
                {
                    this.currentProductInfo = rs.FirstOrDefault();
                    MemoryStream streamSource = new MemoryStream();
                    streamSource = Inventec.Fss.Client.FileDownload.GetFile("Upload/HIS/ProductInfo/" + rs.FirstOrDefault().PRODUCT_INFO);
                    pdfViewer1.LoadDocument(streamSource);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetDefaultValue()
        {
            try
            {
                this.pdfViewer1.DocumentFilePath = "";
                this.btnSave.Enabled = data != null && data.ProductInfoOpen == 1;
                this.btnChooseFile.Enabled = data != null && data.ProductInfoOpen == 1;

            }
            catch (Exception ex)
            {

                LogSystem.Error(ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PrcocessSave();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void PrcocessSave()
        {
            try
            {
                // xoa file cu
                if (this.currentProductInfo != null)
                {
                    //bool isDelete = Inventec.Fss.Client.FileDelete.DeleteFile(GlobalVariables.APPLICATION_CODE, "ProductInfo/" + this.currentProductInfo.PRODUCT_INFO, HIS.Desktop.LocalStorage.ConfigSystem.ConfigSystems.URI_API_FSS);
                    //Inventec.Common.Logging.LogSystem.Debug("Delete file " + this.currentProductInfo.PRODUCT_INFO + " ___ RESULT: " + isDelete);
                }                
                Inventec.Fss.Utility.FileUploadInfo fileResults;
                Inventec.Common.Logging.LogSystem.Debug("this.openFile.FileName " + this.openFile.FileName);
                Byte[] file = System.IO.File.ReadAllBytes(this.openFile.FileName);
                using (MemoryStream mm = new MemoryStream(file))
                {
                    mm.ReadByte();
                    mm.Position = 0;
                    Inventec.Common.Logging.LogSystem.Debug("HIS.Desktop.LocalStorage.ConfigSystem.ConfigSystems.URI_API_FSS " + HIS.Desktop.LocalStorage.ConfigSystem.ConfigSystems.URI_API_FSS);
                    Inventec.Common.Logging.LogSystem.Debug("GlobalVariables.APPLICATION_CODE " + GlobalVariables.APPLICATION_CODE);
                    fileResults = Inventec.Fss.Client.FileUpload.UploadFile(GlobalVariables.APPLICATION_CODE, "ProductInfo", mm, this.fileName, true);
                }
                if (fileResults != null)
                {
                    CommonParam param = new CommonParam();
                    bool success = false;
                    LogSystem.Debug("ProcessSave -> 1");
                    if (this.currentProductInfo != null)
                    {
                        //update
                        HIS_PRODUCT_INFO updateData = this.currentProductInfo;
                        updateData.PRODUCT_INFO = this.fileName;
                        LogSystem.Debug("du lieu gui len API update. " + LogUtil.TraceData("updateData", updateData));
                        var rs = new BackendAdapter(param).Post<HIS_PRODUCT_INFO>("/api/HisProductInfo/Update", ApiConsumers.MosConsumer, updateData, param);
                        if (rs != null)
                        {
                            success = true;
                            this.currentProductInfo = rs;
                            //EnableControlChange();
                        }
                    }
                    else
                    {
                        //create
                        HIS_PRODUCT_INFO createData = new HIS_PRODUCT_INFO();
                        createData.MEDICINE_TYPE_ID = this.data.MedicineTypeId;
                        createData.PRODUCT_INFO = this.fileName;
                        LogSystem.Debug("du lieu gui len API create. " + LogUtil.TraceData("createData", createData));
                        var rs = new BackendAdapter(param).Post<HIS_PRODUCT_INFO>("api/HisProductInfo/Create", ApiConsumers.MosConsumer, createData, param);
                        if (rs != null)
                        {
                            success = true;
                            this.currentProductInfo = rs;
                            //EnableControlChange();
                        }
                    }
                    LogSystem.Debug("ProcessSave -> end");
                    MessageManager.Show(this, param, success);
                }
            }
            catch (Exception ex)
            {
                PdfZoomInCommand command = new PdfZoomInCommand(pdfViewer1);
                command.Execute();
                LogSystem.Error(ex);
            }
        }

        private void pdfViewer1_ZoomChanged(object sender, DevExpress.XtraPdfViewer.PdfZoomChangedEventArgs e)
        {
            if (e.ZoomMode == DevExpress.XtraPdfViewer.PdfZoomMode.ActualSize)
            {

            }
        }
    }
}
