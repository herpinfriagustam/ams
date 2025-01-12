namespace Clinic
{
    partial class ReservationScan
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lStatus = new DevExpress.XtraEditors.LabelControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lPurpose = new System.Windows.Forms.Label();
            this.lRfid = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lEmpid = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDoc = new DevExpress.XtraEditors.SimpleButton();
            this.btnMid = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lInfo = new DevExpress.XtraEditors.LabelControl();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textRecvTemp = new DevExpress.XtraEditors.TextEdit();
            this.textScanOut = new DevExpress.XtraEditors.TextEdit();
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            this.timerEnd = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.loading = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::Clinic.WaitForm1), true, true);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textRecvTemp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textScanOut.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(723, 408);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(717, 74);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel3.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(717, 74);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lStatus);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(475, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 68);
            this.panel3.TabIndex = 0;
            // 
            // lStatus
            // 
            this.lStatus.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lStatus.Location = new System.Drawing.Point(0, 0);
            this.lStatus.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(239, 68);
            this.lStatus.TabIndex = 0;
            this.lStatus.Text = "-";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lPurpose);
            this.panel4.Controls.Add(this.lRfid);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(275, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(194, 68);
            this.panel4.TabIndex = 1;
            // 
            // lPurpose
            // 
            this.lPurpose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lPurpose.AutoSize = true;
            this.lPurpose.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPurpose.Location = new System.Drawing.Point(78, 35);
            this.lPurpose.Name = "lPurpose";
            this.lPurpose.Size = new System.Drawing.Size(18, 23);
            this.lPurpose.TabIndex = 9;
            this.lPurpose.Text = "-";
            this.lPurpose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lRfid
            // 
            this.lRfid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lRfid.AutoSize = true;
            this.lRfid.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lRfid.Location = new System.Drawing.Point(78, 12);
            this.lRfid.Name = "lRfid";
            this.lRfid.Size = new System.Drawing.Size(18, 23);
            this.lRfid.TabIndex = 7;
            this.lRfid.Text = "-";
            this.lRfid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Tujuan";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "RFID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lName);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.lEmpid);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(266, 68);
            this.panel5.TabIndex = 2;
            // 
            // lName
            // 
            this.lName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lName.AutoSize = true;
            this.lName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lName.Location = new System.Drawing.Point(75, 35);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(18, 23);
            this.lName.TabIndex = 6;
            this.lName.Text = "-";
            this.lName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(13, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 19);
            this.label6.TabIndex = 5;
            this.label6.Text = "Nama";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lEmpid
            // 
            this.lEmpid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lEmpid.AutoSize = true;
            this.lEmpid.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lEmpid.Location = new System.Drawing.Point(75, 12);
            this.lEmpid.Name = "lEmpid";
            this.lEmpid.Size = new System.Drawing.Size(18, 23);
            this.lEmpid.TabIndex = 4;
            this.lEmpid.Text = "-";
            this.lEmpid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(13, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "NIK";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnDoc, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMid, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 83);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(717, 272);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnDoc
            // 
            this.btnDoc.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDoc.Appearance.Options.UseBackColor = true;
            this.btnDoc.Appearance.Options.UseFont = true;
            this.btnDoc.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDoc.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btnDoc.Location = new System.Drawing.Point(3, 3);
            this.btnDoc.Name = "btnDoc";
            this.btnDoc.Size = new System.Drawing.Size(352, 266);
            this.btnDoc.TabIndex = 0;
            this.btnDoc.Text = "Dokter";
            this.btnDoc.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnMid
            // 
            this.btnMid.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMid.Appearance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMid.Appearance.Options.UseBackColor = true;
            this.btnMid.Appearance.Options.UseFont = true;
            this.btnMid.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMid.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnMid.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btnMid.Location = new System.Drawing.Point(361, 3);
            this.btnMid.Name = "btnMid";
            this.btnMid.Size = new System.Drawing.Size(353, 266);
            this.btnMid.TabIndex = 1;
            this.btnMid.Text = "Bidan";
            this.btnMid.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lInfo);
            this.panel2.Controls.Add(this.richTextBox1);
            this.panel2.Controls.Add(this.textRecvTemp);
            this.panel2.Controls.Add(this.textScanOut);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 361);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(717, 44);
            this.panel2.TabIndex = 2;
            // 
            // lInfo
            // 
            this.lInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lInfo.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lInfo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lInfo.Location = new System.Drawing.Point(0, 0);
            this.lInfo.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(717, 44);
            this.lInfo.TabIndex = 5;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(146, 10);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(67, 25);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // textRecvTemp
            // 
            this.textRecvTemp.Location = new System.Drawing.Point(78, 13);
            this.textRecvTemp.Name = "textRecvTemp";
            this.textRecvTemp.Size = new System.Drawing.Size(62, 20);
            this.textRecvTemp.TabIndex = 2;
            this.textRecvTemp.Visible = false;
            this.textRecvTemp.TextChanged += new System.EventHandler(this.textRecvTemp_TextChanged);
            // 
            // textScanOut
            // 
            this.textScanOut.Location = new System.Drawing.Point(9, 13);
            this.textScanOut.Name = "textScanOut";
            this.textScanOut.Size = new System.Drawing.Size(63, 20);
            this.textScanOut.TabIndex = 1;
            this.textScanOut.Visible = false;
            // 
            // timerStart
            // 
            this.timerStart.Interval = 200;
            this.timerStart.Tick += new System.EventHandler(this.timerStart_Tick);
            // 
            // timerEnd
            // 
            this.timerEnd.Tick += new System.EventHandler(this.timerEnd_Tick);
            // 
            // serialPort1
            // 
            this.serialPort1.DataBits = 7;
            this.serialPort1.RtsEnable = true;
            // 
            // loading
            // 
            this.loading.ClosingDelay = 500;
            // 
            // ReservationScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 408);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ReservationScan";
            this.Text = "Reservation Scan";
            this.Load += new System.EventHandler(this.ReservationScan_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textRecvTemp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textScanOut.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnDoc;
        private DevExpress.XtraEditors.SimpleButton btnMid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lEmpid;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lPurpose;
        private System.Windows.Forms.Label lRfid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timerStart;
        private System.Windows.Forms.Timer timerEnd;
        private DevExpress.XtraEditors.LabelControl lStatus;
        private DevExpress.XtraEditors.TextEdit textScanOut;
        private System.IO.Ports.SerialPort serialPort1;
        private DevExpress.XtraEditors.TextEdit textRecvTemp;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private DevExpress.XtraEditors.LabelControl lInfo;
        private DevExpress.XtraSplashScreen.SplashScreenManager loading;
    }
}