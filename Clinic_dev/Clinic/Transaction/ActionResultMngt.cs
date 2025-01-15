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
    public partial class ActionResultMngt : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Poli> listPoli = new List<Poli>();
        List<Stat> listStat = new List<Stat>();
        List<Stat> listStat2 = new List<Stat>();
        List<Layanan> listLayanan = new List<Layanan>();

        public string  v_status = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string upd_col = "", s_policd = "";

        public ActionResultMngt()
        {
            InitializeComponent();
        }

        private void FirstInspection_Load(object sender, EventArgs e)
        {
            initData();
            LoadData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "ActionResultMngt");
        }
        private void initData()
        {
            dtgl.Text = today;
            string sql_poli = " select poli_cd, poli_name from KLINIK.cs_policlinic where status = 'A'  ";
            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_poli, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);
            listPoli.Clear();
            listPoli.Add(new Poli() { poliCode = "", poliName = "Pilih" });
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                listPoli.Add(new Poli() { poliCode = dt2.Rows[i]["poli_cd"].ToString(), poliName = dt2.Rows[i]["poli_name"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            }

            luPoliCd.Properties.DataSource = listPoli;
            luPoliCd.Properties.ValueMember = "poliCode";
            luPoliCd.Properties.DisplayMember = "poliCode";

            luPoliCd.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luPoliCd.Properties.DropDownRows = listPoli.Count;
            luPoliCd.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luPoliCd.Properties.AutoSearchColumnIndex = 1;
            luPoliCd.Properties.NullText = "Pilih";

            listStat.Clear();
            listStat.Add(new Stat() { statCode = "RSV", statName = "Reservasi" });
            listStat.Add(new Stat() { statCode = "NUR", statName = "Pemeriksaan Awal" });
            listStat.Add(new Stat() { statCode = "RSV", statName = "Reservasi" });
            listStat.Add(new Stat() { statCode = "INS", statName = "Pemeriksaan" });
            listStat.Add(new Stat() { statCode = "MED", statName = "Obat" });
            listStat.Add(new Stat() { statCode = "CLS", statName = "Selesai" });
            listStat.Add(new Stat() { statCode = "PAY", statName = "Pembeyaran" });
            listStat.Add(new Stat() { statCode = "CAN", statName = "Batal" });

            listLayanan.Clear();
            string sql_laya = " select treat_item_id, treat_item_name from KLINIK.cs_treatment_item where treat_group_id = 'TRG08'  ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_laya, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listLayanan.Add(new Layanan() { layananCode = dt.Rows[i]["treat_item_id"].ToString(), layananName = dt.Rows[i]["treat_item_name"].ToString() });
            }

            listStat2.Clear();
            listStat2.Add(new Stat() { statCode = "DOC", statName = "Dokter" });
            listStat2.Add(new Stat() { statCode = "MID", statName = "Obgyn" });
            listStat2.Add(new Stat() { statCode = "ETC", statName = "Lain2" });
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoadData();
            loadDataAction("", "", "", "");
        }

        private void LoadData()
        {
            string SQL;

            SQL = "";
            //SQL = SQL + Environment.NewLine + "select action, patient_no, rm_no, visit_date, que01, tgl, name, status, poli_cd, purpose, visit_remark,  ";
            //SQL = SQL + Environment.NewLine + "case when TIME_INSPECTION is null then 'N' else 'Y' end stat_proses, ID_VISIT, TYPE_PATIENT,GENDER ";
            //SQL = SQL + Environment.NewLine + "from ( ";
            //SQL = SQL + Environment.NewLine + "select 'S' action, a.patient_no, c.name, b.rm_no, a.visit_date, a.que01,  ";
            //SQL = SQL + Environment.NewLine + "to_char(visit_date,'yyyy-mm-dd') tgl, DECODE(a.status,'DON',(SELECT Z.pay_status FROM KLINIK.cs_treatment_head Z  WHERE a.id_visit = Z.id_visit ),a.status) status, a.poli_cd, a.purpose, a.visit_remark, ";
            //SQL = SQL + Environment.NewLine + "(select count(0) from KLINIK.cs_action  ";
            //SQL = SQL + Environment.NewLine + "where rm_no=b.rm_no ";
            //SQL = SQL + Environment.NewLine + "and visit_no=a.que01 ";
            //SQL = SQL + Environment.NewLine + "and insp_date=trunc(a.visit_date)) cnt,a.ID_VISIT,TYPE_PATIENT, a.TIME_INSPECTION,c.GENDER ";
            //SQL = SQL + Environment.NewLine + "from KLINIK.cs_visit a  ";
            //SQL = SQL + Environment.NewLine + "join KLINIK.cs_patient b on (a.patient_no=b.patient_no)  ";
            //SQL = SQL + Environment.NewLine + "join KLINIK.cs_patient_info c on (a.patient_no=c.patient_no)  ";
            //SQL = SQL + Environment.NewLine + "where 1=1  ";
            //SQL = SQL + Environment.NewLine + "and to_char(a.visit_date,'yyyy-mm-dd')='" + dtgl.Text + "'  ";
            //SQL = SQL + Environment.NewLine + "and a.status not in ('CAN') ) ";
            //SQL = SQL + Environment.NewLine + "where 1=1 ";
            //SQL = SQL + Environment.NewLine + "  and USED_BY ='LAB' ";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select action, patient_no, rm_no, visit_date, que01, tgl, name, status, poli_cd, purpose, visit_remark,   ";
            SQL = SQL + Environment.NewLine + "       case when TIME_INSPECTION is null then 'N' else 'Y' end stat_proses, ID_VISIT, TYPE_PATIENT,GENDER  ";
            SQL = SQL + Environment.NewLine + "from (  ";
            SQL = SQL + Environment.NewLine + "        select 'S' action, a.patient_no, c.name, b.rm_no, a.visit_date, a.que01,   ";
            SQL = SQL + Environment.NewLine + "                to_char(a.visit_date,'yyyy-mm-dd') tgl, DECODE(a.status,'DON',(SELECT Z.pay_status FROM KLINIK.cs_treatment_head Z  WHERE a.id_visit = Z.id_visit ),a.status) status, a.poli_cd, a.purpose, a.visit_remark,  ";
            SQL = SQL + Environment.NewLine + "                (select count(0) from KLINIK.cs_action   ";
            SQL = SQL + Environment.NewLine + "        where rm_no=b.rm_no  ";
            SQL = SQL + Environment.NewLine + "          and visit_no=a.que01  ";
            SQL = SQL + Environment.NewLine + "          and insp_date=trunc(a.visit_date)) cnt,a.ID_VISIT,TYPE_PATIENT, a.TIME_INSPECTION,c.GENDER  ";
            SQL = SQL + Environment.NewLine + "        from KLINIK.cs_visit a   ";
            SQL = SQL + Environment.NewLine + "        join KLINIK.cs_patient b on (a.patient_no=b.patient_no)   ";
            SQL = SQL + Environment.NewLine + "        join KLINIK.cs_patient_info c on (a.patient_no=c.patient_no)   ";
            SQL = SQL + Environment.NewLine + "        join KLINIK.cs_treatment_head d on (a.ID_VISIT = d.ID_VISIT)   ";
            SQL = SQL + Environment.NewLine + "        join KLINIK.cs_treatment_detail e on (d.head_id=e.head_id)   ";
            SQL = SQL + Environment.NewLine + "        join KLINIK.cs_treatment_item f on (e.treat_item_id=f.treat_item_id)   ";
            SQL = SQL + Environment.NewLine + "        where 1=1   ";
            SQL = SQL + Environment.NewLine + "        and to_char(a.visit_date,'yyyy-mm-dd')='" + dtgl.Text + "'   ";
            SQL = SQL + Environment.NewLine + "        and a.status not in ('CAN') ";
            SQL = SQL + Environment.NewLine + "        and f.USED_BY ='LAB'  )  ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";


            if (luPoliCd.Text == "Pilih")
            {
                SQL = SQL + Environment.NewLine + "and poli_cd like '%%' ";
            }
            else
            {
                SQL = SQL + Environment.NewLine + "and poli_cd like '%" + luPoliCd.GetColumnValue("poliCode").ToString() + "%' ";
            }

            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 40;
                gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();

                gridView1.Columns[0].Caption = "Action";
                gridView1.Columns[1].Caption = "Pasien No";
                gridView1.Columns[2].Caption = "RM No";
                gridView1.Columns[3].Caption = "Visit Date";
                gridView1.Columns[4].Caption = "Visit No";
                gridView1.Columns[5].Caption = "Tgl";
                gridView1.Columns[6].Caption = "Nama";
                gridView1.Columns[7].Caption = "Status Pemeriksaan";
                gridView1.Columns[8].Caption = "Poli";
                gridView1.Columns[9].Caption = "Berobat";
                gridView1.Columns[10].Caption = "Remark";
                gridView1.Columns[11].Caption = "Konfirmasi";
                gridView1.Columns[12].Caption = "ID_VISIT";
                gridView1.Columns[13].Caption = "TYPE_PATIENT";
                gridView1.Columns[14].Caption = "GENDER";

                RepositoryItemLookUpEdit pLookup = new RepositoryItemLookUpEdit();
                pLookup.DataSource = listPoli;
                pLookup.ValueMember = "poliCode";
                pLookup.DisplayMember = "poliName";

                pLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                pLookup.DropDownRows = listPoli.Count;
                pLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                pLookup.AutoSearchColumnIndex = 1;
                pLookup.NullText = "";
                gridView1.Columns[8].ColumnEdit = pLookup;

                RepositoryItemLookUpEdit sLookup = new RepositoryItemLookUpEdit();
                sLookup.DataSource = listStat;
                sLookup.ValueMember = "statCode";
                sLookup.DisplayMember = "statName";

                sLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                sLookup.DropDownRows = listStat.Count;
                sLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                sLookup.AutoSearchColumnIndex = 1;
                sLookup.NullText = "";
                gridView1.Columns[7].ColumnEdit = sLookup;

                RepositoryItemLookUpEdit stLookup = new RepositoryItemLookUpEdit();
                stLookup.DataSource = listStat2;
                stLookup.ValueMember = "statCode";
                stLookup.DisplayMember = "statName";

                stLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                stLookup.DropDownRows = listStat2.Count;
                stLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                stLookup.AutoSearchColumnIndex = 1;
                stLookup.NullText = "";
                gridView1.Columns[9].ColumnEdit = stLookup;

                RepositoryItemLookUpEdit kLookup = new RepositoryItemLookUpEdit();
                kLookup.DataSource = listLayanan;
                kLookup.ValueMember = "layananCode";
                kLookup.DisplayMember = "layananName";

                kLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                kLookup.DropDownRows = listLayanan.Count;
                kLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                kLookup.AutoSearchColumnIndex = 1;
                kLookup.NullText = "";
                gridView1.Columns[10].ColumnEdit = kLookup;

                //gridView1.Columns[5].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[6].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[7].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[8].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[9].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[10].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[11].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;
                gridView1.Columns[2].Visible = false;
                gridView1.Columns[3].Visible = false;
                gridView1.Columns[4].Visible = false;
                gridView1.Columns[12].Visible = false;
                gridView1.Columns[13].Visible = false;
                gridView1.Columns[14].Visible = false;
                //if (v_status == "SYS" || v_status == "DOH" || v_status == "DOC")
                //{
                //    gridView1.Columns[10].Visible = true;
                //}
                //else
                //{
                //    gridView1.Columns[10].Visible = false;
                //}

                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Berobat")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);

                if (kk == "Dokter")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;

                }
                else if (kk == "Obgyn")
                {
                    e.Appearance.BackColor = Color.Salmon;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (kk == "Lain2")
                {
                    e.Appearance.BackColor = Color.MediumPurple;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                
            }

            if (e.Column.Caption == "Konfirmasi")
            {
                string st = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);

                if (st == "N")
                {
                    e.Appearance.BackColor = Color.Yellow;
                    e.Appearance.ForeColor = Color.Black;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (st == "Y")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }
        
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Status Pemeriksaan")
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

        

        private void luPoliCd_EditValueChanged(object sender, EventArgs e)
        {
            if (luPoliCd.Text != "")
            {
                string sql = "", poli_name="";
                sql = " select poli_name from KLINIK.cs_policlinic where poli_cd='" + luPoliCd.GetColumnValue("poliCode").ToString() + "' ";
                OleDbConnection oraConnectp = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOrap = new OleDbDataAdapter(sql, oraConnectp);
                DataTable dtp = new DataTable();
                adOrap.Fill(dtp);
                if (dtp.Rows.Count > 0)
                {
                    poli_name = dtp.Rows[0]["poli_name"].ToString();
                    lPoliNm.Text = poli_name;
                }
                else
                {
                    lPoliNm.Text = "-";
                }
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            //gridView1.Columns[0].Caption = "Action";
            //gridView1.Columns[1].Caption = "Pasien No";
            //gridView1.Columns[2].Caption = "RM No";
            //gridView1.Columns[3].Caption = "Visit Date";
            //gridView1.Columns[4].Caption = "Visit No";
            //gridView1.Columns[5].Caption = "Tgl";
            //gridView1.Columns[6].Caption = "Nama";
            //gridView1.Columns[7].Caption = "Status Pemeriksaan";
            //gridView1.Columns[8].Caption = "Poli";
            //gridView1.Columns[9].Caption = "Berobat";
            //gridView1.Columns[10].Caption = "Remark";
            //gridView1.Columns[11].Caption = "Konfirmasi";

            string s_pasno = "", s_rmno = "", s_vdate = "", s_visitno = "", berobat= "", konf = "", rmk = "";
            string sql_cnt = "", ncnt = "", sql_cnt2 = "", ncnt2 = "", visitid = "", insflag = "", vstatus = "";
            string rm_type = "", p1 = "", p2 = "", teks = "", callid = "", gnder ="", sname ="", sql_all ="";

            s_pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            s_rmno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
            s_vdate = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();
            s_visitno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
            berobat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[9]).ToString();
            rmk = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            konf = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();
            visitid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[12]).ToString();
            insflag = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();
            gnder = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();


            sql_all = "";
            sql_all = sql_all + @" select a.CALL_ID, TYPE_INS, a.que
                                    from KLINIK.CS_CALL_LOG a, KLINIK.cs_visit b
                                    where a.que = b.que01
                                    AND a.que = '" + s_visitno + @"'    
                                    AND TRUNC(A.INS_DATE) = TRUNC(VISIT_DATE)  ";

            OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_all, oraConnect5);
            DataTable dt5 = new DataTable();
            adOra5.Fill(dt5);
            if (dt5.Rows.Count > 0)
            {
                rm_type = dt5.Rows[0]["TYPE_INS"].ToString();
                callid = dt5.Rows[0]["CALL_ID"].ToString();
            }



            if (berobat.ToString().Equals("ETC"))
                vstatus = "PAY";
            else
                vstatus = "MED";

            if (dtgl.Text != "")
            {
                sql_cnt = " select count(0) cnt from KLINIK.cs_treatment_head where to_char(visit_date,'yyyy-mm-dd') = '" + s_vdate + "' and visit_no = '" + s_visitno + "' and rm_no = '" + s_rmno + "' " + " and status = 'OPN' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                ncnt = dt.Rows[0]["cnt"].ToString();
                if (Convert.ToInt32(ncnt) > 0)
                {
                    //MessageBox.Show("Employee ID " + nik + " sudah terdaftar.");
                    OleDbConnection oraConnectTrans2 = ConnOra.Create_Connect_Ora();
                    OleDbCommand command2 = new OleDbCommand();
                    OleDbTransaction trans2 = null;

                    command2.Connection = oraConnectTrans2;
                    oraConnectTrans2.Open();

                    try
                    {

                        trans2 = oraConnectTrans2.BeginTransaction(IsolationLevel.ReadCommitted);
                        command2.Connection = oraConnectTrans2;
                        command2.Transaction = trans2;

                        command2.CommandText = " update KLINIK.cs_visit set status = '" + vstatus + "', TIME_INSPECTION = sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate, MENU_LAST_UPDATED = 'ActionResultMngt', M_UPDATED_DATE = sysdate where patient_no = '" + s_pasno + "' and id_visit = '" + visitid + "'   ";
                        command2.ExecuteNonQuery();


                        if (gnder.ToString().Equals("P"))
                        {
                            p1 = "Ibu ";
                        }
                        else
                        {
                            p1 = "Bapak ";
                        }

                        p2 = sname;

                        teks = "Nomor Antrian " + s_visitno + " " + p1 + p2 + " silahkan menuju ke Kasir";

                        command2.CommandText = " UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'W', type_ins ='" + vstatus + "', stat ='Open', param = '" + teks + "' WHERE CALL_ID = " + callid + " ";
                        command2.ExecuteNonQuery(); 

                        //MessageBox.Show("Employee ID " + nik + " sudah terdaftar.");
                        trans2.Commit();
                        MessageBox.Show("Data Berhasil Di Proses.");
                    }
                    catch (Exception ex)
                    {
                        trans2.Rollback();
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                    oraConnectTrans2.Close();
                }
                else
                {
                    sql_cnt2 = " select count(0) cnt from KLINIK.cs_action where to_char(visit_dt,'yyyy-mm-dd') = '" + s_vdate + "' and visit_no = '" + s_visitno + "' and rm_no = '" + s_rmno + "' " + " and to_char(insp_date,'yyyy-mm-dd') = '" + dtgl.Text + "' ";
                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt2, oraConnect2);
                    DataTable dt2 = new DataTable();
                    adOra2.Fill(dt2);
                    ncnt2 = dt2.Rows[0]["cnt"].ToString();
                    if (Convert.ToInt32(ncnt2) > 0)
                    {
                        return;
                    }
                
                    string sql_seq = "", seq_val = "", sql_tmp = "";
                    sql_seq = " select CS_TREATMENT_HEAD_SEQ.nextval seq from dual ";
                    OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_seq, oraConnect3);
                    DataTable dt3 = new DataTable();
                    adOra3.Fill(dt3);
                    seq_val = dt3.Rows[0]["seq"].ToString();

                    string sql_seq2 = "", seq_val2 = "";
                    sql_seq2 = " select CS_TREATMENT_DETAIL_SEQ.nextval seq from dual ";
                    OleDbConnection oraConnects = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOras = new OleDbDataAdapter(sql_seq2, oraConnects);
                    DataTable dts = new DataTable();
                    adOras.Fill(dts);
                    seq_val2 = dts.Rows[0]["seq"].ToString();

                    OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                    OleDbCommand command = new OleDbCommand();
                    OleDbTransaction trans = null;

                    command.Connection = oraConnectTrans;
                    oraConnectTrans.Open();

                    try
                    {
                        trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                        command.Connection = oraConnectTrans;
                        command.Transaction = trans;

                        command.CommandText = " insert into cs_treatment_head (head_id, rm_no, patient_no, visit_date, visit_no, treat_type_id, status, remarks, pay_status, ins_date, ins_emp, ID_VISIT, INSU_FLAG) values ('" + seq_val + "', '" + s_rmno + "', '" + s_pasno + "', to_date('" + s_vdate + "', 'yyyy-mm-dd'), '" + s_visitno + "', 'TRT02', 'OPN', 'dari pendaftaran', 'OPN', sysdate, '" + DB.vUserId + "', " + visitid + ", '" + insflag + "') ";
                        command.ExecuteNonQuery();

                        command.CommandText = " update cs_visit set status = '" + vstatus + "', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + s_pasno + "' and to_char(visit_date,'yyyy-mm-dd') = '" + s_vdate + "' and ID_VISIT = " + visitid + " ";
                        command.ExecuteNonQuery();

                        sql_tmp = "";
                        sql_tmp = sql_tmp + " insert into cs_treatment_detail ";
                        sql_tmp = sql_tmp + " select " + seq_val2 + " det_id, " + seq_val + " head_id,  b.treat_item_id, to_date('" + dtgl.Text + "', 'yyyy-mm-dd') insp_date, ";
                        sql_tmp = sql_tmp + "         1 treat_qty, 'dari pendaftaran' remark, sysdate ins_date, '" + DB.vUserId + "' ins_emp, ";
                        sql_tmp = sql_tmp + "         null upd_date, null upd_emp, b.treat_item_price,treat_item_price as ttlprice, TO_char(sysdate, 'HH24:MI') tjam, 'gridView1' ,null,null,null,'Y'";
                        sql_tmp = sql_tmp + "   from KLINIK.cs_treatment_item b  ";
                        sql_tmp = sql_tmp + "  where 1=1";
                        sql_tmp = sql_tmp + "    and b.treat_item_id = '" + rmk + "' ";
                        command.CommandText = sql_tmp;
                        command.ExecuteNonQuery();

                        command.CommandText = " insert into cs_action (act_id, rm_no, insp_date, visit_dt, visit_no, detail_id, ins_date, ins_emp) values ( CS_ACTION_SEQ.nextval, '" + s_rmno + "', to_date('" + dtgl.Text + "', 'yyyy-mm-dd'), to_date('" + s_vdate + "', 'yyyy-mm-dd'), '" + s_visitno + "', '" + seq_val2 + "', sysdate, '" + DB.vUserId + "') ";
                        command.ExecuteNonQuery();

                        trans.Commit();
                        //MessageBox.Show(sql_insert);
                        //MessageBox.Show("Query Exec : " + sql_insert);
                        MessageBox.Show("Data Berhasil disimpan.");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("ERROR: " + ex.Message);
                    }

                    oraConnectTrans.Close();
                }
            }
            LoadData();
        }

        private void btnSaveAnam_Click(object sender, EventArgs e)
        {
            string sql_update = "", stat = "";
            string pasno="", que="", vdate="", statProses = "", berobat = "";

            if (MessageBox.Show("Anda yakin akan melakukan proses batal?",
                     "Message",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                vdate = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();
                que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
                stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[7]).ToString();
                berobat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[9]).ToString();
                statProses = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();

                if (berobat != "ETC")
                {
                    MessageBox.Show("Silahkan melakukan proses pembatalan di menu pemeriksaan dokter");
                    return;
                }

                if (berobat == "ETC" && statProses == "Y")
                {
                    MessageBox.Show("Silahkan melakukan proses pembatalan di menu tagihan");
                    return;
                }

                sql_update = "";

                sql_update = sql_update + " update cs_visit";
                sql_update = sql_update + " set status = 'CAN',  ";
                sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                sql_update = sql_update + " where patient_no = '" + pasno + "' and to_char(visit_date,'yyyy-mm-dd') = '" + vdate + "' and que01 = '" + que + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    //LoadData();
                    MessageBox.Show("Data Berhasil dibatalkan");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                LoadData();
            }
            
        }
        

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridView View = sender as GridView;

            string s_pasno = "", s_rm_no = "", s_visit_dt = "", s_visit_no = "", s_remark = "", s_nama = "";
            
            s_pasno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            s_rm_no = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            s_visit_dt = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);
            s_visit_no = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
            s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
            s_remark = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);

            loadDataAction(s_rm_no, s_visit_dt, s_visit_no, s_nama);
        }

        private void loadDataAction(string p_rm_no, string p_visit_dt, string v_visit_no, string v_nama)
        {
            string SQL = "";

            SQL = SQL + Environment.NewLine + " select 'S' action, act_id, rm_no, '"+ v_nama + "' nama,  ";
            SQL = SQL + Environment.NewLine + "        to_char(visit_dt,'yyyy-mm-dd') visit_dt, ";
            SQL = SQL + Environment.NewLine + "        to_char(insp_date,'yyyy-mm-dd') insp_date, ";
            SQL = SQL + Environment.NewLine + "        to_char(treat_date,'yyyy-mm-dd') treat_date, ";
            SQL = SQL + Environment.NewLine + "        'REG' info, act_name, act_remark,  ";
            SQL = SQL + Environment.NewLine + "        treat_item_id,  treat_item_price, ";
            SQL = SQL + Environment.NewLine + "        null adj_type, a.detail_id, a.detail_adj_id ";
            SQL = SQL + Environment.NewLine + "   from KLINIK.cs_action a ";
            SQL = SQL + Environment.NewLine + "   join KLINIK.cs_treatment_detail b on (a.detail_id=b.detail_id) ";
            SQL = SQL + Environment.NewLine + "  where 1=1 ";
            SQL = SQL + Environment.NewLine + "    and rm_no='"+ p_rm_no + "' ";
            SQL = SQL + Environment.NewLine + "    and visit_no='" + v_visit_no + "' ";
            SQL = SQL + Environment.NewLine + "    and to_char(visit_dt,'yyyy-mm-dd')='"+ p_visit_dt + "'  ";
            SQL = SQL + Environment.NewLine + "    and a.detail_adj_id is null ";
            SQL = SQL + Environment.NewLine + "  union ";
            SQL = SQL + Environment.NewLine + " select 'S' action, act_id, rm_no, '" + v_nama + "' nama, ";
            SQL = SQL + Environment.NewLine + "        to_char(visit_dt,'yyyy-mm-dd') visit_dt, ";
            SQL = SQL + Environment.NewLine + "        to_char(insp_date,'yyyy-mm-dd') insp_date, ";
            SQL = SQL + Environment.NewLine + "        to_char(treat_date,'yyyy-mm-dd') treat_date, ";
            SQL = SQL + Environment.NewLine + "        'ADJ' info, act_name, act_remark,  ";
            SQL = SQL + Environment.NewLine + "        treat_item_id, treat_item_price, ";
            SQL = SQL + Environment.NewLine + "        b.adj_type, a.detail_id, a.detail_adj_id ";
            SQL = SQL + Environment.NewLine + "   from KLINIK.cs_action a ";
            SQL = SQL + Environment.NewLine + "   left join KLINIK.cs_treatment_detail_adj b on (a.detail_adj_id=b.detail_adj_id) ";
            SQL = SQL + Environment.NewLine + "  where 1=1 ";
            SQL = SQL + Environment.NewLine + "    and rm_no='" + p_rm_no + "' ";
            SQL = SQL + Environment.NewLine + "    and visit_no='" + v_visit_no + "' ";
            SQL = SQL + Environment.NewLine + "    and to_char(visit_dt,'yyyy-mm-dd')='" + p_visit_dt + "'  ";
            SQL = SQL + Environment.NewLine + "    and adj_type <> 'D' "; 
            SQL = SQL + Environment.NewLine + "  union all        ";
            SQL = SQL + Environment.NewLine + " SELECT 'S' action, ID_VISIT act_id,  ";
            SQL = SQL + Environment.NewLine + "        (  ";
            SQL = SQL + Environment.NewLine + "          SELECT MAX(rm_no)   ";
            SQL = SQL + Environment.NewLine + "            FROM cs_patient   ";
            SQL = SQL + Environment.NewLine + "           WHERE status = 'A'   ";
            SQL = SQL + Environment.NewLine + "             AND group_patient = c.poli_group   ";
            SQL = SQL + Environment.NewLine + "             AND patient_no = a.patient_no ";
            SQL = SQL + Environment.NewLine + "        ) AS rm_no, '" + v_nama + "' nama,   ";
            SQL = SQL + Environment.NewLine + "        to_char(visit_date,'yyyy-mm-dd') visit_dt,  ";
            SQL = SQL + Environment.NewLine + "        to_char(visit_date,'yyyy-mm-dd') insp_date,  ";
            SQL = SQL + Environment.NewLine + "        to_char(visit_date,'yyyy-mm-dd') treat_date,  ";
            SQL = SQL + Environment.NewLine + "        'REG' info, null act_name, null act_remark,   ";
            SQL = SQL + Environment.NewLine + "        to_number(visit_remark) treat_item_id,   treat_item_price,  ";
            SQL = SQL + Environment.NewLine + "        null adj_type, to_number(visit_remark) detail_id, null detail_adj_id   ";
            SQL = SQL + Environment.NewLine + "    FROM cs_visit a JOIN cs_patient_info b ON a.patient_no = b.patient_no   ";
            SQL = SQL + Environment.NewLine + "         LEFT JOIN cs_policlinic c ON (a.poli_cd = c.poli_cd AND c.status = 'A')   ";
            SQL = SQL + Environment.NewLine + "         join KLINIK.CS_TREATMENT_ITEM d on (to_number(visit_remark) =d.TREAT_ITEM_ID)  ";
            SQL = SQL + Environment.NewLine + "   WHERE 1 = 1   ";
            SQL = SQL + Environment.NewLine + "     AND to_char(visit_date,'yyyy-mm-dd')='" + p_visit_dt + "'  ";
            SQL = SQL + Environment.NewLine + "     AND a.poli_cd  in ('POL0007')   ";
            SQL = SQL + Environment.NewLine + "     and visit_remark <> 'I' ";
            SQL = SQL + Environment.NewLine + "     AND a.status IN ('PRE', 'RSV', 'NUR', 'INS', 'OBS', 'HOL')   "; ;



            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl2.DataSource = null;
                gridView2.Columns.Clear();
                gridControl2.DataSource = dt;

                gridView2.OptionsView.ColumnAutoWidth = true;
                gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView2.IndicatorWidth = 40;
                //gridView2.OptionsBehavior.Editable = false;
                gridView2.BestFitColumns();

                gridView2.Columns[0].Caption = "Action";
                gridView2.Columns[1].Caption = "Action ID";
                gridView2.Columns[2].Caption = "RM No";
                gridView2.Columns[3].Caption = "Nama";
                gridView2.Columns[4].Caption = "Tgl Regis";
                gridView2.Columns[5].Caption = "Tgl Ins";
                gridView2.Columns[6].Caption = "Tgl Tindakan";
                gridView2.Columns[7].Caption = "Tipe";
                gridView2.Columns[8].Caption = "Hasil";
                gridView2.Columns[9].Caption = "Rekomendasi";
                gridView2.Columns[10].Caption = "Layanan";
                gridView2.Columns[11].Caption = "Harga";
                gridView2.Columns[12].Caption = "Tipe Adj";
                gridView2.Columns[13].Caption = "Detail ID";
                gridView2.Columns[14].Caption = "Detail Adj ID";

                RepositoryItemLookUpEdit kLookup = new RepositoryItemLookUpEdit();
                kLookup.DataSource = listLayanan;
                kLookup.ValueMember = "layananCode";
                kLookup.DisplayMember = "layananName";

                kLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                kLookup.DropDownRows = listLayanan.Count;
                kLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                kLookup.AutoSearchColumnIndex = 1;
                kLookup.NullText = "";
                gridView2.Columns[10].ColumnEdit = kLookup;

                gridView2.Columns[3].OptionsColumn.ReadOnly = true;
                gridView2.Columns[4].OptionsColumn.ReadOnly = true;
                gridView2.Columns[6].OptionsColumn.ReadOnly = true;
                gridView2.Columns[7].OptionsColumn.ReadOnly = true;
                gridView2.Columns[10].OptionsColumn.ReadOnly = false;
                gridView2.Columns[11].OptionsColumn.ReadOnly = true;

                gridView2.Columns[0].Visible = false;
                gridView2.Columns[1].Visible = false;
                gridView2.Columns[2].Visible = false;
                //gridView2.Columns[3].Visible = false;
                gridView2.Columns[5].Visible = false;
                gridView2.Columns[8].Visible = false;
                gridView2.Columns[9].Visible = false;
                gridView2.Columns[11].Visible = false;
                gridView2.Columns[12].Visible = false;
                gridView2.Columns[13].Visible = false;
                gridView2.Columns[14].Visible = false;

                if (v_status == "SYS" || v_status == "DOH" || v_status == "DOC")
                {
                    gridView2.Columns[9].Visible = true;
                }
                else
                {
                    gridView2.Columns[9].Visible = false;
                }

                if (gridView2.RowCount > 0)
                {
                    btnSaveAct.Enabled = true;
                }
                else
                {
                    btnSaveAct.Enabled = false;
                }

                gridView2.BestFitColumns();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            
            if (e.Column.Caption == "Hasil" || e.Column.Caption == "Rekomendasi")
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

        private void btnCall_Click(object sender, EventArgs e)
        {
            string sql_check5 = "", rm_number = "", p_que = "", id_visit = "", sql1 = "", p_que2 = "";

            p_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
            id_visit = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[12]).ToString();
             
            sql_check5 = "";
            sql_check5 = sql_check5 + @" select TYPE_INS, a.que
                                from KLINIK.CS_CALL_LOG a, KLINIK.cs_visit b
                                where a.que = b.que01
                                AND a.que = '" + p_que + @"'    
                                AND b.id_visit = '" + id_visit + @"'    
                                AND TRUNC(a.INS_DATE) = TRUNC(SYSDATE)
                                AND TRUNC(A.INS_DATE) = TRUNC(VISIT_DATE(+))  ";

            OleDbConnection oraConnect5 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra5 = new OleDbDataAdapter(sql_check5, oraConnect5);
            DataTable dt5 = new DataTable();
            adOra5.Fill(dt5);
            if (dt5.Rows.Count > 0)
            {
                rm_number = dt5.Rows[0]["TYPE_INS"].ToString();
            }

            if (rm_number.ToString().Equals("PAY"))
            {
                sql1 = " ";
                sql1 = @"UPDATE KLINIK.CS_CALL_LOG SET FLAG = 'N' WHERE QUE = '" + p_que + "' and TYPE_INS ='PAY' AND TRUNC(INS_DATE) = TRUNC(SYSDATE)";

                ORADB.Execute(ORADB.XE, sql1);
            }
            else
            {
                MessageBox.Show("Maaf Pasien sudah di Proses, Tidak Dapat Dipanggil Di Bagian Laboratorium.");
                return;
            }
             
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Hasil" || e.Column.Caption == "Rekomendasi")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnSaveAct_Click(object sender, EventArgs e)
        {
            string act_id = "", action = "", hasil = "", rekomendasi = "";
            string sql_update = "";

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                action = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();
                act_id = gridView2.GetRowCellValue(i, gridView2.Columns[1]).ToString();
                hasil = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                rekomendasi = gridView2.GetRowCellValue(i, gridView2.Columns[9]).ToString();

                if (action == "U")
                {
                    sql_update = "";

                    sql_update = sql_update + " update cs_action set act_name = '" + hasil + "', act_remark = '" + rekomendasi + "', ";
                    sql_update = sql_update + " upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                    sql_update = sql_update + " where act_id = '" + act_id + "' ";

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
    }
}