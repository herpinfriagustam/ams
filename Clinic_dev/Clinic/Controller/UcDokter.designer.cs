namespace Clinic.ControllerAntrian
{
    partial class UcDokter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNamaDokter = new DevExpress.XtraEditors.LabelControl();
            this.imgDokter = new DevExpress.XtraEditors.PictureEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.imgDokter.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNamaDokter
            // 
            this.lblNamaDokter.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(127)))), ((int)(((byte)(187)))));
            this.lblNamaDokter.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNamaDokter.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblNamaDokter.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblNamaDokter.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblNamaDokter.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.lblNamaDokter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblNamaDokter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNamaDokter.Location = new System.Drawing.Point(5, 233);
            this.lblNamaDokter.Name = "lblNamaDokter";
            this.lblNamaDokter.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblNamaDokter.Size = new System.Drawing.Size(303, 59);
            this.lblNamaDokter.TabIndex = 3;
            this.lblNamaDokter.Text = "-";
            // 
            // imgDokter
            // 
            this.imgDokter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgDokter.Location = new System.Drawing.Point(30, 20);
            this.imgDokter.Name = "imgDokter";
            this.imgDokter.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.imgDokter.Properties.Appearance.Options.UseBackColor = true;
            this.imgDokter.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.imgDokter.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.imgDokter.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.imgDokter.Size = new System.Drawing.Size(243, 188);
            this.imgDokter.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.imgDokter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.panel1.Size = new System.Drawing.Size(303, 228);
            this.panel1.TabIndex = 5;
            // 
            // UcDokter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblNamaDokter);
            this.Name = "UcDokter";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(313, 297);
            ((System.ComponentModel.ISupportInitialize)(this.imgDokter.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblNamaDokter;
        private DevExpress.XtraEditors.PictureEdit imgDokter;
        private System.Windows.Forms.Panel panel1;
    }
}
