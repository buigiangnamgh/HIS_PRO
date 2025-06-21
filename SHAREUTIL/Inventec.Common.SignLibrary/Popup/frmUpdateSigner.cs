using DevExpress.XtraEditors.ViewInfo;
using EMR.EFMODEL.DataModels;
using EMR.SDO;
using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.Integrate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary.Popup
{
    public partial class frmUpdateSigner : Form
    {
        EMR_SIGNER EditData;

        int positionHandle = -1;

        /// <summary>
        /// sửa thông tin mã người ký
        /// </summary>
        /// <param name="data">người ký</param>
        /// <param name="type">thông báo</param>
        public frmUpdateSigner(EMR_SIGNER data, int type)
        {
            InitializeComponent();

            EditData = data;
            if (type == 1)
            {
                this.lblWaring.Text = "Mã người ký không hợp kệ. Vui lòng nhập và thực hiện ký lại";
            }
        }

        private void frmUpdateSigner_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.EditData == null)
                {
                    this.Close();
                }

                CodeValidationRule CodeRule = new CodeValidationRule();
                CodeRule.txt = txtHsmUserCode;
                this.dxValidationProvider1.SetValidationRule(txtHsmUserCode, CodeRule);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                positionHandle = -1;
                if (!dxValidationProvider1.Validate()) return;

                //save
                this.EditData.HSM_USER_CODE = this.txtHsmUserCode.Text.Trim();

                EmrSignerSDO updateData = new EmrSignerSDO();
                updateData.EmrSigner = this.EditData;
                if (this.EditData.SIGN_IMAGE != null)
                {
                    updateData.ImgBase64Data = Convert.ToBase64String(this.EditData.SIGN_IMAGE);
                }

                CommonParam paramCommon = new CommonParam();
                var apiResult = GlobalStore.EmrConsumer.Post<EMR_SIGNER>("api/EmrSigner/Update", paramCommon, updateData);
                if (apiResult != null)
                {
                    this.Close();
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramCommon), paramCommon)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => EditData), EditData));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void dxValidationProvider1_ValidationFailed(object sender, DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.BaseEdit edit = e.InvalidControl as DevExpress.XtraEditors.BaseEdit;
                if (edit == null)
                    return;

                BaseEditViewInfo viewInfo = edit.GetViewInfo() as BaseEditViewInfo;
                if (viewInfo == null)
                    return;

                if (positionHandle == -1)
                {
                    positionHandle = edit.TabIndex;
                    edit.SelectAll();
                    edit.Focus();
                }
                if (positionHandle > edit.TabIndex)
                {
                    positionHandle = edit.TabIndex;
                    edit.SelectAll();
                    edit.Focus();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            simpleButton1_Click(null, null);
        }
    }
}
