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
using Clinic.Report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Columns;

namespace Clinic
{
    public partial class GuarantorMngt : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Stat> listSt = new List<Stat>();
        List<Stat> listjk = new List<Stat>();
        List<Stat> listPas = new List<Stat>();
        DataSet dsKartu = new DataSet();
        DataTable dtGlPas = new DataTable();

        //public string DB.vUserId = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";

        public GuarantorMngt()
        {
            InitializeComponent();

            foreach (GridColumn column in gridView1.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.String)
                {
                    column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                }
            }
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            //workingDirectory = Environment.CurrentDirectory;
            //resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            initData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "GuarantorMngt");
            //LoadData();
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = " ";
             
            sql_search = sql_search + Environment.NewLine + "select distinct d.gr_no, b.patient_no, b.patient_no, a.nid, a.name, a.relation,  ";
            sql_search = sql_search + Environment.NewLine + "       a.birth_place, null birth_date,   ";
            sql_search = sql_search + Environment.NewLine + "       null gender, a.address, a.city, a.insu_no, a.status, 'S' action, a.job, a.phone  ";
            sql_search = sql_search + Environment.NewLine + " from KLINIK.cs_guarantor a  ";
            sql_search = sql_search + Environment.NewLine + " join KLINIK.cs_patient_info b on a.patient_no=b.patient_no  ";
            sql_search = sql_search + Environment.NewLine + " join KLINIK.cs_visit c on a.PATIENT_NO = c.PATIENT_NO and c.STATUS not in('CLS','CAN') ";
            sql_search = sql_search + Environment.NewLine + " join cs_inpatient d on c.INPATIENT_ID = d.INPATIENT_ID    ";
            sql_search = sql_search + Environment.NewLine + "where 1=1  ";

            if (cmbStatus.Text == "Aktif")
            {
                sql_search = sql_search + Environment.NewLine + "and C.status NOT IN ('CLS','CAN') ";
            }
            else
            {
                sql_search = sql_search + Environment.NewLine + "and a.status = 'I'";
            }

            if (cmbSearch.Text == "Nama")
            {
                sql_search = sql_search + Environment.NewLine + "and a.name like '%" + tNik.Text + "%' ";
            }
            else if (cmbSearch.Text == "No.KTP")
            {
                sql_search = sql_search + Environment.NewLine + "and a.nid like '%" + tNik.Text + "%' ";
            }
            else if (cmbSearch.Text == "No.Hp")
            {
                sql_search = sql_search + Environment.NewLine + "and a.phone like '%" + tNik.Text + "%' ";
            }
            else
            {
                sql_search = sql_search + Environment.NewLine + "and a.insu_no like '%" + tNik.Text + "%' ";
            }
            sql_search = sql_search + Environment.NewLine + "order by a.name ";

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

                //gridView1.Columns[0].OptionsColumn.ReadOnly = true;
                //gridView1.Columns[10].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Caption = "Penjamin No";
                gridView1.Columns[1].Caption = "Pasien No";
                gridView1.Columns[2].Caption = "Nama Pasien";
                gridView1.Columns[3].Caption = "No.KTP Penjamin";
                gridView1.Columns[4].Caption = "Nama Penjamin";
                gridView1.Columns[5].Caption = "Hubungan";
                gridView1.Columns[6].Caption = "Tempat Lahir";
                gridView1.Columns[7].Caption = "Tgl Lahir";
                gridView1.Columns[8].Caption = "Jenis Kelamin";
                gridView1.Columns[9].Caption = "Alamat";
                gridView1.Columns[10].Caption = "Kota";
                gridView1.Columns[11].Caption = "No.BPJS";
                gridView1.Columns[12].Caption = "Status";
                gridView1.Columns[13].Caption = "Action";
                gridView1.Columns[14].Caption = "Pekerjaan";
                gridView1.Columns[15].Caption = "No.Hp";

                RepositoryItemGridLookUpEdit glPas = new RepositoryItemGridLookUpEdit();
                glPas.DataSource = listPas;
                glPas.ValueMember = "statCode";
                glPas.DisplayMember = "statName";

                glPas.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                glPas.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
                glPas.ImmediatePopup = true;
                glPas.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                glPas.NullText = "";
                glPas.PopupFilterMode = PopupFilterMode.Contains;
                gridView1.Columns[2].ColumnEdit = glPas;

                RepositoryItemLookUpEdit jkLookup = new RepositoryItemLookUpEdit();
                jkLookup.DataSource = listjk;
                jkLookup.ValueMember = "statCode";
                jkLookup.DisplayMember = "statName";

                jkLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                jkLookup.DropDownRows = listjk.Count;
                jkLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                jkLookup.AutoSearchColumnIndex = 1;
                jkLookup.NullText = ""; 
                gridView1.Columns[8].ColumnEdit = jkLookup;

                RepositoryItemLookUpEdit stLookup = new RepositoryItemLookUpEdit();
                stLookup.DataSource = listSt;
                stLookup.ValueMember = "statCode";
                stLookup.DisplayMember = "statName";

                stLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                stLookup.DropDownRows = listSt.Count;
                stLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                stLookup.AutoSearchColumnIndex = 1;
                stLookup.NullText = "";
                gridView1.Columns[12].ColumnEdit = stLookup;

                gridView1.BestFitColumns();
                gridView1.Columns[1].OptionsColumn.ReadOnly = true;
                gridView1.Columns[0].Visible = false;
                gridView1.Columns[6].Visible = false;
                gridView1.Columns[7].Visible = false;
                gridView1.Columns[8].Visible = false;
                gridView1.Columns[13].Visible = false;
                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[3].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[4].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[9].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[11].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[15].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[9].OptionsColumn.ReadOnly = false ;
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
            initData();
        }

        private void initData()
        {

            dtGlPas.Clear();
            string sql_poli = " select patient_no, name from KLINIK.cs_patient_info where 1=1 and status = 'A' order by name ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_poli, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            dtGlPas = dt;
            listPas.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listPas.Add(new Stat() { statCode = dt.Rows[i]["patient_no"].ToString(), statName = dt.Rows[i]["name"].ToString() });
                //poli.poliCode = dt2.Rows[i]["poli_cd"].ToString();
                //poli.poliName = dt2.Rows[i]["poli_name"].ToString();
                //listPoli.Add(poli);
            }

            listSt.Clear();
            listSt.Add(new Stat() { statCode = "A", statName = "Aktif" });
            listSt.Add(new Stat() { statCode = "I", statName = "Tidak Aktif" });

            listjk.Clear();
            listjk.Add(new Stat() { statCode = "L", statName = "Laki-laki" });
            listjk.Add(new Stat() { statCode = "P", statName = "Perempuan" });

            cmbSearch.Items.Clear();
            cmbSearch.Items.Add("Nama");
            cmbSearch.Items.Add("No.KTP");
            cmbSearch.Items.Add("No.BPJS");
            cmbSearch.Items.Add("No.Hp");
            cmbSearch.SelectedIndex = 0;

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Aktif");
            cmbStatus.Items.Add("Tidak Aktif");
            cmbStatus.SelectedIndex = 0;
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

            if (e.Column.Caption == "Action" || e.Column.Caption == "Pasien No")
            {

            }
            else
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
            gridView1.AddNewRow();
            //gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns[12], "A");
            view.SetRowCellValue(e.RowHandle, view.Columns[13], "I");
            view.Columns[9].OptionsColumn.ReadOnly = true;
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            string tmp_name = "";

            if (e.Column.Caption == "Nama Pasien")
            {
                string tmp_nopas = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();

                string sql_nm = " select patient_no from KLINIK.cs_patient_info where patient_no = '" + tmp_nopas + "' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_nm, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                tmp_name = dt.Rows[0]["patient_no"].ToString();

                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[13]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[1], tmp_name);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[1], tmp_name);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], "U");
                    simpleButton2.Enabled = true;
                }
            }

            if (e.Column.Caption == "No.KTP Penjamin" || e.Column.Caption == "Nama Pasien" || e.Column.Caption == "Nama Penjamin" || e.Column.Caption == "Tempat Lahir" || e.Column.Caption == "Tgl Lahir" || 
                e.Column.Caption == "Jenis Kelamin" || e.Column.Caption == "Alamat" || e.Column.Caption == "Kota" || e.Column.Caption == "No.BPJS" || e.Column.Caption == "Status" || e.Column.Caption == "No.Hp" ||
                e.Column.Caption == "Hubungan" || e.Column.Caption == "Pekerjaan")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[13]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], "U");
                    simpleButton2.Enabled = true;
                }
            }
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menonaktifkan data?",
                     "Message",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", pasien_no="";

                pasien_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + " update KLINIK.cs_guarantor ";
                sql_delete = sql_delete + " set status = 'I' ";
                sql_delete = sql_delete + " where gr_no = '" + pasien_no + "' ";

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
                    MessageBox.Show("Data Berhasil di non aktifkan");
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }

            }
        }
        
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string tmp_pas_no = "", pas_no = "", ktp = "", nama = "", tmt_lahir = "", p_jamin = "";
            string tgl_lahir = "", jk = "", alamat = "", kota = "", bpjs = "", stat = "", job = "", kk = "";
            string sql_check = "", sql_cnt = "", sql_insert="", sql_update = "", action="", cek_p = "";
            string p_nama = "", hub = "", no_hp = "";
            int queue = 0, visit=0;
            DateTime result;
            
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                p_jamin = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                pas_no = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                ktp = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                p_nama = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString();
                hub = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                tmt_lahir = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                tgl_lahir = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                jk = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                alamat = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                kota = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                bpjs = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                stat = gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
                job= gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString();
                no_hp = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString();

                if (action == "I")
                {
                    if (p_nama == "")
                    {
                        MessageBox.Show("Nama Penjamin harus diisi.");
                    }
                    else if (hub == "")
                    {
                        MessageBox.Show("Hubungan harus diisi.");
                    }
                    else if (no_hp == "")
                    {
                        MessageBox.Show("No HP harus diisi.");
                    }
                    else
                    {
                        sql_check = " select count(0) cnt from KLINIK.cs_guarantor where insu_no is not null and insu_no = '" + bpjs + "'  ";
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_check, oraConnect2);
                        DataTable dt2 = new DataTable();
                        adOra2.Fill(dt2);
                        cek_p = dt2.Rows[0]["cnt"].ToString();

                        sql_cnt = " select 'G' || to_char(sysdate,'yymm') || lpad(count(0)+1,3,'0') pno from KLINIK.cs_guarantor where to_char(ins_date, 'yyyymm') = to_char(sysdate, 'yyyymm')  ";
                        OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_cnt, oraConnect4);
                        DataTable dt4 = new DataTable();
                        adOra4.Fill(dt4);
                        tmp_pas_no = dt4.Rows[0]["pno"].ToString();
                        if (Convert.ToInt32(cek_p) > 0)
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


                            try
                            {

                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                command.CommandText = " insert into KLINIK.cs_guarantor (gr_no, patient_no, nid, name, relation, birth_place, birth_date, gender, address, city, insu_no, status, job, phone, ins_date, ins_emp) values ( '" + tmp_pas_no + "', '" + pas_no + "', '" + ktp + "','" + p_nama + "', '" + hub + "',  '" + tmt_lahir + "',to_date('" + tgl_lahir + "','yyyy-mm-dd'),'" + jk + "','" + alamat + "','" + kota + "', '" + bpjs + "', '" + stat + "', '" + job + "', '" + no_hp + "', sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                //command.CommandText = " insert into cs_patient (rm_no, patient_no, group_patient, status, ins_date, ins_emp) values ('C' || to_char(sysdate,'yymmdd') || replace('" + tmp_pas_no + "','P'), '" + tmp_pas_no + "', 'COMM', 'A', sysdate, '" + DB.vUserId + "') ";
                                //ommand.ExecuteNonQuery();

                                trans.Commit();

                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + sql);
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
                }
                else if (action == "U")
                {
                    sql_update = "";

                    sql_update = sql_update + Environment.NewLine + "update KLINIK.cs_guarantor ";
                    sql_update = sql_update + Environment.NewLine + "set nid = '" + ktp + "', name = '" + p_nama + "', birth_place = '" + tmt_lahir + "', birth_date = to_date('" + tgl_lahir + "','yyyy-mm-dd'), ";
                    sql_update = sql_update + Environment.NewLine + "gender = '" + jk + "', address = '" + alamat + "', city = '" + kota + "', insu_no = '" + bpjs + "', status = '" + stat + "', job = '" + job + "', phone = '" + no_hp + "', ";
                    sql_update = sql_update + Environment.NewLine + "upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
                    sql_update = sql_update + Environment.NewLine + "where patient_no = '" + pas_no + "' ";

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
            //richTextBox1.Text = cek;
            //MessageBox.Show(action);
        }
        
    }
}
