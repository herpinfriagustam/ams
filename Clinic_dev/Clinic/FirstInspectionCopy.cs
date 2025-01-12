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
    public partial class FirstInspectionCopy : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Poli> listPoli = new List<Poli>();
        List<Stat> listStat = new List<Stat>();
        List<Kehamilan> listKehamilan = new List<Kehamilan>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string upd_col = "", s_policd = "";

        public FirstInspectionCopy()
        {
            InitializeComponent();
        }

        private void FirstInspection_Load(object sender, EventArgs e)
        {
            initData();
            LoadData();
        }
        private void initData()
        {

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

            listKehamilan.Clear();
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K1", kehamilanName = "K1" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K2", kehamilanName = "K2" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K3", kehamilanName = "K3" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K4", kehamilanName = "K4" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K5", kehamilanName = "K5" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K6", kehamilanName = "K6" });
            listKehamilan.Add(new Kehamilan() { kehamilanCode = "K7", kehamilanName = "K7" });
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string SQL;

            SQL = " ";
            SQL = SQL + Environment.NewLine + "select c.anamnesa_id, b.rm_no, c.insp_date, c.visit_no, ";
            SQL = SQL + Environment.NewLine + "to_char(insp_date,'yyyy-mm-dd') tgl, d.name, a.poli_cd, a.status, ";
            SQL = SQL + Environment.NewLine + "c.blood_press, c.pulse, c.temperature, c.bb, c.tb, c.allergy, null dd, c.anamnesa, 'S' action, ";
            SQL = SQL + Environment.NewLine + "info_k, a.patient_no, ";
            SQL = SQL + Environment.NewLine + "cholesterol, blood_sugar, uric_acid, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_visit a ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_patient b on (a.patient_no=b.patient_no) ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_anamnesa c on (b.rm_no=c.rm_no and trunc(a.visit_date)=c.insp_date and a.que01=c.visit_no) ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_patient_info d on (a.patient_no=d.patient_no) ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and to_char(insp_date,'yyyy-mm-dd')=to_char(sysdate,'yyyy-mm-dd') ";
            SQL = SQL + Environment.NewLine + "and a.status in ('RSV','NUR') ";
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

                //gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 40;
                //gridView1.OptionsBehavior.Editable = false;
                //gridView1.BestFitColumns();

                gridView1.FixedLineWidth = 4;
                gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[5].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[6].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView1.Columns[7].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].Caption = "ID";
                gridView1.Columns[1].Caption = "RM No";
                gridView1.Columns[2].Caption = "Visit Date";
                gridView1.Columns[3].Caption = "Visit No";
                gridView1.Columns[4].Caption = "Tgl";
                gridView1.Columns[5].Caption = "Nama";
                gridView1.Columns[6].Caption = "Poli";
                gridView1.Columns[7].Caption = "Status";
                gridView1.Columns[8].Caption = "Tensi";
                gridView1.Columns[9].Caption = "Nadi";
                gridView1.Columns[10].Caption = "Suhu";
                gridView1.Columns[11].Caption = "BB";
                gridView1.Columns[12].Caption = "TB";
                gridView1.Columns[13].Caption = "Alergi";
                gridView1.Columns[14].Caption = "Riwayat";
                gridView1.Columns[15].Caption = "Keluhan";
                gridView1.Columns[16].Caption = "Action";
                gridView1.Columns[17].Caption = "Kehamilan";
                gridView1.Columns[18].Caption = "Pasien No";
                gridView1.Columns[19].Caption = "Kolesterol (Mg)";
                gridView1.Columns[20].Caption = "Gula Darah (Mg)";
                gridView1.Columns[21].Caption = "Asam Urat (Mg)";
                gridView1.Columns[22].Caption = "R.Sekarang";
                gridView1.Columns[23].Caption = "R.Dulu";
                gridView1.Columns[24].Caption = "R.Keluarga";
                gridView1.Columns[25].Caption = "Pem.Fisik";
                gridView1.Columns[26].Caption = "Pem.Lain";

                RepositoryItemLookUpEdit pLookup = new RepositoryItemLookUpEdit();
                pLookup.DataSource = listPoli;
                pLookup.ValueMember = "poliCode";
                pLookup.DisplayMember = "poliName";

                pLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                pLookup.DropDownRows = listPoli.Count;
                pLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                pLookup.AutoSearchColumnIndex = 1;
                pLookup.NullText = "";
                gridView1.Columns[6].ColumnEdit = pLookup;

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

                RepositoryItemLookUpEdit kLookup = new RepositoryItemLookUpEdit();
                kLookup.DataSource = listKehamilan;
                kLookup.ValueMember = "kehamilanCode";
                kLookup.DisplayMember = "kehamilanName";

                kLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                kLookup.DropDownRows = listKehamilan.Count;
                kLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                kLookup.AutoSearchColumnIndex = 1;
                kLookup.NullText = "";
                gridView1.Columns[17].ColumnEdit = kLookup;

                gridView1.Columns[4].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.ReadOnly = true;
                gridView1.Columns[6].OptionsColumn.ReadOnly = true;
                gridView1.Columns[7].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;
                gridView1.Columns[2].Visible = false;
                gridView1.Columns[3].Visible = false;
                gridView1.Columns[14].Visible = false;
                gridView1.Columns[16].Visible = false;
                gridView1.Columns[18].Visible = false;

                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
            
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Status")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[7]);

                if (kk == "Reservasi")
                {
                    e.Appearance.BackColor = Color.FromArgb(50, Color.DodgerBlue);
                    e.Appearance.BackColor2 = Color.FromArgb(50, Color.DodgerBlue);
                }
                else
                {
                    e.Appearance.BackColor = Color.FromArgb(70, Color.DodgerBlue);
                    e.Appearance.BackColor2 = Color.FromArgb(70, Color.DodgerBlue);
                }
                
            }

            if (e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "BB" || e.Column.Caption == "TB" || 
                e.Column.Caption == "Alergi" || e.Column.Caption == "Riwayat" || e.Column.Caption == "Keluhan" || e.Column.Caption == "Kehamilan" || e.Column.Caption == "Kolesterol (Mg)" ||
                e.Column.Caption == "Gula Darah (Mg)" || e.Column.Caption == "Asam Urat (Mg)" || e.Column.Caption == "R.Sekarang" || e.Column.Caption == "R.Dulu" || e.Column.Caption == "R.Keluarga" ||
                e.Column.Caption == "Pem.Fisik" || e.Column.Caption == "Pem.Lain")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }
        
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tensi" || e.Column.Caption == "Nadi" || e.Column.Caption == "Suhu" || e.Column.Caption == "BB" || e.Column.Caption == "TB" || 
                e.Column.Caption == "Alergi" || e.Column.Caption == "Riwayat" || e.Column.Caption == "Keluhan" || e.Column.Caption == "Kehamilan" || e.Column.Caption == "Kolesterol (Mg)" ||
                e.Column.Caption == "Gula Darah (Mg)" || e.Column.Caption == "Asam Urat (Mg)" || e.Column.Caption == "R.Sekarang" || e.Column.Caption == "R.Dulu" || e.Column.Caption == "R.Keluarga" ||
                e.Column.Caption == "Pem.Fisik" || e.Column.Caption == "Pem.Lain")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[16]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[16], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[16], "U");
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

        private void btnSaveAnam_Click(object sender, EventArgs e)
        {
            string id = "", tensi = "", nadi = "", suhu = "", alergi = "", keluhan = "", action = "", bb = "", tb="", infok = "", riwayat="",poli="";
            string chol = "", bsugar = "", uacid = "", r_now = "", r_then = "", r_fam = "", anam_physical = "", anam_other = "";
            string sql_update2 = "", sql_update = "", pasno="", que="", vdate="";

            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                id = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                vdate = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                que = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                poli = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                tensi = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                nadi = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                suhu = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                bb = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                tb = gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
                alergi = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
                //riwayat = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();
                keluhan = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString();
                infok = gridView1.GetRowCellValue(i, gridView1.Columns[17]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString();
                pasno = gridView1.GetRowCellValue(i, gridView1.Columns[18]).ToString();
                chol = gridView1.GetRowCellValue(i, gridView1.Columns[19]).ToString();
                bsugar = gridView1.GetRowCellValue(i, gridView1.Columns[20]).ToString();
                uacid = gridView1.GetRowCellValue(i, gridView1.Columns[21]).ToString();
                r_now = gridView1.GetRowCellValue(i, gridView1.Columns[22]).ToString();
                r_then = gridView1.GetRowCellValue(i, gridView1.Columns[23]).ToString();
                r_fam = gridView1.GetRowCellValue(i, gridView1.Columns[24]).ToString();
                anam_physical = gridView1.GetRowCellValue(i, gridView1.Columns[25]).ToString();
                anam_other = gridView1.GetRowCellValue(i, gridView1.Columns[26]).ToString();


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
                else if (poli == "POL0002" && infok=="")
                {
                    MessageBox.Show("Kehamilan harus disi");
                }
                else
                {
                    if (action == "U")
                    {

                        OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                        OleDbCommand command = new OleDbCommand();
                        OleDbTransaction trans = null;

                        command.Connection = oraConnectTrans;
                        oraConnectTrans.Open();

                        sql_update = "";

                        sql_update = sql_update + " update KLINIK.cs_anamnesa" +
                                     " set blood_press = '" + tensi + "', pulse = '" + nadi + "', bb = '" + bb + "', tb = '" + tb + "', " +
                                     " temperature = '" + suhu + "', allergy = '" + alergi + "', anamnesa = '" + keluhan + "', info_k = '" + infok + "',  " +
                                     " cholesterol = '" + chol + "', blood_sugar = '" + bsugar + "', uric_acid = '" + uacid + "', disease_now = '" + r_now + "',  " +
                                     " disease_then = '" + r_then + "', disease_family = '" + r_fam + "', anamnesa_physical = '" + anam_physical + "', anamnesa_other = '" + anam_other + "',  ";
                        sql_update = sql_update + " upd_emp = '" + v_empid + "', upd_date = sysdate ";
                        sql_update = sql_update + " where anamnesa_id = '" + id + "'  ";

                        sql_update2 = sql_update;

                        try
                        {
                            trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                            command.Connection = oraConnectTrans;
                            command.Transaction = trans;

                            command.CommandText = sql_update2;
                            command.ExecuteNonQuery();

                            command.CommandText = " update KLINIK.cs_visit set status = 'NUR', time_reservation=sysdate, upd_emp = '" + v_empid + "', upd_date = sysdate where patient_no = '" + pasno + "' and to_char(visit_date,'yyyy-mm-dd') = '" + vdate + "' and que01 = '" + que + "' ";
                            command.ExecuteNonQuery();

                            trans.Commit();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                        oraConnectTrans.Close();
                    }
                }
                
            }
            LoadData();
        }

        
    }
}