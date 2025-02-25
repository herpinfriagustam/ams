namespace RfidClinic
{
    partial class Warning
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lEstimasi = new DevExpress.XtraEditors.LabelControl();
            this.lAntrian = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.lEstimasi);
            this.panel1.Controls.Add(this.lAntrian);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(943, 563);
            this.panel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Malgun Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Appearance.Options.UseForeColor = true;
            this.btnCancel.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnCancel.Location = new System.Drawing.Point(544, 422);
            this.btnCancel.LookAndFeel.SkinMaskColor = System.Drawing.Color.Red;
            this.btnCancel.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(252, 52);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "TIDAK";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Appearance.Font = new System.Drawing.Font("Malgun Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Appearance.Options.UseForeColor = true;
            this.btnOk.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnOk.Location = new System.Drawing.Point(142, 422);
            this.btnOk.LookAndFeel.SkinMaskColor = System.Drawing.Color.DodgerBlue;
            this.btnOk.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.btnOk.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(252, 52);
            this.btnOk.TabIndex = 17;
            this.btnOk.Text = "YA";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Location = new System.Drawing.Point(117, 314);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(692, 60);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "Apakah anda ingin tetap mendaftar?";
            // 
            // lEstimasi
            // 
            this.lEstimasi.Appearance.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEstimasi.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lEstimasi.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lEstimasi.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lEstimasi.Location = new System.Drawing.Point(117, 242);
            this.lEstimasi.Name = "lEstimasi";
            this.lEstimasi.Size = new System.Drawing.Size(692, 60);
            this.lEstimasi.TabIndex = 15;
            this.lEstimasi.Text = "40 Menit";
            // 
            // lAntrian
            // 
            this.lAntrian.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAntrian.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lAntrian.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lAntrian.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.lAntrian.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lAntrian.Location = new System.Drawing.Point(117, 144);
            this.lAntrian.Name = "lAntrian";
            this.lAntrian.Size = new System.Drawing.Size(692, 87);
            this.lAntrian.TabIndex = 14;
            this.lAntrian.Text = "Anda akan menunggu 10 antrian, \r\ndengan estimasi waktu tunggu -+";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(117, 54);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(692, 60);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "PERHATIAN !!!";
            // 
            // Warning
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 563);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Warning";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Warning";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Warning_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl lEstimasi;
        private DevExpress.XtraEditors.LabelControl lAntrian;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
    }
}