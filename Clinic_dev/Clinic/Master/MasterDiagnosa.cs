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
    public partial class MasterDiagnosa : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Diagnosa> listDiagnosaKate = new List<Diagnosa>();
        List<FlagYn> diagnosaStatus = new List<FlagYn>();

        public string  v_name = "";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";

        public MasterDiagnosa()
        {
            InitializeComponent();
        }

        private void initData()
        {

            string sql_poli = " select cat_id, initcap(cat_name) cat_name from cs_diagnosa_category where status = 'A' order by cat_name ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_poli, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            listDiagnosaKate.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listDiagnosaKate.Add(new Diagnosa() { diagnosaCode = dt.Rows[i]["cat_id"].ToString(), diagnosaName = dt.Rows[i]["cat_name"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            }

            diagnosaStatus.Clear();
            diagnosaStatus.Add(new FlagYn() { flagCode = "", flagName = "" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "A", flagName = "Aktif" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "I", flagName = "Tidak Aktif" });

            luStatus.Properties.DataSource = diagnosaStatus;
            luStatus.Properties.ValueMember = "flagCode";
            luStatus.Properties.DisplayMember = "flagName";

            luStatus.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luStatus.Properties.DropDownRows = diagnosaStatus.Count;
            luStatus.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luStatus.Properties.AutoSearchColumnIndex = 1;
            luStatus.Properties.NullText = "";
            luStatus.ItemIndex = 0;

            luStat2.Properties.DataSource = diagnosaStatus;
            luStat2.Properties.ValueMember = "flagCode";
            luStat2.Properties.DisplayMember = "flagName";

            luStat2.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luStat2.Properties.DropDownRows = diagnosaStatus.Count;
            luStat2.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luStat2.Properties.AutoSearchColumnIndex = 1;
            luStat2.Properties.NullText = "";
            luStat2.ItemIndex = 0;

            tableLayoutPanel1.RowStyles[2] = new RowStyle(SizeType.Absolute, 0);
            tableLayoutPanel1.RowStyles[3] = new RowStyle(SizeType.Absolute, 0);
        }

        private void PrescriptionList_Load(object sender, EventArgs e)
        {
            initData();
            LoadDataKate();
            LoadDataItem();

        }

        private void btnLoadKate_Click(object sender, EventArgs e)
        {
            initData();
            LoadDataKate();
            btnSaveKate.Enabled = false;
            kate_cd = "";
        }

        private void LoadDataKate()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select cat_id, cat_name, status, action from ( ";
            sql_search = sql_search + Environment.NewLine + "select cat_id, initcap(cat_name) cat_name, status, 'S' as action ";
            sql_search = sql_search + Environment.NewLine + "from cs_diagnosa_category ) a where 1=1 ";
            if (luStatus.Text == "Aktif") { stat = "A"; } else if (luStatus.Text == "Tidak Aktif") { stat = "I"; }
            sql_search = sql_search + Environment.NewLine + "and status like '" + stat + "%' ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
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
                gridView1.OptionsBehavior.Editable = true;
                gridView1.BestFitColumns();

                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[4].Visible = false;

                gridView1.Columns[0].Caption = "Kode";
                gridView1.Columns[1].Caption = "Nama Kategori";
                gridView1.Columns[2].Caption = "Status";
                gridView1.Columns[3].Caption = "Action";
                gridView1.Columns[0].MinWidth = 50;
                gridView1.Columns[0].MaxWidth = 50;
                gridView1.Columns[2].MinWidth = 50;
                gridView1.Columns[2].MaxWidth = 50;

                gridView1.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = diagnosaStatus;
                statusLookup.ValueMember = "flagCode";
                statusLookup.DisplayMember = "flagName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = diagnosaStatus.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView1.Columns[2].ColumnEdit = statusLookup;
                gridView1.Columns[3].Visible = false;

                RepositoryItemMemoEdit namaKate = new RepositoryItemMemoEdit();
                namaKate.WordWrap = true;
                gridView1.Columns[1].ColumnEdit = namaKate;

                gridView1.BestFitColumns();
                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

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
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select item_cd, item_name, cat_id, status, action ";
            //sql_search = sql_search + Environment.NewLine + "(select count(0) from cs_diagnosa where item_cd=a.item_cd) as cnt ";
            sql_search = sql_search + Environment.NewLine + "from ( ";
            sql_search = sql_search + Environment.NewLine + "select item_cd, initcap(item_name) item_name, cat_id, status, 'S' as action ";
            sql_search = sql_search + Environment.NewLine + "from cs_diagnosa_item ) a where 1=1 ";
            if (luStatus.Text == "Aktif") { stat = "A"; } else if (luStat2.Text == "Tidak Aktif") { stat = "I"; }
            sql_search = sql_search + Environment.NewLine + "and status like '" + stat + "%' ";
            sql_search = sql_search + Environment.NewLine + "and cat_id like '" + kate_cd + "%' ";
            sql_search = sql_search + Environment.NewLine + "order by item_name asc ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl2.DataSource = null;
                gridView2.Columns.Clear();
                gridControl2.DataSource = dt;

                btnSaveKate.Enabled = false;

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

                gridView2.Columns[0].Caption = "Kode";
                gridView2.Columns[1].Caption = "Diagnosa";
                gridView2.Columns[2].Caption = "Kategori";
                gridView2.Columns[3].Caption = "Status";
                gridView2.Columns[4].Caption = "Action";
                //gridView2.Columns[5].Caption = "Jumlah";

                gridView2.Columns[0].MinWidth = 50;
                gridView2.Columns[1].MinWidth = 150;
                gridView2.Columns[2].MinWidth = 180;
                gridView2.Columns[3].MinWidth = 60;
                //gridView2.Columns[5].MinWidth = 60;

                gridView2.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView2.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[2].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[2].OptionsColumn.AllowEdit = false;
                gridView2.Columns[4].OptionsColumn.ReadOnly = true;

                RepositoryItemLookUpEdit kateLookup = new RepositoryItemLookUpEdit();
                kateLookup.DataSource = listDiagnosaKate;
                kateLookup.ValueMember = "diagnosaCode";
                kateLookup.DisplayMember = "diagnosaName";

                kateLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                kateLookup.DropDownRows = listDiagnosaKate.Count;
                kateLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                kateLookup.AutoSearchColumnIndex = 1;
                kateLookup.NullText = "";
                gridView2.Columns[2].ColumnEdit = kateLookup;

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = diagnosaStatus;
                statusLookup.ValueMember = "flagCode";
                statusLookup.DisplayMember = "flagName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = diagnosaStatus.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView2.Columns[3].ColumnEdit = statusLookup;

                gridView2.Columns[4].Visible = false;

                gridView2.BestFitColumns();

                RepositoryItemMemoEdit namaDiag = new RepositoryItemMemoEdit();
                namaDiag.WordWrap = true;
                gridView2.Columns[1].ColumnEdit = namaDiag;

                gridView2.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                LoadDiagInactive();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void LoadDiagInactive()
        {
            string SQL="", SQL2 = "";

            SQL = SQL + Environment.NewLine + "select b.cat_id, cat_name, a.status, action, ";
            SQL = SQL + Environment.NewLine + "(select count(0) from cs_diagnosa_category where cat_id=b.cat_id) as cnt  ";
            SQL = SQL + Environment.NewLine + "from (  ";
            SQL = SQL + Environment.NewLine + "select item_cd, initcap(item_name) item_name, cat_id, status, 'S' as action ";
            SQL = SQL + Environment.NewLine + "from cs_diagnosa_item ) a  ";
            SQL = SQL + Environment.NewLine + "join cs_diagnosa_category b on (a.cat_id=b.cat_id) ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "and item_cd in (select item_cd from cs_diagnosa ";
            SQL = SQL + Environment.NewLine + "where item_cd in (select item_cd  ";
            SQL = SQL + Environment.NewLine + "from cs_diagnosa_item ";
            SQL = SQL + Environment.NewLine + "where status='I' )) ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl3.DataSource = null;
                gridView3.Columns.Clear();
                gridControl3.DataSource = dt;

                btnSaveItem.Enabled = false;

                //gridView3.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView3.OptionsView.ColumnAutoWidth = true;
                gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView3.IndicatorWidth = 40;
                gridView3.OptionsBehavior.Editable = false;
                gridView3.BestFitColumns();

                //gridView3.OptionsSelection.MultiSelect = true;
                //gridView3.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView3.VisibleColumns[0].Width = 20;
                //gridView3.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView3.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView3.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView3.Columns[4].Visible = false;

                gridView3.Columns[0].Caption = "Kode";
                gridView3.Columns[1].Caption = "Nama Kategori";
                gridView3.Columns[2].Caption = "Status";
                gridView3.Columns[3].Caption = "Action";
                gridView3.Columns[4].Caption = "Jumlah";
                gridView3.Columns[0].MinWidth = 50;
                gridView3.Columns[0].MaxWidth = 50;
                gridView3.Columns[2].MinWidth = 50;
                gridView3.Columns[2].MaxWidth = 50;

                gridView3.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView3.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView3.Columns[2].OptionsColumn.ReadOnly = true;
                gridView3.Columns[3].OptionsColumn.ReadOnly = true;
                gridView3.Columns[4].OptionsColumn.ReadOnly = true;

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = diagnosaStatus;
                statusLookup.ValueMember = "flagCode";
                statusLookup.DisplayMember = "flagName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = diagnosaStatus.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView3.Columns[2].ColumnEdit = statusLookup;
                gridView3.Columns[3].Visible = false;

                RepositoryItemMemoEdit namaKate = new RepositoryItemMemoEdit();
                namaKate.WordWrap = true;
                gridView3.Columns[1].ColumnEdit = namaKate;

                gridView3.BestFitColumns();
                gridView3.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }


            SQL2 = SQL2 + Environment.NewLine + "select item_cd, item_name, cat_id, status, action, ";
            SQL2 = SQL2 + Environment.NewLine + "(select count(0) from cs_diagnosa where item_cd=a.item_cd) as cnt  ";
            SQL2 = SQL2 + Environment.NewLine + "from (  ";
            SQL2 = SQL2 + Environment.NewLine + "select item_cd, initcap(item_name) item_name, cat_id, status, 'S' as action ";
            SQL2 = SQL2 + Environment.NewLine + "from cs_diagnosa_item ) a  ";
            SQL2 = SQL2 + Environment.NewLine + "where 1=1  ";
            SQL2 = SQL2 + Environment.NewLine + "and item_cd in (select item_cd from cs_diagnosa ";
            SQL2 = SQL2 + Environment.NewLine + "where item_cd in (select item_cd  ";
            SQL2 = SQL2 + Environment.NewLine + "from cs_diagnosa_item ";
            SQL2 = SQL2 + Environment.NewLine + "where status='I' )) ";


            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql2 = new OleDbDataAdapter(SQL2, sqlConnect2);
                DataTable dt2 = new DataTable();
                adSql2.Fill(dt2);

                gridControl4.DataSource = null;
                gridView4.Columns.Clear();
                gridControl4.DataSource = dt2;

                btnSaveKate.Enabled = false;

                //gridView4.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView4.OptionsView.ColumnAutoWidth = true;
                gridView4.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView4.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView4.IndicatorWidth = 40;
                gridView4.OptionsBehavior.Editable = false;
                gridView4.BestFitColumns();

                //gridView4.OptionsSelection.MultiSelect = true;
                //gridView4.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView4.VisibleColumns[0].Width = 20;
                //gridView4.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView4.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView4.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView4.Columns[4].Visible = false;

                gridView4.Columns[0].Caption = "Kode";
                gridView4.Columns[1].Caption = "Diagnosa";
                gridView4.Columns[2].Caption = "Kategori";
                gridView4.Columns[3].Caption = "Status";
                gridView4.Columns[4].Caption = "Action";
                gridView4.Columns[5].Caption = "Jumlah";

                gridView4.Columns[0].MinWidth = 50;
                gridView4.Columns[1].MinWidth = 150;
                gridView4.Columns[2].MinWidth = 180;
                gridView4.Columns[3].MinWidth = 60;
                gridView4.Columns[5].MinWidth = 60;

                gridView4.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView4.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView4.Columns[2].OptionsColumn.ReadOnly = true;
                //gridView4.Columns[2].OptionsColumn.AllowEdit = false;
                gridView4.Columns[4].OptionsColumn.ReadOnly = true;
                gridView4.Columns[5].OptionsColumn.ReadOnly = true;

                RepositoryItemLookUpEdit kateLookup = new RepositoryItemLookUpEdit();
                kateLookup.DataSource = listDiagnosaKate;
                kateLookup.ValueMember = "diagnosaCode";
                kateLookup.DisplayMember = "diagnosaName";

                kateLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                kateLookup.DropDownRows = listDiagnosaKate.Count;
                kateLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                kateLookup.AutoSearchColumnIndex = 1;
                kateLookup.NullText = "";
                gridView4.Columns[2].ColumnEdit = kateLookup;

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = diagnosaStatus;
                statusLookup.ValueMember = "flagCode";
                statusLookup.DisplayMember = "flagName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = diagnosaStatus.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView4.Columns[3].ColumnEdit = statusLookup;

                gridView4.Columns[4].Visible = false;

                gridView4.BestFitColumns();

                RepositoryItemMemoEdit namaDiag = new RepositoryItemMemoEdit();
                namaDiag.WordWrap = true;
                gridView4.Columns[1].ColumnEdit = namaDiag;

                gridView4.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                if (dt2.Rows.Count >0 )
                {
                    tableLayoutPanel1.RowStyles[2] = new RowStyle(SizeType.Absolute, 40);
                    tableLayoutPanel1.RowStyles[3] = new RowStyle(SizeType.Absolute, 100);
                }
                else
                {
                    tableLayoutPanel1.RowStyles[2] = new RowStyle(SizeType.Absolute, 0);
                    tableLayoutPanel1.RowStyles[3] = new RowStyle(SizeType.Absolute, 0);
                }

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            //GridView View = sender as GridView;
            //string s_nik = "", s_que = "", s_date = "", sql_his = "", s_check="", s_cnt="", s_nama = "";

            
            //s_nik = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
           
        }

        private void btnSaveKate_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "", sql_cnt = "", sequ = "";
            string p_kode = "", p_nama = "", p_status = "", p_action = "";

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                p_kode = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                p_nama = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                p_status = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                p_action = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();

                if (p_nama == "")
                {
                    MessageBox.Show("Kategori harus diisi");
                }
                else if (p_status == "")
                {
                    MessageBox.Show("Status harus diisi");
                }
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_diagnosa_category (cat_id, cat_name, status, ins_date, ins_emp) values ";
                        sql_insert = sql_insert + " ('CAT' || lpad(cs_diag_cat_seq.nextval,3,'0'), '" + p_nama + "', 'A', sysdate, '" + DB.vUserId + "') ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);
                            //LoadDataKate();
                            MessageBox.Show("Data Berhasil ditambah");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                    else if (p_action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update cs_diagnosa_category set cat_name = '" + p_nama + "', status = '" + p_status + "' ";
                        sql_update = sql_update + " where cat_id = '" + p_kode + "' ";

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
            LoadDataKate();
        }

        private void btnAddKate_Click(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView1.AddNewRow();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveKate.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Nama Kategori" || e.Column.Caption == "Status")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[3], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[3], "U");
                }
            }
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[3], "I");
        }

        private void btnLoadItem_Click(object sender, EventArgs e)
        {
            initData();
            LoadDataItem();
            btnSaveItem.Enabled = false;
            kate_cd = "";
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

            view.SetRowCellValue(e.RowHandle, view.Columns[4], "I");
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveItem.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Diagnosa" || e.Column.Caption == "Kategori" || e.Column.Caption == "Status")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], "U");
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
            string sql_insert = "", sql_update = "", sql_cnt = "";
            string p_kode = "", p_diag = "", p_kate = "", p_status = "", p_action = "";

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                p_kode = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();
                p_diag = gridView2.GetRowCellValue(i, gridView2.Columns[1]).ToString();
                p_kate = gridView2.GetRowCellValue(i, gridView2.Columns[2]).ToString();
                p_status = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                p_action = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();

                if (p_kode == "")
                {
                    MessageBox.Show("ICD harus diisi");
                }
                else if (p_diag == "")
                {
                    MessageBox.Show("Diagnosa harus diisi");
                }
                else if (p_kate == "")
                {
                    MessageBox.Show("Kategori harus diisi");
                }
                else if (p_status == "")
                {
                    MessageBox.Show("Status harus diisi");
                }
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_diagnosa_item (item_cd, item_name, status, ins_date, ins_emp, cat_id) values ";
                        sql_insert = sql_insert + " ('" + p_kode + "', '" + p_diag + "', 'A', sysdate, '" + DB.vUserId + "','" + p_kate + "') ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);
                            //LoadDataKate();
                            MessageBox.Show("Data Berhasil ditambah");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                    else if (p_action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update cs_diagnosa_item set item_name = '" + p_diag + "', status = '" + p_status + "', cat_id = '" + p_kate + "' ";
                        sql_update = sql_update + " where item_cd = '" + p_kode + "' ";

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

            kate_cd = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            LoadDataItem();
            kate_cd = "";
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Kategori" || e.Column.Caption == "Status")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Diagnosa" || e.Column.Caption == "Kategori" || e.Column.Caption == "Status")
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
                    FileName = "diagnosa_kategori.xls",
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
                    FileName = "diagnosa.xls",
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

        private void gridView1_CustomDrawRowIndicator_1(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView3_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView4_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}