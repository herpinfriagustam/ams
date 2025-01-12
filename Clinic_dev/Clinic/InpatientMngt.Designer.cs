namespace Clinic
{
    partial class InpatientMngt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InpatientMngt));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dEndDt = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.dStartDt = new DevExpress.XtraEditors.DateEdit();
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            this.btnLoadRanap = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelRanap = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveRanap = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddRanap = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dEndDt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEndDt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dStartDt.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dStartDt.Properties)).BeginInit();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(770, 465);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dEndDt);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.dStartDt);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.btnLoadRanap);
            this.panel1.Controls.Add(this.btnDelRanap);
            this.panel1.Controls.Add(this.btnSaveRanap);
            this.panel1.Controls.Add(this.btnAddRanap);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 70);
            this.panel1.TabIndex = 0;
            // 
            // dEndDt
            // 
            this.dEndDt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dEndDt.EditValue = null;
            this.dEndDt.Location = new System.Drawing.Point(519, 39);
            this.dEndDt.Name = "dEndDt";
            this.dEndDt.Properties.AutoHeight = false;
            this.dEndDt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dEndDt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dEndDt.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dEndDt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dEndDt.Size = new System.Drawing.Size(88, 20);
            this.dEndDt.TabIndex = 113;
            // 
            // labelControl3
            // 
            this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Location = new System.Drawing.Point(510, 42);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(5, 13);
            this.labelControl3.TabIndex = 112;
            this.labelControl3.Text = "-";
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Location = new System.Drawing.Point(365, 42);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(51, 13);
            this.labelControl4.TabIndex = 111;
            this.labelControl4.Text = "Tanggal :";
            // 
            // dStartDt
            // 
            this.dStartDt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dStartDt.EditValue = null;
            this.dStartDt.Location = new System.Drawing.Point(419, 39);
            this.dStartDt.Name = "dStartDt";
            this.dStartDt.Properties.AutoHeight = false;
            this.dStartDt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dStartDt.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dStartDt.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dStartDt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dStartDt.Size = new System.Drawing.Size(88, 20);
            this.dStartDt.TabIndex = 110;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.Location = new System.Drawing.Point(706, 37);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(61, 23);
            this.btnDownload.TabIndex = 48;
            this.btnDownload.Text = "Unduh";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnLoadRanap
            // 
            this.btnLoadRanap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadRanap.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadRanap.Image")));
            this.btnLoadRanap.Location = new System.Drawing.Point(613, 37);
            this.btnLoadRanap.Name = "btnLoadRanap";
            this.btnLoadRanap.Size = new System.Drawing.Size(87, 23);
            this.btnLoadRanap.TabIndex = 47;
            this.btnLoadRanap.Text = "Muat Ulang";
            this.btnLoadRanap.Click += new System.EventHandler(this.btnLoadRanap_Click);
            // 
            // btnDelRanap
            // 
            this.btnDelRanap.Image = ((System.Drawing.Image)(resources.GetObject("btnDelRanap.Image")));
            this.btnDelRanap.Location = new System.Drawing.Point(197, 37);
            this.btnDelRanap.Name = "btnDelRanap";
            this.btnDelRanap.Size = new System.Drawing.Size(67, 23);
            this.btnDelRanap.TabIndex = 46;
            this.btnDelRanap.Text = "Hapus";
            this.btnDelRanap.Visible = false;
            // 
            // btnSaveRanap
            // 
            this.btnSaveRanap.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveRanap.Image")));
            this.btnSaveRanap.Location = new System.Drawing.Point(12, 37);
            this.btnSaveRanap.Name = "btnSaveRanap";
            this.btnSaveRanap.Size = new System.Drawing.Size(67, 23);
            this.btnSaveRanap.TabIndex = 45;
            this.btnSaveRanap.Text = "Simpan";
            this.btnSaveRanap.Click += new System.EventHandler(this.btnSaveRanap_Click);
            // 
            // btnAddRanap
            // 
            this.btnAddRanap.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRanap.Image")));
            this.btnAddRanap.Location = new System.Drawing.Point(124, 37);
            this.btnAddRanap.Name = "btnAddRanap";
            this.btnAddRanap.Size = new System.Drawing.Size(67, 23);
            this.btnAddRanap.TabIndex = 44;
            this.btnAddRanap.Text = "Tambah";
            this.btnAddRanap.Visible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(12, 8);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(141, 19);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "Rawat Inap Mngt";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 73);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(764, 389);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            // 
            // InpatientMngt
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 465);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "InpatientMngt";
            this.Text = "Rawat Inap Mngt";
            this.Load += new System.EventHandler(this.InpatientMngt_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dEndDt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dEndDt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dStartDt.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dStartDt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnDelRanap;
        private DevExpress.XtraEditors.SimpleButton btnSaveRanap;
        private DevExpress.XtraEditors.SimpleButton btnAddRanap;
        private DevExpress.XtraEditors.SimpleButton btnDownload;
        private DevExpress.XtraEditors.SimpleButton btnLoadRanap;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.DateEdit dStartDt;
        private DevExpress.XtraEditors.DateEdit dEndDt;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}