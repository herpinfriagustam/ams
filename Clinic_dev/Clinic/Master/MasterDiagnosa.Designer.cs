namespace Clinic
{
    partial class MasterDiagnosa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterDiagnosa));
            this.loading = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::Clinic.WaitForm1), true, true);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gridControl4 = new DevExpress.XtraGrid.GridControl();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.luStatus = new DevExpress.XtraEditors.LookUpEdit();
            this.btnLoadKate = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddKate = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveKate = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDownload2 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.luStat2 = new DevExpress.XtraEditors.LookUpEdit();
            this.btnLoadItem = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddItem = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveItem = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl3 = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luStatus.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luStat2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // loading
            // 
            this.loading.ClosingDelay = 500;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.gridControl4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gridControl2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gridControl3, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1113, 657);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gridControl4
            // 
            this.gridControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl4.Location = new System.Drawing.Point(448, 560);
            this.gridControl4.MainView = this.gridView4;
            this.gridControl4.Name = "gridControl4";
            this.gridControl4.Size = new System.Drawing.Size(662, 94);
            this.gridControl4.TabIndex = 7;
            this.gridControl4.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView4});
            // 
            // gridView4
            // 
            this.gridView4.GridControl = this.gridControl4;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsView.ShowGroupPanel = false;
            this.gridView4.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView4_CustomDrawRowIndicator);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.labelControl6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(445, 517);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(668, 40);
            this.panel4.TabIndex = 5;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl6.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl6.Location = new System.Drawing.Point(3, 9);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(75, 19);
            this.labelControl6.TabIndex = 7;
            this.labelControl6.Text = "Diagnosa";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.luStatus);
            this.panel1.Controls.Add(this.btnLoadKate);
            this.panel1.Controls.Add(this.btnAddKate);
            this.panel1.Controls.Add(this.btnSaveKate);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 70);
            this.panel1.TabIndex = 0;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.Location = new System.Drawing.Point(382, 39);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(61, 23);
            this.btnDownload.TabIndex = 36;
            this.btnDownload.Text = "Unduh";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(148, 45);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(43, 13);
            this.labelControl2.TabIndex = 35;
            this.labelControl2.Text = "Status :";
            this.labelControl2.Visible = false;
            // 
            // luStatus
            // 
            this.luStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.luStatus.Location = new System.Drawing.Point(196, 41);
            this.luStatus.Name = "luStatus";
            this.luStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luStatus.Size = new System.Drawing.Size(95, 20);
            this.luStatus.TabIndex = 34;
            this.luStatus.Visible = false;
            // 
            // btnLoadKate
            // 
            this.btnLoadKate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadKate.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadKate.Image")));
            this.btnLoadKate.Location = new System.Drawing.Point(293, 39);
            this.btnLoadKate.Name = "btnLoadKate";
            this.btnLoadKate.Size = new System.Drawing.Size(87, 23);
            this.btnLoadKate.TabIndex = 33;
            this.btnLoadKate.Text = "Muat Ulang";
            this.btnLoadKate.Click += new System.EventHandler(this.btnLoadKate_Click);
            // 
            // btnAddKate
            // 
            this.btnAddKate.Image = ((System.Drawing.Image)(resources.GetObject("btnAddKate.Image")));
            this.btnAddKate.Location = new System.Drawing.Point(6, 39);
            this.btnAddKate.Name = "btnAddKate";
            this.btnAddKate.Size = new System.Drawing.Size(67, 23);
            this.btnAddKate.TabIndex = 31;
            this.btnAddKate.Text = "Tambah";
            this.btnAddKate.Click += new System.EventHandler(this.btnAddKate_Click);
            // 
            // btnSaveKate
            // 
            this.btnSaveKate.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveKate.Image")));
            this.btnSaveKate.Location = new System.Drawing.Point(79, 39);
            this.btnSaveKate.Name = "btnSaveKate";
            this.btnSaveKate.Size = new System.Drawing.Size(67, 23);
            this.btnSaveKate.TabIndex = 30;
            this.btnSaveKate.Text = "Simpan";
            this.btnSaveKate.Click += new System.EventHandler(this.btnSaveKate_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(6, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(150, 19);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "Diagnosa Kategori";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDownload2);
            this.panel2.Controls.Add(this.labelControl4);
            this.panel2.Controls.Add(this.luStat2);
            this.panel2.Controls.Add(this.btnLoadItem);
            this.panel2.Controls.Add(this.btnAddItem);
            this.panel2.Controls.Add(this.btnSaveItem);
            this.panel2.Controls.Add(this.labelControl3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(445, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(668, 70);
            this.panel2.TabIndex = 1;
            // 
            // btnDownload2
            // 
            this.btnDownload2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload2.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload2.Image")));
            this.btnDownload2.Location = new System.Drawing.Point(602, 40);
            this.btnDownload2.Name = "btnDownload2";
            this.btnDownload2.Size = new System.Drawing.Size(62, 23);
            this.btnDownload2.TabIndex = 39;
            this.btnDownload2.Text = "Unduh";
            this.btnDownload2.Click += new System.EventHandler(this.btnDownload2_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Location = new System.Drawing.Point(361, 45);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(43, 13);
            this.labelControl4.TabIndex = 38;
            this.labelControl4.Text = "Status :";
            this.labelControl4.Visible = false;
            // 
            // luStat2
            // 
            this.luStat2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.luStat2.Location = new System.Drawing.Point(411, 42);
            this.luStat2.Name = "luStat2";
            this.luStat2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luStat2.Size = new System.Drawing.Size(100, 20);
            this.luStat2.TabIndex = 37;
            this.luStat2.Visible = false;
            // 
            // btnLoadItem
            // 
            this.btnLoadItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadItem.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadItem.Image")));
            this.btnLoadItem.Location = new System.Drawing.Point(515, 40);
            this.btnLoadItem.Name = "btnLoadItem";
            this.btnLoadItem.Size = new System.Drawing.Size(84, 23);
            this.btnLoadItem.TabIndex = 36;
            this.btnLoadItem.Text = "Muat Ulang";
            this.btnLoadItem.Click += new System.EventHandler(this.btnLoadItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAddItem.Image")));
            this.btnAddItem.Location = new System.Drawing.Point(3, 40);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(67, 23);
            this.btnAddItem.TabIndex = 33;
            this.btnAddItem.Text = "Tambah";
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnSaveItem
            // 
            this.btnSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveItem.Image")));
            this.btnSaveItem.Location = new System.Drawing.Point(76, 40);
            this.btnSaveItem.Name = "btnSaveItem";
            this.btnSaveItem.Size = new System.Drawing.Size(67, 23);
            this.btnSaveItem.TabIndex = 32;
            this.btnSaveItem.Text = "Simpan";
            this.btnSaveItem.Click += new System.EventHandler(this.btnSaveItem_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Location = new System.Drawing.Point(3, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(119, 19);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "Diagnosa Item";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 73);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(439, 441);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.RowAutoHeight = true;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick_1);
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator_1);
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(448, 73);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(662, 441);
            this.gridControl2.TabIndex = 3;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.RowAutoHeight = true;
            this.gridView2.OptionsView.ShowAutoFilterRow = true;
            this.gridView2.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView2_CustomDrawRowIndicator);
            this.gridView2.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView2_RowCellStyle);
            this.gridView2.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView2_InitNewRow);
            this.gridView2.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelControl5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 517);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(445, 40);
            this.panel3.TabIndex = 4;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl5.Location = new System.Drawing.Point(6, 9);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(70, 19);
            this.labelControl5.TabIndex = 6;
            this.labelControl5.Text = "Kategori";
            // 
            // gridControl3
            // 
            this.gridControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl3.Location = new System.Drawing.Point(3, 560);
            this.gridControl3.MainView = this.gridView3;
            this.gridControl3.Name = "gridControl3";
            this.gridControl3.Size = new System.Drawing.Size(439, 94);
            this.gridControl3.TabIndex = 6;
            this.gridControl3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.gridControl3;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView3_CustomDrawRowIndicator);
            // 
            // MasterDiagnosa
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 657);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MasterDiagnosa";
            this.Text = "Master Diagnosa";
            this.Load += new System.EventHandler(this.PrescriptionList_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luStatus.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luStat2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraSplashScreen.SplashScreenManager loading;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnAddKate;
        private DevExpress.XtraEditors.SimpleButton btnSaveKate;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LookUpEdit luStatus;
        private DevExpress.XtraEditors.SimpleButton btnLoadKate;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LookUpEdit luStat2;
        private DevExpress.XtraEditors.SimpleButton btnLoadItem;
        private DevExpress.XtraEditors.SimpleButton btnAddItem;
        private DevExpress.XtraEditors.SimpleButton btnSaveItem;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.SimpleButton btnDownload;
        private DevExpress.XtraEditors.SimpleButton btnDownload2;
        private DevExpress.XtraGrid.GridControl gridControl4;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private System.Windows.Forms.Panel panel4;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraGrid.GridControl gridControl3;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
    }
}