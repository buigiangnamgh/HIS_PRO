using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignToolViewer.Integrate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.Integrate
{
    internal partial class frmLogin : Form
    {
        Action<Inventec.Common.SignLibrary.ADO.SignToken> actSignToken;
        Inventec.Common.SignLibrary.ADO.SignToken signToken;
        internal frmLogin(Action<Inventec.Common.SignLibrary.ADO.SignToken> _actSignToken)
        {
            InitializeComponent();

            this.actSignToken = _actSignToken;           
            if (GlobalStore.IsUseSendDTI)
            {
                lblFortxtEmrUri.Visible = false;
                lblFortxtAcsUri.Visible = false;
                lblFortxtFssUri.Visible = false;
                txtEmrUri.Visible = false;
                txtAcsUri.Visible = false;
                txtFssUri.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtLoginName.Text))
            {
                MessageBox.Show("Chưa nhập trường tên đăng nhập");
                return;
            }
            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Chưa nhập trường mật khẩu");
                return;
            }
            if (String.IsNullOrEmpty(txtEmrUri.Text))
            {
                MessageBox.Show("Chưa nhập trường địa chỉ hệ thống EMR");
                return;
            }
            if (String.IsNullOrEmpty(txtAcsUri.Text))
            {
                MessageBox.Show("Chưa nhập trường địa chỉ hệ thống xác thực (ACS)");
                return;
            }

            if (String.IsNullOrEmpty(txtFssUri.Text))
            {
                MessageBox.Show("Chưa nhập trường địa chỉ hệ thống QL file tập trung (FSS)");
                return;
            }

            ConstanIG.ACS_BASE_URI = txtAcsUri.Text;
            GlobalStore.EMR_BASE_URI = txtEmrUri.Text;
            FssConstant.BASE_URI = txtFssUri.Text;

            //Save to registry
            RegistryProcessor.Write("ACS_BASE_URI", ConstanIG.ACS_BASE_URI);
            RegistryProcessor.Write("EMR_BASE_URI", GlobalStore.EMR_BASE_URI);
            RegistryProcessor.Write("FSS_BASE_URI", FssConstant.BASE_URI);

            Login(txtLoginName.Text, txtPassword.Text);
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ConstanIG.ACS_BASE_URI))
            {
                if (String.IsNullOrEmpty((string)RegistryProcessor.Read("ACS_BASE_URI")))
                {
                    RegistryProcessor.Write("ACS_BASE_URI", ConstanIG.ACS_BASE_URI);
                }
            }
            else
            {
                ConstanIG.ACS_BASE_URI = (string)RegistryProcessor.Read("ACS_BASE_URI");
            }

            if (!String.IsNullOrEmpty(GlobalStore.EMR_BASE_URI))
            {
                if (String.IsNullOrEmpty((string)RegistryProcessor.Read("EMR_BASE_URI")))
                {
                    RegistryProcessor.Write("EMR_BASE_URI", GlobalStore.EMR_BASE_URI);
                }
            }
            else
            {
                GlobalStore.EMR_BASE_URI = (string)RegistryProcessor.Read("EMR_BASE_URI");
            }

            if (!String.IsNullOrEmpty(FssConstant.BASE_URI))
            {
                if (String.IsNullOrEmpty((string)RegistryProcessor.Read("FSS_BASE_URI")))
                {
                    RegistryProcessor.Write("FSS_BASE_URI", FssConstant.BASE_URI);
                }
            }
            else
            {
                FssConstant.BASE_URI = (string)RegistryProcessor.Read("FSS_BASE_URI");
            }

            if (!String.IsNullOrEmpty(ConstanIG.ACS_BASE_URI))
                txtAcsUri.Text = ConstanIG.ACS_BASE_URI;

            if (!String.IsNullOrEmpty(GlobalStore.EMR_BASE_URI))
                txtEmrUri.Text = GlobalStore.EMR_BASE_URI;

            if (!String.IsNullOrEmpty(FssConstant.BASE_URI))
                txtFssUri.Text = FssConstant.BASE_URI;

            if (!String.IsNullOrEmpty(ConstanIG.ACS_BASE_URI) && !String.IsNullOrEmpty(GlobalStore.EMR_BASE_URI))
            {            
                CommonParam param = new CommonParam();
                Inventec.Common.Integrate.ClientTokenManager clientTokenManager = new Inventec.Common.Integrate.ClientTokenManager(GlobalStore.appCode, ConstanIG.ACS_BASE_URI);
                clientTokenManager.UseRegistry(true);

                signToken = new SignLibrary.ADO.SignToken();

                signToken.TokenData = clientTokenManager.Init(param);
                if (signToken.TokenData == null && !String.IsNullOrEmpty(signToken.LoginName) && !String.IsNullOrEmpty(signToken.Password))
                {                   
                    Login(signToken.LoginName, signToken.Password);
                }
                else if (signToken.TokenData != null)
                {
                    GlobalStore.AcsConsumer.SetTokenCode(signToken.TokenData.TokenCode);
                    GlobalStore.EmrConsumer.SetTokenCode(signToken.TokenData.TokenCode);
                    signToken.LoginName = signToken.TokenData.User.LoginName;
                    signToken.UserName = signToken.TokenData.User.UserName;
                    signToken.TokenCode = signToken.TokenData.TokenCode;
                    if (actSignToken != null)
                        actSignToken(signToken);
                    this.Close();
                }
            }
        }

        public bool Login(string loginName, string password)
        {
            bool result = false;
            try
            {
                CommonParam param = new CommonParam();
                Inventec.Common.Integrate.ClientTokenManager clientTokenManager = new Inventec.Common.Integrate.ClientTokenManager(GlobalStore.appCode, ConstanIG.ACS_BASE_URI);
                clientTokenManager.UseRegistry(true);
                signToken = new SignLibrary.ADO.SignToken();
                signToken.TokenData = clientTokenManager.Login(param, loginName, password);

                if (signToken.TokenData != null && signToken.TokenData.User != null)
                {
                    signToken.LoginName = signToken.TokenData.User.LoginName;
                    signToken.UserName = signToken.TokenData.User.UserName;
                    signToken.TokenCode = signToken.TokenData.TokenCode;
                    GlobalStore.AcsConsumer.SetTokenCode(signToken.TokenData.TokenCode);
                    GlobalStore.EmrConsumer.SetTokenCode(signToken.TokenData.TokenCode);
                    if (actSignToken != null)
                        actSignToken(signToken);
                    result = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lưu cấu hình & truy cập vào hệ thống EMR thất bại");
                    Inventec.Common.Logging.LogSystem.Info("Tai khoan hoac mat khau truy cap vao he thong EMR khong chinh xac____" + Inventec.Common.Logging.LogUtil.TraceData("loginName", loginName) + "____" + Inventec.Common.Logging.LogUtil.TraceData("appCode", GlobalStore.appCode) + "____" + Inventec.Common.Logging.LogUtil.TraceData("ACS_BASE_URI", ConstanIG.ACS_BASE_URI));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                result = false;
            }
            return result;
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(null, null);
            }
        }
    }
}
