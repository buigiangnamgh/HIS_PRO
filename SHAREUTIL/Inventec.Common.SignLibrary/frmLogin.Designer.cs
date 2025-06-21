namespace Inventec.Common.Integrate
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLoginName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEmrUri = new System.Windows.Forms.TextBox();
            this.lblFortxtEmrUri = new System.Windows.Forms.Label();
            this.txtAcsUri = new System.Windows.Forms.TextBox();
            this.lblFortxtAcsUri = new System.Windows.Forms.Label();
            this.txtFssUri = new System.Windows.Forms.TextBox();
            this.lblFortxtFssUri = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtLoginName
            // 
            this.txtLoginName.Location = new System.Drawing.Point(180, 17);
            this.txtLoginName.Name = "txtLoginName";
            this.txtLoginName.Size = new System.Drawing.Size(196, 20);
            this.txtLoginName.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(180, 44);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(196, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tên đăng nhập:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(301, 149);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mật khẩu:";
            // 
            // txtEmrUri
            // 
            this.txtEmrUri.Location = new System.Drawing.Point(180, 70);
            this.txtEmrUri.Name = "txtEmrUri";
            this.txtEmrUri.Size = new System.Drawing.Size(196, 20);
            this.txtEmrUri.TabIndex = 3;
            // 
            // lblFortxtEmrUri
            // 
            this.lblFortxtEmrUri.AutoSize = true;
            this.lblFortxtEmrUri.Location = new System.Drawing.Point(13, 73);
            this.lblFortxtEmrUri.Name = "lblFortxtEmrUri";
            this.lblFortxtEmrUri.Size = new System.Drawing.Size(160, 13);
            this.lblFortxtEmrUri.TabIndex = 1;
            this.lblFortxtEmrUri.Text = "Địa chỉ hệ thống backend EMR:";
            // 
            // txtAcsUri
            // 
            this.txtAcsUri.Location = new System.Drawing.Point(180, 96);
            this.txtAcsUri.Name = "txtAcsUri";
            this.txtAcsUri.Size = new System.Drawing.Size(196, 20);
            this.txtAcsUri.TabIndex = 4;
            // 
            // lblFortxtAcsUri
            // 
            this.lblFortxtAcsUri.AutoSize = true;
            this.lblFortxtAcsUri.Location = new System.Drawing.Point(11, 99);
            this.lblFortxtAcsUri.Name = "lblFortxtAcsUri";
            this.lblFortxtAcsUri.Size = new System.Drawing.Size(162, 13);
            this.lblFortxtAcsUri.TabIndex = 1;
            this.lblFortxtAcsUri.Text = "Địa chỉ hệ thống xác thực (ACS):";
            // 
            // txtFssUri
            // 
            this.txtFssUri.Location = new System.Drawing.Point(180, 122);
            this.txtFssUri.Name = "txtFssUri";
            this.txtFssUri.Size = new System.Drawing.Size(196, 20);
            this.txtFssUri.TabIndex = 4;
            // 
            // lblFortxtFssUri
            // 
            this.lblFortxtFssUri.AutoSize = true;
            this.lblFortxtFssUri.Location = new System.Drawing.Point(20, 125);
            this.lblFortxtFssUri.Name = "lblFortxtFssUri";
            this.lblFortxtFssUri.Size = new System.Drawing.Size(153, 13);
            this.lblFortxtFssUri.TabIndex = 1;
            this.lblFortxtFssUri.Text = "Địa chỉ hệ thống QL File (FSS):";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 180);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFortxtFssUri);
            this.Controls.Add(this.lblFortxtAcsUri);
            this.Controls.Add(this.lblFortxtEmrUri);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtFssUri);
            this.Controls.Add(this.txtAcsUri);
            this.Controls.Add(this.txtEmrUri);
            this.Controls.Add(this.txtLoginName);
            this.Name = "frmLogin";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tham số cấu hình";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLoginName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEmrUri;
        private System.Windows.Forms.Label lblFortxtEmrUri;
        private System.Windows.Forms.TextBox txtAcsUri;
        private System.Windows.Forms.Label lblFortxtAcsUri;
        private System.Windows.Forms.TextBox txtFssUri;
        private System.Windows.Forms.Label lblFortxtFssUri;
    }
}