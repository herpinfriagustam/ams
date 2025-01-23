namespace RfidClinic
{
    partial class Reservation
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reservation));
            this.loading = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::RfidClinic.WaitForm1), true, true);
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            this.timerEnd = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.gradientPanel2 = new RfidClinic.GradientPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lbl_noantrian = new DevExpress.XtraEditors.LabelControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textScanOut = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lName = new DevExpress.XtraEditors.LabelControl();
            this.lEmpid = new DevExpress.XtraEditors.LabelControl();
            this.textRecvTemp = new DevExpress.XtraEditors.TextEdit();
            this.lRfid = new DevExpress.XtraEditors.LabelControl();
            this.lPurpose = new DevExpress.XtraEditors.LabelControl();
            this.Tujuan = new DevExpress.XtraEditors.LabelControl();
            this.RFID = new DevExpress.XtraEditors.LabelControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lInfo = new DevExpress.XtraEditors.LabelControl();
            this.gradientPanel1 = new RfidClinic.GradientPanel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblPoli = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit3 = new DevExpress.XtraEditors.PictureEdit();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.gradientPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textScanOut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textRecvTemp.Properties)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.panel5.SuspendLayout();
            this.gradientPanel1.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // loading
            // 
            this.loading.ClosingDelay = 500;
            // 
            // timerStart
            // 
            this.timerStart.Interval = 200;
            // 
            // serialPort1
            // 
            this.serialPort1.DataBits = 7;
            this.serialPort1.RtsEnable = true;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.InsertImage(global::RfidClinic.Properties.Resources.dokter, "dokter", typeof(global::RfidClinic.Properties.Resources), 0);
            this.imageCollection1.Images.SetKeyName(0, "dokter");
            // 
            // gradientPanel2
            // 
            this.gradientPanel2.Angle = 45F;
            this.gradientPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            this.gradientPanel2.ButtomColor = System.Drawing.Color.Empty;
            this.gradientPanel2.Controls.Add(this.tableLayoutPanel1);
            this.gradientPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientPanel2.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.gradientPanel2.Name = "gradientPanel2";
            this.gradientPanel2.Size = new System.Drawing.Size(1036, 761);
            this.gradientPanel2.TabIndex = 1;
            this.gradientPanel2.TopColor = System.Drawing.Color.RoyalBlue;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gradientPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1036, 761);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 99F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 180);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1036, 481);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.labelControl3, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lbl_noantrian, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(13, 339);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1020, 139);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControl3.Location = new System.Drawing.Point(3, 3);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(1014, 34);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "No Antrian Anda";
            // 
            // lbl_noantrian
            // 
            this.lbl_noantrian.Appearance.BackColor = System.Drawing.Color.LightSalmon;
            this.lbl_noantrian.Appearance.Font = new System.Drawing.Font("Tahoma", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_noantrian.Appearance.ForeColor = System.Drawing.Color.White;
            this.lbl_noantrian.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lbl_noantrian.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_noantrian.Location = new System.Drawing.Point(3, 43);
            this.lbl_noantrian.Name = "lbl_noantrian";
            this.lbl_noantrian.Size = new System.Drawing.Size(1014, 66);
            this.lbl_noantrian.TabIndex = 3;
            this.lbl_noantrian.Text = "No Antrian";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.richTextBox1);
            this.panel2.Controls.Add(this.textScanOut);
            this.panel2.Controls.Add(this.labelControl2);
            this.panel2.Controls.Add(this.labelControl1);
            this.panel2.Controls.Add(this.lName);
            this.panel2.Controls.Add(this.lEmpid);
            this.panel2.Controls.Add(this.textRecvTemp);
            this.panel2.Controls.Add(this.lRfid);
            this.panel2.Controls.Add(this.lPurpose);
            this.panel2.Controls.Add(this.Tujuan);
            this.panel2.Controls.Add(this.RFID);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 336);
            this.panel2.TabIndex = 1;
            this.panel2.Visible = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(98, 62);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // textScanOut
            // 
            this.textScanOut.Location = new System.Drawing.Point(0, 0);
            this.textScanOut.Name = "textScanOut";
            this.textScanOut.Size = new System.Drawing.Size(61, 20);
            this.textScanOut.TabIndex = 5;
            this.textScanOut.Visible = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl2.Location = new System.Drawing.Point(0, 0);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(79, 33);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Nama";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(130, 33);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Pasien ID";
            // 
            // lName
            // 
            this.lName.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lName.Appearance.ForeColor = System.Drawing.Color.White;
            this.lName.Location = new System.Drawing.Point(0, 0);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(12, 33);
            this.lName.TabIndex = 2;
            this.lName.Text = "-";
            // 
            // lEmpid
            // 
            this.lEmpid.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEmpid.Appearance.ForeColor = System.Drawing.Color.White;
            this.lEmpid.Location = new System.Drawing.Point(0, 0);
            this.lEmpid.Name = "lEmpid";
            this.lEmpid.Size = new System.Drawing.Size(12, 33);
            this.lEmpid.TabIndex = 3;
            this.lEmpid.Text = "-";
            // 
            // textRecvTemp
            // 
            this.textRecvTemp.Location = new System.Drawing.Point(0, 0);
            this.textRecvTemp.Name = "textRecvTemp";
            this.textRecvTemp.Size = new System.Drawing.Size(134, 20);
            this.textRecvTemp.TabIndex = 6;
            this.textRecvTemp.Visible = false;
            // 
            // lRfid
            // 
            this.lRfid.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lRfid.Appearance.ForeColor = System.Drawing.Color.White;
            this.lRfid.Location = new System.Drawing.Point(592, 37);
            this.lRfid.Name = "lRfid";
            this.lRfid.Size = new System.Drawing.Size(12, 33);
            this.lRfid.TabIndex = 5;
            this.lRfid.Text = "-";
            this.lRfid.Visible = false;
            this.lRfid.Click += new System.EventHandler(this.lRfid_Click);
            // 
            // lPurpose
            // 
            this.lPurpose.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPurpose.Appearance.ForeColor = System.Drawing.Color.White;
            this.lPurpose.Location = new System.Drawing.Point(128, 11);
            this.lPurpose.Name = "lPurpose";
            this.lPurpose.Size = new System.Drawing.Size(12, 33);
            this.lPurpose.TabIndex = 4;
            this.lPurpose.Text = "-";
            // 
            // Tujuan
            // 
            this.Tujuan.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tujuan.Appearance.ForeColor = System.Drawing.Color.White;
            this.Tujuan.Location = new System.Drawing.Point(17, 11);
            this.Tujuan.Name = "Tujuan";
            this.Tujuan.Size = new System.Drawing.Size(94, 33);
            this.Tujuan.TabIndex = 2;
            this.Tujuan.Text = "Tujuan";
            // 
            // RFID
            // 
            this.RFID.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RFID.Appearance.ForeColor = System.Drawing.Color.White;
            this.RFID.Location = new System.Drawing.Point(481, 37);
            this.RFID.Name = "RFID";
            this.RFID.Size = new System.Drawing.Size(69, 33);
            this.RFID.TabIndex = 1;
            this.RFID.Text = "RFID";
            this.RFID.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1026, 336);
            this.panel3.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pictureEdit2);
            this.panel4.Controls.Add(this.pictureEdit1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 661);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1036, 100);
            this.panel4.TabIndex = 2;
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.EditValue = global::RfidClinic.Properties.Resources.back_64;
            this.pictureEdit2.Location = new System.Drawing.Point(68, 17);
            this.pictureEdit2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit2.Size = new System.Drawing.Size(72, 83);
            this.pictureEdit2.TabIndex = 3;
            this.pictureEdit2.Visible = false;
            this.pictureEdit2.EditValueChanged += new System.EventHandler(this.pictureEdit2_EditValueChanged);
            this.pictureEdit2.Click += new System.EventHandler(this.pictureEdit2_Click);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(947, 17);
            this.pictureEdit1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Size = new System.Drawing.Size(72, 83);
            this.pictureEdit1.TabIndex = 2;
            this.pictureEdit1.Click += new System.EventHandler(this.pictureEdit1_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lInfo);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 80);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1036, 100);
            this.panel5.TabIndex = 3;
            // 
            // lInfo
            // 
            this.lInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 32F, System.Drawing.FontStyle.Bold);
            this.lInfo.Appearance.ForeColor = System.Drawing.Color.White;
            this.lInfo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lInfo.Location = new System.Drawing.Point(0, 0);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(1036, 100);
            this.lInfo.TabIndex = 2;
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.Angle = 45F;
            this.gradientPanel1.BackColor = System.Drawing.Color.Transparent;
            this.gradientPanel1.ButtomColor = System.Drawing.Color.Empty;
            this.gradientPanel1.Controls.Add(this.panel7);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(1036, 80);
            this.gradientPanel1.TabIndex = 4;
            this.gradientPanel1.TopColor = System.Drawing.Color.Transparent;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(0)))), ((int)(((byte)(151)))));
            this.panel7.Controls.Add(this.lblPoli);
            this.panel7.Controls.Add(this.pictureEdit3);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1036, 80);
            this.panel7.TabIndex = 7;
            // 
            // lblPoli
            // 
            this.lblPoli.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblPoli.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoli.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblPoli.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblPoli.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblPoli.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblPoli.Location = new System.Drawing.Point(131, 3);
            this.lblPoli.Name = "lblPoli";
            this.lblPoli.Size = new System.Drawing.Size(407, 74);
            this.lblPoli.TabIndex = 1;
            this.lblPoli.Text = "KLINIK PRATAMA SANTOSA";
            // 
            // pictureEdit3
            // 
            this.pictureEdit3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureEdit3.EditValue = ((object)(resources.GetObject("pictureEdit3.EditValue")));
            this.pictureEdit3.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit3.Name = "pictureEdit3";
            this.pictureEdit3.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit3.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit3.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit3.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit3.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.pictureEdit3.Size = new System.Drawing.Size(131, 80);
            this.pictureEdit3.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            // 
            // timer2
            // 
            this.timer2.Interval = 200;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Reservation
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1036, 761);
            this.ControlBox = false;
            this.Controls.Add(this.gradientPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Metropolis";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Reservation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Aplikasi Antrian Klinik Pratama Santosa";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Reservation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.gradientPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textScanOut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textRecvTemp.Properties)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.panel5.ResumeLayout(false);
            this.gradientPanel1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit3.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private DevExpress.XtraEditors.LabelControl lEmpid;
        private DevExpress.XtraEditors.LabelControl lName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lRfid;
        private DevExpress.XtraEditors.LabelControl lPurpose;
        private DevExpress.XtraEditors.LabelControl Tujuan;
        private DevExpress.XtraEditors.LabelControl RFID;
        private DevExpress.XtraEditors.LabelControl lInfo;
        private DevExpress.XtraSplashScreen.SplashScreenManager loading;
        private System.Windows.Forms.Timer timerStart;
        private System.Windows.Forms.Timer timerEnd;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private DevExpress.XtraEditors.TextEdit textScanOut;
        private DevExpress.XtraEditors.TextEdit textRecvTemp;
        private System.Windows.Forms.Panel panel5;
        private GradientPanel gradientPanel1;
        private GradientPanel gradientPanel2;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private System.Windows.Forms.Panel panel7;
        private DevExpress.XtraEditors.LabelControl lblPoli;
        private DevExpress.XtraEditors.PictureEdit pictureEdit3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private DevExpress.XtraEditors.LabelControl lbl_noantrian;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}

