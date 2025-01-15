using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using Clinic.Properties;
using System.IO;
using System.Media;
using System.Threading;
using System.Globalization;

namespace Clinic
{
    public partial class DiagnosaMngt : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Diagnosa> listDiagnosa = new List<Diagnosa>();
        List<DiagnosaType> listDiagnosaType = new List<DiagnosaType>();

        //public string DB.vUserId = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";

        public DiagnosaMngt()
        {
            InitializeComponent();
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            workingDirectory = Environment.CurrentDirectory;
            resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            dDateBgn.Text = today;
            dDateEnd.Text = today;
            initData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "DiagnosaMngt");
            //LoadData();
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = "";
            sql_search = sql_search + Environment.NewLine + "SELECT   TO_CHAR (insp_date, 'yyyy-mm-dd') insp_date, visit_no, c.patient_no, ";
            sql_search = sql_search + Environment.NewLine + "         c.NAME, null dept, b.rm_no, initcap(e.cat_name) cat_name, a.item_cd, type_diagnosa, ";
            sql_search = sql_search + Environment.NewLine + "         'S' action, diagnosa_id, a.remark ";
            sql_search = sql_search + Environment.NewLine + "    FROM KLINIK.cs_diagnosa a JOIN KLINIK.cs_patient b ON a.rm_no = b.rm_no ";
            sql_search = sql_search + Environment.NewLine + "         JOIN KLINIK.cs_patient_info c ON b.patient_no = c.patient_no ";
            sql_search = sql_search + Environment.NewLine + "         JOIN KLINIK.cs_diagnosa_item d ON a.item_cd = d.item_cd ";
            sql_search = sql_search + Environment.NewLine + "         JOIN KLINIK.cs_diagnosa_category e ON d.cat_id = e.cat_id ";
            sql_search = sql_search + Environment.NewLine + "   WHERE 1 = 1 ";
            sql_search = sql_search + Environment.NewLine + "     AND b.status = 'A' ";
            sql_search = sql_search + Environment.NewLine + "     AND TRUNC (insp_date) BETWEEN TO_DATE ('" + dDateBgn.Text + "', 'yyyy-mm-dd') ";
            sql_search = sql_search + Environment.NewLine + "                               AND TO_DATE ('" + dDateEnd.Text + "', 'yyyy-mm-dd') ";
            sql_search = sql_search + Environment.NewLine + "     AND c.patient_no like '" + tNik.Text + "%' ";
            sql_search = sql_search + Environment.NewLine + "ORDER BY insp_date, visit_no,  type_diagnosa ";


            loading.ShowWaitForm();
            try
            {
                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_search, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);

                gridControl1.DataSource = null;
                gridView1.Columns.Clear();
                gridControl1.DataSource = dt;

                simpleButton2.Enabled = false;

                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                gridView1.OptionsView.ColumnAutoWidth = true;
                gridView1.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView1.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView1.IndicatorWidth = 50;
                //gridView1.OptionsBehavior.Editable = false;
                gridView1.BestFitColumns();

                gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;
                gridView1.Columns[4].OptionsColumn.ReadOnly = true;
                gridView1.Columns[5].OptionsColumn.ReadOnly = true;
                gridView1.Columns[6].OptionsColumn.ReadOnly = true;
                gridView1.Columns[9].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Caption = "Tanggal";
                gridView1.Columns[1].Caption = "Antrian";
                gridView1.Columns[2].Caption = "Pasien No";
                gridView1.Columns[3].Caption = "Nama";
                gridView1.Columns[4].Caption = "Dept";
                gridView1.Columns[5].Caption = "Med. Record No";
                gridView1.Columns[6].Caption = "Kategori ICD";
                gridView1.Columns[7].Caption = "Nama ICD";
                gridView1.Columns[8].Caption = "Tipe Diagnosa";
                gridView1.Columns[9].Caption = "Action";
                gridView1.Columns[10].Caption = "ID";
                gridView1.Columns[11].Caption = "Remark";

                gridView1.Columns[0].MinWidth = 70;
                gridView1.Columns[0].MaxWidth = 70;
                gridView1.Columns[1].MinWidth = 60;
                gridView1.Columns[1].MaxWidth = 60;
                gridView1.Columns[2].MinWidth = 80;
                gridView1.Columns[2].MaxWidth = 80;
                gridView1.Columns[5].Width = 100;
                gridView1.Columns[6].Width = 200;
                gridView1.Columns[7].Width = 200;
                gridView1.Columns[8].MinWidth = 90;
                gridView1.Columns[8].MaxWidth = 90;

                RepositoryItemLookUpEdit diagnosaLookup = new RepositoryItemLookUpEdit();
                diagnosaLookup.DataSource = listDiagnosa;
                diagnosaLookup.ValueMember = "diagnosaCode";
                diagnosaLookup.DisplayMember = "diagnosaName";

                diagnosaLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                diagnosaLookup.DropDownRows = listDiagnosa.Count;
                diagnosaLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                diagnosaLookup.AutoSearchColumnIndex = 1;
                diagnosaLookup.NullText = "";
                gridView1.Columns[7].ColumnEdit = diagnosaLookup;

                RepositoryItemLookUpEdit diagnosaTypeLookup = new RepositoryItemLookUpEdit();
                diagnosaTypeLookup.DataSource = listDiagnosaType;
                diagnosaTypeLookup.ValueMember = "diagnosaTypeCode";
                diagnosaTypeLookup.DisplayMember = "diagnosaTypeName";

                diagnosaTypeLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                diagnosaTypeLookup.DropDownRows = listDiagnosaType.Count;
                diagnosaTypeLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                diagnosaTypeLookup.AutoSearchColumnIndex = 1;
                diagnosaTypeLookup.NullText = "";
                gridView1.Columns[8].ColumnEdit = diagnosaTypeLookup;

                gridView1.Columns[4].Visible = false;
                gridView1.Columns[9].Visible = false;
                gridView1.Columns[10].Visible = false;
                gridView1.Columns[3].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[4].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                loading.CloseWaitForm();

                if (gridView1.RowCount > 0)
                {
                    simpleButton4.Enabled = true;
                }
                else
                {
                    simpleButton4.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void initData()
        {
            string sql_poli = " select item_cd, item_name from KLINIK.cs_diagnosa_item where status = 'A' ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_poli, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            listDiagnosa.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listDiagnosa.Add(new Diagnosa() { diagnosaCode = dt.Rows[i]["item_cd"].ToString(), diagnosaName = dt.Rows[i]["item_name"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            }

            listDiagnosaType.Clear();
            listDiagnosaType.Add(new DiagnosaType() { diagnosaTypeCode = "P", diagnosaTypeName = "Primary" });
            listDiagnosaType.Add(new DiagnosaType() { diagnosaTypeCode = "S", diagnosaTypeName = "Secondary" });
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //GridView view = sender as GridView;
            //if (e.Column.Caption != "Berobat") return;
            //// Fill a cell's background if its value is greater than 30. 
            //if (e.CellValue.ToString() == "MID")
            //{
            //    e.Appearance.BackColor = Color.FromArgb(40, Color.LightCoral);
            //}
            //else
            //{
            //    e.Appearance.BackColor = Color.FromArgb(40, Color.DodgerBlue);
            //}
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //GridView View = sender as GridView;
            //if (e.RowHandle >= 0)
            //{
            //    string pur = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
            //    string stat = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            //    if (stat == "Inspection" && pur == "Dokter")
            //    {
            //        //e.Appearance.BackColor = Color.FromArgb(40, Color.DodgerBlue);
            //        e.Appearance.BackColor = Color.DodgerBlue;
            //        //e.Appearance.BackColor2 = Color.White;
            //        e.Appearance.ForeColor = Color.White;
            //        //e.Appearance.Font = new Font("Arial", 9, FontStyle.Bold);
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }

            //    if (stat == "Inspection" && pur == "Bidan")
            //    {
            //        e.Appearance.BackColor = Color.LightCoral;
            //        //e.Appearance.BackColor2 = Color.White;
            //        e.Appearance.ForeColor = Color.White;
            //        e.Appearance.FontStyleDelta = FontStyle.Bold;
            //        e.HighPriority = true;
            //    }
            //}
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Antrian" || e.Column.Caption == "Pasien No" || e.Column.Caption == "Nama ICD" || e.Column.Caption == "Tipe Diagnosa")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Tipe Diagnosa")
            {
                string kk = View.GetRowCellDisplayText(e.RowHandle, View.Columns[8]);
                if (kk == "Primary")
                {
                    e.Appearance.BackColor = Color.FromArgb(150, Color.Blue);
                    e.Appearance.BackColor2 = Color.FromArgb(150, Color.Blue);
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }

           
        }

        private void gridView1_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
        {

        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            GridView view = sender as GridView;

            //string v1 = View.GetRowCellDisplayText(e.RowHandle, View.Columns[10]);
            //string p_que = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
            //string p_empid = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
            //string p_date = today;
            //string p_poli = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
            //string p_pasient = view.GetRowCellValue(e.RowHandle, view.Columns[7]).ToString();
            //string p_workA = view.GetRowCellValue(e.RowHandle, view.Columns[8]).ToString();
            //string p_purpose = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
            //string p_stat = view.GetRowCellValue(e.RowHandle, view.Columns[10]).ToString();
            //MessageBox.Show("Params: " + p_poli + ", " + p_pasient + ", " + p_workA + ", " + p_purpose + ", " + p_stat + ", " + p_que + ", " + p_empid + ", " + p_date);

            //string sql_update;

            //sql_update = " update cs_visit " +
            //             " set poli_cd = '" + p_poli + "', type_patient = '" + p_pasient + "', " +
            //             " work_accident = '" + p_workA + "', purpose = '" + p_purpose + "', status = '" + p_stat + "' " +
            //             " where que01 = '" + p_que  + "' and empid = '" + p_empid + "' and to_char(visit_date,'yyyy-mm-dd') = '" + p_date + "' ";
            try
            {
                //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                //OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                //oraConnect.Open();
                //cm.ExecuteNonQuery();
                //oraConnect.Close();
                //cm.Dispose();

                //MessageBox.Show("Query Exec : " + sql_update);
                
                //MessageBox.Show("Update Success");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridView1.AddNewRow();
            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView1.Columns[0].OptionsColumn.ReadOnly = false;
            gridView1.Columns[1].OptionsColumn.ReadOnly = false;
            gridView1.Columns[2].OptionsColumn.ReadOnly = false;
            
            //gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            //GridView view = sender as GridView;
            //view.SetRowCellValue(e.RowHandle, view.Columns[0], "D010");
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;
            
            if (e.Column.Caption == "Pasien No")
            {
                string p_empid = e.Value.ToString();
                string empid = "", name = "", dept = "", rm="", grp = "";
                string sql_emp = "";

                string tmp_pasno = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                string tmp_que = view.GetRowCellValue(e.RowHandle, view.Columns[1]).ToString();
                string tmp_date = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();

                string SQL = "";
                SQL = SQL + Environment.NewLine + "select (select poli_group from KLINIK.CS_POLICLINIC where poli_cd=v.poli_cd) p_group ";
                SQL = SQL + Environment.NewLine + "from KLINIK.cs_visit v ";
                SQL = SQL + Environment.NewLine + "where to_char(visit_date,'yyyy-mm-dd')='" + tmp_date + "' ";
                SQL = SQL + Environment.NewLine + "and que01='" + tmp_que + "' ";
                SQL = SQL + Environment.NewLine + "and patient_no='" + tmp_pasno + "' ";
                SQL = SQL + Environment.NewLine + "and poli_cd is not null ";

                OleDbConnection sqlConnect1 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql1 = new OleDbDataAdapter(SQL, sqlConnect1);
                DataTable dt1 = new DataTable();
                adSql1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    grp = dt1.Rows[0]["p_group"].ToString();
                }
                else
                {
                    grp = "";
                    view.SetColumnError(gridView1.Columns[2], "Data Reservasi tidak ditemukan");
                }

                sql_emp = sql_emp + Environment.NewLine + "select a.patient_no, name, null dept, ";
                sql_emp = sql_emp + Environment.NewLine + "(select rm_no from KLINIK.cs_patient where status='A' and patient_no=a.patient_no and group_patient='" + grp + "') rm_no  ";
                sql_emp = sql_emp + Environment.NewLine + "from KLINIK.cs_patient_info a ";
                sql_emp = sql_emp + Environment.NewLine + "where 1=1  ";
                sql_emp = sql_emp + Environment.NewLine + "and a.patient_no = '" + p_empid + "' ";


                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_emp, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    name = dt.Rows[0]["name"].ToString();
                    dept = dt.Rows[0]["dept"].ToString();
                    rm = dt.Rows[0]["rm_no"].ToString();
                }
                else
                {
                    empid = ""; dept = "";
                    view.SetColumnError(gridView1.Columns[2], "Data pasien tidak ditemukan");
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[3], name);
                view.SetRowCellValue(e.RowHandle, view.Columns[4], dept);
                view.SetRowCellValue(e.RowHandle, view.Columns[5], rm);
                
                view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
            }

            if (e.Column.Caption == "Nama ICD" || e.Column.Caption == "Tipe Diagnosa" || e.Column.Caption == "Remark")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[9]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "I");
                    simpleButton2.Enabled = true;
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], "U");
                    simpleButton2.Enabled = true;
                }
            }
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                     "Message",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", nik = "", tgl="", que="", id="";

                tgl = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                que = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                nik = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " delete KLINIK.cs_diagnosa ";
                sql_delete = sql_delete + " where diagnosa_id = '" + id + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    LoadData();
                    MessageBox.Show("Data Berhasil didelete");
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }

            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string tgl = "", que = "", nik = "", nama = "", rm_no = "", kode_diag = "", type_diag = "", action = "", remark="";
            string sql_check = "", sql_cnt = "", sql_insert = "", sql_update = "", cek="", anam_cnt="", id = "", sql_cnt2 = "", visit_cnt = "" ;
            DateTime result;

            cek = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                tgl = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                que = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nik = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                rm_no = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                kode_diag = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                type_diag = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                id = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                remark = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();

                if (action == "I")
                {
                    if (kode_diag == "")
                    {
                        MessageBox.Show("Diagnosa Harus diisi");
                    }
                    else if (type_diag == "")
                    {
                        MessageBox.Show("Tipe Diagnosa Harus diisi");
                    }
                    else
                    {
                        sql_cnt2 = " ";

                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "select count(0) cnt from KLINIK.cs_visit ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "where 1=1 ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "and patient_no='" + nik + "' ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "and to_char(visit_date,'yyyy-mm-dd') = '" + tgl + "' ";
                        sql_cnt2 = sql_cnt2 + Environment.NewLine + "and que01='" + que + "' ";
                        OleDbConnection oraConnect2a = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2a = new OleDbDataAdapter(sql_cnt2, oraConnect2a);
                        DataTable dt2a = new DataTable();
                        adOra2a.Fill(dt2a);
                        visit_cnt = dt2a.Rows[0]["cnt"].ToString();

                        if (Convert.ToInt32(visit_cnt) == 0)
                        {
                            MessageBox.Show("Data Reservasi tidak ditemukan, silahkan input data Reservasi");
                        }
                        else if (rm_no == "")
                        {
                            MessageBox.Show("Data Medical Record tidak ditemukan, silahkan input data Medical Record");
                        }
                        else
                        {

                            sql_insert = "  ";
                            
                            sql_insert = sql_insert + Environment.NewLine + "insert into KLINIK.cs_diagnosa ";
                            sql_insert = sql_insert + Environment.NewLine + "(diagnosa_id, rm_no, insp_date, visit_no, item_cd, type_diagnosa, remark, ins_date, ins_emp) ";
                            sql_insert = sql_insert + Environment.NewLine + "values  ";
                            sql_insert = sql_insert + Environment.NewLine + "(cs_diagnosa_seq.nextval,'" + rm_no + "',to_date('" + tgl + "','yyyy-mm-dd'),'" + que + "','" + kode_diag + "','" + type_diag + "', '" + remark + "', ";
                            sql_insert = sql_insert + Environment.NewLine + " sysdate, '" + DB.vUserId + "') ";

                            cek = cek + sql_insert;
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
                                MessageBox.Show("Data Berhasil disimpan.");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR: " + ex.Message);
                            }
                        }
                    }
                }
                else if (action == "U")
                {

                    sql_update = "";

                    sql_update = sql_update + Environment.NewLine + "update KLINIK.cs_diagnosa ";
                    sql_update = sql_update + Environment.NewLine + "set item_cd = '" + kode_diag + "', type_diagnosa = '" + type_diag + "', remark = '" + remark + "', ";
                    sql_update = sql_update + Environment.NewLine + "upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                    sql_update = sql_update + Environment.NewLine + "where diagnosa_id = '" + id + "' ";


                    cek = cek + sql_update;

                    try
                    {
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                        oraConnect.Open();
                        cm.ExecuteNonQuery();
                        oraConnect.Close();
                        cm.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_update);
                        LoadData();
                        MessageBox.Show("Data Berhasil diupdate");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
            }
            richTextBox1.Text = cek;
            //MessageBox.Show(action);
        }
    }
}
