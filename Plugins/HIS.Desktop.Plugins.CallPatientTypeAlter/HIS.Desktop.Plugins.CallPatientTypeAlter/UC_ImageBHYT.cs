using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.Common;
using HIS.Desktop.ModuleExt;
using HIS.Desktop.Plugins.CallPatientTypeAlter.Resources;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Desktop.Common.LanguageManager;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	public class UC_ImageBHYT : UserControl
	{
		private int demBHYT;

		private IContainer components;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private LayoutControlItem layoutControlItem1;

		private LayoutControlItem layoutControlItem2;

		internal SimpleButton btnCamera;

		internal PictureEdit pictureEditImageBHYT;

		public UC_ImageBHYT()
		{
			InitializeComponent();
			SetCaptionByLanguageKey();
		}

		private void btnCamera_Click(object sender, EventArgs e)
		{
			try
			{
				CallModuleCamera();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetCaptionByLanguageKey()
		{
			try
			{
				ResourceLanguageManager.LanguageResource = new ResourceManager("HIS.Desktop.Plugins.CallPatientTypeAlter.Resources.Lang", typeof(UC_ImageBHYT).Assembly);
				layoutControl1.Text = Get.Value("UC_ImageBHYT.layoutControl1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnCamera.Text = Get.Value("UC_ImageBHYT.btnCamera.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CallModuleCamera()
		{
			try
			{
				List<object> list = new List<object>();
				list.Add(new DelegateSelectData(FillImageFromModuleCamereToUC));
				PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.Camera", 0L, 0L, list);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void SetImageDefaultForPictureEdit(Image imageData)
		{
			try
			{
				if (imageData != null)
				{
					pictureEditImageBHYT.Image = imageData;
					pictureEditImageBHYT.Properties.SizeMode = PictureSizeMode.Stretch;
					pictureEditImageBHYT.Tag = "Image";
				}
				else
				{
					ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UC_ImageBHYT));
					pictureEditImageBHYT.EditValue = componentResourceManager.GetObject("pictureEditImageBHYT.EditValue");
					pictureEditImageBHYT.Tag = "NoImage";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void FillImageFromModuleCamereToUC(object dataImage)
		{
			try
			{
				if (dataImage != null)
				{
					pictureEditImageBHYT.Image = (Image)dataImage;
					pictureEditImageBHYT.Tag = ((Image)dataImage).Tag;
					pictureEditImageBHYT.Properties.SizeMode = PictureSizeMode.Stretch;
					pictureEditImageBHYT.Tag = "Image";
				}
				else
				{
					ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UC_ImageBHYT));
					pictureEditImageBHYT.EditValue = componentResourceManager.GetObject("pictureEditImageBHYT.EditValue");
					pictureEditImageBHYT.Tag = "NoImage";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void pictureEditImageBHYT_ImageChanged(object sender, EventArgs e)
		{
			try
			{
				demBHYT++;
				if (demBHYT != 0 && pictureEditImageBHYT.Image != null)
				{
					pictureEditImageBHYT.Tag = "Image";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void DisposeControl()
		{
			try
			{
				demBHYT = 0;
				btnCamera.Click -= new EventHandler(btnCamera_Click);
				pictureEditImageBHYT.ImageChanged -= new EventHandler(pictureEditImageBHYT_ImageChanged);
				pictureEditImageBHYT = null;
				btnCamera = null;
				layoutControlItem2 = null;
				layoutControlItem1 = null;
				layoutControlGroup1 = null;
				layoutControl1 = null;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIS.Desktop.Plugins.CallPatientTypeAlter.UC_ImageBHYT));
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.btnCamera = new DevExpress.XtraEditors.SimpleButton();
			this.pictureEditImageBHYT = new DevExpress.XtraEditors.PictureEdit();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.pictureEditImageBHYT.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.btnCamera);
			this.layoutControl1.Controls.Add(this.pictureEditImageBHYT);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(268, 246);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.btnCamera.Location = new System.Drawing.Point(2, 222);
			this.btnCamera.Name = "btnCamera";
			this.btnCamera.Size = new System.Drawing.Size(264, 22);
			this.btnCamera.StyleController = this.layoutControl1;
			this.btnCamera.TabIndex = 5;
			this.btnCamera.Text = "Chụp thẻ BHYT";
			this.btnCamera.Click += new System.EventHandler(btnCamera_Click);
			this.pictureEditImageBHYT.EditValue = resources.GetObject("pictureEditImageBHYT.EditValue");
			this.pictureEditImageBHYT.Location = new System.Drawing.Point(2, 2);
			this.pictureEditImageBHYT.Name = "pictureEditImageBHYT";
			this.pictureEditImageBHYT.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
			this.pictureEditImageBHYT.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
			this.pictureEditImageBHYT.Size = new System.Drawing.Size(264, 216);
			this.pictureEditImageBHYT.StyleController = this.layoutControl1;
			this.pictureEditImageBHYT.TabIndex = 4;
			this.pictureEditImageBHYT.ImageChanged += new System.EventHandler(pictureEditImageBHYT_ImageChanged);
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[2] { this.layoutControlItem1, this.layoutControlItem2 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(268, 246);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.Control = this.pictureEditImageBHYT;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(268, 220);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			this.layoutControlItem2.Control = this.btnCamera;
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 220);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(268, 26);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.layoutControl1);
			base.Name = "UC_ImageBHYT";
			base.Size = new System.Drawing.Size(268, 246);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.pictureEditImageBHYT.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			base.ResumeLayout(false);
		}
	}
}
