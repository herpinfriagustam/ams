namespace Clinic
{
    partial class TreatNonMedis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TreatNonMedis));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radMCU = new System.Windows.Forms.RadioButton();
            this.radKIR = new System.Windows.Forms.RadioButton();
            this.dDateEnd = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.dDateBgn = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnDelDosis = new DevExpress.XtraEditors.SimpleButton();
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            this.btnLoadDosis = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveDosis = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddDosis = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dDateEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dDateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dDateBgn.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dDateBgn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1081, 531);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.simpleButton1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.dDateEnd);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.dDateBgn);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.btnDelDosis);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.btnLoadDosis);
            this.panel1.Controls.Add(this.btnSaveDosis);
            this.panel1.Controls.Add(this.btnAddDosis);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1081, 70);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radMCU);
            this.groupBox1.Controls.Add(this.radKIR);
            this.groupBox1.Location = new System.Drawing.Point(292, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 42);
            this.groupBox1.TabIndex = 114;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Option";
            // 
            // radMCU
            // 
            this.radMCU.AutoSize = true;
            this.radMCU.Location = new System.Drawing.Point(98, 19);
            this.radMCU.Name = "radMCU";
            this.radMCU.Size = new System.Drawing.Size(47, 17);
            this.radMCU.TabIndex = 1;
            this.radMCU.Text = "MCU";
            this.radMCU.UseVisualStyleBackColor = true;
            this.radMCU.CheckedChanged += new System.EventHandler(this.radMCU_CheckedChanged);
            // 
            // radKIR
            // 
            this.radKIR.AutoSize = true;
            this.radKIR.Checked = true;
            this.radKIR.Location = new System.Drawing.Point(29, 19);
            this.radKIR.Name = "radKIR";
            this.radKIR.Size = new System.Drawing.Size(42, 17);
            this.radKIR.TabIndex = 0;
            this.radKIR.TabStop = true;
            this.radKIR.Text = "KIR";
            this.radKIR.UseVisualStyleBackColor = true;
            this.radKIR.CheckedChanged += new System.EventHandler(this.radKIR_CheckedChanged);
            // 
            // dDateEnd
            // 
            this.dDateEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dDateEnd.EditValue = null;
            this.dDateEnd.Location = new System.Drawing.Point(833, 39);
            this.dDateEnd.Name = "dDateEnd";
            this.dDateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dDateEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dDateEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dDateEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dDateEnd.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dDateEnd.Size = new System.Drawing.Size(85, 20);
            this.dDateEnd.TabIndex = 113;
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Location = new System.Drawing.Point(824, 42);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(5, 13);
            this.labelControl4.TabIndex = 112;
            this.labelControl4.Text = "-";
            // 
            // dDateBgn
            // 
            this.dDateBgn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dDateBgn.EditValue = null;
            this.dDateBgn.Location = new System.Drawing.Point(735, 39);
            this.dDateBgn.Name = "dDateBgn";
            this.dDateBgn.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dDateBgn.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dDateBgn.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dDateBgn.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dDateBgn.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dDateBgn.Size = new System.Drawing.Size(85, 20);
            this.dDateBgn.TabIndex = 111;
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Location = new System.Drawing.Point(680, 42);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(51, 13);
            this.labelControl3.TabIndex = 110;
            this.labelControl3.Text = "Tanggal :";
            // 
            // btnDelDosis
            // 
            this.btnDelDosis.Image = ((System.Drawing.Image)(resources.GetObject("btnDelDosis.Image")));
            this.btnDelDosis.Location = new System.Drawing.Point(196, 37);
            this.btnDelDosis.Name = "btnDelDosis";
            this.btnDelDosis.Size = new System.Drawing.Size(67, 23);
            this.btnDelDosis.TabIndex = 43;
            this.btnDelDosis.Text = "Hapus";
            this.btnDelDosis.Click += new System.EventHandler(this.btnDelDosis_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.Location = new System.Drawing.Point(1017, 37);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(61, 23);
            this.btnDownload.TabIndex = 42;
            this.btnDownload.Text = "Unduh";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnLoadDosis
            // 
            this.btnLoadDosis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadDosis.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadDosis.Image")));
            this.btnLoadDosis.Location = new System.Drawing.Point(924, 37);
            this.btnLoadDosis.Name = "btnLoadDosis";
            this.btnLoadDosis.Size = new System.Drawing.Size(87, 23);
            this.btnLoadDosis.TabIndex = 41;
            this.btnLoadDosis.Text = "Refresh";
            this.btnLoadDosis.Click += new System.EventHandler(this.btnLoadDosis_Click);
            // 
            // btnSaveDosis
            // 
            this.btnSaveDosis.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveDosis.Image")));
            this.btnSaveDosis.Location = new System.Drawing.Point(85, 37);
            this.btnSaveDosis.Name = "btnSaveDosis";
            this.btnSaveDosis.Size = new System.Drawing.Size(67, 23);
            this.btnSaveDosis.TabIndex = 40;
            this.btnSaveDosis.Text = "Simpan";
            this.btnSaveDosis.Click += new System.EventHandler(this.btnSaveDosis_Click);
            // 
            // btnAddDosis
            // 
            this.btnAddDosis.Image = ((System.Drawing.Image)(resources.GetObject("btnAddDosis.Image")));
            this.btnAddDosis.Location = new System.Drawing.Point(12, 37);
            this.btnAddDosis.Name = "btnAddDosis";
            this.btnAddDosis.Size = new System.Drawing.Size(67, 23);
            this.btnAddDosis.TabIndex = 39;
            this.btnAddDosis.Text = "Tambah";
            this.btnAddDosis.Click += new System.EventHandler(this.btnAddDosis_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(174, 19);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "Pelayanan Non Medis";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 73);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1075, 455);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.RowAutoHeight = true;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(458, 34);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(67, 23);
            this.simpleButton1.TabIndex = 115;
            this.simpleButton1.Text = "Print";
            // 
            // TreatNonMedis
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 531);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TreatNonMedis";
            this.Text = "Pelayanan Non Medis";
            this.Load += new System.EventHandler(this.MasterFormula_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dDateEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dDateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dDateBgn.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dDateBgn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnDownload;
        private DevExpress.XtraEditors.SimpleButton btnLoadDosis;
        private DevExpress.XtraEditors.SimpleButton btnSaveDosis;
        private DevExpress.XtraEditors.SimpleButton btnAddDosis;
        private DevExpress.XtraEditors.SimpleButton btnDelDosis;
        private DevExpress.XtraEditors.DateEdit dDateEnd;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.DateEdit dDateBgn;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radMCU;
        private System.Windows.Forms.RadioButton radKIR;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}