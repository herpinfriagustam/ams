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
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;

namespace Clinic
{
    public partial class PatientInfoMngt : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Stat> listSt = new List<Stat>();
        List<Stat> listjk = new List<Stat>();
        List<Stat> listkrj = new List<Stat>();
        List<Stat> listkwn = new List<Stat>();
        List<Stat> listkls = new List<Stat>();
        RepositoryItemDateEdit repositoryItemDateEdit1;
        DataSet dsKartu = new DataSet();

        //public string DB.vUserId = "";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";

        public PatientInfoMngt()
        {
            InitializeComponent();
            foreach (GridColumn column in gridView1.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.DateTime)
                {
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    column.DisplayFormat.FormatString = @"dd\/MM\/yyyy";
                }
            }
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            //workingDirectory = Environment.CurrentDirectory;
            //resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            initData();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "PatientInfoMngt");
            //LoadData();
        }

        private void LoadData()
        {
            string sql_search;

            sql_search = " ";

            sql_search = sql_search + Environment.NewLine + "select patient_no, nid, name, birth_place, to_date(to_char(birth_date,'yyyy-MM-dd'),'yyyy-MM-dd') birth_date,  ";
            sql_search = sql_search + Environment.NewLine + "gender, address, city,insu_class, insu_no, status, 'S' action, job, family_head, phone,  ";
            sql_search = sql_search + Environment.NewLine + "insu_no2, insu_nm2, rfid_no, company, company_addr ";
            sql_search = sql_search + Environment.NewLine + "from cs_patient_info ";
            sql_search = sql_search + Environment.NewLine + "where 1=1 ";
            if (cmbStatus.Text == "Aktif")
            {
                sql_search = sql_search + Environment.NewLine + "and status = 'A' ";
            }
            else
            {
                sql_search = sql_search + Environment.NewLine + "and status = 'I' ";
            }

            if (cmbSearch.Text == "Nama")
            {
                sql_search = sql_search + Environment.NewLine + "and name like '%" + tNik.Text + "%' ";
            }
            else if (cmbSearch.Text == "No.KTP")
            {
                sql_search = sql_search + Environment.NewLine + "and nid like '%" + tNik.Text + "%' ";
            }
            else if (cmbSearch.Text == "No.HP")
            {
                sql_search = sql_search + Environment.NewLine + "and phone like '%" + tNik.Text + "%' ";
            }
            else
            {
                sql_search = sql_search + Environment.NewLine + "and insu_no like '%" + tNik.Text + "%' ";
            }
            sql_search = sql_search + Environment.NewLine + "order by name ";

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
                gridView1.Columns[10].OptionsColumn.ReadOnly = true;

                gridView1.Columns[0].Caption = "Pasien No";
                gridView1.Columns[1].Caption = "No.KTP";
                gridView1.Columns[2].Caption = "Nama";
                gridView1.Columns[3].Caption = "Tempat Lahir";
                gridView1.Columns[4].Caption = "Tgl Lahir";
                gridView1.Columns[5].Caption = "Jenis Kelamin";
                gridView1.Columns[6].Caption = "Alamat";
                gridView1.Columns[7].Caption = "Kota";
                gridView1.Columns[8].Caption = "Kelas BPJS";
                gridView1.Columns[9].Caption = "No.BPJS";
                gridView1.Columns[10].Caption = "Status";
                gridView1.Columns[11].Caption = "Action";
                gridView1.Columns[12].Caption = "Pekerjaan";
                gridView1.Columns[13].Caption = "Kepala Keluarga";
                gridView1.Columns[14].Caption = "No.HP"; 
                gridView1.Columns[15].Caption = "No Asuransi";
                gridView1.Columns[16].Caption = "Nama Asuransi";
                gridView1.Columns[17].Caption = "No RFID";
                gridView1.Columns[18].Caption = "Perusahaan";
                gridView1.Columns[19].Caption = "Alamat Perusahaan";

                //gridView1.Columns[4].ColumnEdit = redate;
                //gridView1.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                //gridView1.Columns[4].DisplayFormat.FormatString = "yyyy-mm-dd";

                RepositoryItemLookUpEdit jkLookup = new RepositoryItemLookUpEdit();
                jkLookup.DataSource = listjk;
                jkLookup.ValueMember = "statCode";
                jkLookup.DisplayMember = "statName";

                jkLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                jkLookup.DropDownRows = listjk.Count+1;
                jkLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                jkLookup.AutoSearchColumnIndex = 1;
                jkLookup.NullText = "";
                gridView1.Columns[5].ColumnEdit = jkLookup;

                RepositoryItemLookUpEdit stLookup = new RepositoryItemLookUpEdit();
                stLookup.DataSource = listSt;
                stLookup.ValueMember = "statCode";
                stLookup.DisplayMember = "statName";

                stLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                stLookup.DropDownRows = listSt.Count + 1;
                stLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                stLookup.AutoSearchColumnIndex = 1;
                stLookup.NullText = "";
                gridView1.Columns[10].ColumnEdit = stLookup;

                RepositoryItemLookUpEdit stkerja = new RepositoryItemLookUpEdit();
                stkerja.DataSource = listkrj;
                stkerja.ValueMember = "statCode";
                stkerja.DisplayMember = "statName";

                stkerja.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                stkerja.DropDownRows = listSt.Count + 1;
                stkerja.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                stkerja.AutoSearchColumnIndex = 1;
                stkerja.NullText = "";
                gridView1.Columns[12].ColumnEdit = stkerja;

                RepositoryItemLookUpEdit stkelas = new RepositoryItemLookUpEdit();
                stkelas.DataSource = listkls;
                stkelas.ValueMember = "statCode";
                stkelas.DisplayMember = "statName";

                stkelas.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                stkelas.DropDownRows = listSt.Count + 1;
                stkelas.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                stkelas.AutoSearchColumnIndex = 1;
                stkelas.NullText = "";
                gridView1.Columns[8].ColumnEdit = stkelas;

                gridView1.BestFitColumns();

                gridView1.Columns[11].Visible = false;
                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[6].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[8].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

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
            listSt.Clear();
            listSt.Add(new Stat() { statCode = "A", statName = "Aktif" });
            listSt.Add(new Stat() { statCode = "I", statName = "Tidak Aktif" });

            listjk.Clear();
            listjk.Add(new Stat() { statCode = "L", statName = "Laki-laki" });
            listjk.Add(new Stat() { statCode = "P", statName = "Perempuan" });

            listkrj.Clear();
            listkrj.Add(new Stat() { statCode = "PNS", statName = "PNS" });
            listkrj.Add(new Stat() { statCode = "TNI/POLRI", statName = "TNI/POLRI" });
            listkrj.Add(new Stat() { statCode = "Swasta", statName = "Swasta" });
            listkrj.Add(new Stat() { statCode = "A", statName = "Petani" });
            listkrj.Add(new Stat() { statCode = "B", statName = "Buruh" });
            listkrj.Add(new Stat() { statCode = "K", statName = "Karyawan" });

            listkwn.Clear();
            listkwn.Add(new Stat() { statCode = "K", statName = "Kawin" });
            listkwn.Add(new Stat() { statCode = "T", statName = "Tidak Kawin" });
            listkwn.Add(new Stat() { statCode = "D", statName = "Duda" });
            listkwn.Add(new Stat() { statCode = "J", statName = "Janda" });

            listkls.Clear();
            listkls.Add(new Stat() { statCode = "1", statName = "Satu" });
            listkls.Add(new Stat() { statCode = "2", statName = "Dua" });
            listkls.Add(new Stat() { statCode = "3", statName = "Tiga" });

            cmbSearch.Items.Clear();
            cmbSearch.Items.Add("Nama");
            cmbSearch.Items.Add("No.KTP");
            cmbSearch.Items.Add("No.BPJS");
            cmbSearch.Items.Add("No.HP");
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
            view.SetRowCellValue(e.RowHandle, view.Columns[10], "A");
            view.SetRowCellValue(e.RowHandle, view.Columns[11], "I");
            //view.Columns[9].OptionsColumn.ReadOnly = true;
            view.Columns[10].OptionsColumn.ReadOnly = true ;
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "No.KTP" || e.Column.Caption == "Nama" || e.Column.Caption == "Tempat Lahir" || e.Column.Caption == "Tgl Lahir" || 
                e.Column.Caption == "Jenis Kelamin" || e.Column.Caption == "Alamat" || e.Column.Caption == "Kota" || e.Column.Caption == "No.BPJS" || 
                e.Column.Caption == "Status" || e.Column.Caption == "Pekerjaan" || e.Column.Caption == "Kepala Keluarga" || e.Column.Caption == "No.HP" || 
                e.Column.Caption == "Kelas BPJS"|| e.Column.Caption == "No Asuransi" || e.Column.Caption == "Nama Asuransi" || e.Column.Caption == "No RFID" ||
                e.Column.Caption == "Perusahaan" || e.Column.Caption == "Alamat Perusahaan")
            {
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], "U");
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

                sql_delete = sql_delete + " update cs_patient_info ";
                sql_delete = sql_delete + " set status = 'I' ";
                sql_delete = sql_delete + " where patient_no = '" + pasien_no + "' ";

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
            string tmp_pas_no = "", pas_no = "", ktp = "", nama = "", tmt_lahir = "";
            string tgl_lahir = "", jk = "", alamat = "", kota = "", bpjs = "", stat = "", job = "", kk = "", nohp = "", kls = "", noinsu2="", nminsu2="", rfid="", comp="", comp_addr="";
            string sql_check = "", sql_cnt = "", sql_insert="", sql_update = "", action="", cek_p = "";
            int queue = 0, visit=0;
            DateTime result;
            
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                
                pas_no = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
                ktp = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
                nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
                tmt_lahir = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
                tgl_lahir = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString().Substring(0,10);
                jk = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
                alamat = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
                kota = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
                kls = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
                bpjs = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
                stat = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
                action = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
                job= gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
                kk = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
                nohp = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString(); 
                noinsu2 = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString();
                nminsu2 = gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString();
                rfid = gridView1.GetRowCellValue(i, gridView1.Columns[17]).ToString();
                comp = gridView1.GetRowCellValue(i, gridView1.Columns[18]).ToString();
                comp_addr = gridView1.GetRowCellValue(i, gridView1.Columns[19]).ToString();

                if (action == "I")
                {
                    if (!DateTime.TryParseExact(
                             tgl_lahir,
                             "dd/MM/yyyy",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.AssumeUniversal,
                             out result))
                    {
                        MessageBox.Show("Format tanggal harus dd/MM/yyyy. Silahkan ubah format tanggal komputer anda.");
                    }
                    else if (nama == "")
                    {
                        MessageBox.Show("Nama Pasien harus diisi.");
                    }
                    else if (jk == "")
                    {
                        MessageBox.Show("Jenis Kelamin harus diisi.");
                    }
                    else if (alamat == "")
                    {
                        MessageBox.Show("Alamat harus diisi.");
                    }
                    else if (kk == "")
                    {
                        MessageBox.Show("Kepala Keluarga harus diisi.");
                    }
                    else if (nohp == "")
                    {
                        MessageBox.Show("No HP harus diisi.");
                    }
                    else
                    {
                        sql_check = " select count(0) cnt from cs_patient_info where insu_no is not null and insu_no = '" + bpjs + "'  ";
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_check, oraConnect2);
                        DataTable dt2 = new DataTable();
                        adOra2.Fill(dt2);
                        cek_p = dt2.Rows[0]["cnt"].ToString();

                        sql_cnt = " select 'P' || to_char(sysdate,'yymm') || lpad(count(0)+1,3,'0') pno from cs_patient_info where to_char(ins_date, 'yyyymm') = to_char(sysdate, 'yyyymm')  ";
                        OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_cnt, oraConnect4);
                        DataTable dt4 = new DataTable();
                        adOra4.Fill(dt4);
                        tmp_pas_no = dt4.Rows[0]["pno"].ToString();
                        if (Convert.ToInt32(cek_p) > 0)
                        {
                            MessageBox.Show("No BPJS " + bpjs + " sudah terdaftar.");
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

                                command.CommandText = " insert into cs_patient_info (patient_no, nid, name, birth_place, birth_date, gender, address, " +
                                                      " city, insu_no, status, job, family_head, " +
                                                      " phone, insu_class, insu_no2, insu_nm2, rfid_no, company, company_addr, ins_date, ins_emp) values " +
                                                      " ( '" + tmp_pas_no + "', '" + ktp + "','" + nama + "',  '" + tmt_lahir + "',to_date('" + tgl_lahir.Substring(0, 10).ToString() + "','dd/MM/yyyy'),'" + jk + "','" + alamat + "', " +
                                                      " '" + kota + "', '" + bpjs + "', '" + stat + "', '" + job + "', '" + kk + "', " +
                                                      " '" + nohp + "', '" + kls + "', '" + noinsu2 + "', '" + nminsu2 + "', '" + rfid + "', '" + comp + "', '" + comp_addr + "', sysdate, '" + DB.vUserId + "') ";

                                command.ExecuteNonQuery();

                                command.CommandText = " insert into cs_patient (rm_no, patient_no, group_patient, status, ins_date, ins_emp) " +
                                                      " values ('C' || to_char(sysdate,'yymmdd') || replace('" + tmp_pas_no + "','P'), '" + tmp_pas_no + "', 'COMM', 'A', sysdate, '" + DB.vUserId + "') ";

                                command.ExecuteNonQuery();

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

                    sql_update = sql_update + Environment.NewLine + "update cs_patient_info ";
                    sql_update = sql_update + Environment.NewLine + "set nid = '" + ktp + "', name = '" + nama + "', birth_place = '" + tmt_lahir + "', birth_date = to_date('" + tgl_lahir.Substring(0, 10).ToString() + "','dd/MM/yyyy'), ";
                    sql_update = sql_update + Environment.NewLine + "gender = '" + jk + "', address = '" + alamat + "', city = '" + kota + "', insu_no = '" + bpjs + "', status = '" + stat + "', job = '" + job + "', family_head = '" + kk + "', phone = '" + nohp + "', insu_class = '" + kls + "', ";
                    sql_update = sql_update + Environment.NewLine + "insu_no2 = '" + noinsu2 + "', insu_nm2 = '" + nminsu2 + "', rfid_no = '" + rfid + "', company = '" + comp + "', company_addr = '" + comp_addr + "', ";
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

        private void btnCetak_Click(object sender, EventArgs e)
        {
            string sql = "";
            string p_pasno = "";

            if (gridView1.RowCount > 0)
            {
                p_pasno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();

                sql = "  select name, a.patient_no, b.rm_no, " +
                      "  birth_place || ', ' || to_char(birth_date,'fmdd Month yyyy', 'nls_date_language = INDONESIAN') as ttl, " +
                      "  round(((sysdate-birth_date)/30)/12) age, " +
                      "  '(' || gender || ')' jk, address alamat, family_head kk " +
                      "  from cs_patient_info a " +
                      "  join cs_patient b on a.patient_no=b.patient_no " +
                      "  where a.patient_no = '" + p_pasno + "' ";

                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                dsKartu.Tables.Clear();
                dsKartu.Tables.Add(dt);

                ReportCard report = new ReportCard(dsKartu);
                report.ShowPreviewDialog();
            }
        }
    }
}
