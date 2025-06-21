using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.ViewInfo;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventec.Common.Controls.EditorLoader;
using EMR.EFMODEL.DataModels;
using System.Threading;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.LibraryMessage;
using Inventec.Common.Integrate;
using EMR.SDO;
using Inventec.Common.SignLibrary.Integrate;

namespace Inventec.Common.SignLibrary
{
    public partial class frmCreateEmrBusiness : Form
    {
        private int rowFocus;
        private int positionHandleControl;
        Action<EMR_BUSINESS> actChoose;
        EMR_BUSINESS currentBusiness { get; set; }
        List<EMR.EFMODEL.DataModels.EMR_BUSINESS> emrBusiness { get; set; }
        List<EMR_FLOW> emrFlowAll { get; set; }
        List<EMR_FLOW> emrFlow { get; set; }
        List<EMR_SIGNER> emrSigner { get; set; }
        List<EMR_SIGNER_FLOW> emrSignerFlow { get; set; }
        List<EmrBusinessADO> lstEmrBusiness { get; set; }
        public frmCreateEmrBusiness(List<EMR.EFMODEL.DataModels.EMR_BUSINESS> emrBusiness, Action<EMR_BUSINESS> actChoose)
        {
            InitializeComponent();
            try
            {
                this.actChoose = actChoose;
                this.emrBusiness = emrBusiness;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void frmCreateEmrBusiness_Load(object sender, EventArgs e)
        {
            try
            {
                CreatThreadLoadData();
                ValidationSingleControlWithMaxLength(txtBusiness, true, 500);
                LoadComboSampleBusiness();
                LoadDefaultCombo();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private void LoadDefaultCombo()
        {
            try
            {
                LoadComboFlow(emrFlow);
                LoadComboSigner(emrSigner);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private void CreatThreadLoadData()
        {
            Thread threadLoadFlow = new Thread(new ThreadStart(LoadFlow));
            Thread threadLoadSigner = new Thread(new ThreadStart(LoadSigner));
            Thread threadLoadBusiness = new Thread(new ThreadStart(LoadBusiness));
            Thread threadLoadSignerFlow = new Thread(new ThreadStart(LoadSignerFlow));
            try
            {
                threadLoadSignerFlow.Start();
                threadLoadFlow.Start();
                threadLoadSigner.Start();
                threadLoadBusiness.Start();
                threadLoadSignerFlow.Join();
                threadLoadFlow.Join();
                threadLoadSigner.Join();
                threadLoadBusiness.Join();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                threadLoadSignerFlow.Abort();
                threadLoadFlow.Abort();
                threadLoadSigner.Abort();
                threadLoadBusiness.Abort();
            }
        }

        private void LoadSignerFlow()
        {
            try
            {
                emrSignerFlow = (new Inventec.Common.SignLibrary.Api.EmrSignerFlow()).Get(new EMR.Filter.EmrSignerFlowFilter());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadBusiness()
        {
            try
            {
                emrBusiness = emrBusiness.Where(o => o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.CREATOR == Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetLoginName()).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadSigner()
        {
            try
            {
                emrSigner = (new Inventec.Common.SignLibrary.Api.EmrSigner()).Get(new EMR.Filter.EmrSignerFilter() { IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE }).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadFlow()
        {
            try
            {
                emrFlowAll = (new Inventec.Common.SignLibrary.Api.EmrFlow()).Get(new EMR.Filter.EmrFlowFilter()).ToList();
                if(emrFlowAll != null && emrFlowAll.Count > 0)
                {
                    emrFlow = emrFlowAll.Where(o => o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.CREATOR == Inventec.UC.Login.Base.ClientTokenManagerStore.ClientTokenManager.GetLoginName()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void LoadComboSampleBusiness()
        {
            try
            {
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();
                columnInfos.Add(new ColumnInfo("BUSINESS_CODE", "", 100, 1));
                columnInfos.Add(new ColumnInfo("BUSINESS_NAME", "", 300, 2));
                ControlEditorADO controlEditorADO = new ControlEditorADO("BUSINESS_NAME", "ID", columnInfos, false, 400);
                ControlEditorLoader.Load(this.cboSample, emrBusiness, controlEditorADO);
                this.cboSample.Properties.ImmediatePopup = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        private void LoadComboFlow(List<EMR_FLOW> data)
        {
            try
            {
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();
                columnInfos.Add(new ColumnInfo("FLOW_CODE", "", 100, 1));
                columnInfos.Add(new ColumnInfo("FLOW_NAME", "", 300, 2));
                ControlEditorADO controlEditorADO = new ControlEditorADO("FLOW_NAME", "ID", columnInfos, false, 400);
                ControlEditorLoader.Load(this.cboFlow, data, controlEditorADO);
                this.cboFlow.Properties.ImmediatePopup = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        private void LoadComboSigner(List<EMR_SIGNER> data)
        {
            try
            {
                List<ColumnInfo> columnInfos = new List<ColumnInfo>();
                columnInfos.Add(new ColumnInfo("LOGINNAME", "", 100, 1));
                columnInfos.Add(new ColumnInfo("USERNAME", "", 300, 2));
                ControlEditorADO controlEditorADO = new ControlEditorADO("USERNAME", "ID", columnInfos, false, 400);
                ControlEditorLoader.Load(this.cboLoginName, data, controlEditorADO);
                this.cboLoginName.Properties.ImmediatePopup = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        public void ValidationSingleControlWithMaxLength(Control control, bool isRequired, int? maxLength)
        {
            try
            {
                Inventec.Desktop.Common.Controls.ValidationRule.ControlMaxLengthValidationRule icdMainRule = new Inventec.Desktop.Common.Controls.ValidationRule.ControlMaxLengthValidationRule();
                icdMainRule.editor = control;
                icdMainRule.maxLength = maxLength;
                icdMainRule.IsRequired = isRequired;
                icdMainRule.ErrorType = ErrorType.Warning;
                dxValidationProvider1.SetValidationRule(control, icdMainRule);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void dxValidationProvider1_ValidationFailed(object sender, ValidationFailedEventArgs e)
        {
            try
            {
                BaseEdit edit = e.InvalidControl as BaseEdit;
                if (edit == null)
                    return;

                BaseEditViewInfo viewInfo = edit.GetViewInfo() as BaseEditViewInfo;
                if (viewInfo == null)
                    return;

                if (this.positionHandleControl == -1)
                {
                    this.positionHandleControl = edit.TabIndex;
                    if (edit.Visible)
                    {
                        edit.SelectAll();
                        edit.Focus();
                    }
                }
                if (this.positionHandleControl > edit.TabIndex)
                {
                    this.positionHandleControl = edit.TabIndex;
                    if (edit.Visible)
                    {
                        edit.SelectAll();
                        edit.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboFlow_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        private void AddSignerFlow(List<EMR_SIGNER_FLOW> emrSignerIds)
        {
            try
            {
                if (AddKey(emrSignerIds.First().FLOW_ID))
                {
                    var emrSigner = this.emrSigner.Where(o => emrSignerIds.Select(p => p.SIGNER_ID).ToList().Exists(p => p == o.ID)).ToList();
                    var flow = emrFlow.FirstOrDefault(o => o.ID == emrSignerIds.First().FLOW_ID);
                    foreach (var item in emrSigner)
                    {
                        AddChild(flow, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboSample_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                if (cboSample.EditValue != null)
                {
                    var listEmrFlow = emrFlowAll.Where(o => o.BUSINESS_ID == Int64.Parse(cboSample.EditValue.ToString())).ToList();
                    lstEmrBusiness = new List<EmrBusinessADO>();
                    LoadDataTree();
                    foreach (var flow in listEmrFlow)
                    {
                        List<EMR_SIGNER_FLOW> emrSignerIds = emrSignerFlow.Where(o => o.FLOW_ID == flow.ID).ToList();
                        if (emrSignerIds != null && emrSignerIds.Count > 0)
                        {
                            AddSignerFlow(emrSignerIds);
                        }
                        else
                        {
                            AddKey(flow.ID);
                        }
                    }
                    LoadDataTree();
                    currentBusiness = emrBusiness.FirstOrDefault(o => o.ID == Int64.Parse(cboSample.EditValue.ToString()));
                    if (string.IsNullOrEmpty(txtBusiness.Text.Trim()))
                        txtBusiness.Text = currentBusiness.BUSINESS_NAME;
                    cboFlow.EditValue = null;
                    cboLoginName.EditValue = null;
                    txtLoginName.Text = null;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboSample_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
                {
                    currentBusiness = null;
                    cboSample.EditValue = null;
                    LoadDefaultCombo();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboFlow_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboFlow.EditValue != null)
                    btnFlow.Enabled = true;
                else
                    btnFlow.Enabled = false;
                cboLoginName.EditValue = null;
                txtLoginName.Text = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboLoginName_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboLoginName.EditValue != null)
                    btnLoginName.Enabled = true;
                else
                    btnLoginName.Enabled = false;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboFlow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
                {
                    cboFlow.EditValue = null;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboLoginName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
                {
                    cboLoginName.EditValue = null;
                    txtLoginName.Text = null;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void txtLoginName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!string.IsNullOrEmpty(txtLoginName.Text.Trim()))
                    {
                        var dataSource = (cboLoginName.Properties.DataSource) as List<EMR_SIGNER>;
                        if (dataSource != null && dataSource.Count > 0)
                        {
                            var signer = dataSource.FirstOrDefault(o => o.LOGINNAME.Equals(txtLoginName.Text.Trim()));
                            if (signer != null)
                            {
                                cboLoginName.Focus();
                                cboLoginName.EditValue = signer.ID;
                                return;
                            }
                        }
                    }
                    cboLoginName.Focus();
                    cboLoginName.ShowPopup();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void btnFlow_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboFlow.EditValue != null)
                {
                    List<EMR_SIGNER_FLOW> emrSignerIds = emrSignerFlow.Where(o => o.FLOW_ID == Int64.Parse(cboFlow.EditValue.ToString())).ToList();
                    if (emrSignerIds != null && emrSignerIds.Count > 0)
                    {
                        AddSignerFlow(emrSignerIds);
                        LoadDataTree();
                    }
                    else
                    {
                        if (AddKey(Int64.Parse(cboFlow.EditValue.ToString())))
                            LoadDataTree();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }


        private bool AddKey(long flowId)
        {
            try
            {
                if (lstEmrBusiness == null)
                    lstEmrBusiness = new List<EmrBusinessADO>();
                var flowExist = lstEmrBusiness.FirstOrDefault(o => o.FLOW_ID == flowId);
                if (flowExist != null)
                {
                    Inventec.Common.SignLibrary.Integrate.MessageManager.Show(String.Format("Danh sách thiết lập ký đã có vai trò {0}.", flowExist.FLOW_NAME));
                    return false;
                }
                var flow = (cboFlow.Properties.DataSource as List<EMR_FLOW>).FirstOrDefault(o => o.ID == flowId);
                EmrBusinessADO ado = new EmrBusinessADO();
                ado.CONCRETE_ID__IN_SETY = flow.ID.ToString();
                ado.USER_NAME = flow.FLOW_NAME;
                ado.FLOW_ID = flow.ID;
                ado.FLOW_CODE = flow.FLOW_CODE;
                ado.FLOW_NAME = flow.FLOW_NAME;
                ado.ROOM_CODE = flow.ROOM_CODE;
                ado.ROOM_NAME = flow.ROOM_NAME;
                ado.ROOM_TYPE_CODE = flow.ROOM_TYPE_CODE;
                if (lstEmrBusiness.Count == 0)
                    ado.NUM_ORDER = 1;
                else
                {
                    ado.NUM_ORDER = lstEmrBusiness.LastOrDefault(o => string.IsNullOrEmpty(o.PARENT_ID__IN_SETY)).NUM_ORDER + 1;
                }
                lstEmrBusiness.Add(ado);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return true;
        }
        private void AddChild(EMR_FLOW flow, EMR_SIGNER singer)
        {
            try
            {
                if (flow == null)
                    return;
                EmrBusinessADO ado = new EmrBusinessADO();
                ado.PARENT_ID__IN_SETY = flow.ID.ToString();
                ado.CONCRETE_ID__IN_SETY = singer.ID.ToString() + Guid.NewGuid().ToString();
                ado.USER_NAME = singer.USERNAME;
                ado.IS_LEAF = true;
                ado.SIGNER_ID = singer.ID;
                ado.TITLE = singer.TITLE;
                ado.DEPARTMENT_NAME = singer.DEPARTMENT_NAME;
                lstEmrBusiness.Add(ado);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void btnLoginName_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstEmrBusiness != null && lstEmrBusiness.Count > 0)
                {
                    var data = treeList1.GetDataRecordByNode(treeList1.FocusedNode);
                    if (cboLoginName.EditValue != null && data != null && data is EmrBusinessADO)
                    {
                        rowFocus = treeList1.GetVisibleIndexByNode(treeList1.FocusedNode);
                        var signer = (cboLoginName.Properties.DataSource as List<EMR_SIGNER>).FirstOrDefault(o => o.ID == Int64.Parse(cboLoginName.EditValue.ToString()));
                        var node = data as EmrBusinessADO;
                        if (!node.IS_LEAF)
                            AddChild(emrFlow.FirstOrDefault(o => o.ID == node.FLOW_ID), signer);
                        else
                            AddChild(emrFlow.FirstOrDefault(o => o.ID == lstEmrBusiness.FirstOrDefault(p => p.CONCRETE_ID__IN_SETY == node.PARENT_ID__IN_SETY).FLOW_ID), signer);
                        LoadDataTree();
                        treeList1.SetFocusedNode(treeList1.GetNodeByVisibleIndex(rowFocus));
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
        private void LoadDataTree()
        {
            try
            {
                if (lstEmrBusiness != null && lstEmrBusiness.Count > 0)
                    lstEmrBusiness = lstEmrBusiness.OrderBy(o => !o.IS_LEAF).ThenBy(o => o.NUM_ORDER).ToList();
                var records = new BindingList<EmrBusinessADO>(lstEmrBusiness);
                treeList1.DataSource = records;
                treeList1.ExpandAll();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void treeList1_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            try
            {
                var data = treeList1.GetDataRecordByNode(e.Node);
                if (data != null && data is EmrBusinessADO)
                {
                    var rowData = data as EmrBusinessADO;
                    if (rowData != null && !rowData.IS_LEAF)
                    {
                        e.Appearance.Font = new Font(e.Appearance.Font.FontFamily, e.Appearance.Font.Size, FontStyle.Bold);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void treeList1_CustomNodeCellEdit(object sender, DevExpress.XtraTreeList.GetCustomNodeCellEditEventArgs e)
        {
            try
            {
                var data = treeList1.GetDataRecordByNode(e.Node);
                if (data != null && data is EmrBusinessADO)
                {
                    var rowData = data as EmrBusinessADO;
                    if (rowData != null && !rowData.IS_LEAF)
                    {
                        if (e.Column.FieldName == "UP")
                            e.RepositoryItem = repUp;
                        else if (e.Column.FieldName == "DOWN")
                            e.RepositoryItem = repDown;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void repDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var data = treeList1.GetDataRecordByNode(treeList1.FocusedNode);
                if (data != null && data is EmrBusinessADO)
                {
                    var business = data as EmrBusinessADO;
                    lstEmrBusiness = lstEmrBusiness.Where(o => o.CONCRETE_ID__IN_SETY != business.CONCRETE_ID__IN_SETY).ToList();
                    if (!business.IS_LEAF)
                    {
                        lstEmrBusiness = lstEmrBusiness.Where(o => o.PARENT_ID__IN_SETY != business.CONCRETE_ID__IN_SETY).ToList();
                    }
                    if (lstEmrBusiness != null && lstEmrBusiness.Count > 0)
                    {
                        int index = 1;
                        lstEmrBusiness.ForEach(o =>
                        {
                            if (string.IsNullOrEmpty(o.PARENT_ID__IN_SETY))
                                o.NUM_ORDER = index++;
                        });
                    }
                    LoadDataTree();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void repDown_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var data = treeList1.GetDataRecordByNode(treeList1.FocusedNode);
                if (data != null && data is EmrBusinessADO)
                {
                    var business = data as EmrBusinessADO;
                    if (!business.IS_LEAF)
                    {
                        var lstParent = lstEmrBusiness.Where(o => string.IsNullOrEmpty(o.PARENT_ID__IN_SETY)).ToList();
                        if (lstParent.Count > 1 && lstParent.Last().CONCRETE_ID__IN_SETY != business.CONCRETE_ID__IN_SETY)
                        {
                            bool nextItem = false;
                            lstEmrBusiness.ForEach(o =>
                            {
                                if (!o.IS_LEAF && nextItem)
                                {
                                    o.NUM_ORDER = o.NUM_ORDER - 1;
                                    nextItem = false;
                                }
                                if (o.CONCRETE_ID__IN_SETY == business.CONCRETE_ID__IN_SETY)
                                {
                                    o.NUM_ORDER = o.NUM_ORDER + 1;
                                    nextItem = true;
                                }

                            });
                            LoadDataTree();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void repUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var data = treeList1.GetDataRecordByNode(treeList1.FocusedNode);
                if (data != null && data is EmrBusinessADO)
                {
                    var business = data as EmrBusinessADO;
                    if (!business.IS_LEAF)
                    {
                        var lstParent = lstEmrBusiness.Where(o => string.IsNullOrEmpty(o.PARENT_ID__IN_SETY)).ToList();
                        if (lstParent.Count > 1 && lstParent.First().CONCRETE_ID__IN_SETY != business.CONCRETE_ID__IN_SETY)
                        {
                            for (int i = 0; i < lstEmrBusiness.Count; i++)
                            {
                                var item = lstEmrBusiness[i];
                                if (!item.IS_LEAF && item.CONCRETE_ID__IN_SETY == business.CONCRETE_ID__IN_SETY)
                                {
                                    item.NUM_ORDER = item.NUM_ORDER - 1;
                                    lstEmrBusiness.FirstOrDefault(o => !o.IS_LEAF && o.NUM_ORDER == item.NUM_ORDER).NUM_ORDER = item.NUM_ORDER + 1;
                                    break;
                                }
                            }
                            LoadDataTree();
                        }
                    }
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
                positionHandleControl = -1;
                if (!dxValidationProvider1.Validate())
                    return;
                WaitingManager.Show();
                CommonParam param = new CommonParam();
                EmrBusinessCreateSDO sdo = new EmrBusinessCreateSDO();
                sdo.BusinessName = txtBusiness.Text.Trim();
                if (lstEmrBusiness != null && lstEmrBusiness.Count > 0)
                {
                    List<EmrFlowCreateSDO> lstFlow = new List<EmrFlowCreateSDO>();
                    var parent = lstEmrBusiness.Where(o => !o.IS_LEAF).ToList();
                    foreach (var item in parent)
                    {
                        EmrFlowCreateSDO flow = new EmrFlowCreateSDO();
                        flow.FlowCode = item.FLOW_CODE;
                        flow.FlowName = item.FLOW_NAME;
                        flow.NumOrder = item.NUM_ORDER;
                        flow.RoomCode = item.ROOM_CODE;
                        flow.RoomName = item.ROOM_NAME;
                        flow.RoomTypeCode = item.ROOM_TYPE_CODE;
                        var signers = lstEmrBusiness.Where(o => o.PARENT_ID__IN_SETY == item.CONCRETE_ID__IN_SETY).ToList();
                        if (signers != null && signers.Count > 0)
                        {
                            flow.SignerIds = signers.Select(o => o.SIGNER_ID).ToList();
                        }
                        lstFlow.Add(flow);
                    }
                    sdo.EmrFlows = lstFlow;
                }
                EMR_BUSINESS result = GlobalStore.EmrConsumer.Post<EMR_BUSINESS>("api/EmrBusiness/CreateBySdo", param, sdo);
                WaitingManager.Hide();
                if (result != null)
                {
                    actChoose(result);
                    this.Close();
                }
                MessageManager.Show(this, param, result != null);
            }
            catch (Exception ex)
            {
                WaitingManager.Hide();
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void cboLoginName_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                if (cboLoginName.EditValue != null)
                {
                    var singer = (cboLoginName.Properties.DataSource as List<EMR_SIGNER>).FirstOrDefault(o => o.ID == Int64.Parse(cboLoginName.EditValue.ToString()));
                    txtLoginName.Text = singer.LOGINNAME;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
