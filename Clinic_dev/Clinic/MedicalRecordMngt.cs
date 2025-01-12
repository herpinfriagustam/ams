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
    public partial class MedicalRecordMngt : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Status> listStat = new List<Status>();
        List<Stat> listSt = new List<Stat>();

        public string v_empid = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";

        public MedicalRecordMngt()
        {
            InitializeComponent();
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            workingDirectory = Environment.CurrentDirectory;
            resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            initData();
            //LoadData();
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = " ";

            sql_search = sql_search + Environment.NewLine + "select rm_no, patient_no, name, dept, group_nm, group_patient, status, ";
            sql_search = sql_search + Environment.NewLine + "hamil_ke, anak_ke, minggu, gpa, hpht, ";
            sql_search = sql_search + Environment.NewLine + "tgl_cuti, taksiran, tgl_ambil, mulai, selesai, action ";
            sql_search = sql_search + Environment.NewLine + "from ( select rm_no, a.patient_no, b.name, null dept, ";
            sql_search = sql_search + Environment.NewLine + "decode(group_patient,'COMM','Umum','PREG','Ibu Hamil','KB') group_nm, group_patient, a.status, ";
            sql_search = sql_search + Environment.NewLine + "info01 hamil_ke, info02 minggu, info03 anak_ke, info04 gpa, info05 hpht, ";
            sql_search = sql_search + Environment.NewLine + "info06 tgl_cuti, info07 taksiran, info08 tgl_ambil, info09 mulai, info10 selesai, 'S' action ";
            sql_search = sql_search + Environment.NewLine + "from cs_patient a ";
            sql_search = sql_search + Environment.NewLine + "join cs_patient_info b on (a.patient_no=b.patient_no) ";
            sql_search = sql_search + Environment.NewLine + "where a.status='A') ";
            sql_search = sql_search + Environment.NewLine + "where group_nm='" + luType.Text + "' ";
            sql_search = sql_search + Environment.NewLine + "and patient_no like '" + tNik.Text + "%' ";

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

                //gridView1.FixedLineWidth = 5;
                //gridView1.Columns[0].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[1].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[2].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[3].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //gridView1.Columns[4].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[2].OptionsColumn.ReadOnly = true;
                gridView1.Columns[3].OptionsColumn.ReadOnly = true;
                gridView1.Columns[4].OptionsColumn.ReadOnly = true;
                gridView1.Columns[17].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Caption = "Med. Record No";
                gridView1.Columns[1].Caption = "Pasien No";
                gridView1.Columns[2].Caption = "Nama";
                gridView1.Columns[3].Caption = "Dept";
                gridView1.Columns[4].Caption = "Group cd";
                gridView1.Columns[5].Caption = "Group Record";
                gridView1.Columns[6].Caption = "Status";
                gridView1.Columns[7].Caption = "Hamil Ke";
                gridView1.Columns[8].Caption = "Anak Ke";
                gridView1.Columns[9].Caption = "Minggu";
                gridView1.Columns[10].Caption = "GPA";
                gridView1.Columns[11].Caption = "HPHT";
                gridView1.Columns[12].Caption = "Tgl Cuti";
                gridView1.Columns[13].Caption = "Taksiran";
                gridView1.Columns[14].Caption = "Tgl Ambil Surat";
                gridView1.Columns[15].Caption = "Tgl Mulai Cuti";
                gridView1.Columns[16].Caption = "Tgl Selesai Cuti";
                gridView1.Columns[17].Caption = "Action";

                gridView1.BestFitColumns();
                gridView1.Columns[3].Visible = false;
                gridView1.Columns[5].Visible = false;

                if (luType.Text != "Ibu Hamil")
                {
                    gridView1.Columns[7].Visible = false; gridView1.Columns[8].Visible = false; gridView1.Columns[9].Visible = false;
                    gridView1.Columns[10].Visible = false; gridView1.Columns[11].Visible = false; gridView1.Columns[12].Visible = false; gridView1.Columns[13].Visible = false;
                    gridView1.Columns[14].Visible = false; gridView1.Columns[15].Visible = false; gridView1.Columns[16].Visible = false;

                }


                RepositoryItemLookUpEdit stLookup = new RepositoryItemLookUpEdit();
                stLookup.DataSource = listSt;
                stLookup.ValueMember = "statCode";
                stLookup.DisplayMember = "statName";

                stLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                stLookup.DropDownRows = listSt.Count;
                stLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                stLookup.AutoSearchColumnIndex = 1;
                stLookup.NullText = "";
                gridView1.Columns[6].ColumnEdit = stLookup;

                gridView1.Columns[17].Visible = false;
                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[3].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

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
            listStat.Clear();
            listStat.Add(new Status() { statusCode = "COMM", statusName = "Umum" });
            listStat.Add(new Status() { statusCode = "PREG", statusName = "Ibu Hamil" });
            listStat.Add(new Status() { statusCode = "FAMP", statusName = "KB" });

            luType.Properties.DataSource = listStat;
            luType.Properties.ValueMember = "statusCode";
            luType.Properties.DisplayMember = "statusName";

            luType.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luType.Properties.DropDownRows = listStat.Count;
            luType.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luType.Properties.AutoSearchColumnIndex = 1;
            luType.Properties.NullText = "";
            luType.ItemIndex = 0;

            listSt.Clear();
            listSt.Add(new Stat() { statCode = "A", statName = "Active" });
            listSt.Add(new Stat() { statCode = "I", statName = "InActive" });
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

            if (e.Column.Caption == "Pasien No")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
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
            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView1.Columns[1].OptionsColumn.ReadOnly = false;
            gridView1.AddNewRow();
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
                string p_pasno = e.Value.ToString();
                string pasno = "", name = "", dept = "";
                string sql_emp = " select patient_no, name from cs_patient_info where 1 = 1 and patient_no = '" + p_pasno + "' ";

                OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adSql = new OleDbDataAdapter(sql_emp, sqlConnect);
                DataTable dt = new DataTable();
                adSql.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    name = dt.Rows[0]["name"].ToString();
                }
                else
                {
                    pasno = "";
                    view.SetColumnError(gridView1.Columns[1], "Employees Not Found");
                }

                view.SetRowCellValue(e.RowHandle, view.Columns[2], name);

                view.SetRowCellValue(e.RowHandle, view.Columns[4], luType.Text);
                view.SetRowCellValue(e.RowHandle, view.Columns[6], "A");
                view.SetRowCellValue(e.RowHandle, view.Columns[17], "I");
            }

            if (e.Column.Caption == "Status" || e.Column.Caption == "Hamil Ke" || e.Column.Caption == "Anak Ke" || e.Column.Caption == "Minggu" || 
                e.Column.Caption == "GPA" || e.Column.Caption == "HPHT" || e.Column.Caption == "Tgl Cuti" || e.Column.Caption == "Taksiran" ||
                e.Column.Caption == "Tgl Ambil Surat" || e.Column.Caption == "Tgl Mulai Cuti" || e.Column.Caption == "Tgl Selesai Cuti")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[17]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[17], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[17], "U");
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
                string sql_delete = "", pasno = "", rm_no="", group="";

                rm_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                group = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " delete cs_patient ";
                sql_delete = sql_delete + " where rm_no = '" + rm_no + "' and patient_no = '" + pasno + "' and group_patient = '" + group + "' ";

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
            string tmp_rm_no = "", tmp_group = "", no_rm = "", nama = "";
            string rm_no = "", pas_no = "", group = "", status = "";
            string info01 = "", info02 = "", info03 = "", info04 = "", info05 = "";
            string info06 = "", info07 = "", info08 = "", info09 = "", info10 = "";
            string sql_check = "", sql_cnt = "", sql_insert="", sql_update = "", cek = "", action="", cek_rm = "";
            int queue = 0, visit=0;
            DateTime result;

            cek = "";
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                no_rm = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                pas_no = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                tmp_group = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                if (tmp_group == "Umum") { group = "COMM"; tmp_rm_no = "C"; }
                else if (tmp_group == "Ibu Hamil") { group = "PREG"; tmp_rm_no = "P"; }
                else { group = "FAMP"; tmp_rm_no = "F"; }
                rm_no = pas_no.Replace("P", "");

                status = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                info01 = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                info03 = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                info02 = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                info04 = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                info05 = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                info06 = gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
                info07 = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
                info08 = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();
                info09 = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString();
                info10 = gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[17]).ToString();

                if (tmp_group != "Ibu Hamil")
                {
                    info06 = "2020-01-01"; info08 = "2020-01-01"; info09 = "2020-01-01"; info10 = "2020-01-01";
                }

                if (nama == "")
                {
                    MessageBox.Show("Data Pasien tidak ditemukan");
                    return;
                }

                if (action == "I")
                {
                    if (!DateTime.TryParseExact(
                             info06,
                             "yyyy-MM-dd",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.AssumeUniversal,
                             out result))
                    {
                        MessageBox.Show("Format tanggal harus yyyy-mm-dd");
                    }
                    else if (!DateTime.TryParseExact(
                             info08,
                             "yyyy-MM-dd",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.AssumeUniversal,
                             out result))
                    {
                        MessageBox.Show("Format tanggal harus yyyy-mm-dd");
                    }
                    else if (!DateTime.TryParseExact(
                             info09,
                             "yyyy-MM-dd",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.AssumeUniversal,
                             out result))
                    {
                        MessageBox.Show("Format tanggal harus yyyy-mm-dd");
                    }
                    else if (!DateTime.TryParseExact(
                             info10,
                             "yyyy-MM-dd",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.AssumeUniversal,
                             out result))
                    {
                        MessageBox.Show("Format tanggal harus yyyy-mm-dd");
                    }
                    else
                    {
                        sql_cnt = " select count(0) cnt from cs_patient where status='A' and patient_no = '" + pas_no + "' and rm_no = '" + tmp_rm_no + "' || to_char(sysdate,'yymmdd') ||'" + rm_no + "' ";
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_cnt, oraConnect2);
                        DataTable dt2 = new DataTable();
                        adOra2.Fill(dt2);
                        cek_rm = dt2.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(cek_rm) > 0)
                        {
                            //MessageBox.Show("Employee ID " + nik + " sudah terdaftar.");
                        }
                        else
                        {

                            sql_insert = sql_insert + Environment.NewLine + "insert into cs_patient ";
                            sql_insert = sql_insert + Environment.NewLine + "(rm_no, patient_no, group_patient, status, ";
                            sql_insert = sql_insert + Environment.NewLine + "info01, info02, info03, info04, info05, ";
                            sql_insert = sql_insert + Environment.NewLine + "info07, info06, info08, info09, info10, ";
                            sql_insert = sql_insert + Environment.NewLine + "ins_date, ins_emp) ";
                            sql_insert = sql_insert + Environment.NewLine + "values  ";
                            sql_insert = sql_insert + Environment.NewLine + "('" + tmp_rm_no + "' || to_char(sysdate,'yymmdd') || '" + rm_no + "' , '" + pas_no + "', '" + group + "','" + status + "', ";
                            sql_insert = sql_insert + Environment.NewLine + " '" + info01 + "','" + info02 + "','" + info03 + "','" + info04 + "','" + info05 + "', '" + info07 + "', ";
                            if (tmp_group != "Ibu Hamil")
                            {
                                sql_insert = sql_insert + Environment.NewLine + " '','','','', ";
                            }
                            else
                            {
                                sql_insert = sql_insert + Environment.NewLine + " '" + info06 + "','" + info08 + "','" + info09 + "','" + info10 + "', ";
                            }
                            sql_insert = sql_insert + Environment.NewLine + " sysdate, '" + v_empid + "') ";

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

                    sql_update = sql_update + Environment.NewLine + "update cs_patient ";
                    sql_update = sql_update + Environment.NewLine + "set status = '" + status + "', info01 = '" + info01 + "', info02 = '" + info02 + "', info03 = '" + info03 + "', ";
                    sql_update = sql_update + Environment.NewLine + "info04 = '" + info04 + "', info05 = '" + info05 + "', info07 = '" + info07 + "', ";
                    if (tmp_group != "Ibu Hamil")
                    {
                        sql_update = sql_update + Environment.NewLine + "info06 = '', info08 = '', info09 = '', info10 = '', ";
                    }
                    else
                    {
                        sql_update = sql_update + Environment.NewLine + "info06 = '" + info06 + "', info08 = '" + info08 + "', info09 = '" + info09 + "', info10 = '" + info10 + "', ";
                    }
                    
                    sql_update = sql_update + Environment.NewLine + "upd_date = sysdate, upd_emp = '" + v_empid + "' ";
                    sql_update = sql_update + Environment.NewLine + "where rm_no = '" + no_rm + "' and patient_no = '" + pas_no + "' and group_patient = '" + group + "' ";

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
