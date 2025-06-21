using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using EMR.EFMODEL.DataModels;
using EMR.Filter;
using EMR.SDO;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.ADO;
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

namespace Inventec.Common.SignLibrary
{
    public partial class FormEmrSign : Form
    {
        private string DocumentCode;
        private string LoginName;
        private long MaxOrder;
        private long MinOrder;
        V_EMR_DOCUMENT Document;
        List<ListSignConfigADO> ListDataSign;

        public FormEmrSign()
        {
            InitializeComponent();
        }

        public FormEmrSign(string documentCode)
        {
            InitializeComponent();
            try
            {
                this.DocumentCode = documentCode;
                this.LoginName = GlobalStore.LoginName;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void FormEmrSign_Load(object sender, EventArgs e)
        {
            try
            {
                LoadKeysFromlanguage();
                SetDefaultData();
                FillDataToControl();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetDefaultData()
        {
            try
            {
                //LblTitle.Text = "";

                Common.Integrate.CommonParam commonParam = new Common.Integrate.CommonParam();
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

        private void FillDataToControl()
        {
            try
            {
                WaitingManager.Show();
                ListDataSign = new List<ListSignConfigADO>();
                CommonParam param = new CommonParam();
                EmrSignViewFilter filter = new EmrSignViewFilter();
                filter.DOCUMENT_ID = this.Document != null ? this.Document.ID : 0;
                var apiressult = GlobalStore.EmrConsumer.Get<List<V_EMR_SIGN>>(EMR.URI.EmrSign.GET_VIEW, param, filter);
                //var apiressult = new BackendAdapter(param).Get<List<V_EMR_SIGN>>(EMR.URI.EmrSign.GET_VIEW, GlobalStore.EmrConsumer, filter, param);
                if (apiressult != null && apiressult.Count > 0)
                {
                    ListDataSign = new List<ListSignConfigADO>();
                    foreach (var item in apiressult)
                    {
                        ListSignConfigADO itemRs = new ListSignConfigADO(item);
                        itemRs.IdRow = itemRs.NUM_ORDER;
                        ListDataSign.Add(itemRs);
                    }

                    if (ListDataSign != null && ListDataSign.Count > 0)
                    {
                        MaxOrder = ListDataSign.Max(o => o.IdRow);
                        MinOrder = MaxOrder;
                        var lstmin = ListDataSign.Where(o => !o.SIGN_TIME.HasValue && !o.REJECT_TIME.HasValue).ToList();
                        if (lstmin != null && lstmin.Count > 0)
                        {
                            MinOrder = lstmin.Min(m => m.IdRow);
                        }

                        ListDataSign = ListDataSign.OrderBy(o => o.IdRow).ToList();
                    }
                }

                WaitingManager.Hide();
                gridControlSign.BeginUpdate();
                gridControlSign.DataSource = null;
                gridControlSign.DataSource = ListDataSign;
                gridControlSign.EndUpdate();
                WaitingManager.Hide();
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadKeysFromlanguage()
        {
           
        }

        private void gridViewSign_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != DevExpress.Data.UnboundColumnType.Bound)
                {
                    var data = (ListSignConfigADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (data != null)
                    {
                        if (e.Column.FieldName == "STT")
                        {
                            e.Value = e.ListSourceRowIndex + 1;
                        }
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
