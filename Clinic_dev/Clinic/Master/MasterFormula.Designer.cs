namespace Clinic
{
    partial class MasterFormula
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterFormula));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdAlkes = new System.Windows.Forms.RadioButton();
            this.rdObat = new System.Windows.Forms.RadioButton();
            this.btnDelDosis = new DevExpress.XtraEditors.SimpleButton();
            this.btnDownload = new DevExpress.XtraEditors.SimpleButton();
            this.btnLoadDosis = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveDosis = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddDosis = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(936, 416);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
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
            this.panel1.Size = new System.Drawing.Size(936, 70);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdAlkes);
            this.groupBox1.Controls.Add(this.rdObat);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(297, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 48);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pilihan";
            // 
            // rdAlkes
            // 
            this.rdAlkes.AutoSize = true;
            this.rdAlkes.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.rdAlkes.Location = new System.Drawing.Point(103, 20);
            this.rdAlkes.Name = "rdAlkes";
            this.rdAlkes.Size = new System.Drawing.Size(59, 17);
            this.rdAlkes.TabIndex = 1;
            this.rdAlkes.Text = "ALKES";
            this.rdAlkes.UseVisualStyleBackColor = true;
            // 
            // rdObat
            // 
            this.rdObat.AutoSize = true;
            this.rdObat.Checked = true;
            this.rdObat.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.rdObat.Location = new System.Drawing.Point(32, 20);
            this.rdObat.Name = "rdObat";
            this.rdObat.Size = new System.Drawing.Size(55, 17);
            this.rdObat.TabIndex = 0;
            this.rdObat.TabStop = true;
            this.rdObat.Text = "OBAT";
            this.rdObat.UseVisualStyleBackColor = true;
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
            this.btnDownload.Location = new System.Drawing.Point(872, 37);
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
            this.btnLoadDosis.Location = new System.Drawing.Point(779, 37);
            this.btnLoadDosis.Name = "btnLoadDosis";
            this.btnLoadDosis.Size = new System.Drawing.Size(87, 23);
            this.btnLoadDosis.TabIndex = 41;
            this.btnLoadDosis.Text = "Muat Ulang";
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
            this.labelControl1.Size = new System.Drawing.Size(87, 19);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "Dosis Obat";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 73);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(930, 340);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 45;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            this.gridView1.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView1_InitNewRow);
            this.gridView1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView1_CellValueChanged);
            // 
            // MasterFormula
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 416);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MasterFormula";
            this.Text = "Master Dosis";
            this.Load += new System.EventHandler(this.MasterFormula_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdAlkes;
        private System.Windows.Forms.RadioButton rdObat;
    }
}