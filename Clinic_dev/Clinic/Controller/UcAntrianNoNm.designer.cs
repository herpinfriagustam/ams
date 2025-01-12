namespace Clinic.ControllerAntrian
{
    partial class UcAntrianNoNm
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
            this.lblPoli = new DevExpress.XtraEditors.LabelControl();
            this.lblLine = new DevExpress.XtraEditors.LabelControl();
            this.lblPasien = new DevExpress.XtraEditors.LabelControl();
            this.lblAntrian = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // lblPoli
            // 
            this.lblPoli.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(127)))), ((int)(((byte)(187)))));
            this.lblPoli.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoli.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblPoli.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblPoli.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblPoli.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblPoli.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPoli.Location = new System.Drawing.Point(5, 5);
            this.lblPoli.Name = "lblPoli";
            this.lblPoli.Size = new System.Drawing.Size(291, 55);
            this.lblPoli.TabIndex = 0;
            this.lblPoli.Text = "Nomor Antrian";
            // 
            // lblLine
            // 
            this.lblLine.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLine.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblLine.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblLine.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLine.Location = new System.Drawing.Point(5, 60);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(291, 0);
            this.lblLine.TabIndex = 1;
            this.lblLine.Visible = false;
            // 
            // lblPasien
            // 
            this.lblPasien.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(222)))), ((int)(((byte)(115)))));
            this.lblPasien.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasien.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(0)))), ((int)(((byte)(151)))));
            this.lblPasien.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblPasien.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblPasien.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblPasien.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblPasien.Location = new System.Drawing.Point(5, 148);
            this.lblPasien.Name = "lblPasien";
            this.lblPasien.Size = new System.Drawing.Size(291, 51);
            this.lblPasien.TabIndex = 2;
            this.lblPasien.Text = "Ayu Yunengsih";
            // 
            // lblAntrian
            // 
            this.lblAntrian.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(222)))), ((int)(((byte)(115)))));
            this.lblAntrian.Appearance.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAntrian.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(0)))), ((int)(((byte)(151)))));
            this.lblAntrian.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblAntrian.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblAntrian.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblAntrian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAntrian.Location = new System.Drawing.Point(5, 60);
            this.lblAntrian.Name = "lblAntrian";
            this.lblAntrian.Size = new System.Drawing.Size(291, 88);
            this.lblAntrian.TabIndex = 3;
            this.lblAntrian.Text = "G002";
            // 
            // UcAntrianNoNm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAntrian);
            this.Controls.Add(this.lblPasien);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.lblPoli);
            this.Name = "UcAntrianNoNm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(301, 204);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblPoli;
        private DevExpress.XtraEditors.LabelControl lblLine;
        private DevExpress.XtraEditors.LabelControl lblPasien;
        private DevExpress.XtraEditors.LabelControl lblAntrian;
    }
}
