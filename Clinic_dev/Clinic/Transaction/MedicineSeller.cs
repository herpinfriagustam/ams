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
    public partial class MedicineSeller : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<FlagYn> diagnosaStatus = new List<FlagYn>();
        List<FlagYn> gender = new List<FlagYn>();
        List<Medicine> listMedicine = new List<Medicine>();
        DataTable dtGlMed = new DataTable();
        List<Layanan> listLaya2 = new List<Layanan>();
        List<Formula2> listFormula2 = new List<Formula2>();
        List<Stat> listType = new List<Stat>();
        List<Stat> listKir = new List<Stat>();
        List<Dosis> listDosis = new List<Dosis>();
        DataTable dtObat; DataTable datstock;
        public string   v_name = "";
        string kate_cd = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string type = "", sdate = "", edate = "";
        RepositoryItemLookUpEdit LookType = new RepositoryItemLookUpEdit();
        RepositoryItemLookUpEdit LookLynan = new RepositoryItemLookUpEdit();
        //string today = "2019-11-27";

        public MedicineSeller()
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
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "MedicineSeller");
        }

        private void initData()
        { 
            gender.Clear();
            gender.Add(new FlagYn() { flagCode = "L", flagName = "Laki-Laki" });
            gender.Add(new FlagYn() { flagCode = "P", flagName = "Perempuan" });
             
            string sql_date = "";
            sql_date = " select to_char(sysdate,'yyyy-mm-dd') sdate, to_char(sysdate,'yyyy-mm-dd') edate from dual ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_date, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            sdate = dt.Rows[0]["sdate"].ToString();
            edate = dt.Rows[0]["edate"].ToString();
            dDateBgn.Text = sdate;
            dDateEnd.Text = edate;

            //listType.Clear();
            //listType.Add(new Stat() { statCode = "MED", statName = "MED" }); 

            string sql_dosis = " select code_id, code_name from CS_CODE_DATA where code_class_id = 'DOSIS' order by SORT_ORDER ";
            OleDbConnection oraCondsd = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOrados = new OleDbDataAdapter(sql_dosis, oraCondsd);
            DataTable dtgsis = new DataTable();
            adOrados.Fill(dtgsis);
            listDosis.Clear();
            for (int i = 0; i < dtgsis.Rows.Count; i++)
            {
                listDosis.Add(new Dosis() { DosisCode = dtgsis.Rows[i]["code_id"].ToString(), DosisName = dtgsis.Rows[i]["code_name"].ToString() });
            }

            dtGlMed.Clear();
            string sql_med = " select distinct b.med_cd, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 and BPJS_COVER ='N'  and MINUS_STOK ='Y' and MED_GROUP in('OBAT','OTC') order by med_name ";
            DataTable dt3 = ConnOra.Data_Table_ora(sql_med); 
            dtGlMed = dt3; 
            listMedicine.Clear();
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
            }
        }

        private void btnLoadDosis_Click(object sender, EventArgs e)
        {
            //initData();
            loadData();
        }

        private void loadData()
        {
            string sql_search, stat = "";
            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "select 'S' action, kir_id, regis_date regis_date, ";
            sql_search = sql_search + Environment.NewLine + "nid, name, gender, birth_place, birth_date  birth_date, ";
            sql_search = sql_search + Environment.NewLine + "addrs, jobs, purpose, height, weight, blood_press, d_now, d_his, eye_status, ";
            sql_search = sql_search + Environment.NewLine + "ID_ITEM_LAYANAN Harga, f_type, decode(STAT_PAY,'X','Closed',STAT_PAY) STAT_PAY ";
            sql_search = sql_search + Environment.NewLine + "from cs_kir ";
            sql_search = sql_search + Environment.NewLine + "where 1=1 AND f_type IN ('MED') ";
            sql_search = sql_search + Environment.NewLine + "and trunc(regis_date) between to_date('" + dDateBgn.Text + "','yyyy-mm-dd') and to_date('" + dDateEnd.Text + "','yyyy-mm-dd') ";
            sql_search = sql_search + Environment.NewLine + "order by regis_date, name ";
             
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gJualObat.DataSource = null;
                gvJualObat.Columns.Clear();
                gJualObat.DataSource = dt;
                 
                gvJualObat.OptionsView.ColumnAutoWidth = true;
                gvJualObat.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gvJualObat.Appearance.HeaderPanel.FontSizeDelta = 0;
                gvJualObat.IndicatorWidth = 40;
                gvJualObat.OptionsBehavior.Editable = true; 

                gvJualObat.FixedLineWidth = 6;
                gvJualObat.Columns[18].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gvJualObat.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gvJualObat.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gvJualObat.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gvJualObat.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gvJualObat.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gvJualObat.Columns[5].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gvJualObat.Columns[19].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gvJualObat.Columns[0].Caption = "Action";
                gvJualObat.Columns[1].Caption = "KIR ID";
                gvJualObat.Columns[2].Caption = "Tgl Regis";
                gvJualObat.Columns[3].Caption = "No KTP";
                gvJualObat.Columns[4].Caption = "Nama";
                gvJualObat.Columns[5].Caption = "JK";
                gvJualObat.Columns[6].Caption = "TempatLahir";
                gvJualObat.Columns[7].Caption = "Tgl Lahir";
                gvJualObat.Columns[8].Caption = "Alamat";
                gvJualObat.Columns[9].Caption = "Pekerjaan";
                gvJualObat.Columns[10].Caption = "Keperluan";
                gvJualObat.Columns[11].Caption = "TB";
                gvJualObat.Columns[12].Caption = "BB";
                gvJualObat.Columns[13].Caption = "Tek.Darah";
                gvJualObat.Columns[14].Caption = "P.Sekarang";
                gvJualObat.Columns[15].Caption = "P.Dahulu";
                gvJualObat.Columns[16].Caption = "Status Mata";
                gvJualObat.Columns[17].Caption = "Layanan";
                gvJualObat.Columns[18].Caption = "Type";
                gvJualObat.Columns[19].Caption = "Status Pay";

                gvJualObat.Columns[0].Visible = false;
                gvJualObat.Columns[1].Visible = false;
                gvJualObat.Columns[6].Visible = false; gvJualObat.Columns[7].Visible = false; gvJualObat.Columns[8].Visible = false; gvJualObat.Columns[9].Visible = false; gvJualObat.Columns[10].Visible = false;
                gvJualObat.Columns[11].Visible = false; gvJualObat.Columns[12].Visible = false; gvJualObat.Columns[13].Visible = false; gvJualObat.Columns[14].Visible = false; gvJualObat.Columns[15].Visible = false;
                gvJualObat.Columns[16].Visible = false; gvJualObat.Columns[17].Visible = false;
                gvJualObat.Columns[18].OptionsColumn.AllowEdit = false;
                //gridView1.Columns[16].OptionsColumn.ReadOnly = true;

                RepositoryItemLookUpEdit genderLookup = new RepositoryItemLookUpEdit();
                genderLookup.DataSource = gender;
                genderLookup.ValueMember = "flagCode";
                genderLookup.DisplayMember = "flagName";

                genderLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                genderLookup.DropDownRows = gender.Count;
                genderLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                genderLookup.AutoSearchColumnIndex = 1;
                genderLookup.NullText = "";
                gvJualObat.Columns[5].ColumnEdit = genderLookup;

                RepositoryItemLookUpEdit mataLookup = new RepositoryItemLookUpEdit();
                mataLookup.DataSource = diagnosaStatus;
                mataLookup.ValueMember = "flagCode";
                mataLookup.DisplayMember = "flagName";

                mataLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                mataLookup.DropDownRows = diagnosaStatus.Count;
                mataLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                mataLookup.AutoSearchColumnIndex = 1;
                mataLookup.NullText = "";
                gvJualObat.Columns[16].ColumnEdit = mataLookup;

                string SQL = "";
                SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) || ' : ' || to_char(treat_item_price) treat_item_name ";
                SQL = SQL + Environment.NewLine + "from cs_treatment_item ";
                SQL = SQL + Environment.NewLine + "where 1=1 ";
                SQL = SQL + Environment.NewLine + "and treat_group_id in ('TRG13','TRG03')  and TREAT_TYPE_ID = 'TRT01' ";

                OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
                DataTable dtly = new DataTable();
                adOraly.Fill(dtly);
                listLaya2.Clear();
                for (int i = 0; i < dtly.Rows.Count; i++)
                {
                    listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
                }

                RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
                glLaya.DataSource = listLaya2;
                glLaya.ValueMember = "layananCode";
                glLaya.DisplayMember = "layananName";

                glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glLaya.ImmediatePopup = true;
                glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glLaya.NullText = "";
                gvJualObat.Columns[17].ColumnEdit = glLaya; 

                //LookType.DataSource = listType;
                //LookType.ValueMember = "statCode";
                //LookType.DisplayMember = "statName";

                //LookType.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //LookType.DropDownRows = listType.Count + 1;
                //LookType.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                //LookType.AutoSearchColumnIndex = 1;
                //LookType.NullText = "";
                //gvJualObat.Columns[18].ColumnEdit = LookType;

                gvJualObat.BestFitColumns();
                //loading.CloseWaitForm();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
        private void LoadDataResep(string kir_id)
        {
            string sql_med_load = "", s_rm = "", s_date = "", s_que = "", sstatus = "", spoli = "";

            s_rm = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, gvJualObat.Columns[10]).ToString();
            s_que = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, gvJualObat.Columns[0]).ToString();
            s_date = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, gvJualObat.Columns[8]).ToString();
            sstatus = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, gvJualObat.Columns[15]).ToString(); 

            sql_med_load = " select a.receipt_id, a.med_cd, b.med_group, a.med_cd, A.formula, type_drink,  " +
                           " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  " +
                           " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -  " +
                           " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) - " +
                           " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock, " +
                           " A.med_qty, initcap(uom) uom, 'S' action, a.confirm, a.days, a.price, a.qty_day, a.dosis " +
                           " from KLINIK.cs_receipt a  JOIN KLINIK.CS_KIR c on(a.ATT3_RECIEPT =  c.KIR_ID) " +
                           " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)  JOIN KLINIK.cs_formula D ON (B.med_cd = D.med_cd AND D.FORMULA_ID = A.formula) " +
                           " where b.status = 'A'   and D.MINUS_STOK ='Y'  and BPJS_COVER ='N' " + 
                           " and c.KIR_ID = '" + kir_id + "' ";

            dtObat = ConnOra.Data_Table_ora(sql_med_load);

            gObatJual.DataSource = null;
            gObatJual.DataSource = dtObat;

            gvObatJual.OptionsView.ColumnAutoWidth = true;
            gvObatJual.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gvObatJual.Appearance.HeaderPanel.FontSizeDelta = 0;
            gvObatJual.IndicatorWidth = 30;
            gvObatJual.BestFitColumns();

            gvObatJual.Columns[6].OptionsColumn.ReadOnly = true;
            gvObatJual.Columns[10].OptionsColumn.ReadOnly = true;

            string sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 and BPJS_COVER ='N' and MINUS_STOK ='Y'  and MED_GROUP in('OBAT','OTC') order by med_name ";
            DataTable dtf = ConnOra.Data_Table_ora(sql_for);
             
            listFormula2.Clear();
            for (int i = 0; i < dtf.Rows.Count; i++)
            {
                listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
            }

            RepositoryItemGridLookUpEdit glmed = new RepositoryItemGridLookUpEdit();
            glmed.DataSource = listMedicine;
            glmed.ValueMember = "medicineCode";
            glmed.DisplayMember = "medicineName";

            glmed.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glmed.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glmed.ImmediatePopup = true;
            glmed.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glmed.NullText = "";
            gvObatJual.Columns[1].ColumnEdit = glmed;

            RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit();
            glfor.DataSource = listFormula2;
            glfor.ValueMember = "formulaCode";
            glfor.DisplayMember = "formulaName";

            glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glfor.ImmediatePopup = true;
            glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glfor.NullText = "";
            gvObatJual.Columns["FORMULA"].ColumnEdit = glfor;

            RepositoryItemLookUpEdit dosisLookup = new RepositoryItemLookUpEdit();
            dosisLookup.DataSource = listDosis;
            dosisLookup.ValueMember = "DosisCode";
            dosisLookup.DisplayMember = "DosisName";
            dosisLookup.NullText = "";
            gvObatJual.Columns["DOSIS"].ColumnEdit = dosisLookup;
             
        }
        private void btnAddDosis_Click(object sender, EventArgs e)
        {
            gvJualObat.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvJualObat.Columns[1].OptionsColumn.ReadOnly = false; 
            gvJualObat.Columns[18].OptionsColumn.ReadOnly = false;
            gvJualObat.AddNewRow();
        }
        
        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[2], today);
            view.SetRowCellValue(e.RowHandle, view.Columns[18], "MED");
        }

        private void btnSaveDosis_Click(object sender, EventArgs e)
        {
            string sql_insert = "", sql_update = "",  pay_sts ="", p_kirid = "", dte ="";
            string p_class = "", p_tgl = "", p_ktp="", p_nama = "", pjk = "", p_status = "", p_attr="", p_action = "", p_layanan="";
            object tgl =null;

            for (int i = 0; i < gvJualObat.DataRowCount; i++)
            {
                 
                p_action = gvJualObat.GetRowCellValue(i, gvJualObat.Columns[0]).ToString();
                p_kirid = gvJualObat.GetRowCellValue(i, gvJualObat.Columns[1]).ToString();
                //p_layanan = gvJualObat.GetRowCellValue(i, gvJualObat.Columns[17]).ToString();
                p_class = gvJualObat.GetRowCellValue(i, gvJualObat.Columns[18]).ToString();
                tgl = gvJualObat.GetRowCellValue(i, gvJualObat.Columns[2]); 
                p_ktp = gvJualObat.GetRowCellValue(i, gvJualObat.Columns[3]).ToString();
                p_nama = gvJualObat.GetRowCellValue(i, gvJualObat.Columns[4]).ToString();
                pjk = gvJualObat.GetRowCellValue(i, gvJualObat.Columns[5]).ToString();
                pay_sts = gvJualObat.GetRowCellValue(i, gvJualObat.Columns[19]).ToString();

                if (p_class == "")
                {
                    MessageBox.Show("Type harus di Tentukan");
                }
                else if (p_nama == "")
                {
                    MessageBox.Show("Nama harus diisi");
                } 
                else
                {
                    if (tgl != null && tgl is DateTime)
                    {
                        DateTime selectedDateTime = (DateTime)tgl;
                        dte = selectedDateTime.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        DateTime selectedDateTime = DateTime.Now;
                        dte = selectedDateTime.ToString("yyyy-MM-dd");
                    }
                    if (p_ktp.ToString().Equals(""))
                        p_ktp = "-";

                    if (p_action == "I")
                    {
                        sql_insert = "";

                        sql_insert = sql_insert + " insert into KLINIK.cs_kir (KIR_ID, REGIS_DATE, NID, NAME, GENDER, F_TYPE,INS_DATE,INS_EMP ) values ";
                        sql_insert = sql_insert + " (KLINIK.CS_KIR_SEQ.nextval,  to_date('" + dte + "', 'yyyy-MM-dd'), '" + p_ktp + "', '" + p_nama + "', '" + pjk + "', "; 
                        sql_insert = sql_insert + " '" + p_class + "', sysdate, '" + DB.vUserId + "')"; 

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose(); 

                            MessageBox.Show("Data Pembeli Berhasil ditambah");
                            loadData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                    else if (p_action == "U" && pay_sts == "N")
                    {
                        sql_update = " ";

                        sql_update = sql_update + " update KLINIK.cs_kir set REGIS_DATE =  to_date('" + dte + "', 'yyyy-MM-dd') , NID = '" + p_ktp + "', NAME = '" + p_nama + "', GENDER = '" + pjk + "' "; 
                        sql_update = sql_update + " where  KIR_ID = '" + p_kirid + "' and F_TYPE = '" + p_class + "' ";

                        try
                        {
                            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm2 = new OleDbCommand(sql_update, oraConnect2);
                            oraConnect2.Open();
                            cm2.ExecuteNonQuery();
                            oraConnect2.Close();
                            cm2.Dispose();
                             
                            MessageBox.Show("Data Berhasil dirubah");
                            loadData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
           
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveDosis.Enabled = true;
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[18]).ToString();
            string sqlkir = "", sttype ="";

            if (e.Column.Caption == "Type" && ((a.ToString() == "KIR") || (a.ToString() == "MCU")))
            {
                if (a.ToString() == "KIR")
                    sttype = "TRG13";
                else
                    sttype = "TRG03";

                sqlkir = " ";
                sqlkir = sqlkir + " select treat_item_id, initcap(treat_item_name) || ' : ' || to_char(treat_item_price) treat_item_name  from KLINIK.cs_treatment_item a ";
                sqlkir = sqlkir + "  where status = 'A'  and TREAT_TYPE_ID = 'TRT01' and TREAT_GROUP_ID = '" + sttype + "' ORDER BY 2";

                OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraly = new OleDbDataAdapter(sqlkir, oraConnectly);
                DataTable dtly = new DataTable();
                adOraly.Fill(dtly);
                listLaya2.Clear();
                for (int i = 0; i < dtly.Rows.Count; i++)
                {
                    listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
                }

                //OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                //OleDbDataAdapter adOraf = new OleDbDataAdapter(sqlkir, oraConnectf);
                //DataTable dt3 = new DataTable();
                //adOraf.Fill(dt3);
                //listLaya2.Clear();
                //for (int i = 0; i < dt3.Rows.Count; i++)
                //{
                //    listLaya2.Add(new Layanan() { layananCode = dt3.Rows[i]["treat_item_id"].ToString(), layananName = dt3.Rows[i]["treat_item_name"].ToString() }); 
                //}

                //LookLynan.DataSource = listLaya2;
                //LookLynan.ValueMember = "statCode";
                //LookLynan.DisplayMember = "statName";

                //LookLynan.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                //LookLynan.DropDownRows = listLaya2.Count + 1;
                //LookLynan.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                //LookLynan.AutoSearchColumnIndex = 1;
                //LookLynan.NullText = "";
                //gridView1.Columns[16].ColumnEdit = LookLynan; 
            }

            if (e.Column.Caption == "Code Class ID" || e.Column.Caption == "Code ID" || e.Column.Caption == "Code Nm" || e.Column.Caption == "Order")
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

        private void gvJualObat_RowClick(object sender, RowClickEventArgs e)
        {
            if (gvJualObat.RowCount < 1)
            {
                btnMedCan.Enabled = false;
                btnMedDel.Enabled = false;
                btnMedAdd.Enabled = false;
                btnMedSave.Enabled = false;
                return;
            } 

            string s_que = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, gvJualObat.Columns[1]).ToString();
            string s_pay = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, gvJualObat.Columns[19]).ToString();
            LoadDataResep(s_que);
            if(s_pay.ToString().Equals("Closed"))
            {
                btnMedCan.Enabled = false;
                btnMedDel.Enabled = false;
                btnMedAdd.Enabled = false;
                btnMedSave.Enabled = false;
            }
            else
            {
                btnMedCan.Enabled = true;
                btnMedDel.Enabled = true;
                btnMedAdd.Enabled = true;
                btnMedSave.Enabled = true;
            }
           
        }

        private void btnMedAdd_Click(object sender, EventArgs e)
        {
            gvObatJual.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gvObatJual.AddNewRow();
        }

        private void gvObatJual_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView; 
            view.SetRowCellValue(e.RowHandle, view.Columns["ACTION"], "I");
        }

        private void btnMedSave_Click(object sender, EventArgs e)
        {

            string kir_id ="", r_id = "", kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", RECEIPT_ID = "";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                if (gvJualObat.RowCount > 0)
                {
                    kir_id = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, gvJualObat.Columns[1]).ToString();
                    object tgl = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, "REGIS_DATE");

                    if (!kir_id.ToString().Equals(""))
                    { 
                        bool save = false; int ssave = 0;
                        for (int i = 0; i < gvObatJual.RowCount; i++)
                        {
                            r_id = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[0]).ToString();
                            kode = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[1]).ToString();
                            dosis = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[4]).ToString();
                            info = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[5]).ToString();
                            jumlah = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[7]).ToString();
                            stok = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[6]).ToString();
                            con = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[10]).ToString();
                            action = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[9]).ToString();
                            harga = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[12]).ToString();
                            hari = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[11]).ToString();
                            jph = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[13]).ToString();
                            info_dosis = gvObatJual.GetRowCellValue(i, gvObatJual.Columns[14]).ToString();

                            string dte = "", sql = " ";
                           
                            if (tgl != null && tgl is DateTime)
                            {
                                DateTime selectedDateTime = (DateTime)tgl;
                                dte = selectedDateTime.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                DateTime selectedDateTime = DateTime.Now;
                                dte = selectedDateTime.ToString("yyyy-MM-dd");
                            }

                            if (r_id.ToString().Equals(""))
                            {
                                sql = "";
                                sql = sql + " Insert into KLINIK.cs_receipt ( RM_NO, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, INS_JAM, ins_date, ins_emp, GRID_NAME, ATT3_RECIEPT) ";
                                sql = sql + " values(  '-', to_date('" + dte + "', 'yyyy-MM-dd'), '" + kode + "', '" + dosis + "', '" + jumlah + "', ";
                                sql = sql + "   '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "',   '" + FN.strVal(gvObatJual, i, "INS_JAM") + "' , sysdate, '" + DB.vUserId + "' , 'gvObatJual', " + kir_id + " ) ";
                                ssave = 2;
                                ORADB.Execute(ORADB.XE, sql);

                            }
                            else
                            {
                                ssave = 1;
                                if (con.ToString().Equals("N"))  
                                {
                                    sql = "";
                                    sql = sql + " Update  KLINIK.cs_receipt ";
                                    sql = sql + "    set  insp_date = to_date('" + dte + "', 'yyyy-MM-dd'),  INS_JAM = '" + FN.strVal(gvObatJual, i, "INS_JAM") + "' , med_qty = '" + jumlah + "', dosis =  '" + info_dosis + "', UPD_DATE = sysdate, UPD_EMP = '" + DB.vUserId + "'  ";
                                    sql = sql + "  where  RECEIPT_ID =  '" + r_id + "'  and ATT3_RECIEPT =  " + kir_id + "   ";

                                    ORADB.Execute(ORADB.XE, sql);
                                    ssave = 3;
                                }
                            }
                        }

                        if (ssave == 1)
                        {
                            MessageBox.Show("Pembelian Obat Tidak Dapat Diganti, Karena Sudah Confirm!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (ssave == 2)
                        {
                            MessageBox.Show("Pembelian Obat Berhasil disimpan!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataResep(kir_id);
                        }
                        else if (ssave == 3)
                        {
                            MessageBox.Show("Pembelian Obat Berhasil di ubah!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataResep(kir_id);
                        }

                    }
                   
                }
            }
            catch (Exception ex)
            {
                FN.errosMsg(ex.Message, "Error");
            }
        }

        private void gvObatJual_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();

            if (a.ToString().Equals(""))
                return;

            string dte = "";

            DateTime selectedDateTime = DateTime.Now;
            dte = selectedDateTime.ToString("yyyy-MM-dd");

            if (e.Column.Caption == "Nama Obat" && (a.Substring(0, 2) == "BP" || a.Substring(0, 2) == "UM" || a.Substring(0, 2) == "ME"))
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns["ACTION"]).ToString();
                string sql_medcd = " ", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";

                //dtStock = null;
                sql_medcd = " select " +
                            " max(klinik.FN_CS_INIT_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + dte.ToString() + "','yyyy-mm-dd'),'" + a + "')) stock from dual ";

                datstock = ConnOra.Data_Table_ora(sql_medcd);

                if (datstock.Rows.Count > 0)
                    cek_stok = datstock.Rows[0]["stock"].ToString();
                else
                    cek_stok = "0"; 
                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " + 
                          " from KLINIK.cs_medicine a  " +
                          " where status = 'A'  " +
                          " and med_cd = '" + a + "' ";

                DataTable dt = ConnOra.Data_Table_ora(sql_med); 

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                sql_for = " select  min(formula_id) formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1 and BPJS_COVER ='N' and MINUS_STOK ='Y' and MED_GROUP in('OBAT','OTC')  and  b.med_cd = '" + med_cd + "'   group by  initcap(formula) , initcap(b.med_name) ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for); 
                
                listFormula2.Clear();
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                }
                if (dtf.Rows.Count == 1)
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], dtf.Rows[0]["formula_id"].ToString());
                else
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], "");

                //view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);

                //view.SetRowCellValue(e.RowHandle, view.Columns[4], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I"); 
                    view.SetRowCellValue(e.RowHandle, view.Columns[2], med_group);
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U"); 
                    view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], med_stok); 
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "N");
                }
                view.Columns[6].OptionsColumn.ReadOnly = true;
                view.Columns[10].OptionsColumn.ReadOnly = true;
            }

            if (e.Column.Caption == "Formula")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string reg_dt = dte.ToString();
                string rm = dte.ToString();
                string que = dte.ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                string kode = "", sql_pilihan = "";

                if (stat == "I")
                { 
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                }
                else
                {
                    sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' ";
                    DataTable dtf = ConnOra.Data_Table_ora(sql_pilihan); 
                    if (dtf.Rows.Count > 0)
                    {
                        kode = dtf.Rows[0]["med_cd"].ToString(); 
                    }
                    else
                    {
                        kode = "";
                    }

                    if (kode == medicine_cd)
                    {
                        view.SetRowCellValue(e.RowHandle, view.Columns[7], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[11], "");
                        view.SetRowCellValue(e.RowHandle, view.Columns[12], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[13], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return; 
                    }
                } 
            }

            if (e.Column.Caption == "Qty")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                string cstock = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                int tot_hari = 0, tot_harga = 0, istock = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' ";
                DataTable dtf = ConnOra.Data_Table_ora(sql_for);

                if (dtf.Rows.Count > 0)
                {
                    med_price = dtf.Rows[0]["med_price"].ToString();
                    qty = dtf.Rows[0]["qty"].ToString();
                }
                else
                {
                    med_price = "0";
                    qty = "0";
                }

                if (tmp_hari == "")
                {
                    tmp_hari = "1";
                }

                tot_hari = Convert.ToInt16(tmp_hari); //Convert.ToInt16(tmp_hari) * Convert.ToInt16(qty);
                tot_harga = Convert.ToInt32(med_price); //Convert.ToInt16(tmp_hari) *

                if (!cstock.ToString().Equals(""))
                {
                    istock = Convert.ToInt32(cstock);
                    if (istock - Convert.ToInt32(qty) < 0)
                    {
                        MessageBox.Show("Stok Obat Kosong. Tidak dapat dipilih..!!!");
                        view.DeleteRow(view.FocusedRowHandle);
                        return;
                    }
                    else
                    {
                        if (tmp_stat == "I")
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                        else
                        {
                            //view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                            view.SetRowCellValue(e.RowHandle, view.Columns[12], tot_harga.ToString());
                            view.SetRowCellValue(e.RowHandle, view.Columns[13], qty);
                            view.SetRowCellValue(e.RowHandle, view.Columns[11], tot_hari.ToString());
                        }
                    }
                }
            }
        }

        private void gvObatJual_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvObatJual_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Jml")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Stok")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);

                if (stok != "")
                {
                    if (Convert.ToInt16(stok) == 0)
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

            if (e.Column.Caption == "Confirm")
            {
                string con = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);

                if (con == "Y")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Code Class ID" || e.Column.Caption == "Code ID" || e.Column.Caption == "Code Nm" || e.Column.Caption == "Order")
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
                string sql_delete = "", p_class = "", p_kode = "";

                p_class = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, gvJualObat.Columns[1]).ToString();
                p_kode = gvJualObat.GetRowCellValue(gvJualObat.FocusedRowHandle, gvJualObat.Columns[2]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " update cs_code_data set status = 'I' ";
                sql_delete = sql_delete + " where code_class_id = '" + p_class + "' and code_id = '" + p_kode + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gvJualObat.DeleteRow(gvJualObat.FocusedRowHandle);
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
            if (gvJualObat.RowCount > 0)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XLS (*.xls)|*.xlsx",
                    FileName = "dosis_obat.xls",
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
                    gJualObat.ExportToXls(saveDialog.FileName);
                }
            }
            else
            {
                MessageBox.Show("Data tidak ditemukan");
            }
        }
    }
    
}