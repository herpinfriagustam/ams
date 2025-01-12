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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using Clinic.Report;
using DevExpress.XtraReports.UI;

namespace Clinic
{
    public partial class Inpatient : DevExpress.XtraEditors.XtraForm
    {
        ConnectDb ConnOra = new ConnectDb();
        List<Room> listRoom = new List<Room>();
        List<Diagnosa> listDiagnosa = new List<Diagnosa>();
        List<DiagnosaType> listDiagnosaType = new List<DiagnosaType>();
        List<Medicine> listMedicine = new List<Medicine>();
        List<MedicineInfo> listMedicineInfo = new List<MedicineInfo>();
        List<Formula> listFormula = new List<Formula>();
        List<Formula2> listFormula2 = new List<Formula2>();
        List<Stat> listGrpLaya = new List<Stat>();
        List<Layanan> listLaya2 = new List<Layanan>();
        List<Layanan> listLaya3= new List<Layanan>();

        DataTable dtGlDiag = new DataTable();
        DataTable dtGlMed = new DataTable();
        DataSet dsVisitDoc = new DataSet();
        DataSet dsSkd = new DataSet();
        DataSet dsMRUmum = new DataSet();
        DataSet dsMRRanap = new DataSet();

        //public string DB.vUserId = "";
        string today = DateTime.Now.ToString("dd/MM/yyyy");
        string pub_reg_date = "", pub_rm_no = "", pub_que = "", pub_date = "", pub_room = "";
        //string today = "2019-11-27";
        string workingDirectory = "", resourcesDirectory = "";

        private void Inpatient_Load(object sender, EventArgs e)
        {
            initData();
            LoadDataPasien();
        }

        public Inpatient()
        {
            InitializeComponent();
        }

        private void initData()
        {
            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = false;
            btnDelAnam.Enabled = false;
            btnCanAnam.Enabled = false;

            btnDelDiag.Enabled = false;
            btnAddDiag.Enabled = false;
            btnSaveDiag.Enabled = false;
            btnCanDiag.Enabled = false;

            btnDelVisit.Enabled = false;
            btnAddVisit.Enabled = false;
            btnSaveVisit.Enabled = false;
            btnCanVisit.Enabled = false;
            btnPrintVisit.Enabled = false;

            btnDelResep.Enabled = false;
            btnAddResep.Enabled = false;
            btnSaveResep.Enabled = false;
            btnCanResep.Enabled = false;

            btnDelTind.Enabled = false;
            btnAddTind.Enabled = false;
            btnSaveTind.Enabled = false;
            btnCanTind.Enabled = false;

            btnActSave.Enabled = false;

            string sql_room = " select room_id, room_name, bed_qty from KLINIK.cs_room order by room_name ";
            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(sql_room, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);

            listRoom.Clear();
            listRoom.Add(new Room() { roomCode = "", roomName = "Pilih", roomQty = "" });
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                listRoom.Add(new Room() { roomCode = dt2.Rows[i]["room_id"].ToString(), roomName = dt2.Rows[i]["room_name"].ToString(), roomQty = dt2.Rows[i]["bed_qty"].ToString() });
            }

            luSelRoom.Properties.DataSource = listRoom;
            luSelRoom.Properties.ValueMember = "roomCode";
            luSelRoom.Properties.DisplayMember = "roomName";

            luSelRoom.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            luSelRoom.Properties.DropDownRows = listRoom.Count;
            luSelRoom.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            luSelRoom.Properties.AutoSearchColumnIndex = 1;
            luSelRoom.Properties.NullText = "Pilih";

            dtGlDiag.Clear();
            string sql_poli = " select item_cd, initcap(item_name) item_name from KLINIK.cs_diagnosa_item where status = 'A' order by item_name ";
            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(sql_poli, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);
            dtGlDiag = dt;
            listDiagnosa.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listDiagnosa.Add(new Diagnosa() { diagnosaCode = dt.Rows[i]["item_cd"].ToString(), diagnosaName = dt.Rows[i]["item_name"].ToString() });
            }

            listDiagnosaType.Clear();
            listDiagnosaType.Add(new DiagnosaType() { diagnosaTypeCode = "B", diagnosaTypeName = "Diagnosa Awal" });
            listDiagnosaType.Add(new DiagnosaType() { diagnosaTypeCode = "E", diagnosaTypeName = "Diagnosa Akhir" });

            dtGlMed.Clear();
            string sql_med = " select med_cd, initcap(med_name) || ' (BPJS: ' || bpjs_cover || ')' med_name from KLINIK.cs_medicine where status = 'A' order by med_name ";
            OleDbConnection sqlConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql3 = new OleDbDataAdapter(sql_med, sqlConnect3);
            DataTable dt3 = new DataTable();
            dtGlMed = dt3;
            adSql3.Fill(dt3);
            listMedicine.Clear();
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                listMedicine.Add(new Medicine() { medicineCode = dt3.Rows[i]["med_cd"].ToString(), medicineName = dt3.Rows[i]["med_name"].ToString() });
            }

            listMedicineInfo.Clear();
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "A", medicineInfoName = "(P.C.) Sesudah Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "B", medicineInfoName = "(A.C.) Sebelum Makan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "C", medicineInfoName = "(P.R.N.) Bila Perlu" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "D", medicineInfoName = "(G.T.T) Diteteskan" });
            listMedicineInfo.Add(new MedicineInfo() { medicineInfoCode = "E", medicineInfoName = "(U.E) Pemakaian Luar" });

            listFormula.Clear();
            listFormula2.Clear();

            string SQL = "";
            SQL = SQL + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL = SQL + Environment.NewLine + "where 1=1 ";
            SQL = SQL + Environment.NewLine + "and (treat_type_id = 'TRT01' or treat_type_id is null) ";
            //SQL = SQL + Environment.NewLine + "and treat_group_id not in ('TRG02','TRG03','TRG05') ";

            OleDbConnection oraConnectly = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraly = new OleDbDataAdapter(SQL, oraConnectly);
            DataTable dtly = new DataTable();
            adOraly.Fill(dtly);
            listLaya2.Clear();
            for (int i = 0; i < dtly.Rows.Count; i++)
            {
                listLaya2.Add(new Layanan() { layananCode = dtly.Rows[i]["treat_item_id"].ToString(), layananName = dtly.Rows[i]["treat_item_name"].ToString() });
            }

            string sql_grplay = " select treat_group_id, initcap(treat_group_name) treat_group_name from KLINIK.cs_treatment_group  ";
            OleDbConnection oraConnectg = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOrag = new OleDbDataAdapter(sql_grplay, oraConnectg);
            DataTable dtg = new DataTable();
            adOrag.Fill(dtg);
            listGrpLaya.Clear();
            for (int i = 0; i < dtg.Rows.Count; i++)
            {
                listGrpLaya.Add(new Stat() { statCode = dtg.Rows[i]["treat_group_id"].ToString(), statName = dtg.Rows[i]["treat_group_name"].ToString() });
            }

            cmbReport.Items.Clear();
            cmbReport.Items.Add("Laporan MR");
            cmbReport.Items.Add("Laporan Ranap");
            cmbReport.SelectedIndex = 0;

            string SQL3 = "";
            SQL3 = SQL3 + Environment.NewLine + "select treat_item_id, initcap(treat_item_name) treat_item_name ";
            SQL3 = SQL3 + Environment.NewLine + "from KLINIK.cs_treatment_item ";
            SQL3 = SQL3 + Environment.NewLine + "where 1=1 ";
            SQL3 = SQL3 + Environment.NewLine + "and (treat_type_id = 'TRT01' or treat_type_id is null) ";
            //SQL = SQL + Environment.NewLine + "and treat_group_id not in ('TRG02','TRG03','TRG05') ";

            OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra3 = new OleDbDataAdapter(SQL, oraConnect3);
            DataTable dt3a = new DataTable();
            adOra3.Fill(dt3a);
            listLaya3.Clear();
            for (int i = 0; i < dt3a.Rows.Count; i++)
            {
                listLaya3.Add(new Layanan() { layananCode = dt3a.Rows[i]["treat_item_id"].ToString(), layananName = dt3a.Rows[i]["treat_item_name"].ToString() });
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void gridView4_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView5_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView6_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView7_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView8_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {

        }


        private void LoadDataPasien()
        {
            string SQL = "";

            //string a = luSelRoom.GetColumnValue("roomCode").ToString();

            SQL = "";
            SQL = SQL + Environment.NewLine + "select a.inpatient_id, b.patient_no, a.rm_no, b.que01,a.DATE_IN, c.NAME,";
            SQL = SQL + Environment.NewLine + "e.ROOM_NAME|| ' (' || substr(a.room_id,-2) || ')' ruangan, round(sysdate-a.date_in) lama, ";
            SQL = SQL + Environment.NewLine + "(select count(0) from KLINIK.cs_visit_doc where visit_dt =b.visit_date) cnt_visit, b.type_patient, d.room_id rid, e.room_name  ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_inpatient a ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_visit b on (a.inpatient_id=b.inpatient_id ) ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_patient_info c on (b.patient_no=c.patient_no) ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_bed d on (a.room_id=d.bed_id) ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_room e on (d.room_id=e.room_id) ";
            SQL = SQL + Environment.NewLine + "where a.status='OPN' ";
            if (luSelRoom.Text == "Pilih")
            {
                SQL = SQL + Environment.NewLine + "and d.room_id like '%%' ";
            }
            else
            {
                SQL = SQL + Environment.NewLine + "and d.room_id like '%" + luSelRoom.GetColumnValue("roomCode").ToString() + "%' ";
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
                //gridView1.BestFitColumns();

                gridView1.Columns[0].Caption = "ID";
                gridView1.Columns[1].Caption = "Pasien No";
                gridView1.Columns[2].Caption = "RM No";
                gridView1.Columns[3].Caption = "Visit No";
                gridView1.Columns[4].Caption = "Tanggal";
                gridView1.Columns[5].Caption = "Nama";
                gridView1.Columns[6].Caption = "Ruangan";
                gridView1.Columns[7].Caption = "Lama Hari";
                gridView1.Columns[8].Caption = "Kunjungan";
                gridView1.Columns[9].Caption = "Tipe";
                gridView1.Columns[10].Caption = "RID";
                gridView1.Columns[11].Caption = "Nama Ruangan";

                //gridView1.Columns[0].Width = 80;
                gridView1.Columns[0].Visible = false;
                gridView1.Columns[1].Visible = false;
                gridView1.Columns[2].Visible = false;
                gridView1.Columns[3].Visible = false;
                gridView1.Columns[4].Visible = false;
                gridView1.Columns[9].Visible = false;
                gridView1.Columns[10].Visible = false;
                gridView1.Columns[11].Visible = false;
                gridView1.BestFitColumns();

                if (gridView1.RowCount > 0)
                {

                }
                else
                {
                    lInfoNama.Text = "-";
                    lInfoDiag.Text = "-";
                    lInfoTipe.Text = "-";
                    lInfoTglMsk.Text = "-";
                    lInfoRiw.Text = "-";
                    lInfoAler.Text = "-";
                }
            }
            catch (Exception ex)
            {
                //loading.CloseWaitForm();
                MessageBox.Show("ERROR: " + ex.Message);
            }
           

        }

        private void luSelRoom_EditValueChanged(object sender, EventArgs e)
        {
            LoadDataPasien();
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            LoadDataPasien();
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            btnAddAnam.Enabled = false;
            btnSaveAnam.Enabled = false;
            btnDelAnam.Enabled = false;
            btnCanAnam.Enabled = false;

            btnDelDiag.Enabled = false;
            btnAddDiag.Enabled = false;
            btnSaveDiag.Enabled = false;
            btnCanDiag.Enabled = false;

            btnDelVisit.Enabled = false;
            btnAddVisit.Enabled = false;
            btnSaveVisit.Enabled = false;
            btnCanVisit.Enabled = false;
            btnPrintVisit.Enabled = false;

            btnDelResep.Enabled = false;
            btnAddResep.Enabled = false;
            btnSaveResep.Enabled = false;
            btnCanResep.Enabled = false;

            btnDelTind.Enabled = false;
            btnAddTind.Enabled = false;
            btnSaveTind.Enabled = false;
            btnCanTind.Enabled = false;

            btnActSave.Enabled = false;

            GridView View = sender as GridView;
            

            string s_id = "", s_pasno = "", s_rmno = "", s_que = "", s_date = "", s_nama = "", s_tipe = "", s_tp = "", s_room = "";

            s_id = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);
            s_pasno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[1]);
            s_rmno = View.GetRowCellDisplayText(e.RowHandle, View.Columns[2]);
            s_que = View.GetRowCellDisplayText(e.RowHandle, View.Columns[3]);
            s_date = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
            s_nama = View.GetRowCellDisplayText(e.RowHandle, View.Columns[5]);
            s_room = View.GetRowCellDisplayText(e.RowHandle, View.Columns[6]);
            s_tipe = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);
            pub_reg_date = s_date;
            pub_rm_no = s_rmno;
            pub_que = s_que;
            pub_date = s_date;
            pub_room = s_room;

            if (s_tipe == "P")
            {
                s_tp = "Perusahaan";
            }
            else if (s_tipe == "B")
            {
                s_tp = "BPJS";
            }
            else
            {
                s_tp = "Umum";
            }

            LoadDataAddInfo(s_id, s_nama,s_tp,s_date);

            if (xtraTabControl2.SelectedTabPage.Text == "Anamnesa")
            {
                LoadDataAnam(s_rmno, s_que, s_date);
                LoadDataDiag(s_rmno, s_que, s_date);
            }
            else if (xtraTabControl2.SelectedTabPage.Text == "Kunjungan Dokter")
            {
                LoadDataVisit(s_rmno, s_que, s_date);
            }
            else if (xtraTabControl2.SelectedTabPage.Text == "Terapi / Resep")
            {
                LoadDataReceipt(s_rmno, s_que, s_date);
            }
            else if (xtraTabControl2.SelectedTabPage.Text == "SKD")
            {
                LoadDataSKD(s_rmno, s_que, s_date);
            }
            else if (xtraTabControl2.SelectedTabPage.Text == "Pelayanan")
            {
                LoadDataLayanan(s_rmno, s_que, s_date);
            }
            else if (xtraTabControl2.SelectedTabPage.Text == "Tindakan")
            {
                LoadDataTindakan(pub_rm_no, pub_que, pub_reg_date);
            }


            if (xtraTabControl1.SelectedTabPage.Text == "Medical Record")
            {
                LoadDataMR(s_rmno, s_que, s_date);
            }

        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Text == "Medical Record")
            {
                if (gridView7.RowCount > 0)
                {
                    LoadDataMR(pub_rm_no, pub_que, pub_reg_date);
                }
                else
                {

                    LoadDataMR("", "", "");
                    btnReportMr.Enabled = false;

                }
            }
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl2.SelectedTabPage.Text == "Anamnesa")
            {
                if (gridView1.RowCount > 0)
                {
                    LoadDataAnam(pub_rm_no, pub_que, pub_reg_date);
                    LoadDataDiag(pub_rm_no, pub_que, pub_reg_date);
                }
                else
                {
                    
                    LoadDataAnam("", "", "");
                    btnCanAnam.Enabled = false; btnDelAnam.Enabled = false; btnAddAnam.Enabled = false; btnSaveAnam.Enabled = false;
                    
                    LoadDataDiag("", "", "");
                    btnCanDiag.Enabled = false; btnDelDiag.Enabled = false; btnAddDiag.Enabled = false; btnSaveDiag.Enabled = false;
                }
                
            }
            else if (xtraTabControl2.SelectedTabPage.Text == "Kunjungan Dokter")
            {
                if (gridView1.RowCount > 0)
                {
                    LoadDataVisit(pub_rm_no, pub_que, pub_reg_date);
                }
                else
                {

                    LoadDataVisit("", "", "");
                    btnPrintVisit.Enabled = false; btnCanVisit.Enabled = false; btnDelVisit.Enabled = false; btnAddVisit.Enabled = false; btnSaveVisit.Enabled = false;
                    
                }

            }
            else if (xtraTabControl2.SelectedTabPage.Text == "Terapi / Resep")
            {
                if (gridView1.RowCount > 0)
                {
                    LoadDataReceipt(pub_rm_no, pub_que, pub_reg_date);
                }
                else
                {

                    LoadDataReceipt("", "", "");
                    btnCanResep.Enabled = false; btnDelResep.Enabled = false; btnAddResep.Enabled = false; btnSaveResep.Enabled = false;

                }
            }
            else if (xtraTabControl2.SelectedTabPage.Text == "SKD")
            {
                if (gridView1.RowCount > 0)
                {
                    LoadDataSKD(pub_rm_no, pub_que, pub_reg_date);
                }
                else
                {

                    LoadDataSKD("", "", "");
                    skdUSave.Enabled = false; skdUPrint.Enabled = false; skdUDel.Enabled = false; 

                }
            }
            else if (xtraTabControl2.SelectedTabPage.Text == "Pelayanan")
            {
                if (gridView1.RowCount > 0)
                {
                    LoadDataLayanan(pub_rm_no, pub_que, pub_reg_date);
                }
                else
                {

                    LoadDataLayanan("", "", "");
                    btnDelTind.Enabled = false; btnAddTind.Enabled = false; btnSaveTind.Enabled = false;  btnCanTind.Enabled = false;

                }
            }
            else if (xtraTabControl2.SelectedTabPage.Text == "Tindakan")
            {
                if (gridView1.RowCount > 0)
                {
                    LoadDataTindakan(pub_rm_no, pub_que, pub_reg_date);
                }
                else
                {

                    LoadDataTindakan("", "", "");
                    btnActSave.Enabled = false;

                }
            }

        }

        private void LoadDataAddInfo(string id, string nama, string tipe, string date)
        {
            lInfoNama.Text = nama;
            lInfoTipe.Text = tipe;
            lInfoTglMsk.Text = date;

            string SQL = "", SQL2 = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select disease_now, allergy  ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_visit a ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_inpatient b on (a.inpatient_id=b.inpatient_id) ";
            SQL = SQL + Environment.NewLine + "join KLINIK.cs_anamnesa c on (trunc(visit_date)=c.insp_date and a.que01=c.visit_no and b.rm_no=c.rm_no) ";
            SQL = SQL + Environment.NewLine + "where a.inpatient_id='" + id + "' ";
            SQL = SQL + Environment.NewLine + "and b.reg_date=c.visit_dt ";
            SQL = SQL + Environment.NewLine + "and b.status='OPN' ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                lInfoRiw.Text = dt.Rows[0]["disease_now"].ToString();
                lInfoAler.Text = dt.Rows[0]["allergy"].ToString();
            }
            else
            {
                lInfoRiw.Text = "-";
                lInfoAler.Text = "-";
            }

            SQL2 = "";
            SQL2 = SQL2 + Environment.NewLine + "select item_name ";
            SQL2 = SQL2 + Environment.NewLine + "from KLINIK.cs_visit a ";
            SQL2 = SQL2 + Environment.NewLine + "join KLINIK.cs_inpatient b on (a.inpatient_id=b.inpatient_id) ";
            SQL2 = SQL2 + Environment.NewLine + "join KLINIK.cs_diagnosa c on (trunc(visit_date)=c.insp_date and a.que01=c.visit_no and b.rm_no=c.rm_no) ";
            SQL2 = SQL2 + Environment.NewLine + "join KLINIK.cs_diagnosa_item d on (c.item_cd = d.item_cd  ) ";
            SQL2 = SQL2 + Environment.NewLine + "where a.inpatient_id='" + id + "' ";
            SQL2 = SQL2 + Environment.NewLine + "and b.reg_date=c.visit_dt ";
            SQL2 = SQL2 + Environment.NewLine + "and c.type_diagnosa = 'B' ";
            


            OleDbConnection sqlConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql2 = new OleDbDataAdapter(SQL2, sqlConnect2);
            DataTable dt2 = new DataTable();
            adSql2.Fill(dt2);

            if (dt2.Rows.Count > 0)
            {
                lInfoDiag.Text = dt2.Rows[0]["item_name"].ToString();
            }
            else
            {
                lInfoDiag.Text = "-";
            }

        }

        
        private void LoadDataAnam(string rmno, string que, string date)
        {
            lAnamNm.Text = lInfoNama.Text;
            lAnamDiag.Text = lInfoDiag.Text;

            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + "select 'S' action, anamnesa_id, visit_dt tgl_reg,  ";
            SQL = SQL + Environment.NewLine + "insp_date tgl, blood_press, pulse, temperature, bb, tb, anamnesa, ";
            SQL = SQL + Environment.NewLine + "rm_no, visit_no ";
            SQL = SQL + Environment.NewLine + "from KLINIK.cs_anamnesa ";
            SQL = SQL + Environment.NewLine + "where rm_no='" + rmno + "' ";
            SQL = SQL + Environment.NewLine + "and trunc(visit_dt) = trunc(to_date(substr('" + date + "',1,10),'dd/MM/yyyy')) ";
            SQL = SQL + Environment.NewLine + "and visit_no='" + que + "' ";
            SQL = SQL + Environment.NewLine + "order by insp_date";

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

            gridView2.Columns[0].Caption = "Action";
            gridView2.Columns[1].Caption = "ID";
            gridView2.Columns[2].Caption = "Tgl Reg";
            gridView2.Columns[3].Caption = "Tanggal";
            gridView2.Columns[4].Caption = "Tensi";
            gridView2.Columns[5].Caption = "Nadi";
            gridView2.Columns[6].Caption = "Suhu";
            gridView2.Columns[7].Caption = "BB";
            gridView2.Columns[8].Caption = "TB";
            gridView2.Columns[9].Caption = "Keluhan";
            gridView2.Columns[10].Caption = "RM No";
            gridView2.Columns[11].Caption = "Visit No";

            gridView2.Columns[2].Width = 70;
            gridView2.Columns[3].Width = 50;
            gridView2.Columns[4].Width = 50;
            gridView2.Columns[5].Width = 50;
            gridView2.Columns[6].Width = 50;
            gridView2.Columns[7].Width = 50;
            gridView2.Columns[8].Width = 50;

            gridView2.Columns[0].Visible = false;
            gridView2.Columns[1].Visible = false;
            gridView2.Columns[2].Visible = false;
            gridView2.Columns[10].Visible = false;
            gridView2.Columns[11].Visible = false;

            gridView2.BestFitColumns();

            if (gridView2.RowCount <= 0)
            {
                btnAddAnam.Enabled = true;
                btnDelAnam.Enabled = false;
                btnSaveAnam.Enabled = false;
                btnCanAnam.Enabled = true;
            }
            else
            {
                btnAddAnam.Enabled = true;
                btnDelAnam.Enabled = true;
                btnSaveAnam.Enabled = true;
                btnCanAnam.Enabled = true;
            }


        }

        private void btnAddAnam_Click(object sender, EventArgs e)
        {
            gridView2.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView2.AddNewRow();
        }

        private void gridView2_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I"); 
            view.SetRowCellValue(e.RowHandle, view.Columns[2], pub_reg_date);
            view.SetRowCellValue(e.RowHandle, view.Columns[3], DateTime.Now.AddHours(0));
            view.SetRowCellValue(e.RowHandle, view.Columns[10], pub_rm_no);
            view.SetRowCellValue(e.RowHandle, view.Columns[11], pub_que);
        }

        private void btnSaveAnam_Click(object sender, EventArgs e)
        {
            string action = "", id = "", reg = "", rm="", que="";
            string tgl = "", tensi = "", nadi = "", suhu = "", bb = "", tb = "", keluhan = "";
            string sql_cnt = "", anam_cnt = "", sql_update = "";

            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                action = gridView2.GetRowCellValue(i, gridView2.Columns[0]).ToString();
                id = gridView2.GetRowCellValue(i, gridView2.Columns[1]).ToString();
                reg = gridView2.GetRowCellValue(i, gridView2.Columns[2]).ToString();
                tgl = gridView2.GetRowCellValue(i, gridView2.Columns[3]).ToString();
                tensi = gridView2.GetRowCellValue(i, gridView2.Columns[4]).ToString();
                nadi = gridView2.GetRowCellValue(i, gridView2.Columns[5]).ToString();
                suhu = gridView2.GetRowCellValue(i, gridView2.Columns[6]).ToString();
                bb = gridView2.GetRowCellValue(i, gridView2.Columns[7]).ToString();
                tb = gridView2.GetRowCellValue(i, gridView2.Columns[8]).ToString();
                keluhan = gridView2.GetRowCellValue(i, gridView2.Columns[9]).ToString();
                rm = gridView2.GetRowCellValue(i, gridView2.Columns[10]).ToString();
                que = gridView2.GetRowCellValue(i, gridView2.Columns[11]).ToString();

                if (tgl == "")
                {
                    MessageBox.Show("Tanggal harus diisi");
                }
                else
                {
                    if (action == "I")
                    {
                        sql_cnt = " select count(0) cnt from KLINIK.cs_anamnesa where anamnesa_id = '" + id + "' ";
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

                            try
                            {

                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                command.CommandText = " insert into KLINIK.cs_anamnesa (anamnesa_id, rm_no, insp_date, visit_no, visit_dt, blood_press, pulse, temperature, bb, tb, anamnesa, ins_date, ins_emp) values(cs_anamnesa_seq.nextval, '" + rm + "', to_date('" + tgl + "', 'dd/MM/yyyy hh:mm:ss'), '" + que + "', to_date('" + reg + "', 'dd/MM/yyyy'), '" + tensi + "', '" + nadi + "', '" + suhu + "', '" + bb + "', '" + tb + "', '" + keluhan + "', sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                //if (status == "First Inspection")
                                //{
                                //    command.CommandText = " update cs_visit set status = 'INS', time_reservation=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'dd/MM/yyyy') = '" + date + "' and que01 = '" + que + "' ";
                                //    command.ExecuteNonQuery();
                                //}


                                trans.Commit();
                                //string cek = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other, infop1, infop2, infop3, infop4, infop5, ins_date, ins_emp) values(cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "', 'dd/MM/yyyy'), '" + tensi + "', '" + nadi + "', '" + suhu + "', '" + alergi + "', '" + keluhan + "', '" + que + "', '" + rnow + "', '" + rold + "', '" + rfam + "', '" + pfisik + "', '" + padd + "', '" + infop1 + "', '" + infop2 + "', '" + infop3 + "', '" + infop4 + "', '" + infop5 + "', sysdate, '" + DB.vUserId + "') ";
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + cek);
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
                        
                        sql_update = sql_update + Environment.NewLine + " update KLINIK.cs_anamnesa set";
                        sql_update = sql_update + Environment.NewLine + " insp_date = to_date('" + tgl + "','dd/MM/yyyy'), ";
                        sql_update = sql_update + Environment.NewLine + " blood_press = '" + tensi + "', ";
                        sql_update = sql_update + Environment.NewLine + " pulse = '" + nadi + "', ";
                        sql_update = sql_update + Environment.NewLine + " temperature = '" + suhu + "', ";
                        sql_update = sql_update + Environment.NewLine + " bb = '" + bb + "', ";
                        sql_update = sql_update + Environment.NewLine + " tb = '" + tb + "', ";
                        sql_update = sql_update + Environment.NewLine + " anamnesa = '" + keluhan + "', ";
                        sql_update = sql_update + Environment.NewLine + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + Environment.NewLine + " where anamnesa_id = '" + id + "' ";


                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil dirubah");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
                
            }
            LoadDataAnam(rm, que, reg);
        }

        private void btnCanAnam_Click(object sender, EventArgs e)
        {
            string stat = gridView2.GetRowCellDisplayText(gridView2.FocusedRowHandle, gridView2.Columns[3]);
            if (stat == "")
            {
                gridView2.DeleteRow(gridView2.FocusedRowHandle);
            }
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveAnam.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tanggal")
            {
                string tmp_reg = "", tmp_ins = "";
                string ins_dt = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                string reg_dt = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                string rm = view.GetRowCellValue(e.RowHandle, view.Columns[10]).ToString();
                string que = view.GetRowCellValue(e.RowHandle, view.Columns[11]).ToString();

                tmp_reg = reg_dt.Replace("/", "").Replace(":", "").Replace(" ", "");
                tmp_ins = ins_dt.Replace("/", "").Replace(":", "").Replace(" ", "");

                if (Convert.ToInt64(tmp_ins) < Convert.ToInt64(tmp_reg))
                {
                    MessageBox.Show("Tgl Periksa Kurang dari Tgl Registrasi");
                    //gridView2.DeleteRow(gridView2.FocusedRowHandle);
                    LoadDataAnam(rm, que, reg_dt);
                    return;
                }
                else
                {

                }
            }

            if (e.Column.Caption != "Action")
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

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption != "Action" )
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDelAnam_Click(object sender, EventArgs e)
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

                id = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns[1]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + Environment.NewLine + " delete KLINIK.cs_anamnesa ";
                sql_delete = sql_delete + Environment.NewLine + " where anamnesa_id = '" + id + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gridView2.DeleteRow(gridView2.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }


        private void LoadDataDiag(string rmno, string que, string date)
        {
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + " select 'S' action, a.diagnosa_id , to_char(visit_dt,'dd/MM/yyyy') tgl_reg, ";
            SQL = SQL + Environment.NewLine + " rm_no, visit_no, to_char(insp_date,'dd/MM/yyyy') tgl,  ";
            SQL = SQL + Environment.NewLine + " a.item_cd, initcap(c.cat_name) category_name, type_diagnosa, a.remark";
            SQL = SQL + Environment.NewLine + " from KLINIK.cs_diagnosa a  ";
            SQL = SQL + Environment.NewLine + " join KLINIK.cs_diagnosa_item b on a.item_cd = b.item_cd  ";
            SQL = SQL + Environment.NewLine + " join KLINIK.cs_diagnosa_category c on b.cat_id = c.cat_id ";
            SQL = SQL + Environment.NewLine + " where a.rm_no = '" + rmno + "'  ";
            SQL = SQL + Environment.NewLine + " and to_char(a.visit_dt,'dd/MM/yyyy') = '" + date + "'  ";
            SQL = SQL + Environment.NewLine + " and a.visit_no = '" + que + "'  ";
            SQL = SQL + Environment.NewLine + " order by type_diagnosa ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl3.DataSource = null;
            gridView3.Columns.Clear();
            gridControl3.DataSource = dt;

            gridView3.OptionsView.ColumnAutoWidth = true;
            gridView3.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView3.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView3.IndicatorWidth = 40;
            //gridView3.OptionsBehavior.Editable = false;

            gridView3.Columns[0].Caption = "Action";
            gridView3.Columns[1].Caption = "ID";
            gridView3.Columns[2].Caption = "Tgl Reg";
            gridView3.Columns[3].Caption = "RM No";
            gridView3.Columns[4].Caption = "Visit No";
            gridView3.Columns[5].Caption = "Tanggal";
            gridView3.Columns[6].Caption = "Diagnosa";
            gridView3.Columns[7].Caption = "Kategori";
            gridView3.Columns[8].Caption = "Tipe";
            gridView3.Columns[9].Caption = "Remark";

            RepositoryItemGridLookUpEdit gldiag = new RepositoryItemGridLookUpEdit();
            gldiag.DataSource = listDiagnosa;
            gldiag.ValueMember = "diagnosaCode";
            gldiag.DisplayMember = "diagnosaName";

            gldiag.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            gldiag.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            gldiag.ImmediatePopup = true;
            gldiag.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            gldiag.NullText = "";
            gridView3.Columns[6].ColumnEdit = gldiag;

            RepositoryItemLookUpEdit diagnosaTypeLookup = new RepositoryItemLookUpEdit();
            diagnosaTypeLookup.DataSource = listDiagnosaType;
            diagnosaTypeLookup.ValueMember = "diagnosaTypeCode";
            diagnosaTypeLookup.DisplayMember = "diagnosaTypeName";

            diagnosaTypeLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            diagnosaTypeLookup.DropDownRows = listDiagnosaType.Count;
            diagnosaTypeLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            diagnosaTypeLookup.AutoSearchColumnIndex = 1;
            diagnosaTypeLookup.NullText = "";
            gridView3.Columns[8].ColumnEdit = diagnosaTypeLookup;

            gridView3.Columns[7].OptionsColumn.ReadOnly = true;
            gridView3.Columns[5].Width = 70;

            gridView3.Columns[0].Visible = false;
            gridView3.Columns[1].Visible = false;
            gridView3.Columns[2].Visible = false;
            gridView3.Columns[3].Visible = false;
            gridView3.Columns[4].Visible = false;

            gridView3.BestFitColumns();

            if (gridView3.RowCount <= 0)
            {
                btnAddDiag.Enabled = true;
                btnDelDiag.Enabled = false;
                btnSaveDiag.Enabled = false;
                btnCanDiag.Enabled = true;
            }
            else
            {
                btnAddDiag.Enabled = true;
                btnDelDiag.Enabled = true;
                btnSaveDiag.Enabled = true;
                btnCanDiag.Enabled = true;
            }
        }

        private void btnAddDiag_Click(object sender, EventArgs e)
        {
            gridView3.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView3.AddNewRow();
        }

        private void gridView3_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[2], pub_reg_date);
            view.SetRowCellValue(e.RowHandle, view.Columns[3], pub_rm_no);
            view.SetRowCellValue(e.RowHandle, view.Columns[4], pub_que);
        }

        private void btnSaveDiag_Click(object sender, EventArgs e)
        {
            string action = "", id = "", reg = "", rm = "", que = "";
            string tgl = "", diagnosa = "", tipe = "", remark = "";
            string sql_cnt = "", anam_cnt = "", sql_update = "";

            for (int i = 0; i < gridView3.DataRowCount; i++)
            {
                action = gridView3.GetRowCellValue(i, gridView3.Columns[0]).ToString();
                id = gridView3.GetRowCellValue(i, gridView3.Columns[1]).ToString();
                reg = gridView3.GetRowCellValue(i, gridView3.Columns[2]).ToString();
                tgl = gridView3.GetRowCellValue(i, gridView3.Columns[5]).ToString();
                rm = gridView3.GetRowCellValue(i, gridView3.Columns[3]).ToString();
                que = gridView3.GetRowCellValue(i, gridView3.Columns[4]).ToString();
                diagnosa = gridView3.GetRowCellValue(i, gridView3.Columns[6]).ToString();
                tipe = gridView3.GetRowCellValue(i, gridView3.Columns[8]).ToString();
                remark = gridView3.GetRowCellValue(i, gridView3.Columns[9]).ToString();

                if (diagnosa == "")
                {
                    MessageBox.Show("Diagnosa harus diisi");
                }
                else if (tgl == "")
                {
                    MessageBox.Show("Tanggal harus diisi");
                }
                else if (tipe == "")
                {
                    MessageBox.Show("Tipe Diagnosa harus diisi");
                }
                else
                {
                    if (action == "I")
                    {
                        sql_cnt = " select count(0) cnt from KLINIK.cs_diagnosa where to_char(visit_dt,'dd/MM/yyyy') = '" + reg + "' and visit_no = '" + que + "' and rm_no = '" + rm + "' " + " and type_diagnosa = '" + tipe + "' ";
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

                            try
                            {

                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                command.CommandText = " insert into KLINIK.cs_diagnosa (diagnosa_id, rm_no, visit_dt, insp_date, item_cd, type_diagnosa, remark, visit_no, ins_date, ins_emp) values(cs_diagnosa_seq.nextval, '" + rm + "', to_date('" + reg + "', 'dd/MM/yyyy'), to_date('" + tgl + "', 'dd/MM/yyyy'), '" + diagnosa + "', '" + tipe + "', '" + remark + "', '" + que + "', sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                //if (status == "First Inspection")
                                //{
                                //    command.CommandText = " update cs_visit set status = 'INS', time_reservation=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'dd/MM/yyyy') = '" + date + "' and que01 = '" + que + "' ";
                                //    command.ExecuteNonQuery();
                                //}


                                trans.Commit();
                                //string cek = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other, infop1, infop2, infop3, infop4, infop5, ins_date, ins_emp) values(cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "', 'dd/MM/yyyy'), '" + tensi + "', '" + nadi + "', '" + suhu + "', '" + alergi + "', '" + keluhan + "', '" + que + "', '" + rnow + "', '" + rold + "', '" + rfam + "', '" + pfisik + "', '" + padd + "', '" + infop1 + "', '" + infop2 + "', '" + infop3 + "', '" + infop4 + "', '" + infop5 + "', sysdate, '" + DB.vUserId + "') ";
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + cek);
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

                        sql_update = sql_update + Environment.NewLine + " update KLINIK.cs_diagnosa set";
                        sql_update = sql_update + Environment.NewLine + " insp_date = to_date('" + tgl + "','dd/MM/yyyy'), ";
                        sql_update = sql_update + Environment.NewLine + " item_cd = '" + diagnosa + "', ";
                        sql_update = sql_update + Environment.NewLine + " type_diagnosa = '" + tipe + "', ";
                        sql_update = sql_update + Environment.NewLine + " remark = '" + remark + "', ";
                        sql_update = sql_update + Environment.NewLine + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + Environment.NewLine + " where diagnosa_id = '" + id + "' ";


                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil dirubah");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
                
            }
            LoadDataDiag(rm, que, reg);
        }

        private void btnCanDiag_Click(object sender, EventArgs e)
        {
            string stat = gridView3.GetRowCellDisplayText(gridView3.FocusedRowHandle, gridView3.Columns[6]);
            if (stat == "")
            {
                gridView3.DeleteRow(gridView3.FocusedRowHandle);
            }
        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveDiag.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tanggal")
            {
                string tmp_reg = "", tmp_ins = "";
                string ins_dt = view.GetRowCellValue(e.RowHandle, view.Columns[5]).ToString();
                string reg_dt = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                string rm = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                string que = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();

                tmp_reg = reg_dt.Replace("-", "");
                tmp_ins = ins_dt.Replace("-", "");

                if (Convert.ToInt32(tmp_ins) < Convert.ToInt32(tmp_reg))
                {
                    MessageBox.Show("Tgl Periksa Kurang dari Tgl Registrasi");
                    //gridView2.DeleteRow(gridView2.FocusedRowHandle);
                    LoadDataDiag(rm, que, reg_dt);
                    return;
                }
                else
                {

                }
            }

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Diagnosa" || e.Column.Caption == "Tipe" || e.Column.Caption == "Remark")
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

        private void gridView3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Diagnosa" || e.Column.Caption == "Tipe" || e.Column.Caption == "Remark")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDelDiag_Click(object sender, EventArgs e)
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

                id = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns[1]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + Environment.NewLine + " delete KLINIK.cs_diagnosa ";
                sql_delete = sql_delete + Environment.NewLine + " where diagnosa_id = '" + id + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gridView3.DeleteRow(gridView3.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void LoadDataVisit(string rmno, string que, string date)
        {
            lVisitNm.Text = lInfoNama.Text;
            lVisitDiag.Text = lInfoDiag.Text;
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + " select 'S' action, vdoc_id, to_char(visit_dt,'dd/MM/yyyy') tgl_reg,  ";
            SQL = SQL + Environment.NewLine + " a.rm_no, visit_no, to_char(insp_date,'dd/MM/yyyy') tgl, visit_note, ";
            SQL = SQL + Environment.NewLine + " klinik.fn_get_pic(a.rm_no,visit_dt,visit_no) pic, name, ";
            SQL = SQL + Environment.NewLine + " to_char(birth_date,'dd/MM/yyyy') || ' (' || round((sysdate-birth_date)/30/12) || ')' age, ";
            SQL = SQL + Environment.NewLine + " '" + pub_room + "' as ruangan, a.detail_id ";
            SQL = SQL + Environment.NewLine + " from KLINIK.cs_visit_doc a ";
            SQL = SQL + Environment.NewLine + " join KLINIK.cs_patient b on (a.rm_no=b.rm_no) ";
            SQL = SQL + Environment.NewLine + " join KLINIK.cs_patient_info c on (b.patient_no=c.patient_no) ";
            SQL = SQL + Environment.NewLine + " left join KLINIK.cs_treatment_detail d on (a.detail_id=d.detail_id)  ";
            SQL = SQL + Environment.NewLine + " where 1=1 ";
            SQL = SQL + Environment.NewLine + " and b.group_patient = 'COMM'  ";
            SQL = SQL + Environment.NewLine + " and a.rm_no = '" + rmno + "'   ";
            //SQL = SQL + Environment.NewLine + " and to_char(a.visit_dt,'dd/MM/yyyy') = '" + date + "'   ";
            SQL = SQL + Environment.NewLine + " and visit_no = '" + que + "'   ";
            SQL = SQL + Environment.NewLine + " order by insp_date  ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl4.DataSource = null;
            gridView4.Columns.Clear();
            gridControl4.DataSource = dt;

            gridView4.OptionsView.ColumnAutoWidth = true;
            gridView4.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView4.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView4.IndicatorWidth = 40;
            //gridView4.OptionsBehavior.Editable = false;
            gridView4.OptionsView.RowAutoHeight = true;

            RepositoryItemMemoEdit note = new RepositoryItemMemoEdit();
            gridControl4.RepositoryItems.Add(note);
            gridView4.Columns[6].ColumnEdit = note;

            gridView4.Columns[0].Caption = "Action";
            gridView4.Columns[1].Caption = "ID";
            gridView4.Columns[2].Caption = "Tgl Reg";
            gridView4.Columns[3].Caption = "RM No";
            gridView4.Columns[4].Caption = "Visit No";
            gridView4.Columns[5].Caption = "Tanggal";
            gridView4.Columns[6].Caption = "Catatan Dokter";
            gridView4.Columns[7].Caption = "Pemeriksa";
            gridView4.Columns[8].Caption = "Nama";
            gridView4.Columns[9].Caption = "Umur";
            gridView4.Columns[10].Caption = "Ruangan";
            gridView4.Columns[11].Caption = "Det ID";

            gridView4.Columns[5].MinWidth = 70;
            gridView4.Columns[5].MaxWidth = 70;

            gridView4.Columns[0].Visible = false;
            gridView4.Columns[1].Visible = false;
            gridView4.Columns[2].Visible = false;
            gridView4.Columns[3].Visible = false;
            gridView4.Columns[4].Visible = false;
            gridView4.Columns[7].Visible = false;
            gridView4.Columns[8].Visible = false;
            gridView4.Columns[9].Visible = false;
            gridView4.Columns[10].Visible = false;
            gridView4.Columns[11].Visible = false;

            //gridView4.BestFitColumns();

            if (gridView4.RowCount <= 0)
            {
                btnAddVisit.Enabled = true;
                btnDelVisit.Enabled = false;
                btnSaveVisit.Enabled = false;
                btnCanVisit.Enabled = true;
                btnPrintVisit.Enabled = false;
            }
            else
            {
                btnAddVisit.Enabled = true;
                btnDelVisit.Enabled = true;
                btnSaveVisit.Enabled = true;
                btnCanVisit.Enabled = true;
                btnPrintVisit.Enabled = true;
            }

            dsVisitDoc.Tables.Clear();
            dsVisitDoc.Tables.Add(dt);
        }

        private void btnAddVisit_Click(object sender, EventArgs e)
        {
            gridView4.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView4.AddNewRow();
        }

        private void gridView4_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[2], pub_reg_date);
            view.SetRowCellValue(e.RowHandle, view.Columns[3], pub_rm_no);
            view.SetRowCellValue(e.RowHandle, view.Columns[4], pub_que);
        }

        private void btnSaveVisit_Click(object sender, EventArgs e)
        {
            string action = "", id = "", reg = "", rm = "", que = "";
            string tgl = "", dok_note = "", sql_price="", price_visit="";
            string sql_cnt = "", anam_cnt = "", sql_update = "", sql_head = "", head_id="";

            for (int i = 0; i < gridView4.DataRowCount; i++)
            {
                action = gridView4.GetRowCellValue(i, gridView4.Columns[0]).ToString();
                id = gridView4.GetRowCellValue(i, gridView4.Columns[1]).ToString();
                reg = gridView4.GetRowCellValue(i, gridView4.Columns[2]).ToString();
                tgl = gridView4.GetRowCellValue(i, gridView4.Columns[5]).ToString();
                rm = gridView4.GetRowCellValue(i, gridView4.Columns[3]).ToString();
                que = gridView4.GetRowCellValue(i, gridView4.Columns[4]).ToString();
                dok_note = gridView4.GetRowCellValue(i, gridView4.Columns[6]).ToString();

                if (tgl == "")
                {
                    MessageBox.Show("Tanggal harus diisi");
                }
                else
                {
                    if (action == "I")
                    {
                        string sql_seq = "", seq_val = "", sql_tmp = "";
                        sql_seq = " select CS_TREATMENT_DETAIL_SEQ.nextval seq from dual ";
                        OleDbConnection oraConnects = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOras = new OleDbDataAdapter(sql_seq, oraConnects);
                        DataTable dts = new DataTable();
                        adOras.Fill(dts);
                        seq_val = dts.Rows[0]["seq"].ToString();

                        sql_cnt = " select count(0) cnt from KLINIK.cs_visit_doc where vdoc_id = '" + id + "' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        anam_cnt = dt.Rows[0]["cnt"].ToString();

                        sql_head = " select head_id from KLINIK.cs_treatment_head where rm_no='" + pub_rm_no + "' and to_char(visit_date,'dd/MM/yyyy')='" + pub_reg_date + "' and visit_no= '" + pub_que + "' ";
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_head, oraConnect2);
                        DataTable dt2 = new DataTable();
                        adOra2.Fill(dt2);
                        if (dt2.Rows.Count > 0)
                        {
                            head_id = dt2.Rows[0]["head_id"].ToString();
                        }

                        sql_price = " select treat_item_price from KLINIK.cs_treatment_item where treat_item_id='5' ";
                        OleDbConnection oraConnectp = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOrap = new OleDbDataAdapter(sql_price, oraConnectp);
                        DataTable dtp = new DataTable();
                        adOrap.Fill(dtp);
                        if (dtp.Rows.Count > 0)
                        {
                            price_visit = dtp.Rows[0]["treat_item_price"].ToString();
                        }

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

                            try
                            {

                                trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                                command.Connection = oraConnectTrans;
                                command.Transaction = trans;

                                command.CommandText = " insert into KLINIK.cs_visit_doc (vdoc_id, rm_no, visit_dt, insp_date, visit_no, visit_note, detail_id, ins_date, ins_emp) values(CS_VISIT_DOC_SEQ.nextval, '" + rm + "', to_date('" + reg + "', 'dd/MM/yyyy'), to_date('" + tgl + "', 'dd/MM/yyyy'), '" + que + "', '" + dok_note + "', " + seq_val + ", sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                command.CommandText = " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, remarks, ins_date, ins_emp) values ( " + seq_val + ", '" + head_id + "', '5', to_date('" + tgl + "', 'dd/MM/yyyy'), 1, " + price_visit + ", null, sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                //if (status == "First Inspection")
                                //{
                                //    command.CommandText = " update cs_visit set status = 'INS', time_reservation=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where empid = '" + nik + "' and to_char(visit_date,'dd/MM/yyyy') = '" + date + "' and que01 = '" + que + "' ";
                                //    command.ExecuteNonQuery();
                                //}


                                trans.Commit();
                                //string cek = " insert into cs_anamnesa (anamnesa_id, rm_no, insp_date, blood_press, pulse, temperature, allergy, anamnesa, visit_no, disease_now, disease_then, disease_family, anamnesa_physical, anamnesa_other, infop1, infop2, infop3, infop4, infop5, ins_date, ins_emp) values(cs_anamnesa_seq.nextval, '" + rm_no + "', to_date('" + date + "', 'dd/MM/yyyy'), '" + tensi + "', '" + nadi + "', '" + suhu + "', '" + alergi + "', '" + keluhan + "', '" + que + "', '" + rnow + "', '" + rold + "', '" + rfam + "', '" + pfisik + "', '" + padd + "', '" + infop1 + "', '" + infop2 + "', '" + infop3 + "', '" + infop4 + "', '" + infop5 + "', sysdate, '" + DB.vUserId + "') ";
                                //MessageBox.Show(sql_insert);
                                //MessageBox.Show("Query Exec : " + cek);
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

                        sql_update = sql_update + Environment.NewLine + " update KLINIK.cs_visit_doc set ";
                        //sql_update = sql_update + Environment.NewLine + " insp_date = to_date('" + tgl + "','dd/MM/yyyy'), ";
                        sql_update = sql_update + Environment.NewLine + " visit_note = '" + dok_note + "', ";
                        sql_update = sql_update + Environment.NewLine + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + Environment.NewLine + " where diagnosa_id = '" + id + "' ";


                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);

                            MessageBox.Show("Data Berhasil dirubah");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
                
            }
            LoadDataVisit(rm, que, reg);
        }

        private void btnCanVisit_Click(object sender, EventArgs e)
        {
            string stat = gridView4.GetRowCellDisplayText(gridView4.FocusedRowHandle, gridView4.Columns[5]);
            if (stat == "")
            {
                gridView4.DeleteRow(gridView4.FocusedRowHandle);
            }
        }

        private void gridView4_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveVisit.Enabled = true;
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tanggal")
            {
                string tmp_reg = "", tmp_ins = "";
                string ins_dt = view.GetRowCellValue(e.RowHandle, view.Columns[5]).ToString();
                string reg_dt = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                string rm = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                string que = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();

                tmp_reg = reg_dt.Replace("-", "");
                tmp_ins = ins_dt.Replace("-", "");

                if (Convert.ToInt32(tmp_ins) < Convert.ToInt32(tmp_reg))
                {
                    MessageBox.Show("Tgl Periksa Kurang dari Tgl Registrasi");
                    //gridView2.DeleteRow(gridView2.FocusedRowHandle);
                    LoadDataVisit(rm, que, reg_dt);
                    return;
                }
                else
                {

                }
            }

            if (e.Column.Caption == "Catatan Dokter")
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

        private void gridView4_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Catatan Dokter")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDelVisit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", id = "", detail_id = "";

                id = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[1]).ToString();
                detail_id = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns[11]).ToString();

                //sql_delete = "";

                //sql_delete = sql_delete + Environment.NewLine + " delete cs_visit_doc ";
                //sql_delete = sql_delete + Environment.NewLine + " where vdoc_id = '" + id + "' ";

                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction trans = null;

                command.Connection = oraConnectTrans;
                oraConnectTrans.Open();

                try
                {
                    //OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    //OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    //oraConnect.Open();
                    //cm.ExecuteNonQuery();
                    //oraConnect.Close();
                    //cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    //gridView4.DeleteRow(gridView4.FocusedRowHandle);

                    trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                    command.Connection = oraConnectTrans;
                    command.Transaction = trans;

                    command.CommandText = " delete KLINIK.cs_visit_doc where vdoc_id = '" + id + "' ";
                    command.ExecuteNonQuery();

                    command.CommandText = " delete KLINIK.cs_treatment_detail where detail_id = '" + detail_id + "' ";
                    command.ExecuteNonQuery();

                    trans.Commit();
                    //MessageBox.Show("Query Exec : " + sql_delete);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                oraConnectTrans.Close();
            }
        }

        private void btnPrintVisit_Click(object sender, EventArgs e)
        {
            ReportVisitNote report = new ReportVisitNote(dsVisitDoc);
            report.ShowPreviewDialog();
        }

        private void LoadDataReceipt(string rmno, string que, string date)
        {
            lResepNm.Text = lInfoNama.Text;
            lResepDiag.Text = lInfoDiag.Text;
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + " select 'S' action, a.receipt_id, to_char(visit_dt,'dd/MM/yyyy') tgl_reg,  ";
            SQL = SQL + Environment.NewLine + " a.rm_no, visit_no, to_char(insp_date,'dd/MM/yyyy') tgl,  ";
            SQL = SQL + Environment.NewLine + " a.med_cd, formula, type_drink,  ";
            SQL = SQL + Environment.NewLine + " klinik.FN_CS_INIT_STOCK(a.insp_date,a.med_cd) +  ";
            SQL = SQL + Environment.NewLine + " klinik.FN_CS_TRX_IN(a.insp_date,a.med_cd) -    ";
            SQL = SQL + Environment.NewLine + " klinik.FN_CS_TRX_OUT(a.insp_date,a.med_cd) -  ";
            SQL = SQL + Environment.NewLine + " klinik.FN_CS_REQ_STOCK(a.insp_date,a.med_cd) stock,  ";
            SQL = SQL + Environment.NewLine + " med_qty, initcap(uom) uom, a.confirm, a.days, a.price, a.qty_day, a.med_cd, a.dosis  ";
            SQL = SQL + Environment.NewLine + " from KLINIK.cs_receipt a   ";
            SQL = SQL + Environment.NewLine + " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd)   ";
            SQL = SQL + Environment.NewLine + " where b.status = 'A'   ";
            SQL = SQL + Environment.NewLine + " and rm_no = '" + rmno + "'   ";
            SQL = SQL + Environment.NewLine + " and to_char(visit_dt, 'dd/MM/yyyy') = '" + date + "'   ";
            SQL = SQL + Environment.NewLine + " and visit_no = '" + que + "'  ";
            SQL = SQL + Environment.NewLine + " order by insp_date  ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl5.DataSource = null;
            gridView5.Columns.Clear();
            gridControl5.DataSource = dt;

            gridView5.OptionsView.ColumnAutoWidth = true;
            gridView5.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView5.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView5.IndicatorWidth = 40;
            //gridView5.OptionsBehavior.Editable = false;

            gridView5.Columns[0].Caption = "Action";
            gridView5.Columns[1].Caption = "ID";
            gridView5.Columns[2].Caption = "Tgl Reg";
            gridView5.Columns[3].Caption = "RM No";
            gridView5.Columns[4].Caption = "Visit No";
            gridView5.Columns[5].Caption = "Tanggal";
            gridView5.Columns[6].Caption = "Nama Obat";
            gridView5.Columns[7].Caption = "Kode Dosis";
            gridView5.Columns[8].Caption = "Info";
            gridView5.Columns[9].Caption = "Stok";
            gridView5.Columns[10].Caption = "Jumlah";
            gridView5.Columns[11].Caption = "Satuan";
            gridView5.Columns[12].Caption = "Confirm";
            gridView5.Columns[13].Caption = "Jml";
            gridView5.Columns[14].Caption = "Harga";
            gridView5.Columns[15].Caption = "Jumlah per Hari";
            gridView5.Columns[16].Caption = "Med Cd";
            gridView5.Columns[17].Caption = "Dosis";

            gridView5.Columns[17].VisibleIndex = 8;
            gridView5.Columns[13].VisibleIndex = 9;

            gridView5.Columns[5].MinWidth = 70;
            gridView5.Columns[5].MaxWidth = 70;

            gridView5.Columns[0].Visible = false;
            gridView5.Columns[1].Visible = false;
            gridView5.Columns[2].Visible = false;
            gridView5.Columns[3].Visible = false;
            gridView5.Columns[4].Visible = false;
            gridView5.Columns[10].Visible = false;
            gridView5.Columns[11].Visible = false;
            gridView5.Columns[14].Visible = false;
            gridView5.Columns[15].Visible = false;
            gridView5.Columns[16].Visible = false;

            gridView5.Columns[9].OptionsColumn.ReadOnly = true;
            gridView5.Columns[10].OptionsColumn.ReadOnly = true;
            gridView5.Columns[11].OptionsColumn.ReadOnly = true;
            gridView5.Columns[12].OptionsColumn.ReadOnly = true;

            string sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  ";
            OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
            DataTable dtf = new DataTable();
            adOraf.Fill(dtf);
            listFormula.Clear();
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
            gridView5.Columns[6].ColumnEdit = glmed;

            RepositoryItemGridLookUpEdit glfor = new RepositoryItemGridLookUpEdit();
            glfor.DataSource = listFormula2;
            glfor.ValueMember = "formulaCode";
            glfor.DisplayMember = "formulaName";

            glfor.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glfor.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glfor.ImmediatePopup = true;
            glfor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glfor.NullText = "";
            gridView5.Columns[7].ColumnEdit = glfor;

            RepositoryItemLookUpEdit medicineInfoLookup = new RepositoryItemLookUpEdit();
            medicineInfoLookup.DataSource = listMedicineInfo;
            medicineInfoLookup.ValueMember = "medicineInfoCode";
            medicineInfoLookup.DisplayMember = "medicineInfoName";

            medicineInfoLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            medicineInfoLookup.DropDownRows = listMedicineInfo.Count;
            medicineInfoLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            medicineInfoLookup.AutoSearchColumnIndex = 1;
            medicineInfoLookup.NullText = "";
            gridView5.Columns[8].ColumnEdit = medicineInfoLookup;

            //gridView5.BestFitColumns();

            if (gridView5.RowCount <= 0)
            {
                btnAddResep.Enabled = true;
                btnDelResep.Enabled = false;
                btnSaveResep.Enabled = false;
                btnCanResep.Enabled = true;
            }
            else
            {
                btnAddResep.Enabled = true;
                btnDelResep.Enabled = true;
                btnSaveResep.Enabled = true;
                btnCanResep.Enabled = true;
            }
        }

        private void btnAddResep_Click(object sender, EventArgs e)
        {
            gridView5.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView5.AddNewRow();
        }

        private void gridView5_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[2], pub_reg_date);
            view.SetRowCellValue(e.RowHandle, view.Columns[3], pub_rm_no);
            view.SetRowCellValue(e.RowHandle, view.Columns[4], pub_que);
        }

        private void btnSaveResep_Click(object sender, EventArgs e)
        {
            string kode = "", dosis = "", info = "", jumlah = "", id = "", stok = "", con = "", action = "", tgl="", rm="",que="",reg="";
            string sql_cnt = "", med_cnt = "", sql_update = "", sql_diag = "", diag_cnt = "", harga = "", hari = "", jph = "", info_dosis = "";

            for (int i = 0; i < gridView5.DataRowCount; i++)
            {
                id = gridView5.GetRowCellValue(i, gridView5.Columns[1]).ToString();
                reg = gridView5.GetRowCellValue(i, gridView5.Columns[2]).ToString();
                rm = gridView5.GetRowCellValue(i, gridView5.Columns[3]).ToString();
                que = gridView5.GetRowCellValue(i, gridView5.Columns[4]).ToString();
                tgl = gridView5.GetRowCellValue(i, gridView5.Columns[5]).ToString();
                kode = gridView5.GetRowCellValue(i, gridView5.Columns[16]).ToString();
                dosis = gridView5.GetRowCellValue(i, gridView5.Columns[7]).ToString();
                info = gridView5.GetRowCellValue(i, gridView5.Columns[8]).ToString();
                jumlah = gridView5.GetRowCellValue(i, gridView5.Columns[10]).ToString();
                stok = gridView5.GetRowCellValue(i, gridView5.Columns[9]).ToString();
                con = gridView5.GetRowCellValue(i, gridView5.Columns[12]).ToString();
                action = gridView5.GetRowCellValue(i, gridView5.Columns[0]).ToString();
                harga = gridView5.GetRowCellValue(i, gridView5.Columns[14]).ToString();
                hari = gridView5.GetRowCellValue(i, gridView5.Columns[13]).ToString();
                jph = gridView5.GetRowCellValue(i, gridView5.Columns[15]).ToString();
                info_dosis = gridView5.GetRowCellValue(i, gridView5.Columns[17]).ToString();

                if (con == "Y")
                {
                    MessageBox.Show("Data tidak bisa dirubah.");
                }
                else if (tgl == "")
                {
                    MessageBox.Show("Tanggal harus diisi.");
                }
                else if (stok == "0")
                {
                    MessageBox.Show("Stok obat tidak tersedia.");
                }
                else if (jumlah == "" || jumlah == "0")
                {
                    MessageBox.Show("Jumlah obat harus diisi.");
                }
                else if (Convert.ToInt16(jumlah) > Convert.ToInt16(stok))
                {
                    MessageBox.Show("Jumlah melebihi stok");
                }
                else if (kode == "")
                {
                    MessageBox.Show("Kode obat harus diisi.");
                }
                else if (dosis == "")
                {
                    MessageBox.Show("Kode Dosis harus diisi.");
                }
                else if (hari == "")
                {
                    MessageBox.Show("Jumlah harus diisi.");
                }
                else if (info == "")
                {
                    MessageBox.Show("Info harus diisi.");
                }
                else if (info_dosis == "")
                {
                    MessageBox.Show("Dosis harus diisi.");
                }
                else
                {
                    
                    if (action == "I")
                    {
                        sql_cnt = " select count(0) cnt from KLINIK.cs_receipt where receipt_id = '" + id + "' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        med_cnt = dt.Rows[0]["cnt"].ToString();

                        if (Convert.ToInt32(med_cnt) > 0)
                        {
                            //MessageBox.Show("Gagal Disimpan.");
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

                                command.CommandText = " insert into KLINIK.cs_receipt (receipt_id, rm_no, visit_dt, insp_date, med_cd, formula, med_qty, type_drink, confirm, price, days, qty_day, dosis, visit_no, ins_date, ins_emp) " +
                                                      " values(cs_receipt_seq.nextval, '" + rm + "', to_date('" + reg + "', 'dd/MM/yyyy'), to_date('" + tgl + "', 'dd/MM/yyyy'), '" + kode + "', '" + dosis + "', '" + jumlah + "', '" + info + "', 'N', " + harga + ", " + hari + ", " + jph + ", '" + info_dosis + "', '" + que + "', sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                //command.CommandText = " update cs_visit set status = 'MED', time_inspection=sysdate, upd_emp = '" + DB.vUserId + "', upd_date = sysdate where patient_no = '" + lMedNik.Text + "' and to_char(visit_date,'dd/MM/yyyy') = '" + lMedDate.Text + "' and que01 = '" + lMedQue.Text + "' ";
                                //command.ExecuteNonQuery();

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

                        sql_update = sql_update + " update KLINIK.cs_receipt" +
                                                  " set med_cd = '" + kode + "', formula = '" + dosis + "', med_qty = '" + jumlah + "', type_drink = '" + info + "', " +
                                                  "     price = '" + harga + "', days = '" + hari + "', qty_day = '" + jph + "', insp_date = to_date('" + tgl + "','dd/MM/yyyy'), dosis = '" + info_dosis + "', ";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where receipt_id = '" + id + "' and confirm='N' ";

                        try
                        {
                            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                            OleDbCommand cm = new OleDbCommand(sql_update, oraConnect);
                            oraConnect.Open();
                            cm.ExecuteNonQuery();
                            oraConnect.Close();
                            cm.Dispose();

                            //MessageBox.Show("Query Exec : " + sql_update);
                            LoadDataReceipt(rm, que, reg);
                            MessageBox.Show("Data Berhasil diupdate");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            LoadDataReceipt(rm, que, reg);
        }

        private void btnCanResep_Click(object sender, EventArgs e)
        {
            string stat = gridView5.GetRowCellDisplayText(gridView5.FocusedRowHandle, gridView5.Columns[6]);
            if (stat == "")
            {
                gridView5.DeleteRow(gridView5.FocusedRowHandle);
            }
        }

        private void gridView5_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveResep.Enabled = true;
            GridView view = sender as GridView;
            string a = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();

            if (e.Column.Caption == "Tanggal")
            {
                string tmp_reg = "", tmp_ins = "";
                string ins_dt = view.GetRowCellValue(e.RowHandle, view.Columns[5]).ToString();
                string reg_dt = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                string rm = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                string que = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();

                tmp_reg = reg_dt.Replace("-", "");
                tmp_ins = ins_dt.Replace("-", "");

                if (tmp_reg == "")
                {
                    tmp_reg = "0";
                }

                if (tmp_ins == "")
                {
                    tmp_ins = "0";
                }

                if (Convert.ToInt32(tmp_ins) < Convert.ToInt32(tmp_reg))
                {
                    MessageBox.Show("Tgl Periksa Kurang dari Tgl Registrasi");
                    //gridView2.DeleteRow(gridView2.FocusedRowHandle);
                    LoadDataReceipt(rm, que, reg_dt);
                    return;
                }
                else
                {
                    
                }
            }

            if (e.Column.Caption == "Nama Obat" && a.Substring(0, 3) == "MED")
            {
                
                string tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();
                string sql_medcd = "", sql_med = "", med_cd = "", med_name = "", med_group = "", med_stok = "", med_uom = "", cek_stok = "", sql_for = "";

                sql_medcd = " select " +
                            " klinik.FN_CS_INIT_STOCK(to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + a + "') +  " +
                            " klinik.FN_CS_TRX_IN(to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + a + "') -  " +
                            " klinik.FN_CS_TRX_OUT(to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + a + "') - " +
                            " klinik.FN_CS_REQ_STOCK(to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + a + "') stock from dual ";

                OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_medcd, oraConnect0);
                DataTable dt0 = new DataTable();
                adOra0.Fill(dt0);
                cek_stok = dt0.Rows[0]["stock"].ToString();

                sql_med = " select med_cd, initcap(med_name) med_name, med_group, '" + cek_stok + "' stock, initcap(uom) uom " +
                          //" stock - (select nvl(SUM(med_qty),0) from cs_receipt  " +
                          //"           where TO_CHAR(insp_date, 'dd/MM/yyyy') = '" + lMedDate.Text + "'  " +
                          //"             and confirm = 'N'  " +
                          //"             and med_cd = a.med_cd) stock, uom  " +
                          //" klinik.FN_CS_INIT_STOCK(to_date('" + lMedDate.Text + "','dd/MM/yyyy'),'"+ medcd + "') +  " +
                          //" klinik.FN_CS_TRX_IN(to_date('" + lMedDate.Text + "','dd/MM/yyyy'),'" + medcd + "') -  " +
                          //" klinik.FN_CS_TRX_OUT(to_date('" + lMedDate.Text + "','dd/MM/yyyy'),'" + medcd + "') - " +
                          //" klinik.FN_CS_REQ_STOCK(to_date('" + lMedDate.Text + "','dd/MM/yyyy'),'" + medcd + "') stock, uom " +
                          " from KLINIK.cs_medicine a  " +
                          " where status = 'A'  " +
                          " and med_cd = '" + a + "' ";

                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_med, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);

                med_cd = dt.Rows[0]["med_cd"].ToString();
                med_name = dt.Rows[0]["med_name"].ToString();
                med_group = dt.Rows[0]["med_group"].ToString();
                med_stok = dt.Rows[0]["stock"].ToString();
                med_uom = dt.Rows[0]["uom"].ToString();

                listFormula.Clear();
                listFormula2.Clear();
                sql_for = " select formula_id, initcap(formula) formula, initcap(b.med_name) med_name from KLINIK.cs_formula a join KLINIK.cs_medicine b on(a.med_cd=b.med_cd) where 1=1  and  b.med_cd = '" + med_cd + "' ";
                OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                DataTable dtf = new DataTable();
                adOraf.Fill(dtf);
                
                for (int i = 0; i < dtf.Rows.Count; i++)
                {
                    listFormula2.Add(new Formula2() { formulaCode = dtf.Rows[i]["formula_id"].ToString(), formulaName = dtf.Rows[i]["formula"].ToString(), medicineName = dtf.Rows[i]["med_name"].ToString() });
                }
                
                view.SetRowCellValue(e.RowHandle, view.Columns[10], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[7], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[13], "");
                view.SetRowCellValue(e.RowHandle, view.Columns[14], 0);
                view.SetRowCellValue(e.RowHandle, view.Columns[15], 0);
                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[16], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], "N");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[16], med_cd);
                    view.SetRowCellValue(e.RowHandle, view.Columns[8], "A");
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], med_stok);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], "0");
                    view.SetRowCellValue(e.RowHandle, view.Columns[11], med_uom);
                    view.SetRowCellValue(e.RowHandle, view.Columns[12], "N");
                }
            }

            if (e.Column.Caption == "Kode Dosis")
            {
                string medicine_cd = view.GetRowCellValue(e.RowHandle, view.Columns[16]).ToString();
                string formula_cd = view.GetRowCellValue(e.RowHandle, view.Columns[7]).ToString();
                string reg_dt = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
                string rm = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                string que = view.GetRowCellValue(e.RowHandle, view.Columns[4]).ToString();
                string stat = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();

                string kode = "", sql_pilihan = "";

                if (stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[13], "");
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], 0);
                    view.SetRowCellValue(e.RowHandle, view.Columns[15], 0);
                }
                else
                {
                    sql_pilihan = " select med_cd from KLINIK.cs_formula where formula_id = '" + formula_cd + "' ";
                    OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                    OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_pilihan, oraConnectf);
                    DataTable dtf = new DataTable();
                    adOraf.Fill(dtf);

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
                        view.SetRowCellValue(e.RowHandle, view.Columns[10], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[13], "");
                        view.SetRowCellValue(e.RowHandle, view.Columns[14], 0);
                        view.SetRowCellValue(e.RowHandle, view.Columns[15], 0);
                    }
                    else
                    {
                        MessageBox.Show("Kode Formula tidak valid");
                        return;
                        //LoadDataReceipt(rm, que, reg_dt);
                        
                    }
                }

                

            }

            if (e.Column.Caption == "Jml")
            {
                string sql_for = "", med_price = "", qty = "", tmp_stat = "";
                string for_cd = view.GetRowCellValue(e.RowHandle, view.Columns[7]).ToString();
                string tmp_hari = view.GetRowCellValue(e.RowHandle, view.Columns[13]).ToString();
                int tot_hari = 0, tot_harga = 0;

                tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[0]).ToString();

                sql_for = " select med_price, qty from KLINIK.cs_formula where formula_id = '" + for_cd + "' ";
                OleDbConnection oraConnectf = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOraf = new OleDbDataAdapter(sql_for, oraConnectf);
                DataTable dtf = new DataTable();
                adOraf.Fill(dtf);

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
                    tmp_hari = "0";
                }

                tot_hari = Convert.ToInt16(tmp_hari) * Convert.ToInt16(qty);
                tot_harga = Convert.ToInt16(tmp_hari) * Convert.ToInt16(med_price);

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[15], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], tot_hari.ToString());
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[0], "U");
                    view.SetRowCellValue(e.RowHandle, view.Columns[14], tot_harga.ToString());
                    view.SetRowCellValue(e.RowHandle, view.Columns[15], qty);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], tot_hari.ToString());
                }
            }

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Nama Obat" || e.Column.Caption == "Info")
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

        private void gridView5_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Nama Obat" || e.Column.Caption == "Kode Dosis" || e.Column.Caption == "Dosis" || e.Column.Caption == "Info" || e.Column.Caption == "Jml")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }

            if (e.Column.Caption == "Stok")
            {
                string stok = View.GetRowCellDisplayText(e.RowHandle, View.Columns[9]);

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
                string con = View.GetRowCellDisplayText(e.RowHandle, View.Columns[12]);

                if (con == "Y")
                {
                    e.Appearance.BackColor = Color.DodgerBlue;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
        }

        private void btnDelResep_Click(object sender, EventArgs e)
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

                id = gridView5.GetRowCellValue(gridView5.FocusedRowHandle, gridView5.Columns[1]).ToString();

                sql_delete = "";

                sql_delete = sql_delete + Environment.NewLine + " delete KLINIK.cs_receipt ";
                sql_delete = sql_delete + Environment.NewLine + " where receipt_id = '" + id + "' ";

                try
                {
                    OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm = new OleDbCommand(sql_delete, oraConnect);
                    oraConnect.Open();
                    cm.ExecuteNonQuery();
                    oraConnect.Close();
                    cm.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);
                    gridView5.DeleteRow(gridView5.FocusedRowHandle);
                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        private void LoadDataSKD(string rmno, string que, string date)
        {
            lSKDNm.Text = lInfoNama.Text;
            lSKDDiag.Text = lInfoDiag.Text;
            string SQL = "";
            string letter_id = "", letter_dt = "", bgn_rest = "", end_rest = "", cnt_rest = "", v_date = "";

            SQL = SQL + Environment.NewLine + " select letter_id, to_char(letter_dt,'dd/MM/yyyy') letter_dt,  ";
            SQL = SQL + Environment.NewLine + " to_char(bgn_rest, 'dd/MM/yyyy') bgn_rest,  ";
            SQL = SQL + Environment.NewLine + " to_char(end_rest, 'dd/MM/yyyy') end_rest, cnt_rest,  ";
            SQL = SQL + Environment.NewLine + " to_char(visit_dt, 'dd/MM/yyyy') visit_date  ";
            SQL = SQL + Environment.NewLine + " from KLINIK.cs_sick_leter  ";
            SQL = SQL + Environment.NewLine + " where rm_no = '" + rmno + "'  ";
            SQL = SQL + Environment.NewLine + " and to_char(visit_dt,'dd/MM/yyyy')= '" + date + "' ";
            SQL = SQL + Environment.NewLine + " and visit_no = '" + que + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(SQL, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                v_date = dt.Rows[0]["visit_date"].ToString();
                letter_id = dt.Rows[0]["letter_id"].ToString();
                letter_dt = dt.Rows[0]["letter_dt"].ToString();
                bgn_rest = dt.Rows[0]["bgn_rest"].ToString();
                end_rest = dt.Rows[0]["end_rest"].ToString();
                cnt_rest = dt.Rows[0]["cnt_rest"].ToString();

                dVisitDate.Text = v_date;
                lSkdID.Text = letter_id;
                dLetterDate.Text = letter_dt;
                dLetterStart.Text = bgn_rest;
                dLetterEnd.Text = end_rest;
                tLetterCnt.Text = cnt_rest;
            }
            else
            {
                dVisitDate.Text = "";
                lSkdID.Text = "";
                dLetterDate.Text = today;
                dLetterStart.Text = today;
                dLetterEnd.Text = "";
                tLetterCnt.Text = "";
            }

            if (lSkdID.Text == "")
            {
                skdUPrint.Enabled = false;
                skdUDel.Enabled = false;
            }
            else
            {
                skdUPrint.Enabled = true;
                skdUDel.Enabled = true;
            }
        }

        private void skdUSave_Click(object sender, EventArgs e)
        {
            string sql_cnt = "";
            string skd_cnt = "";

            if (dLetterDate.Text == "")
            {
                MessageBox.Show("Tanggal surat harus diisi");
            }
            else if (dLetterStart.Text == "")
            {
                MessageBox.Show("Tanggal mulai harus diisi");
            }
            else if (dLetterEnd.Text == "")
            {
                MessageBox.Show("Tanggal selesai harus diisi");
            }
            else if (tLetterCnt.Text == "")
            {
                MessageBox.Show("Jumlah hari harus diisi");
            }
            else
            {
                sql_cnt = " select count(0) cnt from KLINIK.cs_sick_leter where to_char(visit_dt,'dd/MM/yyyy') = '" + pub_reg_date + "' and visit_no = '" + pub_que + "' and rm_no = '" + pub_rm_no + "' ";
                OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                DataTable dt = new DataTable();
                adOra.Fill(dt);
                skd_cnt = dt.Rows[0]["cnt"].ToString();

                if (Convert.ToInt32(skd_cnt) > 0)
                {
                    // update data

                    string sql_update = "";

                    sql_update = " update KLINIK.cs_sick_leter set letter_dt = to_date('" + dLetterDate.Text + "','dd/MM/yyyy'), bgn_rest = to_date('" + dLetterStart.Text + "','dd/MM/yyyy'), end_rest = to_date('" + dLetterEnd.Text + "','dd/MM/yyyy'), cnt_rest = '" + tLetterCnt.Text + "', upd_emp='" + DB.vUserId + "', upd_date = sysdate " +
                                 " where letter_id='" + lSkdID.Text + "'  ";

                    try
                    {
                        OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm3 = new OleDbCommand(sql_update, oraConnect3);
                        oraConnect3.Open();
                        cm3.ExecuteNonQuery();
                        oraConnect3.Close();
                        cm3.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete);

                        MessageBox.Show("Data Berhasil dirubah");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
                else
                {
                    // insert data

                    string sql_insert = "";

                    sql_insert = " insert into KLINIK.cs_sick_leter (letter_id, rm_no, insp_date, visit_dt, print_yn, letter_dt, bgn_rest, end_rest, cnt_rest, visit_no, ins_date, ins_emp)  " +
                                 " values (cs_sick_seq.nextval,'" + pub_rm_no + "', to_date('" + pub_reg_date + "','dd/MM/yyyy'), to_date('" + pub_reg_date + "','dd/MM/yyyy'), 'N',to_date('" + dLetterDate.Text + "','dd/MM/yyyy'),to_date('" + dLetterStart.Text + "','dd/MM/yyyy'), to_date('" + dLetterEnd.Text + "','dd/MM/yyyy'),'" + tLetterCnt.Text + "','" + pub_que + "',sysdate,'" + DB.vUserId + "')  ";

                    try
                    {
                        OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                        OleDbCommand cm2 = new OleDbCommand(sql_insert, oraConnect2);
                        oraConnect2.Open();
                        cm2.ExecuteNonQuery();
                        oraConnect2.Close();
                        cm2.Dispose();

                        //MessageBox.Show("Query Exec : " + sql_delete);

                        MessageBox.Show("Data Berhasil ditambah");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
            }
        }

        private void getSkd()
        {
            string sql_skd = "";

            sql_skd = sql_skd + Environment.NewLine + "select a.patient_no, a.name, a.gender, round(((sysdate-birth_date)/30)/12) age, job, null position,  ";
            sql_skd = sql_skd + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP(ORDER BY type_diagnosa asc) diagnosa  ";
            sql_skd = sql_skd + Environment.NewLine + "from KLINIK.cs_diagnosa a   join KLINIK.cs_diagnosa_item b on (a.item_cd = b.item_cd)  ";
            sql_skd = sql_skd + Environment.NewLine + "where b.status = 'A'  ";
            sql_skd = sql_skd + Environment.NewLine + "and rm_no = c.rm_no  ";
            sql_skd = sql_skd + Environment.NewLine + "and insp_date = trunc(b.visit_date)  ";
            sql_skd = sql_skd + Environment.NewLine + "and visit_no = b.que01) as diagnosa, letter_no,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(visit_date, 'dd Month yyyy', 'nls_date_language = INDONESIAN') visit_date,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(letter_dt, 'dd fmMonth yyyy', 'nls_date_language = INDONESIAN') letter_dt,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(bgn_rest, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') bgn_rest,  ";
            sql_skd = sql_skd + Environment.NewLine + "TO_CHAR(end_rest, 'dd Mon yyyy', 'nls_date_language = INDONESIAN') end_rest, cnt_rest, b.purpose,  ";
            sql_skd = sql_skd + Environment.NewLine + "decode (b.purpose,'DOC','dr. ','') || (select distinct klinik.FN_GET_NAME(ins_emp) nama  ";
            sql_skd = sql_skd + Environment.NewLine + " from KLINIK.cs_diagnosa a      ";
            sql_skd = sql_skd + Environment.NewLine + " where rm_no = c.rm_no     ";
            sql_skd = sql_skd + Environment.NewLine + " and insp_date = trunc(visit_date)  ";
            sql_skd = sql_skd + Environment.NewLine + " and visit_no = que01) pic,  ";
            sql_skd = sql_skd + Environment.NewLine + "'Dokter Pemeriksa' as pic_info  ";
            sql_skd = sql_skd + Environment.NewLine + "from KLINIK.cs_patient_info a  ";
            sql_skd = sql_skd + Environment.NewLine + "join KLINIK.cs_visit b on (a.patient_no = b.patient_no)  ";
            sql_skd = sql_skd + Environment.NewLine + "join KLINIK.cs_patient c on(b.patient_no = c.patient_no)  ";
            sql_skd = sql_skd + Environment.NewLine + "join KLINIK.cs_sick_leter d on(c.rm_no = d.rm_no)  ";
            sql_skd = sql_skd + Environment.NewLine + "where b.que01 = d.visit_no  ";
            sql_skd = sql_skd + Environment.NewLine + "and trunc(b.visit_date) = d.insp_date  ";
            sql_skd = sql_skd + Environment.NewLine + "and to_char(b.visit_date, 'dd/MM/yyyy') = '" + pub_reg_date + "'  ";
            sql_skd = sql_skd + Environment.NewLine + "and c.status = 'A'   and b.que01 = '" + pub_que + "'  ";
            sql_skd = sql_skd + Environment.NewLine + "and c.group_patient = 'COMM'   and c.rm_no = '" + pub_rm_no + "' ";


            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_skd, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            dsSkd.Tables.Clear();
            dsSkd.Tables.Add(dt);
        }

        private void skdUPrint_Click(object sender, EventArgs e)
        {
            getSkd();
            ReportSkdUmum report = new ReportSkdUmum(dsSkd);
            report.ShowPreviewDialog();
        }

        private void skdUDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "";

                sql_delete = " delete from KLINIK.cs_sick_leter where letter_id = '" + lSkdID.Text + "'  ";

                try
                {
                    OleDbConnection oraConnect4 = ConnOra.Create_Connect_Ora();
                    OleDbCommand cm4 = new OleDbCommand(sql_delete, oraConnect4);
                    oraConnect4.Open();
                    cm4.ExecuteNonQuery();
                    oraConnect4.Close();
                    cm4.Dispose();

                    //MessageBox.Show("Query Exec : " + sql_delete);

                    MessageBox.Show("Data Berhasil dihapus");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }
        
        private void LoadDataLayanan(string rmno, string que, string date)
        {
            string sql_load = "";
            string s_rm = "", s_que = "", s_date = "", p_rm = "", p_que = "", p_date = "", p_name = "", p_anamnesa = "", p_diagnosa = "";

            sql_load = sql_load + Environment.NewLine + "select a.patient_no, a.name,  ";
            sql_load = sql_load + Environment.NewLine + "c.rm_no, to_char(b.visit_date,'dd/MM/yyyy') visit_date, que01,  ";
            sql_load = sql_load + Environment.NewLine + "(select LISTAGG(initcap(anamnesa), ', ') WITHIN GROUP (ORDER BY ins_emp asc) anamnesa ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_anamnesa   ";
            sql_load = sql_load + Environment.NewLine + "where rm_no=c.rm_no   ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)  ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) anamnesa,  ";
            sql_load = sql_load + Environment.NewLine + "(select LISTAGG(initcap(item_name), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa  ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_diagnosa a  ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_diagnosa_item b on (a.item_cd=b.item_cd) ";
            sql_load = sql_load + Environment.NewLine + "where b.status='A'  ";
            sql_load = sql_load + Environment.NewLine + "and rm_no=c.rm_no  ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)  ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) as diagnosa,  ";
            sql_load = sql_load + Environment.NewLine + "(select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_receipt a   ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_medicine b on (a.med_cd=b.med_cd)  ";
            sql_load = sql_load + Environment.NewLine + "where b.status='A'   ";
            sql_load = sql_load + Environment.NewLine + "and rm_no=c.rm_no   ";
            sql_load = sql_load + Environment.NewLine + "and insp_date=trunc(b.visit_date)  ";
            sql_load = sql_load + Environment.NewLine + "and visit_no=b.que01) as resep ";
            sql_load = sql_load + Environment.NewLine + "from KLINIK.cs_patient_info a   ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_visit b on (a.patient_no = b.patient_no)  ";
            sql_load = sql_load + Environment.NewLine + "join KLINIK.cs_patient c on (b.patient_no = c.patient_no)  ";
            sql_load = sql_load + Environment.NewLine + "where  to_char(b.visit_date, 'dd/MM/yyyy') = '" + pub_reg_date + "'   ";
            sql_load = sql_load + Environment.NewLine + "and c.status = 'A'  ";
            sql_load = sql_load + Environment.NewLine + "and b.que01 = '" + pub_que + "'  ";
            sql_load = sql_load + Environment.NewLine + "and c.group_patient = 'COMM'  ";
            sql_load = sql_load + Environment.NewLine + "and c.rm_no = '" + pub_rm_no + "' ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                p_rm = dt.Rows[0]["rm_no"].ToString();
                p_que = dt.Rows[0]["que01"].ToString();
                p_date = dt.Rows[0]["visit_date"].ToString();
                p_name = dt.Rows[0]["name"].ToString();
                p_anamnesa = dt.Rows[0]["anamnesa"].ToString();
                p_diagnosa = dt.Rows[0]["diagnosa"].ToString();
            }

            lTindRm.Text = p_rm;
            lTindQue.Text = p_que;
            lTindDate.Text = p_date;

            lTinName.Text = p_name;
            lTinAnam.Text = p_anamnesa;
            lTinDiag.Text = p_diagnosa;

            LoadAddTind();
        }

        private void LoadAddTind()
        {
            string sql_tind_load = "";

            sql_tind_load = sql_tind_load + Environment.NewLine + "select b.detail_id, c.treat_group_id, b.treat_item_id, b.treat_qty, b.total_price, ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "b.remarks, 'S' action, a.head_id, to_char(b.treat_date,'dd/MM/yyyy') treat_date, a.pay_status, b.treat_item_price ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "from KLINIK.cs_treatment_head a ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "join KLINIK.cs_treatment_detail b on (a.head_id=b.head_id) ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "join KLINIK.cs_treatment_item c on (b.treat_item_id=c.treat_item_id) ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "where rm_no='" + pub_rm_no + "' ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "and to_char(visit_date,'dd/MM/yyyy')='" + pub_reg_date + "' ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "and visit_no='" + pub_que + "' ";
            sql_tind_load = sql_tind_load + Environment.NewLine + "and a.status='OPN' ";

            OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_tind_load, oraConnect2);
            DataTable dt2 = new DataTable();
            adOra2.Fill(dt2);

            gridControl6.DataSource = null;
            gridView6.Columns.Clear();
            gridControl6.DataSource = dt2;

            gridView6.OptionsView.ColumnAutoWidth = true;
            gridView6.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView6.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView6.IndicatorWidth = 30;
            //gridView6.OptionsBehavior.Editable = false;
            gridView6.BestFitColumns();

            gridView6.Columns[0].Caption = "ID";
            gridView6.Columns[1].Caption = "Grup Tindakan";
            gridView6.Columns[2].Caption = "Nama Tindakan";
            gridView6.Columns[3].Caption = "Jumlah";
            gridView6.Columns[4].Caption = "Harga";
            gridView6.Columns[5].Caption = "Remark";
            gridView6.Columns[6].Caption = "Action";
            gridView6.Columns[7].Caption = "Head ID";
            gridView6.Columns[8].Caption = "Tanggal";
            gridView6.Columns[9].Caption = "Status Bayar";
            gridView6.Columns[10].Caption = "Item Price";

            gridView6.Columns[3].Width = 60;
            gridView6.Columns[4].Width = 80;

            //gridView6.Columns[9].VisibleIndex = 6;

            gridView6.Columns[0].Visible = false;
            //gridView6.Columns[5].Visible = false;
            gridView6.Columns[6].Visible = false;
            gridView6.Columns[7].Visible = false;
            //gridView6.Columns[8].Visible = false;
            gridView6.Columns[9].Visible = false;
            gridView6.Columns[10].Visible = false;
            gridView6.Columns[8].VisibleIndex = 0;

            gridView6.Columns[1].OptionsColumn.ReadOnly = true;
            //gridView6.Columns[3].OptionsColumn.ReadOnly = true;
            gridView6.Columns[4].OptionsColumn.ReadOnly = true;
            gridView6.Columns[6].OptionsColumn.ReadOnly = true;

            RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
            glLaya.DataSource = listLaya2;
            glLaya.ValueMember = "layananCode";
            glLaya.DisplayMember = "layananName";

            glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glLaya.ImmediatePopup = true;
            glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glLaya.NullText = "";
            gridView6.Columns[2].ColumnEdit = glLaya;

            RepositoryItemLookUpEdit grpLookup = new RepositoryItemLookUpEdit();
            grpLookup.DataSource = listGrpLaya;
            grpLookup.ValueMember = "statCode";
            grpLookup.DisplayMember = "statName";

            grpLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            grpLookup.DropDownRows = listGrpLaya.Count;
            grpLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            grpLookup.AutoSearchColumnIndex = 1;
            grpLookup.NullText = "";
            gridView6.Columns[1].ColumnEdit = grpLookup;

            btnAddTind.Enabled = true;

            if (gridView6.RowCount > 0)
            {
                btnAddTind.Enabled = true;
                btnSaveTind.Enabled = true;
                btnCanTind.Enabled = true;
                btnDelTind.Enabled = true;
            }
            else
            {
                btnAddTind.Enabled = true;
                btnSaveTind.Enabled = false;
                btnCanTind.Enabled = true;
                btnDelTind.Enabled = false;
            }

            
        }

        private void btnAddTind_Click(object sender, EventArgs e)
        {
            gridView6.OptionsBehavior.EditingMode = GridEditingMode.Default;
            gridView6.AddNewRow();
        }

        private void gridView6_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView view = sender as GridView;

            //view.SetRowCellValue(e.RowHandle, view.Columns[5], "A");
            //gridView6.Columns[3].OptionsColumn.ReadOnly = false;
            view.SetRowCellValue(e.RowHandle, view.Columns[6], "I");
            view.SetRowCellValue(e.RowHandle, view.Columns[8], today);
            //view.SetRowCellValue(e.RowHandle, view.Columns[6], "TRT02");
            btnAddTind.Enabled = false;
        }

        private void btnSaveTind_Click(object sender, EventArgs e)
        {
            string date = "", nama_laya = "", head = "", detail = "", ldate = "", qty = "", price = "", remarks = "", action = "", stbyr = "", item_price="";
            string sql_cnt = "", diag_cnt = "", sql_update = "";

            for (int i = 0; i < gridView6.DataRowCount; i++)
            {
                detail = gridView6.GetRowCellValue(i, gridView6.Columns[0]).ToString();
                head = gridView6.GetRowCellValue(i, gridView6.Columns[7]).ToString();
                nama_laya = gridView6.GetRowCellValue(i, gridView6.Columns[2]).ToString();
                ldate = gridView6.GetRowCellValue(i, gridView6.Columns[8]).ToString();
                qty = gridView6.GetRowCellValue(i, gridView6.Columns[3]).ToString();
                price = gridView6.GetRowCellValue(i, gridView6.Columns[4]).ToString();
                remarks = gridView6.GetRowCellValue(i, gridView6.Columns[5]).ToString();
                action = gridView6.GetRowCellValue(i, gridView6.Columns[6]).ToString();
                stbyr = gridView6.GetRowCellValue(i, gridView6.Columns[9]).ToString();
                item_price = gridView6.GetRowCellValue(i, gridView6.Columns[10]).ToString();

                if (nama_laya == "")
                {
                    MessageBox.Show("Nama Layanan harus diisi");
                }
                else if (stbyr != "OPN")
                {
                    MessageBox.Show("Data tidak bisa ditambah");
                }
                else
                {
                    if (action == "I")
                    {
                        //sql_cnt = " select count(0) cnt from cs_treatment_detail where head_id = '" + head + "' and to_char(treat_date,'dd/MM/yyyy') = '" + ldate + "' and treat_item_id = '" + nama_laya + "' ";
                        sql_cnt = " select count(0) cnt from KLINIK.cs_treatment_detail where head_id = '" + head + "' and treat_item_id = '" + nama_laya + "' and to_char(treat_date,'dd/MM/yyyy') = '" + ldate + "' ";
                        OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
                        OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cnt, oraConnect);
                        DataTable dt = new DataTable();
                        adOra.Fill(dt);
                        diag_cnt = dt.Rows[0]["cnt"].ToString();
                        if (Convert.ToInt32(diag_cnt) > 0)
                        {
                            //MessageBox.Show("Employee ID " + nik + " sudah terdaftar.");
                        }
                        else
                        {

                            string sql_seq = "", seq_val = "", sql_tmp = "";
                            sql_seq = " select CS_TREATMENT_DETAIL_SEQ.nextval seq from dual ";
                            OleDbConnection oraConnects = ConnOra.Create_Connect_Ora();
                            OleDbDataAdapter adOras = new OleDbDataAdapter(sql_seq, oraConnects);
                            DataTable dts = new DataTable();
                            adOras.Fill(dts);
                            seq_val = dts.Rows[0]["seq"].ToString();

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

                                command.CommandText = " insert into KLINIK.cs_treatment_detail (detail_id, head_id, treat_item_id, treat_date, treat_qty, treat_item_price, total_price, remarks, ins_date, ins_emp) values ( '" + seq_val + "', '" + head + "', '" + nama_laya + "', to_date('" + ldate + "', 'dd/MM/yyyy'), " + qty + ", " + item_price + ", " + price + ", '" + remarks + "', sysdate, '" + DB.vUserId + "') ";
                                command.ExecuteNonQuery();

                                command.CommandText = " insert into KLINIK.cs_action (act_id, rm_no, insp_date, visit_dt, visit_no, detail_id, ins_date, ins_emp) values ( CS_ACTION_SEQ.nextval, '" + pub_rm_no + "', to_date('" + ldate + "', 'dd/MM/yyyy'), to_date('" + pub_reg_date + "', 'dd/MM/yyyy'), '" + pub_que + "', '" + seq_val + "', sysdate, '" + DB.vUserId + "') ";
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

                        sql_update = sql_update + " update KLINIK.cs_treatment_detail" +
                                                  " set remarks = '" + remarks + "', treat_qty = '" + qty + "', total_price = " + Convert.ToInt16(qty) * Convert.ToInt16(item_price) + ",  ";
                        sql_update = sql_update + " upd_emp = '" + DB.vUserId + "', upd_date = sysdate ";
                        sql_update = sql_update + " where detail_id = '" + detail + "' ";

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
        

        private void btnCanTind_Click(object sender, EventArgs e)
        {
            string stat = gridView6.GetRowCellDisplayText(gridView6.FocusedRowHandle, gridView6.Columns[2]);
            if (stat == "")
            {
                gridView6.DeleteRow(gridView6.FocusedRowHandle);
            }
        }

        private void gridView6_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            btnSaveTind.Enabled = true;
            GridView view = sender as GridView;

            string a = "", tmp_stat = "";

            a = view.GetRowCellValue(e.RowHandle, view.Columns[2]).ToString();
            tmp_stat = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();

            if (e.Column.Caption == "Nama Tindakan" && a != "")
            {
                string sql_ = "", sql_head = "", group_id = "", price = "", head_id = "", stbyr = "";
                sql_ = " select treat_group_id, treat_item_price from KLINIK.cs_treatment_item where treat_item_id = " + a + " ";

                OleDbConnection oraConnect0 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra0 = new OleDbDataAdapter(sql_, oraConnect0);
                DataTable dt0 = new DataTable();
                adOra0.Fill(dt0);
                if (dt0.Rows.Count > 0)
                {
                    group_id = dt0.Rows[0]["treat_group_id"].ToString();
                    price = dt0.Rows[0]["treat_item_price"].ToString();
                }

                sql_head = " select head_id, pay_status from KLINIK.cs_treatment_head where rm_no = '" + pub_rm_no + "' and to_char(visit_date,'dd/MM/yyyy') = '" + pub_reg_date + "' and visit_no = '" + pub_que + "' ";

                OleDbConnection oraConnect1 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra1 = new OleDbDataAdapter(sql_head, oraConnect1);
                DataTable dt1 = new DataTable();
                adOra1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    head_id = dt1.Rows[0]["head_id"].ToString();
                    stbyr = dt1.Rows[0]["pay_status"].ToString();
                }

                if (tmp_stat == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "I");
                    view.SetRowCellValue(e.RowHandle, view.Columns[7], head_id);
                    view.SetRowCellValue(e.RowHandle, view.Columns[1], group_id);
                    //view.SetRowCellValue(e.RowHandle, view.Columns[2], a);
                    view.SetRowCellValue(e.RowHandle, view.Columns[4], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns[10], price);
                    view.SetRowCellValue(e.RowHandle, view.Columns[3], "1");
                    view.SetRowCellValue(e.RowHandle, view.Columns[9], stbyr);
                    
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "U");
                }

            }

            if (e.Column.Caption == "Jumlah")
            {
                string tmp_qty = view.GetRowCellValue(e.RowHandle, view.Columns[3]).ToString();
                string tmp_item_price = view.GetRowCellValue(e.RowHandle, view.Columns[10]).ToString();
                int tmp_price = 0;

                tmp_price = Convert.ToInt32(tmp_qty) * Convert.ToInt32(tmp_item_price);

                view.SetRowCellValue(e.RowHandle, view.Columns[4], tmp_price.ToString());
            }

            if (e.Column.Caption == "Remark" || e.Column.Caption == "Jumlah")
            {
                string tmp_stat2 = view.GetRowCellValue(e.RowHandle, view.Columns[6]).ToString();
                if (tmp_stat2 == "I")
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "I");
                }
                else
                {
                    view.SetRowCellValue(e.RowHandle, view.Columns[6], "U");
                }
            }
        }

        private void gridView6_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;

            if (e.Column.Caption == "Tanggal" || e.Column.Caption == "Nama Tindakan" || e.Column.Caption == "Remark" || e.Column.Caption == "Jumlah")
            {
                e.Appearance.BackColor = Color.OldLace;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void btnDelTind_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Anda yakin akan menghapus data?",
                      "Message",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.No)
            {

            }
            else
            {
                string sql_delete = "", id = "", payst = "";

                id = gridView6.GetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns[0]).ToString();
                payst = gridView6.GetRowCellValue(gridView6.FocusedRowHandle, gridView6.Columns[9]).ToString();

                OleDbConnection oraConnectTrans = ConnOra.Create_Connect_Ora();
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction trans = null;

                command.Connection = oraConnectTrans;
                oraConnectTrans.Open();

                try
                {
                    if (payst == "OPN")
                    {
                        trans = oraConnectTrans.BeginTransaction(IsolationLevel.ReadCommitted);
                        command.Connection = oraConnectTrans;
                        command.Transaction = trans;


                        command.CommandText = " delete KLINIK.cs_treatment_detail where detail_id = '" + id + "' ";
                        command.ExecuteNonQuery();

                        command.CommandText = " delete KLINIK.cs_action where detail_id = '" + id + "' ";
                        command.ExecuteNonQuery();

                        trans.Commit();
                        //MessageBox.Show(sql_insert);
                        //MessageBox.Show("Query Exec : " + sql_insert);
                        MessageBox.Show("Data Berhasil didelete.");
                    }
                    else
                    {
                        //
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("ERROR: " + ex.Message);
                }
                oraConnectTrans.Close();
                LoadAddTind();
            }
        }
        

        private void LoadDataMR(string rmno, string que, string date)
        {
            lMrName.Text = lInfoNama.Text;
            lMrDtIn.Text = lInfoTglMsk.Text;

            string sql_load = "", sql_mr_load = "", sql_mr_print = "";
            string s_rm = "", s_que = "", s_date = "";
            string p_name = "", p_nik = "", p_rm = "", p_address = "", p_age = "", p_gender = "";

            sql_load = " select a.name, a.patient_no, c.rm_no, a.address, gender,   " +
                       " a.birth_place || ', ' || birth_date || ' (' || round(((sysdate-birth_date)/30)/12) || ' tahun)' as ttl   " +
                       " from KLINIK.cs_patient_info a   " +
                       " join KLINIK.cs_visit b on (a.patient_no = b.patient_no)   " +
                       " join KLINIK.cs_patient c on (b.patient_no = c.patient_no)   " +
                       " where 1 = 1   " +
                       " and to_char(b.visit_date, 'dd/MM/yyyy') = '" + pub_reg_date + "'   " +
                       " and c.status = 'A'   and b.que01 = '" + pub_que + "'   " +
                       " and c.group_patient = 'COMM'   and c.rm_no = '" + pub_rm_no + "'   ";

            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_load, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                p_name = dt.Rows[0]["name"].ToString();
                p_nik = dt.Rows[0]["patient_no"].ToString();
                p_rm = dt.Rows[0]["rm_no"].ToString();
                p_address = dt.Rows[0]["address"].ToString();
                p_age = dt.Rows[0]["ttl"].ToString();
                p_gender = dt.Rows[0]["gender"].ToString();
            }
            else
            {
                p_name = "";
                p_nik = "";
                p_rm = "";
                p_address = "";
                p_age = "";
                p_gender = "";
            }

            if (cmbReport.Text == "Laporan MR")
            {
                sql_mr_load = "";
                sql_mr_load = sql_mr_load + Environment.NewLine + "select poli_cd, nvl(ddate,to_char(insp_date,'dd/MM/yyyy')) ddate, anamnesa, diagnosa,  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "terapi || ' ' || klinik.FN_GET_RESEP_OUT(rm_no,visit_no,insp_date) as terapi, pic ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "from ( ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + "select a.rm_no,visit_no, insp_date, to_char(b.insp_date,'dd/MM/yyyy') ddate, ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + "'Tensi : ' || blood_press || ', Nadi : ' || pulse ||   ', Suhu : ' || temperature || ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + "', Alergi : ' || allergy ||   ', Keluhan : ' || anamnesa as anamnesa, ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "select a.rm_no,visit_no, insp_date,  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "(select distinct poli_name from KLINIK.cs_visit aa  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "join KLINIK.cs_policlinic bb on (aa.poli_cd=bb.poli_cd)  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "where trunc(visit_date)=b.insp_date  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "and visit_no=que01  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "and aa.patient_no=a.patient_no) poli_cd,    ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "(select to_char(visit_date,'dd/MM/yyyy hh24:mi:ss') ddate  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "from KLINIK.cs_visit aa ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "join KLINIK.cs_patient bb ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "on aa.patient_no=bb.patient_no ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "where bb.status='A' ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "and to_char(visit_date,'dd/MM/yyyy')=to_char(b.insp_date,'dd/MM/yyyy') ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "and que01=b.visit_no ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "and rm_no=a.rm_no) ddate, ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Tensi : ' || blood_press || ',' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Nadi : ' || pulse || ',' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Suhu : ' || temperature || ',' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'BB : ' || bb || ',' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'TB : ' || tb || ',' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Alergi : ' || allergy || ',' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Keluhan : ' || anamnesa || ',' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'R.Sekarang : ' || disease_now || ',' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'R.Dulu : ' || disease_then || ',' ||  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'R.Kel : ' || disease_family || ',' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Fisik : ' || anamnesa_physical || ',' ||  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Lain : ' || anamnesa_other  as anamnesa, ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "(select LISTAGG(item_name || decode(remark,null,null, ' (' || remark || ')'), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " from KLINIK.cs_diagnosa a   join KLINIK.cs_diagnosa_item b on (a.item_cd = b.item_cd) ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " where b.status = 'A' ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and rm_no = b.rm_no ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no) diagnosa, ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + "(select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + " from cs_receipt a  ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + " join cs_medicine b on (a.med_cd = b.med_cd) ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + " where b.status = 'A' ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + " and rm_no = b.rm_no  ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date ";
                //sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no) terapi, ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Obat : ' || (select LISTAGG(initcap(med_name)||'.'||formula||'.'||med_qty, ', ') WITHIN GROUP (ORDER BY med_name asc) resep ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " from KLINIK.cs_receipt a  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd) ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " where b.status = 'A' ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and rm_no = b.rm_no  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no) || ', ' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'SKD : ' || (select nvl(cnt_rest,end_rest - (bgn_rest -1)) skd_cnt ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " from KLINIK.cs_sick_leter a ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' || ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'OBS : ' || (select hrs_cnt hrs_cnt ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " from KLINIK.cs_observation a ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Tindakan : ' || (select act_name ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " from KLINIK.cs_action a ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Rujukan : ' || (select hos_name || ' / ' || hos_doc ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " from KLINIK.cs_refer a ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "'Rekomendasi : ' || (select recom_remark ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " from KLINIK.cs_recommendation a ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " where rm_no = b.rm_no  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and insp_date = b.insp_date  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + " and visit_no = b.visit_no )  terapi,  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "klinik.FN_GET_PIC(b.rm_no, insp_date, visit_no) pic  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "from KLINIK.cs_patient a ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "join KLINIK.cs_anamnesa b on (a.rm_no = b.rm_no) ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "where a.status = 'A' ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "and group_patient = 'COMM' ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "and b.rm_no = '" + pub_rm_no + "') ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "where 1=1 ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "order by ddate desc ";


                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_mr_load, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);

                gridControl7.DataSource = null;
                gridView7.Columns.Clear();
                gridControl7.DataSource = dt2;

                gridView7.OptionsView.ColumnAutoWidth = true;
                gridView7.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView7.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView7.IndicatorWidth = 30;
                gridView7.OptionsBehavior.Editable = false;
                //gridView7.BestFitColumns();
                gridView7.OptionsView.RowAutoHeight = true;

                RepositoryItemMemoEdit tgl = new RepositoryItemMemoEdit();
                gridControl7.RepositoryItems.Add(tgl);
                gridView7.Columns[1].ColumnEdit = tgl;

                RepositoryItemMemoEdit anam = new RepositoryItemMemoEdit();
                gridControl7.RepositoryItems.Add(anam);
                gridView7.Columns[2].ColumnEdit = anam;

                RepositoryItemMemoEdit diag = new RepositoryItemMemoEdit();
                gridControl7.RepositoryItems.Add(diag);
                gridView7.Columns[3].ColumnEdit = diag;

                RepositoryItemMemoEdit tera = new RepositoryItemMemoEdit();
                gridControl7.RepositoryItems.Add(tera);
                gridView7.Columns[4].ColumnEdit = tera;

                RepositoryItemMemoEdit pic = new RepositoryItemMemoEdit();
                gridControl7.RepositoryItems.Add(pic);
                gridView7.Columns[5].ColumnEdit = pic;

                gridView7.Columns[0].Caption = "Poli";
                gridView7.Columns[1].Caption = "Tanggal";
                gridView7.Columns[2].Caption = "Anamnesa";
                gridView7.Columns[3].Caption = "Diagnosa";
                gridView7.Columns[4].Caption = "Terapi";
                gridView7.Columns[5].Caption = "Pemeriksa";

                gridView7.BestFitColumns();
                gridView7.Columns[0].Width = 60;
                gridView7.Columns[1].Width = 80;
                gridView7.Columns[3].Width = 100;
                gridView7.Columns[4].Width = 120;
                gridView7.Columns[5].Width = 80;
                
                LoadDataMRPrint(pub_rm_no, p_name, p_nik, p_address, p_age, p_gender);
            }
            else
            {
                sql_mr_load = "";
                sql_mr_load = sql_mr_load + Environment.NewLine + "select 1 ord, 'Anamnesa' info,CS_DETAIL_INS_VALUE('ANAMNESA','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "select 2 ord, 'Alergi' info,CS_DETAIL_INS_VALUE('ALERGI','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "select 3 ord, 'Riwayat' info,CS_DETAIL_INS_VALUE('RIWAYAT','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "select 4 ord, 'Pemeriksaan Penunjuang' info,CS_DETAIL_INS_VALUE('PENUNJANG','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "select 5 ord, 'Diagnosa' info,CS_DETAIL_INS_VALUE('DIAGNOSA','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "select 6 ord, 'Kondisi Umum' info,CS_DETAIL_INS_VALUE('KONDISI','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "union  ";
                sql_mr_load = sql_mr_load + Environment.NewLine + "select 7 ord, 'Pengobatan' info,CS_DETAIL_INS_VALUE('OBAT','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";


                OleDbConnection oraConnect2 = ConnOra.Create_Connect_Ora();
                OleDbDataAdapter adOra2 = new OleDbDataAdapter(sql_mr_load, oraConnect2);
                DataTable dt2 = new DataTable();
                adOra2.Fill(dt2);

                gridControl7.DataSource = null;
                gridView7.Columns.Clear();
                gridControl7.DataSource = dt2;

                gridView7.OptionsView.ColumnAutoWidth = true;
                gridView7.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
                gridView7.Appearance.HeaderPanel.FontSizeDelta = 0;
                gridView7.IndicatorWidth = 30;
                gridView7.OptionsBehavior.Editable = false;
                //gridView7.BestFitColumns();
                gridView7.OptionsView.RowAutoHeight = true;

                RepositoryItemMemoEdit tgl = new RepositoryItemMemoEdit();
                gridControl7.RepositoryItems.Add(tgl);
                gridView7.Columns[2].ColumnEdit = tgl;

                gridView7.Columns[0].Caption = "No";
                gridView7.Columns[1].Caption = "Perihal";
                gridView7.Columns[2].Caption = "Keterangan";

                gridView7.Columns[0].Visible = false;

                gridView7.BestFitColumns();
                gridView7.Columns[1].Width = 100;

                LoadDataRanapPrint(pub_rm_no, pub_que, pub_reg_date, p_name, p_age, pub_room);
            }

            

            if (gridView7.RowCount > 0)
            {
                btnReportMr.Enabled = true;
            }
            else
            {
                btnReportMr.Enabled = false;
            }
        }

        private void cmbReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataMR(pub_rm_no, pub_que, pub_reg_date);
        }

        private void LoadDataMRPrint(string p_rm, string p_name, string p_nik, string p_address, string p_age, string p_gender)
        {
            string sql_mr_print = "";

            sql_mr_print = "";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select '" + p_name + "' name, '" + p_nik + "' nik, '" + p_rm + "' rm, '" + p_address + "' addr, '" + p_age + "' age, '" + p_gender + "' gender, ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "poli_cd, ddate, anamnesa, diagnosa,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "terapi || ' ' || klinik.FN_GET_RESEP_OUT(rm_no,visit_no,insp_date) as terapi, pic ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "from ( ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "select a.rm_no,visit_no, insp_date, to_char(b.insp_date,'dd/MM/yyyy') ddate, ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "'Tensi : ' || blood_press || ', Nadi : ' || pulse ||   ', Suhu : ' || temperature || ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "', Alergi : ' || allergy ||   ', Keluhan : ' || anamnesa as anamnesa, ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select a.rm_no,visit_no, insp_date,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "(select distinct poli_name from KLINIK.cs_visit aa  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join KLINIK.cs_policlinic bb on (aa.poli_cd=bb.poli_cd)  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where trunc(visit_date)=b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and visit_no=que01  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and aa.patient_no=a.patient_no) poli_cd,    ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "(select to_char(visit_date,'dd/MM/yyyy hh24:mi:ss') ddate  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "from KLINIK.cs_visit aa ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join KLINIK.cs_patient bb ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "on aa.patient_no=bb.patient_no ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where bb.status='A' ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and que01=b.visit_no ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and to_char(visit_date,'dd/MM/yyyy')=to_char(b.insp_date,'dd/MM/yyyy') ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and rm_no=a.rm_no) ddate, ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Tensi : ' || blood_press || ',' ||   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Nadi : ' || pulse || ',' ||   ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Suhu : ' || temperature || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'BB : ' || bb || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'TB : ' || tb || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Alergi : ' || allergy || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Keluhan : ' || anamnesa || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'R.Sekarang : ' || disease_now || ',' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'R.Dulu : ' || disease_then || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'R.Kel : ' || disease_family || ',' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Fisik : ' || anamnesa_physical || ',' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Lain : ' || anamnesa_other  as anamnesa, ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "(select LISTAGG(item_name, ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "(select LISTAGG(item_name || decode(remark,null,null, ' (' || remark || ')'), ', ') WITHIN GROUP (ORDER BY type_diagnosa asc) diagnosa ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from KLINIK.cs_diagnosa a   join KLINIK.cs_diagnosa_item b on (a.item_cd = b.item_cd) ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where b.status = 'A' ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and rm_no = b.rm_no ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no) diagnosa, ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + "(select LISTAGG(initcap(med_name), ', ') WITHIN GROUP (ORDER BY med_name asc) resep ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " from cs_receipt a  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " join cs_medicine b on (a.med_cd = b.med_cd) ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " where b.status = 'A' ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and rm_no = b.rm_no  ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date ";
            //sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no) terapi, ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Obat : ' || (select LISTAGG(initcap(med_name)||'.'||formula||'.'||med_qty, ', ') WITHIN GROUP (ORDER BY med_name asc) resep ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from KLINIK.cs_receipt a  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " join KLINIK.cs_medicine b on (a.med_cd = b.med_cd) ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where b.status = 'A' ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no) || ', ' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'SKD : ' || (select nvl(cnt_rest,end_rest - (bgn_rest -1)) skd_cnt ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from KLINIK.cs_sick_leter a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' || ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'OBS : ' || (select hrs_cnt hrs_cnt ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from KLINIK.cs_observation a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Tindakan : ' || (select act_name ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from KLINIK.cs_action a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Rujukan : ' || (select hos_name || ' / ' || hos_doc ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from KLINIK.cs_refer a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no ) || ', ' ||  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "'Rekomendasi : ' || (select recom_remark ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " from KLINIK.cs_recommendation a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " where rm_no = b.rm_no  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and insp_date = b.insp_date  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + " and visit_no = b.visit_no )  terapi,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "klinik.FN_GET_PIC(b.rm_no, insp_date, visit_no) pic  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "from KLINIK.cs_patient a ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "join KLINIK.cs_anamnesa b on (a.rm_no = b.rm_no) ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where a.status = 'A' ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and group_patient = 'COMM' ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "and b.rm_no = '" + p_rm + "') ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "where 1=1 ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "order by ddate desc  ";

            OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_mr_print, oraConnect3);
            DataTable dt3 = new DataTable();
            adOra3.Fill(dt3);

            dsMRUmum.Tables.Clear();
            dsMRUmum.Tables.Add(dt3);
        }


        private void LoadDataRanapPrint(string p_rm, string p_no, string p_vdate, string p_name, string p_age, string p_room)
        {
            string sql_mr_print = "";

            sql_mr_print = "";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select '" + p_rm + "' rm, '" + p_name + "' nama, '" + p_age + "' age, '" + p_room + "' room,  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "ord, info, val from  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "( ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 1 ord, 'Anamnesa' info,CS_DETAIL_INS_VALUE('ANAMNESA','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 2 ord, 'Alergi' info,CS_DETAIL_INS_VALUE('ALERGI','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 3 ord, 'Riwayat' info,CS_DETAIL_INS_VALUE('RIWAYAT','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 4 ord, 'Pemeriksaan Penunjuang' info,CS_DETAIL_INS_VALUE('PENUNJANG','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 5 ord, 'Diagnosa' info,CS_DETAIL_INS_VALUE('DIAGNOSA','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 6 ord, 'Kondisi Umum' info,CS_DETAIL_INS_VALUE('KONDISI','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "union  ";
            sql_mr_print = sql_mr_print + Environment.NewLine + "select 7 ord, 'Pengobatan' info,CS_DETAIL_INS_VALUE('OBAT','" + pub_rm_no + "',to_date('" + pub_reg_date + "','dd/MM/yyyy'),'" + pub_que + "') as val from dual ";
            sql_mr_print = sql_mr_print + Environment.NewLine + ") ";

            OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra3 = new OleDbDataAdapter(sql_mr_print, oraConnect3);
            DataTable dt3 = new DataTable();
            adOra3.Fill(dt3);

            dsMRRanap.Tables.Clear();
            dsMRRanap.Tables.Add(dt3);
        }

        private void btnReportMr_Click(object sender, EventArgs e)
        {
            if (cmbReport.Text == "Laporan MR")
            {
                ReportMRUmum report = new ReportMRUmum(dsMRUmum);
                report.ShowPreviewDialog();
            }
            else
            {
                ReportMRRanap report = new ReportMRRanap(dsMRRanap);
                report.ShowPreviewDialog();
            }
            
        }

        private void LoadDataTindakan(string p_rm, string p_que, string p_date)
        {
            lActNm.Text = lInfoNama.Text;
            lActDiag.Text = lInfoDiag.Text;
            string SQL = "";

            SQL = "";
            SQL = SQL + Environment.NewLine + " select act_id, to_char(a.insp_date,'dd/MM/yyyy') insp_date, treat_item_id  ";
            SQL = SQL + Environment.NewLine + " from KLINIK.cs_action a ";
            SQL = SQL + Environment.NewLine + " join KLINIK.cs_treatment_detail b on (a.detail_id=b.detail_id) ";
            SQL = SQL + Environment.NewLine + " where 1=1   ";
            SQL = SQL + Environment.NewLine + " and rm_no = '" + p_rm + "'   ";
            SQL = SQL + Environment.NewLine + " and to_char(a.visit_dt, 'dd/MM/yyyy') = '" + p_date + "'   ";
            SQL = SQL + Environment.NewLine + " and visit_no = '" + p_que + "'  ";
            SQL = SQL + Environment.NewLine + " order by a.insp_date  ";

            OleDbConnection sqlConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adSql = new OleDbDataAdapter(SQL, sqlConnect);
            DataTable dt = new DataTable();
            adSql.Fill(dt);

            gridControl8.DataSource = null;
            gridView8.Columns.Clear();
            gridControl8.DataSource = dt;

            gridView8.OptionsView.ColumnAutoWidth = true;
            gridView8.Appearance.HeaderPanel.FontStyleDelta = System.Drawing.FontStyle.Bold;
            gridView8.Appearance.HeaderPanel.FontSizeDelta = 0;
            gridView8.IndicatorWidth = 40;
            gridView8.OptionsBehavior.Editable = false;

            gridView8.Columns[0].Caption = "ID";
            gridView8.Columns[1].Caption = "Tanggal";
            gridView8.Columns[2].Caption = "Nama Tindakan";

            RepositoryItemGridLookUpEdit glLaya = new RepositoryItemGridLookUpEdit();
            glLaya.DataSource = listLaya3;
            glLaya.ValueMember = "layananCode";
            glLaya.DisplayMember = "layananName";

            glLaya.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            glLaya.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            glLaya.ImmediatePopup = true;
            glLaya.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            glLaya.NullText = "";
            gridView8.Columns[2].ColumnEdit = glLaya;

            gridView8.Columns[0].Visible = false;
            gridView8.Columns[1].MinWidth = 70;
            gridView8.Columns[1].MaxWidth = 70;

            mActHasil.Text = "";
            mActRmk.Text = "";

            if (gridView8.RowCount > 0)
            {
                btnActSave.Enabled = true;
            }
            else
            {
                btnActSave.Enabled = false;
            }
        }

        private void gridView8_RowClick(object sender, RowClickEventArgs e)
        {
            GridView View = sender as GridView;


            string sql_cek="", s_id = "", hasil = "", rekom ="", id = "";

            s_id = View.GetRowCellDisplayText(e.RowHandle, View.Columns[0]);

            sql_cek = " select act_id, act_name, act_remark from KLINIK.cs_action where act_id = '" + s_id + "' ";
            OleDbConnection oraConnect = ConnOra.Create_Connect_Ora();
            OleDbDataAdapter adOra = new OleDbDataAdapter(sql_cek, oraConnect);
            DataTable dt = new DataTable();
            adOra.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                hasil = dt.Rows[0]["act_name"].ToString();
                rekom = dt.Rows[0]["act_remark"].ToString();
                id = dt.Rows[0]["act_id"].ToString();

                mActHasil.Text = hasil;
                mActRmk.Text = rekom;
                lActID.Text = id;
            }
            else
            {
                mActHasil.Text = "";
                mActRmk.Text = "";
                lActID.Text = "";
            }
        }

        private void btnActSave_Click(object sender, EventArgs e)
        {
            string sql_update = "";

            sql_update = " update KLINIK.cs_action set act_name = '" + mActHasil.Text + "', act_remark = '" + mActRmk.Text + "', upd_emp='" + DB.vUserId + "', upd_date = sysdate " +
                         " where act_id='" + lActID.Text + "'  ";

            try
            {
                OleDbConnection oraConnect3 = ConnOra.Create_Connect_Ora();
                OleDbCommand cm3 = new OleDbCommand(sql_update, oraConnect3);
                oraConnect3.Open();
                cm3.ExecuteNonQuery();
                oraConnect3.Close();
                cm3.Dispose();

                //MessageBox.Show("Query Exec : " + sql_delete);

                MessageBox.Show("Data Berhasil disimpan");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void mActHasil_TextChanged(object sender, EventArgs e)
        {
            lRemainAct.Text = (200 - mActHasil.Text.Length).ToString();
            if (mActHasil.Text.Length > 200 || mActRmk.Text.Length > 200)
            {
                btnActSave.Enabled = false;
            }
            else
            {
                btnActSave.Enabled = true;
            }
        }

        private void mActRmk_TextChanged(object sender, EventArgs e)
        {
            lRemainAct2.Text = (200 - mActRmk.Text.Length).ToString();
            if (mActHasil.Text.Length > 200 || mActRmk.Text.Length > 200)
            {
                btnActSave.Enabled = false;
            }
            else
            {
                btnActSave.Enabled = true;
            }
        }
    }
}