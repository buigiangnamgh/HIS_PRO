using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.Plugins.Library.ElectronicBill.Data;
using Inventec.Common.Logging;
using Inventec.Desktop.Common.Message;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace HIS.Desktop.Plugins.Library.ElectronicBill.SignInvoice
{
	public class FormSignInvoice : Form
	{
		private SignInitData InitData;

		private string TempFileName;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private SimpleButton btnSignAndRelease;

		private LayoutControlGroup layoutControlGroup1;

        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;

		private TextEdit txtEmail;

		private TextEdit txtName;

		private EmptySpaceItem emptySpaceItem1;

        private DevExpress.XtraLayout.LayoutControlItem lciName;

        private DevExpress.XtraLayout.LayoutControlItem lciEmail;

		private RadPdfViewer pdfView;

        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;

		public FormSignInvoice(SignInitData initData)
		{
			InitializeComponent();
			InitData = initData;
			SetIcon();
		}

		private void SetIcon()
		{
			try
			{
				base.Icon = Icon.ExtractAssociatedIcon(Path.Combine(ApplicationStoreLocation.ApplicationDirectory, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]));
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void FormSignInvoice_Load(object sender, EventArgs e)
		{
			try
			{
				if (InitData != null)
				{
					if (InitData.fileToBytes != null)
					{
						TempFileName = Path.GetTempFileName();
						TempFileName = TempFileName.Replace("tmp", "pdf");
						try
						{
							File.WriteAllBytes(TempFileName, InitData.fileToBytes);
						}
						catch (Exception ex)
						{
							LogSystem.Error(ex);
						}
						pdfView.LoadDocument(TempFileName);
					}
					else if (!string.IsNullOrWhiteSpace(InitData.FileDownload))
					{
						WaitingManager.Show();
						WebClient webClient = new WebClient();
						ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
						byte[] buffer = webClient.DownloadData(InitData.FileDownload);
						Stream stream = new MemoryStream(buffer);
						WaitingManager.Hide();
						pdfView.LoadDocument(stream);
					}
					if (!string.IsNullOrWhiteSpace(InitData.Email) && !string.IsNullOrWhiteSpace(InitData.Name))
					{
						txtEmail.Text = InitData.Email;
						txtName.Text = InitData.Name;
					}
				}
				txtName.Focus();
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		private void btnSignAndRelease_Click(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(txtName.Text) && string.IsNullOrWhiteSpace(txtEmail.Text))
				{
					XtraMessageBox.Show("Cần bổ sung thông tin email của người nhận khi có thông tin tên");
					txtEmail.Focus();
					return;
				}
				if (string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtEmail.Text))
				{
					XtraMessageBox.Show("Cần bổ sung thông tin tên của người nhận khi có thông tin email");
					txtName.Focus();
					return;
				}
				SignDelegate signDelegate = new SignDelegate();
				signDelegate.Email = txtEmail.Text.Trim();
				signDelegate.Name = txtName.Text.Trim();
				string errorMessage = "";
				if (InitData != null && InitData.SignAndRelease != null && InitData.SignAndRelease(signDelegate, ref errorMessage))
				{
					Close();
				}
				else if (!string.IsNullOrWhiteSpace(errorMessage))
				{
					XtraMessageBox.Show(errorMessage);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void DeleteTempFile()
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(TempFileName) && File.Exists(TempFileName))
				{
					File.Delete(TempFileName);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			DeleteTempFile();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.pdfView = new RadPdfViewer();
			this.txtEmail = new DevExpress.XtraEditors.TextEdit();
			this.txtName = new DevExpress.XtraEditors.TextEdit();
			this.btnSignAndRelease = new DevExpress.XtraEditors.SimpleButton();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.lciName = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciEmail = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.pdfView).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtEmail.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtName.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciName).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciEmail).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add((System.Windows.Forms.Control)(object)this.pdfView);
			this.layoutControl1.Controls.Add(this.txtEmail);
			this.layoutControl1.Controls.Add(this.txtName);
			this.layoutControl1.Controls.Add(this.btnSignAndRelease);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(880, 561);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			((System.Windows.Forms.Control)(object)this.pdfView).BackColor = System.Drawing.SystemColors.ControlLightLight;
			((System.Windows.Forms.Control)(object)this.pdfView).Location = new System.Drawing.Point(2, 28);
			((System.Windows.Forms.Control)(object)this.pdfView).Name = "pdfView";
			((RadControl)this.pdfView).RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 200, 200);
			((System.Windows.Forms.Control)(object)this.pdfView).Size = new System.Drawing.Size(876, 531);
			((System.Windows.Forms.Control)(object)this.pdfView).TabIndex = 8;
			this.pdfView.ThumbnailsScaleFactor = 0.15f;
			this.txtEmail.EnterMoveNextControl = true;
			this.txtEmail.Location = new System.Drawing.Point(317, 2);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(341, 20);
			this.txtEmail.StyleController = this.layoutControl1;
			this.txtEmail.TabIndex = 7;
			this.txtName.EnterMoveNextControl = true;
			this.txtName.Location = new System.Drawing.Point(97, 2);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(121, 20);
			this.txtName.StyleController = this.layoutControl1;
			this.txtName.TabIndex = 6;
			this.btnSignAndRelease.Location = new System.Drawing.Point(772, 2);
			this.btnSignAndRelease.Name = "btnSignAndRelease";
			this.btnSignAndRelease.Size = new System.Drawing.Size(106, 22);
			this.btnSignAndRelease.StyleController = this.layoutControl1;
			this.btnSignAndRelease.TabIndex = 4;
			this.btnSignAndRelease.Text = "Ký và phát hành";
			this.btnSignAndRelease.Click += new System.EventHandler(btnSignAndRelease_Click);
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[5] { this.layoutControlItem1, this.emptySpaceItem1, this.lciName, this.lciEmail, this.layoutControlItem2 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(880, 561);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.Control = this.btnSignAndRelease;
			this.layoutControlItem1.Location = new System.Drawing.Point(770, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(110, 26);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(660, 0);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(110, 26);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			this.lciName.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciName.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciName.Control = this.txtName;
			this.lciName.Location = new System.Drawing.Point(0, 0);
			this.lciName.Name = "lciName";
			this.lciName.Size = new System.Drawing.Size(220, 26);
			this.lciName.Text = "Tên người nhận:";
			this.lciName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciName.TextSize = new System.Drawing.Size(90, 20);
			this.lciName.TextToControlDistance = 5;
			this.lciEmail.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciEmail.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciEmail.Control = this.txtEmail;
			this.lciEmail.Location = new System.Drawing.Point(220, 0);
			this.lciEmail.Name = "lciEmail";
			this.lciEmail.Size = new System.Drawing.Size(440, 26);
			this.lciEmail.Text = "Email người nhận:";
			this.lciEmail.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciEmail.TextSize = new System.Drawing.Size(90, 20);
			this.lciEmail.TextToControlDistance = 5;
			this.layoutControlItem2.Control = (System.Windows.Forms.Control)(object)this.pdfView;
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 26);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(880, 535);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(880, 561);
			base.Controls.Add(this.layoutControl1);
			base.Name = "FormSignInvoice";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Chi tiết hóa đơn";
			base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			base.Load += new System.EventHandler(FormSignInvoice_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.pdfView).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtEmail.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtName.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciName).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciEmail).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			base.ResumeLayout(false);
		}
	}
}
