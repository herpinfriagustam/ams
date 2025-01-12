using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;

namespace Clinic
{
    public partial class DiagnosaInactive : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> diagnosaStatus = new List<FlagYn>();
        List<Diagnosa> listDiagnosa = new List<Diagnosa>();
        List<DiagnosaType> listDiagnosaType = new List<DiagnosaType>();
        List<Diagnosa> listDiagnosaAct = new List<Diagnosa>();

        DataTable dtGlDiag = new DataTable();
        DataTable dtGlDiagAct = new DataTable();

        public string v_empid = "", v_name = "";
        string item_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";

        public DiagnosaInactive()
        {
            InitializeComponent();
        }

        private void initData()
        {

            diagnosaStatus.Clear();
            diagnosaStatus.Add(new FlagYn() { flagCode = "", flagName = "" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "A", flagName = "Aktif" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "I", flagName = "Tidak Aktif" });

            listDiagnosaType.Clear();
            listDiagnosaType.Add(new DiagnosaType() { diagnosaTypeCode = "P", diagnosaTypeName = "Primary" });
            listDiagnosaType.Add(new DiagnosaType() { diagnosaTypeCode = "S", diagnosaTypeName = "Secondary" });

            dtGlDiag.Clear();
            string sql_poli = " select item_cd, initcap(item_name) item_name from KLINIK.cs_diagnosa_item where 1=1 order by item_name ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_poli, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            dtGlDiag = dt;
            listDiagnosa.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listDiagnosa.Add(new Diagnosa() { diagnosaCode = dt.Rows[i]["item_cd"].ToString(), diagnosaName = dt.Rows[i]["item_name"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            }

            dtGlDiagAct.Clear();
            string sql_diag = " select item_cd, initcap(item_name) item_name from KLINIK.cs_diagnosa_item where status='A' order by item_name ";
            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_diag, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);
            dtGlDiagAct = dt2;
            listDiagnosaAct.Clear();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                listDiagnosaAct.Add(new Diagnosa() { diagnosaCode = dt2.Rows[i]["item_cd"].ToString(), diagnosaName = dt2.Rows[i]["item_name"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            }

            gDiagAct.Properties.DataSource = listDiagnosaAct;
            gDiagAct.Properties.ValueMember = "diagnosaCode";
            gDiagAct.Properties.DisplayMember = "diagnosaName";
            gDiagAct.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            gDiagAct.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            gDiagAct.Properties.ImmediatePopup = true;
            gDiagAct.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            gDiagAct.Properties.NullText = "";

        }

        private void PrescriptionList_Load(object sender, EventArgs e)
        {
            initData();
            LoadInactiveDiag();

        }

        private void btnLoadKate_Click(object sender, EventArgs e)
        {
            initData();
            gDiagAct.Text = "";
            diagCd.Text = "-";
            LoadInactiveDiag();
            item_cd = "";
            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
        }

        private void LoadInactiveDiag()
        {
            string SQL;

            SQL = "";
            SQL = SQL + Environment.NewLine + "select item_cd, item_name, klinik.FN_GET_CATE_NAME(item_cd) cat_id, status, action, ";
            SQL = SQL + Environment.NewLine + "(select count(0) from KLINIK.cs_diagnosa where item_cd=a.item_cd) as cnt  ";
            SQL = SQL + Environment.NewLine + "from (  ";
            SQL = SQL + Environment.NewLine + "select item_cd, initcap(item_name) item_name, cat_id, status, 'S' as action ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_diagnosa_item ) a  ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "and item_cd in (select item_cd from KLINIK.cs_diagnosa ";
            SQL = SQL + Environment.NewLine + "where item_cd in (select item_cd  ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_diagnosa_item ";
            SQL = SQL + Environment.NewLine + "where status='I' )) ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                btnSaveItem.Enabled = false;

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 40;
                gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();

                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[4].Visible = false;

                gridView1.Columns[0].Caption = "Kode";
                gridView1.Columns[1].Caption = "Nama Diagnosa";
                gridView1.Columns[2].Caption = "Nama Kategori";
                gridView1.Columns[3].Caption = "Status";
                gridView1.Columns[4].Caption = "Action";
                gridView1.Columns[5].Caption = "Jumlah";
                gridView1.Columns[0].MinWidth = 50;
                gridView1.Columns[0].MaxWidth = 50;
                gridView1.Columns[3].MinWidth = 70;
                gridView1.Columns[3].MaxWidth = 70;
                gridView1.Columns[5].MinWidth = 50;
                gridView1.Columns[5].MaxWidth = 50;

                gridView1.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;
                gridView1.Columns[4].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.ReadOnly = true;

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = diagnosaStatus;
                statusLookup.ValueMember = "flagCode";
                statusLookup.DisplayMember = "flagName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = diagnosaStatus.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView1.Columns[3].ColumnEdit = statusLookup;
                gridView1.Columns[4].Visible = false;

                RepositoryItemMemoEdit namaDiag = new RepositoryItemMemoEdit();
                namaDiag.WordWrap = true;
                gridView1.Columns[1].ColumnEdit = namaDiag;

                gridView1.BestFitColumns();
                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                RepositoryItemMemoEdit namaKate = new RepositoryItemMemoEdit();
                namaKate.WordWrap = true;
                gridView1.Columns[2].ColumnEdit = namaKate;

                gridView1.BestFitColumns();
                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void LoadDataItem()
        {
            string SQL;

            SQL = "";
            SQL = SQL + Environment.NewLine + "select rm_no, to_char(insp_date,'yyyy-mm-dd') insp_date, visit_no, 'TT' || substr(rm_no,2,8) empid, ";
            SQL = SQL + Environment.NewLine + "klinik.FN_GET_NAME('TT' || substr(rm_no,2,8)) name, ";
            SQL = SQL + Environment.NewLine + "(select   ";
            SQL = SQL + Environment.NewLine + "'Tensi : ' || blood_press || ',' ||    ";
            SQL = SQL + Environment.NewLine + "'Nadi : ' || pulse || ',' ||         ";
            SQL = SQL + Environment.NewLine + "'Suhu : ' || temperature || ',' ||    ";
            SQL = SQL + Environment.NewLine + "'BB : ' || bb || ',' ||  ";
            SQL = SQL + Environment.NewLine + "'TB : ' || tb || ',' ||  ";
            SQL = SQL + Environment.NewLine + "'Alergi : ' || allergy || ',' ||       ";
            SQL = SQL + Environment.NewLine + "'Keluhan : ' || anamnesa || ',' ||   ";
            SQL = SQL + Environment.NewLine + "'R.Sekarang : ' || disease_now || ',' ||  ";
            SQL = SQL + Environment.NewLine + "'R.Dulu : ' || disease_then || ',' ||   ";
            SQL = SQL + Environment.NewLine + "'R.Kel : ' || disease_family || ',' ||   ";
            SQL = SQL + Environment.NewLine + "'Fisik : ' || anamnesa_physical || ',' ||   ";
            SQL = SQL + Environment.NewLine + "'Lain : ' || anamnesa_other  as anamnesa  ";
            SQL = SQL + Environment.NewLine + " from KLINIK.cs_anamnesa where insp_date=c.insp_date ";
            SQL = SQL + Environment.NewLine + " and visit_no=c.visit_no   ";
            SQL = SQL + Environment.NewLine + " and rm_no=c.rm_no) anamnesa,   ";
            SQL = SQL + Environment.NewLine + "'Obat : ' || (select LISTAGG(initcap(med_name)||'.'||formula||'.'||med_qty, ', ')  ";
            SQL = SQL + Environment.NewLine + " WITHIN GROUP (ORDER BY med_name asc) resep   ";
            SQL = SQL + Environment.NewLine + " from KLINIK.cs_receipt a    ";
            SQL = SQL + Environment.NewLine + " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)   ";
            SQL = SQL + Environment.NewLine + " where b.status = 'A'   ";
            SQL = SQL + Environment.NewLine + " and rm_no = c.rm_no    ";
            SQL = SQL + Environment.NewLine + " and insp_date = c.insp_date ";
            SQL = SQL + Environment.NewLine + " and visit_no = c.visit_no) medicine, ";
            SQL = SQL + Environment.NewLine + "klinik.FN_GET_CATE_NAME(item_cd) kategori, ";
            SQL = SQL + Environment.NewLine + "item_cd, type_diagnosa, remark, ";
            SQL = SQL + Environment.NewLine + "klinik.FN_GET_PIC(rm_no,insp_date,visit_no) pic, ";
            SQL = SQL + Environment.NewLine + "'S' action, item_cd as item_temp ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_diagnosa c ";
            SQL = SQL + Environment.NewLine + "where item_cd = '" + item_cd + "' ";


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl2.DataSource = null;
                gridView2.Columns.Clear();
                gridControl2.DataSource = dt;

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView2.OptionsView.ColumnAutoWidth = true;
                gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView2.IndicatorWidth = 40;
                gridView2.OptionsBehavior.Editable = true;
                gridView2.BestFitColumns();

                //gridView2.OptionsSelection.MultiSelect = true;
                //gridView2.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView2.VisibleColumns[0].Width = 20;
                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[4].Visible = false;

                gridView2.Columns[0].Caption = "RM No";
                gridView2.Columns[1].Caption = "Tanggal";
                gridView2.Columns[2].Caption = "No Antrian";
                gridView2.Columns[3].Caption = "Pasien No";
                gridView2.Columns[4].Caption = "Nama";
                gridView2.Columns[5].Caption = "Anamnesa";
                gridView2.Columns[6].Caption = "Terapi";
                gridView2.Columns[7].Caption = "Kategori";
                gridView2.Columns[8].Caption = "Diagnosa";
                gridView2.Columns[9].Caption = "Tipe Diagnosa";
                gridView2.Columns[10].Caption = "Remark";
                gridView2.Columns[11].Caption = "Pemeriksa";
                gridView2.Columns[12].Caption = "Action";
                gridView2.Columns[13].Caption = "temp";

                gridView2.Columns[0].MinWidth = 70;
                gridView2.Columns[1].MinWidth = 70;
                gridView2.Columns[2].MinWidth = 50;
                gridView2.Columns[3].MinWidth = 80;
                gridView2.Columns[4].MinWidth = 100;
                gridView2.Columns[5].MinWidth = 150;
                gridView2.Columns[6].MinWidth = 150;
                gridView2.Columns[7].MinWidth = 100;
                gridView2.Columns[8].MinWidth = 120;
                gridView2.Columns[9].MinWidth = 100;
                gridView2.Columns[10].MinWidth = 120;
                gridView2.Columns[11].MinWidth = 100;

                gridView2.Columns[0].Visible = false;
                gridView2.Columns[1].Visible = false;
                gridView2.Columns[2].Visible = false;
                gridView2.Columns[12].Visible = false;
                gridView2.Columns[13].Visible = false;

                RepositoryItemGridLookUpEdit gldiag = new RepositoryItemGridLookUpEdit();
                gldiag.DataSource = listDiagnosa;
                gldiag.ValueMember = "diagnosaCode";
                gldiag.DisplayMember = "diagnosaName";

                gldiag.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                gldiag.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                gldiag.ImmediatePopup = true;
                gldiag.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                gldiag.NullText = "";
                gridView2.Columns[8].ColumnEdit = gldiag;

                RepositoryItemLookUpEdit diagnosaTypeLookup = new RepositoryItemLookUpEdit();
                diagnosaTypeLookup.DataSource = listDiagnosaType;
                diagnosaTypeLookup.ValueMember = "diagnosaTypeCode";
                diagnosaTypeLookup.DisplayMember = "diagnosaTypeName";

                diagnosaTypeLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                diagnosaTypeLookup.DropDownRows = listDiagnosaType.Count;
                diagnosaTypeLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                diagnosaTypeLookup.AutoSearchColumnIndex = 1;
                diagnosaTypeLookup.NullText = "";
                gridView2.Columns[9].ColumnEdit = diagnosaTypeLookup;

                //gridView2.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[2].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[2].OptionsColumn.AllowEdit = false;
                gridView2.Columns[3].OptionsColumn.ReadOnly = true;
                gridView2.Columns[4].OptionsColumn.ReadOnly = true;
                gridView2.Columns[5].OptionsColumn.ReadOnly = true;
                gridView2.Columns[6].OptionsColumn.ReadOnly = true;
                gridView2.Columns[7].OptionsColumn.ReadOnly = true;
                gridView2.Columns[9].OptionsColumn.ReadOnly = true;
                gridView2.Columns[10].OptionsColumn.ReadOnly = true;
                gridView2.Columns[11].OptionsColumn.ReadOnly = true;
                gridView2.Columns[12].OptionsColumn.ReadOnly = true;
                gridView2.Columns[13].OptionsColumn.ReadOnly = true;

                gridView2.BestFitColumns();

                RepositoryItemMemoEdit anam = new RepositoryItemMemoEdit();
                anam.WordWrap = true;
                gridView2.Columns[5].ColumnEdit = anam;

                gridView2.Columns[5].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                RepositoryItemMemoEdit med = new RepositoryItemMemoEdit();
                med.WordWrap = true;
                gridView2.Columns[6].ColumnEdit = med;

                gridView2.Columns[6].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                RepositoryItemMemoEdit kate = new RepositoryItemMemoEdit();
                kate.WordWrap = true;
                gridView2.Columns[7].ColumnEdit = kate;

                gridView2.Columns[7].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                if (dt.Rows.Count > 0)
                {
                    gDiagAct.Enabled = true;
                }
                else
                {
                    gDiagAct.Enabled = false;
                }
                //LoadDiagInactive();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            //if (e.Column.Caption == "Nama Kategori" || e.Column.Caption == "Status")
            //{
            //    string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
            //    if (tmp_stat == "I")
            //    {
            //        view.SetRowCellValue(e.RowHandle, view.Columns[3], "I");
            //    }
            //    else
            //    {
            //        view.SetRowCellValue(e.RowHandle, view.Columns[3], "U");
            //    }
            //}
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            //view.SetRowCellValue(e.RowHandle, view.Columns[3], "I");
        }

        private void btnLoadItem_Click(object sender, EventArgs e)
        {
            initData();
            LoadDataItem();
            btnSaveItem.Enabled = false;
        }

        private void gridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[12], "I");
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveItem.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Diagnosa")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[12]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], "U");
                }
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            gridView2.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView2.Columns[0].OptionsColumn.AllowEdit = true;
            gridView2.AddNewRow();
        }

        private void btnSaveItem_Click(object sender, EventArgs e)
        {
            string sql_update = "", p_rm="", p_date="", p_visit_no="", p_type="";
            string p_kode = "", p_diag = "", p_kate = "", p_status = "", p_action = "", p_temp="";

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                p_rm = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();
                p_date = gridView2.GetRowCellValue(i, gridView2.Columns[1]).ToString();
                p_visit_no = gridView2.GetRowCellValue(i, gridView2.Columns[2]).ToString();
                p_diag = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                p_type = gridView2.GetRowCellValue(i, gridView2.Columns[9]).ToString();
                p_action = gridView2.GetRowCellValue(i, gridView2.Columns[12]).ToString();
                p_temp = gridView2.GetRowCellValue(i, gridView2.Columns[13]).ToString();

                if (p_diag == "")
                {
                    MessageBox.Show("Diagnosa harus diisi");
                }
                else
                {
                    if (p_action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update KLINIK.cs_diagnosa set item_cd = '" + p_diag + "', ";
                        sql_update = sql_update + " noted = 'old:' || '" + p_temp + "' || ', new:' || '" + p_diag + "' || ', by:' || '" + v_empid + "' || ' ' || to_char(sysdate,'yyyy-mm-dd hh24:mi:ss') ";
                        sql_update = sql_update + " where rm_no = '" + p_rm + "' and insp_date = to_date('" + p_date + "','yyyy-mm-dd') and visit_no = '" + p_visit_no + "' and type_diagnosa = '" + p_type + "' ";

                        try
                        {
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm2 = new OleDbCommand(sql_update, oraConnect2);
                            oraConnect2.Open();
                            cm2.ExecuteNonQuery();
                            oraConnect2.Close();
                            cm2.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);
                            //LoadDataKate();
                            MessageBox.Show("Data Berhasil dirubah");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            LoadDataItem();
        }

        private void gridView1_RowClick_1(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;

            item_cd = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            initData();
            gDiagAct.Text = "";
            diagCd.Text = "-";
            LoadDataItem();
            item_cd = "";
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            //if (e.Column.Caption == "Nama Kategori" || e.Column.Caption == "Status")
            //{
            //    e.Appearance.BackColor = Color.OldLace;
            //    e.Appearance.ForeColor = Color.Black;
            //}
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Diagnosa")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "diagnosa_inactive.xls",
                    RestoreDirectory = true,
                    CheckFileExists = false,
                    CheckPathExists = true,
                    OverwritePrompt = true,
                    DereferenceLinks = true,
                    ValidateNames = true,
                    AddExtension = false,
                    FilterIndex = 1
                };
                saveDialog.InitialDirectory = "C:\\";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl1.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }

        private void btnDownload2_Click(object sender, EventArgs e)
        {
            if (gridView2.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "diagnosa_medical_record_inactive.xls",
                    RestoreDirectory = true,
                    CheckFileExists = false,
                    CheckPathExists = true,
                    OverwritePrompt = true,
                    DereferenceLinks = true,
                    ValidateNames = true,
                    AddExtension = false,
                    FilterIndex = 1
                };
                saveDialog.InitialDirectory = "C:\\";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    gridControl2.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }

        private void gDiagAct_EditValueChanged(object sender, EventArgs e)
        {
            GridLookUpEdit lookUp = sender as GridLookUpEdit;

            diagCd.Text = lookUp.EditValue.ToString();

            // Access the currently selected data row
            //DataRowView dataRow = lookUp.GetSelectedDataRow() as DataRowView;
            DataRowView row = lookUp.Properties.GetRowByKeyValue(lookUp.EditValue) as DataRowView;
            // Assign the row's Picture field value to the PictureEdit control
            if (row != null)
            {
                diagCd.Text = row[0].ToString();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (gDiagAct.Text == "")
            {
                MessageBox.Show("Pilih Diagnosa");
            }
            else if (gridView2.RowCount > 0)
            {
                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    gridView2.SetRowCellValue(i, gridView2.Columns[8], diagCd.Text);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }

        private void gridView1_CustomDrawRowIndicator_1(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        
    }
}