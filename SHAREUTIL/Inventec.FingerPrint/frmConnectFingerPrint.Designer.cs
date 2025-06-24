namespace Inventec.FingerPrint
{
    partial class frmConnectFingerPrint
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnectFingerPrint));
            this.bnInit = new System.Windows.Forms.Button();
            this.bnOpen = new System.Windows.Forms.Button();
            this.bnEnroll = new System.Windows.Forms.Button();
            this.bnVerify = new System.Windows.Forms.Button();
            this.bnFree = new System.Windows.Forms.Button();
            this.bnClose = new System.Windows.Forms.Button();
            this.bnIdentify = new System.Windows.Forms.Button();
            this.textRes = new System.Windows.Forms.TextBox();
            this.picFPImg = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbIdx = new System.Windows.Forms.ComboBox();
            this.bnChoose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picFPImg)).BeginInit();
            this.SuspendLayout();
            // 
            // bnInit
            // 
            this.bnInit.Location = new System.Drawing.Point(36, 1);
            this.bnInit.Margin = new System.Windows.Forms.Padding(4);
            this.bnInit.Name = "bnInit";
            this.bnInit.Size = new System.Drawing.Size(100, 31);
            this.bnInit.TabIndex = 0;
            this.bnInit.Text = "Initialize";
            this.bnInit.UseVisualStyleBackColor = true;
            this.bnInit.Visible = false;
            this.bnInit.Click += new System.EventHandler(this.bnInit_Click);
            // 
            // bnOpen
            // 
            this.bnOpen.Enabled = false;
            this.bnOpen.Location = new System.Drawing.Point(36, 1);
            this.bnOpen.Margin = new System.Windows.Forms.Padding(4);
            this.bnOpen.Name = "bnOpen";
            this.bnOpen.Size = new System.Drawing.Size(100, 31);
            this.bnOpen.TabIndex = 1;
            this.bnOpen.Text = "Open";
            this.bnOpen.UseVisualStyleBackColor = true;
            this.bnOpen.Visible = false;
            this.bnOpen.Click += new System.EventHandler(this.bnOpen_Click);
            // 
            // bnEnroll
            // 
            this.bnEnroll.Enabled = false;
            this.bnEnroll.Location = new System.Drawing.Point(36, 0);
            this.bnEnroll.Margin = new System.Windows.Forms.Padding(4);
            this.bnEnroll.Name = "bnEnroll";
            this.bnEnroll.Size = new System.Drawing.Size(100, 31);
            this.bnEnroll.TabIndex = 2;
            this.bnEnroll.Text = "Enroll";
            this.bnEnroll.UseVisualStyleBackColor = true;
            this.bnEnroll.Visible = false;
            this.bnEnroll.Click += new System.EventHandler(this.bnEnroll_Click);
            // 
            // bnVerify
            // 
            this.bnVerify.Enabled = false;
            this.bnVerify.Location = new System.Drawing.Point(36, 0);
            this.bnVerify.Margin = new System.Windows.Forms.Padding(4);
            this.bnVerify.Name = "bnVerify";
            this.bnVerify.Size = new System.Drawing.Size(100, 31);
            this.bnVerify.TabIndex = 3;
            this.bnVerify.Text = "Verify";
            this.bnVerify.UseVisualStyleBackColor = true;
            this.bnVerify.Visible = false;
            this.bnVerify.Click += new System.EventHandler(this.bnVerify_Click);
            // 
            // bnFree
            // 
            this.bnFree.Enabled = false;
            this.bnFree.Location = new System.Drawing.Point(36, 1);
            this.bnFree.Margin = new System.Windows.Forms.Padding(4);
            this.bnFree.Name = "bnFree";
            this.bnFree.Size = new System.Drawing.Size(100, 31);
            this.bnFree.TabIndex = 4;
            this.bnFree.Text = "Finalize";
            this.bnFree.UseVisualStyleBackColor = true;
            this.bnFree.Visible = false;
            this.bnFree.Click += new System.EventHandler(this.bnFree_Click);
            // 
            // bnClose
            // 
            this.bnClose.Enabled = false;
            this.bnClose.Location = new System.Drawing.Point(36, 1);
            this.bnClose.Margin = new System.Windows.Forms.Padding(4);
            this.bnClose.Name = "bnClose";
            this.bnClose.Size = new System.Drawing.Size(100, 31);
            this.bnClose.TabIndex = 5;
            this.bnClose.Text = "Close";
            this.bnClose.UseVisualStyleBackColor = true;
            this.bnClose.Visible = false;
            this.bnClose.Click += new System.EventHandler(this.bnClose_Click);
            // 
            // bnIdentify
            // 
            this.bnIdentify.Enabled = false;
            this.bnIdentify.Location = new System.Drawing.Point(36, 0);
            this.bnIdentify.Margin = new System.Windows.Forms.Padding(4);
            this.bnIdentify.Name = "bnIdentify";
            this.bnIdentify.Size = new System.Drawing.Size(100, 31);
            this.bnIdentify.TabIndex = 6;
            this.bnIdentify.Text = "Identiy";
            this.bnIdentify.UseVisualStyleBackColor = true;
            this.bnIdentify.Visible = false;
            this.bnIdentify.Click += new System.EventHandler(this.bnIdentify_Click);
            // 
            // textRes
            // 
            this.textRes.Location = new System.Drawing.Point(1, 424);
            this.textRes.Margin = new System.Windows.Forms.Padding(4);
            this.textRes.Multiline = true;
            this.textRes.Name = "textRes";
            this.textRes.ReadOnly = true;
            this.textRes.Size = new System.Drawing.Size(451, 42);
            this.textRes.TabIndex = 7;
            // 
            // picFPImg
            // 
            this.picFPImg.Location = new System.Drawing.Point(13, 0);
            this.picFPImg.Margin = new System.Windows.Forms.Padding(4);
            this.picFPImg.Name = "picFPImg";
            this.picFPImg.Size = new System.Drawing.Size(423, 361);
            this.picFPImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFPImg.TabIndex = 8;
            this.picFPImg.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Index:";
            this.label1.Visible = false;
            // 
            // cmbIdx
            // 
            this.cmbIdx.FormattingEnabled = true;
            this.cmbIdx.Location = new System.Drawing.Point(84, 4);
            this.cmbIdx.Margin = new System.Windows.Forms.Padding(4);
            this.cmbIdx.Name = "cmbIdx";
            this.cmbIdx.Size = new System.Drawing.Size(52, 24);
            this.cmbIdx.TabIndex = 10;
            this.cmbIdx.Visible = false;
            // 
            // bnChoose
            // 
            this.bnChoose.Location = new System.Drawing.Point(164, 379);
            this.bnChoose.Margin = new System.Windows.Forms.Padding(4);
            this.bnChoose.Name = "bnChoose";
            this.bnChoose.Size = new System.Drawing.Size(100, 37);
            this.bnChoose.TabIndex = 12;
            this.bnChoose.Text = "Chọn";
            this.bnChoose.UseVisualStyleBackColor = true;
            this.bnChoose.Click += new System.EventHandler(this.bnChoose_Click);
            // 
            // frmConnectFingerPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 468);
            this.Controls.Add(this.bnChoose);
            this.Controls.Add(this.cmbIdx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picFPImg);
            this.Controls.Add(this.textRes);
            this.Controls.Add(this.bnIdentify);
            this.Controls.Add(this.bnClose);
            this.Controls.Add(this.bnFree);
            this.Controls.Add(this.bnVerify);
            this.Controls.Add(this.bnEnroll);
            this.Controls.Add(this.bnOpen);
            this.Controls.Add(this.bnInit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmConnectFingerPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ký bằng vân tay";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picFPImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bnInit;
        private System.Windows.Forms.Button bnOpen;
        private System.Windows.Forms.Button bnEnroll;
        private System.Windows.Forms.Button bnVerify;
        private System.Windows.Forms.Button bnFree;
        private System.Windows.Forms.Button bnClose;
        private System.Windows.Forms.Button bnIdentify;
        private System.Windows.Forms.TextBox textRes;
        private System.Windows.Forms.PictureBox picFPImg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbIdx;
        private System.Windows.Forms.Button bnChoose;
    }
}

