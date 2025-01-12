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
    public partial class FirstInspectionPreg : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Kehamilan> listKehamilan = new List<Kehamilan>();
        List<Stat> listStat = new List<Stat>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string upd_col = "", s_policd = "";

        public FirstInspectionPreg()
        {
            InitializeComponent();
        }

        private void FirstInspection_Load(object sender, EventArgs e)
        {
            btnCreate.Enabled = false;
            btnSaveAnam.Enabled = false;
            btnAddAnam.Enabled = false;
            btnSaveAdd.Enabled = false;

            initData();
            LoadData();
        }
        private void initData()
        {
            listKehamilan.Clear();
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K1", kehamilanName = "K1" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K2", kehamilanName = "K2" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K3", kehamilanName = "K3" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K4", kehamilanName = "K4" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K5", kehamilanName = "K5" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K6", kehamilanName = "K6" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K7", kehamilanName = "K7" });

            listStat.Clear();
            listStat.Add(new Stat() { statCode = "A", statName = "Active" });
            listStat.Add(new Stat() { statCode = "I", statName = "Inactive" });
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = " ";
            sql_search = sql_search + " select que01, empid, name, gender, age, blood_type, poli_name, purpose,   " +
                                      " purpose_nm, status,  " +
                                      " (select max(rm_no) from KLINIK.cs_patient where status='A' and group_patient=aa.type_mr and empid=aa.empid) as rm_no,  " +
                                      " decode(type_mr,'PREG','Ibu Hamil','FAMP','KB','Umum') as type_mr, poli_cd from (  " +
                                      " select que01, a.empid, b.name, gender, age, blood_type, poli_name, purpose,   " +
                                      " decode(purpose,'DOC','Dokter','Bidan') purpose_nm,   " +
                                      " case when a.status='RSV' then 'Reservation' " +
                                      " when a.status='NUR' then 'First Inspection' end status, " +
                                      //" case when a.poli_cd='POL0002' then 'PREG'  " +
                                      //" when a.poli_cd='POL0003' then 'FAMP' else 'COMM' end as type_mr, a.poli_cd " +
                                      " poli_group as type_mr, a.poli_cd " +
                                      " from KLINIK.cs_visit a join KLINIK.cs_employees  b on a.empid=b.empid  " +
                                      " join KLINIK.cs_policlinic c on (a.poli_cd=c.poli_cd and c.status='A') " +
                                      " where 1 = 1  " +
                                      " and to_char(visit_date,'yyyy-mm-dd')= '" + today + "'  " +
                                      " and a.status in ('RSV','NUR') " +
                                      " and purpose = 'MID' " +
                                      " order by purpose, que01 ) aa ";
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 30;
                gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();

                gridView1.Columns[0].Caption = "Antrian";
                gridView1.Columns[1].Caption = "NIK";
                gridView1.Columns[2].Caption = "Nama";
                gridView1.Columns[3].Caption = "Jenis Kelamin";
                gridView1.Columns[4].Caption = "Umur";
                gridView1.Columns[5].Caption = "Gol Darah";
                gridView1.Columns[6].Caption = "Poli";
                gridView1.Columns[7].Caption = "purpose";
                gridView1.Columns[8].Caption = "Berobat";
                gridView1.Columns[9].Caption = "Status";
                gridView1.Columns[10].Caption = "Medical Record";
                gridView1.Columns[11].Caption = "Type Record";
                gridView1.Columns[12].Caption = "Poli Cd";

                gridView1.Columns[7].Visible = false;
                gridView1.Columns[12].Visible = false;
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoadData();
            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = false;
            gridControl2.DataSource = null;

            btnSaveAdd.Enabled = false;
            gridControl3.DataSource = null;
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridView View = sender as GridView;
            string s_rm = "", s_que = "", s_poli = "", s_group = "", s_rmno = "", group="", s_nik="", s_nama="";
            s_rm = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            s_que = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_nik = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            s_poli = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
            s_rmno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            s_group = View.GetRowCellDisplayText(e.RowHandle, View.Columns[11]);
            s_policd = View.GetRowCellDisplayText(e.RowHandle, View.Columns[12]);

            if (s_rm == "")
            {
                btnCreate.Enabled = true;
                btnSaveAnam.Enabled = false;
                btnSaveAdd.Enabled = false;
            }
            else
            {
                btnCreate.Enabled = false;
            }

            //string sql_anam = " select to_char(insp_date,'yyyy-mm-dd') as insp_date, rm_no, visit_no, blood_press, pulse, temperature, allergy, anamnesa, info_k, 'S' action from cs_anamnesa where rm_no = '" + s_rm + "' and to_char(insp_date,'yyyy-mm-dd') = '" + today + "' ";
            string sql_anam = "";
            sql_anam = " select to_char(insp_date,'yyyy-mm-dd') as insp_date, '" + s_nama + "' as nama, visit_no, " +
                       " blood_press, pulse, temperature, allergy, anamnesa, info_k, 'S' action, rm_no " +
                       " from KLINIK.cs_anamnesa where rm_no = '" + s_rm + "' and to_char(insp_date,'yyyy-mm-dd') = '" + today + "' ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_anam, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl2.DataSource = null;
            gridView2.Columns.Clear();
            gridControl2.DataSource = dt;

            gridView2.OptionsView.ColumnAutoWidth = true;
            gridView2.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView2.Appearance.HeaderPanel.FontSizeDelta = 0;
            //gridView2.BestFitColumns();

            gridView2.Columns[0].Caption = "Tanggal";
            gridView2.Columns[1].Caption = "Nama";
            gridView2.Columns[2].Caption = "Antrian";
            gridView2.Columns[3].Caption = "Tensi";
            gridView2.Columns[4].Caption = "Nadi";
            gridView2.Columns[5].Caption = "Suhu";
            gridView2.Columns[6].Caption = "Alergi";
            gridView2.Columns[7].Caption = "Keluhan Utama";
            gridView2.Columns[8].Caption = "Kehamilan";
            gridView2.Columns[9].Caption = "Action";
            gridView2.Columns[10].Caption = "Medical Record";

            RepositoryItemLookUpEdit kLookup = new RepositoryItemLookUpEdit();
            kLookup.DataSource = listKehamilan;
            kLookup.ValueMember = "kehamilanCode";
            kLookup.DisplayMember = "kehamilanName";

            kLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            kLookup.DropDownRows = listKehamilan.Count;
            kLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            kLookup.AutoSearchColumnIndex = 1;
            kLookup.NullText = "";
            gridView2.Columns[8].ColumnEdit = kLookup;
            gridView2.BestFitColumns();

            if (s_poli == "Poli Ibu Hamil")
            {
                gridView2.Columns[8].Visible = true;
            }
            else
            {
                gridView2.Columns[8].Visible = false;
            }

            //gridView2.Columns[8].Visible = false;
            gridView2.Columns[9].Visible = false;

            if (gridView2.RowCount > 0)
            {
                btnSaveAnam.Enabled = true;
                //btnAddAnam.Enabled = false;
            }
            else
            {
                btnSaveAnam.Enabled = false;
                //btnAddAnam.Enabled = true;
            }

            if (s_rm != "")
            {
                btnAddAnam.Enabled = true;
            }
            else
            {
                btnAddAnam.Enabled = false;
            }


            string sql_addinfo = "", sql_info = "", p_col = "";

            sql_addinfo = " select info_cd, description from KLINIK.cs_add_info where status = 'A' and poli_cd = '" + s_policd + "' ";

            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_addinfo, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                p_col = p_col + ", " + dt2.Rows[i]["info_cd"].ToString();
            }

            if (s_group == "Umum")
            {
                group = "COMM";
            }
            else if (s_group == "KB")
            {
                group = "FAMP";
            }
            else
            {
                group = "PREG";
            }

            sql_info = " ";
            sql_info = sql_info + " select  empid, group_patient, decode(group_patient,'PREG','Ibu Hamil','FAMP','KB','Umum') group_patient_nm, '" + s_nama + "' as nama, 'U' as a, status, rm_no ";
            sql_info = sql_info + p_col;
            sql_info = sql_info + " from KLINIK.cs_patient where status='A' and group_patient='" + group + "' and empid='" + s_nik + "' ";

            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_info, sqlConnect3);
            DataTable dt3 = new DataTable();
            adSql3.Fill(dt3);

            gridControl3.DataSource = null;
            gridView3.Columns.Clear();
            gridControl3.DataSource = dt3;

            gridView3.OptionsView.ColumnAutoWidth = true;
            gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
            //gridView3.BestFitColumns();
            int ii = 0;

            gridView3.Columns[0].Caption = "NIK";
            gridView3.Columns[1].Caption = "Type Record";
            gridView3.Columns[2].Caption = "Type Record";
            gridView3.Columns[3].Caption = "Nama";
            gridView3.Columns[4].Caption = "Action";
            gridView3.Columns[5].Caption = "Status";
            gridView3.Columns[6].Caption = "Medical Record";

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                ii = i + 7;
                gridView3.Columns[ii].Caption = dt2.Rows[i]["description"].ToString();
            }
            RepositoryItemLookUpEdit statLookup = new RepositoryItemLookUpEdit();
            statLookup.DataSource = listStat;
            statLookup.ValueMember = "statCode";
            statLookup.DisplayMember = "statName";

            statLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            statLookup.DropDownRows = listStat.Count;
            statLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            statLookup.AutoSearchColumnIndex = 1;
            statLookup.NullText = "";
            gridView3.Columns[5].ColumnEdit = statLookup;

            gridView3.Columns[0].OptionsColumn.ReadOnly = true;
            gridView3.Columns[1].OptionsColumn.ReadOnly = true;
            gridView3.Columns[2].OptionsColumn.ReadOnly = true;
            gridView3.Columns[3].OptionsColumn.ReadOnly = true;
            gridView3.Columns[4].OptionsColumn.ReadOnly = true;
            gridView3.BestFitColumns();
            gridView3.Columns[1].Visible = false;
            gridView3.Columns[4].Visible = false;
            gridView3.Columns[6].Visible = false;

            if (gridView3.RowCount > 0)
            {
                btnSaveAdd.Enabled = true;
            }
            else
            {
                btnSaveAdd.Enabled = false;
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Status")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
                string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);

                if (kk == "First Inspection" && pur == "Dokter")
                {
                    e.Appearance.BackColor = Color.FromArgb(50, Color.DodgerBlue);
                    e.Appearance.BackColor2 = Color.FromArgb(50, Color.DodgerBlue);
                }
                else if (kk == "First Inspection" && pur == "Bidan")
                {
                    e.Appearance.BackColor = Color.FromArgb(50, Color.LightCoral);
                    e.Appearance.BackColor2 = Color.FromArgb(50, Color.LightCoral);
                }
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string sql_insert="";
            string rm_no = "", nik = "", grp = "", poli = "", cd1 = "", cd2 = "", cd3 = "";
            
            nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
            poli = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();

            if (poli == "Poli Ibu Hamil")
            {
                grp = "PREG";
            }
            else if (poli == "Poli KB")
            {
                grp = "FAMP";
            }
            else
            {
                grp = "COMM";
            }

            cd1 = grp.Substring(0, 1);
            cd2 = nik.Substring(2);
            cd3 = DateTime.Now.ToString("yyMMdd");

            rm_no = cd1 + cd2 + cd3;

            sql_insert = " insert into KLINIK.cs_patient (rm_no, empid, group_patient, status, ins_date, ins_emp) values ('" + rm_no + "', '" + nik + "', '" + grp + "', 'A', sysdate, '" + v_empid + "') ";
            try
            {
                OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect3);
                oraConnect3.Open();
                cm.ExecuteNonQuery();
                oraConnect3.Close();
                cm.Dispose();

                //MessageBox.Show(sql_insert);
                //MessageBox.Show("Query Exec : " + sql);
                
                LoadData();
                btnCreate.Enabled = false;
                MessageBox.Show("Data Berhasil disimpan.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            gridView2.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView2.AddNewRow();
            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = true;
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "Alergi" || e.Column.Caption == "Keluhan Utama")
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

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            string tmp_que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
            string tmp_rm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
            string tmp_nm = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
            view.SetRowCellValue(e.RowHandle, view.Columns[0], today);
            view.SetRowCellValue(e.RowHandle, view.Columns[1], tmp_nm);
            view.SetRowCellValue(e.RowHandle, view.Columns[10], tmp_rm);
            view.SetRowCellValue(e.RowHandle, view.Columns[2], tmp_que);
            view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
            gridView2.Columns[0].OptionsColumn.ReadOnly = true;
            gridView2.Columns[1].OptionsColumn.ReadOnly = true;
            gridView2.Columns[10].OptionsColumn.ReadOnly = true;
            gridView2.Columns[2].OptionsColumn.ReadOnly = true;
            gridView2.Columns[9].OptionsColumn.ReadOnly = true;
        }

        private void btnSaveAnam_Click(object sender, EventArgs e)
        {
            string date = "", que = "", tensi = "", nadi = "", suhu = "", alergi = "", keluhan = "", action = "", rm_no = "", nik="", infok = "";
            string sql_update2 = "", sql_cnt = "", sql_insert = "", sql_update = "", anam_cnt="";

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                date = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();
                rm_no = gridView2.GetRowCellValue(i, gridView2.Columns[10]).ToString();
                que = gridView2.GetRowCellValue(i, gridView2.Columns[2]).ToString();
                tensi = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                nadi = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                suhu = gridView2.GetRowCellValue(i, gridView2.Columns[5]).ToString();
                alergi = gridView2.GetRowCellValue(i, gridView2.Columns[6]).ToString();
                keluhan = gridView2.GetRowCellValue(i, gridView2.Columns[7]).ToString();
                infok = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                action = gridView2.GetRowCellValue(i, gridView2.Columns[9]).ToString();
                nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();

                if (tensi == "")
                {
                    MessageBox.Show("Tensi harus diisi");
                }
                else if (nadi == "")
                {
                    MessageBox.Show("Nadi harus diisi");
                }
                else if (keluhan == "")
                {
                    MessageBox.Show("Keluhan harus diisi");
                }
                else
                {
                    if (action == "I")
                    {
                        sql_cnt = " select count(0) cnt from KLINIK.cs_anamnesa where to_char(insp_date,'yyyy-mm-dd') = '" + today + "' and visit_no = '" + que + "' and rm_no = '" + rm_no + "' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        anam_cnt = dt.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(anam_cnt) > 0)
                        {
                            //MessageBox.Show("Employee ID " + nik + " sudah terdaftar.");
                        }
                        else
                        {
                            OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                            OleDbCommand command = new OleDbCommand();
                            OleDbTransaction trans = null;

                            command.Connection = oraConnectTrans;
                            oraConnectTrans.Open();


                            //sql_insert = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, ins_date, ins_emp) values (cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "','yyyy-mm-dd'), '" + tensi + "', '" + nadi + "','" + suhu + "', '" + alergi + "', '" + keluhan + "', '" + que + "', sysdate, '" + v_empid + "') ";

                            try
                            {
                                //OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                                //OleDbCommand cm = new OleDbCommand(sql_insert, oraConnect3);
                                //oraConnect3.Open();
                                //cm.ExecuteNonQuery();
                                //oraConnect3.Close();
                                //cm.Dispose();

                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                command.CommandText = " insert into KLINIK.cs_anamnesa (anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, info_k, ins_date, ins_emp) values(cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "', 'yyyy-mm-dd'), '" + tensi + "', '" + nadi + "', '" + suhu + "', '" + alergi + "', '" + keluhan + "', '" + que + "', '" + infok + "', sysdate, '" + v_empid + "') ";
                                command.ExecuteNonQuery();

                                command.CommandText = " update KLINIK.cs_visit set status = 'NUR', time_reservation=sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'yyyy-mm-dd') = '" + date + "' and que01 = '" + que + "' ";
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
                    else if (action == "U")
                    {
                        sql_update = "";

                        sql_update = sql_update + " update KLINIK.cs_anamnesa" +
                                     " set blood_press = '" + tensi + "', pulse = '" + nadi + "', " +
                                     " temperature = '" + suhu + "', allergy = '" + alergi + "', anamnesa = '" + keluhan + "', info_k = '" + infok + "',  ";
                        sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                        sql_update = sql_update + " where rm_no = '" + rm_no + "' and to_char(insp_date,'yyyy-mm-dd') = '" + date + "' and visit_no = '" + que + "' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
                
            }
        }

        private void btnSaveAdd_Click(object sender, EventArgs e)
        {
            string val = "", stat="", nik="", rm_no="";
            string sql_addinfo = "";

            sql_addinfo = " select info_cd, description from KLINIK.cs_add_info where status = 'A' and poli_cd = '" + s_policd + "' ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_addinfo, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            int iii = 0;
            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                nik = gridView3.GetRowCellValue(i, gridView3.Columns[0]).ToString();
                rm_no = gridView3.GetRowCellValue(i, gridView3.Columns[6]).ToString();
                stat = gridView3.GetRowCellValue(i, gridView3.Columns[5]).ToString();

                upd_col = upd_col + " update KLINIK.cs_patient set status = '" + stat + "' ";
                for (int ii = 0; ii < dt.Rows.Count; ii++)
                {
                    iii = ii + 7;
                    val = gridView3.GetRowCellValue(i, gridView3.Columns[iii]).ToString();

                    upd_col = upd_col + ", " + dt.Rows[ii]["info_cd"].ToString() + " = '" + val + "' ";
                }
                upd_col = upd_col + " , upd_date=sysdate, upd_emp='" + v_empid + "' ";
                upd_col = upd_col + " where empid='" + nik + "' and rm_no='" + rm_no + "' ";


                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(upd_col, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_update);

                    //MessageBox.Show("Update Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }

                upd_col = "";
            }
            
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "Alergi" || e.Column.Caption == "Keluhan Utama" || e.Column.Caption == "Kehamilan")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.Caption == "Hamil Ke" || e.Column.Caption == "Minggu" || e.Column.Caption == "Anak Ke" || e.Column.Caption == "GPA" || e.Column.Caption == "HPHT" || e.Column.Caption == "Tgl Cuti" || e.Column.Caption == "Taksiran" || e.Column.Caption == "Tgl Ambil Surat Cuti" || e.Column.Caption == "Tgl Cuti" || e.Column.Caption == "Mulai Cuti" || e.Column.Caption == "Selesai Cuti")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //GridView view = sender as GridView;

            //if (e.Column.Caption == "Status")
            //{
            //    string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
            //    if (tmp_stat == "I")
            //    {
            //        view.SetRowCellValue(e.RowHandle, view.Columns[4], "I");
            //    }
            //    else
            //    {
            //        view.SetRowCellValue(e.RowHandle, view.Columns[4], "U");
            //    }
            //}
        }
    }
}