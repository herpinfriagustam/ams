namespace Clinic
{
    partial class ReservationMngt3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReservationMngt3));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNotif = new DevExpress.XtraEditors.SimpleButton();
            this.btnTunda = new DevExpress.XtraEditors.SimpleButton();
            this.btnLanjut = new DevExpress.XtraEditors.SimpleButton();
            this.btnCreate = new DevExpress.XtraEditors.SimpleButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cQue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cEmpid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cGender = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cAge = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cPoli = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cTypePatient = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cWorkAcc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cPurpose = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddAnam = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveAnam = new DevExpress.XtraEditors.SimpleButton();
            this.lFirstInsp = new DevExpress.XtraEditors.LabelControl();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSaveAdd = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl3 = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.loading = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::Clinic.WaitForm1), true, true);
            this.timerObs = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gridControl2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.gridControl3, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1132, 649);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnNotif);
            this.panel1.Controls.Add(this.btnTunda);
            this.panel1.Controls.Add(this.btnLanjut);
            this.panel1.Controls.Add(this.btnCreate);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.simpleButton4);
            this.panel1.Controls.Add(this.simpleButton3);
            this.panel1.Controls.Add(this.simpleButton2);
            this.panel1.Controls.Add(this.simpleButton1);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1126, 64);
            this.panel1.TabIndex = 0;
            // 
            // btnNotif
            // 
            this.btnNotif.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNotif.Image = ((System.Drawing.Image)(resources.GetObject("btnNotif.Image")));
            this.btnNotif.Location = new System.Drawing.Point(770, 38);
            this.btnNotif.Name = "btnNotif";
            this.btnNotif.Size = new System.Drawing.Size(73, 23);
            this.btnNotif.TabIndex = 32;
            this.btnNotif.Text = "Cek Data";
            this.btnNotif.Visible = false;
            this.btnNotif.Click += new System.EventHandler(this.btnNotif_Click);
            // 
            // btnTunda
            // 
            this.btnTunda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTunda.Enabled = false;
            this.btnTunda.Image = ((System.Drawing.Image)(resources.GetObject("btnTunda.Image")));
            this.btnTunda.Location = new System.Drawing.Point(849, 38);
            this.btnTunda.Name = "btnTunda";
            this.btnTunda.Size = new System.Drawing.Size(67, 23);
            this.btnTunda.TabIndex = 31;
            this.btnTunda.Text = "Tunda";
            this.btnTunda.Visible = false;
            this.btnTunda.Click += new System.EventHandler(this.btnTunda_Click);
            // 
            // btnLanjut
            // 
            this.btnLanjut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLanjut.Enabled = false;
            this.btnLanjut.Image = ((System.Drawing.Image)(resources.GetObject("btnLanjut.Image")));
            this.btnLanjut.Location = new System.Drawing.Point(922, 38);
            this.btnLanjut.Name = "btnLanjut";
            this.btnLanjut.Size = new System.Drawing.Size(67, 23);
            this.btnLanjut.TabIndex = 30;
            this.btnLanjut.Text = "Lanjut";
            this.btnLanjut.Visible = false;
            this.btnLanjut.Click += new System.EventHandler(this.btnLanjut_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Image = ((System.Drawing.Image)(resources.GetObject("btnCreate.Image")));
            this.btnCreate.Location = new System.Drawing.Point(995, 38);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(128, 23);
            this.btnCreate.TabIndex = 29;
            this.btnCreate.Text = "Buat Medical Record";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(559, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(564, 29);
            this.richTextBox1.TabIndex = 29;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(155, 38);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(67, 23);
            this.simpleButton4.TabIndex = 28;
            this.simpleButton4.Text = "Panggil";
            this.simpleButton4.Visible = false;
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(10, 38);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(67, 23);
            this.simpleButton3.TabIndex = 27;
            this.simpleButton3.Text = "Tambah";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(82, 38);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(67, 23);
            this.simpleButton2.TabIndex = 26;
            this.simpleButton2.Text = "Simpan";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(228, 38);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(84, 23);
            this.simpleButton1.TabIndex = 25;
            this.simpleButton1.Text = "Muat Ulang";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(10, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(79, 19);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Reservasi";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 73);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemLookUpEdit2});
            this.gridControl1.Size = new System.Drawing.Size(1126, 293);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.cQue,
            this.cEmpid,
            this.cName,
            this.cGender,
            this.cAge,
            this.cPoli,
            this.cTypePatient,
            this.cWorkAcc,
            this.cPurpose,
            this.cStatus});
            gridFormatRule1.Name = "Format0";
            gridFormatRule1.Rule = null;
            this.gridView1.FormatRules.Add(gridFormatRule1);
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            this.gridView1.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            this.gridView1.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridView1_RowStyle);
            this.gridView1.EditFormPrepared += new DevExpress.XtraGrid.Views.Grid.EditFormPreparedEventHandler(this.gridView1_EditFormPrepared);
            this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            this.gridView1.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.gridView1_RowUpdated);
            // 
            // cQue
            // 
            this.cQue.Caption = "Antrian";
            this.cQue.Name = "cQue";
            this.cQue.Visible = true;
            this.cQue.VisibleIndex = 0;
            // 
            // cEmpid
            // 
            this.cEmpid.Caption = "Pasien No";
            this.cEmpid.Name = "cEmpid";
            this.cEmpid.OptionsColumn.ReadOnly = true;
            this.cEmpid.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.cEmpid.Visible = true;
            this.cEmpid.VisibleIndex = 1;
            // 
            // cName
            // 
            this.cName.Caption = "Nama";
            this.cName.Name = "cName";
            this.cName.Visible = true;
            this.cName.VisibleIndex = 2;
            // 
            // cGender
            // 
            this.cGender.Caption = "Gender";
            this.cGender.Name = "cGender";
            // 
            // cAge
            // 
            this.cAge.Caption = "Umur";
            this.cAge.Name = "cAge";
            // 
            // cPoli
            // 
            this.cPoli.Caption = "Poli";
            this.cPoli.Name = "cPoli";
            this.cPoli.OptionsColumn.AllowEdit = false;
            this.cPoli.Visible = true;
            this.cPoli.VisibleIndex = 3;
            // 
            // cTypePatient
            // 
            this.cTypePatient.Caption = "Pasien";
            this.cTypePatient.Name = "cTypePatient";
            this.cTypePatient.OptionsColumn.AllowEdit = false;
            this.cTypePatient.Visible = true;
            this.cTypePatient.VisibleIndex = 4;
            // 
            // cWorkAcc
            // 
            this.cWorkAcc.Caption = "Work Accident";
            this.cWorkAcc.Name = "cWorkAcc";
            this.cWorkAcc.OptionsColumn.AllowEdit = false;
            this.cWorkAcc.Visible = true;
            this.cWorkAcc.VisibleIndex = 5;
            // 
            // cPurpose
            // 
            this.cPurpose.Caption = "Berobat";
            this.cPurpose.Name = "cPurpose";
            this.cPurpose.OptionsColumn.AllowEdit = false;
            this.cPurpose.Visible = true;
            this.cPurpose.VisibleIndex = 6;
            // 
            // cStatus
            // 
            this.cStatus.Caption = "Status";
            this.cStatus.Name = "cStatus";
            this.cStatus.OptionsColumn.AllowEdit = false;
            this.cStatus.Visible = true;
            this.cStatus.VisibleIndex = 7;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnAddAnam);
            this.panel2.Controls.Add(this.btnSaveAnam);
            this.panel2.Controls.Add(this.lFirstInsp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 372);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1126, 44);
            this.panel2.TabIndex = 2;
            // 
            // btnAddAnam
            // 
            this.btnAddAnam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAnam.Image = ((System.Drawing.Image)(resources.GetObject("btnAddAnam.Image")));
            this.btnAddAnam.Location = new System.Drawing.Point(977, 12);
            this.btnAddAnam.Name = "btnAddAnam";
            this.btnAddAnam.Size = new System.Drawing.Size(67, 23);
            this.btnAddAnam.TabIndex = 28;
            this.btnAddAnam.Text = "Tambah";
            this.btnAddAnam.Click += new System.EventHandler(this.btnAddAnam_Click);
            // 
            // btnSaveAnam
            // 
            this.btnSaveAnam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAnam.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAnam.Image")));
            this.btnSaveAnam.Location = new System.Drawing.Point(1050, 12);
            this.btnSaveAnam.Name = "btnSaveAnam";
            this.btnSaveAnam.Size = new System.Drawing.Size(67, 23);
            this.btnSaveAnam.TabIndex = 27;
            this.btnSaveAnam.Text = "Simpan";
            this.btnSaveAnam.Click += new System.EventHandler(this.btnSaveAnam_Click);
            // 
            // lFirstInsp
            // 
            this.lFirstInsp.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFirstInsp.Location = new System.Drawing.Point(10, 12);
            this.lFirstInsp.Name = "lFirstInsp";
            this.lFirstInsp.Size = new System.Drawing.Size(152, 19);
            this.lFirstInsp.TabIndex = 3;
            this.lFirstInsp.Text = "Pemeriksaan Awal";
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(3, 422);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(1126, 84);
            this.gridControl2.TabIndex = 3;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.ColumnAutoWidth = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridView2.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView2_InitNewRow);
            this.gridView2.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSaveAdd);
            this.panel3.Controls.Add(this.labelControl2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 512);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1126, 44);
            this.panel3.TabIndex = 4;
            // 
            // btnSaveAdd
            // 
            this.btnSaveAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAdd.Image")));
            this.btnSaveAdd.Location = new System.Drawing.Point(1050, 13);
            this.btnSaveAdd.Name = "btnSaveAdd";
            this.btnSaveAdd.Size = new System.Drawing.Size(67, 23);
            this.btnSaveAdd.TabIndex = 30;
            this.btnSaveAdd.Text = "Simpan";
            this.btnSaveAdd.Click += new System.EventHandler(this.btnSaveAdd_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(10, 13);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(169, 19);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "Informasi Tambahan";
            // 
            // gridControl3
            // 
            this.gridControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl3.Location = new System.Drawing.Point(3, 562);
            this.gridControl3.MainView = this.gridView3;
            this.gridControl3.Name = "gridControl3";
            this.gridControl3.Size = new System.Drawing.Size(1126, 84);
            this.gridControl3.TabIndex = 5;
            this.gridControl3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.gridControl3;
            this.gridView3.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsView.ColumnAutoWidth = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            // 
            // loading
            // 
            this.loading.ClosingDelay = 500;
            // 
            // timerObs
            // 
            this.timerObs.Interval = 1000;
            this.timerObs.Tick += new System.EventHandler(this.timerObs_Tick);
            // 
            // ReservationMngt3
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 649);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ReservationMngt3";
            this.Text = "Reservasi dan Pemeriksaan Awal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ReservationInput_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn cQue;
        private DevExpress.XtraGrid.Columns.GridColumn cEmpid;
        private DevExpress.XtraGrid.Columns.GridColumn cGender;
        private DevExpress.XtraGrid.Columns.GridColumn cAge;
        private DevExpress.XtraGrid.Columns.GridColumn cPoli;
        private DevExpress.XtraGrid.Columns.GridColumn cName;
        private DevExpress.XtraGrid.Columns.GridColumn cTypePatient;
        private DevExpress.XtraGrid.Columns.GridColumn cWorkAcc;
        private DevExpress.XtraGrid.Columns.GridColumn cPurpose;
        private DevExpress.XtraSplashScreen.SplashScreenManager loading;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraGrid.Columns.GridColumn cStatus;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.LabelControl lFirstInsp;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.SimpleButton btnSaveAnam;
        private DevExpress.XtraEditors.SimpleButton btnCreate;
        private DevExpress.XtraEditors.SimpleButton btnAddAnam;
        private DevExpress.XtraEditors.SimpleButton btnSaveAdd;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGrid.GridControl gridControl3;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraEditors.SimpleButton btnTunda;
        private DevExpress.XtraEditors.SimpleButton btnLanjut;
        private DevExpress.XtraEditors.SimpleButton btnNotif;
        private System.Windows.Forms.Timer timerObs;
    }
}