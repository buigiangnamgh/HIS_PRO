using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using EMR.EFMODEL.DataModels;
using EMR.Filter;
using EMR.TDO;
using Inventec.Common.Integrate;
using Inventec.Common.Integrate.EditorLoader;
using Inventec.Common.SignLibrary.Api;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
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
    public partial class frmSignerAdd : Form
    {
        List<SignTDO> listSign;
        List<EMR_SIGNER> signers;
        List<EMR_SIGN_TEMP> signTemplates;
        Action<List<SignTDO>> actAfterSave;
        Action<bool> actAfterIsSignParanelCheckchanged;
        internal const long stepNumOrder = 1;
        bool isSignParanel;
        bool isEnablebbtnchkSignParanel;
        bool isNoActionWhileSignParanelchanged;
        string documentCode;
        EMR_TREATMENT treatment;
        EMR.EFMODEL.DataModels.EMR_SIGNER signer;
        bool IsAddPatientSign = false;
        public frmSignerAdd(List<SignTDO> listsign, Action<List<SignTDO>> actAfterSave, Action<bool> actAfterIsSignParanelCheckchanged, bool isSignParanel, bool isEnablebbtnchkSignParanel, string documentCode, EMR_TREATMENT _treatment, EMR.EFMODEL.DataModels.EMR_SIGNER _signer, bool IsAddPatientSign) : this(listsign, actAfterSave, actAfterIsSignParanelCheckchanged, isSignParanel, isEnablebbtnchkSignParanel, documentCode, _treatment, _signer)
        {
            try
            {
                this.IsAddPatientSign = IsAddPatientSign;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public frmSignerAdd(List<SignTDO> listsign, Action<List<SignTDO>> actAfterSave, Action<bool> actAfterIsSignParanelCheckchanged, bool isSignParanel, bool isEnablebbtnchkSignParanel, string documentCode, EMR_TREATMENT _treatment, EMR.EFMODEL.DataModels.EMR_SIGNER _signer)
        {
            try
            {
                InitializeComponent();

                this.listSign = listsign;
                this.actAfterSave = actAfterSave;
                this.actAfterIsSignParanelCheckchanged = actAfterIsSignParanelCheckchanged;
                this.isSignParanel = isSignParanel;
                this.isEnablebbtnchkSignParanel = isEnablebbtnchkSignParanel;
                this.documentCode = documentCode;
                this.treatment = _treatment;
                this.signer = _signer;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void frmSignerAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.isNoActionWhileSignParanelchanged = true;

                this.chkISignParanel.Checked = this.isSignParanel;
                this.chkISignParanel.Enabled = this.isEnablebbtnchkSignParanel;
                this.isNoActionWhileSignParanelchanged = false;

                this.InitComboCommon(this.cboSigner, this.GetSigner(), "ID", "USERNAME", "USERNAME", 150, "TITLE", 150, "DEPARTMENT_NAME", 200, "", 0);
                this.InitComboCommon(this.cboSignTemplate, this.GetSignTemplate(), "ID", "SIGN_TEMP_NAME", "SIGN_TEMP_CODE");

                gridView1.BeginUpdate();
                gridView1.GridControl.DataSource = this.listSign;
                gridView1.EndUpdate();
                if (!String.IsNullOrEmpty(this.documentCode))
                {
                    this.chkISignParanel.Enabled = false;
                }
                if (IsAddPatientSign && (this.listSign == null || this.listSign.Count == 0 || this.listSign.FirstOrDefault(o => o.PatientCode == treatment.PATIENT_CODE) == null))
                {
                    btnAddPatient_Click(null, null);
                    if (GlobalStore.EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION == "1")
                    {
                        this.chkISignParanel.Checked = this.chkISignParanel.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<EMR_SIGNER> GetSigner()
        {
            this.signers = new EmrSigner().Get().Where(o => o.IS_ACTIVE == 1).OrderBy(o => o.USERNAME).ThenBy(o => o.NUM_ORDER).ToList();
            return signers;
        }

        /// <summary>
        /// - Bổ sung combobox "Mẫu thiết lập ký": Load dữ liệu từ bảng EMR_SIGN_TEMP. Chỉ hiển thị các dữu liệu đang mở khóa và do người dùng tạo order theo mã mẫu.
        ///- Nếu người dùng chọn "Mẫu thiết lập ký thì thực hiện:
        /// </summary>
        /// <returns></returns>
        private List<EMR_SIGN_TEMP> GetSignTemplate()
        {
            EmrSignTempFilter filter = new EmrSignTempFilter();
            filter.IS_ACTIVE = 1;
            filter.ORDER_DIRECTION = "ASC";
            filter.ORDER_FIELD = "SIGN_TEMP_CODE";
            filter.CREATOR = this.signer.LOGINNAME;
            this.signTemplates = new EmrSignTemplate().Get(filter);

            return signTemplates;
        }

        private EMR.EFMODEL.DataModels.EMR_SIGNER GetSignerById(long id)
        {
            return this.signers.FirstOrDefault(o => o.ID == id);
        }

        private void FocusShowpopup(LookUpEdit cboEditor, bool isSelectFirstRow)
        {
            try
            {
                cboEditor.Focus();
                cboEditor.ShowPopup();
                if (isSelectFirstRow)
                    PopupLoader.SelectFirstRowPopup(cboEditor);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void FocusShowpopup(GridLookUpEdit cboEditor, bool isSelectFirstRow)
        {
            try
            {
                cboEditor.Focus();
                cboEditor.ShowPopup();
                if (isSelectFirstRow)
                    PopupLoader.SelectFirstRowPopup(cboEditor);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void InitComboCommon(Control cboEditor, object data, string valueMember, string displayMember, string displayMemberCode)
        {
            try
            {
                InitComboCommon(cboEditor, data, valueMember, displayMember, displayMember, 0, displayMemberCode, 0, "", 0, "", 0);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void InitComboCommon(Control cboEditor, object data, string valueMember, string displayMember, string col1Name, int col1Width, string col2Name, int col2Width, string col3Name, int col3Width, string col4Name, int col4Width)
        {
            try
            {
                int popupWidth = 0;
                int visibleIndex = 1;
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();
                if (!String.IsNullOrEmpty(col1Name))
                {
                    columnInfos.Add(new ColumnInfo(col1Name, "", (col1Width > 0 ? col1Width : 250), visibleIndex, true));
                    popupWidth += (col1Width > 0 ? col1Width : 250);
                    visibleIndex += 1;
                }
                if (!String.IsNullOrEmpty(col2Name))
                {
                    columnInfos.Add(new ColumnInfo(col2Name, "", (col2Width > 0 ? col2Width : 100), visibleIndex, true));
                    popupWidth += (col2Width > 0 ? col2Width : 100);
                    visibleIndex += 1;
                }
                if (!String.IsNullOrEmpty(col3Name))
                {
                    columnInfos.Add(new ColumnInfo(col3Name, "", (col3Width > 0 ? col3Width : 100), visibleIndex, true));
                    popupWidth += (col3Width > 0 ? col3Width : 100);
                    visibleIndex += 1;
                }
                if (!String.IsNullOrEmpty(col4Name))
                {
                    columnInfos.Add(new ColumnInfo(col4Name, "", (col4Width > 0 ? col4Width : 100), visibleIndex, true));
                    popupWidth += (col4Width > 0 ? col4Width : 100);
                    visibleIndex += 1;
                }
                ControlEditorADO controlEditorADO = new ControlEditorADO(displayMember, valueMember, columnInfos, false, popupWidth);

                ControlEditorLoader.Load(cboEditor, data, controlEditorADO);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        long GetMaxNumOrder()
        {
            long max = stepNumOrder;
            if (this.listSign != null && this.listSign.Count > 0)
            {
                max = this.listSign.Max(o => o.NumOrder) + stepNumOrder;
            }
            return max;
        }

        bool CheckExistsPatientSign()
        {
            bool valid = false;
            try
            {
                valid = this.listSign.Any(o => o.PatientCode == treatment.PATIENT_CODE);
            }
            catch (Exception ex)
            {
                valid = false;
            }
            return valid;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSigner.EditValue == null)
                {
                    MessageBox.Show("Chưa chọn người ký");
                    FocusShowpopup(cboSigner, false);
                    return;
                }

                SignTDO sign = new SignTDO();
                EMR_SIGNER signerFind = GetSignerById((long)cboSigner.EditValue);

                sign.SignerId = signerFind.ID;
                sign.Loginname = signerFind.LOGINNAME;
                sign.Username = signerFind.USERNAME;
                sign.FullName = signerFind.USERNAME;
                sign.FirstName = signerFind.USERNAME;
                sign.NumOrder = GetMaxNumOrder();
                sign.Title = signerFind.TITLE;
                sign.DepartmentCode = signerFind.DEPARTMENT_CODE;
                sign.DepartmentName = signerFind.DEPARTMENT_NAME;
                sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                if (this.listSign == null)
                    this.listSign = new List<SignTDO>();

                this.listSign.Add(sign);

                gridView1.BeginUpdate();
                gridView1.GridControl.DataSource = this.listSign.OrderBy(o => o.NumOrder).ToList();
                gridView1.EndUpdate();

                cboSigner.EditValue = null;
                txtLoginName.Text = "";
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
                if (this.actAfterSave != null)
                {
                    var bchk = ((this.listSign != null && this.listSign.Count > 0) ? this.listSign.Any(o => o.NumOrder > 0) : true);
                    if (bchk)
                    {
                        this.actAfterSave(this.listSign);
                        this.Close();
                    }
                    else
                    {
                        MessageManager.Show("Danh sách người ký phải gán thứ tự ký");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckExistsPatientSign())
                {
                    MessageManager.Show(string.Format(MessageUitl.GetMessage(MessageConstan.BenhNhanDaCoTrongLuongKy), treatment.PATIENT_CODE));
                    return;
                }

                if (this.listSign == null)
                    this.listSign = new List<SignTDO>();
                SignTDO sign = new SignTDO();
                sign.Username = treatment.VIR_PATIENT_NAME;
                if (GlobalStore.EMR_EMR_DOCUMENT_PATIENT_SIGN_FIRST_OPTION == "1")
                {
                    sign.NumOrder = 1;
                    if (listSign != null && listSign.Count > 0)
                        listSign.ForEach(o => o.NumOrder++);
                }
                else
                    sign.NumOrder = GetMaxNumOrder();
                sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                sign.PatientCode = treatment.PATIENT_CODE;
                sign.FirstName = treatment.FIRST_NAME;
                sign.LastName = treatment.LAST_NAME;
                sign.FullName = treatment.VIR_PATIENT_NAME;


                this.listSign.Add(sign);

                gridView1.BeginUpdate();
                gridView1.GridControl.DataSource = this.listSign.OrderBy(o => o.NumOrder).ToList();
                gridView1.EndUpdate();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                SignTDO data = (SignTDO)gridView1.GetFocusedRow();

                if (MessageBox.Show("Bạn có muốn xóa dữ liệu không", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.listSign.Remove(data);

                    gridView1.BeginUpdate();
                    gridView1.GridControl.DataSource = (this.listSign != null ? this.listSign.OrderBy(o => o.NumOrder).ToList() : null);
                    gridView1.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound)
                {
                    SignTDO signTDO = (SignTDO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
                    if (signTDO != null)
                    {
                        if (e.Column.FieldName == "UsernameDisplay")
                        {
                            e.Value = (String.IsNullOrEmpty(signTDO.Loginname) ? signTDO.FullName + " (Bệnh nhân)" : signTDO.Username);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void txtLoginName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string searchCode = (sender as DevExpress.XtraEditors.TextEdit).Text.ToUpper();
                    if (String.IsNullOrEmpty(searchCode))
                    {
                        this.cboSigner.EditValue = null;
                        this.FocusShowpopup(cboSigner, true);
                    }
                    else
                    {
                        var data = this.signers
                            .Where(o => o.IS_ACTIVE == 1 && o.LOGINNAME.ToUpper().Contains(searchCode.ToUpper())).ToList();

                        var searchResult = (data != null && data.Count > 0) ? (data.Count == 1 ? data : data.Where(o => o.LOGINNAME.ToUpper() == searchCode.ToUpper()).ToList()) : null;
                        if (searchResult != null && searchResult.Count == 1)
                        {
                            this.cboSigner.EditValue = searchResult[0].ID;
                            this.txtLoginName.Text = searchResult[0].LOGINNAME;
                            this.btnAdd.Focus();
                            e.Handled = true;
                        }
                        else
                        {
                            this.cboSigner.EditValue = null;
                            this.FocusShowpopup(cboSigner, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboSigner_Closed(object sender, ClosedEventArgs e)
        {
            try
            {
                if (e.CloseMode == PopupCloseMode.Normal)
                {
                    if (this.cboSigner.EditValue != null)
                    {
                        var data = this.signers.FirstOrDefault(o => o.IS_ACTIVE == 1 && o.ID == TypeConvertParse.ToInt64((this.cboSigner.EditValue ?? "").ToString()));
                        if (data != null)
                        {
                            this.txtLoginName.Text = data.LOGINNAME;
                        }
                    }

                    this.btnAdd.Focus();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboSigner_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.cboSigner.EditValue != null)
                    {
                        var data = this.signers.FirstOrDefault(o => o.IS_ACTIVE == 1 && o.ID == TypeConvertParse.ToInt64((this.cboSigner.EditValue ?? "").ToString()));
                        if (data != null)
                        {
                            this.txtLoginName.Text = data.LOGINNAME;
                            this.btnAdd.Focus();
                        }
                    }
                }
                else
                {
                    this.cboSigner.ShowPopup();
                    PopupLoader.SelectFirstRowPopup(this.cboSigner);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void chkISignParanel_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.isNoActionWhileSignParanelchanged)
                {
                    return;
                }

                if (actAfterIsSignParanelCheckchanged != null)
                {
                    actAfterIsSignParanelCheckchanged(chkISignParanel.Checked);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboSignTemplate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.cboSignTemplate.EditValue != null)
                    {
                        var data = this.signTemplates.FirstOrDefault(o => o.IS_ACTIVE == 1 && o.ID == TypeConvertParse.ToInt64((this.cboSignTemplate.EditValue ?? "").ToString()));
                        if (data != null)
                        {
                            ProcessSelectSignTemp(data);
                            this.btnAdd.Focus();
                        }
                    }
                }
                else
                {
                    this.cboSigner.ShowPopup();
                    PopupLoader.SelectFirstRowPopup(this.cboSigner);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        void ProcessSelectSignTemp(EMR_SIGN_TEMP signTemp)
        {
            gridControl1.DataSource = null;
            this.listSign = new List<SignTDO>();

            var signOrders = new EmrSignOrder().GetByTemp(signTemp.ID);
            if (signOrders != null && signOrders.Count > 0)
            {
                foreach (var item in signOrders)
                {
                    SignTDO sign = new SignTDO();
                    if (item.SIGNER_ID.HasValue && item.SIGNER_ID.Value > 0 && (item.IS_PATIENT_SIGN ?? 0) != 1)
                    {
                        EMR_SIGNER signerFind = GetSignerById(item.SIGNER_ID.Value);
                        if (signerFind != null)
                        {
                            sign.SignerId = signerFind.ID;
                            sign.Loginname = signerFind.LOGINNAME;
                            sign.Username = signerFind.USERNAME;
                            sign.FullName = signerFind.USERNAME;
                            sign.FirstName = signerFind.USERNAME;

                            sign.Title = signerFind.TITLE;
                            sign.DepartmentCode = signerFind.DEPARTMENT_CODE;
                            sign.DepartmentName = signerFind.DEPARTMENT_NAME;
                        }
                        else
                        {
                            Inventec.Common.Logging.LogSystem.Info("ProcessSelectSignTemp: khong tim thay signer theo thong tin signorder____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            continue;
                        }
                    }
                    else if (item.IS_PATIENT_SIGN.HasValue && item.IS_PATIENT_SIGN.Value == 1)
                    {
                        if (CheckExistsPatientSign())
                        {
                            Inventec.Common.Logging.LogSystem.Info("ProcessSelectSignTemp.CheckExistsPatientSign: da co thiet lap benh nhan ky__du lieu bi trung lap__can kiem tra lai____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => signOrders), signOrders));
                            continue;
                        }

                        sign.Username = treatment.VIR_PATIENT_NAME;
                        sign.PatientCode = treatment.PATIENT_CODE;
                        sign.FirstName = treatment.FIRST_NAME;
                        sign.LastName = treatment.LAST_NAME;
                        sign.FullName = treatment.VIR_PATIENT_NAME;
                    }
                    sign.NumOrder = item.NUM_ORDER;
                    sign.SignTime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                    if (this.listSign == null)
                        this.listSign = new List<SignTDO>();

                    this.listSign.Add(sign);
                }

                gridView1.BeginUpdate();
                gridView1.GridControl.DataSource = this.listSign.OrderBy(o => o.NumOrder).ToList();
                gridView1.EndUpdate();
            }
        }

        private void cboSignTemplate_Closed(object sender, ClosedEventArgs e)
        {
            try
            {
                if (e.CloseMode == PopupCloseMode.Normal)
                {
                    if (this.cboSignTemplate.EditValue != null)
                    {
                        var data = this.signTemplates.FirstOrDefault(o => o.IS_ACTIVE == 1 && o.ID == TypeConvertParse.ToInt64((this.cboSignTemplate.EditValue ?? "").ToString()));
                        if (data != null)
                        {
                            ProcessSelectSignTemp(data);
                        }
                    }

                    this.btnAdd.Focus();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

    }
}
