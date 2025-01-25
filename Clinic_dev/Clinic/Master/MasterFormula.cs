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
using System.Data.OleDb;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;

namespace Clinic
{
    public partial class MasterFormula : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> diagnosaStatus = new List<FlagYn>();
        List<Status> TypeStatus = new List<Status>(); List<Stat> listdosis = new List<Stat>();
        List<FlagYn> FlagStok = new List<FlagYn>();
        List<Medicine> listMedicine = new List<Medicine>(); List<Poli> listpoli = new List<Poli>();
        List<MedGroup> lMedicine = new List<MedGroup>();
        List<CFunction.C_UOM> listMuom = new List<CFunction.C_UOM>();
        DataTable dtGlMed = new DataTable();
        DataTable dt = new DataTable(); DataTable dt_poli = new DataTable();
        DataTable dt_uom = new DataTable();
        DataTable dt_obat = new DataTable();
        int badd =0;
        public string   v_name = "", sql ="";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        RepositoryItemGridLookUpEdit cm_uom = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit cm_poli = new RepositoryItemGridLookUpEdit();
        RepositoryItemGridLookUpEdit LokObatGrid = new RepositoryItemGridLookUpEdit();
        public MasterFormula()
        {
            InitializeComponent();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void MasterFormula_Load(object sender, EventArgs e)
        {
            initData();
            loadData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "MasterFormula");
        }

        private void initData()
        {
            

            FlagStok.Clear();
            FlagStok.Add(new FlagYn() { flagCode = "Y", flagName = "Y" });
            FlagStok.Add(new FlagYn() { flagCode = "N", flagName = "N" });

            diagnosaStatus.Clear();
            diagnosaStatus.Add(new FlagYn() { flagCode = "A", flagName = "Aktif" });
            diagnosaStatus.Add(new FlagYn() { flagCode = "I", flagName = "Tidak Aktif" });

            TypeStatus.Clear();
            TypeStatus.Add(new Status() { statusCode = "BPJS", statusName = "BPJS" });
            TypeStatus.Add(new Status() { statusCode = "UMUM", statusName = "UMUM" });
            TypeStatus.Add(new Status() { statusCode = "ASURANSI", statusName = "ASURANSI" });

            SetUom();
            SetPoli();
        }
        void SetUom()
        {
            string sql1 = " ";
            sql1 = " ";
            sql1 = sql1 + " select code_id CODE_UOM, code_id UOM from CS_CODE_DATA where CODE_CLASS_ID ='MED_UOM' order by 1  ";
            dt_uom = ConnOra.Data_Table_ora(sql1);
            //lookupna(cm_uom, "formula", dt_uom);

            listdosis.Clear();
            for (int i = 0; i < dt_uom.Rows.Count; i++)
            {
                listdosis.Add(new Stat() { statCode = dt_uom.Rows[i]["CODE_UOM"].ToString(), statName = dt_uom.Rows[i]["UOM"].ToString() });
            }
        }
        void SetPoli()
        {
            string sql1 = " ";
            sql1 = " ";
            sql1 = sql1 + " select POLI_CD, POLI_NAME from CS_POLICLINIC where POLI_CD in('POL0001','POL0002','POL0000','POL0006') order by 1  ";
            dt_poli = ConnOra.Data_Table_ora(sql1);
            lookupna(cm_poli, "POLI_NAME", dt_poli);

            listpoli.Clear();
            for (int i = 0; i < dt_poli.Rows.Count; i++)
            {
                listpoli.Add(new Poli() { poliCode = dt_poli.Rows[i]["POLI_CD"].ToString(), poliName = dt_poli.Rows[i]["POLI_NAME"].ToString() });
            }
        }
        void lookupna(RepositoryItemGridLookUpEdit lookna, string ngaran, DataTable datatablena)
        {
            lookna.DataSource = datatablena;
            lookna.DisplayMember = ngaran;
            lookna.ValueMember = ngaran;
        }
        private void btnLoadDosis_Click(object sender, EventArgs e)
        {
            initData();
            loadData();
        }

        private void loadData()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, formula_id, b.MED_GROUP, b.med_cd,  b.med_cd med_name,  upper(formula) formula, qty, base_price, med_price, a.MINUS_STOK, a.POLI_CD Poli, a.ATT1 status  , a.ATT2 Kategori, a.Racikan "; //initcap(med_name) 
            sql_search = sql_search + Environment.NewLine + "from cs_formula  a, CS_MEDICINE b";
            sql_search = sql_search + Environment.NewLine + "where a.med_cd(+) = b.med_cd  and b.status='A'  ";
            if (rdObat.Checked)
                sql_search = sql_search + Environment.NewLine + "     and MED_GROUP ='OBAT'  ";
            else
                sql_search = sql_search + Environment.NewLine + "     and MED_GROUP ='ALKES'   ";
            sql_search = sql_search + Environment.NewLine + "order by 4,3,2 ";

            //loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                //DataTable dt = new DataTable();
                dt.Clear();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 50;
                gridView1.OptionsBehavior.Editable = true; 

                //gridView1.OptionsSelection.MultiSelect = true;
                //gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
                //gridView1.VisibleColumns[0].Width = 20;
                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[0].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[4].Visible = false;

                gridView1.Columns[0].Caption = "Action";
                gridView1.Columns[1].Caption = "ID";
                gridView1.Columns[2].Caption = "Kategori";
                gridView1.Columns[3].Caption = "Kode Obat";
                gridView1.Columns[4].Caption = "Obat";
                gridView1.Columns[5].Caption = "Dosis";
                gridView1.Columns[6].Caption = "Jumlah";
                gridView1.Columns[7].Caption = "Harga Beli";
                gridView1.Columns[8].Caption = "Harga Jual";
                gridView1.Columns[9].Caption = "Minus Stok";
                gridView1.Columns[10].Caption = "Poli";
                gridView1.Columns[11].Caption = "Status";
                gridView1.Columns[12].Caption = "Kategori";
                gridView1.Columns[13].Caption = "Racikan";

                gridView1.Columns[0].Width = 25;
                gridView1.Columns[1].Width = 25;
                gridView1.Columns[2].Width = 50;
                gridView1.Columns[3].Width = 30;
                gridView1.Columns[4].Width = 150;
                gridView1.Columns[5].Width = 55;
                gridView1.Columns[6].Width = 40;
                gridView1.Columns[7].Width = 60;
                gridView1.Columns[8].Width = 60;
                gridView1.Columns[9].Width = 70;
                gridView1.Columns[10].Width = 85;
                gridView1.Columns[11].Width = 80;
                gridView1.Columns[12].Width = 120;
                gridView1.Columns[13].Width = 70;
                string sql_med = "";

                sql_med = "";
                sql_med = sql_med + Environment.NewLine + " select DISTINCT a.att2 Kategori, b.med_cd Kode_Obat, initcap(med_name)  Nama_Obat   ";
                sql_med = sql_med + Environment.NewLine + "   from KLINIK.cs_formula a right join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1    ";
                sql_med = sql_med + Environment.NewLine + "    and b.status = 'A'  ";
                if(rdObat.Checked)
                    sql_med = sql_med + Environment.NewLine + "     and MED_GROUP ='OBAT'  ";
                else
                    sql_med = sql_med + Environment.NewLine + "     and MED_GROUP ='ALKES'   "; 
                sql_med = sql_med + Environment.NewLine + "  order by a.att2,3  ";

                OleDbConnection sqlConnectU = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSqlU = new OleDbDataAdapter(sql_med, sqlConnectU);
                DataTable dtU = new DataTable();
                dtGlMed = dtU;
                adSqlU.Fill(dtU);
                lMedicine.Clear();
                //dtGlMed.Clear();
                for (int i = 0; i < dtU.Rows.Count; i++)
                {
                    lMedicine.Add(new MedGroup() { Kategori = dtU.Rows[i]["Kategori"].ToString(), Kode_Obat = dtU.Rows[i]["Kode_Obat"].ToString(), Nama_Obat = dtU.Rows[i]["Nama_Obat"].ToString() });
                }



                //dtGlMed.Clear();
                //string sql_med = " select med_cd, initcap(med_name) med_name from cs_medicine where status = 'A' order by med_name ";
                //OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
                //DataTable dt3 = new DataTable();
                //dtGlMed = dt3;
                //adSql3.Fill(dt3);
                //listMedicine.Clear();
                //for (int i = 0; i < dt3.Rows.Count; i++)
                //{
                //    listMedicine.Add(new MedGroup() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
                //}

                ConnOra.LookUpGroupGridFilter(lMedicine, gridView1, "Kategori", "Kode_Obat", "Nama_Obat", LokObatGrid, 4);

                //RepositoryItemGridLookUpEdit glmed = new RepositoryItemGridLookUpEdit();
                //glmed.DataSource = listMedicine;
                //glmed.ValueMember = "medicineCode";
                //glmed.DisplayMember = "medicineName";

                //glmed.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //glmed.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                //glmed.ImmediatePopup = true;
                //glmed.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //glmed.NullText = "";
                //gridView1.Columns[4].ColumnEdit = glmed;

                RepositoryItemLookUpEdit dLookup = new RepositoryItemLookUpEdit();
                dLookup.DataSource = diagnosaStatus;
                dLookup.ValueMember = "flagCode";
                dLookup.DisplayMember = "flagName";

                dLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                dLookup.DropDownRows = diagnosaStatus.Count;
                dLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                dLookup.AutoSearchColumnIndex = 1;
                dLookup.NullText = "Aktif";
                gridView1.Columns[10].ColumnEdit = dLookup;

                RepositoryItemLookUpEdit dLookStok = new RepositoryItemLookUpEdit();
                dLookStok.DataSource = FlagStok;
                dLookStok.ValueMember = "flagCode";
                dLookStok.DisplayMember = "flagName";
                dLookStok.NullText = "Y";
                gridView1.Columns[9].ColumnEdit = dLookStok;

                RepositoryItemGridLookUpEdit glpoli = new RepositoryItemGridLookUpEdit();
                glpoli.DataSource = listpoli ;
                glpoli.ValueMember = "poliCode";
                glpoli.DisplayMember = "poliName";

                glpoli.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glpoli.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glpoli.ImmediatePopup = true;
                glpoli.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glpoli.NullText = "";
                gridView1.Columns[10].ColumnEdit = glpoli;

                RepositoryItemGridLookUpEdit glUom = new RepositoryItemGridLookUpEdit();
                glUom.DataSource = listdosis;
                glUom.ValueMember = "statCode";
                glUom.DisplayMember = "statName";

                glUom.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glUom.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glUom.ImmediatePopup = true;
                glUom.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glUom.NullText = "";
                gridView1.Columns[5].ColumnEdit = glUom;

                RepositoryItemGridLookUpEdit glStatus = new RepositoryItemGridLookUpEdit();
                glStatus.DataSource = TypeStatus;
                glStatus.ValueMember = "statusCode";
                glStatus.DisplayMember = "statusName";

                glStatus.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glStatus.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                //glStatus.ImmediatePopup = true;
                glStatus.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glStatus.NullText = "";
                gridView1.Columns[11].ColumnEdit = glStatus;


                //cm_uom.NullText = "";
                //cm_poli.NullText = "";
                //gridView1.Columns[5].ColumnEdit = cm_uom;
                //gridView1.Columns[11].ColumnEdit = TypeStatus;
                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;
                //gridView1.Columns[11].Visible = false;
                gridView1.Columns[2].OptionsColumn.AllowEdit = false;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[4].OptionsColumn.ReadOnly = true;

                gridView1.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnAddDosis_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
                return;

            Random rand = new Random();
            badd = 1;
            //DataTable dt = gridControl1.DataSource as DataTable;
            DataRow newRow = dt.NewRow();
            int pos = gridView1.GetDataSourceRowIndex(gridView1.FocusedRowHandle) + 1;
            dt.Rows.InsertAt(newRow, pos);
            gridView1.SetRowCellValue(pos, gridView1.Columns[0], "I");
            if (rdObat.Checked)
                gridView1.SetRowCellValue(pos, gridView1.Columns[2], "OBAT");
            else
                gridView1.SetRowCellValue(pos, gridView1.Columns[2], "ALKES");
            gridView1.FocusedRowHandle = pos;// DevExpress.XtraGrid.GridControl.NewItemRowHandle;
            gridView1.FocusedColumn = gridView1.VisibleColumns[2];
            gridView1.CloseEditor();
            gridView1.UpdateCurrentRow();
            gridView1.ShowEditor();
            //TextEdit edit = sender as TextEdit;
            //if (edit == null) return;
            //edit.SelectionStart = 1;
            //edit.SelectionLength = 0; 

            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default; 
            //gridView1.FocusedRowHandle = DevExpress.XtraGrid.GridControl.NewItemRowHandle +1; 
            //gridView1.FocusedColumn = gridView1.VisibleColumns[0]; 
        
         }
        
        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            if(rdObat.Checked)
                view.SetRowCellValue(e.RowHandle, view.Columns[2], "OBAT");
            else
                view.SetRowCellValue(e.RowHandle, view.Columns[2], "ALKES");
        }

        private void btnSaveDosis_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "", sql_cnt = "", p_jumlah = "", p_hargaB = "", p_hargaj = "", p_fstok = "", p_id ="", p_poli ="", p_racikan ="";
            string p_kode = "", p_dosis = "", p_status = "", p_action = "", p_kategori = "";
            
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {

                //gridView1.Columns[0].Caption = "Action";
                //gridView1.Columns[1].Caption = "ID";
                //gridView1.Columns[2].Caption = "Kategori";
                //gridView1.Columns[3].Caption = "Kode Obat";
                //gridView1.Columns[4].Caption = "Obat";
                //gridView1.Columns[5].Caption = "Dosis";
                //gridView1.Columns[6].Caption = "Jumlah";
                //gridView1.Columns[7].Caption = "Harga Beli";
                //gridView1.Columns[8].Caption = "Harga Jual";
                //gridView1.Columns[9].Caption = "Minus Stok";
                //gridView1.Columns[10].Caption = "Poli";
                //gridView1.Columns[11].Caption = "Status";

                p_action = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                p_id = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                p_kode = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                p_dosis = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                p_jumlah = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                p_hargaB = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                p_hargaj = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                p_fstok = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                p_poli = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                p_status = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                p_kategori = gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
                p_racikan = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();


                if (p_action == "I" && p_id.ToString().Equals(""))
                {
                    if (p_kode == "")
                    {
                        MessageBox.Show("Nama Obat harus diisi"); return;
                    }
                    else if (p_dosis == "")
                    {
                    MessageBox.Show("Dosis harus diisi"); return;
                    }
                    else if (p_jumlah == "")
                    {
                        MessageBox.Show("Jumlah harus diisi"); return;
                    }
                    else if (p_hargaj == "")
                    {
                        MessageBox.Show("Harga harus diisi"); return;
                    }
                    if(p_fstok.ToString().Equals(""))
                        p_fstok = gridView1.GetFocusedRowCellDisplayText(gridView1.Columns[9]).ToString();
                    sql_insert = "";

                    sql_insert = sql_insert + " insert into cs_formula (formula_id, med_cd, formula, BASE_PRICE, med_price, qty, MINUS_STOK, att1, POLI_CD, ins_date, ins_emp, status, att2, racikan) values ";
                    sql_insert = sql_insert + " (CS_FORMULA_SEQ.nextval, '" + p_kode + "', '" + p_dosis + "', '" + p_hargaB + "', '" + p_hargaj + "', '" + p_jumlah + "','" + p_fstok + "', '" + p_status + "', '" + p_poli + "', sysdate, '" + DB.vUserId + "', 'A', '" + p_kategori + "', '" + p_racikan + "') ";

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
                else if (p_action == "U" && p_id.ToString().Equals(""))
                {
                    if (p_kode == "")
                    {
                        MessageBox.Show("Nama Obat harus diisi"); return;
                    }
                    else if (p_dosis == "")
                    {
                        MessageBox.Show("Dosis harus diisi"); return;
                    }
                    else if (p_jumlah == "")
                    {
                        MessageBox.Show("Jumlah harus diisi"); return;
                    }
                    else if (p_hargaj == "")
                    {
                        MessageBox.Show("Harga harus diisi"); return;
                    }

                    if (p_fstok.ToString().Equals(""))
                        p_fstok = gridView1.GetFocusedRowCellDisplayText(gridView1.Columns[9]).ToString();

                    sql_insert = "";
                    sql_insert = sql_insert + " insert into cs_formula (formula_id, med_cd, formula, BASE_PRICE, med_price, qty, MINUS_STOK, att1, POLI_CD, ins_date, ins_emp, status, att2, RACIKAN) values ";
                    sql_insert = sql_insert + " (CS_FORMULA_SEQ.nextval, '" + p_kode + "', '" + p_dosis + "', '" + p_hargaB + "', '" + p_hargaj + "', '" + p_jumlah + "','" + p_fstok + "', '" + p_status + "', '" + p_poli + "', sysdate, '" + DB.vUserId + "', 'A', '" + p_kategori + "', '" + p_racikan + "') ";

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
                else if (p_action == "U" && !p_id.ToString().Equals(""))
                {
                    if (p_kode == "")
                    {
                        MessageBox.Show("Nama Obat harus diisi"); return;
                    }
                    else if (p_dosis == "")
                    {
                        MessageBox.Show("Dosis harus diisi"); return;
                    }
                    else if (p_jumlah == "")
                    {
                        MessageBox.Show("Jumlah harus diisi"); return;
                    }
                    else if (p_hargaj == "")
                    {
                        MessageBox.Show("Harga harus diisi"); return;
                    }

                    sql = sql + " ";
                    sql = sql + " insert into KLINIK.CS_FORMULA_HIS select a.*,  '" + DB.vUserId + "' CREATED_BY, sysdate CREATED_DATE from KLINIK.CS_FORMULA a where  formula_id = '" + p_id + "'  ";
                    ORADB.Execute(ORADB.XE, sql);
                           
                    sql_update = ""; 
                    sql_update = sql_update + " update cs_formula set med_cd = '" + p_kode + "', formula = '" + p_dosis + "', BASE_PRICE = '" + p_hargaB + "' , med_price = '" + p_hargaj + "', qty = '" + p_jumlah + "', MINUS_STOK = '" + p_fstok + "', POLI_CD ='" + p_poli + "', ";
                    sql_update = sql_update + " upd_date = sysdate, upd_emp = '" + DB.vUserId + "' , att1 = '" + p_status + "' , att2 = '" + p_kategori + "', racikan ='" + p_racikan + "'";
                    sql_update = sql_update + " where formula_id = '" + p_id + "' ";

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
            loadData();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveDosis.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Obat" || e.Column.Caption == "Dosis" || e.Column.Caption == "Jumlah" || e.Column.Caption == "Harga Beli" || e.Column.Caption == "Harga Jual" || e.Column.Caption == "Minus Stok" || e.Column.Caption == "Status" || e.Column.Caption == "Kategori" || e.Column.Caption == "Racikan")
            {               
                if(badd == 1)
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
                }
                else
                {
                    string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
                    if (tmp_stat == "I")
                    {
                        view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
                    }
                    else
                    {
                        view.SetRowCellValue(e.RowHandle, view.Columns[0], "U");
                    }
                }  
            }
            if (e.Column.Caption == "Obat")
            {
                string sql1 = " ", id = "";
                id = gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns[4]).ToString();
                if(!id.ToString().Equals(""))
                {
                    sql1 = " ";
                    sql1 = sql1 + " select MED_GROUP, MED_CD from CS_MEDICINE where MED_CD = '" + id + "'  ";
                    dt_obat = ConnOra.Data_Table_ora(sql1);
                    view.SetRowCellValue(e.RowHandle, view.Columns[2], dt_obat.Rows[0][0].ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[3], id);
                } 
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Obat" || e.Column.Caption == "Dosis" || e.Column.Caption == "Jumlah" || e.Column.Caption == "Harga Beli" || e.Column.Caption == "Harga Jual" || e.Column.Caption == "Minus Stok" || e.Column.Caption == "Poli" || e.Column.Caption == "Status" || e.Column.Caption == "Kategori" || e.Column.Caption == "Racikan")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDelDosis_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", id = "";

                id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();

                sql_delete = " "; 
                sql_delete = sql_delete + " update cs_formula set status = 'I' ,upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                sql_delete = sql_delete + " where formula_id = '" + id + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                CFunction.Export_excel(gridView1);
                //SaveFileDialog saveDialog = new SaveFileDialog
                //{
                //    Filter = "XLS (*.xls)|*.xlsx",
                //    FileName = "dosis_obat.xls",
                //    RestoreDirectory = true,
                //    CheckFileExists = false,
                //    CheckPathExists = true,
                //    OverwritePrompt = true,
                //    DereferenceLinks = true,
                //    ValidateNames = true,
                //    AddExtension = false,
                //    FilterIndex = 1
                //};
                //saveDialog.InitialDirectory = "C:\\";
                //if (saveDialog.ShowDialog() == DialogResult.OK)
                //{
                //    gridControl1.ExportToXls(saveDialog.FileName);
                //}
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }
    }
    
}