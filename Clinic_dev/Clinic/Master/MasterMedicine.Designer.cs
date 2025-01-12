namespace Clinic
{
    partial class MasterMedicine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterMedicine));
            this.loading = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::Clinic.WaitForm1), true, true);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.luStatus = new DevExpress.XtraEditors.LookUpEdit();
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            this.btnLoadKate = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddKate = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveKate = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lTransIn = new DevExpress.XtraEditors.LabelControl();
            this.btnAddItem = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveItem = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnAddOut = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveOut = new DevExpress.XtraEditors.SimpleButton();
            this.lTransOut = new DevExpress.XtraEditors.LabelControl();
            this.gridControl3 = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtLimitStok = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl4 = new DevExpress.XtraGrid.GridControl();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luStatus.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLimitStok.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            this.SuspendLayout();
            // 
            // loading
            // 
            this.loading.ClosingDelay = 500;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1433, 679);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.luStatus);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.btnLoadKate);
            this.panel1.Controls.Add(this.btnAddKate);
            this.panel1.Controls.Add(this.btnSaveKate);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 70);
            this.panel1.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(482, 44);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(43, 13);
            this.labelControl2.TabIndex = 39;
            this.labelControl2.Text = "Status :";
            // 
            // luStatus
            // 
            this.luStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.luStatus.Location = new System.Drawing.Point(530, 41);
            this.luStatus.Name = "luStatus";
            this.luStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.luStatus.Size = new System.Drawing.Size(95, 20);
            this.luStatus.TabIndex = 38;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.Location = new System.Drawing.Point(723, 39);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(61, 23);
            this.btnDownload.TabIndex = 37;
            this.btnDownload.Text = "Unduh";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnLoadKate
            // 
            this.btnLoadKate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadKate.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadKate.Image")));
            this.btnLoadKate.Location = new System.Drawing.Point(631, 39);
            this.btnLoadKate.Name = "btnLoadKate";
            this.btnLoadKate.Size = new System.Drawing.Size(86, 23);
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
            this.labelControl1.Size = new System.Drawing.Size(144, 19);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "Data Master Obat";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lTransIn);
            this.panel2.Controls.Add(this.btnAddItem);
            this.panel2.Controls.Add(this.btnSaveItem);
            this.panel2.Controls.Add(this.labelControl3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(788, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(645, 70);
            this.panel2.TabIndex = 1;
            // 
            // lTransIn
            // 
            this.lTransIn.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTransIn.Location = new System.Drawing.Point(3, 44);
            this.lTransIn.Name = "lTransIn";
            this.lTransIn.Size = new System.Drawing.Size(107, 16);
            this.lTransIn.TabIndex = 34;
            this.lTransIn.Text = "Transaksi Masuk";
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddItem.Image = ((System.Drawing.Image)(resources.GetObject("btnAddItem.Image")));
            this.btnAddItem.Location = new System.Drawing.Point(502, 39);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(67, 23);
            this.btnAddItem.TabIndex = 33;
            this.btnAddItem.Text = "Tambah";
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnSaveItem
            // 
            this.btnSaveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveItem.Image")));
            this.btnSaveItem.Location = new System.Drawing.Point(575, 39);
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
            this.labelControl3.Size = new System.Drawing.Size(167, 19);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "Data Transaksi Obat";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.gridControl2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.gridControl3, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(788, 70);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(645, 609);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(3, 3);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(639, 278);
            this.gridControl2.TabIndex = 3;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.RowAutoHeight = true;
            this.gridView2.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView2_CustomDrawRowIndicator);
            this.gridView2.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView2_RowCellStyle);
            this.gridView2.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView2_InitNewRow);
            this.gridView2.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAddOut);
            this.panel3.Controls.Add(this.btnSaveOut);
            this.panel3.Controls.Add(this.lTransOut);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 284);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(645, 40);
            this.panel3.TabIndex = 4;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // btnAddOut
            // 
            this.btnAddOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddOut.Image = ((System.Drawing.Image)(resources.GetObject("btnAddOut.Image")));
            this.btnAddOut.Location = new System.Drawing.Point(502, 11);
            this.btnAddOut.Name = "btnAddOut";
            this.btnAddOut.Size = new System.Drawing.Size(67, 23);
            this.btnAddOut.TabIndex = 37;
            this.btnAddOut.Text = "Tambah";
            this.btnAddOut.Click += new System.EventHandler(this.btnAddOut_Click);
            // 
            // btnSaveOut
            // 
            this.btnSaveOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveOut.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveOut.Image")));
            this.btnSaveOut.Location = new System.Drawing.Point(575, 11);
            this.btnSaveOut.Name = "btnSaveOut";
            this.btnSaveOut.Size = new System.Drawing.Size(67, 23);
            this.btnSaveOut.TabIndex = 36;
            this.btnSaveOut.Text = "Simpan";
            this.btnSaveOut.Click += new System.EventHandler(this.btnSaveOut_Click);
            // 
            // lTransOut
            // 
            this.lTransOut.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTransOut.Location = new System.Drawing.Point(3, 14);
            this.lTransOut.Name = "lTransOut";
            this.lTransOut.Size = new System.Drawing.Size(106, 16);
            this.lTransOut.TabIndex = 35;
            this.lTransOut.Text = "Transaksi Keluar";
            // 
            // gridControl3
            // 
            this.gridControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl3.Location = new System.Drawing.Point(3, 327);
            this.gridControl3.MainView = this.gridView3;
            this.gridControl3.Name = "gridControl3";
            this.gridControl3.Size = new System.Drawing.Size(639, 279);
            this.gridControl3.TabIndex = 5;
            this.gridControl3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.gridControl3;
            this.gridView3.Name = "gridView3";
            this.gridView3.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView3_CustomDrawRowIndicator);
            this.gridView3.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView3_RowCellStyle);
            this.gridView3.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView3_InitNewRow);
            this.gridView3.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView3_CellValueChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.gridControl1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.gridControl4, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 70);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(788, 609);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.labelControl5);
            this.panel4.Controls.Add(this.txtLimitStok);
            this.panel4.Controls.Add(this.labelControl4);
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 359);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(788, 50);
            this.panel4.TabIndex = 5;
            // 
            // labelControl5
            // 
            this.labelControl5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Location = new System.Drawing.Point(617, 17);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(103, 16);
            this.labelControl5.TabIndex = 38;
            this.labelControl5.Text = "Jumlah Stok <=";
            // 
            // txtLimitStok
            // 
            this.txtLimitStok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLimitStok.EditValue = "5";
            this.txtLimitStok.Location = new System.Drawing.Point(723, 16);
            this.txtLimitStok.Name = "txtLimitStok";
            this.txtLimitStok.Properties.Mask.EditMask = "n0";
            this.txtLimitStok.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtLimitStok.Size = new System.Drawing.Size(52, 20);
            this.txtLimitStok.TabIndex = 37;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl4.Location = new System.Drawing.Point(5, 14);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(115, 19);
            this.labelControl4.TabIndex = 35;
            this.labelControl4.Text = "Stok Terbatas";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Clinic.Properties.Resources.alert;
            this.pictureBox1.Location = new System.Drawing.Point(121, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 36;
            this.pictureBox1.TabStop = false;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 3);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(782, 353);
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
            // gridControl4
            // 
            this.gridControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl4.Location = new System.Drawing.Point(3, 412);
            this.gridControl4.MainView = this.gridView4;
            this.gridControl4.Name = "gridControl4";
            this.gridControl4.Size = new System.Drawing.Size(782, 194);
            this.gridControl4.TabIndex = 3;
            this.gridControl4.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView4});
            // 
            // gridView4
            // 
            this.gridView4.GridControl = this.gridControl4;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsView.RowAutoHeight = true;
            this.gridView4.OptionsView.ShowAutoFilterRow = true;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            this.gridView4.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView4_CustomDrawRowIndicator);
            this.gridView4.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView4_RowCellStyle);
            // 
            // MasterMedicine
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1433, 679);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MasterMedicine";
            this.Text = "Master Obat";
            this.Load += new System.EventHandler(this.PrescriptionList_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luStatus.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLimitStok.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
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
        private DevExpress.XtraEditors.SimpleButton btnLoadKate;
        private DevExpress.XtraEditors.SimpleButton btnAddItem;
        private DevExpress.XtraEditors.SimpleButton btnSaveItem;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraGrid.GridControl gridControl3;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraEditors.LabelControl lTransIn;
        private DevExpress.XtraEditors.LabelControl lTransOut;
        private DevExpress.XtraEditors.SimpleButton btnAddOut;
        private DevExpress.XtraEditors.SimpleButton btnSaveOut;
        private DevExpress.XtraEditors.SimpleButton btnDownload;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LookUpEdit luStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.GridControl gridControl4;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraEditors.TextEdit txtLimitStok;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}