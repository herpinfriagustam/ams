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
using System.IO.Ports;

namespace Clinic
{
    public partial class PatientInfo : DevExpress.XtraEditors.XtraForm
    {
        private KoneksiOra koneksi;
        ConnectDb ConnOra = new ConnectDb();
        KoneksiOra conn = new KoneksiOra();
        DB cnn = new DB();
        List<Stat> listSt = new List<Stat>();
        List<Stat> listjk = new List<Stat>();
        List<Stat> listkrj = new List<Stat>();
        List<Stat> listkwn = new List<Stat>();
        List<Stat> listkls = new List<Stat>();
        List<Stat> listPT = new List<Stat>();
        RepositoryItemDateEdit repositoryItemDateEdit1;
        DataSet dsKartu = new DataSet();
        DataTable dt_provinsi = new DataTable();
        DataTable dt_kbputen = new DataTable();
        DataTable dt_kecamatan = new DataTable();
        DataTable dt_kelurahan = new DataTable();
        DataTable dt_kota  = new DataTable();
        DataTable dt_pasien = new DataTable();

        string InputData_scanner = String.Empty;
        delegate void SetTextCallback(string text);

        public string   sql = "", RFIDSCAN ="";
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";

        public PatientInfo()
        {
            InitializeComponent();
            foreach (GridColumn column in gridView1.Columns)
            {
                if (Type.GetTypeCode(column.ColumnType) == TypeCode.DateTime)
                {
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    column.DisplayFormat.FormatString = @"yyyy-MM-dd";
                }
            }
        }

        private void ReservationInput_Load(object sender, EventArgs e)
        {
            //workingDirectory = Environment.CurrentDirectory;
            //resourcesDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Resources\\";
            initData(); DataArea(); DataKota();
            ConnOra.InsertHistoryAkses(DB.vUserId, ConnOra.my_IP, "PatientInfo");
            //var port = new SerialPort("COM1");
            //try
            //{
            //    serialPort1.Open();
            //    serialPort1.DataReceived += serialPort1_DataReceived;
            //}
            //catch (Exception ex)
            //{
            //    ///*_formMain*/.CloseWaitForm();
            //    MessageBox.Show("ERROR : COM1 Port not found." + "\n" + "Dev info: " + ex.Message);
            //}

            //LoadData();
        }
        void DataArea()
        {
            sql = "  ";
            sql = sql + " select code CODE, name PROVINSI from klinik.KEMENDAGRI_AREA where lvl = 1 order by 2 "; 

            dt_provinsi = ConnOra.Data_Table_ora(sql);
            lookupna(lkProvinsi, "PROVINSI", dt_provinsi); 
        }
        void DataKota()
        {
            sql = "  ";
            sql = sql + " select  REPLACE(REPLACE(name,'Kab. ',''),'Kota ','') KOTA from klinik.KEMENDAGRI_AREA where lvl = 2 " + "\r\n"; 
            sql = sql + "  order by 1 " + "\r\n";

            dt_kota = ConnOra.Data_Table_ora(sql); 
        }
        void DataKab()
        {
            sql = "  ";
            sql = sql + " select code CODE, name KABUPATEN from klinik.KEMENDAGRI_AREA where lvl = 2 " + "\r\n";
            sql = sql + "    and PARENT_CODE = '" + Convert.ToString(lkProvinsi.GetColumnValue("CODE"))  + "' " + "\r\n";
            sql = sql + "  order by 2 " + "\r\n";

            dt_kbputen = ConnOra.Data_Table_ora(sql);
            lookupna(lkkota, "KABUPATEN", dt_kbputen);
        }
        void DataKec()
        {
            sql = "  ";
            sql = sql + " select code CODE, name KECAMATAN from klinik.KEMENDAGRI_AREA where lvl = 3 " + "\r\n";
            sql = sql + "    and PARENT_CODE = '" + Convert.ToString(lkkota.GetColumnValue("CODE")) + "' " + "\r\n";
            sql = sql + "  order by 2 " + "\r\n";

            dt_kecamatan = ConnOra.Data_Table_ora(sql);
            lookupna(lkkecamatan, "KECAMATAN", dt_kecamatan);
        }
        void DataKel()
        {
            sql = "  ";
            sql = sql + " select code CODE, name KELURAHAN from klinik.KEMENDAGRI_AREA where lvl = 4 " + "\r\n";
            sql = sql + "    and PARENT_CODE = '" + Convert.ToString(lkkecamatan.GetColumnValue("CODE")) + "' " + "\r\n";
            sql = sql + "  order by 2 " + "\r\n";

            dt_kelurahan = ConnOra.Data_Table_ora(sql);
            lookupna(lkkelurahan, "KELURAHAN", dt_kelurahan);
        }
        void lookupna(LookUpEdit lookna, string ngaran, DataTable datatablena)
        {
            lookna.Properties.DataSource = null;
            lookna.Properties.DataSource = datatablena;
            lookna.Properties.DisplayMember = ngaran;
            lookna.Properties.ValueMember = ngaran;
            lookna.Properties.PopulateColumns();
            lookna.Properties.Columns[0].Visible = false;
        } 
        private void lkProvinsi_EditValueChanged(object sender, EventArgs e)
        {
            DataKab(); DataKec(); DataKel();
        }
        private void lkkota_EditValueChanged(object sender, EventArgs e)
        {
            DataKec(); DataKel();
        } 
        private void lkkecamatan_EditValueChanged(object sender, EventArgs e)
        {
            DataKel();
        } 
        private void LoadData()
        {
            string sql_search; 

            sql_search = " ";

            sql_search = sql_search + Environment.NewLine + "select a.patient_no, nid, name, birth_place, to_date(to_char(birth_date,'yyyy-MM-dd'),'yyyy-MM-dd') birth_date,  ";
            sql_search = sql_search + Environment.NewLine + "gender, address, city,insu_class, insu_no, a.status, 'U' action, job, family_head, phone,  ";
            sql_search = sql_search + Environment.NewLine + " insu_no2, insu_nm2, rfid_no, company, company_addr, b.RM_NO ";
            sql_search = sql_search + Environment.NewLine + "  from cs_patient_info a, cs_patient b ";
            sql_search = sql_search + Environment.NewLine + " where 1=1  and a.patient_no = b.patient_no ";
            if (cmbStatus.Text == "Aktif")
            {
                sql_search = sql_search + Environment.NewLine + "and a.status = 'A' ";
            }
            else
            {
                sql_search = sql_search + Environment.NewLine + "and a.status = 'I' ";
            }

            if (cmbSearch.Text == "Nama")
            {
                sql_search = sql_search + Environment.NewLine + "and upper(a.name) like upper('%" + tNik.Text + "%') ";
            }
            else if (cmbSearch.Text == "No.KTP")
            {
                sql_search = sql_search + Environment.NewLine + "and a.nid like '%" + tNik.Text + "%' ";
            }
            else if (cmbSearch.Text == "No.HP")
            {
                sql_search = sql_search + Environment.NewLine + "and a.phone like '%" + tNik.Text + "%' ";
            }
            else
            {
                sql_search = sql_search + Environment.NewLine + "and a.insu_no like '%" + tNik.Text + "%' ";
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

                gridView1.Columns[6].Visible = false; gridView1.Columns[7].Visible = false; gridView1.Columns[13].Visible = false;
                gridView1.Columns[14].Visible = false; gridView1.Columns[17].Visible = false;

                //gridView1.Columns[4].ColumnEdit = redate;
                gridView1.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                gridView1.Columns[4].DisplayFormat.FormatString = "yyyy-MM-dd";

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

                RepositoryItemLookUpEdit PTLookup = new RepositoryItemLookUpEdit();
                PTLookup.DataSource = listPT;
                PTLookup.ValueMember = "statCode";
                PTLookup.DisplayMember = "statName";

                PTLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                PTLookup.DropDownRows = listPT.Count + 1;
                PTLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                PTLookup.AutoSearchColumnIndex = 1;
                PTLookup.NullText = "";
                gridView1.Columns[18].ColumnEdit = PTLookup;

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

                RepositoryItemLookUpEdit kotaLookup = new RepositoryItemLookUpEdit(); 
                kotaLookup.DataSource = dt_kota;
                kotaLookup.ValueMember = "KOTA";
                kotaLookup.DisplayMember = "KOTA";

                kotaLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                kotaLookup.DropDownRows = dt_kota.Rows.Count + 1;
                kotaLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                kotaLookup.AutoSearchColumnIndex = 1;
                kotaLookup.NullText = "";
                gridView1.Columns[3].ColumnEdit = kotaLookup;

                gridView1.BestFitColumns();

                gridView1.Columns[11].Visible = false;
                gridView1.Columns[20].Visible = false;
                gridView1.Columns[1].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[2].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[6].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gridView1.Columns[8].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;

                simpleButton2.Enabled = true;
                gridView1.Columns[0].OptionsColumn.AllowEdit = false;
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

            string sql_room = " select ID_PT, NAMA_PT  from KLINIK.CS_ASURANSI_PT where status = 'A' ";
            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_room, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);

            listPT.Clear();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                listPT.Add(new Stat() { statCode = dt2.Rows[i]["ID_PT"].ToString(), statName = dt2.Rows[i]["NAMA_PT"].ToString()  });
            }

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
            textBox4.Text = ""; textBox5.Text = ""; textBox9.Text = ""; textBox10.Text = ""; textBox14.Text = "";
            textBox13.Text = ""; textBox11.Text = ""; textScanOut.Text = "";
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
            string tmp_pas_no = "", pas_no = "", ktp = "", nama = "", tmt_lahir = "", sql_updat ="";
            string tgl_lahir = "", jk = "", alamat = "", kota = "", bpjs = "", stat = "", job = "", kk = "", nohp = "", kls = "", noinsu2="", nminsu2="", rfid="", comp="", comp_addr="";
            string sql_check = "", sql_cnt = "", sql_insert="", sql_update = "", action="", cek_p = "";
            int queue = 0, visit=0;
            string pasien_no = "" , tglahir = "";
            DateTime parsedDate;
            DateTime result; 

            if (gridView1.FocusedRowHandle < 0)
                return;

            pasien_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();

            if (pasien_no.ToString().Equals(""))
            {
                pas_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                ktp = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                nama = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                tmt_lahir = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[3]).ToString();
                tgl_lahir = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
                jk = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();
                alamat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();
                kota = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[7]).ToString();
                kls = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
                bpjs = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[9]).ToString();
                stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
                action = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();
                job = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[12]).ToString();
                kk = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();
                nohp = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
                noinsu2 = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();
                nminsu2 = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[16]).ToString();
                rfid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[17]).ToString();
                comp = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[18]).ToString();
                comp_addr = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[19]).ToString();
                

                if (nama == "")
                {
                    MessageBox.Show("Nama Pasien harus di input.");
                    return;
                }
                else if (jk == "")
                {
                    MessageBox.Show("Jenis Kelamin harus di input."); return;
                }
                else if (textBox3.Text == "")
                {
                    MessageBox.Show("Alamat harus di input."); return;
                }
                //else if (textBox4.Text == "")
                //{
                //    MessageBox.Show("Nama Orang Tua harus di input.");
                //}
                //else if (textBox10.Text == "")
                //{
                //    MessageBox.Show("Data Keluarga Dekat harus di input.");
                //}
                else if (tgl_lahir.Length < 5)
                {
                    MessageBox.Show("Tanggal Lahir Pasien harus di input."); return;
                }
                //else if (!DateTime.TryParse(tgl_lahir, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                //{
                //    //    //    string tgllahir =  parsedDate.ToString("yyyy-MM-dd");
                //    //    //    //!DateTime.TryParseExact(
                //    //    //    //     tglahir.ToString(),
                //    //    //    //     "yyyy-MM-dd",
                //    //    //    //     CultureInfo.InvariantCulture,
                //    //    //    //     DateTimeStyles.AssumeUniversal,
                //    //    //    //     out result)
                //    //    //}
                //    //    //else
                //    //    //{
                //    //    MessageBox.Show("Format tanggal harus yyyy-MM-dd. Silahkan ubah format tanggal komputer anda.");
                //    //}
                //} 
                else
                {
                    parsedDate = DateTime.Parse(tgl_lahir);
                    tglahir = parsedDate.ToString("yyyy-MM-dd");

                    //string tglahir = "";
                    //if (DateTime.TryParse(tgl_lahir, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                    //      tglahir = parsedDate.ToString("yyyy-MM-dd");

                    sql_check = " select count(0) cnt from cs_patient_info where insu_no is not null and insu_no = '" + bpjs + "'  ";
                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_check, oraConnect2);
                    DataTable dt2 = new DataTable();
                    adOra2.Fill(dt2);
                    cek_p = dt2.Rows[0]["cnt"].ToString();

                    //sql_cnt = " select 'P' || to_char(sysdate,'yymm') || lpad(count(0)+1,3,'0') pno from cs_patient_info where to_char(ins_date, 'yyyymm') = to_char(sysdate, 'yyyymm')  ";

                    if (Convert.ToInt32(cek_p) > 0)
                    {
                        MessageBox.Show("No BPJS " + bpjs + " sudah terdaftar.");
                    }
                    else
                    {
                        sql_cnt = " select 'P' || to_char(sysdate,'yymm') || LPAD(CS_PATIENT_SEQ.NEXTVAL, 4, '0') pno from dual ";
                        OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_cnt, oraConnect4);
                        DataTable dt4 = new DataTable();
                        adOra4.Fill(dt4);
                        tmp_pas_no = dt4.Rows[0]["pno"].ToString();


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
                                                  " city, insu_no, status, job,  " +
                                                  " phone, insu_class, insu_no2, insu_nm2, rfid_no, company, company_addr, ins_date, ins_emp, " +
                                                  " BBAHASA, AGAMA, PENDIDIKAN_TR, FAMILY_HEAD, JOB_FAMILY_HEAD,  " +
                                                  " BANGSA, STAT_KAWIN, GOL_DARAH, PROVINSI, KABUPATEN, KECAMATAN,KELURAHAN,KEL_TERDEKAT,KEL_ALAMAT,KEL_TELP,NO_IHS,NO_KK,RT,RW, NO_RUMAH ) values " +
                                                  " ( '" + tmp_pas_no + "', '" + ktp + "',initcap('" + nama.Replace("'", "''") + "'),  '" + tmt_lahir + "',to_date('" + tglahir.ToString() + "','yyyy-MM-dd'),'" + jk + "','" + textBox3.Text.Replace("'", "''") + "', " +
                                                  " '" + kota + "', '" + bpjs + "', '" + stat + "', '" + job + "',  " +
                                                  " '" + textBox7.Text + "', '" + kls + "', '" + noinsu2 + "', '" + nminsu2 + "', '" + rfid + "', '" + comp + "', '" + comp_addr + "', sysdate, '" + DB.vUserId + "', " +
                                                " '" + FN.getVal(gbBahasa) + "', '" + FN.radioVal(radioGroup18) + "', '" + FN.getVal(groupBox5) + "', initcap('" + textBox4.Text.Replace("'", "''") + "'), '" + textBox5.Text + "',  " +
                                                " '" + FN.radioVal(radioGroup2) + "', '" + FN.radioVal(radioGroup3) + "', '" + cboGol.Text + "', '" + Convert.ToString(lkProvinsi.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkota.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkecamatan.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkelurahan.GetColumnValue("CODE")) + "',  " +
                                                " initcap('" + textBox10.Text.Replace("'", "''") + "'), '" + textBox9.Text + "', '" + textBox8.Text + "', '" + textBox13.Text + "', '" + textBox11.Text + "', '" + cboRT.Text + "', '" + cboRW.Text + "', '" + textBox6.Text + "' ) ";

                            command.ExecuteNonQuery();

                            command.CommandText = " insert into cs_patient (rm_no, patient_no, group_patient, status, ins_date, ins_emp, info11) " +
                                                  " values ( '" + tmp_pas_no.Substring(1, 8) + "' , '" + tmp_pas_no + "', 'COMM', 'A', sysdate, '" + DB.vUserId + "', '" + textBox15.Text + "') ";

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
                        //LoadData();
                    } 
                } 
            }
            else
            {
                //string tglahir = "";
                //if (DateTime.TryParse(tgl_lahir, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                //    tglahir = parsedDate.ToString("yyyy-MM-dd");

                pas_no = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[0]).ToString();
                ktp = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[1]).ToString();
                nama = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[2]).ToString();
                tmt_lahir = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[3]).ToString();
                tgl_lahir = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[4]).ToString();
                jk = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[5]).ToString();
                alamat = textBox3.Text;// gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[6]).ToString();
                kota = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[7]).ToString();
                kls = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[8]).ToString();
                bpjs = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[9]).ToString();
                stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[10]).ToString();
                action = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[11]).ToString();
                job = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[12]).ToString();
                kk = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[13]).ToString();
                nohp = textBox7.Text;// gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[14]).ToString();
                noinsu2 = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[15]).ToString();
                nminsu2 = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[16]).ToString();
                rfid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[17]).ToString();
                comp = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[18]).ToString();
                comp_addr = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns[19]).ToString();

                if (tgl_lahir.Length < 5)
                {
                    MessageBox.Show("Tanggal Lahir Pasien harus di input."); return;
                }
                parsedDate = DateTime.Parse(tgl_lahir);
                tglahir = parsedDate.ToString("yyyy-MM-dd");

                

                sql_update = " "; 
                sql_update = sql_update + Environment.NewLine + "update cs_patient_info ";
                sql_update = sql_update + Environment.NewLine + "set nid = '" + ktp + "', name = '" + nama + "', birth_place = '" + tmt_lahir + "', birth_date = to_date('" + tglahir.ToString() + "','yyyy-MM-dd'), ";
                sql_update = sql_update + Environment.NewLine + "gender = '" + jk + "', address = '" + alamat + "', city = '" + kota + "', insu_no = '" + bpjs + "', status = '" + stat + "', job = '" + job + "', family_head = '" + textBox4.Text  + "', phone = '" + nohp + "', insu_class = '" + kls + "', ";
                sql_update = sql_update + Environment.NewLine + "insu_no2 = '" + noinsu2 + "', insu_nm2 = '" + nminsu2 + "', rfid_no = '" + textScanOut.Text + "', company = '" + comp + "', company_addr = '" + comp_addr + "', ";
                sql_update = sql_update + Environment.NewLine + "BBAHASA = '" + FN.getVal(gbBahasa,5) + "', AGAMA = '" + FN.radioVal(radioGroup18) + "', PENDIDIKAN_TR = '" + FN.getVal(groupBox5)  + "', JOB_FAMILY_HEAD = '" + textBox5.Text + "', BANGSA = '" + FN.radioVal(radioGroup2) + "', ";
                sql_update = sql_update + Environment.NewLine + "STAT_KAWIN = '" + FN.radioVal(radioGroup3) + "', GOL_DARAH = '" + cboGol.Text + "', PROVINSI = '" + Convert.ToString(lkProvinsi.GetColumnValue("CODE")) + "', KABUPATEN = '" + Convert.ToString(lkkota.GetColumnValue("CODE")) + "', KECAMATAN = '" + Convert.ToString(lkkecamatan.GetColumnValue("CODE")) + "', ";
                sql_update = sql_update + Environment.NewLine + "KELURAHAN = '" + Convert.ToString(lkkelurahan.GetColumnValue("CODE")) + "', KEL_TERDEKAT = '" + textBox10.Text + "', KEL_ALAMAT = '" + textBox9.Text + "', KEL_TELP = '" + textBox8.Text + "', NO_IHS = '" + textBox13.Text + "', ";
                sql_update = sql_update + Environment.NewLine + "NO_KK = '" + textBox11.Text + "', RT = '" + cboRT.Text + "', RW = '" + cboRW.Text + "', NO_RUMAH = '" + textBox6.Text + "' , ";
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

                    sql_updat = " ";
                    sql_updat = sql_updat + Environment.NewLine + "update cs_patient ";
                    sql_updat = sql_updat + Environment.NewLine + "set info11 = '" + textBox15.Text + "' ";
                    sql_updat = sql_updat + Environment.NewLine + "where patient_no = '" + pas_no + "' ";

                    OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm2 = new OleDbCommand(sql_updat, oraConnect2);
                    oraConnect2.Open();
                    cm2.ExecuteNonQuery();
                    oraConnect2.Close();
                    cm2.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_update);


                    MessageBox.Show("Data Berhasil diupdate");
                    //LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                
            }

            
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{

            //    //pas_no = gridView1.GetRowCellValue(i, gridView1.Columns[0]).ToString();
            //    //ktp = gridView1.GetRowCellValue(i, gridView1.Columns[1]).ToString();
            //    //nama = gridView1.GetRowCellValue(i, gridView1.Columns[2]).ToString();
            //    //tmt_lahir = gridView1.GetRowCellValue(i, gridView1.Columns[3]).ToString();
            //    //tgl_lahir = gridView1.GetRowCellValue(i, gridView1.Columns[4]).ToString().Substring(0,10);
            //    //jk = gridView1.GetRowCellValue(i, gridView1.Columns[5]).ToString();
            //    //alamat = gridView1.GetRowCellValue(i, gridView1.Columns[6]).ToString();
            //    //kota = gridView1.GetRowCellValue(i, gridView1.Columns[7]).ToString();
            //    //kls = gridView1.GetRowCellValue(i, gridView1.Columns[8]).ToString();
            //    //bpjs = gridView1.GetRowCellValue(i, gridView1.Columns[9]).ToString();
            //    //stat = gridView1.GetRowCellValue(i, gridView1.Columns[10]).ToString();
            //    //action = gridView1.GetRowCellValue(i, gridView1.Columns[11]).ToString();
            //    //job= gridView1.GetRowCellValue(i, gridView1.Columns[12]).ToString();
            //    //kk = gridView1.GetRowCellValue(i, gridView1.Columns[13]).ToString();
            //    //nohp = gridView1.GetRowCellValue(i, gridView1.Columns[14]).ToString(); 
            //    //noinsu2 = gridView1.GetRowCellValue(i, gridView1.Columns[15]).ToString();
            //    //nminsu2 = gridView1.GetRowCellValue(i, gridView1.Columns[16]).ToString();
            //    //rfid = gridView1.GetRowCellValue(i, gridView1.Columns[17]).ToString();
            //    //comp = gridView1.GetRowCellValue(i, gridView1.Columns[18]).ToString();
            //    //comp_addr = gridView1.GetRowCellValue(i, gridView1.Columns[19]).ToString();

            //    //if (action == "I")
            //    //{
            //    //    if (!DateTime.TryParseExact(
            //    //             tgl_lahir,
            //    //             "yyyy-MM-dd",
            //    //             CultureInfo.InvariantCulture,
            //    //             DateTimeStyles.AssumeUniversal,
            //    //             out result))
            //    //    {
            //    //        MessageBox.Show("Format tanggal harus yyyy-MM-dd. Silahkan ubah format tanggal komputer anda.");
            //    //    }
            //    //    else if (nama == "")
            //    //    {
            //    //        MessageBox.Show("Nama Pasien harus diisi.");
            //    //    }
            //    //    else if (jk == "")
            //    //    {
            //    //        MessageBox.Show("Jenis Kelamin harus diisi.");
            //    //    }
            //    //    else if (textBox3.Text == "")
            //    //    {
            //    //        MessageBox.Show("Alamat harus diisi.");
            //    //    }
            //    //    else if (textBox4.Text  == "")
            //    //    {
            //    //        MessageBox.Show("Nama Orang Tua harus diisi.");
            //    //    }
            //    //    else if (textBox10.Text == "")
            //    //    {
            //    //        MessageBox.Show("Data Keluarga Dekat harus diisi.");
            //    //    }
            //    //    //else if (nohp == "")
            //    //    //{
            //    //    //    MessageBox.Show("No HP harus diisi.");
            //    //    //}
            //    //    else
            //    //    {
            //    //        sql_check = " select count(0) cnt from cs_patient_info where insu_no is not null and insu_no = '" + bpjs + "'  ";
            //    //        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            //    //        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_check, oraConnect2);
            //    //        DataTable dt2 = new DataTable();
            //    //        adOra2.Fill(dt2);
            //    //        cek_p = dt2.Rows[0]["cnt"].ToString();

            //    //        sql_cnt = " select 'P' || to_char(sysdate,'yymm') || lpad(count(0)+1,3,'0') pno from cs_patient_info where to_char(ins_date, 'yyyymm') = to_char(sysdate, 'yyyymm')  ";
            //    //        OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
            //    //        OleDbDataAdapter adOra4 = new OleDbDataAdapter(sql_cnt, oraConnect4);
            //    //        DataTable dt4 = new DataTable();
            //    //        adOra4.Fill(dt4);
            //    //        tmp_pas_no = dt4.Rows[0]["pno"].ToString();
            //    //        if (Convert.ToInt32(cek_p) > 0)
            //    //        {
            //    //            MessageBox.Show("No BPJS " + bpjs + " sudah terdaftar.");
            //    //        }
            //    //        else
            //    //        {
            //    //            OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
            //    //            OleDbCommand command = new OleDbCommand();
            //    //            OleDbTransaction trans = null;

            //    //            command.Connection = oraConnectTrans;
            //    //            oraConnectTrans.Open();


            //    //            try
            //    //            {

            //    //                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
            //    //                command.Connection = oraConnectTrans;
            //    //                command.Transaction = trans;


            //    //                command.CommandText = " insert into cs_patient_info (patient_no, nid, name, birth_place, birth_date, gender, address, " +
            //    //                                      " city, insu_no, status, job,  " +
            //    //                                      " phone, insu_class, insu_no2, insu_nm2, rfid_no, company, company_addr, ins_date, ins_emp, " +
            //    //                                      " BBAHASA, AGAMA, PENDIDIKAN_TR, FAMILY_HEAD, JOB_FAMILY_HEAD,  " +
            //    //                                      " BANGSA, STAT_KAWIN, GOL_DARAH, PROVINSI, KABUPATEN, KECAMATAN,KELURAHAN,KEL_TERDEKAT,KEL_ALAMAT,KEL_TELP,NO_IHS,NO_KK,RT,RW, NO_RUMAH ) values " +
            //    //                                      " ( '" + tmp_pas_no + "', '" + ktp + "','" + nama + "',  '" + tmt_lahir + "',to_date('" + tglahir.ToString() + "','yyyy-MM-dd'),'" + jk + "','" + textBox3.Text + "', " +
            //    //                                      " '" + kota + "', '" + bpjs + "', '" + stat + "', '" + job + "',  " +
            //    //                                      " '" + textBox7.Text + "', '" + kls + "', '" + noinsu2 + "', '" + nminsu2 + "', '" + rfid + "', '" + comp + "', '" + comp_addr + "', sysdate, '" + DB.vUserId + "', " +
            //    //                                    " '" + FN.chkListOf(chkSkalaNyeri) + "', '" + FN.radioVal(radioGroup18) + "', '" + FN.radioVal(radioGroup1) + "', '" + textBox4.Text + "', '" + textBox5.Text + "',  " +
            //    //                                    " '" + FN.radioVal(radioGroup2) + "', '" + FN.radioVal(radioGroup3) + "', '" + cboGol.Text + "', '" + Convert.ToString(lkProvinsi.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkota.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkecamatan.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkelurahan.GetColumnValue("CODE")) + "',  " +
            //    //                                    " '" + textBox10.Text  + "', '" + textBox9.Text + "', '" + textBox8.Text + "', '" + textBox13.Text + "', '" + textBox11.Text + "', '" + cboRT.Text + "', '" + cboRW.Text + "', '" + textBox6.Text + "' ) ";

            //    //                command.ExecuteNonQuery();

            //    //                command.CommandText = " insert into cs_patient (rm_no, patient_no, group_patient, status, ins_date, ins_emp) " +
            //    //                                      " values ('C' || to_char(sysdate,'yymmdd') || replace('" + tmp_pas_no + "','P'), '" + tmp_pas_no + "', 'COMM', 'A', sysdate, '" + DB.vUserId + "') ";

            //    //                command.ExecuteNonQuery(); 
            //    //                trans.Commit(); 
            //    //                //MessageBox.Show(sql_insert);
            //    //                //MessageBox.Show("Query Exec : " + sql);
            //    //                MessageBox.Show("Data Berhasil disimpan.");
            //    //            }
            //    //            catch (Exception ex)
            //    //            {
            //    //                trans.Rollback();
            //    //                MessageBox.Show("ERROR: " + ex.Message);
            //    //            }
            //    //            oraConnectTrans.Close();
            //    //        }
            //    //    }
            //    //}
            //    //else if (action == "U")
            //    //{
            //    //    sql_update = "";

            //    //    sql_update = sql_update + Environment.NewLine + "update cs_patient_info ";
            //    //    sql_update = sql_update + Environment.NewLine + "set nid = '" + ktp + "', name = '" + nama + "', birth_place = '" + tmt_lahir + "', birth_date = to_date('" + tglahir.ToString() + "','yyyy-MM-dd'), ";
            //    //    sql_update = sql_update + Environment.NewLine + "gender = '" + jk + "', address = '" + alamat + "', city = '" + kota + "', insu_no = '" + bpjs + "', status = '" + stat + "', job = '" + job + "', family_head = '" + kk + "', phone = '" + nohp + "', insu_class = '" + kls + "', ";
            //    //    sql_update = sql_update + Environment.NewLine + "insu_no2 = '" + noinsu2 + "', insu_nm2 = '" + nminsu2 + "', rfid_no = '" + rfid + "', company = '" + comp + "', company_addr = '" + comp_addr + "', ";
            //    //    sql_update = sql_update + Environment.NewLine + "BBAHASA = '" + FN.chkListOf(chkSkalaNyeri) + "', AGAMA = '" + FN.radioVal(radioGroup18) + "', PENDIDIKAN_TR = '" + FN.radioVal(radioGroup1) + "', JOB_FAMILY_HEAD = '" + textBox5.Text + "', BANGSA = '" + FN.radioVal(radioGroup2) + "', ";
            //    //    sql_update = sql_update + Environment.NewLine + "STAT_KAWIN = '" + FN.radioVal(radioGroup3) + "', GOL_DARAH = '" + cboGol.Text + "', PROVINSI = '" + Convert.ToString(lkProvinsi.GetColumnValue("CODE")) + "', KABUPATEN = '" + Convert.ToString(lkkota.GetColumnValue("CODE")) + "', KECAMATAN = '" + Convert.ToString(lkkecamatan.GetColumnValue("CODE")) + "', ";
            //    //    sql_update = sql_update + Environment.NewLine + "KELURAHAN = '" + Convert.ToString(lkkelurahan.GetColumnValue("CODE")) + "', KEL_TERDEKAT = '" + textBox10.Text + "', KEL_ALAMAT = '" + textBox9.Text + "', KEL_TELP = '" + textBox8.Text + "', NO_IHS = '" + textBox13.Text + "', ";
            //    //    sql_update = sql_update + Environment.NewLine + "NO_KK = '" + textBox11.Text + "', RT = '" + cboRT.Text  + "', RW = '" + cboRW.Text + "', NO_RUMAH = '" + textBox6.Text + "' , ";
            //    //    sql_update = sql_update + Environment.NewLine + "upd_date = sysdate, upd_emp = '" + DB.vUserId + "' ";
            //    //    sql_update = sql_update + Environment.NewLine + "where patient_no = '" + pas_no + "' ";

            //    //    try
            //    //    {
            //    //        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            //    //        OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
            //    //        oraConnect.Open();
            //    //        cm.ExecuteNonQuery();
            //    //        oraConnect.Close();
            //    //        cm.Dispose();

            //    //        //MessageBox.Show("Query Exec : " + sql_update);


            //    //        MessageBox.Show("Data Berhasil diupdate");
            //    //    }
            //    //    catch (Exception ex)
            //    //    {
            //    //        MessageBox.Show("ERROR: " + ex.Message);
            //    //    }
            //    //}
            //}
            //richTextBox1.Text = cek;
            //MessageBox.Show(action);
        }

        private void checkLookUp(string dt, LookUpEdit le)
        {
            string[] val = dt.Split(new string[] { "::" }, StringSplitOptions.None);
            if (val.Length == 1)
            { 
                if (val[0] != "")
                {
                    le.EditValue = val[0];
                }
            }
        }
        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            string sql_search;
            GridView View = sender as GridView;
            string s_pinfo = "";

            if (View.RowCount < 2)
                return;

            if (View.FocusedColumn.Caption == "Pasien No")
            {
                View.OptionsBehavior.Editable = true;
                s_pinfo = View.GetRowCellDisplayText(View.FocusedRowHandle, View.Columns[0]);

                string qdata = "SELECT DISTINCT B.*, a.RM_NO, a.info11 old_rm FROM CS_PATIENT A, CS_PATIENT_INFO B WHERE A.PATIENT_NO = B.PATIENT_NO AND A.PATIENT_NO = '" + s_pinfo + "'";
                dt_pasien = ConnOra.Data_Table_ora(qdata);

                if (dt_pasien.Rows.Count > 0)
                {
                    cboGol.Text = FN.rowVal(dt_pasien, "GOL_DARAH");
                    FN.splitVal(FN.rowVal(dt_pasien, "STAT_KAWIN"), radioGroup3);
                    FN.splitVal(FN.rowVal(dt_pasien, "AGAMA"), radioGroup18);
                    textBox7.Text = FN.rowVal(dt_pasien, "PHONE");
                    lkProvinsi.EditValue = FN.GetDataLook(lkProvinsi, "CODE", dt_pasien.Rows[0]["PROVINSI"].ToString());
                    lkkota.EditValue = FN.GetDataLook(lkkota, "CODE", dt_pasien.Rows[0]["KABUPATEN"].ToString());
                    lkkecamatan.EditValue = FN.GetDataLook(lkkecamatan, "CODE", dt_pasien.Rows[0]["KECAMATAN"].ToString());
                    lkkelurahan.EditValue = FN.GetDataLook(lkkelurahan, "CODE", dt_pasien.Rows[0]["KELURAHAN"].ToString());
                    cboRT.Text = FN.rowVal(dt_pasien, "RT");
                    cboRW.Text = FN.rowVal(dt_pasien, "RW");
                    textBox6.Text = FN.rowVal(dt_pasien, "NO_RUMAH");
                    textBox3.Text = FN.rowVal(dt_pasien, "ADDRESS");
                    FN.splitVal2(FN.rowVal(dt_pasien, "BBAHASA"), gbBahasa, txStsPsikologi);
                    FN.splitVal1(FN.rowVal(dt_pasien, "PENDIDIKAN_TR"), radioGroup1, textBox2);
                    FN.splitVal1(FN.rowVal(dt_pasien, "PENDIDIKAN_TR"), radioGroup1, textBox2);
                    //FN.setCheckList(FN.rowVal(dt_pasien, "BBAHASA"), gbBahasa);
                    FN.splitVal(FN.rowVal(dt_pasien, "BANGSA"), radioGroup2);
                    //FN.splitVal2(FN.rowVal(dt_pasien, "PENDIDIKAN_TR"), radioGroup1, textBox2);
                    textBox4.Text = FN.rowVal(dt_pasien, "FAMILY_HEAD");
                    textBox5.Text = FN.rowVal(dt_pasien, "JOB_FAMILY_HEAD");
                    textBox10.Text = FN.rowVal(dt_pasien, "KEL_TERDEKAT");
                    textBox9.Text = FN.rowVal(dt_pasien, "KEL_ALAMAT");
                    textBox8.Text = FN.rowVal(dt_pasien, "KEL_TELP");
                    textBox13.Text = FN.rowVal(dt_pasien, "NO_IHS");
                    textBox11.Text = FN.rowVal(dt_pasien, "NO_KK");
                    textBox14.Text = FN.rowVal(dt_pasien, "RM_NO");
                    textBox15.Text = FN.rowVal(dt_pasien, "OLD_RM");
                    textScanOut.Text = FN.rowVal(dt_pasien, "RFID_NO");

                    textBox1.Text =  FN.rowVal(dt_pasien, "PATIENT_NO");
                    textBox12.Text =  FN.rowVal(dt_pasien, "NAME");
                    simpleButton5.Enabled = true;

                }
                else
                {
                    cboGol.Text = ""; 
                    textBox7.Text = "";
                    lkProvinsi.EditValue = "";
                    lkkota.EditValue = "";
                    lkkecamatan.EditValue = "";
                    lkkelurahan.EditValue = "";
                    cboRT.Text = "";
                    cboRW.Text = "";
                    textBox6.Text = "";
                    textBox3.Text = "";  
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox10.Text = "";
                    textBox9.Text = "";
                    textBox8.Text = "";
                    textBox13.Text = "";
                    textBox11.Text = "";
                    textBox14.Text = "";
                    textBox15.Text = "";
                    textScanOut.Text = "";
                    textBox1.Text = "";
                    textBox12.Text = "";
                }

            }

            //string sql_search;
            //GridView View = sender as GridView;
            //string s_pinfo = "";

            //s_pinfo = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);

            //string qdata = "SELECT DISTINCT B.*, a.RM_NO FROM CS_PATIENT A, CS_PATIENT_INFO B WHERE A.PATIENT_NO = B.PATIENT_NO AND A.PATIENT_NO = '" + s_pinfo + "'";
            //dt_pasien = ConnOra.Data_Table_ora(qdata);

            ////    //                command.CommandText = " insert into cs_patient_info (patient_no, nid, name, birth_place, birth_date, gender, address, " +
            ////    //                                      " city, insu_no, status, job,  " +
            ////    //                                      " phone, insu_class, insu_no2, insu_nm2, rfid_no, company, company_addr, ins_date, ins_emp, " +
            ////    //                                      " BBAHASA, AGAMA, PENDIDIKAN_TR, FAMILY_HEAD, JOB_FAMILY_HEAD,  " +
            ////    //                                      " BANGSA, STAT_KAWIN, GOL_DARAH, PROVINSI, KABUPATEN, KECAMATAN,KELURAHAN,KEL_TERDEKAT,KEL_ALAMAT,KEL_TELP,NO_IHS,NO_KK,RT,RW, NO_RUMAH ) values " +
            ////    //                                      " ( '" + tmp_pas_no + "', '" + ktp + "','" + nama + "',  '" + tmt_lahir + "',to_date('" + tglahir.ToString() + "','yyyy-MM-dd'),'" + jk + "','" + textBox3.Text + "', " +
            ////    //                                      " '" + kota + "', '" + bpjs + "', '" + stat + "', '" + job + "',  " +
            ////    //                                      " '" + textBox7.Text + "', '" + kls + "', '" + noinsu2 + "', '" + nminsu2 + "', '" + rfid + "', '" + comp + "', '" + comp_addr + "', sysdate, '" + DB.vUserId + "', " +
            ////    //                                    " '" + FN.chkListOf(chkSkalaNyeri) + "', '" + FN.radioVal(radioGroup18) + "', '" + FN.radioVal(radioGroup1) + "', '" + textBox4.Text + "', '" + textBox5.Text + "',  " +
            ////    //                                    " '" + FN.radioVal(radioGroup2) + "', '" + FN.radioVal(radioGroup3) + "', '" + cboGol.Text + "', '" + Convert.ToString(lkProvinsi.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkota.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkecamatan.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkelurahan.GetColumnValue("CODE")) + "',  " +
            ////    //                                    " '" + textBox10.Text  + "', '" + textBox9.Text + "', '" + textBox8.Text + "', '" + textBox13.Text + "', '" + textBox11.Text + "', '" + cboRT.Text + "', '" + cboRW.Text + "', '" + textBox6.Text + "' ) ";


            //if (dt_pasien.Rows.Count > 0)
            //{
            //    cboGol.Text = FN.rowVal(dt_pasien, "GOL_DARAH");
            //    FN.splitVal(FN.rowVal(dt_pasien, "STAT_KAWIN"), radioGroup3);
            //    FN.splitVal(FN.rowVal(dt_pasien, "AGAMA"), radioGroup18);
            //    textBox7.Text = FN.rowVal(dt_pasien, "PHONE");
            //    lkProvinsi.EditValue = FN.GetDataLook(lkProvinsi, "CODE", dt_pasien.Rows[0]["PROVINSI"].ToString());
            //    lkkota.EditValue = FN.GetDataLook(lkkota, "CODE", dt_pasien.Rows[0]["KABUPATEN"].ToString());
            //    lkkecamatan.EditValue = FN.GetDataLook(lkkecamatan, "CODE", dt_pasien.Rows[0]["KECAMATAN"].ToString());
            //    lkkelurahan.EditValue = FN.GetDataLook(lkkelurahan, "CODE", dt_pasien.Rows[0]["KELURAHAN"].ToString());
            //    cboRT.Text = FN.rowVal(dt_pasien, "RT");
            //    cboRW.Text = FN.rowVal(dt_pasien, "RW");
            //    textBox6.Text = FN.rowVal(dt_pasien, "NO_RUMAH");
            //    textBox3.Text = FN.rowVal(dt_pasien, "ADDRESS");
            //    FN.splitVal2(FN.rowVal(dt_pasien, "BBAHASA"), gbBahasa, txStsPsikologi);
            //    FN.splitVal1(FN.rowVal(dt_pasien, "PENDIDIKAN_TR"), radioGroup1, textBox2);
            //    FN.splitVal1(FN.rowVal(dt_pasien, "PENDIDIKAN_TR"), radioGroup1, textBox2);
            //    //FN.setCheckList(FN.rowVal(dt_pasien, "BBAHASA"), gbBahasa);
            //    FN.splitVal(FN.rowVal(dt_pasien, "BANGSA"), radioGroup2);
            //    //FN.splitVal2(FN.rowVal(dt_pasien, "PENDIDIKAN_TR"), radioGroup1, textBox2);
            //    textBox4.Text = FN.rowVal(dt_pasien, "FAMILY_HEAD");
            //    textBox5.Text = FN.rowVal(dt_pasien, "JOB_FAMILY_HEAD");
            //    textBox10.Text = FN.rowVal(dt_pasien, "KEL_TERDEKAT");
            //    textBox9.Text = FN.rowVal(dt_pasien, "KEL_ALAMAT");
            //    textBox8.Text = FN.rowVal(dt_pasien, "KEL_TELP");
            //    textBox13.Text = FN.rowVal(dt_pasien, "NO_IHS");
            //    textBox11.Text = FN.rowVal(dt_pasien, "NO_KK");
            //    textBox14.Text = FN.rowVal(dt_pasien, "RM_NO");
            //    textScanOut.Text = FN.rowVal(dt_pasien, "RFID_NO");

            //    simpleButton5.Enabled = true;
            //    //lkProvinsi.EditValue = dt_pasien.Rows[0]["PROVINSI"].ToString();
            //    //lkkota.EditValue = dt_pasien.Rows[0]["KABUPATEN"].ToString();
            //    //lkkecamatan.EditValue = dt_pasien.Rows[0]["KECAMATAN"].ToString();
            //    //lkkelurahan.EditValue = dt_pasien.Rows[0]["KELURAHAN"].ToString();

            //    //checkLookUp(FN.rowVal(dt_pasien, "PROVINSI"), lkProvinsi);
            //    //checkLookUp(FN.rowVal(dt_pasien, "KABUPATEN"), lkkota);
            //    //checkLookUp(FN.rowVal(dt_pasien, "KECAMATAN"), lkkecamatan);
            //    //checkLookUp(FN.rowVal(dt_pasien, "KELURAHAN"), lkkelurahan);

            //    //functionSplitIndex_3(dtpasien.Rows[0]["rangsang_laktil"].ToString(), radioGroup18, txt_rangsang_laktil);
            //    //functionSplitIndex_3(dataTable2.Rows[0]["plasenta_intack"].ToString(), rb_plasenta_intack, txt_plasenta_intack);
            //    //functionSplitIndex_3(dataTable2.Rows[0]["plasenta_tidak_lahir"].ToString(), rb_plasenta_tidak_lahir, txt_plasenta_tidak_lahir);
            //    //functionSplitIndex_3(dataTable2.Rows[0]["laserasi"].ToString(), rb_laserasi, txt_laserasi);

            //    //functionSplitIndex_5(dataTable2.Rows[0]["laserasi_parinium"].ToString(), rb_la

            //}
            ////sql_search = " ";

            ////sql_search = sql_search + Environment.NewLine + "select patient_no, nid, name, birth_place, to_date(to_char(birth_date,'yyyy-MM-dd'),'yyyy-MM-dd') birth_date,  ";
            ////sql_search = sql_search + Environment.NewLine + "gender, address, city,insu_class, insu_no, status, 'S' action, job, family_head, phone,  ";
            ////sql_search = sql_search + Environment.NewLine + "insu_no2, insu_nm2, rfid_no, company, company_addr ";
            ////sql_search = sql_search + Environment.NewLine + "from cs_patient_info ";
            ////sql_search = sql_search + Environment.NewLine + "where 1=1 "; 
            ////sql_search = sql_search + Environment.NewLine + "and nid like '%" + tNik.Text + "%' "; 
            ////sql_search = sql_search + Environment.NewLine + "order by name ";
        }
        private void functionSplitIndex_3(string data_asli, DevExpress.XtraEditors.RadioGroup rbt, DevExpress.XtraEditors.TextEdit txt)
        {
            string[] aa = data_asli.Split(new string[] { "::" }, StringSplitOptions.None);
            if (aa.Length >= 3)
            {
                rbt.SelectedIndex = Convert.ToInt32(aa[0]);
                txt.EditValue = (aa[2] == null) ? "" : aa[2];
            }

        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            //string sql_search;
            //GridView View = sender as GridView;
            //string s_pinfo = "";

            //if (View.FocusedColumn.Caption == "Action" || View.FocusedColumn.Caption == "Pasien No")
            //{
            //    View.OptionsBehavior.Editable = true;
            //    s_pinfo = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);

            //    string qdata = "SELECT DISTINCT B.*, a.RM_NO FROM CS_PATIENT A, CS_PATIENT_INFO B WHERE A.PATIENT_NO = B.PATIENT_NO AND A.PATIENT_NO = '" + s_pinfo + "'";
            //    dt_pasien = ConnOra.Data_Table_ora(qdata);

            //    //    //                command.CommandText = " insert into cs_patient_info (patient_no, nid, name, birth_place, birth_date, gender, address, " +
            //    //    //                                      " city, insu_no, status, job,  " +
            //    //    //                                      " phone, insu_class, insu_no2, insu_nm2, rfid_no, company, company_addr, ins_date, ins_emp, " +
            //    //    //                                      " BBAHASA, AGAMA, PENDIDIKAN_TR, FAMILY_HEAD, JOB_FAMILY_HEAD,  " +
            //    //    //                                      " BANGSA, STAT_KAWIN, GOL_DARAH, PROVINSI, KABUPATEN, KECAMATAN,KELURAHAN,KEL_TERDEKAT,KEL_ALAMAT,KEL_TELP,NO_IHS,NO_KK,RT,RW, NO_RUMAH ) values " +
            //    //    //                                      " ( '" + tmp_pas_no + "', '" + ktp + "','" + nama + "',  '" + tmt_lahir + "',to_date('" + tglahir.ToString() + "','yyyy-MM-dd'),'" + jk + "','" + textBox3.Text + "', " +
            //    //    //                                      " '" + kota + "', '" + bpjs + "', '" + stat + "', '" + job + "',  " +
            //    //    //                                      " '" + textBox7.Text + "', '" + kls + "', '" + noinsu2 + "', '" + nminsu2 + "', '" + rfid + "', '" + comp + "', '" + comp_addr + "', sysdate, '" + DB.vUserId + "', " +
            //    //    //                                    " '" + FN.chkListOf(chkSkalaNyeri) + "', '" + FN.radioVal(radioGroup18) + "', '" + FN.radioVal(radioGroup1) + "', '" + textBox4.Text + "', '" + textBox5.Text + "',  " +
            //    //    //                                    " '" + FN.radioVal(radioGroup2) + "', '" + FN.radioVal(radioGroup3) + "', '" + cboGol.Text + "', '" + Convert.ToString(lkProvinsi.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkota.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkecamatan.GetColumnValue("CODE")) + "', '" + Convert.ToString(lkkelurahan.GetColumnValue("CODE")) + "',  " +
            //    //    //                                    " '" + textBox10.Text  + "', '" + textBox9.Text + "', '" + textBox8.Text + "', '" + textBox13.Text + "', '" + textBox11.Text + "', '" + cboRT.Text + "', '" + cboRW.Text + "', '" + textBox6.Text + "' ) ";


            //    if (dt_pasien.Rows.Count > 0)
            //    {
            //        cboGol.Text = FN.rowVal(dt_pasien, "GOL_DARAH");
            //        FN.splitVal(FN.rowVal(dt_pasien, "STAT_KAWIN"), radioGroup3);
            //        FN.splitVal(FN.rowVal(dt_pasien, "AGAMA"), radioGroup18);
            //        textBox7.Text = FN.rowVal(dt_pasien, "PHONE");
            //        lkProvinsi.EditValue = FN.GetDataLook(lkProvinsi, "CODE", dt_pasien.Rows[0]["PROVINSI"].ToString());
            //        lkkota.EditValue = FN.GetDataLook(lkkota, "CODE", dt_pasien.Rows[0]["KABUPATEN"].ToString());
            //        lkkecamatan.EditValue = FN.GetDataLook(lkkecamatan, "CODE", dt_pasien.Rows[0]["KECAMATAN"].ToString());
            //        lkkelurahan.EditValue = FN.GetDataLook(lkkelurahan, "CODE", dt_pasien.Rows[0]["KELURAHAN"].ToString());
            //        cboRT.Text = FN.rowVal(dt_pasien, "RT");
            //        cboRW.Text = FN.rowVal(dt_pasien, "RW");
            //        textBox6.Text = FN.rowVal(dt_pasien, "NO_RUMAH");
            //        textBox3.Text = FN.rowVal(dt_pasien, "ADDRESS");
            //        FN.splitVal2(FN.rowVal(dt_pasien, "BBAHASA"), gbBahasa, txStsPsikologi);
            //        FN.splitVal1(FN.rowVal(dt_pasien, "PENDIDIKAN_TR"), radioGroup1, textBox2);
            //        FN.splitVal1(FN.rowVal(dt_pasien, "PENDIDIKAN_TR"), radioGroup1, textBox2);
            //        //FN.setCheckList(FN.rowVal(dt_pasien, "BBAHASA"), gbBahasa);
            //        FN.splitVal(FN.rowVal(dt_pasien, "BANGSA"), radioGroup2);
            //        //FN.splitVal2(FN.rowVal(dt_pasien, "PENDIDIKAN_TR"), radioGroup1, textBox2);
            //        textBox4.Text = FN.rowVal(dt_pasien, "FAMILY_HEAD");
            //        textBox5.Text = FN.rowVal(dt_pasien, "JOB_FAMILY_HEAD");
            //        textBox10.Text = FN.rowVal(dt_pasien, "KEL_TERDEKAT");
            //        textBox9.Text = FN.rowVal(dt_pasien, "KEL_ALAMAT");
            //        textBox8.Text = FN.rowVal(dt_pasien, "KEL_TELP");
            //        textBox13.Text = FN.rowVal(dt_pasien, "NO_IHS");
            //        textBox11.Text = FN.rowVal(dt_pasien, "NO_KK");
            //        textBox14.Text = FN.rowVal(dt_pasien, "RM_NO");
            //        textScanOut.Text = FN.rowVal(dt_pasien, "RFID_NO");

            //        simpleButton5.Enabled = true;
            //        //lkProvinsi.EditValue = dt_pasien.Rows[0]["PROVINSI"].ToString();
            //        //lkkota.EditValue = dt_pasien.Rows[0]["KABUPATEN"].ToString();
            //        //lkkecamatan.EditValue = dt_pasien.Rows[0]["KECAMATAN"].ToString();
            //        //lkkelurahan.EditValue = dt_pasien.Rows[0]["KELURAHAN"].ToString();

            //        //checkLookUp(FN.rowVal(dt_pasien, "PROVINSI"), lkProvinsi);
            //        //checkLookUp(FN.rowVal(dt_pasien, "KABUPATEN"), lkkota);
            //        //checkLookUp(FN.rowVal(dt_pasien, "KECAMATAN"), lkkecamatan);
            //        //checkLookUp(FN.rowVal(dt_pasien, "KELURAHAN"), lkkelurahan);

            //        //functionSplitIndex_3(dtpasien.Rows[0]["rangsang_laktil"].ToString(), radioGroup18, txt_rangsang_laktil);
            //        //functionSplitIndex_3(dataTable2.Rows[0]["plasenta_intack"].ToString(), rb_plasenta_intack, txt_plasenta_intack);
            //        //functionSplitIndex_3(dataTable2.Rows[0]["plasenta_tidak_lahir"].ToString(), rb_plasenta_tidak_lahir, txt_plasenta_tidak_lahir);
            //        //functionSplitIndex_3(dataTable2.Rows[0]["laserasi"].ToString(), rb_laserasi, txt_laserasi);

            //        //functionSplitIndex_5(dataTable2.Rows[0]["laserasi_parinium"].ToString(), rb_la

            //    }
            //}
               

            //string sql_search;
            //GridView View = sender as GridView;
            //string s_pinfo = "";

            //s_pinfo = View.GetRowCellDisplayText(gridView1.FocusedRowHandle, View.Columns[0]);

            //string qdata = "SELECT B.* FROM CS_PATIENT A, CS_PATIENT_INFO B WHERE A.PATIENT_NO = B.PATIENT_NO AND A.PATIENT_NO = '" + s_pinfo + "' AND GROUP_PATIENT ='COMM' ";

            //dt_pasien = ConnOra.Data_Table_ora(qdata);

            //if (dt_pasien.Rows.Count > 0)
            //{
            //    FN.splitVal(FN.rowVal(dt_pasien, "AGAMA"), radioGroup18);

            //    //functionSplitIndex_3(dtpasien.Rows[0]["rangsang_laktil"].ToString(), radioGroup18, txt_rangsang_laktil);
            //    //functionSplitIndex_3(dataTable2.Rows[0]["plasenta_intack"].ToString(), rb_plasenta_intack, txt_plasenta_intack);
            //    //functionSplitIndex_3(dataTable2.Rows[0]["plasenta_tidak_lahir"].ToString(), rb_plasenta_tidak_lahir, txt_plasenta_tidak_lahir);
            //    //functionSplitIndex_3(dataTable2.Rows[0]["laserasi"].ToString(), rb_laserasi, txt_laserasi);

            //    //functionSplitIndex_5(dataTable2.Rows[0]["laserasi_parinium"].ToString(), rb_la

            //}
        }

        private void labelControl25_Click(object sender, EventArgs e)
        {

        }

        private void gridView1_Click(object sender, EventArgs e)
        {
           
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {

           
        }

        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
           
        }

        private void gridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }
        private Control lastSender;

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox34.Checked) txStsPsikologi.Enabled = true;
            else txStsPsikologi.Enabled = false;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived_scanner);

            //check_rfid();

            var port = new SerialPort("COM1");
            try
            {
                serialPort1.Open();
                serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort1_DataReceived);  
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR : COM1 Port not found." + "\n" + "Dev info: " + ex.Message);
            }

        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string strLine;
                strLine = serialPort1.ReadLine().ToString().Trim();
                strLine = strLine.Replace("\r\n", "");
                strLine = strLine.Replace("\u0002", "");
                strLine = strLine.Replace("\u0003", "");
                strLine = strLine.Replace("\r\n\u0003", "");
                strLine = strLine.Replace("\n", "");
                strLine = strLine.Replace("\r", "");

                //string strLine = serialPort1.ReadLine().ToString().Replace("\u0002", "").Replace("\r\n\u0003", "").Replace("\r", "");                
                RFIDSCAN = strLine.TrimStart('0');
                this.Invoke(new EventHandler(DisplayText));
            }
            catch (Exception)
            {
                serialPort1.Close();
            }
        }
        private void DisplayText(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            //if (textScanOut.ContainsFocus)
            //{
                bool isEmp = checkIfExists(RFIDSCAN);
                if (!isEmp)
                {
                    //checkIfExists(RFIDSCAN);
                    MessageBox.Show("RFID Berhasil di register.");
                }
                else
                {
                    textScanOut.Text = "";
                    textScanOut.BackColor = Color.FromArgb(255, 192, 192);

                    MessageBox.Show("There is an pasien using the same RFID Number, please use different RFID."); 
                    return;
                }
            //}
            //Cursor.Current = Cursors.Default;
        }

        private bool checkIfExists(string n)
        {
            try
            {
                string SQL = "";
                SQL += Environment.NewLine + "SELECT * FROM CS_PATIENT_INFO WHERE RFID_NO = '" + n + "' AND STATUS = 'A' ";

                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt); 

                if (dt.Rows.Count > 0)
                {
                    textScanOut.Text = "";
                    textScanOut.BackColor = Color.FromArgb(255, 192, 192);
                    
                    //MessageBox.Show("RFID number already registered.");
                    simpleButton5.Enabled = false ; 
                    return true;
                }
                else
                {
                    textScanOut.Text = n;
                    textScanOut.BackColor = Color.FromArgb(192, 255, 255);
                    simpleButton5.Enabled = true;
                    simpleButton5.Focus();
                     
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        //private void SetText_scanner(string text)
        //{
        //    this.textScanOut.Text = "";
        //    this.textScanOut.Text = text;
        //    text = text.Replace("\u0002", "");
        //    text = text.Replace("\r\n", "");
        //    //text = text.Replace("\n", "");
        //    text = text.Replace("\u0003", "");
        //    //this.textRecvTemp.Text = text;
        //    this.textBox12.Text = text;
        //}
        //private void port_DataReceived_scanner(object sender, SerialDataReceivedEventArgs e)
        //{
        //    Thread.Sleep(500);
        //    InputData_scanner = serialPort1.ReadExisting();
        //    if (textScanOut.Enabled == true)
        //    {
        //        if (InputData_scanner != String.Empty)
        //        {
        //            this.BeginInvoke(new SetTextCallback(SetText_scanner), new object[] { InputData_scanner });
        //        }
        //    }

        //}
        string lsMSG = "";
        int lsOK = 0;
        private void Blinking(String Message, int mbOk)
        {
            lsMSG = Message;
            lsOK = mbOk;
            timerStart.Interval = 150;
            timerStart.Enabled = true;

            timerEnd.Enabled = true;
            timerEnd.Interval = 2000;

        }
        public void check_rfid()
        {
            string temp = "";
            try
            {
                if (serialPort1.IsOpen)

                serialPort1.Close();
                temp = temp + " " + Convert.ToString(serialPort1.PortName);
                temp = temp + " " + Convert.ToString(serialPort1.BaudRate);
                temp = temp + " " + Convert.ToString(serialPort1.DataBits);
                temp = temp + " " + Convert.ToString(serialPort1.StopBits);
                temp = temp + " " + Convert.ToString(serialPort1.Parity);
                temp = temp + " " + Convert.ToString(serialPort1.Handshake);
                richTextBox1.Text = temp;
                serialPort1.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                //Blinking("Check COM Port RFID!", 0);
            }
        }

        private void PatientInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

        private void textRecvTemp_TextChanged(object sender, EventArgs e)
        {
            if (textRecvTemp.Text != "")
            {
                //check_rfid(textRecvTemp.Text);
            }
        }
       
        private void EnableTextEdit(object sender, EventArgs e)
        {
            Control parentControl = null;

            if (sender is RadioGroup)
            {
                RadioGroup radioGroup = (RadioGroup)sender;
                lastSender = radioGroup;
                parentControl = radioGroup.Parent;
                if (radioGroup.EditValue != null && radioGroup.EditValue?.ToString() == "1")
                {
                    if (parentControl != null) FN.EnableControls(parentControl, true, lastSender);
                }
                else
                {
                    if (parentControl != null) FN.EnableControls(parentControl, false, lastSender);
                }
            }
            else if (sender is CheckEdit)
            {
                CheckEdit checkEdit = (CheckEdit)sender;
                lastSender = checkEdit;
                parentControl = checkEdit.Parent;
                if (checkEdit.Checked)
                {
                    if (parentControl != null) FN.EnableControls(parentControl, true, lastSender);
                }
                else
                {
                    if (parentControl != null) FN.EnableControls(parentControl, false, lastSender);
                }
            }

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
