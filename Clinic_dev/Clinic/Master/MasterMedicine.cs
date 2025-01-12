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
using System.Diagnostics;

namespace Clinic
{
    public partial class MasterMedicine : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Medicine> listMedicine = new List<Medicine>();
        List<MedicineGroup> medicineGroup = new List<MedicineGroup>();
        List<FlagYn> medicineStatus = new List<FlagYn>();
        List<Status> outStatus = new List<Status>();
        List<Medicine> listMedUom = new List<Medicine>();
        List<Medicine> listMedBpjs = new List<Medicine>();

        public string   v_name = "";
        string med_cd = "", med_nm = "", trx_dt = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";

        public MasterMedicine()
        {
            InitializeComponent();
        }

        private void initData()
        {

            string sql_med = " select med_cd, initcap(med_name) med_name from cs_medicine where status = 'A' order by med_name ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_med, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            listMedicine.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listMedicine.Add(new Medicine() { medicineCode = dt.Rows[i]["med_cd"].ToString(), medicineName = dt.Rows[i]["med_name"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            }

            medicineGroup.Clear();
            medicineGroup.Add(new MedicineGroup() { mgroup = "OBAT", group = "OBAT" });
            medicineGroup.Add(new MedicineGroup() { mgroup = "ALKES", group = "ALKES" });
            medicineGroup.Add(new MedicineGroup() { mgroup = "OTC", group = "OTC" });

            medicineStatus.Clear();
            medicineStatus.Add(new FlagYn() { flagCode = "", flagName = "" });
            medicineStatus.Add(new FlagYn() { flagCode = "A", flagName = "Aktif" });
            medicineStatus.Add(new FlagYn() { flagCode = "I", flagName = "Tidak Aktif" });

            luStatus.Properties.DataSource = medicineStatus;
            luStatus.Properties.ValueMember = "flagCode";
            luStatus.Properties.DisplayMember = "flagName";

            luStatus.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luStatus.Properties.DropDownRows = medicineStatus.Count;
            luStatus.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luStatus.Properties.AutoSearchColumnIndex = 1;
            luStatus.Properties.NullText = "";
            luStatus.ItemIndex = 0;

            lTransIn.Text = "Transaksi Masuk";
            lTransOut.Text = "Transaksi Keluar";

            outStatus.Clear();
            outStatus.Add(new Status() { statusCode = "RTN", statusName = "Return" });
            outStatus.Add(new Status() { statusCode = "EXP", statusName = "Expire" });
            outStatus.Add(new Status() { statusCode = "MAN", statusName = "Manual" });
            outStatus.Add(new Status() { statusCode = "ADJ", statusName = "Adjust" });

            string sql_med_uom = " select code_id, initcap(code_name) code_name from cs_code_data where code_class_id ='MED_UOM'  and status = 'A' order by sort_order ";
            OleDbConnection sqlConnectUom = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSqlUom = new OleDbDataAdapter(sql_med_uom, sqlConnectUom);
            DataTable dtUom = new DataTable();
            adSqlUom.Fill(dtUom);
            listMedUom.Clear();
            for (int i = 0; i < dtUom.Rows.Count; i++)
            {
                listMedUom.Add(new Medicine() { medicineCode = dtUom.Rows[i]["code_id"].ToString(), medicineName = dtUom.Rows[i]["code_name"].ToString() });

            }

            string sql_bpjs_cov = " select code_id, initcap(code_name) code_name from cs_code_data where code_class_id ='YES_NO'  and status = 'A' order by sort_order ";
            OleDbConnection sqlConnectBpjs = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSqlBpjs = new OleDbDataAdapter(sql_bpjs_cov, sqlConnectBpjs);
            DataTable dtBpjs = new DataTable();
            adSqlBpjs.Fill(dtBpjs);
            listMedBpjs.Clear();
            for (int i = 0; i < dtBpjs.Rows.Count; i++)
            {
                listMedBpjs.Add(new Medicine() { medicineCode = dtBpjs.Rows[i]["code_id"].ToString(), medicineName = dtBpjs.Rows[i]["code_name"].ToString() });

            }

            tableLayoutPanel3.RowStyles[1] = new RowStyle(SizeType.Absolute, 0);
            tableLayoutPanel3.RowStyles[2] = new RowStyle(SizeType.Absolute, 0);
        }

        private void PrescriptionList_Load(object sender, EventArgs e)
        {
            initData();
            LoadDataMaster();
            LoadDataLimit();
            SoftBlink(labelControl4, Color.LightPink, Color.Red, 1600, false);
        }

        private void btnLoadKate_Click(object sender, EventArgs e)
        {
            //initData();
            LoadDataMaster();
            LoadDataLimit();
            btnSaveKate.Enabled = false;
            btnAddItem.Enabled = false;
            btnSaveItem.Enabled = false;
            btnAddOut.Enabled = false;
            btnSaveOut.Enabled = false;
            gridControl2.DataSource = null;
            gridControl3.DataSource = null;
            med_cd = "";
            med_nm = "";
            trx_dt = "";
            lTransIn.Text = "Transaksi Masuk ";
            lTransOut.Text = "Transaksi Keluar ";
        }

        private void LoadDataMaster()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select med_cd, med_name, uom, LIMIT_STOCK, status, stok_awal, stok_in, stok_out, stok, action, bpjs_cover,med_group from (  ";
            sql_search = sql_search + Environment.NewLine + "select med_cd, initcap(med_name) med_name, uom, status, LIMIT_STOCK, ";
            sql_search = sql_search + Environment.NewLine + "FN_CS_INIT_STOCK(sysdate,med_cd) as stok_awal, ";
            sql_search = sql_search + Environment.NewLine + "FN_CS_TRX_IN(sysdate,med_cd) as stok_in, ";
            sql_search = sql_search + Environment.NewLine + "FN_CS_TRX_OUT(sysdate,med_cd) as stok_out, ";
            sql_search = sql_search + Environment.NewLine + "FN_CS_INIT_STOCK(sysdate,med_cd) + ";
            sql_search = sql_search + Environment.NewLine + "FN_CS_TRX_IN(sysdate,med_cd) -  ";
            sql_search = sql_search + Environment.NewLine + "FN_CS_TRX_OUT(sysdate,med_cd) -  ";
            sql_search = sql_search + Environment.NewLine + "FN_CS_REQ_STOCK(sysdate,med_cd) as stok, ";
            sql_search = sql_search + Environment.NewLine + "'S' as action, ";
            sql_search = sql_search + Environment.NewLine + "bpjs_cover ,med_group ";
            sql_search = sql_search + Environment.NewLine + "from cs_medicine ";
            sql_search = sql_search + Environment.NewLine + "where 1=1 ";
            if (luStatus.Text == "Aktif") { stat = "A"; } else if (luStatus.Text == "Tidak Aktif") { stat = "I"; }
            sql_search = sql_search + Environment.NewLine + "and status like '" + stat + "%' ";
            sql_search = sql_search + Environment.NewLine + ") a where 1=1  ";
            sql_search = sql_search + Environment.NewLine + "order by med_name  ";

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
                btnSaveOut.Enabled = false;
                btnAddItem.Enabled = false;
                btnAddOut.Enabled = false;

                //////gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 40;
                ////gridView1.OptionsBehavior.Editable = true;
                gridView1.BestFitColumns();

                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                gridView1.VisibleColumns[0].Width = 20;
                gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
                gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                gridView1.Columns[4].Visible = false;

                gridView1.Columns[0].Caption = "Kode";
                gridView1.Columns[1].Caption = "Nama Obat";
                gridView1.Columns[2].Caption = "Satuan";
                gridView1.Columns[3].Caption = "Limit Stock";
                gridView1.Columns[4].Caption = "Status";
                gridView1.Columns[5].Caption = "Stok Awal";
                gridView1.Columns[6].Caption = "In";
                gridView1.Columns[7].Caption = "Out";
                gridView1.Columns[8].Caption = "Stok Saat Ini";
                gridView1.Columns[9].Caption = "Action";
                gridView1.Columns[10].Caption = "BPJS";
                gridView1.Columns[11].Caption = "Group";

                gridView1.Columns[0].MinWidth = 60;
                gridView1.Columns[0].MaxWidth = 60;
                gridView1.Columns[2].MinWidth = 60;
                gridView1.Columns[2].MaxWidth = 60;
                gridView1.Columns[3].MinWidth = 80;
                gridView1.Columns[3].MaxWidth = 80;
                gridView1.Columns[4].MinWidth = 70;
                gridView1.Columns[4].MaxWidth = 70;
                gridView1.Columns[5].MinWidth = 80;
                gridView1.Columns[5].MaxWidth = 80;
                gridView1.Columns[6].MinWidth = 50;
                gridView1.Columns[6].MaxWidth = 50;
                gridView1.Columns[7].MinWidth = 60;
                gridView1.Columns[7].MaxWidth = 60;
                gridView1.Columns[8].MinWidth = 90;
                gridView1.Columns[8].MaxWidth = 90;
                gridView1.Columns[9].MinWidth = 50;
                gridView1.Columns[9].MaxWidth = 50;
                gridView1.Columns[10].MinWidth = 50;
                gridView1.Columns[10].MaxWidth = 50;
                gridView1.Columns[11].MinWidth = 60;
                gridView1.Columns[11].MaxWidth = 60;

                gridView1.Columns[0].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.AllowEdit = false;
                gridView1.Columns[6].OptionsColumn.AllowEdit = false;
                gridView1.Columns[7].OptionsColumn.AllowEdit = false;
                gridView1.Columns[8].OptionsColumn.AllowEdit = false;
                gridView1.Columns[9].OptionsColumn.AllowEdit = false; 

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = medicineStatus;
                statusLookup.ValueMember = "flagCode";
                statusLookup.DisplayMember = "flagName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = medicineStatus.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView1.Columns[4].ColumnEdit = statusLookup;
                gridView1.Columns[9].Visible = false;

                RepositoryItemLookUpEdit uomLookup = new RepositoryItemLookUpEdit();
                uomLookup.DataSource = listMedUom;
                uomLookup.ValueMember = "medicineCode";
                uomLookup.DisplayMember = "medicineName";

                uomLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                uomLookup.DropDownRows = listMedUom.Count;
                uomLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                uomLookup.AutoSearchColumnIndex = 1;
                uomLookup.NullText = "";
                gridView1.Columns[2].ColumnEdit = uomLookup;

                RepositoryItemLookUpEdit bpjsLookup = new RepositoryItemLookUpEdit();
                bpjsLookup.DataSource = listMedBpjs;
                bpjsLookup.ValueMember = "medicineCode";
                bpjsLookup.DisplayMember = "medicineName";

                bpjsLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                bpjsLookup.DropDownRows = listMedBpjs.Count;
                bpjsLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                bpjsLookup.AutoSearchColumnIndex = 1;
                bpjsLookup.NullText = "";
                gridView1.Columns[10].ColumnEdit = bpjsLookup;

                RepositoryItemMemoEdit namaObat = new RepositoryItemMemoEdit();
                namaObat.WordWrap = true;
                gridView1.Columns[1].ColumnEdit = namaObat;

                RepositoryItemLookUpEdit medgroup = new RepositoryItemLookUpEdit();
                medgroup.DataSource = medicineGroup;
                medgroup.ValueMember = "mgroup";
                medgroup.DisplayMember = "group";

                medgroup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                medgroup.DropDownRows = medicineGroup.Count;
                medgroup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                medgroup.AutoSearchColumnIndex = 1;
                medgroup.NullText = "";
                gridView1.Columns[11].ColumnEdit = medgroup;

                //gridView1.BestFitColumns();
                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                gridView1.Columns[10].VisibleIndex = 4;

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void LoadDataTransIn()
        {
            string sql_search, stat = "";

            sql_search = "";

            sql_search = sql_search + Environment.NewLine + "select to_char(trans_date, 'yyyy-mm-dd') trans_date, initcap(b.med_name) med_name,  ";
            sql_search = sql_search + Environment.NewLine + "a.trans_qty, batch_no, to_char(expire_date, 'yyyy-mm-dd') expire_date, trans_remark, 'S' as action  ";
            sql_search = sql_search + Environment.NewLine + "from cs_medicine_trans a ";
            sql_search = sql_search + Environment.NewLine + "join cs_medicine b on (a.med_cd=b.med_cd) ";
            sql_search = sql_search + Environment.NewLine + "where b.status='A' ";
            sql_search = sql_search + Environment.NewLine + "and trans_type='IN' ";
            sql_search = sql_search + Environment.NewLine + "and to_char(trans_date, 'yyyy-mm')=to_char(sysdate,'yyyy-mm') ";
            sql_search = sql_search + Environment.NewLine + "and a.med_cd='" + med_cd + "' ";
            sql_search = sql_search + Environment.NewLine + "order by trans_date desc, a.trans_id desc ";


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

                gridView2.Columns[0].Caption = "Tanggal";
                gridView2.Columns[1].Caption = "Nama Obat";
                gridView2.Columns[2].Caption = "Jumlah";
                gridView2.Columns[3].Caption = "Batch No";
                gridView2.Columns[4].Caption = "Tgl Expire";
                gridView2.Columns[5].Caption = "Remark";
                gridView2.Columns[6].Caption = "Action";

                gridView2.Columns[0].Width = 80;
                gridView2.Columns[2].Width = 60;
                gridView2.Columns[4].Width = 80;

                gridView2.Columns[0].OptionsColumn.AllowEdit = false;
                gridView2.Columns[1].OptionsColumn.AllowEdit = false;
                gridView2.Columns[2].OptionsColumn.AllowEdit = false;
                gridView2.Columns[3].OptionsColumn.AllowEdit = false;
                gridView2.Columns[4].OptionsColumn.AllowEdit = false;
                gridView2.Columns[5].OptionsColumn.AllowEdit = false;

                //gridView2.Columns[2].OptionsColumn.ReadOnly = true;
                //gridView2.Columns[2].OptionsColumn.AllowEdit = false;
                gridView2.Columns[1].Visible = false;
                gridView2.Columns[6].Visible = false;

                gridView2.BestFitColumns();

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
            string p_kode = "", p_nama = "", p_satuan = "", p_status = "", p_bpjs = "", p_action = "", p_limit ="", p_group ="";

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                p_kode = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                p_nama = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                p_satuan = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                p_limit = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                p_status = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                p_action = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                p_bpjs = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                p_group = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();

                if (p_action == "I")
                    {
                        if (p_nama == "")
                        {
                            MessageBox.Show("Nama Obat harus diisi"); return;
                        }
                        else if (p_satuan == "")
                        {
                            MessageBox.Show("Satuan harus diisi"); return;
                        }
                        else if (p_status == "")
                        {
                            MessageBox.Show("Status harus diisi"); return;
                        }
                        else if (p_group == "")
                        {
                            MessageBox.Show("Group Obat harus diisi"); return;
                        }

                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_medicine (med_cd, med_name, status, uom, bpjs_cover, ins_date, ins_emp, LIMIT_STOCK, MED_GROUP) values ";
                        sql_insert = sql_insert + " ('MED' || lpad(cs_med_cd_seq.nextval,4,'0'), '" + p_nama + "', 'A', upper('" + p_satuan + "'), '" + p_bpjs + "', sysdate, '" + DB.vUserId + "', '" + p_limit + "', '" + p_group + "') ";

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

                        if (p_nama == "")
                        {
                            MessageBox.Show("Nama Obat harus diisi"); return;
                        }
                        else if (p_satuan == "")
                        {
                            MessageBox.Show("Satuan harus diisi"); return;
                        }
                        else if (p_status == "")
                        {
                            MessageBox.Show("Status harus diisi"); return;
                        }
                        else if (p_group == "")
                        {
                            MessageBox.Show("Group Obat harus diisi"); return;
                        }
                        sql_update = "";

                        sql_update = sql_update + " update cs_medicine set med_name = '" + p_nama + "', status = '" + p_status + "', uom = upper('" + p_satuan + "'), bpjs_cover = '" + p_bpjs + "', ";
                        sql_update = sql_update + " upd_date = sysdate, upd_emp = '" + DB.vUserId + "', MED_GROUP ='" + p_group + "', LIMIT_STOCK = '" + p_limit + "' ";
                        sql_update = sql_update + " where med_cd = '" + p_kode + "' ";

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
            LoadDataMaster();
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
            if (view.RowCount < 0)
                return;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Status" || e.Column.Caption == "Satuan" || e.Column.Caption == "BPJS" || e.Column.Caption == "Limit Stock" || e.Column.Caption == "Group")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                }
            }
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (view.RowCount < 0)
            //    return;

            //view.SetRowCellValue(e.RowHandle, view.Columns[8], "I");

            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[4], "A");
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
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

            view.SetRowCellValue(e.RowHandle, view.Columns[0], trx_dt);
            view.SetRowCellValue(e.RowHandle, view.Columns[1], med_nm);
            view.SetRowCellValue(e.RowHandle, view.Columns[6], "I");
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveItem.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Nama Obat" || e.Column.Caption == "Jumlah" || e.Column.Caption == "Batch No" || e.Column.Caption == "Tgl Expire" || e.Column.Caption == "Remark")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "U");
                }
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            //GridView view = sender as GridView;

            gridView2.OptionsBehavior.EditingMode = GridEditingMode.Default;
            //gridView2.Columns[0].OptionsColumn.AllowEdit = true;
            //gridView2.Columns[1].OptionsColumn.AllowEdit = true;
            gridView2.Columns[2].OptionsColumn.AllowEdit = true;
            gridView2.Columns[3].OptionsColumn.AllowEdit = true;
            gridView2.Columns[4].OptionsColumn.AllowEdit = true;
            gridView2.Columns[5].OptionsColumn.AllowEdit = true;
            gridView2.AddNewRow();
        }

        private void btnSaveItem_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "";
            string p_qty = "", p_batch = "", p_expire = "", p_remark = "", p_action = "";

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                p_qty = gridView2.GetRowCellValue(i, gridView2.Columns[2]).ToString();
                p_batch = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                p_expire = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                p_remark = gridView2.GetRowCellValue(i, gridView2.Columns[5]).ToString();
                p_action = gridView2.GetRowCellValue(i, gridView2.Columns[6]).ToString();

                if (p_qty == "" || Convert.ToInt16(p_qty) <1)
                {
                    MessageBox.Show("Jumlah harus diisi");
                }
                else if (p_batch == "")
                {
                    MessageBox.Show("No Batch harus diisi");
                }
                else if (p_expire == "")
                {
                    MessageBox.Show("Tanggal Expire harus diisi");
                }
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_medicine_trans (trans_id, med_cd, trans_type, trans_date, trans_qty, batch_no, expire_date, trans_remark, ins_date, ins_emp) values ";
                        sql_insert = sql_insert + " (cs_medtrans_seq.nextval, '" + med_cd + "', 'IN', sysdate, '" + p_qty + "', '" + p_batch + "', to_date('" + p_expire + "','yyyy-mm-dd'), '" + p_remark + "', sysdate, '" + DB.vUserId + "') ";

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
                    //else if (p_action == "U")
                    //{
                    //    sql_update = "";

                    //    sql_update = sql_update + " update cs_diagnosa_item set item_name = '" + p_diag + "', status = '" + p_status + "', cat_id = '" + p_kate + "' ";
                    //    sql_update = sql_update + " where item_cd = '" + p_kode + "' ";

                    //    try
                    //    {
                    //        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    //        OleDbCommand cm2 = new OleDbCommand(sql_update, oraConnect2);
                    //        oraConnect2.Open();
                    //        cm2.ExecuteNonQuery();
                    //        oraConnect2.Close();
                    //        cm2.Dispose();

                    //        //MessageBox.Show("Query Exec : " + sql_update);
                    //        //LoadDataKate();
                    //        MessageBox.Show("Data Berhasil dirubah");
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MessageBox.Show("ERROR: " + ex.Message);
                    //    }
                    //}
                }
            }
            
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridView1_RowClick_1(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;

            if (View.RowCount < 0)
                return;


            med_cd = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            med_nm = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            trx_dt = today;
            LoadDataTransIn();
            LoadDataTransOut();

            lTransIn.Text = "Transaksi Masuk : " + med_nm;
            lTransOut.Text = "Transaksi Keluar : " + med_nm;
            //med_cd = "";
            //med_nm = "";
            //trx_dt = "";

            btnAddItem.Enabled = true;
            btnAddOut.Enabled = true;
        }

        

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (View.RowCount < 0)
                return;


            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Satuan" || e.Column.Caption == "Status" || e.Column.Caption == "BPJS" || e.Column.Caption == "Limit Stock" || e.Column.Caption == "Group")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Stok Saat Ini")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
                string limit = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
                e.Appearance.BackColor = Color.Gainsboro;
                e.Appearance.ForeColor = Color.Black;

                if (stok != "")
                {
                    if (Convert.ToInt32(stok) <= 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (limit != "")
                    {
                        if (Convert.ToInt32(stok) < Convert.ToInt32(limit))
                        {
                            e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                            e.Appearance.ForeColor = Color.White;
                            e.Appearance.FontStyleDelta = FontStyle.Bold;
                        }
                    }
                }

            }
        }

        

        private void gridView1_CustomDrawRowIndicator_1(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        

        private void LoadDataTransOut()
        {
            string sql_search, stat = "";

            sql_search = "";

            //sql_search = sql_search + Environment.NewLine + "select to_char(trans_date, 'yyyy-mm-dd') trans_date, initcap(b.med_name) med_name, ";
            //sql_search = sql_search + Environment.NewLine + "a.trans_qty, trans_cd, a.receipt_id, e.name, trans_remark, 'S' as action   ";
            //sql_search = sql_search + Environment.NewLine + "from cs_medicine_trans a  ";
            //sql_search = sql_search + Environment.NewLine + "join cs_medicine b on (a.med_cd=b.med_cd)  ";
            //sql_search = sql_search + Environment.NewLine + "join cs_receipt c on (a.receipt_id=c.receipt_id) ";
            //sql_search = sql_search + Environment.NewLine + "join cs_patient d on (c.rm_no=d.rm_no) ";
            //sql_search = sql_search + Environment.NewLine + "join cs_employees e on (d.empid=e.empid) ";
            //sql_search = sql_search + Environment.NewLine + "where b.status='A'  ";
            //sql_search = sql_search + Environment.NewLine + "and trans_type='OUT' ";
            //sql_search = sql_search + Environment.NewLine + "and to_char(trans_date, 'yyyy-mm')=to_char(sysdate,'yyyy-mm') ";
            //sql_search = sql_search + Environment.NewLine + "and a.med_cd='" + med_cd + "' ";
            //sql_search = sql_search + Environment.NewLine + "order by trans_date desc ";

            sql_search = sql_search + Environment.NewLine + "select trans_date, med_name, trans_qty, trans_cd, receipt_id, name, trans_remark, action ";
            sql_search = sql_search + Environment.NewLine + "from ( ";
            sql_search = sql_search + Environment.NewLine + "select a.trans_id, to_char(trans_date, 'yyyy-mm-dd') trans_date, initcap(b.med_name) med_name,  ";
            sql_search = sql_search + Environment.NewLine + "a.trans_qty, trans_cd, a.receipt_id, e.name, trans_remark, 'S' as action    ";
            sql_search = sql_search + Environment.NewLine + "from cs_medicine_trans a   ";
            sql_search = sql_search + Environment.NewLine + "join cs_medicine b on (a.med_cd=b.med_cd)   ";
            sql_search = sql_search + Environment.NewLine + "join cs_receipt c on (a.receipt_id=c.receipt_id)  ";
            sql_search = sql_search + Environment.NewLine + "join cs_patient d on (c.rm_no=d.rm_no)  ";
            sql_search = sql_search + Environment.NewLine + "join cs_patient_info e on (d.patient_no=e.patient_no)  ";
            sql_search = sql_search + Environment.NewLine + "where b.status='A'   ";
            sql_search = sql_search + Environment.NewLine + "and trans_type='OUT'  ";
            sql_search = sql_search + Environment.NewLine + "and to_char(trans_date, 'yyyy-mm')=to_char(sysdate,'yyyy-mm')  ";
            sql_search = sql_search + Environment.NewLine + "and a.med_cd='" + med_cd + "'  ";
            sql_search = sql_search + Environment.NewLine + "union ";
            sql_search = sql_search + Environment.NewLine + "select a.trans_id, to_char(trans_date, 'yyyy-mm-dd') trans_date, initcap(b.med_name) med_name,  ";
            sql_search = sql_search + Environment.NewLine + "a.trans_qty, trans_cd, a.receipt_id, null name, trans_remark, 'S' as action   ";
            sql_search = sql_search + Environment.NewLine + "from cs_medicine_trans a   ";
            sql_search = sql_search + Environment.NewLine + "join cs_medicine b on (a.med_cd=b.med_cd)   ";
            sql_search = sql_search + Environment.NewLine + "where b.status='A'   ";
            sql_search = sql_search + Environment.NewLine + "and trans_type='OUT'  ";
            sql_search = sql_search + Environment.NewLine + "and trans_cd in ('RTN','EXP','MAN','ADJ') ";
            sql_search = sql_search + Environment.NewLine + "and to_char(trans_date, 'yyyy-mm')=to_char(sysdate,'yyyy-mm')  ";
            sql_search = sql_search + Environment.NewLine + "and a.med_cd='" + med_cd + "' ) ";
            sql_search = sql_search + Environment.NewLine + "order by trans_date desc, trans_id desc ";



            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl3.DataSource = null;
                gridView3.Columns.Clear();
                gridControl3.DataSource = dt;

                btnSaveOut.Enabled = false;

                //gridView3.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView3.OptionsView.ColumnAutoWidth = true;
                gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView3.IndicatorWidth = 40;
                gridView3.OptionsBehavior.Editable = true;
                gridView3.BestFitColumns();

                //gridView3.OptionsSelection.MultiSelect = true;
                //gridView3.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView3.VisibleColumns[0].Width = 20;
                //gridView3.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView3.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView3.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView3.Columns[4].Visible = false;

                gridView3.Columns[0].Caption = "Tanggal";
                gridView3.Columns[1].Caption = "Nama Obat";
                gridView3.Columns[2].Caption = "Jumlah";
                gridView3.Columns[3].Caption = "Kode";
                gridView3.Columns[4].Caption = "ID";
                gridView3.Columns[5].Caption = "Nama";
                gridView3.Columns[6].Caption = "Remark";
                gridView3.Columns[7].Caption = "Action";

                gridView3.Columns[0].Width = 70;
                gridView3.Columns[2].Width = 50;
                gridView3.Columns[3].Width = 50;
                gridView3.Columns[4].Width = 40;
                gridView3.Columns[5].Width = 80;
                gridView3.Columns[7].Width = 40;

                gridView3.Columns[0].OptionsColumn.AllowEdit = false;
                gridView3.Columns[1].OptionsColumn.AllowEdit = false;
                gridView3.Columns[2].OptionsColumn.AllowEdit = false;
                gridView3.Columns[3].OptionsColumn.AllowEdit = false;
                gridView3.Columns[4].OptionsColumn.AllowEdit = false;
                gridView3.Columns[5].OptionsColumn.AllowEdit = false;
                gridView3.Columns[6].OptionsColumn.AllowEdit = false;
                gridView3.Columns[7].OptionsColumn.AllowEdit = false;

                RepositoryItemLookUpEdit statusLookup = new RepositoryItemLookUpEdit();
                statusLookup.DataSource = outStatus;
                statusLookup.ValueMember = "statusCode";
                statusLookup.DisplayMember = "statusName";

                statusLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                statusLookup.DropDownRows = outStatus.Count;
                statusLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                statusLookup.AutoSearchColumnIndex = 1;
                statusLookup.NullText = "";
                gridView3.Columns[3].ColumnEdit = statusLookup;

                //gridView3.Columns[2].OptionsColumn.ReadOnly = true;
                //gridView3.Columns[2].OptionsColumn.AllowEdit = false;
                gridView3.Columns[1].Visible = false;
                gridView3.Columns[7].Visible = false;

                gridView3.BestFitColumns();

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        

        private void gridView3_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnAddOut_Click(object sender, EventArgs e)
        {
            //GridView view = sender as GridView;

            gridView3.OptionsBehavior.EditingMode = GridEditingMode.Default;
            //gridView3.Columns[0].OptionsColumn.AllowEdit = true;
            //gridView3.Columns[1].OptionsColumn.AllowEdit = true;
            gridView3.Columns[2].OptionsColumn.AllowEdit = true;
            gridView3.Columns[3].OptionsColumn.AllowEdit = true;
            gridView3.Columns[6].OptionsColumn.AllowEdit = true;
            gridView3.AddNewRow();
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.Caption == "Jumlah" || e.Column.Caption == "Batch No" || e.Column.Caption == "Tgl Expire" || e.Column.Caption == "Remark")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.Caption == "Jumlah" || e.Column.Caption == "Kode" || e.Column.Caption == "Remark")
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
                    FileName = "obat.xls",
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

        private void gridView3_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], trx_dt);
            view.SetRowCellValue(e.RowHandle, view.Columns[1], med_nm);
            //view.SetRowCellValue(e.RowHandle, view.Columns[3], "RTN");
            view.SetRowCellValue(e.RowHandle, view.Columns[7], "I");
        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveOut.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Nama Obat" || e.Column.Caption == "Jumlah" || e.Column.Caption == "Kode" || e.Column.Caption == "Remark")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[7]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], "U");
                }
            }
        }

        private void btnSaveOut_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "";
            string p_qty = "", p_code = "", p_remark = "", p_action = "";

            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                p_qty = gridView3.GetRowCellValue(i, gridView3.Columns[2]).ToString();
                p_code = gridView3.GetRowCellValue(i, gridView3.Columns[3]).ToString();
                p_remark = gridView3.GetRowCellValue(i, gridView3.Columns[6]).ToString();
                p_action = gridView3.GetRowCellValue(i, gridView3.Columns[7]).ToString();

                if (p_qty == "" || Convert.ToInt16(p_qty) < 1)
                {
                    MessageBox.Show("Jumlah harus diisi");
                }
                else if (p_code == "" && p_action == "I")
                {
                    MessageBox.Show("Kode harus diisi");
                }
                else if (p_remark == "" && p_action == "I")
                {
                    MessageBox.Show("Remark harus diisi");
                }
                else
                {
                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into cs_medicine_trans (trans_id, med_cd, trans_type, trans_date, trans_qty, trans_cd, trans_remark, ins_date, ins_emp) values ";
                        sql_insert = sql_insert + " (cs_medtrans_seq.nextval, '" + med_cd + "', 'OUT', sysdate, '" + p_qty + "', '" + p_code + "', '" + p_remark + "', sysdate, '" + DB.vUserId + "') ";

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
                }
            }
        }

        private void LoadDataLimit()
        {
            string SQL = "", limit = "";

            if (Convert.ToInt16(txtLimitStok.Text) <= 0)
            {
                limit = "5";
                txtLimitStok.Text = "5";
            }
            else
            {
                limit = txtLimitStok.Text;
            }

            SQL = SQL + Environment.NewLine + "select LISTAGG(med_name, '; ') WITHIN GROUP (ORDER BY med_name, LIMIT_STOCK ASC) med_name, stok from (   ";
            SQL = SQL + Environment.NewLine + "select initcap(med_name) med_name, LIMIT_STOCK , ";
            SQL = SQL + Environment.NewLine + "klinik.FN_CS_INIT_STOCK(sysdate,med_cd) +  ";
            SQL = SQL + Environment.NewLine + "klinik.FN_CS_TRX_IN(sysdate,med_cd) -   ";
            SQL = SQL + Environment.NewLine + "klinik.FN_CS_TRX_OUT(sysdate,med_cd) -   ";
            SQL = SQL + Environment.NewLine + "klinik.FN_CS_REQ_STOCK(sysdate,med_cd) as stok ";
            SQL = SQL + Environment.NewLine + "from cs_medicine  ";
            SQL = SQL + Environment.NewLine + "where 1=1  ";
            SQL = SQL + Environment.NewLine + "and status = 'A' and med_cd  not like 'AK%' ) a  ";
            SQL = SQL + Environment.NewLine + "where 1=1   ";
            SQL = SQL + Environment.NewLine + "and stok <=  LIMIT_STOCK ";
            SQL = SQL + Environment.NewLine + "group by stok ";
            SQL = SQL + Environment.NewLine + "order by stok   ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql2 = new OleDbDataAdapter(SQL, sqlConnect2);
                DataTable dt2 = new DataTable();
                adSql2.Fill(dt2);

                gridControl4.DataSource = null;
                gridView4.Columns.Clear();
                gridControl4.DataSource = dt2;

                ////gridView4.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView4.OptionsView.ColumnAutoWidth = true;
                gridView4.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView4.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView4.IndicatorWidth = 50;
                gridView4.OptionsBehavior.Editable = false;
                //gridView4.BestFitColumns();

                gridView4.Columns[0].Caption = "Nama Obat";
                gridView4.Columns[1].Caption = "Stok Saat Ini";

                gridView4.Columns[1].MinWidth = 80;
                gridView4.Columns[1].MaxWidth = 80;

                gridView4.Columns[0].OptionsColumn.AllowEdit = false;
                gridView4.Columns[1].OptionsColumn.AllowEdit = false;

                RepositoryItemMemoEdit nmObat = new RepositoryItemMemoEdit();
                nmObat.WordWrap = true;
                gridView4.Columns[0].ColumnEdit = nmObat;

                gridView4.BestFitColumns();
                gridView4.Columns[0].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                if (dt2.Rows.Count > 0)
                {
                    tableLayoutPanel3.RowStyles[1] = new RowStyle(SizeType.Absolute, 50);
                    tableLayoutPanel3.RowStyles[2] = new RowStyle(SizeType.Absolute, 200);
                }
                else
                {
                    tableLayoutPanel3.RowStyles[1] = new RowStyle(SizeType.Absolute, 0);
                    tableLayoutPanel3.RowStyles[2] = new RowStyle(SizeType.Absolute, 0);
                }

                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private async void SoftBlink(Control ctrl, Color c1, Color c2, short CycleTime_ms, bool BkClr)
        {
            var sw = new Stopwatch(); sw.Start();
            short halfCycle = (short)Math.Round(CycleTime_ms * 0.5);
            while (true)
            {
                await Task.Delay(1);
                var n = sw.ElapsedMilliseconds % CycleTime_ms;
                var per = (double)Math.Abs(n - halfCycle) / halfCycle;
                var red = (short)Math.Round((c2.R - c1.R) * per) + c1.R;
                var grn = (short)Math.Round((c2.G - c1.G) * per) + c1.G;
                var blw = (short)Math.Round((c2.B - c1.B) * per) + c1.B;
                var clr = Color.FromArgb(red, grn, blw);
                if (BkClr) ctrl.BackColor = clr; else ctrl.ForeColor = clr;
            }
        }

        private void gridView4_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView4_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Stok Saat Ini")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;

                if (stok != "")
                {
                    if (Convert.ToInt16(stok) <= 0)
                    {
                        e.Appearance.BackColor = Color.Crimson;
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (Convert.ToInt16(stok) <= 20)
                    {
                        e.Appearance.BackColor = Color.FromArgb(150, Color.OrangeRed);
                        e.Appearance.ForeColor = Color.White;
                        e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }

            }
        }
    }
}